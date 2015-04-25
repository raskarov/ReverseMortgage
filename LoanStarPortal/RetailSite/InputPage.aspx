<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InputPage.aspx.cs" Inherits="LoanStarPortal.RetailSite.InputPage" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Reverse Mortgage Calculator</title>
    <link href="input.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
    function CheckInputOnSave(id1,id2){
        var r=true;
        var r1=CheckValue(id1,'');
        r = r&&r1;
        r1=CheckValue(id2,'');
        r = r&&r1;
        return r;
    }
    function CheckInputOnCalculate(id1,id2,id3,id4,id5,id6,id7){
        var r=true;
        var r1=CheckValue(id1,'');
        r=r&&r1;
        r1=CheckValue(id2,'');
        r=r&&r1;
        r1=CheckValue(id3,'0');        
        r=r&&r1;
        r1=CheckValue(id4,'0');
        r=r&&r1;
        r1=CheckValue(id5,'');
        r=r&&r1;
        r1=CheckValue(id6,'');
        r=r&&r1;
        r1=CheckValue(id7,'');
        return r;
    }
    function CheckValue(id,val){
        var r=false;
        var o=document.getElementById(id);
        if(o!=null){
            r=o.value!=val;
        }
        o=document.getElementById(id+'_err');        
        if(!r){
            o.style.display='block';
        }else{
            o.style.display='none';
        }
        return r;
    }
    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="height:30px;">
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td align="center" class="header">Reverse Mortgage Calculator</td>
        </tr>
        <tr style="padding-top:10px;">
            <td align="center">
                <table border="0" cellpadding="0" cellspacing="0" width="50%">
                    <tr>
                        <td class="label">
                            <asp:Label ID="Label1" runat="server" Text="Value"></asp:Label>
                        </td>
                        <td class="control">
                            <radi:radnumerictextbox id="tbValue" runat="server" maxvalue="10000000" minvalue="0" numberformat-decimaldigits="0" type="Number" width="100px" MaxLength="10" BackColor="yellow"/>
                        </td>
                        <td align="left">
                            <span id="tbValue_err" runat="server" style="color: Red; display: none">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="Label2" runat="server" Text="Address"></asp:Label>
                        </td>
                        <td class="control">
                            <asp:TextBox ID="tbAddress1" runat="server" MaxLength="256" Width="200px" BackColor="yellow"></asp:TextBox>
                         </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                        </td>
                        <td class="control">
                            <asp:TextBox ID="tbAddress2" runat="server" BackColor="yellow" MaxLength="256" Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="Label4" runat="server" Text="City"></asp:Label>
                        </td>
                        <td class="control">
                            <asp:TextBox ID="tbCity" runat="server" BackColor="yellow" MaxLength="50" Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="Label5" runat="server" Text="State"></asp:Label>
                        </td>
                        <td class="control">
                            <asp:DropDownList ID="ddlState" runat="server" BackColor="yellow" Width="206px" AutoPostBack="True" OnSelectedIndexChanged="ddlState_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td align="left">
                            <span id="ddlState_err" runat="server" style="color: Red; display: none">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="Label6" runat="server" Text="Zip"></asp:Label>
                        </td>
                        <td class="control">
                            <radI:RadMaskedTextBox ID="tbZip" runat="server" BackColor="yellow" DisplayFormatPosition="Right"
                                DisplayMask="#####" DisplayPromptChar=" " Mask="#####" Width="120px">
                            </radI:RadMaskedTextBox>
                        </td>
                        <td align="left">
                            <span id="tbZip_err" runat="server" style="color: Red; display: none">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="Label7" runat="server" Text="County"></asp:Label>
                        </td>
                        <td class="control">
                            <asp:DropDownList ID="ddlCounty" runat="server" BackColor="yellow" Width="206px"></asp:DropDownList>
                        </td>
                        <td align="left">
                            <span id="ddlCounty_err" runat="server" style="color: Red; display: none">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="Label8" runat="server" Text="Date of birth"></asp:Label>
                        </td>
                        <td class="control">
                            <radI:RadDateInput ID="rdidob1" runat="server" BackColor="yellow" Width="100px" DateFormat="MM/dd/yyyy" DisplayDateFormat="MM/dd/yyyy" MinDate="1700-01-01"></radI:RadDateInput>                            
                        </td>
                        <td align="left">
                            <span id="rdidob1_err" runat="server" style="color: Red; display: none">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="Label9" runat="server" Text="Date of birth"></asp:Label>
                        </td>
                        <td class="control">
                            <radI:RadDateInput ID="rdidob2" runat="server" Width="100px" DateFormat="MM/dd/yyyy" MinDate="1700-01-01"></radI:RadDateInput>
                        </td>
                        <td align="left">
                            <span id="rdidob2_err" runat="server" style="color: Red; display: none">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="Label10" runat="server" Text="Liens"></asp:Label>
                        </td>
                        <td class="control">
                            <asp:TextBox ID="tbLiens" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td align="left">
                            <span id="tbLiens_err" runat="server" style="color: Red; display: none">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="Label11" runat="server" Text="First Name"></asp:Label>
                        </td>
                        <td class="control">
                            <asp:TextBox ID="tbFirstName" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                        </td>
                        <td align="left">
                            <span id="tbFirstName_err" runat="server" style="color: Red; display: none">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="Label12" runat="server" Text="Last Name"></asp:Label>
                        </td>
                        <td class="control">
                            <asp:TextBox ID="tbLastName" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                        </td>
                        <td align="left">
                            <span id="tbLastName_err" runat="server" style="color: Red; display: none">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="Label13" runat="server" Text="Phone"></asp:Label>
                        </td>
                        <td class="control">
                            <radI:RadMaskedTextBox ID="tbPhone" runat="server" DisplayFormatPosition="Right"
                                DisplayMask="(###) ###-####" DisplayPromptChar=" " Mask="(###) ###-####" Width="100px">
                            </radI:RadMaskedTextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr style="padding-top:10px;">
                        <td colspan="3">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="width:240px;">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="90px"/>
                                    </td>                                       
                                    <td align="left">
                                         <asp:Button ID="btnCalculate" runat="server" OnClick="btnCalculate_Click" Text="Calculate" Width="90px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
