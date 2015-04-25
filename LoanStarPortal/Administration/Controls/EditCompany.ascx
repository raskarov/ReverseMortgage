<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditCompany.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditCompany" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="RadUpload.Net2" Namespace="Telerik.WebControls" TagPrefix="radU" %>
<script language="javascript" type="text/javascript">
<!--
function btnSaveClient_Click()
{
    var cbLender = document.getElementById('<%=cbLender.ClientID %>');
    var cbProductList = document.getElementById('<%=trProductList.ClientID %>').getElementsByTagName('input');
    var lblMessage = document.getElementById('<%=lblMessage.ClientID %>');    
    var i;
    var needProduct = cbLender.checked;
    if (needProduct){
        for (i = 0; i < cbProductList.length; i++)
            if (cbProductList[i].checked){
                needProduct = false;
                break;
            }
    }            
            
    lblMessage.innerHTML = needProduct ? 'You need to select at least one product for lender' : '';
    var tb = document.getElementById('<%=tbCompany.ClientID %>');
    var lbl = document.getElementById('<%=lblCompanyErr.ClientID %>');
    if (tb.value=='') {

        lbl.innerText = '*';
        return false;
    }else {lbl.innerText = '';}
    return !needProduct;
}
function ShowAffiliateTr(id1,id2,trid){
    var o1 = document.getElementById(id1);
    var o2 = document.getElementById(id2);
    var t = document.getElementById(trid);
    if(t!=null){
        t.style.display = (o1.checked||o2.checked)?'block':'none';
    }
}
function ShowProductTr(o,trid){
    var t = document.getElementById(trid);
    if(t!=null){
        t.style.display = o.checked?'block':'none';
    }
}

function SetEditLink(o,linkid){
    var l = document.getElementById(linkid);
    if(o.checked){
        l.removeAttribute("disabled");
    }else{
        l.setAttribute("disabled","true");
    }
}
-->
</script>
<table width="800px" border="0" cellpadding="0" cellspacing="0">
<tr style="height:1px;">
    <td style="width:300px;">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td colspan="3" align="center">
        <asp:Label ID="lblHeader" runat="server" Text="" SkinID="AdminHeader"></asp:Label>
    </td>
</tr>
<tr style="height:20px">
    <td colspan="3" align="center">
        <asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
    </td>
</tr>
<tr>
    <td align="right" style="padding-right:3px;">Company:</td>
    <td style="width:240px;">
        <asp:TextBox ID="tbCompany" runat="server" MaxLength="100" Width="210px"></asp:TextBox>
        <asp:Label ID="lblCompanyErr" runat="server" Text="" ForeColor="red" Width="8px"></asp:Label>
    </td>
    <td>&nbsp;</td>
</tr>
<tr>
    <td style="width:300px">&nbsp;</td>
    <td colspan="2" align="center">
        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
            <tr>
                <td style="width:19%" align="center">
                    <asp:CheckBox ID="cbOriginator" runat="server" Text="Originator" />
                </td>
                <td style="width:19%" align="center">
                    <asp:CheckBox ID="cbLender" runat="server" Text="Lender" />
                </td>
                <td style="width:19%" align="center">
                    <asp:CheckBox ID="cbServicer" runat="server" Text="Servicer" />
                </td>
                <td style="width:19%" align="center">
                    <asp:CheckBox ID="cbInvestor" runat="server" Text="Investor" />
                </td>
                <td style="width:19%" align="center">
                    <asp:CheckBox ID="cbTrustee" runat="server" Text="Trustee" />
                </td>                
            </tr>
        </table>
    </td>
</tr>
<tr runat="server" id="trProductList">
    <td style="width:300px">&nbsp;</td>
    <td colspan="2" align="center" style="padding-top:10px;padding-bottom:10px">
        <table border="0" cellpadding="0" cellspacing="0" style="width:100%" >
            <tr>
                <td style="border:solid 1px black;width:100%;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width:100%" >
                    <asp:Repeater ID="rpLenderProduct" runat="server" OnItemDataBound="rpLenderProduct_ItemDataBound">
                    <ItemTemplate>        
                        <tr>
                            <td align="left">
                                <asp:CheckBox runat="server" ID="cbProduct" ></asp:CheckBox>
                            </td>
                            <td>
                                <asp:HyperLink ID="hlEdit" runat="server">Edit</asp:HyperLink>
                            </td>
                        </tr>
                    </ItemTemplate>
                    </asp:Repeater>            
                    </table>                
                </td>
            </tr>
        </table>
    </td>
</tr>
<tr runat="server" id="trManageAffiliate" style="height:40px;padding-top:5px;padding-bottom:5px">
    <td align="right"  style="width:300px">
        <asp:HyperLink ID="hlManageAffiliates" runat="server">Manage Affiliates</asp:HyperLink>
    </td>
    <td colspan="2">&nbsp;</td>
</tr>
<tr runat="server" id="trUploadLogo">
    <td style="padding-right:3px;width:300px" align="right">
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
    </td>
    <td colspan="2">
        <asp:FileUpload ID="UploadLogoImage" runat="server" Width="288px" />&nbsp;&nbsp(Image size 200x25)
    </td>
</tr>
<tr runat="server" id="trlogo">
    <td style="padding-right:3px;width:300px" align="right">
        <asp:Label ID="Label3" runat="server" Text="Current Logo:"></asp:Label>
    </td>
    <td colspan="2">
        <asp:Image ID="imgLogo" runat="server" Width="200px" Height="25px"/>&nbsp;&nbsp<asp:Button ID="btnRemove" runat="server" Text="Remove" OnClick="btnRemove_Click" CausesValidation="False" Width="80px"/>
    </td>
</tr>
<tr runat="server" id="trRoleBasedSecurity">
    <td style="padding-right:3px;width:300px" align="right">
        <asp:Label ID="Label4" runat="server" Text="Use global role-based security settings"></asp:Label>
    </td>
    <td>
        <asp:CheckBox ID="cbRoleSettings" runat="server" />
    </td>
    <td>&nbsp;</td>
</tr>
<tr runat="server" id="trRequiredFields">
    <td style="padding-right:3px;width:300px" align="right">
        <asp:Label ID="Label6" runat="server" Text="Use global required fields settings"></asp:Label>
    </td>
    <td>
        <asp:CheckBox ID="cbRequiredFields" runat="server" />
    </td>
    <td>&nbsp;</td>
</tr>
<tr runat="server" id="trOriginator">
    <td style="padding-right:3px;width:300px" align="right">
        <asp:Label ID="Label15" runat="server" Text="Turn on pre-underwriting questioning sequences <br/>for all documents where available?"></asp:Label>
    </td>
    <td>
        <asp:CheckBox ID="cbPreUnderwritingQuestions" runat="server" ></asp:CheckBox>
    </td>
</tr>
<tr runat="server" id="trLeadManagement">
    <td style="padding-right:3px;width:300px" align="right">
        <asp:Label ID="Label1" runat="server" Text="Turn on Lead management"></asp:Label>
    </td>
    <td>
        <asp:CheckBox ID="cbIsLeadManagementEnabled" runat="server" ></asp:CheckBox>
    </td>
</tr>
<tr id="trRetail" runat="server">
    <td align="right" style="padding-right:3px;width:300px">
        <asp:Label ID="Label5" runat="server" Text="Enable retail tools for loan officers(Y/N)?"></asp:Label>
    </td>
    <td>
        <asp:CheckBox ID="cbRetailTools" runat="server" />
    </td>
    <td>&nbsp;</td>
</tr>
<tr style="padding-top:10px">
    <td style="width:300px">&nbsp;</td>
    <td>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="left"><asp:Button ID="btnBack" runat="server" Text="Back" SkinID="AdminButton" OnClick="btnBack_Click" CausesValidation="False"/></td>
                <td align="right" style="padding-right:3px"><asp:Button ID="btnSave" runat="server" Text="Save"  SkinID="AdminButton" OnClientClick="return btnSaveClient_Click();" OnClick="btnSave_Click" /></td>
            </tr>
        </table>        
    </td>
    <td>&nbsp;</td>
</tr>
</table>

