<%@ Page language="c#" Codebehind="updateDB.aspx.cs" AutoEventWireup="True" Inherits="WebMailPro.updateDB" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title></title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<p runat="server" ID="P1" NAME="P1"><%=sb.ToString()%></p>
		<form id="Form1" method="post" runat="server">
			<hr />
			<asp:Label ID="outputLabel" Runat="server" Font-Size="Large"></asp:Label>
			<br />
		</form>
	</body>
</HTML>
