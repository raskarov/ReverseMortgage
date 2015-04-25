<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StateLicensing.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.StateLicensing" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<div style="padding-left:20px;padding-top:10px">
<table cellpadding="0" cellspacing="0" border="0" style="width:100%;height:600px">
    <tr>
        <td>
        <radG:RadGrid ID="G" Skin="WebBlue" GridLines="None" EnableAJAX="false" runat="server"        
            Width="400px" PageSize="30"  AllowPaging="True" AutoGenerateColumns="False" 
            OnItemDataBound="G_ItemDataBound"
            OnNeedDataSource="GetData"
            AllowMultiRowEdit="false" AllowSorting="True" 
            >
            <ClientSettings>
            <Resizing EnableRealTimeResize="True"/>
            </ClientSettings>
                <MasterTableView AllowNaturalSort="False" EditMode="InPlace" DataKeyNames="ID" >
                    <Columns>
                        <radG:GridTemplateColumn HeaderText="State" SortExpression="Name" UniqueName="TemplateColumn">
                            <ItemTemplate>
                                <asp:Label ID="lblState" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="150px"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="License required" UniqueName="TemplateColumn1">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbLicenseRequired" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsPersonalLicenseRequired") %>' AutoPostBack="true" OnCheckedChanged="CheckChanged"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="120px"/>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </radG:GridTemplateColumn>                        
                    </Columns>
                    <RowIndicatorColumn Visible="False">
                        <HeaderStyle Width="20px" />
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="False">
                        <HeaderStyle Width="19px" />
                    </ExpandCollapseColumn>
                </MasterTableView>
                <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
            </radG:RadGrid>        
        </td>
    </tr>
</table>
</div>
