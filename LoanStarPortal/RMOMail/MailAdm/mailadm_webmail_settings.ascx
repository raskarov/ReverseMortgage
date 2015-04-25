<%@ Control Language="c#" AutoEventWireup="True" Codebehind="mailadm_webmail_settings.ascx.cs" Inherits="WebMailPro.MailAdm.mailadm_webmail_settings" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type="text/javascript">
	function mailProtocolChange() 
	{
		// change port
		var select = document.getElementById('wmSettingsID_intIncomingMailProtocol');
		var port = document.getElementById('wmSettingsID_intIncomingMailPort');
		port.value = (select.value == 0) ? '110' : '143'; 
	}
	
	function Run()
	{
		if (document.getElementById('wmSettingsID_intEnableAttachmentSizeLimit').checked)
		{
			document.getElementById('wmSettingsID_intAttachmentSizeLimit').readOnly = false;
			document.getElementById('wmSettingsID_intAttachmentSizeLimit').style.background = "#FFFFFF";
		}
		else
		{
			document.getElementById('wmSettingsID_intAttachmentSizeLimit').readOnly = true;
			document.getElementById('wmSettingsID_intAttachmentSizeLimit').style.background = "#EEEEEE";
		}
		
		if (document.getElementById('wmSettingsID_intEnableMailboxSizeLimit').checked)
		{
			document.getElementById('wmSettingsID_intMailboxSizeLimit').readOnly = false;
			document.getElementById('wmSettingsID_intMailboxSizeLimit').style.background = "#FFFFFF";
		}
		else
		{
			document.getElementById('wmSettingsID_intMailboxSizeLimit').readOnly = true;
			document.getElementById('wmSettingsID_intMailboxSizeLimit').style.background = "#EEEEEE";
		}
		document.getElementById('wmSettingsID_intDirectModeIsDefault').disabled = !document.getElementById('wmSettingsID_intAllowDirectMode').checked;
		//document.getElementById('wmSettingsID_intAttachmentSizeLimit').disabled = !document.getElementById('wmSettingsID_intEnableAttachmentSizeLimit').checked;
		//document.getElementById('wmSettingsID_intMailboxSizeLimit').disabled = !document.getElementById('wmSettingsID_intEnableMailboxSizeLimit').checked;
	}
</script>
<table class="wm_admin_center" width="500" border="0">
	<tr>
		<td width="150"></td>
		<td width="160"></td>
		<td></td>
	</tr>
	<tr>
		<td class="wm_admin_title" colSpan="3">WebMail Settings</td>
	</tr>
	<tr>
		<td colSpan="2"><br>
		</td>
	</tr>
	<tr>
		<td align="right">Site name:
		</td>
		<td colSpan="2"><input type="text" id="txtSiteName" size="50" runat="server" class="wm_input" maxlength="100"></td>
	</tr>
	<tr>
		<td align="right" valign="top">License key:
		</td>
		<td colSpan="2" valign="top">
			<input type="password" value="<%Response.Write(licenseKey);%>" name="txtLicenseKey" id="txtLicenseKey" maxlength="50" size="50" class="wm_input" >
			<div runat="server" id="errorLicenseKeyLabelID" class="messdiv" />
		</td>
	</tr>
	<tr>
		<td colSpan="3"><br>
		</td>
	</tr>
	<tr>
		<td class="wm_admin_title" colSpan="3">Default Mail Server Settings</td>
	</tr>
	<tr>
		<td colSpan="3"><br>
		</td>
	</tr>
	<tr>
		<td align="right">Incoming Mail:
		</td>
		<td><input type="text" id="txtIncomingMail" maxlength="100" runat="server" class="wm_input"></td>
		<td><nobr>Port:&nbsp;<input type="text" id="intIncomingMailPort" maxlength="4" size="3" runat="server" class="wm_input">
				&nbsp;<select id="intIncomingMailProtocol" runat="server" onchange="Run()">
					<option value="POP3" selected>POP3</option>
					<option Value="IMAP4">IMAP4</option>
					<option Value="XMail">XMail</option>
				</select></nobr>
		</td>
	</tr>
	<tr>
		<td align="right">Outgoing Mail:
		</td>
		<td><input type="text" id="txtOutgoingMail" maxlength="100" runat="server" class="wm_input"></td>
		<td>Port:&nbsp;<input type="text" id="intOutgoingMailPort" maxlength="4" size="3" runat="server" class="wm_input">
		</td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
		<td colSpan="2"><input type="checkbox" id="intReqSmtpAuthentication" style="VERTICAL-ALIGN: middle" runat="server">
			<label for="wmSettingsID_intReqSmtpAuthentication">Requires SMTP authentication</label>
		</td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
		<td colSpan="2"><input type="checkbox" id="intAllowDirectMode" style="VERTICAL-ALIGN: middle" runat="server"
				onclick="Run()"><label for="wmSettingsID_intAllowDirectMode">Allow direct mode</label>
		</td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
		<td colSpan="2"><input type="checkbox" id="intDirectModeIsDefault" style="VERTICAL-ALIGN: middle" Runat="server">
			<label for="wmSettingsID_intDirectModeIsDefault">Direct mode is default</label>
		</td>
	</tr>
	<tr>
		<td align="right">Attachment size limit:
		</td>
		<td colSpan="2"><input type="text" id="intAttachmentSizeLimit" maxlength="10" runat="server" class="wm_input"
				style="WIDTH: 85px"> bytes&nbsp;&nbsp;&nbsp; <input type="checkbox" id="intEnableAttachmentSizeLimit" style="VERTICAL-ALIGN: middle"
				runat="server" onclick="Run()"><label for="wmSettingsID_intEnableAttachmentSizeLimit">Enable 
				attachment size limit</label>
		</td>
	</tr>
	<tr>
		<td align="right">Mailbox size limit:
		</td>
		<td colSpan="2"><input type="text" id="intMailboxSizeLimit" maxlength="10" runat="server" class="wm_input"
				style="WIDTH: 85px"> bytes &nbsp;&nbsp;&nbsp; <input type="checkbox" id="intEnableMailboxSizeLimit" style="VERTICAL-ALIGN: middle" runat="server"
				onclick="Run()"><label for="wmSettingsID_intEnableMailboxSizeLimit">Enable 
				mailbox size limit</label>
		</td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
		<td colSpan="2"><input type="checkbox" id="intAllowUsersChangeEmailSettings" style="VERTICAL-ALIGN: middle"
				runat="server"><label for="wmSettingsID_intAllowUsersChangeEmailSettings">Allow new
				users to change email settings</label>
		</td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
		<td colSpan="2"><input type="checkbox" id="intAllowNewUsersRegister" style="VERTICAL-ALIGN: middle" runat="server">
			<label for="wmSettingsID_intAllowNewUsersRegister">Allow automatic registration of 
				new users on first login</label>
		</td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
		<td colSpan="2"><input type="checkbox" id="intAllowUsersAddNewAccounts" style="VERTICAL-ALIGN: middle"
				runat="server"> <label for="wmSettingsID_intAllowUsersAddNewAccounts">Allow 
				users to add new email accounts</label>
		</td>
	</tr>
	<tr>
		<td colSpan="2"><br>
		</td>
	</tr>
	<tr>
		<td class="wm_admin_title" colSpan="3">Internationalization Support</td>
	</tr>
	<tr>
		<td colSpan="3"><br>
		</td>
	</tr>
	<tr>
		<td align="right">Default user charset
		</td>
		<td colSpan="2"><select id="txtDefaultUserCharset" runat="server" class="wm_input" style="WIDTH: 320px">
				<option Value="0" selected>Default</option>
				<option Value="950">Chinese Traditional</option>
				<option Value="949">Korean (EUC)</option>
				<option Value="50225">Korean (ISO)</option>
				<option Value="50220">Japanese</option>
				<option Value="932">Japanese (Shift-JIS)</option>
				<option Value="28591">Western Alphabet (ISO)</option>
				<option Value="28592">Central European Alphabet (ISO)</option>
				<option Value="28593">Latin 3 Alphabet (ISO)</option>
				<option Value="28594">Baltic Alphabet (ISO)</option>
				<option Value="28595">Cyrillic Alphabet (ISO)</option>
				<option Value="28596">Arabic Alphabet (ISO)</option>
				<option Value="28597">Greek Alphabet (ISO)</option>
				<option Value="28598">Hebrew Alphabet (ISO)</option>
				<option Value="20866">Cyrillic Alphabet (KOI8-R)</option>
				<option Value="65000">Universal Alphabet (UTF-7)</option>
				<option Value="65001">Universal Alphabet (UTF-8)</option>
				<option Value="1250">Central European Alphabet (Windows)</option>
				<option Value="1251">Cyrillic Alphabet (Windows)</option>
				<option Value="1252">Western Alphabet (Windows)</option>
				<option Value="1253">Greek Alphabet (Windows)</option>
				<option Value="1254">Turkish Alphabet</option>
				<option Value="1255">Hebrew Alphabet (Windows)</option>
				<option Value="1256">Arabic Alphabet (Windows)</option>
				<option Value="1257">Baltic Alphabet (Windows)</option>
				<option Value="1258">Vietnamese Alphabet (Windows)</option>
			</select></td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
		<td colSpan="2"><input type="checkbox" id="intAllowUsersChangeCharset" style="VERTICAL-ALIGN: middle" runat="server"><label for="wmSettingsID_intAllowUsersChangeCharset">Allow 
				users to change charset</label>
		</td>
	</tr>
	<tr>
		<td align="right">Default user time offset</td>
		<td colSpan="2"><select id="txtDefaultTimeZone" runat="server" class="wm_input" style="WIDTH: 320px">
				<option Value="0" selected>Default</option>
				<option Value="1">(GMT -12:00) Eniwetok, Kwajalein, Dateline Time</option>
				<option Value="2">(GMT -11:00) Midway Island, Samoa</option>
				<option Value="3">(GMT -10:00) Hawaii</option>
				<option Value="4">(GMT -09:00) Alaska</option>
				<option Value="5">(GMT -08:00) Pacific Time (US &amp; Canada); Tijuana</option>
				<option Value="6">(GMT -07:00) Arizona</option>
				<option Value="7">(GMT -07:00) Mountain Time (US &amp; Canada)</option>
				<option Value="8">(GMT -06:00) Central America</option>
				<option Value="9">(GMT -06:00) Central Time (US &amp; Canada)</option>
				<option Value="10">(GMT -06:00) Mexico City, Tegucigalpa</option>
				<option Value="11">(GMT -06:00) Saskatchewan</option>
				<option Value="12">(GMT -05:00) Indiana (East)</option>
				<option Value="13">(GMT -05:00) Eastern Time (US &amp; Canada)</option>
				<option Value="14">(GMT -05:00) Bogota, Lima, Quito</option>
				<option Value="15">(GMT -04:00) Santiago</option>
				<option Value="16">(GMT -04:00) Caracas, La Paz</option>
				<option Value="17">(GMT -04:00) Atlantic Time (Canada)</option>
				<option Value="18">(GMT -03:30) Newfoundland</option>
				<option Value="19">(GMT -03:00) Greenland</option>
				<option Value="20">(GMT -03:00) Buenos Aires, Georgetown</option>
				<option Value="21">(GMT -03:00) Brasilia</option>
				<option Value="22">(GMT -02:00) Mid-Atlantic</option>
				<option Value="23">(GMT -01:00) Cape Verde Is.</option>
				<option Value="24">(GMT -01:00) Azores</option>
				<option Value="25">(GMT) Casablanca, Monrovia</option>
				<option Value="26">(GMT) Dublin, Edinburgh, Lisbon, London</option>
				<option Value="27">(GMT +01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna</option>
				<option Value="28">(GMT +01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague</option>
				<option Value="29">(GMT +01:00) Brussels, Copenhagen, Madrid, Paris</option>
				<option Value="30">(GMT +01:00) Sarajevo, Skopje, Sofija, Vilnius, Warsaw, Zagreb</option>
				<option Value="31">(GMT +01:00) West Central Africa</option>
				<option Value="32">(GMT +02:00) Athens, Istanbul, Minsk</option>
				<option Value="33">(GMT +02:00) Bucharest</option>
				<option Value="34">(GMT +02:00) Cairo</option>
				<option Value="35">(GMT +02:00) Harare, Pretoria</option>
				<option Value="36">(GMT +02:00) Helsinki, Riga, Tallinn</option>
				<option Value="37">(GMT +02:00) Israel, Jerusalem Standard Time</option>
				<option Value="38">(GMT +03:00) Baghdad</option>
				<option Value="39">(GMT +03:00) Arab, Kuwait, Riyadh</option>
				<option Value="40">(GMT +03:00) Moscow, St. Petersburg, Volgograd</option>
				<option Value="41">(GMT +03:00) East Africa, Nairobi</option>
				<option Value="42">(GMT +03:30) Tehran</option>
				<option Value="43">(GMT +04:00) Abu Dhabi, Muscat</option>
				<option Value="44">(GMT +04:00) Baku, Tbilisi, Yerevan</option>
				<option Value="45">(GMT +04:30) Kabul</option>
				<option Value="46">(GMT +05:00) Ekaterinburg</option>
				<option Value="47">(GMT +05:00) Islamabad, Karachi, Sverdlovsk, Tashkent</option>
				<option Value="48">(GMT +05:30) Calcutta, Chennai, Mumbai, New Delhi, India 
					Standard Time</option>
				<option Value="49">(GMT +05:45) Kathmandu, Nepal</option>
				<option Value="50">(GMT +06:00) Almaty, Novosibirsk, North Central Asia</option>
				<option Value="51">(GMT +06:00) Astana, Dhaka</option>
				<option Value="52">(GMT +06:00) Sri Jayewardenepura, Sri Lanka</option>
				<option Value="53">(GMT +06:30) Rangoon</option>
				<option Value="54">(GMT +07:00) Bangkok, Hanoi, Jakarta</option>
				<option Value="55">(GMT +07:00) Krasnoyarsk</option>
				<option Value="56">(GMT +08:00) Beijing, Chongqing, Hong Kong SAR, Urumqi</option>
				<option Value="57">(GMT +08:00) Irkutsk, Ulaan Bataar</option>
				<option Value="58">(GMT +08:00) Kuala Lumpur, Singapore</option>
				<option Value="59">(GMT +08:00) Perth, Western Australia</option>
				<option Value="60">(GMT +08:00) Taipei</option>
				<option Value="61">(GMT +09:00) Osaka, Sapporo, Tokyo</option>
				<option Value="62">(GMT +09:00) Seoul, Korea Standard time</option>
				<option Value="63">(GMT +09:00) Yakutsk</option>
				<option Value="64">(GMT +09:30) Adelaide, Central Australia</option>
				<option Value="65">(GMT +09:30) Darwin</option>
				<option Value="66">(GMT +10:00) Brisbane, East Australia</option>
				<option Value="67">(GMT +10:00) Canberra, Melbourne, Sydney, Hobart</option>
				<option Value="68">(GMT +10:00) Guam, Port Moresby</option>
				<option Value="69">(GMT +10:00) Hobart, Tasmania</option>
				<option Value="70">(GMT +10:00) Vladivostok</option>
				<option Value="71">(GMT +11:00) Magadan, Solomon Is., New Caledonia</option>
				<option Value="72">(GMT +12:00) Auckland, Wellington</option>
				<option Value="73">(GMT +12:00) Fiji Islands, Kamchatka, Marshall Is.</option>
				<option Value="74">(GMT +13:00) Nuku'alofa, Tonga</option>
			</select></td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
		<td colSpan="2"><input type="checkbox" id="intAllowUsersChangeTimeOffset" style="VERTICAL-ALIGN: middle"
				runat="server"><label for="wmSettingsID_intAllowUsersChangeTimeOffset">Allow 
				users to change time offset</label>
		</td>
	</tr>
	<tr>
		<td colSpan="3"><br>
		</td>
	</tr>
	<tr>
		<td class="wm_admin_title" colSpan="3">Password</td>
	</tr>
	<tr>
		<td colSpan="3"><br>
		</td>
	</tr>
	<tr>
		<td align="right">New password:
		</td>
		<td colSpan="2"><asp:TextBox TextMode="password" cssclass="wm_input" id="txtPasswordNew" maxlength="100" runat="server" /></td>
	</tr>
	<tr>
		<td align="right">Confirm password:
		</td>
		<td colSpan="2"><asp:TextBox TextMode="password" cssclass="wm_input" id="txtPasswordConfirm" maxlength="100"
				runat="server" /></td>
	</tr>
	<tr>
		<td align="center" colSpan="2"><br>
			<div Runat="server" ID="messLabelID" Class="messdiv" />
			<br>
			<div runat="server" id="errorLabelID" class="messdiv" />
		</td>
	</tr> <!-- hr -->
	<tr>
		<td colSpan="3">
			<hr SIZE="1">
		</td>
	</tr>
	<tr>
		<td align="right" colSpan="3"><asp:Button id="SaveButton" style="FONT-WEIGHT: bold" runat="server" text="Save" Width="100" onclick="SaveButton_Click" />&nbsp;
		</td>
	</tr>
</table>
<script type="text/javascript">
	Run();
</script>
