<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Default.ascx.cs" Inherits="WebMailPro.classic._DefaultScr" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register Src="LowToolbar.ascx" TagPrefix="BaseWebmail" TagName="LowToolbar" %>
<%@ Register Src="FoldersPart.ascx" TagPrefix="BaseWebmail" TagName="FoldersPart" %>
<%@ Register Src="MailList.ascx" TagPrefix="BaseWebmail" TagName="MailList" %>
<%@ Register Src="MessageContainer.ascx" TagPrefix="BaseWebmail" TagName="MessageContainer" %>
<div class="wm_background">
	<table class="wm_mail_container" id="main_container">
		<tr>
			<td rowspan="3">
				<BaseWebmail:FoldersPart id="_Control_FoldersPart" runat="server" Skin=<%#_skin%>></BaseWebmail:FoldersPart>
			</td>
			<td class="wm_vresizer_part" rowspan="3">
				<div class="wm_vresizer_width"></div>
				<div class="wm_vresizer" id="vert_resizer"></div>
				<div class="wm_vresizer_width"></div>
			</td>
			<td id="inbox_part">
				<BaseWebmail:MailList id="_Control_Maillist" runat="server"  Skin=<%#_skin%>></BaseWebmail:MailList>
			</td>
		</tr>
		<tr>
			<td>
				<div class="wm_hresizer_height"></div>
				<div class="wm_hresizer" id="hor_resizer"></div>
				<div class="wm_hresizer_height"></div>
			</td>
		</tr>
		<tr>
		<% if(ShowPreviewPane)
			{%>
			<td id="message_td">
		<%  }
			else
			{%>
			<td id="message_td" class="wm_hide">
		<%	}%>
				<BaseWebmail:MessageContainer id="_Control_Messagecontainer" runat="server"  Skin=<%#_skin%>></BaseWebmail:MessageContainer>
			</td>
		</tr>
		<tr>
			<td class="wm_lowtoolbar" colspan="3" id="lowtoolbar">
				<BaseWebmail:LowToolbar id="_Control_Lowtoolbar" runat="server" Skin=<%#_skin%>></BaseWebmail:LowToolbar>
			</td>
		</tr>
	</table>
</div>
