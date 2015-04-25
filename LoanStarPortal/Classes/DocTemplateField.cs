using System;
using System.Collections;
using System.Data;
using BossDev.CommonUtils;

namespace LoanStar.Common
{
    public enum DTVFieldType
    {
        FieldText = 0, 
        FieldBool = 1
    }

    public class DocTemplateField : BaseObject
    {
        /// <summary>
        /// Any method of DocTemplateField class must throw custom exceptions of this type only
        /// </summary>
        public class DocTemplateFieldException : BaseObjectException
        {
            public DocTemplateFieldException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public DocTemplateFieldException(string message)
                : base(message)
            {
            }

            public DocTemplateFieldException()
            {
            }
        }

        #region Private fields
        private int docTemplateVersionID = -1;
        private int mpFieldID;
        private string mpFieldName = String.Empty;
        private string dtvFieldName = String.Empty;
        private string regionName = String.Empty;
        private DTVFieldType fieldType = DTVFieldType.FieldText;
        private string fieldTypeDescription = String.Empty;
        private int groupIndex1;
        private int groupIndex2;

//        private static readonly DatabaseAccess dbAccess = new DatabaseAccess(AppSettings.SqlConnectionString);
        #endregion

        #region Constructors
        public DocTemplateField(DataRowView fieldRow)
        {
            ID = ConvertToInt( fieldRow["ID"], -1 );
            docTemplateVersionID = ConvertToInt(fieldRow["DocTemplateVerID"], -1);
            MPFieldID = ConvertToInt( fieldRow["FieldID"], -1 );
            MPFieldName = ConvertToString( fieldRow["MPFiledName"], String.Empty );
            DTVFieldName = ConvertToString(fieldRow["DTVFieldName"], null);
            FieldType = (DTVFieldType)ConvertToInt(fieldRow["TypeID"], 0);
            FieldTypeDescription = ConvertToString( fieldRow["Type"], String.Empty );
            GroupIndex1 = ConvertToInt(fieldRow["GroupIndex1"], 0);
            GroupIndex2 = ConvertToInt(fieldRow["GroupIndex2"], 0);
            RegionName = ConvertToString(fieldRow["RegionName"], String.Empty);
        }

        public DocTemplateField(string _dtvFieldName, DTVFieldType _fieldType)
        {
            DTVFieldName = _dtvFieldName;
            FieldType = _fieldType;
        }

        public DocTemplateField(string _dtvFieldName, DTVFieldType _fieldType, string _regionName)
        {
            DTVFieldName = _dtvFieldName;
            FieldType = _fieldType;
            RegionName = _regionName;
        }
        #endregion

        #region Properties
        public string Key
        {
            get
            {
                string key = String.IsNullOrEmpty(RegionName) ?
                                DTVFieldName :
                                String.Format("{0}:{1}", RegionName, DTVFieldName);

                return key;
            }
        }

        public int GroupIndex1
        {
            get
            {
                return groupIndex1;
            }
            set
            {
                groupIndex1 = value;
            }
        }

        public int GroupIndex2
        {
            get
            {
                return groupIndex2;
            }
            set
            {
                groupIndex2 = value;
            }
        }

        public int DocTemplateVersionID
        {
            get
            {
                return docTemplateVersionID;
            }
            set
            {
                docTemplateVersionID = value;
            }
        }

        public int MPFieldID
        {
            get
            {
                return mpFieldID;
            }
            set
            {
                mpFieldID = value;
            }
        }

        public string MPFieldName
        {
            get
            {
                return mpFieldName;
            }
            set
            {
                mpFieldName = value == null ? String.Empty : value;
            }
        }

        public string MPGroupFieldName
        {
            get
            {
                int groupIndex = mpFieldName.LastIndexOf('.');
                return groupIndex < 0 ? String.Empty : mpFieldName.Substring(0, groupIndex);
            }
        }

        public DTVFieldType FieldType
        {
            get
            {
                return fieldType;
            }
            set
            {
                fieldType = value;
            }
        }

        public string FieldTypeDescription
        {
            get
            {
                return fieldTypeDescription;
            }
            set
            {
                fieldTypeDescription = value == null ? String.Empty : value;
            }
        }

        public string DTVFieldName
        {
            get
            {
                return dtvFieldName;
            }
            set
            {
                dtvFieldName = value == null ? String.Empty : value;
            }
        }

        public string RegionName
        {
            get
            {
                return regionName;
            }
            set
            {
                regionName = value == null ? String.Empty : value;
            }
        }
        #endregion

        #region Public methods
        public void Update()
        {
            Update(db);
        }

        public void Update(DatabaseAccess _dbAccess)
        {
            int res = _dbAccess.ExecuteScalarInt("EditDocTemplateField", 
                                                    ID,
                                                    DocTemplateVersionID, 
                                                    MPFieldID, 
                                                    ConvertToInt(FieldType, 0),
                                                    DTVFieldName,
                                                    GroupIndex1,
                                                    GroupIndex2, 
                                                    String.IsNullOrEmpty(RegionName) ? null : RegionName);

            if(res <= 0)
                throw new DocTemplateFieldException("Document template field was not updated succesfully");

            ID = ID <= 0 ? res : ID;
        }

        public void Delete(DatabaseAccess _dbAccess)
        {
            int res = _dbAccess.ExecuteScalarInt("DeleteDocTemplateField", ID);

            if (res <= 0)
                throw new DocTemplateFieldException("Document template field was not deleted succesfully");

            ID = -1;
        }
        #endregion

        #region Static methods
        public static void DeleteFieldList(ICollection fieldList, DatabaseAccess _dbAccess)
        {
            IEnumerator fieldEnumerator = fieldList.GetEnumerator();
            while (fieldEnumerator.MoveNext())
                (fieldEnumerator.Current as DocTemplateField).Delete(_dbAccess);

            fieldEnumerator.Reset();
        }

        public static void UpdateFieldList(ICollection fieldList, DatabaseAccess _dbAccess)
        {
            IEnumerator fieldEnumerator = fieldList.GetEnumerator();
            while(fieldEnumerator.MoveNext())
                (fieldEnumerator.Current as DocTemplateField).Update(_dbAccess);

            fieldEnumerator.Reset();
        }

        public static Hashtable GetDocFieldHash(DataView docFieldView)
        {
            Hashtable docFieldHash = new Hashtable();
            foreach (DataRowView fieldRow in docFieldView)
            {
                DocTemplateField docTemplateField = new DocTemplateField(fieldRow);
                docFieldHash[docTemplateField.Key] = docTemplateField;
            }

            return docFieldHash;
        }

        public DataTable GetRegionTable()
        {
            DataTable regionTable = new DataTable();
            regionTable.TableName = RegionName;
            return regionTable;
        }
        #endregion
    }
}
