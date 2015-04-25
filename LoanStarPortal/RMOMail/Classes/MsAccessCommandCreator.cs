using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Text;
using MailBee.ImapMail;
using WebMailPro;

namespace WebMailPro
{
	public class MsAccessCommandCreator : CommandCreator
	{
		public MsAccessCommandCreator(OleDbConnection connection, OleDbCommand command) : base(connection, command){}

		protected override IDataParameter CreateParameter(string paramName, object paramValue)
		{
			Log.WriteLine("CreateParameter", string.Format("{0}='{1}'", paramName, paramValue));
			if (paramValue.GetType() == typeof(DateTime))
			{
				paramValue = ((DateTime)paramValue).ToString();
			}
			return new OleDbParameter(paramName, paramValue);
		}

		public override string SelectIdentity()
		{
			return @"SELECT @@IDENTITY;";
		}

        public override IDbCommand CreateTable(string name, string prefix)
        {
            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
            prefix = EncodeQuotes(prefix);

            string commandText = string.Empty;
            switch (name)
            {
                #region CreateCalendarTablesCommands
                case Constants.TablesNames.acal_calendars:
                    commandText = string.Format(@"
CREATE TABLE [{0}{1}] (
    [calendar_id] AUTOINCREMENT,
    [user_id] int NOT NULL default 0,
    [calendar_name] varchar (100) NOT NULL,
    [calendar_description] varchar default NULL,
    [calendar_color] int NOT NULL default 0,
    [calendar_active] tinyint NOT NULL default 0,
PRIMARY KEY  ([calendar_id]))", prefix, Constants.TablesNames.acal_calendars);

                              break;
                case Constants.TablesNames.acal_events:
                    commandText = string.Format(@"
CREATE TABLE [{0}{1}] (
    [event_id] AUTOINCREMENT,
    [calendar_id] int NOT NULL default 0,
    [event_timefrom] datetime default NULL,
    [event_timetill] datetime default NULL, 
    [event_allday] tinyint NOT NULL default 0,
    [event_name] varchar (100) NOT NULL,
    [event_text] varchar default NULL,
    [event_priority] tinyint NULL DEFAULT 0,
PRIMARY KEY  ([event_id]))", prefix, Constants.TablesNames.acal_events);
                    break;
                case Constants.TablesNames.acal_users_data:
                    commandText = string.Format(@"
CREATE TABLE [{0}{1}] (
    [settings_id] AUTOINCREMENT,
    [user_id] int NOT NULL default 0,
    [timeformat] tinyint NOT NULL default 1,
    [dateformat] tinyint NOT NULL default 1,
    [showweekends] tinyint NOT NULL default 0,
    [workdaystarts] tinyint NOT NULL default 0,
    [workdayends] tinyint NOT NULL default 1,
    [showworkday] tinyint NOT NULL default 0,
    [weekstartson] tinyint NOT NULL default 0,
    [defaulttab] tinyint NOT NULL default 1,
    [country] varchar (2) default NULL,
    [timezone] smallint NULL,
    [alltimezones] tinyint NOT NULL default 0,
PRIMARY KEY ([settings_id]))", prefix, Constants.TablesNames.acal_users_data);
                    break;
                #endregion
            }

            return PrepareCommand(commandText, null);
        }
        
        public override IDbCommand SelectAwmAccountsForAdmin(int page, int pageSize, string orderBy, bool asc, string searchCondition)
		{
			if (page > 1)
			{
				return base.SelectAwmAccountsForAdmin (page, pageSize, orderBy, asc, searchCondition);
			}
			string whereCondition = string.Empty;
			if ((searchCondition != null) && (searchCondition.Length > 0))
			{
				whereCondition = string.Format(@"WHERE [email] LIKE ('*{0}*') OR [last_login] LIKE ('*{0}*') OR [logins_count] LIKE ('*{0}*') OR [mail_inc_host] LIKE ('*{0}*') OR [mail_out_host] LIKE ('*{0}*')", EncodeQuotes(searchCondition));
			}

			string commandText = string.Format(@"SELECT TOP {2} [{0}].[id_user], [id_acct], [email], [last_login], [logins_count], [mail_inc_host], [mail_out_host], [mailbox_size], [mailbox_limit] FROM [{0}]
INNER JOIN [{1}] ON [{0}].[id_user] = [{1}].[id_user] {5}
ORDER BY [{3}] {4};",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_settings),
				pageSize,
				orderBy,
				(asc) ? "ASC" : "DESC",
				whereCondition);

			return PrepareCommand(commandText, null);
		}

		public override IDbCommand SelectAwmMessages(int id_acct, long id_folder_db, int pageNumber, int msgsOnPage, string order, bool asc)
		{
			if (pageNumber > 1)
			{
				return base.SelectAwmMessages (id_acct, id_folder_db, pageNumber, msgsOnPage, order, asc);
			}

			string commandText = string.Format(@"SELECT TOP {1} * FROM {0}
WHERE [id_acct]=@id_acct AND [id_folder_db]=@id_folder_db 
ORDER BY {2} {3};",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				msgsOnPage, // 1
				order,
				(asc) ? "ASC" : "DESC");

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmColumns(int id_column, int id_user, int value) {
			string commandText = string.Format(@"INSERT INTO {0} (id_user, id_column, column_value)
VALUES (?, ?, ?);",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_columns));


			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@id_column", id_column));
			parameters.Add(CreateParameter("@value", value));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmMessages(int id_acct, int pageNumber, int msgsOnPage, string condition, FolderCollection folders, bool inHeadersOnly, string order, bool asc)
		{
			if (pageNumber > 1)
			{
				return base.SelectAwmMessages (id_acct, pageNumber, msgsOnPage, condition, folders, inHeadersOnly, order, asc);
			}
			StringBuilder folder_ids = new StringBuilder();
			if (folders != null)
			{
				foreach (Folder fld in folders)
				{
					folder_ids.AppendFormat("{0},", fld.ID);
				}
				if (folder_ids.Length > 0)
				{
					folder_ids.Remove(folder_ids.Length - 1, 1);
				}
			}
			string bodyLike = string.Empty;
			if (!inHeadersOnly)
			{
				bodyLike = string.Format(" OR [body_text] LIKE '%{0}%'", condition);
			}

			string commandText = string.Format(@"SELECT TOP {1} * FROM [{0}] INNER JOIN [{6}] ON [{0}].id_folder_db = [{6}].id_folder 
WHERE [{0}].[id_acct]=@id_acct AND [id_folder_db] IN ({4}) AND 
([from_msg] LIKE '%{5}%' OR [to_msg] LIKE '%{5}%' OR [cc_msg] LIKE '%{5}%' OR [bcc_msg]
LIKE '%{5}%' OR [subject] LIKE '%{5}%'{7})
ORDER BY {2} {3};",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				msgsOnPage,
				order,
				(asc) ? "ASC" : "DESC",
				folder_ids,
				EncodeQuotes(condition),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),
				bodyLike);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmFolders(int id_acct)
		{
			string commandText = string.Format(@"DELETE FROM {0} WHERE [id_acct]=@id_acct;", 
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmFolders(int id_acct, long id_folder)
		{
			string commandText = string.Format(@"
DELETE FROM {0} WHERE [id_folder]=@id_folder AND [id_acct]=@id_acct
",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders));


			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAUsers(bool deleted)
		{
			string commandText = string.Format(@"INSERT INTO {0} (deleted) VALUES(@deleted);",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.a_users));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@deleted", deleted));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmFilters(int id_acct, byte field, byte condition, string filter, byte action, long id_folder)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_acct, [field], [condition], [filter], [action], id_folder)
VALUES (@id_acct, @field, @condition, @filter, @action, @id_folder)",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_filters));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@field", field));
			parameters.Add(CreateParameter("@condition", condition));
			parameters.Add(CreateParameter("@filter", (filter.Length > 255) ? filter.Substring(0, 255) : filter));
			parameters.Add(CreateParameter("@action", action));
			parameters.Add(CreateParameter("@id_folder", id_folder));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmAccounts(int id_user, bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol, string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host, string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm, bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode, short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt, string delimiter, long mailbox_size)
		{
			string commandText = string.Format(@"INSERT INTO {0}
(id_user, def_acct, deleted, email, mail_protocol, mail_inc_host, mail_inc_login, mail_inc_pass,
 mail_inc_port, mail_out_host, mail_out_login, mail_out_pass, mail_out_port, mail_out_auth, friendly_nm,
 use_friendly_nm, def_order, getmail_at_login, mail_mode, mails_on_server_days, signature, signature_type,
 signature_opt, delimiter, mailbox_size)
VALUES
(@id_user, @def_acct, @deleted, @email, @mail_protocol, @mail_inc_host, @mail_inc_login, @mail_inc_pass, 
@mail_inc_port, @mail_out_host, @mail_out_login, @mail_out_pass, @mail_out_port, @mail_out_auth, @friendly_nm,
@use_friendly_nm, @def_order, @getmail_at_login, @mail_mode, @mails_on_server_days, @signature, @signature_type,
@signature_opt, @delimiter, @mailbox_size);",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@def_acct", def_acct));
			parameters.Add(CreateParameter("@deleted", deleted));
			parameters.Add(CreateParameter("@email", (email.Length > 255) ? email.Substring(0, 255) : email));
			parameters.Add(CreateParameter("@mail_protocol", mail_protocol));
			parameters.Add(CreateParameter("@mail_inc_host", (mail_inc_host.Length > 255) ? mail_inc_host.Substring(0, 255) : mail_inc_host));
			parameters.Add(CreateParameter("@mail_inc_login", (mail_inc_login.Length > 255) ? mail_inc_login.Substring(0, 255) : mail_inc_login));
			parameters.Add(CreateParameter("@mail_inc_pass", (mail_inc_pass.Length > 255) ? mail_inc_pass.Substring(0, 255) : mail_inc_pass));
			parameters.Add(CreateParameter("@mail_inc_port", mail_inc_port));
			parameters.Add(CreateParameter("@mail_out_host", (mail_out_host.Length > 255) ? mail_out_host.Substring(0, 255) : mail_out_host));
			parameters.Add(CreateParameter("@mail_out_login", (mail_out_login.Length > 255) ? mail_out_login.Substring(0, 255) : mail_out_login));
			parameters.Add(CreateParameter("@mail_out_pass", (mail_out_pass.Length > 255) ? mail_out_pass.Substring(0, 255) : mail_out_pass));
			parameters.Add(CreateParameter("@mail_out_port", mail_out_port));
			parameters.Add(CreateParameter("@mail_out_auth", mail_out_auth));
			parameters.Add(CreateParameter("@friendly_nm", (friendly_nm.Length > 255) ? friendly_nm.Substring(0, 255) : friendly_nm));
			parameters.Add(CreateParameter("@use_friendly_nm", use_friendly_nm));
			parameters.Add(CreateParameter("@def_order", def_order));
			parameters.Add(CreateParameter("@getmail_at_login", getmail_at_login));
			parameters.Add(CreateParameter("@mail_mode", mail_mode));
			parameters.Add(CreateParameter("@mails_on_server_days", mails_on_server_days));
			parameters.Add(CreateParameter("@signature", signature));
			parameters.Add(CreateParameter("@signature_type", signature_type));
			parameters.Add(CreateParameter("@signature_opt", signature_opt));
			parameters.Add(CreateParameter("@delimiter", delimiter));
			parameters.Add(CreateParameter("@mailbox_size", mailbox_size));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmAddrBook(int id_user, string h_email, string fullname, string notes, bool use_friendly_nm, string h_street, string h_city, string h_state, string h_zip, string h_country, string h_phone, string h_fax, string h_mobile, string h_web, string b_email, string b_company, string b_street, string b_city, string b_state, string b_zip, string b_country, string b_job_title, string b_department, string b_office, string b_phone, string b_fax, string b_web, byte birthday_day, byte birthday_month, short birthday_year, string other_email, short primary_email, long id_addr_prev, bool tmp, int use_frequency, bool auto_create)
		{
			string commandText = string.Format(@"INSERT INTO [{0}]([id_user], [h_email], [fullname], [notes],
 [use_friendly_nm], [h_street], [h_city], [h_state], [h_zip], [h_country], [h_phone], [h_fax], [h_mobile],
 [h_web], [b_email], [b_company], [b_street], [b_city], [b_state], [b_zip], [b_country], [b_job_title],
 [b_department], [b_office], [b_phone], [b_fax], [b_web], [birthday_day], [birthday_month], [birthday_year],
 [other_email], [primary_email], [id_addr_prev], [tmp], [use_frequency], [auto_create])
VALUES(@id_user, @h_email, @fullname, @notes,
 @use_friendly_nm, @h_street, @h_city, @h_state, @h_zip, @h_country, @h_phone, @h_fax, @h_mobile,
 @h_web, @b_email, @b_company, @b_street, @b_city, @b_state, @b_zip, @b_country, @b_job_title,
 @b_department, @b_office, @b_phone, @b_fax, @b_web, @birthday_day, @birthday_month, @birthday_year,
 @other_email, @primary_email, @id_addr_prev, @tmp, @use_frequency, @auto_create);",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@h_email", (h_email.Length > 255) ? h_email.Substring(0, 255) : h_email));
			parameters.Add(CreateParameter("@fullname", (fullname.Length > 255) ? fullname.Substring(0, 255) : fullname));
			parameters.Add(CreateParameter("@notes", (notes.Length > 255) ? notes.Substring(0, 255) : notes));
			parameters.Add(CreateParameter("@use_friendly_nm", use_friendly_nm));
			parameters.Add(CreateParameter("@h_street", (h_street.Length > 255) ? h_street.Substring(0, 255) : h_street));
			parameters.Add(CreateParameter("@h_city", (h_city.Length > 200) ? h_city.Substring(0, 200) : h_city));
			parameters.Add(CreateParameter("@h_state", (h_state.Length > 200) ? h_state.Substring(0, 200) : h_state));
			parameters.Add(CreateParameter("@h_zip", (h_zip.Length > 10) ? h_zip.Substring(0, 10) : h_zip));
			parameters.Add(CreateParameter("@h_country", (h_country.Length > 200) ? h_country.Substring(0, 200) : h_country));
			parameters.Add(CreateParameter("@h_phone", (h_phone.Length > 50) ? h_phone.Substring(0, 50) : h_phone));
			parameters.Add(CreateParameter("@h_fax", (h_fax.Length > 50) ? h_fax.Substring(0, 50) : h_fax));
			parameters.Add(CreateParameter("@h_mobile", (h_mobile.Length > 50) ? h_mobile.Substring(0, 50) : h_mobile));
			parameters.Add(CreateParameter("@h_web", (h_web.Length > 255) ? h_web.Substring(0, 255): h_web));
			parameters.Add(CreateParameter("@b_email", (b_email.Length > 255) ? b_email.Substring(0, 255) : b_email));
			parameters.Add(CreateParameter("@b_company", (b_company.Length > 200) ? b_company.Substring(0, 200) : b_company));
			parameters.Add(CreateParameter("@b_street", (b_street.Length > 255) ? b_street.Substring(0, 255) : b_street));
			parameters.Add(CreateParameter("@b_city", (b_city.Length > 200) ? b_city.Substring(0, 200) : b_city));
			parameters.Add(CreateParameter("@b_state", (b_state.Length > 200) ? b_state.Substring(0, 255) : b_state));
			parameters.Add(CreateParameter("@b_zip", (b_zip.Length > 10) ? b_zip.Substring(0, 10) : b_zip));
			parameters.Add(CreateParameter("@b_country", (b_country.Length > 200) ? b_country.Substring(0, 200) : b_country));
			parameters.Add(CreateParameter("@b_job_title", (b_job_title.Length > 100) ? b_job_title.Substring(0, 100) : b_job_title));
			parameters.Add(CreateParameter("@b_department", (b_department.Length > 200) ? b_department.Substring(0, 200) : b_department));
			parameters.Add(CreateParameter("@b_office", (b_office.Length > 200) ? b_office.Substring(0, 200) : b_office));
			parameters.Add(CreateParameter("@b_phone", (b_phone.Length > 50) ? b_phone.Substring(0, 50) : b_phone));
			parameters.Add(CreateParameter("@b_fax", (b_fax.Length > 50) ? b_fax.Substring(0, 50) : b_fax));
			parameters.Add(CreateParameter("@b_web", (b_web.Length > 255) ? b_web.Substring(0, 255) : b_web));
			parameters.Add(CreateParameter("@birthday_day", birthday_day));
			parameters.Add(CreateParameter("@birthday_month", birthday_month));
			parameters.Add(CreateParameter("@birthday_year", birthday_year));
			parameters.Add(CreateParameter("@other_email", (other_email.Length > 255) ? other_email.Substring(0, 255) : other_email));
			parameters.Add(CreateParameter("@primary_email", primary_email));
			parameters.Add(CreateParameter("@id_addr_prev", id_addr_prev));
			parameters.Add(CreateParameter("@tmp", tmp));
			parameters.Add(CreateParameter("@use_frequency", use_frequency));
			parameters.Add(CreateParameter("@auto_create", auto_create));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmAddrGroups(int id_user, string group_nm, string phone, string fax, string web, bool organization, int use_frequency, string email, string company, string street, string city, string state, string zip, string country)
		{
			string commandText = string.Format(@"INSERT INTO {0}([id_user], [group_nm], [phone], [fax], [web], [organization], [use_frequency], [email], [company], [street], [city], [state], [zip], [country])
VALUES(@id_user, @group_nm, @phone, @fax, @web, @organization, @use_frequency, @email, @company, @street, @city, @state, @zip, @country);
",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@group_nm", (group_nm.Length > 255) ? group_nm.Substring(0, 255) : group_nm));
			parameters.Add(CreateParameter("@phone", (phone.Length > 50) ? phone.Substring(0, 50) : phone));
			parameters.Add(CreateParameter("@fax", (fax.Length > 50) ? fax.Substring(0, 50) : fax));
			parameters.Add(CreateParameter("@web", (web.Length > 255) ? web.Substring(0, 255) : web));
			parameters.Add(CreateParameter("@organization", organization));
			parameters.Add(CreateParameter("@use_frequency", use_frequency));
			parameters.Add(CreateParameter("@email", (email.Length > 255) ? email.Substring(0, 255) : email));
			parameters.Add(CreateParameter("@company", (company.Length > 200) ? company.Substring(0, 200) : company));
			parameters.Add(CreateParameter("@street", (street.Length > 255) ? street.Substring(0, 255) : street));
			parameters.Add(CreateParameter("@city", (city.Length > 200) ? city.Substring(0, 200) : city));
			parameters.Add(CreateParameter("@state", (state.Length > 200) ? state.Substring(0, 200) : state));
			parameters.Add(CreateParameter("@zip", (zip.Length > 10) ? zip.Substring(0, 10) : zip));
			parameters.Add(CreateParameter("@country", (country.Length > 200) ? country.Substring(0, 200) : country));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmFolders(int id_acct, long id_parent, FolderType type, string name, string full_path, FolderSyncType sync_type, bool hide, short fld_order)
		{
			string commandText = string.Format(@"
INSERT INTO {0} ([id_acct] ,[id_parent] ,[type] ,[name] ,[full_path] ,[sync_type] ,[hide] ,[fld_order])
	VALUES (@id_acct, @id_parent, @type, @name, @full_path, @sync_type, @hide, @fld_order);",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_parent", id_parent));
			parameters.Add(CreateParameter("@type", type));
			parameters.Add(CreateParameter("@name", (name.Length > 100) ? name.Substring(0, 100) : name));
			parameters.Add(CreateParameter("@full_path", (full_path.Length > 255) ? full_path.Substring(0, 255) : full_path));
			parameters.Add(CreateParameter("@sync_type", sync_type));
			parameters.Add(CreateParameter("@hide", hide));
			parameters.Add(CreateParameter("@fld_order", fld_order));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmSettings(int id_user, short msgs_per_page, bool white_listing, bool x_spam, DateTime last_login, int logins_count, string def_skin, string def_lang, int def_charset_inc, short def_timezone, string def_date_fmt, bool hide_folders, long mailbox_limit, bool allow_change_settings, bool allow_dhtml_editor, bool allow_direct_mode, bool hide_contacts, int db_charset, short horiz_resizer, short vert_resizer, byte mark, byte reply, short contacts_per_page, int def_charset_out, byte view_mode)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_user, msgs_per_page, white_listing, x_spam, 
 last_login, logins_count, def_skin, def_lang, def_charset_inc, def_charset_out, def_timezone, def_date_fmt,
 hide_folders, mailbox_limit, allow_change_settings, allow_dhtml_editor, allow_direct_mode, hide_contacts, 
db_charset, horiz_resizer, vert_resizer, mark, reply, contacts_per_page, view_mode)
VALUES(@id_user, @msgs_per_page, @white_listing, @x_spam,
 @last_login, @logins_count, @def_skin, @def_lang, @def_charset_inc, @def_charset_out, @def_timezone, @def_date_fmt,
 @hide_folders, @mailbox_limit, @allow_change_settings, @allow_dhtml_editor, @allow_direct_mode, @hide_contacts,
 @db_charset, @horiz_resizer, @vert_resizer, @mark, @reply, @contacts_per_page, @view_mode);",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_settings));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@msgs_per_page", msgs_per_page));
			parameters.Add(CreateParameter("@white_listing", white_listing));
			parameters.Add(CreateParameter("@x_spam", x_spam));
			parameters.Add(CreateParameter("@last_login", last_login));
			parameters.Add(CreateParameter("@logins_count", logins_count));
			parameters.Add(CreateParameter("@def_skin", (def_skin.Length > 255) ? def_skin.Substring(0, 255) : def_skin));
			parameters.Add(CreateParameter("@def_lang", (def_lang.Length > 50) ? def_lang.Substring(0, 50) : def_lang));
			parameters.Add(CreateParameter("@def_charset_inc", def_charset_inc));
			parameters.Add(CreateParameter("@def_charset_out", def_charset_out));
			parameters.Add(CreateParameter("@def_timezone", def_timezone));
			parameters.Add(CreateParameter("@def_date_fmt", (def_date_fmt.Length > 20) ? def_date_fmt.Substring(0, 20) : def_date_fmt));
			parameters.Add(CreateParameter("@hide_folders", hide_folders));
			parameters.Add(CreateParameter("@mailbox_limit", mailbox_limit));
			parameters.Add(CreateParameter("@allow_change_settings", allow_change_settings));
			parameters.Add(CreateParameter("@allow_dhtml_editor", allow_dhtml_editor));
			parameters.Add(CreateParameter("@allow_direct_mode", allow_direct_mode));
			parameters.Add(CreateParameter("@hide_contacts", hide_contacts));
			parameters.Add(CreateParameter("@db_charset", db_charset));
			parameters.Add(CreateParameter("@horiz_resizer", horiz_resizer));
			parameters.Add(CreateParameter("@vert_resizer", vert_resizer));
			parameters.Add(CreateParameter("@mark", mark));
			parameters.Add(CreateParameter("@reply", reply));
			parameters.Add(CreateParameter("@contacts_per_page", contacts_per_page));
			parameters.Add(CreateParameter("@view_mode", view_mode));

			return PrepareCommand(commandText, parameters);
		}

        public override IDbCommand InsertIntoAwmTemp(int id_acct, string data_val)
        {
            string commandText = string.Format(@"INSERT INTO {0}(id_acct, data_val)
VALUES(?, ?)",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_temp));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@id_acct", id_acct));
            parameters.Add(CreateParameter("@data_val", data_val));

            return PrepareCommand(commandText, parameters);
        }

		public override IDbCommand UpdateAwmMessagesFlags(int id_acct, long id_folder_db, bool allMessages, int[] ids, SystemMessageFlags flags, MessageFlagAction flagsAction)
		{
			string flagsField = string.Empty;
			ArrayList flagsStrs = new ArrayList();
			string uidsStr = string.Empty;
			if (!allMessages)
			{
				string strIn = NumberArrayToString(ids);
				uidsStr = string.Format(" AND [id_msg] IN ({0})", strIn);
			}

			switch (flagsAction)
			{
				case MessageFlagAction.Add:
				{
					if ((flags & SystemMessageFlags.Seen) > 0) flagsStrs.Add("[seen]=1");
					if ((flags & SystemMessageFlags.Flagged) > 0) flagsStrs.Add("[flagged]=1");
					if ((flags & SystemMessageFlags.Deleted) > 0) flagsStrs.Add("[deleted]=1");
					if ((flags & SystemMessageFlags.Answered) > 0) flagsStrs.Add("[replied]=1");
					flagsField = "([flags] BOR @flags)";
					break;
				}
				case MessageFlagAction.Remove:
				{
					if ((flags & SystemMessageFlags.Seen) > 0) flagsStrs.Add("[seen]=0");
					if ((flags & SystemMessageFlags.Flagged) > 0) flagsStrs.Add("[flagged]=0");
					if ((flags & SystemMessageFlags.Deleted) > 0) flagsStrs.Add("[deleted]=0");
					if ((flags & SystemMessageFlags.Answered) > 0) flagsStrs.Add("[replied]=0");
					flagsField = "([flags] BAND BNOT(@flags))";
					break;
				}
				case MessageFlagAction.Replace:
				{
					flagsStrs.Add(string.Format("[seen]={0}", ((flags & SystemMessageFlags.Seen) > 0) ? "1" : "0"));
					flagsStrs.Add(string.Format("[flagged]={0}", ((flags & SystemMessageFlags.Flagged) > 0) ? "1" : "0"));
					flagsStrs.Add(string.Format("[deleted]={0}", ((flags & SystemMessageFlags.Deleted) > 0) ? "1" : "0"));
					flagsStrs.Add(string.Format("[replied]={0}", ((flags & SystemMessageFlags.Answered) > 0) ? "1" : "0"));
					flagsField = "@flags";
					break;
				}
			}

			string commandText = string.Format(@"UPDATE {0}
SET {2},[flags]={3}
WHERE [id_acct]=@id_acct AND [id_folder_db]=@id_folder_db {1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				uidsStr, // 1
				string.Join(",", (string[])flagsStrs.ToArray(typeof(string))), // 2
				flagsField); // 3

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@flags", (int)flags));
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand UpdateAwmFilters(int id_filter, int id_acct, byte field, byte condition, string filter, byte action, long id_folder)
		{
			string commandText = string.Format(@"UPDATE {0}
SET [id_acct]=@id_acct, [field]=@field, [condition]=@condition, [filter]=@filter, [action]=@action,
 [id_folder]=@id_folder
WHERE [id_filter]=@id_filter
",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_filters)); // 7

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@field", field));
			parameters.Add(CreateParameter("@condition", condition));
			parameters.Add(CreateParameter("@filter", (filter.Length > 255) ? filter.Substring(0, 255) : filter));
			parameters.Add(CreateParameter("@action", action));
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@id_filter", id_filter));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmMessagesOlderThanXDays(int id_acct, long id_folder, int daysCount)
		{
			string commandText = string.Format(@"SELECT * FROM [{0}]
WHERE [id_acct]=@id_acct AND [id_folder_db]=@id_folder_db AND DATEDIFF('d', msg_date, DATE()) > {1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				daysCount.ToString(CultureInfo.InvariantCulture));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmAddrBookGroups(int id_user, int contactsOnPage, int page, int sort_field, int sort_order)
		{
            string filter = "email";
			switch (sort_field)
			{
				case 0:
					filter = "is_group";
					break;
				case 1:
					filter = "name";
					break;
				default:
				case 2:
					filter = "email";
					break;
				case 3:
					filter = "use_frequency";
					break;
			}

			string commandText = string.Format(@"SELECT TOP {2} * FROM (SELECT use_frequency, id_addr AS [uniq_id], id_addr AS [id], fullname AS [name],
Choose ([primary_email] + 1, [h_email], [b_email], [other_email]) AS email, 0 AS is_group FROM {0}
WHERE id_user=@id_user
UNION
SELECT use_frequency, -id_group AS [uniq_id], id_group AS [id], group_nm AS [name], '' AS email, 1 AS is_group FROM {1}
WHERE id_user=@id_user ORDER BY {3} {4}) AS union_table
ORDER BY {3} {4}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),
				contactsOnPage,
				filter,
				(sort_order == 0) ? "ASC" : "DESC");

			if (page > 1)
			{
				commandText = string.Format(@"SELECT TOP {2} * FROM (SELECT id_addr AS [uniq_id], id_addr AS [id], fullname AS [name],
Choose ([primary_email] + 1, [h_email], [b_email], [other_email]) AS email, 0 AS is_group FROM {0}
WHERE id_user=@id_user
UNION
SELECT -id_group AS [uniq_id], id_group AS [id], group_nm AS [name], '' AS email, 1 AS is_group FROM {1}
WHERE id_user=@id_user) as union_table 
WHERE [uniq_id] NOT IN (SELECT TOP {3} [uniq_id] FROM (SELECT id_addr AS [uniq_id], id_addr AS [id], fullname AS [name],
Choose ([primary_email], [h_email], [b_email], [other_email]) AS email, 0 AS is_group FROM {0}
WHERE id_user=@id_user
UNION
SELECT -id_group AS [uniq_id], id_group AS [id], group_nm AS [name], '' AS email, 1 AS is_group FROM {1}
WHERE id_user=@id_user) as union_table2
ORDER BY [{4}] {5})
ORDER BY [{4}] {5}",
					EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
					EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),
					contactsOnPage,
					(page > 0) ? (page - 1) * contactsOnPage : 0,
					filter,
					(sort_order == 0) ? "ASC" : "DESC");
			}

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SearchAwmAddrBookGroups(int id_user, int contactsOnPage, int page, string sort_field, int sort_order, int id_group, string look_for, int look_for_type)
		{
			string lookForSearchCondition = string.Empty;
			if ((look_for != null) && (look_for.Length > 0))
			{
				lookForSearchCondition = EscapeWildcardCharacters(EncodeQuotes(look_for));
			}

			string groupSearchCondition = string.Empty;
			if (id_group >= 0)
			{
				groupSearchCondition = string.Format(@"(SELECT [{0}].* FROM {0} INNER JOIN {1} ON {0}.id_addr={1}.id_addr WHERE {1}.id_group={2}) AS table_join",
					EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
					EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts),
					id_group);
			}
			else
			{
				groupSearchCondition = EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book);
			}

			string commandText = string.Format(@"SELECT TOP {2} * FROM (SELECT id_addr AS [uniq_id], id_addr AS [id], fullname AS [name],
Choose ([primary_email] + 1, [h_email], [b_email], [other_email]) AS email, 0 AS is_group, use_frequency AS frequency FROM {6}
WHERE id_user=@id_user AND (fullname LIKE '{5}' OR h_email LIKE '{5}' OR b_email LIKE '{5}' OR other_email LIKE '{5}')
UNION
SELECT -id_group AS [uniq_id], id_group AS [id], group_nm AS [name], '' AS email, 1 AS is_group, use_frequency AS frequency FROM {1}
WHERE id_user=@id_user AND (group_nm LIKE '{5}' OR email LIKE '{5}') ORDER BY {3} {4}) AS union_table
ORDER BY {3} {4}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),
				contactsOnPage,
				sort_field,
				(sort_order == 0) ? "ASC" : "DESC",
				(look_for_type == 0) ? "%" + lookForSearchCondition + "%" : lookForSearchCondition + "%",
				groupSearchCondition);

			if (page > 1)
			{
				commandText = string.Format(@"SELECT TOP {2} * FROM (SELECT id_addr AS [uniq_id], id_addr AS [id], fullname AS [name],
Choose ([primary_email] + 1, [h_email], [b_email], [other_email]) AS email, 0 AS is_group, use_frequency AS frequency FROM {7}
WHERE id_user=@id_user AND (fullname LIKE '{6}' OR h_email LIKE '{6}' OR b_email LIKE '{6}' OR other_email LIKE '{6}')
UNION
SELECT -id_group AS [uniq_id], id_group AS [id], group_nm AS [name], '' AS email, 1 AS is_group, use_frequency AS frequency FROM {1}
WHERE id_user=@id_user AND (group_nm LIKE '{6}' OR email LIKE '{6}')) as union_table 
WHERE [uniq_id] NOT IN (SELECT TOP {3} [uniq_id] FROM (SELECT id_addr AS [uniq_id], id_addr AS [id], fullname AS [name],
Choose ([primary_email], [h_email], [b_email], [other_email]) AS email, 0 AS is_group, use_frequency AS frequency FROM {7}
WHERE id_user=@id_user AND (fullname LIKE '{6}' OR h_email LIKE '{6}' OR b_email LIKE '{6}' OR other_email LIKE '{6}')
UNION
SELECT -id_group AS [uniq_id], id_group AS [id], group_nm AS [name], '' AS email, 1 AS is_group, use_frequency AS frequency FROM {1}
WHERE id_user=@id_user AND (group_nm LIKE '{6}' OR email LIKE '{6}')) as union_table2
ORDER BY [{4}] {5})
ORDER BY [{4}] {5}",
					EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
					EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),
					contactsOnPage,
					(page > 0) ? (page - 1) * contactsOnPage : 0,
					sort_field,
					(sort_order == 0) ? "ASC" : "DESC",
					(look_for_type == 0) ? "%" + lookForSearchCondition + "%" : lookForSearchCondition + "%",
					groupSearchCondition);
			}

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			//parameters.Add(CreateParameter("@id_group", id_group));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmMessagesMarkAsDelete(int id_acct, long id_folder)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
WHERE id_acct=@id_acct AND id_folder_db=@id_folder AND deleted=-1",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder", id_folder));

			return PrepareCommand(commandText, parameters);
		}
	}

}
