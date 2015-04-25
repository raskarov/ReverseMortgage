<%@ Register Src="Copyright.ascx" TagPrefix="BaseWebmail" TagName="Copyright" %>
<%@ Register Src="Logo.ascx" TagPrefix="BaseWebmail" TagName="Logo" %>
<%@ Page language="c#" CodeBehind="default.aspx.cs" AutoEventWireup="True" Inherits="WebMailPro._default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="Content-Script-Type" content="text/javascript" />
	<meta http-equiv="Cache-Control" content="private,max-age=1209600" />
	<title><%=settings.SiteName%></title>
	<link rel="stylesheet" href="skins/<%=defaultSkin%>/styles.css" type="text/css" id="skin" />
	<script type="text/javascript">
		var isAjax = <%=defaultIsAjax%>;
		var WebMailUrl = 'webmail.aspx';
		var LoginUrl = 'default.aspx';
		var ActionUrl = 'xml_processing.aspx';
		var Title = '<%=defaultTitle%>';
		var SkinName = '<%=defaultSkin%>';
		var HideLoginMode = <%=defaultHideLoginMode%>;
		var DomainOptional = '<%=defaultDomainOptional%>';
		var AllowAdvancedLogin = <%=defaultAllowAdvancedLogin%>;
		var AdvancedLogin = '<%=advancedLogin%>';
		var EmptyHtmlUrl = 'empty.html';
		var CheckMailUrl = 'check-mail.aspx';
	</script>
	<script type="text/javascript" src="langs_js.aspx?lang=<%=defaultLang%>"></script>
	<script type="text/javascript" src="_defines.js"></script>
	<script type="text/javascript" src="class.common.js"></script>
	<script type="text/javascript" src="_functions.js"></script>
	<script type="text/javascript" src="class.login.js"></script>
	
</head>

<body onload="Init();">

<table class="wm_hide" id="info">
	<tr>
		<td class="wm_info_message" id="info_message"></td>
	</tr>
</table>
<div align="center" id="content" class="wm_content">
	<BaseWebmail:Logo id="Control_Logo" runat="server"></BaseWebmail:Logo>
	<div id="login_screen">
		<div class="<%=errorClass%>" id="login_error"><%=errorDesc%></div>
		<form action="default.aspx?mode=submit" method="post" id="login_form" name="login_form">
			<input type="hidden" name="advanced_login" value="<%=advancedLogin%>" />
		<table class="wm_login" id="login_table" border="0" cellspacing="0" cellpadding="0">
			<tr>
				<td class="wm_login_header" colspan="5"><%=_resMan.GetString("LANG_LoginInfo")%></td>
			</tr>
			<tr id="email_cont"<%=emailClass%>>
				<td class="wm_title"><%=_resMan.GetString("LANG_Email")%>:</td>
				<td colspan="4">
					<input class="wm_input" type="text" id="email" name="email" value="" maxlength="255" 
						onfocus="this.className = 'wm_input_focus';" onblur="this.className = 'wm_input';" />
				</td>
			</tr>
			<tr id="login_cont"<%=loginClass%>>
				<td class="wm_title"><%=_resMan.GetString("LANG_Login")%>:</td>
				<td colspan="4" id="login_parent"><nobr>
					<input class="wm_input" type="text" id="login" name="login" value="" maxlength="255" style="width:<%=loginWidth%>;" 
						onfocus="this.className = 'wm_input_focus';" onblur="this.className = 'wm_input';" />
					<span id="domain"><%=domainContent%></span>
				</nobr></td>
			</tr>
			<tr>
				<td class="wm_title"><%=_resMan.GetString("LANG_Password")%>:</td>
				<td colspan="4">
					<input class="wm_input wm_password_input" type="password" id="password" name="password" value="" maxlength="255" 
						onfocus="this.className = 'wm_input_focus wm_password_input';" onblur="this.className = 'wm_input wm_password_input';" />
				</td>
			</tr>
			<% if (settings.AllowAdvancedLogin) { %>
			<tr id="incoming"<%=advancedClass%>>
				<td class="wm_title"><%=_resMan.GetString("LANG_IncServer")%>:</td>
				<td>
					<input class="wm_advanced_input" type="text" value="<%=defaultIncServer%>" id="inc_server" name="inc_server" maxlength="255"
						onfocus="this.className = 'wm_advanced_input_focus';" onblur="this.className = 'wm_advanced_input';" />
				</td>
				<td>
					<select class="wm_advanced_input" id="inc_protocol" name="inc_protocol" 
						onfocus="this.className = 'wm_advanced_input_focus';" onblur="this.className = 'wm_advanced_input';">
						<option value="<%=POP3_PROTOCOL%>" <%=pop3Selected%>><%=_resMan.GetString("LANG_PopProtocol")%></option>
						<option value="<%=IMAP4_PROTOCOL%>" <%=imap4Selected%>><%=_resMan.GetString("LANG_ImapProtocol")%></option>
					</select>
				</td>
				<td class="wm_title"><%=_resMan.GetString("LANG_IncPort")%>:</td>
				<td>
					<input class="wm_advanced_input" type="text" value="<%=defaultIncPort%>" id="inc_port" name="inc_port" maxlength="5"
						onfocus="this.className = 'wm_advanced_input_focus';" onblur="this.className = 'wm_advanced_input';" />
				</td>
			</tr>
			<tr id="outgoing"<%=advancedClass%>>
				<td class="wm_title"><%=_resMan.GetString("LANG_OutServer")%>:</td>
				<td colspan="2">
					<input class="wm_advanced_input" type="text" value="<%=defaultOutServer%>" id="out_server" name="out_server" maxlength="255"
					onfocus="this.className = 'wm_advanced_input_focus';" onblur="this.className = 'wm_advanced_input';" />
				</td>
				<td class="wm_title"><%=_resMan.GetString("LANG_OutPort")%>:</td>
				<td>
					<input class="wm_advanced_input" type="text" value="<%=defaultOutPort%>" id="out_port" name="out_port" maxlength="5"
						onfocus="this.className = 'wm_advanced_input_focus';" onblur="this.className = 'wm_advanced_input';" />
				</td>
			</tr>
			<tr id="authentication"<%=advancedClass%>>
				<td colspan="5">
					<input class="wm_checkbox" type="checkbox" value="1" id="smtp_auth" name="smtp_auth"<%=smtpAuthChecked%>>
					<label for="smtp_auth"><%=_resMan.GetString("LANG_UseSmtpAuth")%></label>
				</td>
			</tr>
			<% } %>
			<tr>
				<td colspan="5">
					<input class="wm_checkbox" type="checkbox" value="1" id="sign_me" name="sign_me"<%=signMeChecked%>>
					<label for="sign_me"><%=_resMan.GetString("LANG_SignMe")%></label>
				</td>
			</tr>
			<tr>
				<td colspan="5">
				<% if (defaultAllowAdvancedLogin == "true") { %>
					<span class="wm_login_switcher">
						<a class="wm_reg" href="<%=switcherHref%>" id="login_mode_switcher"><%=switcherText%></a>
					</span>
				<% } %>
					<span class="wm_login_button">
						<input class="wm_button" type="submit" id="submit" name="submit" value="<%=_resMan.GetString("LANG_Enter")%>" />
					</span>
				</td>
			</tr>
		</table>
		</form>
		
	</div>
</div>
<BaseWebmail:Copyright id="Control_Copyright" runat="server"></BaseWebmail:Copyright>
</body>
</html>
