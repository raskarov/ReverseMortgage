<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewVendors.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewVendors" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<table border="0" width="100%" cellpadding="0" cellspacing="0">
    <tr>    
        <td>
            <radG:RadGrid ID="gVendors" Skin="WebBlue" runat="server" CssClass="RadGrid" GridLines="None" AllowPaging="False" AllowSorting="true" Width="99%" AutoGenerateColumns="False" EnableAJAX="False" ShowStatusBar="false" HorizontalAlign="NotSet" OnItemCommand="gVendors_ItemCommand" OnSortCommand="gVendors_SortCommand" OnPageIndexChanged="gVendors_PageIndexChanged">
            <MasterTableView CommandItemDisplay="Bottom" GridLines="None" DataKeyNames="ID">
                <Columns>
                    <radG:GridBoundColumn UniqueName="Name" HeaderText="Company" DataField="Name" SortExpression="Name">
                        <HeaderStyle Width="59%" />
                    </radG:GridBoundColumn>
                    <radG:GridBoundColumn UniqueName="PrimaryContact" HeaderText="Contact " DataField="PCName" SortExpression="PrimaryContact">
                        <HeaderStyle Width="40%" />
                    </radG:GridBoundColumn>
                    <radG:GridButtonColumn CommandName="Edit"  ButtonType="ImageButton" ImageUrl="~/images/Edit.gif" UniqueName="editcolumn">
                    </radG:GridButtonColumn>                    
                    <radG:GridButtonColumn CommandName="Delete" ButtonType="ImageButton" ImageUrl="~/images/Delete.gif" UniqueName="deletecolumn" ConfirmText="Are you sure you want to delete this vendor?">
                    </radG:GridButtonColumn>
                </Columns>
                <CommandItemTemplate>
                    <div style="padding:10px 5px;">
                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="InitInsert" Visible='true' CssClass="EmailLinks"><img style="border:0px;vertical-align:middle;" alt="" src="/RadControls/Grid/Skins/WebBlue/AddRecord.gif" />Add new Vendor</asp:LinkButton>
                    </div>
                </CommandItemTemplate>                
                <ExpandCollapseColumn Visible="False">
                    <HeaderStyle Width="19px" />
                </ExpandCollapseColumn>
                <RowIndicatorColumn Visible="False">
                    <HeaderStyle Width="20px" />
                </RowIndicatorColumn>
            </MasterTableView>
                <PagerStyle Mode="NumericPages" PagerTextFormat="" />
        </radG:RadGrid>
        </td>
    </tr>
</table>
