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

namespace LoanStar.CommonExt
{
    public class EMailWrapper
    {

        //public static Wrapper() { }

        private static LoanStar.Common.AppUser _AppUser;

        public static LoanStar.Common.AppUser AppCurrentUser
        {
            get { return _AppUser; }
            set { _AppUser = value; }
        }


    }
}
