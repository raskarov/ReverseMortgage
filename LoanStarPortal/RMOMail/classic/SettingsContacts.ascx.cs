using System.Globalization;

namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for SettingsContacts.
	/// </summary>
	public partial class SettingsContacts : System.Web.UI.UserControl
	{
		protected WebmailResourceManager _resMan = null;
		protected string _skin = Constants.defaultSkinName;
		protected jsbuilder _js;
		public jsbuilder js
		{
			get { return _js; }
			set { _js = value; }
		}

		public string Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			_js.AddText(@"
			function CheckCPPField()
			{
				var oVal = new CValidate();
				if (!oVal.IsPositiveNumber(document.getElementById('"+textBoxContactsPerPage.ClientID+@"').value))
				{
					alert(Lang.WarningContactsPerPage);
					return false;
				}
				return true;
			}
			");
			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			InitControls();

			Account acct = Session[Constants.sessionAccount] as Account;
			if ((acct != null) && (acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
			{
				this.textBoxContactsPerPage.Value = acct.UserOfAccount.Settings.ContactsPerPage.ToString(CultureInfo.InvariantCulture);
			}
		}

		private void InitControls()
		{
			saveButton.Value = _resMan.GetString("Save");
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

		protected void saveButton_ServerClick(object sender, System.EventArgs e)
		{
			try
			{
				Account acct = Session[Constants.sessionAccount] as Account;
				if ((acct != null) && (acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
				{
					BaseWebMailActions actions = new BaseWebMailActions(acct, this.Page);
					short contactsPerPage = 10;

					if (Validation.CheckIt(Validation.ValidationTask.CPP, this.textBoxContactsPerPage.Value))
					{
						try
						{
							contactsPerPage = short.Parse(Validation.Corrected);
						}
						catch
						{
							contactsPerPage = acct.UserOfAccount.Settings.ContactsPerPage;	
						}
					}
					else
					{
						contactsPerPage = acct.UserOfAccount.Settings.ContactsPerPage;	
					}
					actions.UpdateContactsSettings(true, contactsPerPage);

					Session[Constants.sessionReportText] = _resMan.GetString("ReportContactsSettingsUpdatedSuccessfuly");

					Session[Constants.sessionAccount] = acct;
				}
			} 				
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}
	}
}
