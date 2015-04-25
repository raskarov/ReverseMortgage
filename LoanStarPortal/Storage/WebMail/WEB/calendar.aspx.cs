using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using WebMailPro;

namespace Calendar_NET
{
    public partial class calendar : System.Web.UI.Page
    {
		protected WebmailResourceManager _resMan = null;
        protected WebMailPro.classic.jsbuilder js = new WebMailPro.classic.jsbuilder();
        protected WebmailSettings sett = new WebmailSettings().CreateInstance();
        protected string defaultSkin = Constants.defaultSkinName;
        protected string defaultLang = Constants.defaultLang;
        protected string strBodyOnClick = String.Empty;
        protected string action = null;
        protected string request = null;
        protected Account acct = null;

        public calendar() { }

        protected void Page_Load(object sender, EventArgs e)
        {
            _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();

            acct = (Account)Session[Constants.sessionAccount];
            if (acct == null) // session expired
            {
                Response.Redirect("Default.aspx"); // ToDo: FIXME!!!
                Response.End();
                return;
            }
            defaultSkin = acct.UserOfAccount.Settings.DefaultSkin;
            defaultLang = acct.UserOfAccount.Settings.DefaultLanguage;

            BaseWebMailActions bwa = new BaseWebMailActions(acct, this.Page);
            Account[] Accounts = bwa.GetAccounts(acct.IDUser);

            if (Accounts.Length > 1)
            {
                strBodyOnClick = "PopupMenu.checkShownItems();";

                string _txt = @"PopupMenu.addItem(document.getElementById('popup_menu_1'), document.getElementById('popup_control_1'), 'wm_account_menu', document.getElementById('popup_replace_1'), document.getElementById('popup_replace_1'), '', '', '', '');";

                js.AddInitText(_txt);
            }
            string initfromClient = @"initCalendar();";
            js.AddInitText(initfromClient);

            js.AddText("function ResizeElements(mode) {}");
            string temp = @"
							function ChangeAccount(id)
							{
                                document.location.href = 'webmail.aspx?acct=' + id;
							}
						";
            js.AddText(temp);

# region Check for users data in calendar tables

            DBHelper dbH = new DBHelper();
            int userID = Convert.ToInt32(Session[Constants.sessionUserID]);

            if (!dbH.IsUserDataExists(userID))
            {
                dbH.InsertUsersData(userID, sett.DefaultTimeFormat, sett.DefaultDateFormat, sett.ShowWeekends, sett.WorkdayStarts, sett.WorkdayEnds, sett.ShowWorkDay, sett.WeekStartsOn, sett.DefaultTab, sett.DefaultCountry.ToString(), sett.DefaultTimeZone, sett.AllTimeZones);
                dbH.InsertDefaultCalendar(userID);
            }
            
# endregion

            ClientScriptManager cs = this.ClientScript;

            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "vars"))
            {
                string tmp_string = this.Request.Url.AbsoluteUri;
                int lInd = tmp_string.LastIndexOf("/");
                tmp_string = tmp_string.Substring(0, lInd) + "/";

                StringBuilder cstext = new StringBuilder();
                cstext.Append("<script type=text/javascript>");
                cstext.Append("var SITE_NAME = 'AfterLogic Calendar';");
                cstext.Append("var SCREEN_SETTINGS = 'Settings';");
                cstext.Append("var SCREEN_DAY = 'Day View';");
                cstext.Append("var SCREEN_WEEK = 'Week View';");
                cstext.Append("var SCREEN_MONTH = 'Month View';");
                cstext.Append("var fullhost='" + tmp_string + "';"); //absolut uri - full url // authority - host:port
                cstext.Append("</script>");
                cs.RegisterClientScriptBlock(this.GetType(), "vars", cstext.ToString(), false);
            }
        }
    }
}
