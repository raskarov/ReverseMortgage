<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocFieldMapping.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.DocFieldMapping" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register TagPrefix="rada" Namespace="Telerik.WebControls" Assembly="RadAjax.NET2" %>

<script language="javascript" type="text/javascript">
window.onload = function()
{   
    document.getElementById('<%=btnBound.ClientID %>').disabled = true;
    document.getElementById('<%=btnUnBound.ClientID %>').disabled = true;
}

function BoundingResponseEnd(sender, arguments)
{
    document.getElementById('<%=btnBound.ClientID %>').disabled = true;
    document.getElementById('<%=btnUnBound.ClientID %>').disabled = true;
}

function SetAllowMapping(allow)
{
    document.getElementById('<%=btnBound.ClientID %>').disabled = true;
    window['<%=MPFieldsGrid.ClientID %>'].ClientSettings.Selecting.AllowRowSelect = allow == 0 ? false : true;
    window['<%=DTUnBoundFieldsGrid.ClientID %>'].ClientSettings.Selecting.AllowRowSelect = allow == 0 ? false : true;
}

function UpdateBinding()
{
    var lengthMPFieldRows = window['<%=MPFieldsGrid.ClientID %>'].MasterTableView.SelectedRows.length;
    var lengthDTFieldRows = window['<%=DTUnBoundFieldsGrid.ClientID %>'].MasterTableView.SelectedRows.length;
    if(lengthMPFieldRows == 0 || lengthDTFieldRows == 0)
    {
        document.getElementById('<%=btnBound.ClientID %>').disabled = true;
        return;
    }
    
    var dtvFormatType = document.getElementById('<%=dtvFormatType.ClientID %>').value;
    if (dtvFormatType.length > 0 && dtvFormatType == 2)
    {    
        var mpRowObject = window['<%=MPFieldsGrid.ClientID %>'].MasterTableView.SelectedRows[0];
        var mpTypeCell = window['<%=MPFieldsGrid.ClientID %>'].MasterTableView.GetCellByColumnUniqueName(mpRowObject, "type");
        var mpTypeValue = mpTypeCell.innerHTML;
        if(mpTypeValue != 'Boolean')
            mpTypeValue = 'String';
            
        var pdfRowObject;
        var pdfTypeCell;
        var pdfTypeValue;
        for(var i = 0; i < lengthDTFieldRows; i++)
        {
            pdfRowObject = window['<%=DTUnBoundFieldsGrid.ClientID %>'].MasterTableView.SelectedRows[i];
            pdfTypeCell = window['<%=DTUnBoundFieldsGrid.ClientID %>'].MasterTableView.GetCellByColumnUniqueName(pdfRowObject, "Type");
            pdfTypeValue = pdfTypeCell.innerHTML;
            
            if(mpTypeValue != pdfTypeValue)
            {
                document.getElementById('<%=btnBound.ClientID %>').disabled = true;
                document.getElementById('<%=lblActionInfoArea.ClientID %>').innerHTML = 'Type mismatch!';
                return;
            }
        }
    }
    
    document.getElementById('<%=btnBound.ClientID %>').disabled = false;
    document.getElementById('<%=lblActionInfoArea.ClientID %>').innerHTML = '';
}

function DTUnBoundFieldsGrid_RowSelected(rowObject)
{
    UpdateBinding();
}

function DTBoundFieldsGrid_RowSelected(rowObject)
{
    document.getElementById('<%=btnUnBound.ClientID %>').disabled = false;
}

function MPFieldsGrid_RowSelected(rowObject)
{
    UpdateBinding();
}

function RefreshMPFieldsGrid()
{
    window['<%=MPFieldsGrid.ClientID %>'].AjaxRequest('<%= this.UniqueID %>', 'RefreshMPFieldsGrid');
}

function RefreshDTUnBoundFieldsGrid()
{
    window['<%=DTUnBoundFieldsGrid.ClientID %>'].AjaxRequest('<%= this.UniqueID %>', 'RefreshUnBoundFieldsGrid');
}
</script>

<rada:RadAjaxManager id="RadAjaxManager1" runat="server">
    <ajaxsettings>
        <rada:ajaxsetting ajaxcontrolid="btnBound">
            <updatedcontrols>
                <rada:ajaxupdatedcontrol controlid="lblActionInfoArea"></rada:ajaxupdatedcontrol>
                <rada:ajaxupdatedcontrol controlid="DTUnBoundFieldsGrid"></rada:ajaxupdatedcontrol>
                <rada:ajaxupdatedcontrol controlid="DTBoundFieldsGrid"></rada:ajaxupdatedcontrol>
            </updatedcontrols>
        </rada:ajaxsetting>
        <rada:ajaxsetting ajaxcontrolid="btnUnBound">
            <updatedcontrols>
                <rada:ajaxupdatedcontrol controlid="lblActionInfoArea"></rada:ajaxupdatedcontrol>
                <rada:ajaxupdatedcontrol controlid="DTUnBoundFieldsGrid"></rada:ajaxupdatedcontrol>
                <rada:ajaxupdatedcontrol controlid="DTBoundFieldsGrid"></rada:ajaxupdatedcontrol>
            </updatedcontrols>
        </rada:ajaxsetting>
    </ajaxsettings>
    <ClientEvents OnResponseEnd="BoundingResponseEnd" />
</rada:RadAjaxManager>

<asp:HiddenField ID="dtvFormatType" runat="server" />

<table width="100%" border="0" cellspacing="0" cellpadding="0" >
    <tr>
        <td align="left" style="width:40%;">
            <asp:DropDownList ID="ddlMPFieldGroup" Width="160px" onchange="RefreshMPFieldsGrid();" runat="server">
            </asp:DropDownList>
        </td>
        <td style="width:20%"></td>
        <td align="left" style="width:40%;">
            <asp:DropDownList ID="ddlUnBoundFieldRegion" Width="160px" onchange="RefreshDTUnBoundFieldsGrid();" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    
    <tr>
        <td align="left" valign="top" style="width:40%;">
            <div style="padding: 10px 6px 20px 6px;">
            <div style="clear:both;"></div>
            <radG:RadGrid ID="MPFieldsGrid" runat="server" AutoGenerateColumns="False" EnableAJAX="True" Width="450px" GridLines="None" Skin="WebBlue" OnItemDataBound="MPFieldsGrid_ItemDataBound" OnNeedDataSource="MPFieldsGrid_NeedDataSource" AllowPaging="False" Height="271px">
                <MasterTableView TableLayout="Fixed" >
                    <PagerStyle Visible="False" />
                    <Columns>
                        <radG:GridBoundColumn AllowFiltering="False" AllowSorting="False" ConvertEmptyStringToNull="False"
                            DataField="Description" Groupable="False" HeaderText="Description" Reorderable="False"
                            ShowSortIcon="False" UniqueName="Description" DataFormatString="<nobr>{0}</nobr>">
                            <HeaderStyle Width="180px" />
                            <ItemStyle Wrap="False" />
                        </radG:GridBoundColumn>
                        <radG:GridBoundColumn AllowFiltering="False" AllowSorting="False" DataField="id"
                            DataType="System.Int32" Groupable="False" HeaderText="ID" Reorderable="False"
                            Resizable="False" ShowSortIcon="False" UniqueName="id" Visible="False">
                        </radG:GridBoundColumn>
                        <radG:GridBoundColumn AllowFiltering="False" AllowSorting="False" DataField="type" Groupable="False" HeaderText="Type" Reorderable="False"
                            Resizable="False" ShowSortIcon="False" UniqueName="type">
                            <ItemStyle Wrap="False" />
                        </radG:GridBoundColumn>
                        <radG:GridBoundColumn AllowFiltering="False" AllowSorting="False" DataField="MPFiledName" Groupable="False" HeaderText="Field Name (Mortg)" Reorderable="False"
                            Resizable="False" ShowSortIcon="False" UniqueName="MPFieldName">
                            <HeaderStyle Width="180px" />
                            <ItemStyle Wrap="False" />
                        </radG:GridBoundColumn>
                    </Columns>
                    <ExpandCollapseColumn Visible="False" Resizable="False"/>
                    <RowIndicatorColumn Visible="False"/>
                </MasterTableView>
                
                <ItemStyle Wrap="False" Height="20px"/>
                <AlternatingItemStyle Wrap="False" Height="20px"/>
                <ClientSettings EnableClientKeyValues="True">
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                    <Selecting AllowRowSelect="True" />
                    <Resizing AllowColumnResize="True" />
                    <ClientEvents OnRowSelected="MPFieldsGrid_RowSelected" />
                </ClientSettings>
            </radG:RadGrid>
            </div>
        </td>
        
        <td>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" >
                <tr>
                    <td align="center">
                        <asp:Label ID="lblActionInfoArea" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnBound" runat="server" Text="Assign" Enabled="true" Width="80px" OnClick="btnBound_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnUnBound" runat="server" Text="UnAssign" Enabled="true" Width="80px" OnClick="btnUnBound_Click" />
                    </td>
                </tr>
            </table>
        </td>
        
        <td align="left" valign="top" style="width:40%;">
            <div style="padding: 10px 6px 20px 6px;">
            <div style="clear:both;"></div>
            <radG:RadGrid ID="DTUnBoundFieldsGrid" runat="server" AutoGenerateColumns="False" Width="450px" GridLines="None" Skin="WebBlue" AllowMultiRowSelection="True" OnItemDataBound="DTUnBoundFieldsGrid_ItemDataBound" OnNeedDataSource="DTUnBoundFieldsGrid_NeedDataSource" EnableAJAX="True" AllowPaging="False" Height="271px">
                <MasterTableView TableLayout="Fixed" >
                    <PagerStyle Visible="False" />
                    <Columns>
                        <radG:GridBoundColumn AllowFiltering="False" AllowSorting="False" DataField="DTVFieldName"
                            Groupable="False" HeaderText="Field Name (Doc)" Reorderable="False" ShowSortIcon="False"
                            UniqueName="DTVFieldName" DataFormatString="<nobr>{0}</nobr>">
                            <HeaderStyle Width="230px" />
                        </radG:GridBoundColumn>
                        <radG:GridBoundColumn AllowFiltering="False" AllowSorting="False" DataField="ID"
                            DataType="System.Int32" Groupable="False" HeaderText="ID" Reorderable="False"
                            Resizable="False" ShowSortIcon="False" UniqueName="ID" Visible="False">
                        </radG:GridBoundColumn>
                        <radG:GridBoundColumn AllowFiltering="False" AllowSorting="False" DataField="Type"
                            Groupable="False" HeaderText="Type" Reorderable="False" Resizable="False" ShowSortIcon="False"
                            UniqueName="Type">
                        </radG:GridBoundColumn>
                    </Columns>
                    <ExpandCollapseColumn Visible="False" Resizable="False"/>
                    <RowIndicatorColumn Visible="False"/>
                </MasterTableView>

                <ItemStyle Wrap="False" Height="20px"/>
                <AlternatingItemStyle Wrap="False" Height="20px"/>
                <ClientSettings EnableClientKeyValues="True">
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                    <Selecting AllowRowSelect="True" />
                    <Resizing AllowColumnResize="True" />
                    <ClientEvents OnRowSelected="DTUnBoundFieldsGrid_RowSelected" />
                </ClientSettings>
            </radG:RadGrid>
            </div>
        </td>
    </tr>
    
    <tr>
        <td valign="middle" align="center" colspan="3">
            <radG:RadGrid ID="DTBoundFieldsGrid" runat="server" AutoGenerateColumns="False" Width="99%" GridLines="None" Skin="WebBlue" AllowSorting="True" AllowMultiRowSelection="True" OnItemDataBound="DTBoundFieldsGrid_ItemDataBound" OnNeedDataSource="DTBoundFieldsGrid_NeedDataSource" EnableAJAX="True" AllowPaging="False" Height="271px">
                <MasterTableView AllowMultiColumnSorting="True" TableLayout="Fixed" >
                    <PagerStyle Visible="False" />
                    <ExpandCollapseColumn Visible="False" Resizable="False"/>
                    <RowIndicatorColumn Visible="False"/>
                    <Columns>
                        <radG:GridBoundColumn AllowFiltering="False" DataField="GroupName" Groupable="False"
                            HeaderText="Group Name" Reorderable="False" 
                            UniqueName="GroupName" DataFormatString="<nobr>{0}</nobr>">
                            <HeaderStyle Width="150px" />
                        </radG:GridBoundColumn>
                        <radG:GridBoundColumn AllowFiltering="False" DataField="MPFiledName" Groupable="False"
                            HeaderText="Filed Name (Mortg)" Reorderable="False" 
                            UniqueName="MPFieldName" DataFormatString="<nobr>{0}</nobr>">
                        </radG:GridBoundColumn>
                        <radG:GridBoundColumn AllowFiltering="False" DataField="DTVFieldName"
                            Groupable="False" HeaderText="Field Name (Doc)" Reorderable="False"
                            UniqueName="DTVFieldName" DataFormatString="<nobr>{0}</nobr>">
                        </radG:GridBoundColumn>
                        <radG:GridBoundColumn AllowFiltering="False" AllowSorting="False" DataField="ID"
                            DataType="System.Int32" Groupable="False" HeaderText="ID" Reorderable="False"
                            Resizable="False" ShowSortIcon="False" UniqueName="ID" Visible="False">
                        </radG:GridBoundColumn>
                        <radG:GridBoundColumn AllowFiltering="False" AllowSorting="False" DataField="Type"
                            Groupable="False" HeaderText="Type" Reorderable="False" Resizable="False" ShowSortIcon="False"
                            UniqueName="Type">
                            <HeaderStyle Width="60px" />
                        </radG:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                
                <ItemStyle Wrap="False" Height="20px"/>
                <AlternatingItemStyle Wrap="False" Height="20px"/>
                <ClientSettings EnableClientKeyValues="True">
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                    <Selecting AllowRowSelect="True" />
                    <Resizing AllowColumnResize="True" />
                    <ClientEvents OnRowSelected="DTBoundFieldsGrid_RowSelected" />
                </ClientSettings>
            </radG:RadGrid>
        </td>
    </tr>
</table>