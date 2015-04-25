using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class PayoffEdit : EditGridFormControl
    {
        private const string NOTSELECTEDTEXT = "-Select-";
        private const int NOTSELECTEDVALUE = 0;
        protected const string ONBLUR = "onblur";

        #region fields
        private Payoff payoff;
        private string objectName;
        private DataView dvPayoffStatus;
        #endregion

        #region properties
        protected DataView DvPayoffStatus
        {
            get
            {
                if (dvPayoffStatus == null)
                {
                    dvPayoffStatus = Payoff.GetPayOffStatusList();
                }
                return dvPayoffStatus;
            }
        }
        public override string ObjectName
        {
            get { return objectName; }
            set { objectName = value; }
        }
        public override object EditObject
        {
            set { payoff = value as Payoff; }
        }
        #endregion

        public override void BindData()
        {
            tbCreditor.Text = payoff.Creditor;
            tbCreditor.ReadOnly = !IsFieldEditable("Creditor");
            tbAmount.Text = payoff.Amount.ToString();
            tbAmount.ReadOnly = !IsFieldEditable("Amount");
            diExpDate.SelectedDate = payoff.ExpDate;
            diExpDate.Enabled = IsFieldEditable("ExpDate");
            if (payoff.Perdiem>0)
            {
                tbPerdiem.Text = payoff.Perdiem.ToString();
            }
            tbPerdiem.ReadOnly = !IsFieldEditable("Perdiem");
            tbAddress1.Text = payoff.Address1;
            tbAddress1.ReadOnly = !IsFieldEditable("Address1");
            tbAddress2.Text = payoff.Address2;
            tbAddress2.ReadOnly = !IsFieldEditable("Address2");
            tbCity.Text = payoff.City;
            tbCity.ReadOnly = !IsFieldEditable("City");
            tbZip.Text = payoff.Zip;
            tbZip.ReadOnly = !IsFieldEditable("Zip");
            tbAccountNumber.Text = payoff.AccountNumber;
            tbAccountNumber.ReadOnly = !IsFieldEditable("AccountNumber");
            tbPoc.Text = payoff.POC.ToString();
            tbPoc.ReadOnly = !IsFieldEditable("POC");
            ddlState.DataSource = CurrentPage.GetDictionary("vwState");
            ddlState.DataTextField = "name";
            ddlState.DataValueField = "id";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem(NOTSELECTEDTEXT, NOTSELECTEDVALUE.ToString()));
            ddlState.SelectedValue = payoff.StateId.ToString();
            ddlState.Enabled = IsFieldEditable("StateId");
            ddlStatus.DataSource = DvPayoffStatus;
            ddlStatus.DataTextField = "name";
            ddlStatus.DataValueField = "id";
            ddlStatus.DataBind();
            ddlStatus.SelectedValue = payoff.PayoffCalculatedStatusId.ToString();
            ddlStatus.Enabled = IsFieldEditable("PayOffStatusId");
            SetFieldsVisibility(payoff.PayoffStatusId > Payoff.ORDEREDSTATUSID);
            SetValidators(payoff.PayoffStatusId == Payoff.RECEIVEDSTATUSID);
        }
        private void SetValidators(bool isVisible)
        {
            rfAccountNumber.Visible = isVisible;
            rfPerdiem.Visible = isVisible;
            rfExpDate.Visible = isVisible;
        }
        private void SetFieldsVisibility(bool visible)
        {
            tbPerdiem.Visible = visible;
            diExpDate.Visible = visible;
            lblExpDate.Visible = visible;
            lblPerdiem.Visible = visible;
            lblAmountPoc.Visible = visible;
            tbPoc.Visible = visible;
            lblFinancedAmount1.Visible = !lblExpDate.Visible;
            lblFinancedAmountValue1.Visible = lblFinancedAmount1.Visible;
            lblFinancedAmount2.Visible = !lblFinancedAmount1.Visible;
            lblFinancedAmountValue2.Visible = lblFinancedAmount2.Visible;
            lblFinancedAmountValue1.Text = payoff.Financed.ToString("C");
            lblFinancedAmountValue2.Text = payoff.Financed.ToString("C");
            string js = String.Format("AmountFocusLost(this,'{0}','{1}',{2});", tbAmount.ID, (visible?tbPoc.ID:""), payoff.POC);
            tbAmount.Attributes.Add(ONBLUR,js);
            if(visible)
            {
                js = String.Format("PocAmountFocusLost(this,'{0}','{1}');", tbPoc.ID, tbAmount.ID);
                tbPoc.Attributes.Add(ONBLUR, js);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ArrayList logs = new ArrayList();
            string creditor = tbCreditor.Text;
            if (payoff.Creditor != creditor)
            {
                logs.Add(new MortgageLogEntry("Payoff", payoff.ID, "Payoff.Creditor", payoff.Creditor, creditor, CurrentPage.CurrentUser.Id));
                payoff.Creditor = creditor;
            }
            int statusId = int.Parse(ddlStatus.SelectedValue);
            if(statusId!=payoff.PayoffCalculatedStatusId)
            {
                logs.Add(new MortgageLogEntry("Payoff", payoff.ID, "Payoff.PayoffStatusId", payoff.PayoffStatusId.ToString(), statusId.ToString(), CurrentPage.CurrentUser.Id));
                payoff.PayoffStatusId = statusId;
            }
            decimal amount = Convert.ToDecimal(tbAmount.Text);
            if(amount!=payoff.Amount)
            {
                logs.Add(new MortgageLogEntry("Payoff", payoff.ID, "Payoff.Amount", payoff.Amount.ToString(), amount.ToString(), CurrentPage.CurrentUser.Id));
                payoff.Amount = amount;
            }
            if(tbPerdiem.Visible)
            {
                decimal perdiem = 0;
                if(!String.IsNullOrEmpty(tbPerdiem.Text))
                {
                    perdiem = Convert.ToDecimal(tbPerdiem.Text);
                }
                if (payoff.Perdiem != perdiem)
                {
                    logs.Add(new MortgageLogEntry("Payoff", payoff.ID, "Payoff.Perdiem", payoff.Perdiem.ToString(), perdiem.ToString(), CurrentPage.CurrentUser.Id));
                    payoff.Perdiem = perdiem;
                }
            }
            if((diExpDate.Visible)&&(diExpDate.SelectedDate!=null))
            {
                DateTime expdate = (DateTime)diExpDate.SelectedDate;
                if (expdate != payoff.ExpDate)
                {
                    logs.Add(new MortgageLogEntry("Payoff", payoff.ID, "Payoff.ExpDate", payoff.ExpDate.ToString(), expdate.ToString(), CurrentPage.CurrentUser.Id));
                    payoff.ExpDate = expdate;
                }
            }
            string address = tbAddress1.Text;
            if(payoff.Address1!=address)
            {
                logs.Add(new MortgageLogEntry("Payoff", payoff.ID, "Payoff.Address1", payoff.Address1, address, CurrentPage.CurrentUser.Id));
                payoff.Address1 = address;
            }
            address = tbAddress2.Text;
            if (payoff.Address2 != address)
            {
                logs.Add(new MortgageLogEntry("Payoff", payoff.ID, "Payoff.Address2", payoff.Address2, address, CurrentPage.CurrentUser.Id));
                payoff.Address2 = address;
            }
            string city = tbCity.Text;
            if(payoff.City!=city)
            {
                logs.Add(new MortgageLogEntry("Payoff", payoff.ID, "Payoff.City", payoff.City, city, CurrentPage.CurrentUser.Id));
                payoff.City = city;
            }
            string zip = tbZip.Text;
            if(payoff.Zip!=zip)
            {
                logs.Add(new MortgageLogEntry("Payoff", payoff.ID, "Payoff.Zip", payoff.Zip, zip, CurrentPage.CurrentUser.Id));
                payoff.Zip = zip;
            }
            string accountnumber = tbAccountNumber.Text;
            if(payoff.AccountNumber!=accountnumber)
            {
                logs.Add(new MortgageLogEntry("Payoff", payoff.ID, "Payoff.AccountNumber", payoff.AccountNumber, accountnumber, CurrentPage.CurrentUser.Id));
                payoff.AccountNumber = accountnumber;
            }
            int stateId;
            if (!String.IsNullOrEmpty(ddlState.SelectedValue) && int.TryParse(ddlState.SelectedValue, out stateId))
            {
                if (payoff.StateId != stateId)
                {
                    logs.Add(new MortgageLogEntry("Payoff", payoff.ID, "Payoff.StateId", payoff.StateId.ToString(), stateId.ToString(), CurrentPage.CurrentUser.Id));
                    payoff.StateId = stateId;
                }
            }
            decimal pocAmount = Convert.ToDecimal(tbPoc.Text);
            if (pocAmount != payoff.POC)
            {
                logs.Add(new MortgageLogEntry("Payoff", payoff.ID, "Payoff.POC", payoff.POC.ToString(), pocAmount.ToString(), CurrentPage.CurrentUser.Id));
                payoff.POC = pocAmount;
            }
            if(payoff.ID<1)
            {
                payoff.CreatedBy = CurrentUser.Id;
            }
            Save(payoff, logs);
        }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFieldsVisibility(int.Parse(ddlStatus.SelectedValue) > Payoff.ORDEREDSTATUSID);
            SetValidators(int.Parse(ddlStatus.SelectedValue) == Payoff.RECEIVEDSTATUSID);
        }
    }
}