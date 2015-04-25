<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditRuleAlert.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditRuleAlert" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
<tr>
    <td align="center">
        <asp:Label ID="lblHeader" runat="server" Text="" SkinID="AdminHeader"></asp:Label>
    </td>
</tr>
<tr>
    <td align="center" runat="server" id="tdruleexp">&nbsp;</td>
</tr>
<tr>
    <td align="center">
        <asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
    </td>
</tr>    
<tr>
    <td valign="top" >
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="width:200px; padding-right:3px" align="right" colspan="2">
                    <asp:Label ID="Label4" runat="server" Text="Message:" SkinID="AdminLabel" ></asp:Label>
                </td>
                <td style="width:300px">
                    <asp:TextBox ID="tbMessage" runat="server" Text="" Width="315px" MaxLength="256" ></asp:TextBox>
                </td>
                <td align="left">
                    <asp:Label ID="tbMessageerr" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr style="padding-top:5px;">
                <td style="width:135px">&nbsp;</td>
                <td style="width:65px;border-top:solid 1px black; border-bottom:solid 1px black; border-left:solid 1px black;padding-top:3px;padding-bottom:3px" align="left">
                    <asp:RadioButton ID="rbEvent" runat="server" Text="Event" GroupName="alertevent" Checked="True"/>
                </td>
                <td style="width:300px;border-top:solid 1px black; border-bottom:solid 1px black; border-right:solid 1px black;padding-top:3px;padding-bottom:3px" valign="middle">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width:65px">
                                <asp:Label ID="Label1" runat="server" Text="EventType:" SkinID="AdminLabel" Width="50px"></asp:Label>
                            </td>
                            <td style="width:135px">
                                <asp:DropDownList ID="ddlEventType" runat="server" SkinID="AdminSelect" AutoPostBack="false" Width="130px" ></asp:DropDownList>&nbsp;
                            </td>
                            <td align="left" style="width:10px">
                                <asp:Label ID="ddltypeerr" runat="server" ForeColor="Red" Width="5px"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>                  
                </td>
                <td>&nbsp;</td>
            </tr>            
            <tr>
                <td style="width:135px">&nbsp;</td>
                <td style="width:65px;" align="left">
                    <asp:RadioButton ID="rbAlert" runat="server" Text="Alert  " GroupName="alertevent"/>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="left"><asp:Button ID="btnCancel" runat="server" Text="Cancel"  SkinID="AdminButton" OnClick="btnCancel_Click" CausesValidation="False"/></td>
                            <td align="right"><asp:Button ID="btnSave" runat="server" Text="Save"  SkinID="AdminButton" OnClick="btnSave_Click" CausesValidation="False"/></td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>            
            </tr>
        </table>    
    </td>
</tr>
<tr style="padding-top:5px;">
    <td>
            <radG:RadGrid ID="G" runat="server" AllowPaging="false" AllowSorting="False" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" OnItemCommand="G_ItemCommand" OnItemDataBound="G_ItemDataBound">
            <ClientSettings>
            <Resizing AllowColumnResize="True" EnableRealTimeResize="false"/>
            </ClientSettings>
                <MasterTableView AllowNaturalSort="False">
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Message">
                            <ItemTemplate>
                                <asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Message") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="400px"/>
                        </radG:GridTemplateColumn>                    
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Type">
                            <ItemTemplate>
                                <asp:Label ID="lbltype" eventtypeid='<%# DataBinder.Eval(Container.DataItem, "EventTypeId") %>' runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Type") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px"/>
                        </radG:GridTemplateColumn>                        
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Action" Resizable="False" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton  id="btnEdit" ImageUrl="~/images/btn_grd_edit.gif" CommandName="editalert" AlternateText="Edit" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="delete" AlternateText="Delete" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                        </radG:GridTemplateColumn>                        
                    </Columns>
                </MasterTableView>
                <PagerStyle HorizontalAlign="Left" Mode="NumericPages" PagerTextFormat="" />
            </radG:RadGrid>    
    </td>
</tr>
<tr style="padding-top:5px;">
    <td align="center"><asp:Button ID="btnClose" runat="server" Text="Close"  SkinID="AdminButton" OnClick="btnClose_Click"/></td>
</tr>
</table>
