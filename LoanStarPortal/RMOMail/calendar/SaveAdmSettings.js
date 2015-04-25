
//function creates a string of all settings parameters through a separator "#",//
//then raises a server processor, which parsed entrance row and//
//received all the parameters in settings.xml//
function SendAdmSettings()
{
    var allParameters = "";//save all the parameters of settings as a string through a separator "#" here//
    var container = document.getElementById("cldrSettingsID_AllParameters");
    //---------------------------------------------------------------------------------------------------------------------------------------
    var _defTimeFormat1 = document.getElementById("defTimeFormat1");//Radio box//
    var _defTimeFormat2 = document.getElementById("defTimeFormat2");//Radio box//
    var _defDateFormat = document.getElementById("defDateFormat");//Select//
    var _showWeekends = document.getElementById("showWeekends");//Check box//
    var _WorkdayStarts = document.getElementById("WorkdayStarts");//Select//
    var _WorkdayEnds = document.getElementById("WorkdayEnds");//Select//
    var _showWorkDay = document.getElementById("showWorkDay");//Check box//
    var _weekStartsOn = document.getElementById("WeekStartsOn");//Select//
    var _defTab_daily = document.getElementById("defTab_daily");//Radio box//
    var _defTab_weekly = document.getElementById("defTab_weekly");//Radio box//
    var _defTab_monthly = document.getElementById("defTab_monthly");//Radio box//
    var _defCountry = document.getElementById("defCountry");//Select//
    var _defTimeZone = document.getElementById("defTimeZone");//Select//
    var _allTimeZones = document.getElementById("allTimeZones");//Check box//
    //-----------------------------------//
    //tentatively check all the data here//
    //-----------------------------------//
    if(_defTimeFormat1.checked) allParameters += "1";
    if(_defTimeFormat2.checked) allParameters += "2";
    allParameters += "#";
    allParameters += _defDateFormat.selectedIndex + 1;
    allParameters += "#";
    if(_showWeekends.checked) allParameters += "1"; else allParameters += "0";
    allParameters += "#";
    allParameters += _WorkdayStarts.selectedIndex;
    allParameters += "#";
    allParameters += _WorkdayEnds.selectedIndex;
    allParameters += "#";
    if(_showWorkDay.checked) allParameters += "1"; else allParameters += "0";
    allParameters += "#";
    if(_defTab_daily.checked) allParameters += "1";
    if(_defTab_weekly.checked) allParameters += "2";
    if(_defTab_monthly.checked) allParameters += "3";
    allParameters += "#";
    allParameters += _defCountry.value;
    allParameters += "#";
    allParameters += _defTimeZone.options[_defTimeZone.selectedIndex].value
    allParameters += "#";
    if(_allTimeZones.checked) allParameters += "1"; else allParameters += "0";
    allParameters += "#";
    allParameters += _weekStartsOn.selectedIndex;
    //---------------------------------------------------------------------------------------------------------------------------------------
    container.value = allParameters;
    __doPostBack('cldrSettingsID$SaveSettings', '');
}