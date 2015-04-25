using System;
using System.Web.UI;
using System.Collections;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class MortgageTab : AppControl
    {
        #region constants
        private const string POSTBACKCONTROL = "__EVENTTARGET";
        private const string POSTBACKARGUMENT = "__EVENTARGUMENT";
        private const string APPLISTCONTROL = "ApplicantList1:RadPBMortgages";
//        private const string TABSCONTROLNAME = "Tabs:CtrlMortgageProfiles1:";
        private const string TABCLICK = "Tabs:CtrlMortgageProfiles1:TabsMortgageProfiles";
        private const string STYLEATTRIBUTE = "style";
        private const string REDCOLOR = "color:Red";
        #endregion

        #region tabs' id
        public const int TABBORROWERID = 1;
        public const int TABPROPERTYID = 2;
        private const int TABMORTGAGEID = 3;
        private const int TABLENDERID = 4;
        #endregion

        #region delegates
        public event UpdateNeeded OnUpdateNeeded;
        public delegate void UpdateNeeded();
        public event ActionTriggered OnActionTriggered;
        public delegate void ActionTriggered(MortgageProfile mp, int actionMask);

        #endregion

        #region Fields / Properties 
        private Hashtable modifiedFields = null;
        private string lastPostBackField;
        private MortgageProfile mp;
//        private DataTabControl currentMortgageControl;
        protected int CurrentTab
        {
            get
            {
                Object o = Session[Constants.CURRENTTOPFIRSTTABID];
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
                Session[Constants.CURRENTTOPFIRSTTABID] = value;
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
            DataTabControl ctl = GetMortgageDataControl();
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
                mp.ResetCompletedTabs();
                ctl.BuildControl(pageview);
                modifiedFields = ctl.ModifiedFields;
                SetRedFormatting();
                PropertyTabControl mpctl = ctl as PropertyTabControl;
                if (mpctl != null)
                {
                    mpctl.OnUpdateNeeded += UpdateTabs;
                }
                else
                {
                    MortgageTabControl mmctl = ctl as MortgageTabControl;
                    if (mmctl != null)
                    {
                        mmctl.OnUpdateNeeded += UpdateTabs;
                    }
                }
                if (!String.IsNullOrEmpty(lastPostBackField))
                {
                    if(mp.RuleObjectFields.ContainsKey(lastPostBackField))
                    {
                        if(OnActionTriggered!=null)
                        {
                            int objectMask = (int)mp.RuleObjectFields[lastPostBackField]; 
                            OnActionTriggered(mp,objectMask);
                        }
                    }
                }
            }
            RadMultiPage1.SelectedIndex = CurrentTab - 1;
            TabsMortgageProfiles.SelectedIndex = CurrentTab - 1;
            CurrentPage.ClientScript.RegisterHiddenField("tabId", TabsMortgageProfiles.ClientID);
            CurrentPage.ClientScript.RegisterHiddenField("tab1index", CurrentTab.ToString());
        }
        private void SetRedFormatting()
        {
            char[] arr = new char[mp.CompletedTabs.Count];
            for (int i = 0; i < arr.Length;i++ )
            {
                arr[i] = '1';
            }
            foreach (DictionaryEntry item in mp.CompletedTabs)
            {
                int tabId = int.Parse(item.Key.ToString());
                bool isCompleted = true;
                Hashtable ht = item.Value as Hashtable;
                if (ht != null)
                {
                    foreach (DictionaryEntry childTab in ht)
                    {
                        bool res = (bool)childTab.Value;
                        if (!res)
                        {
                            isCompleted = false;
                            break;
                        }
                    }
                }
                if(!isCompleted)
                {
                    arr[tabId - 1] = '0';
                }
                SetTabRedFormatting(tabId, isCompleted);
            }
            string s = "";
            for(int i=0;i<arr.Length;i++)
            {
                s += arr[i];
            }
            CurrentPage.ClientScript.RegisterHiddenField("mortgreq", s);
        }
        private void SetTabRedFormatting(int tabId,bool isCompleted)
        {
            switch (tabId)
            {
                case TABBORROWERID:
                    tabBorrower.Attributes.Add(STYLEATTRIBUTE, isCompleted ? "" : REDCOLOR);
                    break;
                case TABPROPERTYID:
                    tabProperty.Attributes.Add(STYLEATTRIBUTE, (isCompleted && (!mp.PayoffUpdateNeeded)) ? "" : REDCOLOR);
                    break;
                case TABMORTGAGEID:
//                    tabMortgage.Attributes.Add(STYLEATTRIBUTE, (isCompleted && (!mp.InvoiceUpdateNeeded)) ? "" : REDCOLOR);
                    tabMortgage.Attributes.Add(STYLEATTRIBUTE, isCompleted ? "" : REDCOLOR);
                    break;
                case TABLENDERID:
                    tabLender.Attributes.Add(STYLEATTRIBUTE, isCompleted ? "" : REDCOLOR);
                    break;
            }
        }

        private void UpdateTabs()
        {
 //           SetTabColor();
            SetRedFormatting();
            ((Default) CurrentPage).appList.RepopulateMortgageList();
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
            if (!String.IsNullOrEmpty(mp.Property.City) && mp.Property.StateId > 0)
                lblCity.Text += ", ";
            if (mp.Property.StateId > 0)
                lblState.Text = mp.Property.StateName;

            //if (!String.IsNullOrEmpty(mp.Property.City) && !String.IsNullOrEmpty(mp.Property.County))
            //    lblCity.Text += ", ";
            //if (!String.IsNullOrEmpty(mp.Property.County))
            //    lblCounty.Text = mp.Property.County;
            //if (!String.IsNullOrEmpty(mp.Property.County) && mp.Property.StateId > 0)
            //    lblCounty.Text += ", ";
            //else if (!String.IsNullOrEmpty(mp.Property.City) && mp.Property.StateId > 0)
//                lblCity.Text += ", ";
//            if (mp.Property.StateId > 0)
  //              lblState.Text = mp.Property.StateName;
        }
        private bool ProcessPostBack()
        {
            string controlName = Page.Request[POSTBACKCONTROL];
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
                lastPostBackField = Page.Request[POSTBACKARGUMENT];
            }

            return false;
        }
        private DataTabControl GetMortgageDataControl()
        {
            DataTabControl result = null;
            string ctlName = String.Empty;
            switch (CurrentTab)
            {
                case TABBORROWERID:
                    ctlName = Constants.FECTLMORTGAGEBORROWERSTAB;
                    break;
                case TABPROPERTYID:
                    ctlName = Constants.FECTLMORTGAGEPROPERTYTAB;
                    break;
                case TABMORTGAGEID:
                    ctlName = Constants.FECTLMORTGAGEMORTGAGETAB;
                    break;
                case TABLENDERID:
                    ctlName = Constants.FECTLMORTGAGELENDERTAB;
                    break;
            }
            if (!String.IsNullOrEmpty(ctlName))
            {
                result = LoadControl(Constants.FECONTROLSLOCATION + ctlName) as DataTabControl;
            }
            return result;
        }
        #endregion

        #region Event handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            GetCorresponderLender();
            mp = CurrentPage.GetMortgage(MortgageProfileID);
            mp.BuildRuleEvaluationTree(CurrentPage.GetRuleTreePublic());
//            SetTabColor();
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
                    CurrentPage.CurrentTopSecondTabIndex = 0;
                }
            }
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
            //if (mp.InvoiceUpdateNeeded)
            //{
            //    tabMortgage.Attributes.Add("style", "color:Red");
            //}
            //else
            //{
            //    tabMortgage.Attributes.Add("style", "");
            //}
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
        protected void Page_PreRender(Object sender, EventArgs args)
        {
        }

        protected void pvPreRender(Object sender, EventArgs args)
        {
            PageView pv = (PageView)sender;
            if ((CurrentTab - 1) == pv.Index)
            {
                if (modifiedFields != null && modifiedFields.Count > 0)
                {
                    foreach (Object key in modifiedFields.Keys)
                    {
                        ContentPrimitive cp = (ContentPrimitive)modifiedFields[key];
                        Control ctl = FindPrimitiveControl(pv, cp.ControlId);
                        if(ctl!=null)
                        {
                            cp.OverrideValue(ctl);
                        }
                    }
                }
            }
            //if ((CurrentTab - 1) == pv.Index)
            //{
            //    if ((currentMortgageControl != null) && (currentMortgageControl.NeedReload))
            //    {
            //        pv.Controls.Clear();
            //        currentMortgageControl.BuildControl(pv);
            //    }
            //}

        }
        private Control FindPrimitiveControl(Control parent, string controlId)
        {
            Control res = null;
            if (!parent.HasControls()) return res;
            for(int i=0; i<parent.Controls.Count;i++)
            {
                string ctlId = parent.Controls[i].ClientID;
                if(ctlId.EndsWith(controlId))
                {
                    return parent.Controls[i];
                }
                res = FindPrimitiveControl(parent.Controls[i], controlId);
                if (res != null) return res;
            }
            return res;
        }

        #endregion
    }
}