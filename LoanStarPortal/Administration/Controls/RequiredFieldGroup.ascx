<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequiredFieldGroup.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.RequiredFieldGroup" %>
<div runat="server" id="groupdiv">
    <asp:CheckBox ID="cbAll" runat="server"  Text="Check/uncheck all" EnableViewState="False" />
    <asp:DataList ID="DataList1" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" OnItemDataBound="ItemDataBound" EnableViewState="False">
        <ItemTemplate>
            <asp:CheckBox ID="CheckBox1" runat="server"  />
        </ItemTemplate>
    </asp:DataList>
</div>
