<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="LoanStarPortal.Controls.Header" %>
<table border="0" cellspacing="0" cellpadding="0" width="100%" style="margin: 0 0 0 0;">
    <tr style="background-color: #C1D9F0; height: 25px;">
        <td style="width: 210px; height: 25px;"></td>
        <td align="left" class="logged" style="padding-left: 160px; height: 25px;">
            <a href="javascript:DisplayLinks('email')" id="rmMortgage_m1_m2" class="link" runat="server"><span class="text" runat="server" id="spEmail">Emails</span></a>
            &nbsp;&nbsp;<a href="javascript:DisplayLinks('calendar')" id="rmMortgage_m1_m10" class="link"><span class="text">Calendar</span></a>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span runat="server" id="divSearchLeads"><span class="text">Search leads:</span>
                &nbsp;&nbsp;<span><asp:TextBox runat="server" ID="tbSearchLeads" MaxLength="50" Width="100px"></asp:TextBox></span>
                <span>
                    <asp:Button ID="btnSearchLead" runat="server" Text="Go" CausesValidation="false" Width="30px" OnClick="btnSearchLead_Click" /></span>
            </span>
        </td>
        <td style="padding-right: 150px; height: 25px;" class="logged" align="left">&nbsp;<asp:Literal ID="lblLogged" runat="server" Text="User: " /></td>
       <%-- <td style="float:right">
            <a href="javascript:openLinks('notes')" class="link header-link" runat="server"><span class="text" runat="server">Notes</span></a>
            <a href="javascript:openLinks('documents')" class="link header-link" runat="server"><span class="text" runat="server">Documents</span></a>
            <a href="javascript:openLinks('conditions')" class="link header-link" runat="server"><span class="text" runat="server">Conditions</span></a>
        </td>--%>
        <td style="height: 25px; width: 100px;">&nbsp;</td>
    </tr>
</table>
<script type="text/javascript">
    function WebMailCallBack() {
    }
    WebMailCallBack.prototype = {
        SetUnreadEmails: function (n) {
            var el = document.getElementById('<%=spEmail.ClientID %>');
	    if (el != null) {
	        if (n > 0) {
	            el.innerHTML = 'Emails(' + n + ')';
	            el.className = 'text_red';
	        } else {
	            el.innerHTML = 'Emails';
	            el.className = 'text';
	        }
	    }
	}
}
	function DisplayLinks(displayMode) {
	    if (displayMode === "email") {
	        AjaxNS.AR('rmMortgage', 'EmailLinkClicked', 'RadAjaxManager1', event);
	    }
	    if (displayMode === "calendar") {
	        AjaxNS.AR('rmMortgage', 'rmMortgage$m1$m10', 'RadAjaxManager1', event);
	    }
	}

	function openLinks(type) {
	    AjaxNS.AR('rmMortgage', type, 'RadAjaxManager1', event);
	};
</script>
<input type="hidden" id="emailLinkId" value='<%=spEmail.ClientID %>' />