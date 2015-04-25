using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Web.UI;
using System.Xml.XPath;
using WebMailPro;

namespace Calendar_NET
{
	public class CalendarResourceManager
	{
		protected CultureInfo _cultureInfo;
		protected ResourceManager _resManager;

		public CultureInfo Culture
		{
			get { return _cultureInfo; }
			set { _cultureInfo = value; }
		}

		public ResourceManager Manager
		{
			get { return _resManager; }
			set { _resManager = value; }
		}

		public CalendarResourceManager(string resourceDir)
		{
			_cultureInfo = CultureInfo.InvariantCulture;
			_resManager = ResourceManager.CreateFileBasedResourceManager("calendar", resourceDir, null);
		}

		public string GetString(string name)
		{
			try
			{
				string result = _resManager.GetString(name, _cultureInfo);
				if (result == null)
				{
					result = name;
				}
				return result;
			}
			catch (Exception)
			{
				return name;
			}
		}
	}

	/// <summary>
	/// Summary description for CalendarResourceManager.
	/// </summary>
	public class CalendarResourceManagerCreator : Control
	{
		public CalendarResourceManager CreateResourceManager()
		{
			/*CalendarSettings settings = (new CalendarSettingsCreator()).CreateCalendarSettings();
			string lang = settings.DefaultLanguage;
			if (this.Context.Session[Constants.sessionAccount] != null)
			{
				Account acct = this.Context.Session[Constants.sessionAccount] as Account;
				if (acct != null)
				{
					if ((acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
					{
						lang = acct.UserOfAccount.Settings.DefaultLanguage;
					}
				}
			}

			CalendarResourceManager newManager = null;
			string culture = "";
			if (this.Context.Session[Constants.sessionResourceManager] != null)
			{
				newManager = this.Context.Session[Constants.sessionResourceManager] as CalendarResourceManager;
				if ((newManager != null) && (string.Compare(newManager.Culture.EnglishName, lang, true, CultureInfo.InvariantCulture) == 0))
				{
					return newManager;
				}
			}
			string langsXml = Path.Combine(Utils.GetDataFolderPath(), @"langs\langs.xml");
			if (File.Exists(langsXml))
			{
				XPathDocument xpathDoc = new XPathDocument(langsXml);
				XPathNavigator nav = xpathDoc.CreateNavigator();
				XPathNodeIterator langIter = nav.Select(string.Format("langs/lang[FriendlyName='{0}']/CultureName", lang));
				if (langIter.MoveNext())
				{
					culture = langIter.Current.Value;
				}
				else
				{
					culture = "";
				}
			}
			if (newManager != null)
			{
				newManager.Manager.ReleaseAllResources();
			}
			newManager = new CalendarResourceManager(Path.Combine(Utils.GetDataFolderPath(), @"langs"));
			newManager.Culture = new CultureInfo(culture);
			this.Context.Session.Add(Constants.sessionResourceManager, newManager);*/

			CalendarResourceManager newManager = null;
			newManager = new CalendarResourceManager(Path.Combine(ConfigurationManager.AppSettings[Constants.appSettingsDataFolderPath], @"langs"));
			newManager.Culture = new CultureInfo("ru");
			return newManager;
		}

	}
}