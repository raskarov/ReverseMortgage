using System;
using System.Collections;
using System.Globalization;
using System.Web.UI;
using System.Xml;
using System.Xml.XPath;
using NetSpell.SpellChecker;
using System.IO;
using System.Text.RegularExpressions;

namespace WebMailPro
{
    public partial class spellcheck : Page
    {
        private NetSpell.SpellChecker.Spelling SpellChecker;
        private NetSpell.SpellChecker.Dictionary.WordDictionary WordDictionary;
        private const char _marker = '|';
        private const int MAX_SUGGESTIONS = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            ArrayList arr = new ArrayList();

            string xmlText = Request["xml"];

            if (arr.Count > 0) xmlText = (string)arr[0];
            if (xmlText != null)
            {
                Log.WriteLine("", ">>>>>>>>>>>>>>>>  IN  >>>>>>>>>>>>>>>>");
                Log.WriteLine("", xmlText);
                Log.WriteLine("", ">>>>>>>>>>>>>>>>  IN  >>>>>>>>>>>>>>>>");

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlText);
                XmlNodeReader nodeReader = new XmlNodeReader(doc);
                XPathDocument xpathDoc = new XPathDocument(nodeReader);
                XPathNavigator xpathNav = xpathDoc.CreateNavigator();
                XPathNodeIterator xpathParamNodeIter = xpathNav.Select(@"webmail/param");
                string action = "";
                string suggestWord = "";
                while (xpathParamNodeIter.MoveNext())
                {
                    string name = xpathParamNodeIter.Current.GetAttribute("name", "");
                    string val = xpathParamNodeIter.Current.GetAttribute("value", "");
                    if ((val == null) || (val.Length == 0))
                    {
                        val = Utils.DecodeHtml(xpathParamNodeIter.Current.Value);
                    }

                    switch (name.ToLower(CultureInfo.InvariantCulture))
                    {
                        case "request":
                            action = val;
                            break;
                        case "word":
                            suggestWord = val;
                            break;
                    }
                }

                if (action == "spell")
                {
                    XPathNavigator textNode = xpathNav.SelectSingleNode(@"webmail/text");
                    string text = Utils.DecodeHtml(textNode.Value);
                    doc = CreateServerXmlDocumentResponse(action, text);

                }
                else if (action == "suggest")
                {
                    doc = CreateServerXmlDocumentResponse(action, suggestWord);
                }

                Response.Clear();
                Response.ContentType = @"text/xml";
                Log.WriteLine("", "<<<<<<<<<<<<<<<<<  OUT  <<<<<<<<<<<<<<<");
                Log.WriteLine("", doc.OuterXml);
                Log.WriteLine("", "<<<<<<<<<<<<<<<<<  OUT  <<<<<<<<<<<<<<<");
                Response.Write(doc.OuterXml);
                Response.End();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            // if not in cache, create new
            WordDictionary = new NetSpell.SpellChecker.Dictionary.WordDictionary();
            WordDictionary.EnableUserFile = false;

            //getting folder for dictionaries
            string dicFolder = Path.Combine(Utils.GetDataFolderPath(), @"dictionary");
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			string defLang = settings.DefaultLanguage;
			Account acct = (Account)Session[Constants.sessionAccount];
			if (acct != null)
			{
				defLang = acct.UserOfAccount.Settings.DefaultLanguage;
			}
            string dictionaryFile = "en-US.dic";
			switch (defLang)
			{
				case "French": dictionaryFile = "fr-FR.dic"; break;
				case "German": dictionaryFile = "de-DE.dic"; break;
			}
            WordDictionary.DictionaryFolder = dicFolder;
            WordDictionary.DictionaryFile = dictionaryFile;

            //load and initialize the dictionary
            WordDictionary.Initialize();

            // create spell checker
            SpellChecker = new NetSpell.SpellChecker.Spelling();
            SpellChecker.ShowDialog = false;
            SpellChecker.Dictionary = WordDictionary;

        }

        private XmlDocument CreateServerXmlDocumentResponse(string action, string text)
        {
            Regex _wordEx, _htmlEx;
            XmlDocument result = new XmlDocument();
            result.PreserveWhitespace = true;
            XmlDeclaration xmlDecl = result.CreateXmlDeclaration("1.0", "utf-8", "");
            result.AppendChild(xmlDecl);
            XmlElement webmailNode = result.CreateElement("webmail");
            result.AppendChild(webmailNode);
            XmlElement spellcheckNode = result.CreateElement("spellcheck");
            webmailNode.AppendChild(spellcheckNode);

            if (action == "spell")
            {
                spellcheckNode.SetAttribute("action", "spellcheck");
                _htmlEx = new Regex(@"(<.*?\>)|(&nbsp;)", RegexOptions.IgnoreCase & RegexOptions.Compiled);
                //_htmlEx = new Regex(@"</[c-g\d]+>|</[i-o\d]+>|</[a\d]+>|</[q-z\d]+>|<[cg]+[^>]*>|<[i-o]+[^>]*>|<[q-z]+[^>]*>|<[a]+[^>]*>|<(\[^\]*\|'[^']*'|[^'\>])*>", RegexOptions.IgnoreCase & RegexOptions.Compiled);
                //_htmlEx = new Regex(@"/</?\w+((\s+\w+(\s*=\s*(?:\"".*?\""|\'.*?\'|[^\'\"">\s]+))?)+\s*|\s*)/?>/", RegexOptions.IgnoreCase & RegexOptions.Compiled);
                
                text = _htmlEx.Replace(text, ReplaceHtml);
                
                Log.WriteLine("", ">>>>>>>>>>>>>>>>  TEXT  >>>>>>>>>>>>>>>>");
                Log.WriteLine("", text);
                Log.WriteLine("", ">>>>>>>>>>>>>>>>  TEXT  >>>>>>>>>>>>>>>>");

                _wordEx = new Regex(@"\b[" + WordDictionary.TryCharacters + @"']+\b", RegexOptions.Compiled);
                MatchCollection words = _wordEx.Matches(text);

                foreach (Match word in words)
                {
                    if (!SpellChecker.TestWord(word.ToString()))
                    {
                        XmlElement node = result.CreateElement("misp");
                        spellcheckNode.AppendChild(node);
                        node.SetAttribute("pos", word.Index.ToString());
                        node.SetAttribute("len", word.Length.ToString());
                    }
                }
            }
            else if (action == "suggest")
            {
                spellcheckNode.SetAttribute("action", "suggest");
                SpellChecker.Suggest(text);
                int i = 0;
                foreach (string word in SpellChecker.Suggestions)
                {
                    if (i++ < MAX_SUGGESTIONS)
                    {
                        XmlElement node = result.CreateElement("param");
                        spellcheckNode.AppendChild(node);
                        node.SetAttribute("name", "word");
                        node.SetAttribute("value", word.ToString());
                    }
                }
            }

            return result;
        }
        private string ReplaceHtml(Match m)
        {
            string result = "";
            for (int i = 0; i < m.Length; i++)
            {
                result += _marker;
            }
            return result;
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
            this.Load += new EventHandler(this.Page_Load);
            this.Init += new EventHandler(this.Page_Init);
        }
        #endregion
    }
}
