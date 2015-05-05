using BossDev.CommonUtils;
using LoanStar.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace LoanStarPortal.Handlers
{
    /// <summary>
    /// Summary description for Completed
    /// </summary>
    public class Completed : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;

            //deserialize the object
            Info objUser = Deserialize<Info>(context);

            DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);
            int res = db.ExecuteScalarInt("UpdateCompletedCondition", objUser.id, true);

            var jsonSerializer = new JavaScriptSerializer();
            String resp = "Completed";
            
            context.Response.Write(jsonSerializer.Serialize(resp));
        }

        public T Deserialize<T>(HttpContext context)
        {
            //read the json string
            string jsonData = new StreamReader(context.Request.InputStream).ReadToEnd();

            //cast to specified objectType
            var obj = (T)new JavaScriptSerializer().Deserialize<T>(jsonData);

            //return the object
            return obj;
        }

        public class Info
        {
            public Int32 id { get; set; }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}