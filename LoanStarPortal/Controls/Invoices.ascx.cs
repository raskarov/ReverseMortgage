using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;


namespace LoanStarPortal.Controls
{
    public partial class Invoices : MortgageDataGridControl
    {

        #region constants
        private const string OBJECTNAME = "Invoices";
        private static readonly string[] gridFieldsAdd = { 
            "TypeID", "Description", "Provider", "Amount" ,"OtherProviderName","OtherTypeDescription","ProviderTypeId","DeedAmount","Listendorsements","LenderCreditAmount","ThirdPartyPaidAmount"
        };
        private const string EDITFORMCONTROLNAME = "InvoiceEdit.ascx";
        private const string ADDRECORDTEXT = "Add new invoice";
        private const string CHANGEPROVIDERCOMMAND = "ChangeProvider";
        private const string CHANGEAMOUNTCOMMAND = "ChangeAmount";
        private const string CHANGEPOCAMOUNTCOMMAND = "ChangePocAmount";
        private const int MODECHANGEPROVIDER = 4;
        private const int MODECHANGEAMOUNT = 5;
        private const int MODECHANGEPOCAMOUNT = 6;
        private const string ONCLICKATTRIBUTE = "OnClick";
        public const string VIEWORDERCOMMANDNAME = "vieworder";
        public const string SENDORDERCOMMANDNAME = "sendorder";
        #endregion

        #region fields
        private int previosMode;
        //private readonly bool canChangeProvider = true;
        //private readonly bool canChangeAmount = true;
        //private readonly bool canChangePocAmount = true;
        //private bool canChangeStatus;
        private Invoice invoice;
        private readonly string objectName = OBJECTNAME;
        private readonly string addRecordText = ADDRECORDTEXT;
        private DataView dvGridData;
        private decimal totalChargeBorrower = 0;
        private decimal totalChargeSeller = 0;
        private MortgageProfile mp;
        #endregion

        public event UpdateNeeded OnUpdateNeeded;
        public delegate void UpdateNeeded();

        #region Properties

        protected bool CanChangeProvider
        {
            get { return IsFieldEditable("Provider") && IsFieldEditable("OtherProviderName"); }
        }
        protected bool CanChangeAmount
        {
            get { return IsFieldEditable("Amount"); }
        }
        protected bool CanChangePocAmount
        {
            get { return IsFieldEditable("POCAmount"); }
        }
        protected override string EditFormControlName
        {
            get
            {
                return EDITFORMCONTROLNAME;
            }
        }
        protected override int EditMode
        {
            get { return FORMEDIT; }
        }
        protected override string AddRecordText
        {
            get
            {
                return addRecordText;
            }
        }
        protected override string ObjectName
        {
            get
            {
                return objectName;
            }
        }
        protected override DataView DvGridData
        {
            get
            {
                if(dvGridData == null)
                {
                    dvGridData = Invoice.GetInvoiceList(MortgageId);
                }
                return dvGridData;
            }
        }
        protected override object EditObject
        {
            get
            {
                if (invoice == null)
                {
                    invoice = new Invoice();
                }
                return invoice;
            }
        }
        #endregion

        #region Methods
        #region overriden methods
        protected override void Page_Load(object sender, EventArgs e)
        {
            mp = CurrentPage.GetMortgage(MortgageId);
            G = gInvoices;
            RadAjaxManager ajax = ((Default)Page).AjaxManager;
            ajax.AjaxSettings.AddAjaxSetting(gInvoices, gInvoices, null);
            base.Page_Load(sender, e);
        }
        protected override void BindGrid()
        {
            totalChargeBorrower = 0;
            totalChargeSeller = 0;
            base.BindGrid();
        }
        protected override void CheckFieldsAccess()
        {
            canAddNew = true;
            canEdit = false;
            for (int i = 0; i < gridFieldsAdd.Length; i++)
            {
                if (fields.ContainsKey(gridFieldsAdd[i]))
                {
                    canAddNew &= !(bool)fields[gridFieldsAdd[i]];
                    canEdit |= !(bool)fields[gridFieldsAdd[i]];
                }
            }
        }
        protected override void G_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            bool showUpdate = false;
            bool showCancel = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gridMode == MODEVIEW)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    if (btnDelete != null)
                    {
                        if (canAddNew)
                        {
                            btnDelete.Attributes.Add(ONCLICKATTRIBUTE, "javascript:{{var r=confirm('Delete this invoice?');if (!r)return false;}};");
                        }
                        else
                        {
                            btnDelete.Visible = false;
                        }
                    }
                }
                DataRowView dr = (DataRowView)e.Row.DataItem;
                int id = Convert.ToInt32(dr["id"].ToString());
                Invoice inv;
                if (id > 0)
                {
                    inv = new Invoice((DataRowView)e.Row.DataItem,mp);
                }                
                else
                {
                    inv = new Invoice(-1);
                }
                bool canEditProvider = (inv.ProviderTypeId == Invoice.OTHERPROVIDERTYPEID) || (inv.ProviderTypeId == Invoice.VENDORPROVIDERTYPEID) && CanChangeProvider;
                Label lblp = (Label)e.Row.FindControl("lblProvider");
                LinkButton lbp = (LinkButton)e.Row.FindControl("lbProvider");
                if ((lblp != null) && (lbp != null))
                {
                    lblp.Visible = !canEditProvider;
                    lbp.Visible = canEditProvider;
                }
                //Label lbla_ = (Label)e.Row.FindControl("lblInvoiceAmount");
                //LinkButton lba_ = (LinkButton)e.Row.FindControl("lbInvoiceAmount");
                //if ((lbla_ != null) && (lba_ != null))
                //{
                //    if (inv.HasFormula)
                //    {
                //        lba_.Visible = false;
                //        lbla_.Visible = true;
                //    }
                //    else
                //    {
                //        if(CanChangeAmount)
                //        {
                //            lba_.Visible = true;
                //            lbla_.Visible = false;
                //        }
                //        else
                //        {
                //            lba_.Visible = false;
                //            lbla_.Visible = true;
                //        }
                //    }
                //}
                //Label lblpa = (Label)e.Row.FindControl("lblPocAmount");
                //LinkButton lbpa = (LinkButton)e.Row.FindControl("lbPocAmount");
                //if ((lblpa != null) && (lbpa != null))
                //{
                //    lblpa.Visible = !CanChangePocAmount;
                //    lbpa.Visible = CanChangePocAmount;
                //}

                if (gridMode != MODEVIEW && currentRow == EditRowId)
                {
                    if (id > 0)
                    {
                        invoice = new Invoice((DataRowView)e.Row.DataItem,mp);
                    }
                    else
                    {
                        invoice = new Invoice(-1);
                    }

                    showCancel = true;
                    if (gridMode == MODECHANGEPROVIDER)
                    {
                        if (lblp != null) lblp.Visible = false;
                        if (lbp != null) lbp.Visible = false;

                        if (invoice.ProviderTypeId == Invoice.OTHERPROVIDERTYPEID)
                        {
                            TextBox tb = (TextBox)e.Row.FindControl("tbProvidere");
                            RequiredFieldValidator v = (RequiredFieldValidator)e.Row.FindControl("rftbProvidere");
                            if ((tb != null) && (v != null))
                            {
                                tb.Visible = true;
                                v.Visible = true;
                                tb.Text = invoice.OtherProviderName;
                                showUpdate = true;
                            }
                        }
                        else if (invoice.ProviderTypeId == Invoice.VENDORPROVIDERTYPEID)
                        {
                            DropDownList ddlv = (DropDownList)e.Row.FindControl("ddlProvidere");
                            if (ddlv != null)
                            {
                                DataView dv = VendorGlobal.GetVendorsForInvice(CurrentUser.CompanyId, invoice.TypeID, mp.ID);
                                dv.RowFilter = "(isdeleted=0 and isdisabled=0) or isinuse=1";
                                ddlv.DataSource = dv;
                                ddlv.DataTextField = "Name";
                                ddlv.DataValueField = "ID";
                                ddlv.DataBind();
                                ddlv.SelectedValue = invoice.ProviderId.ToString();
                                showUpdate = false;
                                ddlv.Visible = true;
                            }
                        }
                    }
                    else if (gridMode == MODECHANGEAMOUNT)
                    {
                        //if (lbla_ != null) lbla_.Visible = false;
                        //if (lba_ != null) lba_.Visible = false;
                        //RadNumericTextBox tb = (RadNumericTextBox)e.Row.FindControl("tbAmount");
                        //if (tb != null)
                        //{
                        //    tb.Visible = true;
                        //    tb.Text = invoice.Amount.ToString();
                        //    showUpdate = true;
                        //}
                        //RangeValidator rv = (RangeValidator)e.Row.FindControl("rvAmount");
                        //if (rv != null)
                        //{
                        //    rv.MinimumValue = invoice.POCAmount.ToString();
                        //    rv.Visible = true;
                        //}                        
                    }
                    else if (gridMode == MODECHANGEPOCAMOUNT)
                    {
                        //if (lblpa != null) lblpa.Visible = false;
                        //if (lbpa != null) lbpa.Visible = false;
                        //RadNumericTextBox tb = (RadNumericTextBox)e.Row.FindControl("tbPocAmount");
                        //if (tb != null)
                        //{
                        //    tb.Visible = true;
                        //    tb.Text = invoice.POCAmount.ToString();
                        //    showUpdate = true;
                        //}
                        //RangeValidator rv = (RangeValidator)e.Row.FindControl("rvPocAmount");
                        //if (rv != null)
                        //{
                        //    rv.MaximumValue = invoice.Amount.ToString();
                        //    rv.Visible = true;
                        //}
                    }
                    EditItemId = invoice.ID;
                }
                if (gridMode != MODEVIEW)
                {
                    if (currentRow == EditRowId)
                    {
                        SetActionColumn(e, showCancel, showUpdate);
                    }
                    else
                    {
                        SetActionColumn(e, false, false);
                    }
                }
                currentRow++;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (showFooter)
                {
                    CreateFooter(e, 1);
                    e.Row.Cells[1].Text = "Total borrower: <br>Total seller: ";
//                    e.Row.Cells[1].ColumnSpan = canAddNew ? 1 : 2;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[2].Text = totalChargeBorrower.ToString("C") + "<br />" + totalChargeSeller.ToString("C");
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
//                    try
//                    {
////                        if (!canAddNew) e.Row.Cells.RemoveAt(4);
////                        e.Row.Cells.RemoveAt(4);
//                        e.Row.Cells.RemoveAt(3);
//                    }
//                    catch { }

                }
            }
        }
        protected override void G_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            previosMode = gridMode;
            if (e.CommandName == DELETECOMMAND)
            {
                Invoice.Delete(Convert.ToInt32(e.CommandArgument));
                ResetDataSource();
                mp.ResetInvoices();
                BindGrid();
            }
            else
            {
                if (e.CommandName == CHANGEPROVIDERCOMMAND)
                {
                    EditRowId = Convert.ToInt32(e.CommandArgument);
                    gridMode = MODECHANGEPROVIDER;
                    showFooter = false;
                    BindGrid();
                }
                else if (e.CommandName == CHANGEAMOUNTCOMMAND)
                {
                    EditRowId = Convert.ToInt32(e.CommandArgument);
                    gridMode = MODECHANGEAMOUNT;
                    showFooter = false;
                    BindGrid();
                }
                else if (e.CommandName == CHANGEPOCAMOUNTCOMMAND)
                {
                    EditRowId = Convert.ToInt32(e.CommandArgument);
                    gridMode = MODECHANGEPOCAMOUNT;
                    showFooter = false;
                    BindGrid();
                }
                else if (e.CommandName == VIEWORDERCOMMANDNAME || e.CommandName == SENDORDERCOMMANDNAME)
                {
                    showFooter = false;
                    BindGrid();
                }
                else
                {
                    Session[InvoiceEdit.INVOICE] = null;
                    base.G_RowCommand(sender, e);
                    if (e.CommandName == CANCELCOMMAND)
                    {
                        showFooter = true;
                        BindGrid();
                    }
                }
            }
        }
        protected override void Save(object o, ArrayList logs)
        {
            if (mp != null)
            {
                invoice.MortgageID = mp.ID;
                mp.SaveInvoiceWithLog(invoice, logs);
            }
            base.Save(o, logs);
        }
        protected override void Update(int rowIndex)
        {
            ArrayList logs = new ArrayList();
            invoice = new Invoice(EditItemId);
            invoice.MortgageID = MortgageId;
            if (previosMode == MODECHANGEPROVIDER)
            {
                TextBox tb = (TextBox)gInvoices.Rows[rowIndex].FindControl("tbProvidere");
                string providerName = tb.Text;
                if (providerName != invoice.OtherProviderName)
                {
                    logs.Add(new MortgageLogEntry("Invoice", invoice.ID, "Invoice.OtherProviderName", invoice.OtherProviderName, providerName, CurrentPage.CurrentUser.Id));
                    invoice.OtherProviderName = providerName;
                    if (mp != null)
                    {
                        mp.SaveInvoiceWithLog(invoice, logs);
                    }
                }
            }
            else if (previosMode == MODECHANGEAMOUNT)
            {
                RadNumericTextBox tb = (RadNumericTextBox)gInvoices.Rows[rowIndex].FindControl("tbAmount");
                decimal amount = 0;
                try
                {
                    amount = decimal.Parse(tb.Text);
                }
                catch { }
                if (amount != invoice.Amount)
                {
                    logs.Add(new MortgageLogEntry("Invoice", invoice.ID, "Invoice.Amount", invoice.Amount.ToString(), amount.ToString(), CurrentPage.CurrentUser.Id));
                    invoice.Amount = amount;
                    if (mp != null)
                    {
                        mp.SaveInvoiceWithLog(invoice, logs);
                    }
                }
            }
            else if (previosMode == MODECHANGEPOCAMOUNT)
            {
                RadNumericTextBox tb = (RadNumericTextBox)gInvoices.Rows[rowIndex].FindControl("tbPocAmount");
                decimal amount = 0;
                try
                {
                    amount = decimal.Parse(tb.Text);
                }
                catch { }
                if (amount != invoice.POCAmount)
                {
                    logs.Add(new MortgageLogEntry("Invoice", invoice.ID, "Invoice.POCAmount", invoice.POCAmount.ToString(), amount.ToString(), CurrentPage.CurrentUser.Id));
                    invoice.POCAmount = amount;
                    if (mp != null)
                    {
                        mp.SaveInvoiceWithLog(invoice, logs);
                    }
                }
            }
        }
        protected override void G_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            base.G_RowDeleting(sender, e);
        }
        protected override void ResetDataSource()
        {
            dvGridData = null;
        }
        #endregion
        protected void Providere_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)gInvoices.Rows[EditRowId].FindControl("ddlProvidere");
            if (ddl != null)
            {
                invoice = new Invoice(EditItemId);
                if (invoice != null)
                {
                    int vendorId = Convert.ToInt32(ddl.SelectedValue);
                    ArrayList logs = new ArrayList();
                    invoice.MortgageID = MortgageId;
                    if (vendorId != invoice.ProviderId)
                    {
                            logs.Add(new MortgageLogEntry("Invoice", invoice.ID, "Invoice.ProviderId", invoice.ProviderId.ToString(), vendorId.ToString(), CurrentPage.CurrentUser.Id));
                            invoice.ProviderId = vendorId;
                            if (mp != null)
                            {
                                mp.SaveInvoiceWithLog(invoice, logs);
                            }
                    }
                }
            }
            gridMode = MODEVIEW;
            EditItemId = -1;
            EditRowId = -1;
            ResetDataSource();
            showFooter = true;
            BindGrid();
        }
        protected string GetFinancedAmount(Object o)
        {
            decimal res = 0;
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                Invoice inv = new Invoice(row,mp);
                res = inv.AmountFinanced;
                if (inv.ChargeToId == Constants.INVOICECHARGETOBORROWER)
                {                    
                    totalChargeBorrower += res;
                }
                else
                {
                    totalChargeSeller += res;
                }
            }
            return res.ToString("C");
        }
        protected string GetInvoiceAmount(Object o)
        {
            string res  = String.Empty;
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                Invoice inv = new Invoice(row,mp);
                if (inv.FeeCategoryID == MortgageProfile.FEECATEGORYGOVERMENTCHARGE)
                {
                    res = inv.Amount.ToString("C"); 
                }
                else
                {
                    res = inv.Amount.ToString("C");
                }                
            }
            return res;
        }
        protected string GetPocAmount(Object o)
        {
            string res = String.Empty;
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                Invoice inv = new Invoice(row,mp);
                if (inv.FeeCategoryID == MortgageProfile.FEECATEGORYGOVERMENTCHARGE)
                {
                    res = "";
                }
                else
                {
                    res = inv.POCAmount.ToString("C");
                }
            }
            return res;
        }
        protected static string GetDueDate(Object o)
        {
            string result = String.Empty;
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                try
                {
                    int statusId = int.Parse(row["statusId"].ToString());
                    if (statusId != Invoice.STATUSPAYATSETTLEMENT)
                    {
                        string s = row["dueDate"].ToString();
                        if (!String.IsNullOrEmpty(s))
                        {
                            DateTime dt = DateTime.Parse(s);
                            result = dt.ToString("MM/dd/yyyy");
                        }
                    }
                }
                catch
                {
                }
            }
            return result;
        }
        protected string GetProviderName(Object o)
        {
            string res = String.Empty;
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                Invoice inv = new Invoice(row,mp);
                res = inv.Provider;
            }
            return res;
        }
        protected static string GetTypeName(Object o)
        {
            string res = String.Empty;
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                res = row["TypeName"].ToString();
                if(Invoice.IsTypeOther(int.Parse(row["typeid"].ToString())))
                {
                    res = row["Description"].ToString();
                }

            }
            return res;
        }

        #endregion

    }
}