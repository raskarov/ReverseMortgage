<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Links.ascx.cs" Inherits="LoanStarPortal.Controls.Links" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>

<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Width="100%" BorderWidth="0" BorderStyle="None" BorderSize="0" Skin="Default" LiveResize="false">
   <radspl:radpane id="TopPane" runat="server" Height="35px" Scrolling="None">
        <div class="paneTitle"><b>Links</b></div>
   </radspl:radpane>  
   <radspl:radsplitbar id="RadSplitBar3" runat="server" CollapseMode="None" EnableResize="false" Visible="false"/>
   <radspl:radpane id="MiddlePane" runat="server" scrolling="Y" >
    <div RadResizeStopLookup="true" RadShowStopLookup="true" >
    <table border="0" cellpadding="3" cellspacing="3" style="width:99%;">
        <tr>
            <td>
                <radG:RadGrid ID="grid" Skin="Windows" runat="server" 
                AutoGenerateColumns="False" EnableAJAX="False" 
                AllowSorting="True" AllowPaging="True" PageSize="30" 
                Width="99%" GridLines="Both"
                OnNeedDataSource="grid_NeedDataSource"
                >
                    <MasterTableView DataKeyNames="ID" AllowSorting="True">
                        <RowIndicatorColumn Visible="False">
                            <HeaderStyle Width="20px" />
                        </RowIndicatorColumn>
                        <NoRecordsTemplate>No items</NoRecordsTemplate>
                        <Columns>
                            <radg:GridBoundColumn HeaderText="Title" DataField="Title" UniqueName="Title" AllowSorting="true" SortExpression="Title">
                                <HeaderStyle Width="35%"/>
                            </radg:GridBoundColumn>
                        <radG:GridTemplateColumn HeaderText="URL" UniqueName="URL">
                            <ItemTemplate>
                                <a href='<%# DataBinder.Eval(Container.DataItem, "URL")%>' target="_blank"><%# DataBinder.Eval(Container.DataItem, "URL") %></a>
                            </ItemTemplate>
                        </radG:GridTemplateColumn>                            
<%--
                            <radg:GridBoundColumn HeaderText="URL" DataField="URL" UniqueName="URL" AllowSorting="true" SortExpression="URL">
                                <HeaderStyle Width="35%"/>
                            </radg:GridBoundColumn>
--%>
                            <radg:GridBoundColumn HeaderText="Description" DataField="Description" UniqueName="Description">
                                <HeaderStyle Width="30%"/>
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
    </div>
    </radspl:radpane>
</radspl:RadSplitter>
