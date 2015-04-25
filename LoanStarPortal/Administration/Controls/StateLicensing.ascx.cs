using System;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class StateLicensing : AppControl
    {
        private void BindData()
        {
            G.DataSource = OriginatorStateLicense.GetLicenseStateList();
            G.DataBind();
        }
        protected void GetData(object source, GridNeedDataSourceEventArgs e)
        {
            G.DataSource = OriginatorStateLicense.GetLicenseStateList();
        }

        protected void G_ItemDataBound(object sender, GridItemEventArgs e)
        {
            //if (e.Item is GridDataItem)
            //{
            //    GridDataItem dataItem = e.Item as GridDataItem;
            //    if (dataItem != null)
            //    {
            //        DataRowView dr = dataItem.DataItem as DataRowView;
            //        if (dr != null)
            //        {
            //            bool canEdit = bool.Parse(dr["canedit"].ToString());
            //            if (!canEdit)
            //            {
            //                dataItem["EditCommandColumn"].Controls.Clear();
            //                dataItem["DeleteColumn"].Controls.Clear();
            //            }
            //        }
            //    }
            //}
        }
        protected void CheckChanged(Object sender, System.EventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            GridDataItem item = (GridDataItem)box.NamingContainer;
            string[] tmp = item.KeyValues.Split(':');
            if(tmp.Length==2)
            {
                try
                {
                    string s = tmp[1].Replace("\"","").Replace("}","");
                    OriginatorStateLicense.SetLicenseStateRequired(int.Parse(s),box.Checked);
                }
                catch
                {
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }

        }
    }
}