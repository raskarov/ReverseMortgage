using System.Text;

namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for SettingsEmailAccountsManageFolders.
	/// </summary>
	public partial class SettingsEmailAccountsManageFolders : System.Web.UI.UserControl
	{
		protected string _skin = Constants.defaultSkinName;
		protected Account _acct = null;
		protected WebmailResourceManager _resMan = null;
		protected jsbuilder _js;

		public string Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}

		public Account EditAccount
		{
			get { return _acct; }
			set { _acct = value; }
		}

		public jsbuilder js
		{
			get { return _js; }
			set { _js = value; }
		}


		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
                okButton.Value = _resMan.GetString("OK");
                folderInfo.Visible = false;
                js.AddText(@"

function CheckFolderName()
{
	if (IsDemo() == true) return false;
	var oVal = new CValidate();
	if (oVal.IsEmpty(document.getElementById('" +textBoxFolderName.ClientID+@"').value))
		{
			alert(Lang.WarningEmptyFolderName);
			return false;
		}
	if (!oVal.IsCorrectFileName(document.getElementById('"+textBoxFolderName.ClientID+@"').value))
	{
		alert(Lang.WarningCorrectFolderName);
		return false;
	}
	return true;
}

function SelectAllInputs(obj)
{
    var inputs = document.getElementsByTagName(""input"");
    var i, c;
    for (i = 0, c = inputs.length; i < c; i++)
    {
        if (inputs[i].type == ""checkbox"" && !inputs[i].disabled)
        {
            inputs[i].checked = obj.checked;
        }
    }
}
var folderInput, folderHref;
                    
function EditFolder(folderId)
{

    if (folderHref && folderInput)
    {
        folderInput.className = ""wm_hide"";
        folderHref.className = """";
    }

    folderHref = document.getElementById(""folder_a_"" + folderId);
    folderInput = document.getElementById(""folder_i_"" + folderId);
    var folderForm = CreateChildWithAttrs(document.body, 'form', [['action', 'basewebmail.aspx?scr=settings_accounts_folders&action=rename'], ['method', 'POST'], ['class', 'wm_hide']]);
	var hideFormInput1 = CreateChildWithAttrs(folderForm, 'input', [['type', 'hidden'], ['name', 'folderId'], ['value', folderId]]);
	var hideFormInput2 = CreateChildWithAttrs(folderForm, 'input', [['type', 'hidden'], ['name', 'folderName']]);
   
    if (folderHref && folderInput && folderForm)
    {
        folderInput.className = """";
        folderHref.className = ""wm_hide"";
        folderInput.size = folderInput.value.length + 2;
        folderInput.onkeydown = function(ev)
        {
            if (isEnter(ev))
            {
                if (folderInput.value != folderHref.innerHTML)
                {
										var oVal = new CValidate();
										if (oVal.IsCorrectFileName(folderInput.value))
										{
											folderHref.innerHTML = folderInput.value;
											hideFormInput2.value = folderInput.value;
											folderForm.submit();
										}
										else
										{
											alert('"+_resMan.GetString("WarningCorrectFolderName")+@"');
										}
                }
                folderInput.className = ""wm_hide"";
                folderHref.className = """";
            }
        }
        folderInput.focus();
    }
    return false;
}

function changeSync(obj, folderId)
{
	var folderForm = CreateChildWithAttrs(document.body, 'form', [['action', 'basewebmail.aspx?scr=settings_accounts_folders&action=sync'], ['method', 'POST'], ['class', 'wm_hide']]);
	var hideFormInput1 = CreateChildWithAttrs(folderForm, 'input', [['type', 'hidden'], ['name', 'folderId'], ['value', folderId]]);
	var hideFormInput2 = CreateChildWithAttrs(folderForm, 'input', [['type', 'hidden'], ['name', 'folderSync'], ['value', obj.value]]);
	folderForm.submit();
}

function DeleteFolders()
{
	if (confirm(Lang.ConfirmAreYouSure))
	{
		var inputs = document.getElementsByTagName(""input"");
		var i, c;
		var form = CreateChildWithAttrs(document.body, 'form', [['action', 'basewebmail.aspx?scr=settings_accounts_folders&action=delete'], ['method', 'POST']]);
		for (i = 0, c = inputs.length; i < c; i++)
		{
			if (inputs[i].type == ""checkbox"" && inputs[i].checked )
			{
				CreateChildWithAttrs(form, 'input', [['type', 'hidden'], ['name', inputs[i].name], ['value', inputs[i].value]]);
			}
		}
		if (c > 1) form.submit();
	}
}






");
		
				BaseWebMailActions actions = new BaseWebMailActions(_acct, this.Page);
				if (Request.QueryString["folder_hide"] != null)
				{
					long id_folder = long.Parse(Request.QueryString["folder_hide"]);
					Folder fld = actions.GetFolder(id_folder);
					if (fld != null)
					{
						fld.Hide = true;
						actions.UpdateFolders( _acct.ID, new Folder[] {fld});
					}
				}
				if (Request.QueryString["folder_show"] != null)
				{
					long id_folder = long.Parse(Request.QueryString["folder_show"]);
					Folder fld = actions.GetFolder(id_folder);
					if (fld != null)
					{
						fld.Hide = false;
						actions.UpdateFolders( _acct.ID, new Folder[] {fld});
					}
				}
				if (Request.QueryString["folders_swap"] != null)
				{
					long id_folder1 = long.Parse(Request.QueryString["fld1"]);
					long id_folder2 = long.Parse(Request.QueryString["fld2"]);
					Folder fld1 = actions.GetFolder(id_folder1);
					if (fld1 != null)
					{
						Folder fld2 = actions.GetFolder(id_folder2);
						if (fld2 != null)
						{
							short temp = fld1.FolderOrder;
							fld1.FolderOrder = fld2.FolderOrder;
							fld2.FolderOrder = temp;
							actions.UpdateFolders(_acct.ID, new Folder[] { fld1, fld2 });
						}
					}
				}
				if (Request.QueryString["action"] == "rename")
				{
					long id_folder = long.Parse(Request.Params["folderId"]);
					string newName = Request.Params["folderName"];
					Folder fld = actions.GetFolder(id_folder);
					if (fld != null)
					{
						if (Validation.CheckIt(Validation.ValidationTask.FolderName, newName))
							{
								fld.UpdateName = Validation.Corrected;
								fld.UpdateFullPath = Folder.CreateNewFullPath(fld.FullPath, _acct.Delimiter, Validation.Corrected);
								actions.UpdateFolders(_acct.ID, new Folder[] { fld });
							}
						else
							{
								throw (new Exception(Validation.ErrorMessage));
							}
					}
				}
				if (Request.QueryString["action"] == "sync")
				{
					long id_folder = long.Parse(Request.Params["folderId"]);
					int sync = int.Parse(Request.Params["folderSync"]);
					Folder fld = actions.GetFolder(id_folder);
					if (fld != null)
					{
						fld.SyncType = (FolderSyncType)sync;
						actions.UpdateFolders(_acct.ID, new Folder[] { fld });
					}
				}
				if (Request.QueryString["action"] == "delete")
				{
					string str = Request.Params["ch[]"];
					if (str != null)
					{
						string[] ids = str.Split(',');
						foreach (string id in ids)
						{
							long id_folder = long.Parse(id);
							Folder fld = actions.GetFolder(id_folder);
							if (fld != null)
							{
								actions.DeleteFolders(_acct.ID, new Folder[] { fld });
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		private void InitControls()
		{
			if (_acct != null)
			{
				this.radioOnMailServer.Checked = true;
				if (_acct.MailIncomingProtocol != IncomingMailProtocol.Imap4)
				{
					this.tdNewFolderIn.Attributes.Add("class", "wm_hide");
				}
				comboParentFolder.Items.Clear();
				OutputFoldersCombo();
				if (comboParentFolder.Items.Count > 0)
				{
					comboParentFolder.SelectedIndex = 0;
				}
				textBoxFolderName.Value = string.Empty;
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
			this.PreRender += new EventHandler(SettingsEmailAccountsManageFolders_PreRender);
			this.okButton.ServerClick+=new EventHandler(okButton_ServerClick);

		}
		#endregion

		public string OutputHeader()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(@"<tr class=""wm_settings_mf_headers"">");
			if (_acct.MailIncomingProtocol != IncomingMailProtocol.Imap4)
			{
				sb.AppendFormat(@"
<td style=""width: 30px;"">
<input class=""wm_checkbox"" type=""checkbox"" onclick=""SelectAllInputs(this)""/>
</td>
<td class=""wm_settings_mf_folder"" style=""width: 410px;"">{0}</td>
<td style=""width: 40px;"">{1}</td>
<td style=""width: 40px;"">{2}</td>
<td style=""width: 100px;"">{3}</td>
<td style=""width: 42px;""/>
",
					_resMan.GetString("Folder"), // 0
					_resMan.GetString("Msgs"), // 1
					_resMan.GetString("Size"), // 2
					_resMan.GetString("ShowThisFolder"));
			}
			else
			{
				sb.AppendFormat(@"
				<td style=""WIDTH: 30px""><input type=""checkbox"" onclick=""SelectAllInputs(this)"" class=""wm_checkbox""></td>
				<td style=""WIDTH: 270px"" class=""wm_settings_mf_folder"">{0}</td>
				<td style=""WIDTH: 40px"">{1}</td>
				<td style=""WIDTH: 40px"">{2}</td>
				<td style=""WIDTH: 140px"">{3}</td>
				<td style=""WIDTH: 100px"">{4}</td>
				<td style=""WIDTH: 42px"" />
",
					_resMan.GetString("Folder"), // 0
					_resMan.GetString("Msgs"), // 1
					_resMan.GetString("Size"), // 2
					_resMan.GetString("Synchronize"), // 3
					_resMan.GetString("ShowThisFolder"));
			}
			sb.Append("</tr>");
			return sb.ToString();
		}

		public string OutputFolders()
		{
			StringBuilder sb = new StringBuilder();
			BaseWebMailActions actions = new BaseWebMailActions(_acct, this.Page);
			FolderCollection fc = actions.GetFoldersList(_acct.ID, -1);

			PrintTreeRecursive(ref sb, (_acct.MailIncomingProtocol == IncomingMailProtocol.Imap4) ? true : false, fc, 0);

			return sb.ToString();
		}

		private void PrintTreeRecursive(ref StringBuilder sb, bool isImapAccount, FolderCollection folders, int padding)
		{
			for (int i = 0; i < folders.Count; i++)
			{
				Folder fld = folders[i];
				bool isSyncFolder = false;
				string syncCombo = string.Empty;
				if (isImapAccount)
				{
					if ((fld.SyncType != FolderSyncType.DontSync) && (fld.SyncType != FolderSyncType.DirectMode))
					{
						isSyncFolder = true;
					}

					string[] selectedParams = new string[0];
					if (fld.SyncType == FolderSyncType.DontSync)
					{
						selectedParams = new string[] { "selected", "", "", "", "", ""};
					}
					else if (fld.SyncType == FolderSyncType.NewHeadersOnly)
					{
						selectedParams = new string[] { "", "selected", "", "", "", ""};
					}
					else if (fld.SyncType == FolderSyncType.AllHeadersOnly)
					{
						selectedParams = new string[] { "", "", "selected", "", "", ""};
					}
					else if (fld.SyncType == FolderSyncType.NewEntireMessages)
					{
						selectedParams = new string[] { "", "", "", "selected", "", ""};
					}
					else if (fld.SyncType == FolderSyncType.AllEntireMessages)
					{
						selectedParams = new string[] { "", "", "", "", "selected", ""};
					}
					else if (fld.SyncType == FolderSyncType.DirectMode)
					{
						selectedParams = new string[] { "", "", "", "", "", "selected"};
					}
					
					syncCombo = string.Format(@"
			<td>
				<select  onchange=""changeSync(this, " + fld.ID.ToString() + @")"">
					<option value=""0"" {0}>Don't Synchronize</option>
					<option value=""1"" {1}>New Headers</option>
					<option value=""2"" {2}>All Headers</option>
					<option value=""3"" {3}>New Messages</option>
					<option value=""4"" {4}>All Messages</option>
					<option value=""5"" {5}>Direct Mode</option>
				</select>
			</td>
",
						selectedParams);
				}

				string folderImgSrc = string.Empty;
				switch (fld.Type)
				{
					case FolderType.Inbox:
						folderImgSrc = string.Format("folder_inbox{1}.gif", _skin, (isSyncFolder) ? "_sync" : string.Empty);
						break;
					case FolderType.SentItems:
						folderImgSrc = string.Format("folder_send{1}.gif", _skin, (isSyncFolder) ? "_sync" : string.Empty);
						break;
					case FolderType.Drafts:
						folderImgSrc = string.Format("folder_drafts{1}.gif", _skin, (isSyncFolder) ? "_sync" : string.Empty);
						break;
					case FolderType.Trash:
						folderImgSrc = string.Format("folder_trash{1}.gif", _skin, (isSyncFolder) ? "_sync" : string.Empty);
						break;
					case FolderType.Custom:
						folderImgSrc = string.Format("folder{1}.gif", _skin, (isSyncFolder) ? "_sync" : string.Empty);
						break;
				}

				string upControl = string.Empty;
				string downControl = string.Empty;
				if (i == 0)
				{
					upControl = string.Format(@"<img src=""skins/{0}/folders/up_inactive.gif"" class="""">", _skin);
				}
				else
				{
					upControl = string.Format(@"<a href=""basewebmail.aspx?scr=settings_accounts_folders&folders_swap=1&fld1={1}&fld2={2}"" onclick=""if(IsDemo() == true) return false;""><img src=""skins/{0}/folders/up.gif"" class=""""></a>", _skin, fld.ID, folders[i - 1].ID);
				}
				if (i == folders.Count - 1)
				{
					downControl = string.Format(@"<img src=""skins/{0}/folders/down_inactive.gif"" class="""">", _skin);
				}
				else
				{
					downControl = string.Format(@"<a href=""basewebmail.aspx?scr=settings_accounts_folders&folders_swap=1&fld1={1}&fld2={2}""><img src=""skins/{0}/folders/down.gif"" class="""" onclick=""if(IsDemo() == true) return false;""></a>", _skin, fld.ID, folders[i + 1].ID);
				}

				string checkBoxOut = string.Empty;
				if (fld.Type == FolderType.Custom)
				{
					checkBoxOut = string.Format(@"
<input name=""ch[]"" value=""{0}"" type=""checkbox"" class=""wm_checkbox"" {1}>",
						fld.ID,
                        ((fld.SubFolders.Count > 0) || (fld.Size > 0)) ? @"disabled=""disabled""" : "");
				}

				string linkOut = string.Empty;
				if (fld.Type == FolderType.Custom)
				{
					linkOut = string.Format(@"<a id=""folder_a_{0}"" onclick=""{1}"" href=""#"">{2}</a>", fld.ID, (fld.Type == FolderType.Custom) ? string.Format("EditFolder({0})", fld.ID) : "", fld.Name);
				}
				else
				{
					linkOut = string.Format(@"{0}", Utils.GetLocalizedFolderNameByType(fld));
				}

				sb.AppendFormat(@"
		<tr>
			<td>{14}</td>
			<td class=""wm_settings_mf_folder"" style=""PADDING-LEFT: {0}px"">
				<img src=""skins/{1}/folders/{6}"">
				<span> </span>
				{15}
				<input id=""folder_i_{8}"" type=""text"" maxlength=""30"" class=""wm_hide"" value=""{2}""></td>
			<td>{3}</td>
			<td>{4}</td>
			{11}
			<td>
				<a href=""basewebmail.aspx?scr=settings_accounts_folders&{12}={8}"" onclick=""if(IsDemo() == true) return false;"" ><img class=""wm_settings_mf_show_hide wm_control_img"" src=""skins/{1}/folders/{5}""></a>
			</td>
			<td class=""wm_settings_mf_up_down"">
				{9}
				{10}
			</td>
		</tr>
",
					padding, // 0
					_skin, // 1
					fld.Name, // 2
					fld.MessageCount, //3
					Utils.GetFriendlySize(fld.Size), // 4
					(!fld.Hide) ? "show.gif" : "hide.gif", // 5
					folderImgSrc, // 6
                    ((fld.Type != FolderType.Custom) || ((fld.SubFolders.Count > 0) || (fld.Size > 0))) ? @"disabled=""disabled""" : "", // 7
					fld.ID, //8
					upControl, // 9
					downControl, // 10
					syncCombo, // 11
					(!fld.Hide) ? "folder_hide" : "folder_show", // 12
					(fld.Type == FolderType.Custom) ? string.Format("EditFolder({0})", fld.ID) : "", // 13
					checkBoxOut, // 14
					linkOut);

                if ((fld.Type == FolderType.Custom) && ((fld.SubFolders.Count > 0) || (fld.Size > 0)))
                {
                    folderInfo.Visible = true;
                }
                if (fld.SubFolders.Count > 0)
				{
					PrintTreeRecursive(ref sb, isImapAccount, fld.SubFolders, padding + 8);
				}
			}
			
		}

		public void OutputFoldersCombo()
		{
			BaseWebMailActions actions = new BaseWebMailActions(_acct, this.Page);
			FolderCollection fc = actions.GetFoldersList(_acct.ID, -1);
			comboParentFolder.Items.Add(new ListItem(_resMan.GetString("NoParent"), "-1$#%"));

			PrintFoldersCombo(fc, 0);
		}

		private void PrintFoldersCombo(FolderCollection folders, int padding)
		{
			StringBuilder space = new StringBuilder();
			for (int i = 0; i < padding; i++)
			{
				space.Append(Server.HtmlDecode("&nbsp;&nbsp;&nbsp;"));
			}
			foreach (Folder fld in folders)
			{
				comboParentFolder.Items.Add(new ListItem(string.Format("{0}{1}", space.ToString(), fld.Name),
					string.Format("{0}$#%{1}", fld.ID, fld.FullPath)));
				if (fld.SubFolders.Count > 0)
				{
					PrintFoldersCombo(fld.SubFolders, padding + 1);
				}
			}
		}

		private void okButton_ServerClick(object sender, System.EventArgs e)
		{
			try
			{
				if (Validation.CheckIt(Validation.ValidationTask.FolderName, textBoxFolderName.Value))
				{
					BaseWebMailActions actions = new BaseWebMailActions(_acct, this.Page);
					string parentValue = comboParentFolder.Value;
					int id_parent = -1;
					string full_name_parent = string.Empty;
					string name = Validation.Corrected;
					int create = (radioInWebmail.Checked) ? 0 : 1;
					if (parentValue != null)
					{
						int splitIndex = parentValue.IndexOf("$#%");
						string id = parentValue.Substring(0, splitIndex);
						id_parent = int.Parse(id);
						full_name_parent = parentValue.Substring(splitIndex + 3, parentValue.Length - (splitIndex + 3));
					}
					actions.NewFolder(_acct.ID, id_parent, full_name_parent, name, create);
				}
				else
				{
					throw (new Exception(Validation.ErrorMessage));
				}
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		private void SettingsEmailAccountsManageFolders_PreRender(object sender, EventArgs e)
		{
			InitControls();
		}

	}
}
