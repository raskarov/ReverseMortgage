<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClosingCostProfile.ascx.cs" Inherits="LoanStarPortal.Controls.ClosingCostProfile" %>
<%@ Register Src="ClosingCostProfileView.ascx" TagName="ClosingCostProfileView" TagPrefix="uc1" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>

<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Width="100%" BorderWidth="0" BorderStyle="None" BorderSize="0" Skin="Default" LiveResize="false">
    <radspl:radpane id="TopPane" runat="server" Height="35px" Scrolling="None">
        <div RadResizeStopLookup="true" RadShowStopLookup="true" >
            <div class="paneTitle"><b>Closing Cost Profiles</b></div>
        </div>
    </radspl:radpane>
    <radspl:radpane id="MiddlePane" runat="server" scrolling="Y" >
        <div RadResizeStopLookup="true" RadShowStopLookup="true" >
            <table border="0" style="width:100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="padding-left:10px;width:120px">
                        <asp:Label ID="Label1" runat="server" Text="Select profile:" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                          <asp:DropDownList ID="ddlProfiles" runat="server" OnSelectedIndexChanged="ddlProfile_IndexChanged" AutoPostBack="true" OnClientSelectedIndexChanging="OnClientIndexChanging">
                </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="padding-left:10px">
                        <asp:Label ID="Label2" runat="server" Text="Edit profile name:" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbProfileName" runat="server" MaxLength="256" Width="300px"></asp:TextBox>                        
                        &nbsp;<asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="*" ControlToValidate="tbProfileName"></asp:RequiredFieldValidator>
                        &nbsp;&nbsp;&nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="Save_Click" Width="60px"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnDelete"  runat="server" Text="Delete" OnClick="Delete_Click" Width="60px" CausesValidation="false"/>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Label ID="lblErr" runat="server" Text="" ForeColor="red"></asp:Label></td>
                </tr>
                <tr style="padding-top:10px" id="trDetails" runat="server">
                    <td colspan="2" style="padding-left:10px;font-weight:bold">Profile details</td>
                </tr>
                <tr id="trGrid" runat="server">                
                    <td colspan="2" style="padding-left:10px">
                        <uc1:ClosingCostProfileView id="ClosingCostProfileView1" runat="server"></uc1:ClosingCostProfileView>
                    </td>
                </tr>
            </table>
        </div>
    </radspl:radpane>    
</radspl:RadSplitter>