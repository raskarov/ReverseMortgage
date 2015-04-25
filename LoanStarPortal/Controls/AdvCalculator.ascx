<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdvCalculator.ascx.cs" Inherits="LoanStarPortal.Controls.AdvCalculator" %>
<%@ Register Src="AdvCalculatorValues.ascx" TagName="AdvCalculatorValues" TagPrefix="uc1" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="RadMenu.Net2" Namespace="Telerik.WebControls" TagPrefix="radM" %>
<script language="javascript" type="text/javascript" defer="defer">
function OnMenuItemClicking(sender, eventArgs){ 
    ClearNeedCheckCalcFalg();
}
function ClearNeedCheckCalcFalg(){
    var c=document.getElementById('calcneedcheck');
    if(c!=null){
        c.value=0;
    }
}
function ClearCalcFalg(){
    var c=document.getElementById('calcmodified');
    if(c!=null){
        c.value=0;
    }
}
function SetCalcFalg(){
    var c=document.getElementById('calcmodified');
    if(c!=null){
        c.value=1;
    }
}
function DisplayMessage(){
    if (document.getElementById('<%=PanelAdvancedCalculator.ClientID %>') != null)
        SetControls();

    var resAdvCalcMsg = document.getElementById('<%=resAdvCalcMessage.ClientID %>');
    if (resAdvCalcMsg != null && resAdvCalcMsg.value.length > 0)
        alert(resAdvCalcMsg.value);
    if (resAdvCalcMsg != null)
        resAdvCalcMsg.value = '';
}
function SetControlsFromRadInput(sender, args){
    SetControls();
}
function SetControls(){
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
function NeedMonthlyIncome(){
    return document.getElementById('<%=chkMonthlyIncome.ClientID %>').checked;
}
function NeedTerm(){
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
function AmountFocusLost(){
    AjaxRequestWithTarget('<%= this.UniqueID %>', 'Amount');
}
function MonthsFocusLost(){

    AjaxRequestWithTarget('<%= this.UniqueID %>', 'Months');
}
function chkMonthlyIncome_ClientClick(sender){
    SetCalcFalg();
    SetControls();
    var trMonthlyIncome = document.getElementById('<%=trMonthlyIncome.ClientID %>');
    trMonthlyIncome.style.display = sender.checked ? 'block' : 'none';
}
function rbMontlyIncome_ClientClick(sender){
    SetCalcFalg();
    SetControls();
    var trTerm = document.getElementById('<%=trTerm.ClientID %>');
    var b = NeedTerm();
    trTerm.style.display = b ? 'block' : 'none';
    SetTermValues(b);
}
function SetTermValues(b){
    var t = document.getElementById('calcTable');
    if(t){
        for(var i=0;i<t.rows.length;i++){
            if(t.rows[i].cells[0].innerHTML=='Final Term'){
                for(var j=1;j<t.rows[i].cells.length;j++){
                    var td=t.rows[i].cells[j];
                    var c=td.firstChild
                    if(c){
                        if(c.tagName.toLowerCase()=='input'){
                            c.value=b?c.getAttribute('val'):'Tenure';
                        }                    
                    }                    
                }
            }
        }
    }    
}

</script>
<asp:HiddenField ID="resAdvCalcMessage" runat="server" />
<asp:Panel ID="PanelAdvancedCalculatorValues" runat="server" Visible="false" >
    <uc1:AdvCalculatorValues id="AdvCalculatorValues" runat="server" OnValidateFirst="ucAdvancedCalculatorValues_ValidateFirst"/>
</asp:Panel>
<asp:Panel ID="PanelAdvancedCalculator" runat="server" Visible="false" DefaultButton="btnCalculate">
<radM:radmenu id="rmCalculator" runat="server" Skin="Web20" style="position:absolute;top:60px;left:216px" CausesValidation="false" OnItemClick="rmMortgage_ItemClick" Height="26px" OnClientItemClicking="OnMenuItemClicking">
    <Items>
        <radm:RadMenuItem Text="Options" AccessKey="o" Value="Options">
            <Items>
                <radM:RadMenuItem AccessKey="c" Text="Show scenario column" Value="ScenarioColumn"></radm:RadMenuItem>
                <radm:RadMenuItem Text="Use application date rate" AccessKey="u" Value="ApplicationRate"></radm:RadMenuItem>
            </Items>
        </radm:RadMenuItem>
    </Items>
</radM:radmenu>
<table class="advctbl" border="0" cellspacing="0" cellpadding="0" id="calcTable">
    <colgroup>
        <col width="50%"/>        
        <col width="1%" />
        <col width="15%"  <%= SetStyle(0) %>/>
        <col width="15%"  <%= SetStyle(1) %> />
        <col width="15%"  <%= SetStyle(2) %>/>
        <col width="15%"  <%= SetStyle(3) %>/>
    </colgroup>
    <tr>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td><asp:DropDownList runat="server" ID="ddlProduct_0" AutoPostBack="true" Visible="false" ></asp:DropDownList></td>
        <td><asp:DropDownList runat="server" ID="ddlProduct_1" AutoPostBack="true" Visible="false" ></asp:DropDownList></td>
        <td><asp:DropDownList runat="server" ID="ddlProduct_2" AutoPostBack="true" Visible="false" ></asp:DropDownList></td>
        <td><asp:DropDownList runat="server" ID="ddlProduct_3" AutoPostBack="true" Visible="false" ></asp:DropDownList></td>
    </tr>
    <asp:Panel ID="pnlRows" runat="server">
    </asp:Panel>
    <tr>
        <td class="advcalctd" colspan="2">
            <asp:CheckBox ID="chkMonthlyIncome" runat="server" Text="Monthly Income" AutoPostBack="False" onclick="chkMonthlyIncome_ClientClick(this);"/>
        </td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
    </tr>
    <tr id="trMonthlyIncome" runat="server">
        <td colspan="6" class="advcalctd" align="left" style="text-align:left;">
            <table border="0" cellspacing="0" cellpadding="0" style="padding-left:10px;">
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rbMontlyIncome" runat="server" AutoPostBack="False">
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
                                        <ClientEvents OnValueChanged="SetCalcFalg" 
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
                                        <ClientEvents  
                                        OnMouseOver="SetControlsFromRadInput" 
                                        OnMouseOut="SetControlsFromRadInput" 
                                        OnFocus="SetControlsFromRadInput"
                                        OnMoveUp="SetControlsFromRadInput"
                                        OnMoveDown="SetControlsFromRadInput"
                                        OnValueChanged="SetCalcFalg"
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
        <col width="8%"/>
        <col width="1%" />
        <col width="12%"/>
        <col width="12%"/>
        <col width="12%"/>
        <col width="12%"/>
    </colgroup>
    <tr>
        <td align="left" style="padding-left:10px;padding-top:20px;"><asp:Button  ID="btnCalculate" runat="server" Text="Calculate" OnClick="btnCalculate_Click" /></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
    </tr>
</table>
</asp:Panel>
<div style="height:25px;">&nbsp;</div>