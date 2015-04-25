using System.Globalization;
using MailBee.ImapMail;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for MailProcessor.
	/// </summary>
	public class MailProcessor
	{
		public event DownloadedMessageHandler MessageDownloaded;
		public event DeleteMessageHanlder MessageDeleted;


		protected DbStorage _dbStorage;
		protected MailServerStorage _serverStorage;

		private MailProcessor()
		{
			_dbStorage = null;
		}

		public MailProcessor(DbStorage storage) : this()
		{
			_dbStorage = storage;
		}

		public Account MailAccount
		{
			get { return _dbStorage.Account; }
		}

		public DbStorage DatabaseStorage
		{
			get { return _dbStorage; }
			//set { _dbStorage = value; }
		}

		public MailServerStorage ServerStorage
		{
			get { return _serverStorage; }
		}

		protected virtual void OnMessageDownloaded(CheckMailEventArgs e) 
		{
			if (MessageDownloaded != null)
				MessageDownloaded(this, e);
		}

		protected virtual void OnMessageDeleted(DeleteMessageEventArgs e)
		{
			if (MessageDeleted != null)
				MessageDeleted(this, e);
		}

		public void Connect()
		{
			_dbStorage.Connect();
		}

		public void Disconnect()
		{
			if (_dbStorage.IsConnected()) _dbStorage.Disconnect();
		}

		public Folder CreateFolder(int id_parent, string parent_full_name, string name, int create)
		{
			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

			if ((name == null) || (name.Length == 0))
			{
				throw new WebMailException(resMan.GetString("WarningEmptyFolderName"));
			}
			if (!FileSystem.IsFolderNameValid(name))
			{
				throw new WebMailIOException(resMan.GetString("WarningCorrectFolderName"));
			}

			Folder fld = new Folder();
			fld.IDAcct = _dbStorage.Account.ID;
			fld.IDParent = id_parent;
			fld.Name = name;
			fld.FullPath = ((parent_full_name != null) && (parent_full_name.Length > 0)) ? string.Format("{0}{1}{2}", parent_full_name, _dbStorage.Account.Delimiter, name) : name;
			//only in webmail
			if (create == 0) fld.SyncType = FolderSyncType.DontSync;
			//in webmail and mail server
            if (create == 1 && _dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Imap4)
			{
                fld.SyncType = FolderSyncType.AllHeadersOnly;
                if (_dbStorage.Account.UserOfAccount.Settings.AllowDirectMode && settings.DirectModeIsDefault)
                {
                    fld.SyncType = FolderSyncType.DirectMode;
                }

				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				ImapStorage imapStorage = _serverStorage as ImapStorage;
				if (imapStorage != null)
				{
					try
					{
						imapStorage.Connect();
						imapStorage.CreateFolder(fld);
					}
					finally
					{
						imapStorage.Disconnect();
					}
				}
			}
			FileSystem fs = new FileSystem(_dbStorage.Account.Email, _dbStorage.Account.ID, true);
			fs.CreateFolder(fld.GetFullPath(_dbStorage.Account.Delimiter));
			return _dbStorage.CreateFolder(fld);
		}

		public void DeleteFolders(Folder[] folders)
		{
			_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
			ImapStorage imapStorage = _serverStorage as ImapStorage;
			if (imapStorage != null)
			{
				try
				{
					imapStorage.Connect();
					foreach (Folder imapFld in folders)
					{
                        Folder tmp = _dbStorage.GetFolder(imapFld.ID);
                        if (tmp.SyncType != FolderSyncType.DontSync)
                        {
                            imapStorage.DeleteFolder(tmp);
                        }
					}
				}
				finally
				{
					imapStorage.Disconnect();
				}
			}
			FileSystem fs = new FileSystem(_dbStorage.Account.Email, _dbStorage.Account.ID, true);
			foreach (Folder dbFld in folders)
			{
				fs.DeleteFolder(dbFld.GetFullPath(_dbStorage.Account.Delimiter));
				_dbStorage.DeleteFolder(dbFld);
			}
		}

		public void UpdateFolders(Folder[] folders, bool updateRemoteFolders)
		{
			FileSystem fs = new FileSystem(_dbStorage.Account.Email, _dbStorage.Account.ID, true);
			foreach (Folder dbFld in folders)
			{
				WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
				if ((dbFld.Name == null) || (dbFld.Name.Length == 0))
				{
					throw new WebMailException(resMan.GetString("WarningEmptyFolderName"));
				}
				if (!FileSystem.IsFolderNameValid(dbFld.UpdateName))
				{
					throw new WebMailIOException(resMan.GetString("WarningCorrectFolderName"));
				}
				string delimiter = _dbStorage.Account.Delimiter;
				fs.RenameFolder(dbFld.GetFullPath(delimiter), dbFld.GetUpdateFullPath(delimiter));
				_dbStorage.UpdateFolder(dbFld);
			}
			if (updateRemoteFolders)
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				ImapStorage imapStorage = _serverStorage as ImapStorage;
				if (imapStorage != null)
				{
					try
					{
						imapStorage.Connect();
						foreach (Folder imapFld in folders)
						{
							if (imapFld.SyncType != FolderSyncType.DontSync)
							{
								bool exists = false;
								FolderCollection fc = _serverStorage.GetFolders();
								if (fc != null)
								{
                                    FolderCollection tmp_fc = new FolderCollection();
                                    FolderCollection.CreateFolderListFromTree(ref tmp_fc, fc);
                                    foreach (Folder f in tmp_fc)
									{
										if (string.Compare(f.FullPath, imapFld.FullPath, true, CultureInfo.InvariantCulture) == 0)
										{
											exists = true;
											break;
										}
									}
								}
								if (!exists)
								{
									imapStorage.CreateFolder(imapFld);
								}
								else
								{
									imapStorage.UpdateFolder(imapFld);
								}
							}
						}
					}
					finally
					{
						imapStorage.Disconnect();
					}
				}
			}
		}

		public void SendMail(WebMailMessage message)
		{
			Smtp.SendMail(_dbStorage.Account, message, message.FromMsg.ToString(), message.ToMsg.ToString());
		}

		public void SendMail(WebMailMessage message, string from, string to)
		{
			Smtp.SendMail(_dbStorage.Account, message, from, to);
		}

		public void Synchronize(FolderCollection folders)
		{
			_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
			if (_serverStorage != null)
			{
				try
				{
					_serverStorage.MessageDownloaded += new DownloadedMessageHandler(_serverStorage_MessageDownloaded);
					_serverStorage.Connect();
					_serverStorage.Synchronize(folders);
				}
				finally
				{
					_serverStorage.MessageDownloaded -= new DownloadedMessageHandler(_serverStorage_MessageDownloaded);
					_serverStorage.Disconnect();
				}
			}
		}

		public void SynchronizeFolders()
		{
			if ((_dbStorage.Account != null) && (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Imap4))
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
                WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				if (_serverStorage != null)
				{
					FolderCollection serverFoldersTree = null;
					FolderCollection serverFoldersList = new FolderCollection();
					try
					{
						_serverStorage.Connect();
						serverFoldersTree = _serverStorage.GetFolders();
					}
					finally
					{
						_serverStorage.Disconnect();
					}
					if (serverFoldersTree != null)
					{
						FolderCollection.CreateFolderListFromTree(ref serverFoldersList, serverFoldersTree);
						FolderCollection dbFoldersTree = _dbStorage.GetFolders();
						FolderCollection dbFoldersList = new FolderCollection();
						FolderCollection.CreateFolderListFromTree(ref dbFoldersList, dbFoldersTree);
						foreach (Folder fld in serverFoldersList)
                        {
							Folder dbServerFolder = dbFoldersList[fld.FullPath];
							if (dbServerFolder != null)
							{
								if (dbServerFolder.Hide != fld.Hide && fld.Type == FolderType.Custom)
								{
									dbServerFolder.Hide = fld.Hide;
									UpdateFolders(new Folder[] { dbServerFolder }, false);
								}
								continue;
							}
							else
							{
								string delimiter = _dbStorage.Account.Delimiter;
								string folderFullName = fld.FullPath;
								int startIndex = 0;
								do
								{
									string newFolderFullName = null;
									startIndex = folderFullName.IndexOf(delimiter, startIndex);
									if (startIndex < 0)		
									{
										newFolderFullName = folderFullName.Substring(0, folderFullName.Length);
									}
									else
									{
										newFolderFullName = folderFullName.Substring(0, startIndex);
										++startIndex;
									}
									Folder existFld = dbFoldersList[newFolderFullName];
									if (existFld != null)
									{
										fld.IDParent = existFld.ID;
										continue;
									}
									else
									{
                                        if (_dbStorage.Account.UserOfAccount.Settings.AllowDirectMode && settings.DirectModeIsDefault)
                                        {
                                            fld.SyncType = FolderSyncType.DirectMode;
                                        }
										dbFoldersList.Add(_dbStorage.CreateFolder(fld));
									}
								} while (startIndex > 0);
							}
						}
						foreach (Folder fld in dbFoldersList)
						{
							if (serverFoldersList[fld.FullPath] != null)
							{
								continue;
							}
							else
							{
								fld.SyncType = FolderSyncType.DontSync;
								UpdateFolders(new Folder[] {fld}, false);
							}
						}
					}
				}
			}
		}

		public WebMailMessageCollection GetMessageHeaders(int pageNumber, Folder fld)
		{
			if (fld.SyncType == FolderSyncType.DirectMode)
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				try
				{
					_serverStorage.Connect();
					return _serverStorage.LoadMessageHeaders(pageNumber, fld);
				}
				finally
				{
					_serverStorage.Disconnect();
				}
			}
			return _dbStorage.LoadMessageHeaders(pageNumber, fld);
		}

		public WebMailMessage GetMessage(object index, Folder fld)
		{
			if (fld.SyncType == FolderSyncType.DirectMode)
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				try
				{
					_serverStorage.Connect();
					WebMailMessage serverMsg = _serverStorage.LoadMessage(index, fld);
					if (serverMsg != null)
					{
						ImapStorage imapStorage = _serverStorage as ImapStorage;
						if (imapStorage != null)
						{
							imapStorage.SetMessagesFlags(new object[] { index },  SystemMessageFlags.Seen, MessageFlagAction.Add, fld);
						}
					}
					return serverMsg;
				}
				finally
				{
					_serverStorage.Disconnect();
				}
			}

			WebMailMessage msg = _dbStorage.LoadMessage(index, fld);
			if (msg != null)
			{
				if (!msg.Downloaded)
				{
					// load message from mail server
					_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
					try
					{
						_serverStorage.Connect();
						object uid = ((msg.StrUid != null) && (msg.StrUid.Length > 0)) ? (object)msg.StrUid : msg.IntUid;
						WebMailMessage serverMsg = _serverStorage.LoadMessage(uid, fld);
						if (serverMsg != null)
						{
							msg.Init(serverMsg.MailBeeMessage, ((msg.StrUid != null) && (msg.StrUid.Length > 0)), fld);
						}
					}
					finally
					{
						_serverStorage.Disconnect();
					}
				}
			}
			return msg;
		}

		public WebMailMessageCollection GetMessages(Folder fld)
		{
			if (fld.SyncType == FolderSyncType.DirectMode)
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				try
				{
					_serverStorage.Connect();
					return _serverStorage.LoadMessages(fld);
				}
				finally
				{
					_serverStorage.Disconnect();
				}
			}
			return _dbStorage.LoadMessages(fld);
		}

		public WebMailMessageCollection GetMessages(object[] messageIndexSet, bool indexIsUid, Folder fld)
		{
			if (fld.SyncType == FolderSyncType.DirectMode)
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				try
				{
					_serverStorage.Connect();
					return _serverStorage.LoadMessages(messageIndexSet, fld);
				}
				finally
				{
					_serverStorage.Disconnect();
				}
			}
			return _dbStorage.LoadMessages(messageIndexSet, fld);
		}

		public FolderCollection GetFolders()
		{
			return GetFolders(false);
		}

		public FolderCollection GetFolders(bool asList)
		{
			FolderCollection fc = _dbStorage.GetFolders();
			if (asList)
			{
				FolderCollection resultList = new FolderCollection();
				FolderCollection.CreateFolderListFromTree(ref resultList, fc);
				return resultList;
			}
			return fc;
		}

		public Folder GetFolder(string folderFullName)
		{
			return _dbStorage.GetFolder(folderFullName);
		}

		public Folder GetFolder(FolderType type)
		{
			return _dbStorage.GetFolder(type);
		}

		public Folder GetFolder(long id_folder)
		{
			return _dbStorage.GetFolder(id_folder);
		}

		public int GetFolderMessageCount(Folder fld)
		{
			if (fld.SyncType == FolderSyncType.DirectMode)
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				try
				{
					Log.WriteLine(@"GetFolderMessageCount", "Connect");
					_serverStorage.Connect();
					return _serverStorage.GetFolderMessageCount(fld.FullPath);
				}
				finally
				{
					Log.WriteLine(@"GetFolderMessageCount", "Disconnect");
					_serverStorage.Disconnect();
				}
			}
			return fld.MessageCount;
		}

		public int GetFolderUnreadMessageCount(Folder fld)
		{
			if (fld.SyncType == FolderSyncType.DirectMode)
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				if ((_serverStorage as Pop3Storage) != null) return 0; // we can't get that info from POP3
				try
				{
					Log.WriteLine(@"GetFolderUnreadMessageCount", "Connect");
					_serverStorage.Connect();
					return _serverStorage.GetFolderUnreadMessageCount(fld.FullPath);
				}
				finally
				{
					Log.WriteLine(@"GetFolderUnreadMessageCount", "Disconnect");
					_serverStorage.Disconnect();
				}
			}
			return fld.UnreadMessageCount;
		}

		public void MoveMessages(object[] messageIndexSet, Folder fromFolder, Folder toFolder)
		{
			_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
			ImapStorage imapStorage = _serverStorage as ImapStorage;
			object[] uids = null;
			if (imapStorage != null)
			{
				WebMailMessageCollection msgs = null;
				if (fromFolder.SyncType != FolderSyncType.DirectMode)
				{
					msgs = _dbStorage.LoadMessages(messageIndexSet, fromFolder);
					uids = msgs.ToUidsCollection(false);
				}
				else
				{
					uids = messageIndexSet;
				}
				try
				{
					imapStorage.Connect();
					//------------------------------
					FolderCollection fc = new FolderCollection();
					fc.Add(toFolder);
					if ((fromFolder.SyncType == FolderSyncType.DontSync) && (toFolder.SyncType == FolderSyncType.DontSync))
					{
						//1: DontSync	|	DB Update  (id_folder_db field), file move	|	DontSync
                        // executing below
					}
					else if ((fromFolder.SyncType == FolderSyncType.DontSync) && (toFolder.SyncType != FolderSyncType.DontSync) && (toFolder.SyncType != FolderSyncType.DirectMode))
					{
						//2: DontSync	|	SaveMessage(Append(ToFolder)) Db+File Delete(FromFolder) Sync(ToFolder)	|	Sync
						imapStorage.SaveMessages(msgs, toFolder);
						DeleteMessages(messageIndexSet, fromFolder);
						PurgeMessages(messageIndexSet, fromFolder);
						imapStorage.Synchronize(fc);
						return;
					}
					else if ((fromFolder.SyncType == FolderSyncType.DontSync) && (toFolder.SyncType == FolderSyncType.DirectMode))
					{
						//3: DontSync	|	SaveMessage(Append(ToFolder)) Db+File Delete(FromFolder)	|	DirectMode
						imapStorage.SaveMessages(msgs, toFolder);
						DeleteMessages(messageIndexSet, fromFolder);
						PurgeMessages(messageIndexSet, fromFolder);
						return;
					}
					else if ((fromFolder.SyncType != FolderSyncType.DontSync) && (fromFolder.SyncType != FolderSyncType.DirectMode) && (toFolder.SyncType == FolderSyncType.DontSync))
					{
						//4: Sync	|	GetMessage(ToFolder) Db+File+Imap Delete(FromFolder) Purge(FromFolder)	|	DontSync
						msgs = imapStorage.LoadMessages(uids, fromFolder);
						SaveMessages(msgs, toFolder);
						DeleteMessages(messageIndexSet, fromFolder);
						PurgeMessages(messageIndexSet, fromFolder);
						return;
					}
					else if ((fromFolder.SyncType != FolderSyncType.DontSync) && (fromFolder.SyncType != FolderSyncType.DirectMode) && (toFolder.SyncType != FolderSyncType.DontSync) && (toFolder.SyncType != FolderSyncType.DirectMode))
					{
						//5: Sync	|	ImapMove Db+File+Imap Delete(FromFolder) Purge(FromFolder) Sync(ToFolder)	|	Sync
						imapStorage.MoveMessages(uids, fromFolder, toFolder);
//						DeleteMessages(uids, fromFolder);
//						PurgeMessages(uids, fromFolder);
						fc.Add(fromFolder);
						imapStorage.Synchronize(fc);
						return;
					}
					else if ((fromFolder.SyncType != FolderSyncType.DontSync) && (fromFolder.SyncType != FolderSyncType.DirectMode) && (toFolder.SyncType == FolderSyncType.DirectMode))
					{
						//6: Sync	|	ImapMove Db+File+Imap Delete(FromFolder) Purge(FromFolder)	|	DirectMode
						imapStorage.MoveMessages(uids, fromFolder, toFolder);
//						DeleteMessages(uids, fromFolder);
//						PurgeMessages(uids, fromFolder);
						fc.Add(fromFolder);
						imapStorage.Synchronize(fc);
						return;
					}
					else if ((fromFolder.SyncType == FolderSyncType.DirectMode) && (toFolder.SyncType == FolderSyncType.DontSync))
					{
						//7: DirectMode	|	GetMessage(ToFolder) Imap Delete(FromFolder) Purge(FromFolder)	|	DontSync
						WebMailMessageCollection coll = imapStorage.LoadMessages(messageIndexSet, fromFolder);
						SaveMessages(coll, toFolder);
						DeleteMessages(messageIndexSet, fromFolder);
						PurgeMessages(messageIndexSet, fromFolder);
						return;
					}
					else if ((fromFolder.SyncType == FolderSyncType.DirectMode) && (toFolder.SyncType != FolderSyncType.DontSync) && (toFolder.SyncType != FolderSyncType.DirectMode))
					{
						//8: DirectMode	|	ImapMove Imap Delete(FromFolder) Purge(FromFolder) Sync(ToFolder)	|	Sync
						imapStorage.MoveMessages(uids, fromFolder, toFolder);
						DeleteMessages(messageIndexSet, fromFolder);
						PurgeMessages(messageIndexSet, fromFolder);
                        imapStorage.Synchronize(fc);
						return;
					}
					else if ((fromFolder.SyncType == FolderSyncType.DirectMode) && (toFolder.SyncType == FolderSyncType.DirectMode))
					{
						//9: DirectMode	|	ImapMove	|	DirectMode
						imapStorage.MoveMessages(uids, fromFolder, toFolder);
						return;
					}
					//------------------------------
				}
				finally
				{
					imapStorage.Disconnect();
				}
			}
            if (fromFolder.SyncType != FolderSyncType.DirectMode)
            {
                _dbStorage.MoveMessages(messageIndexSet, fromFolder, toFolder);
                FileSystem fs = new FileSystem(_dbStorage.Account.Email, _dbStorage.Account.ID, true);
				string delimiter = _dbStorage.Account.Delimiter;
				string fromFolderName = fromFolder.GetFullPath(delimiter);
				string toFolderName = toFolder.GetFullPath(delimiter);
				fs.MoveMessages(DbStorage.ObjectsToInts32(messageIndexSet), fromFolderName, toFolderName);
            }
            else
            {
                if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Pop3)
                {
                    if (toFolder.SyncType == FolderSyncType.DontSync)
                    {
                        Pop3Storage pop3Storage = _serverStorage as Pop3Storage;

                        pop3Storage.Connect();
                        WebMailMessageCollection coll = pop3Storage.LoadMessages(messageIndexSet, fromFolder);
                        SaveMessages(coll, toFolder);
                        DeleteMessages(messageIndexSet, fromFolder);
                    }
                }
                else if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                {
                    if (toFolder.SyncType == FolderSyncType.DontSync)
                    {
                        WMServerStorage wmStorage = _serverStorage as WMServerStorage;

                        wmStorage.Connect();
                        WebMailMessageCollection coll = wmStorage.LoadMessages(messageIndexSet, fromFolder);
                        SaveMessages(coll, toFolder);
                        DeleteMessages(messageIndexSet, fromFolder);
                    }
                }
            }
		}

		public void SetFlags(object[] messageIndexSet, SystemMessageFlags flags, MessageFlagAction flagsAction, Folder fld)
		{
			_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
			ImapStorage imapStorage = _serverStorage as ImapStorage;
			object[] uids = null;
			if (imapStorage != null)
			{
				if (fld.SyncType != FolderSyncType.DirectMode)
				{
					WebMailMessageCollection dbMsgs = _dbStorage.LoadMessages(messageIndexSet, fld);
					uids = dbMsgs.ToUidsCollection(false);
				}
				else
				{
					uids = messageIndexSet;
				}
				try
				{
					imapStorage.Connect();
					imapStorage.SetMessagesFlags(uids, flags, flagsAction, fld);
				}
				finally
				{
					imapStorage.Disconnect();
				}
			}
			if (fld.SyncType != FolderSyncType.DirectMode)
			{
				_dbStorage.SetMessagesFlags(messageIndexSet, flags, flagsAction, fld);
			}
		}

		public void SetFlags(SystemMessageFlags flags, MessageFlagAction flagsAction, Folder fld)
		{
			if (fld.SyncType == FolderSyncType.DirectMode)
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				ImapStorage imapStorage = _serverStorage as ImapStorage;
				if (imapStorage != null)
				{
					try
					{
						imapStorage.Connect();
						imapStorage.SetMessagesFlags(flags, flagsAction, fld);
					}
					finally
					{
						imapStorage.Disconnect();
					}
				}
			}
			else
			{
				_dbStorage.SetMessagesFlags(flags, flagsAction, fld);
			}
		}

		public void DeleteMessages(object[] messageIndexSet, Folder fld)
		{
			if (fld.SyncType == FolderSyncType.DirectMode)
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				try
				{
					_serverStorage.Connect();
					_serverStorage.DeleteMessages(messageIndexSet, fld);
					return;
				}
				finally
				{
					_serverStorage.Disconnect();
				}
			}

			if (_dbStorage.Account != null)
			{
				if (_dbStorage.Account.MailIncomingProtocol != IncomingMailProtocol.Imap4)
				{
					if (fld.Type == FolderType.Trash)
					{
						PurgeMessages(messageIndexSet, fld);
					}
					else
					{
						Folder trash = GetFolder(FolderType.Trash);
						if (trash != null)
						{
							MoveMessages(messageIndexSet, fld, trash);
						}
					}
				}
				else if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Imap4)
				{
					SetFlags(messageIndexSet, SystemMessageFlags.Deleted, MessageFlagAction.Add, fld);
				}
			}
		}

		public void PurgeMessages(object[] messageIndexSet, Folder fld)
		{
			if (_dbStorage.Account != null)
			{
				object[] serverUids = null;
				bool needToDeleteFromServer = false;
				WebMailMessageCollection msgs = null;
				if (fld.SyncType != FolderSyncType.DirectMode)
				{
					if (_dbStorage.Account.MailIncomingProtocol != IncomingMailProtocol.Imap4)
					{
						msgs = _dbStorage.LoadMessagesToDelete(false, messageIndexSet, fld);
						if ((_dbStorage.Account.MailMode == MailMode.DeleteMessageWhenItsRemovedFromTrash)&&(fld.Type == FolderType.Trash))
						{
							if (msgs != null) serverUids = msgs.ToUidsCollection(true);
							needToDeleteFromServer = true;
						}
					}
					else if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Imap4)
					{
						msgs = _dbStorage.LoadMessagesToDelete(true, messageIndexSet, fld);
						if (msgs != null) serverUids = msgs.ToUidsCollection(false);
						needToDeleteFromServer = true;
					}
				}
				else
				{
					serverUids = messageIndexSet;
					needToDeleteFromServer = true;
				}
				if (needToDeleteFromServer)
				{
					if ((serverUids != null) && (serverUids.Length > 0))
					{
						_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
						try
						{
							_serverStorage.MessageDeleted += new DeleteMessageHanlder(_serverStorage_MessageDeleted);
							_serverStorage.Connect();
							_serverStorage.DeleteMessages(serverUids, fld);
						}
						finally
						{
							_serverStorage.MessageDeleted -= new DeleteMessageHanlder(_serverStorage_MessageDeleted);
							_serverStorage.Disconnect();
						}
					}
				}
				if ((msgs != null) && (msgs.Count > 0))
				{
					_dbStorage.DeleteMessages(msgs.ToIDsCollection(), fld);
				}
			}
		}

		public WebMailMessageCollection SearchMessages(int page, string condition, FolderCollection folders, bool inHeadersOnly, out int searchMessagesCount)
		{
			return _dbStorage.SearchMessages(page, condition, folders, inHeadersOnly, out searchMessagesCount);
		}

		public WebMailMessageCollection SearchMessages(string condition, FolderCollection folders, bool inHeadersOnly)
		{
			return _dbStorage.SearchMessages(condition, folders, inHeadersOnly);
		}

		public void SaveMessage(WebMailMessage message, Folder folder)
		{
            if (folder.SyncType == FolderSyncType.DirectMode)
            {
                _serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
                try
                {
                    _serverStorage.Connect();
                    _serverStorage.SaveMessage(message, folder);
                }
                finally
                {
                    _serverStorage.Disconnect();
                }
            }
            else
            {
                _dbStorage.SaveMessage(message, folder);
            }
		}

        public int SaveMessageAndGetId(WebMailMessage message, Folder folder)
        {
            SaveMessage(message, folder);
            int result = -1;
            if (folder.SyncType != FolderSyncType.DirectMode)
                result = _dbStorage.GetLastMsgID();

            return result;
        }

		public void SaveMessages(WebMailMessageCollection messages, Folder fld)
		{
			_dbStorage.SaveMessages(messages, fld);
		}

		public void UpdateMessage(WebMailMessage webMsg)
		{
			_dbStorage.UpdateMessage(webMsg);
		}

		public long CalculateAccountSize(int accountId)
		{
			return _dbStorage.GetMailStorageSize();
		}

		private void _serverStorage_MessageDownloaded(object sender, CheckMailEventArgs e)
		{
			OnMessageDownloaded(e);
		}

		private void _serverStorage_MessageDeleted(object sender, DeleteMessageEventArgs e)
		{
			OnMessageDeleted(e);
		}
	}// END CLASS DEFINITION MailProcessor
}
