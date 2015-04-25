using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.WebControls;

namespace LoanStar.Common
{
    public class DataTabControl : AppControl
    {
        #region constants
        private const string GETTAB = "GetContentTabs";
        private const string GETPSEUDOTAB = "GetContentPseudoTab";
        private const string GETPSEUDOTABGROUPS = "GetPseudoTabGroups";
        private const string GETCONTENTPRIMITIVE = "GetContentPrimitive";
        private const string GETMORTGAGEPROFILEFIELDS = "GetMortgageProfilefields";
        private const string PSEUDOTABFILTER = "tabid={0}";
        private const string PSEUDOTABWRAPPER = "PseudoTabWrapper.ascx";
        private const string DIVIDERDIV = "<div style='height:{0}px;'>&nbsp;</div>";
        protected const int DIVIDERHEIGHT = 20;
        protected const string TABFILTER = "tablevel2id={0}";
        private const string SPLITTERID = "{0}_Splitter";
        private const int SPLITTERHEIGHT = 95;
        private const int SPLITTERWIDTH = 100;
        private const string SPLITTERSKIN = "Default";
        private const string PANE1ID = "bs_pane_tab";
        private const int PANE1HEIGHT = 25;
        private const string SPLITBARID = "SplitBar_{0}";
        private const string PANE2ID = "bs_pane_pageviews";
        private const string TABSTRIPSKIN = "Outlook";
        private const string TABSTRIPID = "tabs{0}";
        private const string MULTIPAGEID = "newMP{0}";
        private const string MULTIPAGECSS = "tabpageview";
        protected const string TABIDFIELDNAME = "id";
        protected const string HEADERFIELDNAME = "header";
        private const string STYLEATTRIBUTE = "style";
        private const string REDCOLOR = "color:Red";

        private const string TABSCONTROLNAME = "Tabs:CtrlMortgageProfiles1:";
        protected const int BORROWERTABID = 1;
        protected const int PROPERTYTABID = 2;
        protected const int MORTGAGETABID = 3;
        protected const int LENDERTABID = 4;
        #endregion

        #region event
        public delegate void ControlAdded(Control ctl);
        #endregion

        #region fields
        protected int tabId = 0;
        protected bool showTabById = false;
        private DataTable dtTabs;
        private DataTable dtPseudoTabs;
        private DataTable dtGroups;
        private DataTable dtPrimitive;
        private DataTable dtFields;
        protected Control container;
        protected MortgageProfile mp;
        protected BaseObject objectToCompare;
        private short tabIndex=1;
        protected bool generateContent = true;
        private int emptyFields = 0;
        private ArrayList tab2ReqFields;
        private ArrayList tab1ReqFields;
        private int tabLevel2Index = 0;
        private int controlToRenderIndex = 0;
        protected RadTabStrip tabstrip;
        protected RadMultiPage newMultiPage;
        private Hashtable modifiedFields = null;
        #endregion

        #region properties
        public Hashtable ModifiedFields
        {
            get { return modifiedFields; }
        }
        protected int TabLevel2Index
        {
            get { return tabLevel2Index; }
            set { tabLevel2Index = value; }
        }
        protected bool HasEmptyFields
        {
            get { return emptyFields > 0; }
        }
        public short TabIndex
        {
            get { return tabIndex; }
            set { tabIndex = value; }
        }
        protected DataTable DtTabs
        {
            get
            {
                if(dtTabs==null)
                {
                    dtTabs = CurrentPage.GetDictionaryTableByProcedure(GETTAB);
                }
                return dtTabs;
            }
        }
        protected DataTable DtPseudoTabs
        {
            get
            {
                if (dtPseudoTabs == null)
                {
                    dtPseudoTabs = CurrentPage.GetDictionaryTableByProcedure(GETPSEUDOTAB);
                }
                return dtPseudoTabs;
            }
        }
        public DataTable DtGroups
        {
            get
            {
                if (dtGroups == null)
                {
                    dtGroups = CurrentPage.GetDictionaryTableByProcedure(GETPSEUDOTABGROUPS);
                }
                return dtGroups;
            }
        }
        public DataTable DtPrimitive
        {
            get
            {
                if (dtPrimitive == null)
                {
                    dtPrimitive = CurrentPage.GetDictionaryTableByProcedure(GETCONTENTPRIMITIVE);
                }
                return dtPrimitive;
            }
        }
        public DataTable DtFields
        {
            get
            {
                if(dtFields==null)
                {
                    dtFields = CurrentPage.GetDictionaryTableByProcedure(GETMORTGAGEPROFILEFIELDS);
                }
                return dtFields;
            }
        }

        public MortgageProfile Mp
        {
            get
            {
                if(mp==null)
                {
                    mp = CurrentPage.GetMortgage(MortgageProfileId);
                }
                return mp;
            }
        }
        protected int MortgageProfileId
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
        public BaseObject ObjectToCompare
        {
            get { return objectToCompare; }
        }
        #endregion
        
        #region methods
        public virtual void BuildControl(Control _container)
        {
            modifiedFields = null;
            container = _container;
            GetTabToRenderIndex();
            PrepairHtml();
            BuildTabs();
            AddTabRequiredFieldArray("aReq1Fields", tab1ReqFields);
            CurrentPage.ClientScript.RegisterHiddenField("tab2index", controlToRenderIndex.ToString());
            CurrentPage.ClientScript.RegisterHiddenField("tab2Id", tabstrip.ClientID);
        }
        protected virtual void BuildTabs()
        {
        }
        protected virtual void PrepairHtml()
        {
        }
        private Control GetPseudoTabs(int parentTabId, string ctlId)
        {
            emptyFields = 0;
            DataRow[] rows = DtPseudoTabs.Select(String.Format(PSEUDOTABFILTER, parentTabId));
            Control pageView = new PlaceHolder();
            pageView.ID = ctlId;
            for (int i = 0; i < rows.Length;i++ )
            {
                Control ctl = LoadControl(Constants.FECONTROLSLOCATION + PSEUDOTABWRAPPER);
                Panel ph = ctl.FindControl("pn") as Panel;
                if(ph!=null)
                {
                    PseudoTab pst = new PseudoTab(rows[i], ph, this,tabIndex);
                    if (pst.Build())
                    {
                        if(pst.ModifiedFields!=null&&pst.ModifiedFields.Count>0)
                        {
                            AddModifiedField(pst.ModifiedFields);
                        }
                        emptyFields += pst.EmptyFields;
                        pageView.Controls.Add(ctl);
                        pageView.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
                        tabIndex = pst.TabIndex;
                    }
                }
            }
            return pageView;
        }
        private void AddModifiedField(IDictionary tbl)
        {
            if (modifiedFields == null)
            {
                modifiedFields = new Hashtable();
            }
            foreach (Object item in tbl.Keys)
            {
                if (!modifiedFields.ContainsKey(item))
                {
                    modifiedFields.Add(item, tbl[item]);
                }
            }
        }
        private static Control GetTabDivider(int height)
        {
            return new LiteralControl(String.Format(DIVIDERDIV, height));
        }
        protected virtual RadSplitter GetSplitter(string objectName)
        {
            RadSplitter splitter = new RadSplitter();
            splitter.ID = String.Format(SPLITTERID,objectName);
            splitter.Height = Unit.Percentage(SPLITTERHEIGHT);
            splitter.Orientation = RadSplitterOrientation.Horizontal;
            splitter.Width = Unit.Percentage(SPLITTERWIDTH);
            splitter.BorderWidth = Unit.Pixel(0);
            splitter.BorderStyle = BorderStyle.None;
            splitter.BorderSize = 0;
            splitter.Skin = SPLITTERSKIN;
            splitter.LiveResize = false;
            return splitter;
        }
        protected virtual RadPane GetPaneTabs()
        {
            RadPane panetabs = new RadPane();
            panetabs.ID = PANE1ID;
            panetabs.Height = Unit.Pixel(PANE1HEIGHT);
            panetabs.Scrolling = RadSplitterPaneScrolling.None;
            return panetabs;
        }
        protected virtual RadSplitBar GetSplitBar(string objectName)
        {
            RadSplitBar splitBar1 = new RadSplitBar();
            splitBar1.ID = String.Format(SPLITBARID,objectName);
            splitBar1.CollapseMode = RadSplitBarCollapseMode.None;
            splitBar1.EnableResize = false;
            splitBar1.Visible = false;
            return splitBar1;
        }
        protected virtual RadPane GetPaneViews()
        {
            RadPane panepageviews = new RadPane();
            panepageviews.ID = PANE2ID;
            panepageviews.Scrolling = RadSplitterPaneScrolling.Y;
            return panepageviews;
        }
        protected virtual RadTabStrip GetTabStrip(string objectName, string clientJsSelect, string clientJsLoad)
        {
            tabstrip = new RadTabStrip();
            tabstrip.EnableViewState = false;
            tabstrip.Skin = TABSTRIPSKIN;
            tabstrip.ID = String.Format(TABSTRIPID,objectName);
            tabstrip.Orientation = RadTabStripOrientation.HorizontalTopToBottom;
            tabstrip.AutoPostBack = true;
            if(!String.IsNullOrEmpty(clientJsSelect))
            {
                tabstrip.OnClientTabSelected = clientJsSelect;
            }
            if(!String.IsNullOrEmpty(clientJsLoad))
            {
                tabstrip.OnClientLoad = clientJsLoad;
            }
            return tabstrip;
        }
        protected static RadMultiPage GetMultiPage(string objectName)
        {
            RadMultiPage newMultiPage = new RadMultiPage();
            newMultiPage.ID = String.Format(MULTIPAGEID,objectName);
            newMultiPage.CssClass = MULTIPAGECSS;
            newMultiPage.RenderSelectedPageOnly = true;
            return newMultiPage; 
        }
        public virtual void UserControlAdded(Control ctl)
        {
        }
        protected virtual void SetTabColor(Tab tab, bool isRed)
        {
            tab.Attributes.Add(STYLEATTRIBUTE,isRed?REDCOLOR:"");
        }
        public int AddTabLevel2RequiredField(bool isCompleted)
        {
            int res = 0;
            if(tab2ReqFields==null)
            {
                tab2ReqFields=new ArrayList();
            }
            else
            {
                res = tab2ReqFields.Count;
            }
            tab2ReqFields.Add(isCompleted);
            return res;
        }
        protected virtual void AddTabLevel1ReqFields()
        {
            if (tab1ReqFields == null)
            {
                tab1ReqFields = new ArrayList();
            }
            bool res = true;
            if(tab2ReqFields!=null)
            {
                for (int i = 0; i < tab2ReqFields.Count; i++)
                {
                    if (!(bool)tab2ReqFields[i])
                    {
                        res = false;
                        break;
                    }
                }
            }
            tab1ReqFields.Add(res);
        }
        private void GetTabToRenderIndex()
        {
            controlToRenderIndex = 0;
            int currentTab = 0;
            try
            {
                currentTab = int.Parse(Page.Request["currenttab"]);
            }
            catch { }
            
            string tabName = String.Empty;
            switch(tabId)
            {
                case BORROWERTABID:
                    tabName = "tabsBorrower";
                    break;
                case PROPERTYTABID:
                    tabName = "tabsProperty";
                    break;
                case MORTGAGETABID:
                    tabName = "tabsMortgage";
                    break;
                case LENDERTABID:
                    tabName = "tabsLender";
                    break;
            }
            string controlName = Page.Request[Constants.REQUESTEVENTTARGET] ?? "";
            if ((controlName == "Tabs:CtrlMortgageProfiles1:TabsMortgageProfiles") || (controlName == "ApplicantList1") || (controlName == "ApplicantList1:RadPBMortgages"))
            {
                currentTab = 0;
            }
            if (controlName == (TABSCONTROLNAME+tabName))
            {
                string str = Page.Request[Constants.REQUESTEVENTARGUMENT];
                controlToRenderIndex = int.Parse(str.Remove(0, (str.Length - 1)));
            }
            else
            {
                if (tabId == BORROWERTABID)
                {
                    if (controlName.IndexOf(":gAKANames:") > 0)
                    {
                        controlToRenderIndex = CurrentPage.CurrentTopSecondTabIndex;
                    }
                    else if (showTabById)
                    {
                        controlToRenderIndex = CurrentPage.CurrentTopSecondTabIndex;
                    }
                    else
                    {
                        controlToRenderIndex = currentTab;
                    }
                }
                if(tabId==PROPERTYTABID)
                {
                    if(controlName.IndexOf(":gPayoffs:")>0)
                    {
                        controlToRenderIndex = 1;
                    }
                    else if (controlName.IndexOf(":gSignatureLines:")>0)
                    {
                        controlToRenderIndex = CurrentPage.CurrentTopSecondTabIndex;
                    }
                    else
                    {
                        controlToRenderIndex = currentTab;
                    }
                }
                else if(tabId==MORTGAGETABID)
                {
                    if (controlName.IndexOf(":gInvoices:") > 0)
                    {
                        controlToRenderIndex = 0;
                    }
                }
            }
        }
        protected void GenerateContent(Tab tab,int parentTabId, string ctlId, PageView currentPageView)
        {
            Control dvd = GetTabDivider(DIVIDERHEIGHT);
            Control data = GetPseudoTabs(parentTabId, ctlId);
            string controlName = Page.Request[Constants.REQUESTEVENTTARGET] ?? "";
            AddTabLevel1ReqFields();
            if(IsVisible(currentPageView.Index, controlName))
            {
                currentPageView.Controls.Add(dvd);
                currentPageView.Controls.Add(data);
                AddTabRequiredFieldArray("aReq2Fields", tab2ReqFields);
            }
            SetTabColor(tab, HasEmptyFields);
            tab2ReqFields = null;
        }
        private bool IsVisible(int pageindex, string controlName)
        {
            if (pageindex == controlToRenderIndex || (String.IsNullOrEmpty(controlName) && pageindex == CurrentPage.CurrentTopSecondTabIndex) || 
                (controlName.Contains("RadAjaxManager1") && pageindex == CurrentPage.CurrentTopSecondTabIndex) || 
                (pageindex == CurrentPage.CurrentTopSecondTabIndex && controlName.Contains("Tabs:RadTabStrip1")) ||
                (pageindex == CurrentPage.CurrentTopSecondTabIndex && showTabById)
                )
                return true;
            else
                return false;
        }
        private void AddTabRequiredFieldArray(string name, ArrayList reqFields)
        {
            CurrentPage.ClientScript.RegisterHiddenField(name, GetRequiredFieldsString(reqFields));
        }
        private static string GetRequiredFieldsString(ArrayList fields)
        {
            StringBuilder sb = new StringBuilder();
            if (fields != null)
            {
                for (int i = 0; i < fields.Count; i++)
                {
                    sb.Append((bool)fields[i] ? "1" : "0");
                }
            }
            return sb.ToString();
        }
//        protected virtual void Page_PreRender(object sender, EventArgs e)
        //protected void CheckData()
        //{
        //    if(modifiedFields != null && modifiedFields.Count > 0)
        //    {
        //        foreach (Object key in modifiedFields.Keys )
        //        {
        //            string dbg = key.ToString();
        //            ContentPrimitive cp = (ContentPrimitive)modifiedFields[key];
        //            dbg = "";
        //        }
                
        //    }
        //}

        #endregion
    }
}
