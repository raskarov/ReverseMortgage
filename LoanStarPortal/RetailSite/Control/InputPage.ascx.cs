using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LoanStar.Common;

namespace LoanStarPortal.RetailSite.Control
{
    public partial class InputPage : AppControl
    {
        private const string ONCLICK = "onclick";
        private const string CHECKSAVEJS = "var r=CheckInputOnSave('{0}','{1}'); if (!r) return false;";
        private const string CHECKCALCULATEJS = "var r=CheckInputOnCalculate('{0}','{1}','{2}','{3}'); if (!r) return false;";
//        private const int LEADSTATUSID = 1;

        #region fields  
        private decimal homeValue;
        private decimal liens;
        private int stateId;
        private int countyId;
        private Telerik.WebControls.RadAjaxManager ajaxManager;
        #endregion
        public Telerik.WebControls.RadAjaxManager AjaxManager
        {
            get { return ajaxManager; }
            set { ajaxManager = value; }
        }
        public int RtMortgageId
        {
            get 
            {
                int res = -1;
                Object o = Session[Constants.RETAILSITEMORTGAGEID];
                if (o != null)
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
            set 
            {
                Session[Constants.RETAILSITEMORTGAGEID] = value;
            }
        }
        public RTInfo RetailSiteInfo
        {
            get 
            {
                RTInfo res;
                Object o = Session["retailsiteinfo"];
                if((o!=null)&& o is RTInfo)
                {
                    res = (RTInfo)o;
                }
                else
                {
                    res = new RTInfo();
                    Session["retailsiteinfo"] = res; 
                }
                return res;
            }
            set { Session["retailsiteinfo"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Attributes.Add(ONCLICK, String.Format(CHECKSAVEJS, tbFirstName.ClientID, tbLastName.ClientID));
            btnCalculate.Attributes.Add(ONCLICK, String.Format(CHECKCALCULATEJS, tbValue.ClientID, ddlState.ClientID, ddlCounty.ClientID, rdidob1.ClientID));
            if (!IsPostBack)
            {
                LoadFromSession();
                tbValue.Focus();
            }
            BindData();
        }
        private void LoadFromSession()
        {
            if (RetailSiteInfo.HomeValue > 0) 
            tbValue.Text = RetailSiteInfo.HomeValue.ToString();
            if(RetailSiteInfo.StateId>0) 
                ddlState.SelectedValue = RetailSiteInfo.StateId.ToString();
            if (RetailSiteInfo.CountyId > 0) 
                ddlCounty.SelectedValue = RetailSiteInfo.CountyId.ToString();
            rdidob1.SelectedDate = RetailSiteInfo.DateOfBirth;
            tbFirstName.Text = RetailSiteInfo.FirstName;
            tbLastName.Text = RetailSiteInfo.LastName;
            tbAddress1.Text = RetailSiteInfo.Address1;
            tbAddress2.Text = RetailSiteInfo.Address2;
            if (RetailSiteInfo.Liens > 0) 
                tbLiens.Text = RetailSiteInfo.Liens.ToString();
            tbCity.Text = RetailSiteInfo.City;
            tbZip.Text = RetailSiteInfo.Zip;
            tbPhone.Text = RetailSiteInfo.Phone;
        }
        private void BindData()
        {
            BindState();
            BindCounty();
        }
        private void BindState()
        {
            DataView dv = DataHelpers.GetDictionary("State");
            dv.Sort = "Name";
            ddlState.DataSource = dv;
            ddlState.DataTextField = "Name";
            ddlState.DataValueField = "Id";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("- Select -", "0"));
        }
        private void BindCounty()
        {
            DataView dv = DataHelpers.GetDictionary("County");
            dv.Sort = "Name";
            dv.RowFilter = "StateId=" + ddlState.SelectedValue.ToString();
            ddlCounty.DataSource = dv;
            ddlCounty.DataTextField = "Name";
            ddlCounty.DataValueField = "Id";
            ddlCounty.DataBind();
            ddlCounty.Items.Insert(0, new ListItem("- Select -", "0"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveBorrower())
            {
                lblMessage.Text = Constants.SUCCESSMESSAGE;
            }
            else 
            {
                lblMessage.Text = "Can't save data";
            }
            ddlCounty.SelectedValue = countyId.ToString();
        }
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            SaveInfo();
            ClearSessionData();
            Response.Redirect("RetailPage.aspx?control=calculate");
        }
        private void ClearSessionData()
        {
            Session.Remove(Constants.ADVANCEDCALCULATOR);
        }
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCounty();
            AjaxManager.ResponseScripts.Add("SetFocusAfterRequest();");
        }
        private void SaveInfo()
        {
            RTInfo rti = RetailSiteInfo;
            homeValue = 0;
            if (!String.IsNullOrEmpty(tbValue.Text))
            {
                homeValue = decimal.Parse(tbValue.Text);
            }
            liens = 0;
            if (!String.IsNullOrEmpty(tbLiens.Text))
            {
                liens = decimal.Parse(tbLiens.Text);
            }
            stateId = -1;
            countyId = -1;
            try
            {
                stateId = int.Parse(ddlState.SelectedValue.ToString());
            }
            catch
            { }
            try
            {
                countyId = int.Parse(GetPostedValue("ddlCounty"));
            }
            catch
            { }
            rti.FirstName = tbFirstName.Text;
            rti.LastName = tbLastName.Text;
            rti.Address1 = tbAddress1.Text;
            rti.Address2 = tbAddress2.Text;
            rti.City = tbCity.Text;
            rti.Zip = tbZip.Text;
            rti.Phone = tbPhone.Text;
            rti.DateOfBirth = rdidob1.SelectedDate;
            rti.StateId = stateId;
            rti.CountyId = countyId;
            rti.HomeValue = homeValue;
            RetailSiteInfo = rti;
        }
        private bool SaveBorrower()
        {
            MortgageProfile mp;
            if (RetailSiteInfo.MortgageId < 0)
            {
                mp = new MortgageProfile();
            }
            else
            {
                mp = CurrentPage.GetMortgage(RtMortgageId);
            }
            SaveInfo();
            int res = RetailSiteInfo.Save(mp, CurrentPage);
            RtMortgageId = res;
            return res > 0;
        }
        private string GetPostedValue(string controlName)
        {
            string res = String.Empty;
            String[] col = Page.Request.Form.AllKeys;
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].EndsWith("$"+controlName))
                {
                    res = Page.Request[col[i]];
                }
            }
            return res;
        }
    }
}