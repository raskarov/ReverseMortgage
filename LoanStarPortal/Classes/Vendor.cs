using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace LoanStar.Common
{
//    public class Vendor : BaseObject
//    {
//        #region Private fields

//        private string companyName = String.Empty;
//        private string firstName = String.Empty;
//        private string lastName = String.Empty;
//        private string address = String.Empty;
//        private string phoneNumber = String.Empty;
//        private string altPhoneNumber = String.Empty;
//        private string faxNumber = String.Empty;
//        private string altFaxNumber = String.Empty;
//        private string mailAddress = String.Empty;
//        private string altMailAddress = String.Empty;
//        private string loginName = String.Empty;
//        private string password = String.Empty;
//        private bool isDisabled = false;
//        //private bool idDeleted = false;
//        private int companyID = -1;
////        private int vendorTypeId = -1;
////        private string vendorTypeName = String.Empty;
////        private bool emailRequired;
//        private Status companyStatus = Status.Unknown;
//        #endregion

//        #region Properties
//        public string CompanyName
//        {
//            get
//            {
//                return companyName;
//            }
//            set
//            {
//                companyName = value ?? String.Empty;
//            }
//        }
//        public string FirstName
//        {
//            get
//            {
//                return firstName;
//            }
//            set
//            {
//                firstName = value ?? String.Empty;
//            }
//        }

//        public string LastName
//        {
//            get
//            {
//                return lastName;
//            }
//            set
//            {
//                lastName = value ?? String.Empty;
//            }
//        }
//        public string Address
//        {
//            get
//            {
//                return address;
//            }
//            set
//            {
//                address = value ?? String.Empty;
//            }
//        }
//        public string PhoneNumber
//        {
//            get
//            {
//                return phoneNumber;
//            }
//            set
//            {
//                phoneNumber = value ?? String.Empty;
//            }
//        }
//        public string AltPhoneNumber
//        {
//            get
//            {
//                return altPhoneNumber;
//            }
//            set
//            {
//                altPhoneNumber = value ?? String.Empty;
//            }
//        }
//        public string FaxNumber
//        {
//            get
//            {
//                return faxNumber;
//            }
//            set
//            {
//                faxNumber = value ?? String.Empty;
//            }
//        }
//        public string AltFaxNumber
//        {
//            get
//            {
//                return altFaxNumber;
//            }
//            set
//            {
//                altFaxNumber = value ?? String.Empty;
//            }
//        }
//        public string MailAddress
//        {
//            get
//            {
//                return mailAddress;
//            }
//            set
//            {
//                mailAddress = value ?? String.Empty;
//            }
//        }
//        public string AltMailAddress
//        {
//            get
//            {
//                return altMailAddress;
//            }
//            set
//            {
//                altMailAddress = value ?? String.Empty;
//            }
//        }
//        public string LoginName
//        {
//            get
//            {
//                return loginName;
//            }
//            set
//            {
//                loginName = value ?? String.Empty;
//            }
//        }

//        public string Password
//        {
//            get
//            {
//                return password;
//            }
//            set
//            {
//                password = value ?? String.Empty;
//            }
//        }

//        public int CompanyID
//        {
//            get
//            {
//                return companyID;
//            }
//            set
//            {
//                companyID = value;
//            }
//        }
//        //public int VendorTypeId
//        //{
//        //    get
//        //    {
//        //        return vendorTypeId;
//        //    }
//        //    set
//        //    {
//        //        vendorTypeId = value;
//        //    }
//        //}
//        //public string VendorTypeName
//        //{
//        //    get
//        //    {
//        //        return vendorTypeName;
//        //    }
//        //    set
//        //    {
//        //        vendorTypeName = value ?? String.Empty;
//        //    }
//        //}
//        //public bool EmailRequired
//        //{
//        //    get
//        //    {
//        //        return emailRequired;
//        //    }
//        //    set
//        //    {
//        //        emailRequired = value;
//        //    }
//        //}
//        public bool IsDisabled
//        {
//            get
//            {
//                return isDisabled;
//            }
//            set
//            {
//                isDisabled = value;
//            }
//        }
//        //public bool IsDeleted
//        //{
//        //    get { return isDisabled; }
//        //}
//        public Status CompanyStatus
//        {
//            get { return companyStatus; }
//        }
//        #endregion

//        #region Constructors
//        public Vendor()
//        {
//        }

//        public Vendor(int vendorID)
//        {
//            ID = vendorID;
//            if (vendorID <= 0)
//                return;

//            DataTable tblVendor = db.GetDataTable("GetVendorByID", vendorID);
//            if (tblVendor.Rows.Count == 0)
//                return;

//            LoadFromDataRow(tblVendor.DefaultView[0]);
//        }
//        #endregion

//        #region Public methods
//        public bool Login(string _login, string _password, out string errMsg)
//        {
//            errMsg = String.Empty;
//            DataTable tblVendor = db.GetDataTable("GetVendorByLogin", _login);
//            if (tblVendor.Rows.Count == 0)
//            {
//                errMsg = "Wrong Login or Password";
//                return false;
//            }

//            LoadFromDataRow(tblVendor.DefaultView[0]);
//            if (Password != _password)
//            {
//                ID = -1;
//                errMsg = "Wrong Login or Password";
//            }
//            else if (IsDisabled)
//            {
//                ID = -1;
//                errMsg = "Vendor is disabled";
//            }
//            //else if (IsDeleted)
//            //{
//            //    ID = -1;
//            //    errMsg = "Vendor is deleted";
//            //}
//            /*else if (CompanyStatus != Status.Enabled)
//            {
//                ID = -1;
//                errMsg = String.Format("Company Status of Vendor is {0}", CompanyStatus);
//            }*/

//            return ID > 0;
//        }

//        public void Save(string xml)
//        {
//            int res = db.ExecuteScalarInt("SaveVendor",
//                                        ID,
//                                        CompanyName,
//                                        FirstName,
//                                        LastName,
//                                        Address,
//                                        PhoneNumber,
//                                        AltPhoneNumber,
//                                        FaxNumber,
//                                        AltFaxNumber,
//                                        MailAddress,
//                                        AltMailAddress,
//                                        LoginName,
//                                        Password,
//                                        CompanyID,
//                                        isDisabled,
//                                        xml);

//            ID = (ID <= 0) ? res : ID;
//        }
//        public void Delete()
//        {
//            int res = db.ExecuteScalarInt("DeleteVendor",ID,true);
//            ID = (ID <= 0) ? res : ID;
//        }
//        public void UnDelete()
//        {
//            int res = db.ExecuteScalarInt("DeleteVendor", ID, false);
//            ID = (ID <= 0) ? res : ID;
//        }


//        #endregion

//        #region Private methods
//        private void LoadFromDataRow(DataRowView row)
//        {
//            ID = ConvertToInt(row["ID"], -1);
//            CompanyName = ConvertToString(row["CompanyName"], String.Empty);
//            FirstName = ConvertToString(row["FirstName"], String.Empty);
//            LastName = ConvertToString(row["LastName"], String.Empty);
//            Address = ConvertToString(row["Address"], String.Empty);
//            PhoneNumber = ConvertToString(row["PhoneNumber"], String.Empty);
//            AltPhoneNumber = ConvertToString(row["AltPhoneNumber"], String.Empty);
//            FaxNumber = ConvertToString(row["FaxNumber"], String.Empty);
//            AltFaxNumber = ConvertToString(row["AltFaxNumber"], String.Empty);
//            MailAddress = ConvertToString(row["MailAddress"], String.Empty);
//            AltMailAddress = ConvertToString(row["AltMailAddress"], String.Empty);
//            LoginName = ConvertToString(row["Login"], String.Empty);
//            Password = ConvertToString(row["Password"], String.Empty);
//            CompanyID = ConvertToInt(row["CompanyID"], -1);
////            VendorTypeId = ConvertToInt(row["VendorTypeId"], -1);
////            VendorTypeName = ConvertToString(row["VendorTypeName"], String.Empty);
////            EmailRequired = Convert.ToBoolean(row["EmailRequired"].ToString());
//            IsDisabled = Convert.ToBoolean(row["IsDisabled"].ToString());
////            isDeleted = Convert.ToBoolean(row["IsDeleted"].ToString());
//            if (row.DataView.Table.Columns.Contains("CompanyStatusID"))
//                companyStatus = (Status)ConvertToInt(row["CompanyStatusID"], 0);
//        }
//        #endregion

//        #region Static methods

//        public static bool CheckLogin(string _login)
//        {
//            return
//            (db.ExecuteScalarInt("GetVendorLoginCheck", _login)==0);
//        }
//        public static DataView GetChargeCategory()
//        {
//            return db.GetDataView("GetChargeCategory");
//        }
//        //public static DataView GetVendorTypeList()
//        //{
//        //    return db.GetDataView("GetVendorTypeList");
//        //}
//        public static DataView GetCompanyVendors(int companyid)
//        {
//            return db.GetDataView("GetCompanyVendors", companyid);
//        }
//        public static DataView GetVendorFeeType(int vendorId)
//        {
//            return db.GetDataView("GetVendorFeeType", vendorId);
//        }

//        //public static DataView GetVendorListByCompany(int companyid)
//        //{
//        //    return db.GetDataView("GetVendorListByCompany", companyid);
//        //}
//        //public static DataView GetVendorListByTypeForInvoice(int companyid, int typ)
//        //{
//        //    return db.GetDataView("GetVendorListByTypeForInvoice", companyid, typ);
//        //}
//        public static DataView GetVendorListForInvoice(int companyId, int feeTypeId)
//        {
//            return db.GetDataView("GetVendorListForInvoice", companyId, feeTypeId);
//        }
//        #endregion
//    }
    public class VendorGlobal : BaseObject
    {
        #region constants
        #region table field names
        private const string IDFIELDNAME = "id";
        private const string NAMEFIELDNAME = "name";
        private const string CORPORATEADDRESS1FIELDNAME = "CorporateAddress1";
        private const string CORPORATEADDRESS2FIELDNAME = "CorporateAddress2";
        private const string COMPANYPHONEFIELDNAME = "CompanyPhone";
        private const string COMPANYFAXFIELDNAME = "CompanyFax";
        private const string COMPANYEMAILFIELDNAME = "CompanyEmail";
        private const string COMPANYCITYFIELDNAME = "CompanyCity";
        private const string COMPANYESTATEIDFIELDNAME = "CompanyStateId";
        private const string COMPANYZIPFIELDNAME = "CompanyZip";
        private const string BILLINGADDRESS1FIELDNAME = "BillingAddress1";
        private const string BILLINGADDRESS2FIELDNAME = "BillingAddress2";
        private const string BILLINGCITYFIELDNAME = "BillingCity";
        private const string BILLINSTATEIDFIELDNAME = "BillingStateId";
        private const string BILLINGZIPFIELDNAME = "BillingZip";
        private const string PRIMARYCONTACTNAMEFIELDNAME = "PCName";
        private const string PRIMARYCONTACTPHONE1FIELDNAME = "PCPhone1";
        private const string PRIMARYCONTACTPHONE2FIELDNAME = "PCPhone2";
        private const string PRIMARYCONTACTEMAILFIELDNAME = "PCEmail";
        private const string SECONDARYCONTACTNAMEFIELDNAME = "SCName";
        private const string SECONDARYCONTACTPHONE1FIELDNAME = "SCPhone1";
        private const string SECONDARYCONTACTPHONE2FIELDNAME = "SCPhone2";
        private const string SECONDARYCONTACTEMAILFIELDNAME = "SCEmail";
        private const string LISENCEEXPDATEFIELDNAME = "LicenseExpDate";
        private const string ISDISABLEDFIELDNAME = "IsDisabled";
        private const string ISDELETEDFIELDNAME = "IsDeleted";
        private const string LOGINFIELDNAME = "login";
        private const string PASSWORDFIELDNAME = "password";
        private const string CITYFIELDNAME = "city";
        private const string ZIPFIELDNAME = "zip";
        private const string STATEIDFIELDNAME = "stateid";
        private const string ISAFFILIATEDWITHORIGINATORFIELDNAME = "isAffiliatedWithOriginator";
        private const string RELATIONSHIPFIELDNAME = "relationship";
        private const string SHORTDESCRIPTIONOFSERVICEFIELDNAME = "shortDescriptionOfServices";
        private const string AFFILIATEDCOMPANYIDFIELDNAME = "affiliatedCompanyId";
        private const string DELIVERYMETHIDIDFIELDNAME = "DeliveryMethodId";
        #endregion
        private const string GETVENDORLIST = "GetVendorsList";
        private const string GETVENDORBYID = "GetVendorGlobalById";
        private const string SAVE = "SaveVendorGlobal";
        private const string GETFEECATEGORY = "GetChargeCategory";
        private const string GETVENDORFEETYPESAMOUNT = "GetVendorFeeTypesAmount";
        private const string SAVEFEE = "SaveVendorFeeTypesAmount";
        private const string SAVEGEOFILTER = "SaveVendorGeoFilter";
        private const string GETSTATELIST = "GetVendorStates";
        private const string GETCOUNTYLIST = "GetVendorCounties";
        private const string GETALWAYSNEVERSETTINGS = "GetVendorAlwaysNeverSettings";
        private const string GETVENDORORIGINATORLIST = "GetVendorUnusedOriginator";
        private const string SAVESETTINGS = "SaveVendorSettings";
        private const string DELETESETTINGS = "DeleteVendorSettings";
        private const string CHANGEPASSWORD = "ChangeVendorPassword";
        private const string DELETE = "DeleteVendorGlobal";
        private const string GETCOMPANYFEEDEFAULTS = "GetCompanyFeeDefaults";
        private const string GETVENDORFORFEETYPE = "GetVendorForfeeType";
        private const string SAVECOMPANYFEEDEFAULT = "SaveDefaultsForVendorForType";
        private const string GETVENDORSFORINVOICE = "GetVendorsForInvoice";
        private const string GETVENDORDELIVERYMETHODLIST = "GetVendorDeliveryMethodList";
        private const string GETVENDORAFFILIATIONFORCOMPANY = "GetVendorAffiliationForCompany";
        private const string SETAFFILIATIONFORCOMPANY = "SetAffiliationForCompany";
        private const string LOGINVENDOR = "LoginVendor";
        private const string GETVENDORORDERFEE = "GetVendorOrderFee";
        public const int SETTLEMENTORCLOSINGFEETYPE = 29;
        public const int TITLEINSURANCEFEETYPE = 31;
        public const int ATTORNEYSFEETYPE = 23;
        public const int NOTARYFEETYPE = 26;
        private const int DELIVERYMETHODVWDID = 2;
        #endregion

        #region fields
        private string name;
        private string corporateAddress1;
        private string corporateAddress2;
        private string companyPhone;
        private string companyFax;
        private string companyEmail;
        private string companyCity;
        private int companyStateId = 0;
        private string companyZip;
        private string billingAddress1;
        private string billingAddress2;
        private string billingCity;
        private int billingStateId = 0;
        private string billingZip;
        private string primaryContactName;
        private string pcPhone1;
        private string pcPhone2;
        private string pcEmail;
        private string secondaryContactName;
        private string scPhone1;
        private string scPhone2;
        private string scEmail;
        private DateTime? licenseExpDate;
        private bool isDisabled;
        private bool isDeleted;
        private string login;
        private string password;
        private bool isAffiliatedWithOriginator = false;
        private string relationship;
        private string shortDescriptionOfServices;
        private int affiliatedCompanyId = 0;
        private Hashtable feeTypes;
        private int deliveryMethodId = 0;
        #endregion

        #region properties
        public int DeliveryMethodId
        {
            get { return deliveryMethodId; }
            set { deliveryMethodId = value; }
        }
        public bool HasLogin
        {
            get { return IsServedFeeType(ATTORNEYSFEETYPE) || IsServedFeeType(NOTARYFEETYPE) || IsServedFeeType(SETTLEMENTORCLOSINGFEETYPE) || (deliveryMethodId == DELIVERYMETHODVWDID); }
        }
        public int AffiliatedCompanyId
        {
            get { return affiliatedCompanyId; }
            set { affiliatedCompanyId = value; }
        }
        public string ShortDescriptionOfServices
        {
            get { return shortDescriptionOfServices; }
            set { shortDescriptionOfServices = value; }
        }
        public string Relationship
        {
            get { return relationship; }
            set { relationship = value; }
        }
        public bool IsAffiliatedWithOriginator
        {
            get { return isAffiliatedWithOriginator; }
            set { isAffiliatedWithOriginator = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string CorporateAddress1
        {
            get { return corporateAddress1; }
            set { corporateAddress1 = value; }
        }
        public string CorporateAddress2
        {
            get { return corporateAddress2; }
            set { corporateAddress2 = value; }
        }
        public string CompanyPhone
        {
            get { return companyPhone; }
            set { companyPhone = value; }
        }
        public string CompanyFax
        {
            get { return companyFax; }
            set { companyFax = value; }
        }
        public string CompanyEmail
        {
            get { return companyEmail; }
            set { companyEmail = value; }
        }
        public string CompanyCity
        {
            get { return companyCity; }
            set { companyCity = value; }
        }
        public string CompanyZip
        {
            get { return companyZip; }
            set { companyZip = value; }
        }
        public int CompanyStateId
        {
            get { return companyStateId; }
            set { companyStateId = value; }
        }
        public string BillingAddress1
        {
            get { return billingAddress1; }
            set { billingAddress1 = value; }
        }
        public string BillingAddress2
        {
            get { return billingAddress2; }
            set { billingAddress2 = value; }
        }
        public string BillingCity
        {
            get { return billingCity; }
            set { billingCity = value; }
        }
        public string BillingZip
        {
            get { return billingZip; }
            set { billingZip = value; }
        }
        public int BillingStateId
        {
            get { return billingStateId; }
            set { billingStateId = value; }
        }

        public string PrimaryContactName
        {
            get { return primaryContactName; }
            set { primaryContactName = value; }
        }
        public string PCPhone1
        {
            get { return pcPhone1; }
            set { pcPhone1 = value; }
        }
        public string PCPhone2
        {
            get { return pcPhone2; }
            set { pcPhone2 = value; }
        }
        public string PCEmail
        {
            get { return pcEmail; }
            set { pcEmail = value; }
        }
        public string SecondaryContactName
        {
            get { return secondaryContactName; }
            set { secondaryContactName = value; }
        }
        public string SCPhone1
        {
            get { return scPhone1; }
            set { scPhone1 = value; }
        }
        public string SCPhone2
        {
            get { return scPhone2; }
            set { scPhone2 = value; }
        }
        public string SCEmail
        {
            get { return scEmail; }
            set { scEmail = value; }
        }
        public DateTime? LicenseExpDate
        {
            get { return licenseExpDate; }
            set { licenseExpDate = value; }
        }
        public bool IsDisabled
        {
            get { return isDisabled; }
            set { isDisabled = value; }
        }
        public bool IsDeleted
        {
            get { return isDeleted; }
        }
        public string Login
        {
            get { return login; }
            set { login = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = Utils.GetMD5Hash(value); }
        }
        public bool IsServedFeeType(int feeTypeId)
        {
            bool res = false;
            if (FeeTypes != null)
            { 
                if(FeeTypes.ContainsKey(feeTypeId))
                {
                    res = true;
                }
            }
            return res;
        }
        private Hashtable FeeTypes
        {
            get 
            {
                if (feeTypes == null)
                {
                    feeTypes = GetFeeTypes();
                }
                return feeTypes;
            }
        }
        #endregion

        #region constructor
        public VendorGlobal()
        {
            ID = -1;
        }
        public VendorGlobal(int vendorID)
        {
            ID = vendorID;
            if (vendorID <= 0)
                return;

            DataView dv = db.GetDataView(GETVENDORBYID, ID);
            if (dv.Count == 1)
            {
                LoadFromDataRow(dv[0]);
            }
            else
            {
                ID = -1;
            }          
        }

        #endregion

        #region methods
        
        #region public
        public static bool IsLoginRequired(int feeTypeId)
        { 
            return (feeTypeId == ATTORNEYSFEETYPE) || (feeTypeId == NOTARYFEETYPE) || (feeTypeId == SETTLEMENTORCLOSINGFEETYPE);
        }
        public int Save()
        {
            return db.ExecuteScalarInt(SAVE
                    , ID
                    , name
                    , corporateAddress1
                    , corporateAddress2
                    , companyPhone
                    , companyFax
                    , companyEmail
                    , companyCity
                    , companyStateId
                    , companyZip
                    , billingAddress1
                    , billingAddress2
                    , billingCity
                    , billingStateId
                    , billingZip
                    , primaryContactName
                    , pcPhone1
                    , pcPhone2
                    , pcEmail
                    , secondaryContactName
                    , scPhone1
                    , scPhone2
                    , scEmail
                    , licenseExpDate
                    , isDisabled
                    , login
                    , password
                    , isAffiliatedWithOriginator
                    , relationship
                    , shortDescriptionOfServices
                    , affiliatedCompanyId
                    , deliveryMethodId
            );
        }
        public DataView GetFeeTypesAmount()
        {
            return db.GetDataView(GETVENDORFEETYPESAMOUNT, ID);
        }
        public int SaveFeeTypes(string data)
        {
            feeTypes = null;
            return db.ExecuteScalarInt(SAVEFEE, ID, data); 
        }
        public int SaveGeoFilter(string data, int stateId)
        {
            return db.ExecuteScalarInt(SAVEGEOFILTER, ID, stateId, data);
        }
        public DataView GetStates()
        {
            return db.GetDataView(GETSTATELIST,ID);
        }
        public DataView GetCounties(int stateId_)
        {
            return db.GetDataView(GETCOUNTYLIST, ID,stateId_);
        }
        public DataView GetAlwaysNeverSettings()
        {
            return db.GetDataView(GETALWAYSNEVERSETTINGS, ID);
        }
        public DataView GetOriginatorList()
        {
            return db.GetDataView(GETVENDORORIGINATORLIST, ID);
        }
        public int SaveSettings(int originatorId, bool isAlways)
        {
            return db.ExecuteScalarInt(SAVESETTINGS, ID, originatorId, isAlways);
        }
        public int DeleteSettings(int originatorId)
        {
            return db.ExecuteScalarInt(DELETESETTINGS, ID, originatorId);
        }
        public bool LoginVendor(string _login, string _password, out string errMsg)
        {
            errMsg = String.Empty;
            DataView dv = db.GetDataView(LOGINVENDOR, _login);
            if (dv.Count == 0)
            {
                errMsg = "Wrong Login or Password";
                return false;
            }
            LoadFromDataRow(dv[0]);
            if (password != Utils.GetMD5Hash(_password))
            {
                ID = -1;
                errMsg = "Wrong Login or Password";
            }
            else if (isDisabled)
            {
                ID = -1;
                errMsg = "Vendor is disabled";
            }
            return ID > 0;
        }
        public bool ChangePassword(string newPassword)
        {
            return db.ExecuteScalarInt(CHANGEPASSWORD, ID, newPassword) == 1;
        }

        #endregion

        #region private
        private Hashtable GetFeeTypes()
        {
            Hashtable res = new Hashtable();
            DataView dv = GetFeeTypesAmount();
            for (int i = 0; i < dv.Count; i++)
            {
                bool selected = bool.Parse(dv[i]["selected"].ToString());
                if (selected)
                {
                    int id = int.Parse(dv[i]["id"].ToString());
                    res.Add(id, selected);
                }
            }
            return res;
        }
        private void LoadFromDataRow(DataRowView row)
        {
            ID = int.Parse(row[IDFIELDNAME].ToString());
            name = row[NAMEFIELDNAME].ToString();
            corporateAddress1  = row[CORPORATEADDRESS1FIELDNAME].ToString();
            corporateAddress2 = row[CORPORATEADDRESS2FIELDNAME].ToString();
            companyPhone = row[COMPANYPHONEFIELDNAME].ToString();
            companyFax = row[COMPANYFAXFIELDNAME].ToString();
            companyEmail = row[COMPANYEMAILFIELDNAME].ToString();
            companyCity = row[COMPANYCITYFIELDNAME].ToString();
            companyStateId = int.Parse(row[COMPANYESTATEIDFIELDNAME].ToString());
            companyZip = row[COMPANYZIPFIELDNAME].ToString();
            billingAddress1 = row[BILLINGADDRESS1FIELDNAME].ToString();
            billingAddress2 = row[BILLINGADDRESS2FIELDNAME].ToString();
            billingCity = row[BILLINGCITYFIELDNAME].ToString();
            billingStateId = int.Parse(row[BILLINSTATEIDFIELDNAME].ToString());
            billingZip = row[BILLINGZIPFIELDNAME].ToString();
            primaryContactName = row[PRIMARYCONTACTNAMEFIELDNAME].ToString();
            pcPhone1 = row[PRIMARYCONTACTPHONE1FIELDNAME].ToString();
            pcPhone2 = row[PRIMARYCONTACTPHONE2FIELDNAME].ToString();
            pcEmail = row[PRIMARYCONTACTEMAILFIELDNAME].ToString();
            secondaryContactName = row[SECONDARYCONTACTNAMEFIELDNAME].ToString();
            scPhone1 = row[SECONDARYCONTACTPHONE1FIELDNAME].ToString();
            scPhone2 = row[SECONDARYCONTACTPHONE2FIELDNAME].ToString();
            scEmail = row[SECONDARYCONTACTEMAILFIELDNAME].ToString();
            if(row[LISENCEEXPDATEFIELDNAME] != DBNull.Value)
                licenseExpDate = DateTime.Parse(row[LISENCEEXPDATEFIELDNAME].ToString());
            isDisabled = bool.Parse(row[ISDISABLEDFIELDNAME].ToString());
            isDeleted = bool.Parse(row[ISDELETEDFIELDNAME].ToString());
            login = row[LOGINFIELDNAME].ToString();
            password = row[PASSWORDFIELDNAME].ToString();
            isAffiliatedWithOriginator = bool.Parse(row[ISAFFILIATEDWITHORIGINATORFIELDNAME].ToString());
            relationship= row[RELATIONSHIPFIELDNAME].ToString();
            shortDescriptionOfServices = row[SHORTDESCRIPTIONOFSERVICEFIELDNAME].ToString();
            affiliatedCompanyId = int.Parse(row[AFFILIATEDCOMPANYIDFIELDNAME].ToString());
            deliveryMethodId = int.Parse(row[DELIVERYMETHIDIDFIELDNAME].ToString());
        }
        #endregion

        #region static
        public static DataView GetVendorFeeOrderList()
        {
            return db.GetDataView(GETVENDORORDERFEE);
        }
        public static DataView GetVendorsList()
        {
            return db.GetDataView(GETVENDORLIST);
        }
        public static DataView GetFeeCategory()
        {
            return db.GetDataView(GETFEECATEGORY);
        }
        public static bool DeleteVendor(int vendorId)
        {
            return db.ExecuteScalarInt(DELETE, vendorId) == 1;
        }
        public static DataView GetCompanyFeeDefaults(int companyId)
        {
            return db.GetDataView(GETCOMPANYFEEDEFAULTS, companyId);
        }
        public static DataView GetVendorForFeeType(int feeTypeId)
        {
            return db.GetDataView(GETVENDORFORFEETYPE, feeTypeId);
        }
        public static bool SaveCompanyFeeDefault(int companyId,int feeTypeId, int firstVendorId, int secondVendorId, int thirdVendorId)
        {
            return db.ExecuteScalarInt(SAVECOMPANYFEEDEFAULT, companyId, feeTypeId, firstVendorId, secondVendorId, thirdVendorId) == 1;
        }
        public static DataView GetVendorsForInvice(int companyId,int feeTypeId, int mortgageId)
        {
            return db.GetDataView(GETVENDORSFORINVOICE, companyId, feeTypeId, mortgageId);
        }
        public static DataView GetVendorDeliveryMethodList()
        {
            return db.GetDataView(GETVENDORDELIVERYMETHODLIST);
        }
        public static string GetAffiliationForCompany(int vendorId, int companyId)
        {
            return db.ExecuteScalarString(GETVENDORAFFILIATIONFORCOMPANY, vendorId, companyId);
        }
        public static bool SetAffiliationForCompany(int vendorId, int companyId, string  affiliation)
        {
            return db.ExecuteScalarInt(SETAFFILIATIONFORCOMPANY, vendorId, companyId, affiliation)==1;
        }

        #endregion
        #endregion

    }
    public class VendorSettings
    {
        #region fields
        private int vendorId;
        private readonly int originatorId = -1;
//        private bool isAlways = true;
        #endregion

        #region properties
        public int Id
        {
            get { return originatorId; }
        }
        #endregion

        #region constructor
        public VendorSettings(int _vendorId)
        {
            vendorId = _vendorId;
        }
        public VendorSettings(DataRowView dr)
        {
            vendorId = int.Parse(dr["vendorid"].ToString());
            originatorId = int.Parse(dr["id"].ToString());
  //          isAlways = bool.Parse(dr["isAlways"].ToString());
        }
        #endregion
    }
    public class UniqueThirdPartyProvider : BaseObject
    {
        #region constants
        private const string GETMORTGAGETHIRDPARTYPROVIDERS = "GetMortgageThirdPartyProviders";
        #endregion

        #region fields
        private readonly int invoiceId;
        private readonly string feeType;
        private readonly decimal charge;
        private readonly string name;
        private readonly string address1;
        private readonly string address2;
        private readonly string city;
        private readonly string state;
        private readonly string zip;
        private readonly string phone;
        private readonly string relationship;
        #endregion

        #region properties
        public string FeeType
        {
            get { return feeType; }
        }
        public decimal Charge
        {
            get { return charge; }
        }
        public string NameAndAddress
        {
            get
            {
                return name+", " + (address1 + " " + address2).Trim() + ", " + city + ", " + state + ", " + zip;
            }
        }
        public string Phone
        {
            get { return phone; }
        }
        public string Relationship
        {
            get { return relationship;}
        }
        #endregion

        #region constructor
        public UniqueThirdPartyProvider(DataRowView dr, List<Invoice> invoices)
        {
            invoiceId = int.Parse(dr["Id"].ToString());
            Invoice invoice = GetInvoiceById(invoices, invoiceId);
            if(invoice != null)
            {
                feeType = invoice.TypeName;
                charge = invoice.Amount;
            }
            //feeType = dr["FeeType"].ToString();
            //charge = decimal.Parse(dr["InvoiceAmount"].ToString());
            name = dr["VendorName"].ToString();
            address1 = dr["Address1"].ToString();
            address2 = dr["Address2"].ToString();
            city = dr["City"].ToString();
            zip = dr["Zip"].ToString();
            state = dr["State"].ToString();
            phone = dr["phone"].ToString();
            relationship = dr["Relationship"].ToString();
        }
        #endregion

        #region methods
        private static Invoice GetInvoiceById(List<Invoice> invoices, int id)
        {
            for (int i = 0; i < invoices.Count;i++ )
            {
                if(invoices[i].ID==id)
                {
                    return invoices[i];
                }
            }
            return null;
        }

        public static DataView GetMortgageProviders(int mortgageId)
        {
            return db.GetDataView(GETMORTGAGETHIRDPARTYPROVIDERS, mortgageId);
        }

        #endregion


    }
}
