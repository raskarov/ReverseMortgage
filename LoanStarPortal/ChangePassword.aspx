<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="LoanStarPortal.ChangePassword" Theme="Default" %>

<%@ Register Src="Controls/ChangePassword.ascx" TagName="ChangePassword" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>RM LOS</title>
</head>
<body>
<form id="form1" runat="server">
<table border="0" width="100%" cellpadding="0" cellspacing="0">
    <tr style="height:100px">
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td align="center">
        </td>
    </tr>
    <tr style="height:490px; vertical-align:top;">
        <td align="center">
            <div style="border:solid 1px black;width:600px;padding-bottom:30px;margin-top:30px">
                <uc1:ChangePassword ID="ChangePassword2" runat="server" OnPasswordUpdated="PasswordUpdated" />
            </div>
        </td>
    </tr>    
    </table>    
</form>
</body>
</html>
