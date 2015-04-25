using System;
using System.IO;

namespace LoanStar.Common
{
    public class Logger
    {
        private const string logFile = "logfile_{0}.txt";
        public static string StorageFolder = String.Empty;

        private readonly string folderName = String.Empty;
        private readonly string fileName = String.Empty;
        private bool addTimeStamp = true;

        public bool AddTimeStamp
        {
            get { return addTimeStamp; }
            set { addTimeStamp = value; }
        }

        public Logger() : this(StorageFolder)
        {
        }

        public Logger(string _folderName)
        {
            folderName = _folderName;
            fileName = Path.Combine(folderName, GetFileName());
        }
        public Logger(string _folderName, string _fileName, bool createNew)
        {
            folderName = _folderName;
            fileName = Path.Combine(folderName, _fileName);
            if(createNew)
            {
                FileInfo fi = new FileInfo(fileName);
                if(fi.Exists)
                {
                    fi.Delete();
                }
            }
        }
        public void WriteLine(string data)
        {
            Write(data + Environment.NewLine);
        }
        public virtual void WriteException(Exception ex)
        {
            WriteLine(String.Empty);
            WriteLine(String.Format("Error : {0}", ex.Message));
            WriteLine(String.Format("Stack trace : {0}", ex.StackTrace));
        }
        public void Write(string data)
        {
            FileStream fsLog = null;
            StreamWriter swLog = null;
            try 
            {
                fsLog = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                swLog = new StreamWriter(fsLog);
                if (addTimeStamp)
                {
                    swLog.Write("<< " + DateTime.Now + " >> ");
                }
                swLog.Write(data);
            }
            catch (IOException)
            {
            }
            finally
            {
                if (swLog != null) swLog.Close();
                if (fsLog != null) fsLog.Close();
            }

        }

        private static string GetFileName()
        {
            return String.Format(logFile, DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day);
        }
    }
}
