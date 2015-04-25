<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReserveEdit.ascx.cs" Inherits="LoanStarPortal.Controls.ReserveEdit" %>
<%@ Register Namespace="Telerik.WebControls" TagPrefix="radI" Assembly="RadInput.NET2"%>

<table border="0" cellspacing="3" cellpadding="3" align="center" style="width:99%;">
    <tr>
        <td class="editFormLabel">Type: </td>
        <td>&nbsp;<asp:DropDownList ID="ddlType" runat="server" DataTextField="Name" DataValueField="ID" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="Description" ErrorMessage="*"></asp:RequiredFieldValidator></td>
        <td class="editFormLabel">Months needed: </td>
        <td>&nbsp;<asp:TextBox ID="Months" runat="server" MaxLength="3" Width="150px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="Months" ErrorMessage="*"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="*" ControlToValidate="Months" Type="Integer" MinimumValue="0" MaximumValue="999"></asp:RangeValidator></td>
    </tr>
    <tr id="trDescription" runat="server" visible="false">
        <td class="editFormLabel">Description: </td>
        <td>&nbsp;<asp:TextBox ID="Description" runat="server" Width="145px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="editFormLabel">Statement Start: </td>
        <td><radI:RadDateInput ID="StatStart" runat="server" Width="150px" MinDate="1900-01-01"></radI:RadDateInput></td>
        <td class="editFormLabel">Statement End: </td>
        <td><radI:RadDateInput ID="StatEnd" runat="server" Width="150px" MinDate="1900-01-01"></radI:RadDateInput></td>
    </tr>
    <tr>
        <td class="editFormLabel">Charge to: </td>
        <td>&nbsp;<asp:DropDownList ID="ddlChargeTo" runat="server" CssClass="ddl" Width="150px"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlChargeTo" ErrorMessage="*"></asp:RequiredFieldValidator></td>
        <td class="editFormLabel">Statement Amount: </td>
        <td><radI:RadNumericTextBox Skin="Default" Type="Currency" Width="150px" ID="Amount" runat="server" MinValue="0"></radI:RadNumericTextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="Amount" ErrorMessage="*"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td align="right" colspan="2">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="publicEditFormButton" CommandName="Update" OnClick="btnSave_Click" />&nbsp;
        </td>
        <td colspan="2">
         &nbsp<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="publicEditFormButton" CommandName="Cancel" OnClick="btnCancel_Click"/>            
        </td>
    </tr>    
</table>