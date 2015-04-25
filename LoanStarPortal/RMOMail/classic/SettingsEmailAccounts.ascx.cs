using System.Text;
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
	///		Summary description for SettingsEmailAccounts.
	/// </summary>
	public partial class SettingsEmailAccounts : System.Web.UI.UserControl
	{
		protected string _skin = Constants.defaultSkinName;
		protected System.Web.UI.HtmlControls.HtmlTable tableAccounts;
		protected WebmailResourceManager _resMan = null;
		private int _selectedTab = 0;
		protected Account _acct = null;
		protected string classForAllTabs = "wm_settings_accounts_info";
		protected jsbuilder _js;
		protected bool _allowChangeEmailSettings = false;
		protected bool _allowAddNewAccount = false;

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
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			_allowAddNewAccount = settings.AllowUsersAddNewAccounts;
			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			InitControls();

			Account acct = Session[Constants.sessionAccount] as Account;
			_acct = acct;
			if (acct != null)
			{
				try
				{
					bool accountPropertiesScr = Request.QueryString["scr"] == Constants.StaticScreenNames.settings_accounts_properties;
					if ((acct != null) && (acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
					{
						_allowChangeEmailSettings = acct.UserOfAccount.Settings.AllowChangeSettings;
						if (!_allowChangeEmailSettings && accountPropertiesScr)
						{
							Response.Redirect(@"basewebmail.aspx?scr=" + Constants.StaticScreenNames.settings_accounts_filters);
							return;
						}
					}
					BaseWebMailActions actions = new BaseWebMailActions(acct, this.Page);

					if (accountPropertiesScr && Request.QueryString["new_acct"] != null)
					{
						acct = null;
						classForAllTabs = "wm_hide";
					}
					else if (accountPropertiesScr && Request.QueryString["delete_acct"] != null)
					{
						int id_acct = int.Parse(Request.QueryString["delete_acct"]);
						actions.DeleteAccount(id_acct);
						if (actions.CurrentAccount == null)
						{
							Response.Redirect("default.aspx");
						}
						if (acct.ID == id_acct)
						{
							Session[Constants.sessionAccount] = acct = actions.CurrentAccount;
						}
					}
					else if (accountPropertiesScr && Request.QueryString["edit_acct"] != null)
					{
						acct = actions.GetAccount(int.Parse(Request.QueryString["edit_acct"]));
						if (acct != null)
						{
							if (Session[Constants.sessionAccount] != null)
							{
								Session[Constants.sessionAccountEdit] = Request.QueryString["edit_acct"];
							}
							else
							{
								Session.Add(Constants.sessionAccountEdit, Request.QueryString["edit_acct"]);
							}
						}
					}
					else
					{
						int id_acct = acct.ID;
						if (Session[Constants.sessionAccountEdit] == null)
						{
							Session.Add(Constants.sessionAccountEdit, acct.ID);
						}
						else
						{
							id_acct = int.Parse(Session[Constants.sessionAccountEdit].ToString());
						}
						acct = actions.GetAccount(id_acct);
					}
					if (acct != null)
					{
						_acct = acct;
					}

					switch (Request.QueryString["scr"])
					{
						case Constants.StaticScreenNames.settings_accounts_properties:
							SettingsEmailAccountsProperties seap = LoadControl(@"SettingsEmailAccountsProperties.ascx") as SettingsEmailAccountsProperties;
							if (seap != null)
							{
								seap.ID = "settingsEmailAccountsPropertiesID";
								seap.Skin = _skin;
								seap.EditAccount = acct;
								seap.js = js;
								contentPlaceHolder.Controls.Add(seap);
							}
							_selectedTab = 0;
							break;
						case Constants.StaticScreenNames.settings_accounts_filters:
							SettingsEmailAccountsFilters seaf = LoadControl(@"SettingsEmailAccountsFilters.ascx") as SettingsEmailAccountsFilters;
							if (seaf != null)
							{
								seaf.ID = "settingsEmailAccountsFiltersID";
								seaf.Skin = _skin;
								seaf.EditAccount = acct;
								seaf.js = js;
								contentPlaceHolder.Controls.Add(seaf);
							}
							_selectedTab = 1;
							break;
						case Constants.StaticScreenNames.settings_accounts_signature:
							SettingsEmailAccountsSignature seas = LoadControl(@"SettingsEmailAccountsSignature.ascx") as SettingsEmailAccountsSignature;
							if (seas != null)
							{
								seas.ID = "settingsEmailAccountsSignatureID";
								seas.Skin = _skin;
								seas.EditAccount = acct;
								seas.js = js;
								contentPlaceHolder.Controls.Add(seas);
							}
							_selectedTab = 2;
							break;
						case Constants.StaticScreenNames.settings_accounts_folders:
							SettingsEmailAccountsManageFolders seamf = LoadControl(@"SettingsEmailAccountsManageFolders.ascx") as SettingsEmailAccountsManageFolders;
							if (seamf != null)
							{
								seamf.ID = "settingsEmailAccountsManageFolderID";
								seamf.Skin = _skin;
								seamf.EditAccount = acct;
								seamf.js = js;
								contentPlaceHolder.Controls.Add(seamf);
							}
							_selectedTab = 3;
							break;
					}
				}
				catch (Exception ex)
				{
					((basewebmail)this.Page).OutputException(ex);
				}
			}

			// Put user code to initialize the page here
		}

		private void InitControls()
		{
			this.buttonNewAccount.Value = _resMan.GetString("AddNewAccount");
		}

		public string GetClassForTab(int tabID)
		{
			if (tabID == _selectedTab)
			{
				return "wm_settings_switcher_select_item";
			}
			return "wm_settings_switcher_item";
		}

		public string OutputAccounts()
		{
			Account curr_acct = _acct;
			StringBuilder sb = new StringBuilder();
			if (curr_acct != null)
			{
				BaseWebMailActions actions = new BaseWebMailActions(curr_acct, this.Page);
				Account[] accts = actions.GetAccounts(curr_acct.IDUser);

				foreach (Account acct in accts)
				{
					string deleteCell = string.Format(@"
		<td style=""width: 10px"">
			<a href=""basewebmail.aspx?scr={0}&delete_acct={1}"" onclick=""if((IsDemo()==true)||(!confirm('{2}'))) return false;"">{3}</a>
		</td>",
						Constants.StaticScreenNames.settings_accounts_properties,//0
						acct.ID,//1
						_resMan.GetString("ConfirmDeleteAccount"),//2
						_resMan.GetString("Delete"));//3

					sb.AppendFormat(@"
	<tr class=""{0}"">
		<td onclick=""document.location='basewebmail.aspx?scr={1}&edit_acct={2}'"">{3}</td>{4}
	</tr>
",
						(acct.ID == curr_acct.ID) ? "wm_settings_list_select" : "wm_control", // 0
						Constants.StaticScreenNames.settings_accounts_properties, // 1
						acct.ID, // 2
						(acct.ID == curr_acct.ID) ? "<b>" + acct.Email + "</b>" : acct.Email, // 3
						_allowChangeEmailSettings ? deleteCell : ""); // 4
				}
			}
			return sb.ToString();
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
