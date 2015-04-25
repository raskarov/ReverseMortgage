using System.IO;

namespace WebMailPro.MailAdm
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for mailadm_mail_server_integration.
	/// </summary>
	public partial class mailadm_mail_server_integration : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			if (!Page.IsPostBack)
			{
				intEnableMwServer.Checked = settings.EnableWmServer;
				txtWmServerRootPath.Value = settings.WmServerRootPath;
				txtWmServerHostName.Value = settings.WmServerHost;
				intWmAllowManageXMailAccounts.Checked = settings.WmAllowManageXMailAccounts;

			}
			else
			{
				Trace.Write("Mail Server settings is postback");
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

		protected void server_connection_ServerClick(object sender, EventArgs e)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			try
			{
				if (txtWmServerRootPath.Value.Length > 0)
				{
					string fullPath = Path.Combine(txtWmServerRootPath.Value, "domains");
					if (!Directory.Exists(fullPath))
					{
						throw new WebMailIOException(string.Format(@"Server Root Path '{0}' incorrect.", Utils.EncodeHtml(fullPath)));
					}
					WMServerStorage storage = new WMServerStorage(txtWmServerHostName.Value, null);
					storage.GetDomainList(); // test command

					SuccessOutput(Constants.mailAdmConnectSuccess);
				}
				else
				{
					throw new WebMailIOException(@"Server Root Path not set.");
				}
			}
			catch (Exception ex)
			{
				UnsuccessOutput(Constants.mailAdmConnectUnsuccess, ex);
			}
		}

		protected void save_ServerClick(object sender, EventArgs e)
		{
			try
			{
				WebMailSettingsCreator creator = new WebMailSettingsCreator();
				creator.ResetWebMailSettings();
				WebmailSettings settings = creator.CreateWebMailSettings();

				settings.EnableWmServer = intEnableMwServer.Checked;
				settings.WmServerRootPath = txtWmServerRootPath.Value;
                settings.WmServerHost = txtWmServerHostName.Value;
                settings.WmAllowManageXMailAccounts = intWmAllowManageXMailAccounts.Checked;

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
