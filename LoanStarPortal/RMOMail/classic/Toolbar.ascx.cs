namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for Toolbar.
	/// </summary>
	public partial class Toolbar : System.Web.UI.UserControl
	{
		protected jsbuilder _js;
		public jsbuilder js
		{
			get { return _js; }
			set
			{
				Log.WriteLine(@"JS Property", "Set Toolbar JS");
				_js = value;
			}
		}

		protected Account _acct = null;
		public Account CurrentAccount
		{
			get { return _acct; }
			set { _acct = value; }
		}

		protected string _skin = Constants.defaultSkinName;
		protected WebmailResourceManager _manager = null;
		protected string screen = null;
		protected string FolderList = null;
		protected string GroupList = null;
		protected long _id_folder;
		protected bool IsPOP3 = false;

		protected bool IsSearch = false;
		protected string LookFor = "";

		protected string strpadding = String.Empty;
		protected Folder CurrFolder;
		protected bool IsTrash = false;

		public string Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}


		protected void Page_Load(object sender, System.EventArgs e)
		{
			_manager = (new WebmailResourceManagerCreator()).CreateResourceManager();

			_acct = Session[Constants.sessionAccount] as Account;

			LoadButtons();
		}

		public void LoadButtons()
		{
			try
			{
				LoadData();

				BaseWebMailActions bwa = new BaseWebMailActions(_acct, this.Page);
                
				Folder CurrFolder = bwa.GetFolder(_id_folder);

                if(CurrFolder.Type == WebMailPro.FolderType.Trash)
				{
					IsTrash = true;
				}
				else
				{
					IsTrash = false;
				}

				if(Request.QueryString.Get("scr") == null)
				{
					screen = null;
				}
				else
				{
					screen = Request.QueryString.Get("scr").ToLower();
				}

				if(_acct.MailIncomingProtocol.ToString() == "Pop3")
				{
					IsPOP3 = true;
				}
				else
				{
					IsPOP3 = false;
				}

				HideButtons();
                
				switch(screen)
				{
                    case "condition_message":
				        BackToFollowUp.Visible = true;
                        Send.Visible = true;
                        Importance.Visible = true;
                        AddJavaScriptNewMessage();
                        break;
					case "new_message":
					case "drafts_message":
                        BackToList.Visible = true;
						Save.Visible = true;
						Send.Visible = true;
						Importance.Visible = true;
						AddJavaScriptNewMessage();
						break;
					case "view_message":
                        BackToList.Visible = true;
						NewMessage.Visible = true;
						Reply.Visible= true;
						Forward.Visible = true;
						Save.Visible = true;
						DeletePop.Visible = true;
						Print.Visible = true;
						break;
					case "settings_common":
					case "settings_accounts_properties":
					case "settings_accounts_filters":
					case "settings_accounts_signature":
					case "settings_accounts_folders":
					case "settings_contacts":
						break;
					case "contacts":
					case "contacts_view":
					case "contacts_add":
					case "contacts_import":
                        BackToList.Visible = true;
						NewMessageContacts.Visible = true;
						NewContact.Visible = true;
						NewGroup.Visible = true;
						AddContactsTo.Visible = true;
						PlaceholderContacts.Visible = true;
						ImportContacts.Visible = true;
						SearchContacts.Visible = true;
						LoadGroups(_acct);
						AddJavaScriptContacts();
						break;
					case "reply_message":
					case "replyall_message":
					case "forward_message":
                        BackToList.Visible = true;
						Save.Visible = true;
						Send.Visible = true;
						Importance.Visible = true;
						AddJavaScriptNewMessage();
						break;
					default:
						NewMessage.Visible = true;
						CheckMail.Visible = true;
						Reply.Visible= true;
						Forward.Visible = true;
						MarkAsRead.Visible = true;
						MoveToFolder.Visible = true;
						if(IsPOP3)
						{
							DeletePop.Visible = true;
						}
						else
						{
							DeleteImap.Visible = true;
							ReloadFoldersTree.Visible = true;
						}

						if(IsPOP3)
						{
							if(IsTrash)
							{
								EmptyTrash.Visible = true;
							}
							else
							{
								EmptyTrash.Visible = false;
							}
						}

						Search.Visible = true;
						if(IsSearch)
						{
							smallLookFor.Value = LookFor;
						}
						else
						{
							smallLookFor.Value = "";
						}

						LoadFolders(_acct);
						AddJavaScriptDefault();
						break;
				}
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void LoadData()
		{
			_id_folder = (Session["id_folder"] != null) ? (long)Session["id_folder"] : -1;
			//Current Search
			if(Session["is_search"] != null)
			{
				IsSearch = (bool)Session["is_search"];
				LookFor = (string)Session["look_for"];
			}
			else
			{
				IsSearch = false;
				LookFor = null;
			}
		}

		protected void LoadFolders(Account acct)
		{
			try
			{
				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

                DropDownListFolders.Items.Clear();
				FolderList = "";

				FolderCollection Folders = bwml.GetFoldersList(acct.ID, 0);

				ListItem FoldersItem = new ListItem();

				FoldersItem.Text = _manager.GetString("AllMailFolders");
				FoldersItem.Value = "-1";

				DropDownListFolders.Items.Add(FoldersItem);

				strpadding = "";

				TreeRecursiv(Folders, 0);
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void TreeRecursiv(FolderCollection folders, int padding)
		{
			foreach (Folder fld in folders)
			{
				if(fld.Hide) continue;

				ListItem FoldersItems = new ListItem();

				for(int i = 0; padding > i; i++)
				{
					strpadding += "&nbsp;";
				}

				FoldersItems.Text = Server.HtmlDecode(strpadding) + fld.Name;
				FoldersItems.Value = fld.ID.ToString();

				DropDownListFolders.Items.Add(FoldersItems);
				if (this._id_folder == fld.ID)
				{
					DropDownListFolders.SelectedIndex = folders.IndexOf(fld) + 1; // "All Folders" first item
				}

				FolderList += @"<div onmouseout=""this.className='wm_menu_item';"" onmouseover=""this.className='wm_menu_item_over';"" class=""wm_menu_item"" onclick=""MoveToFolder('" + fld.ID + @"');"">" + strpadding + fld.Name + @"</div>";
				
				if (fld.SubFolders.Count > 0)
				{
					TreeRecursiv(fld.SubFolders, padding + 4);
				}
				else
				{
					strpadding = "";
				}
			}
		}

		protected void LoadGroups(Account acct)
		{
			try
			{
				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				DropDownListGroups.Items.Clear();
				GroupList = "";

				AddressBookGroup[] Groups = bwml.GetGroups();

				ListItem GroupsItem = new ListItem();

				GroupsItem.Text = _manager.GetString("AllGroups");
				GroupsItem.Value = "-1";

				DropDownListGroups.Items.Add(GroupsItem);

				for(int i = 0; Groups.Length > i; i++)
				{
					ListItem GroupsItems = new ListItem();

					GroupsItems.Text = Groups[i].GroupName;
					GroupsItems.Value = Groups[i].IDGroup.ToString();

					DropDownListGroups.Items.Add(GroupsItems);

					GroupList += @"<div onmouseout=""this.className='wm_menu_item';"" onmouseover=""this.className='wm_menu_item_over';"" class=""wm_menu_item"" onclick=""AddContactsToGroup(" + Groups[i].IDGroup + @");"">" + Groups[i].GroupName + @"</div>";
				}

				GroupList += @"<div onmouseout=""this.className='wm_menu_item_spec';"" onmouseover=""this.className='wm_menu_item_over';"" class=""wm_menu_item_spec"" onclick=""javascript:NewGroup()"">- " + _manager.GetString("NewGroup") + @" -</div>";
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void AddJavaScriptDefault()
		{
			string text = String.Empty;

			if(IsTrash)
			{
				text = @"
					function DoDeleteMessages()
					{
						var idsArray = InboxLines.GetCheckedLines();
						
						if(idsArray.IdArray.length > 0)
						{
							if(confirm('Are you sure?'))
							{
								HFAction = document.getElementById(""HFAction"");
								HFRequest = document.getElementById(""HFRequest"");
								HFValue = document.getElementById(""HFValue"");
								
								HFAction.value = """";
								HFRequest.value = """";
								HFValue.value = """";

								HFAction.value = ""operation_messages"";
								HFRequest.value = ""delete"";

								for(i = 0; idsArray.IdArray.length > i; i++)
								{
									if(idsArray.IdArray.length != i+1)
									{
										HFValue.value += idsArray.IdArray[i] + ""@#%"";
									}
									else
									{
										HFValue.value += idsArray.IdArray[i];
									}
								}
								
								__doPostBack('PostBackButton','');
							}
						}
						else
						{
							alert(""" + _manager.GetString("WarningMarkListItem") + @""");
						}
					}
				";
			}
			else
			{
				text = @"
					function DoDeleteMessages()
					{
						var idsArray = InboxLines.GetCheckedLines();
						
						if(idsArray.IdArray.length > 0)
						{
							HFAction = document.getElementById(""HFAction"");
							HFRequest = document.getElementById(""HFRequest"");
							HFValue = document.getElementById(""HFValue"");
							
							HFAction.value = """";
							HFRequest.value = """";
							HFValue.value = """";

							HFAction.value = ""operation_messages"";
							HFRequest.value = ""delete"";

							for(i = 0; idsArray.IdArray.length > i; i++)
							{
								if(idsArray.IdArray.length != i+1)
								{
									HFValue.value += idsArray.IdArray[i] + ""@#%"";
								}
								else
								{
									HFValue.value += idsArray.IdArray[i];
								}
							}
							
							__doPostBack('PostBackButton','');
						}
						else
						{
							alert(""" + _manager.GetString("WarningMarkListItem") + @""");
						}
					}
				";
			}

			Log.WriteLine(@"JS Property", "JS ADD TEXT");
			_js.AddText(text);

			if(!IsPOP3)
			{
				text = @"PopupMenu.addItem(document.getElementById('popup_menu_13'), document.getElementById('popup_control_13'), 'wm_popup_menu', document.getElementById('popup_replace_13'), document.getElementById('popup_title_13'), 'wm_tb', 'wm_tb_press', 'wm_toolbar_item', 'wm_toolbar_item_over');";
			}
			else
			{
				text = String.Empty;
			}
			string InitText = text + @"
				PopupMenu.addItem(document.getElementById('popup_menu_4'), document.getElementById('popup_control_4'), 'wm_popup_menu', document.getElementById('popup_replace_4'), document.getElementById('popup_title_4'), 'wm_tb', 'wm_tb_press', 'wm_toolbar_item', 'wm_toolbar_item_over');
				PopupMenu.addItem(document.getElementById('popup_menu_3'), document.getElementById('popup_control_3'), 'wm_popup_menu', document.getElementById('popup_replace_3'), document.getElementById('popup_title_3'), 'wm_tb', 'wm_tb_press', 'wm_toolbar_item', 'wm_toolbar_item_over');
				PopupMenu.addItem(document.getElementById('popup_menu_2'), document.getElementById('popup_control_2'), 'wm_popup_menu', document.getElementById('popup_replace_2'), document.getElementById('popup_control_2'), 'wm_tb', 'wm_tb_press', 'wm_toolbar_item', 'wm_toolbar_item_over');
				SearchForm = new CSearchForm(document.getElementById('search_form'), document.getElementById('search_small_form'), document.getElementById('search_control'), document.getElementById('search_control_img'), 'search_form', document.getElementById('bigLookFor'), document.getElementById('Control_Toolbar_smallLookFor'), '" + Skin + @"');
				SearchForm.Show();
				/*document.body.onclick += ""SearchForm.checkVisibility(event, Browser.Mozilla);"";*/
			";

			js.AddInitText(InitText);
		}

		protected void AddJavaScriptContacts()
		{
			string text = null;

			text = @"
				function SubmitSearchContacts(e)
				{
					if (e.keyCode == 13)
					{
						DoSearchContacts();
					}
				}

				function SubmitSearchContactsAdvanced(e)
				{
					if (e.keyCode == 13)
					{
						DoSearchContactsAdvanced();
					}
				}

				function AddContactsToGroup(id)
				{
					HFAction = document.getElementById(""HFAction"");
					HFRequest = document.getElementById(""HFRequest"");
					HFValue = document.getElementById(""HFValue"");
					
					HFAction.value = """";
					HFRequest.value = """";
					HFValue.value = """";

					HFAction.value = ""add"";
					HFRequest.value = ""contact_to_group"";

					var arr = Selection.GetCheckedLines();
					var cg = """";

					if(arr.length > 0)
					{
						for(i = 0; arr.length > i; i++)
						{
							temp = arr[i].split(""_"");
							
							if(temp[0] == ""c"")
							{
								if(arr.length != i+1)
								{
									cg += temp[1] + ""----"";
								}
								else
								{
									cg += temp[1];
								}
							}
						}

						HFValue.value = id + ""----"" + cg;

						__doPostBack('PostBackButton','');
					}
					else
					{
						alert(""" + _manager.GetString("AlertNoContactsGroupsSelected") + @""");
					}
				}

				function DoSearchContacts()
				{
					HFAction = document.getElementById(""HFAction"");
					HFRequest = document.getElementById(""HFRequest"");
					HFValue = document.getElementById(""HFValue"");
					objInput = document.getElementById(""smallLookFor"");
					
					HFAction.value = """";
					HFRequest.value = """";
					HFValue.value = """";

					HFAction.value = ""search"";
					HFRequest.value = ""contacts_normal"";
					HFValue.value = objInput.value;

					__doPostBack('PostBackButton','');
				}

				function DoSearchContactsAdvanced()
				{
					HFAction = document.getElementById(""HFAction"");
					HFRequest = document.getElementById(""HFRequest"");
					HFValue = document.getElementById(""HFValue"");
					objLookFor = document.getElementById(""bigLookFor"");
					objQfolder = document.getElementById(""Control_Toolbar_DropDownListGroups"");

					HFAction.value = """";
					HFRequest.value = """";
					HFValue.value = """";

					HFAction.value = ""search"";
					HFRequest.value = ""contacts_advanced"";

					HFValue.value = objLookFor.value + ""@#%"" + objQfolder.value;

					__doPostBack('PostBackButton','');
				}

				function ImportContacts()
				{
					HFAction = document.getElementById(""HFAction"");
					HFRequest = document.getElementById(""HFRequest"");

					HFAction.value = ""import"";
					HFRequest.value = ""contacts"";

					__doPostBack('PostBackButton','');

				}

				function NewContact()
				{
					HFAction = document.getElementById(""HFAction"");
					HFRequest = document.getElementById(""HFRequest"");

					HFAction.value = ""new"";
					HFRequest.value = ""contact"";

					__doPostBack('PostBackButton','');
				}

				function NewGroup()
				{
					HFAction = document.getElementById(""HFAction"");
					HFRequest = document.getElementById(""HFRequest"");

					HFAction.value = ""new"";
					HFRequest.value = ""group"";

					__doPostBack('PostBackButton','');
				}

				function DoDelete()
				{
					HFAction = document.getElementById(""HFAction"");
					HFRequest = document.getElementById(""HFRequest"");
					HFValue = document.getElementById(""HFValue"");

					var arr = Selection.GetCheckedLines();
					var temp = """";
					var cg = """";

					if(arr.length > 0)
					{
						if(confirm('Are you sure?'))
						{
							HFAction.value = """";
							HFRequest.value = """";
							HFValue.value = """";

							for(i = 0; arr.length > i; i++)
							{
								if(arr.length != i+1)
								{
									cg += arr[i] + ""----"";
								}
								else
								{
									cg += arr[i];
								}
							}

							HFAction.value = ""delete"";
							HFRequest.value = ""contact"";
							HFValue.value = cg;

							__doPostBack('PostBackButton','');
						}
					}
					else
					{
						alert(""" + _manager.GetString("AlertNoContactsGroupsSelected") + @""");
					}
				}
			";

			_js.AddText(text);

			text = @"
				PopupMenu = new CPopupMenus();
				PopupMenu.addItem(document.getElementById('popup_menu_7'), document.getElementById('popup_control_7'), 'wm_popup_menu', document.getElementById('popup_control_7'), document.getElementById('popup_control_7'), 'wm_toolbar_item', 'wm_toolbar_item_press', 'wm_toolbar_item', 'wm_toolbar_item_over');
				SearchForm = new CSearchForm(document.getElementById('search_form'), document.getElementById('search_small_form'), document.getElementById('search_control'), document.getElementById('search_control_img'), 'search_form', document.getElementById('bigLookFor'), document.getElementById('smallLookFor'), '" + Skin + @"');
				SearchForm.Show();
			";

			_js.AddInitText(text);
		}

		protected void AddJavaScriptNewMessage()
		{
			string text = null;

			text = @"
				PriorityImg = document.getElementById(""priority_img"");
				PriorityText = document.getElementById(""priority_text"");
				PriorityInput = document.getElementById(""priority_input"");
			";
			_js.AddInitText(text);

			text = @"
				var PriorityImg, PriorityText, PriorityInput;

				function SetPriority(value)
				{
					switch (value) {
						case 5:
							PriorityInput.value = 5;
							PriorityImg.src = ""skins/" + _skin + @"/menu/priority_low.gif"";
							PriorityText.innerHTML = """ + _manager.GetString("Low") + @""";
						break;
						default:
						case 3:
							PriorityInput.value = 3;
							PriorityImg.src = ""skins/" + _skin + @"/menu/priority_normal.gif"";
							PriorityText.innerHTML = """ + _manager.GetString("Normal") + @""";
						break;
						case 1:
							PriorityInput.value = 1;
							PriorityImg.src = ""skins/" + _skin + @"/menu/priority_high.gif"";
							PriorityText.innerHTML = """ + _manager.GetString("High") + @""";
						break;
					}
				}

				function ChangePriority()
				{
					switch (PriorityInput.value) {
						case ""5"":
							SetPriority(3);
						break;
						default:
						case ""3"":
							SetPriority(1);
						break;
						case ""1"":
							SetPriority(5);
						break;
					}
				}

				function DoSendButton()
				{
					var toemail = document.getElementById(""newMessageID_toTextBox"").value;
					var subject = document.getElementById(""newMessageID_subjectTextBox"").value;

					if (toemail.length > 0)
					{
						if (subject.length == 0)
						{
							if (!confirm(Lang.ConfirmEmptySubject)) return false;
						}

						if (submitSaveMessage())
						{
							HFAction = document.getElementById(""HFAction"");
							HFRequest = document.getElementById(""HFRequest"");
							//HFValue = document.getElementById(""HFValue"");

							HFAction.value = ""send"";
							HFRequest.value = ""message"";
							//HFValue.value = lineid;

							__doPostBack('PostBackButton','');
						}
					}
					else
					{
						alert(""" + _manager.GetString("WarningToBlank")+ @""");
					}
				}

				function DoSaveButton()
				{
					if(submitSaveMessage())
					{
						HFAction = document.getElementById(""HFAction"");
						HFRequest = document.getElementById(""HFRequest"");
						//HFValue = document.getElementById(""HFValue"");
								
						HFAction.value = ""save"";
						HFRequest.value = ""message"";
						//HFValue.value = lineid;
								
						__doPostBack('PostBackButton','');
					}
				}

			";
			_js.AddText(text);
		}

		protected void HideButtons()
		{
            BackToFollowUp.Visible = false;
            BackToList.Visible = false;
			NewMessage.Visible = false;
			CheckMail.Visible = false;
			ReloadFoldersTree.Visible = false;
			Reply.Visible = false;
			Forward.Visible = false;
			MarkAsRead.Visible = false;
			MoveToFolder.Visible = false;
			DeletePop.Visible = false;
			EmptyTrash.Visible = false;
			DeleteImap.Visible = false;
			Search.Visible = false;
			Save.Visible = false;
			Send.Visible = false;
			Print.Visible = false;
			NewGroup.Visible = false;
			NewMessageContacts.Visible = false;
			NewContact.Visible = false;
			ImportContacts.Visible = false;
			Importance.Visible = false;
			AddContactsTo.Visible = false;
			PreviousMsg.Visible = false;
			NextMsg.Visible = false;
			PlaceholderContacts.Visible = false;
			SearchContacts.Visible = false;
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
