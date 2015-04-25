using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class MortgageProfileContacts : AppControl
    {
        #region constants
        private const string ROLEIDATTRIBUTE = "roleid";
        private const string ROLENAMEFIELDNAME = "name";
        private const string USERNAMEFIELDNAME = "username";
        private const string IDFIELDNAME = "id";
        private const string NOTSELECTEDTEXT = "-Select-";
        private const int NOTSELECTEDVALUE = -1;
        protected const string ONCLICK = "onclick";
        private const string POSTBACKCONTROL = "__EVENTTARGET";
        private const string assignMeId = ":btnAssignMe_";
        #endregion

        private DataView dvRoles;
        private DataView dvUsers;
        private MortgageProfile mp;
        private bool isManager;
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
        protected int MortgageId
        {
            get
            {
                return Convert.ToInt32(Session[Constants.MortgageID]);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            mp = CurrentPage.GetMortgage(MortgageId);
            BindData();
            if ( GetPostedData())
            {
                bool res = mp.SaveContacts() > 0;
                if (res)
                {
                    dvUsers = null;
                    dvRoles = null;
                    BindData();
                }
            }
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
                    btn.ID += "_" + roleId;
                    bool buttonVisible = false;
                    if (mp.AssignedUsers != null)
                    {
                        o = mp.AssignedUsers[roleId];
                    }
                    if (isManager)
                    {
                        ddl.ID = "ur_" + roleId;
                        ddl.AutoPostBack = true;
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
                    //if (btn.Visible)
                    //{
                    //    string fullName = CurrentUser.FirstName.Trim() + " " + CurrentUser.LastName;
                    //    btn.Attributes.Add(ONCLICK,"AssignUser("+ roleId + ",'" + assignMe.ClientID + "');");
                    //}
                }
            }
        }
        private bool GetPostedData()
        {
            bool res = false;
            string ctlName = Page.Request.Form[POSTBACKCONTROL];
            if (!String.IsNullOrEmpty(ctlName))
            {
                int i = ctlName.IndexOf(assignMeId);
                if ( i > 0)
                {
                    try
                    {
                        string s = ctlName.Substring(i + assignMeId.Length);
                        int roleId = int.Parse(s);
                        if (mp.AssignedUsers.ContainsKey(roleId))
                        {
                            mp.AssignedUsers[roleId] = CurrentUser.Id;
                        }
                        else
                        {
                            mp.AssignedUsers.Add(roleId, CurrentUser.Id);
                        }
                        res = true;
                    }
                    catch
                    {
                    }
                }
                else if (ctlName.IndexOf(":" + rpAsignee.ID + ":")>0)
                {
                    try
                    {
                        int userId = int.Parse(Page.Request.Form[ctlName.Replace(":", "$")]);
                        int roleId = int.Parse(ctlName.Substring(ctlName.LastIndexOf(":ur_") + 4));
                        if (userId > 0)
                        {
                            if (mp.AssignedUsers.ContainsKey(roleId))
                            {
                                mp.AssignedUsers[roleId] = userId;
                            }
                            else
                            {
                                mp.AssignedUsers.Add(roleId, userId);
                            }
                            res = true;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            return res;
        }

    //    private bool GetPostedData1()
    //    {
    //        bool res = false;
    //        isManager = CurrentUser.IsMortgageManager();
    //        string[] postData = Page.Request.Form.AllKeys;
    //        if (isManager)
    //        {
    //            string search = "$Contacts_" + mp.ID + "$";
    //            for (int i = 0; i < postData.Length; i++)
    //            {
    //                int k = postData[i].IndexOf(search);
    //                if (k > 0)
    //                {
    //                    string ddlName = postData[i].Substring(k + search.Length);
    //                    k = ddlName.LastIndexOf("_");
    //                    if (k > 0)
    //                    {
    //                        int roleId = int.Parse(ddlName.Substring(k + 1));
    //                        int userId = int.Parse(Page.Request[postData[i]]);
    //                        if (userId > 0)
    //                        {
    //                            if (mp.AssignedUsers.ContainsKey(roleId))
    //                            {
    //                                mp.AssignedUsers[roleId] = userId;
    //                            }
    //                            else
    //                            {
    //                                mp.AssignedUsers.Add(roleId, userId);
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            string search = "$Contacts_" + mp.ID + "$assignMe";
    //            for (int i = 0; i < postData.Length; i++)
    //            {
    //                if (postData[i].Contains(search))
    //                {
    //                    string[] roles = Page.Request[postData[i]].Split(',');
    //                    for (int k = 0; k < roles.Length; k++)
    //                    {
    //                        string role = roles[k].Trim();
    //                        if (!String.IsNullOrEmpty(role))
    //                        {
    //                            try
    //                            {
    //                                int roleId = int.Parse(role);
    //                                if (mp.AssignedUsers.ContainsKey(roleId))
    //                                {
    //                                    mp.AssignedUsers[roleId] = CurrentUser.Id;
    //                                }
    //                                else
    //                                {
    //                                    mp.AssignedUsers.Add(roleId, CurrentUser.Id);
    //                                }
    //                                res = true;
    //                            }
    //                            catch { }
    //                        }
    //                    }
    //                    break;
    //                }
    //            }
    //        }
    //        return res;
    //    }
    }
}