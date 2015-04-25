<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditManagerRelation.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditManagerRelation" %>
<div style="padding-left:5px;padding-top:20px;padding-bottom:15px;">
<table border="0" cellspacing="0" cellpadding="0" align="center" style="width:100%" id="tblManager" runat="server">
    <tr>
        <td class="tdlrole"><asp:Label ID="lblRoleName" runat="server" Text="Manager:" ></asp:Label></td>
        <td class="tdcrole">
            <asp:DropDownList ID="ddlManager" runat="server" Width="250px"></asp:DropDownList>
            <asp:Label ID="lblManagerErr" runat="server" Text="*" ForeColor="red"></asp:Label>
        </td>
    </tr>
</table>
<table border="0" cellspacing="0" cellpadding="0" align="center" style="width:100%" runat="server" id="tblEmployee">
    <tr>
        <td><asp:Label ID="Label1" runat="server" Text="Employees" Font-Bold="true"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="gEmployee" runat="server" SkinID="TotalGrid" AutoGenerateColumns = "false" EnableViewState = "true" AllowPaging="False" AllowSorting="true" 
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
                <asp:TemplateField HeaderText="Name" SortExpression="Name">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblName" EnableViewState="false" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                        <asp:DropDownList ID="ddlEmployee" runat="server" Visible="false"/>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Role" SortExpression="RoleName">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblRoleName" EnableViewState="false" Text='<%# DataBinder.Eval(Container.DataItem,"RoleName") %>'></asp:Label>                        
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEdit" CommandName="Edit" CommandArgument='<%# GetCurrentRow() %>' runat="server" ImageUrl="~/images/edit.gif"/>
                        <asp:ImageButton ID="imgDelete" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' runat="server" ImageUrl="~/images/delete.gif"/>
                        <asp:ImageButton ID="imgUpdate" CommandName="Update" runat="server" ImageUrl="~/images/update.gif" Visible="false" />
                        <asp:ImageButton ID="imgCancel" CommandName="Cancel" runat="server" CausesValidation="false" ImageUrl="~/images/cancel.gif" Visible="false"/>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label runat="server" ID="lbl" EnableViewState="false"></asp:Label>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" Width="40px" />
                </asp:TemplateField>
            </columns>
            </asp:GridView>
        </td>
    </tr>
</table>
</div>