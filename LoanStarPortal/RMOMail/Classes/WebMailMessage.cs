using System;
using System.Text;
using MailBee;
using MailBee.ImapMail;
using MailBee.Mime;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for WebMailMessage.
	/// </summary>
    /// 
    [Serializable]
	public class WebMailMessage
	{
		#region Fields
		protected Account _account = null;
		protected MailMessage _message = null;
		protected long _id = -1;
		protected int _id_msg = -1;
		protected int _id_acct = -1;
		protected long _id_folder_srv = -1;
		protected long _id_folder_db = -1;
		protected string _str_uid = string.Empty;
		protected long _int_uid = -1;
		protected EmailAddress _from_msg = null;
		protected EmailAddressCollection _to_msg = null;
		protected EmailAddressCollection _cc_msg = null;
		protected EmailAddressCollection _bcc_msg = null;
		protected string _subject = string.Empty;
		protected DateTime _msg_date = Constants.MinDate;
		protected bool _attachments = false;
		protected long _size = 0;
		protected bool _seen = false;
		protected bool _flagged = false;
		protected MailPriority _priority = MailPriority.None;
		protected bool _downloaded = false;
		protected bool _x_spam = false;
		protected bool _rtl = false;
		protected bool _deleted = false;
		protected bool _is_full = false;
		protected bool _replied = false;
		protected bool _forwarded = false;
		protected SystemMessageFlags _flags = SystemMessageFlags.None;
		protected string _body_text = string.Empty;
		protected bool _grayed = false;
		protected int _charset = -1;
		protected byte[] _rawMsg = null;
		protected byte _safety = 0;

		protected string _folder_full_name = string.Empty;

        protected string _LoanApplicants = string.Empty;
        protected string _LoanAppIDs = string.Empty;
	    private int conditionId = -1;
	    private int associationType = 0;


		#endregion

		#region Properties
	    public int AssociationType
	    {
            get { return associationType; }
            set { associationType = value; }
	    }
        public int ConditionId
	    {
            get { return conditionId; }
            set { conditionId = value; }
	    }

	    public string LoanAppIDs
        {
            get { return _LoanAppIDs; }
            set { _LoanAppIDs = value; }
        }

        public string LoanApplicants
        {
            get { return _LoanApplicants; }
            set { _LoanApplicants = value; }
        }

		public long ID
		{
			get { return _id; }
			set { _id = value; }
		}

		public int IDMsg
		{
			get { return _id_msg; }
			set { _id_msg = value; }
		}

		public MailMessage MailBeeMessage
		{
			get { return _message; }
			set { _message = value; }
		}

		public int IDAcct
		{
			get { return _id_acct; }
			set { _id_acct = value; }
		}

		public long IDFolderSrv
		{
			get { return _id_folder_srv; }
			set { _id_folder_srv = value; }
		}

		public long IDFolderDB
		{
			get { return _id_folder_db; }
			set { _id_folder_db = value; }
		}

		public string StrUid
		{
			get { return _str_uid; }
			set { _str_uid = value; }
		}

		public long IntUid
		{
			get { return _int_uid; }
			set { _int_uid = value; }
		}

		public EmailAddress FromMsg
		{
			get { return _from_msg; }
			set { _from_msg = value; }
		}

		public EmailAddressCollection ToMsg
		{
			get { return _to_msg; }
			set { _to_msg = value; }
		}

		public EmailAddressCollection CcMsg
		{
			get { return _cc_msg; }
			set { _cc_msg = value; }
		}

		public EmailAddressCollection BccMsg
		{
			get { return _bcc_msg; }
			set { _bcc_msg = value; }
		}

		public string Subject
		{
			get { return _subject; }
			set { _subject = value; }
		}

		public DateTime MsgDate
		{
			get { return _msg_date; }
			set { _msg_date = value; }
		}

		public bool Attachments
		{
			get { return _attachments; }
			set { _attachments = value; }
		}

		public long Size
		{
			get { return _size; }
			set { _size = value; }
		}

		public bool Seen
		{
			get { return _seen; }
			set { _seen = value; }
		}

		public bool Flagged
		{
			get { return _flagged; }
			set { _flagged = value; }
		}

		public MailPriority Priority
		{
			get { return _priority; }
			set { _priority = value; }
		}

		public bool Downloaded
		{
			get { return _downloaded; }
			set { _downloaded = value; }
		}

		public bool XSpam
		{
			get { return _x_spam; }
			set { _x_spam = value; }
		}

		public bool Rtl
		{
			get { return _rtl; }
			set { _rtl = value; }
		}

		public bool Deleted
		{
			get { return _deleted; }
			set { _deleted = value; }
		}

		public bool IsFull
		{
			get { return _is_full; }
			set { _is_full = value; }
		}

		public bool Replied
		{
			get { return _replied; }
			set { _replied = value; }
		}

		public bool Forwarded
		{
			get { return _forwarded; }
			set { _forwarded = value; }
		}

		public SystemMessageFlags Flags
		{
			get { return _flags; }
			set { _flags = value; }
		}

		public string BodyText
		{
			get { return _body_text; }
			set { _body_text = value; }
		}

		public string FolderFullName
		{
			get { return _folder_full_name; }
			set { _folder_full_name = value; }
		}

		public bool Grayed
		{
			get { return _grayed; }
			set { _grayed = value; }
		}

		public int OverrideCharset
		{
			get { return _charset; }
			set { _charset = value; }
		}

		public byte[] RawMsg
		{
			get { return _rawMsg; }
		}
		
		public byte Safety
		{
			get{
				return _safety;
			}
			set{
				_safety = value;
			}
		}
		#endregion

		public WebMailMessage(Account acct)
		{
			_account = acct;
			_id_acct = acct.ID;
		}

		public void Init(MailMessage msg, bool isUidStr, Folder fld)
		{
			if (isUidStr)
			{
				string uid = msg.UidOnServer as string;
				if (uid != null) _str_uid = uid;
			}
			else
			{
				if ((msg.UidOnServer != null) && ((Convert.ToInt64(msg.UidOnServer) != -1)))
				{
					_int_uid = Convert.ToInt64(msg.UidOnServer);
				}
			}
			if (fld != null)
			{
				_id_folder_db = fld.ID;
				_folder_full_name = fld.FullPath;
			}
			InitFromMailMessage(msg);
			if (fld != null && fld.SyncType != FolderSyncType.DontSync)
			{
				_downloaded = msg.IsEntire;
			}
			else
			{
				_downloaded = true;
			}
		}

		private void InitFromMailMessage(MailMessage msg)
		{
			_message = msg;
			if ((_account != null) && (_account.UserOfAccount != null) && (_account.UserOfAccount.Settings != null))
			{
				if ((_message.Charset == null) || (_message.Charset == string.Empty))
				{
					Encoding defEnc = Encoding.Default;
					try
					{
						defEnc = Utils.GetEncodingByCodePage(_account.UserOfAccount.Settings.DefaultCharsetInc);
					}
					catch {}
					_message.Parser.EncodingDefault = defEnc;
					if (_charset > 0)
					{
						_message.Parser.EncodingOverride = Utils.GetEncodingByCodePage(_charset);
					}
					_message.Parser.Apply();
				}
			}
			_rawMsg = _message.GetMessageRawData();
			_from_msg = _message.From;
			_to_msg = _message.To;
			_cc_msg = _message.Cc;
			_bcc_msg = _message.Bcc;
			_subject = _message.Subject;
			_message.Parser.DatesAsUtc = true;
			DateTime dt = Constants.MinDate;
			try
			{
				dt = (_message.DateReceived != DateTime.MinValue) ? _message.DateReceived : _message.Date;
			}
			catch (MailBeeDateParsingException)
			{
				dt = DateTime.Now;
			}
			_msg_date = (dt < Constants.MinDate) ? Constants.MinDate : dt;
			_attachments = _message.HasAttachments;
			_size = (_message.SizeOnServer > 0) ? _message.SizeOnServer : _message.Size;
			_priority = _message.Priority;

			_body_text = _message.BodyPlainText.ToLower();
		}

		public void InitImapFlags(MessageFlagSet flags)
		{
			_seen = (flags.SystemFlags & SystemMessageFlags.Seen) > 0;
			_replied = (flags.SystemFlags & SystemMessageFlags.Answered) > 0;
			_flagged = (flags.SystemFlags & SystemMessageFlags.Flagged) > 0;
			_deleted = (flags.SystemFlags & SystemMessageFlags.Deleted) > 0;
		}

	}
}
