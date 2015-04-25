using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for webmail.
	/// </summary>
	public partial class webmail : System.Web.UI.Page
	{
		protected WebmailResourceManager _resMan = null;

		protected string defaultTitle = "MailBee WebMail Pro 4.2";
		protected string defaultSkin = Constants.defaultSkinName;
		protected string defaultLang = Constants.defaultLang;
		protected string check = "0";
		protected string start = "0";
		protected string to = string.Empty;
        Account acct = null;

		protected string jsClearDefaultTitle = string.Empty;
		protected string jsClearDefaultSkin = string.Empty;
		protected string jsClearStart = string.Empty;
		protected string jsClearToAddr = string.Empty;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            acct = (Account)Session[Constants.sessionAccount];
            if (acct == null)
            {
                Response.Redirect("default.aspx?error=1");
            }
            
            if (Request.QueryString.Get("acct") != null)
            {
                ChangeAccount(Request.QueryString.Get("acct"));
            }

            if (Request.QueryString.Get("check") != null)
			{
				check = Request.QueryString.Get("check");
			}
			if (Request.QueryString.Get("start") != null)
			{
				start = Request.QueryString.Get("start");
			}
			if (Request.QueryString.Get("to") != null)
			{
                to = Request.QueryString.Get("to");
			}
			jsClearStart = Utils.EncodeJsSaveString(start);
			jsClearToAddr = Utils.EncodeJsSaveString(to);
			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();

			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

            if (acct != null)
			{
				if ((acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
				{
					defaultTitle = settings.SiteName;
					defaultSkin = acct.UserOfAccount.Settings.DefaultSkin;
					defaultLang = acct.UserOfAccount.Settings.DefaultLanguage;
					jsClearDefaultTitle = Utils.EncodeJsSaveString(defaultTitle);
					jsClearDefaultSkin = Utils.EncodeJsSaveString(defaultSkin);
					return;
				}
			}
			defaultTitle = settings.SiteName;
			defaultSkin = settings.DefaultSkin;
			defaultLang = settings.DefaultLanguage;
			jsClearDefaultTitle = Utils.EncodeJsSaveString(defaultTitle);
			jsClearDefaultSkin = Utils.EncodeJsSaveString(defaultSkin);
		}

        protected void ChangeAccount(string accountID)
        {
            try
            {
                BaseWebMailActions bwa = new BaseWebMailActions(acct, this.Page);

                Account NewAcct = bwa.GetAccount(Convert.ToInt32(accountID));

                if (NewAcct != null)
                {
                    Session.Add(Constants.sessionAccount, NewAcct);
                    acct = NewAcct;
                }
            }
            catch (Exception ex)
            {
                ((basewebmail)this.Page).OutputException(ex);
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
