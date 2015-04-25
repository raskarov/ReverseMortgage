using System;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class ChangeVendorPassword : AppControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentPage.CurrentVendor == null)
            {
                Response.Redirect(Constants.VENDORLOGINPAGE);
                return;
            }
            ClearErrors();
        }
        private void ClearErrors()
        {
            lblErrPassword.Text = "";
            lblErrConfirmPassword.Text = "";
            lblErrOldPassword.Text = "";
        }
        public bool ValidateAndSave(out string errMessage)
        {
            bool res = true;
            errMessage = "";
            if(tbNewPassword.Text.Length<6)
            {
                res = false;
                lblErrPassword.Text = "Password should be at least 6 character long.";
            }
            else if(tbConfirmPassword.Text!=tbNewPassword.Text)
            {
                res = false;
                lblErrConfirmPassword.Text = "Passwords you have typed not identical";
            }
            else if(tbOldPassword.Text != CurrentPage.CurrentVendor.Password)
            {
                res = false;
                lblErrOldPassword.Text = "Wrong password";
            }
            if(res)
            {
                res = CurrentPage.CurrentVendor.ChangePassword(tbNewPassword.Text);
                if(!res) errMessage = "Can't save password";
            }
            return res;
        }
        //private void goBack()
        //{
        //    ((VendorDashBoard) CurrentPage).LoadedControlName = "";
        //    Response.Redirect(Constants.VENDORLOGINPAGE);
        //}
    }
}