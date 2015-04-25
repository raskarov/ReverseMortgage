using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace LoanStar.Common
{
    public class NotificationManager
    {
        #region constants
        public const int WRONGLOGINTYPE = 1;
        #endregion

        #region fields
        private int notificationType; 
        #endregion

        #region constructor
        public NotificationManager(int notificationType_)
        {
            notificationType = notificationType_;
        }
        #endregion

        #region methods
        public void Notify(Object obj)
        {
        }
        #endregion
    }
}
