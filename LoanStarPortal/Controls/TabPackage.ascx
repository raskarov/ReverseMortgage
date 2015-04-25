<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TabPackage.ascx.cs" Inherits="LoanStarPortal.Controls.TabPackage" %>
<%@ Register Src="Package.ascx" TagName="Package" TagPrefix="uc1" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<script language="javascript" type="text/javascript" defer="defer">
function OnClientTabPackageLoaded()
{
    var tabStrip = <%= RadTabStripPackage.ClientID %>;
    RefreshTabPackageButtons(tabStrip);
    
    if (document.getElementById('TabPackage_resTabPackageMessage').value.length > 0)
        alert(document.getElementById('TabPackage_resTabPackageMessage').value)
    document.getElementById('TabPackage_resTabPackageMessage').value = '';
}

function OnClientTabSelectedHandler(sender, eventArgs)
{
    RefreshTabPackageButtons(sender);
}

function RefreshTabPackageButtons(tabStrip)
{
    switch(tabStrip.SelectedIndex)
    {
        case 0:
            {
                document.getElementById('TabPackage_btnSendToClosingAgent').style.display = "none";
                AppPackageDTSelected();
            }
            break;
        case 1:
            {
                document.getElementById('TabPackage_btnSendToClosingAgent').style.display = "block";
                ClosingPackageDTSelected();
            }
            break;
        case 2:
            {
                document.getElementById('TabPackage_btnSendToClosingAgent').style.display = "none";
                MiscPackageDTSelected();
            }
            break;
        default:
            break;
    }
}

function AppPackageDTSelected()
{
    var divObj = document.getElementById('TabPackage_divAppPackage');
    ActivatePrintPackBtn(divObj);
}

function ClosingPackageDTSelected()
{
    var divObj = document.getElementById('TabPackage_divClosingPackage');
    ActivatePrintPackBtn(divObj);
}

function MiscPackageDTSelected()
{
    var divObj = document.getElementById('TabPackage_divMiscPackage');
    ActivatePrintPackBtn(divObj);
}

function ActivatePrintPackBtn(divObj)
{
    var isChecked = false;
    var cbList = divObj.getElementsByTagName('input');
    for (var i = 0; i < cbList.length; i++)
        if(cbList[i].type == 'checkbox' && cbList[i].checked == true)
        {
            isChecked = true;
            break;
        }
            
    var btnPrintPackObj = document.getElementById('TabPackage_btnPrintPack');
    btnPrintPackObj.disabled = !isChecked;
}
</script>
<radspl:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Orientation="Horizontal" Width="100%" BorderWidth="0" BorderStyle="None" BorderSize="0" Skin="Default" LiveResize="false">
    <radspl:RadPane ID="TopPane" runat="server" Height="57px" Scrolling="None">
    <div RadResizeStopLookup="true" RadShowStopLookup="true" >
    <div class="paneTitle"><b>Package</b></div>
    <asp:HiddenField ID="resTabPackageMessage" runat="server" />
    <radTS:RadTabStrip id="RadTabStripPackage" runat="server" Skin="Outlook" MultiPageID="RadMultiPage1" SelectedIndex="0" CausesValidation="false" OnClientTabSelected="OnClientTabSelectedHandler" >
        <Tabs>
            <radts:Tab ID="Tab1" Text="Application Package" Value="IsAppPackage" runat="server"></radts:Tab>
            <radTS:Tab ID="Tab2" Text="Closing Package" Value="IsClosingPackage" runat="server"></radts:Tab>
            <radTS:Tab ID="Tab3" Text="Misc Package" Value="IsMiscPackage" runat="server"></radts:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    </div>    
    </radspl:RadPane>
   <radspl:radsplitbar id="RadSplitBar2" runat="server" CollapseMode="None" EnableResize="false" Visible="false"/>
   <radspl:RadPane id="MiddlePane" runat="server" Scrolling="Y">
    <div RadResizeStopLookup="true" RadShowStopLookup="true" >
    <radTS:RadMultiPage id="RadMultiPage1" runat="server" SelectedIndex="0" >
    <radTS:PageView ID="pvAppPackage" runat="server">
        <div runat="server" id="divAppPackage" style="padding-top:10px;">
            <uc1:Package ID="ucAppPackage" PackageCBOnClickScript="AppPackageDTSelected();" PackageFileName="ApplicationPackage.pdf" Filter="IsAppPackage = 1" runat="server" />
        </div>
        <div style="text-align:center;vertical-align:bottom;padding-top:10px;">
            <asp:Button ID="btnPrintAppPack" Width="200px" runat="server" Text="Create new package" OnClick="btnPrintPack_Click" CssClass="button"/>
        </div>
    </radTS:PageView>
    <radTS:PageView ID="pvClosingPackage" runat="server">
        <div runat="server" id="divClosingPackage">
            <uc1:Package ID="ucClosingPackage" PackageCBOnClickScript="ClosingPackageDTSelected();" PackageFileName="ClosingPackage.pdf" Filter="IsClosingPackage = 1" runat="server" />
        </div>
        <div style="text-align:center;vertical-align:bottom;padding-top:10px;">
            <asp:Button ID="btnPrintClosingPack" Width="200px" runat="server" Text="Create new app package" OnClick="btnPrintPack_Click" CssClass="button"/>
        </div>
        
    </radTS:PageView>
    <radTS:PageView ID="pvMiscPackage" runat="server">
        <div runat="server" id="divMiscPackage">
            <uc1:Package ID="ucMiscPackage" PackageCBOnClickScript="MiscPackageDTSelected();" PackageFileName="MiscellaneousPackage.pdf" Filter="IsMiscPackage = 1" runat="server" />
        </div>
    </radTS:PageView>
</radTS:RadMultiPage>
    </div>
   </radspl:RadPane>
   <radspl:radsplitbar id="RadSplitBar1" runat="server" CollapseMode="None" EnableResize="false" Visible="false"/>
    <radspl:RadPane ID="BottomPane" runat="server" Scrolling="None" Height="30px" >
        <div RadResizeStopLookup="true" RadShowStopLookup="true"  style="text-align:center;vertical-align:text-bottom;">       
        <div style="text-align:center;vertical-align:bottom;">            
            <asp:Button ID="btnSendToClosingAgent" Width="150px" runat="server" Text="Send to Closing Agent" OnClick="btnSendToClosingAgent_Click" CssClass="button" />
        </div>
        </div>
    </radspl:RadPane>  
</radspl:RadSplitter>
