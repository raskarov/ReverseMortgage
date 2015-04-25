<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SettingsEmailAccountsFilters.ascx.cs" Inherits="WebMailPro.classic.SettingsEmailAccountsFilters" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table class="wm_settings_list" id="filterTable">
<%=OutputFiltersTable()%>
</table>
<table class="wm_settings_edit_filter">
	<tr>
		<td runat="server" colspan="3" style="font-weight: bold;" id="trHeader"></td>
	</tr>
	<tr>
		<td class="wm_settings_title"><%=_resMan.GetString("Field")%>:</td>
		<td colspan="2">
			<input runat="server" type="hidden" id="filterId" name="filterId" />
			<select runat="server" id="ruleFilter" name="ruleFilter" />
		</td>
	</tr>
	<tr>
		<td class="wm_settings_title"><%=_resMan.GetString("Condition")%>:</td>
		<td>
			<select runat="server" name="fcontain" id="conditionFilter" />
		</td>
		<td>
			<input runat="server" class="wm_input wm_edit_filter_input" type="text" name="filter_text" id="filter_text" maxlength="99" />
		</td>
	</tr>
	<tr>
		<td class="wm_settings_title"><%=_resMan.GetString("Action")%>:</td>
		<td>
			<select runat="server" name="faction" id="actionFilter" onchange="ChangeAction();" />
		</td>
		<td>
			<select runat="server" name="ffolder" id="filterFolder" />
		</td>
	</tr>
	<tr>
		<td colspan="3" class="wm_settings_title">
			<hr>
			<input runat="server" class="wm_button" type="submit" onclick="return CheckSubmit();" id="submitType" name="submitType" />
			<input runat="server" class="wm_button" type="button" onclick="document.location='basewebmail.aspx?scr=settings_accounts_filters'" id="cancelButton" name="cancelButton" />
		</td>
	</tr>
</table>
<table class="wm_settings_filters">
	<tr>
		<td class="wm_settings_header"><%=_resMan.GetString("OtherFilterSettings")%></td>
	</tr>
	<tr>
		<td>
			<input runat="server" class="wm_checkbox" type="checkbox" value="1" id="checkbox_x_spam" name="x-spam" />
			<label for="settingsID_settingsEmailAccountsID_settingsEmailAccountsFiltersID_checkbox_x_spam"><%=_resMan.GetString("ConsiderXSpam")%></label>
		</td>
	</tr>
	<tr>
		<td class="wm_settings_title">
			<hr><input runat="server" id="applyButton" name="applyButton" type="submit" onclick="return !IsDemo();" class="wm_button" value="" />
		</td>
	</tr>
</table>