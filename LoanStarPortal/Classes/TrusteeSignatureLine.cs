using System;
using System.Data;

namespace LoanStar.Common
{
    public class TrusteeSignatureLine : BaseObject
    {
        #region constants
        private const string GETTRUSTEESIGNATURELINEBYID = "GetTrusteeSignatureLineById";
        private const string GETTRUSTEESIGNATURELINES = "GetTrusteeSignatureLinesForGrid";
        private const string DELETE = "DeleteSignatureLine";
        private const string SAVE = "SaveSignatureLine";
        #endregion

        #region fields
        private int mortgageId = -1;
        private string signatureLine = String.Empty;
        #endregion

        #region properties
        public int MortgageId
        {
            get { return mortgageId; }
            set { mortgageId = value; }
        }
        public string SignatureLine
        {
            get { return signatureLine; }
            set { signatureLine = value; }
        }
        #endregion

        #region constructor
        public TrusteeSignatureLine(int mortgageId_, int id)
        {
            mortgageId = mortgageId_;
            ID = id;
            if (id > 0)
            {
                LoadById();
            }
        }
        public TrusteeSignatureLine(DataRowView row)
        {
            LoadFromDataRow(row);
        }

        #endregion

        #region methods
        public int Save()
        {
            int res = db.ExecuteScalarInt(SAVE, ID,mortgageId, signatureLine);
            if (res > 0)
            {
                ID = res;
            }
            return res;

        }

        private void LoadById()
        {
            DataView dv = db.GetDataView(GETTRUSTEESIGNATURELINEBYID, ID);
            if (dv.Count == 1)
            {
                LoadFromDataRow(dv[0]);
            }
            else
            {
                ID = -1;
            }
        }
        private void LoadFromDataRow(DataRowView dr)
        {
            ID = int.Parse(dr["id"].ToString());
            mortgageId = int.Parse(dr["mortgageId"].ToString());
            signatureLine = dr["signatureLine"].ToString();
        }
        public static DataView GetSignatureLinesForGrid(int mortgageId)
        {
            return db.GetDataView(GETTRUSTEESIGNATURELINES, mortgageId);
        }
        public static bool Delete(int id)
        {
            return db.ExecuteScalarInt(DELETE, id) == 1;
        }
        #endregion
    }

}
