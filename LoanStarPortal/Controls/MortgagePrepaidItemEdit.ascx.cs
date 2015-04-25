using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class MortgagePrepaidItemEdit : EditGridFormControl
    {
        #region fields
        private MortgagePrepaidItem item;
        private DataView dvPrepaidUnits;
        private string objectName;
        #endregion

        #region properties
        public override string ObjectName
        {
            get { return objectName; }
            set { objectName = value; }
        }
        protected DataView DvPrepaidUnits
        {
            get
            {
                if (dvPrepaidUnits == null)
                {
                    dvPrepaidUnits = MortgagePrepaidItem.GetUnitList();
                }
                return dvPrepaidUnits;
            }
        }
        public override object EditObject
        {
            set {item = value as MortgagePrepaidItem;}
        }
        #endregion

        #region methods
        public override void BindData()
        {
            if (item !=null)
            {
                tbDescription.Text = item.Description;
                tbDescription.ReadOnly = !IsFieldEditable("Description");
                tbPaymentTo.Text = item.PaymentTo;
                tbPaymentTo.ReadOnly = !IsFieldEditable("PaymentTo");
                tbAmount.Text = item.Amount.ToString();
                tbAmount.ReadOnly = !IsFieldEditable("Amount");
                ddlUnit.DataSource = DvPrepaidUnits;
                ddlUnit.DataTextField = "name";
                ddlUnit.DataValueField = "id";
                ddlUnit.DataBind();
                ddlUnit.SelectedValue = item.UnitId.ToString();
                ddlUnit.Enabled = IsFieldEditable("UnitId");
                if (item.StatementStart == null)
                {
                    diStatementStart.SelectedDate = DateTime.Now;
                }
                else
                {
                    diStatementStart.SelectedDate = item.StatementStart;
                }
                diStatementStart.Enabled = IsFieldEditable("StatementStart");
                if (item.StatementEnd == null)
                {
                    diStatementEnd.SelectedDate = DateTime.Now;
                }
                else
                {
                    diStatementEnd.SelectedDate = item.StatementEnd;
                }
                diStatementEnd.Enabled = IsFieldEditable("StatementEnd");
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ArrayList logs = new ArrayList();
            string description = tbDescription.Text;
            if (item.Description != description)
            {
                logs.Add(new MortgageLogEntry(ObjectName, item.ID, ObjectName+".Description", item.Description, description, CurrentPage.CurrentUser.Id));
                item.Description = description;
            }
            string paymentTo = tbPaymentTo.Text;
            if(item.PaymentTo!=paymentTo)
            {
                logs.Add(new MortgageLogEntry(ObjectName, item.ID, ObjectName + ".PaymentTo", item.PaymentTo, paymentTo, CurrentPage.CurrentUser.Id));
                item.PaymentTo = tbPaymentTo.Text;
            }
            decimal amount = decimal.Parse(tbAmount.Text);
            if (item.Amount != amount)
            {
                logs.Add(new MortgageLogEntry(ObjectName, item.ID, ObjectName + ".Amount", item.Amount.ToString(), amount.ToString(), CurrentPage.CurrentUser.Id));
                item.Amount = amount;
            }
            DateTime d = (DateTime)diStatementStart.SelectedDate;
            if (item.StatementStart != d)
            {
                logs.Add(new MortgageLogEntry(ObjectName, item.ID, ObjectName + ".StatementStart", item.StatementStart.ToString(), d.ToString(), CurrentPage.CurrentUser.Id));
                item.StatementStart = d;
            }
            d = (DateTime)diStatementEnd.SelectedDate;
            if (item.StatementEnd != d)
            {
                logs.Add(new MortgageLogEntry(ObjectName, item.ID, ObjectName + ".StatementEnd", item.StatementEnd.ToString(), d.ToString(), CurrentPage.CurrentUser.Id));
                item.StatementEnd = d;
            }
            int unitId = Convert.ToInt32(ddlUnit.SelectedValue);
            if (item.UnitId != unitId)
            {
                logs.Add(new MortgageLogEntry(ObjectName, item.ID, ObjectName + ".UnitId", item.UnitId.ToString(), unitId.ToString(), CurrentPage.CurrentUser.Id));
                item.UnitId = unitId;
            }
            Save(item,logs);
        }
        #endregion

    }
}