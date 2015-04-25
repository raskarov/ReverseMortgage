using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using LoanStar.Common;
using Telerik.WebControls; 

namespace LoanStarPortal.Controls
{
    public partial class ClosingCostProfileEdit : EditGridFormControl
    {
        #region constants
        public const string INVOICE = "currentinvoice";
        private const string ORIGINATORLIST = "{0} is the provider";
        private const string FOCUSLOSTGOVERNMENTJS = "AmountGovernmentFocusLost(this,{0});";
        private const string FOCUSLOSTJS = "AmountFocusLost(this);";
        protected const string ONBLUR = "onblur";
        private const string LABELTDCLASS = "editFormLabel";
        private const string CONTROLTDCLASS = "";
        private const string DDLCSS = "ddl";
        private const int DDLWIDTH = 150;
        private const string TBCSS = "ddl";
        private const int TBWIDTH = 145;
        private const int RADTBWIDTH = 150;

        #region controls' id
        private const string DDLFEECATEGORYID = "ddlFeeCategory";
        private const string DDLTYPEID = "ddlType";
        private const string TBOTHERTYPEFEEDESCRIPTIONID = "tbOtherDescription";
        private const string DDLPROVIDERID = "ddlProvider";
        private const string TBPROVIDERID = "tbProvider";
        private const string LBLPROVIDERID = "lblProvider";
        private const string DDLPROVIDERTYPEID = "ddlProviderType";
        private const string TBLISTENDORSEMENSID = "tbListendorsements";
        private const string TBAMOUNTID = "tbAmount1";
        private const string TBPOCAMOUNTID = "tbAmount2";
        private const string TBDEEDAMOUNTID = "tbAmount3";
        private const string TBMORTGAGEAMOUNTID = "tbAmount4";
        private const string TBRELEASEAMOUNTID = "tbAmount5";
        private const string TBLENDERCREDITAMOUNTID = "tbAmount6";
        private const string TBTHIRDPARTYPAIDAMOUNTID = "tbAmount7";
        private const string LBLFINANCEDAMOUNTID = "lblFinancedAmountValue";
        private const string CBCALCULATEDFEEID = "cbCalculateFee";
        private const string DDLPROVIDERFORFORMULAFEEID = "ddlProviderForFormulaFee";
        private const string LBLTITLENAMEID = "lblTitleName";
        private const string DDLFORMULAID = "ddlFormula";
        private const string DDLSTATEID = "ddlState";
        #endregion

        #endregion

        #region fields
        ArrayList LeftColumn;
        ArrayList RightColumn;
        private LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail profileDetail;
        private string objectName;
        private bool enableValidation;
        private DataView dvCategoryList;
        private DataView dvStatusList;
        private DataView dvChargeToList;
//        private MortgageProfile mp;
        private DataView dvProviderType;
        private int defaultProviderTypeId = Invoice.VENDORPROVIDERTYPEID;
        private bool isFeeTypeSelected = false;
        private short tabIndex = 0;
        private DataView dvFormulaList;
        private DataView dvOfficerTitleList;
        private FeeFormulaControl feeFormulaControl;
        private DataView dvStateList;
        private bool isLateBinding = false;
        #endregion

        #region properties
        private DataView DvStateList
        {
            get
            {
                if (dvStateList == null)
                {
                    dvStateList = CurrentInvoice.GetStateList();
                }
                return dvStateList;
            }
        }
        protected DataView DvFormulaList
        {
            get
            {
                if (dvFormulaList == null)
                {
                    dvFormulaList = Invoice.GetFormulaList(CurrentInvoice.TypeId); ;
                }
                return dvFormulaList;
            }
        }
        private bool IsProviderTypeSelected
        {
            get
            {
                bool res = false;
                Object o = Session["providertypeselected"];
                if (o != null)
                {
                    res = bool.Parse(o.ToString());
                }
                return res;
            }
            set
            {
                Session["providertypeselected"] = value;
            }
        }
        public override string ObjectName
        {
            get { return objectName; }
            set { objectName = value; }
        }
        public override object EditObject
        {
            set { profileDetail = value as LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail; }
        }
        public override bool EnableValidation
        {
            get { return enableValidation; }
            set { enableValidation = value; }
        }
        protected DataView DvProviderType
        {
            get
            {
                if (dvProviderType == null)
                {
                    dvProviderType = Invoice.GetProviderType();
                }
                return dvProviderType;
            }
        }
        protected DataView DvCategoryList
        {
            get
            {
                if (dvCategoryList == null)
                {
                    dvCategoryList = Invoice.GetInvoiceFeeCategoryList();
                }
                return dvCategoryList;
            }
        }
        protected DataView DvStatusList
        {
            get
            {
                if (dvStatusList == null)
                {
                    dvStatusList = Invoice.GetInvoiceStatusList();
                }
                return dvStatusList;
            }
        }
        protected DataView DvChargeToList
        {
            get
            {
                if (dvChargeToList == null)
                {
                    dvChargeToList = Invoice.GetInvoiceChargeToList();
                }
                return dvChargeToList;
            }
        }
        protected int MortgageProfileID
        {
            get
            {
                if (Session[Constants.MortgageID] == null)
                    Session[Constants.MortgageID] = 0;
                return Convert.ToInt32(Session[Constants.MortgageID].ToString());
            }
            set
            {
                Session[Constants.MortgageID] = value;
            }
        }
        protected LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail CurrentInvoice
        {
            get
            {
                LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail res = Session[INVOICE] as LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail;
                if (res == null)
                {
                    res = profileDetail;
                    Session[INVOICE] = res;
                }
                return res;
            }
            set { Session[INVOICE] = value; }
        }
        protected DataView DvOfficerTitleList
        {
            get
            {
                if (dvOfficerTitleList == null)
                {
                    dvOfficerTitleList = Invoice.GetTitleOfficerList();
                }
                return dvOfficerTitleList;
            }
        }
        #endregion

        #region Methods

        #region data binding methods
        protected static void AddEmptyItem(DropDownList ddl)
        {
            ListItem li = new ListItem("-Select-", "");
            ddl.Items.Insert(0, li);
        }
        private void BindFeeCategory()
        {
            DropDownList ddl = (DropDownList)FindControl(DDLFEECATEGORYID);
            if (ddl != null)
            {
                ddl.DataSource = DvCategoryList;
                ddl.DataTextField = "name";
                ddl.DataValueField = "id";
                ddl.DataBind();
                ddl.SelectedValue = CurrentInvoice.FeeCategoryId.ToString();
                ddl.Enabled = IsFieldEditable("FeeCategoryID");
                if (CurrentInvoice.ID <= 0)
                {
                    AddEmptyItem(ddl);
                }
            }
        }
        private void BindFeeType()
        {
            DropDownList ddl = (DropDownList)FindControl(DDLTYPEID);
            if (ddl != null)
            {
                if (CurrentInvoice.FeeCategoryId > 0)
                {
                    DataView dv = CurrentInvoice.GetFeeTypes();
                    ddl.DataSource = dv;
                    ddl.DataTextField = "name";
                    ddl.DataValueField = "id";
                    ddl.DataBind();
                    ddl.ClearSelection();
                    ListItem li = ddl.Items.FindByValue(CurrentInvoice.TypeId.ToString());
                    if (li != null) li.Selected = true;
                    defaultProviderTypeId = GetDefaultProviderType(dv);
                }
                AddEmptyItem(ddl);
                ddl.Enabled = IsFieldEditable("TypeID") && (CurrentInvoice.FeeCategoryId > 0);
                isFeeTypeSelected = ddl.Enabled && (CurrentInvoice.TypeId > 0);
            }
        }
        private void SetDescription()
        {
            if (Invoice.IsOtherFeeType(CurrentInvoice.TypeId))
            {
                TextBox tb = (TextBox)FindControl(TBOTHERTYPEFEEDESCRIPTIONID);
                if (tb != null)
                {
                    tb.Text = profileDetail.Description;
                    tb.ReadOnly = !IsFieldEditable("Description");
                }
            }
        }
        private int GetDefaultProviderType(DataView dv)
        {
            int res = Invoice.VENDORPROVIDERTYPEID;
            dv.RowFilter = String.Format("id={0}", CurrentInvoice.TypeId);
            if (dv.Count == 1)
            {
                res = int.Parse(dv[0]["DefaultProviderTypeId"].ToString());
            }
            return res;
        }
        private void BindProvider()
        {
            DropDownList ddl = (DropDownList)FindControl(DDLPROVIDERTYPEID);
            if (ddl != null)
            {
                ddl.Items.Clear();
                if (CurrentInvoice.TypeId > 0)
                {
                    int vendorsCount = 0;
                    if (!Invoice.IsTypeOther(CurrentInvoice.TypeId))
                    {
                        DataView dv = CurrentInvoice.GetVendors(CurrentUser.CompanyId);
                        if (dv.Count > 0)
                        {
                            vendorsCount = dv.Count;
                            AddProviderType(ddl, Invoice.VENDORPROVIDERTYPEID, String.Empty);
                            DropDownList ddlv = (DropDownList)FindControl(DDLPROVIDERID);
                            if (ddlv != null)
                            {
                                ddlv.DataSource = dv;
                                ddlv.DataTextField = "Name";
                                ddlv.DataValueField = "ID";
                                ddlv.DataBind();
                                if (CurrentInvoice.ProviderId > 0)
                                {
                                    try
                                    {
                                        ddlv.Items.FindByValue(CurrentInvoice.ProviderId.ToString()).Selected = true;
                                    }
                                    catch { }
                                }
                                ddlv.Enabled = isFeeTypeSelected;
                            }
                        }
                        if(CurrentInvoice.FeeCategoryId != MortgageProfile.FEECATEGORYITEMSPAYADVANCE)
                        {
                            ddl.Items.Add(new ListItem("Use preferred vendors", Invoice.PREFFEREDVENDORTYPEID.ToString()));
                        }
                    }
                    if (vendorsCount == 0)
                    {
                        AddProviderType(ddl, Invoice.OTHERPROVIDERTYPEID, String.Empty);
                        if (defaultProviderTypeId == Invoice.VENDORPROVIDERTYPEID)
                        {
                            defaultProviderTypeId = Invoice.OTHERPROVIDERTYPEID;
                        }
                    }
                    if (CurrentInvoice.FeeCategoryId == MortgageProfile.FEECATEGORYLENDERCHARGE)
                    {
                        AddProviderType(ddl, Invoice.ORIGINATORPROVIDERTYPEID, String.Format(ORIGINATORLIST, CurrentUser.CompanyName));
                        //Company lender = mp.MortgageInfo.LenderAffiliate;
                        //if (lender != null)
                        //{
                            AddProviderType(ddl, Invoice.LENDERPROVIDERTYPEID, "Lender is the provider");
                        //}
                    }
                    if (CurrentInvoice.FeeCategoryId == MortgageProfile.FEECATEGORYGOVERMENTCHARGE)
                    {
                        AddProviderType(ddl, Invoice.COUNTYRECORDERTYPEID, "County recorder");
                        defaultProviderTypeId = Invoice.COUNTYRECORDERTYPEID;
                    }
                }
                else if ((CurrentInvoice.FeeCategoryId == MortgageProfile.FEECATEGORYGOVERMENTCHARGE))
                {
                    AddProviderType(ddl, Invoice.COUNTYRECORDERTYPEID, "County recorder");
                    defaultProviderTypeId = Invoice.COUNTYRECORDERTYPEID;
                }
                else
                {
                    CurrentInvoice.ProviderTypeId = Invoice.OTHERPROVIDERTYPEID;
                    AddProviderType(ddl, Invoice.OTHERPROVIDERTYPEID, String.Empty);
                    defaultProviderTypeId = Invoice.OTHERPROVIDERTYPEID;
                }
            }
            Label lbl = (Label)FindControl(LBLPROVIDERID);
            if (lbl != null)
            {
                lbl.Text = "Provider:";
            }
            TextBox tb = (TextBox)FindControl(TBPROVIDERID);
            if (tb != null)
            {
                tb.Text = CurrentInvoice.OtherProviderName;
                tb.Enabled = isFeeTypeSelected;
                tb.MaxLength = 100;
                if (CurrentInvoice.ProviderTypeId == Invoice.OTHERPROVIDERTYPEID)
                {
                    tb.Visible = true;
                    if (lbl != null) lbl.Visible = true;
                }
                else
                {
                    tb.Visible = false;
                    if (lbl != null) lbl.Visible = false;
                    RequiredFieldValidator rf = (RequiredFieldValidator) FindControl("rf" + TBPROVIDERID);
                    if(rf!=null)
                    {
                        rf.Visible = false;
                    }
                }
            }
            if (!IsProviderTypeSelected)
            {
                CurrentInvoice.ProviderTypeId = CurrentInvoice.ID > 0 ? CurrentInvoice.ProviderTypeId : defaultProviderTypeId;
            }
            if (ddl != null) ddl.Enabled = isFeeTypeSelected;
        }
        private bool CheckForVendors()
        {
            bool res = false;
            if (CurrentInvoice.TypeId > 0)
            {
                if (!Invoice.IsTypeOther(CurrentInvoice.TypeId))
                {
                    DataView dv = CurrentInvoice.GetVendors(CurrentUser.CompanyId);
                    res = dv.Count > 0;
                }
            }
            return res;
        }
        private void AddProviderType(ListControl ddl, int id, string name)
        {
            DataView dv = DvProviderType;
            dv.RowFilter = String.Format("id={0}", id);
            if (dv.Count == 1)
            {
                string text = String.IsNullOrEmpty(name) ? dv[0]["name"].ToString() : name;
                ddl.Items.Add(new ListItem(text, dv[0]["id"].ToString()));
            }
        }
        private void BindEndorsments()
        {
            if (CurrentInvoice.TypeId == Invoice.TITLEENDORSMENTSFEETYPEID)
            {
                TextBox tb = (TextBox)FindControl(TBLISTENDORSEMENSID);
                if (tb != null)
                {
                    tb.Text = CurrentInvoice.Listendorsements;
                }
            }
        }
        private void SetAmountField(string tbid, decimal amount, string fieldName, string js)
        {
            RadNumericTextBox tb = (RadNumericTextBox)FindControl(tbid);
            if (tb != null)
            {
                tb.Text = amount.ToString();
                tb.Enabled = IsFieldEditable(fieldName);
                if (!String.IsNullOrEmpty(js))
                {
                    tb.Attributes.Add(ONBLUR, js);
                }
            }
        }
        private void BindAmounts()
        {
            string js = FOCUSLOSTJS;
            if (CurrentInvoice.FeeCategoryId == Invoice.GOVERNMENTCHARGECATEGORYID)
            {
                js = String.Format(FOCUSLOSTGOVERNMENTJS, CurrentInvoice.TypeId == Invoice.RECORDINGFEETYPEID ? 1 : 0);
                SetAmountField(TBDEEDAMOUNTID, CurrentInvoice.DeedAmount, "DeedAmount", js);
                SetAmountField(TBMORTGAGEAMOUNTID, CurrentInvoice.MortgageAmount, "MortgageAmount", js);
                if (CurrentInvoice.TypeId == Invoice.RECORDINGFEETYPEID)
                {
                    SetAmountField(TBRELEASEAMOUNTID, CurrentInvoice.ReleaseAmount, "ReleaseAmount", js);
                }
            }
            else
            {
                SetAmountField(TBAMOUNTID, CurrentInvoice.Amount, "Amount", js);
            }
            SetAmountField(TBPOCAMOUNTID, CurrentInvoice.POCAmount, "POCAmount", js);
            SetAmountField(TBLENDERCREDITAMOUNTID, CurrentInvoice.LenderCreditAmount, "LenderCreditAmount", js);
            SetAmountField(TBTHIRDPARTYPAIDAMOUNTID, CurrentInvoice.ThirdPartyPaidAmount, "ThirdPartyPaidAmount", js);
            Label lbl = (Label)FindControl(LBLFINANCEDAMOUNTID);
            if (lbl != null)
            {
                lbl.Text = CurrentInvoice.AmountFinanced.ToString("C");
            }
        }
        private void SetCalculatedFeeCheckBox()
        {
            if (CurrentInvoice.TypeId == MortgageProfile.TYPELENDERLOANORIGINATIONFEE)
            {
                CheckBox cb = (CheckBox)FindControl(CBCALCULATEDFEEID);
                if (cb != null)
                {
                    if (CurrentInvoice.CalculateFee)
                    {
                        cb.Checked = true;
                        RadNumericTextBox tb = (RadNumericTextBox)FindControl(TBAMOUNTID);
                        RequiredFieldValidator rv = (RequiredFieldValidator)FindControl("rf"+TBAMOUNTID);
                        if (tb != null)
                        {
                            tb.Text = "";
                            tb.ReadOnly = true;
                            tb.BackColor = System.Drawing.Color.DarkGray;
                        }
                        if(rv!=null)
                        {
                            rv.Enabled = false;
                        }
                    }
                    Label lbl = (Label)FindControl(LBLFINANCEDAMOUNTID);
                    if (lbl != null)
                    {
                        lbl.Text = "";
                    }
                }
            }
        }
        public override void BindData()
        {
//            mp = CurrentPage.GetMortgage(MortgageProfileID);
            ProcessPostBack();
            BuildTable();
            BindFeeCategory();
            BindFeeType();
            SetDescription();
            if (!CurrentInvoice.HasFormula||(CurrentInvoice.TypeId==Invoice.TITLEINSURANCEFEETYPEID)) BindProvider();
            BindEndorsments();
            BindAmounts();
            if (CurrentInvoice.HasFormula)
            {
                if (!isLateBinding) BindFormula();
            }
            string s = "Cancel";
            string css = "publicEditFormButton";
            if(CurrentInvoice.HasFormula)
            {
                s = "Cancel/Close";
                css = "publicEditFormButtonWide";
            }
            btnCancel.Text = s;
            btnCancel.CssClass = css;
        }
/*
        private static void AddTotalCost(ListControl ddl)
        {
            ListItem li = ddl.Items.FindByValue(Invoice.RECORDINGFEETOTALCOSTFORMULAID.ToString());
            if(li==null)
            {
                ddl.Items.Insert(0,new ListItem("Total cost",Invoice.RECORDINGFEETOTALCOSTFORMULAID.ToString()));
            }
        }
*/

        private void BindFormula()
        {
            DropDownList ddl = (DropDownList)FindControl(DDLFORMULAID);
            if (ddl != null)
            {
                DataView dv = DvFormulaList;
                ddl.DataSource = dv;
                ddl.DataTextField = "name";
                ddl.DataValueField = "id";
                ddl.DataBind();
                int formulaId = CurrentInvoice.FormulaId;
                ListItem li = ddl.Items.FindByValue(formulaId.ToString());
                if (li != null)
                {
                    li.Selected = true;
                }
                feeFormulaControl.BindData(this);
            }
            int stateId = 0;
            try
            {
                stateId = CurrentInvoice.StateId;
            }
            catch
            {
            }
            DropDownList dds = (DropDownList)FindControl(DDLSTATEID);
            if(dds!=null)
            {
                DataView dv = DvStateList;
                if(dv.Count>0)
                {
                    dds.DataSource = dv;
                    dds.DataTextField = "name";
                    dds.DataValueField = "id";
                    dds.DataBind();
                    if (stateId == 0)
                    {
                        stateId = int.Parse(dds.Items[0].Value);
                    }
                    ListItem li = dds.Items.FindByValue(stateId.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                    else
                    {
                        CurrentInvoice.StateId = int.Parse(dds.Items[0].Value);
                    }
                }
                else
                {
                    dds.Items.Add(new ListItem("- Select -","0"));
                    dds.Enabled = false;
                }
            }
            BindFormulaProvider();
        }
        private void BindFormulaProvider()
        {
            if(CurrentInvoice.TypeId!=Invoice.TITLEINSURANCEFEETYPEID)
            {
                DropDownList ddp = (DropDownList)FindControl(DDLPROVIDERFORFORMULAFEEID);
                if (ddp != null)
                {
                    DataView dv = DvOfficerTitleList;
                    dv.RowFilter = String.Format("StateId={0}", CurrentInvoice.StateId);
                    ddp.DataSource = dv;
                    ddp.DataTextField = "name";
                    ddp.DataValueField = "id";
                    ddp.DataBind();
                    ddp.Items.Add(new ListItem("Other", "0"));

                    if (CurrentInvoice.FormulaData.ContainsKey("OfficerTitleId"))
                    {
                        int titleId = int.Parse(CurrentInvoice.FormulaData["OfficerTitleId"].ToString());
                        if (titleId >= 0)
                        {
                            ddp.SelectedValue = titleId.ToString();
                        }
                        if (titleId == 0)
                        {
                            TextBox tb = (TextBox)FindControl(TBPROVIDERID);
                            if (tb != null)
                            {
                                tb.Text = CurrentInvoice.OtherProviderName;
                            }
                        }
                    }
                }
            }
        }

        #endregion
        private void GetAmounts()
        {
            for (int i = 0; i < Page.Request.Form.AllKeys.Length; i++)
            {
                string key = Page.Request.Form.AllKeys[i];
                if (key.EndsWith("$EditFormGridControl$" + TBAMOUNTID))
                {
                    try
                    {
                        CurrentInvoice.Amount = decimal.Parse(Page.Request.Form[key]);
                    }
                    catch { }
                }
                if (key.EndsWith("$EditFormGridControl$" + TBPOCAMOUNTID))
                {
                    try
                    {
                        CurrentInvoice.POCAmount = decimal.Parse(Page.Request.Form[key]);
                    }
                    catch { }
                }
                if (key.EndsWith("$EditFormGridControl$" + TBLENDERCREDITAMOUNTID))
                {
                    try
                    {
                        CurrentInvoice.LenderCreditAmount = decimal.Parse(Page.Request.Form[key]);
                    }
                    catch { }
                }
                if (key.EndsWith("$EditFormGridControl$" + TBTHIRDPARTYPAIDAMOUNTID))
                {
                    try
                    {
                        CurrentInvoice.ThirdPartyPaidAmount = decimal.Parse(Page.Request.Form[key]);
                    }
                    catch { }
                }
                if (key.EndsWith("$EditFormGridControl$" + TBDEEDAMOUNTID))
                {
                    try
                    {
                        CurrentInvoice.DeedAmount = decimal.Parse(Page.Request.Form[key]);
                    }
                    catch { }
                }
                if (key.EndsWith("$EditFormGridControl$" + TBMORTGAGEAMOUNTID))
                {
                    try
                    {
                        CurrentInvoice.MortgageAmount = decimal.Parse(Page.Request.Form[key]);
                    }
                    catch { }
                }
                if (key.EndsWith("$EditFormGridControl$" + TBRELEASEAMOUNTID))
                {
                    try
                    {
                        CurrentInvoice.ReleaseAmount = decimal.Parse(Page.Request.Form[key]);
                    }
                    catch { }
                }
            }
        }
        private void ProcessPostBack()
        {
            string control = Page.Request.Form["__EVENTTARGET"];
            if (!String.IsNullOrEmpty(control))
            {
                if (control.EndsWith(":" + DDLFEECATEGORYID))
                {
                    CurrentInvoice.FeeCategoryId = GetPostBackValue(control);
                    CurrentInvoice.TypeId = 0;
                    IsProviderTypeSelected = false;
                }
                else if (control.EndsWith(":" + DDLTYPEID))
                {
                    CurrentInvoice.TypeId = GetPostBackValue(control);
                    IsProviderTypeSelected = false;
                }
                else if (control.EndsWith(":" + DDLPROVIDERTYPEID))
                {
                    CurrentInvoice.ProviderTypeId = GetPostBackValue(control);
                    IsProviderTypeSelected = true;
                }
                else if (control.EndsWith(":" + CBCALCULATEDFEEID))
                {
                    CurrentInvoice.CalculateFee = GetCheckBoxPostBackValue(control);
                }
                else if (control.EndsWith(":" + DDLFORMULAID))
                {
                    CurrentInvoice.FormulaId = GetPostBackValue(control);
                    isLateBinding = true;
                }
                else if (control.EndsWith(":" + DDLPROVIDERFORFORMULAFEEID))
                {
                    int typeId = GetPostBackValue(control);
                    CurrentInvoice.SetFormulaData("OfficerTitleId", typeId.ToString());
                }
                else if (control.EndsWith(":" + DDLSTATEID))
                {
                    CurrentInvoice.StateId = GetPostBackValue(control);
                    isLateBinding = true;
                }
                GetAmounts();
            }
        }
        private bool GetCheckBoxPostBackValue(string control)
        {
            bool res = false;
            string s = Page.Request.Form[control.Replace(":", "$")];
            if (!String.IsNullOrEmpty(s))
            {
                res = s.ToLower() == "on";
            }
            return res;
        }
        private int GetPostBackValue(string control)
        {
            int res = -1;
            try
            {
                res = int.Parse(Page.Request.Form[control.Replace(":", "$")]);
            }
            catch
            {
            }
            return res;
        }
        #endregion

        #region Event Handlers
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
            CurrentInvoice = null;
        }
        private int GetDdlValue(string controlId)
        {
            int res = 0;
            DropDownList ddl = (DropDownList)FindControl(controlId);
            if (ddl != null)
            {
                try
                {
                    res = int.Parse(ddl.SelectedValue);
                }
                catch { }
            }
            return res;
        }
        private decimal GetRadTextBoxValue(string controlId)
        {
            decimal res = 0;
            RadNumericTextBox tb = (RadNumericTextBox)FindControl(controlId);
            if (tb != null)
            {
                try
                {
                    res = decimal.Parse(tb.Text);
                }
                catch { }
            }
            return res;
        }
        private string GetTextBoxValue(string controlId)
        {
            string res = String.Empty;
            TextBox tb = (TextBox)FindControl(controlId);
            if (tb != null)
            {
                res = tb.Text;
            }
            return res;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            CurrentInvoice.FeeCategoryId = GetDdlValue(DDLFEECATEGORYID);
//            CurrentInvoice.StatusID = GetDdlValue(DDLSTATUSID);
            CurrentInvoice.ProviderTypeId = GetDdlValue(DDLPROVIDERTYPEID);
            if(CurrentInvoice.HasFormula)
            {
                CurrentInvoice.StateId = GetDdlValue(DDLSTATEID);
                profileDetail.StateId = CurrentInvoice.StateId;
                CurrentInvoice.FormulaId = GetDdlValue(DDLFORMULAID);
                profileDetail.FormulaId = CurrentInvoice.FormulaId;
                profileDetail.SetFormulaData("StateId",profileDetail.StateId.ToString());
                profileDetail.SetFormulaData("FormulaId", profileDetail.FormulaId.ToString());
            }
            profileDetail.FeeCategoryId = CurrentInvoice.FeeCategoryId;
            profileDetail.ID = CurrentInvoice.ID;
            if (CurrentInvoice.ProviderTypeId == Invoice.VENDORPROVIDERTYPEID)
            {
                CurrentInvoice.ProviderId = GetDdlValue(DDLPROVIDERID); 
            }
            if (profileDetail.TypeId != CurrentInvoice.TypeId)
            {
                profileDetail.TypeId = CurrentInvoice.TypeId;
            }
            if (profileDetail.ProviderId != CurrentInvoice.ProviderId)
            {
                profileDetail.ProviderId = CurrentInvoice.ProviderId;
            }
            if (profileDetail.ProviderTypeId != CurrentInvoice.ProviderTypeId)
            {
                profileDetail.ProviderTypeId = CurrentInvoice.ProviderTypeId;
            }
            if (CurrentInvoice.HasFormula)
            {
                feeFormulaControl.SetInvoiceData(profileDetail, this, new ArrayList(), CurrentPage.CurrentUser.Id);
                if (CurrentInvoice.TypeId!=Invoice.TITLEINSURANCEFEETYPEID)
                {
                    int providerTypeId = GetDdlValue(DDLPROVIDERFORFORMULAFEEID);
                    profileDetail.SetFormulaData("OfficerTitleId", providerTypeId.ToString());
                    string officerTitle = "";
                    DropDownList ddl = (DropDownList)FindControl(DDLPROVIDERFORFORMULAFEEID);
                    if (ddl != null)
                    {
                        officerTitle = ddl.SelectedItem.Text;
                    }
                    profileDetail.SetFormulaData("OfficerTitle", officerTitle);
                    profileDetail.ProviderTypeId = Invoice.COUNTYRECORDERTYPEID;
                    profileDetail.OtherProviderName = GetTextBoxValue(TBPROVIDERID);
                }
                else
                {
                    if (CurrentInvoice.ProviderTypeId == Invoice.OTHERPROVIDERTYPEID)
                    {
                        CurrentInvoice.OtherProviderName = GetTextBoxValue(TBPROVIDERID);
                        if (profileDetail.OtherProviderName != CurrentInvoice.OtherProviderName)
                        {
                            profileDetail.OtherProviderName = CurrentInvoice.OtherProviderName;
                        }
                    }
                }
            }
            else
            {
                decimal amount;
                if (CurrentInvoice.FeeCategoryId == Invoice.GOVERNMENTCHARGECATEGORYID)
                {
                    amount = GetRadTextBoxValue(TBDEEDAMOUNTID);
                    if (profileDetail.Amount != amount)
                    {
                        profileDetail.DeedAmount = amount;
                    }
                    amount = GetRadTextBoxValue(TBMORTGAGEAMOUNTID);
                    if (profileDetail.Amount != amount)
                    {
                        profileDetail.MortgageAmount = amount;
                    }
                    if (CurrentInvoice.TypeId == Invoice.RECORDINGFEETYPEID)
                    {
                        amount = GetRadTextBoxValue(TBRELEASEAMOUNTID);
                        if (profileDetail.Amount != amount)
                        {
                            profileDetail.ReleaseAmount = amount;
                        }
                    }
                }
                else
                {
                    amount = GetRadTextBoxValue(TBAMOUNTID);
                    if (profileDetail.Amount != amount)
                    {
                        profileDetail.Amount = amount;
                    }
                }
                if (CurrentInvoice.TypeId == Invoice.TITLEENDORSMENTSFEETYPEID)
                {
                    CurrentInvoice.Listendorsements = GetTextBoxValue(TBLISTENDORSEMENSID);
                    if (profileDetail.Listendorsements != CurrentInvoice.Listendorsements)
                    {
                        profileDetail.Listendorsements = CurrentInvoice.Listendorsements;
                    }
                }
                if (Invoice.IsOtherFeeType(CurrentInvoice.TypeId))
                {
                    CurrentInvoice.Description = GetTextBoxValue(TBOTHERTYPEFEEDESCRIPTIONID);
                    if (profileDetail.Description != CurrentInvoice.Description)
                    {
                        profileDetail.Description = CurrentInvoice.Description;
                    }
                }
                if (CurrentInvoice.ProviderTypeId == Invoice.OTHERPROVIDERTYPEID)
                {
                    CurrentInvoice.OtherProviderName = GetTextBoxValue(TBPROVIDERID);
                    if (profileDetail.OtherProviderName != CurrentInvoice.OtherProviderName)
                    {
                        profileDetail.OtherProviderName = CurrentInvoice.OtherProviderName;
                    }
                }
                if (CurrentInvoice.TypeId == MortgageProfile.TYPELENDERLOANORIGINATIONFEE)
                {
                    profileDetail.CalculateFee = CurrentInvoice.CalculateFee;
                }
                decimal pocamount = GetRadTextBoxValue(TBPOCAMOUNTID);
                if (profileDetail.POCAmount != pocamount)
                {
                    profileDetail.POCAmount = pocamount;
                }
                decimal d = GetRadTextBoxValue(TBLENDERCREDITAMOUNTID);
                if (profileDetail.LenderCreditAmount != amount)
                {
                    profileDetail.LenderCreditAmount = d;
                }
                d = GetRadTextBoxValue(TBTHIRDPARTYPAIDAMOUNTID);
                if (profileDetail.ThirdPartyPaidAmount != d)
                {
                    profileDetail.ThirdPartyPaidAmount = d;
                }
            }
            Save(profileDetail, new ArrayList() );
            if(!profileDetail.HasFormula)
            {
                CurrentInvoice = null;
            }
            else
            {
                CurrentInvoice = profileDetail;
                lblMessage.Text = Constants.SUCCESSMESSAGE;
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetCalculatedFeeCheckBox();
            DropDownList ddl = (DropDownList)FindControl(DDLPROVIDERTYPEID);
            if (ddl != null)
            {
                ddl.ClearSelection();
                ListItem li = ddl.Items.FindByValue(CurrentInvoice.ProviderTypeId.ToString());
                if (li != null)
                {
                    li.Selected = true;
                }
            }
            if (isLateBinding)
            {
                BindFormula();
            }
        }
        #endregion

        #region dynamic table methods
        private static CustomValidator GetCustomValidator(string controlId, string errMessage, string func)
        {
            CustomValidator v = new CustomValidator();
            v.ID = "cv" + controlId;
            v.ControlToValidate = controlId;
            v.ErrorMessage = errMessage;
            v.ValidateEmptyText = true;
            v.ClientValidationFunction = func;
            return v;
        }
        private static RequiredFieldValidator GetReqiuiredFieldValidator(string controlId, string errMessage)
        {
            RequiredFieldValidator v = new RequiredFieldValidator();
            v.ID = "rf" + controlId;
            v.ControlToValidate = controlId;
            v.ErrorMessage = errMessage;
            return v;
        }
        private static RangeValidator GetRangeValidator(string controlId, string errMessage, double min, double max)
        {
            RangeValidator v = new RangeValidator();
            v.ID = "rv" + controlId;
            v.ControlToValidate = controlId;
            v.ErrorMessage = errMessage;
            v.Type = ValidationDataType.Double;
            v.MaximumValue = max.ToString();
            v.MinimumValue = min.ToString();
            return v;
        }
        private DropDownList GetDropDownListControl(string id, bool isPostBack, string css, int width)
        {
            DropDownList ddl = new DropDownList();
            ddl.ID = id;
            ddl.TabIndex = tabIndex;
            tabIndex++;
            ddl.AutoPostBack = isPostBack;
            if (!String.IsNullOrEmpty(css))
            {
                ddl.CssClass = css;
            }
            if (width > 0)
            {
                ddl.Width = Unit.Pixel(width);
            }
            return ddl;
        }
        private static Label GetLabel(string text, string id)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.ID = id;
            return lbl;
        }
        private TextBox GetTextBox(string id, string css, int width)
        {
            TextBox tb = new TextBox();
            tb.ID = id;
            tb.TabIndex = tabIndex;
            tabIndex++;
            if (!String.IsNullOrEmpty(css))
            {
                tb.CssClass = css;
            }
            if (width > 0)
            {
                tb.Width = Unit.Pixel(width);
            }
            return tb;
        }
        private RadNumericTextBox GetNumericTextBox(string id)
        {
            RadNumericTextBox tb = new RadNumericTextBox();
            tb.Skin = "Default";
            tb.Type = NumericType.Currency;
            tb.Width = Unit.Pixel(RADTBWIDTH);
            tb.ID = id;
            tb.TabIndex = tabIndex;
            tabIndex++;
            return tb;
        }
        #region left column methods
        private ArrayList GetFeeCategoryControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetDropDownListControl(DDLFEECATEGORYID, true, DDLCSS, DDLWIDTH));
            res.Add(GetReqiuiredFieldValidator(DDLFEECATEGORYID, "*"));
            return res;
        }
        private ArrayList GetFeeTypeControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetDropDownListControl(DDLTYPEID, true, DDLCSS, DDLWIDTH));
            res.Add(GetReqiuiredFieldValidator(DDLTYPEID, "*"));
            return res;
        }
        private ArrayList GetFeeDescriptionControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetTextBox(TBOTHERTYPEFEEDESCRIPTIONID, TBCSS, TBWIDTH));
            res.Add(GetReqiuiredFieldValidator(TBOTHERTYPEFEEDESCRIPTIONID, "*"));
            return res;
        }
        private ArrayList GetProviderTypeControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetDropDownListControl(DDLPROVIDERTYPEID, true, DDLCSS, DDLWIDTH));
            return res;
        }
        private ArrayList GetProviderControl()
        {
            ArrayList res = new ArrayList();
            if (CheckForVendors())
            {
                DropDownList ddl = GetDropDownListControl(DDLPROVIDERID, false, DDLCSS, DDLWIDTH);
                res.Add(ddl);
                RequiredFieldValidator rf = GetReqiuiredFieldValidator(DDLPROVIDERID, "*");
                res.Add(rf);
            }
            else
            {
                TextBox tb = GetTextBox(TBPROVIDERID, "", 144);
                tb.MaxLength = 100;
                res.Add(tb);
                RequiredFieldValidator rf = GetReqiuiredFieldValidator(TBPROVIDERID, "*");
                res.Add(rf);
            }
            return res;
        }
        private ArrayList GetEndorsementsControl()
        {
            ArrayList res = new ArrayList();
            TextBox tb = GetTextBox(TBLISTENDORSEMENSID, "", 144);
            tb.MaxLength = 100;
            res.Add(tb);
            res.Add(GetReqiuiredFieldValidator(TBLISTENDORSEMENSID, "*"));
            return res;
        }
        //private ArrayList GetChargeToControl()
        //{
        //    ArrayList res = new ArrayList();
        //    res.Add(GetDropDownListControl(DDLCHARGETOID, false, DDLCSS, DDLWIDTH));
        //    res.Add(GetReqiuiredFieldValidator(DDLCHARGETOID, "*"));
        //    return res;
        //}
        private bool CheckProviderRow()
        {
            bool res = false;
//            if ((CurrentInvoice.FeeCategoryId == MortgageProfile.FEECATEGORYGOVERMENTCHARGE) && (mp.Property.CountyID != 0) && (CurrentInvoice.ProviderTypeId == Invoice.COUNTYRECORDERTYPEID))
            if ((CurrentInvoice.FeeCategoryId == MortgageProfile.FEECATEGORYGOVERMENTCHARGE) && (CurrentInvoice.ProviderTypeId == Invoice.COUNTYRECORDERTYPEID))
            {
                res = false;
            }
            else if (CurrentInvoice.FeeCategoryId == MortgageProfile.FEECATEGORYLENDERCHARGE)
            {
                if (IsProviderTypeSelected)
                {
                    res = !((CurrentInvoice.ProviderTypeId == Invoice.ORIGINATORPROVIDERTYPEID) || (CurrentInvoice.ProviderTypeId == Invoice.LENDERPROVIDERTYPEID) || (CurrentInvoice.ProviderTypeId == Invoice.PREFFEREDVENDORTYPEID));
                }
                else
                {
                    if (CurrentInvoice.ID < 1)
                    {
                        int providerTypeId = GetDefaultProviderType(CurrentInvoice.GetFeeTypes());
                        res = !((providerTypeId == Invoice.ORIGINATORPROVIDERTYPEID) || (providerTypeId == Invoice.LENDERPROVIDERTYPEID) || (providerTypeId == Invoice.PREFFEREDVENDORTYPEID));
                    }
                }
            }
            else
            {
                res = (CurrentInvoice.ProviderTypeId == Invoice.OTHERPROVIDERTYPEID) || (CurrentInvoice.ProviderTypeId == Invoice.VENDORPROVIDERTYPEID) || (CurrentInvoice.ProviderTypeId == Invoice.PREFFEREDVENDORTYPEID);
            }
            return res;
        }
        private ArrayList GetProviderOtherForFormulaFeeControl()
        {
            ArrayList res = new ArrayList();
            TextBox tb = GetTextBox(TBPROVIDERID, "", 144);
            tb.MaxLength = 100;
            res.Add(tb);
            RequiredFieldValidator rf = GetReqiuiredFieldValidator(TBPROVIDERID, "*");
            res.Add(rf);
            return res;
        }
        private void BuildLeftColumn()
        {
            LeftColumn = new ArrayList();
            InvoiceEdit.InvoiceEditRow categoryRow = new InvoiceEdit.InvoiceEditRow();
            categoryRow.Build(GetLabel("Fee category:", "lblCategory"), LABELTDCLASS, GetFeeCategoryControl(), CONTROLTDCLASS);
            LeftColumn.Add(categoryRow);
            InvoiceEdit.InvoiceEditRow feeRow = new InvoiceEdit.InvoiceEditRow();
            feeRow.Build(GetLabel("Type:", "lblType"), LABELTDCLASS, GetFeeTypeControl(), CONTROLTDCLASS);
            LeftColumn.Add(feeRow);
            if (Invoice.IsOtherFeeType(CurrentInvoice.TypeId))
            {
                InvoiceEdit.InvoiceEditRow descriptionFeeRow = new InvoiceEdit.InvoiceEditRow();
                descriptionFeeRow.Build(GetLabel("Description of fee:", "lblFeeDescription"), LABELTDCLASS, GetFeeDescriptionControl(), CONTROLTDCLASS);
                LeftColumn.Add(descriptionFeeRow);
            }
            if(CurrentInvoice.HasFormula&&CurrentInvoice.TypeId!=Invoice.TITLEINSURANCEFEETYPEID)
            {
                InvoiceEdit.InvoiceEditRow providerTypeRow = new InvoiceEdit.InvoiceEditRow();
                providerTypeRow.Build(GetLabel("Provider:", "lblProviderType"), LABELTDCLASS, GetProviderForFormulaFee(), CONTROLTDCLASS);
                LeftColumn.Add(providerTypeRow);
                if (CurrentInvoice.FormulaData.ContainsKey("OfficerTitleId"))
                {
                    int officerTitleId = int.Parse(CurrentInvoice.FormulaData["OfficerTitleId"].ToString());
                    if (officerTitleId == 0)
                    {
                        InvoiceEdit.InvoiceEditRow providerRow = new InvoiceEdit.InvoiceEditRow();
                        providerRow.Build(GetLabel("", LBLTITLENAMEID), LABELTDCLASS, GetProviderOtherForFormulaFeeControl(), CONTROLTDCLASS);
                        LeftColumn.Add(providerRow);
                    }
                }
            }
            else
            {
                InvoiceEdit.InvoiceEditRow providerTypeRow = new InvoiceEdit.InvoiceEditRow();
                providerTypeRow.Build(GetLabel("Provider type:", "lblProviderType"), LABELTDCLASS, GetProviderTypeControl(), CONTROLTDCLASS);
                LeftColumn.Add(providerTypeRow);
                if (CheckProviderRow())
                {
                    InvoiceEdit.InvoiceEditRow providerRow = new InvoiceEdit.InvoiceEditRow();
                    providerRow.Build(GetLabel("Provider:", LBLPROVIDERID), LABELTDCLASS, GetProviderControl(), CONTROLTDCLASS);
                    LeftColumn.Add(providerRow);
                }
                if (CurrentInvoice.TypeId == Invoice.TITLEENDORSMENTSFEETYPEID)
                {
                    InvoiceEdit.InvoiceEditRow endorsementRow = new InvoiceEdit.InvoiceEditRow();
                    endorsementRow.Build(GetLabel("List endorsements:", "lblEndorsemenst"), LABELTDCLASS, GetEndorsementsControl(), CONTROLTDCLASS);
                    LeftColumn.Add(endorsementRow);
                }
            }
            //if (mp.MortgageInfo.TransactionTypeId == TRANSACTIONPURCHASE)
            //{
            //    InvoiceEdit.InvoiceEditRow chargeToRow = new InvoiceEdit.InvoiceEditRow();
            //    chargeToRow.Build(GetLabel("Charge to:", "lblChargeTo"), LABELTDCLASS, GetChargeToControl(), CONTROLTDCLASS);
            //    LeftColumn.Add(chargeToRow);
            //}
        }
        #endregion

        #region right column methods
        private ArrayList GetLenderCreditAmountControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetNumericTextBox(TBLENDERCREDITAMOUNTID));
            return res;
        }
        private ArrayList GetThirdPartyPaidAmount()
        {
            ArrayList res = new ArrayList();
            res.Add(GetNumericTextBox(TBTHIRDPARTYPAIDAMOUNTID));
            return res;
        }
        private ArrayList GetAmountControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetNumericTextBox(TBAMOUNTID));
            res.Add(GetReqiuiredFieldValidator(TBAMOUNTID, "*"));
            res.Add(GetRangeValidator(TBAMOUNTID, "<=6000", 0, 6000));
            return res;
        }
        private ArrayList GetPocAmountControl()
        {
            string js = "ValidatePocAmount1";
            if (CurrentInvoice.FeeCategoryId == Invoice.GOVERNMENTCHARGECATEGORYID)
            {
                js = "ValidatePocAmount2";
            }
            ArrayList res = new ArrayList();
            res.Add(GetNumericTextBox(TBPOCAMOUNTID));
            res.Add(GetCustomValidator(TBPOCAMOUNTID, "*", js));
            return res;
        }
        private ArrayList GetDeedAmountControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetNumericTextBox(TBDEEDAMOUNTID));
            res.Add(GetReqiuiredFieldValidator(TBDEEDAMOUNTID, "*"));
            res.Add(GetRangeValidator(TBDEEDAMOUNTID, "<=6000", 0, 6000));
            return res;
        }
        private ArrayList GetMortgageAmountControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetNumericTextBox(TBMORTGAGEAMOUNTID));
            res.Add(GetRangeValidator(TBMORTGAGEAMOUNTID, "<=6000", 0, 6000));
            return res;
        }
        private ArrayList GetReleaseAmountControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetNumericTextBox(TBRELEASEAMOUNTID));
            res.Add(GetRangeValidator(TBRELEASEAMOUNTID, "<=6000", 0, 6000));
            return res;
        }
        private static ArrayList GetFinancedAmountValueControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetLabel("", LBLFINANCEDAMOUNTID));
            return res;
        }
        private ArrayList GetProviderForFormulaFee()
        {
            ArrayList res = new ArrayList();
            res.Add(GetDropDownListControl(DDLPROVIDERFORFORMULAFEEID, true, DDLCSS, DDLWIDTH));
            return res;
        }
        private void BuildRightColumn()
        {
            RightColumn = new ArrayList();
            //InvoiceEdit.InvoiceEditRow statusRow = new InvoiceEdit.InvoiceEditRow();
            //statusRow.Build(GetLabel("Status:", "lblStatus"), LABELTDCLASS, GetStatusControl(), CONTROLTDCLASS);
            //RightColumn.Add(statusRow);
            if (CurrentInvoice.HasFormula)
            {
                InvoiceEdit.InvoiceEditRow stateRow = new InvoiceEdit.InvoiceEditRow();
                stateRow.Build(GetLabel("Select state:", "lblState"), LABELTDCLASS, GetStateSelectControl(), CONTROLTDCLASS);
                RightColumn.Add(stateRow);

                InvoiceEdit.InvoiceEditRow formulaRow = new InvoiceEdit.InvoiceEditRow();
                formulaRow.Build(GetLabel("Choose formula:", "lblFormula"), LABELTDCLASS, GetFormulaSelectControl(), CONTROLTDCLASS);
                RightColumn.Add(formulaRow);
            }
            else
            {
                if (CurrentInvoice.FeeCategoryId != Invoice.GOVERNMENTCHARGECATEGORYID)
                {
                    InvoiceEdit.InvoiceEditRow amountRow = new InvoiceEdit.InvoiceEditRow();
                    amountRow.Build(GetLabel("Invoice Amount:", "lblAmount"), LABELTDCLASS, GetAmountControl(), CONTROLTDCLASS);
                    RightColumn.Add(amountRow);
                }
                else
                {
                    InvoiceEdit.InvoiceEditRow deedAmountRow = new InvoiceEdit.InvoiceEditRow();
                    deedAmountRow.Build(GetLabel("Deed Amount:", "lblDeedAmount"), LABELTDCLASS, GetDeedAmountControl(), CONTROLTDCLASS);
                    RightColumn.Add(deedAmountRow);
                    InvoiceEdit.InvoiceEditRow mortgageAmountRow = new InvoiceEdit.InvoiceEditRow();
                    mortgageAmountRow.Build(GetLabel("Mortgage Amount:", "lblMortgageAmount"), LABELTDCLASS, GetMortgageAmountControl(), CONTROLTDCLASS);
                    RightColumn.Add(mortgageAmountRow);
                    if (CurrentInvoice.TypeId == Invoice.RECORDINGFEETYPEID)
                    {
                        InvoiceEdit.InvoiceEditRow releaseAmountRow = new InvoiceEdit.InvoiceEditRow();
                        releaseAmountRow.Build(GetLabel("Release Amount:", "lblReleaseAmount"), LABELTDCLASS, GetReleaseAmountControl(), CONTROLTDCLASS);
                        RightColumn.Add(releaseAmountRow);
                    }
                }
                InvoiceEdit.InvoiceEditRow pocRow = new InvoiceEdit.InvoiceEditRow();
                pocRow.Build(GetLabel("POC Amount:", "lblPocAmount"), LABELTDCLASS, GetPocAmountControl(), CONTROLTDCLASS);
                RightColumn.Add(pocRow);
                InvoiceEdit.InvoiceEditRow lenderCreditRow = new InvoiceEdit.InvoiceEditRow();
                lenderCreditRow.Build(GetLabel("Lender Credit:", "lblLenderCreditAmount"), LABELTDCLASS, GetLenderCreditAmountControl(), CONTROLTDCLASS);
                RightColumn.Add(lenderCreditRow);
                InvoiceEdit.InvoiceEditRow thirdPartyRow = new InvoiceEdit.InvoiceEditRow();
                thirdPartyRow.Build(GetLabel("Third Party paid:", "lblThirdPartyPaidAmount"), LABELTDCLASS, GetThirdPartyPaidAmount(), CONTROLTDCLASS);
                RightColumn.Add(thirdPartyRow);
                InvoiceEdit.InvoiceEditRow financedAmountRow = new InvoiceEdit.InvoiceEditRow();
                financedAmountRow.Build(GetLabel("Financed Amount :", "lblFinancedAmount"), LABELTDCLASS, GetFinancedAmountValueControl(), CONTROLTDCLASS);
                RightColumn.Add(financedAmountRow);
            }
        }
        #endregion
        private static void AddEmptyTd(int count, IList column)
        {
            for (int i = 0; i < count; i++)
            {
                InvoiceEdit.InvoiceEditRow row = new InvoiceEdit.InvoiceEditRow();
                row.BuildEmpty();
                column.Add(row);
            }
        }
        private void BuildTable()
        {
            tabIndex = 100;
            BuildLeftColumn();
            BuildRightColumn();
            if (LeftColumn.Count > RightColumn.Count)
            {
                AddEmptyTd(LeftColumn.Count - RightColumn.Count, RightColumn);
            }
            else if (RightColumn.Count > LeftColumn.Count)
            {
                AddEmptyTd(RightColumn.Count - LeftColumn.Count, LeftColumn);
            }
            for (int i = 0; i < LeftColumn.Count; i++)
            {
                HtmlTableRow row = new HtmlTableRow();
                InvoiceEdit.InvoiceEditRow l = (InvoiceEdit.InvoiceEditRow)LeftColumn[i];
                row.Cells.Add(l.LabelCell);
                row.Cells.Add(l.ControlCell);
                InvoiceEdit.InvoiceEditRow r = (InvoiceEdit.InvoiceEditRow)RightColumn[i];
                row.Cells.Add(r.LabelCell);
                row.Cells.Add(r.ControlCell);
                Table1.Rows.Add(row);
            }
            if (CurrentInvoice.TypeId == MortgageProfile.TYPELENDERLOANORIGINATIONFEE)
            {
                HtmlTableRow rowFee = new HtmlTableRow();
                HtmlTableCell tdl = new HtmlTableCell();
                tdl.Align = "right";
                tdl.ColSpan = 3;
                tdl.Attributes.Add("class", LABELTDCLASS);
                tdl.InnerText = "Calculate the maximum allowable origination fee:";
                rowFee.Cells.Add(tdl);
                HtmlTableCell tdr = new HtmlTableCell();
                CheckBox cb = new CheckBox();
                cb.ID = CBCALCULATEDFEEID;
                cb.AutoPostBack = true;
                tdr.Controls.Add(cb);
                rowFee.Cells.Add(tdr);
                Table1.Rows.Add(rowFee);
            }
            if (CurrentInvoice.HasFormula)
            {
                BuildFormulaControl();
            }
            else
            {
                tblFormula.Visible = false;
            }
        }
        private void BuildFormulaControl()
        {
            feeFormulaControl = null;
            SetStateId();
            SetFormulaId();
            switch (CurrentInvoice.TypeId)
            {
                case Invoice.RECORDINGFEETYPEID:
                    feeFormulaControl = new RecordingFeeFormulaControl(CurrentInvoice);
                    break;
                case Invoice.CITYCOUNTYSTAMPSFEETYPEID:
                    feeFormulaControl = new CityCountyStampsFormulaControl(CurrentInvoice);
                    break;
                case Invoice.STATETAXSTAMPSFEETYPEID:
                    feeFormulaControl = new StateStampsFormulaControl(CurrentInvoice);
                    break;
                case Invoice.TITLEINSURANCEFEETYPEID:
                    feeFormulaControl = new TitleInsuranceFormulaControl(CurrentInvoice);
                    break;

            }
            if (feeFormulaControl != null)
            {
                feeFormulaControl.TabIndex = tabIndex;
                ArrayList rows = feeFormulaControl.Build();
                if (rows != null && rows.Count > 0)
                {
                    for (int i = 0; i < rows.Count; i++)
                    {
                        tblFormula.Rows.Add((HtmlTableRow)rows[i]);
                    }
                }
            }
        }
        private void SetStateId()
        {
            if(CurrentInvoice.StateId<=0)
            {
                DataView dv = DvStateList;
                CurrentInvoice.StateId = int.Parse(dv[0]["id"].ToString());
            }
        }
        private void SetFormulaId()
        {
            DataView dv = DvFormulaList;
            int formulaId = CurrentInvoice.FormulaId;
            if (formulaId <= 0)
            {
                formulaId = 1;
            }
            CurrentInvoice.FormulaId = formulaId;
        }
        private ArrayList GetFormulaSelectControl()
        {
            ArrayList res = new ArrayList();
            res.Add(new LiteralControl("&nbsp;"));
            res.Add(GetDropDownListControl(DDLFORMULAID, true, DDLCSS, DDLWIDTH));
            return res;
        }
        private ArrayList GetStateSelectControl()
        {
            ArrayList res = new ArrayList();
            res.Add(new LiteralControl("&nbsp;"));
            res.Add(GetDropDownListControl(DDLSTATEID, true, DDLCSS, DDLWIDTH));
            return res;
        }
        #endregion
    }
}
