<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MpField.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.MpField" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
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
<div>
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td style="width:155px;text-align:right;padding-right:3px">Select status</td>
                <td><asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"></asp:DropDownList></td>
            </tr>
        </table>        
    <br />
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Skin="WebBlue" OnTabDataBound="RadTabStrip1_TabDataBound" MultiPageID="RadMultiPage1" EnableViewState="False">
    </radTS:RadTabStrip>
    <radTS:RadMultiPage ID="RadMultiPage1" runat="server" Height="440px" Width="100%" EnableViewState="False" AutoScrollBars="True">        
    </radTS:RadMultiPage>
</div>


