using System;
using System.Data;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Telerik.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class Fields : AppControl
    {
        #region constants

        private const string CLASSATTRIBUTE = "class";
        private const string HTMLSPACE = "&nbsp;";
        private const string NOTSELECTEDTEXT = "-Select-";
        private const int NOTSELECTEDVALUE = 0;
        private const string IDFIELDNAME = "id";
        private const int TEXTBOXWIDTH = 155;
        private const int DROPDOWNWIDTH = 161;
        private const int RADDATEINPUTWIDTH = 159;
        private const int RADMASKEDINPUTWIDTH = 159;
        private const int RADCOMBOHEIGHT = 107;
        private const int CONTROLHEIGHT = 18;
        private const string MONEYFORMAT = "money";
        private const string NUMBERFORMAT = "number";
        private const string PERCENTAGEFORMAT = "percentage";
        #endregion

        #region fields
        private int radComboWidth;
        private short startTabOrder = 0;
        private short tabIndex = 0;
        public string Title = string.Empty;
        public bool HasChild;
        #endregion

        #region properties
        public short StartTabOrder
        {
            get { return startTabOrder; }
            set { startTabOrder = value; }
        }
        public short TabIndex
        {
            get { return tabIndex; }
        }
        public int RadComboWidth
        {
            set { radComboWidth = value; }
        }
        public PlaceHolder plholder
        {
            get
            {
                PlaceHolder ctl = null;
                Control ctrl = FindControl("ph");
                if (ctrl != null)
                {
                    ctl = ctrl as PlaceHolder;
                }
                return ctl;
            }
        }
        public string TabTitle
        {
            set
            {
                lblTitle.Text = value;
            }
        }
        #endregion

        #region Methods
        protected void BindYesNoList(RadioButtonList rbl,Object fieldValue)
        {
            DataView dv = CurrentPage.GetDictionary(Constants.DICTIONARYYESNOTABLE);
            if(dv!=null)
            {
                rbl.DataSource = dv;
                rbl.DataTextField = "Name";
                rbl.DataValueField = "Id";
                rbl.DataBind();
                int id = -1;
                if (fieldValue!=null)
                {
                    id = ((bool) fieldValue) ? 1 : 2;
                }
                if (id > 0)
                {
                    rbl.SelectedValue = id.ToString();
                }
                rbl.Attributes.Add("selected", id.ToString());
            }
        }

        protected void BindRadioButtonList(RadioButtonList rbl, MortgageProfileField mpf)
        {
            DataView dv = mpf.ReadDataSource();
            if (dv != null)
            {
                int id = -1;
                try
                {
                    id = Convert.ToInt32(mpf.FieldValue.ToString());
                }
                catch
                {
                }
                rbl.DataSource = dv;
                rbl.DataTextField = mpf.FieldName;//"Name";
                rbl.DataValueField = "Id";
                rbl.DataBind();
                if (id > 0 && dv.Count > 0)
                {
                    rbl.SelectedValue = id.ToString();
                }
                rbl.Attributes.Add("selected",id.ToString());
            }
        }
        protected void BindDropDown(DropDownList ddl, MortgageProfileField mpf)
        {
            DataView dv = null;
            //if (mpf.TableName.ToLower() == "vendor")
            //{
            //    dv = Vendor.GetVendorListByCompany(CurrentUser.CompanyId);
            //    dv.RowFilter = mpf.FilterName;
            //}
            //else
                dv = mpf.ReadDataSource();

            if (dv.Count > 0)
            {
                ddl.DataSource = dv;
                ddl.DataTextField = mpf.FieldName;
                ddl.DataValueField = IDFIELDNAME;
                ddl.DataBind();
            }
            ddl.Items.Insert(0, new ListItem(NOTSELECTEDTEXT, NOTSELECTEDVALUE.ToString()));
        }
        #region RadCombo version
        //protected void BindDropDown(RadComboBox ddl, string tableName, string fieldName, string filterName)
        //{
        //    DataView dv;
        //    if (tableName.ToLower() == "vendor")
        //        dv = Vendor.GetVendorListByCompany(CurrentUser.CompanyId);
        //    else
        //        dv = CurrentPage.GetDictionary(tableName);

        //    if (dv.Count > 0)
        //    {
        //        dv.RowFilter = filterName;
        //        ddl.DataSource = dv;
        //        ddl.DataTextField = fieldName;
        //        ddl.DataValueField = IDFIELDNAME;
        //        ddl.DataBind();
        //        ddl.Items.Insert(0, new RadComboBoxItem(NOTSELECTEDTEXT, NOTSELECTEDVALUE.ToString()));
        //    }
        //}
        #endregion
        public void BuildControls(HtmlTable container, string trStyle, string[] tdStyle, ArrayList objects, int columns)
        {
            lblTitle.Text = Title;
            int cnt = objects.Count / columns;
            if (cnt * columns < objects.Count)
            {
                cnt++;
            }
            for (int i = 0; i < cnt; i++)
            {
                container.Rows.Add(GetTr(i, objects, trStyle, tdStyle, columns, cnt));
            }
            ph.Controls.Add(container);
        }
        public void AddControls(Control ctrl)
        {
            ph.Controls.Add(ctrl);
        }
        private HtmlTableRow GetTr(int i, IList objects,string trStyle,string[] tdStyle,int columns, int rows)
        {
            HtmlTableRow tr = new HtmlTableRow();
            if (!String.IsNullOrEmpty(trStyle))
            {
                tr.Attributes.Add(CLASSATTRIBUTE, trStyle);
            }
            for (int j = 0; j < columns; j++)
            {
                string styleLabel = GetStyle(tdStyle, j,0);
                string styleControl = GetStyle(tdStyle, j, 1);
                int index = i+j*rows;
                tabIndex = (short) index;
                tabIndex += startTabOrder;
                AddOneObject(tr, objects,index, styleLabel, styleControl);
            }
            return tr;
        }
        private static string GetStyle(string[] tdStyle, int i, int k)
        {
            if (tdStyle == null)
            {
                return String.Empty;
            }
            string[] arr;
            if (tdStyle.Length == 1)
            {
                arr = tdStyle[0].Split('/');
            }
            else
            {
                int ii = i;
                if (ii >= tdStyle.Length)
                {
                    ii = tdStyle.Length-1;
                }
                arr = tdStyle[ii].Split('/');
            }
            if (k < arr.Length)
            {
                return arr[k];
            }
            return String.Empty;
        }
        private void AddOneObject(HtmlTableRow tr, IList objects, int i, string styleLabel, string styleControl)
        {
            HtmlTableCell tdlabel = new HtmlTableCell();
            HtmlTableCell tdctl = new HtmlTableCell();
            MortgageProfileField mpf = null;
            if (i < objects.Count)
            {
                mpf = (MortgageProfileField)objects[i];
            }                
            if (!String.IsNullOrEmpty(styleLabel))
            {
                tdlabel.Attributes.Add(CLASSATTRIBUTE, styleLabel);
            }
            if (!String.IsNullOrEmpty(styleControl))
            {
                tdctl.Attributes.Add(CLASSATTRIBUTE, styleControl);
            }
            if (mpf != null)
            {
                if (mpf.IsVisible)
                {
                    Literal lbl = new Literal();
                    if (i < objects.Count)
                    {
                        lbl.Text = mpf.Decsription;
                        lbl.ID = "l_" + mpf.PropertyName;
                        tdctl.Controls.Add(GetControl(mpf));
                        HtmlGenericControl err = new HtmlGenericControl("span");
                        err.Attributes.Add("style", "color:red");
                        err.ID = mpf.FullPropertyName + "_" + mpf.ParentId + "_e";
                        tdctl.Controls.Add(err);
                        HasChild = true;
                    }
                    else
                    {
                        lbl.Text = String.Empty;
                        tdctl.InnerHtml = HTMLSPACE;
                    }
                    //Label lbl = new Label();
                    //if (i < objects.Count)
                    //{
                    //    lbl.Text = mpf.Decsription;
                    //    lbl.ID = "l_" + mpf.PropertyName;
                    //    tdctl.Controls.Add(GetControl(mpf));
                    //    HtmlGenericControl err = new HtmlGenericControl("span");
                    //    err.Attributes.Add("style", "color:red");
                    //    err.ID = mpf.FullPropertyName + "_" + mpf.ParentId+"_e";
                    //    tdctl.Controls.Add(err);
                    //    HasChild = true;
                    //}
                    //else
                    //{
                    //    lbl.Text = String.Empty;
                    //    tdctl.InnerHtml = HTMLSPACE;
                    //}
                    tdlabel.Controls.Add(lbl);
                }
            }
            tr.Cells.Add(tdlabel);
            tr.Cells.Add(tdctl);
        }
        private static void AttachScript(Control ctl,bool needPostback)
        {
            if (ctl == null)
            {

                return;
            }
            if (ctl.GetType() == typeof(TextBox))
            {
                TextBox tctl = (TextBox)ctl;
                tctl.Attributes.Add("onfocus", "FocusGot(this);");
                tctl.Attributes.Add("onblur", "FocusLost(this);");
            }
            else if (ctl.GetType() == typeof(CheckBox))
            {
                ((CheckBox)ctl).Attributes.Add("onclick", "ClickCheckbox(this,"+(needPostback?"1":"0")+");");
            }
            else if (ctl.GetType() == typeof(DropDownList))
            {
                DropDownList rctl = (DropDownList)ctl;
                rctl.Attributes.Add("onchange", "DDLIndexChanged(this," + (needPostback ? "1" : "0") + ");");
                rctl.Attributes.Add("onfocus", "DDLFocusGot(this);");
            }
            //else if (ctl.GetType() == typeof(RadComboBox))
            //{
            //    RadComboBox rctl = (RadComboBox)ctl;
            //    rctl.OnClientSelectedIndexChanged = "RadComboIndexChanged(this," + (needPostback ? "1" : "0") + ");";
            //    rctl.OnClientFocus = "RadComboFocusGot(this);";
            //}
            else if (ctl.GetType() == typeof(RadDateInput))
            {
                ((RadDateInput)ctl).ClientEvents.OnValueChanged = "RadInputDateChanged";
            }
            else if (ctl.GetType() == typeof(RadioButtonList))
            {
                RadioButtonList rbl = (RadioButtonList)ctl;
                rbl.Attributes.Add("onclick", "ClickRBList(this);");
            }
            else if (ctl.GetType() == typeof(RadMaskedTextBox))
            {
                ((RadMaskedTextBox)ctl).Attributes.Add("onfocus", "RadMaskedTextBoxFocusGot(this);");
                ((RadMaskedTextBox)ctl).Attributes.Add("onblur", "RadMaskedTextBoxFocusLost(this," + (needPostback ? "1" : "0") + ");");
            }
            else if (ctl.GetType() == typeof(RadNumericTextBox))
            {
                ((RadNumericTextBox)ctl).Attributes.Add("onfocus", "RadMaskedTextBoxFocusGot(this);");
                ((RadNumericTextBox)ctl).Attributes.Add("onblur", "RadMaskedTextBoxFocusLost(this," + (needPostback ? "1" : "0") + ");");
            }

        }
        private Control GetControl(MortgageProfileField fld)
        {
            Control ctl = null;
            switch(fld.ControlTypeId)
            {
                case MPFieldType.TextBox:
                    TextBox tctl = new TextBox();
                    tctl.TextMode = TextBoxMode.SingleLine;
                    if (fld.FieldValue != null)
                    {
                        if (fld.FieldValue is decimal)
                        {
                            tctl.Text = ((decimal)fld.FieldValue).ToString("0.00");
                        }
                        else
                        {
                            tctl.Text = fld.FieldValue.ToString();
                        }                       
                    }
                    tctl.Enabled = !fld.ReadOnly;
                    tctl.Width = Unit.Pixel(TEXTBOXWIDTH);
//                    tctl.Height = Unit.Pixel(CONTROLHEIGHT);
                    tctl.TabIndex = tabIndex;
                    ctl = tctl;
                    if (tctl.Enabled)
                    {
                        tctl.Attributes.Add("needPostBack", fld.IsPostBack ? "1" : "0");
                        AttachScript(ctl, fld.IsPostBack);
                    }
                    break;
                case MPFieldType.TextArea:
                    TextBox mtctl = new TextBox();
                    mtctl.TextMode = TextBoxMode.MultiLine;
                    if (fld.FieldValue != null) mtctl.Text = fld.FieldValue.ToString();
                    mtctl.Enabled = !fld.ReadOnly;
                    mtctl.Width = Unit.Pixel(TEXTBOXWIDTH);
                    mtctl.TabIndex = tabIndex;
                    ctl = mtctl;
                    if (mtctl.Enabled)
                    {
                        AttachScript(ctl, fld.IsPostBack);
                    }
                    break;
                case MPFieldType.DropDownList:
                    DropDownList dctl = new DropDownList();
                    BindDropDown(dctl, fld);
                    if (fld.FieldValue != null) dctl.SelectedValue = fld.FieldValue.ToString();
                    if (dctl.SelectedValue == "")
                    {
                        dctl.SelectedValue = NOTSELECTEDVALUE.ToString();
                    }
                    dctl.Enabled = !fld.ReadOnly;
                    dctl.Width = Unit.Pixel(DROPDOWNWIDTH);
                    //dctl.Height = Unit.Pixel(CONTROLHEIGHT);
                    dctl.TabIndex = tabIndex;
                    ctl = dctl;
                    if (dctl.Enabled)
                    {
                        AttachScript(ctl, fld.IsPostBack);
                    }
                    break;
                //case MPFieldType.DropDownList:
                //    RadComboBox dctl = new RadComboBox();
                //    BindDropDown(dctl, fld.TableName, fld.FieldName,fld.FilterName);
                //    if (fld.FieldValue != null) dctl.SelectedValue = fld.FieldValue.ToString();
                //    if(dctl.SelectedValue=="")
                //    {
                //        dctl.SelectedValue = NOTSELECTEDVALUE.ToString();
                //    }
                //    dctl.Enabled = !fld.ReadOnly;
                //    dctl.EnableLoadOnDemand = false;
                //    dctl.Skin = "WindowsXP";
                //    dctl.Width = Unit.Pixel(radComboWidth);
                //    dctl.Height = Unit.Pixel(RADCOMBOHEIGHT);
                //    dctl.TabIndex = tabIndex;
                //    dctl.MarkFirstMatch = false;//to improve speed on client is set to false
                //    ctl = dctl;
                //    if (dctl.Enabled)
                //    {
                //       AttachScript(ctl, fld.IsPostBack);
                //    }
                //    break;
                case MPFieldType.CheckBox:
                    CheckBox cctl = new CheckBox();
                    cctl.Checked = Convert.ToBoolean(fld.FieldValue);
                    cctl.Enabled = !fld.ReadOnly;
                    cctl.TabIndex = tabIndex;
                    ctl = cctl;
                    if (cctl.Enabled)
                    {
                        AttachScript(ctl, fld.IsPostBack);
                    }
                    break;
                case MPFieldType.DateInput:
                    RadDateInput rctl = new RadDateInput();
                    rctl.Skin = "Windows";
                    rctl.MinDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
                    rctl.Enabled = !fld.ReadOnly;
                    rctl.TabIndex = tabIndex;
                    if (fld.FieldValue != null) 
                    {
                        DateTime dt = Convert.ToDateTime(fld.FieldValue);
                        if (dt == System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                        {
                            rctl.SelectedDate = null;
                        }
                        else
                        {
                            rctl.SelectedDate = DateTime.Parse(fld.FieldValue.ToString());// Convert.ToDateTime(fld.FieldValue);
                        }
                    }                    
                    else
                    {
                        rctl.SelectedDate = null;
                    }
                    rctl.Width = Unit.Pixel(RADDATEINPUTWIDTH);
                    rctl.Height = Unit.Pixel(20);
//                    rctl.Height = Unit.Pixel(CONTROLHEIGHT);
                    ctl = rctl;
                    if (rctl.Enabled)
                    {
                        rctl.Attributes.Add("needPostBack", fld.IsPostBack?"1":"0");
                        AttachScript(ctl, fld.IsPostBack);
                    }
                    break;
                case MPFieldType.Label:
                    Literal lctl = new Literal();
                    if (fld.FieldValue != null)
                    {
                        if (fld.FieldValue is decimal)
                        {
                            lctl.Text = ((decimal)fld.FieldValue).ToString("C");
                        }
                        else
                        {
                            lctl.Text = fld.FieldValue.ToString();
                        }
                    }
                    ctl = lctl;
                    break;
                //case MPFieldType.Label:
                //    Label lctl = new Label();
                //    if(fld.FieldValue!=null)
                //    {
                //        if (fld.FieldValue is decimal)
                //        {
                //            lctl.Text = ((decimal)fld.FieldValue).ToString("C");
                //        }
                //        else
                //        {
                //            lctl.Text = fld.FieldValue.ToString();
                //        }
                //    }
                //    ctl = lctl;
                //    break;
                case MPFieldType.RadioButtonList:
                    RadioButtonList rbl = new RadioButtonList();
                    rbl.RepeatLayout = RepeatLayout.Table;
                    rbl.RepeatDirection = RepeatDirection.Horizontal;
                    BindRadioButtonList(rbl, fld);
                    rbl.Enabled = !fld.ReadOnly;
                    rbl.TabIndex = tabIndex;
                    ctl = rbl;
                    if (rbl.Enabled)
                    {
                        rbl.Attributes.Add("needPostBack", fld.IsPostBack ? "1" : "0");
                        AttachScript(ctl, fld.IsPostBack);
                    }
                    break;
                case MPFieldType.YesNo:
                    RadioButtonList rblyn = new RadioButtonList();
                    rblyn.RepeatLayout = RepeatLayout.Table;
                    rblyn.RepeatDirection = RepeatDirection.Horizontal;
                    BindYesNoList(rblyn, fld.FieldValue );
                    rblyn.Enabled = !fld.ReadOnly;
                    rblyn.TabIndex = tabIndex;
                    ctl = rblyn;
                    if (rblyn.Enabled)
                    {
                        rblyn.Attributes.Add("needPostBack", fld.IsPostBack ? "1" : "0");
                        AttachScript(ctl, fld.IsPostBack);
                    }
                    break;
                case MPFieldType.MaskedInput:
                    RadMaskedTextBox rmtb = new RadMaskedTextBox();
                    rmtb.Skin = "Windows";
                    rmtb.Enabled = !fld.ReadOnly;
                    rmtb.TabIndex = tabIndex;
                    rmtb.HideOnBlur = true;
                    rmtb.Width = Unit.Pixel(RADMASKEDINPUTWIDTH);
                    rmtb.Height = Unit.Pixel(20);
//                    rmtb.Height = Unit.Pixel(CONTROLHEIGHT);
                    if (fld.FieldValue != null)
                    {
                        rmtb.Text = fld.FieldValue.ToString();
                    }
                    rmtb.Mask = fld.MaskValue;;
                    rmtb.DisplayMask = fld.MaskValue; 
                    rmtb.PromptChar = " ";

                    ctl = rmtb;
                    if (rmtb.Enabled)
                    {
                        rmtb.Attributes.Add("needPostBack", fld.IsPostBack ? "1" : "0");
                        AttachScript(ctl, fld.IsPostBack);
                    }
                    break;
                case MPFieldType.MoneyInput:
                    RadNumericTextBox rntb = new RadNumericTextBox();
                    rntb.Skin = "Windows";
                    rntb.Enabled = !fld.ReadOnly;
                    rntb.TabIndex = tabIndex;
                    rntb.Width = Unit.Pixel(RADMASKEDINPUTWIDTH);
                    rntb.Height = Unit.Pixel(20);
//                    rntb.Height = Unit.Pixel(CONTROLHEIGHT);
                    if (fld.FieldValue != null)
                    {
                        rntb. Text = fld.FieldValue.ToString();
                    }
                    if(fld.MaskValue==MONEYFORMAT)
                    {
                        rntb.Type = NumericType.Currency;
                        rntb.NumberFormat.DecimalDigits = 2;
                        rntb.NumberFormat.DecimalSeparator = ".";
                        rntb.NumberFormat.GroupSeparator = ",";
                        rntb.NumberFormat.GroupSizes = 3;
                    }
                    else if(fld.MaskValue==NUMBERFORMAT)
                    {
                        rntb.Type = NumericType.Number;
                        rntb.NumberFormat.DecimalDigits = 2;
                        rntb.NumberFormat.DecimalSeparator = ".";
                        rntb.NumberFormat.GroupSeparator = ",";
                        rntb.NumberFormat.GroupSizes = 3;
                    }
                    else if(fld.MaskValue==PERCENTAGEFORMAT)
                    {
                        rntb.Type = NumericType.Percent;
                        rntb.NumberFormat.DecimalDigits = 2;
                        rntb.NumberFormat.DecimalSeparator = ".";
                    }
                    ctl = rntb;
                    if (rntb.Enabled)
                    {
                        rntb.Attributes.Add("needPostBack", fld.IsPostBack ? "1" : "0");
                        AttachScript(ctl, fld.IsPostBack);
                    }
                    break;
            }
            if (ctl != null)
            {
                ctl.ID = fld.FullPropertyName + "_" + fld.ParentId;
                ctl.EnableViewState = false;
            }
            return ctl;
        }


        #region obsolete
        //private static void AttachScript(Control ctl)
        //{
        //    if (ctl == null)
        //    {

        //        return;
        //    }
        //    if (ctl.GetType() == typeof(TextBox))
        //    {
        //        TextBox tctl = (TextBox)ctl;
        //        tctl.Attributes.Add("onfocus", "FocusGot(this);");
        //        tctl.Attributes.Add("onblur", "FocusLost(this);");
        //    }
        //    else if (ctl.GetType() == typeof(CheckBox))
        //    {
        //        ((CheckBox)ctl).Attributes.Add("onclick", "ClickCheckbox(this);");
        //    }
        //    else if (ctl.GetType() == typeof(RadComboBox))
        //    {
        //        RadComboBox rctl = (RadComboBox)ctl;
        //        rctl.OnClientSelectedIndexChanged = "RadComboIndexChanged(this);";
        //        rctl.OnClientFocus = "RadComboFocusGot(this);";
        //    }
        //    else if (ctl.GetType() == typeof(RadDateInput))
        //    {
        //        ((RadDateInput)ctl).ClientEvents.OnValueChanged = "RadInputDateChanged";
        //    }
        //    else if (ctl.GetType() == typeof(RadioButtonList))
        //    {
        //        RadioButtonList rbl = (RadioButtonList)ctl;
        //        rbl.Attributes.Add("onclick", "ClickRBList(this);");
        //    }
        //}

        //private Control GetControl(MortgageProfileField fld)
        //{
        //    Control ctl = null;
        //    switch (fld.ControlTypeId)
        //    {
        //        case MPFieldType.TextBox:
        //            TextBox tctl = new TextBox();
        //            tctl.TextMode = TextBoxMode.SingleLine;
        //            if (fld.FieldValue != null)
        //            {
        //                if (fld.FieldValue is decimal)
        //                {
        //                    tctl.Text = ((decimal)fld.FieldValue).ToString("0.00");
        //                }
        //                else
        //                {
        //                    tctl.Text = fld.FieldValue.ToString();
        //                }
        //            }
        //            tctl.Enabled = !fld.ReadOnly;
        //            tctl.Width = Unit.Pixel(TEXTBOXWIDTH);
        //            ctl = tctl;
        //            if (tctl.Enabled)
        //            {
        //                if (fld.IsPostBack)
        //                {
        //                    tctl.AutoPostBack = true;
        //                }
        //                else
        //                {
        //                    AttachScript(ctl);
        //                }
        //            }
        //            break;
        //        case MPFieldType.TextArea:
        //            TextBox mtctl = new TextBox();
        //            mtctl.TextMode = TextBoxMode.MultiLine;
        //            if (fld.FieldValue != null) mtctl.Text = fld.FieldValue.ToString();
        //            mtctl.Enabled = !fld.ReadOnly;
        //            mtctl.Width = Unit.Pixel(TEXTBOXWIDTH);
        //            ctl = mtctl;
        //            if (mtctl.Enabled)
        //            {
        //                if (fld.IsPostBack)
        //                {
        //                    mtctl.AutoPostBack = true;
        //                }
        //                else
        //                {
        //                    AttachScript(ctl);
        //                }
        //            }
        //            break;
        //        case MPFieldType.DropDownList:
        //            RadComboBox dctl = new RadComboBox();
        //            BindDropDown(dctl, fld.TableName, fld.FieldName);
        //            if (fld.FieldValue != null) dctl.SelectedValue = fld.FieldValue.ToString();
        //            dctl.Enabled = !fld.ReadOnly;
        //            dctl.Skin = "WindowsXP";
        //            dctl.Width = Unit.Pixel(radComboWidth);
        //            dctl.Height = Unit.Pixel(RADCOMBOHEIGHT);
        //            ctl = dctl;
        //            if (dctl.Enabled)
        //            {
        //                if (fld.IsPostBack)
        //                {
        //                    dctl.AutoPostBack = true;
        //                }
        //                else
        //                {
        //                    AttachScript(ctl);
        //                }
        //            }
        //            break;
        //        case MPFieldType.CheckBox:
        //            CheckBox cctl = new CheckBox();
        //            cctl.Checked = Convert.ToBoolean(fld.FieldValue);
        //            cctl.Enabled = !fld.ReadOnly;
        //            ctl = cctl;
        //            if (cctl.Enabled)
        //            {
        //                if (fld.IsPostBack)
        //                {
        //                    cctl.AutoPostBack = true;
        //                }
        //                else
        //                {
        //                    AttachScript(ctl);
        //                }
        //            }
        //            break;
        //        case MPFieldType.DateInput:
        //            RadDateInput rctl = new RadDateInput();
        //            rctl.Skin = "Windows";
        //            rctl.MinDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        //            rctl.Enabled = !fld.ReadOnly;
        //            if (fld.FieldValue != null)
        //            {
        //                DateTime dt = Convert.ToDateTime(fld.FieldValue);
        //                if (dt == System.Data.SqlTypes.SqlDateTime.MinValue.Value)
        //                {
        //                    rctl.SelectedDate = null;
        //                }
        //                else
        //                {
        //                    rctl.SelectedDate = DateTime.Parse(fld.FieldValue.ToString());// Convert.ToDateTime(fld.FieldValue);
        //                }
        //            }
        //            else
        //            {
        //                rctl.SelectedDate = rctl.MinDate;
        //            }
        //            rctl.Width = Unit.Pixel(155);
        //            ctl = rctl;
        //            if (rctl.Enabled)
        //            {
        //                if (fld.IsPostBack)
        //                {
        //                    rctl.AutoPostBack = true;
        //                }
        //                else
        //                {
        //                    AttachScript(ctl);
        //                }
        //            }
        //            break;
        //        case MPFieldType.Label:
        //            Label lctl = new Label();
        //            if (fld.FieldValue is decimal)
        //            {
        //                lctl.Text = ((decimal)fld.FieldValue).ToString("C");
        //            }
        //            else
        //            {
        //                lctl.Text = fld.FieldValue.ToString();
        //            }
        //            ctl = lctl;
        //            break;
        //        case MPFieldType.RadioButtonList:
        //            RadioButtonList rbl = new RadioButtonList();
        //            rbl.RepeatLayout = RepeatLayout.Table;
        //            rbl.RepeatDirection = RepeatDirection.Horizontal;
        //            BindRadioButtonList(rbl, fld.TableName, fld.FieldName, fld.FieldValue.ToString());
        //            ctl = rbl;
        //            if (rbl.Enabled)
        //            {
        //                if (fld.IsPostBack)
        //                {
        //                    rbl.AutoPostBack = true;
        //                }
        //                else
        //                {
        //                    AttachScript(ctl);
        //                }
        //            }
        //            break;

        //    }
        //    if (ctl != null)
        //    {
        //        ctl.ID = fld.FullPropertyName + "_" + fld.ParentId;
        //        ctl.EnableViewState = false;
        //    }
        //    return ctl;
        //}

        //public void BuildControls(ArrayList objects,int columns)
        //{
        //    ph.Controls.Add(new LiteralControl("<fieldset id='fieldset1'>"));
        //    HtmlTable table = new HtmlTable();
        //    table.Width = "100%";
        //    table.CellPadding = 0;
        //    table.CellSpacing = 0;
        //    int cellWidth = 50/columns;

        //        for (int i = 0; i < objects.Count; i++)
        //        {
        //            HtmlTableRow row = new HtmlTableRow();
        //            for (int j = 0; j < columns; j++)
        //            {
        //                MortgageProfileField fld = (MortgageProfileField)objects[i];

        //                HtmlTableCell cell1 = new HtmlTableCell();
        //                cell1.Width = cellWidth + "%";
        //                HtmlTableCell cell2 = new HtmlTableCell();
        //                cell2.Width = cellWidth + "%";
        //                Label lbl = new Label();
        //                lbl.Text = fld.Decsription;
        //                cell1.Controls.Add(lbl);
        //                switch (fld.ControlTypeId)
        //                {
        //                    case MPFieldType.TextBox:
        //                        TextBox tb = new TextBox();
        //                        tb.ID = fld.PropertyName;
        //                        tb.TextMode = TextBoxMode.SingleLine;
        //                        if (fld.FieldValue != null) tb.Text = fld.FieldValue.ToString();
        //                        tb.Enabled = !fld.ReadOnly;
        //                        cell2.Controls.Add(tb);
        //                        break;
        //                    case MPFieldType.TextArea:
        //                        TextBox ta = new TextBox();
        //                        ta.ID = fld.PropertyName;
        //                        ta.TextMode = TextBoxMode.MultiLine;
        //                        ta.Columns = 1;
        //                        ta.Width = Unit.Pixel(200);
        //                        if (fld.FieldValue != null) ta.Text = fld.FieldValue.ToString();
        //                        ta.Enabled = !fld.ReadOnly;
        //                        cell2.Controls.Add(ta);
        //                        break;
        //                    case MPFieldType.DropDownList:
        //                        RadComboBox ddl = new RadComboBox();
        //                        ddl.ID = fld.PropertyName;
        //                        ddl.Skin = "WindowsXP";
        //                        BindDropDown(ddl, fld.TableName, fld.FieldName);
        //                        if (fld.FieldValue != null) ddl.SelectedValue = fld.FieldValue.ToString();
        //                        ddl.Enabled = !fld.ReadOnly;
        //                        cell2.Controls.Add(ddl);
        //                        break;
        //                    //case MPFieldType.DropDownList:
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = fld.PropertyName;
        //                    //    BindDroDown(ddl, fld.TableName, fld.FieldName);
        //                    //    if (fld.FieldValue != null) ddl.SelectedValue = fld.FieldValue.ToString();
        //                    //    ddl.Enabled = !fld.ReadOnly;
        //                    //    cell2.Controls.Add(ddl);
        //                    //    break;
        //                    case MPFieldType.CheckBox:
        //                        CheckBox chk = new CheckBox();
        //                        chk.ID = fld.PropertyName;
        //                        //chk.Text = fld.Decsription;
        //                        chk.Checked = Convert.ToBoolean(fld.FieldValue);
        //                        chk.Enabled = !fld.ReadOnly;
        //                        cell2.Controls.Add(chk);
        //                        break;
        //                    case MPFieldType.DateInput:
        //                        RadDateInput di = new RadDateInput();
        //                        di.ID = fld.PropertyName;
        //                        di.Skin = "Windows";
        //                        //di.Width = Unit.Pixel(145);
        //                        if (fld.FieldValue != null) di.SelectedDate = Convert.ToDateTime(fld.FieldValue);
        //                        di.Enabled = !fld.ReadOnly;
        //                        cell2.Controls.Add(di);
        //                        break;
        //                }
        //                row.Controls.Add(cell1);
        //                row.Controls.Add(cell2);
        //                i++;
        //            }
        //            table.Controls.Add(row);
        //        }
        //        //foreach (MortgageProfileField fld in objects)
        //        //{
        //        //    HtmlTableRow row = new HtmlTableRow();
        //        //    HtmlTableCell cell1 = new HtmlTableCell();
        //        //    HtmlTableCell cell2 = new HtmlTableCell();

        //        //    Label lbl = new Label();
        //        //    lbl.Text = fld.Decsription;
        //        //    cell1.Controls.Add(lbl);

        //        //    switch (fld.ControlTypeId)
        //        //    {
        //        //        case MPFieldType.TextBox:
        //        //            TextBox tb = new TextBox();
        //        //            tb.ID = fld.PropertyName;
        //        //            tb.TextMode = TextBoxMode.SingleLine;
        //        //            if (fld.FieldValue != null) tb.Text = fld.FieldValue.ToString();
        //        //            tb.Enabled = !fld.ReadOnly;
        //        //            cell2.Controls.Add(tb);
        //        //            break;
        //        //        case MPFieldType.TextArea:
        //        //            TextBox ta = new TextBox();
        //        //            ta.ID = fld.PropertyName;
        //        //            ta.TextMode = TextBoxMode.MultiLine;
        //        //            ta.Columns = 1;
        //        //            ta.Width = Unit.Pixel(200);
        //        //            if (fld.FieldValue != null) ta.Text = fld.FieldValue.ToString();
        //        //            ta.Enabled = !fld.ReadOnly;
        //        //            cell2.Controls.Add(ta);
        //        //            break;
        //        //        case MPFieldType.DropDownList:
        //        //            DropDownList ddl = new DropDownList();
        //        //            ddl.ID = fld.PropertyName;
        //        //            BindDroDown(ddl, fld.TableName, fld.FieldName);
        //        //            if (fld.FieldValue != null) ddl.SelectedValue = fld.FieldValue.ToString();
        //        //            ddl.Enabled = !fld.ReadOnly;
        //        //            cell2.Controls.Add(ddl);
        //        //            break;
        //        //        case MPFieldType.CheckBox:
        //        //            CheckBox chk = new CheckBox();
        //        //            chk.ID = fld.PropertyName;
        //        //            //chk.Text = fld.Decsription;
        //        //            chk.Checked = Convert.ToBoolean(fld.FieldValue);
        //        //            chk.Enabled = !fld.ReadOnly;
        //        //            cell2.Controls.Add(chk);
        //        //            break;
        //        //        case MPFieldType.DateInput:
        //        //            RadDateInput di = new RadDateInput();
        //        //            di.ID = fld.PropertyName;
        //        //            if (fld.FieldValue != null) di.SelectedDate = Convert.ToDateTime(fld.FieldValue);
        //        //            di.Enabled = !fld.ReadOnly;
        //        //            cell2.Controls.Add(di);
        //        //            break;
        //        //    }
        //        //    row.Controls.Add(cell1);
        //        //    row.Controls.Add(cell2);
        //        //    table.Controls.Add(row);
        //        //}
        //    ph.Controls.Add(table);
        //    ph.Controls.Add(new LiteralControl("</fieldset>"));

        //    //add space under content of control to show content over tabs when scroller appears
        //    ph.Controls.Add(new LiteralControl("<div style='height:30px;'>&nbsp;</div>"));
        //}
        #endregion
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}