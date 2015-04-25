<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PayoffEdit.ascx.cs" Inherits="LoanStarPortal.Controls.PayoffEdit" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<script language="javascript" type="text/javascript">
<!--
function AmountFocusLost(o,id1,id2,v){
    var id =GetRMTBId(o.id)
    var a1 = GetAmountValue(id);
    var a2 = v;
    var n='1';
    if(id2!=''){
        id=id.replace(id1,id2);
        a2 = GetAmountValue(id);    
        n='2';
    }
    var vv=a1-a2;
    vv=Math.round(vv*100)/100;   
    var i=id.lastIndexOf('_');
    if(i>0){
        var d=document.getElementById(id.substring(o,i)+'_lblFinancedAmountValue'+n);
        d.innerText='$'+vv;
    }
}
function PocAmountFocusLost(o,id1,id2){
    var id =GetRMTBId(o.id)
    var a2 = GetAmountValue(id);
    id=id.replace(id1,id2);
    var a1 = GetAmountValue(id);    
    var vv=a1-a2;
    vv=Math.round(vv*100)/100;
    var i=id.lastIndexOf('_');
    if(i>0){
        var d=document.getElementById(id.substring(o,i)+'_lblFinancedAmountValue2');
        d.innerText='$'+vv;
    }
}
function GetAmountValue(id){
    var r = window[id];
    if(r==null) return 0;
    return r.GetValue();
}
-->
</script>
<table border="0" cellpadding="0" cellspacing="0" style="width:97%">
    <tr>
        <td class="editFormLabel">Creditor:</td>
        <td align="left" style="padding-left:3px">
            <asp:TextBox ID="tbCreditor" runat="server" MaxLength="50" Width="200px" TabIndex="1"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="rfvCreditor" runat="server" ErrorMessage="*" ControlToValidate="tbCreditor" Width="10px"></asp:RequiredFieldValidator>
        </td>
        <td class="editFormLabel">Status:</td>
        <td style="padding-left:6px">
            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" Width="180px" TabIndex="2"></asp:DropDownList>
        </td>        
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="editFormLabel" style="height: 24px">Account Number:</td>
        <td style="padding-left:3px; height: 24px;">
            <asp:TextBox ID="tbAccountNumber" runat="server" MaxLength="20" Width="200px" TabIndex="7"></asp:TextBox>            
            <asp:RequiredFieldValidator ID="rfAccountNumber" runat="server" ErrorMessage="*" ControlToValidate="tbAccountNumber"></asp:RequiredFieldValidator></td>
        <td class="editFormLabel">Amount:</td>
        <td align="left" style="padding-bottom:4px;padding-left:3px">
            <radI:RadNumericTextBox ShowSpinButtons="false" Type="Currency" ID="tbAmount" runat="server" Skin="WebBlue" Width="100px"  MinValue="0" TabIndex="3"></radI:RadNumericTextBox>
        </td>        
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="editFormLabel">Address1:</td>
        <td style="padding-left:3px">
            <asp:TextBox ID="tbAddress1" runat="server" MaxLength="256" Width="200px" TabIndex="8"></asp:TextBox>
        </td>
        <td class="editFormLabel"><asp:Label ID="lblExpDate" runat="server" Text="Exp Date:"></asp:Label><asp:Label ID="lblFinancedAmount1" runat="server" Text="Financed:"></asp:Label></td>
        <td style="padding-bottom:4px;padding-left:0px">
            <radCln:RadDatePicker ID="diExpDate" runat="server" TabIndex="4" Width="130px" >
                <DateInput  Height="18px" Width="130px" style="color:#000000;font-size:12px;font-family:Arial, Helvetica, sans-serif;border:1px solid #7F9DB9;background:#FFFFFF;" Skin="WebBlue" TabIndex="4"></DateInput>
                <Calendar Skin="WebBlue"></Calendar>
            </radCln:RadDatePicker>
            <asp:RequiredFieldValidator ID="rfExpDate" runat="server" ErrorMessage="*" ControlToValidate="diExpDate"></asp:RequiredFieldValidator>
            &nbsp;&nbsp;<asp:Label ID="lblFinancedAmountValue1" runat="server" Text=""></asp:Label>
        </td>        
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="editFormLabel">Address2:</td>
        <td style="padding-left:3px">
            <asp:TextBox ID="tbAddress2" runat="server" MaxLength="256" Width="200px" TabIndex="9"></asp:TextBox>
        </td>            
        <td class="editFormLabel"><asp:Label ID="lblPerdiem" runat="server" Text="Perdiem:"></asp:Label></td>
        <td style="padding-bottom:4px;padding-left:3px">
            <radI:RadNumericTextBox Type="Currency" ID="tbPerdiem" runat="server" Height="18px" Culture="English (United States)" LabelCssClass="radLabelCss_Default" MaxValue="70368744177664" MinValue="0" Skin="WebBlue" Width="100px" TabIndex="5"></radI:RadNumericTextBox>&nbsp;
            <asp:RequiredFieldValidator ID="rfPerdiem" runat="server" ErrorMessage="*" ControlToValidate="tbPerdiem"></asp:RequiredFieldValidator></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="editFormLabel" style="height: 24px">City:</td>
        <td style="padding-left:3px; height: 24px;">
            <asp:TextBox ID="tbCity" runat="server" MaxLength="50" Width="200px" TabIndex="10"></asp:TextBox>            
        </td>    
        <td class="editFormLabel" style="height: 24px"><asp:Label ID="lblAmountPoc" runat="server" Text="Amount of payoff paid outside of closing?"></asp:Label></td>
        <td style="height: 24px"><radI:RadNumericTextBox ShowSpinButtons="false" Type="Currency" ID="tbPoc" runat="server" Skin="WebBlue" Width="100px"  MinValue="0" TabIndex="6">
        </radI:RadNumericTextBox></td>        
        <td style="height: 24px">&nbsp;</td>
    </tr>
    <tr>
        <td class="editFormLabel" style="height: 24px">State:</td>
        <td style="padding-left:3px; height: 24px;"><asp:DropDownList ID="ddlState" runat="server" AutoPostBack="false" Width="206px" TabIndex="11"></asp:DropDownList></td>
        <td class="editFormLabel"><asp:Label ID="lblFinancedAmount2" runat="server" Text="Financed:"></asp:Label></td>
        <td>&nbsp;<asp:Label ID="lblFinancedAmountValue2" runat="server" Text=""></asp:Label></td>
        <td>&nbsp;</td>
    </tr>
    <tr >    
        <td class="editFormLabel">Zip:</td>
        <td style="padding-bottom:4px;padding-left:0px" >
            <radI:RadMaskedTextBox ID="tbZip" runat="server" DisplayMask="#####" Mask="#####" PromptChar=" " LabelCssClass="radLabelCss_WebBlue" Skin="WebBlue" Width="80px" TabIndex="12"></radI:RadMaskedTextBox>
        </td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
    </tr>
    <tr style="padding-top:5px;padding-bottom:3px">
        <td align="right" colspan="2" style="padding-right:10px">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="publicEditFormButton" CommandName="Update" OnClick="btnSave_Click" TabIndex="13"/>
        </td>
        <td colspan="2" style="padding-left:10px">
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="publicEditFormButton" CommandName="Cancel" OnClick="btnCancel_Click" TabIndex="14"/>
        </td>
        <td>&nbsp;</td>
    </tr>
</table>
