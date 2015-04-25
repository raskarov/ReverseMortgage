using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;
using Rule=LoanStar.Common.Rule;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditRuleCondition : RuleControl
    {
        #region constants
        private const string CURRENTCONDITIONID = "conditionid";
        private const string ADDHEADERTEXT = "Add condition for rule({0})";
        private const string EDITHEADERTEXT = "Edit condition for rule({0})";
        private const bool TEMPLATE = true;
        private const bool ALL = true;
        private const bool ASCENDING = true;
        private const string NOTSELECTED = "0";
        private const string CONDITIONTEXT = "condition";
        private const string EDITCONDITIONCOMMAND = "editcondition";
        private const string CLICKHANDLER = "onclick";
        private const string JSVALIDATE = "if (!ValidateCondition({0},{1},{2},{3},{4},{5})) return false;";
        #endregion

        #region fields
        private RuleCondition rc;
        #endregion

        #region properties
        protected int CurrentConditionId
        {
            get
            {
                int res = -1;
                Object o = ViewState[CURRENTCONDITIONID];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch { }
                }
                else
                {
                    ViewState[CURRENTCONDITIONID] = res;
                }
                return res;
            }
            set
            {
                ViewState[CURRENTCONDITIONID] = value;
            }
        }
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            btnSave.Attributes.Add(CLICKHANDLER, String.Format(JSVALIDATE, tbTitle.ClientID, tbTitleerr.ClientID, tbDetail.ClientID, tbDetailerr.ClientID, ddlRole.ClientID,ddlRoleerr.ClientID)); 
        }

        #region methods        
        public override void Initialize()
        {
            base.Initialize();
            CurrentConditionId = -1;
            tdruleexp.InnerHtml = RULECODETEXT + rule.GetColoredCodeById();
            BindGrid(false);
            BindRoles();
            BindCondition();
        }
        private void BindCondition()
        {
            if (CurrentConditionId < 0)
            {
                tbDetail.Text = "";
                tbTitle.Text = "";
                ddlRole.SelectedValue = NOTSELECTED;
                lblHeader.Text = String.Format(ADDHEADERTEXT, rule.Name);
            }
            else
            {
                rc = rule.GetCondition(CurrentConditionId);
                tbDetail.Text = rc.Detail; 
                tbTitle.Text = rc.Title;
                ddlRole.SelectedValue = rc.RoleId.ToString();
                lblHeader.Text = String.Format(EDITHEADERTEXT, rule.Name);
            }
        }
        private void BindRoles()
        {
            ddlRole.DataSource = Role.GetList(TEMPLATE, ALL, ASCENDING, CurrentUser.EffectiveCompanyId);
            ddlRole.DataTextField = Role.NAMEFIELDNAME;
            ddlRole.DataValueField = Role.IDFIELDNAME;
            ddlRole.DataBind();
        }
        private void BindGrid(bool notifyParent)
        {
            lblMessage.Text = String.Empty;
            lblHeader.Text = String.Format((CurrentConditionId > 0 ? EDITHEADERTEXT : ADDHEADERTEXT), rule.Name);
            G.DataSource = rule.GetRuleObjectList(Rule.CONDITIONOBJECTTYPEID);
            G.DataBind();
            if (notifyParent)
            {
                refreshMainGrid();
            }
        }
        #endregion

        #region event handlers
        protected void btnClose_Click(object sender, EventArgs e)
        {
            goBack();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            rc = new RuleCondition();
            rc.Id = CurrentConditionId;
            rc.Detail = tbDetail.Text;
            rc.Title = tbTitle.Text;
            rc.RoleId = int.Parse(ddlRole.SelectedValue);
            if (rule.SaveCondition(rc)>0)
            {
                BindGrid(G.Items.Count == 0);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CurrentConditionId = -1;
            BindCondition();
        }
        #endregion

        #region grid related methods
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            int id = int.Parse(e.CommandArgument.ToString());
            switch (cmd)
            {
                case Constants.DELETECOMMAND:
                    rule.DeleteObject(id);
                    BindGrid(G.Items.Count==1);
                    break;
                case EDITCONDITIONCOMMAND:
                    CurrentConditionId = id;
                    BindCondition();
                    break;
                default:
                    return;
            }
        }
        protected void G_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row != null)
                {
                    ImageButton imgbutton = (ImageButton)e.Item.Cells[5].Controls[3];
                    imgbutton.Attributes.Add(ONCLICKATTRIBUTE, String.Format(Constants.JSDELETECONFIRM, CONDITIONTEXT));
                }
            }
        }
        #endregion

    }
}