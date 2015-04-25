using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;


namespace LoanStar.Common
{
    /// <summary>
    /// Class for Borrower.
    /// </summary>
    public class Borrower : BaseObject
    {
        /// <summary>
        /// Any method of DocTemplate class must throw custom exceptions of this type only
        /// </summary>
        public class BorrowerException : BaseObjectException
        {
            public BorrowerException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public BorrowerException(string message)
                : base(message)
            {
            }

            public BorrowerException()
            {
            }
        }

        #region enums
        enum HDMAHide
        {
//            NotSelected = 0,
            AllInformationProvided=1,
            SomeInformationProvided=2,
            NoneInformationProvided=3
        }
        enum Ethnicity
        {
            Hispanic = 1,
            NotHispanic = 2
        }
        enum Race
        {
            AmericanIndianORAlaskaNative = 1,
            Asian = 2,
            BlackORAfricanAmerican = 3,
            NativeHawaiianOROtherPacificIslander = 4,
            White = 5
        }
        enum MartialStatus
        {
            Married = 1,
            Unmaried = 2
//            ,Widowed = 3
        }
        enum Sex
        {
            Male = 1,
            Female = 2
        }
        enum IdentificationDocumentType
        {
            StateIssuedDriversLicense = 1,
            StateIssuedIDCard = 2,
            MilitaryIDCard = 3,
            Passport = 4,
            USAlienRegistrationCard = 5,
            CanadianDriversLicense = 6,
            SocialSecurityCard = 7,
            ForeignCountryVisa = 8,
            InsuranceCard = 9,
            BirthCertificate = 10,
            NonUSCanadianDriversLicense = 11,
            PropertyTaxBill = 12,
            RecentUtilityBill = 13,
            VoterRegistrationCard = 14,
            OrganizationalMembershipCard = 15,
            StudentIdentificationCard = 16,
            BankInvestmentLoanStatements = 17,
            Homecarrenterinsurancepapers = 18
        }
        enum IdentificationDocCategory
        {
            Primary = 1,
            Secondary = 2
        }
        enum POALegalCapacity
        {
//            NotSet = 0,
            PowerOfAttorney = 1,
            ConservatorshipGuardianship = 2,
            Other = 3
        }
        #endregion

        #region constants


        private const string ZIPREGEXP = @"\d{5}";
        private const string PHONEREGEXP = @"\d{10}";
        private const string SSNREGEXP = @"\d{9}";
        private const string BORROWERTABLE = "Borrower";
        private const string HUDLIFEEXPECTANCYTABLE = "HUDLifeExpectancy";
        private const string GETIDENTIFICATIONDOCUMENTS = "GetBorrowerIdentificationDocuments";
        private const string GETBORROWERAKANAMES = "GetBorrowerAkaNames";
        private const string CATEGORYFILETER = "categoryid={0}";
        private const int PRIMARYIDENTIFICATIONID = 1;
        private const int SECONDARYIDENTIFICATIONID = 2;
        #endregion

        #region Private fields
        private readonly MortgageProfile mp = null;
        private string firstName = String.Empty;
        private string lastName = String.Empty;
        private string middleInitial = String.Empty;
        private int sexId; 
        private string address1 = String.Empty;
        private string address2 = String.Empty;
        private string city = String.Empty;
        private int stateID;
        private string zip = String.Empty;
        private DateTime? dateOfBirth;
        private string phone = String.Empty;
        private string ssn = String.Empty;
        private bool archived;
        private int salutationId;
        private int martialStatusId;
        private int? yearsAtPresentAddress;
        private decimal? monthlyIncome;
        private decimal? realEstateAssets;
        private decimal? availableAssets;
        private bool? differentMailingAddressId;
        private bool? usePOAId;
        private bool? decJudments;
        private bool? decBuncruptcy;
        private bool? decLawsuit;
        private bool? decFedDebt;
        private bool? decPrimaryres;
        private bool? decEndorser;
        private bool? decUSCitizen;
        private bool? decPermanentRes;
        private int hdmaHideId = 0;
        private string hdmaRace = String.Empty;
        private int hdmaRaceId=0;
        private string hdmaEthnicity = String.Empty;
        private int hdmaEthnicityId = 0;
        private bool? poaDurableId;
        private bool? poaEncumberingId;
        private bool? poaRevocableId;
        private bool? poaIncapacitatedId;
        private DateTime? poaExecutionDate;
        private int poaLegalCapacityId = 0;
        private bool? poaReviewedDocumentId;
        private bool? poaTransactionApproveId;
        private bool? poaSignedByJudgeId;
        private bool? poaMentionedInterestRateId;
        private bool? poaMentionedAmountId;
        private bool? poaMentionedByNameId;
        private bool? poaNotedInitialRateId;
        private bool? poaEqualOrGreaterMaximumId;
        private Hashtable errMessages;
        private string altContactName;
        private string altContactRelationship;
        private string altContactAddress1;
        private string altContactAddress2;
        private string altContactCity;
        private string altContactZip;
        private string altContactPhone;
        private string altContactAltPhone;
        private int altContactStateId;
        private decimal? totalAmountOfNonREDebts;
        private string aka1;
        private string aka2;
        private string aka3;
        private string aka4;
        private int identificationDocTypeId = 0;
        private BorrowerIdentificationDocument primaryIdentificationDocument;
        private BorrowerIdentificationDocument secondaryIdentificationDocument1;
        private BorrowerIdentificationDocument secondaryIdentificationDocument2;
        private DataView dvIdentificationDocuments;
        private bool? certifyRaceVisualObserId;
        private int counselingMethodId = 0;
        private string partialHMDAInfoStmnt;
        private bool? counseledId;
        private bool? inclOnCounsCertId;
        private decimal? otherRealEstateAssets;
        private bool? courtOrdRecvdId;
        private bool? courtOrdSignedId;
        private string guarFirstName;
        private string guarLastName;
        private string guarAddress1;
        private string guarAddress2;
        private string guarCity;
        private int guarStateId=0;
        private string guarZip;
        private string guarSSNum;
        private DateTime? guarDOB;
        private bool? guarCounsId;
        private bool? crtOrdAprvRMId;
        private bool? crtOrdLnAmntMentionedId;
        private bool? crtOrdLnAmntOkId;
        private bool? crtOrdIntRateMentionedId;
        private bool? crtOrdIntRateAsInitialRateId;
        private DateTime? crtOrdSignDate;

        private DateTime? poaBorrSignDate;
        private string poaFirstName;
        private string poaLastName;
        private string poaAddress1;
        private string poaAddress2;
        private string poaCity;
        private int poaStateId = 0;
        private string poaZip;
        private string poaSSNum;
        private DateTime? poaDOB;
        private bool? poaCounseledId;
        private bool? poaTitleApprvdId;
        private bool? poaBorrCompetentId;
        private bool? poaBorrSignAppId;
        private bool? poaBorrIncompetencyDocuemtned;
        private List<BorrowerIdentificationDocument> identificationDocuments;

        private bool? poaRequested;
        private bool? poaReceived;
        private bool? conservatorRequested;
        private bool? proofOfAgeRequested;
        private bool? proofOfAgeCollected;
        private bool? proofOfSSNRequested;
        private bool? proofOfSSNCollected;
        private string akaNames;
        private string emailAddress;

        #region latest changes

        private string signatureLine = String.Empty;
        private bool? akaNamesYN;
        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }
        public bool HMDADidNotFurnishChecked
        {
            get
            {
                return
                    ((hdmaHideId == (int)HDMAHide.SomeInformationProvided) ||
                     (hdmaHideId == (int)HDMAHide.NoneInformationProvided));
            }
        }
        public string SignatureLine
        {
            get { return signatureLine; }
            set { signatureLine = value; }
        }
        public bool? AKANamesYN
        {
            get { return akaNamesYN; }
            set { akaNamesYN = value; }
        }
        public string AllAKANames
        {
            get
            {
                string res = String.Empty;
                if (akaNamesYN != null && (bool)akaNamesYN)
                {
                    if(String.IsNullOrEmpty(akaNames))
                    {
                        akaNames = GetAkaNames();
                    }
                    res = akaNames;
                }
                return res;
            }
        }
        public string BorrowerType
        {
            get
            {
                string res;
                switch (poaLegalCapacityId)
                {
                    case (int)POALegalCapacity.PowerOfAttorney:
                        res = "P";
                        break;
                    case (int)POALegalCapacity.ConservatorshipGuardianship:
                        res = "G";
                        break;
                    case (int)POALegalCapacity.Other:
                        res = "O";
                        break;
                    default:
                        res = "S";
                        break;
                }
                return res;
            }
        }
        public string POA
        {
            get
            {
                return poaFirstName + " " + poaLastName;
            }
        }
        #endregion
        private bool? proofOfAgeDL;
        private bool? proofOfAgeDLExpired;
        private bool? proofOfAgeDoBMatchOtherDocs;
        private bool? proofOfSSMedicare;
        private bool? proofOfSSMatchOtherDocs;
        private bool? proofOfSSMedicareMatchWithA;

        public bool? ProofOfAgeDL
        {
            get { return proofOfAgeDL;}
            set { proofOfAgeDL = value;}
        }
        public bool? ProofOfAgeDLExpired
        {
            get { return proofOfAgeDLExpired;}
            set { proofOfAgeDLExpired = value;}
        }
        public bool? ProofOfAgeDoBMatchOtherDocs
        {
            get { return proofOfAgeDoBMatchOtherDocs;}
            set { proofOfAgeDoBMatchOtherDocs = value;}
        }
        public bool? ProofOfSSMedicare
        {
            get { return proofOfSSMedicare;}
            set { proofOfSSMedicare = value; }
        }
        public bool? ProofOfSSMatchOtherDocs
        {
            get { return  proofOfSSMatchOtherDocs;}
            set { proofOfSSMatchOtherDocs = value; }
        }
        public bool? ProofOfSSMedicareMatchWithA
        {
            get { return proofOfSSMedicareMatchWithA;}
            set { proofOfSSMedicareMatchWithA = value; }
        }

        #endregion

        #region Static members
        private static SortedList hudLifeExpectancyHash = null;
        private static SortedList HUDLifeExpectancyHash
        {
            get
            {
                if (hudLifeExpectancyHash == null)
                {
                    hudLifeExpectancyHash = new SortedList();
                    DataView hudLifeExpectancyView = DataHelpers.GetDictionary(HUDLIFEEXPECTANCYTABLE);
                    foreach (DataRowView ageRow in hudLifeExpectancyView)
                    {
                        int age = Convert.ToInt32(ageRow["Age"]);
//                        int lifeExpectancy = Convert.ToInt32(ageRow["ARLRoundUp"]);
                        int lifeExpectancy = Convert.ToInt32(ageRow["ARLNearestYear"]);
                        hudLifeExpectancyHash[age] = lifeExpectancy;
                    }
                }

                return hudLifeExpectancyHash;
            }
        }
        public string GetAkaNames()
        {
            string res = String.Empty; 
            DataView dv = GetAkaNames(ID);
            if(dv!=null)
            {
                for(int i=0;i<dv.Count;i++)
                {
                    if(i>0)
                    {
                        res += ",";
                    }
                    res += dv[i]["Name"].ToString();
                }
            }
            return res;
        }

        public static DataView GetAkaNames(int borrowerId)
        {
            return db.GetDataView(GETBORROWERAKANAMES, borrowerId);
        }
        #endregion

        #region properties
        public List<BorrowerIdentificationDocument> IdentificationDocuments
        {
            get
            {
                if (identificationDocuments == null)
                    identificationDocuments = GetIdentificationDocumentList();

                return identificationDocuments;
            }
        }

        public DateTime? POABorrSignDate
        {
            get { return poaBorrSignDate; }
            set { poaBorrSignDate = value; }
        }
        public string POAFirstName
        {
            get { return poaFirstName; }
            set { poaFirstName = value; }
        }
        public string POALastName
        {
            get { return poaLastName; }
            set { poaLastName = value; }
        }
        public string POAAddress1
        {
            get { return poaAddress1; }
            set { poaAddress1 = value; }
        }
        public string POAAddress2
        {
            get { return poaAddress2; }
            set { poaAddress2 = value; }
        }
        public string POACity
        {
            get { return poaCity; }
            set { poaCity = value; }
        }
        public int POAStateId
        {
            get { return poaStateId; }
            set { poaStateId = value; }
        }
        public string POAZip
        {
            get { return poaZip; }
            set { poaZip = value; }
        }
        public string POASSNum
        {
            get { return poaSSNum; }
            set { poaSSNum = value; }
        }
        public DateTime? POADOB
        {
            get { return poaDOB; }
            set { poaDOB = value; }
        }
        public bool? POACounseledId
        {
            get { return poaCounseledId; }
            set { poaCounseledId = value; }
        }
        public bool? POATitleApprvdId
        {
            get { return poaTitleApprvdId; }
            set { poaTitleApprvdId = value; }
        }
        public bool? POABorrCompetentId
        {
            get { return poaBorrCompetentId; }
            set { poaBorrCompetentId = value; }
        }
        public bool? POABorrSignAppId
        {
            get { return poaBorrSignAppId; }
            set { poaBorrSignAppId=value; }
        }
        public bool? POABorrIncompetencyDocuemtned
        {
            get { return poaBorrIncompetencyDocuemtned; }
            set { poaBorrIncompetencyDocuemtned = value; }
        }


        public bool? CrtOrdAprvRMId
        {
            get { return crtOrdAprvRMId; }
            set { crtOrdAprvRMId = value; }
        }
        public bool? CrtOrdLnAmntMentionedId
        {
            get{return crtOrdLnAmntMentionedId;}
            set{ crtOrdLnAmntMentionedId = value;}
        }
        public bool? CrtOrdLnAmntOkId
        {
            get { return crtOrdLnAmntOkId; }
            set { crtOrdLnAmntOkId = value; }
        }
        public bool? CrtOrdIntRateMentionedId
        {
            get { return crtOrdIntRateMentionedId; }
            set { crtOrdIntRateMentionedId = value; }
        }
        public bool? CrtOrdIntRateAsInitialRateId
        {
            get { return crtOrdIntRateAsInitialRateId; }
            set { crtOrdIntRateAsInitialRateId = value; }
        }
        public DateTime? CrtOrdSignDate
        {
            get { return crtOrdSignDate; }
            set { crtOrdSignDate = value; }
        }

        public bool? CourtOrdRecvdId
        {
            get { return courtOrdRecvdId; }
            set { courtOrdRecvdId = value; }
        }
        public bool? CourtOrdSignedId
        {
            get { return courtOrdSignedId; }
            set { courtOrdSignedId = value; }
        }
        public string GuarFirstName
        {
            get { return guarFirstName; }
            set { guarFirstName = value; }
        }
        public string GuarLastName
        {
            get { return guarLastName; }
            set { guarLastName = value; }
        }
        public string GuarAddress1
        {
            get { return guarAddress1; }
            set { guarAddress1 = value; }
        }
        public string GuarAddress2
        {
            get { return guarAddress2; }
            set { guarAddress2 = value; }
        }
        public string GuarCity
        {
            get { return guarCity; }
            set { guarCity = value; }
        }
        public int GuarStateId
        {
            get { return guarStateId; }
            set { guarStateId = value; }
        }
        public string GuarZip
        {
            get { return guarZip; }
            set { guarZip = value; }
        }
        public string GuarSSNum
        {
            get { return guarSSNum; }
            set { guarSSNum = value; }
        }
        public DateTime? GuarDOB
        {

            get { return guarDOB; }
            set { guarDOB = value; }
        }
        public bool? GuarCounsId
        {
            get { return guarCounsId; }
            set { guarCounsId = value; }
        }

        public string PartialHMDAInfoStmnt
        {
            get { return partialHMDAInfoStmnt; }
            set { partialHMDAInfoStmnt = value; }
        }
        public bool HDMADidNotFurnish
        {
            get
            {
                return
                    ((hdmaHideId == (int)HDMAHide.SomeInformationProvided) ||
                     (hdmaHideId == (int)HDMAHide.NoneInformationProvided));
            }
            
        }
        public decimal? OtherRealEstateAssets
        {
            get { return otherRealEstateAssets; }
            set { otherRealEstateAssets = value; }
        }
        public bool StateIssuedDriversLicenseYN
        {
            get
            {
                bool res = false;
                if(identificationDocTypeId==(int)IdentificationDocCategory.Primary)
                {
                    res = PrimaryIdentificationDocument.DocTypeId == (int) IdentificationDocumentType.StateIssuedDriversLicense;
                }
                return res;
            }
        }
        public bool StateIssuedIDCardYN
        {
            get
            {
                bool res = false;
                if(identificationDocTypeId==(int)IdentificationDocCategory.Primary)
                {
                    res = PrimaryIdentificationDocument.DocTypeId == (int) IdentificationDocumentType.StateIssuedIDCard;
                }
                return res;
            }
        }
        public bool MilitaryIDCardYN
        {
            get
            {
                bool res = false;
                if(identificationDocTypeId==(int)IdentificationDocCategory.Primary)
                {
                    res = PrimaryIdentificationDocument.DocTypeId == (int) IdentificationDocumentType.MilitaryIDCard;
                }
                return res;
            }
        }
        public bool PassportYN
        {
            get
            {
                bool res = false;
                if(identificationDocTypeId==(int)IdentificationDocCategory.Primary)
                {
                    res = PrimaryIdentificationDocument.DocTypeId == (int) IdentificationDocumentType.Passport;
                }
                return res;
            }
        }
        public bool USAlienRegistrationCardYN
        {
            get
            {
                bool res = false;
                if(identificationDocTypeId==(int)IdentificationDocCategory.Primary)
                {
                    res = PrimaryIdentificationDocument.DocTypeId == (int) IdentificationDocumentType.USAlienRegistrationCard;
                }
                return res;
            }
        }
        public bool CanadianDriversLicenseYN
        {
            get
            {
                bool res = false;
                if(identificationDocTypeId==(int)IdentificationDocCategory.Primary)
                {
                    res = PrimaryIdentificationDocument.DocTypeId == (int) IdentificationDocumentType.CanadianDriversLicense;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc1SocialSecurityCardYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument1.DocTypeId == (int)IdentificationDocumentType.SocialSecurityCard;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc1ForeignCountryVisaYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument1.DocTypeId == (int)IdentificationDocumentType.ForeignCountryVisa;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc1InsuranceCardYN	
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument1.DocTypeId == (int)IdentificationDocumentType.InsuranceCard;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc1BirthCertificateYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument1.DocTypeId == (int)IdentificationDocumentType.BirthCertificate;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc1NonUSCanadianDriversLicenseYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument1.DocTypeId == (int)IdentificationDocumentType.NonUSCanadianDriversLicense;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc1PropertyTaxBillYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument1.DocTypeId == (int)IdentificationDocumentType.PropertyTaxBill;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc1RecentUtilityBillYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument1.DocTypeId == (int)IdentificationDocumentType.RecentUtilityBill;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc1VoterRegistrationCardYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument1.DocTypeId == (int)IdentificationDocumentType.VoterRegistrationCard;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc1OrganizationalMembershipCardYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument1.DocTypeId == (int)IdentificationDocumentType.OrganizationalMembershipCard;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc1StudentIdentificationCardYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument1.DocTypeId == (int)IdentificationDocumentType.StudentIdentificationCard;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc1BankInvestmentLoanStatementsYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument1.DocTypeId == (int)IdentificationDocumentType.BankInvestmentLoanStatements;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc1HomeCarRenterInsurancePapersYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument1.DocTypeId == (int)IdentificationDocumentType.Homecarrenterinsurancepapers;
                }
                return res;
            }
        }

        public bool SecondaryIDDoc2SocialSecurityCardYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument2.DocTypeId == (int)IdentificationDocumentType.SocialSecurityCard;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc2ForeignCountryVisaYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument2.DocTypeId == (int)IdentificationDocumentType.ForeignCountryVisa;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc2InsuranceCardYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument2.DocTypeId == (int)IdentificationDocumentType.InsuranceCard;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc2BirthCertificateYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument2.DocTypeId == (int)IdentificationDocumentType.BirthCertificate;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc2NonUSCanadianDriversLicenseYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument2.DocTypeId == (int)IdentificationDocumentType.NonUSCanadianDriversLicense;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc2PropertyTaxBillYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument2.DocTypeId == (int)IdentificationDocumentType.PropertyTaxBill;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc2RecentUtilityBillYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument2.DocTypeId == (int)IdentificationDocumentType.RecentUtilityBill;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc2VoterRegistrationCardYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument2.DocTypeId == (int)IdentificationDocumentType.VoterRegistrationCard;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc2OrganizationalMembershipCardYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument2.DocTypeId == (int)IdentificationDocumentType.OrganizationalMembershipCard;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc2StudentIdentificationCardYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument2.DocTypeId == (int)IdentificationDocumentType.StudentIdentificationCard;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc2BankInvestmentLoanStatementsYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument2.DocTypeId == (int)IdentificationDocumentType.BankInvestmentLoanStatements;
                }
                return res;
            }
        }
        public bool SecondaryIDDoc2HomeCarRenterInsurancePapersYN
        {
            get
            {
                bool res = false;
                if (identificationDocTypeId == (int)IdentificationDocCategory.Secondary)
                {
                    res = SecondaryIdentificationDocument2.DocTypeId == (int)IdentificationDocumentType.Homecarrenterinsurancepapers;
                }
                return res;
            }
        }
        public int CounselingMethodId
        {
            get { return counselingMethodId; }
            set { counselingMethodId = value; }
        }
        public bool? CertifyRaceVisualObserId
        {
            get { return certifyRaceVisualObserId; }
            set { certifyRaceVisualObserId = value; }
        }
        public bool? CounseledId
        {
            get { return counseledId; }
            set { counseledId = value; }
        }
        public bool? InclOnCounsCertId
        {
            get { return inclOnCounsCertId; }
            set { inclOnCounsCertId = value; }
        }
        public DataView DvIdentificationDocuments
        {
            get
            {
                if (dvIdentificationDocuments == null)
                {
                    dvIdentificationDocuments = GetIdentificationDocuments();
                }
                return dvIdentificationDocuments;
            }
        }
        public BorrowerIdentificationDocument PrimaryIdentificationDocument
        {
            get
            {
                if(primaryIdentificationDocument==null)
                {
                    primaryIdentificationDocument = GetIdentificationDocument(PRIMARYIDENTIFICATIONID,0);
                }
                return primaryIdentificationDocument;
            }
        }
        public BorrowerIdentificationDocument SecondaryIdentificationDocument1
        {
            get
            {
                if(secondaryIdentificationDocument1==null)
                {
                    secondaryIdentificationDocument1 = GetIdentificationDocument(SECONDARYIDENTIFICATIONID,0);
                }
                return secondaryIdentificationDocument1;
            }
        }
        public BorrowerIdentificationDocument SecondaryIdentificationDocument2
        {
            get
            {
                if(secondaryIdentificationDocument2==null)
                {
                    secondaryIdentificationDocument2 = GetIdentificationDocument(SECONDARYIDENTIFICATIONID,1);
                }
                return secondaryIdentificationDocument2;
            }
        }
        public int IdentificationDocTypeId
        {
            get { return identificationDocTypeId; }
            set { identificationDocTypeId = value; }
        }
        [DbMapping("PrimaryIdentificationDocument", "BorrowerIdentificationDocument", "DocTypeId")]
        public int PrimaryIdentificationDocId
        {
            get { return PrimaryIdentificationDocument.DocTypeId;}
            set { PrimaryIdentificationDocument.DocTypeId = value; }
        }
        [DbMapping("PrimaryIdentificationDocument", "BorrowerIdentificationDocument", "OriginName")]
        public string IdentificationPrimaryDocOriginName
        {
            get { return PrimaryIdentificationDocument.OriginName; }
            set { PrimaryIdentificationDocument.OriginName = value; }
        }
        [DbMapping("PrimaryIdentificationDocument", "BorrowerIdentificationDocument", "IDNumber")]
        public string IdentificationPrimaryDocIDNumber
        {
            get { return PrimaryIdentificationDocument.IDNumber; }
            set { PrimaryIdentificationDocument.IDNumber = value; }
        }
        [DbMapping("PrimaryIdentificationDocument", "BorrowerIdentificationDocument", "IssuanceDate")]
        public DateTime? IdentificationPrimaryDocIssuanceDate
        {
            get { return PrimaryIdentificationDocument.IssuanceDate; }
            set { PrimaryIdentificationDocument.IssuanceDate = value; }
        }
        [DbMapping("PrimaryIdentificationDocument", "BorrowerIdentificationDocument", "ExpirationDate")]
        public DateTime? IdentificationPrimaryDocExpirationDate
        {
            get { return PrimaryIdentificationDocument.ExpirationDate; }
            set { PrimaryIdentificationDocument.ExpirationDate = value; }
        }
        [DbMapping("SecondaryIdentificationDocument1", "BorrowerIdentificationDocument", "DocTypeId")]
        public int SecondaryIdentificationDoc1Id
        {
            get { return SecondaryIdentificationDocument1.DocTypeId; }
            set { SecondaryIdentificationDocument1.DocTypeId = value; }
        }
        [DbMapping("SecondaryIdentificationDocument1", "BorrowerIdentificationDocument", "OriginName")]
        public string IdentificationSecondaryDoc1OriginName
        {
            get { return SecondaryIdentificationDocument1.OriginName; }
            set { SecondaryIdentificationDocument1.OriginName = value; }
        }
        [DbMapping("SecondaryIdentificationDocument1", "BorrowerIdentificationDocument", "IDNumber")]
        public string IdentificationSecondaryDoc1IDNumber
        {
            get { return SecondaryIdentificationDocument1.IDNumber; }
            set { SecondaryIdentificationDocument1.IDNumber = value; }
        }
        [DbMapping("SecondaryIdentificationDocument1", "BorrowerIdentificationDocument", "IssuanceDate")]
        public DateTime? IdentificationSecondaryDoc1IssuanceDate
        {
            get { return SecondaryIdentificationDocument1.IssuanceDate; }
            set { SecondaryIdentificationDocument1.IssuanceDate = value; }
        }
        [DbMapping("SecondaryIdentificationDocument1", "BorrowerIdentificationDocument", "ExpirationDate")]
        public DateTime? IdentificationSecondaryDoc1ExpirationDate
        {
            get { return SecondaryIdentificationDocument1.ExpirationDate; }
            set { SecondaryIdentificationDocument1.ExpirationDate = value; }
        }
        [DbMapping("SecondaryIdentificationDocument2", "BorrowerIdentificationDocument", "DocTypeId")]
        public int SecondaryIdentificationDoc2Id
        {
            get { return SecondaryIdentificationDocument2.DocTypeId; }
            set { SecondaryIdentificationDocument2.DocTypeId = value; }
        }
        [DbMapping("SecondaryIdentificationDocument2", "BorrowerIdentificationDocument", "OriginName")]
        public string IdentificationSecondaryDoc2OriginName
        {
            get { return SecondaryIdentificationDocument2.OriginName; }
            set { SecondaryIdentificationDocument2.OriginName = value; }
        }
        [DbMapping("SecondaryIdentificationDocument2", "BorrowerIdentificationDocument", "IDNumber")]
        public string IdentificationSecondaryDoc2IDNumber
        {
            get { return SecondaryIdentificationDocument2.IDNumber; }
            set { SecondaryIdentificationDocument2.IDNumber = value; }
        }
        [DbMapping("SecondaryIdentificationDocument2", "BorrowerIdentificationDocument", "IssuanceDate")]
        public DateTime? IdentificationSecondaryDoc2IssuanceDate
        {
            get { return SecondaryIdentificationDocument2.IssuanceDate; }
            set { SecondaryIdentificationDocument2.IssuanceDate = value; }
        }
        [DbMapping("SecondaryIdentificationDocument2", "BorrowerIdentificationDocument", "ExpirationDate")]
        public DateTime? IdentificationSecondaryDoc2ExpirationDate
        {
            get { return SecondaryIdentificationDocument2.ExpirationDate; }
            set { SecondaryIdentificationDocument2.ExpirationDate = value; }
        }
        public int? LifeExpectancy
        {
            get
            {
                if (ActualAge == null || HUDLifeExpectancyHash.Count == 0)
                    return null;

                int age = (int)ActualAge;
                if (!HUDLifeExpectancyHash.ContainsKey(age))
                {
                    IList ageList = HUDLifeExpectancyHash.GetKeyList();
                    int minAge = Convert.ToInt32(ageList[0]);
                    int maxAge = Convert.ToInt32(ageList[ageList.Count - 1]);
                    if (age < minAge)
                        age = minAge;
                    else if (age > maxAge)
                        age = maxAge;
                }

                return Convert.ToInt32(HUDLifeExpectancyHash[age]);
            }
        }
        //public int? LifeExpectancy
        //{
        //    get
        //    {
        //        if (NearestAge == null || HUDLifeExpectancyHash.Count == 0)
        //            return null;

        //        int age = (int)NearestAge;
        //        if (!HUDLifeExpectancyHash.ContainsKey(age))
        //        {
        //            IList ageList = HUDLifeExpectancyHash.GetKeyList();
        //            int minAge = Convert.ToInt32(ageList[0]);
        //            int maxAge = Convert.ToInt32(ageList[ageList.Count - 1]);
        //            if (age < minAge)
        //                age = minAge;
        //            else if (age > maxAge)
        //                age = maxAge;
        //        }

        //        return Convert.ToInt32(HUDLifeExpectancyHash[age]);
        //    }
        //}
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
            }
        }
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
            }
        }
        public string FullName
        {
            get
            {
                if (String.IsNullOrEmpty(firstName) && String.IsNullOrEmpty(lastName))
                    return String.Empty;

                return String.Format("{0}, {1}", lastName, firstName);
            }
        }
        public string MiddleInitial
        {
            get { return middleInitial; }
            set { middleInitial = value; }
        }
        public DateTime? DateOfBirth
        {
            get
            {
                return dateOfBirth;
            }
            set
            {
                dateOfBirth = value;
            }
        }
        public string Address
        {
            get { return address1 + " " + address2; }
        }
        public string Address1
        {
            get
            {
                return address1;
            }
            set
            {
                address1 = value;
            }
        }
        public string Address2
        {
            get
            {
                return address2;
            }
            set
            {
                address2 = value;
            }
        }
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }
        public int StateID
        {
            get
            {
                return stateID;
            }
            set
            {
                stateID = value;
            }
        }
        public string Zip
        {
            get
            {
                return zip;
            }
            set
            {
                zip = value;
            }
        }
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                phone = value;
            }
        }
        public int SexId
        {
            get
            {
                return sexId;
            }
            set
            {
                sexId = value;
            }
        }
        public bool SexFemaleYN
        {
            get { return SexId==(int)Sex.Female;}
        }
        public bool SexMaleYN
        {
            get { return SexId == (int)Sex.Male; }
        }
        public string SSN
        {
            get
            {
                return ssn;
            }
            set
            {
                ssn = value;
            }
        }
        public int SalutationId
        {
            get { return salutationId; }
            set { salutationId = value; }
        }
        public int MartialStatusId
        {
            get { return martialStatusId; }
            set { martialStatusId = value; }
        }
        public bool MartialStatusMarriedYN
        {
            get { return martialStatusId == (int)MartialStatus.Married; }
        }
        public bool MartialStatusUnmarriedYN
        {
            get { return martialStatusId == (int)MartialStatus.Unmaried; }
        }
        public string AKA1
        {
            get { return aka1; }
            set { aka1 = value; }
        }
        public string AKA2
        {
            get { return aka2; }
            set { aka2 = value; }
        }
        public string AKA3
        {
            get { return aka3; }
            set { aka3 = value; }
        }
        public string AKA4
        {
            get { return aka4; }
            set { aka4 = value; }
        }
        public int? ActualAge
        {
            get
            {
                if(dateOfBirth == null)
                {
                    return null;
                }
                DateTime dob = (DateTime) dateOfBirth;
                int res;
                res = DateTime.Now.Year - dob.Year;
                if (dob.AddYears(res) > DateTime.Now) res--;
                return res;
            }
        }
        public int? NearestAge
        {
            get
            {
                if (dateOfBirth == null)
                {
                    return null;
                }
                int res;
                DateTime dob = (DateTime)dateOfBirth;
                res = DateTime.Now.Year - dob.Year;
                DateTime nextBirthDate = dob.AddYears(res);
                if (DateTime.Now.AddDays(90) > nextBirthDate)
                {
                    res++;
                    if (dob.AddYears(res) > DateTime.Now) res--;
                }
                return res;
            }
        }
        public int? YearsAtPresentAddress
        {
            get { return yearsAtPresentAddress; }
            set { yearsAtPresentAddress = value; }
        }
        public decimal? MonthlyIncome
        {
            get { return monthlyIncome; }
            set { monthlyIncome = value; }
        }
        public decimal? RealEstateAssets
        {
            get { return realEstateAssets; }
            set { realEstateAssets = value; }
        }
        public decimal? AvailableAssets
        {
            get { return availableAssets; }
            set { availableAssets = value; }
        }
        public bool? DifferentMailingAddressId
        {
            get { return differentMailingAddressId; }
            set { differentMailingAddressId = value; }
        }
        public bool? UsePOAId
        {
            get { return usePOAId; }
            set { usePOAId = value; }
        }
        public bool? DecJudments
        {
            get { return decJudments; }
            set { decJudments = value; }
        }
        public bool DecJudmentsYes
        {
            get 
            {
                bool res = false;
                if (decJudments!=null)
                {
                    res = (bool) decJudments;
                }
                return res; 
            }
        }
        public bool DecJudmentsNo
        {
            get
            {
                bool res = false;
                if (decJudments != null)
                {
                    res = !(bool)decJudments;
                }
                return res; 
            }
        }
        public bool? DecBuncruptcy
        {
            get { return decBuncruptcy; }
            set { decBuncruptcy = value; }
        }
        public bool DecBuncruptcyYes
        {
            get
            {
                bool res = false;
                if (decBuncruptcy != null)
                {
                    res = (bool)decBuncruptcy;
                }
                return res; 
            }
        }
        public bool DecBuncruptcyNo
        {
            get
            {
                bool res = false;
                if (decBuncruptcy != null)
                {
                    res = !(bool)decBuncruptcy;
                }
                return res; 
            }
        }
        public bool? DecLawsuit
        {
            get { return decLawsuit; }
            set { decLawsuit = value; }
        }
        public bool DecLawsuitYes
        {
            get
            {
                bool res = false;
                if (decLawsuit != null)
                {
                    res = (bool)decLawsuit;
                }
                return res; 
            }
        }
        public bool DecLawsuitNo
        {
            get
            {
                bool res = false;
                if (decLawsuit != null)
                {
                    res = !(bool)decLawsuit;
                }
                return res;
            }
        }
        public bool? DecFedDebt
        {
            get { return decFedDebt; }
            set { decFedDebt = value; }
        }
        public bool DecFedDebtYes
        {
            get
            {
                bool res = false;
                if (decFedDebt != null)
                {
                    res = (bool)decFedDebt;
                }
                return res;
            }
        }
        public bool DecFedDebtNo
        {
            get
            {
                bool res = false;
                if (decFedDebt != null)
                {
                    res = !(bool)decFedDebt;
                }
                return res;
            }
        }
        public bool? DecPrimaryres
        {
            get { return decPrimaryres; }
            set { decPrimaryres = value; }
        }
        public bool DecPrimaryresYes
        {
            get
            {
                bool res = false;
                if (decPrimaryres != null)
                {
                    res = (bool)decPrimaryres;
                }
                return res;
            }
        }
        public bool DecPrimaryresNo
        {
            get
            {
                bool res = false;
                if (decPrimaryres != null)
                {
                    res = !(bool)decPrimaryres;
                }
                return res;
            }
        }
        public bool? DecEndorser
        {
            get { return decEndorser; }
            set { decEndorser = value; }
        }
        public bool DecEndorserYes
        {
            get { return decEndorser != null && decEndorser == true; }
        }
        public bool DecEndorserNo
        {
            get { return decEndorser != null && decEndorser == false; }
        }
        public bool? DecUSCitizen
        {
            get { return decUSCitizen; }
            set { decUSCitizen = value; }
        }
        public bool DecUSCitizenYes
        {
            get { return decUSCitizen != null && decUSCitizen == true; }
        }
        public bool DecUSCitizenNo
        {
            get { return decUSCitizen != null && decUSCitizen == false; }
        }
        public bool? DecPermanentRes
        {
            get { return decPermanentRes; }
            set { decPermanentRes = value; }
        }
        public bool DecPermanentResYes
        {
            get { return decPermanentRes != null && decPermanentRes == true; }
        }
        public bool DecPermanentResNo
        {
            get { return decPermanentRes != null && decPermanentRes == false; }
        }
        public int HDMAHideId
        {
            get { return hdmaHideId; }
            set { hdmaHideId = value; }
        }
        public string HDMARace
        {
            get { return hdmaRace; }
            set { hdmaRace = value; }
        }
        public int HDMARaceId
        {
            get { return hdmaRaceId; }
            set { hdmaRaceId = value; }
        }
        public bool RaceAmericanIndianYN
        {
            get { return hdmaRaceId == (int)Race.AmericanIndianORAlaskaNative; }
        }
        public bool RaceAsianYN
        {
            get { return hdmaRaceId == (int)Race.Asian; }
        }
        public bool RaceAfricanAmericanYN
        {
            get { return hdmaRaceId == (int)Race.BlackORAfricanAmerican; }
        }
        public bool RaceHawaiianYN
        {
            get { return hdmaRaceId == (int)Race.NativeHawaiianOROtherPacificIslander; }
        }
        public bool RaceWhiteYN
        {
            get { return hdmaRaceId == (int)Race.White; }
        }
        public string HDMAEthnicity
        {
            get { return hdmaEthnicity; }
            set { hdmaEthnicity = value; }
        }
        public int HDMAEthnicityId
        {
            get { return hdmaEthnicityId; }
            set { hdmaEthnicityId = value; }
        }
        public bool HMDAEthnicityHispanicYN
        {
            get { return (hdmaEthnicityId == (int)Ethnicity.Hispanic); }
        }
        public bool HMDAEthnicityNotHispanicYN
        {
            get { return (hdmaEthnicityId == (int)Ethnicity.NotHispanic); }
        }
        public bool? PoaDurableId
        {
            get { return poaDurableId; }
            set { poaDurableId = value; }
        }
        public bool? PoaEncumberingId
        {
            get { return poaEncumberingId; }
            set { poaEncumberingId = value; }
        }
        public bool? PoaRevocableId
        {
            get { return poaRevocableId; }
            set { poaRevocableId = value; }
        }
        public bool? PoaIncapacitatedId
        {
            get { return poaIncapacitatedId; }
            set { poaIncapacitatedId = value; }
        }
        public DateTime? PoaExecutionDate
        {
            get { return poaExecutionDate; }
            set { poaExecutionDate = value; }
        }
        public int POALegalCapacityId
        {
            get { return poaLegalCapacityId; }
            set { poaLegalCapacityId = value; }
        }
        public bool? POAReviewedDocumentId
        {
            get { return poaReviewedDocumentId; }
            set { poaReviewedDocumentId = value; }
        }
        public bool? POATransactionApproveId
        {
            get { return poaTransactionApproveId; }
            set { poaTransactionApproveId = value; }
        }
        public bool? POASignedByJudgeId
        {
            get { return poaSignedByJudgeId; }
            set { poaSignedByJudgeId = value; }
        }
        public bool? POAMentionedInterestRateId
        {
            get { return poaMentionedInterestRateId; }
            set { poaMentionedInterestRateId = value; }
        }
        public bool? POAMentionedAmountId
        {
            get { return poaMentionedAmountId; }
            set { poaMentionedAmountId = value; }
        }
        public bool? POAMentionedByNameId
        {
            get { return poaMentionedByNameId; }
            set { poaMentionedByNameId = value; }
        }
        public bool? POANotedInitialRateId
        {
            get { return poaNotedInitialRateId; }
            set { poaNotedInitialRateId = value; }
        }
        public bool? POAEqualOrGreaterMaximumId
        {
            get { return poaEqualOrGreaterMaximumId; }
            set { poaEqualOrGreaterMaximumId = value; }
        }
        public bool Archived
        {
            get
            {
                return archived;
            }
            set
            {
                archived = value;
            }
        }
        public Hashtable ErrMessages
        {
            get
            {
                if (errMessages == null)
                {
                    errMessages = MortgageProfileField.GetErrorMessages("Borrowers");
                }
                return errMessages;
            }
        }
        public string AltContactName
        {
            get { return altContactName; }
            set { altContactName = value; }
        }
        public string AltContactRelationship
        {
            get { return altContactRelationship; }
            set { altContactRelationship = value; }
        }
        public string AltContactAddress1
        {
            get { return altContactAddress1; }
            set { altContactAddress1 = value; }
        }
        public string AltContactAddress2
        {
            get { return altContactAddress2; }
            set { altContactAddress2 = value; }
        }
        public string AltContactAddress
        {
            get { return altContactAddress1 + " " + altContactAddress2; }
        }
        public string AltContactCity
        {
            get { return altContactCity; }
            set { altContactCity = value; }
        }
        public string AltContactZip
        {
            get { return altContactZip; }
            set { altContactZip = value; }
        }
        public string AltContactAltPhone
        {
            get { return altContactAltPhone; }
            set { altContactAltPhone = value; }
        }
        public string AltContactPhone
        {
            get { return altContactPhone; }
            set { altContactPhone = value; }
        }
        public int AltContactStateId
        {
            get { return altContactStateId; }
            set { altContactStateId = value; }
        }
        public decimal? TotalAmountOfNonREDebts
        {
            get { return totalAmountOfNonREDebts; }
            set { totalAmountOfNonREDebts = value; }
        }
        public bool? POARequested
        {
            get { return poaRequested; }
            set { poaRequested = value; }
        }
        public bool? POAReceived
        {
            get { return poaReceived; }
            set { poaReceived = value; }
        }
        public bool? ConservatorRequested
        {
            get { return conservatorRequested; }
            set { conservatorRequested = value; }
        }
        public bool? ProofOfAgeRequested
        {
            get { return proofOfAgeRequested; }
            set { proofOfAgeRequested = value; }
        }
        public bool? ProofOfAgeCollected
        {
            get { return proofOfAgeCollected; }
            set { proofOfAgeCollected = value; }
        }
        public bool? ProofOfSSNRequested
        {
            get { return proofOfSSNRequested; }
            set { proofOfSSNRequested = value; }
        }
        public bool? ProofOfSSNCollected
        {
            get { return proofOfSSNCollected; }
            set { proofOfSSNCollected = value; }
        }
        public string Guardian
        {
            get { return guarFirstName + " " + guarLastName; }
        }
        #endregion

        #region contsructor
        public Borrower(MortgageProfile _mp)
	    {
            mp = _mp;
	    }
        public Borrower(DataRowView dr, MortgageProfile _mp)
        {
            mp = _mp;
            PopulateFromDataRow(dr);
        }
        public Borrower()
        {
        }
        #endregion

        #region methods

        #region public
        public void Archive()
        {
            db.ExecuteScalar("ArchiveUnarchiveBorrower",
                                    ID,
                                    true);
        }
        public void UnArchive()
        {
            db.ExecuteScalar("ArchiveUnarchiveBorrower",
                                    ID,
                                    false);
        }
        public override int Save(MortgageProfile mp_, string objectName,string fullPropertyName, string propertyName, int objectTypeId, object val, object oldVal,string parentFieldName, int parentId, bool isRequired)
        {
            return base.Save(mp_, BORROWERTABLE, fullPropertyName, propertyName, objectTypeId, val, oldVal, parentFieldName, parentId, isRequired);
        }
        public override bool ValidateProperty(string propertyName, out string err)
        {
            bool res = true;
            err = String.Empty;
            switch (propertyName)
            {
                case "FirstName":
                    if (String.IsNullOrEmpty(firstName))
                    {
                        res = false;
                        GetError(propertyName, out err);
                    }
                    break;
                case "LastName":
                    if (String.IsNullOrEmpty(lastName))
                    {
                        res = false;
                        GetError(propertyName, out err);
                    }
                    break;
                case "Zip":
                    if (!String.IsNullOrEmpty(zip))
                    {
                        res = MortgageProfileField.ValidateRegexp(zip, ZIPREGEXP);
                        if (!res)
                        {
                            GetError(propertyName, out err);
                        }
                    }
                    break;
                case "Phone":
                    if (!String.IsNullOrEmpty(phone))
                    {
                        res = MortgageProfileField.ValidateRegexp(phone, PHONEREGEXP);
                        if (!res)
                        {
                            GetError(propertyName, out err);
                        }
                    }
                    break;
                case "SSN":
                    if(!String.IsNullOrEmpty(ssn))
                    {
                        res = MortgageProfileField.ValidateRegexp(ssn, SSNREGEXP);
                        if (!res)
                        {
                            GetError(propertyName, out err);
                        }
                    }
                    break;
                case "YearsAtPresentAddress":
                    if(yearsAtPresentAddress!=null)
                    {
                        res = !((yearsAtPresentAddress <= 0) || (yearsAtPresentAddress > ActualAge));
                        if (!res)
                        {
                            GetError(propertyName, out err);
                        }
                    }
                    break;
                case "MonthlyIncome":
                    if(monthlyIncome!=null)
                    {
                        res = monthlyIncome >= 0;
                        if (!res)
                        {
                            GetError(propertyName, out err);
                        }
                    }
                    break;
                case "RealEstateAssets":
                    if(realEstateAssets!=null)
                    {
                        res = realEstateAssets >= 0;
                        if (!res)
                        {
                            GetError(propertyName, out err);
                        }
                    }
                    break;
                case "AvailableAssets":
                    if(availableAssets!=null)
                    {
                        res = availableAssets >= 0;
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
        private DataView GetIdentificationDocuments()
        {
            return db.GetDataView(GETIDENTIFICATIONDOCUMENTS, ID);
        }

        private BorrowerIdentificationDocument GetIdentificationDocument(int categoryId,int index)
        {
            DataView dv = DvIdentificationDocuments;
            dv.RowFilter = String.Format(CATEGORYFILETER, categoryId);
            BorrowerIdentificationDocument res;
            if(index < dv.Count)
            {
                res = new BorrowerIdentificationDocument(dv[index]);
            }
            else
            {
                res = new BorrowerIdentificationDocument();
            }
            return res;
        }

        private List<BorrowerIdentificationDocument> GetIdentificationDocumentList()
        {
            DataView docTypeView = mp.CurrentPage.GetDictionary("BorrowerIdentificationDocType");
            docTypeView.Sort = "id";

            BorrowerIdentificationDocument primeIdentDoc = PrimaryIdentificationDocument;
            BorrowerIdentificationDocument secIdentDoc1 = SecondaryIdentificationDocument1;
            BorrowerIdentificationDocument secIdentDoc2 = SecondaryIdentificationDocument2;

            List<BorrowerIdentificationDocument> identDocList = new List<BorrowerIdentificationDocument>();
            foreach (DataRowView docTypeRow in docTypeView)
            {
                int docTypeID = Convert.ToInt32(docTypeRow["id"]);
                string docName = docTypeRow["Name"].ToString();
                if (primeIdentDoc.DocTypeId == docTypeID)
                    identDocList.Add(primeIdentDoc);
                else if (secIdentDoc1.DocTypeId == docTypeID)
                    identDocList.Add(secIdentDoc1);
                else if (secIdentDoc2.DocTypeId == docTypeID)
                    identDocList.Add(secIdentDoc2);
                else
                    identDocList.Add(new BorrowerIdentificationDocument(docTypeID, docName));
            }

            return identDocList;
        }
        #endregion

        #endregion
    }

    public class BorrowerIdentificationDocument :BaseObject
    {
        #region fields
        private int categoryId = 0;
        private int docTypeId = 0;
        private string docName = String.Empty;
        private int originId = 0;
        private string originName = String.Empty;
        private string idNumber = String.Empty;
        private DateTime? issuanceDate = null;
        private DateTime? expirationDate = null;
        #endregion

        #region properties
        public string DocName
        {
            get { return docName; }
            set { docName = value; }
        }
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }
        public int DocTypeId
        {
            get { return docTypeId; }
            set { docTypeId = value; }
        }
        public int OriginId
        {
            get { return originId; }
            set { originId = value; }
        }
        public string OriginName
        {
            get { return originName; }
            set { originName = value; }
        }
        public string IDNumber
        {
            get { return idNumber; }
            set { idNumber = value; }
        }
        public DateTime? IssuanceDate
        {
            get { return issuanceDate; }
            set { issuanceDate = value; }
        }
        public DateTime? ExpirationDate
        {
            get { return expirationDate; }
            set { expirationDate = value; }
        }
        #endregion

        #region constructor
        public BorrowerIdentificationDocument()
        {
        }
        public BorrowerIdentificationDocument(int _docTypeID, string _docName)
        {
            docTypeId = _docTypeID;
            docName = _docName;
        }
        public BorrowerIdentificationDocument(DataRowView dr)
        {
            if(dr!=null)
            {
                PopulateFromDataRow(dr);
            }
        }
        #endregion
    }
    public class BorrowerAkaName : BaseObject
    { 
        #region constants
        private const string GETBORROWERAKANAMEBYID = "GetBorrowerAkaNameById";
        private const string SAVE = "SaveBorrowerAkaName";
        private const string DELETE = "DeleteBorrowerAkaName";
        #endregion

        #region fields
        private int borrowerId;
        private string name;
        #endregion

        #region properties
        public int BorrowerId
        {
            get { return borrowerId; }
            set { borrowerId = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion

        #region constructor
        public BorrowerAkaName(int id)
        {
            ID = id;
            if (id > 0)
            {
                LoadById();
            }
        }
        public BorrowerAkaName(DataRowView dr)
        {
            LoadFromDataRow(dr);
        }
        #endregion

        #region methods
        public static bool Delete(int id)
        {
            return db.ExecuteScalarInt(DELETE, id) == 1;
        }
        public int Save()
        {
            int res = db.ExecuteScalarInt(SAVE, ID, borrowerId, name);
            if (res > 0)
            {
                ID = res;
            }
            return res;
        }
        private void LoadFromDataRow(DataRowView dr)
        {
            ID = int.Parse(dr["id"].ToString());
            borrowerId = int.Parse(dr["borrowerid"].ToString());
            name = dr["name"].ToString();
        }
        private void LoadById()
        {
            DataView dv = db.GetDataView(GETBORROWERAKANAMEBYID, ID);
            if (dv.Count == 1)
            {
                LoadFromDataRow(dv[0]);
            }
            else
            {
                ID = -1;
            }
        }
        #endregion
    }

}
