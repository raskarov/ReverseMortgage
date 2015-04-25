using System;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;
using System.Collections.Generic;
namespace LoanStarPortal.Controls
{
    public partial class Notes : AppControl
    {
        #region Fields/Properties
        private readonly Color colorNote = Color.Yellow;
        private readonly Color colorAlert = Color.LightPink;
        private readonly Color colorEvent = Color.LightBlue;
        private readonly Color colorEmail = Color.LightGreen;

        private const string ConditionType = "Condition"; 
        private const string AlertType = "Alert";
        private const string NoteType = "Note";
        private const string EventType = "Event";
        private const string EmailType = "Email";
        private const string MESSAGEBOARDFILTER = "mbfilter";
        private const int DATESELECTION = 0;
        private const int DETAILSSELECTION = 2;
        private const int ITEMSELECTION = 1;

        public int DisplayLevelSelection
        {
            get
            {
                int res = 1;
                Object o = Session["DisplayLevelSelection"];
                if(o!=null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set { Session["DisplayLevelSelection"] = value; }
        }
        public  string DivQuickNoteId
        {

            get { return divQuickNote.ClientID;}
        }
        private int MortgageID
        {
            get
            {
                return Convert.ToInt32(Session[Constants.MortgageID]);
            }
        }
        public int? ConditionID
        {
            set
            {
                ViewState["mbConditionID"] = value;
            }
            get
            {
                if (ViewState["mbConditionID"] == null)
                    return null;
                else
                    return Convert.ToInt32(ViewState["mbConditionID"]);
            }
        }
        public bool IsFirstLoad
        {
            get
            {
                Object o = ViewState["FirstLoadNotes"];
                bool res = true;
                if (o != null)
                {
                    try
                    {
                        res = bool.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set
            {
                ViewState["FirstLoadNotes"] = value;
            }
        }
        public MessageBoardFilter CurrentFilter
        {
            get 
            {
                MessageBoardFilter res = Session[MESSAGEBOARDFILTER] as MessageBoardFilter;
                if (res == null)
                {
                    res = new MessageBoardFilter();
                    Session[MESSAGEBOARDFILTER] = res;
                }
                return res;
            }
            set
            {
                Session[MESSAGEBOARDFILTER] = value;
            }
        }
        private List<Message> _AllMessages = null;
        public List<Message> AllMessages
        {
            get
            {
                _AllMessages = Session["AllMessages"] as List<Message>;
                if (_AllMessages==null)
                {
                    _AllMessages =  LoadMessages();
                    Session["AllMessages"] = _AllMessages;
                }
                return _AllMessages;
            }
            set
            {
                Session["AllMessages"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
                CurrentFilter.MinutesOffset = CurrentUser.GetMinutesOffset();
                GetDisplayLevelSelection();
                BindDisplayLevel();
                CurrentFilter.SetMortgage(MortgageID);
                SetCheckBoxes();
                BindMessageBoard();
        }
        private void BindDisplayLevel()
        {
            ddlDisplayLevel.Items.Clear();
            ddlDisplayLevel.Items.Add(new ListItem("Dates", DATESELECTION.ToString()));
            ddlDisplayLevel.Items.Add(new ListItem("Items", ITEMSELECTION.ToString()));
            ddlDisplayLevel.Items.Add(new ListItem("Detail",DETAILSSELECTION.ToString()));
            ddlDisplayLevel.SelectedValue = DisplayLevelSelection.ToString();
            ddlDisplayLevel.Attributes.Add("onchange","DisplayLevelChanged(this);");
        }
        private void GetDisplayLevelSelection()
        {
            string tmp = GetPostedValue(ddlDisplayLevel.ID);
            if(!String.IsNullOrEmpty(tmp))
            {
                try
                {
                    DisplayLevelSelection = int.Parse(tmp);
                }
                catch
                {
                }
            }
        }
        private string GetPostedValue(string controlId)
        {
            string res = "";
            for (int i = 0; i < Page.Request.Form.AllKeys.Length; i++)
            {
                string s = Page.Request.Form.AllKeys[i];
                if (s.EndsWith("$" + controlId))
                {
                    res = Page.Request.Form[s];
                    break;
                }
            }
            return res;
        }
        public void BindData()
        {
            AllMessages = null;
            chbEvents.Visible = !(CurrentFilter.ConditionId > 0);
            BindMessageBoard();
        }
        private static void AddDateAttribute(WebControl item,string val)
        {
            item.Attributes["Date"] = val;
        }

        private void BindMessageBoard()
        {
            panelMessageBoard.Items.Clear();
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageID);
            DateTime MortgageDate = mp.Created;
            CultureInfo culture = CultureInfo.InvariantCulture;
            int weekNum = culture.Calendar.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
          
            int monthNum = DateTime.Today.Month;

            //Today
            var todayMessages = AllMessages.Where(y => y.Created.Date == DateTime.Today).ToList();
            if (todayMessages.Count > 0)
            {
                ConfigurePanel(todayMessages, new RadPanelItem("Today"));
            }

            //Yesterday
            var yesterdayMessages = AllMessages.Where(x => x.Created != null && x.Created.Date == DateTime.Today.AddDays(-1).Date).ToList();
            if (yesterdayMessages.Count > 0)
            {
                ConfigurePanel(yesterdayMessages, new RadPanelItem("Yesterday"));
            }

            //This week (Between yesterday and first day of current week)
            if (DateTime.Today.DayOfWeek != DayOfWeek.Monday && DateTime.Today.DayOfWeek != DayOfWeek.Tuesday) //We already have that data pulled above
            {
                var startOfThisWeek = DateTime.Today.StartOfWeek(DayOfWeek.Monday).Date;
                var ThisWeekMessages = AllMessages.Where(x => x.Created != null && (x.Created.Date <= DateTime.Today.AddDays(-2) && x.Created.Date >= startOfThisWeek)).ToList();
                if (ThisWeekMessages.Count > 0)
                {
                    ConfigurePanel(ThisWeekMessages, new RadPanelItem("This week"), true);
                }
            }
            //Last week
            var EndOfLastWeek = DateTime.Today.StartOfWeek(DayOfWeek.Monday).AddDays(-1).Date;
            var StartOfLastWeek = EndOfLastWeek.StartOfWeek(DayOfWeek.Monday);
            var LastWeekMessages = AllMessages.Where(x => x.Created != null && x.Created.Date <= EndOfLastWeek && x.Created.Date >= StartOfLastWeek).ToList();
            if (LastWeekMessages.Count > 0)
            {
                ConfigurePanel(LastWeekMessages, new RadPanelItem("Last week"), true);
            }
            //Older
            var OldMessages = AllMessages.Where(x => x.Created != null && x.Created.Date <= StartOfLastWeek.AddDays(-1)).ToList();
            if (OldMessages.Count > 0)
            {
                ConfigurePanel(OldMessages, new RadPanelItem("Older messages"), true);
            }
        }

        private void ConfigurePanel(List<Message> todayMessages, RadPanelItem todayPane, bool NeedDate = false)
        {
            AddDateAttribute(todayPane, "1");
            todayPane.Expanded = (DisplayLevelSelection == ITEMSELECTION) || (DisplayLevelSelection == DETAILSSELECTION);
            panelMessageBoard.Items.Add(todayPane);
            FillPanelItem(todayPane, todayMessages, NeedDate);
        }
        private List<Message> LoadMessages()
        {
            
           var messages =  MessageBoard.LoadMessageBoard(CurrentFilter, CurrentUser.Id);
           var list = messages.DataTableToList<Message>();
           return list;
        }

        private void FillPanelItem(IRadPanelItemContainer item, List<Message> messages, bool NeedDate)
        {
            messages.ForEach(x=>AddPanelItem(item,x,NeedDate));                  
        }

        private void AddPanelItem(IRadPanelItemContainer item, Message message, bool NeedDate)
        {
            string MesType = message.MessageType;
            RadPanelItem newItem = new RadPanelItem();
            newItem.Expanded = DisplayLevelSelection == DETAILSSELECTION;
            string Title = message.Title;

            if (Title.Length > 17)
                Title = Title.Substring(0, 17) + "...";

            string imgPath = "";
            DateTime dt = message.Created;
            string Date = (NeedDate) ? dt.ToString("MM/dd/yyyy") : dt.ToString("hh:mm tt");
            newItem.Value = String.Format("{0}&{1}", message.ID, message.Created);
            newItem.Width = Unit.Percentage(100);
            string Description="";
            Description += Convert.ToString(message.Description).Replace("\n", "<br>");
            switch (MesType)
            {
                case ConditionType:
                    newItem.BackColor = colorEvent;
                    imgPath = ResolveUrl("~/Images/itask.gif");
                    break;
                case EventType:
                    newItem.BackColor = colorEvent;
                    imgPath = ResolveUrl("~/Images/ievent.gif");
                    break;
                case AlertType:
                    newItem.BackColor = colorAlert;
                    imgPath = ResolveUrl("~/Images/ialert.gif");
                    break;
                case NoteType:
                    newItem.BackColor = colorNote;
                    imgPath = ResolveUrl("~/Images/inote.gif");
                    break;
                case EmailType:
                    newItem.BackColor = colorEmail;
                    imgPath = ResolveUrl("~/Images/imailunread.gif");
                    break;
            }
            newItem.Text = String.Format("<span style='float:right;'>{3}</span><img src='{0}' border='0' />&nbsp;{1}&nbsp;&nbsp;<b>{2}</b>", imgPath, MesType, Title, Date);

            RadPanelItem ci = new RadPanelItem();
            //Panel panelGeneral = new Panel();
            Literal lblText = new Literal();
            lblText.Text = String.Format("<div style='border:none;width:99%;'><table width='100%'><tr><td width='2px'>&nbsp;</td><td style='line-height:15px;'>{0}</td></tr></table></div>", Description);
            ci.Controls.Add(lblText);
            newItem.Items.Add(ci);
            item.Items.Add(newItem);
        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbNote.Text))
                Condition.SaveNote(null, MortgageID, tbNote.Text, CurrentUser.Id);
            tbNote.Text = "";
            AllMessages = null;
            BindData();
        }
        private void SetFilter(Object sender)
        {
            CheckBox cb = sender as CheckBox;
            if (cb != null)
            {
                if (cb.ID == chbEvents.ID)
                {
                    CurrentFilter.ShowEvents = cb.Checked;
                }
                else if (cb.ID == chbNote.ID)
                {
                    CurrentFilter.ShowNotes = cb.Checked;
                }
            }
        }
        private void SetCheckBoxes()
        {
//            chbConditions.Checked = CurrentFilter.ShowConditions;
            chbNote.Checked = CurrentFilter.ShowNotes;
            //chbEmail.Checked = CurrentFilter.ShowEmails;
            chbEvents.Checked = CurrentFilter.ShowEvents;
        }
        protected void FilterChanged(object sender, EventArgs e)
        {
            SetFilter(sender);
            BindData();
        }

    }
}