using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class LoanOfficerStateLicensing : AppControl
    {
        #region constants
        private const int GRIDVIEWMODE = 0;
        private const int GRIDEDITMODE = 1;
        private const int GRIDADDMODE = 2;
        private const string LOANOFFICERLICENSEGRIDMODE = "loanofficerlicensegridmode";
        private const string SORTEXPRESSION = "loanofficerlicensesortexpression";
        private const string DESCENDING = "desc";
        private const string ASCENDING = "asc";
        private const string PAGEINDEX = "loanofficerlicensepageindex";
        private const string ADDCOMMAND = "Add";
        private const string EDITCOMMAND = "Edit";
        private const string UPDATECOMMAND = "Update";
        private const string CANCELCOMMAND = "Cancel";
        private const string DELETECOMMAND = "Delete";
        private const string SORTCOMMAND = "Sort";
        private const string PAGECOMMAND = "Page";
        private const string ONCLICK = "onclick";
        private const string DELETEJS = "javascript:{{var r=confirm('Delete this record?');if (!r)return false;}};";
        private const string EDITROW = "editrowloanofficer";
        private const string EDITITEM = "edititemloanofficer";
        #endregion


        #region fields
        private DataView dvStateLicense;
        private int currentRow = 0;
        private bool showFooter = true;
        #endregion
        
        #region properties
        private AppUser User
        {
            get
            {
                return Session[Constants.USEREDITOBJECTNAME] as AppUser;
            }
        }

        private DataView DvStateLicense
        {
            get
            {
                if (dvStateLicense == null)
                {
                    dvStateLicense = OriginatorStateLicense.GetStateLicenseListForLoanOfficer(User.CompanyId, User.Id);
                }
                return dvStateLicense;
            }
        }
        private int GridMode
        {
            get
            {
                int res = GRIDVIEWMODE;
                Object o = Session[LOANOFFICERLICENSEGRIDMODE];
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
            set { Session[LOANOFFICERLICENSEGRIDMODE] = value; }
        }
        private string SortExpression
        {
            get
            {
                string res = String.Empty;
                Object o = Session[SORTEXPRESSION];
                if (o != null)
                {
                    res = o.ToString();
                }
                return res;
            }
            set
            {
                Session[SORTEXPRESSION] = value;
            }
        }
        private int PageIndex
        {
            get
            {
                int res = 0;
                Object o = Session[PAGEINDEX];
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
                Session[PAGEINDEX] = value;
            }
        }
        private int EditRowId
        {
            get
            {
                int res = -1;
                Object o = Session[EDITROW];
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
            set { Session[EDITROW] = value; }
        }
        private int EditItemId
        {
            get
            {
                int res = -1;
                Object o = Session[EDITITEM];
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
                Session[EDITITEM] = value;
            }
        }
        #endregion

        #region methods
        private void BindData()
        {
            BindGrid();
        }

        #region grid
        private static void SetActionColumn(GridViewRowEventArgs e, bool isCurrentRow)
        {
            ImageButton btn = (ImageButton)e.Row.FindControl("imgEdit");
            if (btn != null)
            {
                btn.Visible = false;
            }
            btn = (ImageButton)e.Row.FindControl("imgDelete");
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
        private void UpdateLicensing(int rowIndex)
        {
            TextBox tb = (TextBox)gStateLicense.Rows[rowIndex].FindControl("tbLicenseNumber");
            RadDateInput rd = (RadDateInput)gStateLicense.Rows[rowIndex].FindControl("tbExpirationDate");
            if (tb != null && rd != null)
            {
                string licenseNumber = tb.Text;
                DateTime dt = (DateTime)rd.SelectedDate;
                OriginatorStateLicense.SaveLoanOfficerLicense(EditItemId, User.Id, licenseNumber, dt);
            }
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
            Session["order_" + expr] = dir;
        }
        protected string GetCurrentRow()
        {
            return currentRow.ToString();
        }
        private string GetSortDirection(string sortexpression)
        {
            string res;
            Object o = Session["order_" + sortexpression];
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
        private string GetSort()
        {
            string res = SortExpression;
            if (!String.IsNullOrEmpty(res))
            {
                res += " " + GetSortDirection(res);
            }
            return res;
        }
        private void BindGrid()
        {
            currentRow = 0;
            DataView dv = DvStateLicense;
            dv.Sort = GetSort();
            gStateLicense.PageIndex = PageIndex;
            gStateLicense.DataSource = dv;
            gStateLicense.DataBind();
            showFooter = (GridMode == GRIDVIEWMODE);
            if (gStateLicense.FooterRow != null) gStateLicense.FooterRow.Visible = showFooter;
        }
        protected void gStateLicense_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            showFooter = true;
            GridMode = GRIDVIEWMODE;
            if (e.CommandName == ADDCOMMAND)
            {
                showFooter = false;
                GridMode = GRIDADDMODE;
                EditRowId = 0;
            }
            else if (e.CommandName == UPDATECOMMAND)
            {
                UpdateLicensing(EditRowId);
                EditItemId = -1;
                EditRowId = -1;
                dvStateLicense = null;
            }
            else if (e.CommandName == CANCELCOMMAND)
            {
                EditItemId = -1;
                EditRowId = -1;
                dvStateLicense = null;
            }
            else if (e.CommandName == EDITCOMMAND)
            {
                showFooter = false;
                EditRowId = Convert.ToInt32(e.CommandArgument);
                GridMode = GRIDEDITMODE;
            }
            else if (e.CommandName == DELETECOMMAND)
            {
                OriginatorStateLicense.DeleteLoanOfficerLicense(Convert.ToInt32(e.CommandArgument),User.Id);
                dvStateLicense = null;
            }
            else if (e.CommandName == SORTCOMMAND)
            {
                SetSort(e.CommandArgument.ToString());
            }
            else if (e.CommandName == PAGECOMMAND)
            {
                PageIndex = Convert.ToInt32(e.CommandArgument.ToString()) - 1;
            }
            BindGrid();

        }
        protected void gStateLicense_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (GridMode == GRIDVIEWMODE)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    DataRowView dr = (DataRowView)e.Row.DataItem;
                    string licenseNumber = dr["licensenumber"].ToString();
                    if (btnDelete != null)
                    {
                        if (licenseNumber == "N/A")
                        {
                            btnDelete.Enabled = false;
                        }
                        else
                        {
                            btnDelete.Attributes.Add(ONCLICK, DELETEJS);
                        }
                    }
                }
                if (GridMode != GRIDVIEWMODE && currentRow == EditRowId)
                {
                    DataRowView dr = (DataRowView)e.Row.DataItem;
                    DateTime dt = Holidays.RemoveTime(DateTime.Now);
                    string licenseNumber = String.Empty;
                    if (dr != null)
                    {
                        EditItemId = Convert.ToInt32(dr["id"].ToString());
                        if (EditItemId > 0)
                        {
                            dt = DateTime.Parse(dr["expirationdate"].ToString());
                            licenseNumber = dr["licensenumber"].ToString();
                            if (licenseNumber == "N/A") licenseNumber = "";
                        }
                    }
                    else
                    {
                        EditItemId = -1;
                    }
                    Label lbl1 = (Label)e.Row.FindControl("lblLicenseNumber");
                    if (lbl1 != null)
                    {
                        lbl1.Visible = false;
                    }
                    TextBox tb = (TextBox)e.Row.FindControl("tbLicenseNumber");
                    if (tb != null)
                    {
                        tb.Visible = true;
                        tb.Text = licenseNumber;
                    }
                    RequiredFieldValidator rf = (RequiredFieldValidator)e.Row.FindControl("rfLicenseNumber");
                    if (rf != null)
                    {
                        rf.Visible = true;
                    }
                    Label lbl2 = (Label)e.Row.FindControl("lblExpirationDate");
                    if (lbl2 != null)
                    {
                        lbl2.Visible = false;
                    }
                    RadDateInput rd = (RadDateInput)e.Row.FindControl("tbExpirationDate");
                    if (rd != null)
                    {
                        rd.SelectedDate = dt;
                        rd.Visible = true;
                    }
                    RequiredFieldValidator rf1 = (RequiredFieldValidator)e.Row.FindControl("rfExpirationDate");
                    if (rf1 != null)
                    {
                        rf1.Visible = true;
                    }
                }
                if (GridMode != GRIDVIEWMODE)
                {
                    SetActionColumn(e, currentRow == EditRowId);
                }
                currentRow++;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (showFooter)
                {
//                    CreateFooter(e, 0, "Add state");
                }
            }
        }
        protected void gStateLicense_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }
        protected void gStateLicense_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
        }
        protected void gStateLicense_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }
        protected void gStateLicense_RowCancel(object sender, GridViewCancelEditEventArgs e)
        {
        }
        protected void gStateLicense_SortCommand(object source, GridViewSortEventArgs e)
        {
        }
        protected void gStateLicense_Sorting(object source, GridViewSortEventArgs e)
        {
        }
        protected void gStateLicense_PageIndexChanged(object source, EventArgs e)
        {
        }
        protected void gStateLicense_PageIndexChanging(Object source, EventArgs e)
        {
        }

        #endregion
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }
    }
}