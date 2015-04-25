using System.Data;

namespace LoanStar.Common
{
    public class Investor : BaseObject
    {
        #region constants
        private const string LOADBYCOMPANYID = "LoadInvestorByCompanyId";
        private const string SAVE = "SaveInvestor";
        #endregion

        #region fields
        private string address1;
        private string address2;
        private string city;
        private string name;
        private int stateId = 0;
        private string zip;
        private int companyId = 0;
        private int locationId = 0;
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
        #endregion

        #region constructor
        public Investor()
        {
        }
        public Investor(int _companyId)
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

