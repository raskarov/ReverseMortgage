using System;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;


namespace LoanStar.Common
{
    public class MortgageDataGridControl : AppControl
    {
        #region constants
        private const string EMPTYTEXT = "No records to display";
        protected const string ADDCOMMAND = "Add";
        protected const string EDITCOMMAND = "Edit";
        protected const string UPDATECOMMAND = "Update";
        protected const string SORTCOMMAND = "Sort";
        protected const string PAGECOMMAND = "Page";
        protected const string CANCELCOMMAND = "Cancel";
        protected const string DELETECOMMAND = "Delete";
        protected const string ASCENDING = "asc";
        protected const string DESCENDING = "desc";
        protected const int MODEVIEW = 0;
        protected const int MODEEDIT = 1;
        protected const int MODEADD = 2;
        protected const int INLINEEDIT = 1;
        protected const int FORMEDIT = 2;
        #endregion

        #region fields
        protected int currentRow = 0;
        protected bool canEdit = false;
        protected bool canAddNew = false;
        protected ArrayList objectFields;
        protected readonly Hashtable fields = new Hashtable();
        private GridView g;
        protected bool showFooter = true;
        protected bool enableValidation = false;
        #endregion

        #region properties
        protected virtual bool BindOnLoad
        {
            get { return true; }
        }
        protected int EditRowId
        {
            get
            {
                int res = -1;
                Object o = Session[ObjectName + "_currentrow"];
                if (o!=null)
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
            set { Session[ObjectName + "_currentrow"] = value; }
        }
        protected int gridMode
        {
            get
            {
                int res = MODEVIEW;
                Object o = Session[ObjectName+"_gridmode"];
                if (o != null)
                {
                    try
                    {
                        res = Convert.ToInt32(o);
                    }
                    catch { }
                }
                return res;
            }
            set
            {
                Session[ObjectName + "_gridmode"] = value;
            }
        }
        protected int EditItemId
        {
            get
            {
                int res = -1;
                Object o = Session[ObjectName+"_edititemid"];
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
                Session[ObjectName + "_edititemid"] = value;
            }
        }
        protected GridView G
        {
            set { g = value; }
        }
        protected virtual string EditFormControlName
        {
            get { return String.Empty; }
        }
        protected virtual string AddRecordText
        {
            get { return String.Empty; }
        }
        protected virtual string ObjectName
        {
            get { return String.Empty; }
        }
        protected virtual DataView DvGridData
        {
            get { return null; }
        }
        protected virtual int EditMode
        {
            get { return INLINEEDIT; }
        }
        protected virtual object EditObject
        {
            get { return null; }
        }
        protected int MortgageId
        {
            get
            {
                return Convert.ToInt32(Session[Constants.MortgageID]);
            }
        }
        protected string SortExpression
        {
            get
            {
                string res = String.Empty;
                Object o = Session[ObjectName + "_sortexpression"];
                if (o != null)
                {
                    res = o.ToString();
                }
                return res;
            }
            set
            {
                Session[ObjectName + "_sortexpression"] = value;
            }
        }
        protected virtual bool NeedDispatch
        {
            get { return false; }
        }
        protected virtual string GridName
        {
            get { return ""; }
        }
        #endregion

        #region methods

        #region virtual methods
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                EditItemId = -1;
                gridMode = MODEVIEW;
            }
            GetGridFields();
            CheckFieldsAccess();
            if (BindOnLoad)
            {
                BindGrid();
            }            
        }
        protected virtual void BindGrid()
        {
            currentRow = 0;
            bool isEmpty = (DvGridData.Count == 1)&&(Convert.ToInt32(DvGridData[0]["id"])<0);
            DataView dv;
            bool needInsert;
            if(EditMode == FORMEDIT)
            {
                needInsert = !isEmpty && ((gridMode == MODEEDIT) || (gridMode == MODEADD));
            }
            else
            {
                needInsert = !isEmpty && (gridMode == MODEADD);
            }
            if (needInsert)
            {
                dv = GetDuplicate(DvGridData);
            }
            else
            {
                dv = DvGridData;
            }
            dv.Sort = GetSort();
            g.DataSource = dv;
            g.DataBind();
            showFooter = (gridMode == MODEVIEW);
            if (g.FooterRow!=null) g.FooterRow.Visible = showFooter;
            if (gridMode != MODEADD)
            {
                if (isEmpty)
                {
                    int columns = g.Rows[0].Cells.Count;
                    g.Rows[0].Cells.Clear();
                    g.Rows[0].Cells.Add(new TableCell());
                    g.Rows[0].Cells[0].ColumnSpan = columns;
                    g.Rows[0].Cells[0].Text = EMPTYTEXT;
                }
            }
            if(((gridMode==MODEADD)||(gridMode==MODEEDIT))&&(EditMode==FORMEDIT))
            {
                ShowForm();
            }
        }
        protected virtual void G_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            gridMode = MODEVIEW;
            if (e.CommandName == ADDCOMMAND)
            {
                showFooter = false;
                gridMode = MODEADD;
                EditRowId = EditMode==FORMEDIT?-1:0;
            }
            else if (e.CommandName == EDITCOMMAND)
            {
                showFooter = false;
                EditRowId = Convert.ToInt32(e.CommandArgument);
                gridMode = MODEEDIT;
            }
            else if (e.CommandName == UPDATECOMMAND)
            {
                Update(EditRowId);
                ResetDataSource();
                gridMode = MODEVIEW;
                EditItemId = -1;
            }
            else if(e.CommandName==SORTCOMMAND)
            {
                SetSort(e.CommandArgument.ToString());
            }
            else if (e.CommandName==PAGECOMMAND)
            {
                g.PageIndex = Convert.ToInt32(e.CommandArgument.ToString())-1;
            }
            else if (e.CommandName==CANCELCOMMAND)
            {
                ResetDataSource();
            }
            else if (e.CommandName == DELETECOMMAND)
            {
                EditItemId = Convert.ToInt32(e.CommandArgument);
            }
            if (gridMode == MODEVIEW && e.CommandName != DELETECOMMAND)
            {
                EditItemId = -1;
                EditRowId = -1;
            }
            BindGrid();
        }
        protected virtual string GetSort()
        {
            string res = SortExpression;
            if(!String.IsNullOrEmpty(res))
            {
                res += " " + GetSortDirection(res);
            }
            return res;
        }
        protected virtual void SetSort(string order)
        {
            SortExpression = order;
            string dir = GetSortDirection(order);
            if (dir == ASCENDING)
            {
                dir = DESCENDING;
            }
            else
            {
                dir = ASCENDING;
            }
            Session[ObjectName + "_" + order] = dir;
        }
        protected virtual string GetSortDirection(string sortexpression)
        {
            string res;
            Object o = Session[ObjectName + "_" + sortexpression];
            if(o!=null)
            {
                res = o.ToString();
            }
            else
            {
                res = DESCENDING;
            }
            return res;
        }
        protected virtual void ShowForm()
        {
            if (!String.IsNullOrEmpty(EditFormControlName))
            {
                EditGridFormControl ctl = LoadControl(Constants.FECONTROLSLOCATION + EditFormControlName) as EditGridFormControl;
                if (ctl != null)
                {
                    ctl.EditObject = EditObject;
                    ctl.ID = "EditFormGridControl";
                    ctl.fields = fields;
                    ctl.EnableValidation = enableValidation;
                    ctl.ObjectName = ObjectName;
                    ctl.OnCancel += CancelEdit;
                    ctl.OnSave += Save;
                    ctl.BindData();
                    TableCell td = new TableCell();
                    int n = EditRowId + 1;
                    td.ColumnSpan = g.Rows[n].Cells.Count;
                    g.Rows[n].Cells.Clear();
                    td.Controls.Add(ctl);
                    g.Rows[n].Cells.Add(td);
                }
            }
        }
        protected virtual void CheckFieldsAccess()
        {
        }
        protected virtual void Update(int rowIndex)
        {
        }
        protected virtual void Delete(int rowIndex)
        {
        }
        protected virtual void G_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }
        protected virtual void G_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
        }
        protected virtual void G_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }
        protected virtual void G_RowCancel(object sender, GridViewCancelEditEventArgs e)
        {
        }
        protected virtual void G_SortCommand(object source, GridViewSortEventArgs e)
        {
        }
        protected virtual void G_Sorting(object  source,GridViewSortEventArgs e)
        {
        }
        protected virtual void G_PageIndexChanged(object source, EventArgs e)
        {
        }
        protected virtual void G_PageIndexChanging(Object source, EventArgs e)
        {
        }
        protected virtual void G_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        protected virtual void CancelEdit()
        {
            Session.Remove(Constants.GRIDPROPERTYLIST);
            ResetDataSource();
            gridMode = MODEVIEW;
            showFooter = true;
            EditItemId = -1;
            BindGrid();
        }
        protected virtual void CancelSave()
        {
            
        }
        protected virtual void Save(Object o, ArrayList logs)
        {
            CancelEdit();
        }
        protected virtual void ResetDataSource()
        {
        }
        protected virtual void GetGridFields()
        {
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageId);
            if (mp != null)
            {
                objectFields = mp.GetObjectFields(ObjectName);
                if (objectFields != null)
                {
                    for (int i = 0; i < objectFields.Count; i++)
                    {
                        MortgageProfileField mpf = (MortgageProfileField)objectFields[i];
                        if (mpf != null)
                        {
                            fields.Add(mpf.FullPropertyName.Replace(ObjectName + ".", ""), mpf.ReadOnly);
                        }
                    }
                }
            }
        }
        #endregion

        private DataView GetDuplicate(DataView dv)
        {
            DataTable dt = dv.Table;
            DataRow dr = dt.NewRow();
            int n = gridMode == MODEADD ? 0 : EditRowId+g.PageSize*g.PageIndex;
            dr.ItemArray = dt.Rows[n].ItemArray;
            if (gridMode==MODEADD)
            {
                dr["id"] = -1;
            }
            EditItemId = Convert.ToInt32(dr["id"]);
            dt.Rows.InsertAt(dr, n);
            return dt.DefaultView;
        }
        protected bool IsFieldEditable(string fieldName)
        {
            bool res = true;
            if (fields.ContainsKey((fieldName)))
                res = !(bool)fields[fieldName];

            return res;
        }
        protected void CreateFooter(GridViewRowEventArgs e,int colspan)
        {
            if (canAddNew)
            {
                int cellcount = e.Row.Cells.Count;
                int n = colspan;
                if ((colspan <= 0) || (colspan >= cellcount))
                {
                    n = cellcount;
                }
                TableCell td = GetAddTd(n);
                if (n==cellcount)
                {
                    e.Row.Cells.Clear();
                    e.Row.Cells.Add(td);
                }
                else
                {
                    for(int i=0;i<colspan;i++)
                    {
                        e.Row.Cells.RemoveAt(0);
                    }
                    e.Row.Cells.AddAt(0,td);
                }
            }
        }
        protected string GetCurrentRow()
        {
            return currentRow.ToString();
        }
        protected static void SetActionColumn(GridViewRowEventArgs e, bool isVisible,bool showUpdate)
        {
            ImageButton btn = (ImageButton) e.Row.FindControl("imgEdit");
            if (btn!=null)
            {
                btn.Visible = false;
            }
            btn = (ImageButton)e.Row.FindControl("imgDelete");
            if (btn != null)
            {
                btn.Visible = false;
            }
            if (isVisible)
            {
                if(showUpdate)
                {
                    btn = (ImageButton)e.Row.FindControl("imgUpdate");
                    if (btn != null)
                    {
                        btn.Visible = true;
                    }
                }
                btn = (ImageButton)e.Row.FindControl("imgCancel");
                if (btn != null)
                {
                    btn.Visible = true;
                }
            }
        }
        protected static string GetMoney(Object o, string fieldName)
        {
            string result = String.Empty;
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                try
                {
                    decimal amount = Convert.ToDecimal(row[fieldName].ToString());
                    result = amount.ToString("C");
                }
                catch
                {
                }
            }
            return result;
        }
        protected static string GetDate(Object o,string fieldName)
        {
            string result = String.Empty;
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                if (row[fieldName]!=DBNull.Value)
                {
                    DateTime expDate = Convert.ToDateTime(row[fieldName].ToString());
                    result = expDate.ToString("MM/dd/yyyy");
                }
            }
            return result;
        }
        private TableCell GetAddTd(int colspan)
        {
            TableCell td = new TableCell();
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
            lb.Text = AddRecordText;
            lb.CssClass = "EmailLinks";
            lb.CommandName = ADDCOMMAND;
            td.Controls.Add(lb);
            return td;
        }
        #endregion

    }
}
