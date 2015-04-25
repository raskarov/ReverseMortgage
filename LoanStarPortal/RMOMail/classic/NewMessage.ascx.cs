using System.Globalization;
using MailBee.Mime;

namespace WebMailPro.classic
{
	using System;

	public enum MessageAction
	{
		New,
		Reply,
		ReplyAll,
		Forward
	}

	/// <summary>
	///		Summary description for NewMessage.
	/// </summary>
	public partial class NewMessage : System.Web.UI.UserControl
	{
		protected System.Web.UI.HtmlControls.HtmlTextArea textareaBody;
		protected WebMailMessage _message = null;
		protected MessageAction _action = MessageAction.New;
		protected WebmailResourceManager _resMan = null;
		protected string _skin = Constants.defaultSkinName;
		protected string editor_resize = string.Empty;
		protected string editor_replace = string.Empty;
		protected bool isHtml;
		protected string attachmentsHtml = string.Empty;
		protected bool withSignature;
		protected string body = string.Empty;
		protected string setText = string.Empty;

		protected int _id = -1;
		protected string _uid = string.Empty;
		protected long _id_folder = -1;
		protected string _full_name_folder = string.Empty;
		protected int _charset = 0;
		protected string _to = string.Empty;
		protected System.Web.UI.WebControls.LinkButton eisShowAll;
		
		protected bool _acceptFlag = false;

        public string a_start1 = string.Empty;
        public string a_start2 = string.Empty;
        public string a_start3 = string.Empty;
        public string a_end = string.Empty;
		
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

		public MessageAction Action
		{
			get { return _action; }
			set { _action = value; }
		}

		public int id
		{
			get { return _id; }
			set { _id = value; }
		}

		public string uid
		{
			get { return _uid; }
			set { _uid = value; }
		}

		public long id_folder
		{
			get { return _id_folder; }
			set { _id_folder = value; }
		}

		public string full_name_folder
		{
			get { return _full_name_folder; }
			set { _full_name_folder = value; }
		}

		public int charset
		{
			get { return _charset; }
			set { _charset = value; }
		}

		public string to
		{
			get { return _to; }
			set { _to = value; }
		}
		
		public bool AcceptFlag
		{
			get {return _acceptFlag;}
			set {_acceptFlag = value;}
		}
        private void SetLoanAssociation()
        {
            LoanStar.Common.MortgageProfile mp = new LoanStar.Common.MortgageProfile(Convert.ToInt32(Session[LoanStar.Common.Constants.MortgageID], null));
            lblLoan.Text = mp.YoungestBorrower.FullName;
            trLoan.Visible = true;

        }

	    protected void Page_Load(object sender, System.EventArgs e)
		{
			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			eisShowOne.Text = _resMan.GetString("ShowPictures");
			eisShowOne.Attributes["href"] = "#acceptThis";
			eisShowOne.Attributes["onclick"] = "javascript: eisAcceptNew()";
			eisPanel.Visible = false;
            if(Request.QueryString["scr"]!=null)
            {
                if (Request.QueryString["scr"]=="condition_message")
                {
                    SetLoanAssociation();
                }
            }
//			if (!Page.IsPostBack)
//			{
				try
				{
					if ((this.Request.QueryString["to"] != null) || (this.Request.QueryString["to"] != string.Empty))
					{
						_to = this.Request.QueryString["to"];
					}

					Account acct = Session[Constants.sessionAccount] as Account;
					if (acct != null)
					{
						WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
						if (settings.AllowContacts)
                        {
                            a_end = "</a>";
                            a_start1 = @"<a href=""#"" onclick=""PopupContacts('contactlist.aspx?f=1');"">";
                            a_start2 = @"<a href=""#"" onclick=""PopupContacts('contactlist.aspx?f=2');"">";
                            a_start3 = @"<a href=""#"" onclick=""PopupContacts('contactlist.aspx?f=3');"">";
                        }

                        BaseWebMailActions actions = new BaseWebMailActions(acct, this.Page);
						if ((id > 0)||(uid!=String.Empty))
						{
							if (_acceptFlag)
							{
								_message = actions.GetMessage(id, uid, id_folder, full_name_folder, charset, 1);
								_message.Safety = 1;
								_acceptFlag = false;
							}
							else
								_message = actions.GetMessage(id, uid, id_folder, full_name_folder, charset);
								
						}

						foreach (string contactStr in Request.Form)
						{
							if (contactStr.StartsWith("contacts_"))
							{
								string[] contactArr = contactStr.Split(new char[] { '_' });
								if (contactArr.Length == 3)
								{
									int contactID = int.Parse(contactArr[1], CultureInfo.InvariantCulture);
									if (string.Compare(contactArr[2], "g", true, CultureInfo.InvariantCulture) == 0)
									{
										AddressBookGroup group = actions.GetGroup(contactID);
										if (group != null)
										{
											foreach (AddressBookContact contact in group.Contacts)
											{
												if (contact != null)
												{
													string email = contact.HEmail;
													switch (contact.PrimaryEmail)
													{
														case ContactPrimaryEmail.Business:
															email = contact.BEmail;
															break;
														case ContactPrimaryEmail.Other:
															email = contact.OtherEmail;
															break;
													}
													if (email.Length > 0) _to += string.Format(@"{0},", email);
												}
											}
										}
									}
									else
									{
										AddressBookContact contact = actions.GetContact(contactID);
										if (contact != null)
										{
											string email = contact.HEmail;
											switch (contact.PrimaryEmail)
											{
												case ContactPrimaryEmail.Business:
													email = contact.BEmail;
													break;
												case ContactPrimaryEmail.Other:
													email = contact.OtherEmail;
													break;
											}
											if (email.Length > 0) _to += string.Format(@"{0},", email);
										}
									}
								}
							}
						}

						switch (_action)
						{
							case MessageAction.New:
								InitComponentForNew();
								break;
							case MessageAction.Reply:
								InitComponentForReply();
								break;
							case MessageAction.ReplyAll:
								InitComponentForReplyAll();
								break;
							case MessageAction.Forward:
								InitComponentForForward();
								break;
						}
						if (acct.UserOfAccount.Settings.AllowDhtmlEditor)
						{
							editor_resize = "HTMLEditor.Resize(width, height);";
							editor_replace = "HTMLEditor.Replace();";
						}
						else
						{
							editor_resize = @"plainEditor.style.height = (height - 1) + ""px"";
					plainEditor.style.width = (width - 2) + ""px"";
					";
							editor_replace = "";
						}

						isHtml = acct.UserOfAccount.Settings.AllowDhtmlEditor;
						attachmentsHtml = string.Empty;

						js.AddText(@"
var bcc, bcc_mode, bcc_mode_switcher, hiddenform;

var plainCont = null;
var plainEditor = null;
var HTMLEditor = null;
var EditAreaUrl = 'edit-area.aspx';
var prevWidth = 0;
var prevHeight = 0;
var rowIndex = 0;

function ResizeElements(mode)
{
	var width = GetWidth();
	if (width < 684)
		width = 684;
	width = width - 30;
	var height = Math.ceil(width/3);
	
	if (prevWidth != width && prevHeight != height) {
		prevWidth = width;
		prevHeight = height;
		if (plainCont != null) {
			plainCont.style.height = height + ""px"";
			plainCont.style.width = width + ""px"";
			" + editor_resize + @"			
		}
	}
}

				function eisAcceptNew()
				{
					HFAction = document.getElementById(""HFAction"");
					HFRequest = document.getElementById(""HFRequest"");
					HFAction.value = ""eis"";
					HFRequest.value = ""acceptnew"";
					__doPostBack('PostBackButton','');
				}

function ChangeBCCMode()
{
	if (!bcc) bcc = document.getElementById('bcc');
	if (!bcc_mode_switcher) bcc_mode_switcher = document.getElementById('bcc_mode_switcher');
    if (bcc_mode == 'hide') {
        bcc_mode = 'show';
        bcc.className = '';
        bcc_mode_switcher.innerHTML = Lang.HideBCC;
    } else {
        bcc_mode = 'hide';
        bcc.className = 'wm_hide';
        bcc_mode_switcher.innerHTML = Lang.ShowBCC;
    }
" + editor_replace + @"	
    return false;
}

function LoadAttachmentHandler(attachObj)
{
    var attachtable = document.getElementById('attachmentTable');
    if (attachObj)
    {
        var imageLink = GetFileParams(attachObj.FileName);
        var tr = attachtable.insertRow(rowIndex++);
		var now = new Date();
		var tempname = now.getTime() + now.getMilliseconds() + '_' + attachObj.TempName;
        tr.id = 'tr_' + tempname;
        var td = tr.insertCell(0);
        td.className = 'wm_attachment';
        var innerHtml = '<img src=\'./images/icons/' + imageLink.image + '\' />';
        innerHtml += '<input type=\'hidden\' name=""attachments[' + attachObj.TempName + ']"" value=\'' + attachObj.FileName + '\'>';
        innerHtml += attachObj.FileName + ' (' + GetFriendlySize(attachObj.Size) + ') <a href=\'#\' id=\'' + tempname + '\' onclick=\'return  DeleteAttach(this.id);\'>' + Lang.Delete + '</a>';
        td.innerHTML = innerHtml;
    }
}

function DeleteAttach(idline)
{
    var trtable = document.getElementById(""tr_"" + idline);
    if (trtable)
    {
        trtable.className = ""wm_hide"";
        CleanNode(trtable);
    }
    return false;
}

function addattach()
{
    if (!hiddenform) hiddenform = CreateChildWithAttrs(document.body, 'form', [['method', 'POST'],['action', 'upload.aspx'], ['target', 'UploadFrame'], ['enctype', 'multipart/form-data'], ['class', 'wm_hide']]);
    var fileinput = document.getElementById('fileupload');
    hiddenform.appendChild(fileinput);
    hiddenform.submit();
	CleanNode(hiddenform);
	CreateChildWithAttrs(document.getElementById('forfile'), 'input', [['type', 'file'],['name', 'fileupload'], ['id', 'fileupload'], ['class', 'wm_file']]);
}

function WriteEmails(str, field)
{
	var mailInput;
	if (field == 2) {
		mailInput = document.getElementById(""newMessageID_ccTextBox"");
	} else if (field == 3) {
		mailInput = document.getElementById(""newMessageID_bccTextBox"");
	} else {
		mailInput = document.getElementById(""newMessageID_toTextBox"");
	}
	if (mailInput) {
		mailInput.value = (mailInput.value == """") ? str : mailInput.value + "", "" + str;
		mailInput.focus();
	}
}
			");

						js.AddInitText(@"
bcc_mode = 'hide';
bcc = document.getElementById('bcc');
bcc_mode_switcher = document.getElementById('bcc_mode_switcher');

plainEditor = document.getElementById('editor_area');
plainCont = document.getElementById('editor_cont');
			");

						withSignature = false;
						switch (acct.SignatureOptions)
						{
							case SignatureOptions.AddSignatureToAllOutgoingMessages:
								withSignature = true;
								break;
							case SignatureOptions.DontAddSignatureToRepliesAndForwards:
								withSignature = (_action == MessageAction.New);
								break;
							case SignatureOptions.DontAddSignature:
								withSignature = false;
								break;
						}

						if (_message != null)
						{
							if ((_action != MessageAction.Forward) && (_action != MessageAction.Reply) && (_action != MessageAction.ReplyAll))
							{
								withSignature = false;
							}

							js.AddInitText(@"SetPriority(" + (int)_message.Priority + ");");

							if (acct.UserOfAccount.Settings.AllowDhtmlEditor)
							{
								switch (_action)
								{
									case MessageAction.Forward:
									case MessageAction.Reply:
									case MessageAction.ReplyAll:
										if (_message.MailBeeMessage.BodyHtmlText.Length > 0)
										{
											isHtml = true;
											body = Utils.GetMessageHtmlReplyToBody(acct, _message.MailBeeMessage);
											if ((_message.Safety == 0)&&(isHtml)) eisPanel.Visible = true;
											else eisPanel.Visible = false;
										}
										else if (_message.MailBeeMessage.BodyPlainText.Length > 0)
										{
											isHtml = false;
											body = Utils.GetMessagePlainReplyToBody(acct, _message.MailBeeMessage);
										}
										break;
									default:
										if (_message.MailBeeMessage.BodyHtmlText.Length > 0)
										{
											isHtml = true;
											body = _message.MailBeeMessage.BodyHtmlText;
										}
										else if (_message.MailBeeMessage.BodyPlainText.Length > 0)
										{
											isHtml = false;
											body = _message.MailBeeMessage.BodyPlainText;
										}
										break;
								}
							}
							else
							{
								isHtml = false;
								switch (_action)
								{
									case MessageAction.Forward:
									case MessageAction.Reply:
									case MessageAction.ReplyAll:
										body = Utils.GetMessagePlainReplyToBody(acct, _message.MailBeeMessage);
										break;
									default:
										body = _message.MailBeeMessage.BodyPlainText;
										break;
								}
							}

							if (_message.Attachments && (_action != MessageAction.Reply) && (_action != MessageAction.ReplyAll))
							{
								foreach (Attachment attach in _message.MailBeeMessage.Attachments)
								{
									attachmentsHtml += String.Format(@"
<tr id=""tr_{5}"">
		<td class=""wm_attachment""><input type=""hidden"" name=""attachments[{5}]"" value=""{1}""><img src=""{0}""/><span>{1} ({2}) </span><a href=""{3}"" onClick=""return  DeleteAttach(this.id);"">{4}</a></td>
</tr>",
										Utils.GetAttachmentIconLink(attach), // 0
										Utils.GetShortFilename((attach.Name.Length > 0) ? attach.Name : attach.Filename), // 1
										Utils.GetFriendlySize(attach.Size), // 2
										"#", // 3
										_resMan.GetString("Delete"), // 4
										attach.SavedAs					
										);
								}
							}
						}
						else
						{
							js.AddInitText("SetPriority(3);");
							string signature = string.Empty;
							if (acct.SignatureOptions != SignatureOptions.DontAddSignature)
							{
								if (acct.UserOfAccount.Settings.AllowDhtmlEditor)
								{
									if (acct.SignatureType == SignatureType.Html)
									{
										signature = "\r\n\r\n" + acct.Signature;
									}
									else
									{
										signature = "<br/><br/>" + acct.Signature;
									}
								}
								else
								{
									if (acct.SignatureType == SignatureType.Html)
									{
										signature = "\r\n\r\n" + Utils.ConvertHtmlToPlain(acct.Signature);;
									}
									else
									{
										signature = "<br/><br/>" + acct.Signature;
									}
								}
							}
							body = signature;
						}

						if (acct.UserOfAccount.Settings.AllowDhtmlEditor)
						{
							js.AddFile(@"class.html-editor.js");
							setText = (isHtml) ? "HTMLEditor.SetHtml(mess);" : "HTMLEditor.SetText(mess);";
							body = body.Replace("\"", @"\""");
							body = body.Replace("\n", @"\n");
							body = body.Replace("\r", @"\r");
							js.AddText(@"
		function submitSaveMessage()
        {
            var hiddenkey = document.getElementById(""ishtml"");
            
            if (HTMLEditor._htmlMode) {
                plainEditor.value = HTMLEditor.GetText();
                hiddenkey.value = ""1"";
            } else {
                hiddenkey.value = ""0"";
            }
            if (bcc_mode == ""hide"")
            {
                document.getElementById(""newMessageID_bccTextBox"").value = """";
            }
            return true;
        }
     
        function EditAreaLoadHandler() { HTMLEditor.LoadEditArea();    }
        function CreateLinkHandler(url) { HTMLEditor.CreateLinkFromWindow(url); }
        function DesignModeOnHandler(rer) {
            HTMLEditor.Show();
            var mess = """ + body + @""";
            if (mess.length == 0) mess = ""<br />"";
            " + setText + @"
        }");
							js.AddInitText(@"
HTMLEditor = new CHtmlEditorField(true);
HTMLEditor.SetPlainEditor(plainEditor, document.getElementById(""editor_switcher""));
HTMLEditor.Show();
");
						}
						else
						{
							js.AddText(@"
     function submitSaveMessage()
        {
            var hiddenkey = document.getElementById(""ishtml"");
            hiddenkey.value = ""0"";
            if (bcc_mode == ""hide"")
            {
                document.getElementById(""newMessageID_bccTextBox"").value = """";
            }
            return true;
        }
");
						}

						if (Request.QueryString["mailto"] != null)
						{
							toTextBox.Value = Request.QueryString["mailto"];
						}
					}
				}
				catch (Exception ex)
				{
					((basewebmail)this.Page).OutputException(ex);
				}
//			} // !PostBack
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

		private void InitComponentForNew()
		{
			Account acct = Session[Constants.sessionAccount] as Account;
			if (acct != null)
			{
				string from = acct.Email;
				if (acct.UseFriendlyName && acct.FriendlyName.Trim().Length > 0)
				{
					from = string.Format(@"""{0}"" <{1}>", acct.FriendlyName.Trim(), acct.Email);
				}
				this.fromTextBox.Value = from;
				this.toTextBox.Value = to;
				if (_message != null)
				{
					this.toTextBox.Value = _message.ToMsg.ToString();
					this.ccTextBox.Value = _message.CcMsg.ToString();
					this.bccTextBox.Value = _message.BccMsg.ToString();
					this.subjectTextBox.Value = _message.Subject;
				}
			}
		}

		public void InitComponentForForward()
		{
			if (_message != null)
			{
				InitComponentForNew();

				this.ccTextBox.Value = _message.CcMsg.ToString();
				this.bccTextBox.Value = _message.BccMsg.ToString();
				this.subjectTextBox.Value = _message.Subject;
			}
		}

		public void InitComponentForReply()
		{
			InitComponentForForward();
			this.toTextBox.Value = _message.FromMsg.ToString();
			this.subjectTextBox.Value = string.Format("Re: {0}", _message.Subject);
		}

		public void InitComponentForReplyAll()
		{
			InitComponentForReply();
			this.toTextBox.Value = _message.FromMsg.ToString() + ", " + _message.CcMsg.ToString();
		}
		
		public string SwitcherCode()
		{
			Account acct = Session[Constants.sessionAccount] as Account;
			if (acct != null)
			{
				return (acct.UserOfAccount.Settings.AllowDhtmlEditor)
					? @"<a class=""wm_reg"" href=""#"" id=""editor_switcher"">" + _resMan.GetString("SwitchToPlainMode") + @"</a>" :"";
			}
			return string.Empty;
		}

	}
}