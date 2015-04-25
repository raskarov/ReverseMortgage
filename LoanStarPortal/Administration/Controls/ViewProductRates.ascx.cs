using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class ViewProductRates : AppControl
    {
        private Product product;
        private bool isManualInput = false;
        private const string ROOTELEMENT = "root";
        private const string PRODUCTIDATTRIBUTE = "productid";
        private const string ITEMELEMENT = "item";

        private void BindData()
        {
            if(isManualInput)
            {
                Gm.Rebind();
            }
            else
            {
                G.Rebind();
            }
            
        }
        protected void G_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            G.DataSource = Product.GetRateListForProduct(product.ID);
        }
        protected void Gm_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            Gm.DataSource = Product.GetRateListForProduct(product.ID);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            product = CurrentPage.GetObject(Constants.PRODUCTOBJECT) as Product;
            isManualInput =
                !(product.ProductRateUpdateIntervalId == (int) Product.ProductRateUpdateInterval.WeeklyOnTuesday &&
                  product.ProductRateInputMethodId == (int) Product.ProductRateInputMethod.CMT_1Yr_10Yr);
            gAUtomatic.Visible = !isManualInput;
            trCopyRate.Visible = isManualInput;
            if (trCopyRate.Visible)
            {
                trCopyRate.Visible = BindCopyControls();
            }
            gManual.Visible = isManualInput;
            if (!IsPostBack)
            {
                lblHeader.Text = "Index rates lis for " + product.Name;
                BindData();
            }
        }
        private bool BindCopyControls()
        {
            bool res;
            DataView dv = product.GetProductsListToCopyRates();
            res = dv.Count > 0;
            if(res)
            {
                dlProducts.DataSource = dv;
                dlProducts.DataBind();
            }
            rdFrom.SelectedDate = DateTime.Now;
            rdTo.SelectedDate = DateTime.Now;
            return res;
        }
        protected static string GetDayOfWeek(Object o)
        {
            string res = String.Empty;
            DataRowView row = o as DataRowView;
            if(row!=null)
            {
                res = DateTime.Parse(row["period"].ToString()).DayOfWeek.ToString();
            }
            return res;
        }
        protected static string GetFloatValue(Object o, string fieldName, bool fridayOnly, string format)
        {
            string res = String.Empty;
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                bool showValue = !fridayOnly;
                if(!showValue)
                {
                    showValue = DateTime.Parse(row["period"].ToString()).DayOfWeek == DayOfWeek.Friday;
                }
                if (showValue)
                {
                    if (row[fieldName] != DBNull.Value)
                    {
                        double d = double.Parse(row[fieldName].ToString());
                        if(String.IsNullOrEmpty(format))
                        {
                            res = d.ToString();
                        }
                        else
                        {
                            res = String.Format(format, d);
                        }
                    }
                }
            }
            return res;
        }
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.UpdateCommandName)
            {
                int id = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString());
                RadNumericTextBox tbi = (RadNumericTextBox)e.Item.FindControl("tbInitialIndex");
                RadNumericTextBox tbe = (RadNumericTextBox)e.Item.FindControl("tbExpectedIndex");
                RadNumericTextBox tbpi = (RadNumericTextBox)e.Item.FindControl("tbPublishedIndex");
                RadNumericTextBox tbpm = (RadNumericTextBox)e.Item.FindControl("tbPublishedMargin");
                RadNumericTextBox tbpie = (RadNumericTextBox)e.Item.FindControl("tbPublishedExpectedIndex");
                RadNumericTextBox tbpme = (RadNumericTextBox)e.Item.FindControl("tbPublishedExpectedMargin");

                if ((tbi != null) && (tbe != null) && (tbpi != null) && (tbpm != null) && (tbpie != null) && (tbpme != null))
                {
                    Product.UpdateRates(id, tbi.Value, tbe.Value,tbpi.Value, tbpm.Value,tbpie.Value,tbpme.Value);
                    G.Rebind();
                }
            }
        }
        protected void Gm_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.UpdateCommandName || e.CommandName== RadGrid.PerformInsertCommandName)
            {
                int id = -1;
                if(e.CommandName == RadGrid.UpdateCommandName)
                {
                    id = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString());
                }

                RadNumericTextBox tbpii = (RadNumericTextBox)e.Item.FindControl("tbPublishedIndex");
                RadNumericTextBox tbpim = (RadNumericTextBox)e.Item.FindControl("tbPublishedMargin");
                RadNumericTextBox tbpei = (RadNumericTextBox)e.Item.FindControl("tbPublishedExpectedIndex");
                RadNumericTextBox tbpem = (RadNumericTextBox)e.Item.FindControl("tbPublishedExpectedMargin");
                RadDateInput rd = (RadDateInput)e.Item.FindControl("rdPeriod");
                if ((tbpii != null)&&(tbpim!=null)&&(tbpei!=null)&&(tbpem!=null)&&rd!=null)
                {
                    double pii = tbpii.Value??0;
                    double pim = tbpim.Value ?? 0;
                    double pei = tbpei.Value ?? 0;
                    pei = pei == 0 ? pii : pei;
                    double pem = tbpem.Value ?? 0; 
                    pem = pem == 0 ? pim : pem;
                    DateTime dt = Utils.RemoveTime(rd.SelectedDate ?? DateTime.Now);
                    product.SaveRates(id, dt, pii, pim, pei, pem);
                    Gm.CurrentPageIndex = 0;
                    Gm.Rebind();
                }
            }
            else if(e.CommandName=="Delete")
            {
                int id = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString());
                Product.DeleteRates(id);
                Gm.Rebind();
            }
        }
        protected void Gm_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                if (editedItem != null)
                {
                    DataRowView dr = editedItem.DataItem as DataRowView;

                    DateTime dt = DateTime.Now;
                    double pii = 0;
                    double pim = 0;
                    double pei = 0;
                    double pem = 0;
                    if (dr != null)
                    {
                        dt = DateTime.Parse(dr["Period"].ToString());
                        pii = float.Parse(dr["PublishedInitialIndex"].ToString());
                        pim = float.Parse(dr["PublishedInitialMargin"].ToString());
                        pei = float.Parse(dr["PublishedExpectedIndex"].ToString());
                        pem = float.Parse(dr["PublishedExpectedMargin"].ToString());

                    }
                    RadDateInput dp = editedItem.FindControl("rdPeriod") as RadDateInput;
                    if (dp != null)
                    {
                        dp.SelectedDate = dt;
                    }
                    RadNumericTextBox tbpii = editedItem.FindControl("tbPublishedIndex") as RadNumericTextBox;
                    if(tbpii!=null)
                    {
                        tbpii.Value = pii;
                    }
                    RadNumericTextBox tbpim = editedItem.FindControl("tbPublishedMargin") as RadNumericTextBox;
                    if (tbpim != null)
                    {
                        tbpim.Value = pim;
                    }

                    RadNumericTextBox tbpei = editedItem.FindControl("tbPublishedExpectedIndex") as RadNumericTextBox;
                    if (tbpei != null)
                    {
                        tbpei.Value = pei;
                    }
                    RadNumericTextBox tbpem = editedItem.FindControl("tbPublishedExpectedMargin") as RadNumericTextBox;
                    if (tbpem != null)
                    {
                        tbpem.Value = pem;
                    }

                }
            }
        }
        protected void dlProducts_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            if (dr != null)
            {
                int id = int.Parse(dr["id"].ToString());
                CheckBox cb = (CheckBox) e.Item.FindControl("cbProduct");
                if(cb!=null)
                {
                    cb.Attributes.Add(PRODUCTIDATTRIBUTE,id.ToString());
                }
            }
        }

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            string err;
            string productListXml;
            if(ValidateForCopy(out err, out productListXml))
            {
                DateTime dt1 = (DateTime)rdFrom.SelectedDate;
                DateTime dt2 = (DateTime)rdTo.SelectedDate;
                int res = product.CopyRates(Utils.RemoveTime(dt1), Utils.RemoveTime(dt2), productListXml);
                lblMessage.Text = String.Format("{0} rows has been copied", res);
            }
            else
            {
                lblMessage.Text = err;
            }
            //string productName = ddlProductToCopy.SelectedItem.Text;
            //int productToCopyId = int.Parse(ddlProductToCopy.SelectedValue);
            //int res = Product.CopyRates(product.ID, productToCopyId, Utils.RemoveTime(dt1), Utils.RemoveTime(dt2));
            //lblMessage.Text = String.Format("{0} rate(s) has beed copied to product {1}",res,productName);
        }
        private bool ValidateForCopy(out string err, out string productListXml)
        {
            err = String.Empty;
            productListXml = String.Empty;
            bool res = true;
            if (rdFrom.SelectedDate == null || rdTo.SelectedDate == null)
            {
                err = "Please specify date range";
                res = false;
            }
            else if (rdFrom.SelectedDate>rdTo.SelectedDate)
            {
                err = "Date from must be greater then date to";
                res = false;
            }
            else
            {
                productListXml = GetSelectedProducts();
                if(String.IsNullOrEmpty(productListXml))
                {
                    res = false;
                    err = "Please select at least one product";
                }
            }
            return res;
        }
        private string GetSelectedProducts()
        {
            string res = String.Empty;
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            for (int i=0; i< dlProducts.Items.Count; i++)
            {
                CheckBox cb = (CheckBox) dlProducts.Items[i].FindControl("cbProduct");
                if(cb!=null&&cb.Checked)
                {
                    XmlNode n = d.CreateElement(ITEMELEMENT);
                    XmlAttribute a = d.CreateAttribute("id");
                    a.Value = cb.Attributes[PRODUCTIDATTRIBUTE];
                    n.Attributes.Append(a);
                    root.AppendChild(n);
                }
            }
            if(root.ChildNodes.Count>0)
            {
                d.AppendChild(root);
                res = d.OuterXml;
            }
            return res;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            goBack();
        }
        private void goBack()
        {
            Response.Redirect("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.EDITPRODUCT);
        }
    }
}