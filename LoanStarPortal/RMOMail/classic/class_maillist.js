/*
Classes:
	CvResizerPart
	ChResizerPart
	CMainContainer
	Base64
*/

var Base64 = {
	_keyStr : "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",
	encode : function (input) {
		var output = "";
		var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
		var i = 0;
		input = Base64._utf8_encode(input);
		while (i < input.length) {
			chr1 = input.charCodeAt(i++);
			chr2 = input.charCodeAt(i++);
			chr3 = input.charCodeAt(i++);
			enc1 = chr1 >> 2;
			enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
			enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
			enc4 = chr3 & 63;
			if (isNaN(chr2)) {
				enc3 = enc4 = 64;
			} else if (isNaN(chr3)) {
				enc4 = 64;
			}
			output = output +
			this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) +
			this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);
		}
		return output;
	},

	decode : function (input) {
		var output = "";
		var chr1, chr2, chr3;
		var enc1, enc2, enc3, enc4;
		var i = 0;
		input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");
		while (i < input.length) {
			enc1 = this._keyStr.indexOf(input.charAt(i++));
			enc2 = this._keyStr.indexOf(input.charAt(i++));
			enc3 = this._keyStr.indexOf(input.charAt(i++));
			enc4 = this._keyStr.indexOf(input.charAt(i++));
			chr1 = (enc1 << 2) | (enc2 >> 4);
			chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
			chr3 = ((enc3 & 3) << 6) | enc4;
			output = output + String.fromCharCode(chr1);
			if (enc3 != 64) {
				output = output + String.fromCharCode(chr2);
			}
			if (enc4 != 64) {
				output = output + String.fromCharCode(chr3);
			}
		}
		output = Base64._utf8_decode(output);
		return output;
	},

	_utf8_encode : function (string) {
		string = string.replace(/\r\n/g,"\n");
		var utftext = "";
		for (var n = 0; n < string.length; n++) {
			var c = string.charCodeAt(n);
			if (c < 128) {
				utftext += String.fromCharCode(c);
			}
			else if((c > 127) && (c < 2048)) {
				utftext += String.fromCharCode((c >> 6) | 192);
				utftext += String.fromCharCode((c & 63) | 128);
			}
			else {
				utftext += String.fromCharCode((c >> 12) | 224);
				utftext += String.fromCharCode(((c >> 6) & 63) | 128);
				utftext += String.fromCharCode((c & 63) | 128);
			}
		}
		return utftext;
	},

	_utf8_decode : function (utftext) {
		var string = "";
		var i = 0;
		var c = c1 = c2 = 0;
		while ( i < utftext.length ) {
			c = utftext.charCodeAt(i);
			if (c < 128) {
				string += String.fromCharCode(c);
				i++;
			}
			else if((c > 191) && (c < 224)) {
				c2 = utftext.charCodeAt(i+1);
				string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
				i += 2;
			}
			else {
				c2 = utftext.charCodeAt(i+1);
				c3 = utftext.charCodeAt(i+2);
				string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
				i += 3;
			}
		}
		return string;
	}
}

function CvResizerPart()
{
	this.resizer = document.getElementById('vert_resizer');
	this.width = 2;
}

CvResizerPart.prototype =
{
	resizeElements: function(height)
	{
		MovableVerticalDiv.updateVerticalSize(height);
	}
}

function ChResizerPart()
{
	this.resizer = document.getElementById('hor_resizer');
	this.height = 2;	
}

ChResizerPart.prototype =
{
	resizeElements: function(width)
	{
		MovableHorizontalDiv.updateHorizontalSize(width);
	}
}

function CMainContainer()
{
		this.table = document.getElementById("main_container");
		this.info = document.getElementById("info");
		this.logo = document.getElementById("logo");
		this.accountslist = document.getElementById("accountslist");
		this.toolbar = document.getElementById("toolbar");
		this.lowtoolbar = document.getElementById("lowtoolbar");
		//logo + accountslist + toolbar + lowtoolbar
		
		this.external_height = 56 + 32 + 26 + 24;
		this.inner_height = 362;
}

CMainContainer.prototype =
{
	getExternalHeight: function()
	{
		var offsetHeight; var res = 0;
		offsetHeight = this.accountslist.offsetHeight;	if (offsetHeight) { res += offsetHeight; } else { res += 32; }
		offsetHeight = this.logo.offsetHeight;			if (offsetHeight >= 0) { res += offsetHeight; } else { res += 50; }
		offsetHeight = this.toolbar.offsetHeight;		if (offsetHeight) { res += offsetHeight; } else { res += 26; }
		offsetHeight = this.lowtoolbar.offsetHeight;	if (offsetHeight) { res += offsetHeight; } else { res += 24; }

		if (res != 0) this.external_height = res;
		return this.external_height;
	}
}

function CFoldersPart(mode, skinName)
{
	this.isPreviewPane = mode;
	this.container = document.getElementById('folders_part');
	this.width = 115;
	this.height = 100;
	this.realwidth = this.width;
	this.folders = document.getElementById('folders');
	this.folders_hide = document.getElementById('folders_hide');
	this.folders_hide_img = document.getElementById('folders_hide_img');
	this.manage_folders = document.getElementById('manage_folders');
	
	this.skinName = skinName;

	//border + manage folders + hide folders
	this.external_height = 2 + 22 + 20;
	
}

CFoldersPart.prototype =
{
	resizeElementsWidth: function(width)
	{
		this.width = width;
		//MovableVerticalDiv._leftPosition = width;
		this.container.style.width = this.width + 'px';
		this.folders.style.width = this.width + 'px';
		this.folders_hide.style.width = this.width + 'px';
		this.manage_folders.style.width = this.width + 'px';
	},
	
	resizeElementsHeight: function(height)
	{
		this.height = height;
		this.container.style.height = (this.height) + 'px';
		this.folders.style.height = (this.height - this.external_height) + 'px';
	},
	
	show: function()
	{
		this.width = this.realwidth;
		this.folders_hide_img.src = './skins/' + this.skinName + '/folders/hide_folders.gif';
		this.folders_hide_img.title = Lang.HideFolders;
		this.folders.className = 'wm_folders';
		this.manage_folders.className = 'wm_manage_folders';
		CreateCookie('wm_hide_folders', 0, 20);
	},
	
	hide: function()
	{
		this.realwidth = this.width;
		this.width = 18;
		this.folders_hide_img.src = './skins/' + this.skinName + '/folders/show_folders.gif';
		this.folders_hide_img.title = Lang.ShowFolders;
		if (this.isPreviewPane) {
			this.folders.className = 'wm_hide';
			this.manage_folders.className = 'wm_hide';
		} else {
			this.folders.className = 'wm_unvisible';
			this.manage_folders.className = 'wm_unvisible';
		}
		CreateCookie('wm_hide_folders', 1, 20);
	}
}

function CMessagesBox(isPreviewPane, PageSwitcherClass)
{
	this.isPreviewPane = isPreviewPane;
	
	// list
	this.page_switcher = PageSwitcherClass._mainCont;
	this.list_table = document.getElementById('inbox_part');
	this.list_div = document.getElementById('inbox_div');
	
	this.list_container = document.getElementById('list_container');
	this.list = document.getElementById('list');
	this.subject = document.getElementById('subject');
	
	// message
	this.mess_table = document.getElementById('message_td');
	this.mess_container = document.getElementById('message_container');
	this.mess_table = document.getElementById('message_table');
	this.mess_header = document.getElementById('message_headers');
	this.mess_body = document.getElementById('messageBody');
	this.mess_center = document.getElementById('message_center');
	this.mess_attach = document.getElementById('MessageAttachments');
	this.mess_res = document.getElementById('vres');
	
	this.height = 350;
	this.width = 500; 
	
	this.min_height = 350;
	this.min_width = 500;
	
	/*
	this.list_width = 350;
	this.list_height = 300;
	
	this.list_min_height = 100;
	this.list_min_width = 500;
	
	this.mess_width = 350;
	this.mess_height = 500;
	
	this.mess_min_height = 100;
	this.mess_min_width = 500;
	*/
	
	this.min_upper = 20 + 1;
	this.min_lower = 2 + 4 + 45 + 100;
}

CMessagesBox.prototype =
{

	getMessageHeaderHeight: function()
	{	
		var h = this.mess_header.offsetHeight;
		if (document.getElementById('eis')) h+=document.getElementById('eis').offsetHeight;
		return (h && h > 0) ? h : 50;
	},

	resizeElementsHeight: function(height, mode)
	{
		this.height = (height > this.min_height) ? height : this.min_height;
		
		var listHeight = (this.isPreviewPane) ? MovableHorizontalDiv._topPosition - this.min_upper - 4: height;
		this.list_table.style.height = listHeight + 'px';
		this.list_div.style.height = listHeight + 'px';
		this.list_container.style.height = (listHeight - this.min_upper) + 'px';
		
		var messHeight = this.height - listHeight - hResizerPart.height - 4;
		if (messHeight < 100) messHeight = 100;

		this.mess_table.style.height = messHeight + 'px';
		this.mess_container.style.height = messHeight + 'px';
		this.mess_body.style.height = messHeight - this.getMessageHeaderHeight() + 'px';
		this.mess_center.style.height = messHeight - this.getMessageHeaderHeight() - 16 + 'px';
		this.mess_attach.style.height = messHeight - this.getMessageHeaderHeight() + 'px';
		this.mess_res.style.height = messHeight - this.getMessageHeaderHeight() + 'px';
	},
	
	resizeElementsWidth: function(width)
	{
		this.width = (width > this.min_width) ? width : this.min_width;
		this.list_table.style.width = this.width + 'px';
		this.list_div.style.width = this.width + 'px';
		this.list_container.style.width = this.width + 'px';
		this.list.style.width = this.width + 'px';
		this.subject.style.width = (this.width - 404) + 'px';
		
		this.mess_table.style.width = this.width + 'px';
		this.mess_container.style.width = this.width + 'px';
		this.mess_table.style.width = this.width + 'px';
		this.mess_body.style.width = this.width + 'px';
		this.mess_center.style.width = this.width + 'px';
		this.mess_center.style.top = '0px';
		this.mess_center.style.position = 'absolute';
	},
	
	resizeAttachmentWidth: function(width)
	{
		this.mess_attach.style.left = '0px';
		this.mess_attach.style.width = width + 'px';
		this.mess_center.style.width = this.mess_body.offsetWidth - width - this.mess_res.offsetWidth + 'px';
		this.mess_center.style.left = width + this.mess_res.offsetWidth + 'px';
		this.mess_res.style.left = width + 'px';
	}
}


/*
function CMessagePart(mode)
{
	this.isPreviewPane = mode;
	this.table = document.getElementById('message_td');
	this.container = document.getElementById('message_container');
	this.message_table = document.getElementById('message_table');
	this.header = document.getElementById('message_headers');
	this.body = document.getElementById('messageBody');
	
	this.width = 361;
	this.height = 361;
		
	//31 + infobar + border + inbox headers
	this.min_upper = 20 + 1;
	//2 borders + resizer + message headers + message
	this.min_lower = 2 + 4 + 45 + 100;
	
	this.min_height = 100;
	this.min_width = 500;

}

CMessagePart.prototype =
{
	resizeElementsHeight: function(height)
	{
		this.height = (height > this.min_height) ? height : this.min_height;
		this.table.style.height = this.height + 'px';
		this.container.style.height = this.height + 'px';
		this.body.style.height = (this.height - this.header.offsetHeight - 10) + 'px';
	},
	
	resizeElementsWidth: function(width)
	{
		this.width = (width > this.min_width) ? width : this.min_width;
		this.table.style.width = this.width + 'px';
		this.container.style.width = this.width + 'px';
		this.message_table.style.width = this.width + 'px';
		this.header.style.width = this.width + 'px';
		this.body.style.width = this.width + 'px';
	}
}
*/
