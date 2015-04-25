using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;


namespace LoanStarPortal.Controls
{
    public partial class MortgageCorrespondentLender : MortgageDataControl
    {

        #region constants
        private const string TABGENERAL = "General";
        //private const string TABINVOICES = "Invoices";
        public const string MORTGAGELENDERWRAPPERNAME = "MortgageLender";
        #endregion

        private bool isManager;


        #region methods

        public override void BuildControl(Control pageview)
        {
            container = pageview;

            RadSplitter splitter = new RadSplitter();
            splitter.ID = "Lender_Splitter";
            splitter.Height = Unit.Percentage(95);
            splitter.Orientation = RadSplitterOrientation.Horizontal;
            splitter.Width = Unit.Percentage(100);
            splitter.BorderWidth = Unit.Pixel(0);
            splitter.BorderStyle = BorderStyle.None;
            splitter.BorderSize = 0;
            splitter.Skin = "Default";
            splitter.LiveResize = false;

            RadPane panetabs = new RadPane();
            panetabs.ID = "cl_pane_tab";
            panetabs.Height = Unit.Pixel(25);
            panetabs.Scrolling = RadSplitterPaneScrolling.None;

            RadSplitBar splitBar1 = new RadSplitBar();
            splitBar1.ID = "SplitBarCL";
            splitBar1.CollapseMode = RadSplitBarCollapseMode.None;
            splitBar1.EnableResize = false;
            splitBar1.Visible = false;

            RadPane panepageviews = new RadPane();
            panepageviews.ID = "cl_pane_pageviews";
            panepageviews.Scrolling = RadSplitterPaneScrolling.Y;

            RadTabStrip tabstrip = new RadTabStrip();
            tabstrip.Skin = "Outlook";
            tabstrip.ID = "tabsLender";
            tabstrip.Orientation = RadTabStripOrientation.HorizontalTopToBottom;
            tabstrip.AutoPostBack = false;

            RadMultiPage newMultiPage = new RadMultiPage();
            newMultiPage.ID = "newMPLender";
            newMultiPage.CssClass = "tabpageview";

            tabstrip.MultiPageID = "newMPLender";

            tabstrip.Tabs.Add(new Tab(TABGENERAL));
            PageView newPage1 = new PageView();
            newPage1.ID = "Contacts_" + (newMultiPage.PageViews.Count);
            newMultiPage.PageViews.Add(newPage1);
            newPage1.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
            MortgageContacts mcontacts = (MortgageContacts)LoadControl(Constants.FECONTROLSLOCATION + Constants.FECTLMORTGAGECONTACTS);
            mcontacts.ID = "contacts";
            mcontacts.BuildControl(newPage1);
            newPage1.Controls.Add(mcontacts);

            newPage1.Controls.Add(GetTabDivider(DIVIDERHEIGHT));

            ClosingAgent cagent = (ClosingAgent)LoadControl(Constants.FECONTROLSLOCATION + "ClosingAgent.ascx");
            cagent.ID = "closingagent";
            cagent.BuildControl(newPage1);
            newPage1.Controls.Add(cagent);

            newPage1.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
            MortgageImportantDates mdates = (MortgageImportantDates)LoadControl(Constants.FECONTROLSLOCATION + Constants.FECTLMORTGAGEDATES);
            mdates.ID = "importantdates";
            mdates.BuildControl(newPage1);
            newPage1.Controls.Add(mdates);
            //newPage1.Controls.Add(new LiteralControl(FOOTERDIV));


            //tabstrip.Tabs.Add(new Tab(TABINVOICES));
            //PageView newPage2 = new PageView();
            //newPage2.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
            //Invoices inv = LoadControl(Constants.FECONTROLSLOCATION + Constants.FECTLINVOICES) as Invoices;
            //if (inv != null)
            //{
            //    inv.ID = "invoices";
            //    newPage2.Controls.Add(inv);
            //}
            //newMultiPage.PageViews.Add(newPage2);

            panepageviews.Controls.Add(newMultiPage);
            panetabs.Controls.Add(tabstrip);
            splitter.Items.Add(panetabs);
            splitter.Items.Add(splitBar1);
            splitter.Items.Add(panepageviews);
            pageview.Controls.Add(splitter);

            //pageview.Controls.Add(tabstrip);
            //pageview.Controls.Add(newMultiPage);
            tabstrip.SelectedIndex = 0;
            newMultiPage.SelectedIndex = 0;
        }
        public void GetPostedData()
        {
            GetContactsData();
        }
        public bool SaveData()
        {
            return Mp.SaveContacts() > 0;
        }
        private void GetContactsData()
        {
            isManager = CurrentUser.IsMortgageManager();
            string[] postData = Page.Request.Form.AllKeys;
            if (isManager)
            {
                string search = "$Contacts_" + Mp.ID + "$";
                for (int i = 0; i < postData.Length; i++)
                {
                    int k = postData[i].IndexOf(search);
                    if (k > 0)
                    {
                        string ddlName = postData[i].Substring(k + search.Length);
                        k = ddlName.LastIndexOf("_");
                        if (k > 0)
                        {
                            int roleId = int.Parse(ddlName.Substring(k + 1));
                            int userId = int.Parse(Page.Request[postData[i]]);
                            if (userId > 0)
                            {
                                if (Mp.AssignedUsers.ContainsKey(roleId))
                                {
                                    Mp.AssignedUsers[roleId] = userId;
                                }
                                else
                                {
                                    Mp.AssignedUsers.Add(roleId, userId);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                string search = "$Contacts_" + Mp.ID + "$assignMe";
                for (int i = 0; i < postData.Length; i++)
                {
                    if (postData[i].Contains(search))
                    {
                        string[] roles = Page.Request[postData[i]].Split(',');
                        for (int k = 0; k < roles.Length; k++)
                        {
                            string role = roles[k].Trim();
                            if (!String.IsNullOrEmpty(role))
                            {
                                try
                                {
                                    int roleId = int.Parse(role);
                                    if (Mp.AssignedUsers.ContainsKey(roleId))
                                    {
                                        Mp.AssignedUsers[roleId] = CurrentUser.Id;
                                    }
                                    else
                                    {
                                        Mp.AssignedUsers.Add(roleId, CurrentUser.Id);
                                    }

                                }
                                catch { }
                            }
                        }
                        break;
                    }
                }
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}