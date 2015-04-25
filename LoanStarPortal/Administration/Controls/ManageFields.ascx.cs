using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration
{
    public partial class ManageFields : AppControl
    {
        #region constants
        private const string FIELDGROUPTABLE = "vwFieldGroup";
        private const string NOTSELECTEDTEXT = "- Select - ";
        private const int NOTSELECTEDVALUE = 0;
        private const string FIELDGRIDFILTER = "fieldfilter";
        private const string GROUPFILTER = "FieldGroupId={0}";
        private const string FILEDFILTER = "id={0}";
        public const string TABFILTER = "tablevel2id={0}";
        public const string PSEUDOTABFILTER = "tabid={0}";
        #endregion

        #region fields
        private int groupId = -1;
        private int fieldId = -1;
        private int tab1Id = -1;
        private int tab2Id = -1;
        private int pseudoTabId = -1;
        #endregion

        #region properties
        protected string GridFilter
        {
            get
            {
                string res = String.Empty;
                Object o = Session[FIELDGRIDFILTER];
                if(o!=null)
                {
                    res = o.ToString();
                }
                else
                {
                    Session[FIELDGRIDFILTER] = res;
                }
                return res;
            }
            set
            {
                Session[FIELDGRIDFILTER] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsLoanStarAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        #region methods
        private void GetDdlValues()
        {
            groupId = GetDdlValue(ddlGroup);
            fieldId = GetDdlValue(ddlField);
            tab1Id = GetDdlValue(ddlTab1);
            tab2Id = GetDdlValue(ddlTab2);
            pseudoTabId = GetDdlValue(ddlPseudoTab);
        }
        private static int GetDdlValue(ListControl ddl)
        {
            int res = -1;
            if(ddl!=null)
            {
                try
                {
                    res = int.Parse(ddl.SelectedValue);
                }
                catch
                {
                }
            }
            return res;
        }
        private void BindData()
        {
            BindGrid();
            BindGroup();
            BindField();
            BindTab1();
            BindTab2();
            BindPseudoTab();
        }
        private void BindGrid()
        {
            DataView dv = MortgageProfileFieldInfo.GetFieldsInfo();
            dv.RowFilter = GridFilter;
            G.DataSource = dv;
            G.DataBind();
        }
        private void BindField()
        {
            if(groupId>0)
            {
                DataView dv = MortgageProfileFieldInfo.GetFieldInGroup(groupId);
                dv.Sort = "fieldname";
                ddlField.DataSource = dv;
                ddlField.DataTextField = "fieldname";
                ddlField.DataValueField = "id";
                ddlField.DataBind();
                ddlField.Enabled = true;
            }
            else
            {
                ddlField.Enabled = false;
            }
            AddEmptyItem(ddlField);
        }
        private void BindGroup()
        {
            DataView dv = CurrentPage.GetDictionary(FIELDGROUPTABLE);
            dv.Sort = Field.GROUPNAMEFIELDNAME;
            ddlGroup.DataSource = dv;
            ddlGroup.DataTextField = Field.GROUPNAMEFIELDNAME;
            ddlGroup.DataValueField = Field.IDFIELDNAME;
            ddlGroup.DataBind();
            AddEmptyItem(ddlGroup);
        }
        private void BindTab1()
        {
            ddlTab1.DataSource = MortgageProfileFieldInfo.GetTopLevelTab();
            ddlTab1.DataTextField = "TabLevel2Name";
            ddlTab1.DataValueField = "id";
            ddlTab1.DataBind();
            AddEmptyItem(ddlTab1);
        }
        private void BindTab2()
        {
            if(tab1Id>0)
            {
                DataView dv = MortgageProfileFieldInfo.GetSecondLevelTabs();
                dv.RowFilter = String.Format(TABFILTER, tab1Id);
                ddlTab2.DataSource = dv;
                ddlTab2.DataTextField = "Header";
                ddlTab2.DataValueField = "id";
                ddlTab2.DataBind();
                ddlTab2.Enabled = true;
            }
            else
            {
                ddlTab2.Enabled = false;
            }
            AddEmptyItem(ddlTab2);
        }
        private void BindPseudoTab()
        {
            if(tab2Id>0)
            {
                DataView dv = MortgageProfileFieldInfo.GetPseudoTabs();
                dv.RowFilter = String.Format(PSEUDOTABFILTER, tab2Id);
                ddlPseudoTab.DataSource = dv;
                ddlPseudoTab.DataTextField = "header";
                ddlPseudoTab.DataValueField = "id";
                ddlPseudoTab.DataBind();
                ddlPseudoTab.Enabled = true;
            }
            else
            {
                ddlPseudoTab.Enabled = false;
            }
            AddEmptyItem(ddlPseudoTab);
        }
        private static void AddEmptyItem(ListControl ddl)
        {
            ddl.Items.Insert(0, new ListItem(NOTSELECTEDTEXT, NOTSELECTEDVALUE.ToString()));
        }
        private void SetFilter()
        {
            string filter1 = String.Empty;
            if(groupId>0)
            {
                filter1 += String.Format(GROUPFILTER, groupId);
                if (fieldId > 0)
                {
                    filter1 = String.Format(FILEDFILTER, fieldId);
                }
            }
            string filter2 = String.Empty;
            if(tab1Id>0)
            {
                filter2 = String.Format("tab1id={0}", tab1Id);
                if(tab2Id>0)
                {
                    filter2 = String.Format("tab2id={0}", tab2Id);
                    if(pseudoTabId>0)
                    {
                        filter2 = String.Format("pseudotabid={0}", pseudoTabId);
                    }
                }
            }
            string filter3 = String.Empty;
            if (!String.IsNullOrEmpty(tbLabel.Text))
            {
                filter3 = "description like '%" + tbLabel.Text + "%'";
            }
            string filter = filter1;
            if(!String.IsNullOrEmpty(filter))
            {
                if(!String.IsNullOrEmpty(filter2))
                {
                    filter += " and ";
                }
            }
            filter += filter2;
            if (!String.IsNullOrEmpty(filter))
            {
                if (!String.IsNullOrEmpty(filter3))
                {
                    filter += " and ";
                }
            }
            filter += filter3;
            GridFilter = filter;
        }

        #endregion

        #region events
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            if(e.CommandName.ToLower()==Constants.EDITCOMMAND)
            {
                Session["editfieldinit"] = false; 
                Response.Redirect(ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.VIEWFIELDDETAILS + "&" + Constants.IDPARAM + "=" + e.CommandArgument));
            }
        }

        protected void G_SortCommand(object source, GridSortCommandEventArgs e)
        {
            BindGrid();
        }
        protected void G_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            BindGrid();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetDdlValues();
            SetFilter();
            BindGrid();
        }
        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupId = int.Parse(ddlGroup.SelectedValue);
            BindField();
        }
        protected void ddlField_SelectedIndexChanged(object sender, EventArgs e)
        {
            fieldId = int.Parse(ddlField.SelectedValue);
        }
        protected void ddlTab1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tab1Id = int.Parse(ddlTab1.SelectedValue);
            BindTab2();
        }
        protected void ddlTab2_SelectedIndexChanged(object sender, EventArgs e)
        {
            tab2Id = int.Parse(ddlTab2.SelectedValue);
            BindPseudoTab();
        }
        protected static string GetId(Object o)
        {
            DataRowView item = (DataRowView)o;
            return item["id"] + "&gid=" + item["groupId"];
        }
        #endregion

        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {

        }
    }
}