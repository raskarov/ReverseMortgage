using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;


namespace LoanStarPortal.Administration.Controls
{
    public partial class ViewLeadCampaign : AppControl
    {
        #region constants
        private const int GRIDVIEWMODE = 0;
        private const int GRIDEDITMODE = 1;
        private const int GRIDADDMODE = 2;
        private const string LEADCAMPAIGNGRIDMODE = "leadcampaigngridmode";
        private const string EDITROW = "editrowid";
        private const string EDITITEM = "edititemid";
        private const string SORTEXPRESSION = "leadcampaignsort";
        private const string ASCENDING = "asc";
        private const string DESCENDING = "desc";
        private const string LEADCAMPAIGNPAGEINDEX = "leadcampaignpageindex";
        private const string EMPTYTEXT = "No records to display";
        private const string ADDCOMMAND = "Add";
        private const string EDITCOMMAND = "Edit";
        private const string SORTCOMMAND = "Sort";
        private const string PAGECOMMAND = "Page";
        private const string DELETECOMMAND = "Delete";
        private const string ONCLICK = "onclick";
        private const string DELETEJS = "javascript:{{var r=confirm('Delete this campaign?');if (!r)return false;}};";
        #endregion

        #region fields
        private DataView dvCampaign;
        private int currentRow = 0;
        private bool showFooter = true;
        private EditLeadCampaign editControl;
        private bool isNewSaved = false;
        #endregion

        #region properties
        private LeadCampaign CurrentCampaign
        {
            get
            {
                return (LeadCampaign)Session["currentleadcampaign"];
            }
            set
            {
                Session["currentleadcampaign"] = value;
            }
        }
        private int PageIndex
        {
            get
            {
                int res = 0;
                Object o = ViewState[LEADCAMPAIGNPAGEINDEX];
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
                ViewState[LEADCAMPAIGNPAGEINDEX] = value;
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
        private int GridMode
        {
            get
            {
                int res = GRIDVIEWMODE;
                Object o = ViewState[LEADCAMPAIGNGRIDMODE];
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
            set { ViewState[LEADCAMPAIGNGRIDMODE] = value; }
        }
        private DataView DvCampaign
        {
            get
            {
                if (dvCampaign == null)
                {
                    dvCampaign = LeadCampaign.GetCampignListForCompany(CurrentUser.EffectiveCompanyId);
                }
                return dvCampaign;
            }
        }
        #endregion

        #region methods
        protected string GetCurrentRow()
        {
            return currentRow.ToString();
        }
        protected static string GetOnOffState(Object item)
        {
            DataRowView dr = (DataRowView)item;
            return bool.Parse(dr["IsOn"].ToString()) ? "On" : "Off";
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

        private void BindCampaignGrid()
        {
            currentRow = 0;
            bool isEmpty = (DvCampaign.Count == 1) && (Convert.ToInt32(DvCampaign[0]["id"]) < 0);
            bool needInsert = !isEmpty && (GridMode == GRIDADDMODE);
            DataView dv;
            if (needInsert)
            {
                dv = GetDuplicate(DvCampaign);
            }
            else
            {
                dv = DvCampaign;
            }
            dv.Sort = GetSort();
            gLeadCampaign.PageIndex = PageIndex;
            gLeadCampaign.DataSource = dv;
            gLeadCampaign.DataBind();
            showFooter = (GridMode == GRIDVIEWMODE);
            if (gLeadCampaign.FooterRow != null) gLeadCampaign.FooterRow.Visible = showFooter;
            if (GridMode != GRIDADDMODE)
            {
                if (isEmpty)
                {
                    int columns = gLeadCampaign.Rows[0].Cells.Count;
                    gLeadCampaign.Rows[0].Cells.Clear();
                    gLeadCampaign.Rows[0].Cells.Add(new TableCell());
                    gLeadCampaign.Rows[0].Cells[0].ColumnSpan = columns;
                    gLeadCampaign.Rows[0].Cells[0].Text = EMPTYTEXT;
                }
            }
            if (GridMode != GRIDVIEWMODE)
            {
                ShowForm();
            }
        }
        private void ShowForm()
        {
            editControl = (EditLeadCampaign) LoadControl(Constants.FEADMINCONTROLSLOCATION + Constants.FECTLEDITLEADCAMPAIGN);
            if (editControl != null)
            {
                editControl.OnCancel += CancelEdit;
                editControl.OnSave += Save;
                editControl.BindData();
                TableCell td = new TableCell();
                int n = EditRowId + 1;
                if (n >= gLeadCampaign.Rows.Count)
                {
                    n = gLeadCampaign.Rows.Count - 1;
                }
                td.ColumnSpan = gLeadCampaign.Rows[n].Cells.Count;
                gLeadCampaign.Rows[n].Cells.Clear();
                td.Controls.Add(editControl);
                gLeadCampaign.Rows[n].Cells.Add(td);
            }

        }
        private void CancelEdit()
        {
            GridMode = GRIDVIEWMODE;
            dvCampaign = null;
            showFooter = true;
            BindCampaignGrid();
        }
        private void Save()
        {
            if(CurrentCampaign!=null)
            {
                if(CurrentCampaign.CompanyId <= 0)
                {
                    CurrentCampaign.CompanyId = CurrentUser.EffectiveCompanyId;
                }
                if (CurrentCampaign.Save() > 0)
                {
                    dvCampaign = null;
                }
            }
            CancelEdit();
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
            lb.Text = "Add campaign";
            lb.CssClass = "EmailLinks";
            lb.CommandName = ADDCOMMAND;
            td.Controls.Add(lb);
            return td;
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
            else if (e.CommandName == DELETECOMMAND)
            {
                if(LeadCampaign.Delete(Convert.ToInt32(e.CommandArgument)))
                {
                    dvCampaign = null;
                }
            }
            else if (e.CommandName == SORTCOMMAND)
            {
                SetSort(e.CommandArgument.ToString());
            }
            else if (e.CommandName == PAGECOMMAND)
            {
                PageIndex = Convert.ToInt32(e.CommandArgument.ToString()) - 1;
            }
            BindCampaignGrid();
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
                if(GridMode!=GRIDVIEWMODE)
                {
                    if(isNewSaved)
                    {
                        DataRowView dr = (DataRowView)e.Row.DataItem;
                        int id = int.Parse(dr["id"].ToString());
                        if(id==EditItemId)
                        {
                            CurrentCampaign = new LeadCampaign((DataRowView)e.Row.DataItem);
                            EditRowId = currentRow;
                        }
                    }
                    else
                    {
                        if(currentRow == EditRowId)
                        {
                            DataRowView dr = (DataRowView)e.Row.DataItem;
                            if (GridMode == GRIDADDMODE)
                            {
                                CurrentCampaign = new LeadCampaign(-1, CurrentUser.EffectiveCompanyId);
                            }
                            else
                            {
                                CurrentCampaign = new LeadCampaign((DataRowView)e.Row.DataItem);
                            }
                        }
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

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            BindCampaignGrid();
        }
    }
}