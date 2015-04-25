using System;
using System.Collections.Generic;
using System.Data;

namespace LoanStar.Common
{
    public class MortgagePrepaidItem : BaseObject
    {
        public class MortgagePrepaidItemException : BaseObjectException
        {
            public MortgagePrepaidItemException(string message, Exception innerException)
                : base(message, innerException)
            {
            }
            public MortgagePrepaidItemException(string message)
                : base(message)
            {
            }
            public MortgagePrepaidItemException()
            {
            }
        }
        #region constants
        #region sp names
        private const string LOADBYID = "LoadPrepaidItemById";
        private const string GETGRIDLIST = "GetPrepaidItemListForGrid";
        private const string GETUNITLIST = "GetAdvancePaymentUnitList";
        #endregion
        #region fields names
        private const string IDFIELDNAME = "id";
        private const string MORTGAGEIDFIELDNAME = "mortgageid";
        private const string DESCRIPTIONFIELDNAME = "description";
        private const string AMOUNTFIELDNAME = "amount";
        private const string PAYMENTTOFIELDNAME = "paymentto";
        private const string UNITIDFIELDNAME = "unitId";
        private const string UNITFIELDNAME = "unit";
        private const string STATEMENTSTARTFIELDNAME = "StatementStart";
        private const string STATEMENTENDFIELDNAME = "StatementEnd";
        #endregion
        #endregion

        #region fields
        private int mortgageId = -1;
        private string description = String.Empty;
        private string paymentTo = String.Empty;
        private decimal amount = 0;
        private int unitId = -1;
        private string unit = String.Empty;
        private DateTime? statementStart;
        private DateTime? statementEnd;
        #endregion

        #region properties
        public int MortgageId
        {
            get { return mortgageId; }
            set { mortgageId = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string PaymentTo
        {
            get { return paymentTo; }
            set { paymentTo = value; }
        }
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public int UnitId
        {
            get { return unitId; }
            set { unitId = value; }
        }
        public string Unit
        {
            get { return unit; }
        }
        public DateTime? StatementStart
        {
            get { return statementStart; }
            set { statementStart = value; }
        }
        public DateTime? StatementEnd
        {
            get { return statementEnd; }
            set { statementEnd = value; }
        }

        #endregion

        #region constructors
        public MortgagePrepaidItem():this(-1)
        {
        }
        public MortgagePrepaidItem(int id)
        {
            ID = id;
            if (ID>0)
            {
                LoadById();
            }
        }
        public MortgagePrepaidItem(DataRowView dr)
        {
            LoadFromDataRow(dr);
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
            description = dr[DESCRIPTIONFIELDNAME].ToString();
            paymentTo = dr[PAYMENTTOFIELDNAME].ToString();
            amount = Convert.ToDecimal(dr[AMOUNTFIELDNAME]);
            unitId = Convert.ToInt32(dr[UNITIDFIELDNAME].ToString());
            unit = dr[UNITFIELDNAME].ToString();
            if (dr[STATEMENTSTARTFIELDNAME] != DBNull.Value)
            {
                statementStart = Convert.ToDateTime(dr[STATEMENTSTARTFIELDNAME].ToString());
            }
            if (dr[STATEMENTENDFIELDNAME] != DBNull.Value)
            {
                statementEnd = Convert.ToDateTime(dr[STATEMENTENDFIELDNAME].ToString());
            }
        }
        public static DataView GetPrepaidItemsForGrid(int MortgageID)
        {
            return db.GetDataView(GETGRIDLIST, MortgageID);
        }
        public static DataView GetUnitList()
        {
            return db.GetDataView(GETUNITLIST);
        }

        public static List<MortgagePrepaidItem> GetObjectList(int mortgageId)
        {
            DataView dv = GetPrepaidItemsForGrid(mortgageId);

            List<MortgagePrepaidItem> res = new List<MortgagePrepaidItem>();
            foreach (DataRowView dr in dv)
            {
                MortgagePrepaidItem item = new MortgagePrepaidItem(dr);
                res.Add(item);
            }

            return res;
        }

        #endregion
    }
}
