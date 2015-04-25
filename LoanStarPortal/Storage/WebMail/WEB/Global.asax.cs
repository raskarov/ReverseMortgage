using System;
using System.Configuration;
using System.ComponentModel;
using System.IO;
using System.Web;

namespace WebMailPro 
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : HttpApplication
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{

		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_Error(Object sender, EventArgs e)
		{

		}

		protected void Session_End(Object sender, EventArgs e)
		{
			if (Session[Constants.sessionTempFolder] != null)
			{
				string tempFolder = Session[Constants.sessionTempFolder].ToString();
				if (Directory.Exists(tempFolder))
				{
					try
					{
						Directory.Delete(tempFolder, true);
					}
					catch {}
				}
			}
		}

		protected void Application_End(Object sender, EventArgs e)
		{
            try
            {
                string datapath = String.Empty;
                if (ConfigurationManager.AppSettings[Constants.appSettingsDataFolderPath] != null)
                {
                    datapath = ConfigurationManager.AppSettings[Constants.appSettingsDataFolderPath];
                }

                string fullpath = Path.Combine(datapath, Constants.tempFolderName);
                if (fullpath != string.Empty && Directory.Exists(fullpath))
                {
                    Directory.Delete(fullpath, true);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine("Application_End Directory.Delete", ex.Message);
            }
		}
			
		#region Web Form Designer generated code
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

