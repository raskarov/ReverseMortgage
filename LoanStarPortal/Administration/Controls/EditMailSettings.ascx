<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditMailSettings.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditMailSettings" %>
<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>
<table border="0" cellspacing="0" cellpadding="0" align="center" style="width:100%">
    <tr>
        <td style="padding-left:20px;"><asp:Label ID="lblGeneralErr" runat="server" Text="" ForeColor="Red"></asp:Label></td>
    </tr>    
    <tr>
        <td>
            <table border="2" cellspacing="0" cellpadding="5" align="center" style="width: 50%">
                <tr>
                    <td colspan="2" style="text-align: center; border-bottom: black solid; height: 31px;"><strong>POP3</strong></td>
                </tr>
                <tr>
                    <td class="mailsettinglbl"><asp:Label ID="lblPOP3Server" runat="server" Text="Server :"></asp:Label></td>
                    <td class="mailsettingctl">
                        <asp:TextBox ID="tbPOP3Server" runat="server" Width="60%" ></asp:TextBox>&nbsp;<asp:Label ID="lblErrPOP3Server" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="mailsettinglbl"><asp:Label ID="lblPOP3Port" runat="server" Text="Port :"></asp:Label></td>
                    <td class="mailsettingctl">
                        <asp:TextBox ID="tbPOP3Port" runat="server" Width="15%" ></asp:TextBox>&nbsp;<asp:Label ID="lblErrPOP3Port" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center; border-bottom: black solid;">
                        <strong>SMTP<br />
                            Global Admin's Email &nbsp;- {<asp:Label ID="lblGlobalAdmin" runat="server" ForeColor="Blue"></asp:Label>}</strong></td>
                </tr>
                <tr>
                    <td class="mailsettinglbl">Server:</td>
                    <td class="mailsettingctl">
                        <asp:TextBox ID="tbSMTPServer" runat="server" Width="67%"></asp:TextBox>&nbsp;<asp:Label ID="lblErrSMTPServer" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="mailsettinglbl">Port:</td>
                    <td class="mailsettingctl">
                        <asp:TextBox ID="tbSMTPPort" runat="server" Width="15%"></asp:TextBox>&nbsp;<asp:Label ID="lblErrSMTPPort" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>            
                </tr>
                <tr>
                    <td><asp:Button ID="btnTest" runat="server" Text="Test" Width="50px" OnClick="btnTest_Click" /></td>
                    <td><asp:Label ID="lblInfo" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table align="center" style="width: 50%">
                <tr>
                    <td align="right"><asp:Button ID="btnSave" runat="server" Text="Save" Width="50px" OnClick="btnSave_Click" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>

