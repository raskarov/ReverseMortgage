using System;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class Payoffs : MortgageDataGridControl
    {
        #region constants
        private const string OBJECTNAME = "Payoffs";
        private const string ADDRECORDTEXT = "Add new payoff";
        private const string CHANGESTATUSCOMMAND = "ChangeStatus";
        private const int MODECHANGESTATUS = 3;
        private const int ORDEREDSTATUSID = 2;
        private const int NEEDTOORDEREDSTATUSID = 1;
        private const int MORTGAGESTATUSAPPLICATIONID = 3;
        private const string UPDATENEEDEDCSS = "payoffUpdateNeeded";
        private static readonly string[] gridFieldsAdd = { 
            "Creditor", "PayOffStatusId", "Amount"
        };
        private static readonly string[] gridFieldsEdit = { 
            "Perdiem", "ExpDate","Address1","Address2","City","Zip","AccountNumber","StateId"
        };
        private const string EDITFORMCONTROLNAME = "PayoffEdit.ascx";
        private const string ONCLICKATTRIBUTE = "OnClick";
        #endregion

        #region fields
        private bool canChangeStatus;
        private Payoff payoff;
        private DataView dvPayoffStatus;
        private readonly string objectName = OBJECTNAME;
        private readonly string addRecordText = ADDRECORDTEXT;
        private DataView dvGridData;
        private int selectedStatusId = -1;
        private MortgageProfile mp;
        #endregion

        public event UpdateNeeded OnUpdateNeeded;
        public delegate void UpdateNeeded();


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
        protected override DataView DvGridData
        {
            get
            {
                if (dvGridData == null)
                {
                    dvGridData = Payoff.GetPayoffListForGrid(MortgageId);
                }
                return dvGridData;
            }
        }
        protected override int EditMode
        {
            get { return FORMEDIT; }
        }
        protected override string EditFormControlName
        {
            get
            {
                return EDITFORMCONTROLNAME;
            }
        }
        protected override object EditObject
        {
            get
            {
                if (payoff == null)
                {
                    payoff = new Payoff(-1);
                }
                return payoff;
            }
        }
        #endregion

        #region methods

        #region overriden methods
        protected override void Page_Load(object sender, EventArgs e)
        {
            mp = CurrentPage.GetMortgage(MortgageId);
            G = gPayoffs;
            base.Page_Load(sender, e);
        }
        protected override void CheckFieldsAccess()
        {
            canAddNew = true;
            canEdit = false;
            canChangeStatus = false;
            for (int i = 0; i < gridFieldsAdd.Length; i++)
            {
                if (fields.ContainsKey(gridFieldsAdd[i]))
                {
                    canAddNew &= !(bool)fields[gridFieldsAdd[i]];
                    canEdit |= !(bool)fields[gridFieldsAdd[i]];
                }
            }
            for (int i = 0; i < gridFieldsEdit.Length; i++)
            {
                if (fields.ContainsKey(gridFieldsEdit[i]))
                {
                    canEdit |= !(bool)fields[gridFieldsEdit[i]];
                }
            }
            if (fields.ContainsKey("PayoffStatusId"))
            {
                canChangeStatus = !(bool)fields["PayoffStatusId"];
            }
        }
        protected override void G_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView) e.Row.DataItem;
                int id = Convert.ToInt32(dr["id"].ToString());
                Payoff item;
                if(id>0)
                {
                    item = new Payoff(dr);
                }
                else
                {
                    item = new Payoff(-1);
                }
                int statusId = item.PayoffCalculatedStatusId;
                int realstatusId = item.PayoffStatusId;
                SetStatusColumn(e,statusId);
                SetVisibleColumn(e,statusId);
                if(mp.CurProfileStatusID == MortgageProfile.MANAGEDLEADSTATUSID)
                {
                    SetColumnStyle(e, false);
                }
                else
                {
                    if ((statusId == NEEDTOORDEREDSTATUSID) || (realstatusId == NEEDTOORDEREDSTATUSID))
                    {
                        bool b = mp.CurProfileStatusID > MORTGAGESTATUSAPPLICATIONID;
                        if (item.OrderedDate != null)
                        {
                            DateTime dt = ((DateTime)item.OrderedDate).AddDays(5);
                            b = b || (DateTime.Now > dt);
                        }
                        SetColumnStyle(e, b);
                    }
                    else
                    {
                        SetColumnStyle(e, statusId != realstatusId);
                    }
                }
                if(gridMode!=MODEVIEW)
                {
                    if (currentRow==EditRowId)
                    {
                        payoff = item;
                        EditItemId = payoff.ID;
                        SetActionColumn(e, gridMode == MODECHANGESTATUS, gridMode != MODECHANGESTATUS);
                    }
                    else
                    {
                        SetActionColumn(e, false, gridMode != MODECHANGESTATUS);
                    }
                }
                else if(gridMode == MODEVIEW)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    if (btnDelete != null)
                    {
                        if (canAddNew)
                        {
                            btnDelete.Attributes.Add(ONCLICKATTRIBUTE, "javascript:{{var r=confirm('Delete this payoff?');if (!r)return false;}};");
                        }
                        else
                        {
                            btnDelete.Visible = false;
                        }
                    }
                }
                currentRow++;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (showFooter)
                {
                    CreateFooter(e,0);
                }
            }
        }
        private static void SetColumnStyle(GridViewRowEventArgs e, bool updateNeeded)
        {
            if (updateNeeded)
            {
                Label lbl = (Label)e.Row.FindControl("lblCreditor");
                if (lbl != null)
                {
                    lbl.CssClass = UPDATENEEDEDCSS;
                }
                lbl = (Label)e.Row.FindControl("lblStatus");
                if (lbl != null)
                {
                    lbl.CssClass = UPDATENEEDEDCSS;
                }
                LinkButton lb = (LinkButton)e.Row.FindControl("lbStatus");
                if (lb != null)
                {
                    lb.CssClass = UPDATENEEDEDCSS;
                }
                lbl = (Label)e.Row.FindControl("lblAmount");
                if (lbl != null)
                {
                    lbl.CssClass = UPDATENEEDEDCSS;
                }
                lbl = (Label)e.Row.FindControl("lblPerdiem");
                if (lbl != null)
                {
                    lbl.CssClass = UPDATENEEDEDCSS;
                }
                lbl = (Label)e.Row.FindControl("lblExpDate");
                if (lbl != null)
                {
                    lbl.CssClass = UPDATENEEDEDCSS;
                }
                lbl = (Label)e.Row.FindControl("lblDaysLeft");
                if (lbl != null)
                {
                    lbl.CssClass = UPDATENEEDEDCSS;
                }
            }
        }
        private static void SetVisibleColumn(GridViewRowEventArgs e, int statusId)
        {
            if (statusId > ORDEREDSTATUSID)
            {
                Label lbl = (Label)e.Row.FindControl("lblPerdiem");
                if (lbl != null)
                {
                    lbl.Visible = true;
                }
                lbl = (Label)e.Row.FindControl("lblExpDate");
                if (lbl != null)
                {
                    lbl.Visible = true;
                }
            }
        }
        private void SetStatusColumn(GridViewRowEventArgs e, int statusId)
        {
            if((gridMode==MODECHANGESTATUS)&&(currentRow==EditRowId))
            {
                Label lbl = (Label)e.Row.FindControl("lblStatus");
                if (lbl != null)
                {
                    lbl.Visible = false;
                }
                LinkButton lb = (LinkButton)e.Row.FindControl("lbStatus");
                if (lb != null)
                {
                    lb.Visible = false;
                }
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlStatus");
                if (ddl != null)
                {
                    ddl.Visible = true;
                    ddl.DataSource = DvPayoffStatus;
                    ddl.DataTextField = "name";
                    ddl.DataValueField = "id";
                    ddl.DataBind();
                    ddl.SelectedValue = statusId.ToString();
                }
            }
            else if ((gridMode != MODEVIEW) || !IsFieldEditable("Status"))
            {
                Label lbl = (Label)e.Row.FindControl("lblStatus");
                if (lbl != null)
                {
                    lbl.Visible = true;
                }
                LinkButton lb = (LinkButton)e.Row.FindControl("lbStatus");
                if (lb != null)
                {
                    lb.Visible = false;
                }
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlStatus");
                if (ddl != null)
                {
                    ddl.Visible = false;
                }
            }
        }
        protected override void Save(object o, ArrayList logs)
        {
            if (mp != null)
            {
                ChangeAjaxSetting(false);
                payoff.MortgageID = mp.ID;
                mp.SavePayoffWithLog(payoff, logs);
                CheckUpdate();
                Tabs tabs = ((Default)Page).tabs;
                if (tabs != null)
                {
                    tabs.SetTabColor();
                }
            }
            base.Save(o, logs);
        }
        protected override void CancelEdit()
        {
            ChangeAjaxSetting(false);
            base.CancelEdit();
        }
        protected override void G_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == DELETECOMMAND)
            {
                Payoff.Delete(Convert.ToInt32(e.CommandArgument));
                ResetDataSource();
                mp.ResetPayoff();
                Tabs tabs = ((Default)Page).tabs;
                if (tabs != null)
                {
                    tabs.SetTabColor();
                }
                BindGrid();
            }
            else if (e.CommandName != CHANGESTATUSCOMMAND)
            {
                base.G_RowCommand(sender, e);
                if (e.CommandName == CANCELCOMMAND)
                {
                    showFooter = true;
                    BindGrid();
                }
            }
            else
            {
                EditRowId = Convert.ToInt32(e.CommandArgument);
                gridMode = MODECHANGESTATUS;
                showFooter = false;
                BindGrid();
            }
        }
        protected override void ResetDataSource()
        {
            dvGridData = null;
        }
        #endregion

        protected void ChangeAjaxSetting(bool add)
        {
            RadAjaxManager ajax = ((Default)Page).AjaxManager;
            if (add)
                ajax.AjaxSettings.AddAjaxSetting(gPayoffs, gPayoffs, null);
            else
            {
                //((Default)this.Page).RemoveAjaxSetting(gPayoffs, gPayoffs);
                AjaxSettingsCollection ajaxSettings = ajax.AjaxSettings;
                foreach (AjaxSetting ajaxSetting in ajaxSettings)
                {
                    if (ajaxSetting.AjaxControlID == gPayoffs.ID)
                    {
                        AjaxUpdatedControl settingToRemove = null;
                        foreach (AjaxUpdatedControl ajaxUpdatedControl in ajaxSetting.UpdatedControls)
                        {
                            if (ajaxUpdatedControl.ControlID == gPayoffs.ClientID)
                            {
                                settingToRemove = ajaxUpdatedControl;
                                break;
                            }
                        }
                        if (settingToRemove != null)
                        {
                            ajaxSetting.UpdatedControls.Remove(settingToRemove);
                            break;
                        }
                    }
                }
            }
        }

        protected bool CanChangeStatus
        {
            get { return canChangeStatus; }
        }
        protected void Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool needEdit = false;
            DropDownList ddl = (DropDownList)gPayoffs.Rows[EditRowId].FindControl("ddlStatus");
            if (ddl != null)
            {
                payoff = new Payoff(EditItemId);
                if (payoff != null)
                {
                    selectedStatusId = Convert.ToInt32(ddl.SelectedValue);
                    if(gridMode==MODECHANGESTATUS)
                    {
                        ArrayList logs = new ArrayList();
                        payoff.MortgageID = MortgageId;
                        if (selectedStatusId != payoff.PayoffStatusId)
                        {
                            logs.Add(new MortgageLogEntry("Payoff", payoff.ID, "Payoff.PayoffStatusId", payoff.PayoffStatusId.ToString(), selectedStatusId.ToString(), CurrentPage.CurrentUser.Id));
                            payoff.PayoffStatusId = selectedStatusId;
                        }
                        payoff.PayoffStatusId = selectedStatusId;
                        if (mp != null)
                        {
                            mp.SavePayoffWithLog(payoff, logs);
                            CheckUpdate();
                        }
                    }
                    needEdit = payoff.PayoffStatusId==Payoff.RECEIVEDSTATUSID;
                }
            }
            if (gridMode == MODECHANGESTATUS)
            {
                if (needEdit)
                {
                    showFooter = false;
                    gridMode = MODEEDIT;
                    ResetDataSource();
                    BindGrid();
                }
                else
                {
                    ChangeAjaxSetting(false);
                    gridMode = MODEVIEW;
                    EditItemId = -1;
                    EditRowId = -1;
                    ResetDataSource();
                    showFooter = true;
                    BindGrid();
                }
            }
            else
            {
                if (selectedStatusId > ORDEREDSTATUSID)
                {
                    Label lbl = (Label)gPayoffs.Rows[EditRowId].FindControl("lblPerdiem");
                    if (lbl != null)
                    {
                        lbl.Visible = true;
                    }
                    lbl = (Label)gPayoffs.Rows[EditRowId].FindControl("lblExpDate");
                    if (lbl != null)
                    {
                        lbl.Visible = true;
                    }
                }
            }
        }
        protected static string GetDaysLeft(Object o)
        {
            string result = String.Empty;
            DataRowView row = (DataRowView)o;
            if ((row != null)&&(Convert.ToInt32(row["id"].ToString())>0))
            {
                if (CheckStatus(row))
                {
                    result = row["DaysLeft"].ToString();
                }
            }
            return result;
        }
        protected static string GetPeridem(Object o, string fieldName)
        {
            string result = String.Empty;
            DataRowView row = (DataRowView)o;
            if ((row != null) && (Convert.ToInt32(row["id"].ToString()) > 0))
            {
                if (CheckStatus(row))
                {
                    result = GetMoney(o, fieldName);
                }
            }
            return result;
        }
        protected static string GetExpDate(Object o, string fieldName)
        {
            string result = String.Empty;
            DataRowView row = (DataRowView)o;
            if ((row != null) && (Convert.ToInt32(row["id"].ToString()) > 0))
            {
                if (CheckStatus(row))
                {
                    result = GetDate(o, fieldName);
                }
            }
            return result;
        }
        private static bool CheckStatus(DataRowView row)
        {
            bool result = false;
            if (!String.IsNullOrEmpty(row["PayoffStatusid"].ToString()))
            {
                int statusId = Convert.ToInt32(row["PayoffStatusid"].ToString());
                result = statusId > 2;
            }
            return result;
        }
        private void CheckUpdate()
        {
            bool updateStatus = Payoff.GetUpdateStatus(mp.ID);
            if (mp.PayoffUpdateNeeded != updateStatus)
            {
                mp.PayoffUpdateNeeded = updateStatus;
                if (OnUpdateNeeded != null)
                {
                    OnUpdateNeeded();
                }
            }
        }

        #endregion

    }
}