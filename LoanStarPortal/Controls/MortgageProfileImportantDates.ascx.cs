using System;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class MortgageProfileImportantDates : AppControl
    {
        private MortgageProfile mp;

        protected int MortgageId
        {
            get
            {
                return Convert.ToInt32(Session[Constants.MortgageID]);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            mp = CurrentPage.GetMortgage(MortgageId);
            BindData();
        }
        private void BindData()
        {
            rpDates.DataSource = mp.GetImportantDates();
            rpDates.DataBind();
        }
    }
}