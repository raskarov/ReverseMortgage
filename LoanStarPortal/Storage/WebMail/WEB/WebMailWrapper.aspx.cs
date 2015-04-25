using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WebMailPro
{
    public partial class WebMailWrapper : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Integration integration = new Integration(ConfigurationManager.AppSettings["dataFolderPath"], @".");
            try
            {
                //request string is used until a soap interface exist between the two products
                //this method would actaully make a Web Service call and get the fields below.
                string userEmail = this.Request["userEmail"];
                string userEmailLogin = this.Request["userEmailLogin"];
                string userEmailPassword = this.Request["userEmailPassword"];
                string emailPopServer = this.Request["emailPopServer"];
                string emailPopPort = this.Request["emailPopPort"];
                string emailSMTP = this.Request["emailSMTP"];
                string emailSMTPPort = this.Request["emailSMTPPort"];
                string emailPPOPSSL = this.Request["emailPPOPSSL"];
                string emailSMTPLogin = this.Request["emailSMTPLogin"];
                string emailSMTPPassword = this.Request["emailSMTPPassword"];

                Account userAccount = integration.GetAccountByMailLogin(userEmail, userEmailLogin, userEmailPassword);
                if (userAccount != null)
                    integration.UserLoginByEmail(userEmail, userEmailLogin, userEmailPassword,WMStartPage.Mailbox);
                else
                {
                    userAccount = new Account();
                    userAccount.Email = userEmail;
                    userAccount.MailIncomingHost = emailPopServer;
                    userAccount.MailIncomingLogin = userEmailLogin;
                    userAccount.MailIncomingPassword = userEmailPassword;
                    userAccount.MailIncomingPort = Convert.ToInt32(emailPopPort);
                    userAccount.MailIncomingProtocol = IncomingMailProtocol.Pop3;
                    userAccount.MailOutgoingAuthentication = Convert.ToBoolean(emailPPOPSSL);
                    userAccount.MailOutgoingHost = emailSMTP;
                    userAccount.MailOutgoingLogin = emailSMTPLogin;
                    userAccount.MailOutgoingPassword = emailSMTPPassword;
                    userAccount.MailOutgoingPort = Convert.ToInt32(emailSMTPPort);
                    userAccount.MailMode = MailMode.LeaveMessagesOnServer;
                    userAccount.GetMailAtLogin = true;

                    integration.CreateUserFromAccount(userAccount);

                    integration.UserLoginByEmail(userEmail, userEmailLogin, userEmailPassword, WMStartPage.Mailbox);


                }
            }
            catch (WebMailException ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}
