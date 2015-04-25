using System;
using System.Data;

namespace LoanStar.Common
{
    public class Event : BaseObject
    {
        #region constants
        
        #region sp names
        private const string GETTYPELIST = "GetEventTypeList";
        private const string SAVEEVENT = "SaveEvent";
        #endregion

        #region db field names
        public const string NAMEFIELDNAME = "name";
        public const string IDFIELDNAME = "id";
        #endregion

        #endregion

        #region fields
        private readonly DataRow dataRow;
        private int mortgageID = 0;
        private int typeID = 0;
        private string description = String.Empty;
        private string eventType = String.Empty;
        private DateTime created;
        //private DateTime day;
        //private int companyId = Constants.LOANSTARCOMPANYID;
        #endregion

        #region  properties
        public int TypeID
        {
            get { return typeID; }
            set { typeID = value; }
        }
        public int MortgageID
        {
            get { return mortgageID; }
            set { mortgageID = value; }
        }
        public int TypeId
        {
            get { return typeID; }
            set { typeID = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string EventType
        {
            get { return eventType; }
            set { eventType = value; }
        }
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        #endregion


        public Event(): this(0)
	    {
	    }
        public Event(int id)
        {
            ID = id;
            if (ID <= 0)
                return;

            DataTable dt = db.GetDataTable("GetEvent", ID);
            if (dt.Rows.Count > 0)
            {
                LoadFromDataRow(dt.Rows[0]);
                ID = Convert.ToInt32(dataRow["ID"]);
            }
            else
                dataRow = dt.NewRow();
        }
        #region Methods
        private void LoadFromDataRow(DataRow dr)
        {
            mortgageID = Convert.ToInt32(dr["MortgageID"]);
            typeID = Convert.ToInt32(dr["TypeId"]);
            description = dr["Description"].ToString();
            eventType = dr["EventType"].ToString();
            if (dr["Created"] != DBNull.Value)
            {
                created = Convert.ToDateTime(dr["Created"]);
            }
        }

        #region Public methods
        public int Save()
        {
            int res = db.ExecuteScalarInt(SAVEEVENT, ID, mortgageID, typeID, description);
            if (res > 0)
            {
                ID = res;
            }
            return res;
        }
        #endregion

        #region Static methods
        public static DataView GetTypeList()
        {
            return db.GetDataView(GETTYPELIST);
        }
        public static void Save(int MortgageId, int TypeId, string Description)
        {
            db.Execute(SAVEEVENT, 0, MortgageId, TypeId, Description);
        }
        public static void UpdateMPEventRules(int mortgageid, string ruleList)
        {
            db.Execute("UpdateMPEventRules", mortgageid, ruleList);
        }
        #endregion
        #endregion
    }
}
