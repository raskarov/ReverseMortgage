using System.Web.UI;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class ClosingAgent :  MortgageDataControl
    {
        #region constants
        private const string OBJECTNAME = "MortgageInfo";
        private const int GENERALGROUPID = 9;
        private const string TITLECOMPANYHEADER = "Title company";
        #endregion

        #region fields
        private MortgageInfo mpinfo;
        private static readonly string[] MortgageGeneral = { 
            "ClosingAgentId","TitleOrderNumber"
        };
        #endregion

        #region methods
        public override void BuildControl(Control pageview)
        {
            objectName = OBJECTNAME;
            mpinfo = Mp.MortgageInfo;
            base.BuildControl(pageview);
            mpinfo = Mp.MortgageInfo;
        }

        protected override void SetUIFields()
        {
            SetUIGroupFields(objectFields, MortgageGeneral, GENERALGROUPID);
        }

        protected override void PrepairObjectHtml()
        {
            Mp.EvaluateObjectFieldsVisibilty(mpinfo, objectFields);
            Fields ctl = GetTabWrapper(TITLECOMPANYHEADER, mpinfo, objectFields, GENERALGROUPID, "trmortgage", new string[] { "tdmortgagelabel1/tdmortgagecontrol1", "tdmortgagelabel2/tdmortgagecontrol2" }, 1);
            ctl.ID = "ctl_closingagent" + mpinfo.ID;
            if (ctl.HasChild)
            {
                ph.Controls.Add(ctl);
            }

        }

        #endregion

    }
}