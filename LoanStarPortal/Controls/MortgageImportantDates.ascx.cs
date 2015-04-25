using System;
using System.Web.UI;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class MortgageImportantDates : MortgageDataControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public override void BuildControl(Control _container)
        {
            container = _container;
            _container.Controls.Add(GetContactsControl());
        }
        private Control GetContactsControl()
        {
            ControlWrapper wrapper = (ControlWrapper)LoadControl(Constants.FECONTROLSLOCATION + "ControlWrapper.ascx");
            wrapper.ID = "Mortgagedates_" + Mp.ID;
            BindData();
            wrapper.Controls.Add(phImportantDates);
            return wrapper;
        }
        private void BindData()
        {
            rpDates.DataSource = Mp.GetImportantDates();
            rpDates.DataBind();
        }
    }
}