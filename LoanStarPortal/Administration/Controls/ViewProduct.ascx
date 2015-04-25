<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewProduct.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewProduct" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>
<script type="text/javascript">

function UpdateRates(obj,id,id1){
    var o = document.getElementById(id);
    if(o!=null){
        o.value='Now updating...';
        o.setAttribute('disabled','disabled');
        var o1 = document.getElementById(id1);
        if(o1!=null) o1.setAttribute('disabled','disabled');        
        SendRequest(id,obj,id1);
    }
}
function SendRequest(id,req,id1){
    if (req.length==0) return;
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
	url = url.toLowerCase().replace('default.aspx','DataUpdates.aspx');
	url+='?obj='+req;
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
			    ProcessResponse(id,resp,id1,req);
			    xmlhhtp=null;
			}else if(status==500){
			    xmlhttp=null;
            	TimeOutHandler(id,req,id1);
			}else{
			    alert(statusText);
			}
		}
	}
	xmlhttp.send(null);	    
}
function RefreshContent(){
}
var _id;
var _req;
var _id1;
function TimeOutHandler(id,req,id1){
    _id=id;_req='check'+req;_id1=id1;
    window.setTimeout('CheckUpdateStatus()',500);    
}
function CheckUpdateStatus(){
    SendRequest(_id,_req,_id1);
}
function GetXmlHttp(){
		if(window.ActiveXObject){
			try {
				return new ActiveXObject("Microsoft.XMLHTTP");
			}catch(e){
				return null;
			}
		}else if (window.XMLHttpRequest){
			return new XMLHttpRequest();
		}
		return null;
}
function ProcessResponse(id,data,id1,req){
    var xmlDoc=null;
    var err = '';
    var result= '';
    var arr = null;
    if (navigator.userAgent.toLowerCase().indexOf('msie')>0){
    	xmlDoc=new ActiveXObject("Microsoft.XMLDOM");
		xmlDoc.loadXML(data);
		arr = xmlDoc.documentElement.attributes;
    }else{
        var parser=new DOMParser();
        xmlDoc=parser.parseFromString(data,"text/xml");
        arr = xmlDoc.documentElement.attributes;        
        parser=null;
    }
	xmlDoc=null;    
	result = GetAttribute(arr,'result');
	if(result=='2'){
	    TimeOutHandler(id,req,id1);return;
	}
    var o=document.getElementById(id);
    if(o!=null){
        o.value='Update Now';
        o.removeAttribute('disabled');
        var o1=document.getElementById(id1);
        if(o1!=null) o1.removeAttribute('disabled');
    }	
	if (result == '0'){		
        RefreshContent();
    }
}
function RefreshContent(){
}
function GetAttribute(arr,name){
    for(var i=0;i<arr.length;i++){
        if(arr[i].name==name){
            return arr[i].value;
        }
    }
}
</script>
<table width="100%" border="0" cellspacing="0" cellpadding="5">
	<!--tr>
	    <td valign="top" class="cssGridCtl">
	        <table border="0" cellpadding="0" cellspacing="0" width="100%">
	            <tr>
	                <td><asp:HyperLink Runat="server" ID="addLink" CssClass="cssLink" NavigateUrl="#">Add product</asp:HyperLink></td>
                </tr>
	        </table>
        </td>
    </tr-->
    <tr>
        <td>      

            <radG:RadGrid ID="G" runat="server" AllowPaging="False" AllowSorting="True" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" OnItemCommand="G_ItemCommand" PageSize="1000" >
            <ClientSettings>
            <Resizing AllowColumnResize="True" EnableRealTimeResize="True" />
       <Scrolling AllowScroll="True" UseStaticHeaders="true" />
            </ClientSettings>
                <MasterTableView AllowNaturalSort="False" CommandItemDisplay="Bottom" CommandItemSettings-AddNewRecordText="Add new product" CommandItemSettings-RefreshText="">
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Product" SortExpression="name" >
                            <ItemTemplate>
                                <asp:Label ID="lblLogin" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Action" Resizable="False">
                            <ItemTemplate>
                             <asp:ImageButton  id="btnEdit" ImageUrl="~/images/btn_grd_edit.gif" CommandName="edit" AlternateText="Edit product" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>
                             <%--<asp:ImageButton  id="btnDelete" ImageUrl="~/images/btn_grd_delete.gif" CommandName="delete" AlternateText="Delete product" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />--%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="15px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </radG:GridTemplateColumn>                        
                    </Columns>
                </MasterTableView>
                <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
            </radG:RadGrid>
                        
	    </td>
    </tr>
    <tr>
        <td>
         
<!--                
                <asp:Button ID="btnRateUpdate" runat="server" Text="Update Now" OnClick="btnRateUpdate_Click" />
-->                
<!--                
                <h5>Lending limit</h5>
                Last time update: <asp:Label ID="lblLastLimitDate" runat="server"></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp;                
-->                 
<!--                
                <asp:Button ID="btnLimitUpdate" runat="server" Text="Update Now" OnClick="btnLimitUpdate_Click" />                
-->                
<!--
                <input id="btnLimitUpdate" type="button" value="Update Now" onclick="UpdateRates('limit','btnLimitUpdate','btnRateUpdate');" />
                <asp:Button ID="btnViewLimits" runat="server" Text="View data" OnClick="btnViewLimits_Click" />
-->                
             <h5>Index rate update:</h5>
                <input id="btnRateUpdate" type="button" value="Update Now" onclick="UpdateRates('rate', 'btnRateUpdate', 'btnLimitUpdate');" />
                <h5>Update log</h5>
            <radG:RadGrid ID="gridLog" runat="server" AllowPaging="True" AllowSorting="False" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" PageSize="200">
            <ClientSettings>
                  <Scrolling AllowScroll="True" UseStaticHeaders="true" />
            <Resizing AllowColumnResize="True" EnableRealTimeResize="True"/>
            </ClientSettings>
                <MasterTableView AllowNaturalSort="False">
                    <Columns>
                        <radG:GridBoundColumn DataField="Created" HeaderText="Created"></radG:GridBoundColumn>
                        <radG:GridBoundColumn DataField="EntryType" HeaderText="Type"></radG:GridBoundColumn>
                        <radG:GridBoundColumn DataField="Message" HeaderText="Message"></radG:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
            </radG:RadGrid>            
        </td>
    </tr>
</table>
    