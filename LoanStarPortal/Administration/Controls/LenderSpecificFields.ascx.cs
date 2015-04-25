using System;
using System.Data;
using System.Xml;
using System.Web.UI.WebControls;
using Telerik.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class LenderSpecificFields : AppControl
    {
        #region constants
        private const string STATEDICTIONARYTABLE = "vwState";
        private const string STATEEXTDICTIONARYTABLE = "vwStateExt";
        private const string DBERROR = "Can't save data to db";
        private const string STATELINKID = "lbState";
        private const string CURRENTORIGINATORSTATEID = "currentoriginatorstateid";
        private const string CURRENTORIGINATORSTATE = "currentoriginatorstate";
        private const string EDITITEM = "edititemid";
        private const string EDITLICENSINGITEM = "edititemlicensingid";
        private const string ADDCOMMAND = "Add";
        private const int GRIDVIEWMODE = 0;
        private const int GRIDEDITMODE = 1;
        private const int GRIDADDMODE = 2;
        private const string LENDERFHAIDGRIDMODE = "settingsgridmode";
        private const string ORIGINATORLICENSEGRIDMODE = "originatorlicensegridmode";
        private const string EDITROW = "editrowid";
        private const string EDITLICENSINGROW = "eidtlicensingrow";
        private const string EDITCOMMAND = "Edit";
        private const string UPDATECOMMAND = "Update";
        private const string CANCELCOMMAND = "Cancel";
        private const string SORTCOMMAND = "Sort";
        private const string PAGECOMMAND = "Page";
        private const string ONCLICK = "onclick";
        private const string DELETEJS = "javascript:{{var r=confirm('Delete this record?');if (!r)return false;}};";
        private const string EMPTYTEXT = "No records to display";
        private const string DELETECOMMAND = "Delete";
        private const string ASCENDING = "asc";
        private const string DESCENDING = "desc";
        private const string SORTEXPRESSION = "lenderfhasort";
        private const string SORTEXPRESSIONLICENSING = "originatorstatelicensingsort";
        
        private const string SETTINGSPAGEINDEX = "lenderfhapageindex";
        private const string LICENSINGPAGEINDEX = "licensingpageindex";
        private const string PRODUCTCHECKBOXID = "cbProduct";
        private const string CHECKFIELDJS = "CheckProduct(this,'{0}','{1}');";
        private const string CHECKALLJS = "CheckAllProducts(this,'{0}');";
        private const string ONCLICKATTRIBUTE = "onclick";
        private const string JSCBSHOWROW = "SetClosingInfoVisibility(this.checked,'{0}');";
        private const string STYLEATTRIBUTE = "style";
        private const string TRVISIBLE = "display:inline";
        private const string TRHIDDEN = "display:none";
        private const string ROOTELEMENT = "Root";
        private const string ITEMELEMENT = "item";
        private const string IDATTRIBUTE = "id";
        private const string PRODUCTIDATTRIBUTE = "productid";

        #endregion

        #region fields
        private Lender lsp;
        private Originator originator;
        private Investor investor;
        private Trustee trustee;
        private Servicer servicer;
        private DataView dvStateExt = null;
        private int roles=-1;
        private int stateTab = 0;
        private DataView dvProducts;
        private DataView dvStateLicense;
        private bool allProductsSelected = false;
        private int currentRow = 0;
        private int currentRowLicense = 0;
        private bool showFooter = true;
        private bool showFooterLicensing = true;
        #endregion

        #region properties
        protected DataView DvStateExt
        {
            get
            {
                if (dvStateExt == null)
                    dvStateExt = CurrentPage.GetDictionary(STATEEXTDICTIONARYTABLE);
                return dvStateExt;
            }
        }
        private DataView dvState = null;
        private DataView dvStateUS = null;
        private DataView dvLocation = null;
        private DataView DvLocation
        {
            get
            {
                if (dvLocation == null)
                {
                    dvLocation = LoanStar.Common.CompanyLocation.GetCompanyLocationList(CurrentUser.EffectiveCompanyId);
                }
                
                return dvLocation;
            }
        }
        protected DataView DvStateUS
        {
            get
            {
                if (dvStateUS == null)
                    dvStateUS = DataHelpers.GetStateListUS();
                return dvStateUS;
            }
        }
        protected DataView DvState
        {
            get
            {
                if (dvState == null)
                    dvState = CurrentPage.GetDictionary(STATEDICTIONARYTABLE);
                return dvState;
            }
        }
        private DataView dvOriginatorStates = null;
        protected DataView DvOriginatorStates
        {
            get
            {
                if(dvOriginatorStates==null)
                {
                    dvOriginatorStates = originator.GetStates();
                }
                return dvOriginatorStates;
            }
        }
        protected int Roles
        {
            get 
            {
                if(roles < 0)
                {
                    roles = Company.GetRolesMask(CurrentUser.EffectiveCompanyId);
                }
                return roles;
            }
        }
        private string CurrentOriginatorState
        {
            get
            {
                string res = "";
                try
                {
                    Object o = ViewState[CURRENTORIGINATORSTATE];
                    if(o!=null)
                    {
                        res = o.ToString();
                    }
                }
                catch
                {
                }
                return res;
            }
            set { ViewState[CURRENTORIGINATORSTATE] = value; }
        }
        private int CurrentOriginatorStateId
        {
            get
            {
                int res = -1;
                try
                {
                    Object o = ViewState[CURRENTORIGINATORSTATEID];
                    if(o!=null)
                    {
                        res = int.Parse(o.ToString());
                    }
                }
                catch
                {
                }
                return res;
            }
            set { ViewState[CURRENTORIGINATORSTATEID] = value; }
        }
        protected int EditItemId
        {
            get
            {
                int res = -1;
                Object o = ViewState[EDITITEM];
                if (o != null)
                {
                    try
                    {
                        res = Convert.ToInt32(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set
            {
                ViewState[EDITITEM] = value;
            }
        }
        protected int EditItemLicensingId
        {
            get
            {
                int res = -1;
                Object o = ViewState[EDITLICENSINGITEM];
                if (o != null)
                {
                    try
                    {
                        res = Convert.ToInt32(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set
            {
                ViewState[EDITLICENSINGITEM] = value;
            }
        }
        
        private int GridMode
        {
            get
            {
                int res = GRIDVIEWMODE;
                Object o = ViewState[LENDERFHAIDGRIDMODE];
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
            set { ViewState[LENDERFHAIDGRIDMODE] = value; }
        }
        private int GridModeLicense
        {
            get
            {
                int res = GRIDVIEWMODE;
                Object o = ViewState[ORIGINATORLICENSEGRIDMODE];
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
            set { ViewState[ORIGINATORLICENSEGRIDMODE] = value; }
        }
        protected int EditRowId
        {
            get
            {
                int res = -1;
                Object o = ViewState[EDITROW];
                if (o != null)
                {
                    try
                    {
                        res = Convert.ToInt32(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set { ViewState[EDITROW] = value; }
        }
        protected int EditRowLicensingId
        {
            get
            {
                int res = -1;
                Object o = ViewState[EDITLICENSINGROW];
                if (o != null)
                {
                    try
                    {
                        res = Convert.ToInt32(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set { ViewState[EDITLICENSINGROW] = value; }
        }
        private int PageIndex
        {
            get
            {
                int res = 0;
                Object o = ViewState[SETTINGSPAGEINDEX];
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
                ViewState[SETTINGSPAGEINDEX] = value;
            }
        }
        private int PageIndexLicensing
        {
            get
            {
                int res = 0;
                Object o = ViewState[LICENSINGPAGEINDEX];
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
                ViewState[LICENSINGPAGEINDEX] = value;
            }
        }
        
        private string SortExpression
        {
            get
            {
                string res = String.Empty;
                Object o = ViewState[SORTEXPRESSION];
                if (o != null)
                {
                    res = o.ToString();
                }
                return res;
            }
            set
            {
                ViewState[SORTEXPRESSION] = value;
            }
        }
        private string SortExpressionLicensing
        {
            get
            {
                string res = String.Empty;
                Object o = ViewState[SORTEXPRESSIONLICENSING];
                if (o != null)
                {
                    res = o.ToString();
                }
                return res;
            }
            set
            {
                ViewState[SORTEXPRESSIONLICENSING] = value;
            }
        }
        private DataView DvProducts
        {
            get
            {
                if (dvProducts == null)
                {
                    dvProducts = originator.GetProductListForClosing();
                }
                return dvProducts;
            }
        }
        private DataView DvStateLicense
        {
            get
            {
                if(dvStateLicense==null)
                {
                    dvStateLicense = OriginatorStateLicense.GetStateLicenseList(originator.ID);
                }
                return dvStateLicense;
            }
        }
        #endregion

        #region methods
        private void BindData()
        {
            if ((Roles & Company.ISLENDERBIT) != 0)
            {
                BindLenderData();
            }
            if ((Roles & Company.ISORIGINATORBIT) != 0)
            {
                BindOriginatorData();
            }
            if ((Roles & Company.ISINVESTORBIT) != 0)
            {
                BindInvestorData();
            }
            if ((Roles & Company.ISTRUSTEEBIT) != 0)
            {
                BindTrusteeData();
            }
            if ((Roles & Company.ISSERVICERBIT) != 0)
            {
                BindServicerData();
            }
        }
        private void SetTabVisibility()
        {
            if ((Roles & Company.ISLENDERBIT) == 0)
            {
                TabsMortgageProfiles.Tabs.Remove(tabLender);
                RadMultiPage1.PageViews.Remove(pvLender);
            }
            if ((Roles & Company.ISORIGINATORBIT) == 0)
            {
                TabsMortgageProfiles.Tabs.Remove(tabOriginator);
                RadMultiPage1.PageViews.Remove(pvOriginator);
            }
            if ((Roles & Company.ISSERVICERBIT) == 0)
            {
                TabsMortgageProfiles.Tabs.Remove(tabServicer);
                RadMultiPage1.PageViews.Remove(pvServicer);
            }
            if ((Roles & Company.ISINVESTORBIT) == 0)
            {
                TabsMortgageProfiles.Tabs.Remove(tabInvestor);
                RadMultiPage1.PageViews.Remove(pvInvestor);
            }
            if ((Roles & Company.ISTRUSTEEBIT) == 0)
            {
                TabsMortgageProfiles.Tabs.Remove(tabTrustee);
                RadMultiPage1.PageViews.Remove(pvTrustee);
            }
        }
        private void SetRadioButton(string name, int id)
        {
            RadioButton rb = (RadioButton)FindControl(name + id);
            if (rb != null)
            {
                rb.Checked = true;
            }
        }
        private static void BindDdl(ListControl ddl, DataView dv, int selectedValue)
        {
            ddl.DataSource = dv;
            ddl.DataTextField = "Name";
            ddl.DataValueField = "id";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("- Select -", "0"));
            if (selectedValue > 0)
            {
                ddl.SelectedValue = selectedValue.ToString();
            }
        }
        private int GetRadioButton(string name, int cnt)
        {
            for (int i = 1; i <= cnt; i++)
            {
                RadioButton rb = (RadioButton)FindControl(name + i);
                if ((rb != null) && rb.Checked)
                {
                    return i;
                }
            }
            return 0;
        }
        #endregion

        #region lender tab
        private void BindLenderData()
        {
            lsp = new Lender(CurrentUser.EffectiveCompanyId);
            //tbAddress1.Text = lsp.Address1;
            //tbAddress2.Text = lsp.Address2;
            //tbCity.Text = lsp.City;
            //tbZip.Text = lsp.Zip;
            //tbName.Text = lsp.Name;
            tbPhone.Text = lsp.PhoneNumber;
            tbSponcorcode.Text = lsp.SponsoreAgentCode;
            SetRadioButton("rbassigned", lsp.HowManyAssignsId);
            SetRadioButton("rbIncludeAssignments", lsp.IncludesAssignmentsId);
            SetRadioButton("rbBlankInclude", lsp.BlankIncludesId);
//            BindDdl(ddlState, DvState, lsp.StateId);
            BindDdl(ddlLenderLocation, DvLocation, lsp.LocationId);
            tbdefaultMortNotInsured.Text = lsp.DefaultMortNotInsured.ToString();
            tbrecordedReturnTo.Text = lsp.RecordedReturnTo;
            tbwrittenStatementFromSecretaryNotElegibility.Text = lsp.WrittenStatementFromSecretaryNotElegibility.ToString();
            tbCorpHead.Text = lsp.CorpHead;
            BindDdl(ddlOperatesUnderJurisdiction, DvStateExt, 0);
            ddlOperatesUnderJurisdiction.SelectedValue = lsp.OperatesUnderJurisdictionID.ToString();
            rbNCClosedLoanSeller1.Checked = lsp.NCClosedLoanSeller;
            rbNCClosedLoanSeller2.Checked = !rbNCClosedLoanSeller1.Checked;
            tbClosingFaxNumber.Text = lsp.ClosingFaxNumber;
            tbLenderMortgageClause.Text = lsp.LenderMortgageClause;
            tbLenderLoginPage.Text = lsp.LoginPage;
            tbBaydocsLenderID.Text = lsp.BaydocsLenderID;
            tbBaydocsLenderCode.Text = lsp.BaydocsLenderCode;

            tbTitleCommitmentInsuredClause.Text = lsp.TitleCommitmentInsuredClause;
            tbRecordReturnToAddress.Text = lsp.RecordReturnToAddress;
            tbPlaceOfPaymentAddress.Text = lsp.PlaceOfPaymentAddress;
            tbRtrnFnlTtlPolAddress.Text = lsp.RtrnFnlTtlPolAddress;
            tbMortgageeClause.Text = lsp.MortgageeClause;
            tbLifeOfLoanClause.Text = lsp.LifeOfLoanClause;
            tbRightToCancelAddress.Text = lsp.RightToCancelAddress;
            tbAbbreviatedName.Text = lsp.AbbreviatedName;
            if (lsp.ID > 0)
            {
                BindLenderFHASponsorIdGrid();
            }
            else
            {
                trFHAGrid.Visible = false;
            }
        }
        private void BindLenderFHASponsorIdGrid()
        {
            currentRow = 0;
            DataView dv1 = lsp.GetFhaSponsorId();
            bool isEmpty = (dv1.Count == 1) && (Convert.ToInt32(dv1[0]["id"]) < 0);
            bool needInsert = !isEmpty && (GridMode == GRIDADDMODE);
            DataView dv;
            if (needInsert)
            {
                dv = GetDuplicate(dv1);
            }
            else
            {
                dv = dv1;
            }
            dv.Sort = GetSort();
            G.PageIndex = PageIndex;
            G.DataSource = dv;
            G.DataBind();
            showFooter = (GridMode == GRIDVIEWMODE);
            if (G.FooterRow != null) G.FooterRow.Visible = showFooter;
            if (GridMode != GRIDADDMODE)
            {
                if (isEmpty)
                {
                    int columns = G.Rows[0].Cells.Count;
                    G.Rows[0].Cells.Clear();
                    G.Rows[0].Cells.Add(new TableCell());
                    G.Rows[0].Cells[0].ColumnSpan = columns;
                    G.Rows[0].Cells[0].Text = EMPTYTEXT;
                }
            }
        }
        public bool Validate()
        {
            bool res = true;
            int rb = GetRadioButton("rbNCClosedLoanSeller", 2);
            if (rb == 0)
            {
                res = false;
                lblErrNCClosedLoanSeller.Text = "*";
            }
            //if (tbAddress1.Text == "")
            //{
            //    res = false;
            //    lblErrAddress1.Text = "*";
            //}
            //if (tbCity.Text == "")
            //{
            //    res = false;
            //    lblErrCity.Text = "*";
            //}
            //if (tbName.Text == "")
            //{
            //    res = false;
            //    lblErrName.Text = "*";
            //}
            if (tbPhone.Text == "")
            {
                res = false;
                lblErrPhone.Text = "*";
            }
            //if (tbZip.Text == "")
            //{
            //    res = false;
            //    lblErrZip.Text = "*";
            //}
            if (tbrecordedReturnTo.Text == "")
            {
                res = false;
                lblErrRecordedReturnTo.Text = "*";
            }
            if (tbCorpHead.Text == "")
            {
                res = false;
                lblErrCorpHead.Text = "*";
            }
            if (tbdefaultMortNotInsured.Text == "")
            {
                res = false;
                lblErrDefaultMortNotInsured.Text = "*";
            }
            if (tbwrittenStatementFromSecretaryNotElegibility.Text == "")
            {
                res = false;
                lblErrwrittenStatementFromSecretaryNotElegibility.Text = "*";
            }
            if (tbRecordReturnToAddress.Text == "")
            {
                res = false;
                lblErrRecordReturnToAddress.Text = "*";
            }
            if (tbPlaceOfPaymentAddress.Text == "")
            {
                res = false;
                lblPlaceOfPaymentAddress.Text = "*";
            }
            if (tbRtrnFnlTtlPolAddress.Text == "")
            {
                res = false;
                lblErrRtrnFnlTtlPolAddress.Text = "*";
            }
            if (tbTitleCommitmentInsuredClause.Text == "")
            {
                res = false;
                lblErrTitleCommitmentInsuredClause.Text = "*";
            }
            if (tbMortgageeClause.Text == "")
            {
                res = false;
                lblErrMortgageeClause.Text = "*";
            }
            if (tbLifeOfLoanClause.Text == "")
            {
                res = false;
                lblErrLifeOfLoanClause.Text = "*";
            }
            if (tbRightToCancelAddress.Text == "")
            {
                res = false;
                lblErrRightToCancelAddress.Text = "*";
            }
            if (tbClosingFaxNumber.Text == "")
            {
                res = false;
                lblErrClosingFaxNumber.Text = "*";
            }
            if (tbLenderMortgageClause.Text == "")
            {
                res = false;
                lblErrLenderMortgageClause.Text = "*";
            }
            if (tbBaydocsLenderID.Text == "")
            {
                res = false;
                lblBaydocsLenderID.Text = "*";
            }
            if (tbBaydocsLenderCode.Text == "")
            {
                res = false;
                lblErrBaydocsLenderCode.Text = "*";
            }
            //int ddlId = 0;
            //try
            //{
            //    ddlId = int.Parse(ddlState.SelectedValue);
            //}
            //catch { }
            //if (ddlId == 0)
            //{
            //    res = false;
            //    lblErrState.Text = "*";
            //}
            int ddlId = 0;
            try
            {
                ddlId = int.Parse(ddlLenderLocation.SelectedValue);
            }
            catch { }
            if (ddlId == 0)
            {
                res = false;
                lblErrLenderLocation.Text = "*";
            }
            ddlId = 0;
            try
            {
                ddlId = int.Parse(ddlOperatesUnderJurisdiction.SelectedValue);
            }
            catch { }
            if (ddlId == 0)
            {
                res = false;
                lblErrOperatesUnderJurisdiction.Text = "*";
            }
            return res;
        }

        #region FHA Sponsor Id grid
        private DataView GetDuplicate(DataView dv)
        {
            DataTable dt = dv.Table;
            DataRow dr = dt.NewRow();
            int n = GridMode == GRIDADDMODE ? 0 : EditRowId;
            dr.ItemArray = dt.Rows[n].ItemArray;
            if (GridMode == GRIDADDMODE)
            {
                dr["id"] = -1;
            }
            EditItemId = Convert.ToInt32(dr["id"]);
            dt.Rows.InsertAt(dr, n);
            return dt.DefaultView;
        }
        private string GetSort()
        {
            string res = SortExpression;
            if (!String.IsNullOrEmpty(res))
            {
                res += " " + GetSortDirection(res);
            }
            return res;
        }
        private void Update(int rowIndex)
        {
            DropDownList ddl = (DropDownList)G.Rows[rowIndex].FindControl("ddlState");
            TextBox tb = (TextBox)G.Rows[rowIndex].FindControl("tbFHASponsorId");
            if (ddl != null && tb != null)
            {
                lsp.FHAStateId = int.Parse(ddl.SelectedValue);
                lsp.FHASponsorId = tb.Text;
                lsp.SaveFhaSponsorId();
            }
        }
        private void CreateFooter(GridViewRowEventArgs e, int colspan, string addnewrecordtext)
        {
            int cellcount = e.Row.Cells.Count;
            int n = colspan;
            if ((colspan <= 0) || (colspan >= cellcount))
            {
                n = cellcount;
            }
            TableCell td = GetAddTd(n, addnewrecordtext);
            if (n == cellcount)
            {
                e.Row.Cells.Clear();
                e.Row.Cells.Add(td);
            }
            else
            {
                for (int i = 0; i < colspan; i++)
                {
                    e.Row.Cells.RemoveAt(0);
                }
                e.Row.Cells.AddAt(0, td);
            }
        }
        private TableCell GetAddTd(int colspan, string addnewtext)
        {
            TableCell td = new TableCell();
            td.CssClass = "footertd";
            td.ColumnSpan = colspan;
            td.VerticalAlign = VerticalAlign.Middle;
            ImageButton img = new ImageButton();
            img.ID = "imgAdd";
            img.BorderWidth = 0;
            img.AlternateText = "Add";
            img.ImageUrl = ResolveUrl(Constants.IMAGEFOLDER + "/addrecord.gif");
            img.CommandName = ADDCOMMAND;
            td.Controls.Add(img);
            LinkButton lb = new LinkButton();
            lb.ID = "lbAdd";
            lb.Text = addnewtext; 
            lb.CssClass = "EmailLinks";
            lb.CommandName = ADDCOMMAND;
            td.Controls.Add(lb);
            return td;
        }
        protected string GetCurrentRow()
        {
            return currentRow.ToString();
        }
        protected void G_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            showFooter = true;
            GridMode = GRIDVIEWMODE;
            if (e.CommandName == ADDCOMMAND)
            {
                showFooter = false;
                GridMode = GRIDADDMODE;
                EditRowId = 0;
            }
            else if (e.CommandName == UPDATECOMMAND)
            {
                Update(EditRowId);
                GridMode = GRIDVIEWMODE;
                EditItemId = -1;
            }
            else if (e.CommandName == EDITCOMMAND)
            {
                showFooter = false;
                EditRowId = Convert.ToInt32(e.CommandArgument);
                GridMode = GRIDEDITMODE;
            }
            else if (e.CommandName == DELETECOMMAND)
            {
                lsp.FHAStateId = Convert.ToInt32(e.CommandArgument);
                lsp.DeleteFhaSponsorId();
            }
            else if (e.CommandName == SORTCOMMAND)
            {
                SetSort(e.CommandArgument.ToString());
            }
            else if (e.CommandName == PAGECOMMAND)
            {
                PageIndex = Convert.ToInt32(e.CommandArgument.ToString()) - 1;
            }
            BindLenderFHASponsorIdGrid();
        }
        private void SetSort(string expr)
        {
            SortExpression = expr;
            string dir = GetSortDirection(expr);
            if (dir == ASCENDING)
            {
                dir = DESCENDING;
            }
            else
            {
                dir = ASCENDING;
            }
            ViewState["order_" + expr] = dir;
        }
        private string GetSortDirection(string sortexpression)
        {
            string res;
            Object o = ViewState["order_" + sortexpression];
            if (o != null)
            {
                res = o.ToString();
            }
            else
            {
                res = DESCENDING;
            }
            return res;
        }
        protected void G_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }
        protected void G_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
        }
        protected void G_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }
        protected void G_RowCancel(object sender, GridViewCancelEditEventArgs e)
        {
        }
        protected void G_SortCommand(object source, GridViewSortEventArgs e)
        {
        }
        protected void G_Sorting(object source, GridViewSortEventArgs e)
        {
        }
        protected void G_PageIndexChanged(object source, EventArgs e)
        {
        }
        protected void G_PageIndexChanging(Object source, EventArgs e)
        {
        }
        protected void G_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (GridMode == GRIDVIEWMODE)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    if (btnDelete != null)
                    {
                        btnDelete.Attributes.Add(ONCLICK, DELETEJS);
                    }
                }
                if (GridMode != GRIDVIEWMODE && currentRow == EditRowId)
                {
                    DataRowView dr = (DataRowView)e.Row.DataItem;
                    string sponsorId = String.Empty;
                    if (dr != null)
                    {
                        EditItemId = Convert.ToInt32(dr["id"].ToString());
                        sponsorId = dr["fhasponsorid"].ToString();
                    }
                    else
                    {
                        EditItemId = -1;
                    }
                    DropDownList ddl = (DropDownList)e.Row.FindControl("ddlState");
                    Label lbl = (Label)e.Row.FindControl("lblState");
                    if (lbl != null)
                    {
                        lbl.Visible = false;
                    }
                    if (ddl != null)
                    {
                        ddl.Visible = true;
                        if (ddl.Visible)
                        {
                            ddl.DataSource = lsp.GetStates(EditItemId);
                            ddl.DataTextField = "name";
                            ddl.DataValueField = "id";
                            ddl.DataBind();
                            ListItem li = ddl.Items.FindByValue(EditItemId.ToString());
                            if (li != null)
                            {
                                li.Selected = true;
                            }
                        }
                    }
                    Label lbl1 = (Label)e.Row.FindControl("lblFHASponsorId");
                    if (lbl1 != null)
                    {
                        lbl1.Visible = false;
                    }
                    TextBox tb = (TextBox)e.Row.FindControl("tbFHASponsorId");
                    if (tb != null)
                    {
                        tb.Visible = true;
                        tb.Text = sponsorId;
                    }
                    RequiredFieldValidator rf = (RequiredFieldValidator)e.Row.FindControl("rfFHASponsorId");
                    if (rf != null)
                    {
                        rf.Visible = true;
                    }
                }
                if (GridMode != GRIDVIEWMODE)
                {
                    SetActionColumn(e, currentRow == EditRowId);
                }
                currentRow++;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (showFooter)
                {
                    CreateFooter(e, 0, "Add FHA Sponsor ID");
                }
            }
        }
        private static void SetActionColumn(GridViewRowEventArgs e, bool isCurrentRow)
        {
            ImageButton btn = (ImageButton)e.Row.FindControl("imgEdit");
            if (btn != null)
            {
                btn.Visible = false;
            }
            btn = (ImageButton)e.Row.FindControl("imgDelete");
            if (btn != null)
            {
                btn.Visible = false;
            }
            if (isCurrentRow)
            {
                btn = (ImageButton)e.Row.FindControl("imgUpdate");
                if (btn != null)
                {
                    btn.Visible = true;
                }
                btn = (ImageButton)e.Row.FindControl("imgCancel");
                if (btn != null)
                {
                    btn.Visible = true;
                }
            }
        }
        #endregion

        #region event handlers
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                lsp = new Lender(CurrentUser.EffectiveCompanyId);
                //lsp.Address1 = tbAddress1.Text;
                //lsp.Address2 = tbAddress2.Text;
                //lsp.City = tbCity.Text;
                //lsp.Zip = tbZip.Text;
                //lsp.Name = tbName.Text;
                lsp.PhoneNumber = tbPhone.Text;
                lsp.SponsoreAgentCode = tbSponcorcode.Text;
                lsp.HowManyAssignsId = GetRadioButton("rbassigned", 3);
                lsp.IncludesAssignmentsId = GetRadioButton("rbIncludeAssignments", 2);
                lsp.BlankIncludesId = GetRadioButton("rbBlankInclude", 2);
//                lsp.StateId = int.Parse(ddlState.SelectedValue);
                lsp.LocationId = int.Parse(ddlLenderLocation.SelectedValue);
                lsp.WrittenStatementFromSecretaryNotElegibility = int.Parse(tbwrittenStatementFromSecretaryNotElegibility.Text);
                lsp.DefaultMortNotInsured = int.Parse(tbdefaultMortNotInsured.Text);
                lsp.RecordedReturnTo = tbrecordedReturnTo.Text;
                lsp.CorpHead = tbCorpHead.Text;

                lsp.OperatesUnderJurisdictionID = Convert.ToInt32(ddlOperatesUnderJurisdiction.SelectedValue);


                lsp.NCClosedLoanSeller = rbNCClosedLoanSeller1.Checked;
                lsp.ClosingFaxNumber = tbClosingFaxNumber.Text;
                lsp.LenderMortgageClause = tbLenderMortgageClause.Text;
                lsp.LoginPage = tbLenderLoginPage.Text;
                lsp.BaydocsLenderID = tbBaydocsLenderID.Text;
                lsp.BaydocsLenderCode = tbBaydocsLenderCode.Text;
                lsp.TitleCommitmentInsuredClause = tbTitleCommitmentInsuredClause.Text;
                lsp.RecordReturnToAddress = tbRecordReturnToAddress.Text;
                lsp.PlaceOfPaymentAddress = tbPlaceOfPaymentAddress.Text;
                lsp.RtrnFnlTtlPolAddress = tbRtrnFnlTtlPolAddress.Text;
                lsp.MortgageeClause = tbMortgageeClause.Text;
                lsp.LifeOfLoanClause = tbLifeOfLoanClause.Text;
                lsp.RightToCancelAddress = tbRightToCancelAddress.Text;
                lsp.AbbreviatedName = tbAbbreviatedName.Text;
                lsp.SponsorAgentID = lsp.SponsoreAgentCode;

                int res = lsp.Save();
                if (res > 0)
                {
                    lblMessage.Text = Constants.SUCCESSMESSAGE;
                    if (lsp.ID < 0)
                    {
                        lsp.ID = res;
                        trFHAGrid.Visible = true;
                        BindLenderFHASponsorIdGrid();
                    }
                }
                else
                {
                    lblMessage.Text = DBERROR;
                }
            }
        }
        #endregion

        #endregion


        #region originator related
        private void BindOriginatorData()
        {
            ClearOriginatorErrors();
            originator = new Originator(CurrentUser.EffectiveCompanyId);
            if (CurrentOriginatorStateId > 0)
            {
                originator.StateId = CurrentOriginatorStateId;
            }
            BindGeneralOriginatorData();
            BindStateSpecificData();
            BindClosingInfoData();
            BindOriginatorStateLicensing();
            SetTabsState();
        }
        private void ClearOriginatorErrors()
        {
            lblErrClosingName.Text = "";
            lblErrClosingAddress.Text = "";
            lblErrClosingCity.Text = "";
            lblErrClosingState.Text = "";
            lblErrClosingZip.Text = "";
            lblErrClosingPhoneNumber.Text = "";
            lblErrClosingStateOfInc.Text = "";
        }
        private void SetTabsState()
        {
            tsOriginator.Tabs[2].Enabled = originator.ID > 0 && originator.IsCompanyCloseLoans;
        }
        private void BindGeneralOriginatorData()
        {
            tbCounsDaysNotifyMeExp.Value = originator.CounsDaysNotifyMeExp;
            tbTitleDaysNotifyMeExp.Value = originator.TitleDaysNotifyMeExp;
            tbAppraisalDaysNotifyMeExp.Value = originator.AppraisalDaysNotifyMeExp;
            tbPestDaysNotifyMeExp.Value = originator.PestDaysNotifyMeExp;
            tbBidDaysNotifyMeExp.Value = originator.BidDaysNotifyMeExp;
            tbWaterTestDaysNotifyMeExp.Value = originator.WaterTestDaysNotifyMeExp;
            tbSepticInspDaysNotifyMeExp.Value = originator.SepticInspDaysNotifyMeExp;
            tbOilTankInspDaysNotifyMeExp.Value = originator.OilTankInspDaysNotifyMeExp;
            tbRoofInspDaysNotifyMeExp.Value = originator.RoofInspDaysNotifyMeExp;
            tbFloodCertDaysNotifyMeExp.Value = originator.FloodCertDaysNotifyMeExp;
            tbCreditReportDaysNotifyMeExp.Value = originator.CreditReportDaysNotifyMeExp;
            tbLDPDaysNotifyMeExp.Value = originator.LDPDaysNotifyMeExp;
            tbEPLSDaysNotifyMeExp.Value = originator.EPLSDaysNotifyMeExp;
            tbCaivrsDaysNotifyMeExp.Value = originator.CaivrsDaysNotifyMeExp;
            tbBaydocsID.Text = originator.BaydocsID;
            cbIsCompanyCloseLoans.Checked = originator.IsCompanyCloseLoans;
            tbClosingName.Text = originator.ClosingName;
            tbClosingAddress.Text = originator.ClosingAddress;
            tbClosingCity.Text = originator.ClosingCity;
            BindDdl(ddlClosingState, DvState, originator.ClosingStateId);
            tbClosingZip.Text = originator.ClosingZip;
            tbClosingPhoneNumber.Text = originator.ClosingPhoneNumber;
            BindDdl(ddlClosingStateOfInc, DvStateUS, originator.ClosingStateOfIncId);
            
            divClosingData.Attributes.Add(STYLEATTRIBUTE, originator.IsCompanyCloseLoans?TRVISIBLE:TRHIDDEN);

        }
        private void BindStateSpecificData()
        {
            BindStates();
            if (CurrentOriginatorStateId > 0)
            {
                tsOriginatorState.Tabs[1].Enabled = true;
                tsOriginatorState.Tabs[1].Text = "Details(" + CurrentOriginatorState + ")";
                tbOriginatorAddress1.Text = originator.Address1;
                tbOriginatorAddress2.Text = originator.Address2;
                tbOriginatorName.Text = originator.Name;
                tbFHAOriginatorNumber.Text = originator.FHAOriginatorNumber;
                tbOriginatorCity.Text = originator.City;
                tbOriginatorFax.Text = originator.Fax;
                tbOriginatorPhone.Text = originator.Phone;
                tbOriginatorZip.Text = originator.Zip;
            }
            else
            {
                tsOriginatorState.Tabs[1].Enabled = false;
                tsOriginatorState.Tabs[1].Text = "Details";
            }
            tsOriginatorState.SelectedIndex = stateTab;
            btnDeleteOriginatorDetails.Enabled = originator.HasState(CurrentOriginatorStateId);
            RadMultiPage3.SelectedIndex = tsOriginatorState.SelectedIndex;
        }
        private void BindClosingInfoData()
        {
            dlProducts.DataSource = DvProducts;
            dlProducts.DataBind();
        }

        #region State licansing grid
        private void BindOriginatorStateLicensing()
        {
            currentRowLicense = 0;
            bool isEmpty = (DvStateLicense.Count == 1) && (Convert.ToInt32(DvStateLicense[0]["id"]) < 0);
            bool needInsert = !isEmpty && (GridModeLicense == GRIDADDMODE);
            DataView dv;
            if (needInsert)
            {
                dv = GetDuplicateLicensing(DvStateLicense);
            }
            else
            {
                dv = DvStateLicense;
            }
            dv.Sort = GetSortStateLicensing();
            gStateLicense.PageIndex = PageIndexLicensing;
            gStateLicense.DataSource = dv;
            gStateLicense.DataBind();
            showFooterLicensing = (GridModeLicense == GRIDVIEWMODE);
            if (gStateLicense.FooterRow != null) gStateLicense.FooterRow.Visible = showFooterLicensing;
            if (GridModeLicense != GRIDADDMODE)
            {
                if (isEmpty)
                {
                    int columns = gStateLicense.Rows[0].Cells.Count;
                    gStateLicense.Rows[0].Cells.Clear();
                    gStateLicense.Rows[0].Cells.Add(new TableCell());
                    gStateLicense.Rows[0].Cells[0].ColumnSpan = columns;
                    gStateLicense.Rows[0].Cells[0].Text = EMPTYTEXT;
                }
            }

        }
        private DataView GetDuplicateLicensing(DataView dv)
        {
            DataTable dt = dv.Table;
            DataRow dr = dt.NewRow();
            int n = GridModeLicense == GRIDADDMODE ? 0 : EditRowId;
            dr.ItemArray = dt.Rows[n].ItemArray;
            if (GridModeLicense == GRIDADDMODE)
            {
                dr["id"] = -1;
            }
            EditItemLicensingId = Convert.ToInt32(dr["id"]);
            dt.Rows.InsertAt(dr, n);
            return dt.DefaultView;
        }
        protected string GetCurrentLicensingRow()
        {
            return currentRowLicense.ToString();
        }
        private void SetSortLicensing(string expr)
        {
            SortExpressionLicensing = expr;
            string dir = GetSortDirection(expr);
            if (dir == ASCENDING)
            {
                dir = DESCENDING;
            }
            else
            {
                dir = ASCENDING;
            }
            ViewState["order_" + expr] = dir;
        }
        private string GetSortStateLicensing()
        {
            string res = SortExpressionLicensing;
            if (!String.IsNullOrEmpty(res))
            {
                res += " " + GetSortDirection(res);
            }
            return res;
        }
        private void UpdateLicensing(int rowIndex)
        {
            DropDownList ddl = (DropDownList)gStateLicense.Rows[rowIndex].FindControl("ddlLicenseState");
            TextBox tb = (TextBox)gStateLicense.Rows[rowIndex].FindControl("tbLicenseNumber");
            RadDateInput rd = (RadDateInput)gStateLicense.Rows[rowIndex].FindControl("tbExpirationDate");
            if (ddl != null && tb != null && rd != null)
            {
                int stateId = int.Parse(ddl.SelectedValue);
                string licenseNumber = tb.Text;
                DateTime dt = (DateTime) rd.SelectedDate;
                OriginatorStateLicense.Save(EditItemLicensingId, originator.ID, stateId, licenseNumber, dt);
            }
        }

        protected void gStateLicense_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            showFooterLicensing = true;
            GridModeLicense = GRIDVIEWMODE;
            if (e.CommandName == ADDCOMMAND)
            {
                showFooterLicensing = false;
                GridModeLicense = GRIDADDMODE;
                EditRowLicensingId = 0;
            }
            else if (e.CommandName == UPDATECOMMAND)
            {
                UpdateLicensing(EditRowLicensingId);
                EditItemLicensingId = -1;
                EditRowLicensingId = -1;
                dvStateLicense = null;
            }
            else if (e.CommandName == CANCELCOMMAND)
            {
                EditItemLicensingId = -1;
                EditRowLicensingId = -1;
                dvStateLicense = null;
            }
            else if (e.CommandName == EDITCOMMAND)
            {
                showFooterLicensing = false;
                EditRowLicensingId = Convert.ToInt32(e.CommandArgument);
                GridModeLicense = GRIDEDITMODE;
            }
            else if (e.CommandName == DELETECOMMAND)
            {
                OriginatorStateLicense.Delete(Convert.ToInt32(e.CommandArgument));
                dvStateLicense = null;
            }
            else if (e.CommandName == SORTCOMMAND)
            {
                SetSortLicensing(e.CommandArgument.ToString());
            }
            else if (e.CommandName == PAGECOMMAND)
            {
                PageIndexLicensing = Convert.ToInt32(e.CommandArgument.ToString()) - 1;
            }

            BindOriginatorStateLicensing();


        }
        protected void gStateLicense_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (GridModeLicense == GRIDVIEWMODE)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    if (btnDelete != null)
                    {
                        btnDelete.Attributes.Add(ONCLICK, DELETEJS);
                    }
                }
                if (GridModeLicense != GRIDVIEWMODE && currentRowLicense == EditRowLicensingId)
                {
                    DataRowView dr = (DataRowView)e.Row.DataItem;
                    int stateId = 0;
                    DateTime dt = Holidays.RemoveTime(DateTime.Now);
                    string licenseNumber = String.Empty;
                    if (dr != null)
                    {
                        EditItemLicensingId = Convert.ToInt32(dr["id"].ToString());
                        if (EditItemLicensingId>0)
                        {
                            stateId = int.Parse(dr["stateId"].ToString());
                            dt = DateTime.Parse(dr["expirationdate"].ToString());
                            licenseNumber = dr["licensenumber"].ToString();
                        }
                    }
                    else
                    {
                        EditItemLicensingId = -1;
                    }
                    DropDownList ddl = (DropDownList)e.Row.FindControl("ddlLicenseState");
                    Label lbl = (Label)e.Row.FindControl("lblLicenseState");
                    if (lbl != null)
                    {
                        lbl.Visible = false;
                    }
                    if (ddl != null)
                    {
                        ddl.Visible = true;
                        if (ddl.Visible)
                        {
                            ddl.DataSource = OriginatorStateLicense.GetStateListForLicense(EditItemLicensingId, originator.ID);
                            ddl.DataTextField = "name";
                            ddl.DataValueField = "id";
                            ddl.DataBind();
                            ddl.Items.Insert(0,new ListItem("-Select-",0.ToString()));
                            ListItem li = ddl.Items.FindByValue(stateId.ToString());
                            if (li != null)
                            {
                                li.Selected = true;
                            }
                        }
                        RangeValidator rv = (RangeValidator)e.Row.FindControl("rvLicenseState");
                        if(rv!=null)
                        {
                            rv.Visible = true;
                        }
                    }
                    Label lbl1 = (Label)e.Row.FindControl("lblLicenseNumber");
                    if (lbl1 != null)
                    {
                        lbl1.Visible = false;
                    }
                    TextBox tb = (TextBox)e.Row.FindControl("tbLicenseNumber");
                    if (tb != null)
                    {
                        tb.Visible = true;
                        tb.Text = licenseNumber;
                    }
                    RequiredFieldValidator rf = (RequiredFieldValidator)e.Row.FindControl("rfLicenseNumber");
                    if (rf != null)
                    {
                        rf.Visible = true;
                    }
                    Label lbl2 = (Label)e.Row.FindControl("lblExpirationDate");
                    if (lbl2 != null)
                    {
                        lbl2.Visible = false;
                    }
                    RadDateInput rd = (RadDateInput)e.Row.FindControl("tbExpirationDate");
                    if(rd != null)
                    {
                        rd.SelectedDate = dt;
                        rd.Visible = true;
                    }
                    RequiredFieldValidator rf1 = (RequiredFieldValidator)e.Row.FindControl("rfExpirationDate");
                    if (rf1 != null)
                    {
                        rf1.Visible = true;
                    }
                }
                if (GridModeLicense != GRIDVIEWMODE)
                {
                    SetActionColumn(e, currentRowLicense == EditRowLicensingId);
                }
                currentRowLicense++;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (showFooterLicensing)
                {
                    CreateFooter(e, 0,"Add state");
                }
            }
        }
        protected void gStateLicense_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }
        protected void gStateLicense_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
        }
        protected void gStateLicense_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }
        protected void gStateLicense_RowCancel(object sender, GridViewCancelEditEventArgs e)
        {
        }
        protected void gStateLicense_SortCommand(object source, GridViewSortEventArgs e)
        {
        }
        protected void gStateLicense_Sorting(object source, GridViewSortEventArgs e)
        {
        }
        protected void gStateLicense_PageIndexChanged(object source, EventArgs e)
        {
        }
        protected void gStateLicense_PageIndexChanging(Object source, EventArgs e)
        {
        }
        

        #endregion

        private bool ValidateOriginatorGeneral()
        {
            bool res = true;
            if(cbIsCompanyCloseLoans.Checked)
            {
                if (String.IsNullOrEmpty(tbClosingName.Text))
                {
                    lblErrClosingName.Text = "*";
                    res = false;
                }
                if (String.IsNullOrEmpty(tbClosingAddress.Text))
                {
                    lblErrClosingAddress.Text = "*";
                    res = false;
                }
                if (String.IsNullOrEmpty(tbClosingCity.Text))
                {
                    lblErrClosingCity.Text = "*";
                    res = false;
                }
                if (String.IsNullOrEmpty(tbClosingZip.Text))
                {
                    lblErrClosingZip.Text = "*";
                    res = false;
                }
                if (String.IsNullOrEmpty(tbClosingPhoneNumber.Text))
                {
                    lblErrClosingPhoneNumber.Text = "*";
                    res = false;
                }
                if (ddlClosingStateOfInc.SelectedValue == "0")
                {
                    lblErrClosingStateOfInc.Text = "*";
                    res = false;
                }
                if(ddlClosingState.SelectedValue=="0")
                {
                    lblErrClosingState.Text = "*";
                    res = false;
                }
            }
            return res;
        }

        private void BindStates()
        {
            dlStates.DataSource = DvOriginatorStates;
            dlStates.DataBind();
        }
        private void RebindOriginatorStateSpecificData()
        {
            CurrentOriginatorStateId = -1;
            CurrentOriginatorState = "";
            dvOriginatorStates = null;
            BindOriginatorData();
            lblMessage.Text = Constants.SUCCESSMESSAGE;
        }
        private string GetProductList()
        {
            string res = "";
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            foreach (DataListItem item in dlProducts.Items)
            {
                CheckBox cb = (CheckBox)item.FindControl(PRODUCTCHECKBOXID);
                if (cb != null && cb.Checked)
                {
                    XmlNode n = d.CreateElement(ITEMELEMENT);
                    XmlAttribute a = d.CreateAttribute(IDATTRIBUTE);
                    a.Value = cb.Attributes[PRODUCTIDATTRIBUTE];
                    n.Attributes.Append(a);
                    root.AppendChild(n);
                }
            }
            if (root.ChildNodes.Count > 0)
            {
                d.AppendChild(root);
                res = d.OuterXml;
            }
            return res;
        }

        #region event handlers
        protected void dlProducts_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            if (dr != null)
            {
                int id = int.Parse(dr["id"].ToString());
                string name = dr["name"].ToString();
                bool isChecked = bool.Parse(dr["selected"].ToString());
                allProductsSelected &= isChecked;
                CheckBox cb = (CheckBox)e.Item.FindControl(PRODUCTCHECKBOXID);
                if (cb != null)
                {
                    cb.Checked = isChecked;
                    cb.Text = name;
                    cb.Attributes.Add(PRODUCTIDATTRIBUTE,id.ToString());
                    cb.Attributes.Add(ONCLICK, String.Format(CHECKFIELDJS, cbAllProducts.ClientID, productdiv.ClientID));
                }
            }
        }
        protected void btnSaveOriginator_Click(object sender, EventArgs e)
        {
            if(ValidateOriginatorGeneral())
            {
                originator = new Originator(CurrentUser.EffectiveCompanyId);
                if (tbCounsDaysNotifyMeExp.Value != null)
                {
                    originator.CounsDaysNotifyMeExp = int.Parse(tbCounsDaysNotifyMeExp.Text);
                }
                if (tbTitleDaysNotifyMeExp.Value != null)
                {
                    originator.TitleDaysNotifyMeExp = int.Parse(tbTitleDaysNotifyMeExp.Text);
                }
                if (tbAppraisalDaysNotifyMeExp.Value != null)
                {
                    originator.AppraisalDaysNotifyMeExp = int.Parse(tbAppraisalDaysNotifyMeExp.Text);
                }
                if (tbPestDaysNotifyMeExp.Value != null)
                {
                    originator.PestDaysNotifyMeExp = int.Parse(tbPestDaysNotifyMeExp.Text);
                }
                if (tbBidDaysNotifyMeExp.Value != null)
                {
                    originator.BidDaysNotifyMeExp = int.Parse(tbBidDaysNotifyMeExp.Text);
                }
                if (tbWaterTestDaysNotifyMeExp.Value != null)
                {
                    originator.WaterTestDaysNotifyMeExp = int.Parse(tbWaterTestDaysNotifyMeExp.Text);
                }
                if (tbSepticInspDaysNotifyMeExp.Value != null)
                {
                    originator.SepticInspDaysNotifyMeExp = int.Parse(tbSepticInspDaysNotifyMeExp.Text);
                }
                if (tbOilTankInspDaysNotifyMeExp.Value != null)
                {
                    originator.OilTankInspDaysNotifyMeExp = int.Parse(tbOilTankInspDaysNotifyMeExp.Text);
                }
                if (tbRoofInspDaysNotifyMeExp.Value != null)
                {
                    originator.RoofInspDaysNotifyMeExp = int.Parse(tbRoofInspDaysNotifyMeExp.Text);
                }
                if (tbFloodCertDaysNotifyMeExp.Value != null)
                {
                    originator.FloodCertDaysNotifyMeExp = int.Parse(tbFloodCertDaysNotifyMeExp.Text);
                }
                if (tbCreditReportDaysNotifyMeExp.Value != null)
                {
                    originator.CreditReportDaysNotifyMeExp = int.Parse(tbCreditReportDaysNotifyMeExp.Text);
                }
                if (tbLDPDaysNotifyMeExp.Value != null)
                {
                    originator.LDPDaysNotifyMeExp = int.Parse(tbLDPDaysNotifyMeExp.Text);
                }
                if (tbEPLSDaysNotifyMeExp.Value != null)
                {
                    originator.EPLSDaysNotifyMeExp = int.Parse(tbEPLSDaysNotifyMeExp.Text);
                }
                if (tbCaivrsDaysNotifyMeExp.Value != null)
                {
                    originator.CaivrsDaysNotifyMeExp = int.Parse(tbCaivrsDaysNotifyMeExp.Text);
                }
                originator.BaydocsID = tbBaydocsID.Text;
                originator.IsCompanyCloseLoans = cbIsCompanyCloseLoans.Checked;
                originator.ClosingName = tbClosingName.Text;
                originator.ClosingAddress = tbClosingAddress.Text;
                originator.ClosingCity = tbClosingCity.Text;
                originator.ClosingStateId = int.Parse(ddlClosingState.SelectedValue);
                originator.ClosingZip = tbClosingZip.Text;
                originator.ClosingPhoneNumber = tbClosingPhoneNumber.Text;
                originator.ClosingStateOfIncId = int.Parse(ddlClosingStateOfInc.SelectedValue);

                int res = originator.Save();
                if (res > 0)
                {
                    lblMessage.Text = Constants.SUCCESSMESSAGE;
                    SetTabsState();
                }
                else
                {
                    lblMessage.Text = DBERROR;
                }
            }
            divClosingData.Attributes.Add(STYLEATTRIBUTE, cbIsCompanyCloseLoans.Checked ? TRVISIBLE : TRHIDDEN);
        }
        protected void btnSaveClosingOriginator_Click(object sender, EventArgs e)
        {
            string list = GetProductList();
            if(!String.IsNullOrEmpty(list))
            {
                originator = new Originator(CurrentUser.EffectiveCompanyId);
                if(originator.SaveClosingProduct(list))
                {
                    lblMessage.Text = Constants.SUCCESSMESSAGE;
                }
                else
                {
                    lblMessage.Text = DBERROR;
                }
            }
        }
        protected void btnSaveOriginatorDetails_Click(object sender, EventArgs e)
        {
            originator.StateId = CurrentOriginatorStateId;
            originator.Address1 = tbOriginatorAddress1.Text;
            originator.Address2 = tbOriginatorAddress2.Text;
            originator.Name = tbOriginatorName.Text;
            originator.FHAOriginatorNumber = tbFHAOriginatorNumber.Text;
            originator.Phone = tbOriginatorPhone.Text;
            originator.City = tbOriginatorCity.Text;
            originator.Fax = tbOriginatorFax.Text;
            originator.Zip = tbOriginatorZip.Text;
            originator.CurrentStateData.Save();
            RebindOriginatorStateSpecificData();
        }
        protected void btnDeleteOriginatorDetails_Click(object sender, EventArgs e)
        {
            originator.CurrentStateData.Delete();
            RebindOriginatorStateSpecificData();
        }
        protected void lbStateSelected_Click(object sender, EventArgs e)
        {
            CurrentOriginatorStateId = int.Parse(((LinkButton)sender).Attributes["stateid"]);
            CurrentOriginatorState = ((LinkButton)sender).Text;
            originator.StateId = CurrentOriginatorStateId;
            stateTab = 1;
            BindStateSpecificData();
        }
        protected void dlStates_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            if (dr != null)
            {
                int id = int.Parse(dr["id"].ToString());
                string name = dr["name"].ToString();
                int oid = int.Parse(dr["originatorId"].ToString());
                LinkButton lb = (LinkButton)e.Item.FindControl(STATELINKID);
                if (lb != null)
                {
                    lb.Attributes.Add("stateid", id.ToString());
                    lb.Text = name;
                    lb.ID = STATELINKID + "_" + id;
                    string css = "lbstatenocounty";
                    if (oid > 0)
                    {
                        css = "lbstateallcounty";
                    }
                    lb.CssClass = css;
                }
            }
        }
        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(CurrentUser.IsCorrespondentLenderAdmin || CurrentUser.LoggedAsOriginator))
            {
                Response.Redirect(ResolveUrl("~/" + CurrentUser.GetDefaultPage()));
            }
            cbAllProducts.Attributes.Add(ONCLICK, String.Format(CHECKALLJS, productdiv.ClientID));
            cbIsCompanyCloseLoans.Attributes.Add(ONCLICKATTRIBUTE, String.Format(JSCBSHOWROW, divClosingData.ClientID));
            SetTabVisibility();
            lblMessage.Text = "";
            BindData();
        }

        
        #region servicer
        private void BindServicerData()
        {
            servicer = new Servicer(CurrentUser.EffectiveCompanyId);
            tbServicerName.Text = servicer.Name;
            tbBaydocServicerId.Text = servicer.BaydocServicerId;
            //tbServicerAddress1.Text = servicer.Address1;
            //tbServicerAddress2.Text = servicer.Address2;
            //tbServicerCity.Text = servicer.City;
            //tbServicerZip.Text = servicer.Zip;
            tbServicerPhone.Text = servicer.Phone;
            tbServicerFax.Text = servicer.Fax;
//            BindDdl(ddlServicerState,DvState,servicer.StateId);
            BindDdl(ddlServicerLocation, DvLocation, servicer.LocationId);
        }
        protected void btnSaveServicer_Click(object sender, EventArgs e)
        {
            servicer.Name = tbServicerName.Text;
            servicer.BaydocServicerId = tbBaydocServicerId.Text;
            //servicer.Address1 = tbServicerAddress1.Text;
            //servicer.Address2 = tbServicerAddress2.Text;
            //servicer.City = tbServicerCity.Text;
            //servicer.Zip = tbServicerZip.Text;
            servicer.Phone = tbServicerPhone.Text;
            servicer.Fax = tbServicerFax.Text;
//            servicer.StateId = int.Parse(ddlServicerState.SelectedValue);
            servicer.LocationId = int.Parse(ddlServicerLocation.SelectedValue);

            if(servicer.Save()>0)
            {
                lblMessage.Text = Constants.SUCCESSMESSAGE;
            }
        }
        #endregion



        #region investor related
        private void BindInvestorData()
        {
            investor = new Investor(CurrentUser.EffectiveCompanyId);
            //tbInvestorAddress1.Text = investor.Address1;
            //tbInvestorAddress2.Text = investor.Address2;
            //tbInvestorName.Text = investor.Name;
            //tbInvestorCity.Text = investor.City;
            //tbInvestorZip.Text = investor.Zip;
            //BindDdl(ddlInvestorState, DvState, investor.StateId);
            BindDdl(ddlInvestorLocation, DvLocation, investor.LocationId);
        }
        protected void btnSaveInvestor_Click(object sender, EventArgs e)
        {
            investor = new Investor(CurrentUser.EffectiveCompanyId);
            //investor.Address1 = tbInvestorAddress1.Text;
            //investor.Address2 = tbInvestorAddress2.Text;
            //investor.Name = tbInvestorName.Text;
            //investor.City = tbInvestorCity.Text;
            //investor.Zip = tbInvestorZip.Text;
            //investor.StateId = int.Parse(ddlInvestorState.SelectedValue);
            investor.LocationId = int.Parse(ddlInvestorLocation.SelectedValue);
            int res = investor.Save();
            if (res > 0)
            {
                lblMessage.Text = Constants.SUCCESSMESSAGE;
            }
            else
            {
                lblMessage.Text = DBERROR;
            }
        }
        #endregion

        #region trustee related
        private void BindTrusteeData()
        {
            trustee = new Trustee(CurrentUser.EffectiveCompanyId);
            //tbTrusteeAddress1.Text = trustee.Address1;
            //tbTrusteeAddress2.Text = trustee.Address2;
            //tbTrusteeName.Text = trustee.Name;
            //tbTrusteeCity.Text = trustee.City;
            //tbTrusteeZip.Text = trustee.Zip;
            //BindDdl(ddlTrusteeState, DvState, trustee.StateId);
            BindDdl(ddlTrusteeLocation, DvLocation, trustee.LocationId);
        }
        protected void btnSaveTrustee_Click(object sender, EventArgs e)
        {
            trustee = new Trustee(CurrentUser.EffectiveCompanyId);
            //trustee.Address1 = tbTrusteeAddress1.Text;
            //trustee.Address2 = tbTrusteeAddress2.Text;
            //trustee.Name = tbTrusteeName.Text;
            //trustee.City = tbTrusteeCity.Text;
            //trustee.Zip = tbTrusteeZip.Text;
            //trustee.StateId = int.Parse(ddlTrusteeState.SelectedValue);
            trustee.LocationId = int.Parse(ddlTrusteeLocation.SelectedValue);
            int res = trustee.Save();
            if (res > 0)
            {
                lblMessage.Text = Constants.SUCCESSMESSAGE;
            }
            else
            {
                lblMessage.Text = DBERROR;
            }
        }
        #endregion

    }
}