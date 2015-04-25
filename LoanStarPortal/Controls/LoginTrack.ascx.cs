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
using LoanStar.Common;
using Telerik;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class LoginTrack : AppControl
    {
        #region Fields/Properties
        public bool IsFirstLoad
        {
            get
            {
                Object o = ViewState["FirstLoad"];
                bool res = true;
                if (o != null)
                {
                    res = Convert.ToBoolean(o.ToString());
                }
                return res;
            }
            set
            {
                ViewState["FirstLoad"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsFirstLoad)
            {
                grid.DataBind();
                IsFirstLoad = false;
            }
        }

        protected void grid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            if (CurrentUser != null)
            {
                grid.DataSource = CurrentUser.GetUserLogs();
            }
        }
    }
}