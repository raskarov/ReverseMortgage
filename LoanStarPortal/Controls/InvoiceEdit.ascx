<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceEdit.ascx.cs" Inherits="LoanStarPortal.Controls.InvoiceEdit"%>
<%@ Register Namespace="Telerik.WebControls" TagPrefix="radI" Assembly="RadInput.NET2"%>
<script language="javascript" type="text/javascript">
<!--
function ShowOrder(url){
    window.open(url);
}
function OrderMessage(msg){
    alert(msg);
}
function CheckAmountFinanced(){
    if(amountTotal<0){
        alert("Financed amount can't be less then 0");
        return false;
    }    
    return true;
}
function CalculateTwoTierUnit(o,ids,m,m1){
    var i = o.id.lastIndexOf('_text');
    var n=1;
    if((i>0)&&((i+5)==o.id.length)){
        n=2;
    }
    var id=GetClearId(o.id,n); 
    if(id!=''){
        var a=ids.split('&');
        var a1=GetControlsIds(id,a[0]);
        var a2=GetControlsIds(id,a[1]);
        var a3=GetControlsIds(id,a[2]);
        var av1=GetSelectValues(a1);
        var av2=GetRCTAmounts(a2);
        var t=CalculateTwoTier(av1,av2,m,m1);
        SetAmountLabel(a3[0],t);
        if(av2[5]!='') t=t-av2[5];
        if(av2[6]!='') t=t-av2[6];
        if(av2[7]!='') t=t-av2[7];
        SetAmountTotal(a3[1],t);
    }
}
function CalculateTwoTier(av1,av2,m,m1){
    var t=m;
    if(av1[0]==2){
        t*=1.5;
    }else if(av1[0]==3){
        t=m1;
    }
    
    var u1=av2[0];
    var p1=av2[1];
    var t1=av2[2];
    var u2=av2[3];
    var p2=av2[4];
    if((u1!='')&&(u1!=0)&&(p1!='')&&(p1!=0)&&(t1!='')&&(t1!=0)){
        if(t<t1){
            t1=t;
        }
        t=t-t1;
        r = GetTierUnitFee(t1,p1,u1);
        if(t>0){
            if((u2!='')&&(u2!=0)&&(p2!='')&&(p2!=0)){
                r += GetTierUnitFee(t,p2,u2);
            }
        }
    }else{
        r = 0;
    }    
    return r;
}
function GetTierUnitFee(t,p,u){
    var r=0;
    if((t>0)&&(p>0)&&(u>0)){
        r=(t/p)*u;
    }
    return r;
}
function CalculatePercentage(o,ids,m,m1){
    var i = o.id.lastIndexOf('_text');
    var n=1;
    if((i>0)&&((i+5)==o.id.length)){
        n=2;
    }
    var id=GetClearId(o.id,n); 
    if(id!=''){
        var a=ids.split('&');
        var a1=GetControlsIds(id,a[0]);
        var a2=GetControlsIds(id,a[1]);
        var a3=GetControlsIds(id,a[2]);
        var av1=GetSelectValues(a1);
        var av2=GetRCTAmounts(a2);
        var t=CalculateRowForPercentage(av1,av2,m,m1);
        SetAmountLabel(a3[0],t);
        if (av2[1]!='') t=t-av2[1];
        if (av2[2]!='') t=t-av2[2];
        if (av2[3]!='') t=t-av2[3];
        SetAmountTotal(a3[1],t);
    }    
}
function CalculateRowForPercentage(av1,av2,m,m1){
    var r=m;    
    var p=av2[0];
    if(av1[0]==2){
        r*=1.5;
    }else if(av1[0]==3){
        r=m1;
    }
    if((p=='')||(p==0)){
        r = 0;
    }else{
        r = (r * p)/100.0;
    }
    return r;
}
function CalculatePerUnit(o,ids,m,m1){
    var i = o.id.lastIndexOf('_text');
    var n=1;
    if((i>0)&&((i+5)==o.id.length)){
        n=2;
    }
    var id=GetClearId(o.id,n); 
    if(id!=''){
        var a=ids.split('&');
        var a1=GetControlsIds(id,a[0]);
        var a2=GetControlsIds(id,a[1]);
        var a3=GetControlsIds(id,a[2]);
        var av1=GetSelectValues(a1);
        var av2=GetRCTAmounts(a2);
        var t=CalculateRowForPerUnit(av1,av2,m,m1);
        SetAmountLabel(a3[0],t);
        if (av2[2]!='') t=t-av2[2];
        if (av2[3]!='') t=t-av2[3];
        if (av2[4]!='') t=t-av2[4];
        SetAmountTotal(a3[1],t);
    }    
}
function CalculateRowForPerUnit(av1,av2,m,m1){
    var r=m;
    var u=av2[1];
    var pu=av2[0];
    if(av1[0]==2){
        r*=1.5;
    }else if(av1[0]==3){
        r=m1;
    }    
    if((pu=='')||(u=='')||(pu==0)||(u==0)){
        r = 0;
    }else{
        if(av1[1]==1){
            var k = Math.round(r/u);
            if(k*u<r){
                k++;
            }
            r=k*pu;
        }else{
            r=Math.round(r/u)*pu;
        }
    }
    return r;
}
function GetSelectValues(a){
    var r=new Array(a.length);
    for(var i=0;i<a.length;i++){
        var o=document.getElementById(a[i]);
        r[i]=o.value;
    }
    return r;
}
function ValidateOriginatorFee(src,arg){
    var o = document.getElementById(src.id);
    o=document.getElementById(o.getAttribute('controltovalidate')+'_text');    
    var cb = GetCheckBox(src.id);    
    var id= GetRMTBId(o.id);
    var a = GetAmountValue(id);
    var a1 = o.getAttribute('maxallowed');
    if(a*1>a1*1){
        alert('Sorry, you cannot input an origination fee greater than the maximum allowable fee.');
        arg.IsValid = false;
        var r =  window[id];
        r.SetValue(a1);
        r.Disable();
        if(cb!=null){       
            cb.click();
        }
     }else{
        arg.IsValid = true;
    }    
}
function GetCheckBox(id){
    var i=id.lastIndexOf('_');
    if(i<0) return null;
    id=id.substring(0,i)+'_cbCalculateFee';
    return document.getElementById(id);
}
/*
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
*/
function AmountGovernmentFocusLost(o,t){
    var id =GetRMTBId(o.id)
    id=id.substring(0,id.length-1);
    var a3 = GetAmountValue(id+'3');
    var a4 = GetAmountValue(id+'4');
    var a2 = GetAmountValue(id+'2');
    var a6 = GetAmountValue(id+'6');
    var a7 = GetAmountValue(id+'7');    
    var v=0;
    if(t=='1'){
        var a5 = GetAmountValue(id+'5');
        v=a3+a4+a5;
    }else{
        v=a3+a4;
    }
    v -= (a2+a6+a7);
    var i=id.lastIndexOf('_');
    if(i>0){
        SetAmountTotal(id.substring(o,i)+'_lblFinancedAmountValue',v);
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
        SetAmountTotal(id.substring(o,i)+'_lblFinancedAmountValue',v);
    }
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
