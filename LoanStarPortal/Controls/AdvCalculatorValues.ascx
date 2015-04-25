<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdvCalculatorValues.ascx.cs" Inherits="LoanStarPortal.Controls.AdvCalculatorValues" %>
<%@ Register Namespace="Telerik.WebControls" TagPrefix="radI" Assembly="RadInput.NET2"%>
<br />
<br />
<br />
<asp:Label ID="lblErrMesssage" runat="server" Text="" ForeColor="Red"></asp:Label>
<table border="0" cellspacing="0" cellpadding="0" style="width:600px; text-align:center;" >
    <colgroup>
        <col width="35%"/>
        <col width="7%" />
        <col width="50%" align="left"/>
        <col width="8%" align="left"/>
    </colgroup>
    <tr>
        <td class="advcalctd" style="height: 22px">Youngest Borrower Date Of Birth</td>
        <td style="height: 22px"></td>
        <td style="height: 22px">
            <radI:RadDateInput ID="diYBBirthDate" runat="server" Width="125px" Skin="Windows" MinDate="1900-01-01">
            </radI:RadDateInput>
        </td>
        <td style="height: 22px">
            <asp:RequiredFieldValidator ID="RequiredFieldValidator0" runat="server" ControlToValidate="diYBBirthDate" ErrorMessage="*"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="advcalctd" style="height: 24px">Home Value</td>
        <td style="height: 24px"></td>
        <td style="height: 24px">
            <asp:TextBox ID="tbHomeValue" runat="server" CssClass="lcCalcAdvValuesInputText"></asp:TextBox>
        </td>
        <td style="height: 24px">
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbHomeValue" ErrorMessage="*"></asp:RequiredFieldValidator>
            <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="*" Type="Currency" ControlToValidate="tbHomeValue" MinimumValue="0" MaximumValue="1000000000"></asp:RangeValidator>
        </td>
    </tr>
</table>
<br />
<div style="width:100%; text-align:left;">
<asp:Button ID="btnSave" runat="server" Text="Start Calculation" OnClick="btnSave_Click" />
</div>


