using System;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class TrusteeSignatureLines : MortgageDataGridControl
    {
        #region constants
        private const string OBJECTNAME = "TrusteeSignatureLine";
        private static readonly string[] gridFieldsAdd = { "SignatureLine" };
        private const string ADDRECORDTEXT = "Add new record";
        private const string ONCLICKATTRIBUTE = "OnClick";
        #endregion

        #region fields
        private TrusteeSignatureLine line;
        private int borrowerId;
        private readonly string objectName = OBJECTNAME;
        private readonly string addRecordText = ADDRECORDTEXT;
        private DataView dvGridData;
        private MortgageProfile mp;
        private bool isVisible;
        #endregion

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
                    dvGridData = TrusteeSignatureLine.GetSignatureLinesForGrid(mp.ID);
                }
                return dvGridData;
            }
        }
        protected override object EditObject
        {
            get
            {
                if (line == null)
                {
                    line = new TrusteeSignatureLine(mp.Trustee.ID, -1);
                }
                return line;
            }
        }
        #endregion


        protected override void Page_Load(object sender, EventArgs e)
        {
            mp = CurrentPage.GetMortgage(MortgageId);
            G = gSignatureLines;
            RadAjaxManager ajax = ((Default)Page).AjaxManager;
            ajax.AjaxSettings.AddAjaxSetting(gSignatureLines, gSignatureLines, null);
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
            int statusId;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gridMode == MODEVIEW)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    if (btnDelete != null)
                    {
                        btnDelete.Attributes.Add(ONCLICKATTRIBUTE, "javascript:{{var r=confirm('Delete this signature line?');if (!r)return false;}};");
                    }
                }
                if (gridMode != MODEVIEW && currentRow == EditRowId)
                {
                    DataRowView dr = (DataRowView)e.Row.DataItem;
                    int id = Convert.ToInt32(dr["id"].ToString());
                    if (id > 0)
                        line = new TrusteeSignatureLine((DataRowView)e.Row.DataItem);
                    else
                        line = new TrusteeSignatureLine(mp.Trustee.ID, -1);

                    EditItemId = line.ID;
                    if (gridMode != MODEVIEW)
                    {
                        Label lbl = (Label)e.Row.FindControl("lblSignatureLine"); ;
                        TextBox tb = (TextBox)e.Row.FindControl("tbSignatureLine");
                        RequiredFieldValidator rfv = (RequiredFieldValidator)e.Row.FindControl("rfv");
                        if (tb != null)
                        {
                            tb.Visible = true;
                            tb.Text = line.SignatureLine;
                            if (rfv!=null) rfv.Visible = true;
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
                        SetActionColumn(e, true, true);
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
                TrusteeSignatureLine.Delete(Convert.ToInt32(e.CommandArgument));
                ResetDataSource();
                BindGrid();
            }
        }
        protected override void Save(object o, ArrayList logs)
        {
        }
        protected override void Update(int rowIndex)
        {
            line = new TrusteeSignatureLine(mp.ID,EditItemId);
            if (line.MortgageId < 1) line.MortgageId = mp.ID;
            TextBox tb = (TextBox)gSignatureLines.Rows[rowIndex].FindControl("tbSignatureLine");
            if (tb != null)
            {
                line.SignatureLine = tb.Text;
            }
            line.Save();
        }
        protected override void G_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            base.G_RowDeleting(sender, e);
        }
        protected override void ResetDataSource()
        {
            dvGridData = null;
        }


    }
}