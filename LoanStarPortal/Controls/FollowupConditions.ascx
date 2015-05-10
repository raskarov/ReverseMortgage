<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FollowupConditions.ascx.cs" Inherits="LoanStarPortal.Controls.FollowupConditions" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<%@ Register Assembly="RadPanelbar.Net2" Namespace="Telerik.WebControls" TagPrefix="radPnlB" %>
<%@ Register Src="EmailAdd.ascx" TagName="EmailAdd" TagPrefix="uc1" %>

<script type="text/javascript" src="scripts/users/FollowupConditions.js"></script>

<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Width="100%" BorderWidth="0" BorderStyle="None" Skin="Default">
    <radspl:RadPane ID="TopPane" runat="server" Height="29px" Scrolling="None">
        <div class="paneTitle">
            <table border="0" cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td class="title" nowrap="nowrap">
                        <asp:Label ID="lblTitle" runat="server" Text="Condition / Task manager"></asp:Label></td>
                </tr>
            </table>
        </div>
    </radspl:RadPane>

    <radspl:RadSplitBar ID="RadSplitBar3" runat="server" CollapseMode="None" EnableResize="false" Visible="false" />
    <radspl:RadPane ID="MiddlePane" runat="server" Scrolling="Y">
        <asp:Panel ID="PanelTasks" runat="server">
            <radG:RadGrid ID="gridConditions" Skin="Default" runat="server" AutoGenerateColumns="False" EnableAJAX="False" GridLines="Vertical"
                AllowPaging="True" PageSize="30"
                OnDetailTableDataBind="gridConditions_DetailTableDataBind"
                OnNeedDataSource="gridConditions_NeedDataSource"
                OnItemCommand="gridConditions_ItemCommand"
                OnItemDataBound="gridConditions_ItemDataBound"
                OnPreRender="gridConditions_PreRender"
                OnPageIndexChanged="gridConditions_PageIndexChanged">
                <MasterTableView DataKeyNames="ID">
                    <DetailTables>
                        <radG:GridTableView DataKeyNames="ID" Name="Description" Width="100%" ShowHeader="False">
                            <Columns>
                                <radG:GridBoundColumn DataField="Description"></radG:GridBoundColumn>
                            </Columns>
                        </radG:GridTableView>
                    </DetailTables>

                    <NoRecordsTemplate>
                        <h2>No items</h2>
                    </NoRecordsTemplate>
                    <%--<ExpandCollapseColumn Visible="False">
                                    <HeaderStyle Width="19px" />
                                </ExpandCollapseColumn>
                                <RowIndicatorColumn Visible="False">
                                    <HeaderStyle Width="20px" />
                                </RowIndicatorColumn>--%>
                    <Columns>
                        <radG:GridBoundColumn HeaderText="ID" DataField="ID" UniqueName="ID" Display="False">
                        </radG:GridBoundColumn>
                        <radG:GridButtonColumn UniqueName="TitleColumn" HeaderText="Title" CommandName="LoadItem" DataTextField="Title" ItemStyle-CssClass="tdLinkToEdit" ItemStyle-Width="50%"></radG:GridButtonColumn>
                        <radG:GridTemplateColumn HeaderText="Authority Level" UniqueName="AuthorityLevel" DataField="AuthorityLevel" HeaderStyle-Width="40px" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <input type="hidden" class="row_id" value="<%# DataBinder.Eval(Container.DataItem, "ID") %>" />
                                <input type="hidden" class="row_completed" value="<%# DataBinder.Eval(Container.DataItem, "Completed") %>" />
                                <input type="hidden" class="row_diffdays" value="<%# DataBinder.Eval(Container.DataItem, "DiffDays") %>" />
                                <span><%# DataBinder.Eval(Container.DataItem, "AuthorityLevelName") %></span>
                            </ItemTemplate>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Frequency" UniqueName="Frequency" DataField="RecurrenceName" HeaderStyle-Width="40px" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <span><%# DataBinder.Eval(Container.DataItem, "RecurrenceName") %></span>
                            </ItemTemplate>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Next Follow Up Date" UniqueName="NextFollowUpDate" HeaderStyle-Width="40px" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <span><%# DataBinder.Eval(Container.DataItem, "NextFollowUpDate") %></span>
                            </ItemTemplate>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <div style="vertical-align: top;">
                                    <asp:ImageButton ID="ibtnEmail" runat="server" ImageUrl="~/Images/Add_Email.gif" BorderWidth="0" ImageAlign="Bottom" CommandName="CreateEmail" />&nbsp;&nbsp;/&nbsp;&nbsp;<asp:ImageButton ID="ibtnNote" runat="server" ImageUrl="~/Images/Add-Note.gif" BorderWidth="0" ImageAlign="Bottom" CommandName="CreateNote" />
                                </div>
                            </ItemTemplate>
                        </radG:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <PagerStyle Mode="NumericPages" />
                <%--<ClientSettings EnablePostBackOnRowClick="false"></ClientSettings>--%>
            </radG:RadGrid>

            <br />
            <br />
            &nbsp;<asp:LinkButton ID="btnAddCondition" runat="server" Text="Add Condition/Task" CssClass="AddCondition" OnClick="btnAddCondition_Click"></asp:LinkButton>

        </asp:Panel>
        <asp:Panel ID="PanelEmailAdd" runat="server" Visible="False" Width="100%" Height="100%">
            <uc1:EmailAdd ID="EmailAdd1" runat="server" />
        </asp:Panel>
        <asp:Panel ID="PanelUnderwriter" runat="server" Visible="false">
            <br />
            <br />
            <table border="0" cellpadding="3" cellspacing="3" style="padding-top: 30px; padding-left: 30px; width: 100%; border-top: solid 1px #000000;">
                <colgroup>
                    <col width="25%" />
                    <col width="75%" />
                </colgroup>
                <tr runat="server" id="trCredit" visible="false">
                    <td>
                        <asp:CheckBox ID="chkCredit" runat="server" Text="Credit approved" AutoPostBack="True" OnCheckedChanged="chkCredit_CheckedChanged" /></td>
                    <td>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnPrintCCDE" runat="server" Text="Print CCDE" CausesValidation="False" /></td>
                </tr>
                <tr runat="server" id="trProperty" visible="false">
                    <td>
                        <asp:CheckBox ID="chkProperty" runat="server" Text="Property approved" AutoPostBack="True" OnCheckedChanged="chkProperty_CheckedChanged" /></td>
                    <td>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnPrintDE" runat="server" Text="Print DE Approval" CausesValidation="False" /></td>
                </tr>
            </table>
        </asp:Panel>
    </radspl:RadPane>
</radspl:RadSplitter>

<asp:Panel ID="panel_dialog" runat="server" Visible="false" CssClass="pnlDialog">
    <div class="paneGrid" style="width: 310px; height: 25px;">
        <b>Details</b>
        <asp:Button CssClass="rght" ID="btnHideDialog" runat="server" Text="X" OnClick="btnHideDialog_Click" />
    </div>
    <div style="width: 310px;">
        <table border="0" cellspacing="0" cellpadding="0" style="width: 100%" class="TasksHeader">
            <tr>
                <td class="TasksHeader" width="100%"><a href="#" onclick="HideShowDiv('TableConditions');" class="TasksHeader">
                    <div style="width: 100%; cursor: hand;">Condition/Task details</div>
                </a></td>
            </tr>
            <tr>
                <td>
                    <table id="TableConditions" style="width: 100%;">
                        <tr>
                            <td align="left" style="width: 70px; padding-left: 5px">Title</td>
                            <td style="width: 95%">
                                <asp:TextBox ID="tbCondTitle" runat="server" Width="215px" CssClass="task"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" ControlToValidate="tbCondTitle" ValidationGroup="condition_val"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr valign="top">
                            <td align="left" style="width: 70px; padding-left: 5px">Description</td>
                            <td style="width: 95%">
                                <asp:TextBox ID="tbCondDesc" runat="server" TextMode="MultiLine" Rows="5" Width="215px" CssClass="task"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 70px; padding-left: 5px">Authority</td>
                            <td style="width: 95%" valign="top">
                                <asp:DropDownList ID="ddlAuthLevel" runat="server" DataTextField="Name" DataValueField="ID" Width="218px" CssClass="task"></asp:DropDownList>
                                <asp:Label ID="lblAuthLevel" runat="server" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="5" style="width: 100%">
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnSatisfy" runat="server" Text="Satisfy" OnClick="btnSatisfy_Click" /><%--&nbsp;&nbsp;<asp:Button ID="btnComplete" runat="server" Text="Complete" OnClick="btnComplete_Click" />--%></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table border="0" cellspacing="0" cellpadding="0" width="100%" style="width: 100%" class="TasksHeader">
            <tr>
                <td class="TasksHeader"><a href="#" onclick="HideShowDiv('TableFollowUp');" class="TasksHeader">
                    <div style="width: 100%; cursor: hand;">Follow up details</div>
                </a></td>
            </tr>
            <tr>
                <td>
                    <table id="TableFollowUp" style="width: 100%">
                        <tr>
                            <td align="left" style="width: 110px; padding-left: 5px">Frequency</td>
                            <td>
                                <asp:DropDownList ID="ddlRecurrence" runat="server" DataTextField="Name" DataValueField="ID" Width="150px" CssClass="task"></asp:DropDownList><asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="*" ValidationGroup="follow_val" ControlToValidate="ddlRecurrence" Type="Integer" MinimumValue="1" MaximumValue="100"></asp:RangeValidator></td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 110px; padding-left: 5px">Starting when</td>
                            <td>
                                <radCln:RadDatePicker ID="rdpStartDate" runat="server" Width="150px" SkinID="Windows">
                                    <dateinput width="150px" height="18px" style="color: #000000; font-size: 12px; font-family: Arial, Helvetica, sans-serif; padding-left: 4px; border: 1px solid #7F9DB9; background: #FFFFFF;"></dateinput>
                                    <calendar skin="WebBlue"></calendar>
                                </radCln:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ValidationGroup="follow_val" ControlToValidate="rdpStartDate"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td align="left" style="width: 110px; padding-left: 5px">Next follow up date:</td>
                            <td>
                                <asp:Label runat="server" ID="lblNextWorkDate" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="right">
                                <asp:Button ID="btnSubmitFollow" runat="server" Text="Save" ValidationGroup="follow_val" OnClick="btnSave_Click" />
                                <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="RefreshPage_Click" Visible="false" />
                                <asp:HiddenField ID="conditionActiveID" ClientIDMode="Static" Value="0" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table border="0" cellspacing="0" cellpadding="0" width="100%" style="width: 100%" class="TasksHeader" runat="server" id="tblNotes">
            <tr>
                <td class="TasksHeader"><a href="#" onclick="HideShowDiv('TableNote');return false;" class="TasksHeader">
                    <div style="width: 100%; cursor: hand;">Condition/Task Note</div>
                </a></td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%" id="TableNote">
                        <tr>
                            <td colspan="2">
                                <asp:TextBox ID="tbNote" runat="server" TextMode="MultiLine" Rows="5" Width="96%" CssClass="task"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="tbNote" ValidationGroup="val_note" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td align="right">Select one:</td>
                            <td>
                                <asp:DropDownList ID="ddlAction" runat="server" Width="228px" CssClass="task">
                                    <asp:ListItem Text="--Select one--" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Generic condition/task note" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Not complete. Progress follow-up date" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Document collected/task complete" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Retract Collected Document" Value="4"></asp:ListItem>
                                </asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="ddlAction" ValidationGroup="val_note" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="5" style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnShowNotes" runat="server" Text="Show notes" OnClick="btnShowNotes_Click" /></td>
                                        <td align="right">
                                            <asp:Button ID="btnSubmitNote" runat="server" Text="Add Note" ValidationGroup="val_note" OnClick="btnSubmitNote_Click" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>

<style>
    .pnlDialog {
        z-index: 200;
        position: absolute;
        top: 70px;
        left: 500px;
    }

    .rght {
        float: right;
    }
</style>
