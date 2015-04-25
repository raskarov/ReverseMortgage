using System;
using System.Collections;
using System.ComponentModel;
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

namespace WebMailPro
{
	/// <summary>
	/// Summary description for langs_js.
	/// </summary>
	public partial class langs_js : System.Web.UI.Page
	{
		protected WebmailResourceManager _resMan = null;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here

            string lang = Request.QueryString.Get("lang");
            if (null == lang)
            {
                _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
            }
            else
            {
                _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager(lang);
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

		protected string OutputLangsConsts()
		{
			StringBuilder sb = new StringBuilder();
            
			if ((_resMan != null) && (_resMan.Manager != null))
			{
				IDictionaryEnumerator enumerator = _resMan.Manager.GetResourceSet(_resMan.Culture, true, true).GetEnumerator();
				while (enumerator.MoveNext())
				{
					sb.AppendFormat(@"	{0}: '{1}',
", enumerator.Key, Utils.EncodeJsSaveString(enumerator.Value.ToString()));
				}
				if (sb.Length > 3)
				{
					sb.Remove(sb.Length - 3, 3); // remove last ",\r\n"
				}
			}

			return sb.ToString();
		}
	}
}
