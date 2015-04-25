using System;
using System.Data;

namespace LoanStar.Common
{
    public class Company :BaseObject
    {
        #region constants

        #region field names
        public const string COMPANYFIELDNAME = "company";
        public const string IDFIELDNAME = "id";
        public const string LOGOFIELDNAME = "LogoImage";

        public const string POP3SERVERNAME = "POP3Server";
        public const string POP3PORTNAME = "POP3Port";
        public const string POP3_SSL = "POP3SSL";
        public const string POP3USERIDNAME = "POP3USERID";
        public const string POP3PASSWORDNAME = "POP3PASSWORD";

        public const string SMTPSERVERNAME = "SMTPServer"; 
        public const string SMTPPORTNAME = "SMTPPort";
        public const string SMTPUSERIDNAME = "SMTPUSERID";
        public const string SMTPPASSWORDNAME = "SMTPPASSWORD";

        private const string ISORIGINATORFIELDNAME = "IsOriginator";
        private const string ISLENDERFIELDNAME = "IsLender";
        private const string ISSERVICERFIELDNAME = "IsServicer";
        private const string ISINVESTORFIELDNAME = "IsInvestor";
        private const string ISTRUSTEEFIELDNAME = "IsTrustee";
        private const string ISGLOBALSETTINGSFIELDNAME = "isglobalsettings";
        private const string ISRETAILENABLEDFIELDNAME = "isretailenabled";
        private const string ISGLOBALREQUIREDFIELDSFIELDNAME = "IsGlobalRequiredFields";
        private const string PREUNDERWRITINGQUESTIONSFIELDNAME = "PreUnderwritingQuestions";
        private const string ISLEADMANAGEMENTENABLEDFIELDNAME = "IsLeadManagementEnabled";
        
        #endregion

        #region sp names
        private const string GETLIST = "GetCompanyList";
        private const string GETLENDERLIST = "GetLenderCompanyList";
        private const string GETCOMPANYLIST = "GetListCompany";
        private const string GETRMOCOMPANYLIST = "GetRMOCompanyList";        
        private const string LOADCOMPANYBYID = "LoadCompanyById";
//        private const string DELETECOMPANY = "DeleteCompany";
        private const string SAVECOMPANY = "SaveCompany";
        private const string SAVESERVICERANDINVESTOR = "SaveServicerAndInvestor";
        private const string GETSTRUCTURE = "GetCompanyStructure";
        private const string GETAFFILIATES = "GetCompanyAffiliates";
        private const string GETAFFILIATESCANDIDATE = "GetCompanyAffiliatesCandidate";
        private const string REMOVELOGO = "DeleteCompanyLogo";
        private const string GETROLESLIST = "GetCompanyRolesList";
        private const string GETAFFILIATESROLESLIST = "GetCompanyAffiliatesRolesList";
        private const string GETAFFILIATELIST = "GetAffiliateList";
        private const string GETCOMPANYROLES = "GetCompanyRoles";
        private const string GETCOMPANYAFFILIATEROLES = "GetCompanyAffiliateRoles";
        private const string GETCOMPANYAFFILIATESBYROLE = "GetCompanyAffiliateByRole";
        private const string GETCOMPANYLEADCAPMAIGNSETTING = "GetCompanyLeadCampaignSettings";
        private const string GETCOMPANYREQUIREDFIELDSETTINGS = "GetCompanyRequiredFieldsSettings";
        private const string GETCOMPANYROLEBASEDSECURITYSETTINGS = "GetCompanyRoleBasedSecuritySettings";
        private const string CHANGESTATUSCOMPANY = "ChangeCompanyStatus";
        #endregion

        public const int ISORIGINATORBIT = 0x1;
        public const int ISLENDERBIT = 0x2;
        public const int ISSERVICERBIT = 0x4;
        public const int ISINVESTORBIT = 0x8;
        public const int ISTRUSTEEBIT = 0x10;

        #endregion

        #region fields
        private string name = String.Empty;
        private string logoImage = String.Empty;

        private string pop3Server = String.Empty;
        private string _POP3UserID = String.Empty;
        private string _POP3Password = String.Empty;
        private int pop3Port = -1;
        private bool pop3SSL = false;

        private bool isOriginator = false;
        private bool isLender = false;
        private bool isServicer = false;
        private bool isInvestor = false;
        private bool isTrustee = false;
        private bool isGlobalSettings = true;
        private bool isRetailEnabled = false;
        private bool isUsingGlobalRequiredFields = false;

        private string _SMTPServer = String.Empty;
        private int _smtpPort = -1;
        private string _SMTPUserID = String.Empty;
        private string _SMTPPassword = String.Empty;
        private bool preUnderwritingQuestions;
        private string _TestSendTo;
        private bool isLeadManagementEnabled = false;

        public bool IsLeadManagementEnabled
        {
            get { return isLeadManagementEnabled; }
            set { isLeadManagementEnabled = value; }
        }
	    public string TestSendTo
	    {
		    get { return _TestSendTo;}
		    set { _TestSendTo = value;}
	    }

        public string SMTPServer
        {
            get { return _SMTPServer; }
            set { _SMTPServer = value; }
        }

        public string POP3Password
        {
            get { return _POP3Password; }
            set { _POP3Password = value; }
        }

        public string POP3UserID
        {
            get { return _POP3UserID; }
            set { _POP3UserID = value; }
        }

        public string SMTPPassword
        {
            get { return _SMTPPassword; }
            set { _SMTPPassword = value; }
        }

        public string SMTPUserID
        {
            get { return _SMTPUserID; }
            set { _SMTPUserID = value; }
        }

        public int SMTPPort
        {
            get { return _smtpPort; }
            set { _smtpPort = value; }
        }

        #endregion

        #region properties

        public string Name
        {
            get {return name;}
            set {name=value;}
        }
        public string LogoImage
        {
            get { return logoImage; }
            set { logoImage = value; }
        }
        public bool POP3SSL
        {
            get
            {
                return pop3SSL;
            }
            set
            {
                pop3SSL = value;
            }
        }
        public string POP3Server
        {
            get
            {
                return pop3Server;
            }
            set
            {
                pop3Server = value ?? String.Empty;
            }
        }
        public int POP3Port
        {
            get
            {
                return pop3Port;
            }
            set
            {
                pop3Port = value;
            }
        }
        public string POP3PortStr
        {
            get
            {
                return pop3Port < 0 ? String.Empty : pop3Port.ToString();
            }
            set
            {
                pop3Port = String.IsNullOrEmpty(value) ? -1 : Convert.ToInt32(value);
            }
        }
        public string SMTPPortStr
        {
            get
            {
                return _smtpPort < 0 ? String.Empty : _smtpPort.ToString();
            }
            set
            {
                _smtpPort = String.IsNullOrEmpty(value) ? -1 : Convert.ToInt32(value);
            }
        }
        public bool IsOriginator
        {
            get{ return isOriginator;}
            set{ isOriginator = value;}
        }
        public bool IsLender
        {
            get{ return isLender;}
            set{ isLender = value;}
        }
        public bool IsServicer
        {
            get{ return isServicer;}
            set{ isServicer = value;}
        }
        public bool IsInvestor
        {
            get { return isInvestor; }
            set { isInvestor = value; }
        }
        public bool IsTrustee
        {
            get { return isTrustee; }
            set { isTrustee = value; }
        }
        public bool IsGlobalSettings
        {
            get { return isGlobalSettings; }
            set { isGlobalSettings = value; }
        }
        public bool IsRetailEnabled
        {
            get { return isRetailEnabled; }
            set { isRetailEnabled = value; }
        }
        public bool IsUsingGlobalRequiredFields
        {
            get { return isUsingGlobalRequiredFields; }
            set { isUsingGlobalRequiredFields = value; }
        }
        public bool PreUnderwritingQuestions
        {
            get { return preUnderwritingQuestions; }
            set { preUnderwritingQuestions = value; }
        }
        #endregion

        #region constructor
        public Company()
        {
        }
        public Company(int id)
        { 
            ID = id;
            LoadById();
        }
        #endregion
        
        #region public methods
        
        #region instance methods
        public int Save(string productList)
        {
            bool isNew = ID == -1;
            int res = db.ExecuteScalarInt(SAVECOMPANY, ID, name, logoImage, pop3Server, pop3Port, pop3SSL, _smtpPort, SMTPServer
                    , isOriginator, isLender, isServicer, isInvestor, IsTrustee
                    , isGlobalSettings, isRetailEnabled, isUsingGlobalRequiredFields, preUnderwritingQuestions
                    , isLeadManagementEnabled
                    , productList);
            if ((res > 0) && isNew)
            {
                ID = res;
            }
            return res;
        }
        public static bool ChangeStatus(int id)
        {
            return db.ExecuteScalarInt(CHANGESTATUSCOMPANY, id) == 1;
        }
        public bool RemoveLogo()
        {
            bool res = db.ExecuteScalarInt(REMOVELOGO, ID) == 1;
            if(res)
            {
                logoImage = "";
            }
            return res;
        }
        public DataView GetAffiliates()
        {
            return db.GetDataView(GETAFFILIATES, ID);
        }
        public DataView GetAffiliatesCandidate(int roleId)
        {
            return db.GetDataView(GETAFFILIATESCANDIDATE, ID, roleId);
        }
        public DataView GetStructure()
        {
            return db.GetDataView(GETSTRUCTURE, ID);
        }

        public bool SaveServicerInvestorTrustee(int productId,string servicerList, string investorList, string trusteeList)
        {
            return db.ExecuteScalarInt(SAVESERVICERANDINVESTOR, ID, productId,servicerList,investorList,trusteeList) > 0;
        }

        #endregion

        #region static
        public static DataView GetLenderList(bool all)
        {
            return db.GetDataView(GETLENDERLIST, all);
        }
        public static DataView GetList(bool all)
        {
            return db.GetDataView(GETLIST,all);
        }
        public static DataView GetListForGrid(string orderby, string where)
        {
            return db.GetDataView(GETCOMPANYLIST, orderby, where);
        }
        public static DataView GetRMOCompanyList()
        {
            return db.GetDataView(GETRMOCOMPANYLIST);
        }
        public static DataView GetRoleList()
        {
            return db.GetDataView(GETROLESLIST);
        }
        public static DataView GetAffiliatesRoleList()
        {
            return db.GetDataView(GETAFFILIATESROLESLIST);
        }
        public static DataView GetAffiliateList()
        {
            return db.GetDataView(GETAFFILIATELIST);
        }
        public static int GetRolesMask(int _id)
        {
            int res = 0;
            DataView dv = db.GetDataView(GETCOMPANYROLES, _id);
            if(dv.Count==1)
            {
                if(dv[0][ISORIGINATORFIELDNAME]!=DBNull.Value)
                {
                    if(bool.Parse(dv[0][ISORIGINATORFIELDNAME].ToString()))
                    {
                        res += ISORIGINATORBIT;
                    }
                }
            }
            if(dv[0][ISLENDERFIELDNAME]!=DBNull.Value)
            {
                if (bool.Parse(dv[0][ISLENDERFIELDNAME].ToString()))
                {
                    res += ISLENDERBIT;
                }
            }
            if(dv[0][ISSERVICERFIELDNAME]!=DBNull.Value)
            {
                if (bool.Parse(dv[0][ISSERVICERFIELDNAME].ToString()))
                {
                    res += ISSERVICERBIT;
                }
            }
            if(dv[0][ISINVESTORFIELDNAME]!=DBNull.Value)
            {
                if (bool.Parse(dv[0][ISINVESTORFIELDNAME].ToString()))
                {
                    res += ISINVESTORBIT;
                }
            }
            if (dv[0][ISTRUSTEEFIELDNAME] != DBNull.Value)
            {
                if (bool.Parse(dv[0][ISTRUSTEEFIELDNAME].ToString()))
                {
                    res += ISTRUSTEEBIT;
                }
            }
            return res;
        }
        public static int GetAffiliatesMask(int _id)
        {
            int res = 0;
            DataView dv = db.GetDataView(GETCOMPANYAFFILIATEROLES, _id);
            if (dv.Count == 1)
            {
                if (dv[0][ISORIGINATORFIELDNAME] != DBNull.Value)
                {
                    if (int.Parse(dv[0][ISORIGINATORFIELDNAME].ToString())==1)
                    {
                        res += ISORIGINATORBIT;
                    }
                }
            }
            if (dv[0][ISLENDERFIELDNAME] != DBNull.Value)
            {
                if (int.Parse(dv[0][ISLENDERFIELDNAME].ToString())==1)
                {
                    res += ISLENDERBIT;
                }
            }
            if (dv[0][ISSERVICERFIELDNAME] != DBNull.Value)
            {
                if (int.Parse(dv[0][ISSERVICERFIELDNAME].ToString())==1)
                {
                    res += ISSERVICERBIT;
                }
            }
            if (dv[0][ISINVESTORFIELDNAME] != DBNull.Value)
            {
                if (int.Parse(dv[0][ISINVESTORFIELDNAME].ToString()) == 1)
                {
                    res += ISINVESTORBIT;
                }
            }
            return res;
        }
        public static DataView GetCompanyAffiliateByRole(int companyId, int roleId)
        {
            return db.GetDataView(GETCOMPANYAFFILIATESBYROLE, companyId, roleId);
        }
        public static bool IsGlobalRequiredFields(int companyId)
        {
            return db.ExecuteScalarBool(GETCOMPANYREQUIREDFIELDSETTINGS, companyId);
        }
        public static bool IsLeadCapmaignEnabled(int companyId)
        {
            return db.ExecuteScalarBool(GETCOMPANYLEADCAPMAIGNSETTING, companyId);
        }
        public static bool IsGlobalRoleBasedSecurity(int companyId)
        {
            return db.ExecuteScalarBool(GETCOMPANYROLEBASEDSECURITYSETTINGS, companyId);
        }
        #endregion

        #endregion

        #region private methods
        private void LoadById()
        {
            DataView dv = db.GetDataView(LOADCOMPANYBYID, ID);
            if (dv.Count == 1)
            {
                name = dv[0][COMPANYFIELDNAME].ToString();
                logoImage = dv[0][LOGOFIELDNAME].ToString();
                
                pop3Server = ObjectConvert.ConvertToString(dv[0][POP3SERVERNAME], String.Empty);
                pop3Port = ObjectConvert.ConvertToInt(dv[0][POP3PORTNAME], -1);
                pop3SSL = ObjectConvert.ConvertToBool(dv[0][POP3_SSL], false);
                //_POP3UserID = ObjectConvert.ConvertToString(dv[0][POP3USERIDNAME], String.Empty);
                //_POP3Password = ObjectConvert.ConvertToString(dv[0][POP3PASSWORDNAME], String.Empty);
                
                _SMTPServer = ObjectConvert.ConvertToString(dv[0][SMTPSERVERNAME], String.Empty);
                _smtpPort = ObjectConvert.ConvertToInt(dv[0][SMTPPORTNAME], -1);
                //_SMTPUserID = ObjectConvert.ConvertToString(dv[0][SMTPUSERIDNAME], String.Empty);
                //_SMTPPassword = ObjectConvert.ConvertToString(dv[0][SMTPPASSWORDNAME], String.Empty);

                if(dv[0][ISORIGINATORFIELDNAME] != DBNull.Value)
                {
                    isOriginator = bool.Parse(dv[0][ISORIGINATORFIELDNAME].ToString());
                }
                if (dv[0][ISLENDERFIELDNAME] != DBNull.Value)
                {
                    isLender = bool.Parse(dv[0][ISLENDERFIELDNAME].ToString());
                }
                if (dv[0][ISSERVICERFIELDNAME] != DBNull.Value)
                {
                    isServicer = bool.Parse(dv[0][ISSERVICERFIELDNAME].ToString());
                }
                if (dv[0][ISINVESTORFIELDNAME] != DBNull.Value)
                {
                    isInvestor = bool.Parse(dv[0][ISINVESTORFIELDNAME].ToString());
                }
                if (dv[0][ISTRUSTEEFIELDNAME] != DBNull.Value)
                {
                    isTrustee = bool.Parse(dv[0][ISTRUSTEEFIELDNAME].ToString());
                }
                isGlobalSettings = bool.Parse(dv[0][ISGLOBALSETTINGSFIELDNAME].ToString());
                if (dv[0][ISRETAILENABLEDFIELDNAME] != DBNull.Value)
                {
                    isRetailEnabled = bool.Parse(dv[0][ISRETAILENABLEDFIELDNAME].ToString());
                }
                if (dv[0][ISGLOBALREQUIREDFIELDSFIELDNAME] != DBNull.Value)
                {
                    isUsingGlobalRequiredFields = bool.Parse(dv[0][ISGLOBALREQUIREDFIELDSFIELDNAME].ToString());
                }
                if (dv[0][PREUNDERWRITINGQUESTIONSFIELDNAME] != DBNull.Value)
                {
                    preUnderwritingQuestions = bool.Parse(dv[0][PREUNDERWRITINGQUESTIONSFIELDNAME].ToString());
                }
                if (dv[0][ISLEADMANAGEMENTENABLEDFIELDNAME] != DBNull.Value)
                {
                    isLeadManagementEnabled = bool.Parse(dv[0][ISLEADMANAGEMENTENABLEDFIELDNAME].ToString());
                }
            }
            else
            {
                ID = -1;
            }
        }
        #endregion

        public static AppUser GetCompanyAdmin(int companyId, string smtpServer)
        {
            AppUser res = null;
            int userId = db.ExecuteScalarInt("GetCompanyAdminWithEmail", companyId, smtpServer);
            if (userId > 0)
            {
                res = new AppUser(userId);
            }
            return res;
        }

        public static DataView GetUserListForPipeLine(int companyId, int userId, int roleId)
        {
            return db.GetDataView("GetUserListForPipeLine", companyId, userId, roleId);
        }
        public static DataView GetLoanOfficerList(int companyId)
        {
            return db.GetDataView("GetCompanyLoanOfficerList", companyId);
        }
    }
    public class CompanyAffiliate : BaseObject
    {
        public enum AffiliateType
        {
            Originator = 1,
            Lender = 2,
            Servicer = 3,
            Investor = 4,
            Trustee = 5
        }
        #region constants
        private const string GETAFFILIATEBYID = "GetAffiliateById";
        private const string SAVE = "SaveAffiliateCompany";
        private const string DELETE = "DeleteAffiliateCompany";
        private const string COMPANYIDFIELDNAME = "companyid";
        private const string AFFILIATECOMPANYIDFIELDNAME = "affiliatecompanyid";
        private const string COMPANYTYPEIDFIELDNAME = "companytypeid";
        #endregion
        
        #region fields
        private int companyId = -1;
        private int affiliateCompanyId = -1;
        private int affiliatecompanyTypeId = 0;
        #endregion

        #region properties
        public int AffiliatecompanyTypeId
        {
            get { return affiliatecompanyTypeId; }
            set { affiliatecompanyTypeId = value; }
        }
        public int AffiliateCompanyId
        {
            get { return affiliateCompanyId; }
            set { affiliateCompanyId = value; }
        }
        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }
        #endregion

        #region constructor
        public CompanyAffiliate(int _id)
        {
            ID = _id;
            LoadById();
        }
        #endregion

        public int Save()
        {
            return db.ExecuteScalarInt(SAVE, ID, companyId, affiliateCompanyId, affiliatecompanyTypeId);
        }
        private void LoadById()
        {
            if(ID > 0)
            {
                DataView dv = db.GetDataView(GETAFFILIATEBYID, ID);
                if(dv.Count == 1)
                {
                    companyId = int.Parse(dv[0][COMPANYIDFIELDNAME].ToString());
                    affiliateCompanyId = int.Parse(dv[0][AFFILIATECOMPANYIDFIELDNAME].ToString());
                    affiliatecompanyTypeId = int.Parse(dv[0][COMPANYTYPEIDFIELDNAME].ToString());
                }
            }
            else
            {
                ID = -1;
            }
        }
        public static bool Delete(int _id)
        {
            return db.ExecuteScalarInt(DELETE, _id) == 1;
        }
    }
}
