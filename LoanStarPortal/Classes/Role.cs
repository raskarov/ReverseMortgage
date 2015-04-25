using System;
using System.Text;
using System.Data;
using BossDev.CommonUtils;

namespace LoanStar.Common
{
    /// <summary>
    /// Summary description for Role
    /// </summary>
    public class Role
    {
        #region constants
        #region db fields
        public const string IDFIELDNAME = "id";
        public const string PARENTIDFIELDNAME = "parentroleid";
        public const string NAMEFIELDNAME = "name";
        public const string COMPANYIDFIELDNAME = "companyid";
        public const string ALREADYEXISTS = "Role with this name already exists";
        public const string UNKNOWNERROR = "Can't save data";
        public const string AUTHORITYFIELDNAME = "AuthorityLevel";
        #endregion

        #region procedure names
        private const string GETROLETEMPLATELIST    = "GetRoleTemplateList";
        private const string GETAUTHORITYLIST       = "GetAuthorityList";
        private const string GETMORTAGEGROLELIST    = "GetMortgageRoleList";
        private const string GETROLELIST            = "GetRoleList";
        private const string DELETEROLE             = "DeleteRole";
        private const string DELETEROLETEMPLATE     = "DeleteRoleTemplate";
        private const string GETROLETEMPLATESTATUSLIST  = "GetRoleTemplateStatusList";
        private const string GETROLESTATUSLIST      = "GetRoleStatusList";
        private const string GETMPSTATUSLIST        = "GetMpStatusList";
        private const string LOADROLETEMPLATEBYID   = "GetRoleTemplate";
        private const string DELETEROLESTATUSTEMPLATE = "DeleteRoleTemplateStatus";
        private const string DELETEROLESTATUS        = "DeleteRoleStatus";
        private const string SAVEROLETEMPLATE       = "SaveRoleTemplate";
        private const string SAVEROLE              = "SaveRole";
        private const string GETALLOWEDROLETEMPLATESTATUS = "GetAllowedRoleTemplateStatus";
        private const string GETALLOWEDROLESTATUS = "GetAllowedRoleStatus";
        private const string GETFIELDFORROLETEMPLATE = "GetFieldForRoleTemplate";
        private const string GETFIELDFORROLE            = "GetFieldForRole";
        private const string SAVEFIELDFORROLETEMPLATE = "SaveFieldForRoleTemplate";
        private const string SAVEFIELDFORROLE = "SaveFieldForRole";
        private const string GETCOMPANYOPERATIONUSERLIST = "GetCompanyOperationUser";
        private const string GETROLESRUCTURE = "GetRoleTemplateStructure";
        private const string GETROLETEMPLATELISTFORAUTHORITYLEVEL = "GetRoleTemplatesForAuthorityLevel";
        #endregion
        public const int ROOTROLEID = (int)AppUser.UserRoles.ExecutiveManager;
        #endregion

        #region fields
        private static readonly DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);
        private int id = -1;
        private string name = String.Empty;
        private readonly bool isTemplate;
        private string lastError = string.Empty;
        private int templateId = -1;
        private int companyId = -1;
        #endregion

        #region properties
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string LastError
        {
            get { return lastError; }
        }
        public int TemplateId
        {
            get { return templateId; }
            set { templateId = value; }
        }
        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }
        #endregion

        #region constructors
        public Role(bool isTemplate)
        {
            this.isTemplate = isTemplate;
        }
        public Role(int id, bool isTemplate)
        {
            this.isTemplate = isTemplate;
            this.id = id;
            if (id > 0) 
            {
                LoadById();
            }
        }
        public Role(int id, string name, bool isTemplate)
        {
            this.isTemplate = isTemplate;
            this.id = id;
            this.name = name;
        }
        public Role(DataRow dr)
        {
            id = int.Parse(dr[IDFIELDNAME].ToString());
            name = dr[NAMEFIELDNAME].ToString();
        }
        #endregion

        #region methods

        #region public

        #region instance
        public bool Delete()
        {
            return db.ExecuteScalarInt((isTemplate?DELETEROLETEMPLATE:DELETEROLE), id) == 1;
        }
        public bool DeleteStatus(int _id)
        {
            return db.ExecuteScalarInt((isTemplate ? DELETEROLESTATUSTEMPLATE : DELETEROLESTATUS), _id) == 1;
        }
        public DataView GetRoleStatusList()
        {
            return db.GetDataView((isTemplate ? GETROLETEMPLATESTATUSLIST : GETROLESTATUSLIST), id);
        }
        //public bool AddMPStatus(int initstatusid, int finalstatusid)
        //{
        //    return db.ExecuteScalarInt((isTemplate ? ADDROLETEMPLATESTATUS : ADDROLESTATUS), id, initstatusid, finalstatusid)==1;
        //}
        public bool Save()
        {
            int res =  -1;
            lastError = String.Empty;
            try
            {
                if (isTemplate)
                {
                    res = db.ExecuteScalarInt(SAVEROLETEMPLATE, id, name);
                }
                else
                {
                    res = db.ExecuteScalarInt(SAVEROLE, id, name, templateId, companyId);
                }
                if (res > 0)
                {
                    id = res;
                }
                else if (res == -1)
                {
                    lastError = ALREADYEXISTS;
                }
                else
                {
                    lastError = UNKNOWNERROR;
                }
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
                id = -1;
            }            
            return res>0;
        }
        public DataView GetAllowedMpStatusList(int statusid)
        {
            return db.GetDataView(isTemplate ? GETALLOWEDROLETEMPLATESTATUS : GETALLOWEDROLESTATUS, id, statusid);
        }
        public DataView GetAllowedFieldList(int groupid, int statusid)
        {
            if (isTemplate)
            {
                return db.GetDataView(GETFIELDFORROLETEMPLATE , id, groupid, statusid);
            }
            return db.GetDataView(GETFIELDFORROLE, id, groupid, statusid, companyId);
        }
        public bool SaveFieldList(string xml,int statusId)
        {
            bool res = false;
            lastError = String.Empty;
            if (isTemplate)
            {
                try
                {
                    res = db.ExecuteScalarInt(SAVEFIELDFORROLETEMPLATE, id,statusId, xml) == 1;
                }
                catch (Exception ex)
                {
                    lastError = ex.Message;
                }
            }
            else
            {
                try
                {
                    res = db.ExecuteScalarInt(SAVEFIELDFORROLE, id,statusId,companyId, xml) == 1;
                }
                catch (Exception ex)
                {
                    lastError = ex.Message;
                }
            }
            return res;
        }
        #endregion

        #region static
        public static DataView GetRoleStructure()
        {
            return db.GetDataView(GETROLESRUCTURE);
        }
        public static DataView GetList(bool isTemplate,bool all, bool ascending,int companyid)
        {
            if (isTemplate)
            {
                return db.GetDataView(GETROLETEMPLATELIST, all, ascending);
            }
            else 
            {
                return db.GetDataView(GETROLELIST, all, ascending, companyid);
            }
        }
        public static DataView GetRoleList()
        {
            return db.GetDataView("GetRoleListForPipeLine");
        }

        public static DataView GetAuthorityList()
        {
            return db.GetDataView(GETAUTHORITYLIST);
        }
        public static DataView GetListForSelect()
        {
            return db.GetDataView(GETROLETEMPLATELIST, true, true);
        }
        public static DataView GetMortgageRoleList()
        {
           return db.GetDataView(GETMORTAGEGROLELIST);
        }
        public static DataView GetOperationUserList(int companyId)
        {
            return db.GetDataView(GETCOMPANYOPERATIONUSERLIST, companyId);
        }
        public static DataView GetMPStatusList(int statusid)
        {
            return db.GetDataView(GETMPSTATUSLIST, statusid);
        }
        public static DataView GetRoleTemplateListForAuthorityLevel()
        {
            return db.GetDataView(GETROLETEMPLATELISTFORAUTHORITYLEVEL);
        }
        public static string GetManagerRoleNameById(int roleId)
        {
            DataView dv = db.GetDataView("GetManagerRoleNameById", roleId);
            StringBuilder sb = new StringBuilder();
            sb.Append(dv[0]["name"].ToString());
            for (int i = 1; i < dv.Count; i++ )
            {
                sb.Append(String.Format("({0})", dv[i]["name"]));
            }
            return sb.ToString();
        }

        #endregion

        #endregion

        #region private
        private void LoadById()
        {
            DataView dv = db.GetDataView(LOADROLETEMPLATEBYID, id);
            if (dv.Count > 0)
            {
                id = int.Parse(dv[0][IDFIELDNAME].ToString());
                name = dv[0][NAMEFIELDNAME].ToString();
            }
            else 
            {
                id = -1;
            }
        }
        #endregion

        #endregion
    }
}
