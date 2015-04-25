<%@ Control Language="c#" AutoEventWireup="True" Codebehind="mailadm_database_settings.ascx.cs" Inherits="WebMailPro.MailAdm.mailadm_database_settings" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type="text/javascript">
	
	function DisableAllTextBoxes()
	{
		document.getElementById('dbSettingsID_txtSqlLogin').readOnly = true;
		document.getElementById('dbSettingsID_txtSqlLogin').style.background = "#EEEEEE"; 

		document.getElementById('dbSettingsID_txtSqlPassword').readOnly = true;
		document.getElementById('dbSettingsID_txtSqlPassword').style.background = "#EEEEEE"; 

		document.getElementById('dbSettingsID_txtSqlName').readOnly = true;
		document.getElementById('dbSettingsID_txtSqlName').style.background = "#EEEEEE"; 

		document.getElementById('dbSettingsID_txtSqlDsn').readOnly = true;
		document.getElementById('dbSettingsID_txtSqlDsn').style.background = "#EEEEEE"; 

		document.getElementById('dbSettingsID_txtSqlSrc').readOnly = true;
		document.getElementById('dbSettingsID_txtSqlSrc').style.background = "#EEEEEE"; 

		document.getElementById('dbSettingsID_txtAccessFile').readOnly = true;
		document.getElementById('dbSettingsID_txtAccessFile').style.background = "#EEEEEE"; 
	}

	function SqlRadioClick()
	{
		DisableAllTextBoxes();
		document.getElementById('dbSettingsID_txtSqlLogin').readOnly = false;
		document.getElementById('dbSettingsID_txtSqlLogin').style.background = "#FFFFFF";
		
		document.getElementById('dbSettingsID_txtSqlPassword').readOnly = false;
		document.getElementById('dbSettingsID_txtSqlPassword').style.background = "#FFFFFF";
		
		document.getElementById('dbSettingsID_txtSqlName').readOnly = false;
		document.getElementById('dbSettingsID_txtSqlName').style.background = "#FFFFFF";
		
		document.getElementById('dbSettingsID_txtSqlDsn').readOnly = false;
		document.getElementById('dbSettingsID_txtSqlDsn').style.background = "#FFFFFF";
		
		document.getElementById('dbSettingsID_txtSqlSrc').readOnly = false;
		document.getElementById('dbSettingsID_txtSqlSrc').style.background = "#FFFFFF";
	}

	function AccessRadioClick()
	{
		DisableAllTextBoxes();
		document.getElementById('dbSettingsID_txtAccessFile').readOnly = false;
		document.getElementById('dbSettingsID_txtAccessFile').style.background = "#FFFFFF";
	}
	
	function UseCustomConnectionStringClick()
	{
		if (document.getElementById('dbSettingsID_useCS').checked)
		{
			document.getElementById('dbSettingsID_odbcConnectionString').readOnly = false;
			document.getElementById('dbSettingsID_odbcConnectionString').style.background = "#FFFFFF";
		}
		else
		{
			document.getElementById('dbSettingsID_odbcConnectionString').readOnly = true;
			document.getElementById('dbSettingsID_odbcConnectionString').style.background = "#EEEEEE";
		}
		//document.getElementById('dbSettingsID_odbcConnectionString').readOnly = !document.getElementById('dbSettingsID_useCS').checked;
	}
</script>
<table class="wm_admin_center" width="500">
	<tr>
		<td width="170"></td>
		<td></td>
	</tr> <!-- 1 -->
	<tr>
		<td class="wm_admin_title" colSpan="2">Database Settings</td>
	</tr>
	<tr>
		<td colSpan="2"><br>
		</td>
	</tr>
	<tr>
		<td align="left" colSpan="2"><input id="intDbTypeMsSql" style="VERTICAL-ALIGN: middle" onclick="SqlRadioClick()" type="radio"
				name="intDbType" runat="server"> <label id="labid0" for="dbSettingsID_intDbTypeMsSql">
				<strong>MS SQL Server</strong></label>
		</td>
	</tr> <!-- 2 -->
	<tr>
		<td align="left" colSpan="2"><input id="intDbTypeMySql" style="VERTICAL-ALIGN: middle" onclick="SqlRadioClick()" type="radio"
				name="intDbType" runat="server"> <label id="labid1" for="dbSettingsID_intDbTypeMySql">
				<strong>MySQL</strong></label>&nbsp;
		</td>
	</tr>
	<tr>
		<td align="right">SQL login:
		</td>
		<td><input class="wm_input" id="txtSqlLogin" type="text" size="45" runat="server" />
		</td>
	</tr>
	<tr>
		<td align="right">SQL password:
		</td>
		<td><asp:textbox id="txtSqlPassword" runat="server" TextMode="password" Columns="45" CssClass="wm_input"></asp:textbox></td>
	</tr>
	<tr>
		<td align="right">Database name:
		</td>
		<td><input class="wm_input" id="txtSqlName" type="text" size="45" runat="server">
		</td>
	</tr>
	<tr>
		<td align="right">Data source (DSN):
		</td>
		<td><input class="wm_input" id="txtSqlDsn" type="text" size="45" runat="server">
		</td>
	</tr>
	<tr>
		<td align="right">Host:
		</td>
		<td><input class="wm_input" id="txtSqlSrc" type="text" size="45" runat="server">
		</td>
	</tr> <!-- 3 -->
	<tr>
		<td align="left" colSpan="2"><br>
			<input id="intDbTypeMsAccess" style="VERTICAL-ALIGN: middle" onclick="AccessRadioClick()"
				type="radio" name="intDbType" runat="server"> <label id="labid2" for="dbSettingsID_intDbTypeMsAccess">
				<strong>MS Access</strong></label>
		</td>
	</tr>
	<tr>
		<td align="right">MS Access file (*.mdb):
		</td>
		<td id="AccessTd" align="left"><input type="text" Class="wm_input" id="txtAccessFile" size="45" runat="server">
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td align="left"><A class="wm_list_item_link" href="mailadm.aspx?mode=wm_db_compact">Compact 
				.mdb database</A>
		</td>
	</tr> <!-- 4 -->
	<tr>
		<td align="left" colSpan="2"><br>
			<!--<asp:RadioButton Runat="server" style="VERTICAL-ALIGN: middle" GroupName="intDbType" id="intDbType3" />
			<label for="intDbType3" id="labid3"><strong>ODBC Connection String</strong></label>--></td>
	</tr>
	<tr>
		<td align="right">Connection String:
		</td>
		<td align="left"><input class="wm_input" id="odbcConnectionString" type="text" size="45" runat="server">
		</td>
	</tr>
	<tr>
		<td align="right"></td>
		<td><input class="wm_checkbox" id="useCS" onclick="UseCustomConnectionStringClick()" type="checkbox"
				runat="server"> <label for="dbSettingsID_useCS">Use connection string</label>
		</td>
	</tr>
	<tr>
		<td align="center" colSpan="2"><br>
			<div Runat="server" ID="messLabelID" Class="messdiv" />
			<br>
			<div runat="server" id="errorLabelID" class="messdiv" />
		</td>
	</tr> <!-- hr -->
	<tr>
		<td colSpan="2">
			<hr SIZE="1">
		</td>
	</tr>
	<tr>
		<td align="left">
			<asp:button id="test_connection" style="FONT-WEIGHT: bold; FLOAT: left" Width="150" Text="Test Connection"
				Runat="server" onclick="test_connection_Click"></asp:button>
			<asp:Button ID="create_tables" Text="Create Tables" Runat="server" style="FONT-WEIGHT: bold; FLOAT: left"
				Width="150" onclick="create_tables_Click" />
		</td>
		<td align="right">
			<asp:button id="SubmitButton" Width="100" Text="Save" Runat="server" onclick="SubmitButton_Click"></asp:button>&nbsp;
		</td>
	</tr>
</table>
<script type="text/javascript">
<!--
	if (document.getElementById('dbSettingsID_intDbTypeMsSql').checked || document.getElementById('dbSettingsID_intDbTypeMySql').checked)
	{
		SqlRadioClick();
	}
	else if (document.getElementById('dbSettingsID_intDbTypeMsAccess').checked)
	{
		AccessRadioClick();
	}
	UseCustomConnectionStringClick();
//-->
</script>
