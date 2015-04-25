using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class MortgageProperty : MortgageDataControl
    {
        #region constants
        private const string PROPERTYOBJECTNAME = "Property";
        private const string GENERALHEADER = "Description";
        private const string HAZARDHEADER = "Hazard";
        private const string FLOODHEADER = "Flood";
        private const string SITEHEADER = "Site";
        private const string IMPROVEMENTSHEADER = "Improvements";
        private const string APPRAISALREVIEWHEADER = "Appraisal Review";
        private const string INSPECTIONREVIEWHEADER = "Inspection Review";
        private const string COMMITMENTREVIEWHEADER = "Commitment Review";
        private const string TERMITEREPORTREVIEWHEADER = "Termite Report Review";
        private const string REPORTREVIEWHEADER = "Report Review";
        private const string HOMEOWNERHEADER = "Home Owner's Association";
        private const string TABGENERAL = "General";
        private const string TABLIENS = "Liens";
        private const string TABINSURANCE = "Insurance";
        public const string PROPERTYWRAPPERNAME = "Property";

        #region property field groups
        private const int GENERALGROUPID = 1;
        private const int HAZARDGROUPID = 3;
        private const int FLOODGROUPID = 4;
        private const int SITEGROUPID = 5;
        private const int IMPROVEMENTSGROUPID = 6;
        private const int APPRAISALREVIEWGROUPID = 7;
        private const int INSPECTIONREVIEWGROUPID = 8;
        private const int COMMITMENTREVIEWGROUPID = 9;
        private const int REPORTREVIEWGROUPID = 10;
        private const int HOMEOWNERGROUPID = 11;
        private const int TERMITEREPORTREVIEWGROUPID = 12;
        
        private static readonly string[] PropertyGeneralInfo = { 
            "ProjectName","Address1", "Address2","PropertyTaxDueAmount","City","NeighborhoodName","StateId","Zip","CountyID"
            ,"SPCounty","SPOwnerOfPublicRecord","SPLegalDescription","SPAssessorsParcelNumber"
            ,"SPOccupancyId","SPHomeTypeId","SPHOAAssessmentAmount","SPHOAAssessmentUnitId","SPTitleHeldId"
            ,"SPTitleHeldLeaseholdExpiration","SPHeldInTrustId","SPWaterId","SPSewerId","SPStreetId"
            ,"SPFloodZoneId","SPUnitsId","SPYearBuilt","SPValue","SPRepairsYNId","RealEstateTaxes","HOAFees"
            ,"PropertyRightsOther","HOAFeesPaidId","DaysAtProperty183Id"
            ,"ManuCondoId","ManuSqFt","ManuDateBuilt","ManuOrigLocId","ManuTaxedAsREId","ManuPermChassisId","ManuAxlesToungueGoneId"
            ,"ManuUtilitiesInstalledId","ManuSkirtingId","ManuAdditionsEverId","ManuPermFoundId","ManuPermFoundInspectedId"
            ,"ManuPermFoundInspRecvdId","ManuFoundInspOkId","ManuHaveTagsId", "RecvdEstimates", "RepairsStructural"
        };
        private static readonly string[] PropertyHazardInfo = { 
            "HazardId","HazDwelling", "HazStart","HazExp","HazPremium","HazMtgeeClauseOK","HazAllBorrOnPolicy"
            ,"HazAgencyName","HazAgentName","HazAgencyPhone","HazAgencyFax"
            ,"HazardPolicyPeriodId","LenderMortgageeClauseHzdLabel","PolicyMortgageeClauseMatchYNId","AllBorrowersOnPolicyYNId"
            ,"ReadAntiCoercionStmntId","SelectedHazardAgentId","HazAgencyAddress1","HazAgencyAddress2","HazAgencyCity"
            ,"HazAgencyStateId","HazAgencyZip"
            ,"HazDecRecvdId","HazPolGuarReplCostId","HazPolCoverageAmnt","HazPolDeductAmnt","HazPolStart","HazPolEnd"
            ,"HazPolAddressOkId","HazPolNamesOkId","ISelectedHazardAgent"
        };
        private static readonly string[] PropertyFloodInfo = { 
            "FloodId","FldDwelling", "FldStart","FldExp","FldPremium","FldMtgeeClauseOK","FldAllBorrOnPolicy"
            ,"FloodPolicyPeriodId","FldAgencyName","FldAgentName","FldAgencyPhone","FldAgencyFax","LenderMortgageeClauseFldLabel"
            ,"FloodCertOrderedId","FloodCertRecvdId","FloodCertGEOCodingId","FloodCertLifeOfLoanId","FloodCertLifeOfLoanAssignedId"
            ,"FloodCertFloodZoneId","FloodDecRecvdId","FloodPolicyCoverage","FloodPolicyCoverageIncreasingId"
            ,"FloodPolicyDeductible","FloodPolicyStart","FloodPolicyEnd","ManuFEMALetterId","ManuElevationCertReceivedId","ElevCertAbove100YrFldPlnId"
        };
        private static readonly string[] PropertySiteInfo = { 
            "Area","WaterId","SanatarySewerId","WaterPrivate","SanatarySewerPrivate","StreetId", "FEMAMapNumber", "FEMAMapDate","FloodZoneDescription"
        };
        private static readonly string[] PropertyImprovementsInfo = { 
            "NumberOfStories","StatusID","PoolId","FoundationTypeID","BuiltAfter1978Id","ChildUnder7Id","WellId","SepticId"
        };
        private static readonly string[] PropertyAppraisalReviewInfo = { 
            "OrderedAppraisalId","ReceivedAppraisalId","AppraisalAddressMatchId","AppraisalCaseNumAllPgsId","AppraiserFHAApprovedId"
            ,"AppraiserLicensedId","AppraisalLglDescriptionId","AppraisalCensusTractNumId","AppraisalOwnerOccId","AppraisalLotDimPresentId"
            ,"AppraisalLandAreaId","AppraisalZoningId","AppraisalBestUseId","AppraisalAdverseConditionsId","AppraisalRepairsYNId","AppraisalFourOrLessUnitsId"
            ,"AppraisalAsIsId","AppraisalREL","AppraisalSiteCostNewPresentId","AppraisalSiteValue","AppraisalCostNew","PUDSectionCompleteId"
            ,"AppraisalPhotosPresentId","AppraisalSketchPresentId","AppraisalMapPresentId","AppraisalCommentWellSepticId"
            ,"AppraisalInspectionDate"
            ,"AppraisedValue","PrimaryHeatSourceId","SpaceHeatInspectionId","SpaceHeatTypicalId","ConvHeat50DegreesId"
            ,"WhichAppraisalFormUsedId","PublicWaterStmntId","PublicWaterAvailableId","RecvdBidWaterHookupId","IncludeTapFeeId"
            ,"WaterHookupBid","WaterHookupWaivedId","PrivateWaterTypeId","WaterTestNeededId","WaterTestOrderedId","WaterTestReceivedId"
            ,"WaterTestPotableId","NewWellInstalledId","NewWellCostPaidAtClosingId","NewWellInvoiceRecvdId"
            ,"SharedWellNumberHouses","SharedWellRecvdAgreemntId","PublicSewerStmntId","PublicSewerAvailableId"
            ,"SewerHookupBidRecvdId","SewerBidIncludeTapFeeId","SewerBidAmount","TryToWaiveSewerHookupId","SewerHookupWaivedId"
            ,"PrivateSewerTypeId","SepticConditionId","SepticInspectionOrderedId","SepticInspReportRecvdId","RecvdSharedSepticAgreemntId"
            ,"ManuAppraisalId","CondoAppraisalId","AppraiserName"
        };
        private static readonly string[] PropertyInspectionReviewInfo = { 
            "PropInspOrderedId","PropInspReceivedId"
        };
        private static readonly string[] PropertyCommitmentReviewInfo = { 
            "WhatOtherTypeOnTitleId","OtherPersonsInVestingId","TrustRecvdId","TrustAllPagesId","TrustBorrTheBeneficId"
            ,"TrustRevocableId","TitleApprovedTrustId","LegalStandNBSId","NBSName","NBSAddress1","NBSAddress2","NBSCity"
            ,"NBSStateId","NBSZip","NBSSS","NBSDOB"
            ,"NBSRecvdLetterId","NBSStateSeparationId"
            ,"NBSLiveSeparateStmntId","NBSNoInterestStmntId","NBSApproveRMStmntId","NBSNotarizedId"
        };
        private static readonly string[] PropertyReportReviewInfo = { 
            "OrdTitleYetId","RecTitleYetId","TitleIssueDate","TitlePropAddressOkId","TitleFHACaseNumbOkId"
            ,"TitleLglDesc","TitleTaxSearchId","TitleLiensYNId","NextPropTaxDue","OnTitleNotApplyId","VestingChangingId"
            ,"RemovDeceasedId","DeathCertReceivedYNId","ExactLegalVesting","StandEndorseId","PUDEndorseId","TrustEndorseId"
            ,"IncludeTitleInsuredClauseId","ReceivedCPLId","CPLLetterOkId","AskedForSurveyId","ReceivedSurveyId"
            ,"SurveyTitleResponseId","SurveyOrderedId","SurveyRecvdId","SurveyApprovedId","AddLienStmnt"
        };
        private static readonly string[] PropertyHomeOwnerInfo = { 
            "RecvdMasterPolicyId","MasterPol100ImprovId","MasterPolProjectNameId","MasterPolManagCoId","MasterPolBorrNamesId"
            ,"MasterPolLoanNumId","MasterPolInsAgntInfoYNId","MasterPolStartDate","MasterPolEndDate"
        };
        private static readonly string[] PropertyTermiteReportReviewInfo = { 
            "AppraisalTermiteFlag", "TermiteDamageFlag", "TermiteInfestationEscrowed", "TermiteInfestationGone", 
            "TermiteOrdered", "TermiteReceived", "TermiteReinspectionRecvd", "TermiteReportInfestation", "TermiteReportLeak", 
            "TermiteTreatmentBidItemized", "TermiteTreatmentCompleted", "TermiteTreatmentOrdered", "TermiteTreatmentQualifiedBid", 
            "TermiteTreatmentRecvdBid", "TermiteReportInspDate", "TermiteTreatmentBidAmount", "HowManyInsp", 
            "TermiteReportExpired"
        };
        #endregion

        #endregion

        public event UpdateNeeded OnUpdateNeeded;
        public delegate void UpdateNeeded();

        private Property mpprop;
        private RadTabStrip tabstrip;

        #region methods

        #region public
        public override void BuildControl(Control pageview)
        {
            objectName = PROPERTYOBJECTNAME;
            mpprop = Mp.Property;
            base.BuildControl(pageview);            
            mpprop = Mp.Property;
        }
        protected override void SetUIFields()
        {
            SetUIGroupFields(objectFields, PropertyGeneralInfo, GENERALGROUPID);
            SetUIGroupFields(objectFields, PropertyHazardInfo, HAZARDGROUPID);
            SetUIGroupFields(objectFields, PropertyFloodInfo, FLOODGROUPID);
            SetUIGroupFields(objectFields, PropertySiteInfo, SITEGROUPID);
            SetUIGroupFields(objectFields, PropertyImprovementsInfo, IMPROVEMENTSGROUPID);
            SetUIGroupFields(objectFields, PropertyAppraisalReviewInfo, APPRAISALREVIEWGROUPID);
            SetUIGroupFields(objectFields, PropertyInspectionReviewInfo, INSPECTIONREVIEWGROUPID);
            SetUIGroupFields(objectFields, PropertyCommitmentReviewInfo, COMMITMENTREVIEWGROUPID);
            SetUIGroupFields(objectFields, PropertyReportReviewInfo, REPORTREVIEWGROUPID);
            SetUIGroupFields(objectFields, PropertyHomeOwnerInfo, HOMEOWNERGROUPID);
            SetUIGroupFields(objectFields, PropertyTermiteReportReviewInfo, TERMITEREPORTREVIEWGROUPID);
        }
        protected override void PrepairObjectHtml()
        {
            Mp.EvaluateObjectFieldsVisibilty(mpprop,objectFields);
            NeedReload = NeedReload || Mp.SetFieldValueForObject(mpprop, PROPERTYOBJECTNAME, FieldsDataRules, RuleDependantFields);

            RadSplitter splitter = new RadSplitter();
            splitter.ID = "Property_Splitter";
            splitter.Height = Unit.Percentage(95);
            splitter.Orientation = RadSplitterOrientation.Horizontal;
            splitter.Width = Unit.Percentage(100);
            splitter.BorderWidth = Unit.Pixel(0);
            splitter.BorderStyle = BorderStyle.None;
            splitter.BorderSize = 0;
            splitter.Skin = "Default";
            splitter.LiveResize = false;

            RadPane panetabs = new RadPane();
            panetabs.ID = "ps_pane_tab";
            panetabs.Height = Unit.Pixel(25);
            panetabs.Scrolling = RadSplitterPaneScrolling.None;

            RadSplitBar splitBar1 = new RadSplitBar();
            splitBar1.ID = "SplitBarPS";
            splitBar1.CollapseMode = RadSplitBarCollapseMode.None;
            splitBar1.EnableResize = false;
            splitBar1.Visible = false;

            RadPane panepageviews = new RadPane();
            panepageviews.ID = "ps_pane_pageviews";
            panepageviews.Scrolling = RadSplitterPaneScrolling.Y;


            tabstrip = new RadTabStrip();
            tabstrip.Skin = "Outlook";
            tabstrip.ID = "tabsProperty";
            tabstrip.Orientation = RadTabStripOrientation.HorizontalTopToBottom;
            tabstrip.AutoPostBack = true;

            RadMultiPage newMultiPage = new RadMultiPage();
            newMultiPage.ID = "newMPProperty";
            newMultiPage.CssClass = "tabpageview";
            newMultiPage.RenderSelectedPageOnly = true;

            tabstrip.MultiPageID = "newMPProperty";

            panepageviews.Controls.Add(newMultiPage);
            panetabs.Controls.Add(tabstrip);
            splitter.Items.Add(panetabs);
            splitter.Items.Add(splitBar1);
            splitter.Items.Add(panepageviews);
            container.Controls.Add(splitter);

            radComboWidth = RADCOMBOWIDTH;
            PropertyRepairItems repairItems = (PropertyRepairItems)LoadControl(Constants.FECONTROLSLOCATION + Constants.FECTLREPAIRITEMS);
            repairItems.ID = "repairItems_" + mpprop.ID;
            Tab generaltab = new Tab(TABGENERAL);
            tabstrip.Tabs.Add(generaltab);
            PageView newPage1 = new PageView();
            newPage1.ID = "newPVrepairItems" + (newMultiPage.PageViews.Count);
            newMultiPage.PageViews.Add(newPage1);

            newPage1.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));

            Fields ctl = GetTabWrapper(GENERALHEADER, mpprop, objectFields, GENERALGROUPID, "trpropertyg", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            if(ctl.HasChild)
            {
                ctl.ID = "cg" + GENERALGROUPID;
                repairItems.AddFields(ctl.plholder);
                if (mpprop.SPRepairsYNId == null || !(bool)mpprop.SPRepairsYNId)
                {
                    repairItems.HideGrid();
                }
                newPage1.Controls.Add(repairItems);
            }
            ControlWrapper wrapper1 = (ControlWrapper)LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
            wrapper1.ID = "wps$" + PROPERTYWRAPPERNAME + "_" + mpprop.ID;
            ctl = GetTabWrapper(SITEHEADER, mpprop, objectFields, SITEGROUPID, "trproperty", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            if(ctl.HasChild)
            {
                ctl.ID = "cp" + SITEGROUPID;
                wrapper1.Controls.Add(ctl);
                newPage1.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                newPage1.Controls.Add(wrapper1);
            }

            wrapper1 = (ControlWrapper)LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
            wrapper1.ID = "wpi$" + PROPERTYWRAPPERNAME + "_" + mpprop.ID;
            ctl = GetTabWrapper(IMPROVEMENTSHEADER, mpprop, objectFields, IMPROVEMENTSGROUPID, "trproperty", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            if(ctl.HasChild)
            {
                ctl.ID = "cp" + IMPROVEMENTSGROUPID;
                wrapper1.Controls.Add(ctl);
                newPage1.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                newPage1.Controls.Add(wrapper1);
            }

            wrapper1 = (ControlWrapper)LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
            wrapper1.ID = "war$" + PROPERTYWRAPPERNAME + "_" + mpprop.ID;
            ctl = GetTabWrapper(APPRAISALREVIEWHEADER, mpprop, objectFields, APPRAISALREVIEWGROUPID, "trproperty", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            if(ctl.HasChild)
            {
                ctl.ID = "cp" + APPRAISALREVIEWGROUPID;
                wrapper1.Controls.Add(ctl);
                newPage1.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                newPage1.Controls.Add(wrapper1);
            }

            wrapper1 = (ControlWrapper)LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
            wrapper1.ID = "wir$" + PROPERTYWRAPPERNAME + "_" + mpprop.ID;
            ctl = GetTabWrapper(INSPECTIONREVIEWHEADER, mpprop, objectFields, INSPECTIONREVIEWGROUPID, "trproperty", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            if(ctl.HasChild)
            {
                ctl.ID = "cp" + INSPECTIONREVIEWGROUPID;
                wrapper1.Controls.Add(ctl);
                newPage1.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                newPage1.Controls.Add(wrapper1);
            }

            wrapper1 = (ControlWrapper)LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
            wrapper1.ID = "wcr$" + PROPERTYWRAPPERNAME + "_" + mpprop.ID;
            ctl = GetTabWrapper(COMMITMENTREVIEWHEADER, mpprop, objectFields, COMMITMENTREVIEWGROUPID, "trproperty", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            if(ctl.HasChild)
            {
                ctl.ID = "cp" + COMMITMENTREVIEWGROUPID;
                wrapper1.Controls.Add(ctl);
                newPage1.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                newPage1.Controls.Add(wrapper1);
            }

            wrapper1 = (ControlWrapper)LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
            wrapper1.ID = "wrr$" + PROPERTYWRAPPERNAME + "_" + mpprop.ID;
            ctl = GetTabWrapper(REPORTREVIEWHEADER, mpprop, objectFields, REPORTREVIEWGROUPID, "trproperty", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            if(ctl.HasChild)
            {
                ctl.ID = "cp" + REPORTREVIEWGROUPID;
                wrapper1.Controls.Add(ctl);
                newPage1.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                newPage1.Controls.Add(wrapper1);
            }
            short currentTabIndex = tabIndex++;
            MortgageTrustee mtee = (MortgageTrustee)LoadControl(Constants.FECONTROLSLOCATION + "MortgageTrustee.ascx");
            mtee.tabIndex = currentTabIndex;
            if (mtee != null)
            {
                newPage1.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                mtee.BuildControl(newPage1);
                newPage1.Controls.Add(mtee);
            }

            Payoffs payoff = (Payoffs)LoadControl(Constants.FECONTROLSLOCATION + "Payoffs.ascx");
            payoff.OnUpdateNeeded += UpdateTabs; 
            if (payoff != null)
            {
                payoff.ID = "payoffs_" + mpprop.ID;
                Tab payofftab = new Tab(TABLIENS);
                if (Mp.PayoffUpdateNeeded)
                {
                    payofftab.Attributes.Add("style", "color:red");
                }
                tabstrip.Tabs.Add(payofftab);

                PageView newPage = new PageView();
                newPage.ID = "newPVPayoffs" + (newMultiPage.PageViews.Count);
                newMultiPage.PageViews.Add(newPage);

                newPage.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                newPage.Controls.Add(payoff);
            }

            radComboWidth = RADCOMBOWIDTHFIX;

            ctl = GetTabWrapper(HAZARDHEADER, mpprop, objectFields, HAZARDGROUPID, "trproperty", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl.ID = "cp" + HAZARDGROUPID;
            Fields ctl2 = GetTabWrapper(FLOODHEADER, mpprop, objectFields, FLOODGROUPID, "trproperty", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl2.ID = "cp" + FLOODGROUPID;
            Fields ctl3 = GetTabWrapper(HOMEOWNERHEADER, mpprop, objectFields, HOMEOWNERGROUPID, "trproperty", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            ctl3.ID = "cp" + HOMEOWNERGROUPID;
            if (ctl.HasChild || ctl2.HasChild || ctl3.HasChild)
            {
                ControlWrapper wrapper = (ControlWrapper)LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
                wrapper.ID = "a1$" + PROPERTYWRAPPERNAME + "_" + mpprop.ID;

                tabstrip.Tabs.Add(new Tab(TABINSURANCE));
                PageView newPage = new PageView();
                newPage.ID = "newPVProperty" + (newMultiPage.PageViews.Count);
                newMultiPage.PageViews.Add(newPage);

                newPage.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                newPage.Controls.Add(wrapper);

                if (ctl.HasChild) wrapper.Controls.Add(ctl);
                if (ctl2.HasChild)
                {
                    wrapper.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
                    wrapper.Controls.Add(ctl2);
                }
                if(ctl3.HasChild)
                {
                    wrapper.Controls.Add(GetTabDivider(DIVIDERHEIGHT));
                    wrapper.Controls.Add(ctl3);
                }
            }

            wrapper1 = (ControlWrapper)LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
            wrapper1.ID = "wtr$" + PROPERTYWRAPPERNAME + "_" + mpprop.ID;
            ctl = GetTabWrapper(TERMITEREPORTREVIEWHEADER, mpprop, objectFields, TERMITEREPORTREVIEWGROUPID, "trproperty", new string[] { "tdblGeneral1/tdbcGeneral1", "tdblGeneral2/tdbcGeneral2" }, 2);
            if(ctl.HasChild)
            {
                ctl.ID = "cp" + TERMITEREPORTREVIEWGROUPID;
                wrapper1.Controls.Add(ctl);
                newPage1.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
                newPage1.Controls.Add(wrapper1);
                newPage1.Controls.Add(GetTabDivider(TOPDIVIDERHEIGHT));
            }

            tabstrip.SelectedIndex = 0;
            newMultiPage.SelectedIndex = 0;
        }
        private void UpdateTabs()
        {
            if (Mp.PayoffUpdateNeeded)
            {
                tabstrip.Tabs[1].Attributes.Add("style", "color:red");
            }
            else
            {
                tabstrip.Tabs[1].Attributes.Add("style", "");
            }
            if (OnUpdateNeeded!=null)
            {
                OnUpdateNeeded();
            }
        }

        #endregion

        #endregion

   }
}