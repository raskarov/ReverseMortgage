<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Reports.ascx.cs" Inherits="LoanStarPortal.Controls.Reports" %>

<div class="paneTitle">
    <b>Report</b>
</div>
<br />
<div style="padding-left:10px;">
<asp:LinkButton ID="lbtnPipeLineReport" runat="server" CssClass="linkButton" Text="Pipe Line Report" 
        CommandName="PipeLine" CommandArgument="" OnCommand="lbtnPipeLineReport_Command" >
</asp:LinkButton>
</div>

