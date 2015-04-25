<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorOrder.aspx.cs" Inherits="LoanStarPortal.VendorOrder" %>

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
            <tr style="padding-top:60px">
                <td align="center" style="font-size:large">Thank you. Please download your order by clicking the link below.</td>
            </tr> 
            <tr style="padding-top:40px">
                <td align="center">
                    <asp:LinkButton ID="lbDownLoad" runat="server" OnClick="lbDownLoad_Click">Download order</asp:LinkButton>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
