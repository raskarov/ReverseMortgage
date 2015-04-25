<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicantList.ascx.cs" Inherits="LoanStarPortal.Controls.ApplicantList" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>
<%@ Register Assembly="RadPanelbar.Net2" Namespace="Telerik.WebControls" TagPrefix="radP" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>

<script language="javascript" type="text/javascript" defer="defer">
function ChkMortgageClicked(selectedItemId, selectedAppID, selectedAppFullName, mailMode){
    var selectedItem = document.getElementById(selectedItemId);
    var innerhtml = document.getElementById(selectedItemId).innerHTML;
    var loanAppNameID = '';
    var loanAppIDsID = '';    
    switch(mailMode){
      case 'Default':
        loanAppNameID = 'loanAppNames';
        loanAppIDsID = 'loanAppIDs';
      break;
      case 'NewMessage':
        loanAppNameID = 'loanAppNames';
        loanAppIDsID = 'loanAppIDs';
      break;
      case 'OpenMessage':
        loanAppNameID = 'loanAppNamesView';
        loanAppIDsID = 'loanAppIDsView';
      break;      
      default: 
        loanAppNameID = 'loanAppNames';
        loanAppIDsID = 'loanAppIDs';
      case 'Preview':
        loanAppNameID = 'loanAppNamesPreview';
        loanAppIDsID = 'loanAppIDsPreview';        
      break;    
    }    
    var b=0;
    if(innerhtml.indexOf('MailLink.gif') > -1) {
        //found it now reset everything
        var hostRoot = window.location.protocol + "//" +  window.location.host;
        var replaceIMG = '<IMG class=image alt="" src="'+ hostRoot + '/Images/MailLink.gif">';
        selectedItem.innerHTML = selectedItem.innerHTML.replace(replaceIMG,"");
        var loanNewMsg = document.frames[1].document.getElementById(loanAppNameID);
        if(loanNewMsg != null){
          if(mailMode == 'OpenMessage')
            document.frames[1].document.getElementById(loanAppNameID).innerText =  loanNewMsg.innerText.replace(selectedAppFullName + ';', '');
          else
            document.frames[1].document.getElementById(loanAppNameID).value =  loanNewMsg.value.replace(selectedAppFullName + ';', '');
        }
        b=0;
        var loanAppIDs = document.frames[1].document.getElementById(loanAppIDsID);
        if(loanAppIDs != null){
          document.frames[1].document.getElementById(loanAppIDsID).value =  loanAppIDs.value.replace(selectedAppID + ';', '');
        }
        document.getElementById(selectedItemId).className = "link";
    } else {
        document.getElementById(selectedItemId).className = "link";
        document.getElementById(selectedItemId).innerHTML = '<IMG class=image alt="" src="Images/MailLink.gif">' + selectedItem.innerHTML;
        var loanNewMsg = document.frames[1].document.getElementById(loanAppNameID);
        if(loanNewMsg != null){
            if(mailMode == 'OpenMessage')
                document.frames[1].document.getElementById(loanAppNameID).innerText =  loanNewMsg.innerText + selectedAppFullName + ';' ;
            else
                document.frames[1].document.getElementById(loanAppNameID).value =  loanNewMsg.value + selectedAppFullName + ';' ;
        }
        var loanAppIDs = document.frames[1].document.getElementById(loanAppIDsID);
        if(loanAppIDs != null){
            document.frames[1].document.getElementById(loanAppIDsID).value =  loanAppIDs.value + selectedAppID + ';' ;
        }
        b=1;
    }
    var wm = document.frames[1].window['WebMail'];
    if(wm!=null)    
        wm.UpdateMessageAssociation(document.frames[1].document.getElementById(loanAppNameID).innerText,document.frames[1].document.getElementById(loanAppIDsID).value);
    UpdateAssociatians('appid='+selectedAppID+'&op='+b);
}
function UpdateAssociatians(req){
    var xmlhttp = GetXmlHttp();
    if (xmlhttp==null){
        alert('ajax not supported');
        return;
    }
	var url=document.URL;
	var i = url.indexOf('?');
	if (i>0){
	    url=url.substring(0,i);
	}
	url = url.toLowerCase().replace('default.aspx','ProcessRequest.aspx');
	url+='?'+req;
	xmlhttp.open("GET",url,true);	
	xmlhttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');						
	xmlhttp.setRequestHeader('Accept-Language','en/us');
	xmlhttp.setRequestHeader("Content-Length", "0");
	xmlhttp.onreadystatechange=function(){
		if(xmlhttp.readyState==4){
			var resp=xmlhttp.responseText;
			var statusText=xmlhttp.statusText;
			var status=xmlhttp.status;			
			if (status==200){
			    xmlhhtp=null;
			}else{
            	alert(statusText);
			}
		}
	}
	xmlhttp.send(null);
}
function OnClientGroupClickingHandler(sender, eventArgs){
    if (!CheckError()) return false;
    if(eventArgs.Item.Level > 1) return true;
    if(eventArgs.Item.Expanded)
        eventArgs.Item.Collapse();
    else
        eventArgs.Item.Expand();
    return false;
}
function OnClientIndexChanging(item){
    var res = CheckError();
    if (!res){
        item.ComboBox.HideDropDown();
    }
    return res;
}
function OnClientClick(o){
    var res = CheckError();
    if (!res){
        o.checked=false;
        var s='';
        if (o.id=='ApplicantList1_rbAllLoans'){
            s='ApplicantList1_rbMyPipeline';
        }else{
            s='ApplicantList1_rbAllLoans';
        }
        var o1=document.getElementById(s);
        if (o1!=null){
            o1.checked=true;
        }
    }
    return res;
}
</script>

<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Width="100%" BorderWidth="0" BorderStyle="None" BorderSize="0" Skin="Default" LiveResize="false">
   <radspl:radpane id="TopPane" runat="server" Height="130px" Scrolling="None">
   <div RadResizeStopLookup="true" RadShowStopLookup="true" style="">
    <div class="paneTitle"><b>Mortgages</b></div>
    <table border="0" cellspacing="3" cellpadding="3" width="100%">
        <tr>
            <td style="width:100%;" colspan="2" align="center" >
                <asp:RadioButton ID="rbAllLoans" Text="All Loans" runat="server" GroupName="Switcher" 
                    OnCheckedChanged="rbAllLoans_CheckedChanged" AutoPostBack="True" onclick="if (!OnClientClick(this)) return false;"/>
                <asp:RadioButton ID="rbMyPipeline" Text="My Pipeline" runat="server" GroupName="Switcher" 
                    OnCheckedChanged="rbMyPipeline_CheckedChanged" Checked="True" AutoPostBack="True" onclick="if (!OnClientClick(this)) return false;"/>
            </td>
        </tr>
        <tr>
            <td style="width:10%;">Display: </td>
            <td>
                <radC:RadComboBox ID="ddlDisplay" runat="server" Skin="WindowsXP" OnSelectedIndexChanged="ddlDisplay_SelectedIndexChanged" 
                    AutoPostBack="true" Width="110px" OnClientSelectedIndexChanging="OnClientIndexChanging">
                </radC:RadComboBox>
            </td>
        </tr>        
        <tr>
            <td style="width:10%;">Grouping: </td>
            <td>
                <radC:RadComboBox ID="ddlGroup" runat="server" Skin="WindowsXP" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" 
                    AutoPostBack="true" Width="110px" OnClientSelectedIndexChanging="OnClientIndexChanging">
                </radC:RadComboBox>
            </td>
        </tr>
        <tr style="visibility:hidden;">
            <td style="width:10%;">Sorting: </td>
            <td>
                <radC:RadComboBox ID="dllSort" runat="server" Skin="WindowsXP" OnSelectedIndexChanged="dllSort_SelectedIndexChanged" 
                    AutoPostBack="true" Width="110px" OnClientSelectedIndexChanging="OnClientIndexChanging">
                <Items>
                    <radC:RadComboBoxItem Text="A - Z" Value="YBLastName ASC" Selected="true" />
                    <radC:RadComboBoxItem Text="Z - A" Value="YBLastName DESC" />
                </Items>
                </radC:RadComboBox>
            </td>
        </tr>
    </table>
    </div>
   </radspl:radpane>  
   <radspl:radsplitbar id="RadSplitBar3" runat="server" CollapseMode="None" EnableResize="false" Visible="false"/>
   <radspl:radpane id="MiddlePane" runat="server" scrolling="Y" >
   <div RadResizeStopLookup="true" RadShowStopLookup="true" >
    <radP:RadPanelbar runat="server" ID="RadPBMortgages" Skin="PipeLine" Width="100%" CausesValidation="False" 
        OnClientItemClicking="OnClientGroupClickingHandler" OnItemClick="RadPBMortgages_ItemClick" UseEmbeddedScripts="False" >
    </radP:RadPanelbar>
    </div>
    </radspl:radpane>
 </radspl:RadSplitter>
