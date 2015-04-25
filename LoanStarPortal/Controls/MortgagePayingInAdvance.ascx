<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MortgagePayingInAdvance.ascx.cs" Inherits="LoanStarPortal.Controls.MortgagePayingInAdvance" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>
<br /><br /><br /><br />
<radG:RadGrid ID="gPayingInAdvance" runat="server"  AllowPaging="True" EnableAJAX="false" AllowSorting="False" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" OnItemCommand="Grid_ItemCommand" OnItemDataBound="Grid_ItemDataBound" >
<ClientSettings>
    <Resizing AllowColumnResize="True" EnableRealTimeResize="True"/>
</ClientSettings>
<MasterTableView AllowNaturalSort="False" CommandItemDisplay="Bottom" DataKeyNames="ID" EditMode="InPlace">
<ExpandCollapseColumn Visible="False">
    <HeaderStyle Width="19px"></HeaderStyle>
</ExpandCollapseColumn>
<RowIndicatorColumn Visible="False">
    <HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>                    
    <Columns>
        <radg:GridBoundColumn HeaderText="ID" DataField="ID" ReadOnly="True" UniqueName="ID" Display= "False"></radg:GridBoundColumn>
        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Description" SortExpression="Description">
            <ItemTemplate>
                <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Description") %>' ></asp:Label>
                <radC:RadComboBox ID="ddlDescription" runat="server" AllowCustomText="True" Height="100px" Width="120px" MarkFirstMatch="True" ShowDropDownOnTextboxClick="False" UseEmbeddedScripts="False" Visible="false"></radC:RadComboBox>
            </ItemTemplate>                                
            <HeaderStyle HorizontalAlign="Left" Width="130px"/>
        </radG:GridTemplateColumn>
        <radG:GridTemplateColumn HeaderText="To" SortExpression="PayingTo" UniqueName="PayingTo">
            <ItemTemplate>
                <asp:Label ID="lblPayingTo" runat="server" Text='<%# Eval("PayingTo") %>' ></asp:Label>
                <asp:TextBox runat="server" ID="tbPayingTo" Text="" Width="120px" Visible="false"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPayingTo" ControlToValidate="tbPayingTo" ErrorMessage="*" runat="server" Visible="false"></asp:RequiredFieldValidator>
            </ItemTemplate>
            <HeaderStyle Width="160px" />
        </radG:GridTemplateColumn>                        
        <radG:GridTemplateColumn HeaderText="Amount" SortExpression="Amount" UniqueName="Amount">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblAmount" Text='<%# GetAmount(Container.DataItem) %>'></asp:Label>
                <radI:RadNumericTextBox ShowSpinButtons="false" Type="Currency" ID="tbAmount" runat="server" Visible="false" Width="80px"></radI:RadNumericTextBox>
            </ItemTemplate>
            <HeaderStyle Width="100px" />
        </radG:GridTemplateColumn>
        <radG:GridTemplateColumn HeaderText="Unit" SortExpression="Unit" UniqueName="Unit">
            <ItemTemplate>
                <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>' ></asp:Label>
                <asp:DropDownList ID="ddlUnit" runat="server" Width="100px" AutoPostBack="false" Visible="false"></asp:DropDownList>
            </ItemTemplate>
            <HeaderStyle Width="100px" />
        </radG:GridTemplateColumn>
        <radG:GridTemplateColumn UniqueName="TemplateColumn1">
                <ItemTemplate>
                    <asp:ImageButton ID='imgEdit' CommandName="CustomEdit"  ImageUrl="~/Images/Edit.gif" runat="server" Visible='<%# CanEdit %>'/>
                    <asp:ImageButton ID="imgUpdate" CommandName="CustomUpdate"  ImageUrl="~/Images/Update.gif" runat="server" Visible="false"/>
                    <asp:ImageButton ID="imgCancel" CommandName="CustomCancel"  ImageUrl="~/Images/Cancel.gif" runat="server" Visible="false" CausesValidation="false"/>
                </ItemTemplate>
            <HeaderStyle Width="20px" />
        </radG:GridTemplateColumn>                        
    </Columns>
    <EditFormSettings>
        <EditColumn UniqueName="EditCommandColumn1">
        </EditColumn>
    </EditFormSettings> 
    <CommandItemTemplate>
        <table border="0" cellspacing="0" cellpadding="0" width="100%" runat="server" id="cmdtable">
            <tr>
                <td style="width:18px">
                    <asp:ImageButton ID="imgInsert" runat="server" AlternateText="" BorderWidth="0" CommandName="CustomInsert" ImageUrl="~/RadControls/Grid/Skins/WebBlue/AddRecord.gif" Visible='<%# CanAddNew %>' />
                </td>
                <td>
                    <asp:LinkButton ID="lbAddPayment" runat="server" CommandName="CustomInsert" Visible='<%# CanAddNew %>'>Add new payment</asp:LinkButton>
                </td>
                <td align="right" style="padding-right:5px">
                    <asp:ImageButton ID="imgRefresh" runat="server" AlternateText="" BorderWidth="0" CommandName="RebindGrid" ImageUrl="~/RadControls/Grid/Skins/WebBlue/Refresh.gif" />
                    <asp:LinkButton ID="lbRefresh" runat="server" CommandName="RebindGrid">Refresh</asp:LinkButton>
                </td>                                
            </tr>
        </table>
    </CommandItemTemplate>                                       
</MasterTableView>                
<PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
</radG:RadGrid>    
