using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;
using Telerik.WebControls.RadTreeViewContextMenu;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditCompanyStructure : AppControl
    {
        #region constants
        private const string ADMININPUTCSS = "admininput";
        private const string DISABLEDSUFFIX = "dis";
        private const string CLICKHANDLER = "onclick";
        private const string JSSETPASSWORD = "SetPassword(this.checked,{0},{1})";
        private const string EDITCOMPANYSTRUCTURETEXT = "Edit company structure - {0}";
        private const string IDFIELDNAME = "id";
        private const string PARENTIDFIELDNAME = "parentid";
        private const string NAMEFIELDNAME = "name";
        private const string ABBRIVIATIONFIELDNAME = "abbriviation";
        private const string FULLNAMEFIELDNAME = "UserFullName";
        private const string USERIDFIELDNAME = "userid";
        private const string ROLEIDFIELDNAME = "roleid";
        private const string NODEIDFIELDNAME = "nodeid";
        private const string ROLENAME = "rolename";
        private const string EDITMENUNAME = "Edit";
        private const string DELETEMENUNAME = "Delete";
        private const string ADDMENUNAME = "Add";
        private const string COMPANYSTRUCTUREMODE = "StructureMode";
        private const string EDITUSERID = "edituserid";
        private const string EDITROLEID = "editroleid";
        private const string PARENTID = "parentid";
        private const int TREEVIEWMODE = 0;
        private const string ADDHEADERTEXT = "Add {0} new  user";
        private const string EDITHEADETEXT = "Edit {0} user({1})";
        #endregion 

        #region fiedls
        private Company company;
        private readonly ArrayList contextMenus = new ArrayList();
        private DataView roleStructure;
        private AppUser user;
        private string roleShortName = String.Empty;
        #endregion 

        #region ViewState data
        protected int Mode
        {
            get
            {
                int res = TREEVIEWMODE;
                Object o = ViewState[COMPANYSTRUCTUREMODE];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set 
            {
                ViewState[COMPANYSTRUCTUREMODE] = value;
            }
        }
        protected int EditUserId
        {
            get
            {
                int res = -1;
                Object o = ViewState[EDITUSERID];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set
            {
                ViewState[EDITUSERID] = value;
            }
        }
        protected int EditRoleId
        {
            get
            {
                int res = -1;
                Object o = ViewState[EDITROLEID];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set
            {
                ViewState[EDITROLEID] = value;
            }
        }
        protected int ParentId
        {
            get
            {
                int res = -1;
                Object o = ViewState[PARENTID];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set
            {
                ViewState[PARENTID] = value;
            }
        }
        protected int EditNodeId
        {
            get
            {
                int res = -1;
                Object o = ViewState[NODEIDFIELDNAME];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set
            {
                ViewState[NODEIDFIELDNAME] = value;
            }
        }
        #endregion

        /*
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            company = GetCompany();
            lblMessageUser.Text = "";
            CreatMenus();
            if (!IsPostBack)
            {                
                BindData();
            }
        }

        #region methods
        private void CreatMenus()
        {
            roleStructure = Role.GetRoleStructure();
            for (int i = 0; i < roleStructure.Count; i++)
            {
                contextMenus.Add(CreateMenu(roleStructure[i]));
            }
        }
        private ContextMenu CreateMenu(DataRowView dr)
        {
            ContextMenu res = new ContextMenu();
            res.Width = 150;
            int id = int.Parse(dr[IDFIELDNAME].ToString());
            res.Name = dr[ABBRIVIATIONFIELDNAME].ToString().Trim();
            res.Items.Add(GetMenuItem(EDITMENUNAME, String.Empty));
            if (int.Parse(dr[PARENTIDFIELDNAME].ToString()) != 0)
            {
                res.Items.Add(GetMenuItem(DELETEMENUNAME, String.Empty));
            }
            DataView dv = roleStructure;
            dv.RowFilter = PARENTIDFIELDNAME + "=" + id;
            for (int i = 0; i < dv.Count; i++)
            {
                string name = dv[i][NAMEFIELDNAME].ToString();
                res.Items.Add(GetMenuItem(ADDMENUNAME + " " + name, dv[i][ABBRIVIATIONFIELDNAME].ToString().Trim()+"_"+dv[i][IDFIELDNAME]));
            }
            dv.RowFilter = "";
            return res;
        }
        private static ContextMenuItem GetMenuItem(string text, string id)
        {
            ContextMenuItem item = new ContextMenuItem(text);
            item.ID = id;
            item.PostBack = true;
            return item;
        }
        private void BindData()
        {
            divedit.Visible = false;
            lblHeader.Text = String.Format(EDITCOMPANYSTRUCTURETEXT, company.Name);
//            tbMailPassword.CssClass = ADMININPUTCSS + DISABLEDSUFFIX;
//            tbMailPasswordConfirm.CssClass = ADMININPUTCSS + DISABLEDSUFFIX;
//            cbEnableMailPassword.Attributes.Add(CLICKHANDLER, String.Format(JSSETPASSWORD, tbMailPassword.ClientID, tbMailPasswordConfirm.ClientID));

            RadTreeView1.ContextMenus = contextMenus;
            RadTreeView1.DataSource = company.GetStructure();
            RadTreeView1.DataTextField = ABBRIVIATIONFIELDNAME;
            RadTreeView1.DataValueField = IDFIELDNAME;
            RadTreeView1.DataFieldID = IDFIELDNAME;
            RadTreeView1.DataFieldParentID = PARENTIDFIELDNAME;
            RadTreeView1.DataBind();
        }
        private Company GetCompany()
        {
            Company c = CurrentPage.GetObject(Constants.COMPANYOBJECT) as Company;
            if (c == null)
            {
                c = new Company(CurrentUser.EffectiveCompanyId);
                CurrentPage.StoreObject(c, Constants.COMPANYOBJECT);
            }
            return c;
        }
        private void EditNode(RadTreeNode node)
        {
            RadTreeView1.Enabled = false;
            divedit.Visible = true;
            GetNodeData(node);
            BindNodeData();
        }
        private void BindNodeData()
        {
            DataList1.DataSource = user.GetUserRoles(EditRoleId);
            DataList1.DataBind();
            bool isNew = user.Id < 0;
            if (isNew)
            {
                lblOp.Text = String.Format(ADDHEADERTEXT,roleShortName);
            }
            else
            {
                lblOp.Text = String.Format(EDITHEADETEXT, roleShortName, user.LoginName);
            }
            tbLogin.Text = user.LoginName;
            tbFirstName.Text = user.FirstName;
            tbLastName.Text = user.LastName;

            trPassword.Visible = !isNew;
            tbPassword.Enabled = !trPassword.Visible;
            tbConfirmPassword.Enabled = tbPassword.Enabled;
            tbPassword.CssClass = ADMININPUTCSS + (tbPassword.Enabled ? String.Empty : DISABLEDSUFFIX);
            tbConfirmPassword.CssClass = ADMININPUTCSS + (tbConfirmPassword.Enabled ? String.Empty : DISABLEDSUFFIX);
            if (trPassword.Visible)
            {
                cbEnablePassword.Attributes.Add(CLICKHANDLER, String.Format(JSSETPASSWORD, tbPassword.ClientID, tbConfirmPassword.ClientID));
            }

            trMailPassword.Visible = !isNew;
            tbMailPassword.Enabled = !trMailPassword.Visible;
            tbMailPasswordConfirm.Enabled = tbMailPassword.Enabled;
            tbMailPassword.CssClass = ADMININPUTCSS + (tbMailPassword.Enabled ? String.Empty : DISABLEDSUFFIX);
            tbMailPasswordConfirm.CssClass = ADMININPUTCSS + (tbMailPasswordConfirm.Enabled ? String.Empty : DISABLEDSUFFIX);
            if (trMailPassword.Visible)
            {
                cbEnableMailPassword.Attributes.Add(CLICKHANDLER, String.Format(JSSETPASSWORD, tbMailPassword.ClientID, tbMailPasswordConfirm.ClientID));
            }
            
            tbUserMail.Text = user.MailAddress;
            tbUserName.Text = user.MailUserName;

            tbLogin.Focus();
        }
        private int[] GetRoles()
        {
            int[] result = null;
            ArrayList list = new ArrayList();
            if (EditRoleId > 0)
            {
                list.Add(EditRoleId);
            }
            foreach (DataListItem item in DataList1.Items)
            {
                CheckBox cb = (CheckBox)item.FindControl("Checkbox1");
                if (cb != null && cb.Checked)
                {
                    list.Add(DataList1.DataKeys[item.ItemIndex]);
                }
            }
            if (list.Count > 0)
            {
                result = new int[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    result[i] = (int)list[i];
                }
            }
            return result;
        }
        private void GetNodeData(RadTreeNode node)
        {
            ParentId = -1;
            if (node.Parent != null)
            {
                try
                {
                    ParentId = Convert.ToInt32(node.Attributes[PARENTIDFIELDNAME]);
                }
                catch { }
            }
            int userid = -1;
            try
            {
                userid = Convert.ToInt32(node.Attributes[USERIDFIELDNAME]);
            }
            catch { }
            user = new AppUser(userid);
            EditUserId = user.Id;
            user.CompanyId = CurrentUser.EffectiveCompanyId;
            EditRoleId = -1;
            try
            {
                EditRoleId = Convert.ToInt32(node.Attributes[ROLEIDFIELDNAME]);
            }
            catch { }
            EditNodeId = -1;
            try
            {
                EditNodeId = Convert.ToInt32(node.Attributes[NODEIDFIELDNAME]);
            }
            catch { }
            roleShortName = node.Attributes[ROLENAME];
        }
        private void AddNode(WebControl node, string nodeid)
        {
            ParentId = -1;
            try
            {
                ParentId = int.Parse(node.Attributes[NODEIDFIELDNAME]);
            }
            catch { }
            EditUserId = -1;
            user = new AppUser(EditUserId);
            EditRoleId = -1;
            roleShortName = String.Empty;
            int i = nodeid.IndexOf("_");
            if (i > 0)
            {
                roleShortName = nodeid.Substring(0, i);
                try
                {
                    EditRoleId = int.Parse(nodeid.Substring(i + 1));
                }
                catch { }
            }
            RadTreeView1.Enabled = false;
            divedit.Visible = true;
            BindNodeData();

        }
        private static void DeleteNode(WebControl node)
        {
            try
            {
                int id = Convert.ToInt32(node.Attributes[NODEIDFIELDNAME]);
                AppUser.DeleteFromCompanyStructure(id);
            }
            catch { }
        }
        #endregion

        #region Treeview event handlers
        protected void RadTreeView1_NodeBound(object o, RadTreeNodeEventArgs e)
        {
            DataRowView row = (DataRowView)e.NodeBound.DataItem;
            RadTreeNode node = e.NodeBound;
            node.Attributes.Add(USERIDFIELDNAME, row[USERIDFIELDNAME].ToString());
            node.Attributes.Add(ROLEIDFIELDNAME, row[ROLEIDFIELDNAME].ToString());
            node.Attributes.Add(NODEIDFIELDNAME, row[IDFIELDNAME].ToString());
            node.Attributes.Add(PARENTID, row[PARENTIDFIELDNAME].ToString());
            node.Attributes.Add(ROLENAME, row[ABBRIVIATIONFIELDNAME].ToString());
            node.PostBack = true;
            node.Expanded = true;
            node.Text = string.Format(" {0} {1} ", row[ABBRIVIATIONFIELDNAME], row[FULLNAMEFIELDNAME]);
            node.ContextMenuName = row[ABBRIVIATIONFIELDNAME].ToString().Trim();
        }
        protected void RadTreeView1_NodeClick(object o, RadTreeNodeEventArgs e)
        {
            EditNode(e.NodeClicked);
        }

        protected void RadTreeView1_NodeContextClick(object o, RadTreeNodeEventArgs e)
        {
            string cmd = e.ContextMenuItemText;
            if (cmd.StartsWith(EDITMENUNAME))
            {
                EditNode(e.NodeClicked);
            }
            else if (cmd.StartsWith(ADDMENUNAME))
            {
                AddNode(e.NodeClicked, e.ContextMenuItemID);
            }
            else if (cmd.StartsWith(DELETEMENUNAME))
            {
                DeleteNode(e.NodeClicked);
                BindData();
            }
        }

        #endregion

        #region edit form event handlers
        protected void btnBack_Click(object sender, EventArgs e)
        {
            RadTreeView1.Enabled = true;
            BindData();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            user = new AppUser(EditUserId);
            user.CompanyId = CurrentUser.EffectiveCompanyId;
            bool isNew = user.Id < 0;
            user.LoginName = tbLogin.Text;
            if (isNew || cbEnablePassword.Checked)
            {
                user.Password = tbPassword.Text;
            }
            user.FirstName = tbFirstName.Text;
            user.LastName = tbLastName.Text;
            user.CompanyId = CurrentUser.EffectiveCompanyId;
            user.EmailAccount.FriendlyName = tbUserName.Text;
            user.MailAddress = tbUserMail.Text;
            if (isNew || cbEnableMailPassword.Checked)
            {
                user.MailPassword = tbMailPassword.Text;
            }
            int[] roles = GetRoles();
            if (roles == null)
            {
                roles = new int[1];
                roles[0] = 0;
            }
            int result = user.SaveCompanyUser(EditNodeId, ParentId, EditRoleId, roles);
            if (result < 1)
            {
                lblMessageUser.Text = user.LastError;
            }
            else
            {
                lblMessageUser.Text = Constants.SUCCESSMESSAGE;
                EditUserId = -1;
                RadTreeView1.Enabled = true;
                BindData();
            }           
        }
        #endregion
         */
    }
}