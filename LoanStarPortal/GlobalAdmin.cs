using System;
using System.Collections;
using System.Data;

namespace LoanStar.Common
{
    public class GlobalAdmin : BaseObject
    {

        public class GlobalAdminException : BaseObjectException
        {
            public GlobalAdminException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public GlobalAdminException(string message)
                : base(message)
            {
            }

            public GlobalAdminException()
            {
            }
        }

        #region Constants
        private const string GETGLOBALADMIN = "GetGlobalAdmin";
        private const string SAVE = "SaveGlobalAdmin";
        private const string MAXFEMAFLOODCOVERAGEFIELDNAME = "MaxFEMAFloodCoverage";
        #endregion

        #region fields
        private decimal maxFEMAFloodCoverage;
        #endregion

        #region properties
        public decimal MaxFEMAFloodCoverage
        {
            get { return maxFEMAFloodCoverage; }
            set { maxFEMAFloodCoverage = value; }
        }
        #endregion

        #region Constructors
        public GlobalAdmin()
            : this(0)
        { }
        public GlobalAdmin(int id)
        {
            ID = id;
            if (id > 0)
            {
                LoadById();
            }
        }
        #endregion

        #region methods
        
        #region public
        public int Save()
        {
            return db.ExecuteScalarInt(SAVE, ID, maxFEMAFloodCoverage);
        }
        #endregion

        #region private
        private void LoadById()
        {
            DataView dv = db.GetDataView(GETGLOBALADMIN, ID);
            if (dv.Count == 1)
            {
                maxFEMAFloodCoverage = Convert.ToDecimal(dv[0][MAXFEMAFLOODCOVERAGEFIELDNAME]);
            }
            else
            {
                ID = 0;
            }
        }
        #endregion

        #endregion
    }
}