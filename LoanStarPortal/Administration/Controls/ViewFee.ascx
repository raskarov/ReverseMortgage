<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewFee.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewFee" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<script language="javascript" type="text/javascript">
<!--
function ResetDdl(o){
    if(o.value=='0')retrurn;
    var name=o.id.substring(0,o.id.length-1);
    for(var i=0;i<3;i++){
        var ddlid=name+i;
        var d = document.getElementById(name+i);
        if(d.id!=o.id){
            if(o.value==d.value){
                d.value='0';
            }
        }
    }
}
-->
</script>

<table cellpadding="0" cellspacing="0" width="100%" border="0">
    <tr>
        <td>
            <asp:GridView ID="G" runat="server" SkinID="TotalGrid" AutoGenerateColumns = "false" EnableViewState = "true" AllowPaging="false" AllowSorting="true" idth="99%"
                OnRowCancelingEdit="G_RowCancel" 
                OnRowEditing="G_RowEditing" 
                OnRowUpdating="G_RowUpdating" 
                OnRowDataBound="G_RowDataBound" 
                OnRowCommand="G_RowCommand" 
                OnSorting="G_Sorting" 
                OnPageIndexChanged="G_PageIndexChanged" 
                OnPageIndexChanging="G_PageIndexChanging"            
            >
            <columns>
            <asp:TemplateField HeaderText="Fee type" SortExpression="Name">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblOriginator" EnableViewState="false" Text='<%# DataBinder.Eval(Container.DataItem, "name") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
                <HeaderStyle HorizontalAlign="Left" Width="20%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="First choice provider">
                <ItemTemplate>
                    <asp:Label ID="lblDefault_0" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FirstProviderName") %>'></asp:Label>
                    <asp:DropDownList ID="ddlDefault_0" runat="server" AutoPostBack="false" Visible="false"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
                <HeaderStyle HorizontalAlign="Left" Width="25%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Second choice provider">
                <ItemTemplate>
                    <asp:Label ID="lblDefault_1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SecondProviderName") %>'></asp:Label>
                    <asp:DropDownList ID="ddlDefault_1" runat="server" AutoPostBack="false" Visible="false"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
                <HeaderStyle HorizontalAlign="Left" Width="25%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Third choice provider">
                <ItemTemplate>
                    <asp:Label ID="lblDefault_2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ThirdProviderName") %>'></asp:Label>
                    <asp:DropDownList ID="ddlDefault_2" runat="server" AutoPostBack="false" Visible="false"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
                <HeaderStyle HorizontalAlign="Left" Width="25%" />
            </asp:TemplateField>                        
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:ImageButton ID="imgEdit" CommandName="Edit" CommandArgument='<%# GetCurrentRow() %>' runat="server" ImageUrl="~/images/edit.gif"/>
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
