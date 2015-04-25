<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditRuleCondition.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditRuleCondition" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
<tr>
    <td align="center">
        <asp:Label ID="lblHeader" runat="server" Text="Manage Checklist for rule" SkinID="AdminHeader"></asp:Label>
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
    <td valign="top">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="width:190px;padding-right:3px; height: 24px;" align="right">
                    <asp:Label ID="Label1" runat="server" Text="Title:" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:300px; height: 24px;">
                    <asp:TextBox ID="tbTitle" runat="server" Text="" Width="95%" MaxLength="100" ></asp:TextBox>
                </td>
                <td align="left"><asp:Label ID="tbTitleerr" runat="server" ForeColor="red"></asp:Label></td>
            </tr>
            <tr>
                <td style="padding-right:3px" align="right">
                    <asp:Label ID="Label4" runat="server" Text="Detail:" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:300px">
                    <asp:TextBox ID="tbDetail" runat="server" Text="" Width="95%" MaxLength="256" ></asp:TextBox>
                </td>
                <td align="left"><asp:Label ID="tbDetailerr" runat="server" ForeColor="red"></asp:Label></td>
            </tr>                            
            <tr>                
                <td style="padding-right:3px" align="right">
                    <asp:Label ID="Label5" runat="server" Text="Minimal authority level:" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:300px">
                    <asp:DropDownList ID="ddlRole" runat="server" SkinID="AdminSelect" AutoPostBack="False" Width="150px" ></asp:DropDownList>
                </td>
                <td align="left"><asp:Label ID="ddlRoleerr" runat="server" ForeColor="red"></asp:Label></td>
            </tr>
            <tr style="padding-top:5px;">
                <td>&nbsp;</td>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="left"><asp:Button ID="btnCancel" runat="server" Text="Cancel"  SkinID="AdminButton" OnClick="btnCancel_Click" CausesValidation="False"/></td>
                            <td align="right" style="padding-right:10px;"><asp:Button ID="btnSave" runat="server" Text="Save"  SkinID="AdminButton" OnClick="btnSave_Click" CausesValidation="False"/></td>                
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>    
    </td>
</tr>
<tr style="padding-top:5px;">
    <td>
            <radG:RadGrid ID="G" runat="server" AllowPaging="false" AllowSorting="False" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" OnItemCommand="G_ItemCommand" OnItemDataBound="G_ItemDataBound">
            <ClientSettings>
            <Resizing AllowColumnResize="True" EnableRealTimeResize="false"/>
            </ClientSettings>
                <MasterTableView AllowNaturalSort="False">
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Title">
                            <ItemTemplate>
                                <asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="200px"/>
                        </radG:GridTemplateColumn>                    
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Detail">
                            <ItemTemplate>
                                <asp:Label ID="lblDetail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Detail") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="200px"/>
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Role">
                            <ItemTemplate>
                                <asp:Label ID="lblRole" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RoleName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="140px"/>
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Action" Resizable="False" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton  id="btnEdit" ImageUrl="~/images/btn_grd_edit.gif" CommandName="editcondition" AlternateText="Edit user" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="delete" AlternateText="Delete role" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                        </radG:GridTemplateColumn>                        
                    </Columns>
                </MasterTableView>
                <PagerStyle HorizontalAlign="Left" Mode="NumericPages" PagerTextFormat="" />
            </radG:RadGrid>    
    </td>
</tr>
<tr style="padding-top:5px;">
    <td align="center">
        <asp:Button ID="btnClose" runat="server" Text="Close"  SkinID="AdminButton" OnClick="btnClose_Click"/>
    </td>
</tr>
</table>
