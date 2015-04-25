using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class ViewVendors : AppControl
    {
        #region constants
        private const string SORTEXPRESSION = "vendorpublicgridsort";
        private const string ASCENDING = "asc";
        private const string DESCENDING = "desc";
        public const string VENDORID = "viewvendorid";
        private const string SETTINGSPAGEINDEX = "settingspageindex";
        private const string SORTCOMMAND = "Sort";
        private const string PAGECOMMAND = "Page";
        private const int NONEAFFILIATEID = 0;
        private const int OTHERAFFILIATEID = 1;
        private const string ONCHANGEATTRIBUTE = "onchange";
        private const string ONCHANGEJS = "SetAffiliateRow(this,'{0}');";
        #endregion

        #region fields
        private DataView dvVendor;
        private VendorGlobal vendor;
        private bool needReset = false;
        private DataView dvStates;
        #endregion

        #region properties
        private DataView DvStates
        {
            get
            {
                if (dvStates == null)
                {
                    dvStates = Vendor.GetStates();
                }
                return dvStates;
            }
        }
        private int PageIndex
        {
            get
            {
                int res = 0;
                Object o = ViewState[SETTINGSPAGEINDEX];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set
            {
                ViewState[SETTINGSPAGEINDEX] = value;
            }
        }
        private int ViewMode
        {
            get
            {
                int res = Constants.VENDORVIEWGRID;
                Object o = Session[Constants.VENDORVIEW];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set
            {
                Session[Constants.VENDORVIEW] = value;
            }
        }
        private string SortExpression
        {
            get
            {
                string res = String.Empty;
                Object o = ViewState[SORTEXPRESSION];
                if (o != null)
                {
                    res = o.ToString();
                }
                return res;
            }
            set
            {
                ViewState[SORTEXPRESSION] = value;
            }
        }
        private int VendorId
        {
            get
            {
                int res = -1;
                Object o = Session[VENDORID];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set { Session[VENDORID] = value; }
        }
        private DataView DvVendor
        {
            get
            {
                if (dvVendor == null)
                {
                    dvVendor = VendorGlobal.GetVendorsList();
                }
                return dvVendor;
            }
        }
        private VendorGlobal Vendor
        {
            get
            {
                if (vendor == null)
                {
                    vendor = new VendorGlobal(VendorId);
                }
                return vendor;
            }
        }
        #endregion

        #region methods
        private void BindData()
        {
            if (ViewMode == Constants.VENDORVIEWGRID)
            {
                detailstr.Visible = false;
                buttontr.Visible = false;
                BindGrid();
            }
            else
            {
                gridtr.Visible = false;
                BindDetails();
            }            
        }
        private void BindDetails()
        {
            detailstr.Visible = true;
            buttontr.Visible = true;
            BindBasicInfo();
            if (needReset)
            {
                VendorContacts1.ResetData();
            }
            RadMultiPage1.SelectedIndex = VendorInfo.SelectedIndex;
            VendorContacts1.BindData();
            if (
                CurrentUser.IsInRoles(
                    new int[]
                        {
                            (int) AppUser.UserRoles.ProcessingManager, (int) AppUser.UserRoles.Underwriter,
                            (int) AppUser.UserRoles.UnderwritingManager, (int) AppUser.UserRoles.ClosingManager,
                            (int) AppUser.UserRoles.PostClosingManager, (int) AppUser.UserRoles.OperationsManager,
                            (int) AppUser.UserRoles.ExecutiveManager
                        }))
            {
                BindAffiliate();
                VendorInfo.Tabs[2].Visible = true;
            }
            else
            {
                VendorInfo.Tabs[2].Visible = false;
            }
        }

        private void BindAffiliate()
        {
            ddlAffiliation.Items.Clear();
            ddlAffiliation.Items.Add(new ListItem(("None"), NONEAFFILIATEID.ToString()));
            ddlAffiliation.Items.Add(new ListItem(("Other"), OTHERAFFILIATEID.ToString()));
            string affiliation = VendorGlobal.GetAffiliationForCompany(Vendor.ID, CurrentUser.CompanyId);
            string style = "";
            if (String.IsNullOrEmpty(affiliation))
            {
                style = "display:none";
                ddlAffiliation.SelectedValue = NONEAFFILIATEID.ToString();
            }
            else
            {
                style = "display:block";
                ddlAffiliation.SelectedValue = OTHERAFFILIATEID.ToString();
                tbAffiliation.Text = affiliation;
            }
            trAffiliation.Attributes.Add("style",style);
            ddlAffiliation.Attributes.Add(ONCHANGEATTRIBUTE,String.Format(ONCHANGEJS,trAffiliation.ClientID));
        }

        private void BindBasicInfo()
        {
            tbCompanyName.Text = Vendor.Name;
            tbCorporateAddress1.Text = Vendor.CorporateAddress1;
            tbCorporateAddress2.Text = Vendor.CorporateAddress2;
            tbCompanyPhone.Text = Vendor.CompanyPhone;
            tbCompanyFax.Text = Vendor.CompanyFax;
            tbCompanyEmail.Text = Vendor.CompanyEmail;
            tbCompanyCity.Text = Vendor.CompanyCity;
            tbCompanyZip.Text = Vendor.CompanyZip;
            tbBillingAddress1.Text = Vendor.BillingAddress1;
            tbBillingAddress2.Text = Vendor.BillingAddress2;
            tbBillingCity.Text = Vendor.BillingCity;
            tbBillingZip.Text = Vendor.BillingZip;
            tbPrimaryContact.Text = Vendor.PrimaryContactName;
            tbPCPhone1.Text = Vendor.PCPhone1;
            tbPCPhone2.Text = Vendor.PCPhone2;
            tbPCEmail.Text = Vendor.PCEmail;
            tbSecondaryContact.Text = Vendor.SecondaryContactName;
            tbSCPhone1.Text = Vendor.SCPhone1;
            tbSCPhone2.Text = Vendor.SCPhone2;
            tbSCEmail.Text = Vendor.SCEmail;
            if (Vendor.LicenseExpDate != null)
                dtLicenseExpdate.SelectedDate = Vendor.LicenseExpDate;
            ddlCompanyState.DataSource = DvStates;
            ddlCompanyState.DataTextField = "Name";
            ddlCompanyState.DataValueField = "id";
            ddlCompanyState.DataBind();
            ddlCompanyState.Items.Insert(0, new ListItem("- Select - ", "0"));
            if (Vendor.CompanyStateId > 0) ddlCompanyState.SelectedValue = Vendor.CompanyStateId.ToString();
            ddlBillingState.DataSource = DvStates;
            ddlBillingState.DataTextField = "Name";
            ddlBillingState.DataValueField = "id";
            ddlBillingState.DataBind();
            ddlBillingState.Items.Insert(0, new ListItem("- Select - ", "0"));
            if (Vendor.BillingStateId > 0) ddlBillingState.SelectedValue = Vendor.BillingStateId.ToString();
        }
        private void BindGrid()
        {
            gridtr.Visible = true;
            DataView dv = DvVendor;
            dv.Sort = GetSort();
            gVendors.DataSource = dv;
            gVendors.DataBind();
        }
        private string GetSort()
        {
            string res = SortExpression;
            if (!String.IsNullOrEmpty(res))
            {
                res += " " + GetSortDirection(res);
            }
            return res;
        }
        private string GetSortDirection(string sortexpression)
        {
            string res;
            Object o = ViewState["order_" + sortexpression];
            if (o != null)
            {
                res = o.ToString();
            }
            else
            {
                res = DESCENDING;
            }
            return res;
        }
        private void SetSort(string expr)
        {
            SortExpression = expr;
            string dir = GetSortDirection(expr);
            if (dir == ASCENDING)
            {
                dir = DESCENDING;
            }
            else
            {
                dir = ASCENDING;
            }
            ViewState["order_" + expr] = dir;
        }
        #endregion

        #region databinding handlers
        protected void G_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == SORTCOMMAND)
            {
                SetSort(e.CommandArgument.ToString());
            }
            else if (e.CommandName == PAGECOMMAND)
            {
                PageIndex = Convert.ToInt32(e.CommandArgument.ToString()) - 1;
            }
        }
        protected void G_SortCommand(object source, GridViewSortEventArgs e)
        {
        }
        protected void G_Sorting(object source, GridViewSortEventArgs e)
        {
        }
        protected void G_PageIndexChanged(object source, EventArgs e)
        {
        }
        protected void G_PageIndexChanging(Object source, EventArgs e)
        {
        }

        #endregion

        #region event handlers
        protected void btnBack_Click(object sender, EventArgs e)
        {
            ViewMode = Constants.VENDORVIEWGRID;
            BindData();
        }
        protected void gVendors_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            gVendors.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }
        protected void gVendors_SortCommand(object source, GridSortCommandEventArgs e)
        {
            SetSort(e.SortExpression);
            BindData();
        }
        protected void gVendors_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                ViewMode = Constants.VENDORVIEWDETAILS;
                VendorId = int.Parse(e.CommandArgument.ToString());
                VendorInfo.SelectedIndex = 0;
                needReset = true;
                BindData();
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSaveAffiliation_Click(object sender, EventArgs e)
        {
            if(ddlAffiliation.SelectedValue==OTHERAFFILIATEID.ToString())
            {
                if(VendorGlobal.SetAffiliationForCompany(Vendor.ID, CurrentUser.CompanyId, tbAffiliation.Text))
                {
                    lblMessage.Text = Constants.SUCCESSMESSAGE;
                }
            }
            BindAffiliate();
        }
    }
}