var RadTreeView_KeyboardHooked=false;
var RadTreeView_Active=null;
var RadTreeView_DragActive=null;
var RadTreeView_MouseMoveHooked=false;
var RadTreeView_MouseUpHooked=false;
var RadTreeView_MouseY=0;
var RadTreeViewGlobalFirstParam=null;
var RadTreeViewGlobalSecondParam=null;
var RadTreeViewGlobalThirdParam=null;
var RadTreeViewGlobalFourthParam=null;
var contextMenuToBeHidden=null;
var safariKeyDownFlag=true;
if(typeof (window.RadControlsNamespace)=="undefined"){
window.RadControlsNamespace=new Object();
}
RadControlsNamespace.AppendStyleSheet=function(_1,_2,_3){
if(!_3){
return;
}
if(!_1){
document.write("<"+"link"+" rel='stylesheet' type='text/css' href='"+_3+"' />");
}else{
var _4=document.createElement("LINK");
_4.rel="stylesheet";
_4.type="text/css";
_4.href=_3;
document.getElementById(_2+"StyleSheetHolder").appendChild(_4);
}
};
function RadTreeNode(){
this.Parent=null;
this.TreeView=null;
this.Nodes=new Array();
this.ID=null;
this.ClientID=null;
this.SignImage=null;
this.SignImageExpanded=null;
this.Image=0;
this.ImageExpanded=0;
this.Action=null;
this.Index=0;
this.Level=0;
this.Text=null;
this.Value=null;
this.Category=null;
this.NodeCss=null;
this.NodeCssOver=null;
this.NodeCssSelect=null;
this.ContextMenuName=null;
this.Enabled=true;
this.Expanded=false;
this.Checked=false;
this.Selected=false;
this.DragEnabled=1;
this.DropEnabled=1;
this.EditEnabled=1;
this.ExpandOnServer=0;
this.IsClientNode=0;
this.Attributes=new Array();
this.IsFetchingData=false;
this.CachedText="";
}
RadTreeNode.prototype.ScrollIntoView=function(){
var _5=this.TextElement();
var _6=document.getElementById(this.TreeView.Container);
_6.scrollTop=_5.offsetTop;
};
RadTreeNode.prototype.Next=function(){
var _7=(this.Parent!=null)?this.Parent.Nodes:this.TreeView.Nodes;
return (this.Index>=_7.length)?null:_7[this.Index+1];
};
RadTreeNode.prototype.Prev=function(){
var _8=(this.Parent!=null)?this.Parent.Nodes:this.TreeView.Nodes;
return (this.Index<=0)?null:_8[this.Index-1];
};
RadTreeNode.prototype.NextVisible=function(){
if(this.Expanded&&this.Nodes.length>0){
return this.Nodes[0];
}
if(this.Next()!=null){
return this.Next();
}
var _9=this;
while(_9.Parent!=null){
if(_9.Parent.Next()!=null){
return _9.Parent.Next();
}
_9=_9.Parent;
}
return null;
};
RadTreeNode.prototype.LastVisibleChild=function(_a){
var _b=_a.Nodes;
var _c=_b.length;
var _d=_b[_c-1];
var _e=_d;
if(_d.Expanded&&_d.Nodes.length>0){
_e=this.LastVisibleChild(_d);
}
return _e;
};
RadTreeNode.prototype.PrevVisible=function(){
var _f=this.Prev();
if(_f!=null){
if(_f.Expanded&&_f.Nodes.length>0){
return this.LastVisibleChild(_f);
}
return this.Prev();
}
if(this.Parent!=null){
return this.Parent;
}
return null;
};
RadTreeNode.prototype.Toggle=function(){
if(this.Enabled){
if(this.TreeView.FireEvent(this.TreeView.BeforeClientToggle,this)==false){
return;
}
(this.Expanded)?this.Collapse():this.Expand();
if(this.ExpandOnServer!=2){
this.TreeView.FireEvent(this.TreeView.AfterClientToggle,this);
}
}
};
RadTreeNode.prototype.CollapseNonParentNodes=function(){
for(var i=0;i<this.TreeView.AllNodes.length;i++){
if(this.TreeView.AllNodes[i].Expanded&&!this.IsParent(this.TreeView.AllNodes[i])){
this.TreeView.AllNodes[i].CollapseNoEffect();
}
}
};
RadTreeNode.prototype.EncodeURI=function(s){
try{
return encodeURIComponent(s);
}
catch(e){
return escape(s);
}
};
RadTreeNode.prototype.RaiseNoTreeViewOnServer=function(){
throw new Error("No RadTreeView instance has been created on the server.\n"+"Make sure that you have the control instance created.\n"+"Please review this article for additional information.");
};
RadTreeNode.prototype.FetchDataOnDemand=function(){
if(this.Checked==1){
this.Checked=true;
}
var url=this.TreeView.LoadOnDemandUrl+"&rtnClientID="+this.ClientID+"&rtnLevel="+this.Level+"&rtnID="+this.ID+"&rtnParentPosition="+this.GetParentPositions()+"&rtnText="+this.EncodeURI(this.Text)+"&rtnValue="+this.EncodeURI(this.Value)+"&rtnCategory="+this.EncodeURI(this.Category)+"&rtnChecked="+this.Checked;
var _13;
if(typeof (XMLHttpRequest)!="undefined"){
_13=new XMLHttpRequest();
}else{
_13=new ActiveXObject("Microsoft.XMLHTTP");
}
url=url+"&timeStamp="+encodeURIComponent((new Date()).getTime());
_13.open("GET",url,true);
_13.setRequestHeader("Content-Type","application/json; charset=utf-8");
var _14=this;
_13.onreadystatechange=function(){
if(_13.readyState!=4){
return;
}
var _15=_13.responseText;
if(_13.status==500){
alert("RadTreeView: Server error in the NodeExpand event handler, press ok to view the result.");
document.body.innerHTML=_15;
return;
}
var _16=_15.indexOf(",");
var _17=parseInt(_15.substring(0,_16));
var _18=_15.substring(_16+1,_17+_16+1);
var _19=_15.substring(_17+_16+1);
_14.LoadNodesOnDemand(_18,_13.status,url);
_14.ImageOn();
_14.SignOn();
_14.Expanded=true;
_14.ExpandOnServer=0;
var _1a=_14.TextElement().parentNode;
var _1b=_1a.parentNode;
switch(_14.TreeView.LoadingMessagePosition){
case 0:
case 1:
if(_1a.tagName=="A"){
_1a.firstChild.innerHTML=_14.CachedText;
}else{
_1b=_14.TextElement().parentNode;
if(_14.TextElement().innerText){
_14.TextElement().innerHTML=_14.CachedText;
}else{
_14.TextElement().innerHTML=_14.CachedText;
}
}
break;
case 2:
_1a.removeChild(document.getElementById(_14.ClientID+"Loading"));
_1b=_14.TextElement().parentNode;
break;
case 3:
_1b=_14.TextElement().parentNode;
}
if(_14.Nodes.length>0){
rtvInsertHTML(_1b,_19);
var _1c=_1b.getElementsByTagName("IMG");
for(var i=0;i<_1c.length;i++){
RadTreeView.AlignImage(_1c[i]);
}
var _1e=_1b.getElementsByTagName("INPUT");
for(var i=0;i<_1e.length;i++){
RadTreeView.AlignImage(_1e[i]);
}
}
_14.IsFetchingData=false;
_14.TreeView.FireEvent(_14.TreeView.AfterClientToggle,_14);
};
_13.send(null);
};
RadTreeNode.prototype.Expand=function(){
if(this.ExpandOnServer){
if(!this.TreeView.FireEvent(this.TreeView.BeforeClientToggle,this)){
return;
}
if(this.ExpandOnServer==1){
this.TreeView.PostBack("NodeExpand",this.ClientID);
return;
}
if(this.ExpandOnServer==2){
if(!this.IsFetchingData){
this.IsFetchingData=true;
this.CachedText=this.TextElement().innerHTML;
switch(this.TreeView.LoadingMessagePosition){
case 0:
this.TextElement().innerHTML="<span class="+this.TreeView.LoadingMessageCssClass+">"+this.TreeView.LoadingMessage+"</span> "+this.TextElement().innerHTML;
break;
case 1:
this.TextElement().innerHTML=this.TextElement().innerHTML+" "+"<span class="+this.TreeView.LoadingMessageCssClass+">"+this.TreeView.LoadingMessage+"</span> ";
break;
case 2:
rtvInsertHTML(this.TextElement().parentNode,"<div id="+this.ClientID+"Loading "+" class="+this.TreeView.LoadingMessageCssClass+">"+this.TreeView.LoadingMessage+"</div>");
break;
}
var _1f=this;
window.setTimeout(function(){
_1f.FetchDataOnDemand();
},20);
return;
}
}
}
if(!this.Nodes.length){
return;
}
if(this.TreeView.SingleExpandPath){
this.CollapseNonParentNodes();
}
var _20=document.getElementById("G"+this.ClientID);
if(this.TreeView.ExpandDelay>0){
_20.style.overflow="hidden";
_20.style.height="1px";
_20.style.display="block";
_20.firstChild.style.position="relative";
window.setTimeout("rtvNodeExpand(1,'"+_20.id+"',"+this.TreeView.ExpandDelay+");",20);
}else{
_20.style.display="block";
}
this.ImageOn();
this.SignOn();
this.Expanded=true;
if(!this.IsClientNode){
this.TreeView.UpdateExpandedState();
}
};
RadTreeNode.prototype.GetParentPositions=function(){
var _21=this;
var _22="";
while(_21!=null){
if(_21.Next()!=null){
_22=_22+"1";
}else{
_22=_22+"0";
}
_21=_21.Parent;
}
return _22;
};
RadTreeNode.prototype.Collapse=function(){
if(this.Nodes.length>0){
if(!this.TreeView.FireEvent(this.TreeView.BeforeClientToggle,this)){
return;
}
if(this.ExpandOnServer==1&&this.TreeView.NodeCollapseWired){
this.TreeView.PostBack("NodeCollapse",this.ClientID);
return;
}
if(this.TreeView.ExpandDelay>0){
var _23=document.getElementById("G"+this.ClientID);
if(_23.scrollHeight!="undefined"){
_23.style.overflow="hidden";
_23.style.display="block";
_23.firstChild.style.position="relative";
window.setTimeout("rtvNodeCollapse("+_23.scrollHeight+",'"+_23.id+"',"+this.TreeView.ExpandDelay+" );",20);
}else{
this.CollapseNoEffect();
}
}else{
this.CollapseNoEffect();
}
this.ImageOff();
this.SignOff();
this.Expanded=false;
this.TreeView.UpdateExpandedState();
}
};
RadTreeNode.prototype.CollapseNoEffect=function(){
if(this.Nodes.length>0){
var _24=document.getElementById("G"+this.ClientID);
_24.style.display="none";
this.ImageOff();
this.SignOff();
this.Expanded=false;
this.TreeView.UpdateExpandedState();
}
};
RadTreeNode.prototype.Highlight=function(e){
if(!this.Enabled){
return;
}
if(e){
if(this.TreeView.MultipleSelect&&(e.ctrlKey||e.shiftKey)){
if(this.Selected){
this.TextElement().className=this.NodeCss;
this.Selected=false;
if(this.TreeView.SelectedNode==this){
this.TreeView.SelectedNode=null;
}
this.TreeView.UpdateSelectedState();
return;
}
}else{
this.TreeView.UnSelectAllNodes();
}
}
this.TextElement().className=this.NodeCssSelect;
this.TreeView.SelectNode(this);
this.TreeView.FireEvent(this.TreeView.AfterClientHighlight,this);
};
RadTreeNode.prototype.ExecuteAction=function(e){
if(this.IsClientNode){
return;
}
if(this.TextElement().tagName=="A"){
this.TextElement().click();
}else{
if(this.Action){
this.TreeView.PostBack("NodeClick",this.ClientID);
}
}
if(e){
(document.all)?e.returnValue=false:e.preventDefault();
}
};
RadTreeNode.prototype.Select=function(e){
if(this.TreeView.FireEvent(this.TreeView.BeforeClientClick,this,e)==false){
return;
}
if(this.Enabled){
this.Highlight(e);
this.TreeView.LastHighlighted=this;
this.ExecuteAction();
}else{
(document.all)?e.returnValue=false:e.preventDefault();
}
this.TreeView.FireEvent(this.TreeView.AfterClientClick,this,e);
};
RadTreeNode.prototype.UnSelect=function(){
if(this.TextElement().parentNode&&this.TextElement().parentNode.tagName=="A"){
this.TextElement().parentNode.className=this.NodeCss;
}
this.TextElement().className=this.NodeCss;
this.Selected=false;
};
RadTreeNode.prototype.Disable=function(){
this.TextElement().className=this.TreeView.NodeCssDisable;
this.Enabled=false;
this.Selected=false;
if(this.CheckElement()!=null){
this.CheckElement().disabled=true;
}
};
RadTreeNode.prototype.Enable=function(){
this.TextElement().className=this.NodeCss;
this.Enabled=true;
if(this.CheckElement()!=null){
this.CheckElement().disabled=false;
}
};
RadTreeNode.prototype.Hover=function(e){
var _29=(e.srcElement)?e.srcElement:e.target;
if(this.TreeView.IsRootNodeTag(_29)){
this.TreeView.SetBorderOnDrag(this,_29,e);
return;
}
if(this.Enabled){
if(this.TreeView.FireEvent(this.TreeView.BeforeClientHighlight,this)==false){
return;
}
this.TreeView.LastHighlighted=this;
if(RadTreeView_DragActive!=null&&RadTreeView_DragActive.DragClone!=null&&(!this.Expanded)&&this.ExpandOnServer!=1){
var _2a=this;
window.setTimeout(function(){
_2a.ExpandOnDrag();
},1000);
}
if(!this.Selected){
this.TextElement().className=this.NodeCssOver;
if(this.Image){
this.ImageElement().style.cursor="hand";
}
}
this.TreeView.FireEvent(this.TreeView.AfterClientHighlight,this);
}
};
RadTreeNode.prototype.UnHover=function(e){
var _2c=(e.srcElement)?e.srcElement:e.target;
if(this.TreeView.IsRootNodeTag(_2c)){
this.TreeView.ClearBorderOnDrag(_2c);
return;
}
if(this.Enabled){
this.TreeView.LastHighlighted=null;
if(!this.Selected){
this.TextElement().className=this.NodeCss;
if(this.Image){
this.ImageElement().style.cursor="default";
}
}
this.TreeView.FireEvent(this.TreeView.AfterClientMouseOut,this);
}
};
RadTreeNode.prototype.ExpandOnDrag=function(){
if(RadTreeView_DragActive!=null&&RadTreeView_DragActive.DragClone!=null&&(!this.Expanded)){
if(RadTreeView_Active.LastHighlighted==this){
this.Expand();
}
}
};
RadTreeNode.prototype.CheckBoxClick=function(e){
if(this.Enabled){
if(this.TreeView.FireEvent(this.TreeView.BeforeClientCheck,this,e)==false){
(this.Checked)?this.Check():this.UnCheck();
return;
}
(this.Checked)?this.UnCheck():this.Check();
if(this.TreeView.AutoPostBackOnCheck){
this.TreeView.PostBack("NodeCheck",this.ClientID);
this.TreeView.FireEvent(this.TreeView.AfterClientCheck,this);
return;
}
this.TreeView.FireEvent(this.TreeView.AfterClientCheck,this);
}
};
RadTreeNode.prototype.Check=function(){
if(this.CheckElement()!=null){
this.CheckElement().checked=true;
this.Checked=true;
this.TreeView.UpdateCheckedState();
}
};
RadTreeNode.prototype.UnCheck=function(){
if(this.CheckElement()!=null){
this.CheckElement().checked=false;
this.Checked=false;
this.TreeView.UpdateCheckedState();
}
};
RadTreeNode.prototype.IsSet=function(a){
return (a!=null&&a!="");
};
RadTreeNode.prototype.ImageOn=function(){
var _2f=document.getElementById(this.ClientID+"i");
if(this.ImageExpanded!=0){
_2f.src=this.ImageExpanded;
}
};
RadTreeNode.prototype.ImageOff=function(){
var _30=document.getElementById(this.ClientID+"i");
if(this.Image!=0){
_30.src=this.Image;
}
};
RadTreeNode.prototype.SignOn=function(){
var _31=document.getElementById(this.ClientID+"c");
if(this.IsSet(this.SignImageExpanded)){
_31.src=this.SignImageExpanded;
}
};
RadTreeNode.prototype.SignOff=function(){
var _32=document.getElementById(this.ClientID+"c");
if(this.IsSet(this.SignImage)){
_32.src=this.SignImage;
}
};
RadTreeNode.prototype.TextElement=function(){
var _33=document.getElementById(this.ClientID);
var _34=_33.getElementsByTagName("span")[0];
if(_34==null){
_34=_33.getElementsByTagName("a")[0];
}
return _34;
};
RadTreeNode.prototype.ImageElement=function(){
return document.getElementById(this.ClientID+"i");
};
RadTreeNode.prototype.CheckElement=function(){
return document.getElementById(this.ClientID).getElementsByTagName("input")[0];
};
RadTreeNode.prototype.IsParent=function(_35){
var _36=this.Parent;
while(_36!=null){
if(_35==_36){
return true;
}
_36=_36.Parent;
}
return false;
};
RadTreeNode.prototype.StartEdit=function(){
if(this.EditEnabled){
var _37=this.Text;
this.TreeView.EditMode=true;
var _38=this.TextElement().parentNode;
this.TreeView.EditTextElement=this.TextElement().cloneNode(true);
this.TextElement().parentNode.removeChild(this.TextElement());
var _39=this;
var _3a=document.createElement("input");
_3a.setAttribute("type","text");
_3a.setAttribute("size",this.Text.length+3);
_3a.setAttribute("value",_37);
_3a.className=this.TreeView.NodeCssEdit;
var _3b=this;
_3a.onblur=function(){
_3b.EndEdit();
};
_3a.onchange=function(){
_3b.EndEdit();
};
_3a.onkeypress=function(e){
_3b.AnalyzeEditKeypress(e);
};
_3a.onsubmit=function(){
return false;
};
_38.appendChild(_3a);
this.TreeView.EditInputElement=_3a;
_3a.focus();
_3a.onselectstart=function(e){
if(!e){
e=window.event;
}
if(e.stopPropagation){
e.stopPropagation();
}else{
e.cancelBubble=true;
}
};
var _3e=0;
var _3f=this.Text.length;
if(_3a.createTextRange){
var _40=_3a.createTextRange();
_40.moveStart("character",_3e);
_40.moveEnd("character",_3f);
_40.select();
}else{
_3a.setSelectionRange(_3e,_3f);
}
}
};
RadTreeNode.prototype.EndEdit=function(){
this.TreeView.EditInputElement.onblur=null;
this.TreeView.EditInputElement.onchange=null;
var _41=this.TreeView.EditInputElement.parentNode;
this.TreeView.EditInputElement.parentNode.removeChild(this.TreeView.EditInputElement);
_41.appendChild(this.TreeView.EditTextElement);
if(this.TreeView.FireEvent(this.TreeView.AfterClientEdit,this,this.Text,this.TreeView.EditInputElement.value)!=false){
if(this.Text!=this.TreeView.EditInputElement.value){
var _42=this.ClientID+":"+this.TreeView.EscapeParameter(this.TreeView.EditInputElement.value);
this.TreeView.PostBack("NodeEdit",_42);
return;
}
}
this.TreeView.EditMode=false;
this.TreeView.EditInputElement=null;
this.TreeView.EditTextElement=null;
};
RadTreeNode.prototype.AnalyzeEditKeypress=function(e){
if(document.all){
e=event;
}
if(e.keyCode==13){
(document.all)?e.returnValue=false:e.preventDefault();
if(typeof (e.cancelBubble)!="undefined"){
e.cancelBubble=true;
}
this.EndEdit();
return false;
}
if(e.keyCode==27){
this.TreeView.EditInputElement.value=this.TreeView.EditTextElement.innerHTML;
this.EndEdit();
}
return true;
};
RadTreeNode.prototype.LoadNodesOnDemand=function(s,_45,url){
if(_45==404){
var _47="CallBack URL not found: \n\r\n\r"+url+"\n\r\n\rAre you using URL Rewriter? Please, try setting the AjaxUrl property to match the correct URL you need";
alert(_47);
this.TreeView.FireEvent(this.TreeView.AfterClientCallBackError,this.TreeView);
}else{
try{
eval(s);
var _48=window[this.ClientID+"ClientData"];
for(var i=0;i<_48.length;i++){
var _4a=_48[i][0];
var _4b=_4a.substring(0,_4a.lastIndexOf("_t"));
var _4c=this.TreeView.FindNode(_4b);
if(_4c){
this.TreeView.LoadNode(_48[i],null,_4c);
}else{
_48[i][17]=0;
this.TreeView.LoadNode(_48[i],null,this);
}
}
}
catch(e){
this.TreeView.FireEvent(this.TreeView.AfterClientCallBackError,this.TreeView);
}
}
};
function RadTreeView(_4d){
if(window.tlrkTreeViews==null){
window.tlrkTreeViews=new Array();
}
if(window.tlrkTreeViews[_4d]!=null){
oldTreeView=window.tlrkTreeViews[_4d];
oldTreeView.Dispose();
}
tlrkTreeViews[_4d]=this;
this.Nodes=new Array();
this.AllNodes=new Array();
this.ClientID=null;
this.SelectedNode=null;
this.DragMode=false;
this.DragSource=null;
this.DragClone=null;
this.LastHighlighted=null;
this.MouseInside=false;
this.HtmlElementID="";
this.EditMode=false;
this.EditTextElement=null;
this.EditInputElement=null;
this.BeforeClientClick=null;
this.BeforeClientHighlight=null;
this.AfterClientHighlight=null;
this.AfterClientMouseOut=null;
this.BeforeClientDrop=null;
this.AfterClientDrop=null;
this.BeforeClientToggle=null;
this.AfterClientToggle=null;
this.BeforeClientContextClick=null;
this.BeforeClientContextMenu=null;
this.AfterClientContextClick=null;
this.BeforeClientCheck=null;
this.AfterClientCheck=null;
this.AfterClientMove=null;
this.AfterClientFocus=null;
this.BeforeClientDrag=null;
this.AfterClientEdit=null;
this.AfterClientClick=null;
this.BeforeClientDoubleClick=null;
this.AfterClientCallBackError=null;
this.DragAndDropBetweenNodes=false;
this.AutoPostBackOnCheck=false;
this.CausesValidation=true;
this.ContextMenuVisible=false;
this.ContextMenuName=null;
this.ContextMenuNode=null;
this.SingleExpandPath=false;
this.ExpandDelay=2;
this.TabIndex=0;
this.AllowNodeEditing=false;
this.LoadOnDemandUrl=null;
this.LoadingMessage="(loading ...)";
this.LoadingMessagePosition=0;
this.LoadingMessageCssClass="LoadingMessage";
this.NodeCollapseWired=false;
this.LastBorderElementSet=null;
this.LastDragPosition="on";
this.LastDragNode=null;
this.IsBuilt=false;
}
RadTreeView.AlignImage=function(_4e){
_4e.align="absmiddle";
_4e.style.display="inline";
if(!document.all||window.opera){
if(_4e.nextSibling&&_4e.nextSibling.tagName=="SPAN"){
_4e.nextSibling.style.verticalAlign="middle";
}
if(_4e.nextSibling&&_4e.nextSibling.tagName=="INPUT"){
_4e.nextSibling.style.verticalAlign="middle";
}
}
};
RadTreeView.prototype.OnInit=function(){
var _4f=new Array();
this.PreloadImages(_4f);
GlobalTreeViewImageList=_4f;
var _50=document.getElementById(this.Container).getElementsByTagName("IMG");
for(var i=0;i<_50.length;i++){
var _52=_50[i].className;
if(_52!=null&&_52!=""){
_50[i].src=_4f[_52];
RadTreeView.AlignImage(_50[i]);
}
}
this.LoadTree(_4f);
var _53=document.getElementById(this.Container).getElementsByTagName("INPUT");
for(var i=0;i<_53.length;i++){
RadTreeView.AlignImage(_53[i]);
}
if(document.addEventListener&&(!RadTreeView_KeyboardHooked)){
RadTreeView_KeyboardHooked=true;
document.addEventListener("keydown",this.KeyDownMozilla,false);
}
if((!RadTreeView_MouseMoveHooked)&&(this.DragAndDrop)){
RadTreeView_MouseMoveHooked=true;
if(document.attachEvent){
document.attachEvent("onmousemove",rtvMouseMove);
}
if(document.addEventListener){
document.addEventListener("mousemove",rtvMouseMove,false);
}
}
if(!RadTreeView_MouseUpHooked){
RadTreeView_MouseUpHooked=true;
if(document.attachEvent){
document.attachEvent("onmouseup",rtvMouseUp);
}
if(document.addEventListener){
document.addEventListener("mouseup",rtvMouseUp,false);
}
}
this.AttachAllEvents();
this.IsBuilt=true;
};
RadTreeView.prototype.AttachAllEvents=function(){
var _54=this;
var _55=document.getElementById(this.Container);
_55.onfocus=function(e){
rtvDispatcher(_54.ClientID,"focus",e);
};
_55.onmouseover=function(e){
rtvDispatcher(_54.ClientID,"mover",e);
};
_55.onmouseout=function(e){
rtvDispatcher(_54.ClientID,"mout",e);
};
_55.oncontextmenu=function(e){
rtvDispatcher(_54.ClientID,"context",e);
};
_55.onscroll=function(e){
_54.Scroll();
};
_55.onclick=function(e){
rtvDispatcher(_54.ClientID,"mclick",e);
};
_55.ondblclick=function(e){
rtvDispatcher(_54.ClientID,"mdclick",e);
};
_55.onkeydown=function(e){
rtvDispatcher(_54.ClientID,"keydown",e);
};
_55.onselectstart=function(){
return false;
};
_55.ondragstart=function(){
return false;
};
if(this.DragAndDrop){
_55.onmousedown=function(e){
rtvDispatcher(_54.ClientID,"mdown",e);
};
}
if(window.attachEvent){
window.attachEvent("onunload",function(){
_54.Dispose();
});
}
this.RootElement=_55;
};
RadTreeView.prototype.Dispose=function(){
if(this.disposed){
return;
}
this.disposed=true;
try{
var _5f=this.RootElement;
if(_5f!=null){
for(var _60 in _5f){
if(typeof (_5f[_60])=="function"){
_5f[_60]=null;
}
}
for(var _60 in this){
if(_60!="Dispose"){
this[_60]=null;
}
}
}
this.RootElement=null;
}
catch(err){
}
};
RadTreeView.prototype.PreloadImages=function(_61){
var _62=window[this.ClientID+"ImageData"];
for(var i=0;i<_62.length;i++){
_61[i]=_62[i];
}
};
RadTreeView.prototype.FindNode=function(_64){
for(var i=0;i<this.AllNodes.length;i++){
if(this.AllNodes[i].ClientID==_64){
return this.AllNodes[i];
}
}
return null;
};
RadTreeView.prototype.FindNodeByText=function(_66){
for(var i=0;i<this.AllNodes.length;i++){
if(this.AllNodes[i].Text==_66){
return this.AllNodes[i];
}
}
return null;
};
RadTreeView.prototype.FindNodeByValue=function(_68){
for(var i=0;i<this.AllNodes.length;i++){
if(this.AllNodes[i].Value==_68){
return this.AllNodes[i];
}
}
return null;
};
RadTreeView.prototype.IsChildOf=function(_6a,_6b){
if(_6b==_6a){
return false;
}
while(_6b&&(_6b!=document.body)){
if(_6b==_6a){
return true;
}
try{
_6b=_6b.parentNode;
}
catch(e){
return false;
}
}
return false;
};
RadTreeView.prototype.GetTarget=function(e){
if(!e){
return null;
}
return e.target||e.srcElement;
};
RadTreeView.prototype.LoadTree=function(_6d){
var cd=window[this.ClientID+"ClientData"];
for(var i=0;i<cd.length;i++){
this.LoadNode(cd[i],_6d);
}
};
RadTreeView.prototype.LoadNode=function(cd,_71,_72){
var _73=new RadTreeNode();
_73.ClientID=cd[0];
_73.TreeView=this;
var _74=cd[17];
if(_74>0){
_73.Parent=this.AllNodes[_74-1];
}
if(_72!=null){
_73.Parent=_72;
}
_73.NodeCss=this.NodeCss;
_73.NodeCssOver=this.NodeCssOver;
_73.NodeCssSelect=this.NodeCssSelect;
_73.Text=cd[1];
_73.Value=cd[2];
_73.Category=cd[3];
if(_71!=null){
_73.SignImage=_71[cd[4]];
_73.SignImageExpanded=_71[cd[5]];
}else{
_73.SignImage=GlobalTreeViewImageList[cd[4]];
_73.SignImageExpanded=GlobalTreeViewImageList[cd[5]];
}
if(cd[6]>0){
_73.Image=_71[cd[6]];
}
if(cd[7]>0){
_73.ImageExpanded=_71[cd[7]];
}
_73.Selected=cd[8];
if(_73.Selected){
this.SelectedNode=_73;
}
_73.Checked=cd[9];
_73.Enabled=cd[10];
_73.Expanded=cd[11];
_73.Action=cd[12];
if(this.IsSet(cd[13])){
_73.NodeCss=cd[13];
}
if(this.IsSet(cd[14])){
_73.ContextMenuName=cd[14];
}
this.AllNodes[this.AllNodes.length]=_73;
if(_73.Parent!=null){
_73.Parent.Nodes[_73.Parent.Nodes.length]=_73;
}else{
this.Nodes[this.Nodes.length]=_73;
}
_73.Index=cd[16];
_73.DragEnabled=cd[18];
_73.DropEnabled=cd[19];
_73.ExpandOnServer=cd[20];
if(this.IsSet(cd[21])){
_73.NodeCssOver=cd[21];
}
if(this.IsSet(cd[22])){
_73.NodeCssSelect=cd[22];
}
_73.Level=cd[23];
_73.ID=cd[24];
_73.IsClientNode=cd[25];
_73.EditEnabled=cd[26];
_73.Attributes=cd[27];
};
RadTreeView.prototype.Toggle=function(_75){
this.FindNode(_75).Toggle();
};
RadTreeView.prototype.Select=function(_76,e){
this.FindNode(_76).Select(e);
};
RadTreeView.prototype.Hover=function(_78,e){
var _78=this.FindNode(_78);
if(_78){
_78.Hover(e);
}
};
RadTreeView.prototype.UnHover=function(_7a,e){
var _7a=this.FindNode(_7a);
if(_7a){
_7a.UnHover(e);
}
};
RadTreeView.prototype.CheckBoxClick=function(_7c,e){
this.FindNode(_7c).CheckBoxClick(e);
};
RadTreeView.prototype.Highlight=function(_7e,e){
this.FindNode(_7e).Highlight(e);
};
RadTreeView.prototype.SelectNode=function(_80){
this.SelectedNode=_80;
_80.Selected=true;
this.UpdateSelectedState();
};
RadTreeView.prototype.GetSelectedNodes=function(){
var _81=new Array();
for(var i=0;i<this.AllNodes.length;i++){
if(this.AllNodes[i].Selected){
_81[_81.length]=this.AllNodes[i];
}
}
return _81;
};
RadTreeView.prototype.UnSelectAllNodes=function(_83){
for(var i=0;i<this.AllNodes.length;i++){
if(this.AllNodes[i].Selected&&this.AllNodes[i].Enabled){
this.AllNodes[i].UnSelect();
}
}
};
RadTreeView.prototype.KeyDownMozilla=function(e){
if(navigator.userAgent.toUpperCase().indexOf("SAFARI")!=-1&&e.keyCode!=32&&e.keyCode!=107&&e.keyCode!=109){
safariKeyDownFlag=!safariKeyDownFlag;
if(safariKeyDownFlag){
return;
}
}
var _86=RadTreeView_Active;
if(_86){
var _87=_86.GetTarget(e);
if(_87.tagName.toUpperCase()=="BODY"||_87.tagName.toUpperCase()=="HTML"||_86.IsChildOf(_87,_86.RootElement)||_87==_86.RootElement){
if(!_86.IsBuilt){
return;
}
if(_86.SelectedNode!=null){
if(_86.EditMode){
return;
}
if(e.keyCode==107||e.keyCode==109||e.keyCode==37||e.keyCode==39){
_86.SelectedNode.Toggle();
}
if(e.keyCode==40&&_86.SelectedNode.NextVisible()!=null){
_86.SelectedNode.NextVisible().Highlight(e);
}
if(e.keyCode==38&&_86.SelectedNode.PrevVisible()!=null){
_86.SelectedNode.PrevVisible().Highlight(e);
}
if(e.keyCode==13){
if(_86.FireEvent(_86.BeforeClientClick,_86.SelectedNode,e)==false){
return;
}
_86.SelectedNode.ExecuteAction();
_86.FireEvent(_86.AfterClientClick,_86.SelectedNode,e);
}
if(e.keyCode==32){
_86.SelectedNode.CheckBoxClick();
}
if(e.keyCode==113&&_86.AllowNodeEditing){
_86.SelectedNode.StartEdit();
}
}else{
if(e.keyCode==38||e.keyCode==40||e.keyCode==13||e.keyCode==32){
_86.Nodes[0].Highlight();
}
}
}
}
};
RadTreeView.prototype.KeyDown=function(e){
if(this.EditMode){
return;
}
var _89=this.SelectedNode;
if(_89!=null){
if(e.keyCode==107||e.keyCode==109||e.keyCode==37||e.keyCode==39){
_89.Toggle();
}
if(e.keyCode==40&&_89.NextVisible()!=null){
_89.NextVisible().Highlight(e);
}
if(e.keyCode==38&&_89.PrevVisible()!=null){
_89.PrevVisible().Highlight(e);
}
if(e.keyCode==13){
if(this.FireEvent(this.BeforeClientClick,this.SelectedNode,e)==false){
return;
}
_89.ExecuteAction(e);
this.FireEvent(this.AfterClientClick,this.SelectedNode,e);
}
if(e.keyCode==32){
_89.CheckBoxClick();
(document.all)?e.returnValue=false:e.preventDefault();
}
if(e.keyCode==113&&this.AllowNodeEditing){
_89.StartEdit();
}
}else{
if(e.keyCode==38||e.keyCode==40||e.keyCode==13||e.keyCode==32){
this.Nodes[0].Highlight();
}
}
};
RadTreeView.prototype.UpdateState=function(){
this.UpdateExpandedState();
this.UpdateCheckedState();
this.UpdateSelectedState();
};
RadTreeView.prototype.UpdateExpandedState=function(){
var _8a="";
for(var i=0;i<this.AllNodes.length;i++){
var _8c=(this.AllNodes[i].Expanded)?"1":"0";
_8a+=_8c;
}
document.getElementById(this.ClientID+"_expanded").value=_8a;
};
RadTreeView.prototype.UpdateCheckedState=function(){
var _8d="";
for(var i=0;i<this.AllNodes.length;i++){
var _8f=(this.AllNodes[i].Checked)?"1":"0";
_8d+=_8f;
}
document.getElementById(this.ClientID+"_checked").value=_8d;
};
RadTreeView.prototype.UpdateSelectedState=function(){
var _90="";
for(var i=0;i<this.AllNodes.length;i++){
var _92=(this.AllNodes[i].Selected)?"1":"0";
_90+=_92;
}
document.getElementById(this.ClientID+"_selected").value=_90;
};
RadTreeView.prototype.Scroll=function(){
document.getElementById(this.ClientID+"_scroll").value=document.getElementById(this.Container).scrollTop;
};
RadTreeView.prototype.ContextMenuClick=function(e,p1,p2,p3){
instance=this;
window.setTimeout(function(){
instance.HideContextMenu();
},10);
if(this.FireEvent(this.BeforeClientContextClick,this.ContextMenuNode,p1,p3)==false){
return;
}
if(p2){
var _97=this.ContextMenuNode.ClientID+":"+this.EscapeParameter(p1)+":"+this.EscapeParameter(p3);
this.PostBack("ContextMenuClick",_97);
}
};
RadTreeView.prototype.ContextMenu=function(e,_99){
var src=(e.srcElement)?e.srcElement:e.target;
var _9b=this.FindNode(_99);
if(_9b!=null&&this.BeforeClientContextMenu!=null){
var _9c=this.SelectedNode;
if(this.FireEvent(this.BeforeClientContextMenu,_9b,e,_9c)==false){
return;
}
this.Highlight(_99,e,_9c);
}
if(_9b!=null&&_9b.ContextMenuName!=null&&_9b.Enabled){
if(!this.ContextMenuVisible){
this.ContextMenuNode=_9b;
if(!_9b.Selected){
this.Highlight(_99,e);
}
this.ShowContextMenu(_9b.ContextMenuName,e);
document.all?e.returnValue=false:e.preventDefault();
}
}
};
RadTreeView.prototype.ShowContextMenu=function(_9d,e){
if(!document.readyState||document.readyState=="complete"){
var _9f="rtvcm"+this.ClientID+_9d;
var _a0=document.getElementById(_9f);
if(_a0){
var _a1=_a0.cloneNode(true);
_a1.id=_9f+"_clone";
document.body.appendChild(_a1);
_a1=document.getElementById(_9f+"_clone");
_a1.style.left=this.CalculateXPos(e)+"px";
_a1.style.top=this.CalculateYPos(e)+"px";
_a1.style.position="absolute";
_a1.style.display="block";
this.ContextMenuVisible=true;
this.ContextMenuName=_9d;
document.all?e.returnValue=false:e.preventDefault();
}
}
};
RadTreeView.prototype.CalculateYPos=function(e){
if(document.compatMode&&document.compatMode=="CSS1Compat"){
return (e.clientY+document.documentElement.scrollTop);
}
return (e.clientY+document.body.scrollTop);
};
RadTreeView.prototype.CalculateXPos=function(e){
if(document.compatMode&&document.compatMode=="CSS1Compat"){
return (e.clientX+document.documentElement.scrollLeft);
}
return (e.clientX+document.body.scrollLeft);
};
RadTreeView.prototype.HideContextMenu=function(){
if(!document.readyState||document.readyState=="complete"){
var _a4=document.getElementById("rtvcm"+this.ClientID+this.ContextMenuName+"_clone");
if(_a4){
document.body.removeChild(_a4);
}
this.ContextMenuVisible=false;
}
};
RadTreeView.prototype.MouseClickDispatcher=function(e){
var src=(e.srcElement)?e.srcElement:e.target;
var _a7=rtvGetNodeID(e);
if(_a7!=null&&src.tagName!="DIV"){
var _a8=this.FindNode(_a7);
if(_a8.Selected){
if(this.AllowNodeEditing){
_a8.StartEdit();
return;
}else{
this.Select(_a7,e);
}
}else{
this.Select(_a7,e);
}
}
if(src.tagName=="IMG"){
var _a9=src.className;
if(this.IsSet(_a9)&&this.IsToggleImage(_a9)){
this.Toggle(src.parentNode.id);
}
}
if(src.tagName=="INPUT"&&rtvInsideNode(src)){
this.CheckBoxClick(src.parentNode.id,e);
}
};
RadTreeView.prototype.IsToggleImage=function(n){
return (n==1||n==2||n==5||n==6||n==7||n==8||n==10||n==11);
};
RadTreeView.prototype.DoubleClickDispatcher=function(e,_ac){
var _ad=this.FindNode(_ac);
if(this.FireEvent(this.BeforeClientDoubleClick,_ad)==false){
return;
}
this.Toggle(_ac);
};
RadTreeView.prototype.MouseOverDispatcher=function(e,_af){
this.Hover(_af,e);
};
RadTreeView.prototype.MouseOutDispatcher=function(e,_b1){
this.UnHover(_b1,e);
this.LastDragNode=null;
this.LastHighlighted=null;
};
RadTreeView.prototype.MouseDown=function(e){
if(this.LastHighlighted!=null&&this.DragAndDrop){
if(this.FireEvent(this.BeforeClientDrag,this.LastHighlighted)==false){
return;
}
if(!this.LastHighlighted.DragEnabled){
return;
}
if(e.button==2){
return;
}
this.DragSource=this.LastHighlighted;
this.DragClone=document.createElement("div");
document.body.appendChild(this.DragClone);
RadTreeView_DragActive=this;
var res="";
if(this.MultipleSelect&&(this.SelectedNodesCount()>1)){
for(var i=0;i<this.AllNodes.length;i++){
if(this.AllNodes[i].Selected){
if(this.AllNodes[i].Image){
var img=this.AllNodes[i].ImageElement();
var _b6=img.cloneNode(true);
this.DragClone.appendChild(_b6);
}
var _b7=this.AllNodes[i].TextElement().cloneNode(true);
_b7.className=this.AllNodes[i].NodeCss;
_b7.style.color="gray";
this.DragClone.appendChild(_b7);
this.DragClone.appendChild(document.createElement("BR"));
}
res=res+"text";
}
}
if(res==""){
if(this.LastHighlighted.Image){
var img=this.LastHighlighted.ImageElement();
var _b6=img.cloneNode(true);
this.DragClone.appendChild(_b6);
}
var _b7=this.LastHighlighted.TextElement().cloneNode(true);
_b7.className=this.LastHighlighted.NodeCss;
_b7.style.color="gray";
this.DragClone.appendChild(_b7);
}
this.DragClone.style.position="absolute";
this.DragClone.style.display="none";
if(e.preventDefault){
e.preventDefault();
}
}
};
RadTreeView.prototype.SelectedNodesCount=function(){
var _b8=0;
for(var i=0;i<this.AllNodes.length;i++){
if(this.AllNodes[i].Selected){
_b8++;
}
}
return _b8;
};
RadTreeView.prototype.FireEvent=function(_ba,a,b,c,d){
if(!_ba){
return true;
}
RadTreeViewGlobalFirstParam=a;
RadTreeViewGlobalSecondParam=b;
RadTreeViewGlobalThirdParam=c;
RadTreeViewGlobalFourthParam=d;
var s=_ba+"(RadTreeViewGlobalFirstParam, RadTreeViewGlobalSecondParam, RadTreeViewGlobalThirdParam, RadTreeViewGlobalFourthParam);";
return eval(s);
};
RadTreeView.prototype.Focus=function(e){
this.FireEvent(this.AfterClientFocus,this);
};
RadTreeView.prototype.IsSet=function(a){
return (a!=null&&a!="");
};
RadTreeView.prototype.GetX=function(obj){
var _c3=0;
if(obj.offsetParent){
while(obj.offsetParent){
_c3+=obj.offsetLeft;
obj=obj.offsetParent;
}
}else{
if(obj.x){
_c3+=obj.x;
}
}
return _c3;
};
RadTreeView.prototype.GetY=function(obj){
var _c5=0;
if(obj.offsetParent){
while(obj.offsetParent){
_c5+=obj.offsetTop;
obj=obj.offsetParent;
}
}else{
if(obj.y){
_c5+=obj.y;
}
}
return _c5;
};
RadTreeView.prototype.PostBack=function(_c6,_c7){
var _c8=_c6+"#"+_c7;
if(this.PostBackOptionsClientString){
var _c9=this.PostBackOptionsClientString.replace(/@@arguments@@/g,_c8);
if(typeof (WebForm_PostBackOptions)!="undefined"||_c9.indexOf("_doPostBack")>-1||_c9.indexOf("AsyncRequest")>-1||_c9.indexOf("AsyncRequest")>-1||_c9.indexOf("AjaxNS")>-1){
eval(_c9);
}
}else{
if(this.CausesValidation){
if(!(typeof (Page_ClientValidate)!="function"||Page_ClientValidate())){
return;
}
}
var _ca=this.PostBackFunction.replace(/@@arguments@@/g,_c8);
eval(_ca);
}
};
RadTreeView.prototype.EscapeParameter=function(_cb){
var _cc=_cb.replace(/'/g,"&squote");
_cc=_cc.replace(/#/g,"&ssharp");
_cc=_cc.replace(/:/g,"&scolon");
_cc=_cc.replace(/\\/g,"\\\\");
return _cc;
};
RadTreeView.prototype.IsRootNodeTag=function(_cd){
if(_cd&&_cd.tagName=="DIV"&&_cd.id.indexOf(this.ID)>-1){
return true;
}
return false;
};
RadTreeView.prototype.SetBorderOnDrag=function(_ce,_cf,e){
if(this.DragAndDropBetweenNodes&&this.IsDragActive()){
this.LastDragNode=_ce;
var _d1=this.CalculateYPos(e);
var _d2=this.GetY(_cf);
if(_d1<_d2+_ce.TextElement().offsetHeight){
_cf.style.borderTop="1px dotted black";
this.LastDragPosition="above";
}else{
_cf.style.borderBottom="1px dotted black";
this.LastDragPosition="below";
}
this.LastBorderElementSet=_cf;
}
};
RadTreeView.prototype.ClearBorderOnDrag=function(_d3){
if(_d3&&this.DragAndDropBetweenNodes&&this.IsDragActive()){
_d3.style.borderTop="";
_d3.style.borderBottom="";
this.LastDragPosition="over";
}
};
RadTreeView.prototype.IsDragActive=function(){
for(var key in tlrkTreeViews){
if((typeof (tlrkTreeViews[key])!="function")&&tlrkTreeViews[key].DragClone!=null){
return true;
}
}
return false;
};
function rtvIsAnyContextMenuVisible(){
for(var key in tlrkTreeViews){
if((typeof (tlrkTreeViews[key])!="function")&&tlrkTreeViews[key].ContextMenuVisible){
return true;
}
}
return false;
}
function rtvAdjustScroll(){
if(RadTreeView_DragActive==null||RadTreeView_DragActive.DragClone==null||RadTreeView_Active==null){
return;
}
var _d6=RadTreeView_Active;
var _d7=document.getElementById(RadTreeView_Active.Container);
var _d8,_d9;
_d8=_d6.GetY(_d7);
_d9=_d8+_d7.offsetHeight;
if((RadTreeView_MouseY-_d8)<50&&_d7.scrollTop>0){
_d7.scrollTop=_d7.scrollTop-10;
_d6.Scroll();
RadTreeView_ScrollTimeout=window.setTimeout(function(){
rtvAdjustScroll();
},100);
}else{
if((_d9-RadTreeView_MouseY)<50&&_d7.scrollTop<(_d7.scrollHeight-_d7.offsetHeight+16)){
_d7.scrollTop=_d7.scrollTop+10;
_d6.Scroll();
RadTreeView_ScrollTimeout=window.setTimeout(function(){
rtvAdjustScroll();
},100);
}
}
}
function rtvMouseUp(e){
if(RadTreeView_Active==null){
return;
}
if(e&&!e.ctrlKey){
for(var key in tlrkTreeViews){
if((typeof (tlrkTreeViews[key])!="function")&&tlrkTreeViews[key].ContextMenuVisible){
contextMenuToBeHidden=tlrkTreeViews[key];
window.setTimeout(function(){
if(contextMenuToBeHidden){
contextMenuToBeHidden.HideContextMenu();
}
},10);
return;
}
}
}
if(RadTreeView_DragActive==null||RadTreeView_DragActive.DragClone==null){
return;
}
(document.all)?e.returnValue=false:e.preventDefault();
var _dc=RadTreeView_DragActive.DragSource;
var _dd=RadTreeView_Active.LastHighlighted;
var _de=RadTreeView_Active;
var _df="over";
var _e0;
if(_de.LastBorderElementSet){
_df=_de.LastDragPosition;
_e0=_de.LastDragNode;
_de.ClearBorderOnDrag(_de.LastBorderElementSet);
}
if(_e0){
_dd=_e0;
}
document.body.removeChild(RadTreeView_DragActive.DragClone);
RadTreeView_DragActive.DragClone=null;
if(_dd!=null&&_dd.DropEnabled==false){
return;
}
if(_dc==_dd){
return;
}
if(RadTreeView_DragActive.FireEvent(RadTreeView_DragActive.BeforeClientDrop,_dc,_dd,e,_df)==false){
return;
}
if(_dc.IsClientNode||((_dd!=null)&&_dd.IsClientNode)){
return;
}
var _e1=RadTreeView_DragActive.ClientID+"#"+_dc.ClientID+"#";
var _e2="";
if(_dd==null){
_e2="null"+"#"+RadTreeView_DragActive.HtmlElementID;
}else{
_e2=_de.ClientID+"#"+_dd.ClientID+"#"+_df;
}
if(_dd==null&&RadTreeView_DragActive.HtmlElementID==""){
return;
}
var _e3=_e1+_e2;
RadTreeView_DragActive.PostBack("NodeDrop",_e3);
RadTreeView_DragActive.FireEvent(RadTreeView_DragActive.AfterClientDrop,_dc,_dd,e);
RadTreeView_DragActive=null;
}
function rtvMouseMove(e){
if(rtvIsAnyContextMenuVisible()){
return;
}
if(RadTreeView_DragActive!=null&&RadTreeView_DragActive.DragClone!=null){
var _e5,_e6;
_e5=RadTreeView_DragActive.CalculateXPos(e)+8;
_e6=RadTreeView_DragActive.CalculateYPos(e)+4;
RadTreeView_MouseY=_e6;
rtvAdjustScroll();
RadTreeView_DragActive.DragClone.style.zIndex=999;
RadTreeView_DragActive.DragClone.style.top=_e6+"px";
RadTreeView_DragActive.DragClone.style.left=_e5+"px";
RadTreeView_DragActive.DragClone.style.display="block";
RadTreeView_DragActive.FireEvent(RadTreeView_DragActive.AfterClientMove,e);
}
}
function rtvNodeExpand(a,id,_e9){
var _ea=document.getElementById(id);
var _eb=_ea.scrollHeight;
var _ec=(_eb-a)/_e9;
var _ed=a+_ec;
if(_ed>_eb-1){
_ea.style.height="";
_ea.firstChild.style.position="";
}else{
_ea.style.height=_ed+"px";
window.setTimeout("rtvNodeExpand("+_ed+","+"'"+id+"',"+_e9+");",5);
}
}
function rtvNodeCollapse(a,id,_f0){
var _f1=document.getElementById(id);
var _f2=_f1.scrollHeight;
var _f3=(_f2-Math.abs(_f2-a))/_f0;
var _f4=a-_f3;
if(_f4<=3){
_f1.style.height="";
_f1.style.display="none";
_f1.firstChild.style.position="";
}else{
_f1.style.height=_f4+"px";
window.setTimeout("rtvNodeCollapse("+_f4+","+"'"+id+"',"+_f0+" );",5);
}
}
function rtvGetNodeID(e){
if(RadTreeView_Active==null){
return;
}
var _f6=(e.srcElement)?e.srcElement:e.target;
if(_f6.nodeType==3){
_f6=_f6.parentNode;
}
if(_f6.tagName=="IMG"&&_f6.nextSibling){
var _f7=_f6.className;
if(_f7){
_f7=parseInt(_f7);
if(_f7>12){
_f6=_f6.nextSibling;
}
}
}
if(_f6.id==RadTreeView_Active.ID){
return null;
}
if(_f6.id.indexOf(RadTreeView_Active.ID)>-1&&_f6.tagName=="DIV"){
return _f6.id;
}
while(_f6!=null){
if((_f6.tagName=="SPAN"||_f6.tagName=="A")&&rtvInsideNode(_f6)){
return _f6.parentNode.id;
}
_f6=_f6.parentNode;
}
return null;
}
function rtvInsideNode(_f8){
if(_f8.parentNode&&_f8.parentNode.tagName=="DIV"&&_f8.parentNode.id.indexOf(RadTreeView_Active.ID)>-1){
return _f8.parentNode.id;
}
}
function rtvDispatcher(t,w,e,p1,p2,p3){
if(!e){
e=window.event;
}
if(tlrkTreeViews){
var _ff=rtvGetNodeID(e);
var _100=tlrkTreeViews[t];
if(!_100.IsBuilt){
return;
}
if(rtvIsAnyContextMenuVisible()&&w!="mclick"&&w!="cclick"){
return;
}
if(_100.EditMode){
return;
}
RadTreeView_Active=_100;
var _101=window.netscape&&!window.opera;
var _102=(navigator.userAgent.toLowerCase().indexOf("safari")!=-1);
switch(w){
case "mover":
if(_ff!=null){
_100.MouseOverDispatcher(e,_ff);
}
break;
case "mout":
if(_ff!=null){
_100.MouseOutDispatcher(e,_ff);
}
break;
case "mclick":
_100.MouseClickDispatcher(e);
break;
case "mdclick":
if(_ff!=null){
_100.DoubleClickDispatcher(e,_ff);
}
break;
case "mdown":
_100.MouseDown(e);
break;
case "mup":
_100.MouseUp(e);
break;
case "context":
if(_ff!=null){
_100.ContextMenu(e,_ff);
return false;
}
break;
case "cclick":
_100.ContextMenuClick(e,p1,p2,p3);
break;
case "focus":
_100.Focus(e);
case "keydown":
if(!_101&&!_102){
_100.KeyDown(e);
}
}
}
}
function rtvAppendStyleSheet(_103,_104){
var _105=(navigator.appName=="Microsoft Internet Explorer")&&((navigator.userAgent.toLowerCase().indexOf("mac")!=-1)||(navigator.appVersion.toLowerCase().indexOf("mac")!=-1));
var _106=(navigator.userAgent.toLowerCase().indexOf("safari")!=-1);
if(_105||_106){
document.write("<"+"link"+" rel='stylesheet' type='text/css' href='"+_104+"'>");
}else{
var _107=document.createElement("LINK");
_107.rel="stylesheet";
_107.type="text/css";
_107.href=_104;
document.getElementById(_103+"StyleSheetHolder").appendChild(_107);
}
}
function rtvInsertHTML(_108,html){
if(_108.tagName=="A"){
_108=_108.parentNode;
}
if(document.all){
_108.insertAdjacentHTML("beforeEnd",html);
}else{
var r=_108.ownerDocument.createRange();
r.setStartBefore(_108);
var _10b=r.createContextualFragment(html);
_108.appendChild(_10b);
}
}

//BEGIN_ATLAS_NOTIFY
if (typeof(Sys) != "undefined"){if (Sys.Application != null && Sys.Application.notifyScriptLoaded != null){Sys.Application.notifyScriptLoaded();}}
//END_ATLAS_NOTIFY
