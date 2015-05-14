using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;
using System.Web.Services;

namespace LoanStarPortal.Controls
{
    public partial class FollowupConditions : AppControl
    {

        private const int INIT = 0;
        private const int NOCHANGE = 1;
        private const int NEXTWORKDATE = 2;
        private const int RESET = 3;

        //public event UpdateNeeded OnUpdateNeeded;
        //public delegate void UpdateNeeded();

        #region Fields/Properties
        public bool IsRed
        {
            get
            {
                MortgageProfile mp = CurrentPage.ReloadMortgage(MortgageID);
                return mp.DayToWorkUpdateNeeded;
            }
        }
        private const string FIRST_LOAD = "FollowupFirstLoad";
        private Condition cond;
        private bool hasEmail;
        private bool IsInEmailMode
        {
            get
            {
                bool res = false;
                Object o = Session["condition_message"];
                if (o != null)
                {
                    try
                    {
                        res = bool.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set { Session["condition_message"] = value; }
        }
        private int MortgageID
        {
            get
            {
                return Convert.ToInt32(Session[Constants.MortgageID]);
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
        private bool CanEditCondition(int AuthorityLevel)
        {
            bool res = false;
            if (CurrentUser.MaxAuthorityLevel() >= AuthorityLevel)
                res = true;
            return res;
        }
        private static DateTime GetNextSchedule(DateTime scheduleDate, int reccurence)
        {
            DateTime res = scheduleDate;
            switch (reccurence)
            {
                case 1: //everyday
                    res = scheduleDate.AddDays(1);
                    break;
                case 2: //Every other day
                    res = scheduleDate.AddDays(2);
                    break;
                case 3: //Once a week
                    res = scheduleDate.AddDays(7);
                    break;
                case 4: //Every other week
                    res = scheduleDate.AddDays(14);
                    break;
                case 5: //Once a month
                    res = scheduleDate.AddMonths(1);
                    break;
            }
            return res;
        }
        private bool IsActive
        {
            get
            {
                bool res = true;
                Object o = Session["activefollowuptab"];
                if (o != null)
                {
                    try
                    {
                        int tabid = int.Parse(o.ToString());
                        res = tabid == 0;
                    }
                    catch
                    {
                    }
                }
                return res;
            }
        }
        #endregion

        #region Methods

        protected void ShowEmail(bool show)
        {
            PanelTasks.Visible = !show;
            PanelEmailAdd.Visible = show;
            if (PanelEmailAdd.Visible)
            {
                IsInEmailMode = true;
                Session["assosiationtype"] = 1; //condition 
                EmailAdd1.SetContent();
            }
            else
            {
                Session.Remove("condition_message");
                Session.Remove("assosiationtype");
            }
        }
        protected void SetSatisfiedControls(bool satisfied)
        {
            btnSatisfy.Enabled = !satisfied;
            if (satisfied)
                btnSatisfy.Text = "Unsatisfy";
            else
                btnSatisfy.Text = "Satisfy";
            btnSatisfy.Enabled = (ConditionID > 0);
        }

        protected void ClearConditionFields()
        {
            tbCondTitle.Text = "";
            tbCondDesc.Text = "";
            ddlRecurrence.ClearSelection();
            rdpStartDate.SelectedDate = DateTime.Now;
            lblNextWorkDate.Text = "";
            SetSatisfiedControls(false);
            tbCondTitle.Enabled = true;
            tbCondDesc.Enabled = true;
        }
        protected void ClearFollowupControls()
        {
            ddlRecurrence.ClearSelection();
            rdpStartDate.SelectedDate = null;
            lblNextWorkDate.Text = "";
        }

        protected void AllowSaveCondition(int AuthorityLevel)
        {
            bool allow = CanEditCondition(AuthorityLevel);
            tbCondTitle.ReadOnly = !allow;
            tbCondDesc.ReadOnly = !allow;
            btnSatisfy.Visible = allow;
            if (String.Compare(btnSatisfy.Text, "Satisfied", StringComparison.OrdinalIgnoreCase) == 0)
                btnSatisfy.Enabled = false;
            else
                btnSatisfy.Enabled = allow;
        }
        protected void BuildUnderwriterUI()
        {
            bool CreditSatisfied = Condition.CheckConditionsSatisfied(Constants.CONDITIONCREDITCATEGORYID, MortgageID);
            bool PropertySatisfied = Condition.CheckConditionsSatisfied(Constants.CONDITIONPROPERTYCATEGORYID, MortgageID);
            if (CreditSatisfied || PropertySatisfied)
                PanelUnderwriter.Visible = true;
            else
                PanelUnderwriter.Visible = false;

            if (CreditSatisfied)
            {
                trCredit.Visible = true;
                btnPrintCCDE.Visible = chkCredit.Checked;
            }
            if (PropertySatisfied)
            {
                trProperty.Visible = true;
                btnPrintDE.Visible = chkProperty.Checked;
            }
        }
        protected void BindAuthLevel()
        {
            ddlAuthLevel.DataSource = Role.GetRoleTemplateListForAuthorityLevel();
            ddlAuthLevel.DataTextField = Role.NAMEFIELDNAME;
            ddlAuthLevel.DataValueField = "id";
            ddlAuthLevel.DataBind();
        }
        protected void BindRecurrence()
        {
            DataView dv = Condition.GetRecurrenceList();
            dv.RowFilter = "id<6";
            ddlRecurrence.DataSource = dv;
            ddlRecurrence.DataBind();
            ddlRecurrence.Items.Insert(0, new ListItem(" Select ", "0"));
        }
        protected void BindData()
        {
            BindAuthLevel();
            BindRecurrence();
            rdpStartDate.SelectedDate = DateTime.Now;
            gridConditions.Rebind();
        }

        private static string GetEventTitle(string str)
        {
            if (str.Length > 50) str = str.Substring(0, 50);
            return str;
        }
        public void MortgageDataChanged()
        {
            CurrentPage.CenterRightPanelUpdateNeeded = true;

            CurrentPage.ReloadMortgage(MortgageID);

            Conditions cond_ = ((Default)Page).cond;
            if (cond_ != null)
            {
                cond_.Repopulate();
            }
            Notes notes = ((Default)Page).notes;
            if (notes != null)
            {
                notes.BindData();
            }
            ApplicantList appl = ((Default)Page).appList;
            if (appl != null)
            {
                appl.RepopulateMortgageList();
            }
            Tabs tabs = ((Default)Page).tabs;
            if (tabs != null)
            {
                tabs.SetTabColor();
            }
        }
        private void LoadCondition()
        {
            cond = new Condition(ConditionID);
            tbCondTitle.Text = cond.Title;
            tbCondDesc.Text = cond.Description;
            ddlAuthLevel.ClearSelection();
            ListItem li = ddlAuthLevel.Items.FindByValue(cond.AuthorityLevel.ToString());
            if (li != null) li.Selected = true;
            string authLevel = "";
            if (ddlAuthLevel.SelectedItem != null)
            {
                authLevel = ddlAuthLevel.SelectedItem.Text;
            }
            lblAuthLevel.Text = authLevel;
            lblAuthLevel.Visible = ConditionID > 0;
            ddlAuthLevel.Visible = !lblAuthLevel.Visible;
            SetSatisfiedControls(cond.StatusID == Constants.CONDITIONSTATUSSATISFIED);
            AllowSaveCondition(cond.AuthorityLevelValue);

            if (cond.RecurrenceID > 0 && cond.ScheduleDate != null)
            {
                ddlRecurrence.Items.FindByValue(cond.RecurrenceID.ToString()).Selected = true;
            }
            SetDates(cond, INIT);
            chkCredit.Checked = cond.CreditApproved;
            chkProperty.Checked = cond.PropertyApproved;
        }

        private void SetDates(Condition cond_, int opt)
        {
            switch (opt)
            {
                case INIT:
                    if (cond_.RecurrenceID > 0 && cond_.ScheduleDate != null && cond_.NextFollowUpDate != null)
                    {
                        rdpStartDate.SelectedDate = cond_.ScheduleDate;
                        lblNextWorkDate.Text = ((DateTime)cond_.NextFollowUpDate).ToString("d");
                    }
                    else
                    {
                        rdpStartDate.SelectedDate = DateTime.Now;
                        lblNextWorkDate.Text = "";
                    }
                    break;
                case NOCHANGE:
                    break;
                case NEXTWORKDATE:
                    if (cond_.RecurrenceID > 0 && cond_.NextFollowUpDate != null)
                    {
                        lblNextWorkDate.Text = ((DateTime)cond_.NextFollowUpDate).ToString("d");
                    }
                    else
                    {
                        lblNextWorkDate.Text = "";
                    }
                    break;
                case RESET:
                    lblNextWorkDate.Text = "";
                    break;

            }
        }
        protected void RebindGrid()
        {
            gridConditions.DataSource = null;
            gridConditions.DataBind();
        }
        protected int SaveCondition()
        {
            Condition scond = new Condition(ConditionID);
            if ((ConditionID <= 0) || (ConditionID > 0 && CanEditCondition(scond.AuthorityLevel)))
            {
                scond.Title = tbCondTitle.Text;
                scond.Description = tbCondDesc.Text.Trim();
                scond.MortgageID = MortgageID;
                scond.AuthorityLevelRoleId = Convert.ToInt32(ddlAuthLevel.SelectedValue);
                if (ConditionID <= 0)
                {
                    scond.StatusID = Constants.CONDITIONSTATUSNOTSATISFIED;
                    scond.CreatedBy = CurrentUser.Id;
                    scond.AuthorityLevelRoleId = Convert.ToInt32(ddlAuthLevel.SelectedValue);

                    string begining;
                    if (String.Compare(ddlRecurrence.SelectedValue, "0") != 0)
                        begining = "Follow-up schedule created: ";
                    else
                        begining = "Condition created: ";

                    SaveEvent(Constants.EVENTTYPEIDCONDITIONCREATED, begining + "<b>" + GetEventTitle(scond.Title) + "</b> by " + CurrentUser.FirstName + " " + CurrentUser.LastName + " at " + DateTime.Now.ToString("f"));
                }
                scond.ScheduleDate = rdpStartDate.SelectedDate;
                scond.RecurrenceID = ddlRecurrence.SelectedIndex;
                scond.Save();
                scond.SaveFollowUpDetails();
            }
            return scond.ID;
        }

        private void ReloadMessageBoard(int conditionId)
        {
            Notes notes = ((Default)Page).notes;
            if (notes != null)
            {
                if (conditionId > 0)
                {
                    notes.CurrentFilter.ConditionId = conditionId;
                    notes.BindData();
                }
                else
                {
                    if (notes.CurrentFilter.ConditionId != null)
                    {
                        notes.CurrentFilter.ConditionId = null;
                        notes.BindData();
                    }
                }
            }
            MortgageDataChanged();
        }
        private void ResetConditionFilter()
        {
            Notes notes = ((Default)Page).notes;
            if (notes != null)
            {
                notes.CurrentFilter.ConditionId = null;
            }
        }
        private void BindFirstItem()
        {
            if (gridConditions.MasterTableView.Items.Count > 0)
            {
                int id;
                int.TryParse(gridConditions.MasterTableView.DataKeyValues[0]["ID"].ToString(), out id);
                if (id > 0)
                {
                    ConditionID = id;
                    LoadCondition();
                }
            }
        }
        protected void ClearFollowUpDetails(Condition condition)
        {
            condition.RecurrenceID = 0;
            condition.ScheduleDate = null;
            condition.SaveFollowUpDetails();
            SaveEvent(Constants.EVENTTYPEIDFOLLOWUPCHANGED, "Follow-up schedule for \"" + condition.Title + "\" was changed by " + CurrentUser.FirstName + " " + CurrentUser.LastName + " at " + DateTime.Now.ToString("f"));
        }
        protected void SaveEvent(int typeid, string description)
        {
            Event.Save(MortgageID, typeid, description);
        }
        #endregion

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsActive) return;
            hasEmail = CurrentUser.HasEmail;
            if (ViewState[FIRST_LOAD] == null)
            {
                ConditionID = -1;
                IsInEmailMode = false;
                ViewState[FIRST_LOAD] = 1;
                BindData();
                BindFirstItem();
                ResetConditionFilter();
                BuildUnderwriterUI();
            }
            else
            {
                if (CheckClosingEmail())
                {
                    ShowEmail(false);
                    ReloadMessageBoard(ConditionID);
                }
                gridConditions.Rebind();
            }
        }
        private bool CheckClosingEmail()
        {
            bool res = false;
            Object o = Session["closingemail"];
            if (o != null)
            {
                try
                {
                    res = bool.Parse(o.ToString());
                }
                catch
                {
                }
                Session.Remove("closingemail");
            }
            return res;

        }

        protected void EmailAdd1_MailSent(object sender, EventArgs e)
        {
            ShowEmail(false);
            BindData();
        }
        #region Grid
        protected void gridConditions_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            gridConditions.DataSource = Condition.GetConditionsList(MortgageID);
        }

        protected void gridConditions_DetailTableDataBind(object source,
            GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            ConditionID = (Int32)dataItem.GetDataKeyValue("ID");
            LoadCondition();
            e.DetailTableView.DataSource = cond.Data;

        }

        protected void gridConditions_PreRender(object sender, EventArgs e)
        {
                foreach (GridDataItem item in gridConditions.Items)
                {
                    if (item.OwnerTableView.Name == "Description") continue;
                    int id = Convert.ToInt32(item.OwnerTableView.DataKeyValues[item.ItemIndex]["ID"].ToString());
                    if (ConditionID == id)
                    {
                        item.Selected = true;
                    }
                    DataRowView row = (DataRowView)item.DataItem;

                    if (row != null)
                    {
                        var diffDays = row["DiffDays"].ToString();
                        if (diffDays.Length > 0)
                        {
                            item.ForeColor = Convert.ToInt16(row["DiffDays"]) >= 0
                                ? System.Drawing.Color.Red
                                : System.Drawing.Color.Black;
                        }
                        
                        var isCompleted = Convert.ToInt32(row["StatusId"].ToString())==1;

                        if (isCompleted)
                        {
                            item.ForeColor = System.Drawing.Color.Gray;
                        }

                        Image img = (Image)item.FindControl("imgDoc");
                        if (img != null)
                        {
                            if (
                                String.Compare(row["Status"].ToString(), "Completed",
                                    StringComparison.OrdinalIgnoreCase) == 0
                                || isCompleted)
                            {
                                img.ImageUrl = "~/Images/doc_icon.gif";
                            }
                            else
                            {
                                img.Visible = false;
                            }
                        }
                    }
                    if (hasEmail) continue;
                    ImageButton btn = (ImageButton)item.FindControl("ibtnEmail");
                    if (btn != null)
                    {
                        btn.Visible = false;
                    }
                }
        }
        protected void gridConditions_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "LoadItem")
            {
                ClearConditionFields();
                int id = Convert.ToInt32(gridConditions.MasterTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
                ConditionID = id;
                LoadCondition();

                ReloadMessageBoard(ConditionID);

                gridConditions.SelectedIndexes.Clear();
                gridConditions.SelectedIndexes.Add(e.Item.ItemIndex);
                panel_dialog.Visible = true;
            }
            else if (e.CommandName == "CreateEmail")
            {
                int id = Convert.ToInt32(gridConditions.MasterTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString());
                ConditionID = id;
                LoadCondition();

                ReloadMessageBoard(ConditionID);

                ShowEmail(true);
                panel_dialog.Visible = true;
            }
            else if (e.CommandName == "CreateNote")
            {
                ClearConditionFields();
                ClearFollowupControls();
                int id = Convert.ToInt32(gridConditions.MasterTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString());
                ConditionID = id;
                LoadCondition();
                gridConditions.SelectedIndexes.Clear();
                gridConditions.SelectedIndexes.Add(e.Item.ItemIndex);

                panel_dialog.Visible = true;
            }
        }
        protected void gridConditions_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {


            }
        }
        protected void gridConditions_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            ClearConditionFields();
            ConditionID = 0;
        }
        #endregion
        protected void RefreshPage_Click(object sender, EventArgs e)
        {
            ClearFollowupControls();
            BindRecurrence();

            int id = Convert.ToInt32(conditionActiveID.Value);
            ConditionID = id;
            LoadCondition();

            MortgageDataChanged();
            RebindGrid();
        }

        protected void btnSatisfy_Click(object sender, EventArgs e)
        {
            if (ConditionID > 0)
            {
                Condition cond_ = new Condition(ConditionID);
                int newStatusId = Constants.CONDITIONSTATUSSATISFIED;
                if (cond_.StatusID == Constants.CONDITIONSTATUSSATISFIED)
                {
                    newStatusId = Constants.CONDITIONSTATUSNOTSATISFIED;
                }
                cond_.StatusID = newStatusId;
                cond_.Save();
                if (newStatusId == Constants.CONDITIONSTATUSNOTSATISFIED)
                {
                    SaveEvent(Constants.EVENTTYPEIDCONDITIONUNSATISFIED, "Condition " + cond_.Title + " was unsatisfied by " + CurrentUser.FirstName + " " + CurrentUser.LastName + " at " + DateTime.Now.ToString("f"));
                }
                else
                {
                    SaveEvent(Constants.EVENTTYPEIDCONDITIONSATISFIED, "Condition " + cond_.Title + " was satisfied by " + CurrentUser.FirstName + " " + CurrentUser.LastName + " at " + DateTime.Now.ToString("f"));
                }

                if (cond_.RecurrenceID > 0 && cond_.ScheduleDate != null)
                    ClearFollowUpDetails(cond_);
                SetSatisfiedControls(newStatusId == Constants.CONDITIONSTATUSSATISFIED);
                MortgageDataChanged();
                RebindGrid();
                BuildUnderwriterUI();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ConditionID = SaveCondition();
            lblAuthLevel.Visible = !ddlAuthLevel.Visible;
            MortgageDataChanged();
            Condition cond_ = new Condition(ConditionID);
            SetDates(cond_, NEXTWORKDATE);
            MortgageDataChanged();
            RebindGrid();
            panel_dialog.Visible = false;
        }
        protected void btnShowNotes_Click(object sender, EventArgs e)
        {
            ReloadMessageBoard(ConditionID);
        }

        protected void btnAddCondition_Click(object sender, EventArgs e)
        {
            ConditionID = -1;
            ddlAuthLevel.Visible = true;
            lblAuthLevel.Visible = !ddlAuthLevel.Visible;
            btnSatisfy.Enabled = false;
            
            ReloadMessageBoard(-1);
            ClearConditionFields();
            ddlRecurrence.SelectedIndex = 3;
            panel_dialog.Visible = true;
        }

        protected void chkCredit_CheckedChanged(object sender, EventArgs e)
        {
            btnPrintCCDE.Visible = chkCredit.Checked;
            Condition.UpdateCreditApproved(ConditionID, chkCredit.Checked);
            BuildUnderwriterUI();
        }
        protected void chkProperty_CheckedChanged(object sender, EventArgs e)
        {
            btnPrintDE.Visible = chkProperty.Checked;
            Condition.UpdatePropertyApproved(ConditionID, chkProperty.Checked);
            BuildUnderwriterUI();
        }
        protected void SetStatusToCompleted(int _conditionId)
        {
            ConditionID = _conditionId;
            Condition cond_ = new Condition(ConditionID);
            cond_.Completed = true;
            cond_.Save();
        }

        #endregion

        [WebMethod]
        public static string AdvanceEvent()
        {
            return DateTime.Now.ToString();
        }

        protected void btnHideDialog_Click(object sender, EventArgs e)
        {
            panel_dialog.Visible = false;
        }

    }
}