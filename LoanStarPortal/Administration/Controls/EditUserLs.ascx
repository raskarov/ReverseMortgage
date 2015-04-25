<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditUserLs.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditUserLs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<script language="javascript" type="text/javascript">
//function ValidateLogin(src, arg ){
//    arg.IsValid = arg.Value.length>=6;
//}
//function ValidatePassword(src, arg ){
//    if (document.getElementById(src.controltovalidate).getAttribute('disabled')){
//        arg.IsValid = true;
//    }else{    
//        arg.IsValid = arg.Value.length>5&&arg.Value.length<17;
//        if (arg.IsValid){
//            var cnt = arg.Value.match(/[a-z]/ig);
//            arg.IsValid = cnt.length>=5;
//            if(arg.IsValid){
//                cnt = arg.Value.match(/\d/ig);
//                arg.IsValid = (cnt!=null)&&(cnt.length>0);
//                if(arg.IsValid){                
//                    cnt = arg.Value.match(/^[a-z]\w+[a-z]$/ig);
//                    arg.IsValid=cnt!=null;
//                }
//            }
//        }
//    }
//}
//function CheckSelect(src,arg){
//    arg.IsValid=arg.Value!=0;    
//}
function SetPassword(status,c1,c2){
    if (status) {

        c1.removeAttribute('disabled');
        c2.removeAttribute('disabled');
    }
    else
    {

        c1.setAttribute('disabled', !status);
        c2.setAttribute('disabled', !status);
    }

    var c = status?'admininput':'admininputdis';
    c1.className=c;
    c2.className=c;
    if (status)
        c1.focus();
}
</script>
<table width="700px" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>&nbsp;</td>
        <td colspan="2" align="left">
            <asp:Label ID="lblHeader" runat="server" Text="" SkinID="AdminHeader"></asp:Label>
        </td>
    </tr>
    <tr style="height:20px">
        <td>&nbsp;</td>
        <td colspan="2" align="left">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">                        	            
                <ContentTemplate>    
                    <asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>        
        </td>
    </tr>
    <tr>
        <td class="edituserlabeltd">
            <asp:Label ID="Label1" runat="server" Text="Login:" SkinID="AdminLabel"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="tbLogin" runat="server" MaxLength="16"   Width="160px"></asp:TextBox>&nbsp;
            <asp:Label ID="lblLoginErr" runat="server" Text="Login must be at least 6 character long" ForeColor="red"></asp:Label>
        </td>
        <td>&nbsp;</td>
    </tr>    
    <tr runat="server" id="trPassword">
        <td class="edituserlabeltd">
            <asp:CheckBox ID="cbEnablePassword" runat="server" Text="Change password" />        
        </td>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td class="eultd">&nbsp;</td>
        <td colspan="2"><asp:Label ID="lblPasswordRules" runat="server" Text="" Font-Italic="true"></asp:Label></td>
    </tr>
    <tr>
        <td class="edituserlabeltd">
            <asp:Label ID="Label2" runat="server" Text="Password:"   Width="160px"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="tbPassword" runat="server" TextMode="Password" MaxLength="16"   Width="160px"></asp:TextBox>&nbsp;
            <asp:Label ID="lblPasswordErr" runat="server" Text="" ForeColor="red"></asp:Label>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="edituserlabeltd">
            <asp:Label ID="Label3" runat="server" Text="Confirm Password:" SkinID="AdminLabel"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="tbConfirmPassword" runat="server" TextMode="Password" MaxLength="16"   Width="160px"></asp:TextBox>
            <asp:Label ID="lblConfirmPasswordErr" runat="server" Text="" ForeColor="red"></asp:Label>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="edituserlabeltd">
            <asp:Label ID="Label6" runat="server" Text="First Name:" SkinID="AdminLabel"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="tbFirstName" runat="server"  MaxLength="20"   Width="160px"></asp:TextBox>&nbsp;
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbFirstName" ErrorMessage="*"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="edituserlabeltd">
            <asp:Label ID="Label7" runat="server" Text="Last Name:" SkinID="AdminLabel"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="tbLastName" runat="server" MaxLength="20"  Width="160px"></asp:TextBox>&nbsp;
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbLastName"  ErrorMessage="*"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr style="height:20px"><td colspan="3">&nbsp</td></tr>
    <tr>
        <td class="lefttd">&nbsp;</td>
        <td>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="left"><asp:Button ID="btnBack" runat="server" Text="Back" SkinID="AdminButton" OnClick="btnBack_Click" CausesValidation="False"/></td>
                    <td align="left" ><asp:Button ID="btnSave" runat="server" Text="Save"  SkinID="AdminButton" OnClick="btnSave_Click"/></td>
                </tr>
            </table>        
        </td>
        <td>&nbsp;</td>
    </tr>
</table>
