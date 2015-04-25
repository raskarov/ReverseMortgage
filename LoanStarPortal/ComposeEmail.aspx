<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComposeEmail.aspx.cs" Inherits="LoanStarPortal.ComposeEmail" EnableViewStateMac="false" %>

<%@ Register Src="Controls/EmailAdd.ascx" TagName="EmailAdd" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"  style="height:100%">
<head id="Head1" runat="server">  
    <title>RMEngage</title>  
<link type="text/css" rel="stylesheet" href="App_Themes/Theme/default.css" />
</head>  

<body>
    <form id="form1" runat="server">
    <div>
        <uc1:EmailAdd ID="EmailAdd1" runat="server" />    
    </div>
    </form>
</body>
</html>
