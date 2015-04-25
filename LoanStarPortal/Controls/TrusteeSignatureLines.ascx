<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrusteeSignatureLines.ascx.cs" Inherits="LoanStarPortal.Controls.TrusteeSignatureLines" %>
<asp:GridView ID="gSignatureLines" runat="server" SkinID="TotalGrid"
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
        <asp:TemplateField HeaderText="Signature Line" HeaderStyle-Width="90%">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblSignatureLine" EnableViewState="false" Text='<%#Eval("SignatureLine")%>'></asp:Label>
                <asp:TextBox runat="server" ID="tbSignatureLine" EnableViewState="false" Visible="false" Width="200px" MaxLength="256"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv" runat="server" ErrorMessage="*" ControlToValidate="tbSignatureLine" Visible="false"></asp:RequiredFieldValidator>
            </ItemTemplate>            
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
        <ItemTemplate>
            <asp:ImageButton ID="imgEdit" CommandName="Edit" CommandArgument='<%# GetCurrentRow() %>' runat="server" ImageUrl="~/images/edit.gif" Visible='<%# canEdit %>'/>
            <asp:ImageButton ID="imgDelete" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' runat="server" ImageUrl="~/images/delete.gif" Visible='<%# canEdit %>'/>
            <asp:ImageButton ID="imgUpdate" CommandName="Update" runat="server" ImageUrl="~/images/update.gif" Visible="false"/>
            <asp:ImageButton ID="imgCancel" CommandName="Cancel" runat="server" CausesValidation="false" ImageUrl="~/images/cancel.gif" Visible="false"/>
        </ItemTemplate>
        <FooterTemplate>
            <asp:Label runat="server" ID="lbl" EnableViewState="false"></asp:Label>
        </FooterTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderStyle HorizontalAlign="Right" Width="20px" />        
        </asp:TemplateField>        
    </columns>
</asp:GridView>
<br />

