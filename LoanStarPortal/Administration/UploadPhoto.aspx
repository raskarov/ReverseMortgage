<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadPhoto.aspx.cs" Inherits="LoanStarPortal.Administration.UploadPhoto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Manage Photo</title>
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">   		
            function GetRadWindow(){
                var o=null;
                if (window.radWindow) o = window.radWindow; 
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }
            function EndEdit(){
                GetRadWindow().Close();		
            }
        </script>
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr id="trPhotoImage" runat="server">
            <td style="width:120px;padding-right:5px;" align="right">
                <asp:Label ID="lblCurrentPhoto" runat="server" Text="Current Photo:"></asp:Label>
            </td> 
            <td style="width:300px">
                <asp:Image ID="imgPhoto" runat="server" BorderWidth="0" BorderStyle="None" ImageAlign="Middle"/>
            </td> 
            <td>&nbsp;</td>          
        </tr>
        <tr style="padding-top:10px;">
            <td align="right" style="width: 120px; padding-right: 5px;">
                <asp:Label ID="lblUploadPhoto" runat="server" Text="Upload"></asp:Label>
            </td>
            <td style="width: 300px">
                <asp:FileUpload ID="UserPhoto" runat="server" Width="290px"/>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td align="right" style="width: 120px; padding-right: 5px;">
                <asp:Label ID="Label9" runat="server" Text="Display this photo?"></asp:Label>
            </td>
            <td style="width: 300px">
                <asp:CheckBox ID="cbDisplayPhoto" runat="server" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr style="padding-top:5px;">
            <asp:Label ID="InjectScript" runat="server"></asp:Label>
            <td style="width: 120px;"></td>
            <td>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>    
                        <td align="left">
                            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" Width="80px" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" Width="80px" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>&nbsp;</td>
        </tr>
    </table>
    </form>
</body>
</html>
