using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class MortgageBorrowerCreditReportReview : MortgageDataControl
    {
        #region constants
        private const string OBJECTNAME = "MortgageInfo";
        private const string HEADER = "Credit Report Review";
        #endregion

        #region property field groups
        private const int GROUPID = 11;

        private static readonly string[] Info = {
             "UnpaidMortClosedYrAgo", "UnpaidMortgageFlag","UnpaidMortgageNotOnTitle"
            ,"OrderedCreditReport","ReceivedCreditReport","CreditReportOrderDate","BKFlag"
            ,"BKDischarged","BKTypeId","Ch7BKDischarged","BKDischargeDate","BKIntentionId"
            ,"ForclosureFlag","RushFlag","TaxLienFlag","StudentLoanFlag","StudentLoanDefalut"
            ,"JudgementFlag","JudgementOnTitle"
        };
        #endregion

        private MortgageInfo mpinfo;

        #region Methods
        public override void BuildControl(Control ctr)
        {
            objectName = OBJECTNAME;
            mpinfo = Mp.MortgageInfo;
            base.BuildControl(ctr);
            mpinfo = Mp.MortgageInfo;
        }
        protected override void SetUIFields()
        {
            SetUIGroupFields(objectFields, Info, GROUPID);
        }
        protected override void PrepairObjectHtml()
        {
            Mp.EvaluateObjectFieldsVisibilty(mpinfo, objectFields);

            Fields ctl = GetTabWrapper(HEADER, mpinfo, objectFields, GROUPID, "trborrower", new string[] { "tdborrowerlabel1/tdborrowercontrol1", "tdborrowerlabel11/tdborrowercontrol11" }, 2);

            ctl.ID = "MortgageBorrowerCreditReportReview" + GROUPID;
            ph.Controls.Add(ctl);
        }

        #endregion
    }
}