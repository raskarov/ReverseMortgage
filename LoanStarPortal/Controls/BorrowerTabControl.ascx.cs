using System;
using System.Data;
using System.Web.UI;
using LoanStar.Common;
using Telerik.WebControls;


namespace LoanStarPortal.Controls
{
    public partial class BorrowerTabControl : DataTabControl
    {
        #region constants
        private const int GENERALPSEUDOTABID = 1;
        private const int BORROWERPSEUDOTABID = 2;
        private const string CLIENTTABSELECTEDJS = "TabSelected";
        private const string CLIENTLOADJS = "OnTabLoad";
        private const string PAGEVIEWID = "borrowerpv_{0}";
        private const string BORROWERID = "Borrower_{0}";
        private const string BORROWEROBJECTNAME = "Borrower";

        #endregion



        #region methods

        public override void BuildControl(Control _container)
        {
            tabId = BORROWERTABID;
            if (CurrentPage.IsAjaxPostBackRaisen)
            {
                Object o = Session[Constants.NEWMORTGAGECREATED];
                if (o != null)
                {
                    Session.Remove(Constants.NEWMORTGAGECREATED);
                    CurrentPage.CurrentTopSecondTabIndex = 1;
                    showTabById = true;
                }
            }
            if(!showTabById)
            {
                Object o = Session["StatusChanged"];
                if(o!=null)
                {
                    Session.Remove("StatusCahnged");
                    try
                    {
                        showTabById = bool.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
            }
            base.BuildControl(_container);
        }
        protected override void BuildTabs()
        {
            DataRow[] rows = DtTabs.Select(String.Format(TABFILTER, BORROWERTABID));
            for(int i=0; i < rows.Length; i++)
            {
                int tid = int.Parse(rows[i][TABIDFIELDNAME].ToString());
                if (tid == GENERALPSEUDOTABID)
                {
                    Tab tab = new Tab();
                    tab.Text = rows[i][HEADERFIELDNAME].ToString();
                    tabstrip.Tabs.Add(tab);
                    PageView generalPage = new PageView();
                    generalPage.ID = String.Format(PAGEVIEWID, newMultiPage.PageViews.Count);
                    newMultiPage.PageViews.Add(generalPage);
                    objectToCompare = null;
                    GenerateContent(tab,GENERALPSEUDOTABID, String.Format(BORROWERID, "General"), generalPage);
                }
                else if(tid==BORROWERPSEUDOTABID)
                {
                    foreach(Borrower br in Mp.Borrowers)
                    {
                        Tab tab = new Tab();
                        string tabName = DataHelpers.WrapString(br.FirstName, 25) + " " + DataHelpers.WrapString(br.LastName, 25);
                        tab.Text = tabName;
                        tabstrip.Tabs.Add(tab);
                        PageView generalPage = new PageView();
                        generalPage.ID = String.Format(PAGEVIEWID, newMultiPage.PageViews.Count);
                        newMultiPage.PageViews.Add(generalPage);
                        objectToCompare = br;
                        GenerateContent(tab, BORROWERPSEUDOTABID, String.Format(BORROWERID, br.ID), generalPage);
                    }
                }
            }
            tabstrip.SelectedIndex = CurrentPage.CurrentTopSecondTabIndex;
            newMultiPage.SelectedIndex = CurrentPage.CurrentTopSecondTabIndex;
        }

        protected override void PrepairHtml()
        {
            RadSplitter splitter = GetSplitter(BORROWEROBJECTNAME);
            RadPane panetabs = GetPaneTabs();
            RadSplitBar splitBar1 = GetSplitBar(BORROWEROBJECTNAME);
            RadPane panepageviews = GetPaneViews();
            tabstrip = GetTabStrip(BORROWEROBJECTNAME, CLIENTTABSELECTEDJS, CLIENTLOADJS);
            tabstrip.TabClick += OnTabClick;
            newMultiPage = GetMultiPage(BORROWEROBJECTNAME);
            tabstrip.MultiPageID = newMultiPage.ID;
            panepageviews.Controls.Add(newMultiPage);
            panetabs.Controls.Add(tabstrip);
            splitter.Items.Add(panetabs);
            splitter.Items.Add(splitBar1);
            splitter.Items.Add(panepageviews);
            container.Controls.Add(splitter);
        }
        public override void UserControlAdded(Control ctl)
        {
            if (ctl is BorrowerAkaNames)
            {
                Borrower bor = ObjectToCompare as Borrower;
                if(bor!=null)
                {
                    ((BorrowerAkaNames)ctl).BorrowerId = bor.ID;
                    ctl.Visible = (bor.AKANamesYN != null) ? (bool)bor.AKANamesYN : false;
                }
            }
        }
        protected void OnTabClick(object sender, TabStripEventArgs e)
        {
            CurrentPage.CurrentTopSecondTabIndex = e.Tab.Index;
        }
        #endregion

    }
}