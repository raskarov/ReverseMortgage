namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for AccountsList.
	/// </summary>
	public partial class AccountsList : System.Web.UI.UserControl
	{
		protected string _skin = Constants.defaultSkinName;
		protected WebmailResourceManager _manager = null;
		protected string AcctList = String.Empty;

		public string Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}

		private string _webMailMode;

		private string _linlAccountsList = "basewebmail.aspx?scr=default";
        protected string _settingsLink = "basewebmail.aspx?scr=settings_common";
        protected string _contactsLink = "basewebmail.aspx?scr=contacts";
		protected string _logoutLink = "default.aspx?mode=logout";
		protected string _changeAccountAction = "ChangeAccount(";

		protected string _ContactsDiv = String.Empty;
		protected string _CalendarDiv = String.Empty;

        public string WebMailMode
        {
            get { return _webMailMode; }
            set
            {
                _webMailMode = value;
                switch (value)
                { 
                    case "ajax":
						_linlAccountsList = "#\" onclick=\"parent.HideCalendar('account'); return false;";
                        _settingsLink = "#\" onclick=\"parent.HideCalendar('settings'); return false;";
                        _contactsLink = "#\" onclick=\"parent.HideCalendar('contacts'); return false;";
						_logoutLink = "#\" onclick=\"parent.HideCalendar('logout'); return false;";
						_changeAccountAction = "parent.HideCalendar('account', ";
                        break;
                    case "base":
                    default:
                        _linlAccountsList = "basewebmail.aspx?scr=default";
                        _settingsLink = "basewebmail.aspx?scr=settings_common";
                        _contactsLink = "basewebmail.aspx?scr=contacts";
						_logoutLink = "default.aspx";
						_changeAccountAction = "ChangeAccount(";
                        _CalendarDiv = string.Empty;
                        break;
                }
            }
        }

		protected void Page_Load(object sender, System.EventArgs e)
		{
			Account acct = (Account)Session[Constants.sessionAccount];
			Skin = acct.UserOfAccount.Settings.DefaultSkin;
            
			if (acct == null)
			{
				Response.Redirect("default.aspx");
			}
			else
			{
				try
				{
					_manager = (new WebmailResourceManagerCreator()).CreateResourceManager();

                    if (_webMailMode == "ajax")
                    {
                        _CalendarDiv = @"<div class=""wm_accountslist_contacts"">
											<a href=""#"" onclick=""return false;"">" + _manager.GetString("Calendar") + @"</a>
										</div>";
					}
					WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
					if (settings.AllowContacts)
					{
						_ContactsDiv = @"<div class=""wm_accountslist_contacts"">
											<a href=""" + _contactsLink + @""">" + _manager.GetString("Contacts") + @"</a>
										</div>";
					}

					BaseWebMailActions bwa = new BaseWebMailActions(acct, this.Page);

					Account[] Accounts = bwa.GetAccounts(acct.IDUser);

					if(Accounts.Length > 1)
					{
						AcctList = @"
						<span id=""popup_replace_1"" class=""wm_accountslist_email"">
							<a href="""+_linlAccountsList+@""">
								" + acct.Email + @"
							</a>
						</span>

						<span class=""wm_accountslist_selection"">
							<img onmouseout=""this.src='skins/" + Skin + @"/menu/accounts_arrow.gif'"" onmouseover=""this.src='skins/" + Skin + @"/menu/accounts_arrow_over.gif'"" onmouseup=""this.src='skins/" + Skin + @"/menu/accounts_arrow_over.gif'"" onmousedown=""this.src='skins/" + Skin + @"/menu/accounts_arrow_down.gif'"" src=""skins/" + Skin + @"/menu/accounts_arrow.gif"" id=""popup_control_1"" class=""wm_accounts_arrow""/>
						</span>
					";

						string temp = String.Empty;

						for(int i = 0; Accounts.Length > i; i++)
						{
							if(Accounts[i].ID != acct.ID)
							{
								temp += @"
							<div id=""" + Accounts[i].ID + @""" onmouseout=""this.className='wm_account_item';"" onmouseover=""this.className='wm_account_item_over';"" onclick=""javascript:" + _changeAccountAction + @"this.id);"" class=""wm_account_item"">
								" + Accounts[i].Email + @"
							</div>
						";
							}
						}

						temp = @"
						<div id=""popup_menu_1"" class=""wm_hide"">
							" + temp + @"
						</div>
					";

						AcctList = temp + AcctList;
					}
					else
					{
						AcctList = @"
						<div class=""wm_accountslist_email"">
							<a href=""" + _linlAccountsList + @""">
								" + acct.Email + @"
							</a>
						</div>
						<div class=""wm_accountslist_selection"">
							<img class=""wm_hide"" src=""skins/" + Skin + @"/menu/accounts_arrow.gif"">
						</div>
					";
					}
				}
				catch (Exception ex)
				{
					((basewebmail)this.Page).OutputException(ex);
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
	}
}
