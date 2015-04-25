using System.Globalization;

namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Text.RegularExpressions;

	/// <summary>
	///		Summary description for Inbox.
	/// </summary>
	public partial class Inbox : System.Web.UI.UserControl
	{
		protected WebmailResourceManager _manager = null;
		protected string _skin = Constants.defaultSkinName;
		protected long _id_folder = -1;
		protected string _full_name_folder = string.Empty;
		protected int _page_number = 1;
		protected int _sort_field = 0;
		protected int _sort_order = 0;
		protected string _look_for = string.Empty;
		protected bool IsSearch = false;
		protected int _search_fields = 0;
		protected int _folderMessageCount = 0;
		protected int _folderUnreadMessageCount = 0;
		protected string _messObj = null;
		protected string _mailId = null;
		protected Account acct;
		protected string LabelToFrom = "From";

		public string MailId
		{
			get
			{
				return _mailId;
			}
			set
			{
				_mailId = value;
			}
		}

		protected jsbuilder _js;
		public jsbuilder js
		{
			get { return _js; }
			set { _js = value; }
		}

		public string Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}

		public int PageNumber
		{
			get { return _page_number; }
			set { _page_number = value; }
		}

		public int SortField
		{
			get { return _sort_field; }
			set { _sort_field = value; }
		}

		public int SortOrder
		{
			get { return _sort_order; }
			set { _sort_order = value; }
		}

		public string LookFor
		{
			get { return _look_for; }
			set
			{
				_look_for = value;
			}
		}

		public int SearchFields
		{
			get { return _search_fields; }
			set { _search_fields = value; }
		}

		public int FolderMessageCount
		{
			get { return _folderMessageCount; }
		}

		public int FolderUnreadMessageCount
		{
			get { return _folderUnreadMessageCount; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			acct = Session[Constants.sessionAccount] as Account;
			if (acct != null)
			{
				_manager = (new WebmailResourceManagerCreator()).CreateResourceManager();

				ResetValues();
			
				LoadData();
				if(!IsPostBack)
				{
					OutputMessagesList(acct, _id_folder);
				}
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		protected void ResetValues()
		{
			LiteralList.Text = String.Empty;
			_messObj = String.Empty;
		}

		protected void LoadData()
		{
			_id_folder = (Session["id_folder"] != null) ? (long)Session["id_folder"] : -1;
			IsSearch = (Session["is_search"] != null) ? (bool)Session["is_search"] : false;
			if(IsSearch)
			{
				_look_for = (Session["look_for"] != null) ? (string)Session["look_for"] : string.Empty;
			}
			else
			{
				_look_for = string.Empty;
			}
		}

		public void OutputMessagesList(Account acct, long IDFolder)
		{
			string emptyListStr = (IsSearch) ? _manager.GetString("InfoNoMessagesFound") : _manager.GetString("InfoEmptyFolder");
			string messageListContent = string.Format(@"<tr><td style=""width: 404px;"" colspan=""6""/><td id=""subject"" style=""width: 150px;""/></tr><tr><td colspan=""7""><div class=""wm_inbox_info_message"">{0}</div></td></tr>", emptyListStr);
			try
			{
				ResetValues();

				switch(acct.DefaultOrder)
				{
					case WebMailPro.DefaultOrder.AttachmentDesc:
						_sort_order = 0;
						_sort_field = 10;
						break;
					case WebMailPro.DefaultOrder.Attachment:
						_sort_order = 1;
						_sort_field = 10;
						break;
					case WebMailPro.DefaultOrder.FlagDesc:
						_sort_order = 0;
						_sort_field = 12;
						break;
					case WebMailPro.DefaultOrder.Flag:
						_sort_order = 1;
						_sort_field = 12;
						break;
					case WebMailPro.DefaultOrder.FromDesc:
						_sort_order = 0;
						_sort_field = 2;
						break;
					case WebMailPro.DefaultOrder.From:
						_sort_order = 1;
						_sort_field = 2;
						break;
					case WebMailPro.DefaultOrder.DateDesc:
						_sort_order = 0;
						_sort_field = 0;
						break;
					case WebMailPro.DefaultOrder.Date:
						_sort_order = 1;
						_sort_field = 0;
						break;
					case WebMailPro.DefaultOrder.SizeDesc:
						_sort_order = 0;
						_sort_field = 6;
						break;
					case WebMailPro.DefaultOrder.Size:
						_sort_order = 1;
						_sort_field = 6;
						break;
					case WebMailPro.DefaultOrder.SubjDesc:
						_sort_order = 0;
						_sort_field = 8;
						break;
					case WebMailPro.DefaultOrder.Subj:
						_sort_order = 1;
						_sort_field = 8;
						break;
				}

				BaseWebMailActions bwa = new BaseWebMailActions(acct, this.Page);

				if (IDFolder != -1)
				{
					_id_folder = IDFolder;
					Folder CurrenFolder =  bwa.GetFolder(IDFolder);

					if(CurrenFolder.Type == FolderType.Drafts || CurrenFolder.Type == FolderType.SentItems)
					{
						LabelToFrom = "To";
					}
					else
					{
						Folder SentItemsFolder = bwa.GetFolder(FolderType.SentItems);
						if (CurrenFolder.FullPath.IndexOf(SentItemsFolder.FullPath + acct.Delimiter) == 0)
						{
							LabelToFrom = "To";
						}
						else
						{
							Folder DraftsFolder = bwa.GetFolder(FolderType.Drafts);
							if (CurrenFolder.FullPath.IndexOf(DraftsFolder.FullPath + acct.Delimiter) == 0)
							{
								LabelToFrom = "To";
							}
							else
							{
								LabelToFrom = "From";
							}
						}
					}

					_full_name_folder = CurrenFolder.FullPath;
				}
				else
				{
					_id_folder = IDFolder;
					_full_name_folder = "";
				}
				WebMailMessageCollection msgsColl = bwa.GetMessages(_id_folder, _full_name_folder, _page_number, _sort_field, _sort_order, _look_for, _search_fields, out _folderMessageCount, out _folderUnreadMessageCount);
				System.Web.UI.HtmlControls.HtmlInputHidden HFPageInfo = (System.Web.UI.HtmlControls.HtmlInputHidden)  this.Page.FindControl("HFPageInfo");

				HFPageInfo.Value = _page_number + "----" + acct.UserOfAccount.Settings.MsgsPerPage + "----" + _folderMessageCount;
	
				if(_folderMessageCount > 0)
				{
                    if (msgsColl.Count > 0) messageListContent = string.Empty;
                    for (int i = 0; i < msgsColl.Count; i++)
					{
						WebMailMessage webMsg = msgsColl[i];
                        webMsg.IDFolderSrv = webMsg.IDFolderDB = _id_folder;
						webMsg.FolderFullName = _full_name_folder;
						messageListContent += OutputWebMailMessage(acct, webMsg, (i == 0));
					}

                    _messObj = "InboxLines = new CSelection();" + _messObj;
					_js.AddInitText(_messObj);
				}
            }
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
			finally
			{
				LiteralList.Text = @"<table id=""list"">" + messageListContent + "</table>";
			}
		}

		public string OutputWebMailMessage(Account acct, WebMailMessage webMsg, bool firstMessage)
		{
			string replyForwardStr = string.Empty;
			//if (webMsg.Replied) replyForwardStr = string.Format(@"<img src=""skins/{0}/menu/reply.gif"">", _skin);
			//if (webMsg.Forwarded) replyForwardStr = string.Format(@"<img src=""skins/{0}/menu/forward.gif"">", _skin);

			string attachStr = string.Empty;
			if (webMsg.Attachments) attachStr = string.Format(@"<img src=""skins/{0}/menu/attachment.gif"">", _skin);

			string flagStr = string.Empty;
			if (webMsg.Flagged)
				flagStr = string.Format(@"<img class=""wm_control_img"" src=""skins/{0}/menu/flag.gif"" id=""flag_img_{1}"">", _skin, webMsg.ID);
			else
				flagStr = string.Format(@"<img class=""wm_control_img"" src=""skins/{0}/menu/unflag.gif"" id=""flag_img_{1}"">", _skin, webMsg.ID);

			string toFromStr = string.Empty;
			if (LabelToFrom == "From")
			{
				toFromStr = (_look_for.Length > 0) ? Regex.Replace(Server.HtmlEncode((webMsg.FromMsg.DisplayName != "") ? webMsg.FromMsg.DisplayName: webMsg.FromMsg.ToString()), "(?i)" + _look_for + "(?-i)", "<font>" + _look_for + "</font>") : Server.HtmlEncode((webMsg.FromMsg.DisplayName != "") ? webMsg.FromMsg.DisplayName: webMsg.FromMsg.ToString());
			}
			else
			{
				toFromStr = (_look_for.Length > 0) ? Regex.Replace(Server.HtmlEncode(Utils.GetAddressesFriendlyName(webMsg.ToMsg)), "(?i)" + _look_for + "(?-i)", "<font>" + _look_for + "</font>") : Server.HtmlEncode(Utils.GetAddressesFriendlyName(webMsg.ToMsg));
			}

			_mailId = string.Format(@"{0}----{1}----{2}----{3}----{4}",
				webMsg.IDMsg, // 0
				((webMsg.StrUid != null) && (webMsg.StrUid.Length > 0)) ? webMsg.StrUid : webMsg.IntUid.ToString(CultureInfo.InvariantCulture),
				webMsg.IDFolderDB, // 2
				webMsg.FolderFullName, // 3
				webMsg.OverrideCharset // 4
				);

			string messageRow = string.Format(@"
			<tr id=""{0}"" onclick=""CheckThisLine(event, this);"" ondblclick=""CheckThisLineDb(event, this);"">
				<td{8} id=""none""><input type=""checkbox"" /></td>
				<td{9}>{2}</td>
				<td{10}>{3}</td>
				<td class=""wm_inbox_from_subject""{11}><nobr>{4}</nobr></td>
				<td{12}><nobr>{5}</nobr></td>
				<td{13}><nobr>{6}</nobr></td>
				<td class=""wm_inbox_from_subject""{14}{15}><nobr>{7}</nobr></td>
			</tr>
", 
				Utils.AttributeQuote(_mailId), //0
				replyForwardStr, // 1
				attachStr, // 2
				flagStr, // 33
				toFromStr, // 4
				Utils.GetDateWithOffsetFormatString(acct, webMsg.MsgDate), // 5
				Utils.GetFriendlySize(webMsg.Size), // 6
				(_look_for.Length > 0) ? Regex.Replace(Server.HtmlEncode(webMsg.Subject), "(?i)" + _look_for + "(?-i)", "<font>" + _look_for + "</font>") : Server.HtmlEncode(webMsg.Subject), // 7
                (firstMessage) ? @" style=""WIDTH: 21px; text-align: center;""" : "", // 8
				(firstMessage) ? @" style=""WIDTH: 20px""" : "", // 9
				(firstMessage) ? @" style=""WIDTH: 20px""" : "", // 10
				(firstMessage) ? @" style=""WIDTH: 150px""" : "", // 11
				(firstMessage) ? @" style=""WIDTH: 140px""" : "", // 12
				(firstMessage) ? @" style=""WIDTH: 48px""" : "", // 13
				(firstMessage) ? @" style=""WIDTH: 148px""" : "", // 14
                (firstMessage) ? @" id=""subject""" : "" // 15
				);

				_messObj += string.Format(@"
messObj = {{Read: {0}, Replied: {1}, Forwarded: {2}, Flagged: {3}, Deleted: {4}, Gray: {5}, Id: {6}, Uid: ""{7}"", FolderId: {8}, FolderFullName: ""{9}"", FromAddr: """", Subject: """"}};
var messTr = document.getElementById(""{10}"");
if (messTr) InboxLines.AddLine(new CSelectionPart(messTr, ""{11}"" , messObj));",
				webMsg.Seen.ToString().ToLower(), //0
				webMsg.Replied.ToString().ToLower(), //1
				webMsg.Forwarded.ToString().ToLower(), //2
				webMsg.Flagged.ToString().ToLower(), //3
				webMsg.Deleted.ToString().ToLower(), //4
				webMsg.Grayed.ToString().ToLower(), //5
				webMsg.ID.ToString(), //6
				Utils.EncodeJsSaveString(webMsg.StrUid.ToString()), //7
				webMsg.IDFolderDB.ToString(), //8
				webMsg.FolderFullName.ToString(), //9
				Utils.EncodeJsSaveString(_mailId), //10
				_skin //11
				);

			return messageRow;
		}
	}
}
