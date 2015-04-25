using System;
using System.Xml;
using System.Collections;
using LoanStar.Common;

namespace LoanStarPortal
{
    public partial class ProcessRequest : AppPage
    {

        #region constants
        private const string MORTGAGEID = "mid";
        private const string OBJECTID = "oid";
        private const string REQUIRED = "req";
        private const string PROPERTY = "property";
        private const string VALUE = "value";
        private const int STATUSOK = 0;
        private const int STATUSMORTGAGENOTFOUND = 1;
        private const int STATUSOBJECTNOTFOUND = 2;
        private const int STATUSUPDATEFAILED = 3;
        private const string ROOTELEMENT = "root";
        private const string RESULTCODE = "result";
        private const string CAMPAIGNCODE = "campaignresult";
        private const string ERRMESSAGE = "errtext";
        private const string TABUPDATE = "tabupdate";
        private const string TABVALUE = "tabvalue";
        private const string APPLICANTLISTVALUE = "applistvalue";
        private const string BORROWERFIRSTNAME = "Borrowers.FirstName";
        private const string BORROWERLASTNAME = "Borrowers.LastName";
        private const string BORROWERBIRTHDATE = "Borrowers.DateOfBirth";
        private const string MORTGAGECLOSINGDATE = "MortgageInfo.ClosingDate";
        private const string PROPERTYHOMEVALUE = "Property.SPValue";
        private const string PROPERTYSTATEID = "Property.StateId";
        private const string PROPERTYCOUNTYID = "Property.CountyID";
        private const string MORTGAGEPRODUCTID = "MortgageInfo.ProductId";
        #endregion

        #region fields
        private int mortgageId = -1;
        private int objId = -1;
        private string fullPropertyName = String.Empty;
        private string propertyValue = String.Empty;
        private int statusCode;
        private MortgageProfile mortgage;
        private string errMessage;
        private bool needTabUpdate;
        private bool needApplicantListUpdate;
        private string tabValue = String.Empty;
        private string applicantListValue = String.Empty;
        private bool isRequiredField = false;
        private DateTime? dateOfBirth;
        private int campignUpdateResult = 0;
        private bool campaignDataRequest = false;
        #endregion        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request["campaignid"] != null)
            {
                ProcessCampaign();
                return;
            }
            if(Page.Request["appid"]!=null)
            {
                UpdateEmailAssociatians();
                return;
            }
            if(Page.Request["checklist"]!=null)
            {
                UpdateCheckList();
                return;
            }
            mortgageId = GetRequestInt(MORTGAGEID);
            if (mortgageId > 0)
            {
                objId = GetRequestInt(OBJECTID);
                if (objId >= 0)
                {
                    mortgage = GetMortgage(mortgageId);
                    isRequiredField = GetRequestInt(REQUIRED) == 1;
                    fullPropertyName = GetRequestString(PROPERTY);
                    needTabUpdate = (fullPropertyName == BORROWERFIRSTNAME) || (fullPropertyName == BORROWERLASTNAME);
                    needApplicantListUpdate = needTabUpdate || (fullPropertyName == BORROWERBIRTHDATE);
                    propertyValue = GetRequestString(VALUE);
                    if (fullPropertyName == BORROWERBIRTHDATE)
                    {
                        dateOfBirth = mortgage.YoungestBorrower.DateOfBirth;
                    }
                    ProcessReq();
                    if(fullPropertyName == BORROWERBIRTHDATE)
                    {
                        UpdateBirthDateCampaigns();
                    }
                    if(fullPropertyName == MORTGAGECLOSINGDATE)
                    {
                        UpdateClosingDateCampaigns();
                    }
                }
                else
                {
                    statusCode = STATUSOBJECTNOTFOUND;
                }
            }
            else 
            {
                statusCode = STATUSMORTGAGENOTFOUND;
            }
            SendResponse();
        }

        #region methods
        private bool CheckForCampaigns()
        {
            return
                (mortgage.CurProfileStatusID == MortgageProfile.LEADSTATUSID ||
                 mortgage.CurProfileStatusID == MortgageProfile.MANAGEDLEADSTATUSID) &&
                ((AppPage) Page).CurrentUser.IsLeadManagementEnabled;
        }
        private void UpdateClosingDateCampaigns()
        {
            if(CheckForCampaigns())
            {
                campaignDataRequest = true;
                campignUpdateResult = mortgage.UpdateClosingDateCampaigns();
            }
        }

        private void UpdateBirthDateCampaigns()
        {
            if (CheckForCampaigns())
            {
                if (mortgage.YoungestBorrower.ID == objId)
                {
                    campaignDataRequest = true;
                    campignUpdateResult = mortgage.UpdateBirthDateCampaigns();
                }
            }
        }

        private void SendResponse()
        {
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            XmlAttribute a = d.CreateAttribute(RESULTCODE);
            a.Value = statusCode.ToString();
            root.Attributes.Append(a);
            a = d.CreateAttribute(ERRMESSAGE);
            a.Value = errMessage;
            root.Attributes.Append(a);
            a = d.CreateAttribute(TABUPDATE);
            int updatevalue = 0;
            if (statusCode == STATUSOK)
            {
                if (needTabUpdate)
                {
                    updatevalue += 1;
                }
                if (needApplicantListUpdate)
                {
                    updatevalue += 2;
                }
                if(campaignDataRequest&&(campignUpdateResult!=0))
                {
                    XmlAttribute ca = d.CreateAttribute(CAMPAIGNCODE);
                    ca.Value = campignUpdateResult < 0 ? "0" : "1";
                    root.Attributes.Append(ca);
                }
            }
            a.Value = updatevalue.ToString();
            root.Attributes.Append(a);
            a = d.CreateAttribute(TABVALUE);
            a.Value = tabValue;
            root.Attributes.Append(a);
            a = d.CreateAttribute(APPLICANTLISTVALUE);
            a.Value = applicantListValue;
            root.Attributes.Append(a);
            d.AppendChild(root);
            Response.ContentType = "text/xml";
            Response.Expires = -1;
            Response.Clear();
            Response.Write(d.InnerXml);
        }
        private void ProcessReq()
        {
//            mortgage = GetMortgage(mortgageId);
            mortgage.LastPostBackField = fullPropertyName;
            SetProcessRequired(isRequiredField);
            if (!mortgage.UpdateObject(fullPropertyName, propertyValue, objId, out errMessage))
            {
                statusCode = STATUSUPDATEFAILED;
            }
            else
            {
                PostProcessAction();
                UpdateMortgage(mortgage,mortgageId);
                if (needTabUpdate)
                {
                    Borrower borrower = mortgage.GetBorrowerById(objId);
                    if (borrower != null)
                    {
                        tabValue = borrower.FirstName + " " + borrower.LastName;
                    }                    
                }
                if (needApplicantListUpdate)
                {
                    Borrower b = mortgage.YoungestBorrower;
                    applicantListValue = b.LastName + ", " + b.FirstName;
                }
            }
            SetProcessRequired(false);
        }
        private void PostProcessAction()
        {
            if((fullPropertyName==PROPERTYHOMEVALUE)||(fullPropertyName==PROPERTYSTATEID)
                ||(fullPropertyName==PROPERTYCOUNTYID)||(fullPropertyName==MORTGAGEPRODUCTID))
            {
                mortgage.UpdateInvoiceAmount();
            }
        }
        private int GetRequestInt(string name)
        {
            int res = -1;
            try
            {
                res = Convert.ToInt32(GetRequestString(name));
            }
            catch 
            {
            }
            return res;
        }
        private string GetRequestString(string name)
        {
            string res = String.Empty;
            if (Page.Request[name] != null)
            {
                res = Page.Request[name];
            }
            return res;
        }
        private void ProcessCampaign()
        {
            int campaignId = GetRequestInt("campaignid");
            mortgageId = GetRequestInt("mid");
            int year = GetRequestInt("year");
            int month = GetRequestInt("month");
            string res = "";
            if(campaignId>0&&mortgageId>0&&year>0&&month>0)
            {
                mortgage = GetMortgage(mortgageId);
                LeadCampaign campaign = new LeadCampaign(campaignId,mortgageId);
                ArrayList selection = campaign.GetScheduleDates(mortgage, year, month);
                for (int i = 0; i < selection.Count;i++ )
                {
                    if(i>0)
                    {
                        res += "&";
                    }
                    DateTime dt = (DateTime) selection[i];
                    res += dt.Day;
                }
            }
            PrepareResponse(res);
        }

        private void UpdateEmailAssociatians()
        {
            int mortgageId_ = GetRequestInt("appid");
            int op = GetRequestInt("op");
            if (mortgageId_ > 0 && op >= 0)
            {
                Object o = Session["messageid"];
                if (o!=null)
                {
                    try
                    {
                        int messageId = int.Parse(o.ToString());
                        WebMailHelper.UpdateMessageAssociation(messageId, mortgageId_, op);
                    }
                    catch
                    {
                    }
                }
            }
            PrepareResponse();
        }
        private void PrepareResponse(string data)
        {
            Response.ContentType = "text/xml";
            Response.Expires = -1;
            Response.Clear();
            Response.Write(data);
        }
        private void PrepareResponse()
        {
            Response.ContentType = "text/xml";
            Response.Expires = -1;
            Response.Clear();
            Response.Write("");
        }
        private void UpdateCheckList()
        {
            mortgageId = -1;
            Object o = Session[Constants.MortgageID];
            if (o != null)
            {
                try
                {
                    mortgageId = int.Parse(o.ToString());
                }
                catch
                {
                }
            }
            if(mortgageId>0)
            {
                mortgage = GetMortgage(mortgageId);
                int itemId= GetRequestInt("checklist");
                if(itemId>0)
                {
                    int sel = GetRequestInt("val");
                    if(sel>=0)
                    {
                        mortgage.UpdateCheckList(itemId,(short)sel);
                    }
                }
            }
            PrepareResponse();
        }
        #endregion
    }
}
