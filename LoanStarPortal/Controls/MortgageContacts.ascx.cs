using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class MortgageContacts : MortgageDataControl
    {
        #region constants
        private const string ROLEIDATTRIBUTE = "roleid";
        private const string ROLENAMEFIELDNAME = "name";
        private const string USERNAMEFIELDNAME = "username";
        private const string IDFIELDNAME = "id";
        private const string NOTSELECTEDTEXT = "-Select-";
        private const int NOTSELECTEDVALUE = -1;
        #endregion

        private DataView dvRoles;
        private DataView dvUsers;
        private bool isManager;
        private bool needSave = false;

        protected DataView DvRoles
        { 
            get
            {
                if (dvRoles == null)
                {
                    dvRoles = Role.GetMortgageRoleList();
                }
                return dvRoles;
            }
        }
        protected DataView DvUsers
        {
            get
            {
                if (dvUsers == null)
                {
                    dvUsers = Role.GetOperationUserList(CurrentUser.CompanyId);
                }
                return dvUsers;
            }
        }

        public override void BuildControl(Control _container)
        {
            container = _container;
            _container.Controls.Add(GetContactsControl());
        }
        private Control GetContactsControl()
        {
            ControlWrapper wrapper = (ControlWrapper)LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
            wrapper.ID = "Contacts_" + Mp.ID;
            GetPostedData();
            if (needSave)
            {
                SaveData();
            }
            BindData();
            wrapper.Controls.Add(phContacts);
            return wrapper;
        }
        private void BindData()
        {
            rpAsignee.DataSource = DvRoles;
            rpAsignee.DataBind();
        }
        protected void rpAsignee_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row != null)
                {
                    Label lbl = (Label)e.Item.Controls[1];
                    lbl.Text = row[ROLENAMEFIELDNAME].ToString();
                    int roleId = int.Parse(row[IDFIELDNAME].ToString());
                    isManager = CurrentUser.IsMortgageManager();
                    DropDownList ddl = (DropDownList)e.Item.Controls[3];
                    lbl = (Label)e.Item.Controls[5];
                    Object o = null;
                    DvUsers.RowFilter = ROLEIDATTRIBUTE + "='" + roleId + "'";
                    HtmlInputButton btn = (HtmlInputButton)e.Item.Controls[7];
                    bool buttonVisible = false;
                    if (Mp.AssignedUsers != null)
                    {
                        o = Mp.AssignedUsers[roleId];
                    }
                    if (isManager)
                    {
                        ddl.ID = "ur_" + roleId;
                        ddl.DataSource = DvUsers;
                        ddl.DataTextField = USERNAMEFIELDNAME;
                        ddl.DataValueField = IDFIELDNAME;
                        ddl.DataBind();
                        ddl.Items.Insert(0, new ListItem(NOTSELECTEDTEXT, NOTSELECTEDVALUE.ToString()));
                        if (o != null)
                        {
                            ddl.SelectedValue = o.ToString();
                        }                        
                    }
                    else
                    {
                        string userName = "N/A";
                        if (o != null)
                        {
                            DvUsers.RowFilter = DvUsers.RowFilter + " and " + IDFIELDNAME + "='" + o + "'";
                            if (DvUsers.Count == 1)
                            {
                                userName = DvUsers[0][USERNAMEFIELDNAME].ToString();                                
                            }
                        }
                        else
                        {
                            buttonVisible = CurrentUser.IsInRole(roleId);
                        }
                        lbl.Text = userName;
                        lbl.ID = "uid_" + roleId;
                    }
                    ddl.Visible = isManager;
                    lbl.Visible = !isManager;
                    btn.Visible = buttonVisible;
                    if (btn.Visible)
                    {
                        string fullName = CurrentUser.FirstName.Trim() + " " + CurrentUser.LastName;
                        btn.Attributes.Add(ONCLICK, "AssignUser(this,\"" + fullName + "\"," + roleId + ",'" + "Contacts_" + Mp.ID + "');");
                    }
                }
            }
        }
        public void GetPostedData()
        {
            GetContactsData();
        }
        private void GetContactsData()
        {
            isManager = CurrentUser.IsMortgageManager();
            string[] postData = Page.Request.Form.AllKeys;
            if (isManager)
            {
                string search = "$Contacts_" + Mp.ID + "$";
                for (int i = 0; i < postData.Length; i++)
                {
                    int k = postData[i].IndexOf(search);
                    if (k > 0)
                    {
                        string ddlName = postData[i].Substring(k + search.Length);
                        k = ddlName.LastIndexOf("_");
                        if (k > 0)
                        {
                            int roleId = int.Parse(ddlName.Substring(k + 1));
                            int userId = int.Parse(Page.Request[postData[i]]);
                            if (userId > 0)
                            {
                                if (Mp.AssignedUsers.ContainsKey(roleId))
                                {
                                    Mp.AssignedUsers[roleId] = userId;
                                }
                                else
                                {
                                    Mp.AssignedUsers.Add(roleId, userId);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                string search = "$Contacts_" + Mp.ID + "$assignMe";
                for (int i = 0; i < postData.Length; i++)
                {
                    if (postData[i].Contains(search))
                    {
                        string[] roles = Page.Request[postData[i]].Split(',');
                        for (int k = 0; k < roles.Length; k++)
                        {
                            string role = roles[k].Trim();
                            if (!String.IsNullOrEmpty(role))
                            {
                                try
                                {
                                    int roleId = int.Parse(role);
                                    if (Mp.AssignedUsers.ContainsKey(roleId))
                                    {
                                        Mp.AssignedUsers[roleId] = CurrentUser.Id;
                                    }
                                    else
                                    {
                                        Mp.AssignedUsers.Add(roleId, CurrentUser.Id);
                                    }
                                    needSave = true;
                                }
                                catch { }
                            }
                        }
                        break;
                    }
                }
            }
        }
        public bool SaveData()
        {
            bool res = Mp.SaveContacts() > 0;
            if (res)
            {
                dvUsers = null;
                dvRoles = null;
            }
            return res;
        }
    }
}