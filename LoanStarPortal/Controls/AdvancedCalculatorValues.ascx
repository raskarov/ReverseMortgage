<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdvancedCalculatorValues.ascx.cs" Inherits="LoanStarPortal.Controls.AdvancedCalculatorValues" %>
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
    <tr style="display:none;">
        <td class="advcalctd" style="height: 24px">Lender</td>
        <td style="height: 24px"></td>
        <td style="height: 24px">
            <asp:DropDownList ID="ddlLenderAffiliate" DataTextField="Company" DataValueField="CompanyId" runat="server" Width="127px"></asp:DropDownList>
        </td>
        <td style="height: 24px">
            <asp:RangeValidator ID="RangeValidator4" runat="server" ErrorMessage="*" Type="Integer" ControlToValidate="ddlLenderAffiliate" MinimumValue="1" MaximumValue="1000000000"></asp:RangeValidator>
        </td>
    </tr>
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
            <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="*" Type="Double" ControlToValidate="tbHomeValue" MinimumValue="0.00000000" MaximumValue="1000000000"></asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td class="advcalctd">State</td>
        <td></td>
        <td><asp:DropDownList ID="ddlState" runat="server" DataTextField="Name" DataValueField="id" Width="127px" AutoPostBack="True" OnSelectedIndexChanged="ddlState_SelectedIndexChanged"></asp:DropDownList></td>
        <td>
            <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="*" Type="Integer" ControlToValidate="ddlState" MinimumValue="1" MaximumValue="1000000000"></asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td class="advcalctd">County</td>
        <td></td>
        <td><asp:DropDownList ID="ddlCounty" DataTextField="Name" DataValueField="ID" runat="server" Width="127px"></asp:DropDownList></td>
        <td>
            <asp:RangeValidator ID="RangeValidator3" runat="server" ErrorMessage="*" Type="Integer" ControlToValidate="ddlCounty" MinimumValue="1" MaximumValue="1000000000"></asp:RangeValidator>
        </td>
    </tr>
</table>

<br />
<div style="width:100%; text-align:left;">
<asp:Button ID="btnSave" runat="server" Text="Start Calculation" OnClick="btnSave_Click" />
</div>
