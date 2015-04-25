using System;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class ViewProduct : AppControl
    {
        private Product product;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsLoanStarAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            addLink.NavigateUrl = GetEditProductLink();
            product = new Product();
            CurrentPage.StoreObject(product, Constants.PRODUCTOBJECT);
            if (!Page.IsPostBack)
            {
                BindData();
            }
            else
                RefreshLog();
        }

        private void BindData()
        {
            G.DataSource = Product.GetProductList(false);
            G.DataBind();
            RefreshLog();
        }

        private void RefreshLog()
        {
            object lastLimitDate = EventLog.GetLastLimitDate();
            lblLastLimitDate.Text = (lastLimitDate==DBNull.Value)?"N/A":Convert.ToDateTime(lastLimitDate).ToLongDateString();
//            object lastRateDate = EventLog.GetLastRateDate();
//            lblLastRateDate.Text = (lastRateDate==DBNull.Value)?"N/A":Convert.ToDateTime(lastRateDate).ToLongDateString();
            gridLog.DataSource = EventLog.GetEventLog();
            gridLog.DataBind();
        }

        private string GetEditProductLink()
        {
            return ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.EDITPRODUCT);
        }
        #region grid related methods
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            int id = -1;
            if (cmd != Constants.SORTCOMMAND && cmd != "initinsert")
            {
                id = int.Parse(e.CommandArgument.ToString());
            }
            switch (cmd)
            {
                case Constants.DELETECOMMAND:
                    product.ID = id;
                    product.Delete();
                    break;
                case "initinsert":
                    product = new Product(-1);
                    CurrentPage.StoreObject(product, Constants.PRODUCTOBJECT);
                    Response.Redirect(GetEditProductLink());
                    break;
                case Constants.EDITCOMMAND:
                    product = new Product(id);
                    CurrentPage.StoreObject(product, Constants.PRODUCTOBJECT);
                    Response.Redirect(GetEditProductLink());
                    break;
            }
            BindData();
        }
        #endregion

        protected void btnRateUpdate_Click(object sender, EventArgs e)
        {
            DataLoader.RateLoader rate = new DataLoader.RateLoader();
            rate.GetData();
            RefreshLog();
        }

        protected void btnLimitUpdate_Click(object sender, EventArgs e)
        {
            DataLoader.LendingLimitLoader lim = new DataLoader.LendingLimitLoader();
            lim.GetData();
            RefreshLog();
        }

        protected void btnViewLimits_Click(object sender, EventArgs e)
        {
            Response.Redirect(ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.VIEWLIMIT));
        }

        protected void RadAjaxTimer1_Tick(object sender, TickEventArgs e)
        {

        }
    }
}