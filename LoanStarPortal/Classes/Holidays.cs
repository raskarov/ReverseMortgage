using System;
using System.Data;

namespace LoanStar.Common
{
    public class Holidays : BaseObject
    {
        public class HolidaysException : BaseObjectException
        {
            public HolidaysException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public HolidaysException(string message)
                : base(message)
            {
            }

            public HolidaysException()
            {
            }
        }
        #region constants
        private const string GETHOLIDAYSBYYEARANDCOMPANYID = "GetHolidayByYearAndCompany";
        private const string GETHOLIDAYS = "GetHolidays";
        private const string SAVE = "SaveHoliday";
        private const string DELETE = "DeleteHoliday";
        #endregion

        #region fields
        private string name = String.Empty;
        private DateTime day;
        private int companyId = Constants.LOANSTARCOMPANYID;
        #endregion

        #region properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public DateTime Day
        {
            get { return day; }
            set { day = value; }
        }
        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }
        #endregion

        #region constructor
        public Holidays()
            : this(0)
        { 
        }
        public Holidays(int id)
        {
            ID = id;
        }
        #endregion

        #region methods

        #region instance
        public int Save()
        {
            int res = db.ExecuteScalarInt(SAVE, ID, name, day,companyId);
            if (res > 0)
            {
                ID = res;
            }
            return res;
        }
        public bool Delete()
        {
            return db.ExecuteScalarInt(DELETE, ID) == 1;
        }
        #endregion

        #region static
        public static DateTime GetFirstMonthDay(int year, int month)
        {
            return new DateTime(year,month,1);
        }
        public static DateTime GetSameDateInYear(int year, DateTime dt)
        {
            return new DateTime(year, dt.Month, dt.Day);
        }
        public static int IsDateInMonth(DateTime dt, DateTime firstMonthDay)
        {
            int res;
            DateTime dt1 = RemoveTime(firstMonthDay);
            DateTime dt2 = RemoveTime(dt1.AddMonths(1));
            DateTime dt_ = RemoveTime(dt);
            if(dt_>=dt2)
            {
                res = 1;
            }
            else if (dt_ < dt1)
            {
                res = -1;
            }
            else
            {
                res = 0;
            }
            return res;
        }

        private static DateTime CheckWeekEnd(DateTime dt)
        {
            DateTime res = dt;
            if (dt.DayOfWeek == DayOfWeek.Saturday)
            {
                res = dt.AddDays(2);
            }
            else if (dt.DayOfWeek == DayOfWeek.Sunday)
            {
                res = dt.AddDays(1);
            }
            return res;
        }
        public static DateTime RemoveTime(DateTime dt)
        {
            return new DateTime(dt.Year,dt.Month,dt.Day);
        }
        public static DateTime GetWorkDate(DateTime dt, int companyId)
        {
            DateTime res = RemoveTime(CheckWeekEnd(dt));
            DataView dv = db.GetDataView(GETHOLIDAYS, res, companyId);
            if(dv!=null&&dv.Count>0)
            {
                for(int i=0;i<dv.Count;i++)
                {
                    DateTime d = RemoveTime(DateTime.Parse(dv[i]["holidaydate"].ToString()));
                    if(res<d)
                    {
                        break;
                    }
                    if(res==d)
                    {
                        res = CheckWeekEnd(res.AddDays(1));
                    }
                }
            }
            return res;
        }
        public static DataView GetHolidaysByYearAndCompany(int year, int companyId)
        {
            return db.GetDataView(GETHOLIDAYSBYYEARANDCOMPANYID, year, companyId);
        }
        #endregion

        #endregion
    }
}
