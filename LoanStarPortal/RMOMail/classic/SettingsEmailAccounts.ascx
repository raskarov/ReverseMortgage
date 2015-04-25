<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SettingsEmailAccounts.ascx.cs" Inherits="WebMailPro.classic.SettingsEmailAccounts" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div class="wm_settings_list">
	<table>
		<%=OutputAccounts()%>
	</table>
</div>
<% if (_allowChangeEmailSettings && _allowAddNewAccount) { %>
<table class="wm_settings_add_account_button">
	<tr>
		<td><input runat="server" id="buttonNewAccount" class="wm_button" type="button" onclick="document.location='basewebmail.aspx?scr=settings_accounts_properties&new_acct=1'"></td>
	</tr>
</table>
<% } %>
<div class="<%=classForAllTabs%>">
	<div class="wm_settings_switcher_indent"></div>
	<div class="<%=GetClassForTab(3)%>"><A href="basewebmail.aspx?scr=settings_accounts_folders"><%=_resMan.GetString("ManageFolders")%></A></div>
	<div class="<%=GetClassForTab(2)%>"><A href="basewebmail.aspx?scr=settings_accounts_signature"><%=_resMan.GetString("Signature")%></A></div>
	<div class="<%=GetClassForTab(1)%>"><A href="basewebmail.aspx?scr=settings_accounts_filters"><%=_resMan.GetString("Filters")%></A></div>
	<% if (_allowChangeEmailSettings) { %>
	<div class="<%=GetClassForTab(0)%>"><A href="basewebmail.aspx?scr=settings_accounts_properties"><%=_resMan.GetString("Properties")%></A></div>
	<% } %>
</div>
<asp:PlaceHolder ID="contentPlaceHolder" Runat="server"></asp:PlaceHolder>