using System;

namespace LoanStarPortal.Controls
{
    public partial class LeadCalculator : System.Web.UI.UserControl
    {

        #region constants
        private const string FIRSTLOAD = "FirstLoad";

        #endregion

        #region fields
        LoanStar.Common.LeadCalculator calc;
        #endregion

        #region properties
        protected bool FirstLoad
        {
            get
            {
                bool res = true;
                Object o = ViewState[FIRSTLOAD];
                if (o != null)
                {
                    try
                    {
                        res = Convert.ToBoolean(o.ToString());
                    }
                    catch { }
                }
                return res;
            }
            set
            {
                ViewState[FIRSTLOAD] = value;
            }
        }
        #endregion


        #region methods
        private void BindData()
        {
            tbGrossIncome.Text = calc.DesiredGrossIncom.ToString();
            tbAverageMaxClaim.Text = calc.AverageMaxClaim.ToString();
            tbStandardCommission.Text = calc.StandardCommission.ToString();
            tbEstimatedFallout.Text = calc.EstimatedFallOut.ToString();
            tbProductionNeeded.Text = calc.ProductionNeeded.ToString();

            tbSourcedClosing.Text = calc.SelfSourcedClosing.ClosingPercent.ToString();
            tbNetCommission1.Text = calc.SelfSourcedClosing.NetCommission.ToString();
            tbPerMonthVolume1.Text = Math.Round(calc.SelfSourcedClosing.PerMonthUnitValue,1).ToString();
            tbIncomeMonth1.Text = Math.Round(calc.SelfSourcedClosing.IncomeMonth).ToString("C");
            tbIncomeYear1.Text = Math.Round(calc.SelfSourcedClosing.IncomeYear).ToString("C");
            tbLeadsToApps1.Text = calc.SelfSourcedClosing.PercentLeadsToApps.ToString();
            tbLeadsPerMonth1.Text = Math.Round(calc.SelfSourcedClosing.ReqLeadsperMonth).ToString();
            tbLeadsPerWeek1.Text = Math.Round(calc.SelfSourcedClosing.ReqLeadsperWeek).ToString();
            tbLeadsPerDay1.Text = Math.Round(calc.SelfSourcedClosing.ReqLeadsperWeekDay).ToString();

            tbRmcClosing.Text = calc.RMCreferredClosing.ClosingPercent.ToString();
            tbNetCommission2.Text = calc.RMCreferredClosing.NetCommission.ToString();
            tbPerMonthVolume2.Text = Math.Round(calc.RMCreferredClosing.PerMonthUnitValue,1).ToString();
            tbIncomeMonth2.Text = Math.Round(calc.RMCreferredClosing.IncomeMonth).ToString("C");
            tbIncomeYear2.Text = Math.Round(calc.RMCreferredClosing.IncomeYear).ToString("C");
            tbLeadsToApps2.Text = calc.RMCreferredClosing.PercentLeadsToApps.ToString();
            tbLeadsPerMonth2.Text = Math.Round(calc.RMCreferredClosing.ReqLeadsperMonth).ToString();
            tbLeadsPerWeek2.Text = Math.Round(calc.RMCreferredClosing.ReqLeadsperWeek).ToString();

            tbBrokerClosing1.Text = calc.BrokerInClosingl.ClosingPercent.ToString();
            tbNetCommission3.Text = calc.BrokerInClosingl.NetCommission.ToString();
            tbPerMonthVolume3.Text = Math.Round(calc.BrokerInClosingl.PerMonthUnitValue,1).ToString();
            tbIncomeMonth3.Text = Math.Round(calc.BrokerInClosingl.IncomeMonth).ToString("C");
            tbIncomeYear3.Text = Math.Round(calc.BrokerInClosingl.IncomeYear).ToString("C");
            tbLeadsToApps3.Text = calc.BrokerInClosingl.PercentLeadsToApps.ToString();
            tbLeadsPerMonth3.Text = Math.Round(calc.BrokerInClosingl.ReqLeadsperMonth).ToString();
            tbLeadsPerWeek3.Text = Math.Round(calc.BrokerInClosingl.ReqLeadsperWeek).ToString();
            tbLeadsPerDay3.Text = Math.Round(calc.BrokerInClosingl.ReqLeadsperWeekDay).ToString();

            tbBrokerClosing2.Text = calc.BrokerInClosing2.ClosingPercent.ToString();
            tbNetCommission4.Text = calc.BrokerInClosing2.NetCommission.ToString();
            tbPerMonthVolume4.Text = Math.Round(calc.BrokerInClosing2.PerMonthUnitValue,1).ToString();
            tbIncomeMonth4.Text = Math.Round(calc.BrokerInClosing2.IncomeMonth).ToString("C");
            tbIncomeYear4.Text = Math.Round(calc.BrokerInClosing2.IncomeYear).ToString("C");
            tbLeadsToApps4.Text = calc.BrokerInClosing2.PercentLeadsToApps.ToString();
            tbLeadsPerMonth4.Text = Math.Round(calc.BrokerInClosing2.ReqLeadsperMonth).ToString();
            tbLeadsPerWeek4.Text = Math.Round(calc.BrokerInClosing2.ReqLeadsperWeek).ToString();

            tbGoalYearEnd.Text = Math.Round(calc.GoalForYearEnd,1).ToString();
            tbMonthAverage.Text = Math.Round(calc.AveragePerMonth,1).ToString();
            tbWeekAverage.Text = Math.Round(calc.AveragePerWeek, 1).ToString();

        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            calc = new LoanStar.Common.LeadCalculator();
            if (FirstLoad)
            {
                calc = new LoanStar.Common.LeadCalculator();
                calc.Calculate();
                BindData();
            }            
        }
        protected void DataChanged_TextChanged(object sender, EventArgs e)
        {
            calc = new LoanStar.Common.LeadCalculator(Convert.ToDecimal(tbGrossIncome.Text), float.Parse(tbStandardCommission.Text), Convert.ToDecimal(tbAverageMaxClaim.Text), float.Parse(tbEstimatedFallout.Text));
            calc.SelfSourcedClosing.ClosingPercent = float.Parse(tbSourcedClosing.Text);
            calc.SelfSourcedClosing.NetCommission = float.Parse(tbNetCommission1.Text);
            calc.SelfSourcedClosing.PercentLeadsToApps = float.Parse(tbLeadsToApps1.Text);
            calc.RMCreferredClosing.ClosingPercent = float.Parse(tbRmcClosing.Text);
            calc.RMCreferredClosing.NetCommission = float.Parse(tbNetCommission2.Text);
            calc.RMCreferredClosing.PercentLeadsToApps = float.Parse(tbLeadsToApps2.Text);
            calc.BrokerInClosingl.ClosingPercent = float.Parse(tbBrokerClosing1.Text);
            calc.BrokerInClosingl.NetCommission = float.Parse(tbNetCommission3.Text);
            calc.BrokerInClosingl.PercentLeadsToApps = float.Parse(tbLeadsToApps3.Text);
            calc.BrokerInClosing2.ClosingPercent = float.Parse(tbBrokerClosing2.Text);
            calc.BrokerInClosing2.NetCommission = float.Parse(tbNetCommission4.Text);
            calc.BrokerInClosing2.PercentLeadsToApps = float.Parse(tbLeadsToApps4.Text);

            calc.Calculate();
            BindData();
        }

    }
}