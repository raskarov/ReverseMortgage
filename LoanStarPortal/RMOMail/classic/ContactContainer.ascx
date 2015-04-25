<%@ Control Language="c#" AutoEventWireup="True" Codebehind="ContactContainer.ascx.cs" Inherits="WebMailPro.classic.ContactContainer" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<input runat="server" type="hidden" id="isNewContact" name="isNewContact" value="" />
<input type="hidden" name="contactId" value="" />
<div id="viewTbl">
	<table class="wm_contacts_view">
		<tr runat="server" id="tr_FullName">
			<td class="wm_contacts_view_title"><%=_resMan.GetString("Name")%>:</td>
			<td runat="server" id="td_FullName" class="wm_contacts_name"></td>
		</tr>
		<tr runat="server" id="tr_Email">
			<td class="wm_contacts_view_title"><%=_resMan.GetString("Email")%>:</td>
			<td runat="server" id="td_Email" class="wm_contacts_email"></td>
		</tr>
	</table>
	<table runat="server" id="table_Personal" class="wm_contacts_view">
		<tr>
			<td class="wm_contacts_section_name" colspan="4"><%=_resMan.GetString("Home")%></td>
		</tr>
		<tr runat="server" id="tr_HEmail" >
			<td class="wm_contacts_view_title"><%=_resMan.GetString("PersonalEmail")%>:</td>
			<td colspan="3" runat="server" id="td_HEmail"></td>
		</tr>
		<tr runat="server" id="tr_HStreet">
			<td class="wm_contacts_view_title"><%=_resMan.GetString("StreetAddress")%>:</td>
			<td colspan="3" runat="server" id="td_HStreet"></td>
		</tr>
		<tr runat="server" id="tr_HCityFax">
			<td runat="server" id="td_HCity1" class="wm_contacts_view_title"></td>
			<td runat="server" id="td_HCity2"></td>
			<td runat="server" id="td_HFax1" class="wm_contacts_view_title"></td>
			<td runat="server" id="td_HFax2"></td>
		</tr>
		<tr runat="server" id="tr_HStatePhone">
			<td runat="server" id="td_HState1" class="wm_contacts_view_title"></td>
			<td runat="server" id="td_HState2"></td>
			<td runat="server" id="td_HPhone1" class="wm_contacts_view_title"></td>
			<td runat="server" id="td_HPhone2"></td>
		</tr>
		<tr runat="server" id="tr_HZipMobile">
			<td runat="server" id="td_HZip1"  class="wm_contacts_view_title"></td>
			<td runat="server" id="td_HZip2"></td>
			<td runat="server" id="td_HMobile1"  class="wm_contacts_view_title"></td>
			<td runat="server" id="td_HMobile2"></td>
		</tr>
		<tr runat="server" id="tr_HCountry">
			<td class="wm_contacts_view_title"><%=_resMan.GetString("CountryRegion")%>:</td>
			<td colspan="3" runat="server" id="td_HCountry"></td>
		</tr>
		<tr runat="server" id="tr_HWeb">
			<td class="wm_contacts_view_title"><%=_resMan.GetString("WebPage")%>:</td>
			<td colspan="3" runat="server" id="td_HWeb"></td>
		</tr>
	</table>
	<table runat="server" id="table_Business" class="wm_contacts_view">
		<tr>
			<td class="wm_contacts_section_name" colspan="4"><%=_resMan.GetString("Business")%></td>
		</tr>
		<tr runat="server" id="tr_BEmail" >
			<td class="wm_contacts_view_title"><%=_resMan.GetString("BusinessEmail")%>:</td>
			<td colspan="3" runat="server" id="td_BEmail"></td>
		</tr>
		<tr runat="server" id="tr_BCompanyJob">
			<td runat="server" id="td_BCompany1" class="wm_contacts_view_title"></td>
			<td runat="server" id="td_BCompany2"></td>
			<td runat="server" id="td_BJob1" class="wm_contacts_view_title"></td>
			<td runat="server" id="td_BJob2"></td>
		</tr>
		<tr runat="server" id="tr_BDepartmentOffice">
			<td runat="server" id="td_BDepartment1" class="wm_contacts_view_title"></td>
			<td runat="server" id="td_BDepartment2"></td>
			<td runat="server" id="td_BOffice1" class="wm_contacts_view_title"></td>
			<td runat="server" id="td_BOffice2"></td>
		</tr>
		<tr runat="server" id="tr_BStreet">
			<td class="wm_contacts_view_title"><%=_resMan.GetString("StreetAddress")%>:</td>
			<td colspan="3" runat="server" id="td_BStreet"></td>
		</tr>
		<tr runat="server" id="tr_BCityFax">
			<td runat="server" id="td_BCity1" class="wm_contacts_view_title"></td>
			<td runat="server" id="td_BCity2"></td>
			<td runat="server" id="td_BFax1" class="wm_contacts_view_title"></td>
			<td runat="server" id="td_BFax2"></td>
		</tr>
		<tr runat="server" id="tr_BStatePhone">
			<td runat="server" id="td_BState1"  class="wm_contacts_view_title"></td>
			<td runat="server" id="td_BState2"></td>
			<td runat="server" id="td_BPhone1" class="wm_contacts_view_title"></td>
			<td runat="server" id="td_BPhone2"></td>
		</tr>
		<tr runat="server" id="tr_BZipCountry">
			<td runat="server" id="td_BZip1"  class="wm_contacts_view_title"></td>
			<td runat="server" id="td_BZip2"></td>
			<td runat="server" id="td_BCountry1" class="wm_contacts_view_title"></td>
			<td runat="server" id="td_BCountry2"></td>
		</tr>
		<tr runat="server" id="tr_BWeb">
			<td class="wm_contacts_view_title"><%=_resMan.GetString("WebPage")%>:</td>
			<td colspan="3" runat="server" id="td_BWeb"></td>
		</tr>
	</table>
	<table runat="server" id="table_Other" class="wm_contacts_view">
		<tr>
			<td class="wm_contacts_section_name" colspan="2"><%=_resMan.GetString("Other")%></td>
		</tr>
		<tr runat="server" id="tr_Birthday">
			<td class="wm_contacts_view_title"><%=_resMan.GetString("Birthday")%>:</td>
			<td runat="server" id="td_Birthday"></td>
		</tr>
		<tr runat="server" id="tr_OMail">
			<td class="wm_contacts_view_title"><%=_resMan.GetString("OtherEmail")%>:</td>
			<td runat="server" id="td_OMail"></td>
		</tr>
		<tr runat="server" id="tr_Notes">
			<td class="wm_contacts_view_title"><%=_resMan.GetString("Notes")%>:</td>
			<td id="td_Notes"></td>
		</tr>
	</table>
	<%=printGroup(1)%>
	<table class="wm_contacts_view">
		<tr>
			<td>
				<a href="#" id="switch_to_edit"><%=_resMan.GetString("EditContact")%></a>
			</td>
		</tr>
	</table>
</div>

<!---->

<div id="editTbl" class="wm_hide">
	<table class="wm_contacts_view">
		<tr>
			<td class="wm_contacts_view_title" style="width: 25%;"><%=_resMan.GetString("DefaultEmail")%>:</td>
			<td style="width: 75%;">
				<span id="notSpecified" class="wm_hide"><%=_resMan.GetString("NotSpecifiedYet")%></span>
				<select id="select_default_email" class="wm_hide" style="width: 200px;"></select>
				<input runat="server" id="input_default_email" name="input_default_email" type="text" class="wm_input" maxlength="255"/>
				<input runat="server" id="default_email_type" type="hidden" name="default_email_type" />
			</td>
		</tr>
		<tr>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("Name")%>:</td>
			<td><input runat="server" class="wm_input" type="text" id="c_fullname" name="c_fullname" maxlength="85" /></td>
		</tr>
		<tr>
			<td></td>
			<td>
				<input runat="server" class="wm_checkbox" type="checkbox" name="use_friendly_name" id="use_friendly_name" checked="checked" value="1">
				<label for="ContactsID_ContactsViewID_use_friendly_name"><%=_resMan.GetString("UseFriendlyName1")%></label>
				<label class="wm_secondary_info wm_inline_info" for="ContactsID_ContactsViewID_use_friendly_name"><%=_resMan.GetString("UseFriendlyName2")%></label>
			</td>
		</tr>
	</table>
	<div class="wm_hide" id="more_info_div">
	<table class="wm_contacts_view" style="width: 94%; margin: 0px 15px 2px 15px;">
		<tr>
			<td style="text-align: right; border-top: solid 1px #8D8C89;">
				<a href="" id="more_info_hide"><%=_resMan.GetString("HideAddFields")%></a>
			</td>
		</tr>
	</table>
	<table class="wm_contacts_tab" onclick="ChangeTabVisibility('access');">
		<tr>
			<td>
				<span class="wm_contacts_tab_name">
					<%=_resMan.GetString("Home")%>
				</span>
				<span class="wm_contacts_tab_mode">
					<img id="button_access" src="skins/<%=Skin%>/menu/arrow_up.gif">
				</span>
			</td>
		</tr>
	</table>
	<table <%=Tab1%> id="access">
		<tr>
			<td class="wm_contacts_view_title" style="width: 20%;"><%=_resMan.GetString("PersonalEmail")%>:</td>
			<td style="width: 80%;" colspan="4">
			<input runat="server" class="wm_input" type="text" size="45" id="personal_email" name="personal_email" maxlength="255" /></td>
		</tr>
		<tr>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("StreetAddress")%>:</td>
			<td colspan="4"><textarea runat="server" id="personal_street" class="wm_input" rows="2" cols="35" name="personal_street"></textarea></td>
		</tr>
		<tr>
			<td class="wm_contacts_view_title" style="width: 20%;"><%=_resMan.GetString("City")%>:</td>
			<td style="width: 30%;">
			<input class="wm_input" runat="server" type="text" size="18" id="personal_city" name="personal_city" maxlength="65" /></td>
			<td style="width: 10%;"></td>
			<td class="wm_contacts_view_title" style="width: 10%;"><%=_resMan.GetString("Fax")%>:</td>
			<td style="width: 30%;">
			<input class="wm_input" runat="server" type="text" size="18" id="personal_fax" name="personal_fax"  maxlength="50"/></td>
		</tr>
		<tr>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("StateProvince")%>:</td>
			<td><input runat="server" class="wm_input" type="text" size="18" id="personal_state" name="personal_state" maxlength="65" /></td>
			<td></td>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("Phone")%>:</td>
			<td><input runat="server" class="wm_input" type="text" size="18" id="personal_phone" name="personal_phone" maxlength="50" /></td>
		</tr>
		<tr>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("ZipCode")%>:</td>
			<td><input runat="server" class="wm_input" type="text" size="18" id="personal_zip" name="personal_zip" maxlength="10" /></td>
			<td></td>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("Mobile")%>:</td>
			<td><input runat="server" class="wm_input" type="text" size="18" id="personal_mobile" name="personal_mobile" maxlength="50" /></td>
		</tr>
		<tr>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("CountryRegion")%>:</td>
			<td colspan="4"><input runat="server" class="wm_input" type="text" size="18" id="personal_country" name="personal_country" maxlength="65" /></td>
		</tr>
		<tr>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("WebPage")%>:</td>
			<td colspan="4">
				<input runat="server" class="wm_input" type="text" size="45" name="personal_web" id="personal_web" maxlength="255" />
				<input class="wm_button" type="button" value="<%=_resMan.GetString("Go")%>" onClick="dolocation(GetServerElementByID('personal_web').id);" />
			</td>
		</tr>
	</table>
	<table class="wm_contacts_tab" onclick="ChangeTabVisibility('online_addresses');">
		<tr>
			<td>
				<span class="wm_contacts_tab_name">
					<%=_resMan.GetString("Business")%>
				</span>
				<span class="wm_contacts_tab_mode">
					<img id="button_online_addresses" src="skins/<%=Skin%>/menu/arrow_down.gif">
				</span>
			</td>
		</tr>
	</table>
	<table <%=Tab2%> id="online_addresses">
		<tr>
			<td class="wm_contacts_view_title" style="width: 20%;"><%=_resMan.GetString("BusinessEmail")%>:</td>
			<td style="width: 80%;" colspan="4">
			<input runat="server" class="wm_input" type="text" id="business_email" name="business_email" size="45" maxlength="255" /></td>
		</tr>
		<tr>
			<td class="wm_contacts_view_title" style="width: 20%;"><%=_resMan.GetString("Company")%>:</td>
			<td style="width: 30%;">
			<input runat="server" class="wm_input" type="text" size="18" id="business_company" name="business_company" maxlength="65" /></td>
			<td style="width: 5%;"></td>
			<td class="wm_contacts_view_title" style="width: 15%;"><%=_resMan.GetString("JobTitle")%>:</td>
			<td style="width: 30%;">
			<input runat="server" class="wm_input" type="text" size="18" id="business_job" name="business_job" maxlength="30" /></td>
		</tr>
		<tr>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("Department")%>:</td>
			<td><input runat="server" class="wm_input" type="text" size="18" id="business_departament" name="business_departament" maxlength="65" /></td>
			<td></td>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("Office")%>:</td>
			<td><input runat="server" class="wm_input" type="text" size="18" id="business_office" name="business_office" maxlength="65" /></td>
		</tr>
		<tr>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("StreetAddress")%>:</td>
			<td colspan="4"><textarea runat="server" rows="2" class="wm_input" cols="35" id="business_street" name="business_street"></textarea></td>
		</tr>
		<tr>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("City")%>:</td>
			<td><input runat="server" class="wm_input" type="text" size="18" id="business_city" name="business_city" maxlength="65" /></td>
			<td></td>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("Fax")%>:</td>
			<td><input runat="server" class="wm_input" type="text" size="18" id="business_fax" name="business_fax" maxlength="50" /></td>
		</tr>
		<tr>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("StateProvince")%>:</td>
			<td><input runat="server" class="wm_input" type="text" size="18" id="business_state" name="business_state" maxlength="65" /></td>
			<td></td>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("Phone")%>:</td>
			<td><input runat="server" class="wm_input" type="text" size="18" id="business_phone" name="business_phone" maxlength="50" /></td>
		</tr>
		<tr>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("ZipCode")%>:</td>
			<td><input runat="server" class="wm_input" type="text" size="18" id="business_zip" name="business_zip" maxlength="10" /></td>
			<td></td>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("CountryRegion")%>:</td>
			<td colspan="4"><input runat="server" class="wm_input" type="text" id="business_country" name="business_country" size="18" maxlength="65" /></td>
		</tr>
		<tr>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("WebPage")%>:</td>
			<td colspan="4">
				<input runat="server" class="wm_input" type="text" size="45" id="business_web" name="business_web" maxlength="255" />
				<input class="wm_button" type="button" value="<%=_resMan.GetString("Go")%>" onClick="dolocation(GetServerElementByID('personal_web').id);" />
			</td>
		</tr>
	</table>
	<table class="wm_contacts_tab" onclick="ChangeTabVisibility('phone_numbers');">
		<tr>
			<td>
				<span class="wm_contacts_tab_name">
					<%=_resMan.GetString("Other")%>
				</span>
				<span class="wm_contacts_tab_mode">
					<img id="button_phone_numbers" src="skins/<%=Skin%>/menu/arrow_down.gif">
				</span>
			</td>
		</tr>
	</table>
	<table <%=Tab3%> id="phone_numbers">
		<tr>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("Birthday")%>:</td>
			<td>
				<select runat="server" id="birthday_month" name="birthday_month"></select>
				<select runat="server" id="birthday_day" name="birthday_day"></select>
				<select runat="server" id="birthday_year" name="birthday_year"></select>
			</td>
		</tr>
		<tr>
			<td class="wm_contacts_view_title" style="width: 20%;"><%=_resMan.GetString("OtherEmail")%>:</td>
			<td style="width: 80%;">
			<input runat="server" class="wm_input" id="other_email" name="other_email" type="text" size="45" maxlength="255" /></td>
		</tr>
		<tr>
			<td class="wm_contacts_view_title"><%=_resMan.GetString("Notes")%>:</td>
			<td><textarea runat="server" rows="2" class="wm_input" cols="35" id="other_notes" name="other_notes"></textarea></td>
		</tr>
	</table>
	<table runat="server" id="groupTableControl" class="wm_contacts_tab" onclick="ChangeTabVisibility('street_addresses');">
		<tr>
			<td>
				<span class="wm_contacts_tab_name">
					<%=_resMan.GetString("Groups")%>
				</span>
				<span class="wm_contacts_tab_mode">
					<img id="button_street_addresses" src="skins/<%=Skin%>/menu/arrow_down.gif" />
				</span>
			</td>
		</tr>
	</table>
	<table class="wm_hide" id="street_addresses">
		<tr><td>
			<%=printGroup(2)%>
		</td></tr>
	</table>
	</div>
	<table class="wm_contacts_view" style="width: 94%; margin: 0px 15px 2px 15px;">
		<tr>
			<td style="text-align: right;">
				<a href="#" id="more_info_show"><%=_resMan.GetString("ShowAddFields")%></a>
			</td>
		</tr>
		<tr>
			<td style="text-align: right; border-top: solid 1px #8D8C89;">
				<input runat="server" type="button" id="submitButton" class="wm_button" onclick="checkNameAndEMailFields();" />
				<input runat="server" type="button" id="cancelButton" class="wm_button" onclick="" />
			</td>
		</tr>
	</table>
</div>