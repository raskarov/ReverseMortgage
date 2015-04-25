using System;
using System.Web.Security;
using LoanStar.Common;

namespace LoanStarPortal.Administration
{
    public partial class Login : AppPage
	{
        private const string QSRETURNURL = "ReturnURL";

        protected void Page_Load(object sender, EventArgs e)
        {
            SetFocus(tbLogin);
        }
        protected void Submit_Click(object sender, EventArgs e)
        {
            AppUser user = new AppUser();
            int res = user.Login(tbLogin.Text, tbPassword.Text, Request.UserHostAddress);
            if (res == AppUser.LOGINRESULTOK)
            {
                if (user.IsAdmin || user.IsOriginatorUser)
                {
                    string url = Request.QueryString[QSRETURNURL];
                    SetTimeOffset(int.Parse(utcoffset.Value));
                    if (String.IsNullOrEmpty(url))
                    {
                        url = ResolveUrl("~/" + user.GetDefaultPage());
                    }
                    if (!user.IsPasswordExpired)
                    {
                        CurrentUser = user;
                        FormsAuthentication.SetAuthCookie(user.LoginName, false);
                        Response.Redirect(url);
                    }
                    else
                    {
                        CurrentUser = user;
                        Response.Redirect("../"+Constants.CHANGEPASSWORDPAGENAME + "?" + QSRETURNURL + "="+url);
                    }
                }
                else
                {
                    errMessage.Text = "Wrong Login or Password";
                }
            }
            else if (res == AppUser.LOGINRESULTACCOUNTLOCKED)
            {
                errMessage.Text = "Account is locked";
            }
            else
            {
                errMessage.Text = "Wrong Login or Password";
            }
        }
    }
}
