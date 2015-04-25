using System;
using System.Configuration;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class EmailAdd : AppControl
    {
        //private bool IsNewMessage
        //{
        //    get
        //    {
        //        bool res = false;
        //        Object o = Session["condition_message"];
        //        if(o!=null)
        //        {
        //            try
        //            {
        //                res = bool.Parse(o.ToString());
        //            }
        //            catch
        //            {
        //            }
        //        }
        //        return res;
        //    }
        //}
        public event EventHandler MailSent;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.HasEmail)
            {
                return;
            }
        }
        public void SetContent()
        {
           if (!CurrentUser.HasEmail)
           {
                return;
           }
           if (Global.integration == null)
           {
                Global.integration = new WebMailPro.Integration(ConfigurationManager.AppSettings["dataFolderPath"], @".");
           }
           string sWebMailHost = "RMOMail/webmailwrapper.aspx?displayMode=newmessage";
           TopPane.ContentUrl = sWebMailHost;
        }
    }
}