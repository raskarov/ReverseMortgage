using System;
using System.Data;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;


namespace LoanStarPortal.Controls
{
    public partial class PropertyRepairItems : MortgageDataGridControl
    {

        #region constants
        private const string OBJECTNAME = "PropertyRepairItem";
        private const string ADDRECORDTEXT = "Add new repair item";
        private static readonly string[] gridFieldsAdd = { 
            "Description", "BidAmount","RepairStatusId","EstimateSourceId"
        };
        #endregion

        #region fields
        private PropertyRepairItem item;
        private readonly string objectName = OBJECTNAME;
        private readonly string addRecordText = ADDRECORDTEXT;
        private DataView dvGridData;
        private DataView dvRepairStatusList;
        private DataView dvEstimateSourceList;
        #endregion

        #region properties
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
                    MortgageProfile mp = CurrentPage.GetMortgage(MortgageId);
                    if (mp!=null)
                    {
                        dvGridData = PropertyRepairItem.GetRepairItemsList(mp.Property.ID);
                    }
                }
                return dvGridData;
            }
        }
        protected override string GridName
        {
            get { return gRepairItems.ID; }
        }
        protected override void ResetDataSource()
        {
            dvGridData = null;
        }
        protected DataView DvRepairStatusList
        {
            get
            {
                if (dvRepairStatusList == null)
                {
                    dvRepairStatusList = PropertyRepairItem.GetRepairStatusList();
                }
                return dvRepairStatusList;
            }
        }
        protected DataView DvEstimateSourceList
        {
            get
            {
                if (dvEstimateSourceList == null)
                {
                    dvEstimateSourceList = PropertyRepairItem.GetEstimateSourceList();
                }
                return dvEstimateSourceList;
            }
        }

        #endregion

        #region overriden methods
        protected override void Page_Load(object sender, EventArgs e)
        {
            G = gRepairItems;
            base.Page_Load(sender, e);
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageId);
            lblTotalRepairSetAsides.Text = String.Format("{0:c}", mp.MortgageInfo.TotalRepairSetAsides);
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
                }
            }
            for (int i = 0; i < gridFieldsAdd.Length; i++)
            {
                if (fields.ContainsKey(gridFieldsAdd[i]))
                {
                    canEdit |= !(bool)fields[gridFieldsAdd[i]];
                }
            }
        }
        protected override void G_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                int id = Convert.ToInt32(dr["id"].ToString());
                if (id > 0)
                {
                    item = new PropertyRepairItem(dr);
                }
                else
                {
                    item = new PropertyRepairItem(id);
                }
                if ((gridMode != MODEVIEW) && (currentRow==EditRowId))
                {
                    EditItemId = item.ID;
                    Label lbl;
                    TextBox tb;
                    DropDownList ddl;
                    RadNumericTextBox rnb;
                    RequiredFieldValidator rfv;
                    if (IsFieldEditable("Description"))
                    {
                        lbl = (Label)e.Row.FindControl("lblDescription");
                        if (lbl != null)
                        {
                            lbl.Visible = false;
                        }
                        tb = (TextBox)e.Row.FindControl("tbDescription");
                        if (tb != null)
                        {
                            tb.Visible = true;
                            tb.Text = item.Description;
                        }
                        rfv = (RequiredFieldValidator)e.Row.FindControl("rfvDescription");
                        if (rfv != null)
                        {
                            rfv.Visible = true;
                        }
                    }
                    if (IsFieldEditable("RepairStatusId"))
                    {
                        lbl = (Label)e.Row.FindControl("lblRepairStatus");
                        if (lbl != null)
                        {
                            lbl.Visible = false;
                        }
                        ddl = (DropDownList)e.Row.FindControl("ddlRepairStatus");
                        if (ddl != null)
                        {
                            ddl.Visible = true;
                            ddl.DataSource = DvRepairStatusList;
                            ddl.DataTextField = "name";
                            ddl.DataValueField = "id";
                            ddl.DataBind();
                            ddl.SelectedValue = item.RepairStatusId.ToString();
                        }
                        rfv = (RequiredFieldValidator)e.Row.FindControl("rfvRepairStatus");
                        if (rfv != null)
                        {
                            rfv.Visible = true;
                        }
                    }
                    if (IsFieldEditable("BidAmount"))
                    {
                        lbl = (Label)e.Row.FindControl("lblBidAmount");
                        if (lbl != null)
                        {
                            lbl.Visible = false;
                        }
                        rnb = (RadNumericTextBox)e.Row.FindControl("tbBidAmount");
                        if (rnb != null)
                        {
                            rnb.Visible = true;
                            rnb.Text = item.BidAmount.ToString();
                        }
                        rfv = (RequiredFieldValidator)e.Row.FindControl("rfvBidAmount");
                        if (rfv != null)
                        {
                            rfv.Visible = true;
                        }
                    }
                    if (IsFieldEditable("EstimateSourceId"))
                    {
                        lbl = (Label)e.Row.FindControl("lblEstimateSource");
                        if (lbl != null)
                        {
                            lbl.Visible = false;
                        }
                        ddl = (DropDownList)e.Row.FindControl("ddlEstimateSource");
                        if (ddl != null)
                        {
                            ddl.Visible = true;
                            ddl.DataSource = DvEstimateSourceList;
                            ddl.DataTextField = "name";
                            ddl.DataValueField = "id";
                            ddl.DataBind();
                            ddl.SelectedValue = item.EstimateSourceId.ToString();
                        }
                        rfv = (RequiredFieldValidator)e.Row.FindControl("rfvEstimateSource");
                        if (rfv != null)
                        {
                            rfv.Visible = true;
                        }
                    }
                    SetActionColumn(e, true, true);
                }
                else
                {
                    if (gridMode!=MODEVIEW) SetActionColumn(e, false, true);
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
        protected override void Update(int rowIndex)
        {
            ArrayList logs = new ArrayList();
            item = new PropertyRepairItem(EditItemId);
            TextBox tb = (TextBox)gRepairItems.Rows[rowIndex].FindControl("tbDescription");
            if (tb != null)
            {
                string description = tb.Text;
                if (item.Description != description)
                {
                    logs.Add(new MortgageLogEntry("RepairItem", item.ID, "RepairItem.Description", item.Description, description, CurrentPage.CurrentUser.Id));
                    item.Description = description;
                }
            }
            DropDownList ddl = (DropDownList)gRepairItems.Rows[rowIndex].FindControl("ddlRepairStatus");
            if (ddl != null)
            {
                int statusid = Convert.ToInt32(ddl.SelectedValue);
                if (statusid != item.RepairStatusId)
                {
                    logs.Add(new MortgageLogEntry("RepairItem", item.ID, "RepairItem.RepairStatusId", item.RepairStatusId.ToString(), statusid.ToString(), CurrentPage.CurrentUser.Id));
                    item.RepairStatusId = statusid;
                }
            }
            RadNumericTextBox rntb = (RadNumericTextBox)gRepairItems.Rows[rowIndex].FindControl("tbBidAmount");
            if ((rntb != null) && (rntb.Visible))
            {
                decimal amount = decimal.Parse(rntb.Text);
                if (item.BidAmount != amount)
                {
                    logs.Add(new MortgageLogEntry("RepairItem", item.ID, "RepairItem.BidAmount", item.BidAmount.ToString(), amount.ToString(), CurrentPage.CurrentUser.Id));
                    item.BidAmount = amount;
                }
            }
            ddl = (DropDownList)gRepairItems.Rows[rowIndex].FindControl("ddlEstimateSource");
            if (ddl != null)
            {
                int sourceid = Convert.ToInt32(ddl.SelectedValue);
                if (sourceid != item.EstimateSourceId)
                {
                    logs.Add(new MortgageLogEntry("RepairItem", item.ID, "RepairItem.EstimateSourceId", item.EstimateSourceId.ToString(), sourceid.ToString(), CurrentPage.CurrentUser.Id));
                    item.EstimateSourceId = sourceid;
                }
            }
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageId);
            if (mp!=null)
            {
                item.PropertyId = mp.Property.ID;
                mp.SavePropertyRepairItemWithLog(item, logs);
                lblTotalRepairSetAsides.Text = String.Format("{0:c}", mp.MortgageInfo.TotalRepairSetAsides);
            }
        }
        public void AddFields(Control ctl)
        {
            trfields.Cells[0].Controls.Add(ctl);
        }
        public void HideGrid()
        {
            trrepairitems.Visible = false;
            trtotal.Visible = false;
        }

        #endregion

        #region old code
        //#region constants
        //private const string REPAIRITEMSOBJECTNAME = "PropertyRepairItem";
        //private const int MODEVIEW = 0;
        //private const int MODEEDIT = 1;
        //private const int MODEADD = 2;
        //private const string GRIDMODEVIEW = "repairitemsmode";
        //private const string ADDNEWBUTTONID = "lbAddItem";
        //private const string EDITITEMID = "repairitemgriditem";
        //private const string EDITBUTTONID = "imgEdit";
        //private const string UPDATEBUTTONID = "imgUpdate";
        //private const string ADDNEWIMAGEID = "imgInsert";
        //private static readonly string[] repairAdd = { 
        //    "Description", "BidAmount"
        //};
        //#endregion

        //#region fields
        //private ArrayList objectFields;
        //private readonly Hashtable fields = new Hashtable();
        //private bool canEdit = true;
        //private bool canAddNew = true;
        //private PropertyRepairItem repairItem;
        //private ArrayList logs = null;
        //private MortgageProfile mp;
        //#endregion

        //#region properties
        //protected int gridMode
        //{
        //    get
        //    {
        //        int res = MODEVIEW;
        //        Object o = Session[GRIDMODEVIEW];
        //        if (o != null)
        //        {
        //            try
        //            {
        //                res = Convert.ToInt32(o);
        //            }
        //            catch { }
        //        }
        //        return res;
        //    }
        //    set
        //    {
        //        Session[GRIDMODEVIEW] = value;
        //    }
        //}
        //private int MortgageId
        //{
        //    get
        //    {
        //        return Convert.ToInt32(Session[Constants.MortgageID]);
        //    }
        //}
        //protected bool CanAddNew
        //{
        //    get { return canAddNew; }
        //}
        //protected bool CanEdit
        //{
        //    get { return canEdit; }
        //}
        //protected int EditItemId
        //{
        //    get
        //    {
        //        int res = -1;
        //        Object o = Session[EDITITEMID];
        //        if (o != null)
        //        {
        //            try
        //            {
        //                res = Convert.ToInt32(o.ToString());
        //            }
        //            catch { }
        //        }
        //        return res;
        //    }
        //    set
        //    {
        //        Session[EDITITEMID] = value;
        //    }
        //}
        //#endregion

        //#region methods
        //private void CheckFieldsForEdit()
        //{
        //    if (mp != null)
        //    {
        //        objectFields = mp.GetObjectFields(REPAIRITEMSOBJECTNAME);
        //        if (objectFields != null)
        //        {
        //            for (int i = 0; i < objectFields.Count; i++)
        //            {
        //                MortgageProfileField mpf = (MortgageProfileField)objectFields[i];
        //                if (mpf != null)
        //                {
        //                    fields.Add(mpf.FullPropertyName.Replace(REPAIRITEMSOBJECTNAME + ".", ""), mpf.ReadOnly);
        //                }
        //            }
        //        }
        //        canAddNew = true;
        //        canEdit = false;
        //        for (int i = 0; i < repairAdd.Length; i++)
        //        {
        //            if (fields.ContainsKey(repairAdd[i]))
        //            {
        //                canAddNew &= !(bool)fields[repairAdd[i]];
        //            }
        //        }
        //        for (int i = 0; i < repairAdd.Length; i++)
        //        {
        //            if (fields.ContainsKey(repairAdd[i]))
        //            {
        //                canEdit |= !(bool)fields[repairAdd[i]];
        //            }
        //        }
        //    }
        //}
        //private void CheckCommand()
        //{
        //    string postBackControl = Page.Request.Form["__EVENTTARGET"];
        //    if (!String.IsNullOrEmpty(postBackControl))
        //    {
        //        if (postBackControl.EndsWith(":" + ADDNEWBUTTONID) || postBackControl.EndsWith(":"+ADDNEWIMAGEID))
        //        {
        //            gridMode = MODEADD;
        //            EditItemId = -1;
        //            repairItem = new PropertyRepairItem(EditItemId);
        //        }
        //        else if (postBackControl.EndsWith("_" + EDITBUTTONID))
        //        {
        //            string s = postBackControl.Replace("_" + EDITBUTTONID, "");
        //            int i = s.LastIndexOf("_");
        //            if (i > 0)
        //            {
        //                s = s.Substring(i + 1);
        //                try
        //                {
        //                    EditItemId = Convert.ToInt32(s);
        //                    gridMode = MODEEDIT;
        //                }
        //                catch
        //                {
        //                    EditItemId = -1;
        //                }
        //            }
        //        }
        //        else if (postBackControl.EndsWith(":" + UPDATEBUTTONID))
        //        {
        //            SaveData();
        //            gridMode = MODEVIEW;
        //        }
        //        else 
        //        {
        //            gridMode = MODEVIEW;
        //        }
        //    }
        //    else
        //    {
        //        gridMode = MODEVIEW;
        //    }
        //}
        //private bool IsFieldEditable(string fieldName)
        //{

        //    bool res = true;
        //    if (fields.ContainsKey((fieldName)))
        //        res = !(bool)fields[fieldName];
        //    return res;
        //}
        //private void SetControls(Control e)
        //{
        //    if (IsFieldEditable("Description"))
        //    {
        //        TextBox tb = (TextBox)e.FindControl("tbDescription");
        //        Label lbl = (Label)e.FindControl("lblDescription");
        //        if (tb != null)
        //        {
        //            tb.Text = repairItem.Description;
        //            tb.Visible = true;
        //        }
        //        if (lbl != null)
        //        {
        //            lbl.Visible = false;
        //        }
        //    }
        //    if (IsFieldEditable("BidAmount"))
        //    {
        //        RadNumericTextBox rtb = (RadNumericTextBox)e.FindControl("tbBidAmount");
        //        Label lbl = (Label)e.FindControl("lblBidAmount");
        //        if (rtb != null)
        //        {
        //            rtb.Text = repairItem.BidAmount.ToString();
        //            rtb.Visible = true;
        //        }
        //        if (lbl != null)
        //        {
        //            lbl.Visible = false;
        //        }
        //    }
        //    ImageButton btn = (ImageButton)e.FindControl("imgCancel");
        //    if (btn != null)
        //    {
        //        btn.Visible = true;
        //    }
        //    btn = (ImageButton)e.FindControl("imgUpdate");
        //    if (btn != null)
        //    {
        //        btn.Visible = true;
        //    }
        //    btn = (ImageButton)e.FindControl("imgEdit");
        //    if (btn != null)
        //    {
        //        btn.Visible = false;
        //    }
        //}
        //private void SaveData()
        //{
        //    repairItem = new PropertyRepairItem(EditItemId);
        //    logs = new ArrayList();
        //    string description = GetPostedValue("tbDescription");
        //    if (repairItem.Description != description)
        //    {
        //        logs.Add(new MortgageLogEntry("PropertyRepairItem", repairItem.ID, "PropertyRepairItem.Description", repairItem.Description, description, CurrentPage.CurrentUser.Id));
        //    }
        //    repairItem.Description = description;
        //    string samount = GetPostedValue("tbBidAmount");
        //    if (samount!="")
        //    {
        //        decimal amount;
        //        try
        //        {
        //            amount = decimal.Parse(samount);
        //            if (repairItem.BidAmount != amount)
        //            {
        //                logs.Add(new MortgageLogEntry("PropertyRepairItem", repairItem.ID, "PropertyRepairItem.BidAmount", repairItem.BidAmount.ToString(), amount.ToString(), CurrentPage.CurrentUser.Id));
        //            }
        //            repairItem.BidAmount = amount;
        //        }
        //        catch
        //        {
        //        }
        //    }
        //    if (logs.Count>0)
        //    {
        //        if (mp != null)
        //        {
        //            repairItem.PropertyId = mp.Property.ID;
        //            mp.SavePropertyRepairItemWithLog(repairItem, logs);
        //        }
        //    }
        //}
        //private string GetPostedValue(string controlName)
        //{
        //    string res = String.Empty;
        //    for (int i = 0; i < Page.Request.Form.AllKeys.Length;i++)
        //    {
        //        string key = Page.Request.Form.AllKeys[i];
        //        if (key.EndsWith("$"+controlName))
        //        {
        //            res = Page.Request.Form[key];
        //            break;
        //        }
        //    }
        //    return res;
        //}
        //private void BindGrid()
        //{
        //    DataView dvGridData = mp.Property.ReloadRepairItems(); 
        //    if (gridMode == MODEADD)
        //    {
        //        dvGridData.RowFilter = String.Empty;
        //    }
        //    else
        //    {
        //        dvGridData.RowFilter = "id>0";
        //    }
        //    gRepairItems.MasterTableView.CommandItemDisplay = gridMode == MODEVIEW ? GridCommandItemDisplay.Bottom : GridCommandItemDisplay.None;
        //    gRepairItems.DataSource = dvGridData;
        //    gRepairItems.DataBind();
        //}
        //#endregion

        //#region databinding
        //protected void RpGrid_ItemDataBound(object sender, GridItemEventArgs e)
        //{
        //    if (gridMode == MODEVIEW)
        //    {
        //        if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
        //        {
        //            ImageButton btn = (ImageButton)e.Item.FindControl(EDITBUTTONID);
        //            if (btn != null)
        //            {
        //                DataRowView dr = e.Item.DataItem as DataRowView;
        //                if (dr != null)
        //                {
        //                    btn.ID = "Id_" + Convert.ToInt32(dr["id"]) + "_" + EDITBUTTONID;
        //                }
        //            }
        //        }
        //        return;
        //    }
        //    if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
        //    {
        //        DataRowView dr = e.Item.DataItem as DataRowView;
        //        if (dr != null)
        //        {
        //            int id = Convert.ToInt32(dr["id"]);
        //            if (id == EditItemId)
        //            {
        //                repairItem = new PropertyRepairItem(dr);
        //                SetControls(e.Item);
        //            }
        //            else
        //            {
        //                ImageButton btn = (ImageButton)e.Item.FindControl(EDITBUTTONID);
        //                if (btn != null)
        //                {
        //                    btn.Visible = false;
        //                }
        //            }
        //        }
        //    }
        //}
        //protected static string GetAmount(Object o)
        //{
        //    string result = String.Empty;
        //    if (o is GridInsertionObject)
        //    {
        //        return result;
        //    }
        //    DataRowView row = (DataRowView)o;
        //    if (row != null)
        //    {
        //        decimal amount = Convert.ToDecimal(row["BidAmount"].ToString());
        //        result = amount.ToString("C");
        //    }
        //    return result;
        //}
        //#endregion

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    mp = CurrentPage.GetMortgage(MortgageId);
        //    CheckFieldsForEdit();
        //    CheckCommand();
        //    BindGrid();
        //}
        #endregion
    }
}