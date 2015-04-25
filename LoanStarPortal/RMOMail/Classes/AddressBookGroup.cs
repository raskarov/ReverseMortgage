using System;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for AddressBookGroup.
	/// </summary>
	public class AddressBookGroup
	{
		#region Fields

		private int _id_group = -1;
		private int _id_user = -1;
		private string _group_nm = string.Empty;
		private int _use_frequency;
		private string _email = string.Empty;
		private string _company = string.Empty;
		private string _street = string.Empty;
		private string _city = string.Empty;
		private string _state = string.Empty;
		private string _zip = string.Empty;
		private string _country = string.Empty;
		private string _phone = string.Empty;
		private string _fax = string.Empty;
		private string _web = string.Empty;
		private bool _organization = false;

		private AddressBookContact[] _contacts = new AddressBookContact[0];
		private AddressBookContact[] _newContacts = new AddressBookContact[0];

		#endregion

		#region Properties

		public int IDGroup
		{
			get { return _id_group; }
			set { _id_group = value; }
		}

		public int IDUser
		{
			get { return _id_user; }
			set { _id_user = value; }
		}

		public string GroupName
		{
			get { return _group_nm; }
			set { _group_nm = value; }
		}

		public AddressBookContact[] Contacts
		{
			get { return _contacts; }
			set { _contacts = value; }
		}

		public AddressBookContact[] NewContacts
		{
			get { return _newContacts; }
			set { _newContacts = value; }
		}

		public int UseFrequency
		{
			get { return _use_frequency; }
			set { _use_frequency = value; }
		}

		public string Email
		{
			get { return _email; }
			set { _email = value; }
		}

		public string Company
		{
			get { return _company; }
			set { _company = value; }
		}

		public string Street
		{
			get { return _street; }
			set { _street = value; }
		}

		public string City
		{
			get { return _city; }
			set { _city = value; }
		}

		public string State
		{
			get { return _state; }
			set { _state = value; }
		}

		public string Zip
		{
			get { return _zip; }
			set { _zip = value; }
		}

		public string Country
		{
			get { return _country; }
			set { _country = value; }
		}

		public string Phone
		{
			get { return _phone; }
			set { _phone = value; }
		}

		public string Fax
		{
			get { return _fax; }
			set { _fax = value; }
		}

		public string Web
		{
			get { return _web; }
			set { _web = value; }
		}

		public bool Organization
		{
			get { return _organization; }
			set { _organization = value; }
		}

		#endregion

		public AddressBookGroup() {}

		public void RenameGroup(string newName)
		{
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				dbMan.UpdateAddressBookGroup(this);
			}
			finally
			{
				dbMan.Disconnect();
			}
		}

		public static AddressBookGroup CreateGroup(AddressBookGroup groupToCreate)
		{
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				groupToCreate = dbMan.CreateAddressBookGroup(groupToCreate);
			}
			finally
			{
				dbMan.Disconnect();
			}
			return groupToCreate;
		}

		public static void DeleteGroup(AddressBookGroup groupToDelete)
		{
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				dbMan.CreateAddressBookGroup(groupToDelete);
			}
			finally
			{
				dbMan.Disconnect();
			}
		}

	}
}
