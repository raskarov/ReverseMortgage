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

namespace LoanStarPortal.Administration.Controls
{

    public partial class ViewUserCL : AppControl
    {
        #region constants
        private const string TOOLTIPTEXT = "Click link to {0} user";
        private const string ENABLETEXT = "enable";
        private const string DISABLETEXT = "disable";
        private const string ONCLICKATTRIBUTE = "OnClick";
        private const string USERTEXT = "user";
        #endregion

        #region fields
        //private string orderby = String.Empty;
        //private string whereclause = String.Empty;
        private const bool ALL = true;
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsCorrespondentLenderAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            addLink.NavigateUrl = GetEditUserLink(-1);
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }
        private void BindData()
        {
            G.DataSource = AppUser.GetLenderUserList(CurrentUser.CompanyId,"", "");
            G.DataBind();
            ddlStatus.DataSource = AppUser.GetStatusList(true);
            ddlStatus.DataTextField = AppUser.STATUSNAMEFIELDNAME;
            ddlStatus.DataValueField = AppUser.IDFIELDNAME;
            ddlStatus.DataBind();
        }
        private string GetEditUserLink(int userId)
        {
            return ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.EDITUSER + "&" + Constants.IDPARAM + "=" + userId.ToString());
        }

        protected void Search_Click(object sender, EventArgs e)
        {

        }

        protected void G_InsertCommand(object source, Telerik.WebControls.GridCommandEventArgs e)
        {

        }

        protected void G_ItemDataBound(object sender, Telerik.WebControls.GridItemEventArgs e)
        {
            if ((e.Item.ItemType == Telerik.WebControls.GridItemType.Item) || (e.Item.ItemType == Telerik.WebControls.GridItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row != null)
                {
                    LinkButton lnkbutton = (LinkButton)e.Item.Cells[5].Controls[1];
                    lnkbutton.Text = row[AppUser.STATUSNAMEFIELDNAME].ToString();
                    int userid = int.Parse(row[AppUser.IDFIELDNAME].ToString());
                    bool isCurrent = userid == CurrentUser.Id;
                    lnkbutton.Enabled = !isCurrent;
                    lnkbutton.CommandName = (int.Parse(row[AppUser.STATUSIDFIELDNAME].ToString()) == Constants.ENABLEDSTATUSID) ? Constants.DISABLECOMMAND : Constants.ENABLECOMMAND;
                    lnkbutton.CommandArgument = userid.ToString();
                    lnkbutton.ToolTip = String.Format(TOOLTIPTEXT, lnkbutton.CommandName == Constants.DISABLECOMMAND ? DISABLETEXT : ENABLETEXT);
                    ImageButton imgbutton = (ImageButton)e.Item.Cells[6].Controls[3];
                    imgbutton.Enabled = !isCurrent;
                    imgbutton.CommandArgument = userid.ToString();
                    imgbutton.ImageUrl = "~" + Constants.IMAGEFOLDER + "/" + Constants.DELETEBUTTONIMG + (imgbutton.Enabled ? "" : "_dis") + ".gif";
                    if (imgbutton.Enabled)
                    {
                        imgbutton.Attributes.Add(ONCLICKATTRIBUTE, String.Format(Constants.JSDELETECONFIRM, USERTEXT));
                    }
                    imgbutton = (ImageButton)e.Item.Cells[6].Controls[1];
                    imgbutton.CommandArgument = userid.ToString();
                }
            }
        }

        protected void G_PageIndexChanged(object source, Telerik.WebControls.GridPageChangedEventArgs e)
        {
            G.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }

        protected void G_ItemCommand(object source, Telerik.WebControls.GridCommandEventArgs e)
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
                case Constants.EDITCOMMAND:
                    Response.Redirect(GetEditLink(user.Id));
                    break;
                default:
                    return;
            }
            BindData();
        }
        private string GetEditLink(int userId)
        {
            return ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.EDITUSER + "&" + Constants.IDPARAM + "=" + userId.ToString());
        }
    }
}