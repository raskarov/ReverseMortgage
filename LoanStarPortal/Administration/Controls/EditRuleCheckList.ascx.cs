using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
using LoanStar.Common;
using Telerik.WebControls;
using Rule=LoanStar.Common.Rule;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditRuleCheckList : RuleControl
    {
        #region constants
        private const string ADDHEADERTEXT = "Add checklist for rule({0})";
        private const string EDITHEADERTEXT = "Edit checklist for rule({0})";
        private const string CHECKLISTNAME = "Checklist {0}";
        private const string CHECKLISTTEXT = "checklist";
        private const string CURRENTCHECKLISTID = "checklistid";
        private const string EDITCHECKLISTCOMMAND = "editchecklist";
        private const string ROOTELEMENT = "Root";
        private const string ITEMELEMENT = "item";
        private const string IDATTRIBUTE = "id";
        private const string TEXTATTRIBUTE = "text";
        private const string YESATTRIBUTE = "yes";
        private const string NOATTRIBUTE = "no";
        private const string DONOTKNOWATTRIBUTE = "donotknow";
        private const string TOFOLLOWATTRIBUTE = "tofollow";
        private const string STATUSID = "statusid";
        private const string EMPTYMESSAGE = "Can't save empty checklist";
        private const string ERRORMESSAGE = "Can't save checklist";
        private const string FIELDNEEDEDTEXT = "*";
        private const string SELECTEDFIELDNAME = "selected";
        private const string CLICKHANDLER = "onclick";
        private const string JSCLICKHANDLER = "CheckItems(this.checked,{0},{1},{2},{3},{4});";
        #endregion

        #region fields
        private int counter;
        bool err;
        #endregion

        #region properties
        protected int CurrentCheckListId
        {
            get
            {
                int res = -1;
                Object o = ViewState[CURRENTCHECKLISTID];
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
                    ViewState[CURRENTCHECKLISTID] = res;
                }
                return res;
            }
            set 
            {
                ViewState[CURRENTCHECKLISTID] = value;
            }
        }
        #endregion

        #region methods
        public override void Initialize()
        {
            base.Initialize();
            tdruleexp.InnerHtml = RULECODETEXT+ rule.GetColoredCodeById();
            CurrentCheckListId = -1;
            BindGrid(false);
        }
        private void BindGrid(bool notifyParent)
        {
            lblMessage.Text = String.Empty;
            counter = 0;
            G.DataSource = rule.GetRuleObjectList(Rule.CHECKLISTOBJECTTYPEID);
            G.DataBind();
            if (notifyParent)
            {
                refreshMainGrid();
            }
            BindRepeater();
        }
        private void BindRepeater()
        {
            lblHeader.Text = String.Format((CurrentCheckListId>0? EDITHEADERTEXT:ADDHEADERTEXT), rule.Name);
            rpChecklist.DataSource = Rule.GetCheckList(CurrentCheckListId);
            rpChecklist.DataBind();
        }
        protected Rule GetRule()
        {
            Rule r = CurrentPage.GetObject(Constants.RULEOBJECT) as Rule;
            if (r == null)
            {
                r = new Rule();
                CurrentPage.StoreObject(r, Constants.RULEOBJECT);
            }
            return r;
        }
        private string GetDataXml()
        {
            string res = String.Empty;
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            err = false;
            for (int i = 0; i < rpChecklist.Items.Count; i++)
            {
                CheckBox cb = (CheckBox)rpChecklist.Items[i].Controls[1];
                if (cb.Checked)
                {
                    ((Label)rpChecklist.Items[i].Controls[5]).Text = "";
                    ((Label)rpChecklist.Items[i].Controls[15]).Text = "";
                    string val = ((TextBox)rpChecklist.Items[i].Controls[3]).Text;
                    bool cb1 = ((CheckBox)rpChecklist.Items[i].Controls[7]).Checked;
                    bool cb2 = ((CheckBox)rpChecklist.Items[i].Controls[9]).Checked;
                    bool cb3 = ((CheckBox)rpChecklist.Items[i].Controls[11]).Checked;
                    bool cb4 = ((CheckBox)rpChecklist.Items[i].Controls[13]).Checked;
                    if (String.IsNullOrEmpty(val))
                    {
                        err = true;
                        ((Label)rpChecklist.Items[i].Controls[5]).Text = FIELDNEEDEDTEXT;
                    }
                    if (!(cb1 || cb2 || cb3 || cb4))
                    {
                        err = true;
                        ((Label)rpChecklist.Items[i].Controls[15]).Text = FIELDNEEDEDTEXT;
                    }
                    if (!err)
                    {
                        XmlNode n = d.CreateElement(ITEMELEMENT);
                        XmlAttribute a = d.CreateAttribute(IDATTRIBUTE);
                        a.Value = cb.Attributes[STATUSID];
                        n.Attributes.Append(a);
                        a = d.CreateAttribute(TEXTATTRIBUTE);
                        n.Attributes.Append(a);
                        a.Value = val;
                        AddCheckBoxValue(d, n, YESATTRIBUTE, cb1);
                        AddCheckBoxValue(d, n, NOATTRIBUTE, cb2);
                        AddCheckBoxValue(d, n, DONOTKNOWATTRIBUTE, cb3);
                        AddCheckBoxValue(d, n, TOFOLLOWATTRIBUTE, cb4);
                        root.AppendChild(n);
                    }
                }
            }
            if ((root.ChildNodes.Count > 0) && (!err))
            {
                d.AppendChild(root);
                res = d.OuterXml;
            }
            return res;
        }
        private void AddCheckBoxValue(XmlDocument d, XmlNode n, string attributename, bool ischecked)
        {
            XmlAttribute a = d.CreateAttribute(attributename);
            a.Value = ischecked ? "1" : "0";
            n.Attributes.Append(a);
        }
        #endregion     

        #region event handlers
        protected void rpChecklist_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row != null)
                {
                    CheckBox cb = (CheckBox)e.Item.Controls[1];
                    bool isChecked = bool.Parse(row[SELECTEDFIELDNAME].ToString());
                    TextBox tb = (TextBox)e.Item.Controls[3];
                    CheckBox cb1 = (CheckBox)e.Item.Controls[7];
                    CheckBox cb2 = (CheckBox)e.Item.Controls[9];
                    CheckBox cb3 = (CheckBox)e.Item.Controls[11];
                    CheckBox cb4 = (CheckBox)e.Item.Controls[13];
                    tb.Enabled = isChecked;
                    cb1.Enabled = isChecked;
                    cb2.Enabled = isChecked;
                    cb3.Enabled = isChecked;
                    cb4.Enabled = isChecked;
                    cb.Attributes.Add(CLICKHANDLER, String.Format(JSCLICKHANDLER, tb.ClientID, cb1.ClientID, cb2.ClientID, cb3.ClientID, cb4.ClientID));
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CurrentCheckListId = -1;
            BindRepeater();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string xml = GetDataXml();
            if (!err)
            {
                if (!String.IsNullOrEmpty(xml))
                {
                    if (rule.SaveRuleCheckList(CurrentCheckListId, xml)>0)
                    {
                        lblMessage.Text = Constants.SUCCESSMESSAGE;
                        CurrentCheckListId = -1;
                        BindGrid(G.Items.Count == 0);
                    }
                    else
                    {
                        lblMessage.Text = ERRORMESSAGE;
                    }
                }
                else
                {
                    lblMessage.Text = EMPTYMESSAGE;
                }
            }
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            goBack();
        }
        #endregion

        #region grid related
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
                case EDITCHECKLISTCOMMAND:
                    CurrentCheckListId = id;
                    BindRepeater();
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
                    ImageButton imgbutton = (ImageButton)e.Item.Cells[3].Controls[3];
                    imgbutton.Attributes.Add(ONCLICKATTRIBUTE, String.Format(Constants.JSDELETECONFIRM, CHECKLISTTEXT));
                }
            }
        }
        protected void G_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            G.CurrentPageIndex = e.NewPageIndex;
            CurrentCheckListId = -1;
            BindGrid(false);
        }
        protected string GetCheckListName(object item, string fieldname)
        {
            counter++;
            return String.Format(CHECKLISTNAME, counter);
        }
        #endregion
    }
}