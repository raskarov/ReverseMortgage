<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyFeeSettings.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.CompanyFeeSettings" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<table cellpadding="0" cellspacing="0" width="100%" border="0">
    <tr>
    <td align="center">
    <table cellpadding="0" cellspacing="0" width="50%" border="0">
    <tr runat="server" id="trProduct">
        <td style="width:100px" align="left"><asp:Label id="lblProduct" runat="server" Text="Select product:" SkinID="AdminLabel"></asp:Label></td>
        <td align="left"><asp:DropDownList id="ddlProduct" runat="server" OnSelectedIndexChanged="ddl_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></td>
        <td>&nbsp;</td>
    </tr>
    <tr style="padding-top:5px">
        <td colspan="3" align="left">
        <radG:RadGrid ID="G" Skin="WebBlue" GridLines="None" EnableAJAX="false" runat="server" AllowSorting="false"
            Width="230px"  AllowPaging="False" AutoGenerateColumns="False" OnItemCommand="G_ItemCommand" AllowMultiRowEdit="false" OnItemDataBound="G_ItemDataBound">
            <ClientSettings>
            <Resizing AllowColumnResize="false" EnableRealTimeResize="false"/>
            </ClientSettings>
                <MasterTableView AllowNaturalSort="False" EditMode="InPlace" DataKeyNames="ID" AutoGenerateColumns="False" CommandItemDisplay="None">
                    <Columns>
                        <radG:GridTemplateColumn HeaderText="Fee">
                            <ItemTemplate>
                                <asp:Label ID="lblValue" runat="server" Text='<%#string.Format("{0:C}", Eval("Fee"))%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Default" ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDefault" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsDefault") %>' Enabled="false"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px"/>
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn HeaderText="" ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnSetDefault" runat="server" AlternateText="SetDefault" CommandName="SetDefault" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' ImageUrl="~/Images/btn_add_task.gif" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="20px"/>
                        </radG:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
            </radG:RadGrid>        
        </td>
    </tr>
    <tr style="padding-top:5px">
        <td align="left" colspan="3">
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnLoad" runat="server" Text="Load global settings" OnClick="btnLoad_Click" />
        </td>
    </tr>
    </table>
    </td>
    </tr>
</table>
