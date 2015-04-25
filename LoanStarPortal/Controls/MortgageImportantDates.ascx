<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MortgageImportantDates.ascx.cs" Inherits="LoanStarPortal.Controls.MortgageImportantDates" %>
<asp:PlaceHolder ID="phImportantDates" runat="server">
<table class="kontrTab" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
    <tr>
        <td colspan="3">
            <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                <tr>
                    <td><img alt="" src="images/bg_kontrTab_title_left.gif"/></td>
                    <td class="centerTd_title" align="left">
                        <table cellspacing="0" cellpadding="0" align="left" border="0">
                            <tr>
                                <td class="centerTd_title_text">Important dates</td>
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
                        <asp:Repeater ID="rpDates" runat="server">
                            <ItemTemplate>
                            <tr style="vertical-align:middle;">
                                <td class="tddatesname">
                                    <asp:Label ID="lblDateName" runat="server" Text='<%# Eval("StatusName") %>'></asp:Label>
                                </td>
                                <td class="tddates">
                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("ImportantDate") %>'></asp:Label>
                                </td>                                
                            </tr>
                            </ItemTemplate>
                        
                        </asp:Repeater>
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