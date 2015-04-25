<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailAttachUpload.ascx.cs" Inherits="LoanStarPortal.Controls.EmailAttachUpload" %>

<script language="javascript" type="text/javascript" defer="defer">
var upload_number = 2;

function addFileInput() 
{
	var d = document.createElement("div");
	d.setAttribute("id", "f" + upload_number);

	var l = document.createElement("a");
	l.setAttribute("href", "javascript:removeFileInput('f" + upload_number + "');");
	l.appendChild(document.createTextNode("Remove"));

	var file = document.createElement("input");
	file.setAttribute("type", "file");
	file.setAttribute("name", "attachment" + upload_number);
	file.setAttribute("id", "attachment" + upload_number);
	file.setAttribute("accept", "image/gif,image/jpeg");

	d.appendChild(file);
	d.appendChild(l);
	document.getElementById("moreUploads").appendChild(d);

	upload_number++; 
}

function removeFileInput(i) 
{
	var elm = document.getElementById(i);
	document.getElementById("moreUploads").removeChild(elm);
	upload_number--; 
}

function ClickRemoveAttach(trAttachID)
{
    var trAttach = document.getElementById(trAttachID);
    trAttach.style.display = "none";
    
    var attachID = trAttach.getElementsByTagName("input")[0].value;
    trAttach.getElementsByTagName("input")[0].value = (-1) * attachID;
}
</script>

<div id="divEmailAttachments" runat="server" style="width:100%;">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <asp:Repeater ID="rpEmailAttachments" OnItemDataBound="rpEmailAttachments_ItemDataBound" runat="server">
            <ItemTemplate>
                <asp:Panel ID="pnlAttach" runat="server">
                    <tr id="trAttach" runat="server">
                        <td align="left" style="width:238px;">
                            <asp:Label ID="lblAttachName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'>
                            </asp:Label>
                        </td>
                        <td align="left">
                            <asp:HyperLink ID="hlRemoveAttach" runat="server" Text="Remove">
                            </asp:HyperLink>
                            <asp:HiddenField ID="hfAttachID" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                            <asp:HiddenField ID="hfAttachIsInline" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "IsInline") %>' />
                            <asp:HiddenField ID="hfAttachContentID" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "ContentId") %>' />
                            <asp:HiddenField ID="hfAttachName" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "Name") %>' />
                            <asp:HiddenField ID="hfAttachFileName" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "FileName") %>' />
                            <asp:HiddenField ID="hfAttachContentType" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "ContentTypeStr") %>' />
                            <asp:HiddenField ID="hfAttachTransferEncoding" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "TransferEncodingStr") %>' />
                            <asp:HiddenField ID="hfAttachNamePageCode" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "NamePageCodeStr") %>' />
                        </td>
                    </tr>
                    <tr id="trAttachDownload" runat="server">
                        <td align="left" colspan="2">
                            <asp:LinkButton ID="lbtnSaveAttach" runat="server" CssClass="linkButton" CommandName="SaveAttach" 
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' 
                                OnCommand="lbtnSaveAttach_Command" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'>
                            </asp:LinkButton>
                        </td>
                    </tr>
                </asp:Panel>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</div>

<div id="divUploadEmailAttachments" runat="server" style="width:100%;">
    <input type="file" name="attachment" id="attachment" accept="image/gif,image/jpeg" />
    <div id="moreUploads"></div>
    <div id="moreUploadsLink"><a href="javascript:addFileInput();">Attach another File</a></div>
</div>
