<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminMenu.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.AdminMenu" %>
<%@ Register Assembly="RadToolbar.Net2" Namespace="Telerik.WebControls" TagPrefix="radTlb" %>
<table border="0" cellpadding="0" cellspacing="0" width="99%">
    <tr>
        <td>
            <radTlb:RadToolbar ID="RadToolbar1" Orientation="Vertical" runat="server" OnOnClick="Toolbar1_OnClick" AutoPostBack="True" Skin="Outlook" DisplayEnds="False" CausesValidation="False" ButtonWidth="100%">
            </radTlb:RadToolbar>
        </td>
    </tr>
</table>
<script type="text/javascript">
<%= RadToolbar1.ClientID %>.attachEvent("OnClientClick","MenuItemClickHandler");
function MenuItemClickHandler(sender, eventArgs){
    if(sender.CommandName=='Help'){
            ShowHelp();
            return false;
    }
    return true;
}
function ShowHelp(){
    var url = '<%=HelpUrl %>';
    window.open(url);
}
</script>


