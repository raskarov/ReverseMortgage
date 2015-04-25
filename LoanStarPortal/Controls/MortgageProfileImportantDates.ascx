<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MortgageProfileImportantDates.ascx.cs" Inherits="LoanStarPortal.Controls.MortgageProfileImportantDates" %>
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
