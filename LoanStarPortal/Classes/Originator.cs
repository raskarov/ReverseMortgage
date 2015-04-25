using System.Data;
using System;
using System.Collections;

namespace LoanStar.Common
{
    public class Originator : BaseObject
    {
        #region constants
        private const string LOADBYCOMPANYID = "LoadOriginatorByCompanyId";
        private const string SAVE = "SaveOriginatorGeneralInfo";
        private const string GETORIGINATORSTATES = "GetOriginatorStates";
        private const string GETPRODUCTLISTFORCLOSING = "GetOriginatorProductListForClosing";
        private const string SAVECLOSINGPRODUCT = "SaveOriginatorProductForClosing";
        #endregion

        #region fields
        private int companyId = 0;
        private int stateId = -1;
        private bool preUnderwritingQuestions;
        private Hashtable stateSpecificData;
        private OriginatorStateSpecific currentStateData;

        private int counsDaysNotifyMeExp = 0;
        private int titleDaysNotifyMeExp = 0;
        private int appraisalDaysNotifyMeExp = 0;
        private int pestDaysNotifyMeExp = 0;
        private int bidDaysNotifyMeExp = 0;
        private int waterTestDaysNotifyMeExp = 0;
        private int septicInspDaysNotifyMeExp = 0;
        private int oilTankInspDaysNotifyMeExp = 0;
        private int roofInspDaysNotifyMeExp = 0;
        private int floodCertDaysNotifyMeExp = 0;
        private int creditReportDaysNotifyMeExp = 0;
        private int ldpDaysNotifyMeExp = 0;
        private int eplsDaysNotifyMeExp = 0;
        private int caivrsDaysNotifyMeExp = 0;
        private string baydocsID;

        private bool isCompanyCloseLoans = false;
        private string closingName = String.Empty;
        private string closingAddress = String.Empty;
        private string closingCity = String.Empty;
        private int closingStateId = 0;
        private string closingZip = String.Empty;
        private string closingPhoneNumber = String.Empty;
        private int closingStateOfIncId = 0;
        #endregion

        #region properties
        public bool IsCompanyCloseLoans
        {
            get { return isCompanyCloseLoans; }
            set { isCompanyCloseLoans = value; }
        }
        public string ClosingName
        {
            get { return closingName; }
            set { closingName=value;}
        }
        public string ClosingAddress
        {
            get { return closingAddress; }
            set { closingAddress=value;}
        }
        public string ClosingCity
        {
            get { return closingCity; }
            set { closingCity = value;}
        }
        public int ClosingStateId
        {
            get { return closingStateId; }
            set { closingStateId = value;}
        }
        public string ClosingZip
        {
            get { return closingZip; }
            set { closingZip =value;}
        }
        public string ClosingPhoneNumber
        {
            get { return closingPhoneNumber; }
            set { closingPhoneNumber = value;}
        }
        public int ClosingStateOfIncId
        {
            get { return closingStateOfIncId; }
            set { closingStateOfIncId = value; }
        }
        public string BaydocsID
        {
            get { return baydocsID; }
            set { baydocsID = value; }
        }
        public int CounsDaysNotifyMeExp
        {
            get { return counsDaysNotifyMeExp; }
            set { counsDaysNotifyMeExp = value; }
        }
        public int TitleDaysNotifyMeExp
        {
            get { return titleDaysNotifyMeExp; }
            set { titleDaysNotifyMeExp = value; }
        }
        public int AppraisalDaysNotifyMeExp
        {
            get { return appraisalDaysNotifyMeExp; }
            set { appraisalDaysNotifyMeExp = value; }
        }
        public int PestDaysNotifyMeExp
        {
            get { return pestDaysNotifyMeExp; }
            set { pestDaysNotifyMeExp = value; }
        }
        public int BidDaysNotifyMeExp
        {
            get { return bidDaysNotifyMeExp; }
            set { bidDaysNotifyMeExp = value; }
        }
        public int WaterTestDaysNotifyMeExp
        {
            get { return waterTestDaysNotifyMeExp; }
            set { waterTestDaysNotifyMeExp = value; }
        }
        public int SepticInspDaysNotifyMeExp
        {
            get { return septicInspDaysNotifyMeExp; }
            set { septicInspDaysNotifyMeExp = value; }
        }
        public int OilTankInspDaysNotifyMeExp
        {
            get { return oilTankInspDaysNotifyMeExp; }
            set { oilTankInspDaysNotifyMeExp = value; }
        }
        public int RoofInspDaysNotifyMeExp
        {
            get { return roofInspDaysNotifyMeExp; }
            set { roofInspDaysNotifyMeExp = value; }
        }
        public int FloodCertDaysNotifyMeExp
        {
            get { return floodCertDaysNotifyMeExp; }
            set { floodCertDaysNotifyMeExp = value; }
        }
        public int CreditReportDaysNotifyMeExp
        {
            get { return creditReportDaysNotifyMeExp; }
            set { creditReportDaysNotifyMeExp = value; }
        }
        public int LDPDaysNotifyMeExp
        {
            get { return ldpDaysNotifyMeExp; }
            set { ldpDaysNotifyMeExp = value; }
        }
        public int EPLSDaysNotifyMeExp
        {
            get { return eplsDaysNotifyMeExp; }
            set { eplsDaysNotifyMeExp = value; }
        }
        public int CaivrsDaysNotifyMeExp
        {
            get { return caivrsDaysNotifyMeExp; }
            set { caivrsDaysNotifyMeExp = value; }
        }
        public bool PreUnderwritingQuestions
        {
            get { return preUnderwritingQuestions; }
            set { preUnderwritingQuestions = value; }
        }
        public int CompanyID
        {
            get { return companyId; }
            set { companyId = value; }
        }
        #region state dependable info
        public OriginatorStateSpecific CurrentStateData
        {
            get 
            {
                if((currentStateData==null)||(currentStateData.StateId!=stateId))
                {
                    if(stateSpecificData.ContainsKey(stateId))
                    {
                        currentStateData = (OriginatorStateSpecific) stateSpecificData[stateId];
                    }
                    else
                    {
                        currentStateData = new OriginatorStateSpecific(ID);
                    }
                }
                return currentStateData;
            }
        }
        public string Address1
        {
            get
            {
                return CurrentStateData.Address1;
            }
            set { CurrentStateData.Address1 = value; }
        }
        public string Address2
        {
            get { return CurrentStateData.Address2; }
            set { CurrentStateData.Address2 = value; }
        }
        public string City
        {
            get { return CurrentStateData.City; }
            set { CurrentStateData.City = value; }
        }
        public string Fax
        {
            get { return CurrentStateData.Fax; }
            set { CurrentStateData.Fax = value; }
        }
        public string FHAOriginatorNumber
        {
            get { return CurrentStateData.FHAOriginatorNumber; }
            set { CurrentStateData.FHAOriginatorNumber = value; }
        }
        public string Name
        {
            get { return CurrentStateData.Name; }
            set { CurrentStateData.Name = value; }
        }
        public string Phone
        {
            get { return CurrentStateData.Phone; }
            set { CurrentStateData.Phone = value; }
        }
        public string State
        {
            get { return CurrentStateData.State; }
        }
        public int StateId
        {
            get { return stateId; }
            set
            {
                stateId = value;
                CurrentStateData.StateId = stateId;
            }
        }
        public string Zip
        {
            get { return CurrentStateData.Zip; }
            set { CurrentStateData.Zip = value; }
        }
        #endregion

        #endregion

        #region constructor
        public Originator()
        {
        }
        public Originator(int _companyId)
        {
            companyId = _companyId;
            LoadByCompanyId();
        }
        #endregion

        #region methods
        public DataView GetProductListForClosing()
        {
            return db.GetDataView(GETPRODUCTLISTFORCLOSING, companyId);
        }

        public bool HasState(int stateId_)
        {
            return stateSpecificData.ContainsKey(stateId_);
        }
        public DataView GetStates()
        {
            return db.GetDataView(GETORIGINATORSTATES, ID);
        }
        public bool SaveClosingProduct(string data)
        {
            return db.ExecuteScalarInt(SAVECLOSINGPRODUCT,companyId, data) > 0;
        }

        public int Save()
        {
            return db.ExecuteScalarInt(SAVE, ID, companyId 
                                    , counsDaysNotifyMeExp, titleDaysNotifyMeExp, appraisalDaysNotifyMeExp
                                    , pestDaysNotifyMeExp, bidDaysNotifyMeExp, waterTestDaysNotifyMeExp
                                    , septicInspDaysNotifyMeExp, oilTankInspDaysNotifyMeExp, roofInspDaysNotifyMeExp
                                    , floodCertDaysNotifyMeExp, creditReportDaysNotifyMeExp, ldpDaysNotifyMeExp
                                    , eplsDaysNotifyMeExp, caivrsDaysNotifyMeExp
                                    , baydocsID
                                    , closingName
                                    , closingAddress
                                    , closingCity
                                    , closingStateId
                                    , closingZip
                                    , closingPhoneNumber
                                    , closingStateOfIncId
                                    , isCompanyCloseLoans
                                );
        }
        private void LoadByCompanyId()
        {
            DataSet ds = db.GetDataSet(LOADBYCOMPANYID, companyId);
            if (ds.Tables.Count > 0) 
            {
                LoadGeneralInfo(ds.Tables[0].DefaultView[0]);
                LoadStateSpecificInf(ds);
            }
            else
            {
                ID = -1;
            }
        }
        private void LoadStateSpecificInf(DataSet ds)
        {
            stateSpecificData = new Hashtable();
            if(ds.Tables.Count==2)
            {
                for(int i=0;i<ds.Tables[1].Rows.Count;i++)
                {
                    OriginatorStateSpecific stateSpecific = new OriginatorStateSpecific(ds.Tables[1].DefaultView[i]);
                    stateSpecificData.Add(stateSpecific.StateId, stateSpecific);
                }
            }
        }
        private void LoadGeneralInfo(DataRowView dr)
        {
            PopulateFromDataRow(dr);
        }
        #endregion

    }
    public class OriginatorStateSpecific : BaseObject
    {

        private const string SAVE = "SaveOriginatorStateSpecificInfo";
        private const string DELETE = "DeleteOriginatorStateSpecificInfo";

        #region fields
        private readonly int originatorId;
        private string address1 = String.Empty;
        private string address2 = String.Empty;
        private string city = String.Empty;
        private string fax = String.Empty;
        private string fhaOriginatorNumber = String.Empty;
        private string name = String.Empty;
        private string phone = String.Empty;
        private int stateId = 0;
        private string state = String.Empty;
        private string zip = String.Empty;
        #endregion

        #region properties
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
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }
        public string FHAOriginatorNumber
        {
            get { return fhaOriginatorNumber; }
            set { fhaOriginatorNumber = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        public string State
        {
            get { return state; }
        }
        public int StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }
        public string Zip
        {
            get { return zip; }
            set { zip = value; }
        }
        #endregion

        #region constructor
        public OriginatorStateSpecific(int originatorId_)
        {
            originatorId = originatorId_;
        }
        public OriginatorStateSpecific(DataRowView dr)
        {
            originatorId = int.Parse(dr["originatorId"].ToString());
            address1 = dr["address1"].ToString();
            address2 = dr["address2"].ToString();
            city = dr["city"].ToString();
            fax = dr["fax"].ToString();
            fhaOriginatorNumber = dr["fhaOriginatorNumber"].ToString();
            name = dr["name"].ToString();
            phone = dr["phone"].ToString();
            zip = dr["zip"].ToString();
            stateId = int.Parse(dr["stateId"].ToString());
            state = dr["state"].ToString();
        }
        #endregion

        #region methods
        public int Save()
        {
            return db.ExecuteScalarInt(SAVE, originatorId , address1, address2, city, fax, fhaOriginatorNumber, name, phone, stateId, zip);
        }
        public int Delete()
        {
            return db.ExecuteScalarInt(DELETE, originatorId, stateId);
        }

        #endregion
    }

    public class OriginatorStateLicense :  BaseObject
    {
        #region constants
        private const string GETSTATELICENSEFORORIGINATOR = "GetOriginatorStateLicenseList";
        private const string GETSTATELISTFORLICENSE = "GetStateListForLicense";
        private const string SAVE = "SaveOriginatorStateLicense";
        private const string DELETE = "DeleteOriginatorStateLicense";
        private const string GETLICENSESTATELIST = "GetLicenseStateList";
        private const string SETLICENSEREQUIREDSTATE = "SetLicenseRequiredForState";
        private const string GETLICENSELISTFORLOANOFFICER = "GetLicenseListForLoanOfficer";
        private const string SAVELOANOFFICERLICENSE = "SaveLoanOfficerStateLicense";
        private const string DELETELOANOFFICERLICENSE = "DeleteLoanOfficerLicense";
        #endregion

        #region fields
        #endregion

        #region proeprties
        #endregion

        #region constructor
        #endregion

        #region methods
        
        #region static
        public static DataView GetStateLicenseList(int originatorId)
        {
            return db.GetDataView(GETSTATELICENSEFORORIGINATOR, originatorId);
        }
        public static DataView GetStateListForLicense(int id, int originatorId)
        {
            return db.GetDataView(GETSTATELISTFORLICENSE, id, originatorId);
        }
        public static int Save(int id, int originatorId, int stateId, string licenseNumber, DateTime dt)
        {
            return db.ExecuteScalarInt(SAVE, id, originatorId, stateId, licenseNumber, dt);
        }
        public static int SaveLoanOfficerLicense(int id, int loanofficerId, string licenseNumber, DateTime dt)
        {
            return db.ExecuteScalarInt(SAVELOANOFFICERLICENSE, id, loanofficerId, licenseNumber, dt);
        }
        public static int Delete(int id)
        {
            return db.ExecuteScalarInt(DELETE, id);
        }
        public static int DeleteLoanOfficerLicense(int stateId, int loanOfficerId)
        {
            return db.ExecuteScalarInt(DELETELOANOFFICERLICENSE, stateId, loanOfficerId);
        }
        public static DataView GetLicenseStateList()
        {
            return db.GetDataView(GETLICENSESTATELIST);
        }
        public static int SetLicenseStateRequired(int stateId, bool isRequired)
        {
            return db.ExecuteScalarInt(SETLICENSEREQUIREDSTATE, stateId, isRequired);
        }
        public static DataView GetStateLicenseListForLoanOfficer(int originatorId, int loanOfficerId)
        {
            return db.GetDataView(GETLICENSELISTFORLOANOFFICER,originatorId, loanOfficerId);
        }

        #endregion

        #endregion
    }
}
