<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RetailPage.aspx.cs" Inherits="LoanStarPortal.RetailSiteTemplate.RetailPage" MaintainScrollPositionOnPostback="true" %>
<%@ Register Src="Control/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<%@ Register Src="Control/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Assembly="RadWindow.Net2" Namespace="Telerik.WebControls" TagPrefix="radW" %>
<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />
    <link href="layout.css" rel="stylesheet" type="text/css" />
    <link href="input.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../script.js" language="javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
    var needSetFocus=false;    
    function Reload(){
        window['<%=RadAjaxManager1.ClientID %>'].AjaxRequestWithTarget('<%=Panel1.ClientID %>','');
    }
    function Reload1(o,v1,v2){
        SetNewValues(o,v1,v2);
    }    
    function RsAdvCalculator(){
        radopen("RsAdvancedCalculator.aspx","RsAdvCalculator");
        return false; 
    }    
    function CheckInputOnSave(id1,id2){
        var r=true;
        var r1=CheckValue(id1,'');
        r = r&&r1;
        r1=CheckValue(id2,'');
        r = r&&r1;
        return r;
    }
    function CheckInputOnCalculate(id1,id2,id3,id4){
        var r=true;
        var r1=CheckValue(id1,'');
        r=r&&r1;
        r1=CheckValue(id2,'0');
        r=r&&r1;
        r1=CheckValue(id3,'0');
        r=r&&r1;
        r1=CheckValue(id4,'');
        r=r&&r1;
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
    function AjaxRequestStart(target,args){
        var s=args.EventTarget;
        needSetFocus=s.indexOf('$ddlState')>0;
    }
    function AjaxResponseEnd(){
        if (needSetFocus){
            window.setTimeout('RestoreFocus()',500);
        }
    }
</script>  
<radW:RadWindowManager ID="wm1" runat="server"  Skin="WebBlue" Width="320px" Height="240px">
    <Windows>
            <radW:RadWindow ID="RsAdvCalculator1" NavigateUrl="RsAdvancedCalculator.aspx" runat="server" Modal="True" Title="Calculator" Behavior="None" VisibleStatusbar="false" Width="340px" Height="300px" Skin="WinnXpSilver" SkinsPath="~/RadControls/Window/Skins"/>
            <radW:RadWindow ID="RsAdvCalculator" NavigateUrl="RsAdvancedCalculator.aspx" runat="server" Modal="True" Skin="WebBlue" Title="Calculator" SkinsPath="~/RadControls/Window/Skins" Behavior="None" VisibleStatusbar="false" Width="350px" Height="240px" />
    </Windows>
</radW:RadWindowManager>
        <div id="main">
            <uc1:Header ID="Header1" runat="server" />
            <asp:Panel ID="Panel1" runat="server" >
            </asp:Panel>            
            <div class="clear"></div>
            <uc2:Footer ID="Footer1" runat="server" />            
            </div>
            </div>
        </div>        
<rada:RadAjaxManager id="RadAjaxManager1" runat="server" EnableOutsideScripts="True" >
    <AjaxSettings>
        <radA:AjaxSetting AjaxControlID="wm1">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="Panel1" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="Panel1">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="Panel1" />
            </UpdatedControls>
        </radA:AjaxSetting>
    </AjaxSettings>    
</rada:RadAjaxManager>
    </form>        
</body>
</html>
