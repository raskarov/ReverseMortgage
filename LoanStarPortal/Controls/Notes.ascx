<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Notes.ascx.cs" Inherits="LoanStarPortal.Controls.Notes" %>
<%@ Register Assembly="RadPanelbar.Net2" Namespace="Telerik.WebControls" TagPrefix="radP" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>

<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Width="100%" BorderWidth="0" BorderStyle="None" BorderSize="0" Skin="Default" LiveResize="false">
   <radspl:radpane id="TopPane" runat="server" Height="57px" Scrolling="None">
   <div RadResizeStopLookup="true" RadShowStopLookup="true" >
    <div class="divPaneTitle"><b>Message board</b></div>
<script type="text/javascript">
    function DisplayLevelChanged(o){
        if(o.value==0){
            CollapseAll();    
        }else if(o.value==1){
            ExpandDates();
        }else if(o.value==2){
            ExpandAll();        
        }

    }
    function SetFocusToNote(id1,id2){
        var o = document.getElementById(id2);
        if(o && o!='undefined'){
            if(o.style.display=='none') return;
            o = document.getElementById(id1);
            o.focus();
        }
    }
    function ExpandDates(){
        var panelbar = <%= panelMessageBoard.ClientID %>;    
    for (var i = 0; i < panelbar.AllItems.length; i++){
        var o = panelbar.AllItems[i].GetAttribute("Date");
        if(o && o!='undefined'){
            if(o=='1') {
                panelbar.AllItems[i].Expand();
            } else {
                panelbar.AllItems[i].Collapse();
            }                
        }
        else{
            panelbar.AllItems[i].Collapse();
        }
    }    
}
function CollapseAll(){
    var panelbar = <%= panelMessageBoard.ClientID %>;    
    for (var i = 0; i < panelbar.AllItems.length; i++){
        panelbar.AllItems[i].Collapse();
    }
}
function ExpandAll(){
    var panelbar = <%= panelMessageBoard.ClientID %>;    
    for (var i = 0; i < panelbar.AllItems.length; i++){
        panelbar.AllItems[i].Expand();
    }
}
function CheckKeys(e) {
    var kCode = e.keyCode || e.which;
    if ((e.ctrlKey && kCode == 13) || (kCode==17 && kCode == 13)) {
        SubmitNote();
    }
}
function SubmitNote(){
    RadAjaxManager1.AjaxRequestWithTarget('<%= btnCreate.UniqueID %>', 'btnCreate');
}
</script>
   
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>&nbsp;&nbsp;<img src="/Images/Add-Note.gif" /><a href="#" onclick="HideShowDiv('<%=divQuickNote.ClientID%>');SetFocusToNote('<%=tbNote.ClientID%>','<%=divQuickNote.ClientID%>');">Add note</a></td>
            <td align="right">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="padding-top:4px;">
                        <td align="right" style="padding-right:3px;">Display level:</td>
                        <td align="left"><asp:DropDownList runat="server" ID="ddlDisplayLevel" Width="90px" ></asp:DropDownList></td>
                    </tr>
                </table>            
            </td>
        </tr>
    </table>
    </div>
   </radspl:radpane>  
   <radspl:radsplitbar id="RadSplitBar3" runat="server" CollapseMode="None" EnableResize="false"/>
   <radspl:radpane id="MiddlePane" runat="server" scrolling="Y" >
   <div runat="server" id="divQuickNote" style="display:none;border:1px solid#d0d0FF;width:99%;">
        <div class="paneTitle">
           <table style="width:99%;">
            <tr>
                <td>Edit note</td>
                <td align="right">
                    <asp:Button ID="btnCreate" runat="server" Text="Create" OnClick="btnCreate_Click" />
                    <input type="button" value="Cancel" onclick="HideShowDiv('<%=divQuickNote.ClientID%>    ')" />
                </td>
            </tr>
            </table><br />
        </div>
        <asp:TextBox ID="tbNote" runat="server" TextMode="MultiLine" Rows="5" Columns="1" Width="99%" onkeydown="CheckKeys(event);"></asp:TextBox>
    </div>
    <radP:RadPanelbar runat="server" ID="panelMessageBoard" Skin="WebBlue" Width="100%" CausesValidation="False">
    </radP:RadPanelbar>
   </radspl:radpane>
   <radspl:radsplitbar id="RadSplitBar1" runat="server" CollapseMode="None" EnableResize="false"/>
    <radspl:RadPane ID="BottomPane" runat="server" CssClass="bottomPane" Height="67px" Scrolling="None">
        <div RadResizeStopLookup="true" RadShowStopLookup="true" >
        <br />&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:CheckBox ID="chbEvents" runat="server" Text="Events" Checked="false" AutoPostBack="true" OnCheckedChanged="FilterChanged"/>        
        <asp:CheckBox ID="chbNote" runat="server" Text="Notes" Checked="true" AutoPostBack="true" OnCheckedChanged="FilterChanged"/><br />        
        <br />
        </div>
    </radspl:radpane>  
</radspl:radsplitter>  