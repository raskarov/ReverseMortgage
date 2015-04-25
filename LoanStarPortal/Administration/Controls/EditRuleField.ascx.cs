using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;
using Rule=LoanStar.Common.Rule;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditRuleField : RuleControl
    {
        #region constants
        private const bool ALL = true;
        private const string NOTSELECTED = " - Select - ";
        private const string RECORDTEXT = "field";
        private const string STEPINDEX = "STEP";
        private const string HEADERTEXT = "Manage fields in role ({0})";
        #endregion

        #region properties
        protected int Step
        {
            get
            {
                Object o = ViewState[STEPINDEX];
                int res = 0;
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch
                    {
                        ViewState[STEPINDEX] = res;
                    }
                }
                return res;
            }
            set
            {
                ViewState[STEPINDEX] = value;
            }
        }
        #endregion

        #region methods
        public override void Initialize()
        {
            base.Initialize();
            Step = 0;
            lblHeader.Text = String.Format(HEADERTEXT, rule.Name);
            tdruleexp.InnerHtml = RULECODETEXT + rule.GetColoredCodeById();
            BindData();
            DoStep();
        }
        private void BindData()
        {
            BindGrid(false);
            ddlGroup.DataSource = Field.GetFieldGroup(ALL);
            ddlGroup.DataTextField = Field.GROUPNAMEFIELDNAME;
            ddlGroup.DataValueField = Field.IDFIELDNAME;
            ddlGroup.DataBind();
            ddlField.Enabled = false;
        }
        private void BindGrid(bool notifyParent)
        {
            G.DataSource = rule.GetRuleObjectList(Rule.FIELDOBJECTTYPEID);
            G.DataBind();
            if (notifyParent)
            {
                refreshMainGrid();
            }
        }
        private void DoStep()
        {
            ddlGroup.Enabled = false;
            ddlField.Enabled = false;
            btnCancel.Enabled = false;
            btnAdd.Enabled = false;
            switch (Step)
            { 
                case 0:
                    ddlField.Items.Clear();
                    ddlField.Items.Insert(0, new ListItem(NOTSELECTED, 0.ToString()));
                    ddlGroup.Enabled = true;
                    ddlGroup.Focus();
                    break;
                case 1:
                    ddlField.DataSource = rule.GetAllowedObjectList(Rule.FIELDOBJECTTYPEID, int.Parse(ddlGroup.SelectedValue));
                    ddlField.DataTextField = Field.DESCRIPTIONFIELDNAME;
                    ddlField.DataValueField = Field.IDFIELDNAME;
                    ddlField.DataBind();
                    ddlField.Items.Insert(0, new ListItem(NOTSELECTED, 0.ToString()));
                    ddlField.Enabled = true;
                    ddlField.Focus();
                    ddlGroup.Enabled = false;
                    btnCancel.Enabled = true;
                    btnAdd.Enabled = false;
                    break;
                case 2:
                    btnAdd.Enabled = true;
                    break;
            }
        }
        #endregion

        #region event handlers

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            Step = 1;
            DoStep();
        }
        protected void ddlField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(ddlField.SelectedValue) > 0)
            {
                Step = 2;
                DoStep();
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (rule.SaveRuleObject(int.Parse(ddlField.SelectedValue), Rule.FIELDOBJECTTYPEID, 1)>0)
            {
                BindGrid(G.Items.Count==0);
                Step--;
                DoStep();
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Step--;
            DoStep();
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            goBack();
        }
        #endregion

        #region grid related methods
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
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            if (cmd == Constants.SORTCOMMAND)
            {
                return;
            }
            int id = int.Parse(e.CommandArgument.ToString()); 
            switch (cmd)
            {
                case Constants.DELETECOMMAND:
                    rule.DeleteObject(id);
                    break;
                default:
                    return;
            }
            BindGrid(G.Items.Count == 1);
        }
        protected void G_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            G.CurrentPageIndex = e.NewPageIndex;
            BindGrid(false);
        }
        #endregion 


    }
}