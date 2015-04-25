<%@ Control Language="c#" AutoEventWireup="True" Codebehind="FoldersPart.ascx.cs" Inherits="WebMailPro.classic.Folders_part" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div class="wm_folders_part" id="folders_part">
	<div class="wm_folders_hide_show" id="folders_hide">
		<a href="#" onclick="ChangeFoldersMode(); return false;">
			<img id="folders_hide_img" title="<%=_manager.GetString("HideFolders")%>" src="./skins/<%=Skin%>/folders/hide_folders.gif" class="wm_control_img">
		</a>
	</div>
	<div class="wm_folders" id="folders">
		<div id="Folders" runat="server">
		</div>
	</div>
	<div class="wm_manage_folders" id="manage_folders" align="center">
		<a href="basewebmail.aspx?scr=settings_accounts_folders"><%=_manager.GetString("ManageFolders")%></a>
	</div>
</div>
