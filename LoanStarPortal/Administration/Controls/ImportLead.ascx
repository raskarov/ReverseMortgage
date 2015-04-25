<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportLead.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ImportLead" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register TagPrefix="rada" Namespace="Telerik.WebControls" Assembly="RadAjax.NET2" %>
 <script language="javascript" type="text/javascript">
<!--
function EnableNext(o,trId,btnId){
    if(o.value=='0'){
        s='none';
    }else{
        s='inline';
    }
    var t = document.getElementById(trId); 
    if(t!=null) t.style.display=s;
    var b = document.getElementById(btnId);
    if(b!=null){
        if(o.value=='0'){
            b.setAttribute('disabled','true');
        }else{
            b.removeAttribute('disabled');
        }
    }
}
function EnableImport(o,btnId){
    if(o.value=='0'){
        s='none';
    }else{
        s='inline';
    }
    var b = document.getElementById(btnId);
    if(b!=null){
        if(o.value=='0'){
            b.setAttribute('disabled','true');
        }else{
            b.removeAttribute('disabled');
        }
    }
}
function ShowError(err){
    alert(err);
}
-->
</script>
 <table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td class="importleadlbl">&nbsp;</td>
                    <td><asp:Label ID="lblHeader" runat="server" Text="" SkinID="AdminHeader"></asp:Label></td>
                </tr>
                <tr>
                    <td class="importleadlbl">&nbsp;</td>
                    <td><asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label></td>
                </tr>                
            </table>
        </td>
    </tr> 
    <tr>
        <td>
            <div runat="server" id="divStep1">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr style="padding-top:10px;">
                    <td class="importleadlbl">
                        <asp:Label ID="Label1" runat="server" Text="Choose format"></asp:Label>
                    </td>
                    <td class="importleadctl">
                        <asp:DropDownList ID="ddlImportSource" runat="server"  Width="210px" EnableViewState="false"></asp:DropDownList>
                    </td>        
                </tr>
                <tr runat="server" id="trUpload" style="display:none;padding-top:10px;">
                    <td class="importleadlbl">
                        <asp:Label ID="Label2" runat="server" Text="File :"></asp:Label>
                    </td>
                    <td class="importleadctl">
                        <asp:FileUpload ID="UploadImportData" Width="290px" runat="server"/>
                    </td>        
                </tr>
                <tr style="padding-top:10px;">
                    <td class="importleadlbl">&nbsp;</td>
                    <td class="importleadctl">
                        <asp:Button ID="btnExecuteStep1" runat="server" Text="Next" OnClick="btnExecuteStep1_Click" Enabled="false" Width="80px"/>
                    </td>        
                </tr>
            </table>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <div runat="server" id="divStep2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" >
                <tr>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                            <tr>
                                <td style="width:50%"><asp:Label ID="lblImportedRows" runat="server" Text="" Font-Bold="true"></asp:Label></td>
                                <td align="right"><asp:Label ID="Label3" runat="server" Text="Please select Loan Officer:" ></asp:Label></td>
                                <td align="left" style="padding-left:5px;"><asp:DropDownList ID="ddlLoanOfficer" runat="server"  Width="210px" EnableViewState="false"></asp:DropDownList></td>
                            </tr>
                        </table>                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <radG:RadGrid ID="gImportedRows" Skin="WebBlue" GridLines="None" EnableAJAX="false" runat="server"        
                        Width="99%" PageSize="20"  AllowPaging="True" AutoGenerateColumns="False" 
                        AllowMultiRowEdit="false" 
                        OnItemCommand="gImportedRows_ItemCommand"  
                        >
                        <ClientSettings>
                            <Resizing AllowColumnResize="True" EnableRealTimeResize="false"/>
                        </ClientSettings>
                        <MasterTableView AllowNaturalSort="False" EditMode="InPlace" DataKeyNames="ID" AutoGenerateColumns="False" >
                            <Columns>
                                <radG:GridTemplateColumn HeaderText="First Name" SortExpression="FirstName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FirstName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                                </radG:GridTemplateColumn>
                                <radG:GridTemplateColumn HeaderText="LastName" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblLastName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"LastName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                                </radG:GridTemplateColumn>
                                <radG:GridTemplateColumn HeaderText="Address" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddress1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Address1") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="120px"/>
                                </radG:GridTemplateColumn>
                                <radG:GridTemplateColumn HeaderText="City" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"City") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="80px"/>
                                </radG:GridTemplateColumn>
                                <radG:GridTemplateColumn HeaderText="State" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblState" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"State") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="60px"/>
                                </radG:GridTemplateColumn>
                                <radG:GridTemplateColumn HeaderText="Zip" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblZip" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Zip") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="50px"/>
                                </radG:GridTemplateColumn>
                                <radG:GridTemplateColumn HeaderText="Phone" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblPhone" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Phone") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="50px"/>
                                </radG:GridTemplateColumn>
                                <radG:GridTemplateColumn HeaderText="Email" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"EmailAddress") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="50px"/>
                                </radG:GridTemplateColumn>                                
                                <radG:GridButtonColumn ConfirmText="Delete this row?" ButtonType="ImageButton" ImageUrl="/Images/Delete.gif" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn">
                                    <HeaderStyle Width="20px" />
                                    <ItemStyle HorizontalAlign="Center"/>
                                </radG:GridButtonColumn>                        
                            </Columns>
                        </MasterTableView>
                        <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
                        </radG:RadGrid>                                
                    </td>
                </tr>
                <tr id="trBadrowsheader" runat="server" style="padding-top:10px;">
                    <td><asp:Label ID="lblBadRows" runat="server" Text="" Font-Bold="true"></asp:Label></td>
                </tr>
                <tr id="trBadRowsGrid" runat="server">
                    <td>
                        <radG:RadGrid ID="gBadRows" Skin="WebBlue" GridLines="None" EnableAJAX="false" runat="server"        
                        Width="70%" PageSize="20"  AllowPaging="True" AutoGenerateColumns="False" 
                        AllowMultiRowEdit="false" 
                        OnItemCommand="gBadRows_ItemCommand"  
                        >
                        <ClientSettings>
                            <Resizing AllowColumnResize="True" EnableRealTimeResize="false"/>
                        </ClientSettings>
                        <MasterTableView AllowNaturalSort="False" EditMode="InPlace" DataKeyNames="ID" AutoGenerateColumns="False" >
                            <Columns>
                                <radG:GridTemplateColumn HeaderText="Row #" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblRowNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RowNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="20px"/>
                                </radG:GridTemplateColumn>                            
                                <radG:GridTemplateColumn HeaderText="First Name" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FirstName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                                </radG:GridTemplateColumn>
                                <radG:GridTemplateColumn HeaderText="LastName" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblLastName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"LastName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                                </radG:GridTemplateColumn>
                                <radG:GridTemplateColumn HeaderText="Errors" >
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbError" runat="server" CommandName="viewError" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' >View errors</asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="50px"/>
                                </radG:GridTemplateColumn>                                
                            </Columns>
                        </MasterTableView>
                        <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
                        </radG:RadGrid>                                
                    </td>
                </tr>
                <tr style="padding-top:10px;">
                    <td>
                        <table width="0" cellpadding="0" cellspacing="0" style="width:100%">
                            <tr>
                                <td style="width:20%">&nbsp;</td>
                                <td><asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Width="80px"/></td>
                                <td><asp:Button ID="btnStep2" runat="server" Text="Import" OnClick="btnExecuteStep2_Click" Width="80px"/></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </div>
        </td>
    </tr>
 </table>
 
