<%@ Control Language="c#" AutoEventWireup="True" Codebehind="ViewMessage.ascx.cs" Inherits="WebMailPro.classic.ViewMessage" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<!--<table cellspacing="0" cellpadding="0" border="0" id="wm_mail_container" class="wm_mail_container">
	<tr>
		<td> -->
		<div id="wm_mail_container">
			<table id="message_headers" class="wm_view_message">
				<tr>
					<td class="wm_view_message_title">
						<%=_manager.GetString("From")%>:
					</td>
					<td>
						<span id="fromSpan">
							<asp:Label ID="LabelFrom" Runat="server"></asp:Label>
						</span>
						<img src="skins/<%=_skin%>/contacts/save.gif" title="<%=_manager.GetString("AddToAddressBokk")%>" onclick="javascript:AddContact();" class="wm_add_address_book_img"/>
					</td>
					<td class="wm_headers_switcher">
						<nobr>
							<a id="fullheadersControl" href="#" onclick="return FullHeaders.Show();">
								<%=_manager.GetString("ShowFullHeaders")%>
							</a>
						</nobr>
					</td>
				</tr>
				<tr>
					<td class="wm_view_message_title">
						<%=_manager.GetString("To")%>:
					</td>
					<td colspan="2">
						<asp:Label ID="LabelTo" Runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td class="wm_view_message_title">
						<%=_manager.GetString("Date")%>:
					</td>
					<td colspan="2">
						<asp:Label ID="LabelDate" Runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td class="wm_view_message_title">
						<%=_manager.GetString("Subject")%>:
					</td>
					<td>
						<asp:Label ID="LabelSubject" Runat="server"></asp:Label>
					</td>
				</tr>
				<tr runat="server" id="CharsetList">
					<td class="wm_view_message_title">
						<%=_manager.GetString("Charset")%>:
					</td>
					<td>
						<asp:DropDownList CssClass="wm_view_message_select" id="DropDownListCharsets" runat="server" onchange="javascript:ChangeCharset()"></asp:DropDownList>
					</td>
				</tr>
			</table>
			<table id="table_mail" cellpadding="0" cellspacing="0" class="wm_view_message">
				<tr>
					<td colspan="2">
						<div id="message" style="POSITION: relative">
							<div id="attachments" style="POSITION: absolute; top: 0px;" class="wm_message_attachments">
								<asp:Literal id="LiteralAttachments" runat="server"></asp:Literal>
							</div>
							<div id="resizerDiv" style="POSITION: absolute; top: 0px;" class="wm_vresizer_mess"></div>
							<div class="wm_message" id="message_body" style="POSITION: absolute; top: 0px;">
								<asp:Literal id="LiteralMessageBody" runat="server"></asp:Literal>
							</div>
						</div>
					</td>
				</tr>
				<tr id="lowtoolbar" class="wm_lowtoolbar">
					<td colspan="2">
					    <span class="wm_lowtoolbar_plain_html">
						    <asp:LinkButton ID="LinkButtonSwitchTo" Runat="server"></asp:LinkButton>
						</span>
					</td>			
				</tr>
			</table>
			</div>
			<!--
		</td>
	</tr>
</table> -->
<div id="headersCont" class="wm_hide">
	<div class="wm_message_rfc822" style="TEXT-ALIGN: left" id="headersDiv" runat="server"></div>
	<div class="wm_hide_headers">
		<a href="#" onclick="return FullHeaders.Hide();">
			<%=_manager.GetString("Close")%>
		</a>
	</div>
</div>
<script type="text/javascript">
 function PrevImg(href)
    {
        var shown = window.open(href, 'Popup', 'toolbar=yes,status=no,scrollbars=yes,resizable=yes,width=760,height=480');
        shown.focus();
    }
</script>