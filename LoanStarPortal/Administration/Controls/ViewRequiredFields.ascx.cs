using System;
using System.Data;
using System.Xml;
using System.Collections;
using Telerik.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class ViewRequiredFields : AppControl
    {
        #region constants
        private const string NAMEFIELDNAME = "name";
        private const string IDNAMEFIELDNAME = "id";
        private const string ROOTELEMENT = "Root";
        private const string ITEMELEMENT = "item";
        private const string IDATTRIBUTE = "id";
        private const string DATALIST = "DataList1";

//        private const string CURRENTTAB = "currenttab";
        #endregion

        #region fields
        private DataView dvFields;
        private int statusId = 1;
        private Hashtable statusDictionary;
        #endregion

        #region properties
        protected Hashtable StatusDictionary
        {
            get
            {
                if(statusDictionary==null)
                {
                    statusDictionary = GetStatusDictionary();
                }
                return statusDictionary;
            }
        }
        protected DataView DvFields
        {
            get
            {
                if(dvFields==null)
                {
                    dvFields = Field.GetReqiredFieldsList(CurrentUser.EffectiveCompanyId);
                }
                return dvFields;
            }
        }
        #endregion
        #region methods
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            lblMessage.Text = "";
            BindData();
        }
        private void BindData()
        {
            BindStatus();
            BindTabStrip();
            try
            {
                statusId = int.Parse(ddlStatus.SelectedValue);
            }
            catch
            {
            }
        }
        private static Hashtable GetStatusDictionary()
        {
            Hashtable res = new Hashtable();
            DataView dv = MortgageProfile.GetStatusList();
            for(int i=0;i<dv.Count;i++)
            {
                res.Add(int.Parse(dv[i][IDNAMEFIELDNAME].ToString()),dv[i][NAMEFIELDNAME].ToString());
            }
            return res;
        }

        private void BindStatus()
        {
            ddlStatus.DataSource = MortgageProfile.GetStatusList();
            ddlStatus.DataTextField = NAMEFIELDNAME;
            ddlStatus.DataValueField = IDNAMEFIELDNAME;
            ddlStatus.DataBind();
        }
        private void BindTabStrip()
        {
            RadTabStrip1.Tabs.Clear();
            RadMultiPage1.PageViews.Clear();
            RadTabStrip1.AppendDataBoundItems = false;
            DataView dv = Field.GetFieldGroup(false);
            RadTabStrip1.DataSource = dv;
            RadTabStrip1.DataTextField = NAMEFIELDNAME;
            RadTabStrip1.DataValueField = IDNAMEFIELDNAME;
            RadTabStrip1.DataBind();
            RadTabStrip1.SelectedIndex = 0;
            RadMultiPage1.SelectedIndex = RadTabStrip1.SelectedIndex;
        }
        protected void RadTabStrip1_TabDataBound(object sender, TabStripEventArgs e)
        {
            PageView pv = new PageView();
            DataRowView dr = (DataRowView)e.Tab.DataItem;
            RequiredFieldGroup ctl = LoadControl(Constants.CONTROLSLOCATION + Constants.CTLREQUIREDFIELDGROUP) as RequiredFieldGroup;
            if(ctl!=null)
            {
                int groupId = int.Parse(dr[IDNAMEFIELDNAME].ToString());
                ctl.ID = "g_" + groupId;
                ctl.BindData(DvFields,groupId,statusId,StatusDictionary);
                pv.Controls.Add(ctl);
            }
            RadMultiPage1.PageViews.Add(pv);
        }
        private string GetFieldsInfo()
        {
            string res = String.Empty;
            String[] col = Page.Request.Form.AllKeys;
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].IndexOf("$" + DATALIST + "$") > 0)
                {
                    int j = col[i].LastIndexOf('$');
                    if (j > 0)
                    {
                        XmlNode n = d.CreateElement(ITEMELEMENT);
                        XmlAttribute a = d.CreateAttribute(IDATTRIBUTE);
                        a.Value = col[i].Substring(j + 1);
                        n.Attributes.Append(a);
                        root.AppendChild(n);
                    }
                }
            }
            if (root.ChildNodes.Count > 0)
            {
                d.AppendChild(root);
                res = d.OuterXml;
            }
            return res;
        }

        #endregion

        #region event handlers
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            statusId = int.Parse(ddlStatus.SelectedValue);
            BindTabStrip();
        }
        #endregion
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string data = GetFieldsInfo();
            if(Field.SaveRequiredFieldList(data,CurrentUser.EffectiveCompanyId,int.Parse(ddlStatus.SelectedValue)))
            {
                lblMessage.Text = Constants.SUCCESSMESSAGE;
                statusId = int.Parse(ddlStatus.SelectedValue);
                dvFields = null;
                BindTabStrip();
            }
            else
            {
                lblMessage.Text = "Can't save data";
            }
        }
    }
}