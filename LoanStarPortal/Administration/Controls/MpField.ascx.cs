using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class MpField : AppControl
    {
        #region constants
        private const string GROUPFIELDCTL = "FieldGroup.ascx";
        private const string STATUSNAMEFIELDNAME = "name";
        private const string IDNAMEFIELDNAME = "id";
        private const string ALLSTATUSTEXT = "All statuses";
        private const int ALLSTATUSVALUE = 0;
        private const string CURRENTTAB = "currenttab";
        private const string CURRENTSTATUS = "currentstatus";
        #endregion

        private int groupId = -1;

        public int StatusId
        {
            get 
            {
                int res = 1;
                Object o = ViewState[CURRENTSTATUS];
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
            set
            {
                ViewState[CURRENTSTATUS] = value;
            }
        }
        protected int CurrentTab
        {
            get
            { 
                int res = 0;
                Object o = ViewState[CURRENTTAB];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch{}
                }
                return res;
            }
            set
            {
                ViewState[CURRENTTAB] = value;
            }
        }

        public void RebindData()
        {
            BindStatus();
            BindTabStrip();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        private void BindStatus()
        {
            ddlStatus.DataSource = MortgageProfile.GetStatusList();
            ddlStatus.DataTextField = STATUSNAMEFIELDNAME;
            ddlStatus.DataValueField = IDNAMEFIELDNAME;
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0,new ListItem(ALLSTATUSTEXT,ALLSTATUSVALUE.ToString()));
            ddlStatus.SelectedValue = StatusId.ToString();
        }
        private void BindTabStrip()
        {
            RadTabStrip1.AppendDataBoundItems = true;
            DataView dv = Field.GetFieldGroup(false);
            RadTabStrip1.DataSource = dv;
            RadTabStrip1.DataTextField = Field.NAMEFIELDNAME;
            RadTabStrip1.DataValueField = Field.IDFIELDNAME;
            RadTabStrip1.DataBind();
            RadTabStrip1.SelectedIndex = CurrentTab;
            RadMultiPage1.SelectedIndex = CurrentTab;
        }
        protected void RadTabStrip1_TabDataBound(object sender, TabStripEventArgs e)
        {
            PageView pv = new PageView();
            DataRowView dr = (DataRowView)e.Tab.DataItem;
            groupId = int.Parse(dr[Field.IDFIELDNAME].ToString());
            pv.ID = groupId.ToString();
            FieldGroup ctl = LoadControl(Constants.CONTROLSLOCATION + GROUPFIELDCTL) as FieldGroup;
            if (ctl != null)
            {
                ctl.ID = "g_" + groupId;
                ctl.StatusId = StatusId;
                ctl.GroupId = groupId;
                ctl.BindData();
                pv.Controls.Add(ctl);
            }
            RadMultiPage1.PageViews.Add(pv);
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            StatusId = int.Parse(ddlStatus.SelectedValue);
            BindTabStrip();
        } 
    }
}