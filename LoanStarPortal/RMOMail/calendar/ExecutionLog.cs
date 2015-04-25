using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using WebMailPro;

/// <summary>
/// Summary description for ExecutionLog
/// </summary>
namespace Calendar_NET
{
	public class ExecutionLog
	{
		public ExecutionLog()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static void WriteString(string content, string info)
		{
			string data_path = ConfigurationManager.AppSettings[Constants.appSettingsDataFolderPath];
			string log_path = data_path + "\\exec.log";

			if (!File.Exists(log_path))
				File.Create(log_path).Close();

			StreamWriter wri = new StreamWriter(log_path, true);
			wri.Write("-------------" + info + "------------\n");
			wri.Write(content + "\n");
			wri.Write("-------------------------\n");
			wri.Close();
		}
		public static void WriteBytes(byte[] content, string info)
		{
			string data_path = ConfigurationManager.AppSettings[Constants.appSettingsDataFolderPath];
			string log_path = data_path + "\\exec.log";

			if (!File.Exists(log_path))
				File.Create(log_path).Close();

			StreamWriter wri = new StreamWriter(log_path, true);
			wri.Write("-------------" + info + "------------\n");
			wri.BaseStream.Write(content, 0, content.Length);
			wri.Write("\n-------------------------\n");
			wri.Close();
		}
		public static void WriteChars(char[] content, string info)
		{
			string data_path = ConfigurationManager.AppSettings[Constants.appSettingsDataFolderPath];
			string log_path = data_path + "\\exec.log";

			if (!File.Exists(log_path))
				File.Create(log_path).Close();

			StreamWriter wri = new StreamWriter(log_path, true);
			wri.Write("-------------" + info + "------------\n");
			wri.Write(content);
			wri.Write("\n-------------------------\n");
			wri.Close();
		}
		public static void WriteException(Exception ex)
		{
			string data_path = ConfigurationManager.AppSettings[Constants.appSettingsDataFolderPath];
			string log_path = data_path + "\\Error.log";

			if (!File.Exists(log_path))
				File.Create(log_path).Close();

			StreamWriter wri = new StreamWriter(log_path, true);
			wri.Write("---------------------------------------\n");
			wri.WriteLine(ex.Message + "\n" + ex.StackTrace);
			wri.Close();
		}
	}
}