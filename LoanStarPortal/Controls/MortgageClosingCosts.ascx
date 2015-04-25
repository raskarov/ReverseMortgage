<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MortgageClosingCosts.ascx.cs" Inherits="LoanStarPortal.Controls.MortgageClosingCosts" %>
<br /><br />
Payoffs
<asp:GridView ID="gPays" runat="server" SkinID="TotalGrid"
EnableViewState = "false"
AutoGenerateColumns = "false"
AllowPaging="false" 
AllowSorting="true"
OnRowCancelingEdit="G_RowCancel" 
OnRowEditing="G_RowEditing" 
OnRowUpdating="G_RowUpdating" 
OnRowDataBound="G_RowDataBound" 
EmptyDataText="No records to display!" 
OnRowCommand="G_RowCommand" 
OnSorting="G_Sorting" 
OnPageIndexChanged="G_PageIndexChanged" 
OnPageIndexChanging="G_PageIndexChanging"
 >
    <columns>
        <asp:BoundField DataField="Creditor" HeaderText="Creditor" HeaderStyle-Width="80%"/>
        <asp:TemplateField HeaderText="Amount" HeaderStyle-Width="20%">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblAmount" EnableViewState="false" Text='<%#string.Format("{0:C}", Eval("Amount"))%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label runat="server" ID="lbl" EnableViewState="false"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
    </columns>
</asp:GridView>