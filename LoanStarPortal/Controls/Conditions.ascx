<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Conditions.ascx.cs" Inherits="LoanStarPortal.Controls.Conditions" %>
<%@ Register Assembly="RadPanelbar.Net2" Namespace="Telerik.WebControls" TagPrefix="radP" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>

<script type="text/javascript">
    function CollapseAll()
    {
        var panelbar = <%= panelCondition.ClientID %>;
        
        for (var i = 0; i < panelbar.AllItems.length; i++)
        {
            panelbar.AllItems[i].Collapse();
        }
    }
    
    function ExpandAll()
    {
        var panelbar = <%= panelCondition.ClientID %>;
        
        for (var i = 0; i < panelbar.AllItems.length; i++)
        {
            panelbar.AllItems[i].Expand();
        }
    }
</script>

<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Width="100%" BorderWidth="0" BorderStyle="None" BorderSize="0" Skin="Default" LiveResize="false">
   <radspl:radpane id="TopPane" runat="server" Height="60px" Scrolling="None">
<div RadResizeStopLookup="true" RadShowStopLookup="true" >   
<div class="paneTitle"><b>Conditions</b></div>
    &nbsp;<asp:Image ID="imgExpandAll" runat="server" ImageUrl="~/Images/Expand-All.gif" />
    <a href="javascript:ExpandAll();">Expand All</a>
    &nbsp;<asp:Image ID="imgCollapseAll" runat="server" ImageUrl="~/Images/Collapse-All.gif" />
    <a href="javascript:CollapseAll();">Collapse All</a>
</div>
   </radspl:radpane>  
   <radspl:radsplitbar id="RadSplitBar3" runat="server" CollapseMode="None" EnableResize="false" Visible="false"/>
   <radspl:radpane id="MiddlePane" runat="server" scrolling="Y" >
<div RadResizeStopLookup="true" RadShowStopLookup="true" >
    <radP:RadPanelbar runat="server" ID="panelCondition" Skin="PipeLine" Width="100%" CausesValidation="False"
                     ExpandMode="MultipleExpandedItems" DataFieldID="ID" EnableViewState="False">
        <ItemTemplate>
        <asp:Panel ID="panelGeneral" runat="server" Visible="false">
            <table width="100%" >
                <tr>
                    <td width="10%">&nbsp;</td>
                    <td>
                        <asp:Label ID="lblDetails" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        </ItemTemplate>
    </radP:RadPanelbar>
</div>
    </radspl:radpane>
</radspl:radsplitter>    