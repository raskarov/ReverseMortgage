function CNewContactScreenPart(isEdit, controlId)
{
	this._controlId = controlId; // ContactsID_ContactsViewID_
	this._moreInfo = document.getElementById("more_info");
	
	this._isMoreInfo = false;
	this._moreInfo = document.getElementById("more_info_div");
	this._showMoreInfo = document.getElementById("more_info_show");
	this._hideMoreInfo = document.getElementById("more_info_hide");

	this._defaultEmailSel = document.getElementById("select_default_email");
	this._defaultEmailInp = document.getElementById(this._controlId + "input_default_email");
	this._notSpecified = document.getElementById("notSpecified");
	
	this._bEmailObj = document.getElementById(this._controlId + "business_email");
	this._hEmailObj = document.getElementById(this._controlId + "personal_email");
	this._otherEmailObj = document.getElementById(this._controlId + "other_email");
	this._defEmailType = document.getElementById(this._controlId + "default_email_type");
	
	this._viewTbl = document.getElementById("viewTbl");
	this._editTbl = document.getElementById("editTbl");
	this._switch_to_edit = document.getElementById("switch_to_edit");
	this._cancelExistButton = document.getElementById("ContactsID_ContactsViewID_cancelButton");
	this._cancelNewButton = document.getElementById("ContactsID_ContactsAddID_cancelButton");
	
	this._contactScreen = isEdit; // 0 - view contact, 1 - edit contact
	this._emailAdded = false;
}
		
CNewContactScreenPart.prototype = {		
		
		FillContact: function(mode, isOpen)
		{
			var obj = this;
			if (mode == 0)
			{
			    this._contactScreen = 0;
				this._viewTbl.className = "";	
				this._editTbl.className = "wm_hide";

				this._switch_to_edit.onclick = function()
				{
				    obj._contactScreen = 1;
					obj.FillContact(1);
					if (isOpen) obj.ShowMoreInfo();
					ResizeElements("all");
					return false;
				}
			}
			else if (mode == 1)
			{
			    this._contactScreen = 1;
				this._viewTbl.className = "wm_hide";			
				this._editTbl.className = "";	
	            
	            if (null != this._cancelExistButton)
	            {
				    this._cancelExistButton.onclick = function()
				    {
					    obj.FillContact(0);
				    }
				}
				
				if (null != this._cancelNewButton)
				{
				    this._cancelNewButton.onclick = function()
				    {
					    document.location = 'basewebmail.aspx?scr=contacts';
				    }
				}

				switch (parseInt(this._defEmailType.value))
				{
					case 0:
						this._defaultEmailInp.value = this._hEmailObj.value;
						break;
					case 1:
						this._defaultEmailInp.value = this._bEmailObj.value;
						break;
					case 2:
						this._defaultEmailInp.value = this._otherEmailObj.value;
						break;
					default:
						this._defaultEmailInp.value = this._hEmailObj.value;
						break;
				}

					
				obj.FillDefaultEmailSel();		
				this._hEmailObj.onchange = function()
				{
					obj.FillDefaultEmailSel();
				}
				this._bEmailObj.onchange = function()
				{
					obj.FillDefaultEmailSel();
				}
				this._otherEmailObj.onchange = function()
				{
					obj.FillDefaultEmailSel();
				}
				this._showMoreInfo.onclick = function()
				{
					obj.ShowMoreInfo();
					ResizeElements("all");
					return false;
				}
				this._hideMoreInfo.onclick = function()
				{
					obj.HideMoreInfo();
					ResizeElements("all");
					return false;
				}
				
				if (this._isMoreInfo == false)
				{
					this._notSpecified.className = 'wm_hide';	
					this._defaultEmailSel.className = 'wm_hide';
					this._defaultEmailInp.className = 'wm_input';				
				}
				else
				{
					if (this._hEmailObj.length == 0 && this._bEmailObj.length == 0 && this._otherEmailObj.length == 0)
					{
						this._notSpecified.className = '';
						this._defaultEmailSel.className = 'wm_hide';
					}
					else
					{	
						this._notSpecified.className = 'wm_hide';
						this._defaultEmailSel.className = '';
					}
					
					this._defaultEmailInp = 'wm_hide';
				}
			
			} 
			
		},
		
		InitEditContacts: function(isOpen)
		{
			if (this._contactScreen == 0) //view contact
			{
				this.FillContact(0, isOpen);
				
			} else if (this._contactScreen == 1) { //edit contact

				this.FillContact(1, isOpen);
			}
			
			var obj = this;
            this._defaultEmailSel.onchange = function()
            {
            
                obj._defEmailType.value = obj._defaultEmailSel.value;
            }
			
		},//Init contacts
				
		FillDefaultEmailSel: function()
		{
			var emails = Array();
			var title = Array();
			var ids = Array();
			emails[0] = this._hEmailObj.value;
			emails[1] = this._bEmailObj.value;
			emails[2] = this._otherEmailObj.value;
			title[0] = Lang.Personal + ": ";
			title[1] = Lang.Business + ": ";
			title[2] = Lang.Other + ": ";
			ids[0] = "0";
			ids[1] = "1";
			ids[2] = "2";

			
			if (emails[0].length == 0 && emails[1].length == 0 && emails[2].length == 0) 
			{
				if (this._defaultEmailInp.value.length != 0)
				{
					this.addOption(this._defaultEmailSel, title[0] + this._defaultEmailInp.value, ids[0]);
					this._emailAdded = true;
					this._hEmailObj.value = this._defaultEmailInp.value;
				}
			} else {
				this._notSpecified.className = 'wm_hide';
				this._defaultEmailSel.className = '';
				this.removeAll(this._defaultEmailSel);
				for (var i=0; i<=2; i++) {
					if (emails[i].length != 0) {
						if (this._defEmailType.value == i)
						{
							this.addOption(this._defaultEmailSel, title[i] + emails[i], ids[i], true);
						} else {
							this.addOption(this._defaultEmailSel, title[i] + emails[i], ids[i], false);
						}
						this._emailAdded = true;
					}
				}
			}
		}, //FillDefaultEmailSel
		
		addOption: function(parent, text, value, isDefaultSelected, isSelected)
		{
			var oOption = document.createElement("option");
			oOption.appendChild(document.createTextNode(text));
			oOption.setAttribute("value", value);

			if (isDefaultSelected) oOption.defaultSelected = true;
		  	else if (isSelected) oOption.selected = true;
		
		  	parent.appendChild(oOption);
		},
		
		removeAll: function(parent)
		{
			parent.options.length = 0;
		},
		
		ShowMoreInfo: function()
		{
			this._moreInfo.className = '';
			this._showMoreInfo.className = 'wm_hide';

			if (this._hEmailObj.value.length == 0 && this._bEmailObj.value.length == 0 && this._otherEmailObj.value.length == 0)
			{
				if (this._defaultEmailInp.value.length != 0)
				{
					this._defaultEmailSel.className = '';
					this.addOption(this._defaultEmailSel, Lang.Personal + ": " + this._defaultEmailInp.value, 0); //******
					this._emailAdded = true;
					this._hEmailObj.value = this._defaultEmailInp.value;
					this._notSpecified.className = 'wm_hide';
					
				} else {
					this._notSpecified.className = '';
					this._defaultEmailSel.className = 'wm_hide';
				}
			}
			else
			{	
				var obj = this.TakeEmailAccordingHiddenField(this._defaultEmailSel.options[this._defaultEmailSel.selectedIndex].value);
				obj.value = this._defaultEmailInp.value;
				this.FillDefaultEmailSel();
			
				this._notSpecified.className = 'wm_hide';
				this._defaultEmailSel.className = '';
			}
			this._defaultEmailInp.className = 'wm_hide';
			this._defaultEmailInp.value = "";
			this._isMoreInfo = true;
		},
		
		HideMoreInfo: function()
		{
			this._moreInfo.className = 'wm_hide';
			this._defaultEmailSel.className = 'wm_hide';
			this._showMoreInfo.className = '';
			this._defaultEmailInp.className = 'wm_input';
			if (this._emailAdded == true)
			{
				this._defEmailType.value = this._defaultEmailSel.options[this._defaultEmailSel.selectedIndex].value;
				if (this._defaultEmailSel.value.length != 0) this._defaultEmailInp.value = this.TakeEmailAccordingHiddenField(this._defEmailType.value).value;
			}
			this._notSpecified.className = 'wm_hide';
			this._isMoreInfo = false;
		},
		
		TakeEmailAccordingHiddenField: function(index)
		{
			if (index == 0) return this._hEmailObj;
			else if (index == 1) return this._bEmailObj;
			else if (index == 2) return this._otherEmailObj;
		}
		
}