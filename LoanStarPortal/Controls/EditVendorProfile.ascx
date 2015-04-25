<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditVendorProfile.ascx.cs" Inherits="LoanStarPortal.Controls.EditVendorProfile" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<table border="1" cellpadding="0" cellspacing="0" style="width:90%;">
    <tr style="padding-top:20px;">
        <td class="vendorprofileblbl">&nbsp;</td>
        <td class="vendorprofilectl"><asp:Label ID="Label13" runat="server" Text="Company information" Font-Bold="true"></asp:Label></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label2" runat="server" Text="Company Name"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <asp:TextBox ID="tbCompanyName" runat="server" MaxLength="256" Width="300px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfCompany" runat="server" ErrorMessage="*" ControlToValidate="tbCompanyName"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label1" runat="server" Text="Corporate Address1"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <asp:TextBox ID="tbCorporateAddress1" runat="server" MaxLength="253" Width="300px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfCorporateAddress" runat="server" ErrorMessage="*" ControlToValidate="tbCorporateAddress1"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label26" runat="server" Text="Corporate Address2"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <asp:TextBox ID="tbCorporateAddress2" runat="server" MaxLength="253" Width="300px"></asp:TextBox>                            
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label27" runat="server" Text="Main phone"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <radI:RadMaskedTextBox runat="server" ID="tbCompanyPhone" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="tbCompanyPhone"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>                    
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label3" runat="server" Text="Main fax"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <radI:RadMaskedTextBox runat="server" ID="tbCompanyFax" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>
            <asp:RequiredFieldValidator ID="rfFax" runat="server" ErrorMessage="*" ControlToValidate="tbCompanyFax"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label28" runat="server" Text="Email"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <asp:TextBox ID="tbCompanyEmail" runat="server" MaxLength="512"  Width="300px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ControlToValidate="tbCompanyEmail"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*" SetFocusOnError="True" ControlToValidate="tbCompanyEmail"
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
       </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label18" runat="server" Text="City"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <asp:TextBox ID="tbCompanyCity" runat="server" MaxLength="50"  Width="300px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="tbCompanyCity"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label19" runat="server" Text="State"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <asp:DropDownList ID="ddlCompanyState" runat="server"  Width="200px" EnableViewState="false"></asp:DropDownList>
            <asp:RangeValidator ID="rvddlAmortType" runat="server" ErrorMessage="*" MinimumValue="1" MaximumValue="1000" ControlToValidate="ddlCompanyState" Type="Integer"></asp:RangeValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label20" runat="server" Text="Zip"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <radI:RadMaskedTextBox runat="server" ID="tbCompanyZip" Mask="#####" DisplayMask="#####" Width="100px"></radI:RadMaskedTextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="tbCompanyZip"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr style="padding-top:10px;">
        <td class="vendorprofileblbl">&nbsp;</td>
        <td class="vendorprofilectl"><asp:Label ID="Label14" runat="server" Text="Billing address" Font-Bold="true"></asp:Label></td>
        <td>&nbsp;</td>
    </tr>    
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label4" runat="server" Text="Billing address 1"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <asp:TextBox ID="tbBillingAddress1" runat="server" MaxLength="256"  Width="300px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfBillingAddress" runat="server" ErrorMessage="*" ControlToValidate="tbBillingAddress1"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label29" runat="server" Text="Billing address 2"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <asp:TextBox ID="tbBillingAddress2" runat="server" MaxLength="256"  Width="300px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" ControlToValidate="tbBillingAddress2"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label30" runat="server" Text="City"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <asp:TextBox ID="tbBillingCity" runat="server" MaxLength="50"  Width="300px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" ControlToValidate="tbBillingCity"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label31" runat="server" Text="State"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <asp:DropDownList ID="ddlBillingState" runat="server"  Width="200px" EnableViewState="false"></asp:DropDownList>
            <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="*" MinimumValue="1" MaximumValue="1000" ControlToValidate="ddlBillingState" Type="Integer"></asp:RangeValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label32" runat="server" Text="Zip"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <radI:RadMaskedTextBox runat="server" ID="tbBillingZip" Mask="#####" DisplayMask="#####" Width="100px"></radI:RadMaskedTextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="*" ControlToValidate="tbBillingZip"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr style="padding-top:10px;">
        <td class="vendorprofileblbl">&nbsp;</td>
        <td class="vendorprofilectl"><asp:Label ID="Label15" runat="server" Text="Primary Contact" Font-Bold="true"></asp:Label></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label5" runat="server" Text="Name"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <asp:TextBox ID="tbPrimaryContact" runat="server" MaxLength="256"  Width="300px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfPrimaryContact" runat="server" ErrorMessage="*" ControlToValidate="tbPrimaryContact"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label6" runat="server" Text="Phone 1"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <radI:RadMaskedTextBox runat="server" ID="tbPCPhone1" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>
            <asp:RequiredFieldValidator ID="rfPhone1" runat="server" ErrorMessage="*" ControlToValidate="tbPCPhone1"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label7" runat="server" Text="Phone 2"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <radI:RadMaskedTextBox runat="server" ID="tbPCPhone2" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>                            
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label8" runat="server" Text="Email"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <asp:TextBox ID="tbPCEmail" runat="server" MaxLength="512"  Width="300px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfEmail" runat="server" ErrorMessage="*" ControlToValidate="tbPCEmail"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="reEmail" runat="server" ErrorMessage="*" SetFocusOnError="True" ControlToValidate="tbPCEmail"
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
       </td>
        <td>&nbsp;</td>
    </tr>
    <tr style="padding-top:10px;">
        <td class="vendorprofileblbl">&nbsp;</td>
        <td class="vendorprofilectl"><asp:Label ID="Label16" runat="server" Text="Secondary Contact" Font-Bold="true"></asp:Label></td>
        <td>&nbsp;</td>
    </tr>    
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label9" runat="server" Text="Name"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <asp:TextBox ID="tbSecondaryContact" runat="server" MaxLength="256"  Width="300px"></asp:TextBox>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label10" runat="server" Text="Phone 1"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <radI:RadMaskedTextBox runat="server" ID="tbSCPhone1" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>                            
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label11" runat="server" Text="Phone 2"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <radI:RadMaskedTextBox runat="server" ID="tbSCPhone2" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>                            
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="vendorprofileblbl">
            <asp:Label ID="Label12" runat="server" Text="Email"></asp:Label>
        </td>
        <td class="vendorprofilectl">
            <asp:TextBox ID="tbSCEmail" runat="server" MaxLength="512" Width="300px"></asp:TextBox>
            <asp:RegularExpressionValidator ID="reSCEmail" runat="server" ErrorMessage="*" SetFocusOnError="True"
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tbSCEmail"></asp:RegularExpressionValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
</table>