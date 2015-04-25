using System;
using System.Data;

namespace LoanStar.Common
{
    public class Alert : BaseObject
    {
        #region constants
        private const string MORTGAGEIDFIELDNAME = "MortgageID";
        private const string DESCRIPTIONFIELDNAME = "Description";
        private const string ISACTIVEFIELDNAME = "IsActive";
        private const string CREATEDFIELDNAME = "Created";
        private const string IDFIELDNAME = "Id";
        private const string GETALERT = "GetAlert";
        #endregion

        private readonly DataRow dataRow;

        #region  properties
        public int MortgageID
        {
            set { dataRow[MORTGAGEIDFIELDNAME] = value; }
            get { return Convert.ToInt32(dataRow[MORTGAGEIDFIELDNAME]); }
        }
        public string Description
        {
            set { dataRow[DESCRIPTIONFIELDNAME] = value; }
            get { return Convert.ToString(dataRow[DESCRIPTIONFIELDNAME]); }
        }
        public bool IsActive
        {
            get { return Convert.ToBoolean(dataRow[ISACTIVEFIELDNAME]); }
        }
        public DateTime Created
        {
            get { return Convert.ToDateTime(dataRow[CREATEDFIELDNAME]); }
        }
        #endregion

        #region constructors
        public Alert()
	    {
	    }
        public Alert(int id)
        {
            ID = id;
            if (ID > 0)
            {
                DataTable dt = db.GetDataTable(GETALERT, ID);
                if (dt.Rows.Count > 0)
                {
                    dataRow = dt.Rows[0];
                    ID = Convert.ToInt32(dataRow[IDFIELDNAME]);
                }
                else
                    dataRow = dt.NewRow();
            }
        }
        #endregion
    }
}
