using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LoanStar.Common
{

	/// <summary>
	/// AppPage is a base class for all pages in the project.
	/// It provides common properties such as initialized instances of CurrentUser
	/// 
	/// Warning: Properties of AppControl and AppPage are available in OnLoad() but not on OnInit() !
	/// </summary>
	public class AppPage : Page
    {
        #region constants
        private const int MORTGAGESLIDINGDURATION = 30;
        public const int DICTIONARYSLIDINGDURATION = 60;
        public const string MORTGAGE = "mortgage";
        #endregion

        //		public DatabaseAccess DB;
        #region fields
        private AppUser user;
        private VendorGlobal vendor;
        private const string RULETREEPUBLICKEY = "rtpublic_{0}";
        private const string RULETREEADMINKEY = "rtadmin_{0}";
	    private const string REQUIREDFIELDKEY = "reqfields_{0}";
	    private const string ISRULEEVALUATED = "isruleevaluated";
        private const string PROCESSREQUIRED = "processreq";
        #endregion

        public Logger log;
        #region events/delegates
        public delegate void MortgageEventHandler(object sender, int MortgageID);
        public virtual event MortgageEventHandler MortgageChanged;
        #endregion

        #region properties
	    public bool IsAjaxPostBackRaisen
	    {
	        get
	        {
	            bool res = false;
	            Object o = Session["ajaxpostbackraisen"];
                if(o!=null)
                {
                    try
                    {
                        res = bool.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
	            return res;
	        }
            set { Session["ajaxpostbackraisen"] = value; }
	    }
        public VendorGlobal CurrentVendor
        {
            get
            {
                if (vendor == null)
                    vendor = GetObject(Constants.VENDOROBJECTNAME) as VendorGlobal;
                return vendor;
            }
            set
            {
                vendor = value;
                StoreObject(vendor, Constants.VENDOROBJECTNAME);
            }
        }
        public AppUser CurrentUser
        {
            get
            {
                if (user == null)
                {
                    user = GetUser();
                }
                return user;
            }
            set
            {
                user = value;
                StoreObject(user, Constants.USEROBJECTNAME);
            }
        }
        public virtual Panel PanelRight
        {
            get
            {
                return null;
            }
        }
        public virtual Panel PanelCenter
        {
            get
            {
                return null;
            }
        }
        public virtual Panel PanelLeft
        {
            get
            {
                return null;
            }
        }
	    public int ClientTimeOffest
	    {
	        get
	        {
	            int res = 0;
	            Object o = Session["clienttimeoffset"];
                if(o!=null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
	            return res;
	        }
            set { Session["clienttimeoffset"] = value; }
	    }
        public bool CenterRightPanelUpdateNeeded
        {
            get
            {
                bool res = false;
                Object o = Session[Constants.CENTERRIGHTPANELUPDATENEEDED];
                if (o != null)
                {
                    try
                    {
                        res = bool.Parse(o.ToString());
                    }
                    catch{}
                }
                return res;
            }
            set { Session[Constants.CENTERRIGHTPANELUPDATENEEDED] = value; }
        }
        public bool CenterLeftPanelUpdateNeeded
        {
            get
            {
                bool res = false;
                Object o = Session[Constants.CENTERLEFTPANELUPDATENEEDED];
                if (o != null)
                {
                    try
                    {
                        res = bool.Parse(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set { Session[Constants.CENTERLEFTPANELUPDATENEEDED] = value; }
        }
        #endregion

        #region methods
        protected void SetTimeOffset(int clientOffsetMinute)
        {
            ClientTimeOffest = clientOffsetMinute - (DateTime.UtcNow - DateTime.Now).Minutes;
        }

	    #region private
        private AppUser GetUser()
        {
            AppUser usr = GetObject(Constants.USEROBJECTNAME) as AppUser;
            if (usr == null)
            {
                Response.Redirect("~/Login.aspx?ReturnUrl=/Default.aspx");
            }
            else
            {
                usr.SaveUserLog(DateTime.Now);

            }
            return usr;
        }
        #endregion

        #region public
        #region Cache related methods
        public MortgageProfile GetMortgage(int mortgageId)
        {
            string key = MORTGAGE + "_" + mortgageId;
            MortgageProfile mp=null;
            if (Cache[key] != null)
            {
                mp = Cache[key] as MortgageProfile;
                if (mp != null) mp.BuildRuleEvaluationTree(GetRuleTreePublic());
            }
            if ((mp==null)||(mp.NeedUpdateInCache))
            {
                mp = new MortgageProfile(mortgageId, this);
                SetRuleEvaluationNeeded(true);
                mp.BuildRuleEvaluationTree(GetRuleTreePublic());
                mp.CurrentUserId = CurrentUser.Id;
                Cache.Insert(key, mp, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(MORTGAGESLIDINGDURATION));
            }
            return mp;
        }
        public MortgageProfile ReloadMortgage(int mortgageId)
        {
            MortgageProfile mp = new MortgageProfile(mortgageId, this);
            mp.CurrentUserId = CurrentUser.Id;
            UpdateMortgage(mp,mp.ID);
            return mp;
        }
        public RuleTreePublic GetRuleTreePublic()
        {
            string key = String.Format(RULETREEPUBLICKEY, CurrentUser.CompanyId);
            RuleTreePublic rt = null;
            //#region DEBUG ONLY
            //WriteToLog("GetRuleTreePublic started");
            //WriteToLog("Check cache key = " + key);
            //#endregion
            if(Cache[key]!=null)
            {
                rt = Cache[key] as RuleTreePublic;
            }
            if(rt==null)
            {
                rt = new RuleTreePublic(CurrentUser.CompanyId);
                Cache.Insert(key, rt, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(MORTGAGESLIDINGDURATION));
                //#region DEBUG ONLY
                //WriteToLog("Cache miss!!!");
                //#endregion
            }
            //#region DEBUG ONLY
            //WriteToLog("GetRuleTreePublic ended");
            //#endregion
            return rt;
        }
	    public RuleTreeAdmin GetRuleTreeAdmin()
        {
            string key = String.Format(RULETREEADMINKEY, CurrentUser.EffectiveCompanyId);
            RuleTreeAdmin rt = null;
            if (Cache[key] != null)
            {
                rt = Cache[key] as RuleTreeAdmin;
            }
            if (rt == null)
            {
                rt = new RuleTreeAdmin(CurrentUser.CompanyId);
                Cache.Insert(key, rt, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(MORTGAGESLIDINGDURATION));
            }
            return rt;
        }
        public RuleTreeAdmin ReloadRuleTreeAdmin()
        {
            string key = String.Format(RULETREEADMINKEY, CurrentUser.CompanyId);
            if (Cache[key] != null)
            {
                Cache.Remove(key);
            }
            return GetRuleTreeAdmin();
        }
        public void RemoveFromCacheByKey(string key)
        {
            if(Cache[key]!=null)
            {
                Cache.Remove(key);
            }
        }
	    public void RemoveFromCacheByPartialKey(string key)
        {
            //#region DEBUG ONLY
            //WriteToLog("Try to find key starts with : "+key);
            //#endregion

            int l = key.Length;
            foreach(DictionaryEntry item in Cache)
            {
                string s = item.Key.ToString();
                //#region DEBUG ONLY
                //WriteToLog("Checking key - " + s);
                //#endregion
                if (s.Length>=l)
                {
                    if(String.CompareOrdinal(key,s.Substring(0,l))==0)
                    {
                        Cache.Remove(s);
                        //#region DEBUG ONLY
                        //WriteToLog("Key removed - " + s);
                        //#endregion
                    }
                }
            }
        }
        public void RemoveMortgageFromCache(int mortgageId)
        {
            string key = MORTGAGE + "_" + mortgageId;
            if (Cache[key] != null)
            {
                Cache.Remove(key);
            }
        }
	    public void UpdateMortgage(MortgageProfile mp, int mortgageId)
        {
            string key = MORTGAGE + "_" + mortgageId;
            if (Cache[key] != null)
            {
                Cache.Remove(key);
            }
            Cache.Insert(key, mp, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(MORTGAGESLIDINGDURATION));
        }
        public DataView GetDictionary(string dictionaryName, string filter, string sort)
        {
            DataView dv = GetDictionary(dictionaryName);
            dv.RowFilter = filter;
            dv.Sort = sort;
            return dv;
        }
        public DataView GetDictionary(string dictionaryName, string filter)
        {
            DataView dv = GetDictionary(dictionaryName);
            dv.RowFilter = filter;
            return dv;
        }
        public DataView GetDictionary(string dictionaryName)
        {
            DataTable dt = GetDictionaryTable(dictionaryName);
            return new DataView(dt);
        }
        public DataTable GetDictionaryTable(string dictionaryName)
        {
            if (Cache[dictionaryName] != null)
            {
                return Cache[dictionaryName] as DataTable;
            }
            else
            {
                DataTable dv = DataHelpers.GetDictionaryTable(dictionaryName);
                Cache.Insert(dictionaryName, dv, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(DICTIONARYSLIDINGDURATION));
                return dv;
            }
        }
        public DataTable GetDictionaryTableByProcedure(string procedureName)
        {
            if (Cache[procedureName] != null)
            {
                return Cache[procedureName] as DataTable;
            }
            else
            {
                DataTable dt = DataHelpers.GetDictionaryByProcedure(procedureName);
                Cache.Insert(procedureName, dt, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(DICTIONARYSLIDINGDURATION));
                return dt;
            }
        }
        public DataView GetDictionaryViewByProcedure(string procedureName)
        {
            DataTable dt = GetDictionaryTableByProcedure(procedureName);
            return new DataView(dt);
        }
        public DataView GetDictionaryViewByProcedure(string procedureName, string filter)
        {
            DataView dv = GetDictionaryViewByProcedure(procedureName);
            dv.RowFilter = filter;
            return dv;
        }
        public DataView GetDictionaryViewByProcedure(string procedureName, string filter, string sort)
        {
            DataView dv = GetDictionaryViewByProcedure(procedureName);
            dv.RowFilter = filter;
            dv.Sort = sort;
            return dv;
        }
	    public DataView GetRequiredFields()
	    {
	        string key = String.Format(REQUIREDFIELDKEY, CurrentUser.CompanyId);
            if(Cache[key]!=null)
            {
                return Cache[key] as DataView;
            }
	        DataView dv = DataHelpers.GetRequiredFields(CurrentUser.CompanyId);
            Cache.Insert(key, dv, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(DICTIONARYSLIDINGDURATION));
	        return dv;
	    }
        #endregion
        public bool GetProcessRequired()
        {
            bool res = false;
            Object o = Session[PROCESSREQUIRED];
            if (o != null)
            {
                try
                {
                    res = (bool)o;
                }
                catch
                {
                }
            }
            return res;
        }

	    public void SetProcessRequired(bool val)
        {
            Session[PROCESSREQUIRED] = val;
        }

	    public bool GetRuleEvaluationNeeded()
        {
            bool res = true;
            Object o = Session[ISRULEEVALUATED];
            if(o!=null)
            {
                try
                {
                    res = (bool) o;
                }
                catch
                {
                }
            }
            return res;
        }
        public void SetRuleEvaluationNeeded(bool flg)
        {
            Session[ISRULEEVALUATED] = flg;
        }

	    public void StoreObject(Object o, string name)
        {
            Session[name] = o;
        }
        public Object GetObject(string name)
        {
            return Session[name];
        }
        public string GetValue(string name, string defaultvalue)
        {
            string result = Request[name];
            return (!String.IsNullOrEmpty(result)) ? result : defaultvalue;
        }
        public int GetValueInt(string name, int defaultvalue)
        {
            int result = defaultvalue;
            try
            {
                result = int.Parse(GetValue(name, defaultvalue.ToString()));
            }
            catch
            {
            }
            return result;
        }
        public int CurrentTopSecondTabIndex
        {
            get
            {
                int res = Constants.TOPSECONDTABINDEX;
                if (Session[Constants.CURRENTTOPSECONDTABINDEX] != null)
                {
                    res = int.Parse(Session[Constants.CURRENTTOPSECONDTABINDEX].ToString());
                }
                return res;
            }
            set
            {
                Session[Constants.CURRENTTOPSECONDTABINDEX] = value;
            }
        }
        public bool MessageBoardShowOnlyNotes
        {
            get
            {
                bool res = false;
                if (Session[Constants.MESSAGEBOARDSHOWONLYNOTES] != null)
                {
                    res = bool.Parse(Session[Constants.MESSAGEBOARDSHOWONLYNOTES].ToString());
                }
                return res;
            }
            set
            {
                Session[Constants.MESSAGEBOARDSHOWONLYNOTES] = value;
            }
        }
        public bool IsMailChecked
        {
            get
            {
                bool res = false;
                if (Session[Constants.ISMAILCHECKED] != null)
                {
                    res = bool.Parse(Session[Constants.ISMAILCHECKED].ToString());
                }
                return res;
            }
            set
            {
                Session[Constants.ISMAILCHECKED] = value;
            }
        }
        public virtual Control LoadUserControl(string controlName, Panel panel, string LoadedControlName)
        {
            return null;
        }
        /// <summary>
        /// Performs FindControl() on a given control and all his children
        /// </summary>
        /// <param name="control"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual Control FindControl(Control control, string id)
        {
            Control result = control.FindControl(id);
            if (result == null)
                foreach (Control child in control.Controls)
                {
                    result = FindControl(child, id);
                    if (result != null)
                        break;
                }
            return result;
        }

        /// <summary>
        /// Similar to Control.FindControl() except that it scans recursively
        /// thus allowing to find a unique control on a page.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Control Find(string id)
        {
            return FindControl(this, id);
        }
        #endregion

        #region DEBUG ONLY
	    public DateTime gTime = DateTime.MinValue;
        public DateTime gTime1 = DateTime.MinValue;
        public void WriteToLog(string message)
        {
            if(gTime != DateTime.MinValue)
            {
                TimeSpan ts = DateTime.Now - gTime;
                TimeSpan ts1 = DateTime.Now - gTime1;
                log.WriteLine(message + String.Format(" Elapsed time = {0} - {1} ({2})", ts, ts1,DateTime.Now.ToLongTimeString()));
            }
            else
            {
                gTime1 = DateTime.Now;
                log.WriteLine(message);
            }
            gTime = DateTime.Now;
        }
        public void WriteDebugToLog(string message)
        {
            log.AddTimeStamp = false;
            log.WriteLine(message);
        }

	    #endregion

        protected override PageStatePersister PageStatePersister
        {
            get
            {
                return new SessionPageStatePersister(this);
            }
        }
        #endregion

        #region constructior
        public AppPage()
        {
            log = new Logger(Path.Combine(Server.MapPath(Context.Request.ApplicationPath), AppSettings.LogFolder));
        }
        #endregion

	}
}
