using System;

namespace LoanStar.Common
{
    public class ClosingOriginator :  BaseObject
    {
        #region fields
        private string closingName = String.Empty;
        private string closingAddress = String.Empty;
        private string closingCity = String.Empty;
        private string closingZip = String.Empty;
        private string closingPhoneNumber = String.Empty;
        private int closingStateOfIncId = 0;
        private int closingStateId = 0;
        #endregion

        #region properties
        public string ClosingName
        {
            get { return closingName; }
            set { closingName = value;}
        }
        public string ClosingAddress
        {
            get { return closingAddress; }
            set { closingAddress = value;}
        }
        public string ClosingCity
        {
            get { return closingCity; }
            set { closingCity = value;}
        }
        public string ClosingZip
        {
            get { return closingZip; }
            set { closingZip = value;}
        }
        public string ClosingPhoneNumber
        {
            get { return closingPhoneNumber; }
            set { closingPhoneNumber = value;}
        }
        public int ClosingStateOfIncId
        {
            get { return closingStateOfIncId; }
            set { closingStateOfIncId = value; }
        }
        public int ClosingStateId
        {
            get { return closingStateId; }
            set { closingStateId = value; }
        }
        #endregion

        #region methods
        public void PopulateFromLender(Lender lender)
        {
            closingName = lender.Name;
            closingAddress = lender.Address1 + " " + lender.Address2;
            closingCity = lender.City;
            closingZip = lender.Zip;
            closingPhoneNumber = lender.PhoneNumber;
            closingStateOfIncId = lender.OperatesUnderJurisdictionID;
            closingStateId = lender.StateId;
        }
        public void PopulateFromOriginator(Originator originator)
        {
            closingName = originator.ClosingName;
            closingAddress = originator.ClosingAddress;
            closingCity = originator.ClosingCity;
            closingZip = originator.ClosingZip;
            closingPhoneNumber = originator.ClosingPhoneNumber;
            closingStateOfIncId = originator.ClosingStateOfIncId;
            closingStateId = originator.ClosingStateId;
        }
        #endregion

        #region constructor
        public ClosingOriginator()
        {
        }
        #endregion
    }
}
