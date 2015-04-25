<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendorFeeCategory.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.VendorFeeCategory" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<script language="javascript" type="text/javascript">
<!--
function CheckItem(status,o1){
var o=window[o1.id];
if(status){ 
    o.Enable();
    o.Focus();
}else{
    o.Disable();
}    
}
function CheckItem1(status,o1){
    var cn='class';
    if(document.all){
        o1.setAttribute('disabled',!status);
        var cn='className';
    }else{
        if(status)
            o1.removeAttribute('disabled');
        else
            o1.setAttribute('disabled','true');
    }
    if (status){
        o1.focus();
    }
    var c=o1.getAttribute(cn);
    if(status)
        c = c.replace(/Disabled/,"Enabled")        
    else        
        c = c.replace(/Enabled/,"Disabled")        
    o1.setAttribute(cn,c);    
}

-->
</script>
<table border="0" cellpadding="0" cellspacing="0" style="width:100%" >
    <tr style="padding-top:3px;">
        <td style="padding-left:10px;font-weight:bold;width:120px;" >Fee type</td>
        <td style=";font-weight:bold">Contract Amount</td>
        <td>&nbsp;</td>
    </tr>
    <asp:Repeater ID="rpVendorFeeType" runat="server" OnItemDataBound="rpVendorFeeType_ItemDataBound" EnableViewState="false">
        <ItemTemplate>        
            <tr>
                <td align="left" style="width:300px;" >
                    <asp:CheckBox runat="server" ID="cbFeeType" EnableViewState="true"></asp:CheckBox>
                </td>
                <td>
                    <radI:RadNumericTextBox ID="tbAmount" runat="server" Type="Currency" MinValue="0" Width="70px" EnableViewState="true"/>
                </td>
                <td>&nbsp;</td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>                
