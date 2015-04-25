using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class FieldGroup : AppControl
    {

        private const string CHECKFIELDJS = "CheckField(this,'{0}','{1}');";
        private const string CHECKALLJS = "CheckAll(this,'{0}');";
        private const string ONCLICK = "onclick";

        private int groupId = -1;
        private int statusid;
        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }
        public int StatusId
        {
            set { statusid = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            cbAll.Attributes.Add(ONCLICK, String.Format(CHECKALLJS, groupdiv.ClientID));
        }
        public void BindData()
        {
            Role r = CurrentPage.GetObject(Constants.ROLEOBJECT) as Role;
            if (r != null) DataList1.DataSource = r.GetAllowedFieldList(groupId,statusid);
            DataList1.DataBind();
        }

        protected void ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            if (dr != null)
            {
                int id = int.Parse(dr[Field.IDFIELDNAME].ToString());
                CheckBox cb = (CheckBox)e.Item.Controls[1];
                if (cb != null)
                {
                    cb.Attributes.Add(ONCLICK, String.Format(CHECKFIELDJS, cbAll.ClientID, groupdiv.ClientID));
                    cb.ID = id.ToString();
                    cb.Checked = int.Parse(dr["Selected"].ToString()) == 1;
                }
            }         
        }
    }
}