using System;
using System.Data;
using System.Xml;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditVendor : AppControl
    {
        #region constants
        private const string ADDNEWVENDOR = "Add new vendor";
        private const string EDITVENDORVENDOR = "Edit vendor {0}";
        private const string VENDORID = "editvendorid";
        private const string CATEGORYID = "category_{0}";
        private const string DBERRORMESSAGE = "Can''t save data";
        private const string ROOTELEMENT = "Root";
        private const string ITEMELEMENT = "item";
        private const string IDATTRIBUTE = "id";
        private const string AMOUNTATTRIBUTE = "amount";
        private const string REPEATERID = "rpVendorFeeType";
        private const string STATELINKID = "lbState";
        private const string CHECKALLJS = "CheckAllCounties(this,'{0}');";
        private const string CHECKFIELDJS = "CheckCounty(this,'{0}','{1}');";
        private const string COUNTYCHECKBOXID = "cbCounty";
        private const string ONCLICK = "onclick";
        private const string SELECTEDSTATEID = "selectedstateid";
        private const string SELECTNAME = "id={0}";
        private const int GRIDVIEWMODE = 0;
        private const int GRIDEDITMODE = 1;
        private const int GRIDADDMODE = 2;
        private const string SETTINGSGRIDMODE = "settingsgridmode";
        private const string EDITROW = "editrowid";
        private const string EDITITEM = "edititemid";
        private const string ADDCOMMAND = "Add";
        private const string ALWAYSTEXT = "Always";
        private const string ALWAYSVALUE = "1";
        private const string NEVERTEXT = "Never";
        private const string NEVERVALUE = "0";
        private const string DELETEJS = "javascript:{{var r=confirm('Delete this Originator?');if (!r)return false;}};";
        private const string EMPTYTEXT = "No records to display";
        private const string EDITCOMMAND = "Edit";
        private const string UPDATECOMMAND = "Update";
        private const string SORTCOMMAND = "Sort";
        private const string PAGECOMMAND = "Page";
/*
        private const string CANCELCOMMAND = "Cancel";
*/
        private const string DELETECOMMAND = "Delete";
        private const string ASCENDING = "asc";
        private const string DESCENDING = "desc";
        private const string SORTEXPRESSION = "settingssort";
        private const string SETTINGSPAGEINDEX = "settingspageindex";
        private const string LOGINALREADYEXISTS = "Login already exists";
        private const string SETPASSWORDJS = "SetVisibility(this,'{0}','{1}');";
        private const string SHOWAFFILIATED = "SetAffiliatedVisibility(this,'{0}','{1}','{2}');";
        private const int ITEMSADVANCEDCATEGORYID = 5;
        #endregion

        #region fields
        private VendorGlobal vendor;
        private DataView dvCategory;
        private DataView dvVendorFee;
        private DataView dvStates;
        private DataView dvCounties;
        private DataView dvAlwaysNever;
        private DataView dvOriginators;
        private int selectedFeeTab = 0;
        private int selectedGeoTab = 0;
        private bool allCountiesSelected = false;
        protected int currentRow = 0;
        protected bool showFooter = true;
        private VendorSettings settings;
        private bool hasLogin = false;
        #endregion

        #region properties
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
        private int GridMode
        {
            get 
            {
                int res = GRIDVIEWMODE;
                Object o = ViewState[SETTINGSGRIDMODE];
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
            set { ViewState[SETTINGSGRIDMODE] = value; }
        }
        private DataView DvOriginators
        {
            get
            {
                if (dvOriginators == null)
                {
                    dvOriginators = Vendor.GetOriginatorList();
                }
                return dvOriginators;
            }
        }
        private DataView DvAlwaysNever
        {
            get
            {
                if (dvAlwaysNever == null)
                {
                    dvAlwaysNever = Vendor.GetAlwaysNeverSettings();
                }
                return dvAlwaysNever;
            }
        }
        private DataView DvCounties
        {
            get
            {
                if (dvCounties == null)
                {
                    dvCounties = Vendor.GetCounties(SelectedStateId);
                }
                return dvCounties;
            }
        }
        private DataView DvStates
        {
            get
            {
                if (dvStates == null)
                {
                    dvStates = Vendor.GetStates();
                }
                return dvStates;
            }
        }
        private DataView DvCategory
        {
            get 
            {
                if (dvCategory == null)
                {
                    dvCategory = VendorGlobal.GetFeeCategory();
                }
                return dvCategory;
            }
        }
        private DataView DvVendorFee
        { 
            get
            {
                if (dvVendorFee == null)
                {
                    dvVendorFee = Vendor.GetFeeTypesAmount();
                }
                return dvVendorFee;
            }
            
        }
        private int VendorId
        {
            get 
            {
                int res = -1;
                Object o = Session[VENDORID];
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
            set { Session[VENDORID] = value; }
        }
        private VendorGlobal Vendor
        {
            get
            {
                if (vendor == null)
                {
                    vendor = new VendorGlobal(VendorId);
                }
                return vendor;
            }
        }
        private int SelectedStateId
        {
            get 
            {
                int res = -1;
                Object o = ViewState[SELECTEDSTATEID];
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
            set { ViewState[SELECTEDSTATEID] = value; }
        }
        #endregion

        #region methods
        private void BindData()
        {            
            if (Vendor.ID > 0)
            {
                lblHeader.Text = String.Format(EDITVENDORVENDOR, Vendor.Name);
            }
            else
            {
                lblHeader.Text = ADDNEWVENDOR;
            }            
            BindTabs();
        }
        private void BindTabs()
        {
            BindBasicInfo();
            BindFeeTypes();
            BindGeoFilter();
            BindAlwaysNeverSettings();
        }
        private void BindAlwaysNeverSettings()
        {
            BindAlwaysNeverSettingsGrid();    
        }
        private void BindAlwaysNeverSettingsGrid()
        {
            currentRow = 0;
            bool isEmpty = (DvAlwaysNever.Count == 1) && (Convert.ToInt32(DvAlwaysNever[0]["id"]) < 0);
            bool needInsert = !isEmpty && (GridMode == GRIDADDMODE);
            DataView dv;
            if (needInsert)
            {
                dv = GetDuplicate(DvAlwaysNever);
            }
            else
            {
                dv = DvAlwaysNever;
            }
            dv.Sort = GetSort();
            gSettings.PageIndex = PageIndex;
            gSettings.DataSource = dv;
            gSettings.DataBind();
            showFooter = (GridMode == GRIDVIEWMODE);
            if (gSettings.FooterRow != null) gSettings.FooterRow.Visible = showFooter;
            if (GridMode != GRIDADDMODE)
            {
                if (isEmpty)
                {
                    int columns = gSettings.Rows[0].Cells.Count;
                    gSettings.Rows[0].Cells.Clear();
                    gSettings.Rows[0].Cells.Add(new TableCell());
                    gSettings.Rows[0].Cells[0].ColumnSpan = columns;
                    gSettings.Rows[0].Cells[0].Text = EMPTYTEXT;
                }
            }
        }
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
        private void BindGeoFilter()
        {            
            BindStates();
            BindCounties();
        }
        private void BindCounties()
        {
            if (SelectedStateId > 0)
            {
                tsGeoFilter.Tabs[1].Text = GetStateName(SelectedStateId);
                tsGeoFilter.Tabs[1].Enabled = true;
                allCountiesSelected = (DvCounties.Count>0)?true:false;
                dlCounty.DataSource = DvCounties;
                dlCounty.DataBind();
            }
            else
            {
                tsGeoFilter.Tabs[1].Text = "Please select state";
                tsGeoFilter.Tabs[1].Enabled = false;
            }
        }
        private void BindStates()
        {
            dlStates.DataSource = DvStates;
            dlStates.DataBind();
        }
        private void BindFeeTypes()
        {
            if (!tabFeeTypes.Enabled)
            {
                return;
            }
            tsFee.Tabs.Clear();
            mpFees.PageViews.Clear();
            tsFee.AppendDataBoundItems = false;
            DvCategory.RowFilter = String.Format("id<>{0}",ITEMSADVANCEDCATEGORYID);
            tsFee.DataSource = DvCategory;
            tsFee.DataTextField = "name";
            tsFee.DataValueField = "id";
            tsFee.DataBind();
            tsFee.SelectedIndex = selectedFeeTab;
            mpFees.SelectedIndex = tsFee.SelectedIndex;

        }
        private void BindBasicInfo()
        {
            tbCompanyName.Text = Vendor.Name;
            tbCorporateAddress1.Text = Vendor.CorporateAddress1;
            tbCorporateAddress2.Text = Vendor.CorporateAddress2;
            tbCompanyPhone.Text = Vendor.CompanyPhone;
            tbCompanyFax.Text = Vendor.CompanyFax;
            tbCompanyEmail.Text = Vendor.CompanyEmail;
            tbCompanyCity.Text = Vendor.CompanyCity;
            tbCompanyZip.Text = Vendor.CompanyZip;
            
            tbBillingAddress1.Text = Vendor.BillingAddress1;
            tbBillingAddress2.Text = Vendor.BillingAddress2;
            tbBillingCity.Text = Vendor.BillingCity;
            tbBillingZip.Text = Vendor.BillingZip;

            tbPrimaryContact.Text = Vendor.PrimaryContactName;
            tbPCPhone1.Text = Vendor.PCPhone1;
            tbPCPhone2.Text = Vendor.PCPhone2;
            tbPCEmail.Text = Vendor.PCEmail;
            tbSecondaryContact.Text = Vendor.SecondaryContactName;
            tbSCPhone1.Text = Vendor.SCPhone1;
            tbSCPhone2.Text = Vendor.SCPhone2;
            tbSCEmail.Text = Vendor.SCEmail;
            if(Vendor.LicenseExpDate!=null)
                dtLicenseExpdate.SelectedDate = Vendor.LicenseExpDate;
            ddlCompanyState.DataSource = DvStates;
            ddlCompanyState.DataTextField = "Name";
            ddlCompanyState.DataValueField = "id";
            ddlCompanyState.DataBind();
            ddlCompanyState.Items.Insert(0, new ListItem("- Select - ", "0"));
            if (Vendor.CompanyStateId > 0) ddlCompanyState.SelectedValue = Vendor.CompanyStateId.ToString();
            ddlBillingState.DataSource = DvStates;
            ddlBillingState.DataTextField = "Name";
            ddlBillingState.DataValueField = "id";
            ddlBillingState.DataBind();
            ddlBillingState.Items.Insert(0, new ListItem("- Select - ", "0"));
            if (Vendor.BillingStateId > 0) ddlBillingState.SelectedValue = Vendor.BillingStateId.ToString();

            cbIsAffiliatedWithOriginator.Checked = Vendor.IsAffiliatedWithOriginator;
            cbIsAffiliatedWithOriginator.Attributes.Add(ONCLICK, String.Format(SHOWAFFILIATED, trRelationship.ClientID, trCompany.ClientID,trShortDescriptionOfServices.ClientID));
            tbRelationship.Text = Vendor.Relationship;
            tbShortDescriptionOfServices.Text = Vendor.ShortDescriptionOfServices;
            ddlCompany.DataSource = Company.GetRMOCompanyList();
            ddlCompany.DataTextField = "Company";
            ddlCompany.DataValueField = "id";
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, new ListItem("- Select - ", "0"));
            if (Vendor.AffiliatedCompanyId > 0) ddlCompany.SelectedValue = Vendor.AffiliatedCompanyId.ToString();
            string displaystyle = cbIsAffiliatedWithOriginator.Checked ? "display:block" : "display:none";
            trCompany.Attributes.Add("style", displaystyle);
            trRelationship.Attributes.Add("style", displaystyle);
            trShortDescriptionOfServices.Attributes.Add("style", displaystyle);
            hasLogin = Vendor.HasLogin;
            ddlDeliveryMethod.DataSource = VendorGlobal.GetVendorDeliveryMethodList();
            ddlDeliveryMethod.DataTextField = "name";
            ddlDeliveryMethod.DataValueField = "id";
            ddlDeliveryMethod.DataBind();
            ddlDeliveryMethod.SelectedValue = Vendor.DeliveryMethodId.ToString();
            BindLoginInfo();
            EnableTabs(Vendor.ID > 0);
        }
        private void BindLoginInfo()
        {
            trPassword.Visible = hasLogin;
            trSetPassword.Visible = hasLogin;
            trLogin.Visible = hasLogin;
            trConfirmPassword.Visible = hasLogin;
            tbLogin.Text = Vendor.Login;
            if (hasLogin)
            {
                cbSetPassword.Attributes.Add(ONCLICK, String.Format(SETPASSWORDJS, trPassword.ClientID, trConfirmPassword.ClientID));
                trPassword.Attributes.Add("style", "display:none");
                trConfirmPassword.Attributes.Add("style", "display:none");
                cbSetPassword.Checked = false;
            }
        }
        private void goBack()
        {
            Response.Redirect("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.VIEWVENDORS);
        }
        private void EnableTabs(bool isEnabled)
        {
            for (int i = 1; i < VendorInfo.Tabs.Count; i++)
            {
                VendorInfo.Tabs[i].Enabled = isEnabled;
            }
        }
        private string GetFeePostedData()
        {
            string res = String.Empty;
            String[] col = Page.Request.Form.AllKeys;
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].IndexOf("$" + REPEATERID + "$") > 0)
                { 
                    if(col[i].IndexOf("$" + VendorFeeCategory.CHECKBOXID + "_") > 0)
                    {
                        int id = GetControlId(col[i]);
                        decimal amount = GetFeeTypeAmount(col[i]);
                        if (id > 0)
                        {
                            XmlNode n = d.CreateElement(ITEMELEMENT);
                            XmlAttribute a1 = d.CreateAttribute(IDATTRIBUTE);
                            a1.Value = id.ToString();
                            n.Attributes.Append(a1);
                            XmlAttribute a2 = d.CreateAttribute(AMOUNTATTRIBUTE);
                            a2.Value = amount.ToString();
                            n.Attributes.Append(a2);
                            root.AppendChild(n);
                        }
                    }
                }
            }
            if (root.ChildNodes.Count > 0)
            {
                d.AppendChild(root);
                res = d.OuterXml;
            }
            return res;
        }
        private string GetCountyPostedData()
        {
            string res = String.Empty;
            String[] col = Page.Request.Form.AllKeys;
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].IndexOf("$" + dlCounty.ID + "$") > 0)
                {
                    if (col[i].IndexOf("$" + COUNTYCHECKBOXID + "_") > 0)
                    {
                        int id = GetControlId(col[i]);
                        if (id > 0)
                        {
                            XmlNode n = d.CreateElement(ITEMELEMENT);
                            XmlAttribute a1 = d.CreateAttribute(IDATTRIBUTE);
                            a1.Value = id.ToString();
                            n.Attributes.Append(a1);
                            root.AppendChild(n);
                        }
                    }
                }
            }
            if (root.ChildNodes.Count > 0)
            {
                d.AppendChild(root);
                res = d.OuterXml;
            }
            return res;
        }
        private static int GetControlId(string controlId)
        {
            int res = -1;
            int i = controlId.LastIndexOf("_");
            if (i > 0)
            {
                try
                {
                    res = int.Parse(controlId.Substring(i+1));
                }
                catch { }
                
            }
            return res;
        }
        private decimal GetFeeTypeAmount(string controlId)
        {
            decimal res = 0;
            string s = controlId.Replace("$" + VendorFeeCategory.CHECKBOXID, "$" + VendorFeeCategory.RADINPUTID);
            s = Page.Request[s];
            try
            {
                res = Convert.ToDecimal(s);
            }
            catch { }
            return res;
        }
        private int GetSelectedTab(string tabId)
        {
            int res = 0;
            try 
            {
                res = int.Parse(Page.Request[tabId]);
            }
            catch { }
            return res;
        }
        private void ProcessPostBack()
        {
            string controlName = Page.Request["__EVENTTARGET"];
            if (controlName.Contains("$" + STATELINKID + "_"))
            {
                SelectedStateId = GetIndex(controlName);
                selectedGeoTab = 1;
            }
            BindData();
        }
        private string GetStateName(int id)
        {
            string res = "";
            DataRow[] rows = DvStates.Table.Select(String.Format(SELECTNAME, id));
            if (rows.Length == 1)
            {
                res = rows[0]["name"].ToString();
            }
            return res;
        }
        private static int GetIndex(string controlName)
        {
            int res = -1;
            string[] tmp = controlName.Split('_');
            if (tmp.Length > 1)
            {
                try
                {
                    res = int.Parse(tmp[tmp.Length - 1]);
                }
                catch { }
            }
            return res;
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
        private string GetSortDirection(string sortexpression)
        {
            string res;
            Object o = ViewState[ "order_" + sortexpression];
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
        #endregion

        #region event handlers
        #region Always/Never grid handlers
        private void Update(int rowIndex)
        {
            int originatorId = EditItemId;
            if (originatorId < 0)
            {
                DropDownList ddl = (DropDownList)gSettings.Rows[rowIndex].FindControl("ddlOriginator");
                if (ddl != null)
                {
                    originatorId  = Convert.ToInt32(ddl.SelectedValue);
                }
            }
            DropDownList ddl1 = (DropDownList)gSettings.Rows[rowIndex].FindControl("ddlSettings");
            if (ddl1 != null)
            {
                bool isAlways = ddl1.SelectedValue == ALWAYSVALUE;
                Vendor.SaveSettings(originatorId, isAlways);
                dvOriginators = null;
                dvAlwaysNever = null;
            }
        }
        protected static string GetAlwaysNeverSettings(Object item)
        {
            DataRowView dr = (DataRowView)item;
            bool res = bool.Parse(dr["IsAlways"].ToString());
            return res ? ALWAYSTEXT : NEVERTEXT;
        }
        private void CreateFooter(GridViewRowEventArgs e, int colspan)
        {
                int cellcount = e.Row.Cells.Count;
                int n = colspan;
                if ((colspan <= 0) || (colspan >= cellcount))
                {
                    n = cellcount;
                }
                TableCell td = GetAddTd(n);
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
        private TableCell GetAddTd(int colspan)
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
            lb.Text = "Add Originator";
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
                Vendor.DeleteSettings(Convert.ToInt32(e.CommandArgument));
                dvOriginators = null;
                dvAlwaysNever = null;
            }
            else if (e.CommandName == SORTCOMMAND)
            {
                SetSort(e.CommandArgument.ToString());
            }
            else if (e.CommandName == PAGECOMMAND)
            {
                PageIndex = Convert.ToInt32(e.CommandArgument.ToString()) - 1;
            }
            BindAlwaysNeverSettingsGrid();
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
                    int id = Convert.ToInt32(dr["id"].ToString());
                    if (id > 0)
                    {
                        settings = new VendorSettings((DataRowView)e.Row.DataItem);
                    }
                    else
                    {
                        settings = new VendorSettings(Vendor.ID);
                    }
                    EditItemId = settings.Id;
                    Label lbl = (Label)e.Row.FindControl("lblOriginator");
                    if (lbl != null)
                    {
                        lbl.Visible = (GridMode == GRIDEDITMODE);
                    }
                    DropDownList ddl = (DropDownList)e.Row.FindControl("ddlOriginator");
                    if (ddl != null)
                    {
                        ddl.Visible = (GridMode == GRIDADDMODE);
                        if (ddl.Visible)
                        {
                            ddl.DataSource = DvOriginators;
                            ddl.DataTextField = "name";
                            ddl.DataValueField = "id";
                            ddl.DataBind();
                        }
                    }
                    Label lbl1 = (Label)e.Row.FindControl("lblSettings");
                    if (lbl1 != null)
                    {
                        lbl1.Visible = false;
                    }
                    DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddlSettings");
                    if (ddl1 != null)
                    {
                        ddl1.Visible = true;
                        ddl1.Items.Add(new ListItem(ALWAYSTEXT,ALWAYSVALUE));
                        ddl1.Items.Add(new ListItem(NEVERTEXT,NEVERVALUE));
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
                    CreateFooter(e, 0);
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            goBack();
        }

        #region save handlers
        protected void btnSaveCounty_Click(object sender, EventArgs e)
        {
            int res = Vendor.SaveGeoFilter(GetCountyPostedData(),SelectedStateId);
            dvCounties = null;
            dvStates = null;
            BindData();
            if (res == 1)
            {
                lblMessage.Text = Constants.SUCCESSMESSAGE;
            }
            else
            {
                lblMessage.Text = DBERRORMESSAGE;
            }
        }
        protected void btnSaveFeeAmount_Click(object sender, EventArgs e)
        {
            int res = Vendor.SaveFeeTypes(GetFeePostedData());
            selectedFeeTab = GetSelectedTab(mpFees.ClientID.Replace("_","$")+"_Selected");
            dvVendorFee = null;
            BindData();
            if (res == 1)
            {
                lblMessage.Text = Constants.SUCCESSMESSAGE;
            }
            else
            {
                lblMessage.Text = DBERRORMESSAGE;
            }
        }
        protected void btnSaveBasicInfo_Click(object sender, EventArgs e)
        {
            Vendor.Name = tbCompanyName.Text;
            Vendor.CorporateAddress1 = tbCorporateAddress1.Text;
            Vendor.CorporateAddress2 = tbCorporateAddress2.Text;
            Vendor.CompanyPhone = tbCompanyPhone.Text;
            Vendor.CompanyFax = tbCompanyFax.Text;
            Vendor.CompanyEmail = tbCompanyEmail.Text;
            Vendor.CompanyCity = tbCompanyCity.Text;
            Vendor.CompanyZip = tbCompanyZip.Text;
            Vendor.BillingAddress1 = tbBillingAddress1.Text;
            Vendor.BillingAddress2 = tbBillingAddress2.Text;
            Vendor.BillingCity = tbBillingCity.Text;
            Vendor.BillingZip = tbBillingZip.Text;
            Vendor.PrimaryContactName = tbPrimaryContact.Text;
            Vendor.PCPhone1 = tbPCPhone1.Text;
            Vendor.PCPhone2 = tbPCPhone2.Text;
            Vendor.PCEmail = tbPCEmail.Text;
            Vendor.SecondaryContactName = tbSecondaryContact.Text;
            Vendor.SCPhone1 = tbSCPhone1.Text;
            Vendor.SCPhone2 = tbSCPhone2.Text;
            Vendor.SCEmail = tbSCEmail.Text;
            Vendor.LicenseExpDate = dtLicenseExpdate.SelectedDate;
            Vendor.Login = tbLogin.Text;
            Vendor.CompanyStateId = int.Parse(ddlCompanyState.SelectedValue);
            Vendor.BillingStateId = int.Parse(ddlBillingState.SelectedValue);
            Vendor.DeliveryMethodId = int.Parse(ddlDeliveryMethod.SelectedValue);
            if (Vendor.HasLogin)
            {
                if (String.IsNullOrEmpty(Vendor.Login))
                {
                    Vendor.Password = "";
                }
                else
                {
                    if (cbSetPassword.Checked)
                    {
                        Vendor.Password = tbPassword.Text;
                    }
                }
            }
            Vendor.IsAffiliatedWithOriginator = cbIsAffiliatedWithOriginator.Checked;
            if (Vendor.IsAffiliatedWithOriginator)
            {
                Vendor.AffiliatedCompanyId = int.Parse(ddlCompany.SelectedValue);
                Vendor.Relationship = tbRelationship.Text;
                Vendor.ShortDescriptionOfServices = tbShortDescriptionOfServices.Text;
            }
            int res = Vendor.Save();
            if (res > 0)
            {
                VendorId = res;
                Vendor.ID = res;
                lblMessage.Text = Constants.SUCCESSMESSAGE;
                EnableTabs(Vendor.ID > 0);
            }
            else
            {
                if (res == -2)
                {
                    lblMessage.Text = LOGINALREADYEXISTS;
                }
                else 
                {
                    lblMessage.Text = DBERRORMESSAGE;
                }                
            }
            BindData();
        }        
        #endregion

        #region data bindings
        protected void tsFee_TabDataBound(object sender, TabStripEventArgs e)
        {
            PageView pv = new PageView();
            DataRowView dr = (DataRowView)e.Tab.DataItem;
            VendorFeeCategory ctl = LoadControl(Constants.CONTROLSLOCATION + Constants.CTLVENDORFEECATEGORY) as VendorFeeCategory;
            if (ctl != null)
            {
                int categoryId = int.Parse(dr["id"].ToString());
                ctl.ID = String.Format(CATEGORYID,categoryId);
                ctl.BindData(DvVendorFee, categoryId);
                pv.Controls.Add(ctl);
            }
            mpFees.PageViews.Add(pv);
        }
        protected void dlCounties_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            if (dr != null)
            {
                int id = int.Parse(dr["id"].ToString());
                string name = dr["name"].ToString();
                bool isChecked = bool.Parse(dr["selected"].ToString());
                allCountiesSelected &= isChecked;
                CheckBox cb = (CheckBox)e.Item.FindControl(COUNTYCHECKBOXID);
                if (cb != null)
                {
                    cb.Checked = isChecked;
                    cb.Text = name;
                    cb.ID = COUNTYCHECKBOXID + "_" + id;
                    cb.CssClass = isChecked ? "lbstateallcounty" : "lbstatenocounty";
                    cb.Attributes.Add(ONCLICK, String.Format(CHECKFIELDJS, cbAllCounty.ClientID, countydiv.ClientID));
                }
            }
        }
        protected void dlStates_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            if (dr != null)
            {
                int id = int.Parse(dr["id"].ToString());
                string name = dr["name"].ToString();
                int count = int.Parse(dr["count"].ToString());
                LinkButton lb = (LinkButton)e.Item.FindControl(STATELINKID);
                if (lb != null) 
                {
                    lb.Text = name;
                    lb.ID = STATELINKID + "_" + id;
                    string css = "lbstatenocounty";
                    if(count<0)
                    {
                        css = "lbstateallcounty";
                    }
                    if(count>0)
                    {
                        css = "lbstatecounty";
                    }
                    lb.CssClass = css;
                }
            }
        }
        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsLoanStarAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }            
            lblMessage.Text = "";
            cbAllCounty.Attributes.Add(ONCLICK, String.Format(CHECKALLJS, countydiv.ClientID));
            if (!Page.IsPostBack)
            {
                VendorId = CurrentPage.GetValueInt(Constants.IDPARAM, -1);
                SelectedStateId = -1;
                BindData();
            }
            else
            {
                ProcessPostBack();
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            tsGeoFilter.SelectedIndex = selectedGeoTab;
            mpGeoFilter.SelectedIndex = tsGeoFilter.SelectedIndex;
            cbAllCounty.Checked = allCountiesSelected;
        }

    }
}