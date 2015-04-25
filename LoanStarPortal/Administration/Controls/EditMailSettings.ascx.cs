using System;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditMailSettings : AppControl
    {
        //private string emailToTest = string.Empty;
        //private string EmailToTest
        //{
        //    get
        //    {
        //        if(String.IsNullOrEmpty(emailToTest))
        //        {
        //            AppUser usr = Company.GetCompanyAdmin(CurrentUser.EffectiveCompanyId, tbSMTPServer.Text);
        //            emailToTest = 
        //        }
        //    }
        //}

        #region Private methods
        private void BindData()
        {
            int companyID = CurrentUser.EffectiveCompanyId;
            Company company = new Company(companyID);
            tbPOP3Server.Text = company.POP3Server;
            tbPOP3Port.Text = company.POP3PortStr;
            tbSMTPPort.Text = company.SMTPPortStr;
            tbSMTPServer.Text = company.SMTPServer;
            lblGlobalAdmin.Text = AppSettings.AdminEmail;
        }

        private void TestMailServers()
        {
            string from;
            string to;
            string login;
            string password;
            
            if(CurrentUser.CompanyId!=CurrentUser.EffectiveCompanyId)
            {
                AppUser usr = Company.GetCompanyAdmin(CurrentUser.EffectiveCompanyId, tbSMTPServer.Text);
                if(usr != null)
                {
                    from = usr.EmailAddress;
                    to = CurrentUser.EmailAddress;
                    login = usr.EmailAccount.MailOutgoingLogin;
                    password = usr.EmailAccount.MailOutgoingPassword;
                }
                else
                {
                    lblGeneralErr.Text = "There are no company account to test SMTP server";
                    return;
                }
            }
            else
            {
                from = CurrentUser.EmailAddress;
                to = CurrentUser.EmailAddress;
                login = CurrentUser.EmailAccount.MailOutgoingLogin;
                password = CurrentUser.EmailAccount.MailOutgoingPassword;

            }
            string smtpResult;
            string pop3Result;
            WebMailHelper.TestEmailAccount(from, to, tbSMTPServer.Text, int.Parse(tbSMTPPort.Text), login, password, true ,tbPOP3Server.Text, int.Parse(tbPOP3Port.Text), out smtpResult, out pop3Result);
            lblGeneralErr.Text = String.Format("Account used for testing - {0}<br/>", from);
            if(String.IsNullOrEmpty(smtpResult))
            {
                lblGeneralErr.Text += "SMTP Server test completed successfully<br/>";
            }
            else
            {
                lblGeneralErr.Text += String.Format("SMTP Server Error: {0}<br/>", smtpResult);
            }
            if(String.IsNullOrEmpty(pop3Result))
            {
                lblGeneralErr.Text += "POP3 Server test completed successfully";
            }
            else
            {
                lblGeneralErr.Text += String.Format("POP3 Server Error: {0}", pop3Result);
            }
        }
        private void SaveMailSettings()
        {
            int companyID = CurrentUser.EffectiveCompanyId;
            Company company = new Company(companyID);
            company.POP3Server = tbPOP3Server.Text;
            company.POP3PortStr = tbPOP3Port.Text;
            company.SMTPServer = tbSMTPServer.Text;
            company.SMTPPortStr = tbSMTPPort.Text;
            try
            {
                company.Save("");
                lblGeneralErr.Text = "Operation completed successfully";
            }
            catch (Exception ex)
            {
                lblGeneralErr.Text = String.Format("Error: {0}", ex.Message);
            }
        }
        private bool ValidateData()
        {
            bool res = WebMailHelper.ValidateServer(tbSMTPServer.Text);
            if (!res)
            {
                lblErrSMTPServer.Text = "Incorrect format";
            }
            if (!WebMailHelper.ValidateServer(tbPOP3Server.Text))
            {
                res = false;
                lblErrPOP3Server.Text = "Incorrect format";
            }
            if(!WebMailHelper.ValidatePort(tbPOP3Port.Text))
            {
                res = false;
                lblErrPOP3Port.Text = "Port value should be between 0 and 65535.";
            }
            if (!WebMailHelper.ValidatePort(tbSMTPPort.Text))
            {
                res = false;
                lblErrSMTPPort.Text = "Port value should be between 0 and 65535.";
            }
            return res;
        }
        private void ClearErrors()
        {
            lblErrPOP3Server.Text = String.Empty;
            lblErrPOP3Port.Text = String.Empty;
            lblErrSMTPServer.Text = String.Empty;
            lblErrSMTPPort.Text = String.Empty;
        }
        #endregion

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ClearErrors();
//            lblInfo.Text = String.Format("Your company administrator's email address ({0}) will be used to test these email settings.",)
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        #endregion
        protected void btnTest_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                TestMailServers();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                SaveMailSettings();
            }
        }

     }
}