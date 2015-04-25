using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for updateDB.
	/// </summary>
	public partial class updateDB : System.Web.UI.Page
	{
		protected StringBuilder sb = new StringBuilder();
		protected int _errorCounter = 0;
		protected int _allOps = 0;
        protected SupportedDatabase _dbType;

	
		protected void Page_Load(object sender, System.EventArgs e)
        {
            #region Convert Settings in settings.xml

            WebmailSettings wmsNew = new WebmailSettings().CreateInstance();

            sb.AppendFormat("<font color='black' sise='3' face='verdana'><b>{1}</b>. Start convert settings.xml</font><BR />", "settings.xml", _allOps + 1);
            _allOps++;

            try
            {
                wmsNew.SaveWebmailSettings();
                sb.AppendFormat("<font color='grey' sise='3' face='verdana'>- Convert settings successful.</font><BR /><BR />");
            }
            catch (Exception ex)
            {
                sb.AppendFormat("<font color='grey' sise='3' face='verdana'>- {0}</font><BR /><BR />", ex.Message);
                _errorCounter++;
            }
            # endregion

            Account acct = new Account();
            acct = (Account)Session[Constants.sessionAccount];

            WebmailSettings settings = (new WebmailSettings()).CreateInstance();
            DbStorage storage = DbStorageCreator.CreateDatabaseStorage(acct);
            _dbType = settings.DbType;
            string[] tablesToCreate;
            string[] tablesToAlter = new string[]{};

            try
			{
                if (_dbType == SupportedDatabase.MsAccess)
                {
                    tablesToCreate = new string[] 
                    { 
                        Constants.TablesNames.acal_calendars,
                        Constants.TablesNames.acal_events,
                        Constants.TablesNames.acal_users_data
                    };
                }
                else
                {
                    tablesToCreate = new string[] 
                    { 
                        Constants.TablesNames.awm_columns, 
                        Constants.TablesNames.awm_senders,
                        /* Calendar Tables */
                        Constants.TablesNames.acal_calendars,
                        Constants.TablesNames.acal_events,
                        Constants.TablesNames.acal_users_data
                    };
                    tablesToAlter = new string[] 
                    { 
                        Constants.TablesNames.awm_addr_book, 
                        Constants.TablesNames.awm_addr_groups 
                    };
                }

                Hashtable indices = new Hashtable();
				indices.Add(Constants.TablesNames.awm_reads, new string[] {settings.DbPrefix+"awm_reads","_id_acct_index","id_acct"});
				indices.Add(Constants.TablesNames.awm_columns+"_0", new string[] {settings.DbPrefix+"awm_columns","_id_user_index","id_user"});
				indices.Add(Constants.TablesNames.awm_columns+"_1", new string[] {settings.DbPrefix+"awm_columns","_id_column_index","id_column"});
				indices.Add(Constants.TablesNames.awm_messages+"_0", new string[] {settings.DbPrefix+"awm_messages","_id_folder_srv_index","id_folder_srv"});
				indices.Add(Constants.TablesNames.awm_messages+"_1", new string[] {settings.DbPrefix+"awm_messages","_id_folder_db_index","id_folder_db"});
				indices.Add(Constants.TablesNames.awm_settings, new string[] {settings.DbPrefix+"awm_settings","_id_user_index","id_user"});
				indices.Add(Constants.TablesNames.awm_senders, new string[] {settings.DbPrefix+"awm_senders","_id_user_index","id_user"});
				indices.Add(Constants.TablesNames.awm_accounts, new string[] {settings.DbPrefix+"awm_accounts","_id_user_index","id_user"});
				indices.Add(Constants.TablesNames.awm_addr_groups, new string[] {settings.DbPrefix+"awm_addr_groups","_id_user_index","id_user"});
				indices.Add(Constants.TablesNames.awm_addr_book, new string[] {settings.DbPrefix+"awm_addr_book","_id_user_index","id_user"});
				indices.Add(Constants.TablesNames.awm_folders+"_0", new string[] {settings.DbPrefix+"awm_folders","_id_acct_index","id_acct"});
				indices.Add(Constants.TablesNames.awm_folders+"_1", new string[] {settings.DbPrefix+"awm_folders","_id_parent_index","id_parent"});
				indices.Add(Constants.TablesNames.awm_folders_tree+"_0", new string[] {settings.DbPrefix+"awm_folders_tree","_id_folder_index","id_folder"});
				indices.Add(Constants.TablesNames.awm_folders_tree+"_1", new string[] {settings.DbPrefix+"awm_folders_tree","_id_parent_index","id_parent"});
				indices.Add(Constants.TablesNames.awm_filters+"_0", new string[] {settings.DbPrefix+"awm_filters","_id_acct_index","id_acct"});
				indices.Add(Constants.TablesNames.awm_filters+"_1", new string[] {settings.DbPrefix+"awm_filters","_id_folder_index","id_folder"});
				storage.Connect();

                if (tablesToCreate.Length > 0)
                {
                    foreach (string tableName in tablesToCreate)
                    {
                        sb.AppendFormat("<font color='black' sise='3' face='verdana'><b>{1}</b>. Start create <b>{0}</b> table:</font><BR />", tableName, _allOps + 1);
                        _allOps++;
                        try
                        {
                            storage.CreateTable(tableName, settings.DbPrefix);
                            sb.AppendFormat("<font color='grey' sise='3' face='verdana'>- {0} create successful</font><BR /><BR />", tableName);
                        }
                        catch (Exception ex)
                        {
                            sb.AppendFormat("<font color='grey' sise='3' face='verdana'>- {0}</font><BR /><BR />", ex.Message);
                            _errorCounter++;
                        }
                    }
                }

                sb.AppendFormat(@"<font color='black' sise='3' face='verdana'><b>{0}</b>. Start update tables:</font><BR />", _allOps + 1);
                _allOps++;

                if (tablesToAlter.Length > 0)
                {
                    foreach (string tableName in tablesToAlter)
                    {
                        try
                        {
                            sb.AppendFormat("<font color='black' sise='3' face='verdana'>- Update {0}:</font><BR />", tableName);
                            storage.AlterTable(tableName, settings.DbPrefix);
                        }
                        catch (Exception ex)
                        {
                            sb.AppendFormat("<font color='grey' sise='3' face='verdana'>- {0}</font><BR /><BR />", ex.Message);
                            _errorCounter++;
                        }
                    }
                }
                else
                {
                    sb.AppendFormat("<font color='grey' sise='3' face='verdana'>- There are no tables for updating</font><BR /><BR />");
                }

                sb.AppendFormat(@"<font color='black' sise='3' face='verdana'><b>{0}</b>. Start create new index:</font><BR /><BR />", _allOps + 1);
                _allOps++;
                foreach (DictionaryEntry de in indices)
				{
					string[] aPSC = (string[])de.Value;
					try {
                        sb.AppendFormat("<font color='black' sise='3' face='verdana'>- Create <b>{0}</b> with params <b>{1}</b>, <b>{2}</b>, <b>{3}</b>.</font><br />", de.Key, aPSC[0], aPSC[1], aPSC[2]);
						storage.CreateIndex(aPSC[0],aPSC[1],aPSC[2]);
					}
					catch(Exception ex) {
                        sb.AppendFormat("<font color='grey' sise='3' face='verdana'>- {0}</font><BR /><BR />", ex.Message);
						_errorCounter++;
					}
				}
/*
                if ((_errorCounter!=0)&&(_errorCounter!=_allOps)) {
					outputLabel.Text = String.Format("Structure update complete, but some ({0}/<SPAN style='color: #666666'>{1}</SPAN>) operations was failed.",_errorCounter,_allOps);
					outputLabel.ForeColor = Color.Red;
				}
				if (_errorCounter==0) {
					outputLabel.Text = "Database structure update successfully complete.";
					outputLabel.ForeColor = Color.Green;
				}
				if (_errorCounter==_allOps) {
                    outputLabel.Text = String.Format("Update is done!", _errorCounter);
					outputLabel.ForeColor = Color.Black;
				}
*/
                outputLabel.Text = String.Format("Update is done!", _errorCounter);
                outputLabel.ForeColor = Color.Black;
                outputLabel.Font.Bold = true;
            }
            
            
            catch (Exception ex)
			{
				OutputUnsuccess(ex);
			}
			finally
			{
				storage.Disconnect();
			}
		}

		private void OutputUnsuccess(Exception ex)
		{
			outputLabel.Text = "Update unsuccessful! Error: " + ex.Message;
			outputLabel.ForeColor = Color.Red;
			outputLabel.Font.Bold = true;
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
