<%@ Control Language="c#" AutoEventWireup="True" Codebehind="mailadm_debug_settings.ascx.cs" Inherits="WebMailPro.MailAdm.mailadm_debug_settings" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<!-- [start center] -->
<script type="text/javascript">
	function PopUpWindow(url)
	{
		var shown = window.open(url, 'Popup',
			'left=(screen.width-700)/2,top=(screen.height-400)/2,'+
			'toolbar=no,location=no,directories=no,status=yes,scrollbars=yes,resizable=yes,'+
			'copyhistory=no,width=700,height=400');
		shown.focus();
		return false;
	}
</script>
<table class="wm_admin_center" width="500" border="0">
	<TBODY>
		<tr> <!--<td width="60"></td>		<td></td>--></tr>
		<tr>
			<td class="wm_admin_title" colSpan="2">Debug Settings</td>
		</tr>
		<tr>
			<td colSpan="2"><br>
			</td>
		</tr>
		<tr>
			<td align="right"><input type="checkbox" id="intEnableLogging" runat="server"></td>
			<td><label for="dbgSettingsID_intEnableLogging">Enable logging</label></td>
		</tr>
		<tr>
			<td align="right"></td>
			<td align="left">&nbsp;Path for log&nbsp;&nbsp;<input type="text" id="txtPathForLog" class="wm_input" runat="server" style="WIDTH: 330px"
					readonly></td>
		</tr>
		<tr>
			<td></td>
			<td align="left">
				<input type="button" runat="server" id="ShowAllLogButton" value="View entire log" class="wm_button"
					style="font-size: 11px; width: 150px;" onclick="PopUpWindow('mailadm.aspx?mode=show_log');">&nbsp;&nbsp;
				<input type="button" runat="server" id="ShowPartialLogButton" value="View last 50KB of log"
					class="wm_button" style="font-size: 11px; width: 150px;" onclick="PopUpWindow('mailadm.aspx?mode=show_log');">&nbsp;&nbsp;
				<input type="button" runat="server" value="Clear log" id="ClearLogButton" class="wm_button"
					style="font-size: 11px;" onserverclick="ClearLogButton_ServerClick">
			</td>
		</tr>
		<tr>
			<td align="right"><input type="checkbox" id="intDisableErrorHandling" runat="server"></td>
			<td><label for="dbgSettingsID_intDisableErrorHandling">Disable error handling</label></td>
		</tr>
		<tr>
			<td align="center" colSpan="2"><br>
				<div Runat="server" ID="messLabelID" Class="messdiv" />
				<br>
				<div runat="server" id="errorLabelID" class="messdiv" />
			</td>
		</tr> <!-- hr -->
		<tr>
			<td colSpan="2">
				<hr SIZE="1">
			</td>
		</tr>
		<tr>
			<td align="right" colSpan="2">
				<asp:Button id="SaveButton" text="Save" cssclass="wm_button" Runat="server" Width="100" onclick="SaveButton_Click" />&nbsp;
			</td>
		</tr>
	</TBODY>
</table>
