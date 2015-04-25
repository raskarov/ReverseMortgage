using System.Globalization;

namespace WebMailPro.classic
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for SettingsCommon.
	/// </summary>
	public partial class SettingsCommon : System.Web.UI.UserControl
	{
		protected WebmailResourceManager _resMan = null;
		protected string _skin = Constants.defaultSkinName;
		protected jsbuilder _js;
		public jsbuilder js
		{
			get { return _js; }
			set { _js = value; }
		}
		public string Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			_js.AddText(@"
				function CheckSCFields()
				{
					var oVal = new CValidate();
					if (oVal.IsEmpty(document.getElementById('"+textBoxAdvancedDate.ClientID+@"').value))
					{
						alert(Lang.WarningAdvancedDateFormat);
						return false;
					}
					if (!oVal.IsPositiveNumber(document.getElementById('"+textBoxMessagesPerPage.ClientID+ @"').value))
					{
						alert(Lang.WarningMessagesPerPage);
						return false;
					}
					return true;
				}
function SetAdvanced()
{
	var select = document.getElementById(""settingsID_settingsCommonID_date_format"");
	if (select) select.value = ""advanced"";
}
			
function ChangeAdvanced(objSelect)
{
	var advInput = document.getElementById(""settingsID_settingsCommonID_textBoxAdvancedDate"");
	if (advInput && objSelect && objSelect.value != ""advanced"")
	{
		advInput.value = objSelect.value;
	}
}

			");
			InitControls();
			// Put user code to initialize the page here
			Account acct = Session[Constants.sessionAccount] as Account;
			if ((acct != null) && (acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
			{
				this.textBoxMessagesPerPage.Value = acct.UserOfAccount.Settings.MsgsPerPage.ToString(CultureInfo.InvariantCulture);
				this.checkBoxDisableRte.Checked = !acct.UserOfAccount.Settings.AllowDhtmlEditor;
				
				string[] skins = Utils.GetSupportedSkins(Page.MapPath("skins"));
				int selectedSkinIndex = 0;
				for (int i = 0; i < skins.Length; i++)
				{
					string skin = skins[i];
					ListItem li = new ListItem(skin, skin);
					comboBoxSkins.Items.Add(li);
					if (string.Compare(skin, acct.UserOfAccount.Settings.DefaultSkin, true, CultureInfo.InvariantCulture) == 0)
					{
						selectedSkinIndex = i;
					}
				}
				comboBoxSkins.SelectedIndex = selectedSkinIndex;

				ListItem liCharset = comboBoxDefaultCharset.Items.FindByValue(acct.UserOfAccount.Settings.DefaultCharsetInc.ToString(CultureInfo.InvariantCulture));
				if (liCharset != null)
				{
					liCharset.Selected = true;
				}
				else
				{
					comboBoxDefaultCharset.SelectedIndex = 0;
				}

				ListItem liTimeOffset = comboBoxTimeOffset.Items.FindByValue(acct.UserOfAccount.Settings.DefaultTimeZone.ToString(CultureInfo.InvariantCulture));
				if (liTimeOffset != null)
				{
					liTimeOffset.Selected = true;
				}
				else
				{
					comboBoxTimeOffset.SelectedIndex = 0;
				}

				string[] supportedLangs = Utils.GetSupportedLangs();
				int selectedLangIndex = 0;
				for (int i = 0; i < supportedLangs.Length; i++)
				{
					string lang = supportedLangs[i];
					ListItem li = new ListItem(lang, lang);
					comboBoxLang.Items.Add(li);
					if (string.Compare(lang, acct.UserOfAccount.Settings.DefaultLanguage, true, CultureInfo.InvariantCulture) == 0)
					{
						selectedLangIndex = i;
					}
				}
				comboBoxLang.SelectedIndex = selectedLangIndex;

				switch (acct.UserOfAccount.Settings.DefaultDateFormat.ToLower(CultureInfo.InvariantCulture))
				{
					case Constants.DateFormats.DDMMYY:
                        date_format.Value = Constants.DateFormats.DDMMYY;
						break;
					case Constants.DateFormats.MMDDYY:
                        date_format.Value = Constants.DateFormats.MMDDYY;
						break;
					case Constants.DateFormats.DDMonth:
                        date_format.Value = Constants.DateFormats.DDMonth;
						break;
					default:
                        date_format.Value = "advanced";
						break;
				}

                this.textBoxAdvancedDate.Value = acct.UserOfAccount.Settings.DefaultDateFormat.ToLower(CultureInfo.InvariantCulture);

                if (acct.UserOfAccount.Settings.DefaultTimeFormat == TimeFormats.F24)
                {
                    time_format_24.Checked = true;
                }
                else
                {
                    time_format_12.Checked = true;
                }

				this.view_mode.Checked = ((acct.UserOfAccount.Settings.ViewMode & ViewMode.WithPreviewPane) > 0) ? true : false;
				this.pictures_show.Checked = ((acct.UserOfAccount.Settings.ViewMode & ViewMode.AlwaysShowPictures) > 0) ? true : false;
			}
			DataBind();
		}

		private void InitControls()
		{
            DateTime date = DateTime.Now;
            if (date.ToString("MM") == date.ToString("dd"))
            {
                date = date.AddDays(21 - date.Day);
            }

            date_format.Items.Add(new ListItem(date.ToString("MM/dd/yy", CultureInfo.InvariantCulture), Constants.DateFormats.MMDDYY));
            date_format.Items.Add(new ListItem(date.ToString("dd/MM/yy", CultureInfo.InvariantCulture), Constants.DateFormats.DDMMYY));
            date_format.Items.Add(new ListItem(date.ToString("dd MMM", CultureInfo.InvariantCulture), Constants.DateFormats.DDMonth));
            date_format.Items.Add(new ListItem(_resMan.GetString("DateAdvanced"), "advanced"));

			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetDefault"), "0"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetArabicAlphabetISO"), "28596"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetArabicAlphabet"), "1256"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetBalticAlphabetISO"), "28594"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetBalticAlphabet"), "1257"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetCentralEuropeanAlphabetISO"), "28592"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetCentralEuropeanAlphabet"), "1250"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetChineseSimplifiedEUC"), "51936"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetChineseSimplifiedGB"), "936"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetChineseTraditional"), "950"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetCyrillicAlphabetISO"), "28595"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetCyrillicAlphabetKOI8R"), "20866"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetCyrillicAlphabet"), "1251"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetGreekAlphabetISO"), "28597"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetGreekAlphabet"), "1253"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetHebrewAlphabetISO"), "28598"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetHebrewAlphabet"), "1255"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetJapanese"), "50220"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetJapaneseShiftJIS"), "932"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetKoreanEUC"), "949"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetKoreanISO"), "50225"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetLatin3AlphabetISO"), "28593"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetTurkishAlphabet"), "1254"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetUniversalAlphabetUTF7"), "65000"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetUniversalAlphabetUTF8"), "65001"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetVietnameseAlphabet"), "1258"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetWesternAlphabetISO"), "28591"));
			comboBoxDefaultCharset.Items.Add(new ListItem(_resMan.GetString("CharsetWesternAlphabet"), "1252"));

			comboBoxTimeOffset.Items.Add(new ListItem(_resMan.GetString("TimeDefault"), "0"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -12:00) {0}", _resMan.GetString("TimeEniwetok")), "1"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -11:00) {0}", _resMan.GetString("TimeMidwayIsland")), "2"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -10:00) {0}", _resMan.GetString("TimeHawaii")), "3"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -09:00) {0}", _resMan.GetString("TimeAlaska")), "4"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -08:00) {0}", _resMan.GetString("TimePacific")), "5"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -07:00) {0}", _resMan.GetString("TimeArizona")), "6"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -07:00) {0}", _resMan.GetString("TimeMountain")), "7"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -06:00) {0}", _resMan.GetString("TimeCentralAmerica")), "8"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -06:00) {0}", _resMan.GetString("TimeCentral")), "9"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -06:00) {0}", _resMan.GetString("TimeMexicoCity")), "10"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -06:00) {0}", _resMan.GetString("TimeSaskatchewan")), "11"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -05:00) {0}", _resMan.GetString("TimeIndiana")), "12"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -05:00) {0}", _resMan.GetString("TimeEastern")), "13"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -05:00) {0}", _resMan.GetString("TimeBogota")), "14"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -04:00) {0}", _resMan.GetString("TimeSantiago")), "15"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -04:00) {0}", _resMan.GetString("TimeCaracas")), "16"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -04:00) {0}", _resMan.GetString("TimeAtlanticCanada")), "17"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -03:30) {0}", _resMan.GetString("TimeNewfoundland")), "18"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -03:00) {0}", _resMan.GetString("TimeGreenland")), "19"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -03:00) {0}", _resMan.GetString("TimeBuenosAires")), "20"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -03:00) {0}", _resMan.GetString("TimeBrasilia")), "21"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -02:00) {0}", _resMan.GetString("TimeMidAtlantic")), "22"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -01:00) {0}", _resMan.GetString("TimeCapeVerde")), "23"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT -01:00) {0}", _resMan.GetString("TimeAzores")), "24"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT) {0}", _resMan.GetString("TimeMonrovia")), "25"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT) {0}", _resMan.GetString("TimeGMT")), "26"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +01:00) {0}", _resMan.GetString("TimeBerlin")), "27"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +01:00) {0}", _resMan.GetString("TimePrague")), "28"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +01:00) {0}", _resMan.GetString("TimeParis")), "29"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +01:00) {0}", _resMan.GetString("TimeSarajevo")), "30"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +01:00) {0}", _resMan.GetString("TimeWestCentralAfrica")), "31"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +02:00) {0}", _resMan.GetString("TimeAthens")), "32"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +02:00) {0}", _resMan.GetString("TimeEasternEurope")), "33"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +02:00) {0}", _resMan.GetString("TimeCairo")), "34"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +02:00) {0}", _resMan.GetString("TimeHarare")), "35"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +02:00) {0}", _resMan.GetString("TimeHelsinki")), "36"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +02:00) {0}", _resMan.GetString("TimeIsrael")), "37"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +03:00) {0}", _resMan.GetString("TimeBaghdad")), "38"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +03:00) {0}", _resMan.GetString("TimeArab")), "39"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +03:00) {0}", _resMan.GetString("TimeMoscow")), "40"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +03:00) {0}", _resMan.GetString("TimeEastAfrica")), "41"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +03:30) {0}", _resMan.GetString("TimeTehran")), "42"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +04:00) {0}", _resMan.GetString("TimeAbuDhabi")), "43"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +04:00) {0}", _resMan.GetString("TimeCaucasus")), "44"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +04:30) {0}", _resMan.GetString("TimeKabul")), "45"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +05:00) {0}", _resMan.GetString("TimeEkaterinburg")), "46"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +05:00) {0}", _resMan.GetString("TimeIslamabad")), "47"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +05:30) {0}", _resMan.GetString("TimeBombay")), "48"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +05:45) {0}", _resMan.GetString("TimeNepal")), "49"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +06:00) {0}", _resMan.GetString("TimeAlmaty")), "50"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +06:00) {0}", _resMan.GetString("TimeDhaka")), "51"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +06:00) {0}", _resMan.GetString("TimeSriLanka")), "52"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +06:30) {0}", _resMan.GetString("TimeRangoon")), "53"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +07:00) {0}", _resMan.GetString("TimeBangkok")), "54"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +07:00) {0}", _resMan.GetString("TimeKrasnoyarsk")), "55"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +08:00) {0}", _resMan.GetString("TimeBeijing")), "56"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +08:00) {0}", _resMan.GetString("TimeIrkutsk")), "57"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +08:00) {0}", _resMan.GetString("TimeSingapore")), "58"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +08:00) {0}", _resMan.GetString("TimePerth")), "59"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +08:00) {0}", _resMan.GetString("TimeTaipei")), "60"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +09:00) {0}", _resMan.GetString("TimeTokyo")), "61"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +09:00) {0}", _resMan.GetString("TimeSeoul")), "62"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +09:00) {0}", _resMan.GetString("TimeYakutsk")), "63"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +09:30) {0}", _resMan.GetString("TimeAdelaide")), "64"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +09:30) {0}", _resMan.GetString("TimeDarwin")), "65"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +10:00) {0}", _resMan.GetString("TimeBrisbane")), "66"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +10:00) {0}", _resMan.GetString("TimeSydney")), "67"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +10:00) {0}", _resMan.GetString("TimeGuam")), "68"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +10:00) {0}", _resMan.GetString("TimeHobart")), "69"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +10:00) {0}", _resMan.GetString("TimeVladivostock")), "70"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +11:00) {0}", _resMan.GetString("TimeMagadan")), "71"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +12:00) {0}", _resMan.GetString("TimeWellington")), "72"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +12:00) {0}", _resMan.GetString("TimeFiji")), "73"));
			comboBoxTimeOffset.Items.Add(new ListItem(string.Format(@"(GMT +13:00) {0}", _resMan.GetString("TimeTonga")), "74"));
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

		protected void saveButton_ServerClick(object sender, System.EventArgs e)
		{
			Account acct = Session[Constants.sessionAccount] as Account;
			if ((acct != null) && (acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
			{
				try
				{
					BaseWebMailActions actions = new BaseWebMailActions(acct, this.Page);
					short msgsOnPage = 10;

					if (Validation.CheckIt(Validation.ValidationTask.MPP, this.textBoxMessagesPerPage.Value))
					{
						msgsOnPage = short.Parse(Validation.Corrected);
					}
					else
					{
						msgsOnPage = acct.UserOfAccount.Settings.MsgsPerPage;                        						
					}


					ListItem li = comboBoxDefaultCharset.Items[comboBoxDefaultCharset.SelectedIndex];
					int def_charset = int.Parse(li.Value, CultureInfo.InvariantCulture);

					li = comboBoxTimeOffset.Items[comboBoxTimeOffset.SelectedIndex];
					short def_timezone = short.Parse(li.Value, CultureInfo.InvariantCulture);

                    string def_date_format = Constants.DateFormats.MMDDYY;
					if (!Validation.CheckIt(Validation.ValidationTask.Advanced, textBoxAdvancedDate.Value))
					{
						def_date_format = acct.UserOfAccount.Settings.DefaultDateFormat;
					}
					else
					{
						def_date_format = Validation.Corrected;
					}

                    TimeFormats time_format = (time_format_12.Checked) ? TimeFormats.F12 : TimeFormats.F24;

                    ViewMode mode = ViewMode.WithoutPreviewPane;
					if (view_mode.Checked) mode |= ViewMode.WithPreviewPane;
					if (pictures_show.Checked) mode |= ViewMode.AlwaysShowPictures;

                    actions.UpdateSettings(!this.checkBoxDisableRte.Checked, def_charset, def_charset, def_date_format, comboBoxLang.Value, comboBoxSkins.Value, def_timezone, msgsOnPage, (byte)mode, time_format);
					Session[Constants.sessionAccount] = acct;
					Session[Constants.sessionUserID] = acct.IDUser;
					Session[Constants.sessionReportText] = _resMan.GetString("ReportSettingsUpdatedSuccessfuly");

					Response.Redirect("basewebmail.aspx?scr=settings_common"); // to update interface if we change skin
				}
				catch (Exception ex)
				{
					((basewebmail)this.Page).OutputException(ex);
				}
			}
		}
	}
}
