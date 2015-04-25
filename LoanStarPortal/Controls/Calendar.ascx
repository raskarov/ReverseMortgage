<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Calendar.ascx.cs" Inherits="LoanStarPortal.Controls.Calendar" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>

<%--<script language="javascript">
    ResizePane('calendar');
</script>--%>

<radspl:RadSplitter ID="RadSplitter8" runat="server" Height="100%" Orientation="Horizontal"
    Width="100%" BorderWidth="0" BorderStyle="None" BorderSize="0" Skin="Default" LiveResize="true"  >
    <radspl:RadPane ID="TopPane8" runat="server" Height="100%" Width="100%" Scrolling="Both"  >
    </radspl:RadPane>
</radspl:RadSplitter>

