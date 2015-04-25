<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditRole.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditRole" %>
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
    <td align="center">
        <asp:Label ID="lblHeader" runat="server" Text="" SkinID="AdminHeader"></asp:Label>
    </td>
</tr>
<tr style="height:20px">
    <td align="center">
        <asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
    </td>
</tr>
</table>
<div runat="server" id="divRoleInfo" style="height:540px">
<table width="100%" border="0" cellpadding="0" cellspacing="0">
<tr>
    <td colspan="3" style="padding-left:2px">
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

