<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailAdd.ascx.cs" Inherits="LoanStarPortal.Controls.EmailAdd" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>
<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal"
    Width="100%" BorderWidth="0" BorderStyle="None" BorderSize="0" Skin="Default"
    LiveResize="false">
    <radspl:RadPane ID="TopPane" runat="server" Height="100%" Scrolling="Both">
    </radspl:RadPane>
</radspl:RadSplitter>
<script type="text/javascript">
function BackToFollowUp(){
    AjaxNS.AR('Tabs$RadTabStrip1','Tabs$RadTabStrip1$tabConditions', 'RadAjaxManager1', event);
}
</script>
