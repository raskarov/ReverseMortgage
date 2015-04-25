<%@ Control Language="c#" AutoEventWireup="True" Codebehind="MailList.ascx.cs" Inherits="WebMailPro.classic.Inbox" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div class="wm_inbox" id="inbox_div">
	<div class="wm_inbox_headers">
		<div style="LEFT: 0px; WIDTH: 21px" class="">
		    <input type="checkbox" id="allcheck" onclick="javascript:InboxLines.CheckAllBox(this)" />
		</div>
		<div style="LEFT: 22px; WIDTH: 1px" class="wm_inbox_headers_separate_noresize"></div>
		<div style="LEFT: 23px; WIDTH: 17px" class="wm_control" id="attachment" onclick="javascript:Sort(this);">
			<%	if ((_sort_field == 10) && (_sort_order == 0)) { %>
			<img src="./skins/<%=Skin%>/menu/order_arrow_down.gif">
			<%	}
				else if ((_sort_field == 10) && (_sort_order == 1)) { %>
			<img src="./skins/<%=Skin%>/menu/order_arrow_up.gif">
			<%	} else {%>
			<img src="skins/<%=Skin%>/menu/attachment.gif">
			<%	}%>
		</div>
		<div style="LEFT: 41px; WIDTH: 1px" class="wm_inbox_headers_separate_noresize"></div>
		<div style="LEFT: 43px; WIDTH: 17px" class="wm_control" id="flag" onclick="javascript:Sort(this);">
			<%	if ((_sort_field == 12) && (_sort_order == 0)) { %>
			<img src="./skins/<%=Skin%>/menu/order_arrow_down.gif">
			<%	}
				else if ((_sort_field == 12) && (_sort_order == 1)) { %>
			<img src="./skins/<%=Skin%>/menu/order_arrow_up.gif">
			<%	} else {%>
			<img src="skins/<%=Skin%>/menu/flag.gif">
			<%	}%>
		</div>
		<div style="LEFT: 61px; WIDTH: 1px" class="wm_inbox_headers_separate_noresize"></div>
		<div style="LEFT: 63px; WIDTH: 147px" class="wm_inbox_headers_from_subject wm_control"
			id="from" onclick="javascript:Sort(this);">
			<%=_manager.GetString(LabelToFrom)%>
			<%	if ((_sort_field == 2) && (_sort_order == 0)) { %>
			<img src="./skins/<%=Skin%>/menu/order_arrow_down.gif">
			<%	}
				else if ((_sort_field == 2) && (_sort_order == 1)) { %>
			<img src="./skins/<%=Skin%>/menu/order_arrow_up.gif">
			<%	}%>
		</div>
		<div style="LEFT: 211px; WIDTH: 1px" class="wm_inbox_headers_separate_noresize"></div>
		<div style="LEFT: 213px; WIDTH: 137px" class="wm_control" id="date" onclick="javascript:Sort(this);">
			<%=_manager.GetString("Date")%>
			<%	if ((_sort_field == 0) && (_sort_order == 0)) { %>
			<img src="./skins/<%=Skin%>/menu/order_arrow_down.gif">
			<%	}
				else if ((_sort_field == 0) && (_sort_order == 1)) { %>
			<img src="./skins/<%=Skin%>/menu/order_arrow_up.gif">
			<%	}%>
		</div>
		<div style="LEFT: 351px; WIDTH: 1px" class="wm_inbox_headers_separate_noresize"></div>
		<div style="LEFT: 353px; WIDTH: 47px" class="wm_control" id="size" onclick="javascript:Sort(this);">
			<%=_manager.GetString("Size")%>
			<%	if ((_sort_field == 6) && (_sort_order == 0)) { %>
			<img src="./skins/<%=Skin%>/menu/order_arrow_down.gif">
			<%	}
				else if ((_sort_field == 6) && (_sort_order == 1)) { %>
			<img src="./skins/<%=Skin%>/menu/order_arrow_up.gif">
			<%	}%>
		</div>
		<div style="LEFT: 401px; WIDTH: 1px" class="wm_inbox_headers_separate_noresize"></div>
		<div style="LEFT: 404px; WIDTH: 147px" class="wm_inbox_headers_from_subject wm_control"
			onclick="javascript:Sort(this);">
			<%=_manager.GetString("Subject")%>
			<%	if ((_sort_field == 8) && (_sort_order == 0)) { %>
			<img src="./skins/<%=Skin%>/menu/order_arrow_down.gif">
			<%	}
				else if ((_sort_field == 8) && (_sort_order == 1)) { %>
			<img src="./skins/<%=Skin%>/menu/order_arrow_up.gif">
			<%	}%>
		</div>
	</div>
	<div class="wm_inbox_lines" id="list_container">
		<asp:Literal id="LiteralList" runat="server"></asp:Literal>
	</div>
</div>
