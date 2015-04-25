<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GlobalAdminFields.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.GlobalAdminFields" %>

<table width="800px" border="0" cellpadding="0" cellspacing="0" align="center">
    <colgroup>
        <col width="50%" />
        <col width="49%" />
        <col width="1%" />    
    </colgroup>
    <tr>
        <td colspan="3" align="center">
            <asp:Label ID="lblHeader" runat="server" Text="Global Administration fields" SkinID="AdminHeader"></asp:Label>
        </td>
    </tr>
    <tr style="height:20px">
        <td colspan="3" align="center">
            <asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="label1" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="What is the maximum FEMA flood coverage amount?"/></td>
        <td><asp:TextBox ID="tbMaxFEMAFloodCoverage" runat="server" MaxLength="10" Width="150"></asp:TextBox></td>
        <td><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbMaxFEMAFloodCoverage" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="*" ControlToValidate="tbMaxFEMAFloodCoverage" Type="Double" MinimumValue="0" MaximumValue="10000000"></asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="label2" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="FHA Connection url:"/></td>
        <td><asp:TextBox ID="tbFHAConnectionUrl" runat="server" Width="300" MaxLength="1024"></asp:TextBox></td>
        <td>&nbsp;</td>
    </tr>    
    <tr>
        <td><asp:Label ID="label3" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="LDP url:"/></td>
        <td><asp:TextBox ID="tbLDPUrl" runat="server" Width="300" MaxLength="1024"></asp:TextBox></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td><asp:Label ID="label11" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="GSA url:"/></td>
        <td><asp:TextBox ID="tbGSAUrl" runat="server" Width="300" MaxLength="1024"></asp:TextBox></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td><asp:Label ID="label4" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="USPS url:"/></td>
        <td><asp:TextBox ID="tbUSPSUrl" runat="server" Width="300" MaxLength="1024"></asp:TextBox></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td><asp:Label ID="label5" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Default credit report vendor url:"/></td>
        <td><asp:TextBox ID="tbDefaultCreditReportVendor" runat="server" Width="300" MaxLength="1024"></asp:TextBox></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td><asp:Label ID="label6" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Default flood cert vendor url:"/></td>
        <td><asp:TextBox ID="tbDefaultFloodCertificationVendor" runat="server" Width="300" MaxLength="1024"></asp:TextBox></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td><asp:Label ID="label7" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Default appraisal vendor url:"/></td>
        <td><asp:TextBox ID="tbDefaultAppraisalVendor" runat="server" Width="300" MaxLength="1024"></asp:TextBox></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td><asp:Label ID="label8" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Default counseling agency url:"/></td>
        <td><asp:TextBox ID="tbDefaultCounselingVendor" runat="server" Width="300" MaxLength="1024"></asp:TextBox></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td><asp:Label ID="label9" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Default survey vendor url:"/></td>
        <td><asp:TextBox ID="tbDefaultSurveyVendor" runat="server" Width="300" MaxLength="1024"></asp:TextBox></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td><asp:Label ID="label10" runat="server" EnableViewState="false" SkinID="AdminLabel" Text="Default title vendor url:"/></td>
        <td><asp:TextBox ID="tbDefaultTitleVendor" runat="server" Width="300" MaxLength="1024"></asp:TextBox></td>
        <td>&nbsp;</td>
    </tr>
    <tr style="padding-top:5px;">
        <td></td>
        <td><asp:Button ID="btnSave" runat="server" Text="Save"  SkinID="AdminButton" OnClick="btnSave_Click"/></td>
    </tr>
</table>