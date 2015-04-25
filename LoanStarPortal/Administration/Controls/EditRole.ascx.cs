using System;
using System.Web.UI.WebControls;
using System.Xml;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditRole : AppControl
    {
        #region constants
        private const bool ROLETEMPLATE = true;
        private const string STYLEATTRIBUTE = "style";
        private const string DISPALYBLOCK = "display:block";
        private const string ROOTELEMENT = "Root";
        private const string ITEMELEMENT = "item";
        private const string IDATTRIBUTE = "id";
        private const string DATALIST = "DataList1";
        private const string EDITHEADERTEXT = "Edit role({0})";
        private const string DDLSTATUSCONTROL = "ddlStatus";
        #endregion

        #region fileds
        private Role role;
        private int companyId = -1;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(CurrentUser.IsCorrespondentLenderAdmin || CurrentUser.LoggedAsOriginator))
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            companyId = CurrentUser.EffectiveCompanyId;
            role  = GetRole();
            if (!Page.IsPostBack)
            {
                lblHeader.Text = String.Format(EDITHEADERTEXT, role.Name);
                divRoleInfo.Attributes.Add(STYLEATTRIBUTE, DISPALYBLOCK);
                MpField1.RebindData();
            }
            else
            {
                divRoleInfo.Attributes.Add(STYLEATTRIBUTE, DISPALYBLOCK);
            }
        }
        #region methods
        protected void btnBack_Click(object sender, EventArgs e)
        {
            goBack();            
        }
        private void goBack()
        {
            Response.Redirect("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.VIEWROLE);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            role.CompanyId = companyId;
            CurrentPage.StoreObject(role, Constants.ROLEOBJECT);
            DropDownList ddlstatus = (DropDownList)MpField1.FindControl(DDLSTATUSCONTROL);
            if (SaveFieldsInfo(int.Parse(ddlstatus.SelectedValue)))
            {
                lblMessage.Text = Constants.SUCCESSMESSAGE;
            }
            else
            {
                lblMessage.Text = role.LastError;
            }
            divRoleInfo.Attributes.Add(STYLEATTRIBUTE, DISPALYBLOCK);
            MpField1.RebindData();
        }
        private Role GetRole()
        {
            Role r = CurrentPage.GetObject(Constants.ROLEOBJECT) as Role;
            if (r != null)
            {
                return r;
            }
            r = new Role(!ROLETEMPLATE);
            CurrentPage.StoreObject(r, Constants.ROLEOBJECT);
            return r;
        }
        private bool SaveFieldsInfo(int statusid)
        {
            String[] col = Page.Request.Form.AllKeys;
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].IndexOf(DATALIST) > 0)
                {
                    int j = col[i].LastIndexOf('$');
                    if (j > 0)
                    {
                        XmlNode n = d.CreateElement(ITEMELEMENT);
                        XmlAttribute a = d.CreateAttribute(IDATTRIBUTE);
                        a.Value = col[i].Substring(j + 1);
                        n.Attributes.Append(a);
                        root.AppendChild(n);
                    }
                }
            }
            if (root.ChildNodes.Count > 0)
            {
                d.AppendChild(root);                
            }
            return role.SaveFieldList(d.OuterXml, statusid);
        }
        #endregion
    }
}