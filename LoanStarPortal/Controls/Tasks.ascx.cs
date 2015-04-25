using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class Tasks : AppControl
    {
/*
        public event UpdateNeeded OnUpdateNeeded;
        public delegate void UpdateNeeded();

        #region Fields/Properties
        private const string FIRST_LOAD = "TasksFirstLoad";

        private int MortgageID
        {
            get
            {
                return Convert.ToInt32(Session[Constants.MortgageID]);
            }
        }

        private int TaskID
        {
            get
            {
                if (Session["TaskID"] == null)
                    return -1;
                else
                    return Convert.ToInt32(Session["TaskID"]);
            }
            set
            {
                Session["TaskID"] = value;
            }
        }

        private int ConditionID
        {
            get
            {
                if (Session["ConditionID"] == null)
                    return -1;
                else
                    return Convert.ToInt32(Session["ConditionID"]);
            }
            set
            {
                Session["ConditionID"] = value;
            }
        }

        protected bool ConditionState
        {
            set
            {
                panelCondition.Visible = value;
                trTaskDesc.Visible = false;
                trTaskTitle.Visible = false;
                //trTaskDesc.Visible = !value;
                //trTaskTitle.Visible = !value;
            }
            get
            {
                return panelCondition.Visible;
            }
        }

        private bool ConditionEmpty
        {
            get
            {
                return String.IsNullOrEmpty(tbCondTitle.Text);
            }
        }

        private bool TaskEmpty
        {
            get
            {
                bool res = true;
                if (!String.IsNullOrEmpty(tbTitle.Text) || !String.IsNullOrEmpty(ddlTaskRecurrence.SelectedValue))
                    res = false;

                return res;
            }
        }

        //private bool ShowNotesFields
        //{
        //    get
        //    {
        //        bool res = false;
        //        if (ViewState["ShowNotesFields"] != null)
        //            res = Convert.ToBoolean(ViewState["ShowNotesFields"]);
        //        return res;
        //    }
        //    set
        //    {
        //        ViewState["ShowNotesFields"] = value;
        //    }
        //}

        private bool SelectSavedTask
        {
            get
            {
                bool res = false;
                if (ViewState["SelectSavedTask"] != null)
                    res = Convert.ToBoolean(ViewState["SelectSavedTask"]);
                return res;
            }
            set
            {
                ViewState["SelectSavedTask"] = value;
            }
        }
        private bool SelectSavedCondition
        {
            get
            {
                bool res = false;
                if (ViewState["SelectSavedCondition"] != null)
                    res = Convert.ToBoolean(ViewState["SelectSavedCondition"]);
                return res;
            }
            set
            {
                ViewState["SelectSavedCondition"] = value;
            }
        }
        private bool AddingNewItem
        {
            get
            {
                bool res = false;
                if (ViewState["AddingNewItem"] != null)
                    res = Convert.ToBoolean(ViewState["AddingNewItem"]);
                return res;
            }
            set
            {
                ViewState["AddingNewItem"] = value;
            }
        }
        #endregion

        #region Methods
        private void BindData()
        {
            ddlAssignedTo.DataSource = AppUser.GetTaskUserList(CurrentUser.CompanyId, MortgageID);
            ddlAssignedTo.DataBind();
            ddlAssignedTo.Items.Insert(0, new ListItem("- Not Set -", "-1"));
            ddlAssignedTo.SelectedIndex = ddlAssignedTo.Items.IndexOf(ddlAssignedTo.Items.FindByValue(CurrentUser.Id.ToString()));
            ddlTaskRecurrence.DataSource = Task.GetRecurrenceList();
            ddlTaskRecurrence.DataBind();
            ddlTaskRecurrence.Items.Insert(0, new ListItem(" Select ", ""));
            ddlCondType.DataSource = Condition.GetTypeList();
            ddlCondType.DataBind();
            ddlCondCategory.DataSource = Condition.GetCategoryList();
            ddlCondCategory.DataBind();
            rdpTaskSchedule.SelectedDate = DateTime.Now;
            if (gridConditionTasks.MasterTableView.Items.Count > 0 && ConditionID <= 0 && !AddingNewItem)
            {
                gridConditionTasks.MasterTableView.Items[0].Selected = true;
                string sid = Convert.ToString(gridConditionTasks.MasterTableView.DataKeyValues[0]["SID"]);
                LoadItem(sid);
                gridConditionTasks.SelectedIndexes.Clear();
                gridConditionTasks.SelectedIndexes.Add(0);
            }
            else if (!AddingNewItem)
            {
                ClearConditionFields();
                ClearTaskFields();
            }
            //divAddUComments.Visible = false;
        }*/
        public void RebindGrid()
        {
            gridConditionTasks.Rebind();
        }/*
        private void ClearConditionFields()
        {
            tbCondTitle.Text = "";
            tbCondDesc.Text = "";
            if (ddlCondType.Items.Count > 0)
                ddlCondType.SelectedIndex = 0;
            if (ddlCondCategory.Items.Count > 0)
                ddlCondCategory.SelectedIndex = 0;
            divUComments.Attributes.Clear();
            divAddUComments.Visible = false;
            tbCondNotes.Text = "";
        }
        private void ClearTaskFields()
        {
            tbTitle.Text = "";
            tbDescription.Text = "";
            if (ddlCondType.Items.Count > 0)
                ddlCondType.SelectedIndex = 0;
            ddlAction.SelectedIndex = 0;
            rdpTaskSchedule.SelectedDate = DateTime.Now;
            if (ddlTaskRecurrence.Items.Count > 0)
                ddlTaskRecurrence.SelectedIndex = 0;
            if (ddlAssignedTo.Items.Count > 0)
                ddlAssignedTo.SelectedIndex = 0;
            tbQuickNote.Text = "";
        }
        private void LoadNotesPane(bool onlyNotes)
        {
            Notes noteCtrl;
            Control res = null;
            foreach (Control ctrl in CurrentPage.PanelRight.Controls)
            {
                if (ctrl is Notes)
                {
                    res = ctrl;
                    break;
                }
            }
            if (res == null)
            {
                CurrentPage.PanelRight.Controls.Clear();
                noteCtrl = (Notes)CurrentPage.LoadUserControl(Constants.FECTLNOTES, CurrentPage.PanelRight, null);
            }
            else
            {
                noteCtrl = (Notes)res;
            }
            noteCtrl.IsFirstLoad = true;
            //noteCtrl.LoadTaskNotes(TaskID);
            if (onlyNotes) 
                noteCtrl.LoadNotes();
            else
                noteCtrl.BindData();
        }
        private void LoadMessageBoardPane()
        {
            Notes noteCtrl;
            Control res = null;
            foreach (Control ctrl in CurrentPage.PanelRight.Controls)
            {
                if (ctrl is Notes)
                {
                    res = ctrl;
                    break;
                }
            }
            if (res == null)
            {
                CurrentPage.PanelRight.Controls.Clear();
                noteCtrl = (Notes)CurrentPage.LoadUserControl(Constants.FECTLNOTES, CurrentPage.PanelRight, null);
            }
            else
            {
                noteCtrl = (Notes)res;
            }
            noteCtrl.IsFirstLoad = true;
            noteCtrl.BindData();
        }
        private void UpdateRuleConditions()
        {
            #region old version
            //if (MortgageID > 0)
            //{
            //    MortgageProfile mp;
            //    ArrayList ar;
            //    string xml;
            //    mp = CurrentPage.GetMortgage(MortgageID);
            //    ar = LoanStar.Common.Rule.GetSpecificRules(LoanStar.Common.Rule.TASKOBJECTTYPEID, CurrentUser.CompanyId);
            //    mp.EvaluateRules(ar);
            //    if ((ar != null) && (ar.Count > 0))
            //    {
            //        xml = MortgageProfile.GetRulesXml(ar);
            //        if (xml.Length > 0)
            //        {
            //            mp.UpdateMPTaskRules(xml);
            //        }
            //    }

            //    mp = CurrentPage.GetMortgage(MortgageID);
            //    ar = LoanStar.Common.Rule.GetSpecificRules(LoanStar.Common.Rule.CONDITIONOBJECTTYPEID, CurrentUser.CompanyId);
            //    mp.EvaluateRules(ar);
            //    if ((ar != null) && (ar.Count > 0))
            //    {
            //        xml = MortgageProfile.GetRulesXml(ar);
            //        mp.UpdateMPConditionRules(xml);
            //    }
            //}
            #endregion
            if (MortgageID > 0)
            {
                MortgageProfile mp = CurrentPage.GetMortgage(MortgageID);
                mp.BuildRuleEvaluationTree(CurrentPage.GetRuleTreePublic());
                string xml = mp.GetRuleListXml(RuleTree.TASKBIT);
                if (!String.IsNullOrEmpty(xml))
                {
                    mp.UpdateMPTaskRules(xml);
                }
                mp.UpdateMPConditionRules(mp.GetRuleListXml(RuleTree.CONDITIONBIT));
            }
        }
        private DateTime GetNextSchedule(DateTime scheduleDate, int reccurence)
        {
            DateTime res = scheduleDate;
            DateTime curDate = DateTime.Now;
            TimeSpan span = curDate.Subtract(scheduleDate);
            switch (reccurence)
            {
                case 1: //everyday
                    res = scheduleDate.AddDays(span.Days + 1);
                    break;
                case 2: //Every other day
                    res = scheduleDate.AddDays(Math.Ceiling((Double)(span.Days / 2)) * 2);
                    break;
                case 3: //Once a week
                    res = scheduleDate.AddDays(Math.Ceiling((Double)(span.Days / 7)) * 7);
                    break;
                case 4: //Every other week
                    res = scheduleDate.AddDays(Math.Ceiling((Double)(span.Days / 14)) * 14);
                    break;
                case 5: //Once a month
                    res = scheduleDate.AddMonths(1);
                    break;
            }
            return res;
        }
        private string GetEventTitle(string str)
        {
            if (str.Length > 50) str = str.Substring(1, 50);
            return str;
        }
        private void SaveTask()
        {
            Condition scond = new Condition(ConditionID);
            Task stask = new Task(TaskID);
            if (!String.IsNullOrEmpty(ddlAction.SelectedValue))
            {
                switch (ddlAction.SelectedValue)
                {
                    case "2":
                        stask.ScheduleDate = GetNextSchedule(stask.ScheduleDate, stask.RecurrenceID);
                        break;
                    case "3":
                        stask.ScheduleDate = (DateTime)rdpTaskSchedule.SelectedDate;
                        stask.StatusID = Constants.TASKSTATUSCOMPLETED;
                        break;
                    default:
                        stask.ScheduleDate = (DateTime)rdpTaskSchedule.SelectedDate;
                        break;
                }
            }
            else
                stask.ScheduleDate = (DateTime)rdpTaskSchedule.SelectedDate;

            if (ConditionID == -1)
            {
                stask.Title = tbTitle.Text;
                stask.Description = tbDescription.Text;
            }
            else
            {
                if ((scond.Title != tbTitle.Text && !String.IsNullOrEmpty(tbTitle.Text.Trim()))
                    ||
                    (scond.Details != tbDescription.Text && !String.IsNullOrEmpty(tbDescription.Text.Trim())))
                {
                    stask.Title = tbTitle.Text;
                    stask.Description = tbDescription.Text;
                }
                else
                {
                    stask.Title = tbCondTitle.Text;
                    stask.Description = tbCondDesc.Text;
                }
            }
            stask.MortgageID = MortgageID;
            if (TaskID <= 0) stask.StatusID = Constants.TASKSTATUSACTIVE;
            if (!String.IsNullOrEmpty(ddlTaskRecurrence.SelectedValue))
                stask.RecurrenceID = Convert.ToInt32(ddlTaskRecurrence.SelectedValue);
            else
                stask.RecurrenceID = 6;
            stask.AssignedTo = Convert.ToInt32(ddlAssignedTo.SelectedValue);
            stask.InfoSourceID = 1;
            stask.DifficultyID = 1;
            stask.EstimatedAttempts = "1";
            stask.CreatedBy = CurrentUser.Id;
            stask.ConditionID = ConditionID;
            //if (stask.ID <= 0)
            //{
            //    Event.Save(MortgageID, 27, "Task <b>" + GetEventTitle(stask.Title) + "</b> created.");
            //}

            stask.Save();
            string sNote = tbQuickNote.Text.Trim();
            if (TaskID > 0 && !String.IsNullOrEmpty(sNote))
            {
                DataRow note = stask.CreateNote();
                note["Note"] = sNote;
                tbQuickNote.Text = "";
                stask.SaveNotes();
                LoadNotesPane(true);
            }
            TaskID = stask.ID;
            scond.TaskID = TaskID;
            scond.Save();
            AddingNewItem = false;
            SelectSavedTask = true;
            InitControls();
        }
        private bool SaveCondition()
        {
            Page.Validate("Cond");
            if (Page.IsValid)
            {
                Condition scond = new Condition(ConditionID);
                if ((ConditionID <= 0) || (ConditionID > 0 && CanEditRuleCondition(scond.AuthorityLevel)))
                {
                    scond.Title = tbCondTitle.Text;
                    scond.Details = tbCondDesc.Text.Trim();
                    scond.MortgageID = MortgageID;
                    scond.TypeID = Convert.ToInt32(ddlCondType.SelectedValue);
                    scond.CategoryID = Convert.ToInt32(ddlCondCategory.SelectedValue);
                    if (ConditionID <= 0) scond.StatusID = Constants.CONDITIONSTATUSNOTSATISFIED;
                    scond.Notes = tbCondNotes.Text;
                    if (ConditionID <= 0)
                        scond.CreatedBy = CurrentUser.Id;
                    if (scond.ID <= 0)
                    {
                        scond.AuthorityLevel = CurrentUser.MinAuthorityLevel();
                        string begining;
                        if (!String.IsNullOrEmpty(ddlTaskRecurrence.SelectedValue))
                            begining = "Follow-up schedule created: ";
                        else
                            begining = "Condition created: ";
                        Event.Save(MortgageID, 26, begining + "<b>" + GetEventTitle(scond.Title) + "</b> by " + CurrentUser.FirstName + " " + CurrentUser.LastName + " at " + DateTime.Now.ToString("f") + ".");
                    }
                    if (!String.IsNullOrEmpty(ddlAction.SelectedValue) && String.Compare(ddlAction.SelectedValue, "3") == 0)
                    {
                        scond.Completed = true;
                    }
                    scond.Save();
                }
                else if (ConditionID > 0)
                {
                    if (!String.IsNullOrEmpty(ddlAction.SelectedValue) && String.Compare(ddlAction.SelectedValue, "3") == 0)
                    {
                        scond.Completed = true;
                    }
                }

                ConditionID = scond.ID;
                if (TaskID > 0)
                {
                    scond.TaskID = TaskID;
                    scond.Save();
                }
                else
                {
                    if ((TaskEmpty && TaskID<=0) && !String.IsNullOrEmpty(tbQuickNote.Text))
                    {
                        Task.SaveNote(-1, null, MortgageID, tbQuickNote.Text, CurrentUser.Id);
                        LoadNotesPane(false);
                    }
                }
                SelectSavedCondition = true;
                AddingNewItem = false;
                InitControls();
            }
            return Page.IsValid;
        }
        private int? LoadCondition()
        {
            Condition scond = new Condition(ConditionID);
            tbCondTitle.Text = scond.Title;
            tbCondDesc.Text = scond.Details;
            ddlCondType.SelectedIndex = ddlCondType.Items.IndexOf(ddlCondType.Items.FindByValue(scond.TypeID.ToString()));
            ddlCondCategory.SelectedIndex = ddlCondCategory.Items.IndexOf(ddlCondCategory.Items.FindByValue(scond.CategoryID.ToString()));
            if (scond.StatusID == Constants.CONDITIONSTATUSNOTSATISFIED) btnSatisfied.Visible = true;
            else btnSatisfied.Visible = false;
            if (scond.CreatedBy <= 0)
            {
                AllowSave(true);
                EnableConditionEdit(CanEditRuleCondition(scond.AuthorityLevel));
            }
            else if (scond.CreatedBy == CurrentUser.Id)
            {
                AllowSave(true);
                EnableConditionEdit(true);
            }
            else
            {
                AllowSave(false);
                EnableConditionEdit(false);
            }
            if (scond.CreatedBy > 0 && scond.CreatedBy == CurrentUser.Id)
            {
                divNoUComments.Visible = false;
                if (scond.Notes.Length > 0)
                {
                    tbCondNotes.Text = scond.Notes;
                    divUComments.Visible = true;
                    divAddUComments.Visible = false;
                    divUComments.Attributes.Clear();
                }
                else
                {
                    divAddUComments.Visible = true;
                    divUComments.Visible = true;
                    divUComments.Attributes.Add("style", "display:none");
                }
            }
            else if (scond.CreatedBy > 0 && scond.CreatedBy != CurrentUser.Id)
            {
                divNoUComments.Visible = true;
                divUComments.Visible = false;
                divAddUComments.Visible = false;
            }
            else if (scond.CreatedBy <= 0)
            {
                tbCondNotes.Text = scond.Notes;
                divNoUComments.Visible = false;
                divUComments.Visible = false;
                divUComments.Attributes.Clear();
                divAddUComments.Visible = false;
            }
            //set readonly for fields
            if (scond.CreatedBy > 0)
            {
                if (scond.CreatedBy == CurrentUser.Id || CurrentUser.IsInRole((int) AppUser.UserRoles.Underwriter))
                {
                    tbCondNotes.ReadOnly = false;
                    tbCondNotes.CssClass = "task";
                    AllowEditCondition(true);
                }
                else
                {
                    tbCondNotes.CssClass = "disabled";
                    tbCondNotes.ReadOnly = true;
                }
            }
            return scond.TaskID;
        }
        private void LoadTask()
        {
            Task stask = new Task(TaskID);
            tbTitle.Text = stask.Title;
            tbDescription.Text = stask.Description;
            if (stask.StatusID == Constants.TASKSTATUSACTIVE) btnCompleted.Visible = true;
            else btnCompleted.Visible = false;
            rdpTaskSchedule.SelectedDate = stask.ScheduleDate;
            ddlTaskRecurrence.SelectedIndex = ddlTaskRecurrence.Items.IndexOf(ddlTaskRecurrence.Items.FindByValue(stask.RecurrenceID.ToString()));
            ddlAssignedTo.SelectedIndex = ddlAssignedTo.Items.IndexOf(ddlAssignedTo.Items.FindByValue(stask.AssignedTo.ToString()));
            if (TaskID != -1)
            {
                //trNotes.Visible = true;
                if (ConditionID > 0)
                {
                    //Condition scond = new Condition(ConditionID);
                    //if (scond.Title != stask.Title || scond.Details != stask.Description)
                    //{
                    //    trTaskTitle.Visible = true;
                    //    trTaskDesc.Visible = true;
                    //}
                    //else
                    //{
                        trTaskTitle.Visible = false;
                        trTaskDesc.Visible = false;
                    //}
                }
                UpdateButtons();
            }
        }

        private void InitControls()
        {
            divNoUComments.Visible = false;
            btnCreate.Text = "Submit";
            btnCancel.Text = "Cancel";
            btnCompleted.Visible = false;
            btnSatisfied.Visible = false;
            lblActionError.Visible = false;
        }
        private void UpdateButtons()
        {
            btnCreate.Text = "Submit";
            btnCancel.Text = "Cancel Changes";
        }
        private void AllowSave(bool allow)
        {
            btnCreate.Enabled = allow;
            btnCompleted.Enabled = allow;
            btnSatisfied.Enabled = allow;
        }
        private void AllowEditCondition(bool allow)
        {
            tbCondTitle.Enabled = allow;
            tbCondDesc.Enabled = allow;
            ddlCondType.Enabled = allow;
            ddlCondCategory.Enabled = allow;
        }
        private void AllowEditTask(bool allow)
        {
            tbTitle.Enabled = allow;
            tbDescription.Enabled = allow;
            ddlTaskRecurrence.Enabled = allow;
            rdpTaskSchedule.Enabled = allow;
            ddlAssignedTo.Enabled = allow;
        }
        private bool CanEditRuleCondition(int AuthorityLevel)
        {
            bool res = false;
            if (CurrentUser.MaxAuthorityLevel() >= AuthorityLevel)
                res = true;
            return res;
        }
        private void EnableConditionEdit(bool enable)
        {
            tbCondTitle.Enabled = enable;
            tbCondDesc.Enabled = enable;
            ddlCondType.Enabled = enable;
            ddlCondCategory.Enabled = enable;
            btnSatisfied.Enabled = enable;
        }
        private void ShowEmail(bool show)
        {
            PanelTasks.Visible = !show;
            PanelEmailAdd.Visible = show;
        }

        private void UpdateNotes()
        {
            Notes notes = ((Default)Page).notes as Notes;
            if (notes != null)
            {
                notes.BindData();
            }
        }
        public void MortgageDataChanged()
        {
            CurrentPage.ReloadMortgage(MortgageID);

            Conditions cond = ((Default)Page).cond as Conditions;
            if (cond != null)
            {
                cond.Repopulate();
            }
            Notes notes = ((Default)Page).notes as Notes;
            if (notes != null)
            {
                notes.BindData();
            }
            ApplicantList appl = ((Default)this.Page).appList as ApplicantList;
            if (appl != null)
            {
                appl.RepopulateMortgageList();
            }
            Tabs tabs = ((Default)Page).tabs as Tabs;
            if (tabs != null)
            {
                tabs.SetTabColor();
            }
        }

        protected void LoadItem(string sid)
        {
            ClearConditionFields();
            ClearTaskFields();
            string type = sid.Substring(0, 1);
            int id = Convert.ToInt32(sid.Substring(1));
            switch (type)
            {
                case "C":
                    ConditionState = true;
                    ConditionID = id;
                    int? taskid = LoadCondition();
                    if (taskid.HasValue)
                    {
                        TaskID = taskid.Value;
                        LoadTask();
                    }
                    else
                    {
                        TaskID = -1;
                    }
                    btnCompleted.Visible = false;
                    break;
                case "T":
                    ConditionState = false;
                    ConditionID = 0;
                    TaskID = id;
                    LoadTask();
                    btnSatisfied.Visible = false;
                    break;
            }
        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ViewState[FIRST_LOAD] == null)
            {
                ConditionID = -1;
                TaskID = -1;
                ViewState[FIRST_LOAD] = 1;
                UpdateRuleConditions();
                InitControls();
                LoadMessageBoardPane();
                gridConditionTasks.Rebind();
                BindData();
            }
            else
            {
                gridConditionTasks.Rebind();
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (TaskID != -1 && !String.IsNullOrEmpty(tbQuickNote.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(tbQuickNote.Text.Trim()) && String.IsNullOrEmpty(ddlAction.SelectedValue))
                {
                    lblActionError.Visible = true;
                    return;
                }
            }
            if (!ConditionEmpty)
            {
                if (!TaskEmpty)
                {
                    if (SaveCondition())
                        SaveTask();
                    else
                        return;
                }
                else
                {
                    //if(trNotes.Visible)
                    //{
                        if (!String.IsNullOrEmpty(tbQuickNote.Text.Trim()) && String.IsNullOrEmpty(ddlAction.SelectedValue))
                        {
                            lblActionError.Visible = true;
                            return;
                        }
                    //}
                    if (!SaveCondition())
                        return;
                }
                MortgageDataChanged();
                gridConditionTasks.DataSource = Condition.GetConditionTaskList(MortgageID);
            }
            else if (!TaskEmpty)
            {
                ConditionID = -1;
                SaveTask();
            }
            ConditionState = true;
            gridConditionTasks.Rebind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            TaskID = -1;
            ConditionID = -1;
            ClearConditionFields();
            ClearTaskFields();
        }

        protected void btnCompleted_Click(object sender, EventArgs e)
        {
            Task task = new Task(TaskID);
            task.StatusID = Constants.TASKSTATUSCOMPLETED;
            task.Save();
            gridConditionTasks.Rebind();
            btnCompleted.Visible = false;
        }

        protected void btnSatisfied_Click(object sender, EventArgs e)
        {
            Condition cond = new Condition(ConditionID);
            cond.StatusID = Constants.CONDITIONSTATUSSATISFIED;
            cond.Save();
            btnSatisfied.Visible = false;
            MortgageDataChanged();
            gridConditionTasks.DataSource = Condition.GetConditionTaskList(MortgageID);
            gridConditionTasks.Rebind();
        }

        protected void lbAddCondition_Click(object sender, EventArgs e)
        {
            AddingNewItem = true;
            AllowEditCondition(true);
            AllowSave(true);
            TaskID = -1;
            ConditionID = -1;
            ConditionState = true;
            //trNotes.Visible = false;
            InitControls();
            ClearConditionFields();
            ClearTaskFields();
            gridConditionTasks.SelectedIndexes.Clear();
        }

        protected void lbAddTask_Click(object sender, EventArgs e)
        {
            AddingNewItem = true;
            AllowEditTask(true);
            AllowSave(true);
            TaskID = -1;
            InitControls();
            ClearConditionFields();
            ClearTaskFields();
            ConditionState = false;
            //trNotes.Visible = false;
            gridConditionTasks.SelectedIndexes.Clear();
        }

        protected void lbAddNote_Click(object sender, EventArgs e)
        {
            if (TaskID != -1)
            {
                Task stask = new Task(TaskID);
                DataRow note = stask.CreateNote();
                note["Note"] = tbQuickNote.Text;
                tbQuickNote.Text = "";
                stask.SaveNotes();
                LoadNotesPane(false);
            }
        }

        protected void lbNotes_Click(object sender, EventArgs e)
        {
            LoadNotesPane(true);
        }

        protected void CheckNoteAction(object source, ServerValidateEventArgs args)
        {
            if (!String.IsNullOrEmpty(tbQuickNote.Text.Trim()) && String.IsNullOrEmpty(ddlAction.SelectedValue))
                args.IsValid = false;
            else
                args.IsValid = true;
        }

        protected void EmailAdd1_MailSent(object sender, EventArgs e)
        {
            ShowEmail(false);
            BindData();
        }

        #region Grid events
        protected void gridConditionTasks_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            gridConditionTasks.DataSource = Condition.GetConditionTaskList(MortgageID);
        }
        protected void gridConditionTasks_PreRender(object sender, EventArgs e)
        {
            if (ConditionID > 0 || TaskID > 0)
            {
                foreach (GridDataItem item in gridConditionTasks.Items)
                {
                    string sid = item.OwnerTableView.DataKeyValues[item.ItemIndex]["SID"].ToString();
                    int id = Convert.ToInt32(sid.Substring(1));
                    if ((id == TaskID) || (id == ConditionID))
                    {
                        LoadItem(sid);
                        item.Selected = true;
                    }
                }
            }
        }
        protected void gridConditionTasks_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {

                DataRowView row = (DataRowView)e.Item.DataItem;
                GridDataItem item = e.Item as GridDataItem;
                if (item != null)
                {
                    if (Convert.ToInt16(row["HighLight"]) == 1)
                    {
                        item.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        item.ForeColor = System.Drawing.Color.Transparent;
                    }
                }
                Image img = (Image)e.Item.FindControl("imgDoc");
                if(img != null)
                {
                    if (String.Compare(row["Status"].ToString(), "Completed", true) == 0)
                        img.ImageUrl = "~/Images/doc_icon.gif";
                    else if (Convert.ToBoolean(row["Completed"].ToString()))
                        img.ImageUrl = "~/Images/doc_icon.gif";
                    else
                        img.Visible = false;                    
                }
            }
        }
        protected void gridConditionTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearConditionFields();
            ClearTaskFields();

            string sid = Convert.ToString(gridConditionTasks.MasterTableView.DataKeyValues[gridConditionTasks.SelectedItems[0].ItemIndex]["SID"]);
            string type = sid.Substring(0, 1);
            int id = Convert.ToInt32(sid.Substring(1));
            switch (type)
            {
                case "C":
                    ConditionState = true;
                    ConditionID = id;
                    int? taskid = LoadCondition();
                    if (taskid.HasValue)
                    {
                        TaskID = taskid.Value;
                        LoadTask();
                    }
                    break;
                case "T":
                    ConditionState = false;
                    TaskID = id;
                    LoadTask();
                    break;
            }

        }
        protected void gridConditionTasks_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "LoadItem")
            {
                string sid = Convert.ToString(gridConditionTasks.MasterTableView.DataKeyValues[e.Item.ItemIndex]["SID"]);
                //LoadItem(sid);
                ClearConditionFields();
                ClearTaskFields();
                string type = sid.Substring(0, 1);
                int id = Convert.ToInt32(sid.Substring(1));
                switch (type)
                {
                    case "C":
                        ConditionState = true;
                        ConditionID = id;
                        btnCompleted.Visible = false;
                        break;
                    case "T":
                        ConditionState = false;
                        ConditionID = 0;
                        TaskID = id;
                        btnSatisfied.Visible = false;
                        break;
                }
                gridConditionTasks.SelectedIndexes.Clear();
                gridConditionTasks.SelectedIndexes.Add(e.Item.ItemIndex);
            }
            else if (e.CommandName == "CreateEmail")
            {
                string sid = Convert.ToString(gridConditionTasks.MasterTableView.DataKeyValues[e.Item.ItemIndex]["SID"]);
                string type = sid.Substring(0, 1);
                int id = Convert.ToInt32(sid.Substring(1));
                ShowEmail(true);
                switch (type)
                {
                    case "C":
                        EmailAdd1.CondID = id;
                        break;
                    case "T":
                        EmailAdd1.TaskID = id;
                        break;
                    default:
                        EmailAdd1.TaskID = 0;
                        EmailAdd1.CondID = 0;
                        break;
                }
                EmailAdd1.MailID = 0;
                EmailAdd1.BindData();
                EmailAdd1.StartNewMail();
            }
            else if (e.CommandName == "CreateNote")
            {
                string sid = Convert.ToString(gridConditionTasks.MasterTableView.DataKeyValues[e.Item.ItemIndex]["SID"]);
                LoadItem(sid);
                gridConditionTasks.SelectedIndexes.Clear();
                gridConditionTasks.SelectedIndexes.Add(e.Item.ItemIndex);
                tbQuickNote.Focus();
                if (!CurrentPage.ClientScript.IsClientScriptBlockRegistered("focus"))
                    CurrentPage.ClientScript.RegisterClientScriptBlock(GetType(), "focus",
                    "<script language='javascript' type='text/javascript'>SetFocus('" + tbQuickNote.ClientID + "');</script>");
            }
            UpdateNotes();
        }
        #endregion
        #endregion
 */
    }
}