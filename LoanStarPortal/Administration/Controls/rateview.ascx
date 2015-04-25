<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rateview.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.rateview" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>

<h3>Index rates list</h3>
   <asp:Panel ID="panelFilter" runat="server" Visible="false">
    Show <asp:DropDownList ID="ddlProducts" runat="server" DataTextField="Name" DataValueField="id" AutoPostBack="True" OnSelectedIndexChanged="ddlProducts_SelectedIndexChanged"></asp:DropDownList> product.
    </asp:Panel>
    <h3><asp:Label ID="lblProductName" runat="server"></asp:Label></h3>
    <radG:RadGrid ID="G" runat="server" AllowPaging="True" PageSize="15" AllowSorting="True" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" OnNeedDataSource="G_NeedDataSource" OnItemDataBound="G_ItemDataBound">
    <ClientSettings>
    <Resizing AllowColumnResize="True" EnableRealTimeResize="True"/>
    </ClientSettings>
        <MasterTableView AllowNaturalSort="False">
            <Columns>
                <radG:GridBoundColumn HeaderText="Date" DataField="Period"  DataFormatString="{0:d}"></radG:GridBoundColumn>
                <radG:GridBoundColumn HeaderText="Weekday" DataField="weekday"></radG:GridBoundColumn>
                <radg:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-BorderStyle="None" ItemStyle-BorderWidth="0px">
                   <HeaderTemplate>
                    <TABLE id="Table1" cellSpacing="0" cellPadding="0" width="300" border="0">
                     <TR>
                      <TD colspan="5" align="center"><b>Initial</b></TD>
                     </TR>
                     <TR>
                      <TD width="20%" align="center"><b>Index</b></TD>
                      <TD width="20%" align="center"><b>Margin</b></TD>
                      <TD width="20%" align="center"><b>Daily Rate</b></TD>
                      <TD width="20%" align="center"><b>Running Average</b></TD>
                      <TD width="20%" align="center"><b>Published Rate</b></TD>
                     </TR>
                    </TABLE>
                   </HeaderTemplate>
                   <ItemTemplate>
                    <TABLE id="Table2" cellSpacing="0" cellPadding="0" width="300" border="0">
                     <TR>
                      <TD width="20%"><%# DataBinder.Eval(Container.DataItem, "IndexRate", "{0:f3}")%></TD>
                      <TD width="20%"><%# DataBinder.Eval(Container.DataItem, "Margin", "{0:f3}")%></TD>
                      <TD width="20%"><%# DataBinder.Eval(Container.DataItem, "DailyRate", "{0:f3}")%></TD>
                      <TD width="20%"><%# DataBinder.Eval(Container.DataItem, "AveDailyRate", "{0:f3}")%></TD>
                      <TD width="20%"><asp:Label ID="lblPublishedRate" runat="server"></asp:Label></TD>
                     </TR>
                    </TABLE>
                   </ItemTemplate>
                  </radg:GridTemplateColumn>
                <radg:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-BorderStyle="None" ItemStyle-BorderWidth="0px">
                   <HeaderTemplate>
                    <TABLE id="Table1" cellSpacing="1" cellPadding="1" width="300" border="0">
                     <TR>
                      <TD colspan="5" align="center"><b>Expected</b></TD>
                     </TR>
                     <TR>
                      <TD width="20%" align="center"><b>Index</b></TD>
                      <TD width="20%" align="center"><b>Margin</b></TD>
                      <TD width="20%" align="center"><b>Daily Rate</b></TD>
                      <TD width="20%" align="center"><b>Running Average</b></TD>
                      <TD width="20%" align="center"><b>Published Rate</b></TD>
                     </TR>
                    </TABLE>
                   </HeaderTemplate>
                   <ItemTemplate>
                    <TABLE id="Table2" cellSpacing="1" cellPadding="1" width="300" border="0">
                     <TR>
                      <TD width="20%"><%# DataBinder.Eval(Container.DataItem, "ExpIndexRate", "{0:f3}")%></TD>
                      <TD width="20%"><%# DataBinder.Eval(Container.DataItem, "Margin", "{0:f3}")%></TD>
                      <TD width="20%"><%# DataBinder.Eval(Container.DataItem, "ExpDailyRate", "{0:f3}")%></TD>
                      <TD width="20%"><%# DataBinder.Eval(Container.DataItem, "ExpAveDailyRate", "{0:f3}")%></TD>
                      <TD width="20%"><%# DataBinder.Eval(Container.DataItem, "ExpPublishedIndex", "{0:f2}")%></TD>          
                     </TR>
                    </TABLE>
                   </ItemTemplate>
                  </radg:GridTemplateColumn>
            </Columns>
        </MasterTableView>
        <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
    </radG:RadGrid>            
    <asp:Button ID="btnBack" runat="server" Text="Back" SkinID="AdminButton" OnClick="btnBack_Click" CausesValidation="False"/>