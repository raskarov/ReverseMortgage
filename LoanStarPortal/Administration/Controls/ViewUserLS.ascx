<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewUserLS.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewUserLS" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<script language="javascript" type="text/javascript">
<!--
function ClearFilter( ){
    document.getElementById('<%= tbLogin.ClientID %>').value = '';
    document.getElementById('<%= tbFirstName.ClientID %>').value = '';
    document.getElementById('<%= tbLastName.ClientID %>').value = '';
    document.getElementById('<%= ddlStatus.ClientID %>').value = 0;        
}
-->
</script>

<table width="100%" border="0" cellspacing="0" cellpadding="5">
	<tr>
	    <td valign="top" class="cssGridCtl">
	        <table border="0" cellpadding="0" cellspacing="0" width="100%">
	            <tr>
	                <td><asp:HyperLink Runat="server" ID="addLink" CssClass="cssLink" NavigateUrl="#">Add Administrator</asp:HyperLink></td>
                </tr>	        
                <tr>
                    <td>
                        <div style="border:solid 1px black; padding-left:3px; padding-top:3px; padding-bottom:3px">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr><td colspan="6" align="center"><asp:Label ID="Label6" runat="server" Text="Search conditions:" SkinID="LoginLabel"></asp:Label></td></tr>
                            <tr>
                                <td class="filterlabeltd">
                                    <asp:Label ID="Label3" runat="server" Text="Login:" SkinID="LoginLabel"></asp:Label>
                                </td>                            
                                <td class="filterdatatd">
                                    <asp:TextBox ID="tbLogin" runat="server" SkinID="FilterInput"></asp:TextBox>
                                </td>                            
                                <td class="filterlabeltd">
                                    <asp:Label ID="Label1" runat="server" Text="First Name:" SkinID="LoginLabel"></asp:Label>
                                </td>
                                <td class="filterdatatd">
                                    <asp:TextBox ID="tbFirstName" runat="server" SkinID="FilterInput"></asp:TextBox>
                                </td>
                                <td class="filterlabeltd">
                                    <asp:Label ID="Label2" runat="server" Text="Last Name:" SkinID="LoginLabel"></asp:Label>
                                </td>
                                <td class="filterdatatd">
                                    <asp:TextBox ID="tbLastName" runat="server" SkinID="FilterInput"></asp:TextBox>
                                </td>                                
                            </tr>
                            <tr>
                                <td class="filterlabeltd">
                                    <asp:Label ID="Label5" runat="server" Text="Status:"  SkinID="LoginLabel" ></asp:Label>
                                </td>
                                <td class="filterdatatd">
                                    <asp:DropDownList ID="ddlStatus" runat="server" SkinID="AdminSelect1"  Width="106px"></asp:DropDownList>
                                </td>                                
                                <td class="filterlabeltd">&nbsp;</td>
                                <td class="filterdatatd">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr style="padding-top:5px;">
                                <td>&nbsp;</td>
                                <td><asp:Button ID="btnSearch" runat="server" OnClick="Search_Click" Text="Search" /></td>                                
                                <td>&nbsp;</td>
                                <td>
                                    <input id="Button1" type="button" value="Reset" onclick="ClearFilter();"/>
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                        </div>
                    </td>
                </tr>
	        </table>
        </td>
    </tr>
    <tr>
        <td>      
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">                        	            
            <ContentTemplate>          
            <radG:RadGrid ID="G" runat="server" AllowPaging="false" AllowSorting="True" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" OnItemCommand="G_ItemCommand" OnItemDataBound="G_ItemDataBound" OnSortCommand="G_SortCommand" OnPageIndexChanged="G_PageIndexChanged" >
            <ClientSettings>
            <Resizing AllowColumnResize="True" EnableRealTimeResize="True"/>
            </ClientSettings>
                <MasterTableView AllowNaturalSort="False">
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Login" SortExpression="login" >
                            <ItemTemplate>
                                <asp:Label ID="lblLogin" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Login") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="First Name"  SortExpression="firstname">
                            <ItemTemplate>
                                <asp:Label ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FirstName") %>'></asp:Label>
                            </ItemTemplate>
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Last Name" SortExpression="LastName">
                            <ItemTemplate>
                                <asp:Label ID="lblLastName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LastName") %>'></asp:Label>
                            </ItemTemplate>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Status" Resizable="False"  SortExpression="StatusName" >
                            <ItemTemplate>
                                <asp:LinkButton ID="ChangeStatus" runat="server">LinkButton</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="60px" />
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Action" Resizable="False">
                            <ItemTemplate>
                                <asp:ImageButton  id="btnEdit" ImageUrl="~/images/btn_grd_edit.gif" CommandName="edit" AlternateText="Edit user" Runat="server" />
                                <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="Delete" AlternateText="Delete user" Runat="server"/>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                        </radG:GridTemplateColumn>                        
                    </Columns>
                </MasterTableView>
                <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
            </radG:RadGrid>            
            </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>            
	    </td>
    </tr>
</table>



