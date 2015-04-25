using System.Globalization;

namespace WebMailPro.MailAdm
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for mailadm_interface_settings.
	/// </summary>
	public partial class mailadm_interface_settings : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.RangeValidator validatorMailsPerPage;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here

			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			if (!Page.IsPostBack)
			{
				intMailsPerPage.Value = settings.MailsPerPage.ToString(CultureInfo.InvariantCulture);

				string[] supportedSkins = Utils.GetSupportedSkins(Page.MapPath("skins"));
				for (int i = 0; i < supportedSkins.Length; i++)
				{
					txtDefaultSkin.Items.Add(supportedSkins[i]);
					if (string.Compare(supportedSkins[i], settings.DefaultSkin, true, CultureInfo.InvariantCulture) == 0)
					{
						txtDefaultSkin.SelectedIndex = i;
					}
				}

				string[] supportedLangs = Utils.GetSupportedLangs();
				for (int i = 0; i < supportedLangs.Length; i++)
				{
					txtDefaultLanguage.Items.Add(supportedLangs[i]);
					if (string.Compare(supportedLangs[i], settings.DefaultLanguage, true, CultureInfo.InvariantCulture) == 0)
					{
						txtDefaultLanguage.SelectedIndex = i;
					}
				}

				intAllowUsersChangeSkin.Checked = settings.AllowUsersChangeSkin;
				intAllowUsersChangeLanguage.Checked = settings.AllowUsersChangeLanguage;
				intShowTextLabels.Checked = settings.ShowTextLabels;
				intAllowAjaxVersion.Checked = settings.AllowAjax;
				intAllowDHTMLEditor.Checked = settings.AllowDhtmlEditor;
				intAllowContacts.Checked = settings.AllowContacts;
				intAllowCalendar.Checked = settings.AllowCalendar;
			}
			else
			{
				Trace.Write("Interface settings is postback");
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
			try
			{
				WebMailSettingsCreator creator = new WebMailSettingsCreator();
				creator.ResetWebMailSettings();
				WebmailSettings settings = creator.CreateWebMailSettings();
				try
				{
					settings.MailsPerPage = short.Parse(intMailsPerPage.Value, CultureInfo.InvariantCulture);
					if (settings.MailsPerPage < 0)
					{
						settings.MailsPerPage = Math.Abs(settings.MailsPerPage);
					}
				}
				catch {}

				settings.DefaultSkin = txtDefaultSkin.Value;
				settings.DefaultLanguage = txtDefaultLanguage.Value;
				settings.AllowUsersChangeSkin = intAllowUsersChangeSkin.Checked;
				settings.AllowUsersChangeLanguage = intAllowUsersChangeLanguage.Checked;
				settings.ShowTextLabels = intShowTextLabels.Checked;
				settings.AllowAjax = intAllowAjaxVersion.Checked;
				settings.AllowDhtmlEditor = intAllowDHTMLEditor.Checked;
				settings.AllowContacts = intAllowContacts.Checked;
				settings.AllowCalendar = intAllowCalendar.Checked;

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
