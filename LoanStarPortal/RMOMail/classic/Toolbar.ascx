<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Toolbar.ascx.cs" Inherits="WebMailPro.classic.Toolbar" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table class="wm_toolbar" id="toolbar">
	<tr>
		<td>
			<asp:PlaceHolder ID="Search" Runat="server">
				<div class="wm_toolbar_search_item" style="margin-left: 0px;" id="search_control">
					<img class="wm_search_arrow" id="search_control_img" src="skins/<%=Skin%>/menu/arrow_down.gif" />
				</div>
				<div class="wm_toolbar_search_item" onmouseover="this.className='wm_toolbar_search_item_over'" onmouseout="this.className='wm_toolbar_search_item'" id="search_small_form" style="margin-right: 0px;">
					<input type="text" id="smallLookFor" name="smallLookFor" class="wm_search_input" value="" onkeyup="SubmitSearch(event);" runat="server"/>
					<img class="wm_menu_small_search_img" src="skins/<%=Skin%>/menu/search_button.gif" onclick="javascript:DoSearch()"/>
				</div>	

				<div class="wm_hide" id="search_form">
					<table>
						<tr>
							<td class="wm_search_title">
								<%=_manager.GetString("LookFor")%>
							</td>

							<td class="wm_search_value">
								<input type="text" id="bigLookFor" name="bigLookFor" class="wm_search_input" value="" onkeyup="SubmitSearchAdvanced(event);"/>
								<img class="wm_menu_big_search_img" src="skins/<%=Skin%>/menu/search_button_big.gif" onclick="javascript:DoSearchAdvanced()"/>
							</td>
						</tr>
						<tr>
							<td class="wm_search_title">
								<%=_manager.GetString("SearchIn")%>
							</td>

							<td class="wm_search_value">
								<asp:DropDownList id="DropDownListFolders" runat="server"></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td class="wm_search_value" colspan="2">
								<input type="radio" name="qmmode" id="qmmode1" value="onlyheaders" checked="checked" class="wm_checkbox" />
								<label for="qmmode1"><%=_manager.GetString("QuickSearch")%></label>
							</td>
						</tr>
  						<tr>
  							<td class="wm_search_value" colspan="2">
  								<input type="radio" name="qmmode" id="qmmode2" value="allmessage" class="wm_checkbox" />
  								<label for="qmmode2"><%=_manager.GetString("SlowSearch")%></label>
  							</td>
  						</tr>
					</table>
				</div>
			</asp:PlaceHolder>
			
			<!-- Screen Default !-->
			<asp:PlaceHolder ID="BackToList" Runat="server">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="document.location='basewebmail.aspx?scr=default'">
					<img title="<%=_manager.GetString("BackToList")%>" src="skins/<%=Skin%>/menu/back_to_list.gif" class="wm_menu_new_message_img"/>
					<span class=""><%=_manager.GetString("BackToList")%></span>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="BackToFollowUp" Runat="server">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="document.location='basewebmail.aspx?scr=followup'">
					<img title="Back to follow up" src="skins/<%=Skin%>/menu/back_to_list.gif" class="wm_menu_new_message_img"/>
					<span class="">Back to follow up</span>
				</div>
			</asp:PlaceHolder>			
			<asp:PlaceHolder ID="NewMessage" Runat="server">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="document.location='basewebmail.aspx?scr=new_message'">
					<img title="<%=_manager.GetString("NewMessage")%>" src="skins/<%=Skin%>/menu/new_message.gif" class="wm_menu_new_message_img"/>
					<span class=""><%=_manager.GetString("NewMessage")%></span>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="CheckMail" Runat="server">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="javascript:CheckMail.Start();">
					<img title="<%=_manager.GetString("CheckMail")%>" src="skins/<%=Skin%>/menu/check_mail.gif" class="wm_menu_check_mail_img"/>
					<span class=""><%=_manager.GetString("CheckMail")%></span>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="ReloadFoldersTree" Runat="server">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="javascript:DoReloadFolders();">
					<img title="<%=_manager.GetString("ReloadFolders")%>" src="skins/<%=Skin%>/menu/reload_folders.gif" class="wm_menu_reload_folders_img"/>
					<span class=""><%=_manager.GetString("ReloadFolders")%></span>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="Reply" Runat="server">
				<div id="popup_replace_4" class="wm_tb">
					<div class="wm_toolbar_item" id="popup_title_4" onclick="DoReplyButton();">
						<img title="<%=_manager.GetString("Reply")%>" src="skins/<%=Skin%>/menu/reply.gif" class="wm_menu_reply_img"/>
						<span class=""><%=_manager.GetString("Reply")%></span>
					</div>
					<div id="popup_control_4" class="wm_toolbar_item">
						<img src="skins/<%=Skin%>/menu/popup_menu_arrow.gif" class="wm_menu_control_img"/>
					</div>
				</div>
				<div id="popup_menu_4" class="wm_hide">
					<div class="wm_menu_item" onclick="DoReplyAllButton();" onmouseover="this.className='wm_menu_item_over';" onmouseout="this.className='wm_menu_item';">
						<img class="wm_menu_reply_img" src="skins/<%=Skin%>/menu/replyall.gif" title="<%=_manager.GetString("ReplyAll")%>" />
						<span class=""><%=_manager.GetString("ReplyAll")%></span>
					</div>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="Forward" Runat="server">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="DoForwardButton();" >
					<img title="<%=_manager.GetString("Forward")%>" src="skins/<%=Skin%>/menu/forward.gif" class="wm_menu_reply_img"/>
					<span class=""><%=_manager.GetString("Forward")%></span>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="MarkAsRead" Runat="server">
				<div id="popup_replace_3" class="wm_tb">
					<div id="popup_title_3" class="wm_toolbar_item" onclick="DoReadMessages();">
						<img title="<%=_manager.GetString("MarkAsRead")%>" src="skins/<%=Skin%>/menu/mark_as_read.gif" class="wm_menu_mark_img"/>
						<span class=""><%=_manager.GetString("MarkAsRead")%></span>
					</div>
					<div class="wm_toolbar_item" id="popup_control_3">
						<img src="skins/<%=Skin%>/menu/popup_menu_arrow.gif" class="wm_menu_control_img" />
					</div>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="MoveToFolder" Runat="server">
				<div id="popup_replace_2" class="wm_tb">
					<div id="popup_control_2" class="wm_toolbar_item">
						<img title="<%=_manager.GetString("MoveToFolder")%>" src="skins/<%=Skin%>/menu/move_to_folder.gif" class="wm_menu_move_to_folder_img"/>
						<span class=""><%=_manager.GetString("MoveToFolder")%></span>
						<img src="skins/<%=Skin%>/menu/popup_menu_arrow.gif" class="wm_menu_move_control_img"/>
					</div>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="EmptyTrash" Runat="server">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="javascript:DoEmptyTrash()">
					<img title="<%=_manager.GetString("EmptyTrash")%>" src="skins/<%=Skin%>/menu/empty_trash.gif" class="wm_menu_empty_trash_img"/>
					<span class=""><%=_manager.GetString("EmptyTrash")%></span>
				</div>
			</asp:PlaceHolder>
			<!-- End !-->
			
			<!-- Screen New Message !-->
			<asp:PlaceHolder ID="Send" Runat="server">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="javascript:DoSendButton();">
					<img title="<%=_manager.GetString("SendMessage")%>" src="skins/<%=Skin%>/menu/send.gif" class="wm_menu_send_message_img"/>
					<span class=""><%=_manager.GetString("SendMessage")%></span>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="Print" Runat="server">
			<a href="msgprint.aspx" target="_blank" style="color: #000000; text-decoration: none">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'">
					<img title="<%=_manager.GetString("Print")%>" src="skins/<%=Skin%>/menu/print.gif" class="wm_menu_print_message_img"/>
					<span class=""><%=_manager.GetString("Print")%></span>
				</div>
			</a>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="Save" Runat="server">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="javascript:DoSaveButton();">
					<img title="<%=_manager.GetString("SaveMessage")%>" src="skins/<%=Skin%>/menu/save.gif" class="wm_menu_save_message_img"/>
					<span class=""><%=_manager.GetString("SaveMessage")%></span>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="DeletePop" Runat="server">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="javascript:DoDeleteMessages();">
					<img title="<%=_manager.GetString("Delete")%>" src="skins/<%=Skin%>/menu/delete.gif" class="wm_menu_delete_img"/>
					<span class=""><%=_manager.GetString("Delete")%></span>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="DeleteImap" Runat="server">
				<div class="wm_tb" id="popup_replace_13">
					<div onclick="javascript:DoDeleteMessages();" id="popup_title_13" class="wm_toolbar_item">
						<img title="<%=_manager.GetString("Delete")%><" src="skins/<%=Skin%>/menu/delete.gif" class="wm_menu_delete_img"/> 
						<span class=""><%=_manager.GetString("Delete")%></span>
					</div>
					<div id="popup_control_13" class="wm_toolbar_item">
						<img src="skins/<%=Skin%>/menu/popup_menu_arrow.gif" class="wm_menu_control_img"/>
					</div>
				</div>
				<div id="popup_menu_13" class="wm_hide">
					<div onmouseout="this.className='wm_menu_item';" onmouseover="this.className='wm_menu_item_over';" class="wm_menu_item" onclick="DoUnDeleteMessages();">
						<img title="<%=_manager.GetString("Undelete")%>" src="skins/<%=Skin%>/menu/delete.gif" class="wm_menu_delete_img"/>
						<span class=""><%=_manager.GetString("Undelete")%></span>
					</div>
					<div onmouseout="this.className='wm_menu_item';" onmouseover="this.className='wm_menu_item_over';" class="wm_menu_item" onclick="confirm('Are you sure?') ? DoPurgeDeletedMessages() : '';">
						<img title="<%=_manager.GetString("PurgeDeleted")%>" src="skins/<%=Skin%>/menu/purge.gif" class="wm_menu_delete_img"/>
						<span class=""><%=_manager.GetString("PurgeDeleted")%></span>
					</div>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="Importance" Runat="server">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over';" onmouseout="this.className='wm_toolbar_item';" onclick="javascript:ChangePriority();">
					<img title="<%=_manager.GetString("Importance")%>" src="skins/<%=Skin%>/menu/priority_normal.gif" id="priority_img" class="wm_menu_priority_img" />
					<span id="priority_text" class=""><%=_manager.GetString("Normal")%></span>
				</div>
			</asp:PlaceHolder>
			<!-- End !-->
			
			<!-- Screen Contacts !-->
			<!-- PlaceHolder NewMessage !-->
			<asp:PlaceHolder ID="NewMessageContacts" Runat="server">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="DoNewMessageButton()">
					<img title="<%=_manager.GetString("NewMessage")%>" src="skins/<%=Skin%>/menu/new_message.gif" class="wm_menu_new_message_img"/>
					<span class=""><%=_manager.GetString("NewMessage")%></span>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="NewContact" Runat="server">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="javascript:NewContact();">
					<img title="<%=_manager.GetString("NewContact")%>" src="skins/<%=Skin%>/menu/new_contact.gif" class="wm_menu_new_group_img"/>
					<span class=""><%=_manager.GetString("NewContact")%></span>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="NewGroup" Runat="server">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="javascript:NewGroup()">
					<img title="<%=_manager.GetString("NewGroup")%>" src="skins/<%=Skin%>/menu/new_group.gif" class="wm_menu_new_group_img"/>
					<span class=""><%=_manager.GetString("NewGroup")%></span>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="AddContactsTo" Runat="server">
				<div id="popup_control_7" class="wm_toolbar_item">
					<img title="<%=_manager.GetString("AddContactsTo")%>" class="wm_menu_move_to_folder_img" src="skins/<%=Skin%>/menu/move_to_folder.gif"/>
					<span class=""><%=_manager.GetString("AddContactsTo")%></span>
					<img title="" class="wm_menu_move_control_img" src="skins/<%=Skin%>/menu/popup_menu_arrow.gif"/>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="PlaceholderContacts" Runat="server">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="javascript:DoDelete();">
					<img title="<%=_manager.GetString("Delete")%>" src="skins/<%=Skin%>/menu/delete.gif" class="wm_menu_delete_img"/>
					<span class=""><%=_manager.GetString("Delete")%></span>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="ImportContacts" Runat="server">
				<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="javascript:ImportContacts();">
					<img title="<%=_manager.GetString("ImportContacts")%>" src="skins/<%=Skin%>/menu/import_contacts.gif" class="wm_menu_import_contacts_img"/>
					<span class=""><%=_manager.GetString("ImportContacts")%></span>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="SearchContacts" Runat="server">
				<div id="search_control" style="margin-left: 0px;" class="wm_toolbar_search_item">
					<img src="skins/<%=Skin%>/menu/arrow_down.gif" id="search_control_img" class="wm_search_arrow"/>
				</div>
				<div style="margin-right: 0px;" id="search_small_form" onmouseout="this.className='wm_toolbar_search_item'" onmouseover="this.className='wm_toolbar_search_item_over'" class="wm_toolbar_search_item">
					<input type="text" value="" class="wm_search_input" name="smallLookFor" id="smallLookFor" onkeyup="SubmitSearchContacts(event);"/> 
					<img onclick="javascript:DoSearchContacts();" src="skins/<%=Skin%>/menu/search_button.gif" class="wm_menu_small_search_img"/>
				</div>
				<div id="search_form" class="wm_hide" style="top: 91px; right: 23px;">
					<table>
						<tr>
							<td class="wm_search_title">
								<%=_manager.GetString("LookFor")%>
							</td>
							<td class="wm_search_value">
								<input type="text" value="" class="wm_search_input" name="bigLookFor" id="bigLookFor" onkeyup="SubmitSearchContactsAdvanced(event);"/>
								<img onclick="javascript:DoSearchContactsAdvanced();" src="skins/<%=Skin%>/menu/search_button_big.gif" class="wm_menu_big_search_img"/>
							</td>
						</tr>
						<tr>
							<td class="wm_search_title">
								<%=_manager.GetString("SearchIn")%>
							</td>
							<td class="wm_search_value">
								<asp:DropDownList id="DropDownListGroups" runat="server"></asp:DropDownList>
							</td>
						</tr>
					</table>
				</div>
			</asp:PlaceHolder>
			<!-- End !-->
			
			<!-- Screen View Message !-->
			<!-- PlaceHolder NewMessage !-->
			<!-- PlaceHolder Reply !-->
			<!-- PlaceHolder Forward !-->
			<!-- PlaceHolder Save !-->
			<!-- PlaceHolder DeletePop !-->
			<asp:PlaceHolder ID="PreviousMsg" Runat="server">
				<div class="wm_toolbar_item">
					<img title="<%=_manager.GetString("PreviousMsg")%>" class="wm_menu_next_prev_img" src="skins/<%=Skin%>/menu/message_up.gif"/>
					<span class=""></span>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="NextMsg" Runat="server">
				<div class="wm_toolbar_item">
					<img title="<%=_manager.GetString("NextMsg")%>" class="wm_menu_next_prev_img" src="skins/<%=Skin%>/menu/message_down.gif"/>
					<span class=""></span>
				</div>
			</asp:PlaceHolder>
			<!-- End !-->
		</td>
	</tr>
</table>
<div id="popup_menu_3" class="wm_hide">
	<div onmouseout="this.className='wm_menu_item';" onmouseover="this.className='wm_menu_item_over';" class="wm_menu_item" onclick="DoUnReadMessages();">
		<img title="<%=_manager.GetString("MarkAsUnread")%>" src="skins/<%=Skin%>/menu/mark_as_unread.gif" class="wm_menu_mark_img"/>
		<span class=""><%=_manager.GetString("MarkAsUnread")%></span>
	</div>
	<div onmouseout="this.className='wm_menu_item';" onmouseover="this.className='wm_menu_item_over';" class="wm_menu_item" onclick="DoFlagMessages();">
		<img title="<%=_manager.GetString("MarkFlag")%>" src="skins/<%=Skin%>/menu/flag.gif" class="wm_menu_mark_img"/>
		<span class=""><%=_manager.GetString("MarkFlag")%></span>
	</div>
	<div onmouseout="this.className='wm_menu_item';" onmouseover="this.className='wm_menu_item_over';" class="wm_menu_item" onclick="DoUnFlagMessages();">
		<img title="<%=_manager.GetString("MarkUnflag")%>" src="skins/<%=Skin%>/menu/unflag.gif" class="wm_menu_mark_img" />
		<span class=""><%=_manager.GetString("MarkUnflag")%></span>
	</div>
	<div class="wm_menu_separate"></div>
	<div onmouseout="this.className='wm_menu_item';" onmouseover="this.className='wm_menu_item_over';" class="wm_menu_item" onclick="DoReadAllMessages();">
		<img title="<%=_manager.GetString("MarkAllRead")%>" src="skins/<%=Skin%>/menu/mark_all_read.gif" class="wm_menu_mark_all_img" />
		<span class=""><%=_manager.GetString("MarkAllRead")%></span>
	</div>
	<div onmouseout="this.className='wm_menu_item';" onmouseover="this.className='wm_menu_item_over';" class="wm_menu_item" onclick="DoUnReadAllMessages();">
		<img title="<%=_manager.GetString("MarkAllUnread")%>" src="skins/<%=Skin%>/menu/mark_all_unread.gif" class="wm_menu_mark_all_img"/>
		<span class=""><%=_manager.GetString("MarkAllUnread")%></span>
	</div>
</div>
<div id="popup_menu_2" class="wm_hide">
	<%=FolderList%>
</div>
<div id="popup_menu_7" class="wm_hide">
	<%=GroupList%>
</div>