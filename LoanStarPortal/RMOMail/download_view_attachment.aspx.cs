using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for download_view_attachment.
	/// </summary>
	public partial class download_view_attachment : System.Web.UI.Page
	{
		Account acct = null;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			acct = Session[Constants.sessionAccount] as Account;
			if (acct == null) 
			{
				Response.Redirect("default.aspx");
			}

			string userAgent; //Client browser
			string temp_filename = Request.QueryString["temp_filename"];
			if (temp_filename != null)
			{
				try
				{
					byte[] buffer = new byte[0];
					object tempFolder = Utils.GetTempFolderName(this.Session);
					if (tempFolder != null)
					{
						//Response.Write(tempFolder.ToString() + "<br>");
						//Response.Write(temp_filename);
						//Response.End();
						string safe_temp_file_name=Path.GetFileName(temp_filename);

						string fullPath = Path.Combine(tempFolder.ToString(), safe_temp_file_name);
						if (File.Exists(fullPath))
						{
							using(FileStream fs = File.OpenRead(fullPath))
							{
								buffer = new byte[fs.Length];
								fs.Read(buffer, 0, buffer.Length);
							}
						}
					}
					else
					{
						return;
					}
					string filename = (Request.QueryString["filename"] != null) ? Request.QueryString["filename"] : temp_filename;
					string download = Request.QueryString["download"];
					//*************************************************************
					//IE with cyrillic file names
					//*************************************************************
					string encodedFilename = string.Empty;
					userAgent = Request.UserAgent;
					if (userAgent.IndexOf("MSIE") > -1) 
					{
						encodedFilename = Server.UrlPathEncode(filename);
					}
					else
					{
						encodedFilename = filename;
					}
					//**************************************************************
					if (download != null)
					{
						Response.Clear();

						if (string.Compare(download, "1", true, CultureInfo.InvariantCulture) == 0)
						{
							Response.AddHeader("Content-Disposition", @"attachment; filename="""+encodedFilename+@"""");
							Response.AddHeader("Accept-Ranges", "bytes");
							Response.AddHeader("Content-Length", buffer.Length.ToString(CultureInfo.InvariantCulture));
							Response.AddHeader("Content-Transfer-Encoding", "binary");
							Response.ContentType = "application/octet-stream";
						}
						else
						{
							string ext = Path.GetExtension(filename);
							if ((ext != null) && (ext.Length > 0))
							{
								ext = ext.Substring(1, ext.Length - 1); // remove first dot
							}
							Response.ContentType = Utils.GetAttachmentMimeTypeFromFileExtension(ext);
						}
					}
					Response.BinaryWrite(buffer);
					Response.Flush();
				}
				catch(Exception ex)
				{
					Log.WriteException(ex);
				}
			}
			else
			{
				if ((Request.QueryString["id_msg"] != null) 
					&& (Request.QueryString["uid"] != null)
					&& (Request.QueryString["id_folder"] != null)
					&& (Request.QueryString["folder_path"] != null))
				{
					try
					{
						int id_msg = int.Parse(Request.QueryString["id_msg"], CultureInfo.InvariantCulture);
						long id_folder = long.Parse(Request.QueryString["id_folder"], CultureInfo.InvariantCulture);

						WebMailMessage msg = null;
						MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(Session[Constants.sessionAccount] as Account));
						try
						{
							mp.Connect();
							Folder fld = mp.GetFolder(id_folder);
							if (fld != null)
							{
								msg = mp.GetMessage((fld.SyncType != FolderSyncType.DirectMode) ? (object)id_msg : HttpUtility.UrlDecode(Request.QueryString["uid"]), fld);
							}
						}
						finally
						{
							mp.Disconnect();
						}

						if (msg.MailBeeMessage != null)
						{
							string subj = msg.MailBeeMessage.Subject;
							//�\�, �/�, �?�, �|�, �*�, �<�, �>�, �:�
                            string safeSubject = string.Empty;
							for(int i = 0; i < subj.Length; i++)
							{
                                if (subj[i] == '\\' || subj[i] == '|' || subj[i] == '/' || 
                                    subj[i] == '?' || subj[i] == '*' || subj[i] == '<' || 
                                    subj[i] == '>' || subj[i] == ':')
                                {
                                    continue;
                                }
                                else
                                {
                                    safeSubject += subj[i];
                                }
							}
                            safeSubject = safeSubject.TrimStart();
                            if (safeSubject.Length > 30)
                            {
                                safeSubject = safeSubject.Substring(0, 30).TrimEnd();
                            }
                            safeSubject = safeSubject.TrimEnd(new char[2] { '.', ' ' });
                            if (safeSubject.Length == 0)
                            {
                                safeSubject = "message";
                            }

                            string encodedMsgFilename = string.Empty;
                            userAgent = Request.UserAgent;
                            if (userAgent.IndexOf("MSIE") > -1) 
							{
                                encodedMsgFilename = Server.UrlPathEncode(safeSubject);
                                Response.AddHeader("Expires", "0");
                                Response.AddHeader("Cache-Control", "must-revalidate, post-check=0, pre-check=0");
                                Response.AddHeader("Pragma", "public");
                            }
							else 
							{
                                encodedMsgFilename = safeSubject;
							}
							//**************************************************************
                            byte[] buffer = msg.MailBeeMessage.GetMessageRawData();
                            //**************************************************************
                            Response.Clear();
							Response.ContentType = "application/octet-stream";
							Response.AddHeader("Accept-Ranges", "bytes");
							Response.AddHeader("Content-Length", buffer.Length.ToString(CultureInfo.InvariantCulture));
							Response.AddHeader("Content-Disposition", string.Format(@"attachment; filename=""{0}.eml""", encodedMsgFilename));
							Response.AddHeader("Content-Transfer-Encoding", "binary");
							Response.BinaryWrite(buffer);
							Response.Flush();
						}
					} 
					catch(Exception ex)
					{
						Log.WriteException(ex);
					}
				}
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