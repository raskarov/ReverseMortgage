<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SettingsEmailAccountsSignature.ascx.cs" Inherits="WebMailPro.classic.SettingsEmailAccountsSignature" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<input runat="server" type="hidden" name="isHtml" id="isHtml" value="0">
<table class="wm_settings_signature">
	<tr id="plain_mess">
		<td>
			<div id="external_mess" class="wm_input wm_plain_editor_container">
				<textarea runat="server" id="editor_area" class="wm_plain_editor_text" name="signature"></textarea>
			</div>
		</td>
	</tr>
	<tr>
		<td runat="server" class="wm_settings_title" id="switcherTr">
			<%=SwitcherCode()%>
		</td>
	</tr>
	<tr>
		<td>
			<input runat="server" class="wm_checkbox" type="checkbox" value="1" onclick="(this.checked) ? document.getElementById('settingsID_settingsEmailAccountsID_settingsEmailAccountsSignatureID_replies_forwards').disabled=false:document.getElementById('settingsID_settingsEmailAccountsID_settingsEmailAccountsSignatureID_replies_forwards').disabled=true;"
				id="add_signatures" name="add_signatures"> <label for="settingsID_settingsEmailAccountsID_settingsEmailAccountsSignatureID_add_signatures">
				<%=_resMan.GetString("AddSignatures")%>
			</label>
		</td>
	<tr>
		<td>
			<input runat="server" class="wm_checkbox wm_settings_para" type="checkbox" value="1" id="replies_forwards"
				name="replies_forwards"> <label for="settingsID_settingsEmailAccountsID_settingsEmailAccountsSignatureID_replies_forwards">
				<%=_resMan.GetString("DontAddToReplies")%>
			</label>
		</td>
	</tr>
</table>
<table class="wm_settings_buttons">
	<tr>
		<td>
			<input runat="server" class="wm_button" id="saveButton" type="submit" onclick="return saveSignature();" name="saveButton" onserverclick="saveButton_ServerClick">
		</td>
	</tr>
</table>
