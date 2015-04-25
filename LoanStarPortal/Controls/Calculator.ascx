<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Calculator.ascx.cs" Inherits="LoanStarPortal.Controls.Calculator" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>
<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Width="100%" BorderWidth="0" BorderStyle="None" BorderSize="0" Skin="Default" LiveResize="false">
   <radspl:radpane id="TopPane" runat="server" Height="29px" Scrolling="None">
       <div RadResizeStopLookup="true" RadShowStopLookup="true"  style="padding:5px;">
        <radTS:RadTabStrip ID="rtsCalculators" runat="server" Skin="Outlook" MultiPageID="rmpCalculator" SelectedIndex="0" CausesValidation="false" Orientation="HorizontalTopToBottom"><%--OnTabClick="TabsMortgageProfiles_TabClick" AutoPostBack="false" --%>
            <Tabs>
                <%--<radts:Tab Text="Lead Calculator" Value="LeadCalculator" ID="leadCalc"></radts:Tab>        --%>
                <radts:Tab Text="Advanced Calculator" Value="AdvancedCalculator" ID="advCalc"></radts:Tab>        
            </Tabs>
        </radTS:RadTabStrip>
       </div>
   </radspl:radpane>
   <radspl:radsplitbar id="RadSplitBar3" runat="server" CollapseMode="None" EnableResize="false" Visible="false"/>
   <radspl:radpane id="MiddlePane" runat="server" scrolling="Y" >
    <div RadResizeStopLookup="true" RadShowStopLookup="true" >
    <radTS:RadMultiPage ID="rmpCalculator" runat="server"  SelectedIndex="0" Height="85%" EnableViewState="true">
    <%--<radTS:pageview id="pvLeadCalc" runat="server" EnableViewState="true"></radTS:pageview>--%>
    <radTS:pageview id="pvAdvCalc" runat="server" EnableViewState="true"></radTS:pageview>
</radTS:RadMultiPage>
    </div>
   </radspl:radpane>
</radspl:RadSplitter>