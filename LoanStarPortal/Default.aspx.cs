using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoanStar.Common;
using LoanStarPortal.Controls;
using Telerik.WebControls;
using System.Web.Services;


namespace LoanStarPortal
{
    public partial class Default : AppPage
    {
        #region Properties/Fields
        private bool isReloadPipeLineNeeded = false;
        private String updateControlID;
        public int CurrentTabIndex = -1;
        public override event MortgageEventHandler MortgageChanged;
        private const int LOANMENUID = 1;
        private const int RESOURCESMENUID = 0;
        protected string HelpUrl = String.Empty;

        protected int MortgageId
        {
            get
            {
                int res = 0;
                if (Session[Constants.MortgageID] != null)
                    res = Convert.ToInt32(Session[Constants.MortgageID]);
                return res;
            }
        }
        protected string CurrentCommandName
        {
            get
            {
                return (string)ViewState["CurrentCommandName"];
            }
            set
            {
                ViewState["CurrentCommandName"] = value;
            }
        }
        public ApplicantList appList
        {
            get { return ApplicantList1; }
        }
        public RadAjaxManager AjaxManager
        {
            get { return RadAjaxManager1; }
        }
        public Conditions cond
        {
            get
            {
                Conditions ctl = null;
                if (RightLoadedControlName != null)
                {
                    Control ctrl = RightPanel.FindControl(RightLoadedControlName.Split('.')[0]);
                    if (ctrl != null)
                    {
                        ctl = ctrl as Conditions;
                    }
                }
                return ctl;
            }
        }
        public Emails emails
        {
            get
            {
                Emails ctl = null;
                if (CenterLoadedControlName != null)
                {
                    Control ctrl = CenterPanel.FindControl(CenterLoadedControlName.Split('.')[0]);
                    if (ctrl != null)
                    {
                        ctl = ctrl as Emails;
                    }
                }
                return ctl;
            }
        }
        public TabPackage DocumentPackage
        {
            get
            {
                TabPackage ctl = null;
                if (RightLoadedControlName != null)
                {
                    Control ctrl = RightPanel.FindControl(RightLoadedControlName.Split('.')[0]);
                    if (ctrl != null)
                    {
                        ctl = ctrl as TabPackage;
                    }
                }
                return ctl;
            }
        }
        public Tabs tabs
        {
            get
            {
                Tabs ctl = null;
                if (CenterLoadedControlName != null)
                {
                    Control ctrl = CenterPanel.FindControl(CenterLoadedControlName.Split('.')[0]);
                    if (ctrl != null)
                    {
                        ctl = ctrl as Tabs;
                    }
                }
                return ctl;
            }
        }
        public Notes notes
        {
            get
            {
                Notes ctl = null;
                if (RightLoadedControlName != null)
                {
                    Control ctrl = RightPanel.FindControl(RightLoadedControlName.Split('.')[0]);
                    if (ctrl != null)
                    {
                        ctl = ctrl as Notes;
                    }
                }
                return ctl;
            }
        }
        /// <summary>
        /// Name of control loaded into center pane
        /// </summary>
        private string CenterLoadedControlName
        {
            get
            {
                return Session["CenterLoadedControlName"] as string;
            }
            set
            {
                Session["CenterLoadedControlName"] = value;
            }
        }

        /// <summary>
        /// Name of control loaded into 
        /// </summary>
        private String DialogLoadedControlName
        {
            get { return Session["DialogLoadedControlName"] as String; }
            set { Session["DialogLoadedControlName"] = value; }
        }

        /// <summary>
        /// Name of control loaded into right pane
        /// </summary>
        private string RightLoadedControlName
        {
            get
            {
                return Session["RightLoadedControlName"] as string;
            }
            set
            {
                Session["RightLoadedControlName"] = value;
            }
        }

        public override Panel PanelRight
        {
            get
            {
                return RightPanel;
            }
        }
        public override Panel PanelCenter
        {
            get
            {
                return CenterPanel;
            }
        }
        public override Panel PanelLeft
        {
            get
            {
                return LeftPanel;
            }
        }
        //public RadWindow CreateMailFolderWnd
        //{
        //    get { return CreateMailFolder; }
        //}
        #endregion

        #region Public Methods
        /// <summary>
        /// Load controls when page is loaded 1st time
        /// </summary>
        public void InitControls()
        {
            if (CenterLoadedControlName != null && CenterLoadedControlName != Constants.FECTLEMAILS)
            {
                LoadUserControl(CenterLoadedControlName, CenterPanel, CenterLoadedControlName);
            }
            else
            {
                //LoadUserControl(Constants.FECTLEMAILS, CenterPanel, CenterLoadedControlName);
            }
            if (RightLoadedControlName != null)
            {
                LoadUserControl(RightLoadedControlName, RightPanel, RightLoadedControlName);
            }

            if (DialogLoadedControlName != null)
            {
                LoadUserControl(DialogLoadedControlName, DialogPanel, DialogLoadedControlName);
            }
        }

        protected void MakePanesVisible()
        {
            if (!RadSplitter1.GetSplitBarById("RadSplitBar2").Visible)
                RadSplitter1.GetSplitBarById("RadSplitBar2").Visible = true;
            RadSplitter1.GetPaneById("RightPane").Visible = true;
            RadSplitter1.GetPaneById("RightPane").Width = Unit.Pixel(300);
        }

        /// <summary>
        /// Clear all controls from panels
        /// </summary>
        protected void ClearControls()
        {
            CenterPanel.Controls.Clear();
            RightPanel.Controls.Clear();
        }
        /// <summary>
        /// Load user control to appropriate panel
        /// </summary>
        /// <param name="controlName">Control name</param>
        /// <param name="panel">Panel where control appears</param>
        /// <param name="LoadedControlName">Page property to keep control name (LeftLoadedControlName/CenterLoadedControlName/RightLoadedControlName)</param>
        public override Control LoadUserControl(string controlName, Panel panel, string LoadedControlName)
        {
            if (LoadedControlName != null)
            {
                Control previousControl = panel.FindControl(LoadedControlName.Split('.')[0]);
                if (previousControl != null)
                {
                    panel.Controls.Remove(previousControl);
                    if (panel == CenterPanel) CenterLoadedControlName = String.Empty;
                    if (panel == RightPanel) RightLoadedControlName = String.Empty;
                }
            }
            string userControlID = controlName.Split('.')[0];
            Control targetControl = panel.FindControl(userControlID);
            if (targetControl == null)
            {
                UserControl userControl = String.IsNullOrEmpty(controlName) ? null : (UserControl)LoadControl(Constants.FECONTROLSLOCATION + controlName);
                if (userControl != null)
                {
                    userControl.ID = userControlID.Replace("/", "").Replace("~", "");
                    panel.Controls.Add(userControl);
                    if (panel == CenterPanel) CenterLoadedControlName = controlName;
                    if (panel == RightPanel) RightLoadedControlName = controlName;
                    if (panel == DialogPanel) DialogLoadedControlName = controlName;
                }
                return userControl;
            }
            else
                return targetControl;
        }
        #endregion

        #region Page Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            IsAjaxPostBackRaisen = false;
            SetRuleEvaluationNeeded(true);
            if (!String.IsNullOrEmpty(currenttab.Value))
            {
                CurrentTabIndex = Convert.ToInt32(currenttab.Value);
            }
            if (CurrentUser.CompanyId == Constants.LOANSTARCOMPANYID)
            {
                Response.Redirect(ResolveUrl(CurrentUser.GetDefaultPage()));
            }
            InitControls();
            CheckIfPipeLineReloadNeeded();
            if (!IsPostBack)
            {
                if (Session["EMailDisplayed"] == null)
                    Session.Add("EMailDisplayed", false);

                if (MortgageId > 0)
                    Page.ClientScript.RegisterHiddenField("currentmortgageid", MortgageId.ToString());
            }
            EnableMenuItems((MortgageId > 0));
            if (isReloadPipeLineNeeded)
            {
                appList.RepopulateMortgageList();
            }
            if (Request["__EVENTTARGET"] == "rmMortgage")
            {
                if (Request["__EVENTARGUMENT"] == "EmailLinkClicked")
                {
                    EmailLinkCliked();
                }
                ProcessRequest(Request["__EVENTARGUMENT"]);
            }
            if (Request["__EVENTTARGET"] == "Client")
            {
                if (Request["__EVENTARGUMENT"] == "UpdateMessageBoard")
                {
                    ReloadMessageBoard();
                }
            }
        }
        #endregion

        protected void Page_Unload(object sender, EventArgs e)
        {
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            CenterRightPanelUpdateNeeded = false;
            if (Request.Form["__EVENTARGUMENT"] != null)
            {
                if (Request.Form["__EVENTARGUMENT"].Contains("rmMortgage"))
                {
                    RemoveAjaxSetting(RadAjaxManager1, CenterPanel);
                    RemoveAjaxSetting(LeftPanel, CenterPanel);
                    RemoveAjaxSetting(LeftPanel, RightPanel);
                }
                else
                {
                    AddMortgageProfileAjaxSettings();
                }
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// TEMPORARY ADDED
                //IsMailChecked = false;
                //if (!IsMailChecked)
                //{
                //    ClientScript.RegisterStartupScript(GetType(), "HideEmailProcessing", "<script>ShowProcessImage(true);</script>");
                //    MailTimer.Interval = 500;
                //    MailTimer.Start();
                //}

            }
            if (IsMailChecked)
            {
                //ClientScript.RegisterStartupScript(GetType(), "HideEmailProcessing", "<script>ShowProcessImage(false);</script>");
            }
        }
        protected void Page_LoadComplete(object sender, EventArgs e)
        {

        }
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {

        }
        protected void Page_SaveStateComplete(object sender, EventArgs e)
        {
        }

        #region Event Handler Methods
        protected void AddMortgageProfileAjaxSettings()
        {
            RemoveAjaxSetting(RadAjaxManager1, RadAjaxManager1);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(RadAjaxManager1, CenterPanel, null);
            //RadAjaxManager1.AjaxSettings.AddAjaxSetting(RadAjaxManager1, RadAjaxManager1, null);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(RadAjaxManager1, RightPanel, null);
            //RadAjaxManager1.AjaxSettings.AddAjaxSetting(RadAjaxManager1, DialogPanel, null);
        }
        public void RemoveAjaxSetting(Control ajaxControl, Control updatedControl)
        {
            AjaxSettingsCollection ajaxSettings = RadAjaxManager1.AjaxSettings;
            foreach (AjaxSetting ajaxSetting in ajaxSettings)
            {
                if (ajaxSetting.AjaxControlID == ajaxControl.ID)
                {
                    AjaxUpdatedControl settingToRemove = null;
                    foreach (AjaxUpdatedControl ajaxUpdatedControl in ajaxSetting.UpdatedControls)
                    {
                        if (ajaxUpdatedControl.ControlID == updatedControl.ClientID)
                        {
                            settingToRemove = ajaxUpdatedControl;
                            break;
                        }
                    }
                    if (settingToRemove != null)
                    {
                        ajaxSetting.UpdatedControls.Remove(settingToRemove);
                        break;
                    }
                }
            }
        }
        protected void EnableMenuItems(bool Enable)
        {
            foreach (RadMenuItem item in rmMortgage.Items[LOANMENUID].Items)
            {
                item.Enabled = Enable;
            }
            foreach (RadMenuItem item in rmMortgage.Items[RESOURCESMENUID].Items)
            {
                if (item.Value == "NewMortgage")
                {
                    item.Enabled = CurrentUser.IsInRoles(new int[] { (int)AppUser.UserRoles.LoanOfficer });
                }
            }
            if (CurrentUser.IsMortgageManager())
            {
                //Add "Field Changes"
                RadMenuItem FieldChangesItem;
                FieldChangesItem = rmMortgage.Items[LOANMENUID].Items.FindItemByValue("FieldChanges");
                if (FieldChangesItem != null)
                {
                    FieldChangesItem.Visible = true;
                    FieldChangesItem.Enabled = Enable;
                }
                //Add "Login track"
                RadMenuItem LoginTrackItem;
                LoginTrackItem = rmMortgage.Items[RESOURCESMENUID].Items.FindItemByValue("LoginTrack");
                if (LoginTrackItem != null)
                {
                    LoginTrackItem.Visible = true;
                    LoginTrackItem.Enabled = true;
                }
            }
            RadMenuItem RetailTools;
            RetailTools = rmMortgage.Items[RESOURCESMENUID].Items.FindItemByValue("RetailSite");
            if (CurrentUser.IsRetailEnabled)
            {
                RetailTools.Enabled = true;
                RetailTools.Visible = true;
            }
            RadMenuItem HelpItem = rmMortgage.Items[RESOURCESMENUID].Items.FindItemByValue("Help");
            if (String.IsNullOrEmpty(AppSettings.HelpUrlPublic))
            {
                HelpItem.Enabled = false;
                HelpItem.Visible = false;
            }
            else
            {
                HelpUrl = AppSettings.HelpUrlPublic;
            }
        }

        protected void ApplicantList1_MortgageItemClick(object sender, EventArgs e)
        {

            CenterRightPanelUpdateNeeded = true;
            MortgageArg mArg = (MortgageArg)e;
            int mpID = mArg.MPID;
            Session[Constants.MortgageID] = mpID;
            Session[Constants.CURRENTTOPFIRSTTABID] = MortgageTab.TABBORROWERID;
            Session[Constants.CURRENTTOPSECONDTABINDEX] = Constants.TOPSECONDTABINDEX;
            Session[Constants.CURRENTBOTTOMTABID] = Tabs.TABLOANINFOID;
            Session[Constants.CURRENTCALCULATORTABID] = Calculator.TABLEADCALC;
            Session[Constants.MESSAGEBOARDSHOWONLYNOTES] = false;

            EnableMenuItems(true);
            CenterPanel.Controls.Clear();
            updateControlID = CenterPanel.ID;

            if (mpID > 0)
            {
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(LeftPanel, RightPanel, null);
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(LeftPanel, rmMortgage, null);
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(LeftPanel, CenterPanel, null);
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(CenterPanel, CenterPanel, null);
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(RadAjaxManager1, CenterPanel, null);
                LoadUserControl(Constants.FECTLTABS, CenterPanel, CenterLoadedControlName);
                if (RightPanel.Controls.Count > 0)
                {
                    RightPanel.Controls.Clear();
                }
                LoadUserControl(Constants.FECTLNOTES, RightPanel, RightLoadedControlName);
            }
            else
            {
                LoadUserControl(Constants.FECTLEMAILS, CenterPanel, CenterLoadedControlName);
                LoadUserControl(String.Empty, RightPanel, RightLoadedControlName);
            }
        }

        protected void rmMortgage_ItemClick(object sender, RadMenuEventArgs e)
        {
            if (appList.IsEmailMode)
            {
                appList.ChangeApplicantList(false);
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(rmMortgage, LeftPanel, null);
            }
            RemoveAjaxSetting(rmMortgage, rmMortgage);
            RemoveAjaxSetting(rmMortgage, CenterPanel);
            RemoveAjaxSetting(rmMortgage, RightPanel);

            if (e.Item.Value == "NewMortgage")
            {
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(rmMortgage, CenterPanel, null);
                Session[Constants.MortgageID] = null;
                EnableMenuItems(false);
                CenterPanel.Controls.Clear();
                updateControlID = CenterPanel.ID;
                LoadUserControl(Constants.FECTLTABS, CenterPanel, CenterLoadedControlName);
                if (MortgageChanged != null)
                    MortgageChanged(this, -1);

            }
            else if (e.Item.Value == "Emails")
            {
                LoadEmail();
            }
            else if (e.Item.Value == "Calendar")
            {
                RemoveAjaxSetting(CenterPanel, RightPanel);
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(rmMortgage, CenterPanel, null);
                CenterPanel.Controls.Clear();
                updateControlID = CenterPanel.ID;
                LoadUserControl(Constants.FECTLCALENDAR, CenterPanel, CenterLoadedControlName);
            }
            else if (e.Item.Value == "Vendors")
            {
                Session[Constants.VENDORVIEW] = Constants.VENDORVIEWGRID;

                //RemoveAjaxSetting(CenterPanel, RightPanel);
                //RadAjaxManager1.AjaxSettings.AddAjaxSetting(rmMortgage, CenterPanel, null);
                //RemoveAjaxSetting(CenterPanel, RightPanel);
                //CenterPanel.Controls.Clear();
                //updateControlID = CenterPanel.ID;
                //LoadUserControl(Constants.FECTLVENDORSPUBLIC, CenterPanel, CenterLoadedControlName);

                //RadAjaxManager1.AjaxSettings.AddAjaxSetting(DialogPanel, RadAjaxManager1, null);
                DialogPanel.Controls.Clear();
                updateControlID = DialogPanel.ID;
                LoadUserControl(Constants.FECTLVENDORSPUBLIC, DialogPanel, DialogLoadedControlName);
            }
            else if (e.Item.Value == "MyProfile")
            {
                RemoveAjaxSetting(RadAjaxManager1, DialogPanel);
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(rmMortgage, DialogPanel, null);
                RemoveAjaxSetting(RadAjaxManager1, DialogPanel);
                DialogPanel.Controls.Clear();
                updateControlID = DialogPanel.ID;
                LoadUserControl(Constants.FECTLMYPROFILE, DialogPanel, DialogLoadedControlName);
            }
            else if (e.Item.Value == "GFE")
            {
                RemoveAjaxSetting(CenterPanel, DialogPanel);
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(rmMortgage, DialogPanel, null);
                RemoveAjaxSetting(CenterPanel, DialogPanel);
                DialogPanel.Controls.Clear();
                updateControlID = CenterPanel.ID;
                LoadUserControl(Constants.FECTLGFE, DialogPanel, DialogLoadedControlName);
            }
            else if (e.Item.Value == "Reports")
            {
                RemoveAjaxSetting(RadAjaxManager1, DialogPanel);
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(rmMortgage, DialogPanel, null);
                RemoveAjaxSetting(RadAjaxManager1, DialogPanel);
                DialogPanel.Controls.Clear();
                updateControlID = DialogPanel.ID;
                LoadUserControl(Constants.FECTLREPORTS, DialogPanel, DialogLoadedControlName);
            }
            else if (e.Item.Value == "FieldChanges")
            {
                RemoveAjaxSetting(CenterPanel, RightPanel);
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(rmMortgage, CenterPanel, null);
                RemoveAjaxSetting(CenterPanel, RightPanel);
                CenterPanel.Controls.Clear();
                updateControlID = CenterPanel.ID;
                LoadUserControl(Constants.FECTLFIELDSCHANGES, CenterPanel, CenterLoadedControlName);
            }
            else if (e.Item.Value == "LoginTrack")
            {
                RemoveAjaxSetting(CenterPanel, RightPanel);
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(rmMortgage, CenterPanel, null);
                RemoveAjaxSetting(CenterPanel, RightPanel);
                CenterPanel.Controls.Clear();
                updateControlID = CenterPanel.ID;
                LoadUserControl(Constants.FECTLLOGINTRACK, CenterPanel, CenterLoadedControlName);
            }
            else if (e.Item.Value == "Links")
            {
                RemoveAjaxSetting(RadAjaxManager1, DialogPanel);
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(rmMortgage, DialogPanel, null);
                RemoveAjaxSetting(RadAjaxManager1, DialogPanel);
                DialogPanel.Controls.Clear();
                updateControlID = DialogPanel.ID;
                LoadUserControl(Constants.FECTLLINKS, DialogPanel, DialogLoadedControlName);
            }
            else if (e.Item.Value == "Help")
            {
                RemoveAjaxSetting(CenterPanel, RightPanel);
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(rmMortgage, CenterPanel, null);
                RemoveAjaxSetting(CenterPanel, RightPanel);
                CenterPanel.Controls.Clear();
                updateControlID = CenterPanel.ID;
                LoadUserControl(Constants.FECTLHELP, CenterPanel, CenterLoadedControlName);
            }
            else if (e.Item.Value == "Logout")
            {
                AppUser user = GetObject(Constants.USEROBJECTNAME) as AppUser;
                if (user != null)
                {
                    user.SaveUserLog(user.UserLogId, DateTime.Now);
                }

                FormsAuthentication.SignOut();
                Session.Clear();
                Global.integration = null;
                Global.userAccount = null;
                Response.Redirect("~/" + Constants.PUBLICPAGENAME);
            }
        }

        protected void btnPanelDialogHide_Click(object sender, EventArgs e)
        {

        }

        protected void RightMenu_ItemClick(object sender, RadMenuEventArgs e)
        {
            ProcessRequest(e.Item.Value);
        }

        private void LoadEmail()
        {
            RemoveAjaxSetting(RadAjaxManager1, DialogPanel);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(rmMortgage, DialogPanel, null);
            DialogPanel.Controls.Clear();
            updateControlID = DialogPanel.ID;
            LoadUserControl(Constants.FECTLEMAILS, DialogPanel, DialogLoadedControlName);
        }
        private void EmailLinkCliked()
        {
            CenterRightPanelUpdateNeeded = true;
            Session[Constants.CURRENTTOPFIRSTTABID] = MortgageTab.TABBORROWERID;
            Session[Constants.CURRENTTOPSECONDTABINDEX] = Constants.TOPSECONDTABINDEX;
            Session[Constants.CURRENTBOTTOMTABID] = Tabs.TABLOANINFOID;
            Session[Constants.CURRENTCALCULATORTABID] = Calculator.TABLEADCALC;
            Session[Constants.MESSAGEBOARDSHOWONLYNOTES] = false;
            LoadEmail();
        }

        private void ProcessRequest(String type)
        {
            if (MortgageId <= 0) return;
            if (type == "Notes")
            {
                LoadNotes();
            }
            else if (type == "Conditions")
            {
                LoadConditions();
            }
            else if (type == "Docs")
            {
                LoadDocuments();
            }
        }

        private void LoadNotes()
        {
            RemoveAjaxSetting(RadAjaxManager1, CenterPanel);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(rmMortgage, RightPanel, null);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(RightPanel, CenterPanel, null);
            RightPanel.Controls.Clear();
            updateControlID = RightPanel.ID;
            LoadUserControl(Constants.FECTLNOTES, RightPanel, RightLoadedControlName);
        }

        private void LoadConditions()
        {
            RemoveAjaxSetting(RadAjaxManager1, CenterPanel);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(rmMortgage, RightPanel, null);
            RightPanel.Controls.Clear();
            updateControlID = RightPanel.ID;
            LoadUserControl(Constants.FECTLCONDITIONS, RightPanel, RightLoadedControlName);
        }

        private void LoadDocuments()
        {
            RemoveAjaxSetting(RadAjaxManager1, CenterPanel);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(rmMortgage, RightPanel, null);
            RightPanel.Controls.Clear();
            updateControlID = RightPanel.ID;
            LoadUserControl(Constants.FECTLTABPACKAGE, RightPanel, RightLoadedControlName);
        }

        protected void RadAjaxManager1_ResolveUpdatedControls(object sender, UpdatedControlsEventArgs e)
        {
            e.SuppressError = true;

            if (CenterRightPanelUpdateNeeded)
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(CenterPanel, RightPanel, null);
            else
                RemoveAjaxSetting(CenterPanel, RightPanel);

            if (CenterLeftPanelUpdateNeeded)
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(CenterPanel, LeftPanel, null);
            else
                RemoveAjaxSetting(CenterPanel, LeftPanel);

            AppUser user = GetObject(Constants.USEROBJECTNAME) as AppUser;
            if (user == null)
            {
                Session.Clear();
                Global.integration = null;
                Global.userAccount = null;
                Response.Redirect("~/" + Constants.PUBLICPAGENAME);
            }
            if (updateControlID != null)
                e.UpdatedControls.Add(new AjaxUpdatedControl(updateControlID, String.Empty));
        }
        #endregion
        private void CheckIfPipeLineReloadNeeded()
        {
            Object o = Session[Constants.LASTPIPELINERELOADTIME];
            if (o is DateTime)
            {
                DateTime dt1 = (DateTime)o;
                if (dt1.AddMinutes(AppSettings.RefreshPipeline) < DateTime.Now)
                {
                    isReloadPipeLineNeeded = true;
                }
            }
        }

        protected void MailTimer_Tick(object sender, TickEventArgs e)
        {
            //if (IsMailChecked && MailTimer.IsStarted)
            //{
            //    MailTimer.AutoStart = false;
            //    MailTimer.Stop();
            //    MailTimer.AutoStart = false;

            //    RadAjaxManager1.ResponseScripts.Add("ShowProcessImage(false);");
            //    Emails em = emails as Emails;
            //    if (em != null)
            //    {
            //        RadAjaxManager1.AjaxSettings.AddAjaxSetting(MailTimer, CenterPanel, null);
            //        emails.RebindGrid();
            //    }
            //}
        }

        private void ReloadMessageBoard()
        {
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(rmMortgage, RightPanel, null);
            RightPanel.Controls.Clear();
            updateControlID = RightPanel.ID;
            LoadUserControl(Constants.FECTLNOTES, RightPanel, RightLoadedControlName);
        }

        protected void btnPanelDialogHide_OnClick(object sender, EventArgs e)
        {
            DialogLoadedControlName = null;
        }
    }
}
