using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class Conditions : AppControl
    {
        private const string FIRST_LOAD = "CFirstLoad";

        private MortgageProfile mp;

        #region Properties
        private int MortgageID
        {
            get
            {
                return Convert.ToInt32(Session[Constants.MortgageID]);
            }
        }
        #endregion

        public void BindData()
        {
            panelCondition.Items.Clear();
            DataView ds = Condition.GetConditionsQuickList(MortgageID);
            foreach (DataRow dr in ds.Table.Rows)
            {
                RadPanelItem pi = new RadPanelItem(Convert.ToString(dr["Title"]));
                RadPanelItem ci = new RadPanelItem();
                Literal lblText = new Literal();
                lblText.Text = String.Format("<table width='100%'><tr><td width='2px;'>&nbsp;</td><td style='white-space: normal;'>{0}</td></tr></table>", dr["Description"]);
                ci.Controls.Add(lblText);
                pi.Items.Add(ci);
                panelCondition.Items.Add(pi);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ViewState[FIRST_LOAD] == null)
            {
                ViewState[FIRST_LOAD] = 1;
                //UpdateConditions(true);
            }
            BindData();
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CurrentPage.MortgageChanged += new AppPage.MortgageEventHandler(CurrentPage_MortgageChanged);
        }
        void CurrentPage_MortgageChanged(object sender, int MortgageID)
        {
            //UpdateConditions(true);
            BindData();
        }
        protected void panelCondition_ItemClick(object sender, RadPanelbarEventArgs e)
        {
            Panel panelGeneral = (Panel)e.Item.FindControl("panelGeneral");
            panelGeneral.Visible = !panelGeneral.Visible;
        }

        private void UpdateConditions(bool ReadFromCache)
        {

            if (MortgageID > 0)
            {
                if(ReadFromCache) 
                    mp = CurrentPage.GetMortgage(MortgageID);
                mp.BuildRuleEvaluationTree(CurrentPage.GetRuleTreePublic());
                string str = mp.GetRuleListXml(RuleTree.CONDITIONBIT);
                mp.UpdateMPConditionRules(str);
                Event.UpdateMPEventRules(MortgageID, str);
            }
        }

        public void Repopulate()
        {
            //UpdateConditions(true);
            BindData();
        }

        public void RecalculateCondition(MortgageProfile profile)
        {
            mp = profile;
            //UpdateConditions(false);
            BindData();
        }

    }
}
