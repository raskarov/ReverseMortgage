using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using LoanStarPortal.Controls;

namespace LoanStar.Common
{

    public class MortgageDataControl : AppControl
    {
        #region constants
        protected const string CLASSATTRIBUTE = "class";
        protected const string HTMLSPACE = "&nbsp;";
        protected const string FOOTERDIV = "<div style='height:40px;'>&nbsp;</div>";
        protected const string ONCLICK = "onclick";
        protected const string STYLEATTRIBUTE = "style";
        protected const string DISPLAYNONE = "display:none";
        protected const string ERRMESSAGECLASS = "errmessage";
        private const string DIVIDERDIV = "<div style='height:{0}px;'>&nbsp;</div>";
        protected const int DIVIDERHEIGHT = 20;
        protected const int TOPDIVIDERHEIGHT = 10;
        protected const int RADCOMBOWIDTH = 154;
        protected const int RADCOMBOWIDTHFIX = 137;
        #endregion


        protected Control container;
        private bool needReload;
        private bool conditionFieldFired;
        private bool documentFieldFired;
        protected string objectName = String.Empty;
        public short tabIndex = 1;

        protected ArrayList objectFields;
        protected ArrayList allShownFields;
        protected Hashtable postBackObjectFields;

        protected Hashtable PostBackObjectFields
        {
            get
            {
                if (postBackObjectFields == null)
                {
                    postBackObjectFields = GetObjectPostBackFields(objectName);
                }
                return postBackObjectFields;
            }
        }


        public bool ConditionFieldFired
        {
            get
            {
                return conditionFieldFired;
            }            
        }
        public bool DocumentFieldFired
        {
            get
            {
                return documentFieldFired;
            }
        }

        public bool NeedReload
        {
            get { return needReload; }
            set { needReload = value; }
        }

        #region rules evaluation arrays
//        private ArrayList fieldsVisibilityRules;
        private ArrayList fieldsDataRules;
        private ArrayList reloadFields;
//        private DataView ruleDependantFields;
        #endregion
        protected ArrayList FieldsVisibilityRules
        {
            get
            {
                return Mp.FieldsVisibilityRules;
            }
        }
        protected ArrayList ReloadFields
        {
            get
            {
                if (reloadFields == null)
                {
                    reloadFields = new ArrayList();
                    Hashtable rf = Rule.GetReloadFields(CurrentUser.CompanyId);
                    foreach (DictionaryEntry item in rf)
                    {
                        reloadFields.Add(item.Key);
                    }
                }
                return reloadFields;
            }
        }
        protected ArrayList FieldsDataRules
        {
            get
            {
                if (fieldsDataRules == null)
                {
                    fieldsDataRules = Rule.GetSpecificRules(Rule.DATAOBJECTTYPEID, CurrentUser.CompanyId);
                }
                return fieldsDataRules;
            }
        }
        protected DataView RuleDependantFields
        {
            get { return Mp.RuleDependantFields; }
        }
        protected int radComboWidth = RADCOMBOWIDTH;
        private MortgageProfile mp;
        public MortgageProfile Mp
        {
            get
            {
                if (mp == null)
                {
                    mp = CurrentPage.GetMortgage(MortgageProfileId);
                }
                return mp;
            }
            set 
            {
                mp = value;
            }
        }
        protected int MortgageProfileId
        {
            get
            {
                if (Session[Constants.MortgageID] == null)
                    Session[Constants.MortgageID] = 0;
                return Convert.ToInt32(Session[Constants.MortgageID].ToString());
            }
            set
            {
                Session[Constants.MortgageID] = value;
            }
        }

        #region methods

        #region public & protected
        public string GetPostBackField(string eventTarget, out string propertyName, out int objectId)
        {
            string res = String.Empty;
            propertyName = String.Empty;
            objectId = -1;
            string[] list = eventTarget.Split(':');
            if ((list==null)||(list.Length==0))
            {
                return res;
            }
            int k = list.Length-1;
            while (k >=0) 
            {
                int n = list[k].LastIndexOf("_");
                if (n > 0)
                {
                    propertyName = list[k].Substring(0, n);
                    try
                    {
                        objectId = Convert.ToInt32(list[k].Substring(n + 1));
                        break;
                    }
                    catch { }
                }
                k--;
            } 
            if (objectId < 0)
            {
                return res;
            }
            res = String.Join(":", list, 0, k + 1);
            if (!String.IsNullOrEmpty(res))
            {
                res = res.Replace(":", "$");
            }
            return res;
        }
        protected static void SetUIGroupFields(ArrayList objectFields, string[] fieldsInGroup, int groupId)
        {
            if (fieldsInGroup == null)
            {
                return;
            }
            for (int i = 0; i < objectFields.Count; i++)
            {
                string name = ((MortgageProfileField)objectFields[i]).PropertyName.ToLower();
                for (int j = 0; j < fieldsInGroup.Length; j++)
                {
                    if (String.Compare(fieldsInGroup[j],name,true)==0)
                    {
                        ((MortgageProfileField)objectFields[i]).FieldUIGroupId = groupId;
                        break;
                    }
                }
            }
            return;
        }
        //protected void CalculateRules(ArrayList rules)
        //{
        //    Mp.EvaluateRules(rules);
        //}
        protected static string[] GetAllFields(params object[] fields)
        {
            int cnt = 0;
            for (int i = 0; i < fields.Length; i++)
            {
                cnt += ((string[])fields[i]).Length;
            }
            if (cnt > 0)
            {
                string[] res = new string[cnt];
                int offset = 0;
                for (int i = 0; i < fields.Length; i++)
                {
                    string[] f = (string[])fields[i];
                    for (int j = 0; j < f.Length; j++)
                    {
                        res[j + offset] = f[j];
                    }
                    offset += f.Length;
                }
                return res;
            }
            return null;
        }
        protected static void PreparePostBackFields(string objName, ArrayList fields, Hashtable postBackFields)
        {
            foreach (DictionaryEntry item in postBackFields)
            {
                Object key = item.Key;
                Hashtable val = (Hashtable)item.Value;
                int index = GetFieldFromList(key.ToString().Replace(objName + ".", ""), fields);
                if (index != -1)
                {
                    if (CheckDependantFields(objName, val, fields))
                    {
                        MortgageProfileField mpf = (MortgageProfileField)fields[index];
                        if (mpf != null)
                        {
                            mpf.IsPostBack = true;
                        }
                    }
                }
            }
        }
        protected static void PrepareConditionPostBackField(string objName, ArrayList fields, ArrayList conditionFields)
        {
            for (int i = 0; i < conditionFields.Count; i++)
            {
                string name = conditionFields[i].ToString().Replace(objName + ".", "");
                for (int j = 0; j < fields.Count; j++)
                {
                    MortgageProfileField mpf = (MortgageProfileField)fields[j];
                    if (mpf.PropertyName == name)
                    {
                        mpf.IsPostBack = true;
                        break;
                    }
                }
            }
        }
        protected Hashtable GetObjectPostBackFields(string _objectName)
        {
            Hashtable result = new Hashtable();
            ArrayList list = new ArrayList();
            if (FieldsVisibilityRules != null)
            {
                list.AddRange(FieldsVisibilityRules);
            }
            if (FieldsDataRules != null)
            {
                list.AddRange(FieldsDataRules);
            }
            for (int i=0;i<list.Count;i++)
            {
                RuleEvaluation re = (RuleEvaluation)list[i];
                ArrayList _objectFields = GetObjectFields(re, _objectName);
                if (_objectFields.Count > 0)
                {
                    for (int k = 0; k < _objectFields.Count; k++)
                    {
                        string key = _objectFields[k].ToString();
                        Object o = result[key];
                        if (o == null)
                        {
                            o = new Hashtable();
                            GetDependantFields((Hashtable)o, re.Id);
                            result.Add(key, o);
                        }
                        else
                        {
                            GetDependantFields((Hashtable)o, re.Id);
                            result[key] = o;
                        }
                    }
                }
                CheckChilds(re,result,_objectName);
            }
            return result;
        }
        private void CheckChilds(RuleEvaluation rep, Hashtable result, string _objectName)
        {
            if(rep.ChildRules!=null)
            {
                for(int i=0; i<rep.ChildRules.Count;i++)
                {
                    RuleEvaluation re = (RuleEvaluation)rep.ChildRules[i];
                    ArrayList _objectFields = GetObjectFields(re, _objectName);
                    if (_objectFields.Count > 0)
                    {
                        for (int k = 0; k < _objectFields.Count; k++)
                        {
                            string key = _objectFields[k].ToString();
                            Object o = result[key];
                            if (o == null)
                            {
                                o = new Hashtable();
                                GetDependantFields((Hashtable)o, re.Id);
                                result.Add(key, o);
                            }
                            else
                            {
                                GetDependantFields((Hashtable)o, re.Id);
                                result[key] = o;
                            }
                        }
                    }
                    CheckChilds(re, result, _objectName);
                }
            }
        }
        protected Fields GetTabWrapper(string title, Object obj, ArrayList allfields, int uigroupId, string trStyle, string[] tdStyle, int columns)
        {
            Fields fctl = (Fields)LoadControl(Constants.FECONTROLSLOCATION + "Fields.ascx");
            fctl.StartTabOrder = tabIndex;
            fctl.RadComboWidth = radComboWidth;
            ArrayList fields = GetGroupVisibleFields(allfields, uigroupId);
            Mp.PopulateFields(obj, fields);
            HtmlTable table = new HtmlTable();
            table.Attributes.Add("border", "0");
            table.Attributes.Add("cellspacing", "0");
            table.Attributes.Add("cellpadding", "0");
            table.Attributes.Add("align", "center");
            table.Attributes.Add("style", "width:97%");
            fctl.Title = title;
            fctl.BuildControls(table, trStyle, tdStyle, fields, columns);
            tabIndex = fctl.TabIndex;
            tabIndex++;
            return fctl;
        }
        protected static Control GetTabDivider(int height)
        {
            return new LiteralControl(String.Format(DIVIDERDIV,height));
        }
        protected static ArrayList GetAllFieldsToDisplay(ArrayList allfields)
        {
            ArrayList res = new ArrayList();
            for (int i = 0; i < allfields.Count; i++)
            {
                if (((MortgageProfileField)allfields[i]).FieldUIGroupId > 0)
                {
                    res.Add(allfields[i]);
                }
            }
            return res;
        }
        public void CheckGridPostBackFields()
        {
            Object o = Session[Constants.GRIDPROPERTYLIST];
            if(o!=null)
            {
                string list = o.ToString();
                if(!String.IsNullOrEmpty(list))
                {
                    string[] properties = list.Split(',');
                    Hashtable reloadFields_ = Rule.GetReloadFields(CurrentUser.CompanyId);
                    if ((reloadFields_ != null) && (reloadFields_.Count > 0))
                    {
                        for (int j = 0; j < properties.Length; j++)
                        {
                            if (reloadFields_.ContainsKey(properties[j]))
                            {
                                ArrayList ar = reloadFields_[properties[j]] as ArrayList;
                                if ((ar != null) && (ar.Count > 0))
                                {
                                    for (int i = 0; i < ar.Count; i++)
                                    {
                                        int typeid = (int)ar[i];
                                        if (typeid == Rule.CONDITIONOBJECTTYPEID)
                                        {
                                            conditionFieldFired = true;
                                        }
                                        if (typeid == Rule.DOCUMENTOBJECTTYPEID)
                                        {
                                            documentFieldFired = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void CheckIfReloadNeeded(string propertyName)
        {
            if(!String.IsNullOrEmpty(propertyName))
            {
                Hashtable reloadFields_ = Rule.GetReloadFields(CurrentUser.CompanyId);
                if ((reloadFields_ != null) && (reloadFields_.Count > 0))
                {
                    if (reloadFields_.ContainsKey(propertyName))
                    {
                        ArrayList ar = reloadFields_[propertyName] as ArrayList;
                        if ((ar != null) && (ar.Count > 0))
                        {
                            for (int i = 0; i < ar.Count; i++)
                            {
                                int typeid = (int)ar[i];
                                if (typeid == Rule.CONDITIONOBJECTTYPEID)
                                {
                                    conditionFieldFired = true;
                                }
                                if (typeid == Rule.DOCUMENTOBJECTTYPEID)
                                {
                                    documentFieldFired = true;
                                }
                            }
                        }
                    }
                    if (conditionFieldFired && documentFieldFired)
                    {
                        return;
                    }
                }
                //if((mp.FieldsReset!=null)&&(mp.FieldsReset.Count>0))
                //{
                //    for (int j = 0; j < mp.FieldsReset.Count; j++)
                //    {
                //        string propName = mp.FieldsReset[j].ToString();
                //        if (reloadFields_.ContainsKey(propName))
                //        {
                //            ArrayList ar = reloadFields_[propName] as ArrayList;
                //            if ((ar != null) && (ar.Count > 0))
                //            {
                //                for (int i = 0; i < ar.Count; i++)
                //                {
                //                    int typeid = (int)ar[i];
                //                    if (typeid == Rule.CONDITIONOBJECTTYPEID)
                //                    {
                //                        conditionFieldFired = true;
                //                    }
                //                    if (typeid == Rule.DOCUMENTOBJECTTYPEID)
                //                    {
                //                        documentFieldFired = true;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
            }
        }
        public virtual void BuildControl(Control _container)
        {
            container = _container;
            #region DEBUG ONLY
            CurrentPage.WriteToLog("BuildRuleEvaluationTree started");
            #endregion
            Mp.BuildRuleEvaluationTree(CurrentPage.GetRuleTreePublic());
            #region DEBUG ONLY
            CurrentPage.WriteToLog("BuildRuleEvaluationTree completed");
            #endregion
            #region DEBUG ONLY
            CurrentPage.WriteToLog("PrepairObjectFields started");
            #endregion
            PrepairObjectFields();
            #region DEBUG ONLY
            CurrentPage.WriteToLog("PrepairObjectFields completed");
            #endregion
            #region DEBUG ONLY
            CurrentPage.WriteToLog("PrepairObjectHtml started");
            #endregion
            PrepairObjectHtml();
            #region DEBUG ONLY
            CurrentPage.WriteToLog("PrepairObjectHtml completed");
            #endregion
        }        
        #endregion

        #region private
        private static int GetFieldFromList(string fieldName, IList fields)
        {
            for (int i = 0; i < fields.Count; i++)
            {
                MortgageProfileField mpf = (MortgageProfileField)fields[i];
                if (mpf.PropertyName == fieldName)
                {
                    return i;
                }
            }
            return -1;
        }
        private static bool CheckDependantFields(string objName, Hashtable o, IList fields)
        {
            for (int i = 0; i < fields.Count; i++)
            {
                string key = ((MortgageProfileField)fields[i]).PropertyName;
                if (!String.IsNullOrEmpty(objName))
                {
                    key = objName + "." + key;
                }
                if (o.ContainsKey(key))
                {
                    return true;
                }
            }
            return false;
        }
        protected bool GetRulesValue(RuleField rf)
        {
            bool res = true;
            for (int i = 0; i < rf.Rules.Count; i++)
            {
                res = res && GetRuleValue(Convert.ToInt32(rf.Rules[i]), FieldsVisibilityRules);
            }
            return res;
        }
        protected static bool GetRuleValue(int ruleid, ArrayList rules)
        {
            bool res = true;
            for (int i = 0; i < rules.Count; i++)
            {
                RuleEvaluation re = (RuleEvaluation)rules[i];
                if (re.Id == ruleid)
                {
                    return re.EvaluationValue;
                }
            }
            return res;
        }
        private void GetDependantFields(Hashtable o, int ruleId)
        {
            if (o == null)
            {
                o = new Hashtable();
            }
            RuleDependantFields.RowFilter = "ruleid=" + ruleId;

            for (int i = 0; i < RuleDependantFields.Count; i++)
            {
                string propName = RuleDependantFields[i]["propertyname"].ToString();
                int ruleid = int.Parse(RuleDependantFields[i]["ruleid"].ToString());
                ArrayList ruleidlist = (ArrayList)o[propName];
                if (ruleidlist == null)
                {
                    ruleidlist = new ArrayList();
                    ruleidlist.Add(ruleid);
                    o.Add(propName, ruleidlist);
                }
                else
                {
                    ruleidlist.Add(ruleid);
                    o[propName] = ruleidlist;
                }
            }

        }
        private static ArrayList GetObjectFields(RuleEvaluation re, string _objectName)
        {
            ArrayList result = new ArrayList();
            for (int i = 0; i < re.Units.Count; i++)
            {
                RuleEvaluationUnit ru = (RuleEvaluationUnit)re.Units[i];
                if (ru.PropertyName.StartsWith(_objectName))
                {
                    result.Add(ru.PropertyName);
                }
            }
            return result;
        }
        private static ArrayList GetGroupVisibleFields(IList allfields, int uigroupId)
        {
            ArrayList res = new ArrayList();
            for (int i = 0; i < allfields.Count; i++)
            {
                MortgageProfileField mpf = (MortgageProfileField)allfields[i];
                if ((mpf.FieldUIGroupId == uigroupId) && (mpf.IsVisible))
                {
                    res.Add(allfields[i]);
                }
            }
            return res;
        }

        #endregion

        private void PrepairObjectFields()
        {
            objectFields = Mp.GetObjectFields(objectName);
            SetUIFields();
            allShownFields = GetAllFieldsToDisplay(objectFields);
            PreparePostBackFields(objectName, allShownFields, PostBackObjectFields);
            PrepareConditionPostBackField(objectName, allShownFields, ReloadFields);
        }
        protected virtual void PrepairObjectHtml()
        {
        }
        protected virtual void SetUIFields()
        { 
        }
        #endregion
    }
}
