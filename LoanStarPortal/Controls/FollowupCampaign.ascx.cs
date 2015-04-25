using System;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class FollowupCampaign : AppControl
    {

        #region constants
        #endregion

        #region delegates
        //public event UpdateNeeded OnUpdateNeeded;
        //public delegate void UpdateNeeded();
        #endregion

        #region fields
        private bool hasEmail;
        private DataView dvCampaigns;
        private MortgageProfile mp;
        private LeadCampaign campaign;
        #endregion

        #region properties
        public bool HasCampaigns
        {
            get { return DvCampaigns.Count > 0; }
        }
        public bool IsRed
        {
            get
            {
                return Mp.CampaignUpdateNeeded;
            }
        }
        private int SelectedIndex
        {
            get
            {
                int res = -1;
                Object o = Session["campaigngridselectedindex"];
                if(o!=null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set { Session["campaigngridselectedindex"] = value; }
        }
        private bool IsActive
        {
            get
            {
                bool res = false;
                Object o = Session["activefollowuptab"];
                if (o != null)
                {
                    try
                    {
                        int tabid = int.Parse(o.ToString());
                        res = tabid == 1;
                    }
                    catch
                    {
                    }
                }
                return res;
            }
        }
        private LeadCampaign Campaign
        {
            get
            {
                if(campaign==null||campaign.ID!=CampaignId)
                {
                    campaign = new LeadCampaign(CampaignId, MortgageID);
                }
                return campaign;
            }
        }
        private int SelectedMonth
        {
            get
            {
                int res = DateTime.Now.Month;
                Object o = Session["campaignselectedmonth"];
                if(o!=null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set { Session["campaignselectedmonth"] = value; }
        }
        private int SelectedYear
        {
            get
            {
                int res = DateTime.Now.Year;
                Object o = Session["campaignselectedyear"];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set { Session["campaignselectedyear"] = value; }

        }
        private MortgageProfile Mp
        {
            get
            {
                if(mp==null)
                {
                    mp = CurrentPage.GetMortgage(MortgageID);
                }
                return mp;
            }
        }
        private DataView DvCampaigns
        {
            get
            {
                if(dvCampaigns==null)
                {
                    dvCampaigns = LeadCampaign.GetCompanyActiveCampignsForMortgage(CurrentUser.CompanyId,MortgageID);
                }
                return dvCampaigns;
            }
    }
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
        private int CampaignId
        {
            get
            {
                int res = -1;
                Object o = Session["campaignid"];
                if(o!=null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set { Session["campaignid"] = value; }
        }
        private bool IsFirstLoaded
        {
            get
            {
                bool res = true;
                Object o = ViewState["campaignfirstload"];
                if(o!=null)
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
            set { ViewState["campaignfirstload"] = value; }
        }
        private int MortgageID
        {
            get
            {
                return Convert.ToInt32(Session[Constants.MortgageID]);
            }
        }
        #endregion
        #region Fields/Properties
        //private const string FIRST_LOAD = "FollowupFirstLoad";
        //private Condition cond;
        //private bool hasEmail;
        //private bool IsInEmailMode
        //{
        //    get
        //    {
        //        bool res = false;
        //        Object o = Session["condition_message"];
        //        if (o != null)
        //        {
        //            try
        //            {
        //                res = bool.Parse(o.ToString());
        //            }
        //            catch
        //            {
        //            }
        //        }
        //        return res;
        //    }
        //    set { Session["condition_message"] = value; }
        //}
        //private int MortgageID
        //{
        //    get
        //    {
        //        return Convert.ToInt32(Session[Constants.MortgageID]);
        //    }
        //}
        //private int ConditionID
        //{
        //    get
        //    {
        //        if (Session["ConditionID"] == null)
        //            return -1;
        //        else
        //            return Convert.ToInt32(Session["ConditionID"]);
        //    }
        //    set
        //    {
        //        Session["ConditionID"] = value;
        //    }
        //}
        //private bool CanEditCondition(int AuthorityLevel)
        //{
        //    bool res = false;
        //    if (CurrentUser.MaxAuthorityLevel() >= AuthorityLevel)
        //        res = true;
        //    return res;
        //}
        //private static DateTime GetNextSchedule(DateTime scheduleDate, int reccurence)
        //{
        //    DateTime res = scheduleDate;
        //    switch (reccurence)
        //    {
        //        case 1: //everyday
        //            res = scheduleDate.AddDays(1);
        //            break;
        //        case 2: //Every other day
        //            res = scheduleDate.AddDays(2);
        //            break;
        //        case 3: //Once a week
        //            res = scheduleDate.AddDays(7);
        //            break;
        //        case 4: //Every other week
        //            res = scheduleDate.AddDays(14);
        //            break;
        //        case 5: //Once a month
        //            res = scheduleDate.AddMonths(1);
        //            break;
        //    }
        //    return res;
        //}
        #endregion

        #region methods
        public void MortgageDataChanged()
        {
            CurrentPage.CenterRightPanelUpdateNeeded = true;

            CurrentPage.ReloadMortgage(MortgageID);

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
        private void ReloadMessageBoard(int campaignId)
        {
            Notes notes = ((Default)Page).notes;
            if (notes != null)
            {
                if (campaignId > 0)
                {
                    notes.CurrentFilter.CampaignId = campaignId;
                    notes.BindData();
                }
                else
                {
                    if (notes.CurrentFilter.CampaignId != null)
                    {
                        notes.CurrentFilter.CampaignId = null;
                    }
                    notes.BindData();
                }
                CurrentPage.CenterRightPanelUpdateNeeded = true;
            }
        }
        private string GetCampaignData()
        {
            string res = "";
            if(CampaignId > 0)
            {
                res = String.Format("{0}:{1}:{2}:{3}", CampaignId, MortgageID, DateTime.Now.Year, DateTime.Now.Month);
            }
            return res;
        }
        private void LoadCampaign()
        {
            Page.ClientScript.RegisterHiddenField("currentcampaigndata", GetCampaignData());
            tbCampaignTitle.Text = Campaign.Title;
            tbCampaignDetails.Text = Campaign.Detail;
            BindCampaignDates(SelectedYear,SelectedMonth);
            BindActions();
        }
        private void BindActions()
        {
            ddlCampaignAction.Items.Clear();
            ddlCampaignAction.Items.Add(new ListItem("-Select-",""));
            ddlCampaignAction.Items.Add(new ListItem("Generic campaign note", LeadCampaign.GENERICNOTEACTIONID.ToString()));
            if(Campaign.NextfollowupDate!=null)
            {
                ddlCampaignAction.Items.Add(new ListItem("Not complete. Progress follow-up date", LeadCampaign.NEXTFOLLOWUPDATEACTIONID.ToString()));
            }
            if(!Campaign.IsCompleted)
            {
                ddlCampaignAction.Items.Add(new ListItem("Stop campaign for lead", LeadCampaign.COMPLETEDACTIONID.ToString()));
            }

        }

        private void BindCampaignDates(int year, int month)
        {
            DateTime? dt = Campaign.GetStartDate(Mp,false);
            lblStartDate.Text = String.Format("{0} ({1}{2})", Campaign.Startdate, dt == null ? "N/A" : ((DateTime)dt).ToShortDateString(),Campaign.GetDayOffset());
            
            rdcSchedule.SelectedDates.Clear();
            ArrayList selection = Campaign.GetScheduleDates(Mp, year, month);
            for (int i = 0; i < selection.Count; i++)
            {
                RadDate d1 = new RadDate((DateTime)selection[i]);
                rdcSchedule.SelectedDates.Add(d1);
            }
            if(!Campaign.IsCompleted)
            {
                dt = Campaign.NextfollowupDate;
                lblNextFollowupDate.Text = String.Format("Next follow-up date: {0}", dt == null ? "N/A" : ((DateTime) dt).ToShortDateString());
            }
            else
            {
                lblNextFollowupDate.Text = "Campaign is completed";
            }
        }
        private void BindFirstItem()
        {
            if (gCampaigns.MasterTableView.Items.Count > 0)
            {
                int id;
                int.TryParse(gCampaigns.MasterTableView.DataKeyValues[0]["ID"].ToString(), out id);
                if (id > 0)
                {
                    CampaignId = id;
                    gCampaigns.SelectedIndexes.Clear();
                    gCampaigns.SelectedIndexes.Add(0);
                    LoadCampaign();
                }
            }
        }
        private void BindGrid()
        {
            DataView dv = DvCampaigns;

            dv.RowFilter = Mp.CurProfileStatusID == MortgageProfile.LEADSTATUSID
                               ? "leadsallowed<>2"
                               : "leadsallowed<>3";
            gCampaigns.DataSource = dv;
            gCampaigns.DataBind();
            if(SelectedIndex>=0)
            {
                gCampaigns.SelectedIndexes.Clear();
                gCampaigns.SelectedIndexes.Add(SelectedIndex);
            }
        }
        private void ReBindGrid()
        {
            dvCampaigns = null;
            BindGrid();
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
        protected void ShowEmail(bool show)
        {
            PanelTasks.Visible = !show;
            PanelEmailAdd.Visible = show;
            if (PanelEmailAdd.Visible)
            {
                IsInEmailMode = true;
                Session["assosiationtype"] = 2; //campaign
                EmailAdd1.SetContent();
            }
            else
            {
                Session.Remove("condition_message");
                Session.Remove("assosiationtype");
            }
        }
        protected void EmailAdd1_MailSent(object sender, EventArgs e)
        {
            ShowEmail(false);
            BindGrid();
        }
        private void ReloadMessageBoard()
        {
            if (btnShowCampaignNotes.Text == "Campaign notes")
            {
                ReloadMessageBoard(-1);
            }
            else
            {
                ReloadMessageBoard(CampaignId);
            }
        }

        #endregion
        
        #region grid related
        protected void gCampaigns_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "LoadItem")
            {
                CampaignId = Convert.ToInt32(gCampaigns.MasterTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
                LoadCampaign();
                gCampaigns.SelectedIndexes.Clear();
                gCampaigns.SelectedIndexes.Add(e.Item.ItemIndex);
                SelectedIndex = e.Item.ItemIndex;
            }
            else if (e.CommandName == "CreateEmail")
            {
                int id = Convert.ToInt32(gCampaigns.MasterTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString());
                CampaignId = id;
                LoadCampaign();
                SelectedIndex = e.Item.ItemIndex;
                if (btnShowCampaignNotes.Text == "Campaign notes")
                {
                    ReloadMessageBoard(-1);
                }
                else
                {
                    ReloadMessageBoard(CampaignId);
                }
                ShowEmail(true);
            }
            else if (e.CommandName == "CreateNote")
            {
                int id = Convert.ToInt32(gCampaigns.MasterTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString());
                CampaignId = id;
                LoadCampaign();
                SelectedIndex = e.Item.ItemIndex;
                gCampaigns.SelectedIndexes.Clear();
                gCampaigns.SelectedIndexes.Add(e.Item.ItemIndex);
                tbNote.Focus();
                if (!CurrentPage.ClientScript.IsClientScriptBlockRegistered("focus"))
                {
                    CurrentPage.ClientScript.RegisterClientScriptBlock(GetType(), "focus",
                        "<script language='javascript' type='text/javascript'>SetFocus('" + tbNote.ClientID + "');</script>");
                }
            }
        }
        protected void gCampaigns_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {

                DataRowView row = (DataRowView)e.Item.DataItem;
                GridDataItem item = e.Item as GridDataItem;
                if (item != null)
                {
                    bool isRed = false;
                    bool isCompleted = bool.Parse(row["iscompleted"].ToString());
                    if(!isCompleted)
                    {
                        if(row["NextFollowupdate"]!=DBNull.Value)
                        {
                            DateTime dt = DateTime.Parse(row["NextFollowupdate"].ToString());
                            isRed = dt <= DateTime.Now;
                        }
                    }
                    if (isRed)
                    {
                        item.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        item.ForeColor = System.Drawing.Color.Black;
                    }
                }
                if (!hasEmail)
                {
                    ImageButton btn = (ImageButton)e.Item.FindControl("ibtnEmail");
                    if (btn != null)
                    {
                        btn.Visible = false;
                    }
                }
            }
        }
        protected void gCampaigns_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            CampaignId = -1;
        }
        #endregion


        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsActive) return;
            hasEmail = CurrentUser.HasEmail;
            if(IsFirstLoaded)
            {
                IsFirstLoaded = false;
                CampaignId = -1;
                IsInEmailMode = false;
                BindGrid();
                BindFirstItem();
                Session.Remove("campaigngridselectedindex");
            }
            else
            {
                if (CheckClosingEmail())
                {
                    ShowEmail(false);
                    ReloadMessageBoard();
                }
                BindGrid();
            }
        }
        protected void btnShowCampaignNotes_Click(object sender, EventArgs e)
        {
            if (btnShowCampaignNotes.Text == "Campaign notes")
            {
                btnShowCampaignNotes.Text = "Remove filter";
                ReloadMessageBoard(CampaignId);
            }
            else
            {
                btnShowCampaignNotes.Text = "Campaign notes";
                ReloadMessageBoard(-1);
            }
        }
        protected void btnSubmitCampaignNote_Click(object sender, EventArgs e)
        {
            if(CampaignId>0)
            {
                int actionId = int.Parse(ddlCampaignAction.SelectedValue);
                Campaign.SaveNote(MortgageID,CurrentUser.Id,actionId,tbNote.Text);
                tbNote.Text = "";
                ddlCampaignAction.ClearSelection();
                if(actionId>1)
                {
                    mp = CurrentPage.ReloadMortgage(MortgageID);
                    ReBindGrid();
                    BindCampaignDates(SelectedYear, SelectedMonth);
                    MortgageDataChanged();
                }
                ReloadMessageBoard();
            }
        }
        #endregion
    }
}