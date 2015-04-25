<%@ Control Language="c#" AutoEventWireup="True" Codebehind="mailadm_login.ascx.cs" Inherits="WebMailPro.MailAdm.mailadm_login" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table class="wm_login">
		<tr>
			<td class="wm_login_header" colspan="2">Administration Login</td>
		</tr>
		<tr>
			<td class="wm_title">Login:</td>
			<td>
				<input runat="server" class="wm_input" size="27" type="text" id="login" name="login" onfocus="this.style.background = '#FFF9B2';"
					onblur="this.style.background = 'white';">
			</td>
		</tr>
		<tr>
			<td class="wm_title">Password:</td>
			<td>
				<input runat="server" class="wm_input" type="password" size="27" id="password" name="password"
					onfocus="this.style.background = '#FFF9B2';" onblur="this.style.background = 'white';">
			</td>
		</tr>
		<tr>
			<td colspan="2" align="right">
				<span class="wm_login_button">
					<asp:Button Runat="server" cssclass="wm_button" ID="enter" Text="Login" onclick="enter_Click" />
				</span>
			</td>
		</tr>
</table>
