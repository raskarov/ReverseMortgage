using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using MailBee.Mime;
using Calendar_NET;
using System.Configuration;
using System.IO;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for User.
	/// </summary>
	[Serializable]
	public class User
	{
		protected int _id = -1;
		protected bool _deleted = false;
		protected UserSettings _settings;
		protected UserColumn[] _columns;

		public int ID
		{
			get { return _id; }
			set { _id = value; }
		}

		public bool Deleted
		{
			get { return _deleted; }
			set { _deleted = value; }
		}

		public UserSettings Settings
		{
			get { return _settings; }
			set
			{
				_settings = value;
				if (_settings != null)
				{
					_settings.IDUser = _id;
				}
			}
		}

		public UserColumn[] Columns
		{
			get { return _columns; }
			set
			{
				_columns = value;
				if (_columns != null)
				{
					foreach (UserColumn column in _columns)
					{
						column.IDUser = _id;
					}
				}
			}
		}

		public User() {}

		public User(int id, bool deleted, UserSettings settings) : this()
		{
			_id = id;
			_deleted = deleted;
			_settings = settings;
		}

		public Account CreateAccount(bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol, string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host, string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm, bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode, short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt, string delimiter, long mailbox_size, FolderSyncType inboxSyncType, bool advanced_login)
		{
			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			//----- validate incoming data -----
			if ((email == null) || (email.Trim().Length == 0))
			{
				if (advanced_login) throw new WebMailException(resMan.GetString("WarningEmailFieldBlank"));
			}
			Regex r = new Regex(@"[\w!#\$%\^\{}`~&'\+-=_\.]+@[\w-\.]+");
			Match m = r.Match(email);
			if (!m.Success)
			{
				throw new WebMailException("WarningCorrectEmail");
			}
			if ((mail_inc_login == null) || (mail_inc_login.Trim().Length == 0))
			{
				if (advanced_login) throw new WebMailException(resMan.GetString("WarningLoginFieldBlank"));
			}
			if (mail_inc_login.IndexOfAny(new char[] { '"', '/', '\\', '*', '?', '<', '>', '|', ':' }) >= 0)
			{
				throw new WebMailException(resMan.GetString("WarningCorrectLogin"));
			}
			if ((mail_inc_pass == null) || (mail_inc_pass.Length == 0))
			{
				throw new WebMailException(resMan.GetString("WarningPassBlank"));
			}
			if ((mail_inc_host == null) || (mail_inc_host.Trim().Length == 0))
			{
				throw new WebMailException(resMan.GetString("WarningIncServerBlank"));
			}
			r = new Regex(@"[^(\w-\.)]+");
			m = r.Match(mail_inc_host);
			if (m.Success)
			{
				if (advanced_login) throw new WebMailException(resMan.GetString("WarningCorrectIncServer"));
			}
			if ((mail_inc_port < 0) || (mail_inc_port > 65535))
			{
				if (advanced_login) throw new WebMailException(resMan.GetString("WarningIncPortNumber"));
			}
			if ((mail_out_host == null) || (mail_out_host.Trim().Length == 0))
			{
				if (advanced_login) throw new WebMailException(resMan.GetString("WarningOutServerBlank"));
			}
			r = new Regex(@"[^(\w-\.)]+");
			m = r.Match(mail_out_host);
			if (m.Success)
			{
				if (advanced_login) throw new WebMailException(resMan.GetString("WarningCorrectOutServer"));
			}
			if ((mail_out_port < 0) || (mail_out_port > 65535))
			{
				if (advanced_login) throw new WebMailException(resMan.GetString("WarningOutPortNumber"));
			}
			//------ end validate incoming data --------


			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			Account newAccount = null;
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
            try
            {
				if (settings.EnableWmServer && mail_protocol == IncomingMailProtocol.WMServer)
				{
					string emailDomain = EmailAddress.GetDomainFromEmail(email);
					string emailUser = EmailAddress.GetAccountNameFromEmail(email);
					WMServerStorage storage = new WMServerStorage(null);
					string[] domains = storage.GetDomainList();
					foreach (string domain in domains)
					{
						if (string.Compare(emailDomain, domain, true, CultureInfo.InvariantCulture) == 0)
						{
							if (settings.WmAllowManageXMailAccounts)
							{
								WMServerUser[] users = storage.GetUserList(emailDomain);
								bool userExists = false;
								foreach (WMServerUser user in users)
								{
									if (string.Compare(emailUser, user.Name, true, CultureInfo.InvariantCulture) == 0)
									{
										userExists = true;
										break;
									}
								}
								if (!userExists)
								{
									storage.AddUser(emailDomain, emailUser, mail_inc_pass);
								}
							}
							break;
						}
					}
				}

                dbMan.Connect();

                newAccount = dbMan.CreateAccount(this._id, def_acct, deleted, email, mail_protocol, mail_inc_host, mail_inc_login, mail_inc_pass, mail_inc_port, mail_out_host, mail_out_login, mail_out_pass, mail_out_port, mail_out_auth, friendly_nm, use_friendly_nm, def_order, getmail_at_login, mail_mode, mails_on_server_days, signature, signature_type, signature_opt, delimiter, mailbox_size);
                newAccount.UserOfAccount = this;

                dbMan.DbAccount = newAccount;

                FileSystem fs = new FileSystem(email, newAccount.ID, true);
                fs.CreateAccount();

                if (mail_protocol != IncomingMailProtocol.Imap4)
                {
                    if (settings.AllowDirectMode && settings.DirectModeIsDefault) inboxSyncType = FolderSyncType.DirectMode;

                    if (!settings.StoreMailsInDb) fs.CreateFolder(Constants.FolderNames.Inbox);
                    dbMan.CreateFolder(newAccount.ID, -1, FolderType.Inbox, Constants.FolderNames.Inbox, Constants.FolderNames.Inbox, inboxSyncType, false, 0);

                    if (!settings.StoreMailsInDb) fs.CreateFolder(Constants.FolderNames.SentItems);
                    dbMan.CreateFolder(newAccount.ID, -1, FolderType.SentItems, Constants.FolderNames.SentItems, Constants.FolderNames.SentItems, FolderSyncType.DontSync, false, 1);

                    if (!settings.StoreMailsInDb) fs.CreateFolder(Constants.FolderNames.Drafts);
                    dbMan.CreateFolder(newAccount.ID, -1, FolderType.Drafts, Constants.FolderNames.Drafts, Constants.FolderNames.Drafts, FolderSyncType.DontSync, false, 2);

                    if (!settings.StoreMailsInDb) fs.CreateFolder(Constants.FolderNames.Trash);
                    dbMan.CreateFolder(newAccount.ID, -1, FolderType.Trash, Constants.FolderNames.Trash, Constants.FolderNames.Trash, FolderSyncType.DontSync, false, 3);
                }
                else if (mail_protocol == IncomingMailProtocol.Imap4)
                {
                    MailServerStorage imapStorage = MailServerStorageCreator.CreateMailServerStorage(newAccount);
                    try
                    {
                        imapStorage.Connect();
                        FolderCollection fc = imapStorage.GetFolders();
                        Folder fld = fc[FolderType.Inbox];
                        if (fld != null)
                        {
                            fld.SyncType = FolderSyncType.AllHeadersOnly;
                        }

                        if (settings.AllowDirectMode && settings.DirectModeIsDefault)
                        {
                            fc.ReSetSyncTypeToDirectMode();
                        }

                        fld = fc[FolderType.Drafts];
                        if (fld == null)
                        {
                            if (!settings.StoreMailsInDb) fs.CreateFolder(Constants.FolderNames.Drafts);
                            dbMan.CreateFolder(newAccount.ID, -1, FolderType.Drafts, Constants.FolderNames.Drafts, Constants.FolderNames.Drafts, FolderSyncType.DontSync, false, 2);
                        }

                        fld = fc[FolderType.SentItems];
                        if (fld == null)
                        {
                            if (!settings.StoreMailsInDb) fs.CreateFolder(Constants.FolderNames.SentItems);
                            dbMan.CreateFolder(newAccount.ID, -1, FolderType.SentItems, Constants.FolderNames.SentItems, Constants.FolderNames.SentItems, FolderSyncType.DontSync, false, 1);
                        }

                        newAccount.Delimiter = (fc.Count > 0) ? fc[0].ImapFolder.Delimiter : newAccount.Delimiter;

                        if (!settings.StoreMailsInDb) fs.CreateFoldersTree(fc);
                        dbMan.CreateFoldersTree(fc);
                    }
                    catch (Exception ex)
                    {
                        throw new WebMailException(ex);
                    }
                    finally
                    {
                        imapStorage.Disconnect();
                    }
                }
            }
            catch (IOException ex)
            {
                throw new WebMailException(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new WebMailException(ex);
            }
            catch (WebMailException ex)
            {
                if (newAccount != null)
                {
                    if (dbMan.IsConnected)
                    {
                        dbMan.DeleteAccount(newAccount.ID);
                    }
                    else
                    {
                        dbMan.Connect();
                        dbMan.DeleteAccount(newAccount.ID);
                    }
                }
                throw ex;
            }
			finally
			{
				dbMan.Disconnect();
			}
			return newAccount;
		}

		public Account[] GetUserAccounts()
		{
			Account[] result = new Account[0];
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				result = dbMan.SelectUserAccounts(this._id);
			}
			finally
			{
				dbMan.Disconnect();
			}
			return result;
		}

		public static User CreateUser()
		{
			User newUser = null;
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				newUser = dbMan.CreateUser(false);
			}
			finally
			{
				dbMan.Disconnect();
			}
			return newUser;
		}

		public static User LoadUser(int id)
		{
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				return dbMan.SelectUser(id);
			}
			finally
			{
				dbMan.Disconnect();
			}
		}

		public static void DeleteUser(int id)
		{
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
            DBHelper dbH = new DBHelper();
			try
			{
				dbMan.Connect();
				dbMan.DeleteUser(id);
                dbH.DeleteCalendarUserData(id);
			}
			finally
			{
				dbMan.Disconnect();
			}
		}

        public static void DeleteUserSettings(int id)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            try
            {
                dbMan.Connect();
                dbMan.DeleteSettingsData(id);
            }
            finally
            {
                dbMan.Disconnect();
            }
        }

		public void UpdateUser()
		{
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				dbMan.UpdateUser(this);
			}
			finally
			{
				dbMan.Disconnect();
			}
		}

		public byte GetSenderSafety(string email)
		{
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				return dbMan.GetSenderSafety(email, _id);
			}
			finally
			{
				dbMan.Disconnect();
			}
		}
		public void SetSenderSafety(string email, byte safety)
		{
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				dbMan.SetSender(email, safety);
			}
			finally
			{
				dbMan.Disconnect();
			}
		}
	}

	[Serializable]
	public class UserSettings
	{
		#region Fields
		protected int _id;
		protected int _id_user;
		protected short _msgs_per_page;
		protected bool _white_listing;
		protected bool _x_spam;
		protected DateTime _last_login;
		protected int _logins_count;
		protected string _def_skin;
		protected string _def_lang;
		protected int _def_charset_inc;
		protected int _def_charset_out;
		protected short _def_timezone;
		protected string _def_date_fmt;
        protected TimeFormats _def_time_fmt;
		protected bool _hide_folders;
		protected long _mailbox_limit;
		protected bool _allow_change_settings;
		protected bool _allow_dhtml_editor;
		protected bool _allow_direct_mode;
		protected int _db_charset;
		protected short _horiz_resizer;
		protected short _vert_resizer;
		protected byte _mark;
		protected byte _reply;
		protected short _contacts_per_page;
		protected ViewMode _view_mode;

		#endregion

		#region Properties
		public int ID
		{
			get { return _id; }
			set { _id = value; }
		}

		public int IDUser
		{
			get { return _id_user; }
			set { _id_user = value; }
		}

		public short MsgsPerPage
		{
			get { return _msgs_per_page; }
			set { _msgs_per_page = value; }
		}

		public bool WhiteListing
		{
			get { return _white_listing; }
			set { _white_listing = value; }
		}

		public bool XSpam
		{
			get { return _x_spam; }
			set { _x_spam = value; }
		}

		public DateTime LastLogin
		{
			get { return _last_login; }
			set { _last_login = value; }
		}

		public int LoginsCount
		{
			get { return _logins_count; }
			set { _logins_count = value; }
		}

		public string DefaultSkin
		{
			get { return _def_skin; }
			set { _def_skin = value; }
		}

		public string DefaultLanguage
		{
			get { return _def_lang; }
			set { _def_lang = value; }
		}

		public int DefaultCharsetInc
		{
			get { return _def_charset_inc; }
			set { _def_charset_inc = value; }
		}

		public int DefaultCharsetOut
		{
			get { return _def_charset_out; }
			set { _def_charset_out = value; }
		}

		public short DefaultTimeZone
		{
			get { return _def_timezone; }
			set { _def_timezone = value; }
		}

		public string DefaultDateFormat
		{
			get { return _def_date_fmt; }
			set { _def_date_fmt = value; }
		}

        public TimeFormats DefaultTimeFormat
        {
            get { return _def_time_fmt; }
            set { _def_time_fmt = value; }
        }

		public bool HideFolders
		{
			get { return _hide_folders; }
			set { _hide_folders = value; }
		}

		public long MailboxLimit
		{
			get { return _mailbox_limit; }
			set { _mailbox_limit = value; }
		}

		public bool AllowChangeSettings
		{
			get { return _allow_change_settings; }
			set { _allow_change_settings = value; }
		}

		public bool AllowDhtmlEditor
		{
			get { return _allow_dhtml_editor; }
			set { _allow_dhtml_editor = value; }
		}

		public bool AllowDirectMode
		{
			get { return _allow_direct_mode; }
			set { _allow_direct_mode = value; }
		}

		public int DbCharset
		{
			get { return _db_charset; }
			set { _db_charset = value; }
		}

		public short HorizResizer
		{
			get { return _horiz_resizer; }
			set { _horiz_resizer = value; }
		}

		public short VertResizer
		{
			get { return _vert_resizer; }
			set { _vert_resizer = value; }
		}

		public byte Mark
		{
			get { return _mark; }
			set { _mark = value; }
		}

		public byte Reply
		{
			get { return _reply; }
			set { _reply = value; }
		}

		public short ContactsPerPage
		{
			get { return _contacts_per_page; }
			set { _contacts_per_page = value; }
		}

		public ViewMode ViewMode
		{
			get { return _view_mode; }
			set { _view_mode = value; }
		}

		#endregion

		public UserSettings()
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			_id = -1;
			_id_user = -1;
			_msgs_per_page = settings.MailsPerPage;
			_white_listing = false;
			_x_spam = false;
			_last_login = DateTime.Now;
			_logins_count = 1;
			_def_skin = settings.DefaultSkin;
			_def_lang = settings.DefaultLanguage;
			try
			{
				_def_charset_inc = settings.DefaultUserCharset;
				_def_charset_out = settings.DefaultUserCharset;
			}
			catch (Exception)
			{
				_def_charset_inc = Encoding.Default.CodePage;
				_def_charset_out = Encoding.UTF8.CodePage;
			}
			_def_timezone = settings.DefaultTimeZone;
			_def_date_fmt = Constants.DateFormats.MMDDYY;
            _def_time_fmt = TimeFormats.F12;
			_hide_folders = false;
			_mailbox_limit = settings.MailboxSizeLimit;
			_allow_change_settings = settings.AllowUsersChangeEmailSettings;
			_allow_dhtml_editor = settings.AllowDhtmlEditor;
			_allow_direct_mode = settings.AllowDirectMode;
			_db_charset = Encoding.UTF8.CodePage;
			_horiz_resizer = 150;
			_vert_resizer = 115;
			_mark = 0;
			_reply = 0;
			_contacts_per_page = 20;
			_view_mode = ViewMode.WithPreviewPane;
		}

		public UserSettings(int id, int id_user, short msgs_per_page, bool white_listing, bool x_spam, DateTime last_login, int logins_count, string def_skin, string def_lang, Encoding def_charset_inc, Encoding def_charset_out, short def_timezone, string def_date_fmt, bool hide_folders, long mailbox_limit, bool allow_change_settings, bool allow_dhtml_editor, bool allow_direct_mode, Encoding db_charset, TimeFormats time_fmt)
		{
			_id = id;
			_id_user = id_user;
			_msgs_per_page = msgs_per_page;
			_white_listing = white_listing;
			_x_spam = x_spam;
			_last_login = last_login;
			_logins_count = logins_count;
			_def_skin = def_skin;
			_def_lang = def_lang;
			_def_charset_inc = def_charset_inc.CodePage;
			_def_charset_out = def_charset_out.CodePage;
			_def_timezone = def_timezone;
			_def_date_fmt = def_date_fmt;
            _def_time_fmt = time_fmt;
			_hide_folders = hide_folders;
			_mailbox_limit = mailbox_limit;
			_allow_change_settings = allow_change_settings;
			_allow_dhtml_editor = allow_dhtml_editor;
			_allow_direct_mode = allow_direct_mode;
			_db_charset = db_charset.CodePage;
		}
	}

	[Serializable]
	public class UserColumn
	{
		#region Fields
		protected int _id = 0;
		protected int _id_column = 0;
		protected int _id_user = 0;
		protected int _value = 0;
		#endregion

		#region Properties
		public int ID
		{
			get { return _id; }
			set { _id = value; }
		}

		public int IDColumn
		{
			get { return _id_column; }
			set { _id_column = value; }
		}

		public int IDUser
		{
			get { return _id_user; }
			set { _id_user = value; }
		}

		public int Value
		{
			get { return _value; }
			set { _value = value; }
		}
		#endregion

		#region Methods

		public UserColumn() {}

		public UserColumn(int id, int id_column, int id_user, int value)
		{
            _id = id;
			_id_column = id_column;
			_id_user = id_user;
			_value = value;
		}

		public static UserColumn CreateColumn(int id_column, int id_user, int value)
		{
			DbStorage ds = DbStorageCreator.CreateDatabaseStorage(null);
			try
			{
				return ds.CreateUserColumn(id_column, id_user, value);
			}
			finally
			{
				ds.Disconnect();
			}
		}

		public static void DeleteColumn(UserColumn column)
		{
			DbStorage ds = DbStorageCreator.CreateDatabaseStorage(null);
			try
			{
				ds.DeleteUserColumn(column);
			}
			finally
			{
				ds.Disconnect();
			}
		}

		public static UserColumn[] GetColumnsFromDb(int id_user)
		{
			DbStorage ds = DbStorageCreator.CreateDatabaseStorage(null);
			try
			{
				return ds.GetUserColumns(id_user);
			}
			finally
			{
				ds.Disconnect();
			}
		}

		public void Update()
		{
			DbStorage ds = DbStorageCreator.CreateDatabaseStorage(null);
			try
			{
				ds.UpdateUserColumn(_id_column, _id_user, _value);
			}
			finally
			{
				ds.Disconnect();
			}
		}
		#endregion
	}
}
