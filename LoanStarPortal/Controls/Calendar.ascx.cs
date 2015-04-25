using System;
using System.Web.Security;
using System.Web.UI;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class Calendar : AppControl, IPostBackEventHandler
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //Company company = new Company(this.CurrentUser.CompanyId);

            //string userEmail = this.CurrentUser.EmailAccount.Email;
            //string userEmailLogin = this.CurrentUser.MailUserName;
            //string userEmailPassword = this.CurrentUser.MailPassword;
            //string emailPopServer = company.POP3Server;
            //string emailPopPort = company.POP3PortStr;
            //string emailSMTP = company.SMTPServer;
            //string emailSMTPPort = company.SMTPPortStr;
            //string emailPPOPSSL = company.POP3SSL.ToString();
            //string emailSMTPLogin = company.SMTPUserID;
            //string emailSMTPPassword = company.SMTPPassword;

            //string sWebMailHost = "RMOMail/webmailwrapper.aspx";

            //sWebMailHost = sWebMailHost + "?userEmail=" + userEmail +
            //                    "&userEmailLogin=" + userEmailLogin +
            //                    "&userEmailPassword=" + userEmailPassword +
            //                    "&emailPopServer=" + emailPopServer +
            //                    "&emailPopPort=" + emailPopPort +
            //                    "&emailSMTP=" + emailSMTP +
            //                    "&emailSMTPPort=" + emailSMTPPort +
            //                    "&emailPPOPSSL=" + emailPPOPSSL +
            //                    "&emailSMTPLogin=" + emailSMTPLogin +
            //                    "&emailSMTPPassword=" + emailSMTPPassword +
            //                    "&displayMode=calendar";

            string sWebMailHost = "RMOMail/webmailwrapper.aspx?displayMode=calendar";
            this.TopPane8.ContentUrl = sWebMailHost;

        }

        public void RaisePostBackEvent(string eventArgument)
        {
        }
    }
}