<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Payoffs.ascx.cs" Inherits="LoanStarPortal.Controls.Payoffs" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<table cellspacing="0" cellpadding="0" align="left" border="0" width="100%">
    <tr>
        <td>
            <asp:GridView ID="gPayoffs" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowSorting="true" DataKeyNames="Id" GridLines="None" 
                OnRowCancelingEdit="G_RowCancel" 
                OnRowEditing="G_RowEditing" 
                OnRowUpdating="G_RowUpdating" 
                OnRowDataBound="G_RowDataBound" 
                OnSorting="G_Sorting" 
                OnPageIndexChanged="G_PageIndexChanged" 
                OnPageIndexChanging="G_PageIndexChanging" 
                OnRowDeleting = "G_RowDeleting"
                OnRowCommand="G_RowCommand" 
                CellPadding="4" EmptyDataText="No records to display" ForeColor="#333333" PageSize="5"  EnableViewState="False" SkinID="TotalGrid">
                <Columns>
                    <asp:TemplateField HeaderText="Creditor" SortExpression="Creditor">
                        <ItemTemplate>
                            <asp:Label ID="lblCreditor" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Creditor") %>' Width="95px"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="110px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" SortExpression="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' Width="120px" Visible='<%# !CanChangeStatus %>'></asp:Label>
                            <asp:LinkButton ID="lbStatus" runat="server" CommandName="ChangeStatus" CommandArgument='<%# GetCurrentRow() %>' Visible='<%# CanChangeStatus %>' Width="100px"><%# Eval("Status") %></asp:LinkButton>
                            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" Visible="false" Width="110px" OnSelectedIndexChanged="Status_SelectedIndexChanged" ></asp:DropDownList>                                
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="120px" />
                    </asp:TemplateField>        
                    <asp:TemplateField HeaderText="Amount" SortExpression="Amount">
                        <ItemTemplate>
                            <asp:Label ID="lblAmount" runat="server" Text='<%# GetMoney(Container.DataItem,"Amount") %>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="85px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Perdiem" SortExpression="Perdiem">
                        <ItemTemplate>
                            <asp:Label ID="lblPerdiem" runat="server" Text='<%# GetPeridem(Container.DataItem,"Perdiem") %>' Width="80px"></asp:Label>
                        </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="85px" />
                        </asp:TemplateField>        
                    <asp:TemplateField HeaderText="ExpDate" SortExpression="ExpDate">
                        <ItemTemplate>
                            <asp:Label ID="lblExpDate" runat="server" Text='<%# GetExpDate(Container.DataItem,"ExpDate") %>' Width="85px"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="90px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Days left">
                            <ItemTemplate>
                                <asp:Label ID="lblDaysLeft" runat="server" Text='<%# GetDaysLeft(Container.DataItem) %>' Width="50px"></asp:Label>
                            </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="55px" />
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
                </Columns>
                <PagerStyle HorizontalAlign="Left" />
            </asp:GridView>                    
        </td>
    </tr>
</table>

