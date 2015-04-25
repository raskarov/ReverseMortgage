<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditLeadCampaign.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditLeadCampaign" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<script language="javascript" type="text/javascript">
<!--
function SetScheduleRowVisibility(o,i1,i2){
    var s1;
    var s2;
    if(o.value==1){
        s1='inline';
        s2='none';
    }else{
        s2='inline';
        s1='none';
    }
    var t1=document.getElementById(i1);
    if(t1!=null) t1.style.display=s1;
    var t2=document.getElementById(i2);
    if(t2!=null) t2.style.display=s2;
}
-->
</script>
<table border="0" cellpadding="0" cellspacing="0" style="width:97%">
    <tr>
        <td class="lclbl">&nbsp;</td>
        <td class="lcctl"><asp:CheckBox ID="cbIsOn" runat="server" Text="Turn on campaign"/></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="lclbl"><asp:Label ID="Label1" runat="server" Text="Title:"></asp:Label></td>
        <td class="lcctl">
            <asp:TextBox ID="tbTitle" runat="server" MaxLength="256" Width="400px"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="rfTitle" runat="server" ErrorMessage="*" ControlToValidate="tbTitle"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr valign="top">
        <td class="lclbl"><asp:Label ID="Label2" runat="server" Text="Detail:"></asp:Label></td>
        <td class="lcctl">
            <asp:TextBox ID="tbDetail" runat="server" TextMode="MultiLine" Rows="4" Width="400px"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="rfDetail" runat="server" ErrorMessage="*" ControlToValidate="tbDetail"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="lclbl"><asp:Label ID="Label5" runat="server" Text="Frequency:"></asp:Label></td>
        <td class="lcctl"><asp:DropDownList ID="ddlFrequency" runat="server" Width="220px"></asp:DropDownList></td>
        <td>&nbsp;</td>
    </tr>    
    <tr>
        <td class="lclbl"><asp:Label ID="Label4" runat="server" Text="Start date:"></asp:Label></td>
        <td class="lcctl">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width:225px;"><asp:DropDownList ID="ddlStartDate" runat="server"  Width="220px"></asp:DropDownList></td>
                    <td style="width:50px;"><asp:DropDownList ID="ddlSign" runat="server"  Width="45px" ></asp:DropDownList></td>
                    <td><radI:RadNumericTextBox Type="Number" ID="tbDayOffset" runat="server" MinValue="0" MaxValue="100" Width="60px" NumberFormat-DecimalDigits="0" ></radI:RadNumericTextBox></td>                    
                </tr>
            </table>        
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="lclbl">&nbsp;</td>
        <td class="lcctl"><asp:CheckBox ID="cbIsOnlyWorkingDayAllowed" runat="server" Text="Allow follow up date to fall only on working days"/></td>
        <td>&nbsp;</td>
    </tr>
    <tr valign="top">
        <td class="lclbl"><asp:Label ID="Label3" runat="server" Text="Apply to leads:"></asp:Label></td>
        <td class="lcctl"><asp:DropDownList ID="ddlLeadsType" runat="server" Width="220px"></asp:DropDownList></td>
        <td>&nbsp;</td>
    </tr>
    <tr style="padding-top:5px;padding-bottom:5px;">
        <td class="lclbl">&nbsp;</td>        
        <td class="lcctl">        
            <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                <tr>
                    <td align="left" style="width:100px;"><asp:Button ID="btnSave" runat="server" Text="Save" Width="60px" OnClick="btnSave_Click"/></td>
                    <td style="width:50px;">&nbsp;</td>
                    <td><asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"  Width="60px" OnClick="btnCancel_Click"/></td>
                </tr>
            </table>
        
        </td>
        <td>&nbsp;</td>
    </tr>
</table>
