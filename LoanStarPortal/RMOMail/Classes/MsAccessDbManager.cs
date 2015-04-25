using System;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using MailBee.ImapMail;

namespace WebMailPro
{
	public class MsAccessDbManager : DbManager
	{
		public MsAccessDbManager() : this(null) {}

		public MsAccessDbManager(Account acct) : base(acct)
		{
			_connection = new OleDbConnection();
			_commandCreator = new MsAccessCommandCreator(_connection as OleDbConnection, new OleDbCommand());
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
		public override UserColumn CreateUserColumn(int id_column, int id_user, int value)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.InsertIntoAwmColumns(id_column, id_user, value);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command.CommandText = _commandCreator.SelectIdentity();
				object obj = ExecuteScalarCommand(command);
				myTrans.Commit();

				int id = (obj != null) ? Convert.ToInt32(obj) : -1;

				return new UserColumn(id, id_column, id_user, value);
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
					command.ExecuteNonQuery();

					command.CommandText = _commandCreator.SelectIdentity();
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
							acct.ID = reader.GetInt32(index);
							break;
						case "id_user":
							acct.IDUser = reader.GetInt32(index);
							break;
						case "def_acct":
							acct.DefaultAccount = reader.GetBoolean(index);
							break;
						case "deleted":
							acct.Deleted = reader.GetBoolean(index);
							break;
						case "email":
							acct.Email = reader.GetString(index);
							break;
						case "mail_protocol":
							acct.MailIncomingProtocol = (IncomingMailProtocol) reader.GetInt16(index);
							break;
						case "mail_inc_host":
							acct.MailIncomingHost = reader.GetString(index);
							break;
						case "mail_inc_login":
							acct.MailIncomingLogin = reader.GetString(index);
							break;
						case "mail_inc_pass":
							acct.MailIncomingPassword = Utils.DecodePassword(reader.GetString(index));
							break;
						case "mail_inc_port":
							acct.MailIncomingPort = reader.GetInt32(index);
							break;
						case "mail_out_host":
							acct.MailOutgoingHost = reader.GetString(index);
							break;
						case "mail_out_login":
							acct.MailOutgoingLogin = reader.GetString(index);
							break;
						case "mail_out_pass":
							string out_pass = reader.GetString(index);
							acct.MailOutgoingPassword = Utils.DecodePassword(out_pass);
							break;
						case "mail_out_port":
							acct.MailOutgoingPort = reader.GetInt32(index);
							break;
						case "mail_out_auth":
							acct.MailOutgoingAuthentication = reader.GetBoolean(index);
							break;
						case "friendly_nm":
							acct.FriendlyName = reader.GetString(index);
							break;
						case "use_friendly_nm":
							acct.UseFriendlyName = reader.GetBoolean(index);
							break;
						case "def_order":
							acct.DefaultOrder = (DefaultOrder)reader.GetByte(index);
							break;
						case "getmail_at_login":
							acct.GetMailAtLogin = reader.GetBoolean(index);
							break;
						case "mail_mode":
							acct.MailMode = (MailMode)reader.GetByte(index);
							break;
						case "mails_on_server_days":
							acct.MailsOnServerDays = reader.GetInt16(index);
							break;
						case "signature":
							acct.Signature = reader.GetString(index);
							break;
						case "signature_type":
							acct.SignatureType = (SignatureType)reader.GetByte(index);
							break;
						case "signature_opt":
							acct.SignatureOptions = (SignatureOptions)reader.GetByte(index);
							break;
						case "delimiter":
							acct.Delimiter = reader.GetString(index);
							break;
						case "mailbox_size":
							acct.MailboxSize = Convert.ToInt64(reader.GetDecimal(index));
							break;
					}
				}
			}
			return acct;
		}

	}

}