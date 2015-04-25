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

namespace WebMailPro {
	/// <summary>
	/// Summary description for msgprint.
	/// </summary>
	public partial class msgprint : System.Web.UI.Page {
		protected WebmailResourceManager _manager = null;
		protected WebMailMessage msg = null;
		Account acct = null;
		protected bool IsHTMLMsg = false;
		protected string msgPlain = null;
		protected string msgHTML = null;

		protected void Page_Load(object sender, System.EventArgs e) {
			bool showPicturesSettings = false;
			acct = Session[Constants.sessionAccount] as Account;
			if (acct == null) {
				Response.Redirect("default.aspx");
			}
			else {
				_manager = (new WebmailResourceManagerCreator()).CreateResourceManager();
				int id = (int)Session["id"];
				string uid = (string)Session["uid"];
				long id_folder = (long)Session["id_folder"];
				string full_name_folder = (string)Session["full_name_folder"];
				int charset = (int)Session["charset"];
				if ((acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null)) {
					showPicturesSettings = ((acct.UserOfAccount.Settings.ViewMode & ViewMode.AlwaysShowPictures) > 0) ? true : false;
				}

				try {
					IsHTMLMsg = false;
					msgPlain = null;
					msgHTML = null;
					BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);
					if (showPicturesSettings)
						msg = bwml.GetMessage(id, uid, id_folder, full_name_folder, charset, 1);
					else
						msg = bwml.GetMessage(id, uid, id_folder, full_name_folder, charset);
					LabelFrom.Text = Server.HtmlEncode(msg.FromMsg.ToString());
					LabelTo.Text = Server.HtmlEncode(msg.ToMsg.ToString());
					LabelDate.Text = Utils.GetDateWithOffsetFormatString(acct, msg.MsgDate);
					LabelSubject.Text = Server.HtmlEncode(msg.Subject);
					msgPlain = Utils.MakeHtmlBodyFromPlainBody(msg.MailBeeMessage.BodyPlainText);
					msgHTML = msg.MailBeeMessage.BodyHtmlText;
					if(msgPlain == " ") {
						msgPlain = Utils.ConvertHtmlToPlain(msgHTML);
					}
					if(msgHTML != "") {
						IsHTMLMsg = true;
						//Message body
						MessageBody.Text= msgHTML;
					}
					else {
						IsHTMLMsg = false;
						MessageBody.Text = msgPlain;
					}
				}
				catch (Exception ex) {
					Log.WriteException(ex);
				}
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
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
		private void InitializeComponent() {    
		}
		#endregion
	}
}
