namespace WebMailPro.MailAdm
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for mailadm_login_settings.
	/// </summary>
	public partial class mailadm_login_settings : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here

			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			if (!Page.IsPostBack)
			{
				switch (settings.HideLoginMode)
				{
					case LoginMode.Default:
						standartLoginRadio.Checked = true;
						break;
					case LoginMode.HideLoginFieldLoginIsEmail:
						hideLoginRadio.Checked = true;
						hideLoginSelect.SelectedIndex = 0;
						break;
					case LoginMode.HideLoginFieldLoginIsAccount:
						hideLoginRadio.Checked = true;
						hideLoginSelect.SelectedIndex = 1;
						break;
					case LoginMode.HideEmailField:
						hideEmailRadio.Checked = true;
						break;
					case LoginMode.HideEmailFieldDisplayDomainAfterLogin:
						hideEmailRadio.Checked = true;
						intDisplayDomainAfterLoginField.Checked = true;
						break;
					case LoginMode.HideEmailFieldLoginIsLoginAndDomain:
						hideEmailRadio.Checked = true;
						intLoginAsConcatination.Checked = true;
						break;
					case LoginMode.HideEmailFieldDisplayDomainAfterLoginAndLoginIsLoginAndDomain:
						hideEmailRadio.Checked = true;
						intDisplayDomainAfterLoginField.Checked = true;
						intLoginAsConcatination.Checked = true;
						break;
					default:
						goto case LoginMode.Default;
				}
				txtUseDomain.Value = settings.DefaultDomainOptional;

				intAllowAdvancedLogin.Checked = settings.AllowAdvancedLogin;
				intAutomaticCorrectLogin.Checked = settings.AutomaticCorrectLoginSettings;
			}
			else
			{
				Trace.Write("Login settings is postback");
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
				WebMailSettingsCreator creator = new WebMailSettingsCreator();
				creator.ResetWebMailSettings();
				WebmailSettings settings = creator.CreateWebMailSettings();
				if (standartLoginRadio.Checked)
				{
					settings.HideLoginMode = LoginMode.Default;
				}
				else if (hideLoginRadio.Checked)
				{
					if (hideLoginSelect.SelectedIndex == 0)
					{
						settings.HideLoginMode = LoginMode.HideLoginFieldLoginIsEmail;
					}
					else
					{
						settings.HideLoginMode = LoginMode.HideLoginFieldLoginIsAccount;
					}
				}
				else
				{
					if ((intDisplayDomainAfterLoginField.Checked) && (intLoginAsConcatination.Checked))
					{
						settings.HideLoginMode = LoginMode.HideEmailFieldDisplayDomainAfterLoginAndLoginIsLoginAndDomain;
					}
					else if (intDisplayDomainAfterLoginField.Checked)
					{
						settings.HideLoginMode = LoginMode.HideEmailFieldDisplayDomainAfterLogin;
					}
					else if (intLoginAsConcatination.Checked)
					{
						settings.HideLoginMode = LoginMode.HideEmailFieldLoginIsLoginAndDomain;
					}
					else
					{
						settings.HideLoginMode = LoginMode.HideEmailField;
					}
				}
				settings.DefaultDomainOptional = txtUseDomain.Value;

				settings.AllowAdvancedLogin = intAllowAdvancedLogin.Checked;
				settings.AutomaticCorrectLoginSettings = intAutomaticCorrectLogin.Checked;

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
