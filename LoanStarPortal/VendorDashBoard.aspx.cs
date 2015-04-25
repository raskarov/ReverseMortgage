using System;
using System.Web.UI;
using System.Web.Security;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal
{
    public partial class VendorDashBoard : AppPage
    {
        
        private const string CURRENTCONTROLNAME = "currentcontrolname";
        private const string VIEWVENDORORDERCONTROL = "ViewVendorOrders.ascx";
        private const string EDITVENDORPROFILECONTROL = "EditVendorProfile.ascx";
        private const string CHANGEVENDORPASSSOWRDCONTROL = "ChangeVendorPassword.ascx";


        private Control ctl;

        public string LoadedControlName
        {
            get
            {
                string res = VIEWVENDORORDERCONTROL;
                Object o = ViewState[CURRENTCONTROLNAME];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    res = o.ToString();
                }
                return res;
            }
            set { ViewState[CURRENTCONTROLNAME] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentVendor == null)
            {
                Response.Redirect(Constants.VENDORLOGINPAGE);
                return;
            }
            lblMessage.Text = "";
            BindData();
        }
        private void BindData()
        {
            phVendorControl.Controls.Clear();
            ctl = LoadControl(Constants.FECONTROLSLOCATION + LoadedControlName);
            if (ctl != null)
            {
                phVendorControl.Controls.Add(ctl);
                lblName.Text = CurrentVendor.Name;
            }
            trSave.Visible = LoadedControlName != VIEWVENDORORDERCONTROL;
        }
        protected void rmVendor_ItemClick(object sender, RadMenuEventArgs e)
        {
            if (e.Item.Value == "EditProfile")
            {
                LoadedControlName = EDITVENDORPROFILECONTROL;
            }
            else if (e.Item.Value == "ChangePassword")
            {
                LoadedControlName = CHANGEVENDORPASSSOWRDCONTROL;
            }
            BindData();
        }
        protected void lbLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Response.Redirect(Constants.VENDORLOGINPAGE);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (ctl is LoanStarPortal.Controls.ChangeVendorPassword)
            {
                string errMessage;
                if (((LoanStarPortal.Controls.ChangeVendorPassword) ctl).ValidateAndSave(out errMessage))
                {
                    lblMessage.Text = Constants.SUCCESSMESSAGE;
                }
                else
                {
                    lblMessage.Text = errMessage;
                }
            }
            else if (ctl is LoanStarPortal.Controls.EditVendorProfile)
            {
                if (((LoanStarPortal.Controls.EditVendorProfile)ctl).Save())
                {
                    lblMessage.Text = Constants.SUCCESSMESSAGE;
                }
                else
                {
                    lblMessage.Text = "Can't save data";
                }

            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            LoadedControlName = VIEWVENDORORDERCONTROL;
            BindData();
        }
    }
}
