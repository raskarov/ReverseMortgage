<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Package.ascx.cs" Inherits="LoanStarPortal.Controls.Package" %>
<%@ Register Assembly="RadPanelbar.Net2" Namespace="Telerik.WebControls" TagPrefix="radP" %>

<radP:RadPanelbar runat="server" ID="RadPBDocTemplates" Skin="Outlook" Width="100%" CausesValidation="False" ForeColor="GradientActiveCaption">
    <Items>
        <radp:RadPanelItem Text="&lt;b&gt;Standard&lt;/b&gt;" Value="1" ExpandedCssClass="Outlook" Expanded="True" runat="server">
            <Items>
                <radp:RadPanelItem runat="server">
                    <ItemTemplate>
                    
                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                            <tr>
                                <td align="center" style="width:19px;"></td>
                                <td align="left">
                                    <asp:Label ID="lblNoDTInStandard" runat="server" Text="No Documents"></asp:Label>
                                </td>
                            </tr>
                            <asp:Repeater ID="rpStandard" runat="server" OnItemDataBound="rp_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td align="center" style="width:50px;">
                                            <asp:CheckBox ID="cbPrint" Checked="true" runat="server" Text="" />
                                        </td>
                                        <td align="left">
                                            <asp:HiddenField ID="hfDTID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"DTID") %>' />
                                            <asp:LinkButton ID="lbtnDTTitle" runat="server" CssClass="linkButton" CommandName="DocClicked" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"DTID") %>' OnCommand="lbtnDTTitle_Command" Text='<%# DataBinder.Eval(Container.DataItem,"DTTitle") %>'></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        
                    </ItemTemplate>
                </radp:RadPanelItem>
            </Items>
        </radp:RadPanelItem>
        
        <radp:RadPanelItem Text="&lt;b&gt;State Specific&lt;/b&gt;" Value="2" ExpandedCssClass="Outlook" Expanded="True" runat="server">
            <Items>
                <radp:RadPanelItem runat="server">
                    <ItemTemplate>
                    
                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                            <tr>
                                <td align="center" style="width:19px;"></td>
                                <td align="left">
                                    <asp:Label ID="lblNoDTInStateSpecific" runat="server" Text="No Documents"></asp:Label>
                                </td>
                            </tr>
                            <asp:Repeater ID="rpStateSpecific" runat="server" OnItemDataBound="rp_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td align="center" style="width:50px;">
                                            <asp:CheckBox ID="cbPrint" Checked="true" runat="server" Text="" />
                                        </td>
                                        <td align="left">
                                            <asp:HiddenField ID="hfDTID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"DTID") %>' />
                                            <asp:LinkButton ID="lbtnDTTitle" runat="server" CssClass="linkButton" CommandName="DocClicked" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"DTID") %>' OnCommand="lbtnDTTitle_Command" Text='<%# DataBinder.Eval(Container.DataItem,"DTTitle") %>'></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        
                    </ItemTemplate>
                </radp:RadPanelItem>
            </Items>
        </radp:RadPanelItem>
        
        <radp:RadPanelItem Text="&lt;b&gt;Miscellaneuos&lt;/b&gt;" Value="3" ExpandedCssClass="Outlook" Expanded="True" runat="server">
            <Items>
                <radp:RadPanelItem runat="server">
                    <ItemTemplate>
                    
                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                            <tr>
                                <td align="center" style="width:19px;"></td>
                                <td align="left">
                                    <asp:Label ID="lblNoDTInMiscellaneuos" runat="server" Text="No Documents"></asp:Label>
                                </td>
                            </tr>
                            <asp:Repeater ID="rpMiscellaneuos" runat="server" OnItemDataBound="rp_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td align="center" style="width:50px;">
                                            <asp:CheckBox ID="cbPrint" Checked="true" runat="server" Text="" />
                                        </td>
                                        <td align="left">
                                            <asp:HiddenField ID="hfDTID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"DTID") %>' />
                                            <asp:LinkButton ID="lbtnDTTitle" runat="server" CssClass="linkButton" CommandName="DocClicked" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"DTID") %>' OnCommand="lbtnDTTitle_Command" Text='<%# DataBinder.Eval(Container.DataItem,"DTTitle") %>'></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        
                    </ItemTemplate>
                </radp:RadPanelItem>
            </Items>
        </radp:RadPanelItem>
        
        <radp:RadPanelItem Text="&lt;b&gt;Lender Specific&lt;/b&gt;" Value="4" ExpandedCssClass="Outlook" Expanded="True" runat="server">
            <Items>
                <radp:RadPanelItem runat="server">
                    <ItemTemplate>
                    
                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                            <tr>
                                <td align="center" style="width:19px;"></td>
                                <td align="left">
                                    <asp:Label ID="lblNoDTInLenderSpecific" runat="server" Text="No Documents"></asp:Label>
                                </td>
                            </tr>
                            <asp:Repeater ID="rpLenderSpecific" runat="server" OnItemDataBound="rp_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td align="center" style="width:50px;">
                                            <asp:CheckBox ID="cbPrint" Checked="true" runat="server" Text="" />
                                        </td>
                                        <td align="left">
                                            <asp:HiddenField ID="hfDTID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"DTID") %>' />
                                            <asp:LinkButton ID="lbtnDTTitle" runat="server" CssClass="linkButton" CommandName="DocClicked" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"DTID") %>' OnCommand="lbtnDTTitle_Command" Text='<%# DataBinder.Eval(Container.DataItem,"DTTitle") %>'></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        
                    </ItemTemplate>
                </radp:RadPanelItem>
            </Items>
        </radp:RadPanelItem>
        
        <radp:RadPanelItem Text="&lt;b&gt;Product Specific&lt;/b&gt;" Value="5" ExpandedCssClass="Outlook" Expanded="True" runat="server">
            <Items>
                <radp:RadPanelItem runat="server">
                    <ItemTemplate>
                    
                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                            <tr>
                                <td align="center" style="width:19px;"></td>
                                <td align="left">
                                    <asp:Label ID="lblNoDTInProductSpecific" runat="server" Text="No Documents"></asp:Label>
                                </td>
                            </tr>
                            <asp:Repeater ID="rpProductSpecific" runat="server" OnItemDataBound="rp_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td align="center" style="width:50px;">
                                            <asp:CheckBox ID="cbPrint" Checked="true" runat="server" Text="" />
                                        </td>
                                        <td align="left">
                                            <asp:HiddenField ID="hfDTID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"DTID") %>' />
                                            <asp:LinkButton ID="lbtnDTTitle" runat="server" CssClass="linkButton" CommandName="DocClicked" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"DTID") %>' OnCommand="lbtnDTTitle_Command" Text='<%# DataBinder.Eval(Container.DataItem,"DTTitle") %>'></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        
                    </ItemTemplate>
                </radp:RadPanelItem>
            </Items>
        </radp:RadPanelItem>
    </Items>
</radP:RadPanelbar>

