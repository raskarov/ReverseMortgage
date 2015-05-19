<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewVendors.ascx.cs" Inherits="LoanStarPortal.Controls.ViewVendors" %>
<%@ Register Src="VendorContacts.ascx" TagName="VendorContacts" TagPrefix="uc1" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<script language="javascript" type="text/javascript">

    function ValidateAffiliation(src, arg) {
        var o = document.getElementById(src.id);
        o = document.getElementById(o.getAttribute('controltovalidate'));
        var p = GetParentTr(o);
        if (p) {
            if (p.style.display == 'block') {
                arg.IsValid = o.value != '';
            } else {
                arg.IsValid = true;
            }
        }
    }
    function GetParentTr(o) {
        var p = o;
        while (true) {
            if (!p || !p.parentElement) return null;
            p = p.parentElement;
            if (p.tagName.toLowerCase() == 'tr') {
                return p;
            }
        }
        return null;
    }
    function SetAffiliateRow(o, tr1) {
        var s = o.value;
        var vis = 'block';
        if (s == 0) {
            vis = 'none';
        }
        var t = document.getElementById(tr1);
        if (t != null) {
            t.style.display = vis;
        }
    }

</script>
<div style="padding-left: 10px; padding-top: 5px;">
    <table border="0" width="100%" cellpadding="0" cellspacing="0">
        <tr id="gridtr" runat="server" visible="false">
            <td>
                <radG:RadGrid ID="gVendors" Skin="WebBlue" runat="server" CssClass="RadGrid" GridLines="None" AllowPaging="True" PageSize="20" 
                    AllowSorting="true" Width="99%" AutoGenerateColumns="False"
                    EnableAJAX="False" ShowStatusBar="false" HorizontalAlign="NotSet" OnItemCommand="gVendors_ItemCommand" 
                    OnSortCommand="gVendors_SortCommand" OnPageIndexChanged="gVendors_PageIndexChanged">
                    <MasterTableView GridLines="None" DataKeyNames="ID">
                        <Columns>
                            <radG:GridBoundColumn UniqueName="Name" HeaderText="Company" DataField="Name" SortExpression="Name">
                                <HeaderStyle Width="59%" />
                            </radG:GridBoundColumn>
                            <radG:GridBoundColumn UniqueName="PrimaryContact" HeaderText="Contact " DataField="PCName" SortExpression="PCName">
                                <HeaderStyle Width="40%" />
                            </radG:GridBoundColumn>
                            <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Details" Resizable="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnViewDetails" ImageUrl="~/images/Edit.gif" CommandName="ViewDetails" AlternateText="View details" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                <ItemStyle HorizontalAlign="Center" />
                            </radG:GridTemplateColumn>
                        </Columns>
                        <ExpandCollapseColumn Visible="False">
                            <HeaderStyle Width="19px" />
                        </ExpandCollapseColumn>
                        <RowIndicatorColumn Visible="False">
                            <HeaderStyle Width="20px" />
                        </RowIndicatorColumn>
                    </MasterTableView>
                    <PagerStyle Mode="NumericPages" PagerTextFormat="" />
                </radG:RadGrid>
            </td>
        </tr>
        <tr id="detailstr" runat="server" visible="false">
            <td>
                <radTS:RadTabStrip ID="VendorInfo" runat="server" Skin="Outlook" MultiPageID="RadMultiPage1" SelectedIndex="0" CausesValidation="false" Orientation="HorizontalTopToBottom">
                    <Tabs>
                        <radTS:Tab Text="Basic Information" Value="BasicInfo" ID="tabBasicInfo" runat="server"></radTS:Tab>
                        <radTS:Tab Text="Contacts" Value="Contacts" ID="tabContacts" runat="server"></radTS:Tab>
                        <radTS:Tab Text="Affiliation" Value="Contacts" ID="tab1" runat="server"></radTS:Tab>
                    </Tabs>
                </radTS:RadTabStrip>
                <radTS:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Height="85%" EnableViewState="False">
                    <radTS:PageView ID="pvbasicInfo" runat="server" EnableViewState="False">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; padding-top: 5px">
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label2" runat="server" Text="Company Name"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <asp:TextBox ID="tbCompanyName" runat="server" MaxLength="256" Width="280px" Enabled="false"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label1" runat="server" Text="Corporate Address 1"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <asp:TextBox ID="tbCorporateAddress1" runat="server" MaxLength="256" Width="280px" Enabled="false"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label16" runat="server" Text="Corporate Address 2"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <asp:TextBox ID="tbCorporateAddress2" runat="server" MaxLength="256" Width="280px" Enabled="false"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label27" runat="server" Text="Main phone"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <radI:RadMaskedTextBox runat="server" ID="tbCompanyPhone" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px" Enabled="false"></radI:RadMaskedTextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label15" runat="server" Text="Main fax"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <radI:RadMaskedTextBox runat="server" ID="tbCompanyFax" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px" Enabled="false"></radI:RadMaskedTextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label28" runat="server" Text="Email"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <asp:TextBox ID="tbCompanyEmail" runat="server" MaxLength="512" Width="280px" Enabled="false"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label18" runat="server" Text="City"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <asp:TextBox ID="tbCompanyCity" runat="server" MaxLength="50" Width="280px" Enabled="false"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label19" runat="server" Text="State"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <asp:DropDownList ID="ddlCompanyState" runat="server" Width="200px" EnableViewState="false" Enabled="false"></asp:DropDownList>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label20" runat="server" Text="Zip"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <asp:TextBox ID="tbCompanyZip" runat="server" MaxLength="20" Width="100px" Enabled="false"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label4" runat="server" Text="Billing address 1"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <asp:TextBox ID="tbBillingAddress1" runat="server" MaxLength="256" Width="280px" Enabled="false"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label3" runat="server" Text="Billing address 2"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <asp:TextBox ID="tbBillingAddress2" runat="server" MaxLength="256" Width="280px" Enabled="false"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label30" runat="server" Text="City"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <asp:TextBox ID="tbBillingCity" runat="server" MaxLength="50" Width="280px" Enabled="false"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label31" runat="server" Text="State"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <asp:DropDownList ID="ddlBillingState" runat="server" Width="200px" EnableViewState="false" Enabled="false"></asp:DropDownList>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label32" runat="server" Text="Zip"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <radI:RadMaskedTextBox runat="server" ID="tbBillingZip" Mask="#####" DisplayMask="#####" Width="100px" Enabled="false"></radI:RadMaskedTextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label5" runat="server" Text="Primary Contact"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <asp:TextBox ID="tbPrimaryContact" runat="server" MaxLength="256" Width="280px" Enabled="false"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label6" runat="server" Text="Phone 1"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <radI:RadMaskedTextBox runat="server" ID="tbPCPhone1" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px" Enabled="false"></radI:RadMaskedTextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label7" runat="server" Text="Phone 2"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <radI:RadMaskedTextBox runat="server" ID="tbPCPhone2" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px" Enabled="false"></radI:RadMaskedTextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label8" runat="server" Text="Email"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <asp:TextBox ID="tbPCEmail" runat="server" MaxLength="512" Width="280px" Enabled="false"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label9" runat="server" Text="Secondary Contact"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <asp:TextBox ID="tbSecondaryContact" runat="server" MaxLength="256" Width="280px" Enabled="false"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label10" runat="server" Text="Phone 1"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <radI:RadMaskedTextBox runat="server" ID="tbSCPhone1" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px" Enabled="false"></radI:RadMaskedTextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label11" runat="server" Text="Phone 2"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <radI:RadMaskedTextBox runat="server" ID="tbSCPhone2" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px" Enabled="false"></radI:RadMaskedTextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label12" runat="server" Text="Email"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <asp:TextBox ID="tbSCEmail" runat="server" MaxLength="512" Width="280px" Enabled="false"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label13" runat="server" Text="License expire date"></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <radI:RadDateInput runat="server" ID="dtLicenseExpdate" Enabled="false"></radI:RadDateInput>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label14" runat="server" Text="Disable vendor "></asp:Label>
                                </td>
                                <td class="vendorbasicinfoctl">
                                    <asp:CheckBox ID="cbDisabled" runat="server" Enabled="false" />
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </radTS:PageView>
                    <radTS:PageView ID="pvContacts" runat="server" EnableViewState="False">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr>
                                <td style="padding: 5px;">
                                    <uc1:VendorContacts ID="VendorContacts1" runat="server"></uc1:VendorContacts>
                                </td>
                            </tr>
                        </table>
                    </radTS:PageView>
                    <radTS:PageView ID="pvAffiliation" runat="server" EnableViewState="False">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; padding-top: 10px">
                            <tr>
                                <td class="vendorbasicinfolbl">&nbsp;</td>
                                <td colspan="2">
                                    <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="red"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label17" runat="server" Text="Affiliation:"></asp:Label></td>
                                <td class="vendorbasicinfoctl">
                                    <asp:DropDownList ID="ddlAffiliation" runat="server" Width="120px"></asp:DropDownList></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr id="trAffiliation" runat="server">
                                <td class="vendorbasicinfolbl">
                                    <asp:Label ID="Label21" runat="server" Text="Description:"></asp:Label></td>
                                <td class="vendorbasicinfoctl">
                                    <asp:TextBox ID="tbAffiliation" runat="server" MaxLength="256" Width="350px"></asp:TextBox>&nbsp;<asp:CustomValidator
                                        ID="CustomValidator1" runat="server" ErrorMessage="*" ClientValidationFunction="ValidateAffiliation" ControlToValidate="tbAffiliation" ValidateEmptyText="true"></asp:CustomValidator></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Button ID="btnSaveAffiliation" runat="server" Text="Save" OnClick="btnSaveAffiliation_Click" CausesValidation="true" /></td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </radTS:PageView>
                </radTS:RadMultiPage>
            </td>
        </tr>
        <tr runat="server" id="buttontr" style="padding-top: 10px; padding-left: 10px;">
            <td>
                <asp:Button ID="btnBack" runat="server" Text="Close" OnClick="btnBack_Click" CausesValidation="false" /></td>
        </tr>
    </table>
</div>