<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewRoleTemplate.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewRoleTemplate" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>

<table width="100%" border="0" cellspacing="0" cellpadding="5">
    <tr>
        <td>      
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">                        	            
            <ContentTemplate>    
            <radG:RadGrid ID="G" runat="server" AllowPaging="false" AllowSorting="True" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" OnItemCommand="G_ItemCommand" OnSortCommand="G_SortCommand" OnPageIndexChanged="G_PageIndexChanged">
            <ClientSettings>
            <Resizing AllowColumnResize="True" EnableRealTimeResize="True"/>
            </ClientSettings>
                <MasterTableView AllowNaturalSort="False">
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Role name" SortExpression="Name" >
                            <ItemTemplate>
                                <asp:Label ID="lblRoleName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Action" Resizable="False">
                            <ItemTemplate>
                             <asp:ImageButton  id="btnEdit" ImageUrl="~/images/btn_grd_edit.gif" CommandName="edit" AlternateText="Edit role" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"/>
                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                        </radG:GridTemplateColumn>                        
                    </Columns>
                    <RowIndicatorColumn Visible="False">
                        <HeaderStyle Width="20px" />
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="False">
                        <HeaderStyle Width="19px" />
                    </ExpandCollapseColumn>
                </MasterTableView>
                <PagerStyle HorizontalAlign="Left" Mode="NumericPages" PagerTextFormat="" />
            </radG:RadGrid>                  
            </ContentTemplate>
            </asp:UpdatePanel>            
	    </td>
    </tr>
</table>
