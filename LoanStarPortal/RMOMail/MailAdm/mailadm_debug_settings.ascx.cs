using System.Configuration;
using System.IO;
using System.Text;

namespace WebMailPro.MailAdm
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for mailadm_debug_settings.
	/// </summary>
	public partial class mailadm_debug_settings : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here

			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			if (!Page.IsPostBack)
			{
                string logPath = Utils.GetLogFilePath();			
				intEnableLogging.Checked = settings.EnableLogging;
				txtPathForLog.Value = logPath;
				intDisableErrorHandling.Checked = settings.DisableErrorHandling;
			}
			else
			{
				Trace.Write("Debug settings is postback");
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
				settings.EnableLogging = intEnableLogging.Checked;
				settings.DisableErrorHandling = intDisableErrorHandling.Checked;
				settings.SaveWebmailSettings();

				SuccessOutput(Constants.mailAdmSaveSuccess);
			}
			catch (Exception ex)
			{
				UnsuccessOutput(Constants.mailAdmSaveUnsuccess, ex);
			}
		}

		protected void ClearLogButton_ServerClick(object sender, System.EventArgs e)
		{
			try
			{
				Utils.ClearLog();

				SuccessOutput(Constants.mailAdmLogClearSuccess);
			}
			catch (Exception ex)
			{
				UnsuccessOutput(Constants.mailAdmLogClearUnsuccess, ex);
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
