using System;
using System.Diagnostics;

namespace WebMailPro
{
	/// <summary>
	/// Summary description for Log.
	/// </summary>
	public class Log
	{
		private Log() {}

		public static void Write(string message)
		{
			Trace.Write(message);
		}

		public static void WriteLine(string methodName, string message)
		{
			string output = string.Format("[{2}][{0}] - {1}", methodName, message, DateTime.Now);
			Trace.WriteLine(output);
		}

		public static void WriteException(Exception ex)
		{
			Trace.WriteLine("------------------------------------------------------------");
			Trace.WriteLine(string.Format("[{0}] ERROR!!!", DateTime.Now));
			Trace.WriteLine(string.Format("[Message]\r\n{0}", ex.Message));
			if (ex.InnerException != null)
			{
				Trace.WriteLine(string.Format("[Stack Trace]\r\n{0}", ex.InnerException.StackTrace));
			}
			else
			{
				Trace.WriteLine(string.Format("[Stack Trace]\r\n{0}", ex.StackTrace));
			}
			Trace.WriteLine("------------------------------------------------------------");
		}
	}
}
