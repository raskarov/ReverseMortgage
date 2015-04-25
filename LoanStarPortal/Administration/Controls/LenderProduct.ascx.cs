using System;
using System.Data;
using System.Xml;
using System.Web.UI.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class LenderProduct : AppControl
    {
        #region constants
        private const string PRODUCTIDATTRIBUTE = "productid";
        private const string HEADERTEXT = "Company {0}, product {1}";
        private const string ROOTELEMENT = "Root";
        private const string ITEMELEMENT = "item";
        private const string COMPANYID = "companyid";
        private const string ISDEFAULT = "isdefault";
        private const int NOERROR = 0;
        private const int NOTSELECTED = 1;
        private const int NODEFAULT = 2;
        private const int MUCHDEFAULT = 3;
        private const string CLICKHANDLER = "onclick";
        private const string SETDEFAULTJS = "SetDefaultCb(this,'{0}');";
        private const string DEFAULTCLICKJS = "DefaultCbClick(this);";
        #endregion

        #region fields
        private int productId;
        private Company c;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            productId = CurrentPage.GetValueInt(PRODUCTIDATTRIBUTE , -1);
            c = CurrentPage.GetObject(Constants.COMPANYOBJECT) as Company;
            if (c == null)
            {
                c = new Company();
            }
            ClearErrors();
            if(!Page.IsPostBack)
            {
                BindData();
            }
        }
        private void ClearErrors()
        {
            lblServicer.Text = String.Empty;
            lblInvestor.Text = String.Empty;
        }

        private void BindData()
        {
            lblHeader.Text = String.Format(HEADERTEXT, c.Name, (new Product(productId)).Name);
            BindServicer();
            BindInvestor();
            BindTrustee();
        }
        private void BindTrustee()
        {
            rpTrustee.DataSource = Lender.GetTrusteeList(c.ID, productId);
            rpTrustee.DataBind();
        }
        private void BindServicer()
        {
            rpServicer.DataSource = Lender.GetServicerList(c.ID, productId);
            rpServicer.DataBind();
        }
        private void BindInvestor()
        {
            rpInvestor.DataSource = Lender.GetInvestorList(c.ID, productId);
            rpInvestor.DataBind();
        }
        protected void rpTrustee_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row != null)
                {
                    CheckBox cb = (CheckBox)e.Item.FindControl("cbCompany");
                    if (cb != null)
                    {
                        cb.Checked = bool.Parse(row["selected"].ToString());
                        cb.Text = row["company"].ToString();
                        cb.Attributes.Add(COMPANYID, row["id"].ToString());
                        CheckBox cbd = (CheckBox)e.Item.FindControl("cbDefault");
                        if (cbd != null)
                        {
                            cbd.Checked = bool.Parse(row["defaultservicer"].ToString());
                            cbd.Enabled = cb.Checked;
                            cb.Attributes.Add(CLICKHANDLER, String.Format(SETDEFAULTJS, cbd.ClientID));
                            cbd.Attributes.Add(CLICKHANDLER, DEFAULTCLICKJS);
                        }
                    }
                }
            }
        }
        protected void rpServicer_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row != null)
                {
                    CheckBox cb = (CheckBox)e.Item.FindControl("cbCompany");
                    if (cb != null)
                    {
                        cb.Checked = bool.Parse(row["selected"].ToString());
                        cb.Text = row["company"].ToString();
                        cb.Attributes.Add(COMPANYID, row["id"].ToString());
                        CheckBox cbd = (CheckBox)e.Item.FindControl("cbDefault");
                        if (cbd != null)
                        {
                            cbd.Checked = bool.Parse(row["defaultservicer"].ToString());
                            cbd.Enabled = cb.Checked;
                            cb.Attributes.Add(CLICKHANDLER, String.Format(SETDEFAULTJS, cbd.ClientID));
                            cbd.Attributes.Add(CLICKHANDLER, DEFAULTCLICKJS);
                        }
                    }
                }
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.EDITCOMPANY);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string servicerList;
            string investorList;
            string trusteeList;
            if(Validate(out servicerList, out investorList, out trusteeList))
            {
                if (c.SaveServicerInvestorTrustee(productId, servicerList, investorList, trusteeList))
                {
                    lblMessage.Text = Constants.SUCCESSMESSAGE;
                }
            }
        }
        private bool Validate(out string servicerList, out string investorList, out string trusteeList)
        {
            int err;
            bool res = true;
            servicerList = GetRepeaterList(rpServicer, out err);
            if(err>0)
            {
                switch (err)
                {
                    case NOTSELECTED:
                        lblServicer.Text = "Please select at leats one servicer";
                        break;
                    case NODEFAULT:
                        lblServicer.Text = "Please select default servicer";
                        break;
                    case MUCHDEFAULT:
                        lblServicer.Text = "Only one servicer can be set default";
                        break;
                }
                res = false;
            }
            
            investorList = GetRepeaterList(rpInvestor, out err);
            if (err > 0)
            {
                switch (err)
                {
                    case NOTSELECTED:
                        lblInvestor.Text = "Please select at leats one investor";
                        break;
                    case NODEFAULT:
                        lblInvestor.Text = "Please select default investor";
                        break;
                    case MUCHDEFAULT:
                        lblInvestor.Text = "Only one investor can be set default";
                        break;
                }
                res = false;
            }
            trusteeList = GetRepeaterList(rpTrustee, out err);
            if (err > 0)
            {
                switch (err)
                {
                    case NOTSELECTED:
                        lblTrustee.Text = "Please select at leats one trustee";
                        break;
                    case NODEFAULT:
                        lblTrustee.Text = "Please select default trustee";
                        break;
                    case MUCHDEFAULT:
                        lblTrustee.Text = "Only one trustee can be set default";
                        break;
                }
                res = false;
            }
            return res;
        }
        private static string GetRepeaterList(Repeater rp, out int err)
        {
            string res = String.Empty;
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            int cntall = 0;
            int cntdefault = 0;
            err = NOERROR;
            for (int i = 0; i < rp.Items.Count; i++)
            {
                CheckBox cb = (CheckBox)rp.Items[i].FindControl("cbCompany");
                if ((cb != null) && (cb.Checked))
                {
                    XmlNode n = d.CreateElement(ITEMELEMENT);
                    XmlAttribute a = d.CreateAttribute(COMPANYID);
                    a.Value = cb.Attributes[COMPANYID];
                    n.Attributes.Append(a);
                    cntall++;
                    a = d.CreateAttribute(ISDEFAULT);
                    string val = "0";
                    cb = (CheckBox)rp.Items[i].FindControl("cbDefault");
                    if ((cb != null) && (cb.Checked))
                    {
                        cntdefault++;
                        val = "1";
                    }
                    a.Value = val;
                    n.Attributes.Append(a);
                    root.AppendChild(n);
                }
            }
            if ((cntall > 0) && (cntdefault==1))
            {
                d.AppendChild(root);
                res = d.OuterXml;
            }
            else
            {
                if (cntall == 0)
                {
                    err = NOTSELECTED;
                }
                else if (cntdefault==0)
                {
                    err = NODEFAULT;
                }
                else
                {
                    err = MUCHDEFAULT;
                }
            }
            return res;
        }
    }
}

