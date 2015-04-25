<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewAffiliates.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewAffiliates" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<table border="0" cellpadding="0" cellspacing="0" style="width:100%">
    <tr>
        <td align="center">
            <asp:Label ID="lblCompany" runat="server" Text="Label" SkinID="AdminHeader"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <radG:RadGrid ID="G" runat="server" AllowPaging="True" AllowSorting="True" EnableAJAX="False" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="95%" GroupingEnabled="False" OnItemCommand="G_ItemCommand" OnItemDataBound="G_ItemDataBound">
                <MasterTableView CommandItemDisplay="Bottom" EditMode="InPlace">
                    <ExpandCollapseColumn Visible="False">
                    <HeaderStyle Width="19px"></HeaderStyle>
                    </ExpandCollapseColumn>
                    <RowIndicatorColumn Visible="False">
                    <HeaderStyle Width="20px"></HeaderStyle>
                    </RowIndicatorColumn>                    
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Company Role" SortExpression="CompanyRole">
                            <ItemTemplate>
                                <asp:Label ID="lblCompanyType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyRole") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged"/>
                            </EditItemTemplate>                            
                            <HeaderStyle HorizontalAlign="Left" Width="20%"/>
                        </radG:GridTemplateColumn>                    
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Company" SortExpression="CompanyName">
                            <ItemTemplate>
                                <asp:Label ID="lblCompanyName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlCompany" runat="server" Width="50%"/>
                            </EditItemTemplate>                            
                            <HeaderStyle HorizontalAlign="Left" Width="70%"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Action">
                            <ItemTemplate>
                                <asp:ImageButton  id="btnEdit" ImageUrl="~/images/btn_grd_edit.gif" CommandName="Edit" CommandArgument='<%# Eval("Id") %>' AlternateText="Edit affiliate" Runat="server" />
                                <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="deleteaffiliate" CommandArgument='<%# Eval("Id") %>' AlternateText="Delete affiliate" Runat="server"/>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton  id="btnUpdate" ImageUrl="~/images/Update.gif" CommandName="update" CommandArgument='<%# Eval("Id") %>' AlternateText="Update" Runat="server" />
                                <asp:ImageButton  id="btnCancel" ImageUrl="~/images/Cancel.gif" CommandName="cancel" AlternateText="Cancel" Runat="server"/>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>                        
                    </Columns>
                    <CommandItemTemplate>
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" runat="server" id="cmdtable" visible='<%# ViewMode %>'>
                            <tr>
                                <td style="width:18px">
                                    <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="Add affiliate" BorderWidth="0" CommandName="addaffiliate" ImageUrl="~/RadControls/Grid/Skins/WebBlue/AddRecord.gif" Visible="true" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="InitInsert" Visible="true">Add affiliate</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                 </CommandItemTemplate>                    
                </MasterTableView>
                <PagerStyle Mode="NumericPages" PagerTextFormat="" />
            </radG:RadGrid>
        
        </td>
    </tr>
</table>
