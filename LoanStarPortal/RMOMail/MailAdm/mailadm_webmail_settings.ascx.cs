using System.Globalization;

namespace WebMailPro.MailAdm
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using MailBee;
	using MailBee.Pop3Mail;

	public partial class mailadm_webmail_settings : System.Web.UI.UserControl
	{
		protected string licenseKey = "";
		private const string EMPTYLICENSEKEY = "***************************************";

		protected void Page_Load(object sender, System.EventArgs e)
		{
			errorLicenseKeyLabelID.InnerText = "";

			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			if (settings.LicenseKey.Trim().Length > 0)
			{
				licenseKey = EMPTYLICENSEKEY;
			}
			else
			{
				if (!Page.IsPostBack)
				{
					licenseKey = "";
					errorLicenseKeyLabelID.InnerText = "Warning: License Key is empty";
					errorLicenseKeyLabelID.Style.Add("color", "red");
				}
				else
				{
					if (Request.Form["txtLicenseKey"].Length > 0)
					{
						licenseKey = EMPTYLICENSEKEY;
					}
					else
					{
						licenseKey = "";
						errorLicenseKeyLabelID.InnerText = "Warning: License Key is empty";
						errorLicenseKeyLabelID.Style.Add("color", "red");
					}
				}
			}

			if (!Page.IsPostBack)
			{
				txtSiteName.Value = settings.SiteName;

				txtIncomingMail.Value = settings.IncomingMailServer;
				intIncomingMailPort.Value = settings.IncomingMailPort.ToString(CultureInfo.InvariantCulture);

				if (settings.IncomingMailProtocol == IncomingMailProtocol.Pop3)
				{
					intIncomingMailProtocol.SelectedIndex = 0;
				}
				else if (settings.IncomingMailProtocol == IncomingMailProtocol.Imap4)
				{
					intIncomingMailProtocol.SelectedIndex = 1;
				}
				else if (settings.IncomingMailProtocol == IncomingMailProtocol.WMServer)
				{
					intIncomingMailProtocol.SelectedIndex = 2;
				}

				txtOutgoingMail.Value = settings.OutgoingMailServer;
				intOutgoingMailPort.Value = settings.OutgoingMailPort.ToString(CultureInfo.InvariantCulture);

				intReqSmtpAuthentication.Checked = settings.ReqSmtpAuth;
				intAllowDirectMode.Checked = settings.AllowDirectMode;
				intDirectModeIsDefault.Checked = settings.DirectModeIsDefault;

				intAttachmentSizeLimit.Value = settings.AttachmentSizeLimit.ToString(CultureInfo.InvariantCulture);
				intEnableAttachmentSizeLimit.Checked = settings.EnableAttachmentSizeLimit;
				intMailboxSizeLimit.Value = settings.MailboxSizeLimit.ToString(CultureInfo.InvariantCulture);
				intEnableMailboxSizeLimit.Checked = settings.EnableMailboxSizeLimit;

				intAllowUsersChangeEmailSettings.Checked = settings.AllowUsersChangeEmailSettings;
				intAllowNewUsersRegister.Checked = settings.AllowNewUsersRegister;
				intAllowUsersAddNewAccounts.Checked = settings.AllowUsersAddNewAccounts;

                txtDefaultUserCharset.Value = settings.DefaultUserCharset.ToString();
				intAllowUsersChangeCharset.Checked = settings.AllowUsersChangeCharset;

				if (settings.DefaultTimeZone < txtDefaultTimeZone.Items.Count)
				{
					txtDefaultTimeZone.SelectedIndex = settings.DefaultTimeZone;
				}
				intAllowUsersChangeTimeOffset.Checked = settings.AllowUsersChangeTimeZone;

				txtPasswordNew.Attributes.Add("Value", Constants.nonChangedPassword);
				txtPasswordConfirm.Attributes.Add("Value", Constants.nonChangedPassword);
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

		protected void SaveButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				string licenseKeyFromForm = "";
				Pop3 pop = null;
				licenseKeyFromForm = Request.Form["txtLicenseKey"];
				WebmailSettings settingsOriginal = (new WebMailSettingsCreator()).CreateWebMailSettings();

				WebMailSettingsCreator creator = new WebMailSettingsCreator();
				creator.ResetWebMailSettings();

				WebmailSettings settings = creator.CreateWebMailSettings();
				settings.SiteName = txtSiteName.Value;
				if (licenseKeyFromForm == EMPTYLICENSEKEY || licenseKeyFromForm.Length == 0)
				{
					settings.LicenseKey = settingsOriginal.LicenseKey;
				}
				else
				{
					try
					{
						Pop3.LicenseKey = licenseKeyFromForm;
						pop = new Pop3();
					}
					catch (MailBeeException)
					{
						errorLicenseKeyLabelID.InnerText = "License Key is invalid";
						errorLicenseKeyLabelID.Style.Add("color", "red");
					}

					settings.LicenseKey = licenseKeyFromForm;
				}

				settings.IncomingMailServer = txtIncomingMail.Value;
				try
				{
					settings.IncomingMailPort = int.Parse(intIncomingMailPort.Value, CultureInfo.InvariantCulture);
				}
				catch {}

				switch (intIncomingMailProtocol.Value.ToLower(CultureInfo.InvariantCulture))
				{
					case "pop3":
						settings.IncomingMailProtocol = IncomingMailProtocol.Pop3;
						break;
					case "imap4":
						settings.IncomingMailProtocol = IncomingMailProtocol.Imap4;
						break;
					case "xmail":
						settings.IncomingMailProtocol = IncomingMailProtocol.WMServer;
						break;
				}

				settings.OutgoingMailServer = txtOutgoingMail.Value;
				try
				{
					settings.OutgoingMailPort = int.Parse(intOutgoingMailPort.Value, CultureInfo.InvariantCulture);
				}
				catch {}

				settings.ReqSmtpAuth = intReqSmtpAuthentication.Checked;
				settings.AllowDirectMode = intAllowDirectMode.Checked;
				settings.DirectModeIsDefault = intDirectModeIsDefault.Checked;

				try
				{
					settings.AttachmentSizeLimit = long.Parse(intAttachmentSizeLimit.Value, CultureInfo.InvariantCulture);
				}
				catch {}
				settings.EnableAttachmentSizeLimit = intEnableAttachmentSizeLimit.Checked;
				try
				{
					settings.MailboxSizeLimit = int.Parse(intMailboxSizeLimit.Value, CultureInfo.InvariantCulture);
				}
				catch {}
				settings.EnableMailboxSizeLimit = intEnableMailboxSizeLimit.Checked;

				settings.AllowUsersChangeEmailSettings = intAllowUsersChangeEmailSettings.Checked;
				settings.AllowNewUsersRegister = intAllowNewUsersRegister.Checked;
				settings.AllowUsersAddNewAccounts = intAllowUsersAddNewAccounts.Checked;

				try
				{
					settings.DefaultUserCharset = int.Parse(txtDefaultUserCharset.Value, CultureInfo.InvariantCulture);
				}
				catch {}
				settings.AllowUsersChangeCharset = intAllowUsersChangeCharset.Checked;

				settings.DefaultTimeZone = (short)txtDefaultTimeZone.SelectedIndex;
				settings.AllowUsersChangeTimeZone = intAllowUsersChangeTimeOffset.Checked;

				if (txtPasswordNew.Text != Constants.nonChangedPassword)
				{
					if (txtPasswordConfirm.Text == txtPasswordNew.Text)
					{
						settings.AdminPassword = txtPasswordNew.Text;
					}
				}
				settings.SaveWebmailSettings();

				SuccessOutput(Constants.mailAdmSaveSuccess);
			}
			catch (Exception ex)
			{
				UnsuccessOutput(Constants.mailAdmSaveUnsuccess, ex);
			}
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
