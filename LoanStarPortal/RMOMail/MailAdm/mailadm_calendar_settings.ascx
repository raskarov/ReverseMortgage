<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="mailadm_calendar_settings.ascx.cs" Inherits="Calendar_NET.mailadm_calendar_settings" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

<div>

    <div style="display:none;">
        <asp:LinkButton ID="SaveSettings" runat="server" OnCommand="sendSettings_Click" Text="" />
    </div>
    
    <asp:HiddenField runat="server" ID="AllParameters" Value="" />

    <input type="hidden" name="form_id" value="settings"/>
    <table class="wm_admin_center" width="500" border="0">
        <tr>
	        <td width="150"></td>
	        <td width="160"></td>
        </tr>

        <tr>
	        <td colspan="2" class="wm_admin_title">Calendar Settings</td>
        </tr>

        <tr><td colspan="2"><br /></td></tr>
        <tr>
	        <td align="right">Default Time Format: </td>
	        <td>
		        <input type="radio" name="defTimeFormat" id="defTimeFormat1" value="1" class="wm_checkbox"
		        <% if (settings.DefaultTimeFormat == 1) Response.Write("checked=\'checked\'"); %> />
		        <label for="defTimeFormat1">1PM</label>&nbsp;&nbsp;&nbsp;
		        <input type="radio" name="defTimeFormat" id="defTimeFormat2" value="2" class="wm_checkbox"
		        <% if (settings.DefaultTimeFormat == 2) Response.Write("checked=\'checked\'"); %> />
		        <label for="defTimeFormat2">13:00</label>
	        </td>
        </tr>

    	
        <tr>
	        <td align="right">Default Date Format: </td>
	        <td>
	        <select name="defDateFormat" id="defDateFormat">
		        <option value="1" <% if (settings.DefaultDateFormat == 1) Response.Write("selected=\'selected\'");%>><% Response.Write(timeFormat1); %></option>
		        <option value="2" <% if (settings.DefaultDateFormat == 2) Response.Write("selected=\'selected\'");%>><% Response.Write(timeFormat2); %></option>			
		        <option value="3" <% if (settings.DefaultDateFormat == 3) Response.Write("selected=\'selected\'");%>><% Response.Write(timeFormat3); %></option>
		        <option value="4" <% if (settings.DefaultDateFormat == 4) Response.Write("selected=\'selected\'");%>><% Response.Write(timeFormat4); %></option>			
		        <option value="5" <% if (settings.DefaultDateFormat == 5) Response.Write("selected=\'selected\'");%>><% Response.Write(timeFormat5); %></option>
	        </select>
	        </td>
        </tr>
    	
    		
        <tr>
	        <td></td>
	        <td><input type="checkbox" name="showWeekends" id="showWeekends" value="1" class="wm_checkbox" 
	        <% Response.Write( ((int) settings.ShowWeekends == 1) ? "checked='checked'" : "" );%> />
	        <label for="showWeekends">Show weekends</label></td>
        </tr>
    	

        <tr>
	        <td align="right">Workday starts: </td>
	        <td>
				<span id="WorkdayStartsCont"></span>
				&nbsp;&nbsp;ends: <span id="WorkdayEndsCont"></span>
	        </td>
        </tr>
        <tr>
	        <td></td>
	        <td>
		         <input type="checkbox" name="showWorkDay" id="showWorkDay" value="1" class="wm_checkbox"
		         <% if (settings.ShowWorkDay == 1) Response.Write("checked=\'checked\'");%>/>
		         <label for="showWorkDay">Show workday</label>
	        </td>
        </tr>
    	
        <tr>
	        <td align="right">Week starts on: </td>
	        <td>
	        <select name="weekstartson" id="WeekStartsOn">
		        <option value="0" <% if (settings.WeekStartsOn == 0) Response.Write("selected=\'selected\'");%>>Sunday</option>
		        <option value="1" <% if (settings.WeekStartsOn == 1) Response.Write("selected=\'selected\'");%>>Monday</option>			
	        </select>
	        </td>
        </tr>
    	
        <tr>
	        <td align="right">Default Tab: </td>
	        <td>
		         <input type="radio" name="defTab" id="defTab_daily" value="1" class="wm_checkbox" <% if (settings.DefaultTab == 1) Response.Write( "checked=\'checked\'" );%>/>
		         <label for="defTab_daily">Day</label>&nbsp;&nbsp;&nbsp;
		         <input type="radio" name="defTab" id="defTab_weekly" value="2" class="wm_checkbox" <% if (settings.DefaultTab == 2) Response.Write( "checked=\'checked\'" );%>/>
		         <label for="defTab_weekly">Week</label>&nbsp;&nbsp;&nbsp;
		         <input type="radio" name="defTab" id="defTab_monthly" value="3" class="wm_checkbox" <% if (settings.DefaultTab == 3) Response.Write( "checked=\'checked\'" );%>/>
		         <label for="defTab_monthly">Month</label>
	        </td>
        </tr>

    		
        <tr>
	        <td align="right">Default Country: </td>
	        <td>
		        <select style="width:300px" name="defCountry" id="defCountry">
                <% 
                    for (int i = 0; i < countries.Count; i++)
                    {
                        string code = countries[i].Split('-')[0];
                        string countryName = countries[i].Split('-')[1];
                        Response.Write( "<option value='" + code + "'" );
                        if (settings.DefaultCountry == code) Response.Write( "selected='selected'" );
                        Response.Write(">" + countryName + "</option>\r\n");
                    }
                %>    
                </select>	 			 
	        </td>
        </tr>
    	
        <tr>
	        <td align="right">Default Time Zone: </td>
	        <td id="defTimeZoneCont">
		        <select id="defTimeZone" style="width: 300px;" name="defTimeZone">
		            
		        </select>
	        </td>
        </tr>
        <tr>
	        <td></td>	
	        <td>
		        <input type="checkbox" name="allTimeZones" id="allTimeZones" value="1" class="wm_checkbox" <% if (settings.AllTimeZones == 1) Response.Write( "checked=\'checked\'" );%>/>
		        <label for="allTimeZones">All time zones</label>
	        </td>
        </tr>
    	
        <tr>
	        <td colspan="2" align="center">
		        <div runat="server" id="messLabelID" class="messdiv" />
		        <br />
    			<div runat="server" id="errorLabelID" class="messdiv" />
	        </td>
        </tr>
        <!-- hr -->
        <tr><td colspan="2"><hr size="1" /></td></tr>

        <tr>
	        <td colspan="2" align="right">
		        <input type="submit"
		               id="SaveButton" 
		               name="save" 
		               class="wm_button" 
		               value="Save" 
		               onclick="SendAdmSettings();"
		               style="width: 100px; font-weight: bold" />
		        &nbsp;
	        </td>
        </tr>
    </table>
</div>

<script type="text/javascript">
<!--
//********************************************************************************************************************************************
//Calendar Admin Setiings, page settings
function ASSettings() 
{
	this.WorkdayStartsCont  = document.getElementById('WorkdayStartsCont');
	this.WorkdayEndsCont    = document.getElementById('WorkdayEndsCont');
	this.defCountry		    = document.getElementById('defCountry');
	this.defTimeZoneCont    = document.getElementById('defTimeZoneCont');
	this.timeZoneValue      = "";
	this.allTimeZones	    = document.getElementById('allTimeZones');
	//----------------------------------------------------------------------------------------------------------------------------------------
	this.defTimeZone        = null;
	//----------------------------------------------------------------------------------------------------------------------------------------

	//----------------------------------------------------------------------------------------------------------------------------------------
	var set_workdayStarts	 =  <% Response.Write( settings.WorkdayStarts ); %>;
	var set_workdayEnds		 =  <% Response.Write( settings.WorkdayEnds ); %>;
	var set_timeFormat		 =  <% Response.Write( settings.DefaultTimeFormat ); %>;
	var set_allZones		 =  <% Response.Write( (settings.AllTimeZones == 1) ? 1 : 0 ); %>;
	this.set_DefaultTimeZone =  <% Response.Write( settings.DefaultTimeZone ); %>;
	//----------------------------------------------------------------------------------------------------------------------------------------	
	this.SetTimeFormat(this.WorkdayStartsCont, set_workdayStarts, set_timeFormat);
	this.WorkdayStarts	= document.getElementById('WorkdayStarts');
	this.SetTimeFormat(this.WorkdayEndsCont,  set_workdayEnds, set_timeFormat);
	this.WorkdayEnds	= document.getElementById('WorkdayEnds');
    //----------------------------------------------------------------------------------------------------------------------------------------
	this.LoadTimeZones(set_allZones, this.set_DefaultTimeZone);
    //----------------------------------------------------------------------------------------------------------------------------------------    
    //set handlers
	var obj = this;	
	//----------------------------------------------------------------------------------------------------------------------------------------
	/*radiobox changes time format*/
	document.getElementById("defTimeFormat1").onclick = function() 
	{
		obj.SetTimeFormat(obj.WorkdayStartsCont, obj.WorkdayStarts.value, 1);
	 	obj.SetTimeFormat(obj.WorkdayEndsCont, obj.WorkdayEnds.value, 1);
	}
	//----------------------------------------------------------------------------------------------------------------------------------------
	document.getElementById("defTimeFormat2").onclick = function() 
	{
		obj.SetTimeFormat(obj.WorkdayStartsCont, obj.WorkdayStarts.value, 2);
	 	obj.SetTimeFormat(obj.WorkdayEndsCont, obj.WorkdayEnds.value, 2);
	}	 
	//----------------------------------------------------------------------------------------------------------------------------------------
	/*reload timesones when change country*/	
	this.defCountry.onchange = function() 
	{
		var allZones = (obj.allTimeZones.checked) ? 1 : 0;
        obj.LoadTimeZones(allZones, ""); 
	}
	//----------------------------------------------------------------------------------------------------------------------------------------
	/*reload timezones when choose all timezones*/
	this.allTimeZones.onclick = function() 
	{
		var allZones = (this.checked) ? 1 : 0;
		obj.LoadTimeZones(allZones, obj.timeZoneValue);
	} 
    //****************************************************************************************************************************************
}
//********************************************************************************************************************************************
ASSettings.prototype = {

	//----------------------------------------------------------------------------------------------------------------------------------------
	/*check form on submit*/
	Settings_OnSubmit : function()
	{
		return true;
	},

	//----------------------------------------------------------------------------------------------------------------------------------------
	SetTimeFormat: function(WorkdayContainer, WorkdayValue, TimeFormat) 
	{
		var k = 0;
		var hour = "";
		var selected = "";
		var time = "";
		WorkdayContainer.innerHTML = "";
		var sel = CreateChild(WorkdayContainer, "select");
		sel.name = sel.id = WorkdayContainer.id.substr(0,(WorkdayContainer.id.length-4));
		sel.style.width = "100px";
 
		for (var i = 0; i < 24; i++) 
		{
			opt = CreateChild(sel,"option");
			opt.className = "ts_text";

			if (TimeFormat == 1) 
			{
				if (i==12) k = 0;
				
				if (k==0) 
				    hour = 12;
				else 
				    hour=k;
						
				time = hour + ((i < 12) ? " AM" : " PM");
				k++;
			} 
			else if (TimeFormat == 2) 
			{
				time = (i < 10) ? ("0" + i + ":00") : (i + ":00");
			}
					
			if (i == WorkdayValue) 
			    opt.selected = "selected";
			else 
			    selected = "";

			opt.value = i;
			opt.text = time;
		}
	},
	//----------------------------------------------------------------------------------------------------------------------------------------
	LoadTimeZones: function(allTimeZones, defaultTimeZone)
	{
        var obj = this;
        
        var _defTimeZone = "<select id='defTimeZone' style='width: 300px;' name='defTimeZone'>"
        var i = "";
        var code = this.defCountry.value;
        
        if(allTimeZones)
        {
            if(this.defTimeZone != null) this.set_DefaultTimeZone = this.defTimeZone.options[this.defTimeZone.selectedIndex].value;
            obj.defTimeZoneCont.innerHTML = allTimeZone;
            this.defTimeZone = document.getElementById("defTimeZone");
            this.defTimeZone.selectedIndex = parseInt(this.set_DefaultTimeZone, 10) - 1;
        }
        else
        {
            for(i in timeZoneForCountry[code])
            {
                var index = timeZoneForCountry[code][i];
                var timeZoneValue = AllTimeZonesArr[index];
                if(this.set_DefaultTimeZone == index)
                {
                    _defTimeZone += "<option value='" + index + "' selected='selected'>" + timeZoneValue + "</option>\r\n";
                }
                else
                {
                    _defTimeZone += "<option value='" + index + "'>" + timeZoneValue + "</option>\r\n";
                }
            }
            _defTimeZone += "</select>";
            obj.defTimeZoneCont.innerHTML = _defTimeZone;
            this.defTimeZone = document.getElementById("defTimeZone");
            if (this.defTimeZone && this.defTimeZone.selectedIndex >= 0) {
				this.set_DefaultTimeZone = this.defTimeZone.options[this.defTimeZone.selectedIndex].value;
            }
        }
	}
}

//*******************************************************************************************************************************************
var admSet = new ASSettings();
//*******************************************************************************************************************************************

//-->
</script>

