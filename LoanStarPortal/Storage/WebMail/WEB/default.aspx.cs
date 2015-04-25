using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MailBee.Mime;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for _default.
	/// </summary>
	public partial class _default : System.Web.UI.Page
	{
		protected WebmailResourceManager _resMan = null;

		protected const int POP3_PROTOCOL = 0;
		protected const int IMAP4_PROTOCOL = 1;
		protected const int WMSERVER_PROTOCOL = 2;

		protected string defaultTitle = string.Empty;
		protected string defaultSkin = string.Empty;
		protected string defaultLang = string.Empty;
		protected string defaultIncServer = string.Empty;
		protected int defaultIncProtocol = POP3_PROTOCOL;
		protected int defaultIncPort = 110;
		protected string defaultOutServer = string.Empty;
		protected int defaultOutPort = 25;
		protected bool defaultUseSmtpAuth = true;
		protected bool defaultSignMe = false;
		protected string defaultIsAjax = "true";
		/*****************************/
		protected string defaultAllowAdvancedLogin = "true";
		protected int defaultHideLoginMode = 1;
		protected string defaultDomainOptional = "localhost";
		/*****************************/
		protected string errorClass = string.Empty;
		protected string errorDesc = string.Empty;
		protected string mode = string.Empty;
		protected string switcherHref = string.Empty;
		protected string switcherText = string.Empty;

		protected string emailClass = string.Empty;
		protected string loginClass = string.Empty;
		protected string advancedClass = string.Empty;
		protected string loginWidth = "220px";
		protected string domainContent = string.Empty;
	
		protected string advancedLogin = string.Empty;
		protected string pop3Selected = string.Empty;
		protected string imap4Selected = string.Empty;
		protected string wmserverSelected = string.Empty;
		protected string smtpAuthChecked = string.Empty;
		protected string signMeChecked = string.Empty;
		protected string globalEmail = string.Empty;
		protected string globalLogin = string.Empty;
		protected string globalPassword = string.Empty;
		protected string globalIncServer = string.Empty;
		protected string globalIncProtocol = string.Empty;
		protected string globalIncPort = string.Empty;
		protected string globalOutServer = string.Empty;
		protected string globalOutPort = string.Empty;
		protected string globalUseSmtpAuth = string.Empty;
		protected string globalSignMe = string.Empty;
		protected string globalAdvancedLogin = string.Empty;

		protected string WmServerToHTML = string.Empty;

		protected WebmailSettings settings = null;

		public _default()
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Request.QueryString.Get("mode") != null)
				{
					mode = Request.QueryString.Get("mode");
				}
				if (mode == "logout")
				{
					Log.WriteLine("Page_Load", "Session Clear");
					Session.Clear();
				}
				if (Application[Constants.appSettingsDataFolderPath] == null)
				{
                    Application[Constants.appSettingsDataFolderPath] = ConfigurationManager.AppSettings[Constants.appSettingsDataFolderPath];
				}
			}
			try
			{
				_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
				settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			}
			catch (WebMailSettingsException)
			{
				Response.Write(@"
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
    <div class=""wm_login_error"">WebMail probably not configured</div>
</div>
    <div class=""wm_copyright"" id=""copyright"">
		Powered by <a href=""http://www.afterlogic.com/mailbee/webmail-pro.asp"" target=""_blank""> MailBee WebMail</a><br>
Copyright &copy; 2002-2008 <a href=""http://www.afterlogic.com"" target=""_blank"">AfterLogic Corporation</a>
	</div>
</body>
</html>
");
				Response.End();
				return;
			}

			try
			{

				defaultTitle = Utils.EncodeJsSaveString(settings.SiteName);
				defaultSkin = Utils.EncodeJsSaveString(settings.DefaultSkin);
				defaultLang = settings.DefaultLanguage;
				defaultIncServer = settings.IncomingMailServer;
											   
				switch(settings.IncomingMailProtocol)
				{
					case IncomingMailProtocol.Pop3:
					{
						defaultIncProtocol = POP3_PROTOCOL;
						break;
					}

					case IncomingMailProtocol.Imap4:
					{
						defaultIncProtocol = IMAP4_PROTOCOL;
						break;
					}

					case IncomingMailProtocol.WMServer:
					{
						defaultIncProtocol = WMSERVER_PROTOCOL;
						break;
					}
				}

				defaultIncPort = settings.IncomingMailPort;
				defaultOutServer = settings.OutgoingMailServer;
				defaultOutPort = settings.OutgoingMailPort;
				defaultUseSmtpAuth = settings.ReqSmtpAuth;
				defaultSignMe = false;
				defaultIsAjax = settings.AllowAjax ? "true" : "false";
				defaultAllowAdvancedLogin = (settings.AllowAdvancedLogin) ? "true" : "false";
				defaultHideLoginMode = (int)settings.HideLoginMode;
				defaultDomainOptional = Utils.EncodeJsSaveString(settings.DefaultDomainOptional);

				if (defaultIncProtocol == POP3_PROTOCOL)
				{
					pop3Selected = @" selected=""selected""";
				}
				else
				{
					pop3Selected = "";
				}
				if (defaultIncProtocol == IMAP4_PROTOCOL)
				{
					imap4Selected = @" selected=""selected""";
				}
				else
				{
					imap4Selected = "";
				}
				if (defaultIncProtocol == WMSERVER_PROTOCOL)
				{
					wmserverSelected = @" selected=""selected""";
				}
				else
				{
					wmserverSelected = "";
				}

				if (defaultUseSmtpAuth == true)
				{
					smtpAuthChecked = @" checked=""checked""";
				}
				else
				{
					smtpAuthChecked = "";
				}

				if (defaultSignMe == true)
				{
					signMeChecked = @" checked=""checked""";
				}
				else
				{
					signMeChecked = "";
				}

				//for version without ajax
				errorClass = "wm_hide"; //if there is no error
				errorDesc = "";
				mode = Request["mode"]; //mode = standard|advanced|submit
				switch (mode)
				{
					case "advanced":
						DisplayAdvancedMode();
						break;
					case "submit":
						DisplayStandardMode();
						globalEmail = Request["email"];
						globalLogin = Request["login"];
						globalPassword = Request["password"];
                        globalSignMe = Request["sign_me"];
                        globalAdvancedLogin = Request["advanced_login"];//0|1
                        if (globalAdvancedLogin == "1" || (int)settings.HideLoginMode < 20)
                        {
                            if (!Validation.CheckIt(Validation.ValidationTask.Email, globalEmail))
                            {
                                errorDesc = _resMan.GetString(Validation.ErrorMessage);
                                errorClass = "wm_login_error";
                                break;
                            }
                            else
                            {
                                globalEmail = Validation.Corrected;
                            }
                        }
                        if (globalAdvancedLogin == "1" || (int)settings.HideLoginMode != 10 && (int)settings.HideLoginMode != 11)
                        {
                            if (!Validation.CheckIt(Validation.ValidationTask.Login, globalLogin))
                            {
                                errorDesc = _resMan.GetString(Validation.ErrorMessage);
                                errorClass = "wm_login_error";
                                break;
                            }
                            else
                            {
                                globalLogin = Validation.Corrected;
                            }
                        }
                        if (!Validation.CheckIt(Validation.ValidationTask.Password, globalPassword))
                        {
                            errorDesc = _resMan.GetString(Validation.ErrorMessage);
                            errorClass = "wm_login_error";
                            break;
                        }

                        if (globalAdvancedLogin == "1")
                        {
                            globalIncServer = Request["inc_server"];
                            globalIncProtocol = Request["inc_protocol"];
                            globalIncPort = Request["inc_port"];
                            globalOutServer = Request["out_server"];
                            globalOutPort = Request["out_port"];
                            globalUseSmtpAuth = Request["smtp_auth"];
                            if (!Validation.CheckIt(Validation.ValidationTask.INServer, globalIncServer, globalAdvancedLogin))
                            {
                                errorDesc = _resMan.GetString(Validation.ErrorMessage);
                                errorClass = "wm_login_error";
                                break;
                            }
                            else
                            {
                                globalIncServer = Validation.Corrected;
                            }
                            if (!Validation.CheckIt(Validation.ValidationTask.INPort, globalIncPort, globalAdvancedLogin))
                            {
                                errorDesc = _resMan.GetString(Validation.ErrorMessage);
                                errorClass = "wm_login_error";
                                break;
                            }
                            else
                            {
                                globalIncPort = Validation.Corrected;
                            }
                            if (!Validation.CheckIt(Validation.ValidationTask.OUTServer, globalOutServer, globalAdvancedLogin))
                            {
                                errorDesc = _resMan.GetString(Validation.ErrorMessage);
                                errorClass = "wm_login_error";
                                break;
                            }
                            else
                            {
                                globalOutServer = Validation.Corrected;
                            }
                            if (!Validation.CheckIt(Validation.ValidationTask.OUTPort, globalOutPort, globalAdvancedLogin))
                            {
                                errorDesc = _resMan.GetString(Validation.ErrorMessage);
                                errorClass = "wm_login_error";
                                break;
                            }
                            else
                            {
                                globalOutPort = Validation.Corrected;
                            }
                        }
                        else
                        {
                            globalIncServer = settings.IncomingMailServer;
                            if (settings.IncomingMailProtocol == IncomingMailProtocol.Imap4)
                            {
                                globalIncProtocol = "1";
                            }
                            else
                            {
                                globalIncProtocol = "0";
                            }
                            globalIncPort = settings.IncomingMailPort.ToString();
                            globalOutServer = settings.OutgoingMailServer;
                            globalOutPort = settings.OutgoingMailPort.ToString();
                            if (settings.ReqSmtpAuth)
                            {
                                globalUseSmtpAuth = "1";
                            }
                            else
                            {
                                globalUseSmtpAuth = "0";
                            }
                        }


						try
						{
							Account acct = Account.LoginAccount(globalEmail, globalLogin, globalPassword, globalIncServer, (IncomingMailProtocol) Convert.ToInt32(globalIncProtocol), Convert.ToInt32(globalIncPort), globalOutServer, Convert.ToInt32(globalOutPort), ((string.Compare(globalUseSmtpAuth, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false), (string.Compare(globalSignMe, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false, (string.Compare(globalAdvancedLogin, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false);

                            if (acct != null)
                            {
                                if (globalSignMe == "1")
                                {
                                    HttpCookie idAcctCookie = new HttpCookie("awm_autologin_id");
                                    idAcctCookie.Value = acct.ID.ToString();
                                    idAcctCookie.Path = HttpContext.Current.Request.ApplicationPath;
                                    Response.AppendCookie(idAcctCookie);

                                    string hash = Utils.GetMD5DigestHexString(Utils.EncodePassword(acct.MailIncomingPassword));
                                    HttpCookie autoLoginCookie = new HttpCookie("awm_autologin_data");
                                    autoLoginCookie.Value = hash;
                                    autoLoginCookie.Path = HttpContext.Current.Request.ApplicationPath;
                                    Response.AppendCookie(autoLoginCookie);
                                }

                                Session.Add(Constants.sessionAccount, acct);
                                if (acct.GetMailAtLogin)
                                {
                                    Response.Redirect("basewebmail.aspx?check=1", false);
                                }
                                else
                                {
                                    Response.Redirect("basewebmail.aspx", false);
                                }
                            }
						}
						catch(Exception message)
						{
							Log.WriteLine("Page_Load", "Login Account");
							Log.WriteException(message);
							errorDesc = message.Message;
							errorClass = "wm_login_error"; //if the error was occured
						}
						break;
					default:
						DisplayStandardMode();
						break;
				}
				//end for version without ajax

				string error = Request["error"];
				if (error == null) error = string.Empty;
				if (error == "1") 
				{
					errorDesc = "The previous session was terminated due to a timeout.";
					errorClass = "wm_login_error"; //if the error was occured
				} 
				else 
				{
					HttpCookie idAcctCookie = new HttpCookie("awm_autologin_id");
					idAcctCookie = Request.Cookies["awm_autologin_id"];
					HttpCookie autoLoginCookie = new HttpCookie("awm_autologin_data");
					autoLoginCookie = Request.Cookies["awm_autologin_data"];
					if ((idAcctCookie != null) && (autoLoginCookie != null))
					{
						int id_acct = -1;
						try
						{
							id_acct = int.Parse(idAcctCookie.Value);
						}
						catch (Exception ex)
						{
							Log.WriteLine("Page_Load", "int.Parse");
							Log.WriteException(ex);
						}
						Account acct = Account.LoadFromDb(id_acct, -1);
						if (acct != null)
						{
							string encodePassword = Utils.GetMD5DigestHexString(Utils.EncodePassword(acct.MailIncomingPassword));
							if (string.Compare(encodePassword, autoLoginCookie.Value, true, CultureInfo.InvariantCulture) == 0)
							{
								Session.Add(Constants.sessionAccount, acct);
								idAcctCookie.Expires = DateTime.Now.AddDays(14);
								if (settings.AllowAjax)
								{
									Response.Redirect("webmail.aspx", false);
								}
								else
								{
									Response.Redirect("basewebmail.aspx", false);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.WriteException(ex);
				throw;
			}
		}

		protected void DisplayStandardMode()
		{
			switcherHref = "?mode=advanced";
			switcherText = _resMan.GetString("AdvancedLogin");
			advancedClass = @" class=""wm_hide""";
			advancedLogin = "0";
			if (settings.HideLoginMode == LoginMode.HideEmailField ||
				settings.HideLoginMode == LoginMode.HideEmailFieldDisplayDomainAfterLogin ||
				settings.HideLoginMode == LoginMode.HideEmailFieldDisplayDomainAfterLoginAndLoginIsLoginAndDomain ||
				settings.HideLoginMode == LoginMode.HideEmailFieldLoginIsLoginAndDomain)
			{
				emailClass = @" class=""wm_hide""";
			}
			if (settings.HideLoginMode == LoginMode.HideLoginFieldLoginIsAccount ||
				settings.HideLoginMode == LoginMode.HideLoginFieldLoginIsEmail)
			{
				loginClass = @" class=""wm_hide""";
			}
			if (settings.HideLoginMode == LoginMode.HideEmailFieldDisplayDomainAfterLogin ||
				settings.HideLoginMode == LoginMode.HideEmailFieldDisplayDomainAfterLoginAndLoginIsLoginAndDomain)
			{
				loginWidth = "150px";
				domainContent = "&nbsp;@" + defaultDomainOptional;
			}
		}

		protected void DisplayAdvancedMode()
		{
			switcherHref = "?mode=standard";
			switcherText = _resMan.GetString("StandardLogin");
			advancedClass = "";
			advancedLogin = "1";
			emailClass = "";
			loginClass = "";
			loginWidth = "220px";
			domainContent = "";
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
