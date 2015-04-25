using System;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class MyProfile : AppControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            if (!CurrentUser.HasEmail)
            {
                rtsUser.Tabs[1].Visible = false;
                rmpUser.PageViews[1].Visible = false;
            }
            else
            {
                tbUserName.Text = CurrentUser.EmailAccount.MailIncomingLogin;
                SetPasswords();
            }
        }
        private void SetPasswords()
        {
            tbMailPassword.Attributes.Add("value",CurrentUser.EmailAccount.MailIncomingPassword);
            tbMailPasswordConfirm.Attributes.Add("value", CurrentUser.EmailAccount.MailIncomingPassword);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            CurrentUser.EmailAccount.MailIncomingLogin = tbUserName.Text;
            CurrentUser.EmailAccount.MailOutgoingLogin = tbUserName.Text;
            CurrentUser.EmailAccount.MailIncomingPassword = tbMailPassword.Text;
            CurrentUser.EmailAccount.MailOutgoingPassword = tbMailPassword.Text;
            CurrentUser.EmailAccount.Update(false);
        }
    }
}