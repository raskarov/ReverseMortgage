using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class VendorFeeCategory : AppControl
    {
        #region constants
        private const string CATEGORYFILTER = "categoryid={0} and name<>'Other'";
        private const string FEEIDATTRIBUTE = "feeid";
        private const string CLICKHANDLER = "onclick";
        private const string JSCLICKHANDLER = "CheckItem(this.checked,{0});";
        public const string CHECKBOXID = "cbFeeType";
        public const string RADINPUTID = "tbAmount";
        #endregion

        public bool IsInViewMode = false;
        #region methods
        public void BindData(DataView dv, int categoryId)
        {
            dv.RowFilter = String.Format(CATEGORYFILTER, categoryId);
            rpVendorFeeType.DataSource = dv;
            rpVendorFeeType.DataBind();
        }
        #endregion

        #region event handlers
        protected void rpVendorFeeType_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row != null)
                {
                    bool isChecked =  bool.Parse(row["selected"].ToString());
                    int id = int.Parse(row["id"].ToString());
                    CheckBox cb = (CheckBox)e.Item.FindControl(CHECKBOXID);
                    RadNumericTextBox tb = (RadNumericTextBox)e.Item.FindControl(RADINPUTID);
                    if ((cb != null) && (tb!=null))
                    {
                        cb.Enabled = !IsInViewMode;
                        cb.ID = CHECKBOXID+"_" + id.ToString();
                        tb.ID = RADINPUTID + "_" + id.ToString();
                        tb.ClientIDMode = System.Web.UI.ClientIDMode.Predictable;
                        cb.Text = row["name"].ToString() + (VendorGlobal.IsLoginRequired(id)?"(Password required)":"");
                        cb.Attributes.Add(CLICKHANDLER, String.Format(JSCLICKHANDLER, "ctl07_" + tb.ClientID));
                        cb.Checked = isChecked;
                        if (isChecked)
                        {
                            decimal amount = Convert.ToDecimal(row["amount"].ToString());
                            if (amount > 0)
                            {
                                tb.Text = amount.ToString();
                            }
                            else 
                            {
                                tb.Text = "";
                            }

                            
                        }
                        tb.Enabled = isChecked&&!IsInViewMode;;
                    }
                }
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}