using System;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class GlobalAdminFields : AppControl
    {
        private LoanStar.Common.GlobalAdmin globalAdmin;

        #region Methods
        private void BindData()
        {
//            globalAdmin = new LoanStar.Common.GlobalAdmin(Constants.GID);
            globalAdmin = new LoanStar.Common.GlobalAdmin();
            tbMaxFEMAFloodCoverage.Text = globalAdmin.MaxFEMAFloodCoverage.ToString();
            tbFHAConnectionUrl.Text = globalAdmin.FHAConnectionUrl;
            tbLDPUrl.Text = globalAdmin.LDPUrl;
            tbGSAUrl.Text = globalAdmin.GSAUrl;
            tbUSPSUrl.Text = globalAdmin.USPSUrl;
            tbDefaultCreditReportVendor.Text = globalAdmin.DefaultCreditReportVendor;
            tbDefaultFloodCertificationVendor.Text = globalAdmin.DefaultFloodCertificationVendor;
            tbDefaultAppraisalVendor.Text = globalAdmin.DefaultAppraisalVendor;
            tbDefaultCounselingVendor.Text = globalAdmin.DefaultCounselingVendor;
            tbDefaultSurveyVendor.Text = globalAdmin.DefaultSurveyVendor;
            tbDefaultTitleVendor.Text = globalAdmin.DefaultTitleVendor;
        }

        #endregion
        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsLoanStarAdmin)
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            if (!IsPostBack)
            {
                BindData();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
//            globalAdmin = new LoanStar.Common.GlobalAdmin(Constants.GID);
            globalAdmin = new LoanStar.Common.GlobalAdmin();
            globalAdmin.MaxFEMAFloodCoverage =
            globalAdmin.MaxFEMAFloodCoverage = Convert.ToDecimal(tbMaxFEMAFloodCoverage.Text);
            globalAdmin.FHAConnectionUrl = tbFHAConnectionUrl.Text;
            globalAdmin.LDPUrl = tbLDPUrl.Text;
            globalAdmin.GSAUrl = tbGSAUrl.Text;
            globalAdmin.USPSUrl = tbUSPSUrl.Text;
            globalAdmin.DefaultCreditReportVendor = tbDefaultCreditReportVendor.Text;
            globalAdmin.DefaultFloodCertificationVendor = tbDefaultFloodCertificationVendor.Text;
            globalAdmin.DefaultAppraisalVendor = tbDefaultAppraisalVendor.Text;
            globalAdmin.DefaultCounselingVendor = tbDefaultCounselingVendor.Text;
            globalAdmin.DefaultSurveyVendor = tbDefaultSurveyVendor.Text;
            globalAdmin.DefaultTitleVendor = tbDefaultTitleVendor.Text;
            globalAdmin.Save();
            lblMessage.Text = Constants.SUCCESSMESSAGE;
        }
        #endregion
    }
}