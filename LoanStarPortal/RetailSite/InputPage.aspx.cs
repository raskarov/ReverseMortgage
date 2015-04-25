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

namespace LoanStarPortal.RetailSite
{
    public partial class InputPage : AppPage
    {
        private const string ONCLICK = "onclick";
        private const string CHECKSAVEJS = "return CheckInputOnSave('{0}','{1}');";
        private const string CHECKCALCULATEJS = "return CheckInputOnCalculate('{0}','{1}','{2}','{3}','{4}','{5}','{6}');";

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Attributes.Add(ONCLICK, String.Format(CHECKSAVEJS,tbFirstName.ClientID,tbLastName.ClientID));
            btnCalculate.Attributes.Add(ONCLICK, String.Format(CHECKCALCULATEJS, tbValue.ClientID, tbZip.ClientID, ddlState.ClientID,ddlCounty.ClientID,rdidob1.ClientID,rdidob2.ClientID,tbLiens.ClientID));
            if(!IsPostBack)
            {
                BindData();
            }
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
            ddlState.Items.Insert(0, new ListItem("- Select -","0"));
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

        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {

        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCounty();
        }
    }
}
