using System;
using System.Web.UI.WebControls;
using System.Data;
using LoanStar.Common;

namespace LoanStarPortal
{
    public partial class CreateMortgage : AppPage
    {
        private const string CLOSEANDREBIND = "<script type=\"text/javascript\">CloseAndRebind({0})</script>";
        private const string CANCELEDIT = "<script type=\"text/javascript\">CancelEdit()</script>";
        private const string PARAMNAME = "par";
        private const string NEWMORTGAGE = "mortgage";
        private const int LEADSTATUSID = 1;
        private const int APPLICATIONSTATUSID = 3;
        public const string NEWBORROWERID = "newborrowerid";
        private const int LENDERROLEID = 2;
        private const string NEWMORTGAGEEVENTDESC = "New mortgage created by {0} at {1}";
        private const int MORTGAGECREATEDEVENTTYPEID = 34;
        private const string ONCHANGEATTRIBUTE = "onchange";
        private const string JSCLOSINGCOSTSELECT = "SetStateCountyRows(this,'{0}','{1}');";

        private bool newMortgage;
        private int statusId = 0;
        private int lenderId = 0;

        private int SelectedStateId
        {
            get
            {
                int res = 0;
                Object o = ViewState["selectedstate"];
                if(o!=null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set { ViewState["selectedstate"] = value; }
        }
        private int SelectedCountyId
        {
            get
            {
                int res = 0;
                Object o = ViewState["selectedcounty"];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set { ViewState["selectedcounty"] = value; }
        }
        protected int MortgageProfileID
        {
            get
            {
                if (Session[Constants.MortgageID] == null)
                    Session[Constants.MortgageID] = 0;
                return Convert.ToInt32(Session[Constants.MortgageID].ToString());
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            ClearErrors();
            ddlClosinCost.Attributes.Add(ONCHANGEATTRIBUTE, String.Format(JSCLOSINGCOSTSELECT, trState.ClientID, trCounty.ClientID));
            string param  = GetValue(PARAMNAME,NEWMORTGAGE);
            Page.Header.Title = "RM LOS";
            if (param == NEWMORTGAGE)
            {
                Page.Header.Title += " New mortgage";
                newMortgage = true;
            }
            else
            {
                Page.Header.Title += " New borrower";
                trStatus.Visible = false;
                trGoTocalculator.Visible = false;
                trHeaderLabel.Visible = false;
                trHomeValue.Visible = false;
                trClosingCost.Visible = false;
                trLender.Visible = false;
                trProduct.Visible = false;
                trPayoff.Visible = false;
                trStatus.Visible = false;
                trCounty.Visible = false;
            }
            if (!IsPostBack)
            {
                tbFirstName.Focus();
                BindClosingCost();
                BindLender();
                BindProduct();
                BindStatus();
                BindState();
                BindCounty();
            }
            else
            {
                ProcessPostBack();
            }
        }
        private void ProcessPostBack()
        {
            string controlName = Page.Request["__EVENTTARGET"];
            if (!String.IsNullOrEmpty(controlName))
            {
                if(controlName.EndsWith("ddlState"))
                {
                    SelectedStateId = GetDdlSelectedValue(controlName.Replace(":", "$"));
                    trState.Attributes.Add("style","display:table-row");
                    trCounty.Attributes.Add("style", "display:table-row");
                    BindCounty();
                }
            }
        }
        private int GetDdlSelectedValue(string controlName)
        {
            int res = -1;
            try
            {
                res = int.Parse(Page.Request.Form[controlName]);
            }
            catch
            {
            }
            return res;
        }
        private void BindState()
        {
            ddlState.DataSource = DataHelpers.GetStateList();
            ddlState.DataTextField = "Name";
            ddlState.DataValueField = "Id";
            ddlState.DataBind();
            ddlState.Items.Add(new ListItem("- Select -","0"));
            if(SelectedStateId>0)
            {
                ddlState.SelectedValue = SelectedStateId.ToString();
            }
        }

        private void BindCounty()
        {
            if (SelectedStateId > 0)
            {
                ddlCounty.DataSource = DataHelpers.GetCountiesForState(SelectedStateId);
                ddlCounty.DataTextField = "Name";
                ddlCounty.DataValueField = "Id";
                ddlCounty.DataBind();
                ddlCounty.Items.Insert(0,new ListItem("- Select -", "0"));
                if(SelectedCountyId>0)
                {
                    ddlCounty.SelectedValue = SelectedCountyId.ToString();
                }
            }
            else
            {
                ddlCounty.Items.Add(new ListItem("- Select -", "0"));
            }
        }

        private void ClearErrors()
        {
            lblFirstNameErr.Visible = false;
            lblLastNameErr.Visible = false;
            lblStatusErr.Visible = false;
            lblBirthDateErr.Visible = false;
            lblErrState.Visible = false;
            lblErrCounty.Visible = false; 
        }
        private void BindClosingCost()
        {
            ddlClosinCost.DataSource = ClosingCostProfile.GetClosingCostProfileList(CurrentUser.CompanyId);
            ddlClosinCost.DataTextField = "name";
            ddlClosinCost.DataValueField = "id";
            ddlClosinCost.DataBind();
            ddlClosinCost.Items.Insert(0,new ListItem("- Select- ","0"));
        }
        private void BindLender()
        {
            DataView dv = Company.GetCompanyAffiliateByRole(CurrentUser.CompanyId,LENDERROLEID);
            dv.RowFilter = "StatusId=1";
            ddlLender.DataSource = dv;
            ddlLender.DataTextField = "companyname";
            ddlLender.DataValueField = "affiliatecompanyid";
            ddlLender.DataBind();
            ddlLender.Items.Insert(0, new ListItem("- Select- ", "0"));
            if(lenderId>0)
            {
                ListItem li = ddlLender.Items.FindByValue(lenderId.ToString());
                if (li != null) li.Selected = true;
            }
        }
        private void BindProduct()
        {
            if (lenderId > 0)
            {
                DataView dv = Lender.GetLenderProductList(lenderId);
                dv.RowFilter = "Selected=1";
                ddlProduct.DataSource = dv;
                ddlProduct.DataTextField = "name";
                ddlProduct.DataValueField = "id";
                ddlProduct.DataBind();
            }
            ddlProduct.Items.Insert(0, new ListItem("- Select- ", "0"));
            ddlProduct.Enabled = lenderId > 0;
        }
        private void BindStatus()
        {
            DataView dv = MortgageProfile.GetNewLoanStatusList();
            if(!CurrentUser.IsLeadManagementEnabled)
            {
                dv.RowFilter = "id<>17";
            }
            ddlStatus.DataSource = dv;
            ddlStatus.DataTextField = "name";
            ddlStatus.DataValueField = "id";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0,new ListItem("-Select-","0"));
        }

        private bool ValidateData()
        {
            bool res = true;
            if (String.IsNullOrEmpty(tbFirstName.Text.Trim()))
            {
                lblFirstNameErr.Visible = true;
                res = false;
            }
            if (String.IsNullOrEmpty(tbLastName.Text.Trim()))
            {
                lblLastNameErr.Visible = true;
                res = false;
            }
            if(ddlClosinCost.SelectedValue!="0")
            {
                SelectedStateId = int.Parse(ddlState.SelectedValue);
                if (SelectedStateId == 0)
                {
                    lblErrState.Visible = true;
                    res = false;
                }
                SelectedCountyId = int.Parse(ddlCounty.SelectedValue);
                if (SelectedCountyId == 0)
                {
                    lblErrCounty.Visible = true;
                    res = false;
                }
            }
            statusId = int.Parse(ddlStatus.SelectedValue);
            if(newMortgage)
            {
                if (statusId == 0)
                {
                    lblStatusErr.Visible = true;
                    res = false;
                }
                else if(statusId==APPLICATIONSTATUSID)
                {
                    res = diYBBirthDate.SelectedDate != null;
                    if (res)
                    {
                        DateTime birthDate = ((DateTime)diYBBirthDate.SelectedDate).Date;
                        int yearAge = -1;
                        if (birthDate <= DateTime.Now.Date)
                            yearAge = new DateTime((DateTime.Now.Date - birthDate).Ticks).Year;

                        lblBirthDateErr.Visible = yearAge < 62 || yearAge > 100;
                        res = !lblBirthDateErr.Visible;
                    }
                    else
                    {
                        lblBirthDateErr.Visible = true;
                    }
                }
            }
            //if (rbLead.Checked)
            //{
            //    statusId = LEADSTATUSID;
            //}
            //else if (trStatus.Visible)
            //{
            //    if(rbAppliaction.Checked)
            //    {
            //        statusId = APPLICATIONSTATUSID;
            //    }
            //    if (statusId == 0)
            //    {
            //        lblStatusErr.Visible = true;
            //        res = false;
            //    }
            //}
            //if (rbAppliaction.Checked)
            //{
            //    res = diYBBirthDate.SelectedDate != null;
            //    if(res)
            //    {
            //        DateTime birthDate = ((DateTime)diYBBirthDate.SelectedDate).Date;
            //        int yearAge = -1;
            //        if (birthDate <= DateTime.Now.Date)
            //            yearAge = new DateTime((DateTime.Now.Date - birthDate).Ticks).Year;

            //        lblBirthDateErr.Visible = yearAge < 62 || yearAge > 100;
            //        res = !lblBirthDateErr.Visible;
            //    }
            //    else
            //    {
            //        lblBirthDateErr.Visible = true;
            //    }
            //}
            return res;
        }
        private void CreateEvent(int mortgageId)
        {
            Event.Save(mortgageId, MORTGAGECREATEDEVENTTYPEID,
                       String.Format(NEWMORTGAGEEVENTDESC, CurrentUser.FullName, DateTime.Now));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbFirstName.Text = "";
            tbLastName.Text = "";
            tbPayoffAmount.Text = "";
            InjectScript.Text = CANCELEDIT;
            if(newMortgage)
            {
                ddlStatus.ClearSelection();
                //rbLead.Checked = false;
                //rbAppliaction.Checked = false;
            }
        }
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedStateId = int.Parse(ddlState.SelectedValue);
            string style = "display:" + (SelectedStateId > 0 ? "block" : "none");
            trState.Attributes.Add("style", style);
            trCounty.Attributes.Add("style", style);
            BindCounty();
        }

        protected void ddlLender_SelectedIndexChanged(object sender, EventArgs e)
        {
            lenderId = int.Parse(ddlLender.SelectedValue);
            BindProduct();
        }
        protected void btnOk_Click(object sender, EventArgs e)
        {
            if(ValidateData())
            {
                if (newMortgage)
                {
                    MortgageProfile mp = new MortgageProfile();
                    mp.CurProfileStatusID = statusId;
                    mp.CurrentUserId = CurrentUser.Id;
                    Borrower borrower = mp.Borrowers[0];
                    borrower.FirstName = tbFirstName.Text;
                    borrower.LastName = tbLastName.Text;
                    borrower.DateOfBirth = diYBBirthDate.SelectedDate;
                    mp.Borrowers.Add(borrower);
                    int res = mp.CreateNew(CurrentUser);
                    if (res > 0)
                    {
                        mp = GetMortgage(res);
                        string errMessage;
                        Property property = mp.Property;
                        if (tbHomeValue.Value != null)
                        {
                            mp.UpdateObject("Property.SPValue", tbHomeValue.Value.ToString(), property.ID, out errMessage);
                        }
                        if(tbPayoffAmount.Value!=null)
                        {
                            mp.AddPayoff((decimal)tbPayoffAmount.Value);
                        }
                        int closingCostProfileId = int.Parse(ddlClosinCost.SelectedValue);
                        if(closingCostProfileId>0)
                        {
                            mp.UpdateObject("Property.StateId", SelectedStateId.ToString(), property.ID, out errMessage);
                            mp.UpdateObject("Property.CountyID", SelectedCountyId.ToString(), property.ID, out errMessage);
                            mp.UpdateObject("MortgageInfo.ClosingCostProfileId", closingCostProfileId.ToString(), mp.MortgageInfo.ID, out errMessage);
                        }
                        lenderId = int.Parse(ddlLender.SelectedValue);
                        if (lenderId>0)
                        {
                            mp.UpdateObject("MortgageInfo.LenderAffiliateID", lenderId.ToString(), mp.MortgageInfo.ID, out errMessage);
                            int productId = int.Parse(ddlProduct.SelectedValue);
                            if(productId>0)
                            {
                                mp.UpdateObject("MortgageInfo.ProductId", productId.ToString(), mp.MortgageInfo.ID, out errMessage);
                            }
                        }
                        CreateEvent(mp.ID);
                        RemoveFromCacheByKey(MORTGAGE+"_"+mp.ID);
                        tbFirstName.Text = "";
                        tbLastName.Text = "";
                        tbHomeValue.Text = "";
                        diYBBirthDate.SelectedDate = null;
                    }
                    InjectScript.Text = String.Format(CLOSEANDREBIND, res);
                    tbPayoffAmount.Text = "";
                    //rbLead.Checked = false;
                    //rbAppliaction.Checked = false;
                    ddlClosinCost.ClearSelection();
                    if(cbGotoCalculator.Checked)
                    {
                        Session[Constants.GOTOCALCULATOR] = true;
                    }
                    Session[Constants.NEWMORTGAGECREATED] = res;
                }
                else
                {
                    MortgageProfile mp = GetMortgage(MortgageProfileID);
                    Borrower borrower = new Borrower(mp);
                    borrower.FirstName = tbFirstName.Text;
                    borrower.LastName = tbLastName.Text;
                    borrower.DateOfBirth = diYBBirthDate.SelectedDate;
                    mp.Borrowers.Add(borrower);
                    borrower.ID = mp.SaveNewBorrower(CurrentUser.Id, borrower);
                    if (borrower.ID > 0)
                    {
                        tbFirstName.Text = "";
                        tbLastName.Text = "";
                        diYBBirthDate.SelectedDate = null;
                        if(borrower.ID==mp.YoungestBorrower.ID)
                        {
                            mp.UpdateBirthDateCampaigns();
                        }
                    }
                    Session[NEWBORROWERID] = borrower.ID;
                    InjectScript.Text = String.Format(CLOSEANDREBIND, mp.ID);
                }
            }
            else
            {
                InjectScript.Text = "";
            }
        }
    }
}
