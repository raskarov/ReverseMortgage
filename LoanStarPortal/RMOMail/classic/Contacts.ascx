<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Contacts.ascx.cs" Inherits="WebMailPro.classic.Contacts" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div class="wm_contacts" id="main_contacts">
	<div id="contacts" class="wm_contacts_list">
		<div id="contact_list_div" class="wm_contact_list_div">
			<div id="contact_list_headers" class="wm_inbox_headers">
				<div style="LEFT: 0px; WIDTH: 22px">
				    <input type="checkbox" id="allcheck" onclick="Selection.CheckAllBox(this);" />	
				</div>
				<div class="wm_inbox_headers_separate_noresize" style="LEFT: 23px; WIDTH: 1px"></div>
				<div onclick="javascript:Sort(this);" id="group" style="LEFT: 25px; WIDTH: 22px" class="wm_control"><%=si_Group%></div>
				<div class="wm_inbox_headers_separate_noresize" style="LEFT: 48px; WIDTH: 1px"></div>
				<div onclick="javascript:Sort(this);" id="name" class="wm_inbox_headers_from_subject wm_control"
					style="LEFT: 50px; WIDTH: 138px">
					<%=si_Name%>
					<%=_manager.GetString("Name")%>
				</div>
				<div class="wm_inbox_headers_separate_noresize" style="LEFT: 188px; WIDTH: 1px"></div>
				<div onclick="javascript:Sort(this);" id="email" class="wm_inbox_headers_from_subject wm_control"
					style="LEFT: 190px; WIDTH: 138px">
					<%=si_EMail%>
					<%=_manager.GetString("Email")%>
				</div>
			</div>
			<div class="wm_inbox_lines">
				
					<asp:Literal id="LiteralContactsGroups" runat="server"></asp:Literal>
				
				<!--<div class="wm_inbox_info_message" id="list"><%=_manager.GetString("InfoNoContactsGroups")%><br />
					<div class="wm_view_message_info"><%=_manager.GetString("InfoNewContactsGroups")%></div>
				</div>-->
			</div>
		</div>
	</div>
	<div class="wm_contacts_view_edit" id="contacts_viewer">
	    <asp:Literal id="LiteralContactsViewer_1" runat="server"></asp:Literal>
	    <asp:PlaceHolder id="PlaceHolderContact" Runat="server"></asp:PlaceHolder>
	    <asp:Literal id="LiteralContactsViewer_2" runat="server"></asp:Literal>
	</div>
</div>
<div id="lowtoolbar" class="wm_lowtoolbar">
    <span class="wm_lowtoolbar_messages">
        <%=contacts_count%> <%=_manager.GetString("ContactsCount")%>
    </span>
</div>
