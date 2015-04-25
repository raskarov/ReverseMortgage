using System;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using System.Xml;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditCompany : AppControl
    {

        #region constants
        private const string GENERALERROR = "Can't save data";
        private const string ADDHEADERTEXT = "Add new company";
        private const string EDITHEADERTEXT = "Edit company({0})";
        private const string SELECTLOGOLABEL = "Select logo:";
        private const string CHANGELOGOLABEL = "Change logo:";
        private const string ALREADYEXISTS = "Company already exists";
        private const string CANNOTUNCHECKLENDER = "You can not uncheck Lender";
        private const string CANNOTUNCHECKSERVICER = "You can not uncheck Servicer";
        private const string CANNOTUNCHECKINVESTOR = "You can not uncheck Investor";
        private const string ONCLICK = "onclick";
        private const string ONCLICKJS = "ShowProductTr(this,'{0}');ShowAffiliateTr('{1}','{2}','{3}');";
        private const string ONORIGINATORCLICKJS = "ShowProductTr(this,'{0}');ShowProductTr(this,'{1}');ShowAffiliateTr('{2}','{3}','{4}');ShowProductTr(this,'{5}');ShowProductTr(this,'{6}');ShowProductTr(this,'{7}');ShowProductTr(this,'{8}');ShowProductTr(this,'{9}');";
        private const string ROOTELEMENT = "Root";
        private const string ITEMELEMENT = "item";
        private const string PRODUCTIDATTRIBUTE = "productid";
        private const string EDITLINKJS = "SetEditLink(this,'{0}');";
        private const string CLICKHANDLER = "onclick";
        #endregion

        #region fields
        private Company c;
        #endregion
        private bool IsNew
        {
            get
            {
                bool res = true;
                Object o = ViewState["newcompany"];
                if (o != null)
                {
                    try
                    {
                        res = bool.Parse(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set { ViewState["newcompany"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsLoanStarAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            c = CurrentPage.GetObject(Constants.COMPANYOBJECT) as Company;
            if (!IsPostBack)
            {
                string param_ctl = CurrentPage.GetValue("isnew", "");
                if (!String.IsNullOrEmpty(param_ctl))
                {
                    IsNew = true;
                    c = new Company();
                }
                else
                {
                    IsNew = false;
                    c = CurrentPage.GetObject(Constants.COMPANYOBJECT) as Company;
                    if (c == null)
                    {
                        c = new Company();
                    }
                }
                BindData();
                BindLenderProduct();
            }
            cbLender.Attributes.Add(ONCLICK,String.Format(ONCLICKJS,trProductList.ClientID,cbLender.ClientID,cbOriginator.ClientID,trManageAffiliate.ClientID));
            cbOriginator.Attributes.Add(ONCLICK, String.Format(ONORIGINATORCLICKJS, trRetail.ClientID, trOriginator.ClientID, cbLender.ClientID, cbOriginator.ClientID, trManageAffiliate.ClientID, trRequiredFields.ClientID, trRoleBasedSecurity.ClientID, trUploadLogo.ClientID, trlogo.ClientID, trLeadManagement.ClientID));
        }
        #region methods
        private void BindLenderProduct()
        {
            rpLenderProduct.DataSource = Lender.GetLenderProductList(c.ID);
            rpLenderProduct.DataBind();
        }
        protected void rpLenderProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row != null)
                {
                    CheckBox cb = (CheckBox)e.Item.FindControl("cbProduct");
                    if(cb!=null)
                    {
                        HyperLink hl = (HyperLink)e.Item.FindControl("hlEdit");
                        cb.Attributes.Add(PRODUCTIDATTRIBUTE, row["id"].ToString());
                        cb.Checked = bool.Parse(row["selected"].ToString());
                        cb.Text = row["name"].ToString();
                        cb.Attributes.Add(CLICKHANDLER, String.Format(EDITLINKJS, hl.ClientID));
                        string url = ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.VIEWLENDERPRODUCT + "&" + PRODUCTIDATTRIBUTE + "=" + row["id"]);
                        hl.NavigateUrl = url;
                        hl.Enabled = cb.Checked;
                        hl.Attributes.Add("url",url);
                    }
                }
            }
        }
        private void BindData()
        {
            if (IsNew)
            {
                lblHeader.Text = ADDHEADERTEXT;
                Label2.Text = SELECTLOGOLABEL;
                trlogo.Visible = false;
                hlManageAffiliates.Enabled = false;
            }
            else
            {
                hlManageAffiliates.Enabled = true;
                hlManageAffiliates.NavigateUrl = ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.VIEWAFFILIATE);
                lblHeader.Text = String.Format(EDITHEADERTEXT, c.Name);
                tbCompany.Text = c.Name;
                if (!String.IsNullOrEmpty(c.LogoImage))
                {
                    trlogo.Visible = true;
                    imgLogo.ImageUrl = ResolveUrl(Constants.LOGOIMAGEFOLDER + "/" + c.LogoImage);
                    Label2.Text = CHANGELOGOLABEL;
                }
                else
                {
                    trlogo.Visible = false;
                    Label2.Text = SELECTLOGOLABEL;
                }

            }
            cbRoleSettings.Checked = c.IsGlobalSettings;
            cbRequiredFields.Checked = c.IsUsingGlobalRequiredFields;
            cbOriginator.Checked = c.IsOriginator;
            cbLender.Checked = c.IsLender;
            cbInvestor.Checked = c.IsInvestor;
            cbServicer.Checked = c.IsServicer;
            cbTrustee.Checked = c.IsTrustee;
            if (cbOriginator.Checked || cbLender.Checked)
            {
                trManageAffiliate.Attributes.Add("style", "display:block");
            }
            else
            {
                trManageAffiliate.Attributes.Add("style", "display:none");
            }

            if(cbLender.Checked)
            {
                trProductList.Attributes.Add("style", "display:block");
            }
            else
            {
                trProductList.Attributes.Add("style", "display:none");
            }
            cbRetailTools.Checked = c.IsRetailEnabled;
            if (cbOriginator.Checked)
            {
                trRetail.Attributes.Add("style", "display:block");
                trRequiredFields.Attributes.Add("style", "display:block");
                trUploadLogo.Attributes.Add("style", "display:block");
                trOriginator.Attributes.Add("style", "display:block");
                trRoleBasedSecurity.Attributes.Add("style", "display:block");
                trLeadManagement.Attributes.Add("style","display:block");
                if (trlogo.Visible)
                {
                    trlogo.Attributes.Add("style", "display:block");
                }
            }
            else 
            {
                trRetail.Attributes.Add("style", "display:none");
                trRequiredFields.Attributes.Add("style", "display:none");
                trUploadLogo.Attributes.Add("style", "display:none");
                trOriginator.Attributes.Add("style", "display:none");
                trLeadManagement.Attributes.Add("style", "display:none");
                trRoleBasedSecurity.Attributes.Add("style", "display:none");
            }
            cbPreUnderwritingQuestions.Checked = c.PreUnderwritingQuestions;
            cbIsLeadManagementEnabled.Checked = c.IsLeadManagementEnabled;
        }
        private string SaveImage()
        {
            string result = String.Empty;
            if (UploadLogoImage.HasFile && UploadLogoImage.PostedFile.ContentLength > 0)
            {
                result = UploadFile(UploadLogoImage, Constants.LOGOIMAGEFOLDER);
            }
            return result;
        }
        private string UploadFile(FileUpload uploadFile, string path)
        {
            string fname = uploadFile.FileName.Substring(0, uploadFile.FileName.LastIndexOf('.'));
            string ext = uploadFile.FileName.Substring(uploadFile.FileName.LastIndexOf('.') + 1);
            string newFileName = fname + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + "." + ext;
            string str = Server.MapPath(path) + "\\" + newFileName;
            FileInfo fi = new FileInfo(str);
            if (fi.Exists)
            {
                File.Delete(str);
            }
            uploadFile.SaveAs(str);
            return newFileName;
        }
        private void goBack()
        {
            Response.Redirect("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.VIEWCOMPANY);
        }
        #endregion

        #region event handlers
        protected void btnBack_Click(object sender, EventArgs e)
        {
            goBack();
            
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if(Validate())
            {
                if (IsNew)
                {
                    c = new Company();
                }
                else
                {
                    c = CurrentPage.GetObject(Constants.COMPANYOBJECT) as Company;
                    if (c == null)
                    {
                        c = new Company();
                    }
                }
                c.Name = tbCompany.Text;
                c.IsInvestor = cbInvestor.Checked;
                c.IsLender = cbLender.Checked;
                c.IsOriginator = cbOriginator.Checked;
                c.IsServicer = cbServicer.Checked;
                c.IsTrustee = cbTrustee.Checked;
                c.IsGlobalSettings = cbRoleSettings.Checked;
                c.IsUsingGlobalRequiredFields = cbRequiredFields.Checked;
                if (cbRetailTools.Visible) 
                {
                    c.IsRetailEnabled = cbRetailTools.Checked;
                }
                if (c.IsOriginator)
                {
                   c.PreUnderwritingQuestions = cbPreUnderwritingQuestions.Checked;
                }
                c.IsLeadManagementEnabled = cbIsLeadManagementEnabled.Checked;
                try
                {
                    string logoImg = SaveImage();
                    if (!String.IsNullOrEmpty(logoImg))
                    {
                        c.LogoImage = logoImg;
                        trlogo.Visible = true;
                        imgLogo.ImageUrl = ResolveUrl(Constants.LOGOIMAGEFOLDER + "/" + c.LogoImage);
                    }
                    string productList = String.Empty;
                    if(c.IsLender)
                    {
                        productList = GetProductList();
                    }
                    int result = c.Save(productList);
                    if (result > 0)
                    {
                        IsNew = false;
                        CurrentPage.StoreObject(c, Constants.COMPANYOBJECT);
                        lblMessage.Text = Constants.SUCCESSMESSAGE;
                        BindLenderProduct();
                    }
                    else if (result == -1)
                    {
                        lblMessage.Text = ALREADYEXISTS;
                    }
                    else if (result == -2)
                    {
                        lblMessage.Text = CANNOTUNCHECKLENDER;
                    }
                    else if (result == -3)
                    {
                        lblMessage.Text = CANNOTUNCHECKINVESTOR;
                    }
                    else if (result == -4)
                    {
                        lblMessage.Text = CANNOTUNCHECKSERVICER;
                    }
                    else
                    {
                        lblMessage.Text = GENERALERROR;
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                }
                BindData();
            }
        }
        private string GetProductList()
        {
            string res = String.Empty;
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            int cnt = 0;
            for (int i = 0; i < rpLenderProduct.Items.Count; i++)
            {
                CheckBox cb = (CheckBox)rpLenderProduct.Items[i].FindControl("cbProduct");
                if ((cb != null) && (cb.Checked))
                {
                    XmlNode n = d.CreateElement(ITEMELEMENT);
                    XmlAttribute a = d.CreateAttribute(PRODUCTIDATTRIBUTE);
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

        private bool Validate()
        {
            bool res = cbOriginator.Checked || cbLender.Checked || cbInvestor.Checked || cbServicer.Checked || cbTrustee.Checked;
            if (!res)
            {
                lblMessage.Text = "At least one checkbox should be checked.";
            }
            return res;
        }
        #endregion

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            c.RemoveLogo();
            BindData();
        }

    }
}