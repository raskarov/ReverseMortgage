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
using Telerik.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class ViewVendors : AppControl
    {
        #region constants
        private const string VENDORID = "globalvendorid";
        private const string SORTEXPRESSION = "vendorgridsort";
        private const string ASCENDING = "asc";
        private const string DESCENDING = "desc";
        #endregion

        #region fields
        private DataView dvVendor;
        #endregion

        #region properties
        private string SortExpression
        {
            get
            {
                string res = String.Empty;
                Object o = ViewState[SORTEXPRESSION];
                if (o != null)
                {
                    res = o.ToString();
                }
                return res;
            }
            set
            {
                ViewState[SORTEXPRESSION] = value;
            }
        }
        protected DataView DvVendor
        {
            get
            {
                if (dvVendor == null)
                {
                    dvVendor = VendorGlobal.GetVendorsList();
                }
                return dvVendor;
            }
        }
        #endregion

        #region methods
        private void BindData()
        {
            DataView dv = DvVendor;
            dv.Sort = GetSort();
            gVendors.DataSource = dv;
            gVendors.DataBind();
        }
        private string GetSort()
        {
            string res = SortExpression;
            if (!String.IsNullOrEmpty(res))
            {
                res += " " + GetSortDirection(res);
            }
            return res;
        }
        private string GetEditLink(int vendorId)
        {
            return ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.EDITVENDOR + "&" + Constants.IDPARAM + "=" + vendorId);
        }
        private string GetSortDirection(string sortexpression)
        {
            string res;
            Object o = ViewState["order_" + sortexpression];
            if (o != null)
            {
                res = o.ToString();
            }
            else
            {
                res = DESCENDING;
            }
            return res;
        }
        private void SetSort(string expr)
        {
            SortExpression = expr;
            string dir = GetSortDirection(expr);
            if (dir == ASCENDING)
            {
                dir = DESCENDING;
            }
            else
            {
                dir = ASCENDING;
            }
            ViewState["order_" + expr] = dir;
        }
        #endregion

        #region event handlers
        protected void gVendors_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            gVendors.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }
        protected void gVendors_SortCommand(object source, GridSortCommandEventArgs e)
        {
            SetSort(e.SortExpression);
            BindData();
        }
        protected void gVendors_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                Response.Redirect(GetEditLink(-1));
            }
            else if (e.CommandName == RadGrid.DeleteCommandName)
            {
                VendorGlobal.DeleteVendor(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]));
                BindData();
            }
            if (e.CommandName == RadGrid.EditCommandName)
            {
                Response.Redirect(GetEditLink(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"])));
            }            
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsLoanStarAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            if (!Page.IsPostBack)
            {
//                VendorId = null;
                BindData();
            }
        }
    }
}