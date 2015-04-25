<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MortgageBuyerPay.ascx.cs" Inherits="LoanStarPortal.Controls.MortgageBuyerPay" %>
<br>
BuyerPay
<asp:GridView ID="gPays" runat="server" SkinID="TotalGrid"
EnableViewState = "false"
AutoGenerateColumns = "false"
AllowPaging="false" 
AllowSorting="true"
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
        <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="50%"/>
        <asp:BoundField DataField="PaymentTo" HeaderText="To" HeaderStyle-Width="30%"/>
        <asp:TemplateField HeaderText="Amount" HeaderStyle-Width="20%">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblAmount" EnableViewState="false" Text='<%#string.Format("{0:C}", Eval("BorrowerPortion"))%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label runat="server" ID="lbl" EnableViewState="false"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
    </columns>
</asp:GridView>
<br />