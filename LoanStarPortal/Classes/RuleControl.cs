using System;

namespace LoanStar.Common
{
    //
    // obsolete - old version, was used with grid rule interface
    // 
    public class RuleControl : AppControl
    {
        #region constants
        protected const string ONCLICKATTRIBUTE = "OnClick";
        protected const string RULECODETEXT = "Rule code:  ";
        #endregion

        #region fields
        protected Rule rule;
        #endregion

        #region delegates
        public event BackHandler OnBack;
        public delegate void BackHandler();
        public event DataChange OnDataChange;
        public delegate void DataChange();
        #endregion

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            rule = GetRule();
        }

        #region methods
        private Rule GetRule()
        {
            Rule r = CurrentPage.GetObject(Constants.RULEOBJECT) as Rule;
            if (r == null)
            {
                r = new Rule();
                CurrentPage.StoreObject(r, Constants.RULEOBJECT);
            }
            return r;
        }
        public virtual void Initialize()
        {
            rule = GetRule();
        }
        protected virtual void goBack()
        {
            if (OnBack != null)
            {
                OnBack();
            }
        }
        protected virtual void refreshMainGrid()
        {
            if (OnDataChange != null)
            {
                OnDataChange();
            }
        }
        #endregion
    }
}
