using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditProduct : AppControl
    {

        private const string ADDHEADERTEXT = "Add new product";
        private const string EDITHEADERTEXT = "Edit product({0})";
        private const string ALREADYEXISTS = "Product already exists";
        private const string ONCHANGEATTRIBUTE = "onchange";
        private const string ONCLICKATTRIBUTE = "onclick";
        private const string JSSHOWROW = "SetRowVisibility(this,{0},'{1}');";
        private const string JSSHOWROW1 = "SetRowVisibility1(this,{0},'{1}',{2},'{3}');";
        private const string JSCBSHOWROW = "SetRowVisibilityCb(this.checked,'{0}');";
        private const string JSCBBAYDOCSHOWROW = "SetRowVisibilityBaydocCode(this.checked,'{0}','{1}');";
        private const string JSCBSHOWROWRATELOCK = "SetRowVisibilityProtection(this,'{0}','{1}','{2}','{3}');";
        private const string JSCBSHOWROWINPUTMETHOD = "SetRowVisibilityInputMethod(this,'{0}');";
        private const string JSENABLEBUTTON = "SetButtonContinue(this,'{0}');";
        private const string STYLEATTRIBUTE = "style";
        private const string TRVISIBLE = "display:inline";
        private const string TRHIDDEN = "display:none";
        private const string PRODUCTFILTER = "productId={0}";
        private const string STEP = "addproductstep";
        private const string PRODUCTTEMPLATEID = "prodacttemplateid";
        private Product product;
        private bool isNew;
        private ServiceFee serviceFee;

        private int Step
        {
            get
            {
                int res = 0;
                Object o = ViewState[STEP];
                if (o!=null)
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
            set { ViewState[STEP] = value; }
        }
        private int ProductTemplateId
        {
            get
            {
                int res = 0;
                Object o = ViewState[PRODUCTTEMPLATEID];
                if (o != null)
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
            set { ViewState[PRODUCTTEMPLATEID] = value; }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsLoanStarAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            product = CurrentPage.GetObject(Constants.PRODUCTOBJECT) as Product;
            if (product == null)
            {
                product = new Product();
            }
            isNew = product.ID < 1;
            SetControls();
            if (!IsPostBack)
            {
                InitControls();
                BindData();
            }
        }
        private void SetControls()
        {
            if(!isNew || (Step>0))
            {
                ddlAmortizationType.Attributes.Add(ONCHANGEATTRIBUTE, String.Format(JSSHOWROW, (int)Product.AmortizationType.Other, trAmortizationOther.ClientID));
                ddlArmType.Attributes.Add(ONCHANGEATTRIBUTE, String.Format(JSSHOWROW, (int)Product.ArmType.Other, trArmType.ClientID));
                ddlType.Attributes.Add(ONCHANGEATTRIBUTE, String.Format(JSSHOWROW1, (int)Product.ProductType.Other, trProductType.ClientID + "," + trOtherDescription.ClientID, (int)Product.ProductType.HECM, trSectionOfTheAct.ClientID));

                ddlProductRateLockMethod.Attributes.Add(ONCHANGEATTRIBUTE, String.Format(JSCBSHOWROWRATELOCK, trDaysToLock.ClientID, trFixedDaysToLock.ClientID, rfvDaysToLock.ClientID, rfFixedRateLockDays.ClientID));
                ddlProductRateUpdateInterval.Attributes.Add(ONCHANGEATTRIBUTE, String.Format(JSCBSHOWROWINPUTMETHOD, trProductRateInputMethod.ClientID));

                cbSpecialFeature.Attributes.Add(ONCLICKATTRIBUTE, String.Format(JSCBSHOWROW, trSpecialFeature.ClientID));
            }
            else
            {
                ddlProductTemplate.Attributes.Add(ONCHANGEATTRIBUTE, String.Format(JSENABLEBUTTON, btnContinue.ClientID));
            }
            cbUseBaydocsAppPackagesYN.Attributes.Add(ONCLICKATTRIBUTE, String.Format(JSCBBAYDOCSHOWROW, trBaydocAppPack.ClientID,trBaydocClosingPack.ClientID));
        }

        private void InitControls()
        {
            if(isNew&&Step==0)
            {
                divSelectTemplate.Visible = true;
                divMain.Visible = false;
                ddlProductTemplate.DataSource = Product.GetProductList(true);
                ddlProductTemplate.DataBind();
                ddlProductTemplate.Items.Insert(0,new ListItem("- Select -",0.ToString()));
            }
            else
            {
                divMain.Visible = true;
                divSelectTemplate.Visible = false;
                PanelHECM.Visible = true;
                PanelAll.Visible = true;
                ddlRateRoundingMethod.DataSource = Product.GetProductRateRoundingMethodList();
                ddlRateRoundingMethod.DataBind();
                ddlBasis.DataSource = Product.GetProductBasisList();
                ddlBasis.DataBind();
                ddlProductRateLockMethod.DataSource = Product.GetProductRateLockMethodList();
                ddlProductRateLockMethod.DataTextField = "name";
                ddlProductRateLockMethod.DataValueField = "id";
                ddlProductRateLockMethod.DataBind();
                ddlProductRateUpdateInterval.DataSource = Product.GetProductRateUpdateIntervalList();
                ddlProductRateUpdateInterval.DataBind();
                ddlProductRateInputMethod.DataSource = Product.GetProductRateInputMethodList();
                ddlProductRateInputMethod.DataBind();
            }
        }
        private void BindData()
        {
            if (isNew && (Step == 0)) return;

            lblHeader.Text = isNew?ADDHEADERTEXT:String.Format(EDITHEADERTEXT, product.Name);
            tbName.Text = product.Name;

            tbMargin.Text = product.Margin.ToString();
            if (product.EndOfMonthFlag)
                rbEndOfMonthFlag.Items[0].Selected = true;
            else
                rbEndOfMonthFlag.Items[1].Selected = true;
            tbRelativeRateCap.Text = product.RelativeRateCap.ToString();
            if (product.SharedAppreciation)
                chkSharedAppreciation.Checked = true;
            else
                chkSharedAppreciation.Checked = false;
            
            tbRateRoundingPrecision.Text = product.RateRoundingPrecision.ToString();
            ddlRateRoundingMethod.SelectedValue = product.RateRoundingMethodId.ToString();
            tbPaymentsPerYear.Text = product.PaymentsPerYear.ToString();
            ddlBasis.SelectedValue = product.BasisId.ToString();
            tbPropertyAppreciation.Text = product.PropertyAppreciation.ToString();
            tbUpfrontMortgageInsuranceRate.Text = product.UpfrontMortgageInsuranceRate.ToString();
            tbRenewalMortgageInsuranceRate.Text = product.RenewalMortgageInsuranceRate.ToString();
            cbHECMGuidesCollectingCounsCert.Checked = product.HECMGuidesCollectingCounsCert;
            cbHECMGuidesReviewingCounsCert.Checked = product.HECMGuidesReviewingCounsCert;
            cbHECMGuidesCollectingFHACase.Checked = product.HECMGuidesCollectingFHACase;
            cbHECMGuidesReviewingFHACase.Checked = product.HECMGuidesReviewingFHACase;
            cbHECMGuidesCollectingFHACondoApproval.Checked = product.HECMGuidesCollectingFHACondoApproval;
            cbHECMGuidesReviewingFHACondoApproval.Checked = product.HECMGuidesReviewingFHACondoApproval;
            cbHECMGuidesCollectingAppraisal.Checked = product.HECMGuidesCollectingAppraisal;
            cbHECMGuidesCollectingTermiteInspection.Checked = product.HECMGuidesCollectingTermiteInspection;
            cbHECMGuidesReviewingAppraisal.Checked = product.HECMGuidesReviewingAppraisal;
            cbHECMGuidesReviewingTermiteInspections.Checked = product.HECMGuidesReviewingTermiteInspections;
            cbHECMGuidesCollectingContractorBids.Checked = product.HECMGuidesCollectingContractorBids;
            cbHECMGuidesReviewingContractorBids.Checked = product.HECMGuidesReviewingContractorBids;
            cbHECMGuidesCollectingStructuralInspections.Checked = product.HECMGuidesCollectingStructuralInspections;
            cbHECMGuidesCollectingWaterTests.Checked = product.HECMGuidesCollectingWaterTests;
            cbHECMGuidesReviewingWaterTests.Checked = product.HECMGuidesReviewingWaterTests;
            cbHECMGuidesCollectingSepticInspections.Checked = product.HECMGuidesCollectingSepticInspections;
            cbHECMGuidesReviewingSepticInspections.Checked = product.HECMGuidesReviewingSepticInspections;
            cbHECMGuidesCollectingOilTankInspections.Checked = product.HECMGuidesCollectingOilTankInspections;
            cbHECMGuidesReviewingOilTankInspections.Checked = product.HECMGuidesReviewingOilTankInspections;
            cbHECMGuidesCollectingRoofInspections.Checked = product.HECMGuidesCollectingRoofInspections;
            cbHECMGuidesReviewingRoofInspections.Checked = product.HECMGuidesReviewingRoofInspections;
            cbHECMGuidesCollectingPOAandConservator.Checked = product.HECMGuidesCollectingPOAandConservator;
            cbHECMGuidesReviewingPOA.Checked = product.HECMGuidesReviewingPOA;
            cbHECMGuidesReviewingConservator.Checked = product.HECMGuidesReviewingConservator;
            cbHECMGuidesCollectingCreditReports.Checked = product.HECMGuidesCollectingCreditReports;
            cbHECMGuidesReviewingCreditReports.Checked = product.HECMGuidesReviewingCreditReports;
            cbHECMGuidesCollectingLDP_GSAPrintout.Checked = product.HECMGuidesCollectingLDP_GSAPrintout;
            cbHECMGuidesReviewingLDP_GSAPrintout.Checked = product.HECMGuidesReviewingLDP_GSAPrintout;
            cbHECMGuidesCollectingCAIVRSAuthPrintout.Checked = product.HECMGuidesCollectingCAIVRSAuthPrintout;
            cbHECMGuidesReviewingCAIVRSAuthPrintout.Checked = product.HECMGuidesReviewingCAIVRSAuthPrintout;
            cbHECMGuidesCollectingTrusts.Checked = product.HECMGuidesCollectingTrusts;
            cbHECMGuidesReviewingTrusts.Checked = product.HECMGuidesReviewingTrusts;
            ddlProductRateLockMethod.SelectedValue = product.ProductRateLockMethodId.ToString();
            if (product.DaysToLock==0)
            {
                tbDaysToLock.Text = String.Empty;
            }
            else
            {
                tbDaysToLock.Text = product.DaysToLock.ToString();
            }
            if (product.ExpectedFloorRate==0)
            {
                tbExpectedFloorRate.Text = "";
            }
            else
            {
                tbExpectedFloorRate.Value = product.ExpectedFloorRate;
            }
            if (product.FixedRateLockDays==0)
            {
                tbFixedRateLockDays.Text = String.Empty;
            }
            else
            {
                tbFixedRateLockDays.Text = product.FixedRateLockDays.ToString();
            }
            ddlAmortizationType.DataSource = Product.GetProductAmortizationTypeList();
            ddlAmortizationType.DataTextField = "Name";
            ddlAmortizationType.DataValueField = "id";
            ddlAmortizationType.DataBind();
            ddlAmortizationType.SelectedValue = product.AmortizationTypeId.ToString();
            ddlArmType.DataSource = Product.GetProductArmTypeList();
            ddlArmType.DataTextField = "Name";
            ddlArmType.DataValueField = "id";
            ddlArmType.DataBind();
            ddlArmType.SelectedValue = product.ArmTypeId.ToString();
            ddlType.DataSource = Product.GetProductTypeList();
            ddlType.DataTextField = "Name";
            ddlType.DataValueField = "id";
            ddlType.DataBind();
            ddlType.SelectedValue = product.TypeId.ToString();
            tbAmortizationOther.Text = product.AmortizationTypeOther;
            tbArmTypeOther.Text = product.ArmTypeOther;
            tbTypeOther.Text = product.TypeOther;
            tbOtherDescription.Text = product.OtherDescription;
            cbSpecialFeature.Checked = product.OtherSpecialLoanFeatureAllowed;
            tbSpecialFeature.Text = product.OtherSpecialLoanFeatureDescription;
            ddlRateChangeDay.DataSource = Product.GetDaysList();
            ddlRateChangeDay.DataTextField = "name";
            ddlRateChangeDay.DataValueField = "id";
            ddlRateChangeDay.DataBind();
            ddlRateChangeDay.SelectedValue = product.RateChangeDayId.ToString();
            cbPrimaryResReq.Checked = product.PrimaryResidenceRequired;
            cballowPrimaryRes.Checked = product.AllowPrimaryRes;
            cballowSecondHome.Checked = product.AllowSecondHome;
            cballowInvestmentProp.Checked = product.AllowInvestmentProp;
            cbappraisalRequired.Checked = product.AppraisalRequired;
            cbpropInspRequired.Checked = product.PropInspRequired;
            cbuseStandardFloodGuides.Checked = product.UseStandardFloodGuides;
            tbmaxFloodDeductible.Text = product.MaxFloodDeductible.ToString();
            cbuseStandardHazardGuides.Checked = product.UseStandardHazardGuides;
            tbmaxHazDeductPercent.Text = product.MaxHazDeductPercent.ToString();
            cballowHECMRefi.Checked = product.AllowHECMRefi;
            cballowMultiFamilyProp.Checked = product.AllowMultiFamilyProp;
            cbuseStandardGuidesWellSeptic.Checked = product.UseStandardGuidesWellSeptic;
            cbuseStndGuidesCashToClose.Checked = product.UseStndGuidesCashToClose;
            tbdaysAdvancePayTax.Text = product.DaysAdvancePayTax.ToString();
            cbuseStandardTrustGuides.Checked = product.UseStandardTrustGuides;
            cballowManuHomes.Checked = product.AllowManuHomes;
            cbCreditReportRequired.Checked = product.CreditReportRequiredYN;
            ddlProductIndex.DataSource = Product.GetProductIndex();
            ddlProductIndex.DataTextField = "Name";
            ddlProductIndex.DataValueField = "id";
            ddlProductIndex.DataBind();
            ddlProductIndex.SelectedValue = product.ProductIndexId.ToString();
            BindBaydocsCodes();
            cbBasicGuidesCollectingDeathCerts.Checked = product.BasicGuidesCollectingDeathCerts;
            cbBasicGuidesReviewingDeathCerts.Checked = product.BasicGuidesReviewingDeathCerts;
            cbBasicGuidesCollectingUSPS.Checked = product.BasicGuidesCollectingUSPS;
            cbBasicGuidesReviewingUSPS.Checked = product.BasicGuidesReviewingUSPS;
            cbBasicGuidesCollectingTitle.Checked = product.BasicGuidesCollectingTitle;
            cbBasicGuidesReviewingTitle.Checked = product.BasicGuidesReviewingTitle;
            cbBasicGuidesCollectingFloodCertificates.Checked = product.BasicGuidesCollectingFloodCertificates;
            cbBasicGuidesReviewingFloodCertificates.Checked = product.BasicGuidesReviewingFloodCertificates;
            cbBasicGuidesCollectingHazardDecPages.Checked = product.BasicGuidesCollectingHazardDecPages;
            cbBasicGuidesReviewingHazardDecPages.Checked = product.BasicGuidesReviewingHazardDecPages;
            cbBasicGuidesCollectingFloodDecPages.Checked = product.BasicGuidesCollectingFloodDecPages;
            cbBasicGuidesReviewingFloodDecPages.Checked = product.BasicGuidesReviewingFloodDecPages;
            cbBasicGuidesCollectingProofOfAge.Checked = product.BasicGuidesCollectingProofOfAge;
            cbBasicGuidesReviewingProofOfAge.Checked = product.BasicGuidesReviewingProofOfAge;
            cbBasicGuidesCollectingSSN.Checked = product.BasicGuidesCollectingSSN;
            cbBasicGuidesReviewingSSN.Checked = product.BasicGuidesReviewingSSN;
            cbEquityProtection.Checked = product.EquityProtection;
            tbCounsActiveDays.Value = product.CounsActiveDays;
            tbTitleActiveDays.Value = product.TitleActiveDays;
            tbAppraisalActiveDays.Value = product.AppraisalActiveDays;
            tbPestActiveDays.Value = product.PestActiveDays;
            tbBidActiveDays.Value = product.BidActiveDays;
            tbWaterTestActiveDays.Value = product.WaterTestActiveDays;
            tbSepticInspActiveDays.Value = product.SepticInspActiveDays;
            tbOilTankInspActiveDays.Value = product.OilTankInspActiveDays;
            tbRoofInspActiveDays.Value = product.RoofInspActiveDays;
            tbFloodCertActiveDays.Value = product.FloodCertActiveDays;
            tbCreditReportActiveDays.Value = product.CreditReportActiveDays;
            tbLDPActiveDays.Value = product.LDPActiveDays;
            tbEPLSActiveDays.Value = product.EPLSActiveDays;
            tbCaivrsActiveDays.Value = product.CaivrsActiveDays;
            cbFollowStandardNBSGuides.Checked = product.FollowStandardNBSGuides;
            cbHECMGuidesCollectingLeases.Checked = product.HECMGuidesCollectingLeases;
            cbAgeEligRequirementApply.Checked = product.AgeEligRequirementApply;
            cbAgeEligRequirementClose.Checked = product.AgeEligRequirementClose;
            tbMinAgeToApply.Value = product.MinAgeToApply;
            tbMinAgeToClose.Value = product.MinAgeToClose;
            cbBasicGuidesCollectingHOAHazardDecPages.Checked = product.BasicGuidesCollectingHOAHazardDecPages;
            cbBasicGuidesReviewingHOAHazardDecPages.Checked = product.BasicGuidesReviewingHOAHazardDecPages;
            cbBasicGuidesCollectingMasterFloodDecPages.Checked = product.BasicGuidesCollectingMasterFloodDecPages;
            cbUseSRPLocksYN.Checked = product.UseSRPLocksYN;
            cbHECMGuidesReviewingLeases.Checked = product.HECMGuidesReviewingLeases;
            cbHECMGuidesCollectingConservator.Checked = product.HECMGuidesCollectingConservator;
            cbBasicGuidesReviewingHOAFloodDecPages.Checked = product.BasicGuidesReviewingHOAFloodDecPages;
            cbHECMGuidesReviewingStructuralInspections.Checked = product.HECMGuidesReviewingStructuralInspections;
            tbLendingLimit.Value = (double)product.LendingLimit;
            tbSectionOfTheAct.Text = product.SectionOfTheAct;
            cbHECMGuidesCollectingPOA.Checked = product.HECMGuidesCollectingPOA;
            cbAllowEscrowTaxAndInsurance.Checked = product.AllowEscrowTaxAndInsurance;
            cbAllowEscrowRepiars.Checked = product.AllowEscrowRepiars;
            ddlProductRateUpdateInterval.SelectedValue = product.ProductRateUpdateIntervalId.ToString();
            ddlProductRateInputMethod.SelectedValue = product.ProductRateInputMethodId.ToString();
            cbUseBaydocsAppPackagesYN.Checked = product.UseBaydocsAppPackagesYN;
            SetOtherVisibility();
            BindServiceFeeGrid();
        }
        private void BindBaydocsCodes()
        {
            ddlBaydocsAppPackageCode.DataSource = Product.GetBaydocAppPackageTypeList();
            ddlBaydocsAppPackageCode.DataTextField = "name";
            ddlBaydocsAppPackageCode.DataValueField = "id";
            ddlBaydocsAppPackageCode.DataBind();
            ddlBaydocsAppPackageCode.Items.Insert(0,new ListItem("-Select","0"));
            ddlBaydocsAppPackageCode.SelectedValue = product.BaydocsAppPackageCodeId.ToString();
            ddlBaydocsClosingPackageCode.DataSource = Product.GetBaydocClosingPackageTypeList();
            ddlBaydocsClosingPackageCode.DataTextField = "name";
            ddlBaydocsClosingPackageCode.DataValueField = "id";
            ddlBaydocsClosingPackageCode.DataBind();
            ddlBaydocsClosingPackageCode.Items.Insert(0, new ListItem("-Select", "0"));
            ddlBaydocsClosingPackageCode.SelectedValue = product.BaydocsClosingPackageCodeId.ToString();
        }
        private void BindServiceFeeGrid()
        {
            DataView dv = ServiceFee.GetServiceFee(CurrentUser.EffectiveCompanyId);
            dv.RowFilter = String.Format(PRODUCTFILTER, product.ID);
            G.DataSource = dv;
            G.DataBind();
        }
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            if ((e.CommandName == RadGrid.PerformInsertCommandName) || (e.CommandName == RadGrid.UpdateCommandName))
            {
                if (e.CommandName == RadGrid.UpdateCommandName)
                {
                    serviceFee = new ServiceFee(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString()));
                }
                else
                {
                    serviceFee = new ServiceFee(0);
                }
                
                serviceFee.ProductId = product.ID;
                serviceFee.CompanyId = CurrentUser.EffectiveCompanyId;
                RadNumericTextBox tbFee = e.Item.FindControl("tbfee") as RadNumericTextBox;
                if (tbFee != null)
                {
                    serviceFee.Fee = Convert.ToDecimal(tbFee.Value);
                }
                CheckBox cb = e.Item.FindControl("cbDefaultEdit") as CheckBox;
                if (cb != null)
                {
                    serviceFee.IsDefault = cb.Checked;
                }
                int res = serviceFee.Save();
                if (res > 0)
                {
                    serviceFee.ID = res;
                }
            }
            else if (e.CommandName == RadGrid.DeleteCommandName)
            {
                ServiceFee.Delete(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString()));
            }
            BindServiceFeeGrid();
        }
        protected void G_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                DataRowView dr = e.Item.DataItem as DataRowView;
                if (dr != null)
                {
                    ImageButton btn = e.Item.Controls[5].Controls[0] as ImageButton;
                    if (btn != null)
                    {
                        btn.Visible = Convert.ToBoolean(dr["IsDefault"].ToString()) ? false : true; 
                    }
                }
            }
        }
        protected void G_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                if (editedItem != null)
                {
                    RadNumericTextBox tbFee = e.Item.FindControl("tbfee") as RadNumericTextBox;
                    if (tbFee != null)
                    {
                        DataRowView dr = editedItem.DataItem as DataRowView;
                        if (dr != null)
                        {
                            tbFee.Value = Convert.ToDouble(dr["Fee"].ToString());
                            CheckBox cb = e.Item.FindControl("cbDefaultEdit") as CheckBox;
                            if (cb != null)
                            {
                                cb.Checked = Convert.ToBoolean(dr["IsDefault"].ToString());
                                cb.Enabled = !cb.Checked;
                            }
                        }
                    }
                }
            }
        }

        private void SetOtherVisibility()
        {
            if (product.AmortizationTypeId == (int)Product.AmortizationType.Other)
            {
                trAmortizationOther.Attributes.Add(STYLEATTRIBUTE, TRVISIBLE);
            }
            else
            {
                trAmortizationOther.Attributes.Add(STYLEATTRIBUTE, TRHIDDEN);
            }
            if (product.ArmTypeId == (int)Product.ArmType.Other)
            {
                trArmType.Attributes.Add(STYLEATTRIBUTE, TRVISIBLE);
            }
            else
            {
                trArmType.Attributes.Add(STYLEATTRIBUTE, TRHIDDEN);
            }
            if (product.TypeId == (int)Product.ProductType.Other)
            {
                trProductType.Attributes.Add(STYLEATTRIBUTE, TRVISIBLE);
                trOtherDescription.Attributes.Add(STYLEATTRIBUTE, TRVISIBLE);
            }
            else
            {
                trProductType.Attributes.Add(STYLEATTRIBUTE, TRHIDDEN);
                trOtherDescription.Attributes.Add(STYLEATTRIBUTE, TRHIDDEN);
            }
            if (product.TypeId == (int)Product.ProductType.HECM)
            {
                trSectionOfTheAct.Attributes.Add(STYLEATTRIBUTE, TRVISIBLE);
            }
            else
            {
                trSectionOfTheAct.Attributes.Add(STYLEATTRIBUTE, TRHIDDEN);
            }
            if(product.OtherSpecialLoanFeatureAllowed)
            {
                trSpecialFeature.Attributes.Add(STYLEATTRIBUTE, TRVISIBLE);
            }
            else
            {
                trSpecialFeature.Attributes.Add(STYLEATTRIBUTE, TRHIDDEN);
            }
            if (product.ProductRateLockMethodId == (int)Product.ProductRateLockMethod.PrincipalLimetProtection)
            {
                trDaysToLock.Attributes.Add(STYLEATTRIBUTE, TRVISIBLE);
            }
            else
            {
                trDaysToLock.Attributes.Add(STYLEATTRIBUTE, TRHIDDEN);
                rfvDaysToLock.Enabled = false;
            }
            if (product.ProductRateLockMethodId == (int)Product.ProductRateLockMethod.RateLock)
            {
                trFixedDaysToLock.Attributes.Add(STYLEATTRIBUTE, TRVISIBLE);
            }
            else
            {
                trFixedDaysToLock.Attributes.Add(STYLEATTRIBUTE, TRHIDDEN);
                rfFixedRateLockDays.Enabled = false;
            }
            if(product.ProductRateUpdateIntervalId == (int)Product.ProductRateUpdateInterval.WeeklyOnTuesday)
            {
                trProductRateInputMethod.Attributes.Add(STYLEATTRIBUTE, TRVISIBLE);
            }
            else
            {
                trProductRateInputMethod.Attributes.Add(STYLEATTRIBUTE, TRHIDDEN);
            }
            if(product.UseBaydocsAppPackagesYN)
            {
                trBaydocAppPack.Attributes.Add(STYLEATTRIBUTE, TRVISIBLE);
                trBaydocClosingPack.Attributes.Add(STYLEATTRIBUTE, TRVISIBLE);
            }
            else
            {
                trBaydocAppPack.Attributes.Add(STYLEATTRIBUTE, TRHIDDEN);
                trBaydocClosingPack.Attributes.Add(STYLEATTRIBUTE, TRHIDDEN);
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            goBack();
        }
        protected void btnContinue_Click(object sender, EventArgs e)
        {
            Step = 1;
            product = new Product(int.Parse(ddlProductTemplate.SelectedValue));
            ProductTemplateId = product.ID;
            product.ID = -1;
            product.Name = "";
            CurrentPage.StoreObject(product, Constants.PRODUCTOBJECT);
            SetControls();
            InitControls();
            BindData();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            goBack();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            product.Name = tbName.Text;
            product.Margin = Convert.ToDouble(tbMargin.Text);
            product.EndOfMonthFlag = rbEndOfMonthFlag.Items[0].Selected;
            product.RelativeRateCap = Convert.ToDecimal(tbRelativeRateCap.Text);
            product.SharedAppreciation = chkSharedAppreciation.Checked;
            product.RateRoundingPrecision = Convert.ToDecimal(tbRateRoundingPrecision.Text);
            product.RateRoundingMethodId  = Convert.ToInt16(ddlRateRoundingMethod.SelectedValue);
            product.PaymentsPerYear = Convert.ToInt16(tbPaymentsPerYear.Text);
            product.BasisId = Convert.ToInt16(ddlBasis.SelectedValue);
            product.PropertyAppreciation = Convert.ToDecimal(tbPropertyAppreciation.Text);
            product.UpfrontMortgageInsuranceRate = Convert.ToDecimal(tbUpfrontMortgageInsuranceRate.Text);
            product.RenewalMortgageInsuranceRate = Convert.ToDecimal(tbRenewalMortgageInsuranceRate.Text);
            product.HECMGuidesCollectingCounsCert = cbHECMGuidesCollectingCounsCert.Checked;
            product.HECMGuidesReviewingCounsCert = cbHECMGuidesReviewingCounsCert.Checked;
            product.HECMGuidesCollectingFHACase = cbHECMGuidesCollectingFHACase.Checked;
            product.HECMGuidesReviewingFHACase = cbHECMGuidesReviewingFHACase.Checked;
            product.HECMGuidesCollectingFHACondoApproval = cbHECMGuidesCollectingFHACondoApproval.Checked;
            product.HECMGuidesReviewingFHACondoApproval = cbHECMGuidesReviewingFHACondoApproval.Checked;
            product.HECMGuidesCollectingAppraisal = cbHECMGuidesCollectingAppraisal.Checked;
            product.HECMGuidesCollectingTermiteInspection = cbHECMGuidesCollectingTermiteInspection.Checked;
            product.HECMGuidesReviewingAppraisal = cbHECMGuidesReviewingAppraisal.Checked;
            product.HECMGuidesReviewingTermiteInspections = cbHECMGuidesReviewingTermiteInspections.Checked;
            product.HECMGuidesCollectingContractorBids = cbHECMGuidesCollectingContractorBids.Checked;
            product.HECMGuidesReviewingContractorBids = cbHECMGuidesReviewingContractorBids.Checked;
            product.HECMGuidesCollectingStructuralInspections = cbHECMGuidesCollectingStructuralInspections.Checked;
            product.HECMGuidesCollectingWaterTests = cbHECMGuidesCollectingWaterTests.Checked;
            product.HECMGuidesReviewingWaterTests = cbHECMGuidesReviewingWaterTests.Checked;
            product.HECMGuidesCollectingSepticInspections = cbHECMGuidesCollectingSepticInspections.Checked;
            product.HECMGuidesReviewingSepticInspections = cbHECMGuidesReviewingSepticInspections.Checked;
            product.HECMGuidesCollectingOilTankInspections = cbHECMGuidesCollectingOilTankInspections.Checked;
            product.HECMGuidesReviewingOilTankInspections = cbHECMGuidesReviewingOilTankInspections.Checked;
            product.HECMGuidesCollectingRoofInspections = cbHECMGuidesCollectingRoofInspections.Checked;
            product.HECMGuidesReviewingRoofInspections = cbHECMGuidesReviewingRoofInspections.Checked;
            product.HECMGuidesCollectingPOAandConservator = cbHECMGuidesCollectingPOAandConservator.Checked;
            product.HECMGuidesReviewingPOA = cbHECMGuidesReviewingPOA.Checked;
            product.HECMGuidesReviewingConservator = cbHECMGuidesReviewingConservator.Checked;
            product.HECMGuidesCollectingCreditReports = cbHECMGuidesCollectingCreditReports.Checked;
            product.HECMGuidesReviewingCreditReports = cbHECMGuidesReviewingCreditReports.Checked;
            product.HECMGuidesCollectingLDP_GSAPrintout = cbHECMGuidesCollectingLDP_GSAPrintout.Checked;
            product.HECMGuidesReviewingLDP_GSAPrintout = cbHECMGuidesReviewingLDP_GSAPrintout.Checked;
            product.HECMGuidesCollectingCAIVRSAuthPrintout = cbHECMGuidesCollectingCAIVRSAuthPrintout.Checked;
            product.HECMGuidesReviewingCAIVRSAuthPrintout = cbHECMGuidesReviewingCAIVRSAuthPrintout.Checked;
            product.HECMGuidesCollectingTrusts = cbHECMGuidesCollectingTrusts.Checked;
            product.HECMGuidesReviewingTrusts = cbHECMGuidesReviewingTrusts.Checked;
            product.AmortizationTypeId = int.Parse(ddlAmortizationType.SelectedValue);
            if (product.AmortizationTypeId == (int)Product.AmortizationType.Other)
            {
                product.AmortizationTypeOther = tbAmortizationOther.Text;
            }
            product.ArmTypeId = int.Parse(ddlArmType.SelectedValue);
            if (product.ArmTypeId == (int)Product.ArmType.Other)
            {
                product.ArmTypeOther = tbArmTypeOther.Text;
            }
            product.TypeId = int.Parse(ddlType.SelectedValue);
            if(product.TypeId == (int)Product.ProductType.Other)
            {
                product.TypeOther = tbTypeOther.Text;
                product.OtherDescription = tbOtherDescription.Text;
            }
            product.OtherSpecialLoanFeatureAllowed = cbSpecialFeature.Checked;
            if(product.OtherSpecialLoanFeatureAllowed)
            {
                product.OtherSpecialLoanFeatureDescription = tbSpecialFeature.Text;
            }
            product.RateChangeDayId = int.Parse(ddlRateChangeDay.SelectedValue);
            product.PrimaryResidenceRequired = cbPrimaryResReq.Checked;
            product.AllowPrimaryRes = cballowPrimaryRes.Checked;
            product.AllowSecondHome = cballowSecondHome.Checked;
            product.AllowInvestmentProp = cballowInvestmentProp.Checked;
            product.AppraisalRequired = cbappraisalRequired.Checked;
            product.PropInspRequired = cbpropInspRequired.Checked;
            product.UseStandardFloodGuides = cbuseStandardFloodGuides.Checked;
            product.MaxFloodDeductible = Convert.ToDecimal(tbmaxFloodDeductible.Text);
            product.UseStandardHazardGuides = cbuseStandardHazardGuides.Checked;
            product.MaxHazDeductPercent = Convert.ToDecimal(tbmaxHazDeductPercent.Text);
            product.AllowHECMRefi = cballowHECMRefi.Checked;
            product.AllowMultiFamilyProp = cballowMultiFamilyProp.Checked;
            product.UseStandardGuidesWellSeptic = cbuseStandardGuidesWellSeptic.Checked;
            product.UseStndGuidesCashToClose = cbuseStndGuidesCashToClose.Checked;
            product.DaysAdvancePayTax =  int.Parse(tbdaysAdvancePayTax.Text);
            product.UseStandardTrustGuides = cbuseStandardTrustGuides.Checked;
            product.AllowManuHomes = cballowManuHomes.Checked;
            product.CreditReportRequiredYN = cbCreditReportRequired.Checked;
            product.ProductIndexId = int.Parse(ddlProductIndex.SelectedValue);

            product.BasicGuidesCollectingDeathCerts = cbBasicGuidesCollectingDeathCerts.Checked;
            product.BasicGuidesReviewingDeathCerts = cbBasicGuidesReviewingDeathCerts.Checked;
            product.BasicGuidesCollectingUSPS = cbBasicGuidesCollectingUSPS.Checked;
            product.BasicGuidesReviewingUSPS = cbBasicGuidesReviewingUSPS.Checked;
            product.BasicGuidesCollectingTitle = cbBasicGuidesCollectingTitle.Checked;
            product.BasicGuidesReviewingTitle = cbBasicGuidesReviewingTitle.Checked;
            product.BasicGuidesCollectingFloodCertificates = cbBasicGuidesCollectingFloodCertificates.Checked;
            product.BasicGuidesReviewingFloodCertificates = cbBasicGuidesReviewingFloodCertificates.Checked;
            product.BasicGuidesCollectingHazardDecPages = cbBasicGuidesCollectingHazardDecPages.Checked;
            product.BasicGuidesReviewingHazardDecPages = cbBasicGuidesReviewingHazardDecPages.Checked;
            product.BasicGuidesCollectingFloodDecPages = cbBasicGuidesCollectingFloodDecPages.Checked;
            product.BasicGuidesReviewingFloodDecPages = cbBasicGuidesReviewingFloodDecPages.Checked;
            product.BasicGuidesCollectingProofOfAge = cbBasicGuidesCollectingProofOfAge.Checked;
            product.BasicGuidesReviewingProofOfAge = cbBasicGuidesReviewingProofOfAge.Checked;
            product.BasicGuidesCollectingSSN = cbBasicGuidesCollectingSSN.Checked;
            product.BasicGuidesReviewingSSN = cbBasicGuidesReviewingSSN.Checked;
            product.BaydocsAppPackageCodeId = int.Parse(ddlBaydocsAppPackageCode.SelectedValue);
            product.BaydocsClosingPackageCodeId = int.Parse(ddlBaydocsClosingPackageCode.SelectedValue);
            product.EquityProtection = cbEquityProtection.Checked;
            product.ProductRateLockMethodId = int.Parse(ddlProductRateLockMethod.SelectedValue);
            int daysToLock = 0;
            if(!String.IsNullOrEmpty(tbDaysToLock.Text))
            {
                try
                {
                    daysToLock = int.Parse(tbDaysToLock.Text);
                }
                catch
                {
                }
            }
            product.DaysToLock = daysToLock;
            int fixedDaysToLock = 0;
            if (!String.IsNullOrEmpty(tbFixedRateLockDays.Text))
            {
                try
                {
                    fixedDaysToLock = int.Parse(tbFixedRateLockDays.Text);
                }
                catch
                {
                }
            }
            product.FixedRateLockDays = fixedDaysToLock;

            double expectedFloorRate = 0;
            if(!String.IsNullOrEmpty(tbExpectedFloorRate.Text))
            {
                try
                {
                    expectedFloorRate = double.Parse(tbExpectedFloorRate.Text);
                }
                catch
                {
                }
            }
            product.ExpectedFloorRate = expectedFloorRate;

            if (tbCounsActiveDays.Value!= null)
            {
                product.CounsActiveDays = int.Parse(tbCounsActiveDays.Text);
            }
            if (tbTitleActiveDays.Value != null)
            {
                product.TitleActiveDays = int.Parse(tbTitleActiveDays.Text);
            }
            if (tbAppraisalActiveDays.Value != null)
            {
                product.AppraisalActiveDays = int.Parse(tbAppraisalActiveDays.Text);
            }
            if (tbPestActiveDays.Value != null)
            {
                product.PestActiveDays = int.Parse(tbPestActiveDays.Text);
            }
            if (tbBidActiveDays.Value != null)
            {
                product.BidActiveDays = int.Parse(tbBidActiveDays.Text);
            }
            if (tbWaterTestActiveDays.Value != null)
            {
                product.WaterTestActiveDays = int.Parse(tbWaterTestActiveDays.Text);
            }
            if (tbSepticInspActiveDays.Value != null)
            {
                product.SepticInspActiveDays = int.Parse(tbSepticInspActiveDays.Text);
            }
            if (tbOilTankInspActiveDays.Value != null)
            {
                product.OilTankInspActiveDays = int.Parse(tbOilTankInspActiveDays.Text);
            }
            if (tbRoofInspActiveDays.Value != null)
            {
                product.RoofInspActiveDays = int.Parse(tbRoofInspActiveDays.Text);
            }
            if (tbFloodCertActiveDays.Value != null)
            {
                product.FloodCertActiveDays = int.Parse(tbFloodCertActiveDays.Text);
            }
            if (tbCreditReportActiveDays.Value != null)
            {
                product.CreditReportActiveDays = int.Parse(tbCreditReportActiveDays.Text);
            }
            if (tbLDPActiveDays.Value != null)
            {
                product.LDPActiveDays = int.Parse(tbLDPActiveDays.Text);
            }
            if (tbEPLSActiveDays.Value != null)
            {
                product.EPLSActiveDays = int.Parse(tbEPLSActiveDays.Text);
            }
            if (tbCaivrsActiveDays.Value != null)
            {
                product.CaivrsActiveDays = int.Parse(tbCaivrsActiveDays.Text);
            }
            product.FollowStandardNBSGuides = cbFollowStandardNBSGuides.Checked;
            product.HECMGuidesCollectingLeases = cbHECMGuidesCollectingLeases.Checked;
            product.AgeEligRequirementApply = cbAgeEligRequirementApply.Checked;
            product.AgeEligRequirementClose = cbAgeEligRequirementClose.Checked;
            if (tbMinAgeToApply.Value != null)
            {
                product.MinAgeToApply = int.Parse(tbMinAgeToApply.Text);
            }
            if (tbMinAgeToClose.Value != null)
            {
                product.MinAgeToClose = int.Parse(tbMinAgeToClose.Text);
            }
            product.BasicGuidesCollectingHOAHazardDecPages = cbBasicGuidesCollectingHOAHazardDecPages.Checked;
            product.BasicGuidesReviewingHOAHazardDecPages = cbBasicGuidesReviewingHOAHazardDecPages.Checked;
            product.BasicGuidesCollectingMasterFloodDecPages = cbBasicGuidesCollectingMasterFloodDecPages.Checked;
            product.UseSRPLocksYN = cbUseSRPLocksYN.Checked;
            product.HECMGuidesReviewingLeases = cbHECMGuidesReviewingLeases.Checked;
            product.HECMGuidesCollectingConservator = cbHECMGuidesCollectingConservator.Checked;
            product.BasicGuidesReviewingHOAFloodDecPages = cbBasicGuidesReviewingHOAFloodDecPages.Checked;
            product.HECMGuidesReviewingStructuralInspections = cbHECMGuidesReviewingStructuralInspections.Checked;
            product.LendingLimit = (decimal)tbLendingLimit.Value;
            product.SectionOfTheAct = tbSectionOfTheAct.Text;
            product.HECMGuidesCollectingPOA = cbHECMGuidesCollectingPOA.Checked;
            product.AllowEscrowTaxAndInsurance = cbAllowEscrowTaxAndInsurance.Checked;
            product.AllowEscrowRepiars = cbAllowEscrowRepiars.Checked;
            product.ProductRateUpdateIntervalId = int.Parse(ddlProductRateUpdateInterval.SelectedValue);
            if(product.ProductRateUpdateIntervalId == (int)Product.ProductRateUpdateInterval.WeeklyOnTuesday)
            {
                product.ProductRateInputMethodId = int.Parse(ddlProductRateInputMethod.SelectedValue);
            }
            product.UseBaydocsAppPackagesYN = cbUseBaydocsAppPackagesYN.Checked;
            int res = product.Save(ProductTemplateId);
            if (res > 0)
            {
                if(isNew)
                {
                    CurrentPage.StoreObject(product, Constants.PRODUCTOBJECT);
                    isNew = false;
                    BindData();
                }
                lblMessage.Text = Constants.SUCCESSMESSAGE;
                SetOtherVisibility();
            }
            else if (res==-1)
            {
                lblMessage.Text = ALREADYEXISTS;
            }
        }
        private void goBack()
        {
            Response.Redirect("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.VIEWPRODUCT);
        }

        protected void lbViewRates_Click(object sender, EventArgs e)
        {
            Response.Redirect(ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.VIEWRATE));
        }

    }
}