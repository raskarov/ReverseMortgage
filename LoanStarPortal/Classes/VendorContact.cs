using System;
using System.Data;

namespace LoanStar.Common
{
    public class VendorContact : BaseObject
    {

        #region constants
        private const string ORIGINATORIDFIELDNAME = "originatorId";
        private const string VENDORIDFIELDNAME = "vendorId";
        private const string NAMEFIELDNAME = "name";
        private const string PHONEFIELDNAME = "phone";
        private const string ALTPHONEFIELDNAME = "altphone";
        private const string CELLPHONEFIELDNAME = "cellphone";
        private const string FAXFIELDNAME = "fax";
        private const string EMAILFIELDNAME = "Email";
        private const string ADDRESSFIELDNAME = "Address";
        private const string ISSETTLEMENTAGENTFIELDNAME = "IsSettlementAgent";
        private const string ISTITLEAGENTFIELDNAME = "IsTitleAgent";

        private const string GETVENDORCONTACTS = "GetVendorContacts";
        private const string LOADBYID = "LoadVendorContactById";
        private const string DELETE = "DeleteVendorContact";
        private const string SAVE = "SaveVendorContact";
        #endregion

        #region fields
        private int originatorId;
        private int vendorId;
        private string name;
        private string phone;
        private string altPhone;
        private string fax;
        private string cellPhone;
        private string address;
        private bool isSettlementAgent=false;
        private bool isTitleAgent = false;
        private string email;
        #endregion

        #region properties
        public int OriginatorId
        {
            get { return originatorId; }
            set { originatorId = value; }
        }
        public int VendorId
        {
            get { return vendorId; }
            set { vendorId = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value;}
        }
        public string Phone
        {
            get { return phone; }
            set { phone = value;}
        }
        public string AltPhone
        {
            get { return altPhone; }
            set { altPhone = value;}
        }
        public string Fax
        {
            get { return fax; }
            set { fax = value;}
        }
        public string CellPhone
        {
            get { return cellPhone; }
            set { cellPhone = value;}
        }
        public string Address
        {
            get { return address; }
            set 
            {
                if (value.Length > 512)
                {
                    address = value.Substring(0, 512);
                }
                else
                {
                    address = value;
                }
            }
        }
        public bool IsSettlementAgent
        {
            get { return isSettlementAgent; }
            set { isSettlementAgent = value;}
        }
        public bool IsTitleAgent
        {
            get { return isTitleAgent; }
            set { isTitleAgent = value;}
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        #endregion

        #region constructors
        public VendorContact(int id)
        {
            ID = id;
            if (ID > 0)
            {
                LoadById();
            }
        }
        public VendorContact(DataRowView row)
        {
            ID = ConvertToInt(row["id"], -1);
            LoadFromDataRow(row);
        }

        #endregion

        #region methods
        public int Save()
        {
            int res = db.ExecuteScalarInt(SAVE, ID
                            , originatorId
                            , vendorId
                            , name
                            , phone
                            , altPhone
                            , cellPhone
                            , fax
                            , address
                            , email
                            , isSettlementAgent
                            , isTitleAgent
                        );
            if(res>0)
            {
                ID = res;
            }
            return res;
        }
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
            originatorId = Convert.ToInt32(dr[ORIGINATORIDFIELDNAME]);
            vendorId = Convert.ToInt32(dr[VENDORIDFIELDNAME]);
            name = dr[NAMEFIELDNAME].ToString();
            phone = dr[PHONEFIELDNAME].ToString();
            altPhone = dr[ALTPHONEFIELDNAME].ToString();
            cellPhone = dr[CELLPHONEFIELDNAME].ToString();
            fax = dr[FAXFIELDNAME].ToString();
            email = dr[EMAILFIELDNAME].ToString();
            address = dr[ADDRESSFIELDNAME].ToString();
            isSettlementAgent = bool.Parse(dr[ISSETTLEMENTAGENTFIELDNAME].ToString());
            isTitleAgent = bool.Parse(dr[ISTITLEAGENTFIELDNAME].ToString());
        }
        #region static methods
        public static bool Delete(int id)
        {
            return db.ExecuteScalarInt(DELETE, id) == 1;
        }
        public static DataView GetVendorContacts(int originatorId, int vendorId)
        {
            return db.GetDataView(GETVENDORCONTACTS, originatorId, vendorId);
        }
        #endregion
        #endregion
    }
}
