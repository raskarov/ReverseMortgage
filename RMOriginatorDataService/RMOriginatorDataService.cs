using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using RMOriginatorDataService.Properties;

namespace RMOriginatorDataService
{
    public partial class RMOriginatorDataService : ServiceBase
    {
        Timer indexTimer = new Timer();
        Timer limitTimer = new Timer();
        Settings settings = new Settings();
        bool firstStart = true;

        public RMOriginatorDataService()
        {
            InitializeComponent();
            InitializeEventLog();
            InitializeTimer();
        }

        private void InitializeEventLog()
        {
            if (!System.Diagnostics.EventLog.SourceExists("RMOriginator"))
            {
                System.Diagnostics.EventLog.CreateEventSource("RMOriginator", "RMOriginator log");
            }
            eventLog = new EventLog();
            eventLog.Source = "IndexRate update";
            eventLog.Log = "RMOriginator log";
        }

        private void InitializeTimer()
        {
            // Hook up the Elapsed event for the timer.
            indexTimer.AutoReset = false;
            indexTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            // each minute  1 sec * 60.
            indexTimer.Interval = 180;// *settings.TimeOutRate.TotalSeconds;start after 2.5 min 
            indexTimer.Enabled = true;

            #region  Logic related to limis
            /*            
                limitTimer.AutoReset = false;
                limitTimer.Elapsed += new ElapsedEventHandler(limitTimer_Elapsed);
                // each minute  1 sec * 60.
                limitTimer.Interval = 300 * 1000;// *settings.TimeOutLimit.TotalSeconds;Start after 1 sec
                limitTimer.Enabled = true;
            */
            #endregion

            //eventLog.WriteEntry(String.Format("Timer was initialized ok! TimeOut = {0}", settings.TimeOutRate));
        }

        private void LoadRateData()
        {
            int numUpdates = -1;
            try
            {
                eventLog.WriteEntry("Check for updates for Index rastes.");
                DataLoader.RateLoader rate = new DataLoader.RateLoader();
                numUpdates = rate.GetData();
                if (numUpdates > 0)
                    eventLog.WriteEntry("Rates updated at " + DateTime.Now.ToLongTimeString());
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
            if (numUpdates > 0)
            {
                DateTime today = DateTime.Now;
                indexTimer.Interval = 1000 * ((23 - today.Hour + settings.WakeupTime.Hours) * 3600 + (59 - today.Minute + settings.WakeupTime.Minutes) * 60 + 60 - today.Second + settings.WakeupTime.Seconds);
            }
            else
            {
                indexTimer.Interval = 1000 * settings.TimeOutRate.TotalSeconds;
            }
        }

        #region  Logic related to limis
/*
        private void LoadLimitData()
        {
            try
            {
                eventLog.WriteEntry("Check for updates for limit data");
                DataLoader.LendingLimitLoader lim = new DataLoader.LendingLimitLoader();
                lim.GetData();
                eventLog.WriteEntry("Limit data is updated at " + DateTime.Now.ToLongTimeString());
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
            if (firstStart)
            {
                DateTime today = DateTime.Now;
                limitTimer.Interval = 1000 * ((23 - today.Hour + settings.WakeupTime.Hours) * 3600 + (59 - today.Minute + settings.WakeupTime.Minutes) * 60 + 60 - today.Second + settings.WakeupTime.Seconds);
                firstStart = false;
            }
            else
                limitTimer.Interval = 1000 * settings.TimeOutLimit.TotalSeconds;
        }
        void limitTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                limitTimer.Stop();
                LoadLimitData();
                //eventLog.WriteEntry(DateTime.Now.ToLongTimeString());
            }
            finally
            {
                limitTimer.Start();
            }
        }
 */ 
        #endregion

        protected override void OnStart(string[] args)
        {
            indexTimer.Start();
            #region  Logic related to limis
//            limitTimer.Start();
            #endregion
            //eventLog.WriteEntry("Timer was started");
        }

        protected override void OnStop()
        {
            indexTimer.Enabled = false;
            //eventLog.WriteEntry("Timer was stoped");
        }

        protected override void OnContinue()
        {
            indexTimer.Enabled = true;
            //eventLog.WriteEntry("Timer was started (in continue)");
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                indexTimer.Stop();
                LoadRateData();
            }
            finally
            {
                indexTimer.Start();
            }
        }
    }
}
