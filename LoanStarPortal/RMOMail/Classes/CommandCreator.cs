using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using MailBee.ImapMail;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for CommandCreator.
	/// </summary>
	public abstract class CommandCreator
	{
		protected IDbConnection _connection = null;
		protected IDbCommand _command = null;
		protected WebmailSettings _settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
		protected string _nolock = string.Empty;

		public CommandCreator(IDbConnection connection, IDbCommand command)
		{
			_connection = connection;
			_command = command;
		}

		public CommandCreator(IDbConnection connection, IDbCommand command, bool nolock)
		{
			_connection = connection;
			_command = command;
			if (nolock)
			{
				_nolock = "WITH(NOLOCK) ";
			}
		}

		protected virtual string EncodeQuotes(string str)
		{
			return str.Replace("'", "''");
		}

		protected virtual string EscapeWildcardCharacters(string param)
		{
			StringBuilder sb = new StringBuilder(param);
			sb.Replace("[", "[[]");
			sb.Replace("%", "[%]");
			sb.Replace("_", "[_]");
			return sb.ToString();
		}

		protected virtual string DateTimeToDbString(DateTime input)
		{
            return input.ToString(@"yyyy-MM-dd HH:mm:ss");
		}

		protected virtual IDbCommand PrepareCommand(string commandText, ArrayList parameters)
		{
			Log.WriteLine("PrepareCommand", commandText);
            _command.CommandText = commandText;
			_command.Connection = _connection;

			_command.Parameters.Clear();
			if (parameters != null)
			{
				for (int i = 0; i < parameters.Count; i++)
				{
					IDataParameter parameter = parameters[i] as IDataParameter;
					if (parameter != null)
					{
						_command.Parameters.Add(parameter);
					}
				}
			}
			return _command;
		}

		protected abstract IDataParameter CreateParameter(string parameterName, object parameterValue);

		public abstract string SelectIdentity();

		protected string NumberArrayToString(Array numbers)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < numbers.Length; i++)
			{
				long l = 0;
				bool isNumber = true;
				try
				{
					l = Convert.ToInt64(numbers.GetValue(i));
				}
				catch
				{
					isNumber = false;
				}
				if (isNumber) sb.AppendFormat("{0},", l.ToString(CultureInfo.InvariantCulture));
			}
			if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);
			return sb.ToString();
		}

		public virtual IDbCommand SelectTablesNames()
		{
			string commandText = string.Format(@"select [name] AS tableNames from sysobjects o {0}where xtype = 'U' and OBJECTPROPERTY(o.id, N'IsMSShipped')!=1",_nolock);
			return PrepareCommand(commandText, null);
		}

		public virtual IDbCommand CreateTable(string tableName, string tablePrefix)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			tablePrefix = EncodeQuotes(tablePrefix);

			string commandText = string.Empty;
			switch (tableName)
            {
#region CreateWebMailTablesCommands
                case Constants.TablesNames.a_users:
					commandText = string.Format(@"
CREATE TABLE [{0}a_users] (
	[id_user] [int] IDENTITY (1, 1) NOT NULL ,
	[deleted] [bit] NOT NULL 
) ON [PRIMARY]
", tablePrefix);
					break;
				case Constants.TablesNames.awm_accounts:
					commandText = string.Format(@"
CREATE TABLE [{0}awm_accounts] (
	[id_acct] [int] IDENTITY (1, 1) NOT NULL ,
	[id_user] [int] NOT NULL ,
	[def_acct] [bit] NOT NULL ,
	[deleted] [bit] NOT NULL ,
	[email] [varchar] (255) NOT NULL ,
	[mail_protocol] [smallint] NOT NULL ,
	[mail_inc_host] [varchar] (255) NULL ,
	[mail_inc_login] [varchar] (255) NULL ,
	[mail_inc_pass] [varchar] (255) NULL ,
	[mail_inc_port] [int] NOT NULL ,
	[mail_out_host] [varchar] (255) NULL ,
	[mail_out_login] [varchar] (255) NULL ,
	[mail_out_pass] [varchar] (255) NULL ,
	[mail_out_port] [int] NOT NULL ,
	[mail_out_auth] [bit] NOT NULL ,
	[friendly_nm] [varchar] (200) NULL ,
	[use_friendly_nm] [bit] NOT NULL ,
	[def_order] [tinyint] NOT NULL ,
	[getmail_at_login] [bit] NOT NULL ,
	[mail_mode] [tinyint] NOT NULL ,
	[mails_on_server_days] [smallint] NOT NULL ,
	[signature] [text] NULL ,
	[signature_type] [tinyint] NOT NULL ,
	[signature_opt] [tinyint] NOT NULL ,
	[delimiter] [char] (1) NOT NULL ,
	[mailbox_size] [bigint] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
", tablePrefix);
					break;
				case Constants.TablesNames.awm_addr_book:
					commandText = string.Format(@"
CREATE TABLE [{0}awm_addr_book] (
	[id_addr] [bigint] IDENTITY (1, 1) NOT NULL ,
	[id_user] [int] NOT NULL ,
	[h_email] [varchar] (255) NULL ,
	[fullname] [varchar] (255) NULL ,
	[notes] [varchar] (255) NULL ,
	[use_friendly_nm] [bit] NOT NULL ,
	[h_street] [varchar] (255) NULL ,
	[h_city] [varchar] (200) NULL ,
	[h_state] [varchar] (200) NULL ,
	[h_zip] [varchar] (10) NULL ,
	[h_country] [varchar] (200) NULL ,
	[h_phone] [varchar] (50) NULL ,
	[h_fax] [varchar] (50) NULL ,
	[h_mobile] [varchar] (50) NULL ,
	[h_web] [varchar] (255) NULL ,
	[b_email] [varchar] (255) NULL ,
	[b_company] [varchar] (200) NULL ,
	[b_street] [varchar] (255) NULL ,
	[b_city] [varchar] (200) NULL ,
	[b_state] [varchar] (200) NULL ,
	[b_zip] [varchar] (10) NULL ,
	[b_country] [varchar] (200) NULL ,
	[b_job_title] [varchar] (100) NULL ,
	[b_department] [varchar] (200) NULL ,
	[b_office] [varchar] (200) NULL ,
	[b_phone] [varchar] (50) NULL ,
	[b_fax] [varchar] (50) NULL ,
	[b_web] [varchar] (255) NULL ,
	[other_email] [varchar] (255) NULL ,
	[primary_email] [tinyint] NULL ,
	[id_addr_prev] [bigint] NOT NULL ,
	[tmp] [bit] NOT NULL ,
	[use_frequency] [int] NOT NULL ,
	[auto_create] [bit] NOT NULL ,
	[birthday_day] [tinyint] NOT NULL ,
	[birthday_month] [tinyint] NOT NULL ,
	[birthday_year] [smallint] NOT NULL 
) ON [PRIMARY]
", tablePrefix);
					break;
				case Constants.TablesNames.awm_addr_groups:
					commandText = string.Format(@"
CREATE TABLE [{0}awm_addr_groups] (
	[id_group] [int] IDENTITY (1, 1) NOT NULL ,
	[id_user] [int] NOT NULL ,
	[group_nm] [varchar] (255) NULL ,
	[use_frequency] [int] NOT NULL ,
	[email] [varchar] (255) NULL ,
	[company] [varchar] (200) NULL ,
	[street] [varchar] (255) NULL ,
	[city] [varchar] (200) NULL ,
	[state] [varchar] (200) NULL ,
	[zip] [varchar] (10) NULL ,
	[country] [varchar] (200) NULL ,
	[phone] [varchar] (50) NULL ,
	[fax] [varchar] (50) NULL ,
	[web] [varchar] (255) NULL ,
	[organization] [bit] NOT NULL 
) ON [PRIMARY]
", tablePrefix);
					break;
				case Constants.TablesNames.awm_addr_groups_contacts:
					commandText = string.Format(@"
CREATE TABLE [{0}awm_addr_groups_contacts] (
	[id_addr] [bigint] NOT NULL ,
	[id_group] [int] NOT NULL 
) ON [PRIMARY]
", tablePrefix);
					break;
				case Constants.TablesNames.awm_columns:
					commandText = string.Format(@"
CREATE TABLE [{0}awm_columns] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[id_user] [int] NOT NULL ,
	[id_column] [int] NOT NULL ,
	[column_value] [int] NOT NULL 
) ON [PRIMARY]
", tablePrefix);
					break;
				case Constants.TablesNames.awm_filters:
					commandText = string.Format(@"
CREATE TABLE [{0}awm_filters] (
	[id_filter] [int] IDENTITY (1, 1) NOT NULL ,
	[id_acct] [int] NOT NULL ,
	[field] [tinyint] NOT NULL ,
	[condition] [tinyint] NOT NULL ,
	[filter] [varchar] (255) NULL ,
	[action] [tinyint] NOT NULL ,
	[id_folder] [bigint] NOT NULL 
) ON [PRIMARY]
", tablePrefix);
					break;
				case Constants.TablesNames.awm_folders:
					commandText = string.Format(@"
CREATE TABLE [{0}awm_folders] (
	[id_folder] [bigint] IDENTITY (1, 1) NOT NULL ,
	[id_acct] [int] NOT NULL ,
	[id_parent] [bigint] NOT NULL ,
	[type] [smallint] NOT NULL ,
	[name] [varchar] (100) NULL ,
	[full_path] [varchar] (255) NULL ,
	[sync_type] [tinyint] NOT NULL ,
	[hide] [bit] NOT NULL ,
	[fld_order] [smallint] NOT NULL 
) ON [PRIMARY]
", tablePrefix);
					break;
				case Constants.TablesNames.awm_folders_tree:
					commandText = string.Format(@"
CREATE TABLE [{0}awm_folders_tree] (
	[id] [bigint] IDENTITY (1, 1) NOT NULL ,
	[id_folder] [bigint] NOT NULL ,
	[id_parent] [bigint] NOT NULL ,
	[folder_level] [tinyint] NOT NULL 
) ON [PRIMARY]
", tablePrefix);
					break;
				case Constants.TablesNames.awm_messages:
					commandText = string.Format(@"
CREATE TABLE [{0}awm_messages] (
	[id] [bigint] IDENTITY (1, 1) NOT NULL ,
	[id_msg] [int] NOT NULL ,
	[id_acct] [int] NOT NULL ,
	[id_folder_srv] [bigint] NOT NULL ,
	[id_folder_db] [bigint] NOT NULL ,
	[str_uid] [varchar] (255) NULL ,
	[int_uid] [bigint] NOT NULL ,
	[from_msg] [varchar] (255) NULL ,
	[to_msg] [varchar] (255) NULL ,
	[cc_msg] [varchar] (255) NULL ,
	[bcc_msg] [varchar] (255) NULL ,
	[subject] [varchar] (255) NULL ,
	[msg_date] [datetime] NULL ,
	[attachments] [bit] NOT NULL ,
	[size] [bigint] NOT NULL ,
	[seen] [bit] NOT NULL ,
	[flagged] [bit] NOT NULL ,
	[priority] [tinyint] NOT NULL ,
	[downloaded] [bit] NOT NULL ,
	[x_spam] [bit] NOT NULL ,
	[rtl] [bit] NOT NULL ,
	[deleted] [bit] NOT NULL ,
	[is_full] [bit] NULL ,
	[replied] [bit] NULL ,
	[forwarded] [bit] NULL ,
	[flags] [tinyint] NULL ,
	[body_text] [text] NULL ,
	[grayed] [bit] NOT NULL ,
	[charset] [int] NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
", tablePrefix);
					break;
				case Constants.TablesIndexes.awm_messages_index:
					commandText = string.Format(@"
CREATE INDEX [{0}awm_messages_index] ON [{0}awm_messages]([id_acct], [id_msg]) ON [PRIMARY]", tablePrefix);
					break;
				case Constants.TablesNames.awm_messages_body:
					commandText = string.Format(@"
CREATE TABLE [{0}awm_messages_body] (
	[id] [bigint] IDENTITY (1, 1) NOT NULL ,
	[id_acct] [int] NOT NULL ,
	[id_msg] [int] NOT NULL ,
	[msg] [image] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
", tablePrefix);
					break;
				case Constants.TablesIndexes.awm_messages_body_index:
					commandText = string.Format(@"
CREATE INDEX [{0}DBTABLE_AWM_MESSAGES_INDEX] ON [{0}awm_messages_body]([id_acct], [id_msg]) ON [PRIMARY]", tablePrefix);
					break;
				case Constants.TablesNames.awm_reads:
					commandText = string.Format(@"
CREATE TABLE [{0}awm_reads] (
	[id_read] [bigint] IDENTITY (1, 1) NOT NULL ,
	[id_acct] [int] NOT NULL ,
	[str_uid] [varchar] (255) NOT NULL ,
	[tmp] [bit] NOT NULL DEFAULT (0)
) ON [PRIMARY]
", tablePrefix);
					break;
				case Constants.TablesNames.awm_senders:
					commandText = string.Format(@"
CREATE TABLE [{0}awm_senders] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[id_user] [int] NOT NULL ,
	[email] [varchar] (255) NOT NULL ,
	[safety] [tinyint] NOT NULL 
) ON [PRIMARY]
", tablePrefix);
					break;
				case Constants.TablesNames.awm_settings:
					commandText = string.Format(@"
CREATE TABLE [{0}awm_settings] (
	[id_setting] [int] IDENTITY (1, 1) NOT NULL ,
	[id_user] [int] NOT NULL ,
	[msgs_per_page] [smallint] NOT NULL ,
	[white_listing] [bit] NOT NULL ,
	[x_spam] [bit] NOT NULL ,
	[last_login] [datetime] NULL ,
	[logins_count] [int] NOT NULL ,
	[def_skin] [varchar] (255) NOT NULL  DEFAULT ('{1}'),
	[def_lang] [varchar] (50) NULL ,
	[def_charset_inc] [int] NULL ,
	[def_charset_out] [int] NULL ,
	[def_timezone] [smallint] NOT NULL ,
	[def_date_fmt] [varchar] (20) NOT NULL DEFAULT ('{2}'),
	[hide_folders] [bit] NOT NULL ,
	[mailbox_limit] [bigint] NOT NULL ,
	[allow_change_settings] [bit] NOT NULL ,
	[allow_dhtml_editor] [bit] NOT NULL ,
	[allow_direct_mode] [bit] NOT NULL ,
	[hide_contacts] [bit] NOT NULL ,
	[db_charset] [int] NOT NULL ,
	[horiz_resizer] [smallint] NOT NULL ,
	[vert_resizer] [smallint] NULL ,
	[mark] [tinyint] NOT NULL ,
	[reply] [tinyint] NOT NULL ,
	[contacts_per_page] [smallint] NOT NULL ,
	[view_mode] [tinyint] NOT NULL 
) ON [PRIMARY]
", 
						tablePrefix,
						EncodeQuotes(settings.DefaultSkin),
						EncodeQuotes(Constants.DateFormats.Default));
					break;
				case Constants.TablesNames.awm_temp:
					commandText = string.Format(@"
CREATE TABLE [{0}awm_temp] (
	[id_temp] [bigint] IDENTITY (1, 1) NOT NULL ,
	[id_acct] [int] NOT NULL ,
	[data_val] [text] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
", tablePrefix);
					break;
#endregion

#region CreateCalendarTablesCommands
                case Constants.TablesNames.acal_calendars:
                        commandText = string.Format(@"
DECLARE @col_name VARCHAR(50) 
DECLARE @qryString VARCHAR(1000)
SET @col_name = (SELECT     CONVERT(varchar(50), SERVERPROPERTY('collation')))
SET @qryString='CREATE TABLE [{0}acal_calendars] (
    [calendar_id] [int] PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [user_id] [int] NOT NULL DEFAULT (0),
    [calendar_name] [varchar] (100) COLLATE '+@col_name+' NOT NULL DEFAULT (''''),
    [calendar_description] [varchar] (510) COLLATE '+@col_name+' NOT NULL DEFAULT (''''),
    [calendar_color] [int] NOT NULL DEFAULT (0),
    [calendar_active] [bit] NOT NULL DEFAULT (0)
) ON [PRIMARY]'
EXEC(@qryString)", tablePrefix);
                    break;
                case Constants.TablesNames.acal_events:
                        commandText = string.Format(@"
DECLARE @col_name VARCHAR(50) 
DECLARE @qryString VARCHAR(1000)
SET @col_name = (SELECT     CONVERT(varchar(50), SERVERPROPERTY('collation')))
SET @qryString='CREATE TABLE [{0}acal_events] (
    [event_id] [int] PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    [calendar_id] [int] NOT NULL DEFAULT (0),
    [event_timefrom] [datetime] NOT NULL,
    [event_timetill] [datetime] NOT NULL,
    [event_allday] [bit] NOT NULL DEFAULT (0),
    [event_name] [varchar] (100) COLLATE '+@col_name+' NOT NULL DEFAULT (''''),
    [event_text] [varchar] (510) COLLATE '+@col_name+' NULL,
    [event_priority] [tinyint] NULL DEFAULT (0)
) ON [PRIMARY]'
EXEC(@qryString)", tablePrefix);
                    break;
                case Constants.TablesNames.acal_users_data:
                        commandText = string.Format(@"
DECLARE @col_name VARCHAR(50) 
DECLARE @qryString VARCHAR(1000)
SET @col_name = (SELECT     CONVERT(varchar(50), SERVERPROPERTY('collation')))
SET @qryString = 'CREATE TABLE [{0}acal_users_data] (
    [settings_id] [int] PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [user_id] [int] NOT NULL DEFAULT (0),
    [timeformat] [tinyint] NOT NULL DEFAULT (1), 
    [dateformat] [tinyint] NOT NULL DEFAULT (1), 
    [showweekends]  [tinyint] NOT NULL DEFAULT (0),
    [workdaystarts]  [tinyint] NOT NULL DEFAULT (0), 
    [workdayends] [tinyint] NOT NULL DEFAULT (1), 
    [showworkday] [tinyint] NOT NULL DEFAULT (0),
    [weekstartson] [tinyint] NOT NULL default (0), 
    [defaulttab]  [tinyint] NOT NULL DEFAULT (1), 
    [country] [varchar] (2)  NULL,
	[timezone] [smallint] NULL,
    [alltimezones] [tinyint] NOT NULL DEFAULT (0) ) ON [PRIMARY] '
EXEC (@qryString)", tablePrefix);
                    break;
#endregion
            }

			return PrepareCommand(commandText, null);
		}

		public virtual IDbCommand SelectTableFields(string tableName, string tablePrefix)
		{
			string commandText = string.Format(@"SELECT [INFORMATION_SCHEMA].[COLUMNS].[COLUMN_NAME] FROM [INFORMATION_SCHEMA].[COLUMNS] {3}WHERE [TABLE_NAME]='{0}{1}'",
				EncodeQuotes(tablePrefix),
				EncodeQuotes(tableName),
				_nolock);
			return PrepareCommand(commandText, null);
		}

		public virtual IDbCommand AlterTable(string tableName, string tablePrefix)
		{
			string commandText = string.Empty;
			switch (tableName)
			{
				case Constants.TablesNames.awm_addr_book:
					commandText = string.Format(@"ALTER TABLE [{0}awm_addr_book] 
ADD use_frequency INT DEFAULT 0 NOT NULL, auto_create BIT DEFAULT 0 NOT NULL
", tablePrefix);
					break;
				case Constants.TablesNames.awm_addr_groups:
					commandText = string.Format(@"ALTER TABLE [{0}awm_addr_groups] 
ADD email VARCHAR(255) DEFAULT '' NOT NULL,
company VARCHAR(200) DEFAULT '' NOT NULL,
street VARCHAR(255) DEFAULT '' NOT NULL,
city VARCHAR(200) DEFAULT '' NOT NULL,
state VARCHAR(200) DEFAULT '' NOT NULL,
zip VARCHAR(10) DEFAULT '' NOT NULL,
country VARCHAR(200) DEFAULT '' NOT NULL,
phone VARCHAR(50) DEFAULT '' NOT NULL,
fax VARCHAR(50) DEFAULT '' NOT NULL,
web VARCHAR(255) DEFAULT '' NOT NULL,
organization BIT DEFAULT 0 NOT NULL,
use_frequency INT DEFAULT 0 NOT NULL
", tablePrefix);
					break;
			}

			return PrepareCommand(commandText, null);
		}

		public virtual IDbCommand SelectTableIndexes(string tableName, string tablePrefix)
		{
			string commandText = string.Format(@"select [name] AS tableNames from sysobjects o {0}where xtype = 'U' and OBJECTPROPERTY(o.id, N'IsMSShipped')!=1", _nolock);
			return PrepareCommand(commandText, null);
		}

		public virtual IDbCommand CreateIndex(string prefix, string sufix, string column) {
			string commandText;
			commandText = string.Format(@"CREATE INDEX {0}{1} ON {0} ({2});", prefix, sufix, column);
			return PrepareCommand(commandText, null);
		}

		/// <summary>
		/// Delete row by specified account ID
		/// </summary>
		/// <param name="id_acct">Account ID</param>
		/// <returns>Command</returns>
		public virtual IDbCommand DeleteFromAwmAccounts(int id_acct)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_acct=@id_acct",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand DeleteFromAwmAddrBook(int id_user, long[] id_addrs)
		{
			string strIn = NumberArrayToString(id_addrs);
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_user=@id_user AND id_addr IN ({1})",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
				strIn);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand DeleteFromAwmAddrGroupContacts(long[] id_addrs)
		{
			string strIn = NumberArrayToString(id_addrs);
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_addr IN ({1})",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts),
				strIn);
			
			return PrepareCommand(commandText, null);
		}

		public virtual IDbCommand DeleteFromAwmAddrGroupContacts(int[] id_groups)
		{
			string strIn = NumberArrayToString(id_groups);
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_group IN ({1})",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts),
				strIn);

			return PrepareCommand(commandText, null);
		}

		public virtual IDbCommand DeleteFromAwmAddrGroupContacts(long id_addr)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_addr=@id_addr",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_addr", id_addr));
			
            return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand DeleteFromAwmAddrGroupContacts(int id_group)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_group=@id_group",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_group", id_group));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand DeleteFromAwmAddrGroupContactsAll(int id_user)
		{
			string commandText = string.Format(@"DELETE FROM {0} WHERE id_group IN 
(SELECT id_group FROM {1} WHERE id_user={2})",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),
				id_user);

			return PrepareCommand(commandText, null);
		}

		public virtual IDbCommand DeleteFromAwmAddrGroupContacts(int id_group, long[] id_addrs)
		{
			string strIn = NumberArrayToString(id_addrs);
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_group=@id_group AND id_addr IN ({1})",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts),
				strIn);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_group", id_group));

			return PrepareCommand(commandText, parameters);
		}

		/// <summary>
		/// Delete all rows by specified account ID
		/// </summary>
		/// <param name="id_user">User ID</param>
		/// <returns>Command</returns>
		public virtual IDbCommand DeleteFromAwmAddrBook(int id_user)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_user={1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
				id_user);

			return PrepareCommand(commandText, null);
		}

		/// <summary>
		/// Delete all rows by specified account ID
		/// </summary>
		/// <param name="id_user">User ID</param>
		/// <returns>Command</returns>
		public virtual IDbCommand DeleteFromAwmAddrGroups(int id_user, int id_group)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_user=@id_user AND id_group=@id_group",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@id_group", id_group));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand DeleteFromAwmAddrGroups(int id_user)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_user={1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),
				id_user);

			return PrepareCommand(commandText, null);
		}
		/// <summary>
		/// Delete all rows by specified account ID
		/// </summary>
		/// <param name="id_acct">Account ID</param>
		/// <returns>Command</returns>
		public virtual IDbCommand DeleteFromAwmFilters(int id_acct, int id_filter, long id_folder)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_acct=@id_acct{1}{2}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_filters),
				(id_filter > 0) ? string.Format(" AND id_filter={0}", id_filter): string.Empty,
				(id_folder > 0) ? string.Format(" AND id_folder={0}", id_folder): string.Empty);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand DeleteFromAwmColumns(int id, int id_user)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_user={1}{2}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_columns),
				id_user,
				(id > 0) ? string.Format(" AND id={0}", id): string.Empty);

			return PrepareCommand(commandText, null);
		}

		public virtual IDbCommand DeleteFromAwmDomains(int id)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_domain={1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains),
				id);

			return PrepareCommand(commandText, null);
		}

		/// <summary>
		/// Delete all rows by specified account ID
		/// </summary>
		/// <param name="id_acct">Account ID</param>
		/// <returns>Command</returns>
		public virtual IDbCommand DeleteFromAwmFolders(int id_acct)
		{
			string commandText = string.Format(@"DELETE FROM {0} WHERE id_acct=@id_acct",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		/// <summary>
		/// Delete folder with subfolders by specified account ID and folder ID
		/// </summary>
		/// <param name="id_acct">Account ID</param>
		/// <returns>Command</returns>
		public virtual IDbCommand DeleteFromAwmFolders(int id_acct, long id_folder)
		{
			string commandText = string.Format(@"
DELETE FROM {0} WHERE id_folder=@id_folder AND id_acct=@id_acct
DELETE FROM {1} WHERE id_folder=@id_folder
",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders_tree));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand DeleteFromAwmFoldersTree(int id_acct)
		{
			string commandText = string.Format(@"DELETE FROM {0} WHERE id_folder
 IN (SELECT id_folder FROM {1} {2}WHERE id_acct=@id_acct)",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders_tree),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand DeleteFromAwmFoldersTree(long id_folder)
		{
			string commandText = string.Format(@"DELETE FROM {0} WHERE id_folder=@id_folder",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders_tree));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand DeleteFromAwmMessages(int id_acct)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_acct=@id_acct",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			
			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand DeleteFromAwmMessages(int id_acct, long id_folder_db)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_acct=@id_acct AND id_folder_db=@id_folder_db",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));
			
			return PrepareCommand(commandText, parameters);
		}

		/// <summary>
		/// Delete all rows by specified account ID and message ID's
		/// </summary>
		/// <param name="id_acct">Account ID</param>
		/// <param name="id_msgs">Message ID's</param>
		/// <returns>Command</returns>
		public virtual IDbCommand DeleteFromAwmMessages(int id_acct, int[] id_msgs)
		{
			string strIn = NumberArrayToString(id_msgs);
			
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_acct=@id_acct AND id_msg IN ({1})",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				strIn);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectSumSizesOfRemainMessages(int id_acct)
		{
			string commandText = string.Format(@"SELECT SUM([size]) FROM {0}
{1}WHERE id_acct=@id_acct",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		/// <summary>
		/// Delete all rows by specified account ID
		/// </summary>
		/// <param name="id_acct">Account ID</param>
		/// <returns>Command</returns>
		public virtual IDbCommand DeleteFromAwmMessagesBody(int id_acct)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_acct=@id_acct",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages_body));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			
			return PrepareCommand(commandText, parameters);
		}

		/// <summary>
		/// Delete all rows by specified account ID and message ID
		/// </summary>
		/// <param name="id_acct">Account ID</param>
		/// <param name="id_msg_array">Array of messages ID's</param>
		/// <returns>Command</returns>
		public virtual IDbCommand DeleteFromAwmMessagesBody(int id_acct, int[] id_msg_array)
		{
			string strIn = NumberArrayToString(id_msg_array);
			
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_acct=@id_acct AND id_msg IN ({1})",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages_body),
				strIn);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		/// <summary>
		/// Delete all rows by specified account ID
		/// </summary>
		/// <param name="id_acct">Account ID</param>
		/// <returns>Command</returns>
		public virtual IDbCommand DeleteFromAwmReads(int id_acct)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_acct=@id_acct",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_reads),
				id_acct);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			
			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand InsertIntoAUsers(bool deleted)
		{
			string commandText = string.Format(@"INSERT INTO {0} (deleted) VALUES(@deleted);
{1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.a_users),
				SelectIdentity());

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@deleted", deleted));

			return PrepareCommand(commandText, parameters);
		}

		public IDbCommand DeleteFromAUsers(int id_user)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_user={1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.a_users),
				id_user);

			return PrepareCommand(commandText, null);
		}

		public IDbCommand DeleteFromAwmSettings(int id_user)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_user={1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_settings),
				id_user);

			return PrepareCommand(commandText, null);
		}

        public IDbCommand DeleteFromAuser(int id_user)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_user={1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.a_users),
				id_user);

			return PrepareCommand(commandText, null);
		}

		public IDbCommand DeleteFromAwmSenders(int id_user)
		{
			string commandText = string.Format(@"DELETE FROM {0}
WHERE id_user={1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_senders),
				id_user);

			return PrepareCommand(commandText, null);
		}

		public virtual IDbCommand InsertIntoAwmAccounts(int id_user, bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol, string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host, string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm, bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode, short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt, string delimiter, long mailbox_size)
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
@signature_opt, @delimiter, @mailbox_size);
{1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts),
				SelectIdentity());

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

		public virtual IDbCommand InsertIntoAwmAddrBook(int id_user, string h_email, string fullname, string notes, bool use_friendly_nm, string h_street, string h_city, string h_state, string h_zip, string h_country, string h_phone, string h_fax, string h_mobile, string h_web, string b_email, string b_company, string b_street, string b_city, string b_state, string b_zip, string b_country, string b_job_title, string b_department, string b_office, string b_phone, string b_fax, string b_web, byte birthday_day, byte birthday_month, short birthday_year, string other_email, short primary_email, long id_addr_prev, bool tmp, int use_frequency, bool auto_create)
		{
			string commandText = string.Format(@"INSERT INTO {0}(id_user, h_email, fullname, notes,
 use_friendly_nm, h_street, h_city, h_state, h_zip, h_country, h_phone, h_fax, h_mobile,
 h_web, b_email, b_company, b_street, b_city, b_state, b_zip, b_country, b_job_title,
 b_department, b_office, b_phone, b_fax, b_web, birthday_day, birthday_month, birthday_year,
 other_email, primary_email, id_addr_prev, tmp, use_frequency, auto_create)
VALUES(@id_user, @h_email, @fullname, @notes,
 @use_friendly_nm, @h_street, @h_city, @h_state, @h_zip, @h_country, @h_phone, @h_fax, @h_mobile,
 @h_web, @b_email, @b_company, @b_street, @b_city, @b_state, @b_zip, @b_country, @b_job_title,
 @b_department, @b_office, @b_phone, @b_fax, @b_web, @birthday_day, @birthday_month, @birthday_year,
 @other_email, @primary_email, @id_addr_prev, @tmp, @use_frequency, @auto_create);
{1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
				SelectIdentity());

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

		public virtual IDbCommand InsertIntoAwmAddrGroups(int id_user, string group_nm, string phone, string fax, string web, bool organization, int use_frequency, string email, string company, string street, string city, string state, string zip, string country)
		{
			string commandText = string.Format(@"INSERT INTO {0}(id_user, group_nm, phone, fax, web, organization, use_frequency, email, company, street, city, state, zip, country)
VALUES(@id_user, @group_nm, @phone, @fax, @web, @organization, @use_frequency, @email, @company, @street, @city, @state, @zip, @country);
{1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),
				SelectIdentity());

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

		public virtual IDbCommand InsertIntoAwmColumns(int id_column, int id_user, int value)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_user, id_column, column_value)
VALUES (@id_user, @id_column, @value);
{1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_columns),
				SelectIdentity());


			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@id_column", id_column));
			parameters.Add(CreateParameter("@value", value));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand InsertIntoAwmDomains(string name, short mail_protocol, string mail_inc_host, int mail_inc_port, string mail_out_host, int mail_out_port, bool mail_out_auth)
		{
			string commandText = string.Format(@"INSERT INTO {0} (name, mail_protocol, mail_inc_host, mail_inc_port, mail_out_host, mail_out_port, mail_out_auth)
VALUES (@name, @mail_protocol, @mail_inc_host, @mail_inc_port, @mail_out_host, @mail_out_port, @mail_out_auth);
",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains));


			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@name", name));
			parameters.Add(CreateParameter("@mail_protocol", mail_protocol));
			parameters.Add(CreateParameter("@mail_inc_host", mail_inc_host));
			parameters.Add(CreateParameter("@mail_inc_port", mail_inc_port));
			parameters.Add(CreateParameter("@mail_out_host", mail_out_host));
			parameters.Add(CreateParameter("@mail_out_port", mail_out_port));
			parameters.Add(CreateParameter("@mail_out_auth", mail_out_auth));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand InsertIntoAwmFilters(int id_acct, byte field, byte condition, string filter, byte action, long id_folder)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_acct, field, condition, filter, action, id_folder)
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

		public virtual IDbCommand InsertIntoAwmFolders(int id_acct, long id_parent, FolderType type, string name, string full_path, FolderSyncType sync_type, bool hide, short fld_order)
		{
			string commandText = string.Format(@"
DECLARE @NewFolderID bigint

INSERT INTO {0} (id_acct, id_parent, type, name, full_path, sync_type, hide, fld_order)
	VALUES (@id_acct, @id_parent, @type, @name, @full_path, @sync_type, @hide, @fld_order)

SET @NewFolderID = {2}

INSERT INTO {1} (id_folder, id_parent, folder_level) VALUES (@NewFolderID, @NewFolderID, 0)

INSERT INTO {1}
SELECT @NewFolderID, id_parent, folder_level + 1
FROM {1}
WHERE id_folder=@id_parent
SELECT @NewFolderID",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders_tree),
				SelectIdentity().Remove(0, 7));  // 7 == "SELECT ".Length
				//_nolock);

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

		public virtual IDbCommand InsertIntoAwmFoldersTree(long id_folder)
		{
			string commandText = string.Format(@"
INSERT INTO {0} (id_folder, id_parent, folder_level) VALUES (@id_folder, @id_folder, 0);",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders_tree));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand InsertIntoAwmFoldersTree(long id_folder, long id_parent)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_folder, id_parent, folder_level)
SELECT @id_folder, id_parent, (folder_level + 1) AS folders_level
FROM {0}
WHERE id_folder=@id_parent;",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders_tree));
				// _nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@id_parent", id_parent));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand InsertIntoAwmMessages( 
            int id_msg, 
            int id_acct, 
            long id_folder_srv, 
            long id_folder_db, 
            string str_uid, 
            long int_uid, 
            string from_msg, 
            string to_msg, 
            string cc_msg, 
            string bcc_msg, 
            string subject,
            //string loanapplicants,
            //string loanappids,
            //int conditionId,
            DateTime msg_date, 
            bool attachments, 
            long size, 
            bool seen, 
            bool flagged, 
            byte priority, 
            bool downloaded, 
            bool x_spam, 
            bool rtl, 
            bool deleted, 
            bool is_full, 
            bool replied, 
            bool forwarded, 
            byte flags, 
            string body_text, 
            bool grayed, 
            int charset)
		{
			if (str_uid != null)
			{
				if (str_uid.Length > 255)
				{
					str_uid = str_uid.Substring(0, 255);
					is_full = false;
				}
			}

			if (from_msg != null)
			{
				if (from_msg.Length > 255)
				{
					from_msg = from_msg.Substring(0, 255);
					is_full = false;
				}
			}

			if (to_msg != null)
			{
				if (to_msg.Length > 255)
				{
					to_msg = to_msg.Substring(0, 255);
					is_full = false;
				}
			}

			if (cc_msg != null)
			{
				if (cc_msg.Length > 255)
				{
					cc_msg = cc_msg.Substring(0, 255);
					is_full = false;
				}
			}

			if (bcc_msg != null)
			{
				if (bcc_msg.Length > 255)
				{
					bcc_msg = bcc_msg.Substring(0, 255);
					is_full = false;
				}
			}

			if (subject != null)
			{
				if (subject.Length > 255)
				{
					subject = subject.Substring(0, 255);
					is_full = false;
				}
			}

            //if (loanapplicants != null)
            //{
            //    if (loanapplicants.Length > 255)
            //    {
            //        loanapplicants = loanapplicants.Substring(0, 255);
            //        is_full = false;
            //    }
            //}

            //if (loanappids != null)
            //{
            //    if (loanappids.Length > 255)
            //    {
            //        loanappids = loanappids.Substring(0, 255);
            //        is_full = false;
            //    }
            //}

//            string commandText = string.Format(@"INSERT INTO {0} (id_msg, id_acct, id_folder_srv, id_folder_db,
// str_uid, int_uid, from_msg, to_msg, cc_msg, bcc_msg, subject, loanapplicants, loanappids, conditionId, msg_date, attachments,
// [size], seen, flagged, priority, downloaded, x_spam, rtl, deleted, is_full, replied,
// forwarded, flags, body_text, grayed, charset)
//VALUES(@id_msg, @id_acct, @id_folder_srv, @id_folder_db,
// @str_uid, @int_uid, @from_msg, @to_msg, @cc_msg, @bcc_msg, @subject, @loanapplicants, @loanappids, @conditionId, @msg_date, @attachments,
// @size, @seen, @flagged, @priority, @downloaded, @x_spam, @rtl, @deleted, @is_full, @replied,
// @forwarded, @flags, @body_text, @grayed, @charset)",
			string commandText = string.Format(@"INSERT INTO {0} (id_msg, id_acct, id_folder_srv, id_folder_db,
 str_uid, int_uid, from_msg, to_msg, cc_msg, bcc_msg, subject, msg_date, attachments,
 [size], seen, flagged, priority, downloaded, x_spam, rtl, deleted, is_full, replied,
 forwarded, flags, body_text, grayed, charset)
VALUES(@id_msg, @id_acct, @id_folder_srv, @id_folder_db,
 @str_uid, @int_uid, @from_msg, @to_msg, @cc_msg, @bcc_msg, @subject,  @msg_date, @attachments,
 @size, @seen, @flagged, @priority, @downloaded, @x_spam, @rtl, @deleted, @is_full, @replied,
 @forwarded, @flags, @body_text, @grayed, @charset)",

				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_msg", id_msg));
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_srv", id_folder_srv));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));
			parameters.Add(CreateParameter("@str_uid", str_uid));
			parameters.Add(CreateParameter("@int_uid", int_uid));
			parameters.Add(CreateParameter("@from_msg", from_msg));
			parameters.Add(CreateParameter("@to_msg", to_msg));
			parameters.Add(CreateParameter("@cc_msg", cc_msg));
			parameters.Add(CreateParameter("@bcc_msg", bcc_msg));
			parameters.Add(CreateParameter("@subject", subject));
            //parameters.Add(CreateParameter("@loanapplicants", loanapplicants));
            //parameters.Add(CreateParameter("@loanappids", loanappids));
            //parameters.Add(CreateParameter("@conditionId", conditionId));
			parameters.Add(CreateParameter("@msg_date", msg_date));
			parameters.Add(CreateParameter("@attachments", attachments));
			parameters.Add(CreateParameter("@size", size));
			parameters.Add(CreateParameter("@seen", seen));
			parameters.Add(CreateParameter("@flagged", flagged));
			parameters.Add(CreateParameter("@priority", priority));
			parameters.Add(CreateParameter("@downloaded", downloaded));
			parameters.Add(CreateParameter("@x_spam", x_spam));
			parameters.Add(CreateParameter("@rtl", rtl));
			parameters.Add(CreateParameter("@deleted", deleted));
			parameters.Add(CreateParameter("@is_full", is_full));
			parameters.Add(CreateParameter("@replied", replied));
			parameters.Add(CreateParameter("@forwarded", forwarded));
			parameters.Add(CreateParameter("@flags", flags));
			parameters.Add(CreateParameter("@body_text", body_text));
			parameters.Add(CreateParameter("@grayed", grayed));
			parameters.Add(CreateParameter("@charset", charset));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand InsertIntoAwmMessagesBody(int id_acct, int id_msg, byte[] msg)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_acct, id_msg, msg)
VALUES(@id_acct, @id_msg, @msg)",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages_body));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_msg", id_msg));
			parameters.Add(CreateParameter("@msg", msg));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand InsertIntoAwmReads(int id_acct, string str_uid)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_acct, str_uid, tmp)
VALUES (@id_acct, @str_uid, 0)",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_reads));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@str_uid", str_uid));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand InsertIntoAwmSenders(int id_user, string email, byte safety)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_user, email, safety)
VALUES (@id_user, @email, @safety)",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_senders));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@email", email));
			parameters.Add(CreateParameter("@safety", safety));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand InsertIntoAwmSettings(int id_user, short msgs_per_page, bool white_listing, bool x_spam, DateTime last_login, int logins_count, string def_skin, string def_lang, int def_charset_inc, short def_timezone, string def_date_fmt, bool hide_folders, long mailbox_limit, bool allow_change_settings, bool allow_dhtml_editor, bool allow_direct_mode, bool hide_contacts, int db_charset, short horiz_resizer, short vert_resizer, byte mark, byte reply, short contacts_per_page, int def_charset_out, byte view_mode)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_user, msgs_per_page, white_listing, x_spam, 
 last_login, logins_count, def_skin, def_lang, def_charset_inc, def_charset_out, def_timezone, def_date_fmt,
 hide_folders, mailbox_limit, allow_change_settings, allow_dhtml_editor, allow_direct_mode, hide_contacts, 
db_charset, horiz_resizer, vert_resizer, mark, reply, contacts_per_page, view_mode)
VALUES(@id_user, @msgs_per_page, @white_listing, @x_spam,
 @last_login, @logins_count, @def_skin, @def_lang, @def_charset_inc, @def_charset_out, @def_timezone, @def_date_fmt,
 @hide_folders, @mailbox_limit, @allow_change_settings, @allow_dhtml_editor, @allow_direct_mode, @hide_contacts,
 @db_charset, @horiz_resizer, @vert_resizer, @mark, @reply, @contacts_per_page, @view_mode);
{1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_settings),
				SelectIdentity());

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

		public virtual IDbCommand SelectAwmAccountsForAdmin(int page, int pageSize, string orderBy, bool asc, string searchCondition)
		{
			string whereConditionInternal = string.Empty;
			string whereConditionExternal = string.Empty;
			if ((searchCondition != null) && (searchCondition.Length > 0))
			{
				whereConditionInternal = string.Format(@"WHERE email LIKE ('%{0}%') OR last_login LIKE ('%{0}%') OR logins_count LIKE ('%{0}%') OR mail_inc_host LIKE ('%{0}%') OR mail_out_host LIKE ('%{0}%')", EscapeWildcardCharacters(EncodeQuotes(searchCondition))); //, _nolock);
				whereConditionExternal = string.Format(@"AND (email LIKE ('%{0}%') OR last_login LIKE ('%{0}%') OR logins_count LIKE ('%{0}%') OR mail_inc_host LIKE ('%{0}%') OR mail_out_host LIKE ('%{0}%'))", EscapeWildcardCharacters(EncodeQuotes(searchCondition)));
			}

			string commandText = string.Format(@"SELECT TOP {2} {0}.id_user, id_acct, email, last_login, logins_count, mail_inc_host, mail_out_host, mailbox_size, mailbox_limit FROM {0}
INNER JOIN {1} ON {0}.id_user = {1}.id_user WHERE id_acct NOT IN
(SELECT TOP {3} id_acct FROM {0} {6} ORDER BY {4} {5}) {7}
ORDER BY {4} {5}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_settings),
				pageSize,
				(page > 0) ? (page - 1) * pageSize : 0,
				orderBy,
				(asc) ? "ASC" : "DESC",
				whereConditionInternal,
				whereConditionExternal);
				// _nolock); //8

			return PrepareCommand(commandText, null);
		}

		public virtual IDbCommand SelectMailboxsSize(int id_user)
		{
            string commandText = string.Format(@"SELECT SUM([mailbox_size]) FROM [{0}] {1}WHERE [id_user]=@id_user",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmAccounts(int id_user, int id_acct)
		{
			string whereCondition;
			if ((id_user > 0) && (id_acct > 0))
			{
				whereCondition = string.Format(@"account.id_user={0} AND account.id_acct={1}", id_user, id_acct);
			}
			else if ((id_user > 0) && (id_acct < 0))
			{
				whereCondition = string.Format(@"account.id_user={0}", id_user);
			}
			else
			{
				whereCondition = string.Format(@"account.id_acct={0}", id_acct);
			}

			string commandText = string.Format(@"SELECT * FROM {0} AS account {2}WHERE {1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts),
				whereCondition,
				_nolock);

			return PrepareCommand(commandText, null);
		}

		public virtual IDbCommand SelectAwmAccountsNonDefaultCount(string email, string login, string password)
		{
			string commandText = string.Format(@"
SELECT COUNT(*)	FROM {0}
{4}WHERE email LIKE '{1}' AND mail_inc_login LIKE '{2}' AND mail_inc_pass LIKE '{3}' AND def_acct=0
",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts),
				EncodeQuotes(email),
				EncodeQuotes(login),
				EncodeQuotes(password),
				_nolock);

			return PrepareCommand(commandText, null);
		}

		public virtual IDbCommand SelectAwmAccountsCount(string searchCondition)
		{
			string whereCondition = string.Empty;
			if ((searchCondition != null) && (searchCondition.Length > 0))
			{
				whereCondition = string.Format(@"WHERE email LIKE ('%{0}%') OR last_login LIKE ('%{0}%') OR logins_count LIKE ('%{0}%') OR mail_inc_host LIKE ('%{0}%') OR mail_out_host LIKE ('%{0}%')", EscapeWildcardCharacters(EncodeQuotes(searchCondition)));
			}

			string commandText = string.Format(@"SELECT COUNT(*) FROM {0} INNER JOIN {1} ON {0}.id_user = {1}.id_user {2}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_settings),
				whereCondition);

			return PrepareCommand(commandText, null);
		}

        public virtual IDbCommand SelectAwmUsersCount()
		{
			string commandText = string.Format(@"SELECT COUNT(id_user) FROM {0}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_settings));

			return PrepareCommand(commandText, null);
		}

        

		public virtual IDbCommand SelectAwmAccounts(string email, string login, string password)
		{
			string whereCondition = string.Format(@"email LIKE '{0}'", EncodeQuotes(email));
			if (login != null)
			{
				whereCondition += string.Format(@" AND mail_inc_login LIKE '{0}'", EncodeQuotes(login));
			}
			if (password != null)
			{
				whereCondition += string.Format(@" AND mail_inc_pass LIKE '{0}'", EncodeQuotes(password));
			}
			string commandText = string.Format(@"SELECT * FROM {0} {2}WHERE {1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts),
				whereCondition,
				_nolock);

			return PrepareCommand(commandText, null);
		}

		public virtual IDbCommand SelectAwmColumns(int id_user)
		{
            string commandText = string.Format(@"SELECT * FROM {0} {2}WHERE id_user={1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_columns),
				id_user,
				_nolock);

			return PrepareCommand(commandText, null);
		}

		public virtual IDbCommand SelectAwmFilters(int id_acct, int id_filter)
		{
			string commandText = string.Format(@"
SELECT * FROM {0}
{2}WHERE id_acct=@id_acct{1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_filters),
				(id_filter > 0) ? string.Format(" AND id_filter={0}", id_filter) : string.Empty,
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmFolders(int id_acct, long id_folder)
		{
			string commandText = string.Format(@"SELECT * FROM {0} 
{1}WHERE id_acct=@id_acct AND id_folder=@id_folder",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder", id_folder));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmFolders(int id_acct, FolderType type)
		{
			string commandText = string.Format(@"SELECT * FROM {0} 
{1}WHERE id_acct=@id_acct AND type=@type",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@type", type));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmFolders(int id_acct)
		{
			string commandText = string.Format(@"
SELECT p.*
FROM {0} n, {1} t, {0} p
{2}WHERE n.id_parent = -1
	AND n.id_folder = t.id_parent
	AND t.id_folder = p.id_folder
	AND p.id_acct = @id_acct
ORDER BY t.folder_level",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders_tree),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmFoldersChilds(int id_acct, long id_parent)
		{
			string commandText = string.Format(@"
SELECT p.*
FROM {0} n, {1} t, {0} p
{2}WHERE n.id_parent = @id_parent
	AND n.id_folder = t.id_parent
	AND t.id_folder = p.id_folder
	AND p.id_acct = @id_acct
ORDER BY t.folder_level",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders_tree),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_parent", id_parent));
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmFolders(int id_acct, string full_path)
		{
			string commandText = string.Format(@"SELECT TOP 1 * FROM {0} 
{1}WHERE id_acct=@id_acct AND full_path=@full_path",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@full_path", full_path));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAUsersAndAwmSettings(int id_user)
		{
			string commandText = string.Format(@"SELECT * FROM {0} AS users {2} INNER JOIN {1} AS settings
ON users.id_user = settings.id_user
WHERE users.id_user=@id_user",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.a_users),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_settings),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectIDMsgFromAwmMessages(int id_acct)
		{
            string commandText = string.Format(@"SELECT MAX(id_msg) FROM {0} {1}WHERE id_acct=@id_acct",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectFolderSize(long id_folder, int id_acct)
		{
			string commandText = string.Format(@"
SELECT SUM([size]) FROM {0}
{1}WHERE id_folder_db=@id_folder AND id_acct=@id_acct",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectFolderMessageCount(long id_folder, int id_acct)
		{
			string commandText = string.Format(@"
SELECT COUNT(*)
FROM {0}
{1}WHERE id_folder_db=@id_folder AND id_acct=@id_acct",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAddressBookContactsCount(int id_user, string look_for, int look_for_type)
		{
			string lookForSearchCondition = string.Empty;
			if ((look_for != null) && (look_for.Length > 0))
			{
				lookForSearchCondition = string.Format(@" AND (fullname LIKE '{0}' OR h_email LIKE '{0}' OR b_email LIKE '{0}' OR other_email LIKE '{0}')", (look_for_type == 0) ? "%" + EscapeWildcardCharacters(EncodeQuotes(look_for)) + "%" : EscapeWildcardCharacters(EncodeQuotes(look_for)) + "%");
			}

			string commandText = string.Format(@"
SELECT COUNT(*)
FROM {0}
{2}WHERE id_user=@id_user{1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
				lookForSearchCondition,
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAddressBookGroupsCount(int id_user, string look_for, int look_for_type)
		{
			string lookForSearchCondition = string.Empty;
			if ((look_for != null) && (look_for.Length > 0))
			{
				lookForSearchCondition = string.Format(@" AND (group_nm LIKE '{0}' OR email LIKE '{0}')", (look_for_type == 0) ? "%" + EscapeWildcardCharacters(EncodeQuotes(look_for)) + "%" : EscapeWildcardCharacters(EncodeQuotes(look_for)) + "%");
			}

			string commandText = string.Format(@"
SELECT COUNT(*)
FROM {0}
{2}WHERE id_user=@id_user{1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),
				lookForSearchCondition,
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectFolderUnreadMessageCount(long id_folder, int id_acct)
		{
			string commandText = string.Format(@"
SELECT COUNT(*)
FROM {0}
{1}WHERE id_folder_db=@id_folder AND id_acct=@id_acct AND seen=0",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectTop1AwmMessages(int id_acct, long id_folder_db)
		{
			string commandText = string.Format(@"SELECT TOP 1 int_uid FROM {0}
{1}WHERE id_acct=@id_acct AND id_folder_db=@id_folder_db ORDER BY int_uid DESC",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmMessages(int id_acct, long id_folder_db)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
{1}WHERE id_acct=@id_acct AND id_folder_db=@id_folder_db ORDER BY id_msg ASC",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}
        public virtual IDbCommand SelectLoansAssociation(int id_msg)
        {
            string commandText = @"SELECT * FROM vwLoanAssociation WHERE messageId=@id_msg";
            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@id_msg", id_msg));
            return PrepareCommand(commandText, parameters);
        }

	    public virtual IDbCommand SelectAwmMessages(int id_acct, int id_msg, long id_folder_db)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
{1}WHERE id_acct=@id_acct AND id_folder_db=@id_folder_db AND id_msg=@id_msg ORDER BY id_msg ASC",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));
			parameters.Add(CreateParameter("@id_msg", id_msg));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmMessages(int id_acct, long id_folder_db, long last_int_uid)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
{1}WHERE id_acct=@id_acct AND id_folder_db=@id_folder_db AND int_uid<=@last_int_uid ORDER BY id_msg ASC",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));
			parameters.Add(CreateParameter("@last_int_uid", last_int_uid));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmMessages(int id_acct, long id_folder_db, string[] str_uids)
		{
			StringBuilder sb = new StringBuilder();
			foreach (string str_uid in str_uids)
			{
				sb.AppendFormat("'{0}',", EncodeQuotes(str_uid));
			}
			sb.Remove(sb.Length - 1, 1);
			
			string commandText = string.Format(@" SELECT * FROM {0}
{2}WHERE id_acct=@id_acct AND id_folder_db=@id_folder_db AND str_uid IN ({1}) ORDER BY id_msg ASC
", 
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				sb.ToString(),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmMessages(int id_acct, long id_folder_db, int pageNumber, int msgsOnPage, string order, bool asc)
		{
			string commandText = string.Format(@"SELECT TOP {1} * FROM {0}
{5}WHERE id_acct=@id_acct AND id_folder_db=@id_folder_db AND id_msg NOT IN
 (SELECT TOP {2} id_msg FROM {0} {5}WHERE id_acct=@id_acct AND id_folder_db=@id_folder_db ORDER BY {3} {4})
ORDER BY {3} {4}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				msgsOnPage, // 1
				(pageNumber > 0) ? (pageNumber - 1) * msgsOnPage : 0, // 2
				order, // 3
				(asc) ? "ASC" : "DESC", // 4
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmMessages(int id_acct, int pageNumber, int msgsOnPage, string condition, FolderCollection folders, bool inHeadersOnly, string order, bool asc)
		{
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
				bodyLike = string.Format(" OR body_text LIKE '%{0}%'", EscapeWildcardCharacters(EncodeQuotes(condition)));
			}
			string commandText = string.Format(@"SELECT messages.*, folders.name AS folder_name FROM 
(SELECT TOP {1} * FROM {0} WHERE id_acct=@id_acct AND id_folder_db IN ({5}) AND 
(from_msg LIKE '%{6}%' OR to_msg LIKE '%{6}%' OR cc_msg LIKE '%{6}%' OR bcc_msg
LIKE '%{6}%' OR subject LIKE '%{6}%'{8})
	AND id_msg NOT IN 
	(SELECT TOP {2} id_msg FROM {0} 
	WHERE id_acct=@id_acct AND id_folder_db IN ({5}) AND 
	(from_msg LIKE '%{6}%' OR to_msg LIKE '%{6}%' OR cc_msg LIKE '%{6}%' OR bcc_msg
	LIKE '%{6}%' OR subject LIKE '%{6}%'{8})
	ORDER BY {3} {4})
ORDER BY {3} {4}) AS messages
INNER JOIN {7} AS folders
ON messages.id_folder_db = folders.id_folder",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				msgsOnPage,
				(pageNumber > 0) ? (pageNumber - 1) * msgsOnPage : 0,
				order,
				(asc) ? "ASC" : "DESC",
				folder_ids,
				EscapeWildcardCharacters(EncodeQuotes(condition)),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),
				bodyLike);
				//_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmMessages(int id_acct, string condition, FolderCollection folders, bool inHeadersOnly)
		{
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
				bodyLike = string.Format(" OR body_text LIKE '%{0}%'", EscapeWildcardCharacters(EncodeQuotes(condition)));
			}
			string commandText = string.Format(@"SELECT messages.*, folders.name AS folder_name FROM 
(SELECT * FROM {0} WHERE id_acct=@id_acct AND id_folder_db IN ({3}) AND
(from_msg LIKE '%{1}%' OR to_msg LIKE '%{1}%' OR cc_msg LIKE '%{1}%' OR bcc_msg
LIKE '%{1}%' OR subject LIKE '%{1}%'{4})) AS messages
INNER JOIN {2} AS folders
ON messages.id_folder_db = folders.id_folder",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),//0
				EscapeWildcardCharacters(EncodeQuotes(condition)),//1
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),//2
				folder_ids,//3
				bodyLike);
				// _nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmMessagesBody(int id_acct, int id_msg)
		{
			string commandText = string.Format(@"SELECT msg FROM {0} 
{1}WHERE id_acct=@id_acct AND id_msg=@id_msg",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages_body),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_msg", id_msg));

			return PrepareCommand(commandText, parameters);
		}


		public virtual IDbCommand UpdateAUser(int id_user, bool deleted)
		{
			string commandText = string.Format("UPDATE {0} SET deleted=@deleted WHERE id_user=@id_user",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.a_users));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@deleted", deleted));
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand UpdateAwmAccounts(int id_acct, int id_user, bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol, string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host, string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm, bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode, short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt, string delimiter, long mailbox_size)
		{
			string commandText = string.Format(@"UPDATE {0}
SET id_user=@id_user, def_acct=@def_acct, deleted=@deleted, email=@email, mail_protocol=@mail_protocol,
 mail_inc_host=@mail_inc_host, mail_inc_login=@mail_inc_login, mail_inc_pass=@mail_inc_pass,
 mail_inc_port=@mail_inc_port, mail_out_host=@mail_out_host, mail_out_login=@mail_out_login,
 mail_out_pass=@mail_out_pass, mail_out_port=@mail_out_port, mail_out_auth=@mail_out_auth, friendly_nm=@friendly_nm,
 use_friendly_nm=@use_friendly_nm, def_order=@def_order, getmail_at_login=@getmail_at_login, mail_mode=@mail_mode,
 mails_on_server_days=@mails_on_server_days, signature=@signature, signature_type=@signature_type,
 signature_opt=@signature_opt, delimiter=@delimiter, mailbox_size=@mailbox_size
WHERE id_acct=@id_acct",
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
			parameters.Add(CreateParameter("@mail_out_login", (mail_out_login.Length > 255) ? mail_out_host.Substring(0, 255) : mail_out_login));
			parameters.Add(CreateParameter("@mail_out_pass", (mail_out_pass.Length > 255) ? mail_out_pass.Substring(0, 255) : mail_out_pass));
			parameters.Add(CreateParameter("@mail_out_port", mail_out_port));
			parameters.Add(CreateParameter("@mail_out_auth", mail_out_auth));
			parameters.Add(CreateParameter("@friendly_nm", (friendly_nm.Length > 200) ? friendly_nm.Substring(0, 200) : friendly_nm));
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
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand UpdateAwmAddrBook(long id_addr, int id_user, string h_email, string fullname, string notes, bool use_friendly_nm, string h_street, string h_city, string h_state, string h_zip, string h_country, string h_phone, string h_fax, string h_mobile, string h_web, string b_email, string b_company, string b_street, string b_city, string b_state, string b_zip, string b_country, string b_job_title, string b_department, string b_office, string b_phone, string b_fax, string b_web, byte birthday_day, byte birthday_month, short birthday_year, string other_email, short primary_email, long id_addr_prev, bool tmp, int use_frequency, bool auto_create)
		{
			string commandText = string.Format(@"UPDATE {0}
SET id_user=@id_user, h_email=@h_email, fullname=@fullname, notes=@notes, use_friendly_nm=@use_friendly_nm,
 h_street=@h_street, h_city=@h_city, h_state=@h_state, h_zip=@h_zip, h_country=@h_country,
 h_phone=@h_phone, h_fax=@h_fax, h_mobile=@h_mobile, h_web=@h_web, b_email=@b_email,
 b_company=@b_company, b_street=@b_street, b_city=@b_city, b_state=@b_state, b_zip=@b_zip,
 b_country=@b_country, b_job_title=@b_job_title, b_department=@b_department, b_office=@b_office,
 b_phone=@b_phone, b_fax=@b_fax, b_web=@b_web, birthday_day=@birthday_day,
 birthday_month=@birthday_month, birthday_year=@birthday_year, other_email=@other_email,
 primary_email=@primary_email, id_addr_prev=@id_addr_prev, tmp=@tmp, use_frequency=@use_frequency,
 auto_create=@auto_create
WHERE id_addr=@id_addr",
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
			parameters.Add(CreateParameter("@id_addr", id_addr));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand UpdateAwmAddrGroups(int id_group, int id_user, string group_nm, string phone, string fax, string web, bool organization, int use_frequency, string email, string company, string street, string city, string state, string zip, string country)
		{
			string commandText = string.Format(@"UPDATE {0}
SET id_user=@id_user, group_nm=@group_nm, phone=@phone, fax=@fax, web=@web, 
organization=@organization, use_frequency=@use_frequency, email=@email, company=@company, street=@street, 
city=@city, state=@state, zip=@zip, country=@country
WHERE id_group=@id_group",
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
			parameters.Add(CreateParameter("@id_group", id_group));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand UpdateAwmMessages(
            long id, 
            int id_msg, 
            int id_acct, 
            long id_folder_srv, 
            long id_folder_db, 
            string str_uid, 
            long int_uid, 
            string from_msg, 
            string to_msg, 
            string cc_msg, 
            string bcc_msg, 
            string subject,
            string loanapplicants,
            string loanappids,
            DateTime msg_date, 
            bool attachments, 
            long size, 
            bool seen, 
            bool flagged, 
            byte priority, 
            bool downloaded, 
            bool x_spam, 
            bool rtl, 
            bool deleted, 
            bool is_full, 
            bool replied, 
            bool forwarded, 
            byte flags, 
            string body_text, 
            bool grayed, 
            int charset)
		{
			if (str_uid != null)
			{
				if (str_uid.Length > 255)
				{
					str_uid = str_uid.Substring(0, 255);
					is_full = false;
				}
			}

			if (from_msg != null)
			{
				if (from_msg.Length > 255)
				{
					from_msg = from_msg.Substring(0, 255);
					is_full = false;
				}
			}

			if (to_msg != null)
			{
				if (to_msg.Length > 255)
				{
					to_msg = to_msg.Substring(0, 255);
					is_full = false;
				}
			}

			if (cc_msg != null)
			{
				if (cc_msg.Length > 255)
				{
					cc_msg = cc_msg.Substring(0, 255);
					is_full = false;
				}
			}

			if (bcc_msg != null)
			{
				if (bcc_msg.Length > 255)
				{
					bcc_msg = bcc_msg.Substring(0, 255);
					is_full = false;
				}
			}

			if (subject != null)
			{
				if (subject.Length > 255)
				{
					subject = subject.Substring(0, 255);
					is_full = false;
				}
			}

			if (loanapplicants != null)
			{
				if (loanapplicants.Length > 255)
				{
					loanapplicants = loanapplicants.Substring(0, 255);
					is_full = false;
				}
			}

            if (loanappids != null)
            {
                if (loanappids.Length > 255)
                {
                    loanappids = loanappids.Substring(0, 255);
                    is_full = false;
                }
            }


			// downloaded must updated in 'SaveMessage'
			string commandText = string.Format(@"UPDATE {0}
SET id_msg=@id_msg, id_acct=@id_acct, id_folder_srv=@id_folder_srv, id_folder_db=@id_folder_db,
 str_uid=@str_uid, int_uid=@int_uid, from_msg=@from_msg, to_msg=@to_msg, cc_msg=@cc_msg, bcc_msg=@bcc_msg,
 subject=@subject, loanapplicants=@loanapplicants, loanappids=@loanappids, msg_date=@msg_date, attachments=@attachments, [size]=@size, seen=@seen, flagged=@flagged,
 priority=@priority, x_spam=@x_spam, rtl=@rtl, deleted=@deleted, is_full=@is_full,
 replied=@replied, forwarded=@forwarded, flags=@flags, body_text=@body_text, grayed=@grayed, charset=@charset
WHERE id=@id",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_msg", id_msg));
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_srv", id_folder_srv));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));
			parameters.Add(CreateParameter("@str_uid", str_uid));
			parameters.Add(CreateParameter("@int_uid", int_uid));
			parameters.Add(CreateParameter("@from_msg", from_msg));
			parameters.Add(CreateParameter("@to_msg", to_msg));
			parameters.Add(CreateParameter("@cc_msg", cc_msg));
			parameters.Add(CreateParameter("@bcc_msg", bcc_msg));
			parameters.Add(CreateParameter("@subject", subject));
            parameters.Add(CreateParameter("@loanapplicants", loanapplicants));
            parameters.Add(CreateParameter("@loanappids", loanappids));
			parameters.Add(CreateParameter("@msg_date", msg_date));
			parameters.Add(CreateParameter("@attachments", attachments));
			parameters.Add(CreateParameter("@size", size));
			parameters.Add(CreateParameter("@seen", seen));
			parameters.Add(CreateParameter("@flagged", flagged));
			parameters.Add(CreateParameter("@priority", priority));
			//parameters.Add(CreateParameter("@downloaded", downloaded));
			parameters.Add(CreateParameter("@x_spam", x_spam));
			parameters.Add(CreateParameter("@rtl", rtl));
			parameters.Add(CreateParameter("@deleted", deleted));
			parameters.Add(CreateParameter("@is_full", is_full));
			parameters.Add(CreateParameter("@replied", replied));
			parameters.Add(CreateParameter("@forwarded", forwarded));
			parameters.Add(CreateParameter("@flags", flags));
			parameters.Add(CreateParameter("@body_text", body_text));
			parameters.Add(CreateParameter("@grayed", grayed));
			parameters.Add(CreateParameter("@charset", charset));
			parameters.Add(CreateParameter("@id", id));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand UpdateAwmMessagesFlags(int id_acct, long id_folder_db, bool allMessages, int[] ids, SystemMessageFlags flags, MessageFlagAction flagsAction)
		{
			string flagsField = string.Empty;
			ArrayList flagsStrs = new ArrayList();
			string uidsStr = string.Empty;
			if (!allMessages)
			{
				string strIn = NumberArrayToString(ids);
				uidsStr = string.Format(" AND id_msg IN ({0})", strIn);
			}

			switch (flagsAction)
			{
				case MessageFlagAction.Add:
				{
					if ((flags & SystemMessageFlags.Seen) > 0) flagsStrs.Add("seen=1");
					if ((flags & SystemMessageFlags.Flagged) > 0) flagsStrs.Add("flagged=1");
					if ((flags & SystemMessageFlags.Deleted) > 0) flagsStrs.Add("deleted=1");
					if ((flags & SystemMessageFlags.Answered) > 0) flagsStrs.Add("replied=1");
					flagsField = "(flags | @flags)";
					break;
				}
				case MessageFlagAction.Remove:
				{
					if ((flags & SystemMessageFlags.Seen) > 0) flagsStrs.Add("seen=0");
					if ((flags & SystemMessageFlags.Flagged) > 0) flagsStrs.Add("flagged=0");
					if ((flags & SystemMessageFlags.Deleted) > 0) flagsStrs.Add("deleted=0");
					if ((flags & SystemMessageFlags.Answered) > 0) flagsStrs.Add("replied=0");
					flagsField = "(flags & ~@flags)";
					break;
				}
				case MessageFlagAction.Replace:
				{
					flagsStrs.Add(string.Format("seen={0}", ((flags & SystemMessageFlags.Seen) > 0) ? "1" : "0"));
					flagsStrs.Add(string.Format("flagged={0}", ((flags & SystemMessageFlags.Flagged) > 0) ? "1" : "0"));
					flagsStrs.Add(string.Format("deleted={0}", ((flags & SystemMessageFlags.Deleted) > 0) ? "1" : "0"));
					flagsStrs.Add(string.Format("replied={0}", ((flags & SystemMessageFlags.Answered) > 0) ? "1" : "0"));
					flagsField = "@flags";
					break;
				}
			}

			string commandText = string.Format(@"UPDATE {0}
SET {2},flags={3}
WHERE id_acct=@id_acct AND id_folder_db=@id_folder_db {1}",
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

		public virtual IDbCommand UpdateAwmMessagesFolders(int id_acct, int[] ids, long id_folder_db, long id_folder_db_new)
		{
			string strIn = NumberArrayToString(ids);
			
			string commandText = string.Format(@"UPDATE {0} SET id_folder_db=@id_folder_db_new
WHERE id_acct=@id_acct AND id_folder_db=@id_folder_db AND id_msg IN ({1})
", 
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				strIn); // 4

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder_db_new", id_folder_db_new));
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand UpdateAwmColumns(int id_column, int id_user, int value)
		{
			string commandText = string.Format(@"UPDATE {0}
SET column_value=@value WHERE id_column=@id_column AND id_user=@id_user
",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_columns));

			ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@value", value));
			parameters.Add(CreateParameter("@id_column", id_column));
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand UpdateAwmDomains(int id, string name, short mail_protocol, string mail_inc_host, int mail_inc_port, string mail_out_host, int mail_out_port, bool mail_out_auth)
		{
			string commandText = string.Format(@"UPDATE {0}
SET name=@name, mail_protocol=@mail_protocol, mail_inc_host=@mail_inc_host, mail_inc_port=@mail_inc_port,
 mail_out_host=@mail_out_host, mail_out_port=@mail_out_port, mail_out_auth=@mail_out_auth
WHERE id_domain=@id_domain
",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_domain", id));
			parameters.Add(CreateParameter("@name", name));
			parameters.Add(CreateParameter("@mail_protocol", mail_protocol));
			parameters.Add(CreateParameter("@mail_inc_host", mail_inc_host));
			parameters.Add(CreateParameter("@mail_inc_port", mail_inc_port));
			parameters.Add(CreateParameter("@mail_out_host", mail_out_host));
			parameters.Add(CreateParameter("@mail_out_port", mail_out_port));
			parameters.Add(CreateParameter("@mail_out_auth", mail_out_auth));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand UpdateAwmFilters(int id_filter, int id_acct, byte field, byte condition, string filter, byte action, long id_folder)
		{
			string commandText = string.Format(@"UPDATE {0}
SET id_acct=@id_acct, field=@field, condition=@condition, filter=@filter, action=@action,
 id_folder=@id_folder
WHERE id_filter=@id_filter
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

		public virtual IDbCommand UpdateAwmFolders(int id_acct, long id_folder, short type, string name, string full_path, byte sync_type, bool hide, short fld_order)
		{
			string commandText = string.Format(@" UPDATE {0}
SET type=@type, name=@name, full_path=@full_path, sync_type=@sync_type, hide=@hide, fld_order=@fld_order
WHERE id_acct=@id_acct AND id_folder=@id_folder
",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders)); //8

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@type", type));
			parameters.Add(CreateParameter("@name", (name.Length > 100) ? name.Substring(0, 100) : name));
			parameters.Add(CreateParameter("@full_path", (full_path.Length > 255) ? full_path.Substring(0, 255) : full_path));
			parameters.Add(CreateParameter("@sync_type", sync_type));
			parameters.Add(CreateParameter("@hide", hide));
			parameters.Add(CreateParameter("@fld_order", fld_order));
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder", id_folder));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand UpdateAwmSenders(int id_user, string email, byte safety)
		{
			string commandText = string.Format(@"UPDATE {0} SET
email=@email, safety=@safety
WHERE id_user=@id_user",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_senders));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@email", email));
			parameters.Add(CreateParameter("@safety", safety));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand UpdateAwmSettings(int id, int id_user, int msgs_per_page, bool white_listing, bool x_spam, DateTime last_login, int logins_count, string def_skin, string def_lang, int def_charset_inc, short def_timezone, string def_date_fmt, bool hide_folders, long mailbox_limit, bool allow_change_settings, bool allow_dhtml_editor, bool allow_direct_mode, bool hide_contacts, int db_charset, short horiz_resizer, short vert_resizer, byte mark, byte reply, short contacts_per_page, int def_charset_out, byte view_mode, TimeFormats time_fmt)
		{
			string commandText = string.Format(@"UPDATE {0} SET
id_user=@id_user, msgs_per_page=@msgs_per_page, white_listing=@white_listing, x_spam=@x_spam, last_login=@last_login,
logins_count=@logins_count, def_skin=@def_skin, def_lang=@def_lang, def_charset_inc=@def_charset_inc, def_timezone=@def_timezone,
def_date_fmt=@def_date_fmt, hide_folders=@hide_folders, mailbox_limit=@mailbox_limit, allow_change_settings=@allow_change_settings,
allow_dhtml_editor=@allow_dhtml_editor, allow_direct_mode=@allow_direct_mode, hide_contacts=@hide_contacts, db_charset=@db_charset,
horiz_resizer=@horiz_resizer, vert_resizer=@vert_resizer, mark=@mark, reply=@reply, contacts_per_page=@contacts_per_page,
def_charset_out=@def_charset_out, view_mode=@view_mode
WHERE id_setting=@id_setting",
			
            EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_settings));

			int maxDateFmtLength = 20 - Constants.timeFormat.Length;
			def_date_fmt = (def_date_fmt.Length > maxDateFmtLength) ? def_date_fmt.Substring(0, maxDateFmtLength) : def_date_fmt;
            def_date_fmt = (time_fmt == TimeFormats.F12) ? def_date_fmt + Constants.timeFormat : def_date_fmt;

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
			parameters.Add(CreateParameter("@def_timezone", def_timezone));
			parameters.Add(CreateParameter("@def_date_fmt", def_date_fmt));
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
			parameters.Add(CreateParameter("@def_charset_out", def_charset_out));
			parameters.Add(CreateParameter("@view_mode", view_mode));
			parameters.Add(CreateParameter("@id_setting", id));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectMaxFolderOrder(int id_acct, long id_parent)
		{
			string commandText = string.Format(@"SELECT MAX(fld_order) FROM {0}
{1}WHERE id_parent=@id_parent AND id_acct=@id_acct",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_parent", id_parent));
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmMessagesOlderThanXDays(int id_acct, long id_folder, int daysCount)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
{2}WHERE id_acct=@id_acct AND id_folder_db=@id_folder_db AND DATEDIFF(dd, msg_date, GETDATE()) > {1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				daysCount.ToString(CultureInfo.InvariantCulture),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmReads(int id_acct)
		{
			string commandText = string.Format(@"SELECT str_uid FROM {0}
{1}WHERE id_acct=@id_acct ORDER BY id_read",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_reads),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmSendersSafety(int id_user, string email)
		{
			string commandText = string.Format(@"SELECT safety FROM {0}
{2}WHERE id_user=@id_user AND email LIKE '{1}'",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_senders),
				EncodeQuotes(email),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmMessagesMarkAsDelete(int id_acct, long id_folder)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
{1}WHERE id_acct=@id_acct AND id_folder_db=@id_folder AND deleted=1",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder", id_folder));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmAddrBook(int id_user, long id_addr)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
{1}WHERE id_user=@id_user AND id_addr=@id_addr",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@id_addr", id_addr));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmAddrBook(int id_user, int id_group)
		{
			string commandText = string.Format(@"SELECT * FROM {0} AS contacts
INNER JOIN
{1} AS groups_contacts
ON groups_contacts.id_addr=contacts.id_addr
WHERE groups_contacts.id_group=@id_group AND contacts.id_user=@id_user",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts));
				// _nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_group", id_group));
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmAddrBook(int id_user, string email)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
{2}WHERE id_user=@id_user AND (h_email LIKE '{1}' OR b_email LIKE '{1}' OR other_email LIKE '{1}')",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
				EncodeQuotes(email),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			//parameters.Add(CreateParameter("@email", email));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmAddrBookGroups(int id_user, int contactsOnPage, int page, int sort_field, int sort_order)
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

			string commandText = string.Format(@"SELECT TOP {2} * FROM (SELECT use_frequency, id_addr AS uniq_id, id_addr AS id, fullname AS name,
CASE primary_email
	WHEN 0 THEN h_email
	WHEN 1 THEN b_email
	WHEN 2 THEN other_email
END  AS email, 0 AS is_group FROM {0}
WHERE id_user=@id_user
UNION
SELECT use_frequency, -id_group AS uniq_id, id_group AS id, group_nm AS name, '' AS email, 1 AS is_group FROM {1}
WHERE id_user=@id_user) as union_table 
WHERE uniq_id NOT IN (SELECT TOP {3} uniq_id FROM (SELECT id_addr AS uniq_id, id_addr AS id, fullname AS name,
CASE primary_email
	WHEN 0 THEN h_email
	WHEN 1 THEN b_email
	WHEN 2 THEN other_email
END  AS email, 0 AS is_group FROM {0}
WHERE id_user=@id_user
UNION
SELECT -id_group AS uniq_id, id_group AS id, group_nm AS name, '' AS email, 1 AS is_group FROM {1}
WHERE id_user=@id_user) as union_table2
ORDER BY {4} {5})
ORDER BY {4} {5}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),//0
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),//1
				contactsOnPage,//2
				(page > 0) ? (page - 1) * contactsOnPage : 0,//3
				filter,//4
				(sort_order == 0) ? "ASC" : "DESC");//5
				// _nolock);//6

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SearchAwmAddrBookGroups(int id_user, int contactsOnPage, int page, string sort_field, int sort_order, int id_group, string look_for, int look_for_type)
		{
			string lookForSearchCondition = string.Empty;
			if ((look_for != null) && (look_for.Length > 0))
			{
				lookForSearchCondition = EscapeWildcardCharacters(EncodeQuotes(look_for));
			}

			string groupSearchCondition = string.Empty;
			if (id_group >= 0)
			{
                groupSearchCondition = string.Format(@"(SELECT {0}.* FROM {0} INNER JOIN {1} ON {0}.id_addr={1}.id_addr WHERE {1}.id_group=@id_group) AS table_join",
					EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
					EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts));
					// _nolock);
			}
			else
			{
				groupSearchCondition = EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book);
			}

			string commandText = string.Format(@"SELECT TOP {2} * FROM (SELECT id_addr AS uniq_id, id_addr AS id, fullname AS name,
CASE primary_email
	WHEN 0 THEN h_email
	WHEN 1 THEN b_email
	WHEN 2 THEN other_email
END  AS email, 0 AS is_group, use_frequency AS frequency FROM {7}
WHERE id_user=@id_user AND (fullname LIKE '{6}' OR h_email LIKE '{6}' OR b_email LIKE '{6}' OR other_email LIKE '{6}')
UNION
SELECT -id_group AS uniq_id, id_group AS id, group_nm AS name, email AS email, 1 AS is_group, use_frequency AS frequency FROM {1}
WHERE id_user=@id_user AND (group_nm LIKE '{6}' OR email LIKE '{6}')) as union_table 
WHERE uniq_id NOT IN (SELECT TOP {3} uniq_id FROM (SELECT id_addr AS uniq_id, id_addr AS id, fullname AS name,
CASE primary_email
	WHEN 0 THEN h_email
	WHEN 1 THEN b_email
	WHEN 2 THEN other_email
END  AS email, 0 AS is_group, use_frequency AS frequency FROM {7}
WHERE id_user=@id_user AND (fullname LIKE '{6}' OR h_email LIKE '{6}' OR b_email LIKE '{6}' OR other_email LIKE '{6}')
UNION
SELECT -id_group AS uniq_id, id_group AS id, group_nm AS name, email AS email, 1 AS is_group, use_frequency AS frequency FROM {1}
WHERE id_user=@id_user AND (group_nm LIKE '{6}' OR email LIKE '{6}')) as union_table2
ORDER BY {4} {5})
ORDER BY {4} {5}
",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book), // 0
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups), // 1
				contactsOnPage, // 2
				(page > 0) ? (page - 1) * contactsOnPage : 0, // 3
				sort_field, // 4
				(sort_order == 0) ? "ASC" : "DESC", // 5
				(look_for_type == 0) ? "%" + lookForSearchCondition + "%" : lookForSearchCondition + "%", // 6
				groupSearchCondition);
				// _nolock); // 8

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@id_group", id_group));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmAddrGroups(int id_user, int id_group)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
{1}WHERE id_user=@id_user AND id_group=@id_group",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@id_group", id_group));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmAddrGroups(int id_user, long id_addr)
		{
			string commandText = string.Format(@"SELECT * FROM {0} AS groups_contacts
INNER JOIN
{1} AS groups
ON groups_contacts.id_group=groups.id_group
WHERE groups_contacts.id_addr=@id_addr AND groups.id_user=@id_user",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups));
				// _nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_addr", id_addr));
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmAddrGroups(int id_user)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
{1}WHERE id_user=@id_user",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand InsertIntoAwmAddrGroupsContacts(long id_addr, int id_group)
		{
			string commandText = string.Format(@"INSERT INTO {0}(id_addr, id_group)
VALUES(@id_addr, @id_group)",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_addr", id_addr));
			parameters.Add(CreateParameter("@id_group", id_group));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand SelectAwmMessagesCount(int id_acct, int pageNumber, int msgsOnPage, string condition, FolderCollection folders, bool inHeadersOnly, string order, bool asc)
		{
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
				bodyLike = string.Format(" OR body_text LIKE '%{0}%'", EscapeWildcardCharacters(EncodeQuotes(condition)));
			}
			string commandText = string.Format(@"SELECT COUNT(*) FROM {0}
WHERE id_acct=@id_acct AND id_folder_db IN ({1}) AND 
(from_msg LIKE '%{2}%' OR to_msg LIKE '%{2}%' OR cc_msg LIKE '%{2}%' OR bcc_msg
LIKE '%{2}%' OR subject LIKE '%{2}%'{3});",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),//0
				folder_ids,//1
				EscapeWildcardCharacters(EncodeQuotes(condition)),//2
				bodyLike);
				// _nolock);//4

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public virtual IDbCommand InsertIntoAwmTemp(int id_acct, string data_val)
		{
			string commandText = string.Format(@"INSERT INTO {0}(id_acct, data_val)
VALUES(@id_acct, @data_val)",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_temp));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@data_val", data_val));

			return PrepareCommand(commandText, parameters);
		}

		public IDbCommand DeleteFromAwmTemp(int id_acct, long id_temp)
		{
			string whereCondition = string.Empty;
			if (id_acct > 0)
			{
				whereCondition += string.Format(" id_acct={0} ", id_acct);
			}
			if (id_temp > 0)
			{
				whereCondition += string.Format(" {0}id_temp={1} ",
					(whereCondition.Length > 0) ? "AND " : string.Empty,
					id_temp);
			}
			string commandText = string.Format(@"DELETE FROM {0}
WHERE{1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_temp),
				whereCondition);

			return PrepareCommand(commandText, null);
		}

		public IDbCommand SelectAwmTemp(int id_acct, long id_temp, string data_val)
		{
			string whereCondition = string.Empty;
			if (id_acct > 0)
			{
				whereCondition += string.Format(" id_acct={0} ", id_acct);
			}
			if (id_temp > 0)
			{
				whereCondition += string.Format(" {0}id_temp={1} ",
					(whereCondition.Length > 0) ? "AND " : string.Empty,
					id_temp);
			}
			if ((data_val != null) && (data_val.Length > 0))
			{
				whereCondition += string.Format(" {0}data_val LIKE '{1}' ",
					(whereCondition.Length > 0) ? "AND " : string.Empty,
					data_val);
			}

			string commandText = string.Format(@"SELECT * FROM {0}
{2}WHERE{1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_temp),
				whereCondition,
				_nolock);

			ArrayList parameters = new ArrayList();

			return PrepareCommand(commandText, parameters);
		}

        public IDbCommand SelectAwmDomain(int id_domain)
        {
            string commandText = string.Format(@"SELECT * FROM {0}
WHERE id_domain WHERE id_domain = {1}",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains),
                id_domain);

            return PrepareCommand(commandText, null);
        }

        public IDbCommand SelectAwmDomains(int page, int pageSize, string orderBy, bool asc, string searchCondition)
		{
			string whereConditionInternal = string.Empty;
			string whereConditionExternal = string.Empty;
			if ((searchCondition != null) && (searchCondition.Length > 0))
			{
				whereConditionInternal = string.Format(@"WHERE name LIKE ('%{0}%') OR mail_inc_host LIKE ('%{0}%') OR mail_out_host LIKE ('%{0}%')", EscapeWildcardCharacters(EncodeQuotes(searchCondition)));
				whereConditionExternal = string.Format(@"AND (name LIKE ('%{0}%') OR mail_inc_host LIKE ('%{0}%') OR mail_out_host LIKE ('%{0}%'))", EscapeWildcardCharacters(EncodeQuotes(searchCondition)));
			}

			string commandText = string.Format(@"SELECT TOP {1} * FROM {0}
WHERE id_domain NOT IN
(SELECT TOP {2} id_domain FROM {0} {5} ORDER BY {3} {4}) {6}
ORDER BY {3} {4}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains),
				pageSize,
				(page > 0) ? (page - 1) * pageSize : 0,
				orderBy,
				(asc) ? "ASC" : "DESC",
				whereConditionInternal,
				whereConditionExternal);

			return PrepareCommand(commandText, null);
		}

		public IDbCommand SelectAwmDomainsCount(string searchCondition)
		{
			string whereCondition = string.Empty;
			if ((searchCondition != null) && (searchCondition.Length > 0))
			{
				whereCondition = string.Format(@"WHERE name LIKE ('%{0}%') OR mail_inc_host LIKE ('%{0}%') OR mail_out_host LIKE ('%{0}%')", EscapeWildcardCharacters(EncodeQuotes(searchCondition)));
			}

			string commandText = string.Format(@"SELECT COUNT(*) FROM {0} {1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains),
				whereCondition);

			return PrepareCommand(commandText, null);
		}
	}

	public class MsSqlCommandCreator : CommandCreator
	{
		public MsSqlCommandCreator(SqlConnection connection, SqlCommand command) : base(connection, command, true){}

		protected override IDataParameter CreateParameter(string paramName, object paramValue)
		{
			Log.WriteLine("CreateParameter", string.Format("{0}='{1}'", paramName, paramValue));
			return new SqlParameter(paramName, paramValue);
		}

		public override string SelectIdentity()
		{
			return @"SELECT SCOPE_IDENTITY();";
		}

	}

}
