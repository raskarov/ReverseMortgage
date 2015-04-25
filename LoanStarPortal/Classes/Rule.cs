using System;
using System.Collections;
using System.Data;
using BossDev.CommonUtils;

namespace LoanStar.Common
{
    public class Rule :BaseObject
    {
        #region constants
        
        #region field names
//        private const string ROOTELEMENT = "Root";
//        private const string ITEMELEMENT = "item";
//        private const string IDATTRIBUTE = "id";
        //public const string IDFIELDNAME = "id";
        //public const string NAMEFIELDNAME = "name";
        //public const string STARTDATEFIELDNAME = "startdate";
        //public const string ENDDATEFIELDNAME = "enddate";
        //public const string RULEIDFIELDNAME = "ruleid";
        //public const string FIELDIDFIELDNAME = "fieldid";
        //public const string COMPANYIDFIELDNAME = "companyid";
        //public const string CATEGORYIDFIELDNAME = "categoryid";
        //public const string STATUSIDFIELDNAME = "statusid";
        //private const string LOGICALOPFIELDNAME = "logicalop";
        //private const string COMPAREOPFIELDNAME = "compareop";
        //private const string LITERALVALUEFIELDNAME = "literalvalue";
        //private const string FIELDNAMEFIELDNAME = "fieldname";
        //private const string PROPERTYNAMEFIELDNAME = "propertyname";
        //private const string PARENTIDFIELDNAME = "parentid";
        #endregion

        #region sp names
//        private const string SAVE           = "SaveRule";
//        private const string SAVERULECOPY = "SaveRuleCopy";
//        private const string GETLIST        = "GetRuleList";
//        private const string GETRULESLIST =  "GetRulesList";
//        private const string LOADBYID         = "LoadRuleById";
////        private const string DELETE           = "DeleteRule";
//        private const string GETRULEUNITLIST    = "GetRuleUnitList";
//        private const string GETRULEUNITLISTWITHPARENT = "GetRuleUnitListWithParent";
//        private const string SAVERULEOBJECT     = "SaveRuleObject";
//        private const string GETRULEOBJECTLIST = "GetRuleObjectList";
//        private const string DELETERULEOBJECT = "DeleteRuleObject";
//        private const string DELETERULEOBJECTBYPARENT = "DeleteRuleObjectByParent";
//        private const string GETALLOWEDOBJECTLIST = "GetRuleAllowedObjectList";
//        private const string GETPRODUCTLIST = "GetRuleProductList";
//        private const string GETRULEFIELDACTION = "GetRuleFieldActionList";
//        private const string GETCHECKLIST = "GetCheckListById";
//        private const string SAVERULEDOCUMENT = "EditDocTemplateRelation";
//        private const string SAVERULECHECKLIST = "SaveRuleCheckList";
//        private const string SAVERULECONDITION = "SaveRuleCondition";
//        private const string SAVERULEALERT = "SaveRuleAlert";
//        private const string SAVERULEEVENT = "SaveRuleEvent";
//        private const string SAVERULEDATA = "SaveRuleData";
//        private const string SAVETASK = "SaveRuleTask";
//        private const string GETOBJECTRULE = "GetObjectRule";
//        private const string GETCATEGORY = "GetRuleCategory";
//        private const string SETSTATUS = "SetRuleStatus";
//        private const string GETRULEDEPENDANTFIELDLIST = "GetRuleDependantFieldList";
//        private const string GETCONDITIONFIELDS = "GetConditionField";
//        private const string GETALLDOCTEMPLATELIST = "GetAllDocTemplateList";
//        private const string GETALLCONDITIONLIST = "GetAllConditionsList";
//        private const string GETRELOADFIELDS = "GetReloadField";
//        private const string GETVISIBILITYFIELDLIST = "GetRuleVisibilityFieldList";
////        private const string GETRULETREE = "GetRuleTree";
        #endregion

        #region rule code related
        //private const string COLOREDSTRING = "<span style='color:{0}'>{1}</span>";
        //private const string COLORLOGICALNOT = "green";
        //private const string COLORFIELD = "red";
        //private const string COLORCOMPAREOP = "green";
        //private const string COLORLOGICALOP = "green";
        //private const string COLORVALUE = "blue";
        //private const string HTMLSPACE = "&nbsp;";
        //private const string NOTOP = "not";
        #endregion

        #region rules related objects' id
        public const int FIELDOBJECTTYPEID = 1;
        public const int CONDITIONOBJECTTYPEID = 2;
        public const int TASKOBJECTTYPEID = 3;
        public const int DOCUMENTOBJECTTYPEID = 4;
        public const int CHECKLISTOBJECTTYPEID = 5;
        public const int ALERTOBJECTTYPEID = 6;
        public const int EVENTOBJECTTYPEID = 7;
        public const int DATAOBJECTTYPEID = 8;
        public const int ALERTEVENTOBJECTTYPEID = 10;
        public const int CONDITIONCLOSINGINSTRUCTIONOBJECTTYPEID = 11;
        #endregion

        #endregion

        #region fields
        //private string name = String.Empty;
        //private DateTime? startdate = null;
        //private DateTime? enddate = null;
        //private int companyId = Constants.LOANSTARCOMPANYID;
        //private int categoryId = -1;
        //private static ArrayList fieldRules = null;
        //private static ArrayList documentRules = null;
        //private static ArrayList checkListRules = null;
        //private static ArrayList alertRules = null;
        //private static ArrayList eventRules = null;
        //private static ArrayList dataRules = null;
        //private static ArrayList conditionRules = null;
        //private static ArrayList taskRules = null;
        //private static ArrayList conditionFields = null;
        //private string categoryName = string.Empty;
        //private bool isCopyFromGeneralRule;
        //private int parentId = -1;
        //private static Hashtable reloadFields = null;
        #endregion

        #region constructors
        //public Rule()
        //{
        //}
        //public Rule(int id)
        //{
        //    ID = id;
        //    LoadById();
        //}
        #endregion

        #region properties
        //public string Name
        //{
        //    get { return name; }
        //    set { name = value; }
        //}
        //public DateTime? StartDate
        //{
        //    get { return startdate; }
        //    set { startdate=value; }
        //}
        //public DateTime? EndDate
        //{
        //    get { return enddate; }
        //    set { enddate = value; }
        //}
        //public int CompanyId
        //{
        //    get { return companyId; }
        //    set { companyId = value; }
        //}
        //public int CategoryId
        //{
        //    get { return categoryId; }
        //    set { categoryId = value; }
        //}
        //public string CategoryName
        //{
        //    get { return categoryName; }
        //    set { categoryName = value; }
        //}
        //public bool IsCopyFromGeneralRule
        //{
        //    get { return isCopyFromGeneralRule; }
        //    set { isCopyFromGeneralRule = value; }
        //}
        //public int ParentId
        //{
        //    get { return parentId; }
        //    set { parentId = value; }
        //}
        #endregion

        #region methods
        
        #region public methods

        #region instance methods
        //public bool Delete()
        //{
        //    if (CopyRuleToLender() > 0)
        //    {
        //        return db.ExecuteScalarInt(DELETE, ID) == 1;
        //    }
        //    return false;
            
        //}
        //public bool Enable()
        //{
        //    if (CopyRuleToLender() > 0)
        //    {
        //        return SetStatus(Constants.ENABLEDSTATUSID);
        //    }
        //    return false;            
        //}
        //public bool Disable()
        //{
        //    if (CopyRuleToLender() > 0)
        //    {
        //        return SetStatus(Constants.DISABLEDSTATUSID);
        //    }
        //    return false;
        //}
        //public DataView GetUnitList()
        //{
        //    return db.GetDataView(GETRULEUNITLIST, ID);
        //}
        //public DataView GetUnitListWithParent()
        //{
        //    return db.GetDataView(GETRULEUNITLISTWITHPARENT, ID);
        //}
        //public DataView GetRuleObjectList(int objecttypeid)
        //{
        //    return db.GetDataView(GETRULEOBJECTLIST, ID, objecttypeid);
        //}
        //public bool DeleteObject(int objectid)
        //{
        //    bool useParentId = isCopyFromGeneralRule;
        //    if (CopyRuleToLender() > 0)
        //    {
        //        if (useParentId)
        //        {
        //            return db.ExecuteScalarInt(DELETERULEOBJECTBYPARENT, objectid,ID) == 1;
        //        }
        //        else
        //        {
        //            return db.ExecuteScalarInt(DELETERULEOBJECT, objectid) == 1;
        //        }
                
        //    }
        //    return false;
        //}
        //public DataView GetAllowedObjectList(int objecttypeid, int objectgroupid)
        //{
        //    return db.GetDataView(GETALLOWEDOBJECTLIST, ID, objecttypeid, objectgroupid);
        //}
        //public DataView GetProductList()
        //{
        //    return db.GetDataView(GETPRODUCTLIST, ID);
        //}
//        public string GetColoredCodeById()
//        { 
////            return GetColoredCodeByDataView(GetUnitList());
//            return GetColoredCodeByDataView(GetUnitListWithParent());
//        }
        //public string GetColoredCodeByDataView(DataView dv)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    string currentRuleExp = String.Empty;
        //    int currentRuleId = -1;
        //    bool firstUnit = true;
        //    for (int i = 0; i < dv.Count; i++)
        //    {
        //        string unit = GetUnit(dv[i]);
        //        int ruleId = int.Parse(dv[i]["ruleid"].ToString());
        //        if (ruleId!=currentRuleId)
        //        {
        //            if(currentRuleId!=-1)
        //            {
        //                currentRuleExp = "[" + currentRuleExp + "]";
        //                if(sb.Length>0)
        //                {
        //                    currentRuleExp = HTMLSPACE + GetColoredString("AND", COLORLOGICALOP) + HTMLSPACE + currentRuleExp;
        //                }
        //                sb.Append(currentRuleExp);
        //                currentRuleExp = String.Empty;
        //                firstUnit = true;
        //            }
        //            currentRuleId = ruleId;
        //        }
        //        if (!firstUnit)
        //        {
        //            currentRuleExp += HTMLSPACE + GetColoredString(dv[i][LOGICALOPFIELDNAME].ToString(), COLORLOGICALOP) + HTMLSPACE;
        //        }
        //        currentRuleExp += unit;
        //        firstUnit = false;
        //    }
        //    if(!String.IsNullOrEmpty(currentRuleExp))
        //    {
        //        currentRuleExp = "[" + currentRuleExp + "]";
        //        if (sb.Length > 0)
        //        {
        //            currentRuleExp = HTMLSPACE + GetColoredString("AND", COLORLOGICALOP) + HTMLSPACE + currentRuleExp;
        //        }
        //        sb.Append(currentRuleExp);
        //    }
        //    return sb.ToString();
        //}
        //public RuleCondition GetCondition(int conditionid)
        //{
        //    return new RuleCondition(conditionid);
        //}

        #region Save rule and related objects
        //public int Save(string productList)
        //{
        //    ID = db.ExecuteScalarInt(SAVE, ID, isCopyFromGeneralRule, name, companyId, productList, categoryId, categoryName, startdate ?? (object)DBNull.Value, enddate ?? (object)DBNull.Value,parentId==-1?(object)DBNull.Value:parentId);
        //    return ID;
        //}
        //public int SaveRuleObject(int objectid, int objecttypeid, int ruleactionid)
        //{
        //    if (CopyRuleToLender() > 0)
        //    {
        //        return db.ExecuteScalarInt(SAVERULEOBJECT, ID, objectid, objecttypeid, ruleactionid);
        //    }
        //    return -1;
        //}
        //public int SaveRuleCheckList(int checklistid, string xml)
        //{
        //    if (CopyRuleToLender() > 0)
        //    {
        //        return db.ExecuteScalarInt(SAVERULECHECKLIST, ID, checklistid, xml);
        //    }
        //    return -1;
        //}
        //public int SaveDocument(int dtRelationID, int dtID, int groupID, bool isAppPackage, bool isClosingPackage, bool isMiscPackage)
        //{
        //    if (CopyRuleToLender() > 0)
        //    {
        //        return db.ExecuteScalarInt(SAVERULEDOCUMENT, ID, dtRelationID, dtID, groupID, isAppPackage, isClosingPackage, isMiscPackage);
        //    }
        //    return -1;
        //}
        //public int SaveCondition(RuleCondition rc)
        //{
        //    if (CopyRuleToLender() > 0)
        //    {
        //        return db.ExecuteScalarInt(SAVERULECONDITION, ID, rc.Id, rc.Title, rc.Detail, rc.RoleId);
        //    }
        //    return -1;
        //}
        //public int SaveAlert(int alertid,string message,bool isnew)
        //{
        //    if (CopyRuleToLender() > 0)
        //    {
        //        return db.ExecuteScalarInt(SAVERULEALERT, ID, alertid, message, isnew);
        //    }
        //    return -1;
        //}
        //public int SaveEvent(int eventid, string message, int eventtypeid, bool isnew)
        //{
        //    if (CopyRuleToLender() > 0)
        //    {
        //        return db.ExecuteScalarInt(SAVERULEEVENT, ID, eventid, message, eventtypeid, isnew);
        //    }
        //    return -1;
        //}
        //public int SaveData(int objectid, int fieldid, string fieldvalue)
        //{
        //    if (CopyRuleToLender() > 0)
        //    {
        //        return db.ExecuteScalarInt(SAVERULEDATA, ID, objectid, fieldid, fieldvalue);
        //    }
        //    return -1;
        //}
        //public int SaveTask(RuleTask rt)
        //{
        //    if (CopyRuleToLender() > 0)
        //    {
        //        return db.ExecuteScalarInt(SAVETASK, ID, rt.Id, rt.Title, rt.Description, rt.TypeId, rt.InfoSourceId, rt.DifficultyId);
        //    }
        //    return -1;
        //}
        //private int CopyRuleToLender()
        //{
        //    if (!isCopyFromGeneralRule)
        //    {
        //        return ID;
        //    }
        //    int res = db.ExecuteScalarInt(SAVERULECOPY, ID, companyId);
        //    if (res > 0)
        //    {
        //        ID = res;
        //        isCopyFromGeneralRule = false;
        //    }
        //    return res;
        //}        
        #endregion

        #endregion

        #region static methods
        //public static string GetRulesIdXml(ArrayList ar)
        //{
        //    XmlDocument d = new XmlDocument();
        //    XmlNode root = d.CreateElement(ROOTELEMENT);
        //    for (int i = 0; i < ar.Count; i++)
        //    {
        //        RuleEvaluation re = (RuleEvaluation)ar[i];
        //        if (re.EvaluationValue)
        //        {
        //            XmlNode n = d.CreateElement(ITEMELEMENT);
        //            XmlAttribute a = d.CreateAttribute(IDATTRIBUTE);
        //            a.Value = re.Id.ToString();
        //            n.Attributes.Append(a);
        //            root.AppendChild(n);
        //        }
        //    }
        //    if (root.ChildNodes.Count > 0)
        //    {
        //        d.AppendChild(root);
        //        return d.OuterXml;
        //    }
        //    return "";
        //}
        //public static DataView GetCheckList(int checklistid)
        //{ 
        //    return db.GetDataView(GETCHECKLIST,checklistid);
        //}
        #region obsolete
        //public static DataView GetList(int companyid,int productid,bool showgeneral, string where, string orderby)
        //{
        //    return db.GetDataView(GETLIST, companyid, productid, showgeneral, where, orderby);
        //}
        #endregion
        //public static DataView GetRuleList(int companyid,int productid,bool showgeneral)
        //{
        //    return db.GetDataView(GETRULESLIST, companyid, productid, showgeneral);
        //}
        //public static DataView GetFieldAction()
        //{
        //    return db.GetDataView(GETRULEFIELDACTION);
        //}
        //public static ArrayList GetConditionFields(int lenderid)
        //{
        //    if (conditionFields == null)
        //    {
        //        conditionFields = new ArrayList();
        //        DataView dv = db.GetDataView(GETCONDITIONFIELDS,lenderid);
        //        for(int i=0; i<dv.Count;i++)
        //        {
        //            conditionFields.Add(dv[i][PROPERTYNAMEFIELDNAME].ToString());
        //        }
        //    }
        //    return conditionFields;
        //}
        public static Hashtable GetReloadFields(int lenderId)
        {
            return null;
        }

        //public static Hashtable GetReloadFields(int lenderId)
        //{ 
        //    if (reloadFields==null)
        //    {
        //        reloadFields = new Hashtable();
        //        DataView dv = db.GetDataView(GETRELOADFIELDS, lenderId);
        //        ArrayList ar = new ArrayList();
        //        string currentFieldName = String.Empty;
        //        for (int i = 0; i < dv.Count; i++)
        //        {
        //            string fieldName = dv[i]["propertyname"].ToString();
        //            if (fieldName != currentFieldName)
        //            {
        //                if (currentFieldName != String.Empty)
        //                {
        //                    reloadFields.Add(currentFieldName, ar);
        //                }
        //                ar = new ArrayList();
        //                currentFieldName = fieldName;
        //            }
        //            ar.Add(Convert.ToInt32(dv[i]["objecttypeid"].ToString()));
        //        }
        //        if (currentFieldName != String.Empty)
        //        {
        //            reloadFields.Add(currentFieldName, ar);
        //        }
        //    }
        //    return reloadFields;
        //}
        public static ArrayList GetSpecificRules(int ruleobjecttypeid, int lenderId)
        {
            return null;
        }

        //public static ArrayList GetSpecificRules(int ruleobjecttypeid, int lenderId)
        //{
        //    /*
        //    switch (ruleobjecttypeid)
        //    { 
        //        case FIELDOBJECTTYPEID:
        //            if (fieldRules != null)
        //            {
        //                return fieldRules;
        //            }
        //            break;
        //        case DOCUMENTOBJECTTYPEID:
        //            if (documentRules != null)
        //            {
        //                return documentRules;
        //            }
        //            break;
        //        case CHECKLISTOBJECTTYPEID:
        //            if (checkListRules != null)
        //            {
        //                return checkListRules;
        //            }
        //            break;
        //        case ALERTOBJECTTYPEID:
        //            if (alertRules != null)
        //            {
        //                return alertRules;
        //            }
        //            break;
        //        case EVENTOBJECTTYPEID:
        //            if (eventRules != null)
        //            {
        //                return eventRules;
        //            }
        //            break;
        //        case DATAOBJECTTYPEID:
        //            if (dataRules != null)
        //            {
        //                return dataRules;
        //            }
        //            break;
        //        case CONDITIONOBJECTTYPEID:
        //            if (conditionRules != null)
        //            {
        //                return conditionRules;
        //            }
        //            break;
        //        case TASKOBJECTTYPEID:
        //            if (taskRules != null)
        //            {
        //                return taskRules;
        //            }
        //            break;
        //        default:
        //            return null;                        
        //    }
        //    */
        //    return LoadSpecificRules(ruleobjecttypeid, lenderId);
        //}
        //public static ArrayList GetSpecificRulesTree(int ruleobjecttypeid, int lenderId)
        //{
        //    return LoadSpecificRulesTree(ruleobjecttypeid, lenderId);
        //}
        //private static ArrayList LoadSpecificRules(int ruletypeId, int lenderId)
        //{
        //    return RuleEvaluation.GetRuleEvaluation(db.GetDataView(GETOBJECTRULE,lenderId,ruletypeId));
        //}
        //private static ArrayList LoadSpecificRulesTree(int ruletypeId, int lenderId)
        //{
        //    return RuleEvaluation.GetRuleEvaluationTree(db.GetDataView(GETOBJECTRULE, lenderId, ruletypeId));
        //}

        //public static DataView GetCategory()
        //{
        //    return db.GetDataView(GETCATEGORY);
        //}
        //public static DataView GetRuleDependantFields()
        //{
        //    return db.GetDataView(GETRULEDEPENDANTFIELDLIST);
        //}
        //public static DataView GetVisibilityFields()
        //{
        //    return db.GetDataView(GETVISIBILITYFIELDLIST);
        //}

        //public static DataView GetAllDocumentTemplates()
        //{
        //    return db.GetDataView(GETALLDOCTEMPLATELIST);
        //}
        //public static DataView GetAllConditions(int companyId)
        //{
        //    return db.GetDataView(GETALLCONDITIONLIST,companyId);
        //}

        #endregion

        #endregion

        #region private methods
        //private bool SetStatus(int statusid)
        //{
        //    return (db.ExecuteScalarInt(SETSTATUS, ID, statusid)==1);
        //}
        //private void LoadById()
        //{
        //    DataView dv = db.GetDataView(LOADBYID, ID);
        //    if (dv.Count == 1)
        //    {
        //        ID = int.Parse(dv[0][IDFIELDNAME].ToString());
        //        name = dv[0][NAMEFIELDNAME].ToString();
        //        if (!String.IsNullOrEmpty(dv[0][STARTDATEFIELDNAME].ToString()))
        //        {
        //            startdate = DateTime.Parse(dv[0][STARTDATEFIELDNAME].ToString());
        //        }
        //        if (!String.IsNullOrEmpty(dv[0][ENDDATEFIELDNAME].ToString()))
        //        {
        //            enddate = DateTime.Parse(dv[0][ENDDATEFIELDNAME].ToString());
        //        }
        //        companyId = int.Parse(dv[0][COMPANYIDFIELDNAME].ToString());
        //        categoryId = int.Parse(dv[0][CATEGORYIDFIELDNAME].ToString());
        //        if(dv[0][PARENTIDFIELDNAME]!=DBNull.Value)
        //        {
        //            parentId = int.Parse(dv[0][PARENTIDFIELDNAME].ToString());
        //        }
        //    }
        //}
        //private static string GetUnit(DataRowView row)
        //{
        //    return "(" + GetColoredString(row[Field.LOGICALNOTFIELDNAME].ToString(), COLORLOGICALNOT)
        //        + GetColoredString(row[Field.DESCRIPTIONFIELDNAME].ToString(), COLORFIELD)
        //        + HTMLSPACE + GetColoredString(row[COMPAREOPFIELDNAME].ToString(), COLORCOMPAREOP) + HTMLSPACE
        //        + (bool.Parse(row[LITERALVALUEFIELDNAME].ToString()) ? GetColoredString(row[Field.DATAVALUEFIELDNAME].ToString(), COLORVALUE) : GetColoredString(row[FIELDNAMEFIELDNAME].ToString(), COLORFIELD))
        //        + ")";
        //}
        //private static string GetColoredString(string text, string color)
        //{
        //    return String.Format(COLOREDSTRING, color, text.ToLower()==NOTOP?text+HTMLSPACE:text);
        //}
        #endregion

        #endregion

    }

    public class RuleEvaluation
    {
        #region constants
        private const string RULEIDFIELDNAME = "ruleid";
        private const string PARENTIDFIELDNAME = "parentid";
        #endregion

        #region fields
        private int id = -1;
        private int parentId = -1;
        private ArrayList units;
        private bool evaluationValue=false;
        private ArrayList childRules;
        private bool isCalculated = false;
        private Hashtable skipObjects;
        #endregion

        #region property
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public ArrayList Units
        {
            get { return units; }
            set { units = value; }
        }
        public bool EvaluationValue
        {
            get { return evaluationValue; }
            set { evaluationValue = value; }
        }
        public int ParentId
        {
            get { return parentId; }
        }
        public ArrayList ChildRules
        {
            get { return childRules; }
            set { childRules = value; }
        }
        public Object RuleObject
        {
            get
            {
                if(units!=null)
                {
                    for(int i=0;i<units.Count;i++)
                    {
                        if(((RuleEvaluationUnit)units[i]).IListObject !=null)
                        {
                            return ((RuleEvaluationUnit)units[i]).IListObject;
                        }
                    }
                }
                return null;
                
            }
        }
        public bool IsCalclulated
        {
            get { return isCalculated; }
            set { isCalculated = value; }
        }
        public Hashtable SkipObjects
        {
            get { return skipObjects; }
            set { skipObjects = value; }
        }
        #endregion

        #region methods
        public bool Evaluate(Object o)
        {
            bool res = false;
            if(isCalculated)
            {
                if((units!=null)&&(units.Count>0))
                {
                    if (units.Count < 2)
                    {
                        res = ((RuleEvaluationUnit)units[0]).GetResult(o);
                    }
                    else
                    {
                        for (int i = 0; i < units.Count - 1; i++)
                        {
                            bool b1 = ((RuleEvaluationUnit)units[i]).GetResult(o);
                            bool b2 = ((RuleEvaluationUnit)units[i]).GetResult(o);
                            switch (((RuleEvaluationUnit)units[i + 1]).LogicalOpId)
                            {
                                case (int)RuleEvaluationUnit.LogicalOperation.AND:
                                    res = b1 && b2;
                                    break;
                                case (int)RuleEvaluationUnit.LogicalOperation.OR:
                                    res = b1 || b2;
                                    break;
                            }
                        }
                    }
                }
            }
            return res;
        }
        //private bool GetEvaluationValue(Object o)
        //{
        //    bool res = false;
        //    return res;
        //}

        public bool IsObjectDependant(string objectName)
        {
            for (int i = 0; i < units.Count; i++)
            {
                RuleEvaluationUnit reu = (RuleEvaluationUnit)units[i];
                if (reu.PropertyName.StartsWith(objectName+"."))
                {
                    return true;
                }
            }
            return false;
        }
        //public bool GetEvaluationValue(Object o)
        //{
        //    if (o == null)
        //    {
        //        return evaluationValue;
        //    }
        //    else
        //    {
        //        bool res = false;
        //        Object val = evalValues[o];
        //        if (val != null)
        //        {
        //            res = (bool)val;
        //        }
        //        return res;
        //    }
        //}

        public static ArrayList GetRuleEvaluation(DataView dv)
        {
            ArrayList result = null;
            if (dv.Count > 0)
            {
                result = new ArrayList();
                RuleEvaluation re = new RuleEvaluation();
                ArrayList rul = null;
                for (int i = 0; i < dv.Count; i++)
                {
                    int ruleId = int.Parse(dv[i][RULEIDFIELDNAME].ToString());
                    if (ruleId != re.Id)
                    {
                        if (rul != null)
                        {
                            re.Units = rul;
                            result.Add(re);
                        }
                        re = new RuleEvaluation();
                        rul = new ArrayList();
                        re.Id = ruleId;
                        if(dv[i][PARENTIDFIELDNAME]!=DBNull.Value)
                        {
                            re.parentId = int.Parse(dv[i][PARENTIDFIELDNAME].ToString());
                        }
                    }
                    if (rul != null) rul.Add(new RuleEvaluationUnit(dv[i]));
                }
                if (rul != null)
                {
                    re.Units = rul;
                    result.Add(re);
                }
            }
            return result;
        }
        public static ArrayList GetRuleEvaluationTree(DataView dv)
        {
            ArrayList res = null;
            if(dv.Count>0)
            {
                res = GetChilds(dv, -1);
            }
            return res;
        }
        private static ArrayList GetChilds(DataView dv, int parentid)
        {
            ArrayList res = null;
            string currentFilter = parentid<0?"parentid is null":"parentId="+parentid;
            dv.RowFilter = currentFilter;
            if(dv.Count>0)
            {
                res = new ArrayList();
                RuleEvaluation re = new RuleEvaluation();
                ArrayList rul = new ArrayList();
                for(int i=0;i<dv.Count;i++)
                {
                    int ruleId = int.Parse(dv[i][RULEIDFIELDNAME].ToString());
                    if(ruleId!=re.Id)
                    {
                        if(re.id!=-1)
                        {
                            re.Units = rul;
                            re.ChildRules = GetChilds(dv, re.id);
                            dv.RowFilter = currentFilter;
                            res.Add(re);
                        }
                        re = new RuleEvaluation();
                        rul = new ArrayList();
                        re.Id = ruleId;
                    }
                    rul.Add(new RuleEvaluationUnit(dv[i]));
                }
                if (re.id != -1)
                {
                    re.Units = rul;
                    re.ChildRules = GetChilds(dv, re.id);
                    res.Add(re);
                }
            }
            return res;
        }
        #endregion
    }

  
    public class RuleCondition
    {
        #region constants
        
        #region sp names
        private const string GETCONDITIONBYID = "GetRuleConditionById";
        #endregion

        #region db fields' names
        private const string TITLEFIELDNAME = "title";
        private const string DETAILFIELDNAME = "detail";
        private const string ROLEIDFIELDNAME = "roleid";
        private const string TYPEIDFIELDNAME = "typeid";
        private const string CATEGORYIDFIELDNAME = "categoryid";
        #endregion

        #endregion

        #region fields
        private int id = -1;
        private string title = String.Empty;
        private string detail = String.Empty;
        private int roleid = -1;
        private static readonly DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);
        private int typeId = 0;
        private int categoryId = 0;
        #endregion

        #region property
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Detail
        {
            get { return detail; }
            set { detail = value; }
        }
        public int RoleId
        {
            get { return roleid; }
            set { roleid = value; }
        }
        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        #endregion

        #region constructor
        public RuleCondition()
        { 
        }
        public RuleCondition(int id)
        {
            this.id = id;
            if (id > 0)
            {
                LoadById();
            }
        }
        #endregion

        #region methods
        private void LoadById()
        {
            DataView dv = db.GetDataView(GETCONDITIONBYID, id);
            if (dv.Count == 1)
            {
                title = dv[0][TITLEFIELDNAME].ToString();
                detail = dv[0][DETAILFIELDNAME].ToString();
                roleid = int.Parse(dv[0][ROLEIDFIELDNAME].ToString());
                typeId = int.Parse(dv[0][TYPEIDFIELDNAME].ToString());
                categoryId = int.Parse(dv[0][CATEGORYIDFIELDNAME].ToString());
            }
            else
            {
                id = -1;
            }
        }
        #endregion
    }

    public class RuleClosingInstruction
    {
        #region constants

        #region sp names
        private const string GETCLOSINGINSTRUCTIONBYID = "GetRuleClosingInstructionById";
        #endregion

        #region db fields' names
        private const string TITLEFIELDNAME = "title";
        private const string DETAILFIELDNAME = "detail";
        private const string ROLEIDFIELDNAME = "roleid";
        #endregion

        #endregion

        #region fields
        private int id = -1;
        private string title = String.Empty;
        private string detail = String.Empty;
        private int roleid = -1;
        private static readonly DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);
        #endregion

        #region property
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Detail
        {
            get { return detail; }
            set { detail = value; }
        }
        public int RoleId
        {
            get { return roleid; }
            set { roleid = value; }
        }
        #endregion

        #region constructor
        public RuleClosingInstruction()
        {
        }
        public RuleClosingInstruction(int id)
        {
            this.id = id;
            if (id > 0)
            {
                LoadById();
            }
        }
        #endregion

        #region methods
        private void LoadById()
        {
            DataView dv = db.GetDataView(GETCLOSINGINSTRUCTIONBYID, id);
            if (dv.Count == 1)
            {
                title = dv[0][TITLEFIELDNAME].ToString();
                detail = dv[0][DETAILFIELDNAME].ToString();
                roleid = int.Parse(dv[0][ROLEIDFIELDNAME].ToString());
            }
            else
            {
                id = -1;
            }
        }
        #endregion
    }

    public class RuleTask
    {
        #region constants

        #region sp names
        private const string GETTASKBYID = "GetRuleTaskById";
        #endregion

        #region db fields' names
        private const string TITLEFIELDNAME = "title";
        private const string DESCRIPTIONFIELDNAME = "description";
        private const string TYPEIDFIELDNAME = "tasktypeid";
        private const string INFOSOURCEIDFIELDNAME = "taskinfosourceid";
        private const string DIFFICULTYIDFIELDNAME = "taskdifficultyid";
        #endregion

        #endregion

        #region fields
        private int id = -1;
        private string title = String.Empty;
        private string description = String.Empty;
        private int difficultyid = -1;
        private int typeid = -1;
        private int infosourceid = -1;
        private static readonly DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);
        #endregion

        #region properties
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public int TypeId
        {
            get { return typeid; }
            set { typeid = value; }
        }
        public int InfoSourceId
        {
            get { return infosourceid; }
            set { infosourceid = value; }
        }
        public int DifficultyId
        {
            get { return difficultyid; }
            set { difficultyid = value; }
        }
        #endregion

        #region constructor
        public RuleTask()
        {
        }
        public RuleTask(int id)
        {
            this.id = id;
            if (id > 0)
            {
                LoadById();
            }
        }
        #endregion

        #region methods
        private void LoadById()
        {
            DataView dv = db.GetDataView(GETTASKBYID, id);
            if (dv.Count == 1)
            {
                title = dv[0][TITLEFIELDNAME].ToString();
                description = dv[0][DESCRIPTIONFIELDNAME].ToString();
                difficultyid = int.Parse(dv[0][DIFFICULTYIDFIELDNAME].ToString());
                typeid = int.Parse(dv[0][TYPEIDFIELDNAME].ToString());
                infosourceid = int.Parse(dv[0][INFOSOURCEIDFIELDNAME].ToString());
            }
            else
            {
                id = -1;
            }
        }
        #endregion
    }

  
    public class RuleField
    {
        private string propertyName = String.Empty;
        private ArrayList rules = new ArrayList();

        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }
        public ArrayList Rules
        {
            get { return rules; }
            set { rules = value; }
        }
    }

    public class RuleAlert
    {
        #region constants
        private const string LOADBYOBJECTID = "LoadAlertByObjectId";
        private const string MESSAGEFIELDNAME = "message";
        #endregion

        #region fields
        private static readonly DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);
        private int id;
        private string message = String.Empty;
        #endregion

        #region rpoperties
        public int Id
        {
            get { return id; }
        }
        public string Message
        {
            get { return message; }
        }
        #endregion

        #region constructor
        public RuleAlert(int _id)
        {
            id = _id;
            LoadById();
        }
        #endregion

        #region methods
        private void LoadById()
        {
            if(id>0)
            {
                DataView dv = db.GetDataView(LOADBYOBJECTID, id);
                if(dv.Count==1)
                {
                    message = dv[0][MESSAGEFIELDNAME].ToString();
                }
                else
                {
                    id = -1;
                }
            }
        }

        #endregion
    }

    public class RuleEvent
    {
        #region constants
        private const string LOADBYOBJECTID = "LoadEventByObjectId";
        private const string MESSAGEFIELDNAME = "message";
        private const string EVENTTYPEIDFIELDNAME = "EventTypeId";
        #endregion

        #region fields
        private static readonly DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);
        private int id;
        private string message = String.Empty;
        private int eventTypeId = 0;
        #endregion

        #region rpoperties
        public int Id
        {
            get { return id; }
        }
        public string Message
        {
            get { return message; }
        }
        public int EventTypeId
        {
            get{ return eventTypeId;}
        }
        #endregion

        #region constructor
        public RuleEvent(int _id)
        {
            id = _id;
            LoadById();
        }
        #endregion

        #region methods
        private void LoadById()
        {
            if (id > 0)
            {
                DataView dv = db.GetDataView(LOADBYOBJECTID, id);
                if (dv.Count == 1)
                {
                    message = dv[0][MESSAGEFIELDNAME].ToString();
                    eventTypeId = int.Parse(dv[0][EVENTTYPEIDFIELDNAME].ToString());
                }
                else
                {
                    id = -1;
                }
            }
        }

        #endregion
    }

    public class RuleDocument
    {
        #region constants
        private const string LOADBYOBJECTID = "LoadDocumentByObjectId";
        private const string DOCTEMPLATEIDFIELDNAME = "doctemplateid";
        private const string ISAPPPACKAGEFIELDNAME = "isapppackage";
        private const string ISCLOSINGPACKAGEFIELDNAME = "isclosingpackage";
        private const string ISMISCPACKAGEFIELDNAME = "ismiscpackage";
        #endregion

        #region fields
        private static readonly DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);
        private int id = -1;
        private int documentTemplateId = -1;
        private bool isAppPackage;
        private bool isClosingPackage;
        private bool isMiscPackage;
        #endregion

        #region properties
        public int DocumentTemplateId
        {
            get { return documentTemplateId;}
        }
        public bool IsAppPackage
        {
            get{ return isAppPackage;}
        }
        public bool IsClosingPackage
        {
            get{ return isClosingPackage;}
        }
        public bool IsMiscPackage
        {
            get{ return isMiscPackage;}
        }
        #endregion

        #region constructor
        public RuleDocument(int _id)
        {
            id = _id;
            LoadById();
        }
        #endregion

        #region methods
        private void LoadById()
        {
            if (id > 0)
            {
                DataView dv = db.GetDataView(LOADBYOBJECTID, id);
                if (dv.Count == 1)
                {
                    documentTemplateId = int.Parse(dv[0][DOCTEMPLATEIDFIELDNAME].ToString());
                    isAppPackage = bool.Parse(dv[0][ISAPPPACKAGEFIELDNAME].ToString());
                    isClosingPackage = bool.Parse(dv[0][ISCLOSINGPACKAGEFIELDNAME].ToString());
                    isMiscPackage = bool.Parse(dv[0][ISMISCPACKAGEFIELDNAME].ToString());
                }
                else
                {
                    id = -1;
                }
            }
        }
        #endregion
    }

}
