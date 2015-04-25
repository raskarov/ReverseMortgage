using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;

namespace LoanStar.Common
{
	public class ControlPermissionAttribute : Attribute
	{
		public readonly string Description;

        public ControlPermissionAttribute(string description)
        {
            Description = description;
        }
	}

	/// <summary>
	/// All user controls in the project must be derived from 
	/// BaseControl. It exposes properties and methods which give
	/// access to page tree.
	/// 
	/// Warning: Properties of AppControl and AppPage are available in OnLoad() but not on OnInit() !
	/// </summary>
	public class AppControl : UserControl
	{
        private const string SORTOBJECT = "sort";

		/// <summary>
		/// Just a handy wrapper over Page.Validate()/Page.IsValid.
		/// Use it in your controls instead of calling Page.Validate().
		/// </summary>
		public bool IsValid
		{
			get
			{
				if (!IsPostBack)
					return true;
				else
					try
					{
						return Page.IsValid;
					}
					catch
					{
						Page.Validate();
						return Page.IsValid;
					}
			}
		}
        protected static string GetShortDate(Object item, string name)
        {
            DataRowView row = (DataRowView)item;
            return Convert.ToDateTime(row[name]).ToShortDateString();
        }
        protected static string GetTrimString(Object o, string name, int len)
        {
            DataRowView row = (DataRowView)o;
            string text = row[name].ToString();
            return (text.Length <= len) ? text : text.Substring(0, len - 1) + "...";
        }
        protected string GetSortOrder(string expr)
        {
            Object o = ViewState[SORTOBJECT + expr];
            return o == null ? String.Empty : o.ToString();
        }
        protected void SetSortOrder(string expr, string value)
        {
            ViewState[SORTOBJECT + expr] = value;
        }
        public AppPage CurrentPage
        { 
            get
            {
                return (AppPage)Page;
            }            
        }
        public AppUser CurrentUser
        {
            get
            {
                return CurrentPage.CurrentUser;
            }
        }
        protected static void AppendTextBoxCondition(StringBuilder sb, string txt, string name)
        {
            if (!String.IsNullOrEmpty(txt))
            {
                if (sb.Length > 0)
                {
                    sb.Append(" and ");
                }
                sb.Append(name + " like '" + txt + "%'");
            }
        }
        protected static void AppendSelectCondition(StringBuilder sb, string txt, string name)
        {
            if (int.Parse(txt) != 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" and ");
                }
                sb.Append(name + "=" + txt);
            }
        }

        #region Download properties
        public Stream DownloadStream
        {
            get
            {
                return (Stream)Session["DownloadStream"];
            }
            set
            {
                Session["DownloadStream"] = value;
            }
        }

        public string DownloadGenerationError
        {
            get
            {
                return Convert.ToString(Session["DownloadGenerationError"]);
            }
            set
            {
                Session["DownloadGenerationError"] = value;
            }
        }

        public string DownloadFileName
        {
            get
            {
                return Convert.ToString(Session["DownloadFileName"]);
            }
            set
            {
                Session["DownloadFileName"] = value;
            }
        }

        public string DownloadContentType
        {
            get
            {
                return Convert.ToString(Session["DownloadContentType"]);
            }
            set
            {
                Session["DownloadContentType"] = value;
            }
        }
        #endregion
    }
}
