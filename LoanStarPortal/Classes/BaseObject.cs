using System;
using System.IO;
using System.Data;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using BossDev.CommonUtils;
using System.Web.Caching;

namespace LoanStar.Common
{
    public enum Status
    {
        Unknown     = 0,
        Enabled     = 1,
        Disabled    = 2,
        Deleted     = 3
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DbMapping : Attribute
    {
        public string ObjectName = String.Empty;
        public string TableName = String.Empty;
        public string FieldName = String.Empty;
        public DbMapping()
        {
        }
        public DbMapping(string objectName, string tableName, string fieldName)
        {
            ObjectName = objectName;
            TableName = tableName;
            FieldName = fieldName;
        }
    }

    [Serializable]
    public class BaseObject
    {
        /// <summary>
        /// Any method of BaseObject class must throw custom exceptions of this type only
        /// </summary>
        public class BaseObjectException : Exception
        {
            public BaseObjectException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public BaseObjectException(string message)
                : base(message)
            {
            }

            public BaseObjectException()
            {
            }
        }

        #region constants
        protected const string ITEMELEMENT = "item";
        private const string UPDATEOBJECT = "UpdateObject";
        #endregion

        protected BaseObject() { }
        //protected BaseObject(SerializationInfo info, StreamingContext context)
        //{
        //}

        #region Private fields
        private int id = -1;
        protected static DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);
        private bool isUpdated;
        protected bool isUpdating;
        #endregion

        protected bool isInitialLoad = false;
        #region Cacheing
        private static Cache cache = null;
        public static Cache Cache
        {
            get { return cache; }
            set { cache = value; }
        }
        public DataView GetDictionary(string dictionaryName)
        {
            if (Cache[dictionaryName] != null)
            {
                return Cache[dictionaryName] as DataView;
            }
            else
            {
                DataView dv = DataHelpers.GetDictionary(dictionaryName);
                Cache.Insert(dictionaryName, dv, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(AppPage.DICTIONARYSLIDINGDURATION));
                return dv;
            }
        }
        public DataTable GetDictionaryTable(string dictionaryName)
        {
            if (Cache[dictionaryName] != null)
            {
                return Cache[dictionaryName] as DataTable;
            }
            else
            {
                DataTable dv = DataHelpers.GetDictionaryTable(dictionaryName);
                Cache.Insert(dictionaryName, dv, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(AppPage.DICTIONARYSLIDINGDURATION));
                return dv;
            }
        }
        #endregion

        #region methods
        public virtual bool ValidateProperty(string propertyname, out string err)
        {
            err = "";
            return true;
        }
        public virtual int Save(MortgageProfile mp,string objectName,string fullPropertyName, string propertyName,int objectTypeId, Object val, Object oldVal, string parentFieldName, int parentId,bool isRequired)
        {
            return db.ExecuteScalarInt(UPDATEOBJECT, mp.ID, ID, objectName, propertyName, objectTypeId, val != null ? val.ToString() : "", oldVal != null ? oldVal.ToString() : "", mp.CurrentUserId, parentFieldName, parentId, fullPropertyName, isRequired);
        }
        public virtual XmlNode GetObjectXml(XmlDocument d)
        {
            XmlNode n = d.CreateElement(ITEMELEMENT);
            Type t = GetType();
            PropertyInfo[] pc = t.GetProperties();
            foreach (PropertyInfo pi in pc)
            {
                string propName = pi.Name;
                Object propValue = pi.GetValue(this, null);
                XmlAttribute a = d.CreateAttribute(propName);
                a.Value = propValue.ToString();
                n.Attributes.Append(a);
            }
            return n;
        }
        protected void PopulateFromDataRow(DataRowView dr)
        {
            isInitialLoad = true;
            PropertyInfo[] properties = GetType().GetProperties();
            for (int i = 0; i < properties.Length;i++ )
            {
                PropertyInfo pi = properties[i];
                string propertyName = pi.Name;
                if (dr.DataView.Table.Columns.Contains(propertyName))
                {
                    Object o = dr[propertyName];
                    if (o != DBNull.Value)
                    {
                        Type t = pi.PropertyType;
                        if(IsNullableType(t))
                        {
                            t = Nullable.GetUnderlyingType(t);
                        }
                        pi.SetValue(this, Convert.ChangeType(o, t), null);
                    }
                }
            }
            isInitialLoad = false;
        }
        #endregion

        public static bool IsNullableType(Type theType)
        {
            return (theType.IsGenericType && theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        #region Convert methods
        public virtual Guid ConvertToGuid(object obj, Guid defaultVal)
        {
            return ObjectConvert.ConvertToGuid(obj, defaultVal);
        }

        public virtual double ConvertToDouble(object obj, double defaultVal)
        {
            return ObjectConvert.ConvertToDouble(obj, defaultVal);
        }

        public virtual decimal ConvertToDecimal(object obj, decimal defaultVal)
        {
            return ObjectConvert.ConvertToDecimal(obj, defaultVal);
        }

        public virtual int ConvertToInt(object obj, int defaultVal)
        {
            return ObjectConvert.ConvertToInt(obj, defaultVal);
        }

        public virtual string ConvertToString(object obj, string defaultVal)
        {
            return ObjectConvert.ConvertToString(obj, defaultVal);
        }

        public virtual DateTime ConvertToDateTime(object obj, DateTime defaultVal)
        {
            return ObjectConvert.ConvertToDateTime(obj, defaultVal);
        }

        public virtual MemoryStream ConvertToStream(object obj)
        {
            return ObjectConvert.ConvertToStream(obj);
        }

        public virtual bool ConvertToBool(object obj, bool defaultVal)
        {
            return ObjectConvert.ConvertToBool(obj, defaultVal);
        }
        #endregion

        #region Properties
        [XmlIgnore]
        public virtual int ID
        {
            get
            {
                //if(id == 0)
                //    id = DataHelpers.GetRandomNumber(10000);
                return id;
            }
            set
            {
                id = value;
            }
        }
        [XmlIgnore]
        public virtual bool IsUpdated
        {
            get { return isUpdated; }
            set { isUpdated = value; }
        }
        #endregion
    }

    public sealed class ObjectConvert
    {
        #region Convert static methods
        public static Guid ConvertToGuid(object obj, Guid defaultVal)
        {
            return obj == null || obj == DBNull.Value ? defaultVal : new Guid(Convert.ToString(obj));
        }
        public static double ConvertToDouble(object obj, double defaultVal)
        {
            return obj == null || obj == DBNull.Value ? defaultVal : Convert.ToDouble(obj);
        }

        public static decimal ConvertToDecimal(object obj, decimal defaultVal)
        {
            return obj == null || obj == DBNull.Value ? defaultVal : Convert.ToDecimal(obj);
        }

        public static int ConvertToInt(object obj, int defaultVal)
        {
            return obj == null || obj == DBNull.Value ? defaultVal : Convert.ToInt32(obj);
        }

        public static string ConvertToString(object obj, string defaultVal)
        {
            string defaultStr = defaultVal == null || defaultVal.Trim().Length == 0 ? String.Empty : defaultVal;
            return obj == null || obj == DBNull.Value ? defaultStr : Convert.ToString(obj);
        }

        public static DateTime ConvertToDateTime(object obj, DateTime defaultVal)
        {
            //return obj == null || obj == DBNull.Value || (obj is String && (obj as String).Trim().Length == 0) ? defaultVal : Convert.ToDateTime(obj);
            return obj == null || obj == DBNull.Value || (obj is String && String.IsNullOrEmpty((String)obj)) ? defaultVal : Convert.ToDateTime(obj);
        }

        public static MemoryStream ConvertToStream(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return new MemoryStream();

            byte[] buf = (byte[])obj;
            MemoryStream ms = new MemoryStream(buf);
            ms.Flush();
            ms.Close();
            return ms;
        }

        public static bool ConvertToBool(object obj, bool defaultVal)
        {
            return obj == null || obj == DBNull.Value ? defaultVal : Convert.ToBoolean(obj);
        }
        #endregion
    }
}
