<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditProduct.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditProduct" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<script language="javascript" type="text/javascript">
<!--
function SetRowVisibility(o,i,r){
    var a = r.split(',');
    var s;
    if(o.value==i){
        s='inline';
    }else{
        s='none';
    }
    for(var i=0;i<a.length;i++){
        var rr = document.getElementById(a[i]); 
        if(rr!=null) rr.style.display=s;
    }
}
function SetRowVisibility1(o,i,r,i1,r1){
    var a = r.split(',');
    var s;
    if(o.value==i){
        s='inline';
    }else{
        s='none';
    }
    for(var i=0;i<a.length;i++){
        var rr = document.getElementById(a[i]); 
        if(rr!=null) rr.style.display=s;
    }
    if(o.value==i1){
        s='inline';
    }else{
        s='none';
    }
    var rr = document.getElementById(r1); 
    if(rr!=null) rr.style.display=s;
}
function SetRowVisibilityProtection(val,o,ov){
    var s;
    var en;
    if(val){
        s='inline';
        en=true;
    }else{
        s='none';
        en=false;
    }
    var rr = document.getElementById(o); 
    if(rr!=null) rr.style.display=s;
    var v = document.getElementById(ov);
    if(v!=null) v.enabled=en;
}
function SetRowVisibilityCb(val,o){
    var s;
    if(val){
        s='inline';
    }else{
        s='none';
    }
    var rr = document.getElementById(o); 
    if(rr!=null) rr.style.display=s;
}
function SetRowVisibilityBaydocCode(val,id1,id2){
    var s;
    if(val){
        s='inline';
    }else{
        s='none';
    }
    var rr = document.getElementById(id1); 
    if(rr!=null) rr.style.display=s;
    rr = document.getElementById(id2); 
    if(rr!=null) rr.style.display=s;
}
function ValidateBaydocCode(src,arg){
    var o = document.getElementById(src.id);
    o=document.getElementById(o.getAttribute('controltovalidate'));
    var p=GetParentTr(o);
    if(p){
        if(p.style.display=='inline'){
            arg.IsValid = o.value!='0';
        }else{
            arg.IsValid = true;
        }
    }
}
function ValidateSectionOfTheAct(src,arg){
    var o = document.getElementById(src.id);
    o=document.getElementById(o.getAttribute('controltovalidate'));
    var p=GetParentTr(o);
    if(p){
        if(p.style.display=='inline'){
            arg.IsValid = o.value!='';
        }else{
            arg.IsValid = true;
        }
    }
}
function GetParentTr(o){
    var p=o;
    while(true){
        if(!p.parentElement) return null;
        p=p.parentElement;
        if(p.tagName.toLowerCase()=='tr'){
            return p;
        }
    }
    return null;
}
function SetRowVisibilityProtection(o,i1,i2,rf1,rf2){
    var s1='none';
    var s2='none';
    var en1=false;
    var en2=false;
    if(o.value==1){
        s1='inline';
        en1=true;
    }else if(o.value==2){
        s2='inline';
        en2=true;
    }
    var t1=document.getElementById(i1); 
    if(t1!=null) t1.style.display=s1;

    var t2=document.getElementById(i2); 
    if(t2!=null) t2.style.display=s2;

    var v1 = document.getElementById(rf1);
    if(v1!=null) v1.enabled=en1;
    var v2 = document.getElementById(rf2);
    if(v2!=null) v2.enabled=en2;
}
function SetRowVisibilityInputMethod(o,i1){
    var s1='none';
    if(o.value==1){
        s1='inline';
    }
    var t1=document.getElementById(i1); 
    if(t1!=null) t1.style.display=s1;    
}
function SetButtonContinue(o,i1){
    var en=o.value!=0;
    var b=document.getElementById(i1);
    if(b!=null) 
    {
        if(en){
            b.removeAttribute('disabled');
        }else{
            b.setAttribute('disabled','true');
        }
    }
}
-->
</script>
<div id="divSelectTemplate" runat="server">
<table width="70%" border="0" cellpadding="0" cellspacing="0">
    <tr style="padding-top:30px;">
        <td>&nbsp;</td>
        <td align="left"><asp:Label ID="lblAddNew" runat="server" Text="New product" SkinID="AdminHeader"></asp:Label></td>
    </tr>
    <tr style="padding-top:20px;">
        <td style="width:250px;text-align:right;padding-right:5px;"><asp:Label ID="lblSelectTemplate" runat="server" EnableViewState="false" Text="Please select product template:"/></td>
        <td style="text-align:left"><asp:DropDownList ID="ddlProductTemplate" runat="server" AutoPostBack="false" DataTextField="Name" DataValueField="Id" Width="250px"/></td>
    </tr>
    <tr style="padding-top:20px;">
        <td>&nbsp;</td>
        <td><asp:Button ID="btnContinue" runat="server" Text="Continue" Width="70px" OnClick="btnContinue_Click" CausesValidation="False" Enabled="false"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnCancel" runat="server" Text="Cancel"  Width="70px" OnClick="btnCancel_Click" CausesValidation="False"/>
        </td>
    </tr>    
</table>
</div>
<div id="divMain" runat="server">
<asp:Panel DefaultButton="btnSave" ID="Panel1" runat="server">
<table width="800px" border="0" cellpadding="0" cellspacing="0" align="center">
<colgroup>
    <col width="350px" />
    <col width="170px" />
    <col width="7px" />
    <col width="250px" />
</colgroup>
<tr>
    <td colspan="4" align="center">
        <asp:Label ID="lblHeader" runat="server" Text="" SkinID="AdminHeader"></asp:Label>
    </td>
    <td></td>
</tr>
<tr style="height:20px">
    <td colspan="4" align="center">
        <asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
    </td>
</tr>
<tr>
    <td><asp:Label ID="label1" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Name:"/></td>
    <td><asp:TextBox ID="tbName" runat="server" MaxLength="100" Width="150"></asp:TextBox></td>
    <td><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbName" ErrorMessage="*"></asp:RequiredFieldValidator></td>
    <td>&nbsp;</td>
</tr>
<asp:Panel ID="PanelHECM" runat="server">
<tr style="padding-bottom:1px;">
    <td><asp:Label ID="label2" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Rate Rounding Precision:"/></td>
    <td><radI:RadNumericTextBox Type="Number" ID="tbRateRoundingPrecision" runat="server" MinValue="0.0001" MaxValue="1" Width="100px" NumberFormat-DecimalDigits="4"></radI:RadNumericTextBox></td>
    <td>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbRateRoundingPrecision" ErrorMessage="*"></asp:RequiredFieldValidator>        
    </td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label3" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Rate Rounding Method:"/></td>
    <td><asp:DropDownList ID="ddlRateRoundingMethod" runat="server" DataTextField="Name" DataValueField="ID"></asp:DropDownList></td>
    <td><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlRateRoundingMethod" ErrorMessage="*"></asp:RequiredFieldValidator></td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label4" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Payments Per Year:"/></td>
    <td><radI:RadNumericTextBox Type="Number" ID="tbPaymentsPerYear" runat="server" MinValue="0" MaxValue="365" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox></td>
    <td><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbPaymentsPerYear" ErrorMessage="*"></asp:RequiredFieldValidator></td>
    <td>&nbsp;<asp:Label ID="label45" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="whole integers from 0 to 365"/></td>
</tr>
<tr>    
    <td><asp:Label ID="label5" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Basis:"/></td>
    <td><asp:DropDownList ID="ddlBasis" runat="server" DataTextField="Name" DataValueField="ID"></asp:DropDownList></td>
    <td><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlBasis" ErrorMessage="*"></asp:RequiredFieldValidator></td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label6" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Property Appreciation:"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbPropertyAppreciation" runat="server" MinValue="0" MaxValue="20" Width="100px" NumberFormat-DecimalDigits="3"></radI:RadNumericTextBox>
    </td>
    <td>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbPropertyAppreciation" ErrorMessage="*"></asp:RequiredFieldValidator>
    </td>
    <td>&nbsp;<asp:Label ID="label46" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="cannot be less then 0 or be more then 20"/></td>
</tr>
<tr>    
    <td><asp:Label ID="label7" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Upfront Mortgage Insurance Rate:"/></td>
    <td>        
        <radI:RadNumericTextBox Type="Number" ID="tbUpfrontMortgageInsuranceRate" runat="server" MinValue="0" MaxValue="20" Width="100px" NumberFormat-DecimalDigits="3"></radI:RadNumericTextBox>
    </td>
    <td><asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbUpfrontMortgageInsuranceRate" ErrorMessage="*"></asp:RequiredFieldValidator></td>
    <td>&nbsp;<asp:Label ID="label47" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="cannot be less then 0 or be more then 20"/></td>
</tr>
<tr>
    <td><asp:Label ID="label8" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Renewal Mortgage Insurance Rate:"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbRenewalMortgageInsuranceRate" runat="server" MinValue="0" MaxValue="20" Width="100px" NumberFormat-DecimalDigits="3"></radI:RadNumericTextBox>
    </td>
    <td><asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="tbRenewalMortgageInsuranceRate" ErrorMessage="*"></asp:RequiredFieldValidator></td>
    <td>&nbsp;<asp:Label ID="label48" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="cannot be less then 0 or be more then 20"/></td>
</tr>
<tr>
    <td><asp:Label ID="label10" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for collecting HECM counseling certificates?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingCounsCert" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label52" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for reviewing HECM counseling certificates?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingCounsCert" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label53" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for collecting case number assignments from FHA Connection?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingFHACase" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label54" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for reviewing case number assignments from FHA Connection?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingFHACase" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label55" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for collecting condo approvals or spot condo affidavits?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingFHACondoApproval" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label56" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for reviewing FHA spot condo Affidavits?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingFHACondoApproval" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label57" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for collecting appraisals?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingAppraisal" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label58" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for collecting termite/pest inspections?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingTermiteInspection" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label59" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for reviewing appraisals?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingAppraisal" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label60" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for reviewing termite/pest inspections?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingTermiteInspections" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label61" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for collecting contractor bids?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingContractorBids" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label62" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for reviewing contractor bids?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingContractorBids" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label63" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for collecting structural inspections?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingStructuralInspections" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label64" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for collecting water tests?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingWaterTests" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label65" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for reviewing water tests?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingWaterTests" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label66" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for collecting septic inspections?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingSepticInspections" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label67" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for reviewing a septic inspections?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingSepticInspections" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label68" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for collecting oil tank inspections?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingOilTankInspections" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label69" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for reviewing an oil tank inspections?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingOilTankInspections" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label70" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for collecting roof inspections?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingRoofInspections" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label71" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for reviewing a roof inspections?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingRoofInspections" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label72" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for collecting power of attorney documentation"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingPOAandConservator" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label73" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for reviewing the power of attorney documentation?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingPOA" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label74" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for reviewing the conservator documentation?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingConservator" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label75" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for collecting credit reports?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingCreditReports" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label76" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for reviewing credit report?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingCreditReports" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label77" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for collecting LDP and GSA printouts?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingLDP_GSAPrintout" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label78" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for reviewing LDP and GSA printouts?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingLDP_GSAPrintout" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label79" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for collecting CAIVRS authorizations from FHA Connection?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingCAIVRSAuthPrintout" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label80" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for reviewing CAIVRS authorizations from FHA Connection?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingCAIVRSAuthPrintout" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label81" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for collecting trust agreements?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingTrusts" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label82" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for reviewing trust agreements?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingTrusts" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
</asp:Panel>
<asp:Panel ID="PanelAll" runat="server">
<tr>
    <td><asp:Label ID="label9" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Margin:"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbMargin" runat="server" MinValue="0" MaxValue="1000" Width="100px" NumberFormat-DecimalDigits="4"></radI:RadNumericTextBox>
    </td>
    <td><asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="tbMargin" ErrorMessage="*"></asp:RequiredFieldValidator></td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label11" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="End of Month Flag:"/></td>
    <td>
        <asp:RadioButtonList ID="rbEndOfMonthFlag" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
            <asp:ListItem Text="No" Value="N"></asp:ListItem>
        </asp:RadioButtonList></td>
    <td>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="rbEndOfMonthFlag" ErrorMessage="*"></asp:RequiredFieldValidator>
    </td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label14" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Relative Rate Cap:"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbRelativeRateCap" runat="server" MinValue="0" MaxValue="1000000" Width="100px" NumberFormat-DecimalDigits="4"></radI:RadNumericTextBox>
    </td>
    <td><asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="tbRelativeRateCap" ErrorMessage="*"></asp:RequiredFieldValidator></td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label16" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Shared Appreciation:"/></td>
    <td><asp:CheckBox ID="chkSharedAppreciation" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label17" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Amortization Type:"/></td>
    <td><asp:DropDownList ID="ddlAmortizationType" runat="server" DataTextField="Name" DataValueField="ID" AutoPostBack="false"></asp:DropDownList>&nbsp;<asp:RangeValidator ID="rvddlAmortType" runat="server" ErrorMessage="*" MinimumValue="1" MaximumValue="1000" ControlToValidate="ddlAmortizationType" Type="Integer"></asp:RangeValidator></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr id="trAmortizationOther" runat="server" style="display:none">
    <td><asp:Label ID="label18" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Amortization Other:"/></td>
    <td><asp:TextBox ID="tbAmortizationOther" runat="server" MaxLength="100" Width="150"></asp:TextBox></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label19" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Arm Type:"/></td>
    <td><asp:DropDownList ID="ddlArmType" runat="server" DataTextField="Name" DataValueField="ID" AutoPostBack="false"></asp:DropDownList>&nbsp;<asp:RangeValidator ID="rvddlArmType" runat="server" ErrorMessage="*" MinimumValue="1" MaximumValue="1000" ControlToValidate="ddlArmType" Type="Integer"></asp:RangeValidator></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr id="trArmType" runat="server" style="display:none">
    <td><asp:Label ID="label20" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Arm Other:"/></td>
    <td><asp:TextBox ID="tbArmTypeOther" runat="server" MaxLength="100" Width="150"></asp:TextBox></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label21" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Product Type:"/></td>
    <td><asp:DropDownList ID="ddlType" runat="server" DataTextField="Name" DataValueField="ID" AutoPostBack="false"></asp:DropDownList>&nbsp;<asp:RangeValidator ID="RangeValidator10" runat="server" ErrorMessage="*" MinimumValue="1" MaximumValue="1000" ControlToValidate="ddlType" Type="Integer"></asp:RangeValidator></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr id="trSectionOfTheAct" runat="server" style="display:none">
    <td><asp:Label ID="label12" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Section of the act:"/></td>
    <td><asp:TextBox ID="tbSectionOfTheAct" runat="server" MaxLength="250" Width="150"></asp:TextBox>&nbsp;
    &nbsp;<asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="*" ClientValidationFunction="ValidateSectionOfTheAct" ControlToValidate="tbSectionOfTheAct" ValidateEmptyText="true"></asp:CustomValidator></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr id="trProductType" runat="server" style="display:none">
    <td><asp:Label ID="label22" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Type Other:"/></td>
    <td><asp:TextBox ID="tbTypeOther" runat="server" MaxLength="100" Width="150"></asp:TextBox></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr id="trOtherDescription" runat="server" style="display:none">
    <td><asp:Label ID="label23" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Other description:"/></td>
    <td><asp:TextBox ID="tbOtherDescription" runat="server" MaxLength="256" Width="150"></asp:TextBox></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label25" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Special Feature"/></td>
    <td><asp:CheckBox ID="cbSpecialFeature" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr id="trSpecialFeature" runat="server" style="display:none">
    <td><asp:Label ID="label24" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Special feature description:"/></td>
    <td><asp:TextBox ID="tbSpecialFeature" runat="server" MaxLength="256" Width="150"></asp:TextBox></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label26" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Rate Change Day:"/></td>
    <td><asp:DropDownList ID="ddlRateChangeDay" runat="server" DataTextField="Name" DataValueField="ID" AutoPostBack="false"></asp:DropDownList>&nbsp;<asp:RangeValidator ID="RangeValidator11" runat="server" ErrorMessage="*" MinimumValue="1" MaximumValue="1000" ControlToValidate="ddlRateChangeDay" Type="Integer"></asp:RangeValidator></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label28" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Primary residence required?"/></td>
    <td><asp:CheckBox ID="cbPrimaryResReq" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label29" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product allow for primary residences to be the collateral for financing?"/></td>
    <td><asp:CheckBox ID="cballowPrimaryRes" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label30" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product allow for second or vacation homes to the collateral for financing?"/></td>
    <td><asp:CheckBox ID="cballowSecondHome" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label31" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product allow for investment properties to be the collateral for financing?"/></td>
    <td><asp:CheckBox ID="cballowInvestmentProp" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label32" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Is an appraisal required for this product?"/></td>
    <td><asp:CheckBox ID="cbappraisalRequired" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label33" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Is a property inspection required for this product?"/></td>
    <td><asp:CheckBox ID="cbpropInspRequired" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label34" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product use standard flood insurance guidelines?"/></td>
    <td><asp:CheckBox ID="cbuseStandardFloodGuides" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label35" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="What is the maximum deductible amount on a flood insurance policy for a single family residence for this product?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbmaxFloodDeductible" runat="server" MinValue="0" MaxValue="1000000" Width="100px" NumberFormat-DecimalDigits="4"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label36" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product use standard hazard insurance guidelines?"/></td>
    <td><asp:CheckBox ID="cbuseStandardHazardGuides" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label37" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="What is the maximum deductible amount on a hazard insurance policy for a single family residence for this product?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbmaxHazDeductPercent" runat="server" MinValue="0" MaxValue="1000000" Width="100px" NumberFormat-DecimalDigits="4"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label38" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product allow for a HECM Refi?"/></td>
    <td><asp:CheckBox ID="cballowHECMRefi" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label39" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product allow multifamily properties as the collateral of the financing?"/></td>
    <td><asp:CheckBox ID="cballowMultiFamilyProp" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label40" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product use standard guidelines regarding wells and septics on the same property?"/></td>
    <td><asp:CheckBox ID="cbuseStandardGuidesWellSeptic" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label41" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product use standard guidelines for situations where the borrowers have to bring money to the closing?"/></td>
    <td><asp:CheckBox ID="cbuseStndGuidesCashToClose" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label42" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How many days in advance of the property tax due date do you start requiring property taxes to be paid?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbdaysAdvancePayTax" runat="server" MinValue="0" MaxValue="1000" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label43" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow standard rules for trusts?"/></td>
    <td><asp:CheckBox ID="cbuseStandardTrustGuides" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label44" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product allow manufactured homes as the collateral for financing?"/></td>
    <td><asp:CheckBox ID="cballowManuHomes" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label13" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Is a credit report required for this product?"/></td>
    <td><asp:CheckBox ID="cbCreditReportRequired" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label50" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Product Index:"/></td>
    <td><asp:DropDownList ID="ddlProductIndex" runat="server" DataTextField="Name" DataValueField="ID" AutoPostBack="false"></asp:DropDownList>&nbsp;<asp:RangeValidator ID="RangeValidator17" runat="server" ErrorMessage="*" MinimumValue="1" MaximumValue="1000" ControlToValidate="ddlType" Type="Integer"></asp:RangeValidator></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label83" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for requiring and collecting death certificates?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesCollectingDeathCerts" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label84" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for reviewing death certificates?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesReviewingDeathCerts" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label85" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for collecting USPS verifications?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesCollectingUSPS" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label86" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for reviewing USPS verifications?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesReviewingUSPS" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label87" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for collecting title commitments?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesCollectingTitle" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label88" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for reviewing title commitments?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesReviewingTitle" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label89" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for collecting flood certificates?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesCollectingFloodCertificates" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label90" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for reviewing flood certifications?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesReviewingFloodCertificates" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label91" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for collecting hazard insurance declaration pages for SFRs?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesCollectingHazardDecPages" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label92" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for reviewing hazard insurance declaration pages for SFRs?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesReviewingHazardDecPages" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label93" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for collecting flood insurance declaration pages for SFRs?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesCollectingFloodDecPages" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label94" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for reviewing flood insurance declaration pages for SFRs?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesReviewingFloodDecPages" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label95" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for collecting proof of age?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesCollectingProofOfAge" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label96" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for reviewing proof of age?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesReviewingProofOfAge" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label97" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for collecting proof of social security number?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesCollectingSSN" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label98" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for reviewing proof of social security number?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesReviewingSSN" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label136" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does baydocs have application packages for this product?"/></td>
    <td><asp:CheckBox ID="cbUseBaydocsAppPackagesYN" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr runat="server" id="trBaydocAppPack">    
    <td><asp:Label ID="label99" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Baydocs application package code:"/></td>
    <td><asp:DropDownList ID="ddlBaydocsAppPackageCode" runat="server" Width="250px"/></td>
    <td><asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="*" ClientValidationFunction="ValidateBaydocCode" ControlToValidate="ddlBaydocsAppPackageCode" ValidateEmptyText="true"></asp:CustomValidator></td>
    <td>&nbsp;</td>
</tr>
<tr runat="server" id="trBaydocClosingPack">
    <td><asp:Label ID="label137" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Baydocs closing package code:"/></td>
    <td>
        <asp:DropDownList ID="ddlBaydocsClosingPackageCode" runat="server" Width="250px"/>        
    </td>
    <td><asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="*" ClientValidationFunction="ValidateBaydocCode" ControlToValidate="ddlBaydocsClosingPackageCode" ValidateEmptyText="true"></asp:CustomValidator></td>
    <td>&nbsp;</td>
</tr>
<tr>    
    <td><asp:Label ID="label100" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product offer the equity protection feature?"/></td>
    <td><asp:CheckBox ID="cbEquityProtection" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label15" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Rate lock method:"/></td>
    <td><asp:DropDownList ID="ddlProductRateLockMethod" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr runat="server" id="trDaysToLock">
    <td><asp:Label ID="label27" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Number of days to lock principle limit?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbDaysToLock" runat="server" MinValue="0" MaxValue="10000" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td><asp:RequiredFieldValidator ID="rfvDaysToLock" runat="server" ControlToValidate="tbDaysToLock" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator></td>
    <td></td>
</tr>
<tr runat="server" id="trFixedDaysToLock">
    <td><asp:Label ID="label133" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How many days can this product be locked?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbFixedRateLockDays" runat="server" MinValue="1" MaxValue="10000" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td><asp:RequiredFieldValidator ID="rfFixedRateLockDays" runat="server" ControlToValidate="tbFixedRateLockDays" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator></td>
    <td>&nbsp;</td>
</tr>
<tr style="padding-bottom:1px;">
    <td><asp:Label ID="label49" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="What is the floor for the expected rate on this product?"/></td>
    <td><radI:RadNumericTextBox Type="Percent" ID="tbExpectedFloorRate" runat="server" MinValue="0.0000" MaxValue="100" Width="100px" NumberFormat-DecimalDigits="3"></radI:RadNumericTextBox></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label101" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How many days are counseling certificates good?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbCounsActiveDays" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label118" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How many days are title commitments good?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbTitleActiveDays" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label119" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How many days are appraisal inspections good?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbAppraisalActiveDays" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label120" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How many days are pest inspections good?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbPestActiveDays" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label121" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How many days are contractor bids good?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbBidActiveDays" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label122" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How many days are water tests good?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbWaterTestActiveDays" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label123" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How many days are septic inspections good?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbSepticInspActiveDays" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label124" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How many days are oil tank inspections good?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbOilTankInspActiveDays" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label125" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How many days are roof inspections good?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbRoofInspActiveDays" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label126" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How many days are flood certs good?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbFloodCertActiveDays" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label127" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How many days are credit reports good?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbCreditReportActiveDays" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label128" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How many days are LDPs good?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbLDPActiveDays" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label129" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How many days are EPLSs good?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbEPLSActiveDays" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label130" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How many days are CAIVRS good?"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbCaivrsActiveDays" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label102" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow standard guidelines regarding non-borrowing spouses?"/></td>
    <td><asp:CheckBox ID="cbFollowStandardNBSGuides" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label103" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for collecting leases? "/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingLeases" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label104" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product have a minimum age requirement for applying?"/></td>
    <td><asp:CheckBox ID="cbAgeEligRequirementApply" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label108" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for collecting HOA hazard insurance declaration pages?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesCollectingHOAHazardDecPages" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label109" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for reviewing HOA hazard insurance declaration pages?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesReviewingHOAHazardDecPages" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label110" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for collecting master flood insurance declaration pages?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesCollectingMasterFloodDecPages" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label105" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product have a minimum age requirement for closing? "/></td>
    <td><asp:CheckBox ID="cbAgeEligRequirementClose" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label106" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="MinAgeToApply"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbMinAgeToApply" runat="server" MinValue="0" MaxValue="150" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label107" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="MinAgeToClose"/></td>
    <td>
        <radI:RadNumericTextBox Type="Number" ID="tbMinAgeToClose" runat="server" MinValue="0" MaxValue="150" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
    </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label111" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product use SRP locks?"/></td>
    <td><asp:CheckBox ID="cbUseSRPLocksYN" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label112" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for collecting conservator documentation?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingConservator" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label113" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for reviewing leaseholds?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingLeases" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label114" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for reviewing HOA flood insurance declaration pages?"/></td>
    <td><asp:CheckBox ID="cbBasicGuidesReviewingHOAFloodDecPages" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label115" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow HECM guidelines for reviewing a structural inspection reports?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesReviewingStructuralInspections" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr style="padding-bottom:1px;">
    <td><asp:Label ID="label116" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Lending Limit:"/></td>
    <td><radI:RadNumericTextBox Type="Currency" ID="tbLendingLimit" runat="server" MinValue="0" Width="100px" NumberFormat-DecimalDigits="2"></radI:RadNumericTextBox></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label117" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product follow basic guidelines for collecting power of attorney documentation?"/></td>
    <td><asp:CheckBox ID="cbHECMGuidesCollectingPOA" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label131" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product allow taxes and insurance to be escrowed?"/></td>
    <td><asp:CheckBox ID="cbAllowEscrowTaxAndInsurance" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label132" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Does this product allow repairs to be escrowed?"/></td>
    <td><asp:CheckBox ID="cbAllowEscrowRepiars" runat="server" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td><asp:Label ID="label134" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="How often are rates updated?"/></td>
    <td><asp:DropDownList ID="ddlProductRateUpdateInterval" runat="server" DataTextField="Name" DataValueField="ID"></asp:DropDownList></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr id="trProductRateInputMethod" runat="server">
    <td><asp:Label ID="label135" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Choose input method:"/></td>
    <td><asp:DropDownList ID="ddlProductRateInputMethod" runat="server" DataTextField="Name" DataValueField="ID"></asp:DropDownList></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
</asp:Panel>
<tr style="padding-top:5px">
    <td><asp:Label ID="label51" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Servising Fee:"/></td>    
    <td colspan="3">
        <radG:RadGrid ID="G" Skin="WebBlue" GridLines="None" EnableAJAX="false" runat="server" AllowSorting="false"
            Width="180px" AllowPaging="False" AutoGenerateColumns="False" OnItemCommand="G_ItemCommand" AllowMultiRowEdit="false" OnItemCreated="G_ItemCreated" OnItemDataBound="G_ItemDataBound" >
            <ClientSettings>
            <Resizing AllowColumnResize="false" EnableRealTimeResize="false"/>
            </ClientSettings>
                <MasterTableView AllowNaturalSort="False" EditMode="InPlace" CommandItemDisplay="Bottom" DataKeyNames="ID" AutoGenerateColumns="False" CommandItemSettings-AddNewRecordText="Add new fee">
                    <Columns>
                        <radG:GridTemplateColumn HeaderText="Fee">
                            <ItemTemplate>
                                <asp:Label ID="lblValue" runat="server" Text='<%#string.Format("{0:C}", Eval("Fee"))%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <radI:RadNumericTextBox ID="tbFee" runat="server" Type="Currency" MinValue="0" Width="60px"></radi:RadNumericTextBox>
                            </EditItemTemplate>                            
                            <HeaderStyle HorizontalAlign="Left" Width="100px"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Default" ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDefault" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsDefault") %>' Enabled="false"></asp:CheckBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbDefaultEdit" runat="server"></asp:CheckBox>
                            </EditItemTemplate>                            
                            <HeaderStyle HorizontalAlign="Center" Width="50px"/>
                        </radG:GridTemplateColumn>                        
                        <radg:GridEditCommandColumn ButtonType="ImageButton" UpdateImageUrl="~/Images/Update.gif" 
                            EditImageUrl="~/Images/Edit.gif" InsertImageUrl="~/Images/Insert.gif" 
                            CancelImageUrl="~/Images/Cancel.gif" UniqueName="EditCommandColumn">
                                <HeaderStyle Width="20px" /> 
                        </radg:GridEditCommandColumn>
                        <radG:GridButtonColumn ConfirmText="Delete this fee?" ButtonType="ImageButton" ImageUrl="/Images/Delete.gif" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" >
                            <HeaderStyle Width="20px" />
                            <ItemStyle HorizontalAlign="Center"/>
                        </radG:GridButtonColumn> 
                    </Columns>
                </MasterTableView>
                <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
            </radG:RadGrid>
    </td>
</tr>
<tr style="padding:5px">
    <td colspan="4">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <asp:LinkButton ID="lbViewRates" runat="server" Text="View rate data" OnClick="lbViewRates_Click" CausesValidation="false"></asp:LinkButton><br /><br /><br />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnBack" runat="server" Text="Back" SkinID="AdminButton" OnClick="btnBack_Click" CausesValidation="False"/>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnSave" runat="server" Text="Save"  SkinID="AdminButton" OnClick="btnSave_Click"/>
                </td>
            </tr>
        </table>        
    </td>
</tr>
</table>
</asp:Panel>
</div>