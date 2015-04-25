<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FollowupDates.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.FollowupDates" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<table border="0" width="140px" cellpadding="0" cellspacing="0">
    <tr>
        <td><asp:Label ID="Label5" runat="server" Text="Follow up dates" Font-Bold="true"></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblErr" runat="server" Text="" ForeColor="Red"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="gShedule" runat="server" SkinID="TotalGrid" AutoGenerateColumns = "false" EnableViewState = "true" AllowPaging="false" ShowHeader="False"
                OnRowCommand ="G_RowCommand"
                OnRowDeleting = "G_RowDeleting"
                OnRowCancelingEdit="G_RowCancel" 
                OnRowEditing="G_RowEditing" 
                OnRowUpdating="G_RowUpdating" 
                OnRowDataBound="G_RowDataBound" 
                EmptyDataText="No records to display" 
                OnPageIndexChanged="G_PageIndexChanged" 
                OnPageIndexChanging="G_PageIndexChanging" 
            >
                <columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblDaysOffset" EnableViewState="false" Text='<%# GetDaysOffset(Container.DataItem) %>'></asp:Label>
                            <radI:RadNumericTextBox Type="Number" ID="tbDayOffset" runat="server" MinValue="0" MaxValue="1000" Width="60px" NumberFormat-DecimalDigits="0" Visible="false" Height="95%"></radI:RadNumericTextBox>
                            &nbsp;<asp:RequiredFieldValidator ID="rfDayOffset" runat="server" ErrorMessage="*" ControlToValidate="tbDayOffset" Visible="false"></asp:RequiredFieldValidator>
                        </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />                                
                    </asp:TemplateField>                
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgEdit" CommandName="Edit" CommandArgument='<%# GetCurrentRow() %>' runat="server" ImageUrl="~/images/edit.gif"/>
                            <asp:ImageButton ID="imgDelete" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' runat="server" ImageUrl="~/images/delete.gif"/>
                            <asp:ImageButton ID="imgUpdate" CommandName="Update" runat="server" ImageUrl="~/images/update.gif" Visible="false"/>
                            <asp:ImageButton ID="imgCancel" CommandName="Cancel" runat="server" CausesValidation="false" ImageUrl="~/images/cancel.gif" Visible="false"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" Width="60px"/>
                        <FooterTemplate>
                            <asp:Label runat="server" ID="lbl" EnableViewState="false"></asp:Label>
                        </FooterTemplate>
                        <ItemStyle HorizontalAlign="Center" />                                    
                    </asp:TemplateField>
                </columns>
            </asp:GridView>                    
        </td>
    </tr>
</table>
