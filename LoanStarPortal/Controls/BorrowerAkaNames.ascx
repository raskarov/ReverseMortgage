<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BorrowerAkaNames.ascx.cs" Inherits="LoanStarPortal.Controls.BorrowerAkaNames" %>
<asp:GridView ID="gAKANames" runat="server" SkinID="TotalGrid"
AutoGenerateColumns = "false"
EnableViewState = "false"
AllowPaging="false" 
AllowSorting="true"
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
        <asp:TemplateField HeaderText="Name" HeaderStyle-Width="90%">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblName" EnableViewState="false" Text='<%#Eval("Name")%>'></asp:Label>
                <asp:TextBox runat="server" ID="tbName" EnableViewState="false" Visible="false" Width="200px"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="right" HeaderStyle-Width="40px">
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
</asp:GridView>
<br />
