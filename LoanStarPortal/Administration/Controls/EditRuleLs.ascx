<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditRuleLs.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditRuleLs" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<%@ Register Src="EditRule.ascx" TagName="EditRule" TagPrefix="uc1" %>
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
<table width="100%" border="0" cellpadding="0" cellspacing="0"">
    <tr>
        <td>&nbsp;</td>
        <td align="left" colspan="2">
            <asp:Label ID="lblHeader" runat="server" Text="" SkinID="AdminHeader"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td align="left" colspan="2">
            <asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right" style="padding-right:3px;width:120px">
            <asp:Label ID="Label1" runat="server" Text="Rule name:" SkinID="AdminLabel"></asp:Label>
        </td>
        <td style="width:250px">
            <asp:TextBox ID="tbName" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
        </td>
        <td align="left"><asp:Label ID="lbltberr" runat="server" Text="" ForeColor="red"></asp:Label></td>
    </tr>
    <tr>
        <td align="right" valign="middle" style="padding-right:3px">
            <asp:CheckBox ID="cbEnableDaterange" runat="server" Text="Date range" Font-Bold="true" />
        </td>
        <td style="width:250px">
            <table width="100%" border="0" cellpadding="0" cellspacing="0"">
                <tr>
                    <td align="right" style="padding-right:3px;width:40px">
                        <asp:Label ID="Label2" runat="server" Text="From:" SkinID="AdminLabel"></asp:Label>
                    </td>                    
                    <td>            
                        <radCln:RadDatePicker ID="raddpFrom" runat="server" Width="100px" FocusedDate="2099-12-31">
                            <DatePopupButton Visible="False" />
                            <DateInput onclick="RaddpFromPopup()" />
                        </radCln:RadDatePicker>
                    </td>
                    <td style="width:10px;"><asp:Label ID="lblerrfrom" runat="server" Text="" ForeColor="red"></asp:Label></td>
                    <td style="width:30px;padding-right:3px" align="right">            
                        <asp:Label ID="Label4" runat="server" Text="To:" SkinID="AdminLabel"></asp:Label>      
                    </td>
                    <td>            
                        <radCln:RadDatePicker ID="raddpTo" runat="server" Width="100px" FocusedDate="2099-12-31">
                            <DatePopupButton Visible="False" />
                            <DateInput onclick="RaddpToPopup()" />
                        </radCln:RadDatePicker>            
                    </td>          
                    <td style="width:10px;"><asp:Label ID="lblerrto" runat="server" Text="" ForeColor="red"></asp:Label></td>
                </tr>
            </table>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td colspan="2" align="left">
            <asp:Label ID="Label3" runat="server" Text="Select product" SkinID="AdminLabel"></asp:Label>
        </td>        
    </tr>
    <tr>
        <td colspan="3"><asp:CheckBox ID="cbAll" runat="server"  Text="Check/uncheck all" /></td>
    </tr>
    <tr>
        <td colspan="3">
            <div id="divproduct">
            <asp:DataList ID="dlProducts" runat="server" OnItemDataBound="dlProducts_ItemDataBound" RepeatColumns="4" RepeatDirection="Horizontal">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'  />
                    </ItemTemplate>
            </asp:DataList>
            </div>
        </td>
    </tr>
</table>
<div runat="server" id="divRuleInfo"><uc1:EditRule ID="EditRule1" runat="server"/></div>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
<tr>
    <td class="editrolelabeltd">&nbsp;</td>
    <td class="edituserinputtd">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="left"><asp:Button ID="btnBack" runat="server" Text="Close" SkinID="AdminButton" OnClick="btnBack_Click" CausesValidation="False"/></td>
                <td align="right" style="padding-right:3px"><asp:Button ID="btnSave" runat="server" Text="Save"  SkinID="AdminButton" OnClick="btnSave_Click"/></td>
            </tr>
        </table>        
    </td>
    <td>&nbsp;</td>
</tr>
</table>

