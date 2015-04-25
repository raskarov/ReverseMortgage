using System.Data;
using BossDev.CommonUtils;
using System;

namespace LoanStar.Common
{
    public enum Sex
    {
        Male    = 0,
        Female  = 1
    }

    /// <summary>
    /// Class to get helpers data from db using static methods
    /// </summary>
    public class DataHelpers
    {
        #region constants
        private const string GETSTATENAME = "GetStateName";
        private const string GETREQUIREDFIELDS = "GetRequiredFieldsByCompany";
        private const string GETSTATELIST = "GetStatesList";
        private const string GETSTATELISTUS = "GetStatesListUS";
        private const string GETCOUNTIESFORSTATE = "GetCountiesForState";
        #endregion

        #region Private fields
        private static readonly DatabaseAccess dbAccess = new DatabaseAccess(AppSettings.SqlConnectionString);
        #endregion

        #region Static methods
        public static DataView GetStateListUS()
        {
            return dbAccess.GetDataView(GETSTATELISTUS);
        }
    
        public static DataView GetDataViewByStoredProcedure(string procedureName, int mortgageId)
        {
            return dbAccess.GetDataView(procedureName, mortgageId);
        }
        public static DataView GetDictionary(string tableName)
        {
            return dbAccess.GetDataView("GetDictionaryValues",tableName);
        }
        public static DataTable GetDictionaryTable(string tableName)
        {
            return dbAccess.GetDataTable("GetDictionaryValues", tableName);
        }
        public static DataTable GetDictionaryByProcedure(string procedureName)
        {
            return dbAccess.GetDataTable(procedureName);
        }

        public static DataView GetStateCheckBoxList()
        {
            return dbAccess.GetDataView("GetStateCheckboxList");
        }
        public static DataView GetStateList()
        {
            return dbAccess.GetDataView("GetStateList");
        }
        public static DataView GetCountiesForState(int stateId)
        {
            return dbAccess.GetDataView(GETCOUNTIESFORSTATE,stateId);
        }
        public static DataView GetCountyList(int stateID)
        {
            return dbAccess.GetDataView("GetCountyList", stateID);
        }
        public static string GetStateName(int id)
        {
            return dbAccess.ExecuteScalarString(GETSTATENAME,id);
        }

        public static string WrapString(string str, int length)
        {
            if (str.Length > length) str = str.Remove(length);
            return str;
        }
        public static DataView GetRequiredFields(int companyId)
        {
            return dbAccess.GetDataView(GETREQUIREDFIELDS, companyId);
        }

        public static DataView GetLendingLimitList(int StateID)
        {
            return dbAccess.GetDataView("GetLendingLimitList", (StateID==-1)?DBNull.Value:(object)StateID);
        }

        public static bool IsHoliday(int companyId, DateTime date)
        {
            bool res = false;
            DataView dv = Holidays.GetHolidaysByYearAndCompany(date.Year, companyId);
            if(dv.Table.Rows.Count>0)
            {
                foreach (DataRowView row in dv)
                {
                    DateTime dt = Convert.ToDateTime(row["HolidayDate"]);

                    TimeSpan ts = dt.Subtract(date);
                    if (ts.Days == 0 || dt.DayOfWeek == DayOfWeek.Sunday || dt.DayOfWeek == DayOfWeek.Saturday) res = true;
                }
            }
            return res;
        }

        public static DateTime NextDayAfterHoliday(int companyId, DateTime date)
        {
            DateTime res = date;
            DataView dv = Holidays.GetHolidaysByYearAndCompany(date.Year, companyId);
            if (dv.Table.Rows.Count > 0)
            {
                foreach (DataRowView row in dv)
                {
                    DateTime dt = Convert.ToDateTime(row["HolidayDate"]);
                    TimeSpan ts = dt.Subtract(date);
                    if (ts.Days != 0 && dt.DayOfWeek != DayOfWeek.Sunday && dt.DayOfWeek != DayOfWeek.Saturday)
                        return res;
                    else
                        res = NextDayAfterHoliday(companyId, date.AddDays(1));
                }
            }
            return res;
        }

        public static string RemainderToFraction (double remainder)
        {
            string ret = "";
            remainder = remainder % 1;
            switch ((int)(remainder*1000))
            {
                case 125:
                    ret = "1/8";
                    break;
                case 250:
                    ret = "1/4";
                    break;
                case 375:
                    ret = "3/8";
                    break;
                case 500:
                    ret = "1/2";
                    break;
                case 625:
                    ret = "5/8";
                    break;
                case 750:
                    ret = "3/4";
                    break;
                case 875:
                    ret = "7/8";
                    break;
            }
            return ret;
        }
        public static int GetRandomNumber(int maxvalue)
        {

            System.Security.Cryptography.RandomNumberGenerator random =
                System.Security.Cryptography.RandomNumberGenerator.Create();

            byte[] r = new byte[1];
            random.GetBytes(r);
            double val = (double)r[0] / byte.MaxValue;
            int i = (int)Math.Round(val * maxvalue, 0);
            return i;

        }
        #endregion

    }
}