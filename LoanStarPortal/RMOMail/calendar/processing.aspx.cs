using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Xml.XPath;
using WebMailPro;
using LitJson;
using System.Collections;
using System.Collections.Specialized;

namespace Calendar_NET
{
	public partial class _processing : System.Web.UI.Page
	{
        protected WebmailResourceManager _resMan = null;
        protected WebmailSettings sett = new WebmailSettings().CreateInstance();
        
        protected void Page_Load(object sender, EventArgs e)
		{
            Log.WriteLine("Page_Load", "processing.aspx page load");
            JsonData stringForClient = new JsonData();
			DBHelper eng = new DBHelper();
            _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();

            if (Session[Constants.sessionUserID] != null)
            {
                string user_id = Session[Constants.sessionUserID].ToString();

                if (((Request["calendar_id"] != null) && (eng.isUserCalendar(user_id, Request["calendar_id"]))) ||
                    (Request["calendar_id"] == null) || (Request["calendar_id"] == "0"))
                {
                    try
                    {
                        switch (Request["action"])
                        {
                            case "get_events":
                                stringForClient = eng.SelectEventsByPeriod(Request["from"], Request["till"], user_id);
                                break;
                            case "get_year_events":
                                stringForClient = eng.SelectYearEvents(Request["date"], user_id);
                                break;
                            case "get_settings":
                                DBHelper dbH = new DBHelper();
                                int userID = Convert.ToInt32(Session[Constants.sessionUserID]);
                                if (!dbH.IsUserDataExists(userID))
                                {
                                    dbH.InsertUsersData(userID, sett.DefaultTimeFormat, sett.DefaultDateFormat, 
                                                        sett.ShowWeekends, sett.WorkdayStarts, sett.WorkdayEnds, 
                                                        sett.ShowWorkDay, sett.WeekStartsOn, sett.DefaultTab, 
                                                        sett.DefaultCountry.ToString(), sett.DefaultTimeZone, 
                                                        sett.AllTimeZones);
                                    dbH.InsertDefaultCalendar(userID);
                                }
                                stringForClient = eng.SelectSettingsByUserId(user_id, false);
                                break;
                            case "update_event": 
                                stringForClient = eng.UpdateEvent(Request.QueryString);
                                break;
                            case "delete_event":
                                stringForClient = eng.DeleteEvent(Request["event_id"]);
                                break;
                            case "update_calendar":
                                stringForClient = eng.UpdateCalendar(user_id, Request.QueryString);
                                break;
                            case "delete_calendar":
                                stringForClient = eng.DeleteCalendar(Request["calendar_id"]);
                                break;
                            case "get_calendars":
                                stringForClient = eng.SelectCalendarsByUserId(user_id);
                                break;
                            case "update_settings":
                                stringForClient = eng.UpdateSettings(user_id, Request["displayname"], 
                                                                     Request["timeFormat"], Request["dateFormat"],
                                                                     Request["showWeekends"], Request["workdayStarts"], 
                                                                     Request["WorkdayEnds"], Request["showWorkday"], 
                                                                     Request["weekstartson"], Request["tab"], 
                                                                     Request["country"], Request["TimeZone"], Request["AllTimeZones"]);
                                break;
                            default:
                                stringForClient = null;
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex);
                        stringForClient = eng.GetErrorJsonString("", ex.Message);
                    }
                }
                else if (Request["calendar_id"] != null)
                    stringForClient = eng.GetErrorJsonString("", _resMan.GetString("PROC_WRONG_ACCT_ACCESS"));
                
                if (stringForClient != null)
                    Response.Write(stringForClient.ToString());
                else
                    Response.Write("[]");
            }
            else
            {
                stringForClient = eng.GetErrorJsonString("", _resMan.GetString("PROC_SESSION_ERROR"));
                Response.Write(stringForClient.ToString());
            }
			Response.End();
		}
	}
}