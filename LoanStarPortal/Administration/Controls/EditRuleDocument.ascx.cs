using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;
using Rule=LoanStar.Common.Rule;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditRuleDocument : RuleControl
    {
        #region Private members
        private void RefreshHeader(bool isEdit)
        {
            lblHeader.Text = (isEdit ? "Edit document for rule" : "Add document for rule") + "(" + rule.Name + ")";
        }

        private void PopulateDocumentGroups()
        {
            if (ddlSelectDocGroup.DataSource != null)
                return;

            GroupDocList.SelectCommand = "GetDocumentGroupList";
            GroupDocList.ConnectionString = AppSettings.SqlConnectionString;

            ddlSelectDocGroup.DataSource = GroupDocList;
            ddlSelectDocGroup.DataTextField = "GroupName";
            ddlSelectDocGroup.DataValueField = "GroupID";
            ddlSelectDocGroup.DataBind();
            ddlSelectDocGroup.Items.Insert(0, new ListItem("Select", "0"));
        }

        private void RepopulateDocTemplates()
        {
            DataTable dtTable = rule.GetRuleObjectList(Rule.DOCUMENTOBJECTTYPEID).Table;

            RepopulateUnBoundDocTemplates(dtTable);
            RepopulateBoundDocTemplates(dtTable);
        }

        private void RepopulateUnBoundDocTemplates(DataTable dtTable)
        {
            DataView docTemplateList = new DataView(dtTable);
            docTemplateList.RowFilter = "ID = 0";

            ddlSelectDoc.DataSource = docTemplateList;
            ddlSelectDoc.DataTextField = "DTTitle";
            ddlSelectDoc.DataValueField = "DT_DTRelGroup_ID";//"DTID";
            ddlSelectDoc.DataBind();
            ddlSelectDoc.Items.Insert(0, new ListItem("Select", "0;0"));
        }

        private void RepopulateBoundDocTemplates(DataTable dtTable)
        {
            DataView ruleDTRelationList = new DataView(dtTable);
            ruleDTRelationList.RowFilter = "ID > 0";

            RadGridAssignedDocTemplates.DataSource = ruleDTRelationList;
            RadGridAssignedDocTemplates.DataBind();
        }

        private void CanselEditMode()
        {
            RadGridAssignedDocTemplates.MasterTableView.ClearEditItems();
        }
        #endregion

        #region Event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            PopulateDocumentGroups();
        }

        protected void RadGridAssignedDocTemplates_ItemCommand(object source, GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case RadGrid.UpdateCommandName:
                    {
                        GridEditableItem editItem = (GridEditableItem)e.Item;
                        int groupID = Convert.ToInt32( (editItem["GroupDocList"].Controls[0] as DropDownList).SelectedValue );
                        bool isAppPackage = (editItem["IsAppPackage"].Controls[0] as CheckBox).Checked;
                        bool isClosingPackage = (editItem["IsClosingPackage"].Controls[0] as CheckBox).Checked;
                        bool isMiscPackage = (editItem["IsMiscPackage"].Controls[0] as CheckBox).Checked;
                        int dtRelationID = Convert.ToInt32( e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DTRelID"].ToString() );

                        int resAction;
                        try
                        {
                            resAction = rule.SaveDocument(dtRelationID, 0, groupID, isAppPackage, isClosingPackage, isMiscPackage);
                        }
                        catch
                        {
                            resAction = 0;
                        }
                        lblActionInfoArea.Text = resAction == 0 ? "Database error!" : "Operation compleeted";

                        DataTable dtTable = rule.GetRuleObjectList(Rule.DOCUMENTOBJECTTYPEID).Table;
                        RepopulateBoundDocTemplates(dtTable);
                    }
                    break;
                case RadGrid.DeleteCommandName:
                    {
                        int ROID = Convert.ToInt32( e.CommandArgument.ToString() );

                        bool resAction;
                        try
                        {
                            resAction = rule.DeleteObject(ROID);
                        }
                        catch
                        {
                            resAction = false;
                        }
                        lblActionInfoArea.Text = resAction == false ? "Database error!" : "Operation compleeted";

                        RepopulateDocTemplates();
                        refreshMainGrid();
                    }
                    break;
                case RadGrid.EditCommandName:
                case RadGrid.SortCommandName:
                default:
                    {
                        DataTable dtTable = rule.GetRuleObjectList(Rule.DOCUMENTOBJECTTYPEID).Table;
                        RepopulateBoundDocTemplates(dtTable);
                        lblActionInfoArea.Text = String.Empty;
                    }
                    break;
            }

            RefreshHeader(e.CommandName == RadGrid.EditCommandName);
        }

        protected void RadGridAssignedDocTemplates_SortCommand(object source, GridSortCommandEventArgs e)
        {
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            goBack();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int resAction;
            try
            {
                int dtID = Convert.ToInt32( ddlSelectDoc.SelectedValue.Split(';')[0] );
                int groupID = Convert.ToInt32( ddlSelectDocGroup.SelectedValue );
                bool isAppPackage = cbAppPackage.Checked;
                bool isClosingPackage = cbClosPackage.Checked;
                bool isMiscPackage = cbMiscPackage.Checked;

                resAction = rule.SaveDocument(0, dtID, groupID, isAppPackage, isClosingPackage, isMiscPackage);
            }
            catch
            {
                resAction = 0;
            }
            lblActionInfoArea.Text = resAction <= 0 ? "Database error!" : "Operation compleeted";

            CanselEditMode();
            RepopulateDocTemplates();
            RefreshHeader(false);

            refreshMainGrid();
        }
        #endregion

        #region Public methods
        public override void Initialize()
        {
            base.Initialize();

            CanselEditMode();
            RepopulateDocTemplates();
            RefreshHeader(false);
            tdruleexp.InnerHtml = RULECODETEXT + rule.GetColoredCodeById();
        }
        #endregion
    }
}