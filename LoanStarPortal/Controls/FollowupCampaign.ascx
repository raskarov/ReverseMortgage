<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FollowupCampaign.ascx.cs" Inherits="LoanStarPortal.Controls.FollowupCampaign" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<%@ Register Src="EmailAdd.ascx" TagName="EmailAdd" TagPrefix="uc1" %>
<script language="javascript" type="text/javascript">
<!--
var isSelectAllowed=false;
function CalendarDateSelecting(c,step){
    return isSelectAllowed;
}
function CalendarViewChanged(c,step){
    var v = c.CurrentViews[0];
    var h = document.getElementById('currentcampaigndata');
    if(h!=null){
        var a=h.value.split(':');
        if(a.length>1){
            var req='campaignid='+a[0]+'&mid='+a[1]+'&year='+v._MonthStartDate[0]+'&month='+v._MonthStartDate[1];
            GetSelectedDate(req,c)
        }
    }
}
function UpdateCalendar(c,resp){
    c.UnselectDates(c.GetSelectedDates());
    if(resp.length>0){
        var a=resp.split('&');
        var v=c.CurrentViews[0]
        isSelectAllowed=true;
        for(var i=0;i<a.length;i++){
            var ar = new Array();
            ar[0] =v._MonthStartDate[0];
            ar[1]= v._MonthStartDate[1];
            ar[2] = a[i];
            c.SelectDate(ar,false);
        }
        isSelectAllowed=false;
    }   
}
function GetSelectedDate(req,c){
    var xmlhttp = GetXmlHttp();
    if (xmlhttp==null){
        alert('ajax not supported');
        return;
    }
	var url=document.URL;
	var i = url.indexOf('?');
	if (i>0){
	    url=url.substring(0,i);
	}
	url = url.toLowerCase().replace('default.aspx','ProcessRequest.aspx');
	url+='?'+req;
	xmlhttp.open("GET",url,true);	
	xmlhttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');						
	xmlhttp.setRequestHeader('Accept-Language','en/us');
	xmlhttp.setRequestHeader("Content-Length", "0");
	xmlhttp.onreadystatechange=function(){
		if(xmlhttp.readyState==4){
			var resp=xmlhttp.responseText;
			var statusText=xmlhttp.statusText;
			var status=xmlhttp.status;			
			if (status==200){
			    UpdateCalendar(c,resp);
			    xmlhhtp=null;
			}else{
            	alert(statusText);
			}
		}
	}
	xmlhttp.send(null);
}
-->
</script>
<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Width="100%" BorderWidth="0" BorderStyle="None" Skin="Default">
    <radspl:radpane id="TopPane" runat="server" Height="29px" Scrolling="None">
        <div class="paneTitle">
            <table border="0" cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td class="title" nowrap="nowrap"><asp:Label ID="lblTitle" runat="server" Text="Campaigns manager"></asp:Label></td>
                </tr>
            </table>
        </div>
    </radspl:radpane>  
    <radspl:radsplitbar id="RadSplitBar3" runat="server" CollapseMode="None" EnableResize="false" Visible="false"/>
        <radspl:radpane id="MiddlePane" runat="server" scrolling="Y" >
            <asp:Panel ID="PanelTasks" runat="server">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top" width="75%" bordercolorlight="#000000">
                            <radG:RadGrid ID="gCampaigns" Skin="Default" runat="server" AutoGenerateColumns="False" EnableAJAX="False" GridLines="Vertical" 
                                AllowPaging="True" PageSize="30" Width="99%"
                                OnItemCommand="gCampaigns_ItemCommand" 
                                OnItemDataBound="gCampaigns_ItemDataBound" 
                                OnPageIndexChanged="gCampaigns_PageIndexChanged"            
                            >
                            <MasterTableView DataKeyNames="ID">
                                <NoRecordsTemplate>No items</NoRecordsTemplate>
                                <ExpandCollapseColumn Visible="False">
                                <HeaderStyle Width="19px" />
                                </ExpandCollapseColumn>
                                <RowIndicatorColumn Visible="False">
                                    <HeaderStyle Width="20px" />
                                </RowIndicatorColumn>
                            <Columns>                            
                                <radg:GridBoundColumn HeaderText="SID" DataField="SID" UniqueName="SID" Display= "False"></radg:GridBoundColumn>
                                <radG:GridButtonColumn UniqueName="TitleColumn" HeaderText="Title" CommandName="LoadItem" DataTextField="Title" ItemStyle-Width="85%"></radG:GridButtonColumn>
                                <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px">
                                    <ItemTemplate>
                                        <div style="vertical-align:top;">
                                        <asp:ImageButton ID="ibtnEmail" runat="server" ImageUrl="~/Images/Add_Email.gif" BorderWidth="0" ImageAlign="Bottom" CommandName="CreateEmail"/>&nbsp;&nbsp;/&nbsp;&nbsp;<asp:ImageButton ID="ibtnNote" runat="server" ImageUrl="~/Images/Add-Note.gif" BorderWidth="0" ImageAlign="Bottom" CommandName="CreateNote"/></div>
                                    </ItemTemplate>
                                </radG:GridTemplateColumn>
                            </Columns>
                            </MasterTableView>
                            <PagerStyle Mode="NumericPages" />
                            <ClientSettings EnablePostBackOnRowClick="false"></ClientSettings>
                            </radG:RadGrid>        
                        </td>
                        <td align="left" valign="top" style="width:310px">
                        <div class="paneGrid" style="width:310px;height:25px;"><b>Details</b></div>
                        <div style="width:310px;">
                            <table border="0" cellspacing="0" cellpadding="0" style="width:100%" class="TasksHeader">
                                <tr>
                                    <td class="TasksHeader" width="100%"><a href="#" onclick="HideShowDiv('TableCampaignDetails');" class="TasksHeader"><div style="width:100%;cursor:hand;">Campaign details</div></a></td>
                                </tr>
                                <tr>
                                    <td>
                                        <table id="TableCampaignDetails" style="width:100%;">
                                            <tr>
                                                <td align="left" style="width:70px;padding-left:5px">Title</td>
                                                <td style="width:95%"><asp:TextBox ID="tbCampaignTitle" runat="server" Width="215px" CssClass="task" ReadOnly="true"></asp:TextBox></td>
                                            </tr>
                                            <tr valign="top">
                                                <td align="left" style="width:70px;padding-left:5px">Details</td>
                                                <td style="width:95%"><asp:TextBox ID="tbCampaignDetails" runat="server" TextMode="MultiLine" Rows="5" Width="215px" CssClass="task" ReadOnly="true"></asp:TextBox></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table border="0" cellspacing="0" cellpadding="0" width="100%" style="width:100%" class="TasksHeader">
                                <tr>
                                    <td class="TasksHeader"><a href="#" onclick="HideShowDiv('TableCampaignFollowUp');" class="TasksHeader"><div style="width:100%;cursor:hand;">Follow up details</div></a></td>
                                </tr>
                                <tr>
                                    <td style="width:100%">
                                        <table id="TableCampaignFollowUp" style="width:100%">
                                            <tr>
                                                <td style="width:100%"><asp:Label ID="lblStartDate" runat="server" Text=""></asp:Label></td>
                                            </tr>
                                            <tr align="center">
                                                <td style="width:100%;padding-bottom:5px;">
                                                    <radCln:RadCalendar ID="rdcSchedule" runat="server"  Width="200px" Skin="WebBlue" >
                                                        <SelectedDayStyle BackColor="Red"/>
                                                        <ClientEvents OnCalendarViewChanged="CalendarViewChanged" OnDateSelecting="CalendarDateSelecting">
                                                        </ClientEvents>
                                                    </radCln:RadCalendar>
                                                </td>
                                            </tr>
                                                <tr>
                                                    <td style="width:100%"><asp:Label ID="lblNextFollowupDate" runat="server" Text=""></asp:Label></td>
                                                </tr>                                                
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table border="0" cellspacing="0" cellpadding="0" width="100%" style="width:100%" class="TasksHeader" runat="server" id="tblNotes">
                                <tr>
                                    <td class="TasksHeader"><a href="#" onclick="HideShowDiv('TableCampaignNote');return false;" class="TasksHeader"><div style="width:100%;cursor:hand;">Campaign Note</div></a></td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style="width:100%" id="TableCampaignNote">
                                            <tr>
                                                <td colspan="2"><asp:TextBox ID="tbNote" runat="server" TextMode="MultiLine" Rows="5" Width="96%" CssClass="task"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="tbNote" ValidationGroup="val_campaign" SetFocusOnError="true"></asp:RequiredFieldValidator></td>
                                            </tr>
                                            <tr>
                                                <td align="right">Select one:</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlCampaignAction" runat="server" Width="228px" CssClass="task">
                                                         </asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="ddlCampaignAction" ValidationGroup="val_campaign" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table border="0" cellpadding="0" cellspacing="5" style="width:100%">
                                                        <tr>
                                                            <td style="height: 24px"><asp:Button ID="btnShowCampaignNotes" runat="server" Text="Campaign notes" OnClick="btnShowCampaignNotes_Click" /></td>
                                                            <td align="right" style="height: 24px"><asp:Button ID="btnSubmitCampaignNote" runat="server" Text="Add Note" ValidationGroup="val_campaign" OnClick="btnSubmitCampaignNote_Click" /></td>
                                                        </tr>
                                                    </table>
                                                </td>                                    
                                            </tr>
                                        </table>
                                     </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="PanelEmailAdd" runat="server" Visible="False" Width="100%" Height="100%">
                <uc1:EmailAdd ID="EmailAdd1" runat="server" />
            </asp:Panel>
    </radspl:radpane>
</radspl:RadSplitter>    

