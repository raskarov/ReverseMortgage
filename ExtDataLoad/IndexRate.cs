using System;
using BossDev.CommonUtils;


namespace DataLoader
{
    public class IndexRate
    {
        private const string SAVERATES = "SaveProductRates";
        //        private const float RndBase = 0.125f;

        #region fields
        private int productId;
        private DateTime period;
        private double margin;
        private double dailyIndex;
        #endregion

        #region Properties
        public double DailyIndex
        {
            get { return dailyIndex; }
            set { dailyIndex = value; }
        }
        public DateTime Period
        {
            get { return period; }
            set { period = value; }
        }
        public double Margin
        {
            set { margin = value; }
            get { return margin; }
        }
        public int ProductID
        {
            set { productId = value; }
            get { return productId; }
        }
        #endregion

        #region methods
        public static bool SaveProductRates(DatabaseAccess db, IndexRate currentIndex, IndexRate expectedIndex)
        {
            int res;
            if(expectedIndex!=null)
            {
                res = db.ExecuteScalarInt(SAVERATES
                                          , currentIndex.productId
                                          , currentIndex.period
                                          , currentIndex.margin
                                          , currentIndex.dailyIndex
                                          , expectedIndex.dailyIndex
                    );
            }
            else
            {
                res = db.ExecuteScalarInt(SAVERATES
                                          , currentIndex.productId
                                          , currentIndex.period
                                          , currentIndex.margin
                                          , currentIndex.dailyIndex
                                          , DBNull.Value
                    );
            }
            return res>0;
        }
        #endregion
    }
}

#region old version
//public class IndexRate
//{
//    public Dictionary<string, double> rates = new Dictionary<string, double>();
//    public Dictionary<string, double> avgRates = new Dictionary<string, double>();

//    private const float RndBase = 0.125f;

//    private int productid;
//    private DateTime period;
//    private string weekday;
//    private double index;
//    private double dailyrate;
//    private double avedailyrate;
//    private double margin;
//    private double publishedindex;
//    private bool IsExpected;
//    private bool needround = false;
//    private DatabaseAccess DB;

//    public IndexRate(DatabaseAccess db, bool isexpected)
//    {
//        DB = db;
//        IsExpected = isexpected;
//    }

//    #region Properties
//    public bool NeedRound
//    {
//        set { needround = value; }
//        get { return needround; }
//    }

//    public DateTime Period
//    {
//        set { period = value; }
//        get { return period; }
//    }

//    public string Weekday
//    {
//        get { return weekday; }
//    }

//    public double Index
//    {
//        set { index = value; }
//        get { return index; }
//    }

//    public double Margin
//    {
//        set { margin = value; }
//        get { return margin; }
//    }

//    public double DailyRate
//    {
//        get { return dailyrate; }
//    }

//    public double RunningAverage
//    {
//        get { return avedailyrate; }
//    }

//    public int ProductID
//    {
//        set { productid = value; }
//        get { return productid; }
//    }
//    public double PublishedIndex
//    {
//        get { return publishedindex; }
//    }
//    #endregion

//    #region Private methods

//    private double GetRate(DateTime date, int ProductID)
//    {
//        string key = date.ToShortDateString() + "_" + ProductID.ToString();
//        if (rates.ContainsKey(key))
//            return rates[key];
//        else
//        {
//            try
//            {
//                DataRow row = DB.GetSingleRow("LoadProductRate", ProductID, date);
//                string FieldName = IsExpected ? "ExpDailyRate" : "DailyRate";
//                double value = Convert.ToDouble(row[FieldName]);
//                rates.Add(key, value);
//                return value;
//            }
//            catch (DatabaseAccess.DatabaseAccessException)
//            {
//                return -1;//not exists in DB
//            }
//        }
//    }
//    private double GetAvgRate(DateTime date, int ProductID)
//    {
//        string key = date.ToShortDateString() + "_" + ProductID.ToString();
//        if (avgRates.ContainsKey(key))
//            return avgRates[key];
//        else
//        {
//            try
//            {
//                DataRow row = DB.GetSingleRow("LoadProductRate", ProductID, date);
//                string FieldName = IsExpected ? "ExpAveDailyRate" : "AveDailyRate";
//                double value = Convert.ToDouble(row[FieldName]);
//                avgRates.Add(key, value);
//                return value;
//            }
//            catch (DatabaseAccess.DatabaseAccessException)
//            {
//                return -1;//not exists in DB
//            }
//        }
//    }

//    private DateTime GetPrevFriday(DateTime date)
//    {
//        DateTime startDate = date;
//        switch (startDate.DayOfWeek)
//        {
//            case DayOfWeek.Monday:
//                startDate = startDate.AddDays(-10);
//                break;
//            case DayOfWeek.Tuesday:
//                startDate = startDate.AddDays(-4);
//                break;
//            case DayOfWeek.Wednesday:
//                startDate = startDate.AddDays(-5);
//                break;
//            case DayOfWeek.Thursday:
//                startDate = startDate.AddDays(-6);
//                break;
//            case DayOfWeek.Friday:
//                startDate = startDate.AddDays(-7);
//                break;
//            case DayOfWeek.Saturday:
//                startDate = startDate.AddDays(-8);
//                break;
//            case DayOfWeek.Sunday:
//                startDate = startDate.AddDays(-9);
//                break;
//        }
//        return startDate;
//    }

//    private double RoundValue(double val, double rnd)
//    {
//        int iVal = (int)((val / rnd) + 0.5f);
//        return ((double)iVal) * rnd;
//    }

//    #endregion

//    public void CalculateAll()
//    {
//        if (productid == 4)//hardcode!!
//            NeedRound = true;
//        dailyrate = index + margin;
//        avedailyrate = dailyrate;
//        weekday = period.DayOfWeek.ToString();
//        int numDays = 1;
//        DateTime curDate = period;
//        while (curDate.DayOfWeek > DayOfWeek.Monday)
//        {
//            curDate = curDate.AddDays(-1);
//            double rateVal = GetRate(curDate, productid);
//            if (rateVal != -1)
//            {
//                avedailyrate += rateVal;
//                numDays++;
//            }
//        }
//        avedailyrate = avedailyrate / numDays;
//        string key = period.ToShortDateString() + "_" + productid.ToString();
//        avgRates.Add(key, avedailyrate);
//        rates.Add(key, dailyrate);

//        publishedindex = GetAvgRate(GetPrevFriday(period), ProductID);
//        if (NeedRound)
//            publishedindex = RoundValue(publishedindex, RndBase);
//    }
//}
#endregion
