using System;
using System.Collections;
using System.Globalization;
using System.Web.UI;
using System.Xml;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for xml_processing.
	/// </summary>
	public partial class xml_processing : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			ArrayList arr = new ArrayList();

			string xmlText = Request["xml"];

			if (arr.Count > 0) xmlText = (string)arr[0];
			if (xmlText != null)
			{
				Log.WriteLine("", ">>>>>>>>>>>>>>>>  IN  >>>>>>>>>>>>>>>>");
				Log.WriteLine("", xmlText);
				Log.WriteLine("", ">>>>>>>>>>>>>>>>  IN  >>>>>>>>>>>>>>>>");

				Account acct = Session[Constants.sessionAccount] as Account;

				XmlPacketManager manager = new XmlPacketManager(acct, this);
				XmlPacket packet = manager.ParseClientXmlText(xmlText);

				if (Session[Constants.sessionErrorText] != null)
				{
					manager.ErrorFromSession = Session[Constants.sessionErrorText] as String;
					Session.Remove(Constants.sessionErrorText);
				}

                bool dontSend = false;
                if (packet.action == "send" && packet.request == "message" && acct != null && acct.IsDemo)
                {
                    if (Session["sendCount"] != null)
                    {
                        int sendCount = 0;
                        try
                        {
                            sendCount = int.Parse((string)Session["sendCount"]);
                        }
                        catch {}
                        if (sendCount >= 3)
                        {
                            dontSend = true;
                        }
                        else
                        {
                            Session["sendCount"] = (sendCount + 1).ToString();
                        }
                    }
                    else
                    {
                        Session.Add("sendCount", "1");
                    }
                }

                XmlDocument doc;
                if (dontSend)
                {
                    doc = new XmlDocument();
                    doc.PreserveWhitespace = true;
                    XmlDeclaration xmlDecl = doc.CreateXmlDeclaration("1.0", "utf-8", "");
                    doc.AppendChild(xmlDecl);

                    XmlElement webmailNode = doc.CreateElement("webmail");
                    doc.AppendChild(webmailNode);
                    XmlElement infoElem = webmailNode.OwnerDocument.CreateElement("information");
                    infoElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml("To prevent abuse, no more than 3 e-mails per session is allowed in this demo. To send more e-mails, start another session.")));
                    webmailNode.AppendChild(infoElem);
                }
                else
                {
                    doc = manager.CreateServerXmlDocumentResponse(packet);
                }

                if (Session[Constants.sessionAccount] == null)
				{
					Session.Add(Constants.sessionAccount, manager.CurrentAccount);
					int idUser = -1;
					if (manager.CurrentAccount != null) idUser = manager.CurrentAccount.IDUser;
					if (Session[Constants.sessionUserID] == null)
					{
						Session.Add(Constants.sessionUserID, idUser);
					}
					else
					{
						Session[Constants.sessionUserID] = idUser;
					}
				}
				else
				{
					if (manager.CurrentAccount != null)
					{
						if (!manager.CurrentAccount.Equals(Session[Constants.sessionAccount]))
						{
							Session[Constants.sessionAccount] = manager.CurrentAccount;
							if (Session[Constants.sessionUserID] == null)
							{
								Session.Add(Constants.sessionUserID, manager.CurrentAccount.IDUser);
							}
							else
							{
								Session[Constants.sessionUserID] = manager.CurrentAccount.IDUser;
							}
						}
					}
					else
					{
						Session[Constants.sessionAccount] = null;
					}
				}

				Response.Clear();
				Response.ContentType = @"text/xml";
				Log.WriteLine("", "<<<<<<<<<<<<<<<<<  OUT  <<<<<<<<<<<<<<<");
				Log.WriteLine("", doc.OuterXml);
				Log.WriteLine("", "<<<<<<<<<<<<<<<<<  OUT  <<<<<<<<<<<<<<<");
				Response.Write(doc.OuterXml);
				Response.End();
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
			this.Load += new EventHandler(this.Page_Load);
		}
		#endregion
	}
}
