<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Vendors.ascx.cs" Inherits="LoanStarPortal.Controls.Vendors" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>
<%@ Register TagPrefix="radG" Namespace="Telerik.WebControls" Assembly="RadGrid.Net2" %>
<script language="javascript" type="text/javascript">
<!--
function ValidatePhone(src,arg){
    var o = document.getElementById(src.id);
    o=document.getElementById(o.getAttribute('controltovalidate'));
    var p=GetParentDiv(o);
    if(p){
        if(p.style.display=='block'){
            arg.IsValid = o.value!='';
        }else{
            arg.IsValid = true;
        }
    }
}
function GetParentDiv(o){
    var p=o;
    while(true){
        if(!p.parentElement) return null;
        p=p.parentElement;
        if(p.tagName.toLowerCase()=='div'){
            return p;
        }
    }
    return null;
}
function CheckDelete(){
    return confirm('Are you sure you want to delete this vendor?');
}
function ValidateLogin(src, arg ){
    arg.IsValid = arg.Value.length>=6;
}

function ValidateEmail(src, arg ){
    arg.IsValid = arg.Value.length>1;    
    if (arg.IsValid){
        var b = arg.Value.match(/^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i);
        arg.IsValid = b!=null;
    }
}
function SetExtPropertyDiv(o,ids,divid){
    if(ids=='') return;
    var arr=ids.split(",");
    var list = o.getElementsByTagName('input');
    var vis=false;
    for(var i=0;i<list.length;i++){
        if(list[i].type=='checkbox'){
            if(list[i].id.indexOf('_cblFeeType_')>0){
                var p=list[i].parentElement;
                if(p){
                    var id=p.getAttribute('typeid');
                    for(var k=0;k<arr.length;k++){
                        if(id==arr[k]){
                            if(list[i].checked){
                                vis=true;break;
                            }          
                        }
                    }
                }                
            }
        }
    }
    var d = document.getElementById(divid);
    if(d){
        d.style.display=vis?'block':'none';
    }
}
-->
</script>
<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Width="100%" BorderWidth="0" BorderStyle="None" BorderSize="0" Skin="Default" LiveResize="false">
   <radspl:radpane id="TopPane" runat="server" Height="35px" Scrolling="None">
    <div RadResizeStopLookup="true" RadShowStopLookup="true" >
        <div class="paneTitle"><b>Vendors</b></div>
    </div>
   </radspl:radpane>  
   <radspl:radsplitbar id="RadSplitBar3" runat="server" CollapseMode="None" EnableResize="false" Visible="false"/>
   <radspl:radpane id="MiddlePane" runat="server" scrolling="Y" >
   <div RadResizeStopLookup="true" RadShowStopLookup="true" >
    <table border="0" cellpadding="3" cellspacing="3" style="width:99%;">
        <tr id="rowError" runat="server" visible="false">
            <td align="center">
                <asp:Label ID="lblExists" runat="server" Text="Such Login already exists." CssClass="errmessage"></asp:Label></td>
        </tr>
        <tr>
            <td>
            
<radG:RadGrid ID="gVendors" Skin="WebBlue" runat="server" CssClass="RadGrid" GridLines="None" AllowPaging="True" PageSize="20" AllowSorting="False" Width="99%" AutoGenerateColumns="False" EnableAJAX="False" ShowStatusBar="false" HorizontalAlign="NotSet" OnNeedDataSource="gVendors_NeedDataSource" OnItemCommand="gVendors_ItemCommand" OnItemDataBound="gVendors_ItemDataBound">
            <MasterTableView CommandItemDisplay="Bottom" GridLines="None" DataKeyNames="ID">
                <Columns>
                    <radG:GridBoundColumn UniqueName="Company" HeaderText="Company" DataField="CompanyName">
                        <HeaderStyle Width="59%" />
                    </radG:GridBoundColumn>
                    <radG:GridBoundColumn UniqueName="FirstName" HeaderText="First name" DataField="FirstName">
                        <HeaderStyle Width="20%" />
                    </radG:GridBoundColumn>
                    <radG:GridBoundColumn UniqueName="LastName" HeaderText="Last name" DataField="LastName">
                        <HeaderStyle Width="20%" Wrap="True" />
                    </radG:GridBoundColumn>
                    
                    <radg:GridEditCommandColumn ButtonType="ImageButton" 
                        UpdateImageUrl="~/RadControls/Grid/Skins/WebBlue/Update.gif"  
                        EditImageUrl="~/RadControls/Grid/Skins/WebBlue/Edit.gif"
                        InsertImageUrl="~/RadControls/Grid/Skins/WebBlue/Insert.gif"  
                        CancelImageUrl="~/RadControls/Grid/Skins/WebBlue/Cancel.gif">   
                        <ItemStyle CssClass="MyImageButton" />  
                    </radg:GridEditCommandColumn> 
                    
                    <radG:GridButtonColumn CommandName="Delete" ButtonType="ImageButton" ImageUrl="~/images/Delete.gif" UniqueName="column" ConfirmText="Are you sure you want to delete this vendor?">
                    </radG:GridButtonColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <asp:Label ID="lblExists" runat="server" Text="Vendor with such login already exists." Visible="false"/>
                        <table border="0" cellspacing="3" cellpadding="3" align="center" style="width:99%;background-color:#ffffff;">
                            <tr>
                                <td class="editFormLabel">Select category: </td>
                                <td><asp:DropDownList ID="ddlChargeCategory" runat="server" TabIndex="1" DataTextField="Name" DataValueField="ID" OnSelectedIndexChanged="ChargeCategory_SelectedIndexChanged" AutoPostBack="true" Width="155px" DataSource='<%# LoadTypes()  %>'></asp:DropDownList></td>
                                <td class="editFormLabel">Company name: </td>
                                <td><asp:TextBox ID="CompanyName" runat="server" Width="150px" CssClass="ddl" TabIndex="2" Text='<%# Bind( "CompanyName" ) %>'></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="CompanyName" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td class="editFormLabel">
                                    <asp:Label ID="Label1" runat="server" Text="Select fee:"></asp:Label>
                                </td>
                                <td colspan="3">
                                        <asp:CheckBoxList ID="cblFeeType" runat="server" RepeatColumns="3" RepeatDirection="Vertical"></asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td class="editFormLabel">First name: </td>
                                <td><asp:TextBox ID="FirstName" runat="server" Width="150px" CssClass="ddl" TabIndex="3" Text='<%# Bind( "FirstName" ) %>'></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="FirstName" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                                <td class="editFormLabel">Last name: </td>
                                <td><asp:TextBox ID="LastName" runat="server" Width="150px" CssClass="ddl" TabIndex="4" Text='<%# Bind( "LastName" ) %>'></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="LastName" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td class="editFormLabel" valign="top">Address: </td>
                                <td colspan="3" valign="top"><asp:TextBox ID="Address" runat="server" CssClass="ddl" TextMode="MultiLine" MaxLength="500" Columns="27" Rows="4" TabIndex="5" Text='<%# Bind( "Address" ) %>'></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Address" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Panel ID="ExtraParam" runat="server">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="editFormLabel">Phone number: </td>
                                            <td class="editFormCtl"><asp:TextBox ID="PhoneNumber" runat="server" Width="150px" CssClass="ddl" TabIndex="5" Text='<%# Bind( "PhoneNumber" ) %>'></asp:TextBox><asp:CustomValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="PhoneNumber" ClientValidationFunction="ValidatePhone" ErrorMessage="*" ValidateEmptyText="True"></asp:CustomValidator></td>
                                            <td class="editFormLabel">Alt phone number: </td>
                                            <td><asp:TextBox ID="AltPhoneNumber" runat="server" Width="150px" CssClass="ddl" TabIndex="6" Text='<%# Bind( "AltPhoneNumber" ) %>'></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="editFormLabel">Fax number: </td>
                                            <td class="editFormCtl"><asp:TextBox ID="FaxNumber" runat="server" Width="150px" CssClass="ddl" TabIndex="7" Text='<%# Bind( "FaxNumber" ) %>'></asp:TextBox></td>
                                            <td class="editFormLabel">Alt fax number: </td>
                                            <td><asp:TextBox ID="AltFaxNumber" runat="server" Width="150px" CssClass="ddl" TabIndex="8" Text='<%# Bind( "AltFaxNumber" ) %>'></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="editFormLabel">Email: </td>
                                            <td class="editFormCtl"><asp:TextBox ID="MailAddress" runat="server" Width="150px" CssClass="ddl" TabIndex="9" Text='<%# Bind( "MailAddress" ) %>'></asp:TextBox><asp:CustomValidator ID="RequiredFieldValidator6" runat="server"  ClientValidationFunction="ValidatePhone" ControlToValidate="MailAddress" ErrorMessage="*"  ValidateEmptyText="True" Display="Dynamic"></asp:CustomValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator2" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$" ControlToValidate="MailAddress" runat="server"  ErrorMessage="Incorrect Format" Display="Dynamic"></asp:RegularExpressionValidator></td>
                                            <td class="editFormLabel">Alt email: </td>
                                            <td><asp:TextBox ID="AltMailAddress" runat="server" Width="150px" CssClass="ddl" TabIndex="10" Text='<%# Bind( "AltMailAddress" ) %>'></asp:TextBox><asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$" ControlToValidate="AltMailAddress" runat="server"  ErrorMessage="Incorrect Format" Display="Dynamic"></asp:RegularExpressionValidator></td>
                                        </tr>
                                        <tr>
                                            <td class="editFormLabel">Login: </td>
                                            <td class="editFormCtl"><asp:TextBox ID="Login" runat="server" Width="150px" CssClass="ddl" TabIndex="11" Text='<%# Bind( "Login" ) %>'></asp:TextBox><asp:CustomValidator ID="RequiredFieldValidator7" runat="server" ClientValidationFunction="ValidatePhone"  ControlToValidate="Login" ErrorMessage="*" ValidateEmptyText="True"></asp:CustomValidator></td>
                                            <td class="editFormLabel">Disabled: </td>
                                            <td><asp:CheckBox ID="chkDisabled" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td class="editFormLabel">Password: </td>
                                            <td class="editFormCtl"><asp:TextBox ID="Password" runat="server" Width="150px" CssClass="ddl" TextMode="Password" TabIndex="12"></asp:TextBox><asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="Password" Enabled="false" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                                            <td class="editFormLabel">Confirm password: </td>
                                            <td><asp:TextBox ID="CPassword" runat="server" Width="150px" CssClass="ddl" TextMode="Password" TabIndex="12"></asp:TextBox><asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" ControlToValidate="Password" ControlToCompare="CPassword"></asp:CompareValidator></td>
                                        </tr>                                        
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>                                                                                    
                           <tr>
                                <td align="right" colspan="2">
                                    <asp:Button ID="btnUpdate" Text='Save' runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'>
                                    </asp:Button>&nbsp;
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CommandName="Cancel"></asp:Button></td>
                            </tr>
                        </table>                    
                    </FormTemplate>
                </EditFormSettings>
                <CommandItemTemplate>
                    <div style="padding:10px 5px;">
                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="InitInsert" Visible='<%# !gVendors.MasterTableView.IsItemInserted %>' CssClass="EmailLinks"><img style="border:0px;vertical-align:middle;" alt="" src="Images/addrecord.gif" /> Add new Vendor</asp:LinkButton>
                    </div>
                </CommandItemTemplate>
                
            </MasterTableView>
        </radG:RadGrid>
            
                <br /><br />                
            </td>
        </tr>
    </table>
    </div>
    </radspl:radpane>
</radspl:RadSplitter>
