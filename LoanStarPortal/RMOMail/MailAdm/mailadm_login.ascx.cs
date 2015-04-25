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
	///		Summary description for mailadm_login.
	/// </summary>
	public partial class mailadm_login : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
           // Put user code to initialize the page here
            if (Request.QueryString.Get("login") != null)
                this.login.Value = Request.QueryString.Get("login").ToString();

            if (Request.QueryString.Get("password") != null)
                this.password.Value = Request.QueryString.Get("password").ToString();

            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
            if ((string.Compare(password.Value, settings.AdminPassword, false, CultureInfo.InvariantCulture) == 0) && (string.Compare(login.Value, Constants.mailadmLogin, false, CultureInfo.InvariantCulture) == 0))
            {
                Session.Add("mailadm_login", true);
                if (Request.QueryString.Get("showmenu") != null)
                    Response.Redirect("mailadm.aspx?mode=users&showmenu=true");
                else
                    Response.Redirect("mailadm.aspx?mode=users");
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

		protected void enter_Click(object sender, System.EventArgs e)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			if ((string.Compare(password.Value, settings.AdminPassword, false, CultureInfo.InvariantCulture) == 0) && (string.Compare(login.Value, Constants.mailadmLogin, false, CultureInfo.InvariantCulture) == 0))
			{
				Session.Add("mailadm_login", true);
				Response.Redirect("mailadm.aspx?mode=db");
			}
		}
	}
}
