<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewUserOriginator.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewUserOriginator" %>
<%@ Register Src="EditUserOriginator.ascx" TagName="EditUserOriginator" TagPrefix="uc2" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>
<%@ Register Assembly="RadUpload.NET2" Namespace="Telerik.WebControls" TagPrefix="radu" %>
<script language="javascript" type="text/javascript">
    function ClearFilter(){
        document.getElementById('<%= tbLogin.ClientID %>').value = '';
    document.getElementById('<%= tbFirstName.ClientID %>').value = '';
    document.getElementById('<%= tbLastName.ClientID %>').value = '';    
    document.getElementById('<%= ddlStatus.ClientID %>').value = 0;        
    document.getElementById('<%= ddlRole.ClientID %>').value = 0;
}
function ValidateLogin(src, arg ){
    arg.IsValid = arg.Value.length>6;
}
function ValidatePassword(src, arg ){
    if (document.getElementById(src.controltovalidate).getAttribute('disabled')){
        arg.IsValid = true;
    }else{    
        arg.IsValid = arg.Value.length>5&&arg.Value.length<17;
        if (arg.IsValid){
            var cnt = arg.Value.match(/[a-z]/ig);
            arg.IsValid = cnt.length>=5;
            if(arg.IsValid){
                cnt = arg.Value.match(/\d/ig);
                arg.IsValid = cnt.length>0;        
                if(arg.IsValid){
                    cnt = arg.Value.match(/^[a-z]\w+[a-z]$/ig);
                    arg.IsValid=cnt!=null;
                }
            }
        }
    }        
}
function ValidateEmailPassword(src, arg ){
    if (document.getElementById(src.controltovalidate).getAttribute('disabled')){
        arg.IsValid = true;
    }else{    
        arg.IsValid = arg.Value.length>5&&arg.Value.length<17;
        if (arg.IsValid){
            var cnt = arg.Value.match(/[a-z]/ig);
            arg.IsValid = cnt.length>=5;
            if(arg.IsValid){
                cnt = arg.Value.match(/\d/ig);
                arg.IsValid = cnt.length>0;        
                if(arg.IsValid){
                    cnt = arg.Value.match(/^[a-z]\w+[a-z]$/ig);
                    arg.IsValid=cnt!=null;
                }
            }
        }
    }        
}
function SetPassword(status,c1,c2){
    c1.setAttribute('disabled',!status);
    c2.setAttribute('disabled',!status);
    var c = status?'admininput':'admininputdis';
    c1.className=c;
    c2.className=c;
    if (status)
        c1.focus();
}
-->
</script>
<table width="100%" border="0" cellspacing="0" cellpadding="5" runat="server" id="tblContainer">
	<tr id="trFilter" runat="server">
	    <td valign="top" class="cssGridCtl">
	        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 94px">
                        <div style="border:solid 1px black; padding-left:3px; padding-top:3px; padding-bottom:3px">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td colspan="7" align="center">
                                    <asp:Label ID="Label6" runat="server" Text="Search conditions:" SkinID="LoginLabel"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="filterlabeltd">
                                    <asp:Label ID="Label3" runat="server" Text="User name:" SkinID="LoginLabel"></asp:Label>
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
                                <td>&nbsp;</td>                                
                            </tr>
                            <tr>
                                <td class="filterlabeltd">
                                    <asp:Label ID="Label5" runat="server" Text="Status:"  SkinID="LoginLabel" ></asp:Label>
                                </td>
                                <td class="filterdatatd">
                                    <asp:DropDownList ID="ddlStatus" runat="server" SkinID="AdminSelect1"  Width="106px"></asp:DropDownList>
                                </td>
                                <td class="filterlabeltd">
                                    <asp:Label ID="Label4" runat="server" Text="Role:"  SkinID="LoginLabel" ></asp:Label>
                                </td>
                                <td class="filterdatatd">
                                    <asp:DropDownList ID="ddlRole" runat="server" SkinID="AdminSelect1"  Width="106px"></asp:DropDownList>
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>                                
                            </tr>
                            <tr style="padding-top:8px">
                                <td colspan="7" align="center">
                                    <table cellpadding="0" cellspacing="0" border="0" width="50%">
                                        <tr>
                                            <td align="right"><input id="Button1" type="button" value="Reset" onclick="ClearFilter();" style="width:60px;" /></td>
                                            <td style="width:80px;">&nbsp;</td>
                                            <td align="left"><asp:Button ID="btnSearch" runat="server" OnClick="Search_Click" Text="Search" Width="60px"/></td>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
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
            <radG:RadGrid ID="G" runat="server" AllowPaging="False" AllowSorting="True" EnableAJAX="False" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" OnItemCommand="G_ItemCommand" OnItemDataBound="G_ItemDataBound" OnPageIndexChanged="G_PageIndexChanged" GroupingEnabled="False">
                <MasterTableView CommandItemDisplay="Bottom">
                    <ExpandCollapseColumn Visible="False">
                    <HeaderStyle Width="19px"></HeaderStyle>
                    </ExpandCollapseColumn>
                    <RowIndicatorColumn Visible="False">
                    <HeaderStyle Width="20px"></HeaderStyle>
                    </RowIndicatorColumn>                    
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="First Name" SortExpression="FirstName">
                            <ItemTemplate>
                                <asp:Label ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FirstName") %>'></asp:Label>                                
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Last Name" SortExpression="LastName">
                            <ItemTemplate>
                                <asp:Label ID="lblLastName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LastName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px"/>
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="User name" SortExpression="Login">
                            <ItemTemplate>
                                <asp:Label ID="lblLogin" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Login") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="AD">
                            <ItemTemplate>
                                <asp:Label ID="lblAd" runat="server" Text='<%# GetRole(Container.DataItem,"OriginatorAdminRole") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="LO">
                            <ItemTemplate>
                                <asp:Label ID="lblLO" runat="server" Text='<%# GetRole(Container.DataItem,"LoanOfficerRole") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="LM">
                            <ItemTemplate>
                                <asp:Label ID="lblLM" runat="server" Text='<%# GetRole(Container.DataItem,"LoanManagerRole") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="LA">
                            <ItemTemplate>
                                <asp:Label ID="lblLA" runat="server" Text='<%# GetRole(Container.DataItem,"LoanOfficerAssistantRole") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="OM">
                            <ItemTemplate>
                                <asp:Label ID="lblOM" runat="server" Text='<%# GetRole(Container.DataItem,"OperationsManagerRole") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="PM">
                            <ItemTemplate>
                                <asp:Label ID="lblPM" runat="server" Text='<%# GetRole(Container.DataItem,"ProcessingManagerRole") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="PR">
                            <ItemTemplate>
                                <asp:Label ID="lblPR" runat="server" Text='<%# GetRole(Container.DataItem,"ProcessorRole") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="UM">
                            <ItemTemplate>
                                <asp:Label ID="lblUM" runat="server" Text='<%# GetRole(Container.DataItem,"UnderwritingManagerRole") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="UW">
                            <ItemTemplate>
                                <asp:Label ID="lblUW" runat="server" Text='<%# GetRole(Container.DataItem,"UnderwriterRole") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="CM">
                            <ItemTemplate>
                                <asp:Label ID="lblCM" runat="server" Text='<%# GetRole(Container.DataItem,"ClosingManagerRole") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="CL">
                            <ItemTemplate>
                                <asp:Label ID="lblCL" runat="server" Text='<%# GetRole(Container.DataItem,"CloserRole") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="XR">
                            <ItemTemplate>
                                <asp:Label ID="lblXR" runat="server" Text='<%# GetRole(Container.DataItem,"PostCloserRole") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="XM">
                            <ItemTemplate>
                                <asp:Label ID="lblXM" runat="server" Text='<%# GetRole(Container.DataItem,"PostClosingManagerRole") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="EM">
                            <ItemTemplate>
                                <asp:Label ID="lblEM" runat="server" Text='<%# GetRole(Container.DataItem,"ExecutiveManagerRole") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Action">
                            <ItemTemplate>
                                <asp:ImageButton  id="btnEdit" ImageUrl="~/images/edit.gif" CommandName="edituser" CommandArgument='<%# Eval("Id") %>' AlternateText="Edit user" Runat="server" />
                                <asp:ImageButton  id="btnStatus" ImageUrl="~/images/disable.gif" CommandName="disable" CommandArgument='<%# Eval("Id") %>' AlternateText="Disable user" Runat="server"/>
                                <asp:ImageButton  id="imgDelete" ImageUrl="~/images/Delete.gif" CommandName="delete" CommandArgument='<%# Eval("Id") %>' AlternateText="Delete user" Runat="server"/>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>                        
                    </Columns>
                    <CommandItemTemplate>
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" runat="server" id="cmdtable">
                            <tr>
                                <td style="width:18px">
                                    <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="Add user" BorderWidth="0" CommandName="adduser" ImageUrl="~/RadControls/Grid/Skins/WebBlue/AddRecord.gif" Visible="true" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="adduser" Visible="true">Add user</asp:LinkButton>
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
<rada:RadAjaxManager id="RadAjaxManager1" runat="server" EnableOutsideScripts="True">
    <AjaxSettings>
        <radA:AjaxSetting AjaxControlID="btnSearch">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="G" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="btnBack">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="G" />
                <radA:AjaxUpdatedControl ControlID="tblContainer" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="btnSave">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="G" />
                <radA:AjaxUpdatedControl ControlID="tblContainer" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="G">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="tblContainer" />
            </UpdatedControls>
        </radA:AjaxSetting>
    </AjaxSettings>
</rada:RadAjaxManager>