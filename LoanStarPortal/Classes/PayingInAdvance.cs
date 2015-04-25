using System;
using System.Data;

namespace LoanStar.Common
{
    public class PayingInAdvance : BaseObject
    {
        public class PayingInAdvanceException : BaseObjectException
        {
            public PayingInAdvanceException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public PayingInAdvanceException(string message)
                : base(message)
            {
            }

            public PayingInAdvanceException()
            {
            }
        }

        #region constants
        #region sp names
        private const string LOADBYID = "LoadPayingInAdvanceById";
        private const string GETGRIDLIST = "GetPaymentListForGrid";
        private const string GETLIST = "GetPaymentList";
        private const string GETDESCRIPTIONLIST = "GetDescriptionList";
        private const string GETUNITLIST = "GetAdvancePaymentUnitList";
        private const string GETTOTAL = "GetPayinfInAdvanceTotal";
        #endregion

        #region fields names
        private const string IDFIELDNAME = "id";
        private const string MORTGAGEIDFIELDNAME = "mortgageid";
        private const string DESCRIPTIONIDFIELDNAME = "descriptionId";
        private const string DESCRIPTIONFIELDNAME = "description";
        private const string AMOUNTFIELDNAME = "amount";
        private const string PAYINGTOFIELDNAME = "payingto";
        private const string UNITIDFIELDNAME = "unitId";
        private const string UNITFIELDNAME = "unit";
        #endregion

        #endregion

        #region fields
        private int mortgageId = -1;
        private int descriptionId = -1;
        private string description = String.Empty;
        private string payingTo = String.Empty;
        private decimal amount = 0;
        private int unitId = -1;
        private string unit = String.Empty;
        #endregion

        #region properties
        public int MortgageId
        {
            get { return mortgageId; }
            set { mortgageId = value; }
        }
        public int DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string PayingTo
        {
            get { return payingTo; }
            set { payingTo = value ; }
        }
        public decimal  Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public int UnitId
        {
            get { return unitId; }
            set { unitId = value; }
        }
        #endregion

        #region constructors
        public PayingInAdvance():this(-1)
        {
        }
        public PayingInAdvance(int id)
        {
            ID = id;
            if (ID>0)
            {
                LoadById();
            }
        }
        public PayingInAdvance(DataRowView dr)
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
            mortgageId = Convert.ToInt32(dr[MORTGAGEIDFIELDNAME]);
            descriptionId = Convert.ToInt32(dr[DESCRIPTIONIDFIELDNAME].ToString());
            description = dr[DESCRIPTIONFIELDNAME].ToString();
            payingTo = dr[PAYINGTOFIELDNAME].ToString();
            amount = Convert.ToDecimal(dr[AMOUNTFIELDNAME]);
            unitId = Convert.ToInt32(dr[UNITIDFIELDNAME].ToString());
            unit = dr[UNITFIELDNAME].ToString();
        }

        public static DataView GetPayingInAdvanceForGrid(int MortgageID)
        {
            return db.GetDataView(GETGRIDLIST, MortgageID);
        }
        public static DataView GetPayingInAdvance(int MortgageID)
        {
            return db.GetDataView(GETLIST, MortgageID);
        }
        public static DataView GetDecriptionList()
        {
            return db.GetDataView(GETDESCRIPTIONLIST);
        }
        public static DataView GetUnitList()
        {
            return db.GetDataView(GETUNITLIST);
        }
        public static decimal GetPayinfInAdvanceTotal(int MortgageID)
        {
            decimal total = 0;
            DataRow dr = db.GetSingleRow(GETTOTAL, MortgageID);
            if (dr != null)
                total = Convert.ToDecimal(dr["Total"]);
            return total;
        }
        #endregion
    }
}
