<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MortgageProfiles.ascx.cs" Inherits="LoanStarPortal.Controls.MortgageProfiles" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>

<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="95%" Orientation="Horizontal" Width="100%" BorderWidth="0" BorderStyle="None" BorderSize="0" Skin="Default" LiveResize="false">
   <radspl:radpane id="TopPane" runat="server" Height="29px" Scrolling="None">
   <div RadResizeStopLookup="true" RadShowStopLookup="true" >
    <div class="paneTitle">
    <table border="0" cellspacing="0" cellpadding="0" style="width:100%;vertical-align:top;">
        <tr>
            <td class="title" nowrap width="20%"><asp:Literal ID="lblMortgageName" runat="server"/>&nbsp;</td>
            <td class="title" nowrap width="35%"><asp:Literal ID="lblCity" runat="server"/><asp:Literal ID="lblCounty" runat="server" /><asp:Label ID="lblState" runat="server"></asp:Label>&nbsp;</td>
            <td class="title" nowrap width="20%"><asp:Literal ID="lblProduct" runat="server"/>&nbsp;</td>
            <td class="title" align="right" width="25%"><asp:Literal ID="lblProfileStatus" runat="server"/>&nbsp;&nbsp;</td>
            <td align="right">&nbsp;</td>
        </tr>
    </table>
    </div>
    </div>
   </radspl:radpane>  
   <radspl:radsplitbar id="RadSplitBar3" runat="server" CollapseMode="None" EnableResize="false" Visible="false"/>
   <radspl:radpane id="Radpane1" runat="server" Height="22px">
    <div RadResizeStopLookup="true" RadShowStopLookup="true" >
    <radTS:RadTabStrip id="TabsMortgageProfiles" runat="server" Skin="Outlook" MultiPageID="RadMultiPage1" SelectedIndex="0" CausesValidation="false" Orientation="HorizontalTopToBottom" OnTabClick="TabsMortgageProfiles_TabClick" AutoPostBack="true">
        <Tabs>
            <radts:Tab Text="Borrower" Value="Borrower" ID="tabBorrower"></radts:Tab>        
            <radts:Tab Text="Property" Value="Property" ID="tabProperty"></radts:Tab>
            <radts:Tab Text="Mortgage" Value="Mortgage" ID="tabMortgage"></radts:Tab>
            <radts:Tab Text="" Value="CorresponderLender" ID="tabLender"></radts:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    </div>
   </radspl:radpane>
   <radspl:radpane id="MiddlePane" runat="server" scrolling="Y" >
   <div RadResizeStopLookup="true" RadShowStopLookup="true" >
    <radTS:RadMultiPage id="RadMultiPage1" runat="server" SelectedIndex="0" Height="85%" EnableViewState="False">
        <radTS:PageView id="pvBorrower" runat="server" OnPreRender="pvPreRender">
        </radTS:PageView>
        <radTS:PageView id="pvProperty" runat="server" EnableViewState="False" OnPreRender="pvPreRender">
        </radTS:PageView>
        <radTS:PageView id="pvMortgage" runat="server" EnableViewState="False" OnPreRender="pvPreRender">
        </radTS:PageView>    
        <radTS:PageView id="pvLender" runat="server" EnableViewState="False" OnPreRender="pvPreRender">
        </radTS:PageView> 
    </radTS:RadMultiPage>
    </div>
   </radspl:radpane>
   <radspl:radsplitbar id="RadSplitBar1" runat="server" CollapseMode="None" EnableResize="false" Visible="false"/>
    <radspl:RadPane ID="BottomPane" runat="server" CssClass="bottomPane" Height="30px"
        Scrolling="None">&nbsp;
    </radspl:radpane>  
</radspl:radsplitter>          
