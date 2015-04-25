using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class VendorContactsEdit : EditGridFormControl
    {
        #region fields
        private VendorContact contact;
        private string objectName;
        private VendorGlobal vendor;
        #endregion

        #region properties
        public VendorGlobal Vendor
        {
            get
            {
                if (vendor == null)
                {
                    vendor = new VendorGlobal(contact.VendorId);
                }
                return vendor;
            }
        }
        public override object EditObject
        {
            set { contact = value as VendorContact; }
        }
        public override string ObjectName
        {
            get { return objectName; }
            set { objectName = value; }
        }
        #endregion

        #region methods
        public override void BindData()
        {
            tbName.Text = contact.Name;
            tbPnone.Text = contact.Phone;
            tbAltPhone.Text = contact.AltPhone;
            tbCellPhone.Text = contact.CellPhone;
            tbFax.Text = contact.Fax;
            tbAddress.Text = contact.Address;
            tbEmail.Text = contact.Email;
            cbIsSettlementAgent.Checked = contact.IsSettlementAgent;
            cbIsTitleAgent.Checked = contact.IsTitleAgent;
            trTitleAgent.Visible=Vendor.IsServedFeeType(VendorGlobal.TITLEINSURANCEFEETYPE);
            trSettlementAgent.Visible=Vendor.IsServedFeeType(VendorGlobal.SETTLEMENTORCLOSINGFEETYPE);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            contact.Name = tbName.Text;
            contact.Phone = tbPnone.Text;
            contact.AltPhone = tbAltPhone.Text;
            contact.CellPhone = tbCellPhone.Text;
            contact.Fax = tbFax.Text;
            contact.Address = tbAddress.Text;
            contact.Email = tbEmail.Text;
            if (trSettlementAgent.Visible)
            {
                contact.IsSettlementAgent = cbIsSettlementAgent.Checked;
            }
            if (trTitleAgent.Visible)
            {
                contact.IsTitleAgent = cbIsTitleAgent.Checked;
            }            
            Save(contact, new ArrayList());
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }

    }
}