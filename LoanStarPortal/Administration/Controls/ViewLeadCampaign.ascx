<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewLeadCampaign.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewLeadCampaign" %>
<div style="padding-left:10px;padding-top:10px;">
<table border="0" width="97%" cellpadding="0" cellspacing="0">
    <tr>    
        <td>
            <asp:GridView ID="gLeadCampaign" runat="server" SkinID="TotalGrid" AutoGenerateColumns = "false" EnableViewState = "true" AllowPaging="false" AllowSorting="true" 
                OnRowDeleting = "G_RowDeleting"
                OnRowCancelingEdit="G_RowCancel" 
                OnRowEditing="G_RowEditing" 
                OnRowUpdating="G_RowUpdating" 
                OnRowDataBound="G_RowDataBound" 
                EmptyDataText="No records to display" 
                OnRowCommand="G_RowCommand" 
                OnSorting="G_Sorting" 
                OnPageIndexChanged="G_PageIndexChanged" 
                OnPageIndexChanging="G_PageIndexChanging"
            >
                <columns>
                    <asp:TemplateField HeaderText="On/Off">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblState" EnableViewState="false" Text='<%# GetOnOffState(Container.DataItem) %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" Width="30px" />
                    </asp:TemplateField>                
                    <asp:TemplateField HeaderText="Title" SortExpression="Title">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblTitle" EnableViewState="false" Text='<%# DataBinder.Eval(Container.DataItem,"Title") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" Width="90%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgEdit" CommandName="Edit" CommandArgument='<%# GetCurrentRow() %>' runat="server" ImageUrl="~/images/edit.gif"/>
                            <asp:ImageButton ID="imgDelete" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' runat="server" ImageUrl="~/images/delete.gif"/>
                            <asp:ImageButton ID="imgUpdate" CommandName="Update" runat="server" ImageUrl="~/images/update.gif" Visible="false"/>
                            <asp:ImageButton ID="imgCancel" CommandName="Cancel" runat="server" CausesValidation="false" ImageUrl="~/images/cancel.gif" Visible="false"/>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label runat="server" ID="lbl" EnableViewState="false"></asp:Label>
                        </FooterTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" Width="40px" />
                    </asp:TemplateField>
                </columns>
            </asp:GridView>
        </td>
    </tr>
</table>
</div>