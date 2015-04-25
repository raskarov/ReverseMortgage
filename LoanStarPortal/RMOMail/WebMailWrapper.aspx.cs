using System;
using System.Web;
using LoanStar.Common;

namespace WebMailPro
{
    public partial class WebMailWrapper : AppPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                string displayMode = Request["displayMode"];


                if (LoanStarPortal.Global.userAccount == null)
                {
                    string userEmail = CurrentUser.EmailAccount.Email;
                    string userEmailLogin = CurrentUser.MailUserName;
                    string userEmailPassword = CurrentUser.MailPassword;

                    LoanStarPortal.Global.userAccount = LoanStarPortal.Global.integration.GetAccountByMailLogin(userEmail, userEmailLogin, userEmailPassword);
                    if (LoanStarPortal.Global.userAccount != null)
                    {
                        switch (displayMode)
                        {
                            case "default":
                                {
                                    LoanStarPortal.Global.integration.UserLoginByEmail(userEmail, userEmailLogin, userEmailPassword, WMStartPage.Mailbox);
                                    break;
                                }
                            case "newmessage":
                                {
                                    LoanStarPortal.Global.integration.UserLoginByEmail(userEmail, userEmailLogin, userEmailPassword, WMStartPage.NewMessage);
                                    break;
                                }
                            case "calendar":
                                {
                                    LoanStarPortal.Global.integration.UserLoginByEmail(userEmail, userEmailLogin, userEmailPassword, WMStartPage.Calendar);
                                    break;
                                }
                        }
                    }
                }
                else
                {
                    switch (displayMode)
                    {
                        case "default":
                            {
                               Session.Remove("condition_message");
                               HttpContext.Current.Response.Redirect(string.Format(@"webmail.aspx?" + "" + @"start={0}{1}", 0, ""));
                               break;
                            }
                        case "newmessage":
                            {
                                HttpContext.Current.Response.Redirect(string.Format(@"basewebmail.aspx?scr=condition_message"));
                                break;
                            }
                        case "calendar":
                            {

                                Session.Remove("condition_message");
                                HttpContext.Current.Response.Redirect(string.Format(@"webmail.aspx?" + "" + @"start={0}{1}", 4, ""));
                                break;
                            }
                    }

                }

            }
            catch (WebMailException ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}
