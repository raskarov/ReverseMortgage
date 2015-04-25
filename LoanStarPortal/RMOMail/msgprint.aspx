<%@ Page language="c#" Codebehind="msgprint.aspx.cs" AutoEventWireup="True" Inherits="WebMailPro.msgprint" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<html>
	<head>
		<link rel="stylesheet" href="./skins/Hotmail_Style/styles.css" type="text/css" /></head>
		<body class="wm_body"><div align="center" class="wm_space_before">
		<table class="wm_print">
					<tr>
						<td class="wm_print_content" style="border-width: 0px 1px 1px 0px">
							<%=_manager.GetString("From")%>
						</td>
						<td class="wm_print_content" style="border-width: 0px 0px 1px 1px">
							<asp:Literal ID="LabelFrom" Runat="server"></asp:Literal>
						</td>
					</tr>
					<tr>
						<td class="wm_print_content" style="border-width: 0px 1px 1px 0px">
						<%=_manager.GetString("To")%>
						</td>
						<td class="wm_print_content" style="border-width: 0px 0px 1px 1px">
							<asp:Literal ID="LabelTo" Runat="server"></asp:Literal>
						</td>
					</tr>
					<tr>
						<td class="wm_print_content" style="border-width: 0px 1px 1px 0px">
							<%=_manager.GetString("Date")%>
						</td>
						<td class="wm_print_content" style="border-width: 0px 0px 1px 1px">
							<asp:Literal ID="LabelDate" Runat="server"></asp:Literal>
						</td>
					</tr>
					<tr>
						<td class="wm_print_content" style="border-width: 0px 1px 1px 0px">
							<%=_manager.GetString("Subject")%>
						</td>
						<td class="wm_print_content" style="border-width: 0px 0px 1px 1px">
							<asp:Literal ID="LabelSubject" Runat="server"></asp:Literal>
						</td>
					</tr>
					<tr>
						<td colspan="2" class="wm_print_content" style="border-width: 1px 0px 0px 0px">
							<div class="wm_space_before">
								<asp:Literal ID="MessageBody" Runat="server"></asp:Literal>
							</div>
						</td>
					</tr>
				</table>
			</div>
		</body>
	</html>
