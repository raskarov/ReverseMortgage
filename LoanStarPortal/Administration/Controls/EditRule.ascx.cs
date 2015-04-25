using System;
using System.Data;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditRule : AppControl
    {
        #region constants
        private const string STEPINDEX = "STEP";
        private const string FIRSTRULEUNIT = "FIRSTRULEUNIT";
        private const string ISINITIATED = "EDITRULEISINITIATED";
        private const string WRONGDATE = "Wrong date format";
        private const string FIELDNEEDED = "*";
        private const string WRONGINT = "Wrong int format";
        private const string WRONGFLOAT = "Wrong float format";
        private const string WRONGBOOL = "Wrong bool format";
        private const string DICTIONARYLABELTEXT = "Select value";
        private const string GENERALLABELTEXT = "Enter value";
        private const string STRINGLABELTEXT = "Enter string"; 
        private const string DATELABELTEXT = "Enter date"; 
        private const string INTEGERLABELTEXT = "Enter integer number"; 
        private const string FLOATLABELTEXT = "Enter float number"; 
        private const string BOOLEANLABELTEXT = "Enter {0}/{1}"; 
        private const string TRUESTRING = "Y";
        private const string FALSESTRING = "N";
        private const string ONCLICKATTRIBUTE = "OnClick";
        private const string RULEUNITTEXT = "rule unit";
        private const string NOTSELECTED = " - Select - ";
        private const bool ALL = true;
        private const int CONTROLTEXTBOX = 0;
        private const int CONTROLSELECT = 1;
        private const int CONTROLDATEPICKER = 2;
        private const int CONTROLMASKEDTEXTBOX = 3;
        private const string YES = "Yes";
        private const string YESVALUE = "1";
        private const string NO = "No";
        private const string NOVALUE = "0";
        #endregion

        #region fields/properties
        private Field field = null;
        private LoanStar.Common.Rule rule = null;
        private int firstRowId = -1;
        protected int Step
        {
            get 
            {
                Object o = Session[STEPINDEX];
                int res = 0;
                if (o != null)
                {
                    try 
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch
                    {
                        Session[STEPINDEX] = res;
                    }
                }
                return res;
            }
            set
            {
                Session[STEPINDEX] = value;
            }
        }
        protected bool IsFirst
        {
            get
            {
                Object o = ViewState[FIRSTRULEUNIT];
                if (o != null)
                {
                    return (bool)o;
                }
                return true;
            }
            set
            {
                ViewState[FIRSTRULEUNIT] = value;
            }
        }
        protected bool IsInitiated
        {
            get
            {
                Object o = Session[ISINITIATED];
                if (o != null)
                {
                    return (bool)o;
                }
                return false;
            }
            set
            {
                Session[ISINITIATED] = value;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            field = GetField();
            rule = GetRule();
        }
        #region methods
        public void Initialize()
        {
            Step = 0;
            InitControls();
        }
        private void InitEventHandlers()
        { 
            this.ddlGroup.SelectedIndexChanged += new EventHandler(this.ddlGroup_SelectedIndexChanged);
        }
        private void InitControls()
        {
            field = GetField();
            rule = GetRule();
            if (IsInitiated)
            {
                return;
            }
            IsInitiated = true;
            BindGrid();
            DoStep();
            if ((Step == 0) && (IsFirst))
            {
                Step++;
                DoStep();
            }
        }
        private void BindGrid()
        {            
            DataView dv = rule.GetUnitList();
            IsFirst = dv.Count == 0;
            if (dv.Count > 0)
            {
                firstRowId = int.Parse(dv[0][LoanStar.Common.Rule.IDFIELDNAME].ToString());
            }
            G.DataSource = dv;
            G.DataBind();
            tdexpr.InnerHtml = rule.GetColoredCodeByDataView(dv);
        }
        private bool Validate()
        {
            bool res = true;
            validatormsg.Text = "";
            if (field.IsDictionaryField)
            {
                res = int.Parse(ddlDictionary.SelectedValue.ToString())!=0;
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
                    if (tbValue.Text == String.Empty)
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
                    res = mtb.Text != String.Empty;
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
            int controlType = CONTROLTEXTBOX;
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
        //private LoanStar.Common.Rule GetRule()
        //{
        //    LoanStar.Common.Rule r = CurrentPage.GetObject(Constants.RULEOBJECT) as LoanStar.Common.Rule;
        //    if (r == null)
        //    {
        //        r = new LoanStar.Common.Rule();
        //        CurrentPage.StoreObject(r, Constants.RULEOBJECT);
        //    }
        //    return r;
        //}
        private void DoStep()
        {
            if ((Step < 0) || (Step > 4))
            {
                Step = 0;
            }
            btnAdd.Enabled = false;
            ddlCompareOp.Enabled = false;
            ddlLogicalOp.Enabled = false;
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
                    cbNot.Checked = false;
                    ddlLogicalOp.DataSource = Field.GetLogicOperationList();
                    ddlLogicalOp.DataTextField = Field.COMPOPNAMEFIELDNAME;
                    ddlLogicalOp.DataValueField = Field.IDFIELDNAME;
                    ddlLogicalOp.DataBind();
                    ddlLogicalOp.Enabled = true;
                    ddlField.Items.Insert(0, new ListItem(NOTSELECTED, 0.ToString()));
                    ddlCompareOp.Items.Insert(0, new ListItem(NOTSELECTED, 0.ToString()));
                    ddlGroup.Items.Insert(0, new ListItem(NOTSELECTED, 0.ToString()));
                    btnCancel.Enabled = false;
                    break;
                case 1:
                    ddlGroup.DataSource = Field.GetFieldGroup(ALL);
                    ddlGroup.DataTextField = Field.GROUPNAMEFIELDNAME;
                    ddlGroup.DataValueField = Field.IDFIELDNAME;
                    ddlGroup.DataBind();
                    ddlGroup.Enabled = true;
                    ddlGroup.Focus();
                    btnCancel.Enabled = !IsFirst;
                    break;
                case 2:
                    ddlField.DataSource = Field.GetFieldInGroup(int.Parse(ddlGroup.SelectedValue.ToString()));
                    ddlField.DataTextField = Field.DESCRIPTIONFIELDNAME;
                    ddlField.DataValueField = Field.IDFIELDNAME;
                    ddlField.DataBind();
                    ddlField.Items.Insert(0, new ListItem(NOTSELECTED, 0.ToString()));
                    ddlField.Enabled = true;
                    ddlField.Focus();
                    btnCancel.Enabled = true;
                    break;
                case 3:
                    ddlField.Enabled = false;
                    field = new Field(int.Parse(ddlField.SelectedValue.ToString()));
                    SetValueFieldLabel();
                    SetValueFieldControl();
                    CurrentPage.StoreObject(field, Constants.FIELDOBJECT);
                    ddlCompareOp.DataSource = field.GetAllowedCompOpList();
                    ddlCompareOp.DataTextField = Field.COMPOPNAMEFIELDNAME;
                    ddlCompareOp.DataValueField = Field.IDFIELDNAME;
                    ddlCompareOp.DataBind();
                    ddlCompareOp.Enabled = true;
                    ddlCompareOp.Focus();
                    break;
                case 4:
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
                        ddlDictionary.Items.Add(new ListItem(NOTSELECTED,(-1).ToString()));
                        ddlDictionary.Items.Add(new ListItem(YES,YESVALUE));
                        ddlDictionary.Items.Add(new ListItem(NO,NOVALUE));
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
                        mtb.DisplayFormatPosition = Telerik.WebControls.DisplayFormatPosition.Right;
                        mtb.Focus();
                    }
                    else
                    {
                        tbValue.Enabled = true;
                        tbValue.Focus();
                    }
                    btnAdd.Enabled = true;
                    break;
            }
        }
        #endregion

        #region event handlers
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                RuleUnit ru = new RuleUnit();
                ru.RuleId = rule.Id;
                ru.PropertyName = ddlField.SelectedItem.Text;
                ru.LogicalNot = cbNot.Checked;
                ru.FieldId = field.Id;
                if (!IsFirst)
                {
                    ru.LogicalOpId = int.Parse(ddlLogicalOp.SelectedValue.ToString());
                }
                ru.CompareOpId = int.Parse(ddlCompareOp.SelectedValue.ToString());
                if (ddlDictionary.Visible)
                {
                    if (field.IsDictionaryField)
                    {
                        ru.ReferanceId = int.Parse(ddlDictionary.SelectedValue.ToString());
                    }
                    else
                    {
                        ru.DataValue = ddlDictionary.SelectedItem.Text;
                    }
                }
                else if (dpValue.Visible)
                {
                    ru.DataValue = dpValue.SelectedDate.ToShortDateString();
                }
                else if (mtb.Visible)
                {
                    ru.DataValue = mtb.TextWithLiterals.ToString();
                }
                else
                {
                    ru.DataValue = tbValue.Text;
                }
                int res = ru.Save();
                if (res > 0)
                {
                    BindGrid();
                    Step = 0;
                    DoStep();
                }
            }
        }
        protected void ddlLogicalOp_SelectedIndexChanged(object sender, EventArgs e)
        {
            Step = 1;
            DoStep();
        }
        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            Step = 2;
            DoStep();
        }
        protected void ddlField_SelectedIndexChanged(object sender, EventArgs e)
        {
            Step = 3;
            DoStep();
        } 
        protected void ddlCompareOp_SelectedIndexChanged(object sender, EventArgs e)
        {
            Step = 4;
            DoStep();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Step--;
            DoStep();
        }
        #endregion

        #region grid related handlers
        protected void G_PageIndexChanged(object source, Telerik.WebControls.GridPageChangedEventArgs e)
        {
            G.CurrentPageIndex = e.NewPageIndex;
            BindGrid();
        }
        protected void G_ItemCommand(object source, Telerik.WebControls.GridCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            int id = int.Parse(e.CommandArgument.ToString());
            switch (cmd)
            {
                case Constants.DELETECOMMAND:
                    RuleUnit ru = new RuleUnit();
                    ru.Id = id;
                    ru.Delete();
                    break;
                default:
                    return;
            }
            BindGrid();
        }
        protected string GetLogicalOp(Object o, string name)
        {
            string result = String.Empty;
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                if (firstRowId == int.Parse(row[LoanStar.Common.Rule.IDFIELDNAME].ToString()))
                {
                    return result;
                }
                result = row[name].ToString();
            }
            return result;
        }
        protected void G_ItemDataBound(object sender, Telerik.WebControls.GridItemEventArgs e)
        {
            if ((e.Item.ItemType == Telerik.WebControls.GridItemType.Item) || (e.Item.ItemType == Telerik.WebControls.GridItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row != null)
                {
                    string id = row[LoanStar.Common.Rule.IDFIELDNAME].ToString();
                    ImageButton imgbutton = (ImageButton)e.Item.Cells[7].Controls[1];
                    imgbutton.Attributes.Add(ONCLICKATTRIBUTE, String.Format(Constants.JSDELETECONFIRM, RULEUNITTEXT));
                    imgbutton.CommandArgument = id.ToString();
                }
            }
        }
        #endregion


        private LoanStar.Common.Rule GetRule()
        {
            LoanStar.Common.Rule r = CurrentPage.GetObject(Constants.RULEOBJECT) as LoanStar.Common.Rule;
            if (r == null)
            {
                r = new LoanStar.Common.Rule();
                CurrentPage.StoreObject(r, Constants.RULEOBJECT);
            }
            return r;
        }

    }
}