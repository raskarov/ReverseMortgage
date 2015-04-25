<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendorContactsEdit.ascx.cs" Inherits="LoanStarPortal.Controls.VendorContactsEdit" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<table border="0" cellpadding="0" cellspacing="0" style="width:100%;padding-top:5px;padding-bottom:5px;">
    <tr>
        <td class="contactedit">Name:</td>
        <td style="padding-left:6px;" class="contacteditc">
            <asp:TextBox runat="server" ID="tbName" MaxLength="256" Width="300px"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="*" ControlToValidate="tbName" Width="10px"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>        
        <td class="contactedit">Phone:</td>
        <td class="contacteditc">
            <radI:RadMaskedTextBox runat="server" ID="tbPnone" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>
        </td>        
        <td>&nbsp;</td>
    </tr>   
    <tr>        
        <td class="contactedit">Alt Phone:</td>
        <td class="contacteditc">
            <radI:RadMaskedTextBox runat="server" ID="tbAltPhone" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>
        </td>        
        <td>&nbsp;</td>
    </tr>   
    <tr>        
        <td class="contactedit">Fax:</td>
        <td class="contacteditc">
            <radI:RadMaskedTextBox runat="server" ID="tbFax" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>
        </td>        
        <td>&nbsp;</td>
    </tr>
    <tr>        
        <td class="contactedit">Cell Phone:</td>
        <td class="contacteditc">
            <radI:RadMaskedTextBox runat="server" ID="tbCellPhone" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>
        </td>        
        <td>&nbsp;</td>
    </tr>
    <tr>        
        <td class="contactedit">Address:</td>
        <td style="padding-left:6px;padding-top:3px" class="contacteditc">
            <asp:TextBox runat="server" ID="tbAddress" TextMode="MultiLine" Rows="3" Width="300px"></asp:TextBox>
        </td>        
        <td>&nbsp;</td>
    </tr>    
    <tr runat="server" id="trSettlementAgent">
        <td class="contactedit">Settlement Agent:</td>
        <td class="contacteditc">
            <asp:CheckBox runat="server" ID="cbIsSettlementAgent" ></asp:CheckBox>
        </td>        
        <td>&nbsp;</td>
    </tr>
    <tr runat="server" id="trTitleAgent">
        <td class="contactedit">Title Agent:</td>
        <td class="contacteditc">
            <asp:CheckBox runat="server" ID="cbIsTitleAgent" ></asp:CheckBox>
        </td>        
        <td>&nbsp;</td>
    </tr>
    <tr>        
        <td class="contactedit">Email:</td>
        <td style="padding-left:6px;" class="contacteditc">
            <asp:TextBox runat="server" ID="tbEmail" MaxLength="512" Width="300px"></asp:TextBox>
            <asp:RegularExpressionValidator ID="reEmail" runat="server" ErrorMessage="*" SetFocusOnError="True" ControlToValidate="tbEmail"  ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
        </td>        
        <td>&nbsp;</td>
    </tr>
    <tr style="padding-top:5px">
        <td class="contactedit">&nbsp;</td>
        <td>
            <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                <tr>
                    <td style="width:250px"><asp:Button ID="btnSave" runat="server" Text="Save" CssClass="publicEditFormButton" CommandName="Update" OnClick="btnSave_Click"/></td>
                    <td><asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="publicEditFormButton" CommandName="Cancel" OnClick="btnCancel_Click"/></td>
                </tr>
            </table>           
        </td>
        <td>&nbsp;</td>
    </tr>
</table>
