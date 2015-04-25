using System;
using System.Collections;
using System.Data;
using System.Xml;
using System.Text;
using BossDev.CommonUtils;


namespace LoanStar.Common
{
    public abstract class RuleTree
    {
        #region constants

        #region sp names
        private const string GETCATEGORY = "GetRulesCategory";
        protected const string GETVISIBILITYFIELDLIST = "GetVisibilityDependableFieldList";        
        protected const string GETVARIABLEVISIBILITYFIELDS = "GetVariableVisibilityFields";
        #endregion

        private const string RULEFILTER = "parentId={0}";
        public const string IDFIELDNAME = "id";
        protected const string RULEIDFIELDNAME = "ruleid";
        public const string NAMEFIELDNAME = "name";


        public const int FIELDBIT = 0x1;
        public const int CONDITIONBIT = 0x2;
        public const int TASKBIT = 0x4;
        public const int DOCUMENTBIT = 0x8;
        public const int CHECKLISTBIT = 0x10;
        public const int EVENTBIT = 0x20;
        public const int ALERTBIT = 0x40;
        public const int DATABIT = 0x80;
        public const int CLOSINGINSTRACTIONBIT = 0x100;

        public const int nodeRoot = -1;
        public const int nodeRule = 0;

        #endregion

        protected static DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);

        #region fields
        protected int lenderId;
        protected DataSet ds;
        protected ArrayList roots;
        protected DataView dvRules;
        protected DataView dvUnits;
        #endregion

        #region properties
        public DataView DvUnits
        {
            get{ return dvUnits;}
        }
        #endregion

        #region constructor
        public RuleTree(int _lenderId)
        {
            lenderId = _lenderId;
            GetData();
            BuildTree();
        }
        #endregion

        #region methods

        #region virtual
        protected virtual void GetData()
        {
        }
        protected virtual void BuildTree()
        {
        }
        protected virtual ArrayList GetChilds(RuleNode parent)
        {
            ArrayList res = null;
            int parentId = parent == null ? -1 : parent.Id;
            string currentFilter = String.Format(RULEFILTER, parentId);
            dvRules.RowFilter = currentFilter;
            if (dvRules.Count > 0)
            {
                res = new ArrayList();
                for (int i = 0; i < dvRules.Count; i++)
                {
                    RuleNode rn = CreateNode(dvRules[i], parent);
                    if (rn.Init(dvRules[i]))
                    {
                        rn.ChildNodes = GetChilds(rn);
                        res.Add(rn);
                        dvRules.RowFilter = currentFilter;
                    }
                }
            }
            return res;
        }
        protected virtual RuleNode CreateNode(DataRowView dr, RuleNode parent)
        {
            return null;
        }
        protected virtual RuleNode InitNode()
        {
            return null;
        }
        protected virtual string GetFieldName(DataRowView dr)
        {
            return String.Empty;
        }
        #endregion

        #endregion

        #region static methods
        public static DataView GetCategory(int lenderId)
        {
            return db.GetDataView(GETCATEGORY, lenderId);
        }
        public static DataView GetVariableVisibilityFields(int lenderId)
        {
            return db.GetDataView(GETVARIABLEVISIBILITYFIELDS, lenderId);
        }

        #endregion
    }
    public class RuleTreePublic : RuleTree
    {
        #region constants
        private const string GETRULETREE = "GetRuleTreePublic";
        private const string PROPERTYNAMEFIELDNAME = "propertyname";
        private const string GETDATAFIELDLIST = "GetDataDependableFieldList";
        private const string FIELDVALUEFIELDNAME = "fieldvalue";
        private const string GETUNITANDOBJECTS = "GetUnitAndObjects";
        #endregion

        #region fields
        private Hashtable ruleVisibilityFields = null;
        private Hashtable ruleDataFields = null;
        private Hashtable ruleFields = null;
        #endregion

        #region properties
        public Hashtable RuleVisibilityFields
        {
            get
            {
                if (ruleVisibilityFields == null)
                {
                    ruleVisibilityFields = GetVisibilityFields();
                }
                return ruleVisibilityFields;
            }
        }
        protected Hashtable RuleDataFields
        {
            get
            {
                if (ruleDataFields == null)
                {
                    ruleDataFields = GetDataFields();
                }
                return ruleDataFields;
            }
        }
        public Hashtable RuleFields
        {
            get { return ruleFields; }
        }
        #endregion

        #region constructor
        public RuleTreePublic(int _lenderId)
            : base(_lenderId)
        {

        }
        #endregion

        #region methods

        #region overwrriten
        protected override void GetData()
        {
            ds = db.GetDataSet(GETRULETREE, lenderId);
            dvRules = ds.Tables[0].DefaultView;
            dvUnits = ds.Tables[1].DefaultView;
            GetObjects();
        }
        protected override void BuildTree()
        {
            roots = GetChilds(null);
        }
        protected override RuleNode CreateNode(DataRowView dr, RuleNode parent)
        {
            return new RuleNodePublic(this, dr, parent);
        }
        #endregion

        #region public
        public ArrayList GetAllOtherRoots()
        {
            ArrayList res = new ArrayList();
            for (int i = 0; i < roots.Count; i++)
            {
                RuleNode rn = (RuleNode)roots[i];
                if (((rn.RuleObjectsMask & FIELDBIT) == 0) && (rn.RuleObjectsMask > 1))
                {
                    res.Add(roots[i]);
                }
            }
            return res;
        }
        public ArrayList GetVisibilityRoots()
        {
            ArrayList res = new ArrayList();
            for (int i = 0; i < roots.Count; i++)
            {
                if ((((RuleNode)roots[i]).RuleObjectsMask & FIELDBIT) != 0)
                {
                    res.Add(roots[i]);
                }
            }
            return res;
        }
        public ArrayList GetRuleDataDependantFields(int ruleId)
        {
            ArrayList res = null;
            if (RuleDataFields.ContainsKey(ruleId))
            {
                res = (ArrayList)RuleDataFields[ruleId];
            }
            return res;
        }
        public Hashtable GetRuleVisibilityDependantFields(int ruleId)
        {
            Hashtable res = null;
            if (RuleVisibilityFields.ContainsKey(ruleId))
            {
                res = (Hashtable)RuleVisibilityFields[ruleId];
            }

            return res;
        }
        #endregion

        #region private
        private static Hashtable GetDataFields()
        {
            Hashtable res = new Hashtable();
            DataView dv = db.GetDataView(GETDATAFIELDLIST);
            for (int i = 0; i < dv.Count; i++)
            {
                int ruleId = int.Parse(dv[i][RULEIDFIELDNAME].ToString());
                RuleDataValue rdv = new RuleDataValue(dv[i][PROPERTYNAMEFIELDNAME].ToString(), dv[i][FIELDVALUEFIELDNAME].ToString());
                if (res.ContainsKey(ruleId))
                {
                    ArrayList values = (ArrayList)res[ruleId];
                    values.Add(rdv);
                    res[ruleId] = values;
                }
                else
                {
                    ArrayList values = new ArrayList();
                    values.Add(rdv);
                    res.Add(ruleId, values);
                }
            }
            return res;
        }
        private void GetObjects()
        {
            DataSet ds_ = db.GetDataSet(GETUNITANDOBJECTS, lenderId);
            DataView dvu = ds_.Tables[0].DefaultView;
            DataView dvo = ds_.Tables[1].DefaultView;
            ruleFields = new Hashtable();
            for(int i=0;i<dvu.Count;i++)
            {
                string propertyName = dvu[i][PROPERTYNAMEFIELDNAME].ToString();
                if(!ruleFields.ContainsKey(propertyName))
                {
                    FieldDependancy fd = new FieldDependancy(propertyName,dvu,dvo, dvRules);
                    if (fd.ObjectMask!=0)
                    {
                        ruleFields.Add(propertyName,fd);
                    }
                }
            }
        }
        #endregion

        #region protected
        protected static Hashtable GetVisibilityFields()
        {
            Hashtable res = new Hashtable();
            DataView dv = db.GetDataView(GETVISIBILITYFIELDLIST);
            for (int i = 0; i < dv.Count; i++)
            {
                int ruleId = int.Parse(dv[i][RULEIDFIELDNAME].ToString());
                string fieldName = dv[i][PROPERTYNAMEFIELDNAME].ToString();
                if (res.ContainsKey(ruleId))
                {
                    Hashtable fields = (Hashtable)res[ruleId];
                    if (!fields.ContainsKey(fieldName))
                    {
                        fields.Add(fieldName, false);
                        res[ruleId] = fields;
                    }
                }
                else
                {
                    Hashtable fields = new Hashtable();
                    fields.Add(fieldName, false);
                    res.Add(ruleId, fields);
                }
            }
            return res;
        }
        #endregion

        #endregion
    }
    public class FieldDependancy
    {
        #region constants
        private const string PROPERTYNAMEFILETER = "propertyname='{0}'";
        private const string RULEIDFIELDNAME = "ruleid";
        private const string RULEOBJECTFILTER = "ruleid={0} and objecttypeid={1}";
        private const string RULEFILTER = "id={0}";
        private const string OBJECTIDFIELDNAME = "objectid";
        private const int SHOWFIELDOBJECTTYPEID = 1;
        private const string CONDITIONFIELDNAME = "condition";
        private const string TASKFIELDNAME = "task";
        private const string DOCUMENTFIELDNAME = "document";
        private const string CHECKLISTFIELDNAME = "checklist";
        private const string EVENTFIELDNAME = "event";
        private const string ALERTFIELDNAME = "alert";
        private const string DATAFIELDNAME = "data";
        private const string CLOSINGINSTRUCTIONFIELDNAME = "closinginstruction";
        #endregion

        #region fields
        private readonly Hashtable showFields;
        private readonly int objectMask = 0;
        #endregion

        #region propreties
        public int ObjectMask
        {
            get { return objectMask; }
        }
        #endregion

        #region constructor
        public FieldDependancy(string propertyName, DataView dvUnits, DataView ruleObjects, DataView dvRules)
        {
            int[] rules = GetRules(dvUnits, propertyName);
            if ((rules!=null)&&(rules.Length>0))
            {
                showFields = GetShowFields(rules, ruleObjects);
                objectMask = GetObjectsMask(rules, dvRules);
                if(showFields.Count>0)
                {
                    objectMask += RuleTree.FIELDBIT;
                }
            }
        }
        #endregion
        #region methods
        private static Hashtable GetShowFields(int[] rules,DataView ruleObjects)
        {
            Hashtable res = new Hashtable();
            for (int i = 0; i < rules.Length;i++)
            {
                DataRow[] rows = ruleObjects.Table.Select(String.Format(RULEOBJECTFILTER, rules[i], SHOWFIELDOBJECTTYPEID));
                for(int j=0;j<rows.Length;j++)
                {
                    int objectId = int.Parse(rows[j][OBJECTIDFIELDNAME].ToString());
                    if(!res.ContainsKey(objectId))
                    {
                        res.Add(objectId,true);
                    }
                }
            }
            return res;
        }
        private static int GetObjectsMask(int[] rules,DataView dvRules)
        {
            int res = 0;
            for (int i = 0; i < rules.Length;i++)
            {
                DataRow[] rows = dvRules.Table.Select(String.Format(RULEFILTER, rules[i]));
                for (int j = 0; j < rows.Length; j++)
                {
                    if (int.Parse(rows[j][CONDITIONFIELDNAME].ToString()) > 0)
                    {
                        res += RuleTree.CONDITIONBIT;
                    }
                    if (int.Parse(rows[j][TASKFIELDNAME].ToString()) > 0)
                    {
                        res += RuleTree.TASKBIT;
                    }
                    if (int.Parse(rows[j][DOCUMENTFIELDNAME].ToString()) > 0)
                    {
                        res += RuleTree.DOCUMENTBIT;
                    }
                    if (int.Parse(rows[j][CHECKLISTFIELDNAME].ToString()) > 0)
                    {
                        res += RuleTree.CHECKLISTBIT;
                    }
                    if (int.Parse(rows[j][EVENTFIELDNAME].ToString()) > 0)
                    {
                        res += RuleTree.EVENTBIT;
                    }
                    if (int.Parse(rows[j][ALERTFIELDNAME].ToString()) > 0)
                    {
                        res += RuleTree.ALERTBIT;
                    }
                    if (int.Parse(rows[j][DATAFIELDNAME].ToString()) > 0)
                    {
                        res += RuleTree.DATABIT;
                    }
                    if (int.Parse(rows[j][CLOSINGINSTRUCTIONFIELDNAME].ToString()) > 0)
                    {
                        res += RuleTree.CLOSINGINSTRACTIONBIT;
                    }
                }
            }
            return res;
        }
        private static int[] GetRules(DataView dv, string propertyName)
        {
            DataRow[] rows = dv.Table.Select(String.Format(PROPERTYNAMEFILETER, propertyName));
            if (rows.Length > 0)
            {

                int[] res = new int[rows.Length];
                for (int i = 0; i < rows.Length; i++)
                {
                    res[i] = int.Parse(rows[i][RULEIDFIELDNAME].ToString());
                }
                return res;
            }
            return null;
        }

        #endregion
    }
    public class RuleTreeAdmin : RuleTree
    {
        #region constants
        private const string GETRULETREE = "GetRuleTreeAdmin";
        private const string GETRULEUNITSWITHDATA = "GetAllRuleUnits";
        private const string GETCONDITIONLIST = "GetRuleConditionList";
        private const string GETCLOSINGINSTRUCTIONLIST = "GetRuleClosingInstructionList";
        private const string GETTASKLIST = "GetRuleTaskList";
        private const string GETDOCUMENTLIST = "GetDocumentList";
        private const string GETCHECKLISTLIST = "GetRuleCheckListList";
        private const string GETEVENTLIST = "GetEventList";
        private const string GETALERTLIST = "GetAlertList";
        private const string GETDATAFIELDLIST = "GetDataFieldList";
        private const string DESCRIPTIONNAMEFIELDNAME = "description";
        private const string TITLEFIELDNAME = "title";
        public const string ROOTELEMENT = "Root";
        public const string ROOTRULENAME = "Rule's tree";
        private const string ITEMELEMENT = "item";
        public const string TEXTATTRIBUTE = "text";
        public const string PARENTIDATTRIBUTE = "parentid";
        public const string NODETYPEATTRIBUTE = "type";
        public const string IDATTRIBUTE = "ruleid";
        public const string ENABLEDATTRIBUTE = "enabled";
        public const string CATEGORYIDATTRIBUTE = "categoryid";
        public const string COMPANYIDATTRIBUTE = "companyid";
        #endregion

        #region fields
        private Hashtable ruleVisibilityFields = null;
        private Hashtable ruleUnits = null;
        private Hashtable ruleConditions = null;
        private Hashtable ruleTasks = null;
        private Hashtable ruleDocuments = null;
        private Hashtable ruleCheckLists = null;
        private Hashtable ruleEvents = null;
        private Hashtable ruleAlerts = null;
        private Hashtable ruleDataFields = null;
        private Hashtable ruleClosingInstructions = null;
        private XmlDocument xmlData = null;
        #endregion

        #region properties
        public Hashtable RuleVisibilityFields
        {
            get
            {
                if (ruleVisibilityFields == null)
                {
                    ruleVisibilityFields = GetVisibilityFields();
                }
                return ruleVisibilityFields;
            }
        }
        public Hashtable RuleUnits
        {
            get
            {
                if (ruleUnits == null)
                {
                    ruleUnits = GetRuleUnitsWithData();
                }
                return ruleUnits;
            }
        }
        public Hashtable RuleConditions
        {
            get
            {
                if (ruleConditions == null)
                {
                    ruleConditions = GetRuleObjects(GETCONDITIONLIST);
                }
                return ruleConditions;
            }
        }
        public Hashtable RuleClosingInstructions
        {
            get
            {
                if (ruleClosingInstructions == null)
                {
                    ruleClosingInstructions = GetRuleObjects(GETCLOSINGINSTRUCTIONLIST);
                }
                return ruleClosingInstructions;
            }
        }
        public Hashtable RuleTasks
        {
            get
            {
                if(ruleTasks==null)
                {
                    ruleTasks = GetRuleObjects(GETTASKLIST);
                }
                return ruleTasks;
            }
        }
        public Hashtable RuleDocuments
        {
            get
            {
                if(ruleDocuments==null)
                {
                    ruleDocuments = GetRuleObjects(GETDOCUMENTLIST);
                }
                return ruleDocuments;
            }
        }
        public Hashtable RuleCheckLists
        {
            get
            {
                if(ruleCheckLists==null)
                {
                    ruleCheckLists = GetRuleObjects(GETCHECKLISTLIST);
                }
                return ruleCheckLists;
            }
        }
        public Hashtable RuleEvents
        {
            get
            {
                if(ruleEvents==null)
                {
                    ruleEvents = GetRuleObjects(GETEVENTLIST);
                }
                return ruleEvents;
            }
        }
        public Hashtable RuleAlerts
        {
            get
            {
                if(ruleAlerts==null)
                {
                    ruleAlerts = GetRuleObjects(GETALERTLIST);
                }
                return ruleAlerts;
            }
        }
        public Hashtable RuleData
        {
            get
            {
                if (ruleDataFields == null)
                {
                    ruleDataFields = GetRuleObjects(GETDATAFIELDLIST);
                }
                return ruleDataFields;
            }
        }
        public XmlDocument XmlData
        {
            get
            {
                if(xmlData==null)
                {
                    xmlData = GetRuleTreeXml();
                }
                return xmlData;
            }
        }
        #endregion

        #region constructor
        public RuleTreeAdmin(int _lenderId)
            : base(_lenderId)
        {

        }
        #endregion

        #region methods

        #region overwriten
        protected override void GetData()
        {
            ds = db.GetDataSet(GETRULETREE, lenderId);
            dvRules = ds.Tables[0].DefaultView;
            dvUnits = ds.Tables[1].DefaultView;
        }
        protected override void BuildTree()
        {
            roots = GetChilds(null);
        }
        protected override RuleNode CreateNode(DataRowView dr, RuleNode parent)
        {
            return new RuleNodeAdmin(this, dr, parent);
        }
        #endregion

        #region public
        public Hashtable GetRuleVisibilityDependantFields(int ruleId)
        {
            Hashtable res = null;
            if (RuleVisibilityFields.ContainsKey(ruleId))
            {
                res = (Hashtable)RuleVisibilityFields[ruleId];
            }
            return res;
        }
        public void UpdateNodeExpression(int nodeId, string expression)
        {
            RuleNodeAdmin node = FindNodeById(nodeId);
            if(node!=null)
            {
                node.RuleExpression = expression;
            }
        }
        public XmlDocument GetFilteredTree(int categoryId)
        {
            if(categoryId==0)
            {
                return XmlData;
            }
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            AddAttribute(root, TEXTATTRIBUTE, ROOTRULENAME);
            AddAttribute(root, NODETYPEATTRIBUTE, nodeRoot.ToString());
            for(int i=0;i<XmlData.DocumentElement.ChildNodes.Count;i++)
            {
                XmlNode node = XmlData.DocumentElement.ChildNodes[i];
                int catId = GetAttributeInt(node, CATEGORYIDATTRIBUTE);
                if(catId==categoryId)
                {
                    XmlNode importedNode = d.ImportNode(node, true);
                    root.AppendChild(importedNode);
                }
            }
            d.AppendChild(root);
            return d;
        }
        #endregion

        #region private
        private static int GetAttributeInt(XmlNode xmlNode, string name)
        {
            int res = -1;
            try
            {
                res = int.Parse(xmlNode.Attributes[name].Value);
            }
            catch
            {
            }
            return res;
        }
        private XmlDocument GetRuleTreeXml()
        {
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            AddAttribute(root, TEXTATTRIBUTE, ROOTRULENAME);
            AddAttribute(root, NODETYPEATTRIBUTE, nodeRoot.ToString());
            for (int i = 0; i < roots.Count; i++)
            {
                GetChildsNode((RuleNodeAdmin)roots[i], root);
            }
            d.AppendChild(root);
            return d;
        }
        private RuleNodeAdmin FindNodeById(int nodeId)
        {
            return CheckChilds(roots,nodeId);
        }
        private RuleNodeAdmin CheckChilds(IList list,int nodeId)
        {
            RuleNodeAdmin res = null;
            if((list!=null)&&(list.Count>0))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    RuleNodeAdmin rn = (RuleNodeAdmin)list[i];
                    if (rn.Id == nodeId)
                    {
                        res = rn;
                        break;
                    }
                    res = CheckChilds(rn.ChildNodes,nodeId);
                    if(res!=null)
                    {
                        break;
                    }
                }
            }
            return res;
        }

        private static Hashtable GetRuleUnitsWithData()
        {
            Hashtable res = new Hashtable();
            DataView dv = db.GetDataView(GETRULEUNITSWITHDATA);
            if (dv.Count > 0)
            {
                for (int i = 0; i < dv.Count; i++)
                {
                    RuleEvaluationUnit reu = new RuleEvaluationUnit(dv[i]);
                    res.Add(reu.Id, reu);
                }
            }
            return res;
        }
        private static void AddAttribute(XmlNode node, string attributeName, string attributeValue)
        {
            XmlAttribute a = node.OwnerDocument.CreateAttribute(attributeName);
            a.Value = attributeValue;
            node.Attributes.Append(a);
        }
        private void GetChildsNode(RuleNodeAdmin rn, XmlNode parent)
        {
            XmlNode node = parent.OwnerDocument.CreateElement(ITEMELEMENT);
            //#region DEBUG ONLY
            //AddAttribute(node, TEXTATTRIBUTE, rn.RuleExpression + " id=" + rn.Id);
            //#endregion
            AddAttribute(node, TEXTATTRIBUTE, rn.RuleExpression);
            AddAttribute(node, NODETYPEATTRIBUTE, nodeRule.ToString());
            AddAttribute(node, IDATTRIBUTE, rn.Id.ToString());
            AddAttribute(node, PARENTIDATTRIBUTE, (rn.ParentNode!=null)?rn.ParentNode.Id.ToString():"-1");
            AddAttribute(node,ENABLEDATTRIBUTE,rn.IsEnabled?"1":"0");
            AddAttribute(node, COMPANYIDATTRIBUTE, rn.CompanyId.ToString());
            if(rn.ParentNode==null)
            {
                AddAttribute(node, CATEGORYIDATTRIBUTE, rn.CategoryId.ToString());
            }
            GetLeafs(rn, node);
            AddWithSort(parent,node);
            if ((rn.ChildNodes != null) && (rn.ChildNodes.Count > 0))
            {
                for (int i = 0; i < rn.ChildNodes.Count; i++)
                {
                    GetChildsNode((RuleNodeAdmin)rn.ChildNodes[i], node);
                }
            }
        }
        private static Hashtable GetRuleObjects(string procName)
        {
            Hashtable res = new Hashtable();
            DataView dv = db.GetDataView(procName);
            if (dv.Count > 0)
            {
                for (int i = 0; i < dv.Count; i++)
                {
                    int ruleId = int.Parse(dv[i][RULEIDFIELDNAME].ToString());
                    string conditionTitle = dv[i][TITLEFIELDNAME].ToString();
                    int id = int.Parse(dv[i][IDFIELDNAME].ToString());
                    string s = id + "_" + conditionTitle;
                    if (res.ContainsKey(ruleId))
                    {
                        ArrayList list = res[ruleId] as ArrayList;
                        if (list != null)
                        {
//                            list.Add(conditionTitle);
                            list.Add(s);
                            res[ruleId] = list;
                        }
                    }
                    else
                    {
                        ArrayList list = new ArrayList();
//                        list.Add(conditionTitle);
                        list.Add(s);
                        res.Add(ruleId, list);
                    }
                }
            }
            return res;
        }
        private static void GetLeafs(RuleNodeAdmin rn, XmlNode node)
        {
            GetLeaf(rn, node, FIELDBIT);
            GetLeaf(rn, node, CONDITIONBIT);
            GetLeaf(rn, node, TASKBIT);
            GetLeaf(rn, node, DOCUMENTBIT);
            GetLeaf(rn, node, CHECKLISTBIT);
            GetLeaf(rn, node, EVENTBIT);
            GetLeaf(rn, node, ALERTBIT);
            GetLeaf(rn, node, DATABIT);
            GetLeaf(rn, node, CLOSINGINSTRACTIONBIT);
        }
        private static void GetLeaf(RuleNodeAdmin rn, XmlNode node, int nodeType)
        {
            IList list = rn.GetObjects(nodeType);
            if((list!=null)&&(list.Count>0))
            {
                for(int i=0;i<list.Count;i++)
                {
                    string txt;
                    string id;
                    ParseText(list[i].ToString(), out id, out txt);
                    XmlNode child = node.OwnerDocument.CreateElement(ITEMELEMENT);
                    AddAttribute(child, NODETYPEATTRIBUTE, nodeType.ToString());
//                    AddAttribute(child, TEXTATTRIBUTE, GetPrefix(nodeType)+"("+list[i]+")");
                    AddAttribute(child, TEXTATTRIBUTE, GetPrefix(nodeType) + "(" + txt + ")");
                    AddAttribute(child, IDATTRIBUTE, id);
                    AddAttribute(child, PARENTIDATTRIBUTE, rn.Id.ToString());
                    AddWithSort(node, child);
                }
            }
        }
        private static void ParseText(string s,out string id,out string txt)
        {
            int i = s.IndexOf("_");
            if(i>0)
            {
                id = s.Substring(0,i);
                txt = s.Substring(i + 1, s.Length - i-1);
            }
            else
            {
                id = String.Empty;
                txt = s;
            }
        }

        private static void AddWithSort(XmlNode parent,XmlNode node)
        {
            if((parent.ChildNodes!=null)&&(parent.ChildNodes.Count>0))
            {
                string txt = node.Attributes[TEXTATTRIBUTE].Value;
                int typeId = int.Parse(node.Attributes[NODETYPEATTRIBUTE].Value);
                for(int i=0;i<parent.ChildNodes.Count;i++)
                {
                    XmlNode n = parent.ChildNodes[i];
                    string txt1 = n.Attributes[TEXTATTRIBUTE].Value;
                    int typeId1 = int.Parse(n.Attributes[NODETYPEATTRIBUTE].Value);
                    if((typeId>0)&&(typeId<typeId1))
                    {
                        parent.InsertBefore(node, n);
                        return;
                    }
                    else if (typeId == typeId1)
                    {
                        if (String.Compare(txt1, txt, true) > 0)
                        {
                            parent.InsertBefore(node, n);
                            return;
                        }
                    }
                }
                parent.AppendChild(node);
            }
            else
            {
                parent.AppendChild(node);
            }
        }

        //private static void GetLeaf(RuleNodeAdmin rn, XmlNode node, int nodeType)
        //{
        //    string text = GetText(rn.GetObjects(nodeType), nodeType);
        //    if (!String.IsNullOrEmpty(text))
        //    {
        //        XmlNode child = node.OwnerDocument.CreateElement(ITEMELEMENT);
        //        AddAttribute(child, NODETYPEATTRIBUTE, nodeType.ToString());
        //        AddAttribute(child, TEXTATTRIBUTE, text);
        //        AddAttribute(child, PARENTIDATTRIBUTE, rn.Id.ToString());
        //        node.AppendChild(child);
        //    }
        //}
        //private static string GetText(IList list, int nodeType)
        //{
        //    string res = String.Empty;
        //    if ((list != null) && (list.Count > 0))
        //    {
        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            if (i > 0)
        //            {
        //                res += ",";
        //            }
        //            res += list[i].ToString();
        //        }
        //    }
        //    if (!String.IsNullOrEmpty(res))
        //    {
        //        res = GetPrefix(nodeType) + "(" + res + ")";
        //    }
        //    return res;
        //}
        private static string GetPrefix(int nodeType)
        {
            string res = String.Empty;
            switch (nodeType)
            {
                case FIELDBIT:
                    res = "SF";
                    break;
                case CONDITIONBIT:
                    res = "CD";
                    break;
                case TASKBIT:
                    res = "TS";
                    break;
                case DOCUMENTBIT:
                    res = "DC";
                    break;
                case CHECKLISTBIT:
                    res = "CL";
                    break;
                case EVENTBIT:
                    res = "EV";
                    break;
                case ALERTBIT:
                    res = "AL";
                    break;
                case DATABIT:
                    res = "DT";
                    break;
                case CLOSINGINSTRACTIONBIT:
                    res = "CI";
                    break;
            }
            return res;
        }
        private static Hashtable GetVisibilityFields()
        {
            Hashtable res = new Hashtable();
            DataView dv = db.GetDataView(GETVISIBILITYFIELDLIST);
            for (int i = 0; i < dv.Count; i++)
            {
                int ruleId = int.Parse(dv[i][RULEIDFIELDNAME].ToString());
                string fieldName = dv[i][DESCRIPTIONNAMEFIELDNAME].ToString();
                int id = int.Parse(dv[i][IDFIELDNAME].ToString());
                if (res.ContainsKey(ruleId))
                {
                    Hashtable fields = (Hashtable)res[ruleId];
                    if (!fields.ContainsKey(fieldName))
                    {
                        fields.Add(fieldName, id);
                        res[ruleId] = fields;
                    }
                }
                else
                {
                    Hashtable fields = new Hashtable();
                    fields.Add(fieldName, id);
                    res.Add(ruleId, fields);
                }
            }
            return res;
        }
        #endregion

        #endregion
    }

    public class RuleEvaluationTree
    {

        #region fields
        private ArrayList roots;
        private ArrayList dataObjects;
        private RuleTreePublic rt;
        #endregion

        #region properties
        public ArrayList DataObjects
        {
            get
            {
                if (dataObjects == null)
                {
                    dataObjects = new ArrayList();
                }
                return dataObjects;
            }
            set { dataObjects = value; }
        }
        public RuleTreePublic RTPublic
        {
            get
            {
                return rt;
            }
        }
        #endregion

        #region constructor
        #endregion

        #region methods
        public void Evaluate(MortgageProfile mp, RuleTreePublic _rt)
        {
#if DUMPRULES
            mp.CurrentPage.WriteDebugToLog(String.Format("========================================"+System.Environment.NewLine+"Rule evaluation started for mortgage profile Id={0} @{1}", mp.ID,DateTime.Now.ToLongTimeString()));
#endif
            rt = _rt;
            roots = new ArrayList();
            ArrayList rules = rt.GetVisibilityRoots();
#if DUMPRULES
            mp.CurrentPage.WriteDebugToLog(String.Format("Visibility roots count = {0}", rules.Count));
#endif
            for (int i = 0; i < rules.Count; i++)
            {
                RuleNodePublic rn = (RuleNodePublic)rules[i];
                RuleEvaluationNode ren = new RuleEvaluationNode(rn, null, this);
                ren.EvaluateVisibility(mp, rn);
                roots.Add(ren);
            }
            rules = rt.GetAllOtherRoots();
#if DUMPRULES
            mp.CurrentPage.WriteDebugToLog(String.Format("Not-visibility roots count = {0}", rules.Count));
#endif
            for (int i = 0; i < rules.Count; i++)
            {
                RuleNodePublic rn = (RuleNodePublic)rules[i];
                RuleEvaluationNode ren = new RuleEvaluationNode(rn, null, this);
                ren.EvaluateActions(mp, rn);
                roots.Add(ren);
            }
            SetData(mp);
#if DUMPRULES
            mp.CurrentPage.WriteDebugToLog("========================================" + System.Environment.NewLine + String.Format("Rule evaluation completed for mortgage profile Id={0} @{1}", mp.ID, DateTime.Now.ToLongTimeString()));
#endif
#if DUMPRULES
            mp.CurrentPage.WriteDebugToLog(String.Format(System.Environment.NewLine + "***************" + System.Environment.NewLine+"Visible fields count {0}:",mp.VisibleFields));
            foreach(DictionaryEntry item in mp.VisibleFields)
            {
                string message = item.Key.ToString();
                Hashtable list = item.Value as Hashtable;
                if((list != null)&&(list.Count>0))
                {
                    message += "\t\t\t for objects ";
                    foreach(DictionaryEntry obj in list)
                    {
                        message += "  id=" + ((BaseObject) obj.Key).ID.ToString();
                    }
                }
                mp.CurrentPage.WriteDebugToLog(message);
            }
            mp.CurrentPage.WriteDebugToLog(System.Environment.NewLine + "###############" + System.Environment.NewLine + "Actions triggered:");
            for(int i=0;i<6;i++)
            {
                DumpActions(i,mp);
            }

#endif
        }
#if DUMPRULES
        private static void DumpActions(int i, MortgageProfile mp)
        {
            string message = String.Empty;
            int[] ruleIds=null;
            switch (i)
            {
                case 0:
                    ruleIds = mp.GetRuleList(RuleTree.CONDITIONBIT);
                    message = "Conditions id's : ";
                    break;
                case 1:
                    ruleIds = mp.GetRuleList(RuleTree.TASKBIT);
                    message = "Tasks id's : ";
                    break;
                case 2:
                    ruleIds = mp.GetRuleList(RuleTree.DOCUMENTBIT);
                    message = "Documents id's : ";
                    break;
                case 3:
                    ruleIds = mp.GetRuleList(RuleTree.CHECKLISTBIT);
                    message = "Checklists id's : ";
                    break;
                case 4:
                    ruleIds = mp.GetRuleList(RuleTree.EVENTBIT);
                    message = "Events id's : ";
                    break;
                case 5:
                    ruleIds = mp.GetRuleList(RuleTree.ALERTBIT);
                    message = "Alerts id's : ";
                    break;
            }
            if((ruleIds!=null)&&(ruleIds.Length>0))
            {
                for(int j=0;j<ruleIds.Length;j++)
                {
                    if(j!=0)
                    {
                        message += ",";
                    }
                    message += ruleIds[j].ToString();
                }
            }
            else
            {
                message += "none";
            }
            mp.CurrentPage.WriteDebugToLog(message);
        }
#endif
        private void SetData(MortgageProfile mp)
        {
#if DUMPRULES
            mp.CurrentPage.WriteDebugToLog("-----------------------" + System.Environment.NewLine + String.Format("Setting data mortgage profile Id={0}", mp.ID));
#endif
            if ((DataObjects != null) && (DataObjects.Count > 0))
            {
#if DUMPRULES
            mp.CurrentPage.WriteDebugToLog(String.Format("Data objects count ={0}", DataObjects.Count));
#endif
                for (int i = 0; i < DataObjects.Count; i++)
                {
                    RuleDataValueObject rdvo = (RuleDataValueObject)DataObjects[i];
                    if (rdvo != null)
                    {
#if DUMPRULES
                        mp.CurrentPage.WriteDebugToLog(String.Format("    Property - {0}, data value = {1}, object Id - {2}", rdvo.FullName, rdvo.ObjectValue,rdvo.ObjectId));
#endif
                    }
                }
            }
        }
        #endregion

    }
    public abstract class RuleNode
    {
        #region constants
        private const string UNITFILTER = "ruleId={0}";
        private const string FIELDFIELDNAME = "field";
        private const string CONDITIONFIELDNAME = "condition";
        private const string TASKFIELDNAME = "task";
        private const string DOCUMENTFIELDNAME = "document";
        private const string CHECKLISTFIELDNAME = "checklist";
        private const string EVENTFIELDNAME = "event";
        private const string ALERTFIELDNAME = "alert";
        private const string DATAFIELDNAME = "data";
        #endregion

        #region fields
        protected int id;
        private RuleNode parentNode;
        protected ArrayList units;
        protected ArrayList childNodes;
        protected int ruleObjectsMask = 0;
        protected int currentRuleObjects = 0;
        protected RuleTree tree;
        #endregion

        #region properties
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public RuleNode ParentNode
        {
            get { return parentNode; }
            set { parentNode = value; }
        }
        public ArrayList Units
        {
            get { return units; }
            set { units = value; }
        }
        public int RuleObjectsMask
        {
            get { return ruleObjectsMask; }
            set { ruleObjectsMask = value; }
        }
        public int CurrentRuleObjects
        {
            get { return currentRuleObjects; }
            set { currentRuleObjects = value; }
        }
        public ArrayList ChildNodes
        {
            get { return childNodes; }
            set { childNodes = value; }
        }
        #endregion

        #region constructor
        public RuleNode(RuleTree _tree, DataRowView dr, RuleNode _parent)
        {
            tree = _tree;
            id = int.Parse(dr[RuleTree.IDFIELDNAME].ToString());
            parentNode = _parent;
            GetUnits();
        }
        #endregion

        #region methods
        
        #region virtual
        public virtual bool Init(DataRowView dr)
        {
            return true;
        }
        protected virtual void GetChildFields(Hashtable fields)
        {
        }
        #endregion

        #region public
        #endregion

        #region protected
        protected void SetObjects(DataRowView dr)
        {
            currentRuleObjects = 0;
            if (int.Parse(dr[FIELDFIELDNAME].ToString()) > 0)
            {
                currentRuleObjects |= RuleTree.FIELDBIT;
            }
            if (int.Parse(dr[CONDITIONFIELDNAME].ToString()) > 0)
            {
                currentRuleObjects |= RuleTree.CONDITIONBIT;
            }
            if (int.Parse(dr[TASKFIELDNAME].ToString()) > 0)
            {
                currentRuleObjects |= RuleTree.TASKBIT;
            }
            if (int.Parse(dr[DOCUMENTFIELDNAME].ToString()) > 0)
            {
                currentRuleObjects |= RuleTree.DOCUMENTBIT;
            }
            if (int.Parse(dr[CHECKLISTFIELDNAME].ToString()) > 0)
            {
                currentRuleObjects |= RuleTree.CHECKLISTBIT;
            }
            if (int.Parse(dr[EVENTFIELDNAME].ToString()) > 0)
            {
                currentRuleObjects |= RuleTree.EVENTBIT;
            }
            if (int.Parse(dr[ALERTFIELDNAME].ToString()) > 0)
            {
                currentRuleObjects |= RuleTree.ALERTBIT;
            }
            if (int.Parse(dr[DATAFIELDNAME].ToString()) > 0)
            {
                currentRuleObjects |= RuleTree.DATABIT;
            }
            ruleObjectsMask = currentRuleObjects;
        }
        protected void SetParentObjects()
        {
            if (parentNode != null)
            {
                int parentObjects = parentNode.ruleObjectsMask;
                parentObjects = parentObjects | ruleObjectsMask;
                if (parentObjects != parentNode.ruleObjectsMask)
                {
                    parentNode.ruleObjectsMask = parentObjects;
                    parentNode.SetParentObjects();
                }
            }
        }
        #endregion

        #region private
        private void GetUnits()
        {
            units = null;
            if(tree.DvUnits!=null)
            {
                tree.DvUnits.RowFilter = String.Format(UNITFILTER, id);
                if (tree.DvUnits.Count > 0)
                {
                    units = new ArrayList();
                    for (int i = 0; i < tree.DvUnits.Count; i++)
                    {
                        units.Add(new RuleEvaluationUnit(tree.DvUnits[i]));
                    }
                }
            }
        }
        #endregion

        #endregion
    }
    public class RuleNodePublic : RuleNode
    {
        //#region fields
        //private bool isAlwaysEvaluated = false;
        //#endregion

        #region properties
        protected RuleTreePublic treePublic
        {
            get { return tree as RuleTreePublic; }
        }
        #endregion

        #region constructor
        public RuleNodePublic(RuleTree _tree, DataRowView dr, RuleNode _parent)
            : base(_tree, dr, _parent)
        {
//            isAlwaysEvaluated = bool.Parse(dr["IgnoreVisibiltyForRules"].ToString());
        }
        #endregion

        #region methods

        #region overwritten
        public override bool Init(DataRowView dr)
        {
            if (units != null)
            {
                SetObjects(dr);
                SetParentObjects();
                return true;
            }
            return false;
        }
        protected override void GetChildFields(Hashtable fields)
        {
            if (childNodes != null)
            {
                for (int i = 0; i < childNodes.Count; i++)
                {
                    RuleNodePublic rn = (RuleNodePublic)childNodes[i];
                    Hashtable res = rn.GetVisibilityDependantFieldsWithChilds();
                    if (res != null)
                    {
                        foreach (DictionaryEntry re in res)
                        {
                            if (!fields.ContainsKey(re.Key))
                            {
                                fields.Add(re.Key, re.Value);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region public
        public Hashtable GetVisibilityDependantFieldsWithChilds()
        {
            Hashtable res = treePublic.GetRuleVisibilityDependantFields(id);
            if (res == null)
            {
                res = new Hashtable();
            }
            if (childNodes != null)
            {
                GetChildFields(res);
            }
            return res;
        }
        public Hashtable GetVisibilityDependantFields()
        {
            Hashtable res = treePublic.GetRuleVisibilityDependantFields(id);
            if (res == null)
            {
                res = new Hashtable();
            }
            return res;
        }
        public ArrayList GetDataDependantFields()
        {
            return treePublic.GetRuleDataDependantFields(id);
        }
        #endregion

        #endregion
    }
    public class RuleNodeAdmin : RuleNode
    {
        #region constants
        private const string STATUSIDFIELDNAME = "statusid";
        private const string CATEGORYIDFIELDNAME = "categoryid";
        private const string COMPANYIDFIELDNAME = "lenderid";
        private const int ENABLEDSTATUSID = 1;
        #endregion

        #region fields
        private string ruleExpression = String.Empty;
        private readonly string fullExpression = String.Empty;
        private readonly bool isEnabled = true;
        private readonly int categoryId;
        private readonly int companyId;
        #endregion

        #region properties
        protected RuleTreeAdmin treeAdmin
        {
            get { return tree as RuleTreeAdmin; }
        }
        public string RuleExpression
        {
            get 
            { 
                if(!String.IsNullOrEmpty(ruleExpression))
                {
                    return ruleExpression; 
                }
                return "N/A";
            }
            set { ruleExpression = value; }
        }
        public string FullExpression
        {
            get { return fullExpression; }
        }
        public bool IsEnabled
        {
            get { return isEnabled; }
        }
        public int CategoryId
        {
            get { return categoryId; }
        }
        public int CompanyId
        {
            get { return companyId; }
        }
        #endregion

        #region constructor
        public RuleNodeAdmin(RuleTree _tree, DataRowView dr, RuleNode _parent)
            : base(_tree, dr, _parent)
        {
            isEnabled = int.Parse(dr[STATUSIDFIELDNAME].ToString()) == ENABLEDSTATUSID;
            categoryId = int.Parse(dr[CATEGORYIDFIELDNAME].ToString());
            companyId = int.Parse(dr[COMPANYIDFIELDNAME].ToString());
        }
        #endregion

        #region methods
        
        #region overwritten
        public override bool Init(DataRowView dr)
        {
            SetObjects(dr);
            SetParentObjects();
            SetExpression();
            return true;
        }
        #endregion

        #region private
        private void SetExpression()
        {
            ruleExpression = String.Empty;
            if ((units != null) && (units.Count > 0))
            {
                if(treeAdmin!=null)
                {
                    if (treeAdmin.RuleUnits != null)
                    {
                        for (int i = 0; i < units.Count; i++)
                        {
                            int _id = ((RuleEvaluationUnit)units[i]).Id;
                            if (treeAdmin.RuleUnits.ContainsKey(_id))
                            {
                                RuleEvaluationUnit reu = treeAdmin.RuleUnits[_id] as RuleEvaluationUnit;
                                if (reu != null)
                                {
                                    ruleExpression += reu.GetUnitExpression(i == 0);
                                }
                            }
                        }
                    }
                }
            }
            if(!String.IsNullOrEmpty(ruleExpression))
            {
                ruleExpression = "[" + ruleExpression + "]";
            }
        }
        private ArrayList GetFieldsArray()
        {
            ArrayList res = new ArrayList();
            Hashtable ht = treeAdmin.GetRuleVisibilityDependantFields(id);
            if ((ht != null) && (ht.Count > 0))
            {
                foreach (DictionaryEntry item in ht)
                {
                    string s = item.Value +"_"+ item.Key;
                    res.Add(s);
                }
            }
            return res;
        }
        private ArrayList GetConditionsArray()
        {
            ArrayList res = new ArrayList();
            if (treeAdmin.RuleConditions.ContainsKey(id))
            {
                res = treeAdmin.RuleConditions[id] as ArrayList;
            }
            return res;
        }
        private ArrayList GetClosingInstructionsArray()
        {
            ArrayList res = new ArrayList();
            if (treeAdmin.RuleClosingInstructions.ContainsKey(id))
            {
                res = treeAdmin.RuleClosingInstructions[id] as ArrayList;
            }
            return res;
        }

        private ArrayList GetTasksArray()
        {
            ArrayList res = new ArrayList();
            if (treeAdmin.RuleTasks.ContainsKey(id))
            {
                res = treeAdmin.RuleTasks[id] as ArrayList;
            }
            return res;
        }
        private ArrayList GetDocumentsArray()
        {
            ArrayList res = new ArrayList();
            if (treeAdmin.RuleDocuments.ContainsKey(id))
            {
                res = treeAdmin.RuleDocuments[id] as ArrayList;
            }
            return res;
        }
        private ArrayList GetCheckListsArray()
        {
            ArrayList res = new ArrayList();
            if (treeAdmin.RuleCheckLists.ContainsKey(id))
            {
                res = treeAdmin.RuleCheckLists[id] as ArrayList;
            }
            return res;
        }
        private ArrayList GetEventsArray()
        {
            ArrayList res = new ArrayList();
            if (treeAdmin.RuleEvents.ContainsKey(id))
            {
                res = treeAdmin.RuleEvents[id] as ArrayList;
            }
            return res;
        }
        private ArrayList GetAlertsArray()
        {
            ArrayList res = new ArrayList();
            if (treeAdmin.RuleAlerts.ContainsKey(id))
            {
                res = treeAdmin.RuleAlerts[id] as ArrayList;
            }
            return res;
        }
        private ArrayList GetDataFieldsArray()
        {
            ArrayList res = new ArrayList();
            if (treeAdmin.RuleData.ContainsKey(id))
            {
                res = treeAdmin.RuleData[id] as ArrayList;
            }
            return res;
        }
        #endregion

        #region public
        public ArrayList GetObjects(int objType)
        {
            ArrayList res = null;
            switch(objType)
            {
                case RuleTree.FIELDBIT:
                    res = GetFieldsArray();
                    break;
                case RuleTree.CONDITIONBIT:
                    res = GetConditionsArray();
                    break;
                case RuleTree.TASKBIT:
                    res = GetTasksArray();
                    break;
                case RuleTree.DOCUMENTBIT:
                    res = GetDocumentsArray();
                    break;
                case RuleTree.CHECKLISTBIT:
                    res = GetCheckListsArray();
                    break;
                case RuleTree.EVENTBIT:
                    res = GetEventsArray();
                    break;
                case RuleTree.ALERTBIT:
                    res = GetAlertsArray();
                    break;
                case RuleTree.DATABIT:
                    res = GetDataFieldsArray();
                    break;
                case RuleTree.CLOSINGINSTRACTIONBIT:
                    res = GetClosingInstructionsArray();
                    break;

            }
            return res;
        }
        #endregion

        #endregion
    }
    public class RuleEditNode
    {
        private static readonly DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);
        
        #region constants
        #region sp names
        private const string GETRULENONDEBYID = "GetRuleNodeById";
        private const string GETCHECKLIST = "GetCheckListById";
        private const string LOADUNITS = "GetRuleUnitListWithParent";
        private const string SAVE = "SaveRuleNode";
        private const string GETPARENTCATEGORY = "GetParentRuleCategory";
        private const string SAVERULEOBJECT = "SaveRuleNodeObject";
        private const string GETRULEOBJECTLIST = "GetRuleNodeObjectList";
        private const string DELETERULEOBJECT = "DeleteRuleNodeObject";
        private const string ENABLE = "EnableRuleNode";
        private const string DISABLE = "DisableRuleNode";
        private const string DELETE = "DeleteRuleFromTree";
        private const string COPYBRANCH = "CopyRuleNodeToLender";
        private const string SETNEWPARENT = "SetRuleParent";
        private const string CHANGEOBJECTPARENT = "ChangeRuleObjectParent";
        private const string SAVECONDITION = "SaveRuleNodeCondition";
        private const string SAVECCLOSINGINSTRUCTION = "SaveRuleNodeClosingInstruction";
        private const string SAVETASK = "SaveRuleNodeTask";
        private const string SAVECHECKLIST = "SaveRuleNodeCheckList";
        private const string SAVEALERT = "SaveRuleNodeAlert";
        private const string SAVEEVENT = "SaveRuleNodeEvent";
        private const string SAVERULEDATA = "SaveRuleNodeData";
        private const string SAVERULEDOCUMENT = "SaveRuleNodeDocument";
        private const string GETDOCUMENTLIST = "GetDocumentsForRuleNode";
        #endregion

        #region field's names
        private const string IDFIELDNAME = "id";
        //private const string STARTDATEFIELDNAME = "startdate";
        //private const string ENDDATEFIELDNAME = "enddate";
        private const string CATEGORYIDFIELDNAME = "categoryid";
        private const string STATUSIDFIELDNAME = "statusid";
        private const string PARENTIDFIELDNAME = "parentid";
        private const string CATEGORYNAMEFIELDNAME = "categoryname";
        private const string COMPANYIDFIELDNAME = "companyid";
        private const string COMMENTSFIELDNAME = "comments";
        #endregion

        private const int ENABLEDSTATUSID = 1;
        private const string HTMLSPACE = "&nbsp;";
        private const string COLORLOGICALOP = "green";
        private const string LOGICALOPFIELDNAME = "logicalop";
        private const string COLORLOGICALNOT = "green";
        private const string COLORFIELD = "red";
        private const string COMPAREOPFIELDNAME = "compareop";
        private const string COLORCOMPAREOP = "green";
        private const string LITERALVALUEFIELDNAME = "literalvalue";
        private const string COLORVALUE = "blue";
        private const string FIELDNAMEFIELDNAME = "fieldname";
        private const string COLOREDSTRING = "<span style='color:{0}'>{1}</span>";
        private const string NOTOP = "not";
        #endregion

        #region fields
        private int id = -1;
        private int categoryId = -1;
        private bool isEnabled = true;
        private string ruleName = String.Empty;
        private string ruleExpression = String.Empty;
        private DataView dvUnits;
        private int parentId = -1;
        private string categoryName = String.Empty;
        private DataView dvFields;
        private DataView dvConditions;
        private DataView dvTasks;
        private DataView dvDocuments;
        private DataView dvCheckLists;
        private DataView dvAlertEvents;
        private DataView dvData;
        private int companyId = 1;
        private string comments = String.Empty;
        private bool isReadOnly = false;
        #endregion

        #region properties
        public int Id
        {
            get { return id; }
        }
        public string RuleName
        {
            get
            {
                if(String.IsNullOrEmpty(ruleName))
                {
                    return "N/A";
                }
                return ruleName;
            }
        }
        public string RuleExpression
        {
            get { return ruleExpression; }
        }
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }
        public bool  IsRoot
        {
            get { return parentId == -1; }
        }
        public string CategoryName
        {

            get { return categoryName; }
            set { categoryName = value; }
        }
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }
        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }
        public int ParentId
        {
            get { return parentId; }
            set
            {
                parentId = value;
                if(parentId != -1)
                {
                    GetParentsProperties();
                }
            }
        }
        public bool HasUnits
        {
            get { return !String.IsNullOrEmpty(ruleName); }
        }
        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set { isReadOnly = value; }
        }
        public DataView DvUnits
        {
            get
            {
                if(dvUnits!=null)
                {
                    return dvUnits;
                }
                return new DataView();
            }
        }
        public DataView DvFields
        {
            get
            {   if(dvFields!=null)
                {
                    return dvFields;
                }
                return new DataView();
            }
        }
        public DataView DvConditions
        {
            get 
            { 
                if(dvConditions!=null)
                {
                    return dvConditions; 
                }
                return new DataView();
            }
        }
        public DataView DvTasks
        {
            get 
            { 
                if(dvTasks!=null)
                {
                    return dvTasks; 
                }
                return new DataView();
            }
        }
        public DataView DvDocuments
        {
            get 
            { 
                if(dvDocuments!=null)
                {
                    return dvDocuments; 
                }
                return new DataView();
            }
        }
        public DataView DvCheckLists
        {
            get
            {
                if(dvCheckLists!=null)
                {
                    return dvCheckLists;
                }
                return new DataView();               
            }
        }
        public DataView DvAlertEvents
        {
            get
            {
                if(dvAlertEvents!=null)
                {
                    return dvAlertEvents;
                }
                return new DataView();
            }
        }
        public DataView DvData
        {
            get 
            { 
                if(dvData!=null)
                {
                    return dvData; 
                }
                return new DataView();
            }
        }
        #endregion

        #region constructor
        public RuleEditNode(int _id)
        {
            id = _id;
            LoadById();
        }
        #endregion

        #region methods
        #region private
        private void LoadById()
        {
            if(id>0)
            {
                DataSet ds = db.GetDataSet(GETRULENONDEBYID,id);
                LoadRuleProperties(ds.Tables[0].DefaultView);
                dvUnits = ds.Tables[1].DefaultView;
                if(id>0)
                {
                    LoadUnits();
                    dvFields = ds.Tables[2].DefaultView;
                    dvConditions = ds.Tables[3].DefaultView;
                    dvTasks = ds.Tables[4].DefaultView;
                    dvDocuments = ds.Tables[5].DefaultView;
                    dvCheckLists = ds.Tables[6].DefaultView;
                    dvAlertEvents = ds.Tables[7].DefaultView;
                    dvData = ds.Tables[8].DefaultView;
                }
            }
        }
        private void LoadRuleProperties(DataView dv)
        {
            if (dv.Count == 1)
            {
                id = int.Parse(dv[0][IDFIELDNAME].ToString());
                categoryId = int.Parse(dv[0][CATEGORYIDFIELDNAME].ToString());
                isEnabled = int.Parse(dv[0][STATUSIDFIELDNAME].ToString())==ENABLEDSTATUSID;
                if(dv[0][PARENTIDFIELDNAME]!=DBNull.Value)
                {
                    parentId = int.Parse(dv[0][PARENTIDFIELDNAME].ToString());
                }
                categoryName = dv[0][CATEGORYNAMEFIELDNAME].ToString();
                companyId = int.Parse(dv[0][COMPANYIDFIELDNAME].ToString());
                comments = dv[0][COMMENTSFIELDNAME].ToString();
            }
            else
            {
                id = -1;
            }
        }
        private void LoadUnits()
        {
            ruleExpression = GetColoredCodeByDataView(dvUnits);
            dvUnits.RowFilter = String.Format("ruleid={0}",id);
            ruleName = GetColoredCodeByDataView(dvUnits);
        }
        private static string GetColoredCodeByDataView(DataView dv)
        {
            StringBuilder sb = new StringBuilder();
            string currentRuleExp = String.Empty;
            int currentRuleId = -1;
            bool firstUnit = true;
            for (int i = 0; i < dv.Count; i++)
            {
                string unit = GetUnit(dv[i]);
                int ruleId = int.Parse(dv[i]["ruleid"].ToString());
                if (ruleId != currentRuleId)
                {
                    if (currentRuleId != -1)
                    {
                        currentRuleExp = "[" + currentRuleExp + "]";
                        if (sb.Length > 0)
                        {
                            currentRuleExp = HTMLSPACE + GetColoredString("AND", COLORLOGICALOP) + HTMLSPACE + currentRuleExp;
                        }
                        sb.Append(currentRuleExp);
                        currentRuleExp = String.Empty;
                        firstUnit = true;
                    }
                    currentRuleId = ruleId;
                }
                if (!firstUnit)
                {
                    currentRuleExp += HTMLSPACE + GetColoredString(dv[i][LOGICALOPFIELDNAME].ToString(), COLORLOGICALOP) + HTMLSPACE;
                }
                currentRuleExp += unit;
                firstUnit = false;
            }
            if (!String.IsNullOrEmpty(currentRuleExp))
            {
                currentRuleExp = "[" + currentRuleExp + "]";
                if (sb.Length > 0)
                {
                    currentRuleExp = HTMLSPACE + GetColoredString("AND", COLORLOGICALOP) + HTMLSPACE + currentRuleExp;
                }
                sb.Append(currentRuleExp);
            }
            return sb.ToString();
        }
        private static string GetUnit(DataRowView row)
        {
            return "(" + GetColoredString(row[Field.LOGICALNOTFIELDNAME].ToString(), COLORLOGICALNOT)
                + GetColoredString(row[Field.DESCRIPTIONFIELDNAME].ToString(), COLORFIELD)
                + HTMLSPACE + GetColoredString(row[COMPAREOPFIELDNAME].ToString(), COLORCOMPAREOP) + HTMLSPACE
                + (bool.Parse(row[LITERALVALUEFIELDNAME].ToString()) ? GetColoredString(row[Field.DATAVALUEFIELDNAME].ToString(), COLORVALUE) : GetColoredString(row[FIELDNAMEFIELDNAME].ToString(), COLORFIELD))
                + ")";
        }
        private static string GetColoredString(string text, string color)
        {
            return String.Format(COLOREDSTRING, color, text.ToLower() == NOTOP ? text + HTMLSPACE : text);
        }
        private void ReloadUnits()
        {
            dvUnits = db.GetDataView(LOADUNITS,id);
            LoadUnits();
        }
        private void ReloadObjects(int objectTypeId)
        {
            switch(objectTypeId)
            {
                case Rule.FIELDOBJECTTYPEID:
                    dvFields = db.GetDataView(GETRULEOBJECTLIST, id, Rule.FIELDOBJECTTYPEID);
                    break;
                case Rule.CONDITIONOBJECTTYPEID:
                    dvConditions = db.GetDataView(GETRULEOBJECTLIST, id, Rule.CONDITIONOBJECTTYPEID);
                    break;
                case Rule.TASKOBJECTTYPEID:
                    dvTasks = db.GetDataView(GETRULEOBJECTLIST, id, Rule.TASKOBJECTTYPEID);
                    break;
                case Rule.CHECKLISTOBJECTTYPEID:
                    dvCheckLists = db.GetDataView(GETRULEOBJECTLIST, id, Rule.CHECKLISTOBJECTTYPEID);
                    break;
                case Rule.ALERTEVENTOBJECTTYPEID:
                    dvAlertEvents = db.GetDataView(GETRULEOBJECTLIST, id, Rule.ALERTEVENTOBJECTTYPEID);
                    break;
                case Rule.DATAOBJECTTYPEID:
                    dvData = db.GetDataView(GETRULEOBJECTLIST, id, Rule.DATAOBJECTTYPEID);
                    break;
                case Rule.DOCUMENTOBJECTTYPEID:
                    dvDocuments = db.GetDataView(GETRULEOBJECTLIST, id, Rule.DOCUMENTOBJECTTYPEID);
                    break;
                case Rule.CONDITIONCLOSINGINSTRUCTIONOBJECTTYPEID:
                    dvConditions = db.GetDataView(GETRULEOBJECTLIST, id, Rule.CONDITIONCLOSINGINSTRUCTIONOBJECTTYPEID);
                    break;
            }
        }
        private void GetParentsProperties()
        {
            GetParentCategory();
            GetParentExpression();
        }
        private void GetParentCategory()
        {
            DataView dv = db.GetDataView(GETPARENTCATEGORY, parentId);
            if (dv.Count == 1)
            {
                categoryName = dv[0][CATEGORYNAMEFIELDNAME].ToString();
                categoryId = int.Parse(dv[0][CATEGORYIDFIELDNAME].ToString());
            }
        }
        private void GetParentExpression()
        {
            DataView dv = db.GetDataView(LOADUNITS, parentId);
            ruleExpression = GetColoredCodeByDataView(dv);
        }
        #endregion
        #region public
        public bool Save()
        {
            id = db.ExecuteScalarInt(SAVE, id, companyId, categoryId, categoryName, isEnabled ? 1 : 2, comments, parentId == -1 ? (object)DBNull.Value : parentId);
            return id > 0;
        }
        public static DataView GetCheckList(int checklistid, out string title)
        {
            title=String.Empty;
            DataSet ds = db.GetDataSet(GETCHECKLIST, checklistid);
            if (ds.Tables.Count == 2)
            { 
                DataTable dt = ds.Tables[1];
                if (dt.Rows.Count == 1)
                {
                    title = dt.Rows[0]["Title"].ToString();
                }                
            }
            return ds.Tables[0].DefaultView;
//            return db.GetDataView(GETCHECKLIST, checklistid);
        }
        public bool AddUnit(RuleUnit unit)
        {
            if(id<1)
            {
                if(!Save())
                {
                    return false;
                }
                LoadById();
            }
            unit.RuleId = id;
            int unitId = unit.Save();
            if(unitId>0)
            {
                ReloadUnits();
                return true;
            }
            return false;
        }
        public bool DeleteUnit(RuleUnit unit)
        {
            if(unit.Delete())
            {
                ReloadUnits();
                return true;
            }
            return false;
        }
        public bool AddObject(int objectId, int objectTypeId)
        {
            bool res = db.ExecuteScalarInt(SAVERULEOBJECT, id, objectId, objectTypeId) == 1;
            if(res)
            {
                ReloadObjects(objectTypeId);
            }
            return res;
        }
        public bool DeleteObject(int objectId, int objectTypeId)
        {
            bool res = (db.ExecuteScalarInt(DELETERULEOBJECT, objectId) == 1);
            if(res)
            {
                ReloadObjects(objectTypeId);
            }
            return res;
        }
        public bool AddCondition(RuleCondition rc)
        {
            bool res = db.ExecuteScalarInt(SAVECONDITION, id, rc.Id, rc.Title, rc.Detail, rc.RoleId, rc.TypeId, rc.CategoryId)>0;
            if(res)
            {
                ReloadObjects(Rule.CONDITIONCLOSINGINSTRUCTIONOBJECTTYPEID);
            }
            return res;
        }
        public bool AddClosingInstruction(RuleClosingInstruction rc)
        {
            bool res = db.ExecuteScalarInt(SAVECCLOSINGINSTRUCTION, id, rc.Id, rc.Title, rc.Detail, rc.RoleId) > 0;
            if (res)
            {
                ReloadObjects(Rule.CONDITIONCLOSINGINSTRUCTIONOBJECTTYPEID);
            }
            return res;
        }
        public bool AddTask(RuleTask rt)
        {
            bool res = db.ExecuteScalarInt(SAVETASK, id, rt.Id, rt.Title, rt.Description, rt.TypeId, rt.InfoSourceId, rt.DifficultyId)>0;
            if(res)
            {
                ReloadObjects(Rule.TASKOBJECTTYPEID);
            }
            return res;
        }
        public bool AddCheckList(int checklistId, string data, string title)
        {
            bool res = db.ExecuteScalarInt(SAVECHECKLIST, id, checklistId,title, data) > 0;
            if (res)
            {
                ReloadObjects(Rule.CHECKLISTOBJECTTYPEID);
            }
            return res;
        }
        public bool AddAlert(int alertId,string message)
        {
            bool res = db.ExecuteScalarInt(SAVEALERT, id, alertId, message) > 0;
            if (res)
            {
                ReloadObjects(Rule.ALERTEVENTOBJECTTYPEID);
            }
            return res;
        }
        public bool AddEvent(int eventId,string message,int eventTypeId)
        {
            bool res = db.ExecuteScalarInt(SAVEEVENT, id, eventId, message, eventTypeId) > 0;
            if(res)
            {
                ReloadObjects(Rule.ALERTEVENTOBJECTTYPEID);
            }
            return res;
        }
        public bool CheckEvent(int objectId)
        {
            bool res = false;
            DataRow[] rows = DvAlertEvents.Table.Select(String.Format("id={0}", objectId));
            if(rows.Length==1)
            {
                res = rows[0]["type"].ToString().ToLower() == "event";
            }
            return res;
        }
        public bool AddData(int objectId,int fieldId,string fieldValue)
        {
            bool res = db.ExecuteScalarInt(SAVERULEDATA, id, objectId, fieldId, fieldValue) > 0;
            if(res)
            {
                ReloadObjects(Rule.DATAOBJECTTYPEID);
            }
            return res;
        }
        public bool AddDocument(int objectId,int documentTemplateId, bool isAppPackage, bool isClosingPackage,bool isMiscPackage)
        {
            bool res = db.ExecuteScalarInt(SAVERULEDOCUMENT, id, objectId, documentTemplateId, isAppPackage, isClosingPackage,isMiscPackage)>0;
            if(res)
            {
                ReloadObjects(Rule.DOCUMENTOBJECTTYPEID);
            }
            return res;
        }

        public DataView GetDocumentList(int docId)
        {
            return db.GetDataView(GETDOCUMENTLIST, id, docId);
        }
        public static bool Enable(int ruleNodeId)
        {
            return db.ExecuteScalarInt(ENABLE, ruleNodeId) > 0;
        }
        public static bool Disable(int ruleNodeId)
        {
            return db.ExecuteScalarInt(DISABLE, ruleNodeId) > 0;
        }
        public static bool Delete(int ruleNodeId)
        {
            return db.ExecuteScalarInt(DELETE, ruleNodeId) > 0;
        }
        public static bool CopyBranch(int ruleNodeId, int lenderId)
        {
            return db.ExecuteScalarInt(COPYBRANCH, ruleNodeId, lenderId) > 0;
        }
        public static bool SetNewParent(int ruleNodeId, int parentRuleNodeId)
        {
            return db.ExecuteScalarInt(SETNEWPARENT, ruleNodeId, parentRuleNodeId == -1 ? (object)DBNull.Value : parentRuleNodeId) == 1;
        }
        public static bool ChangeObjectParent(int id, int parentId)
        {
            return db.ExecuteScalarInt(CHANGEOBJECTPARENT, id, parentId) == 1;
        }

        #endregion
        #endregion

    }
    public class RuleEvaluationNode
    {
        #region fields
        private int id;
        private RuleEvaluationNode parentNode;
        private ArrayList childNodes;
        private int ruleObjectsMask = 0;
        private readonly int currentRuleObjects = 0;
        private IList ruleIListObject = null;
        private Hashtable evalValues;
        private bool evalValue = false;
        private bool orResult = false;
        private bool andResult = true;
        private readonly Hashtable skipObjects;
        private readonly Hashtable parentSkippedObjects;
        private readonly RuleEvaluationTree ret;
        #endregion

        #region properties
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public RuleEvaluationNode ParentNode
        {
            get { return parentNode; }
            set { parentNode = value; }
        }
        public int RuleObjectsMask
        {
            get { return ruleObjectsMask; }
            set { ruleObjectsMask = value; }
        }
        public ArrayList ChildNodes
        {
            get { return childNodes; }
            set { childNodes = value; }
        }
        public Hashtable SkipObjects
        {
            get { return skipObjects; }
        }
        #endregion

        #region constructor
        public RuleEvaluationNode(RuleNode rn, RuleEvaluationNode parent, RuleEvaluationTree _ret)
        {
            id = rn.Id;
            ruleObjectsMask = rn.RuleObjectsMask;
            currentRuleObjects = rn.CurrentRuleObjects;
            parentNode = parent;
            ret = _ret;
            if (parent != null)
            {
                parentSkippedObjects = parent.skipObjects;
            }
            else
            {
                parentSkippedObjects = new Hashtable();
            }
            skipObjects = new Hashtable();
        }
        #endregion

        #region methods


        #region evaluation methods
        public void EvaluateActions(MortgageProfile mp, RuleNodePublic rn)
        {
            orResult = false;
            andResult = true;
            ArrayList units = rn.Units;
            evalValues = new Hashtable();
            for (int i = 0; i < units.Count; i++)
            {
                RuleEvaluationUnit reu = (RuleEvaluationUnit)units[i];
                reu.ClearResults();
                reu.EvaluateWithVisibility(mp);
                if (reu.IListObject != null)
                {
                    ruleIListObject = reu.IListObject;
                }
            }
            SetResult(units, mp);
            if (orResult)
            {
                SetData(mp, rn);
                SetActions(mp);
                EvaluateChilds(mp, rn, ret);
            }
//            mp.AddRuleFields(currentRuleObjects, GetFields(units));
            mp.AddRuleFields(ruleObjectsMask, GetFields(units));
        }
        #region 
        public void EvaluateVisibility(MortgageProfile mp, RuleNodePublic rn)
        {
            orResult = false;
            andResult = true;
            ArrayList units = rn.Units;
            evalValues = new Hashtable();
            for (int i = 0; i < units.Count; i++)
            {
                RuleEvaluationUnit reu = (RuleEvaluationUnit)units[i];
                reu.ClearResults();
                reu.Evaluate(mp);
                if (reu.IListObject != null)
                {
                    ruleIListObject = reu.IListObject;
                }
            }
            SetResult(units, mp);
            Hashtable fields = rn.GetVisibilityDependantFields();
#if DUMPRULES
            mp.CurrentPage.WriteDebugToLog(String.Format("---------------" + System.Environment.NewLine + "Show fields action for rule {0}, fields count={1}", rn.Id, fields != null ? fields.Count.ToString() : "N/A"));
#endif
            if(orResult)
            {
                mp.SetVisibleField(fields, skipObjects);
                SetData(mp, rn);
                SetActions(mp);
                EvaluateChilds(mp, rn, ret);
            }
            else
            {
#if DUMPRULES
                if(fields!=null)
                {
                    foreach (DictionaryEntry item in fields)
                    {
                        string key = item.Key.ToString();
                        mp.CurrentPage.WriteDebugToLog(key+"\t\t\t - skipped");
                    }
                }
                if((rn.ChildNodes!=null)&&(rn.ChildNodes.Count>0))
                {
                    string message = String.Empty;
                    for(int i=0;i<rn.ChildNodes.Count;i++)
                    {
                        if(i!=0)
                        {
                            message += ",";
                        }
                        message +=((RuleNodePublic)rn.ChildNodes[i]).Id.ToString();
                    }
                    mp.CurrentPage.WriteDebugToLog("---------------" + System.Environment.NewLine + "Child rules - " + message + " - skipped");
                }
                
#endif
            }
//            mp.AddRuleFields(currentRuleObjects, GetFields(units));
            mp.AddRuleFields(ruleObjectsMask, GetFields(units));
        }
        private static ArrayList GetFields(IList units)
        {
            ArrayList res = new ArrayList();
            for (int i = 0; i < units.Count;i++)
            {
                RuleEvaluationUnit reu = (RuleEvaluationUnit)units[i];
                res.Add(reu.PropertyName);
            }
            return res;
        }
        #endregion
        private void SetData(MortgageProfile mp, RuleNodePublic rn)
        {
            if ((currentRuleObjects & RuleTree.DATABIT) != 0)
            {
                ArrayList list = rn.GetDataDependantFields();
                if ((list != null) && (list.Count > 0))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        RuleDataValue rdv = list[i] as RuleDataValue;
                        if (rdv != null)
                        {
                            ArrayList data = rdv.SetValue(mp, this);
                            if (data != null)
                            {
                                for (int k = 0; k < data.Count; k++)
                                {
                                    ret.DataObjects.Add(data[k]);
                                    RuleDataValueObject rdvo = (RuleDataValueObject)data[k];
                                    if (rdvo != null)
                                    {
                                        string err;
                                        if (mp.UpdateObject(rdvo.FullName, rdvo.ObjectValue, rdvo.ObjectId, out err))
                                        {
                                            mp.AddDataSetField(rdvo);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void SetActions(MortgageProfile mp)
        {
            if (currentRuleObjects > 1)
            {
                mp.AddRule(currentRuleObjects, id);
            }
        }

        private void SetResult(IList units, MortgageProfile mp)
        {
            if ((units != null) && (units.Count > 0))
            {
                if (ruleIListObject != null)
                {
                    SetListValue(units);
                }
                else
                {
                    SetScalarValue(units, mp);
                }
            }
        }
        private void SetScalarValue(IList units, Object obj)
        {
            bool res = ((RuleEvaluationUnit)units[0]).Result;
            for (int i = 1; i < units.Count; i++)
            {
                bool b1 = res;
                bool b2 = ((RuleEvaluationUnit)units[i]).Result;
                switch (((RuleEvaluationUnit)units[i]).LogicalOpId)
                {
                    case (int)RuleEvaluationUnit.LogicalOperation.AND:
                        res = b1 && b2;
                        break;
                    case (int)RuleEvaluationUnit.LogicalOperation.OR:
                        res = b1 || b2;
                        break;
                }
            }
            evalValue = res;
            evalValues.Add(obj, evalValue);
            orResult = evalValue;
            andResult = evalValue;
        }
        private void SetListValue(IList units)
        {
            orResult = false;
            andResult = true;
            foreach (Object obj in ruleIListObject)
            {
                bool res = false;
                if (!parentSkippedObjects.ContainsKey(obj))
                {
                    res = ((RuleEvaluationUnit)units[0]).GetResult(obj);
                }
                for (int i = 1; i < units.Count; i++)
                {
                    bool b1 = res;
                    bool b2 = false;
                    if (!parentSkippedObjects.ContainsKey(obj))
                    {
                        b2 = ((RuleEvaluationUnit)units[i]).GetResult(obj);
                    }
                    switch (((RuleEvaluationUnit)units[i]).LogicalOpId)
                    {
                        case (int)RuleEvaluationUnit.LogicalOperation.AND:
                            res = b1 && b2;
                            break;
                        case (int)RuleEvaluationUnit.LogicalOperation.OR:
                            res = b1 || b2;
                            break;
                    }
                }
                if (!res)
                {
                    if (!skipObjects.ContainsKey(obj))
                    {
                        skipObjects.Add(obj, false);
                    }
                }
                evalValues.Add(obj, res);
                orResult = orResult || res;
                andResult = andResult && res;
            }
            evalValue = orResult;
        }
        private void EvaluateChilds(MortgageProfile mp, RuleNode rn, RuleEvaluationTree _ret)
        {
            if (rn.ChildNodes != null)
            {
                childNodes = new ArrayList();
                for (int i = 0; i < rn.ChildNodes.Count; i++)
                {
                    RuleNodePublic rnc = (RuleNodePublic)rn.ChildNodes[i];
                    RuleEvaluationNode ren = new RuleEvaluationNode(rnc, this, _ret);
                    ren.EvaluateVisibility(mp, rnc);
                }
            }
        }
        #endregion

        #endregion
    }
    public class RuleDataValue
    {
        #region fields
        private readonly string fullName = String.Empty;
        private readonly string objectName = String.Empty;
        private string propertyName = String.Empty;
        private readonly string dataValue = String.Empty;
        #endregion

        #region constructor
        public RuleDataValue(string _fullName, string _dataValue)
        {
            fullName = _fullName;
            dataValue = _dataValue;
            objectName = String.Empty;
            int i = fullName.IndexOf(".");
            if (i > 0)
            {
                objectName = fullName.Substring(0, i);
                propertyName = fullName.Substring(i + 1);
            }
            else
            {
                propertyName = fullName;
            }
        }
        #endregion

        #region methods
        public ArrayList SetValue(MortgageProfile mp, RuleEvaluationNode ren)
        {
            ArrayList res = new ArrayList();
            Object propValue = mp.GetType().GetProperty(objectName).GetValue(mp, null);
            if (propValue is IList)
            {
                IList list = (IList)propValue;
                foreach (Object item in list)
                {
                    BaseObject obj = item as BaseObject;
                    if (ren.SkipObjects != null)
                    {
                        if (obj != null)
                        {
                            if (!ren.SkipObjects.ContainsKey(obj))
                            {
                                RuleDataValueObject rdvo = new RuleDataValueObject(obj.ID, fullName, dataValue);
                                res.Add(rdvo);
                            }
                        }
                    }
                }
            }
            else
            {
                BaseObject obj = propValue as BaseObject;
                if (obj != null)
                {
                    RuleDataValueObject rdvo = new RuleDataValueObject(obj.ID, fullName, dataValue);
                    res.Add(rdvo);
                }
            }
            return res;
        }

        #endregion
    }
    public class RuleDataValueObject
    {
        #region fields
        private readonly int objectId = -1;
        private readonly string fullName = String.Empty;
        private readonly string objectValue = String.Empty;
        #endregion

        #region properties
        public string FullName
        {
            get { return fullName; }
        }
        public int ObjectId
        {
            get { return objectId; }
        }
        public string ObjectValue
        {
            get { return objectValue; }
        }
        #endregion

        #region constructor
        public RuleDataValueObject(int _objectId, string _fullName, string _objectValue)
        {
            objectId = _objectId;
            fullName = _fullName;
            objectValue = _objectValue;
        }
        #endregion
    }
    public class RuleEvaluationUnit
    {

        public enum CompareOperation
        {
            GT = 1,
            LT = 2,
            EQ = 3,
            NOTEQ = 4,
            GET = 5,
            LET = 6
        }
        public enum LogicalOperation
        {
            AND = 1,
            OR = 2,
        }

        #region constants
        private const string LOGICALOPIDFIELDNAME = "logicalopid";
        private const string IDFIELDNAME = "id";
        private const string COMPAREOPIDFIELDNAME = "compareopid";
        private const string PROPERTYVALUEFIELDNAME = "propertyvalue";
        private const string PROPERTYNAMEFIELDNAME = "propertyname";
        private const string LOGICALNOTFIELDNAME = "logicalnot";
        private const string LITERALVALUEFIELDNAME = "literalvalue";
        private const string DATAVALUEFIELDNAME = "datavalue";
        private const string DESCRIPTIONFIELDNAME = "description";
        private const string IGNOREVISIBILITYFIELDNAME = "IgnoreVisibiltyForRules";
        #endregion

        #region fields
        private readonly int id = -1;
        private readonly int logicalOpId = -1;
        private readonly int compareOpId = -1;
        private readonly string propertyValue = String.Empty;
        private readonly string fullName = String.Empty;
        private readonly string propertyName = String.Empty;
        private readonly string objectName = String.Empty;
        private bool logicalNot;
        private readonly bool literalValue = true;
        private readonly string dataValue = String.Empty;
        private bool result;
        private Hashtable results = new Hashtable();
        private IList ilistObject = null;
        private readonly string description = String.Empty;
        private readonly bool isAlwaysEvaluated = false;
        #endregion

        #region properties
        public int Id
        {
            get { return id; }
        }
        public int LogicalOpId
        {
            get { return logicalOpId; }
        }
        public bool LogicalNot
        {
            get { return logicalNot; }
            set { logicalNot = value; }
        }
        public bool Result
        {
            get { return result; }
            set { result = value; }
        }
        public Hashtable Results
        {
            get { return results; }
        }
        public void ClearResults()
        {
            results = new Hashtable();
            result = false;
        }

        public bool GetResult(Object o)
        {
            bool res = Result;
            if (o != null)
            {
                if (results.ContainsKey(o))
                {
                    res = (bool)results[o];
                }
            }
            return res;
        }
        public void SetResult(Object o, bool val)
        {
            if (results.ContainsKey(o))
            {
                results[o] = val;
            }
            else
            {
                results.Add(o, val);
            }
            result |= val;
        }
        public string PropertyName
        {
            get { return fullName; }
        }
        public string PropertyValue
        {
            get { return propertyValue; }
        }
        public int CompareOpId
        {
            get { return compareOpId; }
        }
        public bool LiteralValue
        {
            get { return literalValue; }
        }
        public IList IListObject
        {
            get { return ilistObject; }
            set { ilistObject = value; }
        }
        #endregion

        #region constructor
        public RuleEvaluationUnit(DataRowView dr)
        {
            if (dr.Row.Table.Columns.Contains(IDFIELDNAME))
            {
                id = int.Parse(dr[IDFIELDNAME].ToString());
            }
            logicalOpId = int.Parse(dr[LOGICALOPIDFIELDNAME].ToString());
            compareOpId = int.Parse(dr[COMPAREOPIDFIELDNAME].ToString());
            propertyValue = dr[PROPERTYVALUEFIELDNAME].ToString();
            fullName = dr[PROPERTYNAMEFIELDNAME].ToString();
            logicalNot = bool.Parse(dr[LOGICALNOTFIELDNAME].ToString());
            literalValue = bool.Parse(dr[LITERALVALUEFIELDNAME].ToString());
            if (dr.Row.Table.Columns.Contains(DATAVALUEFIELDNAME))
            {
                dataValue = dr[DATAVALUEFIELDNAME].ToString();
            }
            if (dr.Row.Table.Columns.Contains(DESCRIPTIONFIELDNAME))
            {
                description = dr[DESCRIPTIONFIELDNAME].ToString();
            }
            if (dr.Row.Table.Columns.Contains(IGNOREVISIBILITYFIELDNAME))
            {
                isAlwaysEvaluated = bool.Parse(dr[IGNOREVISIBILITYFIELDNAME].ToString());
            }
            objectName = String.Empty;
            int i = fullName.IndexOf(".");
            if (i > 0)
            {
                objectName = fullName.Substring(0, i);
                propertyName = fullName.Substring(i + 1);
            }
        }
        #endregion

        #region evaluation methods
        public void Evaluate(Object o)
        {
            Object obj = o;
            Hashtable propValue = GetObjectValue(obj);
            Hashtable propValue2;
            if (!literalValue)
            {
                Object refObject;
                propValue2 = GetInderectObjectValue(obj, propertyValue, out refObject);
                IList list = refObject as IList;
                if(ilistObject==null)
                {
                    Object objectVal = null;
                    Object objectKey = null;
                    foreach (DictionaryEntry de in propValue)
                    {
                        objectVal = de.Value;
                        objectKey = de.Key;
                        break;
                    }
                    if(list==null)
                    {
                        Object objectInderectValue = null;
                        foreach (DictionaryEntry de in propValue2)
                        {
                            objectInderectValue = de.Value;
                            break;
                        }
                        bool res = CompareValue(objectVal, objectInderectValue);
                        SetResult(objectKey, res);
                    }
                    else
                    {
                        bool res = true;
                        foreach(DictionaryEntry item in propValue2)
                        {
                            bool b = CompareValue(objectVal, item.Value);
                            res = res && b;
                        }
                        SetResult(objectKey, res);
                    }
                }
                else
                {
                    if(list==null)
                    {
                        Object objectInderectValue = null;
                        foreach (DictionaryEntry de in propValue2)
                        {
                            objectInderectValue = de.Value;
                            break;
                        }
                        foreach (DictionaryEntry item in propValue)
                        {
                            bool res = CompareValue(item.Value, objectInderectValue);
                            SetResult(item.Key, res);
                        }
                    }
                    else
                    {
                        if(ilistObject[0].GetType()==list[0].GetType()){
                            
                            foreach(DictionaryEntry item in propValue)
                            {
                                bool res = false;
                                if(propValue2.ContainsKey(item.Key))
                                {
                                    res = CompareValue(item.Value, propValue2[item.Key]);
                                }
                                SetResult(item.Key, res);
                            }
                        }
                    }
                }
            }
            else
            {
                propValue2 = new Hashtable();
                propValue2.Add(obj, propertyValue);
                foreach (DictionaryEntry item in propValue)
                {
                    bool res = CompareValue(item.Value, propValue2[obj]);
                    SetResult(item.Key, res);
                }
            }
        }
        public void EvaluateWithVisibility(Object o)
        {
            MortgageProfile mp = o as MortgageProfile;
            if ((mp != null) && (mp.VisibleFields != null) && (mp.VisibleFields.Count > 0))
            {
                bool needCalculation = isAlwaysEvaluated;
                if(!needCalculation)
                {
                    needCalculation = mp.VisibleFields.ContainsKey(fullName);
                }
                if(needCalculation)
                {
                    Object obj = o;
                    Hashtable propValue = GetObjectValue(obj);
                    Hashtable propValue2;
                    if (!literalValue)
                    {
                        Object refObject;
                        propValue2 = GetInderectObjectValue(obj, propertyValue, out refObject);
                        IList olist = mp.GetObjectList(mp.GetObjectName(fullName));
                        bool isList = olist != null;
                        Hashtable ht = mp.VisibleFields[fullName] as Hashtable;
                        foreach (DictionaryEntry item in propValue)
                        {
                            bool res;
                            if (isList)
                            {
                                if ((ht != null) && ht.ContainsKey(item.Key))
                                {
                                    res = CompareValue(item.Value, propValue2[obj]);
                                }
                                else
                                {
                                    res = false;
                                }
                            }
                            else
                            {
                                res = CompareValue(item.Value, propValue2[obj]);
                            }
                            SetResult(item.Key, res);
                        }
                    }
                    else
                    {
                        propValue2 = new Hashtable();
                        propValue2.Add(obj, propertyValue);
                        IList olist = mp.GetObjectList(mp.GetObjectName(fullName));
                        bool isList = olist != null;
                        Hashtable ht = mp.VisibleFields[fullName] as Hashtable;
                        foreach (DictionaryEntry item in propValue)
                        {
                            bool res;
                            if (isList)
                            {
                                if ((ht != null) && ht.ContainsKey(item.Key))
                                {
                                    res = CompareValue(item.Value, propValue2[obj]);
                                }
                                else
                                {
                                    res = false;
                                }
                            }
                            else
                            {
                                res = CompareValue(item.Value, propValue2[obj]);
                            }
                            SetResult(item.Key, res);
                        }
                    }
                }
            }
        }
        private static Hashtable GetInderectObjectValue(Object obj, string propertyName, out Object refObject)
        {
            refObject = null;
            Hashtable res = new Hashtable();
            string objectName = String.Empty;
            string propName = propertyName;
            int k = propName.IndexOf(".");
            if (k > 0)
            {
                objectName = propName.Substring(0, k);
                propName = propName.Substring(k + 1);
            }
            if (!String.IsNullOrEmpty(objectName))
            {
                Object propValue = obj.GetType().GetProperty(objectName).GetValue(obj, null);
                refObject = propValue;
                if (propValue is IList)
                {
                    IList ilist = (IList)propValue;
                    for (int i = 0; i < ilist.Count; i++)
                    {
                        Object item = ilist[i];
                        Object val = item.GetType().GetProperty(propName).GetValue(item, null);
                        res.Add(item, val);
                    }
                }
                else
                {
                    res.Add(obj, propValue.GetType().GetProperty(propName).GetValue(propValue, null));
                }
            }
            else
            {
                res.Add(obj, obj.GetType().GetProperty(propName).GetValue(obj, null));
            }
            return res;
        }
        private Hashtable GetObjectValue(Object obj)
        {
            Hashtable res = new Hashtable();
            if (!String.IsNullOrEmpty(objectName))
            {
                Object propValue = obj.GetType().GetProperty(objectName).GetValue(obj, null);
                if (propValue is IList)
                {
                    ilistObject = (IList)propValue;
                    for (int i = 0; i < ilistObject.Count; i++)
                    {
                        Object item = ilistObject[i];
                        Object val = item.GetType().GetProperty(propertyName).GetValue(item, null);
                        res.Add(item, val);
                    }
                }
                else
                {
                    res.Add(propValue, propValue.GetType().GetProperty(propertyName).GetValue(propValue, null));
                }
            }
            else
            {
                res.Add(obj, obj.GetType().GetProperty(propertyName).GetValue(obj, null));
            }
            return res;
        }
        private bool CompareValue(Object val1, Object val2)
        {
            bool res = false;
//            try
//            {
            if ((val1 != null) && (val2 != null))
            {
                Type t = val1.GetType();
                if (t == typeof(DateTime))
                {
                    DateTime mpf = (DateTime)val1;
                    DateTime val = DateTime.Parse(val2.ToString());
                    switch (compareOpId)
                    {
                        case (int)CompareOperation.EQ:
                            res = mpf == val;
                            break;
                        case (int)CompareOperation.NOTEQ:
                            res = mpf != val;
                            break;
                        case (int)CompareOperation.GT:
                            res = mpf > val;
                            break;
                        case (int)CompareOperation.LT:
                            res = mpf < val;
                            break;
                        case (int)CompareOperation.GET:
                            res = mpf >= val;
                            break;
                        case (int)CompareOperation.LET:
                            res = mpf <= val;
                            break;
                    }
                }
                else if (t == typeof(string))
                {
                    string mpf = (string)val1;
                    string val = (string)val2;
                    switch (compareOpId)
                    {
                        case (int)CompareOperation.EQ:
                            res = mpf == val;
                            break;
                        case (int)CompareOperation.NOTEQ:
                            res = mpf != val;
                            break;
                    }
                }
                else if (t == typeof(int))
                {
                    int mpf = Convert.ToInt32(val1);
                    int val = Convert.ToInt32(val2);
                    switch (compareOpId)
                    {
                        case (int)CompareOperation.EQ:
                            res = mpf == val;
                            break;
                        case (int)CompareOperation.NOTEQ:
                            res = mpf != val;
                            break;
                        case (int)CompareOperation.GT:
                            res = mpf > val;
                            break;
                        case (int)CompareOperation.LT:
                            res = mpf < val;
                            break;
                        case (int)CompareOperation.GET:
                            res = mpf >= val;
                            break;
                        case (int)CompareOperation.LET:
                            res = mpf <= val;
                            break;
                    }
                }
                else if (t == typeof(bool))
                {
                    bool mpf = (bool)val1;
                    bool val = (String.Compare(val2.ToString(), "yes", true) == 0);
                    switch (compareOpId)
                    {
                        case (int)CompareOperation.EQ:
                            res = mpf == val;
                            break;
                        case (int)CompareOperation.NOTEQ:
                            res = mpf != val;
                            break;
                    }
                }
                else if (t == typeof(float))
                {
                    float mpf = (float)val1;
                    float val = float.Parse(val2.ToString());
                    switch (compareOpId)
                    {
                        case (int)CompareOperation.EQ:
                            res = mpf == val;
                            break;
                        case (int)CompareOperation.NOTEQ:
                            res = mpf != val;
                            break;
                        case (int)CompareOperation.GT:
                            res = mpf > val;
                            break;
                        case (int)CompareOperation.LT:
                            res = mpf < val;
                            break;
                        case (int)CompareOperation.GET:
                            res = mpf >= val;
                            break;
                        case (int)CompareOperation.LET:
                            res = mpf <= val;
                            break;
                    }
                }
                else if (t == typeof(decimal))
                {
                    decimal mpf = (decimal)val1;
                    decimal val = decimal.Parse(val2.ToString());
                    switch (compareOpId)
                    {
                        case (int)CompareOperation.EQ:
                            res = mpf == val;
                            break;
                        case (int)CompareOperation.NOTEQ:
                            res = mpf != val;
                            break;
                        case (int)CompareOperation.GT:
                            res = mpf > val;
                            break;
                        case (int)CompareOperation.LT:
                            res = mpf < val;
                            break;
                        case (int)CompareOperation.GET:
                            res = mpf >= val;
                            break;
                        case (int)CompareOperation.LET:
                            res = mpf <= val;
                            break;
                    }
                }
            }
            if (logicalNot)
            {
                res = !res;
            }
            //}
            //catch
            //{
            //}
            return res;
        }
        #endregion

        public string GetUnitExpression(bool firstUnit)
        {
            string res = String.Empty;
            string logOp = String.Empty;
            if (!firstUnit)
            {
                logOp = GetLogicalOperation();
            }
            res += description + GetCompareOperation() + dataValue;
            res = logOp + "(" + res + ")";
            if (logicalNot)
            {
                res = " NOT " + res;
            }
            return res;
        }
        private string GetLogicalOperation()
        {
            string res = String.Empty;
            switch (logicalOpId)
            {
                case (int)LogicalOperation.AND:
                    res = " AND ";
                    break;
                case (int)LogicalOperation.OR:
                    res = " OR ";
                    break;
            }
            return res;
        }
        private string GetCompareOperation()
        {
            string res = String.Empty;
            switch (compareOpId)
            {
                case (int)CompareOperation.EQ:
                    res = " = ";
                    break;
                case (int)CompareOperation.GET:
                    res = " >= ";
                    break;
                case (int)CompareOperation.GT:
                    res = " > ";
                    break;
                case (int)CompareOperation.LET:
                    res = " <= ";
                    break;
                case (int)CompareOperation.LT:
                    res = " < ";
                    break;
                case (int)CompareOperation.NOTEQ:
                    res = " <> ";
                    break;
            }
            return res;
        }
    }

    public class RuleUnit
    {
        #region constants

        #region sp names
        private const string SAVE = "SaveRuleUnit";
        private const string DELETE = "DeleteRuleUnit";
        #endregion

        #endregion

        #region fields
        private static readonly DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);
        private int id = -1;
        private int ruleId = -1;
        private int logicalOpId;
        private int fieldId = -1;
        private int compareOpId;
        private string dataValue = String.Empty;
        private int referanceId = -1;
        private bool logicalNot;
        private string propertyName = String.Empty;
        private bool literalValue = true;
        #endregion

        #region properties
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public int RuleId
        {
            get { return ruleId; }
            set { ruleId = value; }
        }
        public int LogicalOpId
        {
            get { return logicalOpId; }
            set { logicalOpId = value; }
        }
        public int CompareOpId
        {
            get { return compareOpId; }
            set { compareOpId = value; }
        }
        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }
        public string DataValue
        {
            get { return dataValue; }
            set { dataValue = value; }
        }
        public int ReferanceId
        {
            get { return referanceId; }
            set { referanceId = value; }
        }
        public bool LogicalNot
        {
            get { return logicalNot; }
            set { logicalNot = value; }
        }
        public int FieldId
        {
            get { return fieldId; }
            set { fieldId = value; }
        }
        public bool LiteralValue
        {
            get { return literalValue; }
            set { literalValue = value; }
        }
        #endregion

        #region methods

        #region public methods

        #region instance methods
        public int Save()
        {
            id = db.ExecuteScalarInt(SAVE, id, ruleId, logicalOpId, fieldId, compareOpId, dataValue, referanceId, logicalNot, literalValue);
            return id;
        }
        public bool Delete()
        {
            return db.ExecuteScalarInt(DELETE, id) == 1;
        }
        #endregion

        #endregion
        #endregion

    }
}
