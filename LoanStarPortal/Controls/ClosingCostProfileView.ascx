<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClosingCostProfileView.ascx.cs" Inherits="LoanStarPortal.Controls.ClosingCostProfileView" %>
<%@ Register Namespace="Telerik.WebControls" TagPrefix="radI" Assembly="RadInput.NET2"%>

<table border="0" cellpadding="0" cellspacing="0" style="width:100%">
    <tr>
        <td>
            <asp:GridView ID="gInvoices" runat="server" SkinID="TotalGrid"
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
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblDescription" EnableViewState="false" Text='<%# GetTypeName(Container.DataItem) %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width ="40%"/>
                    </asp:TemplateField>    
                    <asp:TemplateField HeaderText="Provider">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblProvider" EnableViewState="false" Text='<%# GetProviderName(Container.DataItem) %>' ></asp:Label>
                            <asp:LinkButton ID="lbProvider" runat="server" CommandName="ChangeProvider" CommandArgument='<%# GetCurrentRow() %>' Width="100px" CssClass="EmailLinks"><%# GetProviderName(Container.DataItem)%></asp:LinkButton>
                            <asp:DropDownList ID="ddlProvidere" runat="server" AutoPostBack="true" Visible="false" Width="110px" OnSelectedIndexChanged="Providere_SelectedIndexChanged" ></asp:DropDownList>
                            <asp:TextBox ID="tbProvidere" runat="server" Visible="false" Width="110px" MaxLength="100"></asp:TextBox><asp:RequiredFieldValidator ID="rftbProvidere" ControlToValidate="tbProvidere" ErrorMessage="*" Visible="false" runat="server"></asp:RequiredFieldValidator>
                        </ItemTemplate>
                        <ItemStyle Width ="30%"/>
                        <HeaderStyle HorizontalAlign="Left"  />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblAmount" EnableViewState="false" Text='<%# GetFinancedAmount(Container.DataItem)%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width ="20%" HorizontalAlign="Right"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEdit" CommandName="Edit" CommandArgument='<%# GetCurrentRow() %>' runat="server" ImageUrl="~/images/edit.gif"/>
                        <asp:ImageButton ID="imgDelete" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' runat="server" ImageUrl="~/images/delete.gif"/>
                        <asp:ImageButton ID="imgUpdate" CommandName="Update" runat="server" ImageUrl="~/images/update.gif" Visible="false"/>
                        <asp:ImageButton ID="imgCancel" CommandName="Cancel" runat="server" CausesValidation="false" ImageUrl="~/images/cancel.gif" Visible="false"/>
                    </ItemTemplate>
                   
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" Width="40px" />
                    </asp:TemplateField>
                </columns>
            </asp:GridView>
        </td>
    </tr>
</table>


