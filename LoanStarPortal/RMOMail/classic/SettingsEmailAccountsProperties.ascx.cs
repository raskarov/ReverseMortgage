using System.Globalization;

namespace WebMailPro.classic
{
	using System;
	using System.Web.UI.WebControls;

	/// <summary>
	///		Summary description for SettingsEmailAccountsProperties.
	/// </summary>
	public partial class SettingsEmailAccountsProperties : System.Web.UI.UserControl
	{
		protected string _skin = Constants.defaultSkinName;
		protected WebmailResourceManager _resMan = null;
        protected WebmailSettings settings = null;
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

		protected void Page_Load(object sender, EventArgs e)
		{
			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
            settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

			js.AddFile("classic/class_accountedit.js");
			js.AddInitText(@"
newAccountForm = new CNewAccountForm();
newAccountForm.ShowPOP3AdvancedOptions();
");

            if (!Page.IsPostBack)
            {
                InitControls();
            }
            
			// Put user code to initialize the page here
		}

		private void InitControls()
		{
			comboBoxInboxSyncType.Items.Clear();
			comboBoxInboxSyncType.Items.Add(new ListItem(_resMan.GetString("Pop3SyncTypeEntireHeaders"), "1"));
			comboBoxInboxSyncType.Items.Add(new ListItem(_resMan.GetString("Pop3SyncTypeEntireMessages"), "3"));
			comboBoxInboxSyncType.Items.Add(new ListItem(_resMan.GetString("Pop3SyncTypeDirectMode"), "5"));

			comboBoxProtocol.Items.Clear();
			comboBoxProtocol.Items.Add(new ListItem(_resMan.GetString("Pop3"), "pop"));
			comboBoxProtocol.Items.Add(new ListItem(_resMan.GetString("Imap4"), "imap"));

			saveButton.Value = _resMan.GetString("Save");
			cancelButton.Value = _resMan.GetString("Cancel");

			if (_acct == null)
			{
                cancelButton.Visible = true;
				spanProtocol.Visible = false;

				textBoxIncPort.Value = "110";
				textBoxOutPort.Value = "25";
				mail_mode_0.Checked = true;
                User _user = ((Account) Session[Constants.sessionAccount]).UserOfAccount;
                if (_user.Settings.AllowDirectMode && settings.DirectModeIsDefault)
                {
                    comboBoxInboxSyncType.SelectedIndex = 2;
                }
                else
                {
                    comboBoxInboxSyncType.SelectedIndex = 1;
                }
				textBoxDays.Value = "1";
			}
			else
			{
                cancelButton.Visible = false;
				comboBoxProtocol.Attributes.Add("class", "wm_hide");

				// ----- set default checkbox -----
				if (_acct.UserOfAccount != null)
				{
					Account[] accounts = _acct.UserOfAccount.GetUserAccounts();
					bool hasDefault = false;
					foreach (Account acct in accounts)
					{
						if (acct.ID != _acct.ID)
						{
							if (acct.DefaultAccount)
							{
								hasDefault = true;
								break;
							}
						}
					}
					if (hasDefault)
					{
						checkBoxDefAcct.Checked = _acct.DefaultAccount;
					}
					else
					{
						checkBoxDefAcct.Checked = true;
						checkBoxDefAcct.Disabled = true;
					}
				}
				else
				{
					checkBoxDefAcct.Checked = _acct.DefaultAccount;
				}
				//--------------------------------

				textBoxName.Value = _acct.FriendlyName;
				textBoxEmail.Value = _acct.Email;
				textBoxIncHost.Value = _acct.MailIncomingHost;
				textBoxIncPort.Value = _acct.MailIncomingPort.ToString(CultureInfo.InvariantCulture);
				textBoxIncLogin.Value = _acct.MailIncomingLogin;
				textBoxIncPassword.Attributes.Add("Value", Constants.nonChangedPassword);
				textBoxOutHost.Value = ((_acct.MailOutgoingHost != null) && (_acct.MailOutgoingHost.Length > 0)) ? _acct.MailOutgoingHost : _acct.MailIncomingHost;
				textBoxOutPort.Value = _acct.MailOutgoingPort.ToString(CultureInfo.InvariantCulture);
				textBoxOutLogin.Value = _acct.MailOutgoingLogin;
				textBoxOutPassword.Attributes.Add("Value", Constants.nonChangedPassword);

				checkBoxOutAuth.Checked = _acct.MailOutgoingAuthentication;
				checkBoxUseFriendlyName.Checked = _acct.UseFriendlyName;
				checkBoxGetMailAtLogin.Checked = _acct.GetMailAtLogin;

				textBoxDays.Value = _acct.MailsOnServerDays.ToString(CultureInfo.InvariantCulture);

                if (_acct.MailMode == MailMode.DeleteMessagesFromServer)
                {
                    mail_mode_0.Checked = true;
                }
                else
                {
                    if (_acct.MailMode == MailMode.LeaveMessagesOnServer)
                    {
                        mail_mode_1.Checked = true;
                    }
                    else
                    {
                        if (_acct.MailMode == MailMode.KeepMessagesOnServer)
                        {
                            mail_mode_1.Checked = true;
                            mail_mode_2.Checked = true;
                            textBoxDays.Disabled = false;
                        }
                        if (_acct.MailMode == MailMode.DeleteMessageWhenItsRemovedFromTrash)
                        {
                            mail_mode_1.Checked = true;
                            mail_mode_3.Checked = true;
                        }
                        if (_acct.MailMode == MailMode.KeepMessagesOnServerAndDeleteMessageWhenItsRemovedFromTrash)
                        {
                            mail_mode_1.Checked = true;
                            mail_mode_2.Checked = true;
                            mail_mode_3.Checked = true;
                            textBoxDays.Disabled = false;
                        }
                    }
                }

				switch (_acct.MailIncomingProtocol)
				{
					case IncomingMailProtocol.Pop3:
						comboBoxProtocol.SelectedIndex = 0;
                        spanProtocol.InnerText = _resMan.GetString("Pop3");
						break;
					case IncomingMailProtocol.Imap4:
						comboBoxProtocol.SelectedIndex = 1;
						spanProtocol.InnerText = _resMan.GetString("Imap4");
						break;
					case IncomingMailProtocol.WMServer:
						comboBoxProtocol.SelectedIndex = 2;
                        spanProtocol.InnerText = "XMail";
						break;
				}

				BaseWebMailActions actions = new BaseWebMailActions(_acct, Page);
				Folder inboxFld = actions.GetFolder(Constants.FolderNames.Inbox);
				if (inboxFld != null)
				{
					if (inboxFld.SyncType == FolderSyncType.AllEntireMessages)
					{
						comboBoxInboxSyncType.SelectedIndex = 1;
						checkBoxDeleteNoExists.Checked = true;
					}
					else if (inboxFld.SyncType == FolderSyncType.AllHeadersOnly)
					{
						comboBoxInboxSyncType.SelectedIndex = 0;
						checkBoxDeleteNoExists.Checked = true;
					}
					else if (inboxFld.SyncType == FolderSyncType.DirectMode)
					{
						comboBoxInboxSyncType.SelectedIndex = 2;
						checkBoxDeleteNoExists.Checked = true;
					}
					else if (inboxFld.SyncType == FolderSyncType.NewEntireMessages)
					{
						comboBoxInboxSyncType.SelectedIndex = 1;
						checkBoxDeleteNoExists.Checked = false;
					}
					else if (inboxFld.SyncType == FolderSyncType.NewHeadersOnly)
					{
						comboBoxInboxSyncType.SelectedIndex = 0;
						checkBoxDeleteNoExists.Checked = false;
					}
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

		protected void saveButton_ServerClick(object sender, EventArgs e)
		{
			try
			{
				if (_acct != null)
				{
					BaseWebMailActions actions = new BaseWebMailActions(_acct, Page);
					MailMode mode = MailMode.LeaveMessagesOnServer;
					if (mail_mode_0.Checked)
					{
						mode = MailMode.DeleteMessagesFromServer;
					}
					else if (mail_mode_1.Checked)
					{
						mode = MailMode.LeaveMessagesOnServer;
                        if (mail_mode_2.Checked)
                        {
                            mode = MailMode.KeepMessagesOnServer;
                        }
                        if (mail_mode_3.Checked)
                        {
                            mode = MailMode.DeleteMessageWhenItsRemovedFromTrash;
                        }
                        if (mail_mode_2.Checked && mail_mode_3.Checked)
                        {
                            mode = MailMode.KeepMessagesOnServerAndDeleteMessageWhenItsRemovedFromTrash;
                        }
					}

					FolderSyncType inboxSyncType = FolderSyncType.DontSync;
					if (comboBoxInboxSyncType.SelectedIndex == 1)
					{
						if (checkBoxDeleteNoExists.Checked)
						{
							inboxSyncType = FolderSyncType.AllEntireMessages;
						}
						else
						{
							inboxSyncType = FolderSyncType.NewEntireMessages;
						}
					}
					else if (comboBoxInboxSyncType.SelectedIndex == 0)
					{
						if (checkBoxDeleteNoExists.Checked)
						{
							inboxSyncType = FolderSyncType.AllHeadersOnly;
						}
						else
						{
							inboxSyncType = FolderSyncType.NewHeadersOnly;
						}
					}
					else if (comboBoxInboxSyncType.SelectedIndex == 2)
					{
						inboxSyncType = FolderSyncType.DirectMode;
					}

					short days;
					try
					{
						days = short.Parse(textBoxDays.Value.Trim());
						if (days<0) days = _acct.MailsOnServerDays;
					}
					catch
					{
						days = _acct.MailsOnServerDays;	
					}
					string saEmail;
					string saINServer;
					int saINPort = 0;
					int saOUTPort = 0;
					string saOUTServer;
					string saLogin;
					string saPassword;
					string saSMTPLogin = String.Empty;
					string saSMTPPassword = String.Empty;
					
					saSMTPLogin = textBoxOutLogin.Value;
					saSMTPPassword = textBoxOutPassword.Text;
					if (Validation.CheckIt(Validation.ValidationTask.Email, textBoxEmail.Value))
					{
						saEmail = Validation.Corrected;
					}
					else
					{
						saEmail = _acct.Email;
						throw (new Exception(Validation.ErrorMessage));
					}
					if (Validation.CheckIt(Validation.ValidationTask.Login, textBoxIncLogin.Value,"1"))
					{
						saLogin = Validation.Corrected;
					}
					else
					{
						saLogin = _acct.MailIncomingLogin;
						throw (new Exception(Validation.ErrorMessage));
					}
					if (Validation.CheckIt(Validation.ValidationTask.Password, textBoxIncPassword.Text,"1"))
					{
						saPassword = Validation.Corrected;
					}
					else
					{
						saPassword = _acct.MailIncomingPassword;
						throw (new Exception(Validation.ErrorMessage));
					}
					if (Validation.CheckIt(Validation.ValidationTask.INServer, textBoxIncHost.Value,"1"))
					{
						saINServer = Validation.Corrected;
					}
					else
					{
						saINServer = _acct.MailIncomingHost;
						throw (new Exception(Validation.ErrorMessage));
					}
					if (Validation.CheckIt(Validation.ValidationTask.INPort, textBoxIncPort.Value,"1"))
					{
						saINPort = int.Parse(Validation.Corrected);
					}
					else
					{
						saINPort = _acct.MailIncomingPort;
						throw (new Exception(Validation.ErrorMessage));
					}
					if (Validation.CheckIt(Validation.ValidationTask.OUTServer, textBoxOutHost.Value,"1"))
					{
						saOUTServer = Validation.Corrected;
					}
					else
					{
						saOUTServer = _acct.MailOutgoingHost;
						throw (new Exception(Validation.ErrorMessage));
					}
					if (Validation.CheckIt(Validation.ValidationTask.OUTPort, textBoxOutPort.Value,"1"))
					{
						saOUTPort = int.Parse(Validation.Corrected);
					}
					else
					{
						saOUTPort = _acct.MailOutgoingPort;
						throw (new Exception(Validation.ErrorMessage));
					}
					actions.UpdateAccount(_acct.ID, checkBoxDefAcct.Checked, _acct.MailIncomingProtocol,
						saINPort, saOUTPort, checkBoxOutAuth.Checked,
						checkBoxUseFriendlyName.Checked, days, mode, checkBoxGetMailAtLogin.Checked,
						(int)inboxSyncType, textBoxName.Value, saEmail, saINServer,
						saLogin, saPassword, saOUTServer,	saSMTPLogin, saSMTPPassword);
					
					Session[Constants.sessionReportText] = _resMan.GetString("ReportAccountUpdatedSuccessfuly");
					Account acct = Session[Constants.sessionAccount] as Account;
					if (acct != null)
					{
						if (acct.ID == actions.CurrentAccount.ID) Session[Constants.sessionAccount] = actions.CurrentAccount;
					}
				}
				else
				{
					Account acct = Session[Constants.sessionAccount] as Account;
					if (acct != null)
					{
						BaseWebMailActions actions = new BaseWebMailActions(acct, Page);
						MailMode mail_mode = MailMode.LeaveMessagesOnServer;
						if (mail_mode_0.Checked)
						{
							mail_mode = MailMode.DeleteMessagesFromServer;
						}
						else if (mail_mode_1.Checked)
						{
							mail_mode = MailMode.LeaveMessagesOnServer;
							if (mail_mode_2.Checked)
							{
								mail_mode = MailMode.KeepMessagesOnServer;
							}
							if (mail_mode_3.Checked)
							{
								mail_mode = MailMode.DeleteMessageWhenItsRemovedFromTrash;
							}
                            if (mail_mode_2.Checked && mail_mode_3.Checked)
                            {
                                mail_mode = MailMode.KeepMessagesOnServerAndDeleteMessageWhenItsRemovedFromTrash;
                            }
						}

						int inbox_sync_type;
						if (comboBoxInboxSyncType.SelectedIndex == 0)
						{
							inbox_sync_type = (checkBoxDeleteNoExists.Checked) ? (int)FolderSyncType.AllHeadersOnly : (int)FolderSyncType.NewHeadersOnly;
						}
						else if (comboBoxInboxSyncType.SelectedIndex == 1)
						{
							inbox_sync_type = (checkBoxDeleteNoExists.Checked) ? (int)FolderSyncType.AllEntireMessages : (int)FolderSyncType.NewEntireMessages;
						}
						else
						{
							inbox_sync_type = (int)FolderSyncType.DirectMode;
						}
						IncomingMailProtocol protocol = IncomingMailProtocol.Pop3;
						switch (comboBoxProtocol.Value.ToLower(CultureInfo.InvariantCulture))
						{
							case "pop":
								protocol = IncomingMailProtocol.Pop3;
								break;
							case "imap":
								protocol = IncomingMailProtocol.Imap4;
								break;
						}
						int acctId = actions.NewAccount(checkBoxDefAcct.Checked, protocol, short.Parse(textBoxIncPort.Value), short.Parse(textBoxOutPort.Value), checkBoxOutAuth.Checked, checkBoxUseFriendlyName.Checked, short.Parse(textBoxDays.Value), mail_mode, checkBoxGetMailAtLogin.Checked, inbox_sync_type, textBoxName.Value, textBoxEmail.Value, textBoxIncHost.Value, textBoxIncLogin.Value, textBoxIncPassword.Text, textBoxOutHost.Value, textBoxOutLogin.Value, textBoxOutPassword.Text);
						Session[Constants.sessionReportText] = _resMan.GetString("ReportAccountCreatedSuccessfuly");
						Response.Redirect("basewebmail.aspx?scr=settings_accounts_properties&edit_acct=" + acctId);
					}
				}
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}
	}
}
