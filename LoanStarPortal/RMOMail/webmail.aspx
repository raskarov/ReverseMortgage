<%@ Register Src="Copyright.ascx" TagPrefix="BaseWebmail" TagName="Copyright" %>
<%@ Register Src="Logo.ascx" TagPrefix="BaseWebmail" TagName="Logo" %>

<%@ Page Language="c#" Codebehind="webmail.aspx.cs" AutoEventWireup="True" Inherits="WebMailPro.webmail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<html id="html">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Content-Script-Type" content="text/javascript" />
    <meta http-equiv="Pragma" content="cache" />
    <meta http-equiv="Cache-Control" content="public" />
    <title>
        <%=defaultTitle%>
    </title>
    <link rel="stylesheet" href="skins/<%=defaultSkin%>/styles.css" type="text/css" id="skin" />

<script type="text/javascript">
		function ResizeBodyHandler() {}
</script>

</head>
<body onresize="ResizeBodyHandler();" onload="Init();">
    <table class="wm_information" id="info_cont">
        <tr>
            <td class="wm_info_message" id="info_message">
                <%=_resMan.GetString("InfoWebMailLoading")%>
            </td>
        </tr>
    </table>
    <div id="spell_popup_menu" class="wm_hide">
    </div>
    <div align="center" id="content" class="wm_hide" >
        <BaseWebmail:Logo ID="Control_Logo" runat="server" ></BaseWebmail:Logo>
    </div>
    <BaseWebmail:Copyright ID="Control_Copyright" runat="server" ></BaseWebmail:Copyright>
    <%--<iframe name="session_saver" id="session_saver" src="session_saver.aspx" class="wm_hide">
    </iframe>--%>
</body>


<script type="text/javascript">
	var LoginUrl = 'default.aspx';
	var WebMailUrl = 'webmail.aspx';
	var BaseWebMailUrl = 'webmail.aspx';
	var ActionUrl = 'xml_processing.aspx';
	var EditAreaUrl = 'edit-area.aspx';
	var EmptyHtmlUrl = 'empty.html';
	var UploadUrl = 'upload.aspx';
	var ImportUrl = 'import.aspx';
	var HistoryStorageUrl = 'history-storage.aspx';
	var CheckMailUrl = 'check-mail.aspx';
	var LanguageUrl = 'langs_js.aspx';
	var DataHelpUrl = 'advanced_data_help.aspx';
	var SpellcheckerUrl = 'spellcheck.aspx';
	var CalendarUrl = 'calendar.aspx';
	var CalendarProcessingUrl = 'calendar/processing.aspx';
	var Title = '<%=jsClearDefaultTitle%>';
	var SkinName = '<%=jsClearDefaultSkin%>';
	var Start = <%=jsClearStart%>;
	var ToAddr = '<%=jsClearToAddr%>';
	var Browser;
	var WebMail, HistoryStorage;
</script>

<script type="text/javascript">
	var copy = document.getElementById('copyright');
	if (copy) copy.className = 'wm_hide';
</script>

<script type="text/javascript" src="langs_js.aspx?lang=<%=defaultLang%>"></script>

<script type="text/javascript" src="_defines.js"></script>

<script type="text/javascript" src="class.common.js"></script>

<script type="text/javascript" src="_functions.js"></script>

<script type="text/javascript" src="class.webmail.js"></script>

<script type="text/javascript" src="class.webmail-parts.js"></script>

<script type="text/javascript" src="class.html-editor.js"></script>

<script type="text/javascript" src="class.xml-parsers.js"></script>

<script type="text/javascript" src="class.screens-parts.js"></script>

<script type="text/javascript" src="screen.messages-list.js"></script>

<script type="text/javascript" src="screen.view-message.js"></script>

<script type="text/javascript" src="screen.messages-list-view.js"></script>

<script type="text/javascript" src="screen.new-message.js"></script>

<script type="text/javascript" src="class.variable-table.js"></script>

<!-- scripts for settings -->

<script type="text/javascript" src="screen.user-settings.js"></script>

<script type="text/javascript" src="screen.common-settings.js"></script>

<script type="text/javascript" src="screen.accounts-settings.js"></script>

<script type="text/javascript" src="screen.account-properties.js"></script>

<script type="text/javascript" src="calendar/inc.calendar-settings.js"></script>

<!-- scripts for contacts -->

<script type="text/javascript" src="screen.contacts.js"></script>

<script type="text/javascript" src="screen.view-contact.js"></script>

<script type="text/javascript">
	function Init() {
		Browser = new CBrowser();
		var DataTypes = [
			new CDataType(TYPE_SETTINGS_LIST, false, 0, false, { }, 'settings_list' ),
			new CDataType(TYPE_ACCOUNTS_LIST, false, 0, false, { }, 'accounts' ),
			new CDataType(TYPE_FOLDERS_LIST, false, 0, false, { IdAcct: 'id_acct', Sync: 'sync' }, 'folders_list' ),
			new CDataType(TYPE_MESSAGES_LIST, true, 5, false, { Page: 'page', SortField: 'sort_field', SortOrder: 'sort_order' }, 'messages' ),
			new CDataType(TYPE_MESSAGES_OPERATION, false, 0, false, { }, '' ),
			new CDataType(TYPE_MESSAGE, true, 10, true, { Id: 'id', Charset: 'charset' }, 'message' ),
			new CDataType(TYPE_USER_SETTINGS, false, 0, false, { }, 'settings' ),
			new CDataType(TYPE_ACCOUNT_PROPERTIES, false, 0, false, { IdAcct: 'id_acct' }, 'account' ),
			new CDataType(TYPE_FILTERS, false, 0, false, { IdAcct: 'id_acct' }, 'filters' ),
			new CDataType(TYPE_FILTER_PROPERTIES, false, 0, false, { IdFilter: 'id_filter', IdAcct: 'id_acct' }, 'filter' ),
			new CDataType(TYPE_X_SPAM, false, 0, false, { IdAcct: 'id_acct' }, 'x_spam' ),
			new CDataType(TYPE_CONTACTS_SETTINGS, false, 0, false, { }, 'contacts_settings' ),
			new CDataType(TYPE_SIGNATURE, false, 0, false, { IdAcct: 'id_acct' }, 'signature' ),
			new CDataType(TYPE_FOLDERS, false, 0, false, { IdAcct: 'id_acct' }, 'folders' ),
			new CDataType(TYPE_CONTACTS, false, 0, false, { Page: 'page', SortField: 'sort_field', SortOrder: 'sort_order' }, 'contacts_groups' ),
			new CDataType(TYPE_CONTACT, false, 0, false, { IdAddr: 'id_addr' }, 'contact' ),
			new CDataType(TYPE_GROUPS, false, 0, false, { }, 'groups' ),
			new CDataType(TYPE_GROUP, false, 0, false, { IdGroup: 'id_group' }, 'group' ),
			new CDataType(TYPE_SPELLCHECK, false, 0, false, { Word: 'word' }, 'spellcheck')
		];
		WebMail = new CWebMail(Title, SkinName);
		WebMail.DataSource = new CDataSource( DataTypes, ActionUrl, ErrorHandler, InfoHandler, LoadHandler, TakeDataHandler, ShowLoadingInfoHandler );
		HistoryStorage = new CHistoryStorage(
				{
					Document: document,
					HistoryStorageObjectName: "HistoryStorage",
					PathToPageInIframe: HistoryStorageUrl,
					MaxLimitSteps: 50,
					Browser: Browser
				}
			);
		if (Start)
		{
			WebMail.SetStartScreen(Start);
		}
		WebMail.DataSource.Get(TYPE_SETTINGS_LIST, { }, [], '');
	}
</script>

</html>
