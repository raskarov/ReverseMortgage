using System;
using System.Xml;
using LoanStar.Common;

namespace LoanStarPortal.Administration
{
    public partial class DataUpdates : AppPage
    {
        private const string OBJECTTOUPDATE = "obj";
        private const string INDEXRATE = "rate";
        private const string LENDINGLIMIT = "limit";
        private const string CHECKINDRATE = "checkrate";
        private const string CHECKLENDINGLIMIT = "checklimit";

        private const string ROOTELEMENT = "root";
        private const string RESULTCODE = "result";
        private const string ERRMESSAGE = "errtext";


        private const int STATUSOK = 0;
        private const int STATUSUPDATEFAILED = 1;
        private const int STATUSINPROGRESS = 2;

        private int statusCode = STATUSOK;
        private readonly string errMessage = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string param = GetValue(OBJECTTOUPDATE, String.Empty);
            if(param==INDEXRATE)
            {
                try
                {
                    SetState(param,true);
                    DataLoader.RateLoader rate = new DataLoader.RateLoader();
                    rate.GetData();
                }
                catch
                {
                    statusCode = STATUSUPDATEFAILED;
                }
                SetState(param, false);
            }
            else if(param==LENDINGLIMIT)
            {
                try
                {
                    SetState(param, true);
                    DataLoader.LendingLimitLoader lim = new DataLoader.LendingLimitLoader();
                    lim.GetData();
                }
                catch
                {
                    statusCode = STATUSUPDATEFAILED;
                }
                SetState(param, false);
            }
            else if(param==CHECKINDRATE)
            {
                CheckState(INDEXRATE);
            }
            else if(param==CHECKLENDINGLIMIT)
            {
                CheckState(LENDINGLIMIT);
            }
            SendResponse();
        }
        private void SetState(string param, bool flag)
        {
            if (flag)
            {
                Session[param] = true;
            }
            else
            {
                Session.Remove(param);
            }
        }
        private void CheckState(string name)
        {
            Object o = Session[name];
            if(o==null)
            {
                statusCode = STATUSOK;
            }
            else
            {
                statusCode = STATUSINPROGRESS;
            }
        }

        private void SendResponse()
        {
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            XmlAttribute a = d.CreateAttribute(RESULTCODE);
            a.Value = statusCode.ToString();
            root.Attributes.Append(a);
            a = d.CreateAttribute(ERRMESSAGE);
            a.Value = errMessage;
            root.Attributes.Append(a);
            d.AppendChild(root);
            Response.ContentType = "text/xml";
            Response.Expires = -1;
            Response.Clear();
            Response.Write(d.InnerXml);
        }

    }
}
