<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LoanStarPortal.Administration.Default"  EnableTheming="true" Theme="Default" %>
<%@ Register Src="Controls/LoggedAs.ascx" TagName="LoggedAs" TagPrefix="uc2" %>
<%@ Register Assembly="RadToolbar.Net2" Namespace="Telerik.WebControls" TagPrefix="radTlb" %>
<%@ Register Src="Controls/AdminMenu.ascx" TagName="AdminMenu" TagPrefix="uc1" %>
<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>
<%@ Register Assembly="RadWindow.Net2" Namespace="Telerik.WebControls" TagPrefix="radW" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
     <meta http-equiv="X-UA-Compatible" content="IE=10"/>
    <title>RM LOS</title>
    <link href="../RadControls/Combobox/Skins/WebBlue/Styles.css" rel="stylesheet" type="text/css"/>
    <link href="../RadControls/Tabstrip/Skins/ClassicBlue/styles.css" rel="stylesheet" type="text/css" />
    <link href="../RadControls/Grid/Skins/WebBlue/Styles.css" rel="stylesheet" type="text/css" />
    <link href="../RadControls/Calendar/Skins/WebBlue/Calendar.css" rel="stylesheet" type="text/css" />   

</head>
<body>
         
    <form id="form1" runat="server">
             <rada:AjaxLoadingPanel ID="LoadingPanel1" runat="server" Transparency="30" BackColor="#E0E0E0">
</rada:AjaxLoadingPanel>
          <asp:ScriptManager ID="ScriptManager2" runat="server" EnableScriptLocalization="True">
   </asp:ScriptManager>
        <radw:radwindowmanager id="WindowManager" runat="server" height="240px" skin="WebBlue" width="460px" EnableStandardPopups="False" Modal="True" BorderStyle="None" >
            <Windows>
                <radW:RadWindow ID="UploadPhoto" runat="server" Behavior="None" Height="240px" Left=""
                    NavigateUrl="UploadPhoto.aspx" Skin="WebBlue" SkinsPath="~/RadControls/Window/Skins"
                    Title="Manage Photo" Top="" Width="460px" />
            </Windows>
        </radw:radwindowmanager>
        <table border="0" style="width:98%" cellpadding="0" cellspacing="0">
        <tr style="height:20px"><td>&nbsp;</td></tr>
        <tr>
            <td>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                    <td style="width:300px">&nbsp;</td>
                    <td align="center"></td>
                    <td style="width:300px" align="right"><uc2:LoggedAs ID="LoggedAs1" runat="server" /></td>
                    </tr>
                </table>                
            </td>
        </tr>
        <tr style="height:20px"><td>&nbsp;</td></tr>
        <tr style="vertical-align:top;height:700px">    
            <td>
                <table border="0" cellpadding="0" cellspacing="0" style="width:100%;height:100%">
                    <tr>
                        <td style="width:122px;padding-left:10px" valign="top">
                            <uc1:AdminMenu ID="AdminMenu1" runat="server" />
                        </td>
                        <td valign="top" style="padding-left:3px">
                            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                        </td>
                    </tr>    
                </table>        
            </td>
        </tr>
<!--        <tr style="height:17px; background-color:#E9EEEE"><td align="center">Footer</td></tr>    -->
    </table>
    </form>
</body>
</html>
