<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditUserOriginator.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.EditUserOriginator" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<script language="javascript" type="text/javascript">
function SetDivVisibility(b,id,id1){
    var d = document.getElementById(id);
    if(d!=null){
        d.style.display=b.checked?'none':'inline';
    }
    d = document.getElementById(id1);
    if(d!=null){
        d.style.display=b.checked?'none':'inline';
    }
}
</script>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td align="center">
            <asp:Label ID="lblHeader" runat="server" Text="!!!" SkinID="AdminHeader"></asp:Label>
        </td>
    </tr>
    <tr runat="server">
        <td align="center">
            <asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label>
        </td>
    </tr>
</table>
<radTS:RadTabStrip ID="rtsUser" runat="server" Skin="ClassicBlue" MultiPageID="rmpUser" SelectedIndex="0" Orientation="HorizontalTopToBottom" EnableViewState="False">
    <Tabs>
        <radts:Tab Text="General" Value="General" ID="tabGeneral" runat="server"></radts:Tab>        
        <radts:Tab Text="Email" Value="Email" ID="tabEmail" runat="server"></radts:Tab>
        <radts:Tab Text="Roles" Value="Roles" ID="tabRoles" runat="server"></radts:Tab>
        <radts:Tab Text="Location" Value="Location" ID="tabLocation" runat="server"></radts:Tab>
    </Tabs>
</radTS:RadTabStrip>
<radTS:RadMultiPage ID="rmpUser" runat="server" EnableViewState="False" AutoScrollBars="True" SelectedIndex="0">
        <radTS:PageView id="pvGeneral" runat="server" EnableViewState="False">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr style="height:15px"><td colspan="3">&nbsp;</td></tr>
                <tr>
                    <td class="eultd">
                        <asp:Label ID="Label1" runat="server" Text="Login:" SkinID="AdminLabel"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbLogin" runat="server" MaxLength="16"  Width="160px"></asp:TextBox>&nbsp;
                        <asp:Label ID="lblLoginErr" runat="server" Text="Login must be at least 6 character long" ForeColor="red"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr runat="server" id="trPassword">
                    <td class="eultd">
                        <asp:CheckBox ID="cbEnablePassword" runat="server" Text="Change password" />        
                    </td>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                <td class="eultd">&nbsp;</td>
                    <td colspan="2"><asp:Label ID="lblPasswordRules" runat="server" Text="" Font-Italic="true"></asp:Label></td>
                </tr>
                <tr>
                    <td class="eultd">
                        <asp:Label ID="Label2" runat="server" Text="Password:" SkinID="AdminLabel"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbPassword" runat="server" TextMode="Password" MaxLength="16" Width="160px"></asp:TextBox>&nbsp;
                        <asp:Label ID="lblPasswordErr" runat="server" Text="" ForeColor="red"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="eultd">
                        <asp:Label ID="Label3" runat="server" Text="Confirm Password:" SkinID="AdminLabel"></asp:Label>
                    </td>
                    <td class="euitd">
                        <asp:TextBox ID="tbConfirmPassword" runat="server" TextMode="Password" MaxLength="16" Width="160px"></asp:TextBox>&nbsp;
                        <asp:Label ID="lblConfirmPasswordErr" runat="server" Text="" ForeColor="red"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="eultd">
                        <asp:Label ID="Label6" runat="server" Text="First Name:" SkinID="AdminLabel"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbFirstName" runat="server"  MaxLength="20" Width="160px"></asp:TextBox>&nbsp;
                        <asp:Label ID="lblFirstNameErr" runat="server" Text="" ForeColor="red"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="eultd">
                        <asp:Label ID="Label7" runat="server" Text="Last Name:" SkinID="AdminLabel"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbLastName" runat="server" MaxLength="20" Width="160px"></asp:TextBox>&nbsp;
                        <asp:Label ID="lblLastNameErr" runat="server" Text="" ForeColor="red"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="eultd">
                        <asp:Label ID="Label11" runat="server" Text="Primary email:" SkinID="AdminLabel"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbPrimaryEmail" runat="server" MaxLength="256" Width="160px"></asp:TextBox>&nbsp;
                        <asp:Label ID="lblPrimatyEmailErr" runat="server" Text="" ForeColor="red"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>                
                <tr style="padding-top:5px;" runat="server" id="trManagePhoto">
                    <td align="right" style="padding-right: 3px;">&nbsp;</td>
                    <td><input id="Button2" type="button" value="Manage Photo" onclick="radopen('UploadPhoto.aspx','UploadPhoto');"/></td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </radTS:PageView>
        <radTS:PageView id="pvEmail" runat="server" EnableViewState="False">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr style="padding-top:10px;">
                    <td colspan="3" style="padding-left:10px;font-weight:bold">Use company email account&nbsp;&nbsp;<asp:CheckBox ID="cbCompanyEmailSettings" runat="server" Text="" /></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div runat="server" id="divServers">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>&nbsp;</td>
                                    <td colspan="2" style="text-align: left; padding-left:15px;"><strong>POP3</strong></td>                              
                                </tr>
                                <tr>
                                    <td class="eultd"><asp:Label ID="lblPOP3Server" runat="server" Text="Server :" SkinID="AdminLabel"></asp:Label></td>
                                    <td class="edituseremailctl"><asp:TextBox ID="tbPOP3Server" runat="server" Width="250px" ></asp:TextBox>&nbsp;<asp:Label ID="lblErrPOP3Server" runat="server" Text="" ForeColor="Red"></asp:Label></td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="eultd"><asp:Label ID="lblPOP3Port" runat="server" Text="Port :" SkinID="AdminLabel"></asp:Label></td>
                                    <td class="edituseremailctl"><asp:TextBox ID="tbPOP3Port" runat="server" Width="50px" MaxLength="5"></asp:TextBox>&nbsp;<asp:Label ID="lblErrPOP3Port" runat="server" Text="" ForeColor="Red"></asp:Label></td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="eultd"><asp:Label ID="Label9" runat="server" Text="Leave messages on server:" SkinID="AdminLabel"></asp:Label></td>
                                    <td class="edituseremailctl"><asp:CheckBox ID="cbLeaveMessage" runat="server" ></asp:CheckBox></td>
                                    <td>&nbsp;</td>
                                </tr>                                
                                <tr>
                                    <td>&nbsp;</td>
                                    <td colspan="2" style="text-align: left; padding-left:15px;"><strong>SMTP</strong></td>                              
                                </tr>
                                <tr>
                                    <td class="eultd"><asp:Label ID="Label10" runat="server" Text="Server :" SkinID="AdminLabel"></asp:Label></td>
                                    <td class="edituseremailctl"><asp:TextBox ID="tbSMTPServer" runat="server" Width="250px"></asp:TextBox>&nbsp;<asp:Label ID="lblErrSMTPServer" runat="server" Text="" ForeColor="Red"></asp:Label></td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="eultd"><asp:Label ID="Label12" runat="server" Text="Port :" SkinID="AdminLabel"></asp:Label></td>
                                    <td class="edituseremailctl"><asp:TextBox ID="tbSMTPPort" runat="server" Width="50px" MaxLength="5"></asp:TextBox>&nbsp;<asp:Label ID="lblErrSMTPPort" runat="server" Text="" ForeColor="Red"></asp:Label></td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="eultd"></td>
                                    <td colspan="2"><asp:Label ID="lblTest" runat="server" Text="" ForeColor="Red"></asp:Label></td>
                                </tr>
                                <tr style="padding-top:5px;padding-bottom:5px">
                                    <td class="eultd">&nbsp;</td>
                                    <td class="edituseremailctl"><asp:Button ID="btnTest" runat="server" Text="Test" SkinID="AdminButton" OnClick="btnTest_Click" CausesValidation="False"/></td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </div>                    
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td class="eultd">
                                    <asp:Label ID="lblUserMail" runat="server" Text="Email:" SkinID="AdminLabel"></asp:Label>
                                </td>                
                                <td  class="edituseremailctl">
                                    <asp:TextBox ID="tbUserMail" runat="server" MaxLength="255"  Width="250px"></asp:TextBox>
                                </td>                
                                <td>
                                    <asp:Label ID="lblUserMailErr" runat="server" Text="" ForeColor="red"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trWarning">
                                <td class="eultd">&nbsp;</td>
                                <td colspan="2">
                                <div style="padding-left:10px;font-style:italic">
                                        Warning: If the email address above is a personal email address, do not request the following information or complete the following fields. The user will be able to provide this information from their “My Profile” page located from the “Resources” Menu.
                                </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="eultd">
                                    <asp:Label ID="lblUserName" runat="server" Text="User Name:" SkinID="AdminLabel"></asp:Label>
                                </td>
                                <td  class="edituseremailctl">
                                    <asp:TextBox ID="tbUserName" runat="server" MaxLength="255" Width="250px"></asp:TextBox>&nbsp;<asp:Label ID="lblUserNameErr" runat="server" Text="" ForeColor="red"></asp:Label>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="eultd">
                                    <asp:Label ID="lblMailPassword" runat="server" Text="Password:" SkinID="AdminLabel"></asp:Label>
                                </td>
                                <td  class="edituseremailctl">
                                    <asp:TextBox ID="tbMailPassword" runat="server" MaxLength="16" Width="100px" TextMode="Password"></asp:TextBox>&nbsp;<asp:Label ID="lblMailPasswordErr" runat="server" Text="" ForeColor="red"></asp:Label>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="eultd">
                                    <asp:Label ID="Label5" runat="server" Text="Confirm Password:" SkinID="AdminLabel"></asp:Label>
                                </td>
                                <td  class="edituseremailctl">
                                    <asp:TextBox ID="tbMailPasswordConfirm" runat="server" TextMode="Password" MaxLength="16" Width="100px"  CssClass="admininput"></asp:TextBox>&nbsp;<asp:Label ID="lblMailPasswordConfirmErr" runat="server" Text="" ForeColor="red"></asp:Label>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>                                
                    </td>
                </tr>
            </table>        
        </radTS:PageView>
        <radTS:PageView id="pvRoles" runat="server" EnableViewState="true">
            <div style="padding-top:15px;">
            <asp:DataList ID="dlRoles" runat="server" RepeatColumns="5" RepeatDirection="Horizontal" EnableViewState="true" DataKeyField="id">
            <ItemTemplate>
                <asp:CheckBox ID="Checkbox1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>' Checked='<%# int.Parse(DataBinder.Eval(Container.DataItem,"Selected").ToString())==1?true:false %>' />
            </ItemTemplate>
            </asp:DataList>                    
            </div>
        </radTS:PageView>
        <radTS:PageView id="pvLocation" runat="server" EnableViewState="False">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="padding-top:15px;" >
                <tr>
                    <td class="eultd">
                        <asp:Label ID="lblState" runat="server" Text="State:" SkinID="AdminLabel"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlState" runat="server" Width="164px"></asp:DropDownList>
                    </td>
                    <td>&nbsp;</td>
                </tr>                
                <tr>
                    <td class="eultd">
                        <asp:Label ID="lblCity" runat="server" Text="City:" SkinID="AdminLabel"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbCity" runat="server" MaxLength="50" Width="160px"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="eultd">
                        <asp:Label ID="lblAddress1" runat="server" Text="Address1:" SkinID="AdminLabel"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbAddress1" runat="server" MaxLength="256" Width="350px"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>                
                <tr>
                    <td class="eultd">
                        <asp:Label ID="lblAddress2" runat="server" Text="Address2:" SkinID="AdminLabel"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbAddress2" runat="server" MaxLength="256" Width="350px"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="eultd">
                        <asp:Label ID="Label4" runat="server" Text="Phone:" SkinID="AdminLabel"></asp:Label>
                    </td>
                    <td>
                        <radi:radmaskedtextbox id="tbPhone" runat="server" displaymask="(###) ###-####" displaypromptchar=" " DisplayFormatPosition="Right" Mask="(###) ###-####" Width="100px"></radi:radmaskedtextbox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="eultd">
                        <asp:Label ID="Label8" runat="server" Text="Fax:" SkinID="AdminLabel"></asp:Label>
                    </td>
                    <td>
                        <radi:radmaskedtextbox id="tbFax" runat="server" displaymask="(###) ###-####" displaypromptchar=" " DisplayFormatPosition="Right" Mask="(###) ###-####" Width="100px"></radi:radmaskedtextbox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="eultd">
                        <asp:Label ID="lblZip" runat="server" Text="Zip:" SkinID="AdminLabel"></asp:Label>
                    </td>
                    <td>
                        <radi:radmaskedtextbox id="tbZip" runat="server" displaymask="#####" displaypromptchar=" " DisplayFormatPosition="Right" Mask="#####" Width="100px"></radi:radmaskedtextbox>
                    </td>
                    <td>&nbsp;</td>
                </tr>                
            </table>        
        </radTS:PageView> 
</radTS:RadMultiPage>
<table border="0" cellpadding="0" cellspacing="0">
    <tr style="padding-top:20px;">
        <td style="width:145px">&nbsp;</td>
        <td align="left"><asp:Button ID="btnBack" runat="server" Text="Back" SkinID="AdminButton" OnClick="btnBack_Click" CausesValidation="False"/></td>
        <td align="left" style="padding-left:65px"><asp:Button ID="btnSave" runat="server" Text="Save"  SkinID="AdminButton" OnClick="btnSave_Click"/></td>
    </tr>
</table>

