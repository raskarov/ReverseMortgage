using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WebMailPro
{
    public partial class contactlist : System.Web.UI.Page
    {
        protected WebmailResourceManager _resMan = null;
        protected string _skin = "Hotmail_Style";
        protected Account _acct = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
            _acct = Session[Constants.sessionAccount] as Account;

            if (null != _acct)
            {
                _skin = _acct.UserOfAccount.Settings.DefaultSkin;
            }
        }

        protected string getTableBuffer()
        {
            string output = @"<tr class=""wm_inbox_read_item"" id=""c_001"">
			<td style=""width: 22px; text-align: center;"" id=""none"">
				<input type=""checkbox"" class=""wm_hide""/>
			</td>
			<td class=""wm_inbox_from_subject"">
				No contact
				<input type=""hidden"" value="""" />
			</td>
		</tr>";

            int contacts_count, groups_count;

			try
			{
				BaseWebMailActions bwml = new BaseWebMailActions(_acct, this.Page);

				AddressBookGroupContact[] ContactsGroups = bwml.GetContactsGroups(1, 3, 1, -1, "", 0, out contacts_count, out groups_count);

				string fullname = String.Empty;
				string email = String.Empty;
                string temp = String.Empty;

                if (ContactsGroups.Length > 0)
                {
                    output = String.Empty;
                }

				for(int i = 0; ContactsGroups.Length > i; i++)
				{
					fullname = ContactsGroups[i].fullname;
                    email = ContactsGroups[i].email;
                    
					if(ContactsGroups[i].isGroup)
					{
                        AddressBookGroup group = bwml.GetGroup((int) ContactsGroups[i].id);

                        if (group.Contacts.Length > 0)
                        {
                            string pEmail = String.Empty;
                            string emailsOfGroup = String.Empty;
                            foreach (AddressBookContact contact in group.Contacts)
                            {
                                pEmail = contact.GetPrimaryEmailAsString();
                                if (pEmail.Length > 0)
                                {
                                    emailsOfGroup += (contact.FullName.Length > 0)
                                        ? "&quot;" + contact.FullName + "&quot; <" + pEmail + ">, "
                                        : pEmail + ", ";
                                }
                            }
                            temp = emailsOfGroup.Trim().Trim(',');
					    }
                        else
                        {
                            temp = "";
                        }

                        output += @"<tr class=""wm_inbox_read_item"" id=""g_" + ContactsGroups[i].id.ToString() + @""">
		<td style=""width: 22px; text-align: center;"" id=""none"">
			<input type=""checkbox"" />
		</td>
		<td class=""wm_inbox_from_subject"">
            <img src=""skins/" + _skin + @"/contacts/group.gif""> " + Utils.EncodeHtmlSimple(fullname) + @"
			<input type=""hidden"" value=""" + temp + @""" />
		</td>
	</tr>";
					}
                    else
                    {
                        temp = (fullname.Length > 0) ? "&quot;" + fullname + "&quot; " : "";
                        temp += (fullname.Length > 0) ? "<" + email + ">" : email;

                        output += @"<tr class=""wm_inbox_read_item"" id=""c_" + ContactsGroups[i].id.ToString() + @""">
		<td style=""width: 22px; text-align: center;"" id=""none"">
			<input type=""checkbox"" />
		</td>
		<td class=""wm_inbox_from_subject"">
			" + Utils.EncodeHtmlSimple(temp) + @"
			<input type=""hidden"" value=""" + temp + @""" />
		</td>
	</tr>";
                    }
				}
			
			}
			catch {}

            return output;
		}

        protected string getInputParam()
        {
            int fInt = 1;
            try
            {
                fInt = int.Parse(Request["f"]);
            }
            catch
            {
                fInt = 1;
            }

            if (fInt > 3 || fInt < 1)
            {
                fInt = 1;
            }

            return fInt.ToString();
        }
    }
}
