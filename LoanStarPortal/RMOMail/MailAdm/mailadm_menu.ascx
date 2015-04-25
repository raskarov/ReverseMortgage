<%@ Control Language="c#" AutoEventWireup="True" Codebehind="mailadm_menu.ascx.cs" Inherits="WebMailPro.MailAdm.mailadm_menu" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

			<div <% if (_selectedScreen == 1) Response.Write("class=\"wm_selected_settings_item\""); %> >
				<nobr><li><a href="?mode=db">Database Settings</a></li></nobr>
			</div>
			<div <% if ((_selectedScreen == 2) || (_selectedScreen == 7)) Response.Write("class=\"wm_selected_settings_item\""); %> >
				<nobr><li><a href="?mode=users">Users Management</a></li></nobr>
			</div>
			<div <% if (_selectedScreen == 3) Response.Write("class=\"wm_selected_settings_item\""); %> >
				<nobr><li><a href="?mode=webmail">WebMail Settings</a></li></nobr>
			</div>
			<div <% if (_selectedScreen == 4) Response.Write("class=\"wm_selected_settings_item\""); %> >
				<nobr><li><a href="?mode=interface">Interface Settings</a></li></nobr>
			</div>
			<div <% if (_selectedScreen == 5) Response.Write("class=\"wm_selected_settings_item\""); %> >
				<nobr><li><a href="?mode=login">Login Settings</a></li></nobr>
			</div>
			<div <% if (_selectedScreen == 9) Response.Write("class=\"wm_selected_settings_item\""); %> >
				<nobr><li><a href="?mode=calendar">Calendar Settings</a></li></nobr>
			</div>
			<div <% if (_selectedScreen == 6) Response.Write("class=\"wm_selected_settings_item\""); %> >
				<nobr><li><a href="?mode=debug">Debug Settings</a></li></nobr>
			</div>
			<div <% if (_selectedScreen == 8) Response.Write("class=\"wm_selected_settings_item\""); %> >
				<nobr><li><a href="?mode=mailserver">Mail Server Integration</a></li></nobr>
			</div>
