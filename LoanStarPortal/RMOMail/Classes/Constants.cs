using System;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for Constants.
	/// </summary>
	public class Constants
	{
		protected Constants() {}

		//TODO: add constants to lang file-------
		public const string ntwStreamNull = "Network stream is null.";
		public const string wmServCmdNull = "WMServer command is null.";
		//---------------------------------------

		public static DateTime MinDate = new DateTime(1970, 1, 1);
		public static DateTime MinDateWithMaxZoneTimeOffset = new DateTime(1970, 1, 1, 23, 0, 0);

		public const string mailadmLogin = "mailadm";
		public const string appSettingsDataFolderPath = "dataFolderPath";
		public const string mailFolderName = "Mail";
		public const string tempFolderName = "Temp";
		public const string logFilename = "log.txt";
		public const string defaultSkinName = "Hotmail_Style";
		public const string nonChangedPassword = "77u/ZG9t";
		public const int PathMaxLength = 248;
		public const int DownloadChunk = 50;
		public const string defaultLang = "English";
        public const string webmailTab = "wm.tab";

		public const string sessionUserID = "AUserId";
		public const string sessionAccount = "account";
		public const string sessionTempFolder = "temp_folder";
		public const string sessionSettings = "webmail_settings";
		public const string sessionAccountEdit = "account_edit";
		public const string sessionReportText = "report_text";
		public const string sessionErrorText = "error_text";

		public const string mailAdmSaveSuccess = "Save successful!";
		public const string mailAdmSaveUnsuccess = "Save unsuccessful!";
		public const string mailAdmConnectSuccess = "Connect successful!";
		public const string mailAdmConnectUnsuccess = "Connect unsuccessful!";
		public const string mailAdmTablesExists = "Some tables exist. You must rename or remove it.";
		public const string mailAdmTablesCreated = "Tables created!";
		public const string mailAdmTablesNotCreated = "Creating table error!";
		public const string mailAdmLogClearSuccess = "Log clear successful!";
		public const string mailAdmLogClearUnsuccess = "Log clear error!";
		public const string mailAdmUpdateAccountSuccess = "Update successful!";
		public const string mailAdmUpdateAccountUnsuccess = "Update unsuccessful!";
		public const string mailAdmCreateAccountSuccess = "Create successful!";
		public const string mailAdmCreateAccountUnsuccess = "Create unsuccessful!";

		public struct DateFormats
		{
			public const string Default = "default";
			public const string DDMMYY = "dd/mm/yy";
			public const string MMDDYY = "mm/dd/yy";
			public const string DDMonth = "dd month";
		}

        public const string timeFormat = "|#";

		public struct SupportedLangs
		{
			public const string English = "English";
			public const string French = "French";
			public const string Catala = "Catala";
			public const string Espanyol = "Espanyol";
			public const string Nederlands = "Nederlands";
			public const string Swedish = "Swedish";
			public const string Turkish = "Turkish";
			public const string German = "German";
			public const string Portuguese = "Portuguese";
			public const string Italiano = "Italiano";
		}

		public struct FolderNames
		{
			public const string Inbox = "Inbox";
			public const string InboxLower = "inbox";
			public const string SentItems = "Sent Items";
			public const string SentItemsLower = "sent items";
			public const string Sent = "Sent";
			public const string SentLower = "sent";
			public const string Drafts = "Drafts";
			public const string DraftsLower = "drafts";
			public const string Trash = "Trash";
			public const string TrashLower = "trash";
		}

		public struct TablesNames
		{
            /* WebMail Tables */
            public const string a_users = "a_users";
			public const string awm_accounts = "awm_accounts";
			public const string awm_addr_book = "awm_addr_book";
			public const string awm_addr_groups = "awm_addr_groups";
			public const string awm_addr_groups_contacts = "awm_addr_groups_contacts";
			public const string awm_columns = "awm_columns";
			public const string awm_filters = "awm_filters";
			public const string awm_folders = "awm_folders";
			public const string awm_folders_tree = "awm_folders_tree";
			public const string awm_messages = "awm_messages";
			public const string awm_messages_body = "awm_messages_body";
			public const string awm_reads = "awm_reads";
			public const string awm_senders = "awm_senders";
			public const string awm_settings = "awm_settings";
			public const string awm_temp = "awm_temp";
			/* Calendar Tables */
            public const string acal_calendars = "acal_calendars";
            public const string acal_events = "acal_events";
            public const string acal_users_data = "acal_users_data";
			/* Other Tables */
			public const string awm_domains = "awm_domains";
		}

		public struct TablesIndexes
		{
			public const string awm_messages_index = "awm_messages_index";
			public const string awm_messages_body_index = "awm_messages_body_index";
		}

		public struct StaticScreenNames
		{
			public const string new_message = "new_message";
			public const string settings_common = "settings_common";
			public const string settings_accounts_properties = "settings_accounts_properties";
			public const string settings_accounts_filters = "settings_accounts_filters";
			public const string settings_accounts_signature = "settings_accounts_signature";
			public const string settings_accounts_folders = "settings_accounts_folders";
			public const string settings_contacts = "settings_contacts";
			public const string contacts= "contacts";
			public const string contacts_view = "contacts_view";
			public const string contacts_add = "contacts_add";
			public const string _default = "default";
			public const string mail_list = "mail_list";
			public const string message = "message";
		}

		public struct GroupOperationsRequests
		{
			public const string Delete = "delete";
			public const string Flag = "flag";
			public const string MarkAllRead = "mark_all_read";
			public const string MarkAllUnread = "mark_all_unread";
			public const string MarkRead = "mark_read";
			public const string MarkUnread = "mark_unread";
			public const string MoveToFolder = "move_to_folder";
			public const string Purge = "purge";
			public const string Undelete = "undelete";
			public const string Unflag = "unflag";
		}

		public static string[] Charsets = {
												   "CharsetDefault",
												   "CharsetArabicAlphabetISO",
												   "CharsetArabicAlphabet",
												   "CharsetBalticAlphabetISO",
												   "CharsetBalticAlphabet",
												   "CharsetCentralEuropeanAlphabetISO",
												   "CharsetCentralEuropeanAlphabet",
												   "CharsetChineseTraditional",
												   "CharsetCyrillicAlphabetISO",
												   "CharsetCyrillicAlphabetKOI8R",
												   "CharsetCyrillicAlphabet",
												   "CharsetGreekAlphabetISO",
												   "CharsetGreekAlphabet",
												   "CharsetHebrewAlphabetISO",
												   "CharsetHebrewAlphabet",
												   "CharsetJapanese",
												   "CharsetJapaneseShiftJIS",
												   "CharsetKoreanEUC",
												   "CharsetKoreanISO",
												   "CharsetLatin3AlphabetISO",
												   "CharsetTurkishAlphabet",
												   "CharsetUniversalAlphabetUTF7",
												   "CharsetUniversalAlphabetUTF8",
												   "CharsetVietnameseAlphabet",
												   "CharsetWesternAlphabetISO",
												   "CharsetWesternAlphabet"
											   };

		public static string[] PageName = {
											  "-1",
											  "iso-8859-6",
											  "windows-1256",
											  "iso-8859-4",
											  "windows-1257",
											  "iso-8859-2",
											  "windows-1250",
											  "big5",
											  "iso-8859-5",
											  "koi8-r",
											  "windows-1251",
											  "iso-8859-7",
											  "windows-1253",
											  "iso-8859-8",
											  "windows-1255",
											  "iso-2022-jp",
											  "shift-jis",
											  "euc-kr",
											  "iso-2022-kr",
											  "iso-8859-3",
											  "windows-1254",
											  "utf-7",
											  "utf-8",
											  "windows-1258",
											  "iso-8859-1",
											  "windows-1252"
										  };

		public static int[] PageNumber = {
											  -1,
											  28596,
											  1256,
											  28594,
											  1257,
											  28592,
											  1250,
											  950,
											  28595,
											  20866,
											  1251,
											  28597,
											  1253,
											  28598,
											  1255,
											  50220,
											  932,
											  946,
											  50225,
											  28593,
											  1254,
											  65000,
											  65001,
											  1258,
											  28591,
											  1252
										  };

        public struct WebmailTabKeys
        {
            public const string ControlPort = "CtrlPort";
            public const string SmtpPort = "SmtpPort";
            public const string Login = "Login";
            public const string Pass = "Password";
        }

		}
	}

