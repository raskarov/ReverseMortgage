using System;
using System.IO;
using System.Web;

namespace WebMailPro
{
	public enum WMStartPage
	{
		Mailbox = 0,
		NewMessage = 1,
		Settings = 2,
		Contacts = 3,
		Calendar = 4
	}

	/// <summary>
	/// Summary description for Integration.
	/// </summary>
	public class Integration
	{
		protected WebmailResourceManager _resMan = null;
		protected string _wmRoot = string.Empty;

		public Integration(string wmDataPath, string wmRootPath)
		{
			HttpApplicationState app = HttpContext.Current.Application;
			if (app != null)
			{
				if (app[Constants.appSettingsDataFolderPath] == null)
				{
					app.Add(Constants.appSettingsDataFolderPath, wmDataPath);
				}
			}
			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			_wmRoot = wmRootPath.TrimEnd(new char[2] { '/', '\\' }); ;
		}

		public Account GetAccountByID(int id)
		{
			return Account.LoadFromDb(id, -1);
		}

		public Account GetAccountByMailLogin(string email, string login, string pass)
		{
			return Account.LoadFromDb(email, login, pass);
		}

		public void CreateUser(string email, string login, string password)
		{
			Account acct = new Account();
			acct.Email = email;
			acct.MailIncomingLogin = acct.MailOutgoingLogin = login;
			acct.MailIncomingPassword = acct.MailOutgoingPassword = password;
			CreateUserFromAccount(acct);
		}

		public void CreateUserFromAccount(Account acct)
		{
			if (!UserExists(acct.Email, acct.MailIncomingLogin, acct.MailIncomingPassword))
			{
                User usr = null;
                try
                {
                    usr = User.CreateUser();
                    usr.CreateAccount(acct.DefaultAccount, false, acct.Email, acct.MailIncomingProtocol,
                        acct.MailIncomingHost, acct.MailIncomingLogin, acct.MailIncomingPassword,
                        acct.MailIncomingPort, acct.MailOutgoingHost, acct.MailOutgoingLogin,
                        acct.MailOutgoingPassword, acct.MailOutgoingPort, acct.MailOutgoingAuthentication,
                        acct.FriendlyName, acct.UseFriendlyName, acct.DefaultOrder, acct.GetMailAtLogin,
                        acct.MailMode, acct.MailsOnServerDays, acct.Signature, acct.SignatureType,
                        acct.SignatureOptions, acct.Delimiter, acct.MailboxSize, Folder.DefaultInboxSyncType, false);
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

				throw new WebMailException(_resMan.GetString("PROC_ACCOUNT_EXISTS"));
			}
		}

		public void UserLoginByEmail(string email, string login, string password, WMStartPage startPage, string toEmail)
		{
			Account acct = Account.LoginAccount(email, login, password);
			if (acct != null)
			{
				string sessionHash = Utils.GetMD5DigestHexString(HttpContext.Current.Session.SessionID);
				DbStorage storage = DbStorageCreator.CreateDatabaseStorage(acct);
				try
				{
					storage.Connect();
					storage.CreateTempRow(acct.ID, string.Format(@"sessionHash_{0}", sessionHash));
				}
				catch (WebMailException)
				{
					throw;
				}
				catch (Exception ex)
				{
					throw new WebMailDatabaseException(ex);
				}
				finally
				{
					storage.Disconnect();
				}

                HttpContext.Current.Response.Redirect(_wmRoot + @"/" + string.Format(@"integration/integr.aspx?hash={0}&scr={1}&to={2}", sessionHash, (int)startPage, toEmail));
            }
		}

		public void UserLoginByEmail(string email, string login, string pass, WMStartPage startPage)
		{
			UserLoginByEmail(email, login, pass, startPage, null);
		}

		public bool UserExists(string email, string login, string pass)
		{
			return (Account.LoadFromDb(email, login, pass) != null);
		}


	}
}
