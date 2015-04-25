<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="LoanStarPortal.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link type="text/css" rel="stylesheet" href="App_Themes/Theme/default.css" />
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="width:30%;padding-right:5px" align="right">
                <asp:Label ID="Label1" runat="server" Text="MortgageId:"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="tbMortgageId" runat="server"></asp:TextBox>                
            </td>            
        </tr>    
        <tr>
            <td style="width:30%px;padding-right:5px">&nbsp;</td>
            <td >
                <asp:Label ID="lblResult" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width:30%;padding-right:5px">&nbsp;</td>
            <td>
                <asp:Label ID="lblErr" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>        
        <tr>
            <td style="width:30%;padding-right:5px">&nbsp;</td>
            <td ><asp:Button ID="btnMap" runat="server" Text="Map" OnClick="btnMap_Click" /></td>
        </tr>
    </table> 
        <asp:TextBox ID="TextBox1" runat="server" TextMode="Password"></asp:TextBox>       
    </form>
</body>
</html>
