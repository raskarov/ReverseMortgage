<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendorEdit.ascx.cs" Inherits="LoanStarPortal.Controls.VendorEdit" %>
<asp:Label ID="lblExists" runat="server" Text="Vendor with such login already exists." Visible="false"/>
<table border="0" cellspacing="3" cellpadding="3" align="center" style="width:99%;">
    <tr>
        <td class="editFormLabel">Type: </td>
        <td><asp:DropDownList ID="ddlType" runat="server" TabIndex="1" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true" Width="155px"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlType" ErrorMessage="*"></asp:RequiredFieldValidator></td>
        <td class="editFormLabel">Company name: </td>
        <td><asp:TextBox ID="CompanyName" runat="server" Width="150px" CssClass="ddl" TabIndex="2"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="CompanyName" ErrorMessage="*"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td class="editFormLabel">First name: </td>
        <td><asp:TextBox ID="FirstName" runat="server" Width="150px" CssClass="ddl" TabIndex="3"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="FirstName" ErrorMessage="*"></asp:RequiredFieldValidator></td>
        <td class="editFormLabel">Last name: </td>
        <td><asp:TextBox ID="LastName" runat="server" Width="150px" CssClass="ddl" TabIndex="4"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="LastName" ErrorMessage="*"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td class="editFormLabel" valign="top">Address: </td>
        <td colspan="3" valign="top"><asp:TextBox ID="Address" runat="server" CssClass="ddl" TextMode="MultiLine" MaxLength="500" Columns="27" Rows="4" TabIndex="5"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Address" ErrorMessage="*"></asp:RequiredFieldValidator></td>
    </tr>
    <asp:Panel ID="PanelTitleCompanies" runat="server">
    <tr>
        <td class="editFormLabel">Phone number: </td>
        <td><asp:TextBox ID="PhoneNumber" runat="server" Width="150px" CssClass="ddl" TabIndex="5"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="PhoneNumber" ErrorMessage="*"></asp:RequiredFieldValidator></td>
        <td class="editFormLabel">Alt phone number: </td>
        <td><asp:TextBox ID="AltPhoneNumber" runat="server" Width="150px" CssClass="ddl" TabIndex="6"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="editFormLabel">Fax number: </td>
        <td><asp:TextBox ID="FaxNumber" runat="server" Width="150px" CssClass="ddl" TabIndex="7"></asp:TextBox></td>
        <td class="editFormLabel">Alt fax number: </td>
        <td><asp:TextBox ID="AltFaxNumber" runat="server" Width="150px" CssClass="ddl" TabIndex="8"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="editFormLabel">Email: </td>
        <td><asp:TextBox ID="Email" runat="server" Width="150px" CssClass="ddl" TabIndex="9"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="Email" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$" ControlToValidate="Email" runat="server"  ErrorMessage="Incorrect Format" Display="Dynamic"></asp:RegularExpressionValidator></td>
        <td class="editFormLabel">Alt email: </td>
        <td><asp:TextBox ID="AltEmail" runat="server" Width="150px" CssClass="ddl" TabIndex="10"></asp:TextBox><asp:RegularExpressionValidator ID="RegularExpressionValidator2" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$" ControlToValidate="AltEmail" runat="server"  ErrorMessage="Incorrect Format" Display="Dynamic"></asp:RegularExpressionValidator></td>
    </tr>
    <tr>
        <td class="editFormLabel">Login: </td>
        <td><asp:TextBox ID="Login" runat="server" Width="150px" CssClass="ddl" TabIndex="11"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="Login" ErrorMessage="*"></asp:RequiredFieldValidator></td>
        <td class="editFormLabel">Disabled: </td>
        <td><asp:CheckBox ID="chkDisabled" runat="server" /></td>
    </tr>
    <tr>
        <td class="editFormLabel">Password: </td>
        <td><asp:TextBox ID="Password" runat="server" Width="150px" CssClass="ddl" TextMode="Password" TabIndex="12"></asp:TextBox><asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="Password" Enabled="false" ErrorMessage="*"></asp:RequiredFieldValidator></td>
        <td class="editFormLabel">Confirm password: </td>
        <td><asp:TextBox ID="CPassword" runat="server" Width="150px" CssClass="ddl" TextMode="Password" TabIndex="12"></asp:TextBox><asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" ControlToValidate="Password" ControlToCompare="CPassword"></asp:CompareValidator></td>
    </tr>
    </asp:Panel>
    <tr>
        <td align="right" colspan="2">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="publicEditFormButton" CommandName="Update" OnClick="btnSave_Click" />&nbsp;
        </td>
        <td colspan="2">
         &nbsp<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="publicEditFormButton" CommandName="Cancel" OnClick="btnCancel_Click"/>            
        </td>
    </tr>    
</table>