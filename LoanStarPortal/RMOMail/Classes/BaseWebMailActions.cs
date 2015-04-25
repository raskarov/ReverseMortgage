using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.UI;
using MailBee.Html;
using MailBee.ImapMail;
using MailBee.Mime;

namespace WebMailPro
{
    /// <summary>
    /// Summary description for BaseWebMailActions.
    /// </summary>
    public class BaseWebMailActions
    {
        protected Account _acct = null;
        protected Page _webPage = null;

        public Account CurrentAccount
        {
            get { return _acct; }
        }

        public Page CurrentPage
        {
            get { return _webPage; }
        }

        public BaseWebMailActions(Account acct, Page page)
        {
            _acct = acct;
            _webPage = page;
        }

        public void AddContacts(int id_group, AddressBookContact[] contacts)
        {
            DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(CurrentAccount);
            try
            {
                dbStorage.Connect();
                dbStorage.AddContactsToGroup(id_group, contacts);
            }
            finally
            {
                dbStorage.Disconnect();
            }
        }

        public void DeleteAccount(int id_acct)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                Account[] userAccouts = _acct.UserOfAccount.GetUserAccounts();
                if (userAccouts.Length == 1)
                {
                    Account.DeleteFromDb(_acct);
                    User.DeleteUser(_acct.IDUser);
                    _acct = null;
                    return;
                }
                else
                {
                    Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID);
                    if (acct != null)
                    {
                        if (acct.DefaultAccount)
                        {
                            bool hasDefault = false;
                            foreach (Account account in userAccouts)
                            {
                                if (account.ID != acct.ID)
                                {
                                    if (account.DefaultAccount)
                                    {
                                        hasDefault = true;
                                        break;
                                    }
                                }
                            }
                            if (!hasDefault)
                            {
								throw new WebMailException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("ACCT_CANT_DEL_LAST_DEF_ACCT"));
                            }
                        }
                        Account.DeleteFromDb(acct);
                        if (_acct.ID == id_acct)
                        {
                            userAccouts = _acct.UserOfAccount.GetUserAccounts();
                            if (userAccouts.Length > 0)
                            {
                                _acct = userAccouts[0];
                            }
                            else
                            {
                                _acct = null;
                            }
                        }
                    }
                }
			}//if ((_acct != null) && (_acct.UserOfAccount != null))
            else
            {
                if (_acct == null)
					Log.WriteLine("DeleteAccount", "Account is null.");
                else if (_acct.UserOfAccount == null)
					Log.WriteLine("DeleteAccount", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void DeleteContacts(AddressBookContact[] contacts, AddressBookGroup[] groups)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(_acct);
                try
                {
                    dbStorage.Connect();
                    dbStorage.DeleteAddressBookContactsGroups(contacts, groups);
                }
                finally
                {
                    dbStorage.Disconnect();
                }
            }
            else
            {
                if (_acct == null)
                    Log.WriteLine("DeleteContacts", "Account is null.");
                else if (_acct.UserOfAccount == null)
                    Log.WriteLine("DeleteContacts", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void DeleteFilter(int id_filter, int id_acct)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID);
                if (acct != null)
                {
                    DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(acct);
                    try
                    {
                        dbStorage.Connect();
                        dbStorage.DeleteFilter(id_filter);
                    }
                    finally
                    {
                        dbStorage.Disconnect();
                    }
                }
            }
            else
            {
                if (_acct == null)
                    Log.WriteLine("DeleteFilter", "Account is null.");
                else if (_acct.UserOfAccount == null)
                    Log.WriteLine("DeleteFilter", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void DeleteFolders(int id_acct, Folder[] folders)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID);
                if (acct != null)
                {
                    MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(acct));
                    try
                    {
                        mp.Connect();
                        mp.DeleteFolders(folders);
                    }
                    finally
                    {
                        mp.Disconnect();
                    }
                }
            }
            else
            {
                if (_acct == null)
                    Log.WriteLine("DeleteFolders", "Account is null.");
                else if (_acct.UserOfAccount == null)
                    Log.WriteLine("DeleteFolders", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public Account GetAccount(int id_acct)
        {
            Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID);
            if (acct == null)
            {
				if (_acct == null)
				{
					Log.WriteLine("GetAccount", "Account is null.");
					throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
				}
				else if (_acct.UserOfAccount == null)
				{
					Log.WriteLine("GetAccount", "User is null.");
					throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
				}
				acct = _acct;
            }
            return acct;
        }

        public Account[] GetAccounts(int user_id)
        {
            User u = User.LoadUser(user_id);
            if (u != null)
            {
                return u.GetUserAccounts();
            }
            return null;
        }

        public AddressBookContact GetContact(long id_addr)
        {
            if (_acct != null)
            {
                DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(_acct);
                try
                {
                    dbStorage.Connect();
                    return dbStorage.GetAddressBookContact(id_addr);
                }
                finally
                {
                    dbStorage.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("GetContact", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public AddressBookGroupContact[] GetContactsGroups(int page, short sort_field, short sort_order, int id_group, string look_for, int look_for_type, out int contacts_count, out int groups_count)
        {
            if (_acct != null)
            {
                if (look_for == null) look_for = string.Empty;
                DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(_acct);
                try
                {
                    dbStorage.Connect();

                    contacts_count = dbStorage.GetAddressBookContactsCount(look_for, look_for_type);
                    groups_count = dbStorage.GetAddressBookGroupsCount(look_for, look_for_type);

                    return dbStorage.LoadAddressBookContactsGroups(page, sort_field, sort_order, id_group, look_for, look_for_type);
                }
                finally
                {
                    dbStorage.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("GetContactsGroups", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public Filter GetFilter(int id_filter, int id_acct)
        {
            Filter flt = null;
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID);
                if (acct != null)
                {
                    DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(acct);
                    try
                    {
                        dbStorage.Connect();
                        flt = dbStorage.GetFilter(id_filter);
                    }
                    finally
                    {
                        dbStorage.Disconnect();
                    }
                }
            }
            else
            {
                if (_acct == null)
                    Log.WriteLine("GetFilter", "Account is null.");
                else if (_acct.UserOfAccount == null)
                    Log.WriteLine("GetFilter", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            return flt;
        }

        public Filter[] GetFilters(int id_acct)
        {
            Filter[] flts = null;
            Account acct = _acct;
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID);
            }
            else
            {
                if (_acct == null)
					Log.WriteLine("GetFilters", "Account is null.");
                else if (_acct.UserOfAccount == null)
					Log.WriteLine("GetFilters", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            if (acct != null)
            {
                DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(acct);
                try
                {
                    dbStorage.Connect();
                    flts = dbStorage.GetFilters();
                }
                finally
                {
                    dbStorage.Disconnect();
                }
            }
            return flts;
        }

        public FolderCollection GetFoldersList(int id_acct, int sync)
        {
            Account acct;
            FolderCollection fc = new FolderCollection();
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID);
            }
            else
            {
                if (_acct == null)
					Log.WriteLine("GetFoldersList", "Account is null.");
                else if (_acct.UserOfAccount == null)
					Log.WriteLine("GetFoldersList", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            if (acct != null)
            {
                if (sync != -1)
                {
                    if (_acct.UserOfAccount != null) _acct = acct;
                }

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
            }
            return fc;
        }

        public AddressBookGroup GetGroup(int id_group)
        {
            AddressBookGroup group = null;
            if (_acct != null)
            {
                DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(_acct);
                try
                {
                    dbStorage.Connect();
                    group = dbStorage.GetAddressBookGroup(id_group);
                }
                finally
                {
                    dbStorage.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("GetGroup", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            return group;
        }

        public AddressBookGroup[] GetGroups()
        {
            AddressBookGroup[] groups = null;
            if (_acct != null)
            {
                DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(_acct);
                try
                {
                    dbStorage.Connect();
                    groups = dbStorage.GetAddressBookGroups();
                }
                finally
                {
                    dbStorage.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("GetGroups", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            return groups;
        }

        public string GetSignature(int id_acct)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID);
                if (acct != null)
                {
                    return Utils.EncodeHtml(acct.Signature);
                }
            }
            else
            {
                if (_acct == null)
                    Log.WriteLine("GetSignature", "Account is null.");
                else if (_acct.UserOfAccount == null)
                    Log.WriteLine("GetSignature", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            return string.Empty;
        }

        public bool GetXSpam()
        {
            if ((_acct.UserOfAccount != null) && (_acct.UserOfAccount.Settings != null))
            {
                return _acct.UserOfAccount.Settings.XSpam;
            }
            else
            {
                if (_acct.UserOfAccount == null)
                    Log.WriteLine("UpdateXSpam", "User is null.");
                else if (_acct.UserOfAccount.Settings == null)
                    Log.WriteLine("UpdateXSpam", "User is null.");
                throw new WebMailException("User is null.");
            }
        }

        public Folder GetFolder(string name)
        {
            MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
            try
            {
                mp.Connect();
                return mp.GetFolder(name);
            }
            finally
            {
                mp.Disconnect();
            }
        }

        public Folder GetFolder(long id_folder)
        {
            MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
            try
            {
                mp.Connect();
                return mp.GetFolder(id_folder);
            }
            finally
            {
                mp.Disconnect();
            }
        }

        public Folder GetFolder(FolderType type)
        {
            MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
            try
            {
                mp.Connect();
                return mp.GetFolder(type);
            }
            finally
            {
                mp.Disconnect();
            }
        }

        public WebMailMessage GetMessage(int id, string uid, long id_folder, string full_name_folder, int charset)
        {
            byte safety = 0;
            return GetMessage(id, uid, id_folder, full_name_folder, charset, safety);
        }

        static bool AHrefProcessDelegate(Element elem, MailBee.Html.Rule rule)
        {
            if (elem.TagName.ToLower() == "a")
            {
                MailBee.Html.TagAttribute attr = elem.GetAttributeByName("href");
				string hrefValue = (attr != null && attr.Value != null && attr.Value.Length > 1) ? attr.Value[1].ToString() : "";
                if (hrefValue == "#")
                {
                    return false;
                }
            }

            return true;
        }


        public WebMailMessage GetMessage(int id, string uid, long id_folder, string full_name_folder, int charset, byte safety)
        {
            WebMailMessage msg = null;
            MailBee.Mime.MailMessage outputMsg = null;
            if (_acct != null)
            {
                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
                try
                {
                    mp.Connect();
                    Folder fld = mp.GetFolder(id_folder);
                    if (fld != null)
                    {
                        msg = mp.GetMessage((fld.SyncType != FolderSyncType.DirectMode) ? (object)id : uid, fld);
                        if (msg != null)
                        {
                            msg.OverrideCharset = charset;
                            mp.UpdateMessage(msg);
                            if ((CurrentAccount.UserOfAccount != null) && (CurrentAccount.UserOfAccount.Settings != null))
                            {
                                CurrentAccount.UserOfAccount.Settings.DefaultCharsetInc = charset;
                                CurrentAccount.Update(true);
                            }
                            if (msg.MailBeeMessage != null)
                            {
                                outputMsg = msg.MailBeeMessage.Clone();

                                if (outputMsg != null)
                                {
                                    outputMsg.Parser.EncodingOverride = (charset > 0) ? Utils.GetEncodingByCodePage(charset) : null;
                                    outputMsg.Parser.Apply();
                                }
                                msg.Init(outputMsg, ((msg.StrUid != null) && (msg.StrUid.Length > 0)), fld);
                                mp.UpdateMessage(msg);
                            }
                        }
                    }
                }
                finally
                {
                    mp.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("GetMessage", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }

            if ((msg != null) && (outputMsg != null))
            {
                if (_acct.UserOfAccount.GetSenderSafety(outputMsg.From.Email) == 1) safety = 1;
                if (outputMsg.Attachments.Count > 0)
                {
                    if (_webPage != null)
                    {
						string filename = string.Empty;
						string tempFolder = Utils.GetTempFolderName(_webPage.Session);
						outputMsg.Parser.WorkingFolder = tempFolder;
						outputMsg.BodyHtmlText = outputMsg.GetHtmlAndSaveRelatedFiles(@"get_attachment_binary.aspx?filename=", VirtualMappingType.Static, MessageFolderBehavior.DoNotCreate);
						for (int i = 0; i < outputMsg.Attachments.Count; i++)
						{
							filename = Utils.CreateTempFilePath(tempFolder,
								(outputMsg.Attachments[i].Filename.Length > 0) ? outputMsg.Attachments[i].Filename : outputMsg.Attachments[i].Name);
							outputMsg.Attachments[i].Save(filename, true);
						}
                    }
                }

                if ((outputMsg.BodyHtmlText != null) && (outputMsg.BodyHtmlText.Length > 0))
                {
                    string htmlBody = outputMsg.BodyHtmlText;
                    // MailBee.Html
                    Processor pr = new Processor();
                    // MailBee.Html
                    pr.Dom.OuterHtml = htmlBody;
                    RuleSet rs = new RuleSet();

                    string[] removeAttributes = new string[] { "onActivate", "onAfterPrint",
								"onBeforePrint", "onAfterUpdate", "onBeforeUpdate", "onErrorUpdate",
								"onAbort", "onBeforeDeactivate", "onDeactivate", "onBeforeCopy",
								"onBeforeCut", "onBeforeEditFocus", "onBeforePaste", "onBeforeUnload",
								"onBlur", "onBounce", "onChange", "onClick", "onControlSelect",
								"onCopy", "onCut", "onDblClick", "onDrag", "onDragEnter", "onDragLeave",
								"onDragOver", "onDragStart", "onDrop", "onFilterChange", "onDragDrop",
								"onError", "onFilterChange", "onFinish", "onFocus", "onHelp", "onKeyDown",
								"onKeyPress", "onKeyUp", "onLoad", "onLoseCapture", "onMouseDown",
								"onMouseEnter", "onMouseLeave", "onMouseMove", "onMouseOut",
								"onMouseOver", "onMouseUp", "onMove", "onPaste",
								"onPropertyChange", "onReadyStateChange", "onReset", "onResize",
								"onResizeEnd", "onResizeStart", "onScroll", "onSelectStart",
								"onSelect", "onSelectionChange", "onStart", "onStop", "onSubmit",
								"onUnload"};
					
                    TagAttributeCollection attrsToRemove = new TagAttributeCollection();
                    foreach (string removeAttribute in removeAttributes)
                    {
                        TagAttribute remAttr = new TagAttribute();
                        remAttr.Name = removeAttribute;
                        attrsToRemove.Add(remAttr);
                    }
                    rs.AddTagProcessingRule(".*", null, null, attrsToRemove, false);
					
                    rs.AddTagRemovalRule("head", null);
                    rs.AddTagRemovalRule("title", null);
                    rs.AddTagRemovalRule("style", null);
                    rs.AddTagRemovalRule("script", null);
                    rs.AddTagRemovalRule("object", null);
                    rs.AddTagRemovalRule("embed", null);
                    rs.AddTagRemovalRule("applet", null);
                    rs.AddTagRemovalRule("mocha", null);
                    rs.AddTagRemovalRule("base", null);

                    if (safety == 0)
                    {
                        safety = 1; // sender not safety, but HTML may be safety

                        #region BGImagesReplacement
                        ElementReadOnlyCollection els = pr.Dom.GetAllElements();
                        foreach (Element el in els)
                        {
                            if (el.Attributes != null)
                            {
                                foreach (MailBee.Html.TagAttribute att in el.Attributes)
                                {
                                    if (att.Name.ToLower() == "style")
                                    {
                                        Regex re = new Regex(@"background", RegexOptions.IgnoreCase);
                                        att.Value = re.Replace(att.Value, "wmx_background");
                                    }
                                    if (att.Name.ToLower() == "background")
                                    {
                                        att.Name = att.Name.Insert(0, "wmx_");
                                    }
                                }
                            }
                        }
                        #endregion

                        ElementCollection images = pr.Images;
                        foreach (Element imageElement in images)
                        {
                            TagAttributeCollection searchAttrs = new TagAttributeCollection();
                            TagAttributeCollection addAttrs = new TagAttributeCollection();
                            TagAttributeCollection removeAttrs = new TagAttributeCollection();

                            foreach (TagAttribute attr in imageElement.Attributes)
                            {
                                if ((attr.Name != null) && (attr.Value != null))
                                {
                                    string attrValue = attr.Value.Trim(new char[] { '\'', '"' });
                                    if (attr.Name.ToLower() == "src")
                                    {
                                        if (!attrValue.StartsWith("\"get_attachment_binary.aspx"))
                                        {
                                            safety = 0; // unfortunately, HTML not safety
                                            // external images
                                            MailBee.Html.TagAttribute searchAttr = new TagAttribute();
                                            searchAttr.Name = attr.Name;
                                            searchAttr.Value = Regex.Escape(attr.Value);

                                            searchAttrs.Add(searchAttr);

                                            MailBee.Html.TagAttribute oldAttr = new TagAttribute();
                                            oldAttr.Definition = attr.Name;
                                            removeAttrs.Add(oldAttr);

                                            MailBee.Html.TagAttribute newAttr = new TagAttribute();
                                            //newAttr.Definition = "src='about:blank'";
                                            newAttr.Name = string.Format(@"wmx_{0}", attr.Name);
                                            newAttr.Value = attr.Value;
                                            addAttrs.Add(newAttr);

                                            break;
                                        }
                                    }
                                }
                            }
                            rs.AddTagProcessingRule("img", searchAttrs, addAttrs, searchAttrs, true);
                        }
                    }
                    msg.Safety = safety;
					
                    pr.Dom.Process(rs, null);

                    TagAttributeCollection attrsToAdd = new TagAttributeCollection();
                    TagAttribute addAttr = new MailBee.Html.TagAttribute();
                    addAttr.Name = "target";
                    addAttr.Value = "\"_blank\"";
                    attrsToAdd.Add(addAttr);

                    rs.Clear();
                    rs.AddTagProcessingRule("a", null, attrsToAdd, null, false);
                    ProcessElementDelegate del = new ProcessElementDelegate(AHrefProcessDelegate);
                    
                    outputMsg.BodyHtmlText = pr.Dom.ProcessToString(rs, del);
                }
                else
                {
                    msg.Safety = 1;
                }

                if (outputMsg != null)
                {
                    if ((outputMsg.BodyPlainText == null) || (outputMsg.BodyPlainText.Length == 0))
                    {
                        outputMsg.MakePlainBodyFromHtmlBody();
                    }
                }
            }
            else
            {
                throw new WebMailException("Message not exists.");
            }
            return msg;
        }

        public WebMailMessageCollection GetMessages(long id_folder, string full_name_folder, int page, int sort_field, int sort_order, string look_for, int search_fields, out int folderMessageCount, out int folderUnreadMessageCount)
        {
            if (_acct != null)
            {
                _acct.DefaultOrder = (DefaultOrder)(sort_field + sort_order);
                _acct.Update(false);

                WebMailMessageCollection msgsColl = new WebMailMessageCollection();
                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
                folderMessageCount = 0;
                folderUnreadMessageCount = 0;
                try
                {
                    mp.Connect();

                    FolderCollection fldColl = new FolderCollection();
                    Folder fld = null;
                    if (full_name_folder != null)
                    {
                        if ((id_folder == -1) && (full_name_folder.Length == 0))
                        {
                            fldColl = mp.GetFolders(true);
                            FolderCollection viewedFolders = new FolderCollection();
                            foreach (Folder f in fldColl)
                            {
                                if (!f.Hide) viewedFolders.Add(f);
                            }
                            fldColl = viewedFolders;
                        }
                        else
                        {
                            fld = mp.GetFolder(id_folder);
                            if (fld != null)
                            {
                                fldColl.Add(fld);
                            }
                        }
                    }

                    if ((look_for != null) && (look_for.Length > 0))
                    {
                        // search messages
                        msgsColl = mp.SearchMessages(page, look_for, fldColl, (search_fields == 0) ? true : false, out folderMessageCount);
                    }
                    else
                    {
                        // load messages
                        if (fld != null)
                        {
                            folderMessageCount = mp.GetFolderMessageCount(fld);
                            folderUnreadMessageCount = mp.GetFolderUnreadMessageCount(fld);
                            msgsColl = mp.GetMessageHeaders(page, fld);
                        }
                    }
                }
                finally
                {
                    mp.Disconnect();
                }
                return msgsColl;
            }
            else
            {
                Log.WriteLine("GetMessages", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public Account LoginAccount(string email, string login, string password, string inc_server, IncomingMailProtocol inc_protocol, int inc_port, string out_server, int out_port, bool smtp_auth, bool sign_me, bool advanced_login)
        {
            try
            {
                _acct = Account.LoginAccount(email, login, password, inc_server, inc_protocol, inc_port, out_server, out_port, smtp_auth, sign_me, advanced_login);

                if ((_acct == null) || (_acct.UserOfAccount == null) || (_acct.UserOfAccount.Settings == null))
                {
                    if (_acct == null)
                        Log.WriteLine("LoginAccount", "Account is null.");
                    else if (_acct.UserOfAccount == null)
                        Log.WriteLine("LoginAccount", "User is null.");
                    throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
                }
                return _acct;
            }
            catch (WebMailException ex)
            {
                throw ex;
            }
        }

        public int NewAccount(bool def_acct, IncomingMailProtocol mail_protocol, int mail_inc_port, int mail_out_port, bool mail_out_auth, bool use_friendly_name, short mails_on_server_days, MailMode mail_mode, bool getmail_at_login, int inbox_sync_type, string friendly_nm, string email, string mail_inc_host, string mail_inc_login, string mail_inc_pass, string mail_out_host, string mail_out_login, string mail_out_pass)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return -1;

				WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

				if (settings.EnableWmServer)
				{
					string emailDomain = EmailAddress.GetDomainFromEmail(email);
					string emailUser = EmailAddress.GetAccountNameFromEmail(email);
					WMServerStorage storage = new WMServerStorage(null);
					string[] domains = storage.GetDomainList();
					foreach (string domain in domains)
					{
						if (string.Compare(emailDomain, domain, true, CultureInfo.InvariantCulture) == 0)
						{
							mail_protocol = IncomingMailProtocol.WMServer;
							mail_out_login = email;
							mail_out_pass = mail_inc_pass;
							mail_out_host = settings.WmServerHost;
							mail_out_port = settings.XMailSmtpPort;

							if (settings.WmAllowManageXMailAccounts)
							{
								WMServerUser[] users = storage.GetUserList(emailDomain);
								bool userExists = false;
								foreach (WMServerUser user in users)
								{
									if (string.Compare(emailUser, user.Name, true, CultureInfo.InvariantCulture) == 0)
									{
										userExists = true;
										break;
									}
								}
								if (!userExists)
								{
									storage.AddUser(emailDomain, emailUser, mail_inc_pass);
								}
							}
							break;
						}
					}
				}

				Account acct = new Account();
                FolderSyncType inboxSyncType = (FolderSyncType)inbox_sync_type;
                acct = _acct.UserOfAccount.CreateAccount(def_acct, false, email, mail_protocol, mail_inc_host, mail_inc_login, mail_inc_pass, mail_inc_port, mail_out_host, mail_out_login, mail_out_pass, mail_out_port, mail_out_auth, friendly_nm, use_friendly_name, acct.DefaultOrder, getmail_at_login, mail_mode, mails_on_server_days, string.Empty, acct.SignatureType, acct.SignatureOptions, acct.Delimiter, 0, inboxSyncType, false);
                return (acct != null) ? acct.ID : -1;
            }
            else
            {
                if (_acct == null)
					Log.WriteLine("NewAccount", "Account is null.");
                else if (_acct.UserOfAccount == null)
					Log.WriteLine("NewAccount", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void NewContact(AddressBookContact newContact)
        {
            if (_acct != null)
            {
                DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(_acct);
                try
                {
                    dbStorage.Connect();
                    dbStorage.CreateAddressBookContact(newContact);
                }
                finally
                {
                    dbStorage.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("NewContact", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void NewFilter(int id_acct, byte field, byte condition, byte action, long id_folder, string filter)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                if ((filter == null) || (filter.Trim().Length == 0))
                {
                    WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
                    throw new WebMailException(resMan.GetString("WarningEmptyFilter"));
                }

                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID);
                if (acct != null)
                {
                    DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(acct);
                    try
                    {
                        dbStorage.Connect();
                        dbStorage.CreateFilter(field, condition, filter, action, id_folder);
                    }
                    finally
                    {
                        dbStorage.Disconnect();
                    }
                }
            }
            else
            {
                if (_acct == null)
                    Log.WriteLine("NewFilter", "Account is null.");
                else if (_acct.UserOfAccount == null)
                    Log.WriteLine("NewFilter", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void NewFolder(int id_acct, int id_parent, string full_name_parent, string name, int create)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID);
                if (acct != null)
                {
                    MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(acct));
                    try
                    {
                        mp.Connect();
                        mp.CreateFolder(id_parent, full_name_parent, name, create);
                    }
                    finally
                    {
                        mp.Disconnect();
                    }
                }
            }
            else
            {
                if (_acct == null)
                    Log.WriteLine("NewFolder", "Account is null.");
                else if (_acct.UserOfAccount == null)
                    Log.WriteLine("NewFolder", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void NewGroup(AddressBookGroup group)
        {
            if (_acct != null)
            {
                DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(_acct);
                try
                {
                    dbStorage.Connect();
                    dbStorage.CreateAddressBookGroup(group);
                }
                finally
                {
                    dbStorage.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("NewGroup", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public int SaveMessageToDrafts(WebMailMessage message)
        {
            int msgID = -1;
            if (_acct != null)
            {
                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
                try
                {
                    mp.Connect();

                    Folder draftFolder = mp.GetFolder(FolderType.Drafts);
                    if (draftFolder != null)
                    {
                        WebMailMessage msg = message;
                        msg.Seen = true;

                        if (message.IDMsg > 0)
                        {
                            mp.DeleteMessages(new object[] { message.IDMsg }, draftFolder);
                        }

                        //mp.SaveMessage(msg, draftFolder);
                        msgID = mp.SaveMessageAndGetId(msg, draftFolder);
                    }
                }
                finally
                {
                    mp.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("SaveMessage", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            return msgID;
        }

        public int SaveMessageInbox(WebMailMessage message)
        {
            int msgID = -1;
            if (_acct != null)
            {
                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
                try
                {
                    mp.Connect();

                    Folder inboxFolder = mp.GetFolder(FolderType.Inbox);
                    if (inboxFolder != null)
                    {
                        WebMailMessage msg = message;
                        msg.Seen = true;

                        //if (message.IDMsg > 0)
                        //{
                            mp.DeleteMessages(new object[] { message.IDMsg }, inboxFolder);
                        //}

                        msgID = mp.SaveMessageAndGetId(msg, inboxFolder);
                    }
                }
                finally
                {
                    mp.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("SaveMessage", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            return msgID;
        }

        public int SaveMessageSentItems(WebMailMessage message)
        {
            int msgID = -1;
            if (_acct != null)
            {
                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
                try
                {
                    mp.Connect();

                    Folder sentitemsFolder = mp.GetFolder(FolderType.SentItems);
                    if (sentitemsFolder != null)
                    {
                        WebMailMessage msg = message;
                        msg.Seen = true;

                        //if (message.IDMsg > 0)
                        //{
                            mp.DeleteMessages(new object[] { message.IDMsg }, sentitemsFolder);
                        //}

                        msgID = mp.SaveMessageAndGetId(msg, sentitemsFolder);
                    }
                }
                finally
                {
                    mp.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("SaveMessage", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            return msgID;
        }

        public void SendMessage(WebMailMessage msg)
        {
            if (_acct != null)
            {
                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
                try
                {
                    mp.Connect();

                    mp.SendMail(msg);

                    Folder sentFld = mp.GetFolder(FolderType.SentItems);
                    if (sentFld != null)
                    {
                        msg.Seen = true;
                        try
                        {
                            mp.SaveMessage(msg, sentFld);
                        }
                        catch (WebMailMailBoxException ex) { Log.WriteException(ex); }
                    }

                    Folder draftsFld = mp.GetFolder(FolderType.Drafts);
                    if (msg.IDMsg > 0 && draftsFld != null)
                    {
                        mp.PurgeMessages(new object[] { msg.IDMsg }, draftsFld);
                    }
                }
                finally
                {
                    mp.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("NewMessage", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void UpdateAccount(int id_acct, bool def_acct, IncomingMailProtocol mail_protocol, int mail_inc_port, int mail_out_port, bool mail_out_auth, bool use_friendly_name, short mails_on_server_days, MailMode mail_mode, bool getmail_at_login, int inbox_sync_type, string friendly_nm, string email, string mail_inc_host, string mail_inc_login, string mail_inc_pass, string mail_out_host, string mail_out_login, string mail_out_pass)
        {
            bool allowChangeSettings = false;
            int id_user = -1;
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                id_user = _acct.UserOfAccount.ID;
                allowChangeSettings = (_acct.UserOfAccount.Settings != null) ? _acct.UserOfAccount.Settings.AllowChangeSettings : false;
            }
            else
            {
                if (_acct == null)
					Log.WriteLine("UpdateAccount", "Account is null.");
                else if (_acct.UserOfAccount == null)
					Log.WriteLine("UpdateAccount", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            if (allowChangeSettings)
            {
                Account acct = Account.LoadFromDb(id_acct, id_user);
                if (acct != null)
                {
                    acct.ChangeAccountDefault(def_acct);
                    acct.MailIncomingProtocol = mail_protocol;
                    acct.MailIncomingPort = mail_inc_port;
                    acct.MailOutgoingPort = mail_out_port;
                    acct.MailOutgoingAuthentication = mail_out_auth;
                    acct.UseFriendlyName = use_friendly_name;
                    acct.MailsOnServerDays = mails_on_server_days;
                    acct.GetMailAtLogin = getmail_at_login;
                    if (acct.MailIncomingProtocol != IncomingMailProtocol.Imap4)
                    {
                        acct.MailMode = mail_mode;
                        MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(acct));
                        try
                        {
                            mp.Connect();
                            Folder fld = mp.GetFolder(FolderType.Inbox);
                            if (fld != null)
                            {
                                fld.SyncType = (FolderSyncType)inbox_sync_type;
                                mp.UpdateFolders(new Folder[] { fld }, true);
                            }
                        }
                        finally
                        {
                            mp.Disconnect();
                        }
                    }
                    acct.FriendlyName = friendly_nm;
                    if (string.Compare(acct.Email, email, true, CultureInfo.InvariantCulture) != 0)
                    {
                        FileSystem fs = new FileSystem(acct.Email, acct.ID, true);
                        fs.RenameAccount(email);
                        acct.Email = email;
                    }
                    acct.MailIncomingHost = mail_inc_host;
                    acct.MailIncomingLogin = mail_inc_login;
                    if (string.Compare(mail_inc_pass, Constants.nonChangedPassword, true, CultureInfo.InvariantCulture) != 0)
                    {
                        Log.WriteLine("UpdateAccount", string.Format("Change password. Old: '{0}'. New: '{1}'", acct.MailIncomingPassword, mail_inc_pass));
                        acct.MailIncomingPassword = mail_inc_pass;
                        _acct.MailIncomingPassword = mail_inc_pass;
                    }
                    acct.MailOutgoingHost = ((mail_out_host != null) && (mail_out_host.Length > 0)) ? mail_out_host : mail_inc_host;
                    acct.MailOutgoingLogin = mail_out_login.Trim();
                    if (string.Compare(mail_out_pass, Constants.nonChangedPassword, true, CultureInfo.InvariantCulture) != 0)
                    {
                        acct.MailOutgoingPassword = mail_out_pass;
                        _acct.MailOutgoingPassword = mail_out_pass;
                    }
                    acct.Update(false);

                    if (_acct.ID == id_acct)
                    {
                        _acct = acct;
                    }
                }
            }
        }

        public void UpdateContact(AddressBookContact updateContact)
        {
            if (_acct != null)
            {
                DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(_acct);
                try
                {
                    dbStorage.Connect();
                    dbStorage.UpdateAddressBookContact(updateContact);
                }
                finally
                {
                    dbStorage.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("UpdateContact", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void UpdateContactsSettings(bool whiteListing, short contactsPerPage)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null) && (_acct.UserOfAccount.Settings != null))
            {
                _acct.UserOfAccount.Settings.WhiteListing = whiteListing;
                _acct.UserOfAccount.Settings.ContactsPerPage = contactsPerPage;
                _acct.Update(true);
            }
            else
            {
                if (_acct == null)
                    Log.WriteLine("UpdateContactSettings", "User is null.");
                else if (_acct.UserOfAccount == null)
                    Log.WriteLine("UpdateContactsSettings", "User is null.");
                else if (_acct.UserOfAccount.Settings == null)
                    Log.WriteLine("UpdateContactsSettings", "User is null.");
                throw new WebMailException("User is null.");
            }
        }

        public void UpdateCookieSettings(bool hide_folders, short horiz_resizer, short vert_resizer, byte mark, byte reply, UserColumn[] columns)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null) && (_acct.UserOfAccount.Settings != null))
            {
                if (_acct.IsDemo) return;

                _acct.UserOfAccount.Settings.HideFolders = hide_folders;
                _acct.UserOfAccount.Settings.HorizResizer = horiz_resizer;
                _acct.UserOfAccount.Settings.VertResizer = vert_resizer;
                _acct.UserOfAccount.Settings.Mark = mark;
                _acct.UserOfAccount.Settings.Reply = reply;
                _acct.UserOfAccount.Columns = columns;
                _acct.Update(true);
            }
            else
            {
                if (_acct == null)
					Log.WriteLine("UpdateCookieSettings", "Account is null.");
                else if (_acct.UserOfAccount == null)
					Log.WriteLine("UpdateCookieSettings", "User is null.");
				else if (_acct.UserOfAccount.Settings == null)
					Log.WriteLine("UpdateCookieSettings", "User Settings is null.");
				throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void UpdateFilter(int id_acct, int id_filter, byte field, byte condition, byte action, long id_folder, string filter)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                if ((filter == null) || (filter.Trim().Length == 0))
                {
                    WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
                    throw new WebMailException(resMan.GetString("WarningEmptyFilter"));
                }

                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID);
                if (acct != null)
                {
                    DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(acct);
                    try
                    {
                        dbStorage.Connect();
                        dbStorage.UpdateFilter(id_filter, field, condition, filter, action, id_folder);
                    }
                    finally
                    {
                        dbStorage.Disconnect();
                    }
                }
            }
            else
            {
                if (_acct == null)
                    Log.WriteLine("UpdateFilter", "Account is null.");
                else if (_acct.UserOfAccount == null)
                    Log.WriteLine("UpdateFilter", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void UpdateFolders(int id_acct, /*XmlPacketFolder*/Folder[] folders)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID);
                if (acct != null)
                {
                    MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(acct));
                    try
                    {
                        mp.Connect();
                        mp.UpdateFolders(folders, true);
                    }
                    finally
                    {
                        mp.Disconnect();
                    }
                }
            }
            else
            {
                if (_acct == null)
                    Log.WriteLine("NewFolder", "Account is null.");
                else if (_acct.UserOfAccount == null)
                    Log.WriteLine("NewFolder", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void UpdateGroup(AddressBookGroup group)
        {
            if (_acct != null)
            {
                DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(_acct);
                try
                {
                    dbStorage.Connect();
                    dbStorage.UpdateAddressBookGroup(group);
                }
                finally
                {
                    dbStorage.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("UpdateGroup", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void UpdateSettings(bool allow_dhtml_editor, int def_charset_inc, int def_charset_out, string def_date_fmt, string def_lang, string def_skin, short def_timezone, short msgs_per_page, byte view_mode, TimeFormats time_fmt)
        {
            WebmailSettings webSettings = (new WebMailSettingsCreator()).CreateWebMailSettings();
            if ((_acct != null) && (_acct.UserOfAccount != null) && (_acct.UserOfAccount.Settings != null))
            {
                _acct.UserOfAccount.Settings.AllowDhtmlEditor = allow_dhtml_editor;
                if (webSettings.AllowUsersChangeCharset)
                {
                    _acct.UserOfAccount.Settings.DefaultCharsetInc = def_charset_inc;
                    _acct.UserOfAccount.Settings.DefaultCharsetOut = def_charset_out;
                }
                def_date_fmt = def_date_fmt.Trim();
                if ((def_date_fmt != null) && (def_date_fmt.Length > 0)) _acct.UserOfAccount.Settings.DefaultDateFormat = def_date_fmt;
                _acct.UserOfAccount.Settings.DefaultTimeFormat = time_fmt;

                if (webSettings.AllowUsersChangeLanguage)
                {
                    _acct.UserOfAccount.Settings.DefaultLanguage = def_lang;
                }
                if (webSettings.AllowUsersChangeSkin)
                {
                    string newSkin = "";
                    string[] skins = Utils.GetSupportedSkins(_webPage.MapPath("skins"));
                    if (Utils.GetCurrentSkinIndex(skins, def_skin) > -1)
                    {
                        newSkin = def_skin;
                    }
                    else
                    {
                        if (Utils.GetCurrentSkinIndex(skins, webSettings.DefaultSkin) > -1)
                        {
                            newSkin = webSettings.DefaultSkin;
                        }
                        else if (skins.Length > 0)
                        {
                            newSkin = skins[0];
                        }
                    }
                    _acct.UserOfAccount.Settings.DefaultSkin = newSkin;
                }
                if (webSettings.AllowUsersChangeTimeZone)
                {
                    _acct.UserOfAccount.Settings.DefaultTimeZone = def_timezone;
                }
                _acct.UserOfAccount.Settings.MsgsPerPage = msgs_per_page;
                _acct.UserOfAccount.Settings.ViewMode = (ViewMode)view_mode;

                _acct.Update(true);
            }
            else
            {
                if (_acct == null)
					Log.WriteLine("UpdateSettings", "Account is NULL!!!");
                else if (_acct.UserOfAccount == null)
					Log.WriteLine("UpdateSettings", "User is NULL!!!");
                else if (_acct.UserOfAccount.Settings == null)
					Log.WriteLine("UpdateSettings", "Settings is NULL!!!");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void UpdateSignature(int id_acct, int type, int opt, string str)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID);
                if (acct != null)
                {
                    acct.SignatureType = (SignatureType)type;
                    acct.SignatureOptions = (SignatureOptions)opt;
                    acct.Signature = str;
                    acct.Update(false);

                    if (_acct.ID == id_acct)
                    {
                        _acct = acct;
                    }
                }
            }
            else
            {
                if (_acct == null)
                    Log.WriteLine("UpdateSignature", "Account is null.");
                else if (_acct.UserOfAccount == null)
                    Log.WriteLine("UpdateSignature", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void UpdateXSpam(bool xSpam)
        {
            if (_acct.IsDemo) return;

            if ((_acct.UserOfAccount != null) && (_acct.UserOfAccount.Settings != null))
            {
                _acct.UserOfAccount.Settings.XSpam = xSpam;
                _acct.Update(true);
            }
            else
            {
                if (_acct.UserOfAccount == null)
                    Log.WriteLine("UpdateXSpam", "User is null.");
                else if (_acct.UserOfAccount.Settings == null)
                    Log.WriteLine("UpdateXSpam", "User is null.");
                throw new WebMailException("User is null.");
            }
        }

        public void GroupOperations(string request, long id_folder, string folder_full_name, string search_query, int fields, long to_folder_id, string to_folder_full_name, WebMailMessageCollection messages)
        {
            if (CurrentAccount != null)
            {
                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(CurrentAccount));
                try
                {
                    mp.Connect();

                    if (messages != null)
                    {
                        if ((search_query != null) && (search_query.Length > 0))
                        {
                            FolderCollection fc = new FolderCollection();
                            if ((id_folder != -1) && (folder_full_name != null && folder_full_name.Length > 0))
                            {
                                fc.Add(mp.GetFolder(id_folder));
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
                            WebMailMessageCollection coll = mp.SearchMessages(search_query, fc, (fields == 0) ? true : false);
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
                        else if ((id_folder != -1) && (folder_full_name != null && folder_full_name.Length > 0))
                        {
                            // process messages in single folder
                            Folder fld = mp.GetFolder(id_folder);

                            if (fld != null)
                            {
                                object[] messageIndexSet = null;
                                if (messages.Count > 0)
                                {
                                    messageIndexSet = new object[messages.Count];
                                    for (int i = 0; i < messages.Count; i++)
                                    {
                                        if (fld.SyncType != FolderSyncType.DirectMode)
                                        {
                                            messageIndexSet[i] = messages[i].IDMsg;
                                        }
                                        else
                                        {
                                            if ((messages[i].StrUid != null) && (messages[i].StrUid.Length > 0))
                                            {
                                                messageIndexSet[i] = messages[i].StrUid;
                                            }
                                            else
                                            {
                                                messageIndexSet[i] = messages[i].IntUid;
                                            }
                                        }
                                    }
                                }
                                GroupMessagesProcess(request, mp, messageIndexSet, fld, to_folder_id, to_folder_full_name);
                            }
                        }
                        else
                        {
                            // process messages in many folders
                            for (int i = 0; i < messages.Count; i++)
                            {
                                Folder fld = mp.GetFolder(messages[i].IDFolderDB);
                                if (fld != null)
                                {
                                    GroupMessagesProcess(request, mp, new object[] { messages[i].IDMsg }, fld, to_folder_id, to_folder_full_name);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    mp.Disconnect();
                }
            }
            else
            {
				Log.WriteLine("GroupOperations", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void GroupMessagesProcess(string action, MailProcessor mp, object[] messageIndexSet, Folder fld, long to_folder_id, string to_folder_full_name)
        {
            if (mp != null)
            {
                switch (action)
                {
                    case Constants.GroupOperationsRequests.Delete:
                        mp.DeleteMessages(messageIndexSet, fld);
                        break;
                    case Constants.GroupOperationsRequests.Flag:
                        mp.SetFlags(messageIndexSet, SystemMessageFlags.Flagged, MessageFlagAction.Add, fld);
                        break;
                    case Constants.GroupOperationsRequests.MarkAllRead:
                        mp.SetFlags(SystemMessageFlags.Seen, MessageFlagAction.Add, fld);
                        break;
                    case Constants.GroupOperationsRequests.MarkAllUnread:
                        mp.SetFlags(SystemMessageFlags.Seen, MessageFlagAction.Remove, fld);
                        break;
                    case Constants.GroupOperationsRequests.MarkRead:
                        mp.SetFlags(messageIndexSet, SystemMessageFlags.Seen, MessageFlagAction.Add, fld);
                        break;
                    case Constants.GroupOperationsRequests.MarkUnread:
                        mp.SetFlags(messageIndexSet, SystemMessageFlags.Seen, MessageFlagAction.Remove, fld);
                        break;
                    case Constants.GroupOperationsRequests.MoveToFolder:
                        Folder toFolder = mp.GetFolder(to_folder_id);
                        if (toFolder != null)
                        {
                            mp.MoveMessages(messageIndexSet, fld, toFolder);
                        }
                        break;
                    case Constants.GroupOperationsRequests.Purge:
                        mp.PurgeMessages(messageIndexSet, fld);
                        break;
                    case Constants.GroupOperationsRequests.Undelete:
                        mp.SetFlags(messageIndexSet, SystemMessageFlags.Deleted, MessageFlagAction.Remove, fld);
                        break;
                    case Constants.GroupOperationsRequests.Unflag:
                        mp.SetFlags(messageIndexSet, SystemMessageFlags.Flagged, MessageFlagAction.Remove, fld);
                        break;
                }
            }
        }

        public void IncrementContactsFrequency(EmailAddressCollection emails)
        {
            DbStorage storage = DbStorageCreator.CreateDatabaseStorage(_acct);
            try
            {
                storage.Connect();
                foreach (EmailAddress email in emails)
                {
                    AddressBookContact contact = storage.GetAddressBookContact(email.Email);
                    if (contact != null)
                    {
                        contact.UseFrequency++;
                        UpdateContact(contact);
                    }
                    else
                    {
                        contact = new AddressBookContact();
                        contact.HEmail = email.Email;
                        NewContact(contact);
                    }
                }
            }
            finally
            {
                storage.Disconnect();
            }
        }
    }
}
