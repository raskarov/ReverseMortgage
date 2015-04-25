using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Xml.XPath;
using MailBee.Html;
using MailBee.ImapMail;
using MailBee.Mime;
using Microsoft.Win32;

namespace WebMailPro
{
	public class Validation
	{
		private static string _errorMessage = null;
		private static string _corrected = null;
		private static string _testData = null;
		private static object _additionalInfo = null;
			
		public static string ErrorMessage
		{
			get
			{
				return _errorMessage;
			}
		}
		public static string Corrected
		{
			get
			{
				return _corrected;
			}
		}
		//Enum			
		public enum ValidationTask
		{
			/// <summary>
			/// Input: Not empty email string, like account@domen. Output: Trimmed string.
			/// </summary>
			Email,
			/// <summary>
			/// Input: Not empty login string, without special chars. Output: Trimmed string. Additional info: advanced login flag as string: "0" || "1"
			/// </summary>
			Login,
			/// <summary>
			/// Input: Not empty string. Output: Input.
			/// </summary>
			Password,
			/// <summary>
			/// Input: Incoming email server name, not empty. Output: Trimmed string.
			/// </summary>
			INServer,
			/// <summary>
			/// Input: Positive integer number for incoming port, not empty. Output: Trimmed string.
			/// </summary>
			INPort,
			/// <summary>
			/// Input: Outcoming email server name, not empty. Output: Trimmed string.
			/// </summary>
			OUTServer,
			/// <summary>
			/// Input: Positive integer number for outcoming port, not empty. Output: Trimmed string.
			/// </summary>
			OUTPort,
			/// <summary>
			/// Input: Messages per page value as positive number. Output: Trimmed string.
			/// </summary>
			MPP,
			/// <summary>
			/// Input: (???) Advanced, not empty. Output: Trimmed string.
			/// </summary>
			Advanced,
			/// <summary>
			/// Input: SMTP Server login. Output: Trimmed string.
			/// </summary>
			SMTPLogin,
			/// <summary>
			/// Input: Keep messages on server for days, positive number, not empty. Output: Trimmed string.
			/// </summary>
			KeepMessages,
			/// <summary>
			/// Input: Contacts per page, positive number, not empty. Output: Trimmed string.
			/// </summary>
			CPP,
			/// <summary>
			/// Input: Filter substring, not empty. Output: Trimmed string.
			/// </summary>
			Substring,
			/// <summary>
			/// Input: Valid folder name, not empty, excluding CON, AUX, COM1, COM2, COM3, COM4, LPT1, LPT2, LPT3, PRN, NUL. Output: Trimmed string.
			/// </summary>
			FolderName,
			/// <summary>
			/// Input: Email address for contact. Output: Trimmed string.
			/// </summary>
			ContactsEMail,
			/// <summary>
			/// Input: Name for contact. Output: Trimmed string.
			/// </summary>
			ContactsName,
			/// <summary>
			/// Input: Street for contact. Output: Trimmed string.
			/// </summary>
			ContactsStreet,
			/// <summary>
			/// Input: City for contact. Output: Trimmed string.
			/// </summary>
			ContactsCity,
			/// <summary>
			/// Input: State/province for contact. Output: Trimmed string.
			/// </summary>
			ContactsState,
			/// <summary>
			/// Input: Zip code for contact. Output: Trimmed string.
			/// </summary>
			ContactsZipCode,
			/// <summary>
			/// Input: Country/region for contact. Output: Trimmed string.
			/// </summary>
			ContactsCountry,
			/// <summary>
			/// Input: Fax number for contact. Output: Trimmed string.
			/// </summary>
			ContactsFax,
			/// <summary>
			/// Input: Phone number for contact. Output: Trimmed string.
			/// </summary>
			ContactsPhone,
			/// <summary>
			/// Input: Web page for contact. Output: Trimmed string.
			/// </summary>
			ContactsWebPage,
			/// <summary>
			/// Input: Cell phone number for contact. Output: Trimmed string.
			/// </summary>
			ContactsMobile,
			/// <summary>
			/// Input: Company name for contact. Output: Trimmed string.
			/// </summary>
			ContactsCompany,
			/// <summary>
			/// Input: Company department for contact. Output: Trimmed string.
			/// </summary>
			ContactsDepartment,
			/// <summary>
			/// Input: Job title for contact. Output: Trimmed string.
			/// </summary>
			ContactsJobTitle,
			/// <summary>
			/// Input: Office (???) for contact. Output: Trimmed string.
			/// </summary>
			ContactsOffice,
			/// <summary>
			/// Input: Notes for contact. Output: Trimmed string.
			/// </summary>
			ContactsNotes,
			/// <summary>
			/// Input: Group name, not empty, not existing. Output: Trimmed string.
			/// </summary>
			GroupName,
			/// <summary>
			/// Input: AddContact (???), not empty, not existing. Output: Trimmed string.
			/// </summary>
			AddContacts
		}
	    //Public methods
		public static bool CheckIt(ValidationTask checkFor, string testData)
		{
			return CheckIt(checkFor, testData, null);
		}
		public static bool CheckIt(ValidationTask checkFor, string testData, object additionalInfo)
		{
			if (testData==null)
			{
				_errorMessage = "Null!";
				return false;
			}
			_testData = testData;
			if (additionalInfo!=null)
				{
					_additionalInfo = additionalInfo;
				}
			switch(checkFor)
			{
				case ValidationTask.Email:
					return emailCheck();
				case ValidationTask.Login:
					return loginCheck();
				case ValidationTask.Password:
					return passwordCheck();
				case ValidationTask.INServer:
					return inServerCheck();
				case ValidationTask.INPort:
					return inPortCheck();
				case ValidationTask.OUTServer:
					return outServerCheck();
				case ValidationTask.OUTPort:
					return outPortCheck();
				case ValidationTask.MPP:
					return mppCheck();
				case ValidationTask.Advanced:
					return advancedCheck();
				case ValidationTask.SMTPLogin:
					return smtpLogin();
				case ValidationTask.KeepMessages:
					return keepMessagesCheck();
				case ValidationTask.CPP:
					return cppCheck();
				case ValidationTask.Substring:
					return substringCheck();
				case ValidationTask.FolderName:
					return folderNameCheck();
				case ValidationTask.ContactsEMail:
					return contactsEmailCheck();
				case ValidationTask.ContactsName:
					return contactsNameCheck();
				case ValidationTask.ContactsStreet:
					return contactsStreetCheck();
				case ValidationTask.ContactsCity:
					return contactsCityCheck();
				case ValidationTask.ContactsState:
					return contactsStateCheck();
				case ValidationTask.ContactsZipCode:
					return contactsZipCodeCheck();
				case ValidationTask.ContactsCountry:
					return contactsCountryCheck();
				case ValidationTask.ContactsFax:
					return contactsFaxCheck();
				case ValidationTask.ContactsPhone:
					return contactsPhoneCheck();
				case ValidationTask.ContactsWebPage:
					return contactsWebPageCheck();
				case ValidationTask.ContactsMobile:
					return contactsMobileCheck();
				case ValidationTask.ContactsCompany:
					return contactsCompanyCheck();
				case ValidationTask.ContactsDepartment:
					return contactsDepartmentCheck();
				case ValidationTask.ContactsJobTitle:
					return contactsJobTitleCheck();
				case ValidationTask.ContactsOffice:
					return contactsOfficeCheck();
				case ValidationTask.ContactsNotes:
					return contactsNotesCheck();
				case ValidationTask.GroupName:
					return groupNameCheck();
				case ValidationTask.AddContacts:
					return addContactsCheck();
				default:
					return true;
			}
		}
		//Checks
		/// <summary>
		/// Check for valid email. Additional info required:
		/// </summary>
		/// <returns>True, if valid. False, if invalid.</returns>
		private static bool emailCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length==0)
			{
				_errorMessage = "WarningEmailFieldBlank";
				return false;
			}
			if (!Regex.IsMatch(_corrected,@"^([A-Za-z_0-9!#\$%\^\{\}`~&'\+\-=_.])+@([A-Za-z_0-9-.])+$"))
			{
				_errorMessage = "WarningCorrectEmail";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid login. Additional info required: advanced login flag as string: "0" || "1"
		/// </summary>
		/// <returns>True, if valid. False, if invalid.</returns>
		private static bool loginCheck()
		{
			_corrected = _testData.Trim();
			if ((_corrected.Length==0)&&((_additionalInfo as string)=="1"))
			{
				_errorMessage = "WarningLoginFieldBlank";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid password. Additional info required:
		/// </summary>
		/// <returns></returns>
		private static bool passwordCheck()
		{
			if (_testData.Length==0)
			{
				_errorMessage = "WarningPassBlank";
				return false;
			}
			_corrected = _testData;
			return true;
		}
		/// <summary>
		/// Check for vadid incoming mail server. Additional info required:
		/// </summary>
		/// <returns></returns>
		private static bool inServerCheck()
		{
			_corrected = _testData.Trim();
			if ((_corrected.Length == 0)&&(_additionalInfo as string)=="1")
			{
				_errorMessage = "WarningIncServerBlank";
				return false;
			}
			if (!Regex.IsMatch(_corrected,@"^([A-Za-z0-9-.])+$"))
			{
				_errorMessage = "WarningCorrectIncServer";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid port number. Additional info required:
		/// </summary>
		/// <returns></returns>
		private static bool inPortCheck()
		{
			_corrected = _testData.Trim();
			if ((_corrected.Length == 0)&&((_additionalInfo as string)=="1"))
			{
				_errorMessage = "WarningIncPortBlank";
				return false;
			}
			int iPort;
			try
			{
				iPort = int.Parse(_corrected);
			}
			catch
			{
				_errorMessage = "WarningIncPortNumber";
				return false;
			}
			if ((iPort<0)||(iPort>65535))
			{
				_errorMessage = "WarningIncPortNumber";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid SMTP Server string. Additional info required:
		/// </summary>
		/// <returns></returns>
		private static bool outServerCheck()
		{
			_corrected = _testData.Trim();
			if ((_corrected.Length == 0)&&((_additionalInfo as string)=="1"))
			{
				_errorMessage = "WarningOutPortBlank";
				return false;
			}
			if (!Regex.IsMatch(_corrected,@"^([A-Za-z0-9-.])+$"))
			{
				_errorMessage = "WarningCorrectSMTPServer";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid out port. Additional info required:
		/// </summary>
		/// <returns></returns>
		private static bool outPortCheck()
		{
			_corrected = _testData.Trim();
			if ((_corrected.Length == 0)&&((_additionalInfo as string)=="1"))
			{
				_errorMessage = "WarningOutPortBlank";
				return false;
			}
			int iPort;
			try
			{
				iPort = int.Parse(_corrected);
			}
			catch
			{
				_errorMessage = "WarningOutPortNumber";
				return false;
			}
			if ((iPort<0)||(iPort>65535))
			{
				_errorMessage = "WarningOutPortNumber";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid message per page string. Additional info required:
		/// </summary>
		/// <returns></returns>
		private static bool mppCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			short mPerPage;
			try
			{
				mPerPage = short.Parse(_corrected);
			}
			catch
			{
				_errorMessage = "<undefined>";
				return false;
			}
			if (mPerPage<0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid advanced ???.
		/// </summary>
		/// <returns></returns>
		private static bool advancedCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid smtpLogin. Additional info not required.
		/// </summary>
		/// <returns></returns>
		private static bool smtpLogin()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid "keep messages on server for days" value. Additional info not required.
		/// </summary>
		/// <returns></returns>
		private static bool keepMessagesCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			if (!Regex.IsMatch(_corrected,@"^([\d])+$"))
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid "Contacts per page" value. Additional info not required.
		/// </summary>
		/// <returns></returns>
		private static bool cppCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			if (!Regex.IsMatch(_corrected,@"^([\d])+$"))
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid "Substring" value in filters. Additional info not required.
		/// </summary>
		/// <returns></returns>
		private static bool substringCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "WarningEmptyFilter";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid folder name. Additional info not required.
		/// </summary>
		/// <returns></returns>
		private static bool folderNameCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "WarningEmptyFolderName";
				return false;
			}
			if (Regex.IsMatch(_corrected,@"[""<>/\\\*\?\|:]+"))
			{
				_errorMessage = "WarningCorrectFolderName";
				return false;
			}
			if (Regex.IsMatch(_corrected,@"^(|CON|AUX|COM1|COM2|COM3|COM4|LPT1|LPT2|LPT3|PRN|NUL)+$",RegexOptions.IgnoreCase))
			{
				_errorMessage = "WarningCorrectFolderName";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid email in contacts. Additional info not required.
		/// </summary>
		/// <returns></returns>
		private static bool contactsEmailCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid name in contacts. Additional info not required.
		/// </summary>
		/// <returns></returns>
		private static bool contactsNameCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				return false;
			}
			return true;
		}
		/// <summary>
		/// Checks for valid contacts values. Additional info not required.
		/// </summary>
		/// <returns></returns>
		private static bool contactsStreetCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsCityCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsStateCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsZipCodeCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsCountryCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsFaxCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsPhoneCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsWebPageCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			Regex re = new Regex(@"^[/\;\<\=\>\[\\#\?\]]+");
			_corrected = re.Replace(_corrected, "");
			return true;
		}
		private static bool contactsMobileCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsCompanyCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsDepartmentCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsJobTitleCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsOfficeCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsNotesCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid group name. Additional info required:
		/// </summary>
		/// <returns></returns>
		private static bool groupNameCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "WarningGroupNotComplete";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid add contacts. Additional info required:
		/// </summary>
		/// <returns></returns>
		private static bool addContactsCheck()
		{
			return true;
		}
	}
	//*****************************************************************

	/// <summary>
	/// Summary description for Utils.
	/// </summary>
	public class Utils
	{
		private Utils() { }

		public static string GetDateWithOffsetFormatString(Account acct, DateTime date)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			int timeOffset;
			if (settings.AllowUsersChangeTimeZone)
			{
				timeOffset = acct.UserOfAccount.Settings.DefaultTimeZone;
			}
			else
			{
				timeOffset = settings.DefaultTimeZone;
			}
			DateTime dt = GetMessageDateWithOffset(date, GetMinutesTimesOffset(timeOffset));
			return FormatDate(acct, dt);
		}

		public static DateTime GetMessageDateWithOffset(DateTime msgDateReceivedOriginal, int timeOffsetInMinutes)
		{
			DateTime msgDate;

			msgDate = msgDateReceivedOriginal;
			if (timeOffsetInMinutes != 0 && msgDate >= Constants.MinDateWithMaxZoneTimeOffset)
			{
				TimeZone localZone = TimeZone.CurrentTimeZone;
				if (localZone.IsDaylightSavingTime(DateTime.Now))
				{
					msgDate = msgDate.AddMinutes(timeOffsetInMinutes + 60);
				}
				else
				{
					msgDate = msgDate.AddMinutes(timeOffsetInMinutes);
				}
			} 
			return msgDate;
		}

		public static int GetMinutesTimesOffset(int timeZone)
		{
			int timeOffset = 0;
			switch (timeZone)
			{
				case 0:
					TimeZone tz = TimeZone.CurrentTimeZone;
					TimeSpan ts = tz.GetUtcOffset(DateTime.Now);
					timeOffset = ((tz.IsDaylightSavingTime(DateTime.Now)) ? ts.Hours - 1 : ts.Hours) * 60;
					timeOffset += ts.Minutes;
					break;
				case 1:
					timeOffset = -12 * 60;
					break;
				case 2:
					timeOffset = -11 * 60;
					break;
				case 3:
					timeOffset = -10 * 60;
					break;
				case 4:
					timeOffset = -9 * 60;
					break;
				case 5:
					timeOffset = -8 * 60;
					break;
				case 6:
				case 7:
					timeOffset = -7 * 60;
					break;
				case 8:
				case 9:
				case 10:
				case 11:
					timeOffset = -6 * 60;
					break;
				case 12:
				case 13:
				case 14:
					timeOffset = -5 * 60;
					break;
				case 15:
				case 16:
				case 17:
					timeOffset = -4 * 60;
					break;
				case 18:
					timeOffset = (int)(-3.5 * 60);
					break;
				case 19:
				case 20:
				case 21:
					timeOffset = -3 * 60;
					break;
				case 22:
					timeOffset = -2 * 60;
					break;
				case 23:
				case 24:
					timeOffset = -60;
					break;
				case 25:
				case 26:
					timeOffset = 0;
					break;
				case 27:
				case 28:
				case 29:
				case 30:
				case 31:
					timeOffset = 60;
					break;
				case 32:
				case 33:
				case 34:
				case 35:
				case 36:
				case 37:
					timeOffset = 2 * 60;
					break;
				case 38:
				case 39:
				case 40:
				case 41:
					timeOffset = 3 * 60;
					break;
				case 42:
					timeOffset = (int)(3.5 * 60);
					break;
				case 43:
				case 44:
					timeOffset = 4 * 60;
					break;
				case 45:
					timeOffset = (int)(4.5 * 60);
					break;
				case 46:
				case 47:
					timeOffset = 5 * 60;
					break;
				case 48:
					timeOffset = (int)(5.5 * 60);
					break;
				case 49:
					timeOffset = 5 * 60+45;
					break;
				case 50:
				case 51:
				case 52:
					timeOffset = 6 * 60;
					break;
				case 53:
					timeOffset = (int)(6.5 * 60);
					break;
				case 54:
				case 55:
					timeOffset = 7 * 60;
					break;
				case 56:
				case 57:
				case 58:
				case 59:
				case 60:
					timeOffset = 8 * 60;
					break;
				case 61:
				case 62:
				case 63:
					timeOffset = 9 * 60;
					break;
				case 64:
				case 65:
					timeOffset = (int)(9.5 * 60);
					break;
				case 66:
				case 67:
				case 68:
				case 69:
				case 70:
					timeOffset = 10 * 60;
					break;
				case 71:
					timeOffset = 11 * 60;
					break;
				case 72:
				case 73:
					timeOffset = 12 * 60;
					break;
				case 74:
					timeOffset = 13 * 60;
					break;
			}
			return timeOffset;
		}

		public static string FormatDate(Account acct, DateTime date)
		{
			if (acct != null)
			{
				if ((acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
				{
					if (acct.UserOfAccount.Settings.DefaultDateFormat == null) return string.Empty;
                    string result;

					switch (acct.UserOfAccount.Settings.DefaultDateFormat.ToLower(CultureInfo.InvariantCulture))
					{
						case Constants.DateFormats.Default:
                            result = date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            break;
						case Constants.DateFormats.DDMMYY:
                            result = date.ToString("dd/MM/yy", CultureInfo.InvariantCulture);
                            break;
						case Constants.DateFormats.MMDDYY:
                            result = date.ToString("MM/dd/yy", CultureInfo.InvariantCulture);
                            break;
						case Constants.DateFormats.DDMonth:
							result = date.ToString("dd MMM", CultureInfo.InvariantCulture);
                            break;
						default:
                            result = acct.UserOfAccount.Settings.DefaultDateFormat;

							Regex r = new Regex("yyyy", RegexOptions.Singleline);
							result = r.Replace(result, date.ToString("yyyy", CultureInfo.InvariantCulture));

							r = new Regex("yy", RegexOptions.Singleline);
							result = r.Replace(result, date.ToString("yy", CultureInfo.InvariantCulture));

							r = new Regex("y", RegexOptions.Singleline);
							result = r.Replace(result, date.DayOfYear.ToString(CultureInfo.InvariantCulture));

							r = new Regex("dd", RegexOptions.Singleline);
							result = r.Replace(result, date.Day.ToString(CultureInfo.InvariantCulture));

							r = new Regex("mm", RegexOptions.Singleline);
							result = r.Replace(result, date.Month.ToString(CultureInfo.InvariantCulture));

							int quarter = 0;
							if ((date.Month >= 1)&&(date.Month <= 3)) quarter = 1;
							if ((date.Month >= 4)&&(date.Month <= 6)) quarter = 2;
							if ((date.Month >= 7)&&(date.Month <= 9)) quarter = 3;
							if ((date.Month >= 10)&&(date.Month <= 12)) quarter = 4;
							r = new Regex("q", RegexOptions.Singleline);
							result = r.Replace(result, quarter.ToString(CultureInfo.InvariantCulture));

							r = new Regex("ww", RegexOptions.Singleline);
							result = r.Replace(result, ((date.DayOfYear / 7) + 1).ToString(CultureInfo.InvariantCulture));

							r = new Regex("w", RegexOptions.Singleline);
							result = r.Replace(result, date.ToString("ddd", CultureInfo.InvariantCulture));

							r = new Regex("month", RegexOptions.Singleline);
							result = r.Replace(result, date.ToString("MMM", CultureInfo.InvariantCulture));
                            break;
					}

                    result += (acct.UserOfAccount.Settings.DefaultTimeFormat == TimeFormats.F24)
                        ? date.ToString(" HH:mm") : date.ToString(" hh:mm tt");
                    return result;
				}
			}
			return date.ToString(CultureInfo.InvariantCulture);
		}

		public static string ConvertToDBString(Account acct, string messageString)
		{
			Encoding dbEncoding = Encoding.UTF8;
			if ((acct != null) && (acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
			{
				try
				{
					dbEncoding = GetEncodingByCodePage(acct.UserOfAccount.Settings.DbCharset);
				}
				catch {}
				byte[] bytes = dbEncoding.GetBytes(messageString);
				messageString = Encoding.Default.GetString(bytes);
			}
			return messageString;
		}

		public static string ConvertFromDBString(Account acct, string dbString)
		{
			Encoding dbEncoding = Encoding.UTF8;
			if ((acct != null) && (acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
			{
				try
				{
					dbEncoding = GetEncodingByCodePage(acct.UserOfAccount.Settings.DbCharset);
				}
				catch {}
				byte[] bytes = Encoding.Default.GetBytes(dbString);
				dbString = dbEncoding.GetString(bytes);
			}
			return dbString;
		}

		public static string[] GetSupportedSkins(string skinsFolder)
		{
			ArrayList skinsArr = new ArrayList();
			if (Directory.Exists(skinsFolder))
			{
				string[] skins = Directory.GetDirectories(skinsFolder);
				foreach (string skin in skins)
				{
					skinsArr.Add(Path.GetFileName(skin));
				}
			}
			return (string[])skinsArr.ToArray(typeof (string));
		}

		public static int GetCurrentSkinIndex(string[] supportedSkins, string skinName)
		{
			for (int i = 0; i < supportedSkins.Length; i++)
			{
				if (string.Compare(skinName, supportedSkins[i], true, CultureInfo.InvariantCulture) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		public static string[] GetSupportedLangs()
		{
			ArrayList arr = new ArrayList();
			string langsXml = Path.Combine(GetDataFolderPath(), @"langs\langs.xml");
			if (File.Exists(langsXml))
			{
				XPathDocument xpathDoc = new XPathDocument(langsXml);
				XPathNavigator nav = xpathDoc.CreateNavigator();
				XPathNodeIterator langIter = nav.Select(string.Format("langs/lang/FriendlyName"));
				while (langIter.MoveNext())
				{
					arr.Add(langIter.Current.Value);
				}
			}
			return (arr.Count > 0) ? (string[])arr.ToArray(typeof(string)) : new string[] { Constants.defaultLang };
		}

		public static string EncodeHtml(string s)
		{
			Regex rChar;
			rChar = new Regex("[\x0-\x8\xB-\xC\xE-\x1F]+");
			s = rChar.Replace(s, " ");

			StringBuilder sb = new StringBuilder(s);
			sb.Replace("&", "&amp;");
			sb.Replace("<", "&lt;");
			sb.Replace(">", "&gt;");
			return sb.ToString();
		}

        public static string EncodeHtmlSimple(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            return sb.ToString();
        }

		public static string DecodeHtml(string s)
		{
			StringBuilder sb = new StringBuilder(s);
			sb.Replace("&lt;", "<");
			sb.Replace("&gt;", ">");
			sb.Replace("&amp;", "&");
			return sb.ToString();
		}

		public static string AttributeQuote(string s)
		{
			s = s.Replace("\"", "&quot;");
			return s;
		}

		public static string EncodeCDATABody(string s)
		{
			Regex rChar;
			rChar = new Regex("[\x0-\x8\xB-\xC\xE-\x1F]+");
			s = rChar.Replace(s, " ");

			return s.Replace(@"]]>", @"&#93;&#93;&gt;");
		}

		public static string DecodeHtmlBody(string s)
		{
			return s.Replace(@"&#93;&#93;&gt;", @"]]>");
		}

		public static string EncodeJsSaveString(string s)
		{
			StringBuilder sb = new StringBuilder(s);
			sb.Replace("\\", @"\\");
			sb.Replace("'", @"\'");
			sb.Replace("\"", @"\""");
			sb.Replace("\t", @"\t");
			sb.Replace("\r", @"\r");
			sb.Replace("\n", @"\n");
			return sb.ToString();
		}

		public static string GetMD5DigestHexString(string str)
		{
			byte[] bytes = Encoding.Default.GetBytes(str);
			MD5 md5 = new MD5CryptoServiceProvider();
			bytes = md5.ComputeHash(bytes);
			return BytesToHexString(bytes);
		}

		public static string BytesToHexString(byte[] bytes)
		{
			StringBuilder result = new StringBuilder();
			foreach(byte b in bytes)
			{
				result.Append(b.ToString("x2"));
			}
			return result.ToString();
		}

		public static string GetAttachmentMimeTypeFromFileExtension(string fileExtension)
		{
			string result = "application/octet-stream";

			if ((fileExtension == null) || (fileExtension.Length == 0))
			{
				return result;
			}

			switch (fileExtension.ToLower(CultureInfo.InvariantCulture))
			{
				case "gif": return "image/gif";
				case "png": return "image/png";
				case "jpe":
				case "jpg":
				case "jpeg": return "image/jpeg";
				case "tif":
				case "tiff": return "image/tiff";
				case "bin":
				case "dms":
				case "lha":
				case "lzh":
				case "exe":
				case "class":
				case "dll": return "application/octet-stream";
				case "js": return "application/x-javascript";
				case "swf": return "application/x-shockwave-flash";
				case "doc": return "application/msword";
				case "zip": return "application/zip";
				case "ai":
				case "eps":
				case "ps": return "application/postscript";
				case "pdf": return "application/pdf";
				case "rtf": return "application/rtf";
				case "ppt": return "application/vnd.ms-powerpoint";
				case "htm":
				case "html": return "text/html";
				case "css": return "text/css";
				case "rtx": return "text/richtext";
				case "txt":
				case "asc": return "text/plain";
				case "xml": return "text/xml";
				case "wav": return "audio/x-wav";
				case "mid":
				case "midi": return "audio/midi";
				case "mpga":
				case "mp2":
				case "mp3": return "audio/mpeg";
				case "aif":
				case "aiff": return "audio/x-aiff";
				case "ra": return "audio/x-realaudio";
				case "mpeg":
				case "mpg":
				case "mpe": return "video/mpeg";
				case "qt":
				case "mov": return "video/quicktime";
				case "avi": return "video/x-msvideo";
			}


			RegistryKey regKey = null;
			try
			{
				if (fileExtension[0] == '.')
				{
					fileExtension = fileExtension.Remove(0, 1);
				}

				regKey = Registry.ClassesRoot;
				if (regKey != null)
				{
					regKey = regKey.OpenSubKey(string.Format(CultureInfo.InvariantCulture, ".{0}", fileExtension));
				}
				if (regKey != null)
				{
					result = (string)regKey.GetValue("Content Type");
				}
			}
			catch
			{
				return result;
			}
			finally
			{
				if (regKey != null) regKey.Close();
			}
			return result ?? "application/octet-stream";
		}

		public static string GetTempFolderName(HttpSessionState s)
		{
			string tempFolder = string.Empty;
			if (s != null)
			{
				Account acct = s[Constants.sessionAccount] as Account;
				if (acct != null)
				{
					tempFolder = GetMD5DigestHexString(s.SessionID);
					FileSystem fs = new FileSystem(acct.Email, acct.ID, false);
					tempFolder = fs.CreateFolder(tempFolder);
				}
				if (s[Constants.sessionTempFolder] != null)
				{
					if (string.Compare(s[Constants.sessionTempFolder].ToString(), tempFolder, true, CultureInfo.InvariantCulture) != 0)
					{
						if (Directory.Exists(s[Constants.sessionTempFolder].ToString()))
						{
							Directory.Delete(s[Constants.sessionTempFolder].ToString(), true);
						}
					}
					s[Constants.sessionTempFolder] = tempFolder;
				}
				else
				{
					s.Add(Constants.sessionTempFolder, tempFolder);
				}
			}
			return tempFolder;
		}

		public static string ConvertToUtf7Modified(string src)
		{
			return ImapUtils.ToUtf7String(src);
		}

		public static string ConvertFromUtf7Modified(string src)
		{
			return ImapUtils.FromUtf7String(src);
		}

		public static string CreateTempFilePath(string tempFolderName, string filename)
		{
			string tempFilePath = string.Format("{0}{1}", GetMD5DigestHexString(filename), Path.GetExtension(filename));
			tempFilePath = Path.Combine(tempFolderName, tempFilePath);
			if (File.Exists(tempFilePath))
			{
				int i = 1;
				while (File.Exists(tempFilePath))
				{
					tempFilePath = string.Format("{0}_{2}{1}", GetMD5DigestHexString(filename), Path.GetExtension(filename), i);
					tempFilePath = Path.Combine(tempFolderName, tempFilePath);
					i++;
				}
			}
			if (tempFilePath.Length > Constants.PathMaxLength)
			{
				System.Diagnostics.Debug.Assert(false);
			}
			return tempFilePath;
		}

		public static string EncodePassword(string password)
		{
			if (password == null) return password;

			StringBuilder sb = new StringBuilder();
			byte[] plainBytes = Encoding.UTF8.GetBytes(password);
			if (plainBytes.Length > 0)
			{
				byte encodeByte = plainBytes[0];
				sb.Append(encodeByte.ToString("x2"));
				for (int i = 1; i < plainBytes.Length; i++)
				{
					plainBytes[i] = (byte)(plainBytes[i] ^ encodeByte);
					sb.Append(plainBytes[i].ToString("x2"));
				}
			}
			return sb.ToString();
		}

		public static string DecodePassword(string password)
		{
			string result = string.Empty;
			if ((password.Length > 0) && (password.Length % 2 == 0))
			{
				try
				{
					byte decodeByte = byte.Parse(password.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
					byte[] plainBytes = new byte[password.Length / 2];
					plainBytes[0] = decodeByte;
					if (password.Length > 2)
					{
						int startIndex = 2;
						int currentByte = 1;
						do
						{
							string hexByte = password.Substring(startIndex, 2);
							plainBytes[currentByte] = (byte)(byte.Parse(hexByte, NumberStyles.HexNumber, CultureInfo.InvariantCulture) ^ decodeByte);
							startIndex += 2;
							currentByte++;
						}
						while (startIndex < password.Length);
					}
					result = Encoding.UTF8.GetString(plainBytes);
				}
				catch
				{
					// can't parse hexByte
				}
			}
			return result;
		}

		public static string GetLogFilePath()
		{
			string logPath = string.Empty;
			string dataFolder = GetDataFolderPath();
			if (dataFolder != null)
			{
				logPath = Path.Combine(dataFolder, Constants.logFilename);
			}
			return logPath;
		}

		public static string LoadFromFile(string filename, Encoding enc, int index, int count)
		{
			FileInfo fi = new FileInfo(filename);
			if (fi.Exists)
			{
				if (count < 0) count = (int)fi.Length;
				if ((index + count) <= fi.Length)
				{
					char[] buffer = new char[count];
					using (StreamReader sr = new StreamReader(filename, enc, true))
					{
						int reads = index;
						while (count > 0)
						{
							reads += sr.ReadBlock(buffer, reads, count);
							count -= reads;
						}
					}
					return new string(buffer);
				}
			}
			return null;
		}

		public static string LoadFromFile(string filename, Encoding enc)
		{
			return LoadFromFile(filename, enc, 0, -1);
		}

		public static void ClearLog()
		{
			string logFile = GetLogFilePath();
			FileInfo fi = new FileInfo(logFile);
			if (fi.Exists)
			{
				try
				{
					using (FileStream fs = fi.Open(FileMode.Create)){}
				}
				catch {}
			}
		}

		public static Encoding GetEncodingByCodePage(int codePage)
		{
			Encoding enc = Encoding.Default;
			try
			{
				enc = Encoding.GetEncoding(codePage);
			}
			catch {}
			return enc;
		}

		public static void ReverseCollection(ref CollectionBase collection)
		{
			Stack s = new Stack(collection);
			object[] objArr = s.ToArray();

			collection.Clear();
			foreach (object obj in objArr)
			{
				((IList)collection).Add(obj);
			}
		}

		public static int RandMsgID(int id_msg)
		{
			Random rnd = new Random();
			return id_msg + rnd.Next(1, 10);
		}

		public static string GetMessagePlainReplyToBody(Account acct, MailMessage mailMessage)
		{
			string signature = string.Empty;
			if ((acct.SignatureOptions & SignatureOptions.AddSignatureToAllOutgoingMessages) > 0)
			{
				if (acct.SignatureType == SignatureType.Html)
				{
					MailMessage tempMsg = new MailMessage();
					tempMsg.BodyHtmlText = acct.Signature;
					tempMsg.MakePlainBodyFromHtmlBody();
					signature = tempMsg.BodyPlainText;
				}
				else if (acct.SignatureType == SignatureType.Plain)
				{
					signature = acct.Signature;
				}
			}

			string plainBody;
			if ((mailMessage.BodyPlainText == null) || (mailMessage.BodyPlainText.Length == 0))
			{
				// clone because we need Attach.SavedAs
				MailMessage cloneMsg = mailMessage.Clone();
				cloneMsg.Parser.HtmlToPlainMode = HtmlToPlainAutoConvert.IfHtml;
				cloneMsg.Parser.HtmlToPlainOptions = HtmlToPlainConvertOptions.AddImgAltText | HtmlToPlainConvertOptions.AddUriForAHRef;
				cloneMsg.Parser.Apply();
				plainBody = cloneMsg.BodyPlainText;
			}
			else
			{
				plainBody = mailMessage.BodyPlainText;
			}

			string mess = "\r\n---- " + "OriginalMessage" + " ----\n";
			mess += "From" + ": " + EncodeHtml(mailMessage.From.ToString()) + "\n";
			mess += "To" + ": " + EncodeHtml(mailMessage.To.ToString()) + "\n";
			if (mailMessage.Cc.ToString().Length > 0)
				mess += "CC" + ": " + EncodeHtml(mailMessage.Cc.ToString()) + "\n";
			mess += "Sent" + ": " + DecodeHtml(GetDateWithOffsetFormatString(acct, mailMessage.Date)) + "\n";
			mess += "Subject" + ": " + EncodeHtml(mailMessage.Subject) + "\n\n";
			mess += DecodeHtml(plainBody);
			mess = mess.Replace("\n", "\n>");
			mess = "\r\n\r\n\r\n" + signature + mess;

			return mess;
		}

		public static string GetMessageHtmlReplyToBody(Account acct, MailMessage mailMessage)
		{
			string signature = string.Empty;
			if ((acct.SignatureOptions & SignatureOptions.AddSignatureToAllOutgoingMessages) > 0)
			{
				signature = acct.Signature;
			}

			string htmlBody;
			if ((mailMessage.BodyHtmlText == null) || (mailMessage.BodyHtmlText.Length == 0))
			{
                htmlBody = MakeHtmlBodyFromPlainBody(mailMessage.BodyPlainText);
			}
			else
			{
				htmlBody = mailMessage.BodyHtmlText;
			}

			string mess = @"<br/>" + signature + @"<br/><blockquote style=""border-left: solid 2px #000000; margin-left: 5px; padding-left: 5px;"">";
			mess += "---- " + "OriginalMessage" + " ----<br/>";
			mess += "<b>" + "From" + "</b>: " + EncodeHtml(mailMessage.From.ToString()) + "<br/>";
			mess += "<b>" + "To" + "</b>: " + EncodeHtml(mailMessage.To.ToString()) + "<br/>";
			if (mailMessage.Cc.ToString().Length > 0)
				mess += "<b>" + "CC" + "</b>: " + EncodeHtml(mailMessage.Cc.ToString()) + "<br/>";
			mess += "<b>" + "Sent" + "</b>: " + EncodeHtml(GetDateWithOffsetFormatString(acct, mailMessage.Date)) + "<br/>";
			mess += "<b>" + "Subject" + "</b>: " + EncodeHtml(mailMessage.Subject) + "<br/><br/>";
			mess += htmlBody + "</blockquote>";

			return mess;
		}

		public static string GetAttachmentDownloadLink(Attachment attach, bool isViewLink)
		{
			string attachmentName = (attach.Filename.Length > 0) ? attach.Filename : attach.Name;

			string downloadStr = "download_view_attachment.aspx?download={0}&filename={1}&temp_filename={2}";
			string filename = Path.GetFileName(attach.SavedAs);
			downloadStr = string.Format(downloadStr, (isViewLink) ? "0" : "1", HttpUtility.UrlEncode(attachmentName), HttpUtility.UrlEncode(filename));
			
			return downloadStr;
		}

		public static string GetMessageDownloadLink(WebMailMessage msg, long id_folder, string folder_full_path)
		{
			if ((msg != null))
			{
				return string.Format(@"download_view_attachment.aspx?id_msg={0}&uid={1}&id_folder={2}&folder_path={3}",
					msg.IDMsg,
					HttpUtility.UrlEncode(((msg.StrUid != null) && (msg.StrUid.Length > 0)) ? msg.StrUid : msg.IntUid.ToString(CultureInfo.InvariantCulture)),
					id_folder,
					HttpUtility.UrlEncode(folder_full_path));
			}
			return string.Empty;
		}

		public static string GetAttachmentIconLink(Attachment attach)
		{
			string iconLink;
			switch (attach.ContentType.ToLower(CultureInfo.InvariantCulture))
			{
				case "application/asp":
					iconLink = "images/icons/application_asp.gif";
					break;
				case "application/css":
					iconLink = "images/icons/application_css.gif";
					break;
				case "application/doc":
					iconLink = "images/icons/application_doc.gif";
					break;
				case "application/html":
					iconLink = "images/icons/application_html.gif";
					break;
				case "application/pdf":
					iconLink = "images/icons/application_pdf.gif";
					break;
				case "application/xls":
					iconLink = "images/icons/application_xls.gif";
					break;
				case "image/bmp":
					iconLink = "images/icons/image_bmp.gif";
					break;
				case "image/gif":
					iconLink = "images/icons/image_gif.gif";
					break;
				case "image/jpg":
				case "image/jpeg":
					iconLink = "images/icons/image_jpeg.gif";
					break;
				case "image/tif":
				case "image/tiff":
					iconLink = "images/icons/image_tiff.gif";
					break;
				case "text/plain":
					iconLink = "images/icons/text_plain.gif";
					break;
				default:
					iconLink = "images/icons/attach.gif";
					break;
			}
			return iconLink;
		}

		public static string GetFriendlySize(long byteSize)
		{
			WebmailResourceManager man = (new WebmailResourceManagerCreator()).CreateResourceManager();
			double size = Math.Round((double)byteSize / 1024);
			double mbSize = size / 1024;
			string str = "{0}{1}";
			return (mbSize > 1) ? string.Format(str, Math.Ceiling(mbSize * 10)/10, man.GetString("Mb")) : string.Format(str, size, man.GetString("Kb"));
		}

		public static string GetShortFilename(string filename)
		{
			if (filename.Length > 15)
			{
				return string.Format(@"{0}...", filename.Substring(0, 12));
			}
			return filename;
		}

		public static int GetCodePageNumber(string codePageNum)
		{
			int result = -1;

			for(int i = 0; Constants.PageName.Length > i; i++)
			{
				if(Constants.PageName[i] == codePageNum)
				{
					result = Constants.PageNumber[i];
				}
			}

			return result;
		}

		public static string GetCodePageName(int codePageNum)
		{
			string result = "-1";

			for(int i = 0; Constants.PageNumber.Length > i; i++)
			{
				if(Constants.PageNumber[i] == codePageNum)
				{
					result = Constants.PageName[i];
				}
			}

			return result;
		}

		public static string ConvertHtmlToPlain(string htmlText)
		{
			MailMessage msg = new MailMessage();
			msg.BodyHtmlText = htmlText;
			msg.MakePlainBodyFromHtmlBody();
			return msg.BodyPlainText;
		}

		public static string MakeHtmlBodyFromPlainBody(string bodyPlain)
		{
			StringBuilder sb = new StringBuilder(EncodeHtml(bodyPlain));
			string urlPattern = @"(http|https|ftp|nntp|file|telnet|gopher|wais|prospero)://[\w\d!\$&'\(\)\*\+-\./:\?@_~%=;,#]+";//!$&'()*+-./:?@_~%=;,
			Regex r = new Regex(urlPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);//-_.!~*'();/?:@&=+,$%
			Match m = r.Match(sb.ToString(), 0);
			while (m.Success)
			{
				string link = string.Format(CultureInfo.InvariantCulture, @"<a href=""{0}"">{0}</a>", m.Value);

				sb.Replace(m.Value, link, m.Index, m.Length);
				m = r.Match(sb.ToString(), m.Index + link.Length);
			}
			Regex newLineRegex = new Regex("((\r){0,1}\n){1}", RegexOptions.Singleline);
			sb = new StringBuilder(newLineRegex.Replace(sb.ToString(), "<br>"));

            // MailBee.Html
            Processor pr = new Processor();
            // MailBee.Html
            pr.Dom.OuterHtml = sb.ToString();
            RuleSet rs = new RuleSet();

            TagAttributeCollection attrsToAdd = new TagAttributeCollection();
            TagAttribute addAttr = new TagAttribute();
            addAttr.Name = "target";
            addAttr.Value = "\"_blank\"";
            attrsToAdd.Add(addAttr);
            rs.AddTagProcessingRule("a", null, attrsToAdd, null, false);

            pr.Dom.Process(rs, null);

            return pr.Dom.OuterHtml;
		}

		public static ArrayList Split(string src, string separator)
		{
			ArrayList result = new ArrayList();
			int startIndex = 0;
			while (startIndex < src.Length)
			{
				int index = src.IndexOf(separator, startIndex);
				if (index >= 0)
				{
					result.Add(src.Substring(startIndex, index - startIndex));
					startIndex = index + separator.Length;
				}
				else
				{
					result.Add(src.Substring(startIndex, src.Length - startIndex));
					break;
				}
			}
			return result;
		}

		public static string GetDelimeter()
		{
			string delimeter;

			if(Environment.Version.Major == 1)
			{
				delimeter = ":";
			}
			else
			{
				delimeter = "$";
			}

			return delimeter;
		}

		public static string GetAddressesFriendlyName(EmailAddressCollection coll)
		{
			if (coll == null) return string.Empty;

			StringBuilder sb = new StringBuilder();
			foreach (EmailAddress addr in coll)
			{
				sb.AppendFormat(@"{0}, ",
					(addr.DisplayName != null && addr.DisplayName.Length > 0) ? addr.DisplayName : addr.Email);
			}
			if (sb.Length > 2) sb.Remove(sb.Length - 2, 2);
			return sb.ToString();
		}
		
		public static string GetLocalizedFolderNameByType(Folder fld)
		{
			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			switch (fld.Type)
			{
				case FolderType.Drafts:
					return resMan.GetString(@"FolderDrafts");
				case FolderType.Inbox:
					return resMan.GetString(@"FolderInbox");
				case FolderType.SentItems:
					return resMan.GetString(@"FolderSentItems");
				case FolderType.Trash:
					return resMan.GetString(@"FolderTrash");
			}
			return fld.Name;
		}

		public static string GetDataFolderPath()
		{
            string strDataFolderPath;
			if (HttpContext.Current.Application[Constants.appSettingsDataFolderPath] != null )
			{
				strDataFolderPath = (string)HttpContext.Current.Application[Constants.appSettingsDataFolderPath];
			}
            else
            {
			    strDataFolderPath = ConfigurationManager.AppSettings[Constants.appSettingsDataFolderPath];
            }

			/*converting relative path in absolute*/
			Uri appUri = new Uri(HttpContext.Current.Request.PhysicalApplicationPath);
			Uri newUri = new Uri(appUri, strDataFolderPath);
			strDataFolderPath = newUri.LocalPath;

			return strDataFolderPath;
		}

        public static Hashtable ReadWebmailTab(string xmailRootPath)
        {
            if (xmailRootPath == null) return null;
            Hashtable result = null;
            if (Directory.Exists(xmailRootPath))
            {
                string path = Path.Combine(xmailRootPath, Constants.webmailTab);
                if (File.Exists(path))
                {
                    result = new Hashtable();
                    try
                    {
                        using (StreamReader sr = new StreamReader(File.OpenRead(path)))
                        {
                            string str;
                            while ((str = sr.ReadLine()) != null)
                            {
                                str = str.Trim();
                                if (str.StartsWith("#")) continue;
                                string[] keyValue = str.Split(new char[] { '\t' });
                                if (keyValue.Length == 2)
                                {
                                    string key = keyValue[0].Trim(new char[] { '"' });
                                    string value = keyValue[1].Trim(new char[] { '"' });
                                    if (!result.ContainsKey(key))
                                    {
                                        result.Add(key, value);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new WebMailIOException(ex);
                    }
                }
            }
            return result;
        }

//		public static string DecodePassword(string encoded_pwd)
//		{
//			byte[] raw=Encoding.UTF8.GetBytes(encoded_pwd);
//			string ret=string.Empty;
//			for(int i=0;i<raw.Length;i++)
//				ret+=(char)(raw[i] ^ 101);
//			return ret;
//		}
	}
}
