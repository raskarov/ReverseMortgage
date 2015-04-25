using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;


namespace LoanStarPortal.Controls
{
    public partial class BorrowerAkaNames : MortgageDataGridControl
    {
        #region constants
        private const string OBJECTNAME = "BorrowerAKAName";
        private static readonly string[] gridFieldsAdd = {"Name"};
        private const string ADDRECORDTEXT = "Add new name";
        private const string ONCLICKATTRIBUTE = "OnClick";
        #endregion

        #region fields
        private BorrowerAkaName akaName;
        private int borrowerId;
        private readonly string objectName = OBJECTNAME;
        private readonly string addRecordText = ADDRECORDTEXT;
        private DataView dvGridData;
//        private MortgageProfile mp;
        private bool isVisible;
        #endregion

        //public event UpdateNeeded OnUpdateNeeded;
        //public delegate void UpdateNeeded();

        #region Properties
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = true; }
        }
        public int BorrowerId
        {
            get { return borrowerId; }
            set { borrowerId = value; }
        }
        protected override string AddRecordText
        {
            get
            {
                return addRecordText;
            }
        }
        protected override string ObjectName
        {
            get
            {
                return objectName;
            }
        }
        protected override DataView DvGridData
        {
            get
            {
                if (dvGridData == null)
                {
                    dvGridData = Borrower.GetAkaNames(borrowerId);
                }
                return dvGridData;
            }
        }
        protected override object EditObject
        {
            get
            {
                if (akaName == null)
                {
                    akaName = new BorrowerAkaName(borrowerId);
                }
                return akaName;
            }
        }
        #endregion

        #region Methods

        #region overriden methods
        protected override void Page_Load(object sender, EventArgs e)
        {
//            mp = CurrentPage.GetMortgage(MortgageId);
            G = gAKANames;
            RadAjaxManager ajax = ((Default)Page).AjaxManager;
            ajax.AjaxSettings.AddAjaxSetting(gAKANames, gAKANames, null);
            base.Page_Load(sender, e);
        }
        protected override void CheckFieldsAccess()
        {
            canAddNew = true;
            canEdit = false;
            for (int i = 0; i < gridFieldsAdd.Length; i++)
            {
                if (fields.ContainsKey(gridFieldsAdd[i]))
                {
                    canAddNew &= !(bool)fields[gridFieldsAdd[i]];
                    canEdit |= !(bool)fields[gridFieldsAdd[i]];
                }
            }
        }
        protected override void G_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gridMode == MODEVIEW)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    if (btnDelete != null)
                    {
                        btnDelete.Attributes.Add(ONCLICKATTRIBUTE, "javascript:{{var r=confirm('Delete this name?');if (!r)return false;}};");
                    }
                }
                if (gridMode != MODEVIEW && currentRow == EditRowId)
                {
                    DataRowView dr = (DataRowView)e.Row.DataItem;
                    int id = Convert.ToInt32(dr["id"].ToString());
                    if (id > 0)
                        akaName = new BorrowerAkaName((DataRowView)e.Row.DataItem);
                    else
                        akaName = new BorrowerAkaName(-1);

                    EditItemId = akaName.ID;
                    if (gridMode != MODEVIEW)
                    {
                        Label lbl = (Label)e.Row.FindControl("lblName"); ;
                        TextBox tb = (TextBox)e.Row.FindControl("tbName");
                        if (tb != null)
                        {
                            tb.Visible = true;
                            tb.Text = akaName.Name;
                        }
                        if (lbl != null)
                        {
                            lbl.Visible = false;
                        }

                    }
                }
                if (gridMode != MODEVIEW)
                {
                    if (currentRow == EditRowId)
                    {
                        SetActionColumn(e, true,true);
                    }
                    else
                    {
                        SetActionColumn(e, false, true);
                    }
                }
                currentRow++;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (showFooter)
                {
                    CreateFooter(e, 2);
                }
            }
        }

        protected override void G_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            showFooter = true;
            base.G_RowCommand(sender, e);
            if (e.CommandName == DELETECOMMAND)
            {
                BorrowerAkaName.Delete(Convert.ToInt32(e.CommandArgument));
                ResetDataSource();
                BindGrid();
            }
        }
        protected override void Save(object o, ArrayList logs)
        {
        }
        protected override void Update(int rowIndex)
        {
            akaName = new BorrowerAkaName(EditItemId);
            if (akaName.BorrowerId < 1) akaName.BorrowerId = borrowerId;
            TextBox tb = (TextBox)gAKANames.Rows[rowIndex].FindControl("tbName");
            if (tb != null)
            {
                akaName.Name = tb.Text;
            }
            akaName.Save();
        }
        protected override void G_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            base.G_RowDeleting(sender, e);
        }
        protected override void ResetDataSource()
        {
            dvGridData = null;
        }
        #endregion

 
        #endregion
    }
}