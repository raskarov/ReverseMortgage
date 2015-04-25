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

namespace LoanStarPortal.RetailSite
{
    public partial class RsAdvancedCalculator : AppPage
    {
        private int option = -1;
        private string value1 = "";
        private string value2 = "";
        private LoanStar.Common.AdvancedCalculator AdvCalc
        {
            get { return (LoanStar.Common.AdvancedCalculator)Session[Constants.ADVANCEDCALCULATOR]; }
            set { Session[Constants.ADVANCEDCALCULATOR] = value; }
        }
        private string AdvCalcUniqueID
        {
            get
            {
                object uniqueID = Session[Constants.ADVANCEDCALCULATORUNIQUEID];
                return uniqueID == null ? String.Empty : uniqueID.ToString();
            }
            set { Session[Constants.ADVANCEDCALCULATORUNIQUEID] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControls();
                BindData();
            }
        }
        private void BindData()
        {
            hfAdvCalcUniqueID.Value = AdvCalcUniqueID;
            ltrUnallocatedFunds.Text = AdvCalc.UnallocatedFunds.ToString("C");

            decimal incInitialDraw = AdvCalc.UnallocatedFunds;
            ltrInitialDraw.Text = String.Format(ltrInitialDraw.Text, incInitialDraw.ToString("C"));

            decimal incLineOfCredit = AdvCalc.UnallocatedFunds;//AdvCalc[LaunchStep.Launch3].LineOfCredit - AdvCalc.LineOfCredit;
            ltrCreditLine.Text = String.Format(ltrCreditLine.Text, incLineOfCredit.ToString("C"));

            decimal periodicPayment = AdvCalc[LaunchStep.Launch4].PeriodicPayment;
            ltrTenure.Text = String.Format(ltrTenure.Text, periodicPayment.ToString("C"));

            decimal incPeriodicPayment = AdvCalc[LaunchStep.Launch5].PeriodicPayment;//AdvCalc[LaunchStep.Launch5].PeriodicPayment - AdvCalc.PeriodicPayment;
            ltrlTermPayment.Text = String.Format(ltrlTermPayment.Text, incPeriodicPayment.ToString("C"));
        }
        private void InitControls()
        {
            pnlTermPayment.Visible = AdvCalc.ContainsLaunch(LaunchStep.Launch5);
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            SetAdvancedCalculatorValues();
            Session["calculationcompleted"] = true;
            string s = "<script type='text/javascript'>CloseAndRebind('" + option.ToString() + "','" + value1.ToString() + "','" + value2.ToString() + "')</script>";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowWindow", "<script type='text/javascript'>CloseAndRebind('" + option.ToString() + "','" + value1.ToString() + "','" + value2.ToString() + "')</script>");
        }
        private void SetAdvancedCalculatorValues()
        {
            decimal? mpPaymentAmount = null;
            decimal? mpTerm = null;
            decimal? mpInitialDraw = null;
            decimal? mpCreditLine = null;
            option = -1;                
            value1 = "";
            value2 = "";
            if (rbtnIncreaseInitialDraw.Checked)
            {
                AdvCalc.NeedInitialDraw = true;
                option = 1;                
                mpInitialDraw = AdvCalc[LaunchStep.Launch2].InitialDraw;
                value1 = mpInitialDraw.ToString();
            }
            else if (rbtnIncreaseCreditLine.Checked)
            {
                AdvCalc.NeedCreditLine = true;
                option = 2;
                mpCreditLine = AdvCalc[LaunchStep.Launch3].CreditLine;
                value1 = mpCreditLine.ToString();
            }
            else if (rbtnApplyTenure.Checked)
            {
                AdvCalc.NeedMonthlyIncome = true;
                option = 3;
                AdvCalc.NeedTerm = false;
                mpPaymentAmount = AdvCalc[LaunchStep.Launch4].PaymentAmount;
                mpTerm = AdvCalc[LaunchStep.Launch4].Term;
                value1 = mpPaymentAmount.ToString();
                value2 = mpTerm.ToString();
            }
            else if (rbtnTermPayment.Checked)
            {
                AdvCalc.NeedMonthlyIncome = true;
                AdvCalc.NeedTerm = true;
                mpPaymentAmount = AdvCalc[LaunchStep.Launch5].PaymentAmount;
                value1 = mpPaymentAmount.ToString();
            }
            SaveAdvancedCalculator(mpPaymentAmount, mpTerm, mpInitialDraw, mpCreditLine);
        }
        private void SaveAdvancedCalculator(decimal? mpPaymentAmount, decimal? mpTerm, decimal? mpInitialDraw, decimal? mpCreditLine)
        {
            if (mpCreditLine != null)
            {
                AdvCalc.MPCreditLine = (decimal)mpCreditLine;
            }
            if (mpPaymentAmount != null)
            {
                AdvCalc.MPPaymentAmount = (decimal)mpPaymentAmount;
            }
            if (mpTerm != null)
            {
                AdvCalc.MPTerm = (decimal)mpTerm;
            }
            if (mpInitialDraw != null)
            {
                AdvCalc.MPInitialDraw = (decimal)mpInitialDraw;
            }

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ShowWindow", "<script type='text/javascript'>CloseAndRebind(-1,'','')</script>");
        }
    }
}
