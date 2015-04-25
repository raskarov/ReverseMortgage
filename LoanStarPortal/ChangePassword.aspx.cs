using System;
using System.Web.Security;
using LoanStar.Common;

namespace LoanStarPortal
{
    public partial class ChangePassword : AppPage
    {
        private const string QSRETURNURL = "ReturnURL";

        protected void Page_Load(object sender, EventArgs e)
        {
            if(CurrentUser == null)
            {
                Response.Redirect(Constants.LOGINPAGENAME);
            }
        }
        protected void PasswordUpdated(object sender, EventArgs e)
        {
            string url = Request.QueryString[QSRETURNURL];
            CurrentUser.SaveUserLog(Request.UserHostAddress, DateTime.Now);
            FormsAuthentication.SetAuthCookie(CurrentUser.LoginName, false);
            Response.Redirect(url);
        }
    }
}
