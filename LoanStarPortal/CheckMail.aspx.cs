using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.ComponentModel;
using LoanStar.Common;

namespace LoanStarPortal
{
    public partial class CheckMail : AppPage
    {
        public delegate void LongTimeTask_Delegate();

        protected void ShowError(string txt)
        {
            string script;
            script = String.Format("alert('Can not load new messages from mail server.  <br/>Error: {0}')", txt);
            ClientScript.RegisterClientScriptBlock(GetType(), "MailError", script, true);
        }
        protected void GetMail()
        {
            if (!IsMailChecked)
            {
                try
                {
                    //System.Threading.Thread.Sleep(6000);
                    Mailer.SavePop3MailList(CurrentUser);
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LongTimeTask_Delegate d;
                d = new LongTimeTask_Delegate(GetMail);
                IAsyncResult R;
                R = d.BeginInvoke(new AsyncCallback(TaskCompleted), null);
            }
        }

        public void TaskCompleted(IAsyncResult R)
        {
            IsMailChecked = true;
            //ClientScript.RegisterClientScriptBlock(GetType(), "MailChecked", "window.parent.ChangeLink();", true);
        }

    }
}