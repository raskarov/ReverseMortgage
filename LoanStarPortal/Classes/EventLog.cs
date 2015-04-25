using System;
using System.Data;

namespace LoanStar.Common
{
    public class EventLog : BaseObject
    {
        #region static
        public static object GetLastLimitDate()
        {
            return db.GetSingleRow("select max(Created) from EventLog where EntryType='LendingLimit'")[0];
        }
        public static object GetLastRateDate()
        {
            return db.GetSingleRow("select max(Period) from ProductRate")[0];
        }
        public static DataView GetEventLog()
        {
            return db.GetDataView("EventLogGet", DBNull.Value, DBNull.Value);
        }            
        #endregion
    }
}
