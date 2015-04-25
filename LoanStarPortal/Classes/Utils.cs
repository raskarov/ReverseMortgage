using System;
using System.Text;
using System.Security.Cryptography;
using System.Net.Mail;

namespace LoanStar.Common
{
    public class Utils
    {
        #region constants
        public const string PASSWORDRULESTEXT = "Password must be at least 6 character long, start with letter and must consist of at least 1 digit";
        public const string PASSWORDNOTIDENTICALTEXT = "Passwords you have typed not identical";
        public const string PASSWORDSHORTTEXT = "Password must be at least {0} characters long.";
        public const string PASSWORDRULE = "Password must consist of at least {0} digit and {1} character.";
        public const string OLDPASSWORDINCORRECTTEXT = "Old password is incorrect";
        private const int MINPASSWORDLENGTH = 6;
        private const int MINPASSWORDLETTERS = 5;
        private const int MINPASSWORDDIGITSS = 1;
        #endregion

        private Utils()
        {
        }
        #region methods
        public static string GetMD5Hash(string input)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(input);
            bs = provider.ComputeHash(bs);
            StringBuilder s = new StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();
        }
        public static bool ValidatePassword(string password,string[] previosPasswords, out string message)
        {
            message = String.Empty;
            bool res = true;
            if (password.Length < 6)
            {
                res = false;
                message = String.Format(PASSWORDSHORTTEXT, MINPASSWORDLENGTH);
            }
            else if (!Char.IsLetter(password[0]))
            {
                res = false;
                message = "Password must start with letter.";
            }
            else
            {
                int nchar = 0;
                int ndig = 0;
                for (int i = 0; i < password.Length; i++)
                {
                    if (Char.IsLetter(password[i]))
                    {
                        nchar++;
                    }
                    else if (Char.IsDigit(password[i]))
                    {
                        ndig++;
                    }
                }
                if ((nchar < MINPASSWORDLETTERS) || (ndig < MINPASSWORDDIGITSS))
                {
                    res = false;
                    message = String.Format(PASSWORDRULE, MINPASSWORDDIGITSS, MINPASSWORDLETTERS);
                }
            }
            if(res)
            {
                if(previosPasswords!=null)
                {
                    string hash = GetMD5Hash(password);
                    for(int i=0;i<previosPasswords.Length;i++)
                    {
                        if(previosPasswords[i]==hash)
                        {
                            res = false;
                            message = "Password must be different from previos 4 passwords";
                            break;
                        }
                    }
                }
            }
            return res;
        }
        public static DateTime RemoveTime(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }
        public static bool SendEmail(MailMessage msg, string smtpHost,int smtpPort, out string errMessage)
        {
            bool res = false;
            errMessage = String.Empty;
            try
            {
                SmtpClient smtp = new SmtpClient(smtpHost, smtpPort);
                smtp.Send(msg);
                res = true;
            }
            catch(Exception ex)
            {
                errMessage = ex.Message;
            }
            return res;
        }
        #endregion

    }
}
