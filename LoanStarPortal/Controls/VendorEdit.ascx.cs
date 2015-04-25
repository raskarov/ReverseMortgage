using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class VendorEdit : EditGridFormControl
    {
        #region fields
        private Vendor vendor;
        private string objectName;
        private DataView dvVendorType;
        private const string TITLESCOMPANY = "8";
        private const string NOTARY = "4";
        #endregion

        #region properties
        public override string ObjectName
        {
            get { return objectName; }
            set { objectName = value; }
        }
        public override object EditObject
        {
            set { vendor = value as Vendor; }
        }
        protected DataView DvVendorType
        {
            get
            {
                //if (dvVendorType == null)
                //{
                //    dvVendorType = Vendor.GetVendorTypeList();
                //}
                return dvVendorType;
            }
        }
        #endregion

        #region Methods
        private void AddEmptyItem(DropDownList ddl)
        {
            ListItem li = new ListItem(" Select ", "");
            ddl.Items.Insert(0, li);
        }

        private void BindDropDown()
        {
            ddlType.DataSource = DvVendorType;
            ddlType.DataTextField = "Name";
            ddlType.DataValueField = "ID";
            ddlType.DataBind();
            AddEmptyItem(ddlType);
        }
        public override void BindData()
        {
            //BindDropDown();
            //PanelTitleCompanies.Visible = false;
            //if (vendor != null && !String.IsNullOrEmpty(vendor.FirstName))
            //{
            //    //ddlType.SelectedIndex = 
            //    ddlType.Items.FindByValue(vendor.VendorTypeId.ToString()).Selected = true;
            //    if (vendor.VendorTypeId.ToString() == TITLESCOMPANY || vendor.VendorTypeId.ToString() == NOTARY)
            //        PanelTitleCompanies.Visible = true;
            //    CompanyName.Text = vendor.CompanyName;
            //    FirstName.Text = vendor.FirstName;
            //    LastName.Text = vendor.LastName; 
            //    Address.Text = vendor.Address;
            //    PhoneNumber.Text = vendor.PhoneNumber;
            //    AltPhoneNumber.Text = vendor.AltPhoneNumber;
            //    FaxNumber.Text = vendor.FaxNumber;
            //    AltFaxNumber.Text = vendor.AltFaxNumber;
            //    Email.Text = vendor.MailAddress;
            //    AltEmail.Text = vendor.AltMailAddress;
            //    Login.Text = vendor.LoginName;
            //    Password.Text = vendor.Password;
            //    chkDisabled.Checked = vendor.IsDisabled;
            //}
        }
        #endregion

        #region Event Handlers
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (vendor.ID <= 0 && PanelTitleCompanies.Visible)
            {
                if(!Vendor.CheckLogin(Login.Text))
                {
                    lblExists.Visible = true;
                }
                rfvPassword.Enabled = true;
                rfvPassword.Validate();
                if (!Page.IsValid)
                {
                    rfvPassword.Visible = true;
                    rfvPassword.Focus();
                }
                rfvPassword.IsValid = false;
            }
            else
            {
                ArrayList logs = new ArrayList();
                int vendorTypeId = Convert.ToInt32(ddlType.SelectedValue);
                //if (vendor.VendorTypeId != vendorTypeId)
                //{
                //    logs.Add(
                //        new MortgageLogEntry(ObjectName, vendor.ID, ObjectName + ".VendorTypeId",
                //                             vendor.VendorTypeId.ToString(), vendorTypeId.ToString(),
                //                             CurrentPage.CurrentUser.Id));
                //    vendor.VendorTypeId = vendorTypeId;
                //}

                string companyName = CompanyName.Text;
                if (vendor.CompanyName != companyName)
                {
                    logs.Add(
                        new MortgageLogEntry(ObjectName, vendor.ID, ObjectName + ".CompanyName", vendor.CompanyName,
                                             companyName, CurrentPage.CurrentUser.Id));
                    vendor.CompanyName = companyName;
                }
                string firstName = FirstName.Text;
                if (vendor.FirstName != firstName)
                {
                    logs.Add(
                        new MortgageLogEntry(ObjectName, vendor.ID, ObjectName + ".FirstName", vendor.FirstName,
                                             firstName, CurrentPage.CurrentUser.Id));
                    vendor.FirstName = firstName;
                }
                string lastName = LastName.Text;
                if (vendor.LastName != lastName)
                {
                    logs.Add(
                        new MortgageLogEntry(ObjectName, vendor.ID, ObjectName + ".LastName", vendor.LastName, lastName,
                                             CurrentPage.CurrentUser.Id));
                    vendor.LastName = lastName;
                }
                string address = Address.Text;
                if (vendor.Address != address)
                {
                    logs.Add(
                        new MortgageLogEntry(ObjectName, vendor.ID, ObjectName + ".Address", vendor.Address, address,
                                             CurrentPage.CurrentUser.Id));
                    vendor.Address = address;
                }


                if (!String.IsNullOrEmpty(PhoneNumber.Text))
                {
                    string phoneNumber = PhoneNumber.Text;
                    if (vendor.PhoneNumber != phoneNumber)
                    {
                        logs.Add(
                            new MortgageLogEntry(ObjectName, vendor.ID, ObjectName + ".PhoneNumber", vendor.PhoneNumber,
                                                 phoneNumber, CurrentPage.CurrentUser.Id));
                        vendor.PhoneNumber = phoneNumber;
                    }
                }
                if (!String.IsNullOrEmpty(AltPhoneNumber.Text))
                {
                    string altPhoneNumber = AltPhoneNumber.Text;
                    if (vendor.AltPhoneNumber != altPhoneNumber)
                    {
                        logs.Add(
                            new MortgageLogEntry(ObjectName, vendor.ID, ObjectName + ".AltPhoneNumber",
                                                 vendor.AltPhoneNumber, altPhoneNumber, CurrentPage.CurrentUser.Id));
                        vendor.AltPhoneNumber = altPhoneNumber;
                    }
                }
                if (!String.IsNullOrEmpty(FaxNumber.Text))
                {
                    string faxNumber = FaxNumber.Text;
                    if (vendor.FaxNumber != faxNumber)
                    {
                        logs.Add(
                            new MortgageLogEntry(ObjectName, vendor.ID, ObjectName + ".FaxNumber", vendor.FaxNumber,
                                                 faxNumber, CurrentPage.CurrentUser.Id));
                        vendor.FaxNumber = faxNumber;
                    }
                }
                if (!String.IsNullOrEmpty(AltFaxNumber.Text))
                {
                    string altFaxNumber = AltFaxNumber.Text;
                    if (vendor.AltFaxNumber != altFaxNumber)
                    {
                        logs.Add(
                            new MortgageLogEntry(ObjectName, vendor.ID, ObjectName + ".AltFaxNumber",
                                                 vendor.AltFaxNumber, altFaxNumber, CurrentPage.CurrentUser.Id));
                        vendor.AltFaxNumber = altFaxNumber;
                    }
                }
                if (!String.IsNullOrEmpty(Email.Text))
                {
                    string email = Email.Text;
                    if (vendor.MailAddress != email)
                    {
                        logs.Add(
                            new MortgageLogEntry(ObjectName, vendor.ID, ObjectName + ".MailAddress", vendor.MailAddress,
                                                 email, CurrentPage.CurrentUser.Id));
                        vendor.MailAddress = email;
                    }
                }
                if (!String.IsNullOrEmpty(AltEmail.Text))
                {
                    string altEmail = AltEmail.Text;
                    if (vendor.AltMailAddress != altEmail)
                    {
                        logs.Add(
                            new MortgageLogEntry(ObjectName, vendor.ID, ObjectName + ".AltMailAddress",
                                                 vendor.AltMailAddress, altEmail, CurrentPage.CurrentUser.Id));
                        vendor.AltMailAddress = altEmail;
                    }
                }
                if (!String.IsNullOrEmpty(Login.Text))
                {
                    string login = Login.Text;
                    if (vendor.LoginName != login)
                    {
                        logs.Add(
                            new MortgageLogEntry(ObjectName, vendor.ID, ObjectName + ".LoginName", vendor.LoginName,
                                                 login, CurrentPage.CurrentUser.Id));
                        vendor.LoginName = login;
                    }
                }
                if (!String.IsNullOrEmpty(Password.Text))
                {
                    string password = Password.Text;
                    if (vendor.Password != password)
                    {
                        logs.Add(
                            new MortgageLogEntry(ObjectName, vendor.ID, ObjectName + ".Password", vendor.Password,
                                                 password, CurrentPage.CurrentUser.Id));
                        vendor.Password = password;
                    }
                }
                if (PanelTitleCompanies.Visible)
                {
                    bool isDisabled = chkDisabled.Checked;
                    if (vendor.IsDisabled != isDisabled)
                    {
                        logs.Add(
                            new MortgageLogEntry(ObjectName, vendor.ID, ObjectName + ".IsDisabled", vendor.IsDisabled.ToString(),isDisabled.ToString(), CurrentPage.CurrentUser.Id));
                        vendor.IsDisabled = isDisabled;
                    }
                }
                if (vendor.ID <= 0)
                    logs.Clear();
                vendor.CompanyID = CurrentUser.CompanyId;
                Save(vendor, logs);
            }
        }
        #endregion

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlType.SelectedValue == TITLESCOMPANY || ddlType.SelectedValue == NOTARY)
            {
                PanelTitleCompanies.Visible = true;
                rfvPassword.Enabled = true;
            }
            else
            {
                PanelTitleCompanies.Visible = false;
                rfvPassword.Enabled = false;
            }
        }
    }
}