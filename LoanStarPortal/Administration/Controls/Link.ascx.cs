using System;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class Link : AppControl
    {

        #region methods
        private void BindData()
        {
            grid.DataBind();
        }
        #endregion

        protected void grid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            grid.DataSource = CurrentUser.GetUserLinks(CurrentUser.EffectiveCompanyId);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        protected void grid_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                if (grid.EditItems.Count > 0)
                {
                    grid.MasterTableView.ClearEditItems();
                }
            }
            if (e.CommandName == RadGrid.EditCommandName)
            {
                e.Item.OwnerTableView.IsItemInserted = false;
            }
            if (e.CommandName == RadGrid.DeleteCommandName)
            {
                int id = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString());
                e.Canceled = true;
                CurrentUser.DeleteUserLink(id);
                grid.MasterTableView.ClearEditItems();
                BindData();

            }
            if (e.CommandName == RadGrid.UpdateCommandName)
            {
                e.Item.OwnerTableView.IsItemInserted = false;
                e.Canceled = true;

                int id = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString());
                int company = CurrentUser.EffectiveCompanyId;
                string title = ((TextBox)e.Item.FindControl("txtTitle")).Text.Trim();
                string url = ((TextBox)e.Item.FindControl("txtURL")).Text.Trim();
                string description = ((TextBox)e.Item.FindControl("txtDescription")).Text.Trim();
                CurrentUser.SaveUserLink(id, company, title, url, description);
                grid.MasterTableView.ClearEditItems();
                BindData();
            }
            else if (e.CommandName == RadGrid.PerformInsertCommandName)
            {
                e.Item.OwnerTableView.IsItemInserted = false;
                e.Canceled = true;

                int company = CurrentUser.EffectiveCompanyId;
                string title = ((TextBox)e.Item.FindControl("txtTitle")).Text.Trim();
                string url = ((TextBox)e.Item.FindControl("txtURL")).Text.Trim();
                string description = ((TextBox)e.Item.FindControl("txtDescription")).Text.Trim();
                CurrentUser.SaveUserLink(0, company, title, url, description);
                grid.MasterTableView.ClearEditItems();
                BindData();
            }

        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
    }
}