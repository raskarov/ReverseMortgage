<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MortgageProfileContacts.ascx.cs" Inherits="LoanStarPortal.Controls.MortgageProfileContacts" %>
<table width="100%" cellpadding="0" cellspacing="0" border="0">
    <asp:Repeater ID="rpAsignee" runat="server" OnItemDataBound="rpAsignee_ItemDataBound">
    <ItemTemplate>
    <tr style="vertical-align:middle;">
        <td class="contactslabeltd">
            <asp:Label ID="lblRole" runat="server" Text=""></asp:Label>
        </td>
        <td class="contactsselecttd">
            <asp:DropDownList ID="ddlUsers" runat="server" CssClass="contactsuser">
            </asp:DropDownList>
            <asp:Label ID="lblUser" runat="server" Text=""></asp:Label>                
            <input id="btnAssignMe" type="submit" value="Assign Me" runat="server" />
        </td>
    </tr>
    </ItemTemplate>
    </asp:Repeater>    
</table>