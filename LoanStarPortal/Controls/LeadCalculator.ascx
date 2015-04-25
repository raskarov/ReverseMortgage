<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeadCalculator.ascx.cs" Inherits="LoanStarPortal.Controls.LeadCalculator" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr valign="top">
	    <td valign="top" style="width:30%">
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
				    <td valign="top" align="left" style="padding:3px">
					    <table cellspacing="1" cellpadding="0" width="100%" border="0" class="lcTable">
						    <tr>
							    <td valign="middle" align="left" style="width:138px;height:15px;" class="lcheadertd">Desired Gross Income</td>
                                <td valign="middle" align="left" style="height:15px">                                    
									<asp:TextBox ID="tbGrossIncome" runat="server" CssClass="lcInputText" AutoPostBack="True" OnTextChanged="DataChanged_TextChanged"></asp:TextBox>
								</td>
                            </tr>
							<tr>
							    <td valign="middle" align="left" style="height:15px;" class="lclabeltd">Production Needed</td>
            					<td valign="middle" align="left">
									<asp:TextBox ID="tbProductionNeeded" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
								</td>									
	    					</tr>
							<tr>
							    <td valign="middle" align="left" style="height:15px;" class="lclabeltd">Standard Commission,%</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbStandardCommission" runat="server" CssClass="lcInputText" AutoPostBack="True" OnTextChanged="DataChanged_TextChanged" ></asp:TextBox>
                                </td>
							</tr>
                            <tr>
							    <td valign="middle" align="left" style="height:15px;" class="lclabeltd">Average Max Claim</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbAverageMaxClaim" runat="server" CssClass="lcInputText" AutoPostBack="True" OnTextChanged="DataChanged_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
							<tr>
							    <td valign="middle" align="left" style="height:15px;" class="lclabeltd">Estimated Fall-Out,%</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbEstimatedFallout" runat="server" CssClass="lcInputText" AutoPostBack="True" OnTextChanged="DataChanged_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
							<tr>
							    <td valign="middle" align="left" style="height:75px;">&nbsp;</td>
								<td valign="middle" align="left">&nbsp;</td>
                            </tr>
							<tr align="center">
							    <td valign="middle" colspan="2" style="height:20px" class="lcheadertd">Total Closings Needed</td>
                            </tr>
							<tr>
							    <td valign="middle" align="left" style="height:20px;" class="lclabeltd">Goal for year-End</td>
								<td valign="middle" align="left" style="height:20px">
                                    <asp:TextBox ID="tbGoalYearEnd" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
							<tr>
							    <td valign="middle" align="left" style="height:20px;" class="lclabeltd">Average per Month</td>
								<td valign="middle" align="left" style="height:20px">
                                    <asp:TextBox ID="tbMonthAverage" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
							<tr>
							    <td valign="middle" align="left" style="height:20px;" class="lclabeltd">Average per Week</td>
								<td valign="middle" align="left" style="height:20px">
                                    <asp:TextBox ID="tbWeekAverage" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                         </table>
                    </td>
				</tr>
            </table>
        </td>
		<td valign="top" style="width:35%">
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
                    <td valign="top" align="left" style="padding:3px">
                        <table cellspacing="1" cellpadding="0" width="100%" border="0" class="lcTable">
							<tr>
								<td valign="middle" align="left" style="height:20px; width:150px" class="lcheadertd">Self Sourced Closings,%</td>
								<td valign="middle" align="left" >
                                    <asp:TextBox ID="tbSourcedClosing" runat="server" CssClass="lcInputText" AutoPostBack="True" OnTextChanged="DataChanged_TextChanged"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Net Commission,%</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbNetCommission1" runat="server"  CssClass="lcCalcInputText" Enabled="false" AutoPostBack="True" OnTextChanged="DataChanged_TextChanged"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Per Month Unit Volume</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbPerMonthVolume1" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Income / Month</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbIncomeMonth1" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Income / Year</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbIncomeYear1" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">% of leads to Apps</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbLeadsToApps1" runat="server" CssClass="lcInputText" AutoPostBack="True" OnTextChanged="DataChanged_TextChanged"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Required Leads per Month</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbLeadsPerMonth1" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Required Leads per Week</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbLeadsPerWeek1" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Required Leads per Week Day</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbLeadsPerDay1" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr style="height:5px;"><td colspan="2">&nbsp;</td></tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lcheadertd">RMC Referred Closings,%</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbRmcClosing" runat="server" CssClass="lcInputText" AutoPostBack="True" OnTextChanged="DataChanged_TextChanged"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Net Commission,%</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbNetCommission2" runat="server" CssClass="lcInputText" AutoPostBack="True" OnTextChanged="DataChanged_TextChanged"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Per Month Unit Volume</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbPerMonthVolume2" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Income / Month</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbIncomeMonth2" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Income / Year</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbIncomeYear2" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">% of Leads of Apps</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbLeadsToApps2" runat="server" CssClass="lcInputText" AutoPostBack="True" OnTextChanged="DataChanged_TextChanged"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Required Leads per Month</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbLeadsPerMonth2" runat="server" CssClass="lcInputText"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Required Leads per Week</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbLeadsPerWeek2" runat="server" CssClass="lcInputText"></asp:TextBox>
                                </td>
							</tr>
        				</table>
                    </td>
			    </tr>
			</table>
		</td>
		<td valign="top">
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
				    <td valign="top" align="left" style="padding:3px">
					    <table cellspacing="1" cellpadding="0" width="100%" border="0"  class="lcTable">
							<tr>
								<td valign="middle" align="left" style="width:150px;height:20px" class="lcheadertd">Brokers-In Closings,%</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbBrokerClosing1" runat="server" CssClass="lcInputText" AutoPostBack="True" OnTextChanged="DataChanged_TextChanged"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Net Commission,%</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbNetCommission3" runat="server" CssClass="lcInputText" AutoPostBack="True" OnTextChanged="DataChanged_TextChanged"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Per Month Unit Volume</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbPerMonthVolume3" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Income / Month</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbIncomeMonth3" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Income / Year</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbIncomeYear3" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">% of leads to Apps</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbLeadsToApps3" runat="server" CssClass="lcInputText" AutoPostBack="True"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Required Leads per Month</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbLeadsPerMonth3" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Required Leads per Week</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbLeadsPerWeek3" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Required Leads per Week Day</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbLeadsPerDay3" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr style="height:5px"><td>&nbsp;</td></tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lcheadertd">Broker-In Closings,%</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbBrokerClosing2" runat="server" CssClass="lcInputText" AutoPostBack="True" OnTextChanged="DataChanged_TextChanged"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Net Commission,%</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbNetCommission4" runat="server" CssClass="lcInputText" AutoPostBack="True" OnTextChanged="DataChanged_TextChanged"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Per Month Unit Volume</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbPerMonthVolume4" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Income / Month</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbIncomeMonth4" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Income / Year</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbIncomeYear4" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">% of Leads of Apps</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbLeadsToApps4" runat="server" CssClass="lcInputText" AutoPostBack="True" OnTextChanged="DataChanged_TextChanged"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Required Leads per Month</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbLeadsPerMonth4" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
							<tr>
								<td valign="middle" align="left" style="height:20px" class="lclabeltd">Required Leads per Week</td>
								<td valign="middle" align="left">
                                    <asp:TextBox ID="tbLeadsPerWeek4" runat="server" CssClass="lcCalcInputText" Enabled="false"></asp:TextBox>
                                </td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<div style="height:25px;">&nbsp;</div>