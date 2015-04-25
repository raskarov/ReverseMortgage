<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewRequiredFields.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewRequiredFields" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>
<script language="javascript" type="text/javascript">
<!--
function CheckAll(o,d1){
    var d = document.getElementById(d1);
    var e = d.getElementsByTagName('input');
    for (var i=0; i<e.length; i++){
        if (e[i].type=='checkbox'){
            e[i].checked=o.checked;
        }
    }
}
function CheckField(o,o11,d1){
    var o1=document.getElementById(o11);
    var d=document.getElementById(d1);
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
<asp:Panel runat="server" ID="pnlRf">
<div style="padding-left:5px">
<table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr style="height:30px;">
        <td style="width:200px">&nbsp;</td>
        <td align="left"><asp:Label ID="lblHeader" runat="server" Text="Required fields settings." SkinID="AdminHeader"></asp:Label></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td style="width:200px">&nbsp;</td>
        <td align="left" ><asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label></td>
        <td>&nbsp;</td>
    </tr>
    <tr style="height:20px;padding-top:5px;padding-bottom:5px">
        <td style="width:200px;padding-right:5px" align="right"><asp:Label ID="Label1" runat="server" Text="Select status:"></asp:Label></td>
        <td align="left">
            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" Width="200px"></asp:DropDownList>
        </td>
        <td>&nbsp;</td>
    </tr>
</table>
<br />
<radTS:RadTabStrip ID="RadTabStrip1" runat="server" Skin="WebBlue" OnTabDataBound="RadTabStrip1_TabDataBound" MultiPageID="RadMultiPage1" EnableViewState="False">
</radTS:RadTabStrip>
<radTS:RadMultiPage ID="RadMultiPage1" runat="server" Height="440px" Width="100%" EnableViewState="False" AutoScrollBars="True">        
</radTS:RadMultiPage>
<br />
<br />
<asp:Button ID="btnSave" runat="server" Text="Save"  SkinID="AdminButton" OnClick="btnSave_Click"/>
</div>
</asp:Panel>
<rada:RadAjaxManager id="RadAjaxManager1" runat="server" EnableOutsideScripts="True" >
    <AjaxSettings>
        <radA:AjaxSetting AjaxControlID="pnlRf">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlRf" />
            </UpdatedControls>
        </radA:AjaxSetting>
    </AjaxSettings>
</rada:RadAjaxManager>