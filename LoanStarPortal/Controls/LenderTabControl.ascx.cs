using System;
using System.Data;
using System.Web.UI;
using LoanStar.Common;
using Telerik.WebControls;


namespace LoanStarPortal.Controls
{

    public partial class LenderTabControl : DataTabControl
    {
        #region constants
        private const string LENDEROBJECTNAME = "Lender";
        private const string PAGEVIEWID = "lenderpv_{0}";
        private const string PSEUDOTABID = "lenderps_{0}";
        #endregion

        private PageView currentPv;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public override void BuildControl(Control _container)
        {
            tabId = LENDERTABID;
            base.BuildControl(_container);
        }
        protected override void PrepairHtml()
        {
            RadSplitter splitter = GetSplitter(LENDEROBJECTNAME);
            RadPane panetabs = GetPaneTabs();
            RadSplitBar splitBar1 = GetSplitBar(LENDEROBJECTNAME);
            RadPane panepageviews = GetPaneViews();
            tabstrip = GetTabStrip(LENDEROBJECTNAME, String.Empty, String.Empty);
            tabstrip.TabClick += OnTabClick;
            newMultiPage = GetMultiPage(LENDEROBJECTNAME);
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
                int tid = int.Parse(rows[i][TABIDFIELDNAME].ToString());
                Tab tab = new Tab(rows[i][HEADERFIELDNAME].ToString());
                tabstrip.Tabs.Add(tab);
                PageView generalPage = new PageView();
                currentPv = generalPage;
                generalPage.ID = String.Format(PAGEVIEWID, newMultiPage.PageViews.Count);
                newMultiPage.PageViews.Add(generalPage);
                objectToCompare = null;
                GenerateContent(tab, tid, String.Format(PSEUDOTABID, tid), generalPage);
            }
            tabstrip.SelectedIndex = CurrentPage.CurrentTopSecondTabIndex;
            newMultiPage.SelectedIndex = CurrentPage.CurrentTopSecondTabIndex;
        }
        protected void OnTabClick(object sender, TabStripEventArgs e)
        {
            CurrentPage.CurrentTopSecondTabIndex = e.Tab.Index;
        }
    }
}