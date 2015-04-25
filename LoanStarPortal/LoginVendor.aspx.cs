using System;
using System.Web.Security;
using LoanStar.Common;

namespace LoanStarPortal
{
    public partial class LoginVendor : AppPage
    {
        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            SetFocus(tbLogin);
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
//            Vendor vendor = new Vendor();
            VendorGlobal vendor = new VendorGlobal();
            string errMsg;
            if (vendor.LoginVendor(tbLogin.Text, tbPassword.Text, out errMsg))
            {
                CurrentVendor = vendor;
                FormsAuthentication.SetAuthCookie(vendor.Login, false);
                string url = Request.QueryString["ReturnURL"];
                if (!String.IsNullOrEmpty(url))
                    Response.Redirect(url);
                else
                    Response.Redirect("~/VendorDashBoard.aspx");
            }
            else
                errMessage.Text = errMsg;
        }
        #endregion
    }
}
