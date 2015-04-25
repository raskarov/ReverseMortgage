using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class CompanyFeeSettings : AppControl
    {
        private const string PRODUCTFILTER = "Selected={0}";
        private const string PRODUCTID = "servicefeeproductid";
        private const string SETDEFAULTCOMMANDNAME = "SetDefault";
        private const string SELECTEDPRODUCT = "1";
        private int lenderId;
        private ServiceFee serviceFee;
        private int ProductId
        {
            get 
            {
                Object o = Session[PRODUCTID];
                int res = 0;
                if (o != null)
                { 
                    int.TryParse(o.ToString(),out res);
                }
                return res;
            }
            set
            {
                Session[PRODUCTID] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(CurrentUser.IsCorrespondentLenderAdmin || CurrentUser.LoggedAsOriginator))
            {
                Response.Redirect(ResolveUrl("~/" + CurrentUser.GetDefaultPage()));
            }
            lenderId = CurrentPage.GetValueInt(Constants.IDPARAM, 0);
            if (!IsPostBack)
            {
                BindData();
            }
        }
        private void BindData()
        {
            BindProduct();
            BindGrid();
        }
        private void BindGrid()
        {
            G.DataSource = ServiceFee.GetServiceFeeForProduct(lenderId, ProductId);
            G.DataBind();
        }
        private void goBack()
        {
            Response.Redirect("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.VIEWMYLENDERS);
        }
        private void BindProduct()
        {
            DataView dv = Lender.GetLenderProductList(lenderId);
            dv.RowFilter = String.Format(PRODUCTFILTER, SELECTEDPRODUCT);
            if (dv.Count == 0)
            {
                goBack();
            }
            ProductId = int.Parse(dv[0]["id"].ToString());
            if (dv.Count > 1)
            {
                ddlProduct.DataSource = dv;
                ddlProduct.DataValueField = "id";
                ddlProduct.DataTextField = "name";
                ddlProduct.DataBind();
            }
            else
            {
                trProduct.Visible = false;
            }
        }
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == SETDEFAULTCOMMANDNAME)
            {
                serviceFee = new ServiceFee(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString()));
                serviceFee.IsDefault = true;
                int res = serviceFee.Save();
                if (res > 0)
                {
                    serviceFee.ID = res;
                }
            }
            BindGrid();
        }
        protected void G_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                ImageButton btn = e.Item.FindControl("btnSetDefault") as ImageButton;
                if (btn != null)
                {
                    DataRowView dr = e.Item.DataItem as DataRowView;
                    if (dr != null)
                    {
                        btn.Visible = Convert.ToBoolean(dr["IsDefault"].ToString()) ? false : true;
                    }
                }
            }
        }
        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductId = int.Parse(ddlProduct.SelectedValue);
            BindGrid();
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            goBack();
        }
        protected void btnLoad_Click(object sender, EventArgs e)
        {
            ServiceFee.RestoreGlobalServiceFeeForProduct(lenderId, ProductId);
            BindGrid();
        }

    }
}