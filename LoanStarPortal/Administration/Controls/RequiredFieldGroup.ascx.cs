using System;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class RequiredFieldGroup : AppControl
    {
        private const string FIELDGROUPFILTER = "fieldgroupid={0}";
        private const string IDFIELDNAME = "id";
        private const string DESCRIPTIONFIELDNAME = "description";
        private const string STATUSIDFIELDNAME = "statusid";
        private const string ONCLICK = "onclick";
        private const string CHECKFIELDJS = "CheckField(this,'{0}','{1}');";
        private const string CHECKALLJS = "CheckAll(this,'{0}');";

        private int currentStatusid = -1;
        private Hashtable status;


        protected void Page_Load(object sender, EventArgs e)
        {
            cbAll.Attributes.Add(ONCLICK, String.Format(CHECKALLJS,groupdiv.ClientID));
        }
        public void BindData(DataView dv, int groupId,int statusId, Hashtable _status)
        {
            status = _status;
            currentStatusid = statusId;
            dv.RowFilter = String.Format(FIELDGROUPFILTER, groupId);
            DataList1.DataSource = dv;
            DataList1.DataBind();
        }
        protected void ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            if (dr != null)
            {
                int id = int.Parse(dr[IDFIELDNAME].ToString());
                int reqStatusId = int.Parse(dr[STATUSIDFIELDNAME].ToString());
                string desc = dr[DESCRIPTIONFIELDNAME].ToString();
                CheckBox cb = (CheckBox)e.Item.Controls[1];
                if (cb != null)
                {
                    int i = desc.IndexOf(".");
                    if(i>0)
                    {
                        desc = desc.Substring(i+1);
                    }
                    cb.Attributes.Add(ONCLICK, String.Format(CHECKFIELDJS, cbAll.ClientID, groupdiv.ClientID));
                    cb.ID = id.ToString();
                    cb.Checked = reqStatusId == currentStatusid;
                    if(!cb.Checked&&(reqStatusId>0))
                    {
                        if(status.ContainsKey(reqStatusId))
                        {
                            desc += "(" + status[reqStatusId] + ")";
                        }
                    }
                    cb.Text = desc;
                }
            }
        }

    }
}