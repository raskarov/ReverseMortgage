using System;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.RetailSite.Control
{
    public partial class Calculator : AppControl
    {
        MortgageProfile mp = null;
        private const int LEADSTATUSID = 1;
        #region Properties
        private LoanStar.Common.AdvancedCalculator advCalc = null;

        protected bool IsCalculationCompleted
        {
            get 
            {
                bool res = false;
                Object o = Session["calculationcompleted"];
                if (o != null)
                {
                    try 
                    {
                        res = bool.Parse(o.ToString());
                    }
                    catch
                    {}
                }
                return res;
            }
            set { Session["calculationcompleted"] = value; }
        }
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
        public int RtMortgageId
        {
            get
            {
                int res = -1;
                Object o = Session[Constants.RETAILSITEMORTGAGEID];
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
            set
            {
                Session[Constants.RETAILSITEMORTGAGEID] = value;
            }
        }
        public RTInfo RetailSiteInfo
        {
            get
            {
                RTInfo res;
                Object o = Session["retailsiteinfo"];
                if ((o != null) && o is RTInfo)
                {
                    res = (RTInfo)o;
                }
                else
                {
                    res = new RTInfo();
                    Session["retailsiteinfo"] = res;
                }
                return res;
            }
            set { Session["retailsiteinfo"] = value; }
        }
        #region Methods
        public string SelectColumn(int flag)
        {
            string ret = "";
            AdvancedCalculator calc = GetCalculator();
            if (calc != null)
            {
                if (calc.MPProductID == flag)
                {
                    ret = "class=\"selected_col\"";
                }
            }
            return ret;
        }
        private AdvancedCalculator GetCalculator()
        {
            AdvancedCalculator res = null;
            Object o = Session[Constants.ADVANCEDCALCULATOR];
            if (o != null)
            {
                res = o as AdvancedCalculator;
            }
            return res;
        }
        private void StoreAdvCalc()
        {
            Session[Constants.ADVANCEDCALCULATOR] = advCalc;
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
            if (mpCreditLine != null)
            {
                advCalc.MPCreditLine = (decimal)mpCreditLine;
            }
            if (mpPaymentAmount!=null)
            {
                advCalc.MPPaymentAmount = (decimal)mpPaymentAmount;
            }
            if (mpTerm != null)
            {
                advCalc.MPTerm = (decimal)mpTerm;
            }
            if (mpInitialDraw != null)
            {
                advCalc.MPInitialDraw = (decimal)mpInitialDraw;
            }
            if (mpProductID != null)
            {
                advCalc.MPProductID = (int)mpProductID;
            }           
            StoreAdvCalc();
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

//            StoreErrMsg(AdvCalcErrMsg);
            AdvCalcErrMsg = String.Empty;

//            StoreErrMsg(errMessage);

        }

        private void BindButtons()
        {
            lbDownload.Enabled = advCalc.MPProduct != ProductFlag.None;
            lbMontlyProduct.Enabled = true;
            lbAnnualProduct.Enabled = true;
            lbHomeKeeperProduct.Enabled = true;
            lbMontlyProduct.Enabled = advCalc.MPProductID != 1;
            lbAnnualProduct.Enabled = advCalc.MPProductID != 2;
            lbHomeKeeperProduct.Enabled = advCalc.MPProductID != 4;
        }
        private void BindCustomControls()
        {
            chkInitialDraw.Checked = advCalc.NeedInitialDraw;
            chkCreditLine.Checked = advCalc.NeedCreditLine;
            tbInitialDrawAmount.Text = advCalc.MPInitialDraw == 0 ? String.Empty : advCalc.MPInitialDraw.ToString();
            tbCreditLine.Text = advCalc.MPCreditLine == 0 ? String.Empty : advCalc.MPCreditLine.ToString();
            chkMonthlyIncome.Checked = advCalc.NeedMonthlyIncome;

            if (advCalc.NeedMonthlyIncome)
                trMonthlyIncome.Attributes.Add("style", "display:block");
            else
                trMonthlyIncome.Attributes.Add("style", "display:none");

            rbMontlyIncome.SelectedValue = advCalc.NeedTerm ? "Term" : "Tenure";
            if (advCalc.NeedTerm)
                trTerm.Attributes.Add("style", "display:block");
            else
                trTerm.Attributes.Add("style", "display:none");

            tbAmount.Text = advCalc.MPPaymentAmount >= 0 ? advCalc.MPPaymentAmount.ToString() : String.Empty;
            tbMonths.Text = advCalc.MPTerm >= 0 ? String.Format("{0:d}",(int)advCalc.MPTerm) : String.Empty;
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
            tbPercentageMonthly.Text = String.Format("{0:##.##%}", advCalc.Persentage);//advCalc.Persentage.ToString();
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
            tbPercentageAnnual.Text = String.Format("{0:##.##%}", advCalc.Persentage);//advCalc.Persentage.ToString();
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
            tbPercentageHomeKeeper.Text = String.Format("{0:##.##%}", advCalc.Persentage);//advCalc.Persentage.ToString();
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
            if (RetailSiteInfo.MortgageId > 0)
            {
                mp = CurrentPage.GetMortgage(RetailSiteInfo.MortgageId);
                advCalc = new LoanStar.Common.AdvancedCalculator(mp);
            }
            else
            {
                mp = GetProfileData();
                mp.Liens = RetailSiteInfo.Liens;
                advCalc = GetCalculator();
                if (advCalc == null)
                {
                    advCalc = new LoanStar.Common.AdvancedCalculator(mp);
                }
            }
            StoreAdvCalc();
        }
        private MortgageProfile GetProfileData()
        {
            MortgageProfile res = new MortgageProfile();
            res.CurProfileStatusID = 1;
            Borrower borrower = res.Borrowers[0];
            borrower.FirstName = RetailSiteInfo.FirstName;
            borrower.LastName = RetailSiteInfo.LastName;
            borrower.DateOfBirth = RetailSiteInfo.DateOfBirth;
            res.Borrowers.Add(borrower);
            res.Property.StateId = RetailSiteInfo.StateId;
            res.Property.CountyID = RetailSiteInfo.CountyId;
            res.Property.LendingLimit = Property.GetLendingLimit(res.Property.CountyID);
            res.Property.SPValue = RetailSiteInfo.HomeValue;
            return res;
        }
        #endregion

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentPage.ClientScript.RegisterStartupScript(this.GetType(), "InitAdvancedCalculator", "<script language=\"javascript\" type=\"text/javascript\">DisplayMessage();</script>");
            InitAdvancedCalculator();
            string errMessage = advCalc.ValidateFirst();

            if (FirstLoad)
            {
                 FirstLoad = false;
                 BindData();
            }

        }
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            decimal mpPaymentAmount = String.IsNullOrEmpty(tbAmount.Text) ? 0 : Convert.ToDecimal(tbAmount.Text);
            decimal mpTerm = String.IsNullOrEmpty(tbMonths.Text) ? 0 : Convert.ToDecimal(tbMonths.Text);
            decimal mpInitialDraw = String.IsNullOrEmpty(tbInitialDrawAmount.Text) ? 0 : Convert.ToDecimal(tbInitialDrawAmount.Text);
            decimal mpCreditLine = String.IsNullOrEmpty(tbCreditLine.Text) ? 0 : Convert.ToDecimal(tbCreditLine.Text);
            SaveAdvancedCalculator(mpPaymentAmount, mpTerm, mpInitialDraw, mpCreditLine);

            AdvancedCalculatorLauncher advCalcLauncher = null;
            ProductFlag product = GetProduct();
            if (product != ProductFlag.None)
            {
                mp.MortgageInfo.ProductCalcType = product;
                advCalcLauncher = new AdvancedCalculatorLauncher(advCalc);
                advCalcLauncher.StartLaunches();
            }

            if (advCalcLauncher != null && advCalc.UnallocatedFunds > 0)
            {
                StoreAdvCalc();
                CurrentPage.ClientScript.RegisterStartupScript(this.GetType(), "ShowWindow", "<script language='javascript' type='text/javascript'>RsAdvCalculator();</script>");
            }
            else
                BindData();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }
        private void SaveData()
        {
            if (String.IsNullOrEmpty(RetailSiteInfo.FirstName) || String.IsNullOrEmpty(RetailSiteInfo.LastName))
            {
                pnlInput.Visible = true;
                PanelAdvancedCalculator.Visible = false;
                tbFirstName.Text = RetailSiteInfo.FirstName;
                tbLastName.Text = RetailSiteInfo.LastName;
                if (tbFirstName.Text == "")
                {
                    tbFirstName.Focus();
                }
                else
                {
                    tbLastName.Focus();
                }
            }
            else
            {
                MortgageProfile mp;
                if (RetailSiteInfo.MortgageId < 0)
                {
                    mp = new MortgageProfile();
                }
                else
                {
                    mp = CurrentPage.GetMortgage(RtMortgageId);
                }
                int res = RetailSiteInfo.Save(mp, CurrentPage);
                RtMortgageId = res;
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("RetailPage.aspx?control=input");
        }
        protected void lbSelectProduct_Clicked(object sender, CommandEventArgs e)
        {
            if (e.CommandName != "SelectProduct")
                return;

            ProductFlag selProduct = (ProductFlag)Enum.Parse(typeof(ProductFlag), e.CommandArgument.ToString());

            decimal mpPaymentAmount = String.IsNullOrEmpty(tbAmount.Text) ? 0 : Convert.ToDecimal(tbAmount.Text);
            decimal mpTerm = String.IsNullOrEmpty(tbMonths.Text) ? 0 : Convert.ToDecimal(tbMonths.Text);
            decimal mpInitialDraw = String.IsNullOrEmpty(tbInitialDrawAmount.Text) ? 0 : Convert.ToDecimal(tbInitialDrawAmount.Text);
            decimal mpCreditLine = String.IsNullOrEmpty(tbCreditLine.Text) ? 0 : Convert.ToDecimal(tbCreditLine.Text);
            int mpProductID = (advCalc.RetailCalculatorProducts[selProduct] as Product).ID;
            SaveAdvancedCalculator(mpPaymentAmount, mpTerm, mpInitialDraw, mpCreditLine, mpProductID);

            BindData();
        }
        protected void Page_PreRender(object sender, EventArgs e) 
        {
                if (IsCalculationCompleted)
                {
                    IsCalculationCompleted = false;
                    BindData();
                }

        }

        #region Download input parameters
        private ProductFlag GetProduct()
        {
            ProductFlag calcType = ProductFlag.None;
            switch (advCalc.MPProductID)
            {
                case 1:
                    calcType = ProductFlag.HECM_Monthly;
                    break;
                case 2:
                    calcType = ProductFlag.HECM_Annual;
                    break;
                case 4:
                    calcType = ProductFlag.FNMA;
                    break;
            }
            return calcType;
        }
        protected void lbDownload_Click(object sender, EventArgs e)
        {
            advCalc.Calculate(GetProduct());
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(advCalc.InputParameters));

            base.DownloadGenerationError = String.Empty;
            base.DownloadStream = stream;
            base.DownloadContentType = "text/plain";
            base.DownloadFileName = String.Format("{0}_Input.txt", advCalc.MPProductID.ToString());

            CurrentPage.ClientScript.RegisterStartupScript(this.GetType(), "DownloadFile", "<script language=\"javascript\" type=\"text/javascript\">location.href = '../DownLoad.aspx';</script>");
        }
        protected void btnDownloadMontly_Click(object sender, EventArgs e)
        {
            advCalc.Calculate(ProductFlag.HECM_Monthly);
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(advCalc.InputParameters));

            base.DownloadGenerationError = String.Empty;
            base.DownloadStream = stream;
            base.DownloadContentType = "text/plain";
            base.DownloadFileName = "HECM_Monthly_Input.txt";

            CurrentPage.ClientScript.RegisterStartupScript(this.GetType(), "DownloadFile", "<script language=\"javascript\" type=\"text/javascript\">location.href = '../DownLoad.aspx';</script>");
        }
        protected void btnDownloadAnnual_Click(object sender, EventArgs e)
        {
            advCalc.Calculate(ProductFlag.HECM_Annual);
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(advCalc.InputParameters));

            base.DownloadGenerationError = String.Empty;
            base.DownloadStream = stream;
            base.DownloadContentType = "text/plain";
            base.DownloadFileName = "HECM_Annual_Input.txt";

            CurrentPage.ClientScript.RegisterStartupScript(this.GetType(), "DownloadFile", "<script language=\"javascript\" type=\"text/javascript\">location.href = '../DownLoad.aspx';</script>");
        }
        protected void btnDownloadHomeKeeper_Click(object sender, EventArgs e)
        {
            advCalc.Calculate(ProductFlag.FNMA);
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(advCalc.InputParameters));
            base.DownloadGenerationError = String.Empty;
            base.DownloadStream = stream;
            base.DownloadContentType = "text/plain";
            base.DownloadFileName = "FNMA_Input.txt";
            CurrentPage.ClientScript.RegisterStartupScript(this.GetType(), "DownloadFile", "<script language=\"javascript\" type=\"text/javascript\">location.href = '../DownLoad.aspx';</script>");
        }
        protected void btnOK_Click(object sender, EventArgs e)
        {
            pnlInput.Visible = false;
            PanelAdvancedCalculator.Visible = true;
            RetailSiteInfo.FirstName = tbFirstName.Text;
            RetailSiteInfo.LastName = tbLastName.Text;
            SaveData();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlInput.Visible = false;
            PanelAdvancedCalculator.Visible = true;
        }
 
        #endregion
        #endregion  
    }
}