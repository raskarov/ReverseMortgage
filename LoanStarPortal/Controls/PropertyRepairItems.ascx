<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PropertyRepairItems.ascx.cs" Inherits="LoanStarPortal.Controls.PropertyRepairItems" EnableViewState="true"%>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<table class="kontrTab" cellspacing="0" cellpadding="0" width="97%" align="center" border="0">
    <tr>
        <td colspan="3">
            <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                <tr>
                    <td><img alt="" src="images/bg_kontrTab_title_left.gif"/></td>
                    <td class="centerTd_title" align="left">
                        <table cellspacing="0" cellpadding="0" align="left" border="0">
                            <tr>
                                <td class="centerTd_title_text">Description</td>
                                <td><img alt="" src="images/bg_kontrTab_title_tt_right.gif"/></td>
                            </tr>
                        </table>
                    </td>
                    <td><img alt="" src="images/bg_kontrTab_title_right.gif"/></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="bg_color"><img alt="" src="images/bg_kontrTab_topleft.gif"/></td>
        <td class="bg_kontrTab_top"></td>
        <td class="bg_color"><img alt="" src="images/bg_kontrTab_topright.gif"/></td>
    </tr>
    <tr>
        <td class="bg_kontrTab_left"></td>
        <td class="content_kontrTab">
            <table cellspacing="0" cellpadding="0" align="left" border="0" width="100%">
                <tr id="trfields" runat="server">
                    <td></td>
                </tr>
                <tr style="height:10px"><td>&nbsp;</td></tr>
                <tr id="trrepairitems" runat="server">
                    <td>
                        <asp:GridView ID="gRepairItems" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowSorting="true" DataKeyNames="Id" GridLines="None" OnRowCancelingEdit="G_RowCancel" OnRowEditing="G_RowEditing" OnRowUpdating="G_RowUpdating" OnRowDataBound="G_RowDataBound" OnSorting="G_Sorting" OnPageIndexChanged="G_PageIndexChanged" OnPageIndexChanging="G_PageIndexChanging" CellPadding="4" EmptyDataText="No records to display" ForeColor="#333333" OnRowCommand="G_RowCommand" Width="95%" PageSize="5"  EnableViewState="False"  SkinID="TotalGrid">
                        <Columns>
                            <asp:TemplateField HeaderText="Description" SortExpression="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Description") %>' Width="95px"></asp:Label>
                                <asp:TextBox ID="tbDescription" runat="server" Visible="false" Width="290px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ErrorMessage="*" ControlToValidate="tbDescription" Visible="false"></asp:RequiredFieldValidator>
                            </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="300px" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Repair status">
                            <ItemTemplate>
                                <asp:Label ID="lblRepairStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RepairStatusName") %>' Width="95px"></asp:Label>
                                <asp:DropDownList ID="ddlRepairStatus" runat="server" Visible="false"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvRepairStatus" runat="server" ErrorMessage="*" ControlToValidate="ddlRepairStatus" Visible="false"></asp:RequiredFieldValidator>
                            </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Bid amount" SortExpression="BidAmount">
                            <ItemTemplate>
                                <asp:Label ID="lblBidAmount" runat="server" Text='<%# GetMoney(Container.DataItem,"BidAmount") %>' Width="80px"></asp:Label>
                                <radI:RadNumericTextBox ShowSpinButtons="false" Type="Currency" ID="tbBidAmount" runat="server" Visible="false" Width="75px"></radI:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="rfvBidAmount" runat="server" ErrorMessage="*" ControlToValidate="tbBidAmount" Visible="false"></asp:RequiredFieldValidator>
                            </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="85px" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Estimate source">
                            <ItemTemplate>
                                <asp:Label ID="lblEstimateSource" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"EstimateSourceName") %>' Width="95px"></asp:Label>
                                <asp:DropDownList ID="ddlEstimateSource" runat="server" Visible="false"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvEstimateSource" runat="server" ErrorMessage="*" ControlToValidate="ddlEstimateSource" Visible="false"></asp:RequiredFieldValidator>
                            </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" CommandName="Edit" CommandArgument='<%# GetCurrentRow() %>' runat="server" ImageUrl="~/images/edit.gif"/>
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
                    </td>
                </tr>
                <tr id="trtotal" runat="server">
                    <td>Total Repair Set Asides:&nbsp;<asp:Label ID="lblTotalRepairSetAsides" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
        </td>
        <td class="bg_kontrTab_right"></td>
    </tr>
    <tr>
        <td><img alt="" src="images/bg_kontrTab_botleft.gif"/></td>
        <td class="bg_kontrTab_bottom"></td>
        <td><img alt="" src="images/bg_kontrTab_botright.gif"/></td>
    </tr>
</table>
