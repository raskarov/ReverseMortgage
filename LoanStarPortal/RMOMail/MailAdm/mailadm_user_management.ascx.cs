namespace WebMailPro.MailAdm
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for mailadm_user_management.
	/// </summary>
//	<asp:TemplateColumn HeaderText="Used Space, %" >
//	<ItemTemplate>
//	<div class="wm_progressbar">
//			 <div class="wm_progressbar_used"></div>
//			 </div>
//	</ItemTemplate>
//	</asp:TemplateColumn>

	public partial class mailadm_user_management : System.Web.UI.UserControl
	{
		protected WebmailSettings _settings;
		protected long _accountsCount = 0;
        protected string _usersCountString = ""; 
		protected long _userMailboxsSize = 0;
		protected int _pageSize = 20;
        protected string[] _fieldsASCPrefixes = new string[] { "id_acct", "email", "last_login", "logins_count", "mail_inc_host", "mail_out_host", "mailbox_size" };

        public string OrderBy
        {
            get { return ((string)Session["wm_adm_um_order"] != null) ? (string)Session["wm_adm_um_order"] : "email"; }
            set { Session["wm_adm_um_order"] = value; }
        }

        public bool Asc
        {
            get { return ((string)Session["wm_adm_umAsc"] != null) ? Convert.ToBoolean((string)Session["wm_adm_umAsc"]) : true; }
            set { Session["wm_adm_umAsc"] = value.ToString(); }
        }
        public string SearchCondition
        {
            get { return ((string)Session["wm_adm_um_condition"] != null) ? (string)Session["wm_adm_um_condition"] : string.Empty; }
            set { Session["wm_adm_um_condition"] = value; }
        }

		public long UserMailboxsSize
		{
			get { return _userMailboxsSize; }
			set { _userMailboxsSize = value; }
		}

		public int PageSize
		{
			get { return _pageSize; }
			set { _pageSize = value; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			_settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			// Put user code to initialize the page here
			if (!Page.IsPostBack)
			{
				this.usersDataGridID.CurrentPageIndex = 0;
				this.usersDataGridID.PageSize = _pageSize;
				txtSearch.Value = SearchCondition;
				GridDataBind(0, _pageSize, OrderBy, Asc, SearchCondition);
			}
			else
			{
				//SearchCondition = txtSearch.Value;
				//Response.Redirect(string.Format(@"mailadm.aspx?mode=users&order={0}&asc={1}&condition={2}", OrderBy, Asc, SearchCondition));
			}
		}

		private void GridDataBind(int page, int pageSize, string orderBy, bool asc, string condition)
		{
			DbManager manager = (new DbManagerCreator()).CreateDbManager();
			try
			{
				manager.Connect();
				this._userMailboxsSize = manager.GetUserMailboxsSize(-1);
				_accountsCount = this.usersDataGridID.VirtualItemCount = manager.GetAccountsCount(condition);
                int usercont = (condition.Length > 0) ? -1 : manager.GetUsersCount();
                _usersCountString = (usercont > -1) ? "/ " + usercont + " user(s)" : "";
//				using (IDataReader reader = manager.SelectAccountsAdminReader(page, pageSize, orderBy, asc, condition))
//				{
//					this.usersDataGridID.DataSource = reader;
//					this.usersDataGridID.DataBind();
//				}
				DataView _dv = manager.SelectAccountsAdminReader(page, pageSize, orderBy, asc, condition);
                this.usersDataGridID.DataSource = _dv;
				this.usersDataGridID.DataBind();
                if (_accountsCount - _pageSize <= 0)
                {
                    this.usersDataGridID.PagerStyle.Visible = false;
                }
                setSortingPicture();
                this.usersDataGridID.Columns[0].HeaderImageUrl = @"skins/Hotmail_Style/admin_arrow_1.gif";
			}
			catch (Exception ex)
			{
                Log.WriteException(ex);				
			}
			finally
			{
				manager.Disconnect();
			}
		}

        private void setSortingPicture()
        {
            //usersDataGridID.Columns[Array.IndexOf(_fieldsASCPrefixes, OrderBy)].
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
			this.ButtonCreate.Click += new EventHandler(ButtonCreate_Click);
			this.ButtonSearch.Click += new EventHandler(ButtonSearch_Click);
		}
		#endregion

		protected void ButtonCreate_Click(object sender, EventArgs e)
		{
			//create new user
			Response.Redirect("mailadm.aspx?mode=new_user");
		}

        protected void HeaderLinkClick(object sender, CommandEventArgs e)
        {
            if (OrderBy == e.CommandArgument.ToString())
            {
                Asc = !Asc;
            }
            else
            {
                Asc = true;
                OrderBy = e.CommandArgument.ToString();
            }
            Response.Redirect(string.Format(@"mailadm.aspx?mode=users&order={0}&asc={1}&condition={2}", OrderBy, Asc, SearchCondition));
        }

		protected void ButtonSearch_Click(object sender, EventArgs e)
		{
			//search users
            SearchCondition = txtSearch.Value;
            Response.Redirect(string.Format(@"mailadm.aspx?mode=users&order={0}&asc={1}&condition={2}", OrderBy, Asc, SearchCondition));
//			this.usersDataGridID.CurrentPageIndex = 0;
//			SearchCondition = txtSearch.Value;
//			GridDataBind(0, _pageSize, OrderBy, Asc, SearchCondition);
		}

		protected void ChangeGridPage(object sender, DataGridPageChangedEventArgs e)
		{
			this.usersDataGridID.CurrentPageIndex = e.NewPageIndex;
			GridDataBind(e.NewPageIndex + 1, _pageSize, OrderBy, Asc, SearchCondition);
		}

		protected double CalculateUsedSpace(object mailbox_size, object mailbox_limit)
		{
			if ((mailbox_size != null) && (mailbox_limit != null))
			{
				double ln_mailbox_size = (mailbox_size != DBNull.Value) ? Convert.ToDouble(mailbox_size) : 0;
				double ln_mailbox_limit = (mailbox_limit != DBNull.Value) ? Convert.ToDouble(mailbox_limit) : 0;
				if (ln_mailbox_limit > 0)
				{
					double usedPercent = ln_mailbox_size / ln_mailbox_limit;
					usedPercent *= 100;
					return (usedPercent <= 100) ? Math.Round(usedPercent, 0) : 100;
				}
				else
				{
					return 100;
				}
			}
			return 0;
		}

		protected double CalculateUsedAllSpace(object mailbox_size, object all_mailboxs_size, object mailbox_limit)
		{
			if ((mailbox_size != null) && (all_mailboxs_size != null) && (mailbox_limit != null))
			{
				double ln_mailbox_size = (mailbox_size != DBNull.Value) ? Convert.ToDouble(mailbox_size) : 0;
				double ln_all_mailboxs_size = (all_mailboxs_size != DBNull.Value) ? Convert.ToDouble(all_mailboxs_size) : 0;
				double ln_mailbox_limit = (mailbox_limit != DBNull.Value) ? Convert.ToDouble(mailbox_limit) : 0;
				if (ln_mailbox_limit > 0)
				{
					ln_all_mailboxs_size = ln_all_mailboxs_size - ln_mailbox_size;
					double usedAccount = CalculateUsedSpace(mailbox_size, mailbox_limit);
					double usedPercent = ln_all_mailboxs_size / ln_mailbox_limit;
					usedPercent *= 100;
					usedPercent = (usedPercent <= 100) ? Math.Round(usedPercent, 0) : 100;
					return ((usedPercent - usedAccount) > 0) ? usedPercent - usedAccount : 0;
				}
				else
				{
					return 100;
				}
			}
			return 0;
		}

		protected string GetProgressBarTitle(object mailbox_size, object mailbox_limit)
		{
			if ((mailbox_size != null) && (mailbox_limit != null))
			{
				double ln_mailbox_size = (mailbox_size != DBNull.Value) ? Convert.ToDouble(mailbox_size) : 0;
				double ln_mailbox_limit = (mailbox_limit != DBNull.Value) ? Convert.ToDouble(mailbox_limit) : 0;
				if (ln_mailbox_limit > 0)
				{
					//ln_mailbox_size = ln_mailbox_limit / 2; //TODO: remove this string
					double usedPercent = ln_mailbox_size / ln_mailbox_limit;
					usedPercent *= 100;
					usedPercent = (usedPercent <= 100) ? Math.Round(usedPercent, 0) : 100;
					string alttext = string.Format(@"Account is using  {0}% ('{1}' Kb) of user '{2}' Kb'", 
						usedPercent, Math.Round(ln_mailbox_size/1024), Math.Round(ln_mailbox_limit/1024));
					return alttext;
				}
				else
				{
					return string.Empty;
				}
			}
			return string.Empty;
		}

		protected string GetUsedColor(double usedPercent, double usedAllPercent)
		{
			return "#7E9BAF;";
		}

		protected string GetUsedAllColor(double usedAllPercent)
		{
			return "#B7CEDE;";
		}

        protected void usersDataGridID_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                int _currCortColumn = Array.IndexOf(_fieldsASCPrefixes, OrderBy);
                string _imgSource = Asc ? "url(./skins/Hotmail_Style/admin_arrow_1.gif)" : "url(./skins/Hotmail_Style/admin_arrow_2.gif)";
                e.Item.Cells[_currCortColumn].Attributes.CssStyle.Add("background-repeat", "no-repeat");
                e.Item.Cells[_currCortColumn].Attributes.CssStyle.Add("background-position", "right center");
                e.Item.Cells[_currCortColumn].Attributes.CssStyle.Add("background-image", _imgSource);
                e.Item.Cells[_currCortColumn].Attributes.CssStyle.Add("padding-right", "right center");
                
                LinkButton _lb = (LinkButton)e.Item.Cells[_currCortColumn].FindControl("lbl_" + OrderBy);
                _lb.Attributes.CssStyle.Add("margin-right", "9px");
            }
        }
	}
}
