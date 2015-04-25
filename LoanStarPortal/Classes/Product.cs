using System;
using System.Data;
using BossDev.CommonUtils;

namespace LoanStar.Common
{
    public class Product : BaseObject
    {
        public class ProductException : BaseObjectException
        {
            public ProductException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public ProductException(string message)
                : base(message)
            {
            }

            public ProductException()
            {
            }
        }

        public class ProductRate
        {
            #region fields
            private int id;
            private int productId;
            private DateTime period;
            private double dailyInitialIndex;
            private double weeklyAveInitialIndex;
            private double margin;
            private double weeklyAveInitialRate;
            private double publishedInitialRate;
            private double dailyExpectedIndex;
            private double weeklyAveExpectedIndex;
            private double weeklyAveExpectedRate;
            private double publishedExpectedRate;
            private double publishedInitialIndex;
            private double publishedInitialMargin;
            private double publishedExpectedIndex;
            private double publishedExpectedMargin;
            #endregion

            #region properties
            public DateTime Period
            {
                get { return period; }
            }
            public double Margin
            {
                get { return publishedInitialMargin; }
            }
            public double ExpectedMargin
            {
                get { return publishedInitialMargin; }
            }
            public double PublishedExpectedIndex
            {
                get { return publishedExpectedIndex; }
            }
            public double PublishedInitialIndex
            {
                get { return publishedInitialIndex; }
            }
            public double PublishedInitialRate
            {
                get { return publishedInitialRate; }
            }
            public double PublishedExpectedRate
            {
                get { return publishedExpectedRate; }
            }
            #endregion

            #region constructor
            public ProductRate(DataRow row)
            {
                LoadFromDataRow(row);
            }

            #endregion

            #region methods
            private void LoadFromDataRow(DataRow row)
            {
                id = Convert.ToInt32(row["id"]);
                productId = Convert.ToInt32(row["ProductId"]);
                period = Convert.ToDateTime(row["Period"]);
                if (row["Margin"] != DBNull.Value)
                {
                    margin = Convert.ToDouble(row["Margin"]);
                }
                if (row["DailyInitialIndex"] != DBNull.Value)
                {
                    dailyInitialIndex = Convert.ToDouble(row["DailyInitialIndex"]);
                }
                if (row["WeeklyAveInitialIndex"]!=DBNull.Value)
                {
                    weeklyAveInitialIndex = Convert.ToDouble(row["WeeklyAveInitialIndex"]);
                }
                if (row["WeeklyAveInitialRate"] != DBNull.Value)
                {
                    weeklyAveInitialRate = Convert.ToDouble(row["WeeklyAveInitialRate"]);
                }
                if (row["PublishedInitialRate"] != DBNull.Value)
                {
                    publishedInitialRate = Convert.ToDouble(row["PublishedInitialRate"]);
                }
                if (row["DailyExpectedIndex"] != DBNull.Value)
                {
                    dailyExpectedIndex = Convert.ToDouble(row["DailyExpectedIndex"]);
                }
                if (row["WeeklyAveExpectedIndex"] != DBNull.Value)
                {
                    weeklyAveExpectedIndex = Convert.ToDouble(row["WeeklyAveExpectedIndex"]);
                }
                if (row["WeeklyAveExpectedRate"] != DBNull.Value)
                {
                    weeklyAveExpectedRate = Convert.ToDouble(row["WeeklyAveExpectedRate"]);
                }
                if (row["PublishedExpectedRate"] != DBNull.Value)
                {
                    publishedExpectedRate = Convert.ToDouble(row["PublishedExpectedRate"]);
                }
                if (row["PublishedInitialIndex"] != DBNull.Value)
                {
                    publishedInitialIndex = Convert.ToDouble(row["PublishedInitialIndex"]);
                }
                if (row["PublishedInitialMargin"] != DBNull.Value)
                {
                    publishedInitialMargin = Convert.ToDouble(row["PublishedInitialMargin"]);
                }
                if (row["PublishedExpectedIndex"] != DBNull.Value)
                {
                    publishedExpectedIndex = Convert.ToDouble(row["PublishedExpectedIndex"]);
                }
                if (row["PublishedExpectedMargin"] != DBNull.Value)
                {
                    publishedExpectedMargin = Convert.ToDouble(row["PublishedExpectedMargin"]);
                }
            }
            #endregion
        }

        
        #region enums
        public enum AmortizationType
        {
            Arm = 1,
            Fixed = 2,
            Other = 3
        }
        public enum ArmType
        {
          Monthly = 1,
          Annual = 2,
          Other = 3  
        }
        public enum ProductType
        {
            Homekeeper = 1,
            HECM = 2,
            Other = 3
        }
        public enum ProductTypeSettStmnt
        {
            CONVINS=1,
            CONVUNINS=2,
            FHA=3,
            FmHA=4,
            VA=5
        }
        public enum ProductIndex
        {
        	OneYearCMT  = 1,
            OneMonthCMT = 2,
            OneMonthLibor = 3,
            OneYearLibor = 4,
            Libor = 5,
            Prime = 6
        }
        public enum ProductRateLockMethod
        {
            None =0,
            PrincipalLimetProtection = 1,
            RateLock = 2
        }
        public enum ProductRateUpdateInterval
        {
            WeeklyOnTuesday = 1,
            Daily = 2
        }
        public enum ProductRateInputMethod
        {
            CMT_1Yr_10Yr = 1,
            Manual_Entry = 2
        }
        #endregion

        #region constants
        private const string GETPRODUCTLIST = "GetProductList";
        private const string GETPRODUCTBYID = "GetProductById";
        private const string GETPRODUCTTYPELIST = "GetProductTypeList";
        private const string GETPRODUCTINDEX = "GetProductIndex";
        private const string DELETE = "DeleteProduct";
        private const string SAVE = "SaveProduct";
        private const string NAMEFIELDNAME = "name";
        private const string MARGINFIELDNAME = "Margin";
        private const string RATEROUNDINGPRECISIONFIELDNAME = "RateRoundingPrecision";
        private const string RATEROUNDINGMETHODIDFIELDNAME = "RateRoundingMethodId";
        private const string BASISIDFIELDNAME = "BasisId";
        private const string PAYMENTSPERYEARFIELDNAME = "PaymentsPerYear";
        private const string PROPERTYAPPRECIATIONFIELDNAME = "PropertyAppreciation";
        private const string UPFRONTMORTGAGEINSURANCERATEFIELDNAME = "UpfrontMortgageInsuranceRate";
        private const string RENEWALMORTGAGEINSURANCERATEFIELDNAME = "RenewalMortgageInsuranceRate";
        private const string FIRSTYEARPROPERTYCHARGESFIELDNAME = "FirstYearPropertyCharges";
        private const string ENDOFMONTHFLAGFIELDNAME = "EndOfMonthFlag";
        private const string RELATIVERATECAPFIELDNAME = "RelativeRateCap";
        private const string SHAREDAPPRECIATIONFIELDNAME = "SharedAppreciation";
        private const string AMORTIZATIONTYPEIDFIELDNAME = "amortizationtypeid";
        private const string AMORTIZATIONTYPEOTHERFIELDNAME = "AmortizationTypeOther";
        private const string ARMTYPEIDFIELDNAME = "ArmTypeId";
        private const string ARMTYPEOTHERFIELDNAME = "ArmTypeOther";
        private const string TYPEIDFIELDNAME = "TypeId";
        private const string TYPEOTHERFIELDNAME = "TypeOther";
        private const string OTHERDESCRIPTIONFIELDNAME = "otherDescription";
        private const string SPECIALLOANFEATUREFIELDNAME = "OtherSpecialLoanFeatureAllowed";
        private const string SPECIALFETUREDESCRIPTIONFIELDNAME = "OtherSpecialLoanFeatureDescription";
        private const string CONFORMINGLENDINGLIMITFIELDNAME = "ConformingLendingLimit";
        private const string PRIMARYRESIDENSEREQUIREDFIELDNAME = "PrimaryResidenceRequired";
        private const string ALLOWPRIMARYRESFIELDNAME = "AllowPrimaryRes";
        private const string ALLOWSECONDHOMEFIELDNAME = "AllowSecondHome";
        private const string ALLOWINVESTMENTPROPFIELDNAME = "AllowInvestmentProp";
        private const string APPRAISALREQUIREDFIELDNAME = "appraisalRequired";
        private const string PROPINSPREQUIREDFIELDNAME = "PropInspRequired";
        private const string USESTANDARDFLOODGUIDESFIELDNAME = "useStandardFloodGuides";
        private const string MAXFLOODDEDUCTIBLEFIELDNAME = "maxFloodDeductible";
        private const string USESTANDARDHAZARDGUIDESFIELDNAME = "UseStandardHazardGuides";
        private const string MAXHAZDEDUCTPERCENTFIELDNAME = "MaxHazDeductPercent";
        private const string REPAIRLENGTHFIELDNAME = "RepairLength";
        private const string ALLOWHECMREFIFIELDNAME = "AllowHECMRefi";
        private const string ALLOWMULTIFAMILYPROPFIELDNAME = "AllowMultiFamilyProp";
        private const string USESTANDARDGUIDEWELLSEPTICFIELDNAME = "UseStandardGuidesWellSeptic";
        private const string USESTNDGUIDESCASHTOCLOSEFIELDNAME = "UseStndGuidesCashToClose";
        private const string DAYSADVANCEPAYTAXFIELDNAME = "DaysAdvancePayTax";
        private const string USESTANDARDTRUSTGUIDEFIELDNAME = "UseStandardTrustGuides";
        private const string ALLOWMANUHOMESFIELDNAME = "AllowManuHomes";
        private const string RATECHANGEDAYIDFIELDNAME = "rateChangeDayId";
        private const string CREDITREPORTREQUIREDYNFIELDNAME = "CreditReportRequiredYN";
        private const string MINIMUMAGEFIELDNAME = "MinimumAge";
        private const string RELATIVEADJUSTMENTRATECAPFIELDNAME = "RelativeAdjustmentRateCap";
        private const string SECTIONOFTHEACTFIELDNAME = "SectionOfTheAct";
        private const string SPECIALRIDERDESCRIPTIONFIELDNAME = "SpecialRiderDescription";
        private const string SPECIALRIDERYNFIELDNAME = "SpecialRiderYN";
        private const string TYPESETTSTMNTIDFIELDNAME = "typeSettStmntId";
        private const string USEFHAGUIDESREPAIRSFIELDNAME = "UseFHAGuidesRepairs";
        private const string USEFHAGUIDESTERMITEFIELDNAME = "UseFHAGuidesTermite";
        private const string CONVENTIONALMORTGAGELOANVALUEFIELDNAME = "ConventionalMortgageLoan";
        private const string PRODUCTINDEXIDFIELDNAME = "productindexid";
        private const string GETPRODUCTAMORTIZATIONTYPELIST = "GetProductAmortizationTypeList";
        private const string GETPRODUCTARMTYPELIST = "GetProductArmTypeList";
        private const string GETDAYSLIST = "GetDaysList";
        private const string GETPRODUCTIDS = "GetProductsIds";
        private const string GETPRODUCTNAMES = "GetProductsNames";
        private const string GETPRODUCTLISTFORORIGINATOR = "GetOriginatorProductsList";
        private const string GETALLPRODUCTSWITHLENDERSFORORIGINATOR = "GetAllProducstWithLendersForOriginator";
        private const string GETLENDERSERVICEFEE = "GetCompanyFeeForProduct";
        private const string COPYRATES = "CopyProductRates";
        private const string GETBAYDOCCODES = "GetBaydocsCodes";
        private const string UPDATERATES = "UpdateRates";
        private const string SAVERATES = "SaveRates";
        private const string DELETERATES = "DeleteRates";
        private const string GETPRODUCTLISTTOCOPYRATES = "GetProductListToCopyRates";
        private const decimal CONVENTIONALMORTGAGELOANVALUE = 417000.0m;
        private const int APPPACKAGETYPE = 1;
        private const int CLOSINGPACKAGETYPE = 2;
        #endregion

        #region fields
        private string name = String.Empty;
        private ProductFlag calculationType = ProductFlag.None;
        private double margin = 0;
        private decimal rateRoundingPrecision = 0;
        private int rateRoundingMethodId = 0;
        private int basisId = 0;
        private int paymentsPerYear = 12;
        private decimal propertyAppreciation = 0;
        private decimal upfrontMortgageInsuranceRate = 0;
        private decimal renewalMortgageInsuranceRate = 0;
        private int firstYearPropertyCharges = 0;
        private bool endOfMonthFlag = false;
        private decimal relativeRateCap = 0;
        private bool sharedAppreciation = false;
        private int amortizationTypeId = 0;
        private int armTypeId = 0;
        private string armTypeOther;
        private int typeId = 0;
        private string typeOther;
        private string otherDescription;
        private bool otherSpecialLoanFeatureAllowed;
        private string otherSpecialLoanFeatureDescription;
        private string amortizationTypeOther;
        private int rateChangeDayId = 0;
        private decimal conformingLendingLimit = 0;
        private bool primaryResidenceRequired;
        private bool allowPrimaryRes;
        private bool allowSecondHome;
        private bool allowInvestmentProp;
        private bool appraisalRequired;
        private bool propInspRequired;
        private bool useStandardFloodGuides;
        private decimal maxFloodDeductible;
        private bool useStandardHazardGuides;
        private decimal maxHazDeductPercent;
        private int repairLength = 0;
        private bool allowHECMRefi;
        private bool allowMultiFamilyProp;
        private bool useStandardGuidesWellSeptic;
        private bool useStndGuidesCashToClose;
        private int daysAdvancePayTax;
        private bool allowManuHomes;
        private bool useStandardTrustGuides;
        private bool creditReportRequiredYN;
        private int minimumAge = 0;
        private decimal relativeAdjustmentRateCap = 0;
        private string sectionOfTheAct = String.Empty;
        private string specialRiderDescription = String.Empty;
        private bool? specialRiderYN;
        private int typeSettStmntId = 0;
        private bool? useFHAGuidesRepairs;
        private bool? useFHAGuidesTermite;
        private string repairAdminFormula;
        private DateTime? nextMonthlyRateChange;
        private DateTime? nextAnnualRateChange;
        private decimal monthlyMIPRate = 0;
        private decimal dailyMIPRate = 0;
        private decimal conventionalMortgageLoan = CONVENTIONALMORTGAGELOANVALUE;
        private int productIndexId = 0;

        private bool hecmGuidesCollectingCounsCert;
        private bool hecmGuidesReviewingCounsCert;
        private bool hecmGuidesCollectingFHACase;
        private bool hecmGuidesReviewingFHACase;
        private bool hecmGuidesCollectingFHACondoApproval;
        private bool hecmGuidesReviewingFHACondoApproval;
        private bool hecmGuidesCollectingAppraisal;
        private bool hecmGuidesCollectingTermiteInspection;
        private bool hecmGuidesReviewingAppraisal;
        private bool hecmGuidesReviewingTermiteInspections;        
        private bool hecmGuidesCollectingContractorBids;
        private bool hecmGuidesReviewingContractorBids;
        private bool hecmGuidesCollectingStructuralInspections;
        private bool hecmGuidesCollectingWaterTests;
        private bool hecmGuidesReviewingWaterTests;
        private bool hecmGuidesCollectingSepticInspections;
        private bool hecmGuidesReviewingSepticInspections;
        private bool hecmGuidesCollectingOilTankInspections;
        private bool hecmGuidesReviewingOilTankInspections;
        private bool hecmGuidesCollectingRoofInspections;        
        private bool hecmGuidesReviewingRoofInspections;
        private bool hecmGuidesCollectingPOAandConservator;
        private bool hecmGuidesReviewingPOA;
        private bool hecmGuidesReviewingConservator;
        private bool hecmGuidesCollectingCreditReports;
        private bool hecmGuidesReviewingCreditReports;
        private bool hecmGuidesCollectingLDP_GSAPrintout;
        private bool hecmGuidesReviewingLDP_GSAPrintout;
        private bool hecmGuidesCollectingCAIVRSAuthPrintout;
        private bool hecmGuidesReviewingCAIVRSAuthPrintout;
        private bool hecmGuidesCollectingTrusts;
        private bool hecmGuidesReviewingTrusts;
        private bool basicGuidesCollectingDeathCerts;
        private bool basicGuidesReviewingDeathCerts;
        private bool basicGuidesCollectingUSPS;
        private bool basicGuidesReviewingUSPS;
        private bool basicGuidesCollectingTitle;
        private bool basicGuidesReviewingTitle;
        private bool basicGuidesCollectingProofOfAge;
        private bool basicGuidesReviewingProofOfAge;
        private bool basicGuidesCollectingSSN;
        private bool basicGuidesReviewingSSN;
        private bool basicGuidesCollectingFloodCertificates;
        private bool basicGuidesReviewingFloodCertificates;
        private bool basicGuidesCollectingHazardDecPages;
        private bool basicGuidesReviewingHazardDecPages;
        private bool basicGuidesCollectingFloodDecPages;
        private bool basicGuidesReviewingFloodDecPages;
        private int baydocsAppPackageCodeId = 0;
        private int baydocsClosingPackageCodeId = 0;
        private bool equityProtection;
//        private double yieldSpreadRate = 0;
//        private bool principleLimitProtectionYN =false;
        private int daysToLock=0;
        private double expectedFloorRate = 0;

        private int counsActiveDays = 0;
        private int titleActiveDays = 0;
        private int appraisalActiveDays = 0;
        private int pestActiveDays = 0;
        private int bidActiveDays = 0;

        private int waterTestActiveDays = 0;
        private int septicInspActiveDays = 0;
        private int oilTankInspActiveDays = 0;
        private int roofInspActiveDays = 0;
        private int floodCertActiveDays = 0;

        private int creditReportActiveDays = 0;
        private int ldpActiveDays = 0;
        private int eplsactiveDays = 0;
        private int caivrsActiveDays = 0;

        private bool followStandardNBSGuides;
        private bool hecmGuidesCollectingLeases;
        private bool ageEligRequirementApply;
        private bool ageEligRequirementClose;
        private int minAgeToApply = 0;
        private int minAgeToClose = 0;

        private bool basicGuidesCollectingHOAHazardDecPages;
        private bool basicGuidesReviewingHOAHazardDecPages;
        private bool basicGuidesCollectingMasterFloodDecPages;
        private bool useSRPLocksYN;
        private bool hecmGuidesReviewingLeases;
        private bool hecmGuidesCollectingConservator;
        private bool basicGuidesReviewingHOAFloodDecPages;
        private bool hecmGuidesReviewingStructuralInspections;
        private decimal lendingLimit = 0;
        private bool hecmGuidesCollectingPOA;
        private bool allowEscrowTaxAndInsurance;
        private bool allowEscrowRepiars;
        private int fixedRateLockDays = 0;
        private int productRateLockMethodId = 0;
        private int productRateUpdateIntervalId = 1;
        private int productRateInputMethodId = 1;
        private bool useBaydocsAppPackagesYN = true;
            
        #endregion

        #region properties
        public bool UseBaydocsAppPackagesYN
        {
            get { return useBaydocsAppPackagesYN;}
            set { useBaydocsAppPackagesYN = value; }
        }
        public int ProductRateInputMethodId
        {
            get { return productRateInputMethodId; }
            set { productRateInputMethodId = value; }
        }
        public int ProductRateUpdateIntervalId
        {
            get { return productRateUpdateIntervalId; }
            set { productRateUpdateIntervalId = value; }
        }
        public int ProductRateLockMethodId
        {
            get { return productRateLockMethodId; }
            set { productRateLockMethodId = value; }
        }
        public bool FixedRateLocksYN
        {
            get { return productRateLockMethodId == (int)ProductRateLockMethod.RateLock; }
        }
        public int FixedRateLockDays
        {
            get { return fixedRateLockDays; }
            set { fixedRateLockDays = value; }
        }

        public bool AllowEscrowTaxAndInsurance
        {
            get { return allowEscrowTaxAndInsurance; }
            set { allowEscrowTaxAndInsurance = value; }
        }
        public bool AllowEscrowRepiars
        {
            get { return allowEscrowRepiars; }
            set { allowEscrowRepiars = value; }
        }
        public bool HECMGuidesCollectingPOA
        {
            get { return hecmGuidesCollectingPOA; }
            set { hecmGuidesCollectingPOA = value; }
        }
        public decimal LendingLimit
        {
            get { return lendingLimit; }
            set { lendingLimit = value; }
        }
        public bool HECMGuidesReviewingStructuralInspections
        {
            get { return hecmGuidesReviewingStructuralInspections; }
            set { hecmGuidesReviewingStructuralInspections = value; }
        }
        public bool UseSRPLocksYN
        {
            get { return useSRPLocksYN; }
            set { useSRPLocksYN = value; }
        }
        public bool HECMGuidesReviewingLeases
        {
            get { return hecmGuidesReviewingLeases; }
            set { hecmGuidesReviewingLeases = value; }
        }
        public bool HECMGuidesCollectingConservator
        {
            get { return hecmGuidesCollectingConservator; }
            set { hecmGuidesCollectingConservator = value; }
        }
        public bool BasicGuidesReviewingHOAFloodDecPages
        {
            get { return basicGuidesReviewingHOAFloodDecPages; }
            set { basicGuidesReviewingHOAFloodDecPages = value; }
        }

        public bool BasicGuidesCollectingMasterFloodDecPages
        {
            get { return basicGuidesCollectingMasterFloodDecPages; }
            set { basicGuidesCollectingMasterFloodDecPages = value; }
        }
        public bool BasicGuidesCollectingHOAHazardDecPages
        {
            get { return basicGuidesCollectingHOAHazardDecPages; }
            set { basicGuidesCollectingHOAHazardDecPages = value; }
        }
        public bool BasicGuidesReviewingHOAHazardDecPages
        {
            get { return basicGuidesReviewingHOAHazardDecPages; }
            set { basicGuidesReviewingHOAHazardDecPages = value; }
        }

        public bool FollowStandardNBSGuides
        {
            get { return followStandardNBSGuides; }
            set { followStandardNBSGuides= value;}
        }
        public bool HECMGuidesCollectingLeases
        {
            get { return hecmGuidesCollectingLeases; }
            set { hecmGuidesCollectingLeases= value;}
        }
        public bool AgeEligRequirementApply
        {
            get { return ageEligRequirementApply; }
            set { ageEligRequirementApply= value;}
        }
        public bool AgeEligRequirementClose
        {
            get { return ageEligRequirementClose; }
            set { ageEligRequirementClose= value;}
        }
        public int MinAgeToApply
        {
            get { return minAgeToApply; }
            set { minAgeToApply= value;}
        }
        public int MinAgeToClose
        {
            get { return minAgeToClose; }
            set { minAgeToClose = value; }
        }
        public int CounsActiveDays
        {
            get { return counsActiveDays; }
            set { counsActiveDays = value; }
        }
        public int TitleActiveDays
        {
            get { return titleActiveDays; }
            set { titleActiveDays = value; }
        }
        public int AppraisalActiveDays
        {
            get { return appraisalActiveDays; }
            set { appraisalActiveDays = value; }
        }
        public int PestActiveDays
        {
            get { return pestActiveDays; }
            set { pestActiveDays = value; }
        }
        public int BidActiveDays
        {
            get { return bidActiveDays; }
            set { bidActiveDays = value; }
        }
        public int WaterTestActiveDays
        {
            get { return waterTestActiveDays; }
            set { waterTestActiveDays = value; }
        }
        public int SepticInspActiveDays
        {
            get { return septicInspActiveDays; }
            set { septicInspActiveDays = value; }
        }
        public int OilTankInspActiveDays
        {
            get { return oilTankInspActiveDays; }
            set { oilTankInspActiveDays = value; }
        }
        public int RoofInspActiveDays
        {
            get { return roofInspActiveDays; }
            set { roofInspActiveDays = value; }
        }
        public int FloodCertActiveDays
        {
            get { return floodCertActiveDays; }
            set { floodCertActiveDays = value; }
        }
        public int CreditReportActiveDays
        {
            get { return creditReportActiveDays; }
            set { creditReportActiveDays = value; }
        }
        public int LDPActiveDays
        {
            get { return ldpActiveDays; }
            set { ldpActiveDays = value; }
        }
        public int EPLSActiveDays
        {
            get { return eplsactiveDays; }
            set { eplsactiveDays = value; }
        }
        public int CaivrsActiveDays
        {
            get { return caivrsActiveDays; }
            set { caivrsActiveDays = value; }
        }
        public double ExpectedFloorRate
        {
            get { return expectedFloorRate; }
            set { expectedFloorRate = value; }
        }
        public bool PrincipleLimitProtectionYN
        {
            get { return productRateLockMethodId == (int)ProductRateLockMethod.PrincipalLimetProtection; }
        }
        public int DaysToLock
        {
            get{ return daysToLock;}
            set { daysToLock = value;}
        }
        //public double YieldSpreadRate
        //{
        //    get { return yieldSpreadRate; }
        //    set { yieldSpreadRate = value; }
        //}
        public bool EquityProtection
        {
            get { return equityProtection; }
            set { equityProtection = value; }
        }
        public int BaydocsAppPackageCodeId
        {
            get { return baydocsAppPackageCodeId; }
            set { baydocsAppPackageCodeId = value; }
        }
        public int BaydocsClosingPackageCodeId
        {
            get { return baydocsClosingPackageCodeId; }
            set { baydocsClosingPackageCodeId = value; }
        }
        public bool IndexOneMonthCMTYN
        {
            get { return productIndexId == (int) ProductIndex.OneMonthCMT; }
        }
        public bool IndexOneYearCMTYN
        {
            get { return productIndexId == (int)ProductIndex.OneYearCMT; }
        }
        public bool IndexOneMonthLiborYN
        {
            get { return productIndexId == (int)ProductIndex.OneMonthLibor; }
        }
        public bool IndexOneYearLiborYN
        {
            get { return productIndexId == (int)ProductIndex.OneYearLibor; }
        }
        public bool IndexLiborYN
        {
            get { return productIndexId == (int)ProductIndex.Libor; }
        }
        public bool IndexPrimeYN
        {
            get { return productIndexId == (int)ProductIndex.Prime; }
        }
        public int ProductIndexId
        {
            get { return productIndexId; }
            set { productIndexId = value; }
        }
        public bool TypeHomekeeperYN
        {
            get
            {
                return typeId == (int) ProductType.Homekeeper;
            }
        }
        public bool CreditReportRequiredYN
        {
            get { return creditReportRequiredYN; }
            set { creditReportRequiredYN = value; }
        }
        public bool UseStandardTrustGuides
        {
            get { return useStandardTrustGuides; }
            set { useStandardTrustGuides = value; }
        }
        public bool AllowManuHomes
        {
            get { return allowManuHomes; }
            set { allowManuHomes=value; }
        }
        public bool AllowHECMRefi
        {
            get { return allowHECMRefi; }
            set { allowHECMRefi = value; }
        }
        public bool AllowMultiFamilyProp
        {
            get { return allowMultiFamilyProp; }
            set { allowMultiFamilyProp = value; }
        }
        public bool UseStandardGuidesWellSeptic
        {
            get { return useStandardGuidesWellSeptic; }
            set { useStandardGuidesWellSeptic = value; }
        }
        public bool UseStndGuidesCashToClose
        {
            get { return useStndGuidesCashToClose; }
            set { useStndGuidesCashToClose = value; }
        }
        public int DaysAdvancePayTax
        {
            get { return daysAdvancePayTax; }
            set { daysAdvancePayTax = value; }
        }
        public bool UseStandardHazardGuides
        {
            get { return useStandardHazardGuides; }
            set { useStandardHazardGuides = value; }
        }
        public decimal MaxHazDeductPercent
        {
            get { return maxHazDeductPercent; }
            set { maxHazDeductPercent = value; }
        }
        public decimal MaxFloodDeductible
        {
            get { return maxFloodDeductible; }
            set { maxFloodDeductible = value; }
        }
        public bool UseStandardFloodGuides
        {
            get { return useStandardFloodGuides; }
            set { useStandardFloodGuides = value; }
        }
        public bool PrimaryResidenceRequired
        {
            get { return primaryResidenceRequired; }
            set { primaryResidenceRequired = value; }
        }
        public bool AllowPrimaryRes
        {
            get { return allowPrimaryRes; }
            set { allowPrimaryRes = value; }
        }
        public bool AllowSecondHome
        {
            get { return allowSecondHome; }
            set { allowSecondHome = value; }
        }
        public bool AllowInvestmentProp
        {
            get { return allowInvestmentProp; }
            set { allowInvestmentProp = value; }
        }
        public bool AppraisalRequired
        {
            get { return appraisalRequired; }
            set { appraisalRequired = value; }
        }
        public bool PropInspRequired
        {
            get { return propInspRequired; }
            set { propInspRequired = value; }
        }
        public ProductFlag CalculationType
        {
            get
            {
                return calculationType;
            }
            set
            {
                calculationType = value;
            }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public double Margin
        {
            get { return margin; }
            set { margin = value; }
        }
        public string MarginSpelled
        {
            get
            {
                string ret = DataHelpers.RemainderToFraction(Margin);
                if (!String.IsNullOrEmpty(ret))
                    return NumberToWord.WordFromNumber(Margin) + " and " + ret;
                else
                    return NumberToWord.WordFromNumber(Margin);
            }
        }
        public decimal RateRoundingPrecision
        {
            get { return rateRoundingPrecision; }
            set { rateRoundingPrecision = value; }
        }
        public int RateRoundingMethodId
        {
            get { return rateRoundingMethodId; }
            set { rateRoundingMethodId = value; }
        }
        public int BasisId
        {
            get { return basisId; }
            set { basisId = value; }
        }
        public int PaymentsPerYear
        {
            get { return paymentsPerYear; }
            set { paymentsPerYear = value; }
        }
        public decimal PropertyAppreciation
        {
            get { return propertyAppreciation; }
            set { propertyAppreciation = value; }
        }
        public decimal UpfrontMortgageInsuranceRate
        {
            get { return upfrontMortgageInsuranceRate; }
            set { upfrontMortgageInsuranceRate = value; }
        }
        public decimal RenewalMortgageInsuranceRate
        {
            get { return renewalMortgageInsuranceRate; }
            set { renewalMortgageInsuranceRate = value; }
        }
        public int FirstYearPropertyCharges
        {
            get { return firstYearPropertyCharges; }
            set { firstYearPropertyCharges = value; }
        }
        public bool EndOfMonthFlag
        {
            get { return endOfMonthFlag; }
            set { endOfMonthFlag = value; }
        }
        public decimal RelativeRateCap
        {
            get { return relativeRateCap; }
            set { relativeRateCap = value; }
        }
        public bool SharedAppreciation
        {
            get { return sharedAppreciation; }
            set { sharedAppreciation = value; }
        }
        public int AmortizationTypeId
        {
            get { return amortizationTypeId; }
            set { amortizationTypeId = value; }
        }
        public bool AmortizationTypeArmYN
        {
            get { return amortizationTypeId == (int) AmortizationType.Arm; }
        }
        public bool AmortizationTypeFixedYN
        {
            get { return amortizationTypeId == (int)AmortizationType.Fixed; }
        }
        public bool AmortizationTypeOtherYN
        {
            get { return amortizationTypeId == (int)AmortizationType.Other; }
        }
        public int ArmTypeId
        {
            get { return armTypeId; }
            set { armTypeId = value; }
        }
        public string ArmTypeOther
        {
            get { return armTypeOther;}
            set{ armTypeOther = value;}
        }
        public bool ArmTypeAnnualYN
        {
            get { return armTypeId == (int)ArmType.Annual; }
        }
        public bool ArmTypeMonthlyYN
        {
            get { return armTypeId == (int)ArmType.Monthly; }
        }
        public bool ArmTypeOtherYN
        {
            get{ return armTypeId == (int)ArmType.Other;}
        }
        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }
        public string TypeOther
        {
            get { return typeOther; }
            set { typeOther = value; }
        }
        public bool IsHomekeeperYN
        {
            get{ return typeId == (int) ProductType.Homekeeper;}
        }
        public bool IsHECMYN
        {
            get{ return typeId == (int) ProductType.HECM;}
        }
        public bool IsOtherProductYN
        {
            get{ return typeId == (int) ProductType.Other;}
        }
        public string OtherDescription
        {
            get { return otherDescription; }
            set { otherDescription = value; }
        }
        public bool OtherSpecialLoanFeatureAllowed
        {
            get { return otherSpecialLoanFeatureAllowed; }
            set { otherSpecialLoanFeatureAllowed = value; }
        }
        public string OtherSpecialLoanFeatureDescription
        {
            get { return otherSpecialLoanFeatureDescription; }
            set { otherSpecialLoanFeatureDescription = value; }
        }
        public string AmortizationTypeOther
        {
            get { return amortizationTypeOther; }
            set { amortizationTypeOther = value; }
        }
        public int RateChangeDayId
        {
            get { return rateChangeDayId; }
            set { rateChangeDayId = value; }
        }
        public decimal ConformingLendingLimit
        {
            get { return conformingLendingLimit; }
            set { conformingLendingLimit = value; }
        }
        public int RepairLength
        {
            get { return repairLength; }
            set { repairLength = value; }
        }
        public int MinimumAge
        {
            get { return minimumAge; }
            set { minimumAge = value; }
        }
        public decimal RelativeAdjustmentRateCap
        {
            get { return relativeAdjustmentRateCap; }
            set { relativeAdjustmentRateCap = value; }
        }
        public string SectionOfTheAct
        {
            get { return sectionOfTheAct; }
            set { sectionOfTheAct = value; }
        }
        public string SpecialRiderDescription
        {
            get { return specialRiderDescription; }
            set { specialRiderDescription = value; }
        }
        public bool? SpecialRiderYN
        {
            get { return specialRiderYN; }
            set { specialRiderYN = value; }
        }
        public bool TypeHECMYN
        {
            get { return typeId == (int)ProductType.HECM; }
        }
        public bool TypeOtherYN
        {
            get { return typeId == (int)ProductType.Other; }
        }
        public int TypeSettStmntId
        {
            get { return typeSettStmntId; }
            set { typeSettStmntId = value; }
        }
        public bool TypeSettStmntConvInsYN
        {
            get { return typeSettStmntId==(int)ProductTypeSettStmnt.CONVINS; }
        }
        public bool TypeSettStmntConvUninsYN
        {
            get { return typeSettStmntId == (int)ProductTypeSettStmnt.CONVUNINS; }
        }
        public bool TypeSettStmntFHAYN
        {
            get { return typeSettStmntId == (int)ProductTypeSettStmnt.FHA; }
        }
        public bool TypeSettStmntFmHAYN
        {
            get { return typeSettStmntId == (int)ProductTypeSettStmnt.FmHA; }
        }
        public bool TypeSettStmntVAYN
        {
            get { return typeSettStmntId == (int)ProductTypeSettStmnt.VA; }
        }
        public bool? UseFHAGuidesRepairs
        {
            get { return useFHAGuidesRepairs; }
            set { useFHAGuidesRepairs = value; }
        }
        public bool? UseFHAGuidesTermite
        {
            get { return useFHAGuidesTermite; }
            set { useFHAGuidesTermite = value; }
        }
        public string RepairAdminFormula
        {
            get { return repairAdminFormula; }
            set { repairAdminFormula = value; }
        }
        public DateTime? NextMonthlyRateChange
        {
            get { return nextMonthlyRateChange; }
            set { nextMonthlyRateChange = value; }
        }
        public DateTime? NextAnnualRateChange
        {
            get { return nextAnnualRateChange; }
            set { nextAnnualRateChange = value; }
        }
        public decimal MonthlyMIPRate
        {
            get { return monthlyMIPRate; }
            set { monthlyMIPRate = value; }
        }
        public string InitialRateMarginSpelled
        {
            get { return MarginSpelled; }
        }
        public double InitialRateMargin
        {
            get { return margin; }
        }
        public decimal DailyMIPRate
        {
            get { return dailyMIPRate; }
            set { dailyMIPRate = value; }
        }
        public decimal ConventionalMortgageLoan
        {
            get { return conventionalMortgageLoan; }
            set { conventionalMortgageLoan = value; }
        }

        public bool HECMGuidesCollectingCounsCert
        {
            get { return hecmGuidesCollectingCounsCert; }
            set { hecmGuidesCollectingCounsCert = value; }
        }
        public bool HECMGuidesReviewingCounsCert
        {
            get { return hecmGuidesReviewingCounsCert; }
            set { hecmGuidesReviewingCounsCert = value; }
        }
        public bool HECMGuidesCollectingFHACase
        {
            get { return hecmGuidesCollectingFHACase; }
            set { hecmGuidesCollectingFHACase = value; }
        }
        public bool HECMGuidesReviewingFHACase
        {
            get { return hecmGuidesReviewingFHACase; }
            set { hecmGuidesReviewingFHACase = value; }
        }
        public bool HECMGuidesCollectingFHACondoApproval
        {
            get { return hecmGuidesCollectingFHACondoApproval; }
            set { hecmGuidesCollectingFHACondoApproval = value; }
        }
        public bool HECMGuidesReviewingFHACondoApproval
        {
            get { return hecmGuidesReviewingFHACondoApproval; }
            set { hecmGuidesReviewingFHACondoApproval = value; }
        }
        public bool HECMGuidesCollectingAppraisal
        {
            get { return hecmGuidesCollectingAppraisal; }
            set { hecmGuidesCollectingAppraisal = value; }
        }
        public bool HECMGuidesCollectingTermiteInspection
        {
            get { return hecmGuidesCollectingTermiteInspection; }
            set { hecmGuidesCollectingTermiteInspection = value; }
        }
        public bool HECMGuidesReviewingAppraisal
        {
            get { return hecmGuidesReviewingAppraisal; }
            set { hecmGuidesReviewingAppraisal = value; }
        }
        public bool HECMGuidesReviewingTermiteInspections
        {
            get { return hecmGuidesReviewingTermiteInspections; }
            set { hecmGuidesReviewingTermiteInspections = value; }
        }
        public bool HECMGuidesCollectingContractorBids
        {
            get { return hecmGuidesCollectingContractorBids; }
            set { hecmGuidesCollectingContractorBids = value; }
        }
        public bool HECMGuidesReviewingContractorBids
        {
            get { return hecmGuidesReviewingContractorBids; }
            set { hecmGuidesReviewingContractorBids = value; }
        }
        public bool HECMGuidesCollectingStructuralInspections
        {
            get { return hecmGuidesCollectingStructuralInspections; }
            set { hecmGuidesCollectingStructuralInspections = value; }
        }
        public bool HECMGuidesCollectingWaterTests
        {
            get { return hecmGuidesCollectingWaterTests; }
            set { hecmGuidesCollectingWaterTests = value; }
        }
        public bool HECMGuidesReviewingWaterTests
        {
            get { return hecmGuidesReviewingWaterTests; }
            set { hecmGuidesReviewingWaterTests = value; }
        }
        public bool HECMGuidesCollectingSepticInspections
        {
            get { return hecmGuidesCollectingSepticInspections; }
            set { hecmGuidesCollectingSepticInspections = value; }
        }
        public bool HECMGuidesReviewingSepticInspections
        {
            get { return hecmGuidesReviewingSepticInspections; }
            set { hecmGuidesReviewingSepticInspections = value; }
        }
        public bool HECMGuidesCollectingOilTankInspections
        {
            get { return hecmGuidesCollectingOilTankInspections; }
            set { hecmGuidesCollectingOilTankInspections = value; }
        }
        public bool HECMGuidesReviewingOilTankInspections
        {
            get { return hecmGuidesReviewingOilTankInspections; }
            set { hecmGuidesReviewingOilTankInspections = value; }
        }
        public bool HECMGuidesCollectingRoofInspections
        {
            get { return hecmGuidesCollectingRoofInspections; }
            set { hecmGuidesCollectingRoofInspections = value; }
        }
        public bool HECMGuidesReviewingRoofInspections
        {
            get { return hecmGuidesReviewingRoofInspections; }
            set { hecmGuidesReviewingRoofInspections = value; }
        }
        public bool HECMGuidesCollectingPOAandConservator
        {
            get { return hecmGuidesCollectingPOAandConservator; }
            set { hecmGuidesCollectingPOAandConservator = value; }
        }
        public bool HECMGuidesReviewingPOA
        {
            get { return hecmGuidesReviewingPOA; }
            set { hecmGuidesReviewingPOA = value; }
        }
        public bool HECMGuidesReviewingConservator
        {
            get { return hecmGuidesReviewingConservator; }
            set { hecmGuidesReviewingConservator = value; }
        }
        public bool HECMGuidesCollectingCreditReports
        {
            get { return hecmGuidesCollectingCreditReports; }
            set { hecmGuidesCollectingCreditReports = value; }
        }
        public bool HECMGuidesReviewingCreditReports
        {
            get { return hecmGuidesReviewingCreditReports; }
            set { hecmGuidesReviewingCreditReports = value; }
        }
        public bool HECMGuidesCollectingLDP_GSAPrintout
        {
            get { return hecmGuidesCollectingLDP_GSAPrintout; }
            set { hecmGuidesCollectingLDP_GSAPrintout = value; }
        }
        public bool HECMGuidesReviewingLDP_GSAPrintout
        {
            get { return hecmGuidesReviewingLDP_GSAPrintout; }
            set { hecmGuidesReviewingLDP_GSAPrintout = value; }
        }
        public bool HECMGuidesCollectingCAIVRSAuthPrintout
        {
            get { return hecmGuidesCollectingCAIVRSAuthPrintout; }
            set { hecmGuidesCollectingCAIVRSAuthPrintout = value; }
        }
        public bool HECMGuidesReviewingCAIVRSAuthPrintout
        {
            get { return hecmGuidesReviewingCAIVRSAuthPrintout; }
            set { hecmGuidesReviewingCAIVRSAuthPrintout = value; }
        }
        public bool HECMGuidesCollectingTrusts
        {
            get { return hecmGuidesCollectingTrusts; }
            set { hecmGuidesCollectingTrusts = value; }
        }
        public bool HECMGuidesReviewingTrusts
        {
            get { return hecmGuidesReviewingTrusts; }
            set { hecmGuidesReviewingTrusts = value; }
        }
        public bool BasicGuidesCollectingDeathCerts
        {
            get { return basicGuidesCollectingDeathCerts; }
            set { basicGuidesCollectingDeathCerts = value; }
        }
        public bool BasicGuidesReviewingDeathCerts
        {
            get { return basicGuidesReviewingDeathCerts; }
            set { basicGuidesReviewingDeathCerts = value; }
        }
        public bool BasicGuidesCollectingUSPS
        {
            get { return basicGuidesCollectingUSPS; }
            set { basicGuidesCollectingUSPS = value; }
        }
        public bool BasicGuidesReviewingUSPS
        {
            get { return basicGuidesReviewingUSPS; }
            set { basicGuidesReviewingUSPS = value; }
        }
        public bool BasicGuidesCollectingTitle
        {
            get { return basicGuidesCollectingTitle; }
            set { basicGuidesCollectingTitle = value; }
        }
        public bool BasicGuidesReviewingTitle
        {
            get { return basicGuidesReviewingTitle; }
            set { basicGuidesReviewingTitle = value; }
        }
        public bool BasicGuidesCollectingProofOfAge
        {
            get { return basicGuidesCollectingProofOfAge; }
            set { basicGuidesCollectingProofOfAge = value; }
        }
        public bool BasicGuidesReviewingProofOfAge
        {
            get { return basicGuidesReviewingProofOfAge; }
            set { basicGuidesReviewingProofOfAge = value; }
        }
        public bool BasicGuidesCollectingSSN
        {
            get { return basicGuidesCollectingSSN; }
            set { basicGuidesCollectingSSN = value; }
        }
        public bool BasicGuidesReviewingSSN
        {
            get { return basicGuidesReviewingSSN; }
            set { basicGuidesReviewingSSN = value; }
        }
        public bool BasicGuidesCollectingFloodCertificates
        {
            get { return basicGuidesCollectingFloodCertificates; }
            set { basicGuidesCollectingFloodCertificates = value; }
        }
        public bool BasicGuidesReviewingFloodCertificates
        {
            get { return basicGuidesReviewingFloodCertificates; }
            set { basicGuidesReviewingFloodCertificates = value; }
        }
        public bool BasicGuidesCollectingHazardDecPages
        {
            get { return basicGuidesCollectingHazardDecPages; }
            set { basicGuidesCollectingHazardDecPages = value; }
        }
        public bool BasicGuidesReviewingHazardDecPages
        {
            get { return basicGuidesReviewingHazardDecPages; }
            set { basicGuidesReviewingHazardDecPages = value; }
        }
        public bool BasicGuidesCollectingFloodDecPages
        {
            get { return basicGuidesCollectingFloodDecPages; }
            set { basicGuidesCollectingFloodDecPages = value; }
        }
        public bool BasicGuidesReviewingFloodDecPages
        {
            get { return basicGuidesReviewingFloodDecPages; }
            set { basicGuidesReviewingFloodDecPages = value; }
        }
        #endregion

        #region Constructors
        public Product()
            : this(0)
        { }
        public Product(int id)
        {
            ID = id;
            if (id > 0)
            {
                LoadById();
            }
        }
        public Product(ProductFlag productFlag) : this((int)productFlag)
        { }
        #endregion

        #region methods
        
        #region public
        public int Save(int templateId)
        {
            int res = 
                db.ExecuteScalarInt(SAVE, ID, templateId, Name, Margin, RateRoundingPrecision, RateRoundingMethodId
                ,BasisId, PaymentsPerYear, PropertyAppreciation, UpfrontMortgageInsuranceRate
                ,RenewalMortgageInsuranceRate, FirstYearPropertyCharges
                ,EndOfMonthFlag, RelativeRateCap
                ,SharedAppreciation, amortizationTypeId,amortizationTypeOther,armTypeId,armTypeOther
                ,typeId, typeOther,otherDescription,otherSpecialLoanFeatureAllowed
                ,OtherSpecialLoanFeatureDescription,rateChangeDayId,ConformingLendingLimit
                ,primaryResidenceRequired, allowPrimaryRes, allowSecondHome, allowInvestmentProp
                ,appraisalRequired, propInspRequired,useStandardFloodGuides,maxFloodDeductible
                ,useStandardHazardGuides,maxHazDeductPercent,repairLength,allowHECMRefi,allowMultiFamilyProp
                ,useStandardGuidesWellSeptic,useStndGuidesCashToClose,daysAdvancePayTax
                ,allowManuHomes,useStandardTrustGuides, creditReportRequiredYN, minimumAge
                ,relativeAdjustmentRateCap, sectionOfTheAct, specialRiderDescription, specialRiderYN
                ,typeSettStmntId, useFHAGuidesRepairs, useFHAGuidesTermite
                ,conventionalMortgageLoan,productIndexId
                ,basicGuidesCollectingDeathCerts,basicGuidesReviewingDeathCerts
                ,basicGuidesCollectingUSPS,basicGuidesReviewingUSPS
                ,basicGuidesCollectingTitle,basicGuidesReviewingTitle
                ,basicGuidesCollectingProofOfAge,basicGuidesReviewingProofOfAge
                ,basicGuidesCollectingSSN,basicGuidesReviewingSSN
                ,basicGuidesCollectingFloodCertificates,basicGuidesReviewingFloodCertificates
                ,basicGuidesCollectingHazardDecPages,basicGuidesReviewingHazardDecPages
                ,basicGuidesCollectingFloodDecPages,basicGuidesReviewingFloodDecPages
                ,hecmGuidesCollectingCounsCert,hecmGuidesReviewingCounsCert
                ,hecmGuidesCollectingFHACase,hecmGuidesReviewingFHACase
                ,hecmGuidesCollectingFHACondoApproval,hecmGuidesReviewingFHACondoApproval
                ,hecmGuidesCollectingAppraisal,hecmGuidesCollectingTermiteInspection
                ,hecmGuidesReviewingAppraisal,hecmGuidesReviewingTermiteInspections
                ,hecmGuidesCollectingContractorBids,hecmGuidesReviewingContractorBids
                ,hecmGuidesCollectingStructuralInspections,hecmGuidesCollectingWaterTests
                ,hecmGuidesReviewingWaterTests,hecmGuidesCollectingSepticInspections
                ,hecmGuidesReviewingSepticInspections,hecmGuidesCollectingOilTankInspections
                ,hecmGuidesReviewingOilTankInspections,hecmGuidesCollectingRoofInspections
                ,hecmGuidesReviewingRoofInspections,hecmGuidesCollectingPOAandConservator
                ,hecmGuidesReviewingPOA,hecmGuidesReviewingConservator
                ,hecmGuidesCollectingCreditReports,hecmGuidesReviewingCreditReports
                ,hecmGuidesCollectingLDP_GSAPrintout,hecmGuidesReviewingLDP_GSAPrintout
                ,hecmGuidesCollectingCAIVRSAuthPrintout,hecmGuidesReviewingCAIVRSAuthPrintout
                ,hecmGuidesCollectingTrusts,hecmGuidesReviewingTrusts
                ,baydocsAppPackageCodeId
                ,baydocsClosingPackageCodeId
                ,equityProtection
                ,daysToLock
                ,expectedFloorRate
                ,counsActiveDays
                ,titleActiveDays
                ,appraisalActiveDays
                ,pestActiveDays
                ,bidActiveDays
                ,waterTestActiveDays
                ,septicInspActiveDays
                ,oilTankInspActiveDays
                ,roofInspActiveDays
                ,floodCertActiveDays
                ,creditReportActiveDays
                ,ldpActiveDays
                ,eplsactiveDays
                ,caivrsActiveDays
                ,followStandardNBSGuides
                ,hecmGuidesCollectingLeases
                ,ageEligRequirementApply
                ,ageEligRequirementClose
                ,minAgeToApply
                ,minAgeToClose
                ,basicGuidesCollectingHOAHazardDecPages
                ,basicGuidesReviewingHOAHazardDecPages
                ,basicGuidesCollectingMasterFloodDecPages
                ,useSRPLocksYN
                ,hecmGuidesReviewingLeases
                ,hecmGuidesCollectingConservator
                ,basicGuidesReviewingHOAFloodDecPages
                ,hecmGuidesReviewingStructuralInspections
                ,lendingLimit
                ,hecmGuidesCollectingPOA
                ,allowEscrowTaxAndInsurance
                ,allowEscrowRepiars
                ,fixedRateLockDays
                ,productRateLockMethodId
                ,productRateUpdateIntervalId
                ,productRateInputMethodId
                ,useBaydocsAppPackagesYN
                );
            if((ID<=0)&&(res>0))
            {
                ID = res;
            }
            return res;
        }
        public bool Delete()
        {
            return db.ExecuteScalarInt(DELETE, ID)==1;
        }
        public DataView GetProductsListToCopyRates()
        {
            return db.GetDataView(GETPRODUCTLISTTOCOPYRATES, ID);
        }

        public ProductRate GetNearestProductRate(DateTime period)
        {
            ProductRate rate = null;
            try
            {
                DataRow row = db.GetSingleRow("GetNearestProductRates", period, ID);
                rate = new ProductRate(row);
                return rate;
            }
            catch (DatabaseAccess.DatabaseAccessException)
            {
                return rate;
            }
        }
        public Double GetCompanyServiceFee(int lenderId)
        {
            return Convert.ToDouble(db.ExecuteScalarString("GetDefaultFeeForProduct",ID,lenderId));
        }
        public int SaveRates(int id, DateTime rateDate, double publishedIndex, double publishedMargin, double publishedExpectedIndex, double publishedExpectedMargin)
        {
            return db.ExecuteScalarInt(SAVERATES,id, ID, rateDate, publishedIndex, publishedMargin, publishedExpectedIndex, publishedExpectedMargin);
        }
        public int CopyRates(DateTime from, DateTime to, string productList)
        {
            return db.ExecuteScalarInt(COPYRATES, ID, from, to, productList);
        }

        #endregion

        #region private
        private void LoadById()
        {
            DataView dv = db.GetDataView(GETPRODUCTBYID, ID);
            if (dv.Count == 1)
            {
                if (dv[0][ALLOWHECMREFIFIELDNAME] != DBNull.Value)
                {
                    allowHECMRefi = Convert.ToBoolean(dv[0][ALLOWHECMREFIFIELDNAME]);
                }
                if (dv[0][ALLOWINVESTMENTPROPFIELDNAME] != DBNull.Value)
                {
                    allowInvestmentProp = Convert.ToBoolean(dv[0][ALLOWINVESTMENTPROPFIELDNAME]);
                }
                if (dv[0][ALLOWMULTIFAMILYPROPFIELDNAME] != DBNull.Value)
                {
                    allowMultiFamilyProp = Convert.ToBoolean(dv[0][ALLOWMULTIFAMILYPROPFIELDNAME]);
                }
                if (dv[0][ALLOWPRIMARYRESFIELDNAME] != DBNull.Value)
                {
                    allowPrimaryRes = Convert.ToBoolean(dv[0][ALLOWPRIMARYRESFIELDNAME]);
                }
                if (dv[0][ALLOWSECONDHOMEFIELDNAME] != DBNull.Value)
                {
                    allowSecondHome = Convert.ToBoolean(dv[0][ALLOWSECONDHOMEFIELDNAME]);
                }
                if (dv[0][ALLOWMANUHOMESFIELDNAME] != DBNull.Value)
                {
                    allowManuHomes = Convert.ToBoolean(dv[0][ALLOWMANUHOMESFIELDNAME]);
                }
                amortizationTypeId = int.Parse(dv[0][AMORTIZATIONTYPEIDFIELDNAME].ToString());
                amortizationTypeOther = dv[0][AMORTIZATIONTYPEOTHERFIELDNAME].ToString();
                armTypeId = int.Parse(dv[0][ARMTYPEIDFIELDNAME].ToString());
                armTypeOther = dv[0][ARMTYPEOTHERFIELDNAME].ToString();
                if (dv[0][APPRAISALREQUIREDFIELDNAME] != DBNull.Value)
                {
                    appraisalRequired = Convert.ToBoolean(dv[0][APPRAISALREQUIREDFIELDNAME]);
                }
                basisId = Convert.ToInt16(dv[0][BASISIDFIELDNAME]);
                if (dv[0][CREDITREPORTREQUIREDYNFIELDNAME] != DBNull.Value)
                {
                    creditReportRequiredYN = Convert.ToBoolean(dv[0][CREDITREPORTREQUIREDYNFIELDNAME]);
                }
                if (dv[0][CONFORMINGLENDINGLIMITFIELDNAME] != DBNull.Value)
                {
                    conformingLendingLimit = Convert.ToDecimal(dv[0][CONFORMINGLENDINGLIMITFIELDNAME]);
                }
                if (dv[0][DAYSADVANCEPAYTAXFIELDNAME] != DBNull.Value)
                {
                    daysAdvancePayTax = Convert.ToInt16(dv[0][DAYSADVANCEPAYTAXFIELDNAME]);
                }
                endOfMonthFlag = Convert.ToBoolean(dv[0][ENDOFMONTHFLAGFIELDNAME]);
                firstYearPropertyCharges = Convert.ToInt16(dv[0][FIRSTYEARPROPERTYCHARGESFIELDNAME]);
                margin = Convert.ToDouble(dv[0][MARGINFIELDNAME]);
                if (dv[0][MAXFLOODDEDUCTIBLEFIELDNAME] != DBNull.Value)
                {
                    maxFloodDeductible = Convert.ToDecimal(dv[0][MAXFLOODDEDUCTIBLEFIELDNAME]);
                }
                if (dv[0][MAXHAZDEDUCTPERCENTFIELDNAME] != DBNull.Value)
                {
                    maxHazDeductPercent = Convert.ToDecimal(dv[0][MAXHAZDEDUCTPERCENTFIELDNAME]);
                }
                
                if (dv[0][MINIMUMAGEFIELDNAME] != DBNull.Value)
                {
                    minimumAge = Convert.ToInt16(dv[0][MINIMUMAGEFIELDNAME]);
                }
                name = dv[0][NAMEFIELDNAME].ToString();
                otherSpecialLoanFeatureDescription = dv[0][SPECIALFETUREDESCRIPTIONFIELDNAME].ToString();
                otherDescription = dv[0][OTHERDESCRIPTIONFIELDNAME].ToString();
                if (dv[0][SPECIALLOANFEATUREFIELDNAME] != DBNull.Value)
                {
                    otherSpecialLoanFeatureAllowed = Convert.ToBoolean(dv[0][SPECIALLOANFEATUREFIELDNAME]);
                }
                paymentsPerYear = Convert.ToInt16(dv[0][PAYMENTSPERYEARFIELDNAME]);
                if (dv[0][PRIMARYRESIDENSEREQUIREDFIELDNAME] != DBNull.Value)
                {
                    primaryResidenceRequired = Convert.ToBoolean(dv[0][PRIMARYRESIDENSEREQUIREDFIELDNAME]);
                }
                propertyAppreciation = Convert.ToDecimal(dv[0][PROPERTYAPPRECIATIONFIELDNAME]);
                if (dv[0][PROPINSPREQUIREDFIELDNAME] != DBNull.Value)
                {
                    propInspRequired = Convert.ToBoolean(dv[0][PROPINSPREQUIREDFIELDNAME]);
                }
                rateChangeDayId = int.Parse(dv[0][RATECHANGEDAYIDFIELDNAME].ToString());
                rateRoundingPrecision = Convert.ToDecimal(dv[0][RATEROUNDINGPRECISIONFIELDNAME]);
                rateRoundingMethodId = Convert.ToInt16(dv[0][RATEROUNDINGMETHODIDFIELDNAME]);
                if (dv[0][RELATIVEADJUSTMENTRATECAPFIELDNAME] != DBNull.Value)
                {
                    relativeAdjustmentRateCap = Convert.ToDecimal(dv[0][RELATIVEADJUSTMENTRATECAPFIELDNAME]);
                }
                relativeRateCap = Convert.ToDecimal(dv[0][RELATIVERATECAPFIELDNAME]);
                renewalMortgageInsuranceRate = Convert.ToDecimal(dv[0][RENEWALMORTGAGEINSURANCERATEFIELDNAME]);
                if (dv[0][REPAIRLENGTHFIELDNAME] != DBNull.Value)
                {
                    repairLength = Convert.ToInt16(dv[0][REPAIRLENGTHFIELDNAME]);
                }
                sectionOfTheAct = dv[0][SECTIONOFTHEACTFIELDNAME].ToString();
                sharedAppreciation = Convert.ToBoolean(dv[0][SHAREDAPPRECIATIONFIELDNAME]);
                specialRiderDescription = dv[0][SPECIALRIDERDESCRIPTIONFIELDNAME].ToString();
                if (dv[0][SPECIALRIDERYNFIELDNAME] != DBNull.Value)
                {
                    specialRiderYN = Convert.ToBoolean(dv[0][SPECIALRIDERYNFIELDNAME]);
                }
                typeId = int.Parse(dv[0][TYPEIDFIELDNAME].ToString());
                typeOther = dv[0][TYPEOTHERFIELDNAME].ToString();
                if (dv[0][TYPESETTSTMNTIDFIELDNAME] != DBNull.Value)
                {
                    typeSettStmntId = Convert.ToInt16(dv[0][TYPESETTSTMNTIDFIELDNAME]);
                }
                upfrontMortgageInsuranceRate = Convert.ToDecimal(dv[0][UPFRONTMORTGAGEINSURANCERATEFIELDNAME]);
                if (dv[0][USEFHAGUIDESREPAIRSFIELDNAME] != DBNull.Value)
                {
                    useFHAGuidesRepairs = Convert.ToBoolean(dv[0][USEFHAGUIDESREPAIRSFIELDNAME]);
                }
                if (dv[0][USEFHAGUIDESTERMITEFIELDNAME] != DBNull.Value)
                {
                    useFHAGuidesTermite = Convert.ToBoolean(dv[0][USEFHAGUIDESTERMITEFIELDNAME]);
                }
                if (dv[0][USESTANDARDFLOODGUIDESFIELDNAME] != DBNull.Value)
                {
                    useStandardFloodGuides = Convert.ToBoolean(dv[0][USESTANDARDFLOODGUIDESFIELDNAME]);
                }
                if (dv[0][USESTANDARDHAZARDGUIDESFIELDNAME] != DBNull.Value)
                {
                    useStandardHazardGuides = Convert.ToBoolean(dv[0][USESTANDARDHAZARDGUIDESFIELDNAME]);
                }
                if (dv[0][USESTANDARDGUIDEWELLSEPTICFIELDNAME] != DBNull.Value)
                {
                    useStandardGuidesWellSeptic = Convert.ToBoolean(dv[0][USESTANDARDGUIDEWELLSEPTICFIELDNAME]);
                }
                if (dv[0][USESTNDGUIDESCASHTOCLOSEFIELDNAME] != DBNull.Value)
                {
                    useStndGuidesCashToClose = Convert.ToBoolean(dv[0][USESTNDGUIDESCASHTOCLOSEFIELDNAME]);
                }
                if (dv[0][USESTANDARDTRUSTGUIDEFIELDNAME] != DBNull.Value)
                {
                    useStandardTrustGuides = Convert.ToBoolean(dv[0][USESTANDARDTRUSTGUIDEFIELDNAME]);
                }
                conventionalMortgageLoan = decimal.Parse(dv[0][CONVENTIONALMORTGAGELOANVALUEFIELDNAME].ToString());
                productIndexId = int.Parse(dv[0][PRODUCTINDEXIDFIELDNAME].ToString());
                if (dv[0]["hecmGuidesCollectingCounsCert"] != DBNull.Value)
                {
                    hecmGuidesCollectingCounsCert = Convert.ToBoolean(dv[0]["hecmGuidesCollectingCounsCert"]);
                }
                if (dv[0]["hecmGuidesReviewingCounsCert"] != DBNull.Value)
                {
                    hecmGuidesReviewingCounsCert = Convert.ToBoolean(dv[0]["hecmGuidesReviewingCounsCert"]);
                }
                if (dv[0]["hecmGuidesCollectingFHACase"] != DBNull.Value)
                {
                    hecmGuidesCollectingFHACase = Convert.ToBoolean(dv[0]["hecmGuidesCollectingFHACase"]);
                }
                if (dv[0]["hecmGuidesReviewingFHACase"] != DBNull.Value)
                {
                    hecmGuidesReviewingFHACase = Convert.ToBoolean(dv[0]["hecmGuidesReviewingFHACase"]);
                }
                if (dv[0]["hecmGuidesCollectingFHACondoApproval"] != DBNull.Value)
                {
                    hecmGuidesCollectingFHACondoApproval = Convert.ToBoolean(dv[0]["hecmGuidesCollectingFHACondoApproval"]);
                }
                if (dv[0]["hecmGuidesReviewingFHACondoApproval"] != DBNull.Value)
                {
                    hecmGuidesReviewingFHACondoApproval = Convert.ToBoolean(dv[0]["hecmGuidesReviewingFHACondoApproval"]);
                }
                if (dv[0]["hecmGuidesCollectingAppraisal"] != DBNull.Value)
                {
                    hecmGuidesCollectingAppraisal = Convert.ToBoolean(dv[0]["hecmGuidesCollectingAppraisal"]);
                }
                if (dv[0]["hecmGuidesCollectingTermiteInspection"] != DBNull.Value)
                {
                    hecmGuidesCollectingTermiteInspection = Convert.ToBoolean(dv[0]["hecmGuidesCollectingTermiteInspection"]);
                }
                if (dv[0]["hecmGuidesReviewingAppraisal"] != DBNull.Value)
                {
                    hecmGuidesReviewingAppraisal = Convert.ToBoolean(dv[0]["hecmGuidesReviewingAppraisal"]);
                }
                if (dv[0]["hecmGuidesReviewingTermiteInspections"] != DBNull.Value)
                {
                    hecmGuidesReviewingTermiteInspections = Convert.ToBoolean(dv[0]["hecmGuidesReviewingTermiteInspections"]);
                }
                if (dv[0]["hecmGuidesCollectingContractorBids"] != DBNull.Value)
                {
                    hecmGuidesCollectingContractorBids = Convert.ToBoolean(dv[0]["hecmGuidesCollectingContractorBids"]);
                }
                if (dv[0]["hecmGuidesReviewingContractorBids"] != DBNull.Value)
                {
                    hecmGuidesReviewingContractorBids = Convert.ToBoolean(dv[0]["hecmGuidesReviewingContractorBids"]);
                }
                if (dv[0]["hecmGuidesCollectingStructuralInspections"] != DBNull.Value)
                {
                    hecmGuidesCollectingStructuralInspections = Convert.ToBoolean(dv[0]["hecmGuidesCollectingStructuralInspections"]);
                }
                if (dv[0]["hecmGuidesCollectingWaterTests"] != DBNull.Value)
                {
                    hecmGuidesCollectingWaterTests = Convert.ToBoolean(dv[0]["hecmGuidesCollectingWaterTests"]);
                }
                if (dv[0]["hecmGuidesReviewingWaterTests"] != DBNull.Value)
                {
                    hecmGuidesReviewingWaterTests = Convert.ToBoolean(dv[0]["hecmGuidesReviewingWaterTests"]);
                }
                if (dv[0]["hecmGuidesCollectingSepticInspections"] != DBNull.Value)
                {
                    hecmGuidesCollectingSepticInspections = Convert.ToBoolean(dv[0]["hecmGuidesCollectingSepticInspections"]);
                }
                if (dv[0]["hecmGuidesReviewingSepticInspections"] != DBNull.Value)
                {
                    hecmGuidesReviewingSepticInspections = Convert.ToBoolean(dv[0]["hecmGuidesReviewingSepticInspections"]);
                }
                if (dv[0]["hecmGuidesCollectingOilTankInspections"] != DBNull.Value)
                {
                    hecmGuidesCollectingOilTankInspections = Convert.ToBoolean(dv[0]["hecmGuidesCollectingOilTankInspections"]);
                }
                if (dv[0]["hecmGuidesReviewingOilTankInspections"] != DBNull.Value)
                {
                    hecmGuidesReviewingOilTankInspections = Convert.ToBoolean(dv[0]["hecmGuidesReviewingOilTankInspections"]);
                }
                if (dv[0]["hecmGuidesCollectingRoofInspections"] != DBNull.Value)
                {
                    hecmGuidesCollectingRoofInspections = Convert.ToBoolean(dv[0]["hecmGuidesCollectingRoofInspections"]);
                }
                if (dv[0]["hecmGuidesReviewingRoofInspections"] != DBNull.Value)
                {
                    hecmGuidesReviewingRoofInspections = Convert.ToBoolean(dv[0]["hecmGuidesReviewingRoofInspections"]);
                }
                if (dv[0]["hecmGuidesCollectingPOAandConservator"] != DBNull.Value)
                {
                    hecmGuidesCollectingPOAandConservator = Convert.ToBoolean(dv[0]["hecmGuidesCollectingPOAandConservator"]);
                }
                if (dv[0]["hecmGuidesReviewingPOA"] != DBNull.Value)
                {
                    hecmGuidesReviewingPOA = Convert.ToBoolean(dv[0]["hecmGuidesReviewingPOA"]);
                }
                if (dv[0]["hecmGuidesReviewingConservator"] != DBNull.Value)
                {
                    hecmGuidesReviewingConservator = Convert.ToBoolean(dv[0]["hecmGuidesReviewingConservator"]);
                }
                if (dv[0]["hecmGuidesCollectingCreditReports"] != DBNull.Value)
                {
                    hecmGuidesCollectingCreditReports = Convert.ToBoolean(dv[0]["hecmGuidesCollectingCreditReports"]);
                }
                if (dv[0]["hecmGuidesReviewingCreditReports"] != DBNull.Value)
                {
                    hecmGuidesReviewingCreditReports = Convert.ToBoolean(dv[0]["hecmGuidesReviewingCreditReports"]);
                }
                if (dv[0]["hecmGuidesCollectingLDP_GSAPrintout"] != DBNull.Value)
                {
                    hecmGuidesCollectingLDP_GSAPrintout = Convert.ToBoolean(dv[0]["hecmGuidesCollectingLDP_GSAPrintout"]);
                }
                if (dv[0]["hecmGuidesReviewingLDP_GSAPrintout"] != DBNull.Value)
                {
                    hecmGuidesReviewingLDP_GSAPrintout = Convert.ToBoolean(dv[0]["hecmGuidesReviewingLDP_GSAPrintout"]);
                }
                if (dv[0]["hecmGuidesCollectingCAIVRSAuthPrintout"] != DBNull.Value)
                {
                    hecmGuidesCollectingCAIVRSAuthPrintout = Convert.ToBoolean(dv[0]["hecmGuidesCollectingCAIVRSAuthPrintout"]);
                }
                if (dv[0]["hecmGuidesReviewingCAIVRSAuthPrintout"] != DBNull.Value)
                {
                    hecmGuidesReviewingCAIVRSAuthPrintout = Convert.ToBoolean(dv[0]["hecmGuidesReviewingCAIVRSAuthPrintout"]);
                }
                if (dv[0]["hecmGuidesCollectingTrusts"] != DBNull.Value)
                {
                    hecmGuidesCollectingTrusts = Convert.ToBoolean(dv[0]["hecmGuidesCollectingTrusts"]);
                }
                if (dv[0]["hecmGuidesReviewingTrusts"] != DBNull.Value)
                {
                    hecmGuidesReviewingTrusts = Convert.ToBoolean(dv[0]["hecmGuidesReviewingTrusts"]);
                }
                if (dv[0]["basicGuidesCollectingDeathCerts"] != DBNull.Value)
                {
                    basicGuidesCollectingDeathCerts = Convert.ToBoolean(dv[0]["basicGuidesCollectingDeathCerts"]);
                }
                if (dv[0]["basicGuidesReviewingDeathCerts"] != DBNull.Value)
                {
                    basicGuidesReviewingDeathCerts = Convert.ToBoolean(dv[0]["basicGuidesReviewingDeathCerts"]);
                }
                if (dv[0]["basicGuidesCollectingUSPS"] != DBNull.Value)
                {
                    basicGuidesCollectingUSPS = Convert.ToBoolean(dv[0]["basicGuidesCollectingUSPS"]);
                }
                if (dv[0]["basicGuidesReviewingUSPS"] != DBNull.Value)
                {
                    basicGuidesReviewingUSPS = Convert.ToBoolean(dv[0]["basicGuidesReviewingUSPS"]);
                }
                if (dv[0]["basicGuidesCollectingTitle"] != DBNull.Value)
                {
                    basicGuidesCollectingTitle = Convert.ToBoolean(dv[0]["basicGuidesCollectingTitle"]);
                }
                if (dv[0]["basicGuidesReviewingTitle"] != DBNull.Value)
                {
                    basicGuidesReviewingTitle = Convert.ToBoolean(dv[0]["basicGuidesReviewingTitle"]);
                }
                if (dv[0]["basicGuidesCollectingProofOfAge"] != DBNull.Value)
                {
                    basicGuidesCollectingProofOfAge = Convert.ToBoolean(dv[0]["basicGuidesCollectingProofOfAge"]);
                }
                if (dv[0]["basicGuidesReviewingProofOfAge"] != DBNull.Value)
                {
                    basicGuidesReviewingProofOfAge = Convert.ToBoolean(dv[0]["basicGuidesReviewingProofOfAge"]);
                }
                if (dv[0]["basicGuidesCollectingSSN"] != DBNull.Value)
                {
                    basicGuidesCollectingSSN = Convert.ToBoolean(dv[0]["basicGuidesCollectingSSN"]);
                }
                if (dv[0]["basicGuidesReviewingSSN"] != DBNull.Value)
                {
                    basicGuidesReviewingSSN = Convert.ToBoolean(dv[0]["basicGuidesReviewingSSN"]);
                }
                if (dv[0]["basicGuidesCollectingFloodCertificates"] != DBNull.Value)
                {
                    basicGuidesCollectingFloodCertificates = Convert.ToBoolean(dv[0]["basicGuidesCollectingFloodCertificates"]);
                }
                if (dv[0]["basicGuidesReviewingFloodCertificates"] != DBNull.Value)
                {
                    basicGuidesReviewingFloodCertificates = Convert.ToBoolean(dv[0]["basicGuidesReviewingFloodCertificates"]);
                }
                if (dv[0]["basicGuidesCollectingHazardDecPages"] != DBNull.Value)
                {
                    basicGuidesCollectingHazardDecPages = Convert.ToBoolean(dv[0]["basicGuidesCollectingHazardDecPages"]);
                }
                if (dv[0]["basicGuidesReviewingHazardDecPages"] != DBNull.Value)
                {
                    basicGuidesReviewingHazardDecPages = Convert.ToBoolean(dv[0]["basicGuidesReviewingHazardDecPages"]);
                }
                if (dv[0]["basicGuidesCollectingFloodDecPages"] != DBNull.Value)
                {
                    basicGuidesCollectingFloodDecPages = Convert.ToBoolean(dv[0]["basicGuidesCollectingFloodDecPages"]);
                }
                if (dv[0]["basicGuidesReviewingFloodDecPages"] != DBNull.Value)
                {
                    basicGuidesReviewingFloodDecPages = Convert.ToBoolean(dv[0]["basicGuidesReviewingFloodDecPages"]);
                }
                baydocsAppPackageCodeId = Convert.ToInt32(dv[0]["baydocsLoanTypeID"]);
                baydocsClosingPackageCodeId = Convert.ToInt32(dv[0]["BaydocsClosingPackageCodeId"]);
                if (dv[0]["equityProtection"] != DBNull.Value)
                {
                    equityProtection = Convert.ToBoolean(dv[0]["equityProtection"]);
                }
                //if (dv[0]["yieldSpreadRate"] != DBNull.Value)
                //{
                //    yieldSpreadRate = Convert.ToDouble(dv[0]["yieldSpreadRate"]);
                //}
                //if (dv[0]["PrincipleLimitProtectionYN"] != DBNull.Value)
                //{
                //    principleLimitProtectionYN = Convert.ToBoolean(dv[0]["PrincipleLimitProtectionYN"]);
                //}
                if (dv[0]["DaysToLock"] != DBNull.Value)
                {
                    daysToLock = Convert.ToInt32(dv[0]["DaysToLock"]);
                }
                if (dv[0]["expectedFloorRate"] != DBNull.Value)
                {
                    expectedFloorRate = Convert.ToDouble(dv[0]["expectedFloorRate"]);
                }
                if (dv[0]["counsActiveDays"] != DBNull.Value)
                {
                    counsActiveDays = Convert.ToInt32(dv[0]["counsActiveDays"]);
                }
                if (dv[0]["titleActiveDays"] != DBNull.Value)
                {
                    titleActiveDays = Convert.ToInt32(dv[0]["titleActiveDays"]);
                }
                if (dv[0]["appraisalActiveDays"] != DBNull.Value)
                {
                    appraisalActiveDays = Convert.ToInt32(dv[0]["appraisalActiveDays"]);
                }
                if (dv[0]["pestActiveDays"] != DBNull.Value)
                {
                    pestActiveDays = Convert.ToInt32(dv[0]["pestActiveDays"]);
                }
                if (dv[0]["bidActiveDays"] != DBNull.Value)
                {
                    bidActiveDays = Convert.ToInt32(dv[0]["bidActiveDays"]);
                }
                if (dv[0]["waterTestActiveDays"] != DBNull.Value)
                {
                    waterTestActiveDays = Convert.ToInt32(dv[0]["waterTestActiveDays"]);
                }
                if (dv[0]["septicInspActiveDays"] != DBNull.Value)
                {
                    septicInspActiveDays = Convert.ToInt32(dv[0]["septicInspActiveDays"]);
                }
                if (dv[0]["oilTankInspActiveDays"] != DBNull.Value)
                {
                    oilTankInspActiveDays = Convert.ToInt32(dv[0]["oilTankInspActiveDays"]);
                }
                if (dv[0]["roofInspActiveDays"] != DBNull.Value)
                {
                    roofInspActiveDays = Convert.ToInt32(dv[0]["roofInspActiveDays"]);
                }
                if (dv[0]["floodCertActiveDays"] != DBNull.Value)
                {
                    floodCertActiveDays = Convert.ToInt32(dv[0]["floodCertActiveDays"]);
                }
                if (dv[0]["creditReportActiveDays"] != DBNull.Value)
                {
                    creditReportActiveDays = Convert.ToInt32(dv[0]["creditReportActiveDays"]);
                }
                if (dv[0]["ldpActiveDays"] != DBNull.Value)
                {
                    ldpActiveDays = Convert.ToInt32(dv[0]["ldpActiveDays"]);
                }
                if (dv[0]["eplsactiveDays"] != DBNull.Value)
                {
                    eplsactiveDays = Convert.ToInt32(dv[0]["eplsactiveDays"]);
                }
                if (dv[0]["caivrsActiveDays"] != DBNull.Value)
                {
                    caivrsActiveDays = Convert.ToInt32(dv[0]["caivrsActiveDays"]);
                }
                if (dv[0]["minAgeToApply"] != DBNull.Value)
                {
                    minAgeToApply = Convert.ToInt32(dv[0]["minAgeToApply"]);
                }
                if (dv[0]["minAgeToClose"] != DBNull.Value)
                {
                    minAgeToClose = Convert.ToInt32(dv[0]["minAgeToClose"]);
                }
                if (dv[0]["followStandardNBSGuides"] != DBNull.Value)
                {
                    followStandardNBSGuides = Convert.ToBoolean(dv[0]["followStandardNBSGuides"]);
                }
                if (dv[0]["hecmGuidesCollectingLeases"] != DBNull.Value)
                {
                    hecmGuidesCollectingLeases = Convert.ToBoolean(dv[0]["hecmGuidesCollectingLeases"]);
                }
                if (dv[0]["ageEligRequirementApply"] != DBNull.Value)
                {
                    ageEligRequirementApply = Convert.ToBoolean(dv[0]["ageEligRequirementApply"]);
                }
                if (dv[0]["ageEligRequirementClose"] != DBNull.Value)
                {
                    ageEligRequirementClose = Convert.ToBoolean(dv[0]["ageEligRequirementClose"]);
                }
                if (dv[0]["basicGuidesCollectingHOAHazardDecPages"] != DBNull.Value)
                {
                    basicGuidesCollectingHOAHazardDecPages = Convert.ToBoolean(dv[0]["basicGuidesCollectingHOAHazardDecPages"]);
                }
                if (dv[0]["basicGuidesReviewingHOAHazardDecPages"] != DBNull.Value)
                {
                    basicGuidesReviewingHOAHazardDecPages = Convert.ToBoolean(dv[0]["basicGuidesReviewingHOAHazardDecPages"]);
                }
                if (dv[0]["basicGuidesCollectingMasterFloodDecPages"] != DBNull.Value)
                {
                    basicGuidesCollectingMasterFloodDecPages = Convert.ToBoolean(dv[0]["basicGuidesCollectingMasterFloodDecPages"]);
                }
                if (dv[0]["useSRPLocksYN"] != DBNull.Value)
                {
                    useSRPLocksYN = Convert.ToBoolean(dv[0]["useSRPLocksYN"]);
                }
                if (dv[0]["hecmGuidesReviewingLeases"] != DBNull.Value)
                {
                    hecmGuidesReviewingLeases = Convert.ToBoolean(dv[0]["hecmGuidesReviewingLeases"]);
                }
                if (dv[0]["hecmGuidesCollectingConservator"] != DBNull.Value)
                {
                    hecmGuidesCollectingConservator = Convert.ToBoolean(dv[0]["hecmGuidesCollectingConservator"]);
                }
                if (dv[0]["basicGuidesReviewingHOAFloodDecPages"] != DBNull.Value)
                {
                    basicGuidesReviewingHOAFloodDecPages = Convert.ToBoolean(dv[0]["basicGuidesReviewingHOAFloodDecPages"]);
                }
                if (dv[0]["hecmGuidesReviewingStructuralInspections"] != DBNull.Value)
                {
                    hecmGuidesReviewingStructuralInspections = Convert.ToBoolean(dv[0]["hecmGuidesReviewingStructuralInspections"]);
                }
                if (dv[0]["lendingLimit"] != DBNull.Value)
                {
                    lendingLimit = Convert.ToDecimal(dv[0]["lendingLimit"]);
                }
                if (dv[0]["hecmGuidesCollectingPOA"] != DBNull.Value)
                {
                    hecmGuidesCollectingPOA = Convert.ToBoolean(dv[0]["hecmGuidesCollectingPOA"]);
                }
                if (dv[0]["allowEscrowTaxAndInsurance"] != DBNull.Value)
                {
                    allowEscrowTaxAndInsurance = Convert.ToBoolean(dv[0]["allowEscrowTaxAndInsurance"]);
                }
                if (dv[0]["allowEscrowRepiars"] != DBNull.Value)
                {
                    allowEscrowRepiars = Convert.ToBoolean(dv[0]["allowEscrowRepiars"]);
                }
                if (dv[0]["useBaydocsAppPackagesYN"] != DBNull.Value)
                {
                    useBaydocsAppPackagesYN = Convert.ToBoolean(dv[0]["useBaydocsAppPackagesYN"]);
                }
                //if (dv[0]["fixedRateLocks"] != DBNull.Value)
                //{
                //    fixedRateLocks = Convert.ToBoolean(dv[0]["fixedRateLocks"]);
                //}
                if (dv[0]["fixedRateLockDays"] != DBNull.Value)
                {
                    fixedRateLockDays = Convert.ToInt32(dv[0]["fixedRateLockDays"]);
                }
                productRateLockMethodId = Convert.ToInt32(dv[0]["productRateLockMethodId"]);
                productRateUpdateIntervalId = Convert.ToInt32(dv[0]["productRateUpdateIntervalId"]);
                productRateInputMethodId = Convert.ToInt32(dv[0]["productRateInputMethodId"]);
                 
                calculationType = ProductFlag.HECM_Monthly;
                //code below is temporary...
                //switch ((int)dv[0][IDFIELDNAME])
                //{
                //    case 1:
                //        calculationType = ProductFlag.HECM_Monthly;
                //        break;
                //    case 2:
                //        calculationType = ProductFlag.HECM_Annual;
                //        break;
                //    case 4:
                //        calculationType = ProductFlag.FNMA;
                //        break;
                //    default:
                //        calculationType = ProductFlag.None;
                //        break;
                //}
            }
            else
            {
                ID = 0;
            }
        }
        #endregion

        #region static 
        public static int DeleteRates(int id)
        {
            return db.ExecuteScalarInt(DELETERATES, id);
        }

        public static bool UpdateRates(int id, double? initialIndex, double? expectedIndex, double? publishedIndex, double? publishedMargin, double? publishedExpectedIndex, double? publishedExpectedMargin)
        {
            return db.ExecuteScalarInt(UPDATERATES, id, initialIndex, expectedIndex, publishedIndex, publishedMargin, publishedExpectedIndex, publishedExpectedMargin) > 0;
        }

        public static DataView GetBaydocAppPackageTypeList()
        {
            return db.GetDataView(GETBAYDOCCODES, APPPACKAGETYPE);
        }
        public static DataView GetBaydocClosingPackageTypeList()
        {
            return db.GetDataView(GETBAYDOCCODES, CLOSINGPACKAGETYPE);
        }
        public static int ValidateProductsRate(DateTime period)
        {
            return db.ExecuteScalarInt("ValidateProductsRate", period);
        }
        public static DataView GetProductList(bool all)
        {
            return db.GetDataView(GETPRODUCTLIST, all);
        }
        public static DataView GetProductBasisList()
        {
            return db.GetDataView("GetProductBasisList");
        }
        public static DataView GetProductRateLockMethodList()
        {
            return db.GetDataView("GetProductRateLockMethodList");
        }
        public static DataView GetProductRateRoundingMethodList()
        {
            return db.GetDataView("GetProductRateRoundingMethodList");
        }
        public static DataView GetProductRateUpdateIntervalList()
        {
            return db.GetDataView("GetProductRateUpdateIntervalList");
        }
        public static DataView GetProductRateInputMethodList()
        {
            return db.GetDataView("GetProductRateInputMethodList");
        }

        public static DataView GetProductRateList(int ProductID)
        {
            return db.GetDataView("GetProductRateList", (ProductID == -1) ? DBNull.Value : (object)ProductID);
        }
        public static DataView GetRateListForProduct(int productID)
        {
            return db.GetDataView("GetRateListForProduct", productID);
        }
        public static DataView GetProductAmortizationTypeList()
        {
            return db.GetDataView(GETPRODUCTAMORTIZATIONTYPELIST);
        }
        public static DataView GetProductArmTypeList()
        {
            return db.GetDataView(GETPRODUCTARMTYPELIST);
        }
        public static DataView GetDaysList()
        {
            return db.GetDataView(GETDAYSLIST);
        }
        public static DataView GetProductTypeList()
        {
            return db.GetDataView(GETPRODUCTTYPELIST);
        }
        public static DataView GetProductIndex()
        {
            return db.GetDataView(GETPRODUCTINDEX);
        }
        public static DataView GetProductsIds()
        {
            return db.GetDataView(GETPRODUCTIDS);
        }
        public static DataView GetProductsNames()
        {
            return db.GetDataView(GETPRODUCTNAMES);
        }
        public static DataView GetProductsListForOriginator(int companyId, int productId)
        {
            return db.GetDataView(GETPRODUCTLISTFORORIGINATOR,companyId, productId);
        }
        public static DataView GetAllProductsWithLendersForOriginator(int companyId)
        {
            return db.GetDataView(GETALLPRODUCTSWITHLENDERSFORORIGINATOR,companyId);
        }
        public static DataView GetLenderServiceFee(int productId, int lenderId)
        {
            return db.GetDataView(GETLENDERSERVICEFEE, productId, lenderId);
        }
        public static int CopyRates(int srcProductId, int dstProductId, DateTime dtFrom, DateTime dtTo)
        {
            return db.ExecuteScalarInt(COPYRATES, srcProductId, dstProductId, dtFrom, dtTo);
        }
        #endregion


        #endregion

    }
}
