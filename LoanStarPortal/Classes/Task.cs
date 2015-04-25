using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace LoanStar.Common
{
    [Serializable]
    public class Task : BaseObject
    {
        public const string ACTIVE_STATUS = "Active";
        public const string COMPLETED_STATUS = "Completed";

        public const int RecEveryDay = 1;
        public const int RecEveryOtherDay = 2;
        public const int RecOnceWeek = 3;
        public const int RecEveryOtherWeek = 4;
        public const int RecOnceMonth = 5;
        public const int RecNever = 6;

        private const string TASK_TABLE = "Task";
        private const string TASKNOTE_TABLE = "TaskNote";

        #region fields
        [NonSerialized] private SqlDataAdapter sqlDataAdapterNotes;
        [NonSerialized] private SqlConnection sqlConnection;
        [NonSerialized] private SqlCommand sqlCommandNotesGet;
        [NonSerialized] private SqlCommand sqlCommandNotesSet;
        private readonly DataSet data;
        #endregion

        #region  properties
        public DataRow TaskRow
        {
            get
            {
                return data.Tables[TASK_TABLE].Rows[0];
            }
        }

        public int MortgageID
        {
            set { TaskRow["MortgageID"] = value; }
            get { return Convert.ToInt32(TaskRow["MortgageID"]); }
        }

        public int StatusID
        {
            set { TaskRow["StatusID"] = value; }
            get { return Convert.ToInt32(TaskRow["StatusID"]); }
        }

        public string Title
        {
            set { TaskRow["Title"] = value; }
            get { return Convert.ToString(TaskRow["Title"]); }
        }
        public int AssignedTo
        {
            set 
            {
                if (value != -1)
                    TaskRow["AssignedTo"] = value;
                else
                    TaskRow["AssignedTo"] = DBNull.Value;
            }
            get
            {
                if (TaskRow["AssignedTo"]==DBNull.Value)
                    return -1; 
                else
                    return Convert.ToInt32(TaskRow["AssignedTo"]); 
            }
        }

        public string Description
        {
            set { TaskRow["Description"] = value; }
            get { return Convert.ToString(TaskRow["Description"]); }
        }

        public string TaskType
        {
            set { TaskRow["TaskType"] = value; }
            get { return Convert.ToString(TaskRow["TaskType"]); }
        }

        public DateTime ScheduleDate
        {
            set { TaskRow["ScheduleDate"] = value; }
            get
            {
                if (TaskRow["ScheduleDate"]==DBNull.Value)
                    return DateTime.MinValue; 
                else
                    return Convert.ToDateTime(TaskRow["ScheduleDate"]); 
            }
        }

        public int RecurrenceID
        {
            set { TaskRow["RecurrenceID"] = value; }
            get { return Convert.ToInt32(TaskRow["RecurrenceID"]); }
        }

        public int InfoSourceID
        {
            set { TaskRow["InfoSourceID"] = value; }
            get { return Convert.ToInt32(TaskRow["InfoSourceID"]); }
        }

        public int DifficultyID
        {
            set { TaskRow["DifficultyID"] = value; }
            get { return Convert.ToInt32(TaskRow["DifficultyID"]); }
        }

        public string EstimatedAttempts
        {
            set { TaskRow["EstimatedAttempts"] = value; }
            get { return Convert.ToString(TaskRow["EstimatedAttempts"]); }
        }

        public int CreatedBy
        {
            set { TaskRow["CreatedBy"] = value; }
            get
            {
                if (TaskRow["CreatedBy"] == DBNull.Value)
                    return -1;
                else
                    return Convert.ToInt32(TaskRow["CreatedBy"]);
            }
        }

        public DateTime Created
        {
            get { return Convert.ToDateTime(TaskRow["Created"]); }
        }

        public DataTable Notes
        {
            get { return data.Tables[TASKNOTE_TABLE]; }
        }

        public int? ConditionID
        {
            get 
            {
                if (TaskRow["ConditionID"] == DBNull.Value)
                    return null;
                else
                    return Convert.ToInt32(TaskRow["ConditionID"]); 
            }
            set 
            {
                if (value == null || value == -1)
                    TaskRow["ConditionID"] = DBNull.Value;
                else
                    TaskRow["ConditionID"] = value;
            }
        }

        //public DateTime FullyWorked
        //{
        //    set { TaskRow["FullyWorked"] = value; }
        //    get { return Convert.ToDateTime(TaskRow["FullyWorked"]); }
        //}

        #endregion
        /// <summary>
        /// Any method of Task class must throw custom exceptions of this type only
        /// </summary>
        public class TaskException : BaseObjectException
        {
            public TaskException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public TaskException(string message)
                : base(message)
            {
            }

            public TaskException()
            {
            }
        }

        //protected Task(SerializationInfo info, StreamingContext context) : base(info, context)
        //{
        //    InitializeComponent();
        //    sqlConnection.ConnectionString = AppSettings.SqlConnectionString;
        //}


        public Task(): this(0)
	    {
	    }
        public Task(int id)
        {
            data = new DataSet();
            InitializeComponent();
            ID = id;

            DataTable dt = db.GetDataTable("GetTask", ID);

            dt.TableName = "Task";
            data.Tables.Add(dt);

            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
            }
            sqlDataAdapterNotes.SelectCommand.Parameters["@TaskID"].Value = ID;
            sqlDataAdapterNotes.Fill(data, TASKNOTE_TABLE);

        }

        public DataRow CreateNote()
        {
            DataRow newRow = data.Tables[TASKNOTE_TABLE].NewRow();
            newRow["ID"] = -1;
            newRow["TaskID"] = ID;
            newRow["MortgageID"] = MortgageID;
            newRow["Created"] = DateTime.Now;
            newRow["CreatedBy"] = CreatedBy;
            data.Tables[TASKNOTE_TABLE].Rows.Add(newRow);
            return newRow;
        }


        private void InitializeComponent()
        {
            sqlDataAdapterNotes = new SqlDataAdapter();
            sqlCommandNotesSet = new SqlCommand();
            sqlConnection = new SqlConnection();
            sqlCommandNotesGet = new SqlCommand();

            sqlDataAdapterNotes.InsertCommand = sqlCommandNotesSet;
            sqlDataAdapterNotes.SelectCommand = sqlCommandNotesGet;
            sqlDataAdapterNotes.UpdateCommand = sqlCommandNotesSet;

            sqlConnection.ConnectionString = AppSettings.SqlConnectionString;
            // sqlCommandNotesSet
            // 
            sqlCommandNotesSet.CommandText = "[SaveTaskNote]";
            sqlCommandNotesSet.CommandType = CommandType.StoredProcedure;
            sqlCommandNotesSet.Connection = sqlConnection;
            sqlCommandNotesSet.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, "", DataRowVersion.Current, null));
            sqlCommandNotesSet.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 4));
            sqlCommandNotesSet.Parameters.Add(new SqlParameter("@TaskID", SqlDbType.Int, 4));
            sqlCommandNotesSet.Parameters.Add(new SqlParameter("@MortgageID", SqlDbType.Int, 4));
            sqlCommandNotesSet.Parameters.Add(new SqlParameter("@Note", SqlDbType.NVarChar, 1073741823));
            sqlCommandNotesSet.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.Int, 4));

            // sqlCommandNotesGet
            // 
            sqlCommandNotesGet.CommandText = "[GetTaskNoteList]";
            sqlCommandNotesGet.CommandType = CommandType.StoredProcedure;
            sqlCommandNotesGet.Connection = sqlConnection;
            sqlCommandNotesGet.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, "", DataRowVersion.Current, null));
            sqlCommandNotesGet.Parameters.Add(new SqlParameter("@TaskID", SqlDbType.Int, 4));
        }

        public void Save()
        {
            if(sqlDataAdapterNotes==null)
                InitializeComponent();
            int res = db.ExecuteScalarInt("SaveTask",
                                        ID,
                                        MortgageID,
                                        StatusID,
                                        Title,
                                        AssignedTo,
                                        Description,
                                        ScheduleDate,
                                        RecurrenceID,
                                        InfoSourceID,
                                        DifficultyID,
                                        EstimatedAttempts,
                                        TaskRow["ConditionID"],
                                        CreatedBy);
                                        //,FullyWorked);

            if (res <= 0)
                throw new TaskException("Task was not updated succesfully");

            ID = (ID <= 0)?res:ID;

            sqlConnection.Open();
            try
            {
                if (data.Tables[TASKNOTE_TABLE] != null)
                {
                    sqlDataAdapterNotes.RowUpdating += sqlDataAdapterNotes_RowUpdating;
                    sqlDataAdapterNotes.Update(data.Tables[TASKNOTE_TABLE]);
                }
            }
            catch (Exception ex)
            {
                throw new TaskException("Task notes were not updated succesfully: ", ex.InnerException);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public void SaveNotes()
        {
            if (sqlDataAdapterNotes == null)
                InitializeComponent();
            sqlConnection.Open();
            try
            {
                if (data.Tables[TASKNOTE_TABLE] != null)
                {
                    sqlDataAdapterNotes.RowUpdating += sqlDataAdapterNotes_RowUpdating;
                    sqlDataAdapterNotes.Update(data.Tables[TASKNOTE_TABLE]);
                }
            }
            catch (Exception ex)
            {
                throw new TaskException("Task notes were not updated succesfully: ", ex.InnerException);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        void sqlDataAdapterNotes_RowUpdating(object sender, SqlRowUpdatingEventArgs e)
        {
            if (e.StatementType == StatementType.Delete)
            {
                //sqlCommandNoteDelete.Parameters["@ID"].Value = e.Row["ID", DataRowVersion.Original];}
            }
            else
                AssignParameters(sqlCommandNotesSet, e.Row);
        }

        /// <summary>
        /// Handy method to assign parameters to sql commands in derived classes.
        /// Assumes that commands' parameter names
        /// </summary>
        /// <param name="command"></param>
        /// <param name="row"></param>
        protected virtual void AssignParameters(SqlCommand command, DataRow row)
        {
            // Implicit parameter of any SqlCommand we need to skip
            const string PARAM_RETURN_VALUE = "@RETURN_VALUE";

            foreach (SqlParameter param in command.Parameters)
                if (param.ParameterName != PARAM_RETURN_VALUE && param.ParameterName.Length > 1)
                {
                    // Removing first char (@)
                    string columnName = param.ParameterName.Substring(1);
                    if (row.Table.Columns.IndexOf(columnName) != -1)
                        param.Value = row[columnName];
                }
        }

        #region Static methods
        public static DataView GetTaskList(int MortgageID)
        {
            return db.GetDataView("GetTaskList", MortgageID, DBNull.Value);
        }

        public static DataView GetActiveTaskList(int MortgageID)
        {
            return db.GetDataView("GetTaskList", MortgageID, ACTIVE_STATUS);
        }

        public static DataView GetCompleteTaskList(int MortgageID)
        {
            return db.GetDataView("GetTaskList", MortgageID, COMPLETED_STATUS);
        }

        public static DataView GetTaskStatusList()
        {
            return db.GetDataView("GetTaskStatusList");
        }

        public static DataView GetTaskTypeList()
        {
            return db.GetDataView("GetTaskTypeList");
        }

        public static DataView GetDifficultyList()
        {
            return db.GetDataView("GetTaskDifficultyList");
        }

        public static DataView GetInfoSourceList()
        {
            return db.GetDataView("GetTaskInfoSourceList");
        }

        public static DataView GetRecurrenceList()
        {
            return db.GetDataView("GetTaskRecurrenceList");
        }

        public static int SaveNote(int id, int? taskid, int MortgageID, string note, int userid)
        {
            return (int)db.ExecuteScalar("SaveTaskNote", id, taskid ?? (object)DBNull.Value, MortgageID, note, userid);
        }

        public static DataRow GetNote(int id)
        {
            DataTable tblNote = db.GetDataTable("GetTaskNote", id);
            if (tblNote.Rows.Count > 0)
                return tblNote.Rows[0];
            else
                return null;
        }
        #endregion


        #region ISerializable Members

        #endregion
    }
}
