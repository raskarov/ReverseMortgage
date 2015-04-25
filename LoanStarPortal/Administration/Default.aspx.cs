using System;
using System.Web.Security;
using System.Web.UI;
using LoanStar.Common;

namespace LoanStarPortal.Administration
{
    public partial class Default : AppPage
    {

        #region fields
        private string param_ctl = String.Empty;
        private bool isLoanStarAdmin;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            param_ctl = GetValue(Constants.CONTROLPARAM, Constants.VIEWUSER);
            isLoanStarAdmin = CurrentUser.IsLoanStarAdmin&&(!CurrentUser.LoggedAsOriginator);
            LoadControl();
        }
        private void LoadControl()
        {
            Control ctl;
            string ctlname = String.Empty;
            switch (param_ctl)
            {
                case Constants.VIEWHUDFACTORS:
                    ctlname = Constants.CTLHUDFACTORS;
                    break;
                case Constants.VIEWDOCTEMPLATE:
                    ctlname = Constants.CTLVIEWDOCUMENTTEMPLATE;
                    break;
                case Constants.VIEWUSER:
                    ctlname = isLoanStarAdmin ? Constants.CTLVIEWUSERLS : Constants.CTLVIEWUSERCL;
                    break;
                case Constants.EDITUSER:
                    ctlname = isLoanStarAdmin ? Constants.CTLEDITUSERLS : Constants.CTLEDITCOMPANYSTRUCTURE;
                    break;
                case Constants.VIEWCOMPANY:
                    ctlname = Constants.CTLVIEWCOMPANY;
                    break;
                case Constants.VIEWROLE:
                    ctlname = isLoanStarAdmin ? Constants.CTLVIEWROLETEMPLATE : Constants.CTLVIEWROLE;
                    break;
                case Constants.EDITROLE:
                    ctlname = isLoanStarAdmin ? Constants.CTLEDITROLETEMPLATE : Constants.CTLEDITROLE;
                    break;
                case Constants.EDITDOCTEMPLATE:
                    ctlname = Constants.CTLEDITDOCTEMPLATE;
                    break;
                case Constants.EDITCOMPANY:
                    ctlname = Constants.CTLEDITCOMPANY;
                    break;
                //case Constants.EDITCOMPANYSTRUCTURE:
                //    ctlname = Constants.CTLEDITCOMPANYSTRUCTURE;
                //    break;
                case Constants.VIEWRULETREE:
                    ctlname = Constants.CTLVIEWRULETREE;
                    break;
                case Constants.EDITFIELDS:
                    ctlname = Constants.CTLEDITFIELDS;
                    break;
                case Constants.VIEWHOLIDAYS:
                    ctlname = Constants.CTLHOLIDAYS;
                    break;
                case Constants.VIEWPRODUCT:
                    ctlname = Constants.CTLVIEWPRODUCT;
                    break;
                case Constants.VIEWRATE:
                    ctlname = Constants.CTLVIEWRATE;
                    break;
                case Constants.VIEWLIMIT:
                    ctlname = Constants.CTLVIEWLIMIT;
                    break;
                case Constants.VIEWREQUIREDFIELDS:
                    ctlname = Constants.CTLVIEWREQUIREDFIELDS;
                    break;
                case Constants.VIEWFIELDS:
                    ctlname = Constants.CTLVIEWFIELDS;
                    break;
                case Constants.VIEWFIELDSNEW:
                    ctlname = Constants.CTLVIEWFIELDSNEW;
                    break;
                case Constants.EDITPRODUCT:
                    ctlname = Constants.CTLEDITPRODUCT;
                    break;
                case Constants.EDITUSERORG:
                    ctlname = Constants.CTLEDITUSER;
                    break;
                case Constants.EDITMAILSETTINGS:
                    ctlname = Constants.CTLEDITMAILSETTINGS;
                    break;
                case Constants.VIEWLINKS:
                    ctlname = Constants.CTLLINKS;
                    break;
                case Constants.VIEWAFFILIATE:
                    ctlname = Constants.CTLVIEWAFFILIATES;
                    break;
                case Constants.VIEWGLOBALADMINFIELDS:
                    ctlname = Constants.CTLGLOBALDMIN;
                    break;
                case Constants.VIEWMYORIGINATORS:
                    ctlname = Constants.CTLVIEWMYAFFILIATES;
                    break;
                case Constants.VIEWMYLENDERS:
                    ctlname = Constants.CTLVIEWMYAFFILIATES;
                    break;
                case Constants.VIEWMYSERVICERS:
                    ctlname = Constants.CTLVIEWMYAFFILIATES;
                    break;
                case Constants.VIEWMYINVESTORS:
                    ctlname = Constants.CTLVIEWMYAFFILIATES;
                    break;
                case Constants.VIEWLENDERPRODUCT:
                    ctlname = Constants.CTLVIEWLENDERPRODUCT;
                    break;
                case Constants.VIEWFIELDDETAILS:
                    ctlname = Constants.CTLVIEWFIELDDETAILS;
                    break;
                case Constants.EDITCOMPANYSERVICEFEE:
                    ctlname = Constants.CTLEDITCOMPANYSERVICEFEE;
                    break;
                case Constants.VIEWVENDORS:
                    ctlname = Constants.CTLVIEWVENDORS;
                    break;
                case Constants.VIEWMYFEETYPES:
                    ctlname = Constants.CTLVIEWFEETYPES;
                    break;

                case Constants.EDITVENDOR:
                    ctlname = Constants.CTLEDITVENDOR;
                    break;
                case Constants.IMPORTLEAD:
                    ctlname = Constants.CTLIMPORTLEAD;
                    break;
                case Constants.LEADCAMPAIGN:
                    ctlname = Constants.CTLLEADCAMPAIGN;
                    break;
                case Constants.COMPANYLOCATION:
                    ctlname = Constants.CTLCOMPANYLOCATION;
                    break;
                case Constants.VIEWSTATELICENSING:
                    ctlname = Constants.CTLSTATELICENSING;
                    break;
                case Constants.LOGOUT:

                    
                    FormsAuthentication.SignOut();
                    Session.Clear();
                    Global.integration = null;
                    Global.userAccount = null;
                    Response.Redirect(ResolveUrl("/" + Constants.ADMINPAGENAME));
                    break;
            }
            ctl = LoadControl(Constants.CONTROLSLOCATION + ctlname);
            if (ctl != null)
            {
                PlaceHolder1.Controls.Add(ctl);
            }
        }
    }
}
