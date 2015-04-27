<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewCompany.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewCompany" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<script language="javascript" type="text/javascript">

function ClearFilter( ){
    document.getElementById('<%= tbCompany.ClientID %>').value = '';
}

</script>
</style>
<table width="100%" border="0" cellspacing="0" cellpadding="5">
	<tr>
	    <td valign="top" class="cssGridCtl">
	        <table border="0" cellpadding="0" cellspacing="0" width="100%">
	            <tr>
	                <td><asp:HyperLink Runat="server" ID="addLink" CssClass="cssLink" NavigateUrl="#">Add Company</asp:HyperLink></td>
                </tr>	        
                <tr>
                    <td>
                        <div style="border:solid 1px black; padding-left:3px; padding-top:3px; padding-bottom:3px">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:Label ID="Label6" runat="server" Text="Search conditions:" SkinID="LoginLabel"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:200px;padding-right:3px" align="right">
                                    <asp:Label ID="Label1" runat="server" Text="Company:" SkinID="LoginLabel"></asp:Label>
                                </td>
                                <td style="width:200px">
                                    <asp:TextBox ID="tbCompany" runat="server" Width="200px"></asp:TextBox>
                                </td>                                
                                <td>&nbsp;</td>
                                <td>Include Archived: <asp:CheckBox ID="cbArchived" AutoPostBack="true" runat="server" Text="" OnCheckedChanged="cbArchived_CheckedChanged" /></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left">
                                                <asp:Button ID="btnSearch" runat="server" OnClick="Search_Click" Text="Search" />
                                            </td>
                                            <td align="right">
                                                <input id="Button1" type="button" value="Reset" onclick="ClearFilter();"/>
                                            </td>                                            
                                        </tr>
                                    </table>
                                </td>
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
            	    </td>
    </tr>
</table>    
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">                        	            
            <ContentTemplate>          
            <radG:RadGrid ID="G" runat="server" AllowPaging="False" AllowSorting="True" AutoGenerateColumns="false" Width="100%" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" OnItemCommand="G_ItemCommand" OnItemDataBound="G_ItemDataBound" OnSortCommand="G_SortCommand" OnPageIndexChanged="G_PageIndexChanged" >
                <ClientSettings>
                    <Resizing AllowColumnResize="True" EnableRealTimeResize="True"/>
                </ClientSettings>
                <MasterTableView AllowNaturalSort="False" Width="100%" >
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Company" SortExpression="company" >
                            <ItemTemplate>
                                <asp:Label ID="lblCompany" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Company") %>' ></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Login" ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:LinkButton ID="originatorlogin" runat="server" CommandName="originatorlogin" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' >Login as company</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="120px"/>
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Action" Resizable="False">
                            <ItemTemplate>
                             <asp:ImageButton  id="btnEdit" ImageUrl="~/images/btn_grd_edit.gif" CommandName="edit" AlternateText="Edit company" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>
                             <asp:ImageButton  id="btnDisable" ImageUrl="~/images/btn_grd_delete.gif" CommandName="changestatus" AlternateText="Disable company" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>                        
                    </Columns>
                </MasterTableView>
                <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
            </radG:RadGrid>            
            </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="cbArchived" EventName="CheckedChanged" />
                </Triggers>            
            </asp:UpdatePanel>            

<div style="margin-bottom:50px"></div>



