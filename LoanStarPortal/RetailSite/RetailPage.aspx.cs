using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LoanStar.Common;

namespace LoanStarPortal.RetailSiteTemplate
{
    public partial class RetailPage : AppPage
    {
        private string param_ctl = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            param_ctl = GetValue(Constants.CONTROLPARAM, "");
            if (param_ctl=="init")
            {
                ClearSessionData();
            }
            LoadControl();
        }
        private void LoadControl()
        {
            Control ctl;
            string ctlname = Constants.RSCTLMAINPAGE;
            switch (param_ctl)
            {
                case Constants.RSINPUTPAGE:
                    ctlname = Constants.RSCTLINPUTPAGE;
                    break;
                case Constants.RSCALCULATORPAGE:
                    ctlname = Constants.RSCTLCALCULATORPAGE;
                    break;
            }
            ctl = LoadControl(Constants.RSCONTROLSLOCATION + ctlname);
            if (ctl != null)
            {
                LoanStarPortal.RetailSite.Control.InputPage inputCtl = ctl as LoanStarPortal.RetailSite.Control.InputPage;
                if (inputCtl != null)
                {
                    inputCtl.AjaxManager = RadAjaxManager1; ;
                }
                Panel1.Controls.Add(ctl);
            }
        }
        private void ClearSessionData()
        {
            Session.Remove("retailsiteinfo");
            Session.Remove(Constants.RETAILSITEMORTGAGEID);
        }
    }
}
