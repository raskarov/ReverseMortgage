using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class AdvCalculatorValues : AppControl
    {
        #region Events
        public event EventHandler ValidateFirst;
        #endregion

        private LoanStar.Common.AdvCalculator advCalc = null;

        public LoanStar.Common.AdvCalculator AdvancedCalculator
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

        private int MortgageProfileID
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
            if (FirstLoad)
            {
                BindData();
                FirstLoad = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool res = UpdateMortgageProfile();
            FirstLoad = String.IsNullOrEmpty(AdvancedCalculator.ValidateFirst());

            if (ValidateFirst != null)
                ValidateFirst(this, EventArgs.Empty);
        }
        private bool UpdateMortgageProfile()
        {
            bool res = false;
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageProfileID);
            if (mp != null)
            {
                Borrower bor = mp.YoungestBorrower;
                string ybBirthDate = ((DateTime)diYBBirthDate.SelectedDate).ToString("d");
                string errMessage;
                res = mp.UpdateObject("Borrowers.DateOfBirth", ybBirthDate, bor.ID, out errMessage);
                if (res)
                {
                    Property property = mp.Property;
                    decimal homeValue = Convert.ToDecimal(tbHomeValue.Text);
                    res = mp.UpdateObject("Property.SPValue", homeValue.ToString(), property.ID, out errMessage);

                }
            }
            lblErrMesssage.Text = res ? String.Empty : "Database error";
            return res;
        }
        private void BindData()
        {
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageProfileID);
            if (mp != null)
            {
                diYBBirthDate.SelectedDate = mp.YoungestBorrower.DateOfBirth;
                if (mp.Property.SPValue != null)
                { 
                    tbHomeValue.Text = mp.Property.SPValue.ToString();
                }
            }
        }
    }
}