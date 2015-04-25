using System;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using Telerik.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class ClosingCostProfileView : MortgageDataGridControl
    {
        #region constants
        private const string OBJECTNAME = "ClosingCostProfile";
        private const string EDITFORMCONTROLNAME = "ClosingCostProfileEdit.ascx";
        private const string ADDRECORDTEXT = "Add new record";
        private const string CHANGEPROVIDERCOMMAND = "ChangeProvider";
        private const string CHANGEAMOUNTCOMMAND = "ChangeAmount";
        private const string CHANGEPOCAMOUNTCOMMAND = "ChangePocAmount";
        private const int MODECHANGEPROVIDER = 4;
        private const int MODECHANGEAMOUNT = 5;
        private const int MODECHANGEPOCAMOUNT = 6;
        private const string ONCLICKATTRIBUTE = "OnClick";
        #endregion

        #region fields
        private int previosMode;
        private readonly string objectName = OBJECTNAME;
        private readonly string addRecordText = ADDRECORDTEXT;
        private DataView dvGridData;
        private decimal totalChargeBorrower = 0;
        private decimal totalChargeSeller = 0;
        private LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail details;
        #endregion

        public event UpdateNeeded OnUpdateNeeded;
        public delegate void UpdateNeeded();

        #region Properties
        private int ClosingCostProfileId
        {
            get
            {
                int res = ClosingCostProfile.ADDNEWVALUE;
                Object o = Session[ClosingCostProfile.CURRENTPROFILEID];
                if (o != null)
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
                if (dvGridData == null)
                {
                    dvGridData = LoanStar.Common.ClosingCostProfile.GetClosingCostProfileDetailes(ClosingCostProfileId == 0 ? -1 : ClosingCostProfileId);
                }
                return dvGridData;
            }
        }
        protected override object EditObject
        {
            get
            {
                if (details == null)
                {
                    details = new LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail(-1);
                    details.CompanyId = CurrentUser.CompanyId;
                    details.ClosingCostProfileId = ClosingCostProfileId;
                }
                return details;
            }
        }

        #endregion

        #region Methods
        public void ResetGridData()
        {
            dvGridData = null;
        }

        public void BindData()
        {
            canAddNew = ClosingCostProfileId > 0;
            G = gInvoices;
            BindGrid();
        }

        #region overriden methods
        protected override void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }
        protected override void BindGrid()
        {
            totalChargeBorrower = 0;
            totalChargeSeller = 0;
            base.BindGrid();
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
                        btnDelete.Attributes.Add(ONCLICKATTRIBUTE, "javascript:{{var r=confirm('Delete this row?');if (!r)return false;}};");
                    }
                }
                DataRowView dr = (DataRowView)e.Row.DataItem;
                int id = Convert.ToInt32(dr["id"].ToString());
                LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail item;
                if (id > 0)
                {
                    item = new LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail((DataRowView)e.Row.DataItem);
                }
                else
                {
                    item = new LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail(-1);
                }
                bool canEditProvider = (item.ProviderTypeId == Invoice.OTHERPROVIDERTYPEID) || (item.ProviderTypeId == Invoice.VENDORPROVIDERTYPEID);
                Label lblp = (Label)e.Row.FindControl("lblProvider");
                LinkButton lbp = (LinkButton)e.Row.FindControl("lbProvider");
                if ((lblp != null) && (lbp != null))
                {
                    lblp.Visible = !canEditProvider;
                    lbp.Visible = canEditProvider;
                }
                Label lbla = (Label)e.Row.FindControl("lblInvoiceAmount");
                LinkButton lba = (LinkButton)e.Row.FindControl("lbInvoiceAmount");
                if ((lbla != null) && (lba != null))
                {
                    lba.Visible = !item.HasFormula ;
                    lbla.Visible = !lba.Visible;
                }
                Label lblpa = (Label)e.Row.FindControl("lblPocAmount");
                LinkButton lbpa = (LinkButton)e.Row.FindControl("lbPocAmount");
                if ((lblpa != null) && (lbpa != null))
                {
                    lblpa.Visible = false;
                    lbpa.Visible = true;
                }

                if (gridMode != MODEVIEW && currentRow == EditRowId)
                {
                    if (id > 0)
                    {
                        details = new LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail((DataRowView)e.Row.DataItem);
                    }
                    else
                    {
                        details = new LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail(-1);
                    }
                    details.CompanyId = CurrentUser.CompanyId;
                    details.ClosingCostProfileId = ClosingCostProfileId;
                    showCancel = true;
                    if (gridMode == MODECHANGEPROVIDER)
                    {
                        if (lblp != null) lblp.Visible = false;
                        if (lbp != null) lbp.Visible = false;

                        if (details.ProviderTypeId == Invoice.OTHERPROVIDERTYPEID)
                        {
                            TextBox tb = (TextBox)e.Row.FindControl("tbProvidere");
                            RequiredFieldValidator v = (RequiredFieldValidator)e.Row.FindControl("rftbProvidere");
                            if ((tb != null) && (v != null))
                            {
                                tb.Visible = true;
                                v.Visible = true;
                                tb.Text = details.OtherProviderName;
                                showUpdate = true;
                            }
                        }
                        else if (details.ProviderTypeId == Invoice.VENDORPROVIDERTYPEID)
                        {
                            DropDownList ddlv = (DropDownList)e.Row.FindControl("ddlProvidere");
                            if (ddlv != null)
                            {
                                DataView dv = details.GetVendors(CurrentUser.CompanyId);
                                ddlv.DataSource = dv;
                                ddlv.DataTextField = "Name";
                                ddlv.DataValueField = "ID";
                                ddlv.DataBind();
                                ddlv.SelectedValue = details.ProviderId.ToString();
                                showUpdate = false;
                                ddlv.Visible = true;
                            }
                        }
                    }
                    else if (gridMode == MODECHANGEAMOUNT)
                    {
                        if (lbla != null) lbla.Visible = false;
                        if (lba != null) lba.Visible = false;
                        RadNumericTextBox tb = (RadNumericTextBox)e.Row.FindControl("tbAmount");
                        if (tb != null)
                        {
                            tb.Visible = true;
                            tb.Text = details.Amount.ToString();
                            showUpdate = true;
                        }
                        RangeValidator rv = (RangeValidator)e.Row.FindControl("rvAmount");
                        if (rv != null)
                        {
                            rv.MinimumValue = details.POCAmount.ToString();
                            rv.Visible = true;
                        }
                    }
                    else if (gridMode == MODECHANGEPOCAMOUNT)
                    {
                        if (lblpa != null) lblpa.Visible = false;
                        if (lbpa != null) lbpa.Visible = false;
                        RadNumericTextBox tb = (RadNumericTextBox)e.Row.FindControl("tbPocAmount");
                        if (tb != null)
                        {
                            tb.Visible = true;
                            tb.Text = details.POCAmount.ToString();
                            showUpdate = true;
                        }
                        RangeValidator rv = (RangeValidator)e.Row.FindControl("rvPocAmount");
                        if (rv != null)
                        {
                            rv.MaximumValue = details.Amount.ToString();
                            rv.Visible = true;
                        }
                    }
                    EditItemId = details.ID;
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
//                    e.Row.Cells[1].ColumnSpan = canAddNew ? 3 : 4;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[2].Text = totalChargeBorrower.ToString("C") + "<br />" + totalChargeSeller.ToString("C");
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                    //try
                    //{
                    //    if (!canAddNew) e.Row.Cells.RemoveAt(5);
                    //    e.Row.Cells.RemoveAt(4);
                    //    e.Row.Cells.RemoveAt(3);
                    //}
                    //catch { }

                }
            }
        }

        protected override void G_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            previosMode = gridMode;
            if (e.CommandName == DELETECOMMAND)
            {
                LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail.Delete(Convert.ToInt32(e.CommandArgument));
                ResetDataSource();
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
                //else if (e.CommandName == CHANGEAMOUNTCOMMAND)
                //{
                //    EditRowId = Convert.ToInt32(e.CommandArgument);
                //    gridMode = MODECHANGEAMOUNT;
                //    showFooter = false;
                //    BindGrid();
                //}
                //else if (e.CommandName == CHANGEPOCAMOUNTCOMMAND)
                //{
                //    EditRowId = Convert.ToInt32(e.CommandArgument);
                //    gridMode = MODECHANGEPOCAMOUNT;
                //    showFooter = false;
                //    BindGrid();
                //}
                else
                {
                    if(e.CommandName == "Update")
                    {
                        LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail o = Session[InvoiceEdit.INVOICE] as LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail;
                        if (o !=null&&o.HasFormula)
                        {
                            return;
                        }
                    }
                    
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
            details = (LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail)o;
            details.Save();
            if(!details.HasFormula)
            {
                base.Save(o, logs);
                dvGridData = null;
                BindGrid();
            }
        }
        protected override void Update(int rowIndex)
        {
            details = new LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail(EditItemId);
            if (previosMode == MODECHANGEPROVIDER)
            {
                TextBox tb = (TextBox)gInvoices.Rows[rowIndex].FindControl("tbProvidere");
                string providerName = tb.Text;
                if (providerName != details.OtherProviderName)
                {
                    details.OtherProviderName = providerName;
                    details.Save();
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
                if (amount != details.Amount)
                {
                    details.Amount = amount;
                    details.Save();
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
                if (amount != details.POCAmount)
                {
                    details.POCAmount = amount;
                    details.Save();
                }
            }
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
                details  = new LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail(EditItemId);
                if (details != null)
                {
                    int vendorId = Convert.ToInt32(ddl.SelectedValue);
                    if (vendorId != details.ProviderId)
                    {
                        details.ProviderId = vendorId;
                        details.Save();
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
                LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail item = new LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail(row);
                res = item.AmountFinanced;
                if (item.ChargeToId == Constants.INVOICECHARGETOBORROWER)
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
        protected static string GetInvoiceAmount(Object o)
        {
            string res = String.Empty;
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail item = new LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail(row);
                if (item.FeeCategoryId == MortgageProfile.FEECATEGORYGOVERMENTCHARGE)
                {
                    res = "";
                }
                else
                {
                    res = item.Amount.ToString("C");
                }
            }
            return res;
        }
        protected static string GetPocAmount(Object o)
        {
            string res = String.Empty;
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail item = new LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail(row);
                if (item.FeeCategoryId == MortgageProfile.FEECATEGORYGOVERMENTCHARGE)
                {
                    res = "";
                }
                else
                {
                    res = item.POCAmount.ToString("C");
                }
            }
            return res;
        }
        protected static string GetProviderName(Object o)
        {
            string res = String.Empty;
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail item = new LoanStar.Common.ClosingCostProfile.ClosingCostProfileDetail(row);
                res = item.Provider;
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
                if (Invoice.IsTypeOther(int.Parse(row["typeid"].ToString())))
                {
                    res = row["Description"].ToString();
                }

            }
            return res;
        }

        #endregion

    }
}
   
