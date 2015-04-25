/*
 * Classes:
 * CNewAccountForm
 */

function CNewAccountForm()		
{
	this.fm_protocol = document.getElementById("settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_comboBoxProtocol");
	this.fm_advanced_options = document.getElementById("pop_advanced");

	this.fm_inbox_sync = document.getElementById("settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_comboBoxInboxSyncType");
	this.fm_mail_management_mode1 = document.getElementById("settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_mail_mode_0");
	this.fm_mail_management_mode2 = document.getElementById("settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_mail_mode_1");
	this.fm_keep_for_x_days = document.getElementById("settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_mail_mode_2");
	this.fm_keep_messages_days = document.getElementById("settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_textBoxDays");
	this.fm_delete_messages_from_trash = document.getElementById("settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_mail_mode_3");
	this.fm_int_deleted_as_server = document.getElementById("settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_checkBoxDeleteNoExists");

	this.pop_advanced = document.getElementById("pop_advanced");
	this.arr_inputs = this.pop_advanced.getElementsByTagName("input");

	this.email = document.getElementById("settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_textBoxEmail");
	this.inc_server = document.getElementById("settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_textBoxIncHost");
	this.incoming_port = document.getElementById("settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_textBoxIncPort");
	this.smtp_server = document.getElementById("settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_textBoxOutHost");
	this.smtp_server_port = document.getElementById("settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_textBoxOutPort");
	this.inc_password = document.getElementById("settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_textBoxIncPassword");
	this.inc_login = document.getElementById("settingsID_settingsEmailAccountsID_settingsEmailAccountsPropertiesID_textBoxIncLogin");
}
	
CNewAccountForm.prototype = {
	ShowPOP3AdvancedOptions: function ()
	{
		this.SetDefaultData();
		var obj = this;
		this.fm_protocol.onchange = function() {
			if (obj.fm_protocol.value == "imap") {
				obj.fm_advanced_options.className = "wm_hide";
				obj.incoming_port.value = "143";
			}
			else {
				obj.fm_advanced_options.className = "";
				obj.incoming_port.value = "110";
			}
		};
		
		this.fm_inbox_sync.onchange = function() {
			if (obj.fm_inbox_sync.value == 1) {
				obj.fm_mail_management_mode1.disabled = true;
				obj.fm_mail_management_mode1.checked = false;
				obj.fm_mail_management_mode2.disabled = false;
				obj.fm_mail_management_mode2.checked = true;
				obj.fm_keep_for_x_days.disabled = false;
				obj.fm_delete_messages_from_trash.disabled = false;
				obj.fm_int_deleted_as_server.disabled = false;
				obj.CheckKeepForDays();
			}
			else if (obj.fm_inbox_sync.value == 3) {
				obj.fm_mail_management_mode1.disabled = false;
				obj.fm_mail_management_mode2.disabled = false;
				obj.fm_keep_for_x_days.disabled	= false;
				obj.fm_delete_messages_from_trash.disabled = false;
				obj.fm_int_deleted_as_server.disabled = false;
				obj.CheckKeepForDays();
			}
			else if (obj.fm_inbox_sync.value == 5) {
				for (i=0; i<obj.arr_inputs.length; i++) {
					obj.arr_inputs[i].disabled = true;
				}
			}
		};//fm_inbox_sync.onchange
		
		this.fm_keep_for_x_days.onclick = function() {
			obj.CheckKeepForDays();
		};

		this.fm_mail_management_mode1.onclick = function() {
			obj.fm_mail_management_mode1.disabled = false;
			obj.fm_mail_management_mode2.disabled = false;
			obj.fm_keep_for_x_days.disabled = true;
			obj.fm_delete_messages_from_trash.disabled = true;				
			obj.fm_keep_messages_days.disabled = true;
			
			obj.fm_int_deleted_as_server.checked = false;
			obj.fm_int_deleted_as_server.disabled = true;
		};
		
		this.fm_mail_management_mode2.onclick = function() {
			obj.fm_mail_management_mode1.disabled = false;
			obj.fm_mail_management_mode2.disabled = false;
			obj.fm_keep_for_x_days.disabled	 = false;
			obj.fm_delete_messages_from_trash.disabled = false;				
			obj.CheckKeepForDays();
		    obj.fm_int_deleted_as_server.disabled = false;
		}
	},
	
	CheckKeepForDays: function ()
	{
		if (this.fm_keep_for_x_days.checked) {
			this.fm_keep_messages_days.disabled = false;
			this.fm_int_deleted_as_server.disabled = true;
			this.fm_int_deleted_as_server.checked = false;
		}
		else {
			this.fm_keep_messages_days.disabled = true;
			this.fm_int_deleted_as_server.disabled = false;
		}
	},
	
	SetDefaultData: function()
	{
		if (this.fm_protocol.value == "pop") {
			this.fm_advanced_options.className = "";
		}
		else {
			this.fm_advanced_options.className = "wm_hide";
		};
		
		if (this.fm_inbox_sync.value == 1) {
			this.fm_mail_management_mode1.disabled = true;
			this.fm_mail_management_mode2.disabled = false;
			this.fm_keep_for_x_days.disabled = false;
			this.fm_delete_messages_from_trash.disabled = false;
			this.fm_int_deleted_as_server.disabled	= false;
			this.CheckKeepForDays();
		};

		if (this.fm_inbox_sync.value == 3) {
			if (this.fm_mail_management_mode1.checked == true) {
				this.fm_mail_management_mode2.disabled = false;
				this.fm_keep_for_x_days.disabled = true;
				this.fm_keep_messages_days.disabled = true;
				this.fm_delete_messages_from_trash.disabled = true;
			};
			if (this.fm_mail_management_mode2.checked == true) {
				this.fm_mail_management_mode1.disabled = false;				
				this.fm_keep_for_x_days.disabled = false;
				this.fm_delete_messages_from_trash.disabled = false;
				this.CheckKeepForDays();
			};
			this.fm_int_deleted_as_server.disabled = false;
		};
		
		if (this.fm_inbox_sync.value == 5)
		{
			for (i=0; i<this.arr_inputs.length; i++)
			{
				this.arr_inputs[i].disabled = true;
			}
		};
		
		if (this.fm_mail_management_mode1.checked)
		{
			this.fm_int_deleted_as_server.checked = false;
			this.fm_int_deleted_as_server.disabled = true;
		}
	},//setDefaultData
	
	CheckFields: function()
	{
		var oVal = new CValidate();
		if (oVal.IsEmpty(this.email.value)) {
			alert(Lang.WarningEmailFieldBlank);
			return false;
		};
		if (!oVal.IsCorrectEmail(this.email.value)) {
			alert(Lang.WarningCorrectEmail);
			return false;
		};
		if (oVal.IsEmpty(this.inc_login.value)) {
			alert(Lang.WarningLoginFieldBlank);
			return false;
		};
		if (oVal.IsEmpty(this.inc_password.value)) {
			alert(Lang.WarningIncPassBlank);
			return false;
		};
		if (oVal.IsEmpty(this.inc_server.value)) {
			alert(Lang.WarningIncServerBlank);
			return false;
		};
		if (!oVal.IsCorrectServerName(this.inc_server.value)) {
			alert(Lang.WarningCorrectIncServer);
			return false;
		};
		if (oVal.IsEmpty(this.incoming_port.value)) {
			alert(Lang.WarningIncPortBlank);
			return false;
		};
		if (!oVal.IsPort(this.incoming_port.value)) {
			alert(Lang.WarningIncPortNumber + Lang.DefaultIncPortNumber);
			return false;
		};
		if (oVal.IsEmpty(this.smtp_server.value)) {
			alert(Lang.WarningOutServerBlank);
			return false;
		};
		if (!oVal.IsCorrectServerName(this.smtp_server.value)) {
			alert(Lang.WarningCorrectSMTPServer);
			return false;
		};
		if (oVal.IsEmpty(this.smtp_server_port.value)) {
			alert(Lang.WarningOutPortBlank);
			return false;
		};
		if (!oVal.IsPort(this.smtp_server_port.value)) {
			alert(Lang.WarningOutPortNumber + Lang.DefaultOutPortNumber);
			return false;
		};
		if (this.fm_mail_management_mode2.checked && !oVal.IsPositiveNumber(this.fm_keep_messages_days.value)) {
			alert(Lang.WarningMailsOnServerDays);
			return false;
		};
		return true;
	}
};