<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="LoanStarPortal.ForgotPassword" EnableTheming="true" Theme="Default" EnableViewStateMac="false" %>

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
            <asp:Image ID="Image1"  ImageUrl="~/Images/logo.gif" AlternateText="" width="235px" height="35px" runat="server" />
        </td>
    </tr>
    <tr style="height:490px; vertical-align:top">
        <td align="center">
            <div style="border:solid 1px black;width:300px;padding-bottom:30px;margin-top:30px">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" id="logintable">
                <tr>
                   <td style="width:80px">&nbsp;</td>
                   <td align="left" colspan="2">
                        <asp:Label ID="errMessage" runat="server" SkinID="LoginLabel" Text="Please enter your email"></asp:Label>
                   </td>
                </tr>                
                <tr>
                   <td style="width:80px;padding-right:5px;text-align:right">
                       <asp:Label ID="Label1" runat="server" Text="Email:" SkinID="LoginLabel"></asp:Label>
                   </td>
                   <td style="width:150px;">
                        <asp:TextBox ID="tbLogin" runat="server" MaxLength="256" ></asp:TextBox>                      
                   </td>
                    <td>&nbsp;</td>                        
                    </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Email not valid" ValidationExpression="\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)" ControlToValidate="tbLogin"></asp:RegularExpressionValidator></td>
                    <td>&nbsp;</td>
                </tr>                    
                <tr>                
                   <td>&nbsp;</td>
                   <td align="center">
                       <asp:Button ID="Submit" runat="server" Text="Send" SkinID="LoginButton" OnClick="Submit_Click"/>
                   </td>
                   <td >&nbsp;</td>                   
                </tr>                
            </table>
            </div>
        </td>
    </tr>    
    <tr style="height:17px; background-color:#E9EEEE"><td align="center">Footer</td></tr>    
    </table>
    </form>
</body>
</html>
