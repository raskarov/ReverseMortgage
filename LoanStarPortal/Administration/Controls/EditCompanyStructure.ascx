<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditCompanyStructure.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditCompanyStructure" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>
<%@ Register Assembly="RadTreeView.Net2" Namespace="Telerik.WebControls" TagPrefix="radT" %>
<script language="javascript" type="text/javascript">
<!--
function CheckDelete(node,itemText){
    if (itemText.toLowerCase()=='delete'){
        return confirm('Are you sure you want to delete this user?');
    }
    return true;
}
function ValidateLogin(src, arg ){
    arg.IsValid = arg.Value.length>=6;
}

function ValidateEmail(src, arg ){
    arg.IsValid = arg.Value.length>1;    
    if (arg.IsValid){
        var b = arg.Value.match(/^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i);
        arg.IsValid = b!=null;
    }
}
function ValidatePassword(src, arg ){
    if (document.getElementById(src.controltovalidate).getAttribute('disabled')){
        arg.IsValid = true;
    }else{    
        arg.IsValid = arg.Value.length>5&&arg.Value.length<17;
        if (arg.IsValid){
            var cnt = arg.Value.match(/[a-z]/ig);
            arg.IsValid = cnt.length>=5;
            if(arg.IsValid){
                cnt = arg.Value.match(/\d/ig);
                arg.IsValid = cnt.length>0;        
                if(arg.IsValid){
                    cnt = arg.Value.match(/^[a-z]\w+[a-z]$/ig);
                    arg.IsValid=cnt!=null;
                }
            }
        }
    }        
}
function CheckSelect(src,arg){
    arg.IsValid=arg.Value!=0;    
}
function SetPassword(status,c1,c2){
    c1.setAttribute('disabled',!status);
    c2.setAttribute('disabled',!status);
    var c = status?'admininput':'admininputdis';
    c1.className=c;
    c2.className=c;
    if (status)
        c1.focus();
}
-->
</script>
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">                        	            
<ContentTemplate>
<table width="100%" border="0" cellpadding="0" cellspacing="0" style="height:600px;vertical-align:top">
    <tr style="height:15px" valign="top">
        <td align="center" colspan="2">
            <asp:Label ID="lblHeader" runat="server" Text="" SkinID="AdminHeader"></asp:Label>
        </td>
    </tr>
    <tr style="height:20px" valign="top">
        <td align="center" colspan="2">
            <asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
        </td>
    </tr>
    <tr valign="top" style="height:565px;padding-left:50px">
        <td style="width:350px;vertical-align:top">
            <radT:RadTreeView ID="RadTreeView1" runat="server" OnNodeBound="RadTreeView1_NodeBound" Skin="Outlook" OnNodeClick="RadTreeView1_NodeClick" OnNodeContextClick="RadTreeView1_NodeContextClick" BeforeClientContextClick="CheckDelete">
            </radT:RadTreeView>        
        </td>
        <td valign="top">&nbsp;
            <div style="border:solid 1px black;padding:5px" runat="server" id="divedit">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="3" style="text-align:center">
                            <asp:Label ID="lblOp" runat="server" Text="" SkinID="AdminHeader"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Label ID="lblMessageUser" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
                        </td>
                    </tr>                    
                    <tr>
                        <td>&nbsp;</td>
                        <td colspan="2" align="left">
                            <asp:Label ID="Label1" runat="server" Text="" SkinID="AdminHeader"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height:20px">
                        <td>&nbsp;</td>
                        <td colspan="2" align="left">
                            <asp:Label ID="Label2" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="edituserlabeltd">
                            <asp:Label ID="Label3" runat="server" Text="Login:" SkinID="AdminLabel"></asp:Label>
                        </td>
                        <td class="edituserinputtd">
                            <asp:TextBox ID="tbLogin" runat="server" MaxLength="256" SkinID="AdminInput"></asp:TextBox>
                        </td>
                        <td>
                            <asp:CustomValidator ID="CustomValidator3" runat="server" ClientValidationFunction="ValidateLogin" ControlToValidate="tbLogin" ErrorMessage="Login must be at least 6 charcter long" ValidateEmptyText="True"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr runat="server" id="trPassword">
                        <td class="edituserlabeltd">
                            <asp:CheckBox ID="cbEnablePassword" runat="server" Text="Change password" />        
                        </td>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="edituserlabeltd">
                            <asp:Label ID="Label4" runat="server" Text="Password:" SkinID="AdminLabel"></asp:Label>
                        </td>
                        <td class="edituserinputtd">
                            <asp:TextBox ID="tbPassword" runat="server" TextMode="Password" MaxLength="16" SkinID="AdminInput" CssClass="admininput"></asp:TextBox>
                        </td>
                        <td>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidatePassword" ControlToValidate="tbPassword" ErrorMessage="Wrong password" ValidateEmptyText="True"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="edituserlabeltd">
                            <asp:Label ID="Label5" runat="server" Text="Confirm Password:" SkinID="AdminLabel"></asp:Label>
                        </td>
                        <td class="edituserinputtd">
                            <asp:TextBox ID="tbConfirmPassword" runat="server" TextMode="Password" MaxLength="16" CssClass="admininput"></asp:TextBox>
                        </td>
                        <td>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="tbPassword" ControlToValidate="tbConfirmPassword" ErrorMessage="CompareValidator">Passwords not identical</asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="edituserlabeltd">
                            <asp:Label ID="Label6" runat="server" Text="First Name:" SkinID="AdminLabel"></asp:Label>
                        </td>
                        <td class="edituserinputtd">
                            <asp:TextBox ID="tbFirstName" runat="server"  MaxLength="20" SkinID="AdminInput"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbFirstName" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="edituserlabeltd">
                            <asp:Label ID="Label7" runat="server" Text="Last Name:" SkinID="AdminLabel"></asp:Label>
                        </td>
                        <td class="edituserinputtd">
                            <asp:TextBox ID="tbLastName" runat="server" MaxLength="20" SkinID="AdminInput"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbLastName"  ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="edituserlabeltd">
                            <asp:Label ID="Label8" runat="server" Text="User Roles:" SkinID="AdminLabel"></asp:Label>
                        </td>
                        <td colspan="3">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="3" style="padding:5px">
                            <div style="border:solid 1px  black">
                                <asp:DataList ID="DataList1" runat="server" RepeatColumns="5" RepeatDirection="Horizontal" EnableViewState="true" DataKeyField="id">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Checkbox1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>' Checked='<%# int.Parse(DataBinder.Eval(Container.DataItem,"Selected").ToString())==1?true:false %>' />
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                        </td>
                    </tr>
                    <tr style="height:20px"><td colspan="3">&nbsp</td></tr>
                    <tr>
                        <td colspan="3" style="padding-left:5px;">
                            <asp:Label ID="lblLogonInfo" runat="server" Text="Mail Logon Information" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="padding:5px 5px 5px 5px;">
                            <div style="border:solid 1px  black">
                                <table border="0" cellpadding="0" cellspacing="0" width="99%">
                                    <tr>
                                        <td class="edituserlabeltd">
                                            <asp:Label ID="lblUserMail" runat="server" Text="Email:" SkinID="AdminLabel"></asp:Label>
                                        </td>                
                                        <td style="width:215px;text-align:right;">
                                            <asp:TextBox ID="tbUserMail" runat="server" MaxLength="255" Width="80%"></asp:TextBox>
                                        </td>                
                                        <td style="padding-left:30px;">                                            
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$" ControlToValidate="tbUserMail" runat="server"  ErrorMessage="Incorrect Format"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="edituserlabeltd">
                                            <asp:Label ID="lblUserName" runat="server" Text="User Name:" SkinID="AdminLabel"></asp:Label>
                                        </td>
                                        <td style="width:215px;text-align:right;">
                                            <asp:TextBox ID="tbUserName" runat="server" MaxLength="16" Width="80%"></asp:TextBox>
                                        </td>
                                        <td style="padding-left:30px;">&nbsp;</td>
                                    </tr>            
                                    <tr runat="server" id="trMailPassword">
                                        <td class="edituserlabeltd">
                                            <asp:CheckBox ID="cbEnableMailPassword" runat="server" Text="Change password" />        
                                        </td>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>            
                                    <tr>
                                        <td class="edituserlabeltd">
                                            <asp:Label ID="lblMailPassword" runat="server" Text="Password:" SkinID="AdminLabel"></asp:Label>
                                        </td>
                                        <td style="width:215px;text-align:right;">
                                            <asp:TextBox ID="tbMailPassword" runat="server" MaxLength="16" Width="80%" TextMode="Password"></asp:TextBox>
                                        </td >
                                        <td style="padding-left:30px;">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="edituserlabeltd">
                                            <asp:Label ID="Label9" runat="server" Text="Confirm Password:" SkinID="AdminLabel"></asp:Label>
                                        </td>
                                        <td style="width:215px;text-align:right;">
                                            <asp:TextBox ID="tbMailPasswordConfirm" runat="server" TextMode="Password" MaxLength="16" Width="80%"  CssClass="admininput"></asp:TextBox>
                                        </td >
                                        <td style="padding-left:30px;">
                                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="tbMailPassword" ControlToValidate="tbMailPasswordConfirm" ErrorMessage="">Passwords not identical</asp:CompareValidator>
                                        </td>
                                    </tr>            
                                </table>
                            </div>
                        </td>
                    </tr>                                
                    <tr>
                        <td class="lefttd">&nbsp;</td>
                        <td class="edituserinputtd">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left"><asp:Button ID="btnBack" runat="server" Text="Cancel" SkinID="AdminButton" OnClick="btnBack_Click" CausesValidation="False"/></td>
                                    <td align="right" style="padding-right:3px"><asp:Button ID="btnSave" runat="server" Text="Save"  SkinID="AdminButton" OnClick="btnSave_Click"/></td>
                                </tr>
                            </table>        
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                </table>                
            </div>
        </td>
    </tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>        

