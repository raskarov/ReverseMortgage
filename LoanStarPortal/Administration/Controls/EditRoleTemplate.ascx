<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditRoleTemplate.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditRoleTemplate" %>
<%@ Register Src="MpField.ascx" TagName="MpField" TagPrefix="uc2" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<script language="javascript" type="text/javascript">
<!--
function checkSelect(o1,o2){
    var res = o1.value!=0;
    o2.innerHTML =res?'':'*';
return res;
}
--></script>
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">                        	            
<ContentTemplate>
<table width="100%" border="0" cellpadding="0" cellspacing="0"">
<tr>
    <td class="editrolelabeltd">&nbsp;</td>
    <td colspan="2" align="left">
        <asp:Label ID="lblHeader" runat="server" Text="" SkinID="AdminHeader"></asp:Label>
    </td>
</tr>
<tr style="height:20px">
    <td class="editrolelabeltd">&nbsp;</td>
    <td colspan="2" align="left">
                <asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
    </td>
</tr>
<tr>
    <td class="editrolelabeltd">
        <asp:Label ID="Label1" runat="server" Text="Role template name:" SkinID="AdminLabel"></asp:Label>
    </td>
    <td class="edituserinputtd">
        <asp:TextBox ID="tbName" runat="server" MaxLength="50" SkinID="AdminInput"></asp:TextBox>
    </td>
    <td><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbName" ErrorMessage="*"></asp:RequiredFieldValidator></td>
</tr>
<tr style="height:5px"><td colspan="3">&nbsp;</td></tr>
</table>
<div runat="server" id="divRoleInfo" style="height:540px">
<table width="100%" border="0" cellpadding="0" cellspacing="0">
<tr>
    <td colspan="3" style="padding-left:5px">
        <asp:Panel ID="pnlMpFields" runat="server" Height="525px" Width="100%">
            <uc2:MpField id="MpField1" runat="server"></uc2:MpField>
        </asp:Panel>
        </td>
</tr>
</table>
</div>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
<tr style="height:10px"><td colspan="3">&nbsp;</td></tr>
<tr>
    <td class="editrolelabeltd">&nbsp;</td>
    <td class="edituserinputtd">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="left"><asp:Button ID="btnBack" runat="server" Text="Back" SkinID="AdminButton" OnClick="btnBack_Click" CausesValidation="False"/></td>
                <td align="right" style="padding-right:3px"><asp:Button ID="btnSave" runat="server" Text="Save"  SkinID="AdminButton" OnClick="btnSave_Click"/></td>
            </tr>
        </table>        
    </td>
    <td>&nbsp;</td>
</tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>        

