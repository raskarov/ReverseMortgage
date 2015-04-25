namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for SettingsEmailAccountsSignature.
	/// </summary>
	public partial class SettingsEmailAccountsSignature : System.Web.UI.UserControl
	{

		protected WebmailResourceManager _resMan = null;
		protected string _skin = Constants.defaultSkinName;
		protected Account _acct = null;

		protected jsbuilder _js;

		public string Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}

		public Account EditAccount
		{
			get { return _acct; }
			set { _acct = value; }
		}

		public jsbuilder js
		{
			get { return _js; }
			set { _js = value; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
		}

		protected void Pre_Render(object sender, System.EventArgs e)
		{
			
			InitControls();
			
			if (_acct.UserOfAccount.Settings.AllowDhtmlEditor) 
			{
				string body = _acct.Signature;
				body = body.Replace("\"", @"\""");
				body = body.Replace("\n", @"\n");
				body = body.Replace("\r", @"\r");
				string jsSignType = (_acct.SignatureType == SignatureType.Html) ? "1" : "0";

				_js.AddFile("class.html-editor.js");
				_js.AddText(@"
function saveSignature() 
{
	if (IsDemo() == true) return false;

	var plainEditor = document.getElementById(""settingsID_settingsEmailAccountsID_settingsEmailAccountsSignatureID_editor_area"");
	var hidekey = document.getElementById(""settingsID_settingsEmailAccountsID_settingsEmailAccountsSignatureID_isHtml"");
	if (HTMLEditor._htmlMode) {
		plainEditor.value = HTMLEditor.GetText();
		hidekey.value = ""1"";
	} else {
		hidekey.value = ""0"";
	}
	return true;
}

function EditAreaLoadHandler() { HTMLEditor.LoadEditArea();	}
function CreateLinkHandler(url) { HTMLEditor.CreateLinkFromWindow(url); }
function DesignModeOnHandler(rer)
{
	HTMLEditor.Show();
	var sign = """ + body + @""";
	var signType = " + jsSignType + @";
	if (signType == 0) {
		HTMLEditor.SetText(sign);
	} else {
		if (sign.length == 0) sign = ""<br/>"";
		HTMLEditor.SetHtml(sign);
	}
}
");

				_js.AddInitText(@"
EditAreaUrl = 'edit-area.aspx';
HTMLEditor = new CHtmlEditorField(true);
HTMLEditor.SetPlainEditor(document.getElementById(""settingsID_settingsEmailAccountsID_settingsEmailAccountsSignatureID_editor_area""), document.getElementById(""editor_switcher""));
HTMLEditor.Show();
HTMLEditor.Resize(684, 330);
");
			}
			else
			{
				_js.AddText(@"function saveSignature() {}");
			}

		}

		private void InitControls()
		{
			saveButton.Value = _resMan.GetString("Save");
			add_signatures.Checked = (_acct.SignatureOptions == SignatureOptions.AddSignatureToAllOutgoingMessages || 
				_acct.SignatureOptions == SignatureOptions.DontAddSignatureToRepliesAndForwards);
			replies_forwards.Checked = (_acct.SignatureOptions == SignatureOptions.DontAddSignatureToRepliesAndForwards);
			if (!add_signatures.Checked)
			{
				replies_forwards.Checked = false;
				replies_forwards.Disabled = true;
			}
			editor_area.Value = (_acct.UserOfAccount.Settings.AllowDhtmlEditor) 
				? _acct.Signature : Utils.ConvertHtmlToPlain(_acct.Signature);

			isHtml.Value = (_acct.SignatureType == SignatureType.Html) ? "1" : "0";
		}

		public string SwitcherCode()
		{
			return (_acct.UserOfAccount.Settings.AllowDhtmlEditor)
				? @"<a class=""wm_reg"" href=""#"" id=""editor_switcher"">" + _resMan.GetString("SwitchToPlainMode") + @"</a>" :"";
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
			this.PreRender += new System.EventHandler(this.Pre_Render);
		}
		#endregion

		protected void saveButton_ServerClick(object sender, System.EventArgs e)
		{
			if (_acct != null)
			{
				try
				{
					if (add_signatures.Checked && !replies_forwards.Checked)
					{
						_acct.SignatureOptions = SignatureOptions.AddSignatureToAllOutgoingMessages;
					}
					else if (!replies_forwards.Disabled && replies_forwards.Checked && add_signatures.Checked)
					{
						_acct.SignatureOptions = SignatureOptions.DontAddSignatureToRepliesAndForwards;
					} 
					else
					{
						_acct.SignatureOptions = SignatureOptions.DontAddSignature;
					}

					_acct.SignatureType = (int.Parse(isHtml.Value) == 1)
						? SignatureType.Html : SignatureType.Plain;
					_acct.Signature = (_acct.SignatureType == SignatureType.Plain)
						? Utils.ConvertHtmlToPlain(Server.HtmlDecode(editor_area.Value)) : Server.HtmlDecode(editor_area.Value);
					_acct.Update(false);

					Session[Constants.sessionReportText] = _resMan.GetString("ReportSignatureUpdatedSuccessfuly");
				
					Account acct = Session[Constants.sessionAccount] as Account;
					if (acct != null)
					{
						if (acct.ID == _acct.ID) Session[Constants.sessionAccount] = _acct;
					}
				}
				catch (Exception ex)
				{
					((basewebmail)this.Page).OutputException(ex);
				}
			}
		}

	}
}
