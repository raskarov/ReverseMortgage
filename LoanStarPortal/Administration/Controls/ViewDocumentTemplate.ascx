<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewDocumentTemplate.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewDocumentTemplate" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>

<script language="javascript" type="text/javascript">
function RaddpFromPopup()
    {
        <%= raddpFrom.ClientID %>.ShowPopup();
    }
    
function RaddpToPopup()
    {
        <%= raddpTo.ClientID %>.ShowPopup();
    }
    
function ClearFilter()
    {
        document.getElementById('<%= tbTitle.ClientID %>').value = '';
        <%= raddpTo.ClientID %>.DateInput.Clear();
        <%= raddpFrom.ClientID %>.DateInput.Clear();
        document.getElementById('<%= cbArchived.ClientID %>').checked = false;
    }
    
function RefreshDocTemplates()
    {
        window['<%=RadGridDocTemplates.ClientID %>'].AjaxRequest('<%= this.UniqueID %>', 'ResetAndRebind');
    }
</script>

<table width="100%" border="0" cellspacing="0" cellpadding="0" >
    <tr>
        <td valign="top" align="center">
            <asp:Label ID="lblHeader" runat="server" Text="Document Templates" Font-Bold="True" Font-Size="X-Large"></asp:Label>
        </td>
    </tr>
    <tr>
        <td valign="middle" align="center">
            <hr style="border:solid 0px black"/>
        </td>
    </tr>
    <tr>
        <td align="center">
            <table border="0" cellpadding="0" cellspacing="0" width="70%">
                <tr>
                    <td align="left" colspan="3">
                        <asp:HyperLink Runat="server" ID="AddDocTemplate" CssClass="cssLink" >Add Document Template</asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width:12%">
                        <asp:Label ID="lblFilter" runat="server" Text="Filter:" Font-Bold="True"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblTitle" runat="server" Text="Title:"></asp:Label>
                    </td>
                    <td align="left" style="width:65%">
                        <asp:TextBox ID="tbTitle" runat="server" Width="100%"></asp:TextBox>
                    </td>
                </tr>
 
                <tr>
                    <td></td>
                    <td align="left">
                        <asp:Label ID="lblUpdateDate" runat="server" Text="Last Upload Date:"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblFrom" runat="server" Text="From"></asp:Label>
                        <radCln:RadDatePicker ID="raddpFrom" runat="server" Width="100px" FocusedDate="2099-12-31">
                            <DatePopupButton Visible="False" />
                            <DateInput onclick="RaddpFromPopup()" />
                        </radCln:RadDatePicker>
                        
                        <asp:Label ID="lblTo" runat="server" Text="To"></asp:Label>
                        <radCln:RadDatePicker ID="raddpTo" runat="server" Width="100px" FocusedDate="2099-12-31">
                            <DatePopupButton Visible="False" />
                            <DateInput onclick="RaddpToPopup()" />
                        </radCln:RadDatePicker>
                    </td>
                </tr>
                
                <tr>
                    <td style="height: 38px"></td>
                    <td style="height: 38px" valign="middle" align="left">
                        <asp:Label ID="lblArchived" runat="server" Text="Include Archived:"></asp:Label>
                    </td>
                    <td style="height: 38px" align="left">
                        <asp:CheckBox ID="cbArchived" runat="server" Text=" " onclick="RefreshDocTemplates();" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td valign="middle" align="center">
            <hr style="width:70%; border:solid 0px black"/>
        </td>
    </tr>
    <tr>
        <td align="center" valign="middle">
            &nbsp;<input id="btnRefreshDocs1" style="width: 100px" type="button" value="Refresh List" onclick="RefreshDocTemplates();"/>
            &nbsp;
            <input id="btnClearFilter1" style="width: 100px" type="button" value="Clear Filter" onclick="ClearFilter();"/></td>
    </tr>
    <tr>
        <td valign="middle" align="center">
            <hr style="border:solid 0px black"/>
        </td>
    </tr>
    <tr>
        <td align="center">
            <radG:RadGrid ID="RadGridDocTemplates" runat="server" AutoGenerateColumns="False" EnableAJAX="false" Width="99%" GridLines="None" OnNeedDataSource="RadGridDocTemplates_NeedDataSource" OnSortCommand="RadGridDocTemplates_SortCommand" AllowSorting="True" OnItemCommand="RadGridDocTemplates_ItemCommand" Skin="WebBlue" AllowPaging="false">
                <MasterTableView>
                    <ExpandCollapseColumn Visible="False">
                        <HeaderStyle Width="19px" />
                    </ExpandCollapseColumn>
                    <RowIndicatorColumn Visible="False">
                        <HeaderStyle Width="20px" />
                    </RowIndicatorColumn>
                    <Columns>
                        <radG:GridBoundColumn
                            HeaderText="Title" UniqueName="Title" DataField="Title" SortExpression="Title" AllowFiltering="False" Groupable="False" Reorderable="False">
                        </radG:GridBoundColumn>
                        <radG:GridTemplateColumn HeaderText="Last Upload Date" UniqueName="UploadDate" AllowFiltering="False" DataType="System.DateTime" Groupable="False" Reorderable="False" SortExpression="UploadDate">
                            <ItemTemplate>
                                <asp:Label ID="lblUploadDate" runat="server" Text='<%# GetShortDate(Container.DataItem, "UploadDate") %>'></asp:Label>
                            </ItemTemplate>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn AllowFiltering="False" Groupable="False" HeaderText="Archived"
                            Reorderable="False" SortExpression="Archived" UniqueName="Archived">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbArchived" runat="server" Text='<%# GetArchivedValue(Container.DataItem, "Archived") %>' CommandName="Archive" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn AllowFiltering="False" Groupable="False"
                            HeaderText="Edit" Reorderable="False" Resizable="False" UniqueName="EditLink">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEditDocTemplate" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </radG:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                
                <ItemStyle Wrap="False" Height="20px"/>
                <AlternatingItemStyle Wrap="False" Height="20px"/>
                <PagerStyle Mode="NumericPages" />
                <ClientSettings>
                    <Resizing AllowColumnResize="True" />
                </ClientSettings>
            </radG:RadGrid>
        </td>
    </tr>
</table>
<div style="margin-bottom:50px"></div>