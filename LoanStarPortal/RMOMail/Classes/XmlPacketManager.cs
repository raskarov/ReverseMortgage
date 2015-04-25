using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Xml;
using System.Xml.XPath;
using MailBee;
using MailBee.Html;
using MailBee.Mime;
using Attribute = MailBee.Html.TagAttribute;
using AttributeCollection = MailBee.Html.TagAttributeCollection;


namespace WebMailPro
{
	public class XmlPacketAttachment
	{
		public int size = 0;
		public bool inline = false;
		public string temp_name = string.Empty;
		public string name = string.Empty;
		public string mime_type = string.Empty;

		public static XmlPacketAttachment CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketAttachment result = new XmlPacketAttachment();

			string attrValue = xpathIterator.Current.GetAttribute("size", "");
			if (attrValue.Length > 0) result.size = Convert.ToInt32(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("inline", "");
			if (attrValue.Length > 0) result.inline = (string.Compare(attrValue, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;

			XPathNodeIterator tempNameIter = xpathIterator.Current.Select("temp_name");
			if (tempNameIter.MoveNext()) result.temp_name = Utils.DecodeHtml(tempNameIter.Current.Value);

			XPathNodeIterator nameIter = xpathIterator.Current.Select("name");
			if (nameIter.MoveNext()) result.name = Utils.DecodeHtml(nameIter.Current.Value);

			XPathNodeIterator mimeTypeIter = xpathIterator.Current.Select("mime_type");
			if (mimeTypeIter.MoveNext()) result.mime_type = Utils.DecodeHtml(mimeTypeIter.Current.Value);

			return result;
		}
	}

	public class XmlPacketGroup
	{
		public int id = 0;
		public string name = string.Empty;
		public bool organization = false;
		public string email = string.Empty;
		public string company = string.Empty;
		public string street = string.Empty;
		public string city = string.Empty;
		public string state = string.Empty;
		public string zip = string.Empty;
		public string country = string.Empty;
		public string phone = string.Empty;
		public string fax = string.Empty;
		public string web = string.Empty;
		public XmlPacketContact[] contacts = null;
		public XmlPacketContact[] new_contacts = null;

		public static XmlPacketGroup CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketGroup result = new XmlPacketGroup();

			string attrValue = xpathIterator.Current.GetAttribute("id", "");
			if (attrValue.Length > 0) result.id = Convert.ToInt32(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("organization", "");
			if (attrValue.Length > 0) result.organization = (string.Compare(attrValue, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;

			XPathNodeIterator nameIter = xpathIterator.Current.Select("name");
			if (nameIter.MoveNext()) result.name = Utils.DecodeHtml(nameIter.Current.Value);

			XPathNodeIterator emailIter = xpathIterator.Current.Select("email");
			if (emailIter.MoveNext()) result.email = Utils.DecodeHtml(emailIter.Current.Value);

			XPathNodeIterator companyIter = xpathIterator.Current.Select("company");
			if (companyIter.MoveNext()) result.company = Utils.DecodeHtml(companyIter.Current.Value);

			XPathNodeIterator streetIter = xpathIterator.Current.Select("street");
			if (streetIter.MoveNext()) result.street = Utils.DecodeHtml(streetIter.Current.Value);

			XPathNodeIterator cityIter = xpathIterator.Current.Select("city");
			if (cityIter.MoveNext()) result.city = Utils.DecodeHtml(cityIter.Current.Value);

			XPathNodeIterator stateIter = xpathIterator.Current.Select("state");
			if (stateIter.MoveNext()) result.state = Utils.DecodeHtml(stateIter.Current.Value);

			XPathNodeIterator zipIter = xpathIterator.Current.Select("zip");
			if (zipIter.MoveNext()) result.zip = Utils.DecodeHtml(zipIter.Current.Value);

			XPathNodeIterator countryIter = xpathIterator.Current.Select("country");
			if (countryIter.MoveNext()) result.country = Utils.DecodeHtml(countryIter.Current.Value);

			XPathNodeIterator faxIter = xpathIterator.Current.Select("fax");
			if (faxIter.MoveNext()) result.fax = Utils.DecodeHtml(faxIter.Current.Value);

			XPathNodeIterator phoneIter = xpathIterator.Current.Select("phone");
			if (phoneIter.MoveNext()) result.phone = Utils.DecodeHtml(phoneIter.Current.Value);

			XPathNodeIterator webIter = xpathIterator.Current.Select("web");
			if (webIter.MoveNext()) result.web = Utils.DecodeHtml(webIter.Current.Value);

			ArrayList contacts = new ArrayList();
			XPathNodeIterator contactsIter = xpathIterator.Current.Select("contacts/contact");
			while (contactsIter.MoveNext())
			{
				XmlPacketContact contact = XmlPacketContact.CreateWithXPath(contactsIter);
				if (contact != null) contacts.Add(contact);
			}
			result.contacts = (XmlPacketContact[])contacts.ToArray(typeof(XmlPacketContact));

			XPathNodeIterator newContactsIter = xpathIterator.Current.Select("new_contacts/contact");
			contacts = new ArrayList();
			while (newContactsIter.MoveNext())
			{
				XmlPacketContact contact = XmlPacketContact.CreateWithXPath(newContactsIter);
				if (contact != null) contacts.Add(contact);
			}
			result.new_contacts = (XmlPacketContact[])contacts.ToArray(typeof(XmlPacketContact));

			return result;
		}
	}

	public class XmlPacketContact
	{
		public long id = 0;
		public short primary_email = 0;
		public short use_friendly_name = 0;
		public string fullname = string.Empty;
		public byte birthday_day = 0;
		public byte birthday_month = 0;
		public short birthday_year = 0;
		public string personal_email = string.Empty;
		public string personal_street = string.Empty;
		public string personal_city = string.Empty;
		public string personal_state = string.Empty;
		public string personal_zip = string.Empty;
		public string personal_country = string.Empty;
		public string personal_fax = string.Empty;
		public string personal_phone = string.Empty;
		public string personal_mobile = string.Empty;
		public string personal_web = string.Empty;
		public string business_email = string.Empty;
		public string business_company = string.Empty;
		public string business_job_title = string.Empty;
		public string business_department = string.Empty;
		public string business_office = string.Empty;
		public string business_street = string.Empty;
		public string business_city = string.Empty;
		public string business_state = string.Empty;
		public string business_zip = string.Empty;
		public string business_country = string.Empty;
		public string business_fax = string.Empty;
		public string business_phone = string.Empty;
		public string business_web = string.Empty;
		public string other_email = string.Empty;
		public string other_notes = string.Empty;
		public XmlPacketGroup[] groups = null;

		public static XmlPacketContact CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketContact result = new XmlPacketContact();

			string attrValue = xpathIterator.Current.GetAttribute("id", "");
			if (attrValue.Length > 0) result.id = Convert.ToInt64(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("primary_email", "");
			if (attrValue.Length > 0) result.primary_email = Convert.ToInt16(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("use_friendly_nm", "");
			if (attrValue.Length > 0) result.use_friendly_name = Convert.ToInt16(attrValue);

			XPathNodeIterator fullnameIter = xpathIterator.Current.Select("fullname");
			if (fullnameIter.MoveNext()) result.fullname = Utils.DecodeHtml(fullnameIter.Current.Value);

			XPathNodeIterator birthdayIter = xpathIterator.Current.Select("birthday");
			if (birthdayIter.MoveNext())
			{
				attrValue = birthdayIter.Current.GetAttribute("day", "");
				if (attrValue.Length > 0) result.birthday_day = Convert.ToByte(attrValue, CultureInfo.InvariantCulture);
				
				attrValue = birthdayIter.Current.GetAttribute("month", "");
				if (attrValue.Length > 0) result.birthday_month = Convert.ToByte(attrValue, CultureInfo.InvariantCulture);

				attrValue = birthdayIter.Current.GetAttribute("year", "");
				if (attrValue.Length > 0) result.birthday_year = Convert.ToInt16(attrValue, CultureInfo.InvariantCulture);
			}

			XPathNodeIterator personalEmailIter = xpathIterator.Current.Select("personal/email");
			if (personalEmailIter.MoveNext()) result.personal_email = Utils.DecodeHtml(personalEmailIter.Current.Value);

			XPathNodeIterator personalStreetIter = xpathIterator.Current.Select("personal/street");
			if (personalStreetIter.MoveNext()) result.personal_street = Utils.DecodeHtml(personalStreetIter.Current.Value);

			XPathNodeIterator personalCityIter = xpathIterator.Current.Select("personal/city");
			if (personalCityIter.MoveNext()) result.personal_city = Utils.DecodeHtml(personalCityIter.Current.Value);

			XPathNodeIterator personalStateIter = xpathIterator.Current.Select("personal/state");
			if (personalStateIter.MoveNext()) result.personal_state = Utils.DecodeHtml(personalStateIter.Current.Value);

			XPathNodeIterator personalZipIter = xpathIterator.Current.Select("personal/zip");
			if (personalZipIter.MoveNext()) result.personal_zip = Utils.DecodeHtml(personalZipIter.Current.Value);

			XPathNodeIterator personalCountryIter = xpathIterator.Current.Select("personal/country");
			if (personalCountryIter.MoveNext()) result.personal_country = Utils.DecodeHtml(personalCountryIter.Current.Value);

			XPathNodeIterator personalFaxIter = xpathIterator.Current.Select("personal/fax");
			if (personalFaxIter.MoveNext()) result.personal_fax = Utils.DecodeHtml(personalFaxIter.Current.Value);

			XPathNodeIterator personalPhoneIter = xpathIterator.Current.Select("personal/phone");
			if (personalPhoneIter.MoveNext()) result.personal_phone = Utils.DecodeHtml(personalPhoneIter.Current.Value);

			XPathNodeIterator personalMobileIter = xpathIterator.Current.Select("personal/mobile");
			if (personalMobileIter.MoveNext())
			{
				result.personal_mobile = Utils.DecodeHtml(personalMobileIter.Current.Value);
			}

			XPathNodeIterator personalWebIter = xpathIterator.Current.Select("personal/web");
			if (personalWebIter.MoveNext()) result.personal_web = Utils.DecodeHtml(personalWebIter.Current.Value);

			XPathNodeIterator businessEmailIter = xpathIterator.Current.Select("business/email");
			if (businessEmailIter.MoveNext()) result.business_email = Utils.DecodeHtml(businessEmailIter.Current.Value);

			XPathNodeIterator businessCompanyIter = xpathIterator.Current.Select("business/company");
			if (businessCompanyIter.MoveNext()) result.business_company = Utils.DecodeHtml(businessCompanyIter.Current.Value);

			XPathNodeIterator businessJobTitleIter = xpathIterator.Current.Select("business/job_title");
			if (businessJobTitleIter.MoveNext()) result.business_job_title = Utils.DecodeHtml(businessJobTitleIter.Current.Value);

			XPathNodeIterator businessDepartmentIter = xpathIterator.Current.Select("business/department");
			if (businessDepartmentIter.MoveNext()) result.business_department = Utils.DecodeHtml(businessDepartmentIter.Current.Value);

			XPathNodeIterator businessOfficeIter = xpathIterator.Current.Select("business/office");
			if (businessOfficeIter.MoveNext()) result.business_office = Utils.DecodeHtml(businessOfficeIter.Current.Value);

			XPathNodeIterator businessStreetIter = xpathIterator.Current.Select("business/street");
			if (businessStreetIter.MoveNext()) result.business_street = Utils.DecodeHtml(businessStreetIter.Current.Value);

			XPathNodeIterator businessCityIter = xpathIterator.Current.Select("business/city");
			if (businessCityIter.MoveNext()) result.business_city = Utils.DecodeHtml(businessCityIter.Current.Value);

			XPathNodeIterator businessStateIter = xpathIterator.Current.Select("business/state");
			if (businessStateIter.MoveNext()) result.business_state = Utils.DecodeHtml(businessStateIter.Current.Value);

			XPathNodeIterator businessZipIter = xpathIterator.Current.Select("business/zip");
			if (businessZipIter.MoveNext()) result.business_zip = Utils.DecodeHtml(businessZipIter.Current.Value);

			XPathNodeIterator businessCountryIter = xpathIterator.Current.Select("business/country");
			if (businessCountryIter.MoveNext()) result.business_country = Utils.DecodeHtml(businessCountryIter.Current.Value);

			XPathNodeIterator businessFaxIter = xpathIterator.Current.Select("business/fax");
			if (businessFaxIter.MoveNext()) result.business_fax = Utils.DecodeHtml(businessFaxIter.Current.Value);

			XPathNodeIterator businessPhoneIter = xpathIterator.Current.Select("business/phone");
			if (businessPhoneIter.MoveNext()) result.business_phone = Utils.DecodeHtml(businessPhoneIter.Current.Value);

			XPathNodeIterator businessWebIter = xpathIterator.Current.Select("business/web");
			if (businessWebIter.MoveNext()) result.business_web = Utils.DecodeHtml(businessWebIter.Current.Value);

			XPathNodeIterator otherEmailIter = xpathIterator.Current.Select("other/email");
			if (otherEmailIter.MoveNext()) result.other_email = Utils.DecodeHtml(otherEmailIter.Current.Value);

			XPathNodeIterator otherNotesIter = xpathIterator.Current.Select("other/notes");
			if (otherNotesIter.MoveNext()) result.other_notes = Utils.DecodeHtml(otherNotesIter.Current.Value);

			ArrayList groups = new ArrayList();
			XPathNodeIterator groupsIter = xpathIterator.Current.Select("groups/group");
			while (groupsIter.MoveNext())
			{
                XmlPacketGroup group = XmlPacketGroup.CreateWithXPath(groupsIter.Clone());
				groups.Add(group);
			}
			result.groups = (XmlPacketGroup[])groups.ToArray(typeof(XmlPacketGroup));

			return result;
		}
	}

	public class XmlPacketSignature
	{
		public int type = 0;
		public int opt = 0;
		public string str = string.Empty;

		public static XmlPacketSignature CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketSignature result = null;
			XPathNodeIterator xpathSignatureIter = xpathIterator.Current.Select(@"signature");
			if (xpathSignatureIter.MoveNext())
			{
				result = new XmlPacketSignature();

				string attrValue = xpathSignatureIter.Current.GetAttribute("type", "");
				if (attrValue.Length > 0) result.type = Convert.ToInt32(attrValue);

				attrValue = xpathSignatureIter.Current.GetAttribute("opt", "");
				if (attrValue.Length > 0) result.opt = Convert.ToInt32(attrValue);

				result.str = Utils.DecodeHtml(xpathSignatureIter.Current.Value);
			}
			return result;
		}
	}

	public class XmlPacketAccount
	{
		public int id_acct;
		public bool def_acct;
		public IncomingMailProtocol mail_protocol;
		public int mail_inc_port;
		public int mail_out_port;
		public bool mail_out_auth;
		public bool use_friendly_name;
		public short mails_on_server_days;
		public MailMode mail_mode;
		public bool getmail_at_login;
		public int inbox_sync_type;
		public string friendly_nm;
		public string email;
		public string mail_inc_host;
		public string mail_inc_login;
		public string mail_inc_pass;
		public string mail_out_host;
		public string mail_out_login;
		public string mail_out_pass;

		public XmlPacketAccount()
		{
			id_acct = -1;
			def_acct = false;
			mail_protocol = IncomingMailProtocol.Pop3;
			mail_inc_port = 110;
			mail_out_port = 25;
			mail_out_auth = false;
			use_friendly_name = false;
			mails_on_server_days = 0;
			mail_mode = MailMode.LeaveMessagesOnServer;
			getmail_at_login = false;
			inbox_sync_type = 0;
			friendly_nm = string.Empty;
			email = string.Empty;
			mail_inc_host = string.Empty;
			mail_inc_login = string.Empty;
			mail_inc_pass = string.Empty;
			mail_out_host = string.Empty;
			mail_out_login = string.Empty;
			mail_out_pass = string.Empty;
		}

		public static XmlPacketAccount CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketAccount result = null;
			XPathNodeIterator xpathAccoutIter = xpathIterator.Current.Select(@"account");
			if (xpathAccoutIter.MoveNext())
			{
				result = new XmlPacketAccount();

				string attrValue = xpathAccoutIter.Current.GetAttribute("id", "");
				if (attrValue.Length > 0) result.id_acct = Convert.ToInt32(attrValue);

				result.def_acct = (string.Compare(xpathAccoutIter.Current.GetAttribute("def_acct", ""), "0", true, CultureInfo.InvariantCulture) == 0) ? false : true;
				result.mail_protocol = (IncomingMailProtocol) Convert.ToInt32(xpathAccoutIter.Current.GetAttribute("mail_protocol", ""));

				attrValue = xpathAccoutIter.Current.GetAttribute("mail_inc_port", "");
				if (attrValue.Length > 0) result.mail_inc_port = Convert.ToInt32(attrValue);

				attrValue = xpathAccoutIter.Current.GetAttribute("mail_out_port", "");
				if (attrValue.Length > 0) result.mail_out_port = Convert.ToInt32(attrValue);

				result.mail_out_auth = (string.Compare(xpathAccoutIter.Current.GetAttribute("mail_out_auth", ""), "0", true, CultureInfo.InvariantCulture) == 0) ? false : true;

				result.use_friendly_name = (string.Compare(xpathAccoutIter.Current.GetAttribute("use_friendly_nm", ""), "0", true, CultureInfo.InvariantCulture) == 0) ? false : true;

				attrValue = xpathAccoutIter.Current.GetAttribute("mails_on_server_days", "");
				if (attrValue.Length > 0) result.mails_on_server_days = Convert.ToInt16(attrValue);

				attrValue = xpathAccoutIter.Current.GetAttribute("mail_mode", "");
				switch (attrValue)
				{
					case "0":
						result.mail_mode = MailMode.DeleteMessagesFromServer;
						break;
					case "1":
						result.mail_mode = MailMode.LeaveMessagesOnServer;
						break;
					case "2":
						result.mail_mode = MailMode.KeepMessagesOnServer;
						break;
					case "3":
						result.mail_mode = MailMode.DeleteMessageWhenItsRemovedFromTrash;
						break;
					case "4":
                        result.mail_mode = MailMode.KeepMessagesOnServerAndDeleteMessageWhenItsRemovedFromTrash;
						break;
				}

				result.getmail_at_login = (string.Compare(xpathAccoutIter.Current.GetAttribute("getmail_at_login", ""), "0", true, CultureInfo.InvariantCulture) == 0) ? false : true;

				attrValue = xpathAccoutIter.Current.GetAttribute("inbox_sync_type", "");
				if (attrValue.Length > 0) result.inbox_sync_type = Convert.ToInt32(attrValue);

				XPathNodeIterator friendlyIter = xpathAccoutIter.Current.Select("friendly_nm");
				if (friendlyIter.MoveNext())
				{
					result.friendly_nm = Utils.DecodeHtml(friendlyIter.Current.Value);
				}
				else
				{
					result.friendly_nm = string.Empty;
				}

				XPathNodeIterator emailIter = xpathAccoutIter.Current.Select("email");
				if (emailIter.MoveNext())
				{
					result.email = Utils.DecodeHtml(emailIter.Current.Value);
				}
				else
				{
					result.email = string.Empty;
				}

				XPathNodeIterator mailIncHostIter = xpathAccoutIter.Current.Select("mail_inc_host");
				if (mailIncHostIter.MoveNext())
				{
					result.mail_inc_host = Utils.DecodeHtml(mailIncHostIter.Current.Value);
				}
				else
				{
					result.mail_inc_host = string.Empty;
				}

				XPathNodeIterator mailIncLoginIter = xpathAccoutIter.Current.Select("mail_inc_login");
				if (mailIncLoginIter.MoveNext())
				{
					result.mail_inc_login = Utils.DecodeHtml(mailIncLoginIter.Current.Value);
				}
				else
				{
					result.mail_inc_login = string.Empty;
				}

				XPathNodeIterator mailIncPassIter = xpathAccoutIter.Current.Select("mail_inc_pass");
				if (mailIncPassIter.MoveNext())
				{
					result.mail_inc_pass = Utils.DecodeHtml(mailIncPassIter.Current.Value);
				}
				else
				{
					result.mail_inc_pass = string.Empty;
				}

				XPathNodeIterator mailOutHostIter = xpathAccoutIter.Current.Select("mail_out_host");
				if (mailOutHostIter.MoveNext())
				{
					result.mail_out_host = Utils.DecodeHtml(mailOutHostIter.Current.Value);
				}
				else
				{
					result.mail_out_host = string.Empty;
				}

				XPathNodeIterator mailOutLoginIter = xpathAccoutIter.Current.Select("mail_out_login");
				if (mailOutLoginIter.MoveNext())
				{
					result.mail_out_login = Utils.DecodeHtml(mailOutLoginIter.Current.Value);
				}
				else
				{
					result.mail_out_login = string.Empty;
				}

				XPathNodeIterator mailOutPassIter = xpathAccoutIter.Current.Select("mail_out_pass");
				if (mailOutPassIter.MoveNext())
				{
					result.mail_out_pass = Utils.DecodeHtml(mailOutPassIter.Current.Value);
				}
				else
				{
					result.mail_out_pass = string.Empty;
				}
			}
			return result;
		}
	}

	public class XmlPacketFilter
	{
		public int id_filter;
		public int id_acct;
		public byte field;
		public byte condition;
		public string filter;
		public byte action;
		public long id_folder;

		public XmlPacketFilter()
		{
			id_filter = -1;
			id_acct = -1;
			field = 0;
			condition = 0;
			filter = string.Empty;
			action = 0;
			id_folder = -1;
		}

		public static XmlPacketFilter CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketFilter result = null;
			XPathNodeIterator xpathFilterIter = xpathIterator.Current.Select(@"filter");
			if (xpathFilterIter.MoveNext())
			{
				result = new XmlPacketFilter();

				string attrValue = xpathFilterIter.Current.GetAttribute("id", "");
				if (attrValue.Length > 0) result.id_filter = Convert.ToInt32(attrValue);

				attrValue = xpathFilterIter.Current.GetAttribute("id_acct", "");
				if (attrValue.Length > 0) result.id_acct = Convert.ToInt32(attrValue);

				attrValue = xpathFilterIter.Current.GetAttribute("field", "");
				if (attrValue.Length > 0) result.field = Convert.ToByte(attrValue);

				attrValue = xpathFilterIter.Current.GetAttribute("condition", "");
				if (attrValue.Length > 0) result.condition = Convert.ToByte(attrValue);

				attrValue = xpathFilterIter.Current.GetAttribute("action", "");
				if (attrValue.Length > 0) result.action = Convert.ToByte(attrValue);

				attrValue = xpathFilterIter.Current.GetAttribute("id_folder", "");
				if (attrValue.Length > 0) result.id_folder = Convert.ToInt32(attrValue);

				result.filter = Utils.DecodeHtml(xpathFilterIter.Current.Value);
			}
			return result;
		}
	}

	public class XmlPacketMessages
	{
		public XmlPacketLookFor look_for;
		public XmlPacketFolder to_folder;
		public XmlPacketFolder folder;
		public XmlPacketMessage[] messages;

		public XmlPacketMessages()
		{
			look_for = null;
			to_folder = null;
			messages = new XmlPacketMessage[0];
		}

		public static XmlPacketMessages CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketMessages result = null;
			XPathNodeIterator xpathMessagesIter = xpathIterator.Current.Select(@"messages");
			if (xpathMessagesIter.MoveNext())
			{
				result = new XmlPacketMessages();
				result.look_for = XmlPacketLookFor.CreateWithXPath(xpathMessagesIter.Clone());

				XPathNodeIterator xpathFolderIter = xpathMessagesIter.Current.Select("folder");
				if (xpathFolderIter.MoveNext())
				{
					result.folder = XmlPacketFolder.CreateWithXPath(xpathFolderIter);
				}

				XmlPacketFolder to_folder = null;
				xpathFolderIter = xpathMessagesIter.Current.Select("to_folder");
				if (xpathFolderIter.MoveNext())
				{
					to_folder = XmlPacketFolder.CreateWithXPath(xpathFolderIter);
				}
				result.to_folder = to_folder;
				ArrayList messages = new ArrayList();
				XPathNodeIterator xpathMessageIter = xpathMessagesIter.Current.Select("message");
				while (xpathMessageIter.MoveNext())
				{
					XmlPacketMessage xmlMsg = XmlPacketMessage.CreateWithXPath(xpathMessageIter);
					if (xmlMsg != null)
					{
						messages.Add(xmlMsg);
					}
				}
				result.messages = (XmlPacketMessage[])messages.ToArray(typeof (XmlPacketMessage));
			}
			return result;
		}
	}

	public class XmlPacketMessage
	{
		public int id = 0;
		public int priority = 3;
		public string from = string.Empty;
		public string to = string.Empty;
		public string cc = string.Empty;
		public string bcc = string.Empty;
		public string subject = string.Empty;
        public string loanapplicants = string.Empty;
        public string loanappids = string.Empty;
        public XmlPacketGroup[] groups;
		public bool bodyIsHtml = false;
		public string bodyText = string.Empty;
		public XmlPacketAttachment[] attachments;

		public string uid = string.Empty;
		public XmlPacketFolder folder;

		public static XmlPacketMessage CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketMessage result = new XmlPacketMessage();
			
			string attrValue = xpathIterator.Current.GetAttribute("id", "");
			if (attrValue.Length > 0) result.id = Convert.ToInt32(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("priority", "");
			if (attrValue.Length > 0) result.priority = Convert.ToInt32(attrValue);

			XPathNodeIterator uidIter = xpathIterator.Current.Select("uid");
			if (uidIter.MoveNext())
			{
				result.uid = Utils.DecodeHtml(uidIter.Current.Value);
			}
			else
			{
				result.uid = string.Empty;
			}
			XPathNodeIterator xpathFolderIter = xpathIterator.Current.Select("folder");
			if (xpathFolderIter.MoveNext())
			{
				result.folder = XmlPacketFolder.CreateWithXPath(xpathFolderIter);
			}

			XPathNodeIterator fromIter = xpathIterator.Current.Select("headers/from");
			if (fromIter.MoveNext()) result.from = Utils.DecodeHtml(fromIter.Current.Value);

			XPathNodeIterator toIter = xpathIterator.Current.Select("headers/to");
			if (toIter.MoveNext()) result.to = Utils.DecodeHtml(toIter.Current.Value);

			XPathNodeIterator ccIter = xpathIterator.Current.Select("headers/cc");
			if (ccIter.MoveNext()) result.cc = Utils.DecodeHtml(ccIter.Current.Value);

			XPathNodeIterator bccIter = xpathIterator.Current.Select("headers/bcc");
			if (bccIter.MoveNext()) result.bcc= Utils.DecodeHtml(bccIter.Current.Value);

			XPathNodeIterator subjectIter = xpathIterator.Current.Select("headers/subject");
			if (subjectIter.MoveNext()) result.subject = Utils.DecodeHtml(subjectIter.Current.Value);

            XPathNodeIterator loanapplicantsIter = xpathIterator.Current.Select("headers/loanapplicants");
            if (loanapplicantsIter.MoveNext()) result.loanapplicants = Utils.DecodeHtml(loanapplicantsIter.Current.Value);

            XPathNodeIterator loanappidsIter = xpathIterator.Current.Select("headers/loanappids");
            if (loanappidsIter.MoveNext()) result.loanappids = Utils.DecodeHtml(loanappidsIter.Current.Value);

			ArrayList groups = new ArrayList();
			XPathNodeIterator groupsIter = xpathIterator.Current.Select("headers/groups/group");
			while (groupsIter.MoveNext())
			{
				XmlPacketGroup group = XmlPacketGroup.CreateWithXPath(groupsIter);
				if (group != null)
				{
					groups.Add(group);
				}
			}
			if (groups.Count > 0) result.groups = (XmlPacketGroup[])groups.ToArray(typeof (XmlPacketGroup));

			XPathNodeIterator bodyIter = xpathIterator.Current.Select("body");
			if (bodyIter.MoveNext())
			{
				attrValue = bodyIter.Current.GetAttribute("is_html", "");
				if (attrValue.Length > 0) result.bodyIsHtml = (string.Compare(attrValue, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;

				result.bodyText = bodyIter.Current.Value;
			}

			ArrayList attachments = new ArrayList();
			XPathNodeIterator attachmentIter = xpathIterator.Current.Select("attachments/attachment");
			while (attachmentIter.MoveNext())
			{
				XmlPacketAttachment attach = XmlPacketAttachment.CreateWithXPath(attachmentIter);
				if (attach != null)
				{
					attachments.Add(attach);
				}
			}
			result.attachments = (XmlPacketAttachment[])attachments.ToArray(typeof(XmlPacketAttachment));

			return result;
		}
	}

	public class XmlPacketSkin
	{
		public bool def;
		public string skin;

		public static XmlPacketSkin CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketSkin result = null;
			XPathNodeIterator xpathSkinIter = xpathIterator.Current.Select("skin");
			if (xpathSkinIter.MoveNext())
			{
				result = new XmlPacketSkin();
				result.def = (string.Compare(xpathSkinIter.Current.GetAttribute("def", ""), "0", true, CultureInfo.InvariantCulture) == 0) ? false : true;
				result.skin = Utils.DecodeHtml(xpathSkinIter.Current.Value);
			}
			return result;
		}
	}

	public class XmlPacketLang
	{
		public bool def;
		public string lang;

		public static XmlPacketLang CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketLang result = null;
			XPathNodeIterator xpathLangIter = xpathIterator.Current.Select("lang");
			if (xpathLangIter.MoveNext())
			{
				result = new XmlPacketLang();
				result.def = (string.Compare(xpathLangIter.Current.GetAttribute("def", ""), "0", true, CultureInfo.InvariantCulture) == 0) ? false : true;
				result.lang = Utils.DecodeHtml(xpathLangIter.Current.Value);
			}
			return result;
		}
	}

	public class XmlPacketSettings
	{
		public short msgs_per_page;
		public bool allow_dhtml_editor;
		public string def_skin;
		public int def_charset_inc;
		public int def_charset_out;
		public short def_timezone;
		public string def_lang;
		public string def_date_fmt;
        public TimeFormats def_time_fmt;
		public byte view_mode;

		public static XmlPacketSettings CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketSettings result = null;
			XPathNodeIterator xpathSettingsIter = xpathIterator.Current.Select(@"settings");
			if (xpathSettingsIter.MoveNext())
			{
				result = new XmlPacketSettings();
				
				string attrValue = xpathSettingsIter.Current.GetAttribute("msgs_per_page", "");
				if (attrValue.Length > 0) result.msgs_per_page = Convert.ToInt16(attrValue);
				
				result.allow_dhtml_editor = (string.Compare(xpathSettingsIter.Current.GetAttribute("allow_dhtml_editor", ""), "1", false, CultureInfo.InvariantCulture) == 0) ? true : false;
				
				attrValue = xpathSettingsIter.Current.GetAttribute("def_charset_inc", "");
				if (attrValue.Length > 0) result.def_charset_inc = Convert.ToInt32(attrValue);
				
				attrValue = xpathSettingsIter.Current.GetAttribute("def_charset_out", "");
				if (attrValue.Length > 0) result.def_charset_out = Convert.ToInt32(attrValue);
				
				attrValue = xpathSettingsIter.Current.GetAttribute("def_timezone", "");
				if (attrValue.Length > 0) result.def_timezone = Convert.ToInt16(attrValue);
				
				attrValue = xpathSettingsIter.Current.GetAttribute("view_mode", "");
				if (attrValue.Length > 0) result.view_mode = Convert.ToByte(attrValue);

                attrValue = xpathSettingsIter.Current.GetAttribute("time_format", "");
                result.def_time_fmt = (attrValue.Length > 0 && attrValue == "1") ? TimeFormats.F12 : TimeFormats.F24;

				XPathNodeIterator defSkinIter = xpathSettingsIter.Current.Select("def_skin");
				if (defSkinIter.MoveNext())
				{
					result.def_skin = Utils.DecodeHtml(defSkinIter.Current.Value);
				}
				else
				{
					result.def_skin = string.Empty;
				}
				XPathNodeIterator defLangIter = xpathSettingsIter.Current.Select("def_lang");
				if (defLangIter.MoveNext())
				{
					result.def_lang = Utils.DecodeHtml(defLangIter.Current.Value);
				}
				else
				{
					result.def_lang = string.Empty;
				}
				XPathNodeIterator defDateFmtIter = xpathSettingsIter.Current.Select("def_date_fmt");
				if (defDateFmtIter.MoveNext())
				{
					result.def_date_fmt = Utils.DecodeHtml(defDateFmtIter.Current.Value);
				}
				else
				{
					result.def_date_fmt = string.Empty;
				}
			}
			return result;
		}
	}

	public class XmlPacketFolders
	{
		public XmlPacketFolder[] folderArray;
	
		public static XmlPacketFolders CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketFolders result = new XmlPacketFolders();
			XPathNodeIterator xpathFoldersIter = xpathIterator.Current.Select("folders");
			if (xpathFoldersIter.MoveNext())
			{
				ArrayList folders = new ArrayList();
				XPathNodeIterator xpathFolderIter = xpathFoldersIter.Current.Select("folder");
				while (xpathFolderIter.MoveNext())
				{
					XmlPacketFolder xmlFld = XmlPacketFolder.CreateWithXPath(xpathFolderIter);
					if (xmlFld != null)
					{
						folders.Add(xmlFld);
					}
				}
				result.folderArray = (XmlPacketFolder[])folders.ToArray(typeof (XmlPacketFolder));
			}
			return result;
		}
	}

	public class XmlPacketFolder
	{
		public long id;
		public int sync_type;
		public int hide;
		public short fld_order;
		public string name;
		public string full_name;

		public static XmlPacketFolder CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketFolder result = new XmlPacketFolder();
			string attrValue = xpathIterator.Current.GetAttribute("id", "");
			if (attrValue.Length > 0) result.id = Convert.ToInt64(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("sync_type", "");
			if (attrValue.Length > 0) result.sync_type = Convert.ToInt32(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("hide", "");
			if (attrValue.Length > 0) result.hide = Convert.ToInt32(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("fld_order", "");
			if (attrValue.Length > 0) result.fld_order = Convert.ToInt16(attrValue);

			XPathNodeIterator nameIter = xpathIterator.Current.Select("name");
			if (nameIter.MoveNext())
			{
				result.name = Utils.DecodeHtml(nameIter.Current.Value);
			}

			XPathNodeIterator fullNameIter = xpathIterator.Current.Select("full_name");
			if (fullNameIter.MoveNext())
			{
				result.full_name = Utils.DecodeHtml(fullNameIter.Current.Value);
			}
			else
			{
				result.full_name = Utils.DecodeHtml(xpathIterator.Current.Value);
			}
			return result;
		}
	}

	public class XmlPacketLookFor
	{
		public int fields;
		public string search_query;
		public int type;

		public static XmlPacketLookFor CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketLookFor result = null;
			XPathNodeIterator xpathLookForIter = xpathIterator.Current.Select("look_for");
			if (xpathLookForIter.MoveNext())
			{
				result = new XmlPacketLookFor();
				string attrValue = xpathLookForIter.Current.GetAttribute("fields", "");
				if (attrValue.Length > 0) result.fields = Convert.ToByte(attrValue);
				result.search_query = Utils.DecodeHtml(xpathLookForIter.Current.Value);

				attrValue = xpathLookForIter.Current.GetAttribute("type", "");
				if (attrValue.Length > 0) result.type = Convert.ToInt32(attrValue);
				result.search_query = Utils.DecodeHtml(xpathLookForIter.Current.Value);
			}
			return result;
		}
	}

	public class XmlPacket
	{
		public string action;
		public string request;
		public Hashtable parameters;
		public XmlPacketMessages messages;
		public XmlPacketSettings settings;
		public XmlPacketFolder folder;
		public XmlPacketLookFor look_for;
		public XmlPacketAccount account;
		public XmlPacketFilter filter;
		public XmlPacketSignature signature;
		public XmlPacketFolders folders;
		public XmlPacketGroup group;
		public XmlPacketContact contact;
		public XmlPacketContact[] contacts;
		public XmlPacketGroup[] groups;
		public XmlPacketMessage message;
		public UserColumn[] columns;

		public XmlPacket()
		{
			action = string.Empty;
			request = string.Empty;
            parameters = new Hashtable(StringComparer.OrdinalIgnoreCase);
            messages = null;
			settings = null;
			folder = null;
			look_for = null;
			account = null;
			filter = null;
			signature = null;
			folders = null;
			group = null;
			contact = null;
			contacts = null;
			message = null;
		}
	}


	/// <summary>
	/// Summary description for XmlPacketManager.
	/// </summary>
	public class XmlPacketManager
	{
		private string _informationForUser;
		private string _errorFromSession;
		private BaseWebMailActions _actions;

		public string ErrorFromSession
		{
			get { return _errorFromSession; }
			set { _errorFromSession = value; }
		}

		public Account CurrentAccount
		{
			get
			{
				if (_actions != null)
				{
					return _actions.CurrentAccount;
				}
				return null;
			}
		}

		public Page CurrentPage
		{
			get
			{
				if (_actions != null)
				{
					return _actions.CurrentPage;
				}
				return null;
			}
		}

		public XmlPacketManager(Account acct, Page webPage)
		{
			_actions = new BaseWebMailActions(acct, webPage);
		}

		public XmlPacket ParseClientXmlText(string xmlText)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xmlText);
			return ParseClientXmlDocument(doc);
		}

		public XmlPacket ParseClientXmlDocument(XmlDocument xmlDocument)
		{
			XmlPacket result = new XmlPacket();
			XmlNodeReader nodeReader = new XmlNodeReader(xmlDocument);

			XPathDocument xpathDoc = new XPathDocument(nodeReader);
			XPathNavigator xpathNav = xpathDoc.CreateNavigator();

			XPathNodeIterator xpathParamNodeIter = xpathNav.Select(@"webmail/param");
			while (xpathParamNodeIter.MoveNext())
			{
				string name = xpathParamNodeIter.Current.GetAttribute("name", "");
				string val = xpathParamNodeIter.Current.GetAttribute("value", "");
				if ((val == null) || (val.Length == 0))
				{
					val = Utils.DecodeHtml(xpathParamNodeIter.Current.Value);
				}

				switch (name.ToLower(CultureInfo.InvariantCulture))
				{
					case "action":
						result.action = val;
						break;
					case "request":
						result.request = val;
						break;
					default:
						result.parameters.Add(name, val);
						break;
				}
			}

			XPathNodeIterator webmailIter = xpathNav.Select("webmail");
			if (webmailIter.MoveNext())
			{
				XPathNodeIterator xpathFolderIter = webmailIter.Current.Select("folder");
				if (xpathFolderIter.MoveNext())
				{
					result.folder = XmlPacketFolder.CreateWithXPath(xpathFolderIter);
				}

				result.look_for = XmlPacketLookFor.CreateWithXPath(webmailIter.Clone());

				result.messages = XmlPacketMessages.CreateWithXPath(webmailIter.Clone());

				result.settings = XmlPacketSettings.CreateWithXPath(webmailIter.Clone());

				result.account = XmlPacketAccount.CreateWithXPath(webmailIter.Clone());

				result.filter = XmlPacketFilter.CreateWithXPath(webmailIter.Clone());

				result.signature = XmlPacketSignature.CreateWithXPath(webmailIter.Clone());

				result.folders = XmlPacketFolders.CreateWithXPath(webmailIter.Clone());

				XPathNodeIterator xpathContactIter = webmailIter.Current.Select("contact");
				if (xpathContactIter.MoveNext())
				{
					result.contact = XmlPacketContact.CreateWithXPath(xpathContactIter);
				}

				ArrayList contacts = new ArrayList();
				xpathContactIter = webmailIter.Current.Select("contacts/contact");
				while (xpathContactIter.MoveNext())
				{
					XmlPacketContact contact = XmlPacketContact.CreateWithXPath(xpathContactIter);
					if (contact != null)
					{
						contacts.Add(contact);
					}
				}
				result.contacts = (XmlPacketContact[])contacts.ToArray(typeof(XmlPacketContact));

				XPathNodeIterator xpathGroupIter = webmailIter.Current.Select("group");
				if (xpathGroupIter.MoveNext())
				{
					result.group = XmlPacketGroup.CreateWithXPath(xpathGroupIter);
				}

				ArrayList groups = new ArrayList();
				xpathGroupIter = webmailIter.Current.Select("groups/group");
				while (xpathGroupIter.MoveNext())
				{
					XmlPacketGroup group = XmlPacketGroup.CreateWithXPath(xpathGroupIter);
					if (group != null)
					{
						groups.Add(group);
					}
				}
				result.groups = (XmlPacketGroup[])groups.ToArray(typeof(XmlPacketGroup));

				XPathNodeIterator messageIter = webmailIter.Current.Select("message");
				if (messageIter.MoveNext())
				{
					result.message = XmlPacketMessage.CreateWithXPath(messageIter);
				}

				ArrayList columns = new ArrayList();
				XPathNodeIterator columnsIter = webmailIter.Current.Select("columns/column");
				while (columnsIter.MoveNext())
				{
					UserColumn column = new UserColumn();
					string str = columnsIter.Current.GetAttribute("id", "");
					column.IDColumn = Convert.ToInt32(str);
					column.Value = Convert.ToInt32(columnsIter.Current.GetAttribute("value", ""));
					columns.Add(column);
				}
				result.columns = (UserColumn[]) columns.ToArray(typeof(UserColumn));
			}

			return result;
		}

		public virtual XmlDocument CreateServerXmlDocumentResponse(XmlPacket clientPacket)
		{
			XmlDocument result = new XmlDocument();
			result.PreserveWhitespace = true;
			XmlDeclaration xmlDecl = result.CreateXmlDeclaration("1.0", "utf-8", "");
			result.AppendChild(xmlDecl);
				
			XmlElement webmailNode = result.CreateElement("webmail");
			result.AppendChild(webmailNode);
			WebmailResourceManager _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();				
			try
			{
				if (string.Compare(clientPacket.action, "login", true, CultureInfo.InvariantCulture) == 0)
				{
					WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
					string email = Convert.ToString(clientPacket.parameters["email"]);
					string login = Convert.ToString(clientPacket.parameters["mail_inc_login"]);
					string password = Convert.ToString(clientPacket.parameters["mail_inc_pass"]);
                    bool sign_me = (clientPacket.parameters["sign_me"].ToString() == "1") ? true : false;
                    bool advanced_login = (clientPacket.parameters["advanced_login"].ToString() == "1") ? true : false;
                    if (advanced_login || (int)settings.HideLoginMode < 20)
                    {
                        if (Validation.CheckIt(Validation.ValidationTask.Email, email))
                            email = Validation.Corrected;
                        else
                            throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
                    }
                    if (advanced_login || (int)settings.HideLoginMode != 10 && (int)settings.HideLoginMode != 11)
                    {
                        if (Validation.CheckIt(Validation.ValidationTask.Login, login))
                            login = Validation.Corrected;
                        else
                            throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
                    }
                    if (Validation.CheckIt(Validation.ValidationTask.Password, password))
                        password = Validation.Corrected;
                    else
                        throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
                    string inc_server = string.Empty;
                    int inc_protocol = 0;
                    int inc_port = 110;
                    string out_server = string.Empty;
                    int out_port = 25;
                    bool smtp_auth = false;
                    if (advanced_login)
                    {
                        inc_server = Convert.ToString(clientPacket.parameters["mail_inc_host"]);
                        inc_protocol = Convert.ToInt32(clientPacket.parameters["mail_protocol"]);
                        inc_port = Convert.ToInt32(clientPacket.parameters["mail_inc_port"]);
                        out_server = Convert.ToString(clientPacket.parameters["mail_out_host"]);
                        out_port = Convert.ToInt32(clientPacket.parameters["mail_out_port"]);
                        smtp_auth = (clientPacket.parameters["mail_out_auth"].ToString() == "1") ? true : false;
                        if (Validation.CheckIt(Validation.ValidationTask.INServer, inc_server))
                            inc_server = Validation.Corrected;
                        else
                            throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
                        if (Validation.CheckIt(Validation.ValidationTask.INPort, clientPacket.parameters["mail_inc_port"].ToString()))
                            inc_port = int.Parse(Validation.Corrected);
                        else
                            throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
                        if (Validation.CheckIt(Validation.ValidationTask.OUTServer, out_server))
                            out_server = Validation.Corrected;
                        else
                            throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
                        if (Validation.CheckIt(Validation.ValidationTask.OUTPort, clientPacket.parameters["mail_out_port"].ToString()))
                            out_port = int.Parse(Validation.Corrected);
                        else
                            throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
                    }
                    else
                    {
                        inc_server = settings.IncomingMailServer;
                        inc_protocol = (int) settings.IncomingMailProtocol;
                        inc_port = settings.IncomingMailPort;
                        out_server = settings.OutgoingMailServer;
                        out_port = settings.OutgoingMailPort;
                        smtp_auth = settings.ReqSmtpAuth;
                    }

					LoginAccount(webmailNode, email, login, password, inc_server, inc_protocol, inc_port, out_server, out_port, smtp_auth, sign_me, advanced_login);
				}

				if (CurrentAccount == null) throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));

				switch (clientPacket.action.ToLower(CultureInfo.InvariantCulture))
				{
					case "add":
						switch (clientPacket.request.ToLower(CultureInfo.InvariantCulture))
						{
							case "contacts":
								AddContacts(webmailNode, Convert.ToInt32(clientPacket.parameters["id_group"]), clientPacket.contacts);
								break;
						}
						break;
					case "get":
						switch (clientPacket.request.ToLower(CultureInfo.InvariantCulture))
						{
							case "account":
								GetAccount(webmailNode, Convert.ToInt32(clientPacket.parameters["id_acct"]));
								break;
							case "accounts":
								GetAccounts(webmailNode, ((CurrentAccount != null) && (CurrentAccount.UserOfAccount != null)) ? CurrentAccount.UserOfAccount.ID : -1 , -1);
								break;
							case "contact":
								GetContact(webmailNode, Convert.ToInt64(clientPacket.parameters["id_addr"]));
								break;
							case "contacts_groups":
								GetContactsGroups(webmailNode, Convert.ToInt32(clientPacket.parameters["page"]), Convert.ToInt16(clientPacket.parameters["sort_field"]), Convert.ToInt16(clientPacket.parameters["sort_order"]), Convert.ToInt32(clientPacket.parameters["id_group"]), clientPacket.look_for.search_query, clientPacket.look_for.type);
								break;
							case "contacts_settings":
								GetContactsSettings(webmailNode);
								break;
							case "filter":
								GetFilter(webmailNode, Convert.ToInt32(clientPacket.parameters["id_filter"]), Convert.ToInt32(clientPacket.parameters["id_acct"]));
								break;
							case "filters":
								GetFilters(webmailNode, Convert.ToInt32(clientPacket.parameters["id_acct"]));
								break;
							case "folders_list":
								GetFoldersList(webmailNode, Convert.ToInt32(clientPacket.parameters["id_acct"]), Convert.ToInt32(clientPacket.parameters["sync"]));
								break;
							case "group":
								GetGroup(webmailNode, Convert.ToInt32(clientPacket.parameters["id_group"]));
								break;
							case "groups":
								GetGroups(webmailNode);
								break;
							case "message":
								GetMessage(webmailNode, Convert.ToInt32(clientPacket.parameters["id"]), Convert.ToString(clientPacket.parameters["uid"]), (clientPacket.folder != null) ? clientPacket.folder.id : 0, (clientPacket.folder != null) ? clientPacket.folder.full_name : string.Empty, (MessageMode)Convert.ToInt32(clientPacket.parameters["mode"]), (clientPacket.parameters["charset"] != null) ? Convert.ToInt32(clientPacket.parameters["charset"]) : -1);
								break;
							case "messages":
								GetMessages(webmailNode, (clientPacket.folder != null) ? clientPacket.folder.id : 0, (clientPacket.folder != null) ? clientPacket.folder.full_name : string.Empty, Convert.ToInt32(clientPacket.parameters["page"]), Convert.ToInt32(clientPacket.parameters["sort_field"]), Convert.ToInt32(clientPacket.parameters["sort_order"]), (clientPacket.look_for != null) ? clientPacket.look_for.search_query : string.Empty, (clientPacket.look_for != null) ? clientPacket.look_for.fields : 0);
								break;
							case "settings":
								GetSettings(webmailNode);
								break;
							case "settings_list":
								GetSettingsList(webmailNode);
								break;
							case "signature":
								GetSignature(webmailNode, Convert.ToInt32(clientPacket.parameters["id_acct"]));
								break;
							case "x_spam":
								GetXSpam(webmailNode);
								break;
						}
						break;
					case "new":
						switch (clientPacket.request.ToLower(CultureInfo.InvariantCulture))
						{
							case "account":
								if (clientPacket.account != null)
								{
									int last_id = NewAccount(clientPacket.account.def_acct, clientPacket.account.mail_protocol, clientPacket.account.mail_inc_port, clientPacket.account.mail_out_port, clientPacket.account.mail_out_auth, clientPacket.account.use_friendly_name, clientPacket.account.mails_on_server_days, clientPacket.account.mail_mode, clientPacket.account.getmail_at_login, clientPacket.account.inbox_sync_type, clientPacket.account.friendly_nm, clientPacket.account.email, clientPacket.account.mail_inc_host, clientPacket.account.mail_inc_login, clientPacket.account.mail_inc_pass, clientPacket.account.mail_out_host, clientPacket.account.mail_out_login, clientPacket.account.mail_out_pass);
									GetAccounts(webmailNode, ((CurrentAccount != null) && (CurrentAccount.UserOfAccount != null)) ? CurrentAccount.UserOfAccount.ID : -1 , last_id);
								}
								break;
							case "contact":
								if (clientPacket.contact != null)
								{
									try
									{
										NewContact(InitAddressBookContactFromXml(clientPacket.contact));
									}
									finally
									{
										GetContactsGroups(webmailNode, Convert.ToInt32(clientPacket.parameters["page"]), Convert.ToInt16(clientPacket.parameters["sort_field"]), Convert.ToInt16(clientPacket.parameters["sort_order"]), -1, null, 0);
									}
								}
								break;
							case "filter":
								if (clientPacket.filter != null)
								{
									try
									{
										NewFilter(clientPacket.filter.id_acct, clientPacket.filter.field, clientPacket.filter.condition, clientPacket.filter.action, clientPacket.filter.id_folder, clientPacket.filter.filter);
									}
									finally
									{
										GetFilters(webmailNode, clientPacket.filter.id_acct);
									}
								}
								break;
							case "folder":
								try
								{
									NewFolder(Convert.ToInt32(clientPacket.parameters["id_acct"]), Convert.ToInt32(clientPacket.parameters["id_parent"]), Convert.ToString(clientPacket.parameters["full_name_parent"]), Convert.ToString(clientPacket.parameters["name"]), Convert.ToInt32(clientPacket.parameters["create"]));
								}
								finally
								{
									GetFoldersList(webmailNode, Convert.ToInt32(clientPacket.parameters["id_acct"]), -1);
								}
								break;
							case "group":
								if (clientPacket.group != null)
								{
									try
									{
										NewGroup(InitAddressBookGroupFromXml(clientPacket.group));
									}
									finally
									{
										GetContactsGroups(webmailNode, Convert.ToInt32(clientPacket.parameters["page"]), Convert.ToInt16(clientPacket.parameters["sort_field"]), Convert.ToInt16(clientPacket.parameters["sort_order"]), -1, null, 0);
									}
								}
								break;
						}
						break;
					case "save":
						switch (clientPacket.request.ToLower(CultureInfo.InvariantCulture))
						{
							case "message":
								if (clientPacket.message != null)
								{
                                    SaveMessage(webmailNode, clientPacket.message, false);
								}
								break;
						}
						break;
					case "send":
						switch (clientPacket.request.ToLower(CultureInfo.InvariantCulture))
						{
							case "message":
								if (clientPacket.message != null)
								{
									SendMessage(webmailNode, clientPacket.message);
								}
								break;
						}
						break;
                    case "backsave":
                        switch (clientPacket.request.ToLower(CultureInfo.InvariantCulture))
                        {
                            case "message":
                                if (clientPacket.message != null)
                                {
                                    SaveMessage(webmailNode, clientPacket.message, true);
                                }
                                break;
                        }
                        break;
					case "set":
						switch (clientPacket.request.ToLower(CultureInfo.InvariantCulture))
						{
							case "sender":
								SetSender(webmailNode, clientPacket.parameters["sender"].ToString(), Convert.ToByte(clientPacket.parameters["safety"]));
								break;
						}
						break;
					case "operation_messages":
						if (clientPacket.messages != null)
						{
							GroupOperations(webmailNode, clientPacket.request, (clientPacket.messages.to_folder != null) ? clientPacket.messages.to_folder.id : 0, (clientPacket.messages.to_folder != null) ? clientPacket.messages.to_folder.full_name : string.Empty, clientPacket.messages);
						}
						break;
					case "update":
						switch (clientPacket.request.ToLower(CultureInfo.InvariantCulture))
						{
							case "account":
								if (clientPacket.account != null)
								{
									UpdateAccount(webmailNode, clientPacket.account.id_acct, clientPacket.account.def_acct, clientPacket.account.mail_protocol, clientPacket.account.mail_inc_port, clientPacket.account.mail_out_port, clientPacket.account.mail_out_auth, clientPacket.account.use_friendly_name, clientPacket.account.mails_on_server_days, clientPacket.account.mail_mode, clientPacket.account.getmail_at_login, clientPacket.account.inbox_sync_type, clientPacket.account.friendly_nm, clientPacket.account.email, clientPacket.account.mail_inc_host, clientPacket.account.mail_inc_login, clientPacket.account.mail_inc_pass, clientPacket.account.mail_out_host, clientPacket.account.mail_out_login, clientPacket.account.mail_out_pass);
								}
								break;
							case "contact":
								if (clientPacket.contact != null)
								{
									try
									{
										UpdateContact(InitAddressBookContactFromXml(clientPacket.contact));
									}
									finally
									{
										GetContactsGroups(webmailNode, Convert.ToInt32(clientPacket.parameters["page"]), Convert.ToInt16(clientPacket.parameters["sort_field"]), Convert.ToInt16(clientPacket.parameters["sort_order"]), -1, null, 0);
									}
								}
								break;
							case "contacts_settings":
								UpdateContactsSettings(webmailNode, (string.Compare(clientPacket.parameters["white_listing"].ToString(), "0", true, CultureInfo.InvariantCulture) == 0) ? false : true, Convert.ToInt16(clientPacket.parameters["contacts_per_page"]));
								break;
							case "cookie_settings":
								UpdateCookieSettings(webmailNode, (string.Compare(clientPacket.parameters["hide_folders"].ToString(), "0", true, CultureInfo.InvariantCulture) == 0) ? false : true, Convert.ToInt16(clientPacket.parameters["horiz_resizer"]), Convert.ToInt16(clientPacket.parameters["vert_resizer"]), Convert.ToByte(clientPacket.parameters["mark"]), Convert.ToByte(clientPacket.parameters["reply"]), clientPacket.columns);
								break;
							case "filter":
								if (clientPacket.filter != null)
								{
									try
									{
										UpdateFilter(clientPacket.filter.id_acct, clientPacket.filter.id_filter, clientPacket.filter.field, clientPacket.filter.condition, clientPacket.filter.action, clientPacket.filter.id_folder, clientPacket.filter.filter);
									}
									finally
									{
										GetFilters(webmailNode, clientPacket.filter.id_acct);
									}
								}
								break;
							case "folders":
								if (clientPacket.folders != null)
								{
									try
									{
										UpdateFolders(Convert.ToInt32(clientPacket.parameters["id_acct"]), clientPacket.folders.folderArray);
									}
									finally
									{
										GetFoldersList(webmailNode, Convert.ToInt32(clientPacket.parameters["id_acct"]), -1);
									}
								}
								break;
							case "group":
								if (clientPacket.group != null)
								{
									try
									{
										UpdateGroup(InitAddressBookGroupFromXml(clientPacket.group));
									}
									finally
									{
										GetContactsGroups(webmailNode, Convert.ToInt32(clientPacket.parameters["page"]), Convert.ToInt16(clientPacket.parameters["sort_field"]), Convert.ToInt16(clientPacket.parameters["sort_order"]), -1, null, 0);
									}
								}
								break;
							case "settings":
								if (clientPacket.settings != null)
								{
									if (clientPacket.settings != null)
									{
										UpdateSettings(webmailNode, clientPacket.settings);
									}
								}
								break;
							case "signature":
								if (clientPacket.signature != null)
								{
									UpdateSignature(webmailNode, Convert.ToInt32(clientPacket.parameters["id_acct"]), clientPacket.signature.type, clientPacket.signature.opt, clientPacket.signature.str);
								}
								break;
							case "x_spam":
								UpdateXSpam(webmailNode, (string.Compare(clientPacket.parameters["x_spam"].ToString(), "0", true, CultureInfo.InvariantCulture) == 0) ? false : true);
								break;
						}
						break;
					case "delete":
						switch (clientPacket.request.ToLower(CultureInfo.InvariantCulture))
						{
							case "account":
								try
								{
									DeleteAccount(Convert.ToInt32(clientPacket.parameters["id_acct"]));
								}
								finally
								{
									GetAccounts(webmailNode, ((CurrentAccount != null) && (CurrentAccount.UserOfAccount != null)) ? CurrentAccount.UserOfAccount.ID : -1, -1);
								}
								break;
							case "contacts":
								try
								{
									DeleteContacts(clientPacket.contacts, clientPacket.groups);
								}
								finally
								{
									GetContactsGroups(webmailNode, Convert.ToInt32(clientPacket.parameters["page"]), Convert.ToInt16(clientPacket.parameters["sort_field"]), Convert.ToInt16(clientPacket.parameters["sort_order"]), -1, null, 0);
								}
								break;
							case "filter":
								try
								{
									DeleteFilter(Convert.ToInt32(clientPacket.parameters["id_filter"]), Convert.ToInt32(clientPacket.parameters["id_acct"]));
								}
								finally
								{
									GetFilters(webmailNode, Convert.ToInt32(clientPacket.parameters["id_acct"]));
								}
								break;
							case "folders":
								try
								{
									DeleteFolders(Convert.ToInt32(clientPacket.parameters["id_acct"]), clientPacket.folders.folderArray);
								}
								finally
								{
									GetFoldersList(webmailNode, Convert.ToInt32(clientPacket.parameters["id_acct"]), -1);
								}
								break;
						}
						break;
				}


				if ((_errorFromSession != null) && (_errorFromSession.Length > 0))
				{
					XmlElement errorElement = result.CreateElement("error");
					errorElement.AppendChild(result.CreateCDataSection(Utils.EncodeHtml(_errorFromSession)));
					webmailNode.AppendChild(errorElement);
					_errorFromSession = string.Empty;
				}

				if ((_informationForUser != null) && (_informationForUser.Length > 0))
				{
					XmlElement informationElement = result.CreateElement("information");
					informationElement.AppendChild(result.CreateCDataSection(Utils.EncodeHtml(_informationForUser)));
					webmailNode.AppendChild(informationElement);
					_informationForUser = string.Empty;
				}
			}
			catch (WebMailException ex)
			{
				Log.WriteException(ex);
				webmailNode.RemoveAll();
				if (ex is WebMailSessionException)
				{
					XmlElement errorElement = result.CreateElement("session_error");
					webmailNode.AppendChild(errorElement);
				}
				else
				{
					XmlElement errorElement = result.CreateElement("error");
					errorElement.AppendChild(result.CreateCDataSection(Utils.EncodeJsSaveString(Utils.EncodeHtml(ex.Message))));
					webmailNode.AppendChild(errorElement);
				}
			}
			catch (Exception ex)
			{
				Log.WriteException(ex);
				webmailNode.RemoveAll();
				XmlElement errorElement = result.CreateElement("error");
				errorElement.AppendChild(result.CreateCDataSection(Utils.EncodeJsSaveString(Utils.EncodeHtml(ex.Message))));
				webmailNode.AppendChild(errorElement);
			}
			return result;
		}

		private void SetSender(XmlElement webmailElement, string sender, byte safety)
		{
			DbStorage ds = DbStorageCreator.CreateDatabaseStorage(this._actions.CurrentAccount);
			try
			{
				ds.Connect();
				ds.SetSender(sender, safety);

				XmlElement updateElem = webmailElement.OwnerDocument.CreateElement("update");

                XmlAttribute valueAttr = webmailElement.OwnerDocument.CreateAttribute("value");
				valueAttr.Value = "set_sender";
				updateElem.Attributes.Append(valueAttr);

				webmailElement.AppendChild(updateElem);
			}
			finally
			{
				ds.Disconnect();
			}
		}

		private void GetSettingsList(XmlElement webmailElement)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

			XmlNode settingsListElem = webmailElement.OwnerDocument.CreateElement("settings_list");

			XmlAttribute showTextLabelsAttr = webmailElement.OwnerDocument.CreateAttribute("show_text_labels");
			showTextLabelsAttr.Value = (settings.ShowTextLabels) ? "1" : "0";
			settingsListElem.Attributes.Append(showTextLabelsAttr);

			XmlAttribute allowChangeSettingsAttr = webmailElement.OwnerDocument.CreateAttribute("allow_change_settings");
			allowChangeSettingsAttr.Value = (CurrentAccount.UserOfAccount.Settings.AllowChangeSettings) ? "1" : "0";
			settingsListElem.Attributes.Append(allowChangeSettingsAttr);

			XmlAttribute allowDhtmlEditorAttr = webmailElement.OwnerDocument.CreateAttribute("allow_dhtml_editor");
			allowDhtmlEditorAttr.Value = (CurrentAccount.UserOfAccount.Settings.AllowDhtmlEditor) ? "1" : "0";
			settingsListElem.Attributes.Append(allowDhtmlEditorAttr);

			XmlAttribute allowAddAccountAttr = webmailElement.OwnerDocument.CreateAttribute("allow_add_account");
			allowAddAccountAttr.Value = (settings.AllowUsersAddNewAccounts) ? "1" : "0";
			settingsListElem.Attributes.Append(allowAddAccountAttr);

			XmlAttribute enableMailBoxLimit = webmailElement.OwnerDocument.CreateAttribute("enable_mailbox_size_limit");
			enableMailBoxLimit.Value = (settings.EnableMailboxSizeLimit) ? "1" : "0";
			settingsListElem.Attributes.Append(enableMailBoxLimit);

			XmlAttribute msgsPerPageAttr = webmailElement.OwnerDocument.CreateAttribute("msgs_per_page");
			msgsPerPageAttr.Value = CurrentAccount.UserOfAccount.Settings.MsgsPerPage.ToString(CultureInfo.InvariantCulture);
			settingsListElem.Attributes.Append(msgsPerPageAttr);

			XmlAttribute contactsPerPageAttr = webmailElement.OwnerDocument.CreateAttribute("contacts_per_page");
			contactsPerPageAttr.Value = CurrentAccount.UserOfAccount.Settings.ContactsPerPage.ToString(CultureInfo.InvariantCulture);
			settingsListElem.Attributes.Append(contactsPerPageAttr);

            XmlAttribute timeFormatAttr = webmailElement.OwnerDocument.CreateAttribute("time_format");
            timeFormatAttr.Value = ((byte) CurrentAccount.UserOfAccount.Settings.DefaultTimeFormat).ToString(CultureInfo.InvariantCulture);
            settingsListElem.Attributes.Append(timeFormatAttr);

			XmlElement defSkinElem = webmailElement.OwnerDocument.CreateElement("def_skin");
			string defaultSkin = CurrentAccount.UserOfAccount.Settings.DefaultSkin;
			string[] supportedSkins = Utils.GetSupportedSkins((CurrentPage != null) ? CurrentPage.MapPath("skins") : string.Empty);
			if (Utils.GetCurrentSkinIndex(supportedSkins, defaultSkin) < 0)
			{
				if (supportedSkins.Length > 0)
				{
					defaultSkin = supportedSkins[0];
				}
			}
			defSkinElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(defaultSkin)));
			settingsListElem.AppendChild(defSkinElem);

			XmlElement defDateFmtElem = webmailElement.OwnerDocument.CreateElement("def_date_fmt");
			defDateFmtElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(CurrentAccount.UserOfAccount.Settings.DefaultDateFormat)));
			settingsListElem.AppendChild(defDateFmtElem);

			XmlElement defLangElem = webmailElement.OwnerDocument.CreateElement("def_lang");
			defLangElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(CurrentAccount.UserOfAccount.Settings.DefaultLanguage)));
			settingsListElem.AppendChild(defLangElem);

			XmlAttribute mailboxLimitAttr = webmailElement.OwnerDocument.CreateAttribute("mailbox_limit");
			mailboxLimitAttr.Value = CurrentAccount.UserOfAccount.Settings.MailboxLimit.ToString(CultureInfo.InvariantCulture);
			settingsListElem.Attributes.Append(mailboxLimitAttr);

			MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(CurrentAccount));
			try
			{
				mp.Connect();
				XmlAttribute mailboxSizeAttr = webmailElement.OwnerDocument.CreateAttribute("mailbox_size");
				mailboxSizeAttr.Value = mp.CalculateAccountSize(CurrentAccount.ID).ToString(CultureInfo.InvariantCulture);
				settingsListElem.Attributes.Append(mailboxSizeAttr);
			}
			finally
			{
				mp.Disconnect();
			}

			XmlAttribute hideFoldersAttr = webmailElement.OwnerDocument.CreateAttribute("hide_folders");
			hideFoldersAttr.Value = (CurrentAccount.UserOfAccount.Settings.HideFolders) ? "1" : "0";
			settingsListElem.Attributes.Append(hideFoldersAttr);

			XmlAttribute horizResizerAttr = webmailElement.OwnerDocument.CreateAttribute("horiz_resizer");
			horizResizerAttr.Value = CurrentAccount.UserOfAccount.Settings.HorizResizer.ToString(CultureInfo.InvariantCulture);
			settingsListElem.Attributes.Append(horizResizerAttr);

			XmlAttribute vertResizerAttr = webmailElement.OwnerDocument.CreateAttribute("vert_resizer");
			vertResizerAttr.Value = CurrentAccount.UserOfAccount.Settings.VertResizer.ToString(CultureInfo.InvariantCulture);
			settingsListElem.Attributes.Append(vertResizerAttr);

			XmlAttribute markAttr = webmailElement.OwnerDocument.CreateAttribute("mark");
			markAttr.Value = CurrentAccount.UserOfAccount.Settings.Mark.ToString(CultureInfo.InvariantCulture);
			settingsListElem.Attributes.Append(markAttr);

			XmlAttribute replyAttr = webmailElement.OwnerDocument.CreateAttribute("reply");
			replyAttr.Value = CurrentAccount.UserOfAccount.Settings.Reply.ToString(CultureInfo.InvariantCulture);
			settingsListElem.Attributes.Append(replyAttr);

			XmlAttribute viewModeAttr = webmailElement.OwnerDocument.CreateAttribute("view_mode");
			viewModeAttr.Value = ((int)CurrentAccount.UserOfAccount.Settings.ViewMode).ToString(CultureInfo.InvariantCulture);
			settingsListElem.Attributes.Append(viewModeAttr);

			XmlAttribute defTimeZoneAttr = webmailElement.OwnerDocument.CreateAttribute("def_timezone");
			defTimeZoneAttr.Value = CurrentAccount.UserOfAccount.Settings.DefaultTimeZone.ToString(CultureInfo.InvariantCulture);
			settingsListElem.Attributes.Append(defTimeZoneAttr);

			XmlAttribute allowDirectModeAttr = webmailElement.OwnerDocument.CreateAttribute("allow_direct_mode");
			allowDirectModeAttr.Value = (CurrentAccount.UserOfAccount.Settings.AllowDirectMode) ? "1" : "0";
			settingsListElem.Attributes.Append(allowDirectModeAttr);

            XmlAttribute directModeIsDefault = webmailElement.OwnerDocument.CreateAttribute("direct_mode_is_default");
            directModeIsDefault.Value = (settings.DirectModeIsDefault) ? "1" : "0";
            settingsListElem.Attributes.Append(directModeIsDefault);

			XmlAttribute allowContactsAttr = webmailElement.OwnerDocument.CreateAttribute("allow_contacts");
			allowContactsAttr.Value = (settings.AllowContacts) ? "1" : "0";
			settingsListElem.Attributes.Append(allowContactsAttr);

			XmlAttribute allowCalendarAttr = webmailElement.OwnerDocument.CreateAttribute("allow_calendar");
			allowCalendarAttr.Value = (settings.AllowCalendar) ? "1" : "0";
			settingsListElem.Attributes.Append(allowCalendarAttr);

			if (CurrentAccount.UserOfAccount.Columns != null)
			{
				XmlElement columnsElement = webmailElement.OwnerDocument.CreateElement("columns");
				foreach (UserColumn column in CurrentAccount.UserOfAccount.Columns)
				{
					XmlElement columnElem = webmailElement.OwnerDocument.CreateElement("column");

					XmlAttribute idAttr = webmailElement.OwnerDocument.CreateAttribute("id");
					idAttr.Value = column.IDColumn.ToString(CultureInfo.InvariantCulture);
					columnElem.Attributes.Append(idAttr);

					XmlAttribute valueAttr = webmailElement.OwnerDocument.CreateAttribute("value");
					valueAttr.Value = column.Value.ToString(CultureInfo.InvariantCulture);
					columnElem.Attributes.Append(valueAttr);

					columnsElement.AppendChild(columnElem);
				}
				settingsListElem.AppendChild(columnsElement);
			}

			webmailElement.AppendChild(settingsListElem);
		}

		private WebMailMessage InitWebmailMessage(XmlPacketMessage message)
		{
			MailMessage messageToSend = new MailMessage();
			messageToSend.Priority = (MailPriority)message.priority;
			messageToSend.From = EmailAddress.Parse(Utils.DecodeHtml(message.from));
			messageToSend.To = EmailAddressCollection.Parse(Utils.DecodeHtml(message.to));
			messageToSend.Cc = EmailAddressCollection.Parse(Utils.DecodeHtml(message.cc));
			messageToSend.Bcc = EmailAddressCollection.Parse(Utils.DecodeHtml(message.bcc));
			messageToSend.Subject = Utils.DecodeHtml(message.subject);
            messageToSend.Date = DateTime.Now;
			messageToSend.Charset = Utils.GetEncodingByCodePage(CurrentAccount.UserOfAccount.Settings.DefaultCharsetOut).HeaderName;

			if (message.bodyIsHtml)
			{
				Processor htmlProcessor = new Processor();
				htmlProcessor.Dom.OuterHtml = Utils.DecodeHtmlBody(message.bodyText);
				ElementCollection images = htmlProcessor.Images;
				RuleSet rs = new RuleSet();
				foreach (XmlPacketAttachment xmlAttach in message.attachments)
				{
					if (!xmlAttach.inline) continue;

					foreach (Element imageElement in images)
					{
						AttributeCollection searchAttrs = new AttributeCollection();
						AttributeCollection addAttrs = new AttributeCollection();
						AttributeCollection removeAttrs = new AttributeCollection();

						foreach (TagAttribute attr in imageElement.Attributes)
						{
							if (attr.Name.ToLower() == "src")
							{
								string cid = string.Empty;
								byte[] buffer = null;
								string filename = string.Empty;
								Regex r = new Regex(@"filename=(?<filename>[\w\.]+)", RegexOptions.None);
								Match m = r.Match(attr.Value);
								if (m.Success)
								{
									filename = m.Groups["filename"].Value;
									string tempFolder = Utils.GetTempFolderName((CurrentPage != null) ? CurrentPage.Session : null);
									string fullName = Path.Combine(tempFolder, filename);
									if (File.Exists(fullName))
									{
										using (FileStream fs = File.OpenRead(fullName))
										{
											buffer = new byte[fs.Length];
											fs.Read(buffer, 0, buffer.Length);						
										}
										if (buffer != null)
										{
											cid = Path.GetFileNameWithoutExtension(filename);
											messageToSend.Attachments.Add(buffer, xmlAttach.name, cid, null, null, NewAttachmentOptions.Inline, MailTransferEncoding.Base64);
										}
									}
								}

								TagAttribute searchAttr = new Attribute();
								searchAttr.Name = attr.Name;
								searchAttr.Value = Regex.Escape(attr.Value);

								searchAttrs.Add(searchAttr);

								MailBee.Html.TagAttribute oldAttr = new Attribute();
								oldAttr.Definition = attr.Name;
								removeAttrs.Add(oldAttr);

								MailBee.Html.TagAttribute newAttr = new Attribute();
								//newAttr.Definition = "src='about:blank'";
								newAttr.Name = attr.Name;
								newAttr.Value = string.Format(@"""cid:{0}""", cid);
								addAttrs.Add(newAttr);

								break;
							}
						}
						rs.AddTagProcessingRule("img", searchAttrs, addAttrs, searchAttrs, true);
					}
				}
				messageToSend.BodyHtmlText = htmlProcessor.Dom.ProcessToString(rs, null);
			}
			else
			{
				messageToSend.BodyPlainText = Utils.DecodeHtmlBody(message.bodyText);
			}
			foreach (XmlPacketAttachment xmlAttach in message.attachments)
			{
				if (!xmlAttach.inline)
				{
					string fullPath = Path.Combine(Utils.GetTempFolderName((CurrentPage != null) ? CurrentPage.Session : null), Utils.DecodeHtml(xmlAttach.temp_name));
					messageToSend.Attachments.Add(fullPath, Utils.DecodeHtml(xmlAttach.name));
				}
			}

			if (message.groups != null)
			{
				foreach (XmlPacketGroup group in message.groups)
				{
					AddressBookGroup dbGroup = _actions.GetGroup(group.id);
					dbGroup.UseFrequency++;
					_actions.UpdateGroup(dbGroup);
				}
			}

			WebMailMessage webMsg = new WebMailMessage(CurrentAccount);
			webMsg.Init(messageToSend, ((webMsg.StrUid != null) && (webMsg.StrUid.Length > 0)), null);
			webMsg.IDMsg = message.id;
			webMsg.StrUid = message.uid;
            webMsg.LoanApplicants = Utils.DecodeHtml(message.loanapplicants);
            webMsg.LoanAppIDs = Utils.DecodeHtml(message.loanappids);
            if (message.folder != null)
                webMsg.FolderFullName = message.folder.full_name;

			return webMsg;
		}

		private void SaveMessage(XmlElement webmailNode, XmlPacketMessage message,bool IsBackSave)
		{
			WebMailMessage msg = InitWebmailMessage(message);
            int MsgID = -1;
            switch (msg.FolderFullName)
            {
                case "Inbox":
                    {
                        MsgID = _actions.SaveMessageInbox(msg);
                        break;
                    }
                case "Drafts":
                    {
                        MsgID = _actions.SaveMessageToDrafts(msg);
                        break;
                    }
                case "Sent Items":
                    {
                        MsgID = _actions.SaveMessageSentItems(msg);
                        break;
                    }
                default:
                    MsgID = _actions.SaveMessageToDrafts(msg);
                    break;
            }
			
			XmlElement updateElem = webmailNode.OwnerDocument.CreateElement("update");

			XmlAttribute valueAttr = webmailNode.OwnerDocument.CreateAttribute("value");
			valueAttr.Value = "save_message";
			updateElem.Attributes.Append(valueAttr);

            XmlAttribute idAttr = webmailNode.OwnerDocument.CreateAttribute("id");
            idAttr.Value = MsgID.ToString();
            updateElem.Attributes.Append(idAttr);

            XmlElement uidChild = updateElem.OwnerDocument.CreateElement("uid");
            uidChild.AppendChild(uidChild.OwnerDocument.CreateCDataSection(msg.StrUid));
            updateElem.AppendChild(uidChild);
			
			webmailNode.AppendChild(updateElem);
		}

		private void SendMessage(XmlElement webmailNode, XmlPacketMessage message)
		{
            bool isAddrDiscard = false;
            if (_actions.CurrentAccount != null && _actions.CurrentAccount.IsDemo)
            {
                EmailAddressCollection toColl = EmailAddressCollection.Parse(Utils.DecodeHtml(message.to));
                EmailAddressCollection ccColl = EmailAddressCollection.Parse(Utils.DecodeHtml(message.cc));
                EmailAddressCollection bccColl = EmailAddressCollection.Parse(Utils.DecodeHtml(message.bcc));
                int toCount = toColl.Count;
                int ccCount = ccColl.Count;
                int bccCount = bccColl.Count;
                if (toCount > 3)
                {
                    EmailAddressCollection toAddr = new EmailAddressCollection();
                    for (int i = 0; i < 3; i++)
                    {
                        toAddr.Add(toColl[i]);
                    }
                    toColl = toAddr;
                    ccColl = new EmailAddressCollection();
                    bccColl = new EmailAddressCollection();
                    isAddrDiscard = true;
                }
                else if (ccCount > 3 - toCount)
                {
                    EmailAddressCollection ccAddr = new EmailAddressCollection();
                    for (int i = 0; i < 3 - toCount; i++)
                    {
                        ccAddr.Add(ccColl[i]);
                    }
                    ccColl = ccAddr;
                    bccColl = new EmailAddressCollection();
                    isAddrDiscard = true;
                }
                else if (bccCount > 3 - toCount - ccCount)
                {
                    EmailAddressCollection bccAddr = new EmailAddressCollection();
                    for (int i = 0; i < 3 - toCount - ccCount; i++)
                    {
                        bccAddr.Add(bccColl[i]);
                    }
                    bccColl = bccAddr;
                    isAddrDiscard = true;
                }
                message.to = toColl.AsString;
                message.cc = ccColl.AsString;
                message.bcc = bccColl.AsString;
            }
            WebMailMessage msg = InitWebmailMessage(message);
			_actions.IncrementContactsFrequency(msg.ToMsg);
			_actions.IncrementContactsFrequency(msg.CcMsg);
			_actions.IncrementContactsFrequency(msg.BccMsg);

			_actions.SendMessage(msg);

		    XmlElement updateElem = webmailNode.OwnerDocument.CreateElement("update");

			XmlAttribute valueAttr = webmailNode.OwnerDocument.CreateAttribute("value");
            if (isAddrDiscard)
            {
                valueAttr.Value = "send_message_demo";
            }
            else
            {
                valueAttr.Value = "send_message";
            }
			updateElem.Attributes.Append(valueAttr);

			webmailNode.AppendChild(updateElem);
		}

		private void DeleteContacts(XmlPacketContact[] contacts, XmlPacketGroup[] groups)
		{
			AddressBookContact[] addrBookContacts = new AddressBookContact[contacts.Length];
			for (int i = 0; i < addrBookContacts.Length; i++)
			{
				addrBookContacts[i] = new AddressBookContact();
				addrBookContacts[i].IDAddr = contacts[i].id;
				addrBookContacts[i].IDUser = CurrentAccount.UserOfAccount.ID;
			}
			AddressBookGroup[] addrBookGroups = new AddressBookGroup[groups.Length];
			for (int i = 0; i < addrBookGroups.Length; i++)
			{
				addrBookGroups[i] = new AddressBookGroup();
				addrBookGroups[i].IDGroup = groups[i].id;
				addrBookGroups[i].IDUser = CurrentAccount.UserOfAccount.ID;
			}
			_actions.DeleteContacts(addrBookContacts, addrBookGroups);
		}

		private AddressBookGroup InitAddressBookGroupFromXml(XmlPacketGroup group)
		{
			AddressBookGroup addressBookGroup = _actions.GetGroup(group.id);
			if (addressBookGroup == null) addressBookGroup = new AddressBookGroup();

            addressBookGroup.IDGroup = group.id;
			addressBookGroup.IDUser = CurrentAccount.UserOfAccount.ID;
			addressBookGroup.GroupName = group.name;
			addressBookGroup.Organization = group.organization;
			addressBookGroup.Email = group.email;
			addressBookGroup.Company = group.company;
			addressBookGroup.Street = group.street;
			addressBookGroup.City = group.city;
			addressBookGroup.State = group.state;
			addressBookGroup.Zip = group.zip;
			addressBookGroup.Country = group.country;
			addressBookGroup.Fax = group.fax;
			addressBookGroup.Phone = group.phone;
			addressBookGroup.Web = group.web;

			addressBookGroup.Contacts = new AddressBookContact[group.contacts.Length];
			for (int i = 0; i < addressBookGroup.Contacts.Length; i++)
			{
				addressBookGroup.Contacts[i] = new AddressBookContact();
				addressBookGroup.Contacts[i].IDAddr = group.contacts[i].id;
				addressBookGroup.Contacts[i].IDUser = CurrentAccount.UserOfAccount.ID;
				addressBookGroup.Contacts[i].FullName = group.contacts[i].fullname;
				addressBookGroup.Contacts[i].HEmail = group.contacts[i].personal_email;
			}

			addressBookGroup.NewContacts = new AddressBookContact[group.new_contacts.Length];
			for (int i = 0; i < addressBookGroup.NewContacts.Length; i++)
			{
				addressBookGroup.NewContacts[i] = new AddressBookContact();
				addressBookGroup.NewContacts[i].IDAddr = group.new_contacts[i].id;
				addressBookGroup.NewContacts[i].IDUser = CurrentAccount.UserOfAccount.ID;
				addressBookGroup.NewContacts[i].FullName = group.new_contacts[i].fullname;
				addressBookGroup.NewContacts[i].HEmail = group.new_contacts[i].personal_email;
			}
			return addressBookGroup;
		}

		private void UpdateGroup(AddressBookGroup group)
		{
			WebmailResourceManager _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();				
			if (!Validation.CheckIt(Validation.ValidationTask.GroupName, group.GroupName))
			{
				throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
			}
			group.GroupName = Validation.Corrected;
			AddressBookGroup[] existedGroups = _actions.GetGroups();
			foreach (AddressBookGroup g in existedGroups)
			{
				if ((g.GroupName==group.GroupName)&&(g.IDGroup!=group.IDGroup)) throw (new WebMailException(_resMan.GetString("WarningGroupAlreadyExist")));
			}
			_actions.UpdateGroup(group);
		}

		private AddressBookContact InitAddressBookContactFromXml(XmlPacketContact contact)
		{
			AddressBookContact addressBookContact = _actions.GetContact(contact.id);
			if (addressBookContact == null) addressBookContact = new AddressBookContact();

			addressBookContact.IDAddr = contact.id;
			addressBookContact.IDUser = CurrentAccount.UserOfAccount.ID;
			addressBookContact.PrimaryEmail = (ContactPrimaryEmail)contact.primary_email;
			addressBookContact.UseFriendlyName = (contact.use_friendly_name == 1) ? true : false;
			addressBookContact.FullName = contact.fullname;
			addressBookContact.BirthdayDay = contact.birthday_day;
			addressBookContact.BirthdayMonth = contact.birthday_month;
			addressBookContact.BirthdayYear = contact.birthday_year;
			addressBookContact.HEmail = contact.personal_email;
			addressBookContact.HStreet = contact.personal_street;
			addressBookContact.HCity = contact.personal_city;
			addressBookContact.HState = contact.personal_state;
			addressBookContact.HZip = contact.personal_zip;
			addressBookContact.HCountry = contact.personal_country;
			addressBookContact.HFax = contact.personal_fax;
			addressBookContact.HPhone = contact.personal_phone;
			addressBookContact.HMobile = contact.personal_mobile;
			addressBookContact.HWeb = contact.personal_web;
			addressBookContact.BEmail = contact.business_email;
			addressBookContact.BCompany = contact.business_company;
			addressBookContact.BJobTitle = contact.business_job_title;
			addressBookContact.BDepartment = contact.business_department;
			addressBookContact.BOffice = contact.business_office;
			addressBookContact.BStreet = contact.business_street;
			addressBookContact.BCity = contact.business_city;
			addressBookContact.BState = contact.business_state;
			addressBookContact.BZip = contact.business_zip;
			addressBookContact.BCountry = contact.business_country;
			addressBookContact.BFax = contact.business_fax;
			addressBookContact.BPhone = contact.business_phone;
			addressBookContact.BWeb = contact.business_web;
			addressBookContact.OtherEmail = contact.other_email;
			addressBookContact.Notes = contact.other_notes;
			ArrayList groups = new ArrayList();
			foreach (XmlPacketGroup xmlGroup in  contact.groups)
			{
				AddressBookGroup group = new AddressBookGroup();
				group.IDGroup = xmlGroup.id;
				groups.Add(group);
			}
			addressBookContact.Groups = (AddressBookGroup[])groups.ToArray(typeof(AddressBookGroup));

			return addressBookContact;
		}

		private void UpdateContact(AddressBookContact updateContact)
		{
			bool errorFlag = true;
			if (updateContact.HWeb.Length!=0)
			{
				if (Validation.CheckIt(Validation.ValidationTask.ContactsWebPage, updateContact.HWeb))
				{
					updateContact.HWeb = Validation.Corrected;
				}
			}
			if (updateContact.BWeb.Length!=0)
			{
				if (Validation.CheckIt(Validation.ValidationTask.ContactsWebPage, updateContact.BWeb))
				{
					updateContact.BWeb = Validation.Corrected;
				}
			}
			//If emails and name is empty - ERROR!!!
			int existedFieldsNum = 0;
			if (updateContact.BEmail.Length!=0) existedFieldsNum++;
			if (updateContact.HEmail.Length!=0) existedFieldsNum++;
			if (updateContact.OtherEmail.Length!=0) existedFieldsNum++;
			if ((existedFieldsNum!=0)||(updateContact.FullName.Length!=0)) errorFlag = false;
			if (!errorFlag)
			{
				_actions.UpdateContact(updateContact);
			}
			else
			{
				WebmailResourceManager _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();				
				throw (new WebMailException(_resMan.GetString("WarningContactNotComplete")));
			}
		}

		private void NewGroup(AddressBookGroup group)
		{
			WebmailResourceManager _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();				
			if (!Validation.CheckIt(Validation.ValidationTask.GroupName, group.GroupName))
			{
				throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
			}
			group.GroupName = Validation.Corrected;
			AddressBookGroup[] existedGroups = _actions.GetGroups();
			foreach (AddressBookGroup g in existedGroups)
			{
				if (g.GroupName==group.GroupName) throw (new WebMailException(_resMan.GetString("WarningGroupAlreadyExist")));
			}
			_actions.NewGroup(group);
		}

		private void NewContact(AddressBookContact newContact)
		{
			bool errorFlag = true;
			if (newContact.HWeb.Length!=0)
			{
				if (Validation.CheckIt(Validation.ValidationTask.ContactsWebPage, newContact.HWeb))
				{
					newContact.HWeb = Validation.Corrected;
				}
			}
			if (newContact.BWeb.Length!=0)
			{
				if (Validation.CheckIt(Validation.ValidationTask.ContactsWebPage, newContact.BWeb))
				{
					newContact.BWeb = Validation.Corrected;
				}
			}
			//If emails and name is empty - ERROR!!!
			int existedFieldsNum = 0;
			if (newContact.BEmail.Length!=0) existedFieldsNum++;
			if (newContact.HEmail.Length!=0) existedFieldsNum++;
			if (newContact.OtherEmail.Length!=0) existedFieldsNum++;
			if ((existedFieldsNum!=0)||(newContact.FullName.Length!=0)) errorFlag = false;
			if (!errorFlag)
			{
				_actions.NewContact(newContact);
			}
			else
			{
				WebmailResourceManager _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();				
				throw (new WebMailException(_resMan.GetString("WarningContactNotComplete")));
			}
		}

		private void GetGroups(XmlElement webmailNode)
		{
			AddressBookGroup[] groups = null;
			groups = _actions.GetGroups();
			if (groups != null)
			{
				XmlElement groupsElem = webmailNode.OwnerDocument.CreateElement("groups");

				foreach (AddressBookGroup group in groups)
				{
					XmlElement groupElem = webmailNode.OwnerDocument.CreateElement("group");
						
					XmlAttribute idAttr = webmailNode.OwnerDocument.CreateAttribute("id");
					idAttr.Value = group.IDGroup.ToString(CultureInfo.InvariantCulture);
					groupElem.Attributes.Append(idAttr);

					XmlElement nameElem = webmailNode.OwnerDocument.CreateElement("name");
					nameElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(group.GroupName)));
					groupElem.AppendChild(nameElem);

					groupsElem.AppendChild(groupElem);
				}

				webmailNode.AppendChild(groupsElem);
			}
		}

		private void GetGroup(XmlElement webmailNode, int id_group)
		{
			AddressBookGroup group = null;
			group = _actions.GetGroup(id_group);
			if (group != null)
			{
				XmlElement groupElem = webmailNode.OwnerDocument.CreateElement("group");

				XmlAttribute idAttr = webmailNode.OwnerDocument.CreateAttribute("id");
				idAttr.Value = group.IDGroup.ToString(CultureInfo.InvariantCulture);
				groupElem.Attributes.Append(idAttr);

				XmlAttribute isOrgAttr = webmailNode.OwnerDocument.CreateAttribute("organization");
				isOrgAttr.Value = (group.Organization) ? "1" : "0";
				groupElem.Attributes.Append(isOrgAttr);

				XmlElement nameElem = webmailNode.OwnerDocument.CreateElement("name");
				nameElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(group.GroupName)));
				groupElem.AppendChild(nameElem);

				XmlElement emailElem = webmailNode.OwnerDocument.CreateElement("email");
				emailElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(group.Email)));
				groupElem.AppendChild(emailElem);

				XmlElement companyElem = webmailNode.OwnerDocument.CreateElement("company");
				companyElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(group.Company)));
				groupElem.AppendChild(companyElem);

				XmlElement streetElem = webmailNode.OwnerDocument.CreateElement("street");
				streetElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(group.Street)));
				groupElem.AppendChild(streetElem);

				XmlElement cityElem = webmailNode.OwnerDocument.CreateElement("city");
				cityElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(group.City)));
				groupElem.AppendChild(cityElem);

				XmlElement stateElem = webmailNode.OwnerDocument.CreateElement("state");
				stateElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(group.State)));
				groupElem.AppendChild(stateElem);

				XmlElement zipElem = webmailNode.OwnerDocument.CreateElement("zip");
				zipElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(group.Zip)));
				groupElem.AppendChild(zipElem);

				XmlElement countryElem = webmailNode.OwnerDocument.CreateElement("country");
				countryElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(group.Country)));
				groupElem.AppendChild(countryElem);

				XmlElement faxElem = webmailNode.OwnerDocument.CreateElement("fax");
				faxElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(group.Fax)));
				groupElem.AppendChild(faxElem);

				XmlElement phoneElem = webmailNode.OwnerDocument.CreateElement("phone");
				phoneElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(group.Phone)));
				groupElem.AppendChild(phoneElem);

				XmlElement webElem = webmailNode.OwnerDocument.CreateElement("web");
				webElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(group.Web)));
				groupElem.AppendChild(webElem);

				XmlElement contactsElem = webmailNode.OwnerDocument.CreateElement("contacts");

				foreach (AddressBookContact contact in group.Contacts)
				{
					XmlElement contactElem = webmailNode.OwnerDocument.CreateElement("contact");
						
					idAttr = webmailNode.OwnerDocument.CreateAttribute("id");
					idAttr.Value = contact.IDAddr.ToString(CultureInfo.InvariantCulture);
					contactElem.Attributes.Append(idAttr);

					XmlElement fullnameElem = webmailNode.OwnerDocument.CreateElement("fullname");
					fullnameElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.FullName)));
					contactElem.AppendChild(fullnameElem);

					XmlElement contactEmailElem = webmailNode.OwnerDocument.CreateElement("email");
					contactEmailElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.HEmail)));
					contactElem.AppendChild(contactEmailElem);

					contactsElem.AppendChild(contactElem);
				}

				groupElem.AppendChild(contactsElem);

				webmailNode.AppendChild(groupElem);
			}
		}

		private void GetContactsGroups(XmlElement webmailNode, int page, short sort_field, short sort_order, int id_group, string look_for, int look_for_type)
		{
			AddressBookGroupContact[] groupsContacts = null;

			int contacts_count = 0;
			int groups_count = 0;
			groupsContacts = _actions.GetContactsGroups(page, sort_field, sort_order, id_group, look_for, look_for_type, out contacts_count, out groups_count);
			
			XmlElement contacts_groupsElem = webmailNode.OwnerDocument.CreateElement("contacts_groups");

			XmlAttribute contactsCountAttr = webmailNode.OwnerDocument.CreateAttribute("contacts_count");
			contactsCountAttr.Value = contacts_count.ToString(CultureInfo.InvariantCulture);
			contacts_groupsElem.Attributes.Append(contactsCountAttr);

			XmlAttribute groupsCountAttr = webmailNode.OwnerDocument.CreateAttribute("groups_count");
			groupsCountAttr.Value = groups_count.ToString(CultureInfo.InvariantCulture);
			contacts_groupsElem.Attributes.Append(groupsCountAttr);

			XmlAttribute pageAttr = webmailNode.OwnerDocument.CreateAttribute("page");
			pageAttr.Value = page.ToString(CultureInfo.InvariantCulture);
			contacts_groupsElem.Attributes.Append(pageAttr);

			XmlAttribute sortFieldAttr = webmailNode.OwnerDocument.CreateAttribute("sort_field");
			sortFieldAttr.Value = sort_field.ToString(CultureInfo.InvariantCulture);
			contacts_groupsElem.Attributes.Append(sortFieldAttr);

			XmlAttribute sortOrderAttr = webmailNode.OwnerDocument.CreateAttribute("sort_order");
			sortOrderAttr.Value = sort_order.ToString(CultureInfo.InvariantCulture);
			contacts_groupsElem.Attributes.Append(sortOrderAttr);

			XmlAttribute idGroupAttr = webmailNode.OwnerDocument.CreateAttribute("id_group");
			idGroupAttr.Value = id_group.ToString(CultureInfo.InvariantCulture);
			contacts_groupsElem.Attributes.Append(idGroupAttr);

			XmlElement lookForElem = webmailNode.OwnerDocument.CreateElement("look_for");
			lookForElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(look_for));
			contacts_groupsElem.AppendChild(lookForElem);

			XmlAttribute lookForTypeAttr = webmailNode.OwnerDocument.CreateAttribute("type");
			lookForTypeAttr.Value = look_for_type.ToString(CultureInfo.InvariantCulture);
			lookForElem.Attributes.Append(lookForTypeAttr);

			foreach (AddressBookGroupContact groupContact in groupsContacts)
			{
				XmlElement contactGroupElem = webmailNode.OwnerDocument.CreateElement("contact_group");

				XmlAttribute idAttr = webmailNode.OwnerDocument.CreateAttribute("id");
				idAttr.Value = groupContact.id.ToString(CultureInfo.InvariantCulture);
				contactGroupElem.Attributes.Append(idAttr);

				XmlAttribute isGroupAttr = webmailNode.OwnerDocument.CreateAttribute("is_group");
				isGroupAttr.Value = (groupContact.isGroup) ? "1" : "0";
				contactGroupElem.Attributes.Append(isGroupAttr);
				if (groupContact.fullname != null)
				{
					XmlElement nameElem = webmailNode.OwnerDocument.CreateElement("name");
					nameElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(groupContact.fullname)));
					contactGroupElem.AppendChild(nameElem);
				}
				if ((groupContact.email != null) && (groupContact.email.Length > 0))
				{
					XmlElement emailElem = webmailNode.OwnerDocument.CreateElement("email");
					emailElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(groupContact.email)));
					contactGroupElem.AppendChild(emailElem);
				}
				else
				{
					if (groupContact.isGroup)
					{
						AddressBookGroup group = _actions.GetGroup((int)groupContact.id);
						if (group != null)
						{
							StringBuilder sb = new StringBuilder();
							foreach (AddressBookContact contact in group.Contacts)
							{
								switch (contact.PrimaryEmail)
								{
									case ContactPrimaryEmail.Personal:
										if ((contact.HEmail != null) && (contact.HEmail.Length > 0)) sb.AppendFormat("{0},", contact.HEmail);
										break;
									case ContactPrimaryEmail.Business:
										if ((contact.BEmail != null) && (contact.BEmail.Length > 0)) sb.AppendFormat("{0},", contact.BEmail);
										break;
									case ContactPrimaryEmail.Other:
										if ((contact.OtherEmail != null) && (contact.OtherEmail.Length > 0)) sb.AppendFormat("{0},", contact.OtherEmail);
										break;
								}
							}
							if (sb.Length > 0) sb.Remove(sb.Length - 1, 1); // remove last ','
							XmlElement emailElem = webmailNode.OwnerDocument.CreateElement("email");
							emailElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(sb.ToString())));
							contactGroupElem.AppendChild(emailElem);
						}
					}
				}
				contacts_groupsElem.AppendChild(contactGroupElem);
			}
			webmailNode.AppendChild(contacts_groupsElem);
		}

		private void GetContact(XmlElement webmailNode, long id_addr)
		{
			AddressBookContact contact = _actions.GetContact(id_addr);

			if (contact != null)
			{
				XmlElement contactElem = webmailNode.OwnerDocument.CreateElement("contact");

				XmlAttribute idAttr = webmailNode.OwnerDocument.CreateAttribute("id");
				idAttr.Value = contact.IDAddr.ToString(CultureInfo.InvariantCulture);
				contactElem.Attributes.Append(idAttr);

				XmlAttribute primaryEmailAttr = webmailNode.OwnerDocument.CreateAttribute("primary_email");
				primaryEmailAttr.Value = Convert.ToInt32(contact.PrimaryEmail).ToString(CultureInfo.InvariantCulture);
				contactElem.Attributes.Append(primaryEmailAttr);

				XmlAttribute useFriendlyNameAttr = webmailNode.OwnerDocument.CreateAttribute("use_friendly_name");
				useFriendlyNameAttr.Value = (contact.UseFriendlyName) ? "1" : "0";
				contactElem.Attributes.Append(useFriendlyNameAttr);

				XmlElement fullnameElem = webmailNode.OwnerDocument.CreateElement("fullname");
				fullnameElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.FullName)));
				contactElem.AppendChild(fullnameElem);

				XmlElement birthdayElem = webmailNode.OwnerDocument.CreateElement("birthday");
					
				XmlAttribute birthdayDay = webmailNode.OwnerDocument.CreateAttribute("day");
				birthdayDay.Value = contact.BirthdayDay.ToString(CultureInfo.InvariantCulture);
				birthdayElem.Attributes.Append(birthdayDay);

				XmlAttribute birthdayMonth = webmailNode.OwnerDocument.CreateAttribute("month");
				birthdayMonth.Value = contact.BirthdayMonth.ToString(CultureInfo.InvariantCulture);
				birthdayElem.Attributes.Append(birthdayMonth);

				XmlAttribute birthdayYear = webmailNode.OwnerDocument.CreateAttribute("year");
				birthdayYear.Value = contact.BirthdayYear.ToString(CultureInfo.InvariantCulture);
				birthdayElem.Attributes.Append(birthdayYear);

				contactElem.AppendChild(birthdayElem);

				XmlElement personalElem = webmailNode.OwnerDocument.CreateElement("personal");

				XmlElement emailElem = webmailNode.OwnerDocument.CreateElement("email");
				emailElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.HEmail)));
				personalElem.AppendChild(emailElem);

				XmlElement streetElem = webmailNode.OwnerDocument.CreateElement("street");
				streetElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.HStreet)));
				personalElem.AppendChild(streetElem);

				XmlElement cityElem = webmailNode.OwnerDocument.CreateElement("city");
				cityElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.HCity)));
				personalElem.AppendChild(cityElem);

				XmlElement stateElem = webmailNode.OwnerDocument.CreateElement("state");
				stateElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.HState)));
				personalElem.AppendChild(stateElem);

				XmlElement zipElem = webmailNode.OwnerDocument.CreateElement("zip");
				zipElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.HZip)));
				personalElem.AppendChild(zipElem);

				XmlElement countryElem = webmailNode.OwnerDocument.CreateElement("country");
				countryElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.HCountry)));
				personalElem.AppendChild(countryElem);

				XmlElement faxElem = webmailNode.OwnerDocument.CreateElement("fax");
				faxElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.HFax)));
				personalElem.AppendChild(faxElem);

				XmlElement phoneElem = webmailNode.OwnerDocument.CreateElement("phone");
				phoneElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.HPhone)));
				personalElem.AppendChild(phoneElem);

				XmlElement mobileElem = webmailNode.OwnerDocument.CreateElement("mobile");
				mobileElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.HMobile)));
				personalElem.AppendChild(mobileElem);

				XmlElement webElem = webmailNode.OwnerDocument.CreateElement("web");
				webElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.HWeb)));
				personalElem.AppendChild(webElem);

				contactElem.AppendChild(personalElem);

				XmlElement businessElem = webmailNode.OwnerDocument.CreateElement("business");

				emailElem = webmailNode.OwnerDocument.CreateElement("email");
				emailElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.BEmail)));
				businessElem.AppendChild(emailElem);

				XmlElement companyElem = webmailNode.OwnerDocument.CreateElement("company");
				companyElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.BCompany)));
				businessElem.AppendChild(companyElem);

				XmlElement job_titleElem = webmailNode.OwnerDocument.CreateElement("job_title");
				job_titleElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.BJobTitle)));
				businessElem.AppendChild(job_titleElem);

				XmlElement departmentElem = webmailNode.OwnerDocument.CreateElement("department");
				departmentElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.BDepartment)));
				businessElem.AppendChild(departmentElem);

				XmlElement officeElem = webmailNode.OwnerDocument.CreateElement("office");
				officeElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.BOffice)));
				businessElem.AppendChild(officeElem);

				streetElem = webmailNode.OwnerDocument.CreateElement("street");
				streetElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.BStreet)));
				businessElem.AppendChild(streetElem);

				cityElem = webmailNode.OwnerDocument.CreateElement("city");
				cityElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.BCity)));
				businessElem.AppendChild(cityElem);

				stateElem = webmailNode.OwnerDocument.CreateElement("state");
				stateElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.BState)));
				businessElem.AppendChild(stateElem);

				zipElem = webmailNode.OwnerDocument.CreateElement("zip");
				zipElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.BZip)));
				businessElem.AppendChild(zipElem);

				countryElem = webmailNode.OwnerDocument.CreateElement("country");
				countryElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.BCountry)));
				businessElem.AppendChild(countryElem);

				faxElem = webmailNode.OwnerDocument.CreateElement("fax");
				faxElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.BFax)));
				businessElem.AppendChild(faxElem);

				phoneElem = webmailNode.OwnerDocument.CreateElement("phone");
				phoneElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.BPhone)));
				businessElem.AppendChild(phoneElem);

				webElem = webmailNode.OwnerDocument.CreateElement("web");
				webElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.BWeb)));
				businessElem.AppendChild(webElem);

				contactElem.AppendChild(businessElem);

				XmlElement otherElem = webmailNode.OwnerDocument.CreateElement("other");

				emailElem = webmailNode.OwnerDocument.CreateElement("email");
				emailElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.OtherEmail)));
				otherElem.AppendChild(emailElem);

				XmlElement notesElem = webmailNode.OwnerDocument.CreateElement("notes");
				notesElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(contact.Notes)));
				otherElem.AppendChild(notesElem);

				contactElem.AppendChild(otherElem);

				if (contact.Groups != null)
				{
					XmlElement groupsElem = webmailNode.OwnerDocument.CreateElement("groups");
					foreach (AddressBookGroup group in contact.Groups)
					{
						XmlElement groupElem = webmailNode.OwnerDocument.CreateElement("group");

						idAttr = webmailNode.OwnerDocument.CreateAttribute("id");
						idAttr.Value = group.IDGroup.ToString(CultureInfo.InvariantCulture);
						groupElem.Attributes.Append(idAttr);

						XmlElement nameElem = webmailNode.OwnerDocument.CreateElement("name");
						nameElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(group.GroupName)));
						groupElem.AppendChild(nameElem);

						groupsElem.AppendChild(groupElem);
					}
					contactElem.AppendChild(groupsElem);
				}

				webmailNode.AppendChild(contactElem);
			}
		}

		private void AddContacts(XmlElement webmailNode, int id_group, XmlPacketContact[] contacts)
		{
			AddressBookContact[] addrBookContacts = new AddressBookContact[contacts.Length];
			for (int i = 0; i < addrBookContacts.Length; i++)
			{
				addrBookContacts[i] = new AddressBookContact();
				addrBookContacts[i].IDAddr = contacts[i].id;
				addrBookContacts[i].IDUser = CurrentAccount.UserOfAccount.ID;
			}

			_actions.AddContacts(id_group, addrBookContacts);

			XmlElement updateElem = webmailNode.OwnerDocument.CreateElement("update");
			XmlAttribute valueAttr = webmailNode.OwnerDocument.CreateAttribute("value");
			valueAttr.Value = "group";
			updateElem.Attributes.Append(valueAttr);

			webmailNode.AppendChild(updateElem);
		}

		private void DeleteFolders(int id_acct, XmlPacketFolder[] folders)
		{
			ArrayList foldersArr = new ArrayList();
			foreach (XmlPacketFolder xmlFld in folders)
			{
				Folder fld = new Folder(xmlFld.id, CurrentAccount.ID, -1, xmlFld.name, xmlFld.full_name, FolderType.Custom, (FolderSyncType)xmlFld.sync_type, false, 0);
				if (fld != null)
				{
					foldersArr.Add(fld);
				}
			}
			_actions.DeleteFolders(id_acct, (Folder[])foldersArr.ToArray(typeof(Folder)));
		}

		private void UpdateFolders(int id_acct, XmlPacketFolder[] folders)
		{
			Stack pathStack = new Stack();
			ArrayList foldersArr = new ArrayList();
			foreach (XmlPacketFolder xmlFld in folders)
			{
				bool needToRename = false;
				Folder fld = _actions.GetFolder(xmlFld.id);
				if (fld != null)
				{
					while (pathStack.Count > 0)
					{
						if (fld.FullPath.StartsWith(((RenameFolderStruct)pathStack.Peek()).oldPath))
						{
							fld.UpdateFullPath = ((RenameFolderStruct)pathStack.Peek()).newPath + fld.FullPath.Substring(((RenameFolderStruct)pathStack.Peek()).oldPath.Length);
							needToRename = true;
							break;
						}
						else
						{
							pathStack.Pop();
						}
					}
					if (string.Compare(fld.Name, xmlFld.name, false, CultureInfo.InvariantCulture) != 0)
					{
						needToRename = true;
					}
					if (needToRename)
					{
						if (Validation.CheckIt(Validation.ValidationTask.FolderName, xmlFld.name))
							{
								fld.UpdateName = Validation.Corrected;
							}
						else
							{
								WebmailResourceManager _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();				
								throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
							}
						fld.UpdateFullPath = Folder.CreateNewFullPath(fld.UpdateFullPath, CurrentAccount.Delimiter, xmlFld.name);
						RenameFolderStruct rfs = new RenameFolderStruct();
						rfs.newPath = fld.UpdateFullPath;
						rfs.oldPath = fld.FullPath;
						pathStack.Push(rfs);
					}
					fld.Hide = (xmlFld.hide == 1) ? true : false;
					fld.FolderOrder = xmlFld.fld_order;
					fld.SyncType = (FolderSyncType)xmlFld.sync_type;
					foldersArr.Add(fld);
				}
			}
			_actions.UpdateFolders(id_acct, (Folder[])foldersArr.ToArray(typeof(Folder)));
		}

		private void NewFolder(int id_acct, int id_parent, string full_name_parent, string name, int create)
		{
			if (Validation.CheckIt(Validation.ValidationTask.FolderName, name))
				{
					name = Validation.Corrected;
					_actions.NewFolder(id_acct, id_parent, full_name_parent, name, create);
				}
			else
				{
				WebmailResourceManager _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();				
				throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
				}
		}

		private void UpdateSignature(XmlElement webmailNode, int id_acct, int type, int opt, string str)
		{
			_actions.UpdateSignature(id_acct, type, opt, str);

			XmlElement updateElem = webmailNode.OwnerDocument.CreateElement("update");

			XmlAttribute valueAttr = webmailNode.OwnerDocument.CreateAttribute("value");
			valueAttr.Value = "signature";
			updateElem.Attributes.Append(valueAttr);

			webmailNode.AppendChild(updateElem);
		}

		private void GetSignature(XmlElement webmailNode, int id_acct)
		{
			if ((CurrentAccount != null) && (CurrentAccount.UserOfAccount != null))
			{
				Account acct = Account.LoadFromDb(id_acct, CurrentAccount.UserOfAccount.ID);
				if (acct != null)
				{
					XmlElement signatureElem = webmailNode.OwnerDocument.CreateElement("signature");

					XmlAttribute idAttr = webmailNode.OwnerDocument.CreateAttribute("id");
					idAttr.Value = acct.ID.ToString(CultureInfo.InvariantCulture);
					signatureElem.Attributes.Append(idAttr);

					XmlAttribute typeAttr = webmailNode.OwnerDocument.CreateAttribute("type");
					typeAttr.Value = ((int)acct.SignatureType).ToString(CultureInfo.InvariantCulture);
					signatureElem.Attributes.Append(typeAttr);

					XmlAttribute optAttr = webmailNode.OwnerDocument.CreateAttribute("opt");
					optAttr.Value = ((int)acct.SignatureOptions).ToString(CultureInfo.InvariantCulture);
					signatureElem.Attributes.Append(optAttr);

					signatureElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(acct.Signature)));

					webmailNode.AppendChild(signatureElem);
				}
			}
			else
			{
				if (CurrentAccount == null)
					Log.WriteLine("GetSignature", "Account is null.");
				else if (CurrentAccount.UserOfAccount == null)
					Log.WriteLine("GetSignature", "User is null.");
				throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
			}
		}

		private void UpdateXSpam(XmlElement webmailNode, bool xSpam)
		{
			_actions.UpdateXSpam(xSpam);

			XmlElement xSpamElem = webmailNode.OwnerDocument.CreateElement("x_spam");

			XmlAttribute valueAttr = webmailNode.OwnerDocument.CreateAttribute("value");
			valueAttr.Value = (xSpam) ? "1" : "0";
			xSpamElem.Attributes.Append(valueAttr);

			webmailNode.AppendChild(xSpamElem);
		}

		private void UpdateContactsSettings(XmlElement webmailNode, bool whiteListing, short contactsPerPage)
		{
			if (Validation.CheckIt(Validation.ValidationTask.CPP, contactsPerPage.ToString()))
				{
					_actions.UpdateContactsSettings(whiteListing, short.Parse(Validation.Corrected));
				}
			else
				{
					WebmailResourceManager _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();				
					throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
				}

			XmlElement contactsSettingsElem = webmailNode.OwnerDocument.CreateElement("contacts_settings");

			XmlAttribute whiteListingAttr = webmailNode.OwnerDocument.CreateAttribute("white_listing");
			whiteListingAttr.Value = (whiteListing) ? "1" : "0";
			contactsSettingsElem.Attributes.Append(whiteListingAttr);

			XmlAttribute contactsPerPageAttr = webmailNode.OwnerDocument.CreateAttribute("contacts_per_page");
			contactsPerPageAttr.Value = contactsPerPage.ToString(CultureInfo.InvariantCulture);
			contactsSettingsElem.Attributes.Append(contactsPerPageAttr);

			webmailNode.AppendChild(contactsSettingsElem);
		}

		private void GetXSpam(XmlElement webmailNode)
		{
			bool xspam = _actions.GetXSpam();
			XmlElement xSpamElem = webmailNode.OwnerDocument.CreateElement("x_spam");

			XmlAttribute valueAttr = webmailNode.OwnerDocument.CreateAttribute("value");
			valueAttr.Value = (xspam) ? "1" : "0";
			xSpamElem.Attributes.Append(valueAttr);

			webmailNode.AppendChild(xSpamElem);
		}

		private void GetContactsSettings(XmlElement webmailNode)
		{
			if ((CurrentAccount != null) && (CurrentAccount.UserOfAccount != null) && (CurrentAccount.UserOfAccount.Settings != null))
			{
				XmlElement contactsSettingsElem = webmailNode.OwnerDocument.CreateElement("contacts_settings");

				XmlAttribute whiteListingAttr = webmailNode.OwnerDocument.CreateAttribute("white_listing");
				whiteListingAttr.Value = (CurrentAccount.UserOfAccount.Settings.WhiteListing) ? "1" : "0";
				contactsSettingsElem.Attributes.Append(whiteListingAttr);

				XmlAttribute contactsPerPageAttr = webmailNode.OwnerDocument.CreateAttribute("contacts_per_page");
				contactsPerPageAttr.Value = CurrentAccount.UserOfAccount.Settings.ContactsPerPage.ToString(CultureInfo.InvariantCulture);
				contactsSettingsElem.Attributes.Append(contactsPerPageAttr);

				webmailNode.AppendChild(contactsSettingsElem);
			}
			else
			{
				if (CurrentAccount == null)
					Log.WriteLine("UpdateContactSettings", "Account is null.");
				else if (CurrentAccount.UserOfAccount == null)
					Log.WriteLine("UpdateContactsSettings", "User is null.");
				else if (CurrentAccount.UserOfAccount.Settings == null)
					Log.WriteLine("UpdateContactsSettings", "User Settings is null.");
				throw new WebMailException("User is null.");
			}
		}

		private void DeleteFilter(int id_filter, int id_acct)
		{
			_actions.DeleteFilter(id_filter, id_acct);
		}

		private void UpdateFilter(int id_acct, int id_filter, byte field, byte condition, byte action, long id_folder, string filter)
		{
			if (Validation.CheckIt(Validation.ValidationTask.Substring, filter))
			{
				_actions.UpdateFilter(id_acct, id_filter, field, condition, action, id_folder, Validation.Corrected);
			}
			else
			{
				WebmailResourceManager _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();				
				throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
			}
		}

		private void NewFilter(int id_acct, byte field, byte condition, byte action, long id_folder, string filter)
		{
			if (Validation.CheckIt(Validation.ValidationTask.Substring, filter))
				{
					_actions.NewFilter(id_acct, field, condition, action, id_folder, Validation.Corrected);
				}
			else
				{
					WebmailResourceManager _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();				
					throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
				}
		}

		private void GetFilter(XmlElement webmailElement, int id_filter, int id_acct)
		{
			Filter flt =_actions.GetFilter(id_filter, id_acct);
			if (flt != null)
			{
				XmlElement filterElem = webmailElement.OwnerDocument.CreateElement("filter");

				XmlAttribute idAttr = webmailElement.OwnerDocument.CreateAttribute("id");
				idAttr.Value = flt.IDFilter.ToString(CultureInfo.InvariantCulture);
				filterElem.Attributes.Append(idAttr);

				XmlAttribute fieldAttr = webmailElement.OwnerDocument.CreateAttribute("field");
				fieldAttr.Value = ((int)flt.Field).ToString(CultureInfo.InvariantCulture);
				filterElem.Attributes.Append(fieldAttr);

				XmlAttribute conditionAttr = webmailElement.OwnerDocument.CreateAttribute("condition");
				conditionAttr.Value = ((int)flt.Condition).ToString(CultureInfo.InvariantCulture);
				filterElem.Attributes.Append(conditionAttr);

				XmlAttribute actionAttr = webmailElement.OwnerDocument.CreateAttribute("action");
				actionAttr.Value = ((int)flt.Action).ToString(CultureInfo.InvariantCulture);
				filterElem.Attributes.Append(actionAttr);

				XmlAttribute idFolderAttr = webmailElement.OwnerDocument.CreateAttribute("id_folder");
				idFolderAttr.Value = flt.IDFolder.ToString(CultureInfo.InvariantCulture);
				filterElem.Attributes.Append(idFolderAttr);

				filterElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(flt.FilterStr)));

				webmailElement.AppendChild(filterElem);
			}
		}

		private void GetFilters(XmlElement webmailNode, int id_acct)
		{
			Filter[] filters = _actions.GetFilters(id_acct);
			XmlElement filtersElem = webmailNode.OwnerDocument.CreateElement("filters");
			foreach (Filter flt in filters)
			{
				XmlElement filterElem = webmailNode.OwnerDocument.CreateElement("filter");

				XmlAttribute idAttr = webmailNode.OwnerDocument.CreateAttribute("id");
				idAttr.Value = flt.IDFilter.ToString(CultureInfo.InvariantCulture);
				filterElem.Attributes.Append(idAttr);

				XmlAttribute idAcct = webmailNode.OwnerDocument.CreateAttribute("id_acct");
				idAcct.Value = flt.IDAcct.ToString(CultureInfo.InvariantCulture);
				filterElem.Attributes.Append(idAcct);

				XmlAttribute fieldAttr = webmailNode.OwnerDocument.CreateAttribute("field");
				fieldAttr.Value = ((int)flt.Field).ToString(CultureInfo.InvariantCulture);
				filterElem.Attributes.Append(fieldAttr);

				XmlAttribute conditionAttr = webmailNode.OwnerDocument.CreateAttribute("condition");
				conditionAttr.Value = ((int)flt.Condition).ToString(CultureInfo.InvariantCulture);
				filterElem.Attributes.Append(conditionAttr);

				XmlAttribute actionAttr = webmailNode.OwnerDocument.CreateAttribute("action");
				actionAttr.Value = ((int)flt.Action).ToString(CultureInfo.InvariantCulture);
				filterElem.Attributes.Append(actionAttr);

				XmlAttribute idFolderAttr = webmailNode.OwnerDocument.CreateAttribute("id_folder");
				idFolderAttr.Value = flt.IDFolder.ToString(CultureInfo.InvariantCulture);
				filterElem.Attributes.Append(idFolderAttr);

				filterElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(flt.FilterStr)));

				filtersElem.AppendChild(filterElem);
			}
			webmailNode.AppendChild(filtersElem);
		}

		private void DeleteAccount(int id_acct)
		{
			_actions.DeleteAccount(id_acct);
		}

		private void GetAccount(XmlElement webmailNode, int id_acct)
		{
			Account acct = _actions.GetAccount(id_acct);

			if (acct != null)
			{
				XmlElement accountElem = webmailNode.OwnerDocument.CreateElement("account");

				XmlAttribute idAttr = webmailNode.OwnerDocument.CreateAttribute("id");
				idAttr.Value = acct.ID.ToString(CultureInfo.InvariantCulture);
				accountElem.Attributes.Append(idAttr);

				XmlAttribute defAccountAttr = webmailNode.OwnerDocument.CreateAttribute("def_acct");
				defAccountAttr.Value = (acct.DefaultAccount) ? "1" : "0";
				accountElem.Attributes.Append(defAccountAttr);

				XmlAttribute mailProtocolAttr = webmailNode.OwnerDocument.CreateAttribute("mail_protocol");
				mailProtocolAttr.Value = Convert.ToInt32(acct.MailIncomingProtocol).ToString(CultureInfo.InvariantCulture);
				accountElem.Attributes.Append(mailProtocolAttr);

				XmlAttribute mailIncPortAttr = webmailNode.OwnerDocument.CreateAttribute("mail_inc_port");
				mailIncPortAttr.Value = acct.MailIncomingPort.ToString(CultureInfo.InvariantCulture);
				accountElem.Attributes.Append(mailIncPortAttr);

				XmlAttribute mailOutPortAttr = webmailNode.OwnerDocument.CreateAttribute("mail_out_port");
				mailOutPortAttr.Value = acct.MailOutgoingPort.ToString(CultureInfo.InvariantCulture);
				accountElem.Attributes.Append(mailOutPortAttr);

				XmlAttribute mailOutAuthAttr = webmailNode.OwnerDocument.CreateAttribute("mail_out_auth");
				mailOutAuthAttr.Value = (acct.MailOutgoingAuthentication) ? "1" : "0";
				accountElem.Attributes.Append(mailOutAuthAttr);

				XmlAttribute useFriendlyNameAttr = webmailNode.OwnerDocument.CreateAttribute("use_friendly_nm");
				useFriendlyNameAttr.Value = (acct.UseFriendlyName) ? "1" : "0";
				accountElem.Attributes.Append(useFriendlyNameAttr);

				XmlAttribute mailsOnServerDaysAttr = webmailNode.OwnerDocument.CreateAttribute("mails_on_server_days");
				mailsOnServerDaysAttr.Value = acct.MailsOnServerDays.ToString(CultureInfo.InvariantCulture);
				accountElem.Attributes.Append(mailsOnServerDaysAttr);

				int mail_mode = 1;
				if (acct.MailMode == MailMode.DeleteMessagesFromServer) mail_mode = 0;
				if (acct.MailMode == MailMode.LeaveMessagesOnServer) mail_mode = 1;
				if (acct.MailMode == MailMode.KeepMessagesOnServer) mail_mode = 2;
				if (acct.MailMode == MailMode.DeleteMessageWhenItsRemovedFromTrash) mail_mode = 3;
				if (acct.MailMode == MailMode.KeepMessagesOnServerAndDeleteMessageWhenItsRemovedFromTrash) mail_mode = 4;
				XmlAttribute mailModeAttr = webmailNode.OwnerDocument.CreateAttribute("mail_mode");
				mailModeAttr.Value = mail_mode.ToString(CultureInfo.InvariantCulture);
				accountElem.Attributes.Append(mailModeAttr);

				XmlAttribute getMailAtLoginAttr = webmailNode.OwnerDocument.CreateAttribute("getmail_at_login");
				getMailAtLoginAttr.Value = (acct.GetMailAtLogin) ? "1" : "0";
				accountElem.Attributes.Append(getMailAtLoginAttr);

				XmlAttribute inboxSyncTypeAttr = webmailNode.OwnerDocument.CreateAttribute("inbox_sync_type");
				FolderSyncType inboxSyncType = FolderSyncType.NewHeadersOnly;
				MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(acct));
				try
				{
					mp.Connect();
					Folder fld = mp.GetFolder(FolderType.Inbox);
					if (fld != null) inboxSyncType = fld.SyncType;
				}
				finally
				{
					mp.Disconnect();
				}
				inboxSyncTypeAttr.Value = ((short)inboxSyncType).ToString(CultureInfo.InvariantCulture);
				accountElem.Attributes.Append(inboxSyncTypeAttr);

				XmlElement friendlyNameElem = webmailNode.OwnerDocument.CreateElement("friendly_name");
				friendlyNameElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(acct.FriendlyName)));
				accountElem.AppendChild(friendlyNameElem);

				XmlElement emailElem = webmailNode.OwnerDocument.CreateElement("email");
				emailElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(acct.Email)));
				accountElem.AppendChild(emailElem);

				XmlElement mailIncHostElem = webmailNode.OwnerDocument.CreateElement("mail_inc_host");
				mailIncHostElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(acct.MailIncomingHost)));
				accountElem.AppendChild(mailIncHostElem);

				XmlElement mailIncLoginElem = webmailNode.OwnerDocument.CreateElement("mail_inc_login");
				mailIncLoginElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(acct.MailIncomingLogin)));
				accountElem.AppendChild(mailIncLoginElem);

				XmlElement mailIncPassElem = webmailNode.OwnerDocument.CreateElement("mail_inc_pass");
				mailIncPassElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(Constants.nonChangedPassword)));
				accountElem.AppendChild(mailIncPassElem);

				XmlElement mailOutHostElem = webmailNode.OwnerDocument.CreateElement("mail_out_host");
				mailOutHostElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(acct.MailOutgoingHost)));
				accountElem.AppendChild(mailOutHostElem);

				XmlElement mailOutLoginElem = webmailNode.OwnerDocument.CreateElement("mail_out_login");
				mailOutLoginElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(acct.MailOutgoingLogin)));
				accountElem.AppendChild(mailOutLoginElem);

				string smtpPassw = (acct.MailOutgoingPassword.Length > 0) ? Constants.nonChangedPassword : string.Empty;
				XmlElement mailOutPassElem = webmailNode.OwnerDocument.CreateElement("mail_out_pass");
				mailOutPassElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(smtpPassw)));
				accountElem.AppendChild(mailOutPassElem);

				webmailNode.AppendChild(accountElem);
			}
		}

		private int NewAccount(bool def_acct, IncomingMailProtocol mail_protocol, int mail_inc_port, int mail_out_port, bool mail_out_auth, bool use_friendly_name, short mails_on_server_days, MailMode mail_mode, bool getmail_at_login, int inbox_sync_type, string friendly_nm, string email, string mail_inc_host, string mail_inc_login, string mail_inc_pass, string mail_out_host, string mail_out_login, string mail_out_pass)
		{
			return _actions.NewAccount(def_acct, mail_protocol, mail_inc_port, mail_out_port, mail_out_auth, use_friendly_name, mails_on_server_days, mail_mode, getmail_at_login, inbox_sync_type, friendly_nm, email, mail_inc_host, mail_inc_login, mail_inc_pass, mail_out_host, mail_out_login, mail_out_pass);
		}

		private void UpdateAccount(XmlElement webmailElement, int id_acct, bool def_acct, IncomingMailProtocol mail_protocol, int mail_inc_port, int mail_out_port, bool mail_out_auth, bool use_friendly_name, short mails_on_server_days, MailMode mail_mode, bool getmail_at_login, int inbox_sync_type, string friendly_nm, string email, string mail_inc_host, string mail_inc_login, string mail_inc_pass, string mail_out_host, string mail_out_login, string mail_out_pass)
		{
			WebmailResourceManager _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();				

			if (Validation.CheckIt(Validation.ValidationTask.Email, email))
				email = Validation.Corrected;
			else
				throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
			if (Validation.CheckIt(Validation.ValidationTask.Login, mail_inc_login))
				mail_inc_login = Validation.Corrected;
			else
				throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
			if (Validation.CheckIt(Validation.ValidationTask.Password, mail_inc_pass))
				mail_inc_pass = Validation.Corrected;
			else
				throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
			if (Validation.CheckIt(Validation.ValidationTask.INServer, mail_inc_host))
				mail_inc_host = Validation.Corrected;
			else
				throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
			if (Validation.CheckIt(Validation.ValidationTask.INPort, mail_inc_port.ToString()))
				mail_inc_port = int.Parse(Validation.Corrected);
			else
				throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
			if (Validation.CheckIt(Validation.ValidationTask.OUTServer, mail_out_host))
				mail_out_host = Validation.Corrected;
			else
				throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
			if (Validation.CheckIt(Validation.ValidationTask.OUTPort, mail_out_port.ToString()))
				mail_out_port = int.Parse(Validation.Corrected);
			else
				throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
			if (Validation.CheckIt(Validation.ValidationTask.KeepMessages, mails_on_server_days.ToString()))
				mails_on_server_days = short.Parse(Validation.Corrected);
			else
				throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));

			_actions.UpdateAccount(id_acct, def_acct, mail_protocol, mail_inc_port, mail_out_port, mail_out_auth, use_friendly_name, mails_on_server_days, mail_mode, getmail_at_login, inbox_sync_type, friendly_nm, email, mail_inc_host, mail_inc_login, mail_inc_pass, mail_out_host, mail_out_login, mail_out_pass);

			XmlElement updateElem = webmailElement.OwnerDocument.CreateElement("update");
			XmlAttribute valueAttr = webmailElement.OwnerDocument.CreateAttribute("value");
			valueAttr.Value = "account";
			updateElem.Attributes.Append(valueAttr);

			webmailElement.AppendChild(updateElem);
		}

		private void UpdateCookieSettings(XmlElement webmailElem, bool hide_folders, short horiz_resizer, short vert_resizer, byte mark, byte reply, UserColumn[] columns)
		{
			_actions.UpdateCookieSettings(hide_folders, horiz_resizer, vert_resizer, mark, reply, columns);

			XmlElement updateElem = webmailElem.OwnerDocument.CreateElement("update");

			XmlAttribute valueAttr = webmailElem.OwnerDocument.CreateAttribute("value");
			valueAttr.Value = "cookie_settings";
			updateElem.Attributes.Append(valueAttr);

			webmailElem.AppendChild(updateElem);
		}

		private void UpdateSettings(XmlElement webmailNode, XmlPacketSettings settings)
		{
			WebmailResourceManager _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();				

			if (Validation.CheckIt(Validation.ValidationTask.MPP, settings.msgs_per_page.ToString()))
			{
				settings.msgs_per_page = short.Parse(Validation.Corrected);
			}
			else
			{
				throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
			}
			if (Validation.CheckIt(Validation.ValidationTask.Advanced, settings.def_date_fmt))
			{
				settings.def_date_fmt = Validation.Corrected;
			}
			else
			{
				throw (new WebMailException(_resMan.GetString(Validation.ErrorMessage)));
			}

            _actions.UpdateSettings(settings.allow_dhtml_editor, settings.def_charset_inc, settings.def_charset_out, settings.def_date_fmt, settings.def_lang, settings.def_skin, settings.def_timezone, settings.msgs_per_page, settings.view_mode, settings.def_time_fmt);

			XmlElement updateElem = webmailNode.OwnerDocument.CreateElement("update");
			XmlAttribute valueAttr = webmailNode.OwnerDocument.CreateAttribute("value");
			valueAttr.Value = "settings";
			updateElem.Attributes.Append(valueAttr);

			webmailNode.AppendChild(updateElem);
		}

		private void GetSettings(XmlElement webmailNode)
		{
			if ((CurrentAccount != null) && (CurrentAccount.UserOfAccount != null) && (CurrentAccount.UserOfAccount.Settings != null))
			{
				XmlElement settingsElem = webmailNode.OwnerDocument.CreateElement("settings");

				XmlAttribute msgsPerPageAttr = webmailNode.OwnerDocument.CreateAttribute("msgs_per_page");
				msgsPerPageAttr.Value = CurrentAccount.UserOfAccount.Settings.MsgsPerPage.ToString(CultureInfo.InvariantCulture);
				settingsElem.Attributes.Append(msgsPerPageAttr);

				XmlAttribute allowDhtmlEditorAttr = webmailNode.OwnerDocument.CreateAttribute("allow_dhtml_editor");
				allowDhtmlEditorAttr.Value = (CurrentAccount.UserOfAccount.Settings.AllowDhtmlEditor) ? "1" : "0";
				settingsElem.Attributes.Append(allowDhtmlEditorAttr);

				WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

				if (settings.AllowUsersChangeSkin)
				{
					XmlElement skinsElem = webmailNode.OwnerDocument.CreateElement("skins");
					string defaultSkin = CurrentAccount.UserOfAccount.Settings.DefaultSkin;
					string[] supportedSkins = Utils.GetSupportedSkins((CurrentPage != null) ? CurrentPage.MapPath("skins") : string.Empty);
					if (Utils.GetCurrentSkinIndex(supportedSkins, defaultSkin) < 0)
					{
						if (supportedSkins.Length > 0)
						{
							defaultSkin = supportedSkins[0];
						}
					}
					foreach (string skin in supportedSkins)
					{
						XmlElement skinElem = webmailNode.OwnerDocument.CreateElement("skin");
						XmlAttribute defAttr = webmailNode.OwnerDocument.CreateAttribute("def");
						defAttr.Value = (string.Compare(defaultSkin, skin, true, CultureInfo.InvariantCulture) == 0) ? "1" : "0";
						skinElem.Attributes.Append(defAttr);
						skinElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(skin)));

						skinsElem.AppendChild(skinElem);
					}
					settingsElem.AppendChild(skinsElem);
				}

				if (settings.AllowUsersChangeCharset)
				{
					XmlAttribute defCharsetIncAttr = webmailNode.OwnerDocument.CreateAttribute("def_charset_inc");
					defCharsetIncAttr.Value = CurrentAccount.UserOfAccount.Settings.DefaultCharsetInc.ToString(CultureInfo.InvariantCulture);
					settingsElem.Attributes.Append(defCharsetIncAttr);

					XmlAttribute defCharsetOutAttr = webmailNode.OwnerDocument.CreateAttribute("def_charset_out");
					defCharsetOutAttr.Value = CurrentAccount.UserOfAccount.Settings.DefaultCharsetOut.ToString(CultureInfo.InvariantCulture);
					settingsElem.Attributes.Append(defCharsetOutAttr);
				}

				if (settings.AllowUsersChangeTimeZone)
				{
					XmlAttribute defTimeOffsetAttr = webmailNode.OwnerDocument.CreateAttribute("def_timezone");
					defTimeOffsetAttr.Value = CurrentAccount.UserOfAccount.Settings.DefaultTimeZone.ToString(CultureInfo.InvariantCulture);
					settingsElem.Attributes.Append(defTimeOffsetAttr);
				}

				XmlAttribute viewModeAttr = webmailNode.OwnerDocument.CreateAttribute("view_mode");
				viewModeAttr.Value = ((int)CurrentAccount.UserOfAccount.Settings.ViewMode).ToString(CultureInfo.InvariantCulture);
				settingsElem.Attributes.Append(viewModeAttr);

                XmlAttribute timeFormatAttr = webmailNode.OwnerDocument.CreateAttribute("time_format");
                timeFormatAttr.Value = ((byte)CurrentAccount.UserOfAccount.Settings.DefaultTimeFormat).ToString(CultureInfo.InvariantCulture);
                settingsElem.Attributes.Append(timeFormatAttr);

				if (settings.AllowUsersChangeLanguage)
				{
					XmlElement langsElem = webmailNode.OwnerDocument.CreateElement("langs");

					string defaultLang = CurrentAccount.UserOfAccount.Settings.DefaultLanguage;
					string[] supportedLangs = Utils.GetSupportedLangs();
					foreach (string lang in supportedLangs)
					{
						XmlElement langElem = webmailNode.OwnerDocument.CreateElement("lang");
						XmlAttribute defAttr = webmailNode.OwnerDocument.CreateAttribute("def");
						defAttr.Value = (string.Compare(defaultLang, lang, true, CultureInfo.InvariantCulture) == 0) ? "1" : "0";
						langElem.Attributes.Append(defAttr);
						langElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(lang)));

						langsElem.AppendChild(langElem);
					}
					settingsElem.AppendChild(langsElem);
				}

				XmlElement defDateFmtElem = webmailNode.OwnerDocument.CreateElement("def_date_fmt");
				defDateFmtElem.AppendChild(webmailNode.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(CurrentAccount.UserOfAccount.Settings.DefaultDateFormat)));
				settingsElem.AppendChild(defDateFmtElem);

				webmailNode.AppendChild(settingsElem);
			}
			else
			{
				if (CurrentAccount == null)
					Log.WriteLine("GetSettings", "Account is null.");
				else if (CurrentAccount.UserOfAccount == null)
					Log.WriteLine("GetSettings", "User is null.");
				else if (CurrentAccount.UserOfAccount.Settings == null)
					Log.WriteLine("GetSettings", "User Settings is null.");
				throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
			}
		}

		private void GetAccounts(XmlElement webmailElement, int user_id, int last_id)
		{
			XmlNode accountsNode = webmailElement.OwnerDocument.CreateElement("accounts");
			
			XmlAttribute attributeNode = webmailElement.OwnerDocument.CreateAttribute("last_id");
			attributeNode.Value = last_id.ToString(CultureInfo.InvariantCulture);
			accountsNode.Attributes.Append(attributeNode);
			
			attributeNode = webmailElement.OwnerDocument.CreateAttribute("curr_id");
			attributeNode.Value = (CurrentAccount != null) ? CurrentAccount.ID.ToString(CultureInfo.InvariantCulture) : "0";
			accountsNode.Attributes.Append(attributeNode);

			Account[] accounts = _actions.GetAccounts(user_id);
			if (accounts != null)
			{
				foreach (Account acct in accounts)
				{
					XmlElement accountNode = webmailElement.OwnerDocument.CreateElement("account");

					attributeNode = webmailElement.OwnerDocument.CreateAttribute("id");
					attributeNode.Value = acct.ID.ToString(CultureInfo.InvariantCulture);
					accountNode.Attributes.Append(attributeNode);

					attributeNode = webmailElement.OwnerDocument.CreateAttribute("mail_protocol");
					attributeNode.Value = Convert.ToInt32(acct.MailIncomingProtocol).ToString(CultureInfo.InvariantCulture);
					accountNode.Attributes.Append(attributeNode);

					attributeNode = webmailElement.OwnerDocument.CreateAttribute("def_order");
					attributeNode.Value = ((byte)acct.DefaultOrder).ToString(CultureInfo.InvariantCulture);
					accountNode.Attributes.Append(attributeNode);

					attributeNode = webmailElement.OwnerDocument.CreateAttribute("use_friendly_nm");
					attributeNode.Value = (acct.UseFriendlyName) ? "1" : "0";
					accountNode.Attributes.Append(attributeNode);

					attributeNode = webmailElement.OwnerDocument.CreateAttribute("def_acct");
					attributeNode.Value = (acct.DefaultAccount) ? "1" : "0";
					accountNode.Attributes.Append(attributeNode);

					XmlElement friendlyNameElem = webmailElement.OwnerDocument.CreateElement("friendly_name");
					friendlyNameElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(acct.FriendlyName)));
					accountNode.AppendChild(friendlyNameElem);

					XmlElement emailElem = webmailElement.OwnerDocument.CreateElement("email");
					emailElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(acct.Email)));
					accountNode.AppendChild(emailElem);

					accountsNode.AppendChild(accountNode);
				}
			}
			webmailElement.AppendChild(accountsNode);
		}

		private void GetFoldersList(XmlElement webmailElement, int id_acct, int sync)
		{
			//_actions.GetFoldersList(id_acct, sync);
			Account acct = null;
			if ((CurrentAccount != null) && (CurrentAccount.UserOfAccount != null))
			{
				acct = Account.LoadFromDb(id_acct, CurrentAccount.UserOfAccount.ID);
			}
			else
			{
				if (CurrentAccount == null)
					Log.WriteLine("GetFoldersList", "Account is null.");
				else if (CurrentAccount.UserOfAccount == null)
					Log.WriteLine("GetFoldersList", "User is null.");
				throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
			}
			if (acct != null)
			{
				if (sync != -1)
				{
					if (CurrentAccount.UserOfAccount != null) _actions = new BaseWebMailActions(acct, CurrentPage); /*CurrentAccount = acct*/;
				}

				XmlNode foldersListNode = webmailElement.OwnerDocument.CreateElement("folders_list");
				webmailElement.AppendChild(foldersListNode);

				XmlAttribute attributeNode = webmailElement.OwnerDocument.CreateAttribute("sync");
				attributeNode.Value = sync.ToString();
				foldersListNode.Attributes.Append(attributeNode);

				attributeNode = webmailElement.OwnerDocument.CreateAttribute("id_acct");
				attributeNode.Value = acct.ID.ToString(CultureInfo.InvariantCulture);
				foldersListNode.Attributes.Append(attributeNode);

				FolderCollection fc = new FolderCollection();
				MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(acct));
				try
				{
					mp.Connect();
					fc = mp.GetFolders();
					switch (sync)
					{
						case 2:
							mp.SynchronizeFolders();
							fc = mp.GetFolders();
							break;
					}
					FolderCollection.SortTree(fc);
				}
				finally
				{
					mp.Disconnect();
				}
                FolderTreeToXml(mp, webmailElement.OwnerDocument, foldersListNode, fc);
			}
		}
		
		private void FolderTreeToXml(MailProcessor mp, XmlDocument doc, XmlNode node, FolderCollection folderTree)
		{
			foreach (Folder fld in folderTree)
			{
				XmlElement childNode = doc.CreateElement("folder");

				XmlAttribute attrNode = doc.CreateAttribute("id");
				attrNode.Value = fld.ID.ToString(CultureInfo.InvariantCulture);
				childNode.Attributes.Append(attrNode);

				attrNode = doc.CreateAttribute("id_parent");
				attrNode.Value = fld.IDParent.ToString(CultureInfo.InvariantCulture);
				childNode.Attributes.Append(attrNode);

				attrNode = doc.CreateAttribute("type");
				attrNode.Value = ((int)fld.Type).ToString(CultureInfo.InvariantCulture);
				childNode.Attributes.Append(attrNode);

				attrNode = doc.CreateAttribute("sync_type");
				attrNode.Value = ((int)fld.SyncType).ToString(CultureInfo.InvariantCulture);
				childNode.Attributes.Append(attrNode);

				attrNode = doc.CreateAttribute("hide");
				attrNode.Value = (fld.Hide) ? "1" : "0";
				childNode.Attributes.Append(attrNode);

				attrNode = doc.CreateAttribute("fld_order");
				attrNode.Value = fld.FolderOrder.ToString(CultureInfo.InvariantCulture);
				childNode.Attributes.Append(attrNode);

				attrNode = doc.CreateAttribute("count");
				Log.WriteLine("FolderTreeToXml", "Before Folder Message Count");
				attrNode.Value = mp.GetFolderMessageCount(fld).ToString(CultureInfo.InvariantCulture);
				Log.WriteLine("FolderTreeToXml", "After Folder Message Count");
				childNode.Attributes.Append(attrNode);

				attrNode = doc.CreateAttribute("count_new");
				attrNode.Value = mp.GetFolderUnreadMessageCount(fld).ToString(CultureInfo.InvariantCulture);
				childNode.Attributes.Append(attrNode);

				attrNode = doc.CreateAttribute("size");
				attrNode.Value = fld.Size.ToString(CultureInfo.InvariantCulture);
				childNode.Attributes.Append(attrNode);

				XmlElement nameElement = doc.CreateElement("name");
				nameElement.AppendChild(doc.CreateCDataSection(Utils.EncodeHtml(fld.Name)));
				childNode.AppendChild(nameElement);

				XmlElement fullNameElement = doc.CreateElement("full_name");
				fullNameElement.AppendChild(doc.CreateCDataSection(Utils.EncodeHtml(fld.FullPath)));
				childNode.AppendChild(fullNameElement);

				if ((fld.SubFolders != null) && (fld.SubFolders.Count > 0))
				{
					XmlNode foldersElem = doc.CreateElement("folders");
					childNode.AppendChild(foldersElem);
					FolderTreeToXml(mp, doc, foldersElem, fld.SubFolders);
				}

				node.AppendChild(childNode);
			}
		}

		private void GetMessages(XmlElement webmailElement, long id_folder, string full_name_folder, int page, int sort_field, int sort_order, string look_for, int search_fields)
		{
			int folderMessageCount = 0;
			int folderUnreadMessageCount = 0;
			WebMailMessageCollection msgsColl =_actions.GetMessages(id_folder, full_name_folder, page, sort_field, sort_order, look_for, search_fields, out folderMessageCount, out folderUnreadMessageCount);

			XmlNode messagesElem = webmailElement.OwnerDocument.CreateElement("messages");

			XmlAttribute pageAttr = webmailElement.OwnerDocument.CreateAttribute("page");
			pageAttr.Value = page.ToString();
			messagesElem.Attributes.Append(pageAttr);

			XmlAttribute sortFieldAttr = webmailElement.OwnerDocument.CreateAttribute("sort_field");
			sortFieldAttr.Value = sort_field.ToString();
			messagesElem.Attributes.Append(sortFieldAttr);

			XmlAttribute sortOrderAttr = webmailElement.OwnerDocument.CreateAttribute("sort_order");
			sortOrderAttr.Value = sort_order.ToString();
			messagesElem.Attributes.Append(sortOrderAttr);

			XmlAttribute countAttr = webmailElement.OwnerDocument.CreateAttribute("count");
			countAttr.Value = folderMessageCount.ToString();
			messagesElem.Attributes.Append(countAttr);

			XmlAttribute countNewAttr = webmailElement.OwnerDocument.CreateAttribute("count_new");
			countNewAttr.Value = folderUnreadMessageCount.ToString(CultureInfo.InvariantCulture);
			messagesElem.Attributes.Append(countNewAttr);

			XmlElement folderElem = webmailElement.OwnerDocument.CreateElement("folder");
			XmlAttribute idAttr = webmailElement.OwnerDocument.CreateAttribute("id");
			idAttr.Value = id_folder.ToString(CultureInfo.InvariantCulture);
			folderElem.Attributes.Append(idAttr);
			folderElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(full_name_folder)));
			messagesElem.AppendChild(folderElem);


			XmlElement lookForElem = webmailElement.OwnerDocument.CreateElement("look_for");
			XmlAttribute fieldsAttr = webmailElement.OwnerDocument.CreateAttribute("fields");
			fieldsAttr.Value = search_fields.ToString(CultureInfo.InvariantCulture);
			lookForElem.Attributes.Append(fieldsAttr);
			lookForElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(look_for)));
			messagesElem.AppendChild(lookForElem);

			if (msgsColl != null)
			{
				foreach (WebMailMessage msg in msgsColl)
				{
					XmlNode messageElem = webmailElement.OwnerDocument.CreateElement("message");

					idAttr = webmailElement.OwnerDocument.CreateAttribute("id");
					idAttr.Value = msg.IDMsg.ToString(CultureInfo.InvariantCulture);
					messageElem.Attributes.Append(idAttr);

					XmlAttribute hasAttachmentsAttr = webmailElement.OwnerDocument.CreateAttribute("has_attachments");
					hasAttachmentsAttr.Value = (msg.Attachments) ? "1" : "0";
					messageElem.Attributes.Append(hasAttachmentsAttr);

					XmlAttribute priorityAttr = webmailElement.OwnerDocument.CreateAttribute("priority");
					int priority = 3;
					switch (msg.Priority)
					{
						case MailPriority.High:
						case MailPriority.Highest:
							priority = 1;
							break;
						case MailPriority.Low:
						case MailPriority.Lowest:
							priority = 5;
							break;
						case MailPriority.Normal:
							priority = 3;
							break;
					}
					priorityAttr.Value = priority.ToString(CultureInfo.InvariantCulture);
					messageElem.Attributes.Append(priorityAttr);

					XmlAttribute sizeAttr = webmailElement.OwnerDocument.CreateAttribute("size");
					sizeAttr.Value = msg.Size.ToString(CultureInfo.InvariantCulture);
					messageElem.Attributes.Append(sizeAttr);

					int flags = 0;
					if (msg.Seen) flags |= 1;
					if (msg.Replied) flags |= 2;
					if (msg.Flagged) flags |= 4;
					if (msg.Deleted) flags |= 8;
					if (msg.Forwarded) flags |= 256;
					if (msg.Grayed) flags |= 512;
					XmlAttribute flagsAttr = webmailElement.OwnerDocument.CreateAttribute("flags");
					flagsAttr.Value = flags.ToString(CultureInfo.InvariantCulture);
					messageElem.Attributes.Append(flagsAttr);

					XmlAttribute charsetAttr = webmailElement.OwnerDocument.CreateAttribute("charset");
					charsetAttr.Value = msg.OverrideCharset.ToString(CultureInfo.InvariantCulture);
					messageElem.Attributes.Append(charsetAttr);

					folderElem = webmailElement.OwnerDocument.CreateElement("folder");
					idAttr = webmailElement.OwnerDocument.CreateAttribute("id");
					idAttr.Value = msg.IDFolderDB.ToString();
					folderElem.Attributes.Append(idAttr);
					folderElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(msg.FolderFullName)));
					messageElem.AppendChild(folderElem);

					string from = ((msg.FromMsg.DisplayName != null) && (msg.FromMsg.DisplayName.Length > 0)) ? msg.FromMsg.DisplayName : msg.FromMsg.ToString();
					XmlElement fromElem = webmailElement.OwnerDocument.CreateElement("from");
					fromElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(from)));
					messageElem.AppendChild(fromElem);

					XmlElement toElem = webmailElement.OwnerDocument.CreateElement("to");
					toElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(Utils.GetAddressesFriendlyName(msg.ToMsg))));
					messageElem.AppendChild(toElem);

					EmailAddressCollection replyTo = (msg.MailBeeMessage != null) ? msg.MailBeeMessage.ReplyTo : new EmailAddressCollection(string.Empty);
					XmlElement replyToElem = webmailElement.OwnerDocument.CreateElement("reply_to");
					toElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(Utils.GetAddressesFriendlyName(replyTo))));
					messageElem.AppendChild(replyToElem);

					XmlElement ccElem = webmailElement.OwnerDocument.CreateElement("cc");
					toElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(Utils.GetAddressesFriendlyName(msg.CcMsg))));
					messageElem.AppendChild(ccElem);

					XmlElement bccElem = webmailElement.OwnerDocument.CreateElement("bcc");
					toElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(Utils.GetAddressesFriendlyName(msg.BccMsg))));
					messageElem.AppendChild(bccElem);

					XmlElement subjectElem = webmailElement.OwnerDocument.CreateElement("subject");
					subjectElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(msg.Subject.ToString())));
					messageElem.AppendChild(subjectElem);

                    XmlElement loanapplicantsElem = webmailElement.OwnerDocument.CreateElement("loanapplicants");
                    loanapplicantsElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(msg.LoanApplicants.ToString())));
                    messageElem.AppendChild(loanapplicantsElem);

                    XmlElement loanappidsElem = webmailElement.OwnerDocument.CreateElement("loanappids");
                    loanappidsElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(msg.LoanAppIDs.ToString())));
                    messageElem.AppendChild(loanappidsElem);

					XmlElement dateElem = webmailElement.OwnerDocument.CreateElement("date");
					dateElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(Utils.GetDateWithOffsetFormatString(CurrentAccount, msg.MsgDate))));
					messageElem.AppendChild(dateElem);

					string uid = ((msg.StrUid != null)&&(msg.StrUid.Length > 0)) ? msg.StrUid : msg.IntUid.ToString(CultureInfo.InvariantCulture);
					XmlElement uidElem = webmailElement.OwnerDocument.CreateElement("uid");
					uidElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(uid)));
					messageElem.AppendChild(uidElem);

					messagesElem.AppendChild(messageElem);
				}
			}
			webmailElement.AppendChild(messagesElem);
		}

		private void GetMessage(XmlElement webmailElement, int id, string uid, long id_folder, string full_name_folder, MessageMode mode, int charset)
		{
			byte safety = 0;
			bool showPicturesSettings = ((CurrentAccount.UserOfAccount.Settings.ViewMode & ViewMode.AlwaysShowPictures) > 0) ? true : false;
			if (showPicturesSettings) safety = 1;
			WebMailMessage msg = _actions.GetMessage(id, uid, id_folder, full_name_folder, charset, safety);
			MailMessage outputMsg = (msg != null) ? msg.MailBeeMessage : null;

			if ((msg != null) && (outputMsg != null))
			{
				XmlNode messageElem = webmailElement.OwnerDocument.CreateElement("message");

				XmlAttribute attrNode = webmailElement.OwnerDocument.CreateAttribute("id");
				attrNode.Value = id.ToString();
				messageElem.Attributes.Append(attrNode);

				XmlAttribute safetyAttr = webmailElement.OwnerDocument.CreateAttribute("safety");
				safetyAttr.Value = msg.Safety.ToString(CultureInfo.InvariantCulture);
				messageElem.Attributes.Append(safetyAttr);

				XmlElement uidElem = webmailElement.OwnerDocument.CreateElement("uid");
				uidElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(uid.ToString(CultureInfo.InvariantCulture))));
				messageElem.AppendChild(uidElem);

				XmlElement folderElem = webmailElement.OwnerDocument.CreateElement("folder");
				attrNode = webmailElement.OwnerDocument.CreateAttribute("id");
				attrNode.Value = id_folder.ToString();
				folderElem.Attributes.Append(attrNode);
				folderElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(full_name_folder)));
				messageElem.AppendChild(folderElem);

				bool hasHtml = false;
				if (outputMsg != null)
				{
					hasHtml = ((outputMsg.BodyHtmlText != null) && (outputMsg.BodyHtmlText.Length > 0)) ? true : false;
				}
				attrNode = webmailElement.OwnerDocument.CreateAttribute("html");
				attrNode.Value = (hasHtml) ? "1" : "0";
				messageElem.Attributes.Append(attrNode);

				bool hasPlain = false;
				if (outputMsg != null)
				{
					hasPlain = ((outputMsg.BodyPlainText != null) && (outputMsg.BodyPlainText.Length > 0)) ? true : false;
				}
				attrNode = webmailElement.OwnerDocument.CreateAttribute("plain");
				attrNode.Value = (hasPlain) ? "1" : "0";
				messageElem.Attributes.Append(attrNode);

				bool hasCharset = false;
				if (outputMsg != null)
				{
					if (outputMsg.Charset.Length != 0) hasCharset = true;
				}
				attrNode = webmailElement.OwnerDocument.CreateAttribute("has_charset");
				attrNode.Value = (hasCharset) ? "1" : "0";
				messageElem.Attributes.Append(attrNode);

				int priority = 3;
				switch (msg.Priority)
				{
					case MailPriority.High:
					case MailPriority.Highest:
						priority = 1;
						break;
					case MailPriority.Low:
					case MailPriority.Lowest:
						priority = 5;
						break;
					case MailPriority.Normal:
						priority = 3;
						break;
				}
				attrNode = webmailElement.OwnerDocument.CreateAttribute("priority");
				attrNode.Value = priority.ToString(CultureInfo.InvariantCulture);
				messageElem.Attributes.Append(attrNode);

				//MessageMode newMode = MessageMode.None;
                MessageMode newMode = mode;
				if ((mode & MessageMode.Headers) > 0)
				{
					newMode |= MessageMode.Headers;
					XmlNode headersNode = webmailElement.OwnerDocument.CreateElement("headers");

					XmlNode fromNode = webmailElement.OwnerDocument.CreateElement("from");
					fromNode.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(outputMsg.From.ToString())));
					headersNode.AppendChild(fromNode);

					XmlNode toNode = webmailElement.OwnerDocument.CreateElement("to");
					toNode.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(outputMsg.To.ToString())));
					headersNode.AppendChild(toNode);

					XmlNode ccNode = webmailElement.OwnerDocument.CreateElement("cc");
					ccNode.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(outputMsg.Cc.ToString())));
					headersNode.AppendChild(ccNode);

					XmlNode bccNode = webmailElement.OwnerDocument.CreateElement("bcc");
					bccNode.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(outputMsg.Bcc.ToString())));
					headersNode.AppendChild(bccNode);

					XmlNode replyToNode = webmailElement.OwnerDocument.CreateElement("reply_to");
					replyToNode.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(outputMsg.ReplyTo.ToString())));
					headersNode.AppendChild(replyToNode);

					XmlNode subjectNode = webmailElement.OwnerDocument.CreateElement("subject");
					subjectNode.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(outputMsg.Subject)));
					headersNode.AppendChild(subjectNode);

                    XmlNode loanapplicantsNode = webmailElement.OwnerDocument.CreateElement("loanapplicants");
                    loanapplicantsNode.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(msg.LoanApplicants)));
                    headersNode.AppendChild(loanapplicantsNode);

                    XmlNode loanappidsNode = webmailElement.OwnerDocument.CreateElement("loanappids");
                    loanappidsNode.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(msg.LoanAppIDs)));
                    headersNode.AppendChild(loanappidsNode);

					XmlNode dateNode = webmailElement.OwnerDocument.CreateElement("date");
					DateTime dt = Constants.MinDate;
					try
					{
						dt = (outputMsg.DateReceived != DateTime.MinValue) ? outputMsg.DateReceived : outputMsg.Date;
					}
					catch (MailBeeDateParsingException)
					{
						dt = DateTime.Now;
					}
					dateNode.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(Utils.GetDateWithOffsetFormatString(CurrentAccount, dt))));
					headersNode.AppendChild(dateNode);

					messageElem.AppendChild(headersNode);
				}

				if ((mode & MessageMode.HtmlBody) > 0)
				{
					if (outputMsg != null)
					{
						if ((outputMsg.BodyHtmlText != null) && (outputMsg.BodyHtmlText.Length > 0))
						{
							newMode |= MessageMode.HtmlBody;
							XmlNode htmlPartNode = webmailElement.OwnerDocument.CreateElement("html_part");
							htmlPartNode.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeCDATABody(outputMsg.BodyHtmlText)));
							messageElem.AppendChild(htmlPartNode);
						}
						else
						{
							newMode |= MessageMode.PlainBody; // if we haven't HTML, return Plain
						}
					}
				}

				if (((mode & MessageMode.PlainBody) > 0) || ((newMode & MessageMode.PlainBody) > 0))
				{
                    string modifiedPlainText = Utils.MakeHtmlBodyFromPlainBody(outputMsg.BodyPlainText);
                    newMode |= MessageMode.PlainBody;
                    XmlNode modifiedPlainTextNode = webmailElement.OwnerDocument.CreateElement("modified_plain_text");
                    modifiedPlainTextNode.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeCDATABody(modifiedPlainText)));
					messageElem.AppendChild(modifiedPlainTextNode);
				}

				if ((mode & MessageMode.PlainBodyUnmodified) > 0)
				{
					newMode |= MessageMode.PlainBodyUnmodified;
					XmlNode unmodifiedPlainTextNode = webmailElement.OwnerDocument.CreateElement("unmodified_plain_text");
					unmodifiedPlainTextNode.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeCDATABody(outputMsg.BodyPlainText)));
					messageElem.AppendChild(unmodifiedPlainTextNode);
				}

				if (((mode & MessageMode.HtmlReply) > 0) || ((mode & MessageMode.HtmlForward) > 0))
				{
					if (outputMsg != null)
					{
						string mess = Utils.GetMessageHtmlReplyToBody(CurrentAccount, outputMsg);
						if ((mode & MessageMode.HtmlReply) > 0)
						{
							newMode |= MessageMode.HtmlReply;
							XmlElement replyHtmlElem = webmailElement.OwnerDocument.CreateElement("reply_html");
							replyHtmlElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeCDATABody(mess)));
							messageElem.AppendChild(replyHtmlElem);
						}
						if ((mode & MessageMode.HtmlForward) > 0)
						{
							newMode |= MessageMode.HtmlForward;
							XmlElement forwardHtmlElem = webmailElement.OwnerDocument.CreateElement("forward_html");
							forwardHtmlElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeCDATABody(mess)));
							messageElem.AppendChild(forwardHtmlElem);
						}
					}
				}

				if (((mode & MessageMode.PlainReply) > 0) || ((mode & MessageMode.PlainForward) > 0))
				{
					if (outputMsg != null)
					{
						string mess = Utils.GetMessagePlainReplyToBody(CurrentAccount, outputMsg);
						mess = Utils.DecodeHtml(mess);
						if ((mode & MessageMode.PlainReply) > 0)
						{
							newMode |= MessageMode.PlainReply;
							XmlElement replyPlainElem = webmailElement.OwnerDocument.CreateElement("reply_plain");
							replyPlainElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeCDATABody(mess)));
							messageElem.AppendChild(replyPlainElem);
						}
						if ((mode & MessageMode.PlainForward) > 0)
						{
							newMode |= MessageMode.PlainForward;
							XmlElement forwardPlainElem = webmailElement.OwnerDocument.CreateElement("forward_plain");
							forwardPlainElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeCDATABody(mess)));
							messageElem.AppendChild(forwardPlainElem);
						}
					}
				}

				if ((mode & MessageMode.FullHeaders) > 0)
				{
					newMode |= MessageMode.FullHeaders;
					XmlNode fullHeadersNode = webmailElement.OwnerDocument.CreateElement("full_headers");
                    fullHeadersNode.AppendChild(webmailElement.OwnerDocument.CreateCDataSection((outputMsg != null) ? Utils.EncodeCDATABody(outputMsg.RawHeader) : string.Empty));
					messageElem.AppendChild(fullHeadersNode);
				}

				if ((mode & MessageMode.Attachments) > 0)
				{
					if (outputMsg.Attachments.Count > 0)
					{
						newMode |= MessageMode.Attachments;
						XmlNode attachmntsNode = webmailElement.OwnerDocument.CreateElement("attachments");
						if (outputMsg != null)
						{
							foreach (Attachment attach in outputMsg.Attachments)
							{
								string attachmentName = (attach.Filename.Length > 0) ? attach.Filename : attach.Name;

								XmlNode attachmentNode = webmailElement.OwnerDocument.CreateElement("attachment");

								attrNode = webmailElement.OwnerDocument.CreateAttribute("size");
								attrNode.Value = attach.Size.ToString();
								attachmentNode.Attributes.Append(attrNode);

								attrNode = webmailElement.OwnerDocument.CreateAttribute("inline");
								attrNode.Value = (attach.IsInline) ? "1" : "0";
								attachmentNode.Attributes.Append(attrNode);

								XmlElement filenameElem = webmailElement.OwnerDocument.CreateElement("filename");
								filenameElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(attachmentName));
								attachmentNode.AppendChild(filenameElem);

								string filename = Path.GetFileName(attach.SavedAs);
								if (attach.ContentType.ToLower().StartsWith("image"))
								{
									XmlNode viewNode = webmailElement.OwnerDocument.CreateElement("view");
									viewNode.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.GetAttachmentDownloadLink(attach, true)));
									attachmentNode.AppendChild(viewNode);
								}

								XmlNode downloadNode = webmailElement.OwnerDocument.CreateElement("download");
								downloadNode.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.GetAttachmentDownloadLink(attach, false)));
								attachmentNode.AppendChild(downloadNode);

								XmlElement tempnameElem = webmailElement.OwnerDocument.CreateElement("tempname");
								tempnameElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(filename)));
								attachmentNode.AppendChild(tempnameElem);

								XmlElement mimeTypeElem = webmailElement.OwnerDocument.CreateElement("mime_type");
								mimeTypeElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(Utils.GetAttachmentMimeTypeFromFileExtension(Path.GetExtension(filename)))));
								attachmentNode.AppendChild(mimeTypeElem);

								attachmntsNode.AppendChild(attachmentNode);
							}
						}
						messageElem.AppendChild(attachmntsNode);
					}
				}
				attrNode = webmailElement.OwnerDocument.CreateAttribute("mode");
				attrNode.Value = ((int)newMode).ToString(CultureInfo.InvariantCulture);
				messageElem.Attributes.Append(attrNode);

				attrNode = webmailElement.OwnerDocument.CreateAttribute("charset");
				attrNode.Value = msg.OverrideCharset.ToString(CultureInfo.InvariantCulture);
				messageElem.Attributes.Append(attrNode);

				XmlElement saveLinkElem = webmailElement.OwnerDocument.CreateElement("save_link");
				saveLinkElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.GetMessageDownloadLink(msg, id_folder, full_name_folder)));
				messageElem.AppendChild(saveLinkElem);

				webmailElement.AppendChild(messageElem);
			}
			else
			{
				throw new WebMailException("Message not exists.");
			}
		}

		private void LoginAccount(XmlElement webmailElement, string email, string login, string password, string inc_server, int inc_protocol, int inc_port, string out_server, int out_port, bool smtp_auth, bool sign_me, bool advanced_login)
		{
			_actions.LoginAccount(email, login, password, inc_server, (IncomingMailProtocol)inc_protocol, inc_port, out_server, out_port, smtp_auth, sign_me, advanced_login);
			XmlElement loginElem = webmailElement.OwnerDocument.CreateElement("login");
			if (sign_me)
			{
				XmlAttribute idAttr = webmailElement.OwnerDocument.CreateAttribute("id_acct");
				idAttr.Value = CurrentAccount.ID.ToString(CultureInfo.InvariantCulture);
				loginElem.Attributes.Append(idAttr);

				string hash = Utils.GetMD5DigestHexString(Utils.EncodePassword(CurrentAccount.MailIncomingPassword));
				XmlElement hashElem = webmailElement.OwnerDocument.CreateElement("hash");
				hashElem.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(hash));
				loginElem.AppendChild(hashElem);
			}
			webmailElement.AppendChild(loginElem);
		}

		private void GroupOperations(XmlElement webmailElement, string request, long to_folder_id, string to_folder_full_name, XmlPacketMessages messages)
		{
			if (CurrentAccount != null)
			{
				XmlElement operationMessagesElement = webmailElement.OwnerDocument.CreateElement("operation_messages");
			
				XmlAttribute attributeNode = webmailElement.OwnerDocument.CreateAttribute("type");
				attributeNode.Value = request;
				operationMessagesElement.Attributes.Append(attributeNode);

				XmlElement toFolderElement = webmailElement.OwnerDocument.CreateElement("to_folder");
				attributeNode = webmailElement.OwnerDocument.CreateAttribute("id");
				attributeNode.Value = to_folder_id.ToString(CultureInfo.InvariantCulture);
				toFolderElement.Attributes.Append(attributeNode);
				toFolderElement.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(to_folder_full_name)));
				operationMessagesElement.AppendChild(toFolderElement);

				XmlElement messagesElement = operationMessagesElement.OwnerDocument.CreateElement("messages");

				MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(CurrentAccount));
				try
				{
					mp.Connect();

					if (messages.messages != null)
					{
						if ((messages.look_for.search_query != null) && (messages.look_for.search_query.Length > 0) &&
							(request == "mark_all_read" || request == "mark_all_unread"))
						{
							FolderCollection fc = new FolderCollection();
							if ((messages.folder.id != -1) && (messages.folder.full_name != null && messages.folder.full_name.Length > 0))
							{
								fc.Add(mp.GetFolder(messages.folder.id));
							}
							else
							{
								fc = mp.GetFolders();
								FolderCollection viewedFolders = new FolderCollection();
								foreach (Folder f in fc)
								{
									if (!f.Hide) viewedFolders.Add(f);
								}
								fc = viewedFolders;
							}
							WebMailMessageCollection coll = mp.SearchMessages(messages.look_for.search_query, fc, (messages.look_for.fields == 0) ? true : false);
							object[] messageIndexSet = coll.ToIDsCollection();
							// new request string 
							switch (request)
							{
								case "mark_all_read":
									request = "mark_read";
									break;
								case "mark_all_unread":
									request = "mark_unread";
									break;
							}
							foreach (Folder fld in fc)
							{
								GroupMessagesProcess(request, mp, messageIndexSet, fld, to_folder_id, to_folder_full_name);
							}
						}
						else if ((messages.folder.id != -1) && (messages.folder.full_name != null && messages.folder.full_name.Length > 0))
						{
							// process messages in single folder
							Folder fld = mp.GetFolder(messages.folder.id);

							XmlElement folderElement = webmailElement.OwnerDocument.CreateElement("folder");
							attributeNode = webmailElement.OwnerDocument.CreateAttribute("id");
							attributeNode.Value = fld.ID.ToString(CultureInfo.InvariantCulture);
							folderElement.Attributes.Append(attributeNode);
							folderElement.AppendChild(webmailElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(fld.FullPath)));
							operationMessagesElement.AppendChild(folderElement);

							if (fld != null)
							{
								object[] messageIndexSet = null;
								if (messages.messages.Length > 0)
								{
									messageIndexSet = new object[messages.messages.Length];
									for (int i = 0; i < messages.messages.Length; i++)
									{
										messageIndexSet[i] = (fld.SyncType != FolderSyncType.DirectMode) ? (object)messages.messages[i].id : messages.messages[i].uid;
									}
								}
								GroupMessagesProcess(request, mp, messageIndexSet, fld, to_folder_id, to_folder_full_name);
							}
						}
						else
						{
							// process messages in many folders
							for (int i = 0; i < messages.messages.Length; i++)
							{
								Folder fld = mp.GetFolder(messages.messages[i].folder.id);
								if (fld != null)
								{
									GroupMessagesProcess(request, mp, new object[] {messages.messages[i].id}, fld, to_folder_id, to_folder_full_name);
								}
							}
						}
						// create output xml
						for (int i = 0; i < messages.messages.Length; i++)
						{
							XmlElement messageElement = operationMessagesElement.OwnerDocument.CreateElement("message");

							XmlAttribute idAttr = operationMessagesElement.OwnerDocument.CreateAttribute("id");
							idAttr.Value = messages.messages[i].id.ToString();
							messageElement.Attributes.Append(idAttr);

							XmlElement uidElem = operationMessagesElement.OwnerDocument.CreateElement("uid");
							uidElem.AppendChild(operationMessagesElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(messages.messages[i].uid)));
							messageElement.AppendChild(uidElem);

							XmlElement folderElem = operationMessagesElement.OwnerDocument.CreateElement("folder");
							idAttr = operationMessagesElement.OwnerDocument.CreateAttribute("id");
							idAttr.Value = messages.messages[i].folder.id.ToString();
							folderElem.Attributes.Append(idAttr);
							folderElem.AppendChild(operationMessagesElement.OwnerDocument.CreateCDataSection(Utils.EncodeHtml(messages.messages[i].folder.full_name)));
							messageElement.AppendChild(folderElem);

							messagesElement.AppendChild(messageElement);
						}
					}
				}
				finally
				{
					mp.Disconnect();
				}
				operationMessagesElement.AppendChild(messagesElement);

				webmailElement.AppendChild(operationMessagesElement);
			}
			else
			{
				Log.WriteLine("GroupOperations", "Account is null.");
				throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
			}
		}

		private void GroupMessagesProcess(string action, MailProcessor mp, object[] messageIndexSet, Folder fld, long to_folder_id, string to_folder_full_name)
		{
			_actions.GroupMessagesProcess(action, mp, messageIndexSet, fld, to_folder_id, to_folder_full_name);
		}

	}
}
