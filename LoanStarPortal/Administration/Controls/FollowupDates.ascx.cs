using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class FollowupDates : AppControl
    {
        #region constants
        private const int GRIDVIEWMODE = 0;
        private const int GRIDEDITMODE = 1;
        private const int GRIDADDMODE = 2;
        private const string UDFDATESPAGEINDEX = "udfdatespageindex";
        private const string UDFDATESGRIDMODE = "udfdatesgridmode";
        private const string EDITROW = "editdaterowid";
        private const string EDITITEM = "editdateitemid";
        private const string ONCLICK = "onclick";
        private const string DELETEJS = "javascript:{{var r=confirm('Delete this campaign?');if (!r)return false;}};";
        private const string ADDCOMMAND = "Add";
        private const string PAGECOMMAND = "Page";
        private const string UPDATECOMMAND = "Update";
        private const string DELETECOMMAND = "Delete";
        private const string EDITCOMMAND = "Edit";
        #endregion

        #region fields
        public int LeadCampaignId;
        private int currentRow = 0;
        private DataView dvDates;
        private bool showFooter = true;
        #endregion

        #region properties
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
        private int GridMode
        {
            get
            {
                int res = GRIDVIEWMODE;
                Object o = Session[UDFDATESGRIDMODE];
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
            set { Session[UDFDATESGRIDMODE] = value; }
        }
        private DataView DvDates
        {
            get
            {
                if (dvDates == null)
                {
                    dvDates = LeadCampaign.GetUserDefinedDates(LeadCampaignId);
                }
                return dvDates;
            }
        }
        private int PageIndex
        {
            get
            {
                int res = 0;
                Object o = Session[UDFDATESPAGEINDEX];
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
                Session[UDFDATESPAGEINDEX] = value;
            }
        }
        protected string GetCurrentRow()
        {
            return currentRow.ToString();
        }
        public void Rebind()
        {
            dvDates = null;
            BindGrid();
        }

        #endregion

        #region methods
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

        private void BindGrid()
        {
            currentRow = 0;
            bool isEmpty = (DvDates.Count == 1) && (Convert.ToInt32(DvDates[0]["Id"]) < 0);
            bool needInsert = !isEmpty && (GridMode == GRIDADDMODE);
            DataView dv;
            if (needInsert)
            {
                dv = GetDuplicate(DvDates);
            }
            else
            {
                dv = DvDates;
            }
            gShedule.PageIndex = PageIndex;
            gShedule.DataSource = dv;
            gShedule.DataBind();
            showFooter = (GridMode == GRIDVIEWMODE);
            if (gShedule.FooterRow != null) gShedule.FooterRow.Visible = showFooter;
            if (GridMode != GRIDADDMODE)
            {
                if (isEmpty)
                {
                    int columns = gShedule.Rows[0].Cells.Count;
                    gShedule.Rows[0].Cells.Clear();
                    gShedule.Rows[0].Cells.Add(new TableCell());
                    gShedule.Rows[0].Cells[0].ColumnSpan = columns;
                    gShedule.Rows[0].Cells[0].Text = "No records to display";
                }
            }
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
            lb.Text = "Add date";
            lb.CssClass = "EmailLinks";
            lb.CommandName = ADDCOMMAND;
            td.Controls.Add(lb);
            return td;
        }
        protected static string GetDaysOffset(Object item)
        {
            DataRowView dr = (DataRowView)item;
            return String.Format("+{0}", dr["dayoffset"]);

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
            else if (e.CommandName == UPDATECOMMAND)
            {
                int id = EditItemId;
                RadNumericTextBox tb = (RadNumericTextBox) gShedule.Rows[EditRowId].FindControl("tbDayOffset");
                if(tb!=null)
                {
                    if(!LeadCampaign.SaveDate(id, LeadCampaignId, (int) tb.Value))
                    {
                        lblErr.Text = "This date already exists";
                    }
                    EditRowId = -1;
                    EditItemId = -1;
                    dvDates = null;
                }
            }
            else if (e.CommandName == EDITCOMMAND)
            {
                showFooter = false;
                EditRowId = Convert.ToInt32(e.CommandArgument);
                GridMode = GRIDEDITMODE;
            }
            else if (e.CommandName == DELETECOMMAND)
            {
                if (LeadCampaign.DeleteDate(Convert.ToInt32(e.CommandArgument)))
                {
                    dvDates = null;
                }
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
                    EditItemId = int.Parse(dr["id"].ToString());
                    RadNumericTextBox tb = (RadNumericTextBox)e.Row.FindControl("tbDayOffset");
                    RequiredFieldValidator rf = (RequiredFieldValidator)e.Row.FindControl("rfDayOffset");
                    int offset = 0;
                    if (GridMode == GRIDEDITMODE)
                    {
                        offset = int.Parse(((DataRowView)e.Row.DataItem)["dayoffset"].ToString());
                    }
                    if (tb != null)
                    {
                        tb.Value = offset;
                        tb.Visible = true;
                        if (rf != null)
                        {
                            rf.Visible = true;
                        }
                    }
                    Label lbl = (Label)e.Row.FindControl("lblDaysOffset");
                    if (lbl != null)
                    {
                        lbl.Visible = false;
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
            BindGrid();
        }
    }
}