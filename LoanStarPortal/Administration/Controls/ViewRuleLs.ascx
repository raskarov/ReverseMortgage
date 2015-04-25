<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewRuleLs.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewRuleLs" %>
<%@ Register Src="EditRuleCode.ascx" TagName="EditRuleCode" TagPrefix="uc9" %>
<%@ Register Src="EditRuleDocument.ascx" TagName="EditRuleDocument" TagPrefix="uc8" %>
<%@ Register Src="EditRuleTask.ascx" TagName="EditRuleTask" TagPrefix="uc7" %>
<%@ Register Src="EditRuleData.ascx" TagName="EditRuleData" TagPrefix="uc6" %>
<%@ Register Src="EditRuleAlert.ascx" TagName="EditRuleAlert" TagPrefix="uc5" %>
<%@ Register Src="EditRuleCondition.ascx" TagName="EditRuleCondition" TagPrefix="uc4" %>
<%@ Register Src="EditRuleCheckList.ascx" TagName="EditRuleCheckList" TagPrefix="uc3" %>
<%@ Register Src="EditRuleField.ascx" TagName="EditRuleField" TagPrefix="uc2" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptLocalization="True">
</asp:ScriptManager>
<script language="javascript" type="text/javascript">
function SetSelect(status,o1){
    SetDisabled(o1,status);
}
function ValidateAlert(o1,o1err,o2,o3,o3err){
    var res = true;
    if(o1.value==''){
        o1err.innerHTML='*';
        res = false;
    }else{
        o1err.innerHTML='';
    }
    if (o2.checked){
        if (o3.value==0){
            res=false;
            o3err.innerHTML='*';
        }else{
            o3err.innerHTML='';
        }
    }    
    return res;
}
function CheckItems(status,o1,o2,o3,o4,o5){
    o1.setAttribute('disabled',!status);
    SetDisabled(o2,status);
    SetDisabled(o3,status);
    SetDisabled(o4,status);
    SetDisabled(o5,status);    
    if (status){
        o1.focus();
    }
}
function SetDisabled(o,status){
    o.parentNode.setAttribute('disabled',!status);
    o.setAttribute('disabled',!status);   
}
function ClearFilter(){
    document.getElementById('<%= tbRuleName.ClientID %>').value = '';
    document.getElementById('<%= ddlProduct.ClientID %>').value = 0;        
    document.getElementById('<%= ddlCategory.ClientID %>').value = 0;    
}
function ValidateCondition(o1,o1err,o2,o2err,o3,o3err){
    var res = true;
    if(o1.value==''){
        o1err.innerHTML='*';
        res = false;
    }else{
        o1err.innerHTML='';
    }
    if(o2.value==''){
        o2err.innerHTML='*';
        res = false;
    }else{
        o2err.innerHTML='';
    }
    if (o3.value==0) {
        o3err.innerHTML='*';
        res=false;
    }else{
        o3err.innerHTML='';
    }
    return res;
}
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">                        	            
<ContentTemplate> 
<table width="100%" border="0" cellspacing="0" cellpadding="5">
    <tr runat="server" id="trview">
        <td>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
	            <tr>
	                <td align="left" style="height: 22px">
                        <asp:LinkButton ID="lbAdd" runat="server" OnClick="lbAdd_Click">Add rule</asp:LinkButton>
                    </td>
	                <td align="right" valign="middle" style="height: 22px" id="tdSelectCompany" runat="server">
                        Select originator:&nbsp;
	                    <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" Width="180px"></asp:DropDownList>
                    </td>
                    <td runat="server" id="tdNoSelect">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div style="border:solid 1px black; padding-left:3px; padding-top:3px; padding-bottom:3px">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label4" runat="server" Text="Search filter:" SkinID="AdminLabel"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="width:70px;padding-right:3px" align="right">
                                                <asp:Label ID="Label6" runat="server" Text="Rule name:" SkinID="AdminLabel"></asp:Label>
                                            </td>
                                            <td style="width:160px">
                                                <asp:TextBox ID="tbRuleName" runat="server" Width="160px"></asp:TextBox>
                                            </td>                                
                                            <td style="width:80px;padding-right:3px;" align="right">
                                                <asp:Label ID="Label1" runat="server"  SkinID="AdminLabel">Category:</asp:Label>
                                            </td>
                                            <td align="right" style="width:20px; padding-right:3px">
                                                <asp:DropDownList ID="ddlCategory" runat="server"></asp:DropDownList>
                                            </td>
                                            <td style="width:70px;padding-right:3px" align="right">
                                                <asp:Label ID="Label2" runat="server" Text="Product:" SkinID="AdminLabel"></asp:Label>
                                            </td>
                                            <td style="width:160px">
                                                <asp:DropDownList ID="ddlProduct" runat="server" Width="100%"></asp:DropDownList>
                                            </td>
                                            <td>&nbsp;</td>                                        
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="padding-top:5px;">
                                <td align="center">
                                    <table>
                                        <tr>
                                            <td><input id="Button1" type="button" value="Reset" onclick="ClearFilter();"/></td>
                                            <td style="width:100px;">&nbsp;</td>
                                            <td><asp:Button ID="btnSearch" runat="server" OnClick="Search_Click" Text="Search" /></td>
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
    <tr id="tredit" runat="server">
        <td>
            <div style="border:solid 1px black; padding-left:3px; padding-top:3px; padding-bottom:3px" runat="server" id="controlph">
                <div runat="server" id="diveditrulecode">
                    <uc9:EditRuleCode id="editrulecode" runat="server" />
                </div>
                <div runat="server" id="diveditrulefield">
                    <uc2:EditRuleField ID="editrulefield" runat="server" />
                </div>
                <div runat="server" id="diveditrulechecklist">
                    <uc3:EditRuleCheckList ID="editrulechecklist" runat="server" />
                </div>
                <div runat="server" id="diveditrulecondition">
                    <uc4:EditRuleCondition id="editrulecondition" runat="server" />
                </div>
                <div runat="server" id="diveditrulealert">
                    <uc5:EditRuleAlert id="editrulealert" runat="server" />
                </div>
                <div runat="server" id="diveditruledocument">
                    <uc8:EditRuleDocument id="editruledocument" runat="server" />
                </div>
                <div runat="server" id="diveditruledata">
                    <uc6:EditRuleData id="editruledata" runat="server" />
                </div>                
                <div runat="server" id="diveditruletask">                    
                    <uc7:EditRuleTask ID="editruletask" runat="server" />
                </div>                
            </div>
        </td>
    </tr>
    <tr>
        <td>      
            <radG:RadGrid ID="G" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" OnItemCommand="G_ItemCommand" OnItemDataBound="G_ItemDataBound" OnPageIndexChanged="G_PageIndexChanged" PageSize="20" >
            <ClientSettings>
            <Resizing AllowColumnResize="True" EnableRealTimeResize="True"/>
            </ClientSettings>
                <MasterTableView AllowNaturalSort="False">
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Rule name" SortExpression="Name" >
                            <ItemTemplate>
                                <asp:Label ID="lblRule" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="160px"/>
                        </radG:GridTemplateColumn>                    
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Category" SortExpression="Category" >
                            <ItemTemplate>
                                <asp:Label ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Category") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="120px"/>
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Originator Specific" SortExpression="LenderSpecific" >
                            <ItemTemplate>
                                <asp:Label ID="lblLenderSpecific" runat="server" Text='<%# GetLenderSpecific(Container.DataItem,"LenderSpecific") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="60px"/>
                        </radG:GridTemplateColumn> 
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Parent" SortExpression="ParentRuleName" >
                            <ItemTemplate>
                                <asp:Label ID="lblParentRuleName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ParentRuleName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="60px"/>
                        </radG:GridTemplateColumn>                                               
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Rule Code">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkCode" runat="server" CommandName="editrulecode" Text="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="30px"/>
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Show Field">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkField" runat="server" CommandName="editrulefield" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'>LinkButton</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="40px"/>
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Condition">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkCondition" runat="server" CommandName="editrulecondition" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'>LinkButton</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="55px"/>
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Task">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkTask" runat="server" CommandName="editruletask" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'>LinkButton</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="30px"/>
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Document">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkDocument" runat="server" CommandName="editruledocument" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'>LinkButton</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="60px"/>
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="CheckList">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkCheckList" runat="server" CommandName="editrulechecklist" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'>LinkButton</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="55px"/>
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Alert">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkAlert" runat="server" CommandName="editrulealert" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'>LinkButton</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="30px"/>
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Data">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkData" runat="server" CommandName="editruledata" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'>LinkButton</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="30px"/>
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Status" Resizable="False"  SortExpression="Status" >
                            <ItemTemplate>
                                <asp:LinkButton ID="ChangeStatus" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'>LinkButton</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />                            
                            <HeaderStyle HorizontalAlign="Center" Width="60px" />
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="" Resizable="False"  SortExpression="" >
                            <ItemTemplate>
                                <asp:ImageButton  id="btnAddChild" ImageUrl="~/images/enable.gif" CommandName="addchild" CommandArgument='<%# Eval("Id") %>' AlternateText="Add child rule" Runat="server"/>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />                            
                            <HeaderStyle HorizontalAlign="Center" Width="60px" />
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
</table>
</ContentTemplate>
</asp:UpdatePanel>

