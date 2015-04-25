using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class MpStatus : AppControl
    {
        #region constants
        private const string ONCLICKATTRIBUTE = "OnClick";
        private const string RECORDTEXT = "record";
        private const string ONCLICKHANDLER = "onclick";
        private const string ONCHANGEHANDLER = "onchange";
        private const string CHECKSELECTJS1 = "if (!checkSelect1()) return false;";
        private const string CHECKSELECTJS2 = "if (!checkSelect2()) return false;";
        #endregion

        #region fields
        private Role role;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            role = CurrentPage.GetObject(Constants.ROLEOBJECT) as Role;
            ddlMPStatus1.Attributes.Add(ONCHANGEHANDLER,CHECKSELECTJS1);
            btnSave.Attributes.Add(ONCLICKHANDLER,CHECKSELECTJS2); 
        }
        #region methods
        public void RebindData()
        {
            role = CurrentPage.GetObject(Constants.ROLEOBJECT) as Role;
            BindData();
        }
        private void BindData()
        {            
            if (role != null)
            {
                G.DataSource = role.GetRoleStatusList();
                G.DataBind();
                ddlMPStatus1.DataSource = role.GetAllowedMpStatusList(-1);
                ddlMPStatus1.DataTextField = Role.NAMEFIELDNAME;
                ddlMPStatus1.DataValueField = Role.IDFIELDNAME;
                ddlMPStatus1.DataBind();
                ddlMPStatus2.Enabled = false;
                ddlMPStatus1.Enabled = true;
            }
        }
        protected void ddlMPStatus1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMPStatus1.Enabled = false;
            ddlMPStatus2.DataSource = role.GetAllowedMpStatusList(int.Parse(ddlMPStatus1.SelectedValue.ToString()));
            ddlMPStatus2.DataTextField = Role.NAMEFIELDNAME;
            ddlMPStatus2.DataValueField = Role.IDFIELDNAME;
            ddlMPStatus2.DataBind();
            ddlMPStatus2.Enabled = true;
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            role.AddMPStatus(int.Parse(ddlMPStatus1.SelectedValue.ToString()), int.Parse(ddlMPStatus2.SelectedValue.ToString()));
            BindData();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ddlMPStatus2.Enabled = false;
            ddlMPStatus2.SelectedValue = "0";
            ddlMPStatus1.Enabled = true;
            ddlMPStatus1.SelectedValue = "0";
            ddlMPStatus1.Focus();
        }
        #endregion

        #region grid related methods
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            int id = -1;
            if (cmd != Constants.SORTCOMMAND)
            {
                id = int.Parse(e.CommandArgument.ToString());
            }
            switch (cmd)
            {
                case Constants.DELETECOMMAND:
                    role.DeleteStatus(id);
                    break;
                default:
                    return;
            }
            BindData();
        }
        protected void G_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            G.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }
        protected void G_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row != null)
                {
                    ImageButton imgbutton = (ImageButton)e.Item.Cells[4].Controls[1];
                    imgbutton.Attributes.Add(ONCLICKATTRIBUTE, String.Format(Constants.JSDELETECONFIRM, RECORDTEXT));
                }
            }
        }
        #endregion

    }
}