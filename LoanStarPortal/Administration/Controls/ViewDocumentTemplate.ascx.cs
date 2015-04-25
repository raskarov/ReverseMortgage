using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class ViewDocumentTemplate : AppControl, IPostBackEventHandler
    {
        #region Private fields
        #endregion

        #region Protected methods
        protected static string GetArchivedValue(Object item, string name)
        {
            DataRowView row = (DataRowView)item;
            return Convert.ToBoolean(row[name]) ? "Yes" : "No";
        }
        #endregion

        #region Private methods
        private string GetSort()
        {
            if(RadGridDocTemplates.MasterTableView.SortExpressions.Count == 0)
                return String.Empty;
            
            GridSortExpression expression = RadGridDocTemplates.MasterTableView.SortExpressions[0];
            if(expression.SortOrder == GridSortOrder.None)
                return String.Empty;

            string sortOrder = expression.SortOrder == GridSortOrder.Ascending ? "asc" : "desc";
            string sortExpr = "order by " + expression.FieldName + " " + sortOrder;
            return sortExpr;
        }

        private string GetFilter()
        {
            StringBuilder sb = new StringBuilder();
            
            AppendTextBoxCondition(sb, tbTitle, DocTemplate.DBFieldMap.Title);
//          AppendTextBoxCondition(sb, tbFileName, DocTemplate.DBFieldMap.FileName);
            AppendDateTimePeriodCondition(sb, raddpFrom, raddpTo, DocTemplate.DBFieldMap.UploadDate);
            if (!cbArchived.Checked)
            {
                if (sb.Length > 0)
                    sb.Append(" and ");
                sb.Append("Archived = 0");
            }
            else
            {
                if (sb.Length > 0)
                    sb.Append(" and ");
                sb.Append("Archived IN (0,1)");
            }
            if(sb.Length > 0)
                sb.Insert(0, "where ");

            return sb.ToString();
        }

        private static void AppendTextBoxCondition(StringBuilder sb, ITextControl fieldValueCtrl, string fieldName)
        {
            string fieldValue = fieldValueCtrl.Text;
            if (fieldValue == null || fieldValue.Trim().Length == 0)
                return;

            if (sb.Length > 0)
                sb.Append(" and ");
            sb.Append(fieldName + " like '" + fieldValue + "%'");
        }

        private static void AppendDateTimePeriodCondition(StringBuilder sb, RadDatePicker dpFromCtrl, RadDatePicker dpToCtrl, string fieldName)
        {
            if (dpFromCtrl.IsEmpty && dpToCtrl.IsEmpty)
                return;
            DateTime dateFrom = dpFromCtrl.IsEmpty ? dpFromCtrl.MinDate : (DateTime)dpFromCtrl.SelectedDate;
            DateTime dateTo = dpToCtrl.IsEmpty ? dpToCtrl.MaxDate : (DateTime)dpToCtrl.SelectedDate;

            if (sb.Length > 0)
                sb.Append(" and ");
            sb.Append(fieldName + " >= '" + dateFrom.ToShortDateString() + "' and " + fieldName + " <= '" + dateTo.ToShortDateString() + "'");
        }

        private static void AppendRadioBtnCondition(StringBuilder sb, CheckBox cbCtrl, string fieldName)
        {
            if (!cbCtrl.Checked)
                return;

            if (sb.Length > 0)
                sb.Append(" and ");
            sb.Append(fieldName + " = 1");

        }
        #endregion

        #region Properties
        #endregion

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!CurrentUser.IsLoanStarAdmin)
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));

            AddDocTemplate.NavigateUrl = ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.EDITDOCTEMPLATE);

            if(!Page.IsPostBack)
                RadGridDocTemplates.Rebind();
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            RadGridDocTemplates.Rebind();
        }

        protected void RadGridDocTemplates_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            string whereClause = GetFilter();
            string orderBy = GetSort();
            RadGridDocTemplates.DataSource = DocTemplate.GetDocTemplateList(orderBy, whereClause);
        }

        protected void RadGridDocTemplates_SortCommand(object source, GridSortCommandEventArgs e)
        {
            e.Canceled = true;

            GridSortExpression expression = new GridSortExpression();
            expression.FieldName = e.SortExpression;
            expression.SortOrder = e.NewSortOrder;

            e.Item.OwnerTableView.SortExpressions.Clear();
            e.Item.OwnerTableView.SortExpressions.AddSortExpression(expression);

            RadGridDocTemplates.Rebind();
        }
        #endregion

        protected void RadGridDocTemplates_ItemCommand(object source, GridCommandEventArgs e)
        {
            switch(e.CommandName)
            {
                case "Edit":
                    {
                        e.Canceled = true;
                        
                        string url = ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.EDITDOCTEMPLATE + "&" + Constants.IDPARAM + "=" + e.CommandArgument);

                        Response.Redirect(url);
                    }
                    break;
                case "Archive":
                    {
                        e.Canceled = true;

                        int docTemplateID = Convert.ToInt32(e.CommandArgument);
                        DocTemplate docTemplate = new DocTemplate(docTemplateID);
                        if (docTemplate.Archived)
                            docTemplate.UnArchive();
                        else
                            docTemplate.Archive();

                        RadGridDocTemplates.Rebind();
                    }
                    break;
                default:
                    return;
            }
        }
    }
}