using System.Web.UI;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for DbManagerCreator.
	/// </summary>
	public class DbManagerCreator : Control
	{
		public DbManager CreateDbManager()
		{
			DbManager newManager = null;
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			switch(settings.DbType)
			{
				case SupportedDatabase.MsAccess:
					newManager = new MsAccessDbManager();
					break;
				case SupportedDatabase.MySql:
					newManager = new MySqlDbManager();
					break;
				default:
					newManager = new MsSqlDbManager();
					break;
			}
			return newManager;
		}

		public DbManager CreateDbManager(Account acct)
		{
			DbManager result = CreateDbManager();
			result.DbAccount = acct;
			return result;
		}
	}
}
