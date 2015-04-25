using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditManagerRelation : AppControl
    {
        #region constants
        private const int GRIDVIEWMODE = 0;
        private const int GRIDEDITMODE = 1;
        private const int GRIDADDMODE = 2;
        private const string EMPLOYEEGRIDMODE = "employeegridmode_{0}";
        private const string EDITROW = "editemployeerowid_{0}";
        private const string EDITITEM = "editemployeeitemid_{0}";
        private const string SORTEXPRESSION = "employeesort_{0}";
        private const string ASCENDING = "asc";
        private const string DESCENDING = "desc";
        private const string EMPLOYEEPAGEINDEX = "employeepageindex_{0}";
        private const string EMPTYTEXT = "No records to display";
        private const string DELETEJS = "javascript:{{var r=confirm('Delete this employee?');if (!r)return false;}};";
        private const string ONCLICK = "onclick";
        private const string ADDCOMMAND = "Add";
        private const string EDITCOMMAND = "Edit";
        private const string UPDATECOMMAND = "Update";
        private const string CANCELCOMMAND = "Cancel";
        private const string DELETECOMMAND = "Delete";
        private const string SORTCOMMAND = "Sort";
        private const string PAGECOMMAND = "Page";
        private const string DOPOSTBACK = "__doPostBack('{0}','{1}');";
        #endregion


        #region fields
        private int roleId;
        private bool canHaveEmployee = false;
        private bool canHaveManager = false;
        private int currentRow = 0;
        private DataView dvEmployee;
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

        private int UserId
        {
            get
            {
                int res = -1;
                if(User!=null)
                {
                    res = User.Id;
                }
                return res;
            }
        }
        private DataView DvEmployee
        {
            get
            {
                if(dvEmployee==null)
                {
                    dvEmployee = ManagerRelation.GetEmployee(UserId, roleId);
                }
                return dvEmployee;
            }
        }
        private int GridMode
        {
            get
            {
                int res = GRIDVIEWMODE;
                Object o = Session[String.Format(EMPLOYEEGRIDMODE,roleId)];
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
            set { Session[String.Format(EMPLOYEEGRIDMODE,roleId)] = value; }
        }
        private int EditRowId
        {
            get
            {
                int res = -1;
                Object o = Session[String.Format(EDITROW, roleId)];
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
            set { Session[String.Format(EDITROW, roleId)] = value; }
        }
        private int EditItemId
        {
            get
            {
                int res = -1;
                Object o = Session[String.Format(EDITITEM, roleId)];
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
                Session[String.Format(EDITITEM, roleId)] = value;
            }
        }
        private string SortExpression
        {
            get
            {
                string res = String.Empty;
                Object o = Session[String.Format(SORTEXPRESSION, roleId)];
                if (o != null)
                {
                    res = o.ToString();
                }
                return res;
            }
            set
            {
                Session[String.Format(SORTEXPRESSION, roleId)] = value;
            }
        }
        private int PageIndex
        {
            get
            {
                int res = 0;
                Object o = Session[String.Format(EMPLOYEEPAGEINDEX, roleId)];
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
                Session[String.Format(EMPLOYEEPAGEINDEX, roleId)] = value;
            }
        }
        #endregion

        #region methods
        private int GetRoleId()
        {
            int res = -1;
            try
            {
                string s = ID.Replace("emr_", "");
                res = int.Parse(s);
            }
            catch
            {
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

        #region data binding methods
        private void BindData()
        {
            lblManagerErr.Text = "";
            canHaveEmployee = ManagerRelation.CanHaveEmployee(roleId);
            canHaveManager = ManagerRelation.CanHaveManager(roleId);
            tblManager.Visible = canHaveManager;
            tblEmployee.Visible = canHaveEmployee;
            if (canHaveManager)
            {
                BindManagerDdl();
            }
            if (canHaveEmployee)
            {
                BindGrid();
            }
        }
        private void BindManagerDdl()
        {
            DataView dv = ManagerRelation.GetManagerForUserRole(CurrentUser.EffectiveCompanyId, UserId, roleId);
            ddlManager.DataSource = dv;
            ddlManager.DataTextField = "name";
            ddlManager.DataValueField = "id";
            ddlManager.DataBind();
            ddlManager.Items.Insert(0, new ListItem("- Select -", "0"));
            DataRow[] rows = dv.Table.Select("selected=1");
            if (rows.Length == 1)
            {
                string selected = rows[0]["id"].ToString();
                ListItem li = ddlManager.Items.FindByValue(selected);
                if (li != null) li.Selected = true;
            }
        }
        #endregion

        #region grid related
        private DataView GetDuplicate(DataView dv)
        {
            DataTable dt = dv.Table;
            DataRow dr = dt.NewRow();
            int n = GridMode == GRIDADDMODE ? 0 : EditRowId;
            dr.ItemArray = dt.Rows[n].ItemArray;
            if (GridMode == GRIDADDMODE)
            {
                dr["id"] = -1;
            }
            EditItemId = Convert.ToInt32(dr["id"]);
            dt.Rows.InsertAt(dr, n);
            return dt.DefaultView;
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
            bool isEmpty = (DvEmployee.Count == 1) && (Convert.ToInt32(DvEmployee[0]["id"]) < 0);
            bool needInsert = !isEmpty && (GridMode == GRIDADDMODE);
            DataView dv;
            if (needInsert)
            {
                dv = GetDuplicate(DvEmployee);
            }
            else
            {
                dv = DvEmployee;
            }
            dv.Sort = GetSort();
            gEmployee.PageIndex = PageIndex;
            gEmployee.DataSource = dv;
            gEmployee.DataBind();
            showFooter = (GridMode == GRIDVIEWMODE);
            if (gEmployee.FooterRow != null) gEmployee.FooterRow.Visible = showFooter;
            if (GridMode != GRIDADDMODE)
            {
                if (isEmpty)
                {
                    int columns = gEmployee.Rows[0].Cells.Count;
                    gEmployee.Rows[0].Cells.Clear();
                    gEmployee.Rows[0].Cells.Add(new TableCell());
                    gEmployee.Rows[0].Cells[0].ColumnSpan = columns;
                    gEmployee.Rows[0].Cells[0].Text = EMPTYTEXT;
                }
            }
        }
        protected string GetCurrentRow()
        {
            return currentRow.ToString();
        }
        private void CreateFooter(GridViewRowEventArgs e, int colspan)
        {
            int cellcount = e.Row.Cells.Count;
            int n = colspan;
            if ((colspan <= 0) || (colspan >= cellcount))
            {
                n = cellcount;
            }
            TableCell td = GetAddTd(n);
            if (n == cellcount)
            {
                e.Row.Cells.Clear();
                e.Row.Cells.Add(td);
            }
            else
            {
                for (int i = 0; i < colspan; i++)
                {
                    e.Row.Cells.RemoveAt(0);
                }
                e.Row.Cells.AddAt(0, td);
            }
        }
        private TableCell GetAddTd(int colspan)
        {
            TableCell td = new TableCell();
            td.CssClass = "footertd";
            td.ColumnSpan = colspan;
            td.VerticalAlign = VerticalAlign.Middle;
            ImageButton img = new ImageButton();
            img.ID = "imgAdd";
            img.BorderWidth = 0;
            img.AlternateText = "Add";
            img.ImageUrl = ResolveUrl(Constants.IMAGEFOLDER + "/addrecord.gif");
            img.CommandName = ADDCOMMAND;
            td.Controls.Add(img);
            LinkButton lb = new LinkButton();
            lb.ID = "lbAdd";
            lb.Text = "Add Employee";
            lb.CssClass = "EmailLinks";
            lb.CommandName = ADDCOMMAND;
            td.Controls.Add(lb);
            return td;
        }
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

        #region grid handlers
        protected void G_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (GridMode == GRIDVIEWMODE)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    if (btnDelete != null)
                    {
                        btnDelete.Attributes.Add(ONCLICK, DELETEJS);
                    }
                }
                if (GridMode != GRIDVIEWMODE && currentRow == EditRowId)
                {
                    DataRowView dr = (DataRowView)e.Row.DataItem;
                    int id = Convert.ToInt32(dr["id"].ToString());
                    EditItemId = id;
                    Label lbl = (Label)e.Row.FindControl("lblName");
                    if (lbl != null)
                    {
                        lbl.Visible = false;
                    }
                    DropDownList ddl = (DropDownList)e.Row.FindControl("ddlEmployee");
                    if (ddl != null)
                    {
                        ddl.Visible = true;
                        ddl.DataSource = ManagerRelation.GetEmployeeList(id, CurrentUser.EffectiveCompanyId, UserId, roleId);
                        ddl.DataTextField = "name";
                        ddl.DataValueField = "id";
                        ddl.DataBind();
                    }
                    Label lbl1 = (Label)e.Row.FindControl("lblRoleName");
                    if (lbl1 != null)
                    {
                        lbl1.Visible = false;
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
                    CreateFooter(e, 0);
                }
            }
        }
        protected void G_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            showFooter = true;
            GridMode = GRIDVIEWMODE;
            if (e.CommandName == ADDCOMMAND)
            {
                showFooter = false;
                GridMode = GRIDADDMODE;
                EditRowId = 0;
            }
            else if (e.CommandName == EDITCOMMAND)
            {
                showFooter = false;
                EditRowId = Convert.ToInt32(e.CommandArgument);
                GridMode = GRIDEDITMODE;
            }
            else if (e.CommandName == CANCELCOMMAND)
            {
                dvEmployee = null;
            }
            else if (e.CommandName == UPDATECOMMAND)
            {
                DropDownList ddl = (DropDownList)gEmployee.Rows[EditRowId].FindControl("ddlEmployee");
                if (ddl != null)
                {
                    int val = Convert.ToInt32(ddl.SelectedValue);
                    int userId = val / 100;
                    int employeeRoleId = val - userId * 100;
                    ManagerRelation.SaveManagerEmployee(EditItemId, UserId, roleId, userId, employeeRoleId);
                }
                dvEmployee = null;
                EditItemId = -1;
                EditRowId = -1;
            }
            else if (e.CommandName == DELETECOMMAND)
            {
                ManagerRelation.DeleteEmployee(Convert.ToInt32(e.CommandArgument));
                dvEmployee = null;
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
        protected void G_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
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

        #endregion

        #endregion
        

        protected void Page_Load(object sender, EventArgs e)
        {
            roleId = GetRoleId();
            BindData();
        }
    }
}