<%@ Control Language="c#" AutoEventWireup="True" Codebehind="mailadm_interface_settings.ascx.cs" Inherits="WebMailPro.MailAdm.mailadm_interface_settings" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table class="wm_admin_center" width="500" border="0">
	<tr>
		<td width="150"></td>
		<td></td>
	</tr>
	<tr>
		<td colspan="2" class="wm_admin_title">Interface Settings</td>
	</tr>
	<tr>
		<td colspan="2"><br></td>
	</tr>
	<tr>
		<td align="right">Mails per page:</td>
		<td>
			<input type="text" id="intMailsPerPage" runat="server" size="4" MaxLength="4" class="wm_input">
			<asp:requiredfieldvalidator id="mailsReqValidator" Runat="server" Display="Dynamic" ErrorMessage="!" ControlToValidate="intMailsPerPage"></asp:requiredfieldvalidator>
			<asp:RangeValidator ID="mailsRangeValidator" Runat="server" EnableClientScript="True" ControlToValidate="intMailsPerPage" Display="Dynamic" ErrorMessage="!" Type="Integer" MinimumValue="0" MaximumValue="9999" ></asp:RangeValidator>
		</td>
	</tr>
	<tr>
		<td align="right">Default skin:</td>
		<td>
			<select id="txtDefaultSkin" runat="server" class="wm_input" style="width: 150px" />
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<input type="checkbox" runat="server" style="vertical-align: middle" id="intAllowUsersChangeSkin">
			<label for="ifcSettingsID_intAllowUsersChangeSkin">Allow users to change skin</label>
		</td>
	</tr>
	<tr>
		<td align="right">Default language:</td>
		<td>
			<select runat="server" id="txtDefaultLanguage" class="wm_input" style="width: 150px">
			</select>
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<input type="checkbox" runat="server" id="intAllowUsersChangeLanguage" style="vertical-align: middle">
			<label for="ifcSettingsID_intAllowUsersChangeLanguage">Allow users to change interface language</label>
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<input type="checkbox" runat="server" style="vertical-align: middle" id="intShowTextLabels">
			<label for="ifcSettingsID_intShowTextLabels">Show text labels</label>
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<input type="checkbox" runat="server" style="vertical-align: middle" id="intAllowAjaxVersion">
			<label for="ifcSettingsID_intAllowAjaxVersion">Allow AJAX Version</label>
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<input type="checkbox" runat="server" style="vertical-align: middle" id="intAllowDHTMLEditor">
			<label for="ifcSettingsID_intAllowDHTMLEditor">Allow DHTML editor</label>
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<input type="checkbox" runat="server" style="vertical-align: middle" id="intAllowContacts">
			<label for="ifcSettingsID_intAllowContacts">Allow Contacts</label>
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<input type="checkbox" runat="server" style="vertical-align: middle" id="intAllowCalendar">
			<label for="ifcSettingsID_intAllowCalendar">Allow Calendar</label>
		</td>
	</tr>
	<tr>
		<td align="center" colSpan="2"><br>
			<div runat="server" id="messLabelID" class="messdiv" />
			<br>
			<div runat="server" id="errorLabelID" class="messdiv" />
		</td>
	</tr> <!-- hr -->
	<tr>
		<td colspan="2"><hr size="1"></td>
	</tr>
	<tr>
		<td colspan="2" align="right">
			<asp:Button runat="server" id="SubmitButton" text="Save" cssclass="wm_button" Width="100" onclick="SubmitButton_Click" />&nbsp;
		</td>
	</tr>
</table>
