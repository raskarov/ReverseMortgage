using System;
using System.Collections;
using System.Data;
using System.Xml;
using BossDev.CommonUtils;
using WebMailPro;

namespace LoanStar.Common
{

	/// <summary>
	/// Represents a site user. Supports Active Directory.
	/// Implements all security dependent routines so there is no need 
	/// to use any of Page.User properties or methods.
	/// </summary>
	public class AppUser
	{

        public enum UserRoles
        { 
            Administrator = 1,
            LenderAdministrator = 2,
            LoanOfficer = 4,
            LoanOfficerAssistant = 5,
            OperationsManager = 6,
            ProcessingManager = 7,
            Processor = 9,
            UnderwritingManager = 10,
            Underwriter = 11,
            ClosingManager = 12,
            Closer = 13,
            PostClosingManager = 15,
            PostCloser = 16,
            LoanOfficerManager = 17,
            ExecutiveManager = 18
        }
        #region constants

        #region db field names
        public const string IDFIELDNAME = "id";
        public const string LOGINFIELDNAME = "login";
        private const string PASSWORDFIELDNAME = "password";
        public const string COMPANYIDFIELDNAME = "companyid";
        public const string FIRSTNAMEFIELDNAME = "firstname";
        public const string LASTNAMEFIELDNAME = "lastname";
        public const string STATUSNAMEFIELDNAME = "statusname";
        public const string STATUSIDFIELDNAME = "UserStatusId";
        public const string COMPANYSTATUSIDFIELDNAME = "StatusId";
        public const string FIELDIDFIELDNAME = "FieldId";
        public const string COMPANYNAMEFIELDNAME = "company";
        public const string LOGOIMAGEFIELDNAME = "logoimage";
        public const string MAILUSERNAMEFIELDNAME = "MailUserName";
        public const string MAILPASSWORDFIELDNAME = "MailPassword";
        public const string MAILADDRESSFIELDNAME = "MailAddress";
	    public const string PHONEFIELDNAME = "phone";
        public const string FAXFIELDNAME = "fax";
        public const string ADDRESS1FIELDNAME = "address1";
        public const string ADDRESS2FIELDNAME = "address2";
        public const string STATEIDFIELDNAME = "stateid";
        public const string STATEFIELDNAME = "Abbreviation";
        public const string ISORIGINATORFIELDNAME = "isoriginator";
        public const string ISRETAILENABLEDFIELDNAME = "isretailenabled";
        public const string ZIPFIELDNAME = "zip";
        public const string CITYFIELDNAME = "city";
        public const string DISPLAYPHOTOFIELDNAME = "displayphoto";
        public const string PHOTOUPLOADEDFIELDNAME = "photouploaded";
	    public const string USECOMPANYEMAILSETTINGS = "useCompanyEmailSettings";
        public const string EMAILACCOUNTID = "EmailAccountId";
        public const string POP3SERVERFIELDNAME = "POP3Server";
        public const string POP3PORTFIELDNAME = "POP3Port";
        public const string POP3SSLFIELDNAME = "POP3SSL";
        public const string SMTPSERVERFIELDNAME = "SMTPServer";
        public const string SMTPPORTFIELDNAME = "SMTPPort";
        public const string LOGINATTEMPTSFIELDNAME = "LoginAttempts";
	    public const string PRIMARYEMAILFIELDNAME = "primaryEmail";
        public const string LOCKTIMEFIELDNAME = "LockTime";
        public const string PASSWORDCHANGEDATEFIELDNAME = "PasswordChangeDate";
        public const string PREVIOSPASSWORDSFIELDNAME = "PreviosPasswords";
        public const string ISLEADMANAGEMENTENABLEDFIELDNAME = "IsLeadManagementEnabled";
        
        public const string USERPHOTO = "photo_{0}.jpg";

	    public const int LOGINRESULTOK = 0;
        public const int LOGINRESULTWRONGLOGINPASSWORD = 1;
        public const int LOGINRESULTACCOUNTDISABLED = 2;
        public const int LOGINRESULTCOMPANYDISABLED = 3;
        public const int LOGINRESULTACCOUNTLOCKED = 4;

        #endregion

        #region procedure names
        private const string LOADUSERBYNAME = "LoadUserByName";
        private const string LOADUSERBYID = "LoadUserById";
        private const string GETADMINLIST = "GetGlobalAdmins";
        private const string CHANGEUSERSTATUS = "ChangeUserStatus";
        private const string GETSTATUSLIST = "GetStatusList";
        private const string SAVEADMINUSER = "SaveAdminUser";
        private const string SAVEGLOBALADMINUSER = "SaveGlobalAdminUser";
        private const string SAVE            = "SaveUser";
        public const string GETLENDERUSERLIST = "GetLenderUserList";
        public const string GETTASKUSERLIST = "GetTaskUserList";
        public const string GETLENDERUSERLISTFORGRID = "GetLenderUserListForGrid";
        private const string GETROLESFORUSER = "GetRolesForUser";
        private const string GETROLESFORLENDERUSER = "GetRolesForLenderUser";
        private const string GETUSERROLES = "GetUserRolesList";
        private const string GETUSERMAXAUTHORITYLEVEL = "GetUserMaxAuthorityLevel";
        private const string GETUSERMINAUTHORITYLEVEL = "GetUserMinAuthorityLevel";
        public const string DELETE = "DeleteUser";
        public const string GETEDITABLEFIELDS = "GetEditableFields";
        private const string DELETEUSERFROMCOMPANYSTRUCTURE = "DeleteUserFromCompanyStructure";
        public const string SAVEUSERLOG = "SaveUserLog";
        public const string GETUSERLOGS = "GetUserLogs";
        public const string GETUSERLINKS = "GetUserLinks";
        public const string SAVEUSERLINK = "SaveUserLink";
        public const string DELETEUSERLINKS = "DeleteUserLinks";
	    public const string UNLOCKUSER = "UnlockUser";
	    private const string UPDATEPASSWORD = "UpdateUserPassword";
	    private const string GETCOMPANY = "LoadCompanyById";
        #endregion

        public const string LOGINALREADYEXISTS = "Login already exists";
        private const string ROOTELEMENT = "Root";
        private const string ITEMELEMENT = "item";
        private const string IDATTRIBUTE = "id";

        #region rolesid
        public const int LOANSTARADMINROLEID = 1;
        public const int CORRESPONDENTLENDERROLEID = 2;
        public const int LOANOFFICERROLEID = 4;
        public const int LOANOFFICERASSISTANTROLEID = 5;
        public const int OPERATIONSMANAGERROLEID = 6;
        public const int PROCESSINGMANAGERROLEID = 7;
        public const int PROCESSORROLEID = 9;
        public const int UNDERWRITINGMANAGERROLEID = 10;
        public const int UNDERWRITERROLEID = 11;
        public const int CLOSINGMANAGERROLEID = 12;
        public const int CLOSERROLEID = 13;
        public const int POSTCLOSINGMANAGERROLEID = 15;
        public const int POSTCLOSERROLEID = 16;
        public const int LOANMANAGERROLEID = 17;
        public const int EXECUTIVEMANAGERROLEID = 18;
        #endregion

        #endregion

        #region fields
        private static readonly DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);
        private string loginName = String.Empty;
        private string password = String.Empty;
        private string firstName = String.Empty;
        private string lastName = String.Empty;
        private string phone = String.Empty;
        private string fax = String.Empty;
	    private string address1 = String.Empty;
        private string address2 = String.Empty;
        private int stateId = 0;
        private string state = String.Empty;
	    private string zip = String.Empty;
        private string city = String.Empty;
        private int id = -1;
        private int companyId = -1;
        private string companyName = String.Empty;
        private Hashtable roles;
        private string lastError = String.Empty;
        private int[] editableFields;
        private string logoImage = String.Empty;
        private int userLogId;
        private int originatorId = -1;
	    private string originatorName = String.Empty;
        private Status userStatus = Status.Unknown;
        private Status companyStatus = Status.Unknown;
        private bool isOriginatorUser = false;
        private bool isRetailEnabled = false;
        private bool displayPhoto = false;
        private string photoUploaded = String.Empty;
	    private bool useCompanyEmailSettings = true;
	    private int emailAccountId = -1;
	    private string companyPOP3Server = string.Empty;
        private string companySMTPServer = string.Empty;
	    private int companyPOP3Port = -1;
	    private int companySMTPPort = -1;
	    private Account emailAccount;
	    private DateTime? lockTime = null;
	    private int loginAttempts = 0;
	    private DateTime passwordChangeDate;
	    private string[] previosPasswords;
	    private readonly int timeZone = 0;
	    private string primaryEmail = String.Empty;
	    private bool isLeadManagementEnabled = false;
	    private string managerXml = String.Empty;
        #endregion

        #region properties
	    public string ManagerXml
	    {
            get { return managerXml; }
            set { managerXml = value; }
	    }
	    public bool IsLeadManagementEnabled
	    {
            get { return isLeadManagementEnabled; }
	    }
	    public int TimeZone
	    {
            get { return timeZone; }
	    }
	    public string[] PreviosPasswords
	    {
            get { return previosPasswords;}
	    }
	    public bool IsPasswordExpired
	    {
            get { return passwordChangeDate.AddDays(90) < DateTime.Now; }
	    }
	    public DateTime PasswordChangeDate
	    {
            get { return passwordChangeDate; }
	    }
	    public string CompanyPOP3Server
	    {
            get { return companyPOP3Server; }
	    }
        public string CompanySMTPServer
	    {
            get { return companySMTPServer; }
	    }
	    public int CompanyPOP3Port
	    {
            get { return companyPOP3Port; }
	    }
	    public int CompanySMTPPort
	    {
            get { return companySMTPPort; }
	    }
	    public Account EmailAccount
	    {
            get
            {
                if(emailAccount == null)
                {
                    emailAccount = new Account();
                }
                return emailAccount;
            }
	    }
	    public bool UseCompanyEmailSettings
	    {
            get { return useCompanyEmailSettings; }
            set { useCompanyEmailSettings = value; }
	    }
        public string BaydocsUserId
        {
            get { return firstName.Substring(0, 1) + lastName; }
        }
        public string EmailAddress
        {
            get
            {
                if (emailAccount!=null)
                {
                    return emailAccount.Email;
                }
                return String.Empty;
            }
        }
        public string PhotoUploaded
        {
            get { return photoUploaded; }
            set { photoUploaded = value; }
        }
        public bool DisplayPhoto
        {
            get { return displayPhoto; }
            set { displayPhoto = value; }
        }
        public bool IsRetailEnabled 
        {
            get { return isOriginatorUser && IsInRole(LOANOFFICERROLEID) ? isRetailEnabled : false; }
        }
        public bool IsOriginatorUser
        {
            get
            {
                return isOriginatorUser;
            }
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
	    public int StateId
	    {
            get { return stateId; }
            set { stateId = value; }
	    }
	    public string State
	    {
            get 
            {
                string res = state;
                if (stateId<=0)
                {
                    res =  "N/A";
                }
                return res;
            }
	    }
        public string Zip
	    {
            get { return zip; }
            set { zip = value; }
	    }
        public string City
        {
            get { return city; }
            set { city = value; }
        }
	    public string FullAddress
	    {
	        get
	        {
                return (Address1 + " " + Address2).Trim() + ", " + City + ", " + State + ", " + Zip;
	        }
	    }
	    public string Phone
	    {
            get { return phone; }
            set { phone = value; }
	    }
	    public string Fax
	    {
            get { return fax; }
            set { fax = value; }
	    }
        public Status UserStatus
        {
            get { return userStatus; }
        }
        public Status CompanyStatus
        {
            get { return companyStatus; }
        }
	    public bool HasEmail
	    {
            get { return emailAccountId>0; }
	    }
        public string MailUserName
        {
            get
            {
                if(emailAccount!=null)
                {
                    return emailAccount.MailIncomingLogin;
                }
                return String.Empty;
            }
        }
        public string MailPassword
        {
            get
            {
                if(emailAccount!=null)
                {
                    return emailAccount.MailIncomingPassword;
                }
                return String.Empty;
            }
        }
        public string LoginName
        {
            get { return loginName; }
            set { loginName = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = Utils.GetMD5Hash(value); }
        }
	    public string PrimaryEmail
	    {
            get { return primaryEmail; }
            set { primaryEmail = value; }
	    }
        public string FullName
        {
            get 
            {
                string fName = String.IsNullOrEmpty(firstName) ? String.Empty : firstName;
                string lName = String.IsNullOrEmpty(lastName) ? String.Empty : lastName;
                return String.Format("{0} {1}", fName, lName);
            }
        }
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }
        public int OriginatorId
        {
            get { return originatorId; }
            set { originatorId = value; }
        }
        public Hashtable Roles
        {
            get { return roles; }
            set { roles = value; }
        }
        public string LastError
        {
            get { return lastError; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public bool IsLoanStarAdmin
        {
            get
            {
                return (roles != null) ? roles.ContainsKey(LOANSTARADMINROLEID) : false;
            }
            
        }
        public bool IsCorrespondentLenderAdmin
        {
            get 
            {
                return (roles != null) ? roles.ContainsKey(CORRESPONDENTLENDERROLEID) : false;
            }                
        }
        public bool IsAdmin
        {
            get
            {
                return IsLoanStarAdmin || IsCorrespondentLenderAdmin;
            }
        }
        public int[] EditableFields
        {
            get
            {
                if (editableFields == null)
                {
                    editableFields = LoadEditableFields();
                }
                return editableFields;
            }
        }
        public string CompanyName
        {
            get { return companyName; }
        }
        public string LogoImage
        {
            get { return logoImage; }
        }
        public int UserLogId
        {
            get { return userLogId; }
            set { userLogId = value; }
        }
        public bool LoggedAsOriginator
        {
            get
            {
                return (originatorId > 1);
            }
        }
        public int EffectiveCompanyId
        { 
            get
            {
                if ((IsLoanStarAdmin) && LoggedAsOriginator)
                {
                    return originatorId;
                }
                else
                {
                    return companyId;
                }
            }
        }
	    public string OriginatorName
	    {
            get
            {
                if ((IsLoanStarAdmin) && LoggedAsOriginator)
                {
                    return originatorName;
                }
                else
                {
                    return companyName;
                }
            }
            set { originatorName = value; }
	    }
        public string CompaundName
        {
            get { return firstName + " " + lastName; }
        }
        #endregion

        #region constructor
        public AppUser()
	    {
        }
        public AppUser(int id)
        {
            this.id = id;
            if (this.id >= 0)
            {
                LoadById();
            }
            timeZone = AppSettings.DefaultTimeZone;
        }
        #endregion

        #region public methods
        #region instance
        public int Login(string login, string _password, string loginFrom)
        {
            int res = LOGINRESULTWRONGLOGINPASSWORD;
            LoadByLogin(login, _password, loginFrom);

            if (id <= 0)
            {
                id = -1;
            }
            else if(lockTime!=null)
            {
                res = LOGINRESULTACCOUNTLOCKED;
                if (((DateTime)lockTime).AddSeconds(2)< DateTime.Now)
                {
                    NotificationManager notification = new NotificationManager(NotificationManager.WRONGLOGINTYPE);
                    notification.Notify(this);
                }
                id = -1;
            }
            else if (UserStatus != Status.Enabled)
            {
                id = -1;
                res = LOGINRESULTACCOUNTDISABLED;
            }
            else if (CompanyStatus != Status.Enabled)
            {
                id = -1;
                res = LOGINRESULTCOMPANYDISABLED;
            }
            else if(loginAttempts>0)
            {
                id = -1;
            }
            if(id>0)
            {
                res = LOGINRESULTOK;
            }
            return res;
        }
        public int SaveGlobalAdminUser()
        {
            lastError = String.Empty;
            int result = db.ExecuteScalarInt(SAVEGLOBALADMINUSER
                                        , id
                                        , loginName
                                        , password
                                        , firstName
                                        , lastName
                                        );
            if (result == -1)
            {
                lastError = "Login already exists.";
            }
            else if (result == -2)
            {
                lastError = "DB error.";
            }
            return result;
        }
	    public int SaveAdminUser(bool isGlobalAdmin)
        {
            lastError = String.Empty;
            int result = db.ExecuteScalarInt(SAVEADMINUSER
                                        , id
                                        , loginName
                                        , password
                                        , firstName
                                        , lastName
                                        , companyId
                                        , isGlobalAdmin ? Constants.LOANSTARCOMPANYID : CORRESPONDENTLENDERROLEID);
            if(result == -1)
            {
                lastError = "Login already exists.";
            }
            else if(result == -2)
            {
                lastError = "DB error.";
            }
            return result;
        }
        public void UpdatePassword()
        {
            string passwords = db.ExecuteScalarString(UPDATEPASSWORD, loginName, password);
            SetPreviosPasswords(passwords);
            passwordChangeDate = DateTime.Now;
        }
        public int Save()
        {
            lastError = String.Empty;
            int result = 0;
            try
            {
                try
                {
                    if (!String.IsNullOrEmpty(emailAccount.MailIncomingLogin) && (!String.IsNullOrEmpty(emailAccount.MailOutgoingLogin)))
                    {
                        WebMailHelper.SaveEmailAccount(emailAccount);
                    }
                    emailAccountId = emailAccount.ID;
                    result = db.ExecuteScalarInt(SAVE
                            , id
                            , loginName
                            , password
                            , firstName
                            , lastName
                            , phone
                            , fax
                            , address1
                            , address2
                            , city
                            , zip
                            , stateId
                            , companyId
                            , displayPhoto
                            , photoUploaded
                            , useCompanyEmailSettings
                            , emailAccountId
                            , primaryEmail
                            , GetRolesXml()
                            , managerXml
                            );
                    if (result == -1)
                    {
                        lastError = LOGINALREADYEXISTS;
                    }
                    else
                    {
                        if(id<=0&&result>0)
                        {
                            id = result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    lastError = ex.Message;
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }
            return result;
        }
	    public bool Delete()
        {
            return (db.ExecuteScalarInt(DELETE, id) == 1);
        }
        public int GetMinutesOffset()
        {
            return WebMailPro.Utils.GetMinutesTimesOffset(timeZone);
        }

	    public bool IsOnlyInRoles(params UserRoles[] incomeRoles)
        {
            foreach (DictionaryEntry roleEntry in roles)
            {
                UserRoles curRole = (UserRoles)roleEntry.Key;
                bool isFound = false;
                foreach (UserRoles incomeRole in incomeRoles)
                    if (incomeRole == curRole)
                    {
                        isFound = true;
                        break;
                    }

                if (!isFound)
                    return false;
            }

            return true;            
        }
        public int MaxRole()
        {
            int max = 0;
            if (roles != null)
            {
                foreach(int key in roles.Keys)
                {
                    if (key > max) max = key;
                }
            }
            return max;
        }
        public int MaxAuthorityLevel()
        {
            return GetUserMaxAuthorityLevel();
        }
        public int MinAuthorityLevel()
        {
            return GetUserMinAuthorityLevel();
        }
        
        public bool IsInRoles(params int[] roleIds)
        {
            for (int i = 0; i < roleIds.Length; i++)
            { 
                if (IsInRole(roleIds[i]))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsInRole(int roleId)
        {
            bool res = false;
            if (roles != null)
            {
                res = roles.ContainsKey(roleId);
            }
            return res;
        }
        public bool IsMortgageManager()
        {
            return IsInRole((int)UserRoles.OperationsManager) || IsInRole((int)UserRoles.ClosingManager)
                || IsInRole((int)UserRoles.LoanOfficerManager) || IsInRole((int)UserRoles.PostClosingManager)
                || IsInRole((int)UserRoles.ProcessingManager) || IsInRole((int)UserRoles.UnderwritingManager);
        }
        public bool Disable()
        {
            return ChangeStatus(Constants.DISABLEDSTATUSID);
        }
        public bool Enable()
        {
            return ChangeStatus(Constants.ENABLEDSTATUSID);
        }
        public bool Unlock()
        {
            return (db.ExecuteScalarInt(UNLOCKUSER, id) == 1);
        }
        public string GetDefaultPage()
        {
            string result = Constants.LOGINPAGENAME;
            if (id >= 0)
            {
                result = IsAdmin ? Constants.ADMINPAGENAME : Constants.PUBLICPAGENAME;
            }
            return result;
        }
        public DataView GetAllPossibleRoles()
        {
            return db.GetDataView(GETROLESFORUSER, id);
        }
        public DataView GetAllLenderRoles()
        {
            return db.GetDataView(GETROLESFORLENDERUSER, id);
        }
        public int GetUserMaxAuthorityLevel()
        {
            return db.ExecuteScalarInt(GETUSERMAXAUTHORITYLEVEL, id);
        }
        public int GetUserMinAuthorityLevel()
        {
            return db.ExecuteScalarInt(GETUSERMINAUTHORITYLEVEL, id);
        }            
        public DataView GetUserRoles(int roleid)
        {
            return db.GetDataView(GETUSERROLES, id, roleid);
        }            
        public void SaveUserLog(string ipaddress, DateTime loginTime)
        {
            userLogId = db.ExecuteScalarInt(SAVEUSERLOG, userLogId, id, ipaddress, DateTime.Now);
        }
        public void SaveUserLog(string ipaddress, DateTime loginTime, DateTime logoutTime)
        {
            userLogId = db.ExecuteScalarInt(SAVEUSERLOG, userLogId, id, ipaddress, DateTime.Now);
        }
        public void SaveUserLog(DateTime LastAccessTime)
        {
            if (userLogId>0) db.ExecuteScalarInt(SAVEUSERLOG, userLogId, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, LastAccessTime);
        }
        public void SaveUserLog(int LogId, DateTime LogoutTime)
        {
            if (LogId>0) db.ExecuteScalarInt(SAVEUSERLOG, LogId, DBNull.Value, DBNull.Value, DBNull.Value, LogoutTime, DBNull.Value);
        }
        public DataView GetUserLogs()
        {
            return db.GetDataView(GETUSERLOGS, companyId);
        }
        public DataView GetUserLinks(int companyid)
        {
            return db.GetDataView(GETUSERLINKS, companyid);
        }
        public void SaveUserLink(int linkId, int companyid, string title, string url, string description)
        {
            db.ExecuteScalarInt(SAVEUSERLINK, linkId, companyid, title, url, description);
        }
        public bool DeleteUserLink(int linkId)
        {
            return db.ExecuteScalarInt(DELETEUSERLINKS, linkId) == 1;
        }
        public void LoadCompanyEmailSettings()
        {
            DataView dv = db.GetDataView(GETCOMPANY, companyId);
            if(dv!=null&&dv.Count==1)
            {
                if (dv[0][POP3SERVERFIELDNAME] != DBNull.Value)
                {
                    companyPOP3Server = dv[0][POP3SERVERFIELDNAME].ToString();
                }
                if (dv[0][SMTPSERVERFIELDNAME] != DBNull.Value)
                {
                    companySMTPServer = dv[0][SMTPSERVERFIELDNAME].ToString();
                }
                if (dv[0][POP3PORTFIELDNAME] != DBNull.Value)
                {
                    companyPOP3Port = int.Parse(dv[0][POP3PORTFIELDNAME].ToString());
                }
                if (dv[0][SMTPPORTFIELDNAME] != DBNull.Value)
                {
                    companySMTPPort = int.Parse(dv[0][SMTPPORTFIELDNAME].ToString());
                }
            }
        }

	    #endregion

        #region static
        public static DataView GetStatusList(bool all)
        {
            return db.GetDataView(GETSTATUSLIST,all);
        }
        //public static DataView GetAdminList(string orderby, string whereclause)
        //{
        //    return db.GetDataView(GETADMINLIST, orderby, whereclause);
        //}
        public static DataView GetAdminList()
        {
            return db.GetDataView(GETADMINLIST);
        }

        public static DataView GetLenderUserListForGrid(int lenderid)
        {
            return db.GetDataView(GETLENDERUSERLISTFORGRID, lenderid);
        }
        public static DataView GetTaskUserList(int lenderid, int mortgageid)
        {
            return db.GetDataView(GETTASKUSERLIST, lenderid, mortgageid);
        }
        public static DataView GetLenderUserList(int lenderid, string orderby, string whereclause)
        {
            return db.GetDataView(GETLENDERUSERLIST, lenderid, orderby, whereclause);
        }
        public static bool DeleteFromCompanyStructure(int nodeId)
        {
            return db.ExecuteScalarInt(DELETEUSERFROMCOMPANYSTRUCTURE, nodeId) == 1;
        }

	    

	    #endregion
        #endregion

        #region private
        private int[] LoadEditableFields()
        {
            DataView dv = db.GetDataView(GETEDITABLEFIELDS, id);
            if (dv.Count == 0)
            {
                return null;
            }
            int[] result = new int[dv.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = int.Parse(dv[i][FIELDIDFIELDNAME].ToString());
            }
            return result;
        }
	    private string GetRolesXml()
        {
            if ((Roles==null) ||(Roles.Count==0))
            {
                return String.Empty;
            }
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            foreach(DictionaryEntry item in Roles)
            {
                XmlNode n = d.CreateElement(ITEMELEMENT);
                XmlAttribute a = d.CreateAttribute(IDATTRIBUTE);
                a.Value = item.Key.ToString();
                n.Attributes.Append(a);
                root.AppendChild(n);
            }
            d.AppendChild(root);
            return d.OuterXml;
        }
	    private bool ChangeStatus(int statusId)
        {
            return (db.ExecuteScalarInt(CHANGEUSERSTATUS, id, statusId) == 1);
        }
        private void LoadById()
        {
            DataSet ds = db.GetDataSet(LOADUSERBYID, id);
            if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                Load(ds.Tables);
            }
        }
        private void LoadByLogin(string login, string  _password, string loginFrom)
        {
            id = -1;
            DataSet ds = db.GetDataSet(LOADUSERBYNAME, login, Utils.GetMD5Hash(_password), loginFrom);
            if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                Load(ds.Tables);
            }
        }
        private void Load(DataTableCollection data)
        {
            LoadProperties(data[0].Rows[0]);
            if (data.Count == 2)
            {
                LoadRoles(data[1]);
            }
        }
        private void LoadProperties(DataRow dr)
        {
            id = int.Parse(dr[IDFIELDNAME].ToString());
            loginName = dr[LOGINFIELDNAME].ToString();
            password = dr[PASSWORDFIELDNAME].ToString();
            companyId = int.Parse(dr[COMPANYIDFIELDNAME].ToString());
            firstName = dr[FIRSTNAMEFIELDNAME].ToString();
            lastName = dr[LASTNAMEFIELDNAME].ToString();
            companyName = dr[COMPANYNAMEFIELDNAME].ToString();
            logoImage = dr[LOGOIMAGEFIELDNAME].ToString();
            if (dr.Table.Columns.Contains(STATUSIDFIELDNAME))
                userStatus = (Status)ObjectConvert.ConvertToInt(dr[STATUSIDFIELDNAME], 0);

            if (dr.Table.Columns.Contains(COMPANYSTATUSIDFIELDNAME))
                companyStatus = (Status)ObjectConvert.ConvertToInt(dr[COMPANYSTATUSIDFIELDNAME], 0);
            if(dr.Table.Columns.Contains(PHONEFIELDNAME))
            {
                phone = dr[PHONEFIELDNAME].ToString();
            }
            if (dr.Table.Columns.Contains(FAXFIELDNAME))
            {
                fax = dr[FAXFIELDNAME].ToString();
            }
            address1 = dr[ADDRESS1FIELDNAME].ToString();
            address2 = dr[ADDRESS2FIELDNAME].ToString();
            zip = dr[ZIPFIELDNAME].ToString();
            city = dr[CITYFIELDNAME].ToString();
            stateId = int.Parse(dr[STATEIDFIELDNAME].ToString());
            state = dr[STATEFIELDNAME].ToString();
            if (dr[ISORIGINATORFIELDNAME] != DBNull.Value)
            {
                isOriginatorUser = bool.Parse(dr[ISORIGINATORFIELDNAME].ToString());
            }
            if (dr[ISRETAILENABLEDFIELDNAME] != DBNull.Value)
            {
                isRetailEnabled = bool.Parse(dr[ISRETAILENABLEDFIELDNAME].ToString());
            }
            if (dr[DISPLAYPHOTOFIELDNAME] != DBNull.Value)
            {
                displayPhoto = bool.Parse(dr[DISPLAYPHOTOFIELDNAME].ToString());
            }
            if (dr[PHOTOUPLOADEDFIELDNAME] != DBNull.Value)
            {
                photoUploaded = dr[PHOTOUPLOADEDFIELDNAME].ToString();
            }
            if (dr[USECOMPANYEMAILSETTINGS] != DBNull.Value)
            {
                useCompanyEmailSettings = bool.Parse(dr[USECOMPANYEMAILSETTINGS].ToString());
            }
            if (dr[EMAILACCOUNTID] != DBNull.Value)
            {
                emailAccountId = int.Parse(dr[EMAILACCOUNTID].ToString());
            }
            if(emailAccountId>0)
            {
                emailAccount = WebMailHelper.GetAccoun(emailAccountId);
            }
            else
            {
                emailAccount = new Account();
            }
            if (dr[POP3SERVERFIELDNAME] != DBNull.Value)
            {
                companyPOP3Server = dr[POP3SERVERFIELDNAME].ToString();
            }
            if (dr[SMTPSERVERFIELDNAME] != DBNull.Value)
            {
                companySMTPServer = dr[SMTPSERVERFIELDNAME].ToString();
            }
            if (dr[POP3PORTFIELDNAME] != DBNull.Value)
            {
                companyPOP3Port = int.Parse(dr[POP3PORTFIELDNAME].ToString());
            }
            if (dr[SMTPPORTFIELDNAME] != DBNull.Value)
            {
                companySMTPPort = int.Parse(dr[SMTPPORTFIELDNAME].ToString());
            }
            //if (dr[POP3SSLFIELDNAME] != DBNull.Value)
            //{
            //    companyPOP3SSL = bool.Parse(dr[POP3SSLFIELDNAME].ToString());
            //}
            if (dr[LOCKTIMEFIELDNAME] != DBNull.Value)
            {
                lockTime = DateTime.Parse(dr[LOCKTIMEFIELDNAME].ToString());
            }
            if (dr[LOGINATTEMPTSFIELDNAME] != DBNull.Value)
            {
                loginAttempts = int.Parse(dr[LOGINATTEMPTSFIELDNAME].ToString());
            }
            if (dr[PRIMARYEMAILFIELDNAME] != DBNull.Value)
            {
                primaryEmail = dr[PRIMARYEMAILFIELDNAME].ToString();
            }
            if (dr[PASSWORDCHANGEDATEFIELDNAME] != DBNull.Value)
            {
                passwordChangeDate = DateTime.Parse(dr[PASSWORDCHANGEDATEFIELDNAME].ToString());
            }
            string passwords = "";
            if (dr[PREVIOSPASSWORDSFIELDNAME] != DBNull.Value)
            {
                passwords = dr[PREVIOSPASSWORDSFIELDNAME].ToString();
            }
            SetPreviosPasswords(passwords);
            if (dr[ISLEADMANAGEMENTENABLEDFIELDNAME] != DBNull.Value)
            {
                isLeadManagementEnabled = bool.Parse(dr[ISLEADMANAGEMENTENABLEDFIELDNAME].ToString());
            }

            if(useCompanyEmailSettings&&emailAccount!=null)
            {
                emailAccount.MailIncomingHost = companyPOP3Server;
                emailAccount.MailIncomingPort = companyPOP3Port;
                emailAccount.MailOutgoingHost = companySMTPServer;
                emailAccount.MailOutgoingPort = companySMTPPort;
                emailAccount.MailOutgoingAuthentication = true;
            }
        }
        private void SetPreviosPasswords(string passwords)
        {
            previosPasswords = new string[4];
            for(int i=0;i<previosPasswords.Length; i++)
            {
                previosPasswords[i] = "";
            }
            int k = 0;
            while (!String.IsNullOrEmpty(passwords)&&k<4)
            {
                int len = 32;
                if(passwords.Length<len)
                {
                    len = passwords.Length;
                }
                string s = passwords.Substring(0, len);
                previosPasswords[k] = s;
                passwords = passwords.Substring(len);
                k++;
            }
        }

	    private void LoadRoles(DataTable dt)
        {
            roles = new Hashtable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Role role = new Role(dt.Rows[i]);
                roles.Add(role.Id, role.Name);
            }
        }
        #endregion
	}
}
