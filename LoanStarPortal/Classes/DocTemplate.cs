using System;
using System.Data;
using BossDev.CommonUtils;

namespace LoanStar.Common
{
    public class DocTemplate : BaseObject
    {
        /// <summary>
        /// Any method of DocTemplate class must throw custom exceptions of this type only
        /// </summary>
        public class DocTemplateException : BaseObjectException
        {
            public DocTemplateException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public DocTemplateException(string message)
                : base(message)
            {
            }

            public DocTemplateException()
            {
            }
        }

        #region Private fields
        private string title = String.Empty;
        private int docGroupID = 0;
        private bool archived;
        private int versionID;

//        private static readonly DatabaseAccess dbAccess = new DatabaseAccess(AppSettings.SqlConnectionString);
        #endregion

        #region DB field map
        public class DBFieldMap
        {
            public static string ID = "ID";
            public static string Title = "Title";
            public static string FileName = "FileName";
            public static string UploadDate = "UploadDate";
            public static string Archived = "Archived";
            public static string DocTemplateID = "DocTemplateID";
            public static string CompanyID = "CompanyID";
            public static string RuleID = "RuleID";
            public static string GroupID = "GroupID";
        }
        #endregion

        #region properties
        public int DocGroupID
        {
            get
            {
                return docGroupID;
            }
            set
            {
                docGroupID = value;
            }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public bool Archived
        {
            get { return archived; }
            set { archived = value; }
        }
        public int VersionID
        {
            get { return versionID; }
            set { versionID = value; }
        }
        #endregion

        public DocTemplate(): this(0)
	    {
	    }
        public DocTemplate(int id)
        {
            ID = id;
            if (ID <= 0)
                return;

            DataSet ds = db.GetDataSet("GetDocTemplateById", ID);
            if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ID = id;
                title = dr["Title"].ToString();
                docGroupID = ConvertToInt(dr["GroupID"], 0);
                archived = Convert.ToBoolean(dr["Archived"].ToString());
            }
        }

        #region Public Methods
        public string Update()
        {
            try
            {
                Update(db);
            }
            catch (Exception ex)
            {
                return String.Format("Document template with title {0} can't be stored to the database <br />Error: {1}", Title, ex.Message);
            }

            return String.Empty;
        }

        public void Update(DatabaseAccess _dbAccess)
        {
            int res;

            res = _dbAccess.ExecuteScalarInt("EditDocTemplate",
                                        ID,
                                        Title,
                                        DocGroupID,
                                        Archived);
            ID = ID <= 0 ? res : ID;
        }

        public void Archive()
        {
            db.ExecuteScalar("ArchiveUnarchiveDocTemplate",
                                    ID,
                                    true);
        }

        public void UnArchive()
        {
            db.ExecuteScalar("ArchiveUnarchiveDocTemplate",
                                    ID,
                                    false);
        }
        #endregion


        #region Static methods
        public static DataView GetDocTemplateList(string orderby, string whereclause)
        {
            return db.GetDataView("GetDocTemplateList", orderby, whereclause);
        }

        public static DataTable GetDocumenGroupList()
        {
            return db.GetDataTable("GetDocumentGroupList");
        }

        public static DataTable GetDocTemplateVersionList(int id)
        {
            return db.GetDataTable("GetDocTemplateVersionList", id);
        }

        #endregion
    }
}
