using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class ViewUserLS : AppControl
    {
        #region constants
        private const string TOOLTIPTEXT = "Click link to {0} user";
        private const string ENABLETEXT = "enable";
        private const string DISABLETEXT = "disable";
        private const string ONCLICKATTRIBUTE = "OnClick";
        private const string UNLOCKCOMMAND = "unlock";
        private const string USERTEXT = "user";
        private const string UNLOCKTEXT = "Unlock user";
        #endregion

        #region fields
        private string orderby = String.Empty;
        private string whereclause = String.Empty;
        private const bool ALL = true;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsLoanStarAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            addLink.NavigateUrl = GetEditLink(-1);
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        #region methods
        private void BindData()
        {
            if (IsPostBack)
            {
                whereclause = GetFilter();
            }
            DataView dv = AppUser.GetAdminList();
            dv.RowFilter = whereclause;
            dv.Sort = orderby;
            G.DataSource = dv;
            G.DataBind();
            //ddlCompany.DataSource = Company.GetList(ALL);
            //ddlCompany.DataTextField = Company.COMPANYFIELDNAME;
            //ddlCompany.DataValueField = Company.IDFIELDNAME;
            //ddlCompany.DataBind();
            ddlStatus.DataSource = AppUser.GetStatusList(!ALL);
            ddlStatus.DataTextField = AppUser.STATUSNAMEFIELDNAME;
            ddlStatus.DataValueField = AppUser.IDFIELDNAME;
            ddlStatus.DataBind();
        }
        private string GetEditLink(int userId)
        {
            return ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.EDITUSER + "&" + Constants.IDPARAM + "=" + userId);
        }
        private string GetFilter()
        {
            StringBuilder sb = new StringBuilder();
            AppendTextBoxCondition(sb, tbLogin.Text, AppUser.LOGINFIELDNAME);
            AppendTextBoxCondition(sb, tbFirstName.Text, AppUser.FIRSTNAMEFIELDNAME);
            AppendTextBoxCondition(sb, tbLastName.Text, AppUser.LASTNAMEFIELDNAME);
//            AppendSelectCondition(sb, ddlCompany.SelectedValue, AppUser.COMPANYIDFIELDNAME);
            AppendSelectCondition(sb, ddlStatus.SelectedValue, AppUser.STATUSIDFIELDNAME);
            //if (sb.Length > 0)
            //{
            //    sb.Insert(0, " where ");
            //}
            return sb.ToString();
        }
        private void SortData(string sortexpr)
        {
            string sortorder = GetSortOrder(sortexpr);
            if (sortorder == "asc")
            {
                sortorder = "desc";
            }
            else
            {
                sortorder = "asc";
            }
            SetSortOrder(sortexpr, sortorder);
//            orderby = "order by " + sortexpr + " " + sortorder;
            orderby = sortexpr + " " + sortorder;
        }
        #endregion

        #region event handlers
        protected void Search_Click(object sender, EventArgs e)
        {
            BindData();
        }
        #endregion

        #region grid related methods
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {

            string cmd = e.CommandName.ToLower();
            if (cmd == Constants.SORTCOMMAND)
            {
                return;
            }            
            AppUser user = new AppUser();
            user.Id = int.Parse(e.CommandArgument.ToString());

            switch (cmd)
            {
                case Constants.DISABLECOMMAND:
                    user.Disable();
                    break;
                case Constants.ENABLECOMMAND:
                    user.Enable();
                    break;
                case Constants.DELETECOMMAND:
                    user.Delete();
                    break;
                case UNLOCKCOMMAND:
                    user.Unlock();
                    break;
                case Constants.EDITCOMMAND:
                    Response.Redirect(GetEditLink(user.Id));
                    break;
                default:
                    return;
            }
            BindData();
        }
        protected void G_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row != null)
                {
                    LinkButton lnkbutton = (LinkButton)e.Item.Cells[5].Controls[1];
                    bool isLocked = row["lockTime"] != DBNull.Value;
                    if (isLocked)
                    {
                        e.Item.ForeColor = System.Drawing.Color.Red;
                        lnkbutton.CommandName = UNLOCKCOMMAND;
                        lnkbutton.Text = UNLOCKTEXT;
                    }
                    else
                    {
                        lnkbutton.Text = row[AppUser.STATUSNAMEFIELDNAME].ToString();
                        lnkbutton.CommandName = (int.Parse(row[AppUser.STATUSIDFIELDNAME].ToString()) == Constants.ENABLEDSTATUSID) ? Constants.DISABLECOMMAND : Constants.ENABLECOMMAND;
                    }

                    int userid = int.Parse(row[AppUser.IDFIELDNAME].ToString());
                    bool isCurrent = userid == CurrentUser.Id;
                    lnkbutton.Enabled = !isCurrent;

                    lnkbutton.CommandArgument = userid.ToString();
                    lnkbutton.ToolTip = String.Format(TOOLTIPTEXT,lnkbutton.CommandName == Constants.DISABLECOMMAND ? DISABLETEXT : ENABLETEXT);
                    ImageButton imgbutton = (ImageButton)e.Item.Cells[6].Controls[3];
                    imgbutton.Enabled = !isCurrent;
                    imgbutton.CommandArgument = userid.ToString();
                    imgbutton.ImageUrl = "~" + Constants.IMAGEFOLDER + "/" + Constants.DELETEBUTTONIMG + (imgbutton.Enabled ? "" : "_dis") + ".gif";
                    if (imgbutton.Enabled)
                    {
                        imgbutton.Attributes.Add(ONCLICKATTRIBUTE,String.Format(Constants.JSDELETECONFIRM,USERTEXT));
                    }
                    imgbutton = (ImageButton)e.Item.Cells[6].Controls[1];
                    imgbutton.CommandArgument = userid.ToString();
                }
            }
        }
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