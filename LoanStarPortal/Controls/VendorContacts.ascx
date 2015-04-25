<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendorContacts.ascx.cs" Inherits="LoanStarPortal.Controls.VendorContacts" %>
<table border="0" cellpadding="0" cellspacing="0" style="width:100%">
    <tr>
        <td><asp:Label runat="server" ID="lblVendorName" Font-Bold="true" Font-Size="16px"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="gContacts" runat="server" SkinID="TotalGrid" AutoGenerateColumns = "false" EnableViewState = "true" AllowPaging="true" AllowSorting="true" PageSize="15"
                OnRowDeleting = "G_RowDeleting"
                OnRowCancelingEdit="G_RowCancel" 
                OnRowEditing="G_RowEditing" 
                OnRowUpdating="G_RowUpdating" 
                OnRowDataBound="G_RowDataBound" 
                EmptyDataText="No records to display"
                OnRowCommand="G_RowCommand" 
                OnSorting="G_Sorting" 
                OnPageIndexChanged="G_PageIndexChanged" 
                OnPageIndexChanging="G_PageIndexChanging"
            >
            <columns>
            <asp:TemplateField HeaderText="Name" SortExpression="Name">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblName" EnableViewState="false" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>                    
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
                <HeaderStyle HorizontalAlign="Left" Width="30%" />
             </asp:TemplateField>
            <asp:TemplateField HeaderText="Phone" SortExpression="Phone">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblPnone" EnableViewState="false" Text='<%# DataBinder.Eval(Container.DataItem,"Phone") %>'></asp:Label>                    
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
                <HeaderStyle HorizontalAlign="Left" Width="60px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Fax" SortExpression="Fax">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblFax" EnableViewState="false" Text='<%# DataBinder.Eval(Container.DataItem,"Fax") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
                <HeaderStyle HorizontalAlign="Left" Width="60px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Email" SortExpression="Email">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblEmail" EnableViewState="false" Text='<%# DataBinder.Eval(Container.DataItem,"Email") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
                <HeaderStyle HorizontalAlign="Left" Width="30%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:ImageButton ID="imgEdit" CommandName="Edit" CommandArgument='<%# GetCurrentRow() %>' runat="server" ImageUrl="~/images/edit.gif"/>
                    <asp:ImageButton ID="imgDelete" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' runat="server" ImageUrl="~/images/delete.gif"/>
                    <asp:ImageButton ID="imgUpdate" CommandName="Update" runat="server" ImageUrl="~/images/update.gif" Visible="false"/>
                    <asp:ImageButton ID="imgCancel" CommandName="Cancel" runat="server" CausesValidation="false" ImageUrl="~/images/cancel.gif" Visible="false"/>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label runat="server" ID="lbl" EnableViewState="false"></asp:Label>
                </FooterTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle HorizontalAlign="Center" Width="40px" />
            </asp:TemplateField>
            </columns>
                <PagerStyle HorizontalAlign="Left" />
            </asp:GridView>
        </td>
    </tr>
</table>
