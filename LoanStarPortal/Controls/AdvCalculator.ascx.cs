using System;
using System.Data;
using System.Text;
using System.Collections;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using LoanStar.Common;
using Telerik.WebControls;


namespace LoanStarPortal.Controls
{
    public partial class AdvCalculator : AppControl
    {
        #region constants
        private const int MAXPRODUCTCOUNT = 4;
        private const string ADVCALCLORDER = "advcalculatororder";
        private const string SELECTNAME = "id={0}";
        private const string SELECTLINKID = "lbSelectProduct_{0}";
        private const string DDLPRODUCTID = "ddlProduct_{0}";
        private const string INITIALDRAWAMOUNTID = "tbInitialDrawAmount";
        private const string CREADITLINEAMOUNTID = "tbCreditLine";
        private const string MONTHLYSERVICEFEE = "MonthlyServiceFee";
        private const string SAVEHOMEVALUEID = "btnSaveHomeValue";
        private const string SAVEPAYOFFID = "btnSavePayoff";
        private const string PARAMETERS = "calculatorparameters";
        private const string ONCLICK = "onclick";
//        private const string JSTENURECLICK = "rbMontlyIncome_ClientClick(this,{0});";
        private const string JSTENURECLICK = "rbMontlyIncome_ClientClick(this);";
        #endregion

        #region fields
        private LoanStar.Common.AdvCalculator advCalc = null;
        private bool isLateBinding = false;
        private int[] productsOrder;
        private DataView dvProducts;
        private DataView dvCalcFields;
        private int visibleColumns;
        private bool skipValidation = false;
        private readonly decimal[] monthlyFeeValues = new decimal[MAXPRODUCTCOUNT];
        private ArrayList dvFees;
        private bool resetHomeValue = false;
        private readonly bool needInitialDrawValue = false;
        private readonly bool needCreditLineValue = false;
        private bool needMonthlyIncomeValue = false;
        private bool needTermValue = false;
        Hashtable bindings = null;
        MortgageProfile mp ;
//        private string trFinalTermId = "";
        #endregion

        #region properties
        private bool IsAppRatesUsed
        {
            get
            {
                Object o = ViewState["apprate"];
                bool res = false;
                if (o != null)
                {
                    try
                    {
                        res = bool.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set
            {
                ViewState["apprate"] = value;
            }
        }
        private string Parameters
        {
            get 
            {
                string res = String.Empty;
                Object o = ViewState[PARAMETERS];
                if (o != null)
                {
                    res = o.ToString();
                }
                return res;
            }
            set { ViewState[PARAMETERS] = value; }
        }
        private DataView DvCalcFields
        {
            get 
            {
                if (dvCalcFields == null)
                {
                    dvCalcFields = LoanStar.Common.AdvCalculator.GetFields();
                }
                return dvCalcFields;
            }
        }
        private bool FirstLoad
        {
            get
            {
                Object o = ViewState["FirstLoadAdvCalc"];
                bool res = true;
                if (o != null)
                {
                    try
                    {
                        res = bool.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set
            {
                ViewState["FirstLoadAdvCalc"] = value;
            }
        }
        private bool IsScenarioColumnVisible
        {
            get
            {
                Object o = ViewState["scenariocolumn"];
                bool res = false;
                if (o != null)
                {
                    try
                    {
                        res = bool.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set
            {
                ViewState["scenariocolumn"] = value;
            }
        }
        private int MortgageProfileId
        {
            get
            {
                if (Session[Constants.MortgageID] == null)
                    Session[Constants.MortgageID] = 0;
                return Convert.ToInt32(Session[Constants.MortgageID].ToString());
            }
        }
        private DataView DvProducts
        {
            get 
            {
                if (dvProducts == null)
                {
                    dvProducts = Product.GetProductsListForOriginator(CurrentPage.CurrentUser.CompanyId,mp.ProductID);
                }
                dvProducts.RowFilter = "";
                return dvProducts;
            }
        }
        private bool isNetNegative = false;
        #endregion
        
        #region methods
        private void BuildMenu()
        {
//            rmCalculator.Items[0].Items[1].Enabled = advCalc.SelectedProductId > 0;
            rmCalculator.Items[0].Items[0].Text = (IsScenarioColumnVisible ? "Hide" : "Show") + " scenario column";
            SetApplicationDateRateMenu();
        }
        private void SetApplicationDateRateMenu()
        {
            bool visible = false;
            if(advCalc.SelectedProductId>0)
            {
                Product prod = new Product(advCalc.SelectedProductId);
                if(prod.PrincipleLimitProtectionYN)
                {
                    if(mp.MortgageInfo.ClosingDate==null)
                    {
                        DateTime dt1 = Holidays.GetWorkDate((mp.MortgageInfo.CaseNumAssignDate ?? DateTime.Now).AddDays(prod.DaysToLock), mp.CompanyID);
                        if (dt1 > DateTime.Now)
                        {
                            visible = true;
                        }
                    }
                }
            }
            rmCalculator.Items[0].Items[1].Visible = visible;
            if(visible)
            {
                rmCalculator.Items[0].Items[1].Text = IsAppRatesUsed ? "Don't use application date rate" : "Use application date rate";
            }
            advCalc.UseAppDateRates = visible;
        }

        private void InitAdvancedCalculator()
        {
            mp = CurrentPage.GetMortgage(MortgageProfileId);
            Logger log1 = AppSettings.LogCalculator
                              ? new Logger(Server.MapPath(Constants.STORAGEFOLDER), "calculator.log", true)
                              : null;
            advCalc = new LoanStar.Common.AdvCalculator(mp, DvProducts, true, log1);
            productsOrder = advCalc.GetProductOrder(MAXPRODUCTCOUNT);
            visibleColumns = productsOrder.Length;
            GetServiceeFee();
        }
        private void GetServiceeFee()
        {
            for (int i = 0; i < MAXPRODUCTCOUNT; i++) monthlyFeeValues[i] = -1;
            dvFees = new ArrayList();
            mp = CurrentPage.GetMortgage(MortgageProfileId);
            int lenderId = mp.MortgageInfo.LenderAffiliateID;
            for (int i = 0; i < visibleColumns; i++)
            {
                DataView dv = Product.GetLenderServiceFee(productsOrder[i], lenderId);
                if (advCalc.ServiceeFeeSelection.Contains(productsOrder[i]))
                {
                    decimal fee = (decimal)advCalc.ServiceeFeeSelection[productsOrder[i]];
                    dv.RowFilter = String.Format("fee={0}", fee);
                    if (dv.Count == 0)
                    {
                        dv.RowFilter = "Isdefault=1";
                    }
                    if (dv.Count > 0)
                    {
                        monthlyFeeValues[i] = Convert.ToDecimal(dv[0]["fee"].ToString());
                    }
                }
                else
                {
                    dv.RowFilter = "Isdefault=1";
                    if (dv.Count > 0)
                    {
                        monthlyFeeValues[i] = Convert.ToDecimal(dv[0]["fee"].ToString());
                    }
                }
                dv.RowFilter = "";
                dvFees.Add(dv);
            }
        }
        private void BindData()
        {
            advCalc.CalculatorError = String.Empty;
            string errMsg = "";
            if (!skipValidation) 
            {
                errMsg = advCalc.Validate();
            }
            BuildMenu();
            BuildTable();
            if (!String.IsNullOrEmpty(errMsg))
            {
                resAdvCalcMessage.Value += String.IsNullOrEmpty(resAdvCalcMessage.Value) ? errMsg : Environment.NewLine + errMsg;
            }
            if (!skipValidation)
            {
                if (!String.IsNullOrEmpty(advCalc.CalculatorError))
                {
                    resAdvCalcMessage.Value += String.IsNullOrEmpty(resAdvCalcMessage.Value) ? advCalc.CalculatorError : Environment.NewLine + advCalc.CalculatorError;
                }
            }
        }
        private void BindProducts()
        {
            for (int i = 0; i < MAXPRODUCTCOUNT; i++)
            {
                BindProductDddl(i);
            }
        }
        private void BindProductDddl(int index)
        {
            DropDownList ddl = (DropDownList)FindControl(String.Format(DDLPRODUCTID,index));
            if(ddl!=null)
            {
                if (index < visibleColumns)
                {
                    ddl.Attributes.Add("onchange", "ClearNeedCheckCalcFalg();");
                    DataView dv = DvProducts;
                    string filter = productsOrder[index] == advCalc.SelectedProductId
                                        ? ""
                                        : String.Format("id<>{0}", advCalc.SelectedProductId);
                    dv.RowFilter = filter;
                    ddl.DataSource = dv;
                    ddl.DataTextField = "name";
                    ddl.DataValueField = "id";
                    ddl.DataBind();
                    ddl.EnableViewState = false;
                    ddl.ClearSelection();
                    ListItem li = ddl.Items.FindByValue(productsOrder[index].ToString());
                    if(li!=null)
                    {
                        li.Selected = true;
                    }
                    ddl.Visible = true;
                    if(productsOrder[index]==advCalc.SelectedProductId)
                    {
                        ddl.Enabled = false;
                    }
                }
                else
                {
                    ddl.Visible = false;
                }
            }
        }
        protected string SetStyle(int index)
        {
            string ret = "";
            if (index < visibleColumns)
            {
                if (advCalc.SelectedProductId == productsOrder[index])
                    ret = "class=\"selected_col\"";
            }
            return ret;
        }
        private void SetNewOrder(int index, int newId)
        {
            int currentId = productsOrder[index];
            int replaceIndex = CheckProductIndex(newId,index);
            productsOrder[index] = newId;
            if (replaceIndex >= 0)
            {
                productsOrder[replaceIndex] = currentId;
            }
        }
        private int CheckProductIndex(int id, int index)
        {
            for (int i = 0; i < productsOrder.Length; i++)
            {
                if ((i != index) && (productsOrder[i] == id))
                {
                    return i;
                }
            }
            return -1;
        }
        private void ProcessPostBack()
        {
            string controlName = Page.Request["__EVENTTARGET"];
            if (!String.IsNullOrEmpty(controlName))
            {
                needMonthlyIncomeValue = GetPostedValueBool(chkMonthlyIncome.ID);
                needTermValue = GetPostedValue(rbMontlyIncome.ID) == "Term";
                advCalc.NeedMonthlyIncome = needMonthlyIncomeValue;
                advCalc.NeedTerm = needTermValue;
                bool res = !needMonthlyIncomeValue;
                if (!res)
                {
                    res = needTermValue;
                    if (res)
                    {
                        decimal amount = GetPostedValueDecimal(tbAmount.ID);
                        res = amount > 0;
                    }
                }
                advCalc.NeedCreditLine = res;
                if (controlName.Contains("ddlProduct_"))
                {
                    int newId = GetDdlSelectedValue(controlName.Replace(":", "$"));
                    int index = GetIndex(controlName);
                    if (index >= 0)
                    {
                        SetNewOrder(index, newId);
                        SaveParameters();
                        isLateBinding = true;
                    }
                }
                else if (controlName.Contains("lbSelectProduct_"))
                {
                    int index = GetIndex(controlName);
                    if (index >= 0)
                    {
                        advCalc.SelectedProductId = productsOrder[index];
                        productsOrder = advCalc.GetProductOrder(MAXPRODUCTCOUNT);
                        isLateBinding = true;
                        TabPackage doc = ((Default)Page).DocumentPackage;
                        if (doc != null)
                        {
                            CurrentPage.CenterRightPanelUpdateNeeded = true;
                            CurrentPage.SetRuleEvaluationNeeded(true);
                            doc.RecalculateDocList(CurrentPage.GetMortgage(MortgageProfileId));
                        }
                    }
                }
                else if (controlName.EndsWith(btnCalculate.ID))
                {
                    skipValidation = true;
                }
            }
            BindData();
        }

        private void UpdateHomeValue(decimal homeValue)
        {
            advCalc.UpdateHomeValue(homeValue);
        }
        private int GetDdlSelectedValue(string controlName)
        {
            int res = -1;
            try
            {
                res = int.Parse(Page.Request.Form[controlName]);
            }
            catch
            {
            }
            return res;
        }

        private decimal GetPostedValueDecimal(string controlId)
        {
            decimal res = 0;
            string s = GetPostedValue(controlId);
            if (!String.IsNullOrEmpty(s))
            {
                try
                {
                    res = Convert.ToDecimal(s);
                }
                catch { }
            }
            return res;
        }
        private bool GetPostedValueBool(string controlId)
        {
            bool res;
            string s = GetPostedValue(controlId);
            res = s.ToLower() == "on";
            return res;
        }
        private string GetPostedValue(string controlId)
        {
            string res = "";
            for (int i = 0; i < Page.Request.Form.AllKeys.Length; i++)
            {
                string s = Page.Request.Form.AllKeys[i];
                if (s.EndsWith("$" + controlId))
                {
                    res = Page.Request.Form[s];
                    break;
                }
            }
            return res;
        }
        private void SaveParameters()
        {
            advCalc.Save(productsOrder);
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
        private static string[] GetBindingProperties(DataView dv)
        {
            dv.RowFilter = "bindingpropertyname<>''";
            string[] res = new string[dv.Count];
            for (int i = 0; i < dv.Count; i++)
            {
                res[i] = dv[i]["bindingpropertyname"].ToString();
            }
            return res;
        }
        private void DownloadParameters()
        {
            string productName = advCalc.GetName(advCalc.SelectedProductId);
            string s = Parameters;
            if (!String.IsNullOrEmpty(s))
            {
                MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(s));
                DownloadGenerationError = String.Empty;
                DownloadStream = stream;
                DownloadContentType = "text/plain";
                DownloadFileName = String.Format("{0}_parameters.txt", productName);
                CurrentPage.ClientScript.RegisterStartupScript(GetType(), "DownloadFile", "<script language=\"javascript\" type=\"text/javascript\">location.href = 'DownLoad.aspx';</script>");
            }
        }
        private void SetCalculatorParameters()
        {
            string[] scenarioInputs = GetScenarioInputs(DvCalcFields);
            if(scenarioInputs != null)
            {
                Hashtable tbl = new Hashtable();
                for(int i=0;i<scenarioInputs.Length;i++)
                {
                    string propertyName = scenarioInputs[i];
                    if ((propertyName == "HomeValue") && resetHomeValue) continue;
                    String s = GetScenarioInputValue(propertyName);
                    if (!String.IsNullOrEmpty(s))
                    {
                        Object o = advCalc.GetScenarioValue(propertyName,s);
                        if (o != null)
                        {
                            tbl.Add(propertyName, o);
                        }
                    }                    
                }
                advCalc.ScenarioValues = tbl;
            }
        }
        private string GetScenarioInputValue(string controlId)
        {
            string res = "";
            RadInputControl tb = (RadInputControl)FindControl(controlId + "_sc");
            if (tb != null)
            {
                res = tb.Text;
            }
            return res;
        }
        private static string[] GetScenarioInputs(DataView dv)
        {
            dv.RowFilter = "HasExtraInput=1";
            string[] res = new string[dv.Count];
            for (int i = 0; i < dv.Count; i++)
            {
                res[i] = dv[i]["bindingpropertyname"].ToString();
            }
            return res;
        }
        private void SaveMortgageData()
        {
            decimal mpPaymentAmount = String.IsNullOrEmpty(tbAmount.Text) ? 0 : Convert.ToDecimal(tbAmount.Text);
            decimal mpTerm = String.IsNullOrEmpty(tbMonths.Text) ? 0 : Convert.ToDecimal(tbMonths.Text);
            decimal mpInitialDraw = GetDecimalInputValue(INITIALDRAWAMOUNTID);
            decimal mpCreditLine = GetDecimalInputValue(CREADITLINEAMOUNTID); 
            advCalc.UpdateMortgageData(mpPaymentAmount, mpTerm, mpInitialDraw, mpCreditLine);
        }
        private decimal GetDecimalInputValue(string controlId)
        {
            decimal res = 0;
            RadInputControl tb = (RadInputControl)FindControl(controlId);
            if(tb!=null)
            {
                if (!String.IsNullOrEmpty(tb.Text))
                    res =  Convert.ToDecimal(tb.Text);
            }
            return res;
        }
        private void BindFees()
        { 
            for (int i = 0; i < visibleColumns; i++)
            {
                DropDownList ddl = (DropDownList)FindControl(MONTHLYSERVICEFEE+"_"+i);
                if (ddl != null)
                {
                    DataView dv = dvFees[i] as DataView;
                    ddl.DataSource = dv;
                    ddl.DataTextField = "fee";
                    ddl.DataTextFormatString = "{0:C}";
                    ddl.DataValueField = "id";
                    ddl.DataBind();
                    ListItem li = ddl.Items.FindByText(String.Format("{0:C}",monthlyFeeValues[i]));
                    if (li != null)
                    {
                        li.Selected = true;
                    }                               
                }
            }             
        }
        private void GetMontlyFeeValues()
        {
            for (int i = 0; i < Page.Request.Form.AllKeys.Length; i++)
            {
                string s = Page.Request.Form.AllKeys[i];
                if (s.Contains("$" + MONTHLYSERVICEFEE+"_"))
                {
                    int index = GetIndex(s);
                    try
                    {
                        int res = int.Parse(Page.Request.Form[s]);
                        DataView dv = dvFees[index] as DataView;
                        if (dv != null)
                        {
                            dv.RowFilter = String.Format("id={0}", res);
                            if (dv.Count == 1)
                            {
                                monthlyFeeValues[index] = Convert.ToDecimal(dv[0]["fee"].ToString());
                            }
                            dv.RowFilter = "";
                        }
                    }
                    catch { }
                }
            }
        }
        private void SetPaymentPlanParameters()
        {
            if (advCalc.NeedCreditLine && advCalc.SelectedProductId > 0)
            {
                if (bindings.ContainsKey(advCalc.SelectedProductId))
                {
                    decimal mpCreditLine = 0;
                    decimal month = 0;
                    Hashtable tbl = (Hashtable)bindings[advCalc.SelectedProductId];
                    if (tbl.ContainsKey("LineOfCredit"))
                    {
                        RadNumericTextBox tbcl = (RadNumericTextBox)FindControl(CREADITLINEAMOUNTID);
                        if (tbcl != null)
                        {
                            try
                            {
                                mpCreditLine = decimal.Parse(tbl["LineOfCredit"].ToString());
                                advCalc.UpdateCreditLine(mpCreditLine);
                            }
                            catch 
                            { 
                            }
                            tbcl.Text = mpCreditLine == 0 ? "" : mpCreditLine.ToString();
                        }
                    }
                    if (tbl.ContainsKey("FinalTerm"))
                    {
                        try
                        {
                            month = decimal.Parse(tbl["FinalTerm"].ToString());
                            advCalc.UpdateCreditLine(mpCreditLine);
                        }
                        catch
                        {
                        }
                        tbMonths.Text = month == 0 ? "" : month.ToString();
                    }
                }
            }
        }
        private void SetAmount()
        {
            if (advCalc.NeedTerm && advCalc.SelectedProductId > 0 && advCalc.MPPaymentAmount==0)
            {
                if (bindings.ContainsKey(advCalc.SelectedProductId))
                {
                    Hashtable tbl = (Hashtable)bindings[advCalc.SelectedProductId];
                    if (tbl.ContainsKey("PeriodicPayment"))
                    {
                        RadNumericTextBox tbcl = (RadNumericTextBox)FindControl(tbAmount.ID);
                        if (tbcl != null)
                        {
                            tbcl.Text = tbl["PeriodicPayment"].ToString();
                        }
                    }
                }
            }
        }
        private string GetSelectedFee()
        {
            string res = String.Empty;
            for (int i = 0; i < visibleColumns; i++)
            {
                if(i>0)
                {
                    res+=";";
                }
                res += String.Format("{0}={1}", productsOrder[i], monthlyFeeValues[i]);
            }
            return res;
        }
        #region dynamic html methods
        private void BuildTable()
        {
            pnlRows.Controls.Clear();
            if (!isLateBinding) BindProducts();
            pnlRows.Controls.Add(GetProductNamesRow());
            pnlRows.Controls.Add(GetDownLoadLinkRow());
            ArrayList parametersRows = GetParametersRows();
            for (int i = 0; i < parametersRows.Count; i++)
            {
                pnlRows.Controls.Add((HtmlTableRow)parametersRows[i]);
            }
            if (isNetNegative)
            {
                chkMonthlyIncome.Checked = false;
            }
            else
            {
                chkMonthlyIncome.Checked = advCalc.NeedMonthlyIncome;
            }
            if (advCalc.NeedMonthlyIncome)
                trMonthlyIncome.Attributes.Add("style", "display:block");
            else
                trMonthlyIncome.Attributes.Add("style", "display:none");

            rbMontlyIncome.SelectedValue = advCalc.NeedTerm ? "Term" : "Tenure";
            if (advCalc.NeedTerm)
                trTerm.Attributes.Add("style", "display:block");
            else
                trTerm.Attributes.Add("style", "display:none");
            tbAmount.Text = advCalc.MPPaymentAmount >= 0 ? advCalc.MPPaymentAmount.ToString() : String.Empty;
            tbMonths.Text = advCalc.MPTerm >= 0 ? String.Format("{0:d}", (int)advCalc.MPTerm) : String.Empty;
        }
        private ArrayList GetParametersRows()
        {
            string[] properties = GetBindingProperties(DvCalcFields);
            bindings = new Hashtable();
            for (int i = 0; i < MAXPRODUCTCOUNT; i++)
            {
                if (i < visibleColumns)
                {
                    int productId = productsOrder[i];
                    if (!bindings.ContainsKey(productId))
                    {
                        string parameters;
                        string errMessage;
                        Hashtable tbl = advCalc.GetValues(productId, properties, monthlyFeeValues[i], out errMessage, out parameters);
                        bindings.Add(productId, tbl);
                        if (productId == advCalc.SelectedProductId)
                        {
                            Parameters = parameters;
                            if(advCalc.IsNetAvavilableNegative)
                            {
                                isNetNegative = true;
                            }
                        }
                    }
                }
            }
            ArrayList rows = new ArrayList();
            DvCalcFields.RowFilter = "";
            for (int i = 0; i < DvCalcFields.Count; i++)
            {
                HtmlTableRow row = new HtmlTableRow();
                row.Attributes.Add("style", "vertical-align:top");
                row.Cells.Add(GetLabelCell(DvCalcFields[i]));
                row.Cells.Add(GetExtraInputCell(DvCalcFields[i]));
                for (int j = 0; j < MAXPRODUCTCOUNT; j++)
                {
                    if (j < visibleColumns)
                    {
                        row.Cells.Add(GetProductValues(j, (Hashtable)bindings[productsOrder[j]], DvCalcFields[i]));
                    }
                    else
                    {
                        row.Cells.Add(new HtmlTableCell());
                    }                    
                }
                rows.Add(row);
            }
            return rows;
        }
        private HtmlTableCell GetProductValues(int index, IDictionary productValues, DataRowView dr)
        {
            HtmlTableCell td = new HtmlTableCell();
            if(index < visibleColumns)
            {
                string propertyName = dr["bindingpropertyname"].ToString();
                if (!String.IsNullOrEmpty(propertyName))
                {
                    if (propertyName == MONTHLYSERVICEFEE)
                    {
                        DropDownList ddl = new DropDownList();
                        ddl.TabIndex = -1;
                        ddl.ID = propertyName + "_" + index;
                        string css = dr["ctlcss"].ToString();
                        if (!String.IsNullOrEmpty(css))
                        {
                            ddl.CssClass = css;
                        }
                        td.Controls.Add(ddl);
                    }
                    else
                    {
                        TextBox tb = new TextBox();
                        tb.ID = propertyName + "_" + index;
                        tb.TabIndex = -1;
                        tb.ReadOnly = true;
                        Object o = productValues[propertyName];
                        string format = dr["FormatString"].ToString();
                        string text;
                        if (String.IsNullOrEmpty(format))
                        {
                            text = o.ToString();
                        }
                        else if (format.Contains("{0}"))
                        {
                            text = String.Format(format, o);
                        }
                        else
                        {
                            if (o is decimal)
                            {
                                decimal d = (decimal)o;
                                text = d.ToString(format);
                            }
                            else if (o is double)
                            {
                                double d = (double)o;
                                text = d.ToString(format);
                            }
                            else
                            {
                                text = o.ToString();
                            }
                        }
                        tb.Text = text;
                        string css = dr["ctlcss"].ToString();
                        if (!String.IsNullOrEmpty(css))
                        {
                            tb.CssClass = css;
                        }
                        if(propertyName=="FinalTerm")
                        {
                            tb.Attributes.Add("val",tb.Text);
                            if(!advCalc.NeedTerm)
                            {
                                tb.Text = "Tenure";
                            }
                        }
                        td.Controls.Add(tb);
                    }
                }
            }
            return td;
        }
        private HtmlTableCell GetExtraInputCell(DataRowView dr)
        {
            HtmlTableCell td = new HtmlTableCell();
            if (IsScenarioColumnVisible)
            { 
                if (Convert.ToBoolean(dr["hasextrainput"].ToString()))
                {
                    string propertyname = dr["bindingpropertyname"].ToString();
                    if (propertyname == MONTHLYSERVICEFEE ) return td;
                    if (advCalc.HasSetter(propertyname))
                    {
                        RadNumericTextBox tb = new RadNumericTextBox();
                        tb.EnableViewState = true;
                        tb.ID = propertyname + "_sc";
                        tb.CssClass = "lcInputTxtToRight";
                        string format = dr["formatstring"].ToString();
                        tb.Width = Unit.Pixel(70);
                        tb.MinValue = 0;
                        if (propertyname == "Age")
                        {
                            tb.Type = NumericType.Number;
                            tb.NumberFormat.DecimalDigits = 0;
                            tb.MinValue = 50;
                            tb.MaxValue = 150;
                        }
                        else
                        {
                            if (format.Contains("%"))
                            {
                                tb.Type = NumericType.Percent;
                            }
                            else
                            {
                                tb.Type = NumericType.Currency;
                            }
                        }
                        if (advCalc.ScenarioValues != null)
                        {
                            if (advCalc.ScenarioValues.ContainsKey(propertyname))
                            {
                                tb.Text = advCalc.ScenarioValues[propertyname].ToString();
                            }
                        }
                        td.Controls.Add(tb);
                        if (propertyname == "HomeValue")
                        {                            
                            Button btn = new Button();
                            btn.Click +=SaveHomeValue;
                            btn.ID = SAVEHOMEVALUEID;
                            btn.Text = "Save";
                            btn.Width = Unit.Pixel(50);
                            btn.Height = Unit.Pixel(18);
                            btn.Attributes.Add("style", "font-size:11px");
                            btn.Attributes.Add("onclick", "ClearNeedCheckCalcFalg();");
                            HtmlGenericControl div = new HtmlGenericControl("div");
                            div.Attributes.Add("style","padding:2px;width:100%");
                            div.Controls.Add(btn);
                            td.Controls.Add(div);
                        }
                        if (propertyname == "MortgageBalance")
                        {
                            Button btn = new Button();
                            btn.Click += SavePayoff;
                            btn.ID = SAVEPAYOFFID;
                            btn.Text = "Save";
                            btn.Width = Unit.Pixel(50);
                            btn.Height = Unit.Pixel(18);
                            btn.Attributes.Add("style", "font-size:11px");
                            btn.Attributes.Add("onclick", "ClearNeedCheckCalcFalg();");
                            HtmlGenericControl div = new HtmlGenericControl("div");
                            div.Attributes.Add("style", "padding:2px;width:100%");
                            div.Controls.Add(btn);
                            td.Controls.Add(div);
                        }
                    }
                }
            }
            return td;
        }
        private HtmlTableCell GetLabelCell(DataRowView dr)
        {
            HtmlTableCell td = new HtmlTableCell();
            string css = dr["tdcss"].ToString();
            if (!String.IsNullOrEmpty(css))
            {
                td.Attributes.Add("class", css);
            }
            string propertyName = dr["bindingpropertyname"].ToString();
            if (propertyName == "InitialDraw")
            {
                td.Controls.Add(GetInitDrawControls(dr["displaylabel"].ToString()));
            }
            else if (propertyName == "LineOfCredit")
            {
                td.Controls.Add(GetCreditLineControls(dr["displaylabel"].ToString()));
            }
            else
            {
                td.InnerText = dr["DisplayLabel"].ToString();
            }
            if (propertyName == "FinalTerm")
            {
                td.Attributes.Add("id","tdlblFinalTerm");
            }
            return td;
        }
        private HtmlTable GetCreditLineControls(string label)
        {
            HtmlTable tbl = GetControlTable();
            HtmlTableRow tr = new HtmlTableRow();
            tr.Cells.Add(GetTd( label));
            tr.Cells.Add(GetRadInputTd(CREADITLINEAMOUNTID, advCalc.MPCreditLine));
            tbl.Rows.Add(tr);
            return tbl;
        }
        private HtmlTable GetInitDrawControls(string label)
        {
            HtmlTable tbl = GetControlTable();
            HtmlTableRow tr = new HtmlTableRow();
            tr.Cells.Add(GetTd(label));
            tr.Cells.Add(GetRadInputTd(INITIALDRAWAMOUNTID, advCalc.MPInitialDraw));
            tbl.Rows.Add(tr);
            return tbl;
        }
        private static HtmlTableCell GetRadInputTd(string Id, decimal val)
        {
            HtmlTableCell td = new HtmlTableCell();
            RadNumericTextBox tb = new RadNumericTextBox();
            tb.ID = Id;
            tb.Type = NumericType.Currency;
            tb.Width = Unit.Pixel(70);
            tb.CssClass = "lcInputTxtToRight";
            tb.MinValue = 0;
            tb.Text = val == 0 ? "" : val.ToString();
            tb.ClientEvents.OnValueChanged = "SetCalcFalg";
            td.Controls.Add(tb);
            return td;
        }
        private static HtmlTableCell GetTd(string text)
        {
            HtmlTableCell td = new HtmlTableCell();
            td.InnerText = text;
            return td;
        }
        private static HtmlTable GetControlTable()
        {
            HtmlTable tbl = new HtmlTable();
            tbl.Border = 0;
            tbl.CellSpacing = 3;
            tbl.CellPadding = 0;
            tbl.Attributes.Add("style", "width:99%");
            return tbl;
        }
        private HtmlTableRow GetDownLoadLinkRow()
        {
            HtmlTableRow row = new HtmlTableRow();
            row.Cells.Add(new HtmlTableCell());
            row.Cells.Add(new HtmlTableCell());
            for (int i = 0; i < MAXPRODUCTCOUNT; i++)
            {
                row.Cells.Add(GetSelectProductLinkTd(i));
            }
            return row;
        }
        private HtmlTableCell GetSelectProductLinkTd(int index)
        {
            HtmlTableCell td = new HtmlTableCell();
            if (index < visibleColumns)
            {
                LinkButton lb = new LinkButton();
                lb.ID = String.Format(SELECTLINKID, index);
                lb.Text = "Select product";
                lb.CssClass = "AdvCalcSelProd";
                lb.Enabled = productsOrder[index] != advCalc.SelectedProductId;
                lb.OnClientClick = "ClearNeedCheckCalcFalg();";
                td.Controls.Add(lb);
            }
            return td;
        }
        private HtmlTableRow GetProductNamesRow()
        {
            HtmlTableRow row = new HtmlTableRow();
            row.Cells.Add(new HtmlTableCell());
            row.Cells.Add(new HtmlTableCell());
            for (int i = 0; i < MAXPRODUCTCOUNT; i++)
            {
                row.Cells.Add(GetProductNameTd(i));
            }
            return row;
        }
        private HtmlTableCell GetProductNameTd(int index)
        {
            HtmlTableCell td = new HtmlTableCell();
            if (index < visibleColumns)
            {
                td.Attributes.Add("class", "advcalcheadertd");
                int productId = productsOrder[index];
                DataRow[] rows = DvProducts.Table.Select(String.Format(SELECTNAME, productId));
                if (rows.Length == 1)
                {
                    td.InnerText = rows[0]["name"].ToString();
                }
            }
            return td;
        }
        #endregion
        private string GetModifiedValue()
        {
            string res = "0";
            if (!String.IsNullOrEmpty(Page.Request.Form["calcmodified"]))
            {
                res = Page.Request.Form["calcmodified"];
            }
            return res;
        }

        #endregion

        #region event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentPage.ClientScript.RegisterHiddenField("calcneedcheck","1");
            CurrentPage.ClientScript.RegisterHiddenField("calcmodified", GetModifiedValue());
            btnCalculate.Attributes.Add("onclick", "ClearCalcFalg();");
            CurrentPage.ClientScript.RegisterStartupScript(GetType(), "InitAdvancedCalculator", "<script language=\"javascript\" type=\"text/javascript\">DisplayMessage();</script>");
            InitAdvancedCalculator();
            string controlName = Page.Request["__EVENTTARGET"];
            string errMessage = advCalc.ValidateFirst();
            if (!String.IsNullOrEmpty(controlName) && controlName.Contains("AdvCalculatorValues"))
                AdvCalculatorValues.AdvancedCalculator = advCalc;
            else if (!String.IsNullOrEmpty(errMessage))
            {
                PanelAdvancedCalculatorValues.Visible = true;
                PanelAdvancedCalculator.Visible = false;

                AdvCalculatorValues.AdvancedCalculator = advCalc;
                resAdvCalcMessage.Value = errMessage;
            }
            else
            {
                PanelAdvancedCalculatorValues.Visible = false;
                PanelAdvancedCalculator.Visible = true;

                if (FirstLoad)
                {
                    Session.Remove(ADVCALCLORDER);
                    FirstLoad = false;
                    IsScenarioColumnVisible = false;
                    BindData();
                }
                else
                {
                    ProcessPostBack();
                }
            }
        }
        protected void ucAdvancedCalculatorValues_ValidateFirst(object sender, EventArgs e)
        {
            resAdvCalcMessage.Value = advCalc.ValidateFirst();
            FirstLoad = !String.IsNullOrEmpty(resAdvCalcMessage.Value);

            if (!FirstLoad)
            {
                PanelAdvancedCalculatorValues.Visible = false;
                PanelAdvancedCalculator.Visible = true;
                BindData();
            }
        }
        protected void SavePayoff(object sender, EventArgs e)
        {
            advCalc.Save(productsOrder);
            SaveMortgageData();
            GetMontlyFeeValues();
            string s = GetScenarioInputValue("MortgageBalance");
            if (!String.IsNullOrEmpty(s))
            {
                try
                {
                    decimal payoffValue = Convert.ToDecimal(s);
                    if (payoffValue > 0)
                    {
                        UpdatePayoff(payoffValue);
                    }
                }
                catch { }
            }
            SetCalculatorParameters();
            BindData();
        }
        private void UpdatePayoff(decimal amount)
        {
            if(mp.Payoffs.Count==0)
            {
                mp.AddPayoff(amount);
            }
            else
            {
                NavigateToPayoffs();
            }
        }
        private void NavigateToPayoffs()
        {
            Session[Constants.GOTOPAYOFFS] = true;
            Session[Constants.CURRENTTOPFIRSTTABID] = MortgageTab.TABPROPERTYID;
            Response.Redirect("default.aspx");
        }

        protected void SaveHomeValue(object sender, EventArgs e)
        {
            advCalc.Save(productsOrder);
            SaveMortgageData();            
            GetMontlyFeeValues();
            string s = GetScenarioInputValue("HomeValue");
            if(!String.IsNullOrEmpty(s))
            {
                try
                {
                    decimal homeValue = Convert.ToDecimal(s);
                    if (homeValue > 0)
                    {
                        UpdateHomeValue(homeValue);
                        resetHomeValue = true;
                    }
                }                
                catch{}
            }
            SetCalculatorParameters();
            BindData();
        }
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            advCalc.NeedInitialDraw = needInitialDrawValue;
            advCalc.NeedMonthlyIncome = needMonthlyIncomeValue;
            advCalc.NeedTerm = needTermValue;
            bool res = needCreditLineValue;
            if (!res)
            {
                res = !needMonthlyIncomeValue;
                if (!res)
                {
                    res = needTermValue;
                    if (res)
                    {
                        decimal amount = GetPostedValueDecimal(tbAmount.ID);
                        res = amount > 0;
                    }
                }
            }
            advCalc.NeedCreditLine = res;
            GetMontlyFeeValues();
            advCalc.UpdateFeeSelection(GetSelectedFee());
            advCalc.Save(productsOrder);
            SaveMortgageData();
            SetCalculatorParameters();
            skipValidation = false;            
            BindData();
            SetPaymentPlanParameters();
            SetAmount();
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (isLateBinding) BindProducts();
            BindFees();
//            rbMontlyIncome.Attributes.Add(ONCLICK,String.Format(JSTENURECLICK,trFinalTermId));
            rbMontlyIncome.Attributes.Add(ONCLICK, JSTENURECLICK);
        }
        #endregion

        protected void rmMortgage_ItemClick(object sender, RadMenuEventArgs e)
        {
            if (e.Item.Value == "DownloadParameters")
            {
                DownloadParameters();
            }
            if (e.Item.Value == "ScenarioColumn")
            {
                bool b = IsScenarioColumnVisible;
                IsScenarioColumnVisible = !b;
            }
            if (e.Item.Value == "ApplicationRate")
            {
                bool b = IsAppRatesUsed;
                IsAppRatesUsed = !b;
            }
            BindData();
        }

    }
}
