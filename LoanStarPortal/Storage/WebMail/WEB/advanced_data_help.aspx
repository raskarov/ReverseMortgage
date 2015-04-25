<%@ Page language="c#" Codebehind="advanced_data_help.aspx.cs" AutoEventWireup="True" Inherits="WebMailPro.advanced_data_help" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<html>
  <head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="Content-Script-Type" content="text/javascript" />
	<meta http-equiv="Cache-Control" content="private,max-age=1209600" />
	<title><%=_resMan.GetString("AdvancedDateHelpTitle")%></title>
  </head>
<body style="background-color:#ffffff; font: normal 13px Tahoma, Arial, Helvetica, sans-serif; margin:10px;">
<br />
<b><%=_resMan.GetString("AdvancedDateHelpTitle")%></b>
<br />
<br />
<%=_resMan.GetString("AdvancedDateHelpIntro")%><br />
<br />
<table style="font: normal 13px Tahoma, Arial, Helvetica, sans-serif;">
	<tr>
		<td><b>dd</b></td>
		<td><%=_resMan.GetString("AdvancedDateHelpDayOfMonth")%> </td>
	</tr>
	<tr>
		<td><b>mm</b></td>
		<td><%=_resMan.GetString("AdvancedDateHelpNumericMonth")%></td>
	</tr>
	<tr>
		<td><b>month</b></td>
		<td><%=_resMan.GetString("AdvancedDateHelpTextualMonth")%></td>
	</tr>
	<tr>
		<td><b>yy</b></td>
		<td><%=_resMan.GetString("AdvancedDateHelpYear2")%></td>
	</tr>
	<tr>
		<td><b>yyyy</b></td>
		<td><%=_resMan.GetString("AdvancedDateHelpYear4")%></td>
	</tr>
	<tr>
		<td><b>y</b></td>
		<td><%=_resMan.GetString("AdvancedDateHelpDayOfYear")%> </td>
	</tr>
	<tr>
		<td><b>q</b></td>
		<td><%=_resMan.GetString("AdvancedDateHelpQuarter")%></td>
	</tr>
	<tr>
		<td><b>w</b></td>
		<td><%=_resMan.GetString("AdvancedDateHelpDayOfWeek")%></td>
	</tr>
	<tr>
		<td><b>ww</b></td>
		<td><%=_resMan.GetString("AdvancedDateHelpWeekOfYear")%></td>
	</tr>
</table>
<br />
<%=_resMan.GetString("AdvancedDateHelpConclusion")%>
</body>
</html>
