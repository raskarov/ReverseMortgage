using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class ViewCompany : AppControl
    {
        #region constants
        private const string CHANGESTATUSCOMMAND = "changestatus";
        private const string ORIGINATORLOGIN = "originatorlogin";
        private const int ENABLEDSTATUSID = 1;
        #endregion

        #region fields
        private string whereclause = String.Empty;
        private string orderby = String.Empty;
        private Company c;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsLoanStarAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            addLink.NavigateUrl = GetEditCompanyLink()+"&isnew=1";
            c = new Company();
            CurrentPage.StoreObject(c, Constants.COMPANYOBJECT);
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }
        #region methods
        protected void BindData()
        {
           
                whereclause = GetFilter();

            G.DataSource = Company.GetListForGrid(orderby,whereclause);
            G.DataBind();
        }
        private string GetEditCompanyLink()
        {
            return ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.EDITCOMPANY);
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
            orderby = "order by " + sortexpr + " " + sortorder;
        }
        private string GetFilter()
        {
            StringBuilder sb = new StringBuilder();
            AppendTextBoxCondition(sb, tbCompany.Text, Company.COMPANYFIELDNAME);

            if (!cbArchived.Checked)
            {
                if (sb.Length > 0)
                    sb.Append(" and ");
                sb.Append("StatusId = 1");
            }
            else
            {
                if (sb.Length > 0)
                    sb.Append(" and ");
                sb.Append("StatusId IN (1,2)");
            }

            if (sb.Length > 0)
            {
                sb.Insert(0, " where ");
            }
            return sb.ToString();
        }
        #endregion

        #region event handlers
        protected void Search_Click(object sender, EventArgs e)
        {
            BindData();
        }
        #endregion

        #region grid related methods
        protected void G_SortCommand(object source, GridSortCommandEventArgs e)
        {
            SortData(e.SortExpression);
            BindData();
        }
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            int companyid = -1;
            if (cmd != Constants.SORTCOMMAND)
            {
                companyid = int.Parse(e.CommandArgument.ToString());
            }
            switch (cmd)
            {
                case CHANGESTATUSCOMMAND:
                    Company.ChangeStatus(companyid);
                    break;
                case Constants.EDITCOMMAND:
                    c = new Company(companyid);
                    CurrentPage.StoreObject(c, Constants.COMPANYOBJECT);
                    Response.Redirect(GetEditCompanyLink());
                    break;
                case ORIGINATORLOGIN:
                    c = new Company(companyid);
                    CurrentUser.OriginatorId = companyid;
                    CurrentUser.OriginatorName = c.Name;
                    CurrentPage.StoreObject(c, Constants.COMPANYOBJECT);
                    Response.Redirect(ResolveUrl("~/" + Constants.ADMINPAGENAME));
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
                    int companyid = int.Parse(row[Company.IDFIELDNAME].ToString());
                    bool isActive =int.Parse(row["statusid"].ToString())== ENABLEDSTATUSID;
                    ImageButton imgbutton = (ImageButton)e.Item.Cells[4].Controls[3];
                    imgbutton.Enabled = !(companyid == 1);
                    imgbutton.ImageUrl = "~" + Constants.IMAGEFOLDER + "/" + Constants.DELETEBUTTONIMG + (imgbutton.Enabled ? "" : "_dis") + ".gif";
                    imgbutton.AlternateText = isActive ? "Disable" : "Enable";
                    if(!isActive)
                    {
                        Label lbl = (Label)e.Item.FindControl("lblCompany");
                        if(lbl!=null)
                        {
                            lbl.ForeColor = System.Drawing.Color.Gray;
                        }
                    }
                    if(companyid==1)
                    {
                        LinkButton lb = (LinkButton)e.Item.FindControl("originatorlogin");
                        if (lb != null)
                        {
                            lb.Visible = false;
                        }
                    }
                }
            }
        }
        protected void G_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            G.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }
        #endregion

        protected void cbArchived_CheckedChanged(object sender, EventArgs e)
        {
            BindData();
        }
    }
}