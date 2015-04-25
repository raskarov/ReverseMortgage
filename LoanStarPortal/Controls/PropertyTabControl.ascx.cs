using System;
using System.Data;
using System.Web.UI;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class PropertyTabControl : DataTabControl
    {
        #region constants
        private const int LIENSTABID = 4;
        private const string PROPERTYOBJECTNAME = "Property";
        private const string PAGEVIEWID = "propertypv_{0}";
        private const string PSEUDOTABID = "propertyps_{0}";
        private const string STYLEATTRIBUTE = "style";
        private const string REDCOLORSTYLE = "color:red";
        private const string CLIENTTABSELECTEDJS = "TabSelected";
        #endregion

        private int currentTabId;
        private bool payoffsRedFormatting = false;

        public event UpdateNeeded OnUpdateNeeded;
        public delegate void UpdateNeeded();

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public override void BuildControl(Control _container)
        {
            tabId = PROPERTYTABID;
            Object o = Session[Constants.GOTOPAYOFFS];
            if (o != null)
            {
                Session.Remove(Constants.GOTOPAYOFFS);
                CurrentPage.CurrentTopSecondTabIndex = 1;
                showTabById = true;
            }
            base.BuildControl(_container);
        }
        protected override void PrepairHtml()
        {
            RadSplitter splitter = GetSplitter(PROPERTYOBJECTNAME);
            RadPane panetabs = GetPaneTabs();
            RadSplitBar splitBar1 = GetSplitBar(PROPERTYOBJECTNAME);
            RadPane panepageviews = GetPaneViews();
            tabstrip = GetTabStrip(PROPERTYOBJECTNAME, CLIENTTABSELECTEDJS, String.Empty);
            tabstrip.TabClick += OnTabClick;
            newMultiPage = GetMultiPage(PROPERTYOBJECTNAME);
            tabstrip.MultiPageID = newMultiPage.ID;
            panepageviews.Controls.Add(newMultiPage);

            panetabs.Controls.Add(tabstrip);
            splitter.Items.Add(panetabs);
            splitter.Items.Add(splitBar1);
            splitter.Items.Add(panepageviews);

            container.Controls.Add(splitter);
        }
        protected override void BuildTabs()
        {
            DataRow[] rows = DtTabs.Select(String.Format(TABFILTER, tabId));
            for (int i = 0; i < rows.Length; i++)
            {
                currentTabId = int.Parse(rows[i][TABIDFIELDNAME].ToString());
                Tab tab = new Tab(rows[i][HEADERFIELDNAME].ToString());
                tabstrip.Tabs.Add(tab);
                PageView generalPage = new PageView();
                generalPage.ID = String.Format(PAGEVIEWID, newMultiPage.PageViews.Count);
                newMultiPage.PageViews.Add(generalPage);
                objectToCompare = null;
                GenerateContent(tab, currentTabId, String.Format(PSEUDOTABID, currentTabId), generalPage);
            }
            tabstrip.SelectedIndex = CurrentPage.CurrentTopSecondTabIndex;
            newMultiPage.SelectedIndex = CurrentPage.CurrentTopSecondTabIndex;
        }
        public override void UserControlAdded(Control ctl)
        {
            if (ctl is Payoffs)
            {
                ((Payoffs)ctl).OnUpdateNeeded += UpdateTabs;
            }
            else if (ctl is TrusteeSignatureLines)
            {
                ((TrusteeSignatureLines) ctl).Visible = mp.MortgageInfo.CloseInATrust != null &&
                                                        (bool) mp.MortgageInfo.CloseInATrust;
            }
        }
        protected override void SetTabColor(Tab tab, bool _isRed)
        {
            bool isRed = _isRed;
            if (currentTabId == LIENSTABID)
            {
                payoffsRedFormatting = isRed;
                isRed = isRed || Mp.PayoffUpdateNeeded;
            }
            base.SetTabColor(tab, isRed);
        }
        private void UpdateTabs()
        {
            int curTabId = currentTabId;
            currentTabId = LIENSTABID;
            SetTabColor(tabstrip.Tabs[1],payoffsRedFormatting);
            currentTabId = curTabId;
            if (OnUpdateNeeded != null)
            {
                OnUpdateNeeded();
            }
        }
        protected void OnTabClick(object sender, TabStripEventArgs e)
        {
            CurrentPage.CurrentTopSecondTabIndex = e.Tab.Index;
        }
    }
}