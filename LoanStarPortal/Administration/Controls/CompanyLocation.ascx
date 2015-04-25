<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyLocation.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.CompanyLocation" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>
<%@ Register Assembly="RadTreeView.Net2" Namespace="Telerik.WebControls" TagPrefix="radT" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<script language="javascript" type="text/javascript">
<!--
function CheckDelete(node,itemText){
    if (itemText.toLowerCase()=='delete'){
        return confirm('Are you sure you want to delete this branch?');
    }
    return true;
}
-->
</script>
<table cellpadding="0" cellspacing="0" border="0" style="width:100%;height:600px">
    <tr>
        <td style="width:100%;vertical-align:top;height:100%;border:solid black 1px">
            <radspl:radsplitter id="RadSplitter1" runat="server" Orientation="Vertical" Skin="Outlook" Height="100%" Width="100%">
            <radspl:radpane id="LeftPane" runat="server" Width="40%">    
                <table cellpadding="0" cellspacing="0" border="0" style="width:100%">
                    <tr>
                        <td valign="top">
                            <radT:RadTreeView ID="rtvLocation" runat="server" Skin="Outlook" Width="100%" ExpandDelay="0" OnNodeBound="rtvLocation_NodeBound" BeforeClientContextClick="CheckDelete" OnNodeContextClick="rtvLocation_NodeContextClick" OnNodeClick="rtvLocation_NodeClick" DragAndDrop="True" DragAndDropBetweenNodes="false" OnNodeDrop="rtvLocation_NodeDrop">
                            </radT:RadTreeView>
                        </td>
                    </tr>
                </table>                
                </radspl:radpane>                   
                <radspl:radsplitbar id="RadSplitBar2" runat="server" collapsemode="Backward" Width="10px"/>
                <radspl:radpane id="RightPane" runat="server">
                    <asp:Panel ID="pnlEditLocation" runat="server" Width="100%">
                    <div id="divEditLocation" runat="server">
                    <table cellpadding="0" cellspacing="0" border="0" style="width:100%">
                        <tr style="padding-top:15px;">
                            <td class="tdlloc"><asp:Label ID="lblRuleName" runat="server" Text="Name of location:"></asp:Label></td>
                            <td class="tdcloc">
                                <asp:TextBox ID="tbName" runat="server" MaxLength="100" Width="350px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="tbName"></asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdlloc"><asp:Label ID="Label1" runat="server" Text="Address 1:"></asp:Label></td>
                            <td class="tdcloc">
                                <asp:TextBox ID="tbAddress1" runat="server" MaxLength="256"  Width="350px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="tbAddress1"></asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdlloc"><asp:Label ID="Label3" runat="server" Text="Address 2:"></asp:Label></td>
                            <td class="tdcloc">
                                <asp:TextBox ID="tbAddress2" runat="server" MaxLength="256" Width="350px"></asp:TextBox>                                
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdlloc"><asp:Label ID="Label4" runat="server" Text="City:"></asp:Label></td>
                            <td class="tdcloc">
                                <asp:TextBox ID="tbCity" runat="server" MaxLength="50" Width="350px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="tbCity"></asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdlloc"><asp:Label ID="Label5" runat="server" Text="State:"></asp:Label></td>
                            <td class="tdcloc">
                                <asp:DropDownList ID="ddlState" runat="server"></asp:DropDownList>
                                <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="*" MinimumValue="1" MaximumValue="100" Type="Integer" ControlToValidate="ddlState"></asp:RangeValidator>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdlloc"><asp:Label ID="Label7" runat="server" Text="Zip:"></asp:Label></td>
                            <td class="tdcloc">
                                <radI:RadMaskedTextBox runat="server" ID="tbZip" Mask="#####" DisplayMask="#####" Width="100px"></radI:RadMaskedTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ControlToValidate="tbZip"></asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                        </tr>                        
                        <tr>
                            <td class="tdlloc"><asp:Label ID="Label8" runat="server" Text="Custom field 1:"></asp:Label></td>
                            <td class="tdcloc">
                                <asp:TextBox ID="tbCustomField1" runat="server" MaxLength="100"  Width="350px"></asp:TextBox>                                
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdlloc"><asp:Label ID="Label9" runat="server" Text="Custom field 2:"></asp:Label></td>
                            <td class="tdcloc">
                                <asp:TextBox ID="tbCustomField2" runat="server" MaxLength="100" Width="350px"></asp:TextBox>                                
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdlloc"><asp:Label ID="Label10" runat="server" Text="Custom field 3:"></asp:Label></td>
                            <td class="tdcloc">
                                <asp:TextBox ID="tbCustomField3" runat="server" MaxLength="100" Width="350px"></asp:TextBox>                                
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdlloc"><asp:Label ID="Label11" runat="server" Text="Custom field 4:"></asp:Label></td>
                            <td class="tdcloc">
                                <asp:TextBox ID="tbCustomField4" runat="server" MaxLength="100" Width="350px"></asp:TextBox>                                
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdlloc"><asp:Label ID="Label12" runat="server" Text="Custom field 5:"></asp:Label></td>
                            <td class="tdcloc">
                                <asp:TextBox ID="tbCustomField5" runat="server" MaxLength="100" Width="350px"></asp:TextBox>                                
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdlloc"><asp:Label ID="Label13" runat="server" Text="Custom field 6:"></asp:Label></td>
                            <td class="tdcloc">
                                <asp:TextBox ID="tbCustomField6" runat="server" MaxLength="100" Width="350px"></asp:TextBox>                                
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdlloc"><asp:Label ID="Label14" runat="server" Text="Custom field 7:"></asp:Label></td>
                            <td class="tdcloc">
                                <asp:TextBox ID="tbCustomField7" runat="server" MaxLength="100" Width="350px"></asp:TextBox>                                
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdlloc"><asp:Label ID="Label15" runat="server" Text="Custom field 8:"></asp:Label></td>
                            <td class="tdcloc">
                                <asp:TextBox ID="tbCustomField8" runat="server" MaxLength="100" Width="350px"></asp:TextBox>                                
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdlloc"><asp:Label ID="Label16" runat="server" Text="Custom field 9:"></asp:Label></td>
                            <td class="tdcloc">
                                <asp:TextBox ID="tbCustomField9" runat="server" MaxLength="100" Width="350px"></asp:TextBox>                                
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdlloc"><asp:Label ID="Label17" runat="server" Text="Custom field 10:"></asp:Label></td>
                            <td class="tdcloc">
                                <asp:TextBox ID="tbCustomField10" runat="server" MaxLength="100" Width="350px"></asp:TextBox>                                
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr style="padding-top:10px;">
                            <td class="tdlloc">&nbsp;</td>
                            <td class="tdcloc">
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"  />
                            </td>
                            <td>&nbsp;</td>                        
                        </tr>
                    </table>
                    </div>
                    </asp:Panel>
                </radspl:radpane>   
            </radspl:radsplitter>
        </td>
    </tr>
</table>
