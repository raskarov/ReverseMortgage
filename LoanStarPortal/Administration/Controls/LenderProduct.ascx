<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LenderProduct.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.LenderProduct" %>
<script language="javascript" type="text/javascript">
<!--
function SetDefaultCb(o,cbdid){
    var cb = document.getElementById(cbdid);
    if(cb!=null){
        if(o.checked){
            cb.removeAttribute('disabled');
            cb.parentNode.removeAttribute('disabled');
        }else{
            cb.setAttribute('disabled','true');
            cb.parentNode.setAttribute('disabled',!status);
            cb.checked=false;
        }
    }
}
function DefaultCbClick(o){
    if(o.checked){
        var p = o.parentNode;   
        var t = p.tagName.toLowerCase();
        while (p.tagName.toLowerCase() != 'table'){
            p=p.parentNode;
        }
        var e = p.getElementsByTagName('input');
        for (var i=0; i<e.length; i++){
            if (e[i].type=='checkbox'){
                if(e[i]!=o){
                    if(e[i].id.indexOf('_cbDefault')>0){
                        e[i].checked=false;
                    }
                }            
            }
        }    
    }
}
-->
</script>
<table width="80%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td align="center">
            <asp:Label ID="lblHeader" runat="server" Text="" SkinID="AdminHeader"></asp:Label>
        </td>    
    </tr>
    <tr>
        <td align="center">
            <asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
        </td>
    </tr>    
    <tr style="padding-top:10px">
        <td align="left" style="padding-left:250px">
            <asp:Label ID="Label1" runat="server" Text="Servicer" SkinID="AdminHeader"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="center" style="border:solid 1px black">
            <table style="width:80%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="2" align="left"><asp:Label ID="lblServicer" runat="server" Text="" SkinID="AdminMessage"></asp:Label></td>
                    </tr>            
                    <tr>
                        <td style="width:400px" align="left">Servicer</td>
                        <td align="left">Default</td>
                    </tr>
            <asp:Repeater ID="rpServicer" runat="server" OnItemDataBound="rpServicer_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td align="left">
                            <asp:CheckBox ID="cbCompany" runat="server" />    
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="cbDefault" runat="server" />    
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            </table>
        </td>
    </tr>
    <tr style="padding-top:10px">
        <td align="left" style="padding-left:250px">
            <asp:Label ID="Label2" runat="server" Text="Investor" SkinID="AdminHeader"></asp:Label>
        </td>
    </tr>    
    <tr>
        <td align="center" style="border:solid 1px black">
            <table style="width:80%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="2" align="left"><asp:Label ID="lblInvestor" runat="server" Text="" SkinID="AdminMessage"></asp:Label></td>
                    </tr>            
                    <tr>
                        <td style="width:400px" align="left">Investor</td>
                        <td align="left">Default</td>
                    </tr>
                    <asp:Repeater ID="rpInvestor" runat="server" OnItemDataBound="rpServicer_ItemDataBound">
                    <ItemTemplate>
                    <tr>
                        <td align="left">
                            <asp:CheckBox ID="cbCompany" runat="server" />    
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="cbDefault" runat="server" />    
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            </table>
        </td>
    </tr>    
    <tr style="padding-top:10px">
        <td align="left" style="padding-left:250px">
            <asp:Label ID="Label3" runat="server" Text="Trustee" SkinID="AdminHeader"></asp:Label>
        </td>
    </tr>    
    <tr>
        <td align="center" style="border:solid 1px black">
            <table style="width:80%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="2" align="left"><asp:Label ID="lblTrustee" runat="server" Text="" SkinID="AdminMessage"></asp:Label></td>
                    </tr>            
                    <tr>
                        <td style="width:400px" align="left">Trustee</td>
                        <td align="left">Default</td>
                    </tr>
                    <asp:Repeater ID="rpTrustee" runat="server" OnItemDataBound="rpTrustee_ItemDataBound">
                    <ItemTemplate>
                    <tr>
                        <td align="left">
                            <asp:CheckBox ID="cbCompany" runat="server" />    
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="cbDefault" runat="server" />    
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            </table>
        </td>
    </tr>    
    <tr style="padding-top:20px">
        <td align="center">
            <table style="width:80%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width:390px;padding-left:10px" align="left"><asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" /></td>
                    <td align="left"><asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
