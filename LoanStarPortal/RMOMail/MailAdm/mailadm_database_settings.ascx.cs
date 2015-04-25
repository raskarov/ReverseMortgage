using System.Globalization;

namespace WebMailPro.MailAdm
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for mailadm_database_settings.
	/// </summary>
	public partial class mailadm_database_settings : System.Web.UI.UserControl
	{
		//protected System.Web.UI.WebControls.Label messLabelID;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here

            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
            if (Session["TrySqlPassword"] == null)
            {
                Session["TrySqlPassword"] = settings.DbPassword;
            }
            txtSqlPassword.Attributes.Add("Value", Session["TrySqlPassword"].ToString());

            switch (settings.DbType)
			{
				case SupportedDatabase.MsAccess:
					intDbTypeMsAccess.Checked = true;
					break;
				case SupportedDatabase.MsSqlServer:
					intDbTypeMsSql.Checked = true;
					break;
				case SupportedDatabase.MySql:
					intDbTypeMySql.Checked = true;
					break;
			}
			txtSqlLogin.Value = settings.DbLogin;
			txtSqlName.Value = settings.DbName;
			txtSqlDsn.Value = settings.DbDsn;
			txtSqlSrc.Value = settings.DbHost;
			txtAccessFile.Value = settings.DbPathToMdb;
			odbcConnectionString.Value = settings.DbCustomConnectionString;
			useCS.Checked = settings.UseCustomConnectionString;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		protected void SubmitButton_Click(object sender, System.EventArgs e)
		{
			//Response.Redirect(@"mailadm.aspx?scr=db&save=1");
			Save();
		}

		private void Save()
		{
            Session["TrySqlPassword"] = null;
            try
			{
				WebMailSettingsCreator creator = new WebMailSettingsCreator();
				creator.ResetWebMailSettings();
				WebmailSettings settings = creator.CreateWebMailSettings();
				if (intDbTypeMsAccess.Checked)
				{
					settings.DbType = SupportedDatabase.MsAccess;
				}
				else if (intDbTypeMySql.Checked)
				{
					settings.DbType = SupportedDatabase.MySql;
				}
				else
				{
					settings.DbType = SupportedDatabase.MsSqlServer;
				}
				settings.DbLogin = txtSqlLogin.Value;
				settings.DbPassword = txtSqlPassword.Text;
				settings.DbName = txtSqlName.Value;
				settings.DbDsn = txtSqlDsn.Value;
				settings.DbHost = txtSqlSrc.Value;
				settings.DbPathToMdb = txtAccessFile.Value;
				settings.DbCustomConnectionString = odbcConnectionString.Value;
				settings.UseCustomConnectionString = useCS.Checked;

				settings.SaveWebmailSettings();
				
				SuccessOutput(Constants.mailAdmSaveSuccess);
			}
			catch (Exception ex)
			{
				UnsuccessOutput(Constants.mailAdmSaveUnsuccess, ex);
			}
		}

		protected void test_connection_Click(object sender, System.EventArgs e)
		{
            Session["TrySqlPassword"] = txtSqlPassword.Text;
            txtSqlPassword.Attributes.Add("Value", txtSqlPassword.Text);

            DbManager dbMan = null;
			string connectionString = string.Empty;
			SupportedDatabase dbType = SupportedDatabase.MsSqlServer;
			if (intDbTypeMsAccess.Checked)
			{
				dbMan = new MsAccessDbManager();
				dbType = SupportedDatabase.MsAccess;
			}
			else if (intDbTypeMySql.Checked)
			{
				dbMan = new MySqlDbManager();
				dbType = SupportedDatabase.MySql;
			}
			else
			{
				dbMan = new MsSqlDbManager();
				dbType = SupportedDatabase.MsSqlServer;
			}
			connectionString = DbManager.CreateConnectionString(useCS.Checked, odbcConnectionString.Value,
				txtSqlDsn.Value, dbType, txtAccessFile.Value, txtSqlLogin.Value,
				txtSqlPassword.Text, txtSqlName.Value, txtSqlSrc.Value);
			try
			{
				dbMan.Connect(connectionString);

				SuccessOutput(Constants.mailAdmConnectSuccess);
			}
			catch (WebMailDatabaseException ex)
			{
				UnsuccessOutput(Constants.mailAdmConnectUnsuccess, ex);
			}
			finally
			{
				dbMan.Disconnect();
			}

		}

		protected void create_tables_Click(object sender, System.EventArgs e)
		{
			Save();

            Session["TrySqlPassword"] = txtSqlPassword.Text;
            txtSqlPassword.Attributes.Add("Value", txtSqlPassword.Text);
            
            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			string prefix = settings.DbPrefix;
			string[] dbTablesName = new string[]{};

			string[] tablesNames = new string[] 
            { 
                /* WebMail Tables */
                Constants.TablesNames.a_users, 
                Constants.TablesNames.awm_accounts, 
                Constants.TablesNames.awm_addr_book, 
                Constants.TablesNames.awm_addr_groups, 
                Constants.TablesNames.awm_addr_groups_contacts, 
                Constants.TablesNames.awm_columns, 
                Constants.TablesNames.awm_filters, 
                Constants.TablesNames.awm_folders, 
                Constants.TablesNames.awm_folders_tree, 
                Constants.TablesNames.awm_messages, 
                Constants.TablesNames.awm_messages_body, 
                Constants.TablesNames.awm_reads, 
                Constants.TablesNames.awm_senders, 
                Constants.TablesNames.awm_settings, 
                Constants.TablesNames.awm_temp,  
                /* Calendar Tables */
                Constants.TablesNames.acal_calendars,
                Constants.TablesNames.acal_events,
                Constants.TablesNames.acal_users_data
            };
			bool isTableExist = false;

			DbStorage storage = DbStorageCreator.CreateDatabaseStorage(null);
			try
			{
				storage.Connect();
				dbTablesName = storage.GetTablesNames();
			}
			catch (WebMailDatabaseException ex)
			{
				UnsuccessOutput(Constants.mailAdmTablesNotCreated, ex);
				return;
			}
			finally
			{
				storage.Disconnect();
			}
			for (int i = 0; i < dbTablesName.Length; i++)
			{
				string dbTable = dbTablesName[i];
				foreach (string name in tablesNames)
				{
					if (string.Compare(dbTable, string.Format("{0}{1}", prefix, name), true, CultureInfo.InvariantCulture) == 0)
					{
						isTableExist = true;
					}
				}
			}
			if (isTableExist)
			{
				UnsuccessOutput(Constants.mailAdmTablesExists, null);
				return;
			}

			try
			{
				storage.Connect();
				foreach (string name in tablesNames)
				{
					storage.CreateTable(name, prefix);
				}

				SuccessOutput(Constants.mailAdmTablesCreated);
			}
			catch (WebMailDatabaseException ex)
			{
				UnsuccessOutput(Constants.mailAdmTablesNotCreated, ex);
				return;
			}
			finally
			{
				storage.Disconnect();
			}
		}

		private void SuccessOutput(string outStr)
		{
			messLabelID.InnerText = outStr;
			messLabelID.Style.Add("color", "green");
			messLabelID.Style.Add("font", "bold");

			errorLabelID.InnerText = string.Empty;
		}

		private void UnsuccessOutput(string outStr, Exception ex)
		{
			messLabelID.InnerText = outStr;
			messLabelID.Style.Add("color", "red");
			messLabelID.Style.Add("font", "bold");

			if (ex != null)
			{
				errorLabelID.InnerText = ex.Message;
				errorLabelID.Style.Add("color", "red");
			}
		}

	}
}
