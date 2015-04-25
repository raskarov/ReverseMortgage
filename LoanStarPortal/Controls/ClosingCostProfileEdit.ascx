<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClosingCostProfileEdit.ascx.cs" Inherits="LoanStarPortal.Controls.ClosingCostProfileEdit" %>
<%@ Register Namespace="Telerik.WebControls" TagPrefix="radI" Assembly="RadInput.NET2"%>
<script language="javascript" type="text/javascript">
<!--
function ValidatePocAmount1(src,arg){
    var o = document.getElementById(src.id);
    o=document.getElementById(o.getAttribute('controltovalidate')+'_text');
    var id =GetRMTBId(o.id)
    id=id.substring(0,id.length-1);
    var a1 = GetAmountValue(id+'1');
    var a2 = GetAmountValue(id+'2');
    arg.IsValid = a1>=a2;
}
function ValidatePocAmount2(src,arg){
    var o = document.getElementById(src.id);
    o=document.getElementById(o.getAttribute('controltovalidate')+'_text');
    var id =GetRMTBId(o.id)
    id=id.substring(0,id.length-1);
    var a2 = GetAmountValue(id+'2');
    var a3 = GetAmountValue(id+'3');
    var a4 = GetAmountValue(id+'4');
    var a5 = GetAmountValue(id+'5');
    arg.IsValid = (a3+a4+a5)>=a2;
}
function AmountGovernmentFocusLost(o,t){
    var id =GetRMTBId(o.id)
    id=id.substring(0,id.length-1);
    var a3 = GetAmountValue(id+'3');
    var a4 = GetAmountValue(id+'4');
    var a2 = GetAmountValue(id+'2');
    var v=0;
    if(t=='1'){
        var a5 = GetAmountValue(id+'5');
        v=a3+a4+a5-a2;
    }else{
        v=a3+a4-a2;
    }    
    var i=id.lastIndexOf('_');
    if(i>0){
        var d=document.getElementById(id.substring(o,i)+'_lblFinancedAmountValue');
        d.innerText='$'+v;
    }
}
function AmountFocusLost(o){
    var id =GetRMTBId(o.id)
    id=id.substring(0,id.length-1);
    var a1 = GetAmountValue(id+'1');
    var a2 = GetAmountValue(id+'2');
    var a6 = GetAmountValue(id+'6');
    var a7 = GetAmountValue(id+'7');
    var v=a1-a2-a6-a7;
    var i=id.lastIndexOf('_');
    if(i>0){
        var d=document.getElementById(id.substring(o,i)+'_lblFinancedAmountValue');
        d.innerText='$'+v;
    }
}
function GetAmountValue(id){
    var r = window[id];
    if(r==null) return 0;
    return r.GetValue();
}
function ValidateProvider(src,arg){
    var o = document.getElementById(src.id);
    o=document.getElementById(o.getAttribute('controltovalidate'));
    var p=o.parentElement;
    var s=o.getAttribute('disabled');
    if(s){
        arg.IsValid = true;
        return;
    }
    if(p.style.display=='block'){
        arg.IsValid = o.value!='';
    }else{
        arg.IsValid = true;
    }
}
function SetProvider(o,ddlid,tbid,dvid1,dvid2){
    var v = o.value;
    var p=GetParent(o);
    var ddl=GetElement(p,ddlid,'select');
    var tb=GetElement(p,tbid,'input');
    var dv1=GetElement(p,dvid1,'div');
    var dv2=GetElement(p,dvid2,'div');    
    if(v==4){
        if(dv1) dv1.style.display='none';
        if(tb) tb.removeAttribute('disabled');
        if(dv2) dv2.style.display='block'; 
    }else if(v==1){
        if(dv1) dv1.style.display='block';     
        if(dv2) dv2.style.display='none';
        if(ddl) ddl.removeAttribute('disabled');
    }else{
        if(ddl) ddl.setAttribute('disabled','disabled');
        if(tb) tb.setAttribute('disabled','disabled');
    }
}
function ChechAutoCalculate(o,message){
    alert(message.replace(/-/g,"\n-"));
    return false;
}
function ProviderListClick(o,ddlid,tbid,dvid1,dvid2){
    var list=o.getElementsByTagName('input');
    var p=GetParent(o);
    if(p){
        var ddl=GetElement(p,ddlid,'select');
        var tb=GetElement(p,tbid,'input');
        var dv1=GetElement(p,dvid1,'div');
        var dv2=GetElement(p,dvid2,'div');
        for(var i=0;i<list.length;i++){
            if(list[i].type=='radio'){
                if(list[i].checked){
                    var v=list[i].value;
                    if(v==1){
                        if(ddl) ddl.removeAttribute('disabled');
                        if(dv1) dv1.style.display='block';      
                        if(tb) tb.setAttribute('disabled','disabled');
                        if(dv2)dv2.style.display='none';                                  
                    }else if((v==2)||(v==3)){
                        if(ddl) ddl.setAttribute('disabled','disabled');
                        if(tb) tb.setAttribute('disabled','disabled');
                    }else if(v==4){
                        if(ddl) ddl.setAttribute('disabled','disabled');
                        if(dv1) dv1.style.display='none';
                        if(tb) tb.removeAttribute('disabled');
                        if(dv2) dv2.style.display='block'; 
                    }
                    return;
                }            
            }        
        }
    }
}
function GetElement(o,id,tag){
    var list=o.getElementsByTagName(tag);
    var l=id.length;
    for(var i=0;i<list.length;i++){
        var s=list[i].id;
        s=s.substring(s.length-l);
        if(s==id){
            return list[i];
        }
    }
    return null;
}
function GetParent(o){
    var r=o;
    while(true){
        r=r.parentElement;
        if(!r) return null;
        if(r.tagName.toLowerCase()=='table') return r;
    }
}
-->
</script>
<table border="0" cellspacing="3" cellpadding="3" align="center">
    <tr>
        <td>
            <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true" ForeColor="red"></asp:Label>
        </td>
    </tr>
</table>
<table border="0" cellspacing="3" cellpadding="3" align="center" runat="server" id="Table1">
</table>
<table border="0" cellspacing="3" cellpadding="3" align="center" runat="server" id="tblFormula">
</table>
<table border="0" cellspacing="3" cellpadding="3" align="center">
    <tr>
        <td align="right" colspan="2">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="publicEditFormButton" CommandName="Update" OnClick="btnSave_Click" TabIndex="120"/>&nbsp;
        </td>
        <td colspan="2">
         &nbsp<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="publicEditFormButton" CommandName="Cancel" OnClick="btnCancel_Click" TabIndex="121"/>
        </td>
    </tr>
</table>