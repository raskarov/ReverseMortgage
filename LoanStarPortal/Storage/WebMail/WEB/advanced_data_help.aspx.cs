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
	/// Summary description for advanced_data_help.
	/// </summary>
	public partial class advanced_data_help : System.Web.UI.Page
	{
		protected WebmailResourceManager _resMan = null;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
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
