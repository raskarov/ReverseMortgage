<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FieldDetails.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.FieldDetails" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<table width="800px" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td align="left" colspan="2" style="padding-left:10px">
            <asp:Label ID="lblHeader" runat="server" Text="" SkinID="AdminHeader"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                <tr class="adminfdtr">
                    <td class="adminfdlabel">Display label:</td>
                    <td>
                        <asp:Label ID="lblDisplay" runat="server" Text="" ></asp:Label>
                        <asp:TextBox ID="tbDisplayLabel" runat="server" MaxLength="256" Width="450px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="adminfdtr">
                    <td class="adminfdlabel">Location:</td>
                    <td>
                        <asp:Label ID="lblLocation" runat="server" Text=""></asp:Label>
                        <div id="dvLocations" runat="server">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 110%">
                                <tr>
                                    <td style="width:120px">Top level tab :</td>
                                    <td>
                                        <asp:DropDownList ID="ddlTopLevelTab" runat="server" Width="200px" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>                                    
                                    <td style="width: 120px">Level 2 tab :</td>
                                    <td>
                                        <asp:DropDownList ID="ddlSecondLevelTab" runat="server" Width="200px" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 120px">Pseudo tab :</td>
                                    <td>
                                        <asp:DropDownList ID="ddlPseudoTab" runat="server" Width="200px" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 120px">Pseudo tab group :</td>
                                    <td>
                                        <asp:DropDownList ID="ddlPseudoTabGroup" runat="server" Width="400px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr class="adminfdtr">
                    <td class="adminfdlabel">Display Order:</td>
                    <td>
                        <asp:Label ID="lblDisplayOrder" runat="server" Text=""></asp:Label>
                        <asp:TextBox ID="tbDisplayOrder" runat="server" MaxLength="3" Width="40px"></asp:TextBox><asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="*" ControlToValidate="tbDisplayOrder" MaximumValue="1000" MinimumValue="0" Type="Integer"></asp:RangeValidator>
                    </td>                    
                </tr>
                <tr>
                    <td class="adminfdlabel">&nbsp;</td>
                    <td style="height: 43px">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" Text="Edit" Width="60px" />
                                </td>
                                <td style="width:80px">
                                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" Width="60px"/>
                                </td>
                                <td>
                                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" Width="60px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">    
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                <tr class="adminfdtr">
                    <td class="adminfdlabel">Control:</td>
                    <td style="height: 21px"><asp:Label ID="lblControl" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="adminfdtr">
                    <td class="adminfdlabel">Group:</td>
                    <td><asp:Label ID="lblGroup" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="adminfdtr">
                    <td class="adminfdlabel">Calculated?</td>
                    <td><asp:Label ID="lblCalculated" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="adminfdtr">
                    <td class="adminfdlabel">Can be used in rules?</td>
                    <td><asp:Label ID="lblUsedInRules" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="adminfdtr">
                    <td class="adminfdlabel">Is required?</td>
                    <td><asp:Label ID="lblRequired" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="adminfdtr">
                    <td class="adminfdlabel">Dictionary field?</td>
                    <td><asp:Label ID="lblDictionary" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="adminfdtr">
                    <td class="adminfdlabel">Field type:</td>
                    <td><asp:Label ID="lblType" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="adminfdtr">
                    <td class="adminfdlabel">Db type:</td>
                    <td><asp:Label ID="lblDbType" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="adminfdtr" style="padding-top: 10px">
                    <td class="adminfdlabel">Rules info:</td>
                    <td><asp:Label ID="lblRules" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="trRuleInfo" runat="server">
        <td colspan="2" style="padding-left:10px">
            <table border="1" cellpadding="0" cellspacing="0" style="width:100%" id="tblRuleInfo" runat="server">
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                <tr id="trVisibilityInfo" runat="server" class="adminfdtr" style="padding-top: 10px">
                    <td class="adminfdlabel">Visibility depends on:</td>
                    <td><asp:Label ID="lblVisibility" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                <tr>
                    <td class="adminfdlabel">Document mapping info:</td>
                    <td><asp:Label ID="lblDocMappingInfo" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="trMapInfo" runat="server">
        <td colspan="2" style="padding-left:10px">
            <radg:radgrid id="G" runat="server" autogeneratecolumns="false" borderstyle="Solid" borderwidth="1px" skin="WebBlue" width="90%">
                <MasterTableView AllowNaturalSort="False">
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Document">
                            <ItemTemplate>
                                <asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Title") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="70%"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="version" >
                            <ItemTemplate>
                                <asp:Label ID="lblLabel" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "version") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </radG:GridTemplateColumn>                        
                    </Columns>
                </MasterTableView>
            </radg:radgrid>
        </td>
    </tr>
    <tr style="padding-top:10px">
        <td colspan="2">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                <tr>
                    <td class="adminfdlabel">&nbsp;</td>
                    <td><asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
