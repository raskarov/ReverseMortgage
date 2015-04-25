using System;
using System.Data;
using System.Web.UI;
using System.Xml;
using System.Collections;
using System.Web.UI.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class CheckList : AppControl
    {
        #region Constants
        private const string ROOTELEMENT = "Root";
        private const string ITEMELEMENT = "item";
        private const string IDATTRIBUTE = "id";
        private const string CBYESFIELDNAME = "cbyes";
        private const string CBNOFIELDNAME = "cbno";
        private const string CBDONTKNOWFIELDNAME = "cbdontknow";
        private const string CBTOFOLLOWFIELDNAME = "cbtofollow";
        private const string CBYESVALUEFIELDNAME = "yesvalue";
        private const string CBNOVALUEFIELDNAME = "novalue";
        private const string CBDONTKNOWVALUEFIELDNAME = "dontknowvalue";
        private const string CBTOFOLLOWVALUEFIELDNAME = "tofollowvalue";
        private const string ITEMIDFIELDNAME = "itemid";
        private const string ITEMIDATTRIBUTE = "itemid";
        private const string YESTEXT = "yes";
        private const string NOTEXT = "no";
        private const string DONTKNOWTEXT = "don't know";
        private const string TOFOLLOWTEXT = "to follow";
        private const string YESATTRIBUTE = "yes";
        private const string NOATTRIBUTE = "no";
        private const string DONOTKNOWATTRIBUTE = "donotknow";
        private const string TOFOLLOWATTRIBUTE = "tofollow";
        private const string ONCLICK = "onclick";
        private const string ONCLICKJS = "ClickCLRB(this,'{0}','{1}');";
        #endregion

        #region Fields/Properties
        MortgageProfile mp;
        private string openIssueCount = String.Empty;
        private int statusid;
        protected bool IsFirstLoad
        {
            get
            {
                Object o = Session["CheckListFirstLoad"];
                bool res = true;
                if (o != null)
                {
                    res = Convert.ToBoolean(o.ToString());
                }
                return res;
            }
            set
            {
                Session["CheckListFirstLoad"] = value;
            }
        }
        protected int SelectedStatusId
        {
            get
            {
                Object o = ViewState["SelectedStatusId"];
                int res = 0;
                if (o != null)
                {
                    res = Convert.ToInt16(o);
                }
                return res;
            }
            set
            {
                ViewState["SelectedStatusId"] = value;
            }
        }
        protected int MortgageProfileID
        {
            get
            {
                if (Session[Constants.MortgageID] == null)
                    Session[Constants.MortgageID] = 0;
                return Convert.ToInt32(Session[Constants.MortgageID].ToString());
            }
        }
        protected int NextStatusID
        {
            get
            {
                if (Session["NextStatusID"] == null)
                    Session["NextStatusID"] = 0;
                return Convert.ToInt32(Session["NextStatusID"].ToString());
            }
            set
            {
                Session["NextStatusID"] = value;
            }
        }
        protected string NextStatusName
        {
            get
            {
                if (Session["NextStatusName"] == null)
                    Session["NextStatusName"] = "";
                return Session["NextStatusName"].ToString();
            }
            set
            {
                Session["NextStatusName"] = value;
            }
        }
        #endregion

        #region Methods
        private void BindData()
        {
            if (MortgageProfileID > 0)
            {
                mp = CurrentPage.GetMortgage(MortgageProfileID);

                ddlStatus.DataSource = MortgageProfile.GetPreviousStatusList(mp.CurProfileStatusID);
                ddlStatus.DataBind();
                ddlStatus.ClearSelection();
                pnlLead.Visible = mp.CurProfileStatusID==1;
                if (CurrentUser.IsInRole(Constants.ROLEUNDEWRITER))
                {
                    PanelUnderwriter.Visible = true;
                    DataView dv = MortgageProfile.GetStatusList();
                    if (!mp.IsReadytoClose)
                    {
                        dv.RowFilter = String.Format("id<>{0}", MortgageProfile.CLOSEDSTATUSID);
                    }

                    ddlSetStatus.DataSource = dv;
                    ddlSetStatus.DataBind();
                    ddlSetStatus.Items.FindByValue(mp.CurProfileStatusID.ToString()).Selected = true;
                }

                if (SelectedStatusId <= 0)
                {
                    try
                    {
                        ddlStatus.Items.FindByValue(mp.CurProfileStatusID.ToString()).Selected = true;
                    }
                    catch{}
                }
                else
                {
                    try
                    {
                        ddlStatus.Items.FindByValue(SelectedStatusId.ToString()).Selected = true;
                    }
                    catch { }
                }

                mp.BuildRuleEvaluationTree(CurrentPage.GetRuleTreePublic());
                string xml = mp.GetRuleListXml(RuleTree.CHECKLISTBIT);
                statusid = mp.CurProfileStatusID;
                if (!String.IsNullOrEmpty(xml))
                {
                    rpChecklist.DataSource = mp.GetCheckListList(xml);
                    rpChecklist.DataBind();
                }
                trReqFields.Visible = !mp.IsFieldsCompleted();
            }
            NextStatus(statusid);
            if (NextStatusID > 0)
            {
                if (NextStatusName.Contains("Complete"))
                    btnSave.Text = NextStatusName;
                else
                    btnSave.Text = "Proceed to " + NextStatusName;
            }
            else
            {
                btnSave.Text = NextStatusName;
                btnSave.Enabled = false;
            }
        }

        protected void NextStatus(int currentstatus)
        {
            int res = 0;
            if(currentstatus==MortgageProfile.MANAGEDLEADSTATUSID)
            {
                res = MortgageProfile.LEADSTATUSID;
                NextStatusID = res;
            }
            else
            {
                int[] MPStatuses = MortgageProfile.MPStatusesCheckList();
                for (int i = 0; i < MPStatuses.Length; i++)
                {
                    if (MPStatuses[i] == currentstatus)
                        if ((i + 1) < MPStatuses.Length)
                            res = MPStatuses[i + 1];
                }
                NextStatusID = res;
            }
            if (res > 0) NextStatusName = MortgageProfile.GetStatusName(currentstatus);
        }

        protected int NextCheckListStatus(int currentstatus)
        {
            int res = -1;
            if (MortgageProfileID > 0)
            {
                mp = CurrentPage.GetMortgage(MortgageProfileID);

                int[] MPStatuses = mp.MPCheckListStatuses();
                for (int i = 0; i < MPStatuses.Length; i++)
                {
                    if (MPStatuses[i] == currentstatus)
                    {
                        if((i+1) < MPStatuses.Length) res = MPStatuses[i + 1];
                        break;
                    }
                }
            }
            return res;
        }
        protected void SaveData()
        {
            mp = CurrentPage.GetMortgage(MortgageProfileID);

            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            for (int i = 0; i < rpChecklist.Items.Count; i++)
            {
                Label lb = (Label)rpChecklist.Items[i].Controls[1];
                int itemid = int.Parse(lb.Attributes[ITEMIDATTRIBUTE]);
                CheckBox cb = (CheckBox)rpChecklist.Items[i].Controls[3];
                XmlNode n = d.CreateElement(ITEMELEMENT);
                XmlAttribute a = d.CreateAttribute(IDATTRIBUTE);
                a.Value = itemid.ToString();
                n.Attributes.Append(a);
                string attyes = "0";
                string attno = "0";
                string attdontknow = "0";
                string atttofollow = "0";
                if (cb.Visible)
                {
                    if (cb.Checked)
                    {
                        if (String.Compare(cb.Text, YESTEXT, true) == 0)
                        {
                            attyes = "1";
                        }
                        else if (String.Compare(cb.Text, NOTEXT, true) == 0)
                        {
                            attno = "1";
                        }
                        else if (String.Compare(cb.Text, DONTKNOWTEXT, true)==0)
                        {
                            attdontknow = "1";
                        }
                        else if (String.Compare(cb.Text,TOFOLLOWTEXT,true)==0)
                        {
                            atttofollow = "1";
                        }
                    }
                }
                else
                {
                    RadioButton rb1 = (RadioButton)rpChecklist.Items[i].Controls[5];
                    RadioButton rb2 = (RadioButton)rpChecklist.Items[i].Controls[7];
                    RadioButton rb3 = (RadioButton)rpChecklist.Items[i].Controls[9];
                    RadioButton rb4 = (RadioButton)rpChecklist.Items[i].Controls[11];
                    if (rb1.Visible)
                    {
                        if (rb1.Checked)
                        {
                            attyes = "1";
                        }
                    }
                    if (rb2.Visible)
                    {
                        if (rb2.Checked)
                        {
                            attno = "1";
                        }
                    }
                    if (rb3.Visible)
                    {
                        if (rb3.Checked)
                        {
                            attdontknow = "1";
                        }
                    }
                    if (rb4.Visible)
                    {
                        if (rb4.Checked)
                        {
                            atttofollow = "1";
                        }
                    }
                }
                a = d.CreateAttribute(YESATTRIBUTE);
                a.Value = attyes;
                n.Attributes.Append(a);
                a = d.CreateAttribute(NOATTRIBUTE);
                a.Value = attno;
                n.Attributes.Append(a);
                a = d.CreateAttribute(DONOTKNOWATTRIBUTE);
                a.Value = attdontknow;
                n.Attributes.Append(a);
                a = d.CreateAttribute(TOFOLLOWATTRIBUTE);
                a.Value = atttofollow;
                n.Attributes.Append(a);
                root.AppendChild(n);
            }
            d.AppendChild(root);
            mp.SaveCheckListData(d.OuterXml);
        }

        protected void EnableSaveButton()
        {
            bool enabled = CheckData();
            if (enabled && btnSave.Text.Contains("Complete")) btnSave.Enabled = false;
            else btnSave.Enabled = enabled;
            if (!mp.IsReadytoClose && (NextStatusID == MortgageProfile.CLOSEDSTATUSID))
            {
                btnSave.Enabled = false;
            }
        }

        protected bool CheckData()
        {
            bool result = false;
            ArrayList arResult = new ArrayList();
            for (int i = 0; i < rpChecklist.Items.Count; i++)
            {
                ArrayList arRes = new ArrayList();
                CheckBox cb = (CheckBox)rpChecklist.Items[i].Controls[3];
                if (!cb.Visible && (!cb.Checked && rpChecklist.Items.Count>0))
                {
                    RadioButton rb1 = (RadioButton)rpChecklist.Items[i].Controls[5];
                    RadioButton rb2 = (RadioButton)rpChecklist.Items[i].Controls[7];
                    RadioButton rb3 = (RadioButton)rpChecklist.Items[i].Controls[9];
                    RadioButton rb4 = (RadioButton)rpChecklist.Items[i].Controls[11];
                    if (rb1.Checked || rb2.Checked || rb3.Checked || rb4.Checked)
                    {
                        arRes.Add(rb1.Checked);
                        arRes.Add(rb2.Checked);
                        arRes.Add(rb3.Checked);
                        arRes.Add(rb4.Checked);
                    }
                    arResult.Add(GetGroupBoolResult(arRes));
                }
                else
                {
                    arResult.Add(cb.Checked);
                }
            }
            if (rpChecklist.Items.Count > 0) result = GetBoolResult(arResult);
            else if (rpChecklist.Items.Count == 0) result = true;
            if (NextStatusID == 0) result = false;
            return result;
        }
        protected static bool GetBoolResult(ArrayList ar)
        {
            bool res = true;
            if (ar.Count == 0)
            {
                res = false;
            }
            else
            {
                foreach (object o in ar)
                {
                    if (!(bool)o) res = false;
                }
            }
            return res;
        }
        protected static bool GetGroupBoolResult(ArrayList ar)
        {
            bool res = false;
            if (ar.Count == 0)
            {
                res = false;
            }
            else
            {
                foreach (object o in ar)
                {
                    if ((bool)o) res = true;
                }
            }
            return res;
        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
            IsFirstLoad = false;
            CurrentPage.ClientScript.RegisterHiddenField("openIssueCnt", openIssueCount);
            EnableSaveButton();
        }
        protected void rpChecklist_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string res = string.Empty;
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row != null)
                {
                    string controlName = Page.Request["__EVENTTARGET"];
                    if (!String.IsNullOrEmpty(controlName))
                    {
                        if (controlName.EndsWith("CtrlCheckList1:rpChecklist:ctl00:cb"))
                        {
                            try
                            {
                                res = Page.Request.Form[controlName.Replace(":", "$")];
                            }
                            catch
                            {
                            }
                        }
                    }

                    Label lbl = (Label)e.Item.Controls[1];
                    lbl.Attributes.Add(ITEMIDATTRIBUTE, row[ITEMIDFIELDNAME].ToString());
                    bool cbYes = bool.Parse(row[CBYESFIELDNAME].ToString());
                    if (res == "on" && cbYes) cbYes = true;
                    bool cbNo = bool.Parse(row[CBNOFIELDNAME].ToString());
                    if (res == "on" && cbNo) cbNo = true;
                    bool cbDontknow = bool.Parse(row[CBDONTKNOWFIELDNAME].ToString());
                    bool cbTofollow = bool.Parse(row[CBTOFOLLOWFIELDNAME].ToString());
                    bool cbYesValue = bool.Parse(row[CBYESVALUEFIELDNAME].ToString());
                    bool cbNoValue = bool.Parse(row[CBNOVALUEFIELDNAME].ToString());
                    bool cbDontknowValue = bool.Parse(row[CBDONTKNOWVALUEFIELDNAME].ToString());
                    bool cbTofollowValue = bool.Parse(row[CBTOFOLLOWVALUEFIELDNAME].ToString());
                    int cnt = 0;

                    CheckBox cb = (CheckBox)e.Item.Controls[3];
                    RadioButton rb1 = (RadioButton)e.Item.Controls[5];
                    RadioButton rb2 = (RadioButton)e.Item.Controls[7];
                    RadioButton rb3 = (RadioButton)e.Item.Controls[9];
                    RadioButton rb4 = (RadioButton)e.Item.Controls[11];
                    rb1.Attributes.Add(ITEMIDATTRIBUTE, row[ITEMIDFIELDNAME].ToString());
                    rb2.Attributes.Add(ITEMIDATTRIBUTE, row[ITEMIDFIELDNAME].ToString());
                    rb3.Attributes.Add(ITEMIDATTRIBUTE, row[ITEMIDFIELDNAME].ToString());
                    rb4.Attributes.Add(ITEMIDATTRIBUTE, row[ITEMIDFIELDNAME].ToString());
                    if (cbYes)
                    {
                        cnt++;
                        cb.Text = "Yes";
                        cb.Checked = cbYesValue;
                    }
                    if (cbNo)
                    {
                        cnt++;
                        cb.Text = "No";
                        cb.Checked = cbNoValue;
                    }
                    if (cbDontknow)
                    {
                        cnt++;
                        cb.Text = "Don't know";
                        cb.Checked = cbDontknowValue;
                    }
                    if (cbTofollow)
                    {
                        cnt++;
                        cb.Text = "To follow";
                        cb.Checked = cbTofollowValue;
                    }
                    cb.Visible = (cnt == 1);
                    if (cnt == 1)
                    {
                        cb.Visible = true;
                        rb1.Visible = false;
                        rb2.Visible = false;
                        rb3.Visible = false;
                        rb4.Visible = false;
                    }
                    else
                    {
                        cb.Visible = false; 
                        rb1.Visible = cbYes;
                        rb2.Visible = cbNo;
                        rb3.Visible = cbDontknow;
                        rb4.Visible = cbTofollow;
                        rb1.Checked = cbYesValue;
                        rb2.Checked = cbNoValue;
                        rb3.Checked = cbDontknowValue;
                        rb4.Checked = cbTofollowValue;
                        bool isIssueOpen = !(cbYesValue || cbNoValue || cbDontknowValue || cbTofollowValue);
                        if (isIssueOpen)
                        {
                            openIssueCount += row[ITEMIDFIELDNAME]+";";
                        }
                        string op = isIssueOpen && !btnSave.Text.Contains("Complete") ? "1" : "0";
                        rb1.Attributes.Add(ONCLICK, String.Format(ONCLICKJS,"0",op));
                        rb2.Attributes.Add(ONCLICK, String.Format(ONCLICKJS, "1", op));
                        rb3.Attributes.Add(ONCLICK, String.Format(ONCLICKJS, "2", op));
                        rb4.Attributes.Add(ONCLICK, String.Format(ONCLICKJS, "3", op));
                        rb1.GroupName = "rg";// +counter;
                        rb2.GroupName = "rg";// +counter;
                        rb3.GroupName = "rg";// +counter;
                        rb4.GroupName = "rg";// +counter;
                    }
                }
                else e.Item.Controls.Clear();
            }
        }        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if(CheckData())
            {
                mp = CurrentPage.GetMortgage(MortgageProfileID);
                NextStatus(mp.CurProfileStatusID);
                if (NextStatusID > 0)
                {
                    mp.UpdateMortgageProfileStatus(NextStatusID, CurrentUser.Id);
                    CurrentPage.RemoveMortgageFromCache(mp.ID);
                    CurrentPage.SetRuleEvaluationNeeded(true);
                    mp = CurrentPage.GetMortgage(mp.ID);
//                    ((Default) CurrentPage).appList.RepopulateMortgageList();
                    SelectedStatusId = 0;
                    Session["StatusChanged"] = true;
                    SwitchToInfoTab();
                    ((Default)CurrentPage).appList.RepopulateMortgageList();
                    //if (MortgageProfile.CheckNextStatusVisible(statusid, CurrentUser))
                    //    SwitchToInfoTab();
                    //else
                    //    SwitchToEmails();
                }
            }
            BindData();
        }
        protected void btnCloseLead_Click(object sender, EventArgs e)
        {
            mp = CurrentPage.GetMortgage(MortgageProfileID);
            mp.UpdateMortgageProfileStatus(MortgageProfile.CLOSEDLEADSTATUSID, CurrentUser.Id);
            ((Default)CurrentPage).appList.SetToFirstMortgage();
//            SwitchToInfoTab();
        }
        private int GetGroup(int statusId)
        {
            int res = ApplicantList.ACTIVELOANGROUPID;
            if(statusId == MortgageProfile.MANAGEDLEADSTATUSID)
            {
                res = ApplicantList.MANAGEDLEADGROUPID;
            }
            //else if(statusId == MortgageProfile.CLOSEDLEADSTATUSID||statusId == MortgageProfile.CLOSEDLEADSTATUSID)
            //{
            //    res = ApplicantList.CLOSEDLOANGROUPID;
            //}
            //else if(statusId == MortgageProfile.CANCELLEDSTATUSID||statusId == MortgageProfile.WITHDRAWNSTATUSID)
            //{
            //    res = ApplicantList.CANCELLEDWITHDRAWNLOANGROUPID;
            //}
            return res;
        }

        protected void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SaveData();
            BindData();
            EnableSaveButton();
        }
        protected void SwitchToInfoTab()
        {
            Tabs tabs;
            Control res = null;
            foreach (Control ctrl in CurrentPage.PanelCenter.Controls)
            {
                if (ctrl is Tabs)
                {
                    res = ctrl;
                    break;
                }
            }
            if (res != null)
            {
                ((Default)CurrentPage).appList.SetToLoanGroup(GetGroup(mp.CurProfileStatusID));
                //int statusId = mp.CurProfileStatusID;
                //if(((Default)CurrentPage).appList.IsManagedLeadSelected)
                //{
                //    if(statusId!=MortgageProfile.MANAGEDLEADSTATUSID)
                //    {
                //        ((Default)CurrentPage).appList.SetToActiveLoans();
                //    }
                //}
                //else
                //{
                //    if (statusId == MortgageProfile.MANAGEDLEADSTATUSID)
                //    {
                //        ((Default)CurrentPage).appList.SetToManagedLead();
                //    }
                //}
                tabs = (Tabs)res;
                tabs.ActivateTab(0);
                tabs.CurrentTab = 1;
                ClearSelectedTabs();
                tabs.LoadControls();
                tabs.SetTabColor();
            }
        }
        protected void SwitchToEmails()
        {
            Emails emails;
            CurrentPage.PanelCenter.Controls.Clear();
            ClearSelectedTabs();
            emails = (Emails)CurrentPage.LoadUserControl(Constants.FECTLEMAILS, CurrentPage.PanelCenter, null);
        }
        protected void ClearSelectedTabs()
        {
            Session[Constants.CURRENTTOPFIRSTTABID] = MortgageTab.TABBORROWERID;
            Session[Constants.CURRENTTOPSECONDTABINDEX] = Constants.TOPSECONDTABINDEX;
            Session[Constants.CURRENTBOTTOMTABID] = Tabs.TABLOANINFOID;
            Session[Constants.CURRENTCALCULATORTABID] = Calculator.TABLEADCALC;
        }
        #endregion

        protected void EnableCheckList(bool enabled)
        {
            foreach (RepeaterItem item in rpChecklist.Items)
            {
                foreach(Control ctr in item.Controls)
                {
                    if(ctr.GetType()== typeof(CheckBox) || ctr.GetType()== typeof(RadioButton))
                    {
                        ((WebControl)ctr).Enabled = enabled;
                    }
                }
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ddlStatus.SelectedValue))
            {
                int statusId = Convert.ToInt16(ddlStatus.SelectedValue);
                int curStatusId = mp.CurProfileStatusID;
                mp.CurProfileStatusID = statusId;
                CurrentPage.SetRuleEvaluationNeeded(true);
                mp.BuildRuleEvaluationTree(CurrentPage.GetRuleTreePublic());
                string xml = mp.GetRuleListXml(RuleTree.CHECKLISTBIT);
                mp.CurProfileStatusID = curStatusId;
                CurrentPage.SetRuleEvaluationNeeded(true);
                mp.BuildRuleEvaluationTree(CurrentPage.GetRuleTreePublic());
                if (!String.IsNullOrEmpty(xml))
                {
                    SelectedStatusId = Convert.ToInt16(ddlStatus.SelectedValue);
                    if (SelectedStatusId == mp.CurProfileStatusID)
                    {
                        rpChecklist.DataSource = mp.GetCheckListList(xml);
                        rpChecklist.DataBind();
                        EnableCheckList(true);
                        btnSave.Enabled = true;
                        EnableSaveButton();
                    }
                    else
                    {
                        rpChecklist.DataSource = mp.GetCheckListForStatus(SelectedStatusId,xml);
                        rpChecklist.DataBind();
                        EnableCheckList(false);
                        btnSave.Enabled = false;
                    }
                }
            }
        }

        protected void ddlSetStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            mp = CurrentPage.GetMortgage(MortgageProfileID);
            int statusid_;
            if (int.TryParse(ddlSetStatus.SelectedValue, out statusid_))
            {
                mp.UpdateMortgageProfileStatus(statusid_, CurrentUser.Id);
                CurrentPage.UpdateMortgage(mp, mp.ID);
                mp.RecalculateRequiredField();
                if ((statusid_== MortgageProfile.CANCELLEDSTATUSID)||(statusid_== MortgageProfile.WITHDRAWNSTATUSID))
                {
                    ((Default)CurrentPage).appList.SetToFirstMortgage();
                }
                else
                {
                    SwitchToInfoTab();
                    ((Default)CurrentPage).appList.RepopulateMortgageList();
                    //if (MortgageProfile.CheckNextStatusVisible(statusid_, CurrentUser))
                    //    SwitchToInfoTab();
                    //else
                    //    SwitchToEmails();
                    BindData();
                }
            }
        }
    }
}