<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditDocTemplate.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditDocTemplate" %>
<%@ Register Src="DocFieldMapping.ascx" TagName="DocFieldMapping" TagPrefix="uc1" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<script language="javascript" type="text/javascript">
function CheckAllDeny(chk){    
    var checkboxes = <%=GridVersion.ClientID %>.MasterTableView.Control.getElementsByTagName("INPUT");   
    var index;   
    for(index = 0; index < checkboxes.length; index++){   
        if(checkboxes[index]!=chk){
            if(checkboxes[index].checked){   
                checkboxes[index].checked = false;   
            }     
        }   
    }
}   
</script>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
<tr>
    <td colspan="2" align="center">
        <asp:Label ID="lblHeader" runat="server" Text="" SkinID="AdminHeader"></asp:Label>
    </td>
</tr>
<tr style="height:20px">
    <td colspan="2" align="center">
        <asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
    </td>
</tr>
<asp:Panel runat="server" ID="PanelStep1">
    <tr>
        <td class="edituserlabeltd">Title: </td>
        <td>
            <asp:TextBox ID="txtTitle" Width="150px" runat="server" MaxLength="100" SkinID="AdminInput"></asp:TextBox>
        </td>
    </tr>
    <tr runat="server" id="tr2">
        <td class="edituserlabeltd">Group: </td>
        <td>
            <asp:DropDownList ID="ddlSelectDocGroup" Width="150px" runat="server"></asp:DropDownList>
        </td>
    </tr>
    <asp:Panel runat="server" ID="PanelAddDoc">
    <tr runat="server" id="tr1">
        <td class="edituserlabeltd">Version: </td>
        <td>
            <asp:TextBox ID="txtVersion" Width="150px" runat="server" MaxLength="100" SkinID="AdminInput"></asp:TextBox>
        </td>
    </tr>
    <tr runat="server" id="trAddDoc">
        <td class="edituserlabeltd">File: </td>
        <td>
            <asp:FileUpload ID="UploadPdfFile" Width="600px" runat="server" />
        </td>
    </tr>
    </asp:Panel>
    <tr style="height:20px">
        <td colspan="2">&nbsp</td>
    </tr>
    <asp:Panel runat="server" ID="PanelEdit">
    <tr>
        <td colspan="2" align="center">
        <radG:RadGrid ID="GridVersion" Skin="WebBlue" GridLines="None" EnableAJAX="false" runat="server"        
            Width="99%"  AllowPaging="false" AutoGenerateColumns="False" 
            AllowAutomaticDeletes="True"
            AllowAutomaticUpdates="True"
            OnItemCommand="GridVersion_ItemCommand" 
            AllowMultiRowEdit="false"
            OnItemDataBound="GridVersion_ItemDataBound"
            >
            <PagerStyle Mode="NextPrevAndNumeric" />
            <MasterTableView Width="100%" CommandItemDisplay="Top" DataKeyNames="ID" HorizontalAlign="NotSet" AutoGenerateColumns="False" EditMode="InPlace">
            <CommandItemTemplate>
                <table border="0" cellspacing="0" cellpadding="0" width="100%">
                    <tr>
                        <td>
                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="InitInsert" Visible='<%# !GridVersion.MasterTableView.IsItemInserted %>'>Add new Version</asp:LinkButton>
                        </td>
                        <td align="right">
                            <asp:LinkButton ID="LinkButton4" runat="server" CommandName="RebindGrid">Refresh</asp:LinkButton>
                       </td>
                    </tr>
                </table>
             </CommandItemTemplate>
             <Columns>
                <radG:GridBoundColumn DataField="id" ReadOnly="true" Visible="false">
                    <HeaderStyle Width="0px" />
                </radG:GridBoundColumn>
                <radG:GridTemplateColumn HeaderText="Version">
                    <HeaderStyle Width="30%" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblVersion" Text='<%# Eval("Version") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtVersionG" runat="server" MaxLength="100" SkinID="AdminInput" Text='<%# Eval("Version") %>' Width="100"></asp:TextBox><asp:RequiredFieldValidator id="Requiredfieldvalidator1" runat="server" controltovalidate="txtVersionG" errormessage="*" display="Dynamic" setfocusonerror="true"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                </radG:GridTemplateColumn>
                <radG:GridTemplateColumn HeaderText="File Name">
                    <HeaderStyle Width="40%" />
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="lnkFile" Text='<%# Eval("FileName") %>' NavigateUrl="" Target="_blank"></asp:HyperLink>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:FileUpload ID="UploadPdfFileG" runat="server" />
                    </EditItemTemplate>
                </radG:GridTemplateColumn>                        
                <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Current">
                    <ItemTemplate>
                        <asp:CheckBox ID = "IsCurrent" runat="server" OnCheckedChanged="IsCurrent_CheckedChanged" AutoPostBack="true"/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID = "Current" runat="server"/>
                    </EditItemTemplate>
                    <HeaderStyle Width="50px" />
                </radG:GridTemplateColumn>                        
                <radG:GridBoundColumn DataField="UploadDate" HeaderText="Date" ReadOnly="true">
                    <HeaderStyle Width="25%" />
                </radG:GridBoundColumn>
                <radg:GridEditCommandColumn ButtonType="ImageButton" UpdateImageUrl="~/Images/Update.gif" 
                    EditImageUrl="~/Images/Edit.gif" InsertImageUrl="~/Images/Insert.gif" 
                    CancelImageUrl="~/Images/Cancel.gif" UniqueName="EditCommandColumn">
                    <HeaderStyle Width="20px" /> 
                </radg:GridEditCommandColumn>
                <radG:GridButtonColumn ButtonType="LinkButton" Text="Map" HeaderText="Map fields" CommandName="MapFilelds" >
                    <HeaderStyle Width="60px" /> 
                </radG:GridButtonColumn>                    
            </Columns>
            <EditFormSettings ColumnNumber="2" CaptionDataField="Version" CaptionFormatString="Edit Version">
                    <FormTableItemStyle Wrap="False"></FormTableItemStyle>
                        <FormCaptionStyle CssClass="EditFormHeader"></FormCaptionStyle>
                        <FormMainTableStyle GridLines="None" CellSpacing="0" CellPadding="3" BackColor="White" Width="100%" />
                        <FormTableStyle CellSpacing="0" CellPadding="2" CssClass="module" Height="110px"
                            BackColor="White" />
                        <FormTableAlternatingItemStyle Wrap="False"></FormTableAlternatingItemStyle>
                        <EditColumn ButtonType="ImageButton" UpdateImageUrl="~/Images/Update.gif" EditImageUrl="~/Images/Edit.gif" InsertImageUrl="~/Images/Insert.gif" CancelImageUrl="~/Images/Cancel.gif" InsertText="Insert" UpdateText="Update" UniqueName="EditCommandColumn1" CancelText="Cancel edit"></EditColumn>
                        <FormTableButtonRowStyle HorizontalAlign="Right" CssClass="EditFormButtonRow"></FormTableButtonRowStyle>
            </EditFormSettings>                    
            </MasterTableView>
            <PagerStyle HorizontalAlign="Center" Mode="NumericPages" />
            </radG:RadGrid> 
        </td>
    </tr>
    <tr style="height:20px">
        <td colspan="2">&nbsp</td>
    </tr>
    </asp:Panel>    
    <tr>
        <td align="center" colspan="2">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                <td align="center">
                    <asp:Button ID="btnBack" runat="server" Text="Back" Width="80px" OnClick="btnBack_Click" CausesValidation="False"/>&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnSaveStep1" runat="server" Text="Next"  Width="80px" OnClick="btnSaveStep1_Click" CausesValidation="true"/>
                </td>
                </tr>
            </table>        
        </td>
    </tr>
</asp:Panel>
<asp:Panel runat="server" ID="PanelStep2">
    <tr>
        <td align="center" class="edituserinputtd" colspan="2" style="width:100%">
             <uc1:DocFieldMapping ID="DocFieldMapping1" runat="server" />
        </td>
    </tr>
    <tr style="height:20px"><td colspan="3">&nbsp</td></tr>
    <tr>
        <td colspan="2">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="center"><asp:Button ID="Button1" runat="server" Text="Finish" Width="80px" CausesValidation="False" OnClick="btnBack_Click"/></td>
                </tr>
            </table>        
        </td>
    </tr>
</asp:Panel>
</table>
