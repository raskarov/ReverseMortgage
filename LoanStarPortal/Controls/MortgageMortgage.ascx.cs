using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class MortgageMortgage : MortgageDataControl
    {
        #region constants
//        private const string MORTGAGEOBJECTNAME = "MortgageInfo";
        private const string TABGENERAL = "General";
        private const string TABPAYMENTPLAN = "Payment Plan";
//        private const string TABPAYINGINADVANCE = "Paying in advance";
        private const string TABCLOSINGCOSTS = "Closing costs";
        private const string TABCOUNSELING = "Counseling";

        private const string GENERALHEADER = "General";
        private const string PAYMENTHEADER = "Payment & Calculation";
        private const string LENDERHEADER = "Lender";
        private const string SERVICERHEADER = "Servicer";
        private const string INVESTORHEADER = "Investor";
        private const string GENERALCLOSINGCOSTSHEADER = "Closing costs";
        private const string SPECIALFEATUREHEADER = "Special Loan Feature";
        private const string COUNSELINGHEADER = "Counseling";
        //private const string TABFHA = "FHA";
        //private const string FHAHEADER = "FHA";
        private const string CLOSINGCOSTSHEADER = "Borrower's summary";
        public const string MORTGAGEINFOWRAPPERNAME = "MortgageInfo";

        private const string TRANSACTIONHEADER = "Transaction";
        #endregion

        public event UpdateNeeded OnUpdateNeeded;
        public delegate void UpdateNeeded();

        #region fields' groups
        private const int GENERALGROUPID = 1;
        private static readonly string[] MortageGeneralInfo = { 
            "LenderAffiliateID",
            "Productid","OriginatorLoanNumber","LenderLoanNumber", "FHACaseNumber","SharedAppreciation"
            ,"ShareAppreciation","AppMethodId","AppraisalDepositAmount","AppraisalDepositCollected"
            ,"AppraisalDepositCollectedYN","AppraisalDepositDeposited","OnTitleNotApplying","EscrowItemsId"
            ,"EscrowYears","EscrowYN","HowMuchForImprov","ProceedsUsedForImprov","DateOrigLoanClosed"
            ,"HomeValueAtOrigClosing","LendLimitAtOrigClosing","OldFHACaseNumber","OrdRefiWrkSheet"
            ,"RecvdRefiWrkSheet","RefiOldHECM","RefiWrkSheetAssignedToHUD"
        };
        private const int PAYMENTGROUPID = 2;
        private static readonly string[] MortagePaymentInfo = { 
            "MaximumPrincipleLimit","InitialRate", "AdjustTerms","LifetimeRateCap","Term", "InitialDraw",
            "CreditLine", "PaymentAmount", "OtherCashOut"
        };
        private const int TRANSACTIONGROUPID = 3;
        private static readonly string[] MortageTransaction = {
            "ClosingDate","TransactionTypeId",  "PurchaseContractAmount", "PersonalPropertyAmount", "DepositEarnestMoney"
            ,"CommissionMethodId","CommissionPercentage","CommissionFixedAmount"
        };
        private const int GENERALCLOSINGCOSTSGROUPID = 4;
        private static readonly string[] MortageGeneralClosingCosts = { 
        };
        //private const int LENDERGROUPID = 5;
        //private static readonly string[] MortageLender = {
        //    "LenderName"
        //};
        private const int SERVICERGROUPID = 6;
        private static readonly string[] MortageServicer = {
//            "ServicerName"
            "ServicerId"
        };
        private const int INVESTORGROUPID = 7;
        private static readonly string[] MortageInvestor = {
            "InvestorId"
//            "InvestorName"
        };

        private const int CLOSINGCOSTSGROUPID = 8;
        private static readonly string[] MortageClosingCosts = {
            "PurchaseContractAmountLabel", "PersonalPropertyAmountLabel", "SettlementChargesAmountLabel","GrossAmountDueFromBorrowerAmount", "DepositAmount", "PrincipleAmountOfNewLoan", "PrincipleAmountOfNewLoan", "SettlementChargesToBorrower", "CashPortionInitialDrawAmount", "FromTo"
        };
        private static readonly string[] MortgageSpecialFeature = {
            "OtherSpecialLoanFeature","OtherSpecialLoanFeatureYN"
        };
        private const int SPECIALFEATUREGROUPID = 9;
        //private const int FHAGROUPID = 3;
        //private static readonly string[] MortageFHAInfo = { 
        //    "FHACaseNumber"
        //};
        private const int COUNSELINGGROUPID = 10;
        private static readonly string[] MortageCounseling = {
            "CounselingMethod","CounsMAApproved","OptOutFaceToFace","OrigCertRecvd"
        };
        #endregion

        #region fields
        private MortgageInfo mpinfo;
        private RadTabStrip tabstrip;
        #endregion


        #region methods

        #region public
        public override void BuildControl(Control pageview)
        {
            objectName = MORTGAGEINFOWRAPPERNAME;
            mpinfo = Mp.MortgageInfo;
            base.BuildControl(pageview);
            mpinfo = Mp.MortgageInfo;    
        }
        protected override void SetUIFields()
        {
            SetUIGroupFields(objectFields, MortageGeneralInfo, GENERALGROUPID);
            SetUIGroupFields(objectFields, MortageTransaction, TRANSACTIONGROUPID);
            SetUIGroupFields(objectFields, MortageGeneralClosingCosts, GENERALCLOSINGCOSTSGROUPID);
            SetUIGroupFields(objectFields, MortagePaymentInfo, PAYMENTGROUPID);

            //SetUIGroupFields(objectFields, MortageLender, LENDERGROUPID);
            SetUIGroupFields(objectFields, MortageServicer, SERVICERGROUPID);
            SetUIGroupFields(objectFields, MortageInvestor, INVESTORGROUPID);
            SetUIGroupFields(objectFields, MortageClosingCosts, CLOSINGCOSTSGROUPID);
            SetUIGroupFields(objectFields, MortgageSpecialFeature, SPECIALFEATUREGROUPID);
            SetUIGroupFields(objectFields, MortageCounseling, COUNSELINGGROUPID);
        }
        protected override void PrepairObjectHtml()
        {

            Mp.EvaluateObjectFieldsVisibilty(mpinfo, objectFields);

            RadSplitter splitter = new RadSplitter();
            splitter.ID = "Mortgage_Splitter";
            splitter.Height = Unit.Percentage(95);
            splitter.Orientation = RadSplitterOrientation.Horizontal;
            splitter.Width = Unit.Percentage(100);
            splitter.BorderWidth = Unit.Pixel(0);
            splitter.BorderStyle = BorderStyle.None;
            splitter.BorderSize = 0;
            splitter.Skin = "Default";
            splitter.LiveResize = false;

            RadPane panetabs = new RadPane();
            panetabs.ID = "mm_pane_tab";
            panetabs.Height = Unit.Pixel(25);
            panetabs.Scrolling = RadSplitterPaneScrolling.None;

            RadSplitBar splitBar1 = new RadSplitBar();
            splitBar1.ID = "SplitBarMM";
            splitBar1.CollapseMode = RadSplitBarCollapseMode.None;
            splitBar1.EnableResize = false;
            splitBar1.Visible = false;

            RadPane panepageviews = new RadPane();
            panepageviews.ID = "mm_pane_pageviews";
            panepageviews.Scrolling = RadSplitterPaneScrolling.Y;

            tabstrip = new RadTabStrip();
            tabstrip.Skin = "Outlook";
            tabstrip.Orientation = RadTabStripOrientation.HorizontalTopToBottom;
            tabstrip.AutoPostBack = true;

            RadMultiPage newMultiPage = new RadMultiPage();
            newMultiPage.ID = "newMPMortgage";
            newMultiPage.CssClass = "tabpageview";
            newMultiPage.RenderSelectedPageOnly = true;

            tabstrip.MultiPageID = "newMPMortgage";

            radComboWidth = RADCOMBOWIDTH;

            Fields ctl = GetTabWrapper(GENERALHEADER, mpinfo, objectFields, GENERALGROUPID, "trmortgage", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl.ID = "ctlm_" + mpinfo.ID;

            Fields ctl2 = GetTabWrapper(TRANSACTIONHEADER, mpinfo, objectFields, TRANSACTIONGROUPID, "trmortgage", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl2.ID = "ctltransaction_" + mpinfo.ID;
            PageView newPage = new PageView();
            newPage.ID = "newMortgageGeneral" + (newMultiPage.PageViews.Count);
            newMultiPage.PageViews.Add(newPage);
            Tab generaltab = new Tab(TABGENERAL);
            tabstrip.Tabs.Add(generaltab);

            //if (ctl.HasChild || ctl2.HasChild)
            //{
                ControlWrapper wrapper =
                    (ControlWrapper) LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
                wrapper.ID = MORTGAGEINFOWRAPPERNAME + "_General_" + mpinfo.ID;

//                Tab generaltab = new Tab(TABGENERAL);
                if (Mp.InvoiceUpdateNeeded)
                {
                    generaltab.Attributes.Add("style", "color:red");
                }
//                tabstrip.Tabs.Add(generaltab);

                //PageView newPage = new PageView();
                //newPage.ID = "newMortgageGeneral" + (newMultiPage.PageViews.Count);
                //newMultiPage.PageViews.Add(newPage);

                newPage.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                newPage.Controls.Add(wrapper);
                if (ctl.HasChild)
                {
                    wrapper.Controls.Add(ctl);
                    wrapper.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                }
                if (ctl2.HasChild)
                {
                    wrapper.Controls.Add(ctl2);
                    wrapper.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                }
                short currentTabIndex = tabIndex++;
                ctl = GetTabWrapper(GENERALCLOSINGCOSTSHEADER, mpinfo, objectFields, GENERALCLOSINGCOSTSGROUPID, "trmortgage", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
                wrapper.Controls.Add(ctl);

                Invoices invoices = (Invoices)LoadControl(Constants.FECONTROLSLOCATION + Constants.FECTLINVOICES);
                invoices.OnUpdateNeeded += UpdateTabs;
                invoices.ID = "ctl_mortgage_general_invoices";
                ctl.AddControls(invoices);

                Reserves reserves = (Reserves)LoadControl(Constants.FECONTROLSLOCATION + Constants.FECTLRESERVES);
                reserves.ID = "ctl_mortgage_general_reserves";
                ctl.AddControls(reserves);

                MortgagePrepaidItems prepaidItems = (MortgagePrepaidItems)LoadControl(Constants.FECONTROLSLOCATION + Constants.FECTLPREPAIDITEMS);
                prepaidItems.ID = "ctl_closingcosts_prepaiditems";
                ctl.AddControls(prepaidItems);

                //wrapper.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));

                //ctl = GetTabWrapper(LENDERHEADER, mpinfo, objectFields, LENDERGROUPID, "trmortgage", new string[] { "tdmortgagelabel1/tdmortgagecontrol1", "tdmortgagelabel2/tdmortgagecontrol2" }, 2);
                //wrapper.Controls.Add(ctl);
                wrapper.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                tabIndex = currentTabIndex;
                ctl = GetTabWrapper(SERVICERHEADER, mpinfo, objectFields, SERVICERGROUPID, "trmortgage", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
                if(ctl.HasChild)
                {
                    wrapper.Controls.Add(ctl);
                    wrapper.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                }

                ctl = GetTabWrapper(INVESTORHEADER, mpinfo, objectFields, INVESTORGROUPID, "trmortgage", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
                if(ctl.HasChild)
                {
                    wrapper.Controls.Add(ctl);
                    wrapper.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                }
                ctl = GetTabWrapper(SPECIALFEATUREHEADER, mpinfo, objectFields, SPECIALFEATUREGROUPID, "trmortgage", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
                if(ctl.HasChild)
                {
                    wrapper.Controls.Add(ctl);
                    wrapper.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                }
                ctl = GetTabWrapper(COUNSELINGHEADER, mpinfo, objectFields, COUNSELINGGROUPID, "trmortgage", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            if(ctl.HasChild)
            {
                wrapper.Controls.Add(ctl);
            }
//            }
            newPage = new PageView();
            newPage.ID = "newMortgagePayment" + (newMultiPage.PageViews.Count);
            newMultiPage.PageViews.Add(newPage);
            tabstrip.Tabs.Add(new Tab(TABPAYMENTPLAN));

            ctl = GetTabWrapper(PAYMENTHEADER, mpinfo, objectFields, PAYMENTGROUPID, "trmortgage", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl.ID = "ctlp_" + mpinfo.ID;
            if (ctl.HasChild)
            {
                ControlWrapper wrapper1 = (ControlWrapper)LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
                wrapper1.ID = MORTGAGEINFOWRAPPERNAME + "_Payment_" + mpinfo.ID;

//                tabstrip.Tabs.Add(new Tab(TABPAYMENTPLAN));

//                PageView newPage = new PageView();
//                newPage.ID = "newMortgagePayment" + (newMultiPage.PageViews.Count);
//                newMultiPage.PageViews.Add(newPage);

                newPage.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                newPage.Controls.Add(wrapper1);
                wrapper1.Controls.Add(ctl);

                newPage.Controls.Add(new LiteralControl(FOOTERDIV));
            }

            newPage = new PageView();
            newPage.ID = "closingcosts" + (newMultiPage.PageViews.Count);
            newMultiPage.PageViews.Add(newPage);
            tabstrip.Tabs.Add(new Tab(TABCLOSINGCOSTS));

            ctl = GetTabWrapper(CLOSINGCOSTSHEADER, mpinfo, objectFields, CLOSINGCOSTSGROUPID, "trmortgage", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl.ID = "ctl_closingcosts" + mpinfo.ID;
            //if (ctl.HasChild)
            //{
                ControlWrapper wrapper2 =
                    (ControlWrapper) LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
                wrapper2.ID = MORTGAGEINFOWRAPPERNAME + "_ClosingCosts_" + mpinfo.ID;
//                tabstrip.Tabs.Add(new Tab(TABCLOSINGCOSTS));

                //PageView newPage = new PageView();
                //newPage.ID = "closingcosts" + (newMultiPage.PageViews.Count);
                //newMultiPage.PageViews.Add(newPage);

                newPage.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                newPage.Controls.Add(wrapper2);

                MortgageClosingCosts mclosing =
                    (MortgageClosingCosts)
                    LoadControl(Constants.FECONTROLSLOCATION + Constants.FECTLMORTGAGECLOSINGCOSTS);
                mclosing.ID = "ctl_closingcosts_payoff";
                ctl.AddControls(mclosing);

                MortgageSellerPay mseller =
                    (MortgageSellerPay)
                    LoadControl(Constants.FECONTROLSLOCATION + Constants.FECTLMORTGAGESELLERPAY);
                mseller.ID = "ctl_closingcosts_msellerpay";
                ctl.AddControls(mseller);

                MortgageBuyerPay mbuyerpay =
                    (MortgageBuyerPay)
                    LoadControl(Constants.FECONTROLSLOCATION + Constants.FECTLMORTGAGEBUYERPAY);
                mbuyerpay.ID = "ctl_closingcosts_mbuyerpay";
                ctl.AddControls(mbuyerpay);

                wrapper2.Controls.Add(ctl);

                newPage.Controls.Add(new LiteralControl(FOOTERDIV));
//            }

            panetabs.Controls.Add(tabstrip);
            panepageviews.Controls.Add(newMultiPage);
            splitter.Items.Add(panetabs);
            splitter.Items.Add(splitBar1);
            splitter.Items.Add(panepageviews);
            container.Controls.Add(splitter);


            tabstrip.SelectedIndex = 0;
            newMultiPage.SelectedIndex = 0;
        }

        private void UpdateTabs()
        {
            if (Mp.InvoiceUpdateNeeded)
            {
                tabstrip.Tabs[0].Attributes.Add("style", "color:red");
            }
            else
            {
                tabstrip.Tabs[0].Attributes.Add("style", "");
            }
            if (OnUpdateNeeded != null)
            {
                OnUpdateNeeded();
            }
        }

        #endregion


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

    }
}