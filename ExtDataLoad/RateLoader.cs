using System;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using BossDev.CommonUtils;
using System.Data;
using System.Globalization; 

namespace DataLoader
{
    public class RateLoader
    {
        private const string AppName = "LoanStar.RateLoader";
        private const string ModuleName = "IndexRate";
        private const string GETPRODUCTLISTFORRATES = "GetProductListForRates";

        DatabaseAccess DB;

        private static DateTime GetMonday(DateTime date)
        {
            DateTime startDate = date;
            switch (startDate.DayOfWeek)
            {
                case DayOfWeek.Tuesday:
                    startDate = startDate.AddDays(-1);
                    break;
                case DayOfWeek.Wednesday:
                    startDate = startDate.AddDays(-2);
                    break;
                case DayOfWeek.Thursday:
                    startDate = startDate.AddDays(-3);
                    break;
                case DayOfWeek.Friday:
                    startDate = startDate.AddDays(-4);
                    break;
                case DayOfWeek.Saturday:
                    startDate = startDate.AddDays(-5);
                    break;
                case DayOfWeek.Sunday:
                    startDate = startDate.AddDays(-6);
                    break;
            }
            return startDate;
        }
        private static XmlNodeList GetNodeList(string requestUrl)
        {
            // Prepare web request...
            HttpWebRequest myRequest =
                  (HttpWebRequest)WebRequest.Create(requestUrl);

            // Assign the response object of 'HttpWebRequest' to a 'HttpWebResponse' variable.
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myRequest.GetResponse();

            // Display the contents of the page to the console.
            Stream streamResponse = myHttpWebResponse.GetResponseStream();

            // Get stream object
            StreamReader streamRead = new StreamReader(streamResponse);

            Char[] readBuffer = new Char[256];

            int count = streamRead.Read(readBuffer, 0, 256);
            StringBuilder sb = new StringBuilder();
            while (count > 0)
            {
                sb.Append(readBuffer, 0, count);
                count = streamRead.Read(readBuffer, 0, 256);
            }

            streamRead.Close();
            streamResponse.Close();
            myHttpWebResponse.Close();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
            XmlElement root = doc.DocumentElement;
            XmlNamespaceManager nmgr = new XmlNamespaceManager(doc.NameTable);
            nmgr.AddNamespace("frb", "http://www.federalreserve.gov/structure/compact/common");
            nmgr.AddNamespace("kf", "http://www.federalreserve.gov/structure/compact/H15_H15");
            XmlNodeList nodeList;
            nodeList = root.SelectNodes("//kf:Series", nmgr);
            return nodeList;
        }
        public int GetData()
        {

            DB = new DatabaseAccess(System.Configuration.ConfigurationSettings.AppSettings["SQLConnectionString"]);

            DB.ExecuteScalar("EventLogWrite", ModuleName, AppName + " started at " + DateTime.Now.ToLongTimeString(), DBNull.Value);

            int numUpdates = -1;
            DataView dv = DB.GetDataView(GETPRODUCTLISTFORRATES);
            for( int i=0;i<dv.Count;i++)
            {
                int productId = int.Parse(dv[i]["Id"].ToString());
                double margin = double.Parse(dv[i]["margin"].ToString());
                string requestUrl = dv[i]["DataUrl"].ToString();
                string productName = dv[i]["name"].ToString();

                IndexRate currentRate = new IndexRate();
                currentRate.ProductID = productId;
                currentRate.Margin = margin;

                IndexRate expectedRate = new IndexRate();
                expectedRate.ProductID = productId;
                expectedRate.Margin = margin;
                

                if(dv[i]["LastUpdateDate"]!= DBNull.Value)
                {
                    requestUrl += "&from=" + Convert.ToDateTime(dv[i]["LastUpdateDate"]).AddDays(1).ToString("d", DateTimeFormatInfo.InvariantInfo) + "&to=" + DateTime.Now.ToString("d", DateTimeFormatInfo.InvariantInfo);
                }
                else
                {
                    DateTime curMonday = GetMonday(DateTime.Now);
                    string preRequestUrl = requestUrl + "&from=" + curMonday.AddDays(-21).ToString("d", DateTimeFormatInfo.InvariantInfo) + "&to=" + curMonday.AddDays(-8).ToString("d", DateTimeFormatInfo.InvariantInfo);
                    XmlNodeList preSeriesList = GetNodeList(preRequestUrl);
                    if (preSeriesList.Count > 0)
                    {
                        XmlNamespaceManager nmgr = new XmlNamespaceManager(preSeriesList[0].OwnerDocument.NameTable);
                        nmgr.AddNamespace("frb", "http://www.federalreserve.gov/structure/compact/common");
                        nmgr.AddNamespace("kf", "http://www.federalreserve.gov/structure/compact/H15_H15");
                        XmlNodeList preNodeList = preSeriesList[0].SelectNodes("frb:Obs", nmgr);
                        if (preNodeList.Count > 0)
                        {
                            foreach (XmlNode node in preNodeList)
                            {
                                if (node.Attributes["OBS_STATUS"].Value == "A")
                                {
                                    currentRate.Period = Convert.ToDateTime(node.Attributes["TIME_PERIOD"].Value);
                                    currentRate.DailyIndex = Convert.ToDouble(node.Attributes["OBS_VALUE"].Value, NumberFormatInfo.InvariantInfo);
                                    if (preSeriesList.Count > 1)
                                    {
                                        XmlNode expNode = preSeriesList[1].SelectSingleNode(String.Format("frb:Obs[@TIME_PERIOD='{0}']", node.Attributes["TIME_PERIOD"].Value), nmgr);
                                        if (expNode != null)
                                        {
                                            expectedRate.DailyIndex = Convert.ToDouble(expNode.Attributes["OBS_VALUE"].Value, NumberFormatInfo.InvariantInfo);
                                            expectedRate.Period = currentRate.Period;
                                            IndexRate.SaveProductRates(DB, currentRate, expectedRate);
                                        }
                                    }
                                    else
                                    {
                                        IndexRate.SaveProductRates(DB, currentRate, null);
                                    }
                                }
                            }
                        }
                    }
                    requestUrl += "&from=" + curMonday.AddDays(-7).ToString("d", DateTimeFormatInfo.InvariantInfo) + "&to=" + DateTime.Now.ToString("d", DateTimeFormatInfo.InvariantInfo);
                }
                XmlNodeList nodeSeriesList = GetNodeList(requestUrl);
                if (nodeSeriesList.Count > 0)
                {
                    XmlNamespaceManager nmgr = new XmlNamespaceManager(nodeSeriesList[0].OwnerDocument.NameTable);
                    nmgr.AddNamespace("frb", "http://www.federalreserve.gov/structure/compact/common");
                    nmgr.AddNamespace("kf", "http://www.federalreserve.gov/structure/compact/H15_H15");

                    XmlNodeList nodeList = nodeSeriesList[0].SelectNodes("frb:Obs", nmgr);

                    DateTime minDate = DateTime.MinValue, maxDate = DateTime.MinValue;
                    int count = 0;
                    if (nodeList.Count > 0)
                    {
                        minDate = maxDate = Convert.ToDateTime(nodeList[0].Attributes["TIME_PERIOD"].Value);
                    }
                    foreach (XmlNode node in nodeList)
                    {
                        XmlAttribute aStatus = node.Attributes["OBS_STATUS"];
                        if (aStatus.Value == "A")
                        {
                            XmlAttribute aValue = node.Attributes["OBS_VALUE"];
                            XmlAttribute aPeriod = node.Attributes["TIME_PERIOD"];
                            currentRate.Period = Convert.ToDateTime(aPeriod.Value);
                            currentRate.DailyIndex = Convert.ToDouble(aValue.Value, NumberFormatInfo.InvariantInfo);
                            if (nodeSeriesList.Count > 1)
                            {
                                XmlNode expNode = nodeSeriesList[1].SelectSingleNode(String.Format("frb:Obs[@TIME_PERIOD='{0}']", aPeriod.Value), nmgr);
                                expectedRate.DailyIndex = Convert.ToDouble(expNode.Attributes["OBS_VALUE"].Value, NumberFormatInfo.InvariantInfo);
                                expectedRate.Period = currentRate.Period;
                                IndexRate.SaveProductRates(DB, currentRate, expectedRate);
                            }
                            else
                            {
                                IndexRate.SaveProductRates(DB, currentRate, null);
                            }
                            if (minDate > currentRate.Period)
                                minDate = currentRate.Period;
                            if (maxDate < currentRate.Period)
                                maxDate = currentRate.Period;
                            count++;
                        }
                    }
                    if (nodeList.Count > 0)
                    {
                        DB.ExecuteScalar("EventLogWrite", ModuleName, String.Format("Added  {0} rates from {1} to {2} for product {3}", count, minDate.ToString("d"), maxDate.ToString("d"), productName), DBNull.Value);
                        if (numUpdates == -1)
                            numUpdates = nodeList.Count;
                    }
                    else
                    {
                        numUpdates = nodeList.Count;
                    }
                }
            }
            return numUpdates;
        }
    }

    #region old methods
        //public int GetData_()
        //{

        //    DB = new DatabaseAccess(System.Configuration.ConfigurationSettings.AppSettings["SQLConnectionString"]);

        //    DB.ExecuteScalar("EventLogWrite", ModuleName, AppName + " started at " + DateTime.Now.ToLongTimeString(), DBNull.Value);

        //    int numUpdates = -1; 
        //    DataTable products = DB.GetDataTable("GetProductList", 0);
        //    foreach (DataRow rowProd in products.Rows)
        //    {
        //        IndexRate iRate = new IndexRate(DB, false);
        //        IndexRate expRate = new IndexRate(DB, true);

        //        int ProductID = Convert.ToInt32(rowProd["id"]);
        //        iRate.ProductID = expRate.ProductID = ProductID;
        //        iRate.Margin = expRate.Margin = Convert.ToDouble(rowProd["Margin"]);
        //        string requestUrl = Convert.ToString(rowProd["DataUrl"]);
        //        DataTable tblMaxDate = DB.GetDataTable(String.Format("select max(Period) from ProductRate where ProductId = {0}", rowProd["ID"]));
        //        if (tblMaxDate.Rows[0][0] != DBNull.Value)
        //        {
        //            requestUrl += "&from=" + Convert.ToDateTime(tblMaxDate.Rows[0][0]).AddDays(1).ToString("d", DateTimeFormatInfo.InvariantInfo) + "&to=" + DateTime.Now.ToString("d", DateTimeFormatInfo.InvariantInfo);
        //        }
        //        else
        //        {
        //            DateTime curMonday = GetMonday(DateTime.Now);
        //            string preRequestUrl = requestUrl + "&from=" + curMonday.AddDays(-21).ToString("d", DateTimeFormatInfo.InvariantInfo) + "&to=" + curMonday.AddDays(-8).ToString("d", DateTimeFormatInfo.InvariantInfo);
        //            XmlNodeList preSeriesList = GetNodeList(preRequestUrl);
        //            if (preSeriesList.Count > 0)
        //            {
        //                XmlNamespaceManager nmgr = new XmlNamespaceManager(preSeriesList[0].OwnerDocument.NameTable);
        //                nmgr.AddNamespace("frb", "http://www.federalreserve.gov/structure/compact/common");
        //                nmgr.AddNamespace("kf", "http://www.federalreserve.gov/structure/compact/H15_H15");
        //                XmlNodeList preNodeList = preSeriesList[0].SelectNodes("frb:Obs", nmgr);
        //                if (preNodeList.Count > 0)
        //                {
        //                    foreach (XmlNode node in preNodeList)
        //                    {
        //                        if (node.Attributes["OBS_STATUS"].Value == "A")
        //                        {
        //                            iRate.Period = Convert.ToDateTime(node.Attributes["TIME_PERIOD"].Value);
        //                            iRate.Index = Convert.ToDouble(node.Attributes["OBS_VALUE"].Value, NumberFormatInfo.InvariantInfo);
        //                            iRate.CalculateAll();

        //                            if (preSeriesList.Count > 1)
        //                            {
        //                                XmlNode expNode = preSeriesList[1].SelectSingleNode(String.Format("frb:Obs[@TIME_PERIOD='{0}']", node.Attributes["TIME_PERIOD"].Value), nmgr);
        //                                if (expNode != null)
        //                                {
        //                                    expRate.Index = Convert.ToDouble(expNode.Attributes["OBS_VALUE"].Value, NumberFormatInfo.InvariantInfo);
        //                                    expRate.Period = iRate.Period;
        //                                    expRate.CalculateAll();
        //                                }
        //                            }

        //                        }
        //                    }
        //                }
        //            }
        //            requestUrl += "&from=" + curMonday.AddDays(-7).ToString("d", DateTimeFormatInfo.InvariantInfo) + "&to=" + DateTime.Now.ToString("d", DateTimeFormatInfo.InvariantInfo);
        //        }

        //        XmlNodeList nodeSeriesList = GetNodeList(requestUrl);
        //        if (nodeSeriesList.Count > 0)
        //        {
        //            XmlNamespaceManager nmgr = new XmlNamespaceManager(nodeSeriesList[0].OwnerDocument.NameTable);
        //            nmgr.AddNamespace("frb", "http://www.federalreserve.gov/structure/compact/common");
        //            nmgr.AddNamespace("kf", "http://www.federalreserve.gov/structure/compact/H15_H15");

        //            XmlNodeList nodeList = nodeSeriesList[0].SelectNodes("frb:Obs", nmgr);

        //            DateTime minDate = DateTime.MinValue, maxDate = DateTime.MinValue;
        //            int count = 0;
        //            if (nodeList.Count > 0)
        //            {
        //                minDate = maxDate = Convert.ToDateTime(nodeList[0].Attributes["TIME_PERIOD"].Value);
        //            }
        //            foreach (XmlNode node in nodeList)
        //            {
        //                XmlAttribute aStatus = node.Attributes["OBS_STATUS"];
        //                if (aStatus.Value == "A")
        //                {
        //                    XmlAttribute aValue = node.Attributes["OBS_VALUE"];
        //                    XmlAttribute aPeriod = node.Attributes["TIME_PERIOD"];
        //                    iRate.Period = Convert.ToDateTime(aPeriod.Value);
        //                    iRate.Index = Convert.ToDouble(aValue.Value, NumberFormatInfo.InvariantInfo);
        //                    iRate.CalculateAll();
        //                    if (nodeSeriesList.Count > 1)
        //                    {
        //                        XmlNode expNode = nodeSeriesList[1].SelectSingleNode(String.Format("frb:Obs[@TIME_PERIOD='{0}']", aPeriod.Value), nmgr);
        //                        expRate.Index = Convert.ToDouble(expNode.Attributes["OBS_VALUE"].Value, NumberFormatInfo.InvariantInfo);
        //                        expRate.Period = iRate.Period;
        //                        expRate.CalculateAll();
        //                        DB.ExecuteScalar("SaveProductRate", iRate.ProductID, iRate.Period, iRate.Index, iRate.DailyRate, iRate.RunningAverage, iRate.PublishedIndex, iRate.Margin, iRate.Weekday,
        //                            expRate.Index, expRate.DailyRate, expRate.RunningAverage, expRate.PublishedIndex);
        //                    }
        //                    else
        //                    {
        //                        DB.ExecuteScalar("SaveProductRate", iRate.ProductID, iRate.Period, iRate.Index, iRate.DailyRate, iRate.RunningAverage, iRate.PublishedIndex, iRate.Margin, iRate.Weekday,
        //                                DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value);
        //                    }
        //                    if (minDate > iRate.Period)
        //                        minDate = iRate.Period;
        //                    if (maxDate < iRate.Period)
        //                        maxDate = iRate.Period;
        //                    count++;
        //                }
        //            }
        //            if (nodeList.Count > 0)
        //            {
        //                DB.ExecuteScalar("EventLogWrite", ModuleName, String.Format("Added  {0} rates from {1} to {2} for product {3}", count, minDate.ToString("d"), maxDate.ToString("d"), rowProd["Name"]), DBNull.Value);
        //                if(numUpdates==-1)
        //                    numUpdates = nodeList.Count;
        //            }
        //            else
        //            {
        //                numUpdates = nodeList.Count;
        //            }
        //        }
        //    }
        //    return numUpdates;
        //}

    #endregion
}
