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
	/// Summary description for check_mail.
	/// </summary>
	public partial class check_mail : System.Web.UI.Page
	{
		protected int Type = 0;
		protected string folderName = string.Empty;
		protected string errorDesc = string.Empty;
		protected int msgsCount = 3;
		protected int msgNumber = 0;
		protected string accountName = string.Empty;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Type = int.Parse(Request["Type"]);
			}
			catch( ArgumentNullException )
			{
				Type = 0;
			}
			catch( FormatException )
			{
				Type = 0;
			}
			catch( OverflowException )
			{
				Type = 0;
			}
            
			Account acct = Session[Constants.sessionAccount] as Account;
			if (acct != null)
			{
				if (Type == 1)
				{
					if (acct.UserOfAccount != null)
					{
						Account[] accounts = acct.UserOfAccount.GetUserAccounts();
						foreach (Account account in accounts)
						{
							if (account.GetMailAtLogin)
							{
								accountName = account.Email;
								Response.Write(@"<script type=""text/javascript"">
									parent.SetCheckingAccountHandler('" + accountName + "');</script>");
								Response.Flush();
								CheckMailForAccount(account);
							}
						}
					}
				}
				else if (Type == 0)
				{
					CheckMailForAccount(acct);
				}
			}
			else
			{
				errorDesc = "session_error";
			}
		}

		protected void CheckMailForAccount(Account acct)
		{
			if (acct != null)
			{
				try
				{
					DbStorage dbs = DbStorageCreator.CreateDatabaseStorage(acct);
					MailProcessor mp = new MailProcessor(dbs);
					try
					{
						mp.MessageDownloaded += new DownloadedMessageHandler(mp_MessageDownloaded);
						Response.Write(@"<script type=""text/javascript"">parent.SetStateTextHandler(""Logging on to server..."");</script>");
						mp.Connect();
						Response.Write(@"<script type=""text/javascript"">parent.SetStateTextHandler(""Getting number of messages"");</script>");
						mp.Synchronize(dbs.GetFolders());
					}
					finally
					{
						mp.MessageDownloaded -= new DownloadedMessageHandler(mp_MessageDownloaded);
						Response.Write(@"<script type=""text/javascript"">parent.SetStateTextHandler(""Logging off from server"");</script>");
						mp.Disconnect();
					}
				}
				catch (WebMailException ex)
				{
					Log.WriteException(ex);
					errorDesc = Utils.EncodeJsSaveString(ex.Message);
					if (Type == 1)
					{
						Session.Add(Constants.sessionErrorText, errorDesc);
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

		private void mp_MessageDownloaded(object sender, CheckMailEventArgs e)
		{
			folderName = e.FolderName;
			msgNumber = e.MsgsNumber;
			msgsCount = e.MsgsCount;
			Response.Write(@"<script type=""text/javascript"">
				parent.SetCheckingFolderHandler('" + folderName + "', " + msgsCount + ");</script>");
			Response.Flush();
			Response.Write(@"<script type=""text/javascript"">
				parent.SetRetrievingMessageHandler(" + msgNumber + ");</script>");
			Response.Flush();
			Log.WriteLine("mp_MessageDownloaded", string.Format("Folder: '{0}'; MessageCount: {1}; MessageNumber: {2}", folderName, msgsCount, msgNumber));
		}
	}
}
