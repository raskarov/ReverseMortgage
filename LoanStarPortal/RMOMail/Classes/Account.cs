using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using MailBee.Mime;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for Account.
	/// </summary>
	[Serializable]
	public class Account
	{
		#region Fields
		protected int _id;
		protected int _idUser;
		protected bool _defaultAccount;
		protected bool _deleted;
		protected string _email;
		protected IncomingMailProtocol _mailProtocol;
		protected string _mailIncHost;
		protected string _mailIncLogin;
		protected string _mailIncPassword;
		protected int _mailIncPort;
		protected string _mailOutHost;
		protected string _mailOutLogin;
		protected string _mailOutPassword;
		protected int _mailOutPort;
		protected bool _mailOutAuthentication;
		protected string _friendlyName;
		protected bool _useFriendlyName;
		protected DefaultOrder _defaultOrder;
		protected bool _getMailAtLogin;
		protected MailMode _mailMode;
		protected short _mailsOnServerDays;
		protected string _signature;
		protected SignatureType _signatureType;
		protected SignatureOptions _signatureOptions;
		protected User _user;
		protected string _delimiter;
		protected long _mailbox_size;
		#endregion

		#region Properties
		public int ID
		{
			get { return _id; }
			set { _id = value; }
		}

		public int IDUser
		{
			get { return _idUser; }
			set { _idUser = value; }
		}

		public bool DefaultAccount
		{
			set { _defaultAccount = value;}
			get { return _defaultAccount; }
		}

		public bool Deleted
		{
			get { return _deleted; }
			set { _deleted = value; }
		}

		public string Email
		{
			get { return _email; }
			set { _email = value; }
		}

		public string MailIncomingHost
		{
			get { return _mailIncHost; }
			set { _mailIncHost = value; }
		}

		public string MailIncomingLogin
		{
			get { return _mailIncLogin; }
			set { _mailIncLogin = value; }
		}

		public string MailIncomingPassword
		{
			get
			{
				return _mailIncPassword;
			}
			set
			{
				_mailIncPassword = value;
			}
		}

		public int MailIncomingPort
		{
			get { return _mailIncPort; }
			set { _mailIncPort = value; }
		}

		public string MailOutgoingHost
		{
			get { return _mailOutHost; }
			set { _mailOutHost = value; }
		}

		public string MailOutgoingLogin
		{
			get { return _mailOutLogin; }
			set { _mailOutLogin = value; }
		}

		public string MailOutgoingPassword
		{
			get { return _mailOutPassword; }
			set { _mailOutPassword = value; }
		}

		public int MailOutgoingPort
		{
			get { return _mailOutPort; }
			set { _mailOutPort = value; }
		}

		public bool MailOutgoingAuthentication
		{
			get { return _mailOutAuthentication; }
			set { _mailOutAuthentication = value; }
		}

		public string FriendlyName
		{
			get { return _friendlyName; }
			set { _friendlyName = value; }
		}

		public bool UseFriendlyName
		{
			get { return _useFriendlyName; }
			set { _useFriendlyName = value; }
		}

		public bool GetMailAtLogin
		{
			get { return _getMailAtLogin; }
			set { _getMailAtLogin = value; }
		}

		public short MailsOnServerDays
		{
			get { return _mailsOnServerDays; }
			set { _mailsOnServerDays = value; }
		}

		public string Signature
		{
			get { return _signature; }
			set { _signature = value; }
		}

		public DefaultOrder DefaultOrder
		{
			get { return _defaultOrder; }
			set { _defaultOrder = value; }
		}

		public MailMode MailMode
		{
			get { return _mailMode; }
			set { _mailMode = value; }
		}

		public SignatureType SignatureType
		{
			get { return _signatureType; }
			set { _signatureType = value; }
		}

		public SignatureOptions SignatureOptions
		{
			get { return _signatureOptions; }
			set { _signatureOptions = value; }
		}

		public IncomingMailProtocol MailIncomingProtocol
		{
			get { return _mailProtocol; }
			set { _mailProtocol = value; }
		}

		public User UserOfAccount
		{
			get { return _user; }
			set
			{
				_user = value;
				if (_user != null)
				{
					_idUser = _user.ID;
				}
			}
		}

		public string Delimiter
		{
			get { return _delimiter; }
			set { _delimiter = value; }
		}

		public long MailboxSize
		{
			get { return _mailbox_size; }
			set { _mailbox_size = value; }
		}
		
		
		public bool IsDemo = false;
		
		#endregion

		public Account()
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			_id = -1;
			_idUser = -1;
			_defaultAccount = true;
			_deleted = false;
			_email = "";
			_mailProtocol = settings.IncomingMailProtocol;
			_mailIncHost = settings.IncomingMailServer;
			_mailIncLogin = "";
			_mailIncPassword = "";
			_mailIncPort = settings.IncomingMailPort;
			_mailOutHost = settings.OutgoingMailServer;
			_mailOutLogin = "";
			_mailOutPassword = "";
			_mailOutPort = settings.OutgoingMailPort;
			_mailOutAuthentication = settings.ReqSmtpAuth;
			_friendlyName = "";
			_useFriendlyName = true;
			_defaultOrder = DefaultOrder.DateDesc;
			_getMailAtLogin = true;
			_mailMode = MailMode.LeaveMessagesOnServer;
			_mailsOnServerDays = 1;
			_signature = "";
			_signatureType = SignatureType.Plain;
			_signatureOptions = SignatureOptions.DontAddSignature;
			_user = null;
			_delimiter = "/";
			_mailbox_size = 0;
		}

		public Account(int id_account, bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol, string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host, string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm, bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode, short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt, string delimiter, long mailbox_size)
		{
			_id = id_account;
			_defaultAccount = def_acct;
			_deleted = deleted;
			_email = email;
			_mailProtocol = mail_protocol;
			_mailIncHost = mail_inc_host;
			_mailIncLogin = mail_inc_login;
			_mailIncPassword = mail_inc_pass;
			_mailIncPort = mail_inc_port;
			_mailOutHost = mail_out_host;
			_mailOutLogin = mail_out_login;
			_mailOutPassword = mail_out_pass;
			_mailOutPort = mail_out_port;
			_mailOutAuthentication = mail_out_auth;
			_friendlyName = friendly_nm;
			_useFriendlyName = use_friendly_nm;
			_defaultOrder = def_order;
			_getMailAtLogin = getmail_at_login;
			_mailMode = mail_mode;
			_mailsOnServerDays = mails_on_server_days;
			_signature = signature;
			_signatureType = signature_type;
			_signatureOptions = signature_opt;
			_delimiter = delimiter;
			_mailbox_size = mailbox_size;
		}

		public void Update(bool updateUser)
		{
			if (IsDemo) return;

			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				dbMan.UpdateAccount(this, updateUser);
			}
			finally
			{
				dbMan.Disconnect();
			}
		}

		public void ChangeAccountDefault(bool isDefault)
		{
			if ((_defaultAccount == false) && (isDefault == true))
			{
				DbManager dbMan = (new DbManagerCreator()).CreateDbManager(this);
				try
				{
					dbMan.Connect();
					int nonDefCount = dbMan.GetNotDefaultAccountCount(_email, _mailIncLogin, _mailIncPassword);
					if (nonDefCount > 1)
					{
						throw new WebMailException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("PROC_CANT_LOG_NONDEF"));
					}
					_defaultAccount = isDefault;
				}
				finally
				{
					dbMan.Disconnect();
				}
			}
			else if ((_defaultAccount == true) && (isDefault == false))
			{
				if (_user != null)
				{
					Account[] accounts = _user.GetUserAccounts();
					bool hasDefault = false;
					foreach (Account acct in accounts)
					{
						if (acct.ID != _id)
						{
							if (acct.DefaultAccount)
							{
								hasDefault = true;
								break;
							}
						}
					}
					if (hasDefault) _defaultAccount = isDefault;
				}
			}
		}

		public static Account LoginAccount(string email, string login, string password)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			return LoginAccount(email, login, password, settings.IncomingMailServer, settings.IncomingMailProtocol, settings.IncomingMailPort, settings.OutgoingMailServer, settings.OutgoingMailPort, settings.ReqSmtpAuth, false, false);
		}

		public static Account LoginAccount(string email, string login, string password, string incomingMailServer, IncomingMailProtocol incomingMailProtocol, int incomingPort, string outgoingMailServer, int outgoingMailPort, bool useSmtpAuthentication, bool signAutomatically, bool advanced_login)
		{
			string outgoingMailLogin = string.Empty;
			string outgoingMailPassword = string.Empty;

			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			string tempLogin = login;
			if (!advanced_login)
			{
				switch (settings.HideLoginMode)
				{
					case LoginMode.HideLoginFieldLoginIsAccount:
						login = EmailAddress.Parse(email).GetAccountName();
						tempLogin = null;
						break;
					case LoginMode.HideLoginFieldLoginIsEmail:
						login = email;
						tempLogin = null;
						break;
					case LoginMode.HideEmailField:
					case LoginMode.HideEmailFieldDisplayDomainAfterLogin:
						email = string.Format("{0}@{1}", login, settings.DefaultDomainOptional);
						break;
					case LoginMode.HideEmailFieldLoginIsLoginAndDomain:
					case LoginMode.HideEmailFieldDisplayDomainAfterLoginAndLoginIsLoginAndDomain:
						email = string.Format("{0}@{1}", login, settings.DefaultDomainOptional);
						login = email;
						tempLogin = email;
						break;
				}
			}

			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();

			Account result = LoadFromDb(email, tempLogin, null);
			if (result == null)
			{
				if (!settings.AllowNewUsersRegister)
				{
					throw new WebMailException(resMan.GetString("ErrorPOP3IMAP4Auth"));
				}
				Account acct = new Account();
				acct.Email = email;
				acct.MailIncomingLogin = login;
				acct.MailIncomingPassword = password;
				acct.MailIncomingHost = incomingMailServer;
				acct.MailIncomingProtocol = incomingMailProtocol;
				acct.MailIncomingPort = incomingPort;
				acct.MailOutgoingHost = outgoingMailServer;
				acct.MailOutgoingPort = outgoingMailPort;
				acct.MailOutgoingAuthentication = useSmtpAuthentication;

                if (settings.EnableWmServer)
                {
                    string emailDomain = EmailAddress.GetDomainFromEmail(email);
                    string emailUser = EmailAddress.GetAccountNameFromEmail(email);
                    WMServerStorage storage = new WMServerStorage(null);
                    string[] domains = storage.GetDomainList();
                    foreach (string domain in domains)
                    {
                        if (string.Compare(emailDomain, domain, true, CultureInfo.InvariantCulture) == 0)
                        {
							acct.MailIncomingProtocol = IncomingMailProtocol.WMServer;
							incomingMailProtocol = IncomingMailProtocol.WMServer;
							acct.MailOutgoingLogin = email;
							outgoingMailLogin = email;
							acct.MailOutgoingPassword = password;
							outgoingMailPassword = password;
							acct.MailOutgoingHost = settings.WmServerHost;
							outgoingMailServer = settings.WmServerHost;
							acct.MailOutgoingPort = settings.XMailSmtpPort;
							outgoingMailPort = settings.XMailSmtpPort;
                            break;
                        }
                    }
                }
				bool isXMail = incomingMailProtocol == IncomingMailProtocol.WMServer;
				if (!settings.WmAllowManageXMailAccounts || !isXMail)
				{
					MailServerStorage mss = MailServerStorageCreator.CreateMailServerStorage(acct);
					try
					{
						mss.Connect();
					}
					catch (WebMailException)
					{
						throw;
					}
					finally
					{
						if (mss.IsConnected()) mss.Disconnect();
					}
				}

                User usr = null;
                try
                {
                    usr = User.CreateUser();
					FolderSyncType syncType = (settings.DirectModeIsDefault) ? FolderSyncType.DirectMode : Folder.DefaultInboxSyncType;
					bool getMailAtLogin = true;
					result = usr.CreateAccount(true, false, email, incomingMailProtocol, incomingMailServer, login, password, incomingPort, outgoingMailServer, outgoingMailLogin, outgoingMailPassword, outgoingMailPort, useSmtpAuthentication, "", true, DefaultOrder.DateDesc, getMailAtLogin, MailMode.LeaveMessagesOnServer, 1, "", SignatureType.Plain, SignatureOptions.DontAddSignature, "/", 0, syncType, advanced_login);
                }
                catch (WebMailException ex)
                {
                    if (null != usr)
                    {
                        User.DeleteUserSettings(usr.ID);
                    }
                    throw ex;
                }
			}
			else
			{
				if (string.Compare(result.MailIncomingPassword, password, false, CultureInfo.InvariantCulture) != 0)
				{
					result.MailIncomingPassword = password;
					MailServerStorage mss = MailServerStorageCreator.CreateMailServerStorage(result);
					try
					{
						mss.Connect();
					}
					catch (WebMailException)
					{
						throw;
					}
					finally
					{
						if (mss.IsConnected()) mss.Disconnect();
					}
				}

				if (result.DefaultAccount == false)
				{
					int nonDefaultCount = 0;
				
					DbManagerCreator creator = new DbManagerCreator();
					DbManager dbMan = creator.CreateDbManager();
					try
					{
						dbMan.Connect();
						nonDefaultCount = dbMan.GetNotDefaultAccountCount(email, login, password);
					}
					finally
					{
						dbMan.Disconnect();
					}
					if (nonDefaultCount > 1)
					{
						throw new WebMailException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("PROC_CANT_LOG_NONDEF"));
					}
				}
			}
			if ((result != null) && (result.UserOfAccount != null) && (result.UserOfAccount.Settings != null))
			{
				result.UserOfAccount.Settings.LastLogin = DateTime.Now;
				result.UserOfAccount.Settings.LoginsCount++;
				result.Update(true);
			}
			return result;
		}

		public static Account LoadFromDb(int id_acct, int id_user)
		{
			Account newAccount = new Account();

			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				newAccount = dbMan.SelectAccountData(id_acct, id_user);
			}
			finally
			{
				dbMan.Disconnect();
			}
			return newAccount;
		}

		public static Account LoadFromDb(string email, string login, string password)
		{
			Account newAccount;

			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				newAccount = dbMan.SelectAccountData(email, login, password);
			}
			finally
			{
				dbMan.Disconnect();
			}
			return newAccount;
		}

		public static void DeleteFromDb(Account acct)
		{
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				dbMan.DeleteAccount(acct._id);
			}
			catch (WebMailDatabaseException ex)
			{
				throw new WebMailDatabaseException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("PROC_CANT_DEL_ACCT_BY_ID"), ex);
			}
			finally
			{
				dbMan.Disconnect();
			}

			FileSystem fs = new FileSystem(acct._email, acct._id, true);
			fs.DeleteFolder("");

			fs = new FileSystem(acct._email, acct._id, false);
			fs.DeleteFolder("");

			if (acct.MailIncomingProtocol == IncomingMailProtocol.WMServer)
			{
				WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				try
				{
					if (settings.EnableWmServer && settings.WmAllowManageXMailAccounts)
					{
						WMServerStorage storage = new WMServerStorage(acct);
                        storage.DeleteUser(EmailAddress.GetDomainFromEmail(acct.Email), EmailAddress.GetAccountNameFromEmail(acct.MailIncomingLogin));
					}
				}
				catch (Exception ex)
				{
					Log.WriteException(ex);
				}
			}
		}

	}// END CLASS DEFINITION Account
}
