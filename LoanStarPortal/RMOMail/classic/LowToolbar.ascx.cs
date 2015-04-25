namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for LowToolbar.
	/// </summary>
	public partial class LowToolbar : System.Web.UI.UserControl
	{
		protected string _skin = Constants.defaultSkinName;
		protected WebmailResourceManager _manager = null;
		protected long _id_folder = 0;
		protected string _full_name_folder = string.Empty;
		protected int _page_number = 1;
		protected int _sort_field = 0;
		protected int _sort_order = 0;
		protected string _look_for = string.Empty;
		protected int _search_fields = 0;
		protected int _folderMessageCount = 0;
		protected int _folderUnreadMessageCount = 0;
		protected Account acct = null;
		protected string used;
		protected string full;

		public string Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			acct = Session[Constants.sessionAccount] as Account;

			_manager = (new WebmailResourceManagerCreator()).CreateResourceManager();

			GetCountMails();
		}

		protected void LoadData()
		{
			_id_folder = (Session["id_folder"] != null) ? (long)Session["id_folder"] : -1;
		}

		public void GetCountMails()
		{
			try
			{
				LoadData();

				BaseWebMailActions bwa = new BaseWebMailActions(acct, this.Page);

				if(_id_folder != -1)
				{
					Folder CurrenFolder =  bwa.GetFolder(_id_folder);
					_full_name_folder = CurrenFolder.FullPath;

					_folderMessageCount = CurrenFolder.MessageCount;
					_folderUnreadMessageCount = CurrenFolder.UnreadMessageCount;
				}
				else
				{
					_full_name_folder = "";
				}

				used = Convert.ToString(((acct.MailboxSize / acct.UserOfAccount.Settings.MailboxLimit) * 100));
				full = Utils.GetFriendlySize(acct.UserOfAccount.Settings.MailboxLimit);
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
	}
}
