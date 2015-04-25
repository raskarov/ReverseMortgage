<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginTrack.ascx.cs" Inherits="LoanStarPortal.Controls.LoginTrack" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>

<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Width="100%" BorderWidth="0" BorderStyle="None" BorderSize="0" Skin="Default" LiveResize="false">
   <radspl:radpane id="TopPane" runat="server" Height="35px" Scrolling="None">
        <div class="paneTitle"><b>Login track</b></div>
   </radspl:radpane>  
   <radspl:radsplitbar id="RadSplitBar3" runat="server" CollapseMode="None" EnableResize="false" Visible="false"/>
   <radspl:radpane id="MiddlePane" runat="server" scrolling="Y" >
    <table border="0" cellpadding="3" cellspacing="3" style="width:99%;">
        <tr>
            <td colspan="2">
                <radG:RadGrid ID="grid" Skin="Windows" runat="server" AutoGenerateColumns="False" EnableAJAX="False" GridLines="Both" AllowPaging="True" PageSize="30" Width="99%" OnNeedDataSource="grid_NeedDataSource" AllowSorting="True" ShowGroupPanel="True">
                <ClientSettings AllowDragToGroup="True"/>
                    <MasterTableView DataKeyNames="ID" GroupsDefaultExpanded="false">
                        <ExpandCollapseColumn Visible="False">
                            <HeaderStyle Width="19px" />
                        </ExpandCollapseColumn>
                        <RowIndicatorColumn Visible="False">
                            <HeaderStyle Width="20px" />
                        </RowIndicatorColumn>
                        <NoRecordsTemplate>No items</NoRecordsTemplate>
                        
                       <GroupByExpressions>
                            <radG:GridGroupByExpression>
                                <SelectFields>
                                    <radG:GridGroupByField FieldAlias="UserName" FieldName="UserName" HeaderText="User"></radG:GridGroupByField>
                                </SelectFields>
                                <GroupByFields>
                                    <radG:GridGroupByField FieldName="UserName" SortOrder="Ascending" ></radG:GridGroupByField>
                                </GroupByFields>
                            </radG:GridGroupByExpression>
                        </GroupByExpressions>
                        
                        <Columns>
                            <radg:GridBoundColumn HeaderText="User" DataField="UserName" UniqueName="UserName" AllowSorting="true" SortExpression="UserName">
                                <HeaderStyle Width="35%"/>
                            </radg:GridBoundColumn>
                            <radg:GridBoundColumn HeaderText="IP address" DataField="IPAddress" UniqueName="IPAddress" AllowSorting="true" SortExpression="IPAddress">
                                <HeaderStyle Width="15%"/>
                            </radg:GridBoundColumn>
                            <radg:GridBoundColumn HeaderText="Login time" DataField="LoginTime" UniqueName="LoginTime">
                                <HeaderStyle Width="25%"/>
                            </radg:GridBoundColumn>
                            <radg:GridBoundColumn HeaderText="Logout time" DataField="LogoutTime" UniqueName="LogoutTime">
                                <HeaderStyle Width="25%"/>
                            </radg:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <ItemStyle Height="20px" Wrap="False"/>
                    <AlternatingItemStyle Height="20px" Wrap="False"/>
                    <PagerStyle Position="Bottom" PagerTextFormat="{2} - {3} of {5} {4}" />
                    <ClientSettings EnablePostBackOnRowClick="false">
                    </ClientSettings>
                </radG:RadGrid>
            </td>
        </tr>
    </table>
    </radspl:radpane>
</radspl:RadSplitter>
