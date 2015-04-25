using System;
using System.Web.UI;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class MortgageProfiles : AppControl
    {
        #region constants
        private const string POSTBACKCONTROL = "__EVENTTARGET";
        private const string POSTBACKARGUMENT = "__EVENTARGUMENT";
        private const string APPLISTCONTROL = "ApplicantList1:RadPBMortgages";
        private const string TABSCONTROLNAME = "Tabs:CtrlMortgageProfiles1:";
        private const string TABCLICK = "Tabs:CtrlMortgageProfiles1:TabsMortgageProfiles";
        public const string CURRENTTABID = "currenttabid";
        #endregion

        #region tabs' id
        public const int TABBORROWERID = 1;
        private const int TABPROPERTYID = 2;
        private const int TABMORTGAGEID = 3;
        private const int TABLENDERID = 4;
        #endregion

        #region delegates
//        public event DataChange OnDataChange;
//        public delegate void DataChange();
        public event ConditionChanged OnConditionChanged; 
        public delegate void ConditionChanged(MortgageProfile mp);
        public event DocumentListChanged OnDocumentListChanged;
        public delegate void DocumentListChanged(MortgageProfile mp);
        public event UpdateNeeded OnUpdateNeeded;
        public delegate void UpdateNeeded();

        #endregion

        #region Fields / Properties        
        private MortgageProfile mp;
        private MortgageDataControl currentMortgageControl;
        protected int CurrentTab
        {
            get
            {
                Object o = Session[CURRENTTABID];
                int res = TABBORROWERID;
                if (o!= null)
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
            set
            {
                Session[CURRENTTABID] = value;
            }
            //get
            //{
            //    Object o = ViewState[CURRENTTABID];
            //    int res = TABBORROWERID;
            //    if (o!= null)
            //    {
            //        try
            //        {
            //            res = int.Parse(o.ToString());
            //        }
            //        catch
            //        { 
            //        }
            //    }
            //    return res;
            //}
            //set
            //{
            //    ViewState[CURRENTTABID] = value;
            //}

        }
        protected int MortgageProfileID
        {
            get
            {
                if (Session[Constants.MortgageID] == null)
                    Session[Constants.MortgageID] = 0;
                return Convert.ToInt32(Session[Constants.MortgageID].ToString());
            }
            set
            {
                Session[Constants.MortgageID] = value;
            }
        }
        #endregion

        #region Methods
        private void GetCorresponderLender()
        {
            TabsMortgageProfiles.Tabs[TABLENDERID - 1].Text = CurrentUser.CompanyName;
        }
        private void FillTab()
        {
            MortgageDataControl ctl = GetMortgageDataControl();
            Control pageview = null;
            switch (CurrentTab)
            {
                case TABBORROWERID:
                    pageview = pvBorrower;
                    break;
                case TABPROPERTYID:
                    pageview = pvProperty;
                    break;
                case TABMORTGAGEID:
                    pageview = pvMortgage;
                    break;
                case TABLENDERID:
                    pageview = pvLender;
                    break;
            }
            if (pageview != null)
            {
                #region DEBUG Only
                CurrentPage.WriteToLog(String.Format("Build control({0}) started", TabsMortgageProfiles.Tabs[CurrentTab-1].Value.ToString()));
                #endregion
                ctl.BuildControl(pageview);
                #region DEBUG Only
                CurrentPage.WriteToLog(String.Format("Build control({0}) completed", TabsMortgageProfiles.Tabs[CurrentTab-1].Value.ToString()));
                #endregion
                MortgageProperty mpctl = ctl as MortgageProperty;
                if(mpctl!=null)
                {
                    mpctl.OnUpdateNeeded += UpdateTabs;
                }
                MortgageMortgage mmctl = ctl as MortgageMortgage;
                if (mmctl != null)
                {
                    mmctl.OnUpdateNeeded += UpdateTabs;
                }

                currentMortgageControl = ctl;
            }
            RadMultiPage1.SelectedIndex = CurrentTab - 1;
            TabsMortgageProfiles.SelectedIndex = CurrentTab - 1;
        }
        private void UpdateTabs()
        {
            SetTabColor();
            (CurrentPage as Default).appList.RepopulateMortgageList();
            if(OnUpdateNeeded!=null)
            {
                OnUpdateNeeded();
            }
        }
        private void FillHeader()
        {
            mp = CurrentPage.GetMortgage(MortgageProfileID);
            lblProfileStatus.Text = "Status: " + mp.StatusName;
            lblProduct.Text = "Product : " + (mp.MortgageInfo.ProductId > 0 ? mp.MortgageInfo.ProductName : "Not selected");

            Borrower borrower = mp.YoungestBorrower;
            if (borrower != null)
            {
                lblMortgageName.Text = DataHelpers.WrapString(borrower.FirstName, 25) + " " + DataHelpers.WrapString(borrower.LastName,25);
            }
            else
            {
                lblMortgageName.Text = "N/A";
            }
            if (!String.IsNullOrEmpty(mp.Property.City))
                lblCity.Text = mp.Property.City;
            if (!String.IsNullOrEmpty(mp.Property.City) && !String.IsNullOrEmpty(mp.Property.County))
                lblCity.Text += ", ";
            if (!String.IsNullOrEmpty(mp.Property.County))
                lblCounty.Text = mp.Property.County;
            if (!String.IsNullOrEmpty(mp.Property.County) && mp.Property.StateId > 0)
                lblCounty.Text += ", ";
            else if (!String.IsNullOrEmpty(mp.Property.City) && mp.Property.StateId > 0)
                lblCity.Text += ", ";
            if (mp.Property.StateId > 0)
                lblState.Text = mp.Property.StateName;
        }
        private bool ProcessPostBack()
        {
            string controlName = Page.Request[POSTBACKCONTROL];
            string propertyName = String.Empty;
            if (String.IsNullOrEmpty(controlName))
            {
                return false;
            }

            if (controlName == APPLISTCONTROL)
            {
                return false;
            }
            if (controlName == TABCLICK)
            {
                return true;
            }
            if (controlName == "RadAjaxManager1")
            {
                propertyName = Page.Request[POSTBACKARGUMENT];
            }
            MortgageDataControl ctl = GetMortgageDataControl();
            if (ctl != null)
            {
                ctl.Mp = mp;
                if(!String.IsNullOrEmpty(propertyName))
                {
                    ctl.CheckIfReloadNeeded(propertyName);
                }
                else
                {
                    //Object o = Session[Constants.GRIDPOSTBACK];
                    //if (o != null)
                    //{
                    //    bool isGridPostBack = bool.Parse(o.ToString());
                    //    if (isGridPostBack)
                    //    {
                    //        ctl.CheckGridPostBackFields();
                    //    }
                    //    Session.Remove(Constants.GRIDPOSTBACK);
                    //}
                }
                //if (ctl.CheckMortgageReloadNeeded(propertyName))
                //{
                //    mp = CurrentPage.ReloadMortgage(mp.ID);
                //    ctl.Mp = mp;
                //}
                Control pageview = null;
                switch (CurrentTab)
                {
                    case TABBORROWERID:
                        pageview = pvBorrower;
                        break;
                    case TABPROPERTYID:
                        pageview = pvProperty;
                        break;
                    case TABMORTGAGEID:
                        pageview = pvMortgage;
                        break;
                    case TABLENDERID:
                        pageview = pvLender;
                        break;
                }
                if (pageview != null)
                {
                    pageview.Controls.Clear();
                    ctl.BuildControl(pageview);
                    currentMortgageControl = ctl;
                }
            }
            if (ctl != null)
            {
                if ((OnConditionChanged != null) && (ctl.ConditionFieldFired))
                {
                    OnConditionChanged(mp);
                }

                if ((OnDocumentListChanged != null) && (ctl.DocumentFieldFired))
                {
                    OnDocumentListChanged(mp);
                }
            }
            return true;
        }
        private MortgageDataControl GetMortgageDataControl()
        {
            MortgageDataControl result = null;
            string ctlName = String.Empty;
            switch (CurrentTab)
            {
                case TABBORROWERID:
                    ctlName = Constants.FECTLMORTGAGEBORROWERS;
                    break;
                case TABPROPERTYID:
                    ctlName = Constants.FECTLMORTGAGEPROPERTY;
                    break;
                case TABMORTGAGEID:
                    ctlName = Constants.FECTLMORTGAGEMORTGAGE;
                    break;
                case TABLENDERID:
                    ctlName = Constants.FECTLMORTGAGECORRESPONDENTLENDER;
                    break;
            }
            if (!String.IsNullOrEmpty(ctlName))
            {
                result = LoadControl(Constants.FECONTROLSLOCATION + ctlName) as MortgageDataControl;
            }
            return result;
        }
        private bool CheckAutoPostBackField(string controlName)
        {
            bool res = false;
            if (!controlName.StartsWith(TABSCONTROLNAME))
            {
                return res;
            }
            switch (CurrentTab)
            {
                case TABBORROWERID:
                    res = controlName.IndexOf(":" + MortgageBorrowers.BORROWERWRAPPERNAME + "_") >= 0;
                    break;
                case TABPROPERTYID:
                    if (controlName.IndexOf(":gRepairItems:") > 0)
                        break;
                    res = controlName.IndexOf(":" + MortgageProperty.PROPERTYWRAPPERNAME + "_") >= 0;
                    break;
                case TABMORTGAGEID:
                    res = controlName.IndexOf(":" + MortgageMortgage.MORTGAGEINFOWRAPPERNAME + "_") >= 0;
                    break;
                case TABLENDERID:
                    res = controlName.IndexOf(":" + MortgageCorrespondentLender.MORTGAGELENDERWRAPPERNAME + "_") >= 0;
                    break;
            }
            return res;
        }
        #endregion

        #region Event handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            #region DEBUG ONLY
            CurrentPage.WriteToLog("MortgageProfiles: PageLoad started");
            #endregion
            GetCorresponderLender();
            mp = CurrentPage.GetMortgage(MortgageProfileID);
            SetTabColor();
            //if (AppSettings.DumpRules != 0)
            //{
            //    MortgageDebugInfo mpdebug = new MortgageDebugInfo(CurrentPage.log, mp);
            //    mpdebug.DumpMortgageInfo(AppSettings.DumpRules);
            //}
            FillHeader();
            if (!IsPostBack)
            {
                FillTab();
                TabsMortgageProfiles.SelectedIndex = CurrentTab - 1;
            }
            else
            {
                if (!ProcessPostBack())
                {
                    FillTab();
                }
                else
                {
                    MortgageProperty ctl = currentMortgageControl as MortgageProperty;
                    if (ctl!=null)
                    {
                        ctl.OnUpdateNeeded += UpdateTabs;
                    }
                    MortgageMortgage mctl = currentMortgageControl as MortgageMortgage;
                    if (mctl != null)
                    {
                        mctl.OnUpdateNeeded += UpdateTabs;
                    }
                }
            }
            #region DEBUG ONLY
            CurrentPage.WriteToLog("MortgageProfiles: PageLoad completed");
            #endregion
        }
        private void SetTabColor()
        {
            if (mp.PayoffUpdateNeeded)
            {
                tabProperty.Attributes.Add("style", "color:Red");
            }  
            else
            {
                tabProperty.Attributes.Add("style", "");
            }
            if (mp.InvoiceUpdateNeeded)
            {
                tabMortgage.Attributes.Add("style", "color:Red");
            }
            else
            {
                tabMortgage.Attributes.Add("style", "");
            }
        }
        protected void TabsMortgageProfiles_TabClick(object sender, TabStripEventArgs e)
        {
            switch (e.Tab.Value)
            {
                case "Borrower":
                    CurrentTab = TABBORROWERID;
                    break;
                case "Property":
                    CurrentTab = TABPROPERTYID;
                    break;
                case "Mortgage":
                    CurrentTab = TABMORTGAGEID;
                    break;
                case "CorresponderLender":
                    CurrentTab = TABLENDERID;
                    break;
            }
            FillTab();
        }
        protected void pvPreRender(Object sender, EventArgs args)
        {
            PageView pv = (PageView)sender;
            if ((CurrentTab - 1) == pv.Index)
            {
                if ((currentMortgageControl != null) && (currentMortgageControl.NeedReload))
                {
                    pv.Controls.Clear();
                    currentMortgageControl.BuildControl(pv);
                }
            }

        }
        #endregion
    }
}
