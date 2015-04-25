<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorDashBoard.aspx.cs" Inherits="LoanStarPortal.VendorDashBoard"  Theme="Theme" StylesheetTheme="Theme" %>
<%@ Register Assembly="RadMenu.Net2" Namespace="Telerik.WebControls" TagPrefix="radM" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Vendor Web Dashboard</title>
    <link type="text/css" rel="stylesheet" href="App_Themes/Theme/default.css" />
</head>
<body>
    <form id="form1" runat="server">
    <radM:radmenu id="rmVendor" runat="server" Skin="Web20" style="position:absolute;top:96px;left:10%" CausesValidation="false" OnItemClick="rmVendor_ItemClick" Height="26px" >
        <Items>
            <radm:RadMenuItem Text="Options" AccessKey="o" Value="Options">
                <Items>
                    <radM:RadMenuItem AccessKey="e" Text="Edit my profile" Value="EditProfile"></radm:RadMenuItem>
                    <radm:RadMenuItem Text="Change password" AccessKey="y" Value="ChangePassword"></radm:RadMenuItem>
                </Items>
            </radm:RadMenuItem>
        </Items>
    </radM:radmenu>
    <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
        <tr>
            <td align="center"><asp:Image ID="Image1"  ImageUrl="~/Images/logo.gif" AlternateText="" width="350px" height="52px" runat="server" /></td>
        </tr>
        <tr>
            <td align="center"><asp:Label ID="Label1" runat="server" Text="Vendor Web Dashboard" Font-Size="X-Large" Font-Bold="true" ForeColor="Blue"></asp:Label></td>
        </tr>
        <tr><td align="right" style="padding-right:20px;"><asp:LinkButton ID="lbLogout" runat="server" OnClick="lbLogout_Click" CausesValidation="False">Logout</asp:LinkButton></td></tr>
        <tr><td align="center"><asp:Label ID="lblName" runat="server" SkinID="VendorHeader"></asp:Label></td></tr>
        <tr><td align="center"><asp:Label ID="lblMessage" runat="server" SkinID="VendorMessage"></asp:Label></td></tr>        
        <tr>
            <td align="center">
                <asp:PlaceHolder ID="phVendorControl" runat="server"></asp:PlaceHolder>
            </td>
        </tr>
        <tr runat="server" id="trSave">
            <td align="center">
                <table border="0" cellpadding="0" cellspacing="0" style="width:80%;">
                    <tr style="height:25px;padding-top:10px">
                        <td class="tdvlbl">&nbsp;</td>
                        <td class="tdvctl">
                            <table border="0" cellpadding="0" cellspacing="0" style="width:100%;">
                                <tr>
                                    <td style="width:50%" align="left"><asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>
                                    <td style="width:50%" align="right"><asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" /></td>
                                </tr>
                            </table>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                </table>        
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
