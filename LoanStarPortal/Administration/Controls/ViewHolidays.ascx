<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewHolidays.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewHolidays" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="right" style="width:70px;padding-right:3px">
                        <asp:Label ID="Label1" runat="server" Text="Select Year" Width="100%"></asp:Label>
                    </td>
                    <td align="left" style="width:150px">
                        <asp:DropDownList ID="ddlYears" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlYears_SelectedIndexChanged" Width="60px"></asp:DropDownList>
                    </td>
                    <td>&nbsp;</td>                
                </tr>
            </table>
        </td>        
    </tr>
    <tr>
        <td>
        <radG:RadGrid ID="G" Skin="WebBlue" GridLines="None" EnableAJAX="false" runat="server"        
            Width="600px"  AllowPaging="False" AutoGenerateColumns="False" 
            OnItemCommand="G_ItemCommand"  OnItemDataBound="G_ItemDataBound"
            AllowMultiRowEdit="false" OnItemCreated="G_ItemCreated" 
            >
            <ClientSettings>
            <Resizing AllowColumnResize="True" EnableRealTimeResize="True"/>
            </ClientSettings>
                <MasterTableView AllowNaturalSort="False" EditMode="InPlace" CommandItemDisplay="Bottom" DataKeyNames="ID" AutoGenerateColumns="False" >
                    <Columns>
                        <radG:GridTemplateColumn HeaderText="Holiday Name" SortExpression="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblLogin" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Description") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="100" SkinID="AdminInput" Text='<%# Eval("Description") %>' Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator id="Requiredfieldvalidator1" runat="server" controltovalidate="txtDescription" errormessage="*" display="Dynamic" setfocusonerror="true"></asp:RequiredFieldValidator>
                            </EditItemTemplate>                            
                            <HeaderStyle HorizontalAlign="Left" Width="400px"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Day" ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle">
                            <ItemTemplate>
                                <asp:Label ID="lblDay" runat="server" Text='<%# GetDate(Container.DataItem,"Holidaydate") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <radCln:RadDatePicker ID="radDatePicker" runat="server"  Width="120px" AllowEmpty="false"></radCln:RadDatePicker>
                            </EditItemTemplate>                            
                            <HeaderStyle HorizontalAlign="Center" Width="120px"/>
                        </radG:GridTemplateColumn>                        
                        <radg:GridEditCommandColumn ButtonType="ImageButton" UpdateImageUrl="~/Images/Update.gif" 
                            EditImageUrl="~/Images/Edit.gif" InsertImageUrl="~/Images/Insert.gif" 
                            CancelImageUrl="~/Images/Cancel.gif" UniqueName="EditCommandColumn">
                                <HeaderStyle Width="20px" /> 
                        </radg:GridEditCommandColumn>
                        <radG:GridButtonColumn ConfirmText="Delete this holiday?" ButtonType="ImageButton" ImageUrl="/Images/Delete.gif" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle Width="20px" />
                            <ItemStyle HorizontalAlign="Center"/>
                        </radG:GridButtonColumn>                        
                    </Columns>
                </MasterTableView>
                <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
            </radG:RadGrid>        
        </td>
    </tr>
</table>
