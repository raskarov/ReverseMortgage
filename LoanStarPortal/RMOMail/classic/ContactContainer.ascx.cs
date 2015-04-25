namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using WebMailPro;

	/// <summary>
	///		Summary description for ContactContainer.
	/// </summary>
	public partial class ContactContainer : System.Web.UI.UserControl
	{
		protected WebmailResourceManager _resMan = null;
		protected AddressBookContact _contact = null;
		protected AddressBookGroup[] _groups = null;
		protected string _skin = Constants.defaultSkinName;
		protected string delimeter = "";

		protected string EmailFromMail = "";
		protected string FullNameFromMail = "";
		
		protected bool errorFlag = true;

        public string Tab1 = @"class=""wm_hide""";
        public string Tab2 = @"class=""wm_hide""";
        public string Tab3 = @"class=""wm_hide""";
		
		public AddressBookContact Contact
		{
			get { return _contact; }
			set { _contact = value; }
		}

		public AddressBookGroup[] AllGroups
		{
			get { return _groups; }
			set { _groups = value; }
		}

		protected jsbuilder _js;
		public jsbuilder js
		{
			get { return _js; }
			set { _js = value; }
		}

		public string Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}

		protected bool _isNew = false;
		public bool IsNew
		{
			get { return _isNew; }
			set { _isNew = value; }
		}
















		protected System.Web.UI.HtmlControls.HtmlTableCell td_Notes;
       
		protected string groupsHtml1 = "", groupsHtml2 = "";








		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			delimeter = Utils.GetDelimeter();

			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			
			InitContact();
			if(IsPostBack)
			{
				_saveOrAddContact();
			}

			_js.AddText(@"
			function checkNameAndEMailFields()
			{
                var defEmailValue = document.getElementById('" + input_default_email.ClientID + @"').value;
                var persEmailValue = document.getElementById('" + personal_email.ClientID + @"').value;
                var busEmailValue = document.getElementById('" + business_email.ClientID + @"').value;
                var otherEmailValue = document.getElementById('" + other_email.ClientID + @"').value;
                var fullNameValue = document.getElementById('" + c_fullname.ClientID + @"').value;
                if (defEmailValue == '' && persEmailValue == '' && busEmailValue == '' && 
                 otherEmailValue == '' && fullNameValue == '')
                {
                    alert(Lang.WarningContactNotComplete);
                    return;
                }
                var oVal = new CValidate();
                if (oVal.HasEmailForbiddenSymbols(defEmailValue))
                {
                    alert(Lang.WarningCorrectEmail);
                    return;
                }
                __doPostBack('PostBackButton','');
			}
			");

			_js.AddFile("classic/class_cnewcontactscreen.js");

			_js.AddText(@"
	function dolocation(idUrl)
	{
		var url = document.getElementById(idUrl).value;
		if (url.length > 2)	OpenURL(url);
	}

	function MessageToMail(email)
	{
		document.location = ""basewebmail.aspx?scr=new_message&mailto="" + email;
	}

	function ChangeTabVisibility(tab_name)
	{
		var tab = document.getElementById(tab_name);
		if (!tab) return false;
		if(tab.className == 'wm_contacts_view')
		{
			tab.className = 'wm_hide';
			document.getElementById('button_' + tab_name).src = 'skins/"+ _skin +@"/menu/arrow_down.gif';
		}
		else
		{
			tab.className = 'wm_contacts_view';
			document.getElementById('button_' + tab_name).src = 'skins/"+ _skin +@"/menu/arrow_up.gif';
		}
        ResizeElements(""all"");
	}
	");
	}

		private void InitContact()
		{
			submitButton.Value = _resMan.GetString("Save");
            cancelButton.Value = _resMan.GetString("Cancel");

			birthday_month.Items.Clear();
			birthday_month.Items.Add(new ListItem(_resMan.GetString("Month"), "0"));
			birthday_month.Items.Add(new ListItem(_resMan.GetString("January"), "1"));
			birthday_month.Items.Add(new ListItem(_resMan.GetString("February"), "2"));
			birthday_month.Items.Add(new ListItem(_resMan.GetString("March"), "3"));
			birthday_month.Items.Add(new ListItem(_resMan.GetString("April"), "4"));
			birthday_month.Items.Add(new ListItem(_resMan.GetString("May"), "5"));
			birthday_month.Items.Add(new ListItem(_resMan.GetString("June"), "6"));
			birthday_month.Items.Add(new ListItem(_resMan.GetString("July"), "7"));
			birthday_month.Items.Add(new ListItem(_resMan.GetString("August"), "8"));
			birthday_month.Items.Add(new ListItem(_resMan.GetString("September"), "9"));
			birthday_month.Items.Add(new ListItem(_resMan.GetString("October"), "10"));
			birthday_month.Items.Add(new ListItem(_resMan.GetString("November"), "11"));
			birthday_month.Items.Add(new ListItem(_resMan.GetString("December"), "12"));

			if (!IsNew)
			{
				if (_contact == null) 
				{
					_contact = new AddressBookContact();
				}
				else
				{
                    string tempBool = (_contact.IsOpen()) ? "true" : "false";
					_js.AddInitText(@"
		Selection.CheckLine('c_" + _contact.IDAddr.ToString() + @"');
		newContact = new CNewContactScreenPart(0, 'ContactsID_ContactsViewID_');
		newContact.InitEditContacts(" + tempBool + @");
		");
				}

		// contact view screen

				isNewContact.Value = "1";
				tr_FullName.Visible = (!IsEmpty(_contact.FullName));
				td_FullName.InnerHtml = Utils.EncodeHtml(_contact.FullName);
				c_fullname.Value = _contact.FullName; 

				string MEmail = "";
				switch (_contact.PrimaryEmail)
				{
					default:
					case ContactPrimaryEmail.Personal:	MEmail = Utils.EncodeHtml(_contact.HEmail); break;
					case ContactPrimaryEmail.Business:	MEmail = Utils.EncodeHtml(_contact.BEmail); break;
					case ContactPrimaryEmail.Other:		MEmail = Utils.EncodeHtml(_contact.OtherEmail); break;
				}

				tr_Email.Visible = (!IsEmpty(MEmail));
				input_default_email.Value = MEmail;
				td_Email.InnerHtml = @"<a href=""#"" onclick=""return MessageToMail('" + MEmail + @"')"">" + MEmail + "</a>";

				tr_Birthday.Visible = (_contact.BirthdayDay != 0 || _contact.BirthdayDay != 0 || _contact.BirthdayDay != 0);
				td_Birthday.InnerHtml = _getBirthDay(_contact.BirthdayDay, _contact.BirthdayMonth, _contact.BirthdayYear);


				table_Personal.Visible = (!IsEmpty(_contact.HEmail) || !IsEmpty(_contact.HStreet) ||
					!IsEmpty(_contact.HCity) || !IsEmpty(_contact.HFax) || !IsEmpty(_contact.HState) ||
					!IsEmpty(_contact.HPhone) || !IsEmpty(_contact.HZip) || !IsEmpty(_contact.HMobile) ||
					!IsEmpty(_contact.HCountry) || !IsEmpty(_contact.HWeb));

                Tab1 = (table_Personal.Visible) ? @"class=""wm_contacts_view""" : @"class=""wm_hide""";

				tr_HEmail.Visible = (!IsEmpty(_contact.HEmail));
				td_HEmail.InnerHtml = @"<a href=""#"" onclick=""return MessageToMail('" + Utils.EncodeHtml(_contact.HEmail) + @"')"">" + Utils.EncodeHtml(_contact.HEmail) + "</a>";
				personal_email.Value = _contact.HEmail;
							
				tr_HStreet.Visible = (!IsEmpty(_contact.HStreet));
				td_HStreet.InnerHtml = Utils.EncodeHtml(_contact.HStreet);
				personal_street.InnerHtml = Utils.EncodeHtml(_contact.HStreet);

				tr_HCityFax.Visible = (!IsEmpty(_contact.HCity) || !IsEmpty(_contact.HFax));
				td_HCity1.Visible = (!IsEmpty(_contact.HCity));
				td_HCity2.Visible = (!IsEmpty(_contact.HCity));
				td_HCity1.InnerHtml = _resMan.GetString("City") + ":";
				td_HCity2.InnerHtml = Utils.EncodeHtml(_contact.HCity);
				personal_city.Value = _contact.HCity;
				td_HFax1.Visible = (!IsEmpty(_contact.HFax));
				td_HFax2.Visible = (!IsEmpty(_contact.HFax));
				td_HFax1.InnerHtml = _resMan.GetString("Fax") + ":";
				td_HFax2.InnerHtml = Utils.EncodeHtml(_contact.HFax);
				personal_fax.Value = _contact.HFax;

				tr_HStatePhone.Visible = (!IsEmpty(_contact.HState) || !IsEmpty(_contact.HPhone));
				td_HState1.Visible = (!IsEmpty(_contact.HState));
				td_HState2.Visible = (!IsEmpty(_contact.HState));
				td_HState1.InnerHtml = _resMan.GetString("StateProvince") + ":";
				td_HState2.InnerHtml = Utils.EncodeHtml(_contact.HState);
				personal_state.Value = _contact.HState;
				td_HPhone1.Visible = (!IsEmpty(_contact.HPhone));
				td_HPhone2.Visible = (!IsEmpty(_contact.HPhone));
				td_HPhone1.InnerHtml = _resMan.GetString("Phone") + ":";
				td_HPhone2.InnerHtml = Utils.EncodeHtml(_contact.HPhone);
				personal_phone.Value = _contact.HPhone;

				tr_HZipMobile.Visible = (!IsEmpty(_contact.HZip) || !IsEmpty(_contact.HMobile));
				td_HZip1.Visible = (!IsEmpty(_contact.HZip));
				td_HZip2.Visible = (!IsEmpty(_contact.HZip));
				td_HZip1.InnerHtml = _resMan.GetString("ZipCode") + ":";
				td_HZip2.InnerHtml = Utils.EncodeHtml(_contact.HZip);
				personal_zip.Value = _contact.HZip;
				td_HMobile1.Visible = (!IsEmpty(_contact.HMobile));
				td_HMobile2.Visible = (!IsEmpty(_contact.HMobile));
				td_HMobile1.InnerHtml = _resMan.GetString("Mobile") + ":";
				td_HMobile2.InnerHtml = Utils.EncodeHtml(_contact.HMobile);
				personal_mobile.Value = _contact.HMobile;

				tr_HCountry.Visible = (!IsEmpty(_contact.HCountry));
				td_HCountry.InnerHtml = Utils.EncodeHtml(_contact.HCountry);
				personal_country.Value = _contact.HCountry;
				tr_HWeb.Visible = (!IsEmpty(_contact.HWeb));
				td_HWeb.InnerHtml = Utils.EncodeHtml(_contact.HWeb);
				personal_web.Value = _contact.HWeb;

				table_Business.Visible = (!IsEmpty(_contact.BCompany) || !IsEmpty(_contact.BJobTitle) ||
					!IsEmpty(_contact.BDepartment) || !IsEmpty(_contact.BOffice) || !IsEmpty(_contact.BCity) ||
					!IsEmpty(_contact.BFax) || !IsEmpty(_contact.BState) || !IsEmpty(_contact.BPhone) ||
					!IsEmpty(_contact.BZip) || !IsEmpty(_contact.BCountry) || !IsEmpty(_contact.BEmail) ||
					!IsEmpty(_contact.BStreet) || !IsEmpty(_contact.BWeb));

                Tab2 = (table_Business.Visible) ? @"class=""wm_contacts_view""" : @"class=""wm_hide""";

				tr_BEmail.Visible = (!IsEmpty(_contact.BEmail));
				business_email.Value = _contact.BEmail;
				td_BEmail.InnerHtml = @"<a href=""#"" onclick=""return MessageToMail('" + Utils.EncodeHtml(_contact.BEmail) + @"')"">" + Utils.EncodeHtml(_contact.BEmail) + "</a>";

				tr_BCompanyJob.Visible = (!IsEmpty(_contact.BCompany) || !IsEmpty(_contact.BJobTitle));
				td_BCompany1.Visible = (!IsEmpty(_contact.BCompany));
				td_BCompany2.Visible = (!IsEmpty(_contact.BCompany));
				td_BCompany1.InnerHtml = _resMan.GetString("Company") + ":";
				td_BCompany2.InnerHtml = Utils.EncodeHtml(_contact.BCompany);
				business_company.Value = _contact.BCompany;
				td_BJob1.Visible = (!IsEmpty(_contact.BJobTitle));
				td_BJob2.Visible = (!IsEmpty(_contact.BJobTitle));
				td_BJob1.InnerHtml = _resMan.GetString("JobTitle") + ":";
				td_BJob2.InnerHtml = Utils.EncodeHtml(_contact.BJobTitle);
				business_job.Value = _contact.BJobTitle;

				tr_BDepartmentOffice.Visible = (!IsEmpty(_contact.BDepartment) || !IsEmpty(_contact.BOffice));
				td_BDepartment1.Visible = (!IsEmpty(_contact.BDepartment));
				td_BDepartment2.Visible = (!IsEmpty(_contact.BDepartment));
				td_BDepartment1.InnerHtml = _resMan.GetString("Department") + ":";
				td_BDepartment2.InnerHtml = Utils.EncodeHtml(_contact.BDepartment);
				business_departament.Value = _contact.BDepartment;
				td_BOffice1.Visible = (!IsEmpty(_contact.BOffice));
				td_BOffice2.Visible = (!IsEmpty(_contact.BOffice));
				td_BOffice1.InnerHtml = _resMan.GetString("Office") + ":";
				td_BOffice2.InnerHtml = Utils.EncodeHtml(_contact.BOffice);
				business_office.Value = _contact.BOffice;

				tr_BStreet.Visible = (!IsEmpty(_contact.BStreet));
				td_BStreet.InnerHtml = Utils.EncodeHtml(_contact.BStreet);
				td_BStreet.InnerHtml = Utils.EncodeHtml(_contact.BStreet);

				tr_BCityFax.Visible = (!IsEmpty(_contact.BCity) || !IsEmpty(_contact.BFax));
				td_BCity1.Visible = (!IsEmpty(_contact.BCity));
				td_BCity2.Visible = (!IsEmpty(_contact.BCity));
				td_BCity1.InnerHtml = _resMan.GetString("City") + ":";
				td_BCity2.InnerHtml = Utils.EncodeHtml(_contact.BCity);
				business_city.Value = _contact.BCity;
				td_BFax1.Visible = (!IsEmpty(_contact.BFax));
				td_BFax2.Visible = (!IsEmpty(_contact.BFax));
				td_BFax1.InnerHtml = _resMan.GetString("Fax") + ":";
				td_BFax2.InnerHtml = Utils.EncodeHtml(_contact.BFax);
				business_fax.Value = _contact.BFax;

				tr_BStatePhone.Visible = (!IsEmpty(_contact.BState) || !IsEmpty(_contact.BPhone));
				td_BState1.Visible = (!IsEmpty(_contact.BState));
				td_BState2.Visible = (!IsEmpty(_contact.BState));
				td_BState1.InnerHtml = _resMan.GetString("StateProvince") + ":";
				td_BState2.InnerHtml = Utils.EncodeHtml(_contact.BState);
				business_state.Value = _contact.BState;
				td_BPhone1.Visible = (!IsEmpty(_contact.BPhone));
				td_BPhone2.Visible = (!IsEmpty(_contact.BPhone));
				td_BPhone1.InnerHtml = _resMan.GetString("Phone") + ":";
				td_BPhone2.InnerHtml = Utils.EncodeHtml(_contact.BPhone);
				business_phone.Value = _contact.BPhone;

				tr_BZipCountry.Visible = (!IsEmpty(_contact.BZip) || !IsEmpty(_contact.BCountry));
				td_BZip1.Visible = (!IsEmpty(_contact.BZip));
				td_BZip2.Visible = (!IsEmpty(_contact.BZip));
				td_BZip1.InnerHtml = _resMan.GetString("ZipCode") + ":";
				td_BZip2.InnerHtml = Utils.EncodeHtml(_contact.BZip);
				business_zip.Value = _contact.BZip;
				td_BCountry1.Visible = (!IsEmpty(_contact.BCountry));
				td_BCountry2.Visible = (!IsEmpty(_contact.BCountry));
				td_BCountry1.InnerHtml = _resMan.GetString("CountryRegion") + ":";
				td_BCountry2.InnerHtml = Utils.EncodeHtml(_contact.BCountry);
				business_country.Value = _contact.BCountry;

				tr_BWeb.Visible = (!IsEmpty(_contact.BWeb));
				td_BWeb.InnerHtml = Utils.EncodeHtml(_contact.BWeb);
				business_web.Value = _contact.BWeb;

                table_Other.Visible = (!IsEmpty(_contact.OtherEmail) || !IsEmpty(_contact.Notes) || tr_Birthday.Visible);

                Tab3 = (table_Other.Visible) ? @"class=""wm_contacts_view""" : @"class=""wm_hide""";

				tr_OMail.Visible = (!IsEmpty(_contact.OtherEmail));
				td_OMail.InnerHtml = @"<a href=""#"" onclick=""return MessageToMail('" + Utils.EncodeHtml(_contact.OtherEmail) + @"')"">" + Utils.EncodeHtml(_contact.OtherEmail) + "</a>";
				other_email.Value = _contact.OtherEmail;
				tr_Notes.Visible = (!IsEmpty(_contact.Notes));
				td_Notes.InnerHtml = Utils.EncodeHtml(_contact.Notes);
				other_notes.InnerHtml = _contact.Notes;

				groupsHtml1 = "";
				if (_contact.Groups.Length > 0)
				{
					groupsHtml1 = @"
	<table class=""wm_contacts_view"">
		<tr>
			<td class=""wm_contacts_view_title wm_contacts_section_name"">" + _resMan.GetString("Groups") + @":</td>
					";
					int k = 0;
					int cnt = _contact.Groups.Length;
					foreach (AddressBookGroup _group in _contact.Groups)
					{	
						k++;
						string tempStr = (cnt > k) ? "," : " ";
                        groupsHtml1 += @"<td class=""wm_contacts_groups""><a href=""basewebmail.aspx?scr=contacts_view&idgroup=" + _group.IDGroup + @""">" + _group.GroupName + @"</a>" + tempStr + @"</td>
										";
					}
					groupsHtml1 += @"</tr></table>";
				}

				groupsHtml2 = "";
				if (_groups != null && _groups.Length > 0)
				{
					foreach (AddressBookGroup _group in _groups)
					{
						bool inGroup = false;
						foreach (AddressBookGroup _contactGroup in _contact.Groups)
						{
							if (_contactGroup.IDGroup == _group.IDGroup)
							{
								inGroup = true;
							}
						}
				
						string tempCheck = (inGroup) ? @"checked=""checked""" : "";
						groupsHtml2 += @"<input id=""inp_g_" + _group.IDGroup.ToString() + @""" class=""wm_checkbox"" " + tempCheck + @" type=""checkbox"" value=""" + _group.IDGroup.ToString() + @""" name=""groupsIds[]"" />
										<label for=""inp_g_" + _group.IDGroup.ToString() + @""">" + _group.GroupName + @"</label><br />";
					}
				}
				else
				{
					groupTableControl.Visible = false;
				}
		// contact edit screen

				default_email_type.Value = ((int) _contact.PrimaryEmail).ToString();

				ListItem conactMonth = birthday_month.Items.FindByValue(_contact.BirthdayMonth.ToString());
				if (conactMonth != null)
				{
					conactMonth.Selected = true;
				}

				int lastyear = (int) DateTime.Now.Year;
				birthday_year.Items.Clear();
				birthday_year.Items.Add(new ListItem(_resMan.GetString("Year"), "0"));
				for (int i = lastyear; i > 1899; i--)
				{
					if (_contact.BirthdayYear == i)
					{
						ListItem li = new ListItem(i.ToString(), i.ToString());
						li.Selected = true;
						birthday_year.Items.Add(li);
					}
					else
					{
						birthday_year.Items.Add(new ListItem(i.ToString(), i.ToString()));
					}
				}
							
				birthday_day.Items.Clear();
				birthday_day.Items.Add(new ListItem(_resMan.GetString("Day"), "0"));
				for (int i = 1; i < 32; i++)
				{
					if (_contact.BirthdayDay == i)
					{
						ListItem li = new ListItem(i.ToString(), i.ToString());
						li.Selected = true;
						birthday_day.Items.Add(li);
					}
					else
					{
						birthday_day.Items.Add(new ListItem(i.ToString(), i.ToString()));
					}
				}

				use_friendly_name.Checked = _contact.UseFriendlyName;

		}
		else
		{
			_contact = new AddressBookContact();
			_contact.PrimaryEmail = ContactPrimaryEmail.Personal;

            Tab1 = @"class=""wm_contacts_view""";

			if(Session["NewContactEmail"] != null)
			{
				EmailFromMail = (string)Session["NewContactEmail"];
				FullNameFromMail = (string)Session["NewContactName"];
			}
			else
			{
				EmailFromMail = "";
				FullNameFromMail = "";
			}

			input_default_email.Value = EmailFromMail;
			c_fullname.Value = FullNameFromMail;
			personal_email.Value = EmailFromMail;

			_js.AddInitText(@"
	newContact = new CNewContactScreenPart(1, 'ContactsID_ContactsAddID_');
	newContact.InitEditContacts();
			");

			isNewContact.Value = "1";

			int lastyear = (int) DateTime.Now.Year;
			birthday_year.Items.Clear();
			birthday_year.Items.Add(new ListItem(_resMan.GetString("Year"), "0"));
			for (int i = lastyear; i > 1899; i--)
			{
				birthday_year.Items.Add(new ListItem(i.ToString(), i.ToString()));
			}
						
			birthday_day.Items.Clear();
			birthday_day.Items.Add(new ListItem(_resMan.GetString("Day"), "0"));
			for (int i = 1; i < 32; i++)
			{
				birthday_day.Items.Add(new ListItem(i.ToString(), i.ToString()));
			}

			if (_groups == null || _groups.Length == 0)
			{
				groupTableControl.Visible = false;
			}
		}
	}

		private bool IsEmpty(string Str)
		{
			return (Str == string.Empty || Str == null || Str.Trim() == string.Empty);
		}

		protected string printGroup(int type)
		{
			return (type == 1) ? groupsHtml1 : groupsHtml2;
		}

		protected string getSkin()
		{
			return _skin;
		}

		private string _getBirthDay(byte d, byte m, short y)
		{
			string res = "";
			if (y != 0)
			{
				res += y.ToString();
				if (d != 0 || m != 0) res += ",";
			}
			if (d != 0) res += " " + d.ToString();
			switch (m)
			{	
				default:
				case 1: res += " Jan"; break;
				case 2: res += " Feb"; break;
				case 3: res += " Mar"; break;
				case 4: res += " Apr"; break;
				case 5: res += " May"; break;
				case 6: res += " Jun"; break;
				case 7: res += " Jul"; break;
				case 8: res += " Aug"; break;
				case 9: res += " Sep"; break;
				case 10: res += " Oct"; break;
				case 11: res += " Nov"; break;
				case 12: res += " Dec"; break;
			}
			return res;
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion

		private void _saveOrAddContact()
		{			
			string screen = Request.QueryString["scr"];

			switch(screen)
			{
				case "contacts_view":
					Save("ContactsViewID", 0);
					break;
				case "contacts_add":
					Save("ContactsAddID", 1);
					break;
			}
		}

		//mode = 0 - Edit Contact
		//mode = 1 - Add Contact
		private void Save(string prefix, int mode)
		{
			try
			{
				Account acct = Session[Constants.sessionAccount] as Account;
				BaseWebMailActions bwa = new BaseWebMailActions(acct, this.Page);
				if (_isNew) Contact = new AddressBookContact();
                Contact.FullName = Request.Form[c_fullname.UniqueID.ToString()].Trim();
                Contact.BirthdayDay = byte.Parse(Request.Form[birthday_day.UniqueID.ToString()].Trim());
                Contact.BirthdayMonth = byte.Parse(Request.Form[birthday_month.UniqueID.ToString()].Trim());
                Contact.BirthdayYear = short.Parse(Request.Form[birthday_year.UniqueID.ToString()].Trim());
                if (Request.Form[use_friendly_name.UniqueID.ToString()] == "1")
                {
                    Contact.UseFriendlyName = true;
                }
                else
                {
                    Contact.UseFriendlyName = false;
                }

                string defaultEmailTypeString = Request.Form[default_email_type.UniqueID.ToString()].Trim();
                string defaultEmailValueString = Request.Form[input_default_email.UniqueID.ToString()].Trim();
                string hEmailValueString = Request.Form[personal_email.UniqueID.ToString()].Trim();
                string bEmailValueString = Request.Form[business_email.UniqueID.ToString()].Trim();
                string otherEmailValueString = Request.Form[other_email.UniqueID.ToString()].Trim();
                if (defaultEmailTypeString == string.Empty)
                {
                    if (bEmailValueString != string.Empty)
                    {
                        Contact.PrimaryEmail = ContactPrimaryEmail.Business;
                    }
                    else if (otherEmailValueString != string.Empty)
                    {
                        Contact.PrimaryEmail = ContactPrimaryEmail.Other;
                    }
                    else
                    {
                        Contact.PrimaryEmail = ContactPrimaryEmail.Personal;
                    }
                }
                else
                {
                    Contact.PrimaryEmail = (ContactPrimaryEmail)int.Parse(defaultEmailTypeString);
                }
                switch (Contact.PrimaryEmail)
                {
                    case ContactPrimaryEmail.Business:
                        if (defaultEmailValueString != string.Empty)
                        {
                            Contact.BEmail = defaultEmailValueString;
                        }
                        else
                        {
                            Contact.BEmail = bEmailValueString;
                        }
                        Contact.HEmail = hEmailValueString;
                        Contact.OtherEmail = otherEmailValueString;
                        break;
                    case ContactPrimaryEmail.Other:
                        if (defaultEmailValueString != string.Empty)
                        {
                            Contact.OtherEmail = defaultEmailValueString;
                        }
                        else
                        {
                            Contact.OtherEmail = otherEmailValueString;
                        }
                        Contact.HEmail = hEmailValueString;
                        Contact.BEmail = bEmailValueString;
                        break;
                    default:
                        if (defaultEmailValueString != string.Empty)
                        {
                            Contact.HEmail = defaultEmailValueString;
                        }
                        else
                        {
                            Contact.HEmail = hEmailValueString;
                        }
                        Contact.BEmail = bEmailValueString;
                        Contact.OtherEmail = otherEmailValueString;
                        break;
                }
					
				string temp = "";
				switch(Contact.PrimaryEmail)
				{
					case ContactPrimaryEmail.Business:
						temp = Contact.BEmail;
						break;
					case ContactPrimaryEmail.Personal:
						temp = Contact.HEmail;
						break;
					case ContactPrimaryEmail.Other:
						temp = Contact.OtherEmail;
						break;
				}

                if (temp != "" && Request.Form[input_default_email.UniqueID.ToString()] != "")
				{
                    temp = Request.Form[input_default_email.UniqueID.ToString()];
					switch(Contact.PrimaryEmail)
					{
						case ContactPrimaryEmail.Business:
							Contact.BEmail = temp;
							break;
						case ContactPrimaryEmail.Personal:
							Contact.HEmail = temp;
							break;
						case ContactPrimaryEmail.Other:
							Contact.OtherEmail = temp;
							break;
					}
				}
                Contact.HStreet = Request.Form[personal_street.UniqueID.ToString()].Trim();
                Contact.HCity = Request.Form[personal_city.UniqueID.ToString()].Trim();
                Contact.HState = Request.Form[personal_state.UniqueID.ToString()].Trim();
                Contact.HZip = Request.Form[personal_zip.UniqueID.ToString()].Trim();
                Contact.HFax = Request.Form[personal_fax.UniqueID.ToString()].Trim();
                Contact.HPhone = Request.Form[personal_phone.UniqueID.ToString()].Trim();
                Contact.HMobile = Request.Form[personal_mobile.UniqueID.ToString()].Trim();
                Contact.HCountry = Request.Form[personal_country.UniqueID.ToString()].Trim();
                Contact.HWeb = Request.Form[personal_web.UniqueID.ToString()].Trim();

                Contact.BCompany = Request.Form[business_company.UniqueID.ToString()].Trim();
                Contact.BDepartment = Request.Form[business_departament.UniqueID.ToString()].Trim();
                Contact.BJobTitle = Request.Form[business_job.UniqueID.ToString()].Trim();
                Contact.BOffice = Request.Form[business_office.UniqueID.ToString()].Trim();
                Contact.BStreet = Request.Form[business_street.UniqueID.ToString()].Trim();
                Contact.BCity = Request.Form[business_city.UniqueID.ToString()].Trim();
                Contact.BState = Request.Form[business_state.UniqueID.ToString()].Trim();
                Contact.BZip = Request.Form[business_zip.UniqueID.ToString()].Trim();
                Contact.BFax = Request.Form[business_fax.UniqueID.ToString()].Trim();
                Contact.BPhone = Request.Form[business_phone.UniqueID.ToString()].Trim();
                Contact.BCountry = Request.Form[business_country.UniqueID.ToString()].Trim();
                Contact.BWeb = Request.Form[business_web.UniqueID.ToString()].Trim();

                Contact.Notes = Request.Form[other_notes.UniqueID.ToString()].Trim();
				if (Contact.HWeb.Length!=0)
				{
					if (Validation.CheckIt(Validation.ValidationTask.ContactsWebPage, Contact.HWeb))
					{
						Contact.HWeb = Validation.Corrected;
					}
				}
				if (Contact.BWeb.Length!=0)
				{
					if (Validation.CheckIt(Validation.ValidationTask.ContactsWebPage, Contact.BWeb))
					{
						Contact.BWeb = Validation.Corrected;
					}
				}
				//If emails and name is empty - ERROR!!!
				int existedFieldsNum = 0;
				if (Contact.BEmail.Length!=0) existedFieldsNum++;
				if (Contact.HEmail.Length!=0) existedFieldsNum++;
				if (Contact.OtherEmail.Length!=0) existedFieldsNum++;
				if ((existedFieldsNum!=0)||(Contact.FullName.Length!=0)) errorFlag = false;
				if (!errorFlag)
				{
					if(mode == 1)
					{
						bwa.NewContact(Contact);

						Session[Constants.sessionReportText] = _resMan.GetString("ReportContactSuccessfulyAdded");
					}
					else
					{
						bwa.UpdateContact(Contact);

						Session[Constants.sessionReportText] = _resMan.GetString("ReportContactUpdatedSuccessfuly");
					}
					Response.Redirect("basewebmail.aspx?scr=contacts");
				}
				else
				{
					throw (new Exception(_resMan.GetString("WarningContactNotComplete")));
				}
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}
	}
}
