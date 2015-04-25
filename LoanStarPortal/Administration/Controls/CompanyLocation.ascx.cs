using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using LoanStar.Common;
using Telerik.WebControls;
using Telerik.WebControls.RadTreeViewContextMenu;

namespace LoanStarPortal.Administration.Controls
{
    public partial class CompanyLocation : AppControl
    {
        #region constants
        private const string ROOTMENUNAME = "Rootmenu";
        private const string NODEMENUNAME = "Nodemenu";
        private const int MENUITEMWIDTH = 90;
        private const string ADDCHILDMENUITEMTEXT = "Add branch";
        private const string EDITMENUITEMTEXT = "Edit";
        private const string DELETEMENUITEMTEXT = "Delete";
        private const string LOCATIONIMAGE = "folder.gif";
        private const string STATEDICTIONARYTABLE = "vwState";
        private const string IDATTRIBUTE = "id";
        private const string PARENTIDATTRIBUTE = "parentid";
        #endregion

        #region fields
        private readonly ArrayList contextMenus = new ArrayList();
        private int rootlocationId = -1;
        private DataView dvState = null;
        private RadTreeNode selectedNode;
        private RadTreeNode rootNode;
        #endregion

        #region properties
        private LoanStar.Common.CompanyLocation SelectedLocation
        {
            get
            {
                LoanStar.Common.CompanyLocation res = Session["selectedlocation"] as LoanStar.Common.CompanyLocation;
                if (res == null)
                {
                    res = new LoanStar.Common.CompanyLocation(rootlocationId);
                    Session["selectedlocation"] = res;
                }
                return res;
            }
            set { Session["selectedlocation"] = value; }
        }
        private int SelectedLocationId
        {
            get
            {
                int res = rootlocationId;
                Object o = Session["selectedlocationid"];
                if(o!=null)
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
            set { Session["selectedlocationid"] = value; }
        }
        private DataView DvState
        {
            get
            {
                if (dvState == null)
                    dvState = CurrentPage.GetDictionary(STATEDICTIONARYTABLE);
                return dvState;
            }
        }
        #endregion

        #region methods

        #region context menu
        private void CreateMenuArray()
        {
            ContextMenu rootMenu = new ContextMenu();
            rootMenu.Name = ROOTMENUNAME;
            rootMenu.Width = MENUITEMWIDTH;
            rootMenu.Items.Add(GetMenuItem(ADDCHILDMENUITEMTEXT));
            contextMenus.Add(rootMenu);
            ContextMenu nodeMenu = new ContextMenu();
            nodeMenu.Name = NODEMENUNAME;
            nodeMenu.Width = MENUITEMWIDTH;
            nodeMenu.Items.Add(GetMenuItem(ADDCHILDMENUITEMTEXT));
            nodeMenu.Items.Add(GetMenuItem(EDITMENUITEMTEXT));
            nodeMenu.Items.Add(GetMenuItem(DELETEMENUITEMTEXT));
            contextMenus.Add(nodeMenu);
        }
        private static ContextMenuItem GetMenuItem(string text)
        {
            ContextMenuItem item = new ContextMenuItem();
            item.Text = text;
            item.PostBack = true;
            return item;
        }
        private void EditNode(WebControl node)
        {
            int id = GetTreeNodeAttributeInt(node, IDATTRIBUTE);
            SelectedLocation = new LoanStar.Common.CompanyLocation(id);
            LoadSelectedLocation();
        }
        private void DeleteNode(WebControl node)
        {
            int parentId = GetTreeNodeAttributeInt(node, PARENTIDATTRIBUTE);
            int id = GetTreeNodeAttributeInt(node, IDATTRIBUTE);
            LoanStar.Common.CompanyLocation.Delete(id);
            SelectedLocation = new LoanStar.Common.CompanyLocation(parentId);
            SelectedLocationId = SelectedLocation.ID;
            BindTree();
            LoadSelectedLocation();
        }
        private void AddChildNode(WebControl node)
        {
            int parentId = GetTreeNodeAttributeInt(node, IDATTRIBUTE);
            int companyId = SelectedLocation.CompanyId;
            SelectedLocation = new LoanStar.Common.CompanyLocation(-1);
            SelectedLocation.CompanyId = companyId;
            SelectedLocation.ParentLocationId = parentId;
            LoadSelectedLocation();
        }
        #endregion

        #region tree methods
        private static int GetTreeNodeAttributeInt(WebControl node, string name)
        {
            int res = -1;
            try
            {
                res = int.Parse(node.Attributes[name]);
            }
            catch
            {
            }
            return res;
        }

        private void BindData()
        {
            BindTree();
            LoadSelectedLocation();
        }
        private void BindTree()
        {
            rtvLocation.ContextMenus = contextMenus;
            rtvLocation.DataSource = LoanStar.Common.CompanyLocation.GetLocationTree(CurrentUser.EffectiveCompanyId);
            rtvLocation.DataFieldID = "id";
            rtvLocation.DataFieldParentID = "parentlocationid";
            rtvLocation.DataTextField = "name";
            rtvLocation.DataValueField = "id";
            rtvLocation.DataBind();
            if (selectedNode != null)
            {
                ExpandBranch(selectedNode);
                selectedNode.Focus();
            }
            else
            {
                ExpandBranch(rootNode);
                rootNode.Focus();
            }
        }
        private static void ExpandBranch(RadTreeNode node)
        {
            RadTreeNode currentNode = node;
            while (currentNode != null)
            {
                currentNode.Expanded = true;
                currentNode = currentNode.Parent;
            }
        }
        #endregion

        #region details methods
        private static void BindDdl(ListControl ddl, DataView dv, int selectedValue)
        {
            ddl.DataSource = dv;
            ddl.DataTextField = "Name";
            ddl.DataValueField = "id";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("- Select -", "0"));
            if (selectedValue > 0)
            {
                ddl.SelectedValue = selectedValue.ToString();
            }
        }
        private void LoadSelectedLocation()
        {
            tbName.Text = SelectedLocation.Name;
            tbAddress1.Text = SelectedLocation.Address1;
            tbAddress2.Text = SelectedLocation.Address2;
            tbCity.Text = SelectedLocation.City;
            if(!String.IsNullOrEmpty(SelectedLocation.Zip))
            {
                tbZip.Text = SelectedLocation.Zip;
            }
            
            BindDdl(ddlState, DvState, SelectedLocation.StateId);
            tbCustomField1.Text = SelectedLocation.CustomField1;
            tbCustomField2.Text = SelectedLocation.CustomField2;
            tbCustomField3.Text = SelectedLocation.CustomField3;
            tbCustomField4.Text = SelectedLocation.CustomField4;
            tbCustomField5.Text = SelectedLocation.CustomField5;
            tbCustomField6.Text = SelectedLocation.CustomField6;
            tbCustomField7.Text = SelectedLocation.CustomField7;
            tbCustomField8.Text = SelectedLocation.CustomField8;
            tbCustomField9.Text = SelectedLocation.CustomField9;
            tbCustomField10.Text = SelectedLocation.CustomField10;
        }
        #endregion

        #endregion

        #region event handlers
        protected void rtvLocation_NodeBound(object o, RadTreeNodeEventArgs e)
        {
            RadTreeNode node = e.NodeBound;
            DataRowView row = (DataRowView)e.NodeBound.DataItem;
            bool isRoot = row["parentlocationid"] == DBNull.Value;
            int id = int.Parse(row["id"].ToString());
            int parentId = -1;
            if(id==SelectedLocationId)
            {
                selectedNode = node;
            }
            node.Attributes.Add(IDATTRIBUTE, id.ToString());
            if(isRoot)
            {
                node.ContextMenuName = ROOTMENUNAME;
                node.DragEnabled = false;
                rootlocationId = int.Parse(row["id"].ToString());
                rootNode = node;
            }
            else
            {
                node.ContextMenuName = NODEMENUNAME;
                node.ImageUrl = Constants.IMAGEFOLDER + "/" + LOCATIONIMAGE;
                parentId = int.Parse(row["parentlocationid"].ToString());
            }
            node.Attributes.Add(PARENTIDATTRIBUTE, parentId.ToString());
            node.PostBack = true;
        }
        protected void rtvLocation_NodeContextClick(object o, RadTreeNodeEventArgs e)
        {
            string cmd = e.ContextMenuItemText;
            if (cmd == ADDCHILDMENUITEMTEXT)
            {
                AddChildNode(e.NodeClicked);
            }
            else if (cmd == EDITMENUITEMTEXT)
            {
                EditNode(e.NodeClicked);
            }
            else if (cmd == DELETEMENUITEMTEXT)
            {
                DeleteNode(e.NodeClicked);
            }
        }
        protected void rtvLocation_NodeClick(object o, RadTreeNodeEventArgs e)
        {
            EditNode(e.NodeClicked);
        }
        protected void rtvLocation_NodeDrop(object o, RadTreeNodeEventArgs e)
        {
            //RadTreeNode sourceNode = e.SourceDragNode;
            //RadTreeNode destNode = e.DestDragNode;
            //int srcNodeId = GetTreeNodeAttributeInt(sourceNode, RuleTreeAdmin.IDATTRIBUTE);
            //int destNodeId = GetTreeNodeAttributeInt(destNode, RuleTreeAdmin.IDATTRIBUTE);
            //int nodeTypeId = GetTreeNodeAttributeInt(sourceNode, RuleTreeAdmin.NODETYPEATTRIBUTE);
            //if (nodeTypeId == RuleTree.nodeRule)
            //{
            //    if (RuleEditNode.SetNewParent(srcNodeId, destNodeId))
            //    {
            //        AddControlToUpdate(rtvRule.ID, 0);
            //        EditNode(sourceNode, TABPROPERTYID);
            //        BindTree(true);
            //    }
            //}
            //else
            //{
            //    if (RuleEditNode.ChangeObjectParent(srcNodeId, destNodeId))
            //    {
            //        AddControlToUpdate(rtvRule.ID, 0);
            //        CurrentRuleNodeId = destNodeId;
            //        BindTree(true);
            //    }
            //}
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SelectedLocation.Name = tbName.Text;
            SelectedLocation.Address1 = tbAddress1.Text;
            SelectedLocation.Address2 = tbAddress2.Text;
            SelectedLocation.City = tbCity.Text;
            SelectedLocation.Zip = tbZip.Text;
            SelectedLocation.StateId = int.Parse(ddlState.SelectedValue);
            SelectedLocation.CustomField1 = tbCustomField1.Text;
            SelectedLocation.CustomField2 = tbCustomField2.Text;
            SelectedLocation.CustomField3 = tbCustomField3.Text;
            SelectedLocation.CustomField4 = tbCustomField4.Text;
            SelectedLocation.CustomField5 = tbCustomField5.Text;
            SelectedLocation.CustomField6 = tbCustomField6.Text;
            SelectedLocation.CustomField7 = tbCustomField7.Text;
            SelectedLocation.CustomField8 = tbCustomField8.Text;
            SelectedLocation.CustomField9 = tbCustomField9.Text;
            SelectedLocation.CustomField10 = tbCustomField10.Text;
            SelectedLocation.Save();
            SelectedLocationId = SelectedLocation.ID;
            BindTree();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            CreateMenuArray();
            if (!IsPostBack)
            {
                BindData();
            }
        }
        #endregion

    }
}