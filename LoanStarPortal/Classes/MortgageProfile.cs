using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml;

namespace LoanStar.Common
{
    public class MortgageLogEntry
    {
        private const string ITEMELEMENT = "item";

        private readonly string objectName = String.Empty;
        private readonly int objectId = -1;
        private readonly string propertyName = String.Empty;
        private readonly string oldValue = String.Empty;
        private readonly string newValue = String.Empty;
        private readonly int userId = -1;

        public MortgageLogEntry(string objectName, int objectId, string propertyName, string oldValue, string newValue, int userId)
        {
            this.objectName = objectName;
            this.objectId = objectId;
            this.propertyName = propertyName;
            this.oldValue = oldValue;
            this.newValue = newValue;
            this.userId = userId;
        }
        public XmlNode GetLogNode(XmlDocument d)
        {
            XmlNode n = d.CreateElement(ITEMELEMENT);
            XmlAttribute a = d.CreateAttribute("objectname");
            a.Value = objectName;
            n.Attributes.Append(a);
            a = d.CreateAttribute("propertyname");
            a.Value = propertyName;
            n.Attributes.Append(a);
            a = d.CreateAttribute("objectid");
            a.Value = objectId.ToString();
            n.Attributes.Append(a);
            a = d.CreateAttribute("oldvalue");
            a.Value = oldValue;
            n.Attributes.Append(a);
            a = d.CreateAttribute("newvalue");
            a.Value = newValue;
            n.Attributes.Append(a);
            a = d.CreateAttribute("userid");
            a.Value = userId.ToString();
            n.Attributes.Append(a);
            return n;
        }
    }
    public class MortgageArg : EventArgs
    {
        #region Private fields
        private readonly MortgageProfile mortgageProfile = new MortgageProfile();
        private readonly bool isChecked;
        #endregion

        #region Constructors
        public MortgageArg()
        { }
        public MortgageArg(int mpID)
        {
            mortgageProfile.ID = mpID;
        }
        public MortgageArg(MortgageProfile mp, bool _isChecked)
        {
            mortgageProfile = mp;
            isChecked = _isChecked;
        }
        #endregion

        #region Properties
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
        }

        public MortgageProfile MortgageProfile
        {
            get
            {
                return mortgageProfile;
            }
        }

        public int MPID
        {
            get
            {
                return mortgageProfile.ID;
            }
        }
        #endregion
    }
    public class MortgageProfile : BaseObject
    {
        /// <summary>
        /// Any method of MortgageProfile class must throw custom exceptions of this type only
        /// </summary>
        public class MortgageProfileException : BaseObjectException
        {
            public MortgageProfileException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public MortgageProfileException(string message)
                : base(message)
            {
            }
            public MortgageProfileException()
            {
            }
        }

        #region constants

        #region sp names
        private const string GETASSIGNEDUSERS = "GetMPAssignedUser";
        private const string SAVEASSIGNEDUSERS = "SaveMPAssignedUser";
        private const string GETSTATUSLIST = "GetProfileStatusList";
        private const string GETSTATUSLISTFORCHECKLIST = "GetProfileStatusListForCheckList";
        private const string GETCHECKLISTSTATUSLIST = "GetMPCheckListStatusList";
        private const string GETIMPORTANTDATES = "GetMPDates";
        private const string GETMORTGAGEPROFILEBYID = "GetMortgageProfileById";
        private const string GETMORTGAGEPROFILEFIELDSFOROBJECT = "GetMortgageProfileFieldsForObject";
        private const string GETMORTGAGEPROFILEDOCTEMPLATELIST = "GetMPDocTemplateList";
        private const string GETMORTGAGEPROFILECHECKLIST = "GetMPCheckList";
        private const string SAVEMORTGAGEPROFILECHECKLIST = "SaveMPCheckList";
        private const string SAVEMORTGAGEPROFILESTATUS = "SetLoanStatus";
        private const string UPDATEMORTGAGEPROFILEALERTRULES = "UpdateMPAlertRules";
        private const string UPDATEMORTGAGEPROFILETASKRULES = "UpdateMPTaskRules";
        private const string CREATENEWMORTGAGE = "CreateNewMortgage";
        private const string SAVENEWBORROWER = "SaveNewBorrower";
        private const string SAVEMORTGAGEPROFILEINVOICE = "SaveMPInvoiceWithLog";
        private const string SAVEMORTGAGEPROFILERESERVE = "SaveMPReserveWithLog";
        private const string SAVEMORTGAGEPROFILEPAYOFF = "SaveMPPayoffWithLog";
        private const string SAVEMORTGAGEPROFILEPAYOFFSTATUS = "SetPayoffStatusWithLog";
        private const string SAVEMORTGAGEPROFILEADVANCEDPAYMENT = "SaveAdvancedPaymentWithLog";
        private const string SAVEMORTGAGEPROFILEPREPAIDITEM = "SavePrepaidItemWithLog";
        private const string SAVEPROPERTYREPAIRITEM = "SavePropertyRepairItemWithLog";
        private const string GETMORTGAGEBORROWERS = "GetMortgageBorrowers";
        private const string GETROLEFIELDS = "GetMortgageEditableFields";
        private const string GETTABFIELDS = "GetTabFields";
        private const string GETTAB = "GetContentTabs";
        private const string GETMORTGAGEPROFILEFIELDS = "GetMortgageProfilefields";
        private const string UPDATEINVOICEAMOUNT = "UpdateInvoiceAmount";
        private const string UPDATEINVOICEWITHCLOSINGCOST = "UpdateInvoicesWithClosingCosts";
        private const string GETREQUIREDTOCLOSEFEETYPES = "GetRequiredFeeTypes";
        private const string UPDATECHECKLIST = "UpdateMortgageProfileCheckList";
        private const string GETSIGNATURELINES = "GetSignatureLinesForMortgage";
//        private const string GETFIELDSGROUPS = "GetFieldGroups";
//        private const string GETFIELDSGROUPSFORIMPORT = "GetFieldGroupsForImport";
//        private const string GETFIELDSBYGROUPID = "GetFieldsInGroup";
        #endregion

        private const string ROOTELEMENT = "Root";
        private const string UPDATEELEMENT = "update";
        private const string DELETEELEMENT = "delete";
//        private const string ITEMELEMENT = "item";
        private const string IDATTRIBUTE = "id";
        private const string ROLEIDATTRIBUTE = "roleid";
        private const string USERIDATTRIBUTE = "userid";
        private const string ROLEIDFIELDNAME = "roleid";
        private const string USERIDFIELDNAME = "userid";
        private const string PROPERTYNAMEFIELDNAME = "propertyname";
        private const string BORROWER = "Borrower";
        private const string BORROWERFIRSTNAME = "Borrowers.FirstName";
        private const string BORROWERLASTNAME = "Borrowers.LastName";
        private const string INVOICEFILTER = "feecategoryid={0} and typeid={1}";
        private const int MAXPRODUCTCOUNT = 4;

        #region closing cost category
        private const int FEECATEGORYADDITIONALCHARGE = 1;
        public const int FEECATEGORYGOVERMENTCHARGE = 2;
        public const int FEECATEGORYLENDERCHARGE = 3;
        private const int FEECATEGORYTITLECHARGE = 4;
        public const int FEECATEGORYITEMSPAYADVANCE = 5;
        #endregion
        
        #region closing cost fee types
        private const int TYPEADDITIONALSERVEY = 1;
        private const int TYPEADDITIONALPESTINSPECTION = 2;
        public const int TYPEADDITIONALOTHER = 3;
        private const int TYPEGOVERMENTRECORDINGFEES = 4;
        private const int TYPEGOVERMENTRELEASE = 5;
        private const int TYPEGOVERMENTCITYCOUNTYSTAMPS = 6;
        private const int TYPEGOVERMENTSTATETAXSTAMPS = 7;
        public const int TYPEGOVERMENTOTHER = 8;
        public const int TYPELENDERLOANORIGINATIONFEE = 9;
        private const int TYPELENDERAPPRAISALFEE = 10;
        private const int TYPELENDERCREDITREPORT = 11;
        private const int TYPELENDERLENDERINSPECTIONFEE = 12;
        private const int TYPELENDERINSURANCEAPPLICATIONFEE = 13;
        private const int TYPELENDERTAXSERVICEFEE = 14;
        private const int TYPELENDERREPAIRADMINISTARTIONFEE = 15;
        private const int TYPELENDERFLOODCERTIFICATIONFEE = 16;
        private const int TYPELENDERCOMPLIANCECERTIFICATIONFEE = 17;
        private const int TYPELENDERBROKERFEE = 18;
        private const int TYPELENDERCORRESPONDENTFEE = 19;
        private const int TYPELENDERSERVICECOMPANYFEE = 20;
        public const int TYPELENDEROTHER = 21;
        private const int TYPETITLEABSTARCT = 22;
        private const int TYPETITLEATTORNEYFEE = 23;
        private const int TYPETITLEDOCUMENTPREPARATION = 24;
        private const int TYPETITLELENDERCOVERAGE = 25;
        private const int TYPETITLENOTARY = 26;
        public const int TYPETITLEOTHER = 27;
        private const int TYPETITLEOWNERCOVERAGE = 28;
        private const int TYPETITLESETTELMENTFEE = 29;
        private const int TYPETITLEEXAMINATION = 30;
        private const int TYPETITLEINSURANCE = 31;
        private const int TYPEMORTGAGEINSURANCEPREMIUM = 32;
        private const int TYPEHAZARDINSURANCEREMIUM = 33;
        private const int TYPEFLOODINSURANCEREMIUM = 34;
        private const int TYPEITEMSPAYADVANCEOTHER = 35;
        private const int TYPETITLEINSURANCEBINDER = 36;
        private const int TYPETITLEENDORSEMENTS = 37;
        private const int TYPELENDERCURIERFEE = 38;
        #endregion

        public const int LEADSTATUSID = 1;
        public const int APPLICATIONSTATUSID = 3;
        public const int PROCESSINGSTATUSID = 4;
        public const int UNDERWRITINGSTATUSID = 5;
        public const int APPROVEDSTATUSID = 6;
        public const int SUSPENDEDSTATUSID = 7;
        public const int CLEARTOCLOSESTATUSID = 8;
        public const int DOCSOUTSTATUSID = 9;
        public const int RESCISSIONSTATUSID = 10;
        public const int FUNDEDSTATUSID = 11;
        public const int DISBURSEDSTATUSID = 12;
        public const int CLOSEDSTATUSID = 13;
        public const int CANCELLEDSTATUSID = 14;
        public const int WITHDRAWNSTATUSID = 15;
        public const int CLOSEDLEADSTATUSID = 16;
        public const int MANAGEDLEADSTATUSID = 17;


        #endregion

        #region Private fields

        private AdvCalculator[] calculators;
        readonly AppPage currentPage = null;
        private int curProfileStatusID = 1;
        private string statusName = "Lead";
        private readonly DateTime created;
        private Property property;
        private readonly int propertyId;
        private List<Borrower> borrowers;
        private List<Invoice> invoices;
        private List<Payoff> payoffs;
        private List<MortgagePrepaidItem> mortgageprepaids;
        private Hashtable assignedUsers;
        private AppUser currentUser = null;
        private int currentUserId = -1;
        private MortgageInfo mortgageInfo;
        private readonly int mortgageInfoId;
        private Hashtable objectFields;
        private bool payoffUpdateNeeded;
        private bool dayToWorkUpdateNeeded;
        private bool campaignUpdateNeeded;
        private readonly bool needUpdateInCache = false;
        private string lastPostBackField;
        private RuleEvaluationTree ruleEvaluationTree = null;
        private Hashtable ruleObjects;
        private AppUser loanOfficer;
        private AppUser processor;
        private GlobalAdmin globalAdmin = null;
        private int companyID = -1;
        private Lender lender = null;
        private Investor investor = null;
        private Originator originator = null;
        private Hashtable editableFields;
        private DataTable dtInvoice;
        private ArrayList emptyFields;
        private Hashtable mortgageProfileFieldIds;
        private string requiredFieldsData;
        private readonly int interviewerId = 0;
        private Interviewer interviewer = null;
        private decimal liens = 0;
        private bool? isReadytoClose = null;
        private List<TrusteeSignatureLine> trusteesignatureLines;
        private List<UniqueThirdPartyProvider> uniqueThirdPartyProviders;
        private bool calculatorUpdateNeeded = false;
        #endregion

        #region Properties
        public ClosingOriginator ClosingOriginator
        {
            get
            {
                ClosingOriginator  res = new ClosingOriginator();
                if(mortgageInfo.ClosingOriginatorId == mortgageInfo.CompanyId)
                {
                    res.PopulateFromOriginator(originator);
                }
                else if (mortgageInfo.ClosingOriginatorId == mortgageInfo.LenderAffiliateID)
                {
                    res.PopulateFromLender(lender);
                }
                return res;
            }
        }

        public bool CalculatorUpdateNeeded
        {
            get { return calculatorUpdateNeeded; }
            set
            {
                if(calculatorUpdateNeeded!=value)
                {
                    UpdateCalculatorUpdateNeeded(value);
                }
                calculatorUpdateNeeded = value;
            }
        }

        public List<TrusteeSignatureLine> TrusteesignatureLines
        {
            get
            {
                if(trusteesignatureLines==null)
                {
                    trusteesignatureLines = GetSignatureLines();
                }
                return trusteesignatureLines;
            }
        }
        public Trustee Trustee
        {
            get
            {
                return MortgageInfo.Trustee;
            }
        }
        public Servicer Servicer
        {
            get
            {
                return MortgageInfo.Servicer;
            }
        }

        public VendorGlobal ClosingAgent
        {
            get
            {
                return MortgageInfo.ClosingAgent;
            }
        }

        public bool IsReadytoClose
        {
            get 
            {
                if (isReadytoClose == null)
                { 
                    isReadytoClose = CheckClosingCosts();
                }
                return (bool)isReadytoClose;
            }
        }
        public decimal Liens
        {
            get { return liens; }
            set { liens = value; }
        }
        [DbMapping(TableName = "Interviewer")]
        public Interviewer Interviewer
        {
            get
            {
                if(interviewer==null)
                {
                    interviewer = new Interviewer(interviewerId,this);
                }
                return interviewer;
            }
        }
        public AdvCalculator[] AvailableCalculators
        {
            get
            {
                if (calculators != null && MortgageInfo.IsCalculatorValid)
                {
                    return calculators;
                }
                calculators = null;
                AdvCalculator calc = new AdvCalculator(this, Product.GetProductsListForOriginator(companyID, ProductID),false);
                calc.LoadServiceeFee();
                int[] productsOrder = calc.GetProductOrder(MAXPRODUCTCOUNT);
                int numberOfProducts = productsOrder.Length;
                if(ProductID>0)
                {
                    for (int i = 0; i < productsOrder.Length;i++ )
                    {
                        if(productsOrder[i]==ProductID)
                        {
                            numberOfProducts--;
                            break;
                        }
                    }
                }
                if (numberOfProducts > 0)
                {
                    calculators = new AdvCalculator[numberOfProducts];
                    bool needInitialDraw_ = calc.NeedInitialDraw;
                    bool needCreditLine_ = calc.NeedCreditLine;
                    bool needMonthlyIncome_ = calc.NeedMonthlyIncome;
                    bool needTerm_ = calc.NeedTerm;
                    string feeSelection_ = calc.FeeSelection;
                    int k = 0;
                    for (int i = 0; i < productsOrder.Length; i++)
                    {
                        int productId = productsOrder[i];
                        if(productId!=ProductID)
                        {
                            AdvCalculator calc_ = new AdvCalculator(this, needInitialDraw_, needCreditLine_, needMonthlyIncome_, needTerm_, feeSelection_);
                            decimal serviceeFee = 0;
                            if (calc.ServiceeFeeSelection.ContainsKey(productId))
                            {
                                serviceeFee = (decimal)calc.ServiceeFeeSelection[productId];
                            }
                            calc_.CalculateSingleProduct(productId, (double)serviceeFee);
                            calculators[k] = calc_;
                            k++;
                        }
                    }
                }
                return calculators;
            }
        }
        public bool IsCalculatorReady
        {
            get { return MortgageInfo.IsCalculatorReady; }
        }
        public AdvCalculator Calculator
        {
            get { return MortgageInfo.Calculator; }
            set { MortgageInfo.SetCalculator(value); }
        }
        public MortgageBuffer CalculatorData
        {
            get { return MortgageInfo.Calculator.AdvCalculatorBuffer; }
        }
        public AppPage CurrentPage
        {
            get { return currentPage; }
        }
        [DbMapping(TableName = "Originator")]
        public Originator Originator
        {
            get
            {
                if (originator == null || originator.CompanyID != companyID)
                {
                    originator = new Originator(companyID);
                    originator.StateId = Property.StateId;
                }
                return originator;
            }
        }
        [DbMapping(TableName = "Investor")]
        public Investor Investor
        {
            get
            {
                if (investor == null || investor.CompanyID != companyID)
                    investor = new Investor(companyID);

                return investor;
            }
        }
        [DbMapping(TableName = "Product")]
        public Product Product
        {
            get { return MortgageInfo.Product; }
        }
        [DbMapping(TableName = "LenderSpecificField")]
        public Lender Lender
        {
            get
            {
                if (lender == null || lender.CompanyId != MortgageInfo.LenderAffiliateID)
                {
                    lender = new Lender(MortgageInfo.LenderAffiliateID);
                    lender.FHAStateId = Property.StateId;
                }
                return lender;
            }
        }
        public decimal TotalInvoiceNonPOC
        {
            get
            {
                decimal res = 0;
                if((Invoices!=null)&&(Invoices.Count>0))
                {
                    for(int i=0;i<Invoices.Count;i++)
                    {
                        if(!Invoices[i].IsPOC)
                        {
                            res += Invoices[i].Amount;
                        }
                    }
                }
                return res;
            }
        }
        public GlobalAdmin GlobalAdmin
        {
            get
            {
                if (globalAdmin == null)
//                    globalAdmin = new GlobalAdmin(Constants.GID);
                    globalAdmin = new GlobalAdmin();

                return globalAdmin;
            }
        }
        public AppUser Processor
        {
            get
            {
                if (processor == null)
                {
                    int userId = -1;
                    if (AssignedUsers.ContainsKey(AppUser.PROCESSORROLEID))
                    {
                        userId = int.Parse(AssignedUsers[AppUser.PROCESSORROLEID].ToString());
                    }
                    processor = new AppUser(userId);
                }
                return processor;
            }
            
        }
        public AppUser LoanOfficer
        {
            get
            {
                if (loanOfficer == null)
                {
                    int userId = -1;
                    if (AssignedUsers.ContainsKey(AppUser.LOANOFFICERROLEID))
                    {
                        userId = int.Parse(AssignedUsers[AppUser.LOANOFFICERROLEID].ToString());
                    }
                    loanOfficer = new AppUser(userId);
                }
                return loanOfficer;
            }
        }
        public int CurProfileStatusID
        {
            get { return curProfileStatusID; }
            set
            {
                StatusName = GetStatusName(curProfileStatusID);
                curProfileStatusID = value;
            }
        }
        public string StatusName
        {
            get { return statusName; }
            set { statusName = value; }
        }
        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }
        public int ProductID
        {
            get { return MortgageInfo.ProductId; }
        }
        public string ProductName
        {
            get { return MortgageInfo.ProductName; }
        }
        public DateTime Created
        {
            get { return created; }
        }
        [DbMapping(TableName = "Borrower")]
        public List<Borrower> Borrowers
        {
            get
            {
                if (borrowers == null)
                {
                    borrowers = GetBorrowers();
                }
                return borrowers;
            }
        }
        [DbMapping(TableName = "Invoice")]
        public List<Invoice> Invoices
        {
            get
            {
                if (invoices == null)
                {
                    invoices = GetInvoicesWithoutAdvancePaid();
                    if(ProductID>0&& Calculator.UpFrontPremium>0)
                    {
                        invoices.Add(GetUpfrontMipInvoice());
                    }
                }
                return invoices;
            }
        }
        [DbMapping(TableName = "payoff")]
        public List<Payoff> Payoffs
        {
            get
            {
                if (payoffs == null)
                    payoffs = GetPayoffs();

                return payoffs;
            }
        }
        public List<MortgagePrepaidItem> MortgagePrepaids
        {
            get
            {
                if (mortgageprepaids == null)
                {
                    mortgageprepaids = GetMortgagePrePaids();
                }
                return mortgageprepaids;
            }
        }
        [DbMapping(TableName = "MortgageProperty")]
        public Property Property
        {
            get
            {
                if (property == null)
                {
                    property = new Property(propertyId, this);
                }
                return property;
            }
            set
            {
                property = value;
            }
        }
        [DbMapping(TableName = "MortgageInfo")]
        public MortgageInfo MortgageInfo
        {
            get
            {
                if (mortgageInfo == null)
                {
                    mortgageInfo = new MortgageInfo(mortgageInfoId, this);
                }
                return mortgageInfo;
            }
            set
            {
                mortgageInfo = value;
            }
        }
        public Hashtable AssignedUsers
        {
            get
            {
                if (assignedUsers == null)
                {
                    assignedUsers = GetAssignedUsers();
                }
                return assignedUsers;
            }
        }
        public AppUser CurrentUser
        {
            get
            {
                if (currentUser == null || currentUser.Id != CurrentUserId)
                    currentUser = new AppUser(CurrentUserId);

                return currentUser;
            }
        }
        public int CurrentUserId
        {
            get { return currentUserId; }
            set { currentUserId = value; }
        }
        public Borrower YoungestBorrower
        {
            get
            {
                return GetYoungestBorrower();
            }
        }
        public Borrower Borrower1
        {
            get
            {
                if (Borrowers.Count == 0)
                    return null;

                return Borrowers[0];
            }
        }
        public Borrower Borrower2
        {
            get
            {
                if (Borrowers.Count < 2)
                    return null;

                return Borrowers[1];
            }
        }
        public string AllBorrowerNames
        {
            get
            {
                string res = String.Empty;
                foreach (Borrower borrower in Borrowers)
                {
                    if (!String.IsNullOrEmpty(res))
                    {
                        res += ", ";
                    }
                    res += borrower.FirstName + " " + borrower.LastName;
                }
                return res;
            }
        }
        public string SubjectPropertyAddress
        {
            get
            {
                string res = Property.Address1;
                if (!String.IsNullOrEmpty(Property.Address2))
                {
                    res += " " + Property.Address2;
                }
                if (!String.IsNullOrEmpty(Property.City))
                {
                    if (!String.IsNullOrEmpty(res))
                    {
                        res += ", ";
                    }
                    res += Property.City;
                }
                if (!String.IsNullOrEmpty(Property.StateName) && (Property.StateId > 0))
                {
                    if (!String.IsNullOrEmpty(res))
                    {
                        res += ", ";
                    }
                    res += Property.StateName;
                }
                if (!String.IsNullOrEmpty(Property.Zip))
                {
                    res += ", " + Property.Zip;
                }
                return res;
            }
        }
        public bool PayoffUpdateNeeded
        {
            get { return payoffUpdateNeeded; }
            set { payoffUpdateNeeded = value; }
        }
        public bool DayToWorkUpdateNeeded
        {
            get { return dayToWorkUpdateNeeded; }
            set { dayToWorkUpdateNeeded = value; }
        }
        public bool CampaignUpdateNeeded
        {
            get { return campaignUpdateNeeded; }
            set { campaignUpdateNeeded = value; }
        }
        public bool NeedUpdateInCache
        {
            get { return needUpdateInCache; }
        }
        public string LastPostBackField
        {
            get { return lastPostBackField; }
            set { lastPostBackField = value; }
        }
        public Hashtable VisibleFields
        {
            get { return visibleFields; }
        }
        public ArrayList FieldsVisibilityRules
        {
            get
            {
                return null;
            }
        }
        public DataView RuleDependantFields
        {
            get
            {
                return null;
            }
        }
        private Hashtable EditableFields
        {
            get
            {
                if (editableFields == null)
                {
                    editableFields = GetEditableFields();
                }
                return editableFields;
            }

        }
        private Hashtable dataSetFields;
        public Hashtable DataSetFields
        {
            get { return dataSetFields; }
        }
        public Hashtable RuleObjectFields;
        private Hashtable visibleFields;
        private Hashtable requiredFields;
        protected Hashtable RequiredFields
        {
            get
            {
                if(requiredFields==null)
                {
                    requiredFields = GetRequiredFieldLists();
                }
                return requiredFields;
            }
        }
        private Hashtable completedTabs;
        public Hashtable CompletedTabs
        {
            get
            {
                if (completedTabs==null)
                {
                    completedTabs = GetTabRequiredFields();
                }
                return completedTabs;
            }
        }
        public ArrayList EmptyFields
        {
            get
            {
                if(emptyFields==null)
                {
                    emptyFields=GetEmptyFields();
                }
                return emptyFields;
            }
        }
        public Hashtable MortgageProfileFieldIds
        {
            get
            {
                if(mortgageProfileFieldIds==null)
                {
                    mortgageProfileFieldIds = GetFieldsList();
                }
                return mortgageProfileFieldIds;
            }
        }
        #region Calculator fields
        public ProductFlag ProductCalcType
        {
            get { return MortgageInfo.ProductCalcType; }
        }
        public decimal LenderFees
        {
            get
            {
                decimal res = 0;
                for (int i = 0; i < Invoices.Count; i++)
                {
                    if (Invoices[i].FeeCategoryID == FEECATEGORYLENDERCHARGE)
                    {
                        res += Invoices[i].AmountFinanced;
                    }
                }
                return res;
            }            
        }
        public decimal OtherClosingCosts
        {
            get 
            {
                decimal res = 0;
                for (int i = 0; i < Invoices.Count; i++)
                {
                    if (Invoices[i].FeeCategoryID != FEECATEGORYLENDERCHARGE && Invoices[i].ID>0)
                    {
                        res += Invoices[i].AmountFinanced;
                    }
                }
                return res;
            }
        }
        public decimal OriginationFees
        {
            get
            {
                decimal res = 0;
                for (int i = 0; i < Invoices.Count; i++)
                {
                    if (Invoices[i].TypeID == TYPELENDERLOANORIGINATIONFEE)
                    {
                        res += Invoices[i].AmountFinanced;
                        break;
                    }
                }
                return res;
            }
        }
        public decimal UpFrontPremiumInvoiceValue
        {
            get
            {
                decimal res = 0;
                for (int i = 0; i < Invoices.Count; i++)
                {
                    if (Invoices[i].ID == 0)
                    {
                        res = Invoices[i].AmountFinanced;
                        break;
                    }
                }

                return res; 
            }
        }
        public decimal TotalClosingCosts
        {
            get
            {
                decimal res = 0;
                for (int i = 0; i < Invoices.Count; i++)
                {
                    //if (Invoices[i].ID > 0)
                    //{
                    //    res += Invoices[i].AmountFinanced;
                    //}
                    res += Invoices[i].AmountFinanced;
                }
                return res;
            }
        }
        public decimal ReserveTotalAmount
        {
            get { return 0; }
        }
        public decimal MortgageBalance
        {
            get 
            {
                decimal res;
                if (ID > 0)
                {
                    res = Payoff.GetPayoffTotal(ID);
                }
                else 
                {
                    res = liens;
                }
                return res;
            }
        }
        public decimal? InitialDraw
        {
            get { return MortgageInfo.InitialDraw; }
            set { MortgageInfo.InitialDraw = value; }
        }
        public decimal Repairs
        {
            get { return PropertyRepairItem.GetTotalRepairs(Property.ID); }
        }
        public decimal OtherCashOut
        {
            get { return MortgageInfo.OtherCashOut ?? 0; }
        }
        public decimal? CreditLine
        {
            get { return MortgageInfo.CreditLine; }
            set { MortgageInfo.CreditLine = value; }
        }
        public decimal? PaymentAmount
        {
            get { return MortgageInfo.PaymentAmount; }
            set { MortgageInfo.PaymentAmount = value; }
        }
        public DateTime? PrimaryBorrowerBirthdate
        {
            get
            {
                if (YoungestBorrower != null)
                {
                    return YoungestBorrower.DateOfBirth;
                }
                else return null;
            }
        }
        public DateTime? CoBorrowerBirthdate
        {
            get
            {
                if (Borrower2 != null)
                {
                    return Borrower2.DateOfBirth;
                }
                else return null;
            }
        }
        public decimal MaxClaimAmount
        {
            get
            {
                decimal res = 0.0m;
                decimal marketValue = (decimal)(Property.SPValue != 0 ? Property.SPValue : 0);
                if (MortgageInfo.Product.CalculationType == ProductFlag.FNMA)
                {
                    res = Math.Min(marketValue, Property.LendingLimit);
                }
                else if ((MortgageInfo.Product.CalculationType == ProductFlag.HECM_Annual) || (MortgageInfo.Product.CalculationType == ProductFlag.HECM_Monthly) || (MortgageInfo.Product.CalculationType == ProductFlag.HECM_FIXED))
                {
                    res = Math.Min(marketValue, Product.ConventionalMortgageLoan);
                }
                return res;
            }
        }
        #endregion

        #region invoices groups
        protected DataTable DtInvoice
        {
            get
            {
                if (dtInvoice == null)
                {
                    dtInvoice = GetInvoiceList();
                }
                return dtInvoice;
            }
        }

        #region Invoice groups

        public List<UniqueThirdPartyProvider>UniqueThirdPartyProviders
        {
            get
            {
                if(uniqueThirdPartyProviders == null)
                {
                    uniqueThirdPartyProviders = GetThirdPartyProviders();
                }
                return uniqueThirdPartyProviders;
            }
        }
        private List<UniqueThirdPartyProvider> GetThirdPartyProviders()
        {
            List<UniqueThirdPartyProvider> list = new List<UniqueThirdPartyProvider>();
            DataView dv = UniqueThirdPartyProvider.GetMortgageProviders(ID);
            for (int i = 0; i < dv.Count; i++)
            {
                list.Add(new UniqueThirdPartyProvider(dv[i],Invoices));
            }
            return list;
        }

        #region POC Groups
        public List<Invoice> AdditionalChargeSurveyPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYADDITIONALCHARGE, TYPEADDITIONALSERVEY), true);
            }
        }
        public List<Invoice> AdditionalChargePestPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYADDITIONALCHARGE, TYPEADDITIONALPESTINSPECTION), true);
            }
        }
        public List<Invoice> AdditionalChargeOtherPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYADDITIONALCHARGE, TYPEADDITIONALOTHER), true);
            }
        }
        public List<Invoice> GovernmentChargeRecordingPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYGOVERMENTCHARGE, TYPEGOVERMENTRECORDINGFEES), true);
            }
        }
        public List<Invoice> GovernmentChargeReleasePOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYGOVERMENTCHARGE, TYPEGOVERMENTRELEASE), true);
            }
        }
        public List<Invoice> GovernmentChargeCityCountyStampsPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYGOVERMENTCHARGE, TYPEGOVERMENTCITYCOUNTYSTAMPS), true);
            }
        }
        public List<Invoice> GovernmentChargeStateTaxStampsPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYGOVERMENTCHARGE, TYPEGOVERMENTSTATETAXSTAMPS), true);
            }
        }
        public List<Invoice> GovernmentChargeOtherPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYGOVERMENTCHARGE, TYPEGOVERMENTOTHER), true);
            }
        }
        public List<Invoice> LenderChargeAppraisalFeePOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERAPPRAISALFEE), true);
            }
        }
        public List<Invoice> LenderChargeBrokerFeePOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERBROKERFEE), true);
            }
        }
        public List<Invoice> LenderChargeComplianceCertificationPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERCOMPLIANCECERTIFICATIONFEE), true);
            }
        }
        public List<Invoice> LenderChargeCorrespondentFeePOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERCORRESPONDENTFEE), true);
            }
        }
        public List<Invoice> LenderChargeCreditReportPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERCREDITREPORT), true);
            }
        }
        public List<Invoice> LenderChargeFeeToServiceCompanyPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERSERVICECOMPANYFEE), true);
            }
        }
        public List<Invoice> LenderChargeFloodCertificationFeePOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERFLOODCERTIFICATIONFEE), true);
            }
        }
        public List<Invoice> LenderChargeLoanOriginationFeePOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERLOANORIGINATIONFEE), true);
            }
        }
        public List<Invoice> LenderChargeLenderInspectionFeePOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERLENDERINSPECTIONFEE), true);
            }
        }
        public List<Invoice> LenderChargeMortgageInsuranceApplicationPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERINSURANCEAPPLICATIONFEE), true);
            }
        }
        public List<Invoice> LenderChargeRepairAdministrationFeePOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERREPAIRADMINISTARTIONFEE), true);
            }
        }
        public List<Invoice> LenderChargeTaxServiceFeePOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERTAXSERVICEFEE), true);
            }
        }
        public List<Invoice> LenderChargeOtherPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDEROTHER), true);
            }
        }
        public List<Invoice> TitleChargeAbstractOrTitleSearchPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEABSTARCT), true);
            }
        }
        public List<Invoice> TitleChargeAttorneyFeePOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEATTORNEYFEE), true);
            }
        }
        public List<Invoice> TitleChargeDocumentPreparationPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEDOCUMENTPREPARATION), true);
            }
        }
        public List<Invoice> TitleChargeLendersCoveragePOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLELENDERCOVERAGE), true);
            }
        }
        public List<Invoice> TitleChargeNotaryPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLENOTARY), true);
            }
        }
        public List<Invoice> TitleChargeOwnersCoveragePOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEOWNERCOVERAGE), true);
            }
        }
        public List<Invoice> TitleChargeSettlementOrClosingFeePOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLESETTELMENTFEE), true);
            }
        }
        public List<Invoice> TitleChargeTitleExaminationPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEEXAMINATION), true);
            }
        }
        public List<Invoice> TitleChargeTitleInsurancePOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEINSURANCE), true);
            }
        }
        public List<Invoice> TitleChargeOtherPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEOTHER), true);
            }
        }

        public List<Invoice> LenderChargeLenderCourierFeePOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERCURIERFEE), true);
            }
        }
        public List<Invoice> TitleChargeTitleInsuranceBinderPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEINSURANCEBINDER), true);
            }
        }
        public List<Invoice> TitleChargeTitleEndorsementsPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEENDORSEMENTS), true);
            }
        }
        public List<Invoice> ItemsToPayInAdvanceMortgageInsurancePremiumPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYITEMSPAYADVANCE, TYPEMORTGAGEINSURANCEPREMIUM), true);
            }
        }
        public List<Invoice> ItemsToPayInAdvanceHazardInsurancePremiumPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYITEMSPAYADVANCE, TYPEHAZARDINSURANCEREMIUM), true);
            }
        }
        public List<Invoice> ItemsToPayInAdvanceFloodInsurancePremiumPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYITEMSPAYADVANCE, TYPEFLOODINSURANCEREMIUM), true);
            }
        }
        public List<Invoice> ItemsToPayInAdvanceOtherPOCInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYITEMSPAYADVANCE, TYPEITEMSPAYADVANCEOTHER), true);
            }
        }

        #endregion
         
        public List<Invoice> AdditionalChargeSurveyInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYADDITIONALCHARGE, TYPEADDITIONALSERVEY),false);
            }
        }
        public List<Invoice> AdditionalChargePestInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYADDITIONALCHARGE, TYPEADDITIONALPESTINSPECTION), false);
            }
        }
        public List<Invoice> AdditionalChargeOtherInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYADDITIONALCHARGE, TYPEADDITIONALOTHER), false);
            }
        }
        public List<Invoice> GovernmentChargeRecordingInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYGOVERMENTCHARGE, TYPEGOVERMENTRECORDINGFEES), false);
            }
        }
        public List<Invoice> GovernmentChargeReleaseInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYGOVERMENTCHARGE, TYPEGOVERMENTRELEASE), false);
            }
        }
        public List<Invoice> GovernmentChargeCityCountyStampsInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYGOVERMENTCHARGE, TYPEGOVERMENTCITYCOUNTYSTAMPS), false);
            }
        }
        public List<Invoice> GovernmentChargeStateTaxStampsInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYGOVERMENTCHARGE, TYPEGOVERMENTSTATETAXSTAMPS), false);
            }
        }
        public List<Invoice> GovernmentChargeOtherInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYGOVERMENTCHARGE, TYPEGOVERMENTOTHER), false);
            }
        }
        public List<Invoice> LenderChargeAppraisalFeeInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERAPPRAISALFEE), false);
            }
        }
        public List<Invoice> LenderChargeBrokerFeeInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERBROKERFEE), false);
            }
        }
        public List<Invoice> LenderChargeComplianceCertificationInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERCOMPLIANCECERTIFICATIONFEE), false);
            }
        }
        public List<Invoice> LenderChargeCorrespondentFeeInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERCORRESPONDENTFEE), false);
            }
        }
        public List<Invoice> LenderChargeCreditReportInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERCREDITREPORT), false);
            }
        }
        public List<Invoice> LenderChargeFeeToServiceCompanyInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERSERVICECOMPANYFEE), false);
            }
        }
        public List<Invoice> LenderChargeFloodCertificationFeeInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERFLOODCERTIFICATIONFEE), false);
            }
        }
        public List<Invoice> LenderChargeLoanOriginationFeeInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERLOANORIGINATIONFEE), false);
            }
        }
        public List<Invoice> LenderChargeLenderInspectionFeeInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERLENDERINSPECTIONFEE), false);
            }
        }
        public List<Invoice> LenderChargeMortgageInsuranceApplicationInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERINSURANCEAPPLICATIONFEE), false);
            }
        }
        public List<Invoice> LenderChargeRepairAdministrationFeeInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERREPAIRADMINISTARTIONFEE), false);
            }
        }
        public List<Invoice> LenderChargeTaxServiceFeeInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERTAXSERVICEFEE), false);
            }
        }
        public List<Invoice> LenderChargeOtherInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDEROTHER), false);
            }
        }
        public List<Invoice> TitleChargeAbstractOrTitleSearchInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEABSTARCT), false);
            }
        }
        public List<Invoice> TitleChargeAttorneyFeeInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEATTORNEYFEE), false);
            }
        }
        public List<Invoice> TitleChargeDocumentPreparationInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEDOCUMENTPREPARATION), false);
            }
        }
        public List<Invoice> TitleChargeLendersCoverageInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLELENDERCOVERAGE), false);
            }
        }
        public List<Invoice> TitleChargeNotaryInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLENOTARY), false);
            }
        }
        public List<Invoice> TitleChargeOwnersCoverageInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEOWNERCOVERAGE), false);
            }
        }
        public List<Invoice> TitleChargeSettlementOrClosingFeeInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLESETTELMENTFEE), false);
            }
        }
        public List<Invoice> TitleChargeTitleExaminationInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEEXAMINATION), false);
            }
        }
        public List<Invoice> TitleChargeTitleInsuranceInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEINSURANCE), false);
            }
        }
        public List<Invoice> TitleChargeOtherInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEOTHER), false);
            }
        }

        public List<Invoice> LenderChargeLenderCourierFeeInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYLENDERCHARGE, TYPELENDERCURIERFEE), false);
            }
        }
        public List<Invoice> TitleChargeTitleInsuranceBinderInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEINSURANCEBINDER), false);
            }
        }
        public List<Invoice> TitleChargeTitleEndorsementsInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYTITLECHARGE, TYPETITLEENDORSEMENTS), false);
            }
        }
        public List<Invoice> ItemsToPayInAdvanceMortgageInsurancePremiumInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYITEMSPAYADVANCE, TYPEMORTGAGEINSURANCEPREMIUM), false);
            }
        }
        public List<Invoice> ItemsToPayInAdvanceHazardInsurancePremiumInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYITEMSPAYADVANCE, TYPEHAZARDINSURANCEREMIUM), false);
            }
        }
        public List<Invoice> ItemsToPayInAdvanceFloodInsurancePremiumInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYITEMSPAYADVANCE, TYPEFLOODINSURANCEREMIUM), false);
            }
        }
        public List<Invoice> ItemsToPayInAdvanceOtherInvoices
        {
            get
            {
                return GetInvoicesByFilter(GetInvoiceFilter(FEECATEGORYITEMSPAYADVANCE, TYPEITEMSPAYADVANCEOTHER), false);
            }
        }



        public bool HasRequiredFields
        {
            get { return !String.IsNullOrEmpty(requiredFieldsData); }
        }
        public VendorGlobal TitleVendor
        {
            get
            {
                int titleVendorId=-1;
                if(Invoices!=null)
                {
                    for(int i=0;i<Invoices.Count;i++)
                    {
                        if(Invoices[i].TypeID==TYPETITLEINSURANCE)
                        {
                            titleVendorId = Invoices[i].ProviderId;
                            break;
                        }
                    }
                }                
                return new VendorGlobal(titleVendorId);
            }
        }
        #endregion
        #endregion

        #endregion

        #region Constructor
        public MortgageProfile()
            : this(0)
        {
        }
        public MortgageProfile(int id)
        {
            ID = id;
            if (ID > 0)
            {
                DataSet ds = db.GetDataSet(GETMORTGAGEPROFILEBYID, ID);
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ID = id;
                    curProfileStatusID = Convert.ToInt32(dr["CurProfileStatusID"].ToString());
                    statusName = dr["StatusName"].ToString();
                    companyID = Convert.ToInt32(dr["CompanyID"].ToString());
                    created = Convert.ToDateTime(dr["Created"]);
                    propertyId = Convert.ToInt32(dr["propertyID"].ToString());
                    mortgageInfoId = Convert.ToInt32(dr["mortgageInfoID"].ToString());
                    payoffUpdateNeeded = Convert.ToBoolean(dr["payoffupdateneeded"].ToString());
                    dayToWorkUpdateNeeded = Convert.ToBoolean(dr["DayToWorkUpdateNeeded"].ToString());
                    campaignUpdateNeeded = Convert.ToBoolean(dr["campaignUpdateNeeded"].ToString());
                    requiredFieldsData = dr["requiredfields"].ToString();
                    interviewerId = int.Parse(dr["interviewerId"].ToString());
                    if (dr["CalculatorRunNeeded"] != DBNull.Value)
                    {
                        calculatorUpdateNeeded = bool.Parse(dr["CalculatorRunNeeded"].ToString());
                    }
                }
            }
            else
            {
                Borrowers.Add(new Borrower(this));
            }
        }
        public MortgageProfile(int id, AppPage curPage)
            : this(id)
        {
            currentPage = curPage;
        }
        #endregion

        #region methods
        #region new evaluation logic
        private void InitRuleData()
        {
            visibleFields = new Hashtable();
            RuleObjectFields = new Hashtable();
            ruleObjects = new Hashtable();
            dataSetFields = new Hashtable();
        }
        public void AddRule(int objectTypeId, int ruleId)
        {
            if ((objectTypeId & RuleTree.CONDITIONBIT) != 0)
            {
                AddRuleToObject(RuleTree.CONDITIONBIT, ruleId);
            }
            if ((objectTypeId & RuleTree.TASKBIT) != 0)
            {
                AddRuleToObject(RuleTree.TASKBIT, ruleId);
            }
            if ((objectTypeId & RuleTree.DOCUMENTBIT) != 0)
            {
                AddRuleToObject(RuleTree.DOCUMENTBIT, ruleId);
            }
            if ((objectTypeId & RuleTree.EVENTBIT) != 0)
            {
                AddRuleToObject(RuleTree.EVENTBIT, ruleId);
            }
            if ((objectTypeId & RuleTree.ALERTBIT) != 0)
            {
                AddRuleToObject(RuleTree.ALERTBIT, ruleId);
            }
            if ((objectTypeId & RuleTree.CHECKLISTBIT) != 0)
            {
                AddRuleToObject(RuleTree.CHECKLISTBIT, ruleId);
            }
            if ((objectTypeId & RuleTree.DATABIT) != 0)
            {
                AddRuleToObject(RuleTree.DATABIT, ruleId);
            }
        }
        public void AddRuleFields(int objectTypeId, ArrayList fields)
        {
            if (fields != null)
            {
                for (int i = 0; i < fields.Count; i++)
                {
                    string fieldName = fields[i].ToString();
                    if (RuleObjectFields.ContainsKey(fieldName))
                    {
                        int objects = (int)RuleObjectFields[fieldName];
                        objects |= objectTypeId;
                        RuleObjectFields[fieldName] = objects;
                    }
                    else
                    {
                        RuleObjectFields.Add(fieldName, objectTypeId);
                    }
                }
            }
        }

        private void AddRuleToObject(int objectType, int ruleId)
        {
            if (ruleObjects.ContainsKey(objectType))
            {
                Hashtable ht = (Hashtable)ruleObjects[objectType];
                if (!ht.ContainsKey(ruleId))
                {
                    ht.Add(ruleId, true);
                    ruleObjects[objectType] = ht;
                }
            }
            else
            {
                Hashtable ht = new Hashtable();
                ht.Add(ruleId, true);
                ruleObjects.Add(objectType, ht);
            }
        }
        public void BuildRuleEvaluationTree(RuleTreePublic rt)
        {
            if(currentPage.GetRuleEvaluationNeeded())
            {
                ruleEvaluationTree = new RuleEvaluationTree();
                InitRuleData();
                ruleEvaluationTree.Evaluate(this, rt);
                //update conditions and events
                string xml = GetRuleListXml(RuleTree.CONDITIONBIT);
                UpdateMPConditionRules(xml);
                //UpdateMPEventRules(xml);

                currentPage.SetRuleEvaluationNeeded(false);
                RecalculateRequiredField();
            }
        }
        public int[] GetRuleList(int ruleTypeId)
        {
            if (ruleObjects.ContainsKey(ruleTypeId))
            {
                Hashtable ht = ruleObjects[ruleTypeId] as Hashtable;
                if ((ht != null) && (ht.Count > 0))
                {
                    int[] res = new int[ht.Count];
                    int i = 0;
                    foreach (DictionaryEntry item in ht)
                    {
                        res[i] = int.Parse(item.Key.ToString());
                        i++;
                    }
                    return res;
                }
            }
            return null;
        }
        public string GetRuleListXml(int ruleTypeId)
        {
            string res = String.Empty;
            if (ruleObjects.ContainsKey(ruleTypeId))
            {
                res = BuildXmlFromHashtable((Hashtable)ruleObjects[ruleTypeId]);
            }
            return res;
        }
        private static string BuildXmlFromHashtable(Hashtable ht)
        {
            string res = String.Empty;
            if ((ht != null) && (ht.Count > 0))
            {
                XmlDocument d = new XmlDocument();
                XmlNode root = d.CreateElement(ROOTELEMENT);
                foreach (DictionaryEntry item in ht)
                {
                    XmlNode n = d.CreateElement(ITEMELEMENT);
                    XmlAttribute a = d.CreateAttribute(IDATTRIBUTE);
                    a.Value = item.Key.ToString();
                    n.Attributes.Append(a);
                    root.AppendChild(n);
                }
                if (root.ChildNodes.Count > 0)
                {
                    d.AppendChild(root);
                    res = d.OuterXml;
                }
            }
            return res;
        }
        #endregion
        #region required fields logic
        private string GetEmptyFieldsString()
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < EmptyFields.Count;i++)
            {
                s.Append(":" + EmptyFields[i] + "#");
            }
            return s.ToString();
        }

        public void ResetCompletedTabs()
        {
            completedTabs = null;
        }
        private Hashtable GetTabRequiredFields()
        {
            Hashtable res = new Hashtable();
            DataTable dt = currentPage.GetDictionaryTableByProcedure(GETTABFIELDS);
            DataView dv = new DataView(dt);
            DataView dvt = currentPage.GetDictionaryTableByProcedure(GETTAB).DefaultView;
            for (int i = 0; i < dvt.Count; i++)
            {
                int tabId = int.Parse(dvt[i]["id"].ToString());
                int parentTabId = int.Parse(dvt[i]["tablevel2id"].ToString());
                bool isTabCompleted = CheckTabFields(tabId, dv);
                Hashtable ht;
                if (res.Count > 0)
                {
                    if (res.ContainsKey(parentTabId))
                    {
                        ht = res[parentTabId] as Hashtable;
                        if (ht != null) ht.Add(tabId, isTabCompleted);
                        res[parentTabId] = ht;
                    }
                    else
                    {
                        ht = new Hashtable();
                        ht.Add(tabId, isTabCompleted);
                        res.Add(parentTabId, ht);
                    }
                }
                else
                {
                    ht = new Hashtable();
                    ht.Add(tabId, isTabCompleted);
                    res.Add(parentTabId, ht);
                }
            }
            return res;
        }
        private bool CheckTabFields(int tabId, DataView dv)
        {
            bool res = true;
            if (RequiredFields.Count > 0)
            {
                dv.RowFilter = String.Format("id={0}", tabId);
                for (int i = 0; i < dv.Count; i++)
                {
                    string fieldName = dv[i]["propertyname"].ToString();
                    if (RequiredFields.ContainsKey(fieldName))
                    {
                        if (CheckStatusValue((int)RequiredFields[fieldName]))
//                        if ((int)RequiredFields[fieldName] <= curProfileStatusID)
                        {
                            if (visibleFields.ContainsKey(fieldName))
                            {
                                res = IsFieldCompleted(fieldName, visibleFields[fieldName] as Hashtable);
                                if (!res) break;
                            }
                        }
                    }
                }
            }
            return res;
        }
        private Hashtable GetFieldsList()
        {
            Hashtable res = new Hashtable();
            DataView dv = currentPage.GetDictionaryTableByProcedure(GETMORTGAGEPROFILEFIELDS).DefaultView;
            for (int i = 0; i < dv.Count;i++ )
            {
                int value = int.Parse(dv[i]["id"].ToString());
                string key = dv[i]["propertyname"].ToString();
                res.Add(key,value);
            }
            return res;
        }

        private ArrayList GetEmptyFields()
        {
            ArrayList res = new ArrayList();
            if (RequiredFields.Count > 0)
            {
                foreach (DictionaryEntry item in RequiredFields)
                {
                    string key = item.Key.ToString();
                    int statusId = (int)item.Value;
                    if (CheckStatusValue(statusId))
//                    if (statusId <= curProfileStatusID)
                    {
                        if (visibleFields.ContainsKey(key))
                        {
                            if (!IsFieldCompleted(key, visibleFields[key] as Hashtable))
                            {
                                if(MortgageProfileFieldIds.ContainsKey(key))
                                {
                                    res.Add(MortgageProfileFieldIds[key]);
                                }
                            }
                        }
                    }
                }
            }
            return res;
        }
        public void RecalculateRequiredField()
        {
            emptyFields = null;
            SaveRequiredField();
        }

        public bool IsFieldsCompleted()
        {
            bool res = true;
            if (RequiredFields.Count > 0)
            {
                foreach (DictionaryEntry item in RequiredFields)
                {
                    string key = item.Key.ToString();
                    int statusId = (int)item.Value;
                    if (CheckStatusValue(statusId))
//                    if (statusId <= curProfileStatusID)
                    {
                        if (visibleFields.ContainsKey(key))
                        {
                            res = IsFieldCompleted(key, visibleFields[key] as Hashtable);
                            if (!res)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return res;
        }
        private bool IsFieldCompleted(string fullPropertyName, Hashtable objects)
        {
            bool res = true;
            string[] names = fullPropertyName.Split('.');
            if (names.Length != 2)
            {
                return res;
            }
            string objectName = names[0];
            string propertyName = names[1];
            Object propValue = GetType().GetProperty(objectName).GetValue(this, null);
            if (propValue is IList)
            {
                IList list = (IList)propValue;
                for (int i = 0; i < list.Count; i++)
                {
                    Object item = list[i];
                    if (IsInList(objects, item))
                    {
                        if (!CheckData(item, propertyName))
                        {
                            res = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                res = CheckData(propValue, propertyName);
            }
            return res;
        }
        private static bool IsInList(Hashtable list, Object o)
        {
            bool res = true;
            if (list != null)
            {
                res = list.ContainsKey(o);

            }
            return res;
        }
        private static bool CheckData(Object o, string propertyName)
        {
            bool res = false;
            PropertyInfo pi = o.GetType().GetProperty(propertyName);
            Type t = pi.PropertyType;
            Object value = pi.GetValue(o, null);
            if (value == null) return res;
            if (IsNullableType(t))
            {
                res = value != null;
            }
            else if (t == typeof(String))
            {
                res = value.ToString() != "";
            }
            else if (t == typeof(int))
            {
                res = (int)value != 0;
            }
            else
            {
                res = true;
            }
            return res;
        }
        #endregion
        #region rule related methods
        #region new visibility functionality
        #region new layout version
        public bool CheckPostBackField(string fullPropertyName)
        {
            if ((ruleEvaluationTree != null) && (ruleEvaluationTree.RTPublic != null))
            {
                Hashtable ht = ruleEvaluationTree.RTPublic.RuleFields;
                if (ht != null)
                {
                    if (ht.ContainsKey(fullPropertyName))
                    {
                        FieldDependancy fd = (FieldDependancy)ht[fullPropertyName];
                        if (fd != null) return (fd.ObjectMask != 0);
                    }
                }
            }
            return false;
        }
        private Hashtable GetEditableFields()
        {
            Hashtable res = new Hashtable();
            DataView dv = db.GetDataView(GETROLEFIELDS, ID);
            for (int i = 0; i < dv.Count; i++)
            {
                string fullPropertyName = dv[i][PROPERTYNAMEFIELDNAME].ToString();
                int roleId = int.Parse(dv[i][ROLEIDFIELDNAME].ToString());
                if (!String.IsNullOrEmpty(fullPropertyName))
                {
                    if (res.ContainsKey(fullPropertyName))
                    {
                        Hashtable roles = res[fullPropertyName] as Hashtable;
                        if (roles != null) roles.Add(roleId, true);
                        res[fullPropertyName] = roles;
                    }
                    else
                    {
                        Hashtable roles = new Hashtable();
                        roles.Add(roleId, true);
                        res.Add(fullPropertyName, roles);
                    }
                }
            }
            return res;
        }
        public bool CheckModifiedByRule(string fullPropertyName, BaseObject obj)
        {
            bool res = false;
            if ((dataSetFields != null) && (dataSetFields.ContainsKey(fullPropertyName)))
            {
                res = true;
                //if ((ht == null) || (obj == null))
                //{
                //    return res;
                //}
                //foreach (DictionaryEntry item in ht)
                //{
                //    if (item.Key == obj)
                //    {
                //        return res;
                //    }
                //}
            }
            return res;
        }

        public bool CheckReadOnly(string fullPropertyName, BaseObject obj)
        {
            bool res = false;
            if ((dataSetFields != null) && (dataSetFields.ContainsKey(fullPropertyName)))
            {
                Hashtable ht = (Hashtable)dataSetFields[fullPropertyName];
                if ((ht == null) || (obj == null))
                {
                    return res;
                }
                foreach (DictionaryEntry item in ht)
                {
                    if (item.Key == obj)
                    {
                        return res;
                    }
                }
            }
            if (EditableFields.ContainsKey(fullPropertyName))
            {
                Hashtable roles = EditableFields[fullPropertyName] as Hashtable;
                if (roles != null)
                {
                    int[] allowedRoles = new int[roles.Count];
                    int i = 0;
                    foreach (DictionaryEntry item in roles)
                    {
                        allowedRoles[i] = (int)item.Key;
                        i++;
                    }
                    res = CurrentUser.IsInRoles(allowedRoles);
                }
            }
            return res;
        }
        public Object GetDataValue(string propertyName, string objectName, BaseObject obj, out int objId)
        {
            Object res = null;
            PropertyInfo pi = GetType().GetProperty(objectName);
            Object o = pi.GetValue(this, null);
            if ((o is IList) && (obj != null))
            {
                o = obj;
            }
            objId = -1;
            if (o != null)
            {
                res = o.GetType().GetProperty(propertyName).GetValue(o, null);
                BaseObject oo = o as BaseObject;
                if (oo != null)
                {
                    objId = oo.ID;
                }
            }
            return res;
        }
        public bool CheckFieldVisibility(string propertyName, string objectName, BaseObject obj)
        {
            bool res;
            string key = objectName + "." + propertyName;
            res = visibleFields.ContainsKey(key);
            if (res)
            {
                IList o = GetObjectList(objectName);
                if ((o != null) && (obj != null))
                {
                    Hashtable list = visibleFields[key] as Hashtable;
                    if ((list != null) && (list.Count > 0))
                    {
                        res = list.ContainsKey(obj);
                    }
                }
            }
            return res;
        }
        public IList GetObjectList(string objectName)
        {
            PropertyInfo pi = GetType().GetProperty(objectName);
            return pi.GetValue(this, null) as IList;
        }
        public string GetObjectName(string fullPropertyName)
        {
            string res = fullPropertyName;
            string[] names = fullPropertyName.Split('.');
            if (names.Length == 2)
            {
                res = names[0];
            }
            return res;
        }
        public void SetVisibleField(Hashtable fields, Hashtable skipObjects)
        {
            if ((fields != null) && (fields.Count > 0))
            {
                foreach (DictionaryEntry item in fields)
                {
                    string key = item.Key.ToString();
#if DUMPRULES
                    string message = key;
#endif
                    IList olist = GetObjectList(GetObjectName(key));
                    if (olist == null)
                    {
                        if (!visibleFields.ContainsKey(key))
                        {
                            visibleFields.Add(key, null);
#if DUMPRULES
                            message = message+"\t\t\t - added";
#endif
                        }
                    }
                    else
                    {
                        Hashtable ht = null;
                        if (visibleFields.ContainsKey(key))
                        {
                            ht = visibleFields[key] as Hashtable;
                        }
                        else
                        {
                            visibleFields.Add(key, null);
                        }
                        if (ht == null)
                        {
                            ht = new Hashtable();
                        }
#if DUMPRULES
                        message = message + "\t\t\t - added for objects";
#endif
                        for (int i = 0; i < olist.Count; i++)
                        {
                            Object o = olist[i];
                            if (!((skipObjects != null) && (skipObjects.ContainsKey(o))))
                            {
                                if (!ht.ContainsKey(o))
                                {
#if DUMPRULES
                                    message = message + String.Format("  Id={0}",((BaseObject)o).ID);
#endif
                                    ht.Add(o, true);
                                }
                            }
                        }
                        visibleFields[key] = ht;
                    }
#if DUMPRULES
                    CurrentPage.WriteDebugToLog(message);
#endif
                }
            }
        }
        #endregion
        #endregion
        #endregion
        #region public
        private List<TrusteeSignatureLine> GetSignatureLines()
        {
            List<TrusteeSignatureLine> res = new List<TrusteeSignatureLine>();
            DataView dv = GetSignatureLinesDv();
            foreach (DataRowView row in dv)
            {
                TrusteeSignatureLine line = new TrusteeSignatureLine(row);
                res.Add(line);
            }
            return res;
        }
        private DataView GetSignatureLinesDv()
        {
            return db.GetDataView(GETSIGNATURELINES, ID);
        }
        public void AddPayoff(decimal amount)
        {
            Payoff payoff = new Payoff();
            payoff.Creditor = "Creditor Unknown";
            payoff.Amount = amount;
            payoff.PayoffStatusId = Payoff.NEEDTOORDERSTATUSID;
            payoff.MortgageID = ID;
            payoff.POC = 0;
            payoff.Perdiem = 0;
            payoff.CreatedBy = CurrentUser.Id;
            payoff.Save();
            payoffs = null;
        }

        public void ApplyClosingCostProfile(int closingCostProfileId, int oldValue, decimal lenderFee)
        {
            db.ExecuteScalar(UPDATEINVOICEWITHCLOSINGCOST, ID, closingCostProfileId, oldValue, CurrentUser.Id, lenderFee);
            invoices = null;
        }
/*
        private string CalculateFormulaData(int closingCostProfileId)
        {
            string res = String.Empty;
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            XmlNode update = d.CreateElement(UPDATEELEMENT);
            XmlNode delete = d.CreateElement(DELETEELEMENT);
            DataView dv = ClosingCostProfile.GetClosingCostProfileDetailes(closingCostProfileId);
            for (int i = 0; i < dv.Count; i++ )
            {
                ClosingCostProfile.ClosingCostProfileDetail details = new ClosingCostProfile.ClosingCostProfileDetail(dv[i]);
                if (details.HasFormula)
                {
                    string s = details.GetFormulaData("StateId");
                    if(!String.IsNullOrEmpty(s))
                    {
                        int stateId = -1;
                        try
                        {
                            stateId = int.Parse(s);
                        }
                        catch
                        {
                        }
                        if(stateId == Property.StateId)
                        {
                            decimal amount = details.RecalculateAmount(Property.MaxClaimAmount);
                            if(amount>0)
                            {
                                XmlNode n = d.CreateElement(ITEMELEMENT);
                                XmlAttribute a = d.CreateAttribute("id");
                                a.Value = details.ID.ToString();
                                n.Attributes.Append(a);
                                XmlAttribute a1 = d.CreateAttribute("amount");
                                a1.Value = amount.ToString();
                                n.Attributes.Append(a1);
                                update.AppendChild(n);
                            }
                        }
                        else
                        {
                            XmlNode n = d.CreateElement(ITEMELEMENT);
                            XmlAttribute a = d.CreateAttribute("id");
                            a.Value = details.ID.ToString();
                            n.Attributes.Append(a);
                            delete.AppendChild(n);
                        }
                    }
                    
                }
            }
            if(update.HasChildNodes)
            {
                root.AppendChild(update);
            }
            if (delete.HasChildNodes)
            {
                root.AppendChild(delete);
            }
            if (root.HasChildNodes)
            {
                d.AppendChild(root);
                res = d.OuterXml;
            }
            return res;
        }
*/
        public void UpdateInvoiceAmount()
        {
            db.ExecuteScalar(UPDATEINVOICEAMOUNT, ID, MortgageInfo.LenderFee);
        }

        public ArrayList GetObjectFields(string objectName)
        {
            string key = objectName + "_" + CurrentUserId;
            if ((objectFields != null) && objectFields.ContainsKey(key))
            {
                return objectFields[key] as ArrayList;
            }
            ArrayList fields = GetMortgageProfileFieldsForObject(objectName, currentUserId);
            if (objectFields == null)
            {
                objectFields = new Hashtable();
            }
            objectFields.Add(key, fields);
            return fields;
        }
        public DataSet GetImportantDates()
        {
            return db.GetDataSet(GETIMPORTANTDATES, ID);
        }
        public ArrayList PopulateFields(Object obj, ArrayList fields)
        {
            ArrayList res = new ArrayList();
            for (int i = 0; i < fields.Count; i++)
            {
                MortgageProfileField mpf = (MortgageProfileField)fields[i];
                mpf.ParentId = ((BaseObject)obj).ID;
                PropertyInfo pi = obj.GetType().GetProperty(mpf.PropertyName);
                mpf.FieldValue = pi.GetValue(obj, null);
                res.Add(mpf);
            }
            return res;
        }
        public DataTable GetDocTemplateList(bool evaluateRules)
        {
            string xml = GetRuleListXml(RuleTree.DOCUMENTBIT);
            if (String.IsNullOrEmpty(xml))
            {
                return null;
            }
            return db.GetDataTable(GETMORTGAGEPROFILEDOCTEMPLATELIST, xml);
        }
        public DataView GetCheckListList(string ruleslist)
        {
            return GetCheckListForStatus(curProfileStatusID, ruleslist);
//            return db.GetDataView(GETMORTGAGEPROFILECHECKLIST, ID, curProfileStatusID, ruleslist);
        }

        public DataView GetCheckListForStatus(int statusId, string ruleslist)
        {
            return db.GetDataView(GETMORTGAGEPROFILECHECKLIST, ID, statusId, ruleslist);
        }
        public void AddDataSetField(RuleDataValueObject rdvo)
        {
            Hashtable ht = null;
            if (dataSetFields.ContainsKey(rdvo.FullName))
            {
                ht = (Hashtable)dataSetFields[rdvo.FullName];
            }
            else
            {
                dataSetFields.Add(rdvo.FullName, null);
            }
            IList olist = GetObjectList(GetObjectName(rdvo.FullName));
            if (olist != null)
            {
                foreach (Object item in olist)
                {
                    if (((BaseObject)item).ID == rdvo.ObjectId)
                    {
                        if (ht == null)
                        {
                            ht = new Hashtable();
                        }
                        ht.Add(item, true);
                        dataSetFields[rdvo.FullName] = ht;
                        break;
                    }
                }
            }
        }
        public string GetObjectValue(string fullPropertyName, int objId, out string err)
        {
            string res = String.Empty;
            err = String.Empty;
            if (String.IsNullOrEmpty(fullPropertyName))
            {
                err = "Wrong object";
                return res;
            }
            string objectName;
            string objectPropertyName;
            Object obj = GetObject(fullPropertyName, out objectName, out objectPropertyName);
            if (obj == null)
            {
                err = "Wrong object";
                return res;
            }
            Object target = null;
            if (String.IsNullOrEmpty(objectPropertyName))
            {
                target = this;
            }
            else
            {
                IList list = obj as IList;
                if (list != null)
                {
                    if (objId < 0)
                    {
                        err = "Wrong object";
                        return res;
                    }
                    foreach (Object item in list)
                    {
                        if (((BaseObject)item).ID == objId)
                        {
                            target = item;
                            break;
                        }
                    }
                }
                else
                {
                    target = obj;
                }
            }
            if (target == null)
            {
                err = "Wrong object";
                return res;
            }
            PropertyInfo pi = target.GetType().GetProperty(objectPropertyName);
            res = pi.GetValue(target, null).ToString();
            return res;
        }
        public Object GetObjectByIndex(string fullPropertyName, int index, out string err)
        {
            err = String.Empty;
            if (String.IsNullOrEmpty(fullPropertyName))
            {
                err = "Wrong object";
                return null;
            }
            string objectName;
            string objectPropertyName;
            Object obj = GetObject(fullPropertyName, out objectName, out objectPropertyName);
            if (obj == null)
            {
                err = "Wrong object";
                return null;
            }
            Object target = null;
            if (String.IsNullOrEmpty(objectPropertyName))
            {
                target = this;
            }
            else
            {
                IList list = obj as IList;
                if (list != null)
                {
                    if (index < 0)
                    {
                        err = "Wrong object";
                        return null;
                    }
                    if (index > (list.Count - 1))
                    {
                        err = "Index out of range";
                        return null;
                    }
                    target = list[index];
                }
                else
                {
                    target = obj;
                }
            }
            PropertyInfo pi = target.GetType().GetProperty(objectPropertyName);
            return pi.GetValue(target, null);
        }

        public string GetObjectValueByIndex(string fullPropertyName, int index, out string err)
        {
            string res = String.Empty;
            err = String.Empty;
            if (String.IsNullOrEmpty(fullPropertyName))
            {
                err = "Wrong object";
                return res;
            }
            string objectName;
            string objectPropertyName;
            Object obj = GetObject(fullPropertyName, out objectName, out objectPropertyName);
            if (obj == null)
            {
                err = "Wrong object";
                return res;
            }
            Object target = null;
            if (String.IsNullOrEmpty(objectPropertyName))
            {
                target = this;
            }
            else
            {
                IList list = obj as IList;
                if (list != null)
                {
                    if (index < 0)
                    {
                        err = "Wrong object";
                        return res;
                    }
                    if(index > (list.Count-1))
                    {
                        err = "Index out of range";
                        return res;
                    }
                    target = list[index];
                }
                else
                {
                    target = obj;
                }
            }
            PropertyInfo pi = target.GetType().GetProperty(objectPropertyName);
            Object o = pi.GetValue(target, null);
            if(o!=null)
            {
                res = pi.GetValue(target, null).ToString();
            }
            return res;
        }
        private static Object ConvertValue(string propertyValue, Type t_, string dateFormat)
        {
            Object res = null;
            Type t = t_;
            bool isNullable = IsNullableType(t);
            if(isNullable)
            {
                t = Nullable.GetUnderlyingType(t);
            }
            string typeName = t.Name.ToLower();
            switch(typeName)
            {
                case "boolean":
                    if (propertyValue=="false"||propertyValue=="0")
                    {
                        res = false;
                    }
                    else
                    {
                        res = true;
                    }
                    break;
                case "string":
                    res = propertyValue;
                    break;
                case "datetime":
                    if(String.IsNullOrEmpty(dateFormat))
                    {
                        res = DateTime.Parse(propertyValue);
                    }
                    else
                    {
                        res = DateTime.ParseExact(propertyValue, dateFormat, System.Globalization.CultureInfo.InvariantCulture); 
                    }
                    break;
                case "int32":
                    res = int.Parse(propertyValue);
                    break;
                case "float":
                    res = float.Parse(propertyValue);
                    break;
                case "decimal":
                    res = decimal.Parse(propertyValue.Replace("$","").Replace(",",""));
                    break;

            }
            return res;
        }
        private static Object GetTargetObject(Object src, int index, Type realType)
        {
            Object res = null;
            IList list = src as IList;
            if (list ==  null)
            {
                ArrayList alist = src as ArrayList;
                if (alist == null)
                {
                    throw new Exception(String.Format("Can't create object {0} ", realType.FullName));
                }
                else
                {
                    while ((alist.Count-1)< index)
                    {
                        Object instance = Assembly.GetExecutingAssembly().CreateInstance(realType.FullName);
                        alist.Add(instance);
                    }
                    res = alist[index];
                }

            }
            else
            {
                while ((list.Count - 1) < index)
                {
                    Object instance = Assembly.GetExecutingAssembly().CreateInstance(realType.FullName);
                    list.Add(instance);
                }
                res = list[index];
            }
            return res;
        }

        public bool SetProperty(string fullPropertyName, int index, string propertyValue, string dateFormat)
        {
            string objectName;
            string objectPropertyName;
            Type realType;
            bool isList = MortgageProfile.IsPropertyList(fullPropertyName, out realType);
            if(isList&&index<0)
            {
                throw new Exception(String.Format("For {0} index must be >=0", fullPropertyName));
            }
            if(!isList&&index>=0)
            {
                throw new Exception(String.Format("For {0} index must be <0", fullPropertyName));
            }
            Object obj = GetObject(fullPropertyName, out objectName, out objectPropertyName);
            if (isList)
            {
                obj = GetTargetObject(obj, index, realType);
                if (obj == null)
                {
                    throw new Exception(String.Format("Can't create object {0} ", fullPropertyName));
                }
            }
            PropertyInfo pi = obj.GetType().GetProperty(objectPropertyName);
            if(!String.IsNullOrEmpty(propertyValue))
            {
                Object val = ConvertValue(propertyValue, pi.PropertyType, dateFormat);
                try
                {
                    pi.SetValue(obj, val, null);
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("Can't set field {0} value={1}", fullPropertyName, propertyValue));
                }
            }
            return true;
        }

        public bool UpdateObject(string fullPropertyName, string propertyValue, int objId, out string err)
        {
            err = String.Empty;
            if (String.IsNullOrEmpty(fullPropertyName))
            {
                return false;
            }
            string objectName;
            string objectPropertyName;
            Object obj = GetObject(fullPropertyName, out objectName, out objectPropertyName);
            if (obj == null)
            {
                return false;
            }
            Object target = null;
            if (String.IsNullOrEmpty(objectPropertyName))
            {
                target = this;
            }
            else
            {
                IList list = obj as IList;
                if (list != null)
                {
                    if (objId < 0)
                    {
                        return false;
                    }
                    foreach (Object item in list)
                    {
                        if (((BaseObject)item).ID == objId)
                        {
                            target = item;
                            break;
                        }
                    }
                }
                else
                {
                    target = obj;
                }
            }
            if (target == null)
            {
                return false;
            }
            return ValidateAndSave(target,fullPropertyName , objectPropertyName, propertyValue, out err);
        }
        private static int GetPropertyType(Type t)
        {
            int res = 0;
            if (t.Name.ToLower() == "string")
            {
                res = 1;
            }
            else if (t.Name.ToLower() == "int32")
            {
                res = 2;
            }
            else if (t.Name.ToLower() == "single")
            {
                res = 3;
            }
            else if (t.Name.ToLower() == "decimal")
            {
                res = 4;
            }
            else if (t.Name.ToLower() == "datetime")
            {
                res = 5;
            }
            else if (t.Name.ToLower() == "boolean")
            {
                res = 6;
            }
            return res;
        }
        private Object GetObject(string fullPropertyName, out string objectName, out string objectPropertyName)
        {
            Object res = null;
            int i = fullPropertyName.IndexOf('.');
            objectPropertyName = String.Empty;
            if (i < 0)
            {
                objectName = fullPropertyName;
            }
            else
            {
                objectName = fullPropertyName.Substring(0, i);
                objectPropertyName = fullPropertyName.Substring(i + 1);
            }
            PropertyInfo pi = GetType().GetProperty(objectName);
            try
            {
                res = pi.GetValue(this, null);
            }
            catch { }
            return res;
        }
        private bool ValidateAndSave(object target, string fullpropertyName, string objectPropertyName, string propertyValue, out string err)
        {
            bool res;
            err = "*";
            PropertyInfo pi = target.GetType().GetProperty(objectPropertyName);
            Object val;
            Object oldValue = pi.GetValue(target, null);
            Type t = pi.PropertyType;
            bool isNullable = IsNullableType(t);
            if (isNullable)
            {
                t = Nullable.GetUnderlyingType(t);
            }
            if (isNullable && t.Name == "Boolean" && propertyValue.ToLower() == "- select -")
            {
                propertyValue = "";
            }
            if(String.IsNullOrEmpty(propertyValue))
            {
                val = null;
                pi.SetValue(target, val, null);
            }
            else
            {
                string typeName = t.Name;
                if (typeName == "Boolean")
                {
                    if (String.IsNullOrEmpty(propertyValue))
                    {
                        propertyValue = false.ToString();
                    }
                    else if ((propertyValue == "on") || (propertyValue == "1") || (propertyValue.ToLower() == "yes") || (propertyValue.ToLower() == "true"))
                    {
                        propertyValue = true.ToString();
                    }
                    else
                    {
                        propertyValue = false.ToString();
                    }
                }
                else if (typeName == "DateTime")
                {
                    try
                    {
                        propertyValue = (DateTime.ParseExact(propertyValue, "yyyy-MM-dd-hh-mm-ss", null)).ToString();
                    }
                    catch { }

                }
                try
                {
                    if (IsNullableType(t))
                    {
                        t = Nullable.GetUnderlyingType(t);
                    }
                    val = Convert.ChangeType(propertyValue, t);
                    if (Equals(val,oldValue))
                    {
                        return true;
                    }
                    pi.SetValue(target, val, null);
                }
                catch (Exception ex)       // LOG!!!!
                {
                    err = "*";
                    return false;
                }
            }
            res = ((BaseObject)target).ValidateProperty(objectPropertyName, out err);
            if (res)
            {
                string fieldName;
                string parentFieldName;
                int parentId;
                BaseObject dbObject = GetDbObject(target, pi, out fieldName, out parentFieldName, out parentId);
                res = dbObject.Save(this, dbObject.GetType().Name, fullpropertyName, fieldName, GetPropertyType(t), val, oldValue, parentFieldName, parentId, currentPage.GetProcessRequired()) > 0;
                IsUpdated = res;
            }
            else
            {
                pi.SetValue(target, oldValue, null);
            }
            return res;
        }
        private static BaseObject GetDbObject(Object obj, PropertyInfo pi, out string fieldName, out string parentFieldName, out int parentId)
        {
            BaseObject res = obj as BaseObject;
            fieldName = pi.Name;
            parentId = -1;
            parentFieldName = String.Empty;
            object[] attributes = pi.GetCustomAttributes(true);
            foreach (Object attribute in attributes)
            {
                DbMapping mapping = attribute as DbMapping;
                if (mapping != null)
                {
                    string objectName = mapping.ObjectName;
                    fieldName = mapping.FieldName;
                    if (!String.IsNullOrEmpty(objectName))
                    {
                        PropertyInfo pic = obj.GetType().GetProperty(objectName);
                        if (pic != null)
                        {
                            res = pic.GetValue(obj, null) as BaseObject;
                            parentFieldName = obj.GetType().Name + "id";
                            parentId = ((BaseObject)obj).ID;
                            break;
                        }
                    }
                }
            }
            return res;

        }
        public Borrower GetBorrowerById(int borrowerid)
        {
            foreach (Borrower borrower in Borrowers)
            {
                if (borrower.ID == borrowerid)
                {
                    return borrower;
                }
            }
            return null;
        }
        public int[] MPCheckListStatuses()
        {
            int[] result;
            DataTable td = GetCheckListStatusList().Table;
            result = new int[td.Rows.Count];
            for (int i = 0; i < td.Rows.Count; i++)
            {
                result[i] = (int)td.Rows[i]["id"];
            }
            return result;
        }
        public DataView GetCheckListStatusList()
        {
            return db.GetDataView(GETCHECKLISTSTATUSLIST);
        }
        #region data saving methods
        public int SaveContacts()
        {
            string xml = GetAssignedUserXml();
            if (!String.IsNullOrEmpty(xml))
            {
                return db.ExecuteScalarInt(SAVEASSIGNEDUSERS, ID, companyID, xml);
            }
            return ID;
        }
        public bool UpdateCheckList(int checkListId, short sel)
        {
            return db.ExecuteScalarInt(UPDATECHECKLIST,ID, checkListId, sel) == 1;
        }

        public bool SaveCheckListData(string data)
        {
            return db.ExecuteScalarInt(SAVEMORTGAGEPROFILECHECKLIST, ID, curProfileStatusID, data) > 0;
        }
        public void UpdateMortgageProfileStatus(int statusId, int userId)
        {
            CurProfileStatusID = statusId;
            db.ExecuteScalar(SAVEMORTGAGEPROFILESTATUS, ID, statusId, userId);
        }
        public void UpdateMPAlertRules(string ruleList)
        {
            db.Execute(UPDATEMORTGAGEPROFILEALERTRULES, ID, ruleList);
        }
        public void UpdateMPTaskRules(string ruleList)
        {
            db.Execute(UPDATEMORTGAGEPROFILETASKRULES, ID, ruleList);
        }
        public int CreateNew(AppUser user)
        {
            Borrower borrower = Borrowers[0];
            if (borrower == null)
            {
                return -1;
            }
            ArrayList logs = new ArrayList();
            logs.Add(new MortgageLogEntry(BORROWER, -1, BORROWERFIRSTNAME, String.Empty, borrower.FirstName, CurrentUserId));
            logs.Add(new MortgageLogEntry(BORROWER, -1, BORROWERLASTNAME, String.Empty, borrower.LastName, CurrentUserId));
            object objBorrBirthDate = borrower.DateOfBirth == null ? DBNull.Value : (object)(DateTime)borrower.DateOfBirth;
            return db.ExecuteScalarInt(CREATENEWMORTGAGE, user.Id, user.CompanyId, borrower.FirstName, borrower.LastName, objBorrBirthDate, curProfileStatusID, GetLogXml(logs));
        }
        public int SaveNewBorrower(int userId, Borrower borrower)
        {
            ArrayList logs = new ArrayList();
            logs.Add(new MortgageLogEntry(BORROWER, -1, BORROWERFIRSTNAME, String.Empty, borrower.FirstName, CurrentUserId));
            logs.Add(new MortgageLogEntry(BORROWER, -1, BORROWERLASTNAME, String.Empty, borrower.LastName, CurrentUserId));
            object objBorrBirthDate = borrower.DateOfBirth == null ? DBNull.Value : (object)(DateTime)borrower.DateOfBirth;
            string logData = GetLogXml(logs);
            return db.ExecuteScalarInt(SAVENEWBORROWER, ID, userId, borrower.FirstName, borrower.LastName, objBorrBirthDate, logData);
        }
        public void ResetInvoices()
        {
            invoices = null;
        }
        public void ResetPayoff()
        {
            payoffs = null;
            CalculatorUpdateNeeded = true;
        }
        public bool SaveInvoiceWithLog(Invoice invoice, ArrayList logs)
        {
            invoices = null;
            return db.ExecuteScalarInt(SAVEMORTGAGEPROFILEINVOICE,
                                        invoice.ID,
                                        invoice.MortgageID,
                                        invoice.TypeID,
                                        invoice.Description,
                                        invoice.ProviderId,
                                        invoice.StatusID,
                                        invoice.Amount,
                                        invoice.POCAmount,
                                        invoice.DeedAmount,
                                        invoice.MortgageAmount,
                                        invoice.ReleaseAmount,
                                        invoice.LenderCreditAmount,
                                        invoice.ThirdPartyPaidAmount,
                                        invoice.DueDate,
                                        invoice.ChargeToId,
                                        currentUserId,
                                        invoice.CalculateFee,
                                        invoice.ProviderTypeId,
                                        invoice.OtherProviderName,
                                        invoice.OtherTypeDescription,
                                        invoice.Listendorsements,
                                        invoice.ShowAmounts,
                                        invoice.GetFormulaData(),
                                        GetLogXml(logs)) == 1;
        }
        public bool SaveReserveWithLog(Reserve reserve, ArrayList logs)
        {
            return db.ExecuteScalarInt(SAVEMORTGAGEPROFILERESERVE,
                                        reserve.ID,
                                        reserve.MortgageID,
                                        reserve.Description,
                                        reserve.TypeId,
                                        reserve.Months,
                                        reserve.StatementStart,
                                        reserve.StatementEnd,
                                        reserve.Amount,
                                        reserve.ChargeToId,
                                        currentUserId,
                                        GetLogXml(logs)) == 1;
        }
        public int SavePayoffWithLog(Payoff payoff, ArrayList logs)
        {
            int res = db.ExecuteScalarInt(SAVEMORTGAGEPROFILEPAYOFF
                    , payoff.ID
                    , ID
                    , payoff.Creditor
                    , payoff.PayoffStatusId
                    , payoff.Amount
                    , payoff.POC
                    , payoff.Perdiem
                    , payoff.ExpDate
                    , payoff.Address1
                    , payoff.Address2
                    , payoff.City
                    , payoff.Zip
                    , payoff.AccountNumber
                    , payoff.StateId
                    , payoff.CreatedBy
                    , GetLogXml(logs));
            if (res > 0)
            {
                payoffs = null;
                CalculatorUpdateNeeded = true;
            }
            return res;
        }
        public bool SetPayoffStatusWithLog(Payoff payoff, int statusId, ArrayList logs)
        {
            int res = db.ExecuteScalarInt(SAVEMORTGAGEPROFILEPAYOFFSTATUS,
                payoff.ID,
                statusId,
                ID,
                GetLogXml(logs));
            if (res == 1)
            {
                payoffs = null;
            }
            return res == 1;
        }
        public bool SavePrepaidItemWithLog(MortgagePrepaidItem item, ArrayList logs)
        {
            int res = db.ExecuteScalarInt(SAVEMORTGAGEPROFILEPREPAIDITEM
                    , item.ID
                    , ID
                    , item.Description
                    , item.PaymentTo
                    , item.Amount
                    , item.UnitId
                    , item.StatementStart
                    , item.StatementEnd
                    , currentUserId
                    , GetLogXml(logs));
            if (res > 1)
            {
                mortgageprepaids = null;
            }
            return res == 1;
        }
        public bool SavePaymentWithLog(PayingInAdvance advancePayment, ArrayList logs)
        {
            return db.ExecuteScalarInt(SAVEMORTGAGEPROFILEADVANCEDPAYMENT
                    , advancePayment.ID
                    , advancePayment.MortgageId
                    , advancePayment.DescriptionId
                    , advancePayment.Description
                    , advancePayment.PayingTo
                    , advancePayment.Amount
                    , advancePayment.UnitId
                    , currentUserId
                    , GetLogXml(logs)) == 1;
        }
        public bool SavePropertyRepairItemWithLog(PropertyRepairItem item, ArrayList logs)
        {
            return db.ExecuteScalarInt(SAVEPROPERTYREPAIRITEM
                    , item.ID
                    , ID
                    , item.PropertyId
                    , item.Description
                    , item.BidAmount
                    , item.RepairStatusId
                    , item.EstimateSourceId
                    , currentUserId
                    , GetLogXml(logs)) == 1;
        }

        #endregion


        #endregion

        #region private
        public bool CheckClosingCosts()
        { 
            bool res = false;
            if (Invoices.Count > 0)
            {
                Hashtable tbl = new Hashtable(); 
                for (int i = 0; i < Invoices.Count; i++)
                {
                    int typeId = Invoices[i].TypeID;
                    if(!tbl.Contains(typeId))
                    {
                        tbl.Add(typeId,true);
                    }
                }
                DataView dv = db.GetDataView(GETREQUIREDTOCLOSEFEETYPES);
                for(int i=0;i<dv.Count;i++)
                {
                    int typeId= int.Parse(dv[i]["feetypeid"].ToString());
                    if(!tbl.Contains(typeId))
                    {
                        return false;
                    }
                }
                res = true;
            }
            return res;
        }
        public bool IsFieldRequired(string propertyName)
        {
            bool res = false;
            if(RequiredFields.ContainsKey(propertyName))
            {
                int statusId = (int) RequiredFields[propertyName];
//                res = statusId <= curProfileStatusID;
                res = CheckStatusValue(statusId);
            }
            return res;
        }
        private bool CheckStatusValue(int statusId)
        {
            bool res = statusId <= curProfileStatusID;
            if(curProfileStatusID==MANAGEDLEADSTATUSID)
            {
                res = statusId == curProfileStatusID;
            }
            return res;
        }

        private Hashtable GetRequiredFieldLists()
        {
            Hashtable ht = new Hashtable();
            DataView dv = currentPage.GetRequiredFields();
            for (int i = 0; i < dv.Count;i++ )
            {
                string key = dv[i]["propertyname"].ToString();
                int statusId = int.Parse(dv[i]["statusId"].ToString());
                ht.Add(key,statusId);
            }
            return ht;
        }

        //private ArrayList GetRequiredFields()
        //{
        //    ArrayList res = new ArrayList();
        //    DataTable dt = currentPage.GetDictionaryTableByProcedure(GETREQUIREDFIELDS);
        //    DataView dv = dt.DefaultView;
        //    dv.RowFilter = String.Format(REQUIREDSTATUSFILTER, curProfileStatusID);
        //    for (int i = 0; i < dv.Count; i++)
        //    {
        //        res.Add(dv[i][PROPERTYNAMEFIELDNAME].ToString());
        //    }
        //    return res;
        //}

        private ArrayList GetMortgageProfileFieldsForObject(string objectname, int userid)
        {
            ArrayList res = new ArrayList();
            DataView dv = db.GetDataView(GETMORTGAGEPROFILEFIELDSFOROBJECT, objectname, userid, curProfileStatusID);
            for (int i = 0; i < dv.Count; i++)
            {
                MortgageProfileField mpf = new MortgageProfileField(dv[i]);
                res.Add(mpf);
            }
            return res;
        }
        public string GetFilterValue(MortgageProfileFieldInfo mpfi, BaseObject replaceObject)
        {
            string filterValue = mpfi.FilterValue;
            try
            {
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"\{\w+\.\w+\}");
                System.Text.RegularExpressions.Match m = reg.Match(filterValue);
                while (m.Success)
                {
                    if (m.Captures.Count > 0)
                    {
                        string filter = m.Captures[0].Value;
                        string[] names = m.Captures[0].Value.Substring(1, m.Captures[0].Value.Length - 2).Split('.');
                        if (names.Length == 2)
                        {
                            string objectName = names[0];
                            string propertyName = names[1];
                            object objectValue = GetType().GetProperty(objectName).GetValue(this, null);
                            if (objectValue is IList)
                                objectValue = replaceObject;
                            object propertyValue = objectValue.GetType().GetProperty(propertyName).GetValue(objectValue, null);

                            filterValue = filterValue.Replace(filter, propertyValue.ToString());
                        }
                    }

                    m = m.NextMatch();
                }
            }
            catch { }
            return filterValue;
        }
        public string GetFilterValue(string filterExpr, BaseObject replaceObject)
        {
            string filterValue = filterExpr;
            try
            {
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"\{\w+\.\w+\}");
                System.Text.RegularExpressions.Match m = reg.Match(filterValue);
                while (m.Success)
                {
                    if (m.Captures.Count > 0)
                    {
                        string filter = m.Captures[0].Value;
                        string[] names = m.Captures[0].Value.Substring(1, m.Captures[0].Value.Length - 2).Split('.');
                        if (names.Length == 2)
                        {
                            string objectName = names[0];
                            string propertyName = names[1];
                            object objectValue = GetType().GetProperty(objectName).GetValue(this, null);
                            if (objectValue is IList)
                                objectValue = replaceObject;
                            object propertyValue = objectValue.GetType().GetProperty(propertyName).GetValue(objectValue, null);

                            filterValue = filterValue.Replace(filter, propertyValue.ToString());
                        }
                    }

                    m = m.NextMatch();
                }
            }
            catch { }
            return filterValue;
        }

        private Hashtable GetAssignedUsers()
        {
            Hashtable res = new Hashtable();
            DataView dv = db.GetDataView(GETASSIGNEDUSERS, ID);
            if (dv.Count == 0)
            {
                return res;
            }
            for (int i = 0; i < dv.Count; i++)
            {
                int roleId = int.Parse(dv[i][ROLEIDFIELDNAME].ToString());
                int userId = int.Parse(dv[i][USERIDFIELDNAME].ToString());
                res.Add(roleId, userId);
            }
            return res;
        }
        private List<Borrower> GetBorrowers()
        {
            List<Borrower> res = new List<Borrower>();
            if (ID > 0)
            {
                DataView dv = db.GetDataView(GETMORTGAGEBORROWERS, ID);
                if (dv.Count == 0)
                    return res;
                for (int i = 0; i < dv.Count; i++)
                {
                    res.Add(new Borrower(dv[i], this));
                }
            }
            return res;
        }
        private Invoice GetUpfrontMipInvoice()
        {
            Invoice res = new Invoice(0);
            res.mp = this;
            res.TypeName = "UpfrontPremium";
            res.Amount = Calculator.UpFrontPremium;
            return res;
        }

        private List<Invoice> GetInvoicesWithoutAdvancePaid()
        {
            return Invoice.GetInvoiceObjectList(this);
        }
        private DataTable GetInvoiceList()
        {
            return db.GetDataTable("GetInvoicesByMortgageID", ID);
        }

        private List<Invoice> GetInvoicesByFilter(string filter,bool isPoc)
        {
            List<Invoice> list = new List<Invoice>();
            DataView dv = new DataView(DtInvoice);
//            dv.RowFilter = filter + " and statusid" + (isPoc ? "=2" : "<>2");
            dv.RowFilter = filter;
            for (int i = 0; i < dv.Count; i++)
            {
                list.Add(new Invoice(dv[i],this));
            }
            return list;
        }
        private static string GetInvoiceFilter(int feeCategoryId, int typeId)
        {
            return String.Format(INVOICEFILTER, feeCategoryId, typeId);
        }

        private List<Payoff> GetPayoffs()
        {
            return Payoff.GetPayoffObjectList(ID);
        }
        private List<MortgagePrepaidItem> GetMortgagePrePaids()
        {
            return MortgagePrepaidItem.GetObjectList(ID);
        }

        private Borrower GetYoungestBorrower()
        {
            Borrower result = null;
            foreach (Borrower borrower in Borrowers)
            {
                if (result == null)
                {
                    result = borrower;
                }
                else
                {
                    if (borrower.DateOfBirth > result.DateOfBirth)
                    {
                        result = borrower;
                    }
                }
            }
            return result;
        }
        private string GetAssignedUserXml()
        {
            string result = String.Empty;
            if (AssignedUsers == null)
            {
                return result;
            }
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            foreach (Object key in AssignedUsers.Keys)
            {
                XmlNode n = d.CreateElement(ITEMELEMENT);
                XmlAttribute r = d.CreateAttribute(ROLEIDATTRIBUTE);
                r.Value = key.ToString();
                n.Attributes.Append(r);
                XmlAttribute u = d.CreateAttribute(USERIDATTRIBUTE);
                u.Value = AssignedUsers[key].ToString();
                n.Attributes.Append(u);
                root.AppendChild(n);
            }
            if (root.ChildNodes.Count > 0)
            {
                d.AppendChild(root);
                result = d.OuterXml;
            }
            return result;
        }

        #region Document Template Filling
        public MemoryStream GetResPdfStream(string dtIDXml, out string errMsg)
        {
            errMsg = String.Empty;
            if (dtIDXml.Trim().Length == 0)
                return null;

            MemoryStream dtResMemoryStream = null;
            try
            {
                ArrayList dtVersionList = DocTemplateVersion.GetDocTemplateVersionsByIDs(dtIDXml);
                if (dtVersionList.Count == 0)
                    throw new Exception("DB does not contain at least one version of PDF file for selected DocTemplate");

                Hashtable mpFieldHash = MortgageProfileField.GetMortgageProfileFieldByDTIDs(dtIDXml);

                MemoryStream dtOutMemoryStream;
                foreach (DocTemplateVersion dtVersion in dtVersionList)
                {
                    //MemoryStream dtOutMemoryStream = dtVersion.AppendToPDFStream(this, mpFieldHash, dtResMemoryStream);
                    dtOutMemoryStream = dtVersion.AppendToWordStream(this, mpFieldHash, dtResMemoryStream);
                    if (dtResMemoryStream != null)
                        dtResMemoryStream.Close();
                    dtResMemoryStream = dtOutMemoryStream;
                }

                dtOutMemoryStream = DocTemplateVersion.SaveDocAsPDF(dtResMemoryStream);
                if (dtResMemoryStream != null) dtResMemoryStream.Close();
                dtResMemoryStream = dtOutMemoryStream;
            }
            catch (Exception ex)
            {
                if (dtResMemoryStream != null)
                {
                    dtResMemoryStream.Close();
                    dtResMemoryStream = null;
                }

                errMsg = ex.Message;
            }

            return dtResMemoryStream;
        }

        public void PopulateMPField(MortgageProfileField MPField, DocTemplateField dtField)
        {
            try
            {
                string[] propArr = MPField.PropertyName.Split('.');
                object propValue = this;
                Type propType = GetType();
                foreach (string propName in propArr)
                {
                    if (String.IsNullOrEmpty(propName))
                        continue;
                    else if (propValue is IList)
                    {
                        ArrayList objElementList = new ArrayList();
                        foreach (object objElement in (IList)propValue)
                        {
                            object insideObjElement = objElement.GetType().GetProperty(propName).GetValue(objElement, null);
                            objElementList.Add(insideObjElement);
                        }

                        propValue = objElementList;
                        propType = propValue.GetType();
                        break;
                    }

                    PropertyInfo insidePropInfo = propType.GetProperty(propName);
                    object insidePropValue = insidePropInfo == null ? null : insidePropInfo.GetValue(propValue, null);

                    if (insidePropValue != null && !(insidePropValue is IList))
                    {
                        propValue = insidePropValue;
                        propType = propValue.GetType();
                    }
                    else
                    {
                        int groupIndex1 = dtField.GroupIndex1;
                        int groupIndex2 = dtField.GroupIndex2;
                        bool needParse = ((insidePropInfo == null && insidePropValue == null) || insidePropValue is IList) && groupIndex1 < -1;

                        if (needParse)
                        {
                            insidePropValue = GetCustomObject(dtField.MPGroupFieldName);
                            groupIndex1 = groupIndex2;
                            if (insidePropValue != null && !(insidePropValue is IList))
                            {
                                propValue = insidePropValue;
                                propType = propValue.GetType();
                                continue;
                            }
                        }

                        IList ilist = (IList)insidePropValue;

                        if (insidePropValue == null || groupIndex1 >= ilist.Count)
                        {
                            propValue = null;
                            propType = null;
                            break;
                        }
                        else if (groupIndex1 < 0)
                        {
                            propValue = ilist;
                            propType = propValue.GetType();
                        }
                        else
                        {
                            propValue = ilist[groupIndex1];
                            propType = propValue.GetType();
                        }
                    }
                }
                Type mpFieldType = propType;
                if (propValue == null)
                    MPField.FieldValue = null;
                else if (mpFieldType == typeof(int) && MPField.ContainsDictionaryInfo)
                {
                    DataRow[] selectedRows = CurrentPage.GetDictionary(MPField.TableName).Table.Select(String.Format("id={0}", (int)propValue));
                    MPField.FieldValue = selectedRows.Length > 0 ? selectedRows[0][MPField.FieldName].ToString() : null;
                }
                else if (mpFieldType == typeof(bool))
                    MPField.FieldValue = (bool)propValue;
                else if (mpFieldType == typeof(bool?))
                    MPField.FieldValue = propValue == null ? false : (bool)propValue;
                else if (mpFieldType == typeof(ArrayList))
                    MPField.FieldValue = (ArrayList)propValue;
                else if (mpFieldType == typeof(decimal))
                    MPField.FieldValue = ((decimal)propValue).ToString("#,##0.##;#,##0.##;0");
                else if (mpFieldType == typeof(DateTime))
                    MPField.FieldValue = ((DateTime)propValue).ToString("MM/dd/yyyy");
                else if (MPField.ContainsMapFilter && !String.IsNullOrEmpty(propValue.ToString()))
                    MPField.FieldValue = System.Text.RegularExpressions.Regex.Replace(propValue.ToString(), MPField.MapPattern, MPField.MapReplacement);
                else
                    MPField.FieldValue = propValue.ToString();
            }
            catch (Exception ex)
            {
                string errMsg = String.Format("Property={0} template={1} group={2} was not filled successfully", MPField.PropertyName, dtField.DTVFieldName, dtField.MPGroupFieldName);
                MortgageProfileField.MortgageProfileFieldException mpfEx = new MortgageProfileField.MortgageProfileFieldException(errMsg, ex);
                throw mpfEx;
            }
        }
        private object GetCustomObject(string objPropName)
        {
            object resCustomObject = null;
            try
            {
                string[] propArr = objPropName.Split('.');
                Type propType = GetType();
                object propValue = this;
                foreach (string propName in propArr)
                {
                    if (String.IsNullOrEmpty(propName))
                        continue;

                    int? index = null;
                    int beginIndex = propName.IndexOf('[');
                    if (beginIndex >= 0)
                    {
                        string indexInfo = propName.Substring(beginIndex);
                        if (indexInfo.IndexOf(']') < 0)
                            throw new Exception(String.Format("Group property name {0} is invalid", objPropName));
                        indexInfo = indexInfo.TrimStart('[').TrimEnd(']');
                        index = Convert.ToInt32(indexInfo);
                    }

                    string realPropName = index == null ? propName : propName.Substring(0, beginIndex);
                    PropertyInfo insidePropInfo = propType.GetProperty(realPropName);

                    propValue = insidePropInfo.GetValue(propValue, null);
                    if (propValue != null && propValue is IList && index != null)
                    {
                        int ind = (int)index;
                        IList iList = (IList)propValue;
                        propValue = ind >= 0 && ind < iList.Count ? iList[ind] : null;
                    }

                    if (propValue == null)
                        break;

                    propType = propValue.GetType();
                }

                resCustomObject = propValue;
            }
            catch (TargetInvocationException) { }

            return resCustomObject;
        }

        #endregion
        public int UpdateBirthDateCampaigns()
        {
            int res = 0;
            bool updateNeeded = db.ExecuteScalarBool("UpdateBirthDateCampaigns", ID, YoungestBorrower.DateOfBirth);
            if(campaignUpdateNeeded != updateNeeded)
            {
                res = updateNeeded ? 1 : -1;
            }
            campaignUpdateNeeded = updateNeeded;
            return res;
        }
        public int UpdateClosingDateCampaigns()
        {
            int res = 0;
            bool updateNeeded = db.ExecuteScalarBool("UpdateClosingDateCampaigns", ID, MortgageInfo.ClosingDate);
            if (campaignUpdateNeeded != updateNeeded)
            {
                res = updateNeeded ? 1 : -1;
            }
            campaignUpdateNeeded = updateNeeded;
            return res;
        }

        private static string GetLogXml(ArrayList logs)
        {
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            foreach (MortgageLogEntry item in logs)
            {
                XmlNode n = item.GetLogNode(d);
                if (n != null)
                {
                    root.AppendChild(n);
                }
            }
            if (root.ChildNodes.Count == 0)
            {
                return String.Empty;
            }
            d.AppendChild(root);
            return d.OuterXml;
        }
        public void UpdateMPConditionRules(string ruleList)
        {
            db.Execute("UpdateMPConditionRules", ID, ruleList);
        }
        private void SaveRequiredField()
        {
            requiredFieldsData = GetEmptyFieldsString();
            db.ExecuteScalar("SetMortgageRequiredFields", ID, requiredFieldsData);
        }
        private void UpdateCalculatorUpdateNeeded(bool val)
        {
            db.ExecuteScalarInt("UpdateCalculatorUpdateNeeded", ID, val);
        }
        #endregion

        #region Static methods
        public static DataSet GetMortgageCountInDefault(int companyID)
        {
            return db.GetDataSet("GetMortgageCountInDefault", companyID);
        }
        public static DataSet GetMortgageCountInDefault(int companyID, int userID)
        {
            return db.GetDataSet("GetMortgageCountInDefault", companyID, userID);
        }
        public static DataSet GetMortgageCountInStatuses(int companyID)
        {
            return db.GetDataSet("GetMortgageCountInStatuses", companyID);
        }
        public static DataSet GetMortgageCountInStatuses(int companyID, int userID)
        {
            return db.GetDataSet("GetMortgageCountInStatuses", companyID, userID);
        }
        public static DataSet GetMortgageCountInUsers(int companyID)
        {
            return db.GetDataSet("GetMortgageCountInUsers", companyID);
        }
        public static DataSet GetMortgageCountInUsers(int companyID, int roleID)
        {
            return db.GetDataSet("GetMortgageCountInUsers", companyID, roleID);
        }
        public static DataSet GetMortgageCountInUsers(int companyID, int roleID, int userID)
        {
            return db.GetDataSet("GetMortgageCountInUsers", companyID, roleID, userID);
        }
        public static DataSet GetRoleTemplatesByProfileStatus(int statusid)
        {
            return db.GetDataSet("GetRoleTemplatesByProfileStatus", statusid);
        }
        public static int[] MPStatuses()
        {
            int[] result;
            DataTable td = GetStatusList().Table;
            result = new int[td.Rows.Count];
            for (int i = 0; i < td.Rows.Count; i++)
            {
                result[i] = (int)td.Rows[i]["id"];
            }

            return result;
        }
        public static bool CheckNextStatusVisible(int statusid, AppUser user)
        {
            bool res = false;
            DataTable td = GetRoleTemplatesByProfileStatus(statusid).Tables[0];
            for (int i = 0; i < td.Rows.Count; i++)
            {
                if(user.IsInRole((int)td.Rows[i]["RoleTemplateID"])) 
                    res = true;
            }
            return res;
        }
        public static int[] MPStatusesCheckList()
        {
            int[] result;
            DataTable td = GetStatusListForCheckList().Table;
            result = new int[td.Rows.Count];
            for (int i = 0; i < td.Rows.Count; i++)
            {
                result[i] = (int)td.Rows[i]["id"];
            }

            return result;
        }
        public static DataView GetStatusList()
        {
            return db.GetDataView(GETSTATUSLIST);
        }
        public static DataView GetStatusListForCheckList()
        {
            return db.GetDataView(GETSTATUSLISTFORCHECKLIST);
        }
        public static string GetStatusName(int id)
        {
            return db.ExecuteScalarString("GetStatusName", id);
        }
        public static string GetRulesXml(ArrayList ar)
        {
            string result = "";
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement("Root");
            for (int i = 0; i < ar.Count; i++)
            {
                RuleEvaluation re = (RuleEvaluation)ar[i];
                if (re.EvaluationValue)
                {
                    XmlNode n = d.CreateElement("item");
                    XmlAttribute a = d.CreateAttribute("id");
                    a.Value = re.Id.ToString();
                    n.Attributes.Append(a);
                    root.AppendChild(n);
                }
            }
            if (root.ChildNodes.Count > 0)
            {
                d.AppendChild(root);
                result = d.OuterXml;
            }
            return result;
        }
        public static DataView GetPreviousStatusList(int currentStatus)
        {
            return db.GetDataView("GetPreviousStatusList", currentStatus);
        }
        //public static DataView GetFieldGroups()
        //{
        //    return db.GetDataView(GETFIELDSGROUPS);
        //}
        //public static DataView GetFieldGroupsForImport()
        //{
        //    return db.GetDataView(GETFIELDSGROUPSFORIMPORT);
        //}
        //public static DataView GetFieldsByGroupId(int groupId)
        //{
        //    return db.GetDataView(GETFIELDSBYGROUPID, groupId );
        //}
        public static bool IsPropertyList(string fullPropertyName, out Type realType)
        {
            bool res = false;
            realType = null;
            try
            {
                string[] names = fullPropertyName.Split('.');
                if (names.Length < 2) return false;
                PropertyInfo pi = typeof(MortgageProfile).GetProperty(names[0]);
                Type t = pi.PropertyType;
                realType = t;
                if(t.IsGenericType)
                {
                    res = t.Name.StartsWith("List`");
                    realType = t.GetGenericArguments()[0];
                }
                else if(t.IsArray)
                {
                    res = true;
                }
            }
            catch
            {
            }
            return res;
        }

        #endregion
        public static DataView GetNewLoanStatusList()
        {
            return db.GetDataView("GetNewLoanStatusList");
        }
        public static DataView GetLoanGroupList()
        {
            return db.GetDataView("GetLoanGroups");
        }
        public static DataView GetPipeLineData(int groupId,int groupingTypeId, int companyId, int userId, int roleId)
        {
            return db.GetDataView("GetPipeLineData", groupId, groupingTypeId, companyId, userId, roleId);
        }
        public static DataView GetLoanStatusesForPipeline(int groupId)
        {
            return db.GetDataView("GetLoanStatusesForPipeLine", groupId);
        }

        #endregion
    }


}
