<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoicesOld.ascx.cs" Inherits="LoanStarPortal.Controls.Invoices" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
Invoices:
            <table cellspacing="0" cellpadding="0" align="left" border="0" width="100%">
                <tr>
                    <td>
                        <table border="0" cellspacing="3" cellpadding="3" width="100%">
                            <tr>
                                <td>
                                    <radG:RadGrid ID="gridInvoices" runat="server" AllowPaging="True" EnableAJAX="false" AllowSorting="False" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" OnNeedDataSource="gridInvoices_NeedDataSource" OnItemCommand="gridInvoices_ItemCommand">
                                    <ClientSettings>
                                        <Resizing AllowColumnResize="True" EnableRealTimeResize="True"/>
                                    </ClientSettings>
                                    <MasterTableView AllowNaturalSort="False" EditMode="InPlace" CommandItemDisplay="Bottom" DataKeyNames="ID">
                                    <ExpandCollapseColumn Visible="False">
                                        <HeaderStyle Width="19px"></HeaderStyle>
                                    </ExpandCollapseColumn>
                                    <RowIndicatorColumn Visible="False">
                                        <HeaderStyle Width="20px"></HeaderStyle>
                                    </RowIndicatorColumn>                    
                                    <Columns>
                                        <radG:GridBoundColumn DataField="ID" Display="False" ReadOnly="True" UniqueName="ID"></radg:GridBoundColumn>
                                        <radG:GridDropDownColumn DataField="TypeID" DataSourceID="InvoiceTypeSource" HeaderText="Type" ListTextField="Name" ListValueField="ID" UniqueName="TypeID"></radG:GridDropDownColumn>
                                        <radG:GridDropDownColumn DataField="ProviderID" DataSourceID="InvoiceProviderSource" HeaderText="Provider" ListTextField="Name" ListValueField="ID" UniqueName="ProviderID"></radG:GridDropDownColumn>
                                        <radG:GridCheckBoxColumn DataField="IN" HeaderText="IN" SortExpression="IN" UniqueName="IN"></radG:GridCheckBoxColumn>
                                        <radG:GridTemplateColumn HeaderText="Invoice" SortExpression="Invoice" UniqueName="Invoice">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblInvoice" Text='<%# Eval("Invoice", "{0:C}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" ID="tbInvoice" Text='<%# Bind("Invoice") %>' Width="40px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tbInvoice" ErrorMessage="*" runat="server" ></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        </radG:GridTemplateColumn>
                                        <radG:GridBoundColumn DataField="PMT" HeaderText="PMT" UniqueName="PMT"></radG:GridBoundColumn>                        
                                        <radg:GridEditCommandColumn ButtonType="ImageButton" UpdateImageUrl="~/Images/Update.gif" EditImageUrl="~/Images/Edit.gif" InsertImageUrl="~/Images/Insert.gif" CancelImageUrl="~/Images/Cancel.gif" UniqueName="EditCommandColumn">
                                            <HeaderStyle Width="20px" /> 
                                        </radg:GridEditCommandColumn>
                                    </Columns>
                                    <EditFormSettings ColumnNumber="3" CaptionDataField="Invoice" CaptionFormatString="Edit Invoice">
                                    <FormTableItemStyle Wrap="False"></FormTableItemStyle>
                                    <FormCaptionStyle CssClass="EditFormHeader"></FormCaptionStyle>
                                    <FormMainTableStyle GridLines="None" CellSpacing="0" CellPadding="3" BackColor="White" Width="100%" />
                                    <FormTableStyle CellSpacing="0" CellPadding="2" CssClass="module" Height="110px" BackColor="White" />
                                    <FormTableAlternatingItemStyle Wrap="False"></FormTableAlternatingItemStyle>
                                    <EditColumn ButtonType="ImageButton" UpdateImageUrl="~/Images/Update.gif" EditImageUrl="~/Images/Edit.gif" InsertImageUrl="~/Images/Insert.gif" CancelImageUrl="~/Images/Cancel.gif" InsertText="Insert" UpdateText="Update" UniqueName="EditCommandColumn1" CancelText="Cancel edit"></EditColumn>
                                    <FormTableButtonRowStyle HorizontalAlign="Right" CssClass="EditFormButtonRow"></FormTableButtonRowStyle>
                                    </EditFormSettings>                    
                                    </MasterTableView>
                                    <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
                                </radG:RadGrid>  
                                    <br />
                                    <asp:Label id="lblMessageInvoice" runat="server"></asp:Label>        
                                    <asp:SqlDataSource ID="InvoiceTypeSource" runat="server"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="InvoiceProviderSource" runat="server"></asp:SqlDataSource>
                                </td>
                            </tr>
                        </table>                    
                    </td>
                </tr>
            </table>