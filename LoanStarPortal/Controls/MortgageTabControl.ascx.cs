using System;
using System.Data;
using System.Web.UI;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class MortgageTabControl : DataTabControl
    {
        #region constants
        private const string MORTGAGEOBJECTNAME = "Mortgage";
        private const string PAGEVIEWID = "propertypv_{0}";
        private const string PSEUDOTABID = "propertyps_{0}";
        private const int GENERALTAB = 6;
        private const string STYLEATTRIBUTE = "style";
        private const string REDCOLORSTYLE = "color:red";
        private const string CLIENTTABSELECTEDJS = "TabSelected";
        #endregion

        private int currentTabId;
        private bool invoiceRedFormatting = false;

        public event UpdateNeeded OnUpdateNeeded;
        public delegate void UpdateNeeded();


        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public override void BuildControl(Control _container)
        {
            tabId = MORTGAGETABID;
            base.BuildControl(_container);
        }
        protected override void PrepairHtml()
        {
            RadSplitter splitter = GetSplitter(MORTGAGEOBJECTNAME);
            RadPane panetabs = GetPaneTabs();
            RadSplitBar splitBar1 = GetSplitBar(MORTGAGEOBJECTNAME);
            RadPane panepageviews = GetPaneViews();
            tabstrip = GetTabStrip(MORTGAGEOBJECTNAME, CLIENTTABSELECTEDJS, String.Empty);
            newMultiPage = GetMultiPage(MORTGAGEOBJECTNAME);
            tabstrip.MultiPageID = newMultiPage.ID;
            tabstrip.TabClick += OnTabClick;
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
            if (ctl is Invoices)
            {
                ((Invoices)ctl).OnUpdateNeeded += UpdateTabs;
            }
        }
        protected override void SetTabColor(Tab tab, bool _isRed)
        {
            bool isRed = _isRed;
            if (currentTabId == GENERALTAB)
            {
                invoiceRedFormatting = isRed;
//                isRed = isRed || Mp.InvoiceUpdateNeeded;
            }
            base.SetTabColor(tab, isRed);
        }
        private void UpdateTabs()
        {
            SetTabColor(tabstrip.Tabs[0], invoiceRedFormatting);
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