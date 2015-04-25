using System;
using System.Data;
using System.Collections;

namespace LoanStar.Common
{
    public class Lender : BaseObject
    {
        #region constants

        #region field names
        private const string COMPANYIDFIELDNAME = "CompanyId";
        private const string PASTTHREEYEARSERVICEFIELDNAME = "Pastthreeyearservice";
        private const string HOWMANYASSIGNSIDFIELDNAME = "HowManyAssignsId";
        private const string HOWMANYASSIGNSFIELDNAME = "HowManyAssigns";
        private const string INCLUDESASSIGNMENTSIDFIELDNAME = "IncludesAssignmentsId";
        private const string INCLUDESASSIGNMENTSFIELDNAME = "IncludeAssignments";
        private const string PERCENTASSIGGNMENTFIELDNAME = "PercentAssignment";
        private const string SPONSOREAGENTCODEFIELDNAME = "SponsoreAgentCode";
        private const string BLANKINCLUDESIDFIELDNAME = "BlankIncludesId";
        private const string BLANKINCLUDESFIELDNAME = "BlankIncludes";
        private const string WEAREABLEFIELDNAME = "WeAreAble";
        private const string WEASSIGNSERVICINGFIELDNAME = "WeAssignServicing";
        private const string WEDONTSERVICEMORTGAGELOANSFIELDNAME = "WeDontServiceMortgageLoans";
        private const string WEMAYASSIGNFIELDNAME = "WeMayAssign";
        private const string YOUWILLBEINFORMEDFIELDNAME = "YouWillBeInformed";
/*
        private const string SERVICEYOURLOANFIELDNAME = "ServiceYourLoan";
*/
        private const string ADDRESS1FIELDNAME = "Address1";
        private const string ADDRESS2FIELDNAME = "Address2";
        private const string CITYFIELDNAME = "City";
        private const string NAMEFIELDNAME = "Name";
        private const string PHONENUMBERFIELDNAME = "PhoneNumber";
        private const string STATEIDFIELDNAME = "StateId";
        private const string STATEFIELDNAME = "StateName";
        private const string ZIPFIELDNAME = "Zip";
        private const string RECORDEDRETURNTOFIELDNAME = "recordedReturnTo";
        private const string CORPHEADFIELDNAME = "corpHead";
        private const string DEFAULTMORTNOTINSUREDFIELDNAME = "defaultMortNotInsured";
        private const string WSFSNEFIELDNAME = "writtenStatementFromSecretaryNotElegibility";
        #endregion

        #region sp names
        private const string GETFIELDSBYCOMPANYID = "GetLenderSpecificFieldsByCompanyId";
        private const string SAVEFIELDS = "SaveLenderSpecificFields";
        private const string GETLENDERPRODUCTLIST = "GetLenderProductList";
        private const string GETSERVICERLIST = "GetServicerList";
        private const string GETINVESTORLIST = "GetInvestorList";
        private const string GETTRUSTEELIST = "GetTrusteeList";
        private const string GETFHASPONSORID = "GetLenderFhaSponorId";
        private const string DELETEFHASPONSORID = "DeleteLenderFhaSponorId";
        private const string SAVELENDERFHASPONSORID = "SaveLenderFhaSponorId";
        private const string GETLENDERFHASTATES = "GetLenderFHAStates";
        #endregion

        #endregion

        #region fields
        private int companyId;
        private bool pastthreeyearservice;
        private int howManyAssignsId;
        private string howManyAssigns = String.Empty;
        private int includesAssignmentsId;
        private string includesAssignments = String.Empty;
        private double percentAssignment;
        private string sponsoreAgentCode = String.Empty;
        private int blankIncludesId;
        private string blankIncludes = String.Empty;
        private bool weAreAble;
        private bool weAssignServicing;
        private bool weDontServiceMortgageLoans;
        private bool weMayAssign;
        private bool youWillBeInformed;
        private string address1 = String.Empty;
        private string address2 = String.Empty;
        private string city = String.Empty;
        private string name = String.Empty;
        private string phoneNumber = String.Empty;
        private int stateId;
        private string state = String.Empty;
        private string zip = String.Empty;
        private string recordedReturnTo = String.Empty;
        private string corpHead = String.Empty;
        private int defaultMortNotInsured;
        private int writtenStatementFromSecretaryNotElegibility;
        private int operatesUnderJurisdictionID;
        private string sponsorAgentID;

        private string closingFaxNumber = String.Empty;
        private bool ncClosedLoanSeller = false;
        private string lenderMortgageClause = String.Empty;
        private string loginPage;

        private string baydocsLenderID;
        private string baydocsLenderCode;

        private string titleCommitmentInsuredClause;
        private string recordReturnToAddress;
        private string placeOfPaymentAddress;
        private string rtrnFnlTtlPolAddress;
        private string mortgageeClause;
        private string lifeOfLoanClause;
        private string rightToCancelAddress;
        private string abbreviatedName;
        private Hashtable stateSpecificData;
        private int fhaStateId = 0;
        private string fhaSponsorId = String.Empty;
        private int locationId = 0;
        #endregion

        #region properties
        public int LocationId
        {
            get { return locationId; }
            set { locationId = value ; }
        }
        private Hashtable StateSpecificData
        {
            get
            {
                if(stateSpecificData==null)
                {
                    stateSpecificData = new Hashtable();
                }
                return stateSpecificData;
            }
        }
        public string FHASponsorId
        {
            get
            {
                return fhaSponsorId;
            }
            set
            {
                fhaSponsorId = value;
            }
        }
        public int FHAStateId
        {
            get { return fhaStateId; }
            set
            {
                fhaStateId = value;
                if(StateSpecificData.ContainsKey(fhaStateId))
                {
                    fhaSponsorId = StateSpecificData[fhaStateId].ToString();
                }
                else
                {
                    fhaSponsorId = String.Empty;
                }
            }
        }
        public string AbbreviatedName
        {
            get { return abbreviatedName; }
            set { abbreviatedName = value; }
        }
        public string TitleCommitmentInsuredClause
        {
            get { return titleCommitmentInsuredClause; }
            set { titleCommitmentInsuredClause = value; }
        }
        public string RecordReturnToAddress
        {
            get { return recordReturnToAddress; }
            set { recordReturnToAddress = value; }
        }
        public string PlaceOfPaymentAddress
        {
            get { return placeOfPaymentAddress; }
            set { placeOfPaymentAddress = value; }
        }
        public string RtrnFnlTtlPolAddress
        {
            get { return rtrnFnlTtlPolAddress; }
            set { rtrnFnlTtlPolAddress = value; }
        }
        public string MortgageeClause
        {
            get { return mortgageeClause; }
            set { mortgageeClause = value; }
        }
        public string LifeOfLoanClause
        {
            get { return lifeOfLoanClause; }
            set { lifeOfLoanClause = value; }
        }
        public string RightToCancelAddress
        {
            get { return rightToCancelAddress; }
            set { rightToCancelAddress = value; }
        }

        public string BaydocsLenderID
        {
            get { return baydocsLenderID; }
            set { baydocsLenderID = value; }
        }
        public string BaydocsLenderCode
        {
            get { return baydocsLenderCode; }
            set { baydocsLenderCode = value; }
        }
        public string LoginPage
        {
            get { return loginPage; }
            set { loginPage = value; }
        }
        public bool NCClosedLoanSeller
        {
            get { return ncClosedLoanSeller; }
            set { ncClosedLoanSeller = value; }
        }
        public string ClosingFaxNumber
        {
            get { return closingFaxNumber; }
            set { closingFaxNumber = value; }
        }
        public string LenderMortgageClause
        {
            get { return lenderMortgageClause; }
            set 
            {
                string res = value;
                if (res.Length > 2000) res = res.Substring(0, 2000);
                lenderMortgageClause = res; 
            }
        }

        public int CompanyId
        {
            get { return companyId; }
        }
        public bool Pastthreeyearservice
        {
            get { return pastthreeyearservice; }
            set { pastthreeyearservice = value; }
        }
        public int HowManyAssignsId
        {
            get { return howManyAssignsId; }
            set { howManyAssignsId = value; }
        }
        public string HowManyAssigns
        {
            get { return howManyAssigns; }
        }
        public int IncludesAssignmentsId
        {
            get { return includesAssignmentsId; }
            set { includesAssignmentsId = value; }
        }
        public string IncludesAssignments
        {
            get { return includesAssignments; }
        }
        public double PercentAssignment
        {
            get { return percentAssignment; }
            set { percentAssignment = value; }
        }
        public string SponsoreAgentCode
        {
            get { return sponsoreAgentCode; }
            set { sponsoreAgentCode = value; }
        }
        public int BlankIncludesId
        {
            get { return blankIncludesId; }
            set { blankIncludesId = value; }
        }
        public string BlankIncludes
        {
            get { return blankIncludes; }
        }
        public bool WeAreAble
        {
            get { return weAreAble; }
            set { weAreAble = value; }
        }
        public bool WeAssignServicing
        {
            get { return weAssignServicing; }
            set { weAssignServicing = value; }
        }
        public bool WeDontServiceMortgageLoans
        {
            get { return weDontServiceMortgageLoans; }
            set { weDontServiceMortgageLoans = value; }
        }
        public bool WeMayAssign
        {
            get { return weMayAssign; }
            set { weMayAssign = value; }
        }
        public bool YouWillBeInformed
        {
            get { return youWillBeInformed; }
            set { youWillBeInformed = value; }
        }
        public string Address1
        {
            get { return address1; }
            set { address1 = value; }
        }
        public string Address2
        {
            get { return address2; }
            set { address2 = value; }
        }
        public string Address
        {
            get { return address1 + " " + address2; }
        }
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        public int StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }
        public string State
        {
            get { return state; }
        }

        public string Zip
        {
            get { return zip; }
            set { zip = value; }
        }
        public string RecordedReturnTo
        {
            get { return recordedReturnTo; }
            set { recordedReturnTo = value; }
        }
        public string CorpHead
        {
            get { return corpHead; }
            set { corpHead = value; }
        }
        public int DefaultMortNotInsured
        {
            get { return defaultMortNotInsured; }
            set { defaultMortNotInsured = value; }
        }
        public int WrittenStatementFromSecretaryNotElegibility
        {
            get { return writtenStatementFromSecretaryNotElegibility; }
            set { writtenStatementFromSecretaryNotElegibility = value; }
        }
        public int OperatesUnderJurisdictionID
        {
            get { return operatesUnderJurisdictionID; }
            set { operatesUnderJurisdictionID = value; }
        }
        public string SponsorAgentID
        {
            get { return sponsorAgentID; }
            set { sponsorAgentID = value; }
        }
        #endregion

        #region constaructor
        public Lender()
            : this(0)
        { }
        public Lender(int companyId)
        {
            this.companyId = companyId;
            if (companyId > 0)
            {
                LoadByCompanyId();
            }
        }
        #endregion

        #region methods
        public int Save()
        {
            return db.ExecuteScalarInt(SAVEFIELDS
                , companyId
                , pastthreeyearservice
                , howManyAssignsId
                , includesAssignmentsId
                , percentAssignment
                , sponsoreAgentCode
                , blankIncludesId
                , weAreAble
                , weAssignServicing
                , weDontServiceMortgageLoans
                , weMayAssign
                , youWillBeInformed
//                , address1
//                , address2
//                , city
//                , name
                , phoneNumber
//                , stateId
//                , zip
                , locationId
                , recordedReturnTo
                , corpHead
                , defaultMortNotInsured
                , writtenStatementFromSecretaryNotElegibility
                , operatesUnderJurisdictionID
                , sponsorAgentID
                , ncClosedLoanSeller
                , closingFaxNumber
                , lenderMortgageClause
                , loginPage
                , baydocsLenderID
                , baydocsLenderCode
                , titleCommitmentInsuredClause
                , recordReturnToAddress
                , placeOfPaymentAddress
                , rtrnFnlTtlPolAddress
                , mortgageeClause
                , lifeOfLoanClause
                , rightToCancelAddress
                , abbreviatedName
                );
        }
        private void LoadByCompanyId()
        {
            DataSet ds = db.GetDataSet(GETFIELDSBYCOMPANYID, companyId);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count==1)
                {
                    LoadGeneralInfo(ds.Tables[0].DefaultView[0]);
                }
                LoadStateSpecificInf(ds);
            }
        }
        private void LoadStateSpecificInf(DataSet ds)
        {
            stateSpecificData = new Hashtable();
            if (ds.Tables.Count == 2)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    string fhaSponsorId_ = ds.Tables[1].DefaultView[i]["FHASponsorId"].ToString();
                    int stateId_ = int.Parse(ds.Tables[1].DefaultView[i]["StateId"].ToString());
                    stateSpecificData.Add(stateId_, fhaSponsorId_);
                }
            }
        }

        private void LoadGeneralInfo(DataRowView dr)
        {
            companyId = int.Parse(dr[COMPANYIDFIELDNAME].ToString());
            ID = companyId;
            pastthreeyearservice = bool.Parse(dr[PASTTHREEYEARSERVICEFIELDNAME].ToString());
            howManyAssignsId = int.Parse(dr[HOWMANYASSIGNSIDFIELDNAME].ToString());
            howManyAssigns = dr[HOWMANYASSIGNSFIELDNAME].ToString();
            includesAssignmentsId = int.Parse(dr[INCLUDESASSIGNMENTSIDFIELDNAME].ToString());
            includesAssignments = dr[INCLUDESASSIGNMENTSFIELDNAME].ToString();
            percentAssignment = float.Parse(dr[PERCENTASSIGGNMENTFIELDNAME].ToString());
            sponsoreAgentCode = dr[SPONSOREAGENTCODEFIELDNAME].ToString();
            blankIncludesId = int.Parse(dr[BLANKINCLUDESIDFIELDNAME].ToString());
            blankIncludes = dr[BLANKINCLUDESFIELDNAME].ToString();
            weAreAble = bool.Parse(dr[WEAREABLEFIELDNAME].ToString());
            weAssignServicing = bool.Parse(dr[WEASSIGNSERVICINGFIELDNAME].ToString());
            weDontServiceMortgageLoans = bool.Parse(dr[WEDONTSERVICEMORTGAGELOANSFIELDNAME].ToString());
            weMayAssign = bool.Parse(dr[WEMAYASSIGNFIELDNAME].ToString());
            youWillBeInformed = bool.Parse(dr[YOUWILLBEINFORMEDFIELDNAME].ToString());
            address1 = dr[ADDRESS1FIELDNAME].ToString();
            address2 = dr[ADDRESS2FIELDNAME].ToString();
            city = dr[CITYFIELDNAME].ToString();
            name = dr[NAMEFIELDNAME].ToString();
            phoneNumber = dr[PHONENUMBERFIELDNAME].ToString();
            stateId = int.Parse(dr[STATEIDFIELDNAME].ToString());
            state = dr[STATEFIELDNAME].ToString();
            zip = dr[ZIPFIELDNAME].ToString();
            recordedReturnTo = dr[RECORDEDRETURNTOFIELDNAME].ToString();
            corpHead = dr[CORPHEADFIELDNAME].ToString();
            defaultMortNotInsured = int.Parse(dr[DEFAULTMORTNOTINSUREDFIELDNAME].ToString());
            writtenStatementFromSecretaryNotElegibility = int.Parse(dr[WSFSNEFIELDNAME].ToString());
            operatesUnderJurisdictionID = int.Parse(dr["OperatesUnderJurisdictionID"].ToString());
            sponsorAgentID = dr["SponsorAgentID"].ToString();
            ncClosedLoanSeller = Convert.ToBoolean(dr["NCClosedLoanSeller"]);
            closingFaxNumber = dr["ClosingFaxNumber"].ToString();
            lenderMortgageClause = dr["lenderMortgageClause"].ToString();
            loginPage = dr["lenderLoginPage"].ToString();
            baydocsLenderID = dr["baydocsLenderID"].ToString();
            baydocsLenderCode = dr["baydocsLenderCode"].ToString();
            titleCommitmentInsuredClause = dr["titleCommitmentInsuredClause"].ToString();
            recordReturnToAddress = dr["recordReturnToAddress"].ToString();
            placeOfPaymentAddress = dr["placeOfPaymentAddress"].ToString();
            rtrnFnlTtlPolAddress = dr["rtrnFnlTtlPolAddress"].ToString();
            mortgageeClause = dr["mortgageeClause"].ToString();
            lifeOfLoanClause = dr["lifeOfLoanClause"].ToString();
            rightToCancelAddress = dr["rightToCancelAddress"].ToString();
            abbreviatedName = dr["abbreviatedName"].ToString();
            locationId = int.Parse(dr["locationid"].ToString());
        }
        public DataView GetStates(int stateId_)
        {
            return db.GetDataView(GETLENDERFHASTATES, ID, stateId_);
        }

        public DataView GetFhaSponsorId()
        {
            return db.GetDataView(GETFHASPONSORID, ID);
        }
        public int DeleteFhaSponsorId()
        {
            return db.ExecuteScalarInt(DELETEFHASPONSORID, ID, fhaStateId);
        }
        public int SaveFhaSponsorId()
        {
            return db.ExecuteScalarInt(SAVELENDERFHASPONSORID, ID, fhaStateId, fhaSponsorId);
        }

        public static DataView GetLenderProductList(int lenderId)
        {
            return db.GetDataView(GETLENDERPRODUCTLIST, lenderId);
        }
        public static DataView GetServicerList(int lenderId,int productId)
        {
            return db.GetDataView(GETSERVICERLIST, lenderId, productId);
        }
        public static DataView GetInvestorList(int lenderId, int productId)
        {
            return db.GetDataView(GETINVESTORLIST, lenderId, productId);
        }
        public static DataView GetTrusteeList(int lenderId, int productId)
        {
            return db.GetDataView(GETTRUSTEELIST, lenderId, productId);
        }
        #endregion

    }
}
