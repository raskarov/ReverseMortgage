using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BossDev.CommonUtils;

namespace LoanStar.Common
{
    public class AdminPage : AppPage
    {
        private const string ORIGINATORID = "originatorid";
        public bool LoggedAsOriginator
        { 
            get
            {
                return (OriginatorId > 1);
            }            
        }
        public int OriginatorId
        {
            get
            {
                int res = -1;
                Object o = Session[ORIGINATORID];
                try
                {
                    res = Convert.ToInt32(o.ToString());
                }
                catch{}
                return res;
            }
            set
            {
                Session[ORIGINATORID] = value;
            }
        }
    }
}
