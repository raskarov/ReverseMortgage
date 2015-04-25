using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using MailBee.Mime;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace WebMailPro
{
	public struct WMServerUser
	{
		public string Domain;
		public string Name;
		public string Password;
		public string Type;

		public static WMServerUser Parse(string src)
		{
			WMServerUser user = new WMServerUser();

			Regex r = new Regex(@"""(?<domain>[^""]*)""\t""(?<name>[^""]*)""\t""(?<password>[^""]*)""\t""(?<type>[^""]*)""");
			Match m = r.Match(src);
			if (m.Success)
			{
				user.Domain = m.Groups["domain"].Value;
				user.Name = m.Groups["name"].Value;
				user.Password = m.Groups["password"].Value;
				user.Type = m.Groups["type"].Value;
			}

			return user;
		}
	}

	public class FileDateComparer : IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
			if (x == y) return 0;
			FileInfo fix = x as FileInfo;
			FileInfo fiy = y as FileInfo;
			if (fix == null) return -1;
			if (fiy == null) return 1;

			return fix.CreationTimeUtc.CompareTo(fiy.CreationTimeUtc);
		}

		#endregion
	}

	/// <summary>
	/// Summary description for XMailStorage.
	/// </summary>
	public class WMServerStorage : MailServerStorage
	{
		protected TcpClient _tcpClient = null;
		protected NetworkStream _nwStream = null;
		protected string _hostname = string.Empty;
		protected int _port = 0;
		protected WebmailResourceManager _resMan = null;

		protected string _response = string.Empty;

		public WMServerStorage(string hostname, int port, Account acct) : base(acct)
		{
			_hostname = hostname;
			_port = port;

			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
		}

        public WMServerStorage(string hostname, Account acct)
            : this(hostname, WMServerPort, acct)
        {
        }

		public WMServerStorage(Account acct)
			: this(WMServerHost, WMServerPort, acct)
		{
		}

		public static string WMServerHost
		{
			get
			{
				WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				return settings.WmServerHost;
			}
		}

		public static int WMServerPort
		{
			get
			{
				WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				return settings.XMailControlPort;
			}
		}

        public override void Connect()
        {
			ConnectAndLoginAdmin();
			UserConnect();
        }

        public void UserConnect()
        {
            string domain = EmailAddress.GetDomainFromEmail(_account.Email);
            string user = EmailAddress.GetAccountNameFromEmail(_account.MailIncomingLogin);

            WMServerUser[] wmu = GetUserList(domain, user);
            if (wmu.Length > 0 && wmu[0].Password == _account.MailIncomingPassword && wmu[0].Type == "U")
            {
            }
            else
            {
                throw new WebMailException(_resMan.GetString("ErrorPOP3IMAP4Auth"));
            }
        }

		protected virtual void ConnectAndLoginAdmin()
		{
			try
			{

				WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				_tcpClient = new TcpClient();
				_tcpClient.Connect(_hostname, _port);
				_nwStream = _tcpClient.GetStream();

				ReceiveResponse(); // receive hello from server

				SendRequest(string.Format("{0}	{1}\r\n", settings.XMailLogin, settings.XMailPass));

				ReceiveResponse();
			}
			catch(Exception)
			{
				throw new WebMailException(_resMan.GetString("ErrorPOP3Connect"));
			}
		}

		public override void DeleteMessages(object[] messageIndexSet, Folder fld)
		{
			try
			{
				string mailboxPath = GetMailBoxPath();
				for(int i = 0; messageIndexSet.Length > i; i++)
				{
					string msgPath = Path.Combine(mailboxPath, messageIndexSet[i].ToString());
					if (File.Exists(msgPath))
					{
						File.Delete(msgPath);
					}
				}
			}
			catch (Exception ex)
			{
				throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_DEL_MSGS"));
			}
		}

		protected virtual void DisconnectAdmin()
		{
			if (_nwStream != null) _nwStream.Close();
			if (_tcpClient != null) _tcpClient.Close();
		}

		public override int GetFolderMessageCount(string fullPath)
		{
			try
			{
				string path = GetMailBoxPath();
				if (Directory.Exists(path))
				{
					DirectoryInfo dir = new DirectoryInfo(path);
					return dir.GetFiles().Length;
				}
				return 0;
			}
			catch (Exception ex)
			{
				throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_GET_MESSAGES_COUNT"));
			}
		}

		public override int GetFolderUnreadMessageCount(string fullPath)
		{
			return 0;
		}

		public override long GetMailStorageSize()
		{
			try
			{
				string path = GetMailBoxPath();
				if (Directory.Exists(path))
				{
					DirectoryInfo dir = new DirectoryInfo(path);
					FileInfo[] fileInfos = dir.GetFiles();
					long size = 0;
					foreach (FileInfo fi in fileInfos) 
					{
						size += fi.Length;
					}
					return size;
				}
				return 0;
			}
			catch (Exception ex)
			{
				throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_MAIL_SIZE"));
			}
		}

		public override bool IsConnected()
		{
			return base.IsConnected();
		}

		public override WebMailMessage LoadMessage(object index, Folder fld)
		{
			return LoadMessage(index, fld, false);
		}

		public override WebMailMessageCollection LoadMessageHeaders(int pageNumber, Folder fld)
		{
			WebMailMessageCollection messages = new WebMailMessageCollection();
			try
			{
				string path = GetMailBoxPath();

				DirectoryInfo dir = new DirectoryInfo(path);
				FileInfo[] fis = dir.GetFiles();

				Array.Sort(fis, new FileDateComparer());

				WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				int msgPerPage = settings.MailsPerPage;
				int msgCount = fis.Length;
			
				if ((_account != null)
					&& (_account.UserOfAccount != null)
					&& (_account.UserOfAccount.Settings != null))
				{
					msgPerPage = this._account.UserOfAccount.Settings.MsgsPerPage;
				}

				int startIndex = 0;
				int length = msgPerPage;
				if ((pageNumber * msgPerPage) > msgCount)
				{
					if (msgCount > msgPerPage)
					{
						length = msgCount % msgPerPage;
					}
					else
					{
						length = msgCount;
					}
				}
				else
				{
					startIndex = (msgCount - (pageNumber * msgPerPage));
				}
				for (int i = 0; i < length; i++)
				{
					if (((startIndex + i) < msgCount) && ((startIndex + i) >= 0))
					{
						messages.Add(LoadMessage(fis[startIndex + i].Name, fld, true));
					}
				}
				if (fld.SyncType == FolderSyncType.DirectMode)
				{
					CollectionBase cb = messages as CollectionBase;
					if (cb != null)
					{
						Utils.ReverseCollection(ref cb);
					}
				}
				return messages;
			}
			catch (Exception ex)
			{
				throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_GET_MSG_LIST"));
			}
		}

		public override WebMailMessageCollection LoadMessageHeaders(object[] messageIndexSet, Folder fld)
		{
			return LoadMessages(messageIndexSet, fld, true);
		}

		public override WebMailMessageCollection LoadMessageHeaders(Folder fld)
		{
			return LoadMessages(fld, true);
		}

		public override WebMailMessageCollection LoadMessages(object[] messageIndexSet, Folder fld)
		{
			return LoadMessages(messageIndexSet, fld, false);
		}

		public override WebMailMessageCollection LoadMessages(Folder fld)
		{
			return LoadMessages(fld, false);
		}

		public override void Synchronize(FolderCollection foldersTree)
		{
			DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(_account);
			FolderCollection folders = new FolderCollection();
			FolderCollection.CreateFolderListFromTree(ref folders, foldersTree);

			ArrayList savedUids = new ArrayList();
			ArrayList serverUids = new ArrayList();
			ArrayList dbUidsToDelete = new ArrayList();
			ArrayList dbUids = new ArrayList();

			Folder fld = folders[FolderType.Inbox];
			if (fld != null)
			{
				// if folder synchronization is "don't synchronize" or "direct mode",
				// then do not need to synchronize
				if ((fld.SyncType == FolderSyncType.DontSync) || (fld.SyncType == FolderSyncType.DirectMode)) return;
				
				try
				{
					dbStorage.Connect();

					serverUids.AddRange(GetUids());
					dbUids.AddRange(dbStorage.GetUids());

					serverUids.Sort(Comparer.DefaultInvariant);
					// leave only those serverUids that are not present in dbUids
					// uids that are also present in serverUids, and dbUids, add to dbUidsToDelete
					foreach (string dbUid in dbUids)
					{
						int index = serverUids.BinarySearch(dbUid);
						if (index >= 0)
						{
							serverUids.RemoveAt(index);
						}
						else
						{
							dbUidsToDelete.Add(dbUid);
						}
					}

					// if folder synchronization is "all messages" or "all headers" 
					// then remove from db messages with uids from dbUidsToDelete
					if ((fld.SyncType == FolderSyncType.AllEntireMessages) || (fld.SyncType == FolderSyncType.AllHeadersOnly))
					{
						if (dbUidsToDelete.Count > 0)
						{
							WebMailMessageCollection msgsToDelete = dbStorage.LoadMessagesByUids((string[])dbUidsToDelete.ToArray(typeof(string)), fld);
							dbStorage.DeleteMessages(msgsToDelete.ToIDsCollection(), fld);
						}
					}

					// deleting old messages from the mail server, if exhibited setting KeepMessagesOnServer
                    if (_account.MailMode == MailMode.KeepMessagesOnServer || _account.MailMode == MailMode.KeepMessagesOnServerAndDeleteMessageWhenItsRemovedFromTrash)
					{
						object[] serverUidsToDelete = dbStorage.GetOldMessagesUids(_account.MailsOnServerDays);
						if (serverUidsToDelete.Length > 0)
						{
							DeleteMessages(serverUidsToDelete, fld);
						}
					}

					if (serverUids.Count > 0)
					{
						// receive new messages
						WebMailMessageCollection newMessages = LoadMessages(serverUids.ToArray(), fld);
						ApplyXSpam(newMessages);
						ApplyFilters(newMessages, dbStorage, fld, ref savedUids);

						if (_account.MailMode == MailMode.DeleteMessagesFromServer)
						{
                            if ((fld.SyncType == FolderSyncType.AllHeadersOnly) || (fld.SyncType == FolderSyncType.NewHeadersOnly))
                            {
								Log.WriteLine("Synchronize", "Incorrect Settings: " + _account.MailMode.ToString() + " + " + fld.SyncType.ToString());
                            }
                            else
                            {
                                DeleteMessages(newMessages.ToUidsCollection(true), null);
                            }
						}
					}
				}
				catch (WebMailException)
				{
					dbStorage.SaveUids(savedUids.ToArray());
					throw;
				}
				finally
				{
					dbStorage.ReplaceUids(GetUids());
					dbStorage.Disconnect();
				}
			}

			base.Synchronize(foldersTree);
		}

		#region Admin Functions
		
		public virtual void AddDomain(string domain)
		{
			try
			{
				ExecuteAdminCommand(string.Format(@"domainadd	{0}", domain));
			}
			catch (WebMailWMServerException ex)
			{
				throw PrepareFriendlyException(ex, "Create domain failed.");
			}
		}

		public virtual void DeleteDomain(string domain)
		{
			try
			{
				ExecuteAdminCommand(string.Format(@"domaindel	{0}", domain));
			}
			catch (WebMailWMServerException ex)
			{
				throw PrepareFriendlyException(ex, "Delete domain failed.");
			}
		}

		public virtual string[] GetDomainList()
		{
			try
			{
				ExecuteAdminCommand(string.Format("domainlist"));
			}
			catch (WebMailWMServerException ex)
			{
				throw PrepareFriendlyException(ex, "Retrieve domain list failed.");
			}
			if (_response != null)
			{
				string[] strs = PrepareResultList();
				ArrayList list = new ArrayList();
				foreach (string str in strs)
				{
					list.Add(str.Trim(new char[] { '"' }));
				}
				return (string[])list.ToArray(typeof(string));
			}
			return new string[0];
		}

		public virtual bool IsDomainExists(string domain)
		{
			string[] domains = GetDomainList();
			foreach (string d in domains)
			{
				if (string.Compare(domain, d, true, CultureInfo.InvariantCulture) == 0)
				{
					return true;
				}
			}
			return false;
		}

		public virtual void AddUser(string domain, string user, string password)
		{
			try
			{
				ExecuteAdminCommand(string.Format(@"useradd	{0}	{1}	{2}	U", domain, user, password));
			}
			catch (WebMailWMServerException ex)
			{
				throw PrepareFriendlyException(ex, "Create account failed.");
			}
		}

		public virtual void DeleteUser(string domain, string user)
		{
			try
			{
				ExecuteAdminCommand(string.Format(@"userdel	{0}	{1}", domain, user));
			}
			catch (WebMailWMServerException ex)
			{
				throw PrepareFriendlyException(ex, "Delete account failed.");
			}
		}

        //public virtual WMServerUser[] GetUserList()
        //{
        //    return GetUserList(null, null);
        //}

        public virtual WMServerUser[] GetUserList(string domain)
        {
            return GetUserList(domain, null);
        }

		public virtual WMServerUser[] GetUserList(string domain, string user)
		{
			try
			{
				string command = "userlist";
				if (((domain != null) && (domain.Length > 0)) && ((user != null) && (user.Length > 0)))
				{
					command = string.Format(@"userlist	{0}	{1}", domain, user);
				}
				else if ((domain != null) && (domain.Length > 0))
				{
					command = string.Format(@"userlist	{0}", domain);
				}
				ExecuteAdminCommand(command);
			}
			catch (WebMailWMServerException ex)
			{
				throw PrepareFriendlyException(ex, "Retrieve accounts list failed.");
			}
			if (_response != null)
			{
				string[] strs = PrepareResultList();
				ArrayList list = new ArrayList();
				foreach (string str in strs)
				{
					list.Add(WMServerUser.Parse(str));
				}
				return (WMServerUser[])list.ToArray(typeof(WMServerUser));
			}
			return new WMServerUser[0];
		}

		public virtual bool IsUserExists(string user)
		{
			return IsUserExists(null, user);
		}

		public virtual bool IsUserExists(string domain, string user)
		{
			WMServerUser[] users = GetUserList(domain, null);
			if ((users != null) && (users.Length > 0))
			{
				foreach (WMServerUser u in users)
				{
					if (string.Compare(u.Name, user, true, CultureInfo.InvariantCulture) == 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public virtual void ChangeUserPassword(string domain, string user, string newPassword)
		{
			try
			{
				ExecuteAdminCommand(string.Format(@"userpasswd	{0}	{1}	{2}", domain, user, newPassword));
			}
			catch (WebMailWMServerException ex)
			{
				throw PrepareFriendlyException(ex, "Changing account password failed.");
			}
		}

		#endregion

		protected void SendRequest(string command)
		{
			if (_nwStream == null) throw new WebMailWMServerException(_resMan.GetString("PROC_CANT_GET_MSG_LIST"));

			byte[] sendBytes = Encoding.UTF8.GetBytes(command);
			_nwStream.Write(sendBytes, 0, sendBytes.Length);
		}

		protected string ReceiveResponse()
		{
			if (_nwStream == null) throw new WebMailWMServerException(_resMan.GetString("PROC_CANT_GET_MSG_LIST"));

			bool multiline = false;
			StreamReader sr = new StreamReader(_nwStream, true);
			_response = sr.ReadLine();
			if (!IsPositiveResponse(_response, out multiline))
			{
				throw new WebMailWMServerException(_response);
			}
			if (multiline)
			{
				StringBuilder sb = new StringBuilder();
				string str = string.Empty;
				do
				{
					str = sr.ReadLine();
					sb.Append(str + "\r\n");
				}
				while (str != ".");
				_response = sb.ToString();
			}
			return _response;
		}

		protected bool IsPositiveResponse(string response, out bool multiline)
		{
			multiline = false;
			if ((response != null) && (response.Length > 0))
			{
				if (response.Length >= 6)
				{
					multiline = (string.Compare(response.Substring(1, 5), "00100", true, CultureInfo.InvariantCulture) == 0) ? true : false;
				}
				return (response[0] == '+') ? true : false;
			}
			return false;
		}

		protected void ExecuteAdminCommand(string command)
		{
			if (command == null) throw new WebMailWMServerException(Constants.wmServCmdNull);

			if (!command.EndsWith("\r\n")) command += "\r\n";

			try
			{
				ConnectAndLoginAdmin();

				SendRequest(command);

				ReceiveResponse();

				SendRequest("quit\r\n");

				//ReceiveResponse(); // comment because output results will be overwrite with OK by QUIT
			}
			catch (WebMailException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new WebMailWMServerException(ex);
			}
			finally
			{
				DisconnectAdmin();
			}
		}

		protected WebMailWMServerException PrepareFriendlyException(Exception ex, string friendlyMessage)
		{
			string message = string.Format(@"{0}

Server response: {1}", friendlyMessage, ex.Message);
			return new WebMailWMServerException(message);
		}

		private string[] PrepareResultList()
		{
			ArrayList result = Utils.Split(_response, "\r\n");
	
			string lastStr = (string)result[result.Count - 1];
			if (string.Compare(lastStr, ".", true, CultureInfo.InvariantCulture) == 0) result.RemoveAt(result.Count - 1);
	
			return (string[])result.ToArray(typeof(string));
		}

		protected string GetMailBoxPath()
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			string[] temp = _account.Email.Split('@');
			string strLogin = temp[0];
			string strDomain = temp[1];
			return Path.Combine(settings.WmServerRootPath, @"domains\" + strDomain + @"\" +  strLogin + @"\mailbox");
		}

		protected string[] GetUids()
		{
			ArrayList result = new ArrayList();
			try
			{
				string path = GetMailBoxPath();
				DirectoryInfo dir = new DirectoryInfo(path);
				FileInfo[] fileInfos = dir.GetFiles();
				foreach (FileInfo fi in fileInfos)
				{
					string filename = fi.Name;
					result.Add(filename);
				}
			}
			catch (Exception ex)
			{
				throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_GET_MSG_LIST"));
			}
			return (string[])result.ToArray(typeof(string));
		}

		protected WebMailMessage LoadMessage(object index, Folder fld, bool headersOnly)
		{
			try
			{
				MailMessage message = new MailMessage();
				message.Parser.ParseHeaderOnly = headersOnly;

				string path = GetMailBoxPath();
				path = Path.Combine(path, index.ToString());

				OnMessageDownloaded(new CheckMailEventArgs(fld.Name, _msgsCount, _msgNumber++));
				message.LoadMessage(path);

				WebMailMessage msg = new WebMailMessage(_account);
				msg.Init(message, ((msg.StrUid != null) && (msg.StrUid.Length > 0)), fld);
				if (fld.SyncType == FolderSyncType.DirectMode) msg.Seen = true;
				msg.StrUid = index.ToString();

				return msg;
			}
			catch (Exception ex)
			{
				throw PrepareFriendlyException(ex, _resMan.GetString("PROC_MSG_HAS_DELETED"));
			}
		}

		protected WebMailMessageCollection LoadMessages(object[] messageIndexSet, Folder fld, bool headersOnly)
		{
			_msgNumber = 1;
			_msgsCount = messageIndexSet.Length;
			WebMailMessageCollection messages = new WebMailMessageCollection();
			try
			{
				for(int i = 0; i < messageIndexSet.Length; i++)
				{
					WebMailMessage wmMsg = LoadMessage(messageIndexSet[i], fld, headersOnly);
					messages.Add(wmMsg);
				}
				return messages;
			}
			catch (Exception ex)
			{
				throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_GET_MSG_LIST"));
			}
			finally
			{
				_msgNumber = 0;
				_msgsCount = 0;
			}
		}

		protected WebMailMessageCollection LoadMessages(Folder fld, bool headersOnly)
		{
			WebMailMessageCollection messages = new WebMailMessageCollection();
			try
			{
				string path = GetMailBoxPath();
				DirectoryInfo dir = new DirectoryInfo(path);
				FileInfo[] fis = dir.GetFiles();
				for(int i = 0; i < fis.Length; i++)
				{
					WebMailMessage webMsg = LoadMessage(fis[i].Name, fld, headersOnly);
					messages.Add(webMsg);
				}
				return messages;
			}
			catch (Exception ex)
			{
				throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_GET_MSG_LIST"));
			}
		}

		public virtual void ChangeAdminPassword(string newLogin, string newPassword)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			try
			{
				string serverPath = Path.Combine(settings.WmServerRootPath, "domains");
				if (Directory.Exists(serverPath))
				{
					string paswFilename = Path.Combine(settings.WmServerRootPath, "ctrlaccounts.tab");
					if (File.Exists(paswFilename))
					{
						using (StreamWriter sw = new StreamWriter(File.Open(paswFilename, FileMode.Create)))
						{
							sw.WriteLine(string.Format(@"""{0}""	""{1}""", newLogin, EncryptPassword(newPassword)));
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new WebMailWMServerException("Change admin password failed.", ex);
			}
		}

		protected virtual string EncryptPassword(string password)
		{
			StringBuilder sb = new StringBuilder();
			byte[] bytes = Encoding.UTF8.GetBytes(password);
			for (int i = 0; i < bytes.Length; i++)
			{
				byte b = (byte)((bytes[i] ^ 101) & 0xFF);
				sb.Append(b.ToString("X2"));
			}
			return sb.ToString();
		}

        public static string DecryptPassword(string password)
        {
            if (password == null) return string.Empty;
            string result = string.Empty;
            if ((password.Length > 0) && (password.Length % 2 == 0))
            {
                byte[] decryptedBytes = new byte[password.Length / 2];
                int startIndex = 0;
                int index = 0;
                while (startIndex < password.Length)
                {
                    string strByte = password.Substring(startIndex, 2);
                    int b = 0;
                    if (int.TryParse(strByte, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out b))
                    {
                        b = ((b & 0xFF) ^ 101);
                        decryptedBytes[index] = (byte)b;
                    }
                    startIndex += 2;
                    index++;
                }
                result = Encoding.UTF8.GetString(decryptedBytes);
            }
            return result;
        }

	}
}
