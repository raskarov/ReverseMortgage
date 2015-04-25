namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for Folders_part.
	/// </summary>
	public partial class Folders_part : System.Web.UI.UserControl
	{
		protected long _selectedFolderID = -1;
		protected string _skin = Constants.defaultSkinName;
		protected FolderCollection _folderCollection = new FolderCollection();
		protected WebmailResourceManager _manager = null;
		protected string FoldersList = null;
		protected int _sync = -1;

		public int Sync
		{
			get { return _sync; }
			set { _sync = value; }
		}

		public string Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}

		public long selectedFolderID
		{
			get
			{
				return _selectedFolderID;
			}
			set
			{
				_selectedFolderID = value;
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				_manager = (new WebmailResourceManagerCreator()).CreateResourceManager();

				if(!IsPostBack)
				{
					OutputFoldersTree();
				}

			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion

		public void OutputFoldersTree()
		{
			try
			{
				bool isImapAccount = false;
				Account acct = Session[Constants.sessionAccount] as Account;
				if (acct != null)
				{
					BaseWebMailActions actions = new BaseWebMailActions(acct, this.Page);
					_folderCollection = actions.GetFoldersList(acct.ID, _sync);

					if(!IsPostBack)
					{
						LoadData();
					}

					isImapAccount = (acct.MailIncomingProtocol == IncomingMailProtocol.Imap4) ? true : false;
					PrintTreeRecursive(isImapAccount, _selectedFolderID, _folderCollection, 0);
				}
				else
				{
					Log.Write("Account is NULL!!!");
					Response.Redirect("default.aspx");
				}
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void LoadData()
		{
			_selectedFolderID = (Session["id_folder"] != null) ? (long)Session["id_folder"] : -1;
		}

		protected void PrintTreeRecursive(bool isImapAccount, long selectedFolderID, FolderCollection folders, int padding)
		{
			foreach (Folder fld in folders)
			{
				if (fld.Hide) continue;

				bool isSyncFolder = false;
				if (isImapAccount)
				{
					if ((fld.SyncType != FolderSyncType.DontSync) && (fld.SyncType != FolderSyncType.DirectMode))
					{
						isSyncFolder = true;				
					}
				}
				string folderImgSrc = string.Empty;
				switch (fld.Type)
				{
					case FolderType.Inbox:
						folderImgSrc = string.Format("skins/{0}/folders/folder_inbox{1}.gif", _skin, (isSyncFolder) ? "_sync" : string.Empty);
						break;
					case FolderType.SentItems:
						folderImgSrc = string.Format("skins/{0}/folders/folder_send{1}.gif", _skin, (isSyncFolder) ? "_sync" : string.Empty);
						break;
					case FolderType.Drafts:
						folderImgSrc = string.Format("skins/{0}/folders/folder_drafts{1}.gif", _skin, (isSyncFolder) ? "_sync" : string.Empty);
						break;
					case FolderType.Trash:
						folderImgSrc = string.Format("skins/{0}/folders/folder_trash{1}.gif", _skin, (isSyncFolder) ? "_sync" : string.Empty);
						break;
					case FolderType.Custom:
						folderImgSrc = string.Format("skins/{0}/folders/folder{1}.gif", _skin, (isSyncFolder) ? "_sync" : string.Empty);
						break;
				}

				FoldersList += string.Format(@"
	<div id=""{0}----{1}"" {6} style=""padding-left: {2}px;"" onclick = ""javascript:ChangeFolder(this.id); return false;"">
	<a href=""javascript:void(0); return false;"">
	<img src=""{3}""/>
	<span>{4}{5}</span>
	</a>{7}
	</div>
",
					fld.ID, // 0
					fld.FullPath, // 1
					padding, // 2
					folderImgSrc, // 3
					Utils.GetLocalizedFolderNameByType(fld),//fld.Name, // 4
					(fld.UnreadMessageCount > 0) ? string.Format(@" (<span title=""{0}"">{1}</span>)", _manager.GetString("NewMessages"), fld.UnreadMessageCount) : string.Empty, //5
					(fld.ID == selectedFolderID) ? @"class=""wm_select_folder""" : string.Empty, // 6
					(fld.SyncType == FolderSyncType.DirectMode) ? string.Format(@"&nbsp;<span title=""{0}"" class=""wm_folder_direct_mode"">&nbsp;{1}&nbsp;</span>", _manager.GetString("DirectAccessTitle"), _manager.GetString("DirectAccess")) : "");

				if (fld.SubFolders.Count > 0)
				{
					PrintTreeRecursive(isImapAccount, selectedFolderID, fld.SubFolders, padding + 8);
				}
			}

			Folders.InnerHtml = FoldersList;
		}
	}
}
