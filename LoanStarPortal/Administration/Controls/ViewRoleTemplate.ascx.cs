using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class ViewRoleTemplate : AppControl
    {
        #region constants
        private const bool ISTEMPLATE = true;
        private const bool ALL = true;
        private const string ONCLICKATTRIBUTE = "OnClick";
        private const string ROLETEXT = "role";
        #endregion

        #region fields
        private bool isAscending = true;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsLoanStarAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
//            addLink.NavigateUrl = GetEditRoleLink();
            if (!Page.IsPostBack)
            {
                BindData();
            }
            else
            {
                Role r = new Role(-1, ISTEMPLATE);
                CurrentPage.StoreObject(r, Constants.ROLEOBJECT);
            }
        }
        #region methods
        private void BindData()
        {
            G.DataSource = Role.GetList(ISTEMPLATE,!ALL, isAscending, -1);
            G.DataBind();
        }
        private string GetEditRoleLink()
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
                //case Constants.DELETECOMMAND:
                //    r = new Role(true);
                //    r.Id = roleid;
                //    r.Delete();
                //    break;
                case Constants.EDITCOMMAND:
                    r = new Role(roleid, true);
                    CurrentPage.StoreObject(r, Constants.ROLEOBJECT);
                    Response.Redirect(GetEditRoleLink());
                    break;
                default:
                    return;
            }
            BindData();
        }
        //protected void G_ItemDataBound(object sender, GridItemEventArgs e)
        //{
        //    if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
        //    {
        //        DataRowView row = (DataRowView)e.Item.DataItem;
        //        if (row != null)
        //        {
        //            //string roleid = row[LoanStar.Common.Rule.IDFIELDNAME].ToString();
        //            ImageButton imgbutton = (ImageButton)e.Item.Cells[3].Controls[3];
        //            imgbutton.Attributes.Add(ONCLICKATTRIBUTE, String.Format(Constants.JSDELETECONFIRM, ROLETEXT));
        //        }
        //    }
        //}
        protected void G_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            G.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }
        protected void G_SortCommand(object source, GridSortCommandEventArgs e)
        {
            SortData(e.SortExpression);
            BindData();
        }
        #endregion

    }
}