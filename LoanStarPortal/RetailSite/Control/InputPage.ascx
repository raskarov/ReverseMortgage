<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InputPage.ascx.cs" Inherits="LoanStarPortal.RetailSite.Control.InputPage" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<script type="text/javascript">
function RestoreFocus(){
//var o = document.getElementById('<%=tbZip.ClientID %>'+'_text');
var o = document.getElementById('<%=ddlCounty.ClientID %>');
o.focus();
}
function SetFocusAfterRequest(){
window.setTimeout('RestoreFocus()',500);
//var o = document.getElementById('<%=tbZip.ClientID %>'+'_text');
//o.focus();
//alert('!!!');
}
</script>
<div id="middle">
<div class="indent">
<table border="0" cellpadding="0" cellspacing="0" style="width:100%;background-color:White">
    <tr style="padding-top:10px;height:25px">
        <td align="center" class="header">Reverse Mortgage Calculator</td>
    </tr>
    <tr>
        <td align="center"><asp:Label ID="lblMessage" runat="server" Text="" ForeColor="red"></asp:Label></td>
    </tr>
    <tr style="padding-top: 3px;">
        <td align="center">
            <table border="0" cellpadding="0" cellspacing="0" width="50%">
                <tr>
                    <td class="label">
                        <asp:Label ID="Label1" runat="server" Text="Value"></asp:Label>
                    </td>
                    <td class="control">
                        <radi:radnumerictextbox id="tbValue" runat="server" backcolor="yellow" maxlength="10"
                            maxvalue="10000000" minvalue="0" numberformat-decimaldigits="0" type="Number"
                            width="100px" Height="16px" Font-Size="100%">
                        </radi:radnumerictextbox>
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
                        <asp:TextBox ID="tbAddress1" runat="server" Font-Size="100%" Height="15px" MaxLength="256"
                            Width="200px"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="control">
                        <asp:TextBox ID="tbAddress2" runat="server" Font-Size="100%" Height="15px" MaxLength="256"
                            Width="200px"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="Label4" runat="server" Text="City"></asp:Label>
                    </td>
                    <td class="control">
                        <asp:TextBox ID="tbCity" runat="server" Font-Size="100%" Height="15px" MaxLength="50"
                            Width="200px"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="Label5" runat="server" Text="State"></asp:Label>
                    </td>
                    <td class="control">
                        <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" BackColor="yellow"
                            OnSelectedIndexChanged="ddlState_SelectedIndexChanged" Width="206px">
                        </asp:DropDownList>
                    </td>
                    <td align="left">
                        <span id="ddlState_err" runat="server" style="color: Red; display: none">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="Label7" runat="server" Text="County"></asp:Label>
                    </td>
                    <td class="control">
                        <asp:DropDownList ID="ddlCounty" runat="server" BackColor="yellow" Width="206px">
                        </asp:DropDownList>
                    </td>
                    <td align="left">
                        <span id="ddlCounty_err" runat="server" style="color: Red; display: none">*</span>
                    </td>
                </tr>                
                <tr>
                    <td class="label">
                        <asp:Label ID="Label6" runat="server" Text="Zip"></asp:Label>
                    </td>
                    <td class="control">
                        <radI:RadMaskedTextBox ID="tbZip" runat="server" DisplayFormatPosition="Right" DisplayMask="#####"
                            DisplayPromptChar=" " Font-Size="100%" Height="16px" Mask="#####" Width="120px">
                       </radI:RadMaskedTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="Label8" runat="server" Text="Date of birth"></asp:Label>
                    </td>
                    <td class="control">
                        <radI:RadDateInput ID="rdidob1" runat="server" BackColor="yellow" DateFormat="MM/dd/yyyy"
                            DisplayDateFormat="MM/dd/yyyy" Font-Size="100%" Height="16px" MinDate="1700-01-01"
                            Width="100px"></radi:raddateinput>
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
                        <radI:RadDateInput ID="rdidob2" runat="server" DateFormat="MM/dd/yyyy" Font-Size="100%"
                            Height="16px" MinDate="1700-01-01" Width="100px"></radi:raddateinput>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="Label10" runat="server" Text="Liens"></asp:Label>
                    </td>
                    <td class="control">
                        <radI:RadNumericTextBox ID="tbLiens" runat="server" Font-Size="100%" Height="16px"
                            MaxLength="10" MaxValue="10000000" MinValue="0" NumberFormat-DecimalDigits="0"
                            Type="Number" Width="100px">
                        </radI:RadNumericTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="Label11" runat="server" Text="First Name"></asp:Label>
                    </td>
                    <td class="control">
                        <asp:TextBox ID="tbFirstName" runat="server" Font-Size="100%" Height="15px" MaxLength="100"
                            Width="200px"></asp:TextBox>
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
                        <asp:TextBox ID="tbLastName" runat="server" Font-Size="100%" Height="15px" MaxLength="100"
                            Width="200px"></asp:TextBox>
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
                            DisplayMask="(###) ###-####" DisplayPromptChar=" " Font-Size="100%" Height="16px"
                            Mask="(###) ###-####" Width="100px">
                        </radi:RadMaskedTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr><td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="width: 240px;">
                                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" Width="90px" Height="24px" Font-Bold="true" Font-Size="12px" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnCalculate" runat="server" Width="90px" Height="24px" Font-Bold="true" Font-Size="12px" OnClick="btnCalculate_Click"
                                        Text="Calculate"  />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</div>
</div>