<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Reserves.ascx.cs" Inherits="LoanStarPortal.Controls.Reserves" %>
<asp:Panel runat="server">
Reserves Deposited With Lender: 
<asp:GridView ID="gReserves" runat="server" SkinID="TotalGrid"
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
        <asp:TemplateField HeaderText="Type" HeaderStyle-Width="30%">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblType" EnableViewState="false" Text='<%#Eval("TypeName")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Months needed" HeaderStyle-Width="30%">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblNumber" EnableViewState="false" Text='<%#Eval("Months")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Charge&nbsp;to" HeaderStyle-Wrap="true" HeaderStyle-Width="20%">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblChargeTo" EnableViewState="false" Text='<%#Eval("ChargeName")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Amount" HeaderStyle-Width="15%">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblAmount" EnableViewState="false" Text='<%#string.Format("{0:C}", Eval("Amount"))%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Action">
            <ItemTemplate>
                <asp:ImageButton ID="imgEdit" CommandName="Edit" CommandArgument='<%# GetCurrentRow() %>' runat="server" ImageUrl="~/images/edit.gif" Visible='<%# canEdit %>'/>
                <asp:ImageButton ID="imgUpdate" CommandName="Update" runat="server" ImageUrl="~/images/update.gif" Visible="false"/>
                <asp:ImageButton ID="imgCancel" CommandName="Cancel" runat="server" CausesValidation="false" ImageUrl="~/images/cancel.gif" Visible="false"/>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label runat="server" ID="lbl" EnableViewState="false"></asp:Label>
            </FooterTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderStyle HorizontalAlign="Center" Width="20px" />
        </asp:TemplateField>        
    </columns>
</asp:GridView>
<br /><br />
</asp:Panel>