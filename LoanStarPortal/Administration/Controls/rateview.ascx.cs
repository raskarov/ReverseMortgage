using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class rateview : AppControl
    {
        private const string sProductID = "ProductID";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindData();
        }

        private void BindData()
        {
            Product product = CurrentPage.GetObject(Constants.PRODUCTOBJECT) as Product;
            if (product == null)
            {
                panelFilter.Visible = true;
                ddlProducts.DataSource = Product.GetProductList(false);
                ddlProducts.DataBind();
                ddlProducts.Items.Insert(0, new ListItem("-All-", "-1"));
                ddlProducts.SelectedIndex = 0;
                ViewState[sProductID] = -1;
            }
            else
            {
                ViewState[sProductID] = product.ID;
                lblProductName.Text = product.Name;
            }

            G.Rebind();
        }

        protected void G_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            G.DataSource = Product.GetProductRateList(Convert.ToInt32(ViewState[sProductID]));
        }

        protected void ddlProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState[sProductID] = Convert.ToInt32((ddlProducts.SelectedIndex == -1) ? "-1" : ddlProducts.SelectedValue);
            G.Rebind();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            goBack();
        }

        private void goBack()
        {
            Response.Redirect("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.EDITPRODUCT);
        }

        protected void G_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                Label lbl = (Label)item.FindControl("lblPublishedRate");
                if (lbl != null)
                {
                    Product product = CurrentPage.GetObject(Constants.PRODUCTOBJECT) as Product;
                    if (product != null)
                    {
                        decimal i = Convert.ToDecimal((((DataRowView)(e.Item.DataItem))).Row["PublishedIndex"].ToString());
                        if (product.TypeId == Constants.PRODUCTTYPEHOMEKEEPER)
                        {
                            //lbl.Text = String.Format("{0:#,###.000}", (((DataRowView)(e.Item.DataItem))).Row["PublishedIndex"].ToString());
                            lbl.Text = i.ToString("#,###.000");
                        }
                        else
                            //lbl.Text = String.Format("{0:#,###.00}", (((DataRowView)(e.Item.DataItem))).Row["PublishedIndex"].ToString());
                            lbl.Text = i.ToString("#,###.00");
                            

                    }

                }
            }
        }

    }
}