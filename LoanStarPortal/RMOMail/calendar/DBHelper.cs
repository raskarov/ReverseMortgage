using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.Odbc;
using LitJson;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Globalization;
using WebMailPro;
using System.Text.RegularExpressions;
using System.Collections;

namespace Calendar_NET
{
	public class DBHelper
	{
        protected WebmailSettings wmSettings;
        private SupportedDatabase _dbType;
        CalendarDBWorker eng = null;
        protected WebmailResourceManager _resMan = null;

		public DBHelper()
		{
            wmSettings = new WebmailSettings().CreateInstance();
            _dbType = wmSettings.DbType;
            eng = new CalendarDBWorker(wmSettings);
            _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
		}

		#region Business Logic Layer Selection Methods
		
        public string SelectSettingsByUserId(string user_id, bool forModify)
		{
            Log.WriteLine("SelectSettingsByUserId", string.Format(">>>IN>>: (user_id={0}, forModify={1})", user_id, forModify));
            string sqlCmd = string.Format(@"SELECT timeformat, dateformat, showweekends, workdaystarts, workdayends, showworkday, weekstartson, defaulttab, country, timezone, alltimezones FROM {0}" + 
                Constants.TablesNames.acal_users_data + " WHERE user_id=" + user_id, 
                wmSettings.DbPrefix);
            Log.WriteLine("SelectSettingsByUserId", string.Format("sqlCmd={0}", sqlCmd));
            DataTable dt = eng.ExecuteQuery(sqlCmd);
            string res = GetJsonSubCollection(dt, 0);
            Log.WriteLine("SelectSettingsByUserId", string.Format("<<<OUT<<<: {0}", res));
            return res;
		}
		
        public string SelectSettingsBySettingsId(string settings_id, bool forModify)
		{
            Log.WriteLine("SelectSettingsBySettingsId", string.Format(">>>IN>>: (settings_id={0}, forModify={1})", settings_id, forModify));
            string sqlCmd = string.Format(@"SELECT timeformat, dateformat, showweekends, workdaystarts, workdayends, showworkday, weekstartson, defaulttab, country, timezone, alltimezones FROM {0}" + 
                Constants.TablesNames.acal_users_data+" WHERE settings_id=" + settings_id, 
                wmSettings.DbPrefix);
            Log.WriteLine("SelectSettingsBySettingsId", string.Format("sqlCmd={0}", sqlCmd));
            DataTable dt = eng.ExecuteQuery(sqlCmd);

            string res = GetJsonSubCollection(dt, 0);
            Log.WriteLine("SelectSettingsBySettingsId", string.Format("<<<OUT<<<: {0}", res));
            return res;
        }
		
        public string SelectEvent(string event_id, bool forModify)
		{
            Log.WriteLine("SelectEvent", string.Format(">>>IN>>: (event_id={0}, forModify={1})", event_id, forModify));
            string fields = "event_id, calendar_id, event_timefrom, event_timetill, event_allday, event_name, event_text, event_priority";
            string sqlCmd = string.Format(@"SELECT {1} FROM {0}" + Constants.TablesNames.acal_events + 
                " WHERE event_id=" + event_id,
                wmSettings.DbPrefix, fields);
            Log.WriteLine("SelectEvent", string.Format("sqlCmd={0}", sqlCmd));
            DataTable dt = eng.ExecuteQuery(sqlCmd);

            string res;
            if (forModify)
                res = GetJsonSubCollection(dt, 0);
			else
                res = GetJsonCollectionFromDataTable(dt, "event_id");
            Log.WriteLine("SelectEvent", string.Format("<<<OUT<<<: {0}", res));
         
            return res;
        }

        public string SelectEventsByPeriod(string from, string till, string user_id)
		{
            string res = "[]";
            Log.WriteLine("SelectEventsByPeriod", string.Format(">>>IN>>: (from={0}, till={1}, user_id={2})", from, till, user_id));
            if (Convert.ToInt32(from) < Convert.ToInt32(till))
            {
                string pFrom = ParseDate(from, null).ToString();
                string pTill = ParseDate(till, null).ToString();

                string sqlCmd = string.Format(@"SELECT * FROM {0}" + Constants.TablesNames.acal_events +
                        " WHERE ((event_timefrom BETWEEN @SEP@" + pFrom + "@SEP@ AND @SEP@" + pTill + "@SEP@)" +
                        " OR (event_timetill BETWEEN @SEP@" + pFrom + "@SEP@ AND @SEP@" + pTill + "@SEP@)" +
                        " OR ((event_timefrom <= @SEP@" + pFrom + "@SEP@ ) AND (event_timetill >= @SEP@" + pTill + "@SEP@ )))" +
                        " AND (calendar_id IN (SELECT calendar_id FROM {0}" + Constants.TablesNames.acal_calendars + " WHERE user_id=" + user_id + "))",
                        wmSettings.DbPrefix);

                string separator = "'";
                if (_dbType == SupportedDatabase.MsAccess)
                    separator = "#";
                sqlCmd = sqlCmd.Replace("@SEP@", separator);

                Log.WriteLine("SelectEventsByPeriod", string.Format("sqlCmd={0}", sqlCmd));

                DataTable dt = eng.ExecuteQuery(sqlCmd);

                res = GetJsonCollectionFromDataTable(dt, "event_id");
            }
            Log.WriteLine("SelectEventsByPeriod", string.Format("<<<OUT<<<: {0}", res));
            return res;
		}
		
        public string SelectCalendarsByUserId(string user_id)
		{
            Log.WriteLine("SelectCalendarsByUserId", string.Format(">>>IN>>: (user_id={0})", user_id));
            string fields = "calendar_id, calendar_name, calendar_description, calendar_color, calendar_active";
			string sqlCmd = string.Format(@"SELECT {1} FROM {0}" + Constants.TablesNames.acal_calendars +
                " WHERE user_id=" + user_id,
                wmSettings.DbPrefix, fields);
            Log.WriteLine("SelectCalendarsByUserId", string.Format("sqlCmd={0}", sqlCmd));

            DataTable dt = eng.ExecuteQuery(sqlCmd);
            string res = GetJsonCollectionFromDataTable(dt, "calendar_id");
            Log.WriteLine("SelectCalendarsByUserId", string.Format("<<<OUT<<<: {0}", res));
            return res;
        }

        public DataTable SelectCalendarsDTByUserId(string user_id)
        {
            Log.WriteLine("SelectCalendarsDTByUserId", string.Format(">>>IN>>: (user_id={0})", user_id));
            string fields = "calendar_id, calendar_name, calendar_description, calendar_color, calendar_active";
            string sqlCmd = string.Format(@"SELECT {1} FROM {0}" + Constants.TablesNames.acal_calendars + 
                " WHERE user_id=" + user_id, 
                wmSettings.DbPrefix, fields);
            Log.WriteLine("SelectCalendarsDTByUserId", string.Format("sqlCmd={0}", sqlCmd));
            return eng.ExecuteQuery(sqlCmd);
        }

        public bool isUserCalendar(string user_id, string calendar_id)
        {
            Log.WriteLine("isUserCalendar", string.Format(">>>IN>>: (user_id={0}, calendar_id={1})", user_id, calendar_id));
            string sqlCmd = string.Format(@"SELECT * FROM {0}" + Constants.TablesNames.acal_calendars + 
                " WHERE user_id=" + user_id + " AND calendar_id=" + calendar_id,
                wmSettings.DbPrefix);
            Log.WriteLine("isUserCalendar", string.Format("sqlCmd={0}", sqlCmd));
            DataTable dt = eng.ExecuteQuery(sqlCmd);
            bool res;
            if (dt.Rows.Count > 0)
                res = true;
            else
                res = false;
            Log.WriteLine("isUserCalendar", string.Format("<<<OUT<<<: {0}", res));
            return res;
        }

        public string SelectCalendarsByCalId(string calendar_id, bool forModify)
		{
            Log.WriteLine("SelectCalendarsByCalId", string.Format(">>>IN>>: (calendar_id={0}, forModify={1})", calendar_id, forModify));
            string fields = "calendar_id, calendar_name, calendar_description, calendar_color, calendar_active";
            string sqlCmd = string.Format(@"SELECT {1} FROM {0}" + Constants.TablesNames.acal_calendars +
                " WHERE calendar_id=" + calendar_id, 
                wmSettings.DbPrefix, fields);
            Log.WriteLine("SelectCalendarsByCalId", string.Format("sqlCmd={0}", sqlCmd));
            DataTable dt = eng.ExecuteQuery(sqlCmd);
            
            string res;
			if (forModify)
				res = GetJsonSubCollection(dt, 0);
			else
                res = GetJsonCollectionFromDataTable(dt, "calendar_id");
            Log.WriteLine("SelectCalendarsByCalId", string.Format("<<<OUT<<<: {0}", res));
            return res;
		}
		
        public string SelectCalendarsByCalId2(string calendar_id)
		{
            Log.WriteLine("SelectCalendarsByCalId2", string.Format(">>>IN>>: (calendar_id={0})", calendar_id));
            string fields = "calendar_id, calendar_name, calendar_description, calendar_color, calendar_active";
            string sqlCmd = string.Format(@"SELECT {1} FROM {0}" + Constants.TablesNames.acal_calendars +
                " WHERE calendar_id=" + calendar_id, 
                wmSettings.DbPrefix, fields);
            Log.WriteLine("SelectCalendarsByCalId2", string.Format("sqlCmd={0}", sqlCmd));
            DataTable dt = eng.ExecuteQuery(sqlCmd);
            
            foreach (DataRow row in dt.Rows)
                row["calendar_active"] = ConvertBoolToInt(row["calendar_active"].ToString()).ToString();

			string res = GetJsonSubCollection(dt, 0);
            Log.WriteLine("SelectCalendarsByCalId2", string.Format("<<<OUT<<<: {0}", res));
            return res;
		}
		
        public string SelectYearEvents(string year, string user_id)
		{
            Log.WriteLine("SelectYearEvents", string.Format(">>>IN>>: (year={0}, user_id={1})", year, user_id));
            List<string> res = new List<string>();

			string pYear = ParseDate(year, null).ToString();

			string sqlCmd = string.Format(@"SELECT DISTINCT event_timefrom,event_timetill 
					FROM {0}" +Constants.TablesNames.acal_events+ 
					" WHERE (calendar_id IN (SELECT calendar_id FROM {0}"+Constants.TablesNames.acal_calendars +
                    " WHERE user_id=" + user_id + "))" +
						"AND (YEAR(event_timefrom)=YEAR('" + pYear + "')) ORDER BY event_timefrom ASC", 
                        wmSettings.DbPrefix);
            Log.WriteLine("SelectYearEvents", string.Format("sqlCmd={0}", sqlCmd));
            DataTable dt = eng.ExecuteQuery(sqlCmd);

			if (dt != null)
			{
				foreach (DataRow dr in dt.Rows)
				{
					DateTime dateFrom = DateTime.Parse(dr["event_timefrom"].ToString()).Date;
					DateTime dateTill = DateTime.Parse(dr["event_timetill"].ToString()).Date;
					TimeSpan delta = dateTill.Subtract(dateFrom);
					if (delta.Days == 0)
					{
						string strDate = dateFrom.Year.ToString();
						if (dateFrom.Month < 10)
							strDate += "0" + dateFrom.Month.ToString();
						else
							strDate += dateFrom.Month.ToString();

						if (dateFrom.Day < 10)
							strDate += "0" + dateFrom.Day.ToString();
						else
							strDate += dateFrom.Day.ToString();

						if (!res.Contains(strDate))
							res.Add(strDate);
					}
					else
					{
						for (int i = 0; i <= delta.Days; i++)
						{
							DateTime tmpDate = dateFrom.AddDays((double)i);

							string strDate = tmpDate.Year.ToString();
							if (tmpDate.Month < 10)
								strDate += "0" + tmpDate.Month.ToString();
							else
								strDate += tmpDate.Month.ToString();

							if (tmpDate.Day < 10)
								strDate += "0" + tmpDate.Day.ToString();
							else
								strDate += tmpDate.Day.ToString();

							if (!res.Contains(strDate))
								res.Add(strDate);
						}
					}
				}
			}

			string result = "[]";
			if (res.Count > 0)
			{
				result = "[ @CONTENT@ ]";
				string tmp = "";
				foreach (string str in res)
					tmp += "\"" + str + "\",";
				tmp = tmp.Substring(0, tmp.Length - 1);
				result = result.Replace("@CONTENT@", tmp);
			}
            Log.WriteLine("SelectYearEvents", string.Format("<<<OUT<<<: {0}", result));

			return result;
		}
		#endregion

		#region Business Logic Layer Insertion/Update/Delete Methods
		/// <summary>
		/// This function gets query string as argument an drains all requered values from it.
		/// </summary>
		/// <param name="queryString"></param>
		/// <returns></returns>
        public string UpdateEvent(NameValueCollection queryString)
		{
            string result = "[]";
            string event_id = null;
            string calendar_id = null;
            string new_calendar_id = null;
            string name = null;
            string allday = null;
            string text = null;
            string from = null;
            string till = null;
            string time_from = null;
            string time_till = null;
            string priority = null;
            string strFrom = null;
            string strTill = null;

            if (queryString["event_id"] != null)
                event_id = queryString["event_id"];

            if (queryString["calendar_id"] != null)
                calendar_id = queryString["calendar_id"];

            if (queryString["new_calendar_id"] != null)
                new_calendar_id = queryString["new_calendar_id"];

            if (queryString["name"] != null)
                name = GetASCIIString(queryString["name"]);

            if (queryString["text"] != null)
                text = GetASCIIString(queryString["text"]);

            if (queryString["allday"] != null)
                allday = queryString["allday"];

            if (queryString["from"] != null)
                from = queryString["from"];

            if (queryString["till"] != null)
                till = queryString["till"];

            if (queryString["time_from"] != null)
                time_from = queryString["time_from"];

            if (queryString["time_till"] != null)
                time_till = queryString["time_till"];

            if (queryString["priority"] != null)
                priority = queryString["priority"];

            if ((from != null) && (time_from != null) && (from.Length > 0) && (time_from.Length > 0))
            {
                strFrom = ParseDate(from, time_from);
                DateTime fromDate = DateTime.Parse(strFrom);
                strFrom = fromDate.ToString("u").TrimEnd(new char[2] { 'z', 'Z' });
            }

            if ((till != null) && (time_till != null) && (till.Length > 0) && (time_till.Length > 0))
            {
                strTill = ParseDate(till, time_till);
                DateTime tillDate = DateTime.Parse(strTill);
                strTill = tillDate.ToString("u").TrimEnd(new char[2] { 'z', 'Z' });
            }

            if (event_id == "0")
			{
                Log.WriteLine("InsertEvent", string.Format(">>>IN>>: (event_id={0}, calendar_id={1}, strFrom={2}, strTill={3}, allday={4}, name={5}, text={6})", event_id, calendar_id, strFrom, strTill, allday, name, text));
                if ((event_id != null) && (calendar_id != null) && (strFrom != null) && (strTill != null) && (allday != null) && (name != null) && (name.Trim().Length > 0) && (text != null))
                    result = InsertEvent(event_id, calendar_id, strFrom, strTill, allday, name, text);
			}
			else
			{
                Log.WriteLine("UpdateEvent", string.Format(">>>IN>>: (event_id={0}, calendar_id={1}, new_calendar_id={2}, strFrom={3},  strTill={4}, strAllday={5}, strName={6}, strText={7}, strPriority)", event_id, calendar_id, new_calendar_id, strFrom, strTill, allday, name, text, priority));
                if ((event_id != null) && (calendar_id != null))
                    result = UpdateEvent(event_id, calendar_id, new_calendar_id, strFrom, strTill, allday, name, text, priority);
			}
            Log.WriteLine("UpdateEvent", string.Format("<<<OUT<<<: {0}", result));
            return result;
		}
		
        /// <summary>
		/// This function is the same as InsertEvent(string event_id e.t.c) but gets all arguments as an object array
		/// In fact this function is called by InsertEvent(string event_id e.t.c)
		/// NOTE: if you gona use this function you'll need to pass all arguments exactly in the same way as you 
		/// would do for ordinary function.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
        private string InsertEvent(string event_id, string calendar_id, string strFrom, string strTill, string allday, string name, string text)
		{
			string retVal = "[]";
			string sqlCmd = "";
            
            NameValueCollection pars = new NameValueCollection();
			string filedsSet = @"calendar_id,event_timefrom,event_timetill,event_allday,event_name,event_text"; //GetTableFields("acal_events", 1);
			string[] argsNames = null;
			string parNames = GetParametrNames(filedsSet, true, out argsNames);

            sqlCmd = string.Format(@"INSERT INTO {0}" + Constants.TablesNames.acal_events + " (" + filedsSet + ") VALUES (" + parNames + ")", wmSettings.DbPrefix);
            Log.WriteLine("InsertEvent", string.Format("sqlCmd={0}", sqlCmd));

            int i = 0;
            pars.Add(argsNames[i++], calendar_id);
            pars.Add(argsNames[i++], strFrom);
            pars.Add(argsNames[i++], strTill);
            pars.Add(argsNames[i++], allday);
            pars.Add(argsNames[i++], name);
            pars.Add(argsNames[i++], text);
            
            int lastId = eng.ExecuteNonQueryAndGetLastID(sqlCmd, pars);
            if (lastId != 0)
                retVal = SelectEvent(lastId.ToString(), true);

            Log.WriteLine("InsertEvent", string.Format("<<<OUT<<<: {0}", retVal));

			return retVal;
		}
		
		private string UpdateEvent(string id, string calendar_id, string new_calendar_id, string timefrom, string timetill,
			string allday, string name, string text, string priority)
		{
            Log.WriteLine("UpdateEvent", string.Format(">>>IN>>: (id={0}, calendar_id={1}, new_calendar_id={2}, timefrom={3}, timetill={4}, allday={5}, name={6}, text={7}, priority={8})", id, calendar_id, new_calendar_id, timefrom, timetill, allday, name, text, priority));
            
            string argsStr = String.Empty;
			if (new_calendar_id != null) argsStr += "calendar_id,";
            if (timefrom != null) argsStr += "event_timefrom,";
            if (timetill != null) argsStr += "event_timetill,";
			if (allday != null) argsStr += "event_allday,";
			if ((name != null) && (name.Trim().Length>0)) argsStr += "event_name,";
			if (text != null) argsStr += "event_text,";
			if (priority != null) argsStr += "event_priority,";
			argsStr = argsStr.TrimEnd(',');
			string[] argsNames = null;
			string qryContent = GetParametrNames(argsStr, false, out argsNames);

			string sqlCmd = string.Format(@"UPDATE {0}" + Constants.TablesNames.acal_events +
				" SET " + qryContent +
				" WHERE event_id=" + id + " AND calendar_id=" + calendar_id,
				wmSettings.DbPrefix);

            Log.WriteLine("UpdateEvent", string.Format("sqlCmd={0}", sqlCmd));

			NameValueCollection pars = new NameValueCollection();
			int i = 0;
            if (new_calendar_id != null) pars.Add(argsNames[i++], new_calendar_id);
            if (timefrom != null) pars.Add(argsNames[i++], timefrom);
			if (timetill != null) pars.Add(argsNames[i++], timetill);
			if (allday != null) pars.Add(argsNames[i++], allday);
			if ((name != null) && (name.Trim().Length>0)) pars.Add(argsNames[i++], name);
			if (text != null) pars.Add(argsNames[i++], text);
			if (priority != null) pars.Add(argsNames[i++], priority);

			string result = null;
			if (eng.ExecuteNonQuery(sqlCmd, pars))
				result = SelectEvent(id, true);

            Log.WriteLine("UpdateEvent", string.Format("<<<OUT<<<: {0}", result));
            return result;
		}

		public string DeleteEvent(string event_id)
		{
            Log.WriteLine("DeleteEvent", string.Format(">>>IN>>: (event_id={0})", event_id));
            string sqlCmd = string.Format(@"DELETE FROM {0}" + Constants.TablesNames.acal_events +
                " WHERE event_id=" + event_id, 
                wmSettings.DbPrefix);
            Log.WriteLine("DeleteEvent", string.Format("sqlCmd={0}", sqlCmd));
            eng.ExecuteNonQuery(sqlCmd, null);

            IDictionary hash = new OrderedDictionary();
            hash.Add("event_id", event_id);
            string res = JsonMapper.ToJson(hash);
            Log.WriteLine("DeleteEvent", string.Format("<<<OUT<<<: {0}", res));
            return res;
		}
		
        private void DeletEventsByCalId(string cal_id)
		{
            Log.WriteLine("DeletEventsByCalId", string.Format(">>>IN>>: (cal_id={0})", cal_id));
            string sqlCmd = string.Format(@"DELETE FROM {0}" + Constants.TablesNames.acal_events + 
                " WHERE calendar_id=" + cal_id, 
                wmSettings.DbPrefix);
            Log.WriteLine("DeletEventsByCalId", string.Format("sqlCmd={0}", sqlCmd));
            eng.ExecuteNonQuery(sqlCmd, null);
		}

		public string UpdateSettings(string user_id, string displayName, string timeFormat, string dateFormat, string showWeekends, string workdayStarts,
			string workdayEnds, string showWorkday, string weekStartsOn, string tab, string country, string timeZone, string allTimeZones)
		{
            Log.WriteLine("UpdateSettings", string.Format(">>>IN>>: (user_id={0}, displayName={1}, timeFormat={2}, dateFormat={3}, showWeekends={4}, workdayStarts={5}, workdayEnds={6}, showWorkday={7}, weekStartsOn={8}, tab={9}, country={10}, timeZone={11}, allTimeZones={12})", 
                user_id, displayName, timeFormat, dateFormat, showWeekends, workdayStarts,
			    workdayEnds, showWorkday, weekStartsOn, tab, country, timeZone, allTimeZones));
            string res = UpdateSettingsHide(user_id, GetASCIIString(displayName), timeFormat, dateFormat,
				showWeekends, workdayStarts, workdayEnds, showWorkday, weekStartsOn, tab,
				country, timeZone, allTimeZones);
            Log.WriteLine("UpdateSettings", string.Format("<<<OUT<<<: {0}", res));
            return res;
		}
		
        private string UpdateSettingsHide(params object[] args)
		{
            string ret = null;
			string[] argNames = null;
			string qryContent = GetParametrNames(GetTableFields("acal_users_data", 2), false, out argNames);
			string sqlCmd = string.Format(@"UPDATE {0}"+Constants.TablesNames.acal_users_data+" SET " + qryContent + " WHERE user_id=" + args[0], wmSettings.DbPrefix);
            Log.WriteLine("UpdateSettingsHide", string.Format("sqlCmd={0}", sqlCmd));

            NameValueCollection pars = new NameValueCollection();
			for (int i = 0; i < argNames.Length; i++) //we're starting from displayname field
				pars.Add(argNames[i], args[i + 2].ToString());

            if (eng.ExecuteNonQuery(sqlCmd, pars))
				ret = SelectSettingsByUserId(args[0].ToString(), true);

			return ret;
		}

		public string UpdateCalendar(string user_id, NameValueCollection queryString)
		{
			string result = null;
            Log.WriteLine("InsertCalendar", string.Format(">>>IN>>: (user_id={0}, queryString='{1}')", user_id, queryString));
            if (queryString.Count == 3)
                result = UpdateCalendarColor(queryString["calendar_id"], queryString["color_id"]);
            else
            {
                string calenadar_name = null;
                if (GetASCIIString(queryString["name"]).Trim().Length > 0)
                    calenadar_name = GetASCIIString(queryString["name"]);

                result = InsertCalendarFull(queryString["calendar_id"], user_id, calenadar_name, GetASCIIString(queryString["content"]), queryString["color_id"], 1);
            }
            Log.WriteLine("InsertCalendar", string.Format("<<<OUT<<<: {0}", result));
            return result;
		}

        private string InsertCalendarFull(string cal_id, string user_id, string calendar_name, string calendar_description, string calendar_color, int calendar_active)
		{
            string sqlCmd = "";
			string result = null;
            NameValueCollection pars = new NameValueCollection();

            if ((cal_id == "0") && (calendar_name.Trim().Length > 0)) //insert
			{
                string argsStr = String.Empty;
                if (user_id != null) argsStr += "user_id,";
                if (calendar_name != null) argsStr += "calendar_name,";
                if (calendar_description != null) argsStr += "calendar_description,";
                if (calendar_color != null) argsStr += "calendar_color,";
                argsStr += "calendar_active";

				string[] argsNames = null;
                string parsNames = GetParametrNames(argsStr, true, out argsNames);

                sqlCmd = string.Format(@"INSERT INTO {0}" + Constants.TablesNames.acal_calendars + " (" + argsStr + ") VALUES (" + parsNames + ")", wmSettings.DbPrefix);
                Log.WriteLine("InsertCalendarFull", string.Format("sqlCmd={0}", sqlCmd));

                int i = 0;
                if (user_id != null) pars.Add(argsNames[i++], user_id.ToString());
                if (calendar_name != null) pars.Add(argsNames[i++], calendar_name.ToString());
                if (calendar_description != null) pars.Add(argsNames[i++], calendar_description.ToString());
                if (calendar_color != null) pars.Add(argsNames[i++], calendar_color.ToString());
                pars.Add(argsNames[i++], calendar_active.ToString());                 

                int lastId = eng.ExecuteNonQueryAndGetLastID(sqlCmd, pars);
				if (lastId > 0)
					result = SelectCalendarsByCalId(lastId.ToString(), true);
			}
			else //Update
			{
                string argsStr = String.Empty;
                if (calendar_name != null) argsStr += "calendar_name,";
                if (calendar_description != null) argsStr += "calendar_description,";
                if (calendar_color != null) argsStr += "calendar_color,";
                argsStr += "calendar_active";

                string[] argsNames = null;
                string qryContent = GetParametrNames(argsStr, false, out argsNames);
                
                sqlCmd = string.Format(@"UPDATE {0}" + Constants.TablesNames.acal_calendars + " SET " + qryContent + " WHERE calendar_id=" + cal_id, wmSettings.DbPrefix);
                Log.WriteLine("InsertCalendarFull", string.Format("sqlCmd={0}", sqlCmd));

                int i = 0;
                if (calendar_name != null) pars.Add(argsNames[i++], calendar_name.ToString());
                if (calendar_description != null) pars.Add(argsNames[i++], calendar_description.ToString());
                if (calendar_color != null) pars.Add(argsNames[i++], calendar_color.ToString());
                pars.Add(argsNames[i++], calendar_active.ToString());                 
                
                if (eng.ExecuteNonQuery(sqlCmd, pars))
                    result = SelectCalendarsByCalId(cal_id, true);
			}

            Log.WriteLine("InsertCalendarFull", string.Format("<<<OUT<<<: {0}", result));
			return result;
		}

        public void InsertDefaultCalendar(int userID)
        {
            System.Collections.Specialized.NameValueCollection _params = new System.Collections.Specialized.NameValueCollection();
            _params.Add("calendar_id", "0");
            _params.Add("name", _resMan.GetString("CalendarDefaultName"));
            _params.Add("content", "");
            _params.Add("color_id", "1");
            UpdateCalendar(userID.ToString(), _params);
        }

        private string UpdateCalendarColor(string cal_id, string color)
		{
			string[] argsNames = null;
            
            string result = null;
			string qryContent = GetParametrNames("calendar_color", false, out argsNames);

            string sqlCmd = string.Format(@"UPDATE {0}" + Constants.TablesNames.acal_calendars + " SET " + qryContent + " WHERE calendar_id=" + cal_id, wmSettings.DbPrefix);
            Log.WriteLine("UpdateCalendarColor", string.Format("sqlCmd={0}", sqlCmd));

            NameValueCollection pars = new NameValueCollection();
            pars.Add(GetParametrPrefix() + "calendar_color", color);

            if (eng.ExecuteNonQuery(sqlCmd, pars))
				result = SelectCalendarsByCalId(cal_id, true);

            Log.WriteLine("UpdateCalendarColor", string.Format("<<<OUT<<<: {0}", result));
            return result;
		}

        public void DeleteCalendarsByUserId(string user_id)
        {
            DataTable dt = SelectCalendarsDTByUserId(user_id);
            string jOColl;
            if (dt != null)
                foreach (DataRow dr in dt.Rows)
                    jOColl = DeleteCalendar(dr["calendar_id"].ToString());
        }

        public string DeleteCalendar(string calendar_id)
		{
            DeletEventsByCalId(calendar_id);
            string res;
			string sqlCmd = string.Format(@"DELETE FROM {0}"+Constants.TablesNames.acal_calendars + 
                " WHERE calendar_id=" + calendar_id, 
                wmSettings.DbPrefix);
            Log.WriteLine("DeleteCalendar", string.Format("sqlCmd={0}", sqlCmd));
            if (!eng.ExecuteNonQuery(sqlCmd, null))
				res = null;
        
            IDictionary hash = new OrderedDictionary();
            hash.Add("calendar_id", calendar_id);
            res = JsonMapper.ToJson(hash);
            Log.WriteLine("DeleteCalendar", string.Format("<<<OUT<<<: {0}", res));
            return res;
        }

        public void InsertUsersData(int user_id, int timeformat, int dateformat, int showweekends, int workdaystarts,
                                    int workdayends, int showworkday, int weekstartson, int defaulttab,
                                    string country, int timezone, int alltimezones)
        {
            InsertUserDataHide(user_id, timeformat, dateformat, showweekends, workdaystarts, workdayends, 
                showworkday, weekstartson, defaulttab, country, timezone, alltimezones);
        }

        private void InsertUserDataHide(params object[] args)
        {
            NameValueCollection pars = new NameValueCollection();

            string[] argsNames = null;
            string filedsSet = "user_id,timeformat,dateformat,showweekends,workdaystarts,workdayends,showworkday,weekstartson,defaulttab,country,timezone,alltimezones";
            string qryContent = GetParametrNames(filedsSet, true, out argsNames);

            String sqlCmd = string.Format(@"INSERT INTO {0}" + Constants.TablesNames.acal_users_data +
                            " ({1}) VALUES ({2})", wmSettings.DbPrefix, filedsSet, qryContent);
            Log.WriteLine("InsertUserDataHide", string.Format("sqlCmd={0}", sqlCmd));

            for (int i = 0; i < args.Length; i++)
                pars.Add(argsNames[i], args[i].ToString());

            eng.ExecuteNonQuery(sqlCmd, pars);
        }

        public bool IsUserDataExists(int ID)
        {
            bool res;
            string sqlCmd = string.Format(@"SELECT user_id FROM {0}" + Constants.TablesNames.acal_users_data + 
                " WHERE user_id=" + ID,
                wmSettings.DbPrefix);
            Log.WriteLine("IsUserDataExists", string.Format("sqlCmd={0}", sqlCmd));
            res = false;
            try
            {
                DataTable dt = eng.ExecuteQuery(sqlCmd);
                if ((dt != null)&&(dt.Rows.Count > 0))
                    res = true;
            }
            catch (Exception error)
            {
                ExecutionLog.WriteException(error);
            }
            Log.WriteLine("IsUserDataExists", string.Format("<<<OUT<<<: {0}", res));
            return res;
        }

        public bool DeleteCalendarUserData(int ID)
        {
            bool res;
            string sqlCmd = string.Format(@"DELETE FROM {0}" + Constants.TablesNames.acal_users_data + 
                " WHERE user_id=" + ID,
                wmSettings.DbPrefix);
            Log.WriteLine("DeleteCalendarUserData", string.Format("sqlCmd={0}", sqlCmd));

            try
            {
                eng.ExecuteNonQuery(sqlCmd, null);
                DeleteCalendarsByUserId(ID.ToString());
                res = true;
            }
            catch (Exception error)
            {
                ExecutionLog.WriteException(error);
            }
            res = false;
            Log.WriteLine("DeleteCalendarUserData", string.Format("<<<OUT<<<: {0}", res));
            return res;
        }

        #endregion

		#region Sql Layer Helper Methods
		private string GetParametrPrefix()
		{
			string ret = "@";

			switch (_dbType)
			{
				case SupportedDatabase.MsSqlServer:
					ret = "@";
					break;
				case SupportedDatabase.MySql:
					ret = "?";
					break;
			}
			return ret;
		}
        
        private string GetLastInsertIdCmd()
		{
            string ret = "";

            switch (_dbType)
            {
                case SupportedDatabase.MsAccess:
                case SupportedDatabase.MsSqlServer:
                    ret = "@@identity";
                    break;
                case SupportedDatabase.MySql:
                    ret = "LAST_INSERT_ID()";
                    break;
            }
            return ret;
		}
        
        private string GetTableFields(string tableName, int ignoreFirstNFields)
		{
            string ret = "";
            string sqlCmd = "";

            tableName = string.Format(@"{0}"+tableName, wmSettings.DbPrefix);
            switch (_dbType)
            {
                case SupportedDatabase.MsAccess:
                    sqlCmd = "SELECT DISTINCT TOP 1 * FROM " + tableName;
                    break;
                case SupportedDatabase.MsSqlServer:
                    sqlCmd = @"SELECT     COLUMN_NAME
                           FROM         INFORMATION_SCHEMA.COLUMNS
                           WHERE     (TABLE_NAME = '" + tableName + @"')
                           ORDER BY ORDINAL_POSITION";
                    break;
                case SupportedDatabase.MySql:
                    sqlCmd = @"SHOW FIELDS FROM `" + tableName + "`;";
                    break;
            }
            Log.WriteLine("GetTableFields", string.Format("sqlCmd={0}", sqlCmd));

            DataTable dt = eng.ExecuteQuery(sqlCmd);
            if (dt != null)
            {
                if (_dbType == SupportedDatabase.MsAccess)
                {
                    for (int i = ignoreFirstNFields; i < dt.Columns.Count; i++)
                        ret += dt.Columns[i].Caption + ",";
                    ret = ret.TrimEnd(new char[1] { ',' });
                }
                else
                {
                    for (int i = ignoreFirstNFields; i < dt.Rows.Count; i++)
                        ret += dt.Rows[i][0].ToString() + ",";
                    ret = ret.TrimEnd(new char[1] { ',' });
                }
            }

            Log.WriteLine("GetTableFields", string.Format("<<<OUT<<<: {0}", ret));
            return ret;
		}
        
        private string GetParametrNames(string filedsSet, bool forInsert, out string[] parsArray)
		{
            string[] names = filedsSet.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            parsArray = new string[names.Length];
            string ret = "";

            for (int i = 0; i < names.Length; i++)
            {
                parsArray[i] = GetParametrPrefix() + names[i];

                if (forInsert)
                {
                    if (wmSettings.UseCustomConnectionString || wmSettings.DbDsn != string.Empty)
                        ret += "?,";
                    else
                    {
                        switch (_dbType)
                        {
                            case SupportedDatabase.MsAccess:
                                ret += "?,";
                                break;
                            case SupportedDatabase.MySql:
                            case SupportedDatabase.MsSqlServer:
                                ret += parsArray[i] + ",";
                                break;
                        }
                    }
                }
                else
                {
                    if (wmSettings.UseCustomConnectionString || wmSettings.DbDsn != string.Empty)
                        ret += names[i] + "=?,";
                    else
                    {
                        switch (_dbType)
                        {
                            case SupportedDatabase.MsAccess:
                                ret += names[i] + "=?,";
                                break;
                            case SupportedDatabase.MsSqlServer:
                            case SupportedDatabase.MySql:
                                ret += names[i] + "=" + parsArray[i] + ",";
                                break;
                        }
                    }
                }
            }
            ret = ret.TrimEnd(',');

            return ret;
		}
		#endregion

		#region Business Logic Layer Helper Methods

        public JsonData GetErrorJsonString(string errorCode, string errorDescription)
        {
            IDictionary error = new OrderedDictionary();

            error.Add("error", "true");
            error.Add("code", errorCode);
            error.Add("description", errorDescription);
            
            return JsonMapper.ToJson(error);
        }
        
        private string GetJsonCollectionFromDataTable(DataTable dt, string pk_field_name)
		{
            IDictionary hash = new OrderedDictionary();
            string jsonText = "[]";
			if (dt != null)
			{
                for (int i = 0; i < dt.Rows.Count; i++)
                    hash.Add(dt.Rows[i][pk_field_name].ToString(), GetJsonSubCollectionHash(dt, i));
                jsonText = JsonMapper.ToJson(hash);
            }
            return jsonText;
		}

        private string GetJsonSubCollection(DataTable dt, int rowIndex)
		{
            string result = JsonMapper.ToJson(GetJsonSubCollectionHash(dt, rowIndex));
            if (result == string.Empty)
                result = "[]";
            return result;
		}

        private IDictionary GetJsonSubCollectionHash(DataTable dt, int rowIndex)
        {
            IDictionary hash = new OrderedDictionary();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                switch (dt.Columns[i].ColumnName)
                {
                    case "event_timefrom":
                    case "event_timetill":
                        hash.Add(dt.Columns[i].ColumnName, DateTime.Parse(dt.Rows[rowIndex][i].ToString()).ToString("u").TrimEnd(new char[2] { 'z', 'Z' }));
                        break;
                    case "calendar_name":
                        hash.Add(dt.Columns[i].ColumnName, GetUnicodeString(dt.Rows[rowIndex][i].ToString()));
                        break;
                    case "displayname":
                    case "event_name":
                    case "event_text":
                    case "calendar_description":
                        hash.Add(dt.Columns[i].ColumnName, GetUnicodeString(dt.Rows[rowIndex][i].ToString()));
                        break;
                    case "event_allday":
                    case "calendar_active":
                            hash.Add(dt.Columns[i].ColumnName, ConvertBoolToInt(dt.Rows[rowIndex][i].ToString()).ToString());
                            break;
                    default:
                        hash.Add(dt.Columns[i].ColumnName, dt.Rows[rowIndex][i].ToString());
                        break;
                }
            }
            return hash;
        }

        private string ParseDate(object inDate, object inTime)
		{
			DateTime timeFrom = new DateTime(
			    int.Parse(inDate.ToString().Substring(0, 4)), //get year
			    int.Parse(inDate.ToString().Substring(4, 2)), //get month
			    int.Parse(inDate.ToString().Substring(6, 2)) //get day
											);
			if (inTime != null)
			{
				string strTime = inTime.ToString();
				string[] titems = strTime.Split(':');

                timeFrom = timeFrom.AddHours(double.Parse(titems[0]));
                if (titems.Length > 1)
				    timeFrom = timeFrom.AddMinutes(double.Parse(titems[1]));
			}
			return timeFrom.ToString("u").TrimEnd(new char[2] { 'z', 'Z' });
		}
		
        private double GetEventLength(string event_id)
		{
            JsonData var = JsonMapper.ToObject(SelectEvent(event_id, true));
            
			string strFrom = var["event_timefrom"].ToString();
			string strTill = var["event_timetill"].ToString();
			DateTime dtStart = DateTime.Parse(strFrom);
			DateTime dtEnd = DateTime.Parse(strTill);

			TimeSpan timeSpan = dtEnd.Subtract(dtStart);
			double result = 0;
			result += timeSpan.Days * 24 * 3600;
			result += timeSpan.Hours * 3600;
			result += timeSpan.Minutes * 60;
			result += timeSpan.Seconds;
			return result;
		}
		
        private DateTime GetEventStartDate(string event_id)
		{
			JsonData var = JsonMapper.ToObject(SelectEvent(event_id, true));
			DateTime dtStart = DateTime.Parse(var["event_timefrom"].ToString());
			return dtStart;
		}
		
        private DateTime GetEventEndDate(string event_id)
		{
			JsonData var = JsonMapper.ToObject(SelectEvent(event_id, true));
			DateTime dtEnd = DateTime.Parse(var["event_timetill"].ToString());
			return dtEnd;
		}

        public static string GetASCIIString(string unicode_string)
        {
            string tmp_String = DecodeHtml(unicode_string);
            string utf_string = ConvertToDBString(tmp_String);//Encoding.UTF8.GetString(utf_bytes);
            return utf_string;
        }

        public static string GetUnicodeString(string ascii_string)
        {
            string ucs_string = ConvertFromDBString(ascii_string);//Encoding.Unicode.GetString(ucs_bytes);
            return EncodeHtml(ucs_string);
        }

        public static string ConvertToDBString(string messageString)
        {
            Encoding dbEncoding = Encoding.UTF8;
            try
            {
                dbEncoding = GetEncodingByCodePage(65001);
            }
            catch { }
            byte[] bytes = dbEncoding.GetBytes(messageString);
            messageString = Encoding.Default.GetString(bytes);
            return messageString;
        }

        public static string ConvertFromDBString(string dbString)
        {
            Encoding dbEncoding = Encoding.UTF8;
            try
            {
                dbEncoding = GetEncodingByCodePage(65001);
            }
            catch { }
            byte[] bytes = Encoding.Default.GetBytes(dbString);
            dbString = dbEncoding.GetString(bytes);
            return dbString;
        }

        public static Encoding GetEncodingByCodePage(int codePage)
        {
            Encoding enc = Encoding.Default;
            try
            {
                enc = Encoding.GetEncoding(codePage);
            }
            catch { }
            return enc;
        }

        public static string EncodeHtml(string s)
        {
            Regex rChar;
            rChar = new Regex("[\x0-\x8\xB-\xC\xE-\x1F]+");
            s = rChar.Replace(s, " ");

            StringBuilder sb = new StringBuilder(s);
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            return sb.ToString();
        }
        
        public static string DecodeHtml(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Replace("&lt;", "<");
            sb.Replace("&gt;", ">");
            sb.Replace("&amp;", "&");
            return sb.ToString();
        }

        public static int ConvertBoolToInt(string varBool)
        {
            if ((varBool == Boolean.TrueString) || (varBool == Boolean.FalseString))
                return Convert.ToInt32(Convert.ToBoolean(varBool));
            else
                return Convert.ToInt32(varBool);
        }

		#endregion
	}
}