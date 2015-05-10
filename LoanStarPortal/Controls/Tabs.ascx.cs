using System;
using System.Web.UI;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class Tabs : AppControl
    {

        #region tabs' id
        public const int TABLOANINFOID = 1;
        //private const int TABCALENDARID = 2;
        //private const int TABCALCULATORID = 3;
        //private const int TABCHECKLISTSID = 4;
        //private const int TABCONDITIONSID = 5;
        //private const int TABCALENDARID = 2;
        private const int TABCALCULATORID = TABLOANINFOID + 1;
        private const int TABCHECKLISTSID = TABLOANINFOID + 2;
        private const int TABCONDITIONSID = TABLOANINFOID + 3;

        #endregion

        public event UpdateNeeded OnUpdateNeeded;
        public delegate void UpdateNeeded();

        #region Fields/Properties

        private Control currentControl;
        private MortgageProfile mp;

        public int CurrentTab
        {
            get
            {
                Object o = Session[Constants.CURRENTBOTTOMTABID];
                int res = TABLOANINFOID;
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set
            {
                Session[Constants.CURRENTBOTTOMTABID] = value;
            }
        }
        protected int MortgageProfileID
        {
            get
            {
                if (Session[Constants.MortgageID] == null)
                    Session[Constants.MortgageID] = 0;
                return Convert.ToInt32(Session[Constants.MortgageID].ToString());
            }
        }

        #endregion

        #region Methods
        public void LoadControls()
        {
            mp = CurrentPage.GetMortgage(MortgageProfileID);
            Object o;
            if (CurrentPage.IsAjaxPostBackRaisen)
            {
                bool isManagedLeadCreated = false;
                bool isNewLoanCreated = false;
                o = Session[Constants.NEWMORTGAGECREATED];
                if (o != null)
                {
                    isNewLoanCreated = true;
                    isManagedLeadCreated = mp.CurProfileStatusID == MortgageProfile.MANAGEDLEADSTATUSID;
                }
                if (isNewLoanCreated)
                {
                    if (isManagedLeadCreated && !((Default)CurrentPage).appList.IsManagedLeadSelected)
                    {
                        ((Default)CurrentPage).appList.SetToManagedLead();
                    }
                    else if (((Default)CurrentPage).appList.IsManagedLeadSelected)
                    {
                        ((Default)CurrentPage).appList.SetToActiveLoans();
                    }
                }
                o = Session[Constants.GOTOCALCULATOR];
                if (o != null)
                {
                    CurrentTab = TABCALCULATORID;
                    Session.Remove(Constants.GOTOCALCULATOR);
                    Session.Remove(Constants.NEWMORTGAGECREATED);
                }
                else
                {
                    o = Session[Constants.NEWMORTGAGECREATED];
                    if (o != null)
                    {
                        if (!mp.HasRequiredFields)
                        {
                            Session.Remove(Constants.NEWMORTGAGECREATED);
                            CurrentTab = TABCONDITIONSID;
                        }
                    }
                }
                if (CurrentTab != TABCONDITIONSID)
                    CurrentPage.MessageBoardShowOnlyNotes = false;
            }
            o = Session[Constants.GOTOPAYOFFS];
            if (o != null)
            {
                CurrentTab = TABLOANINFOID;
            }
            switch (CurrentTab)
            {
                case TABLOANINFOID:
                    MortgageTab mt = LoadControl(Constants.FECONTROLSLOCATION + "MortgageTab.ascx") as MortgageTab;
                    if (mt != null)
                    {
                        mt.ID = "CtrlMortgageProfiles1";
                        ViewState["LoadedControl"] = Constants.FECONTROLSLOCATION + "MortgageTab.ascx";
                        currentControl = mt;
                        mt.OnActionTriggered += ActionTriggered;
                        mt.OnUpdateNeeded += UpdateTabs;
                        UpdateDocPane();
                    }
                    break;
                case TABCALCULATORID:
                    Calculator calc = LoadControl(Constants.FECONTROLSLOCATION + Constants.FECTLCALCULATOR) as Calculator;
                    if (calc != null) calc.ID = "CtrlCalculator1";
                    ViewState["LoadedControl"] = Constants.FECONTROLSLOCATION + Constants.FECTLCALCULATOR;
                    currentControl = calc;
                    break;
                case TABCHECKLISTSID:
                    CheckList chk = LoadControl(Constants.FECONTROLSLOCATION + Constants.FECTLCHECKLIST) as CheckList;
                    if (chk != null) chk.ID = "CtrlCheckList1";
                    ViewState["LoadedControl"] = Constants.FECONTROLSLOCATION + Constants.FECTLCHECKLIST;
                    currentControl = chk;
                    break;
                case TABCONDITIONSID:
                    Followup followup = LoadControl(Constants.FECONTROLSLOCATION + Constants.FECTLFOLLOWUP) as Followup;
                    if (followup != null) followup.ID = "CtrlTasks1";
                    if (followup != null) followup.OnUpdateNeeded += UpdateTabs;
                    ViewState["LoadedControl"] = Constants.FECONTROLSLOCATION + Constants.FECTLFOLLOWUP;
                    currentControl = followup;
                    break;
            }
            SetAddNoteScript();
            FillTab();
        }
        private void UpdateDocPane()
        {
            if (mp.LastPostBackField == "MortgageInfo.ProductId" || mp.LastPostBackField == "MortgageInfo.LenderAffiliateID")
            {
                TabPackage docPackage = ((Default)Page).DocumentPackage;
                if (docPackage != null)
                {
                    docPackage.RecalculateDocList(mp);
                    CurrentPage.CenterRightPanelUpdateNeeded = true;
                }
            }
        }
        private void SetAddNoteScript()
        {
            Notes notes = ((Default)Page).notes;
            if (notes != null)
            {
                string script = String.Format("<script language=\"javascript\" type=\"text/javascript\">HideShowNoteDiv('{0}',{1});</script>", notes.DivQuickNoteId, CurrentTab == TABCHECKLISTSID ? 1 : 0);
                CurrentPage.ClientScript.RegisterStartupScript(GetType(), "InitAddNote", script);
            }
        }
        public void ActionTriggered(MortgageProfile mp_, int actionMask)
        {
            if ((actionMask & RuleTree.CONDITIONBIT) != 0)
            {
                Conditions cond = ((Default)Page).cond;
                if (cond != null)
                {
                    cond.RecalculateCondition(mp);
                }
                Notes notes = ((Default)Page).notes;
                if (notes != null)
                {
                    notes.BindData();
                }
            }
            if ((actionMask & RuleTree.DOCUMENTBIT) != 0)
            {
                TabPackage docPackage = ((Default)Page).DocumentPackage;
                if (docPackage != null)
                {
                    docPackage.RecalculateDocList(mp_);
                }
            }
            CurrentPage.CenterRightPanelUpdateNeeded = true;
        }

        public void ActivateTab(int id)
        {
            RadTabStrip1.Tabs[id].Selected = true;
            RadMultiPage1.PageViews[id].Selected = true;
            CurrentPage.CenterLeftPanelUpdateNeeded = true;
        }

        public void MortgageDataChanged()
        {
            ApplicantList appl = ((Default)Page).appList;
            if (appl != null)
            {
                CurrentPage.CenterLeftPanelUpdateNeeded = true;
                appl.RepopulateMortgageList();
            }

            Conditions cond = ((Default)Page).cond;
            if (cond != null)
            {
                cond.Repopulate();
            }
        }
        public void MortgageConditionChanged(MortgageProfile mp_)
        {
            Conditions cond = ((Default)Page).cond;
            if (cond != null)
            {
                CurrentPage.CenterLeftPanelUpdateNeeded = true;
                cond.RecalculateCondition(mp_);
            }
        }
        public void MortgageDocumentListChanged(MortgageProfile mp_)
        {
            TabPackage docPackage = ((Default)Page).DocumentPackage;
            if (docPackage != null)
            {
                docPackage.RecalculateDocList(mp_);
            }
        }
        protected void FillTab()
        {
            if (currentControl == null)
            {
                LoadControls();
            }
            switch (CurrentTab)
            {
                case TABLOANINFOID:
                    RadMultiPage1.PageViews[TABLOANINFOID - 1].Controls.Add(currentControl);
                    break;
                case TABCALCULATORID:
                    RadMultiPage1.PageViews[TABCALCULATORID - 1].Controls.Add(currentControl);
                    break;
                case TABCHECKLISTSID:
                    RadMultiPage1.PageViews[TABCHECKLISTSID - 1].Controls.Add(currentControl);
                    break;
                case TABCONDITIONSID:
                    RadMultiPage1.PageViews[TABCONDITIONSID - 1].Controls.Add(currentControl);
                    break;
            }
        }
        protected void AddControl(string Ctrl, int pageid)
        {
            UserControl uctl = (UserControl)LoadControl(Ctrl);
            if (uctl != null)
            {
                Control existControl = RadMultiPage1.PageViews[pageid].FindControl(Ctrl);
                if (existControl != null)
                {
                    RadMultiPage1.PageViews[pageid].Controls.Remove(existControl);
                }
                else
                    RadMultiPage1.PageViews[pageid].Controls.Add(uctl);
            }
        }
        public void SetTabColor()
        {
            mp = CurrentPage.GetMortgage(MortgageProfileID);
            if (mp.DayToWorkUpdateNeeded || mp.CampaignUpdateNeeded)
            {
                CurrentPage.CenterLeftPanelUpdateNeeded = true;
                tabConditions.Attributes.Add("style", "color:Red");
                tabConditions.SelectedCssClass = "red selected";
            }
            else
            {
                tabConditions.SelectedCssClass = "selected";
                tabConditions.Attributes.Add("style", "");
            }
            if (mp.HasRequiredFields || mp.PayoffUpdateNeeded)
            {
                CurrentPage.CenterLeftPanelUpdateNeeded = true;
                tabLoanInfo.Attributes.Add("style", "color:Red");
                tabLoanInfo.SelectedCssClass = "red selected";
            }
            else
            {
                tabLoanInfo.SelectedCssClass = "selected";
                tabLoanInfo.Attributes.Add("style", "");
            }
            if (mp.CalculatorUpdateNeeded && CurrentTab != TABCALCULATORID)
            {
                tabCalculator.Attributes.Add("style", "color:Red");
                tabCalculator.SelectedCssClass = "red selected";
            }
            else
            {
                tabCalculator.Attributes.Add("style", "");
                tabCalculator.SelectedCssClass = "selected";
            }
        }
        private void UpdateTabs()
        {
            SetTabColor();
            ((Default)CurrentPage).appList.RepopulateMortgageList();
            if (OnUpdateNeeded != null)
            {
                OnUpdateNeeded();
            }
        }
        #endregion

        #region Event hadlers
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ActivateTab(CurrentTab - 1);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            currentControl = null;
            if (Request.Form.Count > 0)
            {
                Control ctrl = Page.FindControl(Request.Form["__EVENTARGUMENT"]);
                if (ctrl != null && ctrl.GetType() == typeof(Tab))
                {
                    Tab currentTab = (Tab)ctrl;
                    switch (currentTab.Value)
                    {
                        case "Info":
                            CurrentTab = TABLOANINFOID;
                            break;
                        case "Calculator":
                            CurrentTab = TABCALCULATORID;
                            break;
                        case "Checklists":
                            CurrentTab = TABCHECKLISTSID;
                            break;
                        case "Conditions":
                            CurrentTab = TABCONDITIONSID;
                            break;
                    }
                    LoadControls();
                }
                else
                {
                    FillTab();
                }
            }
            else if (ViewState["LoadedControl"] == null)
            {
                LoadControls();
            }

            ActivateTab(CurrentTab - 1);
            SetTabColor();
        }
        #endregion
    }
}