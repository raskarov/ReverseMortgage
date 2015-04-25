using System;
using System.Data;

namespace LoanStar.Common
{
    public class Servicer : BaseObject
    {
        #region constants
        private const string GETSERVICERBYCOMPANYIDID = "LoadServicerByCompanyId";
        private const string SAVE = "SaveServicer";
        #endregion

        #region fields
        private readonly int companyId = 0;
        private string name = String.Empty;
        private string baydocServicerId = String.Empty;
        private string address1 = String.Empty;
        private string address2 = String.Empty;
        private string city = String.Empty;
        private int stateId = 0;
        private string zip = String.Empty;
        private string phone = String.Empty;
        private string fax = String.Empty;
        private int locationId = 0;
        #endregion

        #region properties
        public int LocationId
        {
            get { return locationId; }
            set { locationId = value; }
        }
        public string Name
        { 
            get { return name; }
            set { name = value;}
        }
        public string BaydocServicerId
        {
            get { return baydocServicerId; }
            set { baydocServicerId = value; }
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
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        #endregion

        #region constructor
        public Servicer(int companyId_)
        {
            if(companyId_>0)
            {
                companyId = companyId_;
                LoadByCompanyId();
            }
        }

        #endregion

        #region methods
        public int Save()
        {
            int res = 
                    db.ExecuteScalarInt(SAVE, ID
                                       , companyId 
                                       , name
                                       , baydocServicerId
//                                       , address1
//                                       , address2
//                                       , city
//                                       , stateId
//                                       , zip
                                       , phone
                                       , fax
                                       , locationId
                );
            if(res > 0)
            {
                ID = res;
            }
            return res;
        }

        private void LoadByCompanyId()
        {
            DataView dv = db.GetDataView(GETSERVICERBYCOMPANYIDID, companyId);
            if(dv.Count==1)
            {
                ID =  int.Parse(dv[0]["id"].ToString());
                name =  dv[0]["name"].ToString();
                baydocServicerId =  dv[0]["baydocServicerId"].ToString();
                address1 =  dv[0]["address1"].ToString();
                address2 =  dv[0]["address2"].ToString();
                city =  dv[0]["city"].ToString();
                stateId =  int.Parse(dv[0]["stateId"].ToString());
                zip =  dv[0]["zip"].ToString();
                phone =  dv[0]["phone"].ToString();
                fax = dv[0]["fax"].ToString();
                locationId = int.Parse(dv[0]["locationId"].ToString());
            }
            else
            {
                ID = -1;
            }
        }

        #endregion
    }
}
