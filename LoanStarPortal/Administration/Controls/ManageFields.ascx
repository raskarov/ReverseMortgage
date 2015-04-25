<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageFields.ascx.cs" Inherits="LoanStarPortal.Administration.ManageFields" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>
<script language="javascript" type="text/javascript">
<!--
function ClearFilter( ){
    document.getElementById('<%= tbLabel.ClientID %>').value = '';
    document.getElementById('<%= ddlGroup.ClientID %>').value = 0;
    var d=document.getElementById('<%= ddlField.ClientID %>');
    d.value=0;
    d.setAttribute('disabled','disabled');
    document.getElementById('<%= ddlTab1.ClientID %>').value = 0;
    d=document.getElementById('<%= ddlTab2.ClientID %>');
    d.value=0;
    d.setAttribute('disabled','disabled');
    d=document.getElementById('<%= ddlPseudoTab.ClientID %>');
    d.value=0;
    d.setAttribute('disabled','disabled');
}
-->
</script>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <div style="border:solid 1px black; padding-left:3px; padding-top:3px; padding-bottom:3px">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="7" align="center"><asp:Label ID="Label6" runat="server" Text="Search conditions:" SkinID="LoginLabel"></asp:Label></td>
                    </tr>
                    <tr style="padding-top:5px">
                        <td style="width:110px">
                            <asp:Label ID="lblGroup" runat="server" Text="Select group:" SkinID="LoginLabel"></asp:Label>
                        </td>
                        <td style="width:160px">
                            <asp:DropDownList ID="ddlGroup" runat="server" Width="95%" AutoPostBack="True" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td style="width:110px">
                            <asp:Label ID="lblField" runat="server" Text="Select field:" SkinID="LoginLabel"></asp:Label>
                        </td>
                        <td style="width:240px">
                            <asp:DropDownList ID="ddlField" runat="server" Width="95%" OnSelectedIndexChanged="ddlField_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td style="width:110px">
                            <asp:Label ID="lblLabel" runat="server" Text="Label:" SkinID="LoginLabel"></asp:Label>
                        </td>
                        <td style="width:240px">
                            <asp:TextBox ID="tbLabel" runat="server" MaxLength="200" Width="97%" AutoPostBack="True"></asp:TextBox>
                        </td> 
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTab1" runat="server" Text="Select tab level 1:" SkinID="LoginLabel"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTab1" runat="server" AutoPostBack="True" Width="95%" OnSelectedIndexChanged="ddlTab1_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblTab2" runat="server" Text="Select tab level 2:" SkinID="LoginLabel"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTab2" runat="server" AutoPostBack="True" Width="95%" OnSelectedIndexChanged="ddlTab2_SelectedIndexChanged"></asp:DropDownList>
                        </td>                        
                        <td>
                            <asp:Label ID="lblPseudoTab" runat="server" Text="Select pseudotab:" SkinID="LoginLabel"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPseudoTab" runat="server" Width="100%"></asp:DropDownList>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr style="padding-top:5px">
                        <td colspan="3" align="center">
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" width="95%">
                                <tr>
                                    <td align="left"><asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" /></td>
                                    <td align="right"><input id="Button1" type="button" value="Reset" onclick="ClearFilter();return false;"/></td>
                                </tr>
                            </table>
                        </td>
                        <td colspan="3" align="center">
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td style="padding-top:5px;">
            <div runat="server" id="griddiv">
            <radG:RadGrid ID="G" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" PageSize="500" OnPageIndexChanged="G_PageIndexChanged" OnSortCommand="G_SortCommand" OnItemCommand="G_ItemCommand" EnableAJAX="false">
            <ClientSettings>
            <Resizing AllowColumnResize="True" EnableRealTimeResize="True"/>
            </ClientSettings>
                <MasterTableView AllowNaturalSort="False">
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Field" SortExpression="PropertyName" >
                            <ItemTemplate>
                                <asp:Label ID="lblPropertyName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PropertyName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Label"  SortExpression="description">
                            <ItemTemplate>
                                <asp:Label ID="lblLabel" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="390px"/>
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Control" SortExpression="controlname">
                            <ItemTemplate>
                                <asp:Label ID="lblcontrolname" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "controlname") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Value type" SortExpression="valuetype">
                            <ItemTemplate>
                                <asp:Label ID="lblvaluetype" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"valuetype") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Location" SortExpression="location" >
                            <ItemTemplate>
                                <asp:Label ID="lbllocation" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"location") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Action" Resizable="False" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton  id="btnEdit" ImageUrl="~/images/btn_grd_edit.gif" CommandName="edit" CommandArgument='<%# GetId(Container.DataItem) %>' AlternateText="View field info" Runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px"/>
                        </radG:GridTemplateColumn>                        
                    </Columns>
                    <ExpandCollapseColumn Visible="False">
                        <HeaderStyle Width="19px" />
                    </ExpandCollapseColumn>
                    <RowIndicatorColumn Visible="False">
                        <HeaderStyle Width="20px" />
                    </RowIndicatorColumn>
                </MasterTableView>
                <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
            </radG:RadGrid>
            </div>
        </td>
    </tr>
</table>
<rada:RadAjaxManager id="RadAjaxManager1" runat="server" EnableOutsideScripts="True" EnablePageHeadUpdate="False">
    <AjaxSettings>
        <radA:AjaxSetting AjaxControlID="ddlGroup">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="ddlField" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="ddlTab1">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="ddlTab2" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="ddlTab2">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="ddlPseudoTab" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="btnSearch">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="griddiv" />
            </UpdatedControls>
        </radA:AjaxSetting>
    </AjaxSettings>
</rada:RadAjaxManager>    
