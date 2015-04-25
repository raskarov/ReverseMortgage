using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;
using Rule=LoanStar.Common.Rule;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditRuleAlert : RuleControl
    {
        #region
        private const string CURRENTALERTID = "alertid";
        private const string CURRENTITEMTYPE = "isevent";
        private const string CURRENTITEMMESSAGE = "itemmessage";
        private const string CURRENTEVENTTYPETID = "eventtypeid";
        private const string EVENTTYPETEXT = "event";
        private const string NOTSELECTED = "- Select -";
        private const string ADDHEADERTEXT = "Add alert(event) for rule({0})";
        private const string EDITHEADERTEXT = "Edit alert(event) for rule({0})";
        private const string ALERTTEXT = "item";
        private const string CLICKHANDLER = "onclick";
        private const string JSVALIDATE = "if (!ValidateAlert({0},{1},{2},{3},{4})) return false;";
        private const string JSSETSELECT = "SetSelect({0},{1});";
        private const string CANTSAVEMESSAGE = "Can't save data";
        private const string EDITALERTCOMMAND = "editalert";        
        #endregion

        #region properties
        protected int CurrentAlertId
        {
            get
            {
                int res = -1;
                Object o = ViewState[CURRENTALERTID];
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
                    ViewState[CURRENTALERTID] = res;
                }
                return res;
            }
            set
            {
                ViewState[CURRENTALERTID] = value;
            }
        }
        protected bool IsCurrentItemEvent
        {
            get
            {
                bool res = true;
                Object o = ViewState[CURRENTITEMTYPE];
                if (o != null)
                {
                    try
                    {
                        res = bool.Parse(o.ToString());
                    }
                    catch { }
                }
                else
                {
                    ViewState[CURRENTITEMTYPE] = res;
                }
                return res;
            }
            set
            {
                ViewState[CURRENTITEMTYPE] = value;
            }
        }
        protected string CurrentMessage
        {
            get
            {
                string res = String.Empty;
                Object o = ViewState[CURRENTITEMMESSAGE];
                if (o != null)
                {
                    res = o.ToString();
                }
                return res;
            }
            set 
            {
                ViewState[CURRENTITEMMESSAGE] = value;
            }
        }
        protected int CurrentEventTypeId
        {
            get
            {
                int res = -1;
                Object o = ViewState[CURRENTEVENTTYPETID];
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
                    ViewState[CURRENTEVENTTYPETID] = res;
                }
                return res;
            }
            set
            {
                ViewState[CURRENTEVENTTYPETID] = value;
            }
        }
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            btnSave.Attributes.Add(CLICKHANDLER, String.Format(JSVALIDATE, tbMessage.ClientID,tbMessageerr.ClientID, rbEvent.ClientID, ddlEventType.ClientID, ddltypeerr.ClientID));
            rbAlert.Attributes.Add(CLICKHANDLER, String.Format(JSSETSELECT, "!this.checked", ddlEventType.ClientID));
            rbEvent.Attributes.Add(CLICKHANDLER, String.Format(JSSETSELECT, "this.checked", ddlEventType.ClientID)); 
        }

        #region methods
        public override void Initialize()
        {
            base.Initialize();
            InitNewItem();
            tdruleexp.InnerHtml = RULECODETEXT + rule.GetColoredCodeById();
            BindGrid(false);
            BindEventType();
            BindAlert();
        }
        private void InitNewItem()
        {
            CurrentAlertId = -1;
            IsCurrentItemEvent = true;
            CurrentMessage = String.Empty;
        }
        private void BindEventType()
        {
            ddlEventType.DataSource = Event.GetTypeList();
            ddlEventType.DataTextField = Event.NAMEFIELDNAME;
            ddlEventType.DataValueField = Event.IDFIELDNAME;
            ddlEventType.DataBind();
            ddlEventType.Items.Insert(0,new ListItem(NOTSELECTED,0.ToString()));
        }
        private void BindGrid(bool notifyParent)
        { 
            G.DataSource = rule.GetRuleObjectList(Rule.ALERTEVENTOBJECTTYPEID);
            G.DataBind();
            if (notifyParent)
            {
                refreshMainGrid();
            }
        }
        private void BindAlert()
        {
            lblMessage.Text = String.Empty;
            lblHeader.Text = String.Format((CurrentAlertId > 0 ? EDITHEADERTEXT : ADDHEADERTEXT), rule.Name);
            rbEvent.Checked = IsCurrentItemEvent;
            rbAlert.Checked = !rbEvent.Checked;
            ddlEventType.Enabled = rbEvent.Checked;
            tbMessage.Text = CurrentMessage;
            if ((CurrentAlertId > 0) && (IsCurrentItemEvent))
            {
                ddlEventType.SelectedValue=CurrentEventTypeId.ToString();
            }
            tbMessage.Focus();
        }
        #endregion

        #region eventhandlers
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int res;
            bool isNew = CurrentAlertId<0;
            if (rbAlert.Checked)
            {
                isNew = isNew || IsCurrentItemEvent;
                res = rule.SaveAlert(CurrentAlertId,tbMessage.Text,isNew);
                IsCurrentItemEvent = false;
            }
            else
            {
                isNew = isNew || (!IsCurrentItemEvent);
                res = rule.SaveEvent(CurrentAlertId,tbMessage.Text,int.Parse(ddlEventType.SelectedValue),isNew);
                IsCurrentItemEvent = true;
            }
            if (res>0)
            {
                lblMessage.Text = Constants.SUCCESSMESSAGE;
                BindGrid(G.Items.Count == 0);
                InitNewItem();
                BindAlert();
            }
            else
            {
                lblMessage.Text = CANTSAVEMESSAGE;
            }            
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            InitNewItem();
            BindAlert();
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            goBack();
        }
        #endregion

        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            int id = int.Parse(e.CommandArgument.ToString());
            switch (cmd)
            {
                case Constants.DELETECOMMAND:
                    rule.DeleteObject(id);
                    BindGrid(G.Items.Count == 1);
                    break;
                case EDITALERTCOMMAND:
                    IsCurrentItemEvent = ((Label)e.Item.Cells[3].Controls[1]).Text.ToLower() == EVENTTYPETEXT;
                    CurrentEventTypeId = int.Parse(((Label)e.Item.Cells[3].Controls[1]).Attributes[CURRENTEVENTTYPETID]);
                    CurrentAlertId = id;
                    CurrentMessage = ((Label)e.Item.Cells[2].Controls[1]).Text;
                    BindAlert();
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
                    ImageButton imgbutton = (ImageButton)e.Item.Cells[4].Controls[3];
                    imgbutton.Attributes.Add(ONCLICKATTRIBUTE, String.Format(Constants.JSDELETECONFIRM, ALERTTEXT));
                }
            }
        }
    }
}