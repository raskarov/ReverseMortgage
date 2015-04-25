using System;
using System.IO;
using System.Web;
using System.Web.UI;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for upload.
	/// </summary>
	public partial class upload : Page
	{
		protected bool errorOccured = false;
		protected string name = string.Empty;
		protected string tmp_name = string.Empty;
		protected int size = 0;
		protected string mime_type = string.Empty;
		protected string error = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

			HttpFileCollection files = Request.Files;
			if ((files != null) && (files.Count > 0))
			{
				HttpPostedFile file = files[0];
				if (file != null)
				{
					name = Utils.EncodeHtml(Path.GetFileName(file.FileName)).Replace(@"'", @"\'");
					size = file.ContentLength;
					mime_type = Utils.EncodeHtml(file.ContentType);
					if ((file.ContentLength < settings.AttachmentSizeLimit) || (settings.EnableAttachmentSizeLimit == false))
					{
						try
						{
							byte[] buffer = null;
							using (Stream uploadStream = file.InputStream)
							{
								buffer = new byte[uploadStream.Length];
								long numBytesToRead = uploadStream.Length;
								long numBytesRead = 0;
								while (numBytesToRead > 0)
								{
									int n = uploadStream.Read(buffer, (int)numBytesRead, (int)numBytesToRead);
									if (n==0)
										break;
									numBytesRead += n;
									numBytesToRead -= n;
								}
							}
							if (buffer != null)
							{
								string filename = Utils.CreateTempFilePath(Utils.GetTempFolderName(this.Session), file.FileName);
								tmp_name = Path.GetFileName(filename);
								using (FileStream fs = File.Open(filename, FileMode.Create, FileAccess.Write))
								{
									fs.Write(buffer, 0, buffer.Length);
								}
							}
						}
						catch (IOException ex)
						{
							errorOccured = true;
							error = ex.Message;
						}
					}
					else
					{
						errorOccured = true;
						error = (new WebmailResourceManagerCreator()).CreateResourceManager().GetString("FileIsTooBig");
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
			this.Load += new EventHandler(this.Page_Load);
		}
		#endregion
	}
}
