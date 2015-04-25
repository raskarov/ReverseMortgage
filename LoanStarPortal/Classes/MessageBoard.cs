using System;
using System.Data;

namespace LoanStar.Common
{
    public class MessageBoard : BaseObject
    {
        #region Static methods
        public static DataTable LoadMessageBoard(MessageBoardFilter filter, int UserID)
        {
//            return db.GetDataTable("GetMessageBoardData", filter.MortgageId, reportedDate,UserID, filter.ShowConditions, filter.ConditionId>0?false:filter.ShowEvents, filter.ShowNotes, filter.ShowEmails, filter.ConditionId ?? (object)DBNull.Value, filter.MinutesOffset);
            int noteType = 0;
            if (filter.CurrentConditionCampaignId != null)
            {
                noteType = filter.Mode;
            }
            return db.GetDataTable("GetMessageBoardData_v2", filter.MortgageId, UserID, filter.ShowConditions, filter.ShowEvents, filter.ShowNotes, filter.CurrentConditionCampaignId ?? (object)DBNull.Value ,noteType);
        }
        #endregion
    }

    public class Message
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string MessageType { get; set; }
        public int MortgageID { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
    }

    public class MessageBoardFilter
    {
        #region constants
        public const int MODECONDITION = 1;
        public const int MODECAMPAIGN = 2;
        #endregion

        #region fields
        private int mortgageId = 0;
        private bool showConditions = true;
        private bool showNotes = true;
        private bool showEvents = false;
        private bool showEmails = true;
        private int? conditionId;
        private int? campaignId;
        private int minutesOffset = 0;
        private int mode = MODECONDITION; 
        #endregion 


        #region properties
        public int Mode
        {
            get { return mode; }
            set { mode = value; }
        }
        public int MinutesOffset
        {
            get { return minutesOffset; }
            set { minutesOffset = value; }
        }
        public int MortgageId
        {
            get { return mortgageId; }
        }
        public int? CurrentConditionCampaignId
        {
            get
            {
                int? res = conditionId;
                if(mode==MODECAMPAIGN)
                {
                    res = campaignId;
                }
                return res;
            }
            
        }
        public int? ConditionId
        {
            get { return conditionId; }
            set { conditionId = value; }
        }
        public int? CampaignId
        {
            get { return campaignId; }
            set { campaignId = value; }
        }
        public bool ShowConditions
        {
            get { return showConditions; }
            set { showConditions = value; }
        }
        public bool ShowNotes
        {
            get { return showNotes; }
            set { showNotes = value; }
        }
        public bool ShowEvents
        {
            get { return showEvents; }
            set { showEvents = value; }
        }
        public bool ShowEmails
        {
            get { return showEmails; }
            set { showEmails = value; }
        }
        #endregion

        #region methods
        public bool SetMortgage(int _mortgageId)
        {
            bool res = _mortgageId != mortgageId;
            if (res)
            {
                conditionId = null;
                campaignId = null;
            }
            mortgageId = _mortgageId;
            return res;
        }
        #endregion

    }
}
