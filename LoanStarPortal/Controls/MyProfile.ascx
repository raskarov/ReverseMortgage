<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyProfile.ascx.cs" Inherits="LoanStarPortal.Controls.MyProfile" %>
<%@ Register Src="ChangePassword.ascx" TagName="ChangePassword" TagPrefix="uc1" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<div style="height:5px;"></div>
<radTS:RadTabStrip ID="rtsUser" runat="server" Skin="ClassicBlue" MultiPageID="rmpUser" SelectedIndex="0" Orientation="HorizontalTopToBottom" EnableViewState="False" CausesValidation="false">
    <Tabs>
        <radts:Tab Text="Password" Value="Password" ID="tabPassword" runat="server"></radts:Tab>        
        <radts:Tab Text="Email" Value="Email" ID="tabEmail" runat="server"></radts:Tab>
    </Tabs>
</radTS:RadTabStrip>
<radTS:RadMultiPage ID="rmpUser" runat="server" EnableViewState="False" AutoScrollBars="True" SelectedIndex="0" Height="80%">
    <radTS:PageView id="pvPassword" runat="server" EnableViewState="False">
        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
            <tr>
                <td align="left" style="padding-top:10px;">
                    <uc1:ChangePassword id="ChangePassword1" runat="server"></uc1:ChangePassword>        
                </td>
            </tr>
        </table>
    </radTS:PageView>
    <radTS:PageView id="pvEmail" runat="server" EnableViewState="False">
    <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
    <tr style="padding-top:40px;">
        <td class="myprofilelbl"><asp:Label ID="lblUserName" runat="server" Text="User Name:" SkinID="AdminLabel"></asp:Label></td>
        <td class="myprofilectl">
            <asp:TextBox ID="tbUserName" runat="server" MaxLength="255" Width="250px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="tbUserName"></asp:RequiredFieldValidator>       
        </td>         
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="myprofilelbl"><asp:Label ID="lblMailPassword" runat="server" Text="Password:" SkinID="AdminLabel"></asp:Label></td>
        <td class="myprofilectl">
            <asp:TextBox ID="tbMailPassword" runat="server" MaxLength="16" Width="100px" TextMode="Password"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="tbMailPassword"></asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="myprofilelbl"><asp:Label ID="Label5" runat="server" Text="Confirm Password:" SkinID="AdminLabel"></asp:Label></td>
        <td class="myprofilectl">
            <asp:TextBox ID="tbMailPasswordConfirm" runat="server" TextMode="Password" MaxLength="16" Width="100px"  CssClass="admininput"></asp:TextBox>&nbsp;<asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords are not identical" ControlToCompare="tbMailPassword" ControlToValidate="tbMailPasswordConfirm" Type="String"></asp:CompareValidator>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr style="padding-top:10px;">
        <td>&nbsp;</td>
        <td>
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>
        <td>&nbsp;</td>
    </tr>
</table>
    </radTS:PageView>
</radTS:RadMultiPage>

