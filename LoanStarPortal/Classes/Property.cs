using System;
using System.Collections;
using System.Data;


namespace LoanStar.Common
{
    public class Property :BaseObject
    {

        public class PropertyException : BaseObjectException
        {
            public PropertyException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public PropertyException(string message)
                : base(message)
            {
            }

            public PropertyException()
            {
            }
        }


        #region constants
        private const int RESIDENCETYPEPRIMARYID = 1;
        private const int RESIDENCETYPESECONDARYID = 2;
        private const int RESIDENCETYPEINVESTMENTID = 3;
        private const int MANUFACTUREDHOMEID = 5;
        private const int MULTIFAMILTYTYPEID = 6;
        private const decimal LENDINGLIMIT = 625500;
        public const int TYPEEXISTINGHOMEPREVIOUSLYOCCUPIEDID = 1;
        public const int TYPENEWCONDOUNITID = 2;
        public const int TYPEEXISTINGCONDOUNITID = 3;
        public const int TYPEEXISTINGHOMENOTPREVIOUSLYOCCUPIEDID = 4;
        public const int TYPEPERMANENTLYSITEDHOMEANDLOTID = 5;
        #region sp names
        private const string PROPERTYTABLE = "MortgageProperty";
        private const string GETPROPERTYBYID = "GetPropertyById";
/*
        private const string GETLENDINGLIMIT = "GetCountyLendingLimit";
*/
        private const string GETCOUNTYDATA = "GetCountyData";
        private const string ZIPREGEXP = @"\d{5}";
        private const string PHONEREGEXP = @"\d{10}";
        #endregion
        
        #endregion

        enum TitleHeld
        {
            FeeSimple = 1,
            LifeEstate = 2,
            Leasehold = 3
        }
        enum HOAFeesPaid
        {
            Peryear = 1,
            Permonth = 2
        }

        #region fields
        private readonly MortgageProfile mp;
        private decimal lendingLimit = LENDINGLIMIT;
        private string address1 = String.Empty;
        private string address2 = String.Empty;
        private string city = String.Empty;
        private string neighborhoodName;
        private int stateId = -1;
        private string stateName = String.Empty;
        private string zip = String.Empty;
        private string county = String.Empty;
        private int countyID = 0;
        private bool? hazardId;
        private decimal? hazDwelling;
        private DateTime? hazStart;
        private DateTime? hazExp;
        private decimal? hazPremium;
        private string hazAgencyName = String.Empty;
        private string hazAgentName = String.Empty;
        private string hazAgencyPhone = String.Empty;
        private string hazAgencyFax = String.Empty;
        private bool? floodId;
        private decimal? fldDwelling;
        private DateTime? fldStart;
        private DateTime? fldExp;
        private decimal? fldPremium;
        private bool? fldMtgeeClauseOK;
        private bool? fldAllBorrOnPolicy;
        private string fldAgencyName = String.Empty;
        private string fldAgentName = String.Empty;
        private string fldAgencyPhone = String.Empty;
        private string fldAgencyFax = String.Empty;
        private int spTitleHeldId;
        private DateTime? spTitleHeldLeaseholdExpiration;
        private string spTitleHeld = String.Empty;
        private string spTitleIsHeldInTheseNames = String.Empty;
        private bool? spHeldInTrustId;
        private string loTitle = String.Empty;
        private string loFax = String.Empty;
        private string spOwnerOfPublicRecord = String.Empty;
        private string spLegalDescription = String.Empty;
        private string spAssessorsParcelNumber = String.Empty;
        private string spOccupancy = String.Empty;
        private string spHomeType = String.Empty;
        private string spWater = String.Empty;
        private string spSewer = String.Empty;
        private string spStreet = String.Empty;
        private bool? spRepairsYNId;
        private int spOccupancyId = 0;
        private int spHomeTypeId = 0;
        private int spWaterId = 0;
        private int spSewerId = 0;
        private int spStreetId = 0;
        private int spUnitsId=0;
        private int? spYearBuilt;
        private bool? spFloodZoneId;
        private bool? spHOAAssessmentUnitId;
        private decimal? spHOAAssessmentAmount;
        private decimal? spValue;
        private decimal? propertyTaxDueAmount;
        private int hazardPolicyPeriodId = 0;
        private int floodPolicyPeriodId = 0;
        private bool? policyMortgageeClauseMatchYNId;
        private bool? allBorrowersOnPolicyYNId;
        private decimal realEstateTaxes;
        private decimal? hoaFees;
        private string propertyRightsOther;
        private string area;
        private string floodZoneDescription;
        private int foundationTypeID;
        private int statusID;
        private string femaMapNumber;
        private DateTime? femaMapDate;
        private int numberOfStories;
        private string projectName = String.Empty;
        private Hashtable errMessages;
        private bool? readAntiCoercionStmntId;
        private bool? selectedHazardAgentId;
        private string hazAgencyAddress1;
        private string hazAgencyAddress2;
        private string hazAgencyCity;
        private int hazAgencyStateId = 0;
        private string hazAgencyZip;
        private int hoaFeesPaidId = 0;
        private int waterId = 0;
        private int sanatarySewerId = 0;
        private string waterPrivate;
        private string sanatarySewerPrivate;
        private int street = 0;
        private bool? poolId;
        private bool? builtAfter1978Id;
        private bool? childUnder7Id;
        private bool? wellId;
        private bool? septicId;
        private bool? orderedAppraisalId;
        private bool? receivedAppraisalId;
        private bool? appraisalAddressMatchId;
        private bool? appraisalCaseNumAllPgsId;
        private bool? appraiserFHAApprovedId;
        private bool? appraiserLicensedId;
        private bool? appraisalLglDescriptionId;
        private bool? appraisalCensusTractNumId;
        private bool? appraisalOwnerOccId;
        private bool? appraisalLotDimPresentId;
        private int appraisalLandAreaId = 0;
        private bool? appraisalZoningId;
        private bool? appraisalBestUseId;
        private bool? appraisalAdverseConditionsId;
        private bool? appraisalRepairsYNId;
        private bool? appraisalFourOrLessUnitsId;
        private bool? appraisalAsIsId;
        private int? appraisalREL;
        private bool? appraisalSiteCostNewPresentId;
        private decimal? appraisalSiteValue;
        private decimal? appraisalCostNew;
        private bool? pudSectionCompleteId;
        private bool? appraisalPhotosPresentId;
        private bool? appraisalSketchPresentId;
        private bool? appraisalMapPresentId;
        private bool? appraisalCommentWellSepticId;
        private bool? propInspOrderedId;
        private bool? propInspReceivedId;
        private DateTime? appraisalInspectionDate;
        private bool? daysAtProperty183Id;
        private int whatOtherTypeOnTitleId = 0;
        private bool? otherPersonsInVestingId;
        private string exactLegalVesting;
        private bool? floodCertOrderedId;
        private bool? floodCertRecvdId;
        private bool? floodCertGEOCodingId;
        private bool? floodCertLifeOfLoanId;
        private bool? floodCertLifeOfLoanAssignedId;
        private bool? floodCertFloodZoneId;
        private bool? floodDecRecvdId;
        private decimal? floodPolicyCoverage;
        private bool? floodPolicyCoverageIncreasingId;
        private decimal? floodPolicyDeductible;
        private DateTime? floodPolicyStart;
        private DateTime? floodPolicyEnd;
        private bool? manuFEMALetterId;
        private bool? manuElevationCertReceivedId;
        private bool? elevCertAbove100YrFldPlnId;
        private bool? hazDecRecvdId;
        private bool? hazPolGuarReplCostId;
        private decimal? hazPolCoverageAmnt;
        private decimal? hazPolDeductAmnt;
        private DateTime? hazPolStart;
        private DateTime? hazPolEnd;
        private bool? hazPolAddressOkId;
        private bool? hazPolNamesOkId;
        private decimal? appraisedValue;
        private int primaryHeatSourceId = 0;
        private bool? spaceHeatInspectionId;
        private int spaceHeatTypicalId = 0;
        private int convHeat50DegreesId = 0;
        private bool? ordTitleYetId;
        private bool? recTitleYetId;
        private DateTime? titleIssueDate;
        private bool? titlePropAddressOkId;
        private bool? titleFHACaseNumbOkId;
        private string titleLglDesc;
        private bool? titleTaxSearchId;
        private bool? titleLiensYNId;
        private DateTime? nextPropTaxDue;
        private bool? onTitleNotApplyId;
        private bool? vestingChangingId;
        private bool? removDeceasedId;
        private bool? deathCertReceivedYNId;
        private bool? standEndorseId;
        private bool? pudEndorseId;
        private bool? trustEndorseId;
        private bool? includeTitleInsuredClauseId;
        private bool? receivedCPLId;
        private bool? cplLetterOkId;
        private bool? askedForSurveyId;
        private bool? receivedSurveyId;
        private int surveyTitleResponseId = 0;
        private bool? surveyOrderedId;
        private bool? surveyRecvdId;
        private bool? surveyApprovedId;
        private int whichAppraisalFormUsedId = 0;
        private bool? publicWaterStmntId;
        private bool? publicWaterAvailableId;
        private bool? recvdBidWaterHookupId;
        private bool? includeTapFeeId;
        private decimal? waterHookupBid;
        private bool? waterHookupWaivedId;
        private int privateWaterTypeId = 0;
        private bool? waterTestNeededId;
        private bool? waterTestOrderedId;
        private bool? waterTestReceivedId;
        private bool? waterTestPotableId;
        private bool? newWellInstalledId;
        private bool? newWellCostPaidAtClosingId;
        private bool? newWellInvoiceRecvdId;
        private int? sharedWellNumberHouses;
        private bool? sharedWellRecvdAgreemntId;
        private bool? publicSewerStmntId;
        private bool? publicSewerAvailableId;
        private bool? sewerHookupBidRecvdId;
        private bool? sewerBidIncludeTapFeeId;
        private decimal? sewerBidAmount;
        private bool? tryToWaiveSewerHookupId;
        private bool? sewerHookupWaivedId;
        private int privateSewerTypeId = 0;
        private int septicConditionId = 0;
        private bool? septicInspectionOrderedId;
        private bool? septicInspReportRecvdId;
        private bool? recvdSharedSepticAgreemntId;
        private bool? manuAppraisalId;
        private bool? condoAppraisalId;
        private bool? recvdMasterPolicyId;
        private bool? masterPol100ImprovId;
        private bool? masterPolProjectNameId;
        private bool? masterPolManagCoId;
        private bool? masterPolBorrNamesId;
        private bool? masterPolLoanNumId;
        private bool? masterPolInsAgntInfoYNId;
        private DateTime? masterPolStartDate;
        private DateTime? masterPolEndDate;

        private bool? trustRecvdId;
        private bool? trustAllPagesId;
        private bool? trustBorrTheBeneficId;
        private bool? trustRevocableId;
        private bool? titleApprovedTrustId;
        private int legalStandNBSId = 0;
        private string nbsName;
        private string nbsAddress1;
        private string nbsAddress2;
        private string nbsCity;
        private int nbsStateId=0;
        private string nbsZip;
        private string nbsSS;
        private DateTime? nbsDOB;
        private bool? nbsRecvdLetterId;
        private bool? nbsStateSeparationId;
        private bool? nbsLiveSeparateStmntId;
        private bool? nbsNoInterestStmntId;
        private bool? nbsApproveRMStmntId;
        private bool? nbsNotarizedId;

        private bool? manuCondoId;
        private float? manuSqFt;
        private DateTime? manuDateBuilt;
        private bool? manuOrigLocId;
        private bool? manuTaxedAsREId;
        private bool? manuPermChassisId;
        private bool? manuAxlesToungueGoneId;
        private bool? manuUtilitiesInstalledId;
        private bool? manuSkirtingId;
        private bool? manuAdditionsEverId;
        private bool? manuPermFoundId;
        private bool? manuPermFoundInspectedId;
        private bool? manuPermFoundInspRecvdId;
        private bool? manuFoundInspOkId;
        private bool? manuHaveTagsId;

        private bool? appraisalTermiteFlag;
        private bool? termiteDamageFlag;
        private bool? termiteInfestationEscrowed;
        private bool? termiteInfestationGone;
        private bool? termiteOrdered;
        private bool? termiteReceived;
        private bool? termiteReinspectionRecvd;
        private bool? termiteReportInfestation;
        private bool? termiteReportLeak;
        private bool? termiteTreatmentBidItemized;
        private bool? termiteTreatmentCompleted;
        private bool? termiteTreatmentOrdered;
        private bool? termiteTreatmentQualifiedBid;
        private bool? termiteTreatmentRecvdBid;

        private DateTime? termiteReportInspDate;
        private decimal? termiteTreatmentBidAmount;
        private int? howManyInsp;

        private bool? recvdEstimates;
        private bool? repairsStructural;

        private string addLienStmnt = String.Empty;
        private string appraiserName = String.Empty;
        private DateTime? apprInspDate;
        private int? daysAdvancePayTax;
        private bool? iSelectedHazardAgent;
        private bool? isDeedState;
        private bool? outstandingMortLienOnProperty;
        private DateTime? foreclosureDate;

        private bool? collectedUSPS;
        private bool? reviewUSPSMatchAll;
        private bool? collectedFHACondoApproval;
        private bool? requestedFHASpotCondo;
        private bool? collectedFHASpotCondo;
        private bool? titleIncludeLegalDesc;
        private bool? appraisalFirstPagesPresent;
        private bool? appraisalAddendumPresent;
        private bool? appraisalPlatPresent;
        private bool? appraisalLimitedConditionsPresent;
        private bool? contractorBidRequested;
        private bool? structuralInspectionRequested;
        private bool? structuralInspectionCollected;
        private bool? appraisalSepticInspectionNeeded;
        private bool? appraisalOilTankInspectionNeeded;
        private bool? oilTankInspectionRequested;
        private bool? oilTankInspectionCollected;
        private bool? appraisalRoofInspectionNeeded;
        private bool? roofInspectionRequested;
        private bool? roofInspectionCollected;
        private bool? hazardDecPageRequested;
        private bool? floodDecPageRequested;
        private bool? trustRequested;
        private bool? deachCertRequested;
        private int? numberOfBedrooms;
        private string manuMake = String.Empty;
        private string manuModel = String.Empty;
        private string manuSerialNumber = String.Empty;
        private DateTime? manuYearBuilt;
        private string manuHomeSize = String.Empty;
        private string manuTitleCertNumber = String.Empty;
        private DataView dvRepairItems;
        private string trusteeName;
        private string trusteeSignatureLine;
        private int typeHomePurchasedId = 0;
        private bool? firstTimeBuyerYN;
        private int residenceTypeId = 0;

        private bool? titleAppraisalLegalMatch;
        private bool? titleNamesMatchAllOtherDocs;
        private bool? nbsOnTitleYN;
        private bool? requestedLeaseYN;
        private bool? receivedLeaseYN;
        private bool? leaseMortgagedUnlimited;
        private bool? leaseExpDateOk;
        private bool? leaseEndorsement;
        private bool? lotSizeCommonZonedRes;
        private bool? receivedDocIngressEgress;

        private bool? actualAppraiserMatchFHACase;
        private bool? roofDeficiencies;
        private bool? treatmentOrdered;
        private bool? termiteDamageEscrowed;
        private bool? termiteDamageRepairCompleted;
        private bool? termiteDamageOrderedReinsp;
        private bool? termiteDamageRecvdReinsp;
        private bool? termiteDamageGone;
        private bool? termiteDamageOrderedBid;
        private bool? termiteDamageRecvdBid;
        private bool? repairItemized;
        private bool? septicDeficiencies;
        private bool? oilTankDeficiencies;
        private bool? fldPolCorrectAdd;
        private bool? requestedMasterFloodDecPage;
        private bool? collectedMasterFloodDecPage;
        private bool? masterFloodPol100Improv;
        private bool? masterFloodPolProjectName;
        private bool? masterFloodPolManagCo;
        private bool? masterFloodPolBorrNames;
        private bool? masterFloodPolLoanNum;
        private bool? masterFloodPolInsAgntInfoYN;
        private bool? orderedHazMasterPolicy;
        private DateTime? masterFloodPolEndDate;
        private DateTime? masterFloodPolStartDate;

        private DateTime? titleReceivedDate;
        private DateTime? appraisalReceivedDate;
        private DateTime? pestReceivedDate;
        private DateTime? bidReceivedDate;
        private DateTime? waterTestReceivedDate;
        private DateTime? septicInspReceivedDate;
        private DateTime? oilTankInspReceivedDate;
        private DateTime? roofInspReceivedDate;
        private DateTime? floodCertReceivedDate;

        private bool? manuAppraiserUseTwoOtherManus;
        private bool? termiteDamageBidItemized;
        private string otherPropType;

        private bool? escrowRepairs;
        private decimal? repairSetAside;
        private decimal? repairSetAsideTaxesAndInsurance;
        private bool? hazMortgageeClausematchYN;

        public bool? HazMortgageeClausematchYN
        {
            get { return hazMortgageeClausematchYN; }
            set { hazMortgageeClausematchYN = value; }
        }
        public bool? EscrowRepairs
        {
            get { return escrowRepairs; }
            set { escrowRepairs = value; }
        }
        public decimal? RepairSetAside
        {
            get { return repairSetAside; }
            set { repairSetAside = value; }
        }
        public decimal? RepairSetAsideTaxesAndInsurance
        {
            get { return repairSetAsideTaxesAndInsurance; }
            set { repairSetAsideTaxesAndInsurance = value; }
        }

        public int NumberOfLiens
        {
            get
            {
                return mp.Payoffs.Count;
            }
        }
        public string OtherPropType
        {
            get { return otherPropType; }
            set { otherPropType = value; }
        }
        public string ManuRiderX
        {
            get
            {
                string res = String.Empty;
                if (spHomeTypeId == MANUFACTUREDHOMEID)
                {
                    res = "X";
                }
                return res;
            }
        }
        public string FullStreetAddress
        {
            get { return address1 + " " + address2; }
        }
        public decimal TotalPayoffPOC
        {
            get
            {
                decimal res = 0;
                for (int i=0;i<mp.Payoffs.Count;i++)
                {
                    if(mp.Payoffs[i].PayoffStatusId==Payoff.RECEIVEDSTATUSID)
                    {
                        res += mp.Payoffs[i].POC;
                    }
                }
                return res;
            }
        }
        public bool? TermiteDamageBidItemized
        {
            get { return termiteDamageBidItemized; }
            set { termiteDamageBidItemized = value;}
        }
        public bool? ManuAppraiserUseTwoOtherManus
        {
            get { return manuAppraiserUseTwoOtherManus; }
            set { manuAppraiserUseTwoOtherManus = value; }
        }

        public DateTime? TitleNotifyMeDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Originator != null && TitleExpDate != null)
                {
                    res = ((DateTime) TitleExpDate).AddDays(-mp.Originator.TitleDaysNotifyMeExp);
                }
                return res;
            }
        }
        public DateTime? AppraisalNotifyMeDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Originator != null && AppraisalExpDate != null)
                {
                    res = ((DateTime)AppraisalExpDate).AddDays(-mp.Originator.AppraisalDaysNotifyMeExp);
                }
                return res;
            }
        }
        public DateTime? PestNotifyMeDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Originator != null && PestExpDate != null)
                {
                    res = ((DateTime)PestExpDate).AddDays(-mp.Originator.PestDaysNotifyMeExp);
                }
                return res;
            }
        }
        public DateTime? BidNotifyMeDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Originator != null && BidExpDate != null)
                {
                    res = ((DateTime)BidExpDate).AddDays(-mp.Originator.BidDaysNotifyMeExp);
                }
                return res;
            }
        }
        public DateTime? WaterTestNotifyMeDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Originator != null && WaterTestExpDate != null)
                {
                    res = ((DateTime)WaterTestExpDate).AddDays(-mp.Originator.WaterTestDaysNotifyMeExp);
                }
                return res;
            }
        }
        public DateTime? SepticInspNotifyMeDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Originator != null && SepticInspExpDate != null)
                {
                    res = ((DateTime)SepticInspExpDate).AddDays(-mp.Originator.SepticInspDaysNotifyMeExp);
                }
                return res;
            }
        }
        public DateTime? OilTankInspNotifyMeDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Originator != null && OilTankInspExpDate != null)
                {
                    res = ((DateTime)OilTankInspExpDate).AddDays(-mp.Originator.OilTankInspDaysNotifyMeExp);
                }
                return res;
            }
        }
        public DateTime? RoofInspNotifyMeDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Originator != null && RoofInspExpDate != null)
                {
                    res = ((DateTime)RoofInspExpDate).AddDays(-mp.Originator.RoofInspDaysNotifyMeExp);
                }
                return res;
            }
        }
        public DateTime? FloodCertNotifyMeDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Originator != null && FloodCertExpDate != null)
                {
                    res = ((DateTime)FloodCertExpDate).AddDays(-mp.Originator.FloodCertDaysNotifyMeExp);
                }
                return res;
            }
        }


        public DateTime? TitleReceivedDate
        {
            get { return titleReceivedDate; }
            set { titleReceivedDate = value; }
        }
        public DateTime? AppraisalReceivedDate
        {
            get { return appraisalReceivedDate; }
            set { appraisalReceivedDate = value; }
        }
        public DateTime? PestReceivedDate
        {
            get { return pestReceivedDate; }
            set { pestReceivedDate = value; }
        }
        public DateTime? BidReceivedDate
        {
            get { return bidReceivedDate; }
            set { bidReceivedDate = value; }
        }
        public DateTime? WaterTestReceivedDate
        {
            get { return waterTestReceivedDate; }
            set { waterTestReceivedDate = value; }
        }
        public DateTime? SepticInspReceivedDate
        {
            get { return septicInspReceivedDate; }
            set { septicInspReceivedDate = value; }
        }
        public DateTime? OilTankInspReceivedDate
        {
            get { return oilTankInspReceivedDate; }
            set { oilTankInspReceivedDate = value; }
        }
        public DateTime? RoofInspReceivedDate
        {
            get { return roofInspReceivedDate; }
            set { roofInspReceivedDate = value; }
        }
        public DateTime? FloodCertReceivedDate
        {
            get { return floodCertReceivedDate; }
            set { floodCertReceivedDate = value; }
        }
        public DateTime? TitleExpDate
        {
            get
            {
                DateTime? res = null;
                if(mp.Product!=null&&titleReceivedDate!=null)
                {
                    res = ((DateTime) titleReceivedDate).AddDays(mp.Product.TitleActiveDays);
                }
                return res;
            }
        }
        public DateTime? AppraisalExpDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Product != null && appraisalReceivedDate != null)
                {
                    res = ((DateTime)appraisalReceivedDate).AddDays(mp.Product.AppraisalActiveDays);
                }
                return res;
            }
        }
        public DateTime? PestExpDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Product != null && pestReceivedDate != null)
                {
                    res = ((DateTime)pestReceivedDate).AddDays(mp.Product.PestActiveDays);
                }
                return res;
            }
        }
        public DateTime? BidExpDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Product != null && bidReceivedDate != null)
                {
                    res = ((DateTime)bidReceivedDate).AddDays(mp.Product.BidActiveDays);
                }
                return res;
            }
        }
        public DateTime? WaterTestExpDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Product != null && waterTestReceivedDate != null)
                {
                    res = ((DateTime)waterTestReceivedDate).AddDays(mp.Product.WaterTestActiveDays);
                }
                return res;
            }
        }
        public DateTime? SepticInspExpDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Product != null && septicInspReceivedDate != null)
                {
                    res = ((DateTime)septicInspReceivedDate).AddDays(mp.Product.SepticInspActiveDays);
                }
                return res;
            }
        }
        public DateTime? OilTankInspExpDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Product != null && oilTankInspReceivedDate != null)
                {
                    res = ((DateTime)oilTankInspReceivedDate).AddDays(mp.Product.OilTankInspActiveDays);
                }
                return res;
            }
        }
        public DateTime? RoofInspExpDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Product != null && roofInspReceivedDate != null)
                {
                    res = ((DateTime)roofInspReceivedDate).AddDays(mp.Product.RoofInspActiveDays);
                }
                return res;
            }
        }
        public DateTime? FloodCertExpDate
        {
            get
            {
                DateTime? res = null;
                if (mp.Product != null && floodCertReceivedDate != null)
                {
                    res = ((DateTime)floodCertReceivedDate).AddDays(mp.Product.FloodCertActiveDays);
                }
                return res;
            }
        }
        public decimal MinReqFloodCoverageAmt
        {
            get
            {
                decimal res = new GlobalAdmin().MaxFEMAFloodCoverage;
                res = Math.Min(res,Math.Min(appraisalCostNew==null?0:(decimal)appraisalCostNew,(spValue == null ? 0 : (decimal) spValue) -
                         (appraisalSiteValue == null ? 0 : (decimal) appraisalSiteValue)));
                return res;
            }
        }
        public decimal MaxHazardDedAmount
        {
            get
            {
                decimal res = 0;
                if(mp.Product!=null&&HazPolCoverageAmnt!=null)
                {
                    res = (decimal) (mp.Product.MaxHazDeductPercent*HazPolCoverageAmnt);
                }
                return res;
            }
        }
        public decimal MinHazardInsAmnt
        {
            get
            {
                return Math.Min(appraisalCostNew == null ? 0 : (decimal)appraisalCostNew, (spValue == null ? 0 : (decimal)spValue) - (appraisalSiteValue == null ? 0 : (decimal)appraisalSiteValue));
            }
        }
        public bool? FldPolCorrectAdd
        {
            get { return fldPolCorrectAdd; }
            set { fldPolCorrectAdd = value; }
        }
        public bool? RequestedMasterFloodDecPage
        {
            get { return requestedMasterFloodDecPage; }
            set { requestedMasterFloodDecPage = value; }
        }
        public bool? CollectedMasterFloodDecPage
        {
            get { return collectedMasterFloodDecPage; }
            set { collectedMasterFloodDecPage = value; }
        }
        public bool? MasterFloodPol100Improv
        {
            get { return masterFloodPol100Improv; }
            set { masterFloodPol100Improv = value; }
        }
        public bool? MasterFloodPolProjectName
        {
            get { return masterFloodPolProjectName; }
            set { masterFloodPolProjectName = value; }
        }
        public bool? MasterFloodPolManagCo
        {
            get { return masterFloodPolManagCo; }
            set { masterFloodPolManagCo = value; }
        }
        public bool? MasterFloodPolBorrNames
        {
            get { return masterFloodPolBorrNames; }
            set { masterFloodPolBorrNames = value; }
        }
        public bool? MasterFloodPolLoanNum
        {
            get { return masterFloodPolLoanNum; }
            set { masterFloodPolLoanNum = value; }
        }
        public bool? MasterFloodPolInsAgntInfoYN
        {
            get { return masterFloodPolInsAgntInfoYN; }
            set { masterFloodPolInsAgntInfoYN = value; }
        }
        public bool? OrderedHazMasterPolicy
        {
            get { return orderedHazMasterPolicy; }
            set { orderedHazMasterPolicy = value; }
        }
        public DateTime? MasterFloodPolEndDate
        {
            get { return masterFloodPolEndDate; }
            set { masterFloodPolEndDate = value; }
        }
        public DateTime? MasterFloodPolStartDate
        {
            get { return masterFloodPolStartDate; }
            set { masterFloodPolStartDate = value; }
        }


        public bool? ActualAppraiserMatchFHACase
        {
            get { return actualAppraiserMatchFHACase; }
            set { actualAppraiserMatchFHACase = value; }
        }
        public bool? RoofDeficiencies
        {
            get { return roofDeficiencies; }
            set { roofDeficiencies = value; }
        }
        public bool? TreatmentOrdered
        {
            get { return treatmentOrdered; }
            set { treatmentOrdered = value; }
        }
        public bool? TermiteDamageEscrowed
        {
            get { return termiteDamageEscrowed; }
            set { termiteDamageEscrowed = value; }
        }
        public bool? TermiteDamageRepairCompleted
        {
            get { return termiteDamageRepairCompleted; }
            set { termiteDamageRepairCompleted = value; }
        }
        public bool? TermiteDamageOrderedReinsp
        {
            get { return termiteDamageOrderedReinsp; }
            set { termiteDamageOrderedReinsp = value; }
        }
        public bool? TermiteDamageRecvdReinsp
        {
            get { return termiteDamageRecvdReinsp; }
            set { termiteDamageRecvdReinsp = value; }
        }
        public bool? TermiteDamageGone
        {
            get { return termiteDamageGone; }
            set { termiteDamageGone = value; }
        }
        public bool? TermiteDamageOrderedBid
        {
            get { return termiteDamageOrderedBid; }
            set { termiteDamageOrderedBid = value; }
        }
        public bool? TermiteDamageRecvdBid
        {
            get { return termiteDamageRecvdBid; }
            set { termiteDamageRecvdBid = value; }
        }
        public bool? RepairItemized
        {
            get { return repairItemized; }
            set { repairItemized = value; }
        }
        public bool? SepticDeficiencies
        {
            get { return septicDeficiencies; }
            set { septicDeficiencies = value; }
        }
        public bool? OilTankDeficiencies
        {
            get { return oilTankDeficiencies; }
            set { oilTankDeficiencies = value; }
        }
        public bool? TitleAppraisalLegalMatch
        {
            get { return titleAppraisalLegalMatch; }
            set { titleAppraisalLegalMatch = value; }
        }
        public bool? TitleNamesMatchAllOtherDocs
        {
            get { return titleNamesMatchAllOtherDocs; }
            set { titleNamesMatchAllOtherDocs = value; }
        }
        public bool? NBSOnTitleYN
        {
            get { return nbsOnTitleYN; }
            set { nbsOnTitleYN = value; }
        }
        public bool? RequestedLeaseYN{
            get { return requestedLeaseYN; }
            set { requestedLeaseYN = value; }
        }
        public bool? ReceivedLeaseYN
        {
            get { return receivedLeaseYN; }
            set { receivedLeaseYN = value; }
        }
        public bool? LeaseMortgagedUnlimited
        {
            get { return leaseMortgagedUnlimited; }
            set { leaseMortgagedUnlimited = value; }
        }
        public bool? LeaseExpDateOk
        {
            get { return leaseExpDateOk; }
            set { leaseExpDateOk = value; }
        }
        public bool? LeaseEndorsement
        {
            get { return leaseEndorsement; }
            set { leaseEndorsement = value; }
        }
        public bool? LotSizeCommonZonedRes
        {
            get { return lotSizeCommonZonedRes; }
            set { lotSizeCommonZonedRes = value; }
        }
        public bool? ReceivedDocIngressEgress
        {
            get { return receivedDocIngressEgress; }
            set { receivedDocIngressEgress = value; }
        }


        public int ResidenceTypeId
        {
            get { return residenceTypeId; }
            set { residenceTypeId = value; }
        }
        public bool? FirstTimeBuyerYN
        {
            get { return firstTimeBuyerYN; }
            set { firstTimeBuyerYN = value; }
        }
        public bool FirstTimeBuyerYesYN
        {
            get 
            {
                bool res = false;
                if (firstTimeBuyerYN!=null)
                {
                    res = (bool) firstTimeBuyerYN;
                }
                return res; 
            }
        }
        public bool FirstTimeBuyerNoYN
        {
            get
            {
                bool res = false;
                if (firstTimeBuyerYN != null)
                {
                    res = !((bool)firstTimeBuyerYN);
                }
                return res;
            }
        }
        public int TypeHomePurchasedId
        {
            get { return typeHomePurchasedId; }
            set { typeHomePurchasedId = value; }
        }

        public bool TypeHomePurchasedExistingHomePreviouslyOccupiedYN
        {
            get { return typeHomePurchasedId == TYPEEXISTINGHOMEPREVIOUSLYOCCUPIEDID; }
        }
        public bool TypeHomePurchasedNewCondoUnitYN
        {
            get { return typeHomePurchasedId == TYPENEWCONDOUNITID; }
        }
        public bool TypeHomePurchasedExistingCondoUnitYN
        {
            get { return typeHomePurchasedId == TYPEEXISTINGCONDOUNITID; }
        }
        public bool TypeHomePurchasedExistingHomeNotPreviouslyOccupiedYN
        {
            get { return typeHomePurchasedId == TYPEEXISTINGHOMENOTPREVIOUSLYOCCUPIEDID; }
        }
        public bool TypeHomePurchasedPermanentlySitedManufacturedHomeAndLotYN
        {
            get { return typeHomePurchasedId == TYPEPERMANENTLYSITEDHOMEANDLOTID; }
        }
        public string TrusteeName
        {
            get{return trusteeName;}
            set {trusteeName=value;}
        }
        public string TrusteeSignatureLine
        {
            get { return trusteeSignatureLine; }
            set { trusteeSignatureLine = value; }
        }
        public string CompoundAddress
        {
            get {return address1+","+address2;}
        }
        public int? NumberOfBedrooms
        {
            get {return numberOfBedrooms;}
            set {numberOfBedrooms=value;}
        }
        public string ManuHome
        {
            get{return (spHomeTypeId == MANUFACTUREDHOMEID)?"X":"";}
        }
        public string ManuMake
        {
            get{return manuMake;}
            set{manuMake=value;}
        }
        public string ManuModel
        {
            get{return manuModel;}
            set{manuModel=value;}
        }
        public string ManuSerialNumber
        {
            get{return manuSerialNumber;}
            set{manuSerialNumber=value;}
        }
        public DateTime? ManuYearBuilt
        {
            get { return manuYearBuilt; }
            set { manuYearBuilt = value; }
        }
        public string ManuHomeSize
        {
            get { return manuHomeSize; }
            set { manuHomeSize = value; }
        }
        public string ManuTitleCertNumber
        {
            get { return manuTitleCertNumber; }
            set { manuTitleCertNumber = value; }
        }
        public DataView DvRepairItems
        {
            get 
            {
                if (dvRepairItems == null)
                {
                    dvRepairItems = PropertyRepairItem.GetRepairItemsList(ID);
                }
                return dvRepairItems;
            }
        }
        public string MultiFamRider
        {
            get { return (spHomeTypeId == MULTIFAMILTYTYPEID) ? "X" : ""; }
        }
        public string DescriptionOfAllRepairs
        {
            get 
            { 
                string res = "";
                if (DvRepairItems != null)
                {
                    for (int i = 0; i < DvRepairItems.Count; i++)
                    {
                        res += DvRepairItems[i]["description"] + " ";
                    }
                }
                return res;
            }            
        }

        #endregion

        #region properties
        public bool? IsDeedState
        {
            get { return isDeedState; }
            set { isDeedState=value;}
        }
        public bool SecondaryResidenceYN
        {
            get { return residenceTypeId == RESIDENCETYPESECONDARYID; }
        }
        public bool PrimaryResidenceYN
        {
            get { return residenceTypeId == RESIDENCETYPEPRIMARYID; }
        }
        public bool InvestmentPropertyYN
        {
            get { return residenceTypeId == RESIDENCETYPEINVESTMENTID; }
        }

        public decimal? MinFloodInsAmnt
        {
            get 
            {
                if (AppraisedValue == null || AppraisalSiteValue == null || AppraisalCostNew == null)
                    return null;

                decimal minValue = (decimal)AppraisedValue - (decimal)AppraisalSiteValue < (decimal)AppraisalCostNew ?
                    (decimal)AppraisedValue - (decimal)AppraisalSiteValue : (decimal)AppraisalCostNew;
                minValue = minValue < mp.GlobalAdmin.MaxFEMAFloodCoverage ?
                    minValue : mp.GlobalAdmin.MaxFEMAFloodCoverage;

                return minValue;
            }
        }
        public bool? ISelectedHazardAgent
        {
            get { return iSelectedHazardAgent; }
            set { iSelectedHazardAgent = value; }
        }
        public int? DaysAdvancePayTax
        {
            get { return daysAdvancePayTax; }
            set { daysAdvancePayTax = value; }
        }
        public DateTime? ApprInspDate
        {
            get { return apprInspDate; }
            set { apprInspDate = value; }
        }
        public string AppraiserName
        {
            get { return appraiserName; }
            set { appraiserName = value; }
        }
        public string AddLienStmnt
        {
            get { return addLienStmnt; }
            set { addLienStmnt = value; }
        }

        public bool? RecvdEstimates
        {
            get { return recvdEstimates; }
            set { recvdEstimates = value; }
        }
        public bool? RepairsStructural
        {
            get { return repairsStructural; }
            set { repairsStructural = value; }
        }

        public bool? AppraisalTermiteFlag
        {
            get { return appraisalTermiteFlag; }
            set { appraisalTermiteFlag = value; }
        }
        public bool? TermiteDamageFlag
        {
            get { return termiteDamageFlag; }
            set { termiteDamageFlag = value; }
        }
        public bool? TermiteInfestationEscrowed
        {
            get { return termiteInfestationEscrowed; }
            set { termiteInfestationEscrowed = value; }
        }
        public bool? TermiteInfestationGone
        {
            get { return termiteInfestationGone; }
            set { termiteInfestationGone = value; }
        }
        public bool? TermiteOrdered
        {
            get { return termiteOrdered; }
            set { termiteOrdered = value; }
        }
        public bool? TermiteReceived
        {
            get { return termiteReceived; }
            set { termiteReceived = value; }
        }
        public bool? TermiteReinspectionRecvd
        {
            get { return termiteReinspectionRecvd; }
            set { termiteReinspectionRecvd = value; }
        }
        public bool? TermiteReportInfestation
        {
            get { return termiteReportInfestation; }
            set { termiteReportInfestation = value; }
        }
        public bool? TermiteReportLeak
        {
            get { return termiteReportLeak; }
            set { termiteReportLeak = value; }
        }
        public bool? TermiteTreatmentBidItemized
        {
            get { return termiteTreatmentBidItemized; }
            set { termiteTreatmentBidItemized = value; }
        }
        public bool? TermiteTreatmentCompleted
        {
            get { return termiteTreatmentCompleted; }
            set { termiteTreatmentCompleted = value; }
        }
        public bool? TermiteTreatmentOrdered
        {
            get { return termiteTreatmentOrdered; }
            set { termiteTreatmentOrdered = value; }
        }
        public bool? TermiteTreatmentQualifiedBid
        {
            get { return termiteTreatmentQualifiedBid; }
            set { termiteTreatmentQualifiedBid = value; }
        }
        public bool? TermiteTreatmentRecvdBid
        {
            get { return termiteTreatmentRecvdBid; }
            set { termiteTreatmentRecvdBid = value; }
        }

        public DateTime? TermiteReportInspDate
        {
            get { return termiteReportInspDate; }
            set { termiteReportInspDate = value; }
        }
        public decimal? TermiteTreatmentBidAmount
        {
            get { return termiteTreatmentBidAmount; }
            set { termiteTreatmentBidAmount = value; }
        }
        public int? HowManyInsp
        {
            get { return howManyInsp; }
            set { howManyInsp = value; }
        }
        public bool TermiteReportExpired
        {
            get 
            {
                DateTime termRepInspDate = TermiteReportInspDate == null ? 
                    DateTime.Now : (DateTime)TermiteReportInspDate;
                return DateTime.Today > termRepInspDate.AddDays(90).Date;
            }
        }

        public bool? ManuCondoId
        {
            get { return manuCondoId; }
            set { manuCondoId = value; }
        }
        public float? ManuSqFt
        {
            get { return manuSqFt; }
            set { manuSqFt = value; }
        }
        public DateTime? ManuDateBuilt
        {
            get { return manuDateBuilt; }
            set { manuDateBuilt = value; }
        }
        public bool? ManuOrigLocId
        {
            get { return manuOrigLocId; }
            set { manuOrigLocId = value; }
        }
        public bool? ManuTaxedAsREId
        {
            get { return manuTaxedAsREId; }
            set { manuTaxedAsREId = value; }
        }
        public bool? ManuPermChassisId
        {
            get { return manuPermChassisId; }
            set { manuPermChassisId = value; }
        }
        public bool? ManuAxlesToungueGoneId
        {
            get { return manuAxlesToungueGoneId; }
            set { manuAxlesToungueGoneId = value; }
        }
        public bool? ManuUtilitiesInstalledId
        {
            get { return manuUtilitiesInstalledId; }
            set { manuUtilitiesInstalledId = value; }
        }
        public bool? ManuSkirtingId
        {
            get { return manuSkirtingId; }
            set { manuSkirtingId = value; }
        }
        public bool? ManuAdditionsEverId
        {
            get { return manuAdditionsEverId; }
            set { manuAdditionsEverId = value; }
        }
        public bool? ManuPermFoundId
        {
            get{return manuPermFoundId;}
            set { manuPermFoundId = value; }
        }
        public bool? ManuPermFoundInspectedId
        {
            get { return manuPermFoundInspectedId; }
            set { manuPermFoundInspectedId = value; }
        }
        public bool? ManuPermFoundInspRecvdId
        {
            get { return manuPermFoundInspRecvdId; }
            set { manuPermFoundInspRecvdId = value; }
        }
        public bool? ManuFoundInspOkId
        {
            get { return manuFoundInspOkId; }
            set { manuFoundInspOkId = value; }
        }
        public bool? ManuHaveTagsId
        {
            get { return manuHaveTagsId; }
            set { manuHaveTagsId = value; }
        }

        public bool? TrustRecvdId
        {
            get { return trustRecvdId; }
            set { trustRecvdId = value; }
        }
        public bool? TrustAllPagesId
        {
            get { return trustAllPagesId; }
            set { trustAllPagesId = value; }
        }
        public bool? TrustBorrTheBeneficId
        {
            get { return trustBorrTheBeneficId; }
            set { trustBorrTheBeneficId = value; }
        }
        public bool? TrustRevocableId
        {
            get { return trustRevocableId; }
            set { trustRevocableId = value; }
        }
        public bool? TitleApprovedTrustId
        {
            get { return titleApprovedTrustId; }
            set { titleApprovedTrustId = value; }
        }
        public int LegalStandNBSId
        {
            get { return legalStandNBSId; }
            set { legalStandNBSId = value; }
        }
        public string NBSName
        {
            get { return nbsName; }
            set { nbsName = value; }
        }
        public string NBSAddress1
        {
            get { return nbsAddress1; }
            set { nbsAddress1 = value; }
        }
        public string NBSAddress2
        {
            get { return nbsAddress2; }
            set { nbsAddress2 = value; }
        }
        public string NBSCity
        {
            get { return nbsCity; }
            set { nbsCity = value; }
        }
        public int NBSStateId
        {
            get { return nbsStateId; }
            set { nbsStateId = value; }
        }
        public string NBSZip
        {
            get { return nbsZip; }
            set { nbsZip = value; }
        }
        public string NBSSS
        {
            get { return nbsSS; }
            set { nbsSS = value; }
        }
        public DateTime? NBSDOB
        {
            get { return nbsDOB; }
            set { nbsDOB = value; }
        }
        public bool? NBSRecvdLetterId
        {
            get { return nbsRecvdLetterId; }
            set { nbsRecvdLetterId = value; }
        }
        public bool? NBSStateSeparationId
        {
            get { return nbsStateSeparationId; }
            set { nbsStateSeparationId = value; }
        }
        public bool? NBSLiveSeparateStmntId
        {
            get { return nbsLiveSeparateStmntId; }
            set { nbsLiveSeparateStmntId = value; }
        }
        public bool? NBSNoInterestStmntId
        {
            get { return nbsNoInterestStmntId; }
            set { nbsNoInterestStmntId = value; }
        }
        public bool? NBSApproveRMStmntId
        {
            get { return nbsApproveRMStmntId; }
            set { nbsApproveRMStmntId = value; }
        }
        public bool? NBSNotarizedId
        {
            get { return nbsNotarizedId; }
            set { nbsNotarizedId = value; }
        }
        public bool? ManuAppraisalId
        {
            get { return manuAppraisalId; }
            set { manuAppraisalId = value; }
        }
        public bool? CondoAppraisalId
        {
            get { return condoAppraisalId; }
            set { condoAppraisalId = value; }
        }
        public bool? RecvdMasterPolicyId
        {
            get { return recvdMasterPolicyId; }
            set { recvdMasterPolicyId = value; }
        }
        public bool? MasterPol100ImprovId
        {
            get { return masterPol100ImprovId; }
            set { masterPol100ImprovId = value; }
        }
        public bool? MasterPolProjectNameId
        {
            get { return masterPolProjectNameId; }
            set { masterPolProjectNameId = value; }
        }
        public bool? MasterPolManagCoId
        {
            get { return masterPolManagCoId; }
            set { masterPolManagCoId = value; }
        }
        public bool? MasterPolBorrNamesId
        {
            get { return masterPolBorrNamesId; }
            set { masterPolBorrNamesId = value; }
        }
        public bool? MasterPolLoanNumId
        {
            get { return masterPolLoanNumId; }
            set { masterPolLoanNumId = value; }
        }
        public bool? MasterPolInsAgntInfoYNId
        {
            get { return masterPolInsAgntInfoYNId; }
            set { masterPolInsAgntInfoYNId = value; }
        }
        public DateTime? MasterPolStartDate
        {
            get { return masterPolStartDate; }
            set { masterPolStartDate = value; }
        }
        public DateTime? MasterPolEndDate
        {
            get { return masterPolEndDate; }
            set { masterPolEndDate = value; }
        }
        public decimal MaxClaimAmount
        {
            get
            {
                decimal res;
                decimal marketValue = (decimal)(spValue != 0 ? spValue : 0);
                res = Math.Min(marketValue, lendingLimit);
                return res;
            }
        }
        public decimal? TotWaterSewerHookUpBid
        {
            get { return waterHookupBid + sewerBidAmount; }
        }
        public decimal? Percent3OfValue
        {
            get
            {
                if(spValue!=null)
                {
                    return ((decimal)spValue)*0.3m;
                }
                return null;
            }
        }
        public decimal MinHazAmnt
        {
            get
            {
                decimal res;
                if (HazPolGuarReplCostId != null && !(bool)HazPolGuarReplCostId)
                {
                    res = Math.Min(((decimal) appraisedValue - (decimal) appraisalSiteValue), (decimal)appraisalCostNew);
                }
                else
                {
                    res = (decimal)HazPolCoverageAmnt;
                }
                return res;
            }
        }

        public int? SharedWellNumberHouses
        {
            get { return sharedWellNumberHouses; }
            set { sharedWellNumberHouses = value; }
        }
        public bool? SharedWellRecvdAgreemntId
        {
            get { return sharedWellRecvdAgreemntId; }
            set { sharedWellRecvdAgreemntId = value; }
        }
        public bool? PublicSewerStmntId
        {
            get { return publicSewerStmntId; }
            set { publicSewerStmntId = value; }
        }
        public bool? PublicSewerAvailableId
        {
            get { return publicSewerAvailableId; }
            set { publicSewerAvailableId = value; }
        }
        public bool? SewerHookupBidRecvdId
        {
            get { return sewerHookupBidRecvdId; }
            set { sewerHookupBidRecvdId = value; }
        }
        public bool? SewerBidIncludeTapFeeId
        {
            get { return sewerBidIncludeTapFeeId; }
            set { sewerBidIncludeTapFeeId = value; }
        }
        public decimal? SewerBidAmount
        {
            get { return sewerBidAmount; }
            set { sewerBidAmount = value; }
        }
        public bool? TryToWaiveSewerHookupId
        {
            get { return tryToWaiveSewerHookupId; }
            set { tryToWaiveSewerHookupId = value; }
        }
        public bool? SewerHookupWaivedId
        {
            get { return sewerHookupWaivedId; }
            set { sewerHookupWaivedId = value; }
        }
        public int PrivateSewerTypeId
        {
            get { return privateSewerTypeId; }
            set { privateSewerTypeId = value; }
        }
        public int SepticConditionId
        {
            get { return septicConditionId; }
            set { septicConditionId = value; }
        }
        public bool? SepticInspectionOrderedId
        {
            get { return septicInspectionOrderedId; }
            set { septicInspectionOrderedId = value; }
        }
        public bool? SepticInspReportRecvdId
        {
            get { return septicInspReportRecvdId; }
            set { septicInspReportRecvdId = value; }
        }
        public bool? RecvdSharedSepticAgreemntId
        {
            get { return recvdSharedSepticAgreemntId; }
            set { recvdSharedSepticAgreemntId = value; }
        }
        public decimal? WaterHookupBid
        {
            get { return waterHookupBid; }
            set { waterHookupBid = value; }
        }
        public bool? WaterHookupWaivedId
        {
            get { return waterHookupWaivedId; }
            set { waterHookupWaivedId = value; }
        }
        public int PrivateWaterTypeId
        {
            get { return privateWaterTypeId; }
            set { privateWaterTypeId = value; }
        }
        public bool? WaterTestNeededId
        {
            get { return waterTestNeededId; }
            set { waterTestNeededId = value; }
        }
        public bool? WaterTestOrderedId
        {
            get { return waterTestOrderedId; }
            set { waterTestOrderedId = value; }
        }
        public bool? WaterTestReceivedId
        {
            get { return waterTestReceivedId; }
            set { waterTestReceivedId = value; }
        }
        public bool? WaterTestPotableId
        {
            get { return waterTestPotableId; }
            set { waterTestPotableId = value; }
        }
        public bool? NewWellInstalledId
        {
            get { return newWellInstalledId; }
            set { newWellInstalledId = value; }
        }
        public bool? NewWellCostPaidAtClosingId
        {
            get { return newWellCostPaidAtClosingId; }
            set { newWellCostPaidAtClosingId = value; }
        }
        public bool? NewWellInvoiceRecvdId
        {
            get { return newWellInvoiceRecvdId; }
            set { newWellInvoiceRecvdId = value; }
        }

        public bool? PublicWaterStmntId
        {
            get { return publicWaterStmntId; }
            set { publicWaterStmntId = value; }
        }
        public bool? PublicWaterAvailableId
        {
            get { return publicWaterAvailableId; }
            set { publicWaterAvailableId = value; }
        }
        public bool? RecvdBidWaterHookupId
        {
            get { return recvdBidWaterHookupId; }
            set { recvdBidWaterHookupId = value; }
        }
        public bool? IncludeTapFeeId
        {
            get { return includeTapFeeId; }
            set { includeTapFeeId = value; }
        }

        public int WhichAppraisalFormUsedId
        {
            get { return whichAppraisalFormUsedId; }
            set { whichAppraisalFormUsedId = value; }
        }
        public bool? AskedForSurveyId
        {
            get { return askedForSurveyId; }
            set { askedForSurveyId = value; }
        }
        public bool? ReceivedSurveyId
        {
            get { return receivedSurveyId; }
            set { receivedSurveyId = value; }
        }
        public int SurveyTitleResponseId
        {
            get { return surveyTitleResponseId; }
            set { surveyTitleResponseId = value; }
        }
        public bool? SurveyOrderedId
        {
            get { return surveyOrderedId; }
            set { surveyOrderedId = value; }
        }
        public bool? SurveyRecvdId
        {
            get { return surveyRecvdId; }
            set { surveyRecvdId = value; }
        }
        public bool? SurveyApprovedId
        {
            get { return surveyApprovedId; }
            set { surveyApprovedId = value; }
        }


        public bool? StandEndorseId
        {
            get { return standEndorseId; }
            set { standEndorseId = value; }
        }
        public bool? PUDEndorseId
        {
            get { return pudEndorseId; }
            set { pudEndorseId = value; }
        }
        public bool? TrustEndorseId
        {
            get { return trustEndorseId; }
            set { trustEndorseId = value; }
        }
        public bool? IncludeTitleInsuredClauseId
        {
            get { return includeTitleInsuredClauseId; }
            set { includeTitleInsuredClauseId = value; }
        }
        public bool? ReceivedCPLId
        {
            get { return receivedCPLId; }
            set { receivedCPLId = value; }
        }
        public bool? CPLLetterOkId
        {
            get { return cplLetterOkId; }
            set { cplLetterOkId = value; }
        }

        public string TitleLglDesc
        {
            get { return titleLglDesc; }
            set { titleLglDesc = value; }
        }
        public bool? TitleTaxSearchId
        {
            get { return titleTaxSearchId; }
            set { titleTaxSearchId = value; }
        }
        public bool? TitleLiensYNId
        {
            get { return titleLiensYNId; }
            set { titleLiensYNId = value; }
        }
        public DateTime? NextPropTaxDue
        {
            get { return nextPropTaxDue; }
            set { nextPropTaxDue = value; }
        }
        public bool? OnTitleNotApplyId
        {
            get { return onTitleNotApplyId; }
            set { onTitleNotApplyId = value; }
        }
        public bool? VestingChangingId
        {
            get { return vestingChangingId; }
            set { vestingChangingId = value; }
        }
        public bool? RemovDeceasedId
        {
            get { return removDeceasedId; }
            set { removDeceasedId = value; }
        }
        public bool? DeathCertReceivedYNId
        {
            get { return deathCertReceivedYNId; }
            set { deathCertReceivedYNId = value; }
        }

        public bool? TitlePropAddressOkId
        {
            get { return titlePropAddressOkId; }
            set { titlePropAddressOkId = value; }
        }
        public bool? TitleFHACaseNumbOkId
        {
            get { return titleFHACaseNumbOkId; }
            set { titleFHACaseNumbOkId = value; }
        }
        public DateTime? TitleExpireDate
        {
            get
            {
                if(titleIssueDate!=null)
                {
                    return ((DateTime) titleIssueDate).AddDays(180);
                }
                return null;
            }
        }
        public bool? OrdTitleYetId
        {
            get { return ordTitleYetId; }
            set { ordTitleYetId = value; }
        }
        public bool? RecTitleYetId
        {
            get { return recTitleYetId; }
            set { recTitleYetId = value; }
        }
        public DateTime? TitleIssueDate
        {
            get { return titleIssueDate; }
            set { titleIssueDate = value; }
        }

        public decimal MaxHazDeductDollars
        {
            get
            {
                decimal res = 0;
                try
                {
                    if (mp.MortgageInfo.Product != null)
                    {
                        res = mp.MortgageInfo.Product.MaxHazDeductPercent * (decimal)hazPolCoverageAmnt;
                    }
                }
                catch
                {
                }
                return res;
            }
        }
        public int ConvHeat50DegreesId
        {
            get{ return convHeat50DegreesId;}
            set{ convHeat50DegreesId = value; }
        }
        public int SpaceHeatTypicalId
        {
            get{ return spaceHeatTypicalId;}
            set{ spaceHeatTypicalId = value;}
        }
        public bool? SpaceHeatInspectionId
        {
            get{ return spaceHeatInspectionId;}
            set{ spaceHeatInspectionId = value;}
        }
        public int PrimaryHeatSourceId
        {
            get { return primaryHeatSourceId; }
            set { primaryHeatSourceId = value; }
        }
        public decimal? AppraisedValue
        {
            get { return appraisedValue; }
            set { appraisedValue = value; }
        }
        public bool? HazPolAddressOkId
        {
            get { return hazPolAddressOkId; }
            set { hazPolAddressOkId = value; }
        }
        public bool? HazPolNamesOkId
        {
            get { return hazPolNamesOkId; }
            set { hazPolNamesOkId = value; }
        }
        public decimal? HazPolDeductAmnt
        {
            get { return hazPolDeductAmnt; }
            set { hazPolDeductAmnt = value; }
        }
        public DateTime? HazPolStart
        {
            get { return hazPolStart; }
            set { hazPolStart = value; }
        }
        public DateTime? HazPolEnd
        {
            get { return hazPolEnd; }
            set { hazPolEnd = value; }
        }

        public bool? HazDecRecvdId
        {
            get { return hazDecRecvdId; }
            set { hazDecRecvdId = value; }
        }
        public bool? HazPolGuarReplCostId
        {
            get { return hazPolGuarReplCostId; }
            set { hazPolGuarReplCostId = value; }
        }
        public decimal? HazPolCoverageAmnt
        {
            get { return hazPolCoverageAmnt; }
            set { hazPolCoverageAmnt = value; }
        }

        public bool? ManuFEMALetterId
        {
            get { return manuFEMALetterId; }
            set { manuFEMALetterId = value; }
        }
        public bool? ManuElevationCertReceivedId
        {
            get { return manuElevationCertReceivedId; }
            set { manuElevationCertReceivedId = value; }
        }
        public bool? ElevCertAbove100YrFldPlnId
        {
            get { return elevCertAbove100YrFldPlnId; }
            set { elevCertAbove100YrFldPlnId = value; }
        }

        public DateTime? FloodPolicyStart
        {
            get { return floodPolicyStart; }
            set { floodPolicyStart = value; }
        }
        public DateTime? FloodPolicyEnd
        {
            get { return floodPolicyEnd; }
            set { floodPolicyEnd = value; }
        }
        public decimal? FloodPolicyDeductible
        {
            get { return floodPolicyDeductible; }
            set { floodPolicyDeductible = value; }
        }
        public bool? FloodCertOrderedId
        {
            get { return floodCertOrderedId; }
            set { floodCertOrderedId = value; }
        }
        public bool? FloodCertRecvdId
        {
            get { return floodCertRecvdId; }
            set { floodCertRecvdId = value; }
        }
        public bool? FloodCertGEOCodingId
        {
            get { return floodCertGEOCodingId; }
            set { floodCertGEOCodingId = value; }
        }
        public bool? FloodCertLifeOfLoanId
        {
            get { return floodCertLifeOfLoanId; }
            set { floodCertLifeOfLoanId = value; }
        }
        public bool? FloodCertLifeOfLoanAssignedId
        {
            get { return floodCertLifeOfLoanAssignedId; }
            set { floodCertLifeOfLoanAssignedId = value; }
        }
        public bool? FloodCertFloodZoneId
        {
            get { return floodCertFloodZoneId; }
            set { floodCertFloodZoneId = value; }
        }
        public bool? FloodDecRecvdId
        {
            get { return floodDecRecvdId; }
            set { floodDecRecvdId = value; }
        }
        public decimal? FloodPolicyCoverage
        {
            get { return floodPolicyCoverage; }
            set { floodPolicyCoverage = value; }
        }
        public bool? FloodPolicyCoverageIncreasingId
        {
            get { return floodPolicyCoverageIncreasingId; }
            set { floodPolicyCoverageIncreasingId = value; }
        }
        public bool? OtherPersonsInVestingId
        {
            get { return otherPersonsInVestingId; }
            set { otherPersonsInVestingId = value; }
        }

        public int WhatOtherTypeOnTitleId
        {
            get{return whatOtherTypeOnTitleId;}
            set { whatOtherTypeOnTitleId = value; }
        }
        public bool? DaysAtProperty183Id
        {
            get { return daysAtProperty183Id; }
            set { daysAtProperty183Id = value; }
        }
        public bool HECMAppraisalExpired
        {
            get
            {
                if(appraisalInspectionDate==null)
                {
                    return false;
                }
                return DateTime.Now.AddDays(-180) > (DateTime)appraisalInspectionDate;
            }
        }
        public bool HKAppraisalExpired
        {
            get
            {
                if(appraisalInspectionDate==null)
                {
                    return false;
                }
                return DateTime.Now.AddDays(-120) > (DateTime)appraisalInspectionDate;
            }
        }
        public DateTime? AppraisalInspectionDate
        {
            get { return appraisalInspectionDate; }
            set { appraisalInspectionDate = value; }
        }
        public bool? PropInspOrderedId
        {
            get { return propInspOrderedId;}
            set { propInspOrderedId = value; }
        }
        public bool? PropInspReceivedId
        {
            get { return propInspReceivedId; }
            set { propInspReceivedId = value; }
        }
        public bool? OrderedAppraisalId
        {
            get { return orderedAppraisalId; }
            set { orderedAppraisalId = value; }
        }
        public bool? ReceivedAppraisalId
        {
            get { return receivedAppraisalId; }
            set { receivedAppraisalId = value; }
        }
        public bool? AppraisalAddressMatchId
        {
            get { return appraisalAddressMatchId; }
            set { appraisalAddressMatchId = value; }
        }
        public bool? AppraisalCaseNumAllPgsId
        {
            get { return appraisalCaseNumAllPgsId; }
            set { appraisalCaseNumAllPgsId = value; }
        }
        public bool? AppraiserFHAApprovedId
        {
            get { return appraiserFHAApprovedId; }
            set { appraiserFHAApprovedId = value; }
        }
        public bool? AppraiserLicensedId
        {
            get { return appraiserLicensedId; }
            set { appraiserLicensedId = value; }
        }
        public bool? AppraisalLglDescriptionId
        {
            get { return appraisalLglDescriptionId; }
            set { appraisalLglDescriptionId = value; }
        }
        public bool? AppraisalCensusTractNumId
        {
            get { return appraisalCensusTractNumId; }
            set { appraisalCensusTractNumId = value; }
        }
        public bool? AppraisalOwnerOccId
        {
            get { return appraisalOwnerOccId; }
            set { appraisalOwnerOccId = value; }
        }
        public bool? AppraisalLotDimPresentId
        {
            get { return appraisalLotDimPresentId; }
            set { appraisalLotDimPresentId = value; }
        }
        public int AppraisalLandAreaId
        {
            get { return appraisalLandAreaId; }
            set { appraisalLandAreaId = value; }
        }
        public bool? AppraisalZoningId
        {
            get { return appraisalZoningId; }
            set { appraisalZoningId = value; }
        }
        public bool? AppraisalBestUseId
        {
            get { return appraisalBestUseId; }
            set { appraisalBestUseId = value; }
        }
        public bool? AppraisalAdverseConditionsId
        {
            get { return appraisalAdverseConditionsId; }
            set { appraisalAdverseConditionsId = value; }
        }
        public bool? AppraisalRepairsYNId
        {
            get { return appraisalRepairsYNId; }
            set { appraisalRepairsYNId = value; }
        }
        public bool? AppraisalFourOrLessUnitsId
        {
            get { return appraisalFourOrLessUnitsId; }
            set { appraisalFourOrLessUnitsId = value; }
        }
        public bool? AppraisalAsIsId
        {
            get { return appraisalAsIsId; }
            set { appraisalAsIsId = value; }
        }
        public int? AppraisalREL
        {
            get { return appraisalREL; }
            set { appraisalREL = value; }
        }
        public bool? AppraisalSiteCostNewPresentId
        {
            get { return appraisalSiteCostNewPresentId; }
            set { appraisalSiteCostNewPresentId = value; }
        }
        public decimal? AppraisalSiteValue
        {
            get{ return appraisalSiteValue;}
            set{ appraisalSiteValue = value;}
        }
        public decimal? AppraisalCostNew
        {
            get { return appraisalCostNew; }
            set { appraisalCostNew = value; }
        }
        public bool? PUDSectionCompleteId
        {
            get { return pudSectionCompleteId; }
            set { pudSectionCompleteId = value; }
        }
        public bool? AppraisalPhotosPresentId
        {
            get { return appraisalPhotosPresentId; }
            set { appraisalPhotosPresentId = value; }
        }
        public bool? AppraisalSketchPresentId
        {
            get { return appraisalSketchPresentId; }
            set { appraisalSketchPresentId = value; }
        }
        public bool? AppraisalMapPresentId
        {
            get { return appraisalMapPresentId; }
            set { appraisalMapPresentId = value; }
        }
        public bool? AppraisalCommentWellSepticId
        {
            get { return appraisalCommentWellSepticId; }
            set { appraisalCommentWellSepticId = value; }
        }

        public bool WellAndSepticYes
        {
            get { return WellId != null && (bool)WellId && SepticId != null && (bool)SepticId; }
        }
        public bool WellAndSepticNo
        {
            get { return !WellAndSepticYes; }
        }
        public bool? WellId
        {
            get { return wellId; }
            set { wellId = value; }
        }
        public bool? SepticId
        {
            get { return septicId; }
            set { septicId = value; }
        }

        public bool ChildUnder7YesYN
        {
            get { return ChildUnder7Id != null && (bool)ChildUnder7Id; }
        }
        public bool ChildUnder7NoYN
        {
            get { return ChildUnder7Id != null && !(bool)ChildUnder7Id; }
        }
        public bool? ChildUnder7Id
        {
            get { return childUnder7Id; }
            set { childUnder7Id = value; }
        }
        public bool BuiltAfter1978YesYN
        {
            get { return BuiltAfter1978Id != null && (bool)BuiltAfter1978Id; }
        }
        public bool BuiltAfter1978NoYN
        {
            get { return BuiltAfter1978Id != null && !(bool)BuiltAfter1978Id; }
        }
        public bool? BuiltAfter1978Id
        {
            get { return builtAfter1978Id; }
            set { builtAfter1978Id = value; }
        }
        public bool FloodIdNoYN
        {
            get { return floodId != null && !(bool) floodId; }
        }
        public bool FloodIdYesYN
        {
            get { return floodId != null && (bool)floodId; }
        }
        public bool? PoolId
        {
            get { return poolId; }
            set { poolId = value; }
        }
        public int StreetId
        {
            get { return street; }
            set { street = value; }
        }
        public string SanatarySewerPrivate
        {
            get { return sanatarySewerPrivate; }
            set { sanatarySewerPrivate = value; }
        }
        public string WaterPrivate
        {
            get { return waterPrivate; }
            set { waterPrivate = value; }
        }
        public int SanatarySewerId
        {
            get { return sanatarySewerId; }
            set { sanatarySewerId = value; }
        }
        public int WaterId
        {
            get { return waterId; }
            set { waterId = value; }
        }
        public bool HOAFeesPaidPerYearYN
        {
            get { return hoaFeesPaidId == (int) HOAFeesPaid.Peryear; }
        }
        public bool HOAFeesPaidPerMonthYN
        {
            get { return hoaFeesPaidId == (int)HOAFeesPaid.Permonth; }
        }
        public int HOAFeesPaidId
        {
            get { return hoaFeesPaidId; }
            set { hoaFeesPaidId = value; }
        }
        public string SPFullSateName
        {
            get { return stateName; }
        }
        public string HazAgencyZip
        {
            get { return hazAgencyZip; }
            set { hazAgencyZip = value; }
        }
        public int HazAgencyStateId
        {
            get { return hazAgencyStateId; }
            set { hazAgencyStateId = value; }
        }
        public string HazAgencyCity
        {
            get { return hazAgencyCity; }
            set { hazAgencyCity = value; }
        }
        public string HazAgencyAddress1
        {
            get { return hazAgencyAddress1; }
            set { hazAgencyAddress1 = value; }
        }
        public string HazAgencyAddress2
        {
            get { return hazAgencyAddress2; }
            set { hazAgencyAddress2 = value; }
        }

        public bool FloodIdYes
        {
            get { return FloodId != null && (bool)FloodId; }
        }
        public bool FloodIdNo
        {
            get { return FloodId != null && !(bool)FloodId; }
        }
        public bool? SelectedHazardAgentId
        {
            get { return selectedHazardAgentId; }
            set { selectedHazardAgentId = value; }
        }
        public bool? ReadAntiCoercionStmntId
        {
            get { return readAntiCoercionStmntId; }
            set { readAntiCoercionStmntId = value; }
        }
        public decimal LendingLimit
        {
            get { return lendingLimit; }
            set { lendingLimit = value; }
        }
        public string Address1
        {
            get { return address1; }
            set {address1=value;}
        }
        public string Address2
        {
            get { return address2; }
            set {address2=value;}
        }
        public string City
        {
            get { return city; }
            set {city =value;}
        }

        public string NeighborhoodName
        {
            get { return neighborhoodName; }
            set { neighborhoodName = value; }
        }

        public int StateId
        {
            get { return stateId; }
            set
            {
                int oldStateID = stateId;
                stateId = value;
                if (oldStateID < 0 || oldStateID == stateId)
                    return;
                string errMsg;
                mp.UpdateObject("Property.CountyID", "0", ID, out errMsg);
                mp.Originator.StateId = value;
            }
        }
        public string StateName
        {
            get
            {
                string res = String.Empty;
                if(stateId>0)
                    res = db.ExecuteScalarString("GetStateNameById", stateId);
                return res;
            }
            set { stateName = value; }
        }

        public string SPFullStateName
        {
            get { return stateName; }
        }

        public string Zip
        {
            get { return zip; }
            set {zip=value;}
        }
        public string County
        {
            get { return county; }
            set {county=value;}
        }
        public bool? HazardId
        {
            get { return hazardId; }
            set {hazardId=value;}
        }
        public decimal? HazDwelling
        {
            get { return hazDwelling; }
            set {hazDwelling=value;}
        }
        public DateTime? HazStart
        {
            get { return hazStart; }
            set {hazStart=value;}
        }
        public DateTime? HazExp
        {
            get { return hazExp; }
            set {hazExp=value;}
        }
        public decimal? HazPremium
        {
            get { return hazPremium; }
            set { hazPremium=value;}
        }
        public string HazAgencyName
        {
            get { return hazAgencyName; }
            set { hazAgencyName=value;}
        }
        public string HazAgentName
        {
            get { return hazAgentName; }
            set {hazAgentName=value;}
        }
        public string HazAgencyPhone
        {
            get { return hazAgencyPhone; }
            set {hazAgencyPhone=value;}
        }
        public string HazAgencyFax
        {
            get { return hazAgencyFax; }
            set {hazAgencyFax=value;}
        }
        public bool? FloodId
        {
            get { return floodId; }
            set {floodId=value;}
        }
        public decimal? FldDwelling
        {
            get { return fldDwelling; }
            set { fldDwelling=value;}
        }
        public DateTime? FldStart
        {
            get { return fldStart; }
            set { fldStart=value;}
        }
        public DateTime? FldExp
        {
            get { return fldExp; }
            set { fldExp=value;}
        }
        public decimal? FldPremium
        {
            get { return fldPremium; }
            set { fldPremium=value;}
        }
        public bool? FldMtgeeClauseOK
        {
            get { return fldMtgeeClauseOK; }
            set { fldMtgeeClauseOK=value;}
        }
        public bool? FldAllBorrOnPolicy
        {
            get { return fldAllBorrOnPolicy; }
            set { fldAllBorrOnPolicy=value;}
        }
        public string FldAgencyName
        {
            get { return fldAgencyName; }
            set { fldAgencyName=value;}
        }
        public string FldAgentName
        {
            get { return fldAgentName; }
            set { fldAgentName = value; }
        }
        public string FldAgencyPhone
        {
            get { return fldAgencyPhone; }
            set { fldAgencyPhone=value;}
        }
        public string FldAgencyFax
        {
            get { return fldAgencyFax; }
            set { fldAgencyFax=value;}
        }
        public int SPTitleHeldId
        {
            get { return spTitleHeldId; }
            set { spTitleHeldId=value;}
        }

        public bool SPTitleHeldFeeSimpleYN
        {
            get { return spTitleHeldId == (int)TitleHeld.FeeSimple; }
        }

        public bool SPTitleHeldLifeEstateYN
        {
            get { return spTitleHeldId == (int)TitleHeld.LifeEstate; }
        }

        public bool SPTitleHeldLeaseholdYN
        {
            get { return spTitleHeldId == (int)TitleHeld.Leasehold; }
        }

        public DateTime? SPTitleHeldLeaseholdExpiration
        {
            get { return spTitleHeldLeaseholdExpiration; }
            set { spTitleHeldLeaseholdExpiration = value; }
        }

        public string SPTitleHeld
        {
            get { return spTitleHeld; }
            set { spTitleHeld=value;}
        }
        public string SPTitleIsHeldInTheseNames
        {
            get { return spTitleIsHeldInTheseNames; }
            set {spTitleIsHeldInTheseNames=value;}
        }
        public bool? SPHeldInTrustId
        {
            get { return spHeldInTrustId; }
            set { spHeldInTrustId=value;}
        }
        public string LOTitle
        {
            get { return loTitle; }
            set { loTitle=value;}
        }
        public string LOFax
        {
            get { return loFax; }
            set { loFax = value; }
        }
        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }
        public bool? SPRepairsYNId
        {
            get {return spRepairsYNId;}
            set { spRepairsYNId = value; }
        }
        public bool SPRepairsYNYes
        {
            get { return SPRepairsYNId != null && (bool)SPRepairsYNId; }
        }
        public bool SPRepairsYNNo
        {
            get { return SPRepairsYNId != null && !(bool)SPRepairsYNId; }
        }
        public int HazardPolicyPeriodId
        {
            get { return hazardPolicyPeriodId; }
            set { hazardPolicyPeriodId = value; }
        }
        public int FloodPolicyPeriodId
        {
            get { return floodPolicyPeriodId; }
            set { floodPolicyPeriodId = value; }
        }
        public bool? PolicyMortgageeClauseMatchYNId
        {
            get { return policyMortgageeClauseMatchYNId; }
            set { policyMortgageeClauseMatchYNId  = value; }
        }
        public bool? AllBorrowersOnPolicyYNId
        {
            get { return allBorrowersOnPolicyYNId; }
            set { allBorrowersOnPolicyYNId = value; }
        }
        public string LenderMortgageeClauseHzdLabel
        {
            get { return mp.Lender.LenderMortgageClause; }
        }
        public string LenderMortgageeClauseFldLabel
        {
            get
            {
                return mp.Lender.LenderMortgageClause;
            }
        }
        public Hashtable ErrMessages
        {
            get
            {
                if (errMessages == null)
                {
                    errMessages = MortgageProfileField.GetErrorMessages("Property");
                }
                return errMessages;
            }
        }
        public string SPOwnerOfPublicRecord
        {
            get { return spOwnerOfPublicRecord; }
            set { spOwnerOfPublicRecord = value; }
        }
        public string SPLegalDescription
        {
            get { return spLegalDescription;}
            set { spLegalDescription = value;}
        }
        public string SPAssessorsParcelNumber
        {
            get { return spAssessorsParcelNumber;}
            set { spAssessorsParcelNumber = value;}
        }
        public string SPOccupancy
        {
            get { return spOccupancy;}
            set { spOccupancy = value; }
        }
        public int SPOccupancyId
        {
            get { return spOccupancyId;}
            set { spOccupancyId = value;}
        }
        public string SPHomeType
        {
            get { return spHomeType;}
            set { spHomeType = value; }
        }
        public int SPHomeTypeId
        {
            get { return spHomeTypeId;}
            set { spHomeTypeId = value;}
        }
        public string SPWater
        {
            get { return spWater;}
            set { spWater = value; }
        }
        public int SPWaterId
        {
            get { return spWaterId;}
            set { spWaterId = value;}
        }
        public string SPSewer
        {
            get { return spSewer; }
            set { spSewer = value; }
        }
        public int SPSewerId
        {
            get { return spSewerId;}
            set { spSewerId = value;}
        }
        public string SPStreet
        {
            get { return spStreet; }
            set { spStreet = value; }
        }
        public int SPStreetId
        {
            get { return spStreetId; }
            set { spStreetId = value; }
        }

        public string FloodZoneDescription
        {
            get { return floodZoneDescription; }
            set { floodZoneDescription = value; }
        }

        public string FEMAMapNumber
        {
            get { return femaMapNumber; }
            set { femaMapNumber = value; }
        }
        public DateTime? FEMAMapDate
        {
            get { return femaMapDate; }
            set { femaMapDate= value; }
        }
        public int NumberOfStories
        {
            get { return numberOfStories; }
            set { numberOfStories = value; }
        }

        public int StatusID
        {
            get { return statusID; }
            set { statusID = value; }
        }
        public int FoundationTypeID
        {
            get { return foundationTypeID; }
            set { foundationTypeID = value; }
        }
        public int SPUnitsId
        {
            get { return spUnitsId; }
            set { spUnitsId = value;}
        }
        public int? SPYearBuilt
        {
            get { return spYearBuilt;}
            set { spYearBuilt = value;}
        }
        public bool? SPFloodZoneId
        {
            get { return spFloodZoneId;}
            set { spFloodZoneId = value;}
        }
        public bool? SPHOAAssessmentUnitId
        {
            get { return spHOAAssessmentUnitId;}
            set { spHOAAssessmentUnitId = value;}
        }
        public decimal? SPHOAAssessmentAmount
        {
            get { return spHOAAssessmentAmount;}
            set { spHOAAssessmentAmount = value;}
        }
        public decimal? SPValue
        {
            get { return spValue; }
            set
            {
                if (spValue == value)
                    return;
                if(!isInitialLoad)
                {
                    mp.MortgageInfo.ResetCalculator();
                }
                spValue = value;
            }
        }
        public decimal? PropertyTaxDueAmount
        {
            get { return propertyTaxDueAmount; }
            set { propertyTaxDueAmount = value; }
        }

        public decimal RealEstateTaxes
        {
            get { return realEstateTaxes; }
            set { realEstateTaxes = value; }
        }

        public decimal? HOAFees
        {
            get { return hoaFees; }
            set { hoaFees = value; }
        }

        public string PropertyRightsOther
        {
            get { return propertyRightsOther; }
            set { propertyRightsOther = value; }
        }

        public string Area
        {
            get { return area; }
            set { area = value; }
        }
        public int CountyID
        {
            get { return countyID; }
            set
            {
                countyID = value;
//                lendingLimit = GetLendingLimit();
                if(!isInitialLoad)
                {
//                    ReloadCountyValues();
                    mp.ResetInvoices();
                }
            }
        }
        public string ExactLegalVesting
        {
            get { return exactLegalVesting; }
            set { exactLegalVesting = value; }
        }
        public bool? OutstandingMortLienOnProperty
        {
            get { return outstandingMortLienOnProperty; }
            set { outstandingMortLienOnProperty = value; }
        }
        public DateTime? ForeclosureDate
        {
            get { return foreclosureDate; }
            set { foreclosureDate = value; }
        }
        public bool? CollectedUSPS
        {
            get { return collectedUSPS; }
            set { collectedUSPS = value; }
        }
        public bool? ReviewUSPSMatchAll
        {
            get { return reviewUSPSMatchAll; }
            set { reviewUSPSMatchAll = value; }
        }
        public bool? CollectedFHACondoApproval
        {
            get { return collectedFHACondoApproval; }
            set { collectedFHACondoApproval = value; }
        }
        public bool? RequestedFHASpotCondo
        {
            get { return requestedFHASpotCondo; }
            set { requestedFHASpotCondo = value; }
        }
        public bool? CollectedFHASpotCondo
        {
            get { return collectedFHASpotCondo; }
            set { collectedFHASpotCondo = value; }
        }
        public bool? TitleIncludeLegalDesc
        {
            get { return titleIncludeLegalDesc; }
            set { titleIncludeLegalDesc = value; }
        }
        public bool? AppraisalFirstPagesPresent
        {
            get { return appraisalFirstPagesPresent; }
            set { appraisalFirstPagesPresent = value; }
        }
        public bool? AppraisalAddendumPresent
        {
            get { return appraisalAddendumPresent; }
            set { appraisalAddendumPresent = value; }
        }
        public bool? AppraisalPlatPresent
        {
            get { return appraisalPlatPresent; }
            set { appraisalPlatPresent = value; }
        }
        public bool? AppraisalLimitedConditionsPresent
        {
            get { return appraisalLimitedConditionsPresent; }
            set { appraisalLimitedConditionsPresent = value; }
        }
        public bool? ContractorBidRequested
        {
            get { return contractorBidRequested; }
            set { contractorBidRequested = value; }
        }
        public bool? StructuralInspectionRequested
        {
            get { return structuralInspectionRequested; }
            set { structuralInspectionRequested = value; }
        }
        public bool? StructuralInspectionCollected
        {
            get { return structuralInspectionCollected; }
            set { structuralInspectionCollected = value; }
        }
        public bool? AppraisalSepticInspectionNeeded
        {
            get { return appraisalSepticInspectionNeeded; }
            set { appraisalSepticInspectionNeeded = value; }
        }
        public bool? AppraisalOilTankInspectionNeeded
        {
            get { return appraisalOilTankInspectionNeeded; }
            set { appraisalOilTankInspectionNeeded = value; }
        }
        public bool? OilTankInspectionRequested
        {
            get { return oilTankInspectionRequested; }
            set { oilTankInspectionRequested = value; }
        }
        public bool? OilTankInspectionCollected
        {
            get { return oilTankInspectionCollected; }
            set { oilTankInspectionCollected = value; }
        }
        public bool? AppraisalRoofInspectionNeeded
        {
            get { return appraisalRoofInspectionNeeded; }
            set { appraisalRoofInspectionNeeded = value; }
        }
        public bool? RoofInspectionRequested
        {
            get { return roofInspectionRequested; }
            set { roofInspectionRequested = value; }
        }
        public bool? RoofInspectionCollected
        {
            get { return roofInspectionCollected; }
            set { roofInspectionCollected = value; }
        }
        public bool? HazardDecPageRequested
        {
            get { return hazardDecPageRequested; }
            set { hazardDecPageRequested = value; }
        }
        public bool? FloodDecPageRequested
        {
            get { return floodDecPageRequested; }
            set { floodDecPageRequested = value; }
        }
        public bool? TrustRequested
        {
            get { return trustRequested; }
            set { trustRequested = value; }
        }
        public bool? DeachCertRequested
        {
            get { return deachCertRequested; }
            set { deachCertRequested = value; }
        }

        #endregion

        #region constructor
        public Property(int id,MortgageProfile _mp)
        {
            ID = id;
            mp = _mp;
            if (ID > 0)
            {
                LoadById();
            }
        }
        public Property()
        {
        }
        #endregion

        #region methods

        #region public
        public override int Save(MortgageProfile _mp, string objectName, string fullPropertyName, string propertyName, int objectTypeId, object val, object oldVal, string parentFieldName, int parentId, bool isRequired)
        {
            int res = base.Save(_mp, PROPERTYTABLE, fullPropertyName, propertyName, objectTypeId, val, oldVal, parentFieldName, parentId, isRequired);
            if ((res > 0) && (ID < 1))
            {
                mp.Property.ID = res;
            }
            return res;
        }
        public override bool ValidateProperty(string propertyName, out string err)
        {
            bool res = true;
            err = String.Empty;
            switch (propertyName)
            { 
                case "Zip":
                    if(!String.IsNullOrEmpty(zip))
                    {
                        res = MortgageProfileField.ValidateRegexp(zip, ZIPREGEXP);
                        if (!res)
                        {
                            GetError(propertyName, out err);
                        }
                    }
                    break;
                case "HazAgencyPhone":
                    if (!String.IsNullOrEmpty(hazAgencyPhone))
                    {
                        res = MortgageProfileField.ValidateRegexp(hazAgencyPhone, PHONEREGEXP);
                        if (!res)
                        {
                            GetError(propertyName, out err);
                        }
                    }
                    break;
                case "HazAgencyFax":
                    if (!String.IsNullOrEmpty(hazAgencyFax))
                    {
                        res = MortgageProfileField.ValidateRegexp(hazAgencyFax, PHONEREGEXP);
                        if (!res)
                        {
                            GetError(propertyName, out err);
                        }
                    }
                    break;
                case "FldAgencyPhone":
                    if (!String.IsNullOrEmpty(fldAgencyPhone))
                    {
                        res = MortgageProfileField.ValidateRegexp(fldAgencyPhone, PHONEREGEXP);
                        if (!res)
                        {
                            GetError(propertyName, out err);
                        }
                    }
                    break;
                case "FldAgencyFax":
                    if (!String.IsNullOrEmpty(fldAgencyFax))
                    {
                        res = MortgageProfileField.ValidateRegexp(fldAgencyFax, PHONEREGEXP);
                        if (!res)
                        {
                            GetError(propertyName, out err);
                        }
                    }
                    break;
            }
            return res;
        }
        #endregion

        #region private
        private void ReloadCountyValues()
        {
            DataView dv = db.GetDataView(GETCOUNTYDATA, countyID);
            if (dv != null && dv.Count == 1)
            {
                lendingLimit = decimal.Parse(dv[0]["lendinglimit"].ToString());
                county = dv[0]["name"].ToString();
            }

        }
        //private decimal GetLendingLimit()
        //{
        //    decimal res = LENDINGLIMIT;
        //    DataView dv = db.GetDataView(GETLENDINGLIMIT, countyID);
        //    if(dv!=null&&dv.Count==1)
        //    {
        //        res = decimal.Parse(dv[0]["lendinglimit"].ToString());
        //    }
        //    return res;
        //}

        private void GetError(string propertyName, out string err)
        {
            if (ErrMessages.ContainsKey(propertyName))
            {
                err = ErrMessages[propertyName].ToString();
                if(String.IsNullOrEmpty(err))
                {
                    err = "*";
                }
            }
            else
            {
                err = "*";
            }
        }

        private void LoadById()
        {
            DataView dv = db.GetDataView(GETPROPERTYBYID, ID);
            if (dv.Count == 1)
            {
                PopulateFromDataRow(dv[0]);
            }
            else 
            {
                ID = -1;
            }
        }
        #endregion

        public static decimal GetLendingLimit(int countyId)
        {
            decimal res = 0;
            DataView dv = db.GetDataView("GetLendingLimit", countyId);
            if (dv.Count == 1)
            {
                try
                {
                    res = decimal.Parse(dv[0]["lendinglimit"].ToString());
                }
                catch { }
            }
            return res;
        }
        #endregion

    }
}
