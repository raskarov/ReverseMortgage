<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tasks.ascx.cs" Inherits="LoanStarPortal.Controls.Tasks" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>
<%@ Register Src="EmailAdd.ascx" TagName="EmailAdd" TagPrefix="uc1" %>


<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Width="100%" BorderWidth="0" BorderStyle="None" BorderSize="0" Skin="Default" LiveResize="false">
   <radspl:radpane id="TopPane" runat="server" Height="29px" Scrolling="None">
   <div RadResizeStopLookup="true" RadShowStopLookup="true" >
    <div class="paneTitle">
    <table border="0" cellspacing="0" cellpadding="0" width="100%">
        <tr>
            <td class="title" nowrap>
                <asp:Label ID="lblTitle" runat="server" Text="Condition manager"></asp:Label></td>
        </tr>
    </table>
    </div>
    </div>
   </radspl:radpane>  
   <radspl:radsplitbar id="RadSplitBar3" runat="server" CollapseMode="None" EnableResize="false" Visible="false"/>
   <radspl:radpane id="MiddlePane" runat="server" scrolling="Y" >
    <div RadResizeStopLookup="true" RadShowStopLookup="true" >
<asp:Panel ID="PanelTasks" runat="server">
    
    <table cellpadding="0" cellspacing="0">
    <tr>
        <td valign="top" width="75%" bordercolorlight="#000000">
            <radG:RadGrid ID="gridConditionTasks" Skin="Default" runat="server" AutoGenerateColumns="False" EnableAJAX="False" GridLines="Vertical" OnNeedDataSource="gridConditionTasks_NeedDataSource" AllowPaging="True" PageSize="30" OnItemCommand="gridConditionTasks_ItemCommand" Width="99%" OnPreRender="gridConditionTasks_PreRender" OnItemDataBound="gridConditionTasks_ItemDataBound">
                <MasterTableView DataKeyNames="SID">
                    <NoRecordsTemplate>No items</NoRecordsTemplate>
                    <ExpandCollapseColumn Visible="False">
                        <HeaderStyle Width="19px" />
                    </ExpandCollapseColumn>
                    <RowIndicatorColumn Visible="False">
                        <HeaderStyle Width="20px" />
                    </RowIndicatorColumn>
                    <Columns>
                        <radG:GridTemplateColumn HeaderText="Type" UniqueName="Status" HeaderStyle-Width="40px">
                            <ItemTemplate>
                                <img src='Images/status_<%# DataBinder.Eval(Container.DataItem, "Status") %>.gif' alt='<%# DataBinder.Eval(Container.DataItem, "Status") %>'/>
                            </ItemTemplate>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Doc" UniqueName="Doc" HeaderStyle-Width="40px">
                            <ItemTemplate>
                                <asp:Image ID="imgDoc" runat="server" />
                            </ItemTemplate>
                        </radG:GridTemplateColumn>
                        <radg:GridBoundColumn HeaderText="SID" DataField="SID" UniqueName="SID" Display= "False"></radg:GridBoundColumn>
                        <radG:GridButtonColumn UniqueName="TitleColumn" HeaderText="Title" CommandName="LoadItem" DataTextField="Title" ItemStyle-Width="90%"></radG:GridButtonColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <div style="vertical-align:top;">
                                <asp:ImageButton ID="ibtnEmail" runat="server" ImageUrl="~/Images/Add_Email.gif" BorderWidth="0" ImageAlign="Bottom" CommandName="CreateEmail"/>&nbsp;&nbsp;/&nbsp;&nbsp;<asp:ImageButton ID="ibtnNote" runat="server" ImageUrl="~/Images/Add-Note.gif" BorderWidth="0" ImageAlign="Bottom" CommandName="CreateNote"/></div>
                            </ItemTemplate>
                        </radG:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <PagerStyle Mode="NumericPages" />
                <ClientSettings EnablePostBackOnRowClick="false">
                </ClientSettings>
            </radG:RadGrid>
            <br /><br /> 
            &nbsp;<asp:LinkButton ID="lbAddCondition" runat="server" Text="Add condition" OnClick="lbAddCondition_Click" CssClass="AddCondition"></asp:LinkButton><%--&nbsp;&nbsp;
            <asp:LinkButton ID="lbAddTask" runat="server" Text="Add task" OnClick="lbAddTask_Click" CssClass="AddTask"></asp:LinkButton>--%>
         </td>
         <td align="left" valign="top" width="310px">
            <div class="paneGrid" style="width:310px;height:25px;"><b>Details</b></div>
            <div style="width:310px;">
                <asp:Panel ID="panelCondition" runat="server">
                <table border="0" cellspacing="0" cellpadding="0" width="100%" class="TasksHeader">
                    <tr>
                        <td class="TasksHeader" width="100%"><a href="#" onclick="HideShowDiv('TableConditions');" class="TasksHeader"><div style="width:100%;cursor:hand;">Condition details</div></a></td>
                    </tr>
                    <tr>
                        <td>
                            <table id="TableConditions">
                            <tr>
                                <td align="right" width="70px" style="width:70px;">Title</td>
                                <td><asp:TextBox ID="tbCondTitle" runat="server" Width="217px" CssClass="task"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right" width="70px" style="width:70px;">&nbsp;&nbsp;&nbsp;&nbsp;Description</td>
                                <td><asp:TextBox ID="tbCondDesc" runat="server" TextMode="MultiLine" Rows="5" Width="217px" CssClass="task"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right" width="70px" style="width:70px;">Type</td>
                                <td>
                                    <asp:DropDownList ID="ddlCondType" runat="server" DataTextField="Name" DataValueField="ID" Width="222px" CssClass="task"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="70px" style="width:70px;">Category</td>
                                <td>
                                    <asp:DropDownList ID="ddlCondCategory" runat="server" DataTextField="Name" DataValueField="ID" Width="222px" CssClass="task"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <div id="divNoUComments" runat="server">
                                        No Underwriter comments
                                    </div>
                                    <div id="divAddUComments" runat="server">
                                        <a href="#" onclick="HideShowDiv('<%=divUComments.ClientID %>');">Add Undewriter Comments</a>
                                    </div>
                                    <div id="divUComments" runat="server">
                                        <div align="left" style="width:100%;"><b>Underwritter Comments:</b></div>
                                        <div>
                                        <asp:TextBox ID="tbCondNotes" runat="server" TextMode="MultiLine" Rows="5" Width="90%" CssClass="task"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
                <asp:Panel ID="panelTask" runat="server">
                <table border="0" cellspacing="0" cellpadding="0" width="100%" class="TasksHeader">
                    <tr>
                        <td class="TasksHeader"><a href="#" onclick="HideShowDiv('TableTask');" class="TasksHeader"><div style="width:100%;cursor:hand;">Follow up details</div></a></td>
                    </tr>
                    <tr>
                        <td>
                            <table id="TableTask">
                <tr id="trTaskTitle" runat="server">
                    <td align="right" width="70px" style="width:70px;">Title</td>
                    <td>
                        <asp:TextBox ID="tbTitle" runat="server" Width="217px" CssClass="task"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trTaskDesc" runat="server">
                    <td align="right" width="70px" style="width:70px;">Description</td>
                    <td>
                        <asp:TextBox ID="tbDescription" runat="server" TextMode="MultiLine" Rows="5" Width="217px" CssClass="task"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" width="70px" style="width:70px;">Schedule</td>
                    <td>
                        <asp:DropDownList ID="ddlTaskRecurrence" runat="server" DataTextField="Name" DataValueField="ID" Width="222px" CssClass="task"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right" width="70px" style="width:70px;">Starting&nbsp;when</td>
                    <td>
                        <radCln:RadDatePicker ID="rdpTaskSchedule" runat="server" Width="222px" SkinID="Windows">
                        <DateInput Width="222px" Height="18px" style="color:#000000;font-size:12px;font-family:Arial, Helvetica, sans-serif;padding-left:4px;border:1px solid #7F9DB9;background:#FFFFFF;"></DateInput>
                        <Calendar Skin="WebBlue"></Calendar>
                        </radCln:RadDatePicker>
                    </td>
                </tr>            
                <tr>
                    <td align="right" width="70px" style="width:70px;">Assigned to</td>
                    <td><asp:DropDownList ID="ddlAssignedTo" runat="server" DataTextField="FullName" DataValueField="ID" Width="222px" CssClass="task"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                            <tr>
                                <td><b>New task notes</b></td>
                                <td align="right"><asp:Button ID="lbNotes" runat="server" Text="Show task notes" OnClick="lbNotes_Click" /></td>
                            </tr>
                        </table>
                        <table border="0" width="99%"> 
                        <tr>
                            <td colspan="2">
                                <asp:TextBox ID="tbQuickNote" runat="server" TextMode="MultiLine" Rows="5" Width="100%" CssClass="task"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Add note and:</td>
                            <td align="right">
                            <asp:DropDownList ID="ddlAction" runat="server" Width="200px" CssClass="task">
                            <asp:ListItem Text="--Select one--" Value=""></asp:ListItem>
                            <asp:ListItem Text="Don't change the status" Value="1"></asp:ListItem>
                            <asp:ListItem Text="I fully worked this issue" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Mark this issue as complete" Value="3"></asp:ListItem>
                 </asp:DropDownList><asp:Label ID="lblActionError" runat="server" Text=" *" CssClass="errmessage"></asp:Label></td>
                         </tr>                        
                        </table>
                    </td>
                </tr>
                </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td><asp:Button ID="btnCompleted" runat="server" Text="Completed" OnClick="btnCompleted_Click" /><asp:Button ID="btnSatisfied" runat="server" Text="Satisfied" OnClick="btnSatisfied_Click" />&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />&nbsp;</td>
                                    <td align="right"><asp:Button ID="btnCreate" runat="server" Text="Submit" OnClick="btnCreate_Click" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                 </table>
                </asp:Panel>
            </div>
        </td>
    </tr>
    </table>
</asp:Panel>

<asp:Panel ID="PanelEmailAdd" runat="server" Visible="False" Width="100%">
    <uc1:EmailAdd ID="EmailAdd1" runat="server" OnMailSent="EmailAdd1_MailSent" />
</asp:Panel>
    </div>
    </radspl:radpane>
</radspl:RadSplitter>
