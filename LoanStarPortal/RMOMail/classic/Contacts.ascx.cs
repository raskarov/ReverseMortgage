namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Text.RegularExpressions;

	/// <summary>
	///		Summary description for Contacts.
	/// </summary>
	public partial class Contacts : System.Web.UI.UserControl
	{
		protected string _skin = Constants.defaultSkinName;
		protected WebmailResourceManager _manager = null;
		protected Account _acct = null;
		protected string HTMLContactsGroups = string.Empty;
		protected int contacts_count;
		protected int groups_count;
		protected int page;
		protected short _sort_field = 1;
		protected short _sort_order = 1;
		protected int _id_group = -1;
		protected string _look_for = String.Empty;
		protected string screen;
		protected bool IsContact;
		protected bool IsNewContact;
		
		protected string si_Group = string.Empty;
		protected string si_Name = string.Empty;
		protected string si_EMail = string.Empty;

		public int PageNumber
		{
			get { return page; }
			set { page = value; }
		}
		public int IDGroup
		{
			get { return _id_group; }
			set { _id_group = value; }
		}

		public string LookFor
		{
			get { return _look_for; }
			set { _look_for = value; }
		}

		public string Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}

		protected AddressBookContact _contact;
		public AddressBookContact Contact
		{
			get { return _contact; }
			set { _contact = value; }
		}

		protected AddressBookGroup[] _allGroups;
		public AddressBookGroup[] AllGroups
		{
			get { return _allGroups; }
			set { _allGroups = value; }
		}

		protected AddressBookGroup _group;
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

		public short SortField
		{
			get { return _sort_field; }
			set { _sort_field = value; }
		}

		public short SortOrder
		{
			get { return _sort_order; }
			set { _sort_order = value; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			_manager = (new WebmailResourceManagerCreator()).CreateResourceManager();
			switch(_sort_field)
			{
				case 0:
					si_Group = @"<img src=""skins/"+Skin+@"/menu/order_arrow_down.gif"">";
					break;
				case 1:
					si_Name = @"<img src=""skins/"+Skin+@"/menu/order_arrow_down.gif"">";
					break;
				case 2:
					si_EMail = @"<img src=""skins/"+Skin+@"/menu/order_arrow_down.gif"">";
					break;
			}
		}

		protected void Pre_Load(object sender, System.EventArgs e)
		{
			_acct = Session[Constants.sessionAccount] as Account;

			LoadData();

			LoadContactsGroups(_acct);

			_js.AddInitText(@"
				PageSwitcher = new CPageSwitcher('" + _skin + @"');
				PageSwitcher.Build();
				PageSwitcher.Show(" + page.ToString() + ", " + _acct.UserOfAccount.Settings.ContactsPerPage.ToString() + ", " + ((int)(contacts_count + groups_count)).ToString() + @", ""Pager('"", ""');"");
			");

			_js.AddInitText("ContactsList = new CContactsList();");
			_js.AddInitText("Init_contacts();");
			_js.AddFile("classic/class_cnewcontactscreen.js");
			_js.AddFile("classic/class_contacts_selection.js");
			_js.AddFile("classic/contacts_resizer.js");
			_js.AddFile("classic/class_pageswitcher_classic.js");
			_js.AddFile("screen.contacts.js");

			
			js.AddText(@"
var ContactsList, PageSwitcher, Selection;

function DoNewMessageButton(contactId)
{
	if (contactId)
	{
		var cid = ParseCId(contactId);
		if (cid)
		{
			var form = CreateChild(document.body, ""form"");
			form.action = ""basewebmail.aspx?scr=new_message"";
			form.method = ""POST"";
			hideinput = CreateChildWithAttrs(form, 'input', [['type', 'hidden']]);
			hideinput.name = ""contacts_"" + cid.cid + ""_"" + cid.type;
			form.submit();
		}
		return true;
	}
	else
	{
		var arr = Selection.GetCheckedLines();
		var form = CreateChild(document.body, ""form"");
		form.action = ""basewebmail.aspx?scr=new_message"";
		form.method = ""POST"";
		if (arr.length > 0)
		{
			for(i = 0; i < arr.length; i++)
			{
				var cid = ParseCId(arr[i]);
				if (cid)
				{
					hideinput = CreateChildWithAttrs(form, 'input', [['type', 'hidden']]);
					hideinput.name = ""contacts_"" + cid.cid + ""_"" + cid.type;
				}
			}
		}
		form.submit();
		return true;
	}
}
");

			JS_CheckThisLine();

			if(Request.QueryString.Get("scr") == null)
			{
				screen = null;
			}
			else
			{
				screen = Request.QueryString.Get("scr").ToLower();
			}

			switch(screen)
			{
				case "contacts":
					ShowContacts();
					break;
				case "contacts_view":
					ShowContactsView();
					break;
				case "contacts_add":
					ShowContactsAdd();
					break;
				case "contacts_import":
					ShowContactsImport();
					break;
			}				 
		}

		protected void LoadData()
		{
			//Contacts
			if(Session["current_page"] != null)
			{
				PageNumber = (int)Session["current_page"];
			}
			else
			{
				PageNumber = 1;
			}
		}

		protected void ShowContactsImport()
		{
			LiteralContactsViewer_1.Text = @"
				<div>
					<table class=""wm_contacts_card"" id=""wm_contacts_card"">
						<tr>
							<td class=""wm_contacts_card_top_left"">
								<div class=""wm_contacts_card_corner""></div>
							</td>
							<td class=""wm_contacts_card_top""></td>
							<td class=""wm_contacts_card_top_right"">
								<div class=""wm_contacts_card_corner""></div>
							</td>
						</tr>
						<tr>
							<td class=""wm_contacts_card_left""></td>
							<td id=""ImportContacts"">
			";

			PlaceHolderContact.Controls.Clear();
			
			ImportContacts ctrlImportContacts = LoadControl(@"ImportContacts.ascx") as ImportContacts;
			if (ctrlImportContacts != null)
			{
				ctrlImportContacts.ID = "newImportContacts";
				ctrlImportContacts.js = js;

				PlaceHolderContact.Controls.Add(ctrlImportContacts);
			}
			
			LiteralContactsViewer_2.Text = @"
							</td>
							<td class=""wm_contacts_card_right""></td>
						</tr>
						<tr>
							<td class=""wm_contacts_card_bottom_left"">
								<div class=""wm_contacts_card_corner""></div>
							</td>
							<td class=""wm_contacts_card_bottom""></td>
							<td class=""wm_contacts_card_bottom_right"">
								<div class=""wm_contacts_card_corner""></div>
							</td>
						</tr>
					</table>
				</div>
			";

			string text = @"
			ImportContactsScreen = new CImportContactsScreenPart(""" + _skin + @""", true);
			obj = document.getElementById(""ImportContacts"");
			ImportContactsScreen.Build(obj);
			ImportContactsScreen.Show();
			";

			_js.AddInitText(text);

			text = @"
				var EmptyHtmlUrl = ""empty.html"";
				var ImportUrl = ""import.aspx"";
				var ImportContactsScreen;

				function ImportContactsHandler(code, count) {
					switch (code) {
						case 0:
							alert(Lang.ErrorImportContacts);
							break;
						case 1: 
							document.location =""basewebmail.aspx?scr=contacts_import"";
							break;
						case 2:
							alert(Lang.ErrorNoContacts);
							break;
						case 3:
							alert(Lang.ErrorInvalidCSV);
							break;
					}
				}
			";

			_js.AddText(text);
		}

		public void ShowContacts()
		{
			LiteralContactsViewer_1.Text = @"
				<div></div>
			";
		}

		protected void ShowContactsView()
		{
			LiteralContactsViewer_1.Text = @"
				<div>
					<table class=""wm_contacts_card"" id=""wm_contacts_card"">
						<tr>
							<td class=""wm_contacts_card_top_left"">
								<div class=""wm_contacts_card_corner""></div>
							</td>
							<td class=""wm_contacts_card_top""></td>
							<td class=""wm_contacts_card_top_right"">
								<div class=""wm_contacts_card_corner""></div>
							</td>
						</tr>
						<tr>
							<td class=""wm_contacts_card_left""></td>
							<td>
			";

			PlaceHolderContact.Controls.Clear();

			IsContact = (bool)Session["IsContact"];

			if(IsContact)
			{
				ContactContainer ctrlContactsView = LoadControl(@"ContactContainer.ascx") as ContactContainer;
				if (ctrlContactsView != null)
				{
					ctrlContactsView.ID = "ContactsViewID";
					ctrlContactsView.Skin = _skin;
					ctrlContactsView.js = _js;

					ctrlContactsView.IsNew = false;

					ctrlContactsView.AllGroups = _allGroups;
					ctrlContactsView.Contact = _contact;

					PlaceHolderContact.Controls.Add(ctrlContactsView);
				}
			}
			else
			{
				GroupContainer ctrlGroupContainer = LoadControl(@"GroupContainer.ascx") as GroupContainer;
				if (ctrlGroupContainer != null)
				{
					ctrlGroupContainer.ID = "GroupsViewID";
					ctrlGroupContainer.Skin = _skin;
					ctrlGroupContainer.js = _js;

					ctrlGroupContainer.IsNew = false;

					ctrlGroupContainer.Group = _group;

					PlaceHolderContact.Controls.Add(ctrlGroupContainer);
				}
			}

			LiteralContactsViewer_2.Text = @"
							</td>
							<td class=""wm_contacts_card_right""></td>
						</tr>
						<tr>
							<td class=""wm_contacts_card_bottom_left"">
								<div class=""wm_contacts_card_corner""></div>
							</td>
							<td class=""wm_contacts_card_bottom""></td>
							<td class=""wm_contacts_card_bottom_right"">
								<div class=""wm_contacts_card_corner""></div>
							</td>
						</tr>
					</table>
				</div>
			";
		}

		protected void ShowContactsAdd()
		{
			LiteralContactsViewer_1.Text = @"
				<div>
					<table class=""wm_contacts_card"" id=""wm_contacts_card"">
						<tr>
							<td class=""wm_contacts_card_top_left"">
								<div class=""wm_contacts_card_corner""></div>
							</td>
							<td class=""wm_contacts_card_top""></td>
							<td class=""wm_contacts_card_top_right"">
								<div class=""wm_contacts_card_corner""></div>
							</td>
						</tr>
						<tr>
							<td class=""wm_contacts_card_left""></td>
							<td>
			";

			PlaceHolderContact.Controls.Clear();

			IsNewContact = (bool)Session["IsNewContact"];

			if(IsNewContact)
			{
				ContactContainer ctrlContactsAdd = LoadControl(@"ContactContainer.ascx") as ContactContainer;
				if (ctrlContactsAdd != null)
				{
					ctrlContactsAdd.ID = "ContactsAddID";
					ctrlContactsAdd.Skin = _skin;
					ctrlContactsAdd.js = _js;

					ctrlContactsAdd.IsNew = true;

					PlaceHolderContact.Controls.Add(ctrlContactsAdd);
				}
			}
			else
			{
				GroupContainer ctrlGroupContainer = LoadControl(@"GroupContainer.ascx") as GroupContainer;
				if (ctrlGroupContainer != null)
				{
					ctrlGroupContainer.ID = "GroupsAddID";
					ctrlGroupContainer.Skin = _skin;
					ctrlGroupContainer.js = _js;

					ctrlGroupContainer.IsNew = true;

					PlaceHolderContact.Controls.Add(ctrlGroupContainer);
				}
			}

			LiteralContactsViewer_2.Text = @"
							</td>
							<td class=""wm_contacts_card_right""></td>
						</tr>
						<tr>
							<td class=""wm_contacts_card_bottom_left"">
								<div class=""wm_contacts_card_corner""></div>
							</td>
							<td class=""wm_contacts_card_bottom""></td>
							<td class=""wm_contacts_card_bottom_right"">
								<div class=""wm_contacts_card_corner""></div>
							</td>
						</tr>
					</table>
				</div>
			";
		}

		public void LoadContactsGroups(Account acct)
		{
			try
			{
				BaseWebMailActions bwml = new BaseWebMailActions(acct, this.Page);

				AddressBookGroupContact[] ContactsGroups = bwml.GetContactsGroups(page, _sort_field, _sort_order, _id_group, _look_for, 0, out contacts_count, out groups_count);

				HTMLContactsGroups = string.Empty;

				string fullname = String.Empty;
				string email = String.Empty;

				for(int i = 0; ContactsGroups.Length > i; i++)
				{
					if(_look_for.Length > 0)
					{
						fullname = Regex.Replace(ContactsGroups[i].fullname, "(?i)" + _look_for + "(?-i)", "<font>" + _look_for + "</font>");
					}
					else
					{
						fullname = ContactsGroups[i].fullname;
					}

					if(ContactsGroups[i].isGroup)
					{
						HTMLContactsGroups += @"
						<tr class=""wm_inbox_read_item"" id=""g_" + ContactsGroups[i].id + @""" onclick=""CheckThisLine(event, this);"">
							<td style=""width: 22px; text-align: center;"" id=""none"">
								<input type=""checkbox"" />
							</td>
                            <td style=""width: 24px; text-align: center;"">
								<img src=""skins/" + _skin + @"/contacts/group.gif""/>
							</td>
							<td class=""wm_inbox_from_subject"" style=""width: 140px;"">
								<nobr>" + fullname + @"</nobr>
							</td>
							<td class=""wm_inbox_from_subject"" style=""width: 330px;"">
								<nobr></nobr>
							</td>
						</tr>
					";
					}
					else
					{
						if(_look_for.Length > 0)
						{
							email = Regex.Replace(ContactsGroups[i].email, "(?i)" + _look_for + "(?-i)", "<font>" + _look_for + "</font>");
						}
						else
						{
							email = ContactsGroups[i].email;
						}

						// ondblclick=""ComposeTo()""
						HTMLContactsGroups += @"
						<tr class=""wm_inbox_read_item"" id=""c_" + ContactsGroups[i].id + @""" onclick=""CheckThisLine(event, this);"">
							<td style=""width: 22px; text-align: center;"" id=""none"">
								<input type=""checkbox"" />
							</td>
							<td style=""width: 24px; text-align: center;""/>
							<td class=""wm_inbox_from_subject"" style=""width: 140px;"">
								<nobr>" + fullname + @"</nobr>
							</td>
							<td class=""wm_inbox_from_subject"" style=""width: 330px;"">
								<nobr>" + email + @"</nobr>
							</td>
						</tr>
					";
					}
				}

				/*if (HTMLContactsGroups.Length > 0)
				{*/
					LiteralContactsGroups.Text = @"<table style=""WIDTH: 512px; TEXT-ALIGN: center"" id=""list"">" + HTMLContactsGroups + "</table>";
				/*}
				else
				{
					LiteralContactsGroups.Text = "";
				}*/
			}
			catch (Exception ex)
			{
				((basewebmail)this.Page).OutputException(ex);
			}
		}

		protected void JS_CheckThisLine()
		{
			string AddText = @"	
				function Sort(obj)
				{
					HFAction = document.getElementById(""HFAction"");
					HFRequest = document.getElementById(""HFRequest"");
					HFValue = document.getElementById(""HFValue"");
					
					HFAction.value = """";
					HFRequest.value = """";
					HFValue.value = """";

					var sort_field = null;
					var error = false;

					switch(obj.id)
					{
						case ""group"":
							sort_field = 0;
							break;
						case ""name"":
							sort_field = 1;
							break;
						case ""email"":
							sort_field = 2;
							break;
						default:
							error = true;
							break;
					}

					if(!error)
					{
						HFAction.value = ""sort"";
						HFRequest.value = ""contacts"";
						HFValue.value = sort_field;

						__doPostBack('PostBackButton','');
					}
				}

				function ViewAddressRecord(lineid)
				{
					HFAction = document.getElementById(""HFAction"");
					HFRequest = document.getElementById(""HFRequest"");
					HFValue = document.getElementById(""HFValue"");

					lineid = lineid.split(""_"");
					
					HFAction.value = ""get"";

					if(lineid[0] == ""c"")
					{
						HFRequest.value = ""contact"";
					}
					else
					{
						HFRequest.value = ""group"";
					}

					HFValue.value = lineid[1];
					
					__doPostBack('PostBackButton','');
				}
				
				function Pager(id)
				{
					HFPageInfo = document.getElementById(""HFPageInfo"");
		
					HFPageInfo.value = """";

					HFPageInfo.value = ""c----"" + id;
					
					__doPostBack('PagerButton','');
				}
				";

			_js.AddText(AddText);
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

		}
		#endregion
	}
}
