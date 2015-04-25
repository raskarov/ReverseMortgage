using System;
using System.Data;



namespace LoanStar.Common
{
    public class PropertyRepairItem : BaseObject
    {
        public class PropertyRepairItemException : BaseObjectException
        {
            public PropertyRepairItemException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public PropertyRepairItemException(string message)
                : base(message)
            {
            }

            public PropertyRepairItemException()
            {
            }
        }

        #region constants
        private const string GETREPAIRITEMLIST = "GetRepairItemList";
        private const string LOADBYID = "LoadRepairItemById";
        private const string IDFIELDNAME = "id";
        private const string PROPERTYIDFIELDNAME = "propertyid";
        private const string DESCRIPTIONFIELDNAME ="description";
        private const string BIDAMOUNTFIELDNAME = "bidamount";
        private const string REPAIRSTATUSIDFIELDNAME = "RepairStatusId";
        private const string ESTIMATESOURCEIDFIELDNAME = "EstimateSourceId";
        #endregion

        #region fields
        private int propertyId = -1;
        private string description;
        private decimal bidAmount = 0;
        private int repairStatusId = 0;
        private int estimateSourceId = 0;
        #endregion

        #region properties
        public int PropertyId
        {
            get { return propertyId; }
            set { propertyId = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public decimal BidAmount
        {
            get { return bidAmount; }
            set { bidAmount = value; }
        }
        public int RepairStatusId
        {
            get { return repairStatusId; }
            set { repairStatusId = value; }
        }
        public int EstimateSourceId
        {
            get { return estimateSourceId; }
            set { estimateSourceId = value; }
        }
        #endregion

        #region constructor
        public PropertyRepairItem():this(-1)
        {
        }
        public PropertyRepairItem(int id)
        {
            ID = id;
            if (ID>0)
            {
                LoadById();
            }
        }
        public PropertyRepairItem(DataRowView dr)
        {
            if (dr != null)
            {
                LoadFromDataRow(dr);
            }
        }
        #endregion

        #region methods
        private void LoadById()
        {
            DataView dv = db.GetDataView(LOADBYID, ID);
            if (dv.Count == 1)
            {
                LoadFromDataRow(dv[0]);
            }
        }
        private void LoadFromDataRow(DataRowView dr)
        {
            ID = Convert.ToInt32(dr[IDFIELDNAME]);
            propertyId = Convert.ToInt32(dr[PROPERTYIDFIELDNAME]);
            description = dr[DESCRIPTIONFIELDNAME].ToString();
            bidAmount = Convert.ToDecimal(dr[BIDAMOUNTFIELDNAME]);
            repairStatusId = Convert.ToInt32(dr[REPAIRSTATUSIDFIELDNAME]);
            estimateSourceId = Convert.ToInt32(dr[ESTIMATESOURCEIDFIELDNAME]);
        }
        public static DataView GetRepairItemsList(int propertyId)
        {
            return db.GetDataView(GETREPAIRITEMLIST, propertyId);
        }
        public static Decimal GetTotalRepairs(int propertyId)
        {
            string res = db.ExecuteScalarString("GetTotalRepairs", propertyId);
            if (!String.IsNullOrEmpty(res)) return Convert.ToDecimal(res);
            else return 0;
        }
        public static DataView GetRepairStatusList()
        {
            return db.GetDataView("GetRepairStatusList");
        }
        public static DataView GetEstimateSourceList()
        {
            return db.GetDataView("GetEstimateSourceList");
        }
        #endregion


    }
}
