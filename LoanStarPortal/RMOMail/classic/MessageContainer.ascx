<%@ Control Language="c#" AutoEventWireup="True" Codebehind="MessageContainer.ascx.cs" Inherits="WebMailPro.classic.MessageContainer" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div class="wm_message_container" id="message_container">
	<table id="message_table">
		<tr>
			<td>
				<div id="eis">
					<asp:Panel ID="eisPanel" Visible="False" Runat="server">
						<TABLE class="wm_view_message">
							<TR>
								<TD class="wm_safety_info">
									<span id="eisText" class="<%=_eisTextClass%>">
										<%=_manager.GetString("PicturesBlocked")%>
									</span>
									<asp:LinkButton ID="eisShowOne" Runat="server"></asp:LinkButton>
									<asp:LinkButton ID="eisShowAll" Runat="server"></asp:LinkButton>
								</TD>
							</TR>
						</TABLE>
					</asp:Panel>
					</div>
				<div class="wm_message_headers" id="message_headers">
					<div>
					    <asp:Label id="LabelFrom" runat="server" class="wm_message_left wm_message_resized"></asp:Label>
						<!-- add to address book -->
						<% if (LabelFrom.Visible) { %>
						<img src="skins/<%=_skin%>/contacts/save.gif" title="<%=_manager.GetString("AddToAddressBokk")%>" onclick="javascript:AddContact();" class="wm_add_address_book_img"/>
						<% } %>
						<span class="wm_message_left"></span><span class="wm_message_right" id="SwitchTo" runat="server">
							<a href="javascript:void(0);" onclick="<%=LinkButtonSwitchToJs%>" id="LinkButtonSwitchTo">
								<%=LinkButtonSwitchToStr%>
							</a></span>
					</div>
					<div>
						<asp:Label id="LabelTo" runat="server" class="wm_message_left wm_message_resized"></asp:Label>
						<asp:Label id="LabelDate" runat="server" class="wm_message_left"></asp:Label>
					</div>
					<div class="" runat="server" id="CC">
					    <asp:Label id="LabelCC" runat="server" class="wm_message_left wm_message_resized"></asp:Label>
					</div>
					<div class="" runat="server" id="BCC">
						<asp:Label id="LabelBCC" runat="server" class="wm_message_left wm_message_resized"></asp:Label>
					</div>
					<div>
					    <asp:Label id="LabelSubject" runat="server" class="wm_message_left wm_message_resized"></asp:Label>
						<span class="wm_message_right" runat="server" id="CharsetList"><font>
								<%=_manager.GetString("Charset")%>
								:</font>
							<asp:DropDownList id="DropDownListCharsets" runat="server" onchange="javascript:ChangeCharset()"></asp:DropDownList>
						</span>
					</div>
				</div>
			</td>
		</tr>
		<tr>
			<td>
				<div id="messageBody" style="POSITION: relative">
					<div id="MessageAttachments" class="wm_message_attachments" style="POSITION: absolute; TOP: 0px">
						<asp:Literal ID="LiteralMessageAttachments" Runat="server"></asp:Literal>
					</div>
					<div id="vres" class="wm_vresizer_mess" style="POSITION: absolute; TOP: 0px"></div>
					<div class="wm_message" id="message_center" style="LEFT: 0px; POSITION: absolute; TOP: 0px">
						<asp:Literal ID="Component_MessageBody" Runat="server"></asp:Literal>
					</div>
				</div>
			</td>
		</tr>
		</TBODY>
	</table>
</div>
<script type="text/javascript">
 function PrevImg(href)
    {
        var shown = window.open(href, 'Popup', 'toolbar=yes,status=no,scrollbars=yes,resizable=yes,width=760,height=480');
        shown.focus();
    }
</script>
