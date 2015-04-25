<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="limitview.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.limitview" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>

<h3>Lending limit list</h3>
    Show <asp:DropDownList ID="ddlStates" runat="server" DataTextField="Name" DataValueField="id" AutoPostBack="True" OnSelectedIndexChanged="ddlStates_SelectedIndexChanged"></asp:DropDownList> state.
    <radG:RadGrid ID="G" runat="server" AllowPaging="False" AllowSorting="True" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" OnNeedDataSource="G_NeedDataSource">
    <ClientSettings>
    <Resizing AllowColumnResize="True" EnableRealTimeResize="True"/>
    </ClientSettings>
        <MasterTableView AllowNaturalSort="False">
            <Columns>
                <radG:GridBoundColumn HeaderText="State" DataField="StateName"></radG:GridBoundColumn>
                <radG:GridBoundColumn HeaderText="County" DataField="CountyName"></radG:GridBoundColumn>
                <radG:GridBoundColumn HeaderText="OneFamily" DataField="OneFamily"></radG:GridBoundColumn>
                <radG:GridBoundColumn HeaderText="TwoFamily" DataField="TwoFamily"></radG:GridBoundColumn>
                <radG:GridBoundColumn HeaderText="ThreeFamily" DataField="ThreeFamily"></radG:GridBoundColumn>
                <radG:GridBoundColumn HeaderText="FourFamily" DataField="FourFamily"></radG:GridBoundColumn>
            </Columns>
        </MasterTableView>
        <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
    </radG:RadGrid>            
