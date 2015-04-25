using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using BossDev.CommonUtils;

namespace LoanStar.Common
{
    /// <summary>
    /// Class to work with POP3
    /// Example:
    /// /*Create the client*/
    /// Pop3Client freePopClient = new Pop3Client("popserver.website.com", "user", "pass");
    /// /*Pop off all of the messages, this will also connect and disconnect the TCP session*/
    /// Pop3Client.Pop3MessageCollection messages = pop3.PopMessages();
    /// foreach(Pop3Client.Pop3Message message in messages)
    /// {
    /// /* Do something cool here*/
    /// }
    /// </summary>
    public class Pop3Client
    {
        // Fields
        private const string CRLF = "\r\n";
        private string password;
        private int port;
        private string server;
        private string userName;

        // Methods
        public Pop3Client(string server, string userName, string password)
            : this(server, 110, userName, password)
        {
        }

        public Pop3Client(string server, int port, string userName, string password)
        {
            this.server = server;
            this.port = port;
            this.userName = userName;
            this.password = password;
        }

        private bool CheckResponse(string response)
        {
            return (response.Substring(0, 3) == "+OK");
        }

        public Pop3TcpClient ConnectServer()
        {
            Pop3TcpClient client = new Pop3TcpClient(this.server, this.port);
            try
            {
                client.SendCommand(string.Format("USER {0}", this.userName));
                string response = client.GetResponse(false);
                if (this.CheckResponse(response))
                {
                    client.SendCommand(string.Format("PASS {0}", this.password));
                    response = client.GetResponse(false);
                    if (this.CheckResponse(response))
                    {
                        return client;
                    }
                }
            }
            catch
            {
                client.Dispose();
                throw;
            }
            client.Dispose();
            throw new Exception(string.Format("Could not connect to server: {0}", this.server));
        }

        private Pop3Message GetMessage(bool delete)
        {
            Pop3TcpClient tcpClient = this.ConnectServer();
            try
            {
                int messageCount = this.GetMessageCount(tcpClient);
                if (messageCount > 0)
                {
                    tcpClient.SendCommand(string.Format("RETR {0}", messageCount));
                    string response = tcpClient.GetResponse(true);
                    if (this.CheckResponse(response))
                    {
                        Pop3Message message = this.ParseMessage(response);
                        if (delete)
                        {
                            tcpClient.SendCommand(string.Format("DELE {0}", messageCount));
                            response = tcpClient.GetResponse(false);
                            if (this.CheckResponse(response))
                            {
                                return message;
                            }
                        }
                        else
                        {
                            return message;
                        }
                    }
                }
            }
            finally
            {
                tcpClient.Disconnect();
            }
            return null;
        }

        private int GetMessageCount(Pop3TcpClient tcpClient)
        {
            int num = 0;
            tcpClient.SendCommand("STAT");
            string response = tcpClient.GetResponse(false);
            if (!this.CheckResponse(response))
            {
                return num;
            }
            Match match = new Regex(@"\+OK\s*(?<MsgCount>\d*)\s*(?<MsgSize>\d*)", RegexOptions.ExplicitCapture).Match(response);
            if (!match.Success)
            {
                return num;
            }
            string text2 = match.Groups["MsgCount"].Value;
            try
            {
                return Convert.ToInt32(text2);
            }
            catch
            {
                return 0;
            }
        }

        private Pop3MessageCollection GetMessages(bool delete)
        {
            Pop3TcpClient tcpClient = this.ConnectServer();
            Pop3MessageCollection messages = new Pop3MessageCollection();
            try
            {
                int messageCount = this.GetMessageCount(tcpClient);
                if (messageCount <= 0)
                {
                    return messages;
                }
                for (int i = messageCount; i > 0; i--)
                {
                    tcpClient.SendCommand(string.Format("RETR {0}", i));
                    string response = tcpClient.GetResponse(true);
                    if (this.CheckResponse(response))
                    {
                        Pop3Message message = this.ParseMessage(response);
                        if (delete)
                        {
                            tcpClient.SendCommand(string.Format("DELE {0}", i));
                            response = tcpClient.GetResponse(false);
                            if (this.CheckResponse(response))
                            {
                                messages.Add(message);
                            }
                        }
                        else
                        {
                            messages.Add(message);
                        }
                    }
                }
            }
            finally
            {
                tcpClient.Disconnect();
            }
            return messages;
        }

        private Pop3Message ParseMessage(string response)
        {
            Pop3Message message = new Pop3Message();
            Match match = new Regex(@"From:\s*(?<From>.*)$", RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase).Match(response);
            if (match.Success)
            {
                message.From = match.Groups["From"].Value;
            }
            else
            {
                message.From = string.Empty;
            }
            Match match2 = new Regex(@"To:\s*(?<To>.*)$", RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase).Match(response);
            if (match2.Success)
            {
                message.To = match2.Groups["To"].Value;
            }
            else
            {
                message.To = string.Empty;
            }
            Match match3 = new Regex(@"Subject:\s*(?<Subject>.*)$", RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase).Match(response);
            if (match3.Success)
            {
                message.Subject = match3.Groups["Subject"].Value;
            }
            else
            {
                message.Subject = string.Empty;
            }
            message.Message = response.Substring(response.IndexOf("\r\n\r\n") + 2);
            return message;
        }

        public Pop3Message PeekMessage()
        {
            return this.GetMessage(false);
        }

        public Pop3MessageCollection PeekMessages()
        {
            return this.GetMessages(false);
        }

        public Pop3Message PopMessage()
        {
            return this.GetMessage(true);
        }

        public Pop3MessageCollection PopMessages()
        {
            return this.GetMessages(true);
        }

        // Nested Types
        public class Pop3Message
        {
            // Fields
            private string from;
            private string message;
            private string subject;
            private string to;

            // Methods
            public Pop3Message()
            {
            }

            public Pop3Message(string title, string from, string to, string message)
            {
                this.subject = title;
                this.from = from;
                this.to = to;
                this.message = message;
            }

            // Properties
            public string From
            {
                get
                {
                    return this.from;
                }
                set
                {
                    this.from = value;
                }
            }

            public string Message
            {
                get
                {
                    return this.message;
                }
                set
                {
                    this.message = value;
                }
            }

            public string Subject
            {
                get
                {
                    return this.subject;
                }
                set
                {
                    this.subject = value;
                }
            }

            public string To
            {
                get
                {
                    return this.to;
                }
                set
                {
                    this.to = value;
                }
            }
        }

        [Serializable]
        public class Pop3MessageCollection : CollectionBase, IComponent, IDisposable
        {
            // Fields
            private ISite _site = null;

            // Events
            public event EventHandler Disposed;

            // Methods
            public int Add(Pop3Client.Pop3Message aPop3Message)
            {
                return base.List.Add(aPop3Message);
            }

            public bool Contains(Pop3Client.Pop3Message aPop3Message)
            {
                return (this.Find(aPop3Message) != null);
            }

            public void Dispose()
            {
                if (this.Disposed != null)
                {
                    this.Disposed(this, EventArgs.Empty);
                }
            }

            public Pop3Client.Pop3Message Find(Pop3Client.Pop3Message aPop3Message)
            {
                foreach (Pop3Client.Pop3Message message in this)
                {
                    if (message == aPop3Message)
                    {
                        return message;
                    }
                }
                return null;
            }

            public Pop3Client.Pop3Message Find(int hashCode)
            {
                foreach (Pop3Client.Pop3Message message in this)
                {
                    if (message.GetHashCode() == hashCode)
                    {
                        return message;
                    }
                }
                return null;
            }

            public int IndexOf(Pop3Client.Pop3Message aPop3Message)
            {
                for (int i = 0; i < base.List.Count; i++)
                {
                    if (this[i] == aPop3Message)
                    {
                        return i;
                    }
                }
                return -1;
            }

            public void Insert(int index, Pop3Client.Pop3Message aPop3Message)
            {
                base.List.Insert(index, aPop3Message);
            }

            public void Remove(Pop3Client.Pop3Message aPop3Message)
            {
                base.List.Remove(aPop3Message);
            }

            // Properties
            public Pop3Client.Pop3Message this[int index]
            {
                get
                {
                    return (Pop3Client.Pop3Message)base.List[index];
                }
                set
                {
                    base.List[index] = value;
                }
            }

            public ISite Site
            {
                get
                {
                    return this._site;
                }
                set
                {
                    this._site = value;
                }
            }
        }

        public class Pop3TcpClient : IDisposable
        {
            // Fields
            private NetworkStream networkStream;
            private TcpClient tcpClient = new TcpClient();

            // Methods
            public Pop3TcpClient(string server, int port)
            {
                this.tcpClient.Connect(server, port);
                this.networkStream = this.tcpClient.GetStream();
                this.GetResponse(false);
            }

            public void Disconnect()
            {
                this.SendCommand("QUIT");
                this.Dispose();
            }

            public void Dispose()
            {
                if (this.networkStream != null)
                {
                    this.networkStream.Close();
                }
                if (this.tcpClient != null)
                {
                    this.tcpClient.Close();
                }
            }

            public string GetResponse(bool multiline)
            {
                string text = multiline ? "\r\n.\r\n" : "\r\n";
                DateTime now = DateTime.Now;
                TimeSpan span = TimeSpan.FromSeconds(10);
                if (!this.networkStream.CanRead)
                {
                    return null;
                }
                byte[] buffer = new byte[0x1000];
                StringBuilder builder = new StringBuilder();
                int count = 0;
                do
                {
                    if (this.networkStream.DataAvailable)
                    {
                        now = DateTime.Now;
                        count = this.networkStream.Read(buffer, 0, buffer.Length);
                        builder.Append(Encoding.ASCII.GetString(buffer, 0, count));
                    }
                    else if (DateTime.Now.Subtract(now) > span)
                    {
                        Trace.WriteLine("Timeout getting data from Pop3 Server...");
                        break;
                    }
                }
                while (builder.ToString().IndexOf(text) < 0);
                return builder.ToString();
            }

            public void SendCommand(string command)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(command + "\r\n");
                this.networkStream.Write(bytes, 0, bytes.GetLength(0));
            }
        }
    }
}