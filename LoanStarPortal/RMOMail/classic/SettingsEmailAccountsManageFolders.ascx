<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SettingsEmailAccountsManageFolders.ascx.cs" Inherits="WebMailPro.classic.SettingsEmailAccountsManageFolders" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div class="">
	<table class="wm_settings_manage_folders">
		<%=OutputHeader()%>
		<%=OutputFolders()%>
	</table>
</div>
<table class="wm_secondary_info" runat="server" id="folderInfo">
	<tr>
		<td class="wm_secondary_info">
			<%=_resMan.GetString("InfoDeleteNotEmptyFolders")%>
		</td>
	</tr>
</table>
<table class="wm_settings_buttons">
	<tr class="">
		<td class="wm_delete_button">
			<input type="button" value="<%=_resMan.GetString("AddNewFolder")%>" onclick="document.getElementById('new_folder').className='';"
				class="wm_button"> <span></span><input type="button" value="<%=_resMan.GetString("DeleteSelected")%>" onclick="DeleteFolders();" class="wm_button">
		</td>
	</tr>
</table>
<div id="new_folder" class="wm_hide">
	<table class="wm_settings_part_info">
		<tr>
			<td><%=_resMan.GetString("NewFolder")%></td>
		</tr>
	</table>
	<table class="wm_settings_new_folder">
		<tr>
			<td class="wm_settings_title"><%=_resMan.GetString("ParentFolder")%>:</td>
			<td>
				<select runat="server" id="comboParentFolder" class="">
				</select>
			</td>
			<td runat="server" id="tdNewFolderIn" class="" rowspan="2">
				<input runat="server" type="radio" name="on_mail_server" id="radioOnMailServer" class="wm_checkbox">
				<label for="settingsID_settingsEmailAccountsID_settingsEmailAccountsManageFolderID_radioOnMailServer">
					<%=_resMan.GetString("OnMailServer")%>
				</label>
				<br>
				<input runat="server" type="radio" name="on_mail_server" id="radioInWebmail" class="wm_checkbox">
				<label for="settingsID_settingsEmailAccountsID_settingsEmailAccountsManageFolderID_radioInWebmail">
					<%=_resMan.GetString("InWebMail")%>
				</label>
			</td>
		</tr>
		<tr>
			<td class="wm_settings_title"><%=_resMan.GetString("FolderName")%>:</td>
			<td><input runat="server" id="textBoxFolderName" type="text" maxlength="30" class="wm_input"></td>
		</tr>
	</table>
	<table class="wm_settings_buttons">
		<tr>
			<td>
    			<input type="button" class="wm_button" onclick="if (!CheckFolderName()) return false;" id="okButton" runat="server"> 
				<input type="button" class="wm_button" value="<%=_resMan.GetString("Cancel")%>" onclick="document.getElementById('new_folder').className='wm_hide';">
			</td>
		</tr>
	</table>
</div>
