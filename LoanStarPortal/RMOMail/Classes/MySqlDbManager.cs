using System;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Globalization;
using System.Collections;
using System.Text;
using MailBee.Mime;
using MailBee.ImapMail;

namespace WebMailPro
{
	public class MySqlDbManager : DbManager
	{
		public MySqlDbManager() : this(null) {}

		public MySqlDbManager(Account acct) : base(acct)
		{
			_connection = new OdbcConnection();
			_commandCreator = new MySqlCommandCreator(_connection as OdbcConnection, new OdbcCommand());
		}

		public override Account CreateAccount(int id_user, bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol, string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host, string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm, bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode, short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt, string delimiter, long mailbox_size)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.InsertIntoAwmAccounts(id_user, def_acct, deleted, email, mail_protocol, mail_inc_host, mail_inc_login, Utils.EncodePassword(mail_inc_pass), mail_inc_port, mail_out_host, mail_out_login, Utils.EncodePassword(mail_out_pass), mail_out_port, mail_out_auth, friendly_nm, use_friendly_nm, def_order, getmail_at_login, mail_mode, mails_on_server_days, signature, signature_type, signature_opt, delimiter, mailbox_size);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command.CommandText = _commandCreator.SelectIdentity();
				object obj = ExecuteScalarCommand(command);
				if (obj != null)
				{
					myTrans.Commit();
					return new Account(Convert.ToInt32(obj), def_acct, deleted, email, mail_protocol, mail_inc_host, mail_inc_login, mail_inc_pass, mail_inc_port, mail_out_host, mail_out_login, mail_out_pass, mail_out_port, mail_out_auth, friendly_nm, use_friendly_nm, def_order, getmail_at_login, mail_mode, mails_on_server_days, signature, signature_type, signature_opt, delimiter, mailbox_size);
				}
				else
				{
					throw new WebMailDatabaseException("Can't create account");
				}
			}
			catch(Exception ex)
			{
				if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public override User CreateUser(bool deleted)
		{
			IDbTransaction myTrans = null;
			User result = new User();
			result.Deleted = deleted;
			UserSettings settings = new UserSettings();

			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.InsertIntoAUsers(deleted);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command.CommandText = _commandCreator.SelectIdentity();
				object obj = ExecuteScalarCommand(command);
				if (obj != null)
				{
					result.ID = Convert.ToInt32(obj);
				}
				else
				{
					throw new WebMailDatabaseException("Can't create user");
				}

				command = _commandCreator.InsertIntoAwmSettings(result.ID, settings.MsgsPerPage, settings.WhiteListing, settings.XSpam, settings.LastLogin, settings.LoginsCount, settings.DefaultSkin, settings.DefaultLanguage, settings.DefaultCharsetInc, settings.DefaultTimeZone, settings.DefaultDateFormat, settings.HideFolders, settings.MailboxLimit, settings.AllowChangeSettings, settings.AllowDhtmlEditor, settings.AllowDirectMode, false, settings.DbCharset, settings.HorizResizer, settings.VertResizer, settings.Mark, settings.Reply, settings.ContactsPerPage, settings.DefaultCharsetOut, (byte)settings.ViewMode);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command.CommandText = _commandCreator.SelectIdentity();
				obj = ExecuteScalarCommand(command);
				if (obj == null)
				{
					settings = null;
				}
				else
				{
					settings.ID = Convert.ToInt32(obj);
				}
				myTrans.Commit();
				result.Settings = settings;
				return result;
			}
			catch(Exception ex)
			{
				if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public override Folder CreateFolder(int id_account, long id_parent, FolderType type, string name, string full_path, FolderSyncType sync_type, bool hide, short fld_order)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.InsertIntoAwmFolders(id_account, id_parent, type, Utils.ConvertToUtf7Modified(name) + "#", Utils.ConvertToUtf7Modified(full_path) + "#", sync_type, hide, fld_order);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command.CommandText = _commandCreator.SelectIdentity();
				object obj = ExecuteScalarCommand(command);
				long id_folder = (obj != null) ? Convert.ToInt64(obj) : -1;
				if (id_folder > -1)
				{
					command = _commandCreator.InsertIntoAwmFoldersTree(id_folder);
					command.Transaction = myTrans;
					command.ExecuteNonQuery();

					command = _commandCreator.InsertIntoAwmFoldersTree(id_folder, id_parent);
					command.Transaction = myTrans;
					command.ExecuteNonQuery();
				}
				else
				{
					throw new WebMailDatabaseException("Can't create folder");
				}

				myTrans.Commit();
				return new Folder(id_folder, id_account, id_parent, name, full_path, type, sync_type, hide, fld_order);
			}
			catch(Exception ex)
			{
				if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		protected override Account ReadAccount(IDataReader reader)
		{
			Account acct = new Account();
			DataTable schemaTable = reader.GetSchemaTable();

			foreach (DataRow row in schemaTable.Rows)
			{
				int index = (int)row[1];
				if (reader.IsDBNull(index)) continue;
				string fieldName = row[0] as string;
				if (fieldName != null)
				{
					switch (fieldName.ToLower(CultureInfo.InvariantCulture))
					{
						case "id_acct":
							acct.ID = Convert.ToInt32(reader.GetValue(index));
							break;
						case "id_user":
							acct.IDUser = Convert.ToInt32(reader.GetValue(index));
							break;
						case "def_acct":
							acct.DefaultAccount = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "deleted":
							acct.Deleted = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "email":
							acct.Email = Convert.ToString(reader.GetValue(index));
							break;
						case "mail_protocol":
							acct.MailIncomingProtocol = (IncomingMailProtocol) Convert.ToInt16(reader.GetValue(index));
							break;
						case "mail_inc_host":
							acct.MailIncomingHost = Convert.ToString(reader.GetValue(index));
							break;
						case "mail_inc_login":
							acct.MailIncomingLogin = Convert.ToString(reader.GetValue(index));
							break;
						case "mail_inc_pass":
							acct.MailIncomingPassword = Utils.DecodePassword(Convert.ToString(reader.GetValue(index)));
							break;
						case "mail_inc_port":
							acct.MailIncomingPort = Convert.ToInt32(reader.GetValue(index));
							break;
						case "mail_out_host":
							acct.MailOutgoingHost = Convert.ToString(reader.GetValue(index));
							break;
						case "mail_out_login":
							acct.MailOutgoingLogin = Convert.ToString(reader.GetValue(index));
							break;
						case "mail_out_pass":
							acct.MailOutgoingPassword = Utils.DecodePassword(Convert.ToString(reader.GetValue(index)));
							break;
						case "mail_out_port":
							acct.MailOutgoingPort = Convert.ToInt32(reader.GetValue(index));
							break;
						case "mail_out_auth":
							acct.MailOutgoingAuthentication = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "friendly_nm":
							acct.FriendlyName = Convert.ToString(reader.GetValue(index));
							break;
						case "use_friendly_nm":
							acct.UseFriendlyName = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "def_order":
							acct.DefaultOrder = (DefaultOrder) Convert.ToInt16(reader.GetValue(index));
							break;
						case "getmail_at_login":
							acct.GetMailAtLogin = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "mail_mode":
							acct.MailMode = (MailMode) Convert.ToInt16(reader.GetValue(index));
							break;
						case "mails_on_server_days":
							acct.MailsOnServerDays = Convert.ToInt16(reader.GetValue(index));
							break;
						case "signature":
							acct.Signature = Convert.ToString(reader.GetValue(index));
							break;
						case "signature_type":
							acct.SignatureType = (SignatureType) Convert.ToInt16(reader.GetValue(index));
							break;
						case "signature_opt":
							acct.SignatureOptions = (SignatureOptions) Convert.ToInt16(reader.GetValue(index));
							break;
						case "delimiter":
							acct.Delimiter = Convert.ToString(reader.GetValue(index));
							break;
						case "mailbox_size":
							acct.MailboxSize = Convert.ToInt64(reader.GetValue(index));
							break;
					}
				}
			}
			return acct;
		}

		protected override User ReadUser(IDataReader reader)
		{
			DataTable schemaTable = reader.GetSchemaTable();
			int id_user = -1;
			bool deleted = false;

			foreach (DataRow row in schemaTable.Rows)
			{
				int index = (int)row[1];
				if (reader.IsDBNull(index)) continue;
				string fieldName = row[0] as string;
				if (fieldName != null)
				{
					switch (fieldName.ToLower(CultureInfo.InvariantCulture))
					{
						case "users.id_user":
							id_user = Convert.ToInt32(reader.GetValue(index));
							break;
						case "id_user":
							id_user = Convert.ToInt32(reader.GetValue(index));
							break;
						case "deleted":
							deleted = Convert.ToBoolean(reader.GetValue(index));
							break;
					}
				}
			}
			UserSettings settings = ReadUserSettings(reader);
			return new User(id_user, deleted, settings);
		}

		protected override UserSettings ReadUserSettings(IDataReader reader)
		{
			DataTable schemaTable = reader.GetSchemaTable();
			UserSettings settings = new UserSettings();

			foreach (DataRow row in schemaTable.Rows)
			{
				int index = (int)row[1];
				if (reader.IsDBNull(index)) continue;
				string fieldName = row[0] as string;
				if (fieldName != null)
				{
					switch (fieldName.ToLower(CultureInfo.InvariantCulture))
					{
						case "id_setting":
							settings.ID = Convert.ToInt32(reader.GetValue(index));
							break;
						case "users.id_user": // for Access
							settings.IDUser = Convert.ToInt32(reader.GetValue(index));
							break;
						case "id_user":
							settings.IDUser = Convert.ToInt32(reader.GetValue(index));
							break;
						case "msgs_per_page":
							settings.MsgsPerPage = Convert.ToInt16(reader.GetValue(index));
							break;
						case "white_listing":
							settings.WhiteListing = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "x_spam":
							settings.XSpam = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "last_login":
							settings.LastLogin = reader.GetDateTime(index);
							break;
						case "logins_count":
							settings.LoginsCount = Convert.ToInt32(reader.GetValue(index));
							break;
						case "def_skin":
							settings.DefaultSkin = Convert.ToString(reader.GetValue(index));
							break;
						case "def_lang":
							settings.DefaultLanguage = Convert.ToString(reader.GetValue(index));
							break;
						case "def_charset_inc":
							settings.DefaultCharsetInc = Convert.ToInt32(reader.GetValue(index));
							break;
						case "def_charset_out":
							settings.DefaultCharsetOut = Convert.ToInt32(reader.GetValue(index));
							break;
						case "def_timezone":
							settings.DefaultTimeZone = Convert.ToInt16(reader.GetValue(index));
							break;
						case "def_date_fmt":
                            string tempDateFormat = Convert.ToString(reader.GetValue(index));
                            if (tempDateFormat.Length > 2 && tempDateFormat.Substring(tempDateFormat.Length - 2) == Constants.timeFormat)
                            {
                                settings.DefaultTimeFormat = TimeFormats.F12;
                                settings.DefaultDateFormat = tempDateFormat.Substring(0, tempDateFormat.Length - 2);
                            }
                            else
                            {
                                settings.DefaultTimeFormat = TimeFormats.F24;
                                settings.DefaultDateFormat = tempDateFormat;
                            }
							break;
						case "hide_folders":
							settings.HideFolders = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "mailbox_limit":
							settings.MailboxLimit = Convert.ToInt64(reader.GetValue(index));
							break;
						case "allow_change_settings":
							settings.AllowChangeSettings = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "allow_dhtml_editor":
							settings.AllowDhtmlEditor = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "allow_direct_mode":
							settings.AllowDirectMode = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "db_charset":
							try
							{
								settings.DbCharset = Convert.ToInt32(reader.GetValue(index));
							}
							catch
							{
								settings.DbCharset = Encoding.UTF8.CodePage;
							}
							break;
						case "horiz_resizer":
							settings.HorizResizer = Convert.ToInt16(reader.GetValue(index));
							break;
						case "vert_resizer":
							settings.VertResizer = Convert.ToInt16(reader.GetValue(index));
							break;
						case "mark":
							settings.Mark = Convert.ToByte(reader.GetValue(index));
							break;
						case "reply":
							settings.Reply = Convert.ToByte(reader.GetValue(index));
							break;
						case "contacts_per_page":
							settings.ContactsPerPage = Convert.ToInt16(reader.GetValue(index));
							break;
						case "view_mode":
							settings.ViewMode = (ViewMode)Convert.ToByte(reader.GetValue(index));
							break;
					}
				}
			}
			return settings;
		}

		protected override Filter ReadFilter(IDataReader reader)
		{
			DataTable schemaTable = reader.GetSchemaTable();
			Filter result = new Filter(_account);

			foreach (DataRow row in schemaTable.Rows)
			{
				int index = (int)row[1];
				if (reader.IsDBNull(index)) continue;
				string fieldName = row[0] as string;
				if (fieldName != null)
				{
					switch (fieldName.ToLower(CultureInfo.InvariantCulture))
					{
						case "id_filter":
							result.IDFilter = Convert.ToInt32(reader.GetValue(index));
							break;
						case "id_acct":
							result.IDAcct = Convert.ToInt32(reader.GetValue(index));
							break;
						case "field":
							result.Field = (FilterField)Convert.ToByte(reader.GetValue(index));
							break;
						case "condition":
							result.Condition = (FilterCondition)Convert.ToByte(reader.GetValue(index));
							break;
						case "filter":
							result.FilterStr = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "action":
							result.Action = (FilterAction)Convert.ToByte(reader.GetValue(index));
							break;
						case "id_folder":
							result.IDFolder = Convert.ToInt64(reader.GetValue(index));
							break;
					}
				}
			}
			return result;
		}

		protected override Folder ReadFolder(IDataReader reader)
		{
			DataTable schemaTable = reader.GetSchemaTable();
			Folder result = new Folder();

			foreach (DataRow row in schemaTable.Rows)
			{
				int index = (int)row[1];
				if (reader.IsDBNull(index)) continue;
				string fieldName = row[0] as string;
				if (fieldName != null)
				{
					switch (fieldName.ToLower(CultureInfo.InvariantCulture))
					{
						case "id_folder":
							result.ID = Convert.ToInt64(reader.GetValue(index));
							break;
						case "id_acct":
							result.IDAcct = Convert.ToInt32(reader.GetValue(index));
							break;
						case "id_parent":
							result.IDParent = Convert.ToInt64(reader.GetValue(index));
							break;
						case "type":
							result.Type = (FolderType) Convert.ToInt16(reader.GetValue(index));
							break;
						case "name":
							string folderName = Convert.ToString(reader.GetValue(index));
							if (folderName != null && folderName.Length > 0)
							{
								// remove trailing symbol
								folderName = folderName.Remove(folderName.Length - 1, 1);
							}
							result.Name = Utils.ConvertFromUtf7Modified(folderName);
							break;
						case "full_path":
							string folderFullName = Convert.ToString(reader.GetValue(index));
							if (folderFullName != null && folderFullName.Length > 0)
							{
								// remove trailing symbol
								folderFullName = folderFullName.Remove(folderFullName.Length - 1, 1);
							}
							result.FullPath = Utils.ConvertFromUtf7Modified(folderFullName);
							break;
						case "sync_type":
							result.SyncType = (FolderSyncType) Convert.ToInt16(reader.GetValue(index));
							break;
						case "hide":
							result.Hide = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "fld_order":
							result.FolderOrder = Convert.ToInt16(reader.GetValue(index));
							break;
					}
				}
			}
			return result;
		}

		protected override WebMailMessage ReadWebMailMessage(IDataReader reader, Folder fld)
		{
			WebMailMessage webMsg = new WebMailMessage(_account);
			DataTable schemaTable = reader.GetSchemaTable();

			foreach (DataRow row in schemaTable.Rows)
			{
				int index = (int)row[1];
				if (reader.IsDBNull(index)) continue;
				string fieldName = row[0] as string;
				if (fieldName != null)
				{
					switch (fieldName.ToLower(CultureInfo.InvariantCulture))
					{
						case "id":
							webMsg.ID = Convert.ToInt64(reader.GetValue(index));
							break;
						case "id_msg":
							webMsg.IDMsg = Convert.ToInt32(reader.GetValue(index));
							break;
						case "id_acct":
							webMsg.IDAcct = Convert.ToInt32(reader.GetValue(index));
							break;
						case "id_folder_srv":
							webMsg.IDFolderSrv = Convert.ToInt64(reader.GetValue(index));
							break;
						case "id_folder_db":
							webMsg.IDFolderDB = Convert.ToInt64(reader.GetValue(index));
							break;
						case "str_uid":
							webMsg.StrUid = Convert.ToString(reader.GetValue(index));
							break;
						case "int_uid":
							webMsg.IntUid = Convert.ToInt64(reader.GetValue(index));
							break;
						case "from_msg":
							webMsg.FromMsg = EmailAddress.Parse(Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index))));
							break;
						case "to_msg":
							webMsg.ToMsg = EmailAddressCollection.Parse(Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index))));
							break;
						case "cc_msg":
							webMsg.CcMsg = EmailAddressCollection.Parse(Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index))));
							break;
						case "bcc_msg":
							webMsg.BccMsg = EmailAddressCollection.Parse(Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index))));
							break;
						case "subject":
							webMsg.Subject = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "msg_date":
							webMsg.MsgDate = reader.GetDateTime(index);
							break;
						case "attachments":
							webMsg.Attachments = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "size":
							webMsg.Size = Convert.ToInt64(reader.GetValue(index));
							break;
						case "seen":
							webMsg.Seen = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "flagged":
							webMsg.Flagged = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "priority":
							webMsg.Priority = (MailPriority)Convert.ToInt16(reader.GetValue(index));
							break;
						case "downloaded":
							webMsg.Downloaded = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "x_spam":
							webMsg.XSpam = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "rtl":
							webMsg.Rtl = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "deleted":
							webMsg.Deleted = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "is_full":
							webMsg.IsFull = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "replied":
							webMsg.Replied = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "forwarded":
							webMsg.Forwarded = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "flags":
							webMsg.Flags = (SystemMessageFlags)Convert.ToInt16(reader.GetValue(index));
							break;
						case "body_text":
							webMsg.BodyText = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "grayed":
							webMsg.Grayed = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "folder_name":
							webMsg.FolderFullName = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "charset":
							webMsg.OverrideCharset = Convert.ToInt32(reader.GetValue(index));
							break;
					}
				}
			}
			if (fld != null) webMsg.FolderFullName = fld.FullPath;
			return webMsg;
		}

		public override AddressBookContact CreateAddressBookContact(AddressBookContact contactToCreate)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();
				int id_user = contactToCreate.IDUser;
				if (_account != null)
				{
					if (_account.UserOfAccount != null) id_user = _account.UserOfAccount.ID;
				}
				IDbCommand command = _commandCreator.InsertIntoAwmAddrBook(id_user,
					Utils.ConvertToDBString(_account, contactToCreate.HEmail.ToString()),
					Utils.ConvertToDBString(_account, contactToCreate.FullName),
					Utils.ConvertToDBString(_account, contactToCreate.Notes),
					contactToCreate.UseFriendlyName,
					Utils.ConvertToDBString(_account, contactToCreate.HStreet),
					Utils.ConvertToDBString(_account, contactToCreate.HCity),
					Utils.ConvertToDBString(_account, contactToCreate.HState),
					Utils.ConvertToDBString(_account, contactToCreate.HZip),
					Utils.ConvertToDBString(_account, contactToCreate.HCountry),
					Utils.ConvertToDBString(_account, contactToCreate.HPhone),
					Utils.ConvertToDBString(_account, contactToCreate.HFax),
					Utils.ConvertToDBString(_account, contactToCreate.HMobile),
					Utils.ConvertToDBString(_account, contactToCreate.HWeb),
					Utils.ConvertToDBString(_account, contactToCreate.BEmail.ToString()),
					Utils.ConvertToDBString(_account, contactToCreate.BCompany),
					Utils.ConvertToDBString(_account, contactToCreate.BStreet),
					Utils.ConvertToDBString(_account, contactToCreate.BCity),
					Utils.ConvertToDBString(_account, contactToCreate.BState),
					Utils.ConvertToDBString(_account, contactToCreate.BZip),
					Utils.ConvertToDBString(_account, contactToCreate.BCountry),
					Utils.ConvertToDBString(_account, contactToCreate.BJobTitle),
					Utils.ConvertToDBString(_account, contactToCreate.BDepartment),
					Utils.ConvertToDBString(_account, contactToCreate.BOffice),
					Utils.ConvertToDBString(_account, contactToCreate.BPhone),
					Utils.ConvertToDBString(_account, contactToCreate.BFax),
					Utils.ConvertToDBString(_account, contactToCreate.BWeb),
					contactToCreate.BirthdayDay, contactToCreate.BirthdayMonth, contactToCreate.BirthdayYear,
					Utils.ConvertToDBString(_account, contactToCreate.OtherEmail.ToString()),
					(short)contactToCreate.PrimaryEmail, contactToCreate.IDAddrPrev, contactToCreate.Tmp,
					contactToCreate.UseFrequency, contactToCreate.AutoCreate);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command.CommandText = _commandCreator.SelectIdentity();
				object obj = ExecuteScalarCommand(command);
				if (obj != null)
				{
					contactToCreate.IDAddr = Convert.ToInt64(obj);
				}
				else
				{
					throw new WebMailDatabaseException("Can't create contact");
				}
				if ((contactToCreate.Groups != null) && (contactToCreate.Groups.Length > 0))
				{
					for (int i = 0; i < contactToCreate.Groups.Length; i++)
					{
						command = _commandCreator.InsertIntoAwmAddrGroupsContacts(contactToCreate.IDAddr, contactToCreate.Groups[i].IDGroup);
						command.Transaction = myTrans;
						command.ExecuteNonQuery();
					}
				}
				myTrans.Commit();
				return contactToCreate;
			}
			catch(Exception ex)
			{
				if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public override AddressBookGroup CreateAddressBookGroup(AddressBookGroup groupToCreate)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.InsertIntoAwmAddrGroups(groupToCreate.IDUser,
					Utils.ConvertToDBString(_account, groupToCreate.GroupName),
					Utils.ConvertToDBString(_account, groupToCreate.Phone),
					Utils.ConvertToDBString(_account, groupToCreate.Fax),
					Utils.ConvertToDBString(_account, groupToCreate.Web),
					groupToCreate.Organization, groupToCreate.UseFrequency,
					Utils.ConvertToDBString(_account, groupToCreate.Email),
					Utils.ConvertToDBString(_account, groupToCreate.Company),
					Utils.ConvertToDBString(_account, groupToCreate.Street),
					Utils.ConvertToDBString(_account, groupToCreate.City),
					Utils.ConvertToDBString(_account, groupToCreate.State),
					Utils.ConvertToDBString(_account, groupToCreate.Zip),
					Utils.ConvertToDBString(_account, groupToCreate.Country));
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command.CommandText = _commandCreator.SelectIdentity();
				object obj = ExecuteScalarCommand(command);
				if (obj != null)
				{
					groupToCreate.IDGroup = Convert.ToInt32(obj);
				}
				else
				{
					throw new WebMailDatabaseException("Can't create group");
				}

				command = _commandCreator.DeleteFromAwmAddrGroupContacts(groupToCreate.IDGroup);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				foreach (AddressBookContact contact in groupToCreate.Contacts)
				{
					command = _commandCreator.InsertIntoAwmAddrGroupsContacts(contact.IDAddr, groupToCreate.IDGroup);
					command.Transaction = myTrans;
					command.ExecuteNonQuery();
				}

				foreach (AddressBookContact contactToCreate in groupToCreate.NewContacts)
				{
					command = _commandCreator.InsertIntoAwmAddrBook(groupToCreate.IDUser,
						Utils.ConvertToDBString(_account, contactToCreate.HEmail.ToString()),
						Utils.ConvertToDBString(_account, contactToCreate.FullName),
						Utils.ConvertToDBString(_account, contactToCreate.Notes),
						contactToCreate.UseFriendlyName,
						Utils.ConvertToDBString(_account, contactToCreate.HStreet),
						Utils.ConvertToDBString(_account, contactToCreate.HCity),
						Utils.ConvertToDBString(_account, contactToCreate.HState),
						Utils.ConvertToDBString(_account, contactToCreate.HZip),
						Utils.ConvertToDBString(_account, contactToCreate.HCountry),
						Utils.ConvertToDBString(_account, contactToCreate.HPhone),
						Utils.ConvertToDBString(_account, contactToCreate.HFax),
						Utils.ConvertToDBString(_account, contactToCreate.HMobile),
						Utils.ConvertToDBString(_account, contactToCreate.HWeb),
						Utils.ConvertToDBString(_account, contactToCreate.BEmail.ToString()),
						Utils.ConvertToDBString(_account, contactToCreate.BCompany),
						Utils.ConvertToDBString(_account, contactToCreate.BStreet),
						Utils.ConvertToDBString(_account, contactToCreate.BCity),
						Utils.ConvertToDBString(_account, contactToCreate.BState),
						Utils.ConvertToDBString(_account, contactToCreate.BZip),
						Utils.ConvertToDBString(_account, contactToCreate.BCountry),
						Utils.ConvertToDBString(_account, contactToCreate.BJobTitle),
						Utils.ConvertToDBString(_account, contactToCreate.BDepartment),
						Utils.ConvertToDBString(_account, contactToCreate.BOffice),
						Utils.ConvertToDBString(_account, contactToCreate.BPhone),
						Utils.ConvertToDBString(_account, contactToCreate.BFax),
						Utils.ConvertToDBString(_account, contactToCreate.BWeb),
						contactToCreate.BirthdayDay, contactToCreate.BirthdayMonth, contactToCreate.BirthdayYear,
						Utils.ConvertToDBString(_account, contactToCreate.OtherEmail.ToString()),
						(short)contactToCreate.PrimaryEmail, contactToCreate.IDAddrPrev, contactToCreate.Tmp,
						contactToCreate.UseFrequency, contactToCreate.AutoCreate);
					command.Transaction = myTrans;
					command.ExecuteNonQuery();

					command.CommandText = _commandCreator.SelectIdentity();
					obj = ExecuteScalarCommand(command);
					if (obj != null)
					{
						contactToCreate.IDAddr = Convert.ToInt64(obj);
						command = _commandCreator.InsertIntoAwmAddrGroupsContacts(contactToCreate.IDAddr, groupToCreate.IDGroup);
						command.Transaction = myTrans;
						command.ExecuteNonQuery();
					}
				}

				myTrans.Commit();
				return groupToCreate;
			}
			catch(Exception ex)
			{
				if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public override AddressBookGroupContact[] SelectAddressBookContactsGroups(int page, int sort_field, int sort_order, int id_group, string look_for, int look_for_type)
		{
			ArrayList contacts = new ArrayList();
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				string filter = "email";
				switch (sort_field)
				{
					case 0:
						filter = "is_group";
						break;
					case 1:
						filter = "name";
						break;
					case 2:
						filter = "email";
						break;
					case 3:
						filter = "frequency";
						break;
				}

				IDbCommand command = null;
				if ((look_for != null) && (look_for.Length > 0))
				{
					command = _commandCreator.SearchAwmAddrBookGroups(_account.UserOfAccount.ID, _account.UserOfAccount.Settings.ContactsPerPage, page, filter, sort_order, id_group, Utils.ConvertToDBString(_account, look_for), look_for_type);
				}
				else
				{
					command = _commandCreator.SelectAwmAddrBookGroups(_account.UserOfAccount.ID, _account.UserOfAccount.Settings.ContactsPerPage, page, sort_field, sort_order);
				}
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						AddressBookGroupContact group_contact = new AddressBookGroupContact();
						DataTable schemaTable = reader.GetSchemaTable();
						foreach (DataRow row in schemaTable.Rows)
						{
							int index = (int)row[1];
							if (reader.IsDBNull(index)) continue;
							string fieldName = row[0] as string;
							if (fieldName != null)
							{
								switch (fieldName.ToLower(CultureInfo.InvariantCulture))
								{
									case "id":
										group_contact.id = Convert.ToInt64(reader.GetValue(index));
										break;
									case "name":
										group_contact.fullname = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
										break;
									case "email":
										group_contact.email = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
										break;
									case "is_group":
										group_contact.isGroup = Convert.ToBoolean(reader.GetValue(index));
										break;
								}
							}
						}
						contacts.Add(group_contact);
					}
				}
				myTrans.Commit();
				return (AddressBookGroupContact[])contacts.ToArray(typeof(AddressBookGroupContact));
			}
			catch(Exception ex)
			{
				if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public override void UpdateAddressBookGroup(AddressBookGroup groupToUpdate)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.UpdateAwmAddrGroups(groupToUpdate.IDGroup, groupToUpdate.IDUser,
					Utils.ConvertToDBString(_account, groupToUpdate.GroupName),
					Utils.ConvertToDBString(_account, groupToUpdate.Phone),
					Utils.ConvertToDBString(_account, groupToUpdate.Fax),
					Utils.ConvertToDBString(_account, groupToUpdate.Web),
					groupToUpdate.Organization, groupToUpdate.UseFrequency,
					Utils.ConvertToDBString(_account, groupToUpdate.Email),
					Utils.ConvertToDBString(_account, groupToUpdate.Company),
					Utils.ConvertToDBString(_account, groupToUpdate.Street),
					Utils.ConvertToDBString(_account, groupToUpdate.City),
					Utils.ConvertToDBString(_account, groupToUpdate.State),
					Utils.ConvertToDBString(_account, groupToUpdate.Zip),
					Utils.ConvertToDBString(_account, groupToUpdate.Country));
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmAddrGroupContacts(groupToUpdate.IDGroup);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				foreach (AddressBookContact contact in groupToUpdate.Contacts)
				{
					command = _commandCreator.InsertIntoAwmAddrGroupsContacts(contact.IDAddr, groupToUpdate.IDGroup);
					command.Transaction = myTrans;
					command.ExecuteNonQuery();
				}

				foreach (AddressBookContact contactToCreate in groupToUpdate.NewContacts)
				{
					command = _commandCreator.InsertIntoAwmAddrBook(groupToUpdate.IDUser,
						Utils.ConvertToDBString(_account, contactToCreate.HEmail.ToString()),
						Utils.ConvertToDBString(_account, contactToCreate.FullName),
						Utils.ConvertToDBString(_account, contactToCreate.Notes),
						contactToCreate.UseFriendlyName,
						Utils.ConvertToDBString(_account, contactToCreate.HStreet),
						Utils.ConvertToDBString(_account, contactToCreate.HCity),
						Utils.ConvertToDBString(_account, contactToCreate.HState),
						Utils.ConvertToDBString(_account, contactToCreate.HZip),
						Utils.ConvertToDBString(_account, contactToCreate.HCountry),
						Utils.ConvertToDBString(_account, contactToCreate.HPhone),
						Utils.ConvertToDBString(_account, contactToCreate.HFax),
						Utils.ConvertToDBString(_account, contactToCreate.HMobile),
						Utils.ConvertToDBString(_account, contactToCreate.HWeb),
						Utils.ConvertToDBString(_account, contactToCreate.BEmail.ToString()),
						Utils.ConvertToDBString(_account, contactToCreate.BCompany),
						Utils.ConvertToDBString(_account, contactToCreate.BStreet),
						Utils.ConvertToDBString(_account, contactToCreate.BCity),
						Utils.ConvertToDBString(_account, contactToCreate.BState),
						Utils.ConvertToDBString(_account, contactToCreate.BZip),
						Utils.ConvertToDBString(_account, contactToCreate.BCountry),
						Utils.ConvertToDBString(_account, contactToCreate.BJobTitle),
						Utils.ConvertToDBString(_account, contactToCreate.BDepartment),
						Utils.ConvertToDBString(_account, contactToCreate.BOffice),
						Utils.ConvertToDBString(_account, contactToCreate.BPhone),
						Utils.ConvertToDBString(_account, contactToCreate.BFax),
						Utils.ConvertToDBString(_account, contactToCreate.BWeb),
						contactToCreate.BirthdayDay, contactToCreate.BirthdayMonth, contactToCreate.BirthdayYear,
						Utils.ConvertToDBString(_account, contactToCreate.OtherEmail.ToString()),
						(short)contactToCreate.PrimaryEmail, contactToCreate.IDAddrPrev, contactToCreate.Tmp,
						contactToCreate.UseFrequency, contactToCreate.AutoCreate);
					command.Transaction = myTrans;
					object obj = ExecuteScalarCommand(command);
					if (obj != null)
					{
						contactToCreate.IDAddr = Convert.ToInt64(obj);
						command = _commandCreator.InsertIntoAwmAddrGroupsContacts(contactToCreate.IDAddr, groupToUpdate.IDGroup);
						command.Transaction = myTrans;
						command.ExecuteNonQuery();
					}
				}

				myTrans.Commit();
			}
			catch(Exception ex)
			{
				if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		protected override AddressBookContact ReadAddressBookContact(IDataReader reader)
		{
			AddressBookContact result = new AddressBookContact();
			DataTable schemaTable = reader.GetSchemaTable();

			foreach (DataRow row in schemaTable.Rows)
			{
				int index = (int)row[1];
				if (reader.IsDBNull(index)) continue;
				string fieldName = row[0] as string;
				if (fieldName != null)
				{
					switch (fieldName.ToLower(CultureInfo.InvariantCulture))
					{
						case "id_addr":
							result.IDAddr = Convert.ToInt64(reader.GetValue(index));
							break;
						case "id_user":
							result.IDUser = Convert.ToInt32(reader.GetValue(index));
							break;
						case "h_email":
							result.HEmail = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "fullname":
							result.FullName = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "notes":
							result.Notes = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "use_friendly_nm":
							result.UseFriendlyName = (Convert.ToByte(reader.GetValue(index)) == 1) ? true : false;;
							break;
						case "h_street":
							result.HStreet = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "h_city":
							result.HCity = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "h_state":
							result.HState = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "h_zip":
							result.HZip = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "h_country":
							result.HCountry = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "h_phone":
							result.HPhone = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "h_fax":
							result.HFax = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "h_mobile":
							result.HMobile = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "h_web":
							result.HWeb = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_email":
							result.BEmail = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_company":
							result.BCompany = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_street":
							result.BStreet = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_city":
							result.BCity = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_state":
							result.BState = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_zip":
							result.BZip = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_country":
							result.BCountry = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_job_title":
							result.BJobTitle = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_department":
							result.BDepartment = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_office":
							result.BOffice = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_phone":
							result.BPhone = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_fax":
							result.BFax = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_web":
							result.BWeb = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "birthday_day":
							result.BirthdayDay = Convert.ToByte(reader.GetValue(index));
							break;
						case "birthday_month":
							result.BirthdayMonth = Convert.ToByte(reader.GetValue(index));
							break;
						case "birthday_year":
							result.BirthdayYear = Convert.ToInt16(reader.GetValue(index));
							break;
						case "other_email":
							result.OtherEmail = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "primary_email":
							result.PrimaryEmail = (ContactPrimaryEmail)Convert.ToInt16(reader.GetValue(index));
							break;
						case "id_addr_prev":
							result.IDAddrPrev = Convert.ToInt64(reader.GetValue(index));
							break;
						case "tmp":
							result.Tmp = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "use_frequency":
							result.UseFrequency = Convert.ToInt32(reader.GetValue(index));
							break;
						case "auto_create":
							result.AutoCreate = (Convert.ToByte(reader.GetValue(index)) == 1) ? true : false;
							break;
					}
				}
			}
			return result;
		}
	}
}