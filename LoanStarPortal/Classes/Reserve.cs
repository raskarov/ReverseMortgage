using System;
using System.Collections.Generic;
using System.Data;

namespace LoanStar.Common
{
    public class Reserve : BaseObject
    {
        /// <summary>
        /// Any method of Reserve class must throw custom exceptions of this type only
        /// </summary>
        public class ReserveException : BaseObjectException
        {
            public ReserveException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public ReserveException(string message)
                : base(message)
            {
            }

            public ReserveException()
            {
            }
        }


        #region fields
        private int mortgageid;
        private string description=String.Empty;
        private int typeId;
        private string typeName;
        private DateTime? statementStart;
        private DateTime? statementEnd;
        private decimal months;
        private decimal amount;
        private int chargetoid;
        private string chargetoname;
        private int createdby;
        private DateTime created;
        #endregion

        #region  properties
        public int MortgageID
        {
            set { mortgageid = value; }
            get { return mortgageid; }
        }
        public int TypeId
        {
            set { typeId = value; }
            get { return typeId; }
        }
        public string TypeName
        {
            set { typeName = value; }
            get { return typeName; }
        }
        public DateTime? StatementStart
        {
            set { statementStart = value; }
            get { return statementStart; }
        }
        public DateTime? StatementEnd
        {
            set { statementEnd = value; }
            get { return statementEnd; }
        }
        public string Description
        {
            set { description = value; }
            get { return description; }
        }
        public decimal Months
        {
            set { months = value; }
            get { return months; }
        }
        public decimal Amount
        {
            set { amount = value; }
            get { return amount; }
        }
        public int ChargeToId
        {
            set { chargetoid = value; }
            get { return chargetoid; }
        }
        public string ChargeToName
        {
            set { chargetoname = value; }
            get { return chargetoname; }
        }
        public int CreatedBy
        {
            set { createdby = value; }
            get { return createdby; }
        }
        public DateTime Created
        {
            set { created = value; }
            get { return created; }
        }
        #endregion

        #region constructors
        public Reserve()
            : this(-1)
        {
        }
        public Reserve(int id)
        {
            ID = id;
            if (id <= 0)
                return;
            DataView dv = db.GetDataView("GetReserveById", id);
            if (dv.Count == 1)
            {
                LoadByDataRow(dv[0]);
            }
        }
        public Reserve(DataRowView dr)
        {
            LoadByDataRow(dr);
        }
        #endregion

        #region methods
        private void LoadByDataRow(DataRowView dr)
        {
            ID = ConvertToInt(dr["ID"], -1);
            mortgageid = Convert.ToInt32(dr["MortgageID"]);
            typeId = Convert.ToInt32(dr["TypeId"]);
            typeName = dr["TypeName"].ToString();
            if (dr["StatementStart"] != DBNull.Value)
                statementStart = Convert.ToDateTime(dr["StatementStart"]);
            if (dr["StatementEnd"] != DBNull.Value)
                statementEnd = Convert.ToDateTime(dr["StatementStart"]);
            description = dr["Description"].ToString();
            months = Convert.ToDecimal(dr["Number"]);
            amount = Convert.ToDecimal(dr["Amount"]);
            chargetoid = Convert.ToInt32(dr["chargetoid"]);
            chargetoname = dr["ChargeName"].ToString();
            createdby = Convert.ToInt32(dr["CreatedBy"]);
            created = Convert.ToDateTime(dr["Created"]);
        }
        public int Save()
        {
            int res = db.ExecuteScalarInt("SaveReserve",
                                        ID,
                                        mortgageid,
                                        description,
                                        typeId,
                                        months,
                                        statementStart,
                                        statementEnd,
                                        amount,
                                        chargetoid,
                                        createdby);

            if (res <= 0)
                throw new ReserveException("Reserve was not updated succesfully");

            ID = (ID <= 0) ? res : ID;
            return res;
        }
        #endregion

        #region Static methods
        public static DataView GetReserveList(int MortgageID)
        {
            return db.GetDataView("GetReserveList", MortgageID);
        }

        public static DataView GetReserveTypeList()
        {
            return db.GetDataView("GetReserveTypeList");
        }

        public static List<Reserve> GetReserveObjectList(int MortgageID)
        {
            DataView reserverDV = GetReserveList(MortgageID);

            List<Reserve> reserveList = new List<Reserve>();
            foreach (DataRowView reserverRow in reserverDV)
            {
                Reserve reserverObj = new Reserve(reserverRow);
                reserveList.Add(reserverObj);
            }

            return reserveList;
        }

        public static decimal GetReserveTotalAmount(int mortgageID)
        {
            return ObjectConvert.ConvertToDecimal(db.ExecuteScalar("GetReserveTotalAmount", mortgageID), 0);
        }
        #endregion
    }
}