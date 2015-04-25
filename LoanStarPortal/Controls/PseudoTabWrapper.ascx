<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PseudoTabWrapper.ascx.cs" Inherits="LoanStarPortal.Controls.PseudoTabWrapper" %>
<table border="0" cellpadding="0" cellspacing="0" align="center" width="97%" class="kontrTab">
<tr>
	<td colspan="3">
		<table border="0" cellpadding="0" cellspacing="0" align="center" width="100%">
		<tr>
			<td><img src="/images/bg_kontrTab_title_left.gif" alt="" /></td>
			<td class="centerTd_title" align="left">
				<table border="0" cellpadding="0" cellspacing="0" align="left">
				<tr>
					<td class="centerTd_title_text">
                        <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
                    </td>
					<td><img src="/images/bg_kontrTab_title_tt_right.gif" alt="" /></td>
				</tr>
				</table>
			</td>
			<td><img src="/images/bg_kontrTab_title_right.gif" alt="" /></td>
		</tr>
		</table>
	</td>
</tr>
<tr>
	<td class="bg_color"><img src="/images/bg_kontrTab_topleft.gif" alt="" /></td>
	<td class="bg_kontrTab_top"></td>
	<td class="bg_color"><img src="/images/bg_kontrTab_topright.gif" alt="" /></td>
</tr>
<tr>
	<td class="bg_kontrTab_left"></td>
	<td class="content_kontrTab">
	    <asp:Panel ID="pn" runat="server"></asp:Panel>
	</td>
	<td class="bg_kontrTab_right"></td>
</tr>
<tr>
	<td><img src="images/bg_kontrTab_botleft.gif" alt="" /></td>
	<td class="bg_kontrTab_bottom"></td>
	<td><img src="images/bg_kontrTab_botright.gif" alt="" /></td>
</tr>
</table>


