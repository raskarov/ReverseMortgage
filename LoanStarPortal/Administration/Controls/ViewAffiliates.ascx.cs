using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class ViewAffiliates : AppControl
    {

        #region constants
        private const string EDITITEMID = "affiliateid";
        private const string AFFILIATEGRIDMODE = "affiliategridmode";
        private const string AFFILIATECOMPANY = "affiliatecompany";
        private const string MANAGEAFFILIATEHEADER = "Manage affiliates for {0}";
        private const int MODEVIEW = 0;
        private const int MODEEDIT = 1;
        private const int MODEADD = 2;
        private const string NOTSELECTED = "- Select -";
        private const int NOTSELECTEDVALUE = 0;
        private const string ONCLICKATTRIBUTE = "OnClick";
        private const string JSDELETECONFIRM = "if(!confirm('Are you sure you want to delete this company?'))return false;";
        #endregion

        #region fields
        private Company c;
        private DataView dvRoles;
        private DataView dvCompany;
        #endregion

        #region properties
        protected bool ViewMode
        {
            get { return GridMode == MODEVIEW; }
        }
        protected int GridMode
        {
            get
            {
                int res = MODEVIEW;
                Object o = ViewState[AFFILIATEGRIDMODE];
                if(o!=null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set { ViewState[AFFILIATEGRIDMODE] = value; }
        }
        protected DataView DvRoles
        {
            get
            {
                if(dvRoles==null)
                {
                    dvRoles = Company.GetAffiliatesRoleList();
                }
                return dvRoles;
            }
        }
        protected DataView DvCompany
        {
            get
            {
                if(dvCompany==null)
                {
//                    dvCompany = Company.GetAffiliateList();
                    dvCompany = c.GetAffiliatesCandidate(AffiliateCompany.AffiliatecompanyTypeId);
                }
                return dvCompany;
            }
        }
        protected CompanyAffiliate AffiliateCompany
        {
            get
            {
                CompanyAffiliate res = Session[AFFILIATECOMPANY] as CompanyAffiliate;
                if(res==null)
                {
                    res = new CompanyAffiliate(EditItemId);
                    Session[AFFILIATECOMPANY] = res;
                }
                return res;
            }
            set { Session[AFFILIATECOMPANY] = value; }
        }
        protected int EditItemId
        {
            get
            {
                int res = -1;
                Object o = Session[EDITITEMID];
                if(o!=null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set { Session[EDITITEMID] = value; }
        }
        #endregion

        #region methods
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            c = CurrentPage.GetObject(Constants.COMPANYOBJECT) as Company;
            if (c == null)
            {
                c = new Company();
            }
            if (!IsPostBack)
            {
                BindData();
            }
        }
        private void BindData()
        {
            lblCompany.Text = String.Format(MANAGEAFFILIATEHEADER, c.Name);
            BindGrid();
        }
        protected void BindGrid()
        {
            G.DataSource = c.GetAffiliates();
            G.DataBind();
            G.Columns[2].Visible = ViewMode;
            G.Columns[3].Visible = !G.Columns[2].Visible;
        }
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "InitInsert")
            {
                GridMode = MODEADD;
                EditItemId = -1;
                AffiliateCompany.CompanyId = c.ID;
            }
            else if(e.CommandName=="Edit")
            {
                AffiliateCompany = null;
                GridMode = MODEEDIT;
                EditItemId = int.Parse(e.CommandArgument.ToString());
            }
            else if(e.CommandName=="cancel")
            {
                EditItemId = -1;
                GridMode = MODEVIEW;
                AffiliateCompany = null;
            }
            else if (e.CommandName == "update")
            {
                DropDownList ddl = e.Item.FindControl("ddlCompany") as DropDownList;
                if(ddl!=null)
                {
                    AffiliateCompany.AffiliateCompanyId = int.Parse(ddl.SelectedValue);
                }
                AffiliateCompany.Save();
                EditItemId = -1;
                GridMode = MODEVIEW;
                AffiliateCompany = null;
            }
            else if (e.CommandName == "deleteaffiliate")
            {
                CompanyAffiliate.Delete(int.Parse(e.CommandArgument.ToString()));
            }
            BindGrid();
        }
        protected void G_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if(GridMode!=MODEVIEW)
            {
                if(e.Item is GridDataItem)
                {
                    GridEditableItem item = (GridEditableItem)e.Item;
                    if (e.Item.IsInEditMode)
                    {
                        DropDownList ddl = (DropDownList)item.FindControl("ddlRole");
                        if (ddl != null)
                        {
                            ddl.DataSource = DvRoles;
                            ddl.DataTextField = "Name";
                            ddl.DataValueField = "id";
                            ddl.DataBind();
                            ddl.SelectedValue = AffiliateCompany.AffiliatecompanyTypeId.ToString();
                        }
                        ddl = (DropDownList)item.FindControl("ddlCompany");
                        if (ddl != null)
                        {
                            DataView dv = DvCompany;
                            dv.RowFilter = GetFilter();
                            ddl.DataSource = dv;
                            ddl.DataTextField = "Company";
                            ddl.DataValueField = "id";
                            ddl.DataBind();
                            AddEmptyItem(ddl);
                            ddl.SelectedValue = AffiliateCompany.AffiliateCompanyId.ToString();
                            if (AffiliateCompany.AffiliatecompanyTypeId == 0)
                            {
                                ddl.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        ImageButton btn = (ImageButton)item.FindControl("btnUpdate");
                        if(btn!=null)
                        {
                            btn.Visible = false;
                        }
                        btn = (ImageButton)item.FindControl("btnCancel");
                        if (btn != null)
                        {
                            btn.Visible = false;
                        }
                    }
                }
            }
            else
            {
                if(e.Item is GridDataItem)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    ImageButton btn = (ImageButton)item.FindControl("btnDelete");
                    if (btn != null)
                    {
                        btn.Attributes.Add(ONCLICKATTRIBUTE, JSDELETECONFIRM);
                    }
                }
            }
        }
        private string GetFilter()
        {
            string res = "";
            switch(AffiliateCompany.AffiliatecompanyTypeId)
            {
                case (int)CompanyAffiliate.AffiliateType.Originator:
                    res = "IsOriginator=1";
                    break;
                case (int)CompanyAffiliate.AffiliateType.Lender :
                    res = "IsLender=1";
                    break;
                case (int)CompanyAffiliate.AffiliateType.Servicer:
                    res = "IsServicer=1";
                    break;
                case (int)CompanyAffiliate.AffiliateType.Investor:
                    res = "IsInvestor=1";
                    break;
                case (int)CompanyAffiliate.AffiliateType.Trustee:
                    res = "IsTrustee=1";
                    break;

            }
            //if (!String.IsNullOrEmpty(res))
            //{
            //    res = "id<>" + c.ID+ " and "+res;
            //}
            return res;
        }
        private static void AddEmptyItem(ListControl ddl)
        {
            ddl.Items.Insert(0, new ListItem(NOTSELECTED, NOTSELECTEDVALUE.ToString()));
        }
        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            AffiliateCompany.AffiliatecompanyTypeId = ((DropDownList) sender).SelectedIndex;
            BindGrid();
        }
        #endregion
    }
}