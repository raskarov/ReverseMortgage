using System.Globalization;

namespace WebMailPro.MailAdm
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
    using Calendar_NET;

	public partial class mailadm_user_details : System.Web.UI.UserControl
	{
		protected int _id_acct = -1;
		public int AccountID
		{
			get { return _id_acct; }
			set { _id_acct = value; }
		}
		protected bool _isUpdate = false;
		public bool IsUpdate
		{
			get { return _isUpdate; }
			set { _isUpdate = value; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			// Put user code to initialize the page here
			if (!Page.IsPostBack)
			{
				if (_isUpdate)
				{
					SubmitButton.Text = "Update";
					Account acct = Account.LoadFromDb(_id_acct, -1);
					if (acct != null)
					{
						accountsGrid.Visible = true;
						GridDataBind(acct.IDUser);
						if ((acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
						{
							txtLimitMailbox.Value = acct.UserOfAccount.Settings.MailboxLimit.ToString(CultureInfo.InvariantCulture);
							chkAllowDM.Checked = acct.UserOfAccount.Settings.AllowDirectMode;
							chkAllowChangeEmail.Checked = acct.UserOfAccount.Settings.AllowChangeSettings;
						}
						txtFriendlyName.Value = acct.FriendlyName;
						txtEmail.Value = acct.Email;
						txtIncomingServer.Value = acct.MailIncomingHost;
						
						labelIncomingProtocol.CssClass = "";
						selectProtocol.CssClass = "wm_hide";
						if (acct.MailIncomingProtocol == IncomingMailProtocol.Pop3) 
						{
							labelIncomingProtocol.Text = "POP3";
							selectProtocol.SelectedIndex = 0;
						}
						else if (acct.MailIncomingProtocol == IncomingMailProtocol.Imap4)
						{
							labelIncomingProtocol.Text = "IMAP4";
							selectProtocol.SelectedIndex = 1;
						}
						else if (acct.MailIncomingProtocol == IncomingMailProtocol.WMServer)
						{
							labelIncomingProtocol.Text = "XMail";
							selectProtocol.SelectedIndex = 2;
						}
						txtIncomingPort.Value = acct.MailIncomingPort.ToString(CultureInfo.InvariantCulture);
						txtIncomingLogin.Value = acct.MailIncomingLogin;
						txtIncomingPassword.Attributes.Add("Value", acct.MailIncomingPassword);
						txtSmtpServer.Value = acct.MailOutgoingHost;
						txtSmtpPort.Value = acct.MailOutgoingPort.ToString(CultureInfo.InvariantCulture);
						txtSmtpLogin.Value = acct.MailOutgoingLogin;
						txtSmtpPassword.Attributes.Add("Value", acct.MailOutgoingPassword);
						chkUseSmtpAuth.Checked = acct.MailOutgoingAuthentication;
						chkUseFriendlyName.Checked = acct.UseFriendlyName;
						chkSyncFolders.Checked = acct.GetMailAtLogin;
						if ((acct.MailIncomingProtocol == IncomingMailProtocol.Pop3)
							|| (acct.MailIncomingProtocol == IncomingMailProtocol.WMServer))
						{
							switch (acct.MailMode)
							{
								case MailMode.DeleteMessagesFromServer:
									radioDelRecvMsgs.Checked = true;
									break;
								case MailMode.LeaveMessagesOnServer:
									radioLeaveMsgs.Checked = true;
									break;
								case MailMode.KeepMessagesOnServer:
									txtKeepMsgsDays.Value = acct.MailsOnServerDays.ToString(CultureInfo.InvariantCulture);
									chkKeepMsgs.Checked = true;
									goto case MailMode.LeaveMessagesOnServer;
								case MailMode.DeleteMessageWhenItsRemovedFromTrash:
									chkDelMsgsSrv.Checked = true;
									goto case MailMode.LeaveMessagesOnServer;
                                case MailMode.KeepMessagesOnServerAndDeleteMessageWhenItsRemovedFromTrash:
                                    txtKeepMsgsDays.Value = acct.MailsOnServerDays.ToString(CultureInfo.InvariantCulture);
                                    chkKeepMsgs.Checked = true;
                                    chkDelMsgsSrv.Checked = true;
                                    goto case MailMode.LeaveMessagesOnServer;
							}
							Folder fld = null;
							BaseWebMailActions bwa = new BaseWebMailActions(acct, this.Page);
							fld = bwa.GetFolder(FolderType.Inbox);
							if (fld != null)
							{
								switch (fld.SyncType)
								{
									case FolderSyncType.AllEntireMessages:
										chkDelMsgsDB.Checked = true;
										goto case FolderSyncType.NewEntireMessages;
									case FolderSyncType.AllHeadersOnly:
										chkDelMsgsDB.Checked = true;
										goto case FolderSyncType.NewHeadersOnly;
									case FolderSyncType.DirectMode:
										synchronizeSelect.SelectedIndex = 2;
										break;
									case FolderSyncType.NewEntireMessages:
										synchronizeSelect.SelectedIndex = 1;
										break;
									case FolderSyncType.NewHeadersOnly:
										synchronizeSelect.SelectedIndex = 0;
										break;
								}
							}
						}
						else if (acct.MailIncomingProtocol == IncomingMailProtocol.Imap4)
						{
							// IMAP account
							tr_0.Attributes["class"] = tr_1.Attributes["class"] = tr_2.Attributes["class"] = tr_3.Attributes["class"] = tr_4.Attributes["class"] = "wm_hide";
						}
					}
				}
				else
				{
					txtLimitMailbox.Value = settings.MailboxSizeLimit.ToString(CultureInfo.InvariantCulture);
					txtIncomingPort.Value = settings.IncomingMailPort.ToString(CultureInfo.InvariantCulture);
					txtSmtpPort.Value = settings.OutgoingMailPort.ToString(CultureInfo.InvariantCulture);
					labelIncomingProtocol.CssClass = "wm_hide";
					selectProtocol.CssClass = "";
					chkUseSmtpAuth.Checked = true;
					chkUseFriendlyName.Checked = true;
					radioLeaveMsgs.Checked = true;
					chkAllowChangeEmail.Checked = true;
					selectProtocol.Visible = true;
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

		protected void SubmitButton_Click(object sender, System.EventArgs e)
		{
				if (_isUpdate)
				{
					Account acct = Account.LoadFromDb(_id_acct, -1);
					if (acct != null)
					{
						try
						{
							if ((acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
							{
								try
								{
									acct.UserOfAccount.Settings.MailboxLimit = int.Parse(txtLimitMailbox.Value, CultureInfo.InvariantCulture);
								} 
								catch {}
								acct.UserOfAccount.Settings.AllowDirectMode = chkAllowDM.Checked;
								acct.UserOfAccount.Settings.AllowChangeSettings = chkAllowChangeEmail.Checked;
							}
							acct.FriendlyName = txtFriendlyName.Value;
							acct.Email = txtEmail.Value;
							acct.MailIncomingHost = txtIncomingServer.Value;
							try
							{
								acct.MailIncomingPort = int.Parse(txtIncomingPort.Value, CultureInfo.InvariantCulture);
							} 
							catch {}
							acct.MailIncomingLogin = txtIncomingLogin.Value;
							acct.MailIncomingPassword = txtIncomingPassword.Text;
							acct.MailOutgoingHost = txtSmtpServer.Value;
							try
							{
								acct.MailOutgoingPort = int.Parse(txtSmtpPort.Value, CultureInfo.InvariantCulture);
							} 
							catch {}
							acct.MailOutgoingLogin = txtSmtpLogin.Value;
							acct.MailOutgoingPassword = txtSmtpPassword.Text;
							acct.MailOutgoingAuthentication = chkUseSmtpAuth.Checked;
							acct.UseFriendlyName = chkUseFriendlyName.Checked;
							acct.GetMailAtLogin = chkSyncFolders.Checked;
							if (radioDelRecvMsgs.Checked)
							{
								acct.MailMode = MailMode.DeleteMessagesFromServer;
							}
							if (radioLeaveMsgs.Checked)
							{
                                acct.MailMode = MailMode.LeaveMessagesOnServer;
								if (chkKeepMsgs.Checked)
								{
									acct.MailMode = MailMode.KeepMessagesOnServer;
								}
                                if (chkDelMsgsSrv.Checked)
                                {
                                    acct.MailMode = MailMode.DeleteMessageWhenItsRemovedFromTrash;
                                }
                                if (chkKeepMsgs.Checked && chkDelMsgsSrv.Checked)
                                {
                                    acct.MailMode = MailMode.KeepMessagesOnServerAndDeleteMessageWhenItsRemovedFromTrash;
                                }
							}
							try
							{
								acct.MailsOnServerDays = short.Parse(txtKeepMsgsDays.Value, CultureInfo.InvariantCulture);
							} 
							catch {}
							DbManager dbMan = (new DbManagerCreator()).CreateDbManager(acct);
							Folder fld = null;
							try
							{
								dbMan.Connect();
								fld = dbMan.SelectFolder(FolderType.Inbox);
								if (fld != null)
								{
									switch (synchronizeSelect.SelectedValue)
									{
										case "1":
											fld.SyncType = (chkDelMsgsDB.Checked) ? FolderSyncType.AllHeadersOnly : FolderSyncType.NewHeadersOnly;
											break;
										case "2":
											fld.SyncType = (chkDelMsgsDB.Checked) ? FolderSyncType.AllEntireMessages : FolderSyncType.NewEntireMessages;
											break;
										case "3":
											fld.SyncType = FolderSyncType.DirectMode;
											break;
									}
									dbMan.UpdateFolder(fld);
								}
							}
							finally
							{
								dbMan.Disconnect();
							}
							acct.Update(true);
							SuccessOutput(Constants.mailAdmUpdateAccountSuccess);
						}
						catch (Exception ex)
						{
							UnsuccessOutput(Constants.mailAdmUpdateAccountUnsuccess, ex);
						}
					}
				}
				else
				{
                    User usr = null;
					try
					{
						usr = User.CreateUser();
						WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
						int incomingPort = settings.IncomingMailPort;
						int outgoingPort = settings.OutgoingMailPort;
						short mailOnServer = 10;
						long mailBoxSize = settings.MailboxSizeLimit;
						if (txtIncomingPort.Value.Length > 0)
						{
							try
							{
								incomingPort = short.Parse(txtIncomingPort.Value, CultureInfo.InvariantCulture);
							}
							catch {}
						}
						if (txtSmtpPort.Value.Length > 0)
						{
							try
							{
								outgoingPort = short.Parse(txtSmtpPort.Value, CultureInfo.InvariantCulture);
							}
							catch {}
						}
						if (txtKeepMsgsDays.Value.Length > 0)
						{
							try
							{
								mailOnServer = short.Parse(txtKeepMsgsDays.Value, CultureInfo.InvariantCulture);
							}
							catch {}
						}
						if (txtLimitMailbox.Value.Length > 0)
						{
							try
							{
								mailBoxSize = long.Parse(txtLimitMailbox.Value, CultureInfo.InvariantCulture);
							}
							catch {}
						}
						MailMode mailMode = MailMode.DeleteMessagesFromServer;
						if (radioLeaveMsgs.Checked)
						{
                            mailMode = MailMode.LeaveMessagesOnServer;
							if (chkKeepMsgs.Checked)
							{
								mailMode = MailMode.KeepMessagesOnServer;
							}
                            if (chkDelMsgsSrv.Checked)
                            {
                                mailMode = MailMode.DeleteMessageWhenItsRemovedFromTrash;
                            }
                            if (chkKeepMsgs.Checked && chkDelMsgsSrv.Checked)
                            {
                                mailMode = MailMode.KeepMessagesOnServerAndDeleteMessageWhenItsRemovedFromTrash;
                            }
						}

						FolderSyncType inboxSyncType = FolderSyncType.DontSync;
						switch (synchronizeSelect.SelectedValue)
						{
							case "1":
								inboxSyncType = FolderSyncType.NewHeadersOnly;
								if (chkDelMsgsDB.Checked)
								{
									inboxSyncType = FolderSyncType.AllHeadersOnly;							
								}
								break;
							case "2":
								inboxSyncType = FolderSyncType.NewEntireMessages;
								if (chkDelMsgsDB.Checked)
								{
									inboxSyncType = FolderSyncType.AllEntireMessages;
								}
								break;
							case "3":
								inboxSyncType = FolderSyncType.DirectMode;
								break;
						}
						IncomingMailProtocol protocol = IncomingMailProtocol.Pop3;
						switch (selectProtocol.SelectedValue.ToLower(CultureInfo.InvariantCulture))
						{
							case "pop3":
								protocol = IncomingMailProtocol.Pop3;
								break;
							case "imap4":
								protocol = IncomingMailProtocol.Imap4;
								break;
							case "xmail":
								protocol = IncomingMailProtocol.WMServer;
								break;
						}

                        Account acct = null;
                        try
                        {
                            acct = usr.CreateAccount(true, false, txtEmail.Value, protocol, txtIncomingServer.Value, txtIncomingLogin.Value, txtIncomingPassword.Text, incomingPort, txtSmtpServer.Value, txtSmtpLogin.Value, txtSmtpPassword.Text, outgoingPort, chkUseSmtpAuth.Checked, txtFriendlyName.Value, chkUseFriendlyName.Checked,
                                DefaultOrder.Date, false, mailMode, mailOnServer, string.Empty, SignatureType.Plain, SignatureOptions.DontAddSignature, "/", 0, inboxSyncType, false);
                            SuccessOutput(Constants.mailAdmCreateAccountSuccess);
                        }
                        catch (WebMailException wex)
                        {
                            if (null != usr)
                            {
                                User.DeleteUserSettings(usr.ID);
                            }
                            throw wex;
                        }

                        if (null == acct)
                        {
                            throw new WebMailException(Constants.mailAdmCreateAccountUnsuccess);
                        }
                        _id_acct = acct.ID;

						if ((acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
						{
							acct.UserOfAccount.Settings.MailboxLimit = mailBoxSize;
							acct.Update(true);
						}
						Response.Redirect(string.Format(@"mailadm.aspx?mode=edit_user&uid={0}", _id_acct));
					}
					catch (Exception ex)
					{
						UnsuccessOutput(Constants.mailAdmCreateAccountUnsuccess, ex);
					}
				}
		}

		private void GridDataBind(int id_user)
		{
			DbManager manager = (new DbManagerCreator()).CreateDbManager();
			try
			{
				manager.Connect();
				using (IDataReader reader = manager.SelectUserAccountsAdminReader(id_user))
				{
					//TODO: add account to user
					this.accountsGrid.PageSize = 1000;
					this.accountsGrid.VirtualItemCount = 2000;//*/int.MaxValue;
					System.Diagnostics.Debug.WriteLine(this.accountsGrid.PageCount);
					System.Diagnostics.Debug.WriteLine(this.accountsGrid.CurrentPageIndex);
					System.Diagnostics.Debug.WriteLine(this.accountsGrid.VirtualItemCount);
					this.accountsGrid.DataSource = reader;
					this.accountsGrid.DataBind();
				}
			}
			finally
			{
				manager.Disconnect();
			}
		}

		protected string GetClassName(object obj)
		{
            if (obj != null)
            {
            	int id_acct = Convert.ToInt32(obj);
				if (id_acct == _id_acct)
					return "wm_settings_list_select";
            }
			return string.Empty;
		}
		
		private void SuccessOutput(string outStr)
		{
			messLabelID.InnerText = outStr;
			messLabelID.Style.Add("color", "green");
			messLabelID.Style.Add("font", "bold");

			errorLabelID.InnerText = string.Empty;
		}

		private void UnsuccessOutput(string outStr, Exception ex)
		{
			messLabelID.InnerText = outStr;
			messLabelID.Style.Add("color", "red");
			messLabelID.Style.Add("font", "bold");

			if (ex != null)
			{
				errorLabelID.InnerText = ex.Message;
				errorLabelID.Style.Add("color", "red");
			}
		}

	}
}
