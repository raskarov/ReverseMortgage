using System.Web.UI;

namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for Settings.
	/// </summary>
	public partial class Settings : System.Web.UI.UserControl
	{
		protected string _skin = Constants.defaultSkinName;
		protected WebmailResourceManager _resMan = null;
		protected jsbuilder _js;
		protected bool _allowContacts = true;

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

		public bool IsDemoAccount
		{
			get
			{
				Account acct = Session[Constants.sessionAccount] as Account;
				if (acct != null)
				{
					return acct.IsDemo;
				}
				return false;
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			_allowContacts = settings.AllowContacts;

			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();

			js.AddText("function ResizeElements(mode) {}");
			js.AddText(string.Format(@"
function IsDemo()
{{
	{0}
}}
", (IsDemoAccount) ? @"
alert('This feature is disabled for this demo account because this one is shared between all the users. If you wish to test this feature, please logout and then log in your own e-mail account using ""advanced login"" option.');
return true;" : "return false;"));

			Control ctrl = null;
			switch (Request.QueryString["scr"])
			{
				case Constants.StaticScreenNames.settings_common:
					SettingsCommon common = LoadControl(@"SettingsCommon.ascx") as SettingsCommon;
					if (common != null)
					{
						common.ID = "settingsCommonID";
						common.Skin = _skin;
						common.js = js;
						ctrl = common;
					}
					break;
				case Constants.StaticScreenNames.settings_accounts_properties:
				case Constants.StaticScreenNames.settings_accounts_filters:
				case Constants.StaticScreenNames.settings_accounts_signature:
				case Constants.StaticScreenNames.settings_accounts_folders:
					SettingsEmailAccounts sea = LoadControl(@"SettingsEmailAccounts.ascx") as SettingsEmailAccounts;
					if (sea != null)
					{
						sea.ID = "settingsEmailAccountsID";
						sea.Skin = _skin;
						sea.js = js;
						ctrl = sea;
					}
					break;
				case Constants.StaticScreenNames.settings_contacts:
					SettingsContacts contacts = LoadControl(@"SettingsContacts.ascx") as SettingsContacts;
					if (contacts != null)
					{
						contacts.ID = "settingsContactsID";
						contacts.Skin = _skin;
						contacts.js = js;
						ctrl = contacts;
					}
					break;
			}
			if (ctrl != null)
			{
				contentPlaceHolder.Controls.Add(ctrl);
			}

			// Put user code to initialize the page here
			DataBind();
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

		protected string GetMenuClass(int menuID)
		{
			string result = string.Empty;
			switch (Request.QueryString["scr"])
			{
				case Constants.StaticScreenNames.settings_common:
					if (menuID == 0)
					{
						result = "wm_selected_settings_item";
					}
					break;
				case Constants.StaticScreenNames.settings_accounts_properties:
				case Constants.StaticScreenNames.settings_accounts_filters:
				case Constants.StaticScreenNames.settings_accounts_signature:
				case Constants.StaticScreenNames.settings_accounts_folders:
					if (menuID == 1)
					{
						result = "wm_selected_settings_item";
					}
					break;
				case Constants.StaticScreenNames.settings_contacts:
					if (menuID == 2)
					{
						result = "wm_selected_settings_item";
					}
					break;
				default:
					result = string.Empty;
					break;
			}
			return result;
		}
	}
}
