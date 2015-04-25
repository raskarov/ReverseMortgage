using System;
using System.Web.Security;
using LoanStar.Common;

namespace LoanStarPortal
{
    public partial class Login : AppPage
    {
        private const string QSRETURNURL = "ReturnURL";
        private const string ADMINFOLDER = "administration";
        private const string DOCPACKAGENAME = "docpackage.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
                string url = Request.QueryString[QSRETURNURL];
                if ((url != null) && (url.ToLower().IndexOf(ADMINFOLDER) > 0))
                {
                    Response.Redirect(ADMINFOLDER + "/" + Constants.LOGINPAGENAME);
                }
                else if ((url != null) && (url.ToLower().IndexOf(DOCPACKAGENAME) > 0))
                {
                    Response.Redirect(String.Format("/LoginVendor.aspx?{0}={1}", QSRETURNURL, url));
                }
                ClientScript.RegisterStartupScript(GetType(), "SetFocus", "<script>document.getElementById('" + tbLogin.ClientID + "').focus();</script>");
        }
        protected void Submit_Click(object sender, EventArgs e)
        {
            AppUser user = new AppUser();
            int res = user.Login(tbLogin.Text, tbPassword.Text, Request.UserHostAddress);
            if(res == AppUser.LOGINRESULTOK)
            {
                if (user.IsOriginatorUser)
                {
                    SetTimeOffset(int.Parse(utcoffset.Value));
                    string url = Request.QueryString[QSRETURNURL];
                    if (String.IsNullOrEmpty(url))
                    {
                        url = user.GetDefaultPage();
                    }
                    if(!user.IsPasswordExpired)
                    {
                        CurrentUser = user;
                        user.SaveUserLog(Request.UserHostAddress, DateTime.Now);

                        FormsAuthentication.SetAuthCookie(user.LoginName, false);
                        Response.Redirect(url);
                    }
                    else
                    {
                        CurrentUser = user; 
                        Response.Redirect(Constants.CHANGEPASSWORDPAGENAME + "?" + QSRETURNURL + "=" + url);
                    }
                }
                else
                {
                    errMessage.Text = "Wrong Login or Password";
                }
            }
            else if (res==AppUser.LOGINRESULTACCOUNTLOCKED)
            {
                errMessage.Text = "Account is locked";
            }
            else
            {
                errMessage.Text = "Wrong Login or Password";
            }
            //string errMsg;
            //if (user.Login(tbLogin.Text, tbPassword.Text, Request.UserHostAddress, out errMsg))
            //{
            //    if (user.IsOriginatorUser)
            //    {
            //        CurrentUser = user;
            //        user.SaveUserLog(Request.UserHostAddress, DateTime.Now);

            //        FormsAuthentication.SetAuthCookie(user.LoginName, false);
            //        SetTimeOffset(int.Parse(utcoffset.Value));
            //        string url = Request.QueryString[QSRETURNURL];
            //        if (!String.IsNullOrEmpty(url))
            //        {
            //            Response.Redirect(url);
            //        }
            //        else
            //        {
            //            Response.Redirect(user.GetDefaultPage());
            //        }

            //    }
            //    else
            //    {
            //        errMessage.Text = "Wrong Login or Password";
            //    }
            //}
            //else
            //{
            //    errMessage.Text = errMsg;
            //}
        }
    }
}
