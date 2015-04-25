using System;
using System.Web.UI.WebControls;
using LoanStar.Common;



namespace LoanStarPortal.Administration.Controls
{
    public partial class EditLeadCampaign : AppControl
    {
        #region constants
        #endregion

        #region fields
        #endregion

        #region delegates
        public event CancelHandler OnCancel;
        public delegate void CancelHandler();
        public event SaveHandler OnSave;
        public delegate void SaveHandler();
        #endregion

        #region properties
        private LeadCampaign CurrentCampaign
        {
            get
            {
                return (LeadCampaign)Session["currentleadcampaign"];
            }
            set
            {
                Session["currentleadcampaign"] = value;
            }
        }
        #endregion
        #region methods
        private void BindStartDate()
        {
            ddlStartDate.DataSource = LeadCampaign.GetStartDateList();
            ddlStartDate.DataTextField = "name";
            ddlStartDate.DataValueField = "id";
            ddlStartDate.DataBind();
            ListItem sel = ddlStartDate.Items.FindByValue(CurrentCampaign.StartDateId.ToString());
            if (sel != null)
            {
                sel.Selected = true;
            }
            ddlSign.Items.Clear();
            ddlSign.Items.Add(new ListItem("+","0"));
            ddlSign.Items.Add(new ListItem("-", "1"));
            if(CurrentCampaign.StartDayOffset>=0)
            {
                ddlSign.SelectedValue = "0";
            }
            else
            {
                ddlSign.SelectedValue = "1";
            }
            tbDayOffset.Value = Math.Abs(CurrentCampaign.StartDayOffset);
        }
        private void BindFrequency()
        {
            ddlFrequency.DataSource = LeadCampaign.GetRecurrenceList();
            ddlFrequency.DataTextField = "name";
            ddlFrequency.DataValueField = "id";
            ddlFrequency.DataBind();
            ListItem sel = ddlFrequency.Items.FindByValue(CurrentCampaign.RecurrenceId.ToString());
            if (sel != null)
            {
                sel.Selected = true;
            }
        }
        private void BindLeadsType()
        {
            ddlLeadsType.Items.Clear();
            ddlLeadsType.Items.Add(new ListItem("Both",LeadCampaign.LEADTYPEBOTH.ToString()));
            ddlLeadsType.Items.Add(new ListItem("Managed Leads", LeadCampaign.LEADTYPEMANAGED.ToString()));
            ddlLeadsType.Items.Add(new ListItem("User created Leads", LeadCampaign.LEADTYPEUSERCREATED.ToString()));
            ListItem sel = ddlLeadsType.Items.FindByValue(CurrentCampaign.LeadsAllowed.ToString());
            if(sel!=null)
            {
                sel.Selected = true;
            }
        }

        private void BindCampaignData()
        {
            BindStartDate();
            BindFrequency();
            BindLeadsType();
            tbTitle.Text = CurrentCampaign.Title;
            tbDetail.Text = CurrentCampaign.Detail;
            cbIsOn.Checked = CurrentCampaign.IsOn;
            cbIsOnlyWorkingDayAllowed.Checked = CurrentCampaign.IsOnlyWorkingDayAllowed;
        }
        public void BindData()
        {
            BindCampaignData();
        }
        #endregion

        #region event handlers

        protected void btnSave_Click(object sender, EventArgs e)
        {
            CurrentCampaign.Title = tbTitle.Text;
            CurrentCampaign.Detail = tbDetail.Text;
            CurrentCampaign.IsOn = cbIsOn.Checked;
            bool isRecalculationNeeded = CurrentCampaign.IsOnlyWorkingDayAllowed != cbIsOnlyWorkingDayAllowed.Checked;
            CurrentCampaign.IsOnlyWorkingDayAllowed = cbIsOnlyWorkingDayAllowed.Checked;
            int leadType = int.Parse(ddlLeadsType.SelectedValue);
            isRecalculationNeeded |= CurrentCampaign.LeadsAllowed != leadType;
            CurrentCampaign.LeadsAllowed = leadType;
            int dayoffset = (int) tbDayOffset.Value;
            if(ddlSign.SelectedValue=="1")
            {
                dayoffset = -dayoffset;
            }
            isRecalculationNeeded |= CurrentCampaign.StartDayOffset != dayoffset;
            CurrentCampaign.StartDayOffset = dayoffset;
            int startDateId = int.Parse(ddlStartDate.SelectedValue);
            isRecalculationNeeded |= CurrentCampaign.StartDateId != startDateId;
            CurrentCampaign.StartDateId = startDateId;
            int recurrenceId = int.Parse(ddlFrequency.SelectedValue);
            isRecalculationNeeded |= CurrentCampaign.RecurrenceId != recurrenceId;
            CurrentCampaign.RecurrenceId = recurrenceId;
            CurrentCampaign.IsRecalculationNeeded = isRecalculationNeeded;
            if (OnSave != null)
            {
                OnSave();
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if(OnCancel!=null)
            {
                OnCancel();
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}