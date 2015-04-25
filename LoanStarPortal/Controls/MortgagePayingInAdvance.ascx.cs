using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class MortgagePayingInAdvance : AppControl
    {
        #region constants
        private const string ADVANCEPAYMENTOBJECTNAME = "AdvancePayment";
        private const string GRIDMODEVIEW = "advancepaymentmode";
        private const string UPDATECOMMAND = "CustomUpdate";
        private const string EDITITEMID = "advancepaymentgriditem";
        private const string ADDNEWBUTTONID = "lbAddPayment";
        private const string UPDATEBUTTONID = "imgUpdate";
        private const string EDITBUTTONID = "imgEdit";
        private const int MODEVIEW = 0;
        private const int MODEEDIT = 1;
        private const int MODEADD = 2;
        private static readonly string[] paymentAdd = { 
            "DescriptionId", "PayingTo", "Amount","UnitId"
        };
        #endregion

        #region fields
        private ArrayList objectFields;
        private readonly Hashtable fields = new Hashtable();
        private PayingInAdvance advancePayment;
        private bool canEdit=false;
        private bool canAddNew=false;
        private readonly DataView dvDescriptionList = PayingInAdvance.GetDecriptionList();
        private readonly DataView dvUnitList = PayingInAdvance.GetUnitList();
        private ArrayList logs = null;
        private static DataView dvGridData;
        #endregion

        #region properties
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
                Object o = Session[EDITITEMID];
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
                Session[EDITITEMID] = value;
            }
        }
        protected bool CanAddNew
        {
            get { return canAddNew; }
        }
        protected bool CanEdit
        {
            get { return canEdit; }
        }
        protected int gridMode
        {
            get
            {
                int res = MODEVIEW;
                Object o = Session[GRIDMODEVIEW];
                if (o != null)
                {
                    try
                    {
                        res = Convert.ToInt32(o);
                    }
                    catch { }
                }
                return res;
            }
            set
            {
                Session[GRIDMODEVIEW] = value;
            }
        }
        private DataView DvGridData
        {
            get
            {
                if (dvGridData==null)
                {
                    dvGridData = PayingInAdvance.GetPayingInAdvanceForGrid(MortgageID);
                }
                return dvGridData;
            }
        }
        #endregion


        #region methods
        private void SetControls(Control e)
        {
            if (IsFieldEditable("DescriptionId"))
            {
                RadComboBox rc = (RadComboBox)e.FindControl("ddlDescription");
                Label lbl = (Label)e.FindControl("lblDescription");
                if (rc != null)
                {
                    rc.DataSource = dvDescriptionList;
                    rc.DataTextField = "Name";
                    rc.DataValueField = "Id";
                    rc.DataBind();
                    rc.SelectedValue = advancePayment.DescriptionId.ToString();
                    rc.Skin = "WindowsXP";
                    rc.Visible = true;
                }
                if (lbl != null)
                {
                    lbl.Visible = false;
                }
            }
            if (IsFieldEditable("PayingTo"))
            {
                TextBox tb = (TextBox)e.FindControl("tbPayingTo");
                RequiredFieldValidator val = (RequiredFieldValidator)e.FindControl("rfvPayingTo");
                Label lbl = (Label)e.FindControl("lblPayingTo");
                if (tb != null)
                {
                    tb.Text = advancePayment.PayingTo;
                    tb.Visible = true;
                }
                if (val != null)
                {
                    val.Visible = true;
                }
                if (lbl != null)
                {
                    lbl.Visible = false;
                }
            }
            if (IsFieldEditable("Amount"))
            {
                RadNumericTextBox rtb = (RadNumericTextBox)e.FindControl("tbAmount");
                Label lbl = (Label)e.FindControl("lblAmount");
                if (rtb != null)
                {
                    rtb.Text = advancePayment.Amount.ToString();
                    rtb.Visible = true;
                }
                if (lbl != null)
                {
                    lbl.Visible = false;
                }
            }
            if (IsFieldEditable("UnitId"))
            {
                DropDownList ddl = (DropDownList)e.FindControl("ddlUnit");
                if (ddl != null)
                {
                    ddl.DataSource = dvUnitList;
                    ddl.DataTextField = "Name";
                    ddl.DataValueField = "Id";
                    ddl.DataBind();
                    ddl.SelectedValue = advancePayment.UnitId.ToString();
                    ddl.Visible = true;
                }
                Label lbl = (Label)e.FindControl("lblUnit");
                if (lbl != null)
                {
                    lbl.Visible = false;
                }
            }
            ImageButton btn = (ImageButton)e.FindControl("imgCancel");
            if (btn != null)
            {
                btn.Visible = true;
            }
            btn = (ImageButton)e.FindControl("imgUpdate");
            if (btn != null)
            {
                btn.Visible = true;
            }
            btn = (ImageButton)e.FindControl("imgEdit");
            if (btn != null)
            {
                btn.Visible = false;
            }
        }
        private bool IsFieldEditable(string fieldName)
        {
            bool res = true;
            if (fields.ContainsKey((fieldName)))
                res = !(bool)fields[fieldName];

            return res;
        }
        private void CheckCommand()
        {
            string postBackControl = Page.Request.Form["__EVENTTARGET"];
            if (!String.IsNullOrEmpty(postBackControl))
            {
                if (postBackControl.EndsWith(":" + ADDNEWBUTTONID))
                {
                    gridMode = MODEADD;
                    EditItemId = -1;
                    advancePayment = new PayingInAdvance(EditItemId);
                    advancePayment.MortgageId = MortgageID;
                }
                else if (postBackControl.EndsWith("_"+EDITBUTTONID))
                {
                    string s = postBackControl.Replace("_" + EDITBUTTONID,"");
                    int i = s.LastIndexOf("_");
                    if (i>0)
                    {
                        s = s.Substring(i+1);
                        try
                        {
                            EditItemId = Convert.ToInt32(s);
                            gridMode = MODEEDIT;
                        }
                        catch
                        {
                            EditItemId = -1;
                        }
                    }
                }
                else if(!postBackControl.EndsWith(":"+UPDATEBUTTONID))
                {
                    gridMode = MODEVIEW;
                }
            }
            else
            {
                gridMode = MODEVIEW;
            }
        }
        private void CheckFieldsForEdit()
        {
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageID);
            if (mp != null)
            {
                objectFields = mp.GetObjectFields(ADVANCEPAYMENTOBJECTNAME);
                if (objectFields != null)
                {
                    for (int i = 0; i < objectFields.Count; i++)
                    {
                        MortgageProfileField mpf = (MortgageProfileField)objectFields[i];
                        if (mpf != null)
                        {
                            fields.Add(mpf.FullPropertyName.Replace(ADVANCEPAYMENTOBJECTNAME + ".", ""), mpf.ReadOnly);
                        }
                    }
                }
                canAddNew = true;
                canEdit = false;
                for (int i = 0; i < paymentAdd.Length; i++)
                {
                    if (fields.ContainsKey(paymentAdd[i]))
                    {
                        canAddNew &= !(bool)fields[paymentAdd[i]];
                    }
                }
                for (int i = 0; i < paymentAdd.Length; i++)
                {
                    if (fields.ContainsKey(paymentAdd[i]))
                    {
                        canEdit |= !(bool)fields[paymentAdd[i]];
                    }
                }
            }
        }
        private void BuildPayment(Control gridItem)
        {
            advancePayment = new PayingInAdvance(EditItemId);
            advancePayment.MortgageId = MortgageID;
            logs = new ArrayList();
            RadComboBox rc = (RadComboBox)gridItem.FindControl("ddlDescription");
            if ((rc != null) && rc.Visible)
            {
                int descriptionId;
                if (rc.SelectedIndex!=-1)
                {
                    descriptionId = Convert.ToInt32(rc.SelectedValue);
                }
                else
                {
                    descriptionId = -1;
                }
                if (descriptionId!=-1)
                {
                    if (advancePayment.DescriptionId!=descriptionId)
                    {
                        logs.Add(new MortgageLogEntry("AdvancePayment", advancePayment.ID, "AdvancePayment.DescriptionId", advancePayment.DescriptionId.ToString(), descriptionId.ToString(), CurrentPage.CurrentUser.Id));
                    }
                }
                advancePayment.DescriptionId = descriptionId;
                advancePayment.Description = rc.Text;
            }
            TextBox tb = (TextBox)gridItem.FindControl("tbPayingTo");
            if ((tb != null)&&tb.Visible)
            {
                string payingTo = tb.Text;
                if (advancePayment.PayingTo!=payingTo)
                {
                    logs.Add(new MortgageLogEntry("AdvancePayment", advancePayment.ID, "AdvancePayment.PayingTo", advancePayment.PayingTo, payingTo, CurrentPage.CurrentUser.Id));
                }
                advancePayment.PayingTo = payingTo;
            }
            RadNumericTextBox rtb = (RadNumericTextBox)gridItem.FindControl("tbAmount");
            if ((rtb != null)&&rtb.Visible)
            {
                decimal amount = decimal.Parse(rtb.Text);
                if (advancePayment.Amount!=amount)
                {
                    logs.Add(new MortgageLogEntry("AdvancePayment", advancePayment.ID, "AdvancePayment.Amount", advancePayment.Amount.ToString(), amount.ToString(), CurrentPage.CurrentUser.Id));
                }
                advancePayment.Amount = amount;
            }
            DropDownList ddl = (DropDownList)gridItem.FindControl("ddlUnit");
            if ((ddl != null)&&ddl.Visible)
            {
                int unitId =  Convert.ToInt32(ddl.SelectedValue);
                if (advancePayment.UnitId!=unitId)
                {
                    logs.Add(new MortgageLogEntry("AdvancePayment", advancePayment.ID, "AdvancePayment.UnitId", advancePayment.UnitId.ToString(), unitId.ToString(), CurrentPage.CurrentUser.Id));
                }
                advancePayment.UnitId = unitId;
            }
        }
        private void SaveData()
        {
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageID);
            if (mp != null) mp.SavePaymentWithLog(advancePayment, logs);
        }
        #endregion

        #region databinding methods
        protected static string GetAmount(Object o)
        {
            string result = String.Empty;
            if (o is GridInsertionObject)
            {
                return result;
            }
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                decimal amount = Convert.ToDecimal(row["Amount"].ToString());
                result = amount.ToString("C");
            }
            return result;
        }
        private void BindGrid()
        {
            if (gridMode == MODEADD)
            {
                DvGridData.RowFilter = String.Empty;
            }
            else
            {
                DvGridData.RowFilter = "id>0";
            }
            gPayingInAdvance.MasterTableView.CommandItemDisplay = gridMode == MODEVIEW ? GridCommandItemDisplay.Bottom : GridCommandItemDisplay.None;
            gPayingInAdvance.DataSource = DvGridData;
            gPayingInAdvance.DataBind();
        }
        protected void Grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (gridMode == MODEVIEW)
            {
                if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
                {
                    ImageButton btn = (ImageButton)e.Item.FindControl("imgEdit");
                    if (btn!=null)
                    {
                        DataRowView dr = e.Item.DataItem as DataRowView;
                        if (dr != null)
                        {
                            btn.ID = "Id_" + Convert.ToInt32(dr["id"]) + "_" + EDITBUTTONID;
                        }
                    }
                }
                return;
            }
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                DataRowView dr = e.Item.DataItem as DataRowView;
                if (dr != null)
                {
                    int id = Convert.ToInt32(dr["id"]);
                    if (id == EditItemId)
                    {
                        advancePayment = new PayingInAdvance(dr);
                        SetControls(e.Item);
                    }
                    else
                    {
                        ImageButton btn = (ImageButton)e.Item.FindControl("imgEdit");
                        if (btn != null)
                        {
                            btn.Visible = false;
                        }

                    }
                }
            }
        }
        #endregion

        #region event handlers
        protected void Grid_ItemCommand(object source, GridCommandEventArgs e)
        {
            gridMode = MODEVIEW;
            if(e.CommandName == UPDATECOMMAND)
            {
                BuildPayment(e.Item);
                if (advancePayment != null)
                {
                    SaveData();
                }
                EditItemId = -1;
                dvGridData = null;
            }
            else
            {
                EditItemId = -1;
            }
            BindGrid();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckFieldsForEdit();
            CheckCommand();
            BindGrid();
        }
    }
}