using System;
using System.Data;

namespace LoanStar.Common
{
    public class Condition : BaseObject
    {
        public const string SATISFIED_STATUS = "Satisfied";
        public const string NOT_SATISFIED_STATUS = "Not satisfied";

        public class ConditionException : BaseObjectException
        {
            public ConditionException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public ConditionException(string message)
                : base(message)
            {
            }

            public ConditionException()
            {
            }
        }



        #region fields
        private readonly DataRow dataRow;
        #endregion

        #region  properties
        public int MortgageID
        {
            set { dataRow["MortgageID"] = value; }
            get { return Convert.ToInt32(dataRow["MortgageID"]); }
        }
        public string Title
        {
            set { dataRow["Title"] = value; }
            get { return Convert.ToString(dataRow["Title"]); }
        }
        public string Description
        {
            set { dataRow["Description"] = value; }
            get { return Convert.ToString(dataRow["Description"]); }
        }

        public int AuthorityLevel
        {
            set { dataRow["AuthorityLevel"] = value; }
            get
            {
                if (dataRow["AuthorityLevel"] == DBNull.Value)
                    return 0;
                else
                    return Convert.ToInt32(dataRow["AuthorityLevel"]);
            }
        }
        public int AuthorityLevelValue
        {
            set { dataRow["AuthorityLevelValue"] = value; }
            get
            {
                if (dataRow["AuthorityLevelValue"] == DBNull.Value)
                    return 0;
                else
                    return Convert.ToInt32(dataRow["AuthorityLevelValue"]);
            }
        }
        public int TypeID
        {
            set { dataRow["TypeID"] = value; }
            get
            {
                if (dataRow["TypeID"] == DBNull.Value)
                    return 0;
                else
                    return Convert.ToInt32(dataRow["TypeID"]);
            }
        }
        public int CategoryID
        {
            set { dataRow["CategoryID"] = value; }
            get
            {
                if (dataRow["CategoryID"] == DBNull.Value)
                    return 0;
                else 
                    return Convert.ToInt32(dataRow["CategoryID"]);
            }
        }
        public int StatusID
        {
            get
            {
                if (dataRow["StatusID"] == DBNull.Value)
                    return 0;
                else
                    return Convert.ToInt32(dataRow["StatusID"]);
            }
            set { dataRow["StatusID"] = value; }
        }

        public int CreatedBy
        {
            set { dataRow["CreatedBy"] = value; }
            get
            {
                if (dataRow["CreatedBy"]==DBNull.Value)
                    return -1; 
                else
                    return Convert.ToInt32(dataRow["CreatedBy"]); 
            }
        }

        public DateTime Created
        {
            get { return Convert.ToDateTime(dataRow["Created"]); }
        }

        public bool Completed
        {
            set { dataRow["Completed"] = value; }
            get
            {
                if (dataRow["Completed"] == DBNull.Value)
                    return false;
                else 
                    return Convert.ToBoolean(dataRow["Completed"]); 
            }
        }
        public DateTime? ScheduleDate
        {
            set
            {
                if(value == null)
                {
                    dataRow["ScheduleDate"] = DBNull.Value;
                }
                else
                {
                    dataRow["ScheduleDate"] = value;
                }
            }
            get
            {
                if (dataRow["ScheduleDate"] == DBNull.Value)
//                    return DateTime.MinValue; 
                    return null; 
                else
                    return Convert.ToDateTime(dataRow["ScheduleDate"]); 
            }
        }

        public DateTime? NextFollowUpDate
        {
            set
            {
                if(value == null)
                {
                    dataRow["NextFollowUpDate"] = DBNull.Value;
                }
                else
                {
                    dataRow["NextFollowUpDate"] = value;
                }
            }
            get
            {
                if (dataRow["NextFollowUpDate"] == DBNull.Value)
                    return null; 
                else
                    return Convert.ToDateTime(dataRow["NextFollowUpDate"]); 
            }
        }

        public int RecurrenceID
        {
            set { dataRow["RecurrenceID"] = value; }
            get
            {
                if (dataRow["RecurrenceID"] == DBNull.Value)
                    return 0;
                else
                    return Convert.ToInt32(dataRow["RecurrenceID"]); }
        }
        public bool CreditApproved
        {
            set { dataRow["CreditApproved"] = value; }
            get
            {
                if (dataRow["CreditApproved"] == DBNull.Value)
                    return false;
                else
                    return Convert.ToBoolean(dataRow["CreditApproved"]);
            }
        }
        public bool PropertyApproved
        {
            set { dataRow["PropertyApproved"] = value; }
            get
            {
                if (dataRow["PropertyApproved"] == DBNull.Value)
                    return false;
                else
                    return Convert.ToBoolean(dataRow["PropertyApproved"]);
            }
        }
        public int AuthorityLevelRoleId
        {
            set { dataRow["AuthorityLevelRoleId"] = value; }
            get
            {
                if (dataRow["AuthorityLevelRoleId"] == DBNull.Value)
                    return 0;
                else
                    return Convert.ToInt32(dataRow["AuthorityLevelRoleId"]);
            }
        }
        #endregion

        public Condition(int id)
        {
            ID = id;
            DataTable dt = db.GetDataTable("GetConditionByID", ID);
            if (dt.Rows.Count > 0)
            {
                dataRow = dt.Rows[0];
                ID = Convert.ToInt32(dataRow["ID"]);
            }
            else
                dataRow = dt.NewRow();
        }

        public void Save()
        {
            int res = db.ExecuteScalarInt("SaveConditionOnly",
                                        ID,
                                        MortgageID,
                                        Title,
                                        Description,
                                        AuthorityLevelRoleId,
                                        TypeID == 0 ? 1 : TypeID,
                                        CategoryID==0? 1:CategoryID,
                                        StatusID,
                                        CreatedBy,
                                        Completed);

            if (res <= 0)
                throw new ConditionException("Condition was not updated succesfully");

            ID = (ID <= 0) ? res : ID;
        }

        public void SaveFollowUpDetails()
        {
            int res = db.ExecuteScalarInt("SaveConditionFollowUpDetails",
                                        ID,
                                        ScheduleDate ?? (object)DBNull.Value,
                                        RecurrenceID);
            if (res <= 0)
                throw new ConditionException("Followup details was not updated succesfully");
        }
        #region Static methods
        public static int SaveNote(int? conditionid, int mortgageid, string note, int userid)
        {
            return (int)db.ExecuteScalar("SaveConditionsNote", conditionid ?? (object)DBNull.Value, mortgageid, note, userid);
        }
        public static DataView GetConditionsQuickList(int MortgageID)
        {
            return db.GetDataView("GetConditionsQuickList", MortgageID);
        }

        public static DataView GetConditionsList(int MortgageID, int type)
        {
            return db.GetDataView("GetConditionsList", MortgageID, type);
        }

        public static DataView GetStatusList()
        {
            return db.GetDataView("GetConditionStatusList");
        }

        public static DataView GetCategoryList()
        {
            return db.GetDataView("GetConditionCategoryList");
        }

        public static DataView GetTypeList()
        {
            return db.GetDataView("GetConditionTypeList");
        }
        public static DataView GetRecurrenceList()
        {
            return db.GetDataView("GetTaskRecurrenceList");
        }
        public static bool CheckConditionsSatisfied(int category, int motgageid)
        {
            bool res;
            try
            {
                int ret = db.ExecuteScalarInt("CheckConditionsSatisfied", category, motgageid);
                res = (ret == 1);
            }
            catch
            {
                res = false;
            }
            return res;
        }
        public static void UpdatePropertyApproved(int conditionid, bool approved)
        {
            db.Execute("UpdatePropertyApproved", conditionid, approved);
        }
        public static void UpdateCreditApproved(int conditionid, bool approved)
        {
            db.Execute("UpdateCreditApproved", conditionid, approved);
        }

        #endregion
    }
}
