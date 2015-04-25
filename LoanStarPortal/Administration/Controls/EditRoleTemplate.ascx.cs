using System;
using System.Web.UI.WebControls;
using System.Xml;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditRoleTemplate : AppControl
    {
        #region constants
        private const bool ROLETEMPLATE = true;
        private const string ADDHEADERTEXT = "Add new role template";
        private const string EDITHEADERTEXT = "Edit role template({0})";
        private const string STYLEATTRIBUTE = "style";
        private const string DISPALYNONE = "display:none";
        private const string DISPALYBLOCK = "display:block";
        private const string ROOTELEMENT = "Root";
        private const string ITEMELEMENT = "item";
        private const string IDATTRIBUTE = "id";
        private const string DATALIST = "DataList1";
        private const string DDLSTATUSCONTROL = "ddlStatus";
        #endregion

        #region fields
        bool isNew;
        private Role role;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsLoanStarAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            role = GetRole();
            isNew = role.Id < 0;
            if (!Page.IsPostBack)
            {
                if (isNew)
                {
                    divRoleInfo.Attributes.Add(STYLEATTRIBUTE, DISPALYNONE);
                    lblHeader.Text = ADDHEADERTEXT;
                }
                else
                {
                    lblHeader.Text = String.Format(EDITHEADERTEXT, role.Name);
                    tbName.Text = role.Name;
                    divRoleInfo.Attributes.Add(STYLEATTRIBUTE, DISPALYBLOCK);
                    MpField1.RebindData();
                }
            }
            else
            {
                if (!isNew)
                {
                    divRoleInfo.Attributes.Add(STYLEATTRIBUTE, DISPALYBLOCK);
                }
            }
        }
        #region methods
        private void goBack()
        {
            Response.Redirect("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.VIEWROLE);
        }
        private bool SaveFieldsInfo(int statusid)
        {
            String[] col = Page.Request.Form.AllKeys;
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].IndexOf("$" + DATALIST + "$") > 0)
//                if ((col[i].StartsWith(ClientID + "$" + MpField1.ID + @"$g")) && (col[i].IndexOf(DATALIST)>0))
                {
                    int j = col[i].LastIndexOf('$');
                    if (j > 0)
                    {
                        XmlNode n = d.CreateElement(ITEMELEMENT);
                        XmlAttribute a = d.CreateAttribute(IDATTRIBUTE);
                        a.Value = col[i].Substring(j+1);
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
        private Role GetRole()
        {
            Role r = CurrentPage.GetObject(Constants.ROLEOBJECT) as Role;
            if (r != null)
            {
                return r;
            }
            r = new Role(ROLETEMPLATE);
            CurrentPage.StoreObject(r, Constants.ROLEOBJECT);
            return r;
        }
        #endregion

        #region event handlers
        protected void btnBack_Click(object sender, EventArgs e)
        {
            goBack();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            role.Name = tbName.Text;
            if (role.Save())
            {
                CurrentPage.StoreObject(role, Constants.ROLEOBJECT);
                if (!isNew)
                {
                    DropDownList ddlstatus = (DropDownList)MpField1.FindControl(DDLSTATUSCONTROL);
                    if (SaveFieldsInfo(int.Parse(ddlstatus.SelectedValue)))
                    {
                        lblMessage.Text = Constants.SUCCESSMESSAGE;
                    }
                    else
                    {
                        lblMessage.Text = role.LastError;
                    }
                }
                isNew = false;
                divRoleInfo.Attributes.Add(STYLEATTRIBUTE, DISPALYBLOCK);
                MpField1.RebindData();
            }
            else
            {
                lblMessage.Text = role.LastError;
            }
        }
        #endregion
    }
}