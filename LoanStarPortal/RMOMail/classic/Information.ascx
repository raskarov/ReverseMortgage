<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Information.ascx.cs" Inherits="WebMailPro.classic.Information" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table class="wm_hide" id="info">
	<tr>
		<td class="wm_info_image"><IMG id="info_image" src="skins/<%=Skin%>/error.gif" >
		</td>
		<td class="wm_info_message" id="info_message"><%=_manager.GetString("InfoWebMailLoading")%>
		</td>
	</tr>
</table>
