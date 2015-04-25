using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Xml;
using System.Collections;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for SettingsManagement.
	/// </summary>
	[Serializable]
	public class WebmailSettings
	{
		#region Fields
		protected WebmailSettings _instance;

		// WebMailPro Settings
		protected string _siteName;
		protected string _licenseKey;
		protected string _adminPassword;
		protected SupportedDatabase _dbType;
		protected string _dbLogin;
		protected string _dbPassword;
		protected string _dbName;
		protected string _dbDsn;
		protected string _dbHost;
		protected string _dbPathToMdb;
		protected bool _useCustomConnectionString;
		protected string _dbCustomConnectionString;
		protected string _dbPrefix;
		protected IncomingMailProtocol _incomingMailProtocol;
		protected string _incomingMailServer;
		protected int _incomingMailPort;
		protected string _outgoingMailServer;
		protected int _outgoingMailPort;
		protected bool _reqSmtpAuth;
		protected bool _allowAdvancedLogin;
		protected LoginMode _hideLoginMode;
		protected string _defaultDomainOptional;
		protected bool _showTextLabels;
		protected bool _automaticCorrectLoginSettings;
		protected bool _enableLogging;
		protected bool _disableErrorHandling;
		protected bool _allowAjax;
		protected short _mailsPerPage;
		protected long _attachmentSizeLimit;
		protected bool _enableAttachmentSizeLimit;
		protected long _mailboxSizeLimit;
		protected bool _enableMailboxSizeLimit;
		protected short _defaultTimeOffset;
		protected bool _allowUsersChangeTimeOffset;
		protected int _defaultUserCharset;
		protected bool _allowUsersChangeCharset;
		protected string _defaultSkin;
		protected bool _allowUsersChangeSkin;
		protected string _defaultLanguage;
		protected bool _allowUsersChangeLanguage;
		protected bool _allowDhtmlEditor;
		protected bool _allowUsersChangeEmailSettings;
		protected bool _allowDirectMode;
		protected bool _directModeIsDefault;
		protected bool _allowNewUserRegister;
		protected bool _allowUsersAddNewAccount;
		protected bool _storeMailsInDb;
		protected bool _enableWmServer;
		protected string _wmServerRootPath;
        protected string _wmServerHost;
        protected bool _wmAllowManageXMailAccounts;
        protected bool _allowContacts;
        protected bool _allowCalendar;

		// Calendar Settings
		protected int _defaultTimeFormat;
		protected int _defaultDateFormat;
		protected int _showWeekends;
		protected int _workdayStarts;
		protected int _workdayEnds;
		protected int _showWorkDay;
		protected int _weekStartsOn;
        
        private Hashtable _webmailTab;

		protected int _defaultTab;
		protected string _defaultCountry;
		protected int _defaultTimeZoneCalendar;
		protected int _allTimeZones;

		#endregion

		#region Properties

		public string SiteName
		{
			get { return _siteName; }
			set { _siteName = value; }
		}

		public string LicenseKey
		{
			get { return _licenseKey; }
			set { _licenseKey = value; }
		}

		public string AdminPassword
		{
			get { return _adminPassword; }
			set { _adminPassword = value; }
		}

		public SupportedDatabase DbType
		{
			get { return _dbType; }
			set { _dbType = value; }
		}

		public string DbLogin
		{
			get { return _dbLogin; }
			set { _dbLogin = value; }
		}

		public string DbPassword
		{
			get { return _dbPassword; }
			set { _dbPassword = value; }
		}

		public string DbName
		{
			get { return _dbName; }
			set { _dbName = value; }
		}

		public string DbDsn
		{
			get { return _dbDsn; }
			set { _dbDsn = value; }
		}

		public string DbHost
		{
			get { return _dbHost; }
			set { _dbHost = value; }
		}

		public string DbPathToMdb
		{
			get { return _dbPathToMdb; }
			set { _dbPathToMdb = value; }
		}

		public bool UseCustomConnectionString
		{
			get { return _useCustomConnectionString; }
			set { _useCustomConnectionString = value; }
		}

		public string DbCustomConnectionString
		{
			get { return _dbCustomConnectionString; }
			set { _dbCustomConnectionString = value; }
		}

		public string DbPrefix
		{
			get { return _dbPrefix; }
			set { _dbPrefix = value; }
		}

		public IncomingMailProtocol IncomingMailProtocol
		{
			get { return _incomingMailProtocol; }
			set { _incomingMailProtocol = value; }
		}

		public string IncomingMailServer
		{
			get { return _incomingMailServer; }
			set { _incomingMailServer = value; }
		}

		public int IncomingMailPort
		{
			get { return _incomingMailPort; }
			set { _incomingMailPort = value; }
		}

		public string OutgoingMailServer
		{
			get { return _outgoingMailServer; }
			set { _outgoingMailServer = value; }
		}

		public int OutgoingMailPort
		{
			get { return _outgoingMailPort; }
			set { _outgoingMailPort = value; }
		}

		public bool ReqSmtpAuth
		{
			get { return _reqSmtpAuth; }
			set { _reqSmtpAuth = value; }
		}

		public bool AllowAdvancedLogin
		{
			get { return _allowAdvancedLogin; }
			set { _allowAdvancedLogin = value; }
		}

		public LoginMode HideLoginMode
		{
			get { return _hideLoginMode; }
			set { _hideLoginMode = value; }
		}

		public string DefaultDomainOptional
		{
			get { return _defaultDomainOptional; }
			set { _defaultDomainOptional = value; }
		}

		public bool ShowTextLabels
		{
			get { return _showTextLabels; }
			set { _showTextLabels = value; }
		}

		public bool AutomaticCorrectLoginSettings
		{
			get { return _automaticCorrectLoginSettings; }
			set { _automaticCorrectLoginSettings = value; }
		}

		public bool EnableLogging
		{
			get { return _enableLogging; }
			set { _enableLogging = value; }
		}

		public bool DisableErrorHandling
		{
			get { return _disableErrorHandling; }
			set { _disableErrorHandling = value; }
		}

		public bool AllowAjax
		{
			get { return _allowAjax; }
			set { _allowAjax = value; }
		}

		public short MailsPerPage
		{
			get { return _mailsPerPage; }
			set { _mailsPerPage = value; }
		}

		public long AttachmentSizeLimit
		{
			get { return _attachmentSizeLimit; }
			set { _attachmentSizeLimit = value; }
		}

		public bool EnableAttachmentSizeLimit
		{
			get { return _enableAttachmentSizeLimit; }
			set { _enableAttachmentSizeLimit = value; }
		}

		public long MailboxSizeLimit
		{
			get { return _mailboxSizeLimit; }
			set { _mailboxSizeLimit = value; }
		}

		public bool EnableMailboxSizeLimit
		{
			get { return _enableMailboxSizeLimit; }
			set { _enableMailboxSizeLimit = value; }
		}

		public short DefaultTimeZone
		{
			get { return _defaultTimeOffset; }
			set { _defaultTimeOffset = value; }
		}

		public bool AllowUsersChangeTimeZone
		{
			get { return _allowUsersChangeTimeOffset; }
			set { _allowUsersChangeTimeOffset = value; }
		}

		public int DefaultUserCharset
		{
			get { return _defaultUserCharset; }
			set { _defaultUserCharset = value; }
		}

		public bool AllowUsersChangeCharset
		{
			get { return _allowUsersChangeCharset; }
			set { _allowUsersChangeCharset = value; }
		}

		public string DefaultSkin
		{
			get { return _defaultSkin; }
			set { _defaultSkin = value; }
		}

		public bool AllowUsersChangeSkin
		{
			get { return _allowUsersChangeSkin; }
			set { _allowUsersChangeSkin = value; }
		}

		public string DefaultLanguage
		{
			get { return _defaultLanguage; }
			set { _defaultLanguage = value; }
		}

		public bool AllowUsersChangeLanguage
		{
			get { return _allowUsersChangeLanguage; }
			set { _allowUsersChangeLanguage = value; }
		}

		public bool AllowDhtmlEditor
		{
			get { return _allowDhtmlEditor; }
			set { _allowDhtmlEditor = value; }
		}

		public bool AllowUsersChangeEmailSettings
		{
			get { return _allowUsersChangeEmailSettings; }
			set { _allowUsersChangeEmailSettings = value; }
		}

		public bool AllowDirectMode
		{
			get { return _allowDirectMode; }
			set { _allowDirectMode = value; }
		}

		public bool DirectModeIsDefault
		{
			get { return _directModeIsDefault; }
			set { _directModeIsDefault = value; }
		}

		public bool AllowNewUsersRegister
		{
			get { return _allowNewUserRegister; }
			set { _allowNewUserRegister = value; }
		}

		public bool AllowUsersAddNewAccounts
		{
			get { return _allowUsersAddNewAccount; }
			set { _allowUsersAddNewAccount = value; }
		}

		public bool StoreMailsInDb
		{
			get { return _storeMailsInDb; }
			set { _storeMailsInDb = value; }
		}

		public bool EnableWmServer
		{
			get { return _enableWmServer; }
			set { _enableWmServer = value; }
		}

		public string WmServerRootPath
		{
			get { return _wmServerRootPath; }
			set { _wmServerRootPath = value; }
		}

		public bool AllowContacts
		{
			get { return _allowContacts; }
			set { _allowContacts = value; }
		}

        public string WmServerHost
		{
            get { return _wmServerHost; }
            set { _wmServerHost = value; }
		}

        public bool WmAllowManageXMailAccounts
        {
            get { return _wmAllowManageXMailAccounts; }
            set { _wmAllowManageXMailAccounts = value; }
        }

		protected Hashtable WebmailTab
		{
			get { return _webmailTab; }
		}
          
		public bool AllowCalendar
		{
			get { return _allowCalendar; }
			set { _allowCalendar = value; }
		}

		public int DefaultTimeFormat
		{
			get { return _defaultTimeFormat; }
			set { _defaultTimeFormat = value; }
		}

		public int DefaultDateFormat
		{
			get { return _defaultDateFormat; }
			set { _defaultDateFormat = value; }
		}

		public int ShowWeekends
		{
			get { return _showWeekends; }
			set { _showWeekends = value; }
		}

		public int WorkdayStarts
		{
			get { return _workdayStarts; }
			set { _workdayStarts = value; }
		}

		public int WorkdayEnds
		{
			get { return _workdayEnds; }
			set { _workdayEnds = value; }
		}

		public int ShowWorkDay
		{
			get { return _showWorkDay; }
			set { _showWorkDay = value; }
		}

		public int WeekStartsOn
		{
			get { return _weekStartsOn; }
			set { _weekStartsOn = value; }
		}

		public int DefaultTab
		{
			get { return _defaultTab; }
			set { _defaultTab = value; }
		}

		public string DefaultCountry
		{
			get { return _defaultCountry; }
			set { _defaultCountry = value; }
		}

		public int DefaultTimeZoneCalendar
		{
			get { return _defaultTimeZoneCalendar; }
			set { _defaultTimeZoneCalendar = value; }
		}

		public int AllTimeZones
		{
			get { return _allTimeZones; }
			set { _allTimeZones = value; }
		}

		public int XMailControlPort
		{
			get
			{
				int port = 0;
				if (_webmailTab != null)
				{
					if (_webmailTab[Constants.WebmailTabKeys.ControlPort] != null)
					{
						if (int.TryParse(_webmailTab[Constants.WebmailTabKeys.ControlPort].ToString(), out port))
						{
							return port;
						}
					}
				}
				return port;
			}
		}

		public int XMailSmtpPort
		{
			get
			{
				int port = 0;
				if (_webmailTab != null)
				{
					if (_webmailTab[Constants.WebmailTabKeys.SmtpPort] != null)
					{
						if (int.TryParse(_webmailTab[Constants.WebmailTabKeys.SmtpPort].ToString(), out port))
						{
							return port;
						}
					}
				}
				return port;
			}
		}

		public string XMailLogin
		{
			get
			{
				if (_webmailTab != null)
				{
					return (string)_webmailTab[Constants.WebmailTabKeys.Login];
				}
				return null;
			}
		}

		public string XMailPass
		{
			get
			{
				if (_webmailTab != null)
				{
					return WMServerStorage.DecryptPassword((string)_webmailTab[Constants.WebmailTabKeys.Pass]);
				}
				return null;
			}
		}
		#endregion

		public WebmailSettings()
		{
			_instance = null;

			_siteName = string.Empty;
			_licenseKey = string.Empty;
			_adminPassword = string.Empty;
			_dbType = SupportedDatabase.MsSqlServer;
			_dbLogin = string.Empty;
			_dbPassword = string.Empty;
			_dbName = string.Empty;
			_dbDsn = string.Empty;
			_dbHost = string.Empty;
			_dbPathToMdb = string.Empty;
			_useCustomConnectionString = false;
			_dbCustomConnectionString = string.Empty;
			_dbPrefix = string.Empty;
			_incomingMailServer = string.Empty;
			_incomingMailPort = 110;
			_outgoingMailServer = string.Empty;
			_outgoingMailPort = 25;
			_reqSmtpAuth = true;
			_allowAdvancedLogin = true;
			_hideLoginMode = LoginMode.Default;
			_defaultDomainOptional = string.Empty;
			_showTextLabels = true;
			_automaticCorrectLoginSettings = true;
			_enableLogging = true;
			_disableErrorHandling = false;
			_allowAjax = true;
			_mailsPerPage = 20;
			_attachmentSizeLimit = 10000000;
			_enableAttachmentSizeLimit = true;
			_mailboxSizeLimit = 9000000;
			_enableMailboxSizeLimit = true;
			_defaultTimeOffset = 0;
			_allowUsersChangeTimeOffset = true;
			_defaultUserCharset = Encoding.Default.CodePage;
			_allowUsersChangeCharset = true;
			_defaultSkin = Constants.defaultSkinName;
			_allowUsersChangeSkin = true;
			_defaultLanguage = Constants.SupportedLangs.English;
			_allowUsersChangeLanguage = true;
			_allowDhtmlEditor = true;
			_allowUsersChangeEmailSettings = true;
			_allowDirectMode = true;
			_directModeIsDefault = false;
			_allowNewUserRegister = true;
			_allowUsersAddNewAccount = true;
			_storeMailsInDb = false;

            _enableWmServer = false;
            _wmServerRootPath = "";
            _wmServerHost = "";
            _wmAllowManageXMailAccounts = false;

			_allowContacts = true;
			_allowCalendar = true;

			_defaultTimeFormat = 1;
			_defaultDateFormat = 1;
			_showWeekends = 1;
			_workdayStarts = 9;
			_workdayEnds = 18;
			_showWorkDay = 1;
			_weekStartsOn = 0;
			_defaultTab = 2;
			_defaultCountry = "US";
			_defaultTimeZoneCalendar = 38;
			_allTimeZones = 0;

		}

		public WebmailSettings CreateInstance()
		{
			if (_instance == null)
			{
				_instance = new WebmailSettings();
				_instance.LoadWebmailSettings();
			}

			return _instance;
		}

		public WebmailSettings CreateInstance(string dataFolder)
		{
			if (_instance == null)
			{
				_instance = new WebmailSettings();
				_instance.LoadWebmailSettings(dataFolder);
			}

			return _instance;
		}

		public void _LoadXML(XmlNode root)
		{
			for (int i = 0; i < root.ChildNodes.Count; i++)
			{
				XmlNode node = root.ChildNodes[i];
				switch (node.Name)
				{
					case "Common":
					case "WebMail":
					case "Calendar":
						if (node.ChildNodes.Count > 0)
							_LoadXML(node);
						break;
					case "SiteName":
						SiteName = node.InnerText;
						break;
					case "WindowTitle":
						SiteName = node.InnerText;
						break;
					case "LicenseKey":
						LicenseKey = node.InnerText;
						break;
					case "AdminPassword":
						AdminPassword = node.InnerText;
						break;
					case "DBType":
						if (node.InnerText != string.Empty) DbType = (SupportedDatabase)int.Parse(node.InnerText);
						break;
					case "DBLogin":
						DbLogin = node.InnerText;
						break;
					case "DBPassword":
						DbPassword = node.InnerText;
						break;
					case "DBName":
						DbName = node.InnerText;
						break;
					case "DBDSN":
						DbDsn = node.InnerText;
						break;
					case "DBHost":
						DbHost = node.InnerText;
						break;
					case "DBPathToMdb":
						DbPathToMdb = node.InnerText;
						break;
					case "UseCustomConnectionString":
						UseCustomConnectionString = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "DBCustomConnectionString":
						DbCustomConnectionString = node.InnerText;
						break;
					case "DBPrefix":
						DbPrefix = node.InnerText;
						break;
					case "IncomingMailProtocol":
						if (node.InnerText != string.Empty) IncomingMailProtocol = (IncomingMailProtocol)int.Parse(node.InnerText);
						break;
					case "IncomingMailServer":
						IncomingMailServer = node.InnerText;
						break;
					case "IncomingMailPort":
						if (node.InnerText != string.Empty) IncomingMailPort = int.Parse(node.InnerText);
						break;
					case "OutgoingMailServer":
						OutgoingMailServer = node.InnerText;
						break;
					case "OutgoingMailPort":
						if (node.InnerText != string.Empty) OutgoingMailPort = int.Parse(node.InnerText);
						break;
					case "ReqSmtpAuth":
						ReqSmtpAuth = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "AllowAdvancedLogin":
						AllowAdvancedLogin = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "HideLoginMode":
						HideLoginMode = (LoginMode)short.Parse(node.InnerText, CultureInfo.InvariantCulture);
						break;
					case "DefaultDomainOptional":
						DefaultDomainOptional = node.InnerText;
						break;
					case "ShowTextLabels":
						ShowTextLabels = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "AutomaticCorrectLoginSettings":
						AutomaticCorrectLoginSettings = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "EnableLogging":
						EnableLogging = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "DisableErrorHandling":
						DisableErrorHandling = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "AllowAjax":
						AllowAjax = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "MailsPerPage":
						if (node.InnerText != string.Empty) MailsPerPage = short.Parse(node.InnerText);
						break;
					case "EnableAttachmentSizeLimit":
						EnableAttachmentSizeLimit = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "AttachmentSizeLimit":
						if (node.InnerText != string.Empty) AttachmentSizeLimit = long.Parse(node.InnerText);
						break;
					case "EnableMailboxSizeLimit":
						EnableMailboxSizeLimit = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "MailboxSizeLimit":
						if (node.InnerText != string.Empty) MailboxSizeLimit = long.Parse(node.InnerText);
						break;
					case "AllowUsersChangeTimeZone":
						AllowUsersChangeTimeZone = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "DefaultUserCharset":
						if (node.InnerText != string.Empty) DefaultUserCharset = int.Parse(node.InnerText);
						break;
					case "AllowUsersChangeCharset":
						AllowUsersChangeCharset = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "DefaultSkin":
						DefaultSkin = node.InnerText;
						break;
					case "AllowUsersChangeSkin":
						AllowUsersChangeSkin = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "DefaultLanguage":
						DefaultLanguage = node.InnerText;
						break;
					case "AllowUsersChangeLanguage":
						AllowUsersChangeLanguage = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "AllowDHTMLEditor":
						AllowDhtmlEditor = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "AllowUsersChangeEmailSettings":
						AllowUsersChangeEmailSettings = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "AllowDirectMode":
						AllowDirectMode = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "DirectModeIsDefault":
						DirectModeIsDefault = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "AllowNewUsersRegister":
						AllowNewUsersRegister = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "AllowUsersAddNewAccounts":
						AllowUsersAddNewAccounts = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "StoreMailsInDb":
						StoreMailsInDb = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "EnableWmServer":
						EnableWmServer = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "WmServerRootPath":
						WmServerRootPath = node.InnerText;
						break;
                    case "WmServerHost":
                        WmServerHost = node.InnerText;
                        break;
                    case "WmAllowManageXMailAccounts":
                        WmAllowManageXMailAccounts = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
					case "AllowContacts":
						AllowContacts = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "AllowCalendar":
						AllowCalendar = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						break;
					case "DefaultTimeZone":
						if (node.ParentNode.Name == "Calendar")
						{
							if (node.InnerText != string.Empty) DefaultTimeZoneCalendar = short.Parse(node.InnerText);
						}
						else
						{
							if (node.InnerText != string.Empty) DefaultTimeZone = short.Parse(node.InnerText);
						}
						break;
					case "DefaultTimeFormat":
						DefaultTimeFormat = int.Parse(node.InnerText);
						break;
					case "DefaultDateFormat":
						DefaultDateFormat = int.Parse(node.InnerText);
						break;
					case "ShowWeekends":
						ShowWeekends = int.Parse(node.InnerText);
						break;
					case "WorkdayStarts":
						WorkdayStarts = int.Parse(node.InnerText);
						break;
					case "WorkdayEnds":
						WorkdayEnds = int.Parse(node.InnerText);
						break;
					case "ShowWorkDay":
						ShowWorkDay = int.Parse(node.InnerText);
						break;
					case "WeekStartsOn":
						WeekStartsOn = int.Parse(node.InnerText);
						break;
					case "DefaultTab":
						DefaultTab = int.Parse(node.InnerText);
						break;
					case "DefaultCountry":
						DefaultCountry = node.InnerText;
						break;
					case "AllTimeZones":
						AllTimeZones = int.Parse(node.InnerText);
						break;
				}
			}
		}

		public void LoadWebmailSettings()
		{
			LoadWebmailSettings(Utils.GetDataFolderPath());
		}

		public void LoadWebmailSettings(string dataFolder)
		{
			try
			{
			    string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dataFolder, @"Settings\settings.xml");//Path.Combine(dataFolder, @"Settings\settings.xml");

				XmlDocument doc = new XmlDocument();
				doc.Load(filename);
				XmlNode root = doc.DocumentElement;
				_LoadXML(root);
				_webmailTab = Utils.ReadWebmailTab(WmServerRootPath);
			}
			catch (Exception ex)
			{
				throw new WebMailSettingsException(ex);
			}
		}

		public void SaveWebmailSettings()
		{
			SaveWebmailSettings(Utils.GetDataFolderPath());
		}

		public void SaveWebmailSettings(string dataFolder)
		{
			try
			{
				string filename = Path.Combine(dataFolder, @"Settings\settings.xml");

				XmlDocument result = new XmlDocument();
				result.PreserveWhitespace = false;
				XmlDeclaration xmlDecl = result.CreateXmlDeclaration("1.0", "utf-8", "");
				result.AppendChild(xmlDecl);

				XmlElement settingsElem = result.CreateElement("Settings");
				result.AppendChild(settingsElem);

				XmlElement subElemCommon = result.CreateElement("Common");
				settingsElem.AppendChild(subElemCommon);

				XmlElement subElemWebMail = result.CreateElement("WebMail");
				settingsElem.AppendChild(subElemWebMail);

				XmlElement subElemCalendar = result.CreateElement("Calendar");
				settingsElem.AppendChild(subElemCalendar);

				XmlElement subElem = result.CreateElement("SiteName");
				subElem.AppendChild(result.CreateTextNode(SiteName));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("LicenseKey");
				subElem.AppendChild(result.CreateTextNode(LicenseKey));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("AdminPassword");
				subElem.AppendChild(result.CreateTextNode(AdminPassword));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("DBType");
				subElem.AppendChild(result.CreateTextNode(((int)DbType).ToString(CultureInfo.InvariantCulture)));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("DBLogin");
				subElem.AppendChild(result.CreateTextNode(DbLogin));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("DBPassword");
				subElem.AppendChild(result.CreateTextNode(DbPassword));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("DBName");
				subElem.AppendChild(result.CreateTextNode(DbName));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("DBDSN");
				subElem.AppendChild(result.CreateTextNode(DbDsn));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("DBHost");
				subElem.AppendChild(result.CreateTextNode(DbHost));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("DBPathToMdb");
				subElem.AppendChild(result.CreateTextNode(DbPathToMdb));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("UseCustomConnectionString");
				subElem.AppendChild(result.CreateTextNode((UseCustomConnectionString) ? "1" : "0"));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("DBCustomConnectionString");
				subElem.AppendChild(result.CreateTextNode(DbCustomConnectionString));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("DBPrefix");
				subElem.AppendChild(result.CreateTextNode(DbPrefix));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("DefaultSkin");
				subElem.AppendChild(result.CreateTextNode(DefaultSkin));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("AllowUsersChangeSkin");
				subElem.AppendChild(result.CreateTextNode((AllowUsersChangeSkin) ? "1" : "0"));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("DefaultLanguage");
				subElem.AppendChild(result.CreateTextNode(DefaultLanguage));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("AllowUsersChangeLanguage");
				subElem.AppendChild(result.CreateTextNode((AllowUsersChangeLanguage) ? "1" : "0"));
				subElemCommon.AppendChild(subElem);

				subElem = result.CreateElement("IncomingMailProtocol");
				subElem.AppendChild(result.CreateTextNode(((int)IncomingMailProtocol).ToString(CultureInfo.InvariantCulture)));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("IncomingMailServer");
				subElem.AppendChild(result.CreateTextNode(IncomingMailServer));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("IncomingMailPort");
				subElem.AppendChild(result.CreateTextNode(IncomingMailPort.ToString(CultureInfo.InvariantCulture)));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("OutgoingMailServer");
				subElem.AppendChild(result.CreateTextNode(OutgoingMailServer));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("OutgoingMailPort");
				subElem.AppendChild(result.CreateTextNode(OutgoingMailPort.ToString(CultureInfo.InvariantCulture)));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("ReqSmtpAuth");
				subElem.AppendChild(result.CreateTextNode((ReqSmtpAuth) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("AllowAdvancedLogin");
				subElem.AppendChild(result.CreateTextNode((AllowAdvancedLogin) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("HideLoginMode");
				subElem.AppendChild(result.CreateTextNode(((short)HideLoginMode).ToString(CultureInfo.InvariantCulture)));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("DefaultDomainOptional");
				subElem.AppendChild(result.CreateTextNode(DefaultDomainOptional));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("ShowTextLabels");
				subElem.AppendChild(result.CreateTextNode((ShowTextLabels) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("AutomaticCorrectLoginSettings");
				subElem.AppendChild(result.CreateTextNode((AutomaticCorrectLoginSettings) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("EnableLogging");
				subElem.AppendChild(result.CreateTextNode((EnableLogging) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("DisableErrorHandling");
				subElem.AppendChild(result.CreateTextNode((DisableErrorHandling) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("AllowAjax");
				subElem.AppendChild(result.CreateTextNode((AllowAjax) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("MailsPerPage");
				subElem.AppendChild(result.CreateTextNode(MailsPerPage.ToString(CultureInfo.InvariantCulture)));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("AttachmentSizeLimit");
				subElem.AppendChild(result.CreateTextNode(AttachmentSizeLimit.ToString(CultureInfo.InvariantCulture)));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("EnableAttachmentSizeLimit");
				subElem.AppendChild(result.CreateTextNode((EnableAttachmentSizeLimit) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("MailboxSizeLimit");
				subElem.AppendChild(result.CreateTextNode(MailboxSizeLimit.ToString(CultureInfo.InvariantCulture)));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("EnableMailboxSizeLimit");
				subElem.AppendChild(result.CreateTextNode((EnableMailboxSizeLimit) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("DefaultTimeZone");
				subElem.AppendChild(result.CreateTextNode(DefaultTimeZone.ToString(CultureInfo.InvariantCulture)));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("AllowUsersChangeTimeZone");
				subElem.AppendChild(result.CreateTextNode((AllowUsersChangeTimeZone) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("DefaultUserCharset");
				subElem.AppendChild(result.CreateTextNode(DefaultUserCharset.ToString(CultureInfo.InvariantCulture)));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("AllowUsersChangeCharset");
				subElem.AppendChild(result.CreateTextNode((AllowUsersChangeCharset) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("AllowDHTMLEditor");
				subElem.AppendChild(result.CreateTextNode((AllowDhtmlEditor) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("AllowUsersChangeEmailSettings");
				subElem.AppendChild(result.CreateTextNode((AllowUsersChangeEmailSettings) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("AllowDirectMode");
				subElem.AppendChild(result.CreateTextNode((AllowDirectMode) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("DirectModeIsDefault");
				subElem.AppendChild(result.CreateTextNode((DirectModeIsDefault) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("AllowNewUsersRegister");
				subElem.AppendChild(result.CreateTextNode((AllowNewUsersRegister) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("AllowUsersAddNewAccounts");
				subElem.AppendChild(result.CreateTextNode((AllowUsersAddNewAccounts) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("StoreMailsInDb");
				subElem.AppendChild(result.CreateTextNode((StoreMailsInDb) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("EnableWmServer");
				subElem.AppendChild(result.CreateTextNode((EnableWmServer) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("WmServerRootPath");
                subElem.AppendChild(result.CreateTextNode(WmServerRootPath));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("WmServerHost");
                subElem.AppendChild(result.CreateTextNode(WmServerHost));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("WmAllowManageXMailAccounts");
                subElem.AppendChild(result.CreateTextNode((WmAllowManageXMailAccounts) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);
                
				subElem = result.CreateElement("AllowContacts");
				subElem.AppendChild(result.CreateTextNode((AllowContacts) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("AllowCalendar");
				subElem.AppendChild(result.CreateTextNode((AllowCalendar) ? "1" : "0"));
				subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("DefaultTimeFormat");
				subElem.AppendChild(result.CreateTextNode(DefaultTimeFormat.ToString()));
				subElemCalendar.AppendChild(subElem);

				subElem = result.CreateElement("DefaultDateFormat");
				subElem.AppendChild(result.CreateTextNode(DefaultDateFormat.ToString()));
				subElemCalendar.AppendChild(subElem);

				subElem = result.CreateElement("ShowWeekends");
				subElem.AppendChild(result.CreateTextNode(ShowWeekends.ToString()));
				subElemCalendar.AppendChild(subElem);

				subElem = result.CreateElement("WorkdayStarts");
				subElem.AppendChild(result.CreateTextNode(WorkdayStarts.ToString()));
				subElemCalendar.AppendChild(subElem);

				subElem = result.CreateElement("WorkdayEnds");
				subElem.AppendChild(result.CreateTextNode(WorkdayEnds.ToString()));
				subElemCalendar.AppendChild(subElem);

				subElem = result.CreateElement("ShowWorkDay");
				subElem.AppendChild(result.CreateTextNode(ShowWorkDay.ToString()));
				subElemCalendar.AppendChild(subElem);

				subElem = result.CreateElement("WeekStartsOn");
				subElem.AppendChild(result.CreateTextNode(WeekStartsOn.ToString()));
				subElemCalendar.AppendChild(subElem);

				subElem = result.CreateElement("DefaultTab");
				subElem.AppendChild(result.CreateTextNode(DefaultTab.ToString()));
				subElemCalendar.AppendChild(subElem);

				subElem = result.CreateElement("DefaultCountry");
				subElem.AppendChild(result.CreateTextNode(DefaultCountry));
				subElemCalendar.AppendChild(subElem);

				subElem = result.CreateElement("DefaultTimeZone");
				subElem.AppendChild(result.CreateTextNode(DefaultTimeZoneCalendar.ToString()));
				subElemCalendar.AppendChild(subElem);

				subElem = result.CreateElement("AllTimeZones");
				subElem.AppendChild(result.CreateTextNode(AllTimeZones.ToString()));
				subElemCalendar.AppendChild(subElem);

				result.Save(filename);

				_instance = null;
			}
			catch (Exception ex)
			{
				throw new WebMailSettingsException(ex);
			}
		}
	}

	public class WebMailSettingsCreator : Control
	{
		public WebmailSettings CreateWebMailSettings()
		{
			return CreateWebMailSettings(Utils.GetDataFolderPath());
		}

		public WebmailSettings CreateWebMailSettings(string dataFolder)
		{
			WebmailSettings newSettings = new WebmailSettings();
			if (Context.Application[Constants.sessionSettings] != null)
			{
				newSettings = (WebmailSettings)Context.Application[Constants.sessionSettings];
			}
			else
			{
				newSettings = newSettings.CreateInstance(dataFolder);
				Context.Application.Add(Constants.sessionSettings, newSettings);
			}
			return newSettings;
		}

		public void ResetWebMailSettings()
		{
			if (Context.Application[Constants.sessionSettings] != null)
			{
				Context.Application[Constants.sessionSettings] = null;
			}
		}
	}
}
