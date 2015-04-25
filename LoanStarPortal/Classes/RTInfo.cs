using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace LoanStar.Common
{
    public class RTInfo
    {

        private const int LEADSTATUSID = 1;

        #region fields
        private string firstName = String.Empty;
        private string lastName = String.Empty;
        private DateTime? dateOfBirth;
        private decimal homeValue;
        private int stateId = -1; 
        private int countyId = -1;
        private string address1 = String.Empty;
        private string address2 = String.Empty;
        private string city = String.Empty;
        private string zip = String.Empty;
        private string phone = String.Empty;
        private decimal liens;
        private int mortgageId=-1;
        #endregion

        #region properties
        public int MortgageId
        {
            get { return mortgageId; }
            set { mortgageId = value; }
        }
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public DateTime? DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
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
        public int StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }
        public int CountyId
        {
            get { return countyId; }
            set { countyId = value; }
        }
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        public string Zip
        {
            get { return zip; }
            set { zip = value; }
        }
        public decimal Liens
        {
            get { return liens; }
            set { liens = value; }
        }
        public decimal HomeValue
        {
            get { return homeValue; }
            set { homeValue = value; }
        }

        #endregion

        #region constructor
        public RTInfo()
        { 
        }
        public RTInfo(int _mortgageId, string _firstName, string _lastName, DateTime _dateOfBirth, decimal _homeValue
            , int _stateId, int _countyId, string _address1, string _address2, string _city, string _phone
            , string _zip, decimal _liens)
        {
            mortgageId = _mortgageId;
            firstName = _firstName;
            lastName = _lastName;
            dateOfBirth = _dateOfBirth;
            homeValue = _homeValue;
            stateId = _stateId;
            countyId = _countyId;
            address1 = _address1; 
            address2 = _address2;
            city = _city;
            phone = _phone;
            zip = _zip;
            liens = _liens;
        }
        #endregion

        #region methods
        public int Save(MortgageProfile mp, AppPage currentPage)
        {
            int res = MortgageId;
            mp.CurProfileStatusID = LEADSTATUSID;
                string err;
            if (MortgageId > 0)
            {
                mp.UpdateObject("Borrowers.FirstName", FirstName, mp.Borrowers[0].ID, out err);
                mp.UpdateObject("Borrowers.LastName", LastName, mp.Borrowers[0].ID, out err);
                if (DateOfBirth != null)
                {
                    mp.UpdateObject("Borrowers.DateOfBirth", DateOfBirth.ToString(), mp.Borrowers[0].ID, out err);
                }
            }
            else 
            {
                Borrower borrower = mp.Borrowers[0];
                borrower.FirstName = FirstName;
                borrower.LastName = LastName;
                borrower.DateOfBirth = DateOfBirth;
                mp.Borrowers.Add(borrower);
                res = mp.CreateNew(currentPage.CurrentUser);
                if(res>0)
                {
                    mp = currentPage.GetMortgage(res);
                }

                MortgageId = res;
            }
            if (res > 0)
            {
                if (homeValue > 0)
                {
                    mp.UpdateObject("Property.SPValue", homeValue.ToString(), mp.Property.ID, out err);
                }
                if (stateId > 0)
                {
                    mp.UpdateObject("Property.StateId", stateId.ToString(), mp.Property.ID, out err);
                    if (countyId > 0)
                    {
                        mp.UpdateObject("Property.CountyID", countyId.ToString(), mp.Property.ID, out err);
                    }
                }
                if (!String.IsNullOrEmpty(Address1))
                {
                    mp.UpdateObject("Borrowers.Address1", Address1, mp.Borrowers[0].ID, out err);
                }
                if (!String.IsNullOrEmpty(Address2))
                {
                    mp.UpdateObject("Borrowers.Address2", Address2, mp.Borrowers[0].ID, out err);
                }
                if (!String.IsNullOrEmpty(City))
                {
                    mp.UpdateObject("Borrowers.City", City, mp.Borrowers[0].ID, out err);
                }
                if (!String.IsNullOrEmpty(Zip))
                {
                    mp.UpdateObject("Borrowers.Zip", Zip, mp.Borrowers[0].ID, out err);
                }
                if (!String.IsNullOrEmpty(Phone))
                {
                    mp.UpdateObject("Borrowers.Phone", Phone, mp.Borrowers[0].ID, out err);
                }
            }
            return res;
        }
        #endregion

    }
}
