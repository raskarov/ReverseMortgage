using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for import.
	/// </summary>
	public partial class import : Page
	{
		protected int _jsErrorCode = 0;
		protected int _jsContactsImported = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			Account acct = this.Session[Constants.sessionAccount] as Account;
			if (acct != null)
			{
				int fileType = (string.Compare(Request.Form["file_type"], "0", true, CultureInfo.InvariantCulture) == 0) ? 0 : 1;
				HttpFileCollection files = Request.Files;
				if ((files != null) && (files.Count > 0))
				{
					HttpPostedFile file = files[0];
					if (file != null)
					{
						byte[] buffer = null;
						using (Stream uploadStream = file.InputStream)
						{
							buffer = new byte[uploadStream.Length];
							long numBytesToRead = uploadStream.Length;
							long numBytesRead = 0;
							while (numBytesToRead > 0)
							{
								int n = uploadStream.Read(buffer, (int)numBytesRead, (int)numBytesToRead);
								if (n==0)
									break;
								numBytesRead += n;
								numBytesToRead -= n;
							}
						}
						if (buffer != null)
						{
							try
							{
								string csvText = Encoding.Default.GetString(buffer);
								CsvParser parser = new CsvParser(csvText, true);
								DataTable dt = parser.Parse();
								if (dt.Rows.Count == 0)
								{
									WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
									Log.WriteLine("import Page_Load", "Error: No contacts for import");
									_jsErrorCode = 2;
									_jsContactsImported = 0;
									Session[Constants.sessionReportText] = string.Format(@"{0}", resMan.GetString("ErrorNoContacts"));
									return;
								}

								//Outlook 2000/XP/2003
								//outlookCsvFields = new string[] {"First Name","Last Name","Notes","Home Street","Home City","Home Postal Code", "Home State","Home Country","Home Phone", "Home Fax","Mobile Phone","Web page","Company", "Business Street","Business City","Business State","Business Postal Code","Business Country","Job Title","Department","Office Location","Business Phone","Business Fax","Pager","E-mail Address","E-Mail Display Name"};
								//OutlookExpress 6
								//outlookCsvFields = new string[] {"First Name","Last Name","Birthday","Notes", "Home Street", "Home City", "Home Postal Code", "Home State", "Home Country/Region", "Home Phone", "Home Fax", "Mobile Phone", "Personal Web Page", "Company","Business Street", "Business City", "Business State","Business Postal Code","Business Country/Region","Job Title","Department","Office Location","Business Phone","Business Fax","Pager","Business Web Page","E-mail Address", "Name", "Nickname"};
								ArrayList contacts = new ArrayList();
								for (int rowsIndex = 0; rowsIndex < dt.Rows.Count; rowsIndex++)
								{
									bool contactInitialized = false;
									AddressBookContact contact = new AddressBookContact();
									contact.IDUser = acct.UserOfAccount.ID;
									string firstName = string.Empty;
									string lastName = string.Empty;
									string nickname = string.Empty;
									string displayName = string.Empty;
									string birthday = string.Empty;
									DataRow dr = dt.Rows[rowsIndex];
									for (int columnsIndex = 0; columnsIndex < dt.Columns.Count; columnsIndex++)
									{
										if (dr[columnsIndex] != DBNull.Value)
										{
											if (dr[columnsIndex] as string != null)
											{
												switch (dt.Columns[columnsIndex].ColumnName)
												{
													case "First Name":
														firstName = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Last Name":
														lastName = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Notes":
														contact.Notes = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Home Street":
														contact.HStreet = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Home City":
														contact.HCity = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Home Postal Code":
														contact.HZip = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Home State":
														contact.HState = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Home Country":
													case "Home Country/Region":
														contact.HCountry = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Home Phone":
														contact.HPhone = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Home Fax":
														contact.HFax = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Mobile Phone":
														contact.HMobile = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Web page":
													case "Personal Web Page":
														contact.HWeb = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Company":
														contact.BCompany = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Business Street":
														contact.BStreet = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Business City":
														contact.BCity = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Business State":
														contact.BState = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Business Postal Code":
														contact.BZip = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Business Country":
													case "Business Country/Region":
														contact.BCountry = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Job Title":
														contact.BJobTitle = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Department":
														contact.BDepartment = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Office Location":
														contact.BOffice = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Business Phone":
														contact.BOffice = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Business Fax":
														contact.BFax = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Business Web Page":
														contact.BWeb = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "E-mail Address":
														contact.HEmail = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "E-Mail Display Name":
													case "Name":
														displayName = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Birthday":
														birthday = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
													case "Nickname":
														nickname = dr[columnsIndex] as string;
														contactInitialized = true;
														break;
												}
											}
										}
									}
									if (!contactInitialized)
									{
										WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
										Log.WriteLine("import Page_Load", "Error: CSV file has invalid format");
										_jsErrorCode = 3;
										_jsContactsImported = 0;
										Session[Constants.sessionReportText] = string.Format(@"{0}", resMan.GetString("ErrorInvalidCSV"));
										break;
									}
									string fullName = string.Empty;
									if (nickname != string.Empty)
									{
										fullName = string.Format("{0} \"{1}\" {2}", firstName, nickname, lastName);
									}
									else
									{
										fullName = string.Format("{0} {1}", firstName, lastName);
									}
									contact.FullName = fullName;
									try
									{
										DateTime birthdayDate = DateTime.Parse(birthday);
										contact.BirthdayDay = (byte)birthdayDate.Day;
										contact.BirthdayMonth = (byte)birthdayDate.Month;
										contact.BirthdayYear = (short)birthdayDate.Year;
									}
									catch {}
									contacts.Add(contact);
								}
								DbStorage storage = DbStorageCreator.CreateDatabaseStorage(acct);
								try
								{
									storage.Connect();
									foreach (AddressBookContact contact in contacts)
									{
										contact.AutoCreate = true;
										storage.CreateAddressBookContact(contact);
									}
									_jsErrorCode = 1;
									_jsContactsImported = contacts.Count;
									WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
									Session[Constants.sessionReportText] = string.Format(@"{0} {1} {2}", resMan.GetString("InfoHaveImported"), contacts.Count, resMan.GetString("InfoNewContacts"));
								}
								finally
								{
									storage.Disconnect();
								}
							}
							catch
							{
								Log.WriteLine("import Page_Load", "Error while importing contacts");
								_jsErrorCode = 0;
								_jsContactsImported = 0;
							}
						}
					}
				}
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new EventHandler(this.Page_Load);
		}
		#endregion

	}
}
