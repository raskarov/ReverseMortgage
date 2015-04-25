using System;
using System.Data;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class EditVendorProfile : AppControl
    {
        private DataView dvStates;

        private DataView DvStates
        {
            get
            {
                if (dvStates == null)
                {
                    dvStates = CurrentPage.CurrentVendor.GetStates();
                }
                return dvStates;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentPage.CurrentVendor == null)
            {
                Response.Redirect(Constants.VENDORLOGINPAGE);
                return;
            }
            BindData();
        }
        private void BindData()
        {
            tbCompanyName.Text = CurrentPage.CurrentVendor.Name;
            tbCorporateAddress1.Text = CurrentPage.CurrentVendor.CorporateAddress1;
            tbCorporateAddress2.Text = CurrentPage.CurrentVendor.CorporateAddress2;
            tbCompanyPhone.Text = CurrentPage.CurrentVendor.CompanyPhone;
            tbCompanyFax.Text = CurrentPage.CurrentVendor.CompanyFax;
            tbCompanyEmail.Text = CurrentPage.CurrentVendor.CompanyEmail;
            tbCompanyCity.Text = CurrentPage.CurrentVendor.CompanyCity;
            tbCompanyZip.Text = CurrentPage.CurrentVendor.CompanyZip;
            tbBillingAddress1.Text = CurrentPage.CurrentVendor.BillingAddress1;
            tbBillingAddress2.Text = CurrentPage.CurrentVendor.BillingAddress2;
            tbBillingCity.Text = CurrentPage.CurrentVendor.BillingCity;
            tbBillingZip.Text = CurrentPage.CurrentVendor.BillingZip;
            tbPrimaryContact.Text = CurrentPage.CurrentVendor.PrimaryContactName;
            tbPCPhone1.Text = CurrentPage.CurrentVendor.PCPhone1;
            tbPCPhone1.Text = CurrentPage.CurrentVendor.PCPhone1;
            tbPCEmail.Text = CurrentPage.CurrentVendor.PCEmail;
            tbSecondaryContact.Text = CurrentPage.CurrentVendor.SecondaryContactName;
            tbSCPhone1.Text = CurrentPage.CurrentVendor.SCPhone1;
            tbSCPhone1.Text = CurrentPage.CurrentVendor.SCPhone1;
            tbSCEmail.Text = CurrentPage.CurrentVendor.SCEmail;
            ddlCompanyState.DataSource = DvStates;
            ddlCompanyState.DataTextField = "Name";
            ddlCompanyState.DataValueField = "id";
            ddlCompanyState.DataBind();
            if (CurrentPage.CurrentVendor.CompanyStateId > 0) ddlCompanyState.SelectedValue = CurrentPage.CurrentVendor.CompanyStateId.ToString();
            ddlBillingState.DataSource = DvStates;
            ddlBillingState.DataTextField = "Name";
            ddlBillingState.DataValueField = "id";
            ddlBillingState.DataBind();
            if (CurrentPage.CurrentVendor.BillingStateId > 0) ddlBillingState.SelectedValue = CurrentPage.CurrentVendor.BillingStateId.ToString();
        }
        public bool Save()
        {
            CurrentPage.CurrentVendor.Name = tbCompanyName.Text;
            CurrentPage.CurrentVendor.CorporateAddress1 = tbCorporateAddress1.Text;
            CurrentPage.CurrentVendor.CorporateAddress2 = tbCorporateAddress2.Text;
            CurrentPage.CurrentVendor.CompanyPhone = tbCompanyPhone.Text;
            CurrentPage.CurrentVendor.CompanyFax = tbCompanyFax.Text;
            CurrentPage.CurrentVendor.CompanyEmail = tbCompanyEmail.Text;
            CurrentPage.CurrentVendor.CompanyCity = tbCompanyCity.Text;
            CurrentPage.CurrentVendor.CompanyZip = tbCompanyZip.Text;
            CurrentPage.CurrentVendor.BillingAddress1 = tbBillingAddress1.Text;
            CurrentPage.CurrentVendor.BillingAddress2 = tbBillingAddress2.Text;
            CurrentPage.CurrentVendor.BillingCity = tbBillingCity.Text;
            CurrentPage.CurrentVendor.BillingZip = tbBillingZip.Text;
            CurrentPage.CurrentVendor.PrimaryContactName = tbPrimaryContact.Text;
            CurrentPage.CurrentVendor.PCPhone1 = tbPCPhone1.Text;
            CurrentPage.CurrentVendor.PCPhone1 = tbPCPhone1.Text;
            CurrentPage.CurrentVendor.PCEmail = tbPCEmail.Text;
            CurrentPage.CurrentVendor.SecondaryContactName = tbSecondaryContact.Text;
            CurrentPage.CurrentVendor.SCPhone1 = tbSCPhone1.Text;
            CurrentPage.CurrentVendor.SCPhone1 = tbSCPhone1.Text;
            CurrentPage.CurrentVendor.SCEmail = tbSCEmail.Text;
            CurrentPage.CurrentVendor.CompanyStateId = int.Parse(ddlCompanyState.SelectedValue);
            CurrentPage.CurrentVendor.BillingStateId = int.Parse(ddlBillingState.SelectedValue);
           return CurrentPage.CurrentVendor.Save()>0;
        }
    }
}