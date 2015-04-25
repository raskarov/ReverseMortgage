using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.IO;
using WebMailPro;
using System.Globalization;

namespace Calendar_NET
{
    public partial class mailadm_calendar_settings : System.Web.UI.UserControl
    {
        protected WebmailSettings _settings;
        public List<string> countries;
        protected DateTime currentDateTime;
        public string timeFormat1;
		public string timeFormat2;
		public string timeFormat3;
		public string timeFormat4;
		public string timeFormat5;

        public WebmailSettings settings
        {
            set { _settings = value; }
            get { return _settings; }
        }

        private void Page_Load(object sender, EventArgs e)
        {
            currentDateTime = DateTime.Now;
			string day_str, month_dig_str, month_let_str, year_str;

			CultureInfo ci = new CultureInfo("en-US");
			day_str = currentDateTime.ToString("dd");
			month_dig_str = currentDateTime.ToString("MM");
			month_let_str = currentDateTime.ToString("MMM", ci);
			year_str = currentDateTime.ToString("yyyy");
			timeFormat1 = month_dig_str + "/" + day_str + "/" + year_str;
			timeFormat2 = day_str + "/" + month_dig_str + "/" + year_str;
			timeFormat3 = year_str + "-" + month_dig_str + "-" + day_str;
			timeFormat4 = month_let_str + " " + day_str + ", " + year_str;
			timeFormat5 = day_str + " " + month_let_str + ", " + year_str;

            countries = new List<string>();

            string CountryFileName = Path.Combine(ConfigurationManager.AppSettings[Constants.appSettingsDataFolderPath], @"Country\country.dat");
            StreamReader countryReader = null;
            try
            {
                countryReader = File.OpenText(CountryFileName);
                string currentLine = "";
                //----------------------------------------------------------------------------------------------------------------------------
                //fill all countries list//
                while ((currentLine = countryReader.ReadLine()) != null)
                {
                    countries.Add(currentLine);
                }
            }
            catch (Exception error)
            {
                UnsuccessOutput("Error!", error);
            }
            finally
            {
                if (countryReader != null)
                    countryReader.Close();
            }

            settings = new WebmailSettings().CreateInstance();

        }

        public void InitSettingsObject(WebmailSettings obj)
        {
            _settings = obj;
        }

        public void sendSettings_Click(Object sender, CommandEventArgs cmd)
        {
            string[] arr = new string[10];
            arr = AllParameters.Value.Split('#');
            //--------------------------------------------------------------------------------------------------------------------------------
            settings.DefaultTimeFormat = int.Parse(arr[0]);
            settings.DefaultDateFormat = int.Parse(arr[1]);
            settings.ShowWeekends = int.Parse(arr[2]);
            settings.WorkdayStarts = int.Parse(arr[3]);
            settings.WorkdayEnds = int.Parse(arr[4]);
            settings.ShowWorkDay = int.Parse(arr[5]);
            settings.DefaultTab = int.Parse(arr[6]);
            settings.DefaultCountry = arr[7];
            settings.DefaultTimeZoneCalendar = short.Parse(arr[8]);
            settings.AllTimeZones = int.Parse(arr[9]);
            settings.WeekStartsOn = int.Parse(arr[10]);

            try
            {
                settings.SaveWebmailSettings();//save all settings in xml//
                SuccessOutput(Constants.mailAdmSaveSuccess);
            }
            catch(Exception error)
            {
                UnsuccessOutput(Constants.mailAdmSaveUnsuccess, error);            
            }
        }

        private void SuccessOutput(string outStr)
        {
            messLabelID.InnerText = outStr;
            messLabelID.Style.Add("color", "green");
            messLabelID.Style.Add("font", "bold");

            errorLabelID.InnerText = string.Empty;
        }

        private void UnsuccessOutput(string outStr, Exception ex)
        {
            messLabelID.InnerText = outStr;
            messLabelID.Style.Add("color", "red");
            messLabelID.Style.Add("font", "bold");

            if (ex != null)
            {
                errorLabelID.InnerText = ex.Message;
                errorLabelID.Style.Add("color", "red");
            }
        }

    }
}