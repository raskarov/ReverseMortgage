using System;
using System.Collections;
using System.Globalization;
using MailBee.ImapMail;
using MailBee.Mime;

namespace WebMailPro
{
	public class CheckMailEventArgs : EventArgs
	{
		protected string _folderName = "";
		protected int _msgsCount = 0;
		protected int _msgNumber = 0;

		public string FolderName
		{
			get { return _folderName; }
		}

		public int MsgsCount
		{
			get { return _msgsCount; }
		}

		public int MsgsNumber
		{
			get { return _msgNumber; }
		}

		public CheckMailEventArgs(string folderName, int msgsCount, int msgsNumber)
		{
			_folderName = folderName;
			_msgsCount = msgsCount;
			_msgNumber = msgsNumber;
		}
	}

	public class DeleteMessageEventArgs : EventArgs
	{
		protected int _msgIndex = 0;
		protected string _folderName = string.Empty;

		public int MessageIndex
		{
			get { return _msgIndex; }
		}

		public string FolderName
		{
			get { return _folderName; }
		}

		public DeleteMessageEventArgs(int index, string folderName)
		{
			_msgIndex = index;
			_folderName = folderName;
		}
	}

	public delegate void DownloadedMessageHandler(object sender, CheckMailEventArgs e);

	public delegate void DeleteMessageHanlder(object sender, DeleteMessageEventArgs e);

	// Mail Storage: POP3, IMAP, any DB
	public abstract class MailStorage
	{
		public event DownloadedMessageHandler MessageDownloaded;
		public event DeleteMessageHanlder MessageDeleted;

		protected string _folderName = "";
		protected int _msgsCount = 0;
		protected int _msgNumber = 0;

		protected Account _account = null;

		public Account Account
		{
			get { return _account; }
			set { _account = value; }
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

		public MailStorage(Account account) { _account = account; }

		public virtual void Connect() {}

		public virtual bool IsConnected() { return false; }

		public virtual void Disconnect() {}

		public virtual FolderCollection GetFolders() { return null; }

		public virtual long GetMailStorageSize() { return 0; }

		public virtual Folder GetFolder(string folderFullName) { return null; }

		public virtual Folder CreateFolder(Folder fld) { return null; }

		public virtual void DeleteFolder(Folder fld) {}

		public virtual void UpdateFolder(Folder fld) {}

		public virtual WebMailMessageCollection LoadMessageHeaders(Folder fld) { return null; }

		public virtual WebMailMessageCollection LoadMessageHeaders(object[] messageIndexSet, Folder fld) { return null; }

		public virtual WebMailMessageCollection LoadMessageHeaders(int pageNumber, Folder fld) { return null; }

		public virtual WebMailMessageCollection LoadMessages(Folder fld) { return null; }

		public virtual WebMailMessageCollection LoadMessages(object[] messageIndexSet, Folder fld) { return null; }

		public virtual WebMailMessage LoadMessage(object index, Folder fld)
		{
			WebMailMessageCollection coll = LoadMessages(new object[] { index }, fld);
			if ((coll != null) && (coll.Count > 0))
			{
				return coll[0];
			}
			return null;
		}

		public virtual void MoveMessages(object[] messageIndexSet, Folder fromFolder, Folder toFolder) {}

		public virtual void SaveMessage(WebMailMessage message, Folder fld) {}

		public virtual void DeleteMessages(object[] messageIndexSet, Folder fld) {}

		public virtual void SetMessagesFlags(object[] messageIndexSet, SystemMessageFlags flags, MessageFlagAction flagsAction, Folder fld) {}

		public virtual void SetMessagesFlags(SystemMessageFlags flags, MessageFlagAction flagsAction, Folder fld) {}

		public virtual void SaveMessages(WebMailMessageCollection messages, Folder fld) {}

	}// END CLASS DEFINITION MailStorage

	public abstract class MailServerStorage	: MailStorage
	{
		public MailServerStorage(Account account) : base(account) {}

		public virtual void Synchronize(FolderCollection foldersTree) {}

		protected void ApplyFilters(WebMailMessageCollection messageCollection, DbStorage dbStorage, Folder fld, ref ArrayList arr)
		{
			Filter[] filters = dbStorage.GetFilters();
			int id_msg = dbStorage.GetLastMsgID();

			foreach (WebMailMessage webMsg in messageCollection)
			{
				id_msg = Utils.RandMsgID(++id_msg);
				bool needToSave = true;
				foreach (Filter flt in filters)
				{
					FilterAction action = flt.GetActionToApply(webMsg);
					if (action == FilterAction.DeleteFromServerImmediately)
					{
						object[] messageIndexSet = new object[] { -1 };
						if (_account.MailIncomingProtocol != IncomingMailProtocol.Imap4)
						{
							messageIndexSet[0] = webMsg.StrUid;
						}
						else if (_account.MailIncomingProtocol == IncomingMailProtocol.Imap4)
						{
							messageIndexSet[0] = webMsg.IntUid;
						}
						this.DeleteMessages(messageIndexSet, fld);
						needToSave = false;
						break;
					}
					else if (action == FilterAction.MoveToFolder)
					{
						if (needToSave)
						{
							Folder toFld = dbStorage.GetFolder(flt.IDFolder);
							if (toFld != null)
							{
								dbStorage.SaveMessage(id_msg, webMsg, toFld);
							}
							needToSave = false;
						}
						break;
					}
					else if (action == FilterAction.MarkGrey)
					{
						webMsg.Grayed = true;
						break;
					}
				}
				if (needToSave) dbStorage.SaveMessage(id_msg, webMsg, fld);
				arr.Add((webMsg.IntUid > 0) ? (object)webMsg.IntUid : webMsg.StrUid);
			}
		}

		protected void ApplyXSpam(WebMailMessageCollection messageCollection)
		{
			foreach (WebMailMessage webMsg in messageCollection)
			{
				MailMessage msg = webMsg.MailBeeMessage;
				if (msg != null)
				{
					HeaderCollection xspamHeaders = msg.Headers.Items("x-spam");
					if ((xspamHeaders != null) && (xspamHeaders.Count > 0))
					{
						Header xspam = xspamHeaders[0];
						if ((string.Compare(xspam.Value, "probable spam", false, CultureInfo.InvariantCulture) == 0)
							|| (string.Compare(xspam.Value, "suspicious", false, CultureInfo.InvariantCulture) == 0))
						{
							webMsg.XSpam = true;
						}
						else
						{
							webMsg.XSpam = false;
						}
					}
				}
			}
		}

		public abstract int GetFolderMessageCount(string fullPath);

		public abstract int GetFolderUnreadMessageCount(string fullPath);
	}// END CLASS DEFINITION MailServerStorage

	public class MailServerStorageCreator
	{
		public static MailServerStorage CreateMailServerStorage(Account acct)
		{
			switch(acct.MailIncomingProtocol)
			{
				case IncomingMailProtocol.Imap4:
					return new ImapStorage(acct);
				case IncomingMailProtocol.Pop3:
					return new Pop3Storage(acct);
				case IncomingMailProtocol.WMServer:
					return new WMServerStorage(acct);
				default:
					return null;
			}
		}
	}
}
