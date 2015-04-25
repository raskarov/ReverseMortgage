using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{

    public partial class ViewHolidays : AppControl
    {

        #region constants
        private const int NUMBEROFYEARS = 5;
        private const string DATETIMEFORMAT = "dddd, dd MMMM";
        private int currentYear = DateTime.Now.Year;
        #endregion
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Bind();
            }
            else
            {
                currentYear = int.Parse(ddlYears.SelectedValue) + DateTime.Now.Year;
            }
        }
        private void Bind()
        {
            ddlYears.Items.Clear();
            for (int i = 0; i < NUMBEROFYEARS; i++)
            {
                ddlYears.Items.Add(new ListItem((currentYear+i).ToString(),i.ToString()));
            }
            BindGrid(currentYear);
        }
        private void BindGrid(int _currentYear)
        {
            G.DataSource = Holidays.GetHolidaysByYearAndCompany(_currentYear, CurrentUser.EffectiveCompanyId);
            G.DataBind();
        }
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            if ((e.CommandName == RadGrid.PerformInsertCommandName) || (e.CommandName == RadGrid.UpdateCommandName))
            {
                Holidays holiday = new Holidays();
                holiday.ID = -1;
                if (e.CommandName == RadGrid.UpdateCommandName)
                {
                     holiday.ID=Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString());
                }
                holiday.CompanyId = CurrentUser.EffectiveCompanyId;
                TextBox tb = e.Item.FindControl("txtDescription") as TextBox;
                if (tb != null)
                {
                    holiday.Name = tb.Text;
                }
                RadDatePicker rd = e.Item.FindControl("radDatePicker") as RadDatePicker;
                if (rd != null)
                {
                    holiday.Day = (DateTime)rd.SelectedDate;
                }
                holiday.Save();
            }
            else if (e.CommandName == RadGrid.DeleteCommandName)
            { 
                Holidays holiday = new Holidays(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString()));
                holiday.Delete();
            }
            BindGrid(int.Parse(ddlYears.SelectedValue) + DateTime.Now.Year);
        }
        protected static string GetDate(Object item, string fieldName)
        {
            DataRowView dr = (DataRowView)item;
            DateTime dt = DateTime.Parse(dr[fieldName].ToString());
            return dt.ToString(DATETIMEFORMAT);
        }

        protected void G_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                if (editedItem != null)
                {
                    RadDatePicker dp = editedItem.FindControl("radDatePicker") as RadDatePicker;
                    if (dp != null)
                    {
                        DataRowView dr = editedItem.DataItem as DataRowView;
                        DateTime dt = DateTime.Now;
                        if (dr != null)
                        {
                            dt = DateTime.Parse(dr["HolidayDate"].ToString());
                        }
                        dp.SelectedDate = dt;
                        DateTime dt1 = new DateTime(currentYear, 1, 1);
                        DateTime dt2 = new DateTime(currentYear, 12, 31);
                        dp.MinDate = dt1;
                        dp.MaxDate = dt2;
                    }
                }
            }  
        }
        protected void G_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                if (dataItem != null)
                {
                    DataRowView dr = dataItem.DataItem as DataRowView;
                    if (dr != null)
                    {
                        bool canEdit = bool.Parse(dr["canedit"].ToString());
                        if (!canEdit)
                        {
                            dataItem["EditCommandColumn"].Controls.Clear();
                            dataItem["DeleteColumn"].Controls.Clear();
                        }
                    }
                }
            }
        }
        protected void ddlYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentYear = int.Parse(ddlYears.SelectedValue) + DateTime.Now.Year;
            BindGrid(currentYear);
        }
    }
}