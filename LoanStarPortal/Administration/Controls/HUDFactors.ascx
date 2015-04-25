<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HUDFactors.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.HUDFactors" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>


<asp:SqlDataSource runat="server" ID="HUDFactorsSource">
</asp:SqlDataSource>

<div id="MyDiv" style="OVERFLOW:hidden; WIDTH: 900px; HEIGHT: 730px;">

<radG:RadGrid ID="RadGridHUDFactors" runat="server" EnableAJAX="True" Height="730px" Width="900px" 
DataSourceID="HUDFactorsSource" AllowSorting="True" Skin="WebBlue" GridLines="None" >
    <ClientSettings>
        <Resizing AllowColumnResize="True" />
        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" />
    </ClientSettings>
    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
            Font-Underline="False" HorizontalAlign="Left" Wrap="True" VerticalAlign="Middle" Width="130px" />
    <ItemStyle Wrap="False" Height="16px"/>
    <AlternatingItemStyle Wrap="False" Height="16px"/>
    
    <MasterTableView DataSourceID="HUDFactorsSource">
        <ExpandCollapseColumn Visible="False">
            <HeaderStyle Width="19px" />
        </ExpandCollapseColumn>
        <RowIndicatorColumn Visible="False">
            <HeaderStyle Width="20px" />
        </RowIndicatorColumn>
    </MasterTableView>
</radG:RadGrid>

</div>