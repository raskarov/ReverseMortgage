using System;

namespace LoanStarPortal.RMOMail
{
    public partial class CloseEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["closingemail"] = "true";
        }
    }
}
