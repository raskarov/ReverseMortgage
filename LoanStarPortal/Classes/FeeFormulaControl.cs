using System;
using System.Collections;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Telerik.WebControls;

namespace LoanStar.Common
{
    public abstract class FeeFormulaControl
    {

        #region constants
        private const int TOTALJSFORMULAID = 4;
        protected const string HEADERROWCSS = "editFormLabel";
        protected const string DATAROWCSS = "feeformuladatatr";
        protected const string HEADERTDCSS = "feeformulaheadertd";
        protected const string LABELTDCSS = "editFormLabel";
        protected const string CONTROLTDCSS = "feeformulactltd";
        protected const string POCLABEL = "POC Amount:";
        protected const string FINANCEDLABEL = "Financed Amount:";
        protected const string FINANCEDCONTROLID = "lblFinancedAmount";
        protected const string POCID = "tbPocAmount";
        protected const int RADTBWIDTH = 120;
        protected const int TBWIDTH = 146;
        protected const int DDLWIDTH = 150;
        protected const string OBJECTNAME = "Invoices";
        private const string DEFAULTROWCSS = "";
        private const string DEFAULTCELLCSS = "";
        private const string TEXTBOXFOCUSTLOSTJS = "FormulaAmountFocusLost(this,'{0}',{1})";
        protected const string TEXTBOXFIRSTADDITIONALROWFOCUSTLOSTJS = "FormulaAmountFocusLostOneRow(this,'{0}','{1}',{2})";
        protected const string DDLSELECTIONCHANGED = "SelectionChanged(this,'{0}');";
        protected const string ONBLUR = "onblur";
        protected const string ONCHANGEATTRIBUTE = "onchange";
        private const string LABELTDCLASS = "editFormLabel";
        protected const string APPLYTOLOAN = "Apply to loan";
        private readonly string[,] TotalCostDescription = {
                                                              {"", "lbl@ @Total price"}
                                                              , {"lbl@ @Mortgage Amount:", "rc@MortgageAmount@ "}
                                                              , {"lbl@ @Deed Amount:", "rc@DeedAmount@ "}
                                                              , {"lbl@ @POC Amount:", "rc@POCAmount@ "}
                                                              , {"lbl@ @Lender Credit:", "rc@LenderCreditAmount@ "}
                                                              , {"lbl@ @Third Party paid:", "rc@ThirdPartyPaidAmount@ "}
                                                              , {"lbl@ @Financed Amount:", "lbl@FinancedAmount@ "}
                                                          };

        protected readonly ArrayList MaxValues;
        protected readonly ArrayList RoundValues;
        #endregion

        #region fields
        protected IInvoiceData invoice;
        protected int Rows;
        protected int Columns;
        private short tabIndex = -1;
        #endregion

        #region properties
        public short TabIndex
        {
            get { return tabIndex; }
            set { tabIndex = value; }
        }
        protected virtual string LabelStyleCss
        {
            get { return LABELTDCLASS; }
        }
        #endregion

        #region constructor
        public FeeFormulaControl(IInvoiceData inv)
        {
            invoice = inv;
            MaxValues = new ArrayList();
            MaxValues.Add(new ListItem("Max claim", Invoice.MAXCLAIMID.ToString()));
            MaxValues.Add(new ListItem("Maximum Principal Limit", Invoice.MAXPRINCIPLELIMITID.ToString()));
            MaxValues.Add(new ListItem("Principal Limit", Invoice.PRINCIPLELIMITID.ToString()));
            RoundValues = new ArrayList();
            RoundValues.Add(new ListItem("Round up", Invoice.ROUNDUPID.ToString()));
            RoundValues.Add(new ListItem("Round nearest", Invoice.ROUNDNEARESTID.ToString()));
        }
        #endregion

        #region methods
        protected virtual void SetPrinciplaLimit(IInvoiceData inv)
        {
            if(inv is Invoice)
            {
                ((Invoice)inv).SetPrincipalLimit();
            }
        }

        public virtual ArrayList Build()
        {
            ArrayList rows = new ArrayList();
            for (int i=0; i < Rows; i++)
            {
                rows.Add(GetDataRow(i));
            }
            return rows;
        }
        protected virtual HtmlTableRow GetDataRow(int rowIndex)
        {
            HtmlTableRow row = new HtmlTableRow();
            string css = GetRowCss(rowIndex);
            if(!String.IsNullOrEmpty(css))
            {
                row.Attributes.Add("class", css);
            }
            for(int i=0; i < Columns; i++)
            {
                row.Cells.Add(GetDataCell(rowIndex,i));
            }
            return row;
        }
        protected virtual HtmlTableCell GetDataCell(int rowIndex, int columnIndex)
        {
            HtmlTableCell td = new HtmlTableCell();
            string css = GetCellClass(rowIndex, columnIndex);
            if(!String.IsNullOrEmpty(css))
            {
                td.Attributes.Add("class", css);
            }
            Control ctl = GetControl(rowIndex, columnIndex);
            if(ctl!=null)
            {
                td.Controls.Add(ctl);
                if(ctl is Label)
                {
                    td.Attributes.Add("class",LabelStyleCss);
                }
                else
                {
                    AddValidators(td, ctl);
                }
                //else
                //{
                //    td.Controls.Add(GetValidatorControl(ctl.ClientID));
                //}
            }
            return td;
        }
        protected virtual void AddValidators(HtmlTableCell td,Control ctl)
        {
            if(ctl.ID == "POCAmount")
            {
                td.Controls.Add(new LiteralControl("&nbsp;"));
                td.Controls.Add(GetCustomValidator(ctl.ID, "*", "ValidatePOCAmount"));
            }
        }
        private static CustomValidator GetCustomValidator(string controlId, string errMessage, string func)
        {
            CustomValidator v = new CustomValidator();
            v.ID = "cv" + controlId;
            v.ControlToValidate = controlId;
            v.ErrorMessage = errMessage;
            v.ValidateEmptyText = false;
            v.ClientValidationFunction = func;
            return v;
        }
        //private static Control GetValidatorControl(string ctlId)
        //{
        //    LiteralControl ctl = new LiteralControl("<span style='color:red;display:none'>*</span>");
        //    return (Control) ctl;
        //}

        protected virtual string GetRowCss(int rowIndex)
        {
            return DEFAULTROWCSS;
        }
        protected virtual string GetCellClass(int rowIndex, int columnIndex)
        {
            return DEFAULTCELLCSS;
        }
        public virtual void BindData(Control parent)
        {
        }
        protected virtual Control GetControl(int rowIndex, int columnIndex)
        {
            return null;
        }
        public virtual void SetInvoiceData(IInvoiceData inv, Control parent, ArrayList logs, int userId)
        {
        }
        protected virtual string GetTextBoxValue(Control parent, string ctlId)
        {
            string res = "";
            TextBox tb = (TextBox)parent.FindControl(ctlId);
            if (tb != null)
            {
                res = tb.Text;
            }
            return res;
        }
        protected virtual int GetDdlValue(Control parent, string ctlId)
        {
            int res = 0;
            DropDownList ddl = (DropDownList)parent.FindControl(ctlId);
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
        protected virtual decimal GetRadTextBoxValue(Control parent, string ctlId)
        {
            decimal res = 0;
            RadNumericTextBox tb = (RadNumericTextBox)parent.FindControl(ctlId);
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
        protected virtual void SetOtherAmountsAndTotal(Control parent, string total, string js)
        {
            SetRadTextBoxValue(parent, "POCAmount", invoice.POCAmount.ToString(), js);
            SetRadTextBoxValue(parent, "LenderCreditAmount", invoice.LenderCreditAmount.ToString(), js);
            SetRadTextBoxValue(parent, "ThirdPartyPaidAmount", invoice.ThirdPartyPaidAmount.ToString(), js);
            SetLabelValue(parent, "FinancedAmount", total);
        }

        protected virtual void SetPocAndTotal(Control parent, string total, string js)
        {
            SetRadTextBoxValue(parent, "POCAmount",invoice.POCAmount.ToString(),js);
            SetLabelValue(parent, "FinancedAmount", total);
        }
        protected virtual void SetTextBoxValue(Control parent, string ctlId, string value)
        {
            TextBox tb = (TextBox)parent.FindControl(ctlId);
            if (tb != null)
            {
                tb.Text = value;
            }
        }
        protected virtual void SetLabelValue(Control parent, string ctlId, string value)
        {
            Label lbl = (Label)parent.FindControl(ctlId);
            if(lbl!=null)
            {
                lbl.Text = value;
            }
        }
        protected virtual void SetRadTextBoxValue(Control parent, string ctlId, string value, string js)
        {
            RadNumericTextBox tb = (RadNumericTextBox) parent.FindControl(ctlId);
            if (tb != null)
            {
                if (!String.IsNullOrEmpty(value)&&value!="0")
                {
                    tb.Text = value;
                }
                else
                {
                    tb.Text = "";
                }
                if (!String.IsNullOrEmpty(js))
                {
                    tb.Attributes.Add(ONBLUR, js);
                }
            }
        }
        protected virtual void SetDropDownValue(Control parent, string ctlId, ArrayList values, string value, string js)
        {
            DropDownList ddl = (DropDownList) parent.FindControl(ctlId);
            if(ddl!=null)
            {
                for (int i=0; i<values.Count;i++)
                {
                    ddl.Items.Add((ListItem)values[i]);
                }
                ListItem li = ddl.Items.FindByValue(value);
                if (li != null) li.Selected = true;
                ddl.Attributes.Add(ONCHANGEATTRIBUTE,js);
            }
        }

        protected virtual Control ParseControl(string controlDescription)
        {
            Control ctl = null;
            if(!String.IsNullOrEmpty(controlDescription))
            {
                string[] data = controlDescription.Split('@');

                ctl = BuildControl(data[0].Trim(), data[1].Trim(), data[2].Trim());
            }
            return ctl;
        }
        protected virtual Control BuildControl(string ctlType, string ctlId, string ctlValue)
        {
            Control ctl = null;
            switch (ctlType)
            {
                case "lbl" :
                    ctl = new Label();
                    if(!String.IsNullOrEmpty(ctlValue))
                    {
                        ((Label) ctl).Text = ctlValue;
                    }
                    break;
                case "tb":
                    ctl = new TextBox();
                    ((TextBox)ctl).Width = Unit.Pixel(TBWIDTH);
                    ((TextBox) ctl).MaxLength = 100;
                    if (!String.IsNullOrEmpty(ctlValue))
                    {
                        ((TextBox)ctl).Text = ctlValue;
                    }
                    if(tabIndex>0)
                    {
                        ((TextBox) ctl).TabIndex = tabIndex;
                        tabIndex++;
                    }
                    break;
                case "sel":
                    ctl = new DropDownList();
                    ((DropDownList)ctl).Width = Unit.Pixel(DDLWIDTH);
                    ((DropDownList) ctl).AutoPostBack = false;
                    if (tabIndex > 0)
                    {
                        ((DropDownList)ctl).TabIndex = tabIndex;
                        tabIndex++;
                    }
                    break;
                case "rc":
                    ctl = new RadNumericTextBox();
                    ((RadNumericTextBox)ctl).Skin = "Default";
                    ((RadNumericTextBox)ctl).Width = Unit.Pixel(RADTBWIDTH);
                    ((RadNumericTextBox)ctl).Type = NumericType.Currency;
                    ((RadNumericTextBox)ctl).NumberFormat.DecimalDigits = 2;
                    ((RadNumericTextBox)ctl).MinValue = 0;
                    if (tabIndex > 0)
                    {
                        ((RadNumericTextBox)ctl).TabIndex = tabIndex;
                        tabIndex++;
                    }
                    break;
                case "rn":
                    ctl = new RadNumericTextBox();
                    ((RadNumericTextBox)ctl).Skin = "Default";
                    ((RadNumericTextBox)ctl).Width = Unit.Pixel(RADTBWIDTH);
                    ((RadNumericTextBox)ctl).Type = NumericType.Number;
                    ((RadNumericTextBox)ctl).MinValue = 1;
                    ((RadNumericTextBox)ctl).MaxValue = 100;
                    ((RadNumericTextBox) ctl).NumberFormat.DecimalDigits = 0;
                    if (tabIndex > 0)
                    {
                        ((RadNumericTextBox)ctl).TabIndex = tabIndex;
                        tabIndex++;
                    }
                    break;
                case "rp":
                    ctl = new RadNumericTextBox();
                    ((RadNumericTextBox)ctl).Skin = "Default";
                    ((RadNumericTextBox)ctl).Width = Unit.Pixel(RADTBWIDTH);
                    ((RadNumericTextBox)ctl).Type = NumericType.Percent;
                    ((RadNumericTextBox)ctl).MinValue = 0;
                    ((RadNumericTextBox)ctl).MaxValue = 100;
                    ((RadNumericTextBox)ctl).NumberFormat.DecimalDigits = 2;
                    if (tabIndex > 0)
                    {
                        ((RadNumericTextBox)ctl).TabIndex = tabIndex;
                        tabIndex++;
                    }
                    break;
            }
            if(ctl!=null)
            {
                if(!String.IsNullOrEmpty(ctlId))
                {
                    ctl.ID = ctlId;
                }
            }
            return ctl;
        }
        protected virtual Control GetCellControl(string[,] description, int rowIndex, int columnIndex)
        {
            Control ctl = null;
            if (rowIndex < Rows && columnIndex < Columns)
            {
                ctl = ParseControl(description[rowIndex, columnIndex]);
            }
            return ctl;
        }
        protected string GetAmountFocusLostScript(int formulaId)
        {
            return String.Format(TEXTBOXFOCUSTLOSTJS, GetArrayForFocusLost(), formulaId);
        }
        protected virtual string GetArrayForFocusLost()
        {
            return "MortgageAmount_text,DeedAmount_text,POCAmount_text,LenderCreditAmount_text,ThirdPartyPaidAmount_text";
        }
        protected virtual Control GetTotalControl(int rowIndex, int columnIndex)
        {
            return GetCellControl(TotalCostDescription, rowIndex, columnIndex);
        }
        protected virtual void BindTotalCostData(Control parent)
        {
            string js = GetAmountFocusLostScript(TOTALJSFORMULAID);
            string m = invoice.MortgageAmount.ToString();
            string d = invoice.DeedAmount.ToString();
            if (invoice is ClosingCostProfile.ClosingCostProfileDetail)
            {
                m = invoice.GetFormulaData("MortgageAmount");
                d = invoice.GetFormulaData("DeedAmount");
            }
            SetRadTextBoxValue(parent, "MortgageAmount", m, js);
            SetRadTextBoxValue(parent, "DeedAmount", d, js);
            SetOtherAmountsAndTotal(parent, invoice.AmountFinanced.ToString("C"), js);
        }
        protected virtual void SetInvoiceDataForPricePerUnit(IInvoiceData inv, Control parent, IList logs, int userId)
        {
            decimal pu = GetRadTextBoxValue(parent, "PricePerUnit");
            decimal u = GetRadTextBoxValue(parent, "Units");
            int maxSel = GetDdlValue(parent, "MaxSelect");
            int roundSel = GetDdlValue(parent, "RoundSelect");
            inv.SetFormulaData("PricePerUnit", pu.ToString());
            inv.SetFormulaData("Units", u.ToString());
            inv.SetFormulaData("MaxSelect", maxSel.ToString());
            inv.SetFormulaData("RoundSelect", roundSel.ToString());
            decimal a = inv.CalculateTotal();
            if (inv.Amount != a)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".Amount",
                                         inv.Amount.ToString(),
                                         a.ToString(), userId));
                inv.Amount = a;
            }
            GetOtherAmountsData(inv, parent, logs, userId);
            if(maxSel==Invoice.PRINCIPLELIMITID)
            {
                SetPrinciplaLimit(inv);
            }
        }
        protected virtual void SetInvoiceDataPercentage(IInvoiceData inv, Control parent, IList logs, int userId)
        {
            decimal pr = GetRadTextBoxValue(parent, "Percentage");
            int maxSel = GetDdlValue(parent, "MaxSelect");
            inv.SetFormulaData("Percentage", pr.ToString());
            inv.SetFormulaData("MaxSelect", maxSel.ToString());
            decimal a = inv.CalculateTotal();
            if (inv.Amount != a)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".Amount",
                                         inv.Amount.ToString(),
                                         a.ToString(), userId));
                inv.Amount = a;
            }
            GetOtherAmountsData(inv, parent, logs, userId);
            if (maxSel == Invoice.PRINCIPLELIMITID)
            {
                SetPrinciplaLimit(inv);
            }
        }
        protected virtual void SetInvoiceDataForTotal(IInvoiceData inv, Control parent, IList logs, int userId)
        {
            decimal d = GetRadTextBoxValue(parent, "DeedAmount");
            if (inv.DeedAmount != d)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".DeedAmount",
                                         inv.DeedAmount.ToString(),
                                         d.ToString(), userId));
                inv.DeedAmount = d;
            }
            if (inv is ClosingCostProfile.ClosingCostProfileDetail)
            {
                inv.SetFormulaData("DeedAmount", d.ToString());
            }
            decimal m = GetRadTextBoxValue(parent, "MortgageAmount");
            if (inv.MortgageAmount != m)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".MortgageAmount",
                                         inv.MortgageAmount.ToString(),
                                         m.ToString(), userId));
                inv.MortgageAmount = m;
            }
            if (inv is ClosingCostProfile.ClosingCostProfileDetail)
            {
                inv.SetFormulaData("MortgageAmount", d.ToString());
            }
            GetOtherAmountsData(inv, parent, logs, userId);
        }
        protected virtual void GetOtherAmountsData(IInvoiceData inv, Control parent, IList logs, int userId)
        {
            decimal p = GetRadTextBoxValue(parent, "POCAmount");
            if (inv.POCAmount != p)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".POCAmount",
                                         inv.POCAmount.ToString(),
                                         p.ToString(), userId));
                inv.POCAmount = p;
            }
            decimal l = GetRadTextBoxValue(parent, "LenderCreditAmount");
            if (inv.LenderCreditAmount != l)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".LenderCreditAmount",
                                         inv.LenderCreditAmount.ToString(),
                                        l.ToString(), userId));
                inv.LenderCreditAmount = l;
            }
            decimal t = GetRadTextBoxValue(parent, "ThirdPartyPaidAmount");
            if (inv.ThirdPartyPaidAmount != t)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".ThirdPartyPaidAmount",
                                         inv.LenderCreditAmount.ToString(),
                                        l.ToString(), userId));
                inv.ThirdPartyPaidAmount = t;
            }
        }

        #endregion

    }
    public class StampsFormulaControl : FeeFormulaControl
    {
        #region constants
        private const string PERUNITJS = "CalculatePerUnit(this,'{0}',{1},{2});";
        private const string PERCENTJS = "CalculatePercentage(this,'{0}',{1},{2});";
        private readonly string[,] PricePerUnitDescription = {
                                                              {"lbl@ @Jurisdiction uses (select one):", "sel@MaxSelect@ ",""}
                                                              , {"lbl@ @Jurisdiction uses (select one):", "sel@RoundSelect@ ",""}
                                                              , {"lbl@ @Price per unit", "lbl@ @Unit","lbl@ @Total"}
                                                              , {"rc@PricePerUnit@ ", "rc@Units@ ","lbl@Total@"}
                                                              , {"", "lbl@ @POC Amount:", "rc@POCAmount@ "}
                                                              , {"", "lbl@ @Lender Credit:", "rc@LenderCreditAmount@ "}
                                                              , {"", "lbl@ @Third Party paid:", "rc@ThirdPartyPaidAmount@ "}
                                                              , {"", "lbl@ @Financed Amount:", "lbl@FinancedAmount@ "}

                                                          };
        private readonly string[,] PercentageDescription = {
                                                              {"lbl@ @Jurisdiction uses (select one):", "sel@MaxSelect@ "}
                                                              , {"lbl@ @Percentage", "lbl@ @Total"}
                                                              , {"rp@Percentage@ ", "lbl@Total@"}
                                                              , {"lbl@ @POC Amount:", "rc@POCAmount@ "}
                                                              , {"lbl@ @Lender Credit:", "rc@LenderCreditAmount@ "}
                                                              , {"lbl@ @Third Party paid:", "rc@ThirdPartyPaidAmount@ "}
                                                              , {"lbl@ @Financed Amount:", "lbl@FinancedAmount@ "}

                                                          };

        #endregion

        #region constructor
        public StampsFormulaControl(IInvoiceData inv)
            : base(inv)
        {
            switch (inv.FormulaId)
            {
                case Invoice.STAMPSTOTALCOSTFORMULAID:
                    Rows = 7;
                    Columns = 2;
                    break;
                case Invoice.STAMPSPRICEPERUNITFORMULAID:
                    Rows = 8;
                    Columns = 3;
                    break;
                case Invoice.STAMPSPERCENTAGEFORMULAID:
                    Rows = 7;
                    Columns = 2;
                    break;
                default:
                    Rows = 5;
                    Columns = 2;
                    break;
            }
        }
        #endregion

        #region methods
        protected override Control GetControl(int rowIndex, int columnIndex)
        {
            Control ctl = null;
            switch (invoice.FormulaId)
            {
                case Invoice.STAMPSTOTALCOSTFORMULAID:
                    ctl = GetTotalControl(rowIndex, columnIndex);
                    break;
                case Invoice.STAMPSPRICEPERUNITFORMULAID:
                    ctl = GetCellControl(PricePerUnitDescription, rowIndex, columnIndex);
                    break;
                case Invoice.STAMPSPERCENTAGEFORMULAID:
                    ctl = GetCellControl(PercentageDescription, rowIndex, columnIndex);
                    break;
            }
            return ctl;
        }
        public override void BindData(Control parent)
        {
            switch (invoice.FormulaId)
            {
                case Invoice.STAMPSTOTALCOSTFORMULAID:
                    BindTotalCostData(parent);
                    break;
                case Invoice.STAMPSPRICEPERUNITFORMULAID:
                    BindPricePerUnitData(parent);
                    break;
                case Invoice.STAMPSPERCENTAGEFORMULAID:
                    BindPercentageData(parent);
                    break;
            }
        }
        public override void SetInvoiceData(IInvoiceData inv, Control parent, ArrayList logs, int userId)
        {
            switch (invoice.FormulaId)
            {
                case Invoice.STAMPSTOTALCOSTFORMULAID:
                    SetInvoiceDataForTotal(inv, parent, logs, userId);
                    break;
                case Invoice.STAMPSPRICEPERUNITFORMULAID:
                    SetInvoiceDataForPricePerUnit(inv, parent, logs, userId);
                    break;
                case Invoice.STAMPSPERCENTAGEFORMULAID:
                    SetInvoiceDataPercentage(inv, parent, logs, userId);
                    break;
            }
        }
        protected virtual void BindPricePerUnitData(Control parent)
        {
            string js = "";
            string total = APPLYTOLOAN;
            string financedAmount = APPLYTOLOAN;
            if(invoice is Invoice)
            {
                js = String.Format(PERUNITJS, GetArrayForPricePerUnit(), invoice.MaxClaimAmount, invoice.PrincipalLimit);
                total = invoice.CalculateTotal().ToString("C");
                financedAmount = invoice.AmountFinanced.ToString("C");
            }
            SetDropDownValue(parent, "MaxSelect", MaxValues, invoice.GetFormulaData("MaxSelect"), js);
            SetDropDownValue(parent, "RoundSelect", RoundValues, invoice.GetFormulaData("RoundSelect"), js);
            SetRadTextBoxValue(parent, "PricePerUnit", invoice.GetFormulaData("PricePerUnit"), js);
            SetRadTextBoxValue(parent, "Units", invoice.GetFormulaData("Units"), js);
            SetLabelValue(parent, "Total", total );
            SetOtherAmountsAndTotal(parent, financedAmount, js);
        }
        protected virtual void BindPercentageData(Control parent)
        {
            string js = "";
            string total = APPLYTOLOAN;
            string financedAmount = APPLYTOLOAN;
            if (invoice is Invoice)
            {
                js = String.Format(PERCENTJS, GetArrayForPercentage(), invoice.MaxClaimAmount, invoice.PrincipalLimit);
                total = invoice.CalculateTotal().ToString("C");
                financedAmount = invoice.AmountFinanced.ToString("C");
            }
            SetDropDownValue(parent, "MaxSelect", MaxValues, invoice.GetFormulaData("MaxSelect"), js);
            SetRadTextBoxValue(parent, "Percentage", invoice.GetFormulaData("Percentage"), js);
            SetLabelValue(parent, "Total", total);
            SetOtherAmountsAndTotal(parent, financedAmount, js);
        }
        #endregion
        private static string GetArrayForPricePerUnit()
        {
            return "MaxSelect,RoundSelect&PricePerUnit_text,Units_text,POCAmount_text,LenderCreditAmount_text,ThirdPartyPaidAmount_text&Total,FinancedAmount";
        }
        private static string GetArrayForPercentage()
        {
            return "MaxSelect&Percentage_text,POCAmount_text,LenderCreditAmount_text,ThirdPartyPaidAmount_text&Total,FinancedAmount";
        }
    }
    public class CityCountyStampsFormulaControl : StampsFormulaControl
    {

        #region constructor
        public CityCountyStampsFormulaControl(IInvoiceData inv)
            : base(inv)
        {
        }
        #endregion
    }
    public class StateStampsFormulaControl : StampsFormulaControl
    {
        #region constructor
        public StateStampsFormulaControl(IInvoiceData inv)
            : base(inv)
        {
        }
        #endregion
        
    }

    public class RecordingFeeFormulaControl : FeeFormulaControl
    {
        #region constants

        private readonly string[,] TotalCostDescription = {
                                                              {"", "lbl@ @Total price"}
                                                              , {"lbl@ @Mortgage Amount:", "rc@MortgageAmount@ "}
                                                              , {"lbl@ @Deed Amount:", "rc@DeedAmount@ "}
                                                              , {"lbl@ @Release Amount:", "rc@ReleaseAmount@ "}
                                                              , {"lbl@ @POC Amount:", "rc@POCAmount@ "}
                                                              , {"lbl@ @Lender Credit:", "rc@LenderCreditAmount@ "}
                                                              , {"lbl@ @Third Party paid:", "rc@ThirdPartyPaidAmount@ "}
                                                              , {"lbl@ @Financed Amount:", "lbl@FinancedAmount@ "}
                                                          };
        private readonly string[,] FirstAdditionalPageDescription = {
                                                              {"", "lbl@ @First page price","lbl@ @Additional page price","lbl@ @Total pages","lbl@ @Total"}
                                                              , {"lbl@ @Mortgage Amount:", "rc@MortgageFirstPage@ ","rc@MortgageAdditionalPage@ ","rn@MortgageTotalPages@ ","lbl@MortgageTotal@ "}
                                                              , {"lbl@ @Deed Amount:", "rc@DeedFirstPage@ ","rc@DeedAdditionalPage@ ","rn@DeedTotalPages@ ","lbl@DeedTotal@ "}
                                                              , {"lbl@ @Release Amount:", "rc@ReleaseFirstPage@ ","rc@ReleaseAdditionalPage@ ","rn@ReleaseTotalPages@ ","lbl@ReleaseTotal@ "}
                                                              , {"","","","lbl@ @POC Amount:", "rc@POCAmount@ "}
                                                              , {"","","", "lbl@ @Lender Credit:", "rc@LenderCreditAmount@ "}
                                                              , {"","","", "lbl@ @Third Party paid:", "rc@ThirdPartyPaidAmount@ "}
                                                              , {"","","","lbl@ @Financed Amount:", "lbl@FinancedAmount@ "}
                                                          };

        private readonly string[,] SamePageDescription = {
                                                              {"", "tb@Pages@Pages","tb@PricePerPageUnit@PricePerPage/Unit","lbl@ @Total"}
                                                              , {"lbl@ @Mortgage Amount:", "rn@MortgagePages@ ","rc@MortgagePricePerPageUnit@ ","lbl@MortgageTotal@ "}
                                                              , {"lbl@ @Deed  Amount:", "rn@DeedPages@ ","rc@DeedPricePerPageUnit@ ","lbl@DeedTotal@ "}
                                                              , {"lbl@ @Release Amount:", "rn@ReleasePages@ ","rc@ReleasePricePerPageUnit@ ","lbl@ReleaseTotal@ "}
                                                              , {"","","lbl@ @POC Amount:", "rc@POCAmount@ "}
                                                              , {"","", "lbl@ @Lender Credit:", "rc@LenderCreditAmount@ "}
                                                              , {"","", "lbl@ @Third Party paid:", "rc@ThirdPartyPaidAmount@ "}
                                                              , {"","","lbl@ @Financed Amount:", "lbl@FinancedAmount@ "}
                                                          };

        #endregion

        #region constructor
        public RecordingFeeFormulaControl(IInvoiceData inv)
            : base(inv)
        {
            if (inv.FormulaId < 0) inv.FormulaId = Invoice.RECORDINGFEETOTALCOSTFORMULAID;
            switch (inv.FormulaId)
            {
                case Invoice.RECORDINGFEETOTALCOSTFORMULAID:
                    Rows = 8;
                    Columns = 2;
                    break;
                case Invoice.RECORDINGFEEFIRSTADDITIONALPAGEFORMULAID:
                    Rows = 8;
                    Columns = 5;
                    break;
                case Invoice.RECORDINGFEESAMEPRICEPAGEFORMULAID:
                    Rows = 8;
                    Columns = 4;
                    break;
                default:
                    Rows = 8;
                    Columns = 2;
                    break;
            }
        }
        #endregion

        #region overriden methods
        public override void BindData(Control parent)
        {
            switch (invoice.FormulaId)
            {
                case Invoice.RECORDINGFEETOTALCOSTFORMULAID:
                    BindTotalPriceData(parent);
                    break;
                case Invoice.RECORDINGFEEFIRSTADDITIONALPAGEFORMULAID:
                    BindFirstAdditionalPageData(parent);
                    break;
                case Invoice.RECORDINGFEESAMEPRICEPAGEFORMULAID:
                    BindSamePageData(parent);
                    break;
            }
        }
        protected override string GetArrayForFocusLost()
        {
            string res = "";
            switch (invoice.FormulaId)
            {
                case Invoice.RECORDINGFEETOTALCOSTFORMULAID:
                    res = GetJSTotalForTotal();
                    break;
                case Invoice.RECORDINGFEEFIRSTADDITIONALPAGEFORMULAID:
                    res = GetJSTotalForFirstAdditionalPage();
                    break;
                case Invoice.RECORDINGFEESAMEPRICEPAGEFORMULAID:
                    res = GetJSTotalForSamePage();
                    break;
            }
            return res;
        }
        protected override Control GetControl(int rowIndex, int columnIndex)
        {
            Control ctl = null;
            switch (invoice.FormulaId)
            {
                case Invoice.RECORDINGFEETOTALCOSTFORMULAID:
                    ctl = GetCellControl(TotalCostDescription, rowIndex, columnIndex);
                    break;
                case Invoice.RECORDINGFEEFIRSTADDITIONALPAGEFORMULAID:
                    ctl = GetCellControl(FirstAdditionalPageDescription, rowIndex, columnIndex);
                    break;
                case Invoice.RECORDINGFEESAMEPRICEPAGEFORMULAID:
                    ctl = GetCellControl(SamePageDescription, rowIndex, columnIndex);
                    break;
            }
            return ctl;
        }
        public override void SetInvoiceData(IInvoiceData inv, Control parent, ArrayList logs, int userId)
        {
            switch (invoice.FormulaId)
            {
                case Invoice.RECORDINGFEETOTALCOSTFORMULAID:
                    SetInvoiceDataForTotal(inv, parent, logs, userId);
                    break;
                case Invoice.RECORDINGFEEFIRSTADDITIONALPAGEFORMULAID:
                    SetInvoiceDataForFirstAdditionalPage(inv, parent, logs, userId);
                    break;
                case Invoice.RECORDINGFEESAMEPRICEPAGEFORMULAID:
                    SetInvoiceDataForSamePage(inv, parent, logs, userId);
                    break;
            }
        }
        #endregion

        #region private methods

        #region Total costs
        private static string GetJSTotalForTotal()
        {
            return "MortgageAmount_text,DeedAmount_text,ReleaseAmount_text,POCAmount_text,LenderCreditAmount_text,ThirdPartyPaidAmount_text";
        }
        private void BindTotalPriceData(Control parent)
        {
            string js = GetAmountFocusLostScript(Invoice.RECORDINGFEETOTALCOSTFORMULAID);
            string m = invoice.MortgageAmount.ToString();
            string d = invoice.DeedAmount.ToString();
            string r = invoice.ReleaseAmount.ToString();
            string af = invoice.AmountFinanced.ToString("C");
            if(invoice is ClosingCostProfile.ClosingCostProfileDetail)
            {
                m = invoice.GetFormulaData("MortgageAmount");
                d = invoice.GetFormulaData("DeedAmount");
                r = invoice.GetFormulaData("ReleaseAmount");
                decimal af_ = 0;
                if(!String.IsNullOrEmpty(m))
                {
                    af_ += Convert.ToDecimal(m);
                }
                if (!String.IsNullOrEmpty(d))
                {
                    af_ += Convert.ToDecimal(d);
                }
                if (!String.IsNullOrEmpty(r))
                {
                    af_ += Convert.ToDecimal(r);
                }
                af_ -= (invoice.POCAmount + invoice.LenderCreditAmount + invoice.ThirdPartyPaidAmount);
                af = af_.ToString("C");
                    
            }
            SetRadTextBoxValue(parent, "MortgageAmount", m, js);
            SetRadTextBoxValue(parent, "DeedAmount", d, js);
            SetRadTextBoxValue(parent, "ReleaseAmount", r, js);
            SetOtherAmountsAndTotal(parent, af, js);
        }
        protected override void SetInvoiceDataForTotal(IInvoiceData inv, Control parent, IList logs, int userId)
        {
            decimal d = GetRadTextBoxValue(parent, "DeedAmount");
            if (inv.DeedAmount != d)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".DeedAmount",
                                         inv.DeedAmount.ToString(),
                                         d.ToString(), userId));
                inv.DeedAmount = d;
            }
            if( inv is ClosingCostProfile.ClosingCostProfileDetail)
            {
                inv.SetFormulaData("DeedAmount",d.ToString());
            }
            decimal m = GetRadTextBoxValue(parent, "MortgageAmount");
            if (inv.MortgageAmount != m)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".MortgageAmount",
                                         inv.MortgageAmount.ToString(),
                                         m.ToString(), userId));
                inv.MortgageAmount = m;
            }
            if (inv is ClosingCostProfile.ClosingCostProfileDetail)
            {
                inv.SetFormulaData("MortgageAmount", m.ToString());
            }
            decimal r = GetRadTextBoxValue(parent, "ReleaseAmount");
            if (inv.ReleaseAmount != r)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".ReleaseAmount",
                                         inv.ReleaseAmount.ToString(),
                                         r.ToString(), userId));
                inv.ReleaseAmount = r;
            }
            if (inv is ClosingCostProfile.ClosingCostProfileDetail)
            {
                inv.SetFormulaData("ReleaseAmount", r.ToString());
            }
            GetOtherAmountsData(inv, parent, logs, userId);
        }
        #endregion

        #region First/Additional page
        private static string GetJSTotalForFirstAdditionalPage()
        {
            return "MortgageFirstPage_text,MortgageAdditionalPage_text,MortgageTotalPages_text,DeedFirstPage_text,DeedAdditionalPage_text,DeedTotalPages_text,ReleaseFirstPage_text,ReleaseAdditionalPage_text,ReleaseTotalPages_text,POCAmount_text,LenderCreditAmount_text,ThirdPartyPaidAmount_text";
        }
        private static string GetJSTotalForFirstAdditionalPageRow(string name)
        {
            return name + "FirstPage_text," + name + "AdditionalPage_text," + name + "TotalPages_text,";
        }
        private static decimal FirstAdditionalPageCalculation(decimal d1, decimal d2, int d3)
        {
            return d1 + d2 * (d3 - 1);
        }
        private decimal GetFirstAdditionalPageValues(IInvoiceData inv, Control parent, string dataName)
        {
            decimal d1 = GetRadTextBoxValue(parent, dataName + "FirstPage");
            decimal d2 = GetRadTextBoxValue(parent, dataName + "AdditionalPage");
            int d3 = (int)GetRadTextBoxValue(parent, dataName + "TotalPages");
            inv.SetFormulaData(dataName + "FirstPage", d1.ToString());
            inv.SetFormulaData(dataName + "AdditionalPage", d2.ToString());
            inv.SetFormulaData(dataName + "TotalPages", d3.ToString());
            return FirstAdditionalPageCalculation(d1, d2, d3);
        }
        private void SetInvoiceDataForFirstAdditionalPage(IInvoiceData inv, Control parent, IList logs, int userId)
        {
            decimal d = GetFirstAdditionalPageValues(inv, parent, "Deed");
            if (inv.DeedAmount != d)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".DeedAmount",
                                         inv.DeedAmount.ToString(),
                                         d.ToString(), userId));
                inv.DeedAmount = d;
            }
            decimal m = GetFirstAdditionalPageValues(inv, parent, "Mortgage");
            if (inv.MortgageAmount != m)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".MortgageAmount",
                                         inv.MortgageAmount.ToString(),
                                         m.ToString(), userId));
                inv.MortgageAmount = m;
            }
            decimal r = GetFirstAdditionalPageValues(inv, parent, "Release");
            if (inv.ReleaseAmount != r)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".ReleaseAmount",
                                         inv.ReleaseAmount.ToString(),
                                         r.ToString(), userId));
                inv.ReleaseAmount = r;
            }
            GetOtherAmountsData(inv, parent, logs, userId);
        }
        private void SetFirstAdditionalPageValues(Control parent, string dataName, decimal totalAmount, string js)
        {
            SetRadTextBoxValue(parent, dataName + "FirstPage", invoice.GetFormulaData(dataName + "FirstPage"), js);
            SetRadTextBoxValue(parent, dataName + "AdditionalPage", invoice.GetFormulaData(dataName + "AdditionalPage"), js);
            SetRadTextBoxValue(parent, dataName + "TotalPages", invoice.GetFormulaData(dataName + "TotalPages"), js);
            SetLabelValue(parent, dataName + "Total", totalAmount.ToString("C"));
        }
        private void BindFirstAdditionalPageData(Control parent)
        {
            string js = GetAmountFocusLostScript(Invoice.RECORDINGFEEFIRSTADDITIONALPAGEFORMULAID);
            SetFirstAdditionalPageValues(parent, "Mortgage", invoice.MortgageAmount, js + ";" + String.Format(TEXTBOXFIRSTADDITIONALROWFOCUSTLOSTJS, GetJSTotalForFirstAdditionalPageRow("Mortgage"), "MortgageTotal", Invoice.RECORDINGFEEFIRSTADDITIONALPAGEFORMULAID));
            SetFirstAdditionalPageValues(parent, "Deed", invoice.DeedAmount, js + ";" + String.Format(TEXTBOXFIRSTADDITIONALROWFOCUSTLOSTJS, GetJSTotalForFirstAdditionalPageRow("Deed"), "DeedTotal", Invoice.RECORDINGFEEFIRSTADDITIONALPAGEFORMULAID));
            SetFirstAdditionalPageValues(parent, "Release", invoice.ReleaseAmount, js + ";" + String.Format(TEXTBOXFIRSTADDITIONALROWFOCUSTLOSTJS, GetJSTotalForFirstAdditionalPageRow("Release"), "ReleaseTotal", Invoice.RECORDINGFEEFIRSTADDITIONALPAGEFORMULAID));
            SetOtherAmountsAndTotal(parent, invoice.AmountFinanced.ToString("C"), js);
        }
        #endregion

        #region Same page
        private static string GetJSTotalForSamePage()
        {

            return "MortgagePages_text,MortgagePricePerPageUnit_text,DeedPages_text,DeedPricePerPageUnit_text,ReleasePages_text,ReleasePricePerPageUnit_text,POCAmount_text,LenderCreditAmount_text,ThirdPartyPaidAmount_text";
        }
        private static string GetJSTotalForSamePageRow(string name)
        {
            return name + "Pages_text," + name + "PricePerPageUnit_text,";
        }

        private static decimal FirstSamePageCalculation(int d1, decimal d2)
        {
            return d1 * d2;
        }
/*
        private decimal GetTotalForSamePage()
        {
            return invoice.MortgageAmount + invoice.DeedAmount + invoice.ReleaseAmount - invoice.POCAmount;
        }
*/
        private void SetSamePageValues(Control parent, string dataName, decimal totalAmount,string js)
        {
            SetRadTextBoxValue(parent, dataName + "Pages", invoice.GetFormulaData(dataName + "Pages"), js);
            SetRadTextBoxValue(parent, dataName + "PricePerPageUnit", invoice.GetFormulaData(dataName + "PricePerPageUnit"), js);
            SetRadTextBoxValue(parent, dataName + "TotalPages", invoice.GetFormulaData(dataName + "TotalPages"), js);
            SetLabelValue(parent, dataName + "Total", totalAmount.ToString("C"));
        }
        private void BindSamePageData(Control parent)
        {
            string js = GetAmountFocusLostScript(Invoice.RECORDINGFEESAMEPRICEPAGEFORMULAID);
            SetSamePageValues(parent, "Mortgage", invoice.MortgageAmount, js + ";" + String.Format(TEXTBOXFIRSTADDITIONALROWFOCUSTLOSTJS, GetJSTotalForSamePageRow("Mortgage"), "MortgageTotal", Invoice.RECORDINGFEESAMEPRICEPAGEFORMULAID));
            SetSamePageValues(parent, "Deed", invoice.DeedAmount, js + ";" + String.Format(TEXTBOXFIRSTADDITIONALROWFOCUSTLOSTJS, GetJSTotalForSamePageRow("Deed"), "DeedTotal", Invoice.RECORDINGFEESAMEPRICEPAGEFORMULAID));
            SetSamePageValues(parent, "Release", invoice.ReleaseAmount, js + ";" + String.Format(TEXTBOXFIRSTADDITIONALROWFOCUSTLOSTJS, GetJSTotalForSamePageRow("Release"), "ReleaseTotal", Invoice.RECORDINGFEESAMEPRICEPAGEFORMULAID));
            string s = invoice.GetFormulaData("Pages");
            if(!String.IsNullOrEmpty(s))
            {
                SetTextBoxValue(parent, "Pages", s);
            }
            s = invoice.GetFormulaData("PricePerPageUnit");
            if (!String.IsNullOrEmpty(s))
            {
                SetTextBoxValue(parent, "PricePerPageUnit", s);
            }
            SetOtherAmountsAndTotal(parent, invoice.AmountFinanced.ToString("C"), js);
        }
        private decimal GetSamePageValues(IInvoiceData inv, Control parent, string dataName)
        {
            int d1 = (int)GetRadTextBoxValue(parent, dataName + "Pages");
            decimal d2 = GetRadTextBoxValue(parent, dataName + "PricePerPageUnit");
            inv.SetFormulaData(dataName + "Pages", d1.ToString());
            inv.SetFormulaData(dataName + "PricePerPageUnit", d2.ToString());
            return FirstSamePageCalculation(d1, d2);
        }
        private void SetInvoiceDataForSamePage(IInvoiceData inv, Control parent, IList logs, int userId)
        {
            decimal d = GetSamePageValues(inv, parent, "Deed");
            if (inv.DeedAmount != d)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".DeedAmount",
                                         inv.DeedAmount.ToString(),
                                         d.ToString(), userId));
                inv.DeedAmount = d;
            }
            decimal m = GetSamePageValues(inv, parent, "Mortgage");
            if (inv.MortgageAmount != m)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".MortgageAmount",
                                         inv.MortgageAmount.ToString(),
                                         m.ToString(), userId));
                inv.MortgageAmount = m;
            }
            decimal r = GetSamePageValues(inv, parent, "Release");
            if (inv.ReleaseAmount != r)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".ReleaseAmount",
                                         inv.ReleaseAmount.ToString(),
                                         r.ToString(), userId));
                inv.ReleaseAmount = r;
            }
            string s = GetTextBoxValue(parent, "Pages");
            if(!String.IsNullOrEmpty(s))
            {
                inv.SetFormulaData("Pages", s);
            }
            s = GetTextBoxValue(parent, "PricePerPageUnit");
            if (!String.IsNullOrEmpty(s))
            {
                inv.SetFormulaData("PricePerPageUnit", s);
            }
            GetOtherAmountsData(inv, parent, logs, userId);
        }
        #endregion

        #endregion

    }

    public class TitleInsuranceFormulaControl : FeeFormulaControl
    {
        #region constants
        private const int TOTALTITLEINSURANCEJSFORMULAID = 5;
        private const string TWOTIERUNITJS = "CalculateTwoTierUnit(this,'{0}',{1},{2});";
        private const string PERUNITJS = "CalculatePerUnit(this,'{0}',{1},{2});";
        private const string NTIERUNITJS = "CalculateNTierUnit(this,'{0}',{1},{2},{3});";
        private readonly string[,] TotalCostDescription = {
                                                              {"lbl@ @Total price:", "rc@Totalprice@ "}
                                                              , {"lbl@ @POC Amount:", "rc@POCAmount@ "}
                                                              , {"lbl@ @Lender Credit:", "rc@LenderCreditAmount@ "}
                                                              , {"lbl@ @Third Party paid:", "rc@ThirdPartyPaidAmount@ "}
                                                              , {"lbl@ @Financed Amount:", "lbl@FinancedAmount@ "}
                                                          };

        private readonly string[,] TwoTierUnitDescription = {
                                                              {"lbl@ @Jurisdiction uses (select one):", "sel@MaxSelect@ ","","","",""}
                                                              , {"lbl@ @Unit fee", "lbl@ @Per","lbl@ @Up to","lbl@ @Then unit fee","lbl@ @Per additional","lbl@ @Total"}
                                                              , {"rc@UnitFee@ ", "rc@Per@ ","rc@Upto@","rc@Thenunitfee@","rc@Peradditional@","lbl@Total@"}
                                                              , {"","","","", "lbl@ @POC Amount:", "rc@POCAmount@ "}
                                                              , {"","","","","lbl@ @Lender Credit:", "rc@LenderCreditAmount@ "}
                                                              , {"","","","","lbl@ @Third Party paid:", "rc@ThirdPartyPaidAmount@ "}
                                                              , {"","","","", "lbl@ @Financed Amount:", "lbl@FinancedAmount@ "}

                                                          };
        private readonly string[,] FourTierUnitDescription = {
                                                              {"lbl@ @Jurisdiction uses (select one):", "sel@MaxSelect@ ","","","",""}
                                                              , {"lbl@ @Unit fee", "rc@UnitFee0@ ","lbl@ @Per","rc@Per0@ ","lbl@ @Up to","rc@Upto0@"}
                                                              , {"lbl@ @Then unit fee", "rc@UnitFee1@ ","lbl@ @Per","rc@Per1@ ","lbl@ @Up to","rc@Upto1@"}
                                                              , {"lbl@ @Then unit fee", "rc@UnitFee2@ ","lbl@ @Per","rc@Per2@ ","lbl@ @Up to","rc@Upto2@"}
                                                              , {"lbl@ @Then unit fee", "rc@UnitFee3@ ","lbl@ @Per","rc@Per3@ ","","lbl@Total@"}
                                                              , {"lbl@ @POC Amount:", "rc@POCAmount@ ","","","",""}
                                                              , {"lbl@ @Lender Credit:", "rc@LenderCreditAmount@ ","","","",""}
                                                              , {"lbl@ @Third Party paid:", "rc@ThirdPartyPaidAmount@ ","","","",""}
                                                              , {"lbl@ @Financed Amount:", "lbl@FinancedAmount@ ","","","",""}

                                                          };
        #endregion


        #region constructor
        public TitleInsuranceFormulaControl(IInvoiceData inv)
            : base(inv)
        {
            if (inv.FormulaId < 0) inv.FormulaId = Invoice.TITLEINSURANCETOTALCOSTFORMULAID;
            switch (inv.FormulaId)
            {
                case Invoice.TITLEINSURANCETOTALCOSTFORMULAID:
                    Rows = 5;
                    Columns = 2;
                    break;
                case Invoice.TITLEINSURANCETWOTIERFORMULAID:
                    Rows = 7;
                    Columns = 6;
                    break;
                case Invoice.TITLEINSURANCEFOURTIERFORMULAID:
                    Rows = 9;
                    Columns = 6;
                    break;
                default:
                    Rows = 5;
                    Columns = 2;
                    break;
            }
        }
        #endregion

        protected override Control GetControl(int rowIndex, int columnIndex)
        {
            Control ctl = null;
            switch (invoice.FormulaId)
            {
                case Invoice.TITLEINSURANCETOTALCOSTFORMULAID:
                    ctl = GetCellControl(TotalCostDescription, rowIndex, columnIndex);
                    break;
                case Invoice.TITLEINSURANCETWOTIERFORMULAID:
                    ctl = GetCellControl(TwoTierUnitDescription, rowIndex, columnIndex);
                    break;
                case Invoice.TITLEINSURANCEFOURTIERFORMULAID:
                    ctl = GetCellControl(FourTierUnitDescription, rowIndex, columnIndex);
                    break;
            }
            return ctl;
        }
        public override void BindData(Control parent)
        {
            switch (invoice.FormulaId)
            {
                case Invoice.TITLEINSURANCETOTALCOSTFORMULAID:
                    BindTotalCostData(parent);
                    break;
                case Invoice.TITLEINSURANCETWOTIERFORMULAID:
                    BindTwoTierUnitData(parent);
                    break;
                case Invoice.TITLEINSURANCEFOURTIERFORMULAID:
                    BindFourTierUnitData(parent);
                    break;
            }
        }
        protected override void BindTotalCostData(Control parent)
        {
            string js = GetAmountFocusLostScript(TOTALTITLEINSURANCEJSFORMULAID);
            SetRadTextBoxValue(parent, "Totalprice", invoice.GetFormulaData("Totalprice"), js);
            SetOtherAmountsAndTotal(parent, invoice.AmountFinanced.ToString("C"), js);
        }
        protected override string GetArrayForFocusLost()
        {
            return "Totalprice_text,POCAmount_text,LenderCreditAmount_text,ThirdPartyPaidAmount_text";
        }
        protected void BindFourTierUnitData(Control parent)
        {
            string js = "";
            string total = APPLYTOLOAN;
            string financedAmount = APPLYTOLOAN;
            if (invoice is Invoice)
            {
                js = String.Format(NTIERUNITJS, GetArrayForNTiers(4), invoice.MaxClaimAmount, 4, invoice.PrincipalLimit);
                total = invoice.CalculateTotal().ToString("C");
                financedAmount = invoice.AmountFinanced.ToString("C");
            }
            SetDropDownValue(parent, "MaxSelect", MaxValues, invoice.GetFormulaData("MaxSelect"), js);
            for (int i = 0; i < 4;  i++)
            {
                SetRadTextBoxValue(parent, String.Format("UnitFee{0}", i), invoice.GetFormulaData(String.Format("UnitFee{0}", i)), js);
                SetRadTextBoxValue(parent, String.Format("Per{0}", i), invoice.GetFormulaData(String.Format("Per{0}", i)), js);
                if(i<3)
                {
                    SetRadTextBoxValue(parent, String.Format("Upto{0}", i), invoice.GetFormulaData(String.Format("Upto{0}", i)), js);
                }
            }
            SetLabelValue(parent, "Total", total);
            SetOtherAmountsAndTotal(parent, financedAmount, js);
        }
        protected override void AddValidators(HtmlTableCell td,Control ctl)
        {
            if(ctl.ID == "Upto1")
            {
                td.Controls.Add(new LiteralControl("&nbsp;"));
                td.Controls.Add(GetCompareValidatorForUpTo(ctl.ID, "Upto0","*"));
            }
            else if (ctl.ID == "Upto2")
            {
                td.Controls.Add(new LiteralControl("&nbsp;"));
                td.Controls.Add(GetCompareValidatorForUpTo(ctl.ID, "Upto1", "*"));
            }
            base.AddValidators(td,ctl);
        }
        private static Control GetCompareValidatorForUpTo(string ctlId, string ctlToCompareId, string errMessage)
        {
            CompareValidator cv = new CompareValidator();
            cv.ID = "cpv"+ctlId;
            cv.ControlToValidate = ctlId;
            cv.ControlToCompare = ctlToCompareId;
            cv.Operator = ValidationCompareOperator.GreaterThan;
            cv.Type = ValidationDataType.Double;
            cv.ErrorMessage =errMessage;
            return cv;
        }
        protected void BindTwoTierUnitData(Control parent)
        {
            string js = "";
            string total = APPLYTOLOAN;
            string financedAmount = APPLYTOLOAN;
            if (invoice is Invoice)
            {
                js = String.Format(TWOTIERUNITJS, GetArrayForForTwoTier(), invoice.MaxClaimAmount, invoice.PrincipalLimit);
                total = invoice.CalculateTotal().ToString("C");
                financedAmount = invoice.AmountFinanced.ToString("C");
            }

            SetDropDownValue(parent, "MaxSelect", MaxValues, invoice.GetFormulaData("MaxSelect"), js);
            SetRadTextBoxValue(parent, "UnitFee", invoice.GetFormulaData("UnitFee"), js);
            SetRadTextBoxValue(parent, "Per", invoice.GetFormulaData("Per"), js);
            SetRadTextBoxValue(parent, "Upto", invoice.GetFormulaData("Upto"), js);
            SetRadTextBoxValue(parent, "Thenunitfee", invoice.GetFormulaData("Thenunitfee"), js);
            SetRadTextBoxValue(parent, "Peradditional", invoice.GetFormulaData("Peradditional"), js);
            SetLabelValue(parent, "Total", total);
            SetOtherAmountsAndTotal(parent, financedAmount, js);
        }
        public override void SetInvoiceData(IInvoiceData inv, Control parent, ArrayList logs, int userId)
        {
            switch (invoice.FormulaId)
            {
                case Invoice.TITLEINSURANCETOTALCOSTFORMULAID:
                    SetInvoiceDataForTotal(inv, parent, logs, userId);
                    break;
                case Invoice.TITLEINSURANCETWOTIERFORMULAID:
                    SetInvoiceDataTwoTierUnit(inv, parent, logs, userId);
                    break;
                case Invoice.TITLEINSURANCEFOURTIERFORMULAID:
                    SetInvoiceDataFourTierUnit(inv, parent, logs, userId);
                    break;
            }
        }
        protected override void SetInvoiceDataForTotal(IInvoiceData inv, Control parent, IList logs, int userId)
        {
            decimal a = GetRadTextBoxValue(parent, "Totalprice");
            inv.SetFormulaData("Totalprice", a.ToString());
            if (inv.Amount != a)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".Amount",
                                         inv.Amount.ToString(),
                                         a.ToString(), userId));
            }
            inv.Amount = a;
            if (inv is ClosingCostProfile.ClosingCostProfileDetail)
            {
                inv.SetFormulaData("Totalprice", a.ToString());
            }
            GetOtherAmountsData(inv, parent, logs, userId);
        }

        private void SetInvoiceDataTwoTierUnit(IInvoiceData inv, Control parent, IList logs, int userId)
        {
            int maxSel = GetDdlValue(parent, "MaxSelect");
            decimal uf = GetRadTextBoxValue(parent, "UnitFee");
            decimal pr = GetRadTextBoxValue(parent, "Per");
            decimal ut = GetRadTextBoxValue(parent, "Upto");
            decimal tuf = GetRadTextBoxValue(parent, "Thenunitfee");
            decimal pa = GetRadTextBoxValue(parent, "Peradditional");
            inv.SetFormulaData("UnitFee", uf.ToString());
            inv.SetFormulaData("Per", pr.ToString());
            inv.SetFormulaData("Upto", ut.ToString());
            inv.SetFormulaData("Thenunitfee", tuf.ToString());
            inv.SetFormulaData("Peradditional", pa.ToString());
            inv.SetFormulaData("MaxSelect", maxSel.ToString());
            decimal a = inv.CalculateTotal();
            if (inv.Amount != a)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".Amount",
                                         inv.Amount.ToString(),
                                         a.ToString(), userId));
                inv.Amount = a;
            }
            GetOtherAmountsData(inv, parent, logs, userId);
            if (maxSel == Invoice.PRINCIPLELIMITID)
            {
                SetPrinciplaLimit(inv);
            }
        }
        private void SetInvoiceDataFourTierUnit(IInvoiceData inv, Control parent, IList logs, int userId)
        {
            int maxSel = GetDdlValue(parent, "MaxSelect");
            for(int i=0;i<4;i++)
            {
                decimal uf = GetRadTextBoxValue(parent, String.Format("UnitFee{0}",i));
                decimal pr = GetRadTextBoxValue(parent, String.Format("Per{0}",i));
                inv.SetFormulaData(String.Format("UnitFee{0}",i), uf.ToString());
                inv.SetFormulaData(String.Format("Per{0}",i), pr.ToString());
                if(i<3)
                {
                    decimal ut = GetRadTextBoxValue(parent, String.Format("Upto{0}", i));
                    inv.SetFormulaData(String.Format("Upto{0}", i), ut.ToString());
                }

            }
            inv.SetFormulaData("MaxSelect", maxSel.ToString());
            decimal a = inv.CalculateTotal();
            if (inv.Amount != a)
            {
                logs.Add(
                    new MortgageLogEntry(OBJECTNAME, inv.ID, OBJECTNAME + ".Amount",
                                         inv.Amount.ToString(),
                                         a.ToString(), userId));
                inv.Amount = a;
            }
            GetOtherAmountsData(inv, parent, logs, userId);
            if (maxSel == Invoice.PRINCIPLELIMITID)
            {
                SetPrinciplaLimit(inv);
            }
        }
        private static string GetArrayForForTwoTier()
        {
            return "MaxSelect&UnitFee_text,Per_text,Upto_text,Thenunitfee_text,Peradditional_text,POCAmount_text,LenderCreditAmount_text,ThirdPartyPaidAmount_text&Total,FinancedAmount";
        }
        private static string GetArrayForNTiers(int n)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < n;i++ )
            {
                if(i!=0)
                {
                    sb.Append(",");
                }
                if(i < (n-1))
                {
                    sb.Append(String.Format("UnitFee{0}_text,Per{0}_text,Upto{0}_text", i));
                }
                else
                {
                    sb.Append(String.Format("UnitFee{0}_text,Per{0}_text", i));
                }
            }
            return string.Format("MaxSelect&{0},POCAmount_text,LenderCreditAmount_text,ThirdPartyPaidAmount_text&Total,FinancedAmount", sb);
        }
    }
}
