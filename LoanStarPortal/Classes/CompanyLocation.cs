using System;
using System.Data;

namespace LoanStar.Common
{
    public class CompanyLocation : BaseObject
    {
        #region constants
        private const string GETLOCATIONTREE = "GetCompanyLocationTree";
        private const string GETLOCATIONBYID = "GetLocationById";
        private const string GETCOMPANYLOCATIONLIST = "GetCompanyLocationList";
        private const string SAVE = "SaveLocation";
        private const string DELETE = "DeleteLocation";
        #endregion

        #region fields
        private int companyId = 0;
        private int parentLocationId = 0;
        private string name = String.Empty ;
        private string address1 = String.Empty;
        private string address2 = String.Empty;
        private string city = String.Empty;
        private string zip = String.Empty;
        private int stateId = 0;
        private string customField1 = String.Empty;
        private string customField2 = String.Empty;
        private string customField3 = String.Empty;
        private string customField4 = String.Empty;
        private string customField5 = String.Empty;
        private string customField6 = String.Empty;
        private string customField7 = String.Empty;
        private string customField8 = String.Empty;
        private string customField9 = String.Empty;
        private string customField10 = String.Empty;
        #endregion

        #region properties
        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }
        public int ParentLocationId
        {
            get { return parentLocationId; }
            set {parentLocationId = value;}
        }
        public string Name
        {
            get { return  name; }
            set {  name= value;}
        }
        public string Address1
        {
            get { return address1; }
            set { address1 = value;}
        }
        public string Address2
        {
            get { return address2; }
            set { address2 = value;}
        }
        public string City
        {
            get { return city; }
            set { city = value;}
        }
        public string Zip
        {
            get { return zip; }
            set { zip = value;}
        }
        public int StateId
        {
            get { return stateId; }
            set { stateId = value;}
        }
        public string CustomField1
        {
            get { return customField1; }
            set { customField1 = value;}
        }
        public string CustomField2
        {
            get { return customField2; }
            set { customField2 = value;}
        }
        public string CustomField3
        {
            get { return customField3; }
            set { customField3 = value;}
        }
        public string CustomField4
        {
            get { return customField4; }
            set { customField4 = value;}
        }
        public string CustomField5
        {
            get { return customField5; }
            set { customField5 = value;}
        }
        public string CustomField6
        {
            get { return customField6; }
            set { customField6 = value;}
        }
        public string CustomField7
        {
            get { return customField7; }
            set { customField7 = value;}
        }
        public string CustomField8
        {
            get { return customField8; }
            set { customField8 = value;}
        }
        public string CustomField9
        {
            get { return customField9; }
            set { customField9 = value;}
        }
        public string CustomField10
        {
            get { return customField10; }
            set { customField10 = value; }
        }
        #endregion

        #region constructors
        public CompanyLocation(int id)
        {
            ID = id;
            if(ID>0)
            {
                LoadById();
            }
        }
        #endregion

        #region methods

        #region public
        public int Save()
        {
            int res = db.ExecuteScalarInt(SAVE
                                          , ID
                                          , companyId
                                          , parentLocationId
                                          , name
                                          , address1
                                          , address2
                                          , city
                                          , stateId
                                          , zip
                                          , customField1
                                          , customField2
                                          , customField3
                                          , customField4
                                          , customField5
                                          , customField6
                                          , customField7
                                          , customField8
                                          , customField9
                                          , customField10
                );
            if(res>0)
            {
                ID = res;
            }
            return res;
        }
        
        #endregion

        #region private
        private void LoadById()
        {
            DataView dv = db.GetDataView(GETLOCATIONBYID, ID);
            if(dv.Count==1)
            {
                companyId = int.Parse(dv[0]["companyid"].ToString());
                name = dv[0]["name"].ToString();
                address1 = dv[0]["address1"].ToString();
                address2 = dv[0]["address2"].ToString();
                city = dv[0]["city"].ToString();
                stateId = int.Parse(dv[0]["stateId"].ToString());
                zip = dv[0]["zip"].ToString();
                parentLocationId = int.Parse(dv[0]["parentLocationId"].ToString());
                customField1 = dv[0]["customField1"].ToString();
                customField2 = dv[0]["customField2"].ToString();
                customField3 = dv[0]["customField3"].ToString();
                customField4 = dv[0]["customField4"].ToString();
                customField5 = dv[0]["customField5"].ToString();
                customField6 = dv[0]["customField6"].ToString();
                customField7 = dv[0]["customField7"].ToString();
                customField8 = dv[0]["customField8"].ToString();
                customField9 = dv[0]["customField9"].ToString();
                customField10 = dv[0]["customField10"].ToString();
            }
            else
            {
                ID = -1;
            }
        }
        #endregion

        #region static
        public static int Delete(int id)
        {
            return db.ExecuteScalarInt(DELETE, id);
        }
        public static DataView GetLocationTree(int companyId)
        {
            return db.GetDataView(GETLOCATIONTREE, companyId);
        }
        public static DataView GetCompanyLocationList(int companyId)
        {
            return db.GetDataView(GETCOMPANYLOCATIONLIST, companyId);
        }

        #endregion

        #endregion

    }
}
