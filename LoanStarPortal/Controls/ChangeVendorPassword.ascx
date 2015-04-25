<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeVendorPassword.ascx.cs" Inherits="LoanStarPortal.Controls.ChangeVendorPassword" %>
<table border="0" cellpadding="0" cellspacing="0" style="width:80%;">
    <tr style="padding-top:20px;">
        <td align="center"><asp:Label ID="lblHeader" runat="server" Text="Change password" SkinID="VendorHeader"></asp:Label></td>
    </tr>
    <tr>
        <td align="center">
            <table border="0" cellpadding="0" cellspacing="0" style="width:80%;">
                <tr style="height:35px;">
                    <td class="tdvlbl"><asp:Label ID="Label1" runat="server" Text="New password:"></asp:Label></td>
                    <td class="tdvctl"><asp:TextBox ID="tbNewPassword" runat="server" MaxLength="16" Width="98%" TextMode="Password"></asp:TextBox></td>
                    <td align="left" style="padding-left:5px;"><asp:Label ID="lblErrPassword" runat="server" Text="" ForeColor="red"></asp:Label></td>
                </tr>
                <tr style="height:35px;">
                    <td class="tdvlbl"><asp:Label ID="Label2" runat="server" Text="Confirm passwors:"></asp:Label></td>
                    <td class="tdvctl"><asp:TextBox ID="tbConfirmPassword" runat="server" MaxLength="16" Width="98%"  TextMode="Password"></asp:TextBox></td>
                    <td align="left" style="padding-left:5px;"><asp:Label ID="lblErrConfirmPassword" runat="server" Text="" ForeColor="red"></asp:Label></td>
                </tr>
                <tr style="height:35px;">
                    <td class="tdvlbl"><asp:Label ID="Label3" runat="server" Text="Old password:"></asp:Label></td>
                    <td class="tdvctl"><asp:TextBox ID="tbOldPassword" runat="server" Width="98%" TextMode="Password"></asp:TextBox></td>
                    <td align="left" style="padding-left:5px;"><asp:Label ID="lblErrOldPassword" runat="server" Text="" ForeColor="red"></asp:Label></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
