using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class MortgageBorrowerGeneralTab : MortgageDataControl
    {
        #region constants
        private const string OBJECTNAME = "MortgageInfo";
        private const string GENERALHEADER = "General";
        private const string BORROWERSFUNDSTOCLOSEHEADER = "Borrowers Funds To Close";
        private const string CREDITREPORTREVIEWHEADER = "Credit Report Review";
        private const string CONSELINGCERTIFICATEREVIEWWHEADER = "Conseling Certificate Review";
        #endregion

        #region property field groups
        private const int GENERALGROUPID = 100;
        private const int BORROWERSFUNDSTOCLOSEGROUPID = 101;
        private const int CREDITREPORTREVIEWGROUPID = 102;
        private const int CONSELINGCERTIFICATEREVIEWGROUPID = 103;

        private static readonly string[] GeneralInfo = {
            "JointApp"
        };
        
        private static readonly string[] BorrowersFundsToCloseInfo = {
            "BnkStmntCoverAmntShort","CustUseOwnCash","GiftLtrBorrNamesSignOk","GiftLtrDonorRelationOk"
            ,"GiftLtrIncDollar","GiftLtrNoPaymentStmntOk","GiftLtrPaperTrailOK","GiftLtrReceived"
            ,"MethodProveFundsId","RecvdBnkStmnt","RecvdVOD","VODCoverAmntShort","VODFromFinacInst"
        };

        private static readonly string[] CreditReportReviewInfo = {
            "BKDischarged","BKDischargeDate","BKFlag","BKIntentionId","BKTypeId","Ch7BKDischarged"
            ,"CreditReportOrderDate","CreditReportRequestDate","ForclosureFlag","JudgementFlag"
            ,"JudgementOnTitle","OrderedCreditReport","ReceivedCreditReport","RushFlag"
            ,"StudentLoanDefalut","StudentLoanFlag","TaxLienFlag","UnpaidMortClosedYrAgo"
            ,"UnpaidMortgageFlag","UnpaidMortgageNotOnTitle"
        };

        private static readonly string[] ConselingCertificateReviewInfo = {
            "CounsCertCousDate","CounsCertExpDate","CounselorName"
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
            SetUIGroupFields(objectFields, GeneralInfo, GENERALGROUPID);
            SetUIGroupFields(objectFields, BorrowersFundsToCloseInfo, BORROWERSFUNDSTOCLOSEGROUPID);
            SetUIGroupFields(objectFields, CreditReportReviewInfo, CREDITREPORTREVIEWGROUPID);
            SetUIGroupFields(objectFields, ConselingCertificateReviewInfo, CONSELINGCERTIFICATEREVIEWGROUPID);
        }
        protected override void PrepairObjectHtml()
        {
            Mp.EvaluateObjectFieldsVisibilty(mpinfo, objectFields);

            Fields ctl = GetTabWrapper(GENERALHEADER, mpinfo, objectFields, GENERALGROUPID, "trborrower", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl.ID = "MBGeneralGeneralTab" + GENERALGROUPID;
            if (ctl.HasChild)
            {
                ph.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
                ph.Controls.Add(ctl);
            }

            ctl = GetTabWrapper(BORROWERSFUNDSTOCLOSEHEADER, mpinfo, objectFields, BORROWERSFUNDSTOCLOSEGROUPID, "trborrower", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl.ID = "MBGeneralFundsToCloseTab" + BORROWERSFUNDSTOCLOSEGROUPID;
            if (ctl.HasChild)
            {
                ph.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
                ph.Controls.Add(ctl);
            }

            ctl = GetTabWrapper(CREDITREPORTREVIEWHEADER, mpinfo, objectFields, CREDITREPORTREVIEWGROUPID, "trborrower", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl.ID = "MBGeneralCreditReportReviewTab" + CREDITREPORTREVIEWGROUPID;
            if (ctl.HasChild)
            {
                ph.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
                ph.Controls.Add(ctl);
            }

            ctl = GetTabWrapper(CONSELINGCERTIFICATEREVIEWWHEADER, mpinfo, objectFields, CONSELINGCERTIFICATEREVIEWGROUPID, "trborrower", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl.ID = "MBGeneralConselingCertificateReviewTab" + CONSELINGCERTIFICATEREVIEWGROUPID;
            if (ctl.HasChild)
            {
                ph.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
                ph.Controls.Add(ctl);
            }
        }

        #endregion
    }
}