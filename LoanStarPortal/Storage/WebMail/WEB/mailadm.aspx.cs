using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WebMailPro;
using Calendar_NET;

namespace WebMailPro.MailAdm
{
	public enum MailAdmScreen
	{
		Login = 0,
		DatabaseSettings = 1,
		UserManagement = 2,
		WebMailSettings = 3,
		InterfaceSettings = 4,
		LoginSettings = 5,
		DebugSettings = 6,
		UserDetails = 7,
		MailServer = 8,
        CalendarSettings = 9
	}

	/// <summary>
	/// Summary description for mailadm.
	/// </summary>
	public partial class mailadm : System.Web.UI.Page
	{
		protected MailAdmScreen _screen = MailAdmScreen.Login;

		protected mailadm_login loginID;
		protected mailadm_database_settings dbSettingsID;
		protected mailadm_user_management userMngtID;
		protected mailadm_user_details userDetailsID;
		protected mailadm_webmail_settings wmSettingsID;
		protected mailadm_interface_settings ifcSettingsID;
		protected mailadm_login_settings lgnSettingsID;
        protected mailadm_calendar_settings cldrSettingsID;
        protected mailadm_debug_settings dbgSettingsID;
		protected mailadm_mail_server_integration mailServerID;
		protected mailadm_menu MenuID;
		protected WebmailSettings settings;

		public WebmailSettings Settings
		{
			get { return settings; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{

			WebMailSettingsCreator creator = new WebMailSettingsCreator();
			creator.ResetWebMailSettings();
			try
			{
				settings = creator.CreateWebMailSettings();
			}
			catch (WebMailSettingsException)
			{
				Response.Write(string.Format(@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"" />
<html>
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <meta http-equiv=""Content-Script-Type"" content=""text/javascript"" />
    <meta http-equiv=""Cache-Control"" content=""private,max-age=1209600"" />
    <title>WebMail probably not configured</title>
    <link rel=""stylesheet"" href=""skins/Hotmail_Style/styles.css"" type=""text/css"" />
</head>
<body>
<div align=""center"" id=""content"" class=""wm_content"">
    <div class=""wm_logo"" id=""logo"" tabindex=""-1""></div>
    <div class=""wm_login_error"">The web-server has no permission to write into the settings file<br/>or<br/>settings file not exists<br/>{0}<br/><br/>To learn how to grant the appropriate permission, please refer to WebMail documentation:<br/><br/><a href='help/installation_instructions.html'>Installation Instructions</a><br/>
</div>
<div class=""wm_copyright"" id=""copyright"">
	Powered by <a href=""http://www.afterlogic.com/mailbee/webmail-pro.asp"" target=""_blank""> MailBee WebMail</a><br>
Copyright &copy; 2002-2008 <a href=""http://www.afterlogic.com"" target=""_blank"">AfterLogic Corporation</a>
</div>
</body>
</html>
", Path.Combine(Utils.GetDataFolderPath(), @"settings\settings.xml")));
				Response.End();
				return;
			}

			object obj = Session["mailadm_login"];
			if((obj != null) && ((bool)obj == true))
			{
				string mode = this.Request.QueryString["mode"];
				MailadmForm.Attributes.Add("mode", mode);
				if (mode != null)
				{
					if (string.Compare(mode, "db", true, CultureInfo.InvariantCulture) == 0)
					{
						_screen = MailAdmScreen.DatabaseSettings;

						Control ctrl = LoadControl(@"MailAdm\mailadm_database_settings.ascx");
						dbSettingsID = ctrl as mailadm_database_settings;
						if (dbSettingsID != null)
						{
							dbSettingsID.ID = "dbSettingsID";
							ContentPlaceHolder.Controls.Add(dbSettingsID);
						}
						//dbSettingsID.Visible = true;
					}
					else if (string.Compare(mode, "users", true, CultureInfo.InvariantCulture) == 0)
					{ 
						_screen = MailAdmScreen.UserManagement;
						Control ctrl = LoadControl(@"MailAdm\mailadm_user_management.ascx");
						userMngtID = ctrl as mailadm_user_management;
						if (userMngtID != null)
						{
							string asc = this.Request.QueryString["asc"];
							if ((asc != null) && (asc.Length > 0))
							{
								userMngtID.Asc = (string.Compare(asc, "true", true, CultureInfo.InvariantCulture) == 0) ? true : false;	
							}
							string orderBy = this.Request.QueryString["order"];
							if ((orderBy != null) && (orderBy.Length > 0))
							{
								userMngtID.OrderBy = orderBy;
							}
							string condition = this.Request.QueryString["condition"];
                            if ((condition != null) && (condition.Length > 0))
                            {
                                userMngtID.SearchCondition = condition;
                            }
                            else
                            {
                                userMngtID.SearchCondition = string.Empty;
                            }
							userMngtID.ID = "userMngtID";
							ContentPlaceHolder.Controls.Add(userMngtID);
						}
						//userMngtID.Visible = true;
					}
					else if (string.Compare(mode, "user_details", true, CultureInfo.InvariantCulture) == 0)
					{
						_screen = MailAdmScreen.UserDetails;

						Control ctrl = LoadControl(@"MailAdm\mailadm_user_details.ascx");
						userDetailsID = ctrl as mailadm_user_details;
						if (userDetailsID != null)
						{
							userDetailsID.ID = "userDetailsID";
							ContentPlaceHolder.Controls.Add(userDetailsID);
						}
						//userDetailsID.Visible = true;
					}
					else if (string.Compare(mode, "webmail", true, CultureInfo.InvariantCulture) == 0)
					{
						_screen = MailAdmScreen.WebMailSettings;

						Control ctrl = LoadControl(@"MailAdm\mailadm_webmail_settings.ascx");
						wmSettingsID = ctrl as mailadm_webmail_settings;
						if (wmSettingsID != null)
						{
							wmSettingsID.ID = "wmSettingsID";
							ContentPlaceHolder.Controls.Add(wmSettingsID);
						}
						//wmSettingsID.Visible = true;
					}
					else if (string.Compare(mode, "interface", true, CultureInfo.InvariantCulture) == 0)
					{
						_screen = MailAdmScreen.InterfaceSettings;

						Control ctrl = LoadControl(@"MailAdm\mailadm_interface_settings.ascx");
						ifcSettingsID = ctrl as mailadm_interface_settings;
						if (ifcSettingsID != null)
						{
							ifcSettingsID.ID = "ifcSettingsID";
							ContentPlaceHolder.Controls.Add(ifcSettingsID);
						}
						//ifcSettingsID.Visible = true;
					}
					else if (string.Compare(mode, "login", true, CultureInfo.InvariantCulture) == 0)
					{
						_screen = MailAdmScreen.LoginSettings;

						Control ctrl = LoadControl(@"MailAdm\mailadm_login_settings.ascx");
						lgnSettingsID = ctrl as mailadm_login_settings;
						if (lgnSettingsID != null)
						{
							lgnSettingsID.ID = "lgnSettingsID";
							ContentPlaceHolder.Controls.Add(lgnSettingsID);
						}
						//lgnSettingsID.Visible = true;
					}
                    else if (string.Compare(mode, "calendar", true, CultureInfo.InvariantCulture) == 0)
                    {
                        _screen = MailAdmScreen.CalendarSettings;

                        Control ctrl = LoadControl(@"MailAdm\mailadm_calendar_settings.ascx");
                        cldrSettingsID = ctrl as mailadm_calendar_settings;
                        if (cldrSettingsID != null)
                        {
                            cldrSettingsID.ID = "cldrSettingsID";
                            ContentPlaceHolder.Controls.Add(cldrSettingsID);
                        }
                        //cldrSettingsID.Visible = true;
                    }
                    else if (string.Compare(mode, "debug", true, CultureInfo.InvariantCulture) == 0)
					{
						_screen = MailAdmScreen.DebugSettings;

						Control ctrl = LoadControl(@"MailAdm\mailadm_debug_settings.ascx");
						dbgSettingsID = ctrl as mailadm_debug_settings;
						if (dbgSettingsID != null)
						{
							dbgSettingsID.ID = "dbgSettingsID";
							ContentPlaceHolder.Controls.Add(dbgSettingsID);
						}
						//dbgSettingsID.Visible = true;
					}
					else if (string.Compare(mode, "mailserver", true, CultureInfo.InvariantCulture) == 0)
					{
						_screen = MailAdmScreen.MailServer;

						Control ctrl = LoadControl(@"MailAdm\mailadm_mail_server_integration.ascx");
						mailServerID = ctrl as mailadm_mail_server_integration;
						if (mailServerID != null)
						{
							mailServerID.ID = "mailServerID";
							ContentPlaceHolder.Controls.Add(mailServerID);
						}
						//dbgSettingsID.Visible = true;
					}
					else if (string.Compare(mode, "logout", true, CultureInfo.InvariantCulture) == 0)
					{
						Session.Clear();
						_screen = MailAdmScreen.Login;
					}
					else if (string.Compare(mode, "new_user", true, CultureInfo.InvariantCulture) == 0)
					{
						_screen = MailAdmScreen.UserDetails;
//						userDetailsID.Visible = true;

						Control ctrl = LoadControl(@"MailAdm\mailadm_user_details.ascx");
						userDetailsID = ctrl as mailadm_user_details;
						if (userDetailsID != null)
						{
							userDetailsID.IsUpdate = false;
							userDetailsID.ID = "userDetailsID";
							ContentPlaceHolder.Controls.Add(userDetailsID);
						}
					}
					else if (string.Compare(mode, "delete_user", true, CultureInfo.InvariantCulture) == 0)
					{
						_screen = MailAdmScreen.UserManagement;

						Control ctrl = LoadControl(@"MailAdm\mailadm_user_management.ascx");
						userMngtID = ctrl as mailadm_user_management;
						if (userMngtID != null)
						{
							userMngtID.ID = "userMngtID";
							ContentPlaceHolder.Controls.Add(userMngtID);
						}
						//userMngtID.Visible = true;
						string uid = this.Request.QueryString["uid"];
						if (uid.Length > 0)
						{
							try
							{
								int id_acct = int.Parse(uid, CultureInfo.InvariantCulture);
								Account acct = Account.LoadFromDb(id_acct, -1);
								Account[] userAccouts = acct.UserOfAccount.GetUserAccounts();
								if (userAccouts.Length == 1)
								{
									Account.DeleteFromDb(acct);
									WebMailPro.User.DeleteUser(acct.IDUser);
								}
								else
								{
									Account.DeleteFromDb(acct);
								}
							}
							catch (Exception ex)
							{
                                Log.WriteException(ex);								
							}
						}
                        string order = ((string)Session["wm_adm_um_order"] != null) ? (string)Session["wm_adm_um_order"] : "email";
                        string _asc = ((string)Session["wm_adm_um_asc"] != null) ? (string)Session["wm_adm_um_asc"] : "True";
                        string _searchCondition = ((string)Session["wm_adm_um_condition"] != null) ? (string)Session["wm_adm_um_condition"] : string.Empty;

                        Response.Redirect(string.Format(@"mailadm.aspx?mode=users&order={0}&asc={1}&condition={2}", order, _asc, _searchCondition));
					}
					else if (string.Compare(mode, "edit_user", true, CultureInfo.InvariantCulture) == 0)
					{
						string uid = this.Request.QueryString["uid"];
						if (uid.Length > 0)
						{
							try
							{
								int id_acct = int.Parse(uid, CultureInfo.InvariantCulture);
								_screen = MailAdmScreen.UserDetails;

								Control ctrl = LoadControl(@"MailAdm\mailadm_user_details.ascx");
								userDetailsID = ctrl as mailadm_user_details;
								if (userDetailsID != null)
								{
									userDetailsID.IsUpdate = true;
									userDetailsID.AccountID = id_acct;
									userDetailsID.ID = "userDetailsID";
									ContentPlaceHolder.Controls.Add(userDetailsID);
								}
								//userDetailsID.Visible = true;
							}
							catch {}
						}
					}
					else if (string.Compare(mode, "show_log", true, CultureInfo.InvariantCulture) == 0)
					{
						Response.Clear();
						Response.AddHeader("Content-Type", "text/html");
						Response.Write(@"<html><body><pre class=""wm_print_content"">");
						string logPath = Utils.GetLogFilePath();
						Response.Write(HttpUtility.HtmlEncode(Utils.LoadFromFile(logPath, Encoding.Default)));
						Response.Write("</pre></body></html>");
						Response.Flush();
						Response.Close();
					}
					else if (string.Compare(mode, "show_log_partial", true, CultureInfo.InvariantCulture) == 0)
					{
						Response.Clear();
						Response.AddHeader("Content-Type", "text/plain");
						Response.Write(@"<html><body><pre class=""wm_print_content"">");
						string logPath = Utils.GetLogFilePath();
						FileInfo fi = new FileInfo(logPath);
						string log = string.Empty;
						int lastPartSize = 50000;
						if (fi.Exists)
						{
							if (fi.Length > lastPartSize)
							{
								log = Utils.LoadFromFile(logPath, Encoding.Default, (int)fi.Length - lastPartSize, lastPartSize);
							}
							else
							{
								log = Utils.LoadFromFile(logPath, Encoding.Default, 0, lastPartSize);
							}
						}
						Response.Write(log);
						Response.Write("</pre></body></html>");
						Response.Flush();
						Response.Close();
					}
					else if (string.Compare(mode, "clear_log", true, CultureInfo.InvariantCulture) == 0)
					{
						
					}
					else
					{
						_screen = MailAdmScreen.Login;
					}
					MenuID.Screen = (int)_screen;
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
