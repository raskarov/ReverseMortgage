<%@ Register Src="Copyright.ascx" TagPrefix="BaseWebmail" TagName="Copyright" %>
<%@ Register Src="Logo.ascx" TagPrefix="BaseWebmail" TagName="Logo" %>
<%@ Register Src="classic\Information.ascx" TagPrefix="BaseWebmail" TagName="Information" %>
<%@ Register Src="classic\Toolbar.ascx" TagPrefix="BaseWebmail" TagName="Toolbar" %>
<%@ Register Src="classic\AccountsList.ascx" TagPrefix="BaseWebmail" TagName="AccountsList" %>
<%@ Register Src="classic\Contacts.ascx" TagPrefix="BaseWebmail" TagName="Contacts" %>
<%@ Page language="c#" Codebehind="basewebmail.aspx.cs" EnableEventValidation="false" AutoEventWireup="True" validateRequest="false" Inherits="WebMailPro.basewebmail"%>
<%@ Import namespace="WebMailPro.classic" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<html id="html">
	<head>
		<title>
			<%=defaultTitle%>
		</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link rel="stylesheet" href="skins/<%=defaultSkin%>/styles.css" type="text/css" id="skin">
<% if (checkAtL == "1")
   { %>
	<script type="text/javascript" src="langs_js.aspx"></script>
	<script type="text/javascript" src="_defines.js"></script>
	<script type="text/javascript" src="_functions.js"></script>
	<script type="text/javascript" src="class.common.js"></script>
	<script type="text/javascript">
		var checkMail;
		var WebMailUrl = 'basewebmail.aspx<%=parameters%>';
		var LoginUrl = 'default.aspx';
		var CheckMailUrl = 'check-mail.aspx';
		var EmptyHtmlUrl = 'empty.html';
		var Browser = new CBrowser();

		function Init()
		{
			checkMail = new CCheckMail(1);
			checkMail.Start();
		}
		
		function SetCheckingAccountHandler(accountName)
		{
			checkMail.SetAccount(accountName);
		}
		
		function SetStateTextHandler(text) {
			checkMail.SetText(text);
		}
		
		function SetCheckingFolderHandler(folder, count) {
			checkMail.SetFolder(folder, count);
		}
		
		function SetRetrievingMessageHandler(number) {
			checkMail.SetMsgNumber(number);
		}
		
		function SetDeletingMessageHandler(number) {
			checkMail.DeleteMsg(number);
		}
		
		function EndCheckMailHandler(error) {
			if (error == 'session_error') {
				document.location = LoginUrl + '?error=1';
			} else {
				document.location = WebMailUrl;
			}
		}
		
		function CheckEndCheckMailHandler() {
			if (checkMail.started) {
				document.location = WebMailUrl;
			}
		}
	</script>
	<script type="text/javascript">
	function CloseEmail(){
alert('trying to close');	
	    window.parent.BackToFollowUp();
    }
	</script>
</head>
<body onload="Init();">
<div align="center" id="content" class="wm_content">
	<div class="wm_logo" id="logo" tabindex="-1" onfocus="this.blur();"></div>
</div>
<div class="wm_copyright" id="copyright">
	Powered by <a href="http://www.afterlogic.com/mailbee/webmail-pro.asp" target="_blank"> MailBee WebMail</a><br/>
	Copyright &copy; 2002-2008 <a href="http://www.afterlogic.com" target="_blank">AfterLogic Corporation</a>
</div>
</body>
<% }
   else
   { %>
		<%=js.ToHtml()%>
	</head>
	<body onresize="ResizeElements('all');" onclick="<%=strBodyOnClick%>">
		<table class="wm_hide" id="ps_container">
			<tr>
				<td class="wm_inbox_page_switcher_left"></td>
				<td class="wm_inbox_page_switcher_pages" id="ps_pages"></td>
				<td class="wm_inbox_page_switcher_right"></td>
			</tr>
		</table>
		<form id="Form1" method="post" runat="server">
			<INPUT type="hidden" runat="server" id="HFAction" NAME="HFAction">
			<INPUT type="hidden" runat="server" id="HFRequest" NAME="HFRequest">
			<INPUT type="hidden" runat="server" id="HFValue" NAME="HFValue">
			<INPUT type="hidden" runat="server" id="HFPageInfo" NAME="HFPageInfo">
			<BaseWebmail:Information id="Control_Information" runat="server" Skin=<%#defaultSkin%>></BaseWebmail:Information>
			<div align="center" class="wm_content" id="content">
				<BaseWebmail:Logo id="Control_Logo" runat="server"></BaseWebmail:Logo>
				<BaseWebmail:AccountsList id="Control_AccountsList" runat="server" Skin=<%#defaultSkin%> WebMailMode="base" ></BaseWebmail:AccountsList>
				<asp:PlaceHolder ID="PlaceHolderToolbar" Runat="server">
					<BaseWebmail:Toolbar id="Control_Toolbar" runat="server" Skin=<%#defaultSkin%>></BaseWebmail:Toolbar>
				</asp:PlaceHolder>
				<asp:PlaceHolder ID="PlaceHolder" Runat="server">
				</asp:PlaceHolder>
			</div>
			<asp:PlaceHolder ID="PlaceHolderCopyright" Runat="server">				
			</asp:PlaceHolder>
			<div style="display:none">
				<asp:LinkButton ID="PostBackButton" runat="server">PostBackButton</asp:LinkButton>
				<asp:LinkButton ID="PagerButton" runat="server">PagerButton</asp:LinkButton>
			</div>
			<asp:Label ID="InjectScript" runat="server"></asp:Label>
		</form>
		<%=js.InitTextToHtml()%>
		<iframe name="session_saver" id="session_saver" src="session_saver.aspx" class="wm_hide"></iframe>
	</body>
<% } %>
</html>