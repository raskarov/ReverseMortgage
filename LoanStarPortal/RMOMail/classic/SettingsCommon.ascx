<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SettingsCommon.ascx.cs" Inherits="WebMailPro.classic.SettingsCommon" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type="text/javascript">
	var DataHelpUrl='advanced_data_help.aspx';
</script>
<form class="">
	<table class="wm_settings_common">
		<tr class="">
			<td class="wm_settings_title"><%=_resMan.GetString("MsgsPerPage")%>:</td>
			<td colSpan="2"><input class="wm_input" id="textBoxMessagesPerPage" type="text" maxLength="2" size="2"
					runat="server">
			</td>
		</tr>
		<tr class="">
			<td>
			<td colSpan="2"><input class="wm_checkbox" id="checkBoxDisableRte" type="checkbox" value="1" runat="server">
				<label for="settingsID_settingsCommonID_checkBoxDisableRte">
					<%=_resMan.GetString("DisableRTE")%>
				</label>
			</td>
		</tr>
		<tr class="">
			<td class="wm_settings_title"><%=_resMan.GetString("Skin")%>:</td>
			<td colSpan="2"><select id="comboBoxSkins" runat="server"></select>
			</td>
		</tr>
		<tr class="wm_hide">
			<td class="wm_settings_title">Default incoming charset:</td>
			<td colSpan="2"><select id="comboBoxIncomingCharset" runat="server"></select></td>
		</tr>
		<tr class="">
			<td class="wm_settings_title"><%=_resMan.GetString("DefCharset")%>:</td>
			<td colSpan="2"><select id="comboBoxDefaultCharset" runat="server"></select></td>
		</tr>
		<tr class="">
			<td class="wm_settings_title"><%=_resMan.GetString("DefTimeOffset")%>:</td>
			<td colSpan="2"><select id="comboBoxTimeOffset" runat="server"></select></td>
		</tr>
		<tr class="">
			<td class="wm_settings_title"><%=_resMan.GetString("DefLanguage")%>:</td>
			<td colSpan="2"><select id="comboBoxLang" runat="server"></select>
			</td>
		</tr>
        <tr class="">
            <td class="wm_settings_title"><%=_resMan.GetString("DefTimeFormat")%>:</td>
            <td>
                <input runat="server" id="time_format_12" class="wm_checkbox" type="radio" value="1" name="time_format"/>
                <label for="settingsID_settingsCommonID_time_format_12">1PM</label>
                <span>&nbsp;&nbsp;</span>
                <input runat="server" id="time_format_24" class="wm_checkbox" type="radio" value="0" name="time_format"/>
                <label for="settingsID_settingsCommonID_time_format_24">13:00</label>
            </td>
        </tr>
        <tr>
            <td class="wm_settings_title"><%=_resMan.GetString("DefDateFormat")%>:</td>
            <td colspan="2">
                <select runat="server" id="date_format" name="date_format" onchange="ChangeAdvanced(this);"></select>
            </td>
        </tr>
        <tr>
            <td class="wm_settings_title"><%=_resMan.GetString("DateAdvanced")%>:</td>
            <td colspan="2">
                <input class="wm_input" id="textBoxAdvancedDate" type="text" maxlength="20" runat="server" onchange="SetAdvanced();"/>
                <img class="wm_settings_help" src="skins/<%#Skin%>/icons/help.gif" onclick="PopupDataHelp();" />
            </td>
        </tr>
		<tr class="">
			<td></td>
			<td colSpan="2"><input class="wm_checkbox" id="view_mode" type="checkbox" value="1" runat="server">
				<label for="settingsID_settingsCommonID_view_mode">
					<%=_resMan.GetString("ShowViewPane")%>
				</label>
			</td>
		</tr>
		<tr class="">
			<td></td>
			<td colSpan="2"><input class="wm_checkbox" id="pictures_show" type="checkbox" value="1" runat="server">
				<label for="settingsID_settingsCommonID_pictures_show">
					<%=_resMan.GetString("AlwaysShowPictures")%>
				</label>
			</td>
		</tr>
	</table>
	<table class="wm_settings_buttons">
		<tr>
			<td><input class="wm_button" onclick="if (!CheckSCFields()) return false;" id="saveButton"
					type="button" value="Save" runat="server" onserverclick="saveButton_ServerClick"></td>
		</tr>
	</table>
</form>
