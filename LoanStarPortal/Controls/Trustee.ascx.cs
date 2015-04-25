using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class Trustee : MortgageDataGridControl
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
                    dvGridData = mp.GetSignatureLinesDv();
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
                        btnDelete.Attributes.Add(ONCLICKATTRIBUTE, "javascript:{{var r=confirm('Delete this name?');if (!r)return false;}};");
                    }
                }
                if (gridMode != MODEVIEW && currentRow == EditRowId)
                {
                    DataRowView dr = (DataRowView)e.Row.DataItem;
                    int id = Convert.ToInt32(dr["id"].ToString());
                    if (id > 0)
                        line = new TrusteeSignatureLine((DataRowView)e.Row.DataItem);
                    else
                        line = new TrusteeSignatureLine(mp.Trustee.ID,-1);

                    EditItemId = line.ID;
                    if (gridMode != MODEVIEW)
                    {
                        Label lbl = (Label)e.Row.FindControl("lblName"); ;
                        TextBox tb = (TextBox)e.Row.FindControl("tbName");
                        if (tb != null)
                        {
                            tb.Visible = true;
                            tb.Text = line.SignatureLine;
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

    }
}