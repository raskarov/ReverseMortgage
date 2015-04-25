using System;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using LoanStar.Common;


namespace LoanStarPortal.Controls
{
    public partial class VendorContacts : MortgageDataGridControl
    {
        #region constants
        private const string OBJECTNAME = "VendorContacts";
        private const string ADDRECORDTEXT = "Add new contact";
        private const string EDITFORMCONTROLNAME = "VendorContactsEdit.ascx";
        private const string PAGEINDEX = "vendorcontactpageindex";
        private const string ONCLICK = "onclick";
        private const string DELETEJS = "javascript:{{var r=confirm('Delete this Contact?');if (!r)return false;}};";
        #endregion

        #region fields
        private readonly string objectName = OBJECTNAME;
        private readonly string addRecordText = ADDRECORDTEXT;
        private DataView dvGridData;
        private VendorGlobal vendor;
        private VendorContact contact;
        #endregion

        #region properties
        private int PageIndex
        {
            get
            {
                int res = 0;
                Object o = Session[PAGEINDEX];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch { }                    
                }
                return res;
            }
            set
            {
                Session[PAGEINDEX] = value;
            }
        }
        private int VendorId
        {
            get
            {
                int res = -1;
                Object o = Session[ViewVendors.VENDORID];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
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
        protected override object EditObject
        {
            get
            {
                if (contact == null)
                {
                    contact = new VendorContact(-1);
                    contact.OriginatorId = CurrentUser.CompanyId;
                    contact.VendorId = VendorId;
                }
                return contact;
            }
        }

        protected override DataView DvGridData
        {
            get
            {
                if (dvGridData == null)
                {
                    dvGridData = VendorContact.GetVendorContacts(CurrentUser.CompanyId,VendorId);
                }
                return dvGridData;
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
        private VendorGlobal Vendor
        {
            get
            {
                if (vendor == null)
                {
                    vendor = new VendorGlobal(VendorId);
                }
                return vendor;
            }
        }
        #endregion

        #region methods
        protected override void ResetDataSource()
        {
            dvGridData = null;
            gridMode = MODEVIEW;
        }
        protected override void BindGrid()
        {
            gContacts.PageIndex = PageIndex;
            gContacts.PagerSettings.Visible = gridMode == MODEVIEW;
            base.BindGrid();
        }
        protected override void G_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gridMode == MODEVIEW)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    if (btnDelete != null)
                    {
                        btnDelete.Attributes.Add(ONCLICK, DELETEJS);
                    } 
                }
                if (gridMode != MODEVIEW)
                {
                    if (currentRow == EditRowId)
                    {
                        VendorContact item;
                        DataRowView dr = (DataRowView)e.Row.DataItem;
                        int id = Convert.ToInt32(dr["id"].ToString());
                        if (id > 0)
                        {
                            item = new VendorContact(dr);
                        }
                        else
                        {
                            item = new VendorContact(-1);
                        }
                        item.OriginatorId = CurrentUser.CompanyId;
                        item.VendorId = VendorId;
                        contact = item;
                        EditItemId = item.ID;
                        SetActionColumn(e, false, false);
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
                    CreateFooter(e, 0);
                }
            }
        }
        protected override void G_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == DELETECOMMAND)
            {
                VendorContact.Delete(Convert.ToInt32(e.CommandArgument));
                ResetDataSource();
                BindGrid();
            }
            else if (e.CommandName==PAGECOMMAND)
            {    
                int index =   Convert.ToInt32(e.CommandArgument.ToString())-1;      
                gContacts.PageIndex = index;
                PageIndex = index;
                BindGrid();
            }
            else
            {
                base.G_RowCommand(sender, e);
            }
        }
        protected override void Save(object o, ArrayList logs)
        {
            contact.Save();
            base.Save(o, logs);
            dvGridData = null;
            BindGrid();
        }
        #endregion
        public void BindData()
        {
            canAddNew = true;
            lblVendorName.Text = Vendor.Name;
            G = gContacts;
            BindGrid();
        }
        public void ResetData()
        {
            Session.Remove(PAGEINDEX);
        }
        protected override void Page_Load(object sender, EventArgs e)
        {
        }
    }
}