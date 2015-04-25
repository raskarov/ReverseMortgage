<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SettingsEmailAccountsProperties.ascx.cs" Inherits="WebMailPro.classic.SettingsEmailAccountsProperties" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table class="wm_settings_properties">
	<tr>
		<td colSpan="3"><input class="wm_checkbox" id="checkBoxDefAcct" type="checkbox" value="1" runat="server">
			<label for="settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_checkBoxDefAcct">
				<%=_resMan.GetString("UseForLogin")%>
			</label>
		</td>
	</tr>
	<tr>
		<td class="wm_settings_title"><%=_resMan.GetString("MailFriendlyName")%>:</td>
		<td colSpan="2"><input class="wm_input wm_settings_input" id="textBoxName" type="text" maxLength="100"
				runat="server">
		</td>
	</tr>
	<tr>
		<td class="wm_settings_title">* <%=_resMan.GetString("MailEmail")%>:</td>
		<td colSpan="2"><input class="wm_input wm_settings_input" id="textBoxEmail" type="text" maxLength="100"
				runat="server">
		</td>
	</tr>
	<tr>
		<td class="wm_settings_title">* <%=_resMan.GetString("MailIncHost")%>:</td>
		<td><input class="wm_input" id="textBoxIncHost" type="text" maxLength="100" runat="server">
			<span class="" id="spanProtocol" runat="server">Pop3</span>
			<select id="comboBoxProtocol" runat="server">
			</select>
		</td>
		<td><span>* <%=_resMan.GetString("MailIncPort")%>:</span><input class="wm_input wm_port_input" id="textBoxIncPort" type="text" maxLength="4" size="3"
				runat="server">
		</td>
	</tr>
	<tr>
		<td class="wm_settings_title">* <%=_resMan.GetString("MailIncLogin")%>:</td>
		<td colSpan="2"><input class="wm_input wm_settings_input" id="textBoxIncLogin" type="text" maxLength="100"
				runat="server">
		</td>
	</tr>
	<tr>
		<td class="wm_settings_title">* <%=_resMan.GetString("MailIncPass")%>:</td>
		<td colSpan="2"><asp:textbox id="textBoxIncPassword" maxLength="100" TextMode="Password" CssClass="wm_input wm_settings_input"
				Runat="server"></asp:textbox></td>
	</tr>
	<tr>
		<td class="wm_settings_title">* <%=_resMan.GetString("MailOutHost")%>:</td>
		<td><input class="wm_input" id="textBoxOutHost" type="text" maxLength="100" runat="server">
		</td>
		<td><span>* <%=_resMan.GetString("MailOutPort")%>:</span> <input class="wm_input wm_port_input" id="textBoxOutPort" type="text" maxLength="4" size="3"
				runat="server">
		</td>
	</tr>
	<tr>
		<td class="wm_settings_title"><%=_resMan.GetString("MailOutLogin")%>:</td>
		<td colSpan="2"><input class="wm_input wm_settings_input" id="textBoxOutLogin" type="text" maxLength="100"
				runat="server">
		</td>
	</tr>
	<tr>
		<td class="wm_settings_title"><%=_resMan.GetString("MailOutPass")%>:</td>
		<td colSpan="2"><asp:textbox id="textBoxOutPassword" maxLength="100" TextMode="password" CssClass="wm_input wm_settings_input"
				Runat="server"></asp:textbox></td>
	</tr>
	<tr>
		<td colSpan="3"><input class="wm_checkbox" id="checkBoxOutAuth" type="checkbox" value="1" runat="server">
			<label for="settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_checkBoxOutAuth">
				<%=_resMan.GetString("MailOutAuth1")%>
			</label><br/>
			<label for="mail_out_auth" class="wm_secondary_info wm_nextline_info"><%=_resMan.GetString("MailOutAuth2")%></label>
		</td>
	</tr>
	<tr>
		<td colSpan="3"><input class="wm_checkbox" id="checkBoxUseFriendlyName" type="checkbox" value="1" runat="server">
			<label for="settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_checkBoxUseFriendlyName">
				<%=_resMan.GetString("UseFriendlyNm1")%>
			</label>
			<label class="wm_secondary_info wm_inline_info" for="settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_checkBoxUseFriendlyName">
				<%=_resMan.GetString("UseFriendlyNm2")%>
			</label>
		</td>
	</tr>
	<tr>
		<td colSpan="3"><input class="wm_checkbox" id="checkBoxGetMailAtLogin" type="checkbox" value="1" runat="server">
			<label for="settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_checkBoxGetMailAtLogin">
				<%=_resMan.GetString("GetmailAtLogin")%>
			</label>
		</td>
	</tr>
	<tr id="pop_advanced">
		<td colSpan="3"><input class="wm_checkbox" id="mail_mode_0" type="radio" value="1" name="mail_mode" runat="server">
			<label for="settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_mail_mode_0">
				<%=_resMan.GetString("MailMode0")%>
			</label>
			<br>
			<input class="wm_checkbox" id="mail_mode_1" type="radio" value="2" name="mail_mode" runat="server">
			<label for="settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_mail_mode_1">
				<%=_resMan.GetString("MailMode1")%>
			</label>
			<br>
			<input class="wm_checkbox wm_settings_para" id="mail_mode_2" type="checkbox" value="1"
				runat="server"> <label for="settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_mail_mode_2">
				<%=_resMan.GetString("MailMode2")%>
			</label><input class="wm_input" id="textBoxDays" disabled type="text" size="1" runat="server"><span>
				<%=_resMan.GetString("MailsOnServerDays")%>
			</span>
			<br>
			<input class="wm_checkbox wm_settings_para" id="mail_mode_3" type="checkbox" value="1"
				runat="server"> <label for="settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_mail_mode_3">
				<%=_resMan.GetString("MailMode3")%>
			</label>
			
			<br /><br />
			<%=_resMan.GetString("InboxSyncType")%>:
			<select id="comboBoxInboxSyncType" runat="server" NAME="comboBoxInboxSyncType"></select>
		
			<br /><br />
			<input class="wm_checkbox" id="checkBoxDeleteNoExists" type="checkbox" runat="server" NAME="checkBoxDeleteNoExists">
			<label for="settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_checkBoxDeleteNoExists">
				<%=_resMan.GetString("DeleteFromDb")%>
			</label>
		
		</td>
	</tr>
</table>
<table class="wm_settings_buttons">
	<tbody>
		<tr>
			<td class="wm_secondary_info">
			    <%=_resMan.GetString("InfoRequiredFields")%>
			</td>
			<td>
			    <input class="wm_button" id="saveButton" type="button" onclick="if (IsDemo() == true) return false; if (!newAccountForm.CheckFields()) return false;" runat="server" onserverclick="saveButton_ServerClick">
			    <input class="wm_button" id="cancelButton" type="button" runat="server" onclick="document.location='basewebmail.aspx?scr=settings_accounts_properties';"> 
			</td>
		</tr>
	</tbody>
</table>
