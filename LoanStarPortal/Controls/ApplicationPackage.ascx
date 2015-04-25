<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationPackage.ascx.cs" Inherits="LoanStarPortal.Controls.ApplicationPackage" %>
<div class="paneTitle"><b>Application Package</b></div>
<table border="0" cellspacing="3" cellpadding="3" width="100%">
    <tr>
        <td colspan="2" class="paneTableHeader"> Standard</td>
    </tr>
    <tr>
        <td><asp:CheckBox ID="CheckBox1" runat="server" Text="log application"/></td>
        <td><asp:Label ID="Label1" runat="server" Text="2"></asp:Label></td>
    </tr>
</table>
<table border="0" cellspacing="3" cellpadding="3" width="100%">
    <tr>
        <td colspan="2" class="paneTableHeader"> State Specific</td>
    </tr>
    <tr>
        <td><asp:CheckBox ID="CheckBox2" runat="server" Text="log application"/></td>
        <td><asp:Label ID="Label2" runat="server" Text="2"></asp:Label></td>
    </tr>
</table>
<table border="0" cellspacing="3" cellpadding="3" width="100%">
    <tr>
        <td colspan="2" class="paneTableHeader"> Lender Specific</td>
    </tr>
    <tr>
        <td><asp:CheckBox ID="CheckBox3" runat="server" Text="log application"/></td>
        <td><asp:Label ID="Label3" runat="server" Text="2"></asp:Label></td>
    </tr>
</table>
<table border="0" cellspacing="3" cellpadding="3" width="100%">
    <tr>
        <td colspan="2" class="paneTableHeader"> Misc</td>
    </tr>
    <tr>
        <td><asp:CheckBox ID="CheckBox4" runat="server" Text="log application"/></td>
        <td><asp:Label ID="Label4" runat="server" Text="2"></asp:Label></td>
    </tr>
</table>
<div align="center"><asp:Button ID="btnPrint" runat="server" Text="Print Package" /></div>