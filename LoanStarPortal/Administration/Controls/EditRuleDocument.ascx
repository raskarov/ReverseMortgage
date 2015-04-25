<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditRuleDocument.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditRuleDocument" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<script language="javascript" type="text/javascript" defer="defer">
function AddDocTemplate()
{
   var selDTID = document.getElementById('<%=ddlSelectDoc.ClientID %>').value;
    var selDTGroupID = document.getElementById('<%=ddlSelectDocGroup.ClientID %>').value;
    var isValid = true;
    
    if(selDTID == 0)
    {
        document.getElementById('<%=lblSelectDocErr.ClientID %>').innerHTML = '*';
        isValid = false;
    }
    else
        document.getElementById('<%=lblSelectDocErr.ClientID %>').innerHTML = '';
        
    if(selDTGroupID == 0)
    {
        document.getElementById('<%=lblSelectDocGroupErr.ClientID %>').innerHTML = '*';
        isValid = false;
    }
    else
        document.getElementById('<%=lblSelectDocGroupErr.ClientID %>').innerHTML = '';
        
    document.getElementById('<%=lblActionInfoArea.ClientID %>').innerHTML = '';
        
    return isValid;
}

function ddlSelectDoc_OnChange()
{
    var groupID = document.getElementById('<%=ddlSelectDoc.ClientID %>').value.split(';')[1];
    document.getElementById('<%=ddlSelectDocGroup.ClientID %>').value = groupID;
}
</script>

<table width="100%" border="0" cellspacing="0" cellpadding="0" style="table-layout:fixed" >
    <tr>
        <td align="left" style="width:80%">

            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="table-layout:fixed" >
                <tr>
                    <td style="width:150px"></td>
                    <td align="center">
                        <asp:Label ID="lblHeader" runat="server" Text="Add document for rule" SkinID="AdminHeader"></asp:Label>
                    </td>
                    <td style="width:10px"></td>
                </tr>
                <tr>
                    <td style="width:150px"></td>
                    <td align="center" runat="server" id="tdruleexp">&nbsp;</td>
                    <td style="width:10px"></td>
                </tr>
                
                <tr>
                </tr>
                
                <tr>
                    <td align="left" style="width:150px">
                        <asp:Label ID="lblSelectDoc" runat="server" Text="Select document:"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlSelectDoc" Width="100%" runat="server" OnChange="ddlSelectDoc_OnChange();" ></asp:DropDownList>
                    </td>
                    <td align="left" style="width:10px">
                        <asp:Label ID="lblSelectDocErr" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td align="left" valign="middle" colspan="2">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0" >
                            <tr>
                                <td align="left" >
                                    <asp:CheckBox ID="cbAppPackage" runat="server" Text="Application Package" />
                                </td>
                                <td align="left" >
                                    <asp:CheckBox ID="cbClosPackage" runat="server" Text="Closing Package" />
                                </td>
                                <td align="left" >
                                    <asp:CheckBox ID="cbMiscPackage" runat="server" Text="Misc. Package" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td></td>
                </tr>
                
                <tr>
                    <td align="left" >
                        <asp:Label ID="lblSelectDocGroup" runat="server" Text="Select document group:"></asp:Label>
                    </td>
                    <td align="left" >
                        <asp:DropDownList ID="ddlSelectDocGroup" Width="100%" runat="server" ></asp:DropDownList>
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblSelectDocGroupErr" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td align="right" colspan="2">
                        <asp:Label ID="lblActionInfoArea" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
                    </td>
                    <td></td>
                </tr>
                
                <tr>
                    <td align="right" colspan="2" >
                        <asp:Button ID="btnAdd" runat="server" Text="Add" Width="80px" OnClientClick="javascript:return AddDocTemplate();" OnClick="btnAdd_Click" />
                    </td>
                    <td></td>
                </tr>
            </table>

        </td>
        
        <td></td>
    </tr>

    <tr>
        <td align="left" colspan="2" >
            <asp:Label ID="lblAvailableDocuments" runat="server" Text="Current Available Document:"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="left" colspan="2" >
            <radG:RadGrid   ID="RadGridAssignedDocTemplates" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                            Width="99%" GridLines="None" OnItemCommand="RadGridAssignedDocTemplates_ItemCommand" 
                            OnSortCommand="RadGridAssignedDocTemplates_SortCommand" 
                            AllowPaging="True" PageSize="5" Skin="WebBlue">
                
                <MasterTableView EditMode="InPlace" DataKeyNames="DTRelID">
                    <ExpandCollapseColumn Visible="False">
                        <HeaderStyle Width="19px" />
                    </ExpandCollapseColumn>
                    <RowIndicatorColumn Visible="False">
                        <HeaderStyle Width="20px" />
                    </RowIndicatorColumn>
                    
                    <Columns>
                        <radG:GridBoundColumn AllowFiltering="False" AllowSorting="False" DataField="ID"
                            DataType="System.Int32" Groupable="False" HeaderText="ID" Reorderable="False"
                            Resizable="False" ShowSortIcon="False" UniqueName="ID" Visible="False">
                        </radG:GridBoundColumn>
                        <radG:GridBoundColumn AllowFiltering="False" AllowSorting="False" DataField="DTRelID"
                            DataType="System.Int32" Groupable="False" HeaderText="DTRelID" Reorderable="False"
                            Resizable="False" ShowSortIcon="False" UniqueName="DTRelID" Visible="False">
                        </radG:GridBoundColumn>
                        <radG:GridBoundColumn AllowFiltering="False" DataField="DTTitle"
                            Groupable="False" HeaderText="Document Name" Reorderable="False"
                            UniqueName="DTTitle" ReadOnly="True">
                        </radG:GridBoundColumn>
                        <radG:GridCheckBoxColumn AllowFiltering="False" DataField="IsAppPackage" DataType="System.Boolean" 
                            Groupable="False" HeaderText="Is Application Package" Reorderable="False" UniqueName="IsAppPackage">
                        </radG:GridCheckBoxColumn>
                        <radG:GridCheckBoxColumn AllowFiltering="False" DataField="IsClosingPackage" DataType="System.Boolean" 
                            Groupable="False" HeaderText="Is Closing Package" Reorderable="False" UniqueName="IsClosingPackage">
                        </radG:GridCheckBoxColumn>
                        <radG:GridCheckBoxColumn AllowFiltering="False" DataField="IsMiscPackage" DataType="System.Boolean" 
                            Groupable="False" HeaderText="Is Misc. Package" Reorderable="False" UniqueName="IsMiscPackage">
                        </radG:GridCheckBoxColumn>
                        <radG:GridDropDownColumn AllowFiltering="False" Groupable="False" 
                            HeaderText="Group" Reorderable="False" UniqueName="GroupDocList" DataSourceID="GroupDocList" 
                            ListTextField="GroupName" ListValueField="GroupID" DataField="GroupID" >
                        </radG:GridDropDownColumn>
                        <radg:GridEditCommandColumn ButtonType="ImageButton" 
                            EditImageUrl="~/Images/Edit.gif" UpdateImageUrl="~/Images/Update.gif" 
                            CancelImageUrl="~/Images/Cancel.gif" Reorderable="False" Resizable="False" >
                            <HeaderStyle Width="20px" /> 
                        </radg:GridEditCommandColumn>
                        <radG:GridTemplateColumn UniqueName="DeleteCommandColumn" Resizable="False" AllowFiltering="False" 
                            Groupable="False" Reorderable="False">
                            <ItemTemplate>
                                <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="Delete" AlternateText="Delete" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                            </ItemTemplate>
                            <HeaderStyle Width="20px" />
                        </radG:GridTemplateColumn>
                    </Columns>
                    <EditFormSettings>
                        <EditColumn UniqueName="EditCommandColumn1">
                        </EditColumn>
                    </EditFormSettings>
                </MasterTableView>
                
                <PagerStyle Mode="NumericPages" />
                <ClientSettings>
                    <Resizing AllowColumnResize="True" />
                </ClientSettings>
            </radG:RadGrid>
            <asp:SqlDataSource runat="server" ID="GroupDocList">
            </asp:SqlDataSource>
        </td>
    </tr>
    
    <tr>
        <td align="center" colspan="2" >
            <asp:Button ID="btnClose" runat="server" Text="Close" Width="80px" OnClick="btnClose_Click" />
        </td>
    </tr>
</table>