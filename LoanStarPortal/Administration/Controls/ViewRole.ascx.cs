using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class ViewRole : AppControl
    {
        #region constants
        private const bool ISTEMPLATE = true;
        private const bool ALL = true;
        #endregion

        #region fields
        private bool isAscending = true;
        private int companyId = -1;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(CurrentUser.IsCorrespondentLenderAdmin||CurrentUser.LoggedAsOriginator))
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            companyId = CurrentUser.EffectiveCompanyId;
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        #region methods
        private void BindData()
        {
            G.DataSource = Role.GetList(ISTEMPLATE, !ALL, isAscending, companyId);
            G.DataBind();
        }
        private string GetEditLink()
        {
            return ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.EDITROLE);
        }
        private void SortData(string sortexpr)
        {
            string sortorder = GetSortOrder(sortexpr);
            if (sortorder == "asc")
            {
                sortorder = "desc";
                isAscending = false;
            }
            else
            {
                sortorder = "asc";
                isAscending = true;
            }
            SetSortOrder(sortexpr, sortorder);
        }
        #endregion

        #region grid related methods
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            Role r;
            int roleid = -1;
            if (cmd != Constants.SORTCOMMAND)
            {
                roleid = int.Parse(e.CommandArgument.ToString());
            }
            switch (cmd)
            {
                case Constants.EDITCOMMAND:
                    r = new Role(roleid, !ISTEMPLATE);
                    r.CompanyId = CurrentUser.EffectiveCompanyId;
                    //if (CurrentUser.IsLoanStarAdmin && CurrentUser.LoggedAsOriginator)
                    //{
                    //    r.CompanyId = CurrentUser.OriginatorId;
                    //}
                    //else
                    //{
                    //    r.CompanyId = CurrentUser.CompanyId;
                    //}
                    CurrentPage.StoreObject(r, Constants.ROLEOBJECT);
                    Response.Redirect(GetEditLink());
                    break;
                default:
                    return;
            }
            BindData();
        }

        protected void G_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            G.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }
        protected void G_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row != null)
                {
                    string roleid = row[Role.IDFIELDNAME].ToString();
                    ImageButton imgbutton = (ImageButton)e.Item.Cells[3].Controls[1];
                    imgbutton.CommandArgument = roleid;
                }
            }
        }
        protected void G_SortCommand(object source, GridSortCommandEventArgs e)
        {
            SortData(e.SortExpression);
            BindData();
        }
        #endregion
    }
}