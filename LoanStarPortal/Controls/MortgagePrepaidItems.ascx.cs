using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;



namespace LoanStarPortal.Controls
{
    public partial class MortgagePrepaidItems : MortgageDataGridControl
    {

        #region constants
        private const string EDITFORMCONTROLNAME = "MortgagePrepaidItemEdit.ascx";
        private const string OBJECTNAME = "MortgagePrepaid";
        private const string ADDRECORDTEXT = "Add new prepaid item";
        private static readonly string[] gridFieldsAdd = { 
            "Description", "PaymentTo", "Amount","UnitId"
            ,"StatementStart","StatmentEnd"
        };
        #endregion

        #region fields
        private MortgagePrepaidItem item;
        private readonly string objectName = OBJECTNAME;
        private readonly string addRecordText = ADDRECORDTEXT;
        private DataView dvGridData;
        private decimal totalBorrower = 0;
        private decimal totalSeller = 0;
        #endregion

        #region properties
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
                if(dvGridData==null)
                {
                    dvGridData = MortgagePrepaidItem.GetPrepaidItemsForGrid(MortgageId);
                    CalculateTotals();
                }
                return dvGridData;
            }
        }
        protected override object EditObject
        {
            get
            {
                if (item==null)
                {
                    item = new MortgagePrepaidItem(-1);
                }
                return item;
            }
        }
        #endregion

        #region methods

        #region overriden methods
        protected override void Page_Load(object sender, EventArgs e)
        {
            bool visibility = CheckVisibility();
            if (visibility)
            {
                G = gPrepaidItems;
                base.Page_Load(sender, e);
            }
            Visible = visibility;
        }
        private bool CheckVisibility()
        {
            bool res = true;
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageId);
            if (mp != null)
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
                        int id = Convert.ToInt32(((DataRowView) e.Row.DataItem)["id"].ToString());
                        if(id>0)
                        {
                            item = new MortgagePrepaidItem((DataRowView)e.Row.DataItem);
                        }
                        else
                        {
                            item = new MortgagePrepaidItem(-1);
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
                    CreateFooter(e,2);
                    if (totalBorrower>0)
                    {
                        e.Row.Cells[e.Row.Cells.Count - 3].Text = totalBorrower.ToString("C");
                    }
                    if(totalSeller>0)
                    {
                        e.Row.Cells[e.Row.Cells.Count - 2].Text = totalSeller.ToString("C");
                    }
                }
            }
        }
        protected override void Save(object o, ArrayList logs)
        {
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageId);
            if (mp != null)
            {
                item.MortgageId = mp.ID;
                mp.SavePrepaidItemWithLog(item, logs);
            }
            base.Save(o,logs);
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
        private void CalculateTotals()
        {
            totalSeller = -1;
            totalBorrower = -1;
            if(dvGridData!=null)
            for(int i=0;i<dvGridData.Count;i++)
            {
                try
                {
                    totalSeller += decimal.Parse(dvGridData[i]["SellerPortion"].ToString());
                    totalBorrower += decimal.Parse(dvGridData[i]["BorrowerPortion"].ToString());
                }
                catch
                {
                }
            }
        }

        #endregion

        #endregion

    }
}