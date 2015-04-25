<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdvancedCalculator.ascx.cs" Inherits="LoanStarPortal.Controls.AdvancedCalculator" %>
<%@ Register Src="AdvancedCalculatorValues.ascx" TagName="AdvancedCalculatorValues" TagPrefix="uc1" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>

<script language="javascript" type="text/javascript" defer="defer">
function DisplayMessage()
{
    if (document.getElementById('<%=PanelAdvancedCalculator.ClientID %>') != null)
        SetControls();

    var resAdvCalcMsg = document.getElementById('<%=resAdvCalcMessage.ClientID %>');
    if (resAdvCalcMsg != null && resAdvCalcMsg.value.length > 0)
        alert(resAdvCalcMsg.value);
    if (resAdvCalcMsg != null)
        resAdvCalcMsg.value = '';
}

function SetControlsFromRadInput(sender, args)
{
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

<asp:HiddenField ID="resAdvCalcMessage" runat="server" />

<asp:Panel ID="PanelAdvancedCalculatorValues" runat="server" Visible="false" >
<uc1:AdvancedCalculatorValues ID="ucAdvancedCalculatorValues" runat="server" 
    OnValidateFirst="ucAdvancedCalculatorValues_ValidateFirst"/>
</asp:Panel>

<asp:Panel ID="PanelAdvancedCalculator" runat="server" Visible="false" DefaultButton="btnCalculate">
<table class="advcalctbl" border="0" cellspacing="0" cellpadding="0" >
    <colgroup>
        <col width="25%"/>
        <col width="1%" />
        <col width="15%" <%= SelectColumn(1) %>/>
        <col width="15%" <%= SelectColumn(2) %> />
        <col width="15%" <%= SelectColumn(4) %>/>
    </colgroup>
    <tr>
        <td></td>
        <td></td>
        <td class="advcalcheadertd">HECM Monthly</td>
        <td class="advcalcheadertd">HECM Annual</td>
        <td class="advcalcheadertd">Homekeeper</td>
    </tr>
    <tr>
        <td><asp:LinkButton ID="lbDownload" runat="server" OnClick="lbDownload_Click" CssClass="AdvCalcSelProd">Download Input Parameters</asp:LinkButton></td>
        <td></td>
        <td><asp:LinkButton ID="lbMontlyProduct" CssClass="AdvCalcSelProd" CommandName="SelectProduct" CommandArgument="HECM_Monthly" OnCommand="lbSelectProduct_Clicked" runat="server">Select product</asp:LinkButton></td>
        <td><asp:LinkButton ID="lbAnnualProduct" CssClass="AdvCalcSelProd" CommandName="SelectProduct" CommandArgument="HECM_Annual" OnCommand="lbSelectProduct_Clicked" runat="server">Select product</asp:LinkButton></td>
        <td><asp:LinkButton ID="lbHomeKeeperProduct" CssClass="AdvCalcSelProd" CommandName="SelectProduct" CommandArgument="FNMA" OnCommand="lbSelectProduct_Clicked" runat="server">Select product</asp:LinkButton></td>
    </tr>
    <tr style="display:none;">
        <td class="advlcheadertd">Download Input Parameters</td>
        <td></td>
        <td><asp:Button ID="btnDownloadMontly" runat="server" Text="Download" CssClass="lcButton" OnClick="btnDownloadMontly_Click" /></td>
        <td><asp:Button ID="btnDownloadAnnual" runat="server" Text="Download" CssClass="lcButton" OnClick="btnDownloadAnnual_Click" /></td>
        <td><asp:Button ID="btnDownloadHomeKeeper" runat="server" Text="Download" CssClass="lcButton" OnClick="btnDownloadHomeKeeper_Click" /></td>
    </tr>
    <tr>
        <td class="advlcheadertd">Loan Terms</td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td class="advcalctd">Initial Interest Rate</td>
        <td></td>
        <td><asp:TextBox ID="tbInitialRateMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbInitialRateAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbInitialRateHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd">Expected Interest Rate</td>
        <td></td>
        <td><asp:TextBox ID="tbExpectedIntRateMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbExpectedIntRateAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbExpectedIntRateHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd">Interest Rate Cap</td>
        <td></td>
        <td><asp:TextBox ID="tbRateCupMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbRateCupAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbRateCupHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd">Monthly Servicing Fee</td>
        <td></td>
        <td><asp:TextBox ID="tbMontlyServiceFeeMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbMontlyServiceFeeAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbMontlyServiceHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd">Home Value</td>
        <td></td>
        <td><asp:TextBox ID="tbHomeValueMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbHomeValueAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbHomeValueHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd">Pledged Value or Limit</td>
        <td></td>
        <td><asp:TextBox ID="tbPledgedValueMonthly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbPledgedValueAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbPledgedValueHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>   
    <tr>
        <td class="advcalctd">Creditline Growth Rate</td>
        <td></td>
        <td><asp:TextBox ID="tbCreditlineGrowthRateMonthly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbCreditlineGrowthRateAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbCreditlineGrowthRateHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd">Age</td>
        <td></td>
        <td><asp:TextBox ID="tbAgeMonthly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbAgeAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbAgeHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd">Principal Limit</td>
        <td></td>
        <td><asp:TextBox ID="tbPrincipalLimitMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbPrincipalLimitAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td></td>
    </tr>
    <tr>
        <td class="advcalctd">Service Set-Aside</td>
        <td></td>
        <td><asp:TextBox ID="tbPresentValueServiceFeeMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbPresentValueServiceFeeAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbPresentValueServiceFeeHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advlcheadertd">Initial Charges</td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td class="advcalctd">Initial Mort. Ins. Prem.</td>
        <td></td>
        <td><asp:TextBox ID="tbUpFrontPremiumMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbUpFrontPremiumAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbUpFrontPremiumHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd">Financed Origination Fee</td>
        <td></td>
        <td><asp:TextBox ID="tbFinancedOriginationFeeMonthly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbFinancedOriginationFeeAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbFinancedOriginationFeeHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd">Other Financed Costs</td>
        <td></td>
        <td><asp:TextBox ID="tbTotalOtherClosingCostsMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbTotalOtherClosingCostsAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbTotalOtherClosingCostsHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd">Total Initial Charges</td>
        <td></td>
        <td><asp:TextBox ID="tbTotalInitialChargesMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbTotalInitialChargesAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbTotalInitialChargesHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advlcheadertd">Payoffs & Set Asides</td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td class="advcalctd">Debt Payoff Advance</td>
        <td></td>
        <td><asp:TextBox ID="tbExistingMortgageBalanceMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbExistingMortgageBalanceAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbExistingMortgageBalanceHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd">Tax & Ins. Set-aside</td>
        <td></td>
        <td><asp:TextBox ID="tbTaxInsSetAsideMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbTaxInsSetAsideAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbTaxInsSetAsideHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd">Net Available to You</td>
        <td></td>
        <td><asp:TextBox ID="tbNetAvailableMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbNetAvailableAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbNetAvailableHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advlcheadertd">Payment Plan</td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td class="advcalctd">Unallocated Funds</td>
        <td></td>
        <td><asp:TextBox ID="tbUnallocatedFundsMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbUnallocatedFundsAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbUnallocatedFundsHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd">
            <table border="0" cellspacing="3" cellpadding="0" style="width:99%">
                <tr>
                    <td><asp:CheckBox ID="chkInitialDraw" runat="server" Text="Initial Draw" AutoPostBack="False"/></td>
                    <td><radI:RadNumericTextBox ID="tbInitialDrawAmount" runat="server" Type="Currency" MinValue="0" CssClass="lcInputTxtToRight" Width="70px"></radi:RadNumericTextBox></td>
                </tr>
            </table>
        </td>
        <td></td>
        <td><asp:TextBox ID="tbInitialDrawMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbInitialDrawAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbInitialDrawHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd">
            <table border="0" cellspacing="3" cellpadding="0" style="width:99%">
                <tr>
                    <td><asp:CheckBox ID="chkCreditLine" runat="server" Text="Credit Line" AutoPostBack="False"/></td>
                    <td><radI:RadNumericTextBox ID="tbCreditLine" runat="server" Type="Currency" MinValue="0" CssClass="lcInputTxtToRight" Width="70px"></radi:RadNumericTextBox></td>
                </tr>
              </table>        
        </td>
        <td></td>
        <td><asp:TextBox ID="tbCreditLineMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbCreditLineAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbCreditLineHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd">Periodic Payment</td>
        <td></td>
        <td><asp:TextBox ID="tbPeriodicPaymentMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbPeriodicPaymentAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbPeriodicPaymentHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd">Final Term</td>
        <td></td>
        <td><asp:TextBox ID="tbFinalTermMontly" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbFinalTermAnnual" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
        <td><asp:TextBox ID="tbFinalTermHomeKeeper" runat="server" CssClass="lcCalcAdvInputText" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="advcalctd" colspan="5">
            <asp:CheckBox ID="chkMonthlyIncome" runat="server" Text="Monthly Income" AutoPostBack="False" onclick="chkMonthlyIncome_ClientClick(this);"/>
        </td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
    </tr>
    <tr id="trMonthlyIncome" runat="server">
        <td colspan="5" class="advcalctd" align="left" style="text-align:left;">
            <table border="0" cellspacing="0" cellpadding="0" style="padding-left:10px;">
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
                        <table cellspacing="3" cellpadding="0" width="100%">
                            <tr>
                                <td class="advcalctd">Amount</td>
                                <td>
                                    <radI:RadNumericTextBox ID="tbAmount" runat="server" Type="Currency" MinValue="0" CssClass="lcInputTxtToRight" Width="70px">
                                        <ClientEvents OnValueChanged="SetControlsFromRadInput" 
                                            OnMouseOver="SetControlsFromRadInput" 
                                            OnMouseOut="SetControlsFromRadInput" 
                                            OnFocus="SetControlsFromRadInput"
                                            OnMoveUp="SetControlsFromRadInput"
                                            OnMoveDown="SetControlsFromRadInput"
                                            OnBlur="SetControlsFromRadInput"/>
                                    </radI:RadNumericTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="advcalctd">Months</td>
                                <td>
                                    <radI:RadNumericTextBox ID="tbMonths" runat="server" MaxLength="3" Type="Number" MinValue="0" MaxValue="540" CssClass="lcInputTxtToRight" Width="70px" NumberFormat-DecimalDigits="0">
                                        <ClientEvents OnValueChanged="SetControlsFromRadInput" 
                                        OnMouseOver="SetControlsFromRadInput" 
                                        OnMouseOut="SetControlsFromRadInput" 
                                        OnFocus="SetControlsFromRadInput"
                                        OnMoveUp="SetControlsFromRadInput"
                                        OnMoveDown="SetControlsFromRadInput"
                                        OnBlur="SetControlsFromRadInput"/>
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

<table class="advcalctbl" border="0" cellspacing="0" cellpadding="0" >
    <colgroup>
        <col width="25%"/>
        <col width="1%" />
        <col width="15%"/>
        <col width="15%" />
        <col width="15%"/>
    </colgroup>
    <tr>
        <td align="left" style="padding-left:10px;padding-top:20px;"><asp:Button  ID="btnCalculate" runat="server" Text="Calculate" OnClick="btnCalculate_Click" /></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
    </tr>
</table>    

</asp:Panel>

<div style="height:25px;">&nbsp;</div>