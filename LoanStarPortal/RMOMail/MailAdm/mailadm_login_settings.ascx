<%@ Control Language="c#" AutoEventWireup="True" Codebehind="mailadm_login_settings.ascx.cs" Inherits="WebMailPro.MailAdm.mailadm_login_settings" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type="text/javascript">
	function DisableAllControls()
	{
		document.getElementById('lgnSettingsID_hideLoginSelect').readOnly = true;
		document.getElementById('lgnSettingsID_hideLoginSelect').style.background = "#EEEEEE";
		
		document.getElementById('lgnSettingsID_txtUseDomain').readOnly = true;
		document.getElementById('lgnSettingsID_txtUseDomain').style.background = "#EEEEEE";

		document.getElementById('lgnSettingsID_intDisplayDomainAfterLoginField').disabled = true;

		document.getElementById('lgnSettingsID_intLoginAsConcatination').disabled = true;
	}
	
	function StandartLoginRadioClick()
	{
		DisableAllControls();
	}
	
	function HideLoginRadioClick()
	{
		DisableAllControls();
		document.getElementById('lgnSettingsID_hideLoginSelect').readOnly = false;
		document.getElementById('lgnSettingsID_hideLoginSelect').style.background = "#FFFFFF";
	}
	
	function HideEmailRadioClick()
	{
		DisableAllControls();
		document.getElementById('lgnSettingsID_txtUseDomain').readOnly = false;
		document.getElementById('lgnSettingsID_txtUseDomain').style.background = "#FFFFFF";

		document.getElementById('lgnSettingsID_intDisplayDomainAfterLoginField').disabled = false;

		document.getElementById('lgnSettingsID_intLoginAsConcatination').disabled = false;
	}
</script>
<table class="wm_admin_center" width="500">
	<TBODY>
		<tr>
			<td width="50"></td>
			<td></td>
		</tr>
		<tr>
		<tr>
			<td class="wm_admin_title" colSpan="2">Login Settings</td>
		</tr>
		<tr>
			<td colSpan="2"><br>
			</td>
		</tr>
		<tr>
			<td vAlign="top" align="right"><input id="standartLoginRadio" style="VERTICAL-ALIGN: middle" onclick="StandartLoginRadioClick()"
					type="radio" name="standartLoginRadio" runat="server"></td>
			<td><label for="lgnSettingsID_standartLoginRadio">Standard login panel</label></td>
		</tr>
		<tr>
			<td vAlign="top" align="right"><br>
				<input id="hideLoginRadio" style="VERTICAL-ALIGN: middle" onclick="HideLoginRadioClick()"
					type="radio" name="standartLoginRadio" runat="server"></td>
			<td><br>
				<label for="lgnSettingsID_hideLoginRadio">Hide login field</label>
				<br>
				<br>
				<select class="wm_input" id="hideLoginSelect" runat="server">
					<option value="1" selected>Use Email as Login</option>
					<option value="0">Use Account-name as Login</option>
				</select></td>
		</tr>
		<tr>
			<td vAlign="top" align="right"><br>
				<input id="hideEmailRadio" style="VERTICAL-ALIGN: middle" onclick="HideEmailRadioClick()"
					type="radio" name="standartLoginRadio" runat="server"></td>
			<td><br>
				<label for="lgnSettingsID_hideEmailRadio">Hide email field</label>
				<br>
				<br>
				&nbsp;<input class="wm_input" id="txtUseDomain" type="text" size="20" runat="server">&nbsp;&nbsp;domain 
				to use
				<br>
				<br>
				<input id="intDisplayDomainAfterLoginField" style="VERTICAL-ALIGN: middle" type="checkbox"
					runat="server"><label for="lgnSettingsID_intDisplayDomainAfterLoginField">Display 
					domain after login field</label>
				<br>
				<br>
				<input id="intLoginAsConcatination" style="VERTICAL-ALIGN: middle" type="checkbox" runat="server">
				<label for="lgnSettingsID_intLoginAsConcatination">Login as concatenation of 
					"Login" field + "@" + domain</label>
			</td>
		</tr>
		<tr>
			<td vAlign="top" align="right"><input id="intAllowAdvancedLogin" style="VERTICAL-ALIGN: middle" type="checkbox" runat="server"></td>
			<td><label for="lgnSettingsID_intAllowAdvancedLogin">Allow advanced login</label>
			</td>
		</tr>
		<tr>
			<td vAlign="top" align="right"><input id="intAutomaticCorrectLogin" style="VERTICAL-ALIGN: middle" type="checkbox" runat="server"></td>
			<td><label for="lgnSettingsID_intAutomaticCorrectLogin">Automatically detect and 
					correct if user inputs e-mail instead of account-name</label>
			</td>
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
				<asp:Button id="SaveButton" runat="server" text="Save" Width="100" cssclass="wm_button" onclick="SaveButton_Click" />&nbsp;
			</td>
		</tr>
	</TBODY>
</table>
<script type="text/javascript">
	if (document.getElementById('lgnSettingsID_hideLoginRadio').checked)
	{
		HideLoginRadioClick();
	}
	else if (document.getElementById('lgnSettingsID_hideEmailRadio').checked)
	{
		HideEmailRadioClick();
	}
	else if (document.getElementById('lgnSettingsID_standartLoginRadio').checked)
	{
		StandartLoginRadioClick();
	}
</script>
