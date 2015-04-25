<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Calculator.ascx.cs" Inherits="LoanStarPortal.RetailSite.Control.Calculator" %>
<%@ Register Src="../../Controls/AdvancedCalculatorValues.ascx" TagName="AdvancedCalculatorValues"
    TagPrefix="uc1" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>

<script language="javascript" type="text/javascript">
function DisplayMessage()
{
    if (document.getElementById('<%=PanelAdvancedCalculator.ClientID %>') != null)
        SetControls();
}
function SetNewValues(o,v1,v2){
    if(o=='-1') return;
    var c1=false;
    var c2=false;
    var c3=false;
    if(o=='1'){
        c1=true;
    }else if(o=='2'){
        c2=true;
    }else if(o=='3'){
        c3=true;
    }
    document.getElementById('<%=chkInitialDraw.ClientID %>').checked=c1;    
    document.getElementById('<%=chkCreditLine.ClientID %>').checked=c2;    
    document.getElementById('<%=chkMonthlyIncome.ClientID %>').checked=c3;
    document.getElementById('<%=chkMonthlyIncome.ClientID %>')
    var trMonthlyIncome = document.getElementById('<%=trMonthlyIncome.ClientID %>');
    trMonthlyIncome.style.display = c3 ? 'block' : 'none';
//    SetUnalocatedFonds(v1);
}
function SetUnalocatedFonds(v1){
    var u1=document.getElementById('<%=tbUnallocatedFundsMontly.ClientID %>').value;
    u1.
    alert(u1);
    var o=u1*1;
    alert(o);
}
function SetControlsFromRadInput(sender, args){
    SetControls();
}
function SetControls()
{
    var needMonthlyIncome = NeedMonthlyIncome();
    var needTerm = NeedTerm();
    var tbAmount = document.getElementById('<%=tbAmount.ClientID %>');
    var tbMonths = document.getElementById('<%=tbMonths.ClientID %>');
    var btnCalculate = document.getElementById('<%=btnCalculate.ClientID %>');
    btnCalculate.disabled = needMonthlyIncome && needTerm && tbAmount.value.length == 0 && tbMonths.value.length == 0;
    
    var backgroundColor = (tbAmount.value.length == 0 || tbAmount.value == 0) && 
                          (tbMonths.value.length == 0 || tbMonths.value == 0) ? 'Pink' : '#ffffaa';
    
    var tbAmountDisplay = document.getElementById('<%=tbAmount.ClientID %>_text');
    tbAmountDisplay.style.backgroundColor = backgroundColor;
    var tbMonthsDisplay = document.getElementById('<%=tbMonths.ClientID %>_text');
    tbMonthsDisplay.style.backgroundColor = backgroundColor;
}

function NeedMonthlyIncome()
{
    return document.getElementById('<%=chkMonthlyIncome.ClientID %>').checked;
}

function NeedTerm()
{
    var needTerm = false;
    var rbl = document.getElementById('<%=rbMontlyIncome.ClientID %>').getElementsByTagName("input");
    for (var i = 0; i < rbl.length; i++)
        if (rbl[i].checked)
        {
            needTerm = rbl[i].value == 'Term';
            break;
        }
        
    return needTerm;
}

function AmountFocusLost()
{
    AjaxRequestWithTarget('<%= this.UniqueID %>', 'Amount');
}

function MonthsFocusLost()
{
    AjaxRequestWithTarget('<%= this.UniqueID %>', 'Months');
}

function chkMonthlyIncome_ClientClick(sender)
{
    SetControls();

    var trMonthlyIncome = document.getElementById('<%=trMonthlyIncome.ClientID %>');
    trMonthlyIncome.style.display = sender.checked ? 'block' : 'none';
}
function rbMontlyIncome_ClientClick(sender)
{
    SetControls();

    var trTerm = document.getElementById('<%=trTerm.ClientID %>');
    trTerm.style.display = NeedTerm() ? 'block' : 'none';
}
</script>
<div id="middle">
<div class="indent">
<asp:Panel ID="pnlInput" runat="server" Visible="false">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td class="label1">
                <asp:Label ID="Label11" runat="server" Text="First Name"></asp:Label>
            </td>
            <td class="control1">
                <asp:TextBox ID="tbFirstName" runat="server" Font-Size="100%" Height="15px" MaxLength="100" Width="200px"></asp:TextBox>                
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbFirstName" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="label1">
                <asp:Label ID="Label1" runat="server" Text="Last Name"></asp:Label>
            </td>
            <td class="control1">
                <asp:TextBox ID="tbLastName" runat="server" Font-Size="100%" Height="15px" MaxLength="100" Width="200px"></asp:TextBox>                
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbLastName"
                    ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr style="padding-top:20px">
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="btnOk" runat="server" Font-Bold="true" Font-Size="12px" Height="24px"
                    OnClick="btnOK_Click" Text="OK" Width="80px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCancel" runat="server" CausesValidation="False" Font-Bold="true"
                    Font-Size="12px" Height="24px" OnClick="btnCancel_Click" Text="Cancel" Width="80px" /></td>
            <td>&nbsp;</td>
        </tr>
    </table>
    </asp:Panel>
<asp:Panel ID="PanelAdvancedCalculator" runat="server" DefaultButton="btnCalculate" Visible="true">    
    <table border="0" cellpadding="0" cellspacing="0" class="advcalctbl">
        <colgroup>
            <col width="25%" />
            <col width="1%" />
            <col <%= SelectColumn(1) %>="" width="15%" />
            <col <%= SelectColumn(2) %>="" width="15%" />
            <col <%= SelectColumn(4) %>="" width="15%" />
        </colgroup>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td class="advcalcheadertd">
                HECM Monthly</td>
            <td class="advcalcheadertd">
                HECM Annual</td>
            <td class="advcalcheadertd">
                Homekeeper</td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="lbDownload" runat="server" CssClass="AdvCalcSelProd" OnClick="lbDownload_Click">Download Input Parameters</asp:LinkButton></td>
            <td>
            </td>
            <td>
                <asp:LinkButton ID="lbMontlyProduct" runat="server" CommandArgument="HECM_Monthly"
                    CommandName="SelectProduct" CssClass="AdvCalcSelProd" OnCommand="lbSelectProduct_Clicked">Select product</asp:LinkButton></td>
            <td>
                <asp:LinkButton ID="lbAnnualProduct" runat="server" CommandArgument="HECM_Annual"
                    CommandName="SelectProduct" CssClass="AdvCalcSelProd" OnCommand="lbSelectProduct_Clicked">Select product</asp:LinkButton></td>
            <td>
                <asp:LinkButton ID="lbHomeKeeperProduct" runat="server" CommandArgument="FNMA" CommandName="SelectProduct"
                    CssClass="AdvCalcSelProd" OnCommand="lbSelectProduct_Clicked">Select product</asp:LinkButton></td>
        </tr>
        <tr style="display: none;">
            <td class="advlcheadertd">
                Download Input Parameters</td>
            <td>
            </td>
            <td>
                <asp:Button ID="btnDownloadMontly" runat="server" CssClass="lcButton" OnClick="btnDownloadMontly_Click"
                    Text="Download" /></td>
            <td>
                <asp:Button ID="btnDownloadAnnual" runat="server" CssClass="lcButton" OnClick="btnDownloadAnnual_Click"
                    Text="Download" /></td>
            <td>
                <asp:Button ID="btnDownloadHomeKeeper" runat="server" CssClass="lcButton" OnClick="btnDownloadHomeKeeper_Click"
                    Text="Download" /></td>
        </tr>
        <tr>
            <td class="advlcheadertd">
                Loan Terms</td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="advcalctd">
                Initial Interest Rate</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbInitialRateMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbInitialRateAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbInitialRateHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                Expected Interest Rate</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbExpectedIntRateMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbExpectedIntRateAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbExpectedIntRateHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                Interest Rate Cap</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbRateCupMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbRateCupAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbRateCupHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                Monthly Servicing Fee</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbMontlyServiceFeeMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbMontlyServiceFeeAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbMontlyServiceHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                Home Value</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbHomeValueMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbHomeValueAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbHomeValueHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                Pledged Value or Limit</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbPledgedValueMonthly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbPledgedValueAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbPledgedValueHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                Percentage</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbPercentageMonthly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbPercentageAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbPercentageHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr id="Tr1" runat="server" visible="false">
            <td class="advcalctd">
                Creditline Growth Rate</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbCreditlineGrowthRateMonthly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbCreditlineGrowthRateAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbCreditlineGrowthRateHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                Age</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbAgeMonthly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbAgeAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbAgeHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                Principal Limit</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbPrincipalLimitMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbPrincipalLimitAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="advcalctd">
                Service Set-Aside</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbPresentValueServiceFeeMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbPresentValueServiceFeeAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbPresentValueServiceFeeHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advlcheadertd">
                Initial Charges</td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="advcalctd">
                Initial Mort. Ins. Prem.</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbUpFrontPremiumMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbUpFrontPremiumAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbUpFrontPremiumHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                Financed Origination Fee</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbFinancedOriginationFeeMonthly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbFinancedOriginationFeeAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbFinancedOriginationFeeHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                Other Financed Costs</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbTotalOtherClosingCostsMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbTotalOtherClosingCostsAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbTotalOtherClosingCostsHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                Total Initial Charges</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbTotalInitialChargesMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbTotalInitialChargesAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbTotalInitialChargesHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advlcheadertd">
                Payoffs & Set Asides</td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="advcalctd">
                Debt Payoff Advance</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbExistingMortgageBalanceMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbExistingMortgageBalanceAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbExistingMortgageBalanceHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                Tax & Ins. Set-aside</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbTaxInsSetAsideMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbTaxInsSetAsideAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbTaxInsSetAsideHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                Net Available to You</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbNetAvailableMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbNetAvailableAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbNetAvailableHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advlcheadertd">
                Payment Plan</td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="advcalctd">
                Unallocated Funds</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbUnallocatedFundsMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbUnallocatedFundsAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbUnallocatedFundsHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                <table border="0" cellpadding="0" cellspacing="3" style="width: 99%">
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkInitialDraw" runat="server" AutoPostBack="False" Text="Initial Draw" /></td>
                        <td>
                            <radI:RadNumericTextBox ID="tbInitialDrawAmount" runat="server" CssClass="lcInputTxtToRight"
                                MinValue="0" Type="Currency" Width="70px">
                            </radI:RadNumericTextBox></td>
                    </tr>
                </table>
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbInitialDrawMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbInitialDrawAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbInitialDrawHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                <table border="0" cellpadding="0" cellspacing="3" style="width: 99%">
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkCreditLine" runat="server" AutoPostBack="False" Text="Credit Line" /></td>
                        <td>
                            <radI:RadNumericTextBox ID="tbCreditLine" runat="server" CssClass="lcInputTxtToRight"
                                MinValue="0" Type="Currency" Width="70px">
                            </radI:RadNumericTextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbCreditLineMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbCreditLineAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbCreditLineHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                Periodic Payment</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbPeriodicPaymentMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbPeriodicPaymentAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbPeriodicPaymentHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd">
                Final Term</td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="tbFinalTermMontly" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbFinalTermAnnual" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="tbFinalTermHomeKeeper" runat="server" CssClass="lcCalcAdvInputText"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="advcalctd" colspan="5">
                <asp:CheckBox ID="chkMonthlyIncome" runat="server" AutoPostBack="False" onclick="chkMonthlyIncome_ClientClick(this);"
                    Text="Monthly Income" />
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr id="trMonthlyIncome" runat="server">
            <td align="left" class="advcalctd" colspan="5" style="text-align: left;">
                <table border="0" cellpadding="0" cellspacing="0" style="padding-left: 10px;">
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rbMontlyIncome" runat="server" AutoPostBack="False" onclick="rbMontlyIncome_ClientClick(this);">
                                <asp:ListItem Text="Tenure" Value="Tenure"></asp:ListItem>
                                <asp:ListItem Text="Term" Value="Term"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr id="trTerm" runat="server">
                        <td class="advcalctd">
                            <table cellpadding="0" cellspacing="3" width="100%">
                                <tr>
                                    <td class="advcalctd">
                                        Amount</td>
                                    <td>
                                        <radI:RadNumericTextBox ID="tbAmount" runat="server" CssClass="lcInputTxtToRight"
                                            MinValue="0" Type="Currency" Width="70px">
                                            <ClientEvents OnBlur="SetControlsFromRadInput" OnFocus="SetControlsFromRadInput"
                                                OnMouseOut="SetControlsFromRadInput" OnMouseOver="SetControlsFromRadInput" OnMoveDown="SetControlsFromRadInput"
                                                OnMoveUp="SetControlsFromRadInput" OnValueChanged="SetControlsFromRadInput" />
                                        </radI:RadNumericTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="advcalctd">
                                        Months</td>
                                    <td>
                                        <radI:RadNumericTextBox ID="tbMonths" runat="server" CssClass="lcInputTxtToRight"
                                            MaxLength="3" MaxValue="540" MinValue="0" NumberFormat-DecimalDigits="0" Type="Number"
                                            Width="70px">
                                            <ClientEvents OnBlur="SetControlsFromRadInput" OnFocus="SetControlsFromRadInput"
                                                OnMouseOut="SetControlsFromRadInput" OnMouseOver="SetControlsFromRadInput" OnMoveDown="SetControlsFromRadInput"
                                                OnMoveUp="SetControlsFromRadInput" OnValueChanged="SetControlsFromRadInput" />
                                        </radI:RadNumericTextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0" class="advcalctbl">
        <tr>
            <td style="height: 24px">
                <asp:Button ID="btnCalculate" runat="server" Font-Bold="true" Font-Size="12px" Height="24px"
                    OnClick="btnCalculate_Click" Text="Calculate" Width="70px" />
            </td>
            <td style="height: 24px">
                <asp:Button ID="btnSave" runat="server" Font-Bold="true" Font-Size="12px" Height="24px"
                    OnClick="btnSave_Click" Text="Save" Width="70px" />
            </td>
            <td style="height: 24px">
                <asp:Button ID="btnBack" runat="server" Font-Bold="true" Font-Size="12px" Height="24px"
                    OnClick="btnBack_Click" Text="Back" Width="70px" />
            </td>
        </tr>
    </table>
</asp:Panel>
</div>
</div>