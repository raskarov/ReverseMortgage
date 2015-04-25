using System;
using System.IO;
using System.Text;
using System.Web;
using MailBee;
using MailBee.Mime;
using MailBee.Security;
using MailBee.SmtpMail;
using MailBeeSmtp = MailBee.SmtpMail;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for Smtp.
	/// </summary>
	public class Smtp
	{
		// true if successfull, otherwise false
		public static void SendMail(Account account, WebMailMessage message, string from, string to)
		{
			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			try
			{
				MailMessage messageToSend = new MailMessage();
				messageToSend.Priority = message.MailBeeMessage.Priority;
				messageToSend.From = message.MailBeeMessage.From;
				messageToSend.To = message.MailBeeMessage.To;
				messageToSend.Cc = message.MailBeeMessage.Cc;
				messageToSend.Bcc = message.MailBeeMessage.Bcc;
				messageToSend.Subject = message.MailBeeMessage.Subject;
				messageToSend.Date = DateTime.Now;
				messageToSend.BodyHtmlText = message.MailBeeMessage.BodyHtmlText;
				messageToSend.BodyPlainText = message.MailBeeMessage.BodyPlainText;

				string str = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
				messageToSend.Headers.Add("X-Originating-IP", str, false);

				foreach (Attachment attach in message.MailBeeMessage.Attachments)
				{
					messageToSend.Attachments.Add(attach);
				}

				WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				MailBee.SmtpMail.Smtp.LicenseKey = settings.LicenseKey;
				MailBee.SmtpMail.Smtp objSmtp = new MailBee.SmtpMail.Smtp();
				if (settings.EnableLogging)
				{
					objSmtp.Log.Enabled = true;
					string dataFolderPath = Utils.GetDataFolderPath();
					if (dataFolderPath != null)
					{
						objSmtp.Log.Filename = Path.Combine(dataFolderPath, Constants.logFilename);
					}
				}

				SmtpServer server = new SmtpServer();
				server.Name = ((account.MailOutgoingHost != null) && (account.MailOutgoingHost.Length > 0)) ? account.MailOutgoingHost : account.MailIncomingHost;
				server.Port = account.MailOutgoingPort;
				server.AccountName = ((account.MailOutgoingLogin != null) && (account.MailOutgoingLogin.Length > 0)) ? account.MailOutgoingLogin : account.MailIncomingLogin;
				server.Password = ((account.MailOutgoingPassword != null) && (account.MailOutgoingPassword.Length > 0)) ? account.MailOutgoingPassword : account.MailIncomingPassword;
				if (account.MailOutgoingAuthentication)
				{
					server.AuthMethods = AuthenticationMethods.Auto;
					server.AuthOptions = AuthenticationOptions.PreferSimpleMethods;
				}
				if (server.Port == 465)
				{
					server.SslMode = SslStartupMode.OnConnect;
					server.SslProtocol = SecurityProtocol.Auto;
					server.SslCertificates.AutoValidation = CertificateValidationFlags.None;
				}

				objSmtp.SmtpServers.Add(server);

				Encoding outgoingCharset = Utils.GetEncodingByCodePage(account.UserOfAccount.Settings.DefaultCharsetOut);
				messageToSend.Charset = outgoingCharset.HeaderName;

				objSmtp.Message = messageToSend;

				try
				{
					objSmtp.Connect();

					objSmtp.Send();
				}
				catch (MailBeeConnectionException)
				{
					throw new WebMailException(resMan.GetString("ErrorSMTPConnect"));
				}
				catch (MailBeeSmtpLoginBadCredentialsException)
				{
					throw new WebMailException(resMan.GetString("ErrorSMTPAuth"));
				}
				catch (MailBeeException ex)
				{
					throw new WebMailMailBeeException(ex);
				}
				finally
				{
					if (objSmtp.IsConnected) objSmtp.Disconnect();
				}
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
		}

        public static void SendMail(string from, string to, string subject, string body, string smtpHost, int smtpPort, string login, string password, bool smtpAuth)
        {
            WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
            try 
            {
				MailMessage messageToSend = new MailMessage();
                messageToSend.From = new EmailAddress(from);
				messageToSend.To.Add(new EmailAddress(to)); 
				messageToSend.Subject = subject;
                messageToSend.BodyPlainText = body;
				messageToSend.Date = DateTime.Now;

				string str = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
				messageToSend.Headers.Add("X-Originating-IP", str, false);

				WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				MailBee.SmtpMail.Smtp.LicenseKey = settings.LicenseKey;
				MailBee.SmtpMail.Smtp objSmtp = new MailBee.SmtpMail.Smtp();

				SmtpServer server = new SmtpServer();
                server.Name = smtpHost;
				server.Port = smtpPort;
                server.AccountName = login;
                server.Password = password;
				if (smtpAuth)
				{
					server.AuthMethods = AuthenticationMethods.Auto;
					server.AuthOptions = AuthenticationOptions.PreferSimpleMethods;
				}
				if (server.Port == 465)
				{
					server.SslMode = SslStartupMode.OnConnect;
					server.SslProtocol = SecurityProtocol.Auto;
					server.SslCertificates.AutoValidation = CertificateValidationFlags.None;
				}

				objSmtp.SmtpServers.Add(server);

				objSmtp.Message = messageToSend;

				try
				{
					objSmtp.Connect();

					objSmtp.Send();
				}
				catch (MailBeeConnectionException)
				{
					throw new WebMailException(resMan.GetString("ErrorSMTPConnect"));
				}
				catch (MailBeeSmtpLoginBadCredentialsException)
				{
					throw new WebMailException(resMan.GetString("ErrorSMTPAuth"));
				}
				catch (MailBeeException ex)
				{
					throw new WebMailMailBeeException(ex);
				}
				finally
				{
					if (objSmtp.IsConnected) objSmtp.Disconnect();
				}
			}
			catch (MailBeeException ex)
			{
				throw new WebMailMailBeeException(ex);
			}
        }
	}// END CLASS DEFINITION Smtp
}
