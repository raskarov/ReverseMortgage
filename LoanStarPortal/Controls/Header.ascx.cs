using System;
using System.IO;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class Header : AppControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //lblLogged.Text += CurrentUser.LastName + " " + CurrentUser.FirstName + " (" + CurrentUser.LoginName + ")";
                lblLogged.Text += CurrentUser.FirstName + " " +  CurrentUser.LastName;
            }
            rmMortgage_m1_m2.Visible = CurrentUser.HasEmail;
            spEmail.Visible = CurrentUser.HasEmail;
            divSearchLeads.Visible = CurrentUser.IsLeadManagementEnabled;
        }
        private string GetCompanyLogo()
        {
            string res = CurrentUser.LogoImage;
            if (!String.IsNullOrEmpty(res))
            {
                res = Constants.LOGOIMAGEFOLDER + "/" + res;
                string fullPath = Server.MapPath(res);
                FileInfo fi = new FileInfo(fullPath);
                if (!fi.Exists)
                {
                    res = Constants.DEFAULTLOGOIMAGE;
                }
            }
            else
            {
                res = Constants.DEFAULTLOGOIMAGE;
            }
            return res;
        }
        protected void btnSearchLead_Click(object sender, EventArgs e)
        {
            ((Default) CurrentPage).appList.SetManageLeadFilter(tbSearchLeads.Text);
        }

    }
}