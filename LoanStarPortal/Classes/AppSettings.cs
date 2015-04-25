using System;
using System.Configuration;

namespace LoanStar.Common
{
    /// <summary>
	/// Incapsulates all application settings taken from web.config.
	/// This allows all references to web.config's keys to be declared
	/// in a single place (instead of scattered across the code) and 
	/// to be typed as required.
	/// All members must be static.
	/// For more information see corresponding descriptions in web.config.
	/// 
	/// Also you can declare any application-wide constants here.
	/// </summary>
	public class AppSettings
	{
		private AppSettings()
		{
		}

        static AppSettings()
        {
            try
            {
                DumpRules = Convert.ToInt32(ConfigurationManager.AppSettings["DumpRules"]);
            }
            catch { }
            try
            {
                ShowAll = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowAll"]);
            }
            catch
            {
            }
            try
            {
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["LogCalculator"]))
                    LogCalculator = Convert.ToBoolean(ConfigurationManager.AppSettings["LogCalculator"]);
            }
            catch
            {
            }
            try
            {
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RefreshPipeLine"]))
                    RefreshPipeline = Convert.ToInt32((ConfigurationManager.AppSettings["RefreshPipeLine"]));
            }
            catch
            {
            }
            try
            {
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultTimeZone"]))
                    DefaultTimeZone = Convert.ToInt32((ConfigurationManager.AppSettings["DefaultTimeZone"]));
            }
            catch
            {
            }
            try
            {
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["smtpServerPort"]))
                    SmtpServerPort = Convert.ToInt32((ConfigurationManager.AppSettings["smtpServerPort"]));
            }
            catch
            {
            }

        }
        public static int DumpRules = 0;

		// All session variables keys used in the project

		public const string SESSION_LAST_ERROR					= "LastError";
        


		#region General settings
		public static readonly string SqlConnectionString = 
			ConfigurationManager.AppSettings["SqlConnectionString"];
		
		public static readonly string DefaultStartPage =
            ConfigurationManager.AppSettings["DefaultStartPage"];

		public static readonly string DefaultPopupPage =
            ConfigurationManager.AppSettings["DefaultPopupPage"];

		public static readonly string SkinFolder =
            ConfigurationManager.AppSettings["SkinFolder"];

        public static readonly string LogFolder =
           ConfigurationManager.AppSettings["LogFolder"];
        public static readonly string AdminEmail =
            ConfigurationManager.AppSettings["AdminEmail"];

        //#region DEBUG ONLY
        //public static readonly bool NewLayout = false;
        //#endregion
        public static readonly bool ShowAll = false;
        public static readonly bool LogCalculator = false;
//        public static readonly bool OldAdvCalc = true;
        public static readonly int RefreshPipeline = 15;  // minutes
        public static readonly int DefaultTimeZone = 14;
        public static readonly int SmtpServerPort = 25;

        public static readonly string ChkLogin =
           ConfigurationManager.AppSettings["DumpLog"].Remove(0, 3);

        public static readonly string HelpUrlPublic =
           ConfigurationManager.AppSettings["HelpUrlPublic"];

        public static readonly string HelpUrlCompanyAdmin =
           ConfigurationManager.AppSettings["HelpUrlCompanyAdmin"];
        public static readonly string HelpUrlGlobalAdmin =
           ConfigurationManager.AppSettings["HelpUrlGlobalAdmin"];
        public static readonly string SmtpServerHost =
           ConfigurationManager.AppSettings["smtpServerHost"];

        #endregion

    }

}
