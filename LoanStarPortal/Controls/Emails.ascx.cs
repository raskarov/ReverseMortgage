using System;
using System.Configuration;
using System.Web.UI;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class Emails : AppControl , IPostBackEventHandler
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!CurrentUser.HasEmail)
            {
                return;
            }
            if (Global.integration == null)
                 Global.integration = new WebMailPro.Integration(ConfigurationManager.AppSettings["dataFolderPath"], @".");

                //Company company = new Company(CurrentUser.CompanyId);

                //string userEmail = CurrentUser.EmailAccount.Email;
                //string userEmailLogin = CurrentUser.EmailAccount.MailIncomingLogin;
                //string userEmailPassword = CurrentUser.EmailAccount.MailIncomingPassword;
                //string emailPopServer = company.POP3Server;
                //string emailPopPort = company.POP3PortStr;
                //string emailSMTP = company.SMTPServer;
                //string emailSMTPPort = company.SMTPPortStr;
                //string emailPPOPSSL = company.POP3SSL.ToString();
                //string emailSMTPLogin = company.SMTPUserID;
                //string emailSMTPPassword = company.SMTPPassword;

                //string sWebMailHost = "RMOMail/webmailwrapper.aspx";

                //sWebMailHost = sWebMailHost +
                //                                "?userEmail=" + userEmail +
                //                                "&userEmailLogin=" + userEmailLogin +
                //                                "&userEmailPassword=" + userEmailPassword +
                //                                "&emailPopServer=" + emailPopServer +
                //                                "&emailPopPort=" + emailPopPort +
                //                                "&emailSMTP=" + emailSMTP +
                //                                "&emailSMTPPort=" + emailSMTPPort +
                //                                "&emailPPOPSSL=" + emailPPOPSSL +
                //                                "&emailSMTPLogin=" + emailSMTPLogin +
                //                                "&emailSMTPPassword=" + emailSMTPPassword +
                //                                "&displayMode=default";

             string sWebMailHost = "RMOMail/webmailwrapper.aspx?displayMode=default";
             TopPane.ContentUrl = sWebMailHost;

        }

        public void RaisePostBackEvent(string eventArgument)
        {
        }
    }
}