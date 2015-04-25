<%@ Control Language="c#" AutoEventWireup="True" Codebehind="NewMessage.ascx.cs" Inherits="WebMailPro.classic.NewMessage" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:Panel ID="eisPanel" Visible="false" Runat="server">
	<table class="wm_view_message">
		<tr>
			<td class="wm_safety_info">
			    <span><%=_resMan.GetString("PicturesBlocked")%></span>
				<asp:LinkButton id="eisShowOne" Runat="server"></asp:LinkButton>.
			</td>
		</tr>
	</table>
</asp:Panel>
<table class="wm_new_message">
	<tr>
		<td class="wm_new_message_title"><%=_resMan.GetString("From")%>:</td>
		<td><input class="wm_input" id="fromTextBox" tabIndex="1" type="text" size="93" runat="server">
			<input id="id_message" type="hidden" name="id_message" value="<%=id%>"> <input id="priority_input" type="hidden" name="priority_input">
			<input id="ishtml" type="hidden" name="ishtml">
		</td>
	</tr>
	<tr>
		<td class="wm_new_message_title"><%=a_start1 + _resMan.GetString("To") + a_end%>:</td>
		<td><input class="wm_input" id="toTextBox" tabIndex="2" type="text" size="93" runat="server"></td>
	</tr>
	<tr>
		<td class="wm_new_message_title"><%=a_start2 + _resMan.GetString("CC") + a_end%>:</td>
		<td><input class="wm_input" id="ccTextBox" tabIndex="3" type="text" size="93" runat="server"><span>&nbsp;</span>
			<A id="bcc_mode_switcher" onclick="ChangeBCCMode(); return false;" tabIndex="-1" href="#">
				<%=_resMan.GetString("ShowBCC")%>
			</A>
		</td>
	</tr>
	<tr class="wm_hide" id="bcc">
		<td class="wm_new_message_title"><%=a_start3 + _resMan.GetString("BCC") + a_end%>:</td>
		<td><input class="wm_input" id="bccTextBox" tabIndex="4" type="text" size="93" runat="server"></td>
	</tr>
	<tr>
		<td class="wm_new_message_title"><%=_resMan.GetString("Subject")%>:</td>
		<td><input class="wm_input" id="subjectTextBox" tabIndex="5" type="text" size="93" runat="server"></td>
	</tr>
	<tr runat="server" id="trLoan" visible="false">
		<td class="wm_new_message_title">Loan:</td>
		<td><asp:Label ID="lblLoan" runat="server" Text="Loan name will be here!"></asp:Label></td>
	</tr>
	<tr id="plain_mess">
		<td colSpan="2">
			<div class="wm_input wm_plain_editor_container" id="editor_cont">
			    <textarea class="wm_plain_editor_text" id="editor_area" name="message"><%=body%></textarea>
			</div>
		</td>
	</tr>
	<tr class="">
		<td>
		<td class="wm_html_editor_switcher">
			<%=SwitcherCode()%>
		</td>
	</tr>
</table>
<table class="wm_new_message" id="attachmentTable">
	<%=attachmentsHtml%>
</table>
<table class="wm_new_message">
	<tr>
		<td class="wm_attach">
		    <iframe class="wm_hide" id="UploadFrame" name="UploadFrame" src="empty.html"></iframe>
			<span><%=_resMan.GetString("AttachFile")%>:&nbsp;</span>
			<span id="forfile"><input class="wm_file" id="fileupload" type="file" name="fileupload"></span>
			<input class="wm_button" onclick="addattach()" type="button" value="Attach">
		</td>
	</tr>
</table>

