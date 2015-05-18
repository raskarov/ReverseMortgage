<%@ Control Language="C#" AutoEventWireup="true" Codebehind="Emails.ascx.cs" Inherits="LoanStarPortal.Controls.Emails" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>
<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal"
    Width="100%" BorderWidth="0" BorderStyle="None" BorderSize="0" Skin="Default"
    LiveResize="false">
    <radspl:RadPane ID="TopPane" runat="server" Height="100%" Scrolling="Both">
    <div style="width:100%;text-align:center">
        <div style="width:70%;padding-top:90px;text-align:justify; font-size:16px;">
        <p> If you see this page, it means you do not have your email configured correctly OR chose not to use RM LOS™ as an email client with your existing email account. If you would like to use RMEngage™ as an email client, please contact RMEngage™ support for assistance.</p>
<p> To create a lead or loan, roll over the “Resources” menu above and click “Create new mortgage”.</p>

        </div>
    </div>
    </radspl:RadPane>
</radspl:RadSplitter>

<script type="text/javascript" >
function ShowEmailLinks(argsSelectedMenuItem, selectedMsg){   
   var backColor = '';
   var mailMode = '' ;   
   //back to list
   if(argsSelectedMenuItem.ScreenId == 1 && argsSelectedMenuItem.FolderId == null){
      mailMode = 'Default';
      backColor = 'White';
   }
   //new message   
   else if(argsSelectedMenuItem.ScreenId == 4){
      mailMode = 'NewMessage';
      backColor = '#C0FFFF';
   }
   //open message
   else if(argsSelectedMenuItem.ScreenId == 3){
      mailMode = 'OpenMessage';
      backColor = '#C0FFFF';
   }
   //preview
   else if(argsSelectedMenuItem.ScreenId == 1 && argsSelectedMenuItem.FolderId > 0 && argsSelectedMenuItem.IdAcct > 0){
       mailMode = 'Preview';
       backColor = 'White';
   }
   //default
   else {
      mailMode = 'Default';
      backColor = 'White';
   }
   SetAppListControl(mailMode, backColor, selectedMsg);   
}

function ShowEmailLinks(argsSelectedMenuItem, selectedMsg){   
   var backColor = '';
   var mailMode = '' ;
   var needReload =false;
   //back to list
   if(argsSelectedMenuItem.ScreenId == 1 && argsSelectedMenuItem.FolderId == null){
      mailMode = 'Default';
      backColor = 'White';
      needReload = true;
   }
   //new message   
   else if(argsSelectedMenuItem.ScreenId == 4){
      mailMode = 'NewMessage';
      backColor = '#C0FFFF';
   }
   //open message
   else if(argsSelectedMenuItem.ScreenId == 3){
      mailMode = 'OpenMessage';
      backColor = '#C0FFFF';
   }
   //preview
   else if(argsSelectedMenuItem.ScreenId == 1 && argsSelectedMenuItem.FolderId > 0 && argsSelectedMenuItem.IdAcct > 0){
       mailMode = 'Preview';
       backColor = 'White';
   }
   //default
   else {
      mailMode = 'Default';
      backColor = 'White';
   }
   SetAppListControl(mailMode, backColor, selectedMsg); 
}
function SetAppListControl(mailMode, backColor, selectedMsg) {
    var appItems = document.getElementById("ApplicantList1_RadPBMortgages").all
    var iItemCount = appItems.length;
    var hostRoot = window.location.protocol + "//" +  window.location.host;
    while (iItemCount-- > 0) {
        if (appItems[iItemCount].type == 'panelitem') {
            if(mailMode == 'Default'){
                var replaceIMG = '<IMG class=image alt="" src="'+ hostRoot + '/Images/MailLink.gif">';
                appItems[iItemCount].innerHTML = appItems[iItemCount].innerHTML.replace(replaceIMG,"");
                var s=appItems[iItemCount].id.replace(/_/g,'$');
                appItems[iItemCount].pathname ="AjaxNS.AR('ApplicantList1$RadPBMortgages','"+s+"', 'RadAjaxManager1', event)";
            }            
            if(mailMode == 'NewMessage' || mailMode == 'OpenMessage')
            {
                var selectedApplicant = new Array(1);
                selectedApplicant[0] = new Array(3);
            
                selectedApplicant[0][0] = appItems[iItemCount].id;
                selectedApplicant[0][1] = appItems[iItemCount].title;
                selectedApplicant[0][2] = appItems[iItemCount].outerText;
                
                args = selectedApplicant;
                appItems[iItemCount].pathname = 'ChkMortgageClicked("' + 
                                                    selectedApplicant[0][0]  + '","' +
                                                    selectedApplicant[0][1]  + '","' +
                                                    selectedApplicant[0][2]  + '","' +
                                                    mailMode  + '");';
                if(selectedMsg != null){                
                    if(selectedMsg.LoanAppIDs.indexOf(appItems[iItemCount].title) > -1){
                        if(appItems[iItemCount].innerHTML.indexOf('MailLink.gif') == -1)
                            appItems[iItemCount].innerHTML = '<IMG class=image alt="" src="Images/MailLink.gif">' + appItems[iItemCount].innerHTML;
                     }
                     else {
                        var replaceIMG = '<IMG class=image alt="" src="'+ hostRoot + '/Images/MailLink.gif">';
                        appItems[iItemCount].innerHTML = appItems[iItemCount].innerHTML.replace(replaceIMG,"");   
                   }
                }                                   
            }
            appItems[iItemCount].style.backgroundColor = backColor;
        }
    }
}
</script>
