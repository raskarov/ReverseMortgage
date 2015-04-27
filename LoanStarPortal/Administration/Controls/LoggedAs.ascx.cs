using System;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class LoggedAs : AppControl
    {
        private const string LOGGEDAS = "You are logged as <b>{0}</b> in <b>{1}</b> admin area";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            lblLoggedAs.Text = String.Format(LOGGEDAS, CurrentUser.LoginName,CurrentUser.OriginatorName);
            trlogout.Visible = CurrentUser.EffectiveCompanyId != CurrentUser.CompanyId;
        }

        protected void lbLogout_Click(object sender, EventArgs e)
        {
            
            CurrentUser.OriginatorId = CurrentUser.CompanyId;
            CurrentUser.OriginatorName = CurrentUser.CompanyName;
            Response.Redirect(String.Format(@"http://{0}/{1}?j={2}", Request.Url.Authority, CurrentUser.GetDefaultPage(), Guid.NewGuid().ToString()));
        }
    }
}