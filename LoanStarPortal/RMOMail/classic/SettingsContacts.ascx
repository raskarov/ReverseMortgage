<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SettingsContacts.ascx.cs" Inherits="WebMailPro.classic.SettingsContacts" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table class="wm_settings_common">
	<tbody>
		<tr>
			<td><span><%=_resMan.GetString("ContactsPerPage")%>:</span><input class="wm_input" id="textBoxContactsPerPage" type="text" maxLength="2" size="2"
					runat="server">
			</td>
		</tr>
		<tr class="wm_hide">
			<td><input class="wm_checkbox" id="white_listing" type="checkbox"><label for="white_listing">Address 
					Book as White List</label></td>
		</tr>
	</tbody>
</table>
<table class="wm_settings_buttons">
	<tbody>
		<tr>
			<td><input onclick="if (!CheckCPPField()) return false;" class="wm_button" id="saveButton" type="button" value="Save" runat="server" onserverclick="saveButton_ServerClick"></td>
		</tr>
	</tbody>
</table>
