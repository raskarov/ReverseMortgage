using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class MortgageClosingCosts : MortgageDataGridControl
    {
        #region Properties
        private decimal total;
        private DataView dvGridData;

        protected override DataView DvGridData
        {
            get
            {
                if (dvGridData == null)
                {
                    dvGridData = Payoff.GetPayoffList(MortgageId);// MortgagePrepaidItem.GetPrepaidItemsForGrid(MortgageId);
                    GetTotals();
                }
                return dvGridData;
            }
        }
        #endregion

        #region Methods
        protected void GetTotals()
        {
            total = 0;
            DataView dv = DvGridData;
            if (dv.Table.Rows.Count > 0)
            {
                foreach (DataRowView row in dv)
                {
                    try
                    {
                        total += Convert.ToDecimal(row["Amount"]);
                    }
                    catch { }
                }

            }
        }
        #endregion
        #region overriden methods
        protected override void Page_Load(object sender, EventArgs e)
        {
            G = gPays;
            base.Page_Load(sender, e);
        }
        protected override void G_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (showFooter)
                {
                    CreateFooter(e, 1);
                    e.Row.Cells[0].Text = "Total: ";
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[1].Text = total.ToString("C");
                }
            }
        }
        protected override void CheckFieldsAccess()
        {
        }
        protected override void GetGridFields()
        {
        }
        protected override void ResetDataSource()
        {
            dvGridData = null;
        }
        #endregion
    }
}