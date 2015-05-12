<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tabs.ascx.cs" Inherits="LoanStarPortal.Controls.Tabs" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<radTS:RadMultiPage id="RadMultiPage1" runat="server" Height="96%" RenderSelectedPageOnly="true">
    <radTS:PageView id="PageviewInfo" CssClass="tab_container" runat="server"></radTS:PageView>
    <radTS:PageView id="PageviewCalculator" CssClass="tab_container" runat="server"></radTS:PageView>
    <radTS:PageView id="PageviewCheckLists" CssClass="tab_container" runat="server"></radTS:PageView>
    <radTS:PageView id="PageviewConditions" CssClass="tab_container" runat="server"></radTS:PageView>
</radTS:RadMultiPage>
<radTS:RadTabStrip id="RadTabStrip1" runat="server" Skin="ClassicBlue" MultiPageID="RadMultiPage1" CausesValidation="false" style="position:absolute;bottom:37px;left:210px" AutoPostBack="true">
    <Tabs>
        <radts:Tab Text="Loan Info" Value="Info" ID="tabLoanInfo" runat="server"></radts:Tab>
        <radts:Tab Text="Calculator" Value="Calculator" ID="tabCalculator" runat="server"></radts:Tab>
        <radts:Tab Text="Checklists" Value="Checklists" ID="tabChecklists" runat="server"></radts:Tab> 
        <radts:Tab Text="Conditions" Value="Conditions" ID="tabConditions" runat="server"></radts:Tab> 
    </Tabs>
</radTS:RadTabStrip>