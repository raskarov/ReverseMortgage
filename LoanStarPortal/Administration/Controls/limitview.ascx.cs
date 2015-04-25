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
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class limitview : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindData();
        }

        private void BindData()
        {
            ddlStates.DataSource = DataHelpers.GetStateList();
            ddlStates.DataBind();
            ddlStates.Items.Insert(0, new ListItem("-All-", "-1"));
            ddlStates.SelectedIndex = 0;
            G.Rebind();
        }

        protected void ddlStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            G.Rebind();
        }

        protected void G_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            G.DataSource = DataHelpers.GetLendingLimitList(Convert.ToInt32((ddlStates.SelectedIndex == -1) ? "-1" : ddlStates.SelectedValue));
        }
    }
}