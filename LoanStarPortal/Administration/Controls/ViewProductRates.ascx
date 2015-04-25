<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewProductRates.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewProductRates" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td align="left"><asp:Label ID="lblHeader" runat="server" Text="" Font-Size="Larger" Font-Bold="true"></asp:Label></td>
    </tr>
    <tr>
        <td runat="server">
            <div id="gAUtomatic" runat="server">
            <radG:RadGrid ID="G" runat="server" AllowPaging="False" AllowSorting="True" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="98%" 
                OnNeedDataSource="G_NeedDataSource" 
                OnItemCommand="G_ItemCommand"
            >
            <ClientSettings>
            <Resizing AllowColumnResize="false" EnableRealTimeResize="false"/>
            </ClientSettings>
            <MasterTableView AllowNaturalSort="False" EditMode="InPlace" DataKeyNames="ID" >
            <Columns>
                <radg:GridTemplateColumn UniqueName="Date" ItemStyle-BorderStyle="None" ItemStyle-BorderWidth="0px" HeaderText="Date" ItemStyle-Width="50px">
                    <ItemTemplate>
                        <asp:Label ID="lblCompanyName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Period","{0:d}") %>'></asp:Label>
                    </ItemTemplate>
                </radg:GridTemplateColumn>
                <radg:GridTemplateColumn UniqueName="WeekDay" ItemStyle-BorderStyle="None" ItemStyle-BorderWidth="0px" HeaderText="Week day" ItemStyle-Width="50px">
                    <ItemTemplate>
                        <asp:Label ID="lblWeekDay" runat="server" Text='<%# GetDayOfWeek(Container.DataItem)%>'></asp:Label>
                    </ItemTemplate>
                </radg:GridTemplateColumn>
                <radg:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-BorderStyle="None" ItemStyle-BorderWidth="0px">
                   <HeaderTemplate>
                    <table id="Table1" cellspacing="0" cellpadding="0" width="400px" border="0">
                        <tr>
                            <td colspan="5" align="center"><b>Initial</b></td>
                        </tr>
                        <tr>
                            <td style="width:50px" align="center"><b>Daily index</b></td>
                            <td style="width:60px" align="center"><b>Weekly index</b></td>
                            <td style="width:50px" align="center"><b>Margin</b></td>
                            <td style="width:60px" align="center"><b>Weekly Rate</b></td>
                            <td style="width:60px" align="center"><b>Published Index</b></td>
                            <td style="width:60px" align="center"><b>Published Margin</b></td>
                            <td style="width:60px" align="center"><b>Published Rate</b></td>
                        </tr>
                    </table>
                   </HeaderTemplate>
                   <ItemTemplate>
                    <table id="Table2" cellspacing="0" cellpadding="0" width="400px" border="0">
                        <tr>
                            <td style="width:50px;"><%# DataBinder.Eval(Container.DataItem, "DailyInitialIndex", "{0:f3}")%></td>
                            <td style="width:60px"><%# GetFloatValue(Container.DataItem,"weeklyaveinitialindex",true,"{0:f3}")%></td>
                            <td style="width:50px"><%# GetFloatValue(Container.DataItem,"margin",true,"{0:f3}") %></td>
                            <td style="width:60px"><%# GetFloatValue(Container.DataItem,"WeeklyAveInitialRate",true,"{0:f3}") %></td>
                            <td style="width:60px;"><%# GetFloatValue(Container.DataItem, "PublishedInitialIndex", false, "{0:f3}")%></td>
                            <td style="width:60px"><%# GetFloatValue(Container.DataItem, "PublishedInitialMargin", false, "{0:f3}")%></td>
                            <td style="width:60px"><%# DataBinder.Eval(Container.DataItem, "PublishedInitialRate", "{0:f3}")%></td>
                        </tr>
                    </table>
                   </ItemTemplate>
                   <EditItemTemplate>
                    <table id="Table2" cellspacing="0" cellpadding="0" width="400px" border="0">
                        <tr>
                            <td style="width:50px;"><radI:RadNumericTextBox ID="tbInitialIndex" Type="Number" runat="server" Height="18px" MaxValue="100" MinValue="0" Skin="WebBlue" Width="50px" NumberFormat-DecimalDigits="3" Value='<%# DataBinder.Eval(Container.DataItem, "DailyInitialIndex") %>' ></radI:RadNumericTextBox></td>
                            <td style="width:60px"><%# GetFloatValue(Container.DataItem,"weeklyaveinitialindex",true,"{0:f3}")%></td>
                            <td style="width:50px"><%# GetFloatValue(Container.DataItem,"margin",true,"{0:f3}") %></td>
                            <td style="width:60px"><%# GetFloatValue(Container.DataItem,"WeeklyAveInitialRate",true,"{0:f3}") %></td>
                            <td style="width:60px"><radI:RadNumericTextBox ID="tbPublishedIndex" Type="Number" runat="server" Height="18px" MaxValue="100" MinValue="0" Skin="WebBlue" Width="50px" NumberFormat-DecimalDigits="3" Value='<%# DataBinder.Eval(Container.DataItem, "PublishedInitialIndex") %>' ></radI:RadNumericTextBox></td>
                            <td style="width:60px"><radI:RadNumericTextBox ID="tbPublishedMargin" Type="Number" runat="server" Height="18px" MaxValue="100" MinValue="0" Skin="WebBlue" Width="50px" NumberFormat-DecimalDigits="3" Value='<%# DataBinder.Eval(Container.DataItem, "PublishedInitialMargin") %>' ></radI:RadNumericTextBox></td>
                            <td style="width:60px"><%# DataBinder.Eval(Container.DataItem, "PublishedInitialRate", "{0:f3}")%></td>
                        </tr>
                    </table>                   
                   </EditItemTemplate>
                </radg:GridTemplateColumn>
                <radg:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-BorderStyle="None" ItemStyle-BorderWidth="0px" HeaderStyle-HorizontalAlign="Left">
                    <HeaderTemplate>
                    <table id="Table1" cellspacing="1" cellpadding="1" width="400" border="0">
                        <tr>
                            <td colspan="5" align="center"><b>Expected</b></td>
                        </tr>
                        <tr>
                            <td style="width:50px" align="center"><b>Daily index</b></td>
                            <td style="width:60px" align="center"><b>Weekly index</b></td>
                            <td style="width:50px" align="center"><b>Margin</b></td>
                            <td style="width:60px" align="center"><b>Weekly Rate</b></td>
                            <td style="width:60px" align="center"><b>Published Index</b></td>
                            <td style="width:60px" align="center"><b>Published Margin</b></td>
                            <td style="width:60px" align="center"><b>Published Rate</b></td>
                            
                        </tr>
                    </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                    <table id="Table2" cellspacing="1" cellpadding="1" width="400" border="0">
                        <tr>
                            <td style="width:50px;"><%# DataBinder.Eval(Container.DataItem, "DailyExpectedIndex", "{0:f3}")%></td>
                            <td style="width:60px"><%# GetFloatValue(Container.DataItem, "weeklyaveExpectedindex", true, "{0:f3}")%></td>
                            <td style="width:50px"><%# GetFloatValue(Container.DataItem, "margin", true, "{0:f3}")%></td>                            
                            <td style="width:60px"><%# GetFloatValue(Container.DataItem, "WeeklyAveExpectedRate", true, "{0:f3}")%></td>
                            <td style="width:60px;"><%# GetFloatValue(Container.DataItem, "PublishedExpectedIndex", false, "{0:f3}")%></td>
                            <td style="width:60px"><%# GetFloatValue(Container.DataItem, "PublishedExpectedMargin", false, "{0:f3}")%></td>                            
                            <td style="width:60px"><%# DataBinder.Eval(Container.DataItem, "PublishedExpectedRate", "{0:f3}")%></td>
                         </tr>
                    </table>
                    </ItemTemplate>
                    <EditItemTemplate>
                    <table id="Table2" cellspacing="1" cellpadding="1" width="400" border="0">
                        <tr>
                            <td style="width:50px;"><radI:RadNumericTextBox ID="tbExpectedIndex" Type="Number" runat="server" Height="18px" MaxValue="100" MinValue="0" Skin="WebBlue" Width="50px" NumberFormat-DecimalDigits="3" Value='<%# DataBinder.Eval(Container.DataItem, "DailyExpectedIndex") %>' ></radI:RadNumericTextBox></td>
                            <td style="width:60px"><%# GetFloatValue(Container.DataItem, "weeklyaveExpectedindex", true, "{0:f3}")%></td>
                            <td style="width:50px"><%# GetFloatValue(Container.DataItem, "margin", true, "{0:f3}")%></td>                            
                            <td style="width:60px"><%# GetFloatValue(Container.DataItem, "WeeklyAveExpectedRate", true, "{0:f3}")%></td>
                            <td style="width:60px"><radI:RadNumericTextBox ID="tbPublishedExpectedIndex" Type="Number" runat="server" Height="18px" MaxValue="100" MinValue="0" Skin="WebBlue" Width="50px" NumberFormat-DecimalDigits="3" Value='<%# DataBinder.Eval(Container.DataItem, "PublishedExpectedIndex") %>' ></radI:RadNumericTextBox></td>
                            <td style="width:60px"><radI:RadNumericTextBox ID="tbPublishedExpectedMargin" Type="Number" runat="server" Height="18px" MaxValue="100" MinValue="0" Skin="WebBlue" Width="50px" NumberFormat-DecimalDigits="3" Value='<%# DataBinder.Eval(Container.DataItem, "PublishedExpectedMargin") %>' ></radI:RadNumericTextBox></td>                            
                            <td style="width:60px"><%# DataBinder.Eval(Container.DataItem, "PublishedExpectedRate", "{0:f3}")%></td>
                         </tr>
                    </table>                    
                    </EditItemTemplate>
                  </radg:GridTemplateColumn>
                    <radg:GridEditCommandColumn ButtonType="ImageButton" UpdateImageUrl="~/Images/Update.gif" 
                            EditImageUrl="~/Images/Edit.gif" InsertImageUrl="~/Images/Insert.gif" 
                            CancelImageUrl="~/Images/Cancel.gif" UniqueName="EditCommandColumn" >
                        <HeaderStyle Width="20px" /> 
                    </radg:GridEditCommandColumn>                  
            </Columns>
            </MasterTableView>
            <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
            </radG:RadGrid>                    
            </div>
            <div id="gManual" runat="server">
            <radG:RadGrid ID="Gm" runat="server" AllowPaging="True" PageSize="25" AllowSorting="false" AutoGenerateColumns="false" BorderStyle="Solid" 
                BorderWidth="1px" Skin="WebBlue" Width="90%" 
                OnNeedDataSource="Gm_NeedDataSource" 
                OnItemCommand="Gm_ItemCommand"
                OnItemCreated="Gm_ItemCreated"
            >
            <ClientSettings>
            <Resizing AllowColumnResize="false" EnableRealTimeResize="false"/>
            </ClientSettings>
            <MasterTableView AllowNaturalSort="False" EditMode="InPlace" DataKeyNames="ID" CommandItemDisplay="Bottom" CommandItemSettings-AddNewRecordText="Add rate">
            <Columns>
                <radg:GridTemplateColumn UniqueName="Date" ItemStyle-BorderStyle="None" ItemStyle-BorderWidth="0px" HeaderText="Date" ItemStyle-Width="80px">
                    <ItemTemplate>
                        <asp:Label ID="lblCompanyName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Period","{0:d}") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <radI:RadDateInput ID="rdPeriod" runat="server"  Width="60px"></radI:RadDateInput>&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="rdPeriod" Width="8px"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </radg:GridTemplateColumn>
                <radg:GridTemplateColumn UniqueName="WeekDay" ItemStyle-BorderStyle="None" ItemStyle-BorderWidth="0px" HeaderText="Week day" ItemStyle-Width="50px">
                    <ItemTemplate>
                        <asp:Label ID="lblWeekDay" runat="server" Text='<%# GetDayOfWeek(Container.DataItem)%>'></asp:Label>
                    </ItemTemplate>
                </radg:GridTemplateColumn>
                <radg:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-BorderStyle="None" ItemStyle-BorderWidth="0px" HeaderStyle-Width="260px">
                   <HeaderTemplate>
                    <table id="Table1" cellspacing="0" cellpadding="0" width="250px" border="0">
                        <tr>
                            <td colspan="3" align="center"><b>Initial</b></td>
                        </tr>
                        <tr>
                            <td style="width:70px" align="center"><b>Published Index</b></td>
                            <td style="width:70px" align="center"><b>Published Margin</b></td>
                            <td style="width:70px" align="center"><b>Published Rate</b></td>
                        </tr>
                    </table>
                   </HeaderTemplate>
                   <ItemTemplate>
                    <table id="Table2" cellspacing="0" cellpadding="0" width="250px" border="0">
                        <tr>
                            <td style="width:70px;" align="center"><%# GetFloatValue(Container.DataItem, "PublishedInitialIndex", false, "{0:f3}")%></td>
                            <td style="width:70px" align="center"><%# GetFloatValue(Container.DataItem, "PublishedInitialMargin", false, "{0:f3}")%></td>
                            <td style="width:70px" align="center"><%# DataBinder.Eval(Container.DataItem, "PublishedInitialRate", "{0:f3}")%></td>
                        </tr>
                    </table>
                   </ItemTemplate>
                   <EditItemTemplate>
                    <table id="Table2" cellspacing="0" cellpadding="0" width="250px" border="0">
                        <tr>
                            <td style="width:70px"><radI:RadNumericTextBox ID="tbPublishedIndex" Type="Number" runat="server" Height="18px" MaxValue="100" MinValue="0" Skin="WebBlue" Width="60px" NumberFormat-DecimalDigits="3" ></radI:RadNumericTextBox></td>                            
                            <td style="width:70px"><radI:RadNumericTextBox ID="tbPublishedMargin" Type="Number" runat="server" Height="18px" MaxValue="100" MinValue="0" Skin="WebBlue" Width="60px" NumberFormat-DecimalDigits="3" ></radI:RadNumericTextBox></td>
                            <td style="width:70px"><%# DataBinder.Eval(Container.DataItem, "PublishedInitialRate", "{0:f3}")%></td>
                        </tr>
                    </table>                   
                   </EditItemTemplate>
                </radg:GridTemplateColumn>
                <radg:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-BorderStyle="None" ItemStyle-BorderWidth="0px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="260px">
                    <HeaderTemplate>
                    <table id="Table1" cellspacing="1" cellpadding="1" width="250" border="0">
                        <tr>
                            <td colspan="3" align="center"><b>Expected</b></td>
                        </tr>
                        <tr>
                            <td style="width:70px" align="center"><b>Published Index</b></td>
                            <td style="width:70px" align="center"><b>Published Margin</b></td>
                            <td style="width:70px" align="center"><b>Published Rate</b></td>                            
                        </tr>
                    </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                    <table id="Table2" cellspacing="1" cellpadding="1" width="250" border="0">
                        <tr>
                            <td style="width:70px;" align="center"><%# GetFloatValue(Container.DataItem, "PublishedExpectedIndex", false, "{0:f3}")%></td>
                            <td style="width:70px" align="center"><%# GetFloatValue(Container.DataItem, "PublishedExpectedMargin", false, "{0:f3}")%></td>                            
                            <td style="width:70px" align="center"><%# DataBinder.Eval(Container.DataItem, "PublishedExpectedRate", "{0:f3}")%></td>
                         </tr>
                    </table>
                    </ItemTemplate>
                    <EditItemTemplate>
                    <table id="Table2" cellspacing="1" cellpadding="1" width="250" border="0">
                        <tr>
                            <td style="width:70px" align="center"><radI:RadNumericTextBox ID="tbPublishedExpectedIndex" Type="Number" runat="server" Height="18px" MaxValue="100" MinValue="0" Skin="WebBlue" Width="60px" NumberFormat-DecimalDigits="3" ></radI:RadNumericTextBox></td>
                            <td style="width:70px" align="center"><radI:RadNumericTextBox ID="tbPublishedExpectedMargin" Type="Number" runat="server" Height="18px" MaxValue="100" MinValue="0" Skin="WebBlue" Width="60px" NumberFormat-DecimalDigits="3" ></radI:RadNumericTextBox></td>                            
                            <td style="width:70px" align="center"><%# DataBinder.Eval(Container.DataItem, "PublishedExpectedRate", "{0:f3}")%></td>
                         </tr>
                    </table>                    
                    </EditItemTemplate>
                  </radg:GridTemplateColumn>
                    <radg:GridEditCommandColumn ButtonType="ImageButton" UpdateImageUrl="~/Images/Update.gif" 
                            EditImageUrl="~/Images/Edit.gif" InsertImageUrl="~/Images/Insert.gif" 
                            CancelImageUrl="~/Images/Cancel.gif" UniqueName="EditCommandColumn" >
                        <HeaderStyle Width="25px" HorizontalAlign="Right"/> 
                    </radg:GridEditCommandColumn>
                    <radG:GridButtonColumn ConfirmText="Delete this record?" ButtonType="ImageButton" ImageUrl="/Images/Delete.gif" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle Width="15px" />
                            <ItemStyle HorizontalAlign="Left"/>
                    </radG:GridButtonColumn>                    
            </Columns>
            </MasterTableView>
            <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
            </radG:RadGrid>
            </div>
        </td>
    </tr>
    <tr id="trCopyRate" runat="server" style="padding-top:5px;">
        <td>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="center"><asp:Label ID="lblMessage" runat="server" Text="" SkinID="AdminMessage"></asp:Label></td>
                </tr>            
                <tr>                    
                    <td align="left">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="width:230px;text-align:left;">Please select product(s) to copy rates</td>
                                    <td style="width:100px;text-align:right;padding-right:3px;">Date range from:</td>
                                    <td style="width:75px"><radI:RadDateInput ID="rdFrom" runat="server" Width="70px"></radI:RadDateInput></td>
                                    <td style="width:25px;text-align:right;padding-right:5px;">to:</td>
                                    <td style="width:75px"><radI:RadDateInput ID="rdTo" runat="server" Width="70px"></radI:RadDateInput></td>
                                    <td style="text-align:left;padding-left:10px;"><asp:Button ID="btnCopyRate" Text ="Copy Rates" runat="server" OnClick="btnCopy_Click" CausesValidation="False"/></td>
                                </tr>
                            </table>
                    </td>
                </tr>
                <tr style="padding-top:5px;">
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0" style="width:90%" >
                            <tr>
                                <td style="border:solid 1px black;width:100%;">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width:100%" >
                                    <tr>
                                        <td>
                                            <asp:DataList ID="dlProducts" runat="server" RepeatColumns="4" RepeatDirection="Vertical" EnableViewState="true" OnItemDataBound="dlProducts_ItemDataBound">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbProduct" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name")%>' Width="200px" ></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                </table>                
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>                
            </table>   
        </td>
    </tr>
    <tr style="padding-top:5px;">
        <td><asp:Button ID="btnBack" runat="server" Text="Back" SkinID="AdminButton" OnClick="btnBack_Click" CausesValidation="False"/></td>
    </tr>
</table>