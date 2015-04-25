using System;
using System.Collections;
using System.IO;
using MailBee;
using MailBee.Mime;
using MailBee.Pop3Mail;
using MailBee.Security;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for Pop3Storage.
	/// </summary>
	public class Pop3Storage : MailServerStorage
	{
		protected Pop3 _pop3Obj;

		public Pop3Storage(Account account) : base(account)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

			try
			{
				Pop3.LicenseKey = settings.LicenseKey;
				_pop3Obj = new Pop3();
				_pop3Obj.MessageDownloaded += new Pop3MessageDownloadedEventHandler(_pop3Obj_MessageDownloaded);

				if (settings.EnableLogging)
				{
					_pop3Obj.Log.Enabled = true;
					string dataFolderPath = Utils.GetDataFolderPath();
					if (dataFolderPath != null)
					{
						_pop3Obj.Log.Filename = Path.Combine(dataFolderPath, Constants.logFilename);
					}
				}
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
		}

		public override void Connect()
		{
			System.Threading.Thread.Sleep(50);
			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			try
			{
				_pop3Obj.InboxPreloadOptions = Pop3InboxPreloadOptions.Uidl;
				if (_account.MailIncomingPort == 995)
				{
					_pop3Obj.SslMode = SslStartupMode.OnConnect;
					_pop3Obj.SslProtocol = SecurityProtocol.Auto;
					_pop3Obj.SslCertificates.AutoValidation = CertificateValidationFlags.None;
				}
				Log.WriteLine("POP3 Connect", string.Format(@"'{0}' - '{1}'", _account.MailIncomingHost, _account.MailIncomingPort));
				Log.WriteLine("POP3 Connect", string.Format(@"IsConnected - '{0}'", _pop3Obj.IsConnected));
				_pop3Obj.Connect(_account.MailIncomingHost, _account.MailIncomingPort, true);
				_pop3Obj.Login(_account.MailIncomingLogin, _account.MailIncomingPassword, AuthenticationMethods.Auto, AuthenticationOptions.PreferSimpleMethods, null);
			}
			catch (MailBeeConnectionException ex)
			{
				Log.WriteException(ex);
				throw new WebMailException(resMan.GetString("ErrorPOP3Connect"));
			}
			catch (MailBeePop3LoginBadCredentialsException)
			{
				throw new WebMailException(resMan.GetString("ErrorPOP3IMAP4Auth"));
			}
			catch(MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
		}

		public override bool IsConnected()
		{
			return _pop3Obj.IsConnected;
		}

		public override void Disconnect()
		{
			try
			{
				if (IsConnected()) _pop3Obj.Disconnect();
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
		}

		public override long GetMailStorageSize()
		{
			return _pop3Obj.InboxSize;
		}


		public override WebMailMessageCollection LoadMessageHeaders(int pageNumber, Folder fld)
		{
			object[] indexes = GetIndexesFromPageNumber(pageNumber);
			WebMailMessageCollection coll = LoadMessageHeaders(indexes, fld);
            if (fld.SyncType == FolderSyncType.DirectMode)
            {
				CollectionBase cb = coll as CollectionBase;
				if (cb != null)
				{
					Utils.ReverseCollection(ref cb);
				}
            }
			return coll;
		}


		public override WebMailMessageCollection LoadMessageHeaders(Folder fld)
		{
			WebMailMessageCollection returnColl = new WebMailMessageCollection();
			MailMessageCollection messages = null;
			try
			{
				messages = _pop3Obj.DownloadMessageHeaders();
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
			if (messages != null)
			{
				foreach (MailMessage msg in messages)
				{
					WebMailMessage webMsg = new WebMailMessage(_account);
					webMsg.Init(msg, true, fld);
					returnColl.Add(webMsg);
				}
			}
			return returnColl;
		}

		public override WebMailMessageCollection LoadMessageHeaders(object[] messageIndexSet, Folder fld)
		{
			WebMailMessageCollection msgsColl = new WebMailMessageCollection();
			foreach (object uid in messageIndexSet)
			{
				int index = _pop3Obj.GetMessageIndexFromUid(Convert.ToString(uid));
                if (index > 0)
				{
					try
					{
						WebMailMessage webMsg = new WebMailMessage(_account);
						webMsg.Init(_pop3Obj.DownloadMessageHeader(index), true, fld);
						if (fld.SyncType == FolderSyncType.DirectMode) webMsg.Seen = true;
						msgsColl.Add(webMsg);
					}
					catch(MailBeeException ex)
					{
						throw new WebMailMailBeeException(ex);
					}
				}
				else
				{
					continue;
				}
			}
			return msgsColl;
		}

		public override WebMailMessageCollection LoadMessages(Folder fld)
		{
			WebMailMessageCollection returnColl = new WebMailMessageCollection();
			MailMessageCollection messages = null;
			try
			{
				messages = _pop3Obj.DownloadEntireMessages();
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
			if (messages != null)
			{
				foreach (MailMessage msg in messages)
				{
					WebMailMessage webMsg = new WebMailMessage(_account);
					webMsg.Init(msg, true, fld);
					returnColl.Add(webMsg);
				}
			}
			return returnColl;
		}

		public override WebMailMessageCollection LoadMessages(object[] messageIndexSet, Folder fld)
		{
			WebMailMessageCollection msgsColl = new WebMailMessageCollection();
			foreach (string uid in messageIndexSet)
			{
				int index = _pop3Obj.GetMessageIndexFromUid(uid);
                if (index > 0)
				{
					try
					{
						WebMailMessage webMsg = new WebMailMessage(_account);
						webMsg.Init(_pop3Obj.DownloadEntireMessage(index), true, fld);
						msgsColl.Add(webMsg);
					}
					catch (MailBeeException ex)
					{
						throw new WebMailMailBeeException(ex);
					}
				}
				else
				{
					continue;
				}
			}
			return msgsColl;
		}

		public override WebMailMessage LoadMessage(object index, Folder fld)
		{
			int indexOnServer = _pop3Obj.GetMessageIndexFromUid(index.ToString());
            try
			{
				if (indexOnServer > 0)
				{
					WebMailMessage webMsg = new WebMailMessage(_account);
					MailMessage msg = _pop3Obj.DownloadEntireMessage(indexOnServer);
					webMsg.Init(msg, true, fld);
					return webMsg;
				}
				else
				{
					throw new WebMailMailBoxException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("PROC_MSG_HAS_DELETED"));
				}
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
		}

		public override void DeleteMessages(object[] messageIndexSet, Folder fld)
		{
			foreach (string uid in messageIndexSet)
			{
				int index = _pop3Obj.GetMessageIndexFromUid(uid);
                try
				{
					if (index > 0)
					{
						OnMessageDeleted(new DeleteMessageEventArgs(index, (fld != null) ? fld.FullPath : string.Empty));
						_pop3Obj.DeleteMessage(index);
					}
				}
				catch (MailBeeException ex)
				{
					throw new WebMailMailBeeException(ex);
				}
			}
		}

		public override int GetFolderMessageCount(string fullPath)
		{
			return _pop3Obj.InboxMessageCount;
		}

		public override int GetFolderUnreadMessageCount(string fullPath)
		{
			return 0;
		}

		public override void Synchronize(FolderCollection foldersTree)
		{
			DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(_account);
			FolderCollection folders = new FolderCollection();
			FolderCollection.CreateFolderListFromTree(ref folders, foldersTree);
			ArrayList pop3Uids = new ArrayList();
			bool downloadErrorOccured = false;
			//Exception downloadException = null;
			
			Folder fld = folders[FolderType.Inbox];
			if (fld != null)
			{
				if ((fld.SyncType == FolderSyncType.DontSync) || (fld.SyncType == FolderSyncType.DirectMode)) return;
                try
				{
					dbStorage.Connect();

					// get new messages start index
					//WebMailMessageCollection messageCollection = dbStorage.LoadMessages(fld);
					string[] pop3DbUids = dbStorage.GetUids();
					ArrayList dbUidsToDelete = new ArrayList();
					//ArrayList uidsToDelete = new ArrayList();
					int newMsgStartIndex = 0;
					for (int i = pop3DbUids.Length - 1; i >= 0; i--)
					{
						int index = _pop3Obj.GetMessageIndexFromUid(pop3DbUids[i].ToString());
                        if ((index <= _pop3Obj.InboxMessageCount) && (index > 0)) 
						{
							if (newMsgStartIndex < 1)
							{
								newMsgStartIndex = index;
							}
							else
							{
								if ((fld.SyncType == FolderSyncType.NewEntireMessages) || (fld.SyncType == FolderSyncType.NewHeadersOnly))
								{
									break;
								}
							}
						}
						else
						{
							if ((fld.SyncType == FolderSyncType.AllEntireMessages) || (fld.SyncType == FolderSyncType.AllHeadersOnly))
							{
								dbUidsToDelete.Add(pop3DbUids[i]);
							}
						}
					}
					if (_account.MailMode == MailMode.KeepMessagesOnServer || _account.MailMode == MailMode.KeepMessagesOnServerAndDeleteMessageWhenItsRemovedFromTrash)
					{
						object[] serverUidsToDelete = dbStorage.GetOldMessagesUids(_account.MailsOnServerDays);
						// delete from server
						if (serverUidsToDelete.Length > 0)
						{
							this.DeleteMessages(serverUidsToDelete, fld);
						}
					}
					// retrieve new messages
					GetNewMessagesFromServerAndSaveToDb(dbStorage, fld, newMsgStartIndex, out downloadErrorOccured, ref pop3Uids);

					if ((fld.SyncType == FolderSyncType.NewEntireMessages) || (fld.SyncType == FolderSyncType.NewHeadersOnly))
					{
						return;
					}
					// delete messages from db
					if (dbUidsToDelete.Count > 0)
					{
						WebMailMessageCollection msgsToDelete = dbStorage.LoadMessagesByUids((string[])dbUidsToDelete.ToArray(typeof(string)), fld);
						dbStorage.DeleteMessages(msgsToDelete.ToIDsCollection(), fld);
					}
				}
				catch (WebMailException)
				{
					throw;
				}
				finally
				{
					if (!downloadErrorOccured)
					{
						// get all uids and save it
						pop3Uids = ArrayList.Adapter(_pop3Obj.GetMessageUids());
						dbStorage.ReplaceUids(pop3Uids.ToArray());
					}
					else
					{
						dbStorage.SaveUids(pop3Uids.ToArray());
					}
					dbStorage.Disconnect();
				}
			}
		}

		private void GetNewMessagesFromServerAndSaveToDb(DbStorage dbStorage, Folder fld, int newMsgStartIndex, out bool errorOccured, ref ArrayList pop3Uids)
		{
			_folderName = fld.Name;
			_msgNumber = 1;
			_msgsCount = _pop3Obj.InboxMessageCount - newMsgStartIndex;
			MailMessageCollection mailMessageCollection = new MailMessageCollection();
			int maxMsgsPersession = Constants.DownloadChunk;
			try
			{
				do
				{
					int downloadMsgsCount = ((_pop3Obj.InboxMessageCount - newMsgStartIndex) > maxMsgsPersession) ? maxMsgsPersession : _pop3Obj.InboxMessageCount - newMsgStartIndex;
					if ((_pop3Obj.InboxMessageCount - newMsgStartIndex) > 0)
					{
						if ((fld.SyncType == FolderSyncType.NewEntireMessages) || (fld.SyncType == FolderSyncType.AllEntireMessages))
						{
							mailMessageCollection = _pop3Obj.DownloadEntireMessages(newMsgStartIndex + 1, downloadMsgsCount);
						}
						if ((fld.SyncType == FolderSyncType.NewHeadersOnly) || (fld.SyncType == FolderSyncType.AllHeadersOnly))
						{
							mailMessageCollection = _pop3Obj.DownloadMessageHeaders(newMsgStartIndex + 1, downloadMsgsCount);
						}
					}
					WebMailMessageCollection coll = new WebMailMessageCollection(_account, mailMessageCollection, true, fld);
					ApplyXSpam(coll);
					ApplyFilters(coll, dbStorage, fld, ref pop3Uids);
					if (_account.MailMode == MailMode.DeleteMessagesFromServer)
					{
                        if ((fld.SyncType == FolderSyncType.AllHeadersOnly) || (fld.SyncType == FolderSyncType.NewHeadersOnly))
                        {
                            Log.WriteLine("GetNewMessagesFromServerAndSaveToDb", "Incorrect Settings: " + _account.MailMode.ToString() + " + " + fld.SyncType.ToString());
                        }
                        else
                        {
                            DeleteMessages(coll.ToUidsCollection(true), null);
                        }
					}
					newMsgStartIndex += downloadMsgsCount;
				}
				while (newMsgStartIndex < _pop3Obj.InboxMessageCount);
				errorOccured = false;
			}
			catch (Exception ex)
			{
				errorOccured = true;
				throw new WebMailException(ex);
			}
			_folderName = "";
			_msgNumber = 0;
			_msgsCount = 0;
		}

		private object[] GetIndexesFromPageNumber(int pageNumber)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			int msgPerPage = settings.MailsPerPage;
			int msgCount = _pop3Obj.InboxMessageCount;
			
			if ((_account != null)
				&& (_account.UserOfAccount != null)
				&& (_account.UserOfAccount.Settings != null))
			{
				msgPerPage = this._account.UserOfAccount.Settings.MsgsPerPage;
			}
			
			int startIndex = 1;
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
				startIndex = (msgCount - (pageNumber * msgPerPage)) + 1;
			}
			
			//object[] ids = new object[(end - start) + 1];
			ArrayList arr = new ArrayList();
			for (int i = 0; i < length; i++)
			{
				if (((startIndex + i) <= _pop3Obj.InboxMessageCount) && ((startIndex + i) > 0))
				{
					arr.Add(_pop3Obj.GetMessageUidFromIndex(startIndex + i));
				}
			}
			return arr.ToArray();
		}

		private void _pop3Obj_MessageDownloaded(object sender, Pop3MessageDownloadedEventArgs e)
		{
			OnMessageDownloaded(new CheckMailEventArgs(_folderName, _msgsCount, _msgNumber));
			_msgNumber++;
		}
	}
}