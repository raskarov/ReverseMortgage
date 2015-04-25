<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MpStatus.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.MpStatus" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<script language="javascript" type="text/javascript">
function checkSelect1(){
    return checkSelect(document.getElementById('<%= ddlMPStatus1.ClientID %>'),document.getElementById('<%= ddl1err.ClientID %>'));
}
function checkSelect2(){
    var res1 = checkSelect(document.getElementById('<%= ddlMPStatus1.ClientID %>'),document.getElementById('<%= ddl1err.ClientID %>'));
    var res2 = checkSelect(document.getElementById('<%= ddlMPStatus2.ClientID %>'),document.getElementById('<%= ddl2err.ClientID %>'));
    return (res1 && res2);    
}
-->
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<table border="0" cellpadding="0" cellspacing="0" width="99%">
<tr>
    <td style="width:300px;vertical-align:top;" >
        <radG:RadGrid ID="G" runat="server" Width="99%" Height="99%" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" AutoGenerateColumns="false"  AllowPaging="True" PageSize="3" OnItemCommand="G_ItemCommand" OnItemDataBound="G_ItemDataBound"  OnPageIndexChanged="G_PageIndexChanged">
                <MasterTableView AllowNaturalSort="False">
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Initial status">
                            <ItemTemplate>
                                <asp:Label ID="lblstatus1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProfileInitStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Final status">
                            <ItemTemplate>
                                <asp:Label ID="lblstatus2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProfileFinalStatus") %>'></asp:Label>
                            </ItemTemplate>
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
    <td style="padding-left:20px; vertical-align:top">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>&nbsp;</td>
                    <td colspan="2">
                        <asp:Label ID="lblAction" runat="server" Text="Add new" SkinID="AdminLabel"></asp:Label>
                    </td>
            </tr>
            <tr>
                <td style="width:88px; height: 38px;">
                    <asp:Label ID="Label1" runat="server" Text="Initial status" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="height: 38px">
                    <asp:DropDownList ID="ddlMPStatus1" runat="server" SkinID="AdminSelect" Width="140px" AutoPostBack="True" OnSelectedIndexChanged="ddlMPStatus1_SelectedIndexChanged"></asp:DropDownList>
                    <asp:Label ID="ddl1err" runat="server" ForeColor="red"></asp:Label>
                </td>
                <td style="height: 38px" align="left"></td>
            </tr>
            <tr>
                 <td>
                    <asp:Label ID="Label2" runat="server" Text="Final status" SkinID="AdminLabel"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMPStatus2" runat="server" SkinID="AdminSelect" Width="140px"></asp:DropDownList>
                    <asp:Label ID="ddl2err" runat="server" ForeColor="red"></asp:Label>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td align="left">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left" style="width:83px"><asp:Button ID="btnCancel" runat="server" Text="Cancel" SkinID="AdminButton" OnClick="btnCancel_Click" CausesValidation="False"/></td>
                            <td align="left"><asp:Button ID="btnSave" runat="server" Text="Add" SkinID="AdminButton" OnClick="btnAdd_Click" CausesValidation="False"/></td>
                        </tr>
                    </table>                    
                </td>
            </tr>
        </table>        
    </td>
</tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>        
        
