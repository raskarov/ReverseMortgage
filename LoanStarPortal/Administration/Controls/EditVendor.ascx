<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditVendor.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditVendor" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<script language="javascript" type="text/javascript">
<!--
function CheckAllCounties(o,d1){
    var d = document.getElementById(d1);
    var e = d.getElementsByTagName('input');
    for (var i=0; i<e.length; i++){
        if (e[i].type=='checkbox'){
            e[i].checked=o.checked;
            SetCbStyle(e[i]);
        }
    }
}
function SetCbStyle(o){
    var cn ="class";
    if(document.all) cn='className';
    var css=o.checked?"lbstateallcounty":"lbstatenocounty";
    var p =o.parentNode;
    p.setAttribute(cn,css);
}
function CheckCounty(o,o11,d1){
    SetCbStyle(o);
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
function ValidateLogin(src, arg ){
    arg.IsValid = true; //arg.Value.length>=6;
}
function ValidatePassword(src, arg ){
    arg.IsValid=true; 
    //var c=src.getAttribute('controltovalidate') ;
    //var o = document.getElementById(c.replace('tbPassword','CbSetPassword'));
    //var isValid=false;
    //if(o!=null){
    //    if(!o.checked){
    //        arg.IsValid=true;
    //            isValid=true;   
    //    }
    //}
    //if (!isValid){
    //        o = document.getElementById(c.replace('tbPassword','tbLogin'));
    //    if(o.value==''){
    //        arg.IsValid=true;
    //    }else{
    //        arg.IsValid = arg.Value.length>=6;
    //    }
    //}
}
function ValidateConfirmPassword(src, arg ){
    arg.IsValid=true;
    //var c=src.getAttribute('controltovalidate') ;
    //var o=document.getElementById(c.replace('tbConfirmPassword','tbPassword'));
    //arg.IsValid = o.value==arg.Value;
}
function SetVisibility(o,t1,t2){
    var vis=o.checked?'block':'none';
    var o=document.getElementById(t1);
    if(o!=null){
        o.style.display=vis;
    }
    o=document.getElementById(t2);
    if(o!=null){
        o.style.display=vis;
    }
}
function SetAffiliatedVisibility(o,t1,t2,t3){
    var vis=o.checked?'block':'none';
    var o=document.getElementById(t1);
    if(o!=null){
        o.style.display=vis;
    }
    o=document.getElementById(t2);
    if(o!=null){
        o.style.display=vis;
    }
    o=document.getElementById(t3);
    if(o!=null){
        o.style.display=vis;
    }
}
-->
</script>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
<tr>
    <td align="center"><asp:Label ID="lblHeader" runat="server" Text="" SkinID="AdminHeader"></asp:Label></td>
</tr>
<tr>
    <td align="center"><asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label></td>
</tr>
<tr>
    <td style="padding-left:10px;">
        <radTS:RadTabStrip id="VendorInfo" runat="server" Skin="Outlook" MultiPageID="RadMultiPage1" SelectedIndex="0" CausesValidation="false" Orientation="HorizontalTopToBottom">
        <Tabs>
            <radts:Tab Text="Basic Information" Value="BasicInfo" ID="tabBasicInfo" runat="server"></radts:Tab>
            <radts:Tab Text="Fee types" Value="FeeTypes" ID="tabFeeTypes" runat="server"></radts:Tab>
            <radts:Tab Text="Geographical Filters" Value="GeoFilters" ID="tabGeoFilters" runat="server"></radts:Tab>
            <radts:Tab Text="Always/Never Settings" Value="AlwaysNever" ID="tabAlwaysNever" runat="server"></radts:Tab>
        </Tabs>
        </radTS:RadTabStrip>
        <radTS:RadMultiPage id="RadMultiPage1" runat="server" SelectedIndex="0" Height="85%" EnableViewState="False">
            <radTS:PageView id="pvbasicInfo" runat="server" EnableViewState="False">
                <div style="border:1px;border-color:Black;border-style:solid;padding-left:6px;padding-top:6px;padding-bottom:5px;">
                <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label2" runat="server" Text="Company Name"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbCompanyName" runat="server" MaxLength="256" Width="280px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfCompany" runat="server" ErrorMessage="*" ControlToValidate="tbCompanyName"></asp:RequiredFieldValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label1" runat="server" Text="Corporate Address1"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbCorporateAddress1" runat="server" MaxLength="253" Width="280px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfCorporateAddress" runat="server" ErrorMessage="*" ControlToValidate="tbCorporateAddress1"></asp:RequiredFieldValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label26" runat="server" Text="Corporate Address2"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbCorporateAddress2" runat="server" MaxLength="253" Width="280px"></asp:TextBox>                            
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label27" runat="server" Text="Main phone"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <radI:RadMaskedTextBox runat="server" ID="tbCompanyPhone" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="tbCompanyPhone"></asp:RequiredFieldValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label3" runat="server" Text="Main fax"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <radI:RadMaskedTextBox runat="server" ID="tbCompanyFax" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>
                            <asp:RequiredFieldValidator ID="rfFax" runat="server" ErrorMessage="*" ControlToValidate="tbCompanyFax"></asp:RequiredFieldValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label28" runat="server" Text="Email"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbCompanyEmail" runat="server" MaxLength="512"  Width="280px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ControlToValidate="tbCompanyEmail"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*" SetFocusOnError="True" ControlToValidate="tbCompanyEmail"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                       </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label18" runat="server" Text="City"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbCompanyCity" runat="server" MaxLength="50"  Width="280px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="tbCompanyCity"></asp:RequiredFieldValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label19" runat="server" Text="State"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:DropDownList ID="ddlCompanyState" runat="server"  Width="200px" EnableViewState="false"></asp:DropDownList>
                            <asp:RangeValidator ID="rvddlAmortType" runat="server" ErrorMessage="*" MinimumValue="1" MaximumValue="1000" ControlToValidate="ddlCompanyState" Type="Integer"></asp:RangeValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label20" runat="server" Text="Zip"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <radI:RadMaskedTextBox runat="server" ID="tbCompanyZip" Mask="#####" DisplayMask="#####" Width="100px"></radI:RadMaskedTextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="tbCompanyZip"></asp:RequiredFieldValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr> 
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label4" runat="server" Text="Billing address 1"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbBillingAddress1" runat="server" MaxLength="256"  Width="280px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfBillingAddress" runat="server" ErrorMessage="*" ControlToValidate="tbBillingAddress1"></asp:RequiredFieldValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label29" runat="server" Text="Billing address 2"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbBillingAddress2" runat="server" MaxLength="256"  Width="280px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" ControlToValidate="tbBillingAddress2"></asp:RequiredFieldValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label30" runat="server" Text="City"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbBillingCity" runat="server" MaxLength="50"  Width="280px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" ControlToValidate="tbBillingCity"></asp:RequiredFieldValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label31" runat="server" Text="State"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:DropDownList ID="ddlBillingState" runat="server"  Width="200px" EnableViewState="false"></asp:DropDownList>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="*" MinimumValue="1" MaximumValue="1000" ControlToValidate="ddlBillingState" Type="Integer"></asp:RangeValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label32" runat="server" Text="Zip"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <radI:RadMaskedTextBox runat="server" ID="tbBillingZip" Mask="#####" DisplayMask="#####" Width="100px"></radI:RadMaskedTextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="*" ControlToValidate="tbBillingZip"></asp:RequiredFieldValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label5" runat="server" Text="Primary Contact"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbPrimaryContact" runat="server" MaxLength="256"  Width="280px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfPrimaryContact" runat="server" ErrorMessage="*" ControlToValidate="tbPrimaryContact"></asp:RequiredFieldValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label6" runat="server" Text="Phone 1"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <radI:RadMaskedTextBox runat="server" ID="tbPCPhone1" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>
                            <asp:RequiredFieldValidator ID="rfPhone1" runat="server" ErrorMessage="*" ControlToValidate="tbPCPhone1"></asp:RequiredFieldValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label7" runat="server" Text="Phone 2"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <radI:RadMaskedTextBox runat="server" ID="tbPCPhone2" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>                            
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label8" runat="server" Text="Email"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbPCEmail" runat="server" MaxLength="512"  Width="280px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfEmail" runat="server" ErrorMessage="*" ControlToValidate="tbPCEmail"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="reEmail" runat="server" ErrorMessage="*" SetFocusOnError="True" ControlToValidate="tbPCEmail"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                       </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label9" runat="server" Text="Secondary Contact"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbSecondaryContact" runat="server" MaxLength="256"  Width="280px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label10" runat="server" Text="Phone 1"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <radI:RadMaskedTextBox runat="server" ID="tbSCPhone1" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>                            
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label11" runat="server" Text="Phone 2"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <radI:RadMaskedTextBox runat="server" ID="tbSCPhone2" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>                            
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label12" runat="server" Text="Email"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbSCEmail" runat="server" MaxLength="512" Width="280px"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="reSCEmail" runat="server" ErrorMessage="*" SetFocusOnError="True"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbSCEmail"></asp:RegularExpressionValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label13" runat="server" Text="License expire date"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <radI:RadDateInput runat="server" ID="dtLicenseExpdate"></radI:RadDateInput>                            
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr style="height:20px;padding-top:3px;">
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label25" runat="server" Text="Delivery method"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:DropDownList ID="ddlDeliveryMethod" runat="server" ></asp:DropDownList>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label14" runat="server" Text="Disable vendor "></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl"> 
                            <asp:CheckBox ID="cbDisabled" runat="server" />
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr id="trLogin" runat="server">
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label15" runat="server" Text="Login"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbLogin" runat="server" MaxLength="20"  Width="120px"></asp:TextBox>
                            <asp:CustomValidator ID="RequiredFieldValidator7" runat="server" ClientValidationFunction="ValidateLogin"  ControlToValidate="tbLogin" ErrorMessage="Login must be at least 6 characters long" ValidateEmptyText="False"></asp:CustomValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr runat="server" id="trSetPassword">
                        <td class="vendorbasicinfolbl">
                            <asp:CheckBox ID="cbSetPassword" runat="server" Text="Set/Change password"></asp:CheckBox>
                        </td>
                        <td class="vendorbasicinfoctl">
                        </td>
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr id="trPassword" style="display:none" runat="server">
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label16" runat="server" Text="Password"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbPassword" runat="server" MaxLength="20"  Width="120px" TextMode="Password"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidatePassword"  ControlToValidate="tbPassword" ErrorMessage="Password must be at least 6 characters long" ValidateEmptyText="true"></asp:CustomValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr id="trConfirmPassword" style="display:none" runat="server">
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label17" runat="server" Text="Retype Password"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbConfirmPassword" runat="server" MaxLength="20"  Width="120px" TextMode="Password"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ValidateConfirmPassword"  ControlToValidate="tbConfirmPassword" ErrorMessage="Passwords you have typed note identical" ValidateEmptyText="true"></asp:CustomValidator>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label21" runat="server" Text="Is this vendor affiliated with any RM LOS company?"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:CheckBox  ID="cbIsAffiliatedWithOriginator" runat="server" ></asp:CheckBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr id="trCompany" runat="server">
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label22" runat="server" Text="Select company"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:DropDownList ID="ddlCompany" runat="server" ></asp:DropDownList>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr id="trRelationship" runat="server">
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label23" runat="server" Text="Relationship"></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbRelationship" runat="server" MaxLength="256"  Width="280px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr id="trShortDescriptionOfServices" runat="server">
                        <td class="vendorbasicinfolbl">
                            <asp:Label ID="Label24" runat="server" Text="Short description of services "></asp:Label>
                        </td>
                        <td class="vendorbasicinfoctl">
                            <asp:TextBox ID="tbShortDescriptionOfServices" runat="server" MaxLength="512"  Width="280px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr style="padding-top:5px">
                        <td>&nbsp;</td>
                        <td><asp:Button ID="btnSaveBasicInfo" runat="server" Text="Save" OnClick="btnSaveBasicInfo_Click"/> </td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                </div>
            </radTS:PageView>
            <radTS:PageView ID="pvFeetypes" runat="server" EnableViewState="False">
                <div style="border:1px;border-color:Black;border-style:solid;padding-left:6px;padding-top:6px;padding-bottom:5px;">
                <radTS:RadTabStrip ID="tsFee" runat="server" Skin="Outlook" OnTabDataBound="tsFee_TabDataBound" MultiPageID="mpFees" EnableViewState="true">
                </radTS:RadTabStrip>
                <radTS:RadMultiPage ID="mpFees" runat="server" Height="310px" Width="100%" EnableViewState="True" AutoScrollBars="false">        
                </radTS:RadMultiPage>                
                <br />
                <table border="0" style="width:100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width:220px">&nbsp;&nbsp;</td>
                        <td><asp:Button ID="btnSaveFeeAmount" runat="server" Text="Save" OnClick="btnSaveFeeAmount_Click" CausesValidation="false"/></td>
                    </tr>
                </table>
                </div>
            </radTS:PageView>
            <radTS:PageView ID="pvGeoFilter" runat="server" EnableViewState="False">
                <div style="border:1px;border-color:Black;border-style:solid;padding-left:6px;padding-top:6px;padding-bottom:5px;">
                <radTS:RadTabStrip ID="tsGeoFilter" runat="server" Skin="Outlook" MultiPageID="mpGeoFilter" EnableViewState="true">
                <Tabs>
                    <radts:Tab Text="States" Value="States" ID="tabStates" runat="server"></radts:Tab>
                    <radts:Tab Text="Select State" Value="SelecedState" ID="tbCounties" runat="server"></radts:Tab>
                </Tabs>
                </radTS:RadTabStrip>
                <radTS:RadMultiPage id="mpGeoFilter" runat="server" SelectedIndex="0" Height="85%" EnableViewState="False">
                    <radTS:PageView ID="pvStates" runat="server" EnableViewState="False">
                        <table border="0" style="width:100%;vertical-align:top;margin-top:10px;margin-bottom:10px;" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:DataList ID="dlStates" runat="server" RepeatColumns="3" RepeatDirection="Vertical" OnItemDataBound="dlStates_ItemDataBound"  EnableViewState="False">
                                         <ItemTemplate>
                                            <asp:LinkButton ID="lbState" runat="server"></asp:LinkButton>
                                         </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                        </table>                                
                    </radTS:PageView>
                    <radTS:PageView ID="pvCounties" runat="server" EnableViewState="False">
                        <table border="0" style="width:100%;vertical-align:top;margin-top:10px;margin-bottom:10px;" cellpadding="0" cellspacing="0">
                            <tr style="padding-bottom:10px"><td style="padding-left:20px"><asp:CheckBox ID="cbAllCounty" runat="server"  Text="Check/Uncheck All"/></td></tr>
                            <tr>
                                <td>
                                    <div id="countydiv" runat="server">
                                    <asp:DataList ID="dlCounty" runat="server" RepeatColumns="4" RepeatDirection="Vertical" OnItemDataBound="dlCounties_ItemDataBound"  EnableViewState="False">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbCounty" runat="server" TextAlign="Right"/>
                                        </ItemTemplate>
                                    </asp:DataList>
                                    </div>
                                </td>
                            </tr>
                            <tr style="padding-top:10px"><td style="padding-left:5px"><asp:Button ID="btnSaveCounty" runat="server" Text="Save" OnClick="btnSaveCounty_Click"  CausesValidation="false"/></td></tr>
                        </table>                                
                    </radTS:PageView>
                </radTS:RadMultiPage>                
                </div>
            </radTS:PageView>
            <radTS:PageView ID="pvAlwaysNever" runat="server" EnableViewState="False">
                <div style="border:1px;border-color:Black;border-style:solid;padding-left:6px;padding-top:6px;padding-bottom:5px;">
                    <table border="0" width="500px" cellpadding="0" cellspacing="0">
                        <tr>    
                            <td>
                                <asp:GridView ID="gSettings" runat="server" SkinID="TotalGrid" AutoGenerateColumns = "false" EnableViewState = "true" AllowPaging="False" AllowSorting="true" 
                                    OnRowDeleting = "G_RowDeleting"
                                    OnRowCancelingEdit="G_RowCancel" 
                                    OnRowEditing="G_RowEditing" 
                                    OnRowUpdating="G_RowUpdating" 
                                    OnRowDataBound="G_RowDataBound" 
                                    EmptyDataText="No records to display" 
                                    OnRowCommand="G_RowCommand" 
                                    OnSorting="G_Sorting" 
                                    OnPageIndexChanged="G_PageIndexChanged" 
                                    OnPageIndexChanging="G_PageIndexChanging"
                                >
                                <columns>
                                <asp:TemplateField HeaderText="Originator" SortExpression="OriginatorName">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblOriginator" EnableViewState="false" Text='<%# DataBinder.Eval(Container.DataItem,"OriginatorName") %>'></asp:Label>
                                        <asp:DropDownList ID="ddlOriginator" runat="server" Visible="false"/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" Width="350px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Settings">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSettings" EnableViewState="false" Text='<%# GetAlwaysNeverSettings(Container.DataItem) %>'></asp:Label>
                                        <asp:DropDownList ID="ddlSettings" runat="server" Visible="false"/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgEdit" CommandName="Edit" CommandArgument='<%# GetCurrentRow() %>' runat="server" ImageUrl="~/images/edit.gif"/>
                                        <asp:ImageButton ID="imgDelete" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' runat="server" ImageUrl="~/images/delete.gif"/>
                                        <asp:ImageButton ID="imgUpdate" CommandName="Update" runat="server" ImageUrl="~/images/update.gif" Visible="false"/>
                                        <asp:ImageButton ID="imgCancel" CommandName="Cancel" runat="server" CausesValidation="false" ImageUrl="~/images/cancel.gif" Visible="false"/>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lbl" EnableViewState="false"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                </asp:TemplateField>
                                </columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </radTS:PageView>
        </radTS:RadMultiPage>
    </td>
</tr>
<tr style="padding-top:10px;padding-left:10px">
    <td><asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CausesValidation="false" /></td>
</tr>
</table>