<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewRole.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewRole" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>

<table width="100%" border="0" cellspacing="0" cellpadding="5">
<!--	
	<tr>
	    <td valign="top" class="cssGridCtl">
	        <table border="0" cellpadding="0" cellspacing="0" width="100%">
	            <tr>
	                <td><asp:HyperLink Runat="server" ID="addLink" CssClass="cssLink" NavigateUrl="#">Add Role</asp:HyperLink></td>
                </tr>	        
	        </table>
        </td>
    </tr>
-->    
    <tr>
        <td>      
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">                        	            
            <ContentTemplate>
            <radG:RadGrid ID="G" runat="server" AllowPaging="False" AllowSorting="True" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" OnItemCommand="G_ItemCommand" OnItemDataBound="G_ItemDataBound" OnSortCommand="G_SortCommand" OnPageIndexChanged="G_PageIndexChanged" ShowStatusBar="True" >
            <ClientSettings>
            <Resizing AllowColumnResize="True" EnableRealTimeResize="True"/>
            </ClientSettings>
                <MasterTableView AllowNaturalSort="False">
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Role name" SortExpression="name" >
                            <ItemTemplate>
                                <asp:Label ID="lblRoleName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Action" Resizable="False">
                            <ItemTemplate>
                             <asp:ImageButton  id="btnEdit" ImageUrl="~/images/btn_grd_edit.gif" CommandName="edit" AlternateText="Edit role" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>
<!--                             <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="delete" AlternateText="Delete role" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />-->
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
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
                <PagerStyle HorizontalAlign="Left" Mode="NumericPages" PagerTextFormat="" ShowPagerText="False" />
            </radG:RadGrid>            
            </ContentTemplate>
            </asp:UpdatePanel>            
	    </td>
    </tr>
</table>
<div style="margin-bottom:50px"></div>