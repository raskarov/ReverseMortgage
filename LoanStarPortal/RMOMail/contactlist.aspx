<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contactlist.aspx.cs" Inherits="WebMailPro.contactlist" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>WebMail Pro</title>
    <script language="JavaScript" type="text/javascript" src="class.common.js"></script>
	<script language="JavaScript" type="text/javascript" src="./classic/class_contactsmini.js"></script>
	<link rel="stylesheet" href="skins/<%=_skin%>/styles.css" type="text/css"/>
	<script type="text/javascript">
function WriteEmails(str)
{
	if (window.opener && window.opener.WriteEmails) {
		window.opener.WriteEmails(str, <%=getInputParam()%>);
	}
	window.close();
}
	</script>
</head>
<body>
<div class="wm_inbox_lines" style="width: auto;">
<table id="list" style="width: 100%;">
	<%=getTableBuffer()%>
	<tr>
		<td colspan="2" align="center">
			<input type="button" value="<%=_resMan.GetString("OK")%>" onclick="WriteEmails(clist.getContactsAsString())"/>
		</td>
	</tr>
</table>
</div>
<script type="text/javascript">
var clist, Browser;
function init()
{
	Browser = new CBrowser();
	clist = new CContactsSelectionMini();
	clist.FillContacts();
	
	var outH = clist.list.offsetHeight + window.outerHeight - window.innerHeight;
	if (window.outerHeight == null || window.innerHeight == null) {
		outH = 400;
	}
	
	window.resizeTo(300, outH);
}
init();
</script>
</body>
</html>
