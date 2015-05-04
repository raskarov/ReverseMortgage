<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RulesTree.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.RulesTree" %>

<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>
<%@ Register Assembly="RadTreeView.Net2" Namespace="Telerik.WebControls" TagPrefix="radT" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>

<script language="javascript" type="text/javascript">
    function ExpandCollapseTree(o) {
        var t = window['<%=rtvRule.ClientID %>'];
        var v=o.getAttribute('val')==0;    
        var i;
        for(i=0; i<t.AllNodes.length; i++){
            if(v)
                t.AllNodes[i].Expand();
            else            
                t.AllNodes[i].Collapse();
        }
        var newv = '0';
        var txt = 'Expand all';    
        if(v){
            newv = 1 ;
            txt = 'Collapse all';
        }    
        o.setAttribute('val',newv);
        o.setAttribute('value',txt);
    }

    function SetVisibility(o,t1,t2) {
        var vis=o.checked?'none':'block';
        var o=document.getElementById(t1);
        if(o!=null){
            o.style.display=vis;
        }
        o=document.getElementById(t2);
        if(o!=null){
            o.style.display=vis;
        }
    }

    function CheckDelete(node,itemText) {
        if (itemText.toLowerCase()=='delete'){
            return confirm('Are you sure you want to delete this rule?');
        }
        return true;
    }

    function CheckAll(o,d) {
        var e = d.getElementsByTagName('input');
        for (var i=0; i<e.length; i++){
            if (e[i].type=='checkbox'){
                if(!e[i].parentElement.disabled){
                    e[i].checked=o.checked;
                }            
            }
        }
    }

    function CheckField(o,o1,d) {
        var e=d.getElementsByTagName('input');
        var cnt1=0;
        var cnt2=0;
        for (var i=0; i<e.length; i++){
            if ((e[i].type=='checkbox')&&(e[i].id!=o1.id)){
                cnt1 += e[i].checked?1:0;
                cnt2++;
            }
        } 
        if (o.checked){
            o1.checked = cnt1==cnt2;
        }else{
            o1.checked = false;
        }
    }

    function RaddpValuePopup() {
        <%= dpValue.ClientID %>.ShowPopup();
    }

    function RuleTabSelected(sender, eventArgs) {
        var tab = eventArgs.Tab
        var o=document.getElementById('<%=currentruletab.ClientID %>');
        if (o!=null){
            o.value=tab.Index;
        }    
    }

    function SetTabIndex(index) {
        var o = document.getElementById('<%=currentruletab.ClientID %>'); 
        if (o!=null){
            o.value=index;
        }    
    }

    function CheckItems(status,o1,o2,o3,o4,o5) {
        if (status)
            document.getElementById(o1).removeAttribute('disabled');
        else
            document.getElementById(o1).setAttribute('disabled','true');

        SetDisabled(document.getElementById(o2),status);
        SetDisabled(document.getElementById(o3),status);
        SetDisabled(document.getElementById(o4),status);
        SetDisabled(document.getElementById(o5),status);    
        if (status){
            document.getElementById(o1).focus();
        }
    }

    function SetDisabled(o,status) {
        var p = o.parentElement;
        if(status)
        {
            o.removeAttribute('disabled');
            p.removeAttribute('disabled');
        }
        else
        {
            o.setAttribute('disabled','true');
            p.setAttribute('disabled','true');
        }
    }

    function SetSelect(status,o1) {
        SetDisabled(o1,status);
    }

    function RadDatadpValuePopup(){
        <%= rdpData.ClientID %>.ShowPopup();
    }

    var clp=null;
    var cuc="<%=divloading.ClientID %>";

    function RequestStart(args){
        centerElementOnScreen(document.getElementById(cuc),-50,-100);
        clp=RadAjaxNamespace.LoadingPanels["<%= AjaxLoadingPanel1.ClientID %>"];
        clp.Show(cuc);
    }

    function OnResponceEnd(){
        clp.Hide(cuc);
    }

    function centerElementOnScreen(element,dx,dy){
         var scrollTop = document.body.scrollTop;
         var scrollLeft = document.body.scrollLeft;
         var viewPortHeight = document.body.clientHeight;
         var viewPortWidth = document.body.clientWidth;
         if (document.compatMode == "CSS1Compat"){
            viewPortHeight = document.documentElement.clientHeight;
            viewPortWidth = document.documentElement.clientWidth;
            scrollTop = document.documentElement.scrollTop;
            scrollLeft = document.documentElement.scrollLeft;
         }
         var topOffset = Math.ceil(viewPortHeight/2 - element.offsetHeight/2);
         var leftOffset = Math.ceil(viewPortWidth/2 - element.offsetWidth/2);
         var top = scrollTop + topOffset - 40+dy;
         var left = scrollLeft + leftOffset - 70+dx;
         element.style.position = "absolute";
         element.style.top = top + "px";
         element.style.left = left + "px";
    }

    function SetTabsStyle(s) {
        var ar = s.split(',');
        var ts = <%= tabsRuleEdit.ClientID %>;
        if (ts) {
            if(ar[0]!='0'){
                var on=ar[0]=='1';
                for (var i=1; i<ts.Tabs.length; i++){
                    var tb =ts.Tabs[i];
                    if(on)  ts.Tabs[i].Enable();
                    else ts.Tabs[i].Disable();
                }
            }else{
                for(var k=1;k<ar.length;k++){
                    if(ar[k]!='0'){
                        var s = ts.Tabs[k].ID;
                        var en=ar[k]=='1'
                        var o = document.getElementById(ts.Tabs[k].ID);
                        if(o!=null) o.style.color=en?'':'gray';
                    }
                }
            }    
        }
    }

    function UpdateContent(tabs) {
        SetTabsStyle(tabs);
        ScrollToSelectedNode();
    }

    function Dbg() {
        ScrollToSelectedNode();
    }

    function ScrollToSelectedNode() {
        var selectedNode = ctl07_rtvRule.SelectedNode; 
        if (selectedNode != null){  
            var o = document.getElementById(selectedNode.ClientID);
            var t = document.getElementById(ctl07_rtvRule.ClientID);
            var p = document.getElementById('RAD_SPLITTER_PANE_CONTENT_'+'<%=LeftPane.ClientID %>');
            var trh=document.getElementById('trCat').offsetHeight;
            var d = trh+o.offsetTop-p.clientHeight;
            if(d>0){
                d+=trh;
                p.scrollTop=d;
            }
        }
    }

    function DropTest(source, dest, events){
        if (dest==null) return false;
        return !(source.Parent == dest);
    }
</script>
<table cellpadding="0" cellspacing="0" border="0" style="width:100%;height:600px">
    <tr>
        <td style="width:100%;vertical-align:top;height:100%;border:solid black 1px">
            <radspl:radsplitter id="RadSplitter1" runat="server" Orientation="Vertical" Skin="Outlook" Height="100%" Width="100%">
                <radspl:radpane id="LeftPane" runat="server" Width="40%">    
                <table cellpadding="0" cellspacing="0" border="0" style="width:100%">
                    <tr style="height:20px;padding-top:5px;padding-bottom:5px" id="trCat">
                        <td style="padding-left:50px" align="left">
                            <asp:Label ID="Label1" runat="server" Text="Category"></asp:Label>&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlCategory" runat="server" Width="195px" AutoPostBack="True"></asp:DropDownList>&nbsp;&nbsp;
                            <input id="btnExpandCollapse" type="button" value="Expand all" runat="server" onclick="ExpandCollapseTree(this);" val="0"/>
                        </td>                        
                    </tr>
                    <tr>
                        <td valign="top">
                            <radT:RadTreeView ID="rtvRule" runat="server" Skin="Outlook" Width="100%" ExpandDelay="0" OnNodeBound="rtvRule_NodeBound" BeforeClientContextClick="CheckDelete" OnNodeContextClick="rtvRule_NodeContextClick" OnNodeClick="rtvRule_NodeClick" DragAndDrop="True" DragAndDropBetweenNodes="True" OnNodeDrop="rtvRule_NodeDrop" BeforeClientDrop="DropTest">
                            </radT:RadTreeView>
                        </td>
                    </tr>
                </table>                
                </radspl:radpane>                   
                <radspl:radsplitbar id="RadSplitBar2" runat="server" collapsemode="Backward" Width="10px"/>
                <radspl:radpane id="RightPane" runat="server">
                    <asp:Panel ID="pnlEditRule" runat="server" Width="100%">
                    <div id="divEditRule" runat="server" Visible="false">
                    <table cellpadding="0" cellspacing="0" border="0" style="width:100%">
                        <tr>
                            <td style="padding:5px;">
                                <asp:Label ID="lblRuleName" runat="server" Text="Label" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding:5px;">                            
                                <asp:Label ID="Label2" runat="server" Text="Root expression:" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblRuleExpression" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <radTS:RadTabStrip id="tabsRuleEdit" runat="server" Skin="Outlook" MultiPageID="mpRuleEdit" SelectedIndex="0" CausesValidation="false" Orientation="HorizontalTopToBottom" OnClientTabSelected="RuleTabSelected">
                                        <Tabs>
                                            <radts:Tab Text="Rule Properties" Value="General" ID="tabGeneral"></radts:Tab>        
                                            <radts:Tab Text="Show fields" Value="Fields" ID="tabShowFields"></radts:Tab>
                                            <radts:Tab Text="Conditions" Value="Conditions" ID="tabConditions"></radts:Tab>
                                            <radts:Tab Text="Documents" Value="Documents" ID="tabDocuments"></radts:Tab>
                                            <radts:Tab Text="CheckList" Value="CheckLists" ID="tabCheckLists"></radts:Tab>
                                            <radts:Tab Text="Alert" Value="Alerts" ID="tabAlerts"></radts:Tab>
                                            <radts:Tab Text="Data" Value="Data" ID="tabData"></radts:Tab>
                                        </Tabs>
                                    </radTS:RadTabStrip>
                                    <radTS:RadMultiPage id="mpRuleEdit" runat="server" SelectedIndex="0" Height="100%" EnableViewState="False">
                                        <radTS:PageView id="pvGeneral" runat="server" >
                                        <div style="width:100%;padding:5px;">
                                            <table style="width:99%" cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td>
                                                        <table style="width:98%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td style="width:100%">
                                                                    <asp:Label ID="Label3" runat="server" Text="Settings"></asp:Label>
                                                                </td>
                                                                <td style="width:5px">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div style="border:solid 1px black;background-color:#E7F0FB;padding:3px">
                                                                        <table style="width:100%" cellpadding="0" cellspacing="0" border="0">
                                                                            <tr>
                                                                                <td style="width:150px;padding-left:5px" align="left">
                                                                                    <asp:Label ID="Label4" runat="server" Text="Status(Enabled/Disabled)"></asp:Label>
                                                                                </td>
                                                                                <td style="width:10px">
                                                                                    <asp:CheckBox ID="cbStatus" runat="server" />
                                                                                </td>
                                                                                <td style="width:60px;" align="right">
                                                                                    <asp:Label ID="Label5" runat="server" Text="Category:"></asp:Label>
                                                                                </td>
                                                                                <td style="width:180px;padding-left:3px" align="left">
                                                                                    <asp:Label ID="lblCategory" runat="server" Text=""></asp:Label>
                                                                                    <radC:RadComboBox ID="rcbCategory" runat="server" AllowCustomText="True" Height="100px" Width="95%" MarkFirstMatch="True" ShowDropDownOnTextboxClick="False" ZIndex="10000" Skin="WebBlue" UseEmbeddedScripts="False"></radC:RadComboBox>
                                                                                </td>
                                                                                <td style="padding-left:3px"><asp:Label ID="lblErrCategory" runat="server" Text="" ForeColor="red"></asp:Label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="5"><asp:Label ID="Label39" runat="server" Text="Comments:"></asp:Label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="5">
                                                                                    <asp:TextBox ID="tbComments" runat="server" TextMode="MultiLine" Rows="4" Width="95%"></asp:TextBox>
                                                                                 </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                                <td style="width:5px">&nbsp;</td>
                                                            </tr>
                                                            <tr id="trAddExpression" runat="server">
                                                                <td style="width:100%">
                                                                    <asp:Label ID="Label7" runat="server" Text="Add new expression"></asp:Label>
                                                                </td>
                                                                <td style="width:5px">&nbsp;</td>
                                                            </tr>                                                            
                                                            <tr id="trEditor" runat="server">
                                                                <td>
                                                                    <div style="border:solid 1px black;background-color:#E7F0FB;padding:3px" >
                                                                        <table style="width:100%" cellpadding="0" cellspacing="0" border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Panel ID="pnlEditor" runat="server" Width="100%">                                                                                    
                                                                                    <table style="width:100%" cellpadding="0" cellspacing="0" border="0">
                                                                                        <tr>
                                                                                            <td class="expediotrlbl">
                                                                                                <asp:Label ID="Label9" runat="server" Text="NOT" SkinID="AdminLabel"></asp:Label>
                                                                                            </td>
                                                                                            <td class="expeditorctl">
                                                                                                <asp:CheckBox ID="cbNot" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>                
                                                                                            <td class="expediotrlbl">
                                                                                                <asp:Label ID="Label10" runat="server" Text="Logical Op" SkinID="AdminLabel"></asp:Label>
                                                                                            </td>
                                                                                            <td class="expeditorctl">
                                                                                                <asp:DropDownList ID="ddlLogicalOp" runat="server" Width="100px" AutoPostBack="True" CssClass="selectcenter" SkinID="SelectCenter" Enabled="false"></asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="expediotrlbl">
                                                                                                <asp:Label ID="Label11" runat="server" Text="Select group" SkinID="AdminLabel"></asp:Label>
                                                                                            </td>
                                                                                            <td class="expeditorctl">
                                                                                                <asp:DropDownList ID="ddlGroup" runat="server" Width="150px" AutoPostBack="True" Enabled="false"></asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="expediotrlbl">
                                                                                                <asp:Label ID="Label12" runat="server" Text="Select field" SkinID="AdminLabel"></asp:Label>
                                                                                            </td>
                                                                                            <td class="expeditorctl">
                                                                                                <asp:DropDownList ID="ddlField" runat="server" Width="95%" AutoPostBack="True" Enabled="false"></asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="expediotrlbl">
                                                                                                <asp:Label ID="Label13" runat="server" Text="Compare Op" SkinID="AdminLabel"></asp:Label>
                                                                                            </td>
                                                                                            <td class="expeditorctl">
                                                                                                <asp:DropDownList ID="ddlCompareOp" Width="100px" runat="server" AutoPostBack="True" SkinID="SelectCenter" Enabled="false"></asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>                                                                                            
                                                                                            <td class="expediotrlbl">
                                                                                                <asp:Label ID="Label14" runat="server" Text="Field type" SkinID="AdminLabel"></asp:Label>
                                                                                            </td>
                                                                                            <td class="expeditorctl">
                                                                                                <asp:DropDownList ID="ddlFieldType" Width="100px" runat="server" AutoPostBack="True" SkinID="SelectCenter" Enabled="false"></asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>                                                                                                           
                                                                                            <td class="expediotrlbl" valign="top">
                                                                                                <asp:Label ID="lblValue" runat="server" Text="Enter value" SkinID="AdminLabel" Width="130px" ></asp:Label>
                                                                                            </td>
                                                                                            <td class="expeditorctl">
                                                                                                <asp:TextBox ID="tbValue" runat="server" Width="95%" MaxLength="256"></asp:TextBox>
                                                                                                <asp:DropDownList ID="ddlDictionary"  Width="95%" runat="server" Visible="false"></asp:DropDownList>
                                                                                                <radCln:RadDatePicker ID="dpValue" runat="server" Width="100px" Height="20px" FocusedDate="2099-12-31" MinDate="1800-01-01"  Visible="false" SkinID="WebBlue">
                                                                                                    <DatePopupButton Visible="False" />
                                                                                                    <DateInput onclick="RaddpValuePopup()" SkinID="WebBlue" Height="20px" Width="100px" />
                                                                                                </radCln:RadDatePicker>
                                                                                                <radi:RadNumericTextBox id="mtb" runat="server" Width="95%" Visible="false"></radi:RadNumericTextBox>
                                                                                                &nbsp;&nbsp;<asp:Label ID="validatormsg" runat="server" Text="" ForeColor="red"></asp:Label>
                                                                                            </td>                                                                                            
                                                                                        </tr>                                                                                    
                                                                                        <tr style="padding-bottom:5px">
                                                                                            <td class="expediotrlbl"></td>
                                                                                            <td><asp:Button ID="btnCancelExpression" runat="server" Text="Back" SkinID="AdminButton" CausesValidation="False" OnClick="btnCancelExpression_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnAddExpression" runat="server" Text="Add" SkinID="AdminButton" OnClick="btnAddExpression_Click"/></td>
                                                                                        </tr>                                                                                                                                                                            
                                                                                    </table>
                                                                                    </asp:Panel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>                                                                    
                                                                </td>
                                                                <td style="width:5px">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width:100%">
                                                                    <asp:Label ID="Label8" runat="server" Text="Expression grid"></asp:Label>
                                                                </td>
                                                                <td style="width:5px">&nbsp;</td>
                                                            </tr>                                                            
                                                            <tr>
                                                                <td>
                                                                    <div style="border:solid 1px black;background-color:#E7F0FB;padding:3px">
                                                                        <asp:Panel ID="pnlExpressionGrid" runat="server" Width="100%">
                                                                        <table style="width:100%" cellpadding="0" cellspacing="0" border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:GridView ID="gProperties" runat="server" AutoGenerateColumns="False" AllowPaging="true" DataKeyNames="Id" GridLines="None" OnRowDataBound="G_ItemDataBound" EmptyDataText="No records to display" ForeColor="#333333" OnRowCommand="gProperties_ItemCommand" OnPageIndexChanged="G_PageIndexChanged" OnPageIndexChanging="G_PageIndexChanging"  EnableViewState="False" SkinID="TotalGrid" >
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Logical Op">
                                                                                            <ItemTemplate>                                                                                            
                                                                                                <asp:Label ID="lbllogop" runat="server" Text='<%# GetLogicalOp(Container.DataItem) %>'></asp:Label>                                                                                                
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Center" Width="40px"/>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lbllognot" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "logicalnot") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="20px"/>                                                                                        
                                                                                        </asp:TemplateField>        
                                                                                        <asp:TemplateField HeaderText="Field">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblfield" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="120px"/>                                                                                        
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Compare Op">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblcompareop" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "compareop") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Center" Width="40px"/>                                                                                        
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Value">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblvaleu" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "datavalue") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Field">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblfieldname" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FieldName") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Center" Width="100px"/>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Action">
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="deleteobject" AlternateText="Delete item" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>
                                                                                            </ItemTemplate> 
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="40px"/>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>                                                                                        
                                                                                    </Columns>                                                                                        
                                                                                    </asp:GridView>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        </asp:Panel>
                                                                    </div>                                                                    
                                                                </td>
                                                                <td style="width:5px">&nbsp;</td>
                                                            </tr>
                                                            <tr style="height:25px;padding-top:8px">
                                                                <td colspan="2" align="right" style="padding-right:3px">
                                                                    <asp:Button ID="btnCancelProperties" runat="server" Text="Cancel" OnClick="btnCancelProperties_Click" Width="80px" />
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    <asp:Button ID="btnSaveProperties" runat="server" Text="Save" OnClick="btnSaveProperties_Click" Width="80px"  />
                                                                </td>
                                                            </tr>                                                            
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        </radTS:PageView>
                                        <radTS:PageView id="pvFields" runat="server" >
                                        <div style="width:100%;padding:5px">
                                            <table style="width:98%" border="0" cellpadding="0" cellspacing="0" >
                                                <tr id="trAddNewField" runat="server">
                                                    <td>
                                                        <asp:Label ID="Label38" runat="server" Text="Add new field"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trFields" runat="server">
                                                    <td>
                                                        <div style="border:solid 1px black;background-color:#E7F0FB;padding:3px">
                                                        <asp:Panel ID="pnlDdlFields" runat="server" Width="100%">
                                                        <table style="width:100%" border="0" cellpadding="0" cellspacing="0" >
                                                            <tr>
                                                                <td style="width:80px;padding-right:3px" align="right">
                                                                    <asp:Label ID="Label15" runat="server" Text="Select group" SkinID="AdminLabel"></asp:Label>
                                                                </td>    
                                                                <td style="width:85%">
                                                                    <asp:DropDownList ID="ddlGroupField" runat="server" Width="120px" AutoPostBack="true" ></asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>                                                                
                                                                <td style="width:80px;padding-right:3px" align="right">
                                                                    <asp:Label ID="Label16" runat="server" Text="Select field" SkinID="AdminLabel"></asp:Label>                    
                                                                </td>
                                                                <td style="width:85%">
                                                                    <asp:DropDownList ID="ddlFieldField" runat="server" Width="90%"></asp:DropDownList>
                                                                    &nbsp;<asp:Label ID="lblErrField" runat="server" Text="" ForeColor="red"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr style="padding-top:5px;padding-bottom:5px">
                                                                <td>&nbsp;</td>
                                                                <td align="left">
                                                                    <asp:Button ID="btnBackField" runat="server" Text="Back" SkinID="AdminButton" CausesValidation="False" OnClick="btnBackField_Click"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnAddField" runat="server" Text="Add"  SkinID="AdminButton" OnClick="btnAddField_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        </asp:Panel>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label37" runat="server" Text="Fields grid"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="border:solid 1px black;background-color:#E7F0FB;padding:3px">
                                                        <asp:Panel ID="pnlShowFieldsGrid" runat="server" Width="100%">
                                                        <asp:GridView ID="gFields" runat="server" AutoGenerateColumns="False" AllowPaging="true" DataKeyNames="Id" GridLines="None" OnRowDataBound="G_ItemDataBound" EmptyDataText="No records to display" ForeColor="#333333" OnRowCommand="gFields_ItemCommand" OnPageIndexChanged="G_PageIndexChanged" OnPageIndexChanging="G_PageIndexChanging"  EnableViewState="False" SkinID="TotalGrid" PageSize="15" >
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Description">
                                                                    <ItemTemplate>                                                                                            
                                                                        <asp:Label ID="lblfieldName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ObjectName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" Width="100%"/>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Action">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="deleteobject" AlternateText="Delete field" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>
                                                                    </ItemTemplate> 
                                                                <HeaderStyle HorizontalAlign="Center" Width="40px"/>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>                                        
                                                            </Columns>
                                                        </asp:GridView>
                                                        </asp:Panel>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        </radTS:PageView>
                                        <radTS:PageView id="pvConditions" runat="server" >
                                        <div style="width:100%;padding:5px">
                                            <table style="width:99%" border="0" cellpadding="0" cellspacing="0" >
                                                <tr id="trAddNewCondition" runat="server"><td><asp:Label ID="Label41" runat="server" Text="Add new condition"></asp:Label></td></tr>
                                                <tr id="trCondition" runat="server">
                                                    <td>
                                                        <div style="border:solid 1px black;background-color:#E7F0FB;padding:3px">
                                                        <asp:Panel ID="pnlCondition" runat="server" Width="100%">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                            <tr>
                                                                <td style="width:150px;padding-right:3px" align="right">
                                                                    <asp:Label ID="Label17" runat="server" Text="Title:" SkinID="AdminLabel" Width="100%"></asp:Label>
                                                                </td>
                                                                <td style="width:300px">
                                                                    <asp:TextBox ID="tbTitle" runat="server" Text="" Width="95%" MaxLength="100" ></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblConditionTitleErr" runat="server" ForeColor="red"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr style="vertical-align:top">
                                                                <td style="width:150px;padding-right:3px" align="right">
                                                                    <asp:Label ID="Label18" runat="server" Text="Detail:" SkinID="AdminLabel"  Width="95%"></asp:Label>
                                                                </td>
                                                                <td style="width:300px">
                                                                    <asp:TextBox ID="tbDetail" runat="server" Text="" Width="95%" MaxLength="256" Rows="5" TextMode="MultiLine" ></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblConditionDetailErr" runat="server" ForeColor="red"></asp:Label>
                                                                </td>
                                                            </tr>                            
                                                            <tr>
                                                                <td style="width:150px;padding-right:3px" align="right">
                                                                    <asp:Label ID="Label19" runat="server" Text="Minimal authority level:" SkinID="AdminLabel"  Width="100%"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlRole" runat="server" SkinID="AdminSelect" AutoPostBack="False" Width="150px" ></asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblConditionRoleErr" runat="server" ForeColor="red"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr runat="server" id="trCondType" style="display:block">
                                                                <td style="width:150px;padding-right:3px" align="right">
                                                                    <asp:Label ID="Label21" runat="server" Text="Type:" SkinID="AdminLabel"  Width="100%"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlType" runat="server" SkinID="AdminSelect" AutoPostBack="False" Width="150px"  EnableViewState="true"></asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblConditionTypeErr" runat="server" ForeColor="red"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr runat="server" id="trCondCategory" style="display:block">
                                                                <td style="width:150px;padding-right:3px" align="right">
                                                                    <asp:Label ID="Label22" runat="server" Text="Category:" SkinID="AdminLabel"  Width="100%"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlConditionCategory" runat="server" SkinID="AdminSelect" AutoPostBack="False" Width="150px" EnableViewState="true" ></asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblConditionCategoryErr" runat="server" ForeColor="red"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width:150px;padding-right:3px" align="right">
                                                                    <asp:Label ID="Label20" runat="server" Text="Closing Instruction:" SkinID="AdminLabel"  Width="100%"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="cbClosingInstruction" runat="server" SkinID="AdminSelect"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>                                                            
                                                            <tr style="padding-top:5px">
                                                                <td>&nbsp;</td>
                                                                <td><asp:Button ID="btnSaveCondition" runat="server" Text="Save"  SkinID="AdminButton" OnClick="btnSaveCondition_Click" CausesValidation="False"/></td>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                        </table> 
                                                        </asp:Panel>
                                                        </div>                                                   
                                                    </td>
                                                </tr>
                                                <tr><td><asp:Label ID="Label40" runat="server" Text="Conditions grid"></asp:Label></td></tr>
                                                <tr>
                                                    <td>
                                                    <div style="border:solid 1px black;background-color:#E7F0FB;padding:3px">
                                                    <asp:Panel ID="pnlConditionsGrid" runat="server" Width="100%">
                                                    <asp:GridView ID="gConditions" runat="server" AutoGenerateColumns="False" AllowPaging="true" DataKeyNames="Id" GridLines="None" OnRowDataBound="G_ItemDataBound" EmptyDataText="No records to display" ForeColor="#333333" OnRowCommand="gConditions_ItemCommand" OnPageIndexChanged="G_PageIndexChanged" OnPageIndexChanging="G_PageIndexChanging"  EnableViewState="False" SkinID="TotalGrid" >
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Title">
                                                                <ItemTemplate>                                                                                            
                                                                        <asp:Label ID="lblConditionTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" Width="150px"/>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Detail">
                                                                <ItemTemplate>                                                                                            
                                                                        <asp:Label ID="lblConditionDetail" runat="server" Text='<%# GetConditionDetails(Container.DataItem,100) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" Width="320px"/>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Role">
                                                                <ItemTemplate>                                                                                            
                                                                        <asp:Label ID="lblfieldName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RoleName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" Width="120px"/>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Type">
                                                                <ItemTemplate>                                                                                            
                                                                        <asp:Label ID="lblType1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TypeName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Category">
                                                                <ItemTemplate>                                                                                            
                                                                        <asp:Label ID="lblCategory1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Closing Instruction">
                                                                <ItemTemplate>                                                                                            
                                                                        <asp:Checkbox ID="lblClosingInstruction" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsClosingInstruction") %>' Enabled="false"></asp:Checkbox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" Width="120px"/>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>                                                            
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton  id="btnEdit" ImageUrl="~/images/btn_grd_edit.gif" CommandName="editcondition" AlternateText="Edit condition" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />                                                                
                                                                    <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="deleteobject" AlternateText="Delete item" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>
                                                                </ItemTemplate> 
                                                                <HeaderStyle HorizontalAlign="Center" Width="40px"/>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>                                        
                                                        </Columns>                                                    
                                                    </asp:GridView>
                                                    </asp:Panel>
                                                    </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        </radTS:PageView>                                      
                                        <radTS:PageView id="pvDocuments" runat="server" >
                                            <div style="width:100%;padding:5px">
                                            <table style="width:99%" border="0" cellpadding="0" cellspacing="0" >
                                                <tr id="trAddNewDocument" runat="server">
                                                    <td><asp:Label ID="Label36" runat="server" Text="Add new document"></asp:Label></td>
                                                </tr>
                                                <tr id="trDocument" runat="server">
                                                    <td>
                                                        <div style="border:solid 1px black;background-color:#E7F0FB;padding:3px">
                                                            <asp:Panel ID="pnlDocument" runat="server" Width="100%">
                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td style="width:110px;padding-right:3px" align="right">
                                                                            <asp:Label ID="Label49" runat="server" Text="Select document:" SkinID="AdminLabel"></asp:Label>
                                                                        </td>
                                                                        <td style="width:400px">
                                                                            <asp:HiddenField ID="hdnSelectDoc" runat="server" />
                                                                            <asp:DropDownList ID="ddlSelectDoc" Width="95%" runat="server" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="ddlSelectDoc_SelectedIndexChanged" />
                                                                            &nbsp;<asp:Label ID="lblSelectDocErr" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                                        </td>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                    <tr>            
                                                                        <td style="width:110px">&nbsp;</td>                                                        
                                                                        <td colspan="2">
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                                                <tr>                                                                                
                                                                                    <td style="width:150px" align="left">
                                                                                        <asp:CheckBox ID="cbAppPackage" runat="server" Text="Application Package" />
                                                                                    </td>
                                                                                    <td style="width:120px">
                                                                                        <asp:CheckBox ID="cbClosPackage" runat="server" Text="Closing Package" />
                                                                                    </td>
                                                                                    <td style="width:110px">
                                                                                        <asp:CheckBox ID="cbMiscPackage" runat="server" Text="Misc. Package" />
                                                                                    </td>
                                                                                    <td valign="bottom"><asp:Label ID="lblPackageErr" runat="server" Text="" ForeColor="Red"></asp:Label></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:110px">&nbsp;</td>
                                                                        <td style="padding-left:325px"><asp:Button ID="btnSaveDocument" runat="server" CausesValidation="false" Text="Save" SkinID="AdminButton" OnClick="btnSaveDocument_Click" /></td>
                                                                        <td>&nbsp;</td>                                                                    
                                                                    </tr>                                                                
                                                                </table>
                                                            </asp:Panel>
                                                        </div>                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><asp:Label ID="Label59" runat="server" Text="Document grid"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                        <td>
                                                            <div style="border:solid 1px black;background-color:#E7F0FB;padding:3px">
                                                            <asp:Panel ID="pnlDocumentGrid" runat="server" Width="100%">
                                                                <asp:GridView ID="gDocuments" runat="server" AutoGenerateColumns="False" AllowPaging="true" DataKeyNames="Id" GridLines="None" OnRowDataBound="G_ItemDataBound" EmptyDataText="No records to display" ForeColor="#333333" OnRowCommand="gDocuments_ItemCommand" OnPageIndexChanged="G_PageIndexChanged" OnPageIndexChanging="G_PageIndexChanging" EnableViewState="True" SkinID="TotalGrid" >
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Document Name">
                                                                        <ItemTemplate>                                                                                            
                                                                                <asp:Label ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" Width="300px"/>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="App. Pack.">
                                                                        <ItemTemplate>
                                                                                <asp:CheckBox ID="cbAppPackage" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsAppPackage") %>'  Enabled="false"/>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" Width="80px"/>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Cls. Pack.">
                                                                        <ItemTemplate>                                                                                            
                                                                            <asp:CheckBox ID="cbClsPackage" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsClosingPackage") %>'  Enabled="false"/>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" Width="80px"/>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Misc. Pack.">
                                                                        <ItemTemplate>                                                                                            
                                                                            <asp:CheckBox ID="cbMscPackage" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsMiscPackage") %>' Enabled="false"/>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" Width="80px"/>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Group">
                                                                        <ItemTemplate>                                                                                            
                                                                            <asp:Label ID="lblDocGroup" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "GroupName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" Width="50px"/>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Action">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton id="btnEdit" ImageUrl="~/images/btn_grd_edit.gif" CausesValidation="false" CommandName="editdocument" AlternateText="Edit document" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                                                            <asp:ImageButton id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CausesValidation="false" CommandName="deleteobject" AlternateText="Delete item" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>
                                                                        </ItemTemplate> 
                                                                        <HeaderStyle HorizontalAlign="Center" Width="40px"/>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>                                        
                                                                </Columns>
                                                            </asp:GridView>
                                                            </asp:Panel>
                                                            </div>
                                                        </td>
                                                    </tr>
                                            </table>
                                            </div>                                        
                                        </radTS:PageView>
                                        <radTS:PageView id="pvCheckLists" runat="server" >
                                            <div style="width:100%;padding:5px">
                                                <table style="width:99%" border="0" cellpadding="0" cellspacing="0" >
                                                    <tr id="trAddNewChecklist" runat="server"><td><asp:Label ID="Label43" runat="server" Text="Add new checklist"></asp:Label></td></tr>
                                                    <tr id="trChecklist" runat="server">
                                                        <td>
                                                            <div style="border:solid 1px black;background-color:#E7F0FB;padding:3px">
                                                            <asp:Panel ID="pnlCheckList" runat="server" Width="100%">
                                                            <table style="width:99%" border="0" cellpadding="0" cellspacing="0" >
                                                                <tr>
                                                                    <td style="width:105px;padding-right:5px;" align="right">
                                                                        <asp:Label ID="Label6" runat="server" Text="Title" SkinID="AdminLabel"></asp:Label>
                                                                    </td>
                                                                    <td style="width:280px">
                                                                        <asp:TextBox ID="tbclTitle" runat="server" MaxLength="256" Width="90%"/>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbclTitle"
                                                                            ErrorMessage="*"></asp:RequiredFieldValidator></td>
                                                                    <td colspan="4">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:105px">
                                                                        <asp:Label ID="Label26" runat="server" Text="Status" SkinID="AdminLabel"></asp:Label>
                                                                    </td>
                                                                    <td style="width:280px">
                                                                        <asp:Label ID="Label27" runat="server" Text="Question" SkinID="AdminLabel"></asp:Label>
                                                                    </td>
                                                                    <td style="width:50px" align="center">
                                                                        <asp:Label ID="Label28" runat="server" Text="Yes" SkinID="AdminLabel"></asp:Label>
                                                                    </td>
                                                                    <td style="width:50px" align="center">
                                                                        <asp:Label ID="Label29" runat="server" Text="No" SkinID="AdminLabel"></asp:Label>
                                                                    </td>
                                                                    <td style="width:50px" align="center">
                                                                        <asp:Label ID="Label30" runat="server" Text="Don't know" SkinID="AdminLabel"></asp:Label>
                                                                    </td>                
                                                                    <td style="width:50px" align="center">
                                                                        <asp:Label ID="Label31" runat="server" Text="To follow" SkinID="AdminLabel"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <asp:Repeater ID="rpChecklist" runat="server" OnItemDataBound="rpChecklist_ItemDataBound" EnableViewState="true">
                                                                <ItemTemplate>
                                                                <tr>
                                                                    <td style="width:105px">
                                                                        <asp:CheckBox ID="cbStatus" statusid='<%# DataBinder.Eval(Container.DataItem,"id") %>' runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"Selected") %>' Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'  />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="tbQuestion" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Question") %>' Width="90%" MaxLength="256" ></asp:TextBox>
                                                                        <asp:Label ID="lblerrq" runat="server" Text="" Font-Bold="true" ForeColor="red"></asp:Label>
                                                                    </td>
                                                                    <td style="width:50px" align="center">
                                                                        <asp:CheckBox ID="cbYes" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"cbYes") %>'  />
                                                                    </td>
                                                                    <td style="width:50px" align="center">
                                                                        <asp:CheckBox ID="cbNo" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"cbNo") %>'  />
                                                                    </td>
                                                                    <td style="width:50px" align="center">
                                                                        <asp:CheckBox ID="cbDontKnow" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"cbDontKnow") %>'  />
                                                                    </td>
                                                                    <td style="width:50px" align="center">
                                                                        <asp:CheckBox ID="cbToFollow" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"cbtoFollow") %>'  />
                                                                        <asp:Label ID="lblerrcb" runat="server" Text="" Font-Bold="true" ForeColor="red"></asp:Label>
                                                                    </td>                
                                                                </tr>
                                                                </ItemTemplate>
                                                                </asp:Repeater>
                                                                <tr>            
                                                                    <td>&nbsp;</td>
                                                                    <td>
                                                                        <asp:Button ID="btnSaveCheckList" runat="server" Text="Save"  SkinID="AdminButton" OnClick="btnSaveCheckList_Click"/>
                                                                    </td>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>                                                                                                                                
                                                            </table>
                                                            </asp:Panel>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:Label ID="Label44" runat="server" Text="Checklist grid"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="border:solid 1px black;background-color:#E7F0FB;padding:3px">
                                                            <asp:Panel ID="pnlCheckListsGrid" runat="server" Width="100%">
                                                            <asp:GridView ID="gCheckList" runat="server" AutoGenerateColumns="False" AllowPaging="true" DataKeyNames="Id" GridLines="None" OnRowDataBound="G_ItemDataBound" EmptyDataText="No records to display" ForeColor="#333333" OnRowCommand="gCheckList_ItemCommand" OnPageIndexChanged="G_PageIndexChanged" OnPageIndexChanging="G_PageIndexChanging"  EnableViewState="False" SkinID="TotalGrid" >
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Checklist">
                                                                        <ItemTemplate>                                                                                            
                                                                                <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Title") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" Width="400px"/>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Action">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton  id="btnEdit" ImageUrl="~/images/btn_grd_edit.gif" CommandName="editchecklist" AlternateText="Edit checklist" runat="server"  CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' CausesValidation="false" />
                                                                            <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="deleteobject" AlternateText="Delete item" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' CausesValidation="false"/>
                                                                        </ItemTemplate> 
                                                                        <HeaderStyle HorizontalAlign="Center" Width="40px"/>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            </asp:Panel>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </radTS:PageView>    
                                        <radTS:PageView id="pvAlerts" runat="server" >
                                            <div style="width:100%;padding:5px">
                                                <table style="width:99%" border="0" cellpadding="0" cellspacing="0" >
                                                    <tr id="trAddNewAlert" runat="server"><td><asp:Label ID="Label45" runat="server" Text="Add new alert"></asp:Label></td></tr>
                                                    <tr id="trAlert" runat="server">
                                                        <td>
                                                            <div style="border:solid 1px black;background-color:#E7F0FB;padding:3px">
                                                            <asp:Panel ID="pnlAlert" runat="server" Width="100%">                                                        
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td style="width:70px; padding-right:3px" align="right">
                                                                        <asp:Label ID="Label32" runat="server" Text="Message:" SkinID="AdminLabel" ></asp:Label>
                                                                    </td>
                                                                    <td style="width:300px">
                                                                        <asp:TextBox ID="tbMessage" runat="server" Text="" Width="315px" MaxLength="256" ></asp:TextBox>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblAlertMessageErr" runat="server" ForeColor="Red"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr style="padding-top:5px">
                                                                    <td>&nbsp;</td>
                                                                    <td align="left">
                                                                        <asp:Button ID="btnSaveAlert" runat="server" Text="Save"  SkinID="AdminButton" OnClick="btnSaveAlert_Click" CausesValidation="False"/>
                                                                    </td>
                                                                    <td>&nbsp;</td>            
                                                                </tr>
                                                            </table>
                                                            </asp:Panel>
                                                            </div>    
                                                        </td>
                                                    </tr>
                                                    <tr><td><asp:Label ID="Label46" runat="server" Text="Alert grid"></asp:Label></td></tr>
                                                    <tr>
                                                        <td>
                                                            <div style="border:solid 1px black;background-color:#E7F0FB;padding:3px">
                                                            <asp:Panel ID="pnlAlertsGrid" runat="server" Width="100%">
                                                            <asp:GridView ID="gAlert" runat="server" AutoGenerateColumns="False" AllowPaging="true" DataKeyNames="Id" GridLines="None" OnRowDataBound="G_ItemDataBound" EmptyDataText="No records to display" ForeColor="#333333" OnRowCommand="gAlert_ItemCommand" OnPageIndexChanged="G_PageIndexChanged" OnPageIndexChanging="G_PageIndexChanging"  EnableViewState="False" SkinID="TotalGrid" >
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Message">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Message") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" Width="500px"/>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Action">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton  id="btnEdit" ImageUrl="~/images/btn_grd_edit.gif" CommandName="editalert" AlternateText="Edit alert" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />                                                                
                                                                            <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="deleteobject" AlternateText="Delete item" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>
                                                                        </ItemTemplate> 
                                                                        <HeaderStyle HorizontalAlign="Center" Width="40px"/>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            </asp:Panel>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </radTS:PageView>
                                        <radTS:PageView id="pvData" runat="server" >
                                            <div style="width:100%;padding:5px">
                                                <table style="width:99%" border="0" cellpadding="0" cellspacing="0" >
                                                    <tr id="trAddNewData" runat="server"><td><asp:Label ID="Label47" runat="server" Text="Add new data"></asp:Label></td></tr>
                                                    <tr id="trData" runat="server">
                                                        <td>
                                                            <div style="border:solid 1px black;background-color:#E7F0FB;padding:3px">
                                                            <asp:Panel ID="pnlData" runat="server" Width="100%">
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td style="width:80px;padding-right:3px" align="right">
                                                                        <asp:Label ID="Label34" runat="server" Text="Select group" SkinID="AdminLabel"></asp:Label>
                                                                    </td>                                                                    
                                                                    <td style="width:400px">
                                                                        <asp:DropDownList ID="ddlDataGroup" runat="server" Width="100%" AutoPostBack="True" ></asp:DropDownList>
                                                                    </td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:80px;padding-right:3px" align="right">
                                                                        <asp:Label ID="Label35" runat="server" Text="Select field" SkinID="AdminLabel"></asp:Label>
                                                                    </td>
                                                                    <td style="width:400px">
                                                                        <asp:DropDownList ID="ddlDataField" runat="server" Width="100%" AutoPostBack="True"></asp:DropDownList>
                                                                    </td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:80px;padding-left:5px" align="left">
                                                                        <asp:Label ID="lblDataValue" runat="server" Text="Enter value" SkinID="AdminLabel" Width="100%" ></asp:Label>
                                                                    </td>                
                                                                    <td style="width:400px">
                                                                        <asp:TextBox ID="tbDataValue" runat="server" Width="394px" MaxLength="256"></asp:TextBox>
                                                                        <asp:DropDownList ID="ddlDataDictionary"  Width="397px" runat="server" Visible="false"></asp:DropDownList>
                                                                        <radCln:RadDatePicker ID="rdpData" runat="server" Width="80px" FocusedDate="2099-12-31" MinDate="1800-01-01" Visible="false" Height="20px" SkinID="WebBlue">
                                                                            <DatePopupButton Visible="False" />
                                                                            <DateInput onclick="RadDatadpValuePopup()" SkinID="WebBlue" Height="20px" Width="80px" />
                                                                        </radCln:RadDatePicker>                                                                        
                                                                        <radi:radmaskedtextbox id="rmeData" runat="server" displaymask="#########.##" displaypromptchar=" " Width="120px" DisplayFormatPosition="Right" Mask="#########.##" Visible="false"></radi:radmaskedtextbox>
                                                                    </td>                
                                                                    <td><asp:Label ID="lblDataErr" runat="server" ForeColor="Red"></asp:Label></td>                                                                
                                                                </tr>
                                                                <tr style="height:10px"><td colspan="7">&nbsp;</td></tr> 
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Button ID="btnBackData" runat="server" Text="Back" SkinID="AdminButton" CausesValidation="False" OnClick="btnBackData_Click" />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <asp:Button ID="btnAddData" runat="server" Text="Add" SkinID="AdminButton" OnClick="btnAddData_Click"/>
                                                                                </td>
                                                                            </tr>
                                                                        </table>            
                                                                    </td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                            </table>
                                                            </asp:Panel>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr><td><asp:Label ID="Label48" runat="server" Text="Data grid"></asp:Label></td></tr>
                                                    <tr>
                                                        <td>
                                                            <div style="border:solid 1px black;background-color:#E7F0FB;padding:3px">
                                                            <asp:Panel ID="pnlDataGrid" runat="server" Width="100%">
                                                            <asp:GridView ID="gData" runat="server" AutoGenerateColumns="False" AllowPaging="true" DataKeyNames="Id" GridLines="None" OnRowDataBound="G_ItemDataBound" EmptyDataText="No records to display" ForeColor="#333333" OnRowCommand="gData_ItemCommand" OnPageIndexChanged="G_PageIndexChanged" OnPageIndexChanging="G_PageIndexChanging"  EnableViewState="False" SkinID="TotalGrid" >
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Field">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblField" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FieldName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" Width="250px"/>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Value">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFieldValue" runat="server" Text='<%# GetFieldValue(Container.DataItem) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" Width="100px"/>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Action">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="deleteobject" AlternateText="Delete item" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>
                                                                        </ItemTemplate> 
                                                                        <HeaderStyle HorizontalAlign="Center" Width="40px"/>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            </asp:Panel>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </radTS:PageView>
                                    </radTS:RadMultiPage>                                                                        
                                </div>
                            </td>
                        </tr>
                    </table>
                    </div>
                    </asp:Panel>
                </radspl:radpane>   
            </radspl:radsplitter>           
        </td>
    </tr>
</table>
<rada:RadAjaxManager id="RadAjaxManager1" runat="server" EnableOutsideScripts="True" >
    <ClientEvents OnRequestStart="RequestStart" OnResponseEnd="OnResponceEnd"/>
    <AjaxSettings>
        <radA:AjaxSetting AjaxControlID="rtvRule">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlEditRule"/>
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="pnlEditor">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlEditor" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="pnlExpressionGrid">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="lblRuleName" />
                <radA:AjaxUpdatedControl ControlID="lblRuleExpression" />
                <radA:AjaxUpdatedControl ControlID="pvGeneral" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="btnAddExpression">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pvGeneral" />
                <radA:AjaxUpdatedControl ControlID="lblRuleName" />
                <radA:AjaxUpdatedControl ControlID="lblRuleExpression" />
            </UpdatedControls>
        </radA:AjaxSetting>        
        <radA:AjaxSetting AjaxControlID="btnSaveProperties">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pvGeneral" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="pnlDdlFields">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlDdlFields" />
            </UpdatedControls>
        </radA:AjaxSetting>     
        <radA:AjaxSetting AjaxControlID="btnAddField">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlDdlFields" />
                <radA:AjaxUpdatedControl ControlID="pnlShowFieldsGrid" />                
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="pnlShowFieldsGrid">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlShowFieldsGrid" />
            </UpdatedControls>
        </radA:AjaxSetting>        
        <radA:AjaxSetting AjaxControlID="btnSaveCondition">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlCondition" />            
            </UpdatedControls>
        </radA:AjaxSetting>        
        <radA:AjaxSetting AjaxControlID="pnlConditionsGrid">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlConditionsGrid" />
            </UpdatedControls>
        </radA:AjaxSetting>        
        <radA:AjaxSetting AjaxControlID="btnSaveTask">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlTask" />            
            </UpdatedControls>
        </radA:AjaxSetting>        
        <radA:AjaxSetting AjaxControlID="pnlTasksGrid">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlTasksGrid" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="btnSaveCheckList">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlCheckList" />            
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="pnlCheckListsGrid">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlCheckListsGrid" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="btnSaveAlert">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlAlert" />            
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="pnlAlertsGrid">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlAlertsGrid" />
            </UpdatedControls>
        </radA:AjaxSetting> 
        <radA:AjaxSetting AjaxControlID="pnlData">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlData" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="pnlDataGrid">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlDataGrid" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="pnlDocument">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlDocument" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="gCheckList">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="gCheckList" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="btnSaveDocument">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlDocument" />
                <radA:AjaxUpdatedControl ControlID="pnlDocumentGrid" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="btnEdit">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="btnEdit" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="btnDelete">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="btnDelete" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="ddlSelectDoc">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="ddlSelectDoc" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="pnlDocumentGrid">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlDocumentGrid" />
                <radA:AjaxUpdatedControl ControlID="pnlDocument" />
                <radA:AjaxUpdatedControl ControlID="pnlEditRule" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="ddlCategory">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="rtvRule" />
                <radA:AjaxUpdatedControl ControlID="pnlEditRule" />                
            </UpdatedControls>
        </radA:AjaxSetting>        
        <radA:AjaxSetting AjaxControlID="btnCancelProperties">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlEditRule" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="pnlEditRule">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="pnlEditRule" />
            </UpdatedControls>
        </radA:AjaxSetting>
        <radA:AjaxSetting AjaxControlID="gDocuments">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="gDocuments" />
                <radA:AjaxUpdatedControl ControlID="pnlDocument" />
            </UpdatedControls>
        </radA:AjaxSetting>
    </AjaxSettings>    
</rada:RadAjaxManager>

<radA:AjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server" Height="75px" Width="75px" style="position:absolute;" IsSticky="false" InitialDelayTime="1000" MinDisplayTime="1000">
    <asp:Image ID="Image2" runat="server" AlternateText="Loading..." ImageUrl="~/RadControls/Ajax/Skins/Default/Loading.gif" BorderStyle="Solid" BorderWidth="1px"/>
 </radA:AjaxLoadingPanel>
<div runat="server" id="divloading" style="position:absolute;left:400px;top:100px;" ></div>
<input type="hidden" id="currentruletab" runat="server"/>

