<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginVendor.aspx.cs" Inherits="LoanStarPortal.LoginVendor" EnableTheming="true" Theme="Default" EnableViewStateMac="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Vendor</title>
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
                <div style="border:solid 1px black;width:300px;padding-bottom:30px;margin-top:30px">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" id="logintable">
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="errMessage" runat="server" SkinID="ErrLabel"></asp:Label>
                        </td>
                    </tr>                
                    <tr>
                        <td style="text-align:right;padding-right:5px;width:80px;">
                            <asp:Label ID="Label1" runat="server" Text="Login:" SkinID="LoginLabel"></asp:Label>
                        </td>
                        <td style="width:150px;">
                            <asp:TextBox ID="tbLogin" runat="server" SkinID="LoginInput" MaxLength="256"></asp:TextBox>                       
                        </td>
                        <td align="left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbLogin" ErrorMessage="Login field can't be empty">*</asp:RequiredFieldValidator>
                       </td>
                    </tr>
                    <tr>
                       <td style="text-align:right;padding-right:5px">
                           <asp:Label ID="Label2" runat="server" Text="Password:" SkinID="LoginLabel"></asp:Label>
                       </td>
                       <td>
                            <asp:TextBox ID="tbPassword" runat="server" SkinID="LoginInput" MaxLength="16" TextMode="Password"></asp:TextBox>                       
                       </td>
                       <td align="left"><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbPassword">*</asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                       <td>&nbsp;</td>
                       <td>
                           <asp:Button ID="Submit" runat="server" Text="Log in" SkinID="LoginButton" OnClick="Submit_Click"/>
                       </td>
                       <td>&nbsp;</td>
                    </tr>                
    <!--                
                    <tr>
                       <td class="lefttd">&nbsp;</td>                
                       <td align="left"><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="ForgotPassword.aspx">Forgot password</asp:HyperLink></td>
                       <td >&nbsp;</td>
                    </tr>
    -->
                </table>
                </div>
            </td>
        </tr>    
        <tr style="height:17px; background-color:#E9EEEE"><td align="center">Footer</td></tr>
        </table>
    </form>
</body>
</html>
