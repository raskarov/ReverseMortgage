<%@ Control Language="c#" Inherits="Telerik.WebControls.EditorDialogControls.FindAndReplace" AutoEventWireUp="false" CodeBehind="FindAndReplace.ascx.cs" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<%@ Register TagPrefix="telerik" TagName="TabControl" Src="../Controls/TabControl.ascx" %>
<%@ Register TagPrefix="telerik" NameSpace="Telerik.WebControls.EditorControls" Assembly="RadEditor.Net2" %>
<%@ OutputCache Duration="600" VaryByParam="Language;SkinPath" %>
<table id="mainTable" width="100%" cellpadding="0" cellspacing="0" class="MainTable">
	<tr>
		<th height="39" valign="bottom">
			<telerik:tabcontrol id="TabHolder" runat="server" resizecontrolid="mainTable">
				<telerik:tab image="Dialogs/TabIcons/FindAndReplaceTab1.gif" text="<script>localization.showText('Tab1HeaderText');</script>" selected="True" elementid="TabbedEmptySpan1" onclientclick="SwitchTab(false);" />
				<telerik:tab image="Dialogs/TabIcons/FindAndReplaceTab2.gif" text="<script>localization.showText('Tab2HeaderText');</script>" elementid="TabbedEmptySpan2" onclientclick="SwitchTab(true);" />
			</telerik:tabcontrol>
		</th>
	</tr>
	<tr>
		<td class="MainTableContentCell">
			<span id="TabbedEmptySpan1"></span><span id="TabbedEmptySpan2"></span>

<table height="100%" cellpadding="6" cellspacing="0" border="0">
	<tr>
		<td valign="top">
			<table cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td width="90" style="PADDING-BOTTOM:5px">
						<label for="<%=this.ClientID%>_searchText">
							<script>localization.showText('SearchText')</script>
						</label>
					</td>
					<td style="PADDING-RIGHT:10px;PADDING-BOTTOM:5px">
						<input type="text" id="<%=this.ClientID%>_searchText" style="WIDTH:210px" class="Text" tabindex=1>
					</td>
					<td style="PADDING-BOTTOM:5px">
						<button class="Button" type="button" onclick="javascript:Find();" tabindex=9>
							<script>localization.showText('FindNext')</script>
						</button>
					</td>
				</tr>
				<tr id="<%=this.ClientID%>_replaceDialogRow" style="DISPLAY:none">
					<td width="90">
						<label for="<%=this.ClientID%>_replacementText">
							<script>localization.showText('ReplacementText')</script>
						</label>
					</td>
					<td>
						<input type="text" id="<%=this.ClientID%>_replacementText" name="replacementText" style="WIDTH:210px" class="Text" tabindex=2>
					</td>
					<td>
						<button class="Button" type="button" onclick="javascript:Replace();" tabindex=10>
							<script>localization.showText('Replace')</script>
						</button>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<table cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td style="PADDING-RIGHT:4px" class="Label">
									<fieldset style="PADDING-LEFT:5px;WIDTH:133px">
										<legend>
											<script>localization.showText('SearchIn')</script>
										</legend>
										<input type="radio" name="SearchType" onclick="javascript:SetSearchType(this.value);" value="Whole" checked tabindex=3><label for="searchTypeRadioWholeText"><script>localization.showText('SearchEntireText')</script></label><br>
										<input type="radio" name="SearchType" onclick="javascript:SetSearchType(this.value);" value="Selection" tabindex=4><label for="searchTypeRadioSelectionOnly"><script>localization.showText('SearchSelection')</script></label>
									</fieldset>
								</td>
								<td class="Label">
									<fieldset style="PADDING-LEFT:5px;WIDTH:133px">
										<legend>
											<script>localization.showText('SearchDirection')</script>
										</legend>
										<input type="radio" name="SearchDir" onclick="javascript:SetSearchDirection(this.value);" value="Up" tabindex=5><label for="searchDirectionRadioUp"><script>localization.showText('SearchUp')</script></label><br>
										<input type="radio" name="SearchDir" onclick="javascript:SetSearchDirection(this.value);" checked value="Down" tabindex=6><label for="searchDirectionRadioDown"><script>localization.showText('SearchDown')</script></label>
									</fieldset>
								</td>
							</tr>
							<tr>
								<td colspan="2">&nbsp;</td>
							</tr>
							<tr>
								<td colspan="2" class="Label">
									<p>
										<input type="checkbox" style="MARGIN-LEFT:-4px" onclick="javascript:SetCaseMatch(this.checked);" tabindex=7><label for="searchModifierMatchCaseCheckBox"><script>localization.showText('MatchCase')</script></label><br>
										<input type="checkbox" style="MARGIN-LEFT:-4px" onclick="javascript:SetWholeWordMatch(this.checked);" tabindex=8><label for="searchModifierMatchWholeWordsCheckBox"><script>localization.showText('WholeWordsOnly')</script></label>
									</p>
								</td>
							</tr>
						</table>
					</td>
					<td valign="top" style="PADDING-TOP:0px" height="100%">
						<table cellpadding="0" cellspacing="0" border="0" height="100%">
							<tr id="<%=this.ClientID%>_replaceAllDialogRow" style="DISPLAY:none">
								<td style="PADDING-BOTTOM:7px;PADDING-TOP:0px">
									<button class="Button" type="button" onclick="javascript:ReplaceAll();" tabindex=11>
										<script>localization.showText('ReplaceAll')</script>
									</button>
								</td>
							</tr>
							<tr>
								<td valign="top">
									<table border="0" cellpadding="0" cellspacing="0">
										<tr>
											<td style="pading-bottom:4px" tabindex=12>
												<button class="Button" id="submitButton" onclick="javascript:CloseWindow();" type="button" >
													<script>localization.showText('OK')</script>
												</button>
											</td>
										</tr>
										<tr>
											<td style="PADDING-BOTTOM:6px;PADDING-TOP:4px">
												<button class="Button" onclick="javascript:CancelChanges()" type="button" tabindex=13>
													<script>localization.showText('Cancel')</script>
												</button>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>

		</td>
	</tr>
</table>
<script language="javascript">
	var globalControlRef = null;
	var globalControlId = "<%=this.ClientID%>";
	var globalWorkingArea = null;
	var	globalOriginalHtml = null;
		
	var changesMade = false;
			
	var searchBox = document.getElementById(globalControlId+ "_searchText");	
	var replaceBox = document.getElementById(globalControlId+ "_replacementText");
		
	function SetSearchType(oBool)
	{
		//"Whole"/"Selection"
		globalControlRef.SelectionOnly = "Selection" ? true : false;
	}
	
	function SetSearchDirection(dir)
	{
		//"Up/Down"
		globalControlRef.SearchUp = "Up" ? true : false;
	}
	
	function SetCaseMatch(oBool)
	{
		//true/false
		globalControlRef.CaseSensitive = oBool;
	}
	
	function SetWholeWordMatch(oBool)
	{
		//true/false
		globalControlRef.WholeWord = oBool;
	}			
	
	function Find()
	{
		globalControlRef.SearchWord = searchBox.value;				
		globalControlRef.Find();
	}
	
	function Replace()
	{
		changesMade = true;
		globalControlRef.SearchWord = searchBox.value;
		globalControlRef.ReplaceWord = replaceBox.value;		
		globalControlRef.Replace();
	}
	
	function ReplaceAll()
	{
		changesMade = true;
		
		globalControlRef.ResetEngine();//Reset engine
			
		globalControlRef.SearchWord = searchBox.value;
		globalControlRef.ReplaceWord = replaceBox.value;		
		
		globalControlRef.ReplaceAll();
	}


	function SwitchTab(showReplace)
	{
		//Reset engine
		if (globalControlRef) globalControlRef.ResetEngine();
		
		//Set which fields to be visible
		var row1 = document.getElementById(globalControlId+ "_replaceDialogRow");	
		var row2 = document.getElementById(globalControlId+ "_replaceAllDialogRow");		
		row1.style.display = showReplace ? "" : "none";
		row2.style.display = showReplace ? "" : "none";
						
		//Set focus
		var searchTextHolder = document.getElementById(globalControlId + "_searchText");
		if (searchTextHolder) searchTextHolder.focus();
	}


	function CloseWindow()
	{				
		CloseDlg(null, null, false);
	}

	function CancelChanges()
	{		
		window.radWindow.CallbackFunc = null;
		if (changesMade)
		{
			if (globalWorkingArea.tagName.toLowerCase() == "textarea")
			{
				globalWorkingArea.value = globalOriginalHtml;
			}
			else
			{
				globalWorkingArea.innerHTML = globalOriginalHtml;
			}
		}					
		CloseDlg(null, null, false);
	}

	function InitControl()
	{
		var args = GetDialogArguments();
		
		globalWorkingArea = args.area;
		globalOriginalHtml = globalWorkingArea.innerHTML;						
		globalControlRef = new FindAndReplaceControl('<%=this.ClientID%>', globalWorkingArea);						
		
		
		globalControlRef.OriginalSelection = globalControlRef.GetRange();//Store range		

		globalControlRef.OnCancelChanges = CancelChanges;
		globalControlRef.OnCloseWindow = CloseWindow;
	}

	AttachEvent(window, "load", InitControl);
</script>