using System;
using System.Collections;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class FieldDetails : AppControl
    {
        #region constnats
        private const string POSTBACKCONTROL = "__EVENTTARGET";
        private const string HEADER = "{0} - field details";
        private const string GROUPID = "gid";
        private const string MODE = "fieldmode";
        private const int VIEWMODE = 0;
        private const int EDITMODE = 1;
        private const string EDITFIELDID = "editfieldid";
        private const string EDITGROUPID = "editgroupid";
        #endregion

        #region fields
        private int currentOrder;
        private FieldVisualData editField;
        #endregion

        #region properties
        private int fieldId
        {
            get
            {
                int res = -1;
                Object o = ViewState[EDITFIELDID];
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
            set { ViewState[EDITFIELDID] = value;}
        }
        private int groupId
        {
            get
            {
                int res = 0;
                Object o = ViewState[EDITGROUPID];
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
            set { ViewState[EDITGROUPID] = value; }
        }
        protected int CurrentMode
        {
            get
            {
                int res = VIEWMODE;
                Object o = Session[MODE];
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
            set { Session[MODE] = value; }
        }
        public bool IsInited
        { 
            get
            {
                bool res = false;
                Object o = Session["editfieldinit"];
                if (o != null)
                {
                    try
                    {
                        res = bool.Parse(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set { Session["editfieldinit"] = value; }
        }
        private FieldVisualData CurrentEditField
        {
            get
            {
                return Session["currentvisualdat"] as FieldVisualData;
            }
            set { Session["currentvisualdat"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fieldId = CurrentPage.GetValueInt(Constants.IDPARAM, fieldId);
                groupId = CurrentPage.GetValueInt(GROUPID, groupId);
            }
            ProcessPostBack();
            BindData();
        }
        private void ProcessPostBack()
        { 
            string controlName = Page.Request[POSTBACKCONTROL];
            if (!String.IsNullOrEmpty(controlName)&&(CurrentEditField != null))
            {
                if (controlName.EndsWith("$ddlTopLevelTab"))
                {
                    CurrentEditField.ToplevelTabId = GetDdlSelectedValue(controlName);
                }
                else if (controlName.EndsWith("$ddlSecondLevelTab"))
                {
                    CurrentEditField.Level2TabId = GetDdlSelectedValue(controlName);
                }
                else if (controlName.EndsWith("$ddlPseudoTab"))
                {
                    CurrentEditField.PseudoTabId = GetDdlSelectedValue(controlName);
                }
            }
        }
        private int GetDdlSelectedValue(string controlName)
        {
            int res = -1;
            try
            {
                res = int.Parse(Page.Request.Form[controlName]);
            }
            catch
            {
            }
            return res;
        }
        private void BindData()
        {
            DataSet ds = MortgageProfileFieldInfo.GetFieldDetails(fieldId);
            DataView generalInfo = ds.Tables[0].DefaultView;
            generalInfo.RowFilter = String.Format("groupid={0}", groupId);
            if(generalInfo.Count==1)
            {
                BindGeneralInfo(generalInfo[0]);
                DataView ruleInfo = ds.Tables[1].DefaultView;
                BindRuleInfo(ruleInfo);
                DataView visibilityInfo = ds.Tables[2].DefaultView;
                BindVisibilityInfo(visibilityInfo);
                DataView mappingInfo = ds.Tables[3].DefaultView;
                BindDocMappingInfo(mappingInfo);
            }
        }
        private void BindDocMappingInfo(ICollection dvMapInfo)
        {
            if ((dvMapInfo != null) && (dvMapInfo.Count > 0))
            {
                G.DataSource = dvMapInfo;
                G.DataBind();
            }
            else 
            {
                lblDocMappingInfo.Text = "not used in documents";
                trMapInfo.Visible = false;
            }
        }
        private void BindVisibilityInfo(DataView dvVisibility)
        {
            if((dvVisibility!=null)&&(dvVisibility.Count>0))
            {
                trVisibilityInfo.Visible = true;
                string txt = dvVisibility[0]["propertyName"].ToString();
                for(int i=1;i<dvVisibility.Count;i++)
                {
                    txt += "," + dvVisibility[i]["propertyName"];
                }
                lblVisibility.Text = txt;
            }
            else
            {
                trVisibilityInfo.Visible = false;
            }
        }

        private void BindRuleInfo(DataView dvRules)
        {
            if((dvRules!=null)&&(dvRules.Count>0))
            {
                lblRules.Text = String.Format("Used in {0} rule(s)", dvRules.Count);
                trRuleInfo.Visible = true;
                for(int i=0;i<dvRules.Count;i++)
                {
                    tblRuleInfo.Rows.Add(GetOneRule(dvRules[i],i));
                }
            }
            else
            {
                lblRules.Text = "Not used in rules";
                trRuleInfo.Visible = false;
            }
        }
        private static HtmlTableRow GetOneRule(DataRowView dr,int i)
        {
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell td1 = new HtmlTableCell();
            td1.InnerText = (i + 1) + ".";
            td1.Attributes.Add("style","width:10px");
            tr.Cells.Add(td1);
            HtmlTableCell td = new HtmlTableCell();
            int ruleId = int.Parse(dr["ruleid"].ToString());
            RuleEditNode node = new RuleEditNode(ruleId);
            td.InnerHtml = node.RuleExpression;
            tr.Cells.Add(td);
            return tr;
        }
        private void BindGeneralInfo(DataRowView dr)
        {
            string propertyName = dr["propertyname"].ToString();
            lblHeader.Text = String.Format(HEADER, propertyName);
            BindEditableData(dr);
            lblCalculated.Text = bool.Parse(dr["calculated"].ToString()) ? "Yes" : "No";
            lblGroup.Text = dr["groupname"].ToString();
            lblUsedInRules.Text = bool.Parse(dr["usedinrules"].ToString()) ? "Yes" : "No";
            string table = dr["tablename"].ToString();
            lblDictionary.Text = String.IsNullOrEmpty(table) ? "No" : "Yes(" + table + ")";
            lblControl.Text = dr["controlname"].ToString();
            string status = dr["statusname"].ToString();
            lblRequired.Text = String.IsNullOrEmpty(status) ? "No" : "Yes(" + status + ")";
            string typeName;
            bool isNullable;
            bool isReadonly;
            MortgageProfileFieldInfo.GetBLLFieldInfo(propertyName, out typeName, out isNullable, out isReadonly);
            lblType.Text = typeName + (isNullable ? " ,Can be null " : "") + (isReadonly ? " ,Readonly " : "");
            string dbTypeName;
            string tableName;
            int maxLength;
            MortgageProfileFieldInfo.GetDbFieldInfo(propertyName, out tableName, out dbTypeName, out isNullable, out maxLength);
            if (dbTypeName != "N/A")
            {
                lblDbType.Text = (String.IsNullOrEmpty(tableName) ? "" : "Table - " + tableName + "  ,") + dbTypeName + (maxLength > 0 ? " ,Max length=" + maxLength : "") + (isNullable ? " ,Can be null " : "");
            }
            else
            {
                lblDbType.Text = dbTypeName;
            }
        }
        private void BindEditableData(DataRowView dr)
        {
            editField = new FieldVisualData(dr);
            if ((!IsInited)||(CurrentEditField==null))
            {
                CurrentEditField = editField;
                IsInited = true;
            }
            lblDisplay.Text = CurrentEditField.Label;
            tbDisplayLabel.Text = CurrentEditField.Label;
            string location = dr["location"].ToString();
            lblLocation.Text = location;
            currentOrder = CurrentEditField.DisplayOrder;            
            lblDisplayOrder.Text = currentOrder > 0 ? currentOrder.ToString() : "invisible";
            tbDisplayOrder.Text = currentOrder.ToString();
            bool isLable = CurrentMode == VIEWMODE;
            lblDisplay.Visible = isLable;
            tbDisplayLabel.Visible = !isLable;
            lblLocation.Visible = isLable;
            dvLocations.Visible = !isLable;
            lblDisplayOrder.Visible = isLable;
            tbDisplayOrder.Visible = !isLable;
            btnSave.Visible = !isLable;
            btnCancel.Visible = !isLable;
            btnEdit.Visible = isLable;
            if (dvLocations.Visible) 
            {
                BindLocation();
            }
        }
        private void BindLocation()
        { 
            DataView dv  = MortgageProfileFieldInfo.GetTopLevelTab();
            BindDdl(ddlTopLevelTab,dv,"id","tablevel2name",CurrentEditField.ToplevelTabId);
            DataView dv1 = MortgageProfileFieldInfo.GetSecondLevelTabs();
            dv1.RowFilter = String.Format(ManageFields.TABFILTER, int.Parse(ddlTopLevelTab.SelectedValue));
            BindDdl(ddlSecondLevelTab, dv1, "id", "header",CurrentEditField.Level2TabId);
            DataView dv2 = MortgageProfileFieldInfo.GetPseudoTabs();
            dv2.RowFilter = String.Format(ManageFields.PSEUDOTABFILTER, int.Parse(ddlSecondLevelTab.SelectedValue));
            BindDdl(ddlPseudoTab, dv2, "id", "header", CurrentEditField.PseudoTabId);
            DataView dv3 = MortgageProfileFieldInfo.GetPseudoTabGroups();
            dv3.RowFilter = String.Format("pseudotabId={0}", ddlPseudoTab.SelectedValue);
            BindDdl(ddlPseudoTabGroup, dv3, "id", "header", CurrentEditField.PseudoTabGroupId);
            //for (int i = 0; i < ddlPseudoTabGroup.Items.Count; i++)
            //{
            //    ddlPseudoTabGroup.Items[i].Text = (i + 1).ToString();
            //}
        }
        private static void BindDdl(ListControl ddl, DataView dv, string dataFieldName, string textFieldName, int selected)
        {
            ddl.DataSource = dv;
            ddl.DataTextField = textFieldName;
            ddl.DataValueField = dataFieldName;
            ddl.DataBind();
            try
            {
                ddl.SelectedValue = selected.ToString();
            }
            catch 
            {
                int.Parse(ddl.Items[0].Value);
            }
        }
        #region event handlers
        protected void btnSave_Click(object sender, EventArgs e)
        {
            CurrentMode = VIEWMODE;
            CurrentEditField.Label = tbDisplayLabel.Text;
            int order = currentOrder;
            try
            {
                order = int.Parse(tbDisplayOrder.Text);
            }
            catch { }
            CurrentEditField.DisplayOrder = order;
            CurrentEditField.ToplevelTabId = int.Parse(ddlTopLevelTab.SelectedValue);
            CurrentEditField.Level2TabId = int.Parse(ddlSecondLevelTab.SelectedValue);
            CurrentEditField.PseudoTabId = int.Parse(ddlPseudoTab.SelectedValue);
            CurrentEditField.PseudoTabGroupId = int.Parse(ddlPseudoTabGroup.SelectedValue);
            if (CurrentEditField.Update(fieldId, groupId)>0)
            {
                groupId = CurrentEditField.PseudoTabGroupId;
            }
            BindData();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CurrentMode = VIEWMODE;
            BindData();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            CurrentMode = EDITMODE;
            BindData();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.VIEWFIELDSNEW);
        }
        #endregion
    }
}