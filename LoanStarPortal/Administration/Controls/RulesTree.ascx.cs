using System;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Collections;
using LoanStar.Common;
using Telerik.WebControls;
using Telerik.WebControls.RadTreeViewContextMenu;
using Rule=LoanStar.Common.Rule;


namespace LoanStarPortal.Administration.Controls
{
    public partial class RulesTree : AppControl
    {
        #region constants
        private const string POSTBACKCONTROL = "__EVENTTARGET";
        private const string NOTSELECTEDTEXT = "- Select - ";
        private const int NOTSELECTEDVALUE = 0;
        private const string RULEIMAGE = "folder.gif";
        private const bool ENABLED = true;
        private const int MENUITEMWIDTH = 90;
        private const string ROOTMENUNAME = "Rootmenu";
        private const string COPYTREEMENUNAME = "Copytree";
        private const string EDITMENUITEMTEXT = "Edit";
        private const string ADDCHILDMENUITEMTEXT = "Add child";
        private const string COPYTREEMENUITEMTEXT = "Copy rule branch";
        private const string ENABLEMENUITEMTEXT = "Enable";
        private const string DISABLEMENUITEMTEXT = "Disable";
        private const string DELETEMENUITEMTEXT = "Delete";
        private const string CLICKHANDLER = "onclick";
        private const string SELECTEDFIELDNAME = "Selected";
        private const string NOTSELECTED = "- Select -";
        private const string CHECKLISTNAME = "Checklist {0}";
        private const int TABPROPERTYID = 0;
        private const int TABSHOWFIELDSID = 1;
        private const int TABCONDITIONSID = 2;
        private const int TABDOCUMENTSID = 3;
        private const int TABCHECKLISTSID = 4;
        private const int TABALERTSID = 5;
        private const int TABDATAID = 6;
        private const string EDITORSTEPINDEX = "editorstep";
        private const string SHOWFIELDSSTEPINDEX = "showfieldstep";
        private const string DATASTEPINDEX = "datastep";
        private const string VALUETYPETEXT = "Value";
        private const string FIELDTYPETEXT = "Field";
        private const int VALUETYPEVALUE = 1;
        private const int FIELDTYPEVALUE = 2;
        private const string YES = "Yes";
        private const string YESVALUE = "1";
        private const string NO = "No";
        private const string NOVALUE = "2";
        private const string ANOTHERFIELDLABELTEXT = "Select another field";
        private const string DICTIONARYLABELTEXT = "Select value";
        private const string GENERALLABELTEXT = "Enter value";
        private const string STRINGLABELTEXT = "Enter string";
        private const string DATELABELTEXT = "Enter date";
        private const string INTEGERLABELTEXT = "Enter integer";
        private const string FLOATLABELTEXT = "Enter float";
        private const string MONEYLABELTEXT = "Enter money";
        private const int CONTROLTEXTBOX = 0;
        private const int CONTROLSELECT = 1;
        private const int CONTROLDATEPICKER = 2;
        private const int CONTROLMASKEDTEXTBOX = 3;
        private const string LOGICALOPERATIONSTABLE = "LogicalOperation";
        private const string FIELDGROUPTABLE = "vwFieldGroup";
        private const string MORTGAGEFIELDSTABLE = "vwMortgageFields";
        private const string COMPAREOPTABLENAME = "vwCompareOp";
        private const string FIELDSFILTER = "groupid={0}";
        private const string COMPOPFILTER = "allowedforboolandstring={0}";
        private readonly string[] editorDdls = { "ddlLogicalOp", "ddlGroup", "ddlField", "ddlCompareOp", "ddlFieldType" };
        private readonly string[] fieldsDdls = { "ddlGroupField"};
        private readonly string[] dataDdls = { "ddlDataGroup","ddlDataField"};
        private const string UNITSCOUNT = "unitscount";
        private const string CURRENTRULEEDITNODEID = "currentnodeid";
        private const string CURRENTRULENODE = "currentnode";
        private const string FIELDNEEDEDMESSAGE = "*";
        private const string GRIDDELETEBUTTONID = "btnDelete";
        private const string GRIDEDITBUTTONID = "btnEdit";
        private const string ONCLICKATTRIBUTE = "OnClick";
        private const string DELETECOMMAND = "deleteobject";
        private const string EDITCONDITIONCOMMAND = "editcondition";
        private const string EDITCHECKLISTCOMMAND = "editchecklist";
        private const string EDITDOCUMENTCOMMAND = "editdocument";
        private const string PAGECOMMAND = "page";
//        private const string JSDELETECONFIRM = "if(!confirm('Are you sure you want to delete this item?'))return false;";
        private const string JSDELETECONFIRM = "javascript:{{var r=confirm('Are you sure you want to delete this item?');if (!r)return false;}};";        
        private const string EXPRESSIONGRIDNAME = "gProperties";
        private const string SHOWFIELDGRIDNAME = "gFields";
        private const string CONDITIONSGRIDNAME = "gConditions";
        private const string CHECKLISTSGRIDNAME = "gCheckList";
        private const string ALERTSGRIDNAME = "gAlert";
        private const string DATAGRIDNAME = "gData";
        private const string DOCUMENTGRIDNAME = "gDocuments";
        private const string ROOTELEMENT = "Root";
        private const string IDATTRIBUTE = "id"; 
        private const string ITEMELEMENT = "item";
        private const string CURRENTCONDITIONID = "conditionid";
        private const string CURRENTTASKID = "taskid";
        private const string SETTABJS = "SetTabIndex({0});";
        private const string STATUSID = "statusid";
        private const string TEXTATTRIBUTE = "text";
        private const string YESATTRIBUTE = "yes";
        private const string NOATTRIBUTE = "no";
        private const string DONOTKNOWATTRIBUTE = "donotknow";
        private const string TOFOLLOWATTRIBUTE = "tofollow";
        private const string JSCLICKHANDLER = "CheckItems(this.checked,\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\");";
        private const string CURRENTCHECKLISTID = "checklistid";
        private const string ALERTEVENTID = "alerteventid";
        private const string ISEVENT = "isevent";
        private const string EDITALERTCOMMAND = "editalert";
        private const string DATAID = "dataid";
        private const string DOCUMENTID = "documentid";
        private const string EMPTYTABCOLOR = "color:Gray";
        private const string JSUPDATECONTENT = "UpdateContent('{0}');";
        private const string CATEGORYID = "categoryid";
        private const string RTPUBLICKEY = "rtpublic_{0}";
        private const string ISCLIOSINGINSTRUCTION = "isclosingisntruction";
        #endregion

        #region fields
        private readonly ArrayList contextMenus = new ArrayList();
        private DataView dvCategory;
        private int counter;
        private Field field;
        private bool isRebinded = false;
        private readonly int[] tabsState = {0, 0, 0, 0, 0, 0, 0, 0};
        private bool isNewCategory=false;
        #endregion

        #region properties
        protected DataView DvCategory
        {
            get
            {
                if(dvCategory==null)
                {
                    dvCategory = RuleTree.GetCategory(CurrentUser.EffectiveCompanyId);
                }
                return dvCategory;
            }
        }
        protected int EditorStep
        {
            get
            {
                Object o = ViewState[EDITORSTEPINDEX];
                int res = -1;
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
                ViewState[EDITORSTEPINDEX] = value;
            }
        }
        protected int ShowFieldStep
        {
            get
            {
                Object o = ViewState[SHOWFIELDSSTEPINDEX];
                int res = -1;
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
                ViewState[SHOWFIELDSSTEPINDEX] = value;
            }
        }
        protected int DataStep
        {
            get
            {
                Object o = ViewState[DATASTEPINDEX];
                int res = -1;
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
                ViewState[DATASTEPINDEX] = value;
            }
        }
        protected int UnitsCount
        {
            get
            {
                int res = 0;
                Object o = ViewState[UNITSCOUNT];
                try
                {
                    res = int.Parse(o.ToString());
                }
                catch
                {
                }
                return res;
            }
            set { ViewState[UNITSCOUNT] = value; }
        }
        protected int CurrentRuleNodeId
        {
            get
            {
                int res = -1;
                Object o = ViewState[CURRENTRULEEDITNODEID];
                if(o!=null)
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
            set { ViewState[CURRENTRULEEDITNODEID] = value; }
        }
        protected RuleEditNode CurrentEditNode
        {
            get
            {
                RuleEditNode res = Session[CURRENTRULENODE] as RuleEditNode;
                if((res==null)||(res.Id!=CurrentRuleNodeId))
                {
                    res = new RuleEditNode(CurrentRuleNodeId);
                    Session[CURRENTRULENODE] = res;
                }
                return res;
            }
            set { Session[CURRENTRULENODE] = value; }
        }
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
        protected int CurrentTaskId
        {
            get
            {
                int res = -1;
                Object o = ViewState[CURRENTTASKID];
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
                    ViewState[CURRENTTASKID] = res;
                }
                return res;
            }
            set
            {
                ViewState[CURRENTTASKID] = value;
            }
        }
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
        protected int CurrentAlertEventId
        {
            get
            {
                int res = -1;
                Object o = ViewState[ALERTEVENTID];
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
                    ViewState[ALERTEVENTID] = res;
                }
                return res;
            }
            set
            {
                ViewState[ALERTEVENTID] = value;
            }
        }
        protected int CurrentDataId
        {
            get
            {
                int res = -1;
                Object o = ViewState[DATAID];
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
                    ViewState[DATAID] = res;
                }
                return res;
            }
            set
            {
                ViewState[DATAID] = value;
            }
        }
        protected int CurrentDocumentId
        {
            get
            {
                int res = -1;
                Object o = ViewState[DOCUMENTID];
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
                    ViewState[DOCUMENTID] = res;
                }
                return res;
            }
            set
            {
                ViewState[DOCUMENTID] = value;
            }
        }
        protected bool IsEvent
        {
            get
            {
                bool res = true;
                Object o = ViewState[ISEVENT];
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
                    ViewState[ISEVENT] = res;
                }
                return res;
            }
            set
            {
                ViewState[ISEVENT] = value;
            }
        }
        protected bool IsClosingInstruction
        {
            get
            {
                bool res = false;
                Object o = ViewState[ISCLIOSINGINSTRUCTION];
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
                    ViewState[ISCLIOSINGINSTRUCTION] = res;
                }
                return res;
            }
            set
            {
                ViewState[ISCLIOSINGINSTRUCTION] = value;
            }
        }
        protected int CategoryId
        {
            get
            {
                int res = 0;
                Object o = ViewState[CATEGORYID];
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
                    ViewState[CATEGORYID] = res;
                }
                return res;
            }
            set
            {
                ViewState[CATEGORYID] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            cbClosingInstruction.Attributes.Add(ONCLICKATTRIBUTE, String.Format("SetVisibility(this,'{0}','{1}');", trCondType.ClientID, trCondCategory.ClientID));
            CreateMenuArray();
            if (!IsPostBack)
            {
                BindData();
            }
            else
            {
                ProcessPostBack();
            }
        }
        #region methods

        #region postback related methods
        private void ProcessPostBack()
        {
            string controlName = Page.Request[POSTBACKCONTROL];
            if (!String.IsNullOrEmpty(controlName))
            {
                if(controlName.EndsWith(":ddlCategory"))
                {
                    CurrentRuleNodeId = -1;
                    CurrentEditNode = null;
                    CategoryId = GetDdlSelectedValue(controlName.Replace(":", "$"));
                    BindData();
                    return;
                }
                int curTab = GetCurrentTabId();
                switch (curTab)
                {
                    case TABPROPERTYID:
                        ProcessPropertyTabPostBack(controlName);
                        break;
                    case TABSHOWFIELDSID:
                        ProcessShowFieldTabPostBack(controlName);
                        break;
                    case TABCONDITIONSID:
                        ProcessConditionTabPostBack(controlName);
                        break;
                    case TABCHECKLISTSID:
                        ProcessCheckListTabPostBack(controlName);
                        break;
                    case TABALERTSID:
                        ProcessAlertTabPostBack(controlName);
                        break;
                    case TABDATAID:
                        ProcessDataTabPostBack(controlName);
                        break;
                    case TABDOCUMENTSID:
                        ProcessDocumentTabPostBack(controlName);
                        break;
                }
            }
        }
        private int GetCurrentTabId()
        {
            int res = -1;
            try
            {
                res = int.Parse(Page.Request.Form[currentruletab.ClientID.Replace("_", "$")]);
            }
            catch
            {
            }
            return res; 
        }
        private bool CheckForGridCommand(string controlName)
        {
            if(controlName.EndsWith(":"+EXPRESSIONGRIDNAME))
            {
                BindExpressionGrid();
                return true;
            }
            else if (controlName.EndsWith(":" + GRIDDELETEBUTTONID))
            {
                if(controlName.IndexOf(":" + EXPRESSIONGRIDNAME + ":") >= 0)
                {
                    BindExpressionGrid();
                }
                else if (controlName.IndexOf(":" + SHOWFIELDGRIDNAME + ":") >= 0)
                {
                    BindFieldsGrid();
                }
                else if (controlName.IndexOf(":" + CONDITIONSGRIDNAME + ":") >= 0)
                {
                    BindConditionsGrid();
                }
                else if (controlName.IndexOf(":" + CHECKLISTSGRIDNAME + ":") >= 0)
                {
                    BindCheckListGrid();
                }
                else if (controlName.IndexOf(":" + ALERTSGRIDNAME + ":") >= 0)
                {
                    BindAlertGrid();
                }
                else if (controlName.IndexOf(":" + DATAGRIDNAME + ":") >= 0)
                {
                    BindDataGrid();
                }
                else if (controlName.IndexOf(":" + DOCUMENTGRIDNAME + ":") >= 0)
                {
                    BindDocumentGrid();
                }
                return true;
            }
            else if (controlName.IndexOf(":" + CONDITIONSGRIDNAME + ":") >= 0)
            {
                BindConditionsGrid();
            }
            else if (controlName.IndexOf(":" + CHECKLISTSGRIDNAME) >= 0)
            {
                BindCheckListGrid();
            }
            else if (controlName.IndexOf(":" + ALERTSGRIDNAME) >= 0)
            {
                BindAlertGrid();
            }
            else if (controlName.IndexOf(":" + DOCUMENTGRIDNAME) >= 0)
            {
                BindDocumentGrid();
            }
            return false;
        }
        private int GetDdlSelectedValue(string controlName)
        {
            int res = -1;
            try
            {
                res = int.Parse(Page.Request.Form[controlName]);
            }
            catch
            {
            }
            return res;
        }
        private string GetPostValue(string key)
        {
            string res = String.Empty;
            try
            {
                res = Page.Request.Form[key];
            }
            catch
            {
            }
            return res;
        }

        private void StoreDdlValue(string ddlName, int selValue)
        {
            ViewState[ddlName] = selValue;
        }
        private int RestoreDdlSelectedValue(string controlName)
        {
            int res = 0;
            try
            {
                res = int.Parse(ViewState[controlName].ToString());
            }
            catch
            {
            }
            return res;
        }
        private void ProcessPropertyTabPostBack(string controlName)
        {
            for (int i = 0; i < editorDdls.Length; i++)
            {
                if (controlName.EndsWith(":" + editorDdls[i]))
                {
                    StoreDdlValue(editorDdls[i], GetDdlSelectedValue(controlName.Replace(":", "$")));
                    EditorStep++;
                    InitEditor();
                    BindEditorDictionaries();
                    ExecuteEditorStep();
                    return;
                }
            }
            if (CheckForGridCommand(controlName))
            {
                return;
            }
            if (controlName.EndsWith(":btnSaveProperties"))
            {
                BindNodeCategory();
                return;
            }
        }
        private void ProcessShowFieldTabPostBack(string controlName)
        {
            for (int i = 0; i < fieldsDdls.Length; i++)
            {
                if (controlName.EndsWith(":" + fieldsDdls[i]))
                {
                    StoreDdlValue(fieldsDdls[i], GetDdlSelectedValue(controlName.Replace(":", "$")));
                    ShowFieldStep++;
                    BindFieldsDictionaries();
                    ExecuteShowFieldStep();
                    return;
                }
            }
            if (controlName.EndsWith(":btnAddField"))
            {
                BindFieldsDictionaries();
                return;
            }
            if (CheckForGridCommand(controlName))
            {
                return;
            }
        }
        private void ProcessConditionTabPostBack(string controlName)
        {
            if (controlName.EndsWith(":btnSaveCondition"))
            {
                BindAuthorityLevels();
                BindConditionCategory();
                BindConditionType();
            }
            if (CheckForGridCommand(controlName))
            {
                return;
            }
        }
        private void ProcessDataTabPostBack(string controlName)
        {
            for (int i = 0; i < dataDdls.Length; i++)
            {
                if (controlName.EndsWith(":" + dataDdls[i]))
                {
                    StoreDdlValue(dataDdls[i], GetDdlSelectedValue(controlName.Replace(":", "$")));
                    DataStep++;
                    InitDataEditor();
                    BindDataDictionaries();
                    ExecuteDataEditorStep();
                    return;
                }
            }
            if (controlName.EndsWith(":btnAddData"))
            {
                field = GetField();
                if(field.IsDictionaryField)
                {
                    BindDataFieldDictionary();
                }
                else if ((Field.MortgageProfileFieldType)field.TypeId==Field.MortgageProfileFieldType.Boolean)
                {
                    BindYesNo(ddlDataDictionary);
                }
            }
            if (CheckForGridCommand(controlName))
            {
                return;
            }
        }
        private void ProcessCheckListTabPostBack(string controlName)
        {
            if (controlName.EndsWith(":btnSaveCheckList"))
            {
                BindRepeater();
            }
            if (CheckForGridCommand(controlName))
            {
                return;
            }
        }                    
        private void ProcessDocumentTabPostBack(string controlName)
        {
            if (controlName.EndsWith(":btnSaveDocument"))
            {
                BindDocumentDictionaries();
            }
            if (CheckForGridCommand(controlName))
            {
                return;
            }
        }
        private void ProcessAlertTabPostBack(string controlName)
        {
            //if (controlName.EndsWith(":btnSaveAlert"))
            //{
            //    BindAlertDictionary();
            //}
            if (CheckForGridCommand(controlName))
            {
                return;
            }
        }
        #endregion

        #region tree related methods
        private void BindData()
        {
            BindCategory();
            BindTree(false);
            currentruletab.Value = TABPROPERTYID.ToString();
        }
        private void BindCategory()
        {
            ddlCategory.DataSource = DvCategory;
            ddlCategory.DataTextField = RuleTree.NAMEFIELDNAME;
            ddlCategory.DataValueField = RuleTree.IDFIELDNAME;
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem(NOTSELECTEDTEXT, NOTSELECTEDVALUE.ToString()));
        }
        private void BindTree(bool rebind)
        {
            isRebinded = rebind;
            rtvRule.ContextMenus = contextMenus;
            XmlDataSource ds = new XmlDataSource();
            ds.EnableCaching = false;
            RuleTreeAdmin rt;
            if(rebind)
            {
                rt = CurrentPage.ReloadRuleTreeAdmin();
            }
            else
            {
                rt = CurrentPage.GetRuleTreeAdmin();
            }
            ds.Data = rt.GetFilteredTree(CategoryId).OuterXml;
            rtvRule.DataSource = ds;
            rtvRule.DataTextField = "text";
            rtvRule.DataBind();
            RadTreeNode node = rtvRule.SelectedNode;
            if (node != null)
            {
                ExpandBranch(node);
                node.Focus();
            }
        }
        private static void ExpandBranch(RadTreeNode node)
        {
            RadTreeNode currentNode = node;
            while (currentNode!=null)
            {
                currentNode.Expanded = true;
                currentNode = currentNode.Parent;
            }
        }
        #region tree node binding methods
        private static int GetAttributeInt(XmlElement xmlNode, string name)
        {
            int res = -1;
            if (xmlNode.HasAttribute(name))
            {
                try
                {
                    res = int.Parse(xmlNode.Attributes[name].Value);
                }
                catch
                {
                }
            }
            return res;
        }
        private static bool GetEnabled(XmlElement xmlNode)
        {
            bool res = true;
            if (xmlNode.HasAttribute(RuleTreeAdmin.ENABLEDATTRIBUTE))
            {
                res = xmlNode.Attributes[RuleTreeAdmin.ENABLEDATTRIBUTE].Value == "1";
            }
            return res;
        }
        private static string GetNodeCss(int nodeTypeId, bool isEnabled)
        {
            string res = String.Empty;
            switch (nodeTypeId)
            {
                case RuleTreeAdmin.nodeRule:
                    res = isEnabled ? "TreeNodeRule" : "TreeNodeRuleDisabled";
                    break;
                case RuleTreeAdmin.FIELDBIT:
                    res = "TreeNodeSF";
                    break;
                case RuleTreeAdmin.CONDITIONBIT:
                    res = "TreeNodeCD";
                    break;
                case RuleTreeAdmin.TASKBIT:
                    res = "TreeNodeTS";
                    break;
                case RuleTreeAdmin.DOCUMENTBIT:
                    res = "TreeNodeDC";
                    break;
                case RuleTreeAdmin.CHECKLISTBIT:
                    res = "TreeNodeCL";
                    break;
                case RuleTreeAdmin.EVENTBIT:
                    res = "TreeNodeEV";
                    break;
                case RuleTreeAdmin.ALERTBIT:
                    res = "TreeNodeAL";
                    break;
                case RuleTreeAdmin.DATABIT:
                    res = "TreeNodeDT";
                    break;
            }
            return res;
        }
        private static int GetNodeType(XmlElement node)
        {
            int res = -1;
            if (node.HasAttribute(RuleTreeAdmin.NODETYPEATTRIBUTE))
            {
                try
                {
                    res = int.Parse(node.Attributes[RuleTreeAdmin.NODETYPEATTRIBUTE].Value);
                }
                catch
                {
                }
            }
            return res;
        }
        private static string GetNodeImage(int typeId)
        {
            string res = String.Empty;
            if (typeId == RuleTree.nodeRule)
            {
                res = Constants.IMAGEFOLDER + "/" + RULEIMAGE;
            }
            return res;
        }
        protected void rtvRule_NodeBound(object o, RadTreeNodeEventArgs e)
        {
            XmlElement xmlNode = (XmlElement)e.NodeBound.DataItem;
            RadTreeNode node = e.NodeBound;
            int nodeId = GetAttributeInt(xmlNode, RuleTreeAdmin.IDATTRIBUTE);
            node.Value = nodeId.ToString();
            node.Attributes.Add(RuleTreeAdmin.IDATTRIBUTE, GetAttributeInt(xmlNode, RuleTreeAdmin.IDATTRIBUTE).ToString());
            int nodeTypeId = GetNodeType(xmlNode);
            bool isEnabled = GetEnabled(xmlNode);
            int companyId = GetAttributeInt(xmlNode, RuleTreeAdmin.COMPANYIDATTRIBUTE);
            int parentId = GetAttributeInt(xmlNode, RuleTreeAdmin.PARENTIDATTRIBUTE);
            node.Attributes.Add(RuleTreeAdmin.NODETYPEATTRIBUTE, GetAttributeInt(xmlNode, RuleTreeAdmin.NODETYPEATTRIBUTE).ToString());
            node.Attributes.Add(RuleTreeAdmin.PARENTIDATTRIBUTE, parentId.ToString());
            if (nodeTypeId == RuleTree.nodeRule)
            {
                if(companyId==CurrentUser.EffectiveCompanyId)
                {
                    node.ContextMenuName = isEnabled ? DISABLEMENUITEMTEXT : ENABLEMENUITEMTEXT;
                    node.PostBack = true;
                }
                else if((companyId==1)&&(parentId ==-1))
                {
                    node.ContextMenuName = COPYTREEMENUNAME;
                    node.PostBack = true;
                    node.DragEnabled = false;
                    node.DropEnabled = false;
                }
            }
            else if (nodeTypeId == RuleTree.nodeRoot)
            {
                node.ContextMenuName = ROOTMENUNAME;
                node.Expanded = true;
                node.DragEnabled = false;
            }
            else
            {
                node.PostBack = true;
                node.DragEnabled = true;
                node.DropEnabled = false;
            }
            node.ImageUrl = GetNodeImage(nodeTypeId);
            string css = GetNodeCss(nodeTypeId, isEnabled);
            if (!String.IsNullOrEmpty(css))
            {
                node.CssClass = css;
            }
            if((isRebinded)&&(CurrentRuleNodeId!=-1))
            {
                if(nodeId == CurrentRuleNodeId)
                {
                    node.Selected = true;
                }
            }
        }
        #endregion

        #region tree event handlers
        protected void rtvRule_NodeDrop(object o, RadTreeNodeEventArgs e)
        {
            RadTreeNode sourceNode = e.SourceDragNode;
            RadTreeNode destNode = e.DestDragNode;
            int srcNodeId = GetTreeNodeAttributeInt(sourceNode, RuleTreeAdmin.IDATTRIBUTE);
            int destNodeId = GetTreeNodeAttributeInt(destNode, RuleTreeAdmin.IDATTRIBUTE);
            int nodeTypeId = GetTreeNodeAttributeInt(sourceNode, RuleTreeAdmin.NODETYPEATTRIBUTE);
            if (nodeTypeId == RuleTree.nodeRule)
            {
                if (RuleEditNode.SetNewParent(srcNodeId, destNodeId))
                {
                    AddControlToUpdate(rtvRule.ID, 0);
                    EditNode(sourceNode, TABPROPERTYID);
                    BindTree(true);
                }
            }
            else
            {
                if (RuleEditNode.ChangeObjectParent(srcNodeId, destNodeId))
                {
                    AddControlToUpdate(rtvRule.ID, 0);
                    CurrentRuleNodeId = destNodeId;
                    BindTree(true);
                }
            }
        }

        protected void rtvRule_NodeContextClick(object o, RadTreeNodeEventArgs e)
        {
            string cmd = e.ContextMenuItemText;
            if (cmd == EDITMENUITEMTEXT)
            {
                EditNode(e.NodeClicked,TABPROPERTYID);
            }
            else if (cmd == ADDCHILDMENUITEMTEXT)
            {
                AddChildNode(e.NodeClicked);
            }
            else if (cmd == ENABLEMENUITEMTEXT)
            {
                EnableNode(e.NodeClicked);
            }
            else if (cmd == DISABLEMENUITEMTEXT)
            {
                DisableNode(e.NodeClicked);

            }
            else if (cmd == DELETEMENUITEMTEXT)
            {
                DeleteNode(e.NodeClicked);
            }
            else if (cmd==COPYTREEMENUITEMTEXT)
            {
                CopyRuleBranch(e.NodeClicked);
            }
        }
        protected void rtvRule_NodeClick(object o, RadTreeNodeEventArgs e)
        {
            EditNode(e.NodeClicked,GetTabId(e.NodeClicked));
        }
        #endregion

        #region tree contextmenu related methods
        private void CreateMenuArray()
        {
            ContextMenu rootMenu = new ContextMenu();
            rootMenu.Name = ROOTMENUNAME;
            rootMenu.Width = MENUITEMWIDTH;
            rootMenu.Items.Add(GetMenuItem(ADDCHILDMENUITEMTEXT));
            contextMenus.Add(rootMenu);
            contextMenus.Add(CreateMenu(ENABLED));
            contextMenus.Add(CreateMenu(!ENABLED));
            ContextMenu copyTree = new ContextMenu();
            copyTree.Name = COPYTREEMENUNAME;
            copyTree.Width = 140;
            copyTree.Items.Add(GetMenuItem(COPYTREEMENUITEMTEXT));
            contextMenus.Add(copyTree);
        }
        private static ContextMenu CreateMenu(bool enabled)
        {
            ContextMenu res = new ContextMenu();
            res.Name = enabled ? DISABLEMENUITEMTEXT : ENABLEMENUITEMTEXT;
            res.Width = MENUITEMWIDTH;
            res.Items.Add(GetMenuItem(EDITMENUITEMTEXT));
            res.Items.Add(GetMenuItem(ADDCHILDMENUITEMTEXT));
            res.Items.Add(GetMenuItem(enabled ? DISABLEMENUITEMTEXT : ENABLEMENUITEMTEXT));
            res.Items.Add(GetMenuItem(DELETEMENUITEMTEXT));
            return res;
        }
        private static ContextMenuItem GetMenuItem(string text)
        {
            ContextMenuItem item = new ContextMenuItem();
            item.Text = text;
            item.PostBack = true;
            return item;
        }
        #endregion

        #endregion

        #region rule editing related methods

        private void EditNode(WebControl node, int tabIndex)
        {
            divEditRule.Visible = true;
            CurrentEditNode = null;
            int nodeType = GetTreeNodeAttributeInt(node, RuleTreeAdmin.NODETYPEATTRIBUTE);
            if ((nodeType != RuleTreeAdmin.nodeRule) && (nodeType != RuleTreeAdmin.nodeRoot))
            {
                CurrentRuleNodeId = GetTreeNodeAttributeInt(node, RuleTreeAdmin.PARENTIDATTRIBUTE);
            }
            else
            {
                CurrentRuleNodeId = GetTreeNodeAttributeInt(node, RuleTreeAdmin.IDATTRIBUTE);
            }
            CurrentEditNode.IsReadOnly = CurrentEditNode.CompanyId != CurrentUser.EffectiveCompanyId;
            if(CurrentEditNode.IsReadOnly)
            {
                DisableInputs();
            }
            EditorStep = -1;
            LoadNodeData();
            SetCurrentTab(tabIndex);
        }
        private void DisableInputs()
        {
            cbStatus.Enabled = false;
            rcbCategory.Enabled = false;
            trEditor.Visible = false;
            trAddExpression.Visible = false;
            btnSaveProperties.Visible = false;
            tbComments.Enabled = false;
            trAddNewField.Visible = false;
            trFields.Visible = false;
            trAddNewChecklist.Visible = false;
            trChecklist.Visible = false;
            trAddNewCondition.Visible = false;
            trCondition.Visible = false;
            trAddNewAlert.Visible = false;
            trAlert.Visible = false;
            trAddNewDocument.Visible = false;
            trDocument.Visible = false;
            trAddNewData.Visible = false;
            trData.Visible = false;
        }

        private void SetCurrentTab(int tabIndex)
        {
            currentruletab.Value = tabIndex.ToString();
            RadAjaxManager1.ResponseScripts.Add(String.Format(SETTABJS,tabIndex));
            tabsRuleEdit.SelectedIndex = tabIndex;
            mpRuleEdit.SelectedIndex = tabsRuleEdit.SelectedIndex;
        }
        private static int GetTreeNodeAttributeInt(WebControl node, string name)
        {
            int res = -1;
            try
            {
                res = int.Parse(node.Attributes[name]);
            }
            catch
            {
            }
            return res;
        }
        private static int GetTabId(WebControl node)
        {
            int res = TABPROPERTYID;
            int nodeType = GetTreeNodeAttributeInt(node, RuleTreeAdmin.NODETYPEATTRIBUTE);
            switch(nodeType)
            {
                case RuleTree.nodeRule:
                    res = TABPROPERTYID;
                    break;
                case RuleTree.FIELDBIT:
                    res = TABSHOWFIELDSID;
                    break;
                case RuleTree.CONDITIONBIT:
                    res = TABCONDITIONSID;
                    break;
                case RuleTree.DOCUMENTBIT :
                    res = TABDOCUMENTSID;
                    break;
                case RuleTree.CHECKLISTBIT :
                    res = TABCHECKLISTSID;
                    break;
                case RuleTree.EVENTBIT :
                case RuleTree.ALERTBIT :
                    res = TABALERTSID;
                    break;
                case RuleTree.DATABIT:
                    res = TABDATAID;
                    break;
            }
            return res;
        }
        private void AddChildNode(WebControl node)
        {
            divEditRule.Visible = true;
            int parentId = GetTreeNodeAttributeInt(node, RuleTreeAdmin.IDATTRIBUTE);
            EditorStep = -1;
            CurrentEditNode = null;
            CurrentRuleNodeId = -1;
            CurrentEditNode.ParentId = parentId;
            CurrentEditNode.CompanyId = CurrentUser.EffectiveCompanyId;
            LoadNodeData();
            SetCurrentTab(TABPROPERTYID);
        }
        private void LoadNodeData()
        {
            BindPropertiesTab();
            BindFieldsTab();
            BindConditionsTab();
            BindDocumentTab();
            BindCheckListTab();
            BindAlertTab();
            BindDataTab();
        }
        private void EnableNode(WebControl node)
        {
            CurrentRuleNodeId = GetTreeNodeAttributeInt(node, RuleTreeAdmin.IDATTRIBUTE);
            RuleEditNode.Enable(CurrentRuleNodeId);
            AddControlToUpdate(rtvRule.ID,0);
            BindTree(true);
        }
        private void DisableNode(WebControl node)
        {
            CurrentRuleNodeId = GetTreeNodeAttributeInt(node, RuleTreeAdmin.IDATTRIBUTE);
            RuleEditNode.Disable(CurrentRuleNodeId);
            AddControlToUpdate(rtvRule.ID, 0);
            BindTree(true);
        }
        private void DeleteNode(WebControl node)
        {
            int nodeId = GetTreeNodeAttributeInt(node, RuleTreeAdmin.IDATTRIBUTE);
            int parentId = GetTreeNodeAttributeInt(node, RuleTreeAdmin.PARENTIDATTRIBUTE);
            if(RuleEditNode.Delete(nodeId))
            {
                CurrentEditNode = null;
                CurrentRuleNodeId = -1;
                CurrentRuleNodeId = parentId;
                divEditRule.Visible = false;
                AddControlToUpdate(rtvRule.ID,0);
                BindTree(true);
            }
        }
        private void CopyRuleBranch(WebControl node)
        {
            int nodeId = GetTreeNodeAttributeInt(node, RuleTreeAdmin.IDATTRIBUTE);
            if (RuleEditNode.CopyBranch(nodeId,CurrentUser.EffectiveCompanyId))
            {
                CurrentEditNode = null;
                CurrentRuleNodeId = -1;
                AddControlToUpdate(rtvRule.ID, 0);
                BindTree(true);
            }
        }

        #endregion

        #region ruletabs related methods

        private void SetTabStyle(int tabId, bool isEmpty)
        {
            tabsRuleEdit.Tabs[tabId].Enabled = CurrentEditNode.HasUnits;
            if (isEmpty)
            {
                tabsRuleEdit.Tabs[tabId].Attributes.Add("style", EMPTYTABCOLOR);
                tabsState[tabId] = -1;
            }
            else
            {
                tabsRuleEdit.Tabs[tabId].Attributes.Add("style", "");
                tabsState[tabId] = 1;
            }
        }
        private void AddControlToUpdate(string controlId, int index)
        {
            if (controlId == rtvRule.ID)
            {
                RemoveFromCache();
            }
            AjaxSetting item = RadAjaxManager1.AjaxSettings[index];
            item.UpdatedControls.Add(new AjaxUpdatedControl(controlId, ""));
            if (rtvRule.ID == controlId)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format(JSUPDATECONTENT,GetTabsState()));
            }
        }
        private void RemoveFromCache()
        {
            if(CurrentUser.CompanyId==1)
            {
                CurrentPage.RemoveFromCacheByPartialKey("rtpublic_");
            }
            else
            {
                CurrentPage.RemoveFromCacheByKey(String.Format(RTPUBLICKEY,CurrentUser.CompanyId));
            }
        }
        private string GetTabsState()
        {
            string res = String.Empty;
            for(int i=0;i<tabsState.Length;i++)
            {
                if(i>0)
                {
                    res += ",";
                }
                res += tabsState[i].ToString();
            }
            return res;
        }
        #region property tab related methods
        private void BindPropertiesTab()
        {
            lblRuleName.Text = CurrentEditNode.RuleName;
            lblRuleExpression.Text = CurrentEditNode.RuleExpression;
            cbStatus.Checked = CurrentEditNode.IsEnabled;
            BindNodeCategory();
            tbComments.Text = CurrentEditNode.Comments;
            BindExpressionEditor();
        }
        private void BindNodeCategory()
        {
            rcbCategory.Visible = CurrentEditNode.IsRoot;
            lblCategory.Visible = !rcbCategory.Visible;
            if (rcbCategory.Visible)
            {
                rcbCategory.DataSource = DvCategory;
                rcbCategory.DataTextField = RuleTree.NAMEFIELDNAME;
                rcbCategory.DataValueField = RuleTree.IDFIELDNAME;
                rcbCategory.DataBind();

                if (CurrentEditNode.CategoryId > 0)
                {
                    rcbCategory.SelectedValue = CurrentEditNode.CategoryId.ToString();
                }
            }
            else
            {
                lblCategory.Text = CurrentEditNode.CategoryName;
            }

        }
        #region rule code editor related methods
        private void BindExpressionEditor()
        {
            ResetEditor();
            BindExpressionGrid();
            InitEditor();
            BindEditorDictionaries();
            ExecuteEditorStep();
        }
        private void BindExpressionGrid()
        {
            counter = 0;
            DataView dv = CurrentEditNode.DvUnits;
            if(dv!=null)
            {
                UnitsCount = dv.Count;
                gProperties.PageIndex = GetPageIndex("gProperties");
                gProperties.DataSource = dv;
                gProperties.DataBind();
            }
        }
        private void ResetEditor()
        {
            field = new Field();
            CurrentPage.StoreObject(field, Constants.FIELDOBJECT);
            for (int i = 0; i < editorDdls.Length; i++)
            {
                StoreDdlValue(editorDdls[i], 0);
            }
            EditorStep = -1;
        }
        private void InitEditor()
        {
            if (EditorStep == -1)
            {
                EditorStep = gProperties.Rows.Count > 0 ? 0 : 1;
                tbValue.Text = String.Empty;
                field = new Field();
                CurrentPage.StoreObject(field, Constants.FIELDOBJECT);
            }
            else
            {
                field = GetField();
            }
            if ((EditorStep < 0) || (EditorStep > 5))
            {
                EditorStep = 0;
            }
            btnAddExpression.Enabled = false;
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
        }
        private void BindEditorDictionaries()
        {
            BindLogicalOps();
            BindGroups();
            BindFields();
            BindCompOps();
            BindFieldType();
        }
        private void BindLogicalOps()
        {
            ddlLogicalOp.DataSource = CurrentPage.GetDictionary(LOGICALOPERATIONSTABLE);
            ddlLogicalOp.DataTextField = Field.COMPOPNAMEFIELDNAME;
            ddlLogicalOp.DataValueField = Field.IDFIELDNAME;
            ddlLogicalOp.DataBind();
            AddEmptyItem(ddlLogicalOp);
            ddlLogicalOp.SelectedValue = RestoreDdlSelectedValue(editorDdls[0]).ToString();
        }
        private void BindGroups()
        {
            DataView dv = CurrentPage.GetDictionary(FIELDGROUPTABLE);
            dv.Sort = Field.GROUPNAMEFIELDNAME;
            ddlGroup.DataSource = dv;
            ddlGroup.DataTextField = Field.GROUPNAMEFIELDNAME;
            ddlGroup.DataValueField = Field.IDFIELDNAME;
            ddlGroup.DataBind();
            AddEmptyItem(ddlGroup);
            ddlGroup.SelectedValue = RestoreDdlSelectedValue(editorDdls[1]).ToString();
        }
        private void BindFields()
        {
            if (EditorStep>1)
            {
                DataView dvFields = CurrentPage.GetDictionary(MORTGAGEFIELDSTABLE);
                dvFields.RowFilter = String.Format(FIELDSFILTER, RestoreDdlSelectedValue(editorDdls[1]));
                dvFields.Sort = Field.DESCRIPTIONFIELDNAME;
                ddlField.DataSource = dvFields;
                ddlField.DataTextField = Field.DESCRIPTIONFIELDNAME;
                ddlField.DataValueField = Field.IDFIELDNAME;
                ddlField.DataBind();
            }
            AddEmptyItem(ddlField);
            ddlField.SelectedValue = RestoreDdlSelectedValue(editorDdls[2]).ToString();
        }
        private void BindCompOps()
        {
            if(EditorStep>2)
            {
                field = new Field(RestoreDdlSelectedValue(editorDdls[2]));
                CurrentPage.StoreObject(field, Constants.FIELDOBJECT);
                DataView dvComp = CurrentPage.GetDictionary(COMPAREOPTABLENAME);
                if ((field.TypeId == (int)Field.MortgageProfileFieldType.String) || (field.TypeId == (int)Field.MortgageProfileFieldType.Boolean))
                {
                    dvComp.RowFilter = String.Format(COMPOPFILTER, "1");
                }
                ddlCompareOp.DataSource = dvComp;
                ddlCompareOp.DataTextField = Field.COMPOPNAMEFIELDNAME;
                ddlCompareOp.DataValueField = Field.IDFIELDNAME;
                ddlCompareOp.DataBind();
            }
            AddEmptyItem(ddlCompareOp);
            ddlCompareOp.SelectedValue = RestoreDdlSelectedValue(editorDdls[3]).ToString();
        }
        private void BindFieldType()
        {
            ddlFieldType.Items.Add(new ListItem(VALUETYPETEXT, VALUETYPEVALUE.ToString()));
            ddlFieldType.Items.Add(new ListItem(FIELDTYPETEXT, FIELDTYPEVALUE.ToString()));
            AddEmptyItem(ddlFieldType);
            ddlFieldType.SelectedValue = RestoreDdlSelectedValue(editorDdls[4]).ToString();
        }
        private static void AddEmptyItem(ListControl ddl)
        {
            ddl.Items.Insert(0, new ListItem(NOTSELECTED, NOTSELECTEDVALUE.ToString()));
        }
        private void ExecuteEditorStep()
        {
            switch (EditorStep)
            {
                case 0:
                    cbNot.Checked = false;
                    ddlLogicalOp.SelectedValue = NOTSELECTEDVALUE.ToString();
                    ddlLogicalOp.Enabled = true;
                    btnCancelExpression.Enabled = false;
                    break;
                case 1:
                    ddlGroup.Enabled = true;
                    ddlGroup.SelectedValue = NOTSELECTEDVALUE.ToString();
                    ddlGroup.Focus();
                    btnCancelExpression.Enabled = UnitsCount > 0;
                    break;
                case 2:
                    ddlField.Enabled = true;
                    ddlField.Focus();
                    btnCancelExpression.Enabled = true;
                    break;
                case 3:
                    ddlCompareOp.Enabled = true;
                    ddlCompareOp.Focus();
                    break;
                case 4:
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
                            if (field.TypeId == (int)Field.MortgageProfileFieldType.Integer)
                            {
                                mtb.Type = NumericType.Number;
                                mtb.NumberFormat.DecimalDigits = 0;
                            }
                            else if (field.TypeId == (int)Field.MortgageProfileFieldType.Float)
                            {
                                mtb.Type = NumericType.Number;
                                mtb.NumberFormat.DecimalDigits = 2;
                            }
                            else if (field.TypeId == (int)Field.MortgageProfileFieldType.Decimal)
                            {
                                mtb.Type = NumericType.Currency;
                            }
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
                        DataView dv = field.GetFieldList();
                        dv.Sort = Field.FIELDNAMEFIELDNAME;
                        ddlDictionary.DataSource = dv;
                        ddlDictionary.DataTextField = Field.FIELDNAMEFIELDNAME;
                        ddlDictionary.DataValueField = Field.IDFIELDNAME;
                        ddlDictionary.DataBind();
                        ddlDictionary.Focus();
                    }
                    btnAddExpression.Enabled = true;
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
                        case Field.MortgageProfileFieldType.Decimal:
                            txt = MONEYLABELTEXT;
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
                    case Field.MortgageProfileFieldType.Decimal:
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

        #region rule code editor event handlers
        protected void btnCancelExpression_Click(object sender, EventArgs e)
        {
            EditorStep--;
            StoreDdlValue(editorDdls[EditorStep], 0);
            InitEditor();
            BindEditorDictionaries();
            ExecuteEditorStep();
        }
        protected void btnAddExpression_Click(object sender, EventArgs e)
        {
            InitEditor();
            BindEditorDictionaries();
            if (ddlDictionary.Visible || (ddlFieldType.SelectedValue == FIELDTYPEVALUE.ToString()))
            {
                if((field.IsDictionaryField)||(ddlFieldType.SelectedValue == FIELDTYPEVALUE.ToString()))
                {
                    if (ddlFieldType.SelectedValue == FIELDTYPEVALUE.ToString())
                    {
                        ddlDictionary.DataSource = field.GetFieldList();
                        ddlDictionary.DataTextField = Field.FIELDNAMEFIELDNAME;
                        ddlDictionary.DataValueField = Field.IDFIELDNAME;
                        ddlDictionary.DataBind();
                    }
                    else
                    {
                        ddlDictionary.DataSource = field.GetDictionaryList();
                        ddlDictionary.DataTextField = field.FieldName;
                        ddlDictionary.DataValueField = Field.IDFIELDNAME;
                        ddlDictionary.DataBind();
                    }
                }
                else
                {
                    ddlDictionary.Items.Clear();
                    ddlDictionary.Items.Add(new ListItem(NOTSELECTED, (-1).ToString()));
                    ddlDictionary.Items.Add(new ListItem(YES, YESVALUE));
                    ddlDictionary.Items.Add(new ListItem(NO, NOVALUE));
                }
                ddlDictionary.SelectedValue = GetValueFromPostDataInt("$ddlDictionary").ToString();
            }
            if (ValidateEditor())
            {
                RuleUnit ru = new RuleUnit();
                ru.PropertyName = ddlField.SelectedItem.Text;
                ru.LogicalNot = cbNot.Checked;
                ru.FieldId = field.ID;
                ru.LiteralValue = ddlFieldType.SelectedValue == VALUETYPEVALUE.ToString();
                if (!ru.LiteralValue)
                {
                    ru.ReferanceId = int.Parse(ddlDictionary.SelectedValue);
                }
                else
                {
                    ru.DataValue = ddlDictionary.SelectedValue;
                }
                if (UnitsCount > 0)
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
                    ru.DataValue = mtb.Text;
                }
                else
                {
                    ru.DataValue = tbValue.Text;
                }
                if((CurrentEditNode.Id==-1)&&(CurrentEditNode.ParentId==-1))
                {
                    CurrentEditNode.CategoryId = GetDdlSelectedValue(rcbCategory.ClientID.Replace("_","$")+"_value");
                    if(CurrentEditNode.CategoryId==-1)
                    {
                        CurrentEditNode.CategoryName = GetPostValue(rcbCategory.ClientID.Replace("_", "$") + "_text");
                        isNewCategory = true;
                    }
                }
                RuleEditNode ruleNode = CurrentEditNode;
                if (ruleNode.AddUnit(ru))
                {
                    CurrentRuleNodeId = ruleNode.Id;
                    CurrentEditNode = ruleNode;
                    tabsState[TABPROPERTYID] = CurrentEditNode.HasUnits ? 1 : -1;
                    AddControlToUpdate(rtvRule.ID,3);
                    ReloadCategory();
                    UpdateAfterExpressionChanges();
                }
            }
            else
            {
                BindExpressionGrid();
                ExecuteEditorStep();
            }
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
        private int GetValueFromPostDataInt(string controlName)
        {
            int res = 0;
            string data = GetValueFromPostData(controlName);
            if(!String.IsNullOrEmpty(data))
            {
                try
                {
                    res = int.Parse(data);
                }
                catch
                {
                }
            }
            return res;
        }
        private string GetValueFromPostData(string controlName)
        {
            string res = String.Empty;
            for(int i=0;i<Page.Request.Form.AllKeys.Length;i++)
            {
                string key = Page.Request.Form.AllKeys[i];
                if(key.EndsWith(controlName))
                {
                    res = Page.Request.Form[key];
                    break;
                }
            }
            return res;
        }
        private void UpdateAfterExpressionChanges()
        {
            BindExpressionGrid();
            ResetEditor();
            InitEditor();
            ExecuteEditorStep();
            lblRuleName.Text = CurrentEditNode.RuleName;
            lblRuleExpression.Text = CurrentEditNode.RuleExpression;
            BindNodeCategory();
            BindTree(true);
        }
        protected void btnSaveProperties_Click(object sender, EventArgs e)
        {
            if(Validate())
            {
                if (rcbCategory.Visible)
                {
                    if (String.IsNullOrEmpty(rcbCategory.SelectedValue))
                    {
                        CurrentEditNode.CategoryId = -1;
                        CurrentEditNode.CategoryName = rcbCategory.Text;
                        isNewCategory = true;
                    }
                    else
                    {
                        CurrentEditNode.CategoryId = int.Parse(rcbCategory.SelectedValue);
                    }
                }
                CurrentEditNode.IsEnabled = cbStatus.Checked;
                CurrentEditNode.Comments = tbComments.Text;
                RuleEditNode ruleNode = CurrentEditNode;
                if (ruleNode.Save())
                {
                    CurrentRuleNodeId = ruleNode.Id;
                    CurrentEditNode = ruleNode;
                    ReloadCategory();
                    UpdateAfterpropertySave();
                }
            }
        }
        protected void btnCancelProperties_Click(object sender, EventArgs e)
        {
            CurrentEditNode = null;
            CurrentRuleNodeId = -1;
            divEditRule.Visible = false;
        }
        private void ReloadCategory()
        {
            if (isNewCategory)
            {
                dvCategory = null;
                BindCategory();
                AddControlToUpdate(ddlCategory.ID, 3);
            }
        }
        private void UpdateAfterpropertySave()
        {
            AddControlToUpdate(rtvRule.ID,4);
            BindExpressionGrid();
            ResetEditor();
            InitEditor();
            BindEditorDictionaries();
            ExecuteEditorStep();
            lblRuleName.Text = CurrentEditNode.RuleName;
            lblRuleExpression.Text = CurrentEditNode.RuleExpression;
            BindTree(true);
        }
        private bool Validate()
        {
            bool res = true;
            if (rcbCategory.Visible)
            {
                if ((rcbCategory.SelectedValue == (-1).ToString()) && (String.IsNullOrEmpty(rcbCategory.Text)))
                {
                    lblErrCategory.Text = FIELDNEEDEDMESSAGE;
                    res = false;
                }
            }
            return res;
        }
        #endregion

        #region rule expression grid events
        protected string GetLogicalOp(Object o)
        {
            string result = " ";
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                if(counter > 0)
                {
                    result = row["logicalop"].ToString();
                }
                counter++;
            }
            return result;
        }
        protected void gProperties_ItemCommand(object source, GridViewCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            int id = int.Parse(e.CommandArgument.ToString());
            switch (cmd)
            {
                case DELETECOMMAND:
                    RuleUnit ru = new RuleUnit();
                    ru.Id = id;
                    if (CurrentEditNode.DeleteUnit(ru))
                    {
                        tabsState[TABPROPERTYID] = CurrentEditNode.HasUnits ? 1 : -1;
                        AddControlToUpdate(rtvRule.ID,2);
                        AddControlToUpdate(pvGeneral.ID, 2);
                        BindEditorDictionaries();
                        UpdateAfterExpressionChanges();
                    }
                    break;
                case PAGECOMMAND:
                    SetPageIndex("gProperties",id-1);
                    gProperties.PageIndex = id-1;
                    BindExpressionGrid();
                    break;
                default:
                    return;
            }
        }
        #endregion

        #endregion


        #region Show fields tab related methods
        private void BindFieldsTab()
        {
            ShowFieldStep = 0;
            StoreDdlValue(fieldsDdls[ShowFieldStep], 0);
            BindFieldsGrid();
            BindFieldsDictionaries();
            ExecuteShowFieldStep();
            SetTabStyle(TABSHOWFIELDSID, CurrentEditNode.DvFields.Count==0);
        }
        private void BindFieldsGrid()
        {
            gFields.PageIndex = GetPageIndex("gFields");
            gFields.DataSource = CurrentEditNode.DvFields;
            gFields.DataBind();
        }
        private void BindFieldsDictionaries()
        {
            DataView dv = CurrentPage.GetDictionary(FIELDGROUPTABLE);
            dv.Sort = "name";
            ddlGroupField.DataSource = dv;
            ddlGroupField.DataTextField = Field.GROUPNAMEFIELDNAME;
            ddlGroupField.DataValueField = Field.IDFIELDNAME;
            ddlGroupField.DataBind();
            AddEmptyItem(ddlGroupField);
            ddlGroupField.SelectedValue = RestoreDdlSelectedValue(fieldsDdls[0]).ToString();
            if (ShowFieldStep > 0)
            {
                DataView dvFields = CurrentPage.GetDictionary(MORTGAGEFIELDSTABLE);
                dvFields.RowFilter = String.Format(FIELDSFILTER, RestoreDdlSelectedValue(fieldsDdls[0]));
                dvFields.Sort = Field.DESCRIPTIONFIELDNAME;
                ddlFieldField.DataSource = dvFields;
                ddlFieldField.DataTextField = Field.DESCRIPTIONFIELDNAME;
                ddlFieldField.DataValueField = Field.IDFIELDNAME;
                ddlFieldField.DataBind();
            }
            AddEmptyItem(ddlFieldField);
        }
        private void ExecuteShowFieldStep()
        {
            ddlGroupField.Enabled = false;
            ddlFieldField.Enabled = false;
            btnBackField.Enabled = false;
            btnAddField.Enabled = false;
            switch (ShowFieldStep)
            {
                case 0:
                    ddlGroupField.Enabled = true;
                    ddlGroupField.Focus();
                    break;
                case 1:
                    ddlFieldField.Enabled = true;
                    ddlFieldField.Focus();
                    btnBackField.Enabled = true;
                    btnAddField.Enabled = true;
                    break;
            }
        }

        #region show fields grid related methods
        protected void gFields_ItemCommand(object source, GridViewCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            int id = int.Parse(e.CommandArgument.ToString());
            switch (cmd)
            {
                case DELETECOMMAND:
                    CurrentEditNode.DeleteObject(id,Rule.FIELDOBJECTTYPEID);
                    SetTabStyle(TABSHOWFIELDSID, CurrentEditNode.DvFields.Count == 0);
                    AddControlToUpdate(rtvRule.ID,7);
                    BindTree(true);
                    break;
                case PAGECOMMAND:
                    SetPageIndex("gFields",id-1);
                    gFields.PageIndex = id-1;
                    break;
                default:
                    return;
            }
            BindFieldsGrid();
        }
        #endregion

        #region show fields tab evant handlers
        protected void btnBackField_Click(object sender, EventArgs e)
        {
            ShowFieldStep--;
            StoreDdlValue(fieldsDdls[ShowFieldStep], 0);
            BindFieldsDictionaries();
            ExecuteShowFieldStep();
        }
        protected void btnAddField_Click(object sender, EventArgs e)
        {
            if(ddlFieldField.SelectedValue==0.ToString())
            {
                lblErrField.Text = FIELDNEEDEDMESSAGE;
            }
            else
            {
                CurrentEditNode.AddObject(int.Parse(ddlFieldField.SelectedValue),Rule.FIELDOBJECTTYPEID);
                SetTabStyle(TABSHOWFIELDSID, CurrentEditNode.DvFields.Count == 0);
                ShowFieldStep = 1;
                ExecuteShowFieldStep();
                BindFieldsGrid();
                AddControlToUpdate(rtvRule.ID,6);
                BindTree(true);
            }
        }
        #endregion
        #endregion

        #region conditions tab related methods
        private void BindConditionsTab()
        {
            BindAuthorityLevels();
            cbClosingInstruction.Attributes.Add(ONCLICKATTRIBUTE, String.Format("SetVisibility(this,'{0}','{1}');",trCondType.ClientID,trCondCategory.ClientID));
            BindConditionType();
            BindConditionCategory();
            BindConditionsGrid();
            SetTabStyle(TABCONDITIONSID, CurrentEditNode.DvConditions.Count == 0);
        }
        private void BindConditionType()
        {
            ddlType.DataSource = Condition.GetTypeList();
            ddlType.DataTextField = "name";
            ddlType.DataValueField = "id";
            ddlType.DataBind();
            ddlType.Items.Insert(0,new ListItem("- Select -","0"));
        }
        private void BindConditionCategory()
        {
            ddlConditionCategory.DataSource = Condition.GetCategoryList();
            ddlConditionCategory.DataTextField = "name";
            ddlConditionCategory.DataValueField = "id";
            ddlConditionCategory.DataBind();
            ddlConditionCategory.Items.Insert(0, new ListItem("- Select -", "0"));
        }
        private void BindConditionsGrid()
        {
            gConditions.PageIndex = GetPageIndex("gConditions");
            gConditions.DataSource = CurrentEditNode.DvConditions;
            gConditions.DataBind();
        }
        private void BindAuthorityLevels()
        {
            ddlRole.DataSource = Role.GetAuthorityList();
            ddlRole.DataTextField = Role.NAMEFIELDNAME;
            ddlRole.DataValueField = Role.IDFIELDNAME;
            ddlRole.DataBind();
        }

        private void BindRoles()
        {
            ddlRole.DataSource = Role.GetList(true, true, true, CurrentUser.EffectiveCompanyId);
            ddlRole.DataTextField = Role.NAMEFIELDNAME;
            ddlRole.DataValueField = Role.IDFIELDNAME;
            ddlRole.DataBind();
        }
        private bool CheckClosingInstruction(int id)
        {
            bool res = false;
            CurrentEditNode.DvConditions.RowFilter= String.Format("id={0}",id);
            if (CurrentEditNode.DvConditions.Count == 1)
            {
                res = bool.Parse(CurrentEditNode.DvConditions[0]["isclosinginstruction"].ToString());
            }
            CurrentEditNode.DvConditions.RowFilter = "";
            return res;
        }
        protected static string GetConditionDetails(Object o,int len)
        {
            string res = String.Empty;
            DataRowView row = (DataRowView)o;
            if (row != null)
            {
                res = row["Detail"].ToString();
                if(res.Length>len)
                {
                    res = res.Substring(0, len);
                }
            }
            return res;
        }

        protected void gConditions_ItemCommand(object source, GridViewCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            int id = int.Parse(e.CommandArgument.ToString());
            switch (cmd)
            {
                case EDITCONDITIONCOMMAND:
                    BindRoles();
                    BindConditionType();
                    BindConditionCategory();
                    CurrentConditionId = id;
                    IsClosingInstruction = CheckClosingInstruction(id);
                    if (IsClosingInstruction)
                    {
                        RuleClosingInstruction rc = new RuleClosingInstruction(id);
                        tbTitle.Text = rc.Title;
                        tbDetail.Text = rc.Detail;
                        ddlRole.SelectedValue = rc.RoleId.ToString();
                        cbClosingInstruction.Checked = true;
                    }
                    else 
                    {
                        RuleCondition rc = new RuleCondition(id);
                        tbTitle.Text = rc.Title;
                        tbDetail.Text = rc.Detail;
                        ddlRole.SelectedValue = rc.RoleId.ToString();
                        ddlType.SelectedValue = rc.TypeId.ToString();
                        ddlConditionCategory.SelectedValue = rc.CategoryId.ToString();
                        cbClosingInstruction.Checked = false;
                    }
                    cbClosingInstruction.Enabled = false;
                    AddControlToUpdate(pnlCondition.ID, 9);
                    break;
                case DELETECOMMAND:
                    CurrentEditNode.DeleteObject(id, Rule.CONDITIONCLOSINGINSTRUCTIONOBJECTTYPEID);
                    SetTabStyle(TABCONDITIONSID, CurrentEditNode.DvConditions.Count == 0);
                    BindConditionsGrid();
                    AddControlToUpdate(rtvRule.ID, 9);
                    BindTree(true);
                    break;
                case PAGECOMMAND:
                    SetPageIndex("gConditions",id-1);
                    gConditions.PageIndex = id-1;
                    BindConditionsGrid();
                    break;
                default:
                    return;
            }
        }
        protected void btnSaveCondition_Click(object sender, EventArgs e)
        {
            if(ValidateCondition())
            {
                bool res;
                bool isClosingInstruction = CurrentConditionId>0?IsClosingInstruction:cbClosingInstruction.Checked;
                if (isClosingInstruction)
                {
                    RuleClosingInstruction rc = new RuleClosingInstruction();
                    rc.Id = CurrentConditionId;
                    rc.Detail = tbDetail.Text;
                    rc.Title = tbTitle.Text;
                    rc.RoleId = int.Parse(ddlRole.SelectedValue);
                    res = CurrentEditNode.AddClosingInstruction(rc);
                }
                else
                {
                    RuleCondition rc = new RuleCondition();
                    rc.Id = CurrentConditionId;
                    rc.Detail = tbDetail.Text;
                    rc.Title = tbTitle.Text;
                    rc.RoleId = int.Parse(ddlRole.SelectedValue);
                    rc.TypeId = int.Parse(ddlType.SelectedValue);
                    rc.CategoryId = int.Parse(ddlConditionCategory.SelectedValue);
                    res = CurrentEditNode.AddCondition(rc);
                }
                if(res)
                {
                    SetTabStyle(TABCONDITIONSID, CurrentEditNode.DvConditions.Count == 0);
                    CurrentConditionId = -1;
                    tbDetail.Text = "";
                    tbTitle.Text = "";
                    ddlRole.SelectedValue = "0";
                    ddlType.SelectedValue = "0";
                    ddlConditionCategory.SelectedValue = "0";
                    cbClosingInstruction.Checked = false;
                    BindConditionsGrid();
                    AddControlToUpdate(rtvRule.ID,8);
                    AddControlToUpdate(pnlConditionsGrid.ID, 8);
                    BindTree(true);
                }
            }
        }
        private bool ValidateCondition()
        {
            lblConditionTitleErr.Text = String.Empty;
            lblConditionDetailErr.Text = String.Empty;
            lblConditionRoleErr.Text = String.Empty;
            lblConditionTypeErr.Text = String.Empty;
            lblConditionCategoryErr.Text = String.Empty;
            bool res = true;
            if(String.IsNullOrEmpty(tbTitle.Text))
            {
                res = false;
                lblConditionTitleErr.Text = FIELDNEEDEDMESSAGE;
            }
            if(String.IsNullOrEmpty(tbDetail.Text))
            {
                res = false;
                lblConditionDetailErr.Text = FIELDNEEDEDMESSAGE;
            }
            if(int.Parse(ddlRole.SelectedValue)==0)
            {
                res = false;
                lblConditionRoleErr.Text = FIELDNEEDEDMESSAGE;
            }
            if(!cbClosingInstruction.Checked)
            {
                if (int.Parse(ddlType.SelectedValue) == 0)
                {
                    res = false;
                    lblConditionTypeErr.Text = FIELDNEEDEDMESSAGE;
                }
                if (int.Parse(ddlConditionCategory.SelectedValue) == 0)
                {
                    res = false;
                    lblConditionCategoryErr.Text = FIELDNEEDEDMESSAGE;
                }
            }

            return res;
        }

        #endregion

        #region checklist tab related methods
        private void BindCheckListTab()
        {
            CurrentCheckListId = -1;
            BindRepeater();
            BindCheckListGrid();
            SetTabStyle(TABCHECKLISTSID, CurrentEditNode.DvCheckLists.Count == 0);
        }
        private void BindRepeater()
        {
            string title;
            rpChecklist.DataSource = RuleEditNode.GetCheckList(CurrentCheckListId, out title);
            rpChecklist.DataBind();
            tbclTitle.Text = title;
        }
        private void BindCheckListGrid()
        {
            gCheckList.PageIndex = GetPageIndex("gCheckList");
            gCheckList.DataSource = CurrentEditNode.DvCheckLists;
            gCheckList.DataBind();
        }
        protected static string GetCheckListName(object item, string fieldname)
        {
            DataRowView dr = item as DataRowView;
            string res = String.Empty;
            if(dr!=null)
            {
                res = String.Format(CHECKLISTNAME, dr[fieldname]);
            }
            return res;
        }
        protected void gCheckList_ItemCommand(object source, GridViewCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            int id = int.Parse(e.CommandArgument.ToString());
            switch (cmd)
            {
                case EDITCHECKLISTCOMMAND:
                    CurrentCheckListId = id;                    
                    BindRepeater();
                    AddControlToUpdate(pnlCheckList.ID, 13);
                    break;
                case DELETECOMMAND:
                    CurrentEditNode.DeleteObject(id, Rule.CHECKLISTOBJECTTYPEID);
                    SetTabStyle(TABCHECKLISTSID, CurrentEditNode.DvCheckLists.Count == 0);
                    CurrentCheckListId = -1;
                    BindRepeater();
                    BindCheckListGrid();
                    SetTabStyle(TABCHECKLISTSID, CurrentEditNode.DvCheckLists.Count == 0);
                    AddControlToUpdate(rtvRule.ID, 13);
                    BindTree(true);
                    break;
                case PAGECOMMAND:
                    SetPageIndex("gCheckList", id - 1);
                    gCheckList.PageIndex = id-1;
                    BindCheckListGrid();
                    break;
                default:
                    return;
            }
        }
        protected void btnSaveCheckList_Click(object sender, EventArgs e)
        {
            bool err;
            string xml = GetDataXml(out err);
            if (!err)
            {
                if (CurrentEditNode.AddCheckList(CurrentCheckListId, xml, tbclTitle.Text))
                {
                    SetTabStyle(TABCHECKLISTSID, CurrentEditNode.DvCheckLists.Count == 0);
                    CurrentCheckListId = -1;
                    BindCheckListGrid();
                    BindRepeater();
                    AddControlToUpdate(rtvRule.ID, 12);
                    AddControlToUpdate(pnlCheckListsGrid.ID, 12);
                    BindTree(true);
                }
            }
        }
        private string GetDataXml(out bool err)
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
                        ((Label)rpChecklist.Items[i].Controls[5]).Text = FIELDNEEDEDMESSAGE;
                        ((TextBox)rpChecklist.Items[i].Controls[3]).Enabled =true;
                    }
                    if (!(cb1 || cb2 || cb3 || cb4))
                    {
                        err = true;
                        ((Label)rpChecklist.Items[i].Controls[15]).Text = FIELDNEEDEDMESSAGE;
                        ((CheckBox)rpChecklist.Items[i].Controls[7]).Enabled = true;
                        ((CheckBox)rpChecklist.Items[i].Controls[9]).Enabled = true;
                        ((CheckBox)rpChecklist.Items[i].Controls[11]).Enabled = true;
                        ((CheckBox)rpChecklist.Items[i].Controls[13]).Enabled=true;
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
        private static void AddCheckBoxValue(XmlDocument d, XmlNode n, string attributename, bool ischecked)
        {
            XmlAttribute a = d.CreateAttribute(attributename);
            a.Value = ischecked ? "1" : "0";
            n.Attributes.Append(a);
        }
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
        #endregion

        #region alerts tab related methods
        private void BindAlertTab()
        {
            CurrentAlertEventId = -1;
            BindAlertGrid();
            SetTabStyle(TABALERTSID, CurrentEditNode.DvAlertEvents.Count == 0);
        }
        private void BindAlertGrid()
        {
            gAlert.PageIndex = GetPageIndex("gAlert");
            gAlert.DataSource = CurrentEditNode.DvAlertEvents;
            gAlert.DataBind();
        }
        protected void gAlert_ItemCommand(object source, GridViewCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            int id = int.Parse(e.CommandArgument.ToString());
            switch (cmd)
            {
                case EDITALERTCOMMAND:
                    CurrentAlertEventId = id;
                    RuleAlert ra = new RuleAlert(id);
                    tbMessage.Text = ra.Message;
                    AddControlToUpdate(pnlAlert.ID, 15);
                    break;
                case DELETECOMMAND:
                    CurrentEditNode.DeleteObject(id, Rule.ALERTEVENTOBJECTTYPEID);
                    SetTabStyle(TABALERTSID, CurrentEditNode.DvAlertEvents.Count == 0);
                    BindAlertGrid();
                    AddControlToUpdate(rtvRule.ID, 15);
                    BindTree(true);
                    break;
                case PAGECOMMAND:
                    SetPageIndex("gAlert",id-1);
                    gAlert.PageIndex = id-1;
                    BindAlertGrid();
                    break;
                default:
                    return;
            }
        }
        protected void btnSaveAlert_Click(object sender, EventArgs e)
        {
            if(ValidateAlert())
            {
                bool res = CurrentEditNode.AddAlert(CurrentAlertEventId, tbMessage.Text);
                if(res)
                {
                    SetTabStyle(TABALERTSID, CurrentEditNode.DvAlertEvents.Count == 0);
                    CurrentAlertEventId = -1;
                    tbMessage.Text = "";
                    BindAlertGrid();
                    AddControlToUpdate(rtvRule.ID, 14);
                    AddControlToUpdate(pnlAlertsGrid.ID, 14);
                    BindTree(true);
                }
            }
        }
        private bool ValidateAlert()
        {
            bool res = true;
            lblAlertMessageErr.Text = "";
            if(String.IsNullOrEmpty(tbMessage.Text))
            {
                res = false;
                lblAlertMessageErr.Text = FIELDNEEDEDMESSAGE;
            }
            return res;
        }
        #endregion

        #region data tab related methods
        private void BindDataTab()
        {
            CurrentDataId = -1;
            BindDataGrid();
            ResetDataEditor();
            BindDataDictionaries();
            SetTabStyle(TABDATAID, CurrentEditNode.DvData.Count == 0);
            InitDataEditor();
            ExecuteDataEditorStep();
        }
        private void InitDataEditor()
        {
            if ((DataStep < 0) || (DataStep > 2))
            {
                DataStep = 0;
            }
            ddlDataGroup.Enabled = false;
            ddlDataField.Enabled = false;
            btnBackData.Enabled = false;
            btnAddData.Enabled = false;
            tbDataValue.Enabled = false;
            ddlDataDictionary.Enabled = false;
            rdpData.Enabled = false;
            rmeData.Enabled = false;
            tbDataValue.Visible = false;
            ddlDataDictionary.Visible = false;
            rdpData.Visible = false;
            rmeData.Visible = false;
        }
        private void ResetDataEditor()
        {
            field = new Field();
            CurrentPage.StoreObject(field, Constants.FIELDOBJECT);
            for (int i = 0; i < dataDdls.Length; i++)
            {
                StoreDdlValue(dataDdls[i], 0);
            }
            DataStep = -1;
        }
        private void ExecuteDataEditorStep()
        {
            switch(DataStep)
            {
                case 0:
                    ddlDataGroup.Enabled = true;
                    ddlDataGroup.Focus();
                    tbDataValue.Visible = true;
                    break;
                case 1:
                    ddlDataField.Enabled = true;
                    ddlDataGroup.Enabled = false;
                    btnBackData.Enabled = true;
                    ddlDataField.Focus();
                    tbDataValue.Visible = true;
                    break;
                case 2:
                    tbDataValue.Visible = false;
                    field = new Field(int.Parse(ddlDataField.SelectedValue));
                    CurrentPage.StoreObject(field, Constants.FIELDOBJECT);
                    SetDataValueFieldLabel();
                    SetDataValueFieldControl();
                    if (field.IsDictionaryField)
                    {
                        BindDataFieldDictionary();
                        ddlDataDictionary.Focus();
                    }
                    ddlDataField.Enabled = false;
                    ddlDataGroup.Enabled = false;
                    btnBackData.Enabled = true;
                    btnAddData.Enabled = true;
                    break;
            }
        }
        private void BindDataFieldDictionary()
        {
            ddlDataDictionary.DataSource = field.GetDictionaryList();
            ddlDataDictionary.DataTextField = field.FieldName;
            ddlDataDictionary.DataValueField = Field.IDFIELDNAME;
            ddlDataDictionary.DataBind();
        }
        private void SetDataValueFieldLabel()
        {
            if (field.IsDictionaryField)
            {
                lblDataValue.Text = DICTIONARYLABELTEXT;
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
                    case Field.MortgageProfileFieldType.Decimal:
                        txt = MONEYLABELTEXT;
                        break;
                    case Field.MortgageProfileFieldType.Boolean:
                        txt = DICTIONARYLABELTEXT;
                        break;
                }
                lblDataValue.Text = txt;
            }
        }
        private void SetDataValueFieldControl()
        {
            Field.MortgageProfileFieldType tp = (Field.MortgageProfileFieldType)field.TypeId;
            if (field.IsDictionaryField)
            {
                ddlDataDictionary.Visible = true;
                ddlDataDictionary.Enabled = true;
                ddlDataDictionary.Focus();
            }
            else
            {
                switch (tp)
                {
                    case Field.MortgageProfileFieldType.Boolean:
                        ddlDataDictionary.Visible = true;
                        ddlDataDictionary.Enabled = true;
                        ddlDataDictionary.Enabled = true;
                        BindYesNo(ddlDataDictionary);
                        ddlDataDictionary.Focus();
                        break;
                    case Field.MortgageProfileFieldType.DateTime:
                        rdpData.Visible = true;
                        rdpData.SelectedDate = DateTime.Now;
                        rdpData.Enabled = true;
                        break;
                    case Field.MortgageProfileFieldType.Integer:
                    case Field.MortgageProfileFieldType.Float:
                    case Field.MortgageProfileFieldType.Decimal:
                        rmeData.Visible = true;
                        rmeData.Enabled = true;
                        rmeData.Mask = (field.TypeId == (int)Field.MortgageProfileFieldType.Integer) ? "##########" : "#########.##";
//                        rmeData.DisplayMask = mtb.Mask;
                        rmeData.DisplayFormatPosition = DisplayFormatPosition.Right;
                        rmeData.Focus();
                        break;
                    default:
                        tbDataValue.Visible = true;
                        tbDataValue.Enabled = true;
                        break;
                }
            }
        }
        private static void BindYesNo(ListControl ddl)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem(NOTSELECTED, (-1).ToString()));
            ddl.Items.Add(new ListItem(YES, YESVALUE));
            ddl.Items.Add(new ListItem(NO, NOVALUE));
        }
        private void BindDataGrid()
        {
            gData.PageIndex = GetPageIndex("gData");
            gData.DataSource = CurrentEditNode.DvData;
            gData.DataBind();
        }
        private void BindDataDictionaries()
        {
            BindDataGroups();
            BindDataFields();
        }
        private void BindDataGroups()
        {
            DataView dv = CurrentPage.GetDictionary(FIELDGROUPTABLE);
            dv.Sort = Field.GROUPNAMEFIELDNAME;
            ddlDataGroup.DataSource = dv;
            ddlDataGroup.DataTextField = Field.GROUPNAMEFIELDNAME;
            ddlDataGroup.DataValueField = Field.IDFIELDNAME;
            ddlDataGroup.DataBind();
            AddEmptyItem(ddlDataGroup);
            ddlDataGroup.SelectedValue = RestoreDdlSelectedValue(dataDdls[0]).ToString();
        }
        private void BindDataFields()
        {
            if (DataStep > 0)
            {
                DataView dvFields = CurrentPage.GetDictionary(MORTGAGEFIELDSTABLE);
                dvFields.RowFilter = String.Format(FIELDSFILTER, RestoreDdlSelectedValue(dataDdls[0]));
                dvFields.Sort = Field.DESCRIPTIONFIELDNAME;
                ddlDataField.DataSource = dvFields;
                ddlDataField.DataTextField = Field.DESCRIPTIONFIELDNAME;
                ddlDataField.DataValueField = Field.IDFIELDNAME;
                ddlDataField.DataBind();
            }
            AddEmptyItem(ddlDataField);
            ddlDataField.SelectedValue = RestoreDdlSelectedValue(dataDdls[1]).ToString();
        }
        protected static string GetFieldValue(Object o)
        {
            string res = String.Empty;
            DataRowView dr = o as DataRowView;
            if(dr!=null)
            {
                res = dr["FieldValue"].ToString();
                if (res.ToLower() == "- select -")
                {
                    res = "null";
                }
            }
            return res;
        }

        protected void gData_ItemCommand(object source, GridViewCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            int id = int.Parse(e.CommandArgument.ToString());
            switch (cmd)
            {
                case DELETECOMMAND:
                    CurrentEditNode.DeleteObject(id, Rule.DATAOBJECTTYPEID);
                    SetTabStyle(TABDATAID, CurrentEditNode.DvData.Count == 0);
                    BindDataGrid();
                    AddControlToUpdate(rtvRule.ID, 17);
                    BindTree(true);
                    break;
                case PAGECOMMAND:
                    SetPageIndex("gData", id - 1);
                    gData.PageIndex = id - 1;
                    BindDataGrid();
                    break;
                default:
                    return;
            }
        }
        protected void btnBackData_Click(object sender, EventArgs e)
        {
            DataStep--;
            StoreDdlValue(dataDdls[DataStep], 0);
            BindDataDictionaries();
            InitDataEditor();
            ExecuteDataEditorStep();
        }
        protected void btnAddData_Click(object sender, EventArgs e)
        {
            BindDataDictionaries();
            if(ValidateData())
            {
                if(CurrentEditNode.AddData(CurrentDataId,field.ID,GetFieldValue()))
                {
                    SetTabStyle(TABDATAID, CurrentEditNode.DvData.Count == 0);
                    BindDataTab();
                    AddControlToUpdate(pnlDataGrid.ID,16);
                    AddControlToUpdate(rtvRule.ID, 16);
                    BindTree(true);
                }
            }
        }
        private string GetFieldValue()
        {
            string res;
            if (field.IsDictionaryField)
            {
                res = ddlDataDictionary.SelectedValue;
            }
            else
            {
                Field.MortgageProfileFieldType tp = (Field.MortgageProfileFieldType)field.TypeId;
                switch (tp)
                {
                    case Field.MortgageProfileFieldType.Boolean:
                        res = GetYesNoValue(ddlDataDictionary); 
                        break;
                    case Field.MortgageProfileFieldType.DateTime:
                        res = rdpData.SelectedDate!=null?Convert.ToDateTime(rdpData.SelectedDate).ToShortDateString():""; 
                        break;
                    case Field.MortgageProfileFieldType.Integer:
                    case Field.MortgageProfileFieldType.Float:
                    case Field.MortgageProfileFieldType.Decimal:
                        res = rmeData.TextWithLiterals;
                        break;
                    default:
                        res = tbDataValue.Text; 
                        break;
                }
            }
            return res;
        }
        private static string GetYesNoValue(ListControl ddl)
        {
            return ddl.Items[ddl.SelectedIndex].Text;
        }

        private static bool ValidateData()
        {
            bool res = true;
            //lblDataErr.Text = "";
            //if (field.IsDictionaryField)
            //{
            //    res = int.Parse(ddlDataDictionary.SelectedValue) != 0;
            //    if (!res)
            //    {
            //        lblDataErr.Text = FIELDNEEDEDMESSAGE;
            //    }
            //    return res;
            //}
            //Field.MortgageProfileFieldType tp = (Field.MortgageProfileFieldType)field.TypeId;
            //switch (tp)
            //{
            //    case Field.MortgageProfileFieldType.String:
            //        if (String.IsNullOrEmpty(tbDataValue.Text))
            //        {
            //            lblDataErr.Text = FIELDNEEDEDMESSAGE;
            //            res = false;
            //        }
            //        break;
            //    case Field.MortgageProfileFieldType.DateTime:
            //        res = !rdpData.IsEmpty;
            //        if (!res)
            //        {

            //            lblDataErr.Text = FIELDNEEDEDMESSAGE;
            //        }
            //        break;
            //    case Field.MortgageProfileFieldType.Integer:
            //    case Field.MortgageProfileFieldType.Float:
            //    case Field.MortgageProfileFieldType.Decimal:
            //        res = !String.IsNullOrEmpty(rmeData.Text);
            //        if (!res)
            //        {
            //            lblDataErr.Text = FIELDNEEDEDMESSAGE;
            //        }
            //        break;
            //    case Field.MortgageProfileFieldType.Boolean:
            //        res = !(ddlDataDictionary.SelectedValue == "-1");
            //        if (!res)
            //        {
            //            lblDataErr.Text = FIELDNEEDEDMESSAGE;
            //        }
            //        break;
            //}
            return res;
        }
        #endregion

        #region document tab related methods
        private void BindDocumentTab()
        {
            CurrentDocumentId = -1;
            cbAppPackage.Checked = false;
            cbClosPackage.Checked = false;
            cbMiscPackage.Checked = false;
            BindDocumentDictionaries();
            BindDocumentGrid();
            SetTabStyle(TABDOCUMENTSID, CurrentEditNode.DvDocuments.Count == 0);
        }
        private void BindDocumentDictionaries()
        {
            DataView dv = CurrentEditNode.GetDocumentList(CurrentDocumentId);
            dv.Sort = "title";
            ddlSelectDoc.DataSource = dv;
            ddlSelectDoc.DataTextField = "Title";
            ddlSelectDoc.DataValueField = "Id";
            ddlSelectDoc.DataBind();
            AddEmptyItem(ddlSelectDoc);
        }
        private void BindDocumentGrid()
        {
            gDocuments.DataSource = null;
            gDocuments.DataBind();
            gDocuments.PageIndex = GetPageIndex("gDocuments");
            gDocuments.DataSource = CurrentEditNode.DvDocuments;
            gDocuments.DataBind(); 
        }
        protected void gDocuments_ItemCommand(object source, GridViewCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            int id = int.Parse(e.CommandArgument.ToString());
            switch (cmd)
            {
                case EDITDOCUMENTCOMMAND:
                    CurrentDocumentId = id;
                    BindDocumentDictionaries();
                    RuleDocument rd = new RuleDocument(id);
                    ddlSelectDoc.SelectedValue = rd.DocumentTemplateId.ToString();
                    cbAppPackage.Checked = rd.IsAppPackage;
                    cbClosPackage.Checked = rd.IsClosingPackage;
                    cbMiscPackage.Checked = rd.IsMiscPackage;
                    AddControlToUpdate(pnlDocument.ID, 19);
                    break;
                case DELETECOMMAND:
                    CurrentEditNode.DeleteObject(id, Rule.DOCUMENTOBJECTTYPEID);
                    SetTabStyle(TABDOCUMENTSID, CurrentEditNode.DvDocuments.Count == 0);
                    BindDocumentGrid();
                    AddControlToUpdate(rtvRule.ID, 19);
                    BindTree(true);
                    break;
                case PAGECOMMAND:
                    SetPageIndex("gDocuments", id - 1);
                    gDocuments.PageIndex = id - 1;
                    BindDocumentGrid();
                    break;
                default:
                    return;
            }
        }
        protected void btnSaveDocument_Click(object sender, EventArgs e)
        {
            if(ValidateDocument())
            {
                if(CurrentEditNode.AddDocument(CurrentDocumentId,int.Parse(ddlSelectDoc.SelectedValue),cbAppPackage.Checked,cbClosPackage.Checked,cbMiscPackage.Checked))
                {
                    SetTabStyle(TABDOCUMENTSID, CurrentEditNode.DvDocuments.Count == 0);
                    AddControlToUpdate(pnlDocumentGrid.ID, 18);
                    AddControlToUpdate(rtvRule.ID, 18);
                }
            }
            BindTree(true);
            BindDocumentTab();
        }
        private bool ValidateDocument()
        {
            bool res = true;
            lblSelectDocErr.Text = String.Empty;
            if(int.Parse(ddlSelectDoc.SelectedValue)==0)
            {
                res = false;
                lblSelectDocErr.Text = FIELDNEEDEDMESSAGE;
            }
            if(!(cbAppPackage.Checked|cbClosPackage.Checked|cbMiscPackage.Checked))
            {
                res = false;
                lblPackageErr.Text = FIELDNEEDEDMESSAGE;
            }
            return res;
        }

        #endregion

        #region grid event handlers
        protected void G_ItemDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgButton = (ImageButton)e.Row.FindControl(GRIDDELETEBUTTONID);
                if (imgButton != null)
                {
                    if(CurrentEditNode.IsReadOnly)
                    {
                        imgButton.Enabled = false;
                        imgButton = (ImageButton)e.Row.FindControl(GRIDEDITBUTTONID);
                        if(imgButton!=null)
                        {
                            imgButton.Enabled = false;
                        }
                    }
                    else
                    {
                        imgButton.Attributes.Add(ONCLICKATTRIBUTE, JSDELETECONFIRM);
                    }
                    
                }
            }
        }
        protected void G_PageIndexChanged(object source, EventArgs e)
        {
        }
        protected void G_PageIndexChanging(Object source, EventArgs e)
        {
        }
        private void SetPageIndex(string gridName,int pageIndex)
        {
            Session[gridName + "_pageindex"] = pageIndex;
        }
        private int GetPageIndex(string gridName)
        {
            int res = 0;
            Object o = Session[gridName + "_pageindex"];
            if(o!=null)
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

        #endregion


        #endregion


        #endregion

    }
}

