using System;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class ViewMyAffiliate : AppControl
    {
        private const string SERVICEFEECOMMAND = "ServiceFee";

        private int roleId = 0;
        private string roleName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(CurrentUser.IsCorrespondentLenderAdmin || CurrentUser.LoggedAsOriginator))
            {
                Response.Redirect(ResolveUrl("~/" + CurrentUser.GetDefaultPage()));
            }
            string param = CurrentPage.GetValue(Constants.CONTROLPARAM,"");
            switch(param)
            {
                case Constants.VIEWMYLENDERS:
                    roleId = (int)CompanyAffiliate.AffiliateType.Lender;
                    roleName = "My lenders";
                    break;
                case Constants.VIEWMYORIGINATORS:
                    roleId = (int)CompanyAffiliate.AffiliateType.Originator;
                    roleName = "My originators";
                    break;
                case Constants.VIEWMYSERVICERS:
                    roleId = (int)CompanyAffiliate.AffiliateType.Servicer;
                    roleName = "My servicers";
                    break;
                case Constants.VIEWMYINVESTORS:
                    roleId = (int)CompanyAffiliate.AffiliateType.Investor;
                    roleName = "My investors";
                    break;
            }
            if(!IsPostBack)
            {
                BindData();
            }
        }
        private void BindData()
        {
            lblRole.Text = roleName;
            BindGrid();
        }
        private void BindGrid()
        {
            if(roleId!=0)
            {
                G.DataSource = Company.GetCompanyAffiliateByRole(CurrentUser.EffectiveCompanyId, roleId);
                G.DataBind();
                G.MasterTableView.Columns[1].Visible = roleId == (int)CompanyAffiliate.AffiliateType.Lender;
            }
        }
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == SERVICEFEECOMMAND)
            { 
                Response.Redirect(GetServicefeeLink(e.CommandArgument.ToString()));
            }
        }
        private string GetServicefeeLink(string arg)
        {
            return ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.EDITCOMPANYSERVICEFEE+"&id="+arg);
        }
    }
}