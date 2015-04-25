<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditRuleCheckList.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditRuleCheckList" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
<tr>
    <td align="center">
        <asp:Label ID="lblHeader" runat="server" Text="Manage Checklist for rule" SkinID="AdminHeader"></asp:Label>
    </td>
</tr>
<tr>
    <td align="center" runat="server" id="tdruleexp" >&nbsp;</td>
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
                <td style="width:150px">
                    <asp:Label ID="Label1" runat="server" Text="Status" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:280px">
                    <asp:Label ID="Label2" runat="server" Text="Question" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:50px" align="center">
                    <asp:Label ID="Label3" runat="server" Text="Yes" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:50px" align="center">
                    <asp:Label ID="Label4" runat="server" Text="No" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:50px" align="center">
                    <asp:Label ID="Label5" runat="server" Text="Don't know" SkinID="AdminLabel"></asp:Label>
                </td>                
                <td style="width:50px" align="center">
                    <asp:Label ID="Label6" runat="server" Text="To follow" SkinID="AdminLabel"></asp:Label>
                </td>
            </tr>
            <asp:Repeater ID="rpChecklist" runat="server" OnItemDataBound="rpChecklist_ItemDataBound">
            <ItemTemplate>
            <tr>
                <td style="width:150px">
                    <asp:CheckBox ID="cbStatus" statusid='<%# DataBinder.Eval(Container.DataItem,"id") %>' runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"Selected") %>' Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'  />
                </td>
                <td>
                    <asp:TextBox ID="tbQuestion" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Question") %>' Width="90%" MaxLength="256" ></asp:TextBox>
                    <asp:Label ID="lblerrq" runat="server" Text="" Font-Bold="true" ForeColor="red"></asp:Label>
                </td>
                <td style="width:50px" align="center">
                    <asp:CheckBox ID="cbYes" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"cbYes") %>'  />
                </td>
                <td style="width:50px" align="center">
                    <asp:CheckBox ID="cbNo" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"cbNo") %>'  />
                </td>
                <td style="width:50px" align="center">
                    <asp:CheckBox ID="cbDontKnow" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"cbDontKnow") %>'  />
                </td>
                <td style="width:50px" align="center">
                    <asp:CheckBox ID="cbToFollow" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"cbtoFollow") %>'  />
                    <asp:Label ID="lblerrcb" runat="server" Text="" Font-Bold="true" ForeColor="red"></asp:Label>
                </td>                
            </tr>
            </ItemTemplate>
            </asp:Repeater>
            <tr>            
                <td>&nbsp;</td>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="left">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel"  SkinID="AdminButton" OnClick="btnCancel_Click"/>
                            </td>
                            <td align="right" style="padding-right:26px;">
                                <asp:Button ID="btnSave" runat="server" Text="Save"  SkinID="AdminButton" OnClick="btnSave_Click"/>
                            </td>                            
                        </tr>
                    </table>                    
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>    
    </td>
</tr>
<tr style="padding-top:5px">
    <td>
            <radG:RadGrid ID="G" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" OnItemCommand="G_ItemCommand" OnItemDataBound="G_ItemDataBound" OnPageIndexChanged="G_PageIndexChanged" >
            <ClientSettings>
            <Resizing AllowColumnResize="True" EnableRealTimeResize="false"/>
            </ClientSettings>
                <MasterTableView AllowNaturalSort="False">
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Checklist">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# GetCheckListName(Container.DataItem,"Id") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="400px"/>
                        </radG:GridTemplateColumn>                    
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Action" Resizable="False" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton  id="btnEdit" ImageUrl="~/images/btn_grd_edit.gif" CommandName="editchecklist" AlternateText="Edit user" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
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
