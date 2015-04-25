<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FieldGroup.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.FieldGroup" %>
<div runat="server" id="groupdiv">
    <asp:CheckBox ID="cbAll" runat="server"  Text="Check/uncheck all" EnableViewState="False" />
    <asp:DataList ID="DataList1" runat="server" RepeatColumns="5" RepeatDirection="Horizontal" OnItemDataBound="ItemDataBound" EnableViewState="False">
        <ItemTemplate>
            <asp:CheckBox ID="CheckBox1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Description") %>'  />
        </ItemTemplate>
    </asp:DataList>
</div>
