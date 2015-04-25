using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class AdvancedCalculatorValues : AppControl
    {
        #region Events
        public event EventHandler ValidateFirst;
        #endregion

        #region Properties
        private LoanStar.Common.AdvancedCalculator advCalc = null;

        public LoanStar.Common.AdvancedCalculator AdvancedCalculator
        {
            get { return advCalc; }
            set { advCalc = value; }
        }

        public bool FirstLoad
        {
            get
            {
                Object o = ViewState["FirstLoadAdvCalcValues"];
                bool res = true;
                if (o != null)
                {
                    try
                    {
                        res = bool.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set
            {
                ViewState["FirstLoadAdvCalcValues"] = value;
            }
        }

        private Borrower YoungestBorrower
        {
            get
            {
                Borrower youngestBorrower = AdvancedCalculator.Owner.YoungestBorrower;
                if (youngestBorrower == null)
                    youngestBorrower = AdvancedCalculator.Owner.Borrowers[0];
                return youngestBorrower;
            }
        }
        #endregion

        #region Methods
        private void BindData()
        {
            diYBBirthDate.SelectedDate = YoungestBorrower.DateOfBirth;
            tbHomeValue.Text = AdvancedCalculator.Owner.Property.SPValue == null ? 
                String.Empty : AdvancedCalculator.Owner.Property.SPValue.ToString();

//            int lenderAffiliateID = advCalc.Owner.MortgageInfo.LenderAffiliateID;
//            int companyID = CurrentPage.CurrentUser.CompanyId;
//            ddlLenderAffiliate.DataSource = CurrentPage.GetDictionary("vwLenderAffiliate", String.Format("CompanyId={0}", companyID));
//            ddlLenderAffiliate.DataBind();
//            ddlLenderAffiliate.Items.Insert(0, new ListItem("- Select -", "0"));
//            ddlLenderAffiliate.SelectedValue = lenderAffiliateID.ToString();

            int stateID = AdvancedCalculator.Owner.Property.StateId;
            ddlState.DataSource = DataHelpers.GetStateList();
            ddlState.DataBind();
            ddlState.SelectedValue = stateID.ToString();

            int countyID = AdvancedCalculator.Owner.Property.CountyID;
            DataView countyView = DataHelpers.GetCountyList(stateID);
            ddlCounty.DataSource = countyView;
            ddlCounty.DataBind();
            if (countyView.Table.Select(String.Format("ID = {0}", countyID)).Length > 0)
                ddlCounty.SelectedValue = countyID.ToString();
            else if (ddlCounty.Items.Count > 0)
                ddlCounty.SelectedValue = "0";
        }
        #endregion

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (AdvancedCalculator == null || !FirstLoad)
                return;

            BindData();
            FirstLoad = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (AdvancedCalculator == null)
            {
                FirstLoad = true;
                return;
            }

            string errMessage = String.Empty;
            bool resUpdate = true;

//            MortgageInfo mortgageInfo = AdvancedCalculator.Owner.MortgageInfo;
//            int lenderAffiliateID = Convert.ToInt32(ddlLenderAffiliate.SelectedValue);
//            resUpdate &= AdvancedCalculator.Owner.UpdateObject("MortgageInfo.LenderAffiliateID", lenderAffiliateID.ToString(), mortgageInfo.ID, out errMessage);

            Borrower youngestBorrower = YoungestBorrower;
            string ybBirthDate = ((DateTime)diYBBirthDate.SelectedDate).ToString("d");
            resUpdate &= AdvancedCalculator.Owner.UpdateObject("Borrowers.DateOfBirth", ybBirthDate, youngestBorrower.ID, out errMessage);

            Property property = AdvancedCalculator.Owner.Property;
            decimal homeValue = Convert.ToDecimal(tbHomeValue.Text);
            resUpdate &= AdvancedCalculator.Owner.UpdateObject("Property.SPValue", homeValue.ToString(), property.ID, out errMessage);

            int stateID = Convert.ToInt32(ddlState.SelectedValue);
            resUpdate &= AdvancedCalculator.Owner.UpdateObject("Property.StateId", stateID.ToString(), property.ID, out errMessage);

            int countyID = Convert.ToInt32(ddlCounty.SelectedValue);
            resUpdate &= AdvancedCalculator.Owner.UpdateObject("Property.CountyID", countyID.ToString(), property.ID, out errMessage);

            lblErrMesssage.Text = resUpdate ? String.Empty : "Database error";
            lblErrMesssage.Visible = !resUpdate;
            if (resUpdate)
            {
                youngestBorrower.DateOfBirth = diYBBirthDate.SelectedDate;
                //AdvancedCalculator.Owner.Property.SPValue = homeValue;
                //AdvancedCalculator.Owner.Property.StateId = stateID;
                //AdvancedCalculator.Owner.Property.CountyID = countyID;
                AdvancedCalculator.Owner.Property = new Property(property.ID, AdvancedCalculator.Owner);

                CurrentPage.UpdateMortgage(AdvancedCalculator.Owner, AdvancedCalculator.Owner.ID);
            }

            FirstLoad = String.IsNullOrEmpty(AdvancedCalculator.ValidateFirst());

            if (ValidateFirst != null)
                ValidateFirst(this, EventArgs.Empty);
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            int stateID = Convert.ToInt32(ddlState.SelectedValue);
            DataView countyView = DataHelpers.GetCountyList(stateID);
            ddlCounty.DataSource = countyView;
            ddlCounty.DataBind();
            ddlCounty.SelectedValue = "0";
        }
        #endregion
    }
}