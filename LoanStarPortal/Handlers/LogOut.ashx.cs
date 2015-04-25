using LoanStar.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.SessionState;

namespace LoanStarPortal.Handlers
{
    /// <summary>
    /// Summary description for Default
    /// </summary>
    public class LogOut : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            AppUser user = context.Session[Constants.USEROBJECTNAME] as AppUser;
            if (user != null)
            {
                user.SaveUserLog(user.UserLogId, DateTime.Now);
            }

            FormsAuthentication.SignOut();
            context.Session.Clear();
            Global.integration = null;
            Global.userAccount = null;

            var jsonSerializer = new JavaScriptSerializer();
            String resp = "Ok";
            user = null;
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(jsonSerializer.Serialize(resp));
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