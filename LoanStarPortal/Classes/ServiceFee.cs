using System.Data;

namespace LoanStar.Common
{
    public class ServiceFee : BaseObject
    {
        #region constants
        private const string GETSERVICEFEE = "GetServiceFee";
        private const string GETSERVICEFEEFORPRODUCT = "GetServiceFeeForProduct";
        private const string LOADSERVICEFEEBYID = "LoadServiceFeeById";
        private const string RESTOREGLOBALSERVICEFEEFORPRODUCT = "RestoreGlobalServiceFeeForProduct";
        private const string SAVE = "SaveServiceFee";
        private const string DELETE = "DeleteServiceFee";
        private const string GETALLDEFAULTS = "GetAllDefaultServiceFee";
        private const string FEEFIELDNAME = "fee";
        private const string ISDEFAULTFIELDNAME = "isdefault";
        private const string COMPANYIDFIELDNAME = "companyid";
        private const string PRODUCTIDFIELDNAME = "productid";
        #endregion

        #region fields
        private decimal fee = 0;
        private bool isDefault = false;
        private int companyId;
        private int productId;
        #endregion

        #region properties
        public decimal Fee
        {
            get { return fee; }
            set { fee = value; }
        }
        public bool IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }
        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }
        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }
        #endregion

        #region constructor
        public ServiceFee(int _id)
        {
            ID = _id;
            LoadById();
        }
        #endregion

        #region methods
        public int Save()
        {
            return db.ExecuteScalarInt(SAVE, ID, fee, isDefault, companyId, productId);
        }
        public static bool Delete(int id)
        {
            return db.ExecuteScalarInt(DELETE, id) == 1;
        }
        private void LoadById()
        {
            if (ID > 0)
            {
                DataView dv = db.GetDataView(LOADSERVICEFEEBYID, ID);
                if (dv.Count == 1)
                {
                    fee = decimal.Parse(dv[0][FEEFIELDNAME].ToString());
                    isDefault = bool.Parse(dv[0][ISDEFAULTFIELDNAME].ToString());
                    companyId = int.Parse(dv[0][COMPANYIDFIELDNAME].ToString());
                    productId = int.Parse(dv[0][PRODUCTIDFIELDNAME].ToString());
                }
            }
        }
        public static DataView GetServiceFee(int companyId)
        {
            return db.GetDataView(GETSERVICEFEE, companyId);
        }
        public static DataView GetServiceFeeForProduct(int companyId, int productId)
        {
            return db.GetDataView(GETSERVICEFEEFORPRODUCT, companyId, productId);
        }
        public static bool RestoreGlobalServiceFeeForProduct(int companyId, int productId)
        {
            return db.ExecuteScalarInt(RESTOREGLOBALSERVICEFEEFORPRODUCT, companyId, productId)>1;
        }
        public static DataView GetAllDefaultServiceFee()
        {
            return db.GetDataView(GETALLDEFAULTS);
        }
        #endregion

        

    }
}
