namespace WebMailPro.MailAdm
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for mailadm_menu.
	/// </summary>
	public partial class mailadm_menu : System.Web.UI.UserControl
	{
		protected int _selectedScreen = 0;
		protected string defaultSkin = "";

		public int Screen
		{
			get { return _selectedScreen; }
			set { _selectedScreen = value; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			defaultSkin = ((mailadm)Page).Settings.DefaultSkin;
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
	}
}
