using System;
using LoanStar.Common;

namespace LoanStarPortal
{
    public partial class GetHiddenValue : AppPage
    {
        private const string CLOSEANDREBIND = "<script type=\"text/javascript\">CloseAndRebind('{0}','{1}','{2}')</script>";
        private const string CANCELEDIT = "<script type=\"text/javascript\">CancelEdit()</script>";
        private const string PARAMNAME = "id";
        private const string MORTGAGEID = "mid";
        private const string OBJECTID = "oid";
        private const string PROPERTY = "property";


        private string param = String.Empty;
        private int mortgageId = -1;
        private int objId = -1;
        private string fullPropertyName = String.Empty;
        private MortgageProfile mortgage;

        protected void Page_Load(object sender, EventArgs e)
        {
            ClearErrors();
            param = GetValue(PARAMNAME, "");
            tbPassword.Focus();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (CurrentUser.Password == Utils.GetMD5Hash(tbPassword.Text))
            {
                mortgageId = GetRequestInt(MORTGAGEID);
                if (mortgageId > 0)
                {
                    objId = GetRequestInt(OBJECTID);
                    if (objId >= 0)
                    {
                        fullPropertyName = GetRequestString(PROPERTY);
                        mortgage = GetMortgage(mortgageId);
                        string err;
                        string val = mortgage.GetObjectValue(fullPropertyName, objId, out err);
                        InjectScript.Text = String.Format(CLOSEANDREBIND, param, val,err);
                    }
                    else
                    {
                        InjectScript.Text = String.Format(CLOSEANDREBIND, param, "", "Can't retrieve value");
                    }
                }
                else
                {
                    InjectScript.Text = String.Format(CLOSEANDREBIND, param, "", "Can't retrieve value");
                }
            }
            else
            {
                lblMessage.Text = "Wrong password";
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            InjectScript.Text = CANCELEDIT;
        }

        private void ClearErrors()
        {
            lblMessage.Text = "";
        }
        private int GetRequestInt(string name)
        {
            int res = -1;
            try
            {
                res = Convert.ToInt32(GetRequestString(name));
            }
            catch
            {
            }
            return res;
        }
        private string GetRequestString(string name)
        {
            string res = String.Empty;
            if (Page.Request[name] != null)
            {
                res = Page.Request[name];
            }
            return res;
        }
    }
}
