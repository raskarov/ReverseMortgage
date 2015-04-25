using System;
using System.Data;
using System.Web.UI;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class DocFieldMapping : AppControl, IPostBackEventHandler
    {
        #region Private fields
        private DocTemplateVersion docTemplateVersion;
        #endregion

        #region Public methods
        public void BindData()
        {
            ddlMPFieldGroup.DataSource = Field.GetFieldGroupIndex();
            ddlMPFieldGroup.DataTextField = "Name";
            ddlMPFieldGroup.DataValueField = "ComplexID";
            ddlMPFieldGroup.DataBind();

            int index = 1;
            DataView regionView = DocTemplateVersion.GetRegionList();
            foreach (DataRowView regionRow in regionView)
                if (Convert.ToInt32(regionRow["IsRegion"]) > 0)
                {
                    regionRow["IsRegion"] = index;
                    index++;
                }
            ddlUnBoundFieldRegion.DataSource = regionView;
            ddlUnBoundFieldRegion.DataTextField = "RegionName";
            ddlUnBoundFieldRegion.DataValueField = "IsRegion";
            ddlUnBoundFieldRegion.DataBind();

            MPFieldsGrid.Rebind();
            DTUnBoundFieldsGrid.Rebind();
            DTBoundFieldsGrid.Rebind();
        }
        #endregion

        #region Private Methods
        private void RebindBoundAndUnBoundGrids()
        {
            string errMessage = DocTemplateVersion.UpdateFieldList();
            lblActionInfoArea.Text = String.IsNullOrEmpty(errMessage) ? "Operation compleeted" : errMessage;

            DTUnBoundFieldsGrid.Rebind();
            DTBoundFieldsGrid.Rebind();
        }

        private bool AnalyseDTVFieldMapping()
        {
            string selValues = ddlMPFieldGroup.SelectedValue;
            string[] selValuesArr = selValues.Split(',');
            int fieldGroupID = Convert.ToInt32(selValuesArr[0]);
            int groupIndex1 = Convert.ToInt32(selValuesArr[1]);
            int groupIndex2 = Convert.ToInt32(selValuesArr[2]);
            string selRegionName = Convert.ToInt32(ddlUnBoundFieldRegion.SelectedValue) > 0 ? ddlUnBoundFieldRegion.SelectedItem.Text : String.Empty;

            return DocTemplateVersion.AnalyseDTVFieldMapping(selRegionName, fieldGroupID, groupIndex1, groupIndex2);
        }
        #endregion

        #region Properties
        public DocTemplateVersion DocTemplateVersion
        {
            get
            {
                return docTemplateVersion;
            }
            set
            {
                docTemplateVersion = value;
                dtvFormatType.Value = docTemplateVersion == null ?
                    String.Empty : ((int)docTemplateVersion.DTVFormatType).ToString();
            }
        }
        #endregion

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            int canBind = Convert.ToInt32(AnalyseDTVFieldMapping());
            switch (eventArgument)
            {
                case "RefreshMPFieldsGrid":
                    MPFieldsGrid.PageSize = 15;
                    MPFieldsGrid.Rebind();
                    //MPFieldsGrid.ResponseScripts.Add(String.Format("SetAllowMapping({0});", canBind));
                    break;
                case "RefreshUnBoundFieldsGrid":
                    DTUnBoundFieldsGrid.Rebind();
                    //DTUnBoundFieldsGrid.ResponseScripts.Add(String.Format("SetAllowMapping({0});", canBind));
                    break;
                default:
                    break;
            }
        }

        protected void MPFieldsGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            string selValues = ddlMPFieldGroup.SelectedValue;
            string[] selValuesArr = selValues.Split(',');
            int fieldGroupID = Convert.ToInt32(selValuesArr[0].Trim());
            int groupIndex1 = Convert.ToInt32(selValuesArr[1].Trim());
            int groupIndex2 = Convert.ToInt32(selValuesArr[2].Trim());

            MPFieldsGrid.DataSource = Field.GetFieldInGroupWithSelectedType(fieldGroupID, groupIndex1, groupIndex2);
        }

        protected void DTUnBoundFieldsGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            string selRegionName = Convert.ToInt32(ddlUnBoundFieldRegion.SelectedValue) > 0 ? ddlUnBoundFieldRegion.SelectedItem.Text : String.Empty;
            string regionNameFilter = String.IsNullOrEmpty(selRegionName) ? 
                                                "RegionName IS NULL" : String.Format("RegionName = '{0}'", selRegionName);
            
            DataView unBoundDocFieldView = DocTemplateVersion.UnBoundDocFieldView;
            unBoundDocFieldView.RowFilter += String.IsNullOrEmpty(unBoundDocFieldView.RowFilter) ?
                                                regionNameFilter : String.Format(" AND {0}", regionNameFilter);

            DTUnBoundFieldsGrid.DataSource = unBoundDocFieldView;
        }

        protected void DTBoundFieldsGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            DTBoundFieldsGrid.DataSource = DocTemplateVersion.BoundDocFieldView;
        }

        protected void btnBound_Click(object sender, EventArgs e)
        {
            if (MPFieldsGrid.SelectedItems.Count == 0 || DTUnBoundFieldsGrid.SelectedItems.Count == 0)
                return;

            GridDataItem mpFieldItem = (GridDataItem)MPFieldsGrid.SelectedItems[0];
            int mpFiledID = Convert.ToInt32(mpFieldItem["id"].Text);
            string mpFieldName = mpFieldItem["Description"].Text.Replace("<nobr>", null).Replace("</nobr>", null);

            string selValues = ddlMPFieldGroup.SelectedValue;
            string[] selValuesArr = selValues.Split(',');
            int groupIndex1 = Convert.ToInt32(selValuesArr[1]);
            int groupIndex2 = Convert.ToInt32(selValuesArr[2]);
            string groupName = ddlMPFieldGroup.SelectedItem.Text;
            string selRegionName = Convert.ToInt32(ddlUnBoundFieldRegion.SelectedValue) > 0 ? ddlUnBoundFieldRegion.SelectedItem.Text : String.Empty;

            foreach (GridDataItem dtUnBoundFieldItem in DTUnBoundFieldsGrid.SelectedItems)
            {
                string dtvFieldName = dtUnBoundFieldItem["DTVFieldName"].Text.Replace("<nobr>", null).Replace("</nobr>", null);
                DocTemplateVersion.ChangeDocFieldMap(mpFiledID, mpFieldName, dtvFieldName, selRegionName, groupIndex1, groupIndex2, groupName);
            }

            RebindBoundAndUnBoundGrids();
        }

        protected void btnUnBound_Click(object sender, EventArgs e)
        {
            if (DTBoundFieldsGrid.SelectedItems.Count == 0)
                return;
            string selRegionName = Convert.ToInt32(ddlUnBoundFieldRegion.SelectedValue) > 0 ? ddlUnBoundFieldRegion.SelectedItem.Text : String.Empty;

            foreach (GridDataItem dtBoundFieldItem in DTBoundFieldsGrid.SelectedItems)
            {
                string dtvFieldName = dtBoundFieldItem["DTVFieldName"].Text.Replace("<nobr>", null).Replace("</nobr>", null);
                DocTemplateVersion.ChangeDocFieldMap(0, "- Select -", dtvFieldName, selRegionName, 0, 0, "- Select -");
            }

            RebindBoundAndUnBoundGrids();

            int canBind = Convert.ToInt32(AnalyseDTVFieldMapping());
            RadAjaxManager1.ResponseScripts.Add(String.Format("SetAllowMapping({0});", canBind));
        }

        protected void MPFieldsGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                GridDataItem gridItem = e.Item as GridDataItem;
                gridItem["Description"].ToolTip = row["Description"].ToString();
                gridItem["MPFieldName"].ToolTip = row["MPFiledName"].ToString();
            }
        }

        protected void DTUnBoundFieldsGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                GridDataItem gridItem = e.Item as GridDataItem;
                gridItem["DTVFieldName"].ToolTip = row["DTVFieldName"].ToString();
            }
        }

        protected void DTBoundFieldsGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                GridDataItem gridItem = e.Item as GridDataItem;
                gridItem["DTVFieldName"].ToolTip = row["DTVFieldName"].ToString();
                gridItem["MPFieldName"].ToolTip = row["MPFiledName"].ToString();
            }
        }
        #endregion
    }
}