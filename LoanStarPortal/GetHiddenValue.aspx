<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetHiddenValue.aspx.cs" Inherits="LoanStarPortal.GetHiddenValue" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>RM LOS</title>
    <link type="text/css" rel="stylesheet" href="App_Themes/Theme/default.css" />    
</head>
<body class="nmbody" style="border:none;" >
    <form id="form1" runat="server" defaultbutton="btnOK">
<script type="text/javascript">
function GetRadWindow(){
    var o=null;
    if (window.radWindow) o = window.radWindow; 
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }
function CancelEdit(){
    GetRadWindow().Close();		
}
function CloseAndRebind(id,val,msg){
    GetRadWindow().Close();
    GetRadWindow().BrowserWindow.ShowHiddenValue(id,val,msg);
}
</script>        
    <asp:Label ID="InjectScript" runat="server"></asp:Label>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="padding-top:10px">
            <td align="center">To view this field please enter your password</td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="width:100px">&nbsp;</td>
            <td style="width:200px"><asp:Label ID="lblMessage" runat="server" Text="" ForeColor="red"></asp:Label></td>
        </tr>
        <tr>
            <td style="width:100px;text-align:right"><asp:Label ID="Label1" runat="server" Text="Password:"></asp:Label></td>
            <td style="width:200px;padding-left:5px;" >
                <asp:TextBox ID="tbPassword" runat="server" MaxLength="16" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="padding-top:10px;">
            <td style="width:50px">&nbsp;</td>
            <td><asp:Button ID="btnOK" runat="server" Text="Ok" OnClick="btnOK_Click" Width="60px" /></td>
            <td><asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"  Width="60px"/></td>
        </tr>
    </table>    
    </form>
</body>
</html>
