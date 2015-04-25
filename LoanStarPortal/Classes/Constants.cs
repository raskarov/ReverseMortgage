namespace LoanStar.Common
{
    /// <summary>
    /// Summary description for Constants
    /// </summary>
    public class Constants
    {
        #region Session objects
        public const string USEROBJECTNAME = "user";
        public const string VENDOROBJECTNAME = "vendor";
        public const string USEREDITOBJECTNAME = "useredit";
        public const string DOCTEMPLATEEDITOBJECTNAME = "doctemplateedit";
        public const string ROLEOBJECT = "role";
        public const string RULEOBJECT = "rule";
        public const string COMPANYOBJECT = "company";
        public const string PRODUCTOBJECT = "product";
        public const string FIELDOBJECT = "field";
        public const string MortgageID = "MortgageProfileID";
        public const string NEEDCHECKMAIL = "NeedCheckMail";
        public const string ADVANCEDCALCULATOR = "AdvancedCalculator";
        public const string ADVANCEDCALCULATORUNIQUEID = "AdvancedCalculatorUniqueID";
        public const string ADVANCEDCALCULATORERRMSG = "AdvancedCalculatorErrMsg";
        public const string EMAILSUNIQUEID = "EmailsUniqueID";
        public const string EMAILSUSERID = "EmailsUserID";
        public const string NEWMORTGAGECREATED = "newmortgagecreated";
        public const string LASTPIPELINERELOADTIME = "lastpipelinereloadtime";
        #endregion

        #region query string values
        public const string VIEWHUDFACTORS = "HUDFactors";
        public const string VIEWDOCTEMPLATE = "DocumentTemplateView";
        public const string CONTROLPARAM = "control";
        public const string IDPARAM = "id";
        public const string VIEWUSER = "userview";
        public const string EDITUSER = "useredit";
        public const string EDITVENDOR = "vendoredit";
        public const string VIEWFIELDDETAILS = "fielddetails";
        public const string VIEWCOMPANY = "companyview";
        public const string VIEWPRODUCT = "productview";
        public const string VIEWRATE = "rateview";
        public const string VIEWLIMIT = "limitview";
        public const string EDITPRODUCT = "productedit";
        public const string EDITCOMPANY = "companyedit";
        public const string EDITUSERORG = "userorgedit";
//        public const string EDITCOMPANYSTRUCTURE = "companystructureedit";
        public const string VIEWAFFILIATE = "affiliateview";
        public const string VIEWLENDERPRODUCT = "lenderproduct";
        public const string VIEWROLE = "roleview";
        public const string VIEWRULETREE = "ruletreeview";
        public const string VIEWHOLIDAYS = "holidayview";
        public const string VIEWFIELDS = "fieldsview";
        public const string VIEWFIELDSNEW = "fieldsviewnew";
        public const string VIEWGLOBALADMINFIELDS = "GlobalAdminFields";
        public const string VIEWSTATELICENSING = "statelicensing";
        public const string EDITROLE = "roleedit";
        public const string EDITFIELDS = "fieldedit";
        public const string EDITDOCTEMPLATE = "doctemplateedit";
        public const string EDITMAILSETTINGS = "MailSettingsEdit";
        public const string VIEWLINKS = "links";
        public const string VIEWMYLENDERS = "mylenders";
        public const string VIEWMYORIGINATORS = "myoriginators";
        public const string VIEWMYSERVICERS = "myservicers";
        public const string VIEWMYINVESTORS = "myinvestors";
        public const string VIEWMYFEETYPES = "myfeetypes";
        public const string IMPORTLEAD = "importlead";
        public const string COMPANYLOCATION = "location";
        public const string LEADCAMPAIGN = "leadcampaign";
        public const string LOGOUT = "logout";
        public const string CURRENTTOPFIRSTTABID = "currenttopfirsttabid";
        public const string CURRENTTOPSECONDTABINDEX = "currenttopsecondtabindex";
        public const string CURRENTBOTTOMTABID = "currentbottomtabid";
        public const string CURRENTCALCULATORTABID = "currentcalculatortabid";
        public const string VIEWREQUIREDFIELDS = "requiredfields";
        public const string VIEWVENDORS = "vendors";
        public const string GOTOCALCULATOR = "gotocalculator";
        public const string GOTOPAYOFFS = "gotopayoffs";
        public const int TOPSECONDTABINDEX = 0;
        public const string MESSAGEBOARDSHOWONLYNOTES = "MessageBoardShowOnlyNotes";
        public const string ISMAILCHECKED = "IsMailChecked";
        public const string CENTERRIGHTPANELUPDATENEEDED = "CenterRightPanelUpdateNeeded";
        public const string CENTERLEFTPANELUPDATENEEDED = "CenterLeftPanelUpdateNeeded";
        public const string RETAILSITEMORTGAGEID = "rtmortgageid";
        public const string EDITCOMPANYSERVICEFEE = "companyservicefee";
        public const string VENDORVIEW = "vendorviewmode";
        public const int VENDORVIEWGRID = 0;
        public const int VENDORVIEWDETAILS = 1;
        #endregion

        #region page names
        public const string PUBLICPAGENAME = "default.aspx";
        public const string ADMINPAGENAME = "administration/default.aspx";
        public const string LOGINPAGENAME = "login.aspx";
        public const string CHANGEPASSWORDPAGENAME = "ChangePassword.aspx";
        #endregion

        #region admin controls names
        public const string CTLVIEWDOCUMENTTEMPLATE = "ViewDocumentTemplate.ascx";
        public const string CTLHUDFACTORS = "HUDFactors.ascx";
        public const string CTLLINKS = "Link.ascx";
        public const string CTLVIEWAFFILIATES = "ViewAffiliates.ascx";
        public const string CTLVIEWUSERLS = "ViewUserLS.ascx";
        public const string CTLVIEWUSERCL = "ViewUserOriginator.ascx";
        public const string CTLEDITUSER = "EditUserOriginator.ascx";
        public const string CTLEDITUSERLS = "EditUserLS.ascx";
        public const string CTLEDITUSERCL = "EditUserCL.ascx";
        public const string CTLVIEWCOMPANY = "ViewCompany.ascx";
        public const string CTLVIEWROLETEMPLATE = "ViewRoleTemplate.ascx";
        public const string CTLVIEWROLE = "ViewRole.ascx";
        public const string CTLEDITROLETEMPLATE = "EditRoleTemplate.ascx";
        public const string CTLEDITROLE = "EditRole.ascx";
        public const string CTLMPSTATUS = "MpStatus.ascx";
        public const string CTLMPFIELD = "MpField.ascx";
        public const string CTLVIEWRULETREE = "RulesTree.ascx";
        public const string CTLVIEWFIELDS = "ViewFields.ascx";
        public const string CTLVIEWFIELDSNEW = "ManageFields.ascx";
        public const string CTLVIEWREQUIREDFIELDS = "ViewRequiredFields.ascx";
        public const string CTLEDITRULELS = "EditRuleLs.ascx";
        public const string CTLEDITRULECL = "EditRuleCl.ascx";
        public const string CTLEDITFIELDS = "LenderSpecificFields.ascx";
        public const string CTLHOLIDAYS = "ViewHolidays.ascx";
        public const string CTLVIEWPRODUCT = "ViewProduct.ascx";
//        public const string CTLVIEWRATE = "RateView.ascx";
        public const string CTLVIEWRATE = "ViewProductRates.ascx";
        public const string CTLVIEWLIMIT = "LimitView.ascx";
        public const string CTLEDITPRODUCT = "EditProduct.ascx";
        public const string CTLEDITRULE = "EditRule.ascx";
        public const string CTLEDITDOCTEMPLATE = "EditDocTemplate.ascx";
        public const string CTLEDITCOMPANY = "EditCompany.ascx";
        public const string CTLEDITCOMPANYSTRUCTURE = "EditCompanyStructure.ascx";
        public const string CTLEDITRULEFIELD = "EditRuleField.ascx";
        public const string CTLEDITRULECONDITION = "EditRuleCondition.ascx";
        public const string CTLEDITRULETASK = "EditRuleTask.ascx";
        public const string CTLEDITMAILSETTINGS = "EditMailSettings.ascx";
        public const string CTLGLOBALDMIN = "GlobalAdminFields.ascx";
        public const string CTLVIEWMYAFFILIATES = "ViewMyAffiliate.ascx";
        public const string CTLVIEWLENDERPRODUCT = "LenderProduct.ascx";
        public const string CTLREQUIREDFIELDGROUP = "RequiredFieldGroup.ascx";
        public const string CTLVENDORFEECATEGORY = "VendorFeeCategory.ascx";
        public const string CTLVIEWFIELDDETAILS = "FieldDetails.ascx";
        public const string CTLEDITCOMPANYSERVICEFEE = "CompanyFeeSettings.ascx";
        public const string CTLVIEWVENDORS = "ViewVendors.ascx";
        public const string CTLVIEWFEETYPES = "ViewFee.ascx";
        public const string CTLEDITVENDOR = "EditVendor.ascx";
        public const string CTLIMPORTLEAD = "ImportLead.ascx";
        public const string CTLLEADCAMPAIGN = "ViewLeadCampaign.ascx";
        public const string CTLCOMPANYLOCATION = "CompanyLocation.ascx";
        public const string CTLSTATELICENSING = "StateLicensing.ascx";
        public const string CTLEDITMANAGERRELATION = "EditManagerRelation.ascx";
        public const string CTLLOANOFFICERSTATELICENSING = "LoanOfficerStateLicensing.ascx";
        public const string CONTROLSLOCATION = "~/administration/controls/";
        #endregion

        #region frontend controls names
        public const string FECTLTABPACKAGE = "TabPackage.ascx";
        public const string FECTLMRTGAGEPROFILES = "MortgageProfiles.ascx";
        public const string FECTLCALENDAR = "Calendar.ascx";
        public const string FECTLCALCULATOR = "Calculator.ascx";
        public const string FECTLCONDITIONS = "Conditions.ascx";
        public const string FECTLEMAILS = "Emails.ascx";
        public const string FECTLREPORTS = "Reports.ascx";
        public const string FECTLEMAILADD = "EmailAdd.ascx";
        public const string FECTLINVOICES = "Invoices.ascx";
        public const string FECTLRESERVES = "Reserves.ascx";
        public const string FECTLPREPAIDITEMS = "MortgagePrepaidItems.ascx";
        public const string FECTLCHECKLIST = "CheckList.ascx";
        public const string FECTLMORGAGEDETAILS = "MortgageDetails.ascx";
        public const string FECTLFOLLOWUP = "Followup.ascx";
        public const string FECTLNOTES = "Notes.ascx";
        public const string FECTLTABS = "Tabs.ascx";
        public const string FECTLVENDORS = "Vendors.ascx";
        public const string FECTLVENDORSPUBLIC = "ViewVendors.ascx";
        public const string FECTLMYPROFILE = "MyProfile.ascx";
        public const string FECTLGFE = "ClosingCostProfile.ascx";
        public const string FECONTROLSLOCATION = "~/Controls/";
        public const string FEADMINCONTROLSLOCATION = "~/Administration/Controls/";
        public const string FECTLAPPLICANTLIST = "ApplicantList.ascx";
        public const string FECTLMORTGAGEPROPERTY = "MortgageProperty.ascx";
        public const string FECTLMORTGAGEMORTGAGE = "MortgageMortgage.ascx";
        public const string FECTLMORTGAGEBORROWERS = "MortgageBorrowers.ascx";
        public const string FECTLMORTGAGECONTACTS = "MortgageContacts.ascx";
        public const string FECTLMORTGAGEDATES = "MortgageImportantDates.ascx";
        public const string FECTLMORTGAGECLOSINGCOSTS = "MortgageClosingCosts.ascx";
        public const string FECTLMORTGAGESELLERPAY = "MortgageSellerPay.ascx";
        public const string FECTLMORTGAGEBUYERPAY = "MortgageBuyerPay.ascx";
        public const string FECTLLEADCALC = "LeadCalculator.ascx";
//        public const string FECTLADVCALC = "AdvancedCalculator.ascx";
        public const string FECTLADVCALCULATOR = "AdvCalculator.ascx";
        public const string FECTLMORTGAGECORRESPONDENTLENDER = "MortgageCorrespondentLender.ascx";
        public const string FECTLFIELDSCHANGES = "FieldChanges.ascx";
        public const string FECTLLOGINTRACK = "LoginTrack.ascx";
        public const string FECTLLINKS = "Links.ascx";
        public const string FECTLHELP = "Help.ascx";
        public const string FECTLREPAIRITEMS = "PropertyRepairItems.ascx";

        public const string FECTLMORTGAGEBORROWERSTAB = "BorrowertabControl.ascx";
        public const string FECTLMORTGAGEPROPERTYTAB = "PropertytabControl.ascx";
        public const string FECTLMORTGAGEMORTGAGETAB = "MortgagetabControl.ascx";
        public const string FECTLMORTGAGELENDERTAB = "LendertabControl.ascx";
        public const string FECTLEDITLEADCAMPAIGN = "EditLeadCampaign.ascx";
        public const string VENDORLOGINPAGE = "LoginVendor.aspx";
        public const string VENDORPAGE = "VendorDashBoard.aspx";
        #endregion

        #region user related constants
        public const int ENABLEDSTATUSID = 1;
        public const int DISABLEDSTATUSID = 2;
        public const int DELETEDSTATUSID = 3;
        #endregion

        #region general
        public const string IMAGEFOLDER = "/images";
        public const string STORAGEFOLDER = "~/Storage";
        public const string DELETEBUTTONIMG = "btn_grd_delete";
        public const string TEMPLATESFOLDER = "/Templates/";
        public const string PACKAGESFOLDER = "/Packages/";
        public const string MAILATTACHESFOLDER = "/MailAttachments/";
        public const string LOGOIMAGEFOLDER = STORAGEFOLDER + "/LogoImage";
        public const string USERPHOTOFOLDER = STORAGEFOLDER + "/UserPhoto";
        public const string JSDELETECONFIRM = "javascript:{{return confirm('Are you sure you want to delete this {0}?');}};";
        public const string DEFAULTLOGOIMAGE = IMAGEFOLDER + "/na.jpg";
        public const int Yes = 1;
        public const int No = 2;
        public const string CACHEDTABSINDEXESFIELDNAME = "cachedtabsindexes";
        public const string REQUESTEVENTTARGET = "__EVENTTARGET";
        public const string REQUESTEVENTARGUMENT = "__EVENTARGUMENT";
        #endregion

        #region grid commands
        public const string DISABLECOMMAND = "disable";
        public const string ENABLECOMMAND = "enable";
        public const string DELETECOMMAND = "delete";
        public const string EDITCOMMAND = "edit";
        public const string SORTCOMMAND = "sort";
        #endregion

        #region retail site constants
        public const string RSINPUTPAGE = "input";
        public const string RSCALCULATORPAGE = "calculate";
        public const string RSCTLCALCULATORPAGE = "Calculator.ascx";
        public const string RSCTLINPUTPAGE = "InputPage.ascx";
        public const string RSCTLMAINPAGE = "MainContent.ascx";
        public const string RSCONTROLSLOCATION = "~/RetailSite/control/";
        #endregion


        public const int SEXMALE = 0;
        public const int SEXFEMALE = 1;

        public const int GID = 1;

        public const int LOANSTARCOMPANYID = 1;

        public const int TASKSTATUSACTIVE = 1;
        public const int TASKSTATUSCOMPLETED = 2;

        public const int CONDITIONSTATUSSATISFIED = 1;
        public const int CONDITIONSTATUSNOTSATISFIED = 2;
        public const int CONDITIONINVISIBLE = 0;
        public const int CONDITIONVISIBLE = 1;
        public const int CONDITIONPROPERTYCATEGORYID = 1;
        public const int CONDITIONCREDITCATEGORYID = 2;

        public const int EVENTTYPEIDCONDITIONCREATED = 26;
        public const int EVENTTYPEIDCONDITIONCHANGED = 28;
        public const int EVENTTYPEIDFOLLOWUPCREATED = 29;
        public const int EVENTTYPEIDFOLLOWUPCHANGED = 30;
        public const int EVENTTYPEIDCONDITIONSATISFIED = 31;
        public const int EVENTTYPEIDCONDITIONCOMPLETED = 32;
        public const int EVENTTYPEIDCONDITIONREMOVED = 33;
        public const int EVENTTYPEIDCONDITIONUNSATISFIED = 35;


        public const int INVOICECHARGETOBORROWER = 1;
        public const int INVOICECHARGETOSELLER = 2;

        public const int PRODUCTTYPEHOMEKEEPER = 1;

        public const int ROLEUNDEWRITER = 11;

        public const string SUCCESSMESSAGE = "Operation completed successfully";

        public const int DICTIONARYYES = 1;
        public const int DICTIONARYNO = 2;

        public const string GRIDPROPERTYLIST = "gridpropertylist";
        public const string GRIDPOSTBACK = "gridpostback";

        public const string DICTIONARYYESNOTABLE = "DictionaryYesNo";

        private Constants()
        {
        }
    }
}