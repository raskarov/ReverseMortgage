<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditRuleData.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditRuleData" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<script language="javascript" type="text/javascript" defer="defer">
<!--
function RaddpValuePopup(){
    <%= dpValue.ClientID %>.ShowPopup();
}
-->
</script>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
<tr>
    <td align="center">
        <asp:Label ID="lblHeader" runat="server" Text="!!!!!!!!!!!!!!!!!!" SkinID="AdminHeader"></asp:Label>
    </td>
</tr>
<tr>
    <td align="center" runat="server" id="tdruleexp">&nbsp;</td>
</tr>
<tr>
    <td align="center" colspan="2">
        <asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
    </td>
</tr>
<tr>
    <td>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="width:75px;padding-right:3px" align="right">
                    <asp:Label ID="Label1" runat="server" Text="Select group" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:100px">
                    <asp:DropDownList ID="ddlGroup" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged"></asp:DropDownList>
                </td>
                <td style="width:75px;padding-right:3px" align="right">
                    <asp:Label ID="Label5" runat="server" Text="Select field" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:120px">
                    <asp:DropDownList ID="ddlField" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlField_SelectedIndexChanged"></asp:DropDownList>
                </td>
                <td style="width:120px;padding-right:3px" align="right">
                    <asp:Label ID="lblValue" runat="server" Text="Enter value" SkinID="AdminLabel" Width="100%" ></asp:Label>
                </td>                
                <td style="width:120px">
                    <asp:TextBox ID="tbValue" runat="server" Width="115px" MaxLength="256"></asp:TextBox>
                    <asp:DropDownList ID="ddlDictionary"  Width="115px" runat="server"></asp:DropDownList>
                    <radCln:RadDatePicker ID="dpValue" runat="server" Width="115px" FocusedDate="2099-12-31" MinDate="1800-01-01">
                        <DatePopupButton Visible="False" />
                        <DateInput onclick="RaddpValuePopup()" />
                    </radCln:RadDatePicker>
                    <radi:radmaskedtextbox id="mtb" runat="server" displaymask="#########.##" displaypromptchar=" " Width="115px" DisplayFormatPosition="Right" Mask="#########.##"></radi:radmaskedtextbox>
                </td>                
                <td><asp:Label ID="validatormsg" runat="server" Text="" ForeColor="red"></asp:Label></td>
            </tr>
            <tr style="height:10px"><td colspan="7">&nbsp;</td></tr> 
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
        </table>            
    </td>
</tr>   
<tr style="height:10px"><td>&nbsp;</td></tr> 
<tr>
    <td>
    <radG:RadGrid ID="G" runat="server" AllowPaging="false" AllowSorting="False" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" OnItemCommand="G_ItemCommand" OnItemDataBound="G_ItemDataBound">
        <ClientSettings>
        <Resizing AllowColumnResize="True" EnableRealTimeResize="false"/>
        </ClientSettings>
        <MasterTableView AllowNaturalSort="False">
            <Columns>
                <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Field">
                    <ItemTemplate>
                                <asp:Label ID="lblField" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FieldName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="250px"/>
                </radG:GridTemplateColumn>                    
                <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Value">
                    <ItemTemplate>
                        <asp:Label ID="lblFieldValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FieldValue") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="150px"/>
                </radG:GridTemplateColumn>                        
                <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Action" Resizable="False" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="delete" AlternateText="Delete role" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" Width="50px" />
                </radG:GridTemplateColumn>                        
           </Columns>
        </MasterTableView>
        <PagerStyle HorizontalAlign="Left" Mode="NumericPages" PagerTextFormat="" />
        </radG:RadGrid>
    </td>
</tr>
<tr style="height:10px"><td>&nbsp;</td></tr> 
<tr>
    <td align="center"><asp:Button ID="btnClose" runat="server" Text="Close"  SkinID="AdminButton" OnClick="btnClose_Click"/></td>
</tr>
</table>
