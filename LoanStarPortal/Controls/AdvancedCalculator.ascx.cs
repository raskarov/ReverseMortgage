using System;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class AdvancedCalculator : AppControl, IPostBackEventHandler
    {
        #region Properties
        private LoanStar.Common.AdvancedCalculator advCalc = null;

        public bool FirstLoad
        {
            get
            {
                Object o = ViewState["FirstLoadAdvCalc"];
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
                ViewState["FirstLoadAdvCalc"] = value;
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

        private String AdvCalcErrMsg
        {
            get
            {
                if (Session[Constants.ADVANCEDCALCULATORERRMSG] == null)
                    Session[Constants.ADVANCEDCALCULATORERRMSG] = String.Empty;
                return Session[Constants.ADVANCEDCALCULATORERRMSG].ToString();
            }
            set { Session[Constants.ADVANCEDCALCULATORERRMSG] = value; }
        }
        #endregion

        #region Methods
        public string SelectColumn(int flag)
        {
            string ret = "";
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageProfileID);
            if (((int)mp.ProductCalcType) == flag)
                ret = "class=\"selected_col\"";
            return ret;
        }
        private void StoreAdvCalc()
        {
            Session[Constants.ADVANCEDCALCULATOR] = advCalc;
            Session[Constants.ADVANCEDCALCULATORUNIQUEID] = this.UniqueID;
        }
        private void StoreErrMsg(string errMsg)
        {
            if (String.IsNullOrEmpty(errMsg))
                return;

            resAdvCalcMessage.Value += String.IsNullOrEmpty(resAdvCalcMessage.Value) ? 
                errMsg : System.Environment.NewLine + errMsg;
        }

        private void SaveAdvancedCalculator(decimal? mpPaymentAmount, decimal? mpTerm, decimal? mpInitialDraw, decimal? mpCreditLine)
        {
            SaveAdvancedCalculator(mpPaymentAmount, mpTerm, mpInitialDraw, mpCreditLine, null);
        }
        private void SaveAdvancedCalculator(decimal? mpPaymentAmount, decimal? mpTerm, decimal? mpInitialDraw, decimal? mpCreditLine, int? mpProductID)
        {
            advCalc.NeedInitialDraw = chkInitialDraw.Checked;
            advCalc.NeedCreditLine = chkCreditLine.Checked;
            advCalc.NeedMonthlyIncome = chkMonthlyIncome.Checked;
            advCalc.NeedTerm = rbMontlyIncome.SelectedValue == "Term";

            string errMessage = advCalc.Save(mpPaymentAmount, mpTerm, mpInitialDraw, mpCreditLine, mpProductID);
            StoreErrMsg(errMessage);
            CurrentPage.UpdateMortgage(advCalc.Owner, advCalc.Owner.ID);
        }

        private void BindData()
        {
            BindButtons();
            BindCustomControls();

            string errMessage = advCalc.Validate();
            string errHECMMonthly = BindHECMMonthly();
            string errHECMAnnual = BindHECMAnnual();
            string errHomeKeeper = BindHomeKeeper();
            if (String.IsNullOrEmpty(errMessage))
            {
                if (!String.IsNullOrEmpty(errHECMMonthly))
                    errMessage += String.IsNullOrEmpty(errMessage) ? errHECMMonthly : System.Environment.NewLine + errHECMMonthly;
                if (!String.IsNullOrEmpty(errHECMAnnual))
                    errMessage += String.IsNullOrEmpty(errMessage) ? errHECMAnnual : System.Environment.NewLine + errHECMAnnual;
                if (!String.IsNullOrEmpty(errHomeKeeper))
                    errMessage += String.IsNullOrEmpty(errMessage) ? errHomeKeeper : System.Environment.NewLine + errHomeKeeper;
            }

            StoreErrMsg(AdvCalcErrMsg);
            AdvCalcErrMsg = String.Empty;

            StoreErrMsg(errMessage);

//            if (String.IsNullOrEmpty(resAdvCalcMessage.Value))
//                resAdvCalcMessage.Value = advCalc.ValidateClosingDate();
        }

        private void BindButtons()
        {
            lbDownload.Enabled = advCalc.MPProduct != ProductFlag.None;

            lbMontlyProduct.Enabled = advCalc.MPProduct != ProductFlag.HECM_Monthly &&
                advCalc.MPProdTypeHash.ContainsKey(ProductFlag.HECM_Monthly);
            lbAnnualProduct.Enabled = advCalc.MPProduct != ProductFlag.HECM_Annual &&
                advCalc.MPProdTypeHash.ContainsKey(ProductFlag.HECM_Annual);
            lbHomeKeeperProduct.Enabled = advCalc.MPProduct != ProductFlag.FNMA &&
                advCalc.MPProdTypeHash.ContainsKey(ProductFlag.FNMA);
        }

        private void BindCustomControls()
        {
            chkInitialDraw.Checked = advCalc.NeedInitialDraw;
            chkCreditLine.Checked = advCalc.NeedCreditLine;
            tbInitialDrawAmount.Text = advCalc.MPInitialDraw == 0 ? String.Empty : advCalc.MPInitialDraw.ToString();
            tbCreditLine.Text = advCalc.MPCreditLine == 0 ? String.Empty : advCalc.MPCreditLine.ToString();

            chkMonthlyIncome.Checked = advCalc.NeedMonthlyIncome;
            //trMonthlyIncome.Visible = advCalc.NeedMonthlyIncome;
            if (advCalc.NeedMonthlyIncome)
                trMonthlyIncome.Attributes.Add("style", "display:block");
            else
                trMonthlyIncome.Attributes.Add("style", "display:none");

            rbMontlyIncome.SelectedValue = advCalc.NeedTerm ? "Term" : "Tenure";
            //trTerm.Visible = advCalc.NeedMonthlyIncome && advCalc.NeedTerm;
            if (advCalc.NeedTerm)
                trTerm.Attributes.Add("style", "display:block");
            else
                trTerm.Attributes.Add("style", "display:none");

            tbAmount.Text = advCalc.MPPaymentAmount >= 0 ? advCalc.MPPaymentAmount.ToString() : String.Empty;
            tbMonths.Text = advCalc.MPTerm >= 0 ? String.Format("{0:d}",(int)advCalc.MPTerm) : String.Empty;
//            tbAmount.BackColor = tbMonths.BackColor = advCalc.MPPaymentAmount == 0 && advCalc.MPTerm == 0 ?
//                System.Drawing.Color.Pink : System.Drawing.ColorTranslator.FromHtml("#ffffaa");
        }

        private string BindHECMMonthly()
        {
            string resMessage = advCalc.Calculate(ProductFlag.HECM_Monthly);

            tbInitialRateMontly.Text = String.Format("{0}%", advCalc.PublishedIndex);//advCalc.PublishedIndex.ToString();
            tbExpectedIntRateMontly.Text = String.Format("{0}%", advCalc.ExpectedIndexRate);//advCalc.ExpectedIndexRate.ToString();
            tbRateCupMontly.Text = String.Format("{0}%", advCalc.EffectiveRateCap);
            tbMontlyServiceFeeMontly.Text = advCalc.MonthlyServiceFee.ToString("$#,###.00;$-#,###.00;$0");
            tbHomeValueMontly.Text = advCalc.HomeValue.ToString("$#,###.00;$-#,###.00;$0");
            tbPledgedValueMonthly.Text = advCalc.PledgedValueOrLimit.ToString("$#,###.00;$-#,###.00;$0");
            tbCreditlineGrowthRateMonthly.Text = String.Format("{0}%", advCalc.CreditLineGrowthRate);//advCalc.CreditLineGrowthRate.ToString();
            tbAgeMonthly.Text = advCalc.Age.ToString();
            tbPrincipalLimitMontly.Text = advCalc.PrincipalLimit.ToString("$#,###.00;$-#,###.00;$0");
            tbPresentValueServiceFeeMontly.Text = advCalc.ServiceSetAside.ToString("$#,###.00;$-#,###.00;$0");
            tbUpFrontPremiumMontly.Text = advCalc.UpFrontPremium.ToString("$#,###.00;$-#,###.00;$0");
            tbFinancedOriginationFeeMonthly.Text = advCalc.LoanOriginationFees.ToString("$#,###.00;$-#,###.00;$0");
            tbTotalOtherClosingCostsMontly.Text = advCalc.OtherFinancedCosts.ToString("$#,###.00;$-#,###.00;$0");
            tbTotalInitialChargesMontly.Text = advCalc.TotalInitialCharges.ToString("$#,###.00;$-#,###.00;$0");
            tbExistingMortgageBalanceMontly.Text = advCalc.MortgageBalance.ToString("$#,###.00;$-#,###.00;$0");
            tbTaxInsSetAsideMontly.Text = advCalc.ReserveTotalAmount.ToString("$#,###.00;$-#,###.00;$0");//advCalc.FirstYrPropChgs.ToString();
            tbNetAvailableMontly.Text = advCalc.NetAvailable.ToString("$#,###.00;$-#,###.00;$0");
            tbUnallocatedFundsMontly.Text = advCalc.UnallocatedFunds.ToString("$#,###.00;$-#,###.00;$0");
            tbInitialDrawMontly.Text = advCalc.InitialDraw == 0 ? String.Empty : advCalc.InitialDraw.ToString("$#,###.00;$-#,###.00;$0");
            tbCreditLineMontly.Text = advCalc.CreditLine == 0 ? String.Empty : advCalc.CreditLine.ToString("$#,###.00;$-#,###.00;$0");
            tbPeriodicPaymentMontly.Text = advCalc.PeriodicPayment.ToString("$#,###.00;$-#,###.00;$0");
            tbFinalTermMontly.Text = advCalc.FinalTerm.ToString();

            return resMessage;
        }

        private string BindHECMAnnual()
        {
            string resMessage = advCalc.Calculate(ProductFlag.HECM_Annual);

            tbInitialRateAnnual.Text = String.Format("{0}%", advCalc.PublishedIndex);//advCalc.PublishedIndex.ToString();
            tbExpectedIntRateAnnual.Text = String.Format("{0}%", advCalc.ExpectedIndexRate);//advCalc.ExpectedIndexRate.ToString();
            tbRateCupAnnual.Text = String.Format("{0}%", advCalc.EffectiveRateCap);
            tbMontlyServiceFeeAnnual.Text = advCalc.MonthlyServiceFee.ToString("$#,###.00;$-#,###.00;$0");
            tbHomeValueAnnual.Text = advCalc.HomeValue.ToString("$#,###.00;$-#,###.00;$0");
            tbPledgedValueAnnual.Text = advCalc.PledgedValueOrLimit.ToString("$#,###.00;$-#,###.00;$0");
            tbCreditlineGrowthRateAnnual.Text = String.Format("{0}%", advCalc.CreditLineGrowthRate);//advCalc.CreditLineGrowthRate.ToString();
            tbAgeAnnual.Text = advCalc.Age.ToString();
            tbPrincipalLimitAnnual.Text = advCalc.PrincipalLimit.ToString("$#,###.00;$-#,###.00;$0");
            tbPresentValueServiceFeeAnnual.Text = advCalc.ServiceSetAside.ToString("$#,###.00;$-#,###.00;$0");
            tbUpFrontPremiumAnnual.Text = advCalc.UpFrontPremium.ToString("$#,###.00;$-#,###.00;$0");
            tbFinancedOriginationFeeAnnual.Text = advCalc.LoanOriginationFees.ToString("$#,###.00;$-#,###.00;$0");
            tbTotalOtherClosingCostsAnnual.Text = advCalc.OtherFinancedCosts.ToString("$#,###.00;$-#,###.00;$0");
            tbTotalInitialChargesAnnual.Text = advCalc.TotalInitialCharges.ToString("$#,###.00;$-#,###.00;$0");
            tbExistingMortgageBalanceAnnual.Text = advCalc.MortgageBalance.ToString("$#,###.00;$-#,###.00;$0");
            tbTaxInsSetAsideAnnual.Text = advCalc.ReserveTotalAmount.ToString("$#,###.00;$-#,###.00;$0");//advCalc.FirstYrPropChgs.ToString();
            tbNetAvailableAnnual.Text = advCalc.NetAvailable.ToString("$#,###.00;$-#,###.00;$0");
            tbUnallocatedFundsAnnual.Text = advCalc.UnallocatedFunds.ToString("$#,###.00;$-#,###.00;$0");
            tbInitialDrawAnnual.Text = advCalc.InitialDraw == 0 ? String.Empty : advCalc.InitialDraw.ToString("$#,###.00;$-#,###.00;$0");
            tbCreditLineAnnual.Text = advCalc.CreditLine == 0 ? String.Empty : advCalc.CreditLine.ToString("$#,###.00;$-#,###.00;$0");
            tbPeriodicPaymentAnnual.Text = advCalc.PeriodicPayment.ToString("$#,###.00;$-#,###.00;$0");
            tbFinalTermAnnual.Text = advCalc.FinalTerm.ToString();

            return resMessage;
        }

        private string BindHomeKeeper()
        {
            string resMessage = advCalc.Calculate(ProductFlag.FNMA);

            tbInitialRateHomeKeeper.Text = String.Format("{0}%", advCalc.PublishedIndex);//advCalc.PublishedIndex.ToString();
            tbExpectedIntRateHomeKeeper.Text = String.Format("{0}%", advCalc.PublishedIndex);//advCalc.PublishedIndex.ToString();
            tbRateCupHomeKeeper.Text = String.Format("{0}%", advCalc.EffectiveRateCap);
            tbMontlyServiceHomeKeeper.Text = advCalc.MonthlyServiceFee.ToString("$#,###.00;$-#,###.00;$0");
            tbHomeValueHomeKeeper.Text = advCalc.HomeValue.ToString("$#,###.00;$-#,###.00;$0");
            tbPledgedValueHomeKeeper.Text = advCalc.PledgedValueOrLimit.ToString("$#,###.00;$-#,###.00;$0");
            tbCreditlineGrowthRateHomeKeeper.Text = String.Format("{0}%", advCalc.CreditLineGrowthRate);//advCalc.CreditLineGrowthRate.ToString();
            tbAgeHomeKeeper.Text = advCalc.Age.ToString();
            tbPresentValueServiceFeeHomeKeeper.Text = advCalc.ServiceSetAside.ToString("$#,###.00;$-#,###.00;$0");
            tbUpFrontPremiumHomeKeeper.Text = advCalc.UpFrontPremium.ToString("$#,###.00;$-#,###.00;$0");
            tbFinancedOriginationFeeHomeKeeper.Text = advCalc.LoanOriginationFees.ToString("$#,###.00;$-#,###.00;$0");
            tbTotalOtherClosingCostsHomeKeeper.Text = advCalc.OtherFinancedCosts.ToString("$#,###.00;$-#,###.00;$0");
            tbTotalInitialChargesHomeKeeper.Text = advCalc.TotalInitialCharges.ToString("$#,###.00;$-#,###.00;$0");
            tbExistingMortgageBalanceHomeKeeper.Text = advCalc.MortgageBalance.ToString("$#,###.00;$-#,###.00;$0");
            tbTaxInsSetAsideHomeKeeper.Text = advCalc.ReserveTotalAmount.ToString("$#,###.00;$-#,###.00;$0");//advCalc.FirstYrPropChgs.ToString();
            tbNetAvailableHomeKeeper.Text = advCalc.NetAvailable.ToString("$#,###.00;$-#,###.00;$0");
            tbUnallocatedFundsHomeKeeper.Text = advCalc.UnallocatedFunds.ToString("$#,###.00;$-#,###.00;$0");
            tbInitialDrawHomeKeeper.Text = advCalc.InitialDraw == 0 ? String.Empty : advCalc.InitialDraw.ToString("$#,###.00;$-#,###.00;$0");
            tbCreditLineHomeKeeper.Text = advCalc.CreditLine == 0 ? String.Empty : advCalc.CreditLine.ToString("$#,###.00;$-#,###.00;$0");
            tbPeriodicPaymentHomeKeeper.Text = advCalc.PeriodicPayment.ToString("$#,###.00;$-#,###.00;$0");
            tbFinalTermHomeKeeper.Text = advCalc.FinalTerm.ToString();

            return resMessage;
        }

        private void InitAdvancedCalculator()
        {
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageProfileID);
            advCalc = new LoanStar.Common.AdvancedCalculator(mp);
        }

        #endregion

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentPage.ClientScript.RegisterStartupScript(this.GetType(), "InitAdvancedCalculator", "<script language=\"javascript\" type=\"text/javascript\">DisplayMessage();</script>");
            InitAdvancedCalculator();

            string controlName = Page.Request["__EVENTTARGET"];
            string errMessage = advCalc.ValidateFirst();
            if (!String.IsNullOrEmpty(controlName) && controlName.Contains("ucAdvancedCalculatorValues"))
                ucAdvancedCalculatorValues.AdvancedCalculator = advCalc;
            else if (!String.IsNullOrEmpty(errMessage))
            {
                PanelAdvancedCalculatorValues.Visible = true;
                PanelAdvancedCalculator.Visible = false;

                ucAdvancedCalculatorValues.AdvancedCalculator = advCalc;
                resAdvCalcMessage.Value = errMessage;
            }
            else
            {
                PanelAdvancedCalculatorValues.Visible = false;
                PanelAdvancedCalculator.Visible = true;

                if (FirstLoad)
                {
                    FirstLoad = false;
                    BindData();
                }
//                else
//                    ProcessPostBack();
            }
        }

        protected void ucAdvancedCalculatorValues_ValidateFirst(object sender, EventArgs e)
        {
            resAdvCalcMessage.Value = advCalc.ValidateFirst();
            FirstLoad = !String.IsNullOrEmpty(resAdvCalcMessage.Value);

            if (!FirstLoad)
            {
                PanelAdvancedCalculatorValues.Visible = false;
                PanelAdvancedCalculator.Visible = true;
                BindData();
            }
        }

/*        protected void rbMontlyIncome_SelectedIndexChanged(object sender, EventArgs e)
        {
            advCalc.NeedTerm = rbMontlyIncome.SelectedValue == "Term";
            SaveAdvancedCalculator();
            BindData();
        }*/

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument != "BindData")
                return;

            BindData();
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            decimal mpPaymentAmount = String.IsNullOrEmpty(tbAmount.Text) ? 0 : Convert.ToDecimal(tbAmount.Text);
            decimal mpTerm = String.IsNullOrEmpty(tbMonths.Text) ? 0 : Convert.ToDecimal(tbMonths.Text);
            decimal mpInitialDraw = String.IsNullOrEmpty(tbInitialDrawAmount.Text) ? 0 : Convert.ToDecimal(tbInitialDrawAmount.Text);
            decimal mpCreditLine = String.IsNullOrEmpty(tbCreditLine.Text) ? 0 : Convert.ToDecimal(tbCreditLine.Text);
            SaveAdvancedCalculator(mpPaymentAmount, mpTerm, mpInitialDraw, mpCreditLine);

            AdvancedCalculatorLauncher advCalcLauncher = null;
            if (advCalc.MPProduct != ProductFlag.None)
            {
                advCalcLauncher = new AdvancedCalculatorLauncher(advCalc);
                advCalcLauncher.StartLaunches();
            }

            if (String.IsNullOrEmpty(resAdvCalcMessage.Value) && advCalcLauncher != null && !String.IsNullOrEmpty(advCalcLauncher.ErrorMessage))
                StoreErrMsg(advCalcLauncher.ErrorMessage);

            if (advCalcLauncher != null && String.IsNullOrEmpty(resAdvCalcMessage.Value) && advCalc.UnallocatedFunds > 0)
            {
                StoreAdvCalc();
                CurrentPage.ClientScript.RegisterStartupScript(this.GetType(), "ShowWindow", "<script language='javascript' type='text/javascript'>AdvCalculator();</script>");
            }
            else
                BindData();
        }

        protected void lbSelectProduct_Clicked(object sender, CommandEventArgs e)
        {
            if (e.CommandName != "SelectProduct")
                return;

            ProductFlag selProduct = (ProductFlag)Enum.Parse(typeof(ProductFlag), e.CommandArgument.ToString());
            if (!advCalc.MPProdTypeHash.ContainsKey(selProduct))
                return;

            decimal mpPaymentAmount = String.IsNullOrEmpty(tbAmount.Text) ? 0 : Convert.ToDecimal(tbAmount.Text);
            decimal mpTerm = String.IsNullOrEmpty(tbMonths.Text) ? 0 : Convert.ToDecimal(tbMonths.Text);
            decimal mpInitialDraw = String.IsNullOrEmpty(tbInitialDrawAmount.Text) ? 0 : Convert.ToDecimal(tbInitialDrawAmount.Text);
            decimal mpCreditLine = String.IsNullOrEmpty(tbCreditLine.Text) ? 0 : Convert.ToDecimal(tbCreditLine.Text);
            int mpProductID = (advCalc.MPProdTypeHash[selProduct] as Product).ID;
            SaveAdvancedCalculator(mpPaymentAmount, mpTerm, mpInitialDraw, mpCreditLine, mpProductID);

            BindData();
        }

        #region Download input parameters
        protected void lbDownload_Click(object sender, EventArgs e)
        {
            advCalc.Calculate(advCalc.MPProduct);
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(advCalc.InputParameters));

            base.DownloadGenerationError = String.Empty;
            base.DownloadStream = stream;
            base.DownloadContentType = "text/plain";
            base.DownloadFileName = String.Format("{0}_Input.txt", advCalc.MPProduct.ToString());

            CurrentPage.ClientScript.RegisterStartupScript(this.GetType(), "DownloadFile", "<script language=\"javascript\" type=\"text/javascript\">location.href = 'DownLoad.aspx';</script>");
        }

        protected void btnDownloadMontly_Click(object sender, EventArgs e)
        {
            advCalc.Calculate(ProductFlag.HECM_Monthly);
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(advCalc.InputParameters));

            base.DownloadGenerationError = String.Empty;
            base.DownloadStream = stream;
            base.DownloadContentType = "text/plain";
            base.DownloadFileName = "HECM_Monthly_Input.txt";

            CurrentPage.ClientScript.RegisterStartupScript(this.GetType(), "DownloadFile", "<script language=\"javascript\" type=\"text/javascript\">location.href = 'DownLoad.aspx';</script>");
        }

        protected void btnDownloadAnnual_Click(object sender, EventArgs e)
        {
            advCalc.Calculate(ProductFlag.HECM_Annual);
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(advCalc.InputParameters));

            base.DownloadGenerationError = String.Empty;
            base.DownloadStream = stream;
            base.DownloadContentType = "text/plain";
            base.DownloadFileName = "HECM_Annual_Input.txt";

            CurrentPage.ClientScript.RegisterStartupScript(this.GetType(), "DownloadFile", "<script language=\"javascript\" type=\"text/javascript\">location.href = 'DownLoad.aspx';</script>");
        }

        protected void btnDownloadHomeKeeper_Click(object sender, EventArgs e)
        {
            advCalc.Calculate(ProductFlag.FNMA);
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(advCalc.InputParameters));

            base.DownloadGenerationError = String.Empty;
            base.DownloadStream = stream;
            base.DownloadContentType = "text/plain";
            base.DownloadFileName = "FNMA_Input.txt";

            CurrentPage.ClientScript.RegisterStartupScript(this.GetType(), "DownloadFile", "<script language=\"javascript\" type=\"text/javascript\">location.href = 'DownLoad.aspx';</script>");
        }
        #endregion
        #endregion
    }
}