using System;
using System.Web.UI;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class Calculator : AppControl
    {
        #region tabs' id
        public const int TABLEADCALC = 1;
        private const int TABADVCALC = 2;
        #endregion

        private const string CURRENTTABID = "currenttabid";
        #region properties
        protected int CurrentTab
        {
            get
            {
                int res = TABLEADCALC;
                if (Session[Constants.CURRENTCALCULATORTABID] != null)
                {
                    res = int.Parse(Session[Constants.CURRENTCALCULATORTABID].ToString());
                }
                return res;
            }
            set
            {
                Session[Constants.CURRENTCALCULATORTABID] = value;
            }
        }
        public bool IsFirstLoad
        {
            get
            {
                Object o = ViewState["FirstLoad"];
                bool res = true;
                if (o != null)
                {
                    try
                    {
                        res = bool.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set
            {
                ViewState["FirstLoad"] = value;
            }
        }
        #endregion

        #region methods
        private void LoadTab()
        {
            //string calcCtl = Constants.FECONTROLSLOCATION + (AppSettings.OldAdvCalc ? Constants.FECTLADVCALC : Constants.FECTLADVCALCULATOR);
            //Control ctl = LoadControl(calcCtl);
            Control ctl = LoadControl(Constants.FECONTROLSLOCATION + Constants.FECTLADVCALCULATOR);
            if (ctl != null)
            {
                pvAdvCalc.Controls.Add(ctl);
            }
        }
        #endregion

        #region event handlers
        protected void TabsMortgageProfiles_TabClick(object sender, Telerik.WebControls.TabStripEventArgs e)
        {
            //switch (e.Tab.Value)
            //{
            //    case "LeadCalculator":
            //        CurrentTab = TABLEADCALC;
            //        break;
            //    case "AdvancedCalculator":
            //        CurrentTab = TABADVCALC;
            //        break;
            //}
            //LoadTab();
        }
        #endregion

        private bool ProcessPostBack()
        {
            string controlName = Page.Request["__EVENTTARGET"];
            if (controlName == "Tabs:CtrlCalculator1:rtsCalculators")
                return true;

            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsFirstLoad)
            {
                LoadTab();
                IsFirstLoad = false;
            }
            else if (!ProcessPostBack())
                LoadTab();
        }

    }
}