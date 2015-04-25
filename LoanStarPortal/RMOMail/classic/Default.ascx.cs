namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for _DefaultScr.
	/// </summary>
	public partial class _DefaultScr : System.Web.UI.UserControl
	{
		protected WebMailPro.classic.MessageContainer _Control_Messagecontainer;
		protected WebMailPro.classic.Folders_part _Control_FoldersPart;
		protected WebMailPro.classic.Inbox _Control_Maillist;
		protected WebMailPro.classic.Toolbar _Control_Toolbar;
		protected bool ShowPreviewPane;

		public WebMailPro.classic.MessageContainer Control_Messagecontainer
		{
			get { return _Control_Messagecontainer; }
		}

		public WebMailPro.classic.Folders_part Control_FoldersPart
		{
			get { return _Control_FoldersPart; }
		}

		public WebMailPro.classic.Inbox Control_Maillist
		{
			get { return _Control_Maillist; }
		}

		public WebMailPro.classic.Toolbar Control_Toolbar
		{
			get { return _Control_Toolbar; }
		}

		protected string _skin = Constants.defaultSkinName;
		protected WebmailResourceManager _manager = null;
		protected Account acct = null;

		public string Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}

		protected jsbuilder _js;
		public jsbuilder js
		{
			get { return _js; }
			set { _js = value; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			_manager = (new WebmailResourceManagerCreator()).CreateResourceManager();

			acct = Session[Constants.sessionAccount] as Account;

			if ((acct.UserOfAccount.Settings.ViewMode & ViewMode.WithPreviewPane) > 0)
			{
				ShowPreviewPane = true;
			}
			else
			{
				ShowPreviewPane = false;
			}

			Control_Messagecontainer.js = _js;
			Control_Maillist.js = _js;

			_js.AddFile("classic/class_pageswitcher_classic.js");
			_js.AddFile("_class_selection.js");
			_js.AddFile("class.common.js");
		
			_js.AddFile("classic/class_maillist_resizer.js");
			_js.AddFile("classic/class_maillist.js");

			JS_ResizeElements();
			JS_CheckThisLine();
			JS_Init();
		}

		protected void JS_Init()
		{
			string Left = String.Empty;
			string Top = String.Empty;
			string HideFolders = String.Empty;
			
			if (Request.Cookies["wm_vert_resizer"] != null)
			{
				Left = Request.Cookies["wm_vert_resizer"].Value;
				acct.UserOfAccount.Settings.HorizResizer = Convert.ToInt16(Left);
				Response.Cookies["wm_vert_resizer"].Value = null;
				Response.Cookies["wm_vert_resizer"].Expires = new DateTime(1999, 12, 31);
			}
			else
			{
				Left = acct.UserOfAccount.Settings.HorizResizer.ToString();
			}

			if (Request.Cookies["wm_horiz_resizer"] != null)
			{
				Top = Request.Cookies["wm_horiz_resizer"].Value;
				acct.UserOfAccount.Settings.VertResizer =  Convert.ToInt16(Top);
				Response.Cookies["wm_horiz_resizer"].Value = null;
				Response.Cookies["wm_horiz_resizer"].Expires = new DateTime(1999, 12, 31);
			}
			else
			{
				Top = acct.UserOfAccount.Settings.VertResizer.ToString();
			}

			if (Request.Cookies["wm_hide_folders"] != null)
			{
				HideFolders = Request.Cookies["wm_hide_folders"].Value;
				acct.UserOfAccount.Settings.HideFolders = (HideFolders == "0") ? false : true;
				Response.Cookies["wm_hide_folders"].Value = null;
				Response.Cookies["wm_hide_folders"].Expires = new DateTime(1999, 12, 31);
			}
			else
			{
				HideFolders = acct.UserOfAccount.Settings.HideFolders.ToString();
			}

			acct.Update(true);

			if (acct != Session[Constants.sessionAccount])
			{
				Session[Constants.sessionAccount] = acct;
			}
			
			string temp = (HideFolders == "1") ? "isDisplayFolders = false; FoldersPart.hide(); MovableVerticalDiv.free();" : "";

			string AddInitText = @"
				PageSwitcher = new CPageSwitcher('" + _skin + @"');
				PageSwitcher.Build();

				MainContainer = new CMainContainer();
				FoldersPart = new CFoldersPart(isPreviewPane, '" + _skin + @"');
				MessageBox = new CMessagesBox(isPreviewPane, PageSwitcher);
				
				vResizerPart = new CvResizerPart();
				hResizerPart = new ChResizerPart();

				MovableVerticalDiv = new CVerticalResizer(
					vResizerPart.resizer, MainContainer.table, 2, 80, 550, " + Left + @", 'ResizeElements(""width"")', 0);

				MovableHorizontalDiv = new CHorizontalResizer(
					hResizerPart.resizer, MainContainer.table, 2, MessageBox.min_upper + 100, MessageBox.min_lower, " + Top + @", 'ResizeElements(""height"")'); 

				HFPageInfo = document.getElementById('HFPageInfo');
				var tempArr = HFPageInfo.value.split('----');
				
				PageSwitcher.Show(tempArr[0]*1, tempArr[1]*1, tempArr[2]*1, ""Pager('"", ""');"");
				
				" + temp + @"

				CheckMail = new CCheckMail();
					
					var m_attachments = document.getElementById('MessageAttachments');
					var m_message = document.getElementById('messageBody');
					var m_message_center = document.getElementById('message_center');	
					var resDiv = document.getElementById('vres');

				if (messageWithAttach)
				{
					resizerObj = new CVerticalResizer(resDiv, m_message, 2, 10, 40, 140, ""ResizeElementsMini();"", 1);
					m_attachments.style.width = '140px';
					resDiv.style.left = '140px';
					m_message_center.style.left = '142px';
				}
				else
				{
					m_attachments.className = 'wm_hide';
					resDiv.className = 'wm_hide';
				}
			";

			_js.AddInitText(AddInitText);
		}

		protected void JS_CheckThisLine()
		{
            string imapConfirmStrart = "";
            string imapConfirmEnd = "";

            if (acct.MailIncomingProtocol == IncomingMailProtocol.Pop3)
            {
                BaseWebMailActions bwa = new BaseWebMailActions(acct, this.Page);
                Folder curFolder = bwa.GetFolder(_Control_FoldersPart.selectedFolderID);
                if (curFolder.Type == FolderType.Inbox && curFolder.SyncType == FolderSyncType.DirectMode)
                {
                    imapConfirmStrart = "if(confirm('" + _manager.GetString("ConfirmDirectModeAreYouSure") + "')){";
                    imapConfirmEnd = "};";
                }
            }

			string AddText = @"
                var Browser;
				function Sort(obj)
				{
					var sort_field = null;
					var error = false;

					switch(obj.id)
					{
						case 'attachment':
							sort_field = 10;
							break;
						case 'flag':
							sort_field = 12;
							break;
						case 'from':
							sort_field = 2;
							break;
						case 'date':
							sort_field = 0;
							break;
						case 'size':
							sort_field = 6;
							break;
						case 'subject':
							sort_field = 8;
							break;
						default:
							error = true;
							break;
					}

					if(!error)
					{
		    			var HFAction = document.getElementById('HFAction');
	    				var HFRequest = document.getElementById('HFRequest');
    					var HFValue = document.getElementById('HFValue');
					
						HFAction.value = 'sort';
						HFRequest.value = 'mail';
						HFValue.value = sort_field;

						__doPostBack('PostBackButton','');
					}
				}
				
				function eisAcceptThis()
				{
	    			var HFAction = document.getElementById('HFAction');
    				var HFRequest = document.getElementById('HFRequest');
					HFAction.value = 'eis';
					HFRequest.value = 'acceptone';
					__doPostBack('PostBackButton','');
				}
				function eisAcceptSender()
				{
	    			var HFAction = document.getElementById('HFAction');
    				var HFRequest = document.getElementById('HFRequest');
					HFAction.value = 'eis';
					HFRequest.value = 'acceptall';
					__doPostBack('PostBackButton','');
				}

				function SwitchTo(mode)
				{
	    			var HFAction = document.getElementById('HFAction');
    				var HFRequest = document.getElementById('HFRequest');
					
					HFAction.value = 'switch';
					if(mode == 1)
					{
						HFRequest.value = 'html';
					}
					else
					{
						HFRequest.value = 'plain';
					}
					
					__doPostBack('PostBackButton','');

				}			


				function LoadMessageFull(lineid)
				{
					ClickCount = 0;
	    			var HFAction = document.getElementById('HFAction');
    				var HFRequest = document.getElementById('HFRequest');
   					var HFValue = document.getElementById('HFValue');
					
					HFAction.value = 'get';
					HFRequest.value = 'message_full_screen';
					HFValue.value = lineid;
					
					__doPostBack('PostBackButton','');
				}

				function LoadMessage(lineid)
				{
					if(isPreviewPane)
					{
	        			var HFAction = document.getElementById('HFAction');
        				var HFRequest = document.getElementById('HFRequest');
       					var HFValue = document.getElementById('HFValue');
						
						HFAction.value = 'get';
						HFRequest.value = 'message';
						HFValue.value = lineid;
						
    					setTimeout('doPost()', 250);

					}
				}

                function doPost()
                {
                    if (ClickCount == 1)
                    {
                        ClickCount = 0;
                        __doPostBack('PostBackButton','');
                    }
                }

				var ClickCount = 0;

                function CheckThisLine(e, trobj)
                {
	                var id = trobj.id;
                    e = e ? e : window.event;
	                if (e.ctrlKey) {
                        ClickCount = 0;
		                InboxLines.CheckCtrlLine(id);
	                } else if (e.shiftKey) {
                        ClickCount = 0;
		                InboxLines.CheckShiftLine(id);
	                } else {
		                if (Browser.Mozilla) {var elem = e.target;}
		                else {var elem = e.srcElement;}
                		
		                if (!elem || id == """" || elem.id == ""none"") {
			                return false;
		                }
                		
		                var loverTag = elem.tagName.toLowerCase();
                		
		                if (loverTag == ""a"") {
			                LoadMessageFull(id);
		                } else if (loverTag == ""input"") {
			                InboxLines.CheckCBox(id);
		                } else if (loverTag == ""img"") {
							var line = InboxLines.GetLinesById(id);
							if (line.Flagged) {
                                DoUnFlagOneMessage(line.Id);
							} else {
								DoFlagOneMessage(line.Id);
							}
		                } else if (isPreviewPane) {
                            ClickCount++;
			                InboxLines.CheckLine(id);
			                LoadMessage(id);
		                }
	                }
                }	

                function CheckThisLineDb(e, trobj)
                {
	                var id = trobj.id;
	                e = e ? e : window.event;
	                if (Browser.Mozilla) {var elem = e.target;}
	                else {var elem = e.srcElement;}
                	if (!elem || elem.id == ""none"" || elem.tagName.toLowerCase() == ""input"") {
		                return false;
	                }
	                LoadMessageFull(id);
                }

				function DoEmptyTrash()
				{
					if(confirm(""" + _manager.GetString("ConfirmAreYouSure") + @"""))
					{
	        			var HFAction = document.getElementById('HFAction');
        				var HFRequest = document.getElementById('HFRequest');
						
						HFAction.value = 'empty';
						HFRequest.value = 'trash';
					
						__doPostBack('PostBackButton','');
					}
				}

				function DoFlagMessages()
				{
					var idsArray = InboxLines.GetCheckedLines();
					
					if(idsArray.IdArray.length > 0)
					{
	        			var HFAction = document.getElementById('HFAction');
        				var HFRequest = document.getElementById('HFRequest');
       					var HFValue = document.getElementById('HFValue');
						
						HFAction.value = 'operation_messages';
						HFRequest.value = 'flag';

						for(i = 0; idsArray.IdArray.length > i; i++)
						{
							if(idsArray.IdArray.length != i+1)
							{
								HFValue.value += idsArray.IdArray[i] + '@#%';
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
						alert('" + _manager.GetString("WarningMarkListItem") + @"');
					}
				}

				function DoUnFlagMessages()
				{
					var idsArray = InboxLines.GetCheckedLines();

					if(idsArray.IdArray.length > 0)
					{
	        			var HFAction = document.getElementById('HFAction');
        				var HFRequest = document.getElementById('HFRequest');
       					var HFValue = document.getElementById('HFValue');
						
						HFAction.value = 'operation_messages';
						HFRequest.value = 'unflag';

						for(i = 0; idsArray.IdArray.length > i; i++)
						{
							if(idsArray.IdArray.length != i+1)
							{
								HFValue.value += idsArray.IdArray[i] + '@#%';
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
						alert('" + _manager.GetString("WarningMarkListItem") + @"');
					}
				}

				function DoFlagOneMessage(line)
				{
	    			var HFAction = document.getElementById('HFAction');
    				var HFRequest = document.getElementById('HFRequest');
   					var HFValue = document.getElementById('HFValue');
					
					HFAction.value = 'operation_messages';
					HFRequest.value = 'flag';
					HFValue.value = line;
				
					__doPostBack('PostBackButton','');
				}

				function DoUnFlagOneMessage(line)
				{
	    			var HFAction = document.getElementById('HFAction');
    				var HFRequest = document.getElementById('HFRequest');
   					var HFValue = document.getElementById('HFValue');
					
					HFAction.value = 'operation_messages';
					HFRequest.value = 'unflag';
					HFValue.value = line;
				
					__doPostBack('PostBackButton','');
				}		
					
				function DoReadMessages()
				{
					var idsArray = InboxLines.GetCheckedLines();
					
					if(idsArray.IdArray.length > 0)
					{
	        			var HFAction = document.getElementById('HFAction');
        				var HFRequest = document.getElementById('HFRequest');
       					var HFValue = document.getElementById('HFValue');
						
						HFAction.value = 'operation_messages';
						HFRequest.value = 'mark_read';

						for(i = 0; idsArray.IdArray.length > i; i++)
						{
							if(idsArray.IdArray.length != i+1)
							{
								HFValue.value += idsArray.IdArray[i] + '@#%';
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
						alert('" + _manager.GetString("WarningMarkListItem") + @"');
					}
				}

				function DoUnReadMessages()
				{
					var idsArray = InboxLines.GetCheckedLines();
					
					if(idsArray.IdArray.length > 0)
					{
	        			var HFAction = document.getElementById('HFAction');
        				var HFRequest = document.getElementById('HFRequest');
       					var HFValue = document.getElementById('HFValue');
						
						HFAction.value = 'operation_messages';
						HFRequest.value = 'mark_unread';

						for(i = 0; idsArray.IdArray.length > i; i++)
						{
							if(idsArray.IdArray.length != i+1)
							{
								HFValue.value += idsArray.IdArray[i] + '@#%';
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
						alert('" + _manager.GetString("WarningMarkListItem") + @"');
					}
				}

				function SubmitSearch(e)
				{
					if (e.keyCode == 13)
					{
						DoSearch();
					}
				}

				function SubmitSearchAdvanced(e)
				{
					if (e.keyCode == 13)
					{
						DoSearchAdvanced();
					}
				}

				function DoSearch()
				{					
	    			var HFAction = document.getElementById('HFAction');
    				var HFRequest = document.getElementById('HFRequest');
   					var HFValue = document.getElementById('HFValue');
					objInput = document.getElementById('Control_Toolbar_smallLookFor');
					
					HFAction.value = 'search';
					HFRequest.value = 'normal';
					HFValue.value = objInput.value;

					__doPostBack('PostBackButton','');
				}

				function DoSearchAdvanced()
				{					
	    			var HFAction = document.getElementById('HFAction');
    				var HFRequest = document.getElementById('HFRequest');
   					var HFValue = document.getElementById('HFValue');
					objLookFor = document.getElementById('bigLookFor');
					objQfolder = document.getElementById('Control_Toolbar_DropDownListFolders');
					objQmmode1 = document.getElementById('qmmode1');					

					HFAction.value = 'search';
					HFRequest.value = 'advanced';
    				HFValue.value = objLookFor.value + '@#%' + objQfolder.value + '@#%';
					if(objQmmode1.checked)
					{
						HFValue.value = HFValue.value + 'true';
					}
					else
					{
						HFValue.value = HFValue.value + 'false';
					}

					__doPostBack('PostBackButton','');
				}

				function DoDeleteMessages()
				{
					var idsArray = InboxLines.GetCheckedLines();
					
					if(idsArray.IdArray.length > 0)
					{" + imapConfirmStrart
	        		+@" var HFAction = document.getElementById('HFAction');
        				var HFRequest = document.getElementById('HFRequest');
       					var HFValue = document.getElementById('HFValue');
						
						HFAction.value = 'operation_messages';
						HFRequest.value = 'delete';
						HFValue.value = '';

						for(i = 0; idsArray.IdArray.length > i; i++)
						{
							if(idsArray.IdArray.length != i+1)
							{
								HFValue.value += idsArray.IdArray[i] + '@#%';
							}
							else
							{
								HFValue.value += idsArray.IdArray[i];
							}
						}
						
						__doPostBack('PostBackButton','');"+imapConfirmEnd+@"
					}
					else
					{
						alert('" + _manager.GetString("WarningMarkListItem") + @"');
					}
				}

				function DoUnDeleteMessages()
				{
					var idsArray = InboxLines.GetCheckedLines();
					
					if(idsArray.IdArray.length > 0)
					{
	        			var HFAction = document.getElementById('HFAction');
        				var HFRequest = document.getElementById('HFRequest');
       					var HFValue = document.getElementById('HFValue');
						
						HFAction.value = 'operation_messages';
						HFRequest.value = 'undelete';
						HFValue.value = '';

						for(i = 0; idsArray.IdArray.length > i; i++)
						{
							if(idsArray.IdArray.length != i+1)
							{
								HFValue.value += idsArray.IdArray[i] + '@#%';
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
						alert('" + _manager.GetString("WarningMarkListItem") + @"');
					}
				}

				function DoUnDeleteMessages()
				{
					var idsArray = InboxLines.GetCheckedLines();

					if(idsArray.IdArray.length > 0)
					{					
	        			var HFAction = document.getElementById('HFAction');
        				var HFRequest = document.getElementById('HFRequest');
       					var HFValue = document.getElementById('HFValue');
						
						HFAction.value = 'operation_messages';
						HFRequest.value = 'undelete';
						HFValue.value = '';

						for(i = 0; idsArray.IdArray.length > i; i++)
						{
							if(idsArray.IdArray.length != i+1)
							{
								HFValue.value += idsArray.IdArray[i] + '@#%';
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
						alert('" + _manager.GetString("WarningMarkListItem") + @"');
					}
				}

				function DoReloadFolders()
				{
					HFAction = document.getElementById('HFAction');
					
					HFAction.value = 'reload_folders';

					__doPostBack('PostBackButton','');
				}

				function DoPurgeDeletedMessages()
				{
	    			var HFAction = document.getElementById('HFAction');
    				var HFRequest = document.getElementById('HFRequest');
   					var HFValue = document.getElementById('HFValue');
					
					HFAction.value = 'operation_messages';
					HFRequest.value = 'purge';
					HFValue.value = '';

					__doPostBack('PostBackButton','');
				}

				function MoveToFolder(FolderID)
				{
					var idsArray = InboxLines.GetCheckedLines();
					
					if(idsArray.IdArray.length > 0)
					{
	        			var HFAction = document.getElementById('HFAction');
        				var HFRequest = document.getElementById('HFRequest');
       					var HFValue = document.getElementById('HFValue');
						
						HFAction.value = 'operation_messages';
						HFRequest.value = 'move_to_folder';
						HFValue.value = '';

						for(i = 0; idsArray.IdArray.length > i; i++)
						{
							if(idsArray.IdArray.length != i+1)
							{
								HFValue.value += idsArray.IdArray[i] + '@#%';
							}
							else
							{
								HFValue.value += idsArray.IdArray[i] + '@-@' + FolderID;
							}
						}
						
						__doPostBack('PostBackButton','');
					}
					else
					{
						alert('" + _manager.GetString("WarningMarkListItem") + @"');
					}
				}

				function DoReadAllMessages()
				{
	    			var HFAction = document.getElementById('HFAction');
    				var HFRequest = document.getElementById('HFRequest');
					
					HFAction.value = 'operation_messages';
					HFRequest.value = 'mark_all_read';
					
					__doPostBack('PostBackButton','');

				}

				function DoUnReadAllMessages()
				{
	    			var HFAction = document.getElementById('HFAction');
    				var HFRequest = document.getElementById('HFRequest');
					
					HFAction.value = 'operation_messages';
					HFRequest.value = 'mark_all_unread';
					
					__doPostBack('PostBackButton','');

				}

				function ChangeCharset()
				{
   					var HFValue = document.getElementById('HFValue');
					DDLCharsets = document.getElementById('DefaultID__Control_Messagecontainer_DropDownListCharsets');
					
					var tempArr = HFValue.value.split('----');
   					HFValue.value = tempArr[0] + '----' + tempArr[1] + '----' + tempArr[2] + '----' + tempArr[3] + '----' + DDLCharsets.value;
					
					__doPostBack('PostBackButton','');
				}
				
				function ChangeFolder(id)
				{
	    			var HFAction = document.getElementById('HFAction');
    				var HFRequest = document.getElementById('HFRequest');
   					var HFValue = document.getElementById('HFValue');
					
					HFAction.value = 'get';
					HFRequest.value = 'messages';
					HFValue.value = id;
					
					__doPostBack('PostBackButton','');
				}
				
				function Pager(id)
				{
					HFPageInfo = document.getElementById('HFPageInfo');
					
					HFPageInfo.value = 'm----' + id;
					
					__doPostBack('PagerButton','');
				}

				function DoReplyButton()
				{
					var idsArray = InboxLines.GetCheckedLines();
					
					if(idsArray.IdArray.length > 0)
					{
	        			var HFAction = document.getElementById('HFAction');
        				var HFRequest = document.getElementById('HFRequest');
       					var HFValue = document.getElementById('HFValue');
						
						HFAction.value = 'reply';
						HFValue.value = idsArray.IdArray[0];

						__doPostBack('PostBackButton','');
					}
					else
					{
						alert('" + _manager.GetString("WarningMarkListItem") + @"');
					}
				}

				function DoReplyAllButton()
				{
					var idsArray = InboxLines.GetCheckedLines();
					
					if(idsArray.IdArray.length > 0)
					{
	        			var HFAction = document.getElementById('HFAction');
        				var HFRequest = document.getElementById('HFRequest');
       					var HFValue = document.getElementById('HFValue');
						
						HFAction.value = 'replyall';
						HFRequest.value = '';
						HFValue.value = idsArray.IdArray[0];

						__doPostBack('PostBackButton','');
					}
					else
					{
						alert('" + _manager.GetString("WarningMarkListItem") + @"');
					}
				}

				function DoForwardButton()
				{
					var idsArray = InboxLines.GetCheckedLines();
					
					if(idsArray.IdArray.length > 0)
					{
	    	    		var HFAction = document.getElementById('HFAction');
    	    			var HFRequest = document.getElementById('HFRequest');
   	    				var HFValue = document.getElementById('HFValue');
			
						HFAction.value = 'forward';
						HFRequest.value = '';
						HFValue.value = idsArray.IdArray[0];

						__doPostBack('PostBackButton','');
					}
					else
					{
						alert('" + _manager.GetString("WarningMarkListItem") + @"');
					}
				}
				";

			_js.AddText(AddText);
		}

		protected void JS_ResizeElements()
		{
			string AddText = @"
				var MovableVerticalDiv, MovableHorizontalDiv;
				var MainContainer, FoldersPart, MessageBox;
				var PageSwitcher;
				var vResizerPart, hResizerPart;
				var isDisplayFolders = true;
				var isPreviewPane = " + ShowPreviewPane.ToString().ToLower() + @";
				var CheckMailUrl = 'check-mail.aspx';
				var EmptyHtmlUrl = 'empty.html';
				var WebMail = { _html: document.getElementById('html') };
				

				function SetStateTextHandler(text) {
					if (CheckMail) CheckMail.SetText(text);
				}

				function SetCheckingFolderHandler(folder, count) {
					if (CheckMail) CheckMail.SetFolder(folder, count);
				}

				function SetRetrievingMessageHandler(number) {
					if (CheckMail) CheckMail.SetMsgNumber(number);
				}

				function SetDeletingMessageHandler(number) {
					if (CheckMail) CheckMail.DeleteMsg(number);
				}

				function EndCheckMailHandler(error)
				{
					if (CheckMail) CheckMail.End();
					if (error.length > 0) {
						if (error == 'session_error') {
							document.location = 'default.aspx?error=1';
						}
					}
				}

				function CheckEndCheckMailHandler() {
					if (CheckMail && CheckMail.started) {
						CheckMail.End();
						InfoPanel._isError = true;
						InfoPanel.SetInfo(Lang.ErrorCheckMail);
						InfoPanel.Show();
					}
					else
					{
						document.location = ""basewebmail.aspx"";
					}
				}

				function ResizeElementsMini()
				{
					if (messageWithAttach)
					{
						MessageBox.resizeAttachmentWidth(resizerObj._leftShear);
					}
				}

				function ResizeElements(mode)
				{
					var height = GetHeight();
					var width = GetWidth();
                    var subjectObj = document.getElementById(""subject"");
					
					var isAuto = false;
					MainContainer.inner_height = height - 2 - MainContainer.getExternalHeight();
					
					if (MainContainer.inner_height < 300)
					{
						MainContainer.inner_height = 300;
						isAuto = true;
					}
					
					if (mode == 'all') 
					{
						//height
						FoldersPart.resizeElementsHeight(MainContainer.inner_height);
						MessageBox.resizeElementsHeight(MainContainer.inner_height, mode);
						
						//width
						var messageList_width, folderList_width;
						if (isDisplayFolders == true) {FoldersPart.width = MovableVerticalDiv._leftPosition;}
						messageList_width = width - FoldersPart.width - vResizerPart.width - 4;
						if (messageList_width < 600)
						{
							messageList_width = 600;
							folderList_width = width - messageList_width - vResizerPart.width - 4;
							if (folderList_width < 80)
							{

								folderList_width = 80;
								isAuto = true;
								/*document.body.scroll = 'yes';
								document.body.style.overflow = 'auto';*/
							}
						} 
						else
						{
							folderList_width = width - messageList_width - vResizerPart.width - 4;
						}		
							
						FoldersPart.resizeElementsWidth(folderList_width);
						MessageBox.resizeElementsWidth(messageList_width, mode);
						hResizerPart.resizeElements(messageList_width);
						vResizerPart.resizeElements(MainContainer.inner_height);
                        if (subjectObj) {
                            subjectObj.width = (messageList_width - 400) + ""px"";
					    }
                    }

					if (mode == 'height')
					{
						MessageBox.resizeElementsHeight(MainContainer.inner_height, mode);
					}
					
					if (mode == 'width')
					{
						var messageList_width, folderList_width;
						if (isDisplayFolders == true) {FoldersPart.width = MovableVerticalDiv._leftPosition;}
						messageList_width = width - FoldersPart.width - vResizerPart.width - 4;
						if (messageList_width < 600)
						{
							messageList_width = 600;
							folderList_width = width - messageList_width - vResizerPart.width - 4;
							if (folderList_width < 80)
							{
								folderList_width = 80;
								isAuto = true;
							}
						} 
						else
						{
							folderList_width = width - messageList_width - vResizerPart.width - 4;
						}		
							
						FoldersPart.resizeElementsWidth(folderList_width);
						MessageBox.resizeElementsWidth(messageList_width, mode);
						hResizerPart.resizeElements(messageList_width);
                        if (subjectObj) {
                            subjectObj.width = (messageList_width - 400) + ""px"";
					    }
					}
					
					var i_bounds = GetBounds(MessageBox.list_table);
					var ps_bounds = GetBounds(MessageBox.page_switcher);
					
					MessageBox.page_switcher.style.top = (i_bounds.Top + 3) + 'px';
					MessageBox.page_switcher.style.left = (i_bounds.Left + i_bounds.Width - ps_bounds.Width - 18) + 'px';
					SetBodyAutoOverflow(isAuto);
				}

				function ChangeFoldersMode()
				{
					if (isDisplayFolders == true){
						isDisplayFolders = false;
						FoldersPart.hide();
						MovableVerticalDiv.free();
					} else {
						isDisplayFolders = true;
						FoldersPart.show();
						MovableVerticalDiv.busy(FoldersPart.width);
					}
					ResizeElements('width');
				} 

			";
			js.AddText(AddText);
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
