<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateMailFolder.aspx.cs" Inherits="LoanStarPortal.CreateMailFolder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>New mail folder</title>
</head>
<body>
    <form id="form1" runat="server">
    
<script type="text/javascript">
function GetRadWindow(){
    var o=null;
    if (window.radWindow) o = window.radWindow; 
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }

function Close()
{
    GetRadWindow().Close();
}    
function CloseAndRebind()
{
    GetRadWindow().Close();
    
    var emailsUniqueID = document.getElementById('<%=hfEmailsUniqueID.ClientID %>').value;
    if (emailsUniqueID.length > 0)
        GetRadWindow().BrowserWindow.AjaxRequestWithTarget(emailsUniqueID, 'RefreshStatusList');
    else
        GetRadWindow().BrowserWindow.RefreshPage();
}
</script>
    
    <asp:HiddenField ID="hfEmailsUniqueID" runat="server" />
    <asp:HiddenField ID="hfEmailsUserID" runat="server" />
    
    <div style="padding:0px;height:90px;">
        <table style="width:100%;" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="lblFolderNameErr" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr style="padding-top:0px;">
                <td align="center">
                    <asp:TextBox ID="tbFolderName" runat="server" MaxLength="50" Width="90%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFolderName" runat="server" ErrorMessage="*" ControlToValidate="tbFolderName"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr style="padding-top:10px;">
                <td align="left">
                    <asp:Button ID="btnOk" runat="server" Text="OK" Width="60px" OnClick="btnOk_Click"/>&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="60px" OnClick="btnCancel_Click" CausesValidation="false" />
                </td>
            </tr>
        </table>        
    </div>
    </form>
</body>
</html>
