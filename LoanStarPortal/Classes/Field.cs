using System;
using System.Data;

namespace LoanStar.Common
{
    public class Field :BaseObject
    {
        #region constants

        #region db fields name
        public const string NAMEFIELDNAME = "name";
        public const string DESCRIPTIONFIELDNAME = "description";
        public const string IDFIELDNAME = "id";
        public const string COMPOPNAMEFIELDNAME = "name";
        public const string GROUPNAMEFIELDNAME = "name";
        public const string TABLENAMEFIELDNAME = "TableName";
        public const string FIELDNAMEFIELDNAME = "FieldName";
        public const string DICTIONARYFIELDNAME = "DictionaryField";
        public const string FIELDTYPEFIELDNAME = "ValueTypeId";
        public const string DATAVALUEFIELDNAME = "datavalue";
        public const string LOGICALNOTFIELDNAME = "logicalnot";
        public const string FILTERNAMEFIELDNAME = "FilterName";
        #endregion

        #region stored procedure names
        private const string GETFIELDGROUP = "GetFieldGroup";
        private const string GETFIELDGROUPINDEX = "GetFieldGroupIndex";
        private const string GETFIELDINGROUP = "GetFieldInGroup";
        private const string GETLISTFORSELECET = "GetSelectFieldList";
        private const string GETCOMPAREOPLIST = "GetCompareOperationList";
        private const string GETLOGICOPLIST = "GetLogicOperationList";
        private const string LOADFIELDBYID = "LoadFieldById";
        private const string GETDICTIONARYLIST = "GetDictionaryList";
        private const string GETALLOWEDCOMOOPLIST = "GetAllowedCompOpList";
        private const string GETFIELDINGROUPWITHSELECTEDTYPE = "GetFieldInGroupWithSelectedType";
        private const string GETFIELDLIST = "GetFieldList";
        private const string GETFIELDCHANGES = "GetFieldChanges";
        private const string GETREQUIREDFIELDLIST = "GetRequiredFieldList";
        private const string SAVEREQUIREDFIELDLIST = "SaveRequiredFieldList";
        #endregion

        #endregion

        #region enumeration
        internal enum MortgageProfileFieldType
        { 
            NotSelected = 0,
            String = 1,
            DateTime = 2,
            Integer = 3,
            Float = 4,
            Boolean = 5,
            Decimal = 6
        }
        #endregion

        #region fields
        private string tableName = String.Empty;
        private string fieldName = String.Empty;
        private bool isDictionaryField;
        private int typeId;
        private string filterName = String.Empty;
        #endregion

        #region properties

        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
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
        public bool IsDictionaryField
        {
            get { return isDictionaryField; }
            set { isDictionaryField = value; }
        }
        public string FilterName
        {
            get { return filterName; }
            set { filterName = value; }
        }
        #endregion

        #region Constructor
        public Field()
        { 
        }
        public Field(int id)
        {
            ID = id;
            LoadById();
        }
        #endregion

        #region methods

        #region instance

        #region public methods
        public DataView GetFieldList()
        {
            return db.GetDataView(GETFIELDLIST, ID, typeId);
        }
        public DataView GetDictionaryList()
        {
            return db.GetDataView(GETDICTIONARYLIST, tableName, fieldName);
        }
        public DataView GetAllowedCompOpList()
        {
            return db.GetDataView(GETALLOWEDCOMOOPLIST, typeId);
        }
        #endregion

        #region private methods
        private void LoadById()
        {
            if (ID > 0)
            {
                DataView dv = db.GetDataView(LOADFIELDBYID, ID);
                if (dv.Count == 1)
                {
                    tableName = dv[0][TABLENAMEFIELDNAME].ToString();
                    fieldName = dv[0][FIELDNAMEFIELDNAME].ToString();
                    isDictionaryField = bool.Parse(dv[0][DICTIONARYFIELDNAME].ToString());
                    typeId = int.Parse(dv[0][FIELDTYPEFIELDNAME].ToString());
                    filterName = dv[0][FILTERNAMEFIELDNAME].ToString();
                }
                else
                {
                    ID = -1;
                }
            }
        }
        #endregion

        #endregion

        #region static
        public static DataView GetFieldGroup(bool all)
        {
            return db.GetDataView(GETFIELDGROUP, all);
        }
        public static DataView GetFieldGroupIndex()
        {
            return db.GetDataView(GETFIELDGROUPINDEX);
        }
        public static DataView GetFieldInGroup(int groupId)
        {
            return db.GetDataView(GETFIELDINGROUP, groupId);
        }
        public static DataView GetSelectList()
        {
            return db.GetDataView(GETLISTFORSELECET);
        }
        public static DataView GetCompareOperationList()
        {
            return db.GetDataView(GETCOMPAREOPLIST);
        }
        public static DataView GetLogicOperationList()
        {
            return db.GetDataView(GETLOGICOPLIST);
        }
        public static DataView GetFieldInGroupWithSelectedType(int groupId, int groupIndex1, int groupIndex2)
        {
            return db.GetDataView(GETFIELDINGROUPWITHSELECTEDTYPE, groupId, groupIndex1, groupIndex2);
        }

        public static DataView GetFieldChanges(int MortageID, int MPFieldID)
        {
            return db.GetDataView(GETFIELDCHANGES, MortageID, MPFieldID);
        }
        public static DataView GetReqiredFieldsList(int companyId)
        {
            return db.GetDataView(GETREQUIREDFIELDLIST, companyId);
        }
        public static bool SaveRequiredFieldList(string data, int companyId, int statusId)
        {
            return db.ExecuteScalarInt(SAVEREQUIREDFIELDLIST, companyId, statusId, data) == 1;
        }

        #endregion

        #endregion
    }

}
