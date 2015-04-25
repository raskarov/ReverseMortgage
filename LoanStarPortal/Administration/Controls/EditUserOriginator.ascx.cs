using System;
using System.Data;
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditUserOriginator : AppControl
    {
        #region constants
        private const string ADDHEADERTEXT = "Add new user";
        private const string EDITHEADETEXT = "Edit user({0})";
//        private const string CURRENTUSER = "currentuser";
        private const string ADMININPUTCSS = "admininput";
        private const string DISABLEDSUFFIX = "dis";
        private const string CLICKHANDLER = "onclick";
        private const string JSSETPASSWORD = "SetPassword(this.checked,{0},{1})";
        private const string EMAILREGEXP = @"[\w!#\$%\^\{}`~&'\+-=_\.]+@[\w-\.]+";
        private const string TRVISIBLE = "display:inline";
        private const string TRHIDDEN = "display:none";
        private const string STYLEATTRIBUTE = "style";
        private const string ONCLICKATTRIBUTE = "onclick";
        private const string JSSHOWDIV = "SetDivVisibility(this,'{0}','{1}');";
        private const string DDLMANAGERID = "$ddlManager";
        private const string ROOTELEMENT = "Root";
        private const string ITEMELEMENT = "item";
        private const string USERROLEIDATTRIBUTE = "userroleid";
        private const string MANAGERIDATTRIBUTE = "managerid";
        private const string MANAGERROLEIDATTRIBUTE = "managerroleid";
        public const int ADDCOMMANDID = 1;
        public const int EDITCOMMANDID = 2;
        public const int UPDATECOMMANDID = 3;
        public const int CANCELCOMMANDID = 4;
        private const int FIXEDTABCOUNT = 4;
        #endregion

        #region fields
        private bool isNewUser = false;
        private int currentTab = -1;
        #endregion

        private AppUser User
        {
            get
            {
                return Session[Constants.USEREDITOBJECTNAME] as AppUser;
            }
            set { Session[Constants.USEREDITOBJECTNAME] = value; }
        }
        
        #region methods

        private void ClearError()
        {
            //            trErrMessage.Visible = false;
            lblMessage.Text = "";
            lblFirstNameErr.Text = "";
            lblLastNameErr.Text = "";
            lblLoginErr.Text = "";
            lblPasswordErr.Text = "";
            lblConfirmPasswordErr.Text = "";
            lblMailPasswordErr.Text = "";
            lblMailPasswordConfirmErr.Text = "";
            lblUserMailErr.Text = "";
            lblErrPOP3Server.Text = "";
            lblErrPOP3Port.Text = "";
            lblErrSMTPServer.Text = "";
            lblErrSMTPPort.Text = "";
            lblPrimatyEmailErr.Text = "";
        }
        private void SetPasswordsFields()
        {
            tbMailPassword.Attributes.Add("value", tbMailPassword.Text);
            tbMailPasswordConfirm.Attributes.Add("value", tbMailPasswordConfirm.Text);
        }
        private void goBack()
        {
            Response.Redirect("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.VIEWUSER);
        }

        #region validation
        private bool ValidateData()
        {
            bool res = true;
            if (tbLogin.Text.Length < 6)
            {
                currentTab = 0;
                res = false;
                lblLoginErr.Text = "Login must be at least 6 character long";
            }
            if (tbFirstName.Text == String.Empty)
            {
                currentTab = 0;
                res = false;
                lblFirstNameErr.Text = "*";
            }
            if (tbLastName.Text == String.Empty)
            {
                currentTab = 0;
                res = false;
                lblLastNameErr.Text = "*";
            }
            if (String.IsNullOrEmpty(tbPrimaryEmail.Text))
            {
                lblPrimatyEmailErr.Text = "*";
                currentTab = 0;
                res = false;
            }
            else
            {
                bool r = Regex.Match(tbPrimaryEmail.Text, EMAILREGEXP).Success;
                if (!r)
                {
                    res = false;
                    lblUserMailErr.Text = "Incorrect email format.";
                    currentTab = 0;
                }
            }
            isNewUser = User.Id < 0;
            if (isNewUser)
            {
                string message;
                bool r = Utils.ValidatePassword(tbPassword.Text, User.PreviosPasswords, out message);
                if (!r)
                {
                    lblPasswordErr.Text = message;
                    currentTab = 0;
                    res = false;
                }
                else
                {
                    if (tbConfirmPassword.Text != tbPassword.Text)
                    {
                        lblConfirmPasswordErr.Text = Utils.PASSWORDNOTIDENTICALTEXT;
                        currentTab = 0;
                        res = false;
                    }
                }
            }
            else
            {
                if (cbEnablePassword.Checked)
                {
                    string message;
                    bool r = Utils.ValidatePassword(tbPassword.Text, CurrentUser.PreviosPasswords, out message);
                    if (!r)
                    {
                        lblPasswordErr.Text = message;
                        currentTab = 0;
                        res = false;
                    }
                    else
                    {
                        if (tbConfirmPassword.Text != tbPassword.Text)
                        {
                            lblConfirmPasswordErr.Text = Utils.PASSWORDNOTIDENTICALTEXT;
                            currentTab = 0;
                            res = false;
                        }
                    }
                }
            }
            int re = ValidateEmailData();
            if (re == -1) res = false;
            if (!res)
            {
                rtsUser.SelectedIndex = currentTab;
                rmpUser.SelectedIndex = currentTab;
            }
            SetPasswordsFields();
            return res;
        }
        private int ValidateEmailData()
        {
            int res = 0;
            if (!String.IsNullOrEmpty(tbUserMail.Text))
            {
                bool r = Regex.Match(tbUserMail.Text, EMAILREGEXP).Success;
                if (!r)
                {
                    res = -1;
                    lblUserMailErr.Text = "Incorrect email format.";
                    if (currentTab < 0)
                    {
                        currentTab = 1;
                    }
                }
                else if (String.IsNullOrEmpty(tbUserName.Text))
                {
                    res = -1;
                    lblUserNameErr.Text = "*";
                    if (currentTab < 0)
                    {
                        currentTab = 1;
                    }
                }
                if (!cbCompanyEmailSettings.Checked)
                {
                    if (!WebMailHelper.ValidateServer(tbPOP3Server.Text))
                    {
                        res = -1;
                        lblErrPOP3Server.Text = "Incorrect format";
                        if (currentTab < 0)
                        {
                            currentTab = 1;
                        }
                    }
                    if (!WebMailHelper.ValidateServer(tbSMTPServer.Text))
                    {
                        res = -1;
                        lblErrSMTPServer.Text = "Incorrect format";
                        if (currentTab < 0)
                        {
                            currentTab = 1;
                        }
                    }
                    if (!WebMailHelper.ValidatePort(tbPOP3Port.Text))
                    {
                        res = -1;
                        lblErrPOP3Port.Text = "Port value should be between 0 and 65535.";
                        if (currentTab < 0)
                        {
                            currentTab = 1;
                        }
                    }
                    if (!WebMailHelper.ValidatePort(tbSMTPPort.Text))
                    {
                        res = -1;
                        lblErrSMTPPort.Text = "Port value should be between 0 and 65535.";
                        if (currentTab < 0)
                        {
                            currentTab = 1;
                        }
                    }
                }
                if (String.IsNullOrEmpty(tbMailPassword.Text))
                {
                    res = -1;
                    lblMailPasswordErr.Text = "Password can't be empty.";
                    if (currentTab < 0)
                    {
                        currentTab = 1;
                    }
                }
                else
                {
                    if (tbMailPasswordConfirm.Text != tbMailPassword.Text)
                    {
                        res = -1;
                        lblMailPasswordConfirmErr.Text = "Passwords you have typed not identical";
                        if (currentTab < 0)
                        {
                            currentTab = 1;
                        }
                    }
                }
            }
            else
            {
                res = -2;
            }
            return res;
        }
        #endregion

        #region data binding
        private void BindRoles()
        {
            AppUser usr = User;
            if (usr != null)
            {
                dlRoles.DataSource = usr.GetAllLenderRoles();
                dlRoles.DataBind();
            }
        }
        private void BindData()
        {
            AppUser usr = User;
            if (usr != null)
            {
                isNewUser = !(usr.Id > 0);
                if (isNewUser)
                {
                    lblHeader.Text = ADDHEADERTEXT;
                }
                else
                {
                    lblHeader.Text = String.Format(EDITHEADETEXT, usr.LoginName);
                }
                trPassword.Visible = !isNewUser;
                if (trPassword.Visible)
                {
                    tbPassword.Enabled = cbEnablePassword.Checked;
                }
                tbConfirmPassword.Enabled = tbPassword.Enabled;
                if (trPassword.Visible)
                {
                    cbEnablePassword.Attributes.Add(CLICKHANDLER, String.Format(JSSETPASSWORD, tbPassword.ClientID, tbConfirmPassword.ClientID));
                }
                tbPassword.CssClass = ADMININPUTCSS + (tbPassword.Enabled ? String.Empty : DISABLEDSUFFIX);
                tbConfirmPassword.CssClass = ADMININPUTCSS + (tbConfirmPassword.Enabled ? String.Empty : DISABLEDSUFFIX);
                tbFirstName.Text = usr.FirstName;
                tbLastName.Text = usr.LastName;
                tbLogin.Text = usr.LoginName;
                tbMailPassword.Attributes.Add("value", usr.EmailAccount.MailIncomingPassword);
                tbMailPasswordConfirm.Attributes.Add("value", usr.EmailAccount.MailIncomingPassword);
                tbMailPasswordConfirm.Enabled = tbMailPassword.Enabled;
                tbMailPassword.CssClass = ADMININPUTCSS + (tbMailPassword.Enabled ? String.Empty : DISABLEDSUFFIX);
                tbMailPasswordConfirm.CssClass = ADMININPUTCSS + (tbMailPasswordConfirm.Enabled ? String.Empty : DISABLEDSUFFIX);
                cbLeaveMessage.Checked = usr.EmailAccount.MailMode == WebMailPro.MailMode.LeaveMessagesOnServer;
                tbUserMail.Text = usr.EmailAccount.Email;
                tbUserName.Text = usr.EmailAccount.MailIncomingLogin;
                tbPhone.Text = usr.Phone;
                tbFax.Text = usr.Fax;
                tbCity.Text = usr.City;
                tbAddress1.Text = usr.Address1;
                tbAddress2.Text = usr.Address2;
                tbPrimaryEmail.Text = usr.PrimaryEmail;
                tbZip.Text = usr.Zip;
                ddlState.SelectedValue = usr.StateId.ToString();
                trManagePhoto.Visible = usr.IsRetailEnabled;
                cbCompanyEmailSettings.Checked = usr.UseCompanyEmailSettings;
                tbPOP3Server.Text = usr.EmailAccount.MailIncomingHost;
                tbPOP3Port.Text = usr.EmailAccount.MailIncomingPort > 0 ? usr.EmailAccount.MailIncomingPort.ToString() : "";
                tbSMTPServer.Text = usr.EmailAccount.MailOutgoingHost;
                tbSMTPPort.Text = usr.EmailAccount.MailOutgoingPort > 0 ? usr.EmailAccount.MailOutgoingPort.ToString() : "";
                divServers.Attributes.Add(STYLEATTRIBUTE, usr.UseCompanyEmailSettings ? TRHIDDEN : TRVISIBLE);
                trWarning.Attributes.Add(STYLEATTRIBUTE, usr.UseCompanyEmailSettings ? TRHIDDEN : TRVISIBLE);
                BindRelations();
                BindLicense(usr);
            }
        }
        private void ClearTabs()
        {
            int n = rtsUser.Tabs.Count - FIXEDTABCOUNT;
            while (n>0)
            {
                rtsUser.Tabs.RemoveAt(rtsUser.Tabs.Count-1);
                rmpUser.PageViews.RemoveAt(rmpUser.PageViews.Count-1);
                n--;
            }
        }
        private void BindLicense(AppUser usr)
        {
            if (usr.IsInRole(AppUser.LOANOFFICERROLEID))
            {
                Tab tab = new Tab();
                tab.Text = "State Licensing";
                tab.ID = "tbStateLicensing";
                rtsUser.Tabs.Add(tab);
                PageView pv = new PageView();
                pv.ID = "pvStateLicensing";
                LoanOfficerStateLicensing ctl = LoadControl(Constants.CONTROLSLOCATION + Constants.CTLLOANOFFICERSTATELICENSING) as LoanOfficerStateLicensing;
                if (ctl != null)
                {
                    pv.Controls.Add(ctl);
                }
                rmpUser.PageViews.Add(pv);
            }
        }

        private void BindRelations()
        {
            DataView dv = ManagerRelation.GetManagersForUser(User.Id);
            ClearTabs();
            for (int i = 0; i < dv.Count; i++)
            {
                Tab tab = GetTab(dv[i]);
                rtsUser.Tabs.Add(tab);
                PageView pv = GetPageView(dv[i]);
                EditManagerRelation ctl = LoadControl(Constants.CONTROLSLOCATION + Constants.CTLEDITMANAGERRELATION) as EditManagerRelation;
                if(ctl != null)
                {
                    pv.Controls.Add(ctl);
                }
                rmpUser.PageViews.Add(pv);
            }
        }
        private void BindState()
        {
            ddlState.DataSource = CurrentPage.GetDictionary("vwState");
            ddlState.DataTextField = "name";
            ddlState.DataValueField = "id";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("- Select -", "0"));
        }
        #endregion

        
        private static Tab GetTab(DataRowView dr)
        {
            Tab res = new Tab();
            res.Text = dr["name"].ToString();
            res.ID = String.Format("rt{0}", dr["id"]);
            return res;
        }
        private PageView GetPageView(DataRowView dr)
        {
            PageView res = new PageView();
            int roleId = int.Parse(dr["id"].ToString());
            res.ID = String.Format("rt{0}", roleId);
            EditManagerRelation ctl = LoadControl(Constants.CONTROLSLOCATION + Constants.CTLEDITMANAGERRELATION) as EditManagerRelation;
            if(ctl!=null)
            {
                ctl.ID = String.Format("emr_{0}", roleId);
                res.Controls.Add(ctl);
            }
            return res;
        }
        private string GetManagersRelation()
        {
            string res = String.Empty;
            String[] col = Page.Request.Form.AllKeys;
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            for (int i = 0; i < col.Length; i++)
            {
                string s = col[i];
                if (s.EndsWith(DDLMANAGERID))
                {
                    int j = col[i].LastIndexOf('_');
                    if (j > 0)
                    {
                        
                        int userRoleId = -1;
                        try
                        {
                            userRoleId = int.Parse(s.Substring(j + 1).Replace(DDLMANAGERID, ""));
                        }
                        catch
                        {
                        }
                        if(userRoleId>0)
                        {
                            int val = -1;
                            try
                            {
                                val = int.Parse(Page.Request[col[i]]);
                            }
                            catch
                            {
                            }
                            if(val>0)
                            {
                                int managerId = val/100;
                                int managerRoleId = val - managerId*100;
                                XmlNode n = d.CreateElement(ITEMELEMENT);
                                XmlAttribute a = d.CreateAttribute(USERROLEIDATTRIBUTE);
                                a.Value = userRoleId.ToString();
                                n.Attributes.Append(a);
                                XmlAttribute a1 = d.CreateAttribute(MANAGERIDATTRIBUTE);
                                a1.Value = managerId.ToString();
                                n.Attributes.Append(a1);
                                XmlAttribute a2 = d.CreateAttribute(MANAGERROLEIDATTRIBUTE);
                                a2.Value = managerRoleId.ToString();
                                n.Attributes.Append(a2);
                                root.AppendChild(n);

                            }
                        }
                    }
                }
            }
            if (root.ChildNodes.Count > 0)
            {
                d.AppendChild(root);
                res = d.OuterXml;
            }
            return res;
        }
        private Hashtable GetRoles()
        {
            Hashtable res = new Hashtable();
            foreach (DataListItem item in dlRoles.Items)
            {
                CheckBox cb = (CheckBox)item.FindControl("Checkbox1");
                if (cb != null && cb.Checked)
                {
                    res.Add(dlRoles.DataKeys[item.ItemIndex], cb.Text);
                }
            }
            return res;
        }

        #endregion

        #region handlers
        protected void btnTest_Click(object sender, EventArgs e)
        {
            divServers.Attributes.Add(STYLEATTRIBUTE, cbCompanyEmailSettings.Checked ? TRHIDDEN : TRVISIBLE);
            SetPasswordsFields();
            int res = ValidateEmailData();
            if (res == -2)
            {
                lblTest.Text = "Please set email data";
                return;
            }
            if (res == -1) return;
            string smtpResult;
            string pop3Result;
            string from = tbUserMail.Text;
            string to = CurrentUser.EmailAddress;
            string smtpHost = tbSMTPServer.Text;
            int smtpPort = int.Parse(tbSMTPPort.Text);
            string login = tbUserName.Text;
            string password = tbMailPassword.Text;
            bool smtpAuth = true;
            string pop3Server = tbPOP3Server.Text;
            int pop3Port = int.Parse(tbPOP3Port.Text);
            WebMailHelper.TestEmailAccount(from, to, smtpHost, smtpPort, login, password, smtpAuth, pop3Server, pop3Port, out smtpResult, out pop3Result);
            if (String.IsNullOrEmpty(smtpResult))
            {
                lblTest.Text = "SMTP Server test completed successfully";
            }
            else
            {
                lblTest.Text = String.Format("SMTP Server Error: {0}", smtpResult);
            }
            if (String.IsNullOrEmpty(pop3Result))
            {
                lblTest.Text += "<br/>POP3 Server test completed successfully<br/>";
            }
            else
            {
                lblTest.Text = String.Format("<br/>POP3 Server Error: {0}", pop3Result);
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            goBack();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                AppUser user_ = User;
                user_.FirstName = tbFirstName.Text;
                user_.LastName = tbLastName.Text;
                user_.LoginName = tbLogin.Text;
                if (user_.Id < 0 || cbEnablePassword.Checked)
                {
                    if (user_.Password != tbPassword.Text)
                    {
                        user_.Password = tbPassword.Text;
                    }
                }
                user_.UseCompanyEmailSettings = cbCompanyEmailSettings.Checked;
                user_.EmailAccount.MailIncomingLogin = tbUserName.Text;
                user_.EmailAccount.Email = tbUserMail.Text;
                user_.PrimaryEmail = tbPrimaryEmail.Text;
                if (user_.UseCompanyEmailSettings)
                {
                    if (user_.Id < 0)
                    {
                        user_.LoadCompanyEmailSettings();
                    }
                    user_.EmailAccount.MailIncomingHost = user_.CompanyPOP3Server;
                    user_.EmailAccount.MailIncomingPort = user_.CompanyPOP3Port;
                    user_.EmailAccount.MailOutgoingHost = user_.CompanySMTPServer;
                    user_.EmailAccount.MailOutgoingPort = user_.CompanySMTPPort;
                }
                else
                {
                    user_.EmailAccount.MailIncomingHost = tbPOP3Server.Text;
                    user_.EmailAccount.MailIncomingPort = int.Parse(tbPOP3Port.Text);
                    user_.EmailAccount.MailOutgoingHost = tbSMTPServer.Text;
                    user_.EmailAccount.MailOutgoingPort = int.Parse(tbSMTPPort.Text);
                    user_.EmailAccount.MailMode = cbLeaveMessage.Checked
                                                      ? WebMailPro.MailMode.LeaveMessagesOnServer
                                                      : WebMailPro.MailMode.DeleteMessagesFromServer;
                }
                user_.EmailAccount.MailOutgoingAuthentication = true;
                user_.EmailAccount.MailIncomingLogin = tbUserMail.Text;
                user_.EmailAccount.MailOutgoingLogin = tbUserMail.Text;
                user_.EmailAccount.MailIncomingPassword = tbMailPassword.Text;
                user_.EmailAccount.MailOutgoingPassword = tbMailPassword.Text;
                user_.Phone = tbPhone.Text;
                user_.Fax = tbFax.Text;
                user_.City = tbCity.Text;
                user_.Address1 = tbAddress1.Text;
                user_.Address2 = tbAddress2.Text;
                user_.Zip = tbZip.Text;
                user_.StateId = int.Parse(ddlState.SelectedValue);
                user_.Roles = GetRoles();
                user_.ManagerXml = GetManagersRelation();
                if(user_.Save()>0)
                {
                    lblMessage.Text = Constants.SUCCESSMESSAGE;
                    User = user_;
                    BindData();
                }
                else
                {
                    lblMessage.Text = user_.LastError;
                }

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblPasswordRules.Text = Utils.PASSWORDRULESTEXT;
            cbCompanyEmailSettings.Attributes.Add(ONCLICKATTRIBUTE, String.Format(JSSHOWDIV, divServers.ClientID, trWarning.ClientID));
            if (!CurrentUser.IsAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            BindState();
            BindData();
            BindRoles();
            ClearError();
        }
        #endregion
    }
}