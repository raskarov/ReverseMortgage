﻿using System;
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

namespace LoanStarPortal
{
    public partial class RsAdvCalc : AppPage
    {
        #region Properties
        private void ClearSession()
        {
            AdvCalc = null;
            AdvCalcUniqueID = null;
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
        private LoanStar.Common.AdvancedCalculator AdvCalc
        {
            get { return (LoanStar.Common.AdvancedCalculator)Session[Constants.ADVANCEDCALCULATOR]; }
            set { Session[Constants.ADVANCEDCALCULATOR] = value; }
        }
        private String AdvCalcErrMsg
        {
            set { Session[Constants.ADVANCEDCALCULATORERRMSG] = value; }
        }
        #endregion

        private void SaveAdvancedCalculator(decimal? mpPaymentAmount, decimal? mpTerm, decimal? mpInitialDraw, decimal? mpCreditLine)
        {
            string errMsg = AdvCalc.Save(mpPaymentAmount, mpTerm, mpInitialDraw, mpCreditLine);
            AdvCalcErrMsg = errMsg;
            UpdateMortgage(AdvCalc.Owner, AdvCalc.Owner.ID);
        }

        private void BindData()
        {
            hfAdvCalcUniqueID.Value = AdvCalcUniqueID;
            ltrUnallocatedFunds.Text = AdvCalc.UnallocatedFunds.ToString("C");

            decimal incInitialDraw = AdvCalc.UnallocatedFunds;//AdvCalc[LaunchStep.Launch2].InitialDraw - AdvCalc.InitialDraw;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControls();
                BindData();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            decimal? mpPaymentAmount = null;
            decimal? mpTerm = null;
            decimal? mpInitialDraw = null;
            decimal? mpCreditLine = null;
            if (rbtnIncreaseInitialDraw.Checked)
            {
                AdvCalc.NeedInitialDraw = true;
                mpInitialDraw = AdvCalc[LaunchStep.Launch2].InitialDraw;
            }
            else if (rbtnIncreaseCreditLine.Checked)
            {
                AdvCalc.NeedCreditLine = true;
                mpCreditLine = AdvCalc[LaunchStep.Launch3].CreditLine;
            }
            else if (rbtnApplyTenure.Checked)
            {
                AdvCalc.NeedMonthlyIncome = true;
                AdvCalc.NeedTerm = false;
                mpPaymentAmount = AdvCalc[LaunchStep.Launch4].PaymentAmount;
                mpTerm = AdvCalc[LaunchStep.Launch4].Term;
            }
            else if (rbtnTermPayment.Checked)
            {
                AdvCalc.NeedMonthlyIncome = true;
                AdvCalc.NeedTerm = true;
                mpPaymentAmount = AdvCalc[LaunchStep.Launch5].PaymentAmount;
            }
            SaveAdvancedCalculator(mpPaymentAmount, mpTerm, mpInitialDraw, mpCreditLine);

            ClearSession();
            ClientScript.RegisterStartupScript(this.GetType(), "ShowWindow", "<script type='text/javascript'>CloseAndRebind()</script>");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearSession();
            ClientScript.RegisterStartupScript(this.GetType(), "ShowWindow", "<script type='text/javascript'>CloseAndRebind()</script>");
        }
    }
}
