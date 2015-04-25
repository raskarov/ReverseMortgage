using System;
using System.Collections;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class ApplicantList : AppControl, IPostBackEventHandler
    {
        #region constants
        public const int ACTIVELOANGROUPID = 1;
        public const int MANAGEDLEADGROUPID = 2;
        public const int CLOSEDLOANGROUPID = 3;
        public const int CANCELLEDWITHDRAWNLOANGROUPID = 4;

        private const int NOGROUPINGID = -1;
        private const int STATUSGROUPINGID = -2;
        private const int ALLUSERSGROUPINGID = -3;

        private readonly int[] activeLoanRoles = {4, 9, 11, 13};
        #endregion


        #region fields
        private bool setToFirstLoan = false;
        private int firstMortgageId = -1;
        private DataView dvRoles;
        private int groupingTypeId = 1;
        #endregion

        #region properties
        public bool IsManagedLeadSelected
        {
            get { return SelectedLoanGroup == MANAGEDLEADGROUPID; }
        }
        private string ManagedLeadFilter
        {
            get
            {
                string res = "";
                Object o = Session["managedleadfilter"];
                if(o!=null)
                {
                    res = o.ToString();
                }
                return res;
            }
            set { Session["managedleadfilter"] = value; }
        }
        private DataView DvRoles
        {
            get
            {
                if(dvRoles==null)
                {
                    dvRoles = Role.GetRoleList();
                }
                return dvRoles;
            }
        }
        private int SelectedLoanGroup
        {
            get
            {
                int res = ACTIVELOANGROUPID;
                Object o = Session["selectedloangroup"];
                if(o!=null)
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
            set
            {
                Session["selectedloangroup"] = value;
            }
        }
        private int SelectedGroupingId
        {
            get
            {
                int res = STATUSGROUPINGID;
                Object o = Session["selectedloangrouping"];
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
            set
            {
                Session["selectedloangrouping"] = value;
            }
        }
        #endregion


        #region Private methods/properties
        private void StoreGroupSelValue()
        {
            Session["GroupSelValue"] = ddlGroup.SelectedValue;
        }
        private string GroupSelValue
        {
            get
            {
                if (Session["GroupSelValue"] == null)
                    Session["GroupSelValue"] = ddlGroup.SelectedValue;
                return Convert.ToString(Session["GroupSelValue"]);
            }
        }
        private Hashtable PBExpandedStates
        {
            get
            {
                if (Session["PBExpandedStates"] == null/* || !Page.IsPostBack*/)
                {
                    Hashtable pbExpandedStates = new Hashtable();
                    pbExpandedStates[String.Format("{0}_{1}", "NoGrouping", "0")] = true;

                    Session["PBExpandedStates"] = pbExpandedStates;
                }
                return (Hashtable)Session["PBExpandedStates"];
            }
        }
        private int MortgageProfileID
        {
            get
            {
                if (Session[Constants.MortgageID] == null)
                    Session[Constants.MortgageID] = 0;
                return Convert.ToInt32(Session[Constants.MortgageID].ToString());
            }
        }
        private int CompanyID
        {
            get
            {
                return ((AppPage)Page).CurrentUser.CompanyId;
            }
        }
        private AppUser User
        {
            get
            {
                return ((AppPage)Page).CurrentUser; 
            }
        }
        private int UserID
        {
            get
            {
                return ((AppPage)Page).CurrentUser.Id;
            }
        }
        public bool IsEmailMode
        {
            get
            {
                //Object o = ViewState["IsEmailMode"];
                //always show checkboxes
                Object o =  Session["IsEmailMode"];

                bool res = false;
                if (o != null)
                    res = bool.Parse(o.ToString());
                return res;
            }
            set
            {
                ViewState["IsEmailMode"] = value;
            }
        }
        public void ChangeApplicantList(bool IsEmailMode_)
        {
            IsEmailMode = IsEmailMode_;
            RepopulateMortgageList();
        }
        private static Hashtable ParseCheckBoxInfo(string chkBoxInfo)
        {
            Hashtable paramValHash = new Hashtable();
            string[] paramValArr = chkBoxInfo.Split(';');
            foreach (string paramVal in paramValArr)
            {
                if (paramVal.Trim().Length == 0)
                    continue;

                string[] param_val = paramVal.Split('=');
                if (param_val.Length < 2 || param_val[0].Trim().Length == 0 || param_val[1].Trim().Length == 0)
                    continue;

                paramValHash[param_val[0].Trim()] = param_val[1].Trim();
            }

            return paramValHash;
        }

        private void StorePBExpandedStates()
        {
            string groupSelValue = GroupSelValue;

            Hashtable pbExpandedStates = PBExpandedStates;
            foreach (RadPanelItem groupPanelItem in RadPBMortgages.Items)
            {
                string key = groupSelValue + "_" + groupPanelItem.Value;
                pbExpandedStates[key] = groupPanelItem.Expanded;
            }

            StoreGroupSelValue();
        }

        private void ReStorePBExpandedStates()
        {
            string groupSelValue = GroupSelValue;

            Hashtable pbExpandedStates = PBExpandedStates;
            foreach (RadPanelItem groupItem in RadPBMortgages.Items)
            {
                string key = groupSelValue + "_" + groupItem.Value;
//                if (Page.IsPostBack)
                    groupItem.Expanded = pbExpandedStates.ContainsKey(key) ? Convert.ToBoolean(pbExpandedStates[key]) : false;
//                else
//                    groupItem.Expanded = true;

                if (RadPBMortgages.SelectedItem != null && ((RadPanelItem) RadPBMortgages.SelectedItem.Parent).Value == groupItem.Value)
                    groupItem.Expanded = true;
            }
        }
        private void SetLastPipeLineReload()
        {
            Session[Constants.LASTPIPELINERELOADTIME] = DateTime.Now;
        }
        public void RefreshMortgageChkBoxes(ArrayList mpIDList)
        {
            if (!IsEmailMode)
                return;

            foreach (RadPanelItem grouplItem in RadPBMortgages.Items)
                foreach (RadPanelItem mortgageItem in grouplItem.Items)
                {
                    int curMPID = Convert.ToInt32(mortgageItem.Value);
                    ((CheckBox) mortgageItem.Controls[0]).Checked = mpIDList.Contains(curMPID);
                }
        }
        #endregion

        #region Events
        public event EventHandler MortgageItemClick;
        public event EventHandler MortgageCheckBoxClick;
        #endregion

        #region methods

        #region private
        protected void AddAjaxSettings()
        {
            RadAjaxManager ajax = ((Default)Page).AjaxManager;
            ajax.AjaxSettings.AddAjaxSetting(ddlDisplay, RadPBMortgages, null);
            ajax.AjaxSettings.AddAjaxSetting(ddlGroup, RadPBMortgages, null);
            ajax.AjaxSettings.AddAjaxSetting(dllSort, RadPBMortgages, null);
        }
        private static RadComboBoxItem GetSelectGroupItem(string name, int id)
        {
            return new RadComboBoxItem(name, id.ToString());
        }
        private bool IsRoleUsedInActiveLoan(int roleId)
        {
            for (int i = 0; i < activeLoanRoles.Length;i++ )
            {
                if (activeLoanRoles[i] == roleId) return true;
            }
            return false;
        }
        private void BindData()
        {
            BindLoanList();
        }
        private static DataView GetAlphabeting()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", typeof (string)));
            dt.Columns.Add(new DataColumn("Name", typeof (string)));
            for(int i=0;i<26;i++)
            {
                dt.Rows.Add(Convert.ToChar(65 + i).ToString(), Convert.ToChar(65 + i).ToString());
            }
            return dt.DefaultView;
        }
        private DataView GetGroups()
        {
            DataView res = null;
            switch (SelectedGroupingId)
            {
                case NOGROUPINGID:
                    groupingTypeId = 1;
                    if(SelectedLoanGroup == MANAGEDLEADGROUPID)
                    {
                        res = GetAlphabeting();
                    }
                    break;
                case STATUSGROUPINGID :
                    groupingTypeId = 1;
                    res = MortgageProfile.GetLoanStatusesForPipeline(SelectedLoanGroup);
                    break;
                case ALLUSERSGROUPINGID:
                    groupingTypeId = 2;
                    res = Company.GetUserListForPipeLine(CompanyID, rbAllLoans.Checked ? 0 : UserID, 0);
                    break;
                default:
                    groupingTypeId = 3;
                    res = Company.GetUserListForPipeLine(CompanyID, rbAllLoans.Checked ? 0 : UserID, SelectedGroupingId);
                    break;
            }
            return res;
        }
        private DataView GetLoans()
        {
            return MortgageProfile.GetPipeLineData(SelectedLoanGroup,groupingTypeId,CompanyID,rbAllLoans.Checked ? 0: UserID, groupingTypeId==3?SelectedGroupingId:0);
        }
        private string GetFilter()
        {
            string res = "";
            switch (SelectedGroupingId)
            {
                case NOGROUPINGID:
                    if (SelectedLoanGroup == MANAGEDLEADGROUPID)
                    {
//                        res = "YBLastName like '{0}%'";
                        if(!String.IsNullOrEmpty(ManagedLeadFilter))
                        {
                            res = "YBLastName like '{0}%' and (YBLastName like '" + ManagedLeadFilter +
                                  "%' or YBFirstName like '" + ManagedLeadFilter + "%')";
                        }
                        else
                        {
                            res = "YBLastName like '{0}%'";
                        }
                    }
                    break;
                case STATUSGROUPINGID:
                    res = "StatusId={0}";
                    break;
                case ALLUSERSGROUPINGID:
                    res = "UserId={0}";
                    break;
                default:
                    res = "UserId={0}";
                    break;
            }
            return res;
        }

        private void BindDisplayList()
        {
            DataView dv = MortgageProfile.GetLoanGroupList();
            if(!CurrentUser.IsLeadManagementEnabled)
            {
                dv.RowFilter = String.Format("id<>{0}",MANAGEDLEADGROUPID);
            }
            ddlDisplay.DataSource = dv;
            ddlDisplay.DataTextField = "name";
            ddlDisplay.DataValueField = "id";
            ddlDisplay.DataBind();
            ddlDisplay.SelectedValue = SelectedLoanGroup.ToString();
        }
        private void BindGroupList()
        {
            ddlGroup.Items.Clear();
            ddlGroup.Items.Add(GetSelectGroupItem("A-Z",NOGROUPINGID));
            RadComboBoxItem item = GetSelectGroupItem("Status", STATUSGROUPINGID);
            item.Selected = true;
            ddlGroup.Items.Add(item);
            if(SelectedLoanGroup==ACTIVELOANGROUPID)
            {
                for (int i = 0; i < DvRoles.Count; i++)
                {
                    int id = int.Parse(DvRoles[i]["id"].ToString());
                    string name = DvRoles[i]["name"].ToString();
                    if(IsRoleUsedInActiveLoan(id))
                    {
                        ddlGroup.Items.Add(GetSelectGroupItem(name, id));
                    }
                }
                ddlGroup.Items.Add(GetSelectGroupItem("All Users",ALLUSERSGROUPINGID));
            }
            ddlGroup.SelectedValue = SelectedGroupingId.ToString();
        }
        private void BindLoanList()
        {
            
            SetLastPipeLineReload();
            StorePBExpandedStates();

            RadPBMortgages.Items.Clear();
            RadPBMortgages.Width = Unit.Percentage(100);
            DataView dvGroups = GetGroups();
            string filter = GetFilter();
            DataView dvLoans = GetLoans();
            dvLoans.Sort = dllSort.SelectedValue;
            if(dvGroups==null)
            {
                string groupName = ddlGroup.SelectedItem.Text;
                RadPBMortgages.Items.Add(GetPanelGroupItem(groupName, dvLoans));
            }
            else
            {
                for (int i = 0; i < dvGroups.Count; i++)
                {
                    RadPBMortgages.Items.Add(GetPanelGroupItem(dvGroups[i], dvLoans, filter));
                }
            }
            ReStorePBExpandedStates();
            if (!IsEmailMode && MortgageProfileID > 0 && RadPBMortgages.SelectedItem == null)
                ClickMortgage(0);

        }
        private RadPanelItem GetPanelGroupItem(string groupName, DataView dvLoans)
        {
            RadPanelItem res = new RadPanelItem();
            int cnt = dvLoans.Count;
            res.Text = String.Format("<span style=\"float:right\" >{1}</span>&nbsp;<b>{0}</b>", groupName, cnt);
            res.Value = "0";
            res.PostBack = false;
            res.Expanded = true;
            FillGroup(res, dvLoans);
            return res;
        }
        private RadPanelItem GetPanelGroupItem(DataRowView drGroup, DataView dvLoans, string filter)
        {
            RadPanelItem res = new RadPanelItem();
            string groupId = drGroup["id"].ToString();
            if(!String.IsNullOrEmpty(filter))
            {
                dvLoans.RowFilter = String.Format(filter, groupId);
            }
            string groupName = drGroup["name"].ToString();
            int cnt = dvLoans.Count;
            res.Text = String.Format("<span style=\"float:right\" >{1}</span>&nbsp;<b>{0}</b>", groupName, cnt);
            res.Value = groupId;
            res.PostBack = false;
            FillGroup(res, dvLoans);
            return res;
        }
        private void FillGroup(IRadPanelItemContainer group,DataView dvLoans)
        {
            for (int i = 0; i < dvLoans.Count;i++ )
            {
                string mpID = Convert.ToString(dvLoans[i]["MPID"]);
                if (setToFirstLoan)
                {
                    firstMortgageId = int.Parse(mpID);
                }
                string ybFirstName = Convert.ToString(dvLoans[i]["YBFirstName"]);
                string ybLastName = Convert.ToString(dvLoans[i]["YBLastName"]);
                bool mpIsUpdateNeeded = Convert.ToBoolean(dvLoans[i]["MPIsUpdateNeeded"]);
                bool mpIsInvoiceUpdateNeeded = Convert.ToBoolean(dvLoans[i]["MPIsInvoiceUpdateNeeded"]);
                bool mpIsUpdateNeededDay = Convert.ToBoolean(dvLoans[i]["MPIsDayUpdateNeeded"]);
                string mpRequiredFields = Convert.ToString(dvLoans[i]["MPRequiredFields"]);
                bool mpIsCampaignUpdateNeeded = Convert.ToBoolean(dvLoans[i]["mpIsCampaignUpdateNeeded"]);

                RadPanelItem mortgagePanelItem = new RadPanelItem();

                if (setToFirstLoan)
                {
                    mortgagePanelItem.Selected = true;
                    setToFirstLoan = false;
                }

                if (IsEmailMode)
                {
                    CheckBox chk = new CheckBox();
                    chk.Attributes.Add("class", "apptxt");
                    chk.Attributes.Add("val", mpID);
                    chk.Attributes.Add("onclick", "ChkMortgageClicked(this);");
                    chk.AutoPostBack = false;
                    chk.ID = "chk_" + mpID;
                    chk.Text = ybLastName + ", " + ybFirstName;
                    chk.CssClass = "apptxt";
                    if (MortgageProfileID == Convert.ToInt32(mpID))
                        chk.Checked = true;

                    mortgagePanelItem.Controls.Add(chk);
                }
                else
                {
                    mortgagePanelItem.Text = ybLastName + ", " + ybFirstName;
                    if (MortgageProfileID == Convert.ToInt32(mpID))
                        mortgagePanelItem.Selected = true;

                    mortgagePanelItem.Attributes["type"] = "panelitem";
                    mortgagePanelItem.Attributes["title"] = mpID;
                }

                mortgagePanelItem.Value = mpID;
                mortgagePanelItem.PostBack = true;
                if (mpIsUpdateNeeded || mpIsInvoiceUpdateNeeded || mpIsUpdateNeededDay || !String.IsNullOrEmpty(mpRequiredFields) || mpIsCampaignUpdateNeeded)
                {
                    mortgagePanelItem.CssClass = "textUpdateNeeded";
                    mortgagePanelItem.SelectedCssClass = "textUpdateNeededSelected";
                }
                group.Items.Add(mortgagePanelItem);
            }
        }
        #endregion


        #region public
        public void SetToLoanGroup(int groupId)
        {
            if(groupId!=SelectedLoanGroup)
            {
                SelectedLoanGroup = groupId;
                SelectedGroupingId = groupId==ACTIVELOANGROUPID? STATUSGROUPINGID:NOGROUPINGID;
                BindDisplayList();
                BindGroupList();
                BindData();
            }
        }
        public void SetToManagedLead()
        {
            SelectedGroupingId = NOGROUPINGID;
            SelectedLoanGroup = MANAGEDLEADGROUPID;
            BindDisplayList();
            BindGroupList();
            BindData();
        }

        public void SetToActiveLoans()
        {
            SelectedGroupingId = NOGROUPINGID;
            SelectedLoanGroup = ACTIVELOANGROUPID;
            BindDisplayList();
            BindGroupList();
            BindData();
        }

        public void SetManageLeadFilter(string filter)
        {
            ManagedLeadFilter = filter;
            SelectedGroupingId = NOGROUPINGID;
            SelectedLoanGroup = MANAGEDLEADGROUPID;
            BindDisplayList();
            BindGroupList();
            BindData();
        }
        public void RepopulateMortgageList()
        {
            BindLoanList();
        }

        #region old methods
        //private DataSet GetGroupMortgageDS()
        //{
        //    DataSet resGroupMortgageDS = null;
        //    switch (ddlGroup.SelectedValue)
        //    {
        //        case "NoGrouping":
        //            {
        //                if (rbAllLoans.Checked)
        //                    resGroupMortgageDS = MortgageProfile.GetMortgageCountInDefault(CompanyID);
        //                else if (rbMyPipeline.Checked)
        //                    resGroupMortgageDS = MortgageProfile.GetMortgageCountInDefault(CompanyID, UserID);

        //                if (resGroupMortgageDS != null)
        //                    resGroupMortgageDS.Tables[0].Rows[0]["GroupName"] = dllSort.SelectedItem.Text;
        //            }
        //            break;
        //        case "Status":
        //            {
        //                if (rbAllLoans.Checked)
        //                {
        //                    resGroupMortgageDS = MortgageProfile.GetMortgageCountInStatuses(CompanyID);
        //                    if (User.IsOnlyInRoles(AppUser.UserRoles.Processor,
        //                                                AppUser.UserRoles.ProcessingManager,
        //                                                AppUser.UserRoles.Underwriter,
        //                                                AppUser.UserRoles.UnderwritingManager,
        //                                                AppUser.UserRoles.Closer,
        //                                                AppUser.UserRoles.ClosingManager,
        //                                                AppUser.UserRoles.PostCloser,
        //                                                AppUser.UserRoles.PostClosingManager))
        //                        resGroupMortgageDS.Tables[0].DefaultView.RowFilter = "GroupID > 1";
        //                }
        //                else if (rbMyPipeline.Checked)
        //                    resGroupMortgageDS = MortgageProfile.GetMortgageCountInStatuses(CompanyID, UserID);

        //            }
        //            break;
        //        case "AllUsers":
        //            {
        //                if (rbAllLoans.Checked)
        //                    resGroupMortgageDS = MortgageProfile.GetMortgageCountInUsers(CompanyID);
        //                else if (rbMyPipeline.Checked)
        //                    resGroupMortgageDS = MortgageProfile.GetMortgageCountInUsers(CompanyID, -1, UserID);
        //            }
        //            break;
        //        default:
        //            {
        //                if (rbAllLoans.Checked)
        //                    resGroupMortgageDS = MortgageProfile.GetMortgageCountInUsers(CompanyID, Convert.ToInt32(ddlGroup.SelectedValue));
        //                else if (rbMyPipeline.Checked)
        //                    resGroupMortgageDS = MortgageProfile.GetMortgageCountInUsers(CompanyID, Convert.ToInt32(ddlGroup.SelectedValue), UserID);
        //            }
        //            break;
        //    }

        //    return resGroupMortgageDS;
        //}
        //public void RepopulateMortgageList()
        //{
        //    SetLastPipeLineReload();
        //    StorePBExpandedStates();

        //    RadPBMortgages.Items.Clear();
        //    RadPBMortgages.Width = Unit.Percentage(100);

        //    DataSet groupMortgageDS;
        //    DataView groupView;
        //    DataView mortgageView;

        //    try
        //    {
        //        groupMortgageDS = GetGroupMortgageDS();
        //        groupView = groupMortgageDS.Tables[0].DefaultView;
        //        mortgageView = groupMortgageDS.Tables[1].DefaultView;
        //    }
        //    catch (Exception)
        //    {
        //        return;
        //    }

        //    foreach (DataRowView rowGroup in groupView)
        //    {
        //        string groupID = Convert.ToString(rowGroup["GroupID"]);
        //        int statusId = int.Parse(groupID);
        //        // Temporary solution
        //        if (statusId == 13 || statusId == 14 || statusId == 15)
        //        {
        //            continue;
        //        }
        //        string groupName = Convert.ToString(rowGroup["GroupName"]);
        //        string groupCount = Convert.ToString(rowGroup["GroupCount"]);
        //        string groupFilter = Convert.ToString(rowGroup["GroupFilter"]);

        //        mortgageView.RowFilter = groupFilter;
        //        groupCount = mortgageView.Count.ToString();

        //        RadPanelItem groupPanelItem = new RadPanelItem();
        //        groupPanelItem.Text = String.Format("<span style=\"float:right\" >{1}</span>&nbsp;<b>{0}</b>", groupName, groupCount);

        //        groupPanelItem.Value = groupID;
        //        groupPanelItem.PostBack = false;
        //        RadPBMortgages.Items.Add(groupPanelItem);

        //        mortgageView.Sort = dllSort.SelectedValue;

        //        foreach (DataRowView rowMortgage in mortgageView)
        //        {
        //            string mpID = Convert.ToString(rowMortgage["MPID"]);
        //            if (setToFirstLoan)
        //            {
        //                firstMortgageId = int.Parse(mpID);
        //            }
        //            string ybFirstName = Convert.ToString(rowMortgage["YBFirstName"]);
        //            string ybLastName = Convert.ToString(rowMortgage["YBLastName"]);
        //            bool mpIsUpdateNeeded = Convert.ToBoolean(rowMortgage["MPIsUpdateNeeded"]);
        //            bool mpIsInvoiceUpdateNeeded = Convert.ToBoolean(rowMortgage["MPIsInvoiceUpdateNeeded"]);
        //            bool mpIsUpdateNeededDay = Convert.ToBoolean(rowMortgage["MPIsDayUpdateNeeded"]);
        //            string mpRequiredFields = Convert.ToString(rowMortgage["MPRequiredFields"]);

        //            RadPanelItem mortgagePanelItem = new RadPanelItem();

        //            if (setToFirstLoan)
        //            {
        //                mortgagePanelItem.Selected = true;
        //                setToFirstLoan = false;
        //            }

        //            if (IsEmailMode)
        //            {
        //                CheckBox chk = new CheckBox();
        //                chk.Attributes.Add("class", "apptxt");
        //                chk.Attributes.Add("val", mpID);
        //                chk.Attributes.Add("onclick", "ChkMortgageClicked(this);");
        //                chk.AutoPostBack = false;
        //                chk.ID = "chk_" + mpID;
        //                chk.Text = ybLastName + ", " + ybFirstName;
        //                chk.CssClass = "apptxt";
        //                if (MortgageProfileID == Convert.ToInt32(mpID))
        //                    chk.Checked = true;

        //                mortgagePanelItem.Controls.Add(chk);
        //            }
        //            else
        //            {
        //                mortgagePanelItem.Text = ybLastName + ", " + ybFirstName;
        //                if (MortgageProfileID == Convert.ToInt32(mpID))
        //                    mortgagePanelItem.Selected = true;

        //                mortgagePanelItem.Attributes["type"] = "panelitem";
        //                mortgagePanelItem.Attributes["title"] = mpID;
        //            }

        //            mortgagePanelItem.Value = mpID;
        //            mortgagePanelItem.PostBack = true;
        //            if (mpIsUpdateNeeded || mpIsInvoiceUpdateNeeded || mpIsUpdateNeededDay || !String.IsNullOrEmpty(mpRequiredFields))
        //            {
        //                mortgagePanelItem.CssClass = "textUpdateNeeded";
        //                mortgagePanelItem.SelectedCssClass = "textUpdateNeededSelected";
        //            }
        //            groupPanelItem.Items.Add(mortgagePanelItem);
        //        }
        //    }

        //    ReStorePBExpandedStates();
        //    if (!IsEmailMode && MortgageProfileID > 0 && RadPBMortgages.SelectedItem == null)
        //        ClickMortgage(0);
        //}
        //protected void ddlGroup_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    RepopulateMortgageList();
        //}
        //protected void rbMyPipeline_CheckedChanged(object sender, EventArgs e)
        //{
        //    RepopulateMortgageList();
        //}
        //protected void rbAllLoans_CheckedChanged(object sender, EventArgs e)
        //{
        //    RepopulateMortgageList();
        //}
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    setToFirstLoan = false;
        //    if (!Page.IsPostBack)
        //    {
        //        RepopulateGroupList();
        //        RepopulateMortgageList();
        //        rbAllLoans.Enabled = !User.IsOnlyInRoles(AppUser.UserRoles.LoanOfficer, AppUser.UserRoles.LoanOfficerAssistant);
        //        AddAjaxSettings();
        //    }
        //    else if (IsEmailMode)
        //    {
        //        RepopulateMortgageList();
        //    }
        //}
        //private void RepopulateGroupList()
        //{
        //    DataView roleView = new DataView();
        //    try
        //    {
        //        roleView = Role.GetList(true, true, true, CompanyID);
        //    }
        //    catch (Exception) { }
        //    RadComboBoxItem groupItem = new RadComboBoxItem("A-Z", "NoGrouping");
        //    ddlGroup.Items.Add(groupItem);

        //    groupItem = new RadComboBoxItem("Status", "Status");
        //    groupItem.Selected = true;
        //    ddlGroup.Items.Add(groupItem);

        //    roleView.RowFilter = "Name = 'Loan Officer'";
        //    if (roleView.Count > 0)
        //    {
        //        groupItem = new RadComboBoxItem(roleView[0]["Name"].ToString(), roleView[0]["id"].ToString());
        //        ddlGroup.Items.Add(groupItem);
        //    }

        //    roleView.RowFilter = "Name = 'Processor'";
        //    if (roleView.Count > 0)
        //    {
        //        groupItem = new RadComboBoxItem(roleView[0]["Name"].ToString(), roleView[0]["id"].ToString());
        //        ddlGroup.Items.Add(groupItem);
        //    }

        //    roleView.RowFilter = "Name = 'Underwriter'";
        //    if (roleView.Count > 0)
        //    {
        //        groupItem = new RadComboBoxItem(roleView[0]["Name"].ToString(), roleView[0]["id"].ToString());
        //        ddlGroup.Items.Add(groupItem);
        //    }

        //    roleView.RowFilter = "Name = 'Closer'";
        //    if (roleView.Count > 0)
        //    {
        //        groupItem = new RadComboBoxItem(roleView[0]["Name"].ToString(), roleView[0]["id"].ToString());
        //        ddlGroup.Items.Add(groupItem);
        //    }

        //    groupItem = new RadComboBoxItem("All Users", "AllUsers");
        //    ddlGroup.Items.Add(groupItem);
        //}
        #endregion

        #endregion

        #endregion

        #region Event handlers
        protected void ddlDisplay_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            SelectedLoanGroup = int.Parse(ddlDisplay.SelectedValue);
            BindData();
        }
        protected void ddlGroup_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            SelectedGroupingId = int.Parse(ddlGroup.SelectedValue);
            BindData();
        }
        protected void rbMyPipeline_CheckedChanged(object sender, EventArgs e)
        {
            BindData();
        }
        protected void rbAllLoans_CheckedChanged(object sender, EventArgs e)
        {
            BindData();
        }


        #endregion

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            setToFirstLoan = false;
            if (!Page.IsPostBack)
            {
                BindDisplayList();
                BindGroupList();
                BindData();
                rbAllLoans.Enabled = !User.IsOnlyInRoles(AppUser.UserRoles.LoanOfficer, AppUser.UserRoles.LoanOfficerAssistant);
                AddAjaxSettings();
            }
            else if (IsEmailMode)
            {
                RepopulateMortgageList();
            }
        }

        protected void dllSort_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RepopulateMortgageList();
        }

        protected void RadPBMortgages_ItemClick(object sender, RadPanelbarEventArgs e)
        {

            ClickMortgage(Convert.ToInt32(e.Item.Value));
            RepopulateMortgageList();

        }
        private void ClickMortgage(int mortgageId)
        {
            if (MortgageItemClick != null)
            {
                Session["AllMessages"] = null;
                Page.ClientScript.RegisterHiddenField("currentmortgageid", mortgageId.ToString());
                MortgageArg mArg = new MortgageArg(mortgageId);
                MortgageItemClick(this, mArg);
            }
        }
        public void SetToFirstMortgage()
        {
            setToFirstLoan = true;
            RepopulateMortgageList();
            if(firstMortgageId>0)
            {
                ClickMortgage(firstMortgageId);
            }
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            ((AppPage) Page).IsAjaxPostBackRaisen = true;
            if (eventArgument == "true" || eventArgument == "false")
            {
                //always show checkboxes
                if (Session["IsEmailMode"] == null)
                    Session.Add("IsEmailMode", eventArgument);
                else
                    Session["IsEmailMode"] = eventArgument;

                RepopulateMortgageList();
            }
            else if (eventArgument.IndexOf("chk_") < 0)
            {
                ClickMortgage(Convert.ToInt32(eventArgument));
                RepopulateMortgageList();
            }
            else
            {
                Hashtable paramValHash = ParseCheckBoxInfo(eventArgument);

                bool isChecked = Boolean.Parse(paramValHash["Checked"].ToString());
                string chkID = paramValHash["ID"].ToString();
                int mpID = Convert.ToInt32(chkID.Substring(chkID.LastIndexOf("chk_") + 4));

                MortgageProfile mp = CurrentPage.GetMortgage(mpID);
                MortgageArg mArg = new MortgageArg(mp, isChecked);

                foreach (RadPanelItem grouplItem in RadPBMortgages.Items)
                    foreach (RadPanelItem mortgageItem in grouplItem.Items)
                        if (Convert.ToInt32(mortgageItem.Value) == mpID)
                            ((CheckBox) mortgageItem.Controls[0]).Text = mp.YoungestBorrower.FullName;

                if (MortgageCheckBoxClick != null)
                    MortgageCheckBoxClick(this, mArg);

                ((Default) CurrentPage).AjaxManager.ResponseScripts.Add("SetLoadinPanelState(false)");
            }
        }
        #endregion
    }
}
#region unused
 //private void RepopulateGroupList()
 //       {
 //           DataView roleView = new DataView();
 //           try
 //           {
 //               roleView = Role.GetList(true, true, true, CompanyID);
 //           }
 //           catch (Exception) { }
 //           RadComboBoxItem groupItem = new RadComboBoxItem("A-Z", "NoGrouping");
 //           ddlGroup.Items.Add(groupItem);

 //           groupItem = new RadComboBoxItem("Status", "Status");
 //           groupItem.Selected = true;
 //           ddlGroup.Items.Add(groupItem);

 //           roleView.RowFilter = "Name = 'Loan Officer'";
 //           if (roleView.Count > 0)
 //           {
 //               groupItem = new RadComboBoxItem(roleView[0]["Name"].ToString(), roleView[0]["id"].ToString());
 //               ddlGroup.Items.Add(groupItem);
 //           }

 //           roleView.RowFilter = "Name = 'Processor'";
 //           if (roleView.Count > 0)
 //           {
 //               groupItem = new RadComboBoxItem(roleView[0]["Name"].ToString(), roleView[0]["id"].ToString());
 //               ddlGroup.Items.Add(groupItem);
 //           }

 //           roleView.RowFilter = "Name = 'Underwriter'";
 //           if (roleView.Count > 0)
 //           {
 //               groupItem = new RadComboBoxItem(roleView[0]["Name"].ToString(), roleView[0]["id"].ToString());
 //               ddlGroup.Items.Add(groupItem);
 //           }

 //           roleView.RowFilter = "Name = 'Closer'";
 //           if (roleView.Count > 0)
 //           {
 //               groupItem = new RadComboBoxItem(roleView[0]["Name"].ToString(), roleView[0]["id"].ToString());
 //               ddlGroup.Items.Add(groupItem);
 //           }

 //           groupItem = new RadComboBoxItem("All Users", "AllUsers");
 //           ddlGroup.Items.Add(groupItem);
 //       }
#endregion