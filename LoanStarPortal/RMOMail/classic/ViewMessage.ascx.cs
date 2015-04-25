namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Globalization;
	using System.Text.RegularExpressions;
    using System.IO;

	/// <summary>
	///		Summary description for ViewMessage.
	/// </summary>
	public partial class ViewMessage : System.Web.UI.UserControl
	{

		protected int _id_msg = 0;
		protected string _uid = null;
		protected long _id_folder = 0;
		protected string _folder_full_name = null;
		protected int _charset = 0;
		protected jsbuilder _js;
		protected string _skin;
		protected Account acct;
		protected WebmailResourceManager _manager = null;
		protected WebMailMessage msg = null;
		protected MailBee.Mime.AttachmentCollection Attachment = null;
		protected bool LoadHTMLMessage = true;

		protected bool IsHTMLMsg = false;
		protected string msgPlain = null;
		protected string msgHTML = null;
		protected string HTMLAttachments = null;
		protected string HTMLViewLink = null;
		protected string DownloadLink = null;
		protected string IconLink = null;
		protected string ViewLink = null;
		protected string MessageDownloadLink = "";

		public int id
		{
			get { return _id_msg; }
			set { _id_msg = value; }
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
			get { return _folder_full_name; }
			set { _folder_full_name = value; }
		}

		public int charset
		{
			get { return _charset; }
			set { _charset = value; }
		}
		
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

		protected void Page_Load(object sender, System.EventArgs e)
		{
			acct = Session[Constants.sessionAccount] as Account;

			if (acct == null)
			{
				Response.Redirect("default.aspx");
			}
			else
			{
				_manager = (new WebmailResourceManagerCreator()).CreateResourceManager();

				MessageDownloadLink = "";

				string text = @"
					function ResizeElements(mode) 
					{
						NewResizeElements(mode);
						if (!Browser.IE) NewResizeElements(mode);
					}

					function NewResizeElements(mode)
					{
						var height = GetHeight();
						var width = GetWidth();
						var exx = 0; 
						var offsetHeight = 0;
						offsetHeight = (logo) ? logo.offsetHeight : 0;
						exx += (offsetHeight) ? offsetHeight : 0;
						offsetHeight = (accountlist) ? accountlist.offsetHeight : 0;
						exx += (offsetHeight) ? offsetHeight : 0;
						offsetHeight = (toolbar) ? toolbar.offsetHeight : 0;
						exx += (offsetHeight) ? offsetHeight : 0;
						mainContainer.style.height = (height - exx) + 'px';      
						mainContainer.style.width = width + 'px';
						m_message.style.width = (width - 20) + 'px';

						if (resizerObj != null) var atWidth = resizerObj._leftShear;
						else var atWidth = '0';

var maxAttachWidth = width - 42;
if (atWidth > maxAttachWidth) atWidth = maxAttachWidth;

m_attachments.style.height = resDiv.style.height = m_message.style.height = (height - exx - m_headers.offsetHeight - m_lowtoolbar.offsetHeight - 4) + 'px';
m_message_body.style.height = (height - exx - m_headers.offsetHeight - m_lowtoolbar.offsetHeight - 16 - 4) + 'px';

m_attachments.style.width = atWidth + 'px';
m_attachments.style.left = '0px';

if (messageWithAttach)
{
	m_message_body.style.width = (mainContainer.offsetWidth - m_attachments.offsetWidth - resDiv.offsetWidth - 16 - 15) + 'px';
	m_message_body.style.left = (atWidth + 2) + 'px';
	if (resizerObj != null) 
	{
		resDiv.style.width = '2px';
		resDiv.style.left = atWidth + 'px';
	}
}
else
{
	m_message_body.style.width = (mainContainer.offsetWidth - 16 - 15) + 'px';
	m_message_body.style.left = '0px';
}
					}

					function ShowHideHeaders()
					{
						var objDiv = document.getElementById(""headersCont"");
						var objSpan = document.getElementById(""ShowHideText"");
						var Show = """ + _manager.GetString("ShowFullHeaders") + @"""
						var Hide = """ + _manager.GetString("HideFullHeaders") + @"""

						if(objDiv.className == ""wm_hide"")
						{
							objSpan.innerHTML = Hide;
							objDiv.className = ""wm_headers"";
						}
						else
						{
							objSpan.innerHTML = Show;
							objDiv.className = ""wm_hide"";
						}
					}
					function AddContact()
					{
						HFAction = document.getElementById(""HFAction"");
						HFRequest = document.getElementById(""HFRequest"");
						HFValue = document.getElementById(""HFValue"");
						
						HFAction.value = """";
						HFRequest.value = """";
						HFValue.value = """";

						var obj = GetEmailParts(HtmlDecode(document.getElementById(""newViewMessage_LabelFrom"").innerHTML));

						HFAction.value = ""new"";
						HFRequest.value = ""contact"";
						HFValue.value = obj.Email + ""@#%"" + obj.Name;
						
						__doPostBack('PostBackButton','');
					}

					function DoReplyButton()
					{
						HFAction = document.getElementById(""HFAction"");
						
						HFAction.value = """";
						
						HFAction.value = ""reply"";
						
						__doPostBack('PostBackButton','');
					}

					function DoReplyAllButton()
					{
						HFAction = document.getElementById(""HFAction"");
						
						HFAction.value = """";
						
						HFAction.value = ""replyall"";
						
						__doPostBack('PostBackButton','');
					}

					function DoForwardButton()
					{
						HFAction = document.getElementById(""HFAction"");
						
						HFAction.value = """";
						
						HFAction.value = ""forward"";
						
						__doPostBack('PostBackButton','');
					}

				function ChangeCharset()
				{
					HFValue = document.getElementById(""HFValue"");
					DDLCharsets = document.getElementById(""newViewMessage_DropDownListCharsets"");
					
					var tempArr = HFValue.value.split(""----"");

					HFValue.value = tempArr[0] + ""----"" + tempArr[1] + ""----"" + tempArr[2] + ""----"" + tempArr[3] + ""----"" + DDLCharsets.value;
					
					__doPostBack('PostBackButton','');
				}

				function DoDeleteMessages()
				{
					HFAction = document.getElementById(""HFAction"");
					HFRequest = document.getElementById(""HFRequest"");
					HFValue = document.getElementById(""HFValue"");
					
					HFAction.value = """";
					HFRequest.value = """";

					HFAction.value = ""delete"";
					HFRequest.value = ""message"";

					__doPostBack('PostBackButton','');

				}

function CFullHeadersViewer()
{
	this._headersCont = document.getElementById('headersCont');
	this._headersDiv = document.getElementById('newViewMessage_headersDiv');
	this._control = document.getElementById('fullheadersControl');
	this._isShow = false;
}

CFullHeadersViewer.prototype = 
{
	Show: function()
	{
		if (!this._isShow)
		{
			var height = GetHeight();
            var width = GetWidth();
            
			var win_height = height*3/5;
			var win_width = width*3/5;		
			
			this._control.innerHTML = Lang.HideFullHeaders;
			this._headersCont.className = 'wm_headers';

			this._headersCont.style.width = win_width + 'px';
			this._headersCont.style.height = win_height + 'px';
			this._headersCont.style.top = (height - win_height)/2 + 'px';
			this._headersCont.style.left = (width - win_width)/2 + 'px';
			this._headersDiv.style.width = win_width - 10 + 'px';
			this._headersDiv.style.height = win_height - 30 + 'px';		
		
			var obj = this;
			this._control.onclick = function()
			{
				obj.Hide();
			}
			this._isShow = true;
		}
		return false;
	},
	
	Hide: function()
	{
		this._isShow = false;
		var obj = this;
		this._control.onclick = function()
		{
			obj.Show();
		}
		this._headersCont.className = 'wm_hide';
		this._control.innerHTML = Lang.ShowFullHeaders;
		return false;
	}
}
				";

				_js.AddText(text);

				_js.AddFile("classic/class_maillist_resizer.js");

				_js.AddText("var logo, accountlist, toolbar, FullHeaders, mainContainer, resizerObj;");
				_js.AddText("var m_headers, m_attachments, m_message_body, m_lowtoolbar, m_message, resDiv;");

				text = @"
					PopupMenu.addItem(document.getElementById('popup_menu_4'), document.getElementById('popup_control_4'), 'wm_popup_menu', document.getElementById('popup_replace_4'), document.getElementById('popup_title_4'), 'wm_tb', 'wm_tb_press', 'wm_toolbar_item', 'wm_toolbar_item_over');
					logo = document.getElementById(""logo"");
					accountlist = document.getElementById(""accountslist"");
					toolbar = document.getElementById(""toolbar"");
					FullHeaders = new CFullHeadersViewer();
					mainContainer = document.getElementById(""wm_mail_container"");
					m_headers = document.getElementById(""message_headers"");
					m_attachments = document.getElementById(""attachments"");
					m_message_body = document.getElementById(""message_body"");
					m_lowtoolbar = document.getElementById(""lowtoolbar"");
					m_message = document.getElementById(""message"");	
					resDiv = document.getElementById(""resizerDiv"");

					if (messageWithAttach)
					{
						resizerObj = new CVerticalResizer(resDiv, m_message, 2, 10, 40, 140, ""ResizeElements('all');"", 1);
					}
					else
					{
						m_attachments.className = 'wm_hide';
						resDiv.className = 'wm_hide';
					}
				";

				_js.AddInitText(text);

				Load_Message(acct);

				LoadData();
			}
		}

		protected void LoadData()
		{
			if ((object)ViewState["LoadHTMLMessage"] == null)
			{
				ViewState["LoadHTMLMessage"] = true;
			}

			LoadHTMLMessage = (bool)ViewState["LoadHTMLMessage"];
		}

		protected void SaveData()
		{
			ViewState["LoadHTMLMessage"] = LoadHTMLMessage;
		}

		public void Load_Message(Account acct)
		{
			try
			{
				//Reser values
				IsHTMLMsg = false;
				msgPlain = null;
				msgHTML = null;

				//Load Message
				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				msg = bwml.GetMessage(_id_msg, _uid, _id_folder, _folder_full_name, _charset, 1);

                MemoryStream memoryStream = new MemoryStream();
                System.Xml.XmlTextWriter xmlWriter = new System.Xml.XmlTextWriter(memoryStream, System.Text.Encoding.UTF8);
                msg.MailBeeMessage.Serialize(xmlWriter);
                //memoryStream = (MemoryStream)xmlWriter.BaseStream;

                //Folder _folder = bwml.GetFolder(_id_folder);

				MessageDownloadLink = Utils.GetMessageDownloadLink(msg, _id_folder, _folder_full_name);

				string text = @"
					function DoSaveButton()
					{
						document.location = """ + MessageDownloadLink + @""";
					}
				";

				js.AddText(text);

                msgPlain = msg.MailBeeMessage.BodyPlainText;
				msgHTML = msg.MailBeeMessage.BodyHtmlText;

                if (msgPlain.Trim() == "")
				{
                    msgPlain = Utils.ConvertHtmlToPlain(msgHTML);
				}
                msgPlain = Utils.MakeHtmlBodyFromPlainBody(msgPlain);

                if (msgHTML.Trim() != "")
				{
					LinkButtonSwitchTo.Visible = true;
					LinkButtonSwitchTo.Text = _manager.GetString("SwitchToPlain");

					//Message body
					LiteralMessageBody.Text = msgHTML;
				}
				else
				{
					LinkButtonSwitchTo.Visible = true;

					IsHTMLMsg = false;

					//Message body
					LiteralMessageBody.Text = msgPlain;
				}

				//Message headers
				LabelFrom.Text = Server.HtmlEncode(msg.FromMsg.ToString());
				LabelTo.Text = Server.HtmlEncode(msg.ToMsg.ToString());
				LabelDate.Text = Utils.GetDateWithOffsetFormatString(acct, msg.MsgDate);		
				LabelSubject.Text = Server.HtmlEncode(msg.Subject);

				if(msg.MailBeeMessage.Charset == msg.OverrideCharset.ToString())
				{
					CharsetList.Visible = false;
				}
				else
				{
					CharsetList.Visible = true;
	
					DropDownListCharsets.Items.Clear();

					for(int i = 0; Constants.Charsets.Length > i; i++)
					{
						ListItem CharsetsItem = new ListItem();

						CharsetsItem.Text = _manager.GetString(Constants.Charsets[i]);
						CharsetsItem.Value = Constants.PageNumber[i].ToString();

						DropDownListCharsets.Items.Add(CharsetsItem);
					}

					DropDownListCharsets.SelectedValue = msg.OverrideCharset.ToString();
				}

				headersDiv.InnerHtml = @"<pre>" + Utils.EncodeHtml(msg.MailBeeMessage.RawHeader) + @"</pre>";

				if(msg.Attachments)
				{

					_js.AddText("var messageWithAttach = true;");
					HTMLAttachments = "";

					Attachment = msg.MailBeeMessage.Attachments;

					for(int i = 0; Attachment.Count > i; i++)
					{
						HTMLViewLink = "";

						DownloadLink = Utils.GetAttachmentDownloadLink(Attachment[i], false);
						IconLink = Utils.GetAttachmentIconLink(Attachment[i]);

						switch(Attachment[i].ContentType.ToLower(CultureInfo.InvariantCulture))
						{
							case "image/bmp":
							case "image/gif":
							case "image/jpg":
							case "image/jpeg":
							case "image/tif":
							case "image/tiff":
								ViewLink = Utils.GetAttachmentDownloadLink(Attachment[i], true);
								HTMLViewLink = @"<br/><a class=""wm_attach_view_a"" href=""javascript:void(0)"" onclick=""javascript:PrevImg('" + ViewLink + @"')"">" + _manager.GetString("View") + @"</a>";
								break;
						}

						HTMLAttachments += @"
						<div class=""wm_message_attachments"" id=""attachments"">
							<div style=""float: left;"">
								<a class=""wm_attach_download_a"" href=""" + DownloadLink + @""">
									<img title=""" + _manager.GetString("ClickToDownload") + @" " + Attachment[i].Filename + @" (" + Utils.GetFriendlySize(Attachment[i].Size) + @")"" src=""" + IconLink + @""" />
									<br/>
									<span title=""" + _manager.GetString("ClickToDownload") + @" " + Attachment[i].Filename + @" (" + Utils.GetFriendlySize(Attachment[i].Size) + @")"">" + Utils.GetShortFilename(Attachment[i].Filename) + @"</span>
								</a>"
							+ HTMLViewLink
							+ @"</div>
						</div>
					";

					}

					LiteralAttachments.Text = HTMLAttachments;
				}
				else
				{
					_js.AddText("var messageWithAttach = false");
					LiteralAttachments.Text = "";
				}
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		private void LinkButtonSwitchTo_Click(object sender, EventArgs e)
		{
			if(LoadHTMLMessage)
			{
				LiteralMessageBody.Text = msgPlain;
				LinkButtonSwitchTo.Text = _manager.GetString("SwitchToHTML");

				LoadHTMLMessage = false;
			}
			else
			{
				LiteralMessageBody.Text = msgHTML;
				LinkButtonSwitchTo.Text = _manager.GetString("SwitchToPlain");

				LoadHTMLMessage = true;
			}
			
			SaveData();
		}

		public void ChangeCharset(object sender, System.EventArgs e)
		{
			_charset = Convert.ToInt32(DropDownListCharsets.SelectedValue);
			Load_Message(acct);
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
			LinkButtonSwitchTo.Click +=new EventHandler(LinkButtonSwitchTo_Click);
		}
		#endregion
	}
}