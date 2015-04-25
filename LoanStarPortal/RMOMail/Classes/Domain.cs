using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MailBee.Mime;


namespace WebMailPro
{
    public class Domain
    {
        #region Fields
        protected int _id;
        protected string _name;
        protected IncomingMailProtocol _mailProtocol;
        protected string _mailIncHost;
        protected int _mailIncPort;
        protected string _mailOutHost;
        protected int _mailOutPort;
        protected bool _mailOutAuthentication;
        #endregion        
    
        #region Properties
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public IncomingMailProtocol MailIncomingProtocol
        {
            get { return _mailProtocol; }
            set { _mailProtocol = value; }
        }

        public string MailIncomingHost
        {
            get { return _mailIncHost; }
            set { _mailIncHost = value; }
        }

        public int MailIncomingPort
        {
            get { return _mailIncPort; }
            set { _mailIncPort = value; }
        }

        public string MailOutgoingHost
        {
            get { return _mailOutHost; }
            set { _mailOutHost = value; }
        }

        public int MailOutgoingPort
        {
            get { return _mailOutPort; }
            set { _mailOutPort = value; }
        }

        public bool MailOutgoingAuthentication
        {
            get { return _mailOutAuthentication; }
            set { _mailOutAuthentication = value; }
        }
        #endregion
    
		public Domain()
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			_id = -1;
			_name = "";
			_mailProtocol = settings.IncomingMailProtocol;
			_mailIncHost = settings.IncomingMailServer;
			_mailIncPort = settings.IncomingMailPort;
			_mailOutHost = settings.OutgoingMailServer;
			_mailOutPort = settings.OutgoingMailPort;
			_mailOutAuthentication = settings.ReqSmtpAuth;
		}

        public Domain(int id_account, string name, IncomingMailProtocol mail_protocol, string mail_inc_host, int mail_inc_port, string mail_out_host, int mail_out_port, bool mail_out_auth)
		{
			_id = id_account;
			_name = name;
			_mailProtocol = mail_protocol;
			_mailIncHost = mail_inc_host;
			_mailIncPort = mail_inc_port;
			_mailOutHost = mail_out_host;
			_mailOutPort = mail_out_port;
			_mailOutAuthentication = mail_out_auth;
		}

    }
}
