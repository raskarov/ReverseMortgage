using System;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class ChangePassword : AppControl
    {
        private bool canChangePassword = true;
        #region Events
        public event EventHandler PasswordUpdated;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if(CurrentUser == null)
            {
                Response.Redirect(Constants.LOGINPAGENAME);
            }
            else
            {
                lblPasswordRules.Text = Utils.PASSWORDRULESTEXT;
                SetControls();
            }
        }
        private void SetControls()
        {
            lblLastUpdate.Text = String.Format("Last date password was changed - {0}", CurrentUser.PasswordChangeDate.Date.ToShortDateString());
            canChangePassword = CurrentUser.PasswordChangeDate.Date.AddDays(1) < DateTime.Now;
            tbPassword.Enabled = canChangePassword;
            tbOldPassword.Enabled = canChangePassword;
            tbConfirmPassword.Enabled = canChangePassword;
            btnSetPassword.Enabled = canChangePassword;
            if (!canChangePassword)
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnSetPassword_Click(object sender, EventArgs e)
        {
            string message;
            string oldPassword = Utils.GetMD5Hash(tbOldPassword.Text);
            if (Utils.GetMD5Hash(tbPassword.Text)==CurrentUser.Password)
            {
                errMessage.Text = "Password must be different from previos 4 passwords";
            }
            else
            {
                if (!Utils.ValidatePassword(tbPassword.Text, CurrentUser.PreviosPasswords, out message))
                {
                    errMessage.Text = message;
                }
                else
                {
                    if (tbPassword.Text != tbConfirmPassword.Text)
                    {
                        errMessage.Text = Utils.PASSWORDNOTIDENTICALTEXT;
                    }
                    else if (oldPassword != CurrentUser.Password)
                    {
                        errMessage.Text = Utils.OLDPASSWORDINCORRECTTEXT;
                    }
                    else
                    {
                        CurrentUser.Password = tbPassword.Text;
                        CurrentUser.UpdatePassword();
                        SetControls();
                        errMessage.Text = Constants.SUCCESSMESSAGE;
                        if (PasswordUpdated != null)
                        {
                            PasswordUpdated(this, EventArgs.Empty);
                        }
                    }
                }
            }
       }
    }
}