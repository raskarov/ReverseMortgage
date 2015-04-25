using System;
using System.Collections;
using System.Data;

namespace LoanStar.Common
{
    public class MortgageInfo : BaseObject
    {
        public class MortgageInfoException : BaseObjectException
        {
            public MortgageInfoException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public MortgageInfoException(string message)
                : base(message)
            {
            }

            public MortgageInfoException()
            {
            }
        }

        #region enums
        enum AppMethod
        {
            FaceToFace = 1,
            Email = 2,
            Telephone = 3
        }
        public enum TransactionTypeEnum
        {
            NotSelected = 0,
            Refinance = 1,
            Purchase = 2
        }
        public enum EscrowItems
        {
            Both = 1,
            Insurance = 2,
            Taxes = 3
        }
        #endregion

        #region constants
        private const string MORTGAGEINFOTABLE = "MortgageInfo";
        private const string GETMORTGAGEINFOBYID = "GetMortgageInfoById";
        private const string GETTOTALPAYOFFS = "GetTotalPayoffs";
        private const string GETPAYOFFAMONT = "GetPayoffAmount";
        private const int MORTGAGEDUEYEARS = 150;
//        private const decimal LIMIT = 417000;
        private const decimal FIRSTPARTLIMIT = 200000;
        private const decimal FIRSTPARTSUM = 2500;
        private const decimal FIRSTPARTPERCENT = (decimal)0.02;
        private const decimal SECONDPARTPERCENT = (decimal)0.01;
        private const decimal LENDERFEECAP = 6000;
        #endregion

        #region fields
        private readonly MortgageProfile mp;

        private int productId = -1;
        private Product product;

        private DateTime? closingDate;
        private string originatorHeadquarters = String.Empty;
        private string originatorLoanNumber = String.Empty;
        private string lenderLoanNumber = String.Empty;
        private int transactionTypeId;
        private string transactionType = String.Empty;
        private decimal? purchaseContractAmount;
        private decimal? personalPropertyAmount;
        private decimal? settlementChargesAmount;
        private decimal? depositEarnestMoney;
        private decimal? principleAmountOfNewLoan;
        private int commissionMethodId = 0;
        private float? commissionPercentage;
        private decimal? commissionFixedAmount;

        private decimal? existingLoansTakenSubjectTo;
        private decimal? settlementChargesToBorrower;
        private decimal? cashPortionInitialDrawAmount;
        private readonly string servicerName = String.Empty;
        private readonly string investorName = String.Empty;
        private bool? condoRider;
        private bool? pudRider;
        private bool? otherRider;
        private string otherRiderDescription = String.Empty;
        private string adjustTerms = String.Empty;
        private string fhaCaseNumber = String.Empty;
        private Hashtable errMessages;
        private decimal? term;
        private decimal? initialDraw;
        private decimal? creditLine;
        private decimal? paymentAmount;
        private decimal? otherCashOut;
        private decimal? sharedAppreciation;
        private bool? shareAppreciation;
        private string titleOrderNumber = String.Empty;
        private int appMethodId = 0;
        private bool? otherSpecialLoanFeature;
        private decimal? appraisalDepositAmount;
        private bool? appraisalDepositCollected;
        private bool? onTitleNotApplying;
//        private DateTime? appDate;
        private bool? appraisalDepositDeposited;
        private DateTime? caseNumAssignDate;
        private DateTime? caseNumCreateDate;
        private bool? closeInATrust;
        private DateTime? counsCertCousDate;
        private DateTime? counsCertExpDate;
        private bool? counsMAApproved;
        private DateTime? creditReportRequestDate;
        private bool? equityShareYN;
        private int escrowItemsId;
        private int? escrowYears;
        private bool? escrowYN;
        private DateTime? floodCertDetermDate;
        private decimal? howMuchForImprov;
        private bool? jointApp;
        private string nonBorrSpAddress1 = String.Empty;
        private string nonBorrSpAddress2 = String.Empty;
        private DateTime? nonBorrSpBirthDate;
        private string nonBorrSpCity = String.Empty;
        private string nonBorrSpFirstName = String.Empty;
        private string nonBorrSpLastName = String.Empty;
        private string nonBorrSpSS = String.Empty;
        private int nonBorrSpStateID;
        private bool nonBorrSpYN = false;
        private string nonBorrSpZip = String.Empty;
        private bool? optOutFaceToFace;
        private bool? origCertRecvd;
        private bool? otherSpecialLoanFeatureYN;
        private decimal principleLimit = 0;
        private bool? proceedsUsedForImprov;
        private bool repairRiderYN = false;
        private string settPlaceAddress1 = String.Empty;
        private string settPlaceAddress2 = String.Empty;
        private string settPlaceCity = String.Empty;
        private int settPlaceOptionId = 0;
        private int settPlaceStateId = 0;
        private string settPlaceZip = String.Empty;
        private bool specialLoanFeatureOtherYN = false;
        private bool? studentLoanDefalut;
        private bool? studentLoanFlag;
        private bool? taxLienFlag;
        private DateTime? termiteReportInspDate;
        private string trusteeAddress1 = String.Empty;
        private string trusteeAddress2 = String.Empty;
        private DateTime? trusteeBirthDate;
        private string trusteeCity = String.Empty;
        private string trusteeFirstName = String.Empty;
        private string trusteeLastName = String.Empty;
        private bool? trusteesNotOnApp;
        private string trusteeSS = String.Empty;
        private int trusteeStateId;
        private bool trusteeYN = false;
        private string trusteeZip = String.Empty;
        private bool? unpaidMortClosedYrAgo;
        private bool? unpaidMortgageFlag;
        private bool? unpaidMortgageNotOnTitle;
        private bool? vODCoverAmntShort;
        private bool? vODFromFinacInst;
        private bool moneyShortFlag;
        private bool? bKFlag;
        private bool? orderedCreditReport;
        private bool? receivedCreditReport;
        private DateTime? creditReportOrderDate;
        private bool? bKDischarged;
        private int bKTypeId;
        private bool? ch7BKDischarged;
        private DateTime? bKDischargeDate;
        private int bKIntentionId;
        private bool? forclosureFlag;
        private bool? rushFlag;
        private bool? judgementFlag;
        private bool? judgementOnTitle;
        private bool? appraisalDepositCollectedYN;
        private bool? bnkStmntCoverAmntShort;
        private bool? custUseOwnCash;
        private bool? giftLtrBorrNamesSignOk;
        private bool? giftLtrDonorRelationOk;
        private bool? giftLtrIncDollar;
        private bool? giftLtrNoPaymentStmntOk;
        private bool? giftLtrPaperTrailOK;
        private bool? giftLtrReceived;
        private int methodProveFundsId = 0;
        private bool? recvdBnkStmnt;
        private bool? recvdVOD;
        private bool? yrs5Passed;
        private string counselorName = String.Empty;
        private DateTime? dateOrigLoanClosed;
        private decimal? homeValueAtOrigClosing;
        private string interviewerCompanyAddress1 = String.Empty;
        private string interviewerCompanyAddress2 = String.Empty;
        private string interviewerCompanyCity = String.Empty;
        private string interviewerCompanyName = String.Empty;
        private int interviewerCompanyStateId = 0;
        private string interviewerCompanyZip = String.Empty;
        private string interviewerName = String.Empty;
        private string interviewerPhoneNumber = String.Empty;
        private decimal? lendLimitAtOrigClosing;
        private string oldFHACaseNumber;
        private bool? ordRefiWrkSheet;
        private decimal periodicServiceFee=0;
        private bool? recvdRefiWrkSheet;
        private bool? refiOldHECM;
        private bool? refiWrkSheetAssignedToHUD;
        private decimal repairAdminFee = 0;
        private string specialLoanFeatureOtherDescription = String.Empty;
        private int servicerId = 0;
        private int investorId = 0;
        private int lenderAffiliateID = -1;
        private Company lenderAffiliate = null;
        private int closingAgentId = 0;
        private VendorGlobal closingAgent = null;
        private AdvCalculator calculator = null;
        private Hashtable prodTypeHash = null;
        private int counselingMethodId = 0;
        private string calculatorError = String.Empty;
        private int calculatorSettingID = 0;
        private int closingCostProfileId = 0;
        private int closingOriginatorId = 0;
        private bool? allApplicantsCompletedHECMCounseling;
        private bool? requestedFHACase;
        private bool? collectedFHACase;
        private bool? ldpgsaCollected;
        private bool? caivrsCollected;
        private bool isCalculatorValid = false;
        private string corporateSigner = String.Empty;
        private string corporateSignerTitle = String.Empty;
        private string actualSettlementAddress;
        private string acutalCityStateZip;
        private DateTime? fundingDate;
        private DateTime? hecmMonthlyNextRateChange;
        private bool? equityProtection;
        private decimal? appraisalDepositAmountReturned;
        private decimal? cashFromBorrowerForLien;
        private string listEndorsements;
        private decimal? lendersCoverage;
        private int trusteeId = 0;
        private Trustee trustee;
        private Servicer servicer;
        private DateTime? applicationDate;
        private bool? ownOrSoldRealEstate;
        private bool? isItToBeSold;
        private decimal? salesPrice;
        private decimal? originalMortgageAmt;
        private bool? ownMoreThan4Dwellings;

        private string altContactAddress1;
        private string altContactAddress2;
        private string altContactAltPhone;
        private string altContactCity;
        private string altContactName;
        private string altContactPhone;
        private string altContactRelationship;
        private int altContactStateId = 0;
        private string altContactZip;

        private bool? requestedServicingWorksheet;
        private bool? collectedServicingWorksheet;
        private decimal? hecmRefiPrincipleLimitOldLoan;
        private decimal? hecmRefiOrigMIPAmt;

        private string otherREAddress1;
        private string otherREAddress2;
        private string otherRECity;
        private string otherREZip;
        private int otherREStateId = 0;
        private int payoffOldHECMId = 0;
        private bool? adp952;
        private bool? adp958;
        private bool? caseNumberNamesAddressMatch;
        private bool? subjectPropInForeclosure;
        private bool? ldp_GSANamesMatch;
        private bool? caivrsAuthNamesSSMatch;
        private bool? caivrsAuthClear;

        private DateTime? counsReceivedDate;
        private DateTime? creditReportReceivedDate;
        private DateTime? ldpReceivedDate;
        private DateTime? eplsReceivedDate;
        private DateTime? caivrsReceivedDate;

        private bool? srpLocked;
        private DateTime? srpLockExpDate;
        private float? srpRate;
        private bool? giftLtrDonorInfoOk;
        private float? yieldSpreadRate;
        private decimal? srpPremium;
        private decimal? yspPremium;
        private string docsContactName;
        private string docsContactTitle;
        private DateTime? rateLockStartDate;
        #endregion

        #region properties
        public decimal OrigFeePlusSRPplusMaxYSP
        {
            get
            {
                decimal res = 0;
                if(mp.LenderChargeLoanOriginationFeeInvoices!=null && mp.LenderChargeLoanOriginationFeeInvoices.Count>0)
                {
                    res = mp.LenderChargeLoanOriginationFeeInvoices[0].Amount;
                }
                res += SRPplusMaxYSP;
                return res;
            }
        }
        public DateTime? RateLockStartDate
        {
            get { return rateLockStartDate; }
            set { rateLockStartDate = value; }
        }
        public decimal TotalAvailableAssets
        {
            get
            {
                decimal res = 0;
                for(int i = 0; i<mp.Borrowers.Count;i++)
                {
                    res += mp.Borrowers[i].AvailableAssets==null?0:(decimal)mp.Borrowers[i].AvailableAssets;
                }
                return res;
            }
        }
        public string HECMtoHECMRefiXIndicator
        {
            get
            {
                string res = String.Empty;
                if (refiOldHECM != null && (bool) refiOldHECM)
                {
                    res = "X";
                }
                return res;
            }
        }
        public string DocsContactName
        {
            get { return docsContactName; }
            set { docsContactName = value; }
        }
        public string DocsContactTitle
        {
            get { return docsContactTitle; }
            set { docsContactTitle = value; }
        }
        public decimal TotalFinancedAmount
        {
            get
            {
                decimal res = 0;
                for (int i = 0; i < mp.Invoices.Count;i++ )
                {
                    res += mp.Invoices[i].AmountFinanced;
                }
                return res;
            }
        }
        public decimal TotalPOCAmount
        {
            get
            {
                decimal res = 0;
                for (int i = 0; i < mp.Invoices.Count; i++)
                {
                    res += mp.Invoices[i].POCAmount;
                }
                return res;
            }
        }
        public decimal TotalLenderCredit
        {
            get
            {
                decimal res = 0;
                for (int i = 0; i < mp.Invoices.Count; i++)
                {
                    res += mp.Invoices[i].LenderCreditAmount;
                }
                return res;
            }
        }
        public decimal TotalThirdPartyPaid
        {
            get
            {
                decimal res = 0;
                for (int i = 0; i < mp.Invoices.Count; i++)
                {
                    res += mp.Invoices[i].ThirdPartyPaidAmount;
                }
                return res;
            }
        }
        public decimal TotalCharges
        {
            get
            {
                decimal res = 0;
                for (int i = 0; i < mp.Invoices.Count; i++)
                {
                    res += mp.Invoices[i].Amount;
                }
                return res;
            }
        }
        public decimal SRPplusMaxYSP
        {
            get { return (SRPPremium == null ? 0 : (decimal)SRPPremium) + Calculator.MaxYieldSpreadPremium; }
        }
        //public decimal SRPplusYSP
        //{
        //    get { return (srpPremium ?? 0) + (yspPremium ?? 0); }
        //}
        public decimal? YSPPremium
        {
            get { return yspPremium; }
            set { yspPremium = value; }
        }
        public decimal? SRPPremium
        {
            get { return srpPremium; }
            set { srpPremium = value; }
        }
        public float? YieldSpreadRate
        {
            get { return yieldSpreadRate; }
            set { yieldSpreadRate = value; }
        }
        public bool? GiftLtrDonorInfoOk
        {
            get { return giftLtrDonorInfoOk; }
            set { giftLtrDonorInfoOk = value; }
        }
        public bool? SRPLocked
        {
            get { return srpLocked; }
            set { srpLocked = value; }
        }
        public DateTime? SRPLockExpDate
        {
            get { return srpLockExpDate; }
            set { srpLockExpDate = value; }
        }
        public float? SRPRate
        {
            get { return srpRate; }
            set { srpRate = value; }
        }

        public DateTime? CounsExpDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Product != null && counsReceivedDate != null)
                {
                    res = ((DateTime)counsReceivedDate).AddDays(mp.Product.CounsActiveDays);
                }
                return res;
            }
        }
        public DateTime? CreditReportExpDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Product != null && creditReportReceivedDate != null)
                {
                    res = ((DateTime)creditReportReceivedDate).AddDays(mp.Product.CreditReportActiveDays);
                }
                return res;
            }
        }
        public DateTime? LDPExpDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Product != null && ldpReceivedDate != null)
                {
                    res = ((DateTime)ldpReceivedDate).AddDays(mp.Product.LDPActiveDays);
                }
                return res;
            }
        }
        public DateTime? EPLSExpDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Product != null && eplsReceivedDate != null)
                {
                    res = ((DateTime)eplsReceivedDate).AddDays(mp.Product.EPLSActiveDays);
                }
                return res;
            }
        }
        public DateTime? CaivrsExpDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Product != null && caivrsReceivedDate != null)
                {
                    res = ((DateTime)caivrsReceivedDate).AddDays(mp.Product.CaivrsActiveDays);
                }
                return res;
            }
        }
        public DateTime? CounsNotifyMeDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Originator != null && CounsExpDate != null)
                {
                    res = ((DateTime)CounsExpDate).AddDays(-mp.Originator.CounsDaysNotifyMeExp);
                }
                return res;
            }
        }
        public DateTime? CreditReportNotifyMeDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Originator != null && CreditReportExpDate != null)
                {
                    res = ((DateTime)CreditReportExpDate).AddDays(-mp.Originator.CreditReportDaysNotifyMeExp);
                }
                return res;
            }
        }
        public DateTime? LDPNotifyMeDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Originator != null && LDPExpDate != null)
                {
                    res = ((DateTime)LDPExpDate).AddDays(-mp.Originator.LDPDaysNotifyMeExp);
                }
                return res;
            }
        }
        public DateTime? EPLSNotifyMeDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Originator != null && EPLSExpDate != null)
                {
                    res = ((DateTime)EPLSExpDate).AddDays(-mp.Originator.EPLSDaysNotifyMeExp);
                }
                return res;
            }
        }
        public DateTime? CaivrsNotifyMeDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Originator != null && CaivrsExpDate != null)
                {
                    res = ((DateTime)CaivrsExpDate).AddDays(-mp.Originator.CaivrsDaysNotifyMeExp);
                }
                return res;
            }
        }

        public DateTime? CounsReceivedDate
        {
            get { return counsReceivedDate; }
            set { counsReceivedDate = value; }
        }
        public DateTime? CreditReportReceivedDate
        {
            get { return creditReportReceivedDate; }
            set { creditReportReceivedDate = value; }
        }
        public DateTime? LDPReceivedDate
        {
            get { return ldpReceivedDate; }
            set { ldpReceivedDate = value; }
        }
        public DateTime? EPLSReceivedDate
        {
            get { return eplsReceivedDate; }
            set { eplsReceivedDate = value; }
        }
        public DateTime? CaivrsReceivedDate
        {
            get { return caivrsReceivedDate; }
            set { caivrsReceivedDate = value; }
        }
        public bool? CAIVRSAuthNamesSSMatch
        {
            get { return caivrsAuthNamesSSMatch; }
            set { caivrsAuthNamesSSMatch = value; }
        }
        public bool? CAIVRSAuthClear
        {
            get { return caivrsAuthClear; }
            set { caivrsAuthClear = value; }
        }

        public bool? LDP_GSANamesMatch
        {
            get { return ldp_GSANamesMatch; }
            set { ldp_GSANamesMatch = value; }
        }
        public bool? SubjectPropInForeclosure
        {
            get { return subjectPropInForeclosure; }
            set { subjectPropInForeclosure = value; }
        }
        public DateTime CurrentDatePlus60
        {
            get { return CurrentDate.AddDays(60); }
        }
        public DateTime CurrentDatePlus90
        {
            get { return CurrentDate.AddDays(90); }
        }
        public bool? CaseNumberNamesAddressMatch
        {
            get { return caseNumberNamesAddressMatch; }
            set { caseNumberNamesAddressMatch = value; }
        }
        public bool? ADP952
        {
            get { return adp952; }
            set { adp952 = value; }
        }
        public bool? ADP958
        {
            get { return adp958; }
            set { adp958 = value; }
        }

        public DateTime CurrentDate
        {
            get { return DateTime.Now; }
        }
        public int PayoffOldHECMId
        {
            get { return payoffOldHECMId; }
            set { payoffOldHECMId = value; }
        }
        public decimal PayoffOldHECMAmount
        {
            get
            {
                decimal res = 0;
                if(payoffOldHECMId>0)
                {
                    res = GetPayoffAmount(payoffOldHECMId);
                }
                return res;
            }
        }
        public string OtherREAddress1
        {
            get { return otherREAddress1; }
            set { otherREAddress1 = value; }
        }
        public string OtherREAddress2
        {
            get { return otherREAddress2; }
            set { otherREAddress2 = value; }
        }
        public string OtherRECity
        {
            get { return otherRECity; }
            set { otherRECity = value; }
        }
        public string OtherREZip
        {
            get { return otherREZip; }
            set { otherREZip = value; }
        }
        public int OtherREStateId
        {
            get { return otherREStateId; }
            set { otherREStateId = value; }
        }
        public string State
        {
            get
            {
                string res = String.Empty;
                if (otherREStateId > 0)
                    res = db.ExecuteScalarString("GetStateNameById", otherREStateId);
                return res;
            }
        }
        public bool? RequestedServicingWorksheet
        {
            get { return requestedServicingWorksheet; }
            set { requestedServicingWorksheet = value; }
        }
        public bool? CollectedServicingWorksheet
        {
            get { return collectedServicingWorksheet; }
            set { collectedServicingWorksheet = value; }
        }
        public decimal? HECMRefiPrincipleLimitOldLoan
        {
            get { return hecmRefiPrincipleLimitOldLoan; }
            set { hecmRefiPrincipleLimitOldLoan = value; }
        }
        public decimal? HECMRefiOrigMIPAmt
        {
            get { return hecmRefiOrigMIPAmt; }
            set { hecmRefiOrigMIPAmt = value; }
        }
        public string AltContactAddress1
        {
            get { return altContactAddress1;}
            set { altContactAddress1 = value;}
        }
        public string AltContactAddress2
        {
            get { return altContactAddress2;}
            set { altContactAddress2 = value;}
        }
        public string AltContactAltPhone
        {
            get { return altContactAltPhone;}
            set { altContactAltPhone = value;}
        }
        public string AltContactCity
        {
            get { return altContactCity;}
            set { altContactCity = value;}
        }
        public string AltContactName
        {
            get { return altContactName;}
            set { altContactName = value;}
        }
        public string AltContactPhone
        {
            get { return altContactPhone;}
            set { altContactPhone = value;}
        }
        public string AltContactRelationship
        {
            get { return altContactRelationship;}
            set { altContactRelationship = value;}
        }
        public int AltContactStateId
        {
            get { return altContactStateId; }
            set { altContactStateId = value; }
        }
        public string AltContactZip
        {
            get { return altContactZip; }
            set { altContactZip = value; }
        }


        #region Calculator
        public bool LineOfCreditYN
        {
            get { return calculatorSettingID == 1 && Calculator.NeedCreditLine && !Calculator.NeedMonthlyIncome; }
        }
        public bool ModifiedTenureYN
        {
            get
            {
                return calculatorSettingID == 1 && Calculator.NeedCreditLine &&
                    Calculator.NeedMonthlyIncome && !Calculator.NeedTerm &&
                    (mp.ProductCalcType == ProductFlag.HECM_Monthly || mp.ProductCalcType == ProductFlag.HECM_Annual);
            }
        }
        public bool ModifiedTermYN
        {
            get
            {
                return calculatorSettingID == 1 && Calculator.NeedCreditLine && 
                    Calculator.NeedMonthlyIncome && Calculator.NeedTerm && 
                    (mp.ProductCalcType == ProductFlag.HECM_Monthly || mp.ProductCalcType == ProductFlag.HECM_Annual);
            }
        }
        public bool TenureYN
        {
            get
            {
                return calculatorSettingID == 1 && !Calculator.NeedCreditLine && 
                    Calculator.NeedMonthlyIncome && !Calculator.NeedTerm; 
            }
        }
        public bool TermYN
        {
            get
            {
                return calculatorSettingID == 1 && !Calculator.NeedCreditLine &&
                    Calculator.NeedMonthlyIncome && Calculator.NeedTerm;
            }
        }
        public bool UndecidedYN
        {
            get { return calculatorSettingID == 2; }
        }
        public int CalculatorSettingID
        {
            get { return calculatorSettingID; }
            set { calculatorSettingID = value; }
        }
        public string CalculatorError
        {
            get { return calculatorError; }
        }
        public AdvCalculator Calculator
        {
            get
            {
                if (calculator != null && isCalculatorValid && calculator.CurrentProduct.ID==productId)
                {
                    return calculator;
                }

                AdvCalculator advCalc = new AdvCalculator(mp,false);
                advCalc.CalculatedForSelectedProduct();
                isCalculatorValid = true;
                calculatorError = advCalc.CalculatorError;
                calculator = advCalc;
                return calculator;
            }
        }
        public bool IsCalculatorReady
        {
            get { return calculator != null && isCalculatorValid && calculator.CurrentProduct.ID == productId; }
        }
        public double InitialInterestRate
        {
            get { return Calculator.PublishedInitialRate; }
        }
        public double InitialRate
        {
            get { return Calculator.PublishedInitialRate; }
        }
        public decimal? Term
        {
            get { return term; }
            set
            {
                if (term == value)
                    return;

                calculator = null;
                term = value;
            }
        }
        public decimal? InitialDraw
        {
            get { return initialDraw; }
            set
            {
                if (initialDraw == value)
                    return;

                calculator = null;
                initialDraw = value;
            }
        }
        public decimal? CreditLine
        {
            get { return creditLine; }
            set
            {
                if (creditLine == value)
                    return;

                calculator = null;
                creditLine = value;
            }
        }
        public decimal? PaymentAmount
        {
            get { return paymentAmount; }
            set
            {
                if (paymentAmount == value)
                    return;

                calculator = null;
                paymentAmount = value;
            }
        }
        public decimal? OtherCashOut
        {
            get { return otherCashOut; }
            set
            {
                if (otherCashOut == value)
                    return;

                calculator = null;
                otherCashOut = value;
            }
        }
        public decimal PrincipalLimit
        {
            get { return Convert.ToDecimal(Calculator.PrincipalLimit); }
        }
        public decimal LenderFee
        {
            get
            {
                decimal res;
                decimal maxClaim = mp.Property.LendingLimit;
                if (mp.Property.SPValue != null)
                {
                    maxClaim = Math.Min(maxClaim, (decimal)mp.Property.SPValue);
                }
                decimal firstPart = maxClaim;
                decimal secondPart = 0;
                if (firstPart > FIRSTPARTLIMIT)
                {
                    firstPart = FIRSTPARTLIMIT;
                    secondPart = maxClaim - firstPart;
                }
                res = firstPart * FIRSTPARTPERCENT;
                res = Math.Max(res, FIRSTPARTSUM);
                if (secondPart > 0)
                {
                    res += secondPart * SECONDPARTPERCENT;
                }
                return Math.Min(res, LENDERFEECAP);
            }
        }
        #endregion
        public bool? OwnOrSoldRealEstate
        {
            get { return ownOrSoldRealEstate; }
            set { ownOrSoldRealEstate = value; }
        }
        public bool OwnOrSoldRealEstateYesYN
        {
            get
            {
                bool res = false;
                if (ownOrSoldRealEstate!=null)
                {
                    res = (bool) ownOrSoldRealEstate;
                }
                return res;
            }
        }
        public bool OwnOrSoldRealEstateNoYN
        {
            get
            {
                bool res = false;
                if (ownOrSoldRealEstate != null)
                {
                    res = !((bool)ownOrSoldRealEstate);
                }
                return res;
            }
        }
        public bool? IsItToBeSold
        {
            get { return isItToBeSold; }
            set { isItToBeSold = value; }
        }
        public bool IsItToBeSoldYesYN
        {
            get
            {
                bool res = false;
                if (isItToBeSold!=null)
                {
                    res = (bool)isItToBeSold;
                }
                return res;
            }
        }
        public bool IsItToBeSoldNoYN
        {
            get
            {
                bool res = false;
                if (isItToBeSold != null)
                {
                    res = !((bool)isItToBeSold);
                }
                return res;
            }
        }
        public decimal? SalesPrice
        {
            get { return salesPrice; }
            set { salesPrice = value; }
        }
        public decimal? OriginalMortgageAmt
        {
            get { return originalMortgageAmt; }
            set { originalMortgageAmt = value; }
        }
        public bool? OwnMoreThan4Dwellings
        {
            get { return ownMoreThan4Dwellings; }
            set { ownMoreThan4Dwellings = value; }
        }
        public bool OwnMoreThan4DwellingsYesYN
        {
            get
            {
                bool res = false;
                if(ownMoreThan4Dwellings!=null)
                {
                    res = (bool)ownMoreThan4Dwellings;
                }
                return res;
            }
        }
        public bool OwnMoreThan4DwellingsNoYN
        {
            get
            {
                bool res = false;
                if (ownMoreThan4Dwellings != null)
                {
                    res = !((bool)ownMoreThan4Dwellings);
                }
                return res;
            }
        }
        public DateTime? ApplicationDate
        {
            get { return applicationDate; }
            set { applicationDate = value; }
        }
        public int TrusteeId
        {
            get { return trusteeId; }
            set { trusteeId = value; trustee = null; }
        }
        public string CorporateSigner
        {
            get { return corporateSigner; }
            set { corporateSigner = value; }
        }
        public string CorporateSignerTitle
        {
            get { return corporateSignerTitle; }
            set { corporateSignerTitle = value; }
        }
        public string ActualSettlementAddress
        {
            get { return actualSettlementAddress; }
            set { actualSettlementAddress = value; }
        }
        public string AcutalCityStateZip
        {
            get { return acutalCityStateZip; }
            set { acutalCityStateZip = value; }
        }
        public decimal FHAMaximumClaimAmount
        {
            get
            {
                decimal res = mp.Property.LendingLimit;
                if (mp.Property.SPValue != null)
                {
                    if ((decimal)mp.Property.SPValue < res) res = (decimal)mp.Property.SPValue;
                }
                return res;
            }

        }
        public DateTime? FundingDate
        {
            get { return fundingDate; }
            set { fundingDate = value; }
        }
        public DateTime? HECMMonthlyNextRateChange
        {
            get { return hecmMonthlyNextRateChange; }
            set { hecmMonthlyNextRateChange = value; }
        }
        public bool? EquityProtection
        {
            get { return equityProtection; }
            set { equityProtection = value; }
        }
        public DateTime? IfMonthlyPaymentFirstPaymentDate
        {
            get
            {
                if (mp.MortgageInfo.ClosingDate == null) return null;
                DateTime dt = (DateTime)mp.MortgageInfo.ClosingDate;
                dt = dt.AddMonths(1);
                dt = dt.AddDays(1 - dt.Day);
                return dt;
            }
        }
        public decimal? AppraisalDepositAmountReturned
        {
            get { return appraisalDepositAmountReturned; }
            set { appraisalDepositAmountReturned = value; }
        }
        public decimal? CashFromBorrowerForLien
        {
            get { return cashFromBorrowerForLien; }
            set { cashFromBorrowerForLien = value; }
        }
        public string ListEndorsements
        {
            get { return listEndorsements; }
            set { listEndorsements = value; }
        }
        public decimal? LendersCoverage
        {
            get { return lendersCoverage; }
            set { lendersCoverage = value; }
        }
        //MortgageInfo.ClosingInstruction1
        public Trustee Trustee
        {
            get
            {
                if (trustee == null)
                {
                    trustee = new Trustee(trusteeId);
                }
                return trustee;
            }
        }
        public Servicer Servicer
        {
            get
            {
                if (servicer == null)
                {
                    servicer = new Servicer(servicerId);
                }
                return servicer;
            }
            
        }
        public decimal MaxYSP
        {
            get
            {
                return Calculator.NetPrincipalLimit * (decimal)YieldSpreadRate / 100;
            }
        }
        public bool IsCalculatorValid
        {
            get { return isCalculatorValid; }
        }
        public int ClosingCostProfileId
        {
            get { return closingCostProfileId; }
            set 
            {
                int oldValue = closingCostProfileId;
                closingCostProfileId = value;
                if (!isInitialLoad)
                {
                    if (oldValue != closingCostProfileId)
                    {
                        mp.ApplyClosingCostProfile(closingCostProfileId, oldValue, LenderFee);
                    }
                }
            }
        }
        public decimal? ClosingCostProfileDetailTotal
        {
            get
            {
                ClosingCostProfile profile = new ClosingCostProfile(closingCostProfileId);
                if (profile.ProfileDetails.Count == 0)
                {
                    return null;
                }
                decimal res = 0;
                for (int i = 0; i < profile.ProfileDetails.Count; i++)
                {
                        //string dbg = profile.ProfileDetails[i]["amount"].ToString();
                        //decimal d = decimal.Parse(profile.ProfileDetails[i]["amount"].ToString());
                    res += decimal.Parse(profile.ProfileDetails[i]["amount"].ToString());
                }
                return res;
            }
        }

        public DateTime? NextRateChange
        {
            get
            {
                DateTime? res = null;
                if(closingDate!=null)
                {
                    if(Product.ArmTypeId == (int)Product.ArmType.Monthly)
                    {
                        res = ((DateTime) closingDate).AddMonths(3);
                    }
                    else if (Product.ArmTypeId == (int)Product.ArmType.Annual)
                    {
                        res = ((DateTime)closingDate).AddYears(1);
                    }
                    if(res!=null)
                    {
                        res = ((DateTime) res).AddDays(-(((DateTime) res).Day - 1));
                    }
                }
                return res;
            }
        }
        public string NextRateChangeSpelled
        {
            get
            {
                string res = String.Empty;
                if (NextRateChange != null)
                {
                    res = ((DateTime)NextRateChange).ToString("MMMM d, yyyy");
                }
                return res;
            }
        }
        public bool CashToBorrower
        {
            get { return FromTo > 0; }
        }
        public bool CashFromBorrower
        {
            get { return FromTo < 0; }
        }
        public decimal FromTo
        {
            get { return TotalPaidByForBorrower - GrossAmountDueFromBorrowerAmount; }
        }
        public decimal TotalPaidByForBorrower
        {
            get { return GrossAmountDueFromBorrowerAmount + InitialDraw ?? 0; }
        }
        public decimal GrossAmountDueFromBorrowerAmount
        {
            get { return TotalClosingCosts1400 + TotalPayoffs; }
        }
        public decimal TotalClosingCosts1400
        {
            get { return mp.TotalInvoiceNonPOC + Calculator.UpFrontPremium; }
        }
        public decimal TotalPayoff
        {
            get
            {
                return Payoff.GetPayoffTotal(mp.ID);
            }
        }
        public decimal TotalPayoffs
        {
            get
            {
                decimal res = 0;
                DataView dv = db.GetDataView(GETTOTALPAYOFFS, mp.ID);
                if(dv.Count==1)
                {
                    res = decimal.Parse(dv[0]["total"].ToString());
                }
                return res;
            }
        }
        public int LenderAffiliateID
        {
            get { return lenderAffiliateID; }
            set
            {
                int oldLenderAffiliateID = lenderAffiliateID;
                lenderAffiliateID = value;
                if (oldLenderAffiliateID < 0 || oldLenderAffiliateID == lenderAffiliateID)
                    return;

                string errMsg;
                mp.UpdateObject("MortgageInfo.ProductId", "0", ID, out errMsg);
                mp.UpdateObject("MortgageInfo.ServicerId", "0", ID, out errMsg);
                mp.UpdateObject("MortgageInfo.InvestorId", "0", ID, out errMsg);
                mp.UpdateObject("MortgageInfo.TrusteeId", "0", ID, out errMsg);
                prodTypeHash = null;
            }
        }
        public Company LenderAffiliate
        {
            get
            {
                if (lenderAffiliate == null || lenderAffiliate.ID != lenderAffiliateID)
                    lenderAffiliate = new Company(lenderAffiliateID);

                return lenderAffiliate;
            }
        }
        public Hashtable ProdTypeHash
        {
            get
            {
                if (prodTypeHash != null)
                    return prodTypeHash;

                DataView dvLenderProduct = mp.CurrentPage.GetDictionary("vwLenderProduct");
                prodTypeHash = new Hashtable();
                
                Product prod = new Product(ProductFlag.HECM_Monthly);
                dvLenderProduct.RowFilter = String.Format("id={0} AND LenderId<>{1}", prod.ID, mp.CurrentPage.CurrentUser.CompanyId);
                if (dvLenderProduct.Count > 0)
                    prodTypeHash[prod.CalculationType] = prod;

                prod = new Product(ProductFlag.HECM_Annual);
                dvLenderProduct.RowFilter = String.Format("id={0} AND LenderId<>{1}", prod.ID, mp.CurrentPage.CurrentUser.CompanyId);
                if (dvLenderProduct.Count > 0)
                    prodTypeHash[prod.CalculationType] = prod;

                prod = new Product(ProductFlag.FNMA);
                dvLenderProduct.RowFilter = String.Format("id={0} AND LenderId<>{1}", prod.ID, mp.CurrentPage.CurrentUser.CompanyId);
                if (dvLenderProduct.Count > 0)
                    prodTypeHash[prod.CalculationType] = prod;

                return prodTypeHash;
            }
        }
        public int CompanyId
        {
            get { return mp.CompanyID; }
        }
        public int MortgageStatusId
        {
            get { return mp.CurProfileStatusID; }
        }
        public int BorrowersCount
        {
            get { return mp.Borrowers.Count; }
        }
        public bool? OnTitleNotApplying
        {
            get { return onTitleNotApplying; }
            set { onTitleNotApplying = value; }
        }
        public bool? AppraisalDepositCollected
        {
            get { return appraisalDepositCollected; }
            set { appraisalDepositCollected = value; }
        }
        public decimal? AppraisalDepositAmount
        {
            get { return appraisalDepositAmount; }
            set { appraisalDepositAmount = value; }
        }
        public bool? AppraisalDepositDeposited
        {
            get { return appraisalDepositDeposited; }
            set { appraisalDepositDeposited = value; }
        }
        public int ProductId
        {
            get { return productId; }
            set
            {
                int oldProductID = productId;
                productId = value;
                if (oldProductID < 0 || oldProductID == productId)
                    return;
                string errMsg;
                mp.UpdateObject("MortgageInfo.ServicerId", "0", ID, out errMsg);
                mp.UpdateObject("MortgageInfo.InvestorId", "0", ID, out errMsg);
                mp.UpdateObject("MortgageInfo.TrusteeId", "0", ID, out errMsg);
            }
        }

        public Product Product
        {
            get
            {
                if (product == null || product.ID != productId)
                    product = new Product(productId);

                return product;
            }
        }
        public string ProductName 
        {
            get 
            { 
                return Product.Name; 
            }
            set 
            { 
                Product.Name = value; 
            }
        }
        public ProductFlag ProductCalcType
        {
            get
            {
                return Product.CalculationType;
            }
            set
            {
                Product.CalculationType = value;
            }
        }
        public DateTime? ClosingDate
        {
            get { return closingDate; }
            set { closingDate = value; }
        }
        public string ClosingDateSpelled
        {
            get
            {
                string res = String.Empty;
                if(closingDate!=null)
                {
                    res = ((DateTime) closingDate).ToString("MMMM d, yyyy");
                }
                return res;
            }
        }
        public string OriginatorHeadquarters 
        {
            get { return originatorHeadquarters; }
            set { originatorHeadquarters = value; }
        }
        public string OriginatorLoanNumber
        {
            get { return originatorLoanNumber; }
            set { originatorLoanNumber = value; }
        }
        public string LenderLoanNumber
        {
            get { return lenderLoanNumber; }
            set { lenderLoanNumber = value; }
        }
        public int TransactionTypeId
        {
            get { return transactionTypeId; }
            set { transactionTypeId = value; }
        }
        public bool TransactionTypePurchaseYN
        {
            get { return transactionTypeId == (int)TransactionTypeEnum.Purchase; }
        }
        public bool TransactionTypeRefiYN
        {
            get { return transactionTypeId == (int)TransactionTypeEnum.Refinance; }
        }
        public string TransactionType
        {
            get { return transactionType; }
            set { transactionType = value; }
        }
        public decimal? PurchaseContractAmount
        {
            get { return purchaseContractAmount; }
            set { purchaseContractAmount = value; }
        }
        public string PurchaseContractAmountLabel
        {
            get
            {
                if (purchaseContractAmount!=null)
                {
                    return ((decimal)purchaseContractAmount).ToString("C");
                }
                else
                {
                    return "";
                }
            }
        }
        public decimal? ContractPriceAmount
        {
            get { return purchaseContractAmount; }
        }
        public decimal? PersonalPropertyAmount
        {
            get { return personalPropertyAmount; }
            set { personalPropertyAmount = value; }
        }
        public decimal? SettlementChargesAmount
        {
            get { return settlementChargesAmount; }
            set { settlementChargesAmount = value; }
        }
        public string SettlementChargesAmountLabel
        {
            get
            {
                if(settlementChargesAmount!=null)
                {
                    return ((decimal) settlementChargesAmount).ToString("C");
                }
                else
                {
                    return "";
                }
            }
        }
        public string personalPropertyAmountLabel
        {
            get
            {
                if(personalPropertyAmount!=null)
                {
                    return ((decimal)personalPropertyAmount).ToString("C");
                }
                else
                {
                    return "";
                }
            }
        }
        public decimal? DepositEarnestMoney
        {
            get { return depositEarnestMoney; }
            set { depositEarnestMoney = value; }
        }
        public string DepositAmount
        {
            get
            {
                if (depositEarnestMoney!=null)
                {
                    return ((decimal) depositEarnestMoney).ToString("C");
                }
                else
                {
                    return "";
                }
            }
        }
        public decimal? PrincipleAmountOfNewLoan
        {
            get { return principleAmountOfNewLoan; }
            set { principleAmountOfNewLoan = value; }
        }
        public decimal? ExistingLoansTakenSubjectTo
        {
            get { return existingLoansTakenSubjectTo; }
            set { existingLoansTakenSubjectTo = value; }
        }
        public decimal? SettlementChargesToBorrower
        {
            get { return settlementChargesToBorrower; }
            set { settlementChargesToBorrower = value; }
        }
        public decimal? CashPortionInitialDrawAmount
        {
            get { return cashPortionInitialDrawAmount; }
            set { cashPortionInitialDrawAmount = value; }
        }
        public decimal TotalSellerPayAmount
        {
            get { return PayingInAdvance.GetPayinfInAdvanceTotal(mp.ID); }
        }
        public decimal TotalBuyerPayAmount
        {
            get { return PayingInAdvance.GetPayinfInAdvanceTotal(mp.ID); }
        }
        public string LessAmountsPaidByForBorrower
        {
            get { return TotalPaidByForBorrower.ToString(); }
        }
        public string ServicerName
        {
            get { return servicerName; }
        }
        public int ServicerId
        {
            get { return servicerId; }
            set { servicerId = value; }
        }
        public int InvestorId
        {
            get { return investorId; }
            set { investorId = value; }
        }
        public string InvestorName
        {
            get { return investorName; }
        }
        public string MortgageDueString
        {
            get 
            {
                string res = "N/A";
                Borrower youngest = mp.YoungestBorrower;
                if ((youngest!=null) && (closingDate!=null))
                {
                    if (mp.YoungestBorrower.DateOfBirth != null)
                    {
                        int years = DateTime.Now.Year - ((DateTime) mp.YoungestBorrower.DateOfBirth).Year;
                        res = ((DateTime) closingDate).AddYears(MORTGAGEDUEYEARS-years).ToShortDateString();
                    }
                }
                return res;
            }
        }
        public bool? CondoRider
        {
            get { return condoRider; }
            set { condoRider = value; }
        }
        public bool? PUDRider
        {
            get { return pudRider; }
            set { pudRider = value; }
        }
        public bool? OtherRider
        {
            get { return otherRider; }
            set { otherRider = value; }
        }
        public string OtherRiderDescription
        { 
            get { return otherRiderDescription; }
            set { otherRiderDescription=value; }
        }
        public decimal MaximumPrincipleLimit
        {
            get { return mp.Calculator.PledgedValueOrLimit*1.5m;}
        }
        public string MaximumPrincipleLimitSpelled
        {
            get 
            {
                string res = String.Empty;
                if(MaximumPrincipleLimit>0)
                {
                    res = NumberToWord.WordFromNumber(MaximumPrincipleLimit);
                }
                return res;
            }
        }
        public string MaximumPrincipleLimitString
        {
            get 
            {
                string res = String.Empty;
                if (MaximumPrincipleLimit > 0)
                {
                    res = MaximumPrincipleLimit.ToString("C");
                }
                return res;
            }
        }
        public string InitialRateSpelled
        {
            get
            {
                string ret = DataHelpers.RemainderToFraction(InitialRate);
                if (!String.IsNullOrEmpty(ret))
                    return NumberToWord.WordFromNumber(InitialRate) + " and " + ret;
                else
                    return NumberToWord.WordFromNumber(InitialRate);
                
            }
        }
        public decimal InitialRateAsDaily
        {
            get { return Convert.ToDecimal(InitialRate / 365.0); }
        }
        public decimal InitialRateAsMonthly
        {
            get { return Convert.ToDecimal(InitialRate / 12.0); }
        }
        public string AdjustTerms
        {
            get { return adjustTerms; }
            set { adjustTerms = value; }
        }
        public double LifetimeRateCap
        {
            get { return Calculator.EffectiveRateCap; }
        }
        public string LifetimeRateCapSpelled
        {
            get
            {
                string ret = DataHelpers.RemainderToFraction(LifetimeRateCap);
                if (!String.IsNullOrEmpty(ret))
                    return NumberToWord.WordFromNumber(LifetimeRateCap) + " and " + ret;
                else
                    return NumberToWord.WordFromNumber(LifetimeRateCap);
            }
        }
        public string FHACaseNumber
        {
            get { return fhaCaseNumber; }
            set { fhaCaseNumber = value; }
        }
        public string FullNameCloser
        {
            get 
            {
                string res = "N/A";
                AppUser closer = (AppUser)mp.AssignedUsers[AppUser.UserRoles.Closer];
                if (closer != null)
                {
                    res = closer.FullName;
                }
                return res;

            }
        }
        public int? ClosingDateDay
        {
            get
            {
                if (closingDate != null)
                {
                    return ((DateTime)closingDate).Day;
                }
                else
                {
                    return null;
                }
            }
        }
        public int? ClosingDateMonth
        {
            get
            {
                if (closingDate != null)
                {
                    return ((DateTime)closingDate).Month;
                }
                else
                {
                    return null;
                }
            }
        }
        public int? ClosingDateYear
        {
            get
            {
                if (closingDate != null)
                {
                    return ((DateTime)closingDate).Year;
                }
                else
                {
                    return null;
                }
            }
        }
        public int CommissionMethodId
        {
            get { return commissionMethodId; }
            set { commissionMethodId = value; }
        }
        public float? CommissionPercentage
        {
            get { return commissionPercentage; }
            set { commissionPercentage = value; }
        }
        public decimal? CommissionFixedAmount
        {
            get { return commissionFixedAmount; }
            set { commissionFixedAmount = value; }
        }
        public int ClosingAgentId
        {
            get { return closingAgentId; }
            set { closingAgentId = value; }
        }
        public VendorGlobal ClosingAgent
        {
            get
            {
                if (closingAgent == null || closingAgent.ID != closingAgentId)
                    closingAgent = new VendorGlobal(closingAgentId);

                return closingAgent;
            }
        }
        public string TitleCompanyName
        {
            get { return ClosingAgent.Name; }
        }
        public string TitleCompanyAddress
        {
            get { return ClosingAgent.CorporateAddress1+" "+ClosingAgent.CorporateAddress2; }
        }
        //public decimal LenderFees
        //{
        //    get { return Invoice.GetInvoiceTotalLenderFees(mp.ID); }
        //}
        public decimal LenderFees
        {
            get { return mp.LenderFees; }
        }
        public decimal OriginationFees
        {
            get { return mp.OriginationFees; }
        }
        //public decimal OriginationFees
        //{
        //    get { return Invoice.GetInvoiceTotalOriginationFees(mp.ID); }
        //}

        public decimal OtherClosingCosts
        {
            get { return mp.OtherClosingCosts; }
        }
        //public decimal OtherClosingCosts
        //{
        //    get { return Invoice.GetInvoiceOtherClosingCosts(mp.ID); }
        //}
        public decimal? SharedAppreciation
        {
            get { return sharedAppreciation; }
            set { sharedAppreciation = value; }
        }
        public bool? ShareAppreciation
        {
            get { return shareAppreciation; }
            set { shareAppreciation = value; }
        }
        public Hashtable ErrMessages
        {
            get
            {
                if (errMessages == null)
                {
                    errMessages = MortgageProfileField.GetErrorMessages("MortgageInfo");
                }
                return errMessages;
            }
        }
        public bool CondoRiderYN
        {
            get
            {
                bool res = false;
                if(condoRider!=null)
                {
                    res = (bool)condoRider;
                }
                return res;
            }
        }
        public bool PUDRiderYN
        {
            get
            {
                bool res = false;
                if(pudRider!=null)
                {
                    res = (bool) pudRider;
                }
                return res;
            }
        }
        public bool OtherRiderYN
        {
            get
            {
                bool res = false;
                if(otherRider!=null)
                {
                    res = (bool) otherRider;
                }
                return res;
            }
        }
        public string TitleOrderNumber
        {
            get { return titleOrderNumber; }
            set { titleOrderNumber = value; }
        }
        public int AppMethodId
        {
            get { return appMethodId; }
            set { appMethodId = value; }
        }
        public bool AppMethodFaceToFaceYN
        {
            get { return appMethodId == (int)AppMethod.FaceToFace; }
        }
        public bool AppMethodMailYN
        {
            get { return appMethodId == (int)AppMethod.Email; }
        }
        public int CounselingMethodId
        {
            get { return counselingMethodId; }
            set { counselingMethodId = value; }
        }
        public bool AppMethodTelephoneYN
        {
            get { return appMethodId == (int)AppMethod.Telephone; }
        }
        public bool? OtherSpecialLoanFeature
        {
            get { return otherSpecialLoanFeature; }
            set { otherSpecialLoanFeature = value; }
        }
        public decimal TotalAmountOfNonRealEstateDebts
        {
            get
            {
                decimal res = 0;
                foreach (Borrower b in mp.Borrowers)
                {
                    res += b.TotalAmountOfNonREDebts == null ? 0 : (decimal)b.TotalAmountOfNonREDebts;
                }
                return res;
            }
        }
        public int? ActualAgeAtClosing
        {
            get
            {
                Borrower youngest = mp.YoungestBorrower;
                if (youngest != null)
                {
                    if (mp.YoungestBorrower.DateOfBirth != null && ClosingDate != null)
                    {
                        int res;
                        res = ((DateTime) ClosingDate).Year - ((DateTime) mp.YoungestBorrower.DateOfBirth).Year;
                        return res;
                        
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        //public DateTime? AppDate
        //{
        //    get { return appDate; }
        //    set { appDate = value; }
        //}
        public DateTime? CaseNumAssignDate
        {
            get { return caseNumAssignDate; }
            set { caseNumAssignDate = value; }
        }
        public DateTime? CaseNumCreateDate
        {
            get { return caseNumCreateDate; }
            set { caseNumCreateDate = value; }
        }
        public bool? CloseInATrust
        {
            get { return closeInATrust; }
            set { closeInATrust = value; }
        }
        public DateTime? CounsCertCousDate
        {
            get { return counsCertCousDate; }
            set { counsCertCousDate = value; }
        }
        public DateTime? CounsCertExpDate
        {
            get { return counsCertExpDate; }
            set { counsCertExpDate = value; }
        }
        public bool? CounsMAApproved
        {
            get { return counsMAApproved; }
            set { counsMAApproved = value; }
        }
        public DateTime? CreditReportRequestDate
        {
            get { return creditReportRequestDate; }
            set { creditReportRequestDate = value; }
        }
        public DateTime? DisbursementDate
        {
            get
            {
                if (RescissionDate != null)
                {
                    DateTime dt = ((DateTime)ClosingDate).AddDays(3);
                    dt = DataHelpers.NextDayAfterHoliday(mp.CompanyID, dt);
                    return dt;
                }
                else
                {
                    return null;
                }
            }
        }
        public bool? EquityShareYN
        {
            get { return equityShareYN; }
            set { equityShareYN = value; }
        }
        public int EscrowItemsId
        {
            get { return escrowItemsId; }
            set { escrowItemsId = value; }
        }
        public bool EscrowItemsInsuranceYN
        {
            get { return escrowItemsId==(int)EscrowItems.Insurance; }
        }
        public bool EscrowItemsTaxesYN
        {
            get { return escrowItemsId == (int)EscrowItems.Taxes; }
        }
        public int? EscrowYears
        {
            get { return escrowYears; }
            set { escrowYears = value; }
        }
        public bool? EscrowYN
        {
            get { return escrowYN; }
            set { escrowYN = value; }
        }
        public DateTime? FloodCertDetermDate
        {
            get { return floodCertDetermDate; }
            set { floodCertDetermDate = value; }
        }        
        public decimal? HowMuchForImprov
        {
            get { return howMuchForImprov; }
            set { howMuchForImprov = value; }
        }
        public double InitialRateLifetimeCap
        {
            get { return Calculator.EffectiveRateCap; }
        }
        public bool JointAppNoYN
        {
            get { return (jointApp!=null) && (jointApp==true); }
        }
        public bool JointAppYesYN
        {
            get { return (jointApp != null) && (jointApp == false); }
        }
        public bool? JointApp
        {
            get { return jointApp; }
            set { jointApp = value; }
        }
        public string NonBorrSpAddress1
        {
            get { return nonBorrSpAddress1; }
            set { nonBorrSpAddress1 = value; }
        }
        public string NonBorrSpAddress2
        {
            get { return nonBorrSpAddress2; }
            set { nonBorrSpAddress2 = value; }
        }
        public DateTime? NonBorrSpBirthDate
        {
            get { return nonBorrSpBirthDate; }
            set { nonBorrSpBirthDate = value; }
        }
        public string NonBorrSpCity
        {
            get { return nonBorrSpCity; }
            set { nonBorrSpCity = value; }
        }
        public string NonBorrSpFirstName
        {
            get { return nonBorrSpFirstName; }
            set { nonBorrSpFirstName = value; }
        }
        public string NonBorrSpLastName
        {
            get { return nonBorrSpLastName; }
            set { nonBorrSpLastName = value; }
        }
        public string NonBorrSpSS
        {
            get { return nonBorrSpSS; }
            set { nonBorrSpSS = value; }
        }
        public int NonBorrSpStateID
        {
            get { return nonBorrSpStateID; }
            set { nonBorrSpStateID = value; }
        }
        public bool NonBorrSpYN
        {
            get { return nonBorrSpYN; }
        }
        public string NonBorrSpZip
        {
            get { return nonBorrSpZip; }
            set { nonBorrSpZip = value; }
        }
        public bool OldEnoughToApply
        {
            get
            {
                if (mp.YoungestBorrower.DateOfBirth != null)
                {
                    DateTime dob = (DateTime)mp.YoungestBorrower.DateOfBirth;
                    return (dob.AddYears(62)<=DateTime.Now.AddDays(90));
                }
                else return false;
            }
        }
        public bool? OptOutFaceToFace
        {
            get { return optOutFaceToFace; }
            set { optOutFaceToFace = value; }
        }
        public bool? OrigCertRecvd
        {
            get { return origCertRecvd; }
            set { origCertRecvd = value; }
        }
        public bool? OtherSpecialLoanFeatureYN
        {
            get { return otherSpecialLoanFeatureYN; }
            set { otherSpecialLoanFeatureYN = value; }
        }
        public decimal PrincipleLimit
        {
            get { return principleLimit; }
            set { principleLimit = value; }
        }
        public bool? ProceedsUsedForImprov
        {
            get { return proceedsUsedForImprov; }
            set { proceedsUsedForImprov = value; }
        }
        public bool ProceedsUsedForImprovNoYN
        {
            get { return proceedsUsedForImprov!=null && proceedsUsedForImprov==false; }
        }
        public bool ProceedsUsedForImprovYesYN
        {
            get { return proceedsUsedForImprov != null && proceedsUsedForImprov == true; }
        }
        public DateTime? RepairEndDate
        {
            get
            {
                if (ClosingDate != null)
                    return ((DateTime)ClosingDate).AddMonths(product.RepairLength);
                else 
                    return null;
            }
        }
        public bool RepairRiderYN
        {
            get { return repairRiderYN; }
        }
        public DateTime? RescissionDate
        {
            get
            {
                if (ClosingDate != null)
                {

                    DateTime dt = ((DateTime) ClosingDate).AddDays(3);
                    dt = DataHelpers.NextDayAfterHoliday(mp.CompanyID, dt);
                    return dt;
                    
                }
                else
                {
                    return null;
                }
            }
        }
        public string SettPlaceAddress1
        {
            get { return settPlaceAddress1; }
            set { settPlaceAddress1 = value; }
        }
        public string SettPlaceAddress2
        {
            get { return settPlaceAddress2; }
            set { settPlaceAddress2 = value; }
        }
        public string SettPlaceCity
        {
            get { return settPlaceCity; }
            set { settPlaceCity = value; }
        }
        public int SettPlaceOptionId
        {
            get { return settPlaceOptionId; }
            set { settPlaceOptionId = value; }
        }
        public int SettPlaceStateId
        {
            get { return settPlaceStateId; }
            set { settPlaceStateId = value; }
        }
        public string SettPlaceZip
        {
            get { return settPlaceZip; }
            set { settPlaceZip = value; }
        }
        public bool SpecialLoanFeatureOtherYN
        {
            get { return specialLoanFeatureOtherYN; }
        }
        public bool? StudentLoanDefalut
        {
            get { return studentLoanDefalut; }
            set { studentLoanDefalut = value; }
        }
        public bool? StudentLoanFlag
        {
            get { return studentLoanFlag; }
            set { studentLoanFlag = value; }
        }
        public bool? TaxLienFlag
        {
            get { return taxLienFlag; }
            set { taxLienFlag = value; }
        }
        public DateTime? TermiteReportInspDate
        {
            get { return termiteReportInspDate; }
            set { termiteReportInspDate = value; }
        }
        public string TrusteeAddress1
        {
            get { return trusteeAddress1; }
            set { trusteeAddress1 = value; }
        }
        public string TrusteeAddress2
        {
            get { return trusteeAddress2; }
            set { trusteeAddress2 = value; }
        }
        public DateTime? TrusteeBirthDate
        {
            get { return trusteeBirthDate; }
            set { trusteeBirthDate = value; }
        }
        public string TrusteeCity
        {
            get { return trusteeCity; }
            set { trusteeCity = value; }
        }
        public string TrusteeFirstName
        {
            get { return trusteeFirstName; }
            set { trusteeFirstName = value; }
        }
        public string TrusteeLastName
        {
            get { return trusteeLastName; }
            set { trusteeLastName = value; }
        }
        public bool? TrusteesNotOnApp
        {
            get { return trusteesNotOnApp; }
            set { trusteesNotOnApp = value; }
        }
        public string TrusteeSS
        {
            get { return trusteeSS; }
            set { trusteeSS = value; }
        }
        public int TrusteeStateId
        {
            get { return trusteeStateId; }
            set { trusteeStateId = value; }
        }
        public bool TrusteeYN
        {
            get { return trusteeYN; }
        }
        public string TrusteeZip
        {
            get { return trusteeZip; }
            set { trusteeZip = value; }
        }
        public bool? UnpaidMortClosedYrAgo
        {
            get { return unpaidMortClosedYrAgo; }
            set { unpaidMortClosedYrAgo = value; }
        }
        public bool? UnpaidMortgageFlag
        {
            get { return unpaidMortgageFlag; }
            set { unpaidMortgageFlag = value; }
        }
        public bool? UnpaidMortgageNotOnTitle
        {
            get { return unpaidMortgageNotOnTitle; }
            set { unpaidMortgageNotOnTitle = value; }
        }
        public bool? VODCoverAmntShort
        {
            get { return vODCoverAmntShort; }
            set { vODCoverAmntShort = value; }
        }
        public bool? VODFromFinacInst
        {
            get { return vODFromFinacInst; }
            set { vODFromFinacInst = value; }
        }
        public int? YoungestBorrowerAge
        {
            get
            {
                if (mp.YoungestBorrower != null && mp.YoungestBorrower.NearestAge != null)
                    return mp.YoungestBorrower.NearestAge;
                else
                    return null;
            }
        }
        public bool MoneyShortFlagUpdate
        {
            set
            {
                if(moneyShortFlag!=value)
                {
                    string errMsg;
                    mp.UpdateObject("MortgageInfo.MoneyShortFlag", value.ToString(), ID, out errMsg);
                }
            }
        }
        public bool MoneyShortFlag
        {
            get { return moneyShortFlag; }
            set
            {
                //if(!isInitialLoad)
                //{
                //    if(moneyShortFlag!=value)
                //    {
                //        string errMsg;
                //        mp.UpdateObject("MortgageInfo.MoneyShortFlag", value.ToString(), ID, out errMsg);
                //    }
                //}
                moneyShortFlag = value;
            }
        }
        public bool? OrderedCreditReport
        {
            get { return orderedCreditReport; }
            set { orderedCreditReport = value; }
        }
        public bool? ReceivedCreditReport
        {
            get { return receivedCreditReport; }
            set { receivedCreditReport = value; }
        }
        public DateTime? CreditReportOrderDate
        {
            get { return creditReportOrderDate; }
            set { creditReportOrderDate = value; }
        }
        public bool CreditReportExpired
        {
            get
            {
                bool res = false;
                if(CreditReportOrderDate!=null)
                {
                     if (DateTime.Now > ((DateTime)CreditReportOrderDate).AddDays(90))
                         res = true;
                }
                return res;
            }
        }
        public bool? BKFlag
        {
            get { return bKFlag; }
            set { bKFlag = value; }
        }
        public bool? BKDischarged
        {
            get { return bKDischarged; }
            set { bKDischarged = value; }
        }
        public int BKTypeId
        {
            get { return bKTypeId; }
            set { bKTypeId = value; }
        }
        public bool? Ch7BKDischarged
        {
            get { return ch7BKDischarged; }
            set { ch7BKDischarged = value; }
        }
        public DateTime? BKDischargeDate
        {
            get { return bKDischargeDate; }
            set { bKDischargeDate = value; }
        }
        public int BKIntentionId
        {
            get { return bKIntentionId; }
            set { bKIntentionId = value; }
        }
        public bool? ForclosureFlag
        {
            get { return forclosureFlag; }
            set { forclosureFlag = value; }
        }
        public bool? RushFlag
        {
            get { return rushFlag; }
            set { rushFlag = value; }
        }
        public bool? JudgementFlag
        {
            get { return judgementFlag; }
            set { judgementFlag = value; }
        }
        public bool? JudgementOnTitle
        {
            get { return judgementOnTitle; }
            set { judgementOnTitle = value; }
        }
        public bool? AppraisalDepositCollectedYN
        {
            get { return appraisalDepositCollectedYN; }
            set { appraisalDepositCollectedYN = value; }
        }
        public bool? BnkStmntCoverAmntShort
        {
            get { return bnkStmntCoverAmntShort; }
            set { bnkStmntCoverAmntShort = value; }
        }
        public bool? CustUseOwnCash
        {
            get { return custUseOwnCash; }
            set { custUseOwnCash = value; }
        }
        public bool? GiftLtrBorrNamesSignOk
        {
            get { return giftLtrBorrNamesSignOk; }
            set { giftLtrBorrNamesSignOk = value; }
        }
        public bool? GiftLtrDonorRelationOk
        {
            get { return giftLtrDonorRelationOk; }
            set { giftLtrDonorRelationOk = value; }
        }
        public bool? GiftLtrIncDollar
        {
            get { return giftLtrIncDollar; }
            set { giftLtrIncDollar = value; }
        }
        public bool? GiftLtrNoPaymentStmntOk
        {
            get { return giftLtrNoPaymentStmntOk; }
            set { giftLtrNoPaymentStmntOk = value; }
        }
        public bool? GiftLtrPaperTrailOK
        {
            get { return giftLtrPaperTrailOK; }
            set { giftLtrPaperTrailOK = value; }
        }
        public bool? GiftLtrReceived
        {
            get { return giftLtrReceived; }
            set { giftLtrReceived = value; }
        }
        public int MethodProveFundsId
        {
            get { return methodProveFundsId; }
            set { methodProveFundsId = value; }
        }
        public bool? RecvdBnkStmnt
        {
            get { return recvdBnkStmnt; }
            set { recvdBnkStmnt = value; }
        }
        public bool? RecvdVOD
        {
            get { return recvdVOD; }
            set { recvdVOD = value; }
        }
        public bool? Yrs5Passed
        {
            get { return yrs5Passed; }
            set { yrs5Passed = value; }
        }
        public DateTime? ClosingPlus180
        {
            get
            {
                if (ClosingDate != null)
                    return ((DateTime)ClosingDate).AddDays(180);
                else 
                    return null;
            }
        }
        public DateTime? ClosingPlus60
        {
            get
            {
                if (ClosingDate != null)
                    return ((DateTime)ClosingDate).AddDays(60);
                else
                    return null;
            }
        }
        public string CounselorName
        {
            get { return counselorName; }
            set { counselorName = value; }
        }
        public DateTime? DateOrigLoanClosed
        {
            get { return dateOrigLoanClosed; }
            set { dateOrigLoanClosed = value; }
            
        }
        public bool EscrowItemsBothYN
        {
            get { return escrowItemsId == (int)EscrowItems.Both; }
        }
        public decimal? HomeValueAtOrigClosing
        {
            get { return homeValueAtOrigClosing; }
            set { homeValueAtOrigClosing = value; }
        }
        public string InterviewerCompanyAddress1
        {
            get { return interviewerCompanyAddress1; }
        }
        public string InterviewerCompanyAddress2
        {
            get { return interviewerCompanyAddress2; }
        }
        public string InterviewerCompanyCity
        {
            get { return interviewerCompanyCity; }
        }
        public string InterviewerCompanyName
        {
            get { return interviewerCompanyName; }
        }
        public int InterviewerCompanyStateId
        {
            get { return interviewerCompanyStateId; }
        }
        public string InterviewerCompanyZip
        {
            get { return interviewerCompanyZip; }
        }
        public string InterviewerName
        {
            get { return interviewerName; }
        }
        public string InterviewerPhoneNumber
        {
            get { return interviewerPhoneNumber; }
        }
        public decimal? LendLimitAtOrigClosing
        {
            get { return lendLimitAtOrigClosing; }
            set { lendLimitAtOrigClosing = value; }
        }
        public string OldFHACaseNumber
        {
            get { return oldFHACaseNumber; }
            set { oldFHACaseNumber = value; }
        }
        public bool? OrdRefiWrkSheet
        {
            get { return ordRefiWrkSheet; }
            set { ordRefiWrkSheet = value; }
        }
        public decimal PeriodicServiceFee
        {
            get { return periodicServiceFee; }
            set { periodicServiceFee = value; }
        }
        public bool? PrincLimitIncr5Times
        {
            get
            {
                return false;
            }
        }
        public bool? RecvdRefiWrkSheet
        {
            get { return recvdRefiWrkSheet; }
            set { recvdRefiWrkSheet = value; }
        }
        public bool? RefiOldHECM
        {
            get { return refiOldHECM; }
            set { refiOldHECM = value; }
        }
        public bool? RefiWrkSheetAssignedToHUD
        {
            get { return refiWrkSheetAssignedToHUD; }
            set { refiWrkSheetAssignedToHUD = value; }
        }
        public decimal RepairAdminFee
        {
            get { return repairAdminFee; }
        }
        public string SpecialLoanFeatureOtherDescription
        {
            get { return specialLoanFeatureOtherDescription; }
        }
        public decimal TotalRepairSetAsides
        {
            get
            {
                decimal res = 0;
                DataView dv = mp.Property.DvRepairItems;
                foreach (DataRowView row in dv)
                {
                    if (row["BidAmount"]!=DBNull.Value) 
                        res += Convert.ToDecimal(row["BidAmount"]);
                }
                return res;
            }
        }
        public string TotalRepairSetAsidesSpelled
        {
            get 
            {
                return NumberToWord.WordFromNumber(TotalRepairSetAsides); 
            }
        }
        public bool NoCheckbox
        {
            get { return false; }
        }
        public bool YesCheckbox
        {
            get { return true; }
        }
        public int ClosingOriginatorId
        {
            get { return closingOriginatorId; }
            set { closingOriginatorId = value; }
        }
        #endregion

        #region constructor
        public MortgageInfo()
            : this(0,null)
        { }
        public MortgageInfo(int id, MortgageProfile mp)
        {
            ID = id;
            this.mp = mp;
            if (id > 0)
            {
                DataView dv = db.GetDataView(GETMORTGAGEINFOBYID, ID);
                if (dv.Count == 1)
                {
                    PopulateFromDataRow(dv[0]);
                }
                else
                {
                    ID = -1;
                }
            }
        }
        public bool? AllApplicantsCompletedHECMCounseling
        {
            get { return allApplicantsCompletedHECMCounseling; }
            set { allApplicantsCompletedHECMCounseling = value; }
        }
        public bool? RequestedFHACase
        {
            get { return requestedFHACase; }
            set { requestedFHACase = value; }
        }
        public bool? CollectedFHACase
        {
            get { return collectedFHACase; }
            set { collectedFHACase = value; }
        }
        public bool? LDPGSACollected
        {
            get { return ldpgsaCollected; }
            set { ldpgsaCollected = value; }
        }
        public bool? CAIVRSCollected
        {
            get { return caivrsCollected; }
            set { caivrsCollected = value; }
        }
        #endregion

        #region methods

        #region public
        public override int Save(MortgageProfile mp_, string objectName, string fullPropertyName, string propertyName, int objectTypeId, object val, object oldVal, string parentFieldName, int parentId, bool isRequired)
        {
            int res = base.Save(mp_, MORTGAGEINFOTABLE, fullPropertyName, propertyName, objectTypeId, val, oldVal, parentFieldName, parentId, isRequired);
            if ((res > 0) && (ID < 1))
            {
                mp_.MortgageInfo.ID = res;
            }
            return res;
        }
        public override bool ValidateProperty(string propertyName, out string err)
        {
            bool res = true;
            err = String.Empty;
            switch (propertyName)
            {
                case "FHACaseNumber":
                    if (!String.IsNullOrEmpty(fhaCaseNumber))
                    {
                        res = !(fhaCaseNumber.Length != 10);
                        if (!res)
                        {
                            GetError(propertyName, out err);
                        }
                    }
                    break;
            }
            return res;
        }
        public void ResetCalculator()
        {
            isCalculatorValid = false;
        }
        public void SetCalculator(AdvCalculator calc)
        {
            calculator = calc;
            isCalculatorValid = true;
        }

        #endregion

        #region private
        private static decimal GetPayoffAmount(int payoffId)
        {
            decimal res = 0;
            DataView dv = db.GetDataView(GETPAYOFFAMONT, payoffId);
            if(dv!=null&&dv.Count==1)
            {
                res = decimal.Parse(dv[0]["amount"].ToString());
            }
            return res;
        }

        private void GetError(string propertyName, out string err)
        {
            if (ErrMessages.ContainsKey(propertyName))
            {
                err = ErrMessages[propertyName].ToString();
            }
            else
            {
                err = "*";
            }
        }
        //private string GetProductName()
        //{
        //    return db.ExecuteScalarString(GETPRODUCTNAMEBYID, productId);
        //}

        #endregion

        #endregion

    }
}
