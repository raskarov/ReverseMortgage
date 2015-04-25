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

namespace LoanStarPortal.Administration.Controls
{
    public partial class ViewFee : AppControl
    {
        #region constants
        private const string SORTEXPRESSION = "vendorfeedefaultgridsort";
        private const string ASCENDING = "asc";
        private const string DESCENDING = "desc";
        private const int GRIDVIEWMODE = 0;
        private const int GRIDEDITMODE = 1;
        private const string GRIDMODE = "feetypegrid";
        private const string FEEDEFAULTPAGEINDEX = "feedefaultpageindex";
        private const string EDITROW = "editrowid";
        private const string EDITITEM = "edititemid";
        private const string PAGECOMMAND = "Page";
        private const string EDITCOMMAND = "Edit";
        private const string UPDATECOMMAND = "Update";
        private const string SORTCOMMAND = "Sort";
        private const string CANCELCOMMAND = "Cancel";
        private const string DDLID = "ddlDefault_{0}";
        private const string LBLID = "lblDefault_{0}";        
        private const string CHANGEJS = "ResetDdl(this);";
//        private const string DEFAULTS = "currentrowdefaults";
        #endregion

        #region fields
        private DataView dvFeeDefaults;
        protected int currentRow = 0;
        #endregion

        #region properties
        private int EditItemId
        {
            get
            {
                int res = -1;
                Object o = ViewState[EDITITEM];
                if (o != null)
                {
                    try
                    {
                        res = Convert.ToInt32(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set
            {
                ViewState[EDITITEM] = value;
            }
        }
        private int EditRowId
        {
            get
            {
                int res = -1;
                Object o = ViewState[EDITROW];
                if (o != null)
                {
                    try
                    {
                        res = Convert.ToInt32(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set { ViewState[EDITROW] = value; }
        }
        private int PageIndex
        {
            get
            {
                int res = 0;
                Object o = ViewState[FEEDEFAULTPAGEINDEX];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set
            {
                ViewState[FEEDEFAULTPAGEINDEX] = value;
            }
        }
        private int GridMode
        {
            get
            {
                int res = GRIDVIEWMODE;
                Object o = ViewState[GRIDMODE];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch { }                    
                }
                return res;
            }
            set
            {
                ViewState[GRIDMODE] = value;
            }
        }
        private DataView DvFeeDefaults
        {
            get 
            {
                if (dvFeeDefaults == null)
                {
                    dvFeeDefaults = VendorGlobal.GetCompanyFeeDefaults(CurrentUser.EffectiveCompanyId);
                }
                return dvFeeDefaults;
            }
        }
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
        #endregion

        #region methods
        private void BindData()
        {
            DataView dv = DvFeeDefaults;
            dv.Sort = GetSort();
            G.DataSource = dv;
            currentRow = 0;
            G.PageIndex = PageIndex;
            G.DataBind();
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
        private ListItem GetEmptyItem()
        {
            return new ListItem("- Select -", "0");
        }
        protected string GetCurrentRow()
        {
            return currentRow.ToString();
        }
        private void BindDdl(GridViewRowEventArgs e, int index, string val, DataView dv)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl(String.Format(DDLID,index));
            if (ddl != null)
            {
                ddl.Visible = true;
                ddl.DataSource = dv;
                ddl.DataTextField = "name";
                ddl.DataValueField = "id";
                ddl.DataBind();
                ddl.Items.Insert(0, GetEmptyItem());
                ddl.Attributes.Add("onchange", CHANGEJS);
                ListItem li = ddl.Items.FindByValue(val);
                if (li != null)
                {
                    li.Selected = true;
                }
            }
            Label lbl = (Label)e.Row.FindControl(String.Format(LBLID, index));
            if (lbl != null)
            {
                lbl.Visible = false;
            }
        }
        private static void SetActionColumn(GridViewRowEventArgs e, bool isCurrentRow)
        {
            ImageButton btn = (ImageButton)e.Row.FindControl("imgEdit");
            if (btn != null)
            {
                btn.Visible = false;
            }
            if (isCurrentRow)
            {
                btn = (ImageButton)e.Row.FindControl("imgUpdate");
                if (btn != null)
                {
                    btn.Visible = true;
                }
                btn = (ImageButton)e.Row.FindControl("imgCancel");
                if (btn != null)
                {
                    btn.Visible = true;
                }
            }
        }
        private int GetValue(GridViewCommandEventArgs e, string ddlId, int rowIndex)
        {
            int res = 0;
            DropDownList ddl = G.Rows[rowIndex].FindControl(ddlId) as DropDownList;
            if (ddl != null)
            {
                try
                {
                    res = int.Parse(ddl.SelectedValue.ToString());
                }
                catch { }
            }
            return res;
        }
        #endregion

        #region handlers
        protected void G_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridMode = GRIDVIEWMODE;
            if (e.CommandName == EDITCOMMAND)
            {
                GridMode = GRIDEDITMODE;
                EditRowId = Convert.ToInt32(e.CommandArgument);
            }
            else if (e.CommandName == UPDATECOMMAND)
            {
                int feeTypeId = EditItemId;
                int firstProviderId = GetValue(e, "ddlDefault_0", EditRowId);
                int secondProviderId = GetValue(e, "ddlDefault_1", EditRowId);
                int thirdProviderId = GetValue(e, "ddlDefault_2", EditRowId);
                VendorGlobal.SaveCompanyFeeDefault(CurrentUser.EffectiveCompanyId, feeTypeId, firstProviderId, secondProviderId, thirdProviderId);
                EditItemId = -1;
            }
            else if (e.CommandName == CANCELCOMMAND)
            {
                EditItemId = -1;
                EditRowId = -1;
            }
            else if (e.CommandName == PAGECOMMAND)
            {
                PageIndex = Convert.ToInt32(e.CommandArgument.ToString()) - 1;
            }
            else if (e.CommandName == SORTCOMMAND)
            {
                SetSort(e.CommandArgument.ToString());
            }
            BindData();
        }
        protected void G_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (GridMode != GRIDVIEWMODE && currentRow == EditRowId)
                {
                    DataRowView dr = (DataRowView)e.Row.DataItem;
                    int feeId = int.Parse(dr["id"].ToString());
                    DataView dv = VendorGlobal.GetVendorForFeeType(feeId);
                    BindDdl(e, 0, dr["FirstDefault"].ToString(), dv);
                    BindDdl(e, 1, dr["SecondDefault"].ToString(), dv);
                    BindDdl(e, 2, dr["ThirdDefault"].ToString(), dv);
                    EditItemId = feeId;
                }
                if (GridMode != GRIDVIEWMODE)
                {
                    SetActionColumn(e, currentRow == EditRowId);
                }
                currentRow++;
            }
        }
        protected void G_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
        }
        protected void G_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }
        protected void G_RowCancel(object sender, GridViewCancelEditEventArgs e)
        {
        }
        protected void G_SortCommand(object source, GridViewSortEventArgs e)
        {
        }
        protected void G_Sorting(object source, GridViewSortEventArgs e)
        {
        }
        protected void G_PageIndexChanged(object source, EventArgs e)
        {
        }
        protected void G_PageIndexChanging(Object source, EventArgs e)
        {
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(CurrentUser.IsCorrespondentLenderAdmin || CurrentUser.LoggedAsOriginator))
            {
                Response.Redirect(ResolveUrl("~/" + CurrentUser.GetDefaultPage()));
            }
            if (!Page.IsPostBack)
            {
                GridMode = GRIDVIEWMODE;
                BindData();
            }
            //else
            //{
            //    ProcesssPostBack();
            //}
        }
    }
}