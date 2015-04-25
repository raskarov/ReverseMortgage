<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoanOfficerStateLicensing.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.LoanOfficerStateLicensing" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<div style="padding-left:15px;padding-top:10px;">
<table border="0" cellspacing="0" cellpadding="0" align="center" style="width:100%" runat="server" id="tblEmployee">
    <tr>
        <td>
            <asp:GridView ID="gStateLicense" runat="server" SkinID="TotalGrid1" AutoGenerateColumns = "false" EnableViewState = "true" AllowPaging="false" AllowSorting="true"  Width="560px"
                OnRowDeleting = "gStateLicense_RowDeleting"
                OnRowCancelingEdit="gStateLicense_RowCancel" 
                OnRowEditing="gStateLicense_RowEditing" 
                OnRowUpdating="gStateLicense_RowUpdating" 
                OnRowDataBound="gStateLicense_RowDataBound" 
                EmptyDataText="No records to display" 
                OnRowCommand="gStateLicense_RowCommand" 
                OnSorting="gStateLicense_Sorting" 
                OnPageIndexChanged="gStateLicense_PageIndexChanged" 
                OnPageIndexChanging="gStateLicense_PageIndexChanging"
            >
            <columns>
                <asp:TemplateField HeaderText="State" SortExpression="StateName">
                    <ItemTemplate>
                        <asp:Label ID="lblLicenseState" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StateName") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="License Number" SortExpression="LicenseNumber">
                    <ItemTemplate>
                        <asp:Label ID="lblLicenseNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LicenseNumber") %>'></asp:Label>
                        <asp:TextBox ID="tbLicenseNumber" runat="server" MaxLength="50" Width="90%" Visible="false"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfLicenseNumber" runat="server" ErrorMessage="*" ControlToValidate="tbLicenseNumber" Width="5%"  Visible="false"></asp:RequiredFieldValidator>
                    </ItemTemplate>                                        
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" Width="250px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Expiration Date" SortExpression="ExpirationDate">
                    <ItemTemplate>
                        <asp:Label ID="lblExpirationDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ExpirationDate","{0:MM/dd/yyyy}") %>'></asp:Label>
                        <radI:RadDateInput runat="server" ID="tbExpirationDate" Visible="false" Width="90px"></radI:RadDateInput>                                                
                        <asp:RequiredFieldValidator ID="rfExpirationDate" runat="server" ErrorMessage="*" ControlToValidate="tbExpirationDate" Width="5%"  Visible="false"></asp:RequiredFieldValidator>
                    </ItemTemplate>                                        
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" Width="120px" />
                </asp:TemplateField>                                        
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEdit" CommandName="Edit" CommandArgument='<%# GetCurrentRow() %>' runat="server" ImageUrl="~/images/edit.gif"/>
                        <asp:ImageButton ID="imgDelete" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' runat="server" ImageUrl="~/images/delete.gif"/>
                        <asp:ImageButton ID="imgUpdate" CommandName="Update" runat="server" ImageUrl="~/images/update.gif" Visible="false"/>
                        <asp:ImageButton ID="imgCancel" CommandName="Cancel" runat="server" CausesValidation="false" ImageUrl="~/images/cancel.gif" Visible="false"/>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" Width="40px" />
                </asp:TemplateField>
            </columns>
            </asp:GridView>
        </td>    
    </tr>
</table>
</div>