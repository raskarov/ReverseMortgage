<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckList.ascx.cs" Inherits="LoanStarPortal.Controls.CheckList" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>
<script type="text/javascript">
function SetProceedButtonState(i){
    var o=document.getElementById('openIssueCnt');
    var a=o.value;    
    a=a.replace(i,'');
    o.value=a;
    if(a==''){
        var b=document.getElementById('<%=btnSave.ClientID %>');
        b.setAttribute('disabled',false);
    }
}
</script>
<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Width="100%" BorderWidth="0" BorderStyle="None" BorderSize="0" Skin="Default" LiveResize="false">
   <radspl:radpane id="TopPane" runat="server" Height="29px" Scrolling="None">
   <div RadResizeStopLookup="true" RadShowStopLookup="true" >
    <div class="paneTitle">
    <table border="0" cellspacing="0" cellpadding="0" width="100%">
        <tr style="vertical-align:middle">
            <td style="width:50px"><b>Checklist</b></td>
            <td nowrap="nowrap" style="padding-left:10px;width:205px"><b>View checklist for another status:</b></td><td align="left" style="padding-right:2px"><asp:DropDownList ID="ddlStatus" runat="server" DataTextField="Name" DataValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"></asp:DropDownList></td>
            <td align="right">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click"/>
            </td>
        </tr>
    </table>
    </div>
   </div>
   </radspl:radpane>  
   <radspl:radsplitbar id="RadSplitBar3" runat="server" CollapseMode="None" EnableResize="false" Visible="false"/>
   <radspl:radpane id="MiddlePane" runat="server" scrolling="Y" >
    <div RadResizeStopLookup="true" RadShowStopLookup="true" >
    <table border="0" cellspacing="0" cellpadding="0" width="100%">
        <tr style="padding-top:4px">
            <asp:Panel ID="pnlLead" runat="server" Visible="false"><td>&nbsp;</td><td align="right" style="padding-right:3px;"><asp:Button ID="btnCloseLead" runat="server" OnClick="btnCloseLead_Click" Text="Close lead"/></td></asp:Panel>
        </tr>    
        <tr style="padding-top:10px">
            <asp:Panel ID="PanelUnderwriter" runat="server" Visible="false"><td align="left" style="padding-left:10px;width:190px;">Change the status of the loan to:</td><td align="left" style="padding-left:3px"><asp:DropDownList ID="ddlSetStatus" runat="server" DataTextField="Name" DataValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="ddlSetStatus_SelectedIndexChanged"></asp:DropDownList></td></asp:Panel>
        </tr>
    </table>
    <table border="0" cellspacing="0" cellpadding="0" width="100%">
        <tr style="padding-bottom:5px;padding-top:10px" runat="server" id="trReqFields">
            <td colspan="2" style="padding-left:10px;">
                <asp:Label ID="lblReqFields" runat="server" Text="There are still outstanding questions" ForeColor="red"></asp:Label> 
            </td>
        </tr>
        <tr style="padding-top:10px">
            <td colspan="2">
                <table border="0" cellspacing="0" cellpadding="0" width="100%">
                <asp:Repeater ID="rpChecklist" runat="server" OnItemDataBound="rpChecklist_ItemDataBound">
                <AlternatingItemTemplate>
                    <tr style="background-color:#f0f2f4">                    
                        <td style="padding-left:10px;width:500px">
                            <asp:Label ID="lblQuestion" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Question") %>'></asp:Label> 
                        </td>
                        <td>
                            <asp:CheckBox ID="cb" runat="server" Visible="true" AutoPostBack="true" OnCheckedChanged="CheckBox_CheckedChanged"/>
                            <asp:RadioButton ID="rbYes" runat="server" Text="Yes" AutoPostBack="true" Visible="true"/>
                            <asp:RadioButton ID="rbNo" runat="server" Text="No" AutoPostBack="true" Visible="true"/>
                            <asp:RadioButton ID="rbDontknow" runat="server" Text="Don't know" AutoPostBack="true" Visible="true"/>
                            <asp:RadioButton ID="rbToFollow" runat="server" Text="To follow" AutoPostBack="true" Visible="true"/>
                        </td>
                    </tr>            
                </AlternatingItemTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="padding-left:10px;width:500px">
                            <asp:Label ID="lblQuestion" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Question") %>'></asp:Label> 
                        </td>
                        <td>
                            <asp:CheckBox ID="cb" runat="server" AutoPostBack="true" Visible="true" OnCheckedChanged="CheckBox_CheckedChanged"/>
                            <asp:RadioButton ID="rbYes" runat="server" Text="Yes" AutoPostBack="true" />
                            <asp:RadioButton ID="rbNo" runat="server" Text="No" AutoPostBack="true" />
                            <asp:RadioButton ID="rbDontknow" runat="server" Text="Don't know" AutoPostBack="true" />
                            <asp:RadioButton ID="rbToFollow" runat="server" Text="To follow" AutoPostBack="true"  />
                        </td>
                    </tr>
                </ItemTemplate>
                </asp:Repeater>
                </table>
            </td>
        </tr>
    </table>
    </div>
   </radspl:radpane>
   <radspl:radsplitbar id="RadSplitBar1" runat="server" CollapseMode="None" EnableResize="false" Visible="false"/>
    <radspl:RadPane ID="BottomPane" runat="server" Height="30px" Scrolling="None">
    <div RadResizeStopLookup="true" RadShowStopLookup="true" >
    &nbsp;
    </div>
    </radspl:radpane>  
</radspl:radsplitter>