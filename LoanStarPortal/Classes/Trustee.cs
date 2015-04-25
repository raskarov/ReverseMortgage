using System.Data;

namespace LoanStar.Common
{
    public class Trustee : BaseObject 
    {
        #region constants
        private const string LOADBYCOMPANYID = "LoadTrusteeByCompanyId";
        private const string SAVE = "SaveTrustee";
        #endregion

        #region fields
        private string address1;
        private string address2;
        private string city;
        private string name;
        private int stateId;
        private string zip;
        private int companyId = 0;
        private int locationId = 0;
        private string stateCode;
        #endregion

        #region properties
        public int LocationId
        {
            get { return locationId; }
            set { locationId = value; }
        }
        public int CompanyID
        {
            get { return companyId; }
            set { companyId = value; }
        }
        public string Address
        {
            get { return address1 + " " + address2; }
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
        public string Name
        {
            get { return name; }
            set { name = value; }
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
        public string StateCode
        {
            get { return stateCode; }
            set { stateCode = value; }
        }
        #endregion

        #region constructor
        public Trustee()
        {
        }
        public Trustee(int _companyId)
        {
            companyId = _companyId;
            LoadByCompanyId();
        }
        #endregion

        #region methods
        public int Save()
        {
            return db.ExecuteScalarInt(SAVE, ID, companyId
//                , address1, address2, city, name, stateId, zip
                , locationId);
         }

        private void LoadByCompanyId()
        {
            DataView dv = db.GetDataView(LOADBYCOMPANYID, companyId);
            if (dv.Count == 1)
            {
                PopulateFromDataRow(dv[0]);
            }
            else
            {
                ID = -1;
            }
        }
        #endregion

    }
}
