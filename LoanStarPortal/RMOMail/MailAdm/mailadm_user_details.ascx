<%@ Control Language="c#" AutoEventWireup="True" Codebehind="mailadm_user_details.ascx.cs" Inherits="WebMailPro.MailAdm.mailadm_user_details" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type="text/javascript">
function DisableAllSyncControls()
{
	document.getElementById('userDetailsID_radioDelRecvMsgs').disabled = true;
	document.getElementById('userDetailsID_radioLeaveMsgs').disabled = true;
	document.getElementById('userDetailsID_chkKeepMsgs').disabled = true;
	document.getElementById('userDetailsID_txtKeepMsgsDays').disabled = true;
	document.getElementById('userDetailsID_chkDelMsgsSrv').disabled = true;
	document.getElementById('userDetailsID_chkDelMsgsDB').disabled = true;
}

function DeleteReceivedMessagesClick()
{
	DisableAllSyncControls();
	document.getElementById('userDetailsID_radioDelRecvMsgs').disabled = false;
	document.getElementById('userDetailsID_chkDelMsgsDB').disabled = false;
}

function LeaveReceivedMessagesClick()
{
	DisableAllSyncControls();
	document.getElementById('userDetailsID_radioLeaveMsgs').disabled = false;
	document.getElementById('userDetailsID_chkKeepMsgs').disabled = false;
	ChangeKeepMessages();
	document.getElementById('userDetailsID_chkDelMsgsSrv').disabled = false;
	document.getElementById('userDetailsID_chkDelMsgsDB').disabled = false;
}

function hideTR()
{
	if (!isHide)
	{
		for (i = 0; i < 5; i++)
		{
			document.getElementById('userDetailsID_tr_' + i).className = '';
		}
		//isHide = false;
	}
	else
	{
		for (i = 0; i < 5; i++)
		{
			document.getElementById('userDetailsID_tr_' + i).className = 'wm_hide';
		}
		//isHide = true;
	}

}

function ChangeKeepMessages()
{
	if (document.getElementById('userDetailsID_chkKeepMsgs').checked)
	{
		document.getElementById('userDetailsID_txtKeepMsgsDays').disabled = false;
	}
	else
	{
		document.getElementById('userDetailsID_txtKeepMsgsDays').disabled = true;
	}
}

function ChangeSyncType()
{
	DisableAllSyncControls();
	switch (document.getElementById('userDetailsID_synchronizeSelect').value) {
		case '1':
		// headers only
			document.getElementById('userDetailsID_radioLeaveMsgs').disabled = false;
			document.getElementById('userDetailsID_radioLeaveMsgs').checked = true;
			document.getElementById('userDetailsID_chkKeepMsgs').disabled = false;
			document.getElementById('userDetailsID_txtKeepMsgsDays').disabled = false;
			document.getElementById('userDetailsID_chkDelMsgsSrv').disabled = false;
			document.getElementById('userDetailsID_chkDelMsgsDB').disabled = false;
		break;
		case '2':
		// entire messages
			document.getElementById('userDetailsID_radioDelRecvMsgs').disabled = false;
			document.getElementById('userDetailsID_radioLeaveMsgs').disabled = false;
			document.getElementById('userDetailsID_chkKeepMsgs').disabled = false;
			document.getElementById('userDetailsID_txtKeepMsgsDays').disabled = false;
			document.getElementById('userDetailsID_chkDelMsgsSrv').disabled = false;
			document.getElementById('userDetailsID_chkDelMsgsDB').disabled = false;
		break;
	}
}

function ChangeProtocol()
{
	if (document.getElementById('userDetailsID_selectProtocol').value == 'IMAP4')
	{
		isHide = true;
		document.getElementById('userDetailsID_txtIncomingPort').value = '143';
	}
	else
	{
		isHide = false;
		document.getElementById('userDetailsID_txtIncomingPort').value = '110';
	}
	hideTR();
}

function Run()
{
	isHide = false;
	ChangeKeepMessages();
	if (document.getElementById('userDetailsID_radioDelRecvMsgs').checked)
	{
		DeleteReceivedMessagesClick();
	}
	else if (document.getElementById('userDetailsID_radioLeaveMsgs').checked)
	{
		LeaveReceivedMessagesClick();
	}
	ChangeSyncType();
	//ChangeProtocol();
}
</script>
<table class="wm_admin_center" width="500" border="0">
	<tr>
		<td width="140"></td>
		<td width="240"></td>
		<td></td>
	</tr>
	<tr>
		<td class="wm_admin_title" colSpan="3">Users Details</td>
	</tr>
	<tr>
		<td align="center" colSpan="3"><br>
			<asp:DataGrid GridLines="Horizontal" ID="accountsGrid" Runat="server" style="margin-bottom: 0px; width: 420px;" CssClass="wm_settings_list" Visible="False" AutoGenerateColumns="False" ShowHeader="False">
				<HeaderStyle HorizontalAlign="Center" Font-Bold="True" BorderStyle="Solid" ForeColor="Black" BackColor="Gainsboro"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="Left">
						<ItemTemplate><%#DataBinder.Eval(Container.DataItem, "email")%></ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn ItemStyle-Width="20px">
						<ItemTemplate>
							&nbsp;&nbsp;<a href="?mode=edit_user&uid=<%#DataBinder.Eval(Container.DataItem, "id_acct")%>">Edit</a>&nbsp;&nbsp;<a href="?mode=delete_user&uid=<%#DataBinder.Eval(Container.DataItem, "id_acct")%>" onclick="return confirm('Delete Account?');">Delete</a>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:DataGrid>
		</td>
	</tr>
	<tr>
		<td colSpan="3"><br>
		</td>
	</tr>
	<tr>
		<td align="right">Mailbox limit:
		</td>
		<td align="left" colSpan="2"><input class="wm_input" id="txtLimitMailbox" type="text" size="13" Runat="server">
			bytes
		</td>
	</tr>
	<tr>
		<td align="right">Name:
		</td>
		<td align="left" colSpan="2"><input class="wm_input" id="txtFriendlyName" style="WIDTH: 350px" type="text" maxLength="100"
				Runat="server">
		</td>
	</tr>
	<tr>
		<td align="right">* Email:
		</td>
		<td align="left" colSpan="2"><input class="wm_input" id="txtEmail" style="WIDTH: 350px" type="text" maxLength="100"
				Runat="server">
			<asp:requiredfieldvalidator id="emailValid" Runat="server" Display="Static" ErrorMessage="!" ControlToValidate="txtEmail"></asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td align="right">* Incoming mail:
		</td>
		<td align="left"><input class="wm_input" id="txtIncomingServer" type="text" maxLength="100" Runat="server">
			<asp:requiredfieldvalidator id="incMailValidator" Runat="server" Display="Dynamic" ErrorMessage="!" ControlToValidate="txtIncomingServer"></asp:requiredfieldvalidator>&nbsp;&nbsp;&nbsp;<asp:label id="labelIncomingProtocol" Runat="server"></asp:label>
			<asp:dropdownlist id="selectProtocol" Runat="server" onchange="ChangeProtocol(this);" CssClass="wm_hide">
				<asp:ListItem Value="POP3">POP3</asp:ListItem>
				<asp:ListItem Value="IMAP4">IMAP4</asp:ListItem>
				<asp:ListItem Value="XMail">XMail</asp:ListItem>
			</asp:dropdownlist></td>
		<td align="right">* Port:&nbsp;&nbsp;&nbsp;&nbsp; <input class="wm_input" id="txtIncomingPort" type="text" maxLength="4" size="3" Runat="server">
			<asp:requiredfieldvalidator id="incPortValidator" Runat="server" Display="Dynamic" ErrorMessage="!" ControlToValidate="txtIncomingPort"></asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td align="right">* Login:
		</td>
		<td align="left" colSpan="2"><input class="wm_input" id="txtIncomingLogin" style="WIDTH: 350px" type="text" maxLength="100"
				Runat="server">
			<asp:requiredfieldvalidator id="incLoginValidator" Runat="server" Display="Dynamic" ErrorMessage="!" ControlToValidate="txtIncomingLogin"></asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td align="right">* Password:
		</td>
		<td align="left" colSpan="2"><asp:textbox id="txtIncomingPassword" Runat="server" CssClass="wm_input" Width="350px" MaxLength="100"
				TextMode="Password"></asp:textbox><asp:requiredfieldvalidator id="incPaswValidator" Runat="server" Display="Dynamic" ErrorMessage="!" ControlToValidate="txtIncomingPassword"></asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td align="right">SMTP server:
		</td>
		<td align="left"><input class="wm_input" id="txtSmtpServer" type="text" maxLength="100" Runat="server">
		</td>
		<td align="right">* Port:&nbsp;&nbsp;&nbsp;&nbsp; <input class="wm_input" id="txtSmtpPort" type="text" maxLength="4" size="3" Runat="server">
			<asp:requiredfieldvalidator id="smtpPortValid" Runat="server" Display="Dynamic" ErrorMessage="!" ControlToValidate="txtSmtpPort"></asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td align="right">SMTP login:
		</td>
		<td align="left" colSpan="2"><input class="wm_input" id="txtSmtpLogin" style="WIDTH: 350px" type="text" maxLength="100"
				Runat="server">
		</td>
	</tr>
	<tr>
		<td align="right">SMTP password:
		</td>
		<td align="left" colSpan="2"><asp:textbox id="txtSmtpPassword" Runat="server" CssClass="wm_input" Width="350px" MaxLength="100"
				TextMode="Password"></asp:textbox></td>
	</tr>
	<tr>
		<td align="left" colSpan="3"><input class="wm_checkbox" id="chkUseSmtpAuth" style="VERTICAL-ALIGN: middle" type="checkbox"
				Runat="server"> <label for="userDetailsID_chkUseSmtpAuth">Use SMTP 
				authentication (You may leave SMTP login/password fields blank, if they're the 
				same as POP3 login/password)</label>
		</td>
	</tr>
	<tr>
		<td align="left" colSpan="3"><input class="wm_checkbox" id="chkUseFriendlyName" style="VERTICAL-ALIGN: middle" type="checkbox"
				Runat="server"> <label for="userDetailsID_chkUseFriendlyName">Use Friendly Name 
				in "From:" field (Your name &lt;sender@mail.com&gt;)</label>
		</td>
	</tr>
	<tr>
		<td align="left" colSpan="3"><input class="wm_checkbox" id="chkSyncFolders" style="VERTICAL-ALIGN: middle" type="checkbox"
				Runat="server"> <label for="userDetailsID_chkSyncFolders">Synchronize folders 
				at login</label>
		</td>
	</tr>
	<tr id="tr_0" runat="server">
		<td align="left" colSpan="3"><input id="radioDelRecvMsgs" style="VERTICAL-ALIGN: middle" type="radio" name="syncMode"
				Runat="server"> <label for="userDetailsID_radioDelRecvMsgs">Delete received 
				messages from server</label>
			<br>
			<input id="radioLeaveMsgs" style="VERTICAL-ALIGN: middle" type="radio" name="syncMode"
				Runat="server"> <label for="userDetailsID_radioLeaveMsgs">Leave messages on 
				server</label>
		</td>
	</tr>
	<tr id="tr_1" runat="server">
		<td align="left" colSpan="3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			<input class="wm_checkbox" id="chkKeepMsgs" style="VERTICAL-ALIGN: middle" onclick="ChangeKeepMessages();"
				type="checkbox" Runat="server"> <label for="userDetailsID_chkKeepMsgs">Keep 
				messages on server for</label> <input class="wm_input" id="txtKeepMsgsDays" type="text" size="1" Runat="server">
			day(s)
			<br>
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <input class="wm_checkbox" id="chkDelMsgsSrv" style="VERTICAL-ALIGN: middle" type="checkbox"
				Runat="server"> <label for="userDetailsID_chkDelMsgsSrv">Delete message from 
				server when it is removed from Trash</label>
		</td>
	</tr>
	<tr id="tr_2" runat="server">
		<td align="left" colSpan="3">Type of Inbox Synchronize:
			<asp:dropdownlist id="synchronizeSelect" Runat="server" onchange="ChangeSyncType();">
				<asp:ListItem Value="1">Headers Only</asp:ListItem>
				<asp:ListItem Value="2">Entire Messages</asp:ListItem>
				<asp:ListItem Value="3">Direct Mode</asp:ListItem>
			</asp:dropdownlist></td>
	</tr>
	<tr id="tr_3" runat="server">
		<td align="left" colSpan="3"><input class="wm_checkbox" id="chkDelMsgsDB" style="VERTICAL-ALIGN: middle" type="checkbox"
				Runat="server"> <label for="userDetailsID_chkDelMsgsDB">Delete message from 
				database if it no longer exists on mail server</label>
		</td>
	</tr>
	<tr id="tr_4" runat="server">
		<td align="left" colSpan="3"><input class="wm_checkbox" id="chkAllowDM" style="VERTICAL-ALIGN: middle" type="checkbox"
				Runat="server"> <label for="userDetailsID_chkAllowDM">Allow Direct Mode</label>
		</td>
	</tr>
	<tr>
		<td align="left" colSpan="3"><input class="wm_checkbox" id="chkAllowChangeEmail" style="VERTICAL-ALIGN: middle" type="checkbox"
				Runat="server"> <label for="userDetailsID_chkAllowChangeEmail">Allow user to 
				change email settings</label>
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
		<td colSpan="3">
			<hr SIZE="1">
		</td>
	</tr>
	<tr>
		<td><input class="wm_button" style="FONT-WEIGHT: bold; WIDTH: 100px" type="button"
				value="Cancel" onclick="document.location.replace('mailadm.aspx?mode=users')">
		</td>
		<td align="right" colSpan="2"><asp:button id="SubmitButton" Runat="server" Width="100px" CssClass="wm_button" Text="Save" onclick="SubmitButton_Click"></asp:button>&nbsp;
		</td>
	</tr>
</table>
<script type="text/javascript">
<!--
	Run();
//-->
</script>
