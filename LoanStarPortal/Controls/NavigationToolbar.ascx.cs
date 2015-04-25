using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class NavigationToolbar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void NavToolbar_OnClick(object sender, RadToolbarClickEventArgs e)
        {
            if (e.Button is RadToolbarToggleButton)
            {
                if (!((RadToolbarToggleButton)e.Button).Toggled)
                {
                    ((RadToolbarToggleButton)e.Button).Toggled = true;
                }
            }

        }
    }
}