using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class Followup : AppControl
    {

        private const string STYLEATTRIBUTE = "style";
        private const string REDCOLOR = "color:Red";

        public event UpdateNeeded OnUpdateNeeded;
        public delegate void UpdateNeeded();

        private bool isCampaignVisible = false;

        #region properties
        private int ActiveTabIndex
        {
            get
            {
                int res = 0;
                Object o = Session["activefollowuptab"];
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
            set { Session["activefollowuptab"] = value; }
        }
        private bool IsFirstLoad
        {
            get
            {
                bool res = true;
                Object o = ViewState["followupfirstload"];
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
            set { ViewState["followupfirstload"] = value; }
        }
        private int MortgageID
        {
            get
            {
                return Convert.ToInt32(Session[Constants.MortgageID]);
            }
        }
        #endregion
        private void SetNoteFilterMode(int mode)
        {
            Notes notes = ((Default)Page).notes;
            if (notes != null)
            {
                notes.CurrentFilter.Mode = mode; 
            }
        }
        private void InitTabs()
        {
            rtsFollowup.SelectedIndex = 0;
            rmpFollowup.SelectedIndex = rtsFollowup.SelectedIndex;
        }
        private void SetTabsColor()
        {
            rtsFollowup.Tabs[0].Attributes.Add(STYLEATTRIBUTE, FollowupConditions1.IsRed ? REDCOLOR : "");
            if(rtsFollowup.Tabs[1].Visible)
            {

                rtsFollowup.Tabs[1].Attributes.Add(STYLEATTRIBUTE, FollowupCampaign1.IsRed? REDCOLOR : "");
            }
        }
        private void SetTabsVisibility()
        {
            isCampaignVisible = CurrentUser.IsLeadManagementEnabled;
            if (isCampaignVisible)
            {
                MortgageProfile mp = CurrentPage.GetMortgage(MortgageID);
                isCampaignVisible = LeadCampaign.IsUsedForStatus(mp.CurProfileStatusID);
            }
            rtsFollowup.Tabs[1].Visible = isCampaignVisible;
        }
        private void CheckForTabClicked()
        {
            string s1 = Page.Request.Form["__EVENTTARGET"];
            string s2 = Page.Request.Form["__EVENTARGUMENT"];
            if(!String.IsNullOrEmpty(s1) && s1.EndsWith(rtsFollowup.ID))
            {
                if(!String.IsNullOrEmpty(s2))
                {
                    if (s2.EndsWith("$tabConditions"))
                    {
                        ActiveTabIndex = 0;
                    }
                    else if (s2.EndsWith("$tabCampaigns"))
                    {
                        ActiveTabIndex = 1;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsFirstLoad)
            {
                IsFirstLoad = false;
                ActiveTabIndex = 0;
                InitTabs();
            }
            else
            {
                CheckForTabClicked();
            }
            SetTabsVisibility();
        }

        protected void rtsFollowup_TabClick(object sender, TabStripEventArgs e)
        {
            ActiveTabIndex = rtsFollowup.SelectedIndex;
            if (rtsFollowup.SelectedIndex == 0)
            {
                SetNoteFilterMode(MessageBoardFilter.MODECONDITION);
            }
            else if (rtsFollowup.SelectedIndex == 1)
            {
                SetNoteFilterMode(MessageBoardFilter.MODECAMPAIGN);
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if(isCampaignVisible)
            {
                if(!FollowupCampaign1.HasCampaigns)
                {
                    rtsFollowup.Tabs[1].Visible = false;
                    ActiveTabIndex = 0;
                    InitTabs();
                }
            }
            
            SetTabsColor();
        }

    }
}