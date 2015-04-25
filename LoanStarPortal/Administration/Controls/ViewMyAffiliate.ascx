<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewMyAffiliate.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewMyAffiliate" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<table border="0" cellpadding="0" cellspacing="0" style="width:100%">
    <tr>
        <td align="center">
            <asp:Label ID="lblRole" runat="server" Text="Label" SkinID="AdminHeader"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="center">
            <radG:RadGrid ID="G" runat="server" AllowSorting="True" EnableAJAX="False" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="80%" GroupingEnabled="False" OnItemCommand="G_ItemCommand">
                <MasterTableView EditMode="InPlace">
                    <ExpandCollapseColumn Visible="False">
                    <HeaderStyle Width="19px"></HeaderStyle>
                    </ExpandCollapseColumn>
                    <RowIndicatorColumn Visible="False">
                    <HeaderStyle Width="20px"></HeaderStyle>
                    </RowIndicatorColumn>                    
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Company" SortExpression="CompanyName">
                            <ItemTemplate>
                                <asp:Label ID="lblCompanyName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80%"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="FeeColumn" HeaderText="" ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkServicefee" runat="server" CommandName="ServiceFee" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"AffiliateCompanyId") %>' >Service fee</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <PagerStyle Mode="NumericPages" PagerTextFormat="" />
            </radG:RadGrid>
        
        </td>
    </tr>    
</table>