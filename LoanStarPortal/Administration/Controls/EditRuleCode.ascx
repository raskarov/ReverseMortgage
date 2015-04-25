<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditRuleCode.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditRuleCode" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<script language="javascript" type="text/javascript" defer="defer">
<!--
function SetDates(){
    var o=document.getElementById('<%= cbEnableDaterange.ClientID %>');
    if (o.checked){ 
        EnableDates();
    } else {
        DisableDates();
    }
}
function EnableDates(){
    <%= raddpFrom.ClientID %>.DateInput.Enable();
    <%= raddpTo.ClientID %>.DateInput.Enable(); 
}
function DisableDates(){
    <%= raddpFrom.ClientID %>.DateInput.Disable();
    <%= raddpTo.ClientID %>.DateInput.Disable();
    <%= raddpFrom.ClientID %>.DateInput.Clear();
    <%= raddpTo.ClientID %>.DateInput.Clear();    
}
function RaddpFromPopup(){
    <%= raddpFrom.ClientID %>.ShowPopup();
}
function RaddpToPopup(){
    <%= raddpTo.ClientID %>.ShowPopup();
}
function RaddpValuePopup(){
    <%= dpValue.ClientID %>.ShowPopup();
}
function CheckAll(o,d){
    var e = d.getElementsByTagName('input');
    for (var i=0; i<e.length; i++){
        if (e[i].type=='checkbox'){
            e[i].checked=o.checked;
        }
    }
}
function CheckField(o,o1,d){
    var e=d.getElementsByTagName('input');
    var cnt1=0;
    var cnt2=0;
    for (var i=0; i<e.length; i++){
        if ((e[i].type=='checkbox')&&(e[i].id!=o1.id)){
            cnt1 += e[i].checked?1:0;
            cnt2++;
        }
    } 
    if (o.checked){
        o1.checked = cnt1==cnt2;
    }else{
        o1.checked = false;
    }
}
-->
</script>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td align="center">
            <asp:Label ID="lblHeader" runat="server" Text="" SkinID="AdminHeader"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="right"  style="padding-right:3px;width:200px;">
                        <asp:Label ID="Label1" runat="server" Text="Rule name:" Font-Bold="true" Width="100%" ></asp:Label>
                    </td>
                    <td style="width:270px">
                        <asp:TextBox ID="tbName" runat="server" MaxLength="50" Width="270px"></asp:TextBox>
                    </td>
                    <td align="left" style="padding-left:3px">
                        <asp:Label ID="lbltberr" runat="server" Text="" ForeColor="red"></asp:Label>
                    </td>                
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="right" valign="middle" style="padding-right:3px;width:200px">
                        <asp:CheckBox ID="cbEnableDaterange" runat="server" Text="Date range" Font-Bold="true" />
                    </td>
                    <td align="right" style="padding-right:3px;width:40px">
                        <asp:Label ID="Label2" runat="server" Text="From:" SkinID="AdminLabel"></asp:Label>
                    </td>                    
                    <td style="padding-left:3px;width:95px;height:24px;padding-bottom:4px;padding-top:4px;">            
                        <radCln:RadDatePicker ID="raddpFrom" runat="server" Width="98%" FocusedDate="2099-12-31" SkinID="WebBlue">
                            <DatePopupButton Visible="False" />
                            <DateInput onclick="RaddpFromPopup()" Skin="" SkinID="WebBlue" />
                            <PopupButton Visible="False" />
                        </radCln:RadDatePicker>
                    </td>
                    <td style="width:10px;">
                        <asp:Label ID="lblerrfrom" runat="server" Text="" ForeColor="red"></asp:Label>
                    </td>
                    <td style="width:30px;padding-right:3px" align="right">            
                        <asp:Label ID="Label4" runat="server" Text="To:" SkinID="AdminLabel"></asp:Label>      
                    </td>
                    <td style="padding-left:3px;width:95px;height:24px;padding-bottom:4px;padding-top:4px;">
                        <radCln:RadDatePicker ID="raddpTo" runat="server" Width="98%" FocusedDate="2099-12-31" SkinID="WebBlue">
                            <DatePopupButton Visible="False" />
                            <DateInput onclick="RaddpToPopup()" Skin="" />
                            <PopupButton Visible="False" />
                        </radCln:RadDatePicker>            
                    </td>          
                    <td style="width:10px;">
                        <asp:Label ID="lblerrto" runat="server" Text="" ForeColor="red"></asp:Label>
                    </td>
                    <td>&nbsp;</td> 
                </tr>
            </table>
        </td>
    </tr>  
    <tr>
        <td>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="right"  style="padding-right:3px;width:200px;">
                        <asp:Label ID="Label12" runat="server" Text="Rule category:" SkinID="AdminLabel"></asp:Label>
                    </td>
                    <td style="width:270px;height:24px;padding-top:4px;padding-bottom:4px" align="left">
                        <radC:RadComboBox ID="ddlCategory" runat="server" AllowCustomText="True" Height="100px" Width="200px" MarkFirstMatch="True" ShowDropDownOnTextboxClick="False" ZIndex="10000" Skin="WebBlue" UseEmbeddedScripts="False"></radC:RadComboBox>            
                    </td>
                    <td>
                        <asp:Label ID="lblErrCategory" runat="server" Text="" ForeColor="red"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>        
        </td>
    </tr>  
    <tr>        
        <td align="left" style="padding-left:200px">
            <asp:Label ID="Label3" runat="server" Text="Select product" SkinID="AdminLabel"></asp:Label>
        </td>        
    </tr>
    <tr>
        <td style="padding-left:5px"><asp:CheckBox ID="cbAll" runat="server"  Text="Check/uncheck all" /></td>
    </tr>
    <tr>
        <td>
            <div id="divproduct" style="padding-left:10px">
            <asp:DataList ID="dlProducts" runat="server" OnItemDataBound="dlProducts_ItemDataBound" RepeatColumns="4" RepeatDirection="Horizontal">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'  />
                    </ItemTemplate>
            </asp:DataList>
            </div>
        </td>
    </tr>
    <tr>
        <td>
        <div runat="server" id="divRuleInfo" style="padding-left:10px">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="width:25px">
                    <asp:Label ID="Label5" runat="server" Text="NOT" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:80px">
                    <asp:Label ID="Label6" runat="server" Text="Logical Op" SkinID="AdminLabel"></asp:Label>
                </td>        
                <td style="width:90px">
                    <asp:Label ID="Label7" runat="server" Text="Select group" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:140px">
                    <asp:Label ID="Label8" runat="server" Text="Select field" SkinID="AdminLabel"></asp:Label>
                </td>        
                <td style="width:80px">
                    <asp:Label ID="Label9" runat="server" Text="Compare Op" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:80px">
                    <asp:Label ID="Label11" runat="server" Text="Field type" SkinID="AdminLabel"></asp:Label>
                </td>                
                <td style="width:130px">
                    <asp:Label ID="lblValue" runat="server" Text="Enter value" SkinID="AdminLabel" Width="130px" ></asp:Label>
                </td>
                <td>&nbsp;</td>        
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="cbNot" runat="server" />
                </td>    
                <td>
                    <asp:DropDownList ID="ddlLogicalOp" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlLogicalOp_SelectedIndexChanged" CssClass="selectcenter" SkinID="SelectCenter"></asp:DropDownList>
                </td>        
                <td>
                    <asp:DropDownList ID="ddlGroup" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged"></asp:DropDownList>
                </td>        
                <td>
                    <asp:DropDownList ID="ddlField" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlField_SelectedIndexChanged"></asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCompareOp" Width="100%" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompareOp_SelectedIndexChanged" SkinID="SelectCenter"></asp:DropDownList>
                </td>        
                <td>
                    <asp:DropDownList ID="ddlFieldType" Width="100%" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFieldType_SelectedIndexChanged" SkinID="SelectCenter"></asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="tbValue" runat="server" Width="98%" MaxLength="256"></asp:TextBox>
                    <asp:DropDownList ID="ddlDictionary"  Width="98%" runat="server"></asp:DropDownList>
                    <radCln:RadDatePicker ID="dpValue" runat="server" Width="98%" FocusedDate="2099-12-31" MinDate="1800-01-01">
                        <DatePopupButton Visible="False" />
                        <DateInput onclick="RaddpValuePopup()" />
                    </radCln:RadDatePicker>
                    <radi:radmaskedtextbox id="mtb" runat="server" displaymask="#########.##" displaypromptchar=" " Width="98%" DisplayFormatPosition="Right" Mask="#########.##"></radi:radmaskedtextbox>
                </td>
                <td>&nbsp;</td>        
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>
                    <asp:Label ID="validatormsg" runat="server" Text="" ForeColor="red">&nbsp;</asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>    
            <tr style="padding-bottom:5px">
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnCancel" runat="server" Text="Back" SkinID="AdminButton" CausesValidation="False" OnClick="btnCancel_Click" /></td>
                <td><asp:Button ID="btnAdd" runat="server" Text="Add" SkinID="AdminButton" OnClick="btnAdd_Click"/></td>                
            </tr>
            <tr>
                <td colspan="7">
                    <radG:RadGrid id="G" runat="server" Width="99%" Height="99%" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" AutoGenerateColumns="false"  AllowPaging="True" PageSize="3" OnPageIndexChanged="G_PageIndexChanged" OnItemDataBound="G_ItemDataBound" OnItemCommand="G_ItemCommand">
                    <MasterTableView AllowNaturalSort="False">
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Logical Op">
                            <ItemTemplate>
                                <asp:Label ID="lbllogop" runat="server" Text='<%# GetLogicalOp(Container.DataItem, "logicalop") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="60px"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn">
                            <ItemTemplate>
                                <asp:Label ID="lbllognot" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "logicalnot") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="20px"/>
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Field">
                            <ItemTemplate>
                                <asp:Label ID="lblfield" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px"/>
                        </radG:GridTemplateColumn>     
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Compare Op">
                            <ItemTemplate>
                                <asp:Label ID="lblcompareop" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "compareop") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="50px"/>
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>                                           
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Value">
                            <ItemTemplate>
                                <asp:Label ID="lblvaleu" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "datavalue") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="100px"/>
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Field">
                            <ItemTemplate>
                                <asp:Label ID="lblfieldname" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FieldName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="100px"/>
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Action" Resizable="False">
                            <ItemTemplate>
                                <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="deleteruleunit" AlternateText="Delete item" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>                           
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>                        
                    </Columns>
                    </MasterTableView>        
                    <PagerStyle HorizontalAlign="Left" Mode="NumericPages" PagerTextFormat="" />
                    </radG:RadGrid>        
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <asp:Label ID="Label10" runat="server" Text="Total expression:"  SkinID="AdminLabel"></asp:Label>
                </td>
            </tr>
            <tr><td colspan="7" runat="server" id="tdexpr">&nbsp;</td></tr>
        </table>
        </div>        
        </td>
    </tr>
    <tr>
        <td align="center" style="padding-top:5px">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="width:200px">&nbsp;</td>
                    <td align="left" style="width:215px"><asp:Button ID="btnBack" runat="server" Text="Close" SkinID="AdminButton" OnClick="btnBack_Click" CausesValidation="False"/></td>
                    <td align="left" ><asp:Button ID="btnSave" runat="server" Text="Save"  SkinID="AdminButton" OnClick="btnSave_Click"/></td>
                    <td>&nbsp;</td>
                </tr>
            </table>        
        </td>
    </tr>
</table>

