<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MortgagePrepaidItemEdit.ascx.cs" Inherits="LoanStarPortal.Controls.MortgagePrepaidItemEdit" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<div style="background-color:Gray;padding:5px">
<table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color:White;padding:4px;">
    <tr>
        <td style="width:100px">
            <asp:Label ID="Label1" runat="server" Text="Description" CssClass="editFormLabel"></asp:Label>
        </td>
        <td style="width:120px">
            <asp:TextBox ID="tbDescription" runat="server" SkinID="PublicEdicTextBox" MaxLength="256"></asp:TextBox>
        </td>
        <td style="width:10px">
            <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ErrorMessage="*" ControlToValidate="tbDescription" Width="8px"></asp:RequiredFieldValidator>
        </td>
        <td style="width:100px;padding-left:20px">
            <asp:Label ID="Label2" runat="server" Text="To" CssClass="editFormLabel"></asp:Label>
        </td>
        <td style="width:120px">
            <asp:TextBox ID="tbPaymentTo" runat="server" SkinID="PublicEdicTextBox" MaxLength="256" ></asp:TextBox>
        </td>
        <td style="width:10px">
            <asp:RequiredFieldValidator ID="rfvPaymentTo" runat="server" ErrorMessage="*" ControlToValidate="tbPaymentTo" Width="8px"></asp:RequiredFieldValidator>
        </td>        
        <td>&nbsp;</td>        
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label3" runat="server" Text="Amount" CssClass="editFormLabel"></asp:Label>
        </td>
        <td>
            <radI:RadNumericTextBox ShowSpinButtons="false" Type="Currency" ID="tbAmount" runat="server" ></radI:RadNumericTextBox>
        </td>
        <td style="width:10px">
            <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ErrorMessage="*" ControlToValidate="tbDescription" Width="8px"></asp:RequiredFieldValidator>
        </td>        
        <td style="padding-left:20px">
            <asp:Label ID="Label4" runat="server" Text="Unit" CssClass="editFormLabel"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlUnit" runat="server"></asp:DropDownList>&nbsp;            
        </td>
        <td style="width:10px">
            <asp:RangeValidator ID="rvUnit" runat="server" ErrorMessage="*" MinimumValue="1" Type="Integer" MaximumValue="100000" ControlToValidate="ddlUnit" Width="8px"></asp:RangeValidator> 
        </td>
        <td>&nbsp;</td>        
    </tr>    
    <tr>
        <td>
            <asp:Label ID="Label5" runat="server" Text="Statement start" CssClass="editFormLabel"></asp:Label>
        </td>
        <td>
            <radCln:RadDatePicker ID="diStatementStart" AllowEmpty="false" runat="server">
                    <DateInput Height="18px" style="color:#000000;font-size:12px;font-family:Arial, Helvetica, sans-serif;padding-left:4px;border:1px solid #7F9DB9;background:#FFFFFF;"></DateInput>
                    <Calendar Skin="WebBlue"></Calendar>
            </radCln:RadDatePicker>
        </td>
        <td style="width:10px">&nbsp;</td>
        <td style="padding-left:20px">
            <asp:Label ID="Label6" runat="server" Text="Statement end" CssClass="editFormLabel"></asp:Label>
        </td>
        <td>
            <radCln:RadDatePicker ID="diStatementEnd" AllowEmpty="false" runat="server" >
                    <DateInput Height="18px" style="color:#000000;font-size:12px;font-family:Arial, Helvetica, sans-serif;padding-left:4px;border:1px solid #7F9DB9;background:#FFFFFF;"></DateInput>
                    <Calendar Skin="WebBlue"></Calendar>
            </radCln:RadDatePicker>        
        </td>
        <td style="width:10px">&nbsp;</td>
        <td>&nbsp;</td>        
    </tr> 
    <tr>
        <td colspan="7" align="right" style="padding-right:40px">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="publicEditFormButton" CommandName="Update" OnClick="btnSave_Click" />&nbsp;&nbsp;&nbsp<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="publicEditFormButton" CommandName="Cancel" OnClick="btnCancel_Click"/>            
        </td>
    </tr>    
</table>
</div>