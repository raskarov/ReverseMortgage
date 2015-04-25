using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class ReserveEdit : EditGridFormControl
    {
        private const int OTHERTYPEID = 2;
        #region fields
        private Reserve reserve;
        private string objectName;
        private DataView dvChargeToList;
        private DataView dvTypeList;
        #endregion

        #region properties
        public override string ObjectName
        {
            get { return objectName; }
            set { objectName = value; }
        }
        public override object EditObject
        {
            set { reserve = value as Reserve; }
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
        protected DataView DvTypeList
        {
            get
            {
                if (dvTypeList == null)
                {
                    dvTypeList = Reserve.GetReserveTypeList();
                }
                return dvTypeList;
            }
        }
        #endregion

        #region Methods
        protected void AddEmptyItem(DropDownList ddl)
        {
            ListItem li = new ListItem("Select", "");
            ddl.Items.Insert(0, li);
        }

        private void BindDropDown(DropDownList ddl, DataView dv, string textfield, string valuefield, string selectedvalue)
        {
            ddl.DataSource = dv;
            ddl.DataTextField = textfield;
            ddl.DataValueField = valuefield;
            ddl.DataBind();
            AddEmptyItem(ddl);
            if (!String.IsNullOrEmpty(selectedvalue))
            {
                ddl.ClearSelection();
                ListItem li = ddl.Items.FindByValue(selectedvalue);
                if (li != null) li.Selected = true;
            }
        }
        public override void BindData()
        {
            if (reserve != null)
            {
                if (reserve.TypeId == OTHERTYPEID)
                {
                    trDescription.Visible = true;
                    Description.Text = reserve.Description;
                    Description.ReadOnly = !IsFieldEditable("Description");
                }
                else
                {
                    trDescription.Visible = false;
                }

                BindDropDown(ddlType, DvTypeList, "Name", "ID", reserve.TypeId.ToString());

                Months.Text = reserve.Months.ToString();
                Months.ReadOnly = !IsFieldEditable("Months");

                if (reserve.StatementStart!=null)
                {
                    StatStart.SelectedDate = reserve.StatementStart;
                    StatStart.ReadOnly = !IsFieldEditable("StatementStart");
                }
                if (reserve.StatementEnd != null)
                {
                    StatEnd.SelectedDate = reserve.StatementEnd;
                    StatEnd.ReadOnly = !IsFieldEditable("StatementEnd");
                }

                BindDropDown(ddlChargeTo, DvChargeToList, "Name", "ID", reserve.ChargeToId.ToString());
                ddlChargeTo.Enabled = IsFieldEditable("ChargeToId");

                Amount.Text = reserve.Amount.ToString();
                Amount.Enabled = IsFieldEditable("Amount");
            }
        }
        #endregion

        #region Event Handlers
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ArrayList logs = new ArrayList();

            int typeId = Convert.ToInt32(ddlType.SelectedValue);
            if (reserve.TypeId != typeId)
            {
                logs.Add(
                    new MortgageLogEntry(ObjectName, reserve.ID, ObjectName + ".TypeId",
                                         reserve.TypeId.ToString(), typeId.ToString(),
                                         CurrentPage.CurrentUser.Id));
                reserve.TypeId = typeId;
            }

            if(!String.IsNullOrEmpty(Months.Text))
            {
                int months = Convert.ToInt32(Months.Text);
                if (reserve.Months != months)
                {
                    logs.Add(
                        new MortgageLogEntry(ObjectName, reserve.ID, ObjectName + ".Months", reserve.Months.ToString(), months.ToString(), CurrentPage.CurrentUser.Id));
                    reserve.Months = months;
                }
            }
            if (trDescription.Visible)
            {
                string description = Description.Text;
                if (reserve.Description != description)
                {
                    logs.Add(
                        new MortgageLogEntry(ObjectName, reserve.ID, ObjectName + ".Description", reserve.Description,
                                             description, CurrentPage.CurrentUser.Id));
                    reserve.Description = description;
                }
            }

            if(StatStart.SelectedDate!=null)
            {
                DateTime start = (DateTime) StatStart.SelectedDate;
                if (reserve.StatementStart != start)
                {
                    logs.Add(
                        new MortgageLogEntry(ObjectName, reserve.ID, ObjectName + ".StatementStart", reserve.StatementStart.ToString(),start.ToString(), CurrentPage.CurrentUser.Id));
                    reserve.StatementStart = start;
                }
            }
            if (StatEnd.SelectedDate != null)
            {
                DateTime end = (DateTime)StatEnd.SelectedDate;
                if (reserve.StatementEnd != end)
                {
                    logs.Add(
                        new MortgageLogEntry(ObjectName, reserve.ID, ObjectName + ".StatementEnd", reserve.StatementEnd.ToString(), end.ToString(), CurrentPage.CurrentUser.Id));
                    reserve.StatementEnd = end;
                }
            }

            decimal amount = decimal.Parse(Amount.Text);
            if (reserve.Amount != amount)
            {
                logs.Add(
                    new MortgageLogEntry(ObjectName, reserve.ID, ObjectName + ".Amount", reserve.Amount.ToString(),
                                         amount.ToString(), CurrentPage.CurrentUser.Id));
                reserve.Amount = amount;
            }

            int chargeToId = Convert.ToInt32(ddlChargeTo.SelectedValue);
            if (reserve.ChargeToId != chargeToId)
            {
                logs.Add(
                    new MortgageLogEntry(ObjectName, reserve.ID, ObjectName + ".ChargeToId",
                                         reserve.ChargeToId.ToString(), chargeToId.ToString(),
                                         CurrentPage.CurrentUser.Id));
                reserve.ChargeToId = chargeToId;
            }

            Save(reserve, logs);
        }
        #endregion

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlType.SelectedValue == OTHERTYPEID.ToString())
                trDescription.Visible = true;
            else
                trDescription.Visible = false;
        }
    }
}