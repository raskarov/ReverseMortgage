<%@ Page language="c#" Codebehind="upload.aspx.cs" AutoEventWireup="False" Inherits="WebMailPro.upload" validateRequest="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<html>
<head>
	<title></title>
</head>
<body>
<% if (errorOccured) { %>
	<script language="JavaScript" type="text/javascript">
		alert('<%=error%>');
	</script>
<% } else { %>
	<script language="JavaScript" type="text/javascript">
		parent.LoadAttachmentHandler({FileName: '<%=name%>', TempName: '<%=tmp_name%>', Size: <%=size%>, MimeType: '<%=mime_type%>'});
	</script>
<% } %>
</body>
</html>

