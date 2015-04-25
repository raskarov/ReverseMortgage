<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Settings.ascx.cs" Inherits="WebMailPro.classic.Settings" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div class="">
	<table class="wm_settings">
		<tr>
			<td class="wm_settings_nav">
				<div class="<%=GetMenuClass(0)%>"><nobr><img src="skins/<%#Skin%>/settings/menu_common.gif"><a href="basewebmail.aspx?scr=settings_common"><%=_resMan.GetString("Common")%></a></nobr></div>
				<div class="<%=GetMenuClass(1)%>"><nobr><img src="skins/<%#Skin%>/settings/menu_accounts.gif"><a href="basewebmail.aspx?scr=settings_accounts_properties"><%=_resMan.GetString("EmailAccounts")%></a></nobr></div>
				<% if (_allowContacts) { %>
				<div class="<%=GetMenuClass(2)%>"><nobr><img src="skins/<%#Skin%>/settings/menu_contacts.gif"><a href="basewebmail.aspx?scr=settings_contacts"><%=_resMan.GetString("Contacts")%></a></nobr></div>
				<% } %>
			</td>
			<td class="wm_settings_cont">
				<asp:PlaceHolder ID="contentPlaceHolder" Runat="server"></asp:PlaceHolder>
				<!-- TODO: controls -->
			</td>
		</tr>
	</table>
</div>
