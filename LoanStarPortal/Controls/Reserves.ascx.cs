using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;


namespace LoanStarPortal.Controls
{
    public partial class Reserves : MortgageDataGridControl
    {
        #region constants
        private const string OBJECTNAME = "Reserves";
        private static readonly string[] gridFieldsAdd = { 
            "Description", "Number", "Amount", "ChargeToId"
        };
        private const string EDITFORMCONTROLNAME = "ReserveEdit.ascx";
        private const string ADDRECORDTEXT = "Add new reserve";
        #endregion

        #region fields
        private Reserve reserve;
        private readonly string objectName = OBJECTNAME;
        private readonly string addRecordText = ADDRECORDTEXT;
        private DataView dvGridData;
        private decimal totalChargeBorrower = 0;
        private decimal totalChargeSeller = 0;
        #endregion

        #region Properties
        protected override string EditFormControlName
        {
            get
            {
                return EDITFORMCONTROLNAME;
            }
        }
        protected override int EditMode
        {
            get { return FORMEDIT; }
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
                    dvGridData = Reserve.GetReserveList(MortgageId);
                    GetTotals();
                }
                return dvGridData;
            }
        }
        protected override object EditObject
        {
            get
            {
                if (reserve == null)
                {
                    reserve = new Reserve();
                }
                return reserve;
            }
        }
        #endregion

        #region Methods
        protected void GetTotals()
        {
            totalChargeBorrower = 0;
            totalChargeSeller = 0;
            DataView dv = DvGridData;
            if (dv.Table.Rows.Count > 0)
            {
                foreach (DataRowView row in dv)
                {
                    try
                    {
                        if (Convert.ToInt32(row["ChargeToId"]) == Constants.INVOICECHARGETOBORROWER)
                            totalChargeBorrower += Convert.ToDecimal(row["Amount"]);
                        if (Convert.ToInt32(row["ChargeToId"]) == Constants.INVOICECHARGETOSELLER)
                            totalChargeSeller += Convert.ToDecimal(row["Amount"]);
                    }
                    catch { }
                }

            }
        }
        #region overriden methods
        protected override void Page_Load(object sender, EventArgs e)
        {
            bool visibility = CheckVisibility();
            if(visibility)
            {
                G = gReserves;
                base.Page_Load(sender, e);
            }
            Visible = visibility;
        }
        private bool CheckVisibility()
        {
            bool res = true;
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageId);
            if(mp!=null)
            {
                res = mp.MortgageInfo.TransactionTypeId == (int)MortgageInfo.TransactionTypeEnum.Purchase;
            }
            return res;
        }

        protected override void G_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gridMode != MODEVIEW)
                {
                    if (currentRow == EditRowId)
                    {
                        int id = Convert.ToInt32(((DataRowView)e.Row.DataItem)["id"].ToString());
                        if (id > 0)
                        {
                            reserve = new Reserve((DataRowView)e.Row.DataItem);
                        }
                        else
                        {
                            reserve = new Reserve(-1);
                        }
                    }
                    SetActionColumn(e, false, true);
                }
                currentRow++;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (showFooter)
                {
                    CreateFooter(e, 1);
                    int cellcount = e.Row.Cells.Count;
                    for (int i = 1; i < cellcount - 3; i++)
                    {
                        e.Row.Cells.RemoveAt(i);
                    }
                    e.Row.Cells[1].Text = "Total borrower: <br>Total seller: ";
                    e.Row.Cells[1].ColumnSpan = cellcount - 3;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                    e.Row.Cells[2].Text = totalChargeBorrower.ToString("C") + "<br />" + totalChargeSeller.ToString("C");
                }
            }
        }
        protected override void Save(object o, ArrayList logs)
        {
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageId);
            if (mp != null)
            {
                reserve.MortgageID = mp.ID;
                mp.SaveReserveWithLog(reserve, logs);
            }
            base.Save(o, logs);
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
                }
            }
            for (int i = 0; i < gridFieldsAdd.Length; i++)
            {
                if (fields.ContainsKey(gridFieldsAdd[i]))
                {
                    canEdit |= !(bool)fields[gridFieldsAdd[i]];
                }
            }
        }
        protected override void ResetDataSource()
        {
            dvGridData = null;
        }
        #endregion

        #endregion

    }
}