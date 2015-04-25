<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FieldChanges.ascx.cs" Inherits="LoanStarPortal.Controls.FieldChanges" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>

<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Width="100%" BorderWidth="0" BorderStyle="None" BorderSize="0" Skin="Default" LiveResize="false">
   <radspl:radpane id="TopPane" runat="server" Height="35px" Scrolling="None">
        <div class="paneTitle"><b>Field changes</b></div>
   </radspl:radpane>  
   <radspl:radsplitbar id="RadSplitBar3" runat="server" CollapseMode="None" EnableResize="false" Visible="false"/>
   <radspl:radpane id="MiddlePane" runat="server" scrolling="Y" >
    <table border="0" cellpadding="3" cellspacing="3" style="width:99%;">
        <tr>
            <td width="5%">Group:&nbsp;<radC:RadComboBox ID="ddlGroup" runat="server" Skin="WindowsXP" AutoPostBack="true" Width="120px" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" Height="145px"></radC:RadComboBox>&nbsp;&nbsp;&nbsp;&nbsp;Field:&nbsp;<radC:RadComboBox ID="ddlField" runat="server" Skin="WindowsXP" AutoPostBack="true" Width="300px" OnSelectedIndexChanged="ddlField_SelectedIndexChanged" Height="145px"></radC:RadComboBox></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <radG:RadGrid ID="gridChanges" Skin="Windows" runat="server" AutoGenerateColumns="False" EnableAJAX="False" GridLines="Both" AllowPaging="True" PageSize="30" Width="99%" OnNeedDataSource="gridChanges_NeedDataSource" AllowSorting="True">
                    <MasterTableView DataKeyNames="ID">
                        <NoRecordsTemplate>No items</NoRecordsTemplate>
                        <ExpandCollapseColumn Visible="False">
                            <HeaderStyle Width="19px" />
                        </ExpandCollapseColumn>
                        <RowIndicatorColumn Visible="False">
                            <HeaderStyle Width="20px" />
                        </RowIndicatorColumn>
                        <Columns>
                            <radg:GridBoundColumn HeaderText="User" DataField="UserName" UniqueName="UserName" AllowSorting="true" SortExpression="UserName">
                                <HeaderStyle Width="18%"/>
                            </radg:GridBoundColumn>
                            <radg:GridBoundColumn HeaderText="Date" DataField="ModifiedDate" UniqueName="ModifiedDate" AllowSorting="true" SortExpression="ModifiedDate">
                                <HeaderStyle Width="22%"/>
                            </radg:GridBoundColumn>
                            <radg:GridBoundColumn HeaderText="From" DataField="OldValue" UniqueName="OldValue" AllowSorting="true" SortExpression="OldValue">
                                <HeaderStyle Width="30%"/>
                            </radg:GridBoundColumn>
                            <radg:GridBoundColumn HeaderText="To" DataField="NewValue" UniqueName="NewValue" AllowSorting="true" SortExpression="NewValue">
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
    </radspl:radpane>
</radspl:RadSplitter>
