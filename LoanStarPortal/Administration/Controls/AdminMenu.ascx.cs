using System;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class AdminMenu : AppControl
    {
        #region constants
        private const string MANAGEDOCTEMPLATETITLE = "Manage Documents";
        private const string MANAGEUSERTITLE = "Manage Users";
        private const string MANAGECOMPANYTITLE = "Manage Companies";
        private const string MANAGEROLETITLE = "Manage Roles";
        private const string RULETREETITLE = "Rule tree";
        private const string MANAGEFIELDSTITLE = "Manage Fields";
        private const string MANAGEMAILSETTINGS = "Manage Mail Settings";
        private const string MANAGEHOLIDAYSTITLE = "Manage Holidays";
        private const string MANAGEPRODUCTTITLE = "Manage Products";
        private const string MANAGELINKS = "Manage Links";
        private const string MANAGEFIELDSNEW = "Manage Fields";
        private const string GLOBALADMINFIELDS = "Global Admin Fields";
        private const string STATELICENSING = "State licensing";
        private const string LOGOUTTITLE = "Logout";
        private const string MANAGEMYLENDERS = "My Lenders";
        private const string MANAGEMYORIGINATORS = "My Originators";
        private const string MANAGEMYSERVICERS = "My Servicers";
        private const string MANAGEMYINVESTORS = "My Investors";
        private const string MANAGEREQUIREDFIELDS = "Required Fields";
        private const string MANAGEVENDORS = "Manage Vendors";
        private const string MANAGEFEETYPES = "Preferred Vendors";
        private const string LEADMANAGEMENT = "Lead Management";
        private const string LEADCAMPAINGMANAGEMENT = "Lead Campaign";
        private const string COMPANYLOCATION = "Branch locations";
        private const string COMPANYADMINHELP = "Help";
        private const string GLOBALADMINHELP = "Help";
        private const string BUTTONGROUP = "adminmenu";
        private const int menuwidth = 120;
        private const string ROLES = "affiliatesroles";
        #endregion
        
        #region fields
        private string currentCmd = String.Empty;
        protected string HelpUrl = String.Empty;
        #endregion

        protected int AffiliateRoles
        {
            get
            {
                int res = 0;
                Object o = Session[ROLES];
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
                else
                {
                    res = Company.GetAffiliatesMask(CurrentUser.EffectiveCompanyId);
                    Session[ROLES] = res;
                }
                return res;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            currentCmd = CurrentPage.GetValue(Constants.CONTROLPARAM, Constants.VIEWUSER);
            if (CurrentUser.IsLoanStarAdmin && (!CurrentUser.LoggedAsOriginator))
            {
               BuildLoanStarAdminMenu();
            }
            else
            {
               BuildCorrespondentLenderAdminMenu();
            }
        }
        #region methods
        private void BuildLoanStarAdminMenu()
        {
            RadToolbar1.Items.Clear();
            AddItem(MANAGEUSERTITLE, Constants.VIEWUSER);
            AddItem(MANAGECOMPANYTITLE, Constants.VIEWCOMPANY);
            AddItem(MANAGEROLETITLE, Constants.VIEWROLE);
            AddItem(MANAGEDOCTEMPLATETITLE, Constants.VIEWDOCTEMPLATE);
            AddItem(RULETREETITLE, Constants.VIEWRULETREE);
            AddItem(MANAGEHOLIDAYSTITLE, Constants.VIEWHOLIDAYS);
            AddItem(MANAGEPRODUCTTITLE, Constants.VIEWPRODUCT);
            AddItem(MANAGEREQUIREDFIELDS, Constants.VIEWREQUIREDFIELDS);
            AddItem(MANAGEVENDORS, Constants.VIEWVENDORS);
            AddItem(MANAGEFIELDSNEW, Constants.VIEWFIELDSNEW);
            AddItem(GLOBALADMINFIELDS, Constants.VIEWGLOBALADMINFIELDS);
            AddItem(STATELICENSING, Constants.VIEWSTATELICENSING);
            if(!String.IsNullOrEmpty(AppSettings.HelpUrlGlobalAdmin))
            {
                AddItem(GLOBALADMINHELP, "Help");
                HelpUrl = AppSettings.HelpUrlGlobalAdmin;
            }
            AddItem(LOGOUTTITLE, Constants.LOGOUT);
        }
        private void BuildCorrespondentLenderAdminMenu()
        {
            RadToolbar1.Items.Clear();
            AddItem(MANAGEUSERTITLE, Constants.VIEWUSER);
            if (!Company.IsGlobalRoleBasedSecurity(CurrentUser.EffectiveCompanyId))
            {
                AddItem(MANAGEROLETITLE, Constants.VIEWROLE);
            }            
            AddItem(MANAGEFIELDSTITLE, Constants.EDITFIELDS);
            if (!Company.IsGlobalRequiredFields(CurrentUser.EffectiveCompanyId))
            {
                AddItem(MANAGEREQUIREDFIELDS, Constants.VIEWREQUIREDFIELDS);
            }
            if (Company.ISORIGINATORBIT != 0)
            {
                AddItem(MANAGEMAILSETTINGS, Constants.EDITMAILSETTINGS);
                AddItem(MANAGEHOLIDAYSTITLE, Constants.VIEWHOLIDAYS);
                AddItem(MANAGELINKS, Constants.VIEWLINKS);
            }
            if ((AffiliateRoles & Company.ISLENDERBIT) != 0)
            {
                AddItem(MANAGEMYLENDERS, Constants.VIEWMYLENDERS);
            }
            if ((AffiliateRoles & Company.ISORIGINATORBIT) != 0)
            {
                AddItem(MANAGEMYORIGINATORS, Constants.VIEWMYORIGINATORS);
            }
            if ((AffiliateRoles & Company.ISSERVICERBIT) != 0)
            {
                AddItem(MANAGEMYSERVICERS, Constants.VIEWMYSERVICERS);
            }
            if ((AffiliateRoles & Company.ISINVESTORBIT) != 0)
            {
                AddItem(MANAGEMYINVESTORS, Constants.VIEWMYINVESTORS);
            }
            if (Company.ISORIGINATORBIT != 0)
            {
                AddItem(MANAGEFEETYPES, Constants.VIEWMYFEETYPES);
                AddItem(LEADMANAGEMENT, Constants.IMPORTLEAD);
            }
            if (Company.IsLeadCapmaignEnabled(CurrentUser.EffectiveCompanyId) && Company.ISORIGINATORBIT!=0)
            {
                AddItem(LEADCAMPAINGMANAGEMENT, Constants.LEADCAMPAIGN);
            }
            if (Company.ISORIGINATORBIT != 0)
            {
                AddItem(COMPANYLOCATION, Constants.COMPANYLOCATION);
            }
            if (!String.IsNullOrEmpty(AppSettings.HelpUrlCompanyAdmin))
            {
                AddItem(COMPANYADMINHELP, "Help");
                HelpUrl = AppSettings.HelpUrlCompanyAdmin;
            }
            AddItem(LOGOUTTITLE, Constants.LOGOUT);
        }
        private void AddItem(string txt, string cmd)
        {
            RadToolbarToggleButton btn = new RadToolbarToggleButton();
            btn.DisplayType = RadToolbarButton.ButtonDisplayType.TextOnly;
            btn.ButtonText = txt;
            btn.CommandName = cmd;
            btn.CausesValidation = false;
            btn.ButtonGroup = BUTTONGROUP;
            btn.Width = menuwidth;
            if (cmd == currentCmd)
            {
                btn.Toggled = true;
            }
            RadToolbar1.Items.Add(btn);
        }
        #endregion

        #region event handlers
        protected void Toolbar1_OnClick(object sender, RadToolbarClickEventArgs e)
        {
            if (e.Button.CommandName != currentCmd)
            {
                Response.Redirect(ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + e.Button.CommandName));
            }
        }
        #endregion
    }
}