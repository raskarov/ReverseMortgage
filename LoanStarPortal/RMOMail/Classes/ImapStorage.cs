using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using MailBee;
using MailBee.ImapMail;
using MailBee.Mime;
using MailBee.Security;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for ImapStorage.
	/// </summary>
	public class ImapStorage : MailServerStorage
	{
		protected Imap _imapObj;

		public ImapStorage(Account account) : base(account)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

			try
			{
				Imap.LicenseKey = settings.LicenseKey;
				_imapObj = new Imap();
				_imapObj.EnvelopeDownloaded += new ImapEnvelopeDownloadedEventHandler(_imapObj_EnvelopeDownloaded);

				if (settings.EnableLogging)
				{
					_imapObj.Log.Enabled = true;
					string dataFolderPath = Utils.GetDataFolderPath();
					if (dataFolderPath != null)
					{
						_imapObj.Log.Filename = Path.Combine(dataFolderPath, Constants.logFilename);
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
			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			try
			{
                if (_account.MailIncomingPort == 993)
                {
                    _imapObj.SslMode = SslStartupMode.OnConnect;
                    _imapObj.SslProtocol = SecurityProtocol.Auto;
                    _imapObj.SslCertificates.AutoValidation = CertificateValidationFlags.None;
                }
                _imapObj.Connect(_account.MailIncomingHost, _account.MailIncomingPort);
				_imapObj.Login(_account.MailIncomingLogin, _account.MailIncomingPassword, AuthenticationMethods.Auto, AuthenticationOptions.PreferSimpleMethods, null);
			}
			catch (MailBeeConnectionException)
			{
				throw new WebMailException(resMan.GetString("ErrorIMAP4Connect"));
			}
			catch (MailBeeImapLoginBadCredentialsException)
			{
				throw new WebMailException(resMan.GetString("ErrorPOP3IMAP4Auth"));
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
		}

		public override bool IsConnected()
		{
			return _imapObj.IsConnected;
		}


		public override void Disconnect()
		{
			try
			{
				if (IsConnected()) _imapObj.Disconnect();
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
		}

		public override long GetMailStorageSize()
		{
			try
			{
				if (_imapObj.GetExtension("QUOTA") != null)
				{
					FolderQuota fq = _imapObj.GetAccountQuota();
					if (fq.MaxStorageSize > -1)
					{
						return fq.MaxStorageSize;
					}
				}
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
			return 0;
		}

		public override int GetFolderMessageCount(string fullPath)
		{
			FolderStatus fs = GetFolderStatus(fullPath);
            if (fs != null) return fs.MessageCount;
            return 0;
		}

		public override int GetFolderUnreadMessageCount(string fullPath)
		{
			FolderStatus fs = GetFolderStatus(fullPath);
			if (fs != null && fs.UnseenCount >= 0) return fs.UnseenCount;
            return 0;
		}

		public override Folder CreateFolder(Folder fld)
		{
			try
			{
				_imapObj.CreateFolder(fld.FullPath);
				if (fld.Hide)
				{
					_imapObj.UnsubscribeFolder(fld.FullPath);
				}
				else
				{
					_imapObj.SubscribeFolder(fld.FullPath);
				}
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
			return fld;
		}

		public override void DeleteFolder(Folder fld)
		{
			try
			{
				if (this.GetImapFolder(fld.FullPath) != null)
				{
					_imapObj.DeleteFolder(fld.FullPath);
				}
			}
			catch (MailBeeException ex)
			{
				// if non empty folder
				throw new WebMailMailBeeException(ex);
			}
		}

		public override void UpdateFolder(Folder fld)
		{
			try
			{
				if (string.Compare(fld.Name, fld.UpdateName, true, CultureInfo.InvariantCulture) != 0)
				{
					if ((fld.UpdateFullPath != null) && (fld.UpdateFullPath.Length > 0))
					{
						_imapObj.RenameFolder(fld.FullPath, fld.UpdateFullPath);
					}
				}
				if (fld.Hide)
				{
					_imapObj.UnsubscribeFolder(fld.FullPath);
				}
				else
				{
					_imapObj.SubscribeFolder(fld.FullPath);
				}
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
		}

        private ArrayList ReSetType(FolderCollection fc, bool hasInbox, bool hasSent, bool hasDrafts)
        {
            for (int i = 0; i < fc.Count; i++)
            {
                if (fc[i].Type == FolderType.Inbox)
                {
					if (hasInbox) fc[i].Type = FolderType.Custom;
					else fc[i].Hide = false;
                    hasInbox = true;
                }
                else if (fc[i].Type == FolderType.SentItems)
                {
                    if (hasSent) fc[i].Type = FolderType.Custom;
					else fc[i].Hide = false;
					hasSent = true;
                }
                else if (fc[i].Type == FolderType.Drafts)
                {
                    if (hasDrafts) fc[i].Type = FolderType.Custom;
					else fc[i].Hide = false;
					hasDrafts = true;
                }
                else
                {
                    fc[i].Type = FolderType.Custom;
                }

                if (fc[i].SubFolders.Count > 0)
                {
                    if (fc[i].Type == FolderType.Inbox)
                    {
                        ArrayList vars = ReSetType(fc[i].SubFolders, hasInbox, hasSent, hasDrafts);
                        if (vars.Count > 2)
                        {
                            hasInbox = (bool) vars[0];
                            hasSent = (bool) vars[1];
                            hasDrafts = (bool) vars[2];
                        }
                    }
                    else
                    {
                        ReSetType(fc[i].SubFolders, true, true, true);
                    }
                }
            }

            ArrayList ArrayResp = new ArrayList();
            ArrayResp.Add(hasInbox);
            ArrayResp.Add(hasSent);
            ArrayResp.Add(hasDrafts);

            return ArrayResp;
        }

        public override FolderCollection GetFolders()
		{
			try
			{
				MailBee.ImapMail.FolderCollection subscribedFolderCollection = _imapObj.DownloadFolders(true);
				MailBee.ImapMail.FolderCollection mailBeeFolderCollection = _imapObj.DownloadFolders();

				FolderCollection tmp_col = FolderCollection.CreateImapFolderTreeFromList(mailBeeFolderCollection, subscribedFolderCollection);
                ReSetType(tmp_col, false, false, false);
                return tmp_col;
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
		}

		public override Folder GetFolder(string folderFullName)
		{
			try
			{
				FolderCollection fc = GetFolders();
				foreach (Folder fld in fc)
				{
					if (fld.FullPath == folderFullName) return fld;
				}
				return null;
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
		}

		public override WebMailMessage LoadMessage(object index, Folder fld)
		{
			long lngUid = 0;
			try
			{
				lngUid = Convert.ToInt64(index);
			}
			catch (InvalidCastException)
			{
				throw new WebMailException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("InvalidUid"));
			}
			try
			{
				if (fld != null) 
				{
					if (_imapObj.SelectFolder(fld.FullPath))
					{
						WebMailMessage result = new WebMailMessage(_account);
						result.Init(_imapObj.DownloadEntireMessage(lngUid, true), false, fld);
						return result;
					}
				}
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
			return null;
		}

		public override WebMailMessageCollection LoadMessageHeaders(int pageNumber, Folder fld)
		{
			WebMailMessageCollection coll = null;
			if (fld != null)
			{
                try
                {
                    if (_imapObj.SelectFolder(fld.FullPath))
                    {
                        if (_imapObj.MessageCount > 0)
                        {
                            coll = LoadMessageHeaders(GetIndexSetFromPageNumber(pageNumber), false, fld);
                        }
                        else
                        {
                            coll = new WebMailMessageCollection();
                        }
                    }
                }
                catch (MailBeeException)
                {
                }
                finally
                {
                    if (fld.SyncType == FolderSyncType.DirectMode)
				    {
					    CollectionBase cb = coll as CollectionBase;
					    if (cb != null)
					    {
						    Utils.ReverseCollection(ref cb);
					    }
				    }
                }
				
			}
			return coll;
		}


		public override WebMailMessageCollection LoadMessageHeaders(Folder fld)
		{
			return LoadMessageHeaders(Imap.AllMessages, false, fld);
		}

		public override WebMailMessageCollection LoadMessageHeaders(object[] messageIndexSet, Folder fld)
		{
			return LoadMessageHeaders(messageIndexSet, fld);
		}

		public override WebMailMessageCollection LoadMessages(Folder fld)
		{
			return LoadMessages(Imap.AllMessages, fld);
		}

		public override WebMailMessageCollection LoadMessages(object[] messageIndexSet, Folder fld)
		{
			if (messageIndexSet.Length > 0)
			{
				string messageIndexSetString = CreateMessageIndexSet(messageIndexSet);

				return LoadMessages(messageIndexSetString, fld);
			}
			return null;
		}

		public override void MoveMessages(object[] messageIndexSet, Folder fromFolder, Folder toFolder)
		{
			if (messageIndexSet.Length > 0)
			{
				try
				{
					if (this.GetImapFolder(toFolder.FullPath) != null)
					{
						if (this.CanSelectFolder(fromFolder.FullPath))
						{
							if (_imapObj.SelectFolder(fromFolder.FullPath))
							{
								_imapObj.MoveMessages(CreateMessageIndexSet(messageIndexSet), true, toFolder.FullPath);
							}
						}
					}
				}
				catch (MailBeeException ex)
				{
					throw new WebMailMailBeeException(ex);
				}
			}
		}

		public override void DeleteMessages(object[] messageIndexSet, Folder fld)
		{
			if (messageIndexSet.Length > 0)
			{
				string messageIndexSetString = CreateMessageIndexSet(messageIndexSet);
				try
				{
					if (CanSelectFolder(fld.FullPath))
					{
						if (_imapObj.SelectFolder(fld.FullPath))
						{
							OnMessageDeleted(new DeleteMessageEventArgs(-1, fld.FullPath));
							_imapObj.DeleteMessages(messageIndexSetString, true);
							_imapObj.Close(true);
						}
					}
				}
				catch (MailBeeException ex)
				{
					throw new WebMailMailBeeException(ex);
				}
			}
		}

		public override void SaveMessage(WebMailMessage message, Folder fld)
		{
			if (message.MailBeeMessage != null)
			{
				try
				{
					SystemMessageFlags flags = SystemMessageFlags.None;
					if (message.Seen) flags = SystemMessageFlags.Seen;
					_imapObj.UploadMessage(message.MailBeeMessage, fld.FullPath, flags);
				}
				catch (MailBeeException ex)
				{
					throw new WebMailMailBeeException(ex);
				}
			}
		}

		public override void SaveMessages(WebMailMessageCollection messages, Folder fld)
		{
			foreach (WebMailMessage message in messages)
			{
				SaveMessage(message, fld);
			}
		}

		public override void SetMessagesFlags(object[] messageIndexSet, SystemMessageFlags flags, MessageFlagAction flagsAction, Folder fld)
		{
			if (messageIndexSet.Length > 0)
			{
				string messageIndexSetString = ImapStorage.CreateMessageIndexSet(messageIndexSet);

				try
				{
					if (this.CanSelectFolder(fld.FullPath))
					{
						if (_imapObj.SelectFolder(fld.FullPath))
						{
							_imapObj.SetMessageFlags(messageIndexSetString, true, flags, flagsAction);
						}
					}
				}
				catch (MailBeeException ex)
				{
					throw new WebMailMailBeeException(ex);
				}
			}
		}

		public override void SetMessagesFlags(SystemMessageFlags flags, MessageFlagAction flagsAction, Folder fld)
		{
			try
			{
				if (this.CanSelectFolder(fld.FullPath))
				{
					if (_imapObj.SelectFolder(fld.FullPath))
					{
						_imapObj.SetMessageFlags(Imap.AllMessages, true, flags, flagsAction);
					}
				}
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
		}


		public override void Synchronize(FolderCollection foldersTree)
		{
			DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(_account);

			FolderCollection folders = new FolderCollection();
			FolderCollection.CreateFolderListFromTree(ref folders, foldersTree);

			try
			{
				dbStorage.Connect();
				foreach (Folder fld in folders)
				{
					if ((fld.SyncType == FolderSyncType.DontSync) || (fld.SyncType == FolderSyncType.DirectMode) ||
						fld.Hide == true)
					{
						continue;
					}

					if (!this.CanSelectFolder(fld.FullPath))
					{
						continue;
					}

					long lastUidDb = dbStorage.GetImapLastMessageUid(fld);
					long lastUidSrv = 0;
					if (lastUidDb >= 0)
					{
						if (_imapObj.SelectFolder(fld.FullPath))
						{
							_folderName = fld.FullPath;
							// receive uid of last message at server
							
							int msgCount = _imapObj.MessageCount;
							if (msgCount <= 0) continue;

							long startUid = 0;
							EnvelopeCollection ec = _imapObj.DownloadEnvelopes(msgCount.ToString(), false, EnvelopeParts.Uid, 0);
							EnvelopeCollection ec_new = new EnvelopeCollection();
							if (msgCount == 1)
							{
								ec_new = ec;
							}
							else
							{
								ec_new = _imapObj.DownloadEnvelopes("1", false, EnvelopeParts.Uid, 0);
							}

							if (ec.Count > 0) lastUidSrv = ec[0].Uid;
							if (ec_new.Count > 0) startUid = ec_new[0].Uid;
							if ((lastUidDb + 1) <= lastUidSrv)
							{
								if (lastUidDb > 0)
								{
									startUid = lastUidDb + 1;
								}

								long endUid = 0;
								bool needToDownload = true;
								bool messageDownloaded = true;
								int maxEnvelopesPerSession = Constants.DownloadChunk;
								do
								{
									try
									{
										if ((startUid + maxEnvelopesPerSession) < lastUidSrv)
										{
											endUid = startUid + maxEnvelopesPerSession;
										}
										else
										{
											endUid = -1;
											needToDownload = false;
										}
										_msgsCount = _imapObj.MessageCount;
										// receive new messages
										if ((fld.SyncType == FolderSyncType.AllEntireMessages) || (fld.SyncType == FolderSyncType.NewEntireMessages))
										{
                                            ec = this.DownloadEnvelopes(startUid, endUid, EnvelopeParts.MessagePreview | EnvelopeParts.Flags | EnvelopeParts.Uid | EnvelopeParts.Rfc822Size, -1);
										}
										if ((fld.SyncType == FolderSyncType.AllHeadersOnly) || (fld.SyncType == FolderSyncType.NewHeadersOnly))
										{
                                            ec = this.DownloadEnvelopes(startUid, endUid, EnvelopeParts.MessagePreview | EnvelopeParts.Flags | EnvelopeParts.Uid | EnvelopeParts.Rfc822Size, 0);
											messageDownloaded = false;
										}
										startUid = endUid + 1;
									}
									finally
									{
										WebMailMessageCollection msgs = new WebMailMessageCollection();
										foreach (Envelope env in ec)
										{
											WebMailMessage msg = new WebMailMessage(_account);
											msg.Init(env.MessagePreview, false, fld);
											msg.InitImapFlags(env.Flags);
											msg.Downloaded = messageDownloaded;
                                            msg.Size = (long) env.Size;
											msgs.Add(msg);
										}
										if (msgs.Count > 0)
										{
											ApplyXSpam(msgs);
										}
										if (msgs.Count > 0)
										{
											ArrayList arr = new ArrayList();
											ApplyFilters(msgs, dbStorage, fld, ref arr);
										}
									}
								}
								while (needToDownload);
							}
							if ((fld.SyncType == FolderSyncType.NewEntireMessages) || (fld.SyncType == FolderSyncType.NewHeadersOnly))
							{
								continue;
							}

							// synchronize old messages
                            ec = _imapObj.DownloadEnvelopes(Imap.AllMessages, false, EnvelopeParts.Flags | EnvelopeParts.Uid, 0);
							WebMailMessageCollection dbMsgs = dbStorage.LoadMessages(lastUidDb, _account.ID, fld);

							for (int dbIndex = 0; dbIndex < dbMsgs.Count; dbIndex++)
							{
								for (int srvIndex = 0; srvIndex < ec.Count; srvIndex++)
								{
									if (dbMsgs[dbIndex].IntUid == ec[srvIndex].Uid)
									{
										dbMsgs[dbIndex].Seen = ((ec[srvIndex].Flags.SystemFlags & SystemMessageFlags.Seen) > 0) ? true : false;
										dbMsgs[dbIndex].Flagged = ((ec[srvIndex].Flags.SystemFlags & SystemMessageFlags.Flagged) > 0) ? true : false;
										dbMsgs[dbIndex].Deleted = ((ec[srvIndex].Flags.SystemFlags & SystemMessageFlags.Deleted) > 0) ? true : false;
										dbMsgs[dbIndex].Replied =((ec[srvIndex].Flags.SystemFlags & SystemMessageFlags.Answered) > 0) ? true : false;
										dbMsgs[dbIndex].Flags = ec[srvIndex].Flags.SystemFlags;
										dbStorage.UpdateMessage(dbMsgs[dbIndex]);
										break;
									}
									else if ((ec[srvIndex].Uid > dbMsgs[dbIndex].IntUid) || (srvIndex == ec.Count - 1))
									{
										if ((fld.SyncType == FolderSyncType.AllEntireMessages) || (fld.SyncType == FolderSyncType.AllHeadersOnly))
										{
											dbStorage.DeleteMessages(new object[] { dbMsgs[dbIndex].IDMsg }, fld);
										}
										continue;
									}
								}
							}
						}
					}
				}
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
			finally
			{
				dbStorage.Disconnect();
			}
		}

		public static string CreateMessageIndexSet(object[] messageIndexSet)
		{
			UidCollection uc = new UidCollection();
			try
			{
				foreach (object s in messageIndexSet)
				{
					uc.Add(Convert.ToInt64(s));
				}
			}
			catch (InvalidCastException)
			{
				throw new WebMailException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("InvalidUid"));
			}
			return uc.ToString();
		}

		private EnvelopeCollection DownloadEnvelopes(long startUid, long endUid, EnvelopeParts parts, int bodyPreviewSize)
		{
			_imapObj.EnableLastDownloaded = true;
			EnvelopeCollection ec = new EnvelopeCollection();
			bool finished = false;
			string endUidStr = (endUid > 0) ? endUid.ToString() : "*";

			do
			{
				try
				{
					_imapObj.DownloadEnvelopes(string.Format("{0}:{1}", startUid, endUidStr), true, parts, bodyPreviewSize);
					ec.Add(_imapObj.LastDownloadedEnvelopes);
					finished = true;
				}
				catch (MailBeeException ex)
				{
					if ((_imapObj.LastDownloadedEnvelopes == null) || (_imapObj.LastDownloadedEnvelopes.Count == 0))
					{
						throw new WebMailMailBeeException(ex);
					}
					else
					{
						ec.Add(_imapObj.LastDownloadedEnvelopes);
						startUid = _imapObj.LastDownloadedEnvelopes[_imapObj.LastDownloadedEnvelopes.Count - 1].Uid + 1;
					}
				}
			}
			while (!finished);

			_imapObj.EnableLastDownloaded = false;
			return ec;
		}

		private WebMailMessageCollection LoadMessages(string messageIndexSet, Folder fld)
		{
			WebMailMessageCollection returnColl = new WebMailMessageCollection();
			EnvelopeCollection envColl = null;
			try
			{
				if (fld != null)
				{
					if (_imapObj.SelectFolder(fld.FullPath))
					{
						envColl = _imapObj.DownloadEnvelopes(messageIndexSet, true, EnvelopeParts.All, -1);
					}
				}
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
			if (envColl != null)
			{
				foreach (Envelope env in envColl)
				{
					WebMailMessage webMsg = new WebMailMessage(_account);
					webMsg.Init(env.MessagePreview, false, fld);
					webMsg.InitImapFlags(env.Flags);
					returnColl.Add(webMsg);
				}
			}
			return returnColl;
		}

		private WebMailMessageCollection LoadMessageHeaders(string messageIndexSet, bool indexAsUid, Folder fld)
		{
			WebMailMessageCollection returnColl = new WebMailMessageCollection();
			EnvelopeCollection envColl = null;
			try
			{
				if (fld != null)
				{
					if (_imapObj.SelectFolder(fld.FullPath))
					{
						envColl = _imapObj.DownloadEnvelopes(messageIndexSet, indexAsUid, EnvelopeParts.MessagePreview | EnvelopeParts.Flags | EnvelopeParts.Uid, 0);
					}
				}
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
			if (envColl != null)
			{
				foreach (Envelope env in envColl)
				{
					WebMailMessage webMsg = new WebMailMessage(_account);
					webMsg.Init(env.MessagePreview, false, fld);
					webMsg.InitImapFlags(env.Flags);
					returnColl.Add(webMsg);
				}
			}
			return returnColl;
		}

		private FolderStatus GetFolderStatus(string fullPath)
		{
            try
            {
                _imapObj.SelectFolder(fullPath);
                return _imapObj.GetFolderStatus(fullPath);
            }
            //catch (MailBeeException ex)
            catch (MailBeeException)
            {
            	//throw new WebMailMailBeeException(ex);
                return null;
            }
		}

		private string GetIndexSetFromPageNumber(int pageNumber)
		{
			string result = string.Empty;
			int msgPerPage = _account.UserOfAccount.Settings.MsgsPerPage;
			int msgCount = _imapObj.MessageCount;
			int startIndex = msgCount - (pageNumber * msgPerPage);
			if (startIndex < 0)
			{
				startIndex = 0;
			}
			startIndex++;
            int endIndex = msgCount - ((pageNumber - 1) * msgPerPage);
            if (endIndex > msgCount)
            {
                endIndex = msgCount;
            }
            result = string.Format("{0}:{1}", startIndex, endIndex);
			return result;
		}

		private MailBee.ImapMail.Folder GetImapFolder(string folderName)
		{
			MailBee.ImapMail.FolderCollection fc = _imapObj.DownloadFolders();
			foreach (MailBee.ImapMail.Folder f in fc)
			{
				if (string.Compare(f.Name, folderName, false, CultureInfo.InvariantCulture) == 0)
				{
					return f;
				}
			}
			return null;
		}

		private bool CanSelectFolder(string folderName)
		{
			MailBee.ImapMail.Folder fld = GetImapFolder(folderName);
			if (fld != null)
			{
				if ((fld.Flags & FolderFlags.Noselect) > 0)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		private void _imapObj_EnvelopeDownloaded(object sender, ImapEnvelopeDownloadedEventArgs e)
		{
			_msgNumber = e.MessageNumber;
			if ((e != null) && (e.DownloadedEnvelope != null))
			{
				if (e.DownloadedEnvelope.MessagePreview != null)
				{
					OnMessageDownloaded(new CheckMailEventArgs(_folderName, _msgsCount, _msgNumber));
				}
			}
		}
	}
}
