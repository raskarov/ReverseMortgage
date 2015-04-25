<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditRuleField.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditRuleField" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<table width="100%" border="0" cellpadding="0" cellspacing="0" >
<tr>
    <td align="center">
        <asp:Label ID="lblHeader" runat="server" Text="Manage field in rule" SkinID="AdminHeader"></asp:Label>
    </td>
</tr>
<tr>
    <td align="center" runat="server" id="tdruleexp">&nbsp;</td>
</tr>
<tr>
    <td align="center">
        <asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
    </td>
</tr>
<tr>
    <td style="padding-top:5px">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" >
            <tr>
                <td style="width:80px;padding-right:3px" align="right">
                    <asp:Label ID="Label1" runat="server" Text="Select group" SkinID="AdminLabel"></asp:Label>
                </td>    
                <td style="width:120px">
                    <asp:DropDownList ID="ddlGroup" runat="server" Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged"></asp:DropDownList>
                </td>
                <td style="width:80px;padding-right:3px" align="right">
                    <asp:Label ID="Label5" runat="server" Text="Select field" SkinID="AdminLabel"></asp:Label>                    
                </td>
                <td style="width:120px">
                    <asp:DropDownList ID="ddlField" runat="server" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="ddlField_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
            <tr style="padding-top:5px;padding-bottom:5px">
                <td>&nbsp;</td>
                <td align="right"><asp:Button ID="btnCancel" runat="server" Text="Back" SkinID="AdminButton" OnClick="btnCancel_Click" CausesValidation="False"/></td>
                <td>&nbsp;</td>
                <td align="right"><asp:Button ID="btnAdd" runat="server" Text="Add"  SkinID="AdminButton" OnClick="btnAdd_Click"/></td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </td>
</tr>
<tr>
    <td>
        <radG:RadGrid ID="G" runat="server" Width="99%" Height="99%" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" AutoGenerateColumns="false"  AllowPaging="True" PageSize="6" OnItemCommand="G_ItemCommand" OnPageIndexChanged="G_PageIndexChanged" OnItemDataBound="G_ItemDataBound">
                <MasterTableView AllowNaturalSort="False">
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Field">
                            <ItemTemplate>
                                <asp:Label ID="lblstatus1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ObjectName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="200px"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Action">
                            <ItemTemplate>
                                <asp:Label ID="lblaction" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ActionName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px"/>
                        </radG:GridTemplateColumn>                        
                         <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Delete" Resizable="False">
                            <ItemTemplate>
                                <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="delete" AlternateText="Delete field " Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>
                    </Columns>
                    <ExpandCollapseColumn Visible="False">
                        <HeaderStyle Width="19px" />
                    </ExpandCollapseColumn>
                    <RowIndicatorColumn Visible="False">
                        <HeaderStyle Width="20px" />
                    </RowIndicatorColumn>
                </MasterTableView>        
            <PagerStyle HorizontalAlign="Left" Mode="NumericPages" PagerTextFormat="" />
        </radG:RadGrid>            
    </td>
</tr>
<tr style="padding-top:5px">
    <td align="center">
        <asp:Button ID="btnBack" runat="server" Text="Close" OnClick="btnBack_Click" SkinID="AdminButton" />
    </td>
</tr>
</table>

