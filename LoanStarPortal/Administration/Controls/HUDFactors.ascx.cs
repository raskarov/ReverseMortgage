using System;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class HUDFactors : AppControl
    {
        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsLoanStarAdmin)
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));

            HUDFactorsSource.SelectCommand = "GetHUDFactors";
            HUDFactorsSource.ConnectionString = AppSettings.SqlConnectionString;
        }

/*        protected void RadGridHUDFactors_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem || e.Item is GridHeaderItem)
                e.Item.Cells[0].CssClass = "locked";

//                for (int i = 0; i < e.Item.Cells.Count; i++)
//                    if (i > 0)
//                        e.Item.Cells[i].CssClass = "locked";
        }*/
        #endregion
    }
}