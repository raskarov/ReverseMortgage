using System;
using System.Data;
using System.Collections;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class Invoices : AppControl
    {
        private const string FIRST_LOAD = "InvoiceFirstLoad";
        private DataTable InvoiceGridSource
        {
            get
            {
                Object obj = ViewState["igds"];
                if (obj != null)
                {
                    return (DataTable)obj;
                }
                else
                {
                    DataTable table = Invoice.GetInvoiceList(MortgageID).Table;
                    ViewState["igds"] = table;
                    return table;
                }
            }
        }
        private int MortgageID
        {
            get
            {
                return Convert.ToInt32(Session[Constants.MortgageID]);
            }
        }
        protected int EditItemId
        {
            get
            {
                int res = -1;
                Object o = Session["EDITITEMID"];
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
                Session["EDITITEMID"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ExecCommand();
            BindData();
        }
        private void ExecCommand()
        {
            for (int i = 0; i < Page.Request.Form.AllKeys.Length; i++)
            {
                string key = Page.Request.Form.AllKeys[i].ToLower();
                if (key.EndsWith("$performinsertbutton.x") || key.EndsWith("$updatebutton.x"))
                {
                    int k = key.IndexOf("$gridinvoices$");
                    if (k > 0)
                    {
                        key = key.Substring(k);
                        k = key.LastIndexOf("$");
                        if (k > 0)
                        {
                            key = key.Substring(0, k+1);
                            if (SaveData(key) > 0)
                            {
                                gridInvoices.Rebind();
                            }

                        }
                    }
                    return;
                }
            }
        }
        private int SaveData(string key)
        {
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageID);
            Invoice invoice = new Invoice(EditItemId);
//            invoice.ID = EditItemId;
            invoice.MortgageID = MortgageID;
            int typeid = GetPostedValueInt(key + "ctl00");
            int providerid = GetPostedValueInt(key + "ctl01");
            bool inValue = GetPostedValueBool(key + "ctl02");
            decimal invoiceSum = GetPostedValueDecimal(key + "tbinvoice");
            string pmt = GetPostedValueString(key + "ctl03");
            ArrayList logs = new ArrayList();
            if (typeid != invoice.TypeID)
            {
                logs.Add(new MortgageLogEntry("Invoice", invoice.ID, "Invoice.TypeID", invoice.TypeID.ToString(), typeid.ToString(), CurrentPage.CurrentUser.Id));
            }
            invoice.TypeID = typeid;
            if (providerid != invoice.ProviderID)
            {
                logs.Add(new MortgageLogEntry("Invoice", invoice.ID, "Invoice.ProviderID", invoice.ProviderID.ToString(), providerid.ToString(), CurrentPage.CurrentUser.Id));
            }
            invoice.ProviderID = providerid;
            if (inValue != invoice.IN)
            {
                logs.Add(new MortgageLogEntry("Invoice", invoice.ID, "Invoice.IN", invoice.IN.ToString(), inValue.ToString(), CurrentPage.CurrentUser.Id));
            }
            invoice.IN = inValue;
            if (invoiceSum != invoice.InvoiceAmt)
            {
                logs.Add(new MortgageLogEntry("Invoice", invoice.ID, "Invoice.InvoiceAmt", invoice.InvoiceAmt.ToString(), invoiceSum.ToString(), CurrentPage.CurrentUser.Id));
            }
            invoice.InvoiceAmt = invoiceSum;
            if (pmt != invoice.PMT)
            {
                logs.Add(new MortgageLogEntry("Invoice", invoice.ID, "Invoice.PMT", invoice.PMT, pmt, CurrentPage.CurrentUser.Id));
            }
            invoice.PMT = pmt;
            if (EditItemId < 0)
            {
                invoice.CreatedBy = CurrentUser.Id;
            }
            return mp.SaveInvoiceWithLog(invoice, logs);
        }
        private string GetPostedValueString(string key)
        {
            string res = String.Empty;
            for (int i = 0; i < Page.Request.Form.AllKeys.Length; i++)
            {
                if (Page.Request.Form.AllKeys[i].ToLower().EndsWith(key))
                {
                    res = Page.Request.Form[Page.Request.Form.AllKeys[i]];
                    break;
                }
            }
            return res;
        }
        private bool GetPostedValueBool(string key)
        {
            return GetPostedValueString(key) == "on";
        }
        private decimal GetPostedValueDecimal(string key)
        {
            decimal res = 0;
            string val = GetPostedValueString(key);
            if (!String.IsNullOrEmpty(val))
            {
                try
                {
                    res = Convert.ToDecimal(val);
                }
                catch { }
            }
            return res;
        }
        private int GetPostedValueInt(string key)
        {
            int res = 0;
            string val = GetPostedValueString(key);
            if (!String.IsNullOrEmpty(val))
            {
                try
                {
                    res = Convert.ToInt32(val);
                }
                catch { }
            }
            return res;
        }
        private void BindData()
        {
            InvoiceTypeSource.SelectCommand = "GetInvoiceTypeList";
            InvoiceTypeSource.ConnectionString = AppSettings.SqlConnectionString;
            InvoiceProviderSource.SelectCommand = "GetInvoiceProviderList";
            InvoiceProviderSource.ConnectionString = AppSettings.SqlConnectionString;
            if (ViewState[FIRST_LOAD] == null)
            {
                ViewState[FIRST_LOAD] = 1;
                gridInvoices.Rebind();
            }
        }
        protected void gridInvoices_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            gridInvoices.DataSource = InvoiceGridSource;
        }
        protected void SaveInvoice(DataRow row)
        {
            Invoice sinvoice = new Invoice();
            sinvoice.ID = Convert.ToInt32(row["ID"]);
            sinvoice.MortgageID = MortgageID;
            sinvoice.TypeID = Convert.ToInt32(row["TypeID"]);
            sinvoice.ProviderID = Convert.ToInt32(row["ProviderID"]);
            sinvoice.IN = Convert.ToBoolean(row["IN"]);
            sinvoice.InvoiceAmt = Convert.ToDecimal(row["Invoice"]);
            sinvoice.PMT = Convert.ToString(row["PMT"]);
            sinvoice.CreatedBy = CurrentUser.Id;
            int id =sinvoice.Save();
            if (id > 0)
                row["ID"] = id;
        }

        protected void gridInvoices_ItemCommand(object source, GridCommandEventArgs e)
        {
            int id = -1;
            if (e.CommandName == RadGrid.EditCommandName)
            {
                DataRowView dr = e.Item.DataItem as DataRowView;
                if (dr != null)
                {
                    id = Convert.ToInt32(dr["id"]);
                }
            }
            EditItemId = id;
//            switch (e.CommandName)
//            {
//                case RadGrid.PerformInsertCommandName:
//                case RadGrid.UpdateCommandName:
//                case RadGrid.DeleteCommandName:
//                    break;
//                case RadGrid.InitInsertCommandName:
//                case RadGrid.EditCommandName:
//                case RadGrid.SortCommandName:
//                default:
////                    gridInvoices.Rebind();
//                    break;
//            }
        }

        //protected void gridInvoices_InsertCommand(object source, GridCommandEventArgs e)
        //{
        //    GridEditableItem editedItem = e.Item as GridEditableItem;
        //    DataTable invoiceTable = InvoiceGridSource;

        //    DataRow newRow = invoiceTable.NewRow();

        //    //As this example demonstrates only in-memory editing, a new primary key value should be generated
        //    //This should not be applied when updating directly the database
        //    DataRow[] allValues = invoiceTable.Select("", "ID", DataViewRowState.CurrentRows);
        //    if (allValues.Length > 0)
        //    {
        //        newRow["ID"] = (int)allValues[allValues.Length - 1]["ID"] + 1;
        //    }
        //    else
        //    {
        //        newRow["ID"] = 1; //the table is empty;
        //    }

        //    //Set new values
        //    Hashtable newValues = new Hashtable();
        //    //The GridTableView will fill the values from all editable columns in the hash
        //    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

        //    try
        //    {
        //        foreach (DictionaryEntry entry in newValues)
        //        {
        //            newRow[(string)entry.Key] = entry.Value;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessageInvoice.Text += "Unable to insert into Invoice. Reason: " + ex.Message;
        //        e.Canceled = true;
        //    }

        //    invoiceTable.Rows.Add(newRow);
             
        //    //Code for updating the database ca go here...
        //    SaveInvoice(newRow);
        //    gridInvoices.Rebind();
        //}

        //protected void gridInvoices_UpdateCommand(object source, GridCommandEventArgs e)
        //{
        //    GridEditableItem editedItem = e.Item as GridEditableItem;
        //    DataTable invoiceTable = this.InvoiceGridSource;

        //    //Locate the changed row in the DataSource
        //    DataRow[] changedRows = invoiceTable.Select("ID = " + editedItem["ID"].Text);

        //    if (changedRows.Length != 1)
        //    {
        //        lblMessageInvoice.Text += "Unable to locate the Invoice for updating.";
        //        e.Canceled = true;
        //        return;
        //    }

        //    //Update new values
        //    Hashtable newValues = new Hashtable();
        //    //The GridTableView will fill the values from all editable columns in the hash
        //    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

        //    changedRows[0].BeginEdit();
        //    try
        //    {
        //        foreach (DictionaryEntry entry in newValues)
        //        {
        //            changedRows[0][(string)entry.Key] = entry.Value;
        //        }
        //        changedRows[0].EndEdit();
        //        SaveInvoice(changedRows[0]);
        //    }
        //    catch (Exception ex)
        //    {
        //        changedRows[0].CancelEdit();
        //        lblMessageInvoice.Text += "Unable to update Invoice. Reason: " + ex.Message;
        //        e.Canceled = true;
        //    }
        //}

/*        protected void gridPayoff_InsertCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            DataTable payoffTable = PayoffGridSource;

            DataRow newRow = payoffTable.NewRow();

            DataRow[] allValues = payoffTable.Select("", "ID", DataViewRowState.CurrentRows);
            if (allValues.Length > 0)
            {
                newRow["ID"] = (int)allValues[allValues.Length - 1]["ID"] + 1;
            }
            else
            {
                newRow["ID"] = 1; //the table is empty;
            }

            //Set new values
            Hashtable newValues = new Hashtable();
            //The GridTableView will fill the values from all editable columns in the hash
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

            try
            {
                foreach (DictionaryEntry entry in newValues)
                {
                    newRow[(string)entry.Key] = entry.Value;
                }
                RadDatePicker rdpExpDate = (RadDatePicker)editedItem.FindControl("rdpExpDate");
                if (rdpExpDate != null)
                {
                    newRow["ExpDate"] = rdpExpDate.SelectedDate;
                }

            }
            catch (Exception ex)
            {
                lblMessagePayoff.Text += "Unable to insert into Payoff. Reason: " + ex.Message;
                e.Canceled = true;
            }

            payoffTable.Rows.Add(newRow);

            //Code for updating the database ca go here...
            SavePayoff(newRow);
            gridPayoff.Rebind();
        }

        protected void gridPayoff_UpdateCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            DataTable payoffTable = this.PayoffGridSource;

            //Locate the changed row in the DataSource
            DataRow[] changedRows = payoffTable.Select("ID = " + editedItem["ID"].Text);

            if (changedRows.Length != 1)
            {
                lblMessagePayoff.Text += "Unable to locate the Payoff for updating.";
                e.Canceled = true;
                return;
            }

            //Update new values
            Hashtable newValues = new Hashtable();
            //The GridTableView will fill the values from all editable columns in the hash
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

            changedRows[0].BeginEdit();
            try
            {
                foreach (DictionaryEntry entry in newValues)
                {
                    changedRows[0][(string)entry.Key] = entry.Value;
                }
                RadDatePicker rdpExpDate = (RadDatePicker)editedItem.FindControl("rdpExpDate");
                if (rdpExpDate != null)
                {
                    changedRows[0]["ExpDate"] = rdpExpDate.SelectedDate;
                }


                changedRows[0].EndEdit();
                SavePayoff(changedRows[0]);
            }
            catch (Exception ex)
            {
                changedRows[0].CancelEdit();
                lblMessagePayoff.Text += "Unable to update Payoff. Reason: " + ex.Message;
                e.Canceled = true;
            }
        }

        protected void gridPayoff_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            gridPayoff.DataSource = PayoffGridSource;
        }

        private void SavePayoff(DataRow row)
        {
            Payoff spayoff = new Payoff();
            spayoff.ID = Convert.ToInt32(row["ID"]);
            spayoff.MortgageID = MortgageID;
            spayoff.Creditor = Convert.ToString(row["Creditor"]);
            spayoff.IN = Convert.ToBoolean(row["IN"]);
            spayoff.Ordered = Convert.ToBoolean(row["Ordered"]);
            spayoff.Amount = Convert.ToDecimal(row["Amount"]);
            spayoff.Perdiem = Convert.ToString(row["Perdiem"]);
            spayoff.ExpDate = Convert.ToDateTime(row["ExpDate"]);
            spayoff.CreatedBy = CurrentUser.Id;
            row["RemDays"] = spayoff.ExpDate.Subtract(DateTime.Now).Days;
            int id = spayoff.Save();
            if (id > 0)
                row["ID"] = id;
        }

        protected void gridPayoff_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == Telerik.WebControls.GridItemType.Item) || (e.Item.ItemType == Telerik.WebControls.GridItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if(e.Item.IsInEditMode)
                {
                    RadDatePicker rdpExpDate = (RadDatePicker)e.Item.FindControl("rdpExpDate");
                    if (rdpExpDate != null)
                    {
                        if (row["ExpDate"] != DBNull.Value)
                            rdpExpDate.SelectedDate = Convert.ToDateTime(row["ExpDate"]);
                    }
                }
            }
        }*/


    }
}