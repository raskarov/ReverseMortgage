<%@ Control Language="c#" AutoEventWireup="True" Codebehind="mailadm_mail_server_integration.ascx.cs" Inherits="WebMailPro.MailAdm.mailadm_mail_server_integration" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table class="wm_admin_center" width="500" border="0">
	<tr>
		<td width="100"></td>
		<td width="160"></td>
		<td></td>
	</tr>
	<tr>
		<td colspan="3" class="wm_admin_title">Settings</td>
	</tr>
	<tr>
		<td colspan="3"><br>
		</td>
	</tr>
	<tr>
		<td align="right">
			<input runat="server" type="checkbox" name="intEnableMwServer" id="intEnableMwServer" value="1">
		</td>
		<td><label for="mailServerID_intEnableMwServer">Enable Integration</label></td>
	</tr>
	<tr>
		<td colspan="3">
			<div class="wm_safety_info">
				<b>Enable Integration</b> - allows managing accounts on AfterLogic XMail Server from WebMail.
			</div>
			<br>
		</td>
	</tr>
	<tr>
		<td align="right">Path to Server:
		</td>
		<td colspan="2">
			<input runat="server" type="text" id="txtWmServerRootPath" name="txtWmServerRootPath" value="C:\Program Files\AfterLogic XMail Server\MailRoot\"
				size="50" class="wm_input" maxlength="500">
		</td>
	</tr>
	<tr>
		<td colspan="3">
			<div class="wm_safety_info">
				<b>Path to Server</b> - path to the MailRoot folder of AfterLogic XMail Server in your system, for instance C:\Program Files\AfterLogic XMail Server\MailRoot\.
			</div>
		</td>
	</tr>
	
	<tr>
		<td align="right">Server Host: </td>
		<td colspan="2">
			<input type="text" runat="server" name="txtWmServerHostName" id="txtWmServerHostName" value="127.0.0.1" size="50" class="wm_input" maxlength="500">
		</td>
	</tr>
	<tr>
	    <td colspan="3">
            <div class="wm_safety_info">
                <b>Server Host</b> - IP address or hostname where AfterLogic XMail Server resides.
            </div>
            <br />
	    </td>
    </tr>
	<tr>
		<td align="right">
			<input type="checkbox"  runat="server" name="intWmAllowManageXMailAccounts" id="intWmAllowManageXMailAccounts" value="1" />
		</td>
		<td colspan="2">
		    <label for="mailServerID_intWmAllowManageXMailAccounts">
		        Allow&nbsp;users&nbsp;to&nbsp;manage&nbsp;accounts&nbsp;on&nbsp;AfterLogic&nbsp;XMail&nbsp;Server
		    </label>
		</td>
	</tr>
	<tr>
	    <td colspan="3">
            <div class="wm_safety_info">
                If a user adds or removes a linked account in his primary account settings and domain part of this account matches any of
                your domains hosted by AfterLogic XMail Server, this account will be added/removed on AfterLogic XMail Server.
            </div>
    	</td>
    </tr>
	<tr>
		<td align="center" colspan="3">
		    <br />
			<div runat="server" id="messLabelID" class="messdiv" />
			<br />
			<div runat="server" id="errorLabelID" class="messdiv" />
		</td>
	</tr>
	<!-- hr -->
	<tr>
		<td colspan="3">
		    <hr size="1">
		</td>
	</tr>
	<tr>
		<td colspan="3" align="right">
			<input runat="server" type="button" class="wm_button" name="server_connection" id="server_connection"
				value="Test Server Connection" style="FLOAT: left; WIDTH: 200px" onserverclick="server_connection_ServerClick"> <input runat="server" type="button" id="save" name="save" class="wm_button" value="Save"
				style="WIDTH: 100px" onserverclick="save_ServerClick">&nbsp;
		</td>
	</tr>
</table>
