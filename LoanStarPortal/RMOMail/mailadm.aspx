<%@ Import Namespace="WebMailPro.MailAdm" %>
<%@ Register Src="Copyright.ascx" TagPrefix="BaseWebmail" TagName="Copyright" %>
<%@ Register Src="Logo.ascx" TagPrefix="BaseWebmail" TagName="Logo" %>
<%@ Register Src="MailAdm\mailadm_login.ascx" TagPrefix="MailAdm" TagName="Login" %>
<%@ Register Src="MailAdm\mailadm_menu.ascx" TagPrefix="MailAdm" TagName="Menu" %>

<%@ Page Language="c#" Codebehind="mailadm.aspx.cs" AutoEventWireup="True" Inherits="WebMailPro.MailAdm.mailadm"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<html>
<head>
    <title>
        <% Response.Write(settings.SiteName + " - Administration"); %>
    </title>
    <link rel="stylesheet" href="skins/<%=settings.DefaultSkin%>/styles.css" type="text/css">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script src="class.common.js" type="text/javascript"></script>

    <script src="calendar/_functions.js" type="text/javascript"></script>

    <script src="calendar/inc.calendar-settings.js" type="text/javascript"></script>

    <script src="calendar/SaveAdmSettings.js" type="text/javascript"></script>

</head>
<body>
    <form id="MailadmForm" method="post" runat="server">
        <div align="center" class="wm_content">
            <BaseWebmail:Logo ID="Control_Logo" runat="server"></BaseWebmail:Logo>
            <table width="100%" class="wm_settings">
                <% if (_screen == MailAdmScreen.Login)
        { %>
                <tr>
                    <td align="center">
                        <MailAdm:Login ID="loginID" runat="server"></MailAdm:Login>
                    </td>
                </tr>
                <% }
        else
        { %>
                <tr>
                    <td colspan="2">
                        <table class="wm_accountslist" id="accountslist">
                            <tr>
                                <td>
                                    <span class="wm_accountslist_email"><a href="default.aspx">Return to mail login form</a>
                                    </span><span class="wm_accountslist_logout"><a href="?mode=logout">Logout</a> </span>
                                    <span class="wm_accountslist_logout">&nbsp;&nbsp;&nbsp;&nbsp;<a href="help/default.htm"
                                        target="_blank">Help</a>&nbsp;&nbsp; </span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td id="MenuTD" runat="server" valign="top" align="left" class="wm_settings_nav">
                        <ul>
                            <MailAdm:Menu ID="MenuID" runat="server"></MailAdm:Menu>
                        </ul>
                    </td>
                    <td class="wm_settings_cont" runat="server" id="tdContentID" valign="top">
                        <!-- Content -->
                        <asp:PlaceHolder ID="ContentPlaceHolder" runat="server"></asp:PlaceHolder>
                    </td>
                </tr>
                <% } %>
                <tr>
                    <td align="center" colspan="2">
                        <BaseWebmail:Copyright ID="Control_Copyright" runat="server"></BaseWebmail:Copyright>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>

<script language="javascript">
		
		
	   var showMenu = GetQueryString("showmenu");
	   if(showMenu == undefined)
	   {
	       if(document.getElementById('MenuTD') != null)
	       {
              document.getElementById('MenuTD').style.display = "none"; 
           }
       }
       else
  	       if(document.getElementById('MenuTD') != null)
  	          if(showMenu == "true")
  	          {
                  document.getElementById('MenuTD').style.display = ""; 
              }
       
    function GetQueryString(stringKey) 
    {
        queryString = window.location.search.substring(1);
        allKeysAndValues = queryString.split("&");
        for (iKeyCounter=0;iKeyCounter<allKeysAndValues.length;iKeyCounter++) 
        {
            stringKeyValue = allKeysAndValues[iKeyCounter].split("=");
            if (stringKeyValue[0] == stringKey) 
            {
                return stringKeyValue[1];
            }
        }
     }
       
</script>

</html>
