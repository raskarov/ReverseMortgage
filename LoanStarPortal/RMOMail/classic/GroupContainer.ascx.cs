using System.Globalization;

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
	///		Summary description for GroupContainer.
	/// </summary>
	public partial class GroupContainer : System.Web.UI.UserControl
	{
		protected WebmailResourceManager _resMan = null;
		protected System.Web.UI.WebControls.Button savebutton;
		protected System.Web.UI.WebControls.Button addgroupbutton;

		protected string _groupContacsHtml = "";
		protected bool _IsNew = false;
		protected string delimeter = "";

        protected string groupEmail = string.Empty;
        protected string groupCompany = string.Empty;
        protected string groupStreet = string.Empty;
        protected string groupCity = string.Empty;
        protected string groupFax = string.Empty;
        protected string groupState = string.Empty;
        protected string groupPhone = string.Empty;
        protected string groupZip = string.Empty;
        protected string groupCountry = string.Empty;
        protected string groupWeb = string.Empty;

        public bool IsNew
		{
			get { return _IsNew; }
			set { _IsNew = value; }
		}

		protected AddressBookGroup _group = null;
		public AddressBookGroup Group
		{
			get { return _group; }
			set { _group = value; }
		}

	
		protected jsbuilder _js;
		public jsbuilder js
		{
			get { return _js; }
			set { _js = value; }
		}

		protected string _skin = Constants.defaultSkinName;
		public string Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}

		public string IsOrganization
		{
			get
			{
				return ((_group != null) && (_group.Organization)) ? "checked" : string.Empty;
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			Account acct = Session[Constants.sessionAccount] as Account;

			delimeter = Utils.GetDelimeter();

			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
            
			try
			{
				if(IsPostBack)
				{
					if (_IsNew)
					{
						// create new group
						AddressBookGroup newGroup = new AddressBookGroup();
						newGroup.IDUser = acct.IDUser;
						newGroup.GroupName = Request.Form[this.groupNameNew.Name];
						newGroup.Phone = Request.Form["gphone"];
						newGroup.Fax = Request.Form["gfax"];
						newGroup.Web = Request.Form["gweb"];
						newGroup.Organization = (string.Compare(Request.Form["isorganization"], "on", true, CultureInfo.InvariantCulture) == 0) ? true : false;
						newGroup.Email = Request.Form["gemail"];
						newGroup.Company = Request.Form["gcompany"];
						newGroup.Street = Request.Form["gstreet"];
						newGroup.City = Request.Form["gcity"];
						newGroup.State = Request.Form["gstate"];
						newGroup.Zip = Request.Form["gzip"];
						newGroup.Country = Request.Form["gcountry"];

						string emailsString = Request.Form[this.groupEmailsNew.Name];
						string[] emailsArray = (emailsString.Trim().Length > 0) ? emailsString.Split(',') : new string[0];

						newGroup.NewContacts = new AddressBookContact[emailsArray.Length];
						for(int i = 0; newGroup.NewContacts.Length > i; i++)
						{
							newGroup.NewContacts[i] = new AddressBookContact();
							newGroup.NewContacts[i].IDUser = acct.IDUser;
							newGroup.NewContacts[i].HEmail = emailsArray[i].Trim();
						}

						if (!Validation.CheckIt(Validation.ValidationTask.GroupName, newGroup.GroupName))
						{
							throw (new Exception(Validation.ErrorMessage));
						}
						newGroup.GroupName = Validation.Corrected;
						BaseWebMailActions baseAction = new BaseWebMailActions(acct, this.Page);
						AddressBookGroup[] existedGroups = baseAction.GetGroups();
						foreach (AddressBookGroup g in existedGroups)
							{
								if (g.GroupName==newGroup.GroupName) throw (new Exception(_resMan.GetString("WarningGroupAlreadyExist")));
							}
						baseAction.NewGroup(newGroup);

						Session[Constants.sessionReportText] = string.Format(@"{0} ""{1}"" {2}", _resMan.GetString("ReportGroupSuccessfulyAdded1"), newGroup.GroupName, _resMan.GetString("ReportGroupSuccessfulyAdded2"));
						Response.Redirect("basewebmail.aspx?scr=contacts");
					}
					else
					{
						if (_group != null)
						{
							// update existing group
							_group.GroupName = Request.Form[this.groupNameEdit.Name];

							if (!Validation.CheckIt(Validation.ValidationTask.GroupName, _group.GroupName))
							{
								throw (new Exception(Validation.ErrorMessage));
							}
							_group.GroupName = Validation.Corrected;

							_group.Phone = Request.Form["gphone"];
							_group.Fax = Request.Form["gfax"];
							_group.Web = Request.Form["gweb"];
							_group.Organization = (string.Compare(Request.Form["isorganization"], "on", true, CultureInfo.InvariantCulture) == 0) ? true : false;
							_group.Email = Request.Form["gemail"];
							_group.Company = Request.Form["gcompany"];
							_group.Street = Request.Form["gstreet"];
							_group.City = Request.Form["gcity"];
							_group.State = Request.Form["gstate"];
							_group.Zip = Request.Form["gzip"];
							_group.Country = Request.Form["gcountry"];
							
							string emailsString = Request.Form[this.groupEmailsEdit.Name];
							string[] emailsArray = (emailsString.Trim().Length > 0) ? emailsString.Split(',') : new string[0];

							_group.NewContacts = new AddressBookContact[emailsArray.Length];
							for(int i = 0; i < _group.NewContacts.Length; i++)
							{
								if (emailsArray[i].Trim().Length > 0)
								{
									_group.NewContacts[i] = new AddressBookContact();
									_group.NewContacts[i].IDUser = acct.IDUser;
									_group.NewContacts[i].HEmail = emailsArray[i].Trim();
								}
							}
							
							BaseWebMailActions baseAction = new BaseWebMailActions(acct, this.Page);

							AddressBookGroup[] existedGroups = baseAction.GetGroups();
							foreach (AddressBookGroup g in existedGroups)
							{
								if ((g.GroupName==_group.GroupName)&&(g.IDGroup!=_group.IDGroup)) throw (new Exception(_resMan.GetString("WarningGroupAlreadyExist")));
							}

							baseAction.UpdateGroup(_group);

							Session[Constants.sessionReportText] = _resMan.GetString("ReportGroupUpdatedSuccessfuly");
							Response.Redirect("basewebmail.aspx?scr=contacts");
						}
					}
				}
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}

            if (_group != null)
            {
                groupEmail = Utils.EncodeHtml(_group.Email);
                groupCompany = Utils.EncodeHtml(_group.Company);
                groupStreet = Utils.EncodeHtml(_group.Street);
                groupCity = Utils.EncodeHtml(_group.City);
                groupFax = Utils.EncodeHtml(_group.Fax);
                groupState = Utils.EncodeHtml(_group.State);
                groupPhone = Utils.EncodeHtml(_group.Phone);
                groupZip = Utils.EncodeHtml(_group.Zip);
                groupCountry = Utils.EncodeHtml(_group.Country);
                groupWeb = Utils.EncodeHtml(_group.Web);
            }
        }

		protected void Pre_Load(object sender, System.EventArgs e)
		{
			_js.AddText(@"
				function GroupCheck(isNew)
				{
					var oVal = new CValidate();
					if (isNew)
					{
						var sGroupName = document.getElementById('"+groupNameNew.ClientID+@"').value;
						var aContacts = document.getElementById('"+groupEmailsNew.ClientID+@"').value.split(',');
					}
					else
					{
						var sGroupName = document.getElementById('"+groupNameEdit.ClientID+@"').value;
						var aContacts = document.getElementById('"+groupEmailsEdit.ClientID+@"').value.split(',');
					}
					if (oVal.IsEmpty(sGroupName))
						{
							alert(Lang.WarningGroupNotComplete);
							return false;
						}
					var iCount = aContacts.length;
					for (var i=0; i<iCount; i++)
					{
						if (oVal.HasEmailForbiddenSymbols(Trim(aContacts[i])))
							{
								alert(Lang.WarningCorrectEmail);
								return false;
							}
					}
					return true;
				}
			");
			_js.AddInitText(@"
OrgTab = document.getElementById(""orgTab"");
OrgTabImg = document.getElementById(""orgTabImg"");
OrgTable = document.getElementById(""orgTable"");
OrgCheckBox = document.getElementById(""isorganization"");
OrgDiv = document.getElementById(""orgDiv"");

ShowHideOrgDiv();
ShowHideOrgForm();
");
			if (_group != null) _js.AddInitText(@"Selection.CheckLine('g_" + _group.IDGroup.ToString() + @"');");
			_js.AddText(@"
			
var OrgTab, OrgTable, OrgCheckBox, OrgDiv;
var isOrg = false;

function ShowHideOrgDiv()
{
    if (!OrgDiv || !OrgCheckBox) return false;
    OrgDiv.className = (OrgCheckBox.checked == true) ? """" : ""wm_hide"";
    ResizeElements(""all"");
}

function ShowHideOrgForm()
{
    if (isOrg)
    {
        OrgTable.className = ""wm_hide"";
        OrgTabImg.src=""skins/" + _skin + @"/menu/arrow_down.gif"";
    }
    else
    {
        OrgTable.className = ""wm_contacts_view"";
        OrgTabImg.src=""skins/" + _skin + @"/menu/arrow_up.gif"";
    }
    
    isOrg = !isOrg;
    ResizeElements(""all"");
}

function dolocation(idurl)
{
	var url = document.getElementById(idurl);
	if (url && url.value.length > 2) OpenURL(url.value);
}

");
			if (!IsNew)
			{
				PlaceHolderGroupView.Visible = true;
				PlaceHolderGroupAdd.Visible = false;

				groupNameEdit.Value = _group.GroupName;
				_groupContacsHtml = _getContactFromGroupAsHtml();

				_js.AddText(@"
function RenameGroup()
{
	var gcontrol = document.getElementById(""renameLink"");
	var gspan = document.getElementById(""span_gname"");
	var ginput = document.getElementById(""ContactsID_GroupsViewID_groupNameEdit"");

	gspan.className = ""wm_hide"";
	gcontrol.className = ""wm_hide"";
	ginput.value = gspan.innerHTML;
	ginput.className = ""wm_input"";

	ginput.onkeydown = function(ev)
	{
		if (isEnter(ev))
		{
			var g_control = document.getElementById(""renameLink"");
			var g_span = document.getElementById(""span_gname"");
			var g_input = document.getElementById(""ContactsID_GroupsViewID_groupNameEdit"");

			g_span.innerHTML = ginput.value;
			g_input.className = ""wm_hide"";
			g_control.className = """";
			g_span.className = """";
			return false;
		}	
	}
	ginput.focus();
}			

function SelectAllInputs(obj)
{
    var table = document.getElementById(""contacts_in_group"");
		if (table)
		{
			var inputs = table.getElementsByTagName(""input"");
			var i, c;
			for (i = 0, c = inputs.length; i < c; i++) {
				if (inputs[i].type == ""checkbox"" && !inputs[i].disabled) {
					inputs[i].checked = obj.checked;
				}
			}
		}
		return false;
}

function DeleteContactsFromGroup()
{
	var inputs = document.getElementsByTagName(""input"");
	var i, c, t = 0;
	for (i = 0, c = inputs.length; i < c; i++)
	{
		if (inputs[i].type == ""checkbox"" && inputs[i].name == ""cont_check[]"")
		{ 
			t++;
			if  (inputs[i].checked)
			{
				t--;
				var tr = document.getElementById(""in_group_"" + inputs[i].value);
				var imp = document.getElementById(""inp_"" + inputs[i].value);
				if (tr) tr.className = ""wm_hide"";
				if (imp) imp.value = ""-1"";
			}
		}
	}
	if (t == 0)
	{
		var table = document.getElementById(""contacts_in_group"");
		if (table) table.className = ""wm_hide"";
		var mail_group = document.getElementById(""mail_group"");
		if (mail_group) mail_group.className = ""wm_hide"";		
	}
}

function MailGroup(id)
{
	var form = CreateChildWithAttrs(document.body,'form', [['method', 'POST']]);
	form.action = """";
	CreateChildWithAttrs(form, 'input', [['type', 'hidden'], ['name', 'groupid'], ['value', id]]);
	form.submit();
	DoNewMessageButton('g_'+id);
}
");
			}
			else
			{
				PlaceHolderGroupView.Visible = false;
				PlaceHolderGroupAdd.Visible = true;
			}
			
		}

		protected string printGroupContacts()
		{
			return _groupContacsHtml;
		}

		private string _getContactFromGroupAsHtml()
		{
			string output = "";
			if (_group != null && _group.Contacts != null && _group.Contacts.Length > 0)
			{
				output = @"
<table id=""contacts_in_group"" class=""wm_inbox_lines"" style=""margin: 20px 20px 0px; width: 300px;"">
<tr class=""wm_inbox_read_item"" id=""in_group_0"">
	<td>
		<input type=""checkbox"" class=""wm_checkbox"" onclick=""SelectAllInputs(this);"" id=""CheckAll"" name=""CheckAll"" />
	</td>
    <td>			
		" + _resMan.GetString("Email") + @"
	</td>
	<td>
		" + _resMan.GetString("Name") + @"
	</td>
</tr>
	";

				foreach( AddressBookContact _conact in _group.Contacts)
				{
					string contactEmail = "";
					switch (_conact.PrimaryEmail)
					{
						default:
						case (ContactPrimaryEmail.Personal): contactEmail = _conact.HEmail; break;
						case (ContactPrimaryEmail.Business): contactEmail = _conact.BEmail; break;
						case (ContactPrimaryEmail.Other): contactEmail = _conact.OtherEmail; break;
					}

					output += @"
<tr class=""wm_inbox_read_item"" id=""in_group_" + _conact.IDAddr.ToString() + @""">
	<td>
		<input type=""checkbox"" class=""wm_checkbox"" id=""ch_" + _conact.IDAddr.ToString() + @""" name=""cont_check[]"" value=""" + _conact.IDAddr.ToString() + @""" />
		<input type=""hidden"" id=""inp_" + _conact.IDAddr.ToString() + @""" value=""" + _conact.IDAddr.ToString() + @""" name=""contactsIds[]"" />
	</td>
	<td class=""wm_inbox_from_subject""><nobr>" + Utils.EncodeHtml(_conact.FullName) + @"</nobr></td>
	<td class=""wm_inbox_from_subject""><nobr>" + Utils.EncodeHtml(contactEmail) + @"</nobr></td>
</tr>
";
				}
				output += @"<tr id=""mail_group"">
								<td colspan=""2"">
									<a href=""#"" onclick=""MailGroup(" + _group.IDGroup.ToString() + @");"">" + _resMan.GetString("MailGroup") + @"
								</td>
								<td style=""text-align: right;"">
									<a href=""#"" onclick=""DeleteContactsFromGroup();"">" + _resMan.GetString("RemoveFromGroup") + @"</a>
								</td>
							</tr></table>";
			}

			return output;
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
			this.PreRender += new System.EventHandler(this.Pre_Load);
//			addgroupbutton.Click += new EventHandler(this.addgroupbutton_Click);
//			savebutton.Click += new EventHandler(this.savebutton_Click);
		}
		#endregion
	}
}
