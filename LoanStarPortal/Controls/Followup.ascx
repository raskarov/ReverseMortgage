<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Followup.ascx.cs" Inherits="LoanStarPortal.Controls.Followup" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>
<%@ Register Src="FollowupCampaign.ascx" TagName="FollowupCampaign" TagPrefix="uc3" %>
<%@ Register Src="FollowupConditions.ascx" TagName="FollowupConditions" TagPrefix="uc2" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Width="100%" BorderWidth="0" BorderStyle="None" Skin="Default">
<radspl:radpane id="TopPane" runat="server" Height="20px" Scrolling="None">
<radTS:RadTabStrip id="rtsFollowup" runat="server" MultiPageID="rmpFollowup" CausesValidation="false" Skin="Outlook" AutoPostBack="True" OnTabClick="rtsFollowup_TabClick" >
    <Tabs>
        <radts:Tab Text="Conditions" Value="Conditions" ID="tabConditions" runat="server"></radts:Tab>
        <radts:Tab Text="Campaigns" Value="Campaigns" ID="tabCampaigns" runat="server"></radts:Tab>
    </Tabs>
</radTS:RadTabStrip>
</radspl:radpane>
<radspl:radsplitbar id="RadSplitBar3" runat="server" CollapseMode="None" EnableResize="false" Visible="false"/>
<radspl:radpane id="Radpane1" runat="server"  Scrolling="Y">
<radTS:RadMultiPage id="rmpFollowup" runat="server" RenderSelectedPageOnly="true">
    <radTS:PageView id="pvConditions" runat="server">
        <uc2:FollowupConditions id="FollowupConditions1" runat="server"></uc2:FollowupConditions>    
    </radTS:PageView>
    <radTS:PageView id="pvCampaign" runat="server">
        <uc3:FollowupCampaign id="FollowupCampaign1" runat="server">
        </uc3:FollowupCampaign>
    </radTS:PageView>
</radTS:RadMultiPage>
</radspl:radpane>
</radspl:RadSplitter>
