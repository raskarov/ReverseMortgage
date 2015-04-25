<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MortgagePrepaidItems.ascx.cs" Inherits="LoanStarPortal.Controls.MortgagePrepaidItems" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<asp:Panel ID="Panel1" runat="server">
Prepaid items:<br />
<asp:GridView ID="gPrepaidItems" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" GridLines="None" OnRowCancelingEdit="G_RowCancel" OnRowEditing="G_RowEditing" OnRowUpdating="G_RowUpdating" OnRowDataBound="G_RowDataBound" CellPadding="4" EmptyDataText="No records to display" ForeColor="#333333" OnRowCommand="G_RowCommand" OnSorting="G_Sorting" OnPageIndexChanged="G_PageIndexChanged" OnPageIndexChanging="G_PageIndexChanging" Width="95%" AllowPaging="True" AllowSorting="True" PageSize="5" SkinID="TotalGrid">
    <Columns>
        <asp:TemplateField HeaderText="Description" SortExpression="Description">
            <ItemTemplate>
                <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Description") %>' Width="95px"></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" Width="100px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="PaymentTo" SortExpression="PaymentTo">
            <ItemTemplate>
                <asp:Label ID="lblPaymentTo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PaymentTo") %>' Width="85px"></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" Width="90px" />
        </asp:TemplateField>        
        <asp:TemplateField HeaderText="Amount" SortExpression="Amount">
            <ItemTemplate>
                <asp:Label ID="lblAmount" runat="server" Text='<%# GetMoney(Container.DataItem,"Amount") %>' Width="85px"></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" Width="90px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Borrower Portion" SortExpression="BorrowerPortion">
            <ItemTemplate>
                <asp:Label ID="lblBorrowerPortion" runat="server" Text='<%# GetMoney(Container.DataItem,"BorrowerPortion") %>' ></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" Width="100px" />
        </asp:TemplateField>        
        <asp:TemplateField HeaderText="Seller Portion" SortExpression="SellerPortion">
            <ItemTemplate>
                <asp:Label ID="lblSellerPortion" runat="server" Text='<%# GetMoney(Container.DataItem,"SellerPortion") %>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" Width="100px" />
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
            <HeaderStyle HorizontalAlign="Center" Width="40px" />
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</asp:Panel>