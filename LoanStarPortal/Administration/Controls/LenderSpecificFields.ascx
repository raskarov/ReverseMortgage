<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LenderSpecificFields.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.LenderSpecificFields" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<script language="javascript" type="text/javascript">
<!--
function SetClosingInfoVisibility(val,o){
    var s;
    if(val){
        s='inline';
    }else{
        s='none';
    }
    var rr = document.getElementById(o); 
    if(rr!=null) rr.style.display=s;
}
function CheckAllProducts(o,d1){
    var d = document.getElementById(d1);
    var e = d.getElementsByTagName('input');
    for (var i=0; i<e.length; i++){
        if (e[i].type=='checkbox'){
            e[i].checked=o.checked;
        }
    }
}
function CheckProduct(o,o11,d1){    
    var o1=document.getElementById(o11);
    var d=document.getElementById(d1);
    var e=d.getElementsByTagName('input');
    var cnt1=0;
    var cnt2=0;
    for (var i=0; i<e.length; i++){
        if ((e[i].type=='checkbox')&&(e[i].id!=o1.id)){
            cnt1 += e[i].checked?1:0;
            cnt2++;
        }
    } 
    if (o.checked){
        o1.checked = cnt1==cnt2;
    }else{
        o1.checked = false;
    }
}
-->
</script>
<div style="padding-left:8px;">
<table border="0" cellpadding="0" cellspacing="0" style="width:100%">
<tr>
    <td align="left" style="padding-left:20px;"><asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label></td>
</tr>
<tr>
    <td>
<radTS:RadTabStrip id="TabsMortgageProfiles" runat="server" Skin="Outlook" MultiPageID="RadMultiPage1" SelectedIndex="0" CausesValidation="false" Orientation="HorizontalTopToBottom">
    <Tabs>
        <radts:Tab Text="Lender settings" Value="Lender" ID="tabLender" runat="server"></radts:Tab>
        <radts:Tab Text="Originator settings" Value="Originator" ID="tabOriginator" runat="server"></radts:Tab>
        <radts:Tab Text="Servicer settings" Value="Servicer" ID="tabServicer" runat="server"></radts:Tab>
        <radts:Tab Text="Investor settings" Value="Investor" ID="tabInvestor" runat="server"></radts:Tab>
        <radts:Tab Text="Trustee settings" Value="Trustee" ID="tabTrustee" runat="server"></radts:Tab>
    </Tabs>
</radTS:RadTabStrip>
    <radTS:RadMultiPage id="RadMultiPage1" runat="server" SelectedIndex="0" Height="85%" EnableViewState="False">
        <radTS:PageView id="pvLender" runat="server" EnableViewState="False">
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="tdfieldlabel">
                        <asp:Label ID="Label5" runat="server" Text="Sponsor agent id code"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:TextBox ID="tbSponcorcode" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfieldlabel">
                        <asp:Label ID="Label15" runat="server" Text="Link to lender login page:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:TextBox ID="tbLenderLoginPage" runat="server" MaxLength="512"></asp:TextBox>                        
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfieldlabel">
                        <asp:Label ID="Label20" runat="server" Text="Phone Number"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <radI:RadMaskedTextBox runat="server" ID="tbPhone" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>
                        <asp:Label ID="lblErrPhone" runat="server" Text="" ForeColor="red"></asp:Label>                        
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfieldlabel">
                        <asp:Label ID="Labell" runat="server" Text="Location"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:DropDownList ID="ddlLenderLocation" runat="server">
                        </asp:DropDownList>
                        <asp:Label ID="lblErrLenderLocation" runat="server" Text="" ForeColor="red"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfieldlabel">
                        <asp:Label ID="Label14" runat="server" Text="Record and Return to:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:TextBox ID="tbrecordedReturnTo" runat="server" MaxLength="256"></asp:TextBox>
                        <asp:Label ID="lblErrRecordedReturnTo" runat="server" Text="" ForeColor="red"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>    
                <tr>
                    <td class="tdfieldlabel">
                        <asp:Label ID="Label29" runat="server" Text="Corporate Headquarters"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:TextBox ID="tbCorpHead" runat="server" MaxLength="256"></asp:TextBox>
                        <asp:Label ID="lblErrCorpHead" runat="server" Text="" ForeColor="red"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfieldlabel">
                        <asp:Label ID="Label30" runat="server" Text="Lender may at it's option require immediate payment-in-full if determined not eligible for insurance within:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <radI:RadNumericTextBox Type="Number" ID="tbdefaultMortNotInsured" runat="server" MinValue="0" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>                        
                        <asp:Label ID="lblErrDefaultMortNotInsured" runat="server" Text="" ForeColor="red"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>    
                <tr>
                    <td class="tdfieldlabel">
                        <asp:Label ID="Label31" runat="server" Text="A written statemtn from the secretary declining to insure shall me deemed proof within:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <radI:RadNumericTextBox Type="Number" ID="tbwrittenStatementFromSecretaryNotElegibility" runat="server" MinValue="0" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
                        <asp:Label ID="lblErrwrittenStatementFromSecretaryNotElegibility" runat="server" ForeColor="red" Text=""></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>        
                <tr>
                    <td class="tdfieldlabel">
                        <asp:Label ID="Label32" runat="server" Text="Operates under jurisdiction:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:DropDownList ID="ddlOperatesUnderJurisdiction" runat="server" DataTextField="Name" DataValueField="id"></asp:DropDownList>
                        <asp:Label ID="lblErrOperatesUnderJurisdiction" runat="server" ForeColor="red" Text=""></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>        
                <tr style="vertical-align:top">
                    <td class="tdfieldlabel">
                        <asp:Label ID="Label34" runat="server" Text="RecordReturnToAddress:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:TextBox ID="tbRecordReturnToAddress" runat="server" TextMode="multiLine" Rows="4" Width="300px"></asp:TextBox>
                        <asp:Label ID="lblErrRecordReturnToAddress" runat="server" ForeColor="red" Text=""></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>        
                <tr style="vertical-align:top">
                    <td class="tdfieldlabel">
                        <asp:Label ID="Label38" runat="server" Text="PlaceOfPaymentAddress:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:TextBox ID="tbPlaceOfPaymentAddress" runat="server" TextMode="multiLine" Rows="4" Width="300px"></asp:TextBox>
                        <asp:Label ID="lblPlaceOfPaymentAddress" runat="server" ForeColor="red" Text=""></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>        
                <tr style="vertical-align:top">
                    <td class="tdfieldlabel">
                        <asp:Label ID="lblRtrnFnlTtlPolAddress" runat="server" Text="RtrnFnlTtlPolAddress:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:TextBox ID="tbRtrnFnlTtlPolAddress" runat="server" TextMode="multiLine" Rows="4" Width="300px"></asp:TextBox>
                        <asp:Label ID="lblErrRtrnFnlTtlPolAddress" runat="server" ForeColor="red" Text=""></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr style="vertical-align:top">
                    <td class="tdfieldlabel">
                        <asp:Label ID="lblTitleInsClauseName" runat="server" Text="TitleCommitmentInsuredClause:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:TextBox ID="tbTitleCommitmentInsuredClause" runat="server" TextMode="MultiLine" Rows="4" Width="300px"></asp:TextBox>
                        <asp:Label ID="lblErrTitleCommitmentInsuredClause" runat="server" ForeColor="red" Text=""></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr style="vertical-align:top">
                    <td class="tdfieldlabel">
                        <asp:Label ID="lblMortgageeClause" runat="server" Text="MortgageeClause:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:TextBox ID="tbMortgageeClause" runat="server" TextMode="MultiLine" Rows="4" Width="300px"></asp:TextBox>
                        <asp:Label ID="lblErrMortgageeClause" runat="server" ForeColor="red" Text=""></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr style="vertical-align:top">
                    <td class="tdfieldlabel">
                        <asp:Label ID="lblLifeOfLoanName1" runat="server" Text="LifeOfLoanClause:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:TextBox ID="tbLifeOfLoanClause" runat="server" TextMode="MultiLine" Rows="4" Width="300px"></asp:TextBox>
                        <asp:Label ID="lblErrLifeOfLoanClause" runat="server" ForeColor="red" Text=""></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr style="vertical-align:top">
                    <td class="tdfieldlabel">
                        <asp:Label ID="lblRightToCancelAddress1" runat="server" Text="RightToCancelAddress:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:TextBox ID="tbRightToCancelAddress" runat="server" TextMode="MultiLine" Rows="4" Width="300px"></asp:TextBox>
                        <asp:Label ID="lblErrRightToCancelAddress" runat="server" ForeColor="red" Text=""></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfieldlabel">
                        <asp:Label ID="lblNCClosedLoanSeller" runat="server" Text="NCClosedLoanSeller:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:RadioButton ID="rbNCClosedLoanSeller1" runat="server" Text="does" GroupName="NCClosedLoanSeller"/>
                        <asp:RadioButton ID="rbNCClosedLoanSeller2" runat="server" Text="does not" GroupName="NCClosedLoanSeller"/>
                        <asp:Label ID="lblErrNCClosedLoanSeller" runat="server" ForeColor="red" Text=""></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfieldlabel">
                        <asp:Label ID="lblClosingFaxNumber" runat="server" Text="ClosingFaxNumber:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <radI:RadMaskedTextBox runat="server" ID="tbClosingFaxNumber" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>
                        <asp:Label ID="lblErrClosingFaxNumber" runat="server" ForeColor="red" Text=""></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr style="vertical-align:top">
                    <td class="tdfieldlabel">
                        <asp:Label ID="lblLenderMortgageClause" runat="server" Text="Lender mortgagee clause:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:TextBox ID="tbLenderMortgageClause" runat="server" TextMode="MultiLine" Rows="4" Width="300px"></asp:TextBox>
                        <asp:Label ID="lblErrLenderMortgageClause" runat="server" ForeColor="red" Text=""></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfieldlabel">
                        <asp:Label ID="Label60" runat="server" Text="Baydocs Lender ID:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:TextBox ID="tbBaydocsLenderID" runat="server" MaxLength="256"></asp:TextBox>
                        <asp:Label ID="lblBaydocsLenderID" runat="server" ForeColor="red" Text=""></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfieldlabel">
                        <asp:Label ID="Label61" runat="server" Text="Baydocs Lender Code:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:TextBox ID="tbBaydocsLenderCode" runat="server" MaxLength="256"></asp:TextBox>
                        <asp:Label ID="lblErrBaydocsLenderCode" runat="server" ForeColor="red" Text=""></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfieldlabel">
                        <asp:Label ID="Label1" runat="server" Text="Abbreviated Name:"></asp:Label>
                    </td>
                    <td class="tdfieldcontrol">
                        <asp:TextBox ID="tbAbbreviatedName" runat="server" MaxLength="256"></asp:TextBox>                        
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr style="vertical-align:top" id="trFHAGrid" runat="server" >
                    <td class="tdfieldlabel"><asp:Label ID="label2" runat="server" EnableViewState="false" Text="FHA Sposor ID:"/></td>    
                    <td colspan="3">
                    <asp:GridView ID="G" runat="server" SkinID="TotalGrid1" AutoGenerateColumns = "false" EnableViewState = "true" AllowPaging="true" AllowSorting="true" PageSize="10" Width="450px"
                                    OnRowDeleting = "G_RowDeleting"
                                    OnRowCancelingEdit="G_RowCancel" 
                                    OnRowEditing="G_RowEditing" 
                                    OnRowUpdating="G_RowUpdating" 
                                    OnRowDataBound="G_RowDataBound" 
                                    EmptyDataText="No records to display" 
                                    OnRowCommand="G_RowCommand" 
                                    OnSorting="G_Sorting" 
                                    OnPageIndexChanged="G_PageIndexChanged" 
                                    OnPageIndexChanging="G_PageIndexChanging"
                                >
                                <columns>
                                <asp:TemplateField HeaderText="State" SortExpression="State">
                                    <ItemTemplate>
                                        <asp:Label ID="lblState" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "State") %>'></asp:Label>
                                        <asp:DropDownList ID="ddlState" runat="server" Width="140px" Visible="false"></asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FHA Sponsor ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFHASponsorId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FHASponsorId") %>'></asp:Label>
                                        <asp:TextBox ID="tbFHASponsorId" runat="server" MaxLength="20" Width="90%" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfFHASponsorId" runat="server" ErrorMessage="*" ControlToValidate="tbFHASponsorId" Width="5%"  Visible="false"></asp:RequiredFieldValidator>
                                    </ItemTemplate>                                        
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" Width="250px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgEdit" CommandName="Edit" CommandArgument='<%# GetCurrentRow() %>' runat="server" ImageUrl="~/images/edit.gif"/>
                                        <asp:ImageButton ID="imgDelete" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' runat="server" ImageUrl="~/images/delete.gif"/>
                                        <asp:ImageButton ID="imgUpdate" CommandName="Update" runat="server" ImageUrl="~/images/update.gif" Visible="false"/>
                                        <asp:ImageButton ID="imgCancel" CommandName="Cancel" runat="server" CausesValidation="false" ImageUrl="~/images/cancel.gif" Visible="false"/>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lbl" EnableViewState="false"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                </asp:TemplateField>
                                </columns>
                                </asp:GridView>
                    </td>
                </tr>
                <tr style="height:30px;padding-top:5px">
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CausesValidation="false" />
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>            
            <div style="height:30px;">&nbsp;</div>
        </radTS:PageView>
        <radTS:PageView id="pvOriginator" runat="server" EnableViewState="False">
            <radTS:RadTabStrip id="tsOriginator" runat="server" Skin="Outlook" MultiPageID="RadMultiPage2" SelectedIndex="0" CausesValidation="false" Orientation="HorizontalTopToBottom">
                <Tabs>
                    <radts:Tab Text="General Info" Value="OriginatorGeneral" ID="OriginatorGeneral" runat="server"></radts:Tab>
                    <radts:Tab Text="State Specific Info" Value="OriginatorStateSpecific" ID="OriginatorStateSpecific" runat="server"></radts:Tab>
                    <radts:Tab Text="Closing Information" Value="OriginatorClosingInfo" ID="Tab1" runat="server"></radts:Tab>
                    <radts:Tab Text="State Licensing" Value="OriginatorStateLicensing" ID="OriginatorStateLicensing" runat="server"></radts:Tab>
                </Tabs>
            </radTS:RadTabStrip>
            <radTS:RadMultiPage id="RadMultiPage2" runat="server" SelectedIndex="0" Height="85%" EnableViewState="False">                
                <radTS:PageView id="pvOriginatorGeneral" runat="server" EnableViewState="False">
                    <div style="padding-top:10px;">
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="tdoriginator1">
                                <asp:Label ID="Label33" runat="server" Text="How many days in advance to notify counseling certs will expire?"></asp:Label>
                            </td>
                            <td class="tdoriginator2">
                                <radI:RadNumericTextBox Type="Number" ID="tbCounsDaysNotifyMeExp" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdoriginator1">
                                <asp:Label ID="Label35" runat="server" Text="How many days in advance to notify title commitments will expire?"></asp:Label>
                            </td>
                            <td class="tdoriginator2">
                                <radI:RadNumericTextBox Type="Number" ID="tbTitleDaysNotifyMeExp" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdoriginator1">
                                <asp:Label ID="Label36" runat="server" Text="How many days in advance to notify appraisals will expire?"></asp:Label>
                            </td>
                            <td class="tdoriginator2">
                                <radI:RadNumericTextBox Type="Number" ID="tbAppraisalDaysNotifyMeExp" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdoriginator1">
                                <asp:Label ID="Label37" runat="server" Text="How many days in advance to notify pest inspections will expire?"></asp:Label>
                            </td>
                            <td class="tdoriginator2">
                                <radI:RadNumericTextBox Type="Number" ID="tbPestDaysNotifyMeExp" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdoriginator1">
                                <asp:Label ID="Label39" runat="server" Text="How many days in advance to notify contractor bids will expire?"></asp:Label>
                            </td>
                            <td class="tdoriginator2">
                                <radI:RadNumericTextBox Type="Number" ID="tbBidDaysNotifyMeExp" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdoriginator1">
                                <asp:Label ID="Label40" runat="server" Text="How many days in advance to notify water tests will expire?"></asp:Label>
                            </td>
                            <td class="tdoriginator2">
                                <radI:RadNumericTextBox Type="Number" ID="tbWaterTestDaysNotifyMeExp" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdoriginator1">
                                <asp:Label ID="Label41" runat="server" Text="How many days in advance to notify septic inspections will expire?"></asp:Label>
                            </td>
                            <td class="tdoriginator2">
                                <radI:RadNumericTextBox Type="Number" ID="tbSepticInspDaysNotifyMeExp" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdoriginator1">
                                <asp:Label ID="Label42" runat="server" Text="How many days in advance to notify oil tank inspections will expire?"></asp:Label>
                            </td>
                            <td class="tdoriginator2">
                                <radI:RadNumericTextBox Type="Number" ID="tbOilTankInspDaysNotifyMeExp" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdoriginator1">
                                <asp:Label ID="Label68" runat="server" Text="How many days in advance to notify roof inspections will expire?"></asp:Label>
                            </td>
                            <td class="tdoriginator2">
                                <radI:RadNumericTextBox Type="Number" ID="tbRoofInspDaysNotifyMeExp" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdoriginator1">
                                <asp:Label ID="Label69" runat="server" Text="How many days in advance to notify flood certs will expire?"></asp:Label>
                            </td>
                            <td class="tdoriginator2">
                                <radI:RadNumericTextBox Type="Number" ID="tbFloodCertDaysNotifyMeExp" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
                            </td>                        
                        </tr>
                        <tr>
                            <td class="tdoriginator1">
                                <asp:Label ID="Label70" runat="server" Text="How many days in advance to notify credit reports will expire?"></asp:Label>
                            </td>
                            <td class="tdoriginator2">
                                <radI:RadNumericTextBox Type="Number" ID="tbCreditReportDaysNotifyMeExp" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdoriginator1">
                                <asp:Label ID="Label71" runat="server" Text="How many days in advance to notify LDP printouts will expire?"></asp:Label>
                            </td>
                            <td class="tdoriginator2">
                                <radI:RadNumericTextBox Type="Number" ID="tbLDPDaysNotifyMeExp" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdoriginator1">
                                <asp:Label ID="Label72" runat="server" Text="How many days in advance to notify EPLS printouts will expire?"></asp:Label>
                            </td>
                            <td class="tdoriginator2">
                                <radI:RadNumericTextBox Type="Number" ID="tbEPLSDaysNotifyMeExp" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdoriginator1">
                                <asp:Label ID="Label73" runat="server" Text="How many days in advance to notify CAIVRS authorizations will expire?"></asp:Label>
                            </td>
                            <td class="tdoriginator2">
                                <radI:RadNumericTextBox Type="Number" ID="tbCaivrsDaysNotifyMeExp" runat="server" MinValue="0" MaxValue="9999" Width="100px" NumberFormat-DecimalDigits="0"></radI:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdoriginator1">
                                <asp:Label ID="Label3" runat="server" Text="Baydocs Originator ID:"></asp:Label>
                            </td>
                            <td class="tdoriginator2">
                                <asp:TextBox ID="tbBaydocsID" runat="server" MaxLength="100" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdoriginator1">
                                <asp:Label ID="Label27" runat="server" Text="Does your company close loans in your companies name?"></asp:Label>
                            </td>
                            <td class="tdoriginator2">
                                <asp:CheckBox ID="cbIsCompanyCloseLoans" runat="server" />
                            </td>
                        </tr>
                        </table>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2">
                            <div id="divClosingData" runat="server">
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="tdoriginator1">
                                            <asp:Label ID="Label13" runat="server" Text="Name:"></asp:Label>
                                        </td>
                                        <td class="tdoriginator2">
                                            <asp:TextBox ID="tbClosingName" runat="server" MaxLength="256"  Width="250px"></asp:TextBox>&nbsp;&nbsp;
                                            <asp:Label ID="lblErrClosingName" runat="server" Text="" ForeColor="red"></asp:Label>
                                        </td>                        
                                    </tr>
                                    <tr valign="top">
                                        <td class="tdoriginator1">
                                            <asp:Label ID="Label24" runat="server" Text="Address:"></asp:Label>
                                        </td>
                                        <td class="tdoriginator2">
                                            <asp:TextBox ID="tbClosingAddress" runat="server" TextMode="MultiLine" Rows="4"  Width="250px"></asp:TextBox>&nbsp;&nbsp;
                                            <asp:Label ID="lblErrClosingAddress" runat="server" Text="" ForeColor="red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdoriginator1">
                                            <asp:Label ID="Label84" runat="server" Text="City:"></asp:Label>
                                        </td>
                                        <td class="tdoriginator2">
                                            <asp:TextBox ID="tbClosingCity" runat="server" MaxLength="50"  Width="200px"></asp:TextBox>&nbsp;&nbsp;
                                            <asp:Label ID="lblErrClosingCity" runat="server" Text="" ForeColor="red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdoriginator1">
                                            <asp:Label ID="Label86" runat="server" Text="State:"></asp:Label>
                                        </td>
                                        <td class="tdoriginator2">
                                            <asp:DropDownList ID="ddlClosingState" runat="server"  Width="200px"></asp:DropDownList>&nbsp;&nbsp;
                                            <asp:Label ID="lblErrClosingState" runat="server" Text="" ForeColor="red"></asp:Label>
                                        </td>                        
                                    </tr>
                                    <tr>
                                        <td class="tdoriginator1">
                                            <asp:Label ID="Label88" runat="server" Text="Zip:"></asp:Label>
                                        </td>
                                        <td class="tdoriginator2">
                                            <radI:RadMaskedTextBox runat="server" ID="tbClosingZip" Mask="#####" DisplayMask="#####" Width="100px"></radI:RadMaskedTextBox>&nbsp;&nbsp;
                                            <asp:Label ID="lblErrClosingZip" runat="server" Text="" ForeColor="red"></asp:Label>
                                        </td>                        
                                    </tr>
                                    <tr>
                                        <td class="tdoriginator1">
                                            <asp:Label ID="Label87" runat="server" Text="Phone Number:"></asp:Label>
                                        </td>
                                        <td class="tdoriginator2">
                                            <radI:RadMaskedTextBox runat="server" ID="tbClosingPhoneNumber" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>&nbsp;&nbsp;
                                            <asp:Label ID="lblErrClosingPhoneNumber" runat="server" Text="" ForeColor="red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdoriginator1">
                                            <asp:Label ID="Label25" runat="server" Text="State of Incorporation:"></asp:Label>
                                        </td>
                                        <td class="tdoriginator2">
                                            <asp:DropDownList ID="ddlClosingStateOfInc" runat="server"  Width="200px"></asp:DropDownList>&nbsp;&nbsp;
                                            <asp:Label ID="lblErrClosingStateOfInc" runat="server" Text="" ForeColor="red"></asp:Label>
                                        </td>
                                    </tr>                                
                                </table>
                            </div>                        
                        </td>
                    </tr>
                    <tr style="height:30px;padding-top:5px">
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="btnOriginator" runat="server" Text="Save" OnClick="btnSaveOriginator_Click" CausesValidation="true" ValidationGroup="OriginatorGeneral" />
                        </td>                    
                    </tr>
                </table>
                </div>
                </radTS:PageView>
                <radTS:PageView id="pvOriginatorStateSpecific" runat="server" EnableViewState="False">
                    <radTS:RadTabStrip id="tsOriginatorState" runat="server" Skin="Outlook" MultiPageID="RadMultiPage3" SelectedIndex="0" CausesValidation="false" Orientation="HorizontalTopToBottom">
                        <Tabs>
                            <radts:Tab Text="State" Value="OriginatorState" ID="OriginatorState" runat="server"></radts:Tab>
                            <radts:Tab Text="Details" Value="OriginatorStateDetails" ID="OriginatorStateDetails" runat="server"></radts:Tab>
                        </Tabs>
                    </radTS:RadTabStrip>
                    <radTS:RadMultiPage id="RadMultiPage3" runat="server" SelectedIndex="0" Height="85%" EnableViewState="False">
                        <radTS:PageView id="pvState" runat="server" EnableViewState="False">
                            <table border="0" style="width:100%;vertical-align:top;margin-top:10px;margin-bottom:10px;" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:DataList ID="dlStates" runat="server" RepeatColumns="3" RepeatDirection="Vertical" OnItemDataBound="dlStates_ItemDataBound"  EnableViewState="False">
                                             <ItemTemplate>
                                                <asp:LinkButton ID="lbState" runat="server" OnClick="lbStateSelected_Click"></asp:LinkButton>
                                             </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                            </table>
                        </radTS:PageView>
                        <radTS:PageView id="pvDetails" runat="server" EnableViewState="False">
                            <div style="padding-top:10px;">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="tdoriginator1"><asp:Label ID="Label48" runat="server" Text="Address1"></asp:Label></td>
                                    <td class="tdoriginator2">
                                        <asp:TextBox ID="tbOriginatorAddress1" runat="server" MaxLength="256" CssClass="longOriginatortb"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="rfvOriginatorAddress" runat="server" ErrorMessage="*" ControlToValidate="tbOriginatorAddress1" ValidationGroup="OriginatorStateDetail"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdoriginator1">
                                        <asp:Label ID="Label49" runat="server" Text="Address2"></asp:Label>
                                    </td>
                                    <td class="tdoriginator2">
                                        <asp:TextBox ID="tbOriginatorAddress2" runat="server" MaxLength="256" CssClass="longOriginatortb"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdoriginator1"><asp:Label ID="Label51" runat="server" Text="Name"></asp:Label></td>
                                    <td class="tdoriginator2">
                                        <asp:TextBox ID="tbOriginatorName" runat="server" MaxLength="100" CssClass="longOriginatortb"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="rfvOriginatorName" runat="server" ErrorMessage="*" ControlToValidate="tbOriginatorName"  ValidationGroup="OriginatorStateDetail"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdoriginator1"><asp:Label ID="Label54" runat="server" Text="What is your FHA Originator ID number?"></asp:Label></td>
                                    <td class="tdoriginator2">
                                        <asp:TextBox ID="tbFHAOriginatorNumber" runat="server" MaxLength="50" CssClass="longOriginatortb" Width="250px"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="rfvFHAOriginatorNumber" runat="server" ErrorMessage="*" ControlToValidate="tbFHAOriginatorNumber" ValidationGroup="OriginatorStateDetail"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdoriginator1"><asp:Label ID="Label56" runat="server" Text="City"></asp:Label></td>
                                    <td class="tdoriginator2">
                                        <asp:TextBox ID="tbOriginatorCity" runat="server" MaxLength="50"  Width="250px"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="rfvOriginatorCity" runat="server" ErrorMessage="*" ControlToValidate="tbOriginatorCity"  ValidationGroup="OriginatorStateDetail"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdoriginator1"><asp:Label ID="Label57" runat="server" Text="Fax"></asp:Label></td>
                                    <td class="tdoriginator2">
                                        <radI:RadMaskedTextBox runat="server" ID="tbOriginatorFax" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="tbOriginatorFax" ValidationGroup="OriginatorStateDetail"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdoriginator1"><asp:Label ID="Label58" runat="server" Text="Phone"></asp:Label></td>
                                    <td class="tdoriginator2">
                                        <radI:RadMaskedTextBox runat="server" ID="tbOriginatorPhone" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="tbOriginatorPhone" ValidationGroup="OriginatorStateDetail"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdoriginator1"><asp:Label ID="Label59" runat="server" Text="Zip"></asp:Label></td>
                                    <td class="tdoriginator2">
                                        <radI:RadMaskedTextBox runat="server" ID="tbOriginatorZip" Mask="#####" DisplayMask="#####" Width="100px"></radI:RadMaskedTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="tbOriginatorZip" ValidationGroup="OriginatorStateDetail"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr style="padding-top:10px;">
                                    <td class="tdoriginator1"></td>
                                    <td class="tdoriginator2"><asp:Button ID="btnOriginatordetails" runat="server" Text="Save" OnClick="btnSaveOriginatorDetails_Click" ValidationGroup="OriginatorStateDetail" CausesValidation="true"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnDeleteOriginatorDetails" runat="server" Text="Delete" OnClick="btnDeleteOriginatorDetails_Click" CausesValidation="false" /></td>
                                </tr>
                            </table>
                            </div>
                        </radTS:PageView>                        
                    </radTS:RadMultiPage>
                </radTS:PageView>
                <radTS:PageView id="pvOriginatorClosingInfo" runat="server" EnableViewState="False">
                    <div style="padding-top:10px;">
                        <table border="0" style="width:100%;vertical-align:top;margin-top:10px;margin-bottom:10px;" cellpadding="0" cellspacing="0">
                            <tr>
                                <td><asp:Label ID="Label26" runat="server" Text="Please select product which orginator can close" Font-Bold="true"/></td>
                            </tr>
                            <tr style="padding-bottom:10px; padding-top:10px;"><td style="padding-left:20px"><asp:CheckBox ID="cbAllProducts" runat="server"  Text="Check/Uncheck All"/></td></tr>
                                <tr>
                                    <td>
                                        <div id="productdiv" runat="server">
                                            <asp:DataList ID="dlProducts" runat="server" RepeatColumns="4" RepeatDirection="Vertical" OnItemDataBound="dlProducts_ItemDataBound"  EnableViewState="False">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbProduct" runat="server" TextAlign="Right"/>
                                                </ItemTemplate>
                                            </asp:DataList>
                                            </div>
                                    </td>
                                </tr> 
                                <tr style="height:30px;padding-top:5px">
                                    <td>
                                        <asp:Button ID="btnSaveClosingOriginator" runat="server" Text="Save" OnClick="btnSaveClosingOriginator_Click" CausesValidation="true" ValidationGroup="OriginatorClosing" />
                                    </td>                    
                                </tr>                                                                       
                        </table>                            
                </div>
             </radTS:PageView>
                <radTS:PageView id="pvOriginatorStateLicensing" runat="server" EnableViewState="False">
                    <div style="padding-top:10px;">
                        <table border="0" style="width:100%;vertical-align:top;margin-top:10px;margin-bottom:10px;" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:GridView ID="gStateLicense" runat="server" SkinID="TotalGrid1" AutoGenerateColumns = "false" EnableViewState = "true" AllowPaging="true" AllowSorting="true" PageSize="20" Width="560px"
                                        OnRowDeleting = "gStateLicense_RowDeleting"
                                        OnRowCancelingEdit="gStateLicense_RowCancel" 
                                        OnRowEditing="gStateLicense_RowEditing" 
                                        OnRowUpdating="gStateLicense_RowUpdating" 
                                        OnRowDataBound="gStateLicense_RowDataBound" 
                                        EmptyDataText="No records to display" 
                                        OnRowCommand="gStateLicense_RowCommand" 
                                        OnSorting="gStateLicense_Sorting" 
                                        OnPageIndexChanged="gStateLicense_PageIndexChanged" 
                                        OnPageIndexChanging="gStateLicense_PageIndexChanging"
                                    >
                                    <columns>
                                        <asp:TemplateField HeaderText="State" SortExpression="StateName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLicenseState" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StateName") %>'></asp:Label>
                                                <asp:DropDownList ID="ddlLicenseState" runat="server" Width="140px" Visible="false"></asp:DropDownList>
                                                <asp:RangeValidator ID="rvLicenseState" runat="server" ErrorMessage="*" MinimumValue="1" MaximumValue="100" Type="Integer" ControlToValidate="ddlLicenseState"></asp:RangeValidator>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="License Number" SortExpression="LicenseNumber">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLicenseNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LicenseNumber") %>'></asp:Label>
                                                <asp:TextBox ID="tbLicenseNumber" runat="server" MaxLength="50" Width="90%" Visible="false"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfLicenseNumber" runat="server" ErrorMessage="*" ControlToValidate="tbLicenseNumber" Width="5%"  Visible="false"></asp:RequiredFieldValidator>
                                            </ItemTemplate>                                        
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="250px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Expiration Date" SortExpression="ExpirationDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpirationDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ExpirationDate","{0:MM/dd/yyyy}") %>'></asp:Label>
                                                <radI:RadDateInput runat="server" ID="tbExpirationDate" Visible="false" Width="90px"></radI:RadDateInput>                                                
                                                <asp:RequiredFieldValidator ID="rfExpirationDate" runat="server" ErrorMessage="*" ControlToValidate="tbExpirationDate" Width="5%"  Visible="false"></asp:RequiredFieldValidator>
                                            </ItemTemplate>                                        
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                        </asp:TemplateField>                                        
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgEdit" CommandName="Edit" CommandArgument='<%# GetCurrentLicensingRow() %>' runat="server" ImageUrl="~/images/edit.gif"/>
                                                <asp:ImageButton ID="imgDelete" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' runat="server" ImageUrl="~/images/delete.gif"/>
                                                <asp:ImageButton ID="imgUpdate" CommandName="Update" runat="server" ImageUrl="~/images/update.gif" Visible="false"/>
                                                <asp:ImageButton ID="imgCancel" CommandName="Cancel" runat="server" CausesValidation="false" ImageUrl="~/images/cancel.gif" Visible="false"/>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lbl" EnableViewState="false"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                        </asp:TemplateField>
                                    </columns>
                                </asp:GridView>
                                </td>
                            </tr>
                        </table>                            
                    </div>
             </radTS:PageView>                                                   
        </radTS:RadMultiPage>            
        </radTS:PageView>
        <radTS:PageView id="pvServicer" runat="server" EnableViewState="False">        
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr style="padding-top:10px;">
                    <td class="tdservicer1">
                        <asp:Label ID="Label4" runat="server" Text="Name"></asp:Label>
                    </td>
                    <td class="tdservicer2">
                        <asp:TextBox ID="tbServicerName" runat="server" MaxLength="100" Width="300px" ></asp:TextBox>
                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" ControlToValidate="tbServicerName" ValidationGroup="Servicer"></asp:RequiredFieldValidator>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="tdservicer1">
                        <asp:Label ID="Label6" runat="server" Text="Baydocs Servicer ID"></asp:Label>
                    </td>
                    <td class="tdservicer2">
                        <asp:TextBox ID="tbBaydocServicerId" runat="server" MaxLength="50"  Width="300px"></asp:TextBox>
                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*" ControlToValidate="tbBaydocServicerId" ValidationGroup="Servicer"></asp:RequiredFieldValidator>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="tdservicer1">
                        <asp:Label ID="Label12" runat="server" Text="Location"></asp:Label>
                    </td>
                    <td class="tdservicer2">
                        <asp:DropDownList ID="ddlServicerLocation" runat="server" Width="200px"></asp:DropDownList>
                        &nbsp;<asp:RangeValidator ID="rvddlServicerLocation" runat="server" ErrorMessage="*" MinimumValue="1" MaximumValue="60" ControlToValidate="ddlServicerLocation" Type="Integer" ValidationGroup="Servicer"></asp:RangeValidator>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="tdservicer1">
                        <asp:Label ID="Label10" runat="server" Text="Phone"></asp:Label>
                    </td>
                    <td class="tdservicer2">
                        <radI:RadMaskedTextBox runat="server" ID="tbServicerPhone" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>
                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="*" ControlToValidate="tbServicerPhone" ValidationGroup="Servicer"></asp:RequiredFieldValidator>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="tdservicer1">
                        <asp:Label ID="Label11" runat="server" Text="Fax"></asp:Label>
                    </td>
                    <td class="tdservicer2">
                        <radI:RadMaskedTextBox runat="server" ID="tbServicerFax" Mask="(###) ###-####" DisplayMask="(###) ###-####" Width="100px"></radI:RadMaskedTextBox>
                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="*" ControlToValidate="tbServicerFax" ValidationGroup="Servicer"></asp:RequiredFieldValidator>
                    </td>
                    <td>&nbsp;</td>
                </tr>            
                <tr style="height:30px;padding-top:5px">
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnServicer" runat="server" Text="Save" OnClick="btnSaveServicer_Click" CausesValidation="true" ValidationGroup="Servicer"/>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <div style="height:30px;">&nbsp;</div>
        </radTS:PageView>
        <radTS:PageView id="pvInvestor" runat="server" EnableViewState="False">
            <div style="padding-left:5px;padding-top:5px">
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="tdinvestor1">
                        <asp:Label ID="Label52" runat="server" Text="State"></asp:Label>
                    </td>
                    <td class="tdInvestor2">
                        <asp:DropDownList ID="ddlInvestorLocation" runat="server" ></asp:DropDownList>
                        <asp:RangeValidator ID="rvddlInvestorLocation" runat="server" ControlToValidate="ddlInvestorLocation" Type="Integer"
                            ErrorMessage="*" MaximumValue="1000" MinimumValue="1" ValidationGroup="Investor"></asp:RangeValidator></td>                    
                </tr>
                <tr style="height:30px;padding-top:5px">
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnSaveInvestor" runat="server" Text="Save" OnClick="btnSaveInvestor_Click"  ValidationGroup="Investor" CausesValidation="true"/>
                    </td>                    
                </tr>
            </table>
            </div>
            <div style="height:30px;">&nbsp;</div>
        </radTS:PageView>
        <radTS:PageView id="pvTrustee" runat="server" EnableViewState="False">
            <div style="padding-left:5px;padding-top:5px">
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="tdinvestor1">
                        <asp:Label ID="Label65" runat="server" Text="Location"></asp:Label>
                    </td>
                    <td class="tdInvestor2">
                        <asp:DropDownList ID="ddlTrusteeLocation" runat="server" ></asp:DropDownList>
                        <asp:RangeValidator ID="rvTrusteeLocation" runat="server" ControlToValidate="ddlTrusteeLocation" Type="Integer"
                            ErrorMessage="*" MaximumValue="1000" MinimumValue="1" ValidationGroup="Trustee"></asp:RangeValidator></td>                    
                </tr>
                <tr style="height:30px;padding-top:5px">
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnSaveTrustee" runat="server" Text="Save" OnClick="btnSaveTrustee_Click" ValidationGroup="Trustee" CausesValidation="true"/>
                    </td>                    
                </tr>
            </table>
            </div>
            <div style="height:30px;">&nbsp;</div>
        </radTS:PageView>        
    </radTS:RadMultiPage>
    </td>
</tr>
</table>
</div>
