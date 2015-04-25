<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateMortgage.aspx.cs" Inherits="LoanStarPortal.CreateMortgage" EnableViewStateMac="false" %>
<%@ Register Namespace="Telerik.WebControls" TagPrefix="radI" Assembly="RadInput.NET2"%>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>New mortgage</title>
    <link type="text/css" rel="stylesheet" href="App_Themes/Theme/default.css" />    
</head>
<body class="nmbody" style="border:none;">
    <form id="form1" runat="server">
<script type="text/javascript">
function GetRadWindow(){
    var o=null;
    if (window.radWindow) o = window.radWindow; 
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }
function CancelEdit(){
    GetRadWindow().Close();		
}
function CloseAndRebind(newid){
    GetRadWindow().Close();
    GetRadWindow().BrowserWindow.AddNewMortgage(newid);    
}
function SetStateCountyRows(o,tr1,tr2){
    var s = o.value;    
    var vis = 'table-row';
    if(s==0){
        vis='none';
    }
    var t = document.getElementById(tr1);
    if(t!=null){
        t.style.display = vis;
    }
    var t = document.getElementById(tr2);
    if(t!=null){
        t.style.display = vis;
    }
}
</script>    
    <div>
    <table border="0" cellpadding="0" cellspacing="0">
        <tr style="height:10px"><td colspan="3">&nbsp;</td></tr>
        <tr style="height:20px;padding-bottom:8px;" runat="server" ID="trHeaderLabel"><td colspan="3" class="nmlabel1"><asp:Label ID="Label3" runat="server" Text="Youngest applicant"></asp:Label></td></tr>
        <tr>
            <td class="nmlabel">
                <asp:Label ID="Label1" runat="server" Text="First Name"></asp:Label>
            </td>
            <td class="nminput">
                <asp:TextBox ID="tbFirstName" runat="server" MaxLength="100" Width="97%"></asp:TextBox>
            </td>
            <td style="padding-left:5px">
                <asp:Label ID="lblFirstNameErr" runat="server" Text="*" ForeColor="red"></asp:Label>
            </td>
        </tr>
        <tr style="padding-top:8px">
            <td class="nmlabel">
                <asp:Label ID="Label2" runat="server" Text="Last Name"></asp:Label></td>
            <td class="nminput">
                <asp:TextBox ID="tbLastName" runat="server" MaxLength="100" Width="97%"></asp:TextBox>
            </td>
            <td style="padding-left:5px">
                <asp:Label ID="lblLastNameErr" runat="server" Text="*" ForeColor="red"></asp:Label>
            </td>
        </tr>        
        <tr style="padding-top:8px">
            <td class="nmlabel">
                <asp:Label ID="lblBirthDate" runat="server" Text="Date Of Birth"></asp:Label>
            </td>
            <td class="nminput">
                <radI:RadDateInput ID="diYBBirthDate" runat="server" Width="100%" MinDate="1900-01-01" Skin="Windows"></radI:RadDateInput>
            </td>
            <td style="padding-left:5px">
                <asp:Label ID="lblBirthDateErr" runat="server" Text="*" ForeColor="red"></asp:Label>
            </td>
        </tr>
        <tr runat="server" id="trHomeValue" style="padding-top:8px">
            <td class="nmlabel">
                <asp:Label ID="Label4" runat="server" Text="Home value"></asp:Label></td>
            <td class="nminput">
                <radI:RadNumericTextBox ShowSpinButtons="false" Type="Currency" ID="tbHomeValue" runat="server" Skin="WebBlue" Width="100%"  MinValue="0" ></radI:RadNumericTextBox>
            </td>
            <td style="padding-left:5px">&nbsp;</td>
        </tr>
        <tr runat="server" id="trPayoff" style="padding-top:10px;vertical-align:top">
            <td class="nmlabel">
                <asp:Label ID="Label9" runat="server" Text="Total amount of liens"></asp:Label></td>
            <td class="nminput">
                <radI:RadNumericTextBox ShowSpinButtons="false" Type="Currency" ID="tbPayoffAmount" runat="server" Skin="WebBlue" Width="100%"  MinValue="0" ></radI:RadNumericTextBox>
            </td>
            <td style="padding-left:5px">&nbsp;</td>
        </tr>        
        <tr runat="server" id="trClosingCost" style="vertical-align:top;padding-top:8px">
            <td class="nmlabel">
                <asp:Label ID="Label5" runat="server" Text="Closing cost profile"></asp:Label></td>
            <td class="nminput">
                <asp:DropDownList runat="server" ID="ddlClosinCost" Width="160px" AutoPostBack="false"></asp:DropDownList>
            </td>
            <td style="padding-left:5px">&nbsp;</td>
        </tr>
        <tr runat="server" id="trState" style="display:none">
            <td class="nmlabel">
                <asp:Label ID="Label10" runat="server" Text="Select state"></asp:Label></td>
            <td class="nminput">
                <asp:DropDownList runat="server" ID="ddlState" Width="160px" AutoPostBack="true"></asp:DropDownList>
            </td>
            <td style="padding-left:5px"><asp:Label ID="lblErrState" runat="server" Text="*" ForeColor="red"></asp:Label></td>
        </tr>        
        <tr runat="server" id="trCounty" style="display:none;">
            <td class="nmlabel">
                <asp:Label ID="Label11" runat="server" Text="Select county"></asp:Label></td>
            <td class="nminput">
                <asp:DropDownList runat="server" ID="ddlCounty" Width="160px" AutoPostBack="false"></asp:DropDownList>
            </td>
            <td style="padding-left:5px"><asp:Label ID="lblErrCounty" runat="server" Text="*" ForeColor="red"></asp:Label></td>
        </tr>
        <tr runat="server" id="trLender" style="vertical-align:top;padding-top:8px">
            <td class="nmlabel">
                <asp:Label ID="Label7" runat="server" Text="Select Lender"></asp:Label></td>
            <td class="nminput">
                <asp:DropDownList runat="server" ID="ddlLender" Width="160px" AutoPostBack="true" OnSelectedIndexChanged="ddlLender_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td style="padding-left:5px">&nbsp;</td>
        </tr>
        <tr runat="server" id="trProduct" style="vertical-align:top;padding-top:8px">
            <td class="nmlabel">
                <asp:Label ID="Label8" runat="server" Text="Select Product"></asp:Label></td>
            <td class="nminput">
                <div runat="server" id="divProduct">
                    <asp:DropDownList runat="server" ID="ddlProduct" Width="160px"></asp:DropDownList>
                </div>
            </td>
            <td style="padding-left:5px">&nbsp;</td>
        </tr>
        <tr runat="server" id="trStatus" style="vertical-align:top;padding-top:8px">
            <td style="height: 20px;" class="nmlabel"><asp:Label ID="Label6" runat="server" Text="Starting status"></asp:Label></td>
            <td style="height: 20px;">
                <asp:DropDownList runat="server" ID="ddlStatus" Width="160px"></asp:DropDownList>
            </td>
            <td style="padding-left:5px; height: 20px;">
                <asp:Label ID="lblStatusErr" runat="server" Text="*" ForeColor="red"></asp:Label>
            </td>
        </tr>
        <tr runat="server" id="trGoTocalculator" style="padding-top:10px;">
            <td colspan="3"  class="nmlabel1"><span style="color:#15428B;font-size:13px;font-weight:bold;">Proceed directly to calculator</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="cbGotoCalculator" runat="server" Checked="True" /></td>
        </tr>
        <tr style="height:10px"><td colspan="3">&nbsp;</td></tr>        
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Label ID="InjectScript" runat="server"></asp:Label>
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td align="left" style="width:70px">
                            <asp:Button ID="btnOk" runat="server" Text="OK" Width="60px" OnClick="btnOk_Click"  />
                        </td>
                        <td align="left" style="padding-left:40px;">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="60px" OnClick="btnCancel_Click" CausesValidation="false" />
                        </td>
                    </tr>
                </table>                
            </td>
            <td>&nbsp;</td>
        </tr>        
    </table>    
    </div>
<rada:RadAjaxManager id="RadAjaxManager1" runat="server" EnableOutsideScripts="True" EnablePageHeadUpdate="False">
    <AjaxSettings>
        <radA:AjaxSetting AjaxControlID="ddlLender">
            <UpdatedControls>
                <radA:AjaxUpdatedControl ControlID="divProduct" />
            </UpdatedControls>
        </radA:AjaxSetting>
    </AjaxSettings>
</rada:RadAjaxManager>
    </form>
</body>
</html>
