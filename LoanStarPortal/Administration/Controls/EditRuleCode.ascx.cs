using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
using LoanStar.Common;
using Telerik.WebControls;
using Rule=LoanStar.Common.Rule;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditRuleCode : RuleControl
    {
        #region constants
        private const string STEPINDEX = "editorstep";
        private const bool ALL = true;
        private const string NOTSELECTED = " - Select - ";
        private const string NOTSELECTEDVALUE = "0";
        private const string YES = "Yes";
        private const string YESVALUE = "1";
        private const string NO = "No";
        private const string NOVALUE = "0";
        private const string RULEUNITTEXT = "rule unit";
        private const string CLICKHANDLER = "onclick";
        private const string JSSETDATES = "SetDates()";
        private const string CHECHALLJS = "CheckAll(this,divproduct);";
        private const string ADDHEADERTEXT = "Add rule";
        private const string EDITHEADERTEXT = "Edit rule({0})";
        private const string FIELDNEEDEDMESSAGE = "*";
        private const string ROOTELEMENT = "Root";
        private const string ITEMELEMENT = "item";
        private const string WRONGDATESMESSAGE = "Date From should be less then Date To";
        private const string IDATTRIBUTE = "id";
        private const string PRODUCTIDATTRIBUTE = "productid";
        private const string DICTIONARYLABELTEXT = "Select value";
        private const string GENERALLABELTEXT = "Enter value";
        private const string STRINGLABELTEXT = "Enter string";
        private const string DATELABELTEXT = "Enter date";
        private const string INTEGERLABELTEXT = "Enter integer number";
        private const string FLOATLABELTEXT = "Enter float number";
        private const string ANOTHERFIELDLABELTEXT = "Select another field";
        private const int CONTROLTEXTBOX = 0;
        private const int CONTROLSELECT = 1;
        private const int CONTROLDATEPICKER = 2;
        private const int CONTROLMASKEDTEXTBOX = 3;
        private const string ALREADYEXISTSMESSAGE = "Rule with such name already exists";
        private const string CANTSAVEMESSAGE = "Can't save rule";
        private const string CHECHPRODUCTJS = "CheckField(this,{0},divproduct);";
        private const string SELECTEDFIELDNAME = "Selected";
        private const string DELETECOMMAND = "deleteruleunit";
        private const string VALUETYPETEXT = "Value";
        private const string FIELDTYPETEXT = "Field";
        private const int VALUETYPEVALUE = 1;
        private const int FIELDTYPEVALUE = 2;
        #endregion

        #region fields
        private int firstRowId = -1;
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

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            field = GetField();
            cbEnableDaterange.Attributes.Add(CLICKHANDLER, JSSETDATES);
            cbAll.Attributes.Add(CLICKHANDLER, CHECHALLJS);                
        }

        #region methods
        public override void Initialize()
        {
            base.Initialize();
            tbName.Focus();
            if (rule.ID > 0)
            {
                divRuleInfo.Visible = true;
                InitializeEditor();
            }
            else
            {
                divRuleInfo.Visible = false;
            }
            BindData();
        }
        protected void BindData()
        {
            cbEnableDaterange.Checked = rule.StartDate != null;
            ddlCategory.DataSource = Rule.GetCategory();
            ddlCategory.DataTextField = Rule.NAMEFIELDNAME;
            ddlCategory.DataValueField = Rule.IDFIELDNAME;
            ddlCategory.DataBind();
            dlProducts.DataSource = rule.GetProductList();
            dlProducts.DataBind();
            if (rule.ID > 0)
            {
                lblHeader.Text = String.Format(EDITHEADERTEXT, rule.Name);
                tbName.Text = rule.Name;
                if (rule.StartDate != null)
                {
                    raddpFrom.SelectedDate = rule.StartDate;
                    raddpTo.SelectedDate = rule.EndDate;
                }
                ddlCategory.SelectedValue = rule.CategoryId.ToString();
            }
            else
            {
                lblHeader.Text = ADDHEADERTEXT;
            }
        }
        private bool Validate()
        {
            bool res = true;
            lbltberr.Text = String.Empty;
            lblerrfrom.Text = String.Empty;
            lblerrto.Text = String.Empty;
            lblMessage.Text = String.Empty;
            if (String.IsNullOrEmpty(tbName.Text)) 
            {
                lbltberr.Text = FIELDNEEDEDMESSAGE;
                res = false;
            }
            if ((ddlCategory.SelectedValue == (-1).ToString()) && (String.IsNullOrEmpty(ddlCategory.Text)))
            {
                lblErrCategory.Text = FIELDNEEDEDMESSAGE;
                res = false;
            }
            if (cbEnableDaterange.Checked)
            {
                if (raddpFrom.IsEmpty)
                {
                    lblerrfrom.Text = FIELDNEEDEDMESSAGE;
                    res = false;
                }
                if (raddpTo.IsEmpty)
                {
                    lblerrto.Text = FIELDNEEDEDMESSAGE;
                    res = false;
                }
                if (raddpTo.SelectedDate < raddpFrom.SelectedDate)
                {
                    lblMessage.Text = WRONGDATESMESSAGE;
                    res = false;
                }
            }
            return res;
        }
        private bool ValidateEditor()
        {
            bool res = true;
            validatormsg.Text = "";
            if (field.IsDictionaryField || (ddlFieldType.SelectedValue == FIELDTYPEVALUE.ToString()))
            {
                res = int.Parse(ddlDictionary.SelectedValue) != 0;
                if (!res)
                {
                    validatormsg.Text = FIELDNEEDEDMESSAGE;
                }
                return res;
            }
            Field.MortgageProfileFieldType tp = (Field.MortgageProfileFieldType)field.TypeId;

            switch (tp)
            {
                case Field.MortgageProfileFieldType.String:
                    if (String.IsNullOrEmpty(tbValue.Text))
                    {
                        validatormsg.Text = FIELDNEEDEDMESSAGE;
                        res = false;
                    }
                    break;
                case Field.MortgageProfileFieldType.DateTime:
                    res = !dpValue.IsEmpty;
                    if (!res)
                    {

                        validatormsg.Text = FIELDNEEDEDMESSAGE;
                    }
                    break;
                case Field.MortgageProfileFieldType.Integer:
                case Field.MortgageProfileFieldType.Float:
                    res = !String.IsNullOrEmpty(mtb.Text);
                    if (!res)
                    {
                        validatormsg.Text = FIELDNEEDEDMESSAGE;
                    }
                    break;
                case Field.MortgageProfileFieldType.Boolean:
                    res = !(ddlDictionary.SelectedValue == "-1");
                    if (!res)
                    {
                        validatormsg.Text = FIELDNEEDEDMESSAGE;
                    }
                    break;
            }
            return res;
        }
        private string GetProductList()
        {
            string res = String.Empty;
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            int cnt = 0;
            for (int i = 0; i<dlProducts.Items.Count; i++)
            {
                CheckBox cb = dlProducts.Items[i].Controls[1] as CheckBox;
                if ((cb != null) && (cb.Checked))
                {
                    XmlNode n = d.CreateElement(ITEMELEMENT);
                    XmlAttribute a = d.CreateAttribute(IDATTRIBUTE);
                    a.Value = cb.Attributes[PRODUCTIDATTRIBUTE];
                    n.Attributes.Append(a);
                    root.AppendChild(n);
                    cnt++;
                }
            }
            if (cnt > 0)
            {
                d.AppendChild(root);
                res = d.OuterXml;
            }
            return res;
        }
        private void InitializeEditor()
        {            
            BindGrid();
            BindDictionary();
            InitEditorFields();
            Step = G.Items.Count > 0 ? 0 : 1;
            DoStep();
        }
        private void InitEditorFields()
        {
            tbValue.Text = String.Empty;
            field = new Field();
            CurrentPage.StoreObject(field, Constants.FIELDOBJECT);
        }
        private void BindGrid()
        {
            DataView dv = rule.GetUnitList();
            if (dv.Count > 0)
            {
                firstRowId = int.Parse(dv[0][Rule.IDFIELDNAME].ToString());
            }
            G.DataSource = dv;
            G.DataBind();
            tdexpr.InnerHtml = rule.GetColoredCodeByDataView(rule.GetUnitListWithParent());
        }
        private void BindDictionary()
        {
            ddlFieldType.Items.Clear();
            ddlFieldType.Items.Insert(0, new ListItem(NOTSELECTED, NOTSELECTEDVALUE));
            ddlLogicalOp.DataSource = Field.GetLogicOperationList();
            ddlLogicalOp.DataTextField = Field.COMPOPNAMEFIELDNAME;
            ddlLogicalOp.DataValueField = Field.IDFIELDNAME;
            ddlLogicalOp.DataBind();
            ddlGroup.DataSource = Field.GetFieldGroup(ALL);
            ddlGroup.DataTextField = Field.GROUPNAMEFIELDNAME;
            ddlGroup.DataValueField = Field.IDFIELDNAME;
            ddlGroup.DataBind();
            ddlField.Items.Insert(0, new ListItem(NOTSELECTED, NOTSELECTEDVALUE));
            ddlCompareOp.Items.Insert(0, new ListItem(NOTSELECTED, NOTSELECTEDVALUE));
        }
        private void DoStep()
        {
            if ((Step < 0) || (Step > 5))
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
            ddlFieldType.Enabled = false;
            dpValue.Enabled = false;
            mtb.Enabled = false;
            SetValueFieldLabel();
            SetValueFieldControl();
            switch (Step)
            {
                case 0:
                    cbNot.Checked = false;
                    ddlLogicalOp.SelectedValue = NOTSELECTEDVALUE;
                    ddlLogicalOp.Enabled = true;
                    ddlField.Items.Insert(0, new ListItem(NOTSELECTED, 0.ToString()));
                    ddlCompareOp.Items.Insert(0, new ListItem(NOTSELECTED, 0.ToString()));
                    btnCancel.Enabled = false;
                    break;
                case 1:
                    ddlGroup.Enabled = true;
                    ddlGroup.SelectedValue = NOTSELECTEDVALUE;
                    ddlGroup.Focus();
                    btnCancel.Enabled = G.Items.Count>0;
                    break;
                case 2:
                    ddlField.DataSource = Field.GetFieldInGroup(int.Parse(ddlGroup.SelectedValue));
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
                    field = new Field(int.Parse(ddlField.SelectedValue));
                    //SetValueFieldLabel();
                    //SetValueFieldControl();
                    CurrentPage.StoreObject(field, Constants.FIELDOBJECT);
                    ddlCompareOp.DataSource = field.GetAllowedCompOpList();
                    ddlCompareOp.DataTextField = Field.COMPOPNAMEFIELDNAME;
                    ddlCompareOp.DataValueField = Field.IDFIELDNAME;
                    ddlCompareOp.DataBind();
                    ddlCompareOp.Enabled = true;
                    ddlCompareOp.Focus();
                    break;
                case 4:
                    ddlFieldType.Items.Clear();
                    ddlFieldType.Items.Insert(0, new ListItem(NOTSELECTED, NOTSELECTEDVALUE));
                    ddlFieldType.Items.Add(new ListItem(VALUETYPETEXT, VALUETYPEVALUE.ToString()));
                    ddlFieldType.Items.Add(new ListItem(FIELDTYPETEXT, FIELDTYPEVALUE.ToString()));
                    ddlFieldType.Enabled = true;
                    ddlFieldType.Focus();
                    break;
                case 5:
                    SetValueFieldLabel();
                    SetValueFieldControl();
                    if (ddlFieldType.SelectedValue == VALUETYPEVALUE.ToString())
                    {
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
                        else if ((field.TypeId == (int)Field.MortgageProfileFieldType.Integer) || (field.TypeId == (int)Field.MortgageProfileFieldType.Float) || (field.TypeId == (int)Field.MortgageProfileFieldType.Decimal))
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
                    }
                    else
                    {
                        ddlDictionary.Enabled = true;
                        ddlDictionary.DataSource = field.GetFieldList();
                        ddlDictionary.DataTextField = Field.FIELDNAMEFIELDNAME;
                        ddlDictionary.DataValueField = Field.IDFIELDNAME;
                        ddlDictionary.DataBind();
                        ddlDictionary.Focus();
                    }
                    btnAdd.Enabled = true;
                    break;
            }
        }
        private void SetValueFieldLabel()
        {
            if (ddlFieldType.SelectedValue == FIELDTYPEVALUE.ToString())
            {
                lblValue.Text = ANOTHERFIELDLABELTEXT;
            }
            else
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
        }
        private void SetValueFieldControl()
        {
            Field.MortgageProfileFieldType tp = (Field.MortgageProfileFieldType)field.TypeId;
            int controlType;
            if (field.IsDictionaryField || (ddlFieldType.SelectedValue == FIELDTYPEVALUE.ToString()))
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
                    tbValue.Enabled = true;
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
        #endregion

        #region postback event handlers
        protected void btnBack_Click(object sender, EventArgs e)
        {
            goBack();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                rule.Name = tbName.Text;
                if (String.IsNullOrEmpty(ddlCategory.SelectedValue))
                {
                    rule.CategoryId = -1;
                }
                else 
                {
                    rule.CategoryId = int.Parse(ddlCategory.SelectedValue);
                }
                
                rule.CategoryName = ddlCategory.Text;
                if (cbEnableDaterange.Checked)
                {
                    rule.StartDate = raddpFrom.SelectedDate;
                    rule.EndDate = raddpTo.SelectedDate;
                }
                else
                {
                    rule.StartDate = null;
                    rule.EndDate = null;
                }
                string productList = GetProductList();
                int res = rule.Save(productList);
                if (res > 0)
                {
                    lblMessage.Text = Constants.SUCCESSMESSAGE;
                    CurrentPage.StoreObject(rule, Constants.RULEOBJECT);
                    cbEnableDaterange.Checked = rule.StartDate != null;
                    divRuleInfo.Visible = rule.ID > 0;
                    InitializeEditor();
                    refreshMainGrid();
                }
                else if (res == -1)
                {
                    lblMessage.Text = ALREADYEXISTSMESSAGE;
                }
                else
                {
                    lblMessage.Text = CANTSAVEMESSAGE; ;
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
        protected void ddlFieldType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Step = 5;
            DoStep();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Step--;
            DoStep();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateEditor())
            {
                RuleUnit ru = new RuleUnit();
                ru.RuleId = rule.ID;
                ru.PropertyName = ddlField.SelectedItem.Text;
                ru.LogicalNot = cbNot.Checked;
                ru.FieldId = field.ID;
                ru.LiteralValue = ddlFieldType.SelectedValue == VALUETYPEVALUE.ToString();
                if (!ru.LiteralValue)
                { 
                    ru.ReferanceId = int.Parse(ddlField.SelectedValue);
                }
                if (G.Items.Count>0)
                {
                    ru.LogicalOpId = int.Parse(ddlLogicalOp.SelectedValue);
                }
                ru.CompareOpId = int.Parse(ddlCompareOp.SelectedValue);
                if (ddlDictionary.Visible)
                {
                    if (field.IsDictionaryField)
                    {
                        ru.ReferanceId = int.Parse(ddlDictionary.SelectedValue);
                    }
                    else
                    {
                        ru.DataValue = ddlDictionary.SelectedItem.Text;
                    }
                }
                else if (dpValue.Visible)
                {
                    ru.DataValue = Convert.ToDateTime(dpValue.SelectedDate).ToShortDateString();
                }
                else if (mtb.Visible)
                {
                    ru.DataValue = mtb.TextWithLiterals;
                }
                else
                {
                    ru.DataValue = tbValue.Text;
                }
                int res = ru.Save();
                if (res > 0)
                {
                    InitializeEditor();
                }
            }
        }
        #endregion

        #region databound event handler
        protected void dlProducts_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            if (dr != null)
            {
                int id = int.Parse(dr[Rule.IDFIELDNAME].ToString());
                CheckBox cb = (CheckBox)e.Item.Controls[1];
                if (cb != null)
                {
                    cb.Attributes.Add(CLICKHANDLER, String.Format(CHECHPRODUCTJS,cbAll.ClientID));
                    cb.Attributes.Add(PRODUCTIDATTRIBUTE, id.ToString());
                    cb.Checked = int.Parse(dr[SELECTEDFIELDNAME].ToString()) == 1;
                }
            }
        }
        #endregion


        #region grid related handlers
        protected void G_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            G.CurrentPageIndex = e.NewPageIndex;
            BindGrid();
        }
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            int id = int.Parse(e.CommandArgument.ToString());
            switch (cmd)
            {
                case DELETECOMMAND:
                    RuleUnit ru = new RuleUnit();
                    ru.Id = id;
                    ru.Delete();
                    InitializeEditor();
                    break;
                default:
                    return;
            }
        }
        protected string GetLogicalOp(Object o, string name)
        {
            string result = String.Empty;
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                if (firstRowId == int.Parse(row[Rule.IDFIELDNAME].ToString()))
                {
                    return result;
                }
                result = row[name].ToString();
            }
            return result;
        }
        protected void G_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row != null)
                {
                    string id = row[Rule.IDFIELDNAME].ToString();
                    ImageButton imgbutton = (ImageButton)e.Item.Cells[8].Controls[1];
                    imgbutton.Attributes.Add(ONCLICKATTRIBUTE, String.Format(Constants.JSDELETECONFIRM, RULEUNITTEXT));
                    imgbutton.CommandArgument = id;
                }
            }
        }
        #endregion 

    }    
}