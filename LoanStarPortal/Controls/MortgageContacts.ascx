<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MortgageContacts.ascx.cs" Inherits="LoanStarPortal.Controls.MortgageContacts" %>
<asp:PlaceHolder ID="phContacts" runat="server">
<table class="kontrTab" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
    <tr>
        <td colspan="3">
            <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                <tr>
                    <td><img alt="" src="images/bg_kontrTab_title_left.gif"/></td>
                    <td class="centerTd_title" align="left">
                        <table cellspacing="0" cellpadding="0" align="left" border="0">
                            <tr>
                                <td class="centerTd_title_text">Contacts</td>
                                <td><img alt="" src="images/bg_kontrTab_title_tt_right.gif"/></td>
                            </tr>
                        </table>
                    </td>
                    <td><img alt="" src="images/bg_kontrTab_title_right.gif"/></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="bg_color"><img alt="" src="images/bg_kontrTab_topleft.gif"/></td>
        <td class="bg_kontrTab_top"></td>
        <td class="bg_color"><img alt="" src="images/bg_kontrTab_topright.gif"/></td>
    </tr>
    <tr>
        <td class="bg_kontrTab_left"></td>
        <td class="content_kontrTab">
            <table cellspacing="0" cellpadding="0" align="left" border="0">
                <tr>
                    <td>                        
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <asp:Repeater ID="rpAsignee" runat="server" OnItemDataBound="rpAsignee_ItemDataBound">
                            <ItemTemplate>
                            <tr style="vertical-align:middle;">
                                <td class="contactslabeltd">
                                    <asp:Label ID="lblRole" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="contactsselecttd">
                                    <asp:DropDownList ID="ddlUsers" runat="server" CssClass="contactsuser">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblUser" runat="server" Text=""></asp:Label>                
                                </td>
                                <td class="contactsassigntd">
                                    <input id="btnAssignMe" type="submit" value="Assign Me" runat="server" />
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            </ItemTemplate>
                        </asp:Repeater>    
                    </table>
                    <input id="assignMe" type="hidden" runat="server"/>    
                    </td>
                </tr>
            </table>
        </td>
        <td class="bg_kontrTab_right"></td>
    </tr>
    <tr>
        <td><img alt="" src="images/bg_kontrTab_botleft.gif"/></td>
        <td class="bg_kontrTab_bottom"></td>
        <td><img alt="" src="images/bg_kontrTab_botright.gif"/></td>
    </tr>
</table>
</asp:PlaceHolder>                    