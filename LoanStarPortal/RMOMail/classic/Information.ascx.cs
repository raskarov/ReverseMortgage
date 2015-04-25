namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for Information.
	/// </summary>
	public partial class Information : System.Web.UI.UserControl
	{
		protected string _skin = Constants.defaultSkinName;
		protected WebmailResourceManager _manager = null;
		
		public string Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			_manager = (new WebmailResourceManagerCreator()).CreateResourceManager();
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
