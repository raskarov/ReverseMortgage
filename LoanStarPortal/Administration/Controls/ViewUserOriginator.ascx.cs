using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;
using System.Web;

namespace LoanStarPortal.Administration.Controls
{
    public partial class ViewUserOriginator : AppControl
    {
        #region constants
        private const string DELETECOMMAND = "delete";
        private const string DISABLECOMMAND = "disable";
        private const string ENABLECOMMAND = "enable";
        private const string UNLOCKCOMMAND = "unlock";
        private const string EDITCOMMAND = "edituser";
        private const string ADDCOMMAND = "adduser";
        private const string USERSTATUSIDFIELDNAME = "userStatusId";
        private const string DISABLEIMAGE = "disable.gif";
        private const string ENABLEIMAGE = "enable.gif";
        private const string ENABLEALTTEXT = "Enable user";
        private const string DISABLEALTTEXT = "Disable user";
        private const string UNLOCKTEXT = "Unlock user";
        private const int ACTIONCOLUMN = 19;
        private const string ONCLICKATTRIBUTE = "OnClick";
/*
        private const int DISABLESTATUSID = 2;
*/
        private const int ENABLESTATUSID = 1;
        private const int MODEVIEW = 0;
        private const int MODEEDIT = 1;
        private const string GRIDMODE = "gridmode";
        private const string GRIDFILTER = "gridfilter";
        private const string CURRENTUSER = "currentuser";
        #endregion

        #region fields
        private DataView dvStatus;
        private DataView dvRole;
/*
        private bool isManagerGridCommand = false;
*/
        #endregion

        #region properties
        protected DataView DvStatus
        {
            get
            {
                if (dvStatus==null)
                {
                    dvStatus = AppUser.GetStatusList(false);
                }
                return dvStatus;
            }
        }
        protected DataView DvRole
        {
            get
            {
                if(dvRole==null)
                {
                    dvRole = Role.GetListForSelect();
                }
                return dvRole;
            }
        }
        protected int GridMode
        {
            get
            {
                int res = MODEVIEW;
                Object o = ViewState[GRIDMODE];
                if (o!=null)
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
            set { ViewState[GRIDMODE] = value; }
        }
        public AppUser EditOriginatorUser
        {
            get
            {
                return Session[CURRENTUSER] as AppUser;
            }
            set { Session[CURRENTUSER] = value; }
        }
        protected string GridFilter
        {
            get
            {
                string res = String.Empty;
                Object o = ViewState[GRIDFILTER];
                if (o!=null)
                {
                    res = o.ToString();
                }
                return res;
            }
            set { ViewState[GRIDFILTER] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            if (!IsPostBack)
            {
                BindData();
            }
            //else
            //{
            //    if (ProcessRequest())
            //    {
            //        EditUserOriginator1.ProcessRelationRequest();
            //    }
            //}
            //SetControls();
        }
        //private bool ProcessRequest()
        //{
        //    bool res = false;
        //    if(!String.IsNullOrEmpty(Page.Request["__EVENTTARGET"]))
        //    {
        //        string cmd = Page.Request["__EVENTTARGET"];
        //        res = cmd.IndexOf("$gEmployee$") > 0;
        //    }
        //    isManagerGridCommand = res;
        //    return res;
        //}

        //private void SetControls()
        //{
        //    trFilter.Visible = GridMode == MODEVIEW;
        //    trEdit.Visible = !trFilter.Visible;
        //    trButtons.Visible = !trFilter.Visible;
        //}
        private void BindData()
        {
            BindGrid();
            BindDictionaries();
        }
        private void BindGrid()
        {
            DataView dv = AppUser.GetLenderUserListForGrid(CurrentUser.EffectiveCompanyId);
            dv.RowFilter = GridFilter;
            G.DataSource = dv;
            G.DataBind();
        }
        private void BindDictionaries()
        {
            ddlStatus.DataSource = DvStatus;
            ddlStatus.DataValueField = "id";
            ddlStatus.DataTextField = "statusname";
            ddlStatus.DataBind();
            ddlRole.DataSource = DvRole;
            ddlRole.DataValueField = "id";
            ddlRole.DataTextField = "name";
            ddlRole.DataBind();
        }
        private void SetFilter()
        {
            StringBuilder sb = new StringBuilder();
            AppendTextBoxCondition(sb, tbLogin.Text, AppUser.LOGINFIELDNAME);
            AppendTextBoxCondition(sb, tbFirstName.Text, AppUser.FIRSTNAMEFIELDNAME);
            AppendTextBoxCondition(sb, tbLastName.Text, AppUser.LASTNAMEFIELDNAME);
            AppendSelectCondition(sb, ddlStatus.SelectedValue, AppUser.STATUSIDFIELDNAME);
            int roleId = int.Parse(ddlRole.SelectedValue);
            if (roleId!=0)
            {
                if(sb.Length>0)
                {
                    sb.Append(" and ");
                }
                string role = "";
                switch(roleId)
                {
                    case AppUser.CORRESPONDENTLENDERROLEID:
                        role = "OriginatorAdminRole";
                        break;
                    case AppUser.LOANOFFICERROLEID:
                        role = "LoanOfficerRole";
                        break;
                    case AppUser.LOANMANAGERROLEID:
                        role = "LoanManagerRole";
                        break;
                    case AppUser.LOANOFFICERASSISTANTROLEID:
                        role = "LoanOfficerAssistantRole";
                        break;
                    case AppUser.OPERATIONSMANAGERROLEID:
                        role = "OperationsManagerRole";
                        break;
                    case AppUser.PROCESSINGMANAGERROLEID:
                        role = "ProcessingManagerRole";
                        break;
                    case AppUser.PROCESSORROLEID:
                        role = "ProcessorRole";
                        break;
                    case AppUser.UNDERWRITINGMANAGERROLEID:
                        role = "UnderwritingManagerRole";
                        break;
                    case AppUser.UNDERWRITERROLEID:
                        role = "UnderwriterRole";
                        break;
                    case AppUser.CLOSINGMANAGERROLEID:
                        role = "ClosingManagerRole";
                        break;
                    case AppUser.CLOSERROLEID:
                        role = "CloserRole";
                        break;
                    case AppUser.POSTCLOSERROLEID:
                        role = "PostCloserRole";
                        break;
                    case AppUser.POSTCLOSINGMANAGERROLEID:
                        role = "PostClosingManagerRole";
                        break;
                    case AppUser.EXECUTIVEMANAGERROLEID:
                        role = "ExecutiveManagerRole";
                        break;
                }
                if (!String.IsNullOrEmpty(role))
                {
                    sb.Append(role + "<>''");
                }
            }
            GridFilter = sb.ToString();
        }
        //private void CancelEdit()
        //{
        //    GridMode = MODEVIEW;
        //    G.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Bottom;
        //    EditUserOriginator1.CurrentTab = 0;
        //    SetControls();
        //    EditOriginatorUser = null;
        //    BindGrid();
        //}
        //private void SwitchToEditMode(AppUser user)
        //{
        //    EditOriginatorUser = user;
        //    G.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
        //    SetControls();
        //    EditUserOriginator1.CurrentTab = 0;
        //    EditUserOriginator1.LoadData();
        //    EditUserOriginator1.LoadRoles();
        //}
        private void ShowDetails(AppUser user)
        {
            CurrentPage.StoreObject(user, Constants.USEREDITOBJECTNAME);
            RadAjaxManager1.Redirect(GetEditUserLink());
            //Response.Redirect(GetEditUserLink());
        }
        private string GetEditUserLink()
        {
            return ResolveUrl("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.EDITUSERORG);
        }

        #region grid related
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            if (cmd == DELETECOMMAND)
            {
                AppUser user = new AppUser();
                user.Id = int.Parse(e.CommandArgument.ToString());
                user.Delete();
            }
            else if (cmd == DISABLECOMMAND)
            {
                AppUser user = new AppUser();
                user.Id = int.Parse(e.CommandArgument.ToString());
                user.Disable();
            }
            else if(cmd==ENABLECOMMAND)
            {
                AppUser user = new AppUser();
                user.Id = int.Parse(e.CommandArgument.ToString());
                user.Enable();
            }
            else if (cmd == UNLOCKCOMMAND)
            {
                AppUser user = new AppUser();
                user.Id = int.Parse(e.CommandArgument.ToString());
                user.Unlock();
            }
            else if (cmd == EDITCOMMAND)
            {
                GridMode = MODEEDIT;
                AppUser user = new AppUser(int.Parse(e.CommandArgument.ToString()));
                ShowDetails(user);
            }
            else if (cmd== ADDCOMMAND)
            {
                GridMode = MODEEDIT;
                AppUser user = new AppUser(-1);
                user.CompanyId = CurrentUser.EffectiveCompanyId;
                ShowDetails(user);
            }
            BindGrid();
        }
        protected void G_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row!=null)
                {
                    int statusId = int.Parse(row[USERSTATUSIDFIELDNAME].ToString());
                    bool isLocked = row["lockTime"] != DBNull.Value;
                    ImageButton imgbtn = (ImageButton) e.Item.Cells[ACTIONCOLUMN].Controls[3];
                    if (imgbtn!=null)
                    {
                        string url ="~" + Constants.IMAGEFOLDER + "/";
                        string cmdName;
                        string alt;
                        if(isLocked)
                        {
                            e.Item.ForeColor = System.Drawing.Color.Red;
                            cmdName = UNLOCKCOMMAND;
                            alt = UNLOCKTEXT;
                            url += ENABLEIMAGE;
                        }
                        else
                        {
                            if (statusId == ENABLESTATUSID)
                            {
                                url += DISABLEIMAGE;
                                cmdName = DISABLECOMMAND;
                                alt = DISABLEALTTEXT;
                            }
                            else
                            {
                                url += ENABLEIMAGE;
                                cmdName = ENABLECOMMAND;
                                alt = ENABLEALTTEXT;
                                e.Item.ForeColor = System.Drawing.Color.DarkGray;
                            }
                        }
                        imgbtn.ImageUrl = url;
                        imgbtn.CommandName = cmdName;
                        imgbtn.AlternateText = alt;
                    }
                    ImageButton btnDelete = (ImageButton)e.Item.FindControl("imgDelete");
                    if (btnDelete != null)
                    {
                        btnDelete.Attributes.Add(ONCLICKATTRIBUTE, "javascript:{{var r=confirm('Delete this user?');if (!r)return false;}};");
                    }
                }
            }
        }
        protected void G_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
        }
        protected static string GetRole(Object o, string fieldName)
        {
            string res = "";
            DataRowView row = (DataRowView)o;
            if (row!=null)
            {
                res = row[fieldName].ToString();
            }
            if (String.IsNullOrEmpty(res))
            {
                res = "&nbsp;";
            }
            return res;
        }

        #endregion
        #region handlers
        protected void Search_Click(object sender, EventArgs e)
        {
            SetFilter();
            BindGrid();
        }
        #endregion
   }
}