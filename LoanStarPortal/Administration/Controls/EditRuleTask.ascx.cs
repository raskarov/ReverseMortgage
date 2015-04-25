using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;
using Rule=LoanStar.Common.Rule;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditRuleTask : RuleControl
    {

        #region constants
        private const string CURRENTITEMID = "currenttaskid";
        private const string NOTSELECTED = "- Select -";
        private const string NOTSELECTEDVALUE = "0";
        private const string JSVALIDATE = "if (!ValidateTask({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})) return false;";
        private const string CLICKHANDLER = "onclick";
        private const string CANTSAVEDATAMESSAGE = "Can't save data";
        private const string ADDHEADERTEXT = "Add task for rule({0})";
        private const string EDITHEADERTEXT = "Edit task for rule({0})";
        private const string EDITDATACOMMAND = "edittask";
        private const string DATATEXT = "task";
        #endregion
        
        #region properties
        protected int CurrentItemId
        {
            get
            {
                int res = -1;
                Object o = ViewState[CURRENTITEMID];
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
                    ViewState[CURRENTITEMID] = res;
                }
                return res;
            }
            set
            {
                ViewState[CURRENTITEMID] = value;
            }
        }
        #endregion
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender,e);
            btnSave.Attributes.Add(CLICKHANDLER, String.Format(JSVALIDATE, tbTitle.ClientID, tbTitleErr.ClientID
                                , tbDescription.ClientID, tbDescriptionErr.ClientID, ddlTaskType.ClientID, ddlTaskTypeErr.ClientID
                                , ddlDifficulty.ClientID, ddlDifficultyErr.ClientID, ddlInfoSource.ClientID,ddlInfoSourceErr.ClientID));
        }
        #region methods
        public override void Initialize()
        {
            base.Initialize();
            CurrentItemId = -1;
            tdruleexp.InnerHtml = RULECODETEXT + rule.GetColoredCodeById();
            BindDictionary();
            BindGrid(false);
            BindTask();
        }
        private void BindDictionary()
        {
            ddlDifficulty.DataSource = Task.GetDifficultyList();
            ddlDifficulty.DataTextField = "Name";
            ddlDifficulty.DataValueField = "Id";
            ddlDifficulty.DataBind();
            ddlDifficulty.Items.Insert(0, new ListItem(NOTSELECTED, "0"));
            ddlInfoSource.DataSource = Task.GetInfoSourceList();
            ddlInfoSource.DataTextField = "Name";
            ddlInfoSource.DataValueField = "Id";
            ddlInfoSource.DataBind();
            ddlInfoSource.Items.Insert(0, new ListItem(NOTSELECTED, "0"));
            ddlTaskType.DataSource = Task.GetTaskTypeList();
            ddlTaskType.DataTextField = "FullName";
            ddlTaskType.DataValueField = "Id";
            ddlTaskType.DataBind();
            ddlTaskType.Items.Insert(0, new ListItem(NOTSELECTED, "0"));
        }
        private void BindTask()
        {
            RuleTask rt = new RuleTask(CurrentItemId);
            tbDescription.Text = rt.Description;
            tbTitle.Text = rt.Title;
            if (rt.Id > 0)
            {
                lblHeader.Text = String.Format(EDITHEADERTEXT, rule.Name);
                ddlDifficulty.SelectedValue = rt.DifficultyId.ToString();
                ddlInfoSource.SelectedValue = rt.InfoSourceId.ToString();
                ddlTaskType.SelectedValue = rt.TypeId.ToString();
            }
            else
            {
                lblHeader.Text = String.Format(ADDHEADERTEXT, rule.Name);
                ddlDifficulty.SelectedValue = NOTSELECTEDVALUE;
                ddlInfoSource.SelectedValue = NOTSELECTEDVALUE;
                ddlTaskType.SelectedValue = NOTSELECTEDVALUE;

            }
        }
        private void BindGrid(bool notifyParent)
        {
            G.DataSource = rule.GetRuleObjectList(Rule.TASKOBJECTTYPEID);
            G.DataBind();
            if (notifyParent)
            {
                refreshMainGrid();
            }
        }
        #endregion

        #region event handlers
        protected void btnSave_Click(object sender, EventArgs e)
        {
            RuleTask rt = new RuleTask();
            rt.Id = CurrentItemId;
            rt.Title = tbTitle.Text;
            rt.Description = tbDescription.Text;
            rt.DifficultyId = int.Parse(ddlDifficulty.SelectedValue);
            rt.TypeId = int.Parse(ddlTaskType.SelectedValue);
            rt.InfoSourceId = int.Parse(ddlInfoSource.SelectedValue);
            int res = rule.SaveTask(rt);
            if (res > 0)
            {
                lblMessage.Text = Constants.SUCCESSMESSAGE;
                BindGrid(G.Items.Count == 0);
                CurrentItemId = -1;
                BindTask();
            }
            else
            {
                lblMessage.Text = CANTSAVEDATAMESSAGE;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CurrentItemId = -1;
            BindTask();
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            goBack();
        }
        #endregion

        #region grid related
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            switch (e.CommandName.ToLower())
            {
                case Constants.DELETECOMMAND:
                    rule.DeleteObject(id);
                    BindGrid(G.Items.Count == 1);
                    break;
                case EDITDATACOMMAND:
                    CurrentItemId = id;
                    BindTask();
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
                    ImageButton imgbutton = (ImageButton)e.Item.Cells[7].Controls[3];
                    imgbutton.Attributes.Add(ONCLICKATTRIBUTE, String.Format(Constants.JSDELETECONFIRM, DATATEXT));
                }
            }
        }
        #endregion
    }
}