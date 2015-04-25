<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditRule.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditRule" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<script language="javascript" type="text/javascript" defer="defer">
<!--
function RaddpValuePopup(){
    <%= dpValue.ClientID %>.ShowPopup();
}
-->
</script>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
<tr>
    <td>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="width:30px">
                    <asp:Label ID="Label4" runat="server" Text="NOT" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:80px">
                    <asp:Label ID="Label3" runat="server" Text="Logical Op" SkinID="AdminLabel"></asp:Label>
                </td>        
                <td style="width:100px">
                    <asp:Label ID="Label1" runat="server" Text="Select group" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:140px">
                    <asp:Label ID="Label5" runat="server" Text="Select field" SkinID="AdminLabel"></asp:Label>
                </td>        
                <td style="width:80px">
                    <asp:Label ID="Label2" runat="server" Text="Compare Op" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:130px">
                    <asp:Label ID="lblValue" runat="server" Text="Enter value" SkinID="AdminLabel" Width="130px" ></asp:Label>
                </td>
                <td>&nbsp;</td>        
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="cbNot" runat="server" />
                </td>    
                <td>
                    <asp:DropDownList ID="ddlLogicalOp" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlLogicalOp_SelectedIndexChanged" CssClass="selectcenter" SkinID="SelectCenter"></asp:DropDownList>
                </td>        
                <td>
                    <asp:DropDownList ID="ddlGroup" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged"></asp:DropDownList>
                </td>        
                <td>
                    <asp:DropDownList ID="ddlField" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlField_SelectedIndexChanged"></asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCompareOp" Width="100%" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompareOp_SelectedIndexChanged" SkinID="SelectCenter"></asp:DropDownList>
                </td>        
                <td>
                    <asp:TextBox ID="tbValue" runat="server" Width="98%" MaxLength="256"></asp:TextBox>
                    <asp:DropDownList ID="ddlDictionary"  Width="98%" runat="server"></asp:DropDownList>
                    <radCln:RadDatePicker ID="dpValue" runat="server" Width="98%" FocusedDate="2099-12-31" MinDate="1800-01-01">
                        <DatePopupButton Visible="False" />
                        <DateInput onclick="RaddpValuePopup()" />
                    </radCln:RadDatePicker>
                    <radi:radmaskedtextbox id="mtb" runat="server" displaymask="#########.##" displaypromptchar=" " Width="98%" DisplayFormatPosition="Right" Mask="#########.##"></radi:radmaskedtextbox>
                </td>
                <td>&nbsp;</td>        
            </tr>
            <tr>
                <td style="height: 19px">&nbsp;</td>
                <td style="height: 19px">&nbsp;</td>
                <td style="height: 19px">&nbsp;</td>
                <td style="height: 19px">&nbsp;</td>
                <td style="height: 19px">&nbsp;</td>
                <td style="height: 19px">
                    <asp:Label ID="validatormsg" runat="server" Text="" ForeColor="red"></asp:Label>
                </td>
                <td style="height: 19px">&nbsp;</td>        
            </tr>    
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="left">
                                <asp:Button ID="btnCancel" runat="server" Text="Back" SkinID="AdminButton" CausesValidation="False" OnClick="btnCancel_Click" />
                            </td>
                            <td align="right">
                                <asp:Button ID="btnAdd" runat="server" Text="Add" SkinID="AdminButton" OnClick="btnAdd_Click"/>
                            </td>
                        </tr>
                    </table>            
                </td>
                <td>&nbsp;</td>        
            </tr>
            <tr style="height:5px"><td colspan="7">&nbsp;</td></tr>
            <tr>
                <td colspan="7">
                    <radG:RadGrid id="G" runat="server" Width="99%" Height="99%" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" AutoGenerateColumns="false"  AllowPaging="True" PageSize="3" OnItemCommand="G_ItemCommand" OnPageIndexChanged="G_PageIndexChanged" OnItemDataBound="G_ItemDataBound">
                    <MasterTableView AllowNaturalSort="False">
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Logical Op">
                            <ItemTemplate>
                                <asp:Label ID="lbllogop" runat="server" Text='<%# GetLogicalOp(Container.DataItem, "logicalop") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="60px"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn">
                            <ItemTemplate>
                                <asp:Label ID="lbllognot" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "logicalnot") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="20px"/>
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Field">
                            <ItemTemplate>
                                <asp:Label ID="lblfield" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'></asp:Label>
                            </ItemTemplate>
                        </radG:GridTemplateColumn>     
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Compare Op">
                            <ItemTemplate>
                                <asp:Label ID="lblcompareop" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "compareop") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="70px"/>
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>                                           
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Value">
                            <ItemTemplate>
                                <asp:Label ID="lblvaleu" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "datavalue") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px"/>
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Action" Resizable="False">
                            <ItemTemplate>
                                <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="delete" AlternateText="Delete Status" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>                           
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>                        
                    </Columns>
                    </MasterTableView>        
                    <PagerStyle HorizontalAlign="Left" Mode="NumericPages" PagerTextFormat="" />
                    </radG:RadGrid>        
                </td>
            </tr>
            <tr>
                <td colspan="7">
                     <asp:Label ID="Label6" runat="server" Text="Total expression:"  SkinID="AdminLabel"></asp:Label>
                </td>
            </tr>
            <tr><td colspan="7" runat="server" id="tdexpr">&nbsp;</td></tr>
        </table>    
    </td>
</tr>
</table>

