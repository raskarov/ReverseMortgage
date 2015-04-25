<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditRuleTask.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditRuleTask" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<script language="javascript" type="text/javascript" defer="defer">
<!--
function ValidateTask(o1,o1err,o2,o2err,o3,o3err,o4,o4err,o5,o5err){
    var res = true;
    if(o1.value==''){
        o1err.innerHTML='*';
        res = false;
    }else{
        o1err.innerHTML='';
    }
    if(o2.value==''){
        o2err.innerHTML='*';
        res = false;
    }else{
        o2err.innerHTML='';
    }
    if(o3.value=='0'){
        o3err.innerHTML='*';
        res = false;
    }else{
        o3err.innerHTML='';
    }
    if(o4.value=='0'){
        o4err.innerHTML='*';
        res = false;
    }else{
        o4err.innerHTML='';
    }
    if(o5.value=='0'){
        o5err.innerHTML='*';
        res = false;
    }else{
        o5err.innerHTML='';
    }
    rturn res;
}
-->
</script>
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
<tr style="padding-top:5px">
    <td>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="width:180px;padding-right:3px" align="right">
                    <asp:Label ID="Label1" runat="server" Text="Title:" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:300px">
                    <asp:TextBox ID="tbTitle" runat="server" Width="300px" MaxLength="100"></asp:TextBox>
                </td>
                <td style="padding-left:2px"><asp:Label ID="tbTitleErr" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr valign="top">
                <td style="padding-right:3px" align="right">
                    <asp:Label ID="Label2" runat="server" Text="Description:" SkinID="AdminLabel"></asp:Label>
                </td>
                <td style="width:300px">
                    <asp:TextBox ID="tbDescription" runat="server" TextMode="MultiLine" Rows="3"  Width="100%"></asp:TextBox>
                </td>
                <td style="padding-left:2px"><asp:Label ID="tbDescriptionErr" runat="server" ForeColor="Red"></asp:Label></td>            
            </tr>
            <tr>
                <td style="padding-right:3px" align="right">
                    <asp:Label ID="Label3" runat="server" Text="Task type" SkinID="AdminLabel"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTaskType" runat="server" Width="100%" AutoPostBack="false" ></asp:DropDownList>
                </td>
                <td style="padding-left:2px"><asp:Label ID="ddlTaskTypeErr" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>            
            <tr>
                <td style="padding-right:3px" align="right">
                    <asp:Label ID="Label4" runat="server" Text="Info source" SkinID="AdminLabel"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlInfoSource" runat="server" Width="100%" AutoPostBack="false" ></asp:DropDownList>
                </td>
                <td style="padding-left:2px"><asp:Label ID="ddlInfoSourceErr" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>            
            <tr>
                <td style="padding-right:3px" align="right">
                    <asp:Label ID="Label5" runat="server" Text="Task difficulty" SkinID="AdminLabel"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDifficulty" runat="server" Width="100%" AutoPostBack="false" ></asp:DropDownList>
                </td>
                <td style="padding-left:2px"><asp:Label ID="ddlDifficultyErr" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>            
            <tr style="padding-top:5px;">
                <td>&nbsp</td>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>                
                            <td align="left"><asp:Button ID="btnCancel" runat="server" Text="Cancel" SkinID="AdminButton" OnClick="btnCancel_Click"/></td>
                            <td align="right"><asp:Button ID="btnSave" runat="server" Text="Save" SkinID="AdminButton" OnClick="btnSave_Click" /></td>                        
                        </tr>
                    </table>
                </td>
                <td>&nbsp</td>
            </tr> 
        </table>
    </td>
</tr>
<tr style="padding-top:5px">
    <td>
    <radG:RadGrid ID="G" runat="server" AllowPaging="false" AllowSorting="False" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" OnItemCommand="G_ItemCommand" OnItemDataBound="G_ItemDataBound">
        <ClientSettings>
        <Resizing AllowColumnResize="True" EnableRealTimeResize="false"/>
        </ClientSettings>
        <MasterTableView AllowNaturalSort="False">
            <Columns>
                <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Field">
                    <ItemTemplate>
                                <asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="120px"/>
                </radG:GridTemplateColumn>                    
                <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Description">
                    <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="140px"/>
                </radG:GridTemplateColumn>                                    
                <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Type">
                    <ItemTemplate>
                                <asp:Label ID="lblTaskType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TaskType") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="100px"/>
                </radG:GridTemplateColumn>
                <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="InfoSource">
                    <ItemTemplate>
                                <asp:Label ID="lblInfoSource" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InfoSource") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="60px"/>
                </radG:GridTemplateColumn>
                <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Difficulty">
                    <ItemTemplate>
                                <asp:Label ID="lblDifficulty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TaskDifficulty") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="60px"/>
                </radG:GridTemplateColumn>                
                <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Action" Resizable="False" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton  id="btnEdit" ImageUrl="~/images/btn_grd_edit.gif" CommandName="edittask" AlternateText="Edit user" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />                    
                        <asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="delete" AlternateText="Delete role" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" Width="50px" />
                </radG:GridTemplateColumn>                        
           </Columns>
        </MasterTableView>
        <PagerStyle HorizontalAlign="Left" Mode="NumericPages" PagerTextFormat="" />
        </radG:RadGrid>    
    </td>
</tr>
<tr style="height:10px"><td>&nbsp;</td></tr> 
<tr>
    <td align="center"><asp:Button ID="btnClose" runat="server" Text="Close"  SkinID="AdminButton" OnClick="btnClose_Click"/></td>
</tr>
</table>

