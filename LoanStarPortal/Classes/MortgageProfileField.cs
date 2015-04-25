using System;
using System.Collections;
using System.Reflection;
using System.Data;
using System.Text.RegularExpressions;


namespace LoanStar.Common
{
    public enum MPFieldType
    {
        Unknown = 0,
        TextBox = 1,
        TextArea = 2,
        DropDownList = 3,
        CheckBox = 4,
        DateInput = 5,
        Label = 6,
        RadioButtonList = 7,
        MaskedInput = 8,
        MoneyInput = 9,
        YesNo = 10
    }

/*    public enum MPFieldGroup
    {
        Unknown             = 0,
        Borrower            = 1,
        Property            = 2,
        Mortgage            = 3,
        Insurance           = 4,
        ImpoundsEscrows     = 5,
        Creditors           = 6,
        Advisors            = 7,
        PaymentPlan         = 8,
        VendorsInvoices     = 9,
        Payoffs             = 10,
        PrepaidItems        = 11,
        RepairItems         = 12,
        Invoice             = 13,
        Reserve             = 14
    }*/  //This is temporary unused

    public class MortgageProfileField : BaseObject
    {
        /// <summary>
        /// Any method of MortgageProfileField class must throw custom exceptions of this type only
        /// </summary>
        public class MortgageProfileFieldException : BaseObjectException
        {
            public MortgageProfileFieldException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public MortgageProfileFieldException(string message)
                : base(message)
            {
            }

            public MortgageProfileFieldException()
            {
            }
        }

        private const string GETOBJECTERRORMESSAGES = "GetObjectErrorMessages";
        private const string GETMORTGAGEPROFILEFIELDBYID = "GetMortgageProfileFieldById";

        #region Private fields
        private string tableName = string.Empty;
        private string fieldName = string.Empty;
        private string decsription = string.Empty;
        private int valueTypeID;
        private MPFieldType controlTypeId = MPFieldType.TextBox;
        private int fieldGroupId;
        private bool dictionaryField;
        private string propertyName = string.Empty;
        private object fieldValue;
        private int displayOrder;
        private bool readOnly;
        private string validationMessage = String.Empty;
        private bool isValid = true;
        private bool isVisible = true;
        private bool isPostBack = false;
        private int fieldUIGroupId;
        private string fullPropertyName = String.Empty;
        private int parentId = -1;
        private string maskValue = String.Empty;
        private string filterName = String.Empty;
        private string dataSource = String.Empty;
        private string mapPattern = String.Empty;
        private string mapReplacement = String.Empty;
        #endregion

        #region Public fields
        public bool ContainsDictionaryInfo
        {
            get { return !String.IsNullOrEmpty(TableName) && !String.IsNullOrEmpty(FieldName); }
        }
        public bool ContainsMapFilter
        {
            get { return !String.IsNullOrEmpty(MapPattern) && !String.IsNullOrEmpty(MapReplacement); }
        }
        public string MapPattern
        {
            get { return mapPattern; }
        }
        public string MapReplacement
        {
            get { return mapReplacement; }
        }
        public string DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }
        public int ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }
        public int FieldUIGroupId
        {
            get { return fieldUIGroupId; }
            set { fieldUIGroupId = value; }
        }
        public int DisplayOrder
        {
            get { return displayOrder; }
        }
        public bool ReadOnly
        {
            get { return readOnly; }
        }
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }
        public string Decsription
        {
            get { return decsription; }
            set { decsription = value; }
        }
        public int ValueTypeID
        {
            get { return valueTypeID; }
            set { valueTypeID = value; }
        }
        public MPFieldType ControlTypeId
        {
            get { return controlTypeId; }
            set { controlTypeId = value; }
        }
        public int FieldGroupId
        {
            get { return fieldGroupId; }
            set { fieldGroupId = value; }
        }
/*        public MPFieldGroup FieldGroup    //This is temporary unused
        {
            get
            {
                if (FieldGroupId > (int)MPFieldGroup.Reserve)
                    return MPFieldGroup.Unknown;

                return (MPFieldGroup)FieldGroupId;
            }
        }*/
        public bool DictionaryField
        {
            get { return dictionaryField; }
            set { dictionaryField = value; }
        }
        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }
        public object FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }
        public string ValidationMessage
        {
            get { return validationMessage; }
        }
        public bool IsValid
        {
            get { return isValid; }
            set { isValid = value; }
        }
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }
        public bool IsPostBack
        {
            get { return isPostBack; }
            set { isPostBack = value; }
        }
        public string FullPropertyName
        {
            get { return fullPropertyName; }
        }
        public string MaskValue
        {
            get { return maskValue; }
        }
        public string FilterName
        {
            get
            {
                return filterName;
            }
            set
            {
                filterName = value;
            }
        }

        #endregion


        #region Constructors
        public MortgageProfileField(): this(0)
	    {
	    }
        public MortgageProfileField(DataRowView dr)
        {
            LoadFromDataRow(dr);
        }
        public MortgageProfileField(int id)
        {
            ID = id;
            if (ID > 0)
            {
                DataView dv = db.GetDataView(GETMORTGAGEPROFILEFIELDBYID, ID);
                if (dv.Count > 0)
                {
                    LoadFromDataRow(dv[0]);
                }
            }
        }
        #endregion

        #region methods

        #region Public methods
        public DataView ReadDataSource() //this method will be changed in the future
        {
            DataTable dt;
            if (!String.IsNullOrEmpty(DataSource))
                dt = GetDictionaryTable(DataSource);
            else if (!String.IsNullOrEmpty(TableName))
                dt = GetDictionaryTable(TableName);
            else
                dt = new DataTable();
            DataView dv = new DataView(dt);

            if (dv.Count > 0 && !String.IsNullOrEmpty(FilterName))
            {
                Regex reg = new Regex(@"\{\w+\.\w+\}");
                Match m = reg.Match(FilterName);
                if (m.Captures.Count == 0)
                    dv.RowFilter = FilterName;
            }

            return dv;
        }
        #endregion

        #region Private methods
        private void LoadFromDataRow(DataRowView dr)
        {
            ID = int.Parse(dr["id"].ToString());
            decsription = dr["Description"].ToString();
            valueTypeID = Convert.ToInt32(dr["ValueTypeID"].ToString());
            controlTypeId = (MPFieldType)Convert.ToInt16(dr["ControlTypeId"]);
            propertyName = dr["PropertyName"].ToString();
            displayOrder = Convert.ToInt32(dr["DisplayOrder"].ToString());
            if (dr.DataView.Table.Columns.Contains("Readonly"))
            {
                readOnly = bool.Parse(dr["Readonly"].ToString());
            }
            tableName = dr["TableName"].ToString();
            fieldName = dr["FieldName"].ToString();
            validationMessage = dr["ValidationMessage"].ToString();
            if (dr.DataView.Table.Columns.Contains("FullPropertyName"))
            {
                fullPropertyName = dr["FullPropertyName"].ToString();
            }
            isPostBack = bool.Parse(dr["isPostBack"].ToString());
            if(dr.DataView.Table.Columns.Contains("MaskValue"))
            {
                maskValue = dr["MaskValue"].ToString();
            }
            if (dr.DataView.Table.Columns.Contains("FilterName"))
            {
                filterName = dr["FilterName"].ToString();
            }
            if (dr.DataView.Table.Columns.Contains("DataSource"))
            {
                dataSource = dr["DataSource"].ToString().Replace("TABLENAME:", null);
            }
            if (dr.DataView.Table.Columns.Contains("MapFilterName"))
            {
                string mapFilterName = dr["MapFilterName"].ToString();
                if (!String.IsNullOrEmpty(mapFilterName))
                {
                    string[] mapFilterNameArr = mapFilterName.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    mapPattern = mapFilterNameArr[0].Trim();
                    mapReplacement = mapFilterNameArr[1].Trim();
                }
            }
        }
        #endregion

        #region Static methods
        public static Hashtable GetErrorMessages(string objectName)
        {
            Hashtable tbl = new Hashtable();
            DataView dv = db.GetDataView(GETOBJECTERRORMESSAGES, objectName);
            for (int i = 0; i < dv.Count; i++)
            {
                string name = dv[i]["PropertyName"].ToString();
                string val = dv[i]["ValidationMessage"].ToString();
                tbl.Add(name, val);
            }
            return tbl;

        }
        public static Hashtable GetMortgageProfileFieldByDTIDs(string docTemplatesXml)
        {
            Hashtable mpFieldHash = new Hashtable();
            if (docTemplatesXml.Trim().Length == 0)
                return mpFieldHash;

            DataView mpFieldView = db.GetDataView("GetMortgageProfileFieldByDTIDs", docTemplatesXml);
            foreach (DataRowView mpFieldRow in mpFieldView)
            {
                MortgageProfileField mpField = new MortgageProfileField(mpFieldRow);
                mpFieldHash[mpField.ID] = mpField;
            }

            return mpFieldHash;
        }
        public static void SetError(ArrayList fields, Hashtable errFields, string fieldName, bool isValid)
        {
            for (int i = 0; i < fields.Count; i++)
            {
                if (((MortgageProfileField)fields[i]).PropertyName == fieldName)
                {
                    errFields.Add(fieldName, ((MortgageProfileField)fields[i]).ValidationMessage);
                    break;
                }
            }
        }
        public static bool ValidateRegexp(string data, string exp)
        {
            return Regex.Match(data, exp).Success;
        }
        #endregion

        #endregion

}

    public class MortgageProfileFieldInfo :BaseObject
    {
        #region constants
        private const string IDFIELDNAME = "id";
        private const string DATASOURCENAMEFIELDNAME = "DataSource";
        private const string TABLENAMEFIELDNAME = "tablename";
        private const string FIELDNAMEFIELDNAME = "fieldname";
        private const string DESCRIPTIONFIELDNAME = "description";
        private const string PROPERTYNAMEFIELDNAME = "propertyname";
        private const string MASKVALUEFIELDNAME = "maskvalue";
        private const string FILTERNAMEFIELDNAME = "filtername";
        private const string VALIDATIONMESSAGEFIELDNAME = "validationmessage";
        private const string ISPOSTBACKFIELDNAME = "ispostback";
        private const string ISHIDDENFIELDNAME = "IsHidden";
        private const string GETTOPLEVELTABS = "GetMortgageInfoTabs";
        private const string GETSECONDLEVELTABS = "GetContentTabs";
        private const string GETCONTENTPSEUDOTABS = "GetContentPseudoTab";
        private const string GETCONTENTPSEUDOTABGROUPS = "GetContentPseudoTabGroup";
        private const string GETFIELDSDETAILS = "GetFieldDetails";
        private const string GETFIELDSINFO = "GetFieldsInfo";
        private const string GETFIELDSBYGROUPID = "GetFieldsByGroupId";
        private const string TABLENAMEKEY = "TABLENAME";
        private const string PROCEDURENAMEKEY = "PROCEDURENAME";
        private const string FILTERVALUEKEY = "FILTERVALUE";
        private const string PROCEDUREPARAMETERSKEY = "PROCEDUREPARAMETERS";
        private const string FIELDNAMEKEY = "FIELDNAME";
        private const int DATASOURCETYPETABLE = 1;
        private const int DATASOURCETYPEPROCEDURE = 2;
        #endregion

        #region fields
        private readonly int id = -1;
        private string tableName;
        private readonly string fieldName;
        private readonly string description;
        private readonly string fullPropertyName;
        private readonly string propertyName;
        private readonly string objectName;
        private readonly string maskValue;
        private readonly string filterValue;
        private readonly bool isPostBack;
        private string procedureName;
        private int datasourceTypeId = DATASOURCETYPETABLE;
        private bool isHidden = false;
//        private readonly Hashtable dataSourceHash = new Hashtable();
        #endregion

        #region properties
        public int Id
        {
            get { return id; }
        }
        public string FullPropertyName
        {
            get { return fullPropertyName; }
        }
        public string PropertyName
        {
            get { return propertyName; }
        }
        public string ObjectName
        {
            get { return objectName; }
        }
        public string Description
        {
            get { return description; }
        }
        public string TableName
        {
            get { return tableName; }
        }
        public string FieldName
        {
            get { return fieldName; }
        }
        public string FilterValue
        {
            get { return filterValue; }
        }
        public string MaskValue
        {
            get { return maskValue; }
        }
        public bool IsPostBack
        {
            get { return isPostBack; }
        }
        public bool IsDataSourceProcedure
        {
            get { return datasourceTypeId == DATASOURCETYPEPROCEDURE; }
        }
        public string ProcedureName
        {
            get { return procedureName; }
        }
        public bool IsHidden
        {
            get { return isHidden; }
        }
        //public string SourceProcedureName
        //{
        //    get
        //    {
        //        return dataSourceHash.Contains(PROCEDURENAMEKEY) ? 
        //            dataSourceHash[PROCEDURENAMEKEY].ToString() : String.Empty;
        //    }
        //}
        //public string SourceProcedureParameters
        //{
        //    get
        //    {
        //        return dataSourceHash.Contains(PROCEDUREPARAMETERSKEY) ? 
        //            dataSourceHash[PROCEDUREPARAMETERSKEY].ToString() : String.Empty;
        //    }
        //}
        //public string SourceTableName
        //{
        //    get
        //    {
        //        return dataSourceHash.Contains(TABLENAMEKEY) ? 
        //            dataSourceHash[TABLENAMEKEY].ToString() : TableName;
        //    }
        //}
        //public string SourceFieldName
        //{
        //    get
        //    {
        //        return dataSourceHash.Contains(FIELDNAMEKEY) ? 
        //            dataSourceHash[FIELDNAMEKEY].ToString() : FieldName;
        //    }
        //}
        //public string SourceFilterValue
        //{
        //    get
        //    {
        //        return dataSourceHash.Contains(FILTERVALUEKEY) ? 
        //            dataSourceHash[FILTERVALUEKEY].ToString() : FilterValue;
        //    }
        //}
        public bool IsDictionaryField
        {
            get { return !String.IsNullOrEmpty(tableName); }
        }
        #endregion

        #region Methods
        private void ParseDataSource(string dataSource)
        {
            if (!String.IsNullOrEmpty(dataSource))
            {
                string[] parts = dataSource.Split(':');
                if (parts.Length != 2) return;
                if (parts[0] == TABLENAMEKEY)
                {
                    tableName = parts[1];
                }
                else if (parts[0] == PROCEDURENAMEKEY)
                {
                    procedureName = parts[1];
                    datasourceTypeId = DATASOURCETYPEPROCEDURE;
                }
            }
        }

        //private void ParseDataSource(string dataSource)
        //{
        //    string[] paramValueArr = dataSource.Split(';');
        //    foreach(string paramValue in paramValueArr)
        //        if (!String.IsNullOrEmpty(paramValue))
        //        {
        //            string[] parts = paramValue.Split(':');
        //            if (parts.Length != 2)
        //                continue;

        //            dataSourceHash[parts[0].Trim().ToUpper()] = parts[1].Trim();
        //        }
        //}

        public static bool UpdateFieldInfo(int fieldId,int groupId, string description, int displayOrder)
        {
            return db.ExecuteScalarInt("UpdateFieldInfo", fieldId, groupId, description, displayOrder) > 0;
        }
        #endregion

        #region constructor
        public MortgageProfileFieldInfo(DataRow dr)
        {
            if (dr != null)
            {
                id = int.Parse(dr[IDFIELDNAME].ToString());
                tableName = dr[TABLENAMEFIELDNAME].ToString();
                fieldName = dr[FIELDNAMEFIELDNAME].ToString();
                description = dr[DESCRIPTIONFIELDNAME].ToString();
                fullPropertyName = dr[PROPERTYNAMEFIELDNAME].ToString();
                string[] names = fullPropertyName.Split('.');
                if (names.Length == 2)
                {
                    objectName = names[0];
                    propertyName = names[1];
                }
                maskValue = dr[MASKVALUEFIELDNAME].ToString();
                filterValue = dr[FILTERNAMEFIELDNAME].ToString();
                isPostBack = bool.Parse(dr[ISPOSTBACKFIELDNAME].ToString());
                isHidden = bool.Parse(dr[ISHIDDENFIELDNAME].ToString());
                ParseDataSource(dr[DATASOURCENAMEFIELDNAME].ToString());
            }
        }
        #endregion

        #region methods
        public static DataView GetFieldsInfo()
        {
            return db.GetDataView(GETFIELDSINFO);
        }
        public static DataView GetFieldInGroup(int groupId)
        {
            return db.GetDataView(GETFIELDSBYGROUPID, groupId);
        }
        public static DataView GetTopLevelTab()
        {
            return db.GetDataView(GETTOPLEVELTABS);
        }
        public static DataView GetSecondLevelTabs()
        {
            return db.GetDataView(GETSECONDLEVELTABS);
        }
        public static DataView GetPseudoTabs()
        {
            return db.GetDataView(GETCONTENTPSEUDOTABS);
        }
        public static DataView GetPseudoTabGroups()
        {
            return db.GetDataView(GETCONTENTPSEUDOTABGROUPS);
        }
        public static DataSet GetFieldDetails(int id)
        {
            return db.GetDataSet(GETFIELDSDETAILS, id);
        }
        public static void GetBLLFieldInfo(string fullPropertyName, out string typeName, out bool isNullable, out bool isReadOnly)
        {
            isNullable = false;
            isReadOnly = false;
            typeName = "N/A";
            try
            {
                int i = fullPropertyName.IndexOf('.');
                if (i < 0)
                {
                    return;
                }
                string objectName = fullPropertyName.Substring(0, i);
                string objectPropertyName = fullPropertyName.Substring(i + 1);
                Type t = typeof (MortgageProfile);
                PropertyInfo pi = t.GetProperty(objectName);
                Type t1 = pi.PropertyType;
                if(t1.IsGenericType)
                {
                    pi = t1.GetGenericArguments()[0].GetProperty(objectPropertyName);
                }
                else
                {
                    pi = t1.GetProperty(objectPropertyName);
                }
                t1 = pi.PropertyType;
                isNullable = IsNullableType(t1);
                if(isNullable)
                {
                    t1 = Nullable.GetUnderlyingType(t1);
                }
                isReadOnly = !pi.CanWrite;
                typeName = t1.Name;
            }
            catch
            {
            }
        }
        public static void GetDbFieldInfo(string fullPropertyName,out string tableName, out string dbType, out bool isNullable, out int length)
        {
            isNullable = false;
            length = 0;
            dbType = "N/A";
            tableName = String.Empty;
            try
            {
                int i = fullPropertyName.IndexOf('.');
                if (i < 0)
                {
                    return;
                }
                string objectName = fullPropertyName.Substring(0, i);
                string objectPropertyName = fullPropertyName.Substring(i + 1);
                Type t = typeof(MortgageProfile);
                PropertyInfo pi = t.GetProperty(objectName);
                object[] attributes = pi.GetCustomAttributes(true);
                foreach (Object attribute in attributes)
                {
                    DbMapping mapping = attribute as DbMapping;
                    if (mapping != null)
                    {
                        tableName = mapping.TableName;
                        break;
                    }

                }
                if(!String.IsNullOrEmpty(tableName))
                {
                    DataView dv = db.GetDataView("GetDbFieldInfo", tableName, objectPropertyName);
                    if(dv.Count==1)
                    {
                        isNullable = dv[0]["Is_nullable"].ToString().ToLower() == "yes";
                        dbType = dv[0]["data_type"].ToString();
                        if(dv[0]["character_maximum_length"]!=DBNull.Value)
                        {
                            length = int.Parse(dv[0]["character_maximum_length"].ToString());
                        }
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

    }
    public class FieldVisualData : BaseObject
    {
        #region fields
        private string label = String.Empty;
        private int displayOrder = 0;
        private int toplevelTabId = 0;
        private int level2TabId = 0;
        private int pseudoTabId = 0;
        private int pseudoTabGroupId = 0;
        #endregion

        #region properties
        public string Label
        {
            get { return label; }
            set { label = value; }
        }
        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }
        public int ToplevelTabId
        {
            get { return toplevelTabId; }
            set { toplevelTabId = value; }
        }
        public int Level2TabId
        {
            get { return level2TabId; }
            set { level2TabId = value; }
        }

        public int PseudoTabId
        {
            get { return pseudoTabId; }
            set { pseudoTabId = value; }
        }
        public int PseudoTabGroupId
        {
            get { return pseudoTabGroupId; }
            set { pseudoTabGroupId = value; }
        }
        #endregion

        #region contsructor
        public FieldVisualData(DataRowView dr)
        {
            if (dr != null)
            {
                label = dr["description"].ToString();
                displayOrder = int.Parse(dr["displayorder"].ToString());
                toplevelTabId = int.Parse(dr["tab1id"].ToString());
                level2TabId = int.Parse(dr["tab2id"].ToString());
                level2TabId = int.Parse(dr["tab2id"].ToString());
                pseudoTabId = int.Parse(dr["pseudotabid"].ToString());
                pseudoTabGroupId = int.Parse(dr["groupid"].ToString());
            }
        }
        #endregion

        #region methods
        public int Update(int fieldId, int groupId)
        {
            return db.ExecuteScalarInt("UpdateFieldInfo", fieldId, groupId, label, displayOrder,pseudoTabGroupId);
        }
        #endregion
    }
}
