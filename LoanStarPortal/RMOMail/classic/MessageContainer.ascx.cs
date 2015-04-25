namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Globalization;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.IO;
	using WebMailPro;

	/// <summary>
	///		Summary description for MessageContainer.
	/// </summary>
	public partial class MessageContainer : System.Web.UI.UserControl
	{
		protected WebmailResourceManager _manager = null;
		protected WebMailMessage msg = null;
		protected MailBee.Mime.AttachmentCollection Attachment = null;
		Account acct = null;

		protected bool _isPreviewPane = false;
		protected int _id_msg = 0;
		protected string _uid = null;
		protected long _id_folder = 0;
		protected string _folder_full_name = null;
		protected int _charset = 0;
		protected bool IsHTMLMsg = false;
		protected string msgPlain = null;
		protected string msgHTML = null;
		protected bool flag = false;
		protected string ViewLink = null;
		protected string DownloadLink = null;
		protected string IconLink = null;
		protected string HTMLAttachments = null;
		protected string HTMLViewLink = null;
		protected string LinkButtonSwitchToStr = null;
		protected string LinkButtonSwitchToJs = null;
		protected bool aaFlag = false;
		protected bool atFlag = false;
		protected bool showPicturesSettings = false;
		protected string _eisTextClass = String.Empty;

		protected System.Web.UI.WebControls.Literal mData;

		public bool isPreviewPane
		{
			get
			{
				return _isPreviewPane;
			}
			set
			{
				_isPreviewPane = value;
			}
		}

		public int id_msg
		{
			get
			{
				return _id_msg;
			}
			set
			{
				_id_msg = value;
			}
		}

		public string uid
		{
			get
			{
				return _uid;
			}
			set
			{
				_uid = value;
			}
		}

		public long id_folder
		{
			get
			{
				return _id_folder;
			}
			set
			{
				_id_folder = value;
			}
		}

		public string folder_full_name
		{
			get
			{
				return _folder_full_name;
			}
			set
			{
				_folder_full_name = value;
			}
		}

		public int charset
		{
			get
			{
				return _charset;
			}
			set
			{
				_charset = value;
			}
		}

		protected jsbuilder _js;
		public jsbuilder js
		{
			get { return _js; }
			set { _js = value; }
		}

		protected string _skin = Constants.defaultSkinName;
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
				if ((acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null)) {
					showPicturesSettings = ((acct.UserOfAccount.Settings.ViewMode & ViewMode.AlwaysShowPictures) > 0) ? true : false;
				}
				_manager = (new WebmailResourceManagerCreator()).CreateResourceManager();
				if (!showPicturesSettings) {
					eisShowOne.Text = _manager.GetString("ShowPictures")+".";
					eisShowOne.Attributes["href"] = "#AcceptThis";
					eisShowOne.Attributes["onclick"] = "javascript: eisAcceptThis()";
					eisShowAll.Text = _manager.GetString("ShowPicturesFromSender")+".";
					eisShowAll.Attributes["href"] = "#AcceptSender";
					eisShowAll.Attributes["onclick"] = "javascript: eisAcceptSender()";
					eisPanel.Visible = false;
				}
				if(!IsPostBack)
				{
					Data_Bind(acct, 0);
				}
				else
				{
				}
				LoadData();
			}
			js.AddText(@"
					function AddContact()
					{
						HFAction = document.getElementById(""HFAction"");
						HFRequest = document.getElementById(""HFRequest"");
						HFValue = document.getElementById(""HFValue"");
						
						HFAction.value = """";
						HFRequest.value = """";
						HFValue.value = """";

						var obj = GetEmailParts(HtmlDecode(document.getElementById(""DefaultID__Control_Messagecontainer_LabelFrom"").innerHTML));

						HFAction.value = ""new"";
						HFRequest.value = ""contact"";
						HFValue.value = obj.Email + ""@#%"" + obj.Name;
						
						__doPostBack('PostBackButton','');
					}
");
		}

		public void AcceptOne()
		{
			eisPanel.Visible = true;
			_eisTextClass = "wm_hide";
			eisShowOne.Visible = false;
			atFlag = true;
		}
		public void AcceptAll()
		{
			eisPanel.Visible = false;
			aaFlag = true;
		}

		//0 - default
		//1 - html
		//2 - plain
		public void Data_Bind(Account acct, int mode)
		{
			StringBuilder sb = new StringBuilder();
			try
			{
				//Reser values
				IsHTMLMsg = false;
				msgPlain = null;
				msgHTML = null;
				
				if(_isPreviewPane)
				{
					ShowHideElements(true);
					BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);
					if (showPicturesSettings) {
						msg = bwml.GetMessage(_id_msg, _uid, _id_folder, _folder_full_name, _charset, 1);
					}
					else {
						if (atFlag||aaFlag) {
							msg = bwml.GetMessage(_id_msg, _uid, _id_folder, _folder_full_name, _charset, 1);
							atFlag = false;
						}
						else msg = bwml.GetMessage(_id_msg, _uid, _id_folder, _folder_full_name, _charset);
						if (aaFlag) {
							acct.UserOfAccount.SetSenderSafety(msg.FromMsg.Email,1);
							aaFlag = false;
							msg.Safety = 1;
						}
					}
					msgPlain = msg.MailBeeMessage.BodyPlainText;
					msgHTML = msg.MailBeeMessage.BodyHtmlText;

                    if (msgPlain.Trim() == "")
					{
						msgPlain = Utils.ConvertHtmlToPlain(msgHTML);
					}
                    msgPlain = Utils.MakeHtmlBodyFromPlainBody(msgPlain);

					LinkButtonSwitchToStr = String.Empty;
					LinkButtonSwitchToJs = String.Empty;
					switch(mode)
					{
						case 0:
							if(msgHTML != "")
							{
								IsHTMLMsg = true;

								if (msg.Safety==0)
								{
									eisPanel.Visible = true;
									eisShowOne.Visible = true;
								}

								//Message headers
								SwitchTo.Attributes.Add("class", "wm_message_right");
								LinkButtonSwitchToStr = _manager.GetString("SwitchToPlain");
								LinkButtonSwitchToJs = "javascript:SwitchTo(2)";
								
								//Message body
								sb.AppendFormat(@"var messageDIV = document.getElementById(""{0}"");","message_center");
								sb.Append(@"if (messageDIV) {");
								sb.AppendFormat(@"messageDIV.innerHTML = ""{0}"";",Utils.EncodeJsSaveString(msgHTML));
								sb.Append(@"}");
								js.AddInitText(sb.ToString());
								//Component_MessageBody.Text= msgHTML;
							}
							else
							{
								IsHTMLMsg = false;

								//Message headers
								SwitchTo.Attributes.Add("class", "wm_hide");
								//Message body
								Component_MessageBody.Text = msgPlain;
							}
							break;
						case 1:
							LinkButtonSwitchToStr = _manager.GetString("SwitchToPlain");
							LinkButtonSwitchToJs = "javascript:SwitchTo(2)";

							if (msg.Safety==0)
							{
								eisPanel.Visible = true;
								eisShowOne.Visible = true;
							}

							//Message body
							sb.AppendFormat(@"var messageDIV = document.getElementById(""{0}"");","message_center");
							sb.Append(@"if (messageDIV) {");
							sb.AppendFormat(@"messageDIV.innerHTML = ""{0}"";",Utils.EncodeJsSaveString(msgHTML));
							sb.Append(@"}");
							js.AddInitText(sb.ToString());
							//Component_MessageBody.Text = msgHTML;
							break;
						case 2:
							//Message headers
							LinkButtonSwitchToStr = _manager.GetString("SwitchToHTML");
							LinkButtonSwitchToJs = "javascript:SwitchTo(1)";
							//Message body
							Component_MessageBody.Text = msgPlain;
							break;
					}

					//Message headers
                    LabelFrom.Text = "<font>" + _manager.GetString("From") + ":</font> " + Server.HtmlEncode(msg.FromMsg.ToString());
                    LabelTo.Text = "<font>" + _manager.GetString("To") + ":</font> " + Server.HtmlEncode(msg.ToMsg.ToString());
                    LabelDate.Text = "<font>" + _manager.GetString("Date") + ":</font> " + Utils.GetDateWithOffsetFormatString(acct, msg.MsgDate);
					
					if(msg.CcMsg.ToString() != "")
					{
                        LabelCC.Text = "<font>" + _manager.GetString("CC") + ":</font> " + Server.HtmlEncode(msg.CcMsg.ToString());
					}
					else
					{
						CC.Attributes.Add("class", "wm_hide");
					}

					if(msg.BccMsg.ToString() != "")
					{
                        LabelBCC.Text = "<font>" + _manager.GetString("BCC") + ":</font> " + Server.HtmlEncode(msg.BccMsg.ToString());
					}
					else
					{
						BCC.Attributes.Add("class", "wm_hide");
					}
					
					LabelSubject.Text = "<font>" + _manager.GetString("Subject") + ":</font> " +  Server.HtmlEncode(msg.Subject);

					if(msg.MailBeeMessage.Charset == msg.OverrideCharset.ToString())
					{
						CharsetList.Attributes.Add("class", "wm_hide");
					}
					else
					{
						CharsetList.Attributes.Add("class", "wm_message_right");

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

					if(msg.Attachments)
					{
						HTMLAttachments = "";

						Attachment = msg.MailBeeMessage.Attachments;
					
						_js.AddText("var messageWithAttach = true;");
						

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
									HTMLViewLink = "<br/><a class=\"wm_attach_view_a\" href=\"javascript:void(0)\" onclick=\"javascript:PrevImg('" + ViewLink + "')\">" + _manager.GetString("View") + "</a>";
									break;
							}

							HTMLAttachments += "<div style=\"float: left;\">" +
								"<a class=\"wm_attach_download_a\" href=\"" + DownloadLink + "\">" +
								"<img title=\"" + _manager.GetString("ClickToDownload") + " " + Attachment[i].Filename + " (" + Utils.GetFriendlySize(Attachment[i].Size) + ")" + "\" src=\"" + IconLink + "\" />" +
								"<br/>" +
								"<span title=\" " + _manager.GetString("ClickToDownload") + " " + Attachment[i].Filename + " (" + Utils.GetFriendlySize(Attachment[i].Size) + ")" + "\">" + Utils.GetShortFilename(Attachment[i].Filename) + "</span>" +
								"</a>" +
								HTMLViewLink +
								"</div>";

						}

						HTMLAttachments += "<div style=\"clear: left; height: 1px;\"></div>";

						LiteralMessageAttachments.Text = HTMLAttachments;
					}
					else
					{
						_js.AddText("var messageWithAttach = false;");
					}
				}
				else
				{
					ShowHideElements(false);
				}

				SaveData();
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void LoadData()
		{
			if (ViewState["IsHTMLMsg"] == null)
			{
				ViewState["IsHTMLMsg"] = false;
				ViewState["msgPlain"] = null;
				ViewState["msgHTML"] = null;
				ViewState["flag"] = false;
			}

			IsHTMLMsg = (bool)ViewState["IsHTMLMsg"];
			msgPlain = (string)ViewState["msgPlain"];
			msgHTML = (string)ViewState["msgHTML"];
			flag = (bool)ViewState["flag"];
		}

		protected void SaveData()
		{
			ViewState["IsHTMLMsg"] = IsHTMLMsg;
			ViewState["msgPlain"] = msgPlain;
			ViewState["msgHTML"] = msgHTML;
			ViewState["flag"] = flag;
		}

		public void ShowHideElements(bool visible)
		{
			//Message headers
			LabelFrom.Visible = visible;
			LabelTo.Visible = visible;
			LabelDate.Visible = visible;
			if(visible)
			{
				SwitchTo.Attributes.Add("class", "wm_message_right");
				CC.Attributes.Add("class", "");
				BCC.Attributes.Add("class", "");
				CharsetList.Attributes.Add("class", "wm_message_right");
			}
			else
			{
				SwitchTo.Attributes.Add("class", "wm_hide");
				CC.Attributes.Add("class", "wm_hide");
				BCC.Attributes.Add("class", "wm_hide");
				CharsetList.Attributes.Add("class", "wm_hide");
			}
			LabelSubject.Visible = visible;

			//Message body
			Component_MessageBody.Visible = visible;
		}

		public void ChangeCharset(object sender, System.EventArgs e)
		{
			_charset = Convert.ToInt32(DropDownListCharsets.SelectedValue);
			Data_Bind(acct, 0);
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
	}
}
