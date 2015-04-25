using System;
using System.Collections.Generic;
using System.Data;

namespace LoanStar.Common
{
    public class Payoff : BaseObject
    {
        /// <summary>
        /// Any method of Invoice class must throw custom exceptions of this type only
        /// </summary>
        public class PayoffException : BaseObjectException
        {
            public PayoffException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public PayoffException(string message)
                : base(message)
            {
            }

            public PayoffException()
            {
            }
        }

        #region constants
        
        #region sp names
        private const string GETPAYOFFSTATUSLIST = "GetPayoffStatusList";
        private const string GETPAYOFFLISTFORGRID = "GetPayoffListForGrid";
        private const string GETUPDATESTATUS = "GetPayoffUpdateNeeded";
        private const string SETSTATUS = "SetPayoffStatus";
        private const string LOADBYID = "LoadPayoffById";
        private const string SAVE = "SavePayoff";
        private const string DELETE = "DeletePayoff";
        #endregion

        #region fields names
        private const string MORTGAGEIDFIELDNAME = "mortgageid";
        private const string CREADITORFIELDNAME = "creditor";
        private const string PAYOFFSTATUSIDFIELDNAME = "payoffstatusid";
        private const string PAYOFFCALCULATEDSTATUSIDFIELDNAME = "payoffcalculatedstatusid";
        private const string AMOUNTFIELDNAME = "amount";
        private const string POCFIELDNAME = "poc";
        private const string PERDIEMFIELDNAME = "perdiem";
        private const string EXPDATEFIELDNAME = "expdate";
        private const string ORDEREDDATEFIELDNAME = "OrderedDate";
        private const string UPDATEORDEREDDATEFIELDNAME = "UpdateOrderedDate";
        private const string ADDRESS1FIELDNAME = "address1";
        private const string ADDRESS2FIELDNAME = "address2";
        private const string CITYFIELDNAME = "city";
        private const string ZIPFIELDNAME = "zip";
        private const string STATEIDFIELDNAME = "stateid";
        private const string ACCOUNTNUMBERFIELDNAME = "AccountNumber";

        #endregion

        public const int NEEDTOORDERSTATUSID = 1;
        public const int ORDEREDSTATUSID = 2;
        public const int RECEIVEDSTATUSID = 3;

        #endregion

        #region fields
        private int mortgageid;
        private string creditor;
        private int payoffstatusid;
        private int payoffcalculatedstatusid; 
        private decimal amount;
        private decimal perdiem;
        private DateTime? expdate;
        private DateTime? ordereddate;
        private DateTime? updateordereddate;
        private int createdby;
        private DateTime created;
        private string address1;
        private string address2;
        private string city;
        private string zip;
        private string accountNumber;
        private int stateId = 0;
        private decimal poc = 0;
        #endregion

        #region  properties
        public int MortgageID
        {
            set { mortgageid = value; }
            get { return mortgageid; }
        }
        public string Creditor
        {
            set { creditor = value; }
            get { return creditor; }
        }
        public int PayoffStatusId
        {
            set { payoffstatusid = value; }
            get { return payoffstatusid; }
        }
        public int PayoffCalculatedStatusId
        {
            get { return payoffcalculatedstatusid; }
        }
        public decimal Amount
        {
            set { amount = value; }
            get { return amount; }
        }
        public decimal Financed
        {
            get { return amount - poc; }
        }
        public decimal Perdiem
        {
            set { perdiem = value; }
            get { return perdiem; }
        }

        public DateTime? ExpDate
        {
            set { expdate = value; }
            get { return expdate; }
        }
        public DateTime? OrderedDate
        {
            get { return ordereddate; }
        }
        public DateTime? UpdateOrderedDate
        {
            get { return updateordereddate; }
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
        public string Address1
        {
            get { return address1; }
            set { address1 = value; }
        }
        public string Address2
        {
            get { return address2; }
            set { address2 = value; }
        }
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        public int StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }
        public string Zip
        {
            get { return zip; }
            set { zip = value; }
        }
        public string AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = value; }
        }
        public decimal POC
        {
            get { return poc; }
            set { poc = value; }
        }
        #endregion


        #region constructor
        public Payoff():this(-1)
        {
        }
        public Payoff(int id)
        {
            ID = id;
            if (ID > 0)
            {
                LoadById();
            }
        }
        public Payoff(DataRowView row)
        {
            ID = ConvertToInt(row["id"], -1);
            LoadFromDataRow(row);
        }
        #endregion

        #region methods

        #region public
        public int Save()
        {
            int res = db.ExecuteScalarInt(SAVE
                                        ,ID
                                        ,mortgageid
                                        ,creditor
                                        ,payoffstatusid
                                        ,amount
                                        ,poc
                                        ,perdiem
                                        ,expdate ?? (object)DBNull.Value
                                        ,createdby);
            if (res > 0)
            {
                ID = res;
            }
            return res;
        }
        public bool SetStatus(int statusId)
        {
            return db.ExecuteScalarInt(SETSTATUS, ID, statusId) == 1;
        }
        #endregion

        #region private
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
            mortgageid = Convert.ToInt32(dr[MORTGAGEIDFIELDNAME]);
            creditor = dr[CREADITORFIELDNAME].ToString();
            if (!String.IsNullOrEmpty(dr[PAYOFFSTATUSIDFIELDNAME].ToString()))
                payoffstatusid = Convert.ToInt32(dr[PAYOFFSTATUSIDFIELDNAME]);
            else
                payoffstatusid = 1;
            if (!String.IsNullOrEmpty(dr[PAYOFFCALCULATEDSTATUSIDFIELDNAME].ToString()))
                payoffcalculatedstatusid = Convert.ToInt32(dr[PAYOFFCALCULATEDSTATUSIDFIELDNAME]);
            else
                payoffcalculatedstatusid = 1;

            amount = Convert.ToDecimal(dr[AMOUNTFIELDNAME]);
            poc = Convert.ToDecimal(dr[POCFIELDNAME]);
            perdiem = Convert.ToDecimal(dr[PERDIEMFIELDNAME]);
            if (dr[EXPDATEFIELDNAME] != DBNull.Value)
            {
                expdate = Convert.ToDateTime(dr[EXPDATEFIELDNAME]);
            }
            if (dr[ORDEREDDATEFIELDNAME] != DBNull.Value)
            {
                ordereddate = Convert.ToDateTime(dr[ORDEREDDATEFIELDNAME]);
            }
            if (dr[UPDATEORDEREDDATEFIELDNAME] != DBNull.Value)
            {
                updateordereddate = Convert.ToDateTime(dr[UPDATEORDEREDDATEFIELDNAME]);
            }
            address1 = dr[ADDRESS1FIELDNAME].ToString();
            address2 = dr[ADDRESS2FIELDNAME].ToString();
            city = dr[CITYFIELDNAME].ToString();
            zip = dr[ZIPFIELDNAME].ToString();
            accountNumber = dr[ACCOUNTNUMBERFIELDNAME].ToString();
            if (!String.IsNullOrEmpty(dr[STATEIDFIELDNAME].ToString()))
            {
                stateId = Convert.ToInt32(dr[STATEIDFIELDNAME]);
            }
        }
        #endregion

        #region Static methods
        public static bool Delete(int id)
        {
            return db.ExecuteScalarInt(DELETE, id) == 1;
        }
        public static DataView GetPayoffListForGrid(int MortgageID)
        {
            return db.GetDataView(GETPAYOFFLISTFORGRID, MortgageID);
        }
        public static DataView GetPayoffList(int MortgageID)
        {
            return db.GetDataView("GetPayoffList", MortgageID);
        }
        public static decimal GetPayoffTotal(int MortgageID)
        {
            decimal total = 0; 
            DataRow dr = db.GetSingleRow("GetPayoffTotal", MortgageID);
            if(dr!=null)
                total = Convert.ToDecimal(dr["Total"]);
            return total;
        }
        public static DataView GetPayOffStatusList()
        {
            return db.GetDataView(GETPAYOFFSTATUSLIST);
        }
        public static List<Payoff> GetPayoffObjectList(int MortgageID)
        {
            DataView payoffDV = db.GetDataView("GetPayoffsByMortgageID", MortgageID);

            List<Payoff> payoffList = new List<Payoff>();
            foreach (DataRowView payoffRow in payoffDV)
            {
                Payoff payoffObj = new Payoff(payoffRow);
                payoffList.Add(payoffObj);
            }

            return payoffList;
        }
        public static bool GetUpdateStatus(int MortgageId)
        {
            return db.ExecuteScalarBool(GETUPDATESTATUS, MortgageId);
        }

        #endregion

        #endregion



    }
}
