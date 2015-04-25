<%@ Control Language="c#" AutoEventWireup="True" Codebehind="GroupContainer.ascx.cs" Inherits="WebMailPro.classic.GroupContainer" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

<!-- Edit Group -->
<asp:PlaceHolder ID="PlaceHolderGroupView" Runat="server" Visible="True">
	<TABLE class="wm_contacts_view">
		<TR>
			<TD><%=_resMan.GetString("GroupName")%>:</TD>
			<TD class="wm_contacts_name">
				<SPAN id="span_gname"><%=Group.GroupName%></SPAN>&nbsp;<A id="renameLink" onclick="return RenameGroup();" href="#"><%=_resMan.GetString("Rename")%></A>
				<INPUT class="wm_hide" id="groupNameEdit" style="WIDTH: 240px" type="text" name="groupNameEdit" runat="server">
			</TD>
		</TR>
		<TR>
			<TD colSpan="2"><INPUT class="wm_checkbox" id="isorganization" onclick="ShowHideOrgDiv()" type="checkbox"
					name="isorganization" <%=IsOrganization%> > <LABEL for="isorganization">
					<%=_resMan.GetString("TreatAsOrganization")%>
				</LABEL>
			</TD>
		</TR>
	</TABLE>
	
	
<div id="orgDiv">
<table class="wm_contacts_tab" style="margin-top: 20px;" id="orgTab">
	<tr onclick="ShowHideOrgForm()">
		<td>
			<span class="wm_contacts_tab_name"><%=_resMan.GetString("Organization")%></span>
			<span class="wm_contacts_tab_mode">
				<img id="orgTabImg" src="skins/<%=Skin%>/menu/arrow_up.gif"/>
			</span>
		</td>
	</tr>
</table>

<table class="wm_contacts_view" id="orgTable">
	<tr>
		<td class="wm_contacts_view_title" style="width: 20%;"><%=_resMan.GetString("LANG_Email")%>:</td>
		<td style="width: 80%;" colspan="4">
			<input class="wm_input" type="text" maxlength="255" size="45" name="gemail" value="<%=groupEmail%>"/>
		</td>
	</tr>
	<tr>
		<td class="wm_contacts_view_title"><%=_resMan.GetString("Company")%>:</td>
		<td colspan="4">
			<input class="wm_input" type="text" maxlength="65" size="18" name="gcompany" value="<%=groupCompany%>"/>
		</td>
	</tr>
	<tr>
		<td class="wm_contacts_view_title"><%=_resMan.GetString("StreetAddress")%>:</td>
		<td colspan="4">
			<textarea class="wm_input" rows="2" cols="35" name="gstreet"><%=groupStreet%></textarea>
		</td>
	</tr>
	<tr>
		<td class="wm_contacts_view_title" style="width: 20%;"><%=_resMan.GetString("City")%>:</td>
		<td style="width: 30%;">
			<input class="wm_input" type="text" maxlength="65" size="18" name="gcity" value="<%=groupCity%>"/>
		</td>
		<td style="width: 5%;"></td>
		<td class="wm_contacts_view_title" style="width: 15%;"><%=_resMan.GetString("Fax")%>:</td>
		<td style="width: 30%;">
			<input class="wm_input" type="text" maxlength="50" size="18" name="gfax" value="<%=groupFax%>"/>
		</td>
	</tr>
	<tr>
		<td class="wm_contacts_view_title"><%=_resMan.GetString("StateProvince")%>:</td>
		<td>
			<input class="wm_input" type="text" maxlength="65" size="18" name="gstate" value="<%=groupState%>"/>
		</td>
		<td></td>
		<td class="wm_contacts_view_title"><%=_resMan.GetString("Phone")%>:</td>
		<td>
			<input class="wm_input" type="text" maxlength="50" size="18" name="gphone" value="<%=groupPhone%>"/>
		</td>
	</tr>
	<tr>
		<td class="wm_contacts_view_title"><%=_resMan.GetString("ZipCode")%>:</td>
		<td>
			<input class="wm_input" type="text" maxlength="10" size="18" name="gzip" value="<%=groupZip%>"/>
		</td>
		<td></td>
		<td class="wm_contacts_view_title"><%=_resMan.GetString("CountryRegion")%>:</td>
		<td>
			<input class="wm_input" type="text" maxlength="65" size="18" name="gcountry" value="<%=groupCountry%>"/>
		</td>
	</tr>
	<tr>
		<td class="wm_contacts_view_title"><%=_resMan.GetString("WebPage")%>:</td>
		<td colspan="4">
			<input class="wm_input" type="text" maxlength="255" size="45" name="gweb" id="gweb" value="<%=groupWeb%>"/>
			<input class="wm_button" type="button" value="<%=_resMan.GetString("Go")%>" onclick="dolocation('gweb');"/>
		</td>
	</tr>
</table>
</div>

	<%=printGroupContacts()%>
	
	<TABLE class="wm_contacts_view" style="WIDTH: 300px">
	    <TR>
			<TD><%=_resMan.GetString("AddContacts")%>:<BR>
			</TD>
		</TR>
		<TR>
			<TD colSpan="2"><TEXTAREA id="groupEmailsEdit" style="WIDTH: 100%; HEIGHT: 70px" name="emails" rows="2" runat="server"></TEXTAREA>
			</TD>
		</TR>
		<TR>
			<TD><%=_resMan.GetString("CommentAddContacts")%></TD>
			<TD style="TEXT-ALIGN: right">
				<input value="<%=_resMan.GetString("Save")%>" type="button" class="wm_button" id="savebutton" onclick="if (GroupCheck(false)) __doPostBack('PostBackButton','');" />
			</TD>
		</TR>
	</TABLE>
</asp:PlaceHolder>

<!-- New Group -->
<asp:PlaceHolder ID="PlaceHolderGroupAdd" Runat="server" Visible="True">
	<TABLE class="wm_contacts_view">
		<TR>
			<TD><%=_resMan.GetString("GroupName")%>:</TD>
			<TD><INPUT class="wm_input wm_group_name_input" ID="groupNameNew" type="text" maxLength="80"
					name="groupNameNew" runat="server">
			</TD>
		</TR>
		<TR>
			<TD colSpan="2"><INPUT class="wm_checkbox" id="isorganization" onclick="ShowHideOrgDiv()" type="checkbox"
					name="isorganization"> <LABEL for="isorganization">
					<%=_resMan.GetString("TreatAsOrganization")%>
				</LABEL>
			</TD>
		</TR>
	</TABLE>

<div id="orgDiv">
<table class="wm_contacts_tab" style="margin-top: 20px;" id="orgTab">
	<tr onclick="ShowHideOrgForm()">
		<td>
			<span class="wm_contacts_tab_name"><%=_resMan.GetString("Organization")%></span>
			<span class="wm_contacts_tab_mode">
				<img id="orgTabImg" src="skins/<%=Skin%>/menu/arrow_up.gif"/>
			</span>
		</td>
	</tr>
</table>

<table class="wm_contacts_view" id="orgTable">
	<tr>
		<td class="wm_contacts_view_title" style="width: 20%;"><%=_resMan.GetString("LANG_Email")%>:</td>
		<td style="width: 80%;" colspan="4">
			<input class="wm_input" type="text" maxlength="255" size="45" name="gemail" value=""/>
		</td>
	</tr>
	<tr>
		<td class="wm_contacts_view_title"><%=_resMan.GetString("Company")%>:</td>
		<td colspan="4">
			<input class="wm_input" type="text" maxlength="65" size="18" name="gcompany" value=""/>
		</td>
	</tr>
	<tr>
		<td class="wm_contacts_view_title"><%=_resMan.GetString("StreetAddress")%>:</td>
		<td colspan="4">
			<textarea class="wm_input" rows="2" cols="35" name="gstreet"></textarea>
		</td>
	</tr>
	<tr>
		<td class="wm_contacts_view_title" style="width: 20%;"><%=_resMan.GetString("City")%>:</td>
		<td style="width: 30%;">
			<input class="wm_input" type="text" maxlength="65" size="18" name="gcity" value=""/>
		</td>
		<td style="width: 5%;"></td>
		<td class="wm_contacts_view_title" style="width: 15%;"><%=_resMan.GetString("Fax")%>:</td>
		<td style="width: 30%;">
			<input class="wm_input" type="text" maxlength="50" size="18" name="gfax" value=""/>
		</td>
	</tr>
	<tr>
		<td class="wm_contacts_view_title"><%=_resMan.GetString("StateProvince")%>:</td>
		<td>
			<input class="wm_input" type="text" maxlength="65" size="18" name="gstate" value=""/>
		</td>
		<td></td>
		<td class="wm_contacts_view_title"><%=_resMan.GetString("Phone")%>:</td>
		<td>
			<input class="wm_input" type="text" maxlength="50" size="18" name="gphone" value=""/>
		</td>
	</tr>
	<tr>
		<td class="wm_contacts_view_title"><%=_resMan.GetString("ZipCode")%>:</td>
		<td>
			<input class="wm_input" type="text" maxlength="10" size="18" name="gzip" value=""/>
		</td>
		<td></td>
		<td class="wm_contacts_view_title"><%=_resMan.GetString("CountryRegion")%>:</td>
		<td>
			<input class="wm_input" type="text" maxlength="65" size="18" name="gcountry" value=""/>
		</td>
	</tr>
	<tr>
		<td class="wm_contacts_view_title"><%=_resMan.GetString("WebPage")%>:</td>
		<td colspan="4">
			<input class="wm_input" type="text" maxlength="255" size="45" name="gweb" id="gweb" value=""/>
			<input class="wm_button" type="button" value="<%=_resMan.GetString("Go")%>" onclick="dolocation('gweb');"/>
		</td>
	</tr>
</table>
</div>

	<TABLE class="wm_contacts_view wm_add_contacts">
		<TR>
			<TD><%=_resMan.GetString("AddContacts")%>:<BR>
			</TD>
		</TR>
		<TR>
			<TD class="wm_secondary_info"><TEXTAREA id="groupEmailsNew" style="WIDTH: 100%; HEIGHT: 70px" name="contactsEmails" rows="2"
					runat="server"></TEXTAREA><BR>
				<%=_resMan.GetString("CommentAddContacts")%>
			</TD>
		</TR>
		<TR>
			<TD style="TEXT-ALIGN: right">
				<input value="<%=_resMan.GetString("CreateGroup")%>" type="button" class="wm_button" id="addgroupbutton" onclick="if (GroupCheck(true)) __doPostBack('PostBackButton','');" />
		</TR>
	</TABLE>
</asp:PlaceHolder>
