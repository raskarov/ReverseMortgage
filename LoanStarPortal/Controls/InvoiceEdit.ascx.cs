using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using LoanStar.Common;
using Telerik.WebControls; 

namespace LoanStarPortal.Controls
{
    public partial class InvoiceEdit : EditGridFormControl
    {
        public class InvoiceEditRow
        {
            HtmlTableCell tdLabel;
            HtmlTableCell tdControl;
            public HtmlTableCell LabelCell
            {
                get { return tdLabel; }
            }
            public HtmlTableCell ControlCell
            {
                get { return tdControl; }
            }

            public void Build(Control labelControl, string labelClass, ArrayList controls, string controlClass)
            {
                tdLabel = new HtmlTableCell();
                tdLabel.Controls.Add(labelControl);
                if (!String.IsNullOrEmpty(labelClass))
                {
                    tdLabel.Attributes.Add("class", labelClass);
                }                
                tdControl = new HtmlTableCell();
                for(int i=0;i<controls.Count;i++)
                {
                    tdControl.Controls.Add((Control)controls[i]);
                }
                if (!String.IsNullOrEmpty(controlClass))
                {
                    tdControl.Attributes.Add("class", controlClass);
                }                
            }
            public void BuildEmpty()
            {
                tdLabel = new HtmlTableCell();
                tdControl = new HtmlTableCell();
            }
        }

        #region constants
        private const int TRANSACTIONPURCHASE = 2;
        private const string ONCLICK = "onclick";
        private const string JSCLICKAUTOCALCULATE = "var r = ChechAutoCalculate(this,'{0}'); if(!r) return false;";
        public const string INVOICE = "currentinvoice";
        private const string ORIGINATORLIST = "{0} is the provider";
        private const string FOCUSLOSTGOVERNMENTJS = "AmountGovernmentFocusLost(this,{0});";
        private const string FOCUSLOSTJS = "AmountFocusLost(this);";
        protected const string ONBLUR = "onblur";
        private const string LABELTDCLASS = "editFormLabel";
        private const string CONTROLTDCLASS = "";
        private const string DDLCSS = "ddl";
        private const int DDLWIDTH = 150;
        private const int BTNWIDTH = 100;
        private const string TBCSS = "ddl";
        private const int TBWIDTH = 145;
        private const int RADTBWIDTH = 150;
        private const string JSSHOWORDER = "ShowOrder('{0}');";
        private const string JSSHOWORDERMESSAGE = "OrderMessage('{0}');";

        #region controls' id
        private const string DDLFEECATEGORYID = "ddlFeeCategory";
        private const string DDLTYPEID = "ddlType";
        private const string TBOTHERTYPEFEEDESCRIPTIONID = "tbOtherDescription";
        private const string DDLPROVIDERID  = "ddlProvider";
        private const string DDLFORMULAID = "ddlFormula";
        private const string DDLMAXCLAIMAMOUNTID = "MaxSelect";
        private const string CBUSETOTALSID = "cbUseTotals";
        private const string DDLCHARGETOID = "ddlChargeTo";
        private const string TBPROVIDERID = "tbProvider";
        private const string LBLPROVIDERID = "lblProvider";
        private const string LBLTITLENAMEID = "lblTitleName";
        private const string DDLPROVIDERTYPEID = "ddlProviderType";
        private const string DDLPROVIDERFORFORMULAFEEID = "ddlProviderForFormulaFee";
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
        private const string BTNVIEWORDERID = "btnViewOrder";
        private const string BTNSENDORDERID = "btnSendOrder";
        #endregion

        #endregion

        #region fields
        ArrayList LeftColumn;
        ArrayList RightColumn;
        private Invoice invoice;
        private string objectName;
        private bool enableValidation;
        private DataView dvCategoryList;
        private DataView dvFormulaList;
        private DataView dvOfficerTitleList;
        private DataView dvStatusList;
        private DataView dvVendorFeeOrder;
        private DataView dvChargeToList;
        private MortgageProfile mp;
        private DataView dvProviderType;
        private int defaultProviderTypeId = Invoice.VENDORPROVIDERTYPEID;
        private bool isFeeTypeSelected = false;        
        private short tabIndex = 0;
        private FeeFormulaControl feeFormulaControl;
        #endregion

        #region properties
        private bool IsProviderTypeSelected
        {
            get
            {
                bool res;
                Object o = Session["providertypeselected"];
                if (o != null)
                {
                    res = bool.Parse(o.ToString());
                }
                else
                {
                    res = CurrentInvoice.ProviderTypeId > 0;
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
            set { invoice = value as Invoice; }
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
        protected DataView DvFormulaList
        {
            get
            {
                if (dvFormulaList == null)
                {
                    dvFormulaList = Invoice.GetFormulaList(CurrentInvoice.TypeID);
                }
                return dvFormulaList;
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
        protected DataView DvVendorFeeOrder
        {
            get
            {
                if(dvVendorFeeOrder == null)
                {
                    dvVendorFeeOrder = VendorGlobal.GetVendorFeeOrderList();
                }
                return dvVendorFeeOrder;
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
        protected Invoice CurrentInvoice
        {
            get
            {
                Invoice res = Session[INVOICE] as Invoice;
                if(res==null)
                {
                    res = invoice;
                    Session[INVOICE] = res;
                }
                return res;
            }
            set { Session[INVOICE] = value; }
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
                ddl.SelectedValue = CurrentInvoice.FeeCategoryID.ToString();
                ddl.Enabled = IsFieldEditable("FeeCategoryID");
                if (CurrentInvoice.ID <= 0)
                {
                    AddEmptyItem(ddl);
                }
            }
        }
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
//                ddl.Items.Insert(0,new ListItem("Total cost",1.ToString()));
                int formulaId = CurrentInvoice.FormulaId;
                ListItem li = ddl.Items.FindByValue(formulaId.ToString());
                if (li != null)
                {
                    li.Selected = true;
                }
                feeFormulaControl.BindData(this);
            }
            DropDownList ddmc = (DropDownList)FindControl(DDLMAXCLAIMAMOUNTID);
            if(ddmc != null)
            {
                if(mp.ProductID<1)
                {
                    ListItem li = ddmc.Items.FindByValue(Invoice.PRINCIPLELIMITID.ToString());
                    if(li!=null)
                    {
                        if(li.Selected)
                        {
                            ddmc.Items[0].Selected = true;
                        }
                        ddmc.Items.Remove(li);
                    }
                }
            }
            DropDownList ddp = (DropDownList)FindControl(DDLPROVIDERFORFORMULAFEEID);
            if(ddp!=null)
            {
                DataView dv = DvOfficerTitleList;
                dv.RowFilter = String.Format("StateId={0}",  mp.Property.StateId);
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
                    if (titleId==0)
                    {
                        TextBox tb = (TextBox) FindControl(TBPROVIDERID);
                        if(tb!=null)
                        {
                            tb.Text = CurrentInvoice.OtherProviderName;
                        }
                    }
                }

            }
        }
        private void BindFeeType()
        {
            DropDownList ddl = (DropDownList)FindControl(DDLTYPEID);
            if (ddl != null)
            {
                if (CurrentInvoice.FeeCategoryID > 0)
                {
                    DataView dv = Invoice.GetInvoiceTypeList(CurrentInvoice.FeeCategoryID, mp.ID, CurrentInvoice.ID);
                    ddl.DataSource = dv;
                    ddl.DataTextField = "name";
                    ddl.DataValueField = "id";
                    ddl.DataBind();
                    ddl.ClearSelection();
                    ListItem li = ddl.Items.FindByValue(CurrentInvoice.TypeID.ToString());
                    if (li != null) li.Selected = true;
                    defaultProviderTypeId = GetDefaultProviderType(dv);
                }
                AddEmptyItem(ddl);
                ddl.Enabled = IsFieldEditable("TypeID") && (CurrentInvoice.FeeCategoryID > 0);
                isFeeTypeSelected = ddl.Enabled && (CurrentInvoice.TypeID>0);
            }
        }
        private void SetDescription()
        {
            if (Invoice.IsOtherFeeType(CurrentInvoice.TypeID))
            {
                TextBox tb = (TextBox)FindControl(TBOTHERTYPEFEEDESCRIPTIONID);
                if (tb != null)
                {
                    tb.Text = invoice.Description;
                    tb.ReadOnly = !IsFieldEditable("Description");
                }               
            }
        }
        private int GetDefaultProviderType(DataView dv)
        {
            int res = Invoice.VENDORPROVIDERTYPEID;
            dv.RowFilter = String.Format("id={0}", CurrentInvoice.TypeID);
            if (dv.Count == 1)
            {
                res = int.Parse(dv[0]["DefaultProviderTypeId"].ToString());
            }
            return res;
        }
        private void BindChargeTo()
        {
            if (mp.MortgageInfo.TransactionTypeId == TRANSACTIONPURCHASE)
            {
                DropDownList ddl = (DropDownList)FindControl(DDLCHARGETOID);
                if (ddl != null)
                {
                    ddl.DataSource = DvChargeToList;
                    ddl.DataTextField = "name";
                    ddl.DataValueField = "id";
                    ddl.DataBind();
                    ddl.SelectedValue = CurrentInvoice.ChargeToId.ToString();
                    ddl.Enabled = IsFieldEditable("ChargeToId");
                }
            }
        }
        private void BindProvider()
        {
            DropDownList ddl = (DropDownList)FindControl(DDLPROVIDERTYPEID);
            if (ddl != null)
            {
                ddl.Items.Clear();
                if (CurrentInvoice.TypeID > 0)
                {
                    int vendorsCount = 0;
                    if (!Invoice.IsTypeOther(CurrentInvoice.TypeID))
                    {
                        DataView dv = VendorGlobal.GetVendorsForInvice(CurrentUser.CompanyId, CurrentInvoice.TypeID, mp.ID);
                        dv.RowFilter = "(isdeleted=0 and isdisabled=0) or isinuse=1";
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
                                ddlv.Enabled = isFeeTypeSelected && IsFieldEditable("Provider");
                            }
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
                    if (CurrentInvoice.FeeCategoryID == MortgageProfile.FEECATEGORYLENDERCHARGE)
                    {
                        AddProviderType(ddl, Invoice.ORIGINATORPROVIDERTYPEID, String.Format(ORIGINATORLIST, CurrentUser.CompanyName));
                        Company lender = mp.MortgageInfo.LenderAffiliate;
                        if (lender != null)
                        {
                            AddProviderType(ddl, Invoice.LENDERPROVIDERTYPEID, String.Format(ORIGINATORLIST, lender.Name));
                        }
                    }
                    if ((mp.Property.CountyID != 0) && (CurrentInvoice.FeeCategoryID == MortgageProfile.FEECATEGORYGOVERMENTCHARGE))
                    {
                        AddProviderType(ddl, Invoice.COUNTYRECORDERTYPEID, String.Format("{0} county recorder", mp.Property.County));
                        defaultProviderTypeId = Invoice.COUNTYRECORDERTYPEID;
                    }
                }
                else if ((CurrentInvoice.FeeCategoryID == MortgageProfile.FEECATEGORYGOVERMENTCHARGE) && (mp.Property.CountyID != 0))
                {
                    AddProviderType(ddl, Invoice.COUNTYRECORDERTYPEID, String.Format("{0} county recorder", mp.Property.County));
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
            if (CurrentInvoice.ProviderTypeId == Invoice.OTHERPROVIDERTYPEID)
            {
                TextBox tb = (TextBox)FindControl(TBPROVIDERID);
                if (tb != null)
                {
                    tb.Text = CurrentInvoice.OtherProviderName;
                    tb.Enabled = isFeeTypeSelected && IsFieldEditable("OtherProviderName");
                    tb.MaxLength = 100;
                }
            }
            if (!IsProviderTypeSelected)
            {
                CurrentInvoice.ProviderTypeId = CurrentInvoice.ID > 0 ? CurrentInvoice.ProviderTypeId : defaultProviderTypeId;
            }
            if (ddl != null) ddl.Enabled = isFeeTypeSelected && IsFieldEditable("ProviderTypeId");
        }
        private bool CheckForVendors()
        {
            bool res = false;
            if (CurrentInvoice.TypeID > 0)
            {
                if (!Invoice.IsTypeOther(CurrentInvoice.TypeID))
                {
                    DataView dv = VendorGlobal.GetVendorsForInvice(CurrentUser.CompanyId, CurrentInvoice.TypeID, mp.ID);
                    dv.RowFilter = "(isdeleted=0 and isdisabled=0) or isinuse=1";
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
            if (CurrentInvoice.TypeID == Invoice.TITLEENDORSMENTSFEETYPEID)
            {
                TextBox tb = (TextBox)FindControl(TBLISTENDORSEMENSID);
                if (tb != null)
                {
                    tb.Text = CurrentInvoice.Listendorsements;
                    tb.Enabled = IsFieldEditable("Listendorsements");
                }
            }
        }
        private void SetAmountField(string tbid, decimal amount,string fieldName, string js, bool canEdit)
        {
            RadNumericTextBox tb = (RadNumericTextBox)FindControl(tbid);
            if (tb != null)
            {
                tb.Text = amount.ToString();
                tb.Enabled = IsFieldEditable(fieldName);
                if (!String.IsNullOrEmpty(js))
                {
                    tb.Attributes.Add(ONBLUR, js);
                    tb.Enabled = canEdit;
                }
            }
        }
        private void BindAmounts()
        {
            string js = FOCUSLOSTJS;
            if (CurrentInvoice.FeeCategoryID == Invoice.GOVERNMENTCHARGECATEGORYID)
            {
                js = String.Format(FOCUSLOSTGOVERNMENTJS, CurrentInvoice.TypeID == Invoice.RECORDINGFEETYPEID ? 1 : 0);
                SetAmountField(TBDEEDAMOUNTID, CurrentInvoice.DeedAmount, "DeedAmount", js, IsFieldEditable("DeedAmount"));
                SetAmountField(TBMORTGAGEAMOUNTID, CurrentInvoice.MortgageAmount, "MortgageAmount", js, IsFieldEditable("MortgageAmount"));
                if (CurrentInvoice.TypeID == Invoice.RECORDINGFEETYPEID)
                {
                    SetAmountField(TBRELEASEAMOUNTID, CurrentInvoice.ReleaseAmount, "ReleaseAmount", js, IsFieldEditable("ReleaseAmount"));
                }
            }
            else
            {
                SetAmountField(TBAMOUNTID, CurrentInvoice.Amount, "Amount", js, IsFieldEditable("Amount"));
            }
            SetAmountField(TBPOCAMOUNTID, CurrentInvoice.POCAmount, "POCAmount", js, IsFieldEditable("POCAmount"));
            SetAmountField(TBLENDERCREDITAMOUNTID, CurrentInvoice.LenderCreditAmount, "LenderCreditAmount", js, IsFieldEditable("LenderCreditAmount"));
            SetAmountField(TBTHIRDPARTYPAIDAMOUNTID, CurrentInvoice.ThirdPartyPaidAmount, "ThirdPartyPaidAmount", js, IsFieldEditable("ThirdPartyPaidAmount"));
            Label lbl = (Label)FindControl(LBLFINANCEDAMOUNTID);
            if (lbl != null)
            { 
                lbl.Text = CurrentInvoice.AmountFinanced.ToString("C");
            }
        }
        private void SetCalculatedFeeCheckBox()
        {
            if (CurrentInvoice.TypeID == MortgageProfile.TYPELENDERLOANORIGINATIONFEE)
            {
                CheckBox cb = (CheckBox)FindControl(CBCALCULATEDFEEID);
                if (cb != null)
                {
                    if (mp.ProductCalcType == ProductFlag.None)
                    {
                        cb.Checked = false;
                        cb.Attributes.Add(ONCLICK, String.Format(JSCLICKAUTOCALCULATE, "To use this checkbox please select product."));
                    }
                    else 
                    {
                        if (CurrentInvoice.CalculateFee)
                        {
                            cb.Checked = true;
                            decimal cc = mp.MortgageInfo.LenderFee;
                            CurrentInvoice.Amount = cc;
                            RadNumericTextBox tb = (RadNumericTextBox)FindControl(TBAMOUNTID);
                            if (tb != null)
                            {
                                tb.Text = cc.ToString();
                                tb.Enabled = false;
                            }
                        }
                    }
                    Label lbl = (Label)FindControl(LBLFINANCEDAMOUNTID);
                    if (lbl != null)
                    {
                        lbl.Text = CurrentInvoice.AmountFinanced.ToString("C");
                    }
                }
            }
        }
        public override void BindData()
        {
            mp = CurrentPage.GetMortgage(MortgageProfileID);
            CurrentInvoice.mp = mp;
            ProcessPostBack();
            BuildTable();
            BindFeeCategory();
            BindFeeType();
            SetDescription();
//            BindStatus();
            BindProvider();
            BindEndorsments();
            BindChargeTo();
            BindAmounts();
            if(CurrentInvoice.HasFormula)
            {
                BindFormula();
            }
            btnSave.Attributes.Add("onclick","var r=CheckAmountFinanced(); if(!r) return false;");
        }
        #endregion

        private void GetAmounts()
        {
            for (int i = 0; i < Page.Request.Form.AllKeys.Length; i++)
            {
                string key = Page.Request.Form.AllKeys[i];
                if(key.EndsWith("$EditFormGridControl$"+TBAMOUNTID))
                {
                    try
                    {
                        CurrentInvoice.Amount = decimal.Parse(Page.Request.Form[key]);
                    }
                    catch { }
                }
                if(key.EndsWith("$EditFormGridControl$"+TBPOCAMOUNTID))
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
            if(!String.IsNullOrEmpty(control))
            {
                if (control.EndsWith(":" + DDLFEECATEGORYID))
                {
                    CurrentInvoice.FeeCategoryID = GetPostBackValue(control);
                    CurrentInvoice.TypeID = 0;
                    IsProviderTypeSelected = false;
                }
                else if (control.EndsWith(":"+DDLTYPEID))
                {
                    CurrentInvoice.TypeID = GetPostBackValue(control);
                    IsProviderTypeSelected = false;
                }
                else if (control.EndsWith(":" + DDLPROVIDERTYPEID))
                {
                    CurrentInvoice.ProviderTypeId = GetPostBackValue(control);
                    IsProviderTypeSelected = true;
                }
                else if (control.EndsWith(":"+CBCALCULATEDFEEID))
                {
                    CurrentInvoice.CalculateFee = GetCheckBoxPostBackValue(control);
                }
                else if (control.EndsWith(":" + DDLFORMULAID))
                {
                    CurrentInvoice.SetFormulaData("FormulaId", GetPostBackValue(control).ToString());
                }
                else if (control.EndsWith(":" + DDLPROVIDERFORFORMULAFEEID))
                {
                    CurrentInvoice.SetFormulaData("OfficerTitleId", GetPostBackValue(control).ToString());
                }
                GetAmounts();
            }
        }
        private bool GetCheckBoxPostBackValue(string control)
        {
            bool res = false;
            string s = Page.Request.Form[control.Replace(":", "$")];
            if(!String.IsNullOrEmpty(s))
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
                catch{}                
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
        protected void btnViewOrder_Click(object sender, EventArgs e)
        {
            VendorFeeOrder order = new VendorFeeOrder(CurrentInvoice.ProviderId,CurrentInvoice.TypeID,CurrentUser,Server.MapPath(Constants.STORAGEFOLDER),mp);
            string fileName;
            string script;
            if(order.View(out fileName))
            {
                string path = Path.Combine(Constants.STORAGEFOLDER.Replace("~", ""), VendorFeeOrder.VENDORORDERFOLDER);
                path = Path.Combine(path, fileName);
                script = String.Format(JSSHOWORDER, path.Replace("\\", "/"));
            }
            else
            {
                script = String.Format(JSSHOWORDERMESSAGE,order.ErrorMessage);
            }
            CurrentPage.ClientScript.RegisterStartupScript(GetType(), "ViewOrder", "<script language=\"javascript\" type=\"text/javascript\">"+script+";</script>");
        }
        protected void btnSendOrder_Click(object sender, EventArgs e)
        {
            VendorFeeOrder order = new VendorFeeOrder(CurrentInvoice.ProviderId, CurrentInvoice.TypeID, CurrentUser, Server.MapPath(Constants.STORAGEFOLDER), mp);
            string script = "";
            string url = CurrentPage.Request.Url.AbsoluteUri.Replace(CurrentPage.Request.Url.AbsolutePath,"/");
            if (order.Send(url))
            {
                script = String.Format(JSSHOWORDERMESSAGE, "Order sent to vendor");
            }
            else
            {
                script = String.Format(JSSHOWORDERMESSAGE, order.ErrorMessage);
            }
            CurrentPage.ClientScript.RegisterStartupScript(GetType(), "SendOrder", "<script language=\"javascript\" type=\"text/javascript\">" + script + ";</script>");            
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ArrayList logs = new ArrayList();
//            CurrentInvoice.StatusID = GetDdlValue(DDLSTATUSID);
            CurrentInvoice.ProviderTypeId = GetDdlValue(DDLPROVIDERTYPEID);
            invoice.FormulaId = GetDdlValue(DDLFORMULAID);
            invoice.mp = CurrentInvoice.mp;
            if (CurrentInvoice.ProviderTypeId == Invoice.VENDORPROVIDERTYPEID)
            {
                CurrentInvoice.ProviderId = GetDdlValue(DDLPROVIDERID); ;
            }
            if (invoice.TypeID != CurrentInvoice.TypeID)
            {
                logs.Add(
                    new MortgageLogEntry(ObjectName, invoice.ID, ObjectName + ".TypeID", invoice.TypeID.ToString(),
                                         CurrentInvoice.TypeID.ToString(), CurrentPage.CurrentUser.Id));
                invoice.TypeID = CurrentInvoice.TypeID;
            }
            if (invoice.ProviderId != CurrentInvoice.ProviderId)
            {
                logs.Add(
                    new MortgageLogEntry(ObjectName, invoice.ID, ObjectName + ".Provider", invoice.ProviderId.ToString(),
                                         CurrentInvoice.ProviderId.ToString(), CurrentPage.CurrentUser.Id));
                invoice.ProviderId = CurrentInvoice.ProviderId;
            }
            if((!CurrentInvoice.HasFormula)||(CurrentInvoice.TypeID==Invoice.TITLEINSURANCEFEETYPEID))
            {
                if (invoice.ProviderTypeId != CurrentInvoice.ProviderTypeId)
                {
                    logs.Add(
                        new MortgageLogEntry(ObjectName, invoice.ID, ObjectName + ".Provider", invoice.ProviderTypeId.ToString(),
                                             CurrentInvoice.ProviderTypeId.ToString(), CurrentPage.CurrentUser.Id));
                    invoice.ProviderTypeId = CurrentInvoice.ProviderTypeId;
                }
            }
            if (invoice.StatusID != CurrentInvoice.StatusID)
            {
                logs.Add(
                    new MortgageLogEntry(ObjectName, invoice.ID, ObjectName + ".StatusId",
                                         invoice.StatusID.ToString(), CurrentInvoice.StatusID.ToString(),
                                         CurrentPage.CurrentUser.Id));
                invoice.StatusID = CurrentInvoice.StatusID;
            }
            if(CurrentInvoice.HasFormula)
            {
                feeFormulaControl.SetInvoiceData(invoice, this, logs, CurrentPage.CurrentUser.Id);
                if(CurrentInvoice.TypeID!=Invoice.TITLEINSURANCEFEETYPEID)
                {
                    int providerTypeId = GetDdlValue(DDLPROVIDERFORFORMULAFEEID);
                    invoice.SetFormulaData("OfficerTitleId", providerTypeId.ToString());
                    string officerTitle = "";
                    DropDownList ddl = (DropDownList)FindControl(DDLPROVIDERFORFORMULAFEEID);
                    if (ddl != null)
                    {
                        officerTitle = ddl.SelectedItem.Text;
                    }
                    invoice.SetFormulaData("OfficerTitle", officerTitle);
                    CurrentInvoice.ProviderTypeId = Invoice.COUNTYRECORDERTYPEID;
                    invoice.ProviderTypeId = CurrentInvoice.ProviderTypeId;
                    CurrentInvoice.OtherProviderName = GetTextBoxValue(TBPROVIDERID);
                    if (invoice.OtherProviderName != CurrentInvoice.OtherProviderName)
                    {
                        logs.Add(
                            new MortgageLogEntry(ObjectName, invoice.ID, ObjectName + ".OtherProviderName",
                                                 invoice.OtherProviderName,
                                                 CurrentInvoice.OtherProviderName, CurrentPage.CurrentUser.Id));
                        invoice.OtherProviderName = CurrentInvoice.OtherProviderName;
                    }
                }
            }
            else
            {
                decimal amount;
                if (CurrentInvoice.FeeCategoryID == Invoice.GOVERNMENTCHARGECATEGORYID)
                {
                    amount = GetRadTextBoxValue(TBDEEDAMOUNTID);
                    if (invoice.DeedAmount != amount)
                    {
                        logs.Add(
                            new MortgageLogEntry(ObjectName, invoice.ID, ObjectName + ".DeedAmount",
                                                 invoice.DeedAmount.ToString(),
                                                 amount.ToString(), CurrentPage.CurrentUser.Id));
                        invoice.DeedAmount = amount;
                    }
                    amount = GetRadTextBoxValue(TBMORTGAGEAMOUNTID);
                    if (invoice.MortgageAmount != amount)
                    {
                        logs.Add(
                            new MortgageLogEntry(ObjectName, invoice.ID, ObjectName + ".MortgageAmount",
                                                 invoice.MortgageAmount.ToString(),
                                                 amount.ToString(), CurrentPage.CurrentUser.Id));
                        invoice.MortgageAmount = amount;
                    }
                    if (CurrentInvoice.TypeID == Invoice.RECORDINGFEETYPEID)
                    {
                        amount = GetRadTextBoxValue(TBRELEASEAMOUNTID);
                        if (invoice.ReleaseAmount != amount)
                        {
                            logs.Add(
                                new MortgageLogEntry(ObjectName, invoice.ID, ObjectName + ".ReleaseAmount",
                                                     invoice.ReleaseAmount.ToString(),
                                                     amount.ToString(), CurrentPage.CurrentUser.Id));
                            invoice.ReleaseAmount = amount;
                        }
                    }
                }
                else
                {
                    amount = GetRadTextBoxValue(TBAMOUNTID);
                    if (invoice.Amount != amount)
                    {
                        logs.Add(
                            new MortgageLogEntry(ObjectName, invoice.ID, ObjectName + ".Amount",
                                                 invoice.Amount.ToString(),
                                                 amount.ToString(), CurrentPage.CurrentUser.Id));
                        invoice.Amount = amount;
                    }
                }
                amount = GetRadTextBoxValue(TBPOCAMOUNTID);
                if (invoice.POCAmount != amount)
                {
                    logs.Add(
                        new MortgageLogEntry(ObjectName, invoice.ID, ObjectName + ".POCAmount",
                                             invoice.POCAmount.ToString(),
                                             amount.ToString(), CurrentPage.CurrentUser.Id));
                    invoice.POCAmount = amount;
                }
                amount = GetRadTextBoxValue(TBLENDERCREDITAMOUNTID);
                if (invoice.LenderCreditAmount != amount)
                {
                    logs.Add(
                        new MortgageLogEntry(ObjectName, invoice.ID, ObjectName + ".LenderCreditAmount",
                                             invoice.LenderCreditAmount.ToString(),
                                             amount.ToString(), CurrentPage.CurrentUser.Id));
                    invoice.LenderCreditAmount = amount;
                }
                amount = GetRadTextBoxValue(TBTHIRDPARTYPAIDAMOUNTID);
                if (invoice.ThirdPartyPaidAmount != amount)
                {
                    logs.Add(
                        new MortgageLogEntry(ObjectName, invoice.ID, ObjectName + ".ThirdPartyPaidAmount",
                                             invoice.ThirdPartyPaidAmount.ToString(),
                                             amount.ToString(), CurrentPage.CurrentUser.Id));
                    invoice.ThirdPartyPaidAmount = amount;
                }
            }
            if (CurrentInvoice.TypeID == Invoice.TITLEENDORSMENTSFEETYPEID)
            {
                CurrentInvoice.Listendorsements = GetTextBoxValue(TBLISTENDORSEMENSID);
                if (invoice.Listendorsements != CurrentInvoice.Listendorsements)
                {
                    logs.Add(
                        new MortgageLogEntry(ObjectName, invoice.ID, ObjectName + ".Listendorsements",
                                             invoice.Listendorsements, CurrentInvoice.Listendorsements,
                                             CurrentPage.CurrentUser.Id));
                    invoice.Listendorsements = CurrentInvoice.Listendorsements;
                }
            }
            if (Invoice.IsOtherFeeType(CurrentInvoice.TypeID))
            {
                CurrentInvoice.Description = GetTextBoxValue(TBOTHERTYPEFEEDESCRIPTIONID);
                if (invoice.Description != CurrentInvoice.Description)
                {
                    logs.Add(
                        new MortgageLogEntry(ObjectName, invoice.ID, ObjectName + ".Description",
                                             invoice.Description,
                                             CurrentInvoice.Description, CurrentPage.CurrentUser.Id));
                    invoice.Description = CurrentInvoice.Description;
                }
            }
            if (CurrentInvoice.ProviderTypeId == Invoice.OTHERPROVIDERTYPEID)
            {
                CurrentInvoice.OtherProviderName = GetTextBoxValue(TBPROVIDERID);
                if (invoice.OtherProviderName != CurrentInvoice.OtherProviderName)
                {
                    logs.Add(
                        new MortgageLogEntry(ObjectName, invoice.ID, ObjectName + ".OtherProviderName",
                                             invoice.OtherProviderName,
                                             CurrentInvoice.OtherProviderName, CurrentPage.CurrentUser.Id));
                    invoice.OtherProviderName = CurrentInvoice.OtherProviderName;
                }
            }
            if (mp.MortgageInfo.TransactionTypeId == TRANSACTIONPURCHASE)
            {
                CurrentInvoice.ChargeToId = GetDdlValue(DDLCHARGETOID);
                if (invoice.ChargeToId != CurrentInvoice.ChargeToId)
                {
                    logs.Add(
                        new MortgageLogEntry(ObjectName, invoice.ID, ObjectName + ".ChargeToId",
                                             invoice.ChargeToId.ToString(), CurrentInvoice.ChargeToId.ToString(),
                                             CurrentPage.CurrentUser.Id));
                    invoice.ChargeToId = CurrentInvoice.ChargeToId;
                }
            }
            if (CurrentInvoice.TypeID == MortgageProfile.TYPELENDERLOANORIGINATIONFEE)
            {
                invoice.CalculateFee = CurrentInvoice.CalculateFee;
            }
            Save(invoice, logs);
            CurrentInvoice = null;
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
/*
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
        private CompareValidator GetCompareValidator(string controlId, string controlToCompareId, string errMessage)
        {
            CompareValidator v = new CompareValidator();
            v.ID = "cv" + controlId;
            v.ControlToValidate = controlId;
            v.ControlToCompare = controlToCompareId;
            v.Type = ValidationDataType.Double;
            v.ErrorMessage = errMessage;
            return v;
        }
*/
        private static Button GetButtonControl(string id, string text, int width, string cmdName, EventHandler handler)
        {
            Button btn = new Button();
            btn.CausesValidation = false;
            btn.ID = id;
            btn.Text = text;
            btn.CommandName = cmdName;
            btn.Width = Unit.Pixel(width);
            if (handler != null) btn.Click += handler;
            return btn;
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
        private CheckBox GetCheckBoxControl(string id, bool isPostBack, string css)
        {
            CheckBox cb = new CheckBox();
            cb.ID = id;
            cb.TabIndex = tabIndex;
            tabIndex++;
            cb.AutoPostBack = isPostBack;
            if (!String.IsNullOrEmpty(css))
            {
                cb.CssClass = css;
            }
            return cb;
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
        private ArrayList GetProviderForFormulaFee()
        {
            ArrayList res = new ArrayList();
            res.Add(GetDropDownListControl(DDLPROVIDERFORFORMULAFEEID, true, DDLCSS, DDLWIDTH));
            return res;
        }

        private ArrayList GetProviderTypeControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetDropDownListControl(DDLPROVIDERTYPEID, true, DDLCSS, DDLWIDTH));
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
        private ArrayList GetChargeToControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetDropDownListControl(DDLCHARGETOID, false, DDLCSS, DDLWIDTH));
            res.Add(GetReqiuiredFieldValidator(DDLCHARGETOID, "*"));
            return res;
        }
        private ArrayList GetButtonViewControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetButtonControl(BTNVIEWORDERID, "View order", BTNWIDTH, LoanStarPortal.Controls.Invoices.VIEWORDERCOMMANDNAME, btnViewOrder_Click));
            return res;
        }
        private ArrayList GetButtonSendControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetButtonControl(BTNSENDORDERID, "Place order", BTNWIDTH, LoanStarPortal.Controls.Invoices.SENDORDERCOMMANDNAME, btnSendOrder_Click));
            return res;
        }

        private bool CheckProviderRow()
        {
            bool res = false;
            if ((CurrentInvoice.FeeCategoryID == MortgageProfile.FEECATEGORYGOVERMENTCHARGE) && (mp.Property.CountyID != 0)&&(CurrentInvoice.ProviderTypeId==Invoice.COUNTYRECORDERTYPEID))
            {
                res = false;
            }
            else if (CurrentInvoice.FeeCategoryID == MortgageProfile.FEECATEGORYLENDERCHARGE)
            {
                if (IsProviderTypeSelected)
                {
                    res = !((CurrentInvoice.ProviderTypeId == Invoice.ORIGINATORPROVIDERTYPEID) || (CurrentInvoice.ProviderTypeId == Invoice.LENDERPROVIDERTYPEID));
                }
                else
                {
                    if (CurrentInvoice.ID < 1)
                    {
                        int providerTypeId = GetDefaultProviderType(Invoice.GetInvoiceTypeList(CurrentInvoice.FeeCategoryID, mp.ID, CurrentInvoice.ID));
                        res = !((providerTypeId == Invoice.ORIGINATORPROVIDERTYPEID) || (providerTypeId == Invoice.LENDERPROVIDERTYPEID));
                    }
                }                
            }
            else
            {
                res = (CurrentInvoice.ProviderTypeId == Invoice.OTHERPROVIDERTYPEID) || (CurrentInvoice.ProviderTypeId == Invoice.VENDORPROVIDERTYPEID);
            }            
            return res;
        }
        private void BuildLeftColumn()
        {
            LeftColumn = new ArrayList();
            InvoiceEditRow categoryRow = new InvoiceEditRow();
            categoryRow.Build(GetLabel("Fee category:", "lblCategory"), LABELTDCLASS, GetFeeCategoryControl(), CONTROLTDCLASS);
            LeftColumn.Add(categoryRow);
            InvoiceEditRow feeRow = new InvoiceEditRow();
            feeRow.Build(GetLabel("Type:", "lblType"), LABELTDCLASS, GetFeeTypeControl(), CONTROLTDCLASS);
            LeftColumn.Add(feeRow);
            if (Invoice.IsOtherFeeType(CurrentInvoice.TypeID))
            {
                InvoiceEditRow descriptionFeeRow = new InvoiceEditRow();
                descriptionFeeRow.Build(GetLabel("Description of fee:", "lblFeeDescription"), LABELTDCLASS, GetFeeDescriptionControl(), CONTROLTDCLASS);
                LeftColumn.Add(descriptionFeeRow);
            }
            if (CurrentInvoice.HasFormula && CurrentInvoice.TypeID != Invoice.TITLEINSURANCEFEETYPEID)
            {
                InvoiceEditRow providerTypeRow = new InvoiceEditRow();
                providerTypeRow.Build(GetLabel("Provider:", "lblProviderType"), LABELTDCLASS, GetProviderForFormulaFee(), CONTROLTDCLASS);
                LeftColumn.Add(providerTypeRow);
                if (CurrentInvoice.FormulaData.ContainsKey("OfficerTitleId"))
                {
                    int officerTitleId = int.Parse(CurrentInvoice.FormulaData["OfficerTitleId"].ToString());
                    if(officerTitleId==0)
                    {
                        InvoiceEditRow providerRow = new InvoiceEditRow();
                        providerRow.Build(GetLabel("", LBLTITLENAMEID), LABELTDCLASS, GetProviderOtherForFormulaFeeControl(), CONTROLTDCLASS);
                        LeftColumn.Add(providerRow);
                    }
                }
            }
            else
            {
                InvoiceEditRow providerTypeRow = new InvoiceEditRow();
                providerTypeRow.Build(GetLabel("Provider type:", "lblProviderType"), LABELTDCLASS, GetProviderTypeControl(), CONTROLTDCLASS);
                LeftColumn.Add(providerTypeRow);
                if (CheckProviderRow())
                {
                    InvoiceEditRow providerRow = new InvoiceEditRow();
                    providerRow.Build(GetLabel("Provider:", LBLPROVIDERID), LABELTDCLASS, GetProviderControl(), CONTROLTDCLASS);
                    LeftColumn.Add(providerRow);
                }
                if (CurrentInvoice.TypeID == Invoice.TITLEENDORSMENTSFEETYPEID)
                {
                    InvoiceEditRow endorsementRow = new InvoiceEditRow();
                    endorsementRow.Build(GetLabel("List endorsements:", "lblEndorsemenst"), LABELTDCLASS, GetEndorsementsControl(), CONTROLTDCLASS);
                    LeftColumn.Add(endorsementRow);
                }
            }
            if (mp.MortgageInfo.TransactionTypeId == TRANSACTIONPURCHASE)
            {
                InvoiceEditRow chargeToRow = new InvoiceEditRow();
                chargeToRow.Build(GetLabel("Charge to:", "lblChargeTo"), LABELTDCLASS, GetChargeToControl(), CONTROLTDCLASS);
                LeftColumn.Add(chargeToRow);
            }
            if (CurrentInvoice.NeedOrder(DvVendorFeeOrder))
            {
                InvoiceEditRow sendOrderRow = new InvoiceEditRow();
                sendOrderRow.Build(new Label(), LABELTDCLASS, GetButtonSendControl(), CONTROLTDCLASS);
                LeftColumn.Add(sendOrderRow);
                InvoiceEditRow viewOrderRow = new InvoiceEditRow();
                viewOrderRow.Build(new Label(), LABELTDCLASS, GetButtonViewControl(), CONTROLTDCLASS);
                LeftColumn.Add(viewOrderRow);
            }
        }
        #endregion

        #region right column methods
/*
        private ArrayList GetStatusControl()
        {
            ArrayList res = new ArrayList();
            res.Add(new LiteralControl("&nbsp;"));
            res.Add(GetDropDownListControl(DDLSTATUSID, false, DDLCSS, DDLWIDTH));
            return res;
        }
*/
        private ArrayList GetFormulaSelectControl()
        {
            ArrayList res = new ArrayList();
            res.Add(new LiteralControl("&nbsp;"));
            res.Add(GetDropDownListControl(DDLFORMULAID, true, DDLCSS, DDLWIDTH));
            return res;
        }
        private ArrayList GetUseTotalsControl()
        {
            ArrayList res = new ArrayList();
            res.Add(new LiteralControl("&nbsp;"));
            res.Add(GetCheckBoxControl(CBUSETOTALSID, false, DDLCSS));
            return res;
        }
        private ArrayList GetAmountControl()
        {
            ArrayList res = new ArrayList();
            RadNumericTextBox tb = GetNumericTextBox(TBAMOUNTID);
            if (CurrentInvoice.TypeID == MortgageProfile.TYPELENDERLOANORIGINATIONFEE)
            {
                double maxallowed = (double)mp.MortgageInfo.LenderFee;
                tb.Attributes.Add("maxallowed", maxallowed.ToString()); 
            }
            res.Add(tb);
            res.Add(GetReqiuiredFieldValidator(TBAMOUNTID, "*"));
            if (CurrentInvoice.TypeID == MortgageProfile.TYPELENDERLOANORIGINATIONFEE)
            {
                res.Add(GetCustomValidator(TBAMOUNTID, "", "ValidateOriginatorFee"));
            }
            return res;
        }
        private ArrayList GetPocAmountControl()
        {
            //string js = "ValidatePocAmount1";
            //if (CurrentInvoice.FeeCategoryID == Invoice.GOVERNMENTCHARGECATEGORYID)
            //{
            //    js = "ValidatePocAmount2";
            //}
            ArrayList res = new ArrayList();
            res.Add(GetNumericTextBox(TBPOCAMOUNTID));
//            res.Add(GetCustomValidator(TBPOCAMOUNTID, "*" ,js));
            return res;
        }
        private ArrayList GetLenderCreditAmountControl()
        {
            //string js = "ValidatePocAmount1";
            //if (CurrentInvoice.FeeCategoryID == Invoice.GOVERNMENTCHARGECATEGORYID)
            //{
            //    js = "ValidatePocAmount2";
            //}
            ArrayList res = new ArrayList();
            res.Add(GetNumericTextBox(TBLENDERCREDITAMOUNTID));
//            res.Add(GetCustomValidator(TBLENDERCREDITAMOUNTID, "*", js));
            return res;
        }
        private ArrayList GetThirdPartyPaidAmount()
        {
            //string js = "ValidatePocAmount1";
            //if (CurrentInvoice.FeeCategoryID == Invoice.GOVERNMENTCHARGECATEGORYID)
            //{
            //    js = "ValidatePocAmount2";
            //}
            ArrayList res = new ArrayList();
            res.Add(GetNumericTextBox(TBTHIRDPARTYPAIDAMOUNTID));
            //            res.Add(GetCustomValidator(TBLENDERCREDITAMOUNTID, "*", js));
            return res;
        }

        private ArrayList GetDeedAmountControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetNumericTextBox(TBDEEDAMOUNTID));
            res.Add(GetReqiuiredFieldValidator(TBDEEDAMOUNTID, "*"));
            return res;
        }
        private ArrayList GetMortgageAmountControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetNumericTextBox(TBMORTGAGEAMOUNTID));
            return res;
        }
        private ArrayList GetReleaseAmountControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetNumericTextBox(TBRELEASEAMOUNTID));
            return res;
        }
        private static ArrayList GetFinancedAmountValueControl()
        {
            ArrayList res = new ArrayList();
            res.Add(GetLabel("", LBLFINANCEDAMOUNTID));
            return res;
        }
        private void BuildRightColumn()
        {
            RightColumn = new ArrayList();
            //InvoiceEditRow statusRow = new InvoiceEditRow();
            //statusRow.Build(GetLabel("Status:", "lblStatus"), LABELTDCLASS, GetStatusControl(), CONTROLTDCLASS);
            //RightColumn.Add(statusRow);
            if (CurrentInvoice.HasFormula)
            {
                InvoiceEditRow formulaRow = new InvoiceEditRow();
                formulaRow.Build(GetLabel("Choose formula:", "lblFormula"), LABELTDCLASS, GetFormulaSelectControl(), CONTROLTDCLASS);
                RightColumn.Add(formulaRow);
                if(CurrentInvoice.HasOrders)
                {
                    InvoiceEditRow gridTotals = new InvoiceEditRow();
                    gridTotals.Build(GetLabel("Use totals from order grid:", "lblTotal"), LABELTDCLASS, GetUseTotalsControl(), CONTROLTDCLASS);
                    RightColumn.Add(gridTotals);
                }
            }
            else
            {
                if (CurrentInvoice.FeeCategoryID != Invoice.GOVERNMENTCHARGECATEGORYID)
                {
                    InvoiceEditRow amountRow = new InvoiceEditRow();
                    amountRow.Build(GetLabel("Invoice Amount:", "lblAmount"), LABELTDCLASS, GetAmountControl(), CONTROLTDCLASS);
                    RightColumn.Add(amountRow);
                }
                else
                {
                    InvoiceEditRow deedAmountRow = new InvoiceEditRow();
                    deedAmountRow.Build(GetLabel("Deed Amount:", "lblDeedAmount"), LABELTDCLASS, GetDeedAmountControl(), CONTROLTDCLASS);
                    RightColumn.Add(deedAmountRow);
                    InvoiceEditRow mortgageAmountRow = new InvoiceEditRow();
                    mortgageAmountRow.Build(GetLabel("Mortgage Amount:", "lblMortgageAmount"), LABELTDCLASS, GetMortgageAmountControl(), CONTROLTDCLASS);
                    RightColumn.Add(mortgageAmountRow);
                    if (CurrentInvoice.TypeID == Invoice.RECORDINGFEETYPEID)
                    {
                        InvoiceEditRow releaseAmountRow = new InvoiceEditRow();
                        releaseAmountRow.Build(GetLabel("Release Amount:", "lblReleaseAmount"), LABELTDCLASS, GetReleaseAmountControl(), CONTROLTDCLASS);
                        RightColumn.Add(releaseAmountRow);
                    }
                }
                InvoiceEditRow pocRow = new InvoiceEditRow();
                pocRow.Build(GetLabel("POC Amount:", "lblPocAmount"), LABELTDCLASS, GetPocAmountControl(), CONTROLTDCLASS);
                RightColumn.Add(pocRow);
                InvoiceEditRow lenderCreditRow = new InvoiceEditRow();
                lenderCreditRow.Build(GetLabel("Lender Credit:", "lblLenderCreditAmount"), LABELTDCLASS, GetLenderCreditAmountControl(), CONTROLTDCLASS);
                RightColumn.Add(lenderCreditRow);
                InvoiceEditRow thirdPartyRow = new InvoiceEditRow();
                thirdPartyRow.Build(GetLabel("Third Party paid:", "lblThirdPartyPaidAmount"), LABELTDCLASS, GetThirdPartyPaidAmount(), CONTROLTDCLASS);
                RightColumn.Add(thirdPartyRow);
                InvoiceEditRow financedAmountRow = new InvoiceEditRow();
                financedAmountRow.Build(GetLabel("Financed Amount :", "lblFinancedAmount"), LABELTDCLASS, GetFinancedAmountValueControl(), CONTROLTDCLASS);
                RightColumn.Add(financedAmountRow);
            }
        }
        private void SetFormulaId()
        {
            DataView dv = DvFormulaList;
            int formulaId = CurrentInvoice.FormulaId;
            if (formulaId<=0)
            {
                formulaId = 1;
            }
            CurrentInvoice.FormulaId = formulaId;
        }

        private void BuildFormulaControl()
        {
            feeFormulaControl = null;
            SetFormulaId();
            switch(CurrentInvoice.TypeID)
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
            if(feeFormulaControl!=null)
            {
                ArrayList rows = feeFormulaControl.Build();
                if(rows!=null&&rows.Count>0)
                {
                    for(int i=0;i<rows.Count;i++)
                    {
                        tblFormula.Rows.Add((HtmlTableRow)rows[i]);
                    }
                }
            }
        }
        #endregion
        private static void AddEmptyTd(int count, IList column)
        {
            for (int i = 0; i < count; i++)
            {
                InvoiceEditRow row = new InvoiceEditRow();
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
                InvoiceEditRow l = (InvoiceEditRow)LeftColumn[i];
                row.Cells.Add(l.LabelCell);
                row.Cells.Add(l.ControlCell);
                InvoiceEditRow r = (InvoiceEditRow)RightColumn[i];
                row.Cells.Add(r.LabelCell);
                row.Cells.Add(r.ControlCell);
                Table1.Rows.Add(row);
            }
            if (CurrentInvoice.TypeID == MortgageProfile.TYPELENDERLOANORIGINATIONFEE)
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
            if(CurrentInvoice.HasFormula)
            {
                BuildFormulaControl();
            }
            else
            {
                tblFormula.Visible = false;
            }
        }
        #endregion
    }    
}