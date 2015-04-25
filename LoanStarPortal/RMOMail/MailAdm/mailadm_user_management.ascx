<%@ Control Language="c#" AutoEventWireup="True" Codebehind="mailadm_user_management.ascx.cs" Inherits="WebMailPro.MailAdm.mailadm_user_management" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<style>
    .headerLink{color:Black;white-space:nowrap;}
</style>
<table class="wm_admin_center_min" width="100%">
	<tr>
		<td>
			<asp:DataGrid Runat="server" Width="98%" AllowPaging="True" OnPageIndexChanged="ChangeGridPage" AllowCustomPaging="True" CssClass="wm_user_table" ID="usersDataGridID" AutoGenerateColumns="False" ShowHeader="true">
				<HeaderStyle HorizontalAlign="Center" Font-Bold="True" BorderStyle="Solid" ForeColor="Black" BackColor="Gainsboro"></HeaderStyle>
				<AlternatingItemStyle CssClass="even"></AlternatingItemStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="User ID">
						<HeaderTemplate>
						    <asp:LinkButton OnCommand="HeaderLinkClick" CommandArgument="id_acct" ID="lbl_id_acct" runat="server" Text="User Id" CssClass="headerLink" style="color:Black;white-space:nowrap;"> </asp:LinkButton>
						</HeaderTemplate>
						<ItemTemplate>
						    <%#DataBinder.Eval(Container.DataItem, "id_user")%> (<%#DataBinder.Eval(Container.DataItem, "id_acct")%>)
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Email">
						<HeaderTemplate>
						    <asp:LinkButton OnCommand="HeaderLinkClick" CommandArgument="email" ID="lbl_email" runat="server" Text="Email" CssClass="headerLink" style="color:Black;white-space:nowrap;"> </asp:LinkButton>
						</HeaderTemplate>
						<ItemTemplate>
					        <%#DataBinder.Eval(Container.DataItem, "email")%>
					    </ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Last Login">
						<HeaderTemplate>
						    <asp:LinkButton OnCommand="HeaderLinkClick" CommandArgument="last_login" ID="lbl_last_login" runat="server" Text="Last Login" CssClass="headerLink" style="color:Black;white-space:nowrap;"> </asp:LinkButton>
						</HeaderTemplate>
						<ItemTemplate>
						    <%#DataBinder.Eval(Container.DataItem, "last_login")%>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Logins">
						<HeaderTemplate>
						    <asp:LinkButton OnCommand="HeaderLinkClick" CommandArgument="logins_count" ID="lbl_logins_count" runat="server" Text="Logins" CssClass="headerLink" style="color:Black;white-space:nowrap;"> </asp:LinkButton>
						</HeaderTemplate>
						<ItemTemplate>
						    <%#DataBinder.Eval(Container.DataItem, "logins_count")%>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Incoming Server">
						<HeaderTemplate>
						    <asp:LinkButton OnCommand="HeaderLinkClick" CommandArgument="mail_inc_host" ID="lbl_mail_inc_host" runat="server" Text="Incoming Server" CssClass="headerLink" style="color:Black;white-space:nowrap;"> </asp:LinkButton>
						</HeaderTemplate>
						<ItemTemplate>
						    <%#DataBinder.Eval(Container.DataItem, "mail_inc_host")%>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Outgoing Server">
						<HeaderTemplate>
						    <asp:LinkButton OnCommand="HeaderLinkClick" CommandArgument="mail_out_host" ID="lbl_mail_out_host" runat="server" Text="Outgoing Server" CssClass="headerLink" style="color:Black;white-space:nowrap;"> </asp:LinkButton>
					    </HeaderTemplate>
						<ItemTemplate>
						    <%#DataBinder.Eval(Container.DataItem, "mail_out_host")%>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Mailbox Size">
						<HeaderTemplate>
						    <asp:LinkButton OnCommand="HeaderLinkClick" CommandArgument="mailbox_size" ID="lbl_mailbox_size" runat="server" Text="Mailbox Size" CssClass="headerLink" style="color:Black;white-space:nowrap;"> </asp:LinkButton>
						</HeaderTemplate>
						<ItemTemplate>
							<% if (_settings.EnableMailboxSizeLimit) { %>
							<div class="wm_progressbar" style="border: solid 1px #7E9BAF;" title="<%#GetProgressBarTitle(DataBinder.Eval(Container.DataItem, "mailbox_size"), DataBinder.Eval(Container.DataItem, "mailbox_limit"))%>">
								<div class="wm_progressbar_used" style="background:<%#GetUsedColor(CalculateUsedSpace(DataBinder.Eval(Container.DataItem, "mailbox_size"), DataBinder.Eval(Container.DataItem, "mailbox_limit")), CalculateUsedAllSpace(DataBinder.Eval(Container.DataItem, "mailbox_size"), DataBinder.Eval(Container.DataItem, "all_mailboxs_size"), DataBinder.Eval(Container.DataItem, "mailbox_limit")))%> width: <%#CalculateUsedSpace(DataBinder.Eval(Container.DataItem, "mailbox_size"), DataBinder.Eval(Container.DataItem, "mailbox_limit"))%>px;"></div>
								<div class="wm_progressbar_used" style="background:<%#GetUsedAllColor(CalculateUsedAllSpace(DataBinder.Eval(Container.DataItem, "mailbox_size"), DataBinder.Eval(Container.DataItem, "all_mailboxs_size"), DataBinder.Eval(Container.DataItem, "mailbox_limit")))%> width: <%#CalculateUsedAllSpace(DataBinder.Eval(Container.DataItem, "mailbox_size"), DataBinder.Eval(Container.DataItem, "all_mailboxs_size"), DataBinder.Eval(Container.DataItem, "mailbox_limit"))%>px;"></div>
							</div>
							<% } else {%>
							<%#DataBinder.Eval(Container.DataItem, "mailbox_size")%> bytes
							<% } %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Action">
						<HeaderTemplate><font color="Black"><nobr>Action</nobr></font></HeaderTemplate>
						<ItemTemplate>
							&nbsp;&nbsp;<a href="?mode=edit_user&uid=<%#DataBinder.Eval(Container.DataItem, "id_acct")%>">Edit</a>&nbsp;&nbsp;<a href="?mode=delete_user&uid=<%#DataBinder.Eval(Container.DataItem, "id_acct")%>" onclick="return confirm('Delete Account?');">Delete</a>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle Mode="NumericPages" HorizontalAlign="Center" PageButtonCount="5"></PagerStyle>
			</asp:DataGrid>
		</td>
	</tr>
	<tr>
		<td>
			<b>Total:</b> <%=_accountsCount%> account(s)<%=_usersCountString%>
		</td>
	</tr>
	<tr>
		<td>
			<table width="98%">
				<tr>
					<td valign="middle" align="center">
						<!--<div style="FLOAT: none"><br>
							[ <b>1</b> | <a href="#">2</a> | <a href="#">3</a> ]
						</div>-->
						<div style="FLOAT: left">
							<asp:Button Runat="server" Text="Create User" cssclass="wm_button" id="ButtonCreate" Width="150" />
						</div>
						<div style="FLOAT: right">
							<input onkeydown="javascript:searchTextboxKeyDown(event);" runat="server" type="text" ID="txtSearch" class="wm_input" size="30" />
							<asp:Button Runat="server" text="Search" cssclass="wm_button" id="ButtonSearch" />
						    <div style="display:none;">
						        <asp:LinkButton ID="lbl_SearchHiddenClick" runat="server" OnClick="ButtonSearch_Click" Text="aaa"></asp:LinkButton>
						    </div>
						
						</div>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<script type="text/javascript">

function searchTextboxKeyDown(e){
    var code = e.keyCode || window.event.keyCode || 0;
    if(code == 13)
    {    
        __doPostBack('userMngtID$lbl_SearchHiddenClick','');
    }
}
</script>