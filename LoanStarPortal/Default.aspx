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

<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
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
    <script type="text/javascript" src="script.js"></script>
    <script type="text/javascript" src="scripts/jquery.contextMenu.js"></script>
    <script type="text/javascript" src="scripts/jquery.ui.position.js"></script>
    <script type="text/javascript" src="scripts/jquery-ui.min.js"></script>
</head>
<body style="height: 100%; margin: 0;" scroll="no">
    <form id="form1" runat="server" style="height: 100%" name="form1" enctype='multipart/form-data'>
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

        <div id="LoadingDiv" style="height: 19px;">Processing...</div>
        <%--<iframe style="position:absolute; left:-200px;top:-200px;width:0px; height:0px;" src="CheckMail.aspx"></iframe>--%>

        <radW:RadWindowManager ID="WindowManager" runat="server" Skin="WebBlue" Width="320px" Height="240px">
            <Windows>
                <radW:RadWindow ID="CreateMortgage1" NavigateUrl="CreateMortgage.aspx?par=mortgage" runat="server" Modal="True" Skin="WebBlue" Title="New mortgage" SkinsPath="~/RadControls/Window/Skins" Behavior="None" VisibleStatusbar="false" Height="450px" Width="350px" ReloadOnShow="true" />
                <radW:RadWindow ID="AddBorrower" NavigateUrl="CreateMortgage.aspx?par=borrower" runat="server" Modal="True" Skin="WebBlue" Title="New mortgage" SkinsPath="~/RadControls/Window/Skins" Behavior="None" VisibleStatusbar="false" Width="350px" Height="240px" ReloadOnShow="true" />
                <radW:RadWindow ID="AdvCalculator" NavigateUrl="AdvCalculator.aspx" runat="server" Modal="True" Skin="WebBlue" Title="Calculator" SkinsPath="~/RadControls/Window/Skins" Behavior="None" VisibleStatusbar="false" Width="350px" Height="240px" />
                <radW:RadWindow ID="CreateMailFolder" NavigateUrl="CreateMailFolder.aspx" runat="server" Modal="True" Skin="WebBlue" Title="New mail folder" SkinsPath="~/RadControls/Window/Skins" Behavior="None" VisibleStatusbar="false" Width="280px" Height="155px" />
                <radW:RadWindow ID="GetHiddenValue" NavigateUrl="GetHiddenValue.aspx" runat="server" Modal="True" Skin="WebBlue" Title="Passwordr" SkinsPath="~/RadControls/Window/Skins" Behavior="None" VisibleStatusbar="false" Width="300px" Height="170px" />
            </Windows>
        </radW:RadWindowManager>
        <radM:RadMenu ID="rmMortgage" runat="server" Skin="Web20" Style="position: absolute; top: 0px; left: 201px" CausesValidation="false" OnItemClick="rmMortgage_ItemClick" Height="26px" OnClientItemClicking="OnClicking">
            <Items>
                <radM:RadMenuItem Text="Resources" AccessKey="t" Value="Tools">
                    <Items>
                        <radM:RadMenuItem Text="My Profile" AccessKey="p" Value="MyProfile"></radM:RadMenuItem>
                        <radM:RadMenuItem AccessKey="a" Enabled="false" Text="Retail site" Value="RetailSite" Visible="false"></radM:RadMenuItem>
                        <radM:RadMenuItem Text="Create new mortgage" AccessKey="m" Value="NewMortgage"></radM:RadMenuItem>
                        <radM:RadMenuItem Text="Emails" AccessKey="e" Value="Emails"></radM:RadMenuItem>
                        <radM:RadMenuItem Text="Vendors" AccessKey="v" Value="Vendors"></radM:RadMenuItem>
                        <radM:RadMenuItem Text="Closing Cost Profiles" AccessKey="c" Value="GFE"></radM:RadMenuItem>
                        <radM:RadMenuItem Text="Reports" AccessKey="r" Value="Reports"></radM:RadMenuItem>
                        <radM:RadMenuItem Text="Help" AccessKey="h" Value="Help"></radM:RadMenuItem>
                        <radM:RadMenuItem Text="Links" AccessKey="l" Value="Links"></radM:RadMenuItem>
                        <radM:RadMenuItem Text="Login track" AccessKey="t" Value="LoginTrack" Visible="false"></radM:RadMenuItem>
                        <radM:RadMenuItem Text="Calendar" AccessKey="c" Value="Calendar" Visible="false"></radM:RadMenuItem>
                        <radM:RadMenuItem IsSeparator="True"></radM:RadMenuItem>
                        <radM:RadMenuItem Text="Logout" AccessKey="x" Value="Logout"></radM:RadMenuItem>
                    </Items>
                </radM:RadMenuItem>
                <radM:RadMenuItem Text="Loan" AccessKey="l" Value="Loan">
                    <Items>
                        <radM:RadMenuItem AccessKey="n" Text="New borrower" Value="NewBorrower"></radM:RadMenuItem>
                        <radM:RadMenuItem Text="Field Changes" AccessKey="f" Value="FieldChanges" Enabled="false" Visible="false"></radM:RadMenuItem>
                    </Items>
                </radM:RadMenuItem>
            </Items>
        </radM:RadMenu>
        <radM:RadMenu runat="server" ID="RightMenu" Skin="Default" style="position: absolute; top: 5px; right: 25px;" OnItemClick="RightMenu_ItemClick" Height="26px">
            <Items>
                <radM:RadMenuItem AccessKey="m" Text="Message board" Value="Notes" CssClass="header-link"></radM:RadMenuItem>
                <radM:RadMenuItem Text="Conditions" AccessKey="c" Value="Conditions" CssClass="header-link"></radM:RadMenuItem>
                <radM:RadMenuItem Text="Documents" AccessKey="d" Value="Docs" CssClass="header-link"></radM:RadMenuItem>
            </Items>
        </radM:RadMenu>
        <div style="height: 100%">
            <radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Skin="Outlook" Width="100%">
                <radspl:RadPane ID="HeaderPane" runat="server" Height="25px">
                    <uc1:Header ID="Header1" runat="server" />
                </radspl:RadPane>
                <radspl:RadSplitBar ID="RadSplitBar3" runat="server" CollapseMode="None" EnableResize="false" />
                <radspl:RadPane ID="MiddlePane" runat="server" Scrolling="None">
                    <radspl:RadSplitter ID="RadSplitter1" runat="server" Orientation="Vertical" Skin="Outlook">
                        <radspl:RadPane ID="LeftPane" runat="server" Width="200" MinWidth="200" MaxWidth="250">
                            <asp:Panel ID="LeftPanel" runat="server" Height="30px" Width="100%">
                                <uc4:ApplicantList ID="ApplicantList1" runat="server" OnMortgageItemClick="ApplicantList1_MortgageItemClick" />
                            </asp:Panel>
                        </radspl:RadPane>
                        <radspl:RadSplitBar ID="RadSplitBar1" runat="server" CollapseMode="Forward" />
                        <radspl:RadPane ID="CenterPane" runat="server" MinWidth="530">
                            <asp:Panel ID="CenterPanel" runat="server" Height="100%">
                            </asp:Panel>
                        </radspl:RadPane>
                        <radspl:RadSplitBar ID="RadSplitBar2" runat="server" CollapseMode="Backward" Width="10px" />
                        <radspl:RadPane ID="RightPane" runat="server" Width="300" MinWidth="100" MaxWidth="300">
                            <asp:Panel ID="RightPanel" runat="server" Width="100%" Height="100%"></asp:Panel>
                        </radspl:RadPane>
                    </radspl:RadSplitter>
                </radspl:RadPane>
                <radspl:RadSplitBar ID="RadSplitBar4" runat="server" CollapseMode="None" EnableResize="false" />
                <radspl:RadPane ID="Radpane2" runat="server" Height="30px">
                    <uc2:Footer ID="Footer1" runat="server"></uc2:Footer>
                </radspl:RadPane>
            </radspl:RadSplitter>

            <asp:Panel runat="server" ID="DialogWrapperPanel" CssClass="pnlDialog left_hide">
                <div class="paneGrid" style="width: 100%; height: 25px;">
                    <b>Details</b>
                    <asp:Button CssClass="rght" ID="btnPanelDialogHide" runat="server" Text="X" OnClick="btnPanelDialogHide_Click" />
                </div>
                <asp:Panel ID="DialogPanel" runat="server">
                </asp:Panel>
            </asp:Panel>
        </div>
        <%--<rada:radajaxtimer id="MailTimer" Runat="server" AutoStart="false" style="width: 426px" OnTick="MailTimer_Tick"></rada:radajaxtimer>--%>
        <radA:RadAjaxManager ID="RadAjaxManager1" runat="server" OnResolveUpdatedControls="RadAjaxManager1_ResolveUpdatedControls" EnableOutsideScripts="true" EnablePageHeadUpdate="false">
            <ClientEvents OnRequestStart="AjaxRequestStart" OnResponseEnd="AjaxResponseEnd" OnResponseReceived="AjaxResponseReceived"></ClientEvents>
            <AjaxSettings>
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
                <radA:AjaxSetting AjaxControlID="RightMenu">
                    <UpdatedControls>
                        <radA:AjaxUpdatedControl ControlID="RightMenu" />
                    </UpdatedControls>
                </radA:AjaxSetting>
                <radA:AjaxSetting AjaxControlID="RightPanel">
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
                <radA:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
                        <radA:AjaxUpdatedControl ControlID="CenterPanel" />
                    </UpdatedControls>
                </radA:AjaxSetting>
                <radA:AjaxSetting AjaxControlID="DialogPanel">
                    <UpdatedControls>
                        <radA:AjaxUpdatedControl ControlID="DialogPanel" />
                    </UpdatedControls>
                </radA:AjaxSetting>
            </AjaxSettings>
        </radA:RadAjaxManager>
        <radA:AjaxLoadingPanel ID="LoadingPanel1" runat="server" Transparency="30" BackColor="#E0E0E0">
        </radA:AjaxLoadingPanel>
        <input type="hidden" id="currenttab" runat="server" />
        <input type="hidden" id="calcmodified" runat="server" />
        <input id="hdBackFromEmail" type="hidden" />
    </form>
</body>
</html>
