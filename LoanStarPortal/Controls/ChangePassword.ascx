<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.ascx.cs" Inherits="LoanStarPortal.Controls.ChangePassword" %>
<table width="100%" cellpadding="0" cellspacing="0" border="0" >
    <tr style="height:40px">
        <td colspan="3" align="left" style="padding-left:20px;"><asp:Label ID="lblPasswordRules" runat="server" Text="Password must be at least 6 character long, start with letter and must consist of at least 1 digit" Font-Bold="true"></asp:Label></td>
    </tr>            
</table>
<table width="100%" cellpadding="0" cellspacing="0" border="0" >
    <tr style="height:20px;">
        <td class="mpplbl">&nbsp;</td>
        <td colspan="2" align="left"><asp:Label ID="errMessage" runat="server" SkinID="ErrLabel"></asp:Label></td>
    </tr>
    <tr>
        <td class="mpplbl">&nbsp;</td>
        <td colspan="2" align="left"><asp:Label ID="lblLastUpdate" runat="server" ></asp:Label></td>
    </tr>
    <tr>
        <td class="mpplbl">&nbsp;</td>
        <td colspan="2" align="left"><asp:Label ID="lblInfo" runat="server" Text="Note: You can change password only once per day" Font-Bold="true"></asp:Label></td>
    </tr>
    <tr style="padding-top:10px;">
        <td class="mpplbl" align="right"  style="padding-right:5px;"><asp:Label ID="Label1" runat="server" Text="New Password:" SkinID="LoginLabel"></asp:Label></td>
        <td align="left">
            <asp:TextBox ID="tbPassword" runat="server" MaxLength="256" TextMode="Password" Width="140px"></asp:TextBox>&nbsp;
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbPassword" ErrorMessage="*" ValidationGroup="Password"></asp:RequiredFieldValidator>
         </td>
         <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="mpplbl" align="right" style="padding-right:5px;"><asp:Label ID="Label2" runat="server" Text="Confirm Password:" SkinID="LoginLabel"></asp:Label></td>
        <td align="left">
            <asp:TextBox ID="tbConfirmPassword" runat="server" MaxLength="16" TextMode="Password" Width="140px"></asp:TextBox>&nbsp;
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbConfirmPassword" ErrorMessage="*" ValidationGroup="Password"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="mpplbl" align="right" style="padding-right:5px;"><asp:Label ID="Label3" runat="server" Text="Old Password:" SkinID="LoginLabel"></asp:Label></td>
        <td align="left">
            <asp:TextBox ID="tbOldPassword" runat="server" MaxLength="16" TextMode="Password" Width="140px"></asp:TextBox>&nbsp;
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbOldPassword" ErrorMessage="*" ValidationGroup="Password"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>                
    <tr style="padding-top:20px">
        <td class="mpplbl">&nbsp;</td>
        <td align="left"><asp:Button ID="btnSetPassword" runat="server" Text="Set password" OnClick="btnSetPassword_Click" ValidationGroup="Password"/></td>
        <td>&nbsp;</td>
    </tr>                
</table>
