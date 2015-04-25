using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.IO;

using LoanStar.Common;

namespace LoanStarPortal
{
    public class Global : System.Web.HttpApplication
    {
        public static WebMailPro.Integration integration = null;
        public static WebMailPro.Account userAccount = null;

        protected void Application_Start(object sender, EventArgs e)
        {
            string virtualFolder = Constants.STORAGEFOLDER + Constants.MAILATTACHESFOLDER;
            Mailer.MailAttachFolder = Server.MapPath(virtualFolder);

            virtualFolder = Constants.STORAGEFOLDER + Constants.TEMPLATESFOLDER;
            DocTemplateVersion.DocTemplateFolder = Server.MapPath(virtualFolder);

            virtualFolder = Constants.STORAGEFOLDER + Constants.PACKAGESFOLDER;
            Package.PackageFolder = Server.MapPath(virtualFolder);

            virtualFolder = Constants.STORAGEFOLDER;
            Logger.StorageFolder = Server.MapPath(virtualFolder);

            BaseObject.Cache = this.Context.Cache;
        }

        protected void Application_End(object sender, EventArgs e)
        {
            
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            integration = null;
            userAccount = null;

            integration = new WebMailPro.Integration(ConfigurationManager.AppSettings["dataFolderPath"], @".");
        }
        protected void Session_End(object sender, EventArgs e)
        {

        }
    }
}