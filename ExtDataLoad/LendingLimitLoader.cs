using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Data;
using BossDev.CommonUtils;

namespace DataLoader
{
    public class LendingLimitLoader
    {
        private const string LendingLimit = "LendingLimit";
        private const string AppName = "LoanStar.LendingLimitLoader";
        public void GetData()
        {
/*            if (!System.Diagnostics.EventLog.SourceExists(AppName))
                System.Diagnostics.EventLog.CreateEventSource(
                   AppName, "Application");
            System.Diagnostics.EventLog EventLog1 = new System.Diagnostics.EventLog();

            EventLog1.Source = AppName;
            EventLog1.WriteEntry(AppName + " started at " + DateTime.Now.ToLongTimeString());*/
            DatabaseAccess DB = new DatabaseAccess(System.Configuration.ConfigurationSettings.AppSettings["SqlConnectionString"]);
            DB.ExecuteScalar("EventLogWrite", LendingLimit, AppName + " started at " + DateTime.Now.ToLongTimeString(), DBNull.Value);

            string requestUrl = System.Configuration.ConfigurationSettings.AppSettings["LendingLimitUrl"];
            // Prepare web request...
            HttpWebRequest myRequest =
                  (HttpWebRequest)WebRequest.Create(requestUrl);

            // Assign the response object of 'HttpWebRequest' to a 'HttpWebResponse' variable.
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myRequest.GetResponse();

            // Display the contents of the page to the console.
            Stream streamResponse = myHttpWebResponse.GetResponseStream();

            // Get stream object
            StreamReader streamRead = new StreamReader(streamResponse);

            Char[] readBuffer = new Char[212];

            // Read from buffer
            String resultData  = streamRead.ReadLine();
            
            int count = 0;
            while (resultData != null)
            {
                try
                {
                    //// Write the data 
                    //Console.WriteLine(resultData);
                    string MSACode = resultData.Substring(0, 5);
                    string SubMSA = resultData.Substring(5, 5);
                    string MSAName = resultData.Substring(10, 50);
                    int OneFamily = Convert.ToInt32(resultData.Substring(66, 7));
                    int TwoFamily = Convert.ToInt32(resultData.Substring(73, 7));
                    int ThreeFamily = Convert.ToInt32(resultData.Substring(80, 7));
                    int FourFamily = Convert.ToInt32(resultData.Substring(87, 7));
                    string StateCode = resultData.Substring(145, 2);
                    string StateName = resultData.Substring(150, 26);
                    string CountyCode = resultData.Substring(147, 3);
                    string CountyName = resultData.Substring(176, 15);
                    DateTime LastRevised = new DateTime(Convert.ToInt32(resultData.Substring(204, 4)), Convert.ToInt32(resultData.Substring(208, 2)), Convert.ToInt32(resultData.Substring(210, 2)));
                    if (CountyCode.Trim() != "" && StateCode.Trim()!="")
                    {
                        if (DB.ExecuteScalarInt("SaveLendingLimit", MSACode.Trim(), SubMSA.Trim(), MSAName.Trim(), OneFamily, TwoFamily, ThreeFamily, FourFamily, LastRevised, CountyCode, StateCode) == -1)
                            DB.ExecuteScalar("EventLogWrite", LendingLimit, String.Format("Can't save lending limit for {0} state, {1} county", StateName.Trim(), CountyName.Trim()), DBNull.Value);
                        else
                            count++;
                    }
                }
                finally
                {
                    // Read from buffer
                    resultData = streamRead.ReadLine();
                }
            }
            DB.ExecuteScalar("EventLogWrite", LendingLimit, String.Format("Updated {0} lending limits", count), DBNull.Value);
            // Release the response object resources.
            streamRead.Close();
            streamResponse.Close();
            // Close response
            myHttpWebResponse.Close();
        }

    }
}
