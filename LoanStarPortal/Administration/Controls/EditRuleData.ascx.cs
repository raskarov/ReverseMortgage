using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;
using Rule=LoanStar.Common.Rule;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditRuleData : RuleControl
    {
        #region constants
        private const bool ALL = true;
        private const string STEPINDEX = "STEP";
        private const string CURRENTITEMID = "dataitemid";
        private const string ADDHEADERTEXT = "Add data for rule({0})";
        private const string EDITHEADERTEXT = "Edit data for rule({0})";
        private const int CONTROLTEXTBOX = 0;
        private const int CONTROLSELECT = 1;
        private const int CONTROLDATEPICKER = 2;
        private const int CONTROLMASKEDTEXTBOX = 3;
        private const string DICTIONARYLABELTEXT = "Select value";
        private const string GENERALLABELTEXT = "Enter value";
        private const string STRINGLABELTEXT = "Enter string";
        private const string DATELABELTEXT = "Enter date";
        private const string INTEGERLABELTEXT = "Enter integer number";
        private const string FLOATLABELTEXT = "Enter float number";
        private const string NOTSELECTED = " - Select - ";
        private const string YES = "Yes";
        private const string YESVALUE = "1";
        private const string NO = "No";
        private const string NOVALUE = "0";
        private const string FIELDNEEDED = "*";
        private const string CANTSAVEMESSAGE = "Can't save data";
        private const string DATATEXT = "item";
        #endregion

        #region fields
        private Field field;
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
        protected int CurrentItemId
        {
            get
            {
                Object o = ViewState[CURRENTITEMID];
                int res = 0;
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch
                    {
                        ViewState[CURRENTITEMID] = res;
                    }
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
            base.Page_Load(sender, e);
            field = GetField();
        }

        #region methods
        private Field GetField()
        {
            Field f = CurrentPage.GetObject(Constants.FIELDOBJECT) as Field;
            if (f != null)
            {
                return f;
            }
            f = new Field();
            CurrentPage.StoreObject(field, Constants.FIELDOBJECT);
            return f;
        } 
        public override void Initialize()
        {
            base.Initialize();
            field = GetField();
            Step = 0;
            InitControls();
            lblHeader.Text = String.Format(CurrentItemId > 0?EDITHEADERTEXT:ADDHEADERTEXT,rule.Name);
            tdruleexp.InnerHtml = RULECODETEXT + rule.GetColoredCodeById();
        }
        private void InitControls()
        {
            BindGroup();
            BindGrid(false);
            DoStep();
        }
        private void BindGroup()
        {
            ddlGroup.DataSource = Field.GetFieldGroup(ALL);
            ddlGroup.DataTextField = Field.GROUPNAMEFIELDNAME;
            ddlGroup.DataValueField = Field.IDFIELDNAME;
            ddlGroup.DataBind();
        }
        private void BindGrid(bool notifyParent)
        {
            G.DataSource = rule.GetRuleObjectList(Rule.DATAOBJECTTYPEID);
            G.DataBind();
            if (notifyParent)
            {
                refreshMainGrid();
            }
        }
        private void DoStep()
        {
            validatormsg.Text = "";
            btnAdd.Enabled = false;
            btnCancel.Enabled = false;
            ddlDictionary.Visible = false;
            ddlGroup.Enabled = false;
            ddlField.Enabled = false;
            tbValue.Enabled = false;
            ddlDictionary.Enabled = false;
            dpValue.Enabled = false;
            mtb.Enabled = false;
            SetValueFieldLabel();
            SetValueFieldControl();
            switch (Step)
            { 
                case 0:
                    ddlField.Items.Insert(0, new ListItem(NOTSELECTED, 0.ToString()));
                    ddlGroup.Enabled = true;
                    ddlGroup.Focus();
                    break;
                case 1:
                    ddlField.DataSource = Field.GetFieldInGroup(int.Parse(ddlGroup.SelectedValue));
                    ddlField.DataTextField = Field.DESCRIPTIONFIELDNAME;
                    ddlField.DataValueField = Field.IDFIELDNAME;
                    ddlField.DataBind();
                    ddlField.Items.Insert(0, new ListItem(NOTSELECTED, 0.ToString()));
                    ddlField.Enabled = true;
                    ddlField.Focus();
                    btnCancel.Enabled = true;
                    break;
                case 2:
                    if (field.IsDictionaryField)
                    {
                        ddlDictionary.Enabled = true;
                        ddlDictionary.DataSource = field.GetDictionaryList();
                        ddlDictionary.DataTextField = field.FieldName;
                        ddlDictionary.DataValueField = Field.IDFIELDNAME;
                        ddlDictionary.DataBind();
                        ddlDictionary.Focus();
                    }
                    else if (field.TypeId == (int)Field.MortgageProfileFieldType.Boolean)
                    {
                        ddlDictionary.Enabled = true;
                        ddlDictionary.Items.Clear();
                        ddlDictionary.Items.Add(new ListItem(NOTSELECTED, (-1).ToString()));
                        ddlDictionary.Items.Add(new ListItem(YES, YESVALUE));
                        ddlDictionary.Items.Add(new ListItem(NO, NOVALUE));
                        ddlDictionary.Focus();
                    }
                    else if (field.TypeId == (int)Field.MortgageProfileFieldType.DateTime)
                    {
                        dpValue.Enabled = true;
                        dpValue.Focus();
                    }
                    else if ((field.TypeId == (int)Field.MortgageProfileFieldType.Integer) || (field.TypeId == (int)Field.MortgageProfileFieldType.Float))
                    {
                        mtb.Enabled = true;
                        mtb.Mask = (field.TypeId == (int)Field.MortgageProfileFieldType.Integer) ? "##########" : "#########.##";
                        mtb.DisplayMask = mtb.Mask;
                        mtb.DisplayFormatPosition = DisplayFormatPosition.Right;
                        mtb.Focus();
                    }
                    else
                    {
                        tbValue.Enabled = true;
                        tbValue.Focus();
                    }
                    btnAdd.Enabled = true;
                    btnCancel.Enabled = true;
                    break;
            }
        }
        private void SetValueFieldLabel()
        {
            if (field.IsDictionaryField)
            {
                lblValue.Text = DICTIONARYLABELTEXT;
            }
            else
            {
                string txt = GENERALLABELTEXT;
                Field.MortgageProfileFieldType tp = (Field.MortgageProfileFieldType)field.TypeId;
                switch (tp)
                {
                    case Field.MortgageProfileFieldType.String:
                        txt = STRINGLABELTEXT;
                        break;
                    case Field.MortgageProfileFieldType.DateTime:
                        txt = DATELABELTEXT;
                        break;
                    case Field.MortgageProfileFieldType.Integer:
                        txt = INTEGERLABELTEXT;
                        break;
                    case Field.MortgageProfileFieldType.Float:
                        txt = FLOATLABELTEXT;
                        break;
                    case Field.MortgageProfileFieldType.Boolean:
                        txt = DICTIONARYLABELTEXT;
                        break;
                }
                lblValue.Text = txt;
            }
        }
        private void SetValueFieldControl()
        {
            Field.MortgageProfileFieldType tp = (Field.MortgageProfileFieldType)field.TypeId;
            int controlType;
            if (field.IsDictionaryField)
            {
                controlType = CONTROLSELECT;
            }
            else
            {
                switch (tp)
                {
                    case Field.MortgageProfileFieldType.Boolean:
                        controlType = CONTROLSELECT;
                        break;
                    case Field.MortgageProfileFieldType.DateTime:
                        controlType = CONTROLDATEPICKER;
                        break;
                    case Field.MortgageProfileFieldType.Integer:
                    case Field.MortgageProfileFieldType.Float:
                        controlType = CONTROLMASKEDTEXTBOX;
                        break;
                    default:
                        controlType = CONTROLTEXTBOX;
                        break;
                }
            }
            switch (controlType)
            {
                case CONTROLSELECT:
                    mtb.Visible = false;
                    dpValue.Visible = false;
                    tbValue.Visible = false;
                    ddlDictionary.Visible = true;
                    ddlDictionary.Enabled = false;
                    ddlDictionary.Items.Insert(0, new ListItem(NOTSELECTED, 0.ToString()));
                    break;
                case CONTROLDATEPICKER:
                    mtb.Visible = false;
                    dpValue.Visible = true;
                    dpValue.Enabled = false;
                    tbValue.Visible = false;
                    ddlDictionary.Visible = false;
                    ddlDictionary.Enabled = false;
                    ddlDictionary.Items.Insert(0, new ListItem(NOTSELECTED, 0.ToString()));
                    break;
                case CONTROLMASKEDTEXTBOX:
                    dpValue.Visible = false;
                    dpValue.Enabled = false;
                    tbValue.Visible = false;
                    mtb.Visible = true;
                    ddlDictionary.Visible = false;
                    ddlDictionary.Enabled = false;
                    ddlDictionary.Items.Insert(0, new ListItem(NOTSELECTED, 0.ToString()));
                    break;
                default:
                    dpValue.Visible = false;
                    tbValue.Visible = true;
                    dpValue.Enabled = false;
                    mtb.Visible = false;
                    ddlDictionary.Visible = false;
                    tbValue.Enabled = false;
                    tbValue.Focus();
                    break;
            }
        }
        private bool Validate()
        {
            bool res = true;
            validatormsg.Text = "";
            if (field.IsDictionaryField)
            {
                res = int.Parse(ddlDictionary.SelectedValue) != 0;
                if (!res)
                {
                    validatormsg.Text = FIELDNEEDED;
                }
                return res;
            }
            Field.MortgageProfileFieldType tp = (Field.MortgageProfileFieldType)field.TypeId;

            switch (tp)
            {
                case Field.MortgageProfileFieldType.String:
                    if (String.IsNullOrEmpty(tbValue.Text))
                    {
                        validatormsg.Text = FIELDNEEDED;
                        res = false;
                    }
                    break;
                case Field.MortgageProfileFieldType.DateTime:
                    res = !dpValue.IsEmpty;
                    if (!res)
                    {

                        validatormsg.Text = FIELDNEEDED;
                    }
                    break;
                case Field.MortgageProfileFieldType.Integer:
                case Field.MortgageProfileFieldType.Float:
                    res = !String.IsNullOrEmpty(mtb.Text);
                    if (!res)
                    {
                        validatormsg.Text = FIELDNEEDED;
                    }
                    break;
                case Field.MortgageProfileFieldType.Boolean:
                    res = !(ddlDictionary.SelectedValue == "-1");
                    if (!res)
                    {
                        validatormsg.Text = FIELDNEEDED;
                    }
                    break;
            }
            return res;
        }
        private string GetFieldValue()
        {
            if (tbValue.Visible)
            {
                return tbValue.Text;
            }
            else if (dpValue.Visible)
            {
                return Convert.ToDateTime(dpValue.SelectedDate).ToShortDateString();
            }
            else if (mtb.Visible)
            {
                return mtb.TextWithLiterals;
            }
            else if (ddlDictionary.Visible)
            {
                return ddlDictionary.SelectedValue;
            }
            return String.Empty;
        }
        #endregion

        #region event handlers
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Step--;
            DoStep();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                int res = rule.SaveData(CurrentItemId,field.ID, GetFieldValue());
                if (res > 0)
                {
                    BindGrid(G.Items.Count == 0);
                    lblMessage.Text = Constants.SUCCESSMESSAGE;
                    Initialize();
                }
                else
                {
                    lblMessage.Text = CANTSAVEMESSAGE;
                }
            }
            
        }
        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            Step = 1;
            DoStep();
        }
        protected void ddlField_SelectedIndexChanged(object sender, EventArgs e)
        {
            Step = 2;
            field = new Field(int.Parse(ddlField.SelectedValue));
            CurrentPage.StoreObject(field, Constants.FIELDOBJECT);
            DoStep();
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
                    ImageButton imgbutton = (ImageButton)e.Item.Cells[4].Controls[1];
                    imgbutton.Attributes.Add(ONCLICKATTRIBUTE, String.Format(Constants.JSDELETECONFIRM, DATATEXT));
                }
            }
        }
        #endregion

    }
}