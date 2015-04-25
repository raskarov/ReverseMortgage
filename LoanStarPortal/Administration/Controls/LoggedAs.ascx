<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoggedAs.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.LoggedAs" %>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:Label ID="lblLoggedAs" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr runat="server" id="trlogout">
        <td>
            <asp:LinkButton ID="lbLogout" runat="server" OnClick="lbLogout_Click" CausesValidation="False">Return to global admin area</asp:LinkButton>
        </td>
    </tr>    
</table>
