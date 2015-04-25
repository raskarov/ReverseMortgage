using System;
using System.Data;
using System.Collections;

namespace LoanStar.Common
{
    public class LeadCampaign :BaseObject
    {
        #region constants
        private const string GETLEADCAMPAIGNBYID = "GetLeadCampaignById";
        private const string GETLEADCAMPAIGNFORMORTGAGEBYID = "GetLeadCampaignForMortgageById";
        private const string GETCAMPAIGNLISTFORCOMPANY = "GetCompanyCampaignListForGrid";
        private const string GETCOMPANYACTIVECAMPAIGNS = "GetCompanyActiveCampaigns";
        private const string GETCOMPANYACTIVECAMPAIGNSFORMORTGAGE = "GetCompanyActiveCampaignsForMortgage";
        private const string GETSTARTDATELIST = "GetLeadCampaignStartDateList";
        private const string GETRECURRENCELIST = "GetLeadCampaignRecurrenceList";
        private const string SAVE = "SaveLeadCampaign";
        private const string DELETE = "DeleteLeadCampaign";
        private const string GETUSERDEFINEDDATES = "GetCampaignUserDefinedDates";
        private const string SAVEDATE = "SaveCampaignDate";
        private const string DELETEDATE = "DeleteCampaignDate";
        private const string SAVENOTE = "SaveCampaignNote";
        public const int FOLLOWUPUSERDEFINEDID = 1;
        public const int FOLLOWUPRECURRINGSHEDID = 2;
        public const int LEADCREATEDATEID = 1;
        public const int BIRTHDAYDATEID = 2;
        public const int CLOSINGDATEID = 3;
        public const int LEADTYPEBOTH = 1;
        public const int LEADTYPEMANAGED = 2;
        public const int LEADTYPEUSERCREATED = 3;
        public const int ONCEPERDAYRECURRANCEID = 1;
        public const int ONCEPERSECONDDAYRECURRANCEID = 2;
        public const int ONCEPERTHIRDDAYRECURRANCEID = 3;
        public const int ONCEPERFOURTHDAYRECURRANCEID = 4;
        public const int ONCEPERWEEKDAYRECURRANCEID = 5;
        public const int ONCEPEROTHERWEEKDAYRECURRANCEID = 6;
        public const int ONCEPERMONTHDAYRECURRANCEID = 7;
        public const int ONCEPERYEARDAYRECURRANCEID = 8;
        public const int GENERICNOTEACTIONID = 1;
        public const int NEXTFOLLOWUPDATEACTIONID = 2;
        public const int COMPLETEDACTIONID = 3;
        #endregion

        #region fileds
        private int companyId;
        private bool isOn;
        private string title;
        private string detail;
        private int startDateId=1;
        private bool isOnlyWorkingDayAllowed;
        private int leadsAllowed=1;
        private int recurrenceId = 1;
        private int startDayOffset = 0;
        private string startdate = String.Empty;
        private DateTime? specificStartDate;
        private bool isRecalculationNeeded = false;
        private int mortgageId = -1;
        private DateTime? nextFollowupDate;
        private bool isCompleted = false;
        #endregion

        #region properties
        public bool IsCompleted
        {
            get { return isCompleted; }
            set { isCompleted = value; }
        }
        public DateTime? NextfollowupDate
        {
            get { return nextFollowupDate; }
            set { nextFollowupDate = value; }
        }
        public bool IsRecalculationNeeded
        {
            get { return isRecalculationNeeded; }
            set { isRecalculationNeeded = value; }
        }
        public int StartDayOffset
        {
            get { return startDayOffset; }
            set { startDayOffset = value; }
            
        }
        public string Startdate
        {
            get { return startdate; }
        }
        public int RecurrenceId
        {
            get { return recurrenceId; }
            set { recurrenceId = value; }
        }
        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }
        public bool IsOn
        {
            get { return isOn;}
            set { isOn = value; }
        }
        public string Title
        {
            get { return title;}
            set { title = value; }
        }
        public string Detail
        {
            get { return detail;}
            set { detail = value; }
        }
        public int StartDateId
        {
            get { return startDateId;}
            set { startDateId = value; }
        }
        public bool IsOnlyWorkingDayAllowed
        {
            get { return isOnlyWorkingDayAllowed; }
            set { isOnlyWorkingDayAllowed = value; }
        }
        public int LeadsAllowed
        {
            get { return leadsAllowed; }
            set { leadsAllowed = value; }
        }
        public DateTime? SpecificStartDate
        {
            get { return specificStartDate; }
            set { specificStartDate=value; }
        }
        #endregion

        #region constructors
        public LeadCampaign(int id)
        {
            ID = id;
            if (ID > 0)
            {
                LoadById();
            }
        }
        public LeadCampaign(int id, int mortgageId_)
        {
            ID = id;
            mortgageId = mortgageId_;
            if (ID > 0)
            {
                LoadForMortgageById();
            }
        }
        //public LeadCampaign(int id, int companyId_)
        //{
        //    companyId = companyId_;
        //    ID = id;
        //    if(ID>0)
        //    {
        //        LoadById();
        //    }
        //}
        public LeadCampaign(DataRowView dr)
        {
            LoadFromDataRow(dr);
        }
        #endregion

        #region methods
        public ArrayList GetScheduleDates(MortgageProfile mp, int year, int month)
        {
            ArrayList res = new ArrayList();
            if (nextFollowupDate!=null&&!isCompleted)
            {
                DateTime? dt = GetStartDate(mp,true);
                if (dt != null)
                {
                    DateTime dt1 = Holidays.RemoveTime((DateTime)dt);
                    if (isOnlyWorkingDayAllowed)
                    {
                        dt1 = Holidays.GetWorkDate(dt1, companyId);
                    }
                    DateTime dtStart = Holidays.RemoveTime(Holidays.GetFirstMonthDay(year, month));
                    DateTime dtEnd = dtStart.AddMonths(1);
                    while (dt1 < dtEnd)
                    {
                        if (dt1>=(DateTime)nextFollowupDate && dt1.Month == month && dt1.Year == year)
                        {
                            res.Add(dt1);
                        }
                        dt1 = GetNextDate(dt1);
                    }
                }
            }
            return res;
        }
        public string GetDayOffset()
        {
            string res = "";
            if(startDayOffset!=0)
            {
                if(startDayOffset>0)
                {
                    res += "+";
                }
                else
                {
                    res += "-";
                }
                res += Math.Abs(startDayOffset).ToString() + " day(s)";
            }
            return res;
        }

        public DateTime? GetStartDate(MortgageProfile mp, bool useOffset)
        {
            DateTime? dt=null;
            switch (startDateId)
            {
                case LEADCREATEDATEID :
                    dt = mp.Created;
                    break;
                case BIRTHDAYDATEID:
                    if (mp.YoungestBorrower.DateOfBirth!=null)
                    {
                        dt = Holidays.GetSameDateInYear(DateTime.Now.Year, (DateTime)mp.YoungestBorrower.DateOfBirth);
                    }
                    break;
                case CLOSINGDATEID:
                    dt = mp.MortgageInfo.ClosingDate;
                    break;
            }
            if (useOffset && dt != null && startDayOffset != 0)
            {
                dt = ((DateTime)dt).AddDays(startDayOffset);
            }
            //if (dt != null && startDayOffset!=0)
            //{
            //    dt = ((DateTime) dt).AddDays(startDayOffset);

            //}
            return dt;
        }

        public int Save()
        {
            int res = db.ExecuteScalarInt(SAVE
                                          , ID
                                          , companyId
                                          , isOn
                                          , title
                                          , detail
                                          , startDateId
                                          , startDayOffset
                                          , isOnlyWorkingDayAllowed
                                          , leadsAllowed
                                          , recurrenceId
                                          , isRecalculationNeeded
                );
            if(res>0)
            {
                ID = res;
            }
            return res;
        }
        public int SaveNote(int mortgageId_, int userId, int actionId, string note)
        {
            if (nextFollowupDate != null)
            {
                nextFollowupDate = GetNextDateInFuture((DateTime)nextFollowupDate);
            }
            if(actionId==COMPLETEDACTIONID)
            {
                isCompleted = true;
            }
            return db.ExecuteScalarInt(SAVENOTE, ID, mortgageId_, note, userId, actionId, nextFollowupDate);
        }

        #region private
        private DateTime GetNextDateInFuture(DateTime dt)
        {
            DateTime res = dt;
            while (true)
            {
                res = GetNextDate(res);
                if (res > DateTime.Now) break;
            }
            return res;
        }

        private DateTime GetNextDate(DateTime dt)
        {
            DateTime res = DateTime.MinValue;
            switch (recurrenceId)
            {
                case ONCEPERDAYRECURRANCEID :
                    res = dt.AddDays(1);
                    break;
                case ONCEPERSECONDDAYRECURRANCEID:
                    res = dt.AddDays(2);
                    break;
                case ONCEPERTHIRDDAYRECURRANCEID:
                    res = dt.AddDays(3);
                    break;
                case ONCEPERFOURTHDAYRECURRANCEID:
                    res = dt.AddDays(4);
                    break;
                case ONCEPERWEEKDAYRECURRANCEID:
                    res = dt.AddDays(7);
                    break;
                case ONCEPEROTHERWEEKDAYRECURRANCEID:
                    res = dt.AddDays(14);
                    break;
                case ONCEPERMONTHDAYRECURRANCEID:
                    res = dt.AddMonths(1);
                    break;
                case ONCEPERYEARDAYRECURRANCEID:
                    res = dt.AddYears(1);
                    break;
            }
            if(isOnlyWorkingDayAllowed)
            {
                res = Holidays.GetWorkDate(res, companyId);
            }
            return res;
        }
        private void LoadForMortgageById()
        {
            DataView dv = db.GetDataView(GETLEADCAMPAIGNFORMORTGAGEBYID, ID, mortgageId);
            if (dv.Count == 1)
            {
                LoadFromDataRow(dv[0]);
            }
            else
            {
                ID = -1;
            }
        }

        private void LoadById()
        {
            DataView dv = db.GetDataView(GETLEADCAMPAIGNBYID, ID);
            if (dv.Count == 1)
            {
                LoadFromDataRow(dv[0]);
            }
            else
            {
                ID = -1;
            }
        }
        private void LoadFromDataRow(DataRowView dr)
        {
            ID = int.Parse(dr["id"].ToString());
            companyId = int.Parse(dr["companyid"].ToString());
            isOn = bool.Parse(dr["ison"].ToString());
            title = dr["title"].ToString();
            detail = dr["detail"].ToString();
            startDateId = int.Parse(dr["startDateId"].ToString());
            recurrenceId = int.Parse(dr["RecurrenceId"].ToString());
            isOnlyWorkingDayAllowed = bool.Parse(dr["isOnlyWorkingDayAllowed"].ToString());
            leadsAllowed = int.Parse(dr["leadsAllowed"].ToString());
            if (dr.Row.Table.Columns.Contains("startdate"))
            {
                startdate = dr["startdate"].ToString();
            }
            if (dr.Row.Table.Columns.Contains("nextfollowupdate"))
            {
                if (dr["nextfollowupdate"]!=DBNull.Value)
                {
                    nextFollowupDate = DateTime.Parse(dr["nextfollowupdate"].ToString());
                }
            }
            if (dr.Row.Table.Columns.Contains("iscompleted"))
            {
                if (dr["iscompleted"] != DBNull.Value)
                {
                    isCompleted = bool.Parse(dr["isCompleted"].ToString());
                }
            }
            startDayOffset = int.Parse(dr["startDayOffset"].ToString());
        }

        #endregion

        #region static
        public static bool DeleteDate(int id)
        {
            return db.ExecuteScalarInt(DELETEDATE, id)==1;
        }

        public static bool SaveDate(int id, int campaignId, int dayoffset)
        {
            return db.ExecuteScalarInt(SAVEDATE, id, campaignId, dayoffset) == 1;
        }

        public static DataView GetUserDefinedDates(int id)
        {
            return db.GetDataView(GETUSERDEFINEDDATES, id);
        }

        public static bool Delete(int id)
        {
            return db.ExecuteScalarInt(DELETE, id) == 1;
        }

        public static DataView GetCampignListForCompany(int companyId)
        {
            return db.GetDataView(GETCAMPAIGNLISTFORCOMPANY, companyId);
        }
        public static DataView GetCompanyActiveCampigns(int companyId)
        {
            return db.GetDataView(GETCOMPANYACTIVECAMPAIGNS, companyId);
        }
        public static DataView GetCompanyActiveCampignsForMortgage(int companyId, int mortgageId)
        {
            return db.GetDataView(GETCOMPANYACTIVECAMPAIGNSFORMORTGAGE , companyId, mortgageId);
        }
        public static DataView GetStartDateList()
        {
            return db.GetDataView(GETSTARTDATELIST);
        }
        public static DataView GetRecurrenceList()
        {
            return db.GetDataView(GETRECURRENCELIST);
        }
        public static  bool IsUsedForStatus(int statusId)
        {
            return statusId == MortgageProfile.MANAGEDLEADSTATUSID || statusId == MortgageProfile.LEADSTATUSID;
        }

        #endregion

        #endregion
    }
}
