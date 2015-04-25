using System;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class ImportLead : AppControl
    {
        #region constants
        private const int STEPLOAD = 1;
        private const int STEPIMPORT = 2;
        private const int STEPDONE = 3;
        private const string IMPORTSTEP = "importstep";
        private const string LEADIMPORT = "leadimport";
        private const string ONCHANGEATTRIBUTE = "onchange";
        private const string JSENABLEUPLOAD = "EnableNext(this,'{0}','{1}');";
        private const string JSENABLEIMPORT = "EnableImport(this,'{0}');";
        
        #endregion

        #region fields
        #endregion

        #region properties
        private LeadImport Import
        {
            get
            {
                Object o = Session[LEADIMPORT];
                return o as LeadImport;
            }
            set { Session[LEADIMPORT] = value; }
        }
        private int Step
        {
            get
            {
                int res = STEPLOAD;
                Object o = ViewState[IMPORTSTEP];
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
            set { ViewState[IMPORTSTEP] = value; }
        }
        #endregion

        #region methods
        private void SetStepControls()
        {
            divStep1.Visible = false;
            divStep2.Visible = false;
            switch (Step)
            {
                case STEPLOAD:
                    divStep1.Visible = true;
                    lblHeader.Text = "Step 1(Upload file)";
                    break;
                case STEPIMPORT:
                    divStep2.Visible = true;
                    lblHeader.Text = "Step 2(Import data)";
                    break;
                case STEPDONE:
                    lblHeader.Text = String.Format("Import completed. {0} loans were created.",Import.ImportedRows);
                    Step = STEPLOAD;
                    Import = null;
                    break;
            }
        }
        private void BindData()
        {
            switch (Step)
            {
                case STEPLOAD:
                    BindLoadStepData();
                    break;
                case STEPIMPORT:
                    BindImportStepData();
                    break;
            }
        }

        #region Step 1 methods
        private void BindLoadStepData()
        {
            ddlImportSource.DataSource = LeadImport.LeadImportSourceList();
            ddlImportSource.DataTextField = "Name";
            ddlImportSource.DataValueField = "Id";
            ddlImportSource.DataBind();
            ddlImportSource.Attributes.Add(ONCHANGEATTRIBUTE, String.Format(JSENABLEUPLOAD, trUpload.ClientID, btnExecuteStep1.ClientID));
        }

        #region step 1 handlers
        protected void btnExecuteStep1_Click(object sender, EventArgs e)
        {
            if (UploadImportData.PostedFile.ContentLength > 0)
            {
                Import = new LeadImport(int.Parse(ddlImportSource.SelectedValue), CurrentUser.EffectiveCompanyId, CurrentUser.Id, Server.MapPath(Constants.STORAGEFOLDER + "/LeadImport"), CurrentPage.GetDictionary);
                if (Import.ExecuteStep(Step, UploadImportData.PostedFile))
                {
                    Step++;
                    SetStepControls();
                    BindData();
                }
                else
                {
                    lblMessage.Text = Import.ErrorMessage;
                    trUpload.Attributes.Add("style", "display:inline");
                    btnExecuteStep1.Enabled = true;
                }
            }
            else
            {
                lblMessage.Text = "Please select file to upload";
                trUpload.Attributes.Add("style", "display:inline");
                btnExecuteStep1.Enabled = true;
            }
        }
        #endregion

        #endregion

        #region Step 2 methods
        private void BindImportStepData()
        {
            BindLoanOfficer();
            BindImportedRows();
            BindRejectedRows();
        }
        private void BindLoanOfficer()
        {
            ddlLoanOfficer.DataSource = Company.GetLoanOfficerList(CurrentUser.EffectiveCompanyId);
            ddlLoanOfficer.DataTextField = "name";
            ddlLoanOfficer.DataValueField = "id";
            ddlLoanOfficer.DataBind();
            ddlLoanOfficer.Items.Insert(0,new ListItem("-Select-","0"));
            ddlLoanOfficer.Attributes.Add(ONCHANGEATTRIBUTE, String.Format(JSENABLEIMPORT, btnStep2.ClientID));
            
        }

        private void BindImportedRows()
        {
            lblImportedRows.Text = String.Format("{0} loan(s) has been created from total {1} row(s)", Import.GoodRows,Import.TotalRows);
            gImportedRows.DataSource = Import.DtImportedRows;
            gImportedRows.DataBind();
        }
        private void BindRejectedRows()
        {
            bool hasErrors = Import.BadRows > 0;
            if(hasErrors)
            {
                //Import.CreateBadRowsFile();
                lblBadRows.Text = String.Format("{0} raw(s) was rejected", Import.BadRows);
                gBadRows.DataSource = Import.DtBadRows;
                gBadRows.DataBind();
            }
            else
            {
                trBadRowsGrid.Visible = false;
                trBadrowsheader.Visible = false;
            }
        }
        protected void gImportedRows_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.DeleteCommandName)
            {
                Import.DeleteRow(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString()));
            }
            BindImportedRows();
        }
        protected void gBadRows_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "viewError")
            {
                string err = Import.GetErrorById(int.Parse(e.CommandArgument.ToString())).Replace("\r\n", "\\n").Replace("'","`");
                CurrentPage.ClientScript.RegisterStartupScript(GetType(), "ShowError", "<script language=\"javascript\" type=\"text/javascript\">ShowError('"+err+"');</script>");
                BindRejectedRows();
            }
        }
        #region step 2 handlers
        protected void btnExecuteStep2_Click(object sender, EventArgs e)
        {
            if(Import.ExecuteStep(Step, int.Parse(ddlLoanOfficer.SelectedValue)))
            {
                Step++;
                Import.CleanupStep(Step);
                SetStepControls();
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Import.CleanupStep(Step);
            Import = null;
            Step = STEPLOAD;
            SetStepControls();
            BindData();
        }
        #endregion
        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetStepControls();
            BindData();
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Step == STEPIMPORT)
            {
                btnStep2.Enabled = ddlLoanOfficer.SelectedValue != null && ddlLoanOfficer.SelectedValue != "0";
            }
        }
    }
}
#region unused
        //private void BindMapStepData()
        //{
        //    BindGroups();
        //    BindIndex();
        //    BindLoanFields();
        //    BindNotBoundColumns();
        //    BindBoundColumns();
        //}
        //private void BindGroups()
        //{
        //    ddlGroup.DataSource = DvFieldGroups;
        //    ddlGroup.DataTextField = "Name";
        //    ddlGroup.DataValueField = "Id";
        //    ddlGroup.DataBind();
        //    ddlGroup.Items.Insert(0,new ListItem("- Select ","0"));
        //    ddlGroup.SelectedValue = SelectedGroupId.ToString();
        //}
        //private void BindLoanFields()
        //{
        //    DataView dv = MortgageProfile.GetFieldsByGroupId(SelectedGroupId);
        //    gLoanProperty.DataSource = dv;
        //    if (dv.Count > 0)
        //    {
        //        if(isGroupChanged)
        //        {
        //            CheckForIndex(dv[0]["propertyname"].ToString());
        //        }
        //    }
        //    gLoanProperty.DataBind();
        //}
        //private void BindNotBoundColumns()
        //{
        //    gNotMappedFields.DataSource = Import.NotMappedFields;
        //    gNotMappedFields.DataBind();
        //}
        //private void BindBoundColumns()
        //{
        //    DataView dv = Import.MappedFields.DefaultView;
        //    dv.Sort = "PropertyName";
        //    gMappedFields.DataSource = dv;
        //    gMappedFields.DataBind();
        //}
        //private void BindIndex()
        //{
        //    ddlIndex.Items.Clear();
        //    for (int i = 0; i < MAXINDEXVALUE; i++)
        //    {
        //        ddlIndex.Items.Add(new ListItem(i.ToString(), i.ToString()));
        //    }
        //}
        //private void CheckForIndex(string propertyName)
        //{
        //    Type t;
        //    if(MortgageProfile.IsPropertyList(propertyName, out t))
        //    {
        //        SelectedIndex = int.Parse(ddlIndex.SelectedValue);
        //    }
        //    else
        //    {
        //        SelectedIndex = -1;
        //    }
        //}
        #region Loan properties grid methods
        //protected string GetPropertyName(Object item)
        //{
        //    DataRowView dr = (DataRowView)item;
        //    string res = dr["propertyName"].ToString();
        //    if (SelectedIndex>=0)
        //    {
        //        res = GetIndexedProperty(res);
        //    }
        //    return res;
        //}
        //private string GetIndexedProperty(string data)
        //{
        //    string[] parts = data.Split('.');
        //    if (parts.Length != 2)
        //    {
        //        return data;
        //    }
        //    return String.Format("{0}[{1}].{2}", parts[0], SelectedIndex, parts[1]);
        //}
        //protected void gLoanProperty_ItemDataBound(object sender, GridItemEventArgs e)
        //{
        //    if (e.Item is GridDataItem)
        //    {
        //        DataRowView row = (DataRowView)e.Item.DataItem;
        //        GridDataItem item = e.Item as GridDataItem;
        //        if (item != null)
        //        {
        //            item["Description"].ToolTip = row["Description"].ToString();
        //            item["PropertyName"].ToolTip = row["PropertyName"].ToString();
        //        }
        //    }
        //}
        #endregion
        #region step 2 handlers
        //public void RaisePostBackEvent(string eventArgument)
        //{
        //    switch (eventArgument)
        //    {
        //        case "groupchange":
        //            SelectedGroupId = int.Parse(ddlGroup.SelectedValue);
        //            gLoanProperty.PageSize = 15;
        //            isGroupChanged = true;
        //            BindLoanFields();
        //            gLoanProperty.ResponseScripts.Add(String.Format("SetControls({0});", (SelectedIndex>=0) ? 1 : 0));
        //            break;
        //        case "indexchange":
        //            SelectedIndex = int.Parse(ddlIndex.SelectedValue);
        //            gLoanProperty.PageSize = 15;
        //            BindLoanFields();
        //            gLoanProperty.ResponseScripts.Add(String.Format("SetControls({0});", (SelectedIndex >= 0) ? 1 : 0));
        //            break;
        //        default:
        //            break;
        //    }

        //}
        //protected void btnUnBound_Click(object sender, EventArgs e)
        //{
        //    if(gMappedFields.SelectedItems.Count == 0)
        //    {
        //        return;
        //    }
        //    foreach (GridDataItem item in gMappedFields.SelectedItems)
        //    {
        //        DataRowView dr = item.DataItem as DataRowView;
        //        if(dr != null)
        //        {
        //            Import.Unmap(dr["id"].ToString());
        //        }
        //    }
        //    BindNotBoundColumns();
        //    BindBoundColumns();
        //    RadAjaxManager1.ResponseScripts.Add(String.Format("SetControls({0});", (SelectedIndex >= 0) ? 1 : 0));
        //}
        //protected void btnBound_Click(object sender, EventArgs e)
        //{
        //    if(gLoanProperty.SelectedItems.Count!=1 || gNotMappedFields.SelectedItems.Count!=1)
        //    {
        //        return;
        //    }
        //    DataRowView propertyRow = gLoanProperty.SelectedItems[0].DataItem as DataRowView;
        //    DataRowView columnRow = gNotMappedFields.SelectedItems[0].DataItem as DataRowView;
        //    if(propertyRow!=null&&columnRow!=null)
        //    {
        //        string propertyName = propertyRow["PropertyName"].ToString();
        //        string columnName = columnRow["columnName"].ToString();
        //        string key = columnRow["Id"].ToString();
        //        if (Import.Map(key, columnName, (SelectedIndex >= 0) ? GetIndexedProperty(propertyName) : propertyName))
        //        {
        //            BindNotBoundColumns();
        //            BindBoundColumns();
        //        }
        //    }
        //    RadAjaxManager1.ResponseScripts.Add(String.Format("SetControls({0});", (SelectedIndex >= 0) ? 1 : 0));
        //}
        //protected void btnExecuteStep2_Click(object sender, EventArgs e)
        //{
        //    if (Import.ExecuteStep(Step, null))
        //    {
        //        Step++;
        //        SetStepControls();
        //        BindData();
        //    }
        //    else
        //    {
        //        lblMessage.Text = Import.ErrorMessage;
        //        trUpload.Attributes.Add("style", "display:inline");
        //        btnExecuteStep1.Enabled = true;
        //    }
        //}
        #endregion

#endregion