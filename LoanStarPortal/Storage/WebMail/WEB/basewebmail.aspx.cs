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
using System.Text.RegularExpressions;
using WebMailPro.classic;
using MailBee.Mime;
using System.IO;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for basewebmail.
	/// </summary>
	public partial class basewebmail : System.Web.UI.Page
	{
		protected WebMailPro.classic.jsbuilder js = new WebMailPro.classic.jsbuilder();
		protected WebmailResourceManager _resMan = null;
        
		protected WebMailPro.classic.MessageContainer Control_Messagecontainer;
		protected WebMailPro.classic.Folders_part Control_FoldersPart;
		protected WebMailPro.classic.Inbox Control_Maillist;
		protected WebMailPro.classic.Toolbar Control_Toolbar;
		protected WebMailPro.Copyright Control_Copyright;
		protected Account acct = null;
		protected string screen = null;
		protected string action = null;
		protected string request = null;

		protected string defaultSkin = Constants.defaultSkinName;
		protected string defaultTitle = null;

		protected int id = -1;
		protected string uid;
		protected long id_folder;
		protected string full_name_folder;
		protected int charset;
		protected MessageAction Action;
		protected bool IsSearch = false;
		protected string LookFor = null;
		protected int GroupID;
		protected long ContactID;
		protected bool IsContact;
		protected bool IsNewContact;
		protected int CurrentPage;
		protected int ContactsSortOrder;
		protected int ContactsSortField;
		protected bool SaveToDrafts = false;

		protected _DefaultScr ctrlDefault;
		protected NewMessage ctrlNewMessage;
		protected ViewMessage ctrlViewMessage;
		protected Contacts ctrlContacts;
		protected string strBodyOnClick = String.Empty;
		protected string AcctList = String.Empty;
		protected string delimeter = "";
        protected string checkAtL = "0";
        protected string parameters = string.Empty;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (Request.QueryString.Get("check") != null)
            {
                checkAtL = Request.QueryString.Get("check");
            }
            if (Request.QueryString.Get("scr") != null)
            {
                parameters = "?scr=" + Request.QueryString.Get("scr");
            }
            if (Request.QueryString.Get("to") != null)
            {
                if (parameters == string.Empty)
                {
                    parameters = "?to=" + Request.QueryString.Get("to");
                }
                else
                {
                    parameters += "&to=" + Request.QueryString.Get("to");
                }
            }
            Log.WriteLine("Page_Load", "Create Resource Manager");
			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			acct = Session[Constants.sessionAccount] as Account;
			Log.WriteLine("Page_Load", (acct == null) ? "Account is NULL" : "Account NOT NULL");
			Log.WriteLine("Session ID 2", Session.SessionID);
			Log.WriteLine("Session ID 2", Session.Keys.Count.ToString());
			if (acct != null)
			{
				try
				{
					if ((acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
					{
						WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

						delimeter = Utils.GetDelimeter();
                        
						defaultSkin = acct.UserOfAccount.Settings.DefaultSkin;
						defaultTitle = settings.SiteName;
					
						SwitchScreen();

						Control_Toolbar.js = js;

						BaseWebMailActions bwa = new BaseWebMailActions(acct, this.Page);

						Account[] Accounts = bwa.GetAccounts(acct.IDUser);

						if(Accounts.Length > 1)
						{
							string temp = @"PopupMenu.addItem(document.getElementById('popup_menu_1'), document.getElementById('popup_control_1'), 'wm_account_menu', document.getElementById('popup_replace_1'), document.getElementById('popup_replace_1'), '', '', '', '');";
							js.AddInitText(temp);
							temp = @"
							function ChangeAccount(id)
							{
								HFAction = document.getElementById('HFAction');
								HFRequest = document.getElementById('HFRequest');
								HFValue = document.getElementById('HFValue');
								
								HFAction.value = 'change';
								HFRequest.value = 'account';
								HFValue.value = id;
								
								__doPostBack('PostBackButton','');
							}
						";
							js.AddText(temp);

						}

						js.AddText(@"
	function GetServerElementByID(name)
	{
		var length = document.all.length;
		for (var i = 0; i < length; i++)
		{
			var elem = document.all[i];
			if (elem.id.toLowerCase().lastIndexOf(name.toLowerCase()) > 0)
			{
				return elem;
			}
		}
	}
");
						this.DataBind();

						return;
					}
				}
				catch (Exception ex)
				{
					((basewebmail)this.Page).OutputException(ex);
				}
			}
			else
			{
				Response.Redirect("default.aspx?error=1");
			}
		}

		protected void LoadSession()
		{
			//Current Folder
			if(Session["id_folder"] != null)
			{
				id_folder = (long)Session["id_folder"];
				full_name_folder = (string)Session["full_name_folder"];
			}
			else
			{
				id_folder = -1;
				full_name_folder = null;
			}

			//Current Mail
			if(Session["id"] != null)
			{
				id = (int)Session["id"];
				uid = (string)Session["uid"];
				charset = (int)Session["charset"];
				Action = (MessageAction)Session["Action"];
			}
			else
			{
				id = -1;
				uid = null;
				charset = -1;
				Action = MessageAction.New;	
			}

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

			//Contacts
			if(Session["current_page"] != null)
			{
				CurrentPage = (int)Session["current_page"];
			}
			else
			{
				CurrentPage = 1;
			}

			//Save to drafts
			if(Session["save_to_drafts"] != null)
			{
				SaveToDrafts = (bool)Session["save_to_drafts"];
			}
			else
			{
				SaveToDrafts = false;
			}
		}

		protected void SaveSession()
		{
			//Current Folder
			Session.Add("id_folder", id_folder);
			Session.Add("full_name_folder", full_name_folder);

			//Current Mail
			Session.Add("id", id);
			Session.Add("uid", uid);
			Session.Add("charset", charset);
			Session.Add("Action", Action);

			//Current Search
			Session.Add("is_search", IsSearch);
			Session.Add("look_for", LookFor);

			//Contacts
			Session.Add("current_page", CurrentPage);

			//Save to drafts
			Session.Add("save_to_drafts", SaveToDrafts);
		}

		//The given function checks type of the screen
		protected void SwitchScreen()
		{
			if(Request.QueryString.Get("scr") == null)
			{
				screen = null;
			}
			else
			{
				screen = Request.QueryString.Get("scr").ToLower();
			}

			switch (screen)
			{
				case "new_message":
					//
					ShowNewMessage();
					strBodyOnClick = "PopupMenu.checkShownItems();";
					break;
				case "reply_message":
					//
					ShowReplyMessage();
					strBodyOnClick = "PopupMenu.checkShownItems();";
					break;
				case "replyall_message":
					//
					ShowReplyAllMessage();
					strBodyOnClick = "PopupMenu.checkShownItems();";
					break;
				case "forward_message":
					//
					ShowForwardMessage();
					strBodyOnClick = "PopupMenu.checkShownItems();";
					break;
				case "drafts_message":
					//
					ShowDraftsMessage();
					strBodyOnClick = "PopupMenu.checkShownItems();";
					break;
				case "settings_common":
					//
					ShowSettings();
					strBodyOnClick = "PopupMenu.checkShownItems();";
					break;
				case "settings_accounts_properties":
					//
					ShowSettings();
					strBodyOnClick = "PopupMenu.checkShownItems();";
					break;
				case "settings_accounts_filters":
					//
					ShowSettings();
					strBodyOnClick = "PopupMenu.checkShownItems();";
					break;
				case "settings_accounts_signature":
					//
					ShowSettings();
					strBodyOnClick = "PopupMenu.checkShownItems();";
					break;
				case "settings_accounts_folders":
					//
					ShowSettings();
					strBodyOnClick = "PopupMenu.checkShownItems();";
					break;
				case "settings_contacts":
					//
					ShowSettings();
					strBodyOnClick = "PopupMenu.checkShownItems();";
					break;
				case "contacts":
					//
					ShowContacts();
					strBodyOnClick = "PopupMenu.checkShownItems(); SearchForm.checkVisibility(event, Browser.Mozilla);";
					break;
				case "contacts_view":
					//
					ShowContactsView();
					strBodyOnClick = "PopupMenu.checkShownItems(); SearchForm.checkVisibility(event, Browser.Mozilla);";
					break;
				case "contacts_add":
					//
					ShowContactsAdd();
					strBodyOnClick = "PopupMenu.checkShownItems(); SearchForm.checkVisibility(event, Browser.Mozilla);";
					break;
				case "contacts_import":
					//
					ShowContactsImport();
					strBodyOnClick = "PopupMenu.checkShownItems(); SearchForm.checkVisibility(event, Browser.Mozilla);";
					break;
				case "drafts":
					//
					ShowDrafts();
					strBodyOnClick = "PopupMenu.checkShownItems(); SearchForm.checkVisibility(event, Browser.Mozilla);";
					break;
				case "default":
					//
					ShowDefault();
					strBodyOnClick = "PopupMenu.checkShownItems(); SearchForm.checkVisibility(event, Browser.Mozilla);";
					break;
				case "view_message":
					//
					ShowViewMessage();
					strBodyOnClick = "PopupMenu.checkShownItems();";
					break;
				default:
					//defaut screen
					ShowDefault();
					strBodyOnClick = "PopupMenu.checkShownItems(); SearchForm.checkVisibility(event, Browser.Mozilla);";
					break;
			}
		}

		protected void ShowContactsImport()
		{
			PlaceHolderToolbar.Visible = true;

			ctrlContacts = LoadControl(@"classic\Contacts.ascx") as Contacts;
			if (ctrlContacts != null)
			{
				ctrlContacts.ID = "ContactsID";
				ctrlContacts.Skin = defaultSkin;
				ctrlContacts.js = js;
				PlaceHolder.Controls.Add(ctrlContacts);
			}

			PlaceHolderCopyright.Visible = true;
		}

		protected void ShowViewMessage()
		{
			if(!IsPostBack)
			{
				id = (int)Session["id"];
				uid = (string)Session["uid"];
				id_folder = (long)Session["id_folder"];
				full_name_folder = (string)Session["full_name_folder"];
				charset = (int)Session["charset"];
			}
			else
			{
				string[] arrTempMail = Regex.Split(HFValue.Value, "----");

				if(arrTempMail.Length > 1)
				{
					id = Convert.ToInt32(arrTempMail[0]);
					uid = arrTempMail[1];
					id_folder = Convert.ToInt64(arrTempMail[2]);
					full_name_folder = arrTempMail[3];
					charset = Convert.ToInt32(arrTempMail[4]);
				}
				else
				{
					id = -100;
				}
			}

			if(id != -100)
			{
				PlaceHolderToolbar.Visible = true;

				ctrlViewMessage = LoadControl(@"classic\ViewMessage.ascx") as ViewMessage;
				if (ctrlViewMessage != null)
				{
					ctrlViewMessage.ID = "newViewMessage";
					ctrlViewMessage.Skin = defaultSkin;
					ctrlViewMessage.js = js;

					ctrlViewMessage.id = id;
					ctrlViewMessage.uid = uid;
					ctrlViewMessage.id_folder = id_folder;
					ctrlViewMessage.full_name_folder = full_name_folder;
					ctrlViewMessage.charset = charset;

					PlaceHolder.Controls.Add(ctrlViewMessage);
				}

				PlaceHolderCopyright.Visible = false;

				HFValue.Value = id + "----" + uid + "----" + id_folder + "----" + full_name_folder + "----" + charset;
			}
		}

		protected void ShowDrafts()
		{
			try
			{
				js.AddText("var messageWithAttach = false;");

				if(!IsPostBack)
				{
					BaseWebMailActions actions = new BaseWebMailActions(acct, this.Page);
					Folder _folder = actions.GetFolder(FolderType.Drafts);

					if(!_folder.Hide)
					{
						id_folder = _folder.ID;
						full_name_folder = _folder.FullPath;
					}
					else
					{
						FolderCollection _folderCollection = actions.GetFoldersList(acct.ID, -1);

						Folder inbox = _folderCollection[Constants.FolderNames.Inbox];

						if (inbox != null)
						{
							if(!inbox.Hide)
							{
								id_folder = inbox.ID;
								full_name_folder = inbox.FullPath;
							}
							else
							{
								for(int i = 0; _folderCollection.Count > i; i++)
								{
									if(!_folderCollection[i].Hide)
									{
										id_folder = _folderCollection[i].ID;
										full_name_folder = _folderCollection[i].FullPath;
										break;
									}
								}
							}
						}
					}

					SaveSession();
				}
				else
				{
					LoadSession();
				}

				PlaceHolderToolbar.Visible = true;

				ctrlDefault = LoadControl(@"classic\Default.ascx") as _DefaultScr;
				if (ctrlDefault != null)
				{
					ctrlDefault.ID = "DefaultID"; 
					ctrlDefault.Skin = defaultSkin;
					ctrlDefault.js = js;
					PlaceHolder.Controls.Add(ctrlDefault);
				}

				PlaceHolderCopyright.Visible = false;
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void ShowDefault()
		{
			try
			{
				js.AddText("var messageWithAttach = false;");

				if(!IsPostBack)
				{
					BaseWebMailActions actions = new BaseWebMailActions(acct, this.Page);
					FolderCollection _folderCollection = actions.GetFoldersList(acct.ID, -1);

					Folder inbox = _folderCollection[Constants.FolderNames.Inbox];

					if (inbox != null)
					{
						if(!inbox.Hide)
						{
							id_folder = inbox.ID;
							full_name_folder = inbox.FullPath;
						}
						else
						{
							for(int i = 0; _folderCollection.Count > i; i++)
							{
								if(!_folderCollection[i].Hide)
								{
									id_folder = _folderCollection[i].ID;
									full_name_folder = _folderCollection[i].FullPath;
									break;
								}
							}
						}

						SaveSession();
					}
				}
				else
				{
					LoadSession();
				}

				PlaceHolderToolbar.Visible = true;

				ctrlDefault = LoadControl(@"classic\Default.ascx") as _DefaultScr;
				if (ctrlDefault != null)
				{
					ctrlDefault.ID = "DefaultID"; 
					ctrlDefault.Skin = defaultSkin;
					ctrlDefault.js = js;
					PlaceHolder.Controls.Add(ctrlDefault);
				}

				PlaceHolderCopyright.Visible = false;
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void ShowContacts()
		{
			PlaceHolderToolbar.Visible = true;

			ctrlContacts = LoadControl(@"classic\Contacts.ascx") as Contacts;
			if (ctrlContacts != null)
			{
				short sortField = 1;
				short sortOrder = 0;
				if (Session["contacts_sort_field"] != null)
				{
					sortField = ((this.HFAction.Value == "sort") && (this.HFRequest.Value == "contacts")) ? short.Parse(this.HFValue.Value) : short.Parse(Session["contacts_sort_field"].ToString());
					Session["contacts_sort_field"] = sortField;
				}
				else
				{
					Session["contacts_sort_field"] = sortField;
				}
				if (Session["contacts_sort_order"] != null)
				{
					sortOrder = short.Parse(Session["contacts_sort_order"].ToString());
					Session["contacts_sort_order"] = (sortOrder > 0) ? 0 : 1;
				}
				else
				{
					Session["contacts_sort_order"] = (sortOrder > 0) ? 0 : 1;
				}
				ctrlContacts.ID = "ContactsID";
				ctrlContacts.Skin = defaultSkin;
				ctrlContacts.js = js;
				ctrlContacts.SortField = sortField;
				ctrlContacts.SortOrder = sortOrder;
				PlaceHolder.Controls.Add(ctrlContacts);
			}

			PlaceHolderCopyright.Visible = true;
		}

		protected void ShowContactsView()
		{
			try
			{
				IsContact = (bool)Session["IsContact"];

				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				PlaceHolderToolbar.Visible = true;

				ctrlContacts = LoadControl(@"classic\Contacts.ascx") as Contacts;
				if (ctrlContacts != null)
				{
					ctrlContacts.ID = "ContactsID";
					ctrlContacts.Skin = defaultSkin;
					ctrlContacts.js = js;

					if(IsContact)
					{
						ContactID = (long)Session["ContactID"];

						ctrlContacts.AllGroups = bwml.GetGroups();
						ctrlContacts.Contact = bwml.GetContact(ContactID);
					}
					else
					{
						GroupID = (int)Session["GroupID"];

						ctrlContacts.Group = bwml.GetGroup(GroupID);
					}

					PlaceHolder.Controls.Add(ctrlContacts);
				}

				PlaceHolderCopyright.Visible = true;
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void ShowContactsAdd()
		{
			PlaceHolderToolbar.Visible = true;

			ctrlContacts = LoadControl(@"classic\Contacts.ascx") as Contacts;
			if (ctrlContacts != null)
			{
				ctrlContacts.ID = "ContactsID";
				ctrlContacts.Skin = defaultSkin;
				ctrlContacts.js = js;
				PlaceHolder.Controls.Add(ctrlContacts);
			}

			PlaceHolderCopyright.Visible = true;
		}

		protected void ShowSettings()
		{
			PlaceHolderToolbar.Visible = false;

			Settings ctrlSettings = LoadControl(@"classic\Settings.ascx") as Settings;
			if (ctrlSettings != null)
			{
				ctrlSettings.ID = "settingsID";
				ctrlSettings.Skin = defaultSkin;
				ctrlSettings.js = js;
				PlaceHolder.Controls.Add(ctrlSettings);
			}

			PlaceHolderCopyright.Visible = true;
		}

		protected void ShowNewMessage()
		{
			PlaceHolderToolbar.Visible = true;

			ctrlNewMessage = LoadControl(@"classic\NewMessage.ascx") as NewMessage;
			if (ctrlNewMessage != null)
			{
				ctrlNewMessage.ID = "newMessageID";
				ctrlNewMessage.Skin = defaultSkin;
				ctrlNewMessage.js = js;
				PlaceHolder.Controls.Add(ctrlNewMessage);
			}

			PlaceHolderCopyright.Visible = true;
		}

		protected void ShowDraftsMessage()
		{
			id = (int)Session["id"];
			uid = (string)Session["uid"];
			id_folder = (long)Session["id_folder"];
			full_name_folder = (string)Session["full_name_folder"];
			charset = (int)Session["charset"];
			Action = (MessageAction)Session["Action"];

			PlaceHolderToolbar.Visible = true;

			ctrlNewMessage = LoadControl(@"classic\NewMessage.ascx") as NewMessage;
			if (ctrlNewMessage != null)
			{
				ctrlNewMessage.ID = "newMessageID";
				ctrlNewMessage.Skin = defaultSkin;
				ctrlNewMessage.js = js;

				ctrlNewMessage.id = id;
				ctrlNewMessage.uid = uid;
				ctrlNewMessage.id_folder = id_folder;
				ctrlNewMessage.full_name_folder = full_name_folder;
				ctrlNewMessage.charset = charset;
				ctrlNewMessage.Action = Action;
				if ((HFAction.Value.ToLower()=="eis")&&(HFRequest.Value.ToLower()=="acceptnew")) ctrlNewMessage.AcceptFlag = true;
				PlaceHolder.Controls.Add(ctrlNewMessage);
			}

			PlaceHolderCopyright.Visible = true;
		}

		protected void ShowReplyMessage()
		{
			id = (int)Session["id"];
			uid = (string)Session["uid"];
			id_folder = (long)Session["id_folder"];
			full_name_folder = (string)Session["full_name_folder"];
			charset = (int)Session["charset"];
			Action = (MessageAction)Session["Action"];

			PlaceHolderToolbar.Visible = true;

			ctrlNewMessage = LoadControl(@"classic\NewMessage.ascx") as NewMessage;
			if (ctrlNewMessage != null)
			{
				ctrlNewMessage.ID = "newMessageID";
				ctrlNewMessage.Skin = defaultSkin;
				ctrlNewMessage.js = js;

				ctrlNewMessage.id = id;
				ctrlNewMessage.uid = uid;
				ctrlNewMessage.id_folder = id_folder;
				ctrlNewMessage.full_name_folder = full_name_folder;
				ctrlNewMessage.charset = charset;
				ctrlNewMessage.Action = Action;
				if ((HFAction.Value.ToLower()=="eis")&&(HFRequest.Value.ToLower()=="acceptnew")) ctrlNewMessage.AcceptFlag = true;
				PlaceHolder.Controls.Add(ctrlNewMessage);
			}

			PlaceHolderCopyright.Visible = true;
		}

		protected void ShowReplyAllMessage()
		{
			id = (int)Session["id"];
			uid = (string)Session["uid"];
			id_folder = (long)Session["id_folder"];
			full_name_folder = (string)Session["full_name_folder"];
			charset = (int)Session["charset"];
			Action = (MessageAction)Session["Action"];

			PlaceHolderToolbar.Visible = true;

			ctrlNewMessage = LoadControl(@"classic\NewMessage.ascx") as NewMessage;
			if (ctrlNewMessage != null)
			{
				ctrlNewMessage.ID = "newMessageID";
				ctrlNewMessage.Skin = defaultSkin;
				ctrlNewMessage.js = js;

				ctrlNewMessage.id = id;
				ctrlNewMessage.uid = uid;
				ctrlNewMessage.id_folder = id_folder;
				ctrlNewMessage.full_name_folder = full_name_folder;
				ctrlNewMessage.charset = charset;
				ctrlNewMessage.Action = Action;
				if ((HFAction.Value.ToLower()=="eis")&&(HFRequest.Value.ToLower()=="acceptnew")) ctrlNewMessage.AcceptFlag = true;
				PlaceHolder.Controls.Add(ctrlNewMessage);
			}

			PlaceHolderCopyright.Visible = true;
		}

		protected void ShowForwardMessage()
		{
			id = (int)Session["id"];
			uid = (string)Session["uid"];
			id_folder = (long)Session["id_folder"];
			full_name_folder = (string)Session["full_name_folder"];
			charset = (int)Session["charset"];
			Action = (MessageAction)Session["Action"];

			PlaceHolderToolbar.Visible = true;
			ctrlNewMessage = LoadControl(@"classic\NewMessage.ascx") as NewMessage;
			if (ctrlNewMessage != null)
			{
				ctrlNewMessage.ID = "newMessageID";
				ctrlNewMessage.Skin = defaultSkin;
				ctrlNewMessage.js = js;

				ctrlNewMessage.id = id;
				ctrlNewMessage.uid = uid;
				ctrlNewMessage.id_folder = id_folder;
				ctrlNewMessage.full_name_folder = full_name_folder;
				ctrlNewMessage.charset = charset;
				ctrlNewMessage.Action = Action;
				bool showPicturesSettings = false;
				if ((acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null)) {
					showPicturesSettings = ((acct.UserOfAccount.Settings.ViewMode & ViewMode.AlwaysShowPictures) > 0) ? true : false;
					ctrlNewMessage.AcceptFlag = showPicturesSettings;
				}
				if (!showPicturesSettings)
					if ((HFAction.Value.ToLower()=="eis")&&(HFRequest.Value.ToLower()=="acceptnew")) ctrlNewMessage.AcceptFlag = true;
				
				PlaceHolder.Controls.Add(ctrlNewMessage);
			}

			PlaceHolderCopyright.Visible = true;
		}

		protected void ShowMessage()
		{
			_DefaultScr ctrlDefault = LoadControl(@"classic\Default.ascx") as _DefaultScr;
			if (ctrlDefault != null)
			{
				ctrlDefault.ID = "DefaultID";
				ctrlDefault.Skin = defaultSkin;
				ctrlDefault.js = js;
				PlaceHolder.Controls.Add(ctrlDefault);
			}

			PlaceHolderCopyright.Visible = false;
		}


		private void PostBackButton_Click(object sender, EventArgs e)
		{
			if(HFAction.Value != null && HFRequest.Value != null)
			{
				action = HFAction.Value.ToLower();
				request = HFRequest.Value.ToLower();

				CheckAction(action, request);
			}
			else
			{
				action = null;
				request = null;

				Control_Messagecontainer.isPreviewPane = false;

				if(HFValue.Value != null)
				{
					Control_Maillist.PageNumber = Convert.ToInt32(HFValue.Value);
				}
			}
		}

		protected void eisAcceptOne()
		{
			ctrlDefault.Control_Messagecontainer.isPreviewPane = true;
			ctrlDefault.Control_Messagecontainer.AcceptOne();
			string[] arrTemp = Regex.Split(HFValue.Value, "----");
			ctrlDefault.Control_Messagecontainer.id_msg = Convert.ToInt32(arrTemp[0]);
			ctrlDefault.Control_Messagecontainer.uid = arrTemp[1];
			ctrlDefault.Control_Messagecontainer.id_folder = Convert.ToInt64(arrTemp[2]);
			ctrlDefault.Control_Messagecontainer.folder_full_name = arrTemp[3];
			ctrlDefault.Control_Messagecontainer.charset = Convert.ToInt32(arrTemp[4]);
			ctrlDefault.Control_Messagecontainer.Data_Bind(acct, 0);
			string[] tempArr = Regex.Split(HFPageInfo.Value, "----");
			ctrlDefault.Control_Maillist.PageNumber = Convert.ToInt32(tempArr[0]);
			ctrlDefault.Control_Maillist.OutputMessagesList(acct, Convert.ToInt64(arrTemp[2]));
			ctrlDefault.Control_FoldersPart.selectedFolderID = Convert.ToInt64(arrTemp[2]);
			ctrlDefault.Control_FoldersPart.OutputFoldersTree();

			string text = @"InboxLines.CheckLine(""" + Utils.EncodeJsSaveString(HFValue.Value) + @""");
				";
				
			js.AddInitText(text);
		}
		protected void eisAcceptSender()
		{
			ctrlDefault.Control_Messagecontainer.isPreviewPane = true;
			ctrlDefault.Control_Messagecontainer.AcceptAll();
			string[] arrTemp = Regex.Split(HFValue.Value, "----");
			ctrlDefault.Control_Messagecontainer.id_msg = Convert.ToInt32(arrTemp[0]);
			ctrlDefault.Control_Messagecontainer.uid = arrTemp[1];
			ctrlDefault.Control_Messagecontainer.id_folder = Convert.ToInt64(arrTemp[2]);
			ctrlDefault.Control_Messagecontainer.folder_full_name = arrTemp[3];
			ctrlDefault.Control_Messagecontainer.charset = Convert.ToInt32(arrTemp[4]);
			ctrlDefault.Control_Messagecontainer.Data_Bind(acct, 0);
			string[] tempArr = Regex.Split(HFPageInfo.Value, "----");
			ctrlDefault.Control_Maillist.PageNumber = Convert.ToInt32(tempArr[0]);
			ctrlDefault.Control_Maillist.OutputMessagesList(acct, Convert.ToInt64(arrTemp[2]));
			ctrlDefault.Control_FoldersPart.selectedFolderID = Convert.ToInt64(arrTemp[2]);
			ctrlDefault.Control_FoldersPart.OutputFoldersTree();

			string text = @"InboxLines.CheckLine(""" + Utils.EncodeJsSaveString(HFValue.Value) + @""");
			";
			
			js.AddInitText(text);
		}
		//-------------------------------------------

		protected void CheckAction(string action, string request)
		{
			switch (action)
			{
				case "eis":
					switch(request)
						{
							case "acceptone":
								eisAcceptOne();
								break;
						case "acceptall":
							eisAcceptSender();
							break;
						case "acceptnew":
							//check in shows!
							break;
					}
					break;
				case "change":
					switch(request)
					{
						case "account":
							ChangeAccount();
							break;
					}
					break;
				case "empty":
					switch(request)
					{
						case "trash":
							EmptyTrash();
							break;
					}
					break;
				case "import":
					switch(request)
					{
						case "contacts":
							ImportContacts();
							break;
					}
				break;
				case "get":
					switch(request)
					{
						case "settings_list":
							break;
						case "messages":
							GetMessages();
							break;
						case "message":
							GetMessage();
							break;
						case "message_full_screen":
							GetMessageFullScreen();
							break;
						case "contacts":
							break;
						case "groups":
							break;
						case "contact":
							GetContact();
							break;
						case "group":
							GetGroup();
							break;
						case "accounts":
							break;
						case "account":
							break;
						case "filters":
							break;
						case "filter":
							break;
						case "x_spam":
							break;
						case "contacts_settings":
							break;
					}
					break;
				case "update":
					break;
				case "send":
					SendMsg();
					break;
				case "save":
					SaveMsg();
					break;
				case "operation_messages":
					switch(request)
					{
						case "delete":
							Delete();
							break;
						case "undelete":
							UnDelete();
							break;
						case "purge":
							Purge();
							break;
						case "mark_read":
							MarkRead();
							break;
						case "mark_unread":
							MarkUnRead();
							break;
						case "flag":
							FlagMessages();
							break;
						case "unflag":
							UnFlagMessages();
							break;
						case "mark_all_read":
							MarkAllRead();
							break;
						case "mark_all_unread":
							MarkAllUnRead();
							break;
						case "move_to_folder":
							MoveToFolder();
							break;
					}
					break;
				case "new":
					switch(request)
					{
						case "contact":
							NewContact();
							break;
						case "group":
							NewGroup();
							break;
					}
					break;
				case "add":
					switch(request)
					{
						case "contact_to_group":
							AddContactToGroup();
							break;
					}
					break;
				case "delete":
					switch(request)
					{
						case "message":
							DeleteMessage();
							break;
						case "contact":
							DeleteContact();
							break;
					}
					break;
				case "reply":
					Reply();
					break;
				case "replyall":
					ReplyAll();
					break;
				case "forward":
					Forward();
					break;
				case "search":
					switch(request)
					{
						case "normal":
							SearchNormal();
							break;
						case "advanced":
							SearchAdvanced();
							break;
						case "contacts_normal":
							SearchContactsNormal();
							break;
						case "contacts_advanced":
							SearchContactsAdvanced();
							break;
					}
					break;
				case "sort":
					switch(request)
					{
						case "mail":
							SortMails();
							break;
						case "contacts":
							SortContacts();
							break;
					}
					break;
				case "reload_folders":
					ReloadFolders();
					break;
				case "switch":
					switch(request)
					{
						case "html":
							SwitchToHtml();
							break;
						case "plain":
							SwitchToPlain();
							break;
					}
					break;
			}
		}

		protected void AddContactToGroup()
		{
			try
			{
				string[] arrTempMail = Regex.Split(HFValue.Value, "----");
				
				BaseWebMailActions baseAction = new BaseWebMailActions(acct, this.Page);

				AddressBookGroup Group = baseAction.GetGroup(Convert.ToInt32(arrTempMail[0]));

				Group.Contacts = new AddressBookContact[arrTempMail.Length - 1];
				
				for(int i = 1; arrTempMail.Length > i; i++)
				{
					Group.Contacts[i-1] = new AddressBookContact();
					Group.Contacts[i-1].IDUser = acct.IDUser;
					Group.Contacts[i-1].IDAddr = Convert.ToInt32(arrTempMail[i]);
				}

				baseAction.UpdateGroup(Group);
				Session[Constants.sessionReportText] = _resMan.GetString("ReportContactAddedToGroup");

				Response.Redirect("basewebmail.aspx?scr=contacts");
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void ChangeAccount()
		{
			try
			{
				BaseWebMailActions bwa = new BaseWebMailActions(acct, this.Page);

				Account NewAcct = bwa.GetAccount(Convert.ToInt32(HFValue.Value));
							
				if(NewAcct != null)
				{
					Session.Add(Constants.sessionAccount, NewAcct);
					Response.Redirect("basewebmail.aspx");
				}
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void SwitchToHtml()
		{
			ctrlDefault.Control_Messagecontainer.isPreviewPane = true;
			string[] arrTemp = Regex.Split(HFValue.Value, "----");
			ctrlDefault.Control_Messagecontainer.id_msg = Convert.ToInt32(arrTemp[0]);
			ctrlDefault.Control_Messagecontainer.uid = arrTemp[1];
			ctrlDefault.Control_Messagecontainer.id_folder = Convert.ToInt64(arrTemp[2]);
			ctrlDefault.Control_Messagecontainer.folder_full_name = arrTemp[3];
			ctrlDefault.Control_Messagecontainer.charset = Convert.ToInt32(arrTemp[4]);
			ctrlDefault.Control_Messagecontainer.Data_Bind(acct, 1);
			string[] tempArr = Regex.Split(HFPageInfo.Value, "----");
			ctrlDefault.Control_Maillist.PageNumber = Convert.ToInt32(tempArr[0]);
			ctrlDefault.Control_Maillist.OutputMessagesList(acct, Convert.ToInt64(arrTemp[2]));
			ctrlDefault.Control_FoldersPart.selectedFolderID = Convert.ToInt64(arrTemp[2]);
			ctrlDefault.Control_FoldersPart.OutputFoldersTree();

			string text = @"InboxLines.CheckLine(""" + Utils.EncodeJsSaveString(HFValue.Value) + @""");
			";
			
			js.AddInitText(text);
		}

		protected void SwitchToPlain()
		{
			ctrlDefault.Control_Messagecontainer.isPreviewPane = true;
			string[] arrTemp = Regex.Split(HFValue.Value, "----");
			ctrlDefault.Control_Messagecontainer.id_msg = Convert.ToInt32(arrTemp[0]);
			ctrlDefault.Control_Messagecontainer.uid = arrTemp[1];
			ctrlDefault.Control_Messagecontainer.id_folder = Convert.ToInt64(arrTemp[2]);
			ctrlDefault.Control_Messagecontainer.folder_full_name = arrTemp[3];
			ctrlDefault.Control_Messagecontainer.charset = Convert.ToInt32(arrTemp[4]);
			ctrlDefault.Control_Messagecontainer.Data_Bind(acct, 2);
			string[] tempArr = Regex.Split(HFPageInfo.Value, "----");
			ctrlDefault.Control_Maillist.PageNumber = Convert.ToInt32(tempArr[0]);
			ctrlDefault.Control_Maillist.OutputMessagesList(acct, Convert.ToInt64(arrTemp[2]));
			ctrlDefault.Control_FoldersPart.selectedFolderID = Convert.ToInt64(arrTemp[2]);
			ctrlDefault.Control_FoldersPart.OutputFoldersTree();

			string text = @"InboxLines.CheckLine(""" + Utils.EncodeJsSaveString(HFValue.Value) + @""");
			";
			
			js.AddInitText(text);
		}

		protected void ReloadFolders()
		{
			LoadSession();
			ctrlDefault.Control_FoldersPart.Sync = 2;
			ctrlDefault.Control_FoldersPart.selectedFolderID = id_folder;
			ctrlDefault.Control_FoldersPart.OutputFoldersTree();
			ctrlDefault.Control_Messagecontainer.ShowHideElements(false);
			ctrlDefault.Control_Maillist.OutputMessagesList(acct, id_folder);
		}

		protected void SortContacts()
		{
			//Response.Redirect("basewebmail.aspx?scr=contacts");

//			short sortField = short.Parse(HFValue.Value);
//			Session["contacts_sort_field"] = sortField;
//			ctrlContacts.SortField = sortField;
//			ctrlContacts.IDGroup = -1;
//			ctrlContacts.LoadContactsGroups(acct);
		}

		protected void SortMails()
		{
			try
			{
				LoadSession();

				ctrlDefault.Control_FoldersPart.selectedFolderID = id_folder;
				ctrlDefault.Control_FoldersPart.OutputFoldersTree();
				ctrlDefault.Control_Messagecontainer.ShowHideElements(false);

				int SortOrder = -1;
				int SortField = -1;

				switch(Convert.ToInt32(HFValue.Value))
				{
					case 10:
						if(acct.DefaultOrder != (WebMailPro.DefaultOrder) Convert.ToInt32(HFValue.Value))
						{
							SortOrder = 0;
							SortField = 10;
						}
						else
						{
							SortOrder = 1;
							SortField = 10;
						}
						break;
					case 12:
						if(acct.DefaultOrder != (WebMailPro.DefaultOrder) Convert.ToInt32(HFValue.Value))
						{
							SortOrder = 0;
							SortField = 12;
						}
						else
						{
							SortOrder = 1;
							SortField = 12;
						}
						break;
					case 2:
						if(acct.DefaultOrder != (WebMailPro.DefaultOrder) Convert.ToInt32(HFValue.Value))
						{
							SortOrder = 0;
							SortField = 2;
						}
						else
						{
							SortOrder = 1;
							SortField = 2;
						}
						break;
					case 0:
						if(acct.DefaultOrder != (WebMailPro.DefaultOrder) Convert.ToInt32(HFValue.Value))
						{
							SortOrder = 0;
							SortField = 0;
						}
						else
						{
							SortOrder = 1;
							SortField = 0;
						}
						break;
					case 6:
						if(acct.DefaultOrder != (WebMailPro.DefaultOrder) Convert.ToInt32(HFValue.Value))
						{
							SortOrder = 0;
							SortField = 6;
						}
						else
						{
							SortOrder = 1;
							SortField = 6;
						}
						break;
					case 8:
						if(acct.DefaultOrder != (WebMailPro.DefaultOrder) Convert.ToInt32(HFValue.Value))
						{
							SortOrder = 0;
							SortField = 8;
						}
						else
						{
							SortOrder = 1;
							SortField = 8;
						}
						break;
				}
			
				acct.DefaultOrder = (WebMailPro.DefaultOrder) (SortOrder + SortField);
				acct.Update(false);

				if (acct != Session[Constants.sessionAccount])
				{
					Session[Constants.sessionAccount] = acct;
				}

				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);
				ctrlDefault.Control_Maillist.OutputMessagesList(acct, id_folder);
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void EmptyTrash()
		{
			try
			{
				LoadSession();

				WebMailMessageCollection messages = new WebMailMessageCollection();
			
				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				bwml.GroupOperations(Constants.GroupOperationsRequests.Delete, id_folder, full_name_folder, "", -1, -1, "", messages);
			
				ctrlDefault.Control_Maillist.OutputMessagesList(acct, id_folder);
				ctrlDefault.Control_FoldersPart.selectedFolderID = id_folder;
				ctrlDefault.Control_FoldersPart.OutputFoldersTree();
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void FlagMessages()
		{
			try
			{
				LoadSession();

				WebMailMessageCollection messages = new WebMailMessageCollection();

				string[] arrTemp = Regex.Split(HFValue.Value, "@#%");
				string[] arrTempMail  = null;

				for(int i = 0; arrTemp.Length > i; i++)
				{
					arrTempMail = Regex.Split(arrTemp[i], "----");

					WebMailMessage msg = new WebMailMessage(acct);

					msg.IDMsg = Convert.ToInt32(arrTempMail[0]);
					if(acct.MailIncomingProtocol.ToString() != "Pop3")
					{
						msg.IntUid = Convert.ToInt64(arrTempMail[1]);
					}
					else
					{
						msg.StrUid = arrTempMail[1];
					}
					msg.IDFolderDB = Convert.ToInt64(arrTempMail[2]);
					msg.FolderFullName = arrTempMail[3];
					msg.OverrideCharset = Convert.ToInt32(arrTempMail[4]);

					messages.Add(msg);
				}
			
				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				bwml.GroupOperations(Constants.GroupOperationsRequests.Flag, id_folder, full_name_folder, "", -1, -1, "", messages);

				ctrlDefault.Control_Maillist.OutputMessagesList(acct, id_folder);
				ctrlDefault.Control_FoldersPart.selectedFolderID = id_folder;
				ctrlDefault.Control_FoldersPart.OutputFoldersTree();
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void UnFlagMessages()
		{
			try
			{
				LoadSession();

				WebMailMessageCollection messages = new WebMailMessageCollection();

				string[] arrTemp = Regex.Split(HFValue.Value, "@#%");
				string[] arrTempMail  = null;

				for(int i = 0; arrTemp.Length > i; i++)
				{
					arrTempMail = Regex.Split(arrTemp[i], "----");

					WebMailMessage msg = new WebMailMessage(acct);

					msg.IDMsg = Convert.ToInt32(arrTempMail[0]);
					if(acct.MailIncomingProtocol.ToString() != "Pop3")
					{
						msg.IntUid = Convert.ToInt64(arrTempMail[1]);
					}
					else
					{
						msg.StrUid = arrTempMail[1];
					}
					msg.IDFolderDB = Convert.ToInt64(arrTempMail[2]);
					msg.FolderFullName = arrTempMail[3];
					msg.OverrideCharset = Convert.ToInt32(arrTempMail[4]);

					messages.Add(msg);
				}
			
				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				bwml.GroupOperations(Constants.GroupOperationsRequests.Unflag, id_folder, full_name_folder, "", -1, -1, "", messages);

				ctrlDefault.Control_Maillist.OutputMessagesList(acct, id_folder);
				ctrlDefault.Control_FoldersPart.selectedFolderID = id_folder;
				ctrlDefault.Control_FoldersPart.OutputFoldersTree();
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void SearchContactsNormal()
		{
			ctrlContacts.LookFor = HFValue.Value;
			ctrlContacts.IDGroup = -1;
			ctrlContacts.LoadContactsGroups(acct);
		}

		protected void SearchContactsAdvanced()
		{
			string[] arrTemp = Regex.Split(HFValue.Value, "@#%");
			ctrlContacts.LookFor = arrTemp[0];
			ctrlContacts.IDGroup = Convert.ToInt32(arrTemp[1]);
			ctrlContacts.LoadContactsGroups(acct);
		}

		protected void SearchNormal()
		{
			if (HFValue.Value.Length > 0)
			{
				IsSearch = true;
				LookFor = HFValue.Value;
			}
			else
			{
				LookFor = string.Empty;
				IsSearch = false;
			}
			SaveSession();
			ctrlDefault.Control_Maillist.LookFor = LookFor;
			ctrlDefault.Control_FoldersPart.selectedFolderID = this.id_folder;
			ctrlDefault.Control_Maillist.OutputMessagesList(acct, this.id_folder);
			ctrlDefault.Control_FoldersPart.OutputFoldersTree();
			Control_Toolbar.LoadButtons();
		}

		protected void SearchAdvanced()
		{
			LoadSession();
			string[] arrTemp = Regex.Split(HFValue.Value, "@#%");
			IsSearch = true;
			LookFor = arrTemp[0];
			long fldID = Convert.ToInt32(arrTemp[1]);
			SaveSession();
			ctrlDefault.Control_Maillist.LookFor = LookFor;
			ctrlDefault.Control_Maillist.SearchFields = (arrTemp[2] == "true") ? 0 : 1;
			if ((LookFor == null) || (LookFor.Length == 0))
			{
				if (fldID < 0)
				{
					BaseWebMailActions bwa = new BaseWebMailActions(acct, this.Page);
					FolderCollection fc = bwa.GetFoldersList(acct.IDUser, -1);
					foreach (Folder fld in fc)
					{
						if (fld.Type == FolderType.Inbox)
						{
							id_folder = fld.ID;
							break;
						}
					}
				}
				ctrlDefault.Control_Maillist.OutputMessagesList(acct, id_folder);
			}
			else
			{
				ctrlDefault.Control_Maillist.OutputMessagesList(acct, fldID);
			}
			ctrlDefault.Control_FoldersPart.selectedFolderID = Convert.ToInt32(arrTemp[1]);
			ctrlDefault.Control_FoldersPart.OutputFoldersTree();
			Control_Toolbar.LoadButtons();
		}

		protected void ImportContacts()
		{
			Response.Redirect("basewebmail.aspx?scr=contacts_import");
		}

		protected void GetMessageFullScreen()
		{
			string[] arrTemp = Regex.Split(HFValue.Value, "----");
			id = Convert.ToInt32(arrTemp[0]);
			uid = arrTemp[1];
			id_folder = Convert.ToInt64(arrTemp[2]);
			full_name_folder = arrTemp[3];
			charset = Convert.ToInt32(arrTemp[4]);

			Session.Add("id", id);
			Session.Add("uid", uid);
			Session.Add("id_folder", id_folder);
			Session.Add("full_name_folder", full_name_folder);
			Session.Add("charset", charset);

			BaseWebMailActions bwa = new BaseWebMailActions(acct, this.Page);

			Folder _folder = bwa.GetFolder(id_folder);

			if(_folder.Type != FolderType.Drafts)
			{
				Response.Redirect("basewebmail.aspx?scr=view_message");
			}
			else
			{
				Action = MessageAction.New;
				Session.Add("Action", Action);
				Response.Redirect("basewebmail.aspx?scr=drafts_message");
			}
		}

		protected void NewGroup()
		{
			IsNewContact = false;
			
			Session.Add("IsNewContact", IsNewContact);

			Response.Redirect("basewebmail.aspx?scr=contacts_add");
		}

		protected void NewContact()
		{
			IsNewContact = true;
			
			Session.Add("IsNewContact", IsNewContact);
			if(HFValue.Value.Length > 0)
			{
				string[] arrTemp = Regex.Split(HFValue.Value, "@#%");
				string Name = arrTemp[1];
				string email = arrTemp[0];

				Session.Add("NewContactName", Name);
				Session.Add("NewContactEmail", email);
			}

			Response.Redirect("basewebmail.aspx?scr=contacts_add");
		}

		protected void GetGroup()
		{
			GroupID = Convert.ToInt32(HFValue.Value);

			IsContact = false;
			
			Session.Add("GroupID", GroupID);
			Session.Add("IsContact", IsContact);

			Response.Redirect("basewebmail.aspx?scr=contacts_view");
		}

		protected void GetContact()
		{
			ContactID = Convert.ToInt64(HFValue.Value);

			IsContact = true;
			
			Session.Add("ContactID", ContactID);
			Session.Add("IsContact", IsContact);

			Response.Redirect("basewebmail.aspx?scr=contacts_view");
		}

		protected void Reply()
		{
			string[] arrTemp = Regex.Split(HFValue.Value, "----");
			id = Convert.ToInt32(arrTemp[0]);
			uid = arrTemp[1];
			id_folder = Convert.ToInt64(arrTemp[2]);
			full_name_folder = arrTemp[3];
			charset = Convert.ToInt32(arrTemp[4]);
			Action = MessageAction.Reply;

			Session.Add("id", id);
			Session.Add("uid", uid);
			Session.Add("id_folder", id_folder);
			Session.Add("full_name_folder", full_name_folder);
			Session.Add("charset", charset);
			Session.Add("Action", Action);

			Response.Redirect("basewebmail.aspx?scr=reply_message");
		}

		protected void ReplyAll()
		{
			string[] arrTemp = Regex.Split(HFValue.Value, "----");
			id = Convert.ToInt32(arrTemp[0]);
			uid = arrTemp[1];
			id_folder = Convert.ToInt64(arrTemp[2]);
			full_name_folder = arrTemp[3];
			charset = Convert.ToInt32(arrTemp[4]);
			Action = MessageAction.ReplyAll;

			Session.Add("id", id);
			Session.Add("uid", uid);
			Session.Add("id_folder", id_folder);
			Session.Add("full_name_folder", full_name_folder);
			Session.Add("charset", charset);
			Session.Add("Action", Action);

			Response.Redirect("basewebmail.aspx?scr=replyall_message");
		}

		protected void Forward()
		{
			string[] arrTemp = Regex.Split(HFValue.Value, "----");
			id = Convert.ToInt32(arrTemp[0]);
			uid = arrTemp[1];
			id_folder = Convert.ToInt64(arrTemp[2]);
			full_name_folder = arrTemp[3];
			charset = Convert.ToInt32(arrTemp[4]);
			Action = MessageAction.Forward;

			Session.Add("id", id);
			Session.Add("uid", uid);
			Session.Add("id_folder", id_folder);
			Session.Add("full_name_folder", full_name_folder);
			Session.Add("charset", charset);
			Session.Add("Action", Action);

			Response.Redirect("basewebmail.aspx?scr=forward_message");
		}

		protected void MarkRead()
		{
			try
			{
				LoadSession();

				WebMailMessageCollection messages = new WebMailMessageCollection();

				string[] arrTemp = Regex.Split(HFValue.Value, "@#%");
				string[] arrTempMail  = null;

				for(int i = 0; arrTemp.Length > i; i++)
				{
					arrTempMail = Regex.Split(arrTemp[i], "----");

					WebMailMessage msg = new WebMailMessage(acct);

					msg.IDMsg = Convert.ToInt32(arrTempMail[0]);
					if(acct.MailIncomingProtocol.ToString() != "Pop3")
					{
						msg.IntUid = Convert.ToInt64(arrTempMail[1]);
					}
					else
					{
						msg.StrUid = arrTempMail[1];
					}
					msg.IDFolderDB = Convert.ToInt64(arrTempMail[2]);
					msg.FolderFullName = arrTempMail[3];
					msg.OverrideCharset = Convert.ToInt32(arrTempMail[4]);

					messages.Add(msg);
				}
			
				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				bwml.GroupOperations(Constants.GroupOperationsRequests.MarkRead, id_folder, full_name_folder, "", -1, -1, "", messages);

				ctrlDefault.Control_Maillist.OutputMessagesList(acct, id_folder);
				ctrlDefault.Control_FoldersPart.selectedFolderID = id_folder;
				ctrlDefault.Control_FoldersPart.OutputFoldersTree();
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void MarkUnRead()
		{
			try
			{
				LoadSession();

				WebMailMessageCollection messages = new WebMailMessageCollection();

				string[] arrTemp = Regex.Split(HFValue.Value, "@#%");
				string[] arrTempMail  = null;

				for(int i = 0; arrTemp.Length > i; i++)
				{
					arrTempMail = Regex.Split(arrTemp[i], "----");

					WebMailMessage msg = new WebMailMessage(acct);

					msg.IDMsg = Convert.ToInt32(arrTempMail[0]);
					if(acct.MailIncomingProtocol.ToString() != "Pop3")
					{
						msg.IntUid = Convert.ToInt64(arrTempMail[1]);
					}
					else
					{
						msg.StrUid = arrTempMail[1];
					}
					msg.IDFolderDB = Convert.ToInt64(arrTempMail[2]);
					msg.FolderFullName = arrTempMail[3];
					msg.OverrideCharset = Convert.ToInt32(arrTempMail[4]);

					messages.Add(msg);
				}
			
				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				bwml.GroupOperations(Constants.GroupOperationsRequests.MarkUnread, id_folder, full_name_folder, "", -1, -1, "", messages);

				ctrlDefault.Control_Maillist.OutputMessagesList(acct, id_folder);
				ctrlDefault.Control_FoldersPart.selectedFolderID = id_folder;
				ctrlDefault.Control_FoldersPart.OutputFoldersTree();
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void MarkAllRead()
		{
			try
			{
				LoadSession();

				WebMailMessageCollection messages = new WebMailMessageCollection();
			
				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				bwml.GroupOperations(Constants.GroupOperationsRequests.MarkAllRead, id_folder, full_name_folder, "", -1, -1, "", messages);
			
				ctrlDefault.Control_Maillist.OutputMessagesList(acct, id_folder);
				ctrlDefault.Control_FoldersPart.selectedFolderID = id_folder;
				ctrlDefault.Control_FoldersPart.OutputFoldersTree();
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void MarkAllUnRead()
		{
			try
			{
				LoadSession();

				WebMailMessageCollection messages = new WebMailMessageCollection();
			
				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				bwml.GroupOperations(Constants.GroupOperationsRequests.MarkAllUnread, id_folder, full_name_folder, "", -1, -1, "", messages);

				ctrlDefault.Control_Maillist.OutputMessagesList(acct, id_folder);
				ctrlDefault.Control_FoldersPart.selectedFolderID = id_folder;
				ctrlDefault.Control_FoldersPart.OutputFoldersTree();
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void MoveToFolder()
		{
			try
			{
				LoadSession();

				WebMailMessageCollection messages = new WebMailMessageCollection();

				string[] arrTempFolderID = Regex.Split(HFValue.Value, "@-@");
				string[] arrTemp = Regex.Split(arrTempFolderID[0], "@#%");
				string[] arrTempMail  = null;

				for(int i = 0; arrTemp.Length > i; i++)
				{
					arrTempMail = Regex.Split(arrTemp[i], "----");

					WebMailMessage msg = new WebMailMessage(acct);

					msg.IDMsg = Convert.ToInt32(arrTempMail[0]);
					if(acct.MailIncomingProtocol.ToString() != "Pop3")
					{
						msg.IntUid = Convert.ToInt64(arrTempMail[1]);
					}
					else
					{
						msg.StrUid = arrTempMail[1];
					}
					msg.IDFolderDB = Convert.ToInt64(arrTempMail[2]);
					msg.FolderFullName = arrTempMail[3];
					msg.OverrideCharset = Convert.ToInt32(arrTempMail[4]);

					messages.Add(msg);
				}
			
				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				Folder folder = bwml.GetFolder(Convert.ToInt64(arrTempFolderID[1]));

				bwml.GroupOperations(Constants.GroupOperationsRequests.MoveToFolder, id_folder, full_name_folder, "", -1, Convert.ToInt32(arrTempFolderID[1]), folder.FullPath, messages);

				ctrlDefault.Control_Maillist.OutputMessagesList(acct, id_folder);
				ctrlDefault.Control_FoldersPart.selectedFolderID = id_folder;
				ctrlDefault.Control_FoldersPart.OutputFoldersTree();
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void Purge()
		{
			try
			{
				LoadSession();

				WebMailMessageCollection messages = new WebMailMessageCollection();
			
				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				bwml.GroupOperations(Constants.GroupOperationsRequests.Purge, id_folder, full_name_folder, "", -1, -1, "", messages);

				ctrlDefault.Control_Maillist.OutputMessagesList(acct, id_folder);
				ctrlDefault.Control_FoldersPart.selectedFolderID = id_folder;
				ctrlDefault.Control_FoldersPart.OutputFoldersTree();
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void UnDelete()
		{
			try
			{
				LoadSession();

				WebMailMessageCollection messages = new WebMailMessageCollection();

				string[] arrTemp = Regex.Split(HFValue.Value, "@#%");
				string[] arrTempMail  = null;

				for(int i = 0; arrTemp.Length > i; i++)
				{
					arrTempMail = Regex.Split(arrTemp[i], "----");

					WebMailMessage msg = new WebMailMessage(acct);

					msg.IDMsg = Convert.ToInt32(arrTempMail[0]);
					if(acct.MailIncomingProtocol.ToString() != "Pop3")
					{
						msg.IntUid = Convert.ToInt64(arrTempMail[1]);
					}
					else
					{
						msg.StrUid = arrTempMail[1];
					}
					msg.IDFolderDB = Convert.ToInt64(arrTempMail[2]);
					msg.FolderFullName = arrTempMail[3];
					msg.OverrideCharset = Convert.ToInt32(arrTempMail[4]);

					messages.Add(msg);
				}

				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				bwml.GroupOperations(Constants.GroupOperationsRequests.Purge, id_folder, full_name_folder, "", -1, -1, "", messages);

				ctrlDefault.Control_Maillist.OutputMessagesList(acct, id_folder);
				ctrlDefault.Control_FoldersPart.selectedFolderID = id_folder;
				ctrlDefault.Control_FoldersPart.OutputFoldersTree();
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void Delete()
		{
			try
			{
				LoadSession();

				WebMailMessageCollection messages = new WebMailMessageCollection();

				string[] arrTemp = Regex.Split(HFValue.Value, "@#%");
				string[] arrTempMail  = null;

				for(int i = 0; arrTemp.Length > i; i++)
				{
					arrTempMail = Regex.Split(arrTemp[i], "----");

					WebMailMessage msg = new WebMailMessage(acct);

					msg.IDMsg = Convert.ToInt32(arrTempMail[0]);
					if(acct.MailIncomingProtocol.ToString() != "Pop3")
					{
						msg.IntUid = Convert.ToInt64(arrTempMail[1]);
					}
					else
					{
						msg.StrUid = arrTempMail[1];
					}
					msg.IDFolderDB = Convert.ToInt64(arrTempMail[2]);
					msg.FolderFullName = arrTempMail[3];
					msg.OverrideCharset = Convert.ToInt32(arrTempMail[4]);

					messages.Add(msg);
				}

				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				bwml.GroupOperations(Constants.GroupOperationsRequests.Delete, id_folder, full_name_folder, "", -1, -1, "", messages);

				ctrlDefault.Control_Maillist.OutputMessagesList(acct, id_folder);
				ctrlDefault.Control_FoldersPart.selectedFolderID = id_folder;
				ctrlDefault.Control_FoldersPart.OutputFoldersTree();
				ctrlDefault.Control_Messagecontainer.ShowHideElements(false);
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void DeleteContact()
		{
			try
			{
				string[] arrTempMail = Regex.Split(HFValue.Value, "----");
				string[] tempContactsGroups = null;

				ArrayList arrContacts = new ArrayList();
				ArrayList arrGroups = new ArrayList(); 

				for(int i = 0; arrTempMail.Length > i; i++)
				{
					tempContactsGroups = Regex.Split(arrTempMail[i], "_");
					if(tempContactsGroups[0] == "c")
					{
						AddressBookContact Contact = new AddressBookContact();
						Contact.IDUser = acct.IDUser;
						Contact.IDAddr = Convert.ToInt32(tempContactsGroups[1]);
						arrContacts.Add(Contact);
					}
					else
					{
						AddressBookGroup Group = new AddressBookGroup();
						Group.IDUser = acct.IDUser;
						Group.IDGroup = Convert.ToInt32(tempContactsGroups[1]);
						arrGroups.Add(Group);
					}
				}

				AddressBookContact[] Contacts = new AddressBookContact[arrContacts.Count];
				AddressBookGroup[] Groups = new AddressBookGroup[arrGroups.Count];

				arrContacts.CopyTo(Contacts);
				arrGroups.CopyTo(Groups);

				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				bwml.DeleteContacts(Contacts, Groups);

				Response.Redirect("basewebmail.aspx?scr=contacts");
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void DeleteMessage()
		{
			try
			{
				LoadSession();

				WebMailMessageCollection messages = new WebMailMessageCollection();

				string[] arrTempMail = Regex.Split(HFValue.Value, "----");

				WebMailMessage msg = new WebMailMessage(acct);

				msg.IDMsg = Convert.ToInt32(arrTempMail[0]);
				if(acct.MailIncomingProtocol.ToString() != "Pop3")
				{
					msg.IntUid = Convert.ToInt64(arrTempMail[1]);
				}
				else
				{
					msg.StrUid = arrTempMail[1];
				}
				msg.IDFolderDB = Convert.ToInt64(arrTempMail[2]);
				msg.FolderFullName = arrTempMail[3];
				msg.OverrideCharset = Convert.ToInt32(arrTempMail[4]);

				messages.Add(msg);

				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				bwml.GroupOperations(Constants.GroupOperationsRequests.Delete, id_folder, full_name_folder, "", -1, -1, "", messages);

				Response.Redirect("basewebmail.aspx?scr=default");
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void SendMsg()
		{
			try
			{
                BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);
                if (bwml.CurrentAccount != null && bwml.CurrentAccount.IsDemo)
                {
                    if (Session["sendCount"] != null)
                    {
                        int sendCount = 0;
                        try
                        {
                            sendCount = int.Parse((string)Session["sendCount"]);
                        }
                        catch { }
                        if (sendCount >= 3)
                        {
                            Session[Constants.sessionReportText] = "To prevent abuse, no more than 3 e-mails per session is allowed in this demo. To send more e-mails, start another session.";
                            Response.Redirect("basewebmail.aspx?scr=default");
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
                int id_msg = Convert.ToInt32(Request.Form["id_message"]);
                string From = Request.Form[ctrlNewMessage.FindControl("fromTextBox").UniqueID.ToString()];
                string To = Request.Form[ctrlNewMessage.FindControl("toTextBox").UniqueID.ToString()];
                string Cc = Request.Form[ctrlNewMessage.FindControl("ccTextBox").UniqueID.ToString()];
                string Bcc = Request.Form[ctrlNewMessage.FindControl("bccTextBox").UniqueID.ToString()];
                string Subject = Request.Form[ctrlNewMessage.FindControl("subjectTextBox").UniqueID.ToString()];
                string Priority = Request.Form["priority_input"];
				string AttachmentTempName = null;
				string AttachmentName = null;
				DateTime Date = DateTime.Now;
				string Charset = Utils.GetEncodingByCodePage(acct.UserOfAccount.Settings.DefaultCharsetOut).HeaderName;

				bool IsHTML = false;

				if(Request.Form["ishtml"] == "1")
				{
					IsHTML = true;
				}
				else
				{
					IsHTML = false;
				}

				string Text = Request.Form["message"];

				MailMessage msg = new MailMessage();
			
				msg.From = EmailAddress.Parse(From);

                bool isAddrDiscard = false;
                EmailAddressCollection toColl = EmailAddressCollection.Parse(To);
                EmailAddressCollection ccColl = EmailAddressCollection.Parse(Cc);
                EmailAddressCollection bccColl = EmailAddressCollection.Parse(Bcc);
                if (bwml.CurrentAccount != null && bwml.CurrentAccount.IsDemo)
                {
                    int toCount = toColl.Count;
                    int ccCount = ccColl.Count;
                    int bccCount = bccColl.Count;
                    if (toCount > 3)
                    {
                        EmailAddressCollection toAddr = new EmailAddressCollection();
                        for (int i = 0; i < 3; i++)
                        {
                            toAddr.Add(toColl[i]);
                        }
                        toColl = toAddr;
                        ccColl = new EmailAddressCollection();
                        bccColl = new EmailAddressCollection();
                        isAddrDiscard = true;
                    }
                    else if (ccCount > 3 - toCount)
                    {
                        EmailAddressCollection ccAddr = new EmailAddressCollection();
                        for (int i = 0; i < 3 - toCount; i++)
                        {
                            ccAddr.Add(ccColl[i]);
                        }
                        ccColl = ccAddr;
                        bccColl = new EmailAddressCollection();
                        isAddrDiscard = true;
                    }
                    else if (bccCount > 3 - toCount - ccCount)
                    {
                        EmailAddressCollection bccAddr = new EmailAddressCollection();
                        for (int i = 0; i < 3 - toCount - ccCount; i++)
                        {
                            bccAddr.Add(bccColl[i]);
                        }
                        bccColl = bccAddr;
                        isAddrDiscard = true;
                    }
                }
                msg.To = toColl;
                msg.Cc = ccColl;
                msg.Bcc = bccColl;
				msg.Subject = Subject;
				msg.Date = Date;
				msg.Charset = Charset;

				switch(Priority)
				{
					case "5":
						msg.Priority = MailPriority.Low;
						break;
					case "3":
						msg.Priority = MailPriority.Normal;
						break;
					case "1":
						msg.Priority = MailPriority.High;
						break;
					default:
						msg.Priority = MailPriority.Normal;
						break;
				}

				for(int i = 0; Request.Form.AllKeys.Length > i; i++)
				{
					if(Request.Form.AllKeys[i].StartsWith("attachments["))
					{
						AttachmentName = Request.Form[Request.Form.AllKeys[i]];

						string[] temp = Regex.Split(Request.Form.AllKeys[i], Regex.Escape("["));
						AttachmentTempName = temp[1];
						temp = Regex.Split(AttachmentTempName, Regex.Escape("]"));
						AttachmentTempName = temp[0];
						AttachmentTempName = Path.Combine(Utils.GetTempFolderName(this.Session), AttachmentTempName);

						msg.Attachments.Add(AttachmentTempName, AttachmentName);
					}
				}

				if(IsHTML)
				{
					msg.BodyHtmlText = Text;
				}
				else
				{
					msg.BodyPlainText = Text;
				}

				WebMailMessage wmsg = new WebMailMessage(acct);
				wmsg.Init(msg, ((wmsg.StrUid != null) && (wmsg.StrUid.Length > 0)), null);

				if(id_msg != -1)
				{
					wmsg.IDMsg = id_msg;
				}

				bwml.IncrementContactsFrequency(wmsg.ToMsg);
				bwml.IncrementContactsFrequency(wmsg.CcMsg);
				bwml.IncrementContactsFrequency(wmsg.BccMsg);
				bwml.SendMessage(wmsg);
                if (isAddrDiscard)
                {
                    Session[Constants.sessionReportText] = "To prevent abuse, no more than 3 e-mail addresses per message is allowed in this demo. All addresses except the first 3 have been discarded.";
                }
                else
                {
                    Session[Constants.sessionReportText] = _resMan.GetString("ReportMessageSent");
                }

				Response.Redirect("basewebmail.aspx");
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void SaveMsg()
		{
			try
			{
				int id_msg = Convert.ToInt32(Request.Form["id_message"]);

				string From = Request.Form["newMessageID" + delimeter + "fromTextBox"];
				string To = Request.Form["newMessageID" + delimeter + "toTextBox"];
				string Cc = Request.Form["newMessageID" + delimeter + "ccTextBox"];
				string Bcc = Request.Form["newMessageID" + delimeter + "bccTextBox"];
				string Subject = Request.Form["newMessageID" + delimeter + "subjectTextBox"];
				string Priority = Request.Form["priority" + delimeter + "input"];
				string AttachmentTempName = null;
				string AttachmentName = null;
				DateTime Date = DateTime.Now;
				string Charset = Utils.GetEncodingByCodePage(acct.UserOfAccount.Settings.DefaultCharsetOut).HeaderName;

				bool IsHTML = false;

				if(Request.Form["ishtml"] == "1")
				{
					IsHTML = true;
				}
				else
				{
					IsHTML = false;
				}

				string Text = Request.Form["message"];

				MailMessage msg = new MailMessage();
			
				msg.From = EmailAddress.Parse(From);
				msg.To = EmailAddressCollection.Parse(To);
				msg.Cc = EmailAddressCollection.Parse(Cc);
				msg.Bcc = EmailAddressCollection.Parse(Bcc);
				msg.Subject = Subject;
				msg.Date = Date;
				msg.Charset = Charset;

				switch(Priority)
				{
					case "Low":
						msg.Priority = MailPriority.Low;
						break;
					case "Normal":
						msg.Priority = MailPriority.Normal;
						break;
					case "High":
						msg.Priority = MailPriority.High;
						break;
					default:
						msg.Priority = MailPriority.Normal;
						break;
				}

				for(int i = 0; Request.Form.AllKeys.Length > i; i++)
				{
					if(Request.Form.AllKeys[i].StartsWith("attachments["))
					{
						AttachmentName = Request.Form[Request.Form.AllKeys[i]];

						string[] temp = Regex.Split(Request.Form.AllKeys[i], Regex.Escape("["));
						AttachmentTempName = temp[1];
						temp = Regex.Split(AttachmentTempName, Regex.Escape("]"));
						AttachmentTempName = temp[0];
						AttachmentTempName = Path.Combine(Utils.GetTempFolderName(this.Session), AttachmentTempName);

						msg.Attachments.Add(AttachmentTempName, AttachmentName);
					}
				}

				if(IsHTML)
				{
					msg.BodyHtmlText = Text;
				}
				else
				{
					msg.BodyPlainText = Text;
				}

				WebMailMessage wmsg = new WebMailMessage(acct);

				if(id_msg != -1)
				{
					wmsg.IDMsg = id_msg;
				}

				wmsg.Init(msg, ((wmsg.StrUid != null) && (wmsg.StrUid.Length > 0)), null);

				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				bwml.SaveMessageToDrafts(wmsg);
				Session[Constants.sessionReportText] = _resMan.GetString("ReportMessageSaved");

				Response.Redirect("basewebmail.aspx?scr=drafts");
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void GetMessage()
		{
			ctrlDefault.Control_Messagecontainer.isPreviewPane = true;
			string[] arrTemp = Regex.Split(HFValue.Value, "----");
			//Session.Add("id",Convert.ToInt32(arrTemp[0]));
			ctrlDefault.Control_Messagecontainer.id_msg = Convert.ToInt32(arrTemp[0]);
			ctrlDefault.Control_Messagecontainer.uid = arrTemp[1];
			ctrlDefault.Control_Messagecontainer.id_folder = Convert.ToInt64(arrTemp[2]);
			ctrlDefault.Control_Messagecontainer.folder_full_name = arrTemp[3];
			ctrlDefault.Control_Messagecontainer.charset = Convert.ToInt32(arrTemp[4]);
			ctrlDefault.Control_Messagecontainer.Data_Bind(acct, 0);
			string[] tempArr = Regex.Split(HFPageInfo.Value, "----");
			ctrlDefault.Control_Maillist.PageNumber = Convert.ToInt32(tempArr[0]);
			ctrlDefault.Control_Maillist.OutputMessagesList(acct, Convert.ToInt64(arrTemp[2]));
			ctrlDefault.Control_FoldersPart.selectedFolderID = Convert.ToInt64(arrTemp[2]);
			ctrlDefault.Control_FoldersPart.OutputFoldersTree();

			string text = @"InboxLines.CheckLine(""" + Utils.EncodeJsSaveString(HFValue.Value) + @""");
			";
			
			js.AddInitText(text);
		}

		protected void GetMessages()
		{
			try
			{
				LoadSession();
				string[] tempArr = Regex.Split(HFValue.Value, "----");
				id_folder = Convert.ToInt64(tempArr[0]);

				SaveSession();

				ctrlDefault.Control_FoldersPart.selectedFolderID = id_folder;
				ctrlDefault.Control_FoldersPart.OutputFoldersTree();
				ctrlDefault.Control_Messagecontainer.ShowHideElements(false);

				Control_Toolbar.LoadButtons();

				if(IsSearch == true)
				{
					//ctrlDefault.Control_Maillist.LookFor = LookFor;
					Session.Remove("is_search");
					Session.Remove("look_for");
				}
				ctrlDefault.Control_Maillist.OutputMessagesList(acct, id_folder);
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		private void PagerButton_Click(object sender, EventArgs e)
		{
			LoadSession();
			string[] arrTemp = Regex.Split(HFPageInfo.Value, "----");
			if(arrTemp[0] == "m")
			{
				ctrlDefault.Control_Messagecontainer.ShowHideElements(false);
				if(IsSearch)
				{
					ctrlDefault.Control_Maillist.LookFor = LookFor;
				}
				else
				{
					ctrlDefault.Control_Maillist.LookFor = "";
				}
				ctrlDefault.Control_Maillist.PageNumber = Convert.ToInt32(arrTemp[1]);
				ctrlDefault.Control_Maillist.OutputMessagesList(acct, id_folder);
			}
			else
			{
				CurrentPage = Convert.ToInt32(arrTemp[1]);
				SaveSession();
				Response.Redirect("basewebmail.aspx?scr=contacts");
			}
		}

		public void OutputException(Exception ex)
		{
			Log.WriteException(ex);

			js.AddText(@"
function CInformation(cont, cls)
{
	this._mainContainer = cont;
	this._containerClass = cls;
}

CInformation.prototype = {
	Show: function ()
	{
		this._mainContainer.className = this._containerClass;
	},
	
	Hide: function ()
	{
		this._mainContainer.className = 'wm_hide';
	},

	Resize: function ()
	{
		var tbl = this._mainContainer;
		tbl.style.width = 'auto';
		var offsetWidth = tbl.offsetWidth;
		var width = GetWidth();
		if (offsetWidth >  0.4 * width) {
			tbl.style.width = '40%';
			offsetWidth = tbl.offsetWidth;
		}
		tbl.style.left = (width - offsetWidth) + 'px';
		tbl.style.top = this.GetScrollY() + 'px';
	},

	GetScrollY: function()
	{
		var scrollY = 0;
		if (document.body && typeof document.body.scrollTop != ""undefined"")
		{
			scrollY += document.body.scrollTop;
			if (scrollY == 0 && document.body.parentNode && typeof document.body.parentNode != ""undefined"")
			{
				scrollY += document.body.parentNode.scrollTop;
			}
		} else if (typeof window.pageXOffset != ""undefined"")  {
			scrollY += window.pageYOffset;
		}
		return scrollY;
	}
}
");

			js.AddInitText(@"
objInfo = document.getElementById(""info"");
objInfoText = document.getElementById(""info_message"");

objInfoText.innerHTML = """ + ex.Message + @""";

Info = new CInformation(objInfo, ""wm_error_information"");
Info.Show();
Info.Resize();	

setTimeout(""Info.Hide();"", 20000);
");

		}

		public void OutputReport()
		{
			if ((Session[Constants.sessionReportText] != null) && (Session[Constants.sessionReportText].ToString().Length > 0))
			{
				js.AddText("var Report;");
				js.AddInitText(@"
Report = new CReport('Report');
Report.Build();
");
				js.AddInitText(string.Format(@"
Report.Show('{0}');
",
					Utils.EncodeJsSaveString((string)Session[Constants.sessionReportText])));
				Session[Constants.sessionReportText] = null;
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
			this.PreRender += new EventHandler(basewebmail_PreRender);
			PostBackButton.Click +=new EventHandler(PostBackButton_Click);
			PagerButton.Click +=new EventHandler(PagerButton_Click);
		}
		#endregion

		private void basewebmail_PreRender(object sender, EventArgs e)
		{
			OutputReport();
		}
	}
}