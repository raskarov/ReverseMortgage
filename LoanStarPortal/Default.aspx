<%@ Page Language="C#" AutoEventWireup="true" Theme="Theme" StylesheetTheme="Theme" CodeBehind="Default.aspx.cs" Inherits="LoanStarPortal.Default" EnableViewStateMac="false" %>
<%@ Register Assembly="RadWindow.Net2" Namespace="Telerik.WebControls" TagPrefix="radW" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>
<%@ Register Src="~/Controls/ApplicantList.ascx" TagName="ApplicantList" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>
<%@ Register Assembly="RadMenu.Net2" Namespace="Telerik.WebControls" TagPrefix="radM" %>
<%@ Import Namespace="System.Threading" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"  style="height:100%">
<head id="Head1" runat="server">  
    <meta http-equiv="X-UA-Compatible" content="IE=8"/>
    <title>RM LOS</title>  
<link type="text/css" rel="stylesheet" href="App_Themes/Theme/default.css" />
<link href="RadControls/Calendar/Skins/Web20/Calendar.css" rel="stylesheet" type="text/css" />
<link href="RadControls/Calendar/Skins/WebBlue/Calendar.css" rel="stylesheet" type="text/css" />
<link href="RadControls/Combobox/Skins/WindowsXP/Styles.css" rel="stylesheet" type="text/css" />
<link href="RadControls/Grid/Skins/Default/Styles.css" rel="stylesheet" type="text/css" />
<link href="RadControls/Grid/Skins/Windows/Styles.css" rel="stylesheet" type="text/css" />
<link href="RadControls/Grid/Skins/WebBlue/Styles.css" rel="stylesheet" type="text/css" />
<link href="RadControls/Input/Skins/Windows/styles.css" rel="stylesheet" type="text/css" />
<link href="RadControls/PanelBar/Skins/WebBlue/styles.css" rel="stylesheet" type="text/css" />
<link href="RadControls/PanelBar/Skins/PipeLine/styles.css" rel="stylesheet" type="text/css" />
<link href="RadControls/Panelbar/Skins/Outlook/styles.css" rel="stylesheet" type="text/css" />
<link href="RadControls/Splitter/Skins/Default/Styles.css" rel="stylesheet" type="text/css" />
<link href="RadControls/Tabstrip/Scripts/3_6_1/tabstrip.css" rel="stylesheet" type="text/css" />
<link href="RadControls/Tabstrip/Skins/Outlook/styles.css" rel="stylesheet" type="text/css" />
<link href="RadControls/Tabstrip/Skins/ClassicBlue/styles.css" rel="stylesheet" type="text/css" />
<link href="RadControls/Editor/Skins/Web20/Editor.css" rel="stylesheet" type="text/css" />
<link href="RadControls/Editor/Skins/Web20/EditorContentArea.css" rel="stylesheet" type="text/css" />
<link href="RadControls/Editor/Skins/Web20/Dialogs.css" rel="stylesheet" type="text/css" />
<link href="RadControls/Editor/Skins/Web20/Controls.css" rel="stylesheet" type="text/css" />
<link href="RadControls/Editor/Skins/Web20/Main.css" rel="stylesheet" type="text/css" />
<link href="scripts/jquery.contextMenu.css" rel="Stylesheet" type="text/css" />
<link href="scripts/jquery-ui.min.css" rel="Stylesheet" type="text/css" />
<link href="scripts/jquery-ui.structure.min.css" rel="Stylesheet" type="text/css" />
<link href="scripts/jquery-ui.theme.min.css" rel="Stylesheet" type="text/css" />
<script type="text/javascript" src="scripts/jquery-2.1.3.min.js"></script>
<script type="text/javascript" src="script.js" language="javascript"></script>
<script type="text/javascript" src="scripts/jquery.contextMenu.js"></script>
<script type="text/javascript" src="scripts/jquery.ui.position.js"></script>
<script type="text/javascript" src="scripts/jquery-ui.min.js"></script>
</head>  
<body style="height:100%;margin:0;" scroll="no">    
<form id="form1" runat="server" style="height:100%" name="form1" enctype='multipart/form-data'>
<script type="text/javascript">
function AjaxRequestWithTarget(uniqueID, parameter)
{
    window['<%=RadAjaxManager1.ClientID %>'].AjaxRequestWithTarget(uniqueID, parameter);
}

function CallApplicantList(parameter)
{
    window['<%=RadAjaxManager1.ClientID %>'].AjaxRequestWithTarget('<%= ApplicantList1.UniqueID %>', parameter);
}
function SetLoadinPanelState(visible)
{
    var loadingpanel=RadAjaxNamespace.LoadingPanels["<%= LoadingPanel1.ClientID %>"];
    if(visible) loadingpanel.Show("<%= ApplicantList1.RadPBMortgages.ClientID %>");
    else loadingpanel.Hide("<%= ApplicantList1.RadPBMortgages.ClientID %>");
}

function ResizePane(displayMode)
{
    var pane = <%= RightPane.ClientID %>;
    var paneSize = pane.width;
    
    if(displayMode == "calendar")
    {

            var paneNewSize = 101;
            for(iAdd = 1; paneNewSize >= 100 && paneNewSize <= 159 ; iAdd++)
            {
               paneNewSize = paneSize - iAdd;
               if(paneNewSize > 160)
                  paneNewSize = 160;
            }
            pane.Resize(-paneNewSize,2); 
    }
    
    if(displayMode == "email") 
    {
        var paneNewSize = (300 - paneSize);
        pane.Resize(paneNewSize,1); 
    }
} 
function ShowHelp(){
<%--    var url = '<%=HelpUrl %>';
    window.open(url);--%>
}
</script>

<div id="LoadingDiv" style="height:19px;">Processing...</div>
<%--<iframe style="position:absolute; left:-200px;top:-200px;width:0px; height:0px;" src="CheckMail.aspx"></iframe>--%>
   
<radW:RadWindowManager ID="WindowManager" runat="server" Skin="WebBlue" Width="320px" Height="240px">
    <Windows>
        <radW:RadWindow ID="CreateMortgage1" NavigateUrl="CreateMortgage.aspx?par=mortgage" runat="server" Modal="True" Skin="WebBlue" Title="New mortgage" SkinsPath="~/RadControls/Window/Skins" Behavior="None" VisibleStatusbar="false" Height="450px" Width="350px" ReloadOnShow="true"/>
        <radW:RadWindow ID="AddBorrower" NavigateUrl="CreateMortgage.aspx?par=borrower" runat="server" Modal="True" Skin="WebBlue" Title="New mortgage" SkinsPath="~/RadControls/Window/Skins" Behavior="None" VisibleStatusbar="false" Width="350px" Height="240px" ReloadOnShow="true"/>
        <radW:RadWindow ID="AdvCalculator" NavigateUrl="AdvCalculator.aspx" runat="server" Modal="True" Skin="WebBlue" Title="Calculator" SkinsPath="~/RadControls/Window/Skins" Behavior="None" VisibleStatusbar="false" Width="350px" Height="240px"/>
        <radW:RadWindow ID="CreateMailFolder" NavigateUrl="CreateMailFolder.aspx" runat="server" Modal="True" Skin="WebBlue" Title="New mail folder" SkinsPath="~/RadControls/Window/Skins" Behavior="None" VisibleStatusbar="false" Width="280px" Height="155px"/>
        <radW:RadWindow ID="GetHiddenValue" NavigateUrl="GetHiddenValue.aspx" runat="server" Modal="True" Skin="WebBlue" Title="Passwordr" SkinsPath="~/RadControls/Window/Skins" Behavior="None" VisibleStatusbar="false" Width="300px" Height="170px" />
    </Windows>
</radW:RadWindowManager>
<radM:radmenu id="rmMortgage" runat="server" Skin="Web20" style="position:absolute;top:0px;left:201px" CausesValidation="false" OnItemClick="rmMortgage_ItemClick" Height="26px" OnClientItemClicking="OnClicking">
    <Items>
        <radm:RadMenuItem Text="Resources" AccessKey="t" Value="Tools" >
            <Items>
                <radm:RadMenuItem Text="My Profile" AccessKey="p" Value="MyProfile"></radm:RadMenuItem>
                <radM:RadMenuItem AccessKey="a" Enabled="false" Text="Retail site" Value="RetailSite" Visible="false"></radm:RadMenuItem>
                <radm:RadMenuItem Text="Create new mortgage" AccessKey="m" Value="NewMortgage"></radm:RadMenuItem>
                <radm:RadMenuItem Text="Emails" AccessKey="e" Value="Emails"></radm:RadMenuItem>
                <radm:RadMenuItem Text="Vendors" AccessKey="v" Value="Vendors"></radm:RadMenuItem>
                <radm:RadMenuItem Text="Closing Cost Profiles" AccessKey="c" Value="GFE"></radm:RadMenuItem>
                <radm:RadMenuItem Text="Reports" AccessKey="r" Value="Reports"></radm:RadMenuItem>
                <radm:RadMenuItem Text="Help" AccessKey="h" Value="Help"></radm:RadMenuItem>                
                <radm:RadMenuItem Text="Links" AccessKey="l" Value="Links"></radm:RadMenuItem>
                <radm:RadMenuItem Text="Login track" AccessKey="t" Value="LoginTrack" Visible="false"></radm:RadMenuItem>
                <radm:RadMenuItem Text="Calendar" AccessKey="c" Value="Calendar" Visible="false"></radm:RadMenuItem>
                <radm:RadMenuItem IsSeparator="True"></radm:RadMenuItem>
                <radm:RadMenuItem Text="Logout" AccessKey="x" Value="Logout"></radm:RadMenuItem>
            </Items>
        </radm:RadMenuItem>
        <radm:RadMenuItem Text="Loan" AccessKey="l" Value="Loan">
            <Items>
                <radM:RadMenuItem AccessKey="n" Text="New borrower" Value="NewBorrower"></radm:RadMenuItem>            
                <radM:RadMenuItem AccessKey="m" Text="Message board" Value="Notes"></radm:RadMenuItem>
                <radm:RadMenuItem Text="Conditions" AccessKey="c" Value="Conditions"></radm:RadMenuItem>
                <radm:RadMenuItem Text="Documents" AccessKey="d" Value="Docs"></radm:RadMenuItem>
                <radm:RadMenuItem Text="Field Changes" AccessKey="f" Value="FieldChanges" Enabled="false" Visible="false"></radm:RadMenuItem>
            </Items>
        </radm:RadMenuItem>        
    </Items>
</radM:radmenu>
<div style="height:100%">
<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Skin="Outlook" Width="100%">   
    <radspl:radpane id="HeaderPane" runat="server" height="25px">
        <uc1:Header ID="Header1" runat="server" />
    </radspl:radpane>  
    <radspl:radsplitbar id="RadSplitBar3" runat="server" CollapseMode="None" EnableResize="false"/>
    <radspl:radpane id="MiddlePane" runat="server" scrolling="None">  
        <radspl:radsplitter id="RadSplitter1" runat="server" Orientation="Vertical" Skin="Outlook" >
            <radspl:radpane id="LeftPane" runat="server" Width="200" MinWidth="200" MaxWidth="250">    
                <asp:Panel id="LeftPanel" runat="server" Height="30px" Width="100%">
                    <uc4:ApplicantList ID="ApplicantList1" runat="server" OnMortgageItemClick="ApplicantList1_MortgageItemClick"/>
                </asp:Panel>   
            </radspl:radpane>                        
            <radspl:radsplitbar id="RadSplitBar1" runat="server" CollapseMode="Forward"/>                    
            <radspl:radpane id="CenterPane" runat="server" MinWidth="530">
                <asp:Panel id="CenterPanel" runat="server" Height="100%">


                </asp:Panel>
            </radspl:radpane>
            <radspl:radsplitbar id="RadSplitBar2" runat="server" collapsemode="Backward" Width="10px"/>
            <radspl:radpane id="RightPane" runat="server" Width="300" MinWidth="100" MaxWidth="300">
                <asp:Panel id="RightPanel" runat="server" Width="100%" Height="100%"></asp:Panel>   
            </radspl:radpane>   
        </radspl:radsplitter>   
    </radspl:radpane>        
    <radspl:radsplitbar id="RadSplitBar4" runat="server" CollapseMode="None" EnableResize="false"/>
    <radspl:radpane id="Radpane2" runat="server" height="30px"><uc2:Footer id="Footer1" runat="server"></uc2:Footer>
    </radspl:radpane>  
</radspl:RadSplitter> 
</div>
<%--<rada:radajaxtimer id="MailTimer" Runat="server" AutoStart="false" style="width: 426px" OnTick="MailTimer_Tick"></rada:radajaxtimer>--%>
<rada:RadAjaxManager id="RadAjaxManager1" runat="server" OnResolveUpdatedControls="RadAjaxManager1_ResolveUpdatedControls" EnableOutsideScripts="true" EnablePageHeadUpdate="false">
    <ClientEvents OnRequestStart="AjaxRequestStart" OnResponseEnd="AjaxResponseEnd" OnResponseReceived="AjaxResponseReceived"></ClientEvents>
    <ajaxsettings>
        <radA:AjaxSetting AjaxControlID="MailTimer">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="MailTimer" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="rmMortgage">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="rmMortgage" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="LeftPanel">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="LeftPanel" />
                <radA:AjaxUpdatedControl ControlID="CenterPanel" />
                <radA:AjaxUpdatedControl ControlID="RightPanel" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="CenterPanel">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="CenterPanel" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="RightPanel">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="RightPanel" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <rada:AjaxSetting AjaxControlID="RadAjaxManager1">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="CenterPanel" />
            </UpdatedControls>
        </rada:AjaxSetting>
    </ajaxsettings>
</rada:RadAjaxManager>
<rada:AjaxLoadingPanel ID="LoadingPanel1" runat="server" Transparency="30" BackColor="#E0E0E0">
</rada:AjaxLoadingPanel>
<input type="hidden" id="currenttab" runat="server"/>
<input type="hidden" id="calcmodified" runat="server"/>
<input id="hdBackFromEmail" type="hidden" />
</form>
</body>   
</html>