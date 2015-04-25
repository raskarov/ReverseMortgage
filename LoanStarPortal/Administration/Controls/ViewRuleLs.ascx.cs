using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;
using Rule=LoanStar.Common.Rule;

namespace LoanStarPortal.Administration.Controls
{
    public partial class ViewRuleLs : AppControl
    {
        #region constants
        private const string COMPANYID = "currentcompanyid";
        private const string ADDLABEL =  "Add";
        private const string EDITLABLE = "Edit";
        private const bool ALL = true;
        private const string CONTROLMODE = "CONTROLMODE";
        private const string PRODUCTNAMEFIELDNAME = "name";
        private const string PRODUCTIDFIELDNAME = "id";
        private const string NAMEFIELDNAME = "Name";
        private const string CATEGORYIDFIELDNAME = "categoryid";
        private static readonly string[] CONTROLS = {"editrulecode","editrulefield","editrulechecklist","editrulecondition"
                                            ,"editrulealert","editruledocument","editruledata","editruletask"};
        private const string DIVCONTROL = "div";
        private const int VIEWGRIDMODE = -1;
        private const int FIRSTLINKBUTTONCELL = 6;
        private static readonly string[] fieldNames = { "codeunit", "field", "condition", "task", "document", "checklist", "alert", "data" };
        private const string  ENABLED = "Enabled";
        private const string  DISABLED = "Disabled";
        private const string NOTSELECTEDTEXT = "- Select - ";
        private const int NOTSELECTEDVALUE = 0;
        private const int LENDERSPECIFICCOLUMN = 2;
        private const string GRIDFILTER = "gridfilter";
        private const string ADDCHILDRULECOMMAND = "addchild";
//        private const string GRIDSORTORDER = "gridsortorder";
        #endregion

        #region fields
        private Rule rule;
        private bool needFullInit;
        //private string orderby = String.Empty;
        //private string whereclause = String.Empty;
        #endregion

        #region property
        protected int ViewMode
        {
            get
            {
                int res = VIEWGRIDMODE;
                Object o = ViewState[CONTROLMODE];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch { }
                }
                else
                {
                    ViewState[CONTROLMODE] = res;
                }
                return res;
            }
            set 
            {
                ViewState[CONTROLMODE] = value;
            }
        }
        protected int CurrentLenderId
        {
            get
            {
                int res = CurrentUser.EffectiveCompanyId;
                Object o = ViewState[COMPANYID];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set
            {
                ViewState[COMPANYID] = value;
            }
        }
        protected string GridFilter
        {
            get
            {
                string res = String.Empty;
                Object o = ViewState[GRIDFILTER];
                if (o != null)
                {
                    res = o.ToString();
                }
                return res;
            }
            set { ViewState[GRIDFILTER] = value; }
        }
        //protected string SortOrder
        //{
        //    get
        //    {
        //        string res = String.Empty;
        //        Object o = ViewState[GRIDSORTORDER];
        //        if (o != null)
        //        {
        //            res = o.ToString();
        //        }
        //        return res;
        //    }
        //    set { ViewState[GRIDSORTORDER] = value; }
        //}
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            //if (IsPostBack)
            //{
            //    whereclause = GetFilter();
            //}
            rule = GetRule();
            SetSelect();
            if (!Page.IsPostBack)
            {
                CurrentLenderId = CurrentUser.EffectiveCompanyId;
                BindData();
                ViewMode = VIEWGRIDMODE;
            }
            InitControls();
        }

        #region methods
        //private string GetFilter()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    AppendTextBoxCondition(sb, tbRuleName.Text, NAMEFIELDNAME);
        //    AppendSelectCondition(sb, ddlCategory.SelectedValue, CATEGORYIDFIELDNAME);
        //    if (sb.Length > 0)
        //    {
        //        sb.Insert(0, " where ");
        //    }
        //    return sb.ToString();
        //}
        private void SetSelect()
        {
            tdSelectCompany.Visible = CurrentUser.IsLoanStarAdmin&&(!CurrentUser.LoggedAsOriginator);
            tdNoSelect.Visible = !tdSelectCompany.Visible;
        }
        private void InitControls()
        {
            for (int i=0; i < CONTROLS.Length; i++)
            {
                Control ctl = FindControl(DIVCONTROL+CONTROLS[i]);
                ctl.Visible = false;
            }
            trview.Visible = (ViewMode == VIEWGRIDMODE);
            tredit.Visible = !trview.Visible;
            if (tredit.Visible)
            {
                string controlName = CONTROLS[ViewMode];
                RuleControl rc = FindControl(controlName) as RuleControl;
                if (rc != null)
                {
                    if (needFullInit)
                    {
                        rc.Initialize();
                    }
                    rc.OnBack += ViewRuleLs_OnBack;
                    rc.OnDataChange += ViewRuleLs_OnDataChange;
                }
                Control divctl = FindControl(DIVCONTROL + controlName);
                divctl.Visible = true;
            }
        }
        private void ViewRuleLs_OnDataChange()
        { 
            BindGrid();
        }
        private void ViewRuleLs_OnBack()
        {
            ViewMode = VIEWGRIDMODE;
            InitControls();
        }
        private int GetSelectedCompanyId()
        {
            int res = CurrentUser.EffectiveCompanyId;
            try
            {
                res = int.Parse(ddlCompany.SelectedValue);
                if (res == 0)
                {
                    res = CurrentUser.EffectiveCompanyId;
                }
            }
            catch
            {
            }
            return res;
        }
        private void BindData()
        {
            BindCompany();
            BindProduct();
            BindCategory();
            BindGrid();
        }
        private void BindCategory()
        {
            ddlCategory.DataSource = Rule.GetCategory();
            ddlCategory.DataTextField = Rule.NAMEFIELDNAME;
            ddlCategory.DataValueField = Rule.IDFIELDNAME;
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem(NOTSELECTEDTEXT,NOTSELECTEDVALUE.ToString()));
        }
        private void BindProduct()
        {
            ddlProduct.DataSource = Product.GetProductList(ALL);
            ddlProduct.DataTextField = PRODUCTNAMEFIELDNAME;
            ddlProduct.DataValueField = PRODUCTIDFIELDNAME;
            ddlProduct.DataBind();
        }
        private void BindCompany()
        {
            if (tdSelectCompany.Visible)
            {
                ddlCompany.DataSource = Company.GetLenderList(ALL);
                ddlCompany.DataTextField = Company.COMPANYFIELDNAME;
                ddlCompany.DataValueField = Company.IDFIELDNAME;
                ddlCompany.DataBind();
            }
        }
        private void BindGrid()
        {
            int productid = 0;
            try
            {
                productid = Convert.ToInt32(ddlProduct.SelectedValue);
            }
            catch
            {
            }
            bool showgeneral = CurrentUser.IsCorrespondentLenderAdmin ||
                               (CurrentUser.IsLoanStarAdmin && CurrentUser.LoggedAsOriginator);
            DataView dv = Rule.GetRuleList(CurrentLenderId, productid, showgeneral);
            dv.RowFilter = GridFilter;
//            dv.Sort = SortOrder;
            G.DataSource = dv;
            G.DataBind();
            G.MasterTableView.Columns[LENDERSPECIFICCOLUMN].Visible = CurrentLenderId != Constants.LOANSTARCOMPANYID;
        }
        //private void SortData(string sortexpr)
        //{
        //    string sortorder = GetSortOrder(sortexpr);
        //    if (sortorder == "asc")
        //    {
        //        sortorder = "desc";
        //    }
        //    else
        //    {
        //        sortorder = "asc";
        //    }
        //    SetSortOrder(sortexpr, sortorder);
        //    orderby = "order by " + sortexpr + " " + sortorder;
        //}
        private Rule GetRule()
        {
            Rule r = CurrentPage.GetObject(Constants.RULEOBJECT) as Rule;
            if (r == null)
            {
                r = new Rule();
            }
            return r;
        }
        private void EditRule(string cmd)
        {
            int mode = VIEWGRIDMODE;
            for (int i = 0; i < CONTROLS.Length; i++)
            {
                if (CONTROLS[i] == cmd)
                {
                    mode = i;
                    break;
                }
            }
            ViewMode = mode;
            needFullInit = true;
            InitControls();
        }
        #endregion

        #region grid related
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            if (cmd == Constants.SORTCOMMAND)
            {
                BindGrid();
                return;
            }
            int id = int.Parse(e.CommandArgument.ToString());
            if(cmd==ADDCHILDRULECOMMAND)
            {
                rule = new Rule(-1);
                rule.CompanyId = CurrentUser.EffectiveCompanyId;
                rule.ParentId = id;
                CurrentPage.StoreObject(rule, Constants.RULEOBJECT);
                EditRule(CONTROLS[0]);
            }
            else
            {
                rule = new Rule(id);
                if (rule.ID > 0)
                {
                    if (rule.CompanyId != CurrentUser.EffectiveCompanyId)
                    {
                        rule.IsCopyFromGeneralRule = true;
                        rule.CompanyId = CurrentUser.EffectiveCompanyId;
                    }
                }
                else
                {
                    rule.CompanyId = CurrentUser.EffectiveCompanyId;
                }
                CurrentPage.StoreObject(rule, Constants.RULEOBJECT);
                if (cmd == Constants.ENABLECOMMAND)
                {
                    rule.Enable();
                    BindGrid();
                }
                else if (cmd == Constants.DISABLECOMMAND)
                {
                    rule.Disable();
                    BindGrid();
                }
                else
                {
                    EditRule(cmd);
                }
            }
        }
        protected void G_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row != null)
                {
                    LinkButton lnkbutton = (LinkButton)e.Item.Cells[FIRSTLINKBUTTONCELL + 8].Controls[1];
                    if (int.Parse(row[Rule.STATUSIDFIELDNAME].ToString()) == Constants.ENABLEDSTATUSID)
                    {
                        lnkbutton.CommandName = Constants.DISABLECOMMAND;
                        lnkbutton.Text=ENABLED;
                        lnkbutton.ForeColor = Color.Green;
                    }
                    else
                    {
                        lnkbutton.CommandName = Constants.ENABLECOMMAND;
                        lnkbutton.Text=DISABLED;
                        lnkbutton.ForeColor = Color.DarkGray;
                    }
                    for (int i = 0; i < fieldNames.Length; i++)
                    {
                        SetLinkButton(e,int.Parse(row[fieldNames[i]].ToString()), i);
                    }
                }
            }
        }
        private static void SetLinkButton(GridItemEventArgs e,int cnt, int i)
        {
            LinkButton lnkbutton = (LinkButton)e.Item.Cells[FIRSTLINKBUTTONCELL+i].Controls[1];
            if (cnt == 0)
            {
                lnkbutton.Text = ADDLABEL;
                lnkbutton.ForeColor = Color.DarkGray;
            }
            else
            {
                lnkbutton.Text = EDITLABLE;
                lnkbutton.ForeColor = Color.Red;
            }
        }
        protected static string GetLenderSpecific(Object o, string fieldname)
        {
             DataRowView row = (DataRowView)o;
             if (row != null)
             {
                 return int.Parse(row[fieldname].ToString()) == 1 ? "Yes" : "No";
             }
             return String.Empty;
        }
        protected void G_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            G.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }
        //protected void G_SortCommand(object source, GridSortCommandEventArgs e)
        //{
        //    SortData(e.SortExpression);
        //    BindGrid();
        //}
        //protected static string GetDate(Object item, string name)
        //{
        //    DataRowView row = (DataRowView)item;
        //    string result = "n/a";
        //    if (!String.IsNullOrEmpty(row[name].ToString()))
        //    {
        //        result = ((DateTime)row[name]).ToShortDateString();
        //    }
        //    return result;
        //}
        #endregion

        #region event handlers
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentLenderId = GetSelectedCompanyId();
            rule.CompanyId = CurrentLenderId;
            CurrentPage.StoreObject(rule, Constants.RULEOBJECT);
            BindGrid();
        }
        protected void Search_Click(object sender, EventArgs e)
        {
            SetFilter();
            BindGrid();
        }
        private void SetFilter()
        {
            StringBuilder sb = new StringBuilder();
            AppendTextBoxCondition(sb, tbRuleName.Text, NAMEFIELDNAME);
            AppendSelectCondition(sb, ddlCategory.SelectedValue, CATEGORYIDFIELDNAME);
            if (sb.Length > 0)
            {
                sb.Insert(0, " ");
            }
            GridFilter = sb.ToString();
        }
        protected void lbAdd_Click(object sender, EventArgs e)
        {
            rule = new Rule(-1);
            rule.CompanyId = CurrentUser.EffectiveCompanyId;
            CurrentPage.StoreObject(rule, Constants.RULEOBJECT);
            EditRule(CONTROLS[0]);
        }
        #endregion
    }
}