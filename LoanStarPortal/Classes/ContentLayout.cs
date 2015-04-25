using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Telerik.WebControls;

namespace LoanStar.Common
{
    public class ContentLayout : AppControl
    {
        #region constants

        #endregion

        #region fields

        #endregion

        private ContentLayout()
        {
        }


    }
    public class PseudoTab : AppControl
    {
        #region constants
        private const string IDFIELDNAME = "id";
        private const string HEADERFIELDNAME = "header";
        private const string COLUMNSFIELDNAME = "columns";
        private const string LABELCSSFIELDNAME = "labelcss";
        private const string CONTROLCSSFIELDNAME = "controlcss";
        private const string TITLELABEL = "lblTitle";
        private const string PSEUDOTABGROUPFILTER = "pseudotabid={0}";
        private const string PSEUDOTABID = "ps_{0}";
        private const int DEFAULTCOLUMNS = 2;
        #endregion

        #region fields
        private readonly int id;
        private readonly Control container;
        private readonly string header;
        private readonly int columns = DEFAULTCOLUMNS;
        private readonly string labelCss = String.Empty;
        private readonly string controlCss = String.Empty;
        private readonly DataTabControl parentDataTab;
        private short tabIndex;
        private int emptyFields = 0;
        private Hashtable modifiedFields = null;
        #endregion

        #region properties
        public Hashtable ModifiedFields
        {
            get { return modifiedFields; }
        }
        public int EmptyFields
        {
            get { return emptyFields; }
        }
        public short TabIndex
        {
            get { return tabIndex; }
        }
        public DataTabControl DataTab
        {
            get { return parentDataTab; }
        }
        public BaseObject ObjectToCompare
        {
            get { return parentDataTab.ObjectToCompare; }
        }
        public string LabelCss
        {
            get { return labelCss; }
        }
        public string ControlCss
        {
            get { return controlCss; }
        }
        #endregion

        #region constructor
        public PseudoTab(DataRow dr, Control _container, DataTabControl parentControl, short startTabIndex)
        {
            tabIndex = startTabIndex;
            container = _container;
            parentDataTab = parentControl;
            id = int.Parse(dr[IDFIELDNAME].ToString());
            header = dr[HEADERFIELDNAME].ToString();
            columns = int.Parse(dr[COLUMNSFIELDNAME].ToString());
            if((columns > DEFAULTCOLUMNS) ||(columns<1))
            {
                columns = DEFAULTCOLUMNS;
            }
            labelCss = dr[LABELCSSFIELDNAME].ToString();
            controlCss = dr[CONTROLCSSFIELDNAME].ToString();
            Label lbl = (Label)container.FindControl(TITLELABEL);
            if(lbl!=null)
            {
                lbl.Text = header;
            }
            container.ID = String.Format(PSEUDOTABID, id);
        }
        #endregion
        #region methods
        public bool Build()
        {
            DataRow[] rows = parentDataTab.DtGroups.Select(String.Format(PSEUDOTABGROUPFILTER,id));
            HtmlTable table = GetTable();
            bool isVisible = true;
            for(int i=0;i<rows.Length;i++)
            {
                PseudoTabGroup pstg = new PseudoTabGroup(this,rows[i],columns,tabIndex,parentDataTab);
                ArrayList groupRows = pstg.Build();
                if ((groupRows != null) && (groupRows.Count > 0))
                {
                    if(pstg.ModifiedFields!=null&&pstg.ModifiedFields.Count>0)
                    {
                        AddModifiedField(pstg.ModifiedFields);
                    }
                    emptyFields += pstg.EmptyFields;
                    for (int j = 0; j < groupRows.Count; j++)
                    {
                        table.Rows.Add((HtmlTableRow) groupRows[j]);
                    }
                    tabIndex = pstg.TabIndex;
                    if (pstg.IsUdfControl)
                    {
                        parentDataTab.UserControlAdded(pstg.UDFControl);
                        isVisible = pstg.UDFControl.Visible;
                    }
                }
            }
            bool res = (table.Rows.Count > 0)&&isVisible;
            if(res)
            {
                container.Controls.Add(table);
            }
            return res;
        }
        private void AddModifiedField(IDictionary tbl)
        {
            if (modifiedFields==null)
            {
                modifiedFields = new Hashtable();
            }
            foreach(Object item in tbl.Keys)
            {
                if(!modifiedFields.ContainsKey(item))
                {
                    modifiedFields.Add(item,tbl[item]);
                }
            }
        }

        private static HtmlTable GetTable()
        {
            HtmlTable table = new HtmlTable();
            table.Attributes.Add("border", "0");
            table.Attributes.Add("cellspacing", "0");
            table.Attributes.Add("cellpadding", "0");
            table.Attributes.Add("align", "center");
            table.Attributes.Add("style", "width:100%;padding-bottom:1px");
            return table;
        }
        #endregion
    }
    public class PseudoTabGroup : AppControl
    {
        #region constants
        private const string GROUPFILTER = "pseudotabgroupid={0}";
        private const string IDFIELDNAME = "id";
        private const string HEADERFIELDNAME = "header";
        private const string HEADERCSSFIELDNAME = "headercss";
        private const string TRSTYLEFIELDNAME = "trstyle";
        private const string TRHEADERSTYLEFIELDNAME = "trheaderstyle";
        private const string CONTROLNAMEFIELDNAME = "controlname";
        private const int DEFAULTCOLUMNS = 2;
        private const string CLASSATTRIBUTE = "class";
        private const string REQUIREDFIELDCSS = "reqfield";
        private const string STYLEATTRIBUTE = "style";
        private const string HRSTYLE = "color:Black;height:1px";
        private const string DEFAULTHEADERTRSTYLE = "height:25px;padding-top:3px;padding-top:3px";
        #endregion

        #region fields
        private readonly int id;
        private readonly string header;
        private readonly PseudoTab parent;
        private readonly int columns;
        private readonly string headerCss;
        private readonly string trStyle;
        private readonly string trHeaderStyle;
        private ArrayList content;
        private readonly string[] labelCss;
        private readonly string[] controlCss;
        private short tabIndex;
        private readonly string controlName;
        private bool isUdfControl = false;
        private Control udfControl;
        private readonly DataTabControl parentDataTab;
        private int emptyFields = 0;
        private Hashtable modifiedFields = null;
        #endregion

        #region properties
        public int EmptyFields
        {
            get { return emptyFields; }
        }
        public short TabIndex
        {
            get { return tabIndex; }
        }
        public bool IsUdfControl
        {
            get { return isUdfControl; }
        }
        public Control UDFControl
        {
            get { return udfControl; }
        }
        public Hashtable ModifiedFields
        {
            get { return modifiedFields; }
        }
        #endregion

        #region constructor
        public PseudoTabGroup(PseudoTab _parent, DataRow dr, int _columns, short startTabIndex, DataTabControl parentTabControl)
        {
            tabIndex = startTabIndex;
            parent = _parent;
            columns = _columns;
            parentDataTab = parentTabControl;
            id = int.Parse(dr[IDFIELDNAME].ToString());
            header = dr[HEADERFIELDNAME].ToString();
            headerCss = dr[HEADERCSSFIELDNAME].ToString();
            trStyle = dr[TRSTYLEFIELDNAME].ToString();
            trHeaderStyle = dr[TRHEADERSTYLEFIELDNAME].ToString();
            controlName = dr[CONTROLNAMEFIELDNAME].ToString();
            labelCss = GetCssArray(parent.LabelCss);
            controlCss = GetCssArray(parent.ControlCss);
        }
        #endregion

        #region methods
        public ArrayList Build()
        {
            if (String.IsNullOrEmpty(controlName))
            {
                return BuildFieldsGroup();
            }
            else
            {
                return BuildControlGroup();
            }
        }
        private ArrayList BuildControlGroup()
        {
            udfControl = parentDataTab.CurrentPage.LoadControl(Constants.FECONTROLSLOCATION + controlName);
            if (udfControl == null)
            {
                return null;
            }
            ArrayList groupRows = new ArrayList();
            if (!String.IsNullOrEmpty(header))
            {
                groupRows.Add(GetHeaderRow());
            }
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell td = new HtmlTableCell();
            td.ColSpan = columns*2;
            td.Controls.Add(udfControl);
            tr.Cells.Add(td);
            groupRows.Add(tr);
            isUdfControl = true;
            return groupRows;
        }

        private ArrayList BuildFieldsGroup()
        {
            MortgageProfile mp = parentDataTab.Mp;
            BaseObject objectToCompare = parent.ObjectToCompare;
            DataRow[] rows = parentDataTab.DtPrimitive.Select(String.Format(GROUPFILTER, id));
            content = new ArrayList();
            for (int i = 0; i < rows.Length; i++)
            {
                ContentPrimitive primitive = new ContentPrimitive(rows[i], tabIndex, parentDataTab);
                if (primitive.Build(mp, objectToCompare))
                {
                    if(primitive.IsRequiredField)
                    {
                        emptyFields++;
                    }
                    if(primitive.IsModifiedByRule)
                    {
                        if(modifiedFields==null)
                        {
                            modifiedFields = new Hashtable();
                        }
                        modifiedFields.Add(primitive.FullPropertyName,primitive);
                    }
                    content.Add(primitive);
                    tabIndex++;
                }
            }
            if (content.Count == 0)
            {
                return null;
            }
            ArrayList groupRows = new ArrayList();
            if (!String.IsNullOrEmpty(header))
            {
                groupRows.Add(GetHeaderRow());
            }
            string cssLabelLeft = GetCss(labelCss, 0);
            string cssLabelRight = GetCss(labelCss, 1);
            string cssHalfLeft = GetCss(labelCss, 2);
            string cssHalfRight = GetCss(labelCss, 3);

            string cssControlLeft = GetCss(controlCss, 0);
            string cssControlRight = GetCss(controlCss, 1);
            string cssFullRow = GetCss(controlCss, 2);
            string cssHalfRow = GetCss(controlCss, 3);

            if (columns == 1)
            {
                for (int i = 0; i < content.Count; i++)
                {
                    ContentPrimitive cp = (ContentPrimitive)content[i];
                    ArrayList cells = GetCells(cp, cssLabelLeft, cssControlLeft, cssHalfLeft, cssHalfRow, cssFullRow);
                    groupRows.Add(GetOneColumnTr(cells));
                }
            }
            else if (columns == DEFAULTCOLUMNS)
            {
                ArrayList left = new ArrayList();
                ArrayList right = new ArrayList();
                int cnt = content.Count;
                int nRows = (cnt + columns - 1)/columns;
                for (int i = 0; i < content.Count;i++ )
                {
                    int rowsLeft = nRows - left.Count;
                    ContentPrimitive cp = (ContentPrimitive) content[i];
                    if(rowsLeft>0)
                    {
                        AddCells(left, cp , cssLabelLeft, cssControlLeft, cssHalfLeft, cssHalfRow, cssFullRow);
                        if (cp.IsFullRow)
                        {
                            int nn = left.Count - right.Count;
                            AddEmpty(right, left.Count-1, cssLabelRight, cssControlRight,true);
                            cnt+=nn;
                            nRows = (cnt + columns - 1) / columns;
                        }
                    }
                    else
                    {
                        if(cp.IsFullRow)
                        {
                            AddCells(left, cp, cssLabelLeft, cssControlLeft, cssHalfLeft, cssHalfRow, cssFullRow);
                            int nn = left.Count - right.Count;
                            AddEmpty(right, left.Count - 1, cssLabelRight, cssControlRight,true);
                            cnt += nn;
                            nRows = (cnt + columns - 1) / columns;
                        }
                        else
                        {
                            AddCells(right,cp,cssLabelRight,cssControlRight,cssHalfRight,cssHalfRow,cssFullRow);
                        }
                    }
                }
                AddEmpty(right, left.Count, cssLabelRight, cssControlRight,false);
                for(int i=0;i<left.Count;i++)
                {
                    groupRows.Add(GetContentTr(left, right, i));
                }
            }
            return groupRows;
        }
        private static void AddEmpty(IList contentColumn, int n, string cssLabel, string cssControl, bool addNull)
        {
            int nn = n - contentColumn.Count;
            for (int i = 0; i < nn; i++)
            {
                contentColumn.Add(GetEmpty(cssLabel, cssControl));
            }
            if(addNull)
            {
                contentColumn.Add(null);
            }
        }
        private HtmlTableRow GetOneColumnTr(IList cells)
        {
            HtmlTableRow tr = new HtmlTableRow();
            if (!String.IsNullOrEmpty(trStyle))
            {
                tr.Attributes.Add(STYLEATTRIBUTE, trStyle);
            }
            AddCellsToTr(tr, cells);
            return tr;
        }
        private HtmlTableRow GetContentTr(IList left, IList right, int index)
        {
            HtmlTableRow tr = new HtmlTableRow();
            if(!String.IsNullOrEmpty(trStyle))
            {
                tr.Attributes.Add(STYLEATTRIBUTE,trStyle);
            }
            if( index<left.Count )
            {
                AddCellsToTr(tr, left[index] as ArrayList);
            }
            if (index < right.Count)
            {
                AddCellsToTr(tr, right[index] as ArrayList);
            }
            return tr;
        }
        private static void AddCellsToTr(HtmlTableRow tr , IList cells)
        {
            if(cells!=null)
            {
                for(int i=0;i<cells.Count;i++)
                {
                    tr.Cells.Add((HtmlTableCell)cells[i]);
                }
            }
        }
        private static ArrayList GetEmpty(string cssLabel,string cssControl)
        {
            ArrayList res = new ArrayList();
            HtmlTableCell tdl = new HtmlTableCell();
            if (!String.IsNullOrEmpty(cssLabel))
            {
                tdl.Attributes.Add(CLASSATTRIBUTE, cssLabel);
            }
            tdl.InnerHtml = "&nbsp;";
            res.Add(tdl);
            HtmlTableCell tdc = new HtmlTableCell();
            if (!String.IsNullOrEmpty(cssControl))
            {
                tdc.Attributes.Add(CLASSATTRIBUTE, cssControl);
            }
            tdc.InnerHtml = "&nbsp;";
            res.Add(tdc);
            return res;
        }
        private void AddCells(IList contentColumn, ContentPrimitive cp, string cssLabel, string cssControl, string cssHalf, string cssFullControl, string cssFullRow)
        {
            contentColumn.Add(GetCells(cp,cssLabel,cssControl,cssHalf,cssFullControl,cssFullRow));
        }
        private ArrayList GetCells(ContentPrimitive cp, string cssLabel, string cssControl,string cssHalf, string cssFullControl, string cssFullRow)
        {
            ArrayList res = new ArrayList();
            Control cLabel = GetLabel(cp);
            Control ctl = cp.ContentControl;
            Control err = cp.ErrControl;
            if(cp.IsLabelTop)
            {
                HtmlTableCell td = new HtmlTableCell();
                td.Controls.Add(cLabel);
                td.Controls.Add(new LiteralControl("<br />"));
               
                td.Controls.Add(ctl);
                td.Controls.Add(err);
                string css;
                if(cp.IsFullRow)
                {
                    td.ColSpan = columns*2;
                    css = cssFullRow;
                }
                else
                {
                    td.ColSpan = columns;
                    css = cssHalf;
                }
                if (!String.IsNullOrEmpty(css))
                {
                    td.Attributes.Add(CLASSATTRIBUTE,css);
                }
                res.Add(td);
            }
            else
            {
                HtmlTableCell tdl = new HtmlTableCell();
                tdl.Controls.Add(cLabel);
                if (!String.IsNullOrEmpty(cssLabel))
                {
                    tdl.Attributes.Add(CLASSATTRIBUTE,cssLabel);
                }
                res.Add(tdl);
                HtmlTableCell tdc = new HtmlTableCell();
                tdc.Controls.Add(ctl);
                tdc.Controls.Add(err);
                string css;
                if(cp.IsFullRow)
                {
                    tdc.ColSpan = columns*2 - 1;
                    css = cssFullControl;
                }
                else
                {
                    css = cssControl;
                }
                if (!String.IsNullOrEmpty(css))
                {
                    tdc.Attributes.Add(CLASSATTRIBUTE,cssControl);
                }
                res.Add(tdc);
            }
            return res;
        }
        private Control GetLabel(ContentPrimitive cp)
        {
            Control ctl;
            Literal lbl = new Literal();
            lbl.Text = cp.Label;
            if(cp.HasReqAttribute)
            {
                int index = parentDataTab.AddTabLevel2RequiredField(!cp.IsRequiredField);
                HtmlGenericControl span = new HtmlGenericControl("span");
                if (cp.IsRequiredField)
                {
                    span.Attributes.Add(CLASSATTRIBUTE, REQUIREDFIELDCSS);
                }
                span.Attributes.Add("reqindex", index.ToString());
                span.ID = cp.ControlId + "_l";
                span.Controls.Add(lbl);
                ctl = span;
            }
            else
            {
                if (!String.IsNullOrEmpty(cp.LabelCss))
                {
                    HtmlGenericControl span = new HtmlGenericControl("span");
                    span.Attributes.Add(CLASSATTRIBUTE, cp.LabelCss);
                    span.Controls.Add(lbl);
                    ctl = span;
                }
                else
                {
                    ctl = lbl;
                }
            }
            return ctl;
        }
        private static string[] GetCssArray(string css)
        {
            if(String.IsNullOrEmpty(css))
            {
                return null;
            }
            return css.Split(';');
        }
        private static string GetCss(string[] arr, int index)
        {
            string res = String.Empty;
            if(arr!=null)
            {
                if (index < arr.Length)
                {
                    res = arr[index];
                }
            }
            return res;
        }
        private HtmlTableRow GetHeaderRow()
        {
            HtmlTableRow tr = new HtmlTableRow();
            string style = DEFAULTHEADERTRSTYLE;
            if(!String.IsNullOrEmpty(trHeaderStyle))
            {
                style = trHeaderStyle;
            }
            tr.Attributes.Add(STYLEATTRIBUTE,style);
            HtmlTableCell td = new HtmlTableCell();
            HtmlTable table = new HtmlTable();
            table.Attributes.Add("border", "0");
            table.Attributes.Add("cellspacing", "0");
            table.Attributes.Add("cellpadding", "0");
            table.Attributes.Add("style", "width:100%");
            bool isMultipleRows = header.IndexOf("<br/>")>=0;
            HtmlTableRow tr1 = new HtmlTableRow();
            HtmlTableCell td1 = new HtmlTableCell();
            if(!isMultipleRows)
            {
                td1.Attributes.Add("nowrap", "nowrap");
            }

            Literal lbl = new Literal();
            lbl.Text = header;
            if (!String.IsNullOrEmpty(headerCss))
            {
                HtmlGenericControl span = new HtmlGenericControl("span");
                span.Attributes.Add(CLASSATTRIBUTE, headerCss);
                span.Controls.Add(lbl);
                td1.Controls.Add(span);
            }
            else
            {
                td1.Controls.Add(lbl);
            }
            tr1.Cells.Add(td1);
            if(isMultipleRows)
            {
                table.Rows.Add(tr1);
                HtmlTableRow tr2 = new HtmlTableRow();
                HtmlTableCell td2 = new HtmlTableCell();
                td2.Attributes.Add("style", "width:100%;padding-left:5px");
                HtmlGenericControl hr = new HtmlGenericControl("hr");
                hr.Attributes.Add(STYLEATTRIBUTE, HRSTYLE);
                td2.Controls.Add(hr);
                tr2.Cells.Add(td2);
                table.Rows.Add(tr2);
            }
            else
            {
                HtmlTableCell td2 = new HtmlTableCell();
                td2.Attributes.Add("style", "width:100%;padding-left:5px");
                HtmlGenericControl hr = new HtmlGenericControl("hr");
                hr.Attributes.Add(STYLEATTRIBUTE, HRSTYLE);
                td2.Controls.Add(hr);
                tr1.Cells.Add(td2);
                table.Rows.Add(tr1);
            }
            //HtmlTableCell td2 = new HtmlTableCell();
            //td2.Attributes.Add("style","width:100%;padding-left:5px");
            //HtmlGenericControl hr = new HtmlGenericControl("hr");
            //hr.Attributes.Add(STYLEATTRIBUTE,HRSTYLE); 
            //td2.Controls.Add(hr);
            //tr1.Cells.Add(td2);
//            table.Rows.Add(tr1);
            td.Controls.Add(table);
            td.ColSpan = columns*2;
            tr.Cells.Add(td);
            return tr;
        }
        //private static ArrayList SplitStr(string data,string separator)
        //{
        //    ArrayList res = new ArrayList();
        //    string tmp = data;
        //    while (tmp.Length>0)
        //    {
        //        int i = tmp.IndexOf(separator);
        //        if(i>=0)
        //        {
        //            res.Add(tmp.Substring(0, i));
        //            tmp = tmp.Substring(i + separator.Length);
        //        }
        //        else
        //        {
        //            res.Add(tmp);
        //            tmp = String.Empty;
        //        }
        //    }
        //    return res;
        //}

        //private HtmlTableRow GetHeaderTr(string txt, bool isLast)
        //{
        //    HtmlTableRow tr = new HtmlTableRow();
        //    HtmlTableCell td1 = new HtmlTableCell();
        //    td1.Attributes.Add("nowrap", "nowrap");
        //    Literal lbl = new Literal();
        //    lbl.Text = txt;
        //    if (!String.IsNullOrEmpty(headerCss))
        //    {
        //        HtmlGenericControl span = new HtmlGenericControl("span");
        //        span.Attributes.Add(CLASSATTRIBUTE, headerCss);
        //        span.Controls.Add(lbl);
        //        td1.Controls.Add(span);
        //    }
        //    else
        //    {
        //        td1.Controls.Add(lbl);
        //    }
        //    tr.Cells.Add(td1);
        //    HtmlTableCell td2 = new HtmlTableCell();
        //    td2.Attributes.Add("style", "width:100%;padding-left:5px");
        //    if (isLast)
        //    {
        //        HtmlGenericControl hr = new HtmlGenericControl("hr");
        //        hr.Attributes.Add(STYLEATTRIBUTE, HRSTYLE);
        //        td2.Controls.Add(hr);
        //    }
        //    else
        //    {
        //        td2.InnerHtml = "&nbsp;";
        //    }
        //    tr.Cells.Add(td2);

        //    return tr;
        //}

        #endregion
    }
    public class ContentPrimitive
    {
        #region enumaration
        public enum ControlType
        {
            Undefined = 0,
            TextBox = 1,
            TextArea = 2,
            DropDownList = 3,
            CheckBox = 4,
            DateInput = 5,
            Label = 6,
            RadioButtonList = 7,
            MaskedInput = 8,
            MoneyInput = 9,
            YesNo = 10,
            HttpLink = 11
        }
        public enum LabelLocation
        {
            Left = 0,
            Top = 1
        }
        #endregion

        #region constants
        
        private const string FIELDFILTER = "id={0}";
        private const string CONTROLTYPEIDFIELDNAME = "controltypeid";
        private const string CONTROLWIDTHFIELDNAME = "controlwidth";
        private const string CONTROLHEIGHTFIELDNAME = "controlheight";
        private const string CONTROLPARAMSFIELDNAME = "params";
        private const string LABELCSSFIELDNAME = "labelcss";
        private const string FIELDIDFIELDNAME = "mpfid";
        private const string LABELALIGN = "labelalign";
        private const string FULLROW = "fullrow";
        #endregion

        #region fields
        private readonly int controlTypeId;
        private readonly string controlWidth;
        private readonly string controlHeight;
        private readonly string controlParams;
        private readonly string labelCss;
        private readonly int fieldId;
        private readonly MortgageProfileFieldInfo mpfi;
        private bool isEditable = false;
        private bool isModifiedByRule = false;
        private Object dataValue=null;
        private int parentId=-1;
        private readonly Hashtable extraParams = new Hashtable();
        private bool isPostBack = false;
        private string filterValue;
        private readonly DataTabControl parentDataTab;
        private Control contentControl;
        private HtmlGenericControl errControl;
        private readonly short tabIndex;
        private bool isRequiredField = false;
        private bool hasReqAttribute = false;
        private MortgageProfile mp;
        #endregion

        #region properties
        public string ControlWidth
        {
            get
            {
                return controlWidth;
            }
        }
        public string ControlHeight
        {
            get { return controlHeight; }
        }
        public MortgageProfileFieldInfo Mpfi
        {
            get { return mpfi; }
        }
        public int ParentId
        {
            get { return parentId; }
        }
        public int ControlTypeId
        {
            get { return controlTypeId; }
        }
        public Object DataValue
        {
            get { return dataValue; }
        }
        public bool IsEditable
        {
            get { return isEditable; }
        }
        public bool IsModifiedByRule
        {
            get { return isModifiedByRule; }
        }

        public short TabIndex
        {
            get { return tabIndex; }
        }
        public string FullPropertyName
        {
            get
            {
                string res = String.Empty;
                if(fieldId>0)
                {
                    res = mpfi.FullPropertyName;
                }
                return res;
            }
        }
        public string LabelCss
        {
            get { return labelCss; }
        }
        public bool IsPostBack
        {
            get { return isPostBack; }
        }
        public bool IsLabelTop
        {
            get
            {
                bool res = false;
                if(extraParams.ContainsKey(LABELALIGN))
                {
                    try
                    {
                        res = int.Parse(extraParams[LABELALIGN].ToString()) == (int) LabelLocation.Top;
                    }
                    catch
                    {
                    }
                }
                return res;
            }
        }
        public bool IsRequiredField
        {
            get { return isRequiredField; }
        }
        public bool HasReqAttribute
        {
            get { return hasReqAttribute; }
        }
        public string ControlId
        {
            get { return mpfi.FullPropertyName + "_" + parentId; }
        }
        public bool IsFullRow
        {
            get
            {
                bool res = false;
                if (extraParams.ContainsKey(FULLROW))
                {
                    res = true;
                }
                return res;
            }
        }
        public DataTabControl ParentDataTab
        {
            get { return parentDataTab; }
        }
        public string Label
        {
            get { return mpfi.Description; }
        }
        public Control ContentControl
        {
            get { return contentControl; }
        }
        public HtmlGenericControl ErrControl
        {
            get { return errControl;}
        }
        public string FilterValue
        {
            get { return filterValue; }
        }
        public MortgageProfile Mp
        {
            get { return mp; }
        }
        #endregion

        #region constructor
        public ContentPrimitive(DataRow dr, short currenttabIndex, DataTabControl _parentDataTab)
        {
            parentDataTab = _parentDataTab;
            tabIndex = currenttabIndex;
            controlTypeId = int.Parse(dr[CONTROLTYPEIDFIELDNAME].ToString());
            controlWidth = dr[CONTROLWIDTHFIELDNAME].ToString();
            controlHeight = dr[CONTROLHEIGHTFIELDNAME].ToString();
            controlParams = dr[CONTROLPARAMSFIELDNAME].ToString();
            labelCss = dr[LABELCSSFIELDNAME].ToString();
            fieldId = int.Parse(dr[FIELDIDFIELDNAME].ToString());
            mpfi = new MortgageProfileFieldInfo(GetFieldDataRow(parentDataTab.DtFields));
            ParseParams();
        }
        #endregion

        #region methods

        #region public
        public void OverrideValue(Control ctl)
        {
            ContentPrimitiveControl c = GetContentControl();
            if(c!=null)
            {
                c.OverrideValue(ctl,this);
            }
        }
        public bool Build(MortgageProfile _mp, BaseObject obj)
        {
            mp = _mp;
            if (String.IsNullOrEmpty(mpfi.FullPropertyName))
            {
                return false;
            }
            if (controlTypeId == (int)ControlType.Undefined)
            {
                return false;
            }
            if (!CheckVisibility(mp, obj))
            {
                return false;
            }
            isModifiedByRule = mp.CheckModifiedByRule(mpfi.FullPropertyName, obj);
            isEditable = mp.CheckReadOnly(mpfi.FullPropertyName, obj);            
            dataValue = mp.GetDataValue(mpfi.PropertyName, mpfi.ObjectName, obj, out parentId);
            isPostBack = mpfi.IsPostBack ? true : mp.CheckPostBackField(mpfi.FullPropertyName);
            filterValue = mp.GetFilterValue(mpfi, obj);
            if (controlTypeId != (int)ControlType.Label)
            {
                hasReqAttribute = mp.IsFieldRequired(mpfi.FullPropertyName);
                if (hasReqAttribute)
                {
                    if(dataValue==null)
                    {
                        isRequiredField = true;
                    }
                    else if(String.IsNullOrEmpty(dataValue.ToString()))
                    {
                        isRequiredField = true;
                    }
                    else
                    {
                        if ((mpfi.IsDictionaryField)&&(dataValue.ToString()=="0"))
                        {
                            isRequiredField = true;
                        }
                    }
                }
            }
            return BuildControl();
        }
        public string GetParam(string paramName)
        {
            string res = String.Empty;
            if((extraParams!=null)&&(extraParams.ContainsKey(paramName)))
            {
                res = extraParams[paramName].ToString();
            }
            return res;
        }

        #endregion

        #region private
        private DataRow GetFieldDataRow(DataTable dtFields)
        {
            DataRow[] filter = dtFields.Select(String.Format(FIELDFILTER, fieldId));
            if((filter!=null)&&(filter.Length==1))
            {
                return filter[0];
            }
            return null;
        }

        private void ParseParams()
        {
            if(!String.IsNullOrEmpty(controlParams))
            {
                string[] par = controlParams.Split(';');
                for(int i=0; i<par.Length;i++)
                {
                    string[] param = par[i].Split('=');
                    if(param.Length==2)
                    {
                        extraParams.Add(param[0].ToLower(),param[1].ToLower());
                    }
                }
            }
        }
        private bool CheckVisibility(MortgageProfile mp_, BaseObject obj)
        {
            if(AppSettings.ShowAll)
            {
                return true;
            }
            if (fieldId < 1)
            {
                return false;
            }
            return mp_.CheckFieldVisibility(mpfi.PropertyName, mpfi.ObjectName, obj);
        }
        private bool BuildControl()
        {
            contentControl = GetControl();
            if(contentControl!=null)
            {
                errControl = new HtmlGenericControl("span");
                errControl.Attributes.Add("style", "color:red");
                errControl.ID = mpfi.FullPropertyName + "_" + parentId + "_e";
            }
            return (contentControl!=null);
        }
        private ContentPrimitiveControl GetContentControl()
        {
            ContentPrimitiveControl ctl = null;
            switch (controlTypeId)
            {
                case (int)ControlType.TextBox:
                case (int)ControlType.TextArea:
                    ctl = new ContentPrimitiveControlTextBox(this);
                    break;
                case (int)ControlType.RadioButtonList:
                case (int)ControlType.YesNo:
                    ctl = new ContentPrimitiveControlRadioButtonList(this);
                    break;
                case (int)ControlType.DateInput:
                    ctl = new ContentPrimitiveControlDateInput(this);
                    break;
                case (int)ControlType.DropDownList:
                    ctl = new ContentPrimitiveControlDropDownList(this);
                    break;
                case (int)ControlType.MaskedInput:
                    ctl = new ContentPrimitiveControlMaskedInput(this);
                    break;
                case (int)ControlType.MoneyInput:
                    ctl = new ContentPrimitiveControlMoneyInput(this);
                    break;
                case (int)ControlType.Label:
                    ctl = new ContentPrimitiveControlLabel(this);
                    break;
                case (int)ControlType.HttpLink:
                    ctl = new ContentPrimitiveControlHttpLink(this);
                    break;
            }
            return ctl;
        }

        private Control GetControl()
        {
            ContentPrimitiveControl ctl = null;
            switch(controlTypeId)
            {
                case (int)ControlType.TextBox:
                case (int)ControlType.TextArea:
                    ctl = new ContentPrimitiveControlTextBox(this);
                    break;
                case (int)ControlType.RadioButtonList:
                case (int)ControlType.YesNo:
                    ctl = new ContentPrimitiveControlRadioButtonList(this);
                    break;
                case (int)ControlType.DateInput:
                    ctl = new ContentPrimitiveControlDateInput(this);
                    break;
                case (int)ControlType.DropDownList:
                    ctl = new ContentPrimitiveControlDropDownList(this);
                    break;
                case (int)ControlType.MaskedInput:
                    ctl = new ContentPrimitiveControlMaskedInput(this);
                    break;
                case (int)ControlType.MoneyInput:
                    ctl = new ContentPrimitiveControlMoneyInput(this);
                    break;
                case (int)ControlType.Label:
                    ctl = new ContentPrimitiveControlLabel(this);
                    break;
                case (int)ControlType.HttpLink:
                    ctl = new ContentPrimitiveControlHttpLink(this);
                    break;

            }
            if(ctl!=null)
            {
                ctl.BindData();
                ctl.AttachScript();
            }
            if(ctl!=null)
            {
                return ctl.Ctl;
            }
            return null;
        }
        #endregion

        #endregion
    }
    public class ContentPrimitiveControl : Control
    {
        protected const string ONFOCUS = "onfocus";
        protected const string ONBLUR = "onblur";
        protected const string ONCLICK = "onclick";
        protected const string ONCHANGE = "onchange";
        protected const string NEEDPOSTBACK = "needPostBack";
        protected ContentPrimitive parent;
        protected Control ctl;
        protected bool IsPercent = false;
        public Control Ctl
        {
            get { return ctl; }
        }
        protected virtual string DefaultWidth
        {
            get { return String.Empty; }
        }
        protected virtual string DefaultHeight
        {
            get { return String.Empty; }
        }
        public ContentPrimitiveControl(ContentPrimitive _parent)
        {
            parent = _parent;
        }
        public virtual void BindData()
        {
            if(ctl!=null)
            {
                ctl.ID = parent.FullPropertyName + "_" + parent.ParentId;
                ctl.EnableViewState = false;
            }
            SetStyle();
        }
        public virtual void AttachScript()
        {
        }
        protected virtual void SetStyle()
        {
        }
        protected virtual string GetHiddenValue(int len)
        {
            string s = "";
            s = s.PadRight(len, '*');
            return s;
//            return "".PadRight(len, '*');
        }

        protected virtual int GetWidth()
        {
            int res = -1;
            string width = parent.ControlWidth;
            if(String.IsNullOrEmpty(width))
            {
                width = DefaultWidth;
            }
            if(!String.IsNullOrEmpty(width))
            {
                width = CheckPercent(width);
                try
                {
                    res = int.Parse(width);
                }
                catch
                {
                }
            }
            return res;
        }
        private string CheckPercent(string w)
        {
            string res = w.ToLower();
            IsPercent = res[res.Length-1]=='%';
            if(IsPercent)
            {
                res = res.Replace("%", "");
            }
            return res;
        }

        protected virtual int GetHeigth()
        {
            int res = -1;
            string height = parent.ControlHeight;
            if (String.IsNullOrEmpty(height))
            {
                height = DefaultHeight;
            }
            if (!String.IsNullOrEmpty(height))
            {
                try
                {
                    res = int.Parse(height);
                }
                catch
                {
                }
            }
            return res;
        }
        public virtual void OverrideValue(Control ctl_, ContentPrimitive cp)
        {
        }
    }
    public class ContentPrimitiveControlTextBox : ContentPrimitiveControl
    {
        private const string FOCUSGOTJS = "FocusGot(this);";
        private const string FOCUSLOSTJS = "FocusLost(this);";
        private const string DEFAULTWIDTH = "150";
        private const string DEFAULTHEIGHT = "18";
        private const string TEXTAREAROWS = "rows";
        private const string MAXLENGTHPARAM = "maxlength";
        private const int DEFAULTROWS = 4;
        protected override string DefaultWidth
        {
            get
            {
                return DEFAULTWIDTH;
            }
        }
        protected override string DefaultHeight
        {
            get
            {
                return DEFAULTHEIGHT;
            }
        }
        public ContentPrimitiveControlTextBox(ContentPrimitive _parent)
            : base(_parent)
        {
            ctl = new TextBox();
            if (parent.ControlTypeId == (int)ContentPrimitive.ControlType.TextBox)
            {
                ((TextBox)ctl).TextMode = TextBoxMode.SingleLine;
            }
            else
            {
                ((TextBox)ctl).TextMode = TextBoxMode.MultiLine;
            }
//            ((TextBox)ctl).ReadOnly = !parent.IsEditable;
            ((TextBox)ctl).Enabled = parent.IsEditable;
            if(parent.IsEditable)
            {
                ((TextBox)ctl).Attributes.Add(NEEDPOSTBACK, parent.IsPostBack ? "1" : "0");
            }
        }
        public override void BindData()
        {
            base.BindData();
            if(parent.DataValue!=null)
            {
                ((TextBox) ctl).Text = parent.DataValue.ToString();
            }
            ((TextBox) ctl).TabIndex = parent.TabIndex;
        }
        public override void AttachScript()
        {
            if (parent.IsEditable)
            {
                base.AttachScript();
                ((TextBox)ctl).Attributes.Add(ONFOCUS, FOCUSGOTJS);
                ((TextBox)ctl).Attributes.Add(ONBLUR, FOCUSLOSTJS);
            }
        }
        public override void OverrideValue(Control ctl_, ContentPrimitive cp)
        {
            TextBox tb = (TextBox) ctl_;
            if(tb!=null&&cp!=null)
            {
                tb.Text = cp.DataValue.ToString();
            }
        }

        protected override void SetStyle()
        {
            int width = GetWidth();
            if(width>0)
            {
                ((TextBox)ctl).Width = IsPercent?Unit.Percentage(width):Unit.Pixel(width);
            }
            if (parent.ControlTypeId == (int)ContentPrimitive.ControlType.TextBox)
            {
                int height = GetHeigth();
                if (height > 0)
                {
                    ((TextBox)ctl).Height = Unit.Pixel(height);
                }
                int maxLength = GetMaxLength();
                if(maxLength>0)
                {
                    ((TextBox) ctl).MaxLength = maxLength;
                }
            }
            else
            {
                int rows = DEFAULTROWS;
                string s = parent.GetParam(TEXTAREAROWS);
                if(!String.IsNullOrEmpty(s))
                {
                    try
                    {
                        int r = int.Parse(s);
                        if(r>0)
                        {
                            rows = r;
                        }
                    }
                    catch
                    {
                    }
                }
                ((TextBox)ctl).Rows = rows;
            }
        }
        private int GetMaxLength()
        {
            int res = -1;
            string s = parent.GetParam(MAXLENGTHPARAM);
            if(!String.IsNullOrEmpty(s))
            {
                try
                {
                    res = int.Parse(s);
                }
                catch
                {
                }
            }
            return res;
        }
    }
    public class ContentPrimitiveControlHttpLink : ContentPrimitiveControl
    {
        public ContentPrimitiveControlHttpLink(ContentPrimitive _parent)
            : base(_parent)
        {
            ctl = new HtmlAnchor();
        }
        public override void BindData()
        {
            if (parent.DataValue != null)
            {
                string url = parent.DataValue.ToString().ToLower();
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    url = "http://" + url;
                }
                ((HtmlAnchor) ctl).HRef = url;
                ((HtmlAnchor)ctl).Target = "_blank";
                ((HtmlAnchor)ctl).InnerText = parent.DataValue.ToString();
            }
        }
    }
    public class ContentPrimitiveControlLabel : ContentPrimitiveControl
    {
        public ContentPrimitiveControlLabel(ContentPrimitive _parent)
            : base(_parent)
        {
            ctl = new Literal();
        }
        public override void BindData()
        {
            if (parent.DataValue != null)
            {
                if (parent.DataValue is decimal)
                {
                    ((Literal)ctl).Text = ((decimal)parent.DataValue).ToString("C");
                }
                else
                {
                    ((Literal)ctl).Text = parent.DataValue.ToString();
                }
            }
        }
    }
    public class ContentPrimitiveControlRadioButtonList : ContentPrimitiveControl
    {
        private const string ONCLICKJS = "ClickRBList(this);";
        public ContentPrimitiveControlRadioButtonList(ContentPrimitive _parent)
            : base(_parent)
        {
            ctl = new RadioButtonList();
            ((RadioButtonList)ctl).RepeatLayout = RepeatLayout.Table;
            ((RadioButtonList)ctl).RepeatDirection = RepeatDirection.Horizontal;
            ((RadioButtonList)ctl).Enabled = parent.IsEditable;
            if(parent.IsEditable)
            {
                ((RadioButtonList)ctl).Attributes.Add(NEEDPOSTBACK, parent.IsPostBack ? "1" : "0");
            }
        }
        public override void BindData()
        {
            base.BindData();
            if(parent.ControlTypeId==(int)ContentPrimitive.ControlType.YesNo)
            {
                BindYesNoList((RadioButtonList)ctl, parent.DataValue);
            }
            else
            {
                BindRadioButtonList((RadioButtonList)ctl, parent.Mpfi, parent.DataValue.ToString());
            }
            ((RadioButtonList)ctl).TabIndex = parent.TabIndex;
        }
        private void BindRadioButtonList(ListControl rbl, MortgageProfileFieldInfo mpfi, string fieldValue)
        {
            DataView dv = parent.ParentDataTab.CurrentPage.GetDictionary(mpfi.TableName);
            if (dv != null)
            {
                int id = -1;
                try
                {
                    id = Convert.ToInt32(fieldValue);
                }
                catch
                {
                }
                rbl.DataSource = dv;
                rbl.DataTextField = mpfi.FieldName;
                rbl.DataValueField = "Id";
                rbl.DataBind();
                if (id > 0)
                {
                    rbl.SelectedValue = id.ToString();
                }
                rbl.Attributes.Add("selected", id.ToString());
            }
        }
        private void BindYesNoList(ListControl rbl, Object fieldValue)
        {
            DataView dv = parent.ParentDataTab.CurrentPage.GetDictionary(Constants.DICTIONARYYESNOTABLE);
            if (dv != null)
            {
                rbl.DataSource = dv;
                rbl.DataTextField = "Name";
                rbl.DataValueField = "Id";
                rbl.DataBind();
                int id = -1;
                if (fieldValue != null)
                {
                    id = ((bool)fieldValue) ? 1 : 2;
                }
                if (id > 0)
                {
                    rbl.SelectedValue = id.ToString();
                }
                rbl.Attributes.Add("selected", id.ToString());
            }
        }
        public override void OverrideValue(Control ctl_, ContentPrimitive cp)
        {
            if (cp.ControlTypeId == (int)ContentPrimitive.ControlType.YesNo)
            {
                RadioButtonList rb = (RadioButtonList)ctl_;
                if (rb != null)
                {
                    BindYesNoList(rb, cp.DataValue);
                }
            }

        }
        public override void AttachScript()
        {
            if(parent.IsEditable)
            {
                base.AttachScript();
                ((RadioButtonList)ctl).Attributes.Add(ONCLICK, ONCLICKJS);
            }
        }
    }
    public class ContentPrimitiveControlDropDownList : ContentPrimitiveControl
    {
        #region constants
        private const string IDFIELDNAME = "id";
        private const string NOTSELECTEDTEXT = "-Select-";
        private const int NOTSELECTEDVALUE = 0;
        private const string INDEXCHANGEDJS = "DDLIndexChanged(this,{0});";
        private const string FOCUSGOTJS = "DDLFocusGot(this);";
        private const string DEFAULTWIDTH = "156";
        private const string DEFAULTHEIGHT = "22";
        #endregion
        protected override string DefaultWidth
        {
            get
            {
                return DEFAULTWIDTH;
            }
        }
        protected override string DefaultHeight
        {
            get
            {
                return DEFAULTHEIGHT;
            }
        }
        public ContentPrimitiveControlDropDownList(ContentPrimitive _parent)
            : base(_parent)
        {
            ctl = new DropDownList();
            ((DropDownList)ctl).Enabled = parent.IsEditable;
            if(parent.IsEditable)
            {
                ((DropDownList)ctl).Attributes.Add(NEEDPOSTBACK, parent.IsPostBack ? "1" : "0");
            }
        }
        public override void BindData()
        {
            base.BindData();
            BindDropDown((DropDownList)ctl, parent.Mpfi);
            if (parent.DataValue != null) ((DropDownList)ctl).SelectedValue = parent.DataValue.ToString();
            if (((DropDownList)ctl).SelectedValue == "")
            {
                ((DropDownList)ctl).SelectedValue = NOTSELECTEDVALUE.ToString();
            }
            ((DropDownList)ctl).TabIndex = parent.TabIndex;

        }
        private void BindDropDown(ListControl ddl, MortgageProfileFieldInfo mpfi)
        {
            DataView dv;
            if (mpfi.IsDataSourceProcedure)
            {
                dv = DataHelpers.GetDataViewByStoredProcedure(mpfi.ProcedureName,parent.Mp.ID);
            }
            else
            {
                dv = parent.ParentDataTab.CurrentPage.GetDictionary(mpfi.TableName, parent.FilterValue);
            }            
            if (dv.Count > 0)
            {
                dv.Sort = mpfi.FieldName;
                ddl.DataSource = dv;
                ddl.DataTextField = mpfi.FieldName;
                ddl.DataValueField = IDFIELDNAME;
                ddl.DataBind();
            }
            ddl.Items.Insert(0, new ListItem(NOTSELECTEDTEXT, NOTSELECTEDVALUE.ToString()));
        }
        //private void BindDropDown(ListControl ddl,MortgageProfileFieldInfo mpfi)
        //{
        //    DataView dv = parent.ParentDataTab.CurrentPage.GetDictionary(mpfi.SourceTableName, parent.FilterValue);
        //    if (dv.Count > 0)
        //    {
        //        dv.Sort = mpfi.SourceFieldName;
        //        ddl.DataSource = dv;
        //        ddl.DataTextField = mpfi.SourceFieldName;
        //        ddl.DataValueField = IDFIELDNAME;
        //        ddl.DataBind();
        //    }
        //    ddl.Items.Insert(0, new ListItem(NOTSELECTEDTEXT, NOTSELECTEDVALUE.ToString()));
        //}
        public override void AttachScript()
        {
            if (parent.IsEditable)
            {
                base.AttachScript();
                ((DropDownList) ctl).Attributes.Add(ONCHANGE,String.Format(INDEXCHANGEDJS, parent.IsPostBack ? "1" : "0"));
                ((DropDownList)ctl).Attributes.Add(ONFOCUS, FOCUSGOTJS);
            }
        }
        protected override void SetStyle()
        {
            int width = GetWidth();
            if (width > 0)
            {
                ((DropDownList)ctl).Width = IsPercent ? Unit.Percentage(width) : Unit.Pixel(width);
            }
            int height = GetHeigth();
            if (height > 0)
            {
                ((DropDownList)ctl).Height = Unit.Pixel(height);
            }
        }
    }
    public class ContentPrimitiveControlDateInput : ContentPrimitiveControl
    {
        private const string ONDATACHANGEJS = "RadInputDateChanged";
        private const string DEFAULTWIDTH = "100";
        private const string DEFAULTHEIGHT = "18";
        protected override string DefaultWidth
        {
            get
            {
                return DEFAULTWIDTH;
            }
        }
        protected override string DefaultHeight
        {
            get
            {
                return DEFAULTHEIGHT;
            }
        }
        public ContentPrimitiveControlDateInput(ContentPrimitive _parent)
            : base(_parent)
        {
            ctl = new RadDateInput();
            ((RadDateInput)ctl).Skin = "Windows";
            ((RadDateInput)ctl).MinDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            ((RadDateInput)ctl).Enabled = parent.IsEditable;
            if (parent.IsEditable)
            {
                ((RadDateInput)ctl).Attributes.Add(NEEDPOSTBACK, parent.IsPostBack ? "1" : "0");
            }
        }
        public override void BindData()
        {
            base.BindData();
            if (parent.DataValue != null)
            {
                DateTime dt = Convert.ToDateTime(parent.DataValue);
                if (dt == System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                {
                    ((RadDateInput)ctl).SelectedDate = null;
                }
                else
                {
                    ((RadDateInput)ctl).SelectedDate = DateTime.Parse(parent.DataValue.ToString());// Convert.ToDateTime(fld.FieldValue);
                }
            }
            else
            {
                ((RadDateInput)ctl).SelectedDate = null;
            }
            ((RadDateInput)ctl).TabIndex = parent.TabIndex;
        }
        public override void AttachScript()
        {
            if (parent.IsEditable)
            {
                base.AttachScript();
                ((RadDateInput)ctl).ClientEvents.OnValueChanged = ONDATACHANGEJS;
            }
        }
        protected override void SetStyle()
        {
            int width = GetWidth();
            if (width > 0)
            {
                ((RadDateInput)ctl).Width = IsPercent ? Unit.Percentage(width) : Unit.Pixel(width);
            }
            int height = GetHeigth();
            if (height > 0)
            {
                ((RadDateInput)ctl).Height = Unit.Pixel(height);
            }
        }
    }
    public class ContentPrimitiveControlMaskedInput : ContentPrimitiveControl
    {
        private const string FOCUSGOTHIDDENJS = "RadMaskedTextBoxHiddenFocusGot(this);";
        private const string FOCUSGOTJS = "RadMaskedTextBoxFocusGot(this);";
        private const string FOCUSLOSTJS = "RadMaskedTextBoxFocusLost(this,{0});";
        private const string DEFAULTWIDTH = "100";
        private const string DEFAULTHEIGHT = "18";
        protected override string DefaultWidth
        {
            get
            {
                return DEFAULTWIDTH;
            }
        }
        protected override string DefaultHeight
        {
            get
            {
                return DEFAULTHEIGHT;
            }
        }
        public ContentPrimitiveControlMaskedInput(ContentPrimitive _parent)
            : base(_parent)
        {
            ctl = new RadMaskedTextBox();
            ((RadMaskedTextBox)ctl).Skin = "Windows";
            ((RadMaskedTextBox)ctl).Enabled = parent.IsEditable;
            ((RadMaskedTextBox)ctl).HideOnBlur = true;
            if (parent.IsEditable)
            {
                ((RadMaskedTextBox)ctl).Attributes.Add(NEEDPOSTBACK, parent.IsPostBack ? "1" : "0");
            }
            if (parent.Mpfi.IsHidden)
            {
                ((RadMaskedTextBox) ctl).Attributes.Add("mask", parent.Mpfi.MaskValue);
                ((RadMaskedTextBox)ctl).Attributes.Add("hidden", "1");
                ((RadMaskedTextBox)ctl).Attributes.Add("editable",((RadMaskedTextBox)ctl).Enabled? "1" : "0");
                ((RadMaskedTextBox)ctl).HideOnBlur = false;
                ((RadMaskedTextBox)ctl).Enabled = true;
                ((RadMaskedTextBox)ctl).ReadOnly = true;
                //string s = GetHiddenValue(parent.DataValue.ToString().Length);
                //if (!String.IsNullOrEmpty(s))
                //{
                //    s = s.Replace("*", @"\*");
                //}
                //((RadMaskedTextBox)ctl).Mask = s;
                //((RadMaskedTextBox)ctl).DisplayMask = s;
                ((RadMaskedTextBox) ctl).DisplayPromptChar = "*";
            }
            ((RadMaskedTextBox)ctl).Mask = parent.Mpfi.MaskValue;
            ((RadMaskedTextBox)ctl).DisplayMask = parent.Mpfi.MaskValue;
            ((RadMaskedTextBox)ctl).PromptChar = " ";
        }
        public override void BindData()
        {
            base.BindData();
            if (parent.DataValue != null)
            {
                ((RadMaskedTextBox) ctl).Text = parent.DataValue.ToString(); 
            }
            ((RadMaskedTextBox)ctl).TabIndex = parent.TabIndex;
            if(parent.Mpfi.IsHidden)
            {
                ((RadMaskedTextBox)ctl).Text = GetHiddenValue(parent.DataValue.ToString().Length);
                ((RadMaskedTextBox)ctl).Text = "";
            }
        }
        public override void AttachScript()
        {
            if (parent.IsEditable || parent.Mpfi.IsHidden)
            {
                base.AttachScript();
                ((RadMaskedTextBox)ctl).Attributes.Add(ONFOCUS, parent.Mpfi.IsHidden? FOCUSGOTHIDDENJS : FOCUSGOTJS);
                ((RadMaskedTextBox)ctl).Attributes.Add(ONBLUR, String.Format(FOCUSLOSTJS,parent.IsPostBack ? "1" : "0"));
            }
        }
        protected override void SetStyle()
        {
            int width = GetWidth();
            if (width > 0)
            {
                ((RadMaskedTextBox)ctl).Width = IsPercent ? Unit.Percentage(width) : Unit.Pixel(width);
            }
            int height = GetHeigth();
            if (height > 0)
            {
                ((RadMaskedTextBox)ctl).Height = Unit.Pixel(height);
            }
        }
    }
    public class ContentPrimitiveControlMoneyInput : ContentPrimitiveControl
    {
        private const string MONEYFORMAT = "money";
        private const string NUMBERFORMAT = "number";
        private const string INTEGERFORMAT = "integer";
        private const string PERCENTAGEFORMAT = "percentage";

        private const string FOCUSGOTJS = "RadMaskedTextBoxFocusGot(this);";
        private const string FOCUSLOSTJS = "RadMaskedTextBoxFocusLost(this,{0});";

        private const string DEFAULTWIDTH = "100";
        private const string DEFAULTHEIGHT = "18";
        protected override string DefaultWidth
        {
            get
            {
                return DEFAULTWIDTH;
            }
        }
        protected override string DefaultHeight
        {
            get
            {
                return DEFAULTHEIGHT;
            }
        }
        public ContentPrimitiveControlMoneyInput(ContentPrimitive _parent)
            : base(_parent)
        {
            ctl = new RadNumericTextBox();
            ((RadNumericTextBox)ctl).Skin = "Windows";
            ((RadNumericTextBox)ctl).Enabled = parent.IsEditable;
            if (parent.Mpfi.MaskValue == MONEYFORMAT)
            {
                ((RadNumericTextBox)ctl).Type = NumericType.Currency;
                ((RadNumericTextBox)ctl).NumberFormat.DecimalDigits = 2;
                ((RadNumericTextBox)ctl).NumberFormat.DecimalSeparator = ".";
                ((RadNumericTextBox)ctl).NumberFormat.GroupSeparator = ",";
                ((RadNumericTextBox)ctl).NumberFormat.GroupSizes = 3;
            }
            else if (parent.Mpfi.MaskValue == NUMBERFORMAT)
            {
                ((RadNumericTextBox)ctl).Type = NumericType.Number;
                ((RadNumericTextBox)ctl).NumberFormat.DecimalDigits = 2;
                ((RadNumericTextBox)ctl).NumberFormat.DecimalSeparator = ".";
                ((RadNumericTextBox)ctl).NumberFormat.GroupSeparator = ",";
                ((RadNumericTextBox)ctl).NumberFormat.GroupSizes = 3;
            }
            else if (parent.Mpfi.MaskValue == INTEGERFORMAT)
            {
                ((RadNumericTextBox)ctl).Type = NumericType.Number;
                ((RadNumericTextBox)ctl).NumberFormat.DecimalDigits = 0;
                ((RadNumericTextBox)ctl).NumberFormat.DecimalSeparator = ".";
                ((RadNumericTextBox)ctl).NumberFormat.GroupSeparator = ",";
                ((RadNumericTextBox)ctl).NumberFormat.GroupSizes = 3;
                ((RadNumericTextBox)ctl).MinValue = 0;
            }
            else if (parent.Mpfi.MaskValue == PERCENTAGEFORMAT)
            {
                ((RadNumericTextBox)ctl).Type = NumericType.Percent;
                ((RadNumericTextBox)ctl).NumberFormat.DecimalDigits = 2;
                ((RadNumericTextBox)ctl).NumberFormat.DecimalSeparator = ".";
            }
        }
        public override void BindData()
        {
            base.BindData();
            if (parent.DataValue != null)
            {
                ((RadNumericTextBox)ctl).Text = parent.DataValue.ToString();
            }
            ((RadNumericTextBox)ctl).TabIndex = parent.TabIndex;
        }
        public override void AttachScript()
        {
            if (parent.IsEditable)
            {
                base.AttachScript();
                ((RadNumericTextBox)ctl).Attributes.Add(ONFOCUS,FOCUSGOTJS);
                ((RadNumericTextBox)ctl).Attributes.Add(ONBLUR, String.Format(FOCUSLOSTJS, parent.IsPostBack ? "1" : "0"));
            }
        }
        protected override void SetStyle()
        {
            int width = GetWidth();
            if (width > 0)
            {
                ((RadNumericTextBox)ctl).Width = IsPercent ? Unit.Percentage(width) : Unit.Pixel(width);
            }
            int height = GetHeigth();
            if (height > 0)
            {
                ((RadNumericTextBox)ctl).Height = Unit.Pixel(height);
            }
        }
    }
    }
