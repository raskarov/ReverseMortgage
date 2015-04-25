<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NavigationToolbar.ascx.cs" Inherits="LoanStarPortal.Controls.NavigationToolbar" %>
<%@ Register Assembly="RadToolbar.Net2" Namespace="Telerik.WebControls" TagPrefix="radTlb" %>
<radTlb:radtoolbar id="NavToolbar" runat="server" Orientation="Vertical" UseFadeEffect="True" OnOnClick="NavToolbar_OnClick" AutoPostBack="true">
    <items>
        <radTlb:radtoolbartogglebutton id="button_email" Tooltip="Email" CommandName="email" ButtonText="Email" DisplayType="TextOnly" ButtonGroup="Navigation"/>
        <radTlb:radtoolbartogglebutton id="button_notes" Tooltip="Notes" CommandName="notes" ButtonText="Notes" DisplayType="TextOnly" ButtonGroup="Navigation"/>
        <radTlb:radtoolbartogglebutton id="button_chat" Tooltip="Chat" CommandName="chat" ButtonText="Chat" DisplayType="TextOnly" ButtonGroup="Navigation"/>
        <radTlb:radtoolbartogglebutton id="button_tasks" Tooltip="Tasks" CommandName="tasks" ButtonText="Tasks" DisplayType="TextOnly" ButtonGroup="Navigation"/>
        <radTlb:radtoolbartogglebutton id="button_alerts" Tooltip="Alerts" CommandName="alerts" ButtonText="Alerts" DisplayType="TextOnly" ButtonGroup="Navigation"/>
        <radTlb:radtoolbartogglebutton id="button_events" Tooltip="Events" CommandName="events" ButtonText="Events" DisplayType="TextOnly" ButtonGroup="Navigation"/>
        <radTlb:radtoolbartogglebutton id="button_info" Tooltip="Info" CommandName="info" ButtonText="Info" DisplayType="TextOnly" ButtonGroup="Navigation"/>
        <radTlb:radtoolbartogglebutton id="button_report" Tooltip="Report" CommandName="report" ButtonText="Report" DisplayType="TextOnly" ButtonGroup="Navigation"/>
        <radTlb:radtoolbartogglebutton id="button_help" Tooltip="Help" CommandName="help" ButtonText="Help" DisplayType="TextOnly" ButtonGroup="Navigation"/>
        <radTlb:radtoolbartogglebutton id="button_guides" Tooltip="Guides" CommandName="guides" ButtonText="Guides" DisplayType="TextOnly" ButtonGroup="Navigation"/>
        <radTlb:radtoolbartogglebutton id="button_fha" Tooltip="FHA" CommandName="fha" ButtonText="FHA" DisplayType="TextOnly" ButtonGroup="Navigation"/>
        <radTlb:radtoolbartogglebutton id="button_links" Tooltip="Links" CommandName="links" ButtonText="Links" DisplayType="TextOnly" ButtonGroup="Navigation"/>
        <radTlb:radtoolbartogglebutton id="button_res" Tooltip="Res." CommandName="res" ButtonText="Res." DisplayType="TextOnly" ButtonGroup="Navigation"/>
    </items>
</radTlb:radtoolbar>