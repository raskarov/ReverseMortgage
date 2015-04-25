<%@ Control Language="c#" AutoEventWireup="True" Codebehind="LowToolbar.ascx.cs" Inherits="WebMailPro.classic.LowToolbar" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<span class="wm_lowtoolbar_messages"><%=_folderMessageCount%> <%=_manager.GetString("MessagesInFolder")%></span>
<span class="wm_lowtoolbar_space_info" title="<%=_manager.GetString("YouUsing")%> <%=used%>% <%=_manager.GetString("OfYour")%> <%=full%>">
	<div class="wm_progressbar">
		<div class="wm_progressbar_used" style="width: <%=used%>px;"></div>
	</div>
</span>