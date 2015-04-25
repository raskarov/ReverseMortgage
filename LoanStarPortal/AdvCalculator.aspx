<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdvCalculator.aspx.cs" Inherits="LoanStarPortal.AdvCalculator" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Calculator</title>
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

function CloseAndRebind()
{
    GetRadWindow().Close();
    
    var advCalcUniqueID = document.getElementById('<%=hfAdvCalcUniqueID.ClientID %>').value;
    if (advCalcUniqueID.length > 0)
        GetRadWindow().BrowserWindow.AjaxRequestWithTarget(advCalcUniqueID, 'BindData');
    else
        GetRadWindow().BrowserWindow.RefreshPage();
}
</script>

    <asp:HiddenField ID="hfAdvCalcUniqueID" runat="server" />
    
    <div style="padding-left:10px;padding-right:10px;">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <colgroup>
            <col width="1%" />
            <col width="99%" />            
        </colgroup>
        <tr style="height:10px"><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td class="calc_label" colspan="2">It seems that there is <asp:Literal ID="ltrUnallocatedFunds" runat="server"></asp:Literal> that has not been allocated to your payment plan. How would you like to distribute this?</td>
        </tr>
        <tr style="height:10px"><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td><asp:RadioButton ID="rbtnIncreaseInitialDraw" runat="server" GroupName="Funds"/>&nbsp;</td>
            <td><asp:Literal ID="ltrInitialDraw" runat="server" Text="Increase your initial draw to {0}"></asp:Literal></td>
        </tr>
        <tr>
            <td><asp:RadioButton ID="rbtnIncreaseCreditLine" runat="server" GroupName="Funds"/>&nbsp;</td>
            <td><asp:Literal ID="ltrCreditLine" runat="server" Text="Increase your credit line to {0}"></asp:Literal></td>
        </tr>
        <tr>
            <td><asp:RadioButton ID="rbtnApplyTenure" runat="server" GroupName="Funds"/>&nbsp;</td>
            <td><asp:Literal ID="ltrTenure" runat="server" Text="Distribute this as {0} in monthly tenure income"></asp:Literal></td>
        </tr>
        
        <asp:Panel ID="pnlTermPayment" runat="server">
        <tr>
            <td><asp:RadioButton ID="rbtnTermPayment" runat="server" GroupName="Funds" />&nbsp;</td>
            <td><asp:Literal ID="ltrlTermPayment" runat="server" Text="Increase your term payment amount by {0}"></asp:Literal></td>
        </tr>
        </asp:Panel>
        
        <tr style="height:10px"><td colspan="2">&nbsp;</td></tr>        
        <tr>
            <td colspan="2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td align="left">
                            <asp:Button ID="btnOk" runat="server" Text="OK" Width="60px" OnClick="btnOk_Click"  />
                        </td>
                        <td align="right">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="60px" OnClick="btnCancel_Click" CausesValidation="false" />                        
                        </td>
                    </tr>
                </table>                
            </td>
            <td>&nbsp;</td>
        </tr>        
    </table>    
    </div>
    </form>
</body>
</html>