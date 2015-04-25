using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class MortgageTrustee : MortgageDataControl
    {
        #region constants
        private const string OBJECTNAME = "MortgageInfo";
        private const string TITLECOMMITMENTREVIEWHEADER = "Title Commitment Review";
        #endregion

        #region property field groups
        private const int TITLECOMMITMENTREVIEWGROUPID = 21;

        private static readonly string[] TitleCommitmentReviewInfo = {
             "TrusteeAddress1", "TrusteeAddress2","TrusteeBirthDate", "TrusteeCity","TrusteeFirstName"
             ,"TrusteeLastName","TrusteesNotOnApp", "TrusteeSS","TrusteeStateId","TrusteeZip"
        };
        #endregion

        private MortgageInfo mpinfo;

        #region Methods
        public override void BuildControl(Control pageview)
        {
            objectName = OBJECTNAME;
            mpinfo = Mp.MortgageInfo;
            base.BuildControl(pageview);
            mpinfo = Mp.MortgageInfo;
        }
        protected override void SetUIFields()
        {
            SetUIGroupFields(objectFields, TitleCommitmentReviewInfo, TITLECOMMITMENTREVIEWGROUPID);
        }
        protected override void PrepairObjectHtml()
        {
            Mp.EvaluateObjectFieldsVisibilty(mpinfo, objectFields);

            Fields ctl = GetTabWrapper(TITLECOMMITMENTREVIEWHEADER, mpinfo, objectFields, TITLECOMMITMENTREVIEWGROUPID, "trproperty", new string[] { "tdpil1/tdpic1", "tdpil2/tdpic2" }, 2);
            if(ctl.HasChild)
            {
                ctl.ID = "MortgageTrustee" + TITLECOMMITMENTREVIEWGROUPID;
                ph.Controls.Add(ctl);
            }
        }

        #endregion
    }
}