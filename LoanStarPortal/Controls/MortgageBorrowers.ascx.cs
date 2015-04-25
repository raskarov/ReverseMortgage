using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class MortgageBorrowers : MortgageDataControl
    {
        #region constants

        private const string DECLARATIONSHEADER = "Declarations";
        private const string GENERALINFOHEADER = "General info";
        private const string HMDAHEADER = "HMDA";
        private const string POAHEADER = "Special Circumstances";
        private const string ALTCONTACTHEADER = "Alt contact";
        private const string IDENTIFICATIONHEADER = "Regulatory";
        private const string COUNCELINGHEADER = "Counceling";
        private const string BORROWERSOBJECTNAME = "Borrowers";
        public const string NEWBORROWERID = "newborrowerid";
        public const string BORROWERWRAPPERNAME = "Borrower";

        #region borrowers field groups

        private const int GENERALGROUPID = 1;
        private const int DECKARATIONGROUPID = 2;
        private const int HDMAGROUPID = 3;
        private const int POAGROUPID = 4;
        private const int ALTCONTACTGROUPID = 5;
        private const int IDENTIFICATIONGROUPID = 6;
        private const int COUNCELINGGROUPID = 7;
        private static readonly string[] BorrowerGeneralInfo = { 
            "SalutationId","FirstName", "LastName","MiddleInitial","DateOfBirth",
            "Address1", "Address2", "SSN", "Zip", "City", "Phone",  "StateID" ,
            "ActualAge","MartialStatusId","SexId","NearestAge","YearsAtPresentAddress",
            "MonthlyIncome","RealEstateAssets","AvailableAssets","DifferentMailingAddressId" ,"TotalAmountOfNonREDebts"
            ,"AKA1","AKA2","AKA3","AKA4","OtherRealEstateAssets"
        };
        private static readonly string[] BorrowerDeclaration = { 
            "DecJudments","DecBuncruptcy", "DecLawsuit","DecFedDebt","DecPrimaryres","DecEndorser",
            "DecUSCitizen", "DecPermanentRes"
        };
        private static readonly string[] BorrowerHDMA = { 
            "HDMAHideId", "HDMARaceId", "HDMAEthnicityId"
        };
        private static readonly string[] BorrowerPOA = { 
            "UsePOAId","PoaDurableId","PoaEncumberingId","PoaRevocableId","PoaIncapacitatedId","PoaExecutionDate"
            ,"POALegalCapacityId","POAReviewedDocumentId","POATransactionApproveId","POASignedByJudgeId","POAMentionedInterestRateId"
            ,"POAMentionedAmountId","POAMentionedByNameId","POAEqualOrGreaterMaximumId"
            ,"CourtOrdRecvdId","CourtOrdSignedId","GuarFirstName","GuarLastName","GuarAddress1","GuarAddress2"
            ,"GuarCity","GuarStateId","GuarZip","GuarSSNum","GuarDOB","GuarCounsId"
            ,"CrtOrdAprvRMId","CrtOrdLnAmntMentionedId","CrtOrdLnAmntOkId","CrtOrdIntRateMentionedId","CrtOrdIntRateAsInitialRateId"
            ,"CrtOrdSignDate","POAFirstName","POALastName","POAAddress1","POAAddress2","POACity","POAStateId","POAZip","POASSNum"
            ,"POADOB","POACounseledId","POATitleApprvdId","POABorrCompetentId","POABorrSignAppId"
            ,"POABorrIncompetencyDocuemtned"
        };
        private static readonly string[] BorrowerAltContact = { 
            "AltContactName","AltContactRelationship","AltContactAddress1","AltContactAddress2","AltContactCity"
            ,"AltContactStateId","AltContactZip","AltContactPhone","AltContactAltPhone"
        };
        private static readonly string[] BorrowerIdentification = { 
            "IdentificationDocTypeId","IdentificationPrimaryDocOriginName","PrimaryIdentificationDocId"
            ,"IdentificationPrimaryDocIDNumber","IdentificationPrimaryDocIssuanceDate","IdentificationPrimaryDocExpirationDate"
            ,"SecondaryIdentificationDoc1Id","IdentificationSecondaryDoc1OriginName","IdentificationSecondaryDoc1IDNumber","IdentificationSecondaryDoc1IssuanceDate","IdentificationSecondaryDoc1ExpirationDate"
            ,"SecondaryIdentificationDoc2Id","IdentificationSecondaryDoc2OriginName","IdentificationSecondaryDoc2IDNumber","IdentificationSecondaryDoc2IssuanceDate","IdentificationSecondaryDoc2ExpirationDate"
        };
        private static readonly string[] BorrowerCounseling = { 
            "CounselingMethodId","PartialHMDAInfoStmnt","CertifyRaceVisualObserId","CounseledId","InclOnCounsCertId"
        };

        #endregion

        #endregion

        private int NewBorrowerId
        {
            get
            {
                int res = -1;
                Object o = Session[NEWBORROWERID];
                if (o != null)
                {
                    try
                    {
                        res = Convert.ToInt32(o);
                    }
                    catch { }
                }
                return res;
            }
            set
            {
                Session[NEWBORROWERID] = value;
            }
        }

        #region methods

        #region public
        public override void BuildControl(Control _container)
        {
            objectName = BORROWERSOBJECTNAME;
            base.BuildControl(_container);
        }
        protected override void SetUIFields()
        {
            SetUIGroupFields(objectFields, BorrowerGeneralInfo, GENERALGROUPID);
            SetUIGroupFields(objectFields, BorrowerDeclaration, DECKARATIONGROUPID);
            SetUIGroupFields(objectFields, BorrowerHDMA, HDMAGROUPID);
            SetUIGroupFields(objectFields, BorrowerPOA, POAGROUPID);
            SetUIGroupFields(objectFields, BorrowerAltContact, ALTCONTACTGROUPID);
            SetUIGroupFields(objectFields, BorrowerIdentification, IDENTIFICATIONGROUPID);
            SetUIGroupFields(objectFields, BorrowerCounseling, COUNCELINGGROUPID);
        }
        protected override void PrepairObjectHtml()
        {
            int radtabindex;
            RadSplitter splitter = new RadSplitter();
            splitter.ID = "Borrower_Splitter";
            splitter.Height = Unit.Percentage(95);
            splitter.Orientation = RadSplitterOrientation.Horizontal;
            splitter.Width = Unit.Percentage(100);
            splitter.BorderWidth = Unit.Pixel(0);
            splitter.BorderStyle = BorderStyle.None;
            splitter.BorderSize = 0;
            splitter.Skin = "Default";
            splitter.LiveResize = false;

            RadPane panetabs = new RadPane();
            panetabs.ID = "bs_pane_tab";
            panetabs.Height = Unit.Pixel(25);
            panetabs.Scrolling = RadSplitterPaneScrolling.None;

            RadSplitBar splitBar1 = new RadSplitBar();
            splitBar1.ID = "SplitBar1";
            splitBar1.CollapseMode = RadSplitBarCollapseMode.None;
            splitBar1.EnableResize = false;
            splitBar1.Visible = false;

            RadPane panepageviews = new RadPane();
            panepageviews.ID = "bs_pane_pageviews";
            panepageviews.Scrolling = RadSplitterPaneScrolling.Y;

            RadTabStrip tabstrip = new RadTabStrip();
            tabstrip.EnableViewState = false;
            tabstrip.Skin = "Outlook";
            tabstrip.ID = "tabsBorrower";
            tabstrip.Orientation = RadTabStripOrientation.HorizontalTopToBottom;
            tabstrip.AutoPostBack = true;
            tabstrip.OnClientTabSelected = "TabSelected";
            tabstrip.OnClientLoad = "OnTabLoad";

            RadMultiPage newMultiPage = new RadMultiPage();
            newMultiPage.ID = "newMPBorrower";
            newMultiPage.CssClass = "tabpageview";
            newMultiPage.RenderSelectedPageOnly = true;

            tabstrip.MultiPageID = "newMPBorrower";
            panepageviews.Controls.Add(newMultiPage);

            panetabs.Controls.Add(tabstrip);
            splitter.Items.Add(panetabs);
            splitter.Items.Add(splitBar1);
            splitter.Items.Add(panepageviews);

            container.Controls.Add(splitter);

            tabstrip.Tabs.Add(new Tab("General"));
            PageView generalPage = new PageView();
            generalPage.ID = "PVBorrowerGeneral" + (newMultiPage.PageViews.Count);
            newMultiPage.PageViews.Add(generalPage);
            generalPage.Controls.Add(GetBorrowerGeneralControl());
            generalPage.Controls.Add(new LiteralControl(FOOTERDIV));


            radComboWidth = RADCOMBOWIDTH;
            int count = 0;
            int selectedTab = 0;
            radtabindex = 0;
            foreach (Borrower borrower in Mp.Borrowers)
            {
                string tabName = DataHelpers.WrapString(borrower.FirstName, 25) + " " + DataHelpers.WrapString(borrower.LastName, 25);
                if (NewBorrowerId == borrower.ID)
                {
                    selectedTab = count;
                    NewBorrowerId = -1;
                }
                if (count == radtabindex)
                {
                    radComboWidth = RADCOMBOWIDTH;
                }
                count++;
                tabstrip.Tabs.Add(new Tab(tabName));
                PageView newPage = new PageView();
                newPage.ID = "newPVBorrower" + (newMultiPage.PageViews.Count);
                newMultiPage.PageViews.Add(newPage);
                newPage.Controls.Add(GetBorrowerControl(borrower));
                newPage.Controls.Add(new LiteralControl(FOOTERDIV));
                radComboWidth = RADCOMBOWIDTHFIX;
            }
            tabstrip.SelectedIndex = selectedTab;
            newMultiPage.SelectedIndex = selectedTab;
        }
        #endregion

        #region private
        private ControlWrapper GetBorrowerGeneralControl()
        {
            ControlWrapper wrapper = (ControlWrapper) LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
            wrapper.ID = BORROWERWRAPPERNAME + "_general";
            MortgageBorrowerGeneralTab mbgt = (MortgageBorrowerGeneralTab)LoadControl(Constants.FECONTROLSLOCATION + "MortgageBorrowerGeneralTab.ascx");
            mbgt.ID = BORROWERWRAPPERNAME + "_MortgageBorrowerGeneralTab";
            if (mbgt != null)
            {
                mbgt.BuildControl(wrapper);
            }
            wrapper.Controls.Add(mbgt);
            wrapper.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
            return wrapper;
        }

        private ControlWrapper GetBorrowerControl(BaseObject borrower)
        {
            ControlWrapper wrapper = (ControlWrapper)LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
            wrapper.ID = BORROWERWRAPPERNAME+"_" + borrower.ID;
            HtmlGenericControl div1 = new HtmlGenericControl("div");
            div1.ID = "divb";
            Mp.EvaluateObjectFieldsVisibilty(borrower, objectFields);
            div1.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
            Fields ctl = GetTabWrapper(GENERALINFOHEADER, borrower, objectFields, GENERALGROUPID, "trborrower", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl.ID = "cb" + GENERALGROUPID + "_" + borrower.ID;
            if (ctl.HasChild)
            {
                div1.Controls.Add(ctl);
            }
            ctl = GetTabWrapper(DECLARATIONSHEADER, borrower, objectFields, DECKARATIONGROUPID, "trborrower", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl.ID = "cb" + DECKARATIONGROUPID + "_" + borrower.ID;
            if (ctl.HasChild)
            {
                div1.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
                div1.Controls.Add(ctl);
            }
            ctl = GetTabWrapper(HMDAHEADER, borrower, objectFields, HDMAGROUPID, "trborrower", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl.ID = "cb" + HDMAGROUPID + "_" + borrower.ID;
            if (ctl.HasChild)
            {
                div1.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
                div1.Controls.Add(ctl);
            }
            ctl = GetTabWrapper(POAHEADER, borrower, objectFields, POAGROUPID, "trborrower", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl.ID = "cb" + POAGROUPID + "_" + borrower.ID;
            if (ctl.HasChild)
            {
                div1.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
                div1.Controls.Add(ctl);
            }
            ctl = GetTabWrapper(ALTCONTACTHEADER, borrower, objectFields, ALTCONTACTGROUPID, "trborrower", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl.ID = "cb" + ALTCONTACTGROUPID + "_" + borrower.ID;
            if (ctl.HasChild)
            {
                div1.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
                div1.Controls.Add(ctl);
            }
            ctl = GetTabWrapper(IDENTIFICATIONHEADER, borrower, objectFields, IDENTIFICATIONGROUPID, "trborrower", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl.ID = "cb" + IDENTIFICATIONGROUPID  + "_" + borrower.ID;
            if (ctl.HasChild)
            {
                div1.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
                div1.Controls.Add(ctl);
            }
            ctl = GetTabWrapper(COUNCELINGHEADER, borrower, objectFields, COUNCELINGGROUPID, "trborrower", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl.ID = "cb" + COUNCELINGGROUPID + "_" + borrower.ID;
            if (ctl.HasChild)
            {
                div1.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
                div1.Controls.Add(ctl);
            }
            //short currentTabIndex = tabIndex++;
            //MortgageBorrowerCreditReportReview mbcrr = (MortgageBorrowerCreditReportReview)LoadControl(Constants.FECONTROLSLOCATION + "MortgageBorrowerCreditReportReview.ascx");
            //mbcrr.tabIndex = currentTabIndex;
            //if (mbcrr != null)
            //{
            //    ControlWrapper wrapper1 = (ControlWrapper)LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
            //    wrapper1.ID = "wbs$MortgageBorrowerCreditReportReview_" + borrower.ID;

            //    mbcrr.BuildControl(wrapper1);
            //    wrapper1.Controls.Add(mbcrr);

            //    div1.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
            //    div1.Controls.Add(wrapper1);
            //}
            //currentTabIndex = tabIndex++;
            //MortgageBorrowerFundsToClose mbftc = (MortgageBorrowerFundsToClose)LoadControl(Constants.FECONTROLSLOCATION + "MortgageBorrowerFundsToClose.ascx");
            //mbftc.tabIndex = currentTabIndex;
            //if (mbftc != null)
            //{
            //    ControlWrapper wrapper2 = (ControlWrapper)LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
            //    wrapper2.ID = "wbs$MortgageBorrowerFundsToClose_" + borrower.ID;

            //    mbftc.BuildControl(wrapper2);
            //    wrapper2.Controls.Add(mbftc);

            //    div1.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
            //    div1.Controls.Add(wrapper2);
            //}


            wrapper.Controls.Add(div1);
            return wrapper;
        }
        #endregion   

        #endregion

    }

}