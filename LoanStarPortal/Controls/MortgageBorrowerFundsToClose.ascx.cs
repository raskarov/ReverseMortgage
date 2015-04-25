using System.Web.UI;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class MortgageBorrowerFundsToClose : MortgageDataControl
    {
        #region constants
        private const string OBJECTNAME = "MortgageInfo";
        private const string HEADER = "Borrowers Funds To Close";
        #endregion

        #region property field groups
        private const int GROUPID = 10;

        private static readonly string[] Info = {
             "VODCoverAmntShort", "VODFromFinacInst"
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

            ctl.ID = "MortgageBorrowerFundsToClose" + GROUPID;
            ph.Controls.Add(ctl);
        }

        #endregion
    }
}