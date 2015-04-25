using System.Data;

namespace LoanStar.Common
{
    public class ManagerRelation : BaseObject
    {
        #region constants
        private const string CANHAVEEMPLOYEE = "CanHaveEmployee";
        private const string CANHAVEMANAGER = "CanHaveManager";
        private const string GETMANAGERLISTFORUSERROLE = "GetManagerListForUserRole";
        private const string GETROLELISTFORUSER = "GetRoleListForUser";
        private const string GETMANAGEREMPLOYEE = "GetManagerEmployee";
        private const string GETEMPLOYEELIST = "GetEmployeeList";
        private const string SAVEMANAGEREMPLOYEE = "SaveManagerEmployee";
        private const string DELETEEMPLOYEE = "DeleteManagerEmployee";
        #endregion

        #region fields
        #endregion

        #region properties
        #endregion

        #region constructors
        private ManagerRelation()
        {
        }

        #endregion

        #region methods
        
        #region static
        public static bool CanHaveEmployee(int roleId)
        {
            return db.ExecuteScalarInt(CANHAVEEMPLOYEE, roleId)>0;
        }
        public static bool CanHaveManager(int roleId)
        {
            return db.ExecuteScalarInt(CANHAVEMANAGER, roleId)>0;
        }
        public static DataView GetManagerForUserRole(int companyId, int userId, int roleId)
        {
            return db.GetDataView(GETMANAGERLISTFORUSERROLE, companyId, userId, roleId);
        }
        public static DataView GetManagersForUser(int userId)
        {
            return db.GetDataView(GETROLELISTFORUSER, userId);
        }
        public static DataView GetEmployee(int userId, int roleId)
        {
            return db.GetDataView(GETMANAGEREMPLOYEE, userId, roleId);
        }
        public static DataView GetEmployeeList(int id, int companyId, int userId, int roleId)
        {
            return db.GetDataView(GETEMPLOYEELIST, id, companyId, userId, roleId);
        }
        public static int SaveManagerEmployee(int id,int managerId,int managerRoleId,int employeeId,int employeeRoleId)
        {
            return db.ExecuteScalarInt(SAVEMANAGEREMPLOYEE, id, managerId, managerRoleId, employeeId, employeeRoleId);
        }
        public static int DeleteEmployee(int id)
        {
            return db.ExecuteScalarInt(DELETEEMPLOYEE, id);
        }

        #endregion

        #endregion

    }
}
