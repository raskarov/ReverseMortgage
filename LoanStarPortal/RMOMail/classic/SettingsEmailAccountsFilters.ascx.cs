namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Text;
	using WebMailPro;

	/// <summary>
	///		Summary description for SettingsEmailAccountsFilters.
	/// </summary>
	public partial class SettingsEmailAccountsFilters : System.Web.UI.UserControl
	{
		
		protected jsbuilder _js;
		public jsbuilder js
		{
			get { return _js; }
			set { _js = value; }
		}

		protected int editFilterId, deleteFilterId;
		protected string filtersTableString;
		
		protected string _skin = Constants.defaultSkinName;
		protected WebmailResourceManager _resMan = null;
		protected Account _acct;
		protected Filter[] _filters;
		protected FolderCollection _folders;
		protected Filter _filter = null;
		protected String errorClass = String.Empty;
		protected String errorDesc = String.Empty;

		protected BaseWebMailActions baseAction;

		public Account EditAccount
		{
			get { return _acct; }
			set { _acct = value; }
		}

		public Filter EditFilter
		{
			get { return _filter; }
			set { _filter = value; }
		}

		public string Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}

		public Filter[] Filters
		{
			get { return _filters; }
			set { _filters = value; }
		}

		public FolderCollection Folders
		{
			get { return _folders; }
			set { _folders = value; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			errorClass = "wm_hide";
			try
			{
				_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();

				string queryEditId = Request.QueryString["fedit"];
				string queryDelId = Request.QueryString["fdel"];
				editFilterId = (queryEditId != null) ? int.Parse(queryEditId) : -1;
				deleteFilterId = (queryDelId != null) ? int.Parse(queryDelId) : -1;

				baseAction = new BaseWebMailActions(_acct, this.Page);
				if (deleteFilterId > 0)
				{
					baseAction.DeleteFilter(deleteFilterId, _acct.ID);
				}

				if (!IsPostBack) 
				{
					_preLoadPage();
				}

				js.AddText(@"
function ChangeAction()
{
	if (document.getElementById('settingsID_settingsEmailAccountsID_settingsEmailAccountsFiltersID_actionFilter').value == " + (int) FilterAction.MoveToFolder + @")
		document.getElementById('settingsID_settingsEmailAccountsID_settingsEmailAccountsFiltersID_filterFolder').disabled = false;
	else
		document.getElementById('settingsID_settingsEmailAccountsID_settingsEmailAccountsFiltersID_filterFolder').disabled = true;
}

function CheckSubmit()
{
	if (IsDemo() == true) return false;
	var obj = document.getElementById('settingsID_settingsEmailAccountsID_settingsEmailAccountsFiltersID_filter_text');
	if (obj.value == '')
	{
		alert(Lang.WarningEmptyFilter);
		return false;
	}
	return true;
}
");
				js.AddInitText("ChangeAction();");
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		private void _preLoadPage()
		{
			try
			{
				Filters = baseAction.GetFilters(_acct.ID);
				Folders = baseAction.GetFoldersList(_acct.ID, -2);

				_preFiltertable(Filters, editFilterId);

				InitControls();

                cancelButton.Value = _resMan.GetString("Cancel"); 
				if (EditFilter != null)
				{
                    cancelButton.Visible = true;
					submitType.Value = _resMan.GetString("Save"); 
					trHeader.InnerHtml = _resMan.GetString("EditFilter"); 
					filter_text.Value = EditFilter.FilterStr;
					if (EditFilter.Action != FilterAction.MoveToFolder)
					{
						filterFolder.Disabled = true;
					}
				}
				else
				{
                    cancelButton.Visible = false;
					_setDefaultForm();
				}
			}			
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		public string OutputFiltersTable()
		{
			return filtersTableString;
		}

		private void _preFiltertable(Filter[] filterCollection, int idEdit)
		{
			filtersTableString = "";
			if (filterCollection != null)
			{
				foreach (Filter _forfilter in filterCollection)
				{
					string trclass = "";
					if (_forfilter.IDFilter == idEdit)
					{
						trclass = @" class=""wm_settings_list_select""";
						EditFilter = _forfilter;
					}
					string field = "";
					switch(_forfilter.Field)
					{
						case FilterField.From: field = _resMan.GetString("From"); break;
						case FilterField.To: field = _resMan.GetString("To"); break;
						case FilterField.Subject: field = _resMan.GetString("Subject"); break;
					}
					string condition = "";
					switch(_forfilter.Condition)
					{
						case FilterCondition.ContainSubstring: condition = _resMan.GetString("ContainSubstring"); break;
						case FilterCondition.ContainExactPhrase: condition = _resMan.GetString("ContainExactPhrase"); break;
						case FilterCondition.NotContainSubstring: condition = _resMan.GetString("NotContainSubstring"); break;
					}

					filtersTableString += @"
<tr" + trclass + @">
	<td>" + condition + @" <b>" + Utils.EncodeHtml(_forfilter.FilterStr) + @"</b> " + "at" + @" " + field + @" " + "field" +@"</td>
	<td style=""width: 20px;""><nobr><a href=""basewebmail.aspx?scr=settings_accounts_filters&fedit=" + _forfilter.IDFilter.ToString() + @""">" + _resMan.GetString("EditFilter") + @"</a></nobr></td>
	<td style=""width: 20px;""><nobr><a onclick=""return confirm(Lang.ConfirmAreYouSure);"" href=""basewebmail.aspx?scr=settings_accounts_filters&fdel=" + _forfilter.IDFilter.ToString() + @""">" + _resMan.GetString("Delete") + @"</a></nobr></td>
</tr>";
				}
			}
		}

		private void InitControls()
		{
			actionFilter.Items.Clear();
			actionFilter.Items.Add(new ListItem(_resMan.GetString("DeleteFromServer"), ((int) FilterAction.DeleteFromServerImmediately).ToString()));
			actionFilter.Items.Add(new ListItem(_resMan.GetString("MarkGrey"), ((int) FilterAction.MarkGrey).ToString()));
			actionFilter.Items.Add(new ListItem(_resMan.GetString("MoveToFolder"), ((int) FilterAction.MoveToFolder).ToString()));
			ListItem actionItem = (EditFilter != null) ?
				actionFilter.Items.FindByValue(((int) EditFilter.Action).ToString()) : null;
			if (actionItem != null)
			{
				actionItem.Selected = true;
			}

			conditionFilter.Items.Clear();
			conditionFilter.Items.Add(new ListItem(_resMan.GetString("ContainSubstring"), ((int) FilterCondition.ContainSubstring).ToString()));
			conditionFilter.Items.Add(new ListItem(_resMan.GetString("ContainExactPhrase"), ((int) FilterCondition.ContainExactPhrase).ToString()));
			conditionFilter.Items.Add(new ListItem(_resMan.GetString("NotContainSubstring"), ((int) FilterCondition.NotContainSubstring).ToString()));
			ListItem conditionItem = (EditFilter != null) ?
				conditionFilter.Items.FindByValue(((int) EditFilter.Condition).ToString()) : null;
			if (conditionItem != null)
			{
				conditionItem.Selected = true;
			}

			ruleFilter.Items.Clear();
			ruleFilter.Items.Add(new ListItem(_resMan.GetString("From"), ((int) FilterField.From).ToString()));
			ruleFilter.Items.Add(new ListItem(_resMan.GetString("To"), ((int) FilterField.To).ToString()));
			ruleFilter.Items.Add(new ListItem(_resMan.GetString("Subject"), ((int) FilterField.Subject).ToString()));
			ListItem ruleItem = (EditFilter != null) ?
				ruleFilter.Items.FindByValue(((int) EditFilter.Field).ToString()) : null;
			if (ruleItem != null)
			{
				ruleItem.Selected = true;
			}

			applyButton.Value = _resMan.GetString("Apply");
			checkbox_x_spam.Checked = _acct.UserOfAccount.Settings.XSpam;

			filterFolder.Items.Clear();
			if (EditFilter != null)
			{
				_createSelectFolderTreeForFilters(Folders, EditFilter.IDFolder, 0);
			}
			else
			{
				_createSelectFolderTreeForFilters(Folders, -1, 0);
			}
		}

		private void _createSelectFolderTreeForFilters(FolderCollection folders, long idSelected, int level)
		{
			int c = folders.Count;
			for (int i = 0; i < c; i++)
			{
				Folder folder = folders[i];
				string prev = string.Empty;
				ListItem option = new ListItem(str_repeat(Server.HtmlDecode("&nbsp;"), level * 3) + Utils.GetLocalizedFolderNameByType(folder), folder.ID.ToString());
				option.Selected = (folder.ID == idSelected);
				filterFolder.Items.Add(option);

				if (folder.SubFolders != null && folder.SubFolders.Count > 0)
				{
					_createSelectFolderTreeForFilters(folder.SubFolders, idSelected, level + 1);
				}
			}
		}

		private string str_repeat(string str, int number)
		{
			string output = "";
			if (number > 0)
			{
				for (int i = 0; i < number; i++)
				{
					output += str;
				}
			}
			return output;
		}

		private void _setDefaultForm()
		{
			actionFilter.Items[0].Selected = true;
			conditionFilter.Items[0].Selected = true;
			ruleFilter.Items[0].Selected = true;
			filterFolder.Items[0].Selected = true;
			filterFolder.Disabled = true;
			submitType.Value = _resMan.GetString("Add");
			trHeader.InnerHtml = _resMan.GetString("NewFilter"); 
			filter_text.Value = "";
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
			applyButton.ServerClick += new EventHandler(applyButton_ServerClick);
			submitType.ServerClick +=new EventHandler(submitType_ServerClick);
		}
		#endregion

		private void applyButton_ServerClick(object sender, EventArgs e)
		{
			if (_acct != null)
			{
				try
				{
					_acct.UserOfAccount.Settings.XSpam = checkbox_x_spam.Checked;
					_acct.Update(true);

					Account acct = Session[Constants.sessionAccount] as Account;
					if (acct != null)
					{
						if (acct.ID == _acct.ID) Session[Constants.sessionAccount] = _acct;
					}
				}
				catch (Exception ex)
				{
					((basewebmail)this.Page).OutputException(ex);
				}
				_preLoadPage();
			}
		}

		private void submitType_ServerClick(object sender, EventArgs e)
		{
			if (_acct != null)
			{
				try
				{
					if (submitType.Value == _resMan.GetString("Add"))
					{
						if (Validation.CheckIt(Validation.ValidationTask.Substring, filter_text.Value))
						{
							Filter newFilter = new Filter(_acct);
							newFilter.Action = (FilterAction) int.Parse(actionFilter.Value);
							newFilter.Condition = (FilterCondition) int.Parse(conditionFilter.Value);
							newFilter.Field = (FilterField) int.Parse(ruleFilter.Value);
							newFilter.FilterStr = Validation.Corrected;
							newFilter.IDFolder = long.Parse(filterFolder.Value);
							baseAction.NewFilter(_acct.ID, (byte) newFilter.Field, 
								(byte) newFilter.Condition, (byte) newFilter.Action, 
								newFilter.IDFolder, newFilter.FilterStr);
						}
						else
						{
							throw (new Exception(Validation.ErrorMessage));
						}
					}
					else
					{
						Filter[] tempFilters = baseAction.GetFilters(_acct.ID);
						if (tempFilters != null && tempFilters.Length > 0)
						{
							foreach (Filter _prefilter in tempFilters)
							{
								if (_prefilter.IDFilter == editFilterId)
								{
									EditFilter = _prefilter;
								}
							}
						}

						if (EditFilter != null)
						{
							EditFilter.Action = (FilterAction) int.Parse(actionFilter.Value);
							EditFilter.Condition = (FilterCondition) int.Parse(conditionFilter.Value);
							EditFilter.Field = (FilterField) int.Parse(ruleFilter.Value);
							EditFilter.FilterStr = Validation.Corrected;
							EditFilter.IDFolder = long.Parse(filterFolder.Value);
							baseAction.UpdateFilter(_acct.ID, EditFilter.IDFilter, (byte) EditFilter.Field, 
								(byte) EditFilter.Condition, (byte) EditFilter.Action, 
								EditFilter.IDFolder, EditFilter.FilterStr);
							Session[Constants.sessionReportText] = _resMan.GetString("ReportFiltersUpdatedSuccessfuly");
						}
					}
				}
				catch (Exception ex)
				{
					((basewebmail)this.Page).OutputException(ex);
				}
			}

			_preLoadPage();
		}
	}
}
