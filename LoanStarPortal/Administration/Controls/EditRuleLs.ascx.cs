using System;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditRuleLs : AppControl
    {
        #region constants
        private const string ADDHEADERTEXT = "Add rule";
        private const string EDITHEADERTEXT = "Edit rule({0})";
        private const string ISINITIATED = "init";
        private const string CLICKHANDLER = "onclick";
        private const string JSSETDATES = "SetDates()";
        private const string FIELDNEEDEDMESSAGE = "*";
        private const string WRONGDATESMESSAGE = "Date From should be less then Date To";
        private const string CANTSAVEMESSAGE = "Can't save rule";
        private const string ALREADYEXISTSMESSAGE = "Rule with such name already exists";
        private const string ROOTELEMENT = "Root";
        private const string ITEMELEMENT = "item";
        private const string IDATTRIBUTE = "id";
        private const string CHECHALLJS = "CheckAll(this,divproduct);";
        private const string CHECHPRODUCTJS = "CheckField(this,{0},divproduct);";
        private const string SELECTEDFIELDNAME = "Selected";
        private const string PRODUCTIDATTRIBUTE = "productid";
        #endregion

        public event BackHandler OnBack;
        public delegate void BackHandler();
        public event DataChange OnDataChange;
        public delegate void DataChange();

        #region fields
        LoanStar.Common.Rule rule = null;
        #endregion

        public bool IsInitiated
        {
            get 
            {
                bool res = true;
                Object o = ViewState[ISINITIATED];
                if (o != null)
                {
                    try
                    {
                        res = bool.Parse(o.ToString());
                    }
                    catch { }
                }
                else 
                {
                    ViewState[ISINITIATED] = res;
                }
                return res;
            }
            set 
            {
                ViewState[ISINITIATED] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rule = GetRule();
            cbEnableDaterange.Attributes.Add(CLICKHANDLER, JSSETDATES);
            cbAll.Attributes.Add(CLICKHANDLER, CHECHALLJS);                
        }

        #region methods
        public void Initialize()
        {
            rule = GetRule();
            tbName.Focus();
            if (rule.Id > 0)
            {
                divRuleInfo.Visible = true;
                EditRule1.Initialize();
            }
            else
            {
                divRuleInfo.Visible = false;
            }
            BindData();
        }
        private void goBack()
        {
            if (OnBack != null)
            {
                OnBack();
            }
        }
        private LoanStar.Common.Rule GetRule()
        {
            LoanStar.Common.Rule r = CurrentPage.GetObject(Constants.RULEOBJECT) as LoanStar.Common.Rule;
            if (r == null)
            {
                r = new LoanStar.Common.Rule();
            }
            return r;
        }
        protected void BindData()
        {
            cbEnableDaterange.Checked = rule.StartDate != null;
            dlProducts.DataSource = rule.GetProductList();
            dlProducts.DataBind();
            if (rule.Id > 0)
            {
                lblHeader.Text = String.Format(EDITHEADERTEXT,rule.Name);
                tbName.Text = rule.Name;
                if (rule.StartDate != null)
                {
                    raddpFrom.SelectedDate = (System.DateTime)rule.StartDate;
                    raddpTo.SelectedDate = (System.DateTime)rule.EndDate;
                }
            }
            else
            {
                lblHeader.Text = ADDHEADERTEXT;
            }
        }
        private bool Validate()
        {
            bool res = true;
            lbltberr.Text = String.Empty;
            lblerrfrom.Text = String.Empty;
            lblerrto.Text = String.Empty;
            lblMessage.Text = String.Empty;
            if (tbName.Text == String.Empty) 
            {
                lbltberr.Text = FIELDNEEDEDMESSAGE;
                res = false;
            }
            if (cbEnableDaterange.Checked)
            {
                if (raddpFrom.IsEmpty)
                {
                    lblerrfrom.Text = FIELDNEEDEDMESSAGE;
                    res = false;
                }
                if (raddpTo.IsEmpty)
                {
                    lblerrto.Text = FIELDNEEDEDMESSAGE;
                    res = false;
                }
                if (raddpTo.SelectedDate < raddpFrom.SelectedDate)
                {
                    lblMessage.Text = WRONGDATESMESSAGE;
                    res = false;
                }
            }
            return res;
        }
        private string GetProductList()
        {
            string res = String.Empty;
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            int cnt = 0;
            for (int i = 0; i<dlProducts.Items.Count; i++)
            {
                CheckBox cb = dlProducts.Items[i].Controls[1] as CheckBox;
                if ((cb != null) && (cb.Checked))
                {
                    XmlNode n = d.CreateElement(ITEMELEMENT);
                    XmlAttribute a = d.CreateAttribute(IDATTRIBUTE);
                    a.Value = cb.Attributes[PRODUCTIDATTRIBUTE];
                    n.Attributes.Append(a);
                    root.AppendChild(n);
                    cnt++;
                }
            }
            if (cnt > 0)
            {
                d.AppendChild(root);
                res = d.OuterXml;
            }
            return res;
        }
        #endregion

        #region postback event handlers
        protected void btnBack_Click(object sender, EventArgs e)
        {
            goBack();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                rule.Name = tbName.Text;
                if (cbEnableDaterange.Checked)
                {
                    rule.StartDate = raddpFrom.SelectedDate;
                    rule.EndDate = raddpTo.SelectedDate;
                }
                else
                {
                    rule.StartDate = null;
                    rule.EndDate = null;
                }
                string productList = GetProductList();
                int res = rule.Save(productList);
                if (res > 0)
                {
                    lblMessage.Text = Constants.SUCCESSMESSAGE;
                    CurrentPage.StoreObject(rule, Constants.RULEOBJECT);
                    cbEnableDaterange.Checked = rule.StartDate != null;
                    divRuleInfo.Visible = rule.Id > 0;
                    EditRule1.Initialize();
                    if (OnDataChange != null)
                    {
                        OnDataChange();
                    }
                }
                else if (res == -1)
                {
                    lblMessage.Text = ALREADYEXISTSMESSAGE;
                }
                else
                {
                    lblMessage.Text = CANTSAVEMESSAGE; ;
                }
            }
        }
        #endregion

        #region databound event handler
        protected void dlProducts_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            if (dr != null)
            {
                int id = int.Parse(dr[LoanStar.Common.Rule.IDFIELDNAME].ToString());
                CheckBox cb = (CheckBox)e.Item.Controls[1];
                if (cb != null)
                {
                    cb.Attributes.Add(CLICKHANDLER, String.Format(CHECHPRODUCTJS,cbAll.ClientID));
                    cb.Attributes.Add(PRODUCTIDATTRIBUTE, id.ToString());
                    cb.Checked = int.Parse(dr[SELECTEDFIELDNAME].ToString()) == 1;
                }
            }
        }
        #endregion
    }
}