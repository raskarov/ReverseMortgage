if(typeof window.RadControlsNamespace=="undefined"){
window.RadControlsNamespace={};
}
if(typeof (window.RadControlsNamespace.DomEventMixin)=="undefined"||typeof (window.RadControlsNamespace.DomEventMixin.Version)==null||window.RadControlsNamespace.DomEventMixin.Version<2){
RadControlsNamespace.DomEventMixin={Version:2,Initialize:function(_1){
_1.CreateEventHandler=this.CreateEventHandler;
_1.AttachDomEvent=this.AttachDomEvent;
_1.DetachDomEvent=this.DetachDomEvent;
_1.DisposeDomEventHandlers=this.DisposeDomEventHandlers;
_1._domEventHandlingEnabled=true;
_1.EnableDomEventHandling=this.EnableDomEventHandling;
_1.DisableDomEventHandling=this.DisableDomEventHandling;
_1.RemoveHandlerRegister=this.RemoveHandlerRegister;
_1.GetHandlerRegister=this.GetHandlerRegister;
_1.AddHandlerRegister=this.AddHandlerRegister;
_1.handlerRegisters=[];
},EnableDomEventHandling:function(){
this._domEventHandlingEnabled=true;
},DisableDomEventHandling:function(){
this._domEventHandlingEnabled=false;
},CreateEventHandler:function(_2,_3){
var _4=this;
return function(e){
if(!_4._domEventHandlingEnabled&&!_3){
return false;
}
return _4[_2](e||window.event);
};
},AttachDomEvent:function(_6,_7,_8,_9){
var _a=this.CreateEventHandler(_8,_9);
var _b=this.GetHandlerRegister(_6,_7,_8);
if(_b!=null){
this.DetachDomEvent(_b.Element,_b.EventName,_8);
}
var _c={"Element":_6,"EventName":_7,"HandlerName":_8,"Handler":_a};
this.AddHandlerRegister(_c);
if(_6.addEventListener){
_6.addEventListener(_7,_a,false);
}else{
if(_6.attachEvent){
_6.attachEvent("on"+_7,_a);
}
}
},DetachDomEvent:function(_d,_e,_f){
var _10=null;
var _11="";
if(typeof _f=="string"){
_11=_f;
_10=this.GetHandlerRegister(_d,_e,_11);
if(_10==null){
return;
}
_f=_10.Handler;
}
if(!_d){
return;
}
if(_d.removeEventListener){
_d.removeEventListener(_e,_f,false);
}else{
if(_d.detachEvent){
_d.detachEvent("on"+_e,_f);
}
}
if(_10!=null&&_11!=""){
this.RemoveHandlerRegister(_10);
_10=null;
}
},DisposeDomEventHandlers:function(){
for(var i=0;i<this.handlerRegisters.length;i++){
var _13=this.handlerRegisters[i];
if(_13!=null){
this.DetachDomEvent(_13.Element,_13.EventName,_13.Handler);
}
}
this.handlerRegisters=[];
},RemoveHandlerRegister:function(_14){
try{
var _15=_14.index;
for(var i in _14){
_14[i]=null;
}
this.handlerRegisters[_15]=null;
}
catch(e){
}
},GetHandlerRegister:function(_17,_18,_19){
for(var i=0;i<this.handlerRegisters.length;i++){
var _1b=this.handlerRegisters[i];
if(_1b!=null&&_1b.Element==_17&&_1b.EventName==_18&&_1b.HandlerName==_19){
return this.handlerRegisters[i];
}
}
return null;
},AddHandlerRegister:function(_1c){
_1c.index=this.handlerRegisters.length;
this.handlerRegisters[this.handlerRegisters.length]=_1c;
}};
RadControlsNamespace.DomEvent={};
RadControlsNamespace.DomEvent.PreventDefault=function(e){
if(!e){
return true;
}
if(e.preventDefault){
e.preventDefault();
}
e.returnValue=false;
return false;
};
RadControlsNamespace.DomEvent.StopPropagation=function(e){
if(!e){
return;
}
if(e.stopPropagation){
e.stopPropagation();
}else{
e.cancelBubble=true;
}
};
RadControlsNamespace.DomEvent.GetTarget=function(e){
if(!e){
return null;
}
return e.target||e.srcElement;
};
RadControlsNamespace.DomEvent.GetRelatedTarget=function(e){
if(!e){
return null;
}
return e.relatedTarget||(e.type=="mouseout"?e.toElement:e.fromElement);
};
RadControlsNamespace.DomEvent.GetKeyCode=function(e){
if(!e){
return 0;
}
return e.which||e.keyCode;
};
}
var RadGridNamespace={};
RadGridNamespace.Prefix="grid_";
RadGridNamespace.InitializeClient=function(_22){
var _23=document.getElementById(_22+"AtlasCreation");
if(!_23){
return;
}
var _24=document.createElement("script");
if(navigator.userAgent.indexOf("Safari")!=-1){
_24.innerHTML=_23.innerHTML;
}else{
_24.text=_23.innerHTML;
}
document.body.appendChild(_24);
document.body.removeChild(_24);
_23.parentNode.removeChild(_23);
};
RadGridNamespace.AsyncRequest=function(_25,_26,_27){
var _28=window[_27];
if(_28!=null&&typeof (_28.AsyncRequest)=="function"){
_28.AsyncRequest(_25,_26);
}
};
RadGridNamespace.AsyncRequestWithOptions=function(_29,_2a){
var _2b=window[_2a];
if(_2b!=null&&typeof (_2b.AsyncRequestWithOptions)=="function"){
_2b.AsyncRequestWithOptions(_29);
}
};
RadGridNamespace.GetWidth=function(_2c){
var _2d;
if(window.getComputedStyle){
_2d=window.getComputedStyle(_2c,"").getPropertyValue("width");
}else{
if(_2c.currentStyle){
_2d=_2c.currentStyle.width;
}else{
_2d=_2c.offsetWidth;
}
}
if(_2d.toString().indexOf("%")!=-1){
_2d=_2c.offsetWidth;
}
if(_2d.toString().indexOf("px")!=-1){
_2d=parseInt(_2d);
}
return _2d;
};
RadGridNamespace.GetScrollBarWidth=function(){
try{
if(typeof (RadGridNamespace.scrollbarWidth)=="undefined"){
var _2e,_2f=0;
var _30=document.createElement("div");
_30.style.position="absolute";
_30.style.top="-1000px";
_30.style.left="-1000px";
_30.style.width="100px";
_30.style.overflow="auto";
var _31=document.createElement("div");
_31.style.width="1000px";
_30.appendChild(_31);
document.body.appendChild(_30);
_2e=_30.offsetWidth;
_2f=_30.clientWidth;
document.body.removeChild(document.body.lastChild);
RadGridNamespace.scrollbarWidth=_2e-_2f;
if(RadGridNamespace.scrollbarWidth<=0||_2f==0){
RadGridNamespace.scrollbarWidth=16;
}
}
return RadGridNamespace.scrollbarWidth;
}
catch(error){
return false;
}
};
RadGridNamespace.GetTableColGroup=function(_32){
try{
return _32.getElementsByTagName("colgroup")[0];
}
catch(error){
return false;
}
};
RadGridNamespace.GetTableColGroupCols=function(_33){
try{
var _34=new Array();
var _35=_33.childNodes[0];
for(var i=0;i<_33.childNodes.length;i++){
if((_33.childNodes[i].tagName)&&(_33.childNodes[i].tagName.toLowerCase()=="col")){
_34[_34.length]=_33.childNodes[i];
}
}
return _34;
}
catch(error){
return false;
}
};
RadGridNamespace.Confirm=function(_37,e){
if(!confirm(_37)){
e.cancelBubble=true;
e.returnValue=false;
return false;
}
};
RadGridNamespace.SynchronizeWithWindow=function(){
};
RadGridNamespace.IsParentRightToLeft=function(_39){
try{
while(_39){
_39=_39.parentNode;
if(_39.currentStyle&&_39.currentStyle.direction.toLowerCase()=="rtl"){
return true;
}else{
if(getComputedStyle&&getComputedStyle(_39,"").getPropertyValue("direction").toLowerCase()=="rtl"){
return true;
}else{
if(_39.dir.toLowerCase()=="rtl"){
return true;
}
}
}
}
return false;
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError,this.OnError);
}
};
RadGridNamespace.FireEvent=function(_3a,_3b,_3c){
try{
var _3d=true;
if(typeof (_3a[_3b])=="string"){
eval(_3a[_3b]);
}else{
if(typeof (_3a[_3b])=="function"){
if(_3c){
switch(_3c.length){
case 1:
_3d=_3a[_3b](_3c[0]);
break;
case 2:
_3d=_3a[_3b](_3c[0],_3c[1]);
break;
}
}else{
_3d=_3a[_3b]();
}
}
}
if(typeof (_3d)!="boolean"){
return true;
}else{
return _3d;
}
}
catch(error){
throw error;
}
};
RadGridNamespace.CheckParentNodesFor=function(_3e,_3f){
while(_3e){
if(_3e==_3f){
return true;
}
_3e=_3e.parentNode;
}
return false;
};
RadGridNamespace.GetCurrentElement=function(e){
if(!e){
var e=window.event;
}
var _41;
if(e.srcElement){
_41=e.srcElement;
}else{
_41=e.target;
}
return _41;
};
RadGridNamespace.GetEventPosX=function(e){
var x=e.clientX;
var _44=RadGridNamespace.GetCurrentElement(e);
while(_44.parentNode){
if(typeof (_44.parentNode.scrollLeft)=="number"){
x+=_44.parentNode.scrollLeft;
}
_44=_44.parentNode;
}
if(document.body.leftMargin!=null){
}
return x;
};
RadGridNamespace.GetEventPosY=function(e){
var y=e.clientY;
var _47=RadGridNamespace.GetCurrentElement(e);
while(_47.parentNode){
if(typeof (_47.parentNode.scrollTop)=="number"){
y+=_47.parentNode.scrollTop;
}
_47=_47.parentNode;
}
if(document.body.topMargin!=null){
}
return y;
};
RadGridNamespace.IsChildOf=function(_48,_49){
while(_48.parentNode){
if(_48.parentNode==_49){
return true;
}
_48=_48.parentNode;
}
return false;
};
RadGridNamespace.GetFirstParentByTagName=function(_4a,_4b){
while(_4a.parentNode){
if(_4a.tagName.toLowerCase()==_4b.toLowerCase()){
return _4a;
}
_4a=_4a.parentNode;
}
return null;
};
RadGridNamespace.FindScrollPosX=function(_4c){
var x=0;
while(_4c.parentNode){
if(typeof (_4c.parentNode.scrollLeft)=="number"){
x+=_4c.parentNode.scrollLeft;
}
_4c=_4c.parentNode;
}
return x;
};
RadGridNamespace.FindScrollPosY=function(_4e){
var y=0;
while(_4e.parentNode){
if(typeof (_4e.parentNode.scrollTop)=="number"){
y+=_4e.parentNode.scrollTop;
}
_4e=_4e.parentNode;
}
return y;
};
RadGridNamespace.FindPosX=function(_50){
try{
var x=0;
if(_50.offsetParent){
while(_50.offsetParent){
x+=_50.offsetLeft;
_50=_50.offsetParent;
}
}else{
if(_50.x){
x+=_50.x;
}
}
return x;
}
catch(error){
return x;
}
};
RadGridNamespace.FindPosY=function(_52){
var y=0;
if(_52.offsetParent){
while(_52.offsetParent){
y+=_52.offsetTop;
_52=_52.offsetParent;
}
}else{
if(_52.y){
y+=_52.y;
}
}
return y;
};
RadGridNamespace.GetNodeNextSiblingByTagName=function(_54,_55){
while((_54!=null)&&(_54.tagName!=_55)){
_54=_54.nextSibling;
}
return _54;
};
RadGridNamespace.GetNodeNextSibling=function(_56){
while(_56!=null){
if(_56.nextSibling){
_56=_56.nextSibling;
}else{
_56=null;
}
if(_56){
if(_56.nodeType==1){
break;
}
}
}
return _56;
};
RadGridNamespace.DeleteSubString=function(_57,_58,_59){
return _57=_57.substring(0,_58)+_57.substring(_59+1,_57.length);
};
RadGridNamespace.ClearDocumentEvents=function(){
if(document.onmousedown!=this.mouseDownHandler){
this.documentOnMouseDown=document.onmousedown;
}
if(document.onselectstart!=this.selectStartHandler){
this.documentOnSelectStart=document.onselectstart;
}
if(document.ondragstart!=this.dragStartHandler){
this.documentOnDragStart=document.ondragstart;
}
this.mouseDownHandler=function(e){
return false;
};
this.selectStartHandler=function(){
return false;
};
this.dragStartHandler=function(){
return false;
};
document.onmousedown=this.mouseDownHandler;
document.onselectstart=this.selectStartHandler;
document.ondragstart=this.dragStartHandler;
};
RadGridNamespace.RestoreDocumentEvents=function(){
if((typeof (this.documentOnMouseDown)=="function")&&(document.onmousedown!=this.mouseDownHandler)){
document.onmousedown=this.documentOnMouseDown;
}else{
document.onmousedown="";
}
if((typeof (this.documentOnSelectStart)=="function")&&(document.onselectstart!=this.selectStartHandler)){
document.onselectstart=this.documentOnSelectStart;
}else{
document.onselectstart="";
}
if((typeof (this.documentOnDragStart)=="function")&&(document.ondragstart!=this.dragStartHandler)){
document.ondragstart=this.documentOnDragStart;
}else{
document.ondragstart="";
}
};
RadGridNamespace.AddStyleSheet=function(_5b){
if(RadGridNamespace.StyleSheets==null){
RadGridNamespace.StyleSheets={};
}
var _5c=RadGridNamespace.StyleSheets[_5b];
if(_5c!=null){
return _5c;
}
if(window.opera!=null){
return;
}
var css=null;
var _5e=null;
var _5f=document.getElementsByTagName("head")[0];
if(window.netscape){
css=document.createElement("style");
css.media="all";
css.type="text/css";
_5f.appendChild(css);
}else{
try{
css=document.createStyleSheet();
}
catch(e){
return false;
}
}
var _60=document.styleSheets[document.styleSheets.length-1];
RadGridNamespace.StyleSheets[_5b]=css;
return _60;
};
RadGridNamespace.ClearStyleSheet=function(ss){
if(ss.deleteRule&&ss.cssRules){
var cnt=ss.cssRules.length;
while(cnt--){
ss.removeRule(cnt);
}
return;
}
var _63=false;
try{
var cnt=ss.rules.length;
while(cnt--){
ss.removeRule(cnt);
}
}
catch(e){
if((e.number&65535)==5){
_63=true;
}
}
if(_63){
try{
while(true){
ss.removeRule(0);
}
}
catch(e){
}
}
return ss;
};
RadGridNamespace.AddRule=function(ss,_65,_66){
try{
if(!ss){
return false;
}
if(ss.insertRule){
var _67=ss.insertRule(_65+" {"+_66+"}",ss.cssRules.length);
return ss.cssRules[ss.cssRules.length-1];
}
if(ss.addRule){
ss.addRule(_65,_66);
return true;
}
return false;
}
catch(e){
return false;
}
};
RadGridNamespace.addClassName=function(_68,_69){
var s=_68.className;
var p=s.split(" ");
if(p.length==1&&p[0]==""){
p=[];
}
var l=p.length;
for(var i=0;i<l;i++){
if(p[i]==_69){
return;
}
}
p[p.length]=_69;
_68.className=p.join(" ");
};
RadGridNamespace.removeClassName=function(_6e,_6f){
if(_6e.className.replace(/^\s*|\s*$/g,"")==_6f){
_6e.className="";
return;
}
var _70=_6e.className.split(" ");
var _71=[];
for(var i=0,l=_70.length;i<l;i++){
if(_70[i]==""){
continue;
}
if(_6f.indexOf(_70[i])==-1){
_71[_71.length]=_70[i];
}
}
_6e.className=_71.join(" ");
return;
_6e.className=(_6e.className.toString()==_6f)?"":_6e.className.replace(_6f,"").replace(/\s*$/g,"");
return;
var p=s.split(" ");
var np=[];
var l=p.length;
var j=0;
for(var i=0;i<l;i++){
if(p[i]!=_6f){
np[j++]=p[i];
}
}
_6e.className=np.join(" ");
};
RadGridNamespace.CheckIsParentDisplay=function(_77){
try{
while(_77){
if(_77.style){
if(_77.currentStyle){
if(_77.currentStyle.display=="none"){
return false;
}
}else{
if(_77.style.display=="none"){
return false;
}
}
}
_77=_77.parentNode;
}
if(window.top){
if(window.top.location!=window.location){
return false;
}
}
return true;
}
catch(e){
return false;
}
};
if(typeof (window.RadControlsNamespace)=="undefined"){
window.RadControlsNamespace=new Object();
}
RadControlsNamespace.AppendStyleSheet=function(_78,_79,_7a){
if(!_7a){
return;
}
if(!_78){
document.write("<"+"link"+" rel='stylesheet' type='text/css' href='"+_7a+"' />");
}else{
var _7b=document.createElement("link");
_7b.rel="stylesheet";
_7b.type="text/css";
_7b.href=_7a;
var _7c=document.getElementById(_79+"StyleSheetHolder");
if(_7c!=null){
document.getElementById(_79+"StyleSheetHolder").appendChild(_7b);
}
}
};
RadGridNamespace.RadGrid=function(_7d){
var _7e=window[_7d.ClientID];
if(_7e!=null&&typeof (_7e.Dispose)=="function"){
window.setTimeout(function(){
_7e.Dispose();
},100);
}
RadControlsNamespace.DomEventMixin.Initialize(this);
this.AttachDomEvent(window,"unload","OnWindowUnload");
window[_7d.ClientID]=this;
window["grid_"+_7d.ClientID]=this;
if(!document.readyState||document.readyState=="complete"||window.opera){
this._constructor(_7d);
}else{
this.objectData=_7d;
this.AttachDomEvent(window,"load","OnWindowLoad");
}
};
RadGridNamespace.RadGrid.prototype.OnWindowUnload=function(e){
this.Dispose();
};
RadGridNamespace.RadGrid.prototype.OnWindowLoad=function(e){
this._constructor(this.objectData);
this.objectData=null;
};
RadGridNamespace.RadGrid.prototype._constructor=function(_81){
this.Type="RadGrid";
this.InitializeEvents(_81.ClientSettings.ClientEvents);
RadGridNamespace.FireEvent(this,"OnGridCreating");
for(var _82 in _81){
this[_82]=_81[_82];
}
this.Initialize();
RadGridNamespace.FireEvent(this,"OnMasterTableViewCreating");
this.GridStyleSheet=RadGridNamespace.AddStyleSheet(this.ClientID);
if(this.ClientSettings.Scrolling.AllowScroll&&this.ClientSettings.Scrolling.UseStaticHeaders){
var ID=_81.MasterTableView.ClientID;
_81.MasterTableView.ClientID=ID+"_Header";
this.MasterTableViewHeader=new RadGridNamespace.RadGridTable(_81.MasterTableView);
this.MasterTableViewHeader._constructor(this);
if(document.getElementById(ID+"_Footer")){
_81.MasterTableView.ClientID=ID+"_Footer";
this.MasterTableViewFooter=new RadGridNamespace.RadGridTable(_81.MasterTableView);
this.MasterTableViewFooter._constructor(this);
}
_81.MasterTableView.ClientID=ID;
}
this.MasterTableView._constructor(this);
RadGridNamespace.FireEvent(this,"OnMasterTableViewCreated");
this.DetailTablesCollection=new Array();
this.LoadDetailTablesCollection(this.MasterTableView,1);
this.AttachDomEvents();
RadGridNamespace.FireEvent(this,"OnGridCreated");
this.InitializeFeatures(_81);
this.Url=this.ClientSettings.AJAXUrl;
this.EnableOutsideScripts=this.ClientSettings.EnableOutsideScripts;
if(typeof (window.event)=="undefined"){
window.event=null;
}
};
RadGridNamespace.RadGrid.prototype.Dispose=function(){
try{
RadGridNamespace.FireEvent(this,"OnGridDestroying");
this.DisposeDomEventHandlers();
this.DisposeEvents();
RadGridNamespace.ClearStyleSheet(this.GridStyleSheet);
this.GridStyleSheet=null;
this.DisposeFeatures();
this.DisposeDetailTablesCollection(this.MasterTableView,1);
if(this.MasterTableViewHeader!=null){
this.MasterTableViewHeader.Dispose();
}
if(this.MasterTableViewFooter!=null){
this.MasterTableViewFooter.Dispose();
}
if(this.MasterTableView!=null){
this.MasterTableView.Dispose();
}
this.DisposeProperties();
}
catch(error){
}
};
RadGridNamespace.RadGrid.ClientEventNames={OnGridCreating:true,OnGridCreated:true,OnGridDestroying:true,OnMasterTableViewCreating:true,OnMasterTableViewCreated:true,OnTableCreating:true,OnTableCreated:true,OnTableDestroying:true,OnScroll:true,OnKeyPress:true,OnRequestStart:true,OnRequestEnd:true,OnRequestError:true,OnError:true,OnRowDeleting:true,OnRowDeleted:true};
RadGridNamespace.RadGrid.prototype.IsClientEventName=function(_84){
return RadGridNamespace.RadGrid.ClientEventNames[_84]==true;
};
RadGridNamespace.RadGrid.prototype.InitializeEvents=function(_85){
for(var _86 in _85){
if(typeof (_85[_86])!="string"){
continue;
}
if(this.IsClientEventName(_86)){
if(_85[_86]!=""){
var _87=_85[_86];
if(_87.indexOf("(")!=-1){
this[_86]=_87;
}else{
this[_86]=eval(_87);
}
}else{
this[_86]=null;
}
}
}
};
RadGridNamespace.RadGrid.prototype.DisposeEvents=function(){
for(var _88 in RadGridNamespace.RadGrid.ClientEventNames){
this[_88]=null;
}
};
RadGridNamespace.RadGrid.prototype.GetDetailTable=function(_89,_8a){
if(_89.HierarchyIndex==_8a){
return _89;
}
if(_89.DetailTables){
for(var i=0;i<_89.DetailTables.length;i++){
var res=this.GetDetailTable(_89.DetailTables[i],_8a);
if(res){
return res;
}
}
}
};
RadGridNamespace.RadGrid.prototype.LoadDetailTablesCollection=function(_8d,_8e){
try{
if(_8d.Controls[0]!=null&&_8d.Controls[0].Rows!=null){
for(var i=0;i<_8d.Controls[0].Rows.length;i++){
var _90=_8d.Controls[0].Rows[i].ItemType;
if(_90=="NestedView"){
var _91=_8d.Controls[0].Rows[i].NestedTableViews;
for(var j=0;j<_91.length;j++){
var _93=_91[j];
if(_93.Visible){
var _94=this.GetDetailTable(this.MasterTableView,_93.HierarchyIndex);
_93.RenderColumns=_94.RenderColumns;
RadGridNamespace.FireEvent(this,"OnTableCreating",[_94]);
_93._constructor(this);
this.DetailTablesCollection[this.DetailTablesCollection.length]=_93;
if(_93.AllowFilteringByColumn){
this.InitializeFilterMenu(_93);
}
RadGridNamespace.FireEvent(this,"OnTableCreated",[_93]);
}
this.LoadDetailTablesCollection(_93,_8e+1);
}
}
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.DisposeDetailTablesCollection=function(_95,_96){
if(_95.Controls[0]!=null&&_95.Controls[0].Rows!=null){
for(var i=0;i<_95.Controls[0].Rows.length;i++){
var _98=_95.Controls[0].Rows[i].ItemType;
if(_98=="NestedView"){
var _99=_95.Controls[0].Rows[i].NestedTableViews;
for(var j=0;j<_99.length;j++){
var _9b=_99[j];
_9b.Dispose();
}
}
}
}
};
RadGridNamespace.RadGrid.prototype.Initialize=function(){
this.Control=document.getElementById(this.ClientID);
if(this.Control==null){
return;
}
this.GridDataDiv=document.getElementById(this.ClientID+"_GridData");
this.GroupPanelControl=document.getElementById(this.GroupPanel.ClientID+"_GroupPanel");
this.GridHeaderDiv=document.getElementById(this.ClientID+"_GridHeader");
this.GridFooterDiv=document.getElementById(this.ClientID+"_GridFooter");
this.PostDataValue=document.getElementById(this.ClientID+"PostDataValue");
this.LoadingTemplate=document.getElementById(this.ClientID+"_LoadingTemplate");
this.PagerControl=document.getElementById(this.MasterTableView.ClientID+"_Pager");
this.TopPagerControl=document.getElementById(this.MasterTableView.ClientID+"_TopPager");
if(this.LoadingTemplate){
this.LoadingTemplate.style.display="none";
if(this.GridDataDiv){
this.GridDataDiv.appendChild(this.LoadingTemplate);
}
}
this.FormID=this.ClientSettings.FormID;
};
RadGridNamespace.RadGrid.prototype.DisposeProperties=function(){
this.Control=null;
this.GridDataDiv=null;
this.GroupPanelControl=null;
this.GridHeaderDiv=null;
this.GridFooterDiv=null;
this.PostDataValue=null;
this.LoadingTemplate=null;
this.PagerControl=null;
};
RadGridNamespace.RadGrid.prototype.InitializeFeatures=function(_9c){
if(!this.MasterTableView.Control){
return;
}
if(this.GroupPanelControl!=null){
this.GroupPanelObject=new RadGridNamespace.RadGridGroupPanel(this.GroupPanelControl,this);
}
if(this.ClientSettings.Scrolling.AllowScroll){
this.InitializeDimensions();
this.InitializeScroll();
}
if(this.Control.align==""){
var _9d=RadGridNamespace.IsParentRightToLeft(this.GridHeaderDiv);
if(!_9d){
this.Control.align="left";
}else{
this.Control.align="right";
}
}
if(this.AllowFilteringByColumn||this.MasterTableView.AllowFilteringByColumn){
var _9e=(this.MasterTableViewHeader)?this.MasterTableViewHeader:this.MasterTableView;
this.InitializeFilterMenu(_9e);
}
if(this.ClientSettings.AllowKeyboardNavigation&&this.MasterTableView.Rows){
if(!this.MasterTableView.RenderActiveItemStyleClass||this.MasterTableView.RenderActiveItemStyleClass==""){
if(this.MasterTableView.RenderActiveItemStyle&&this.MasterTableView.RenderActiveItemStyle!=""){
RadGridNamespace.AddRule(this.GridStyleSheet,".ActiveItemStyle"+this.MasterTableView.ClientID+"1 td",this.MasterTableView.RenderActiveItemStyle);
}else{
RadGridNamespace.AddRule(this.GridStyleSheet,".ActiveItemStyle"+this.MasterTableView.ClientID+"2 td","background-color:#FFA07A;");
}
}
if(this.ActiveRow==null){
this.ActiveRow=this.MasterTableView.Rows[0];
}
this.SetActiveRow(this.ActiveRow);
}
if(window[this.ClientID+"_Slider"]){
this.Slider=new RadGridNamespace.Slider(window[this.ClientID+"_Slider"]);
}
};
RadGridNamespace.RadGrid.prototype.DisposeFeatures=function(){
if(this.Slider!=null){
this.Slider.Dispose();
this.Slider=null;
}
if(this.GroupPanelControl!=null){
this.GroupPanelObject.Dispose();
this.GroupPanelControl=null;
}
if(this.AllowFilteringByColumn||this.MasterTableView.AllowFilteringByColumn){
var _9f=(this.MasterTableViewHeader)?this.MasterTableViewHeader:this.MasterTableView;
this.DisposeFilterMenu(_9f);
}
this.Control=null;
};
RadGridNamespace.RadGrid.prototype.AsyncRequest=function(_a0,_a1){
var _a2;
if(this.StatusBarSettings!=null&&this.StatusBarSettings.StatusLabelID!=null&&this.StatusBarSettings.StatusLabelID!=""){
var _a3=document.getElementById(this.StatusBarSettings.StatusLabelID);
if(_a3!=null){
_a2=_a3.innerHTML;
_a3.innerHTML=this.StatusBarSettings.LoadingText;
}
}
var _a4=this.ClientID;
this.OnRequestEndInternal=function(){
RadGridNamespace.FireEvent(window[_a4],"OnRequestEnd");
if(_a3){
_a3.innerHTML=_a2;
}
};
RadAjaxNamespace.AsyncRequest(_a0,_a1,_a4);
};
RadGridNamespace.RadGrid.prototype.AjaxRequest=function(_a5,_a6){
this.AsyncRequest(_a5,_a6);
};
RadGridNamespace.RadGrid.prototype.ClearSelectedRows=function(){
for(var i=0;i<this.DetailTablesCollection.length;i++){
var _a8=this.DetailTablesCollection[i];
_a8.ClearSelectedRows();
}
this.MasterTableView.ClearSelectedRows();
};
RadGridNamespace.RadGrid.prototype.AsyncRequestWithOptions=function(_a9){
RadAjaxNamespace.AsyncRequestWithOptions(_a9,this.ClientID);
};
RadGridNamespace.RadGrid.prototype.DeleteRow=function(_aa,_ab,e){
var _ad=(e.srcElement)?e.srcElement:e.target;
if(!_ad){
return;
}
var row=_ad.parentNode.parentNode;
var _af=row.parentNode.parentNode;
var _b0=row.rowIndex;
var _b1=row.cells.length;
var _b2=this.GetTableObjectByID(_aa);
var _b3=this.GetRowObjectByRealRow(_b2,row);
var _b4={Row:_b3};
if(!RadGridNamespace.FireEvent(this,"OnRowDeleting",[_b2,_b4])){
return;
}
_af.deleteRow(row.rowIndex);
for(var i=_b0;i<_af.rows.length;i++){
if(_af.rows[i].cells.length!=_b1&&_af.rows[i].style.display!="none"){
_af.deleteRow(i);
i--;
}else{
break;
}
}
if(_af.tBodies[0].rows.length==1&&_af.tBodies[0].rows[0].style.display=="none"){
_af.tBodies[0].rows[0].style.display="";
}
this.PostDataValue.value+="DeletedRows,"+_aa+","+_ab+";";
RadGridNamespace.FireEvent(this,"OnRowDeleted",[_b2,_b4]);
};
RadGridNamespace.RadGrid.prototype.SelectRow=function(_b6,_b7,e){
var _b9=(e.srcElement)?e.srcElement:e.target;
if(!_b9){
return;
}
var row=_b9.parentNode.parentNode;
var _bb=row.parentNode.parentNode;
var _bc=row.rowIndex;
var _bd;
if(_b6==this.MasterTableView.UID){
_bd=this.MasterTableView;
}else{
for(var i=0;i<this.DetailTablesCollection.length;i++){
if(this.DetailTablesCollection[i].ClientID==_bb.id){
_bd=this.DetailTablesCollection[i];
break;
}
}
}
if(_bd!=null){
if(this.AllowMultiRowSelection){
_bd.SelectRow(row,false);
}else{
_bd.SelectRow(row,true);
}
}
};
RadGridNamespace.RadGrid.prototype.SelectAllRows=function(_bf,_c0,e){
var _c2=(e.srcElement)?e.srcElement:e.target;
if(!_c2){
return;
}
var row=_c2.parentNode.parentNode;
var _c4=row.parentNode.parentNode;
var _c5=row.rowIndex;
var _c6;
if(_bf==this.MasterTableView.UID){
_c6=this.MasterTableView;
}else{
for(var i=0;i<this.DetailTablesCollection.length;i++){
if(this.DetailTablesCollection[i].UID==_bf){
_c6=this.DetailTablesCollection[i];
break;
}
}
}
if(_c6!=null){
if(this.AllowMultiRowSelection){
if(_c6==this.MasterTableViewHeader){
_c6=this.MasterTableView;
}
_c6.ClearSelectedRows();
if(_c2.checked){
for(var i=0;i<_c6.Control.tBodies[0].rows.length;i++){
var row=_c6.Control.tBodies[0].rows[i];
_c6.SelectRow(row,false);
}
}else{
for(var i=0;i<_c6.Control.tBodies[0].rows.length;i++){
var row=_c6.Control.tBodies[0].rows[i];
_c6.DeselectRow(row);
}
this.SavePostData("SelectedRows",_c6.ClientID,"");
}
}
}
};
RadGridNamespace.RadGrid.prototype.HandleActiveRow=function(e){
if((this.AllowRowResize)||(this.AllowRowSelect)){
var _c9=this.GetCellFromPoint(e);
if((_c9!=null)&&(_c9.parentNode.id!="")&&(_c9.parentNode.id!=-1)&&(_c9.cellIndex==0)){
var _ca=_c9.parentNode.parentNode.parentNode;
this.SetActiveRow(_ca,_c9.parentNode.rowIndex);
}
}
};
RadGridNamespace.RadGrid.prototype.SetActiveRow=function(_cb){
if(_cb==null){
return;
}
if(_cb.Owner.RenderActiveItemStyle){
RadGridNamespace.removeClassName(this.ActiveRow.Control,"ActiveItemStyle"+_cb.Owner.ClientID+"1");
}else{
RadGridNamespace.removeClassName(this.ActiveRow.Control,"ActiveItemStyle"+_cb.Owner.ClientID+"2");
}
RadGridNamespace.removeClassName(this.ActiveRow.Control,_cb.Owner.RenderActiveItemStyleClass);
if(this.ActiveRow.Control.style.cssText==_cb.Owner.RenderActiveItemStyle){
this.ActiveRow.Control.style.cssText="";
}
this.ActiveRow=_cb;
if(!this.ActiveRow.Owner.RenderActiveItemStyleClass||this.ActiveRow.Owner.RenderActiveItemStyleClass==""){
if(this.ActiveRow.Owner.RenderActiveItemStyle&&this.ActiveRow.Owner.RenderActiveItemStyle!=""){
RadGridNamespace.addClassName(this.ActiveRow.Control,"ActiveItemStyle"+this.ActiveRow.Owner.ClientID+"1");
}else{
RadGridNamespace.addClassName(this.ActiveRow.Control,"ActiveItemStyle"+this.ActiveRow.Owner.ClientID+"2");
}
}else{
RadGridNamespace.addClassName(this.ActiveRow.Control,this.ActiveRow.Owner.RenderActiveItemStyleClass);
}
this.SavePostData("ActiveRow",this.ActiveRow.Owner.ClientID,this.ActiveRow.RealIndex);
};
RadGridNamespace.RadGrid.prototype.GetNextRow=function(_cc,_cd){
if(_cc!=null){
if(_cc.tBodies[0].rows[_cd]!=null){
while(_cc.tBodies[0].rows[_cd]!=null){
_cd++;
if(_cd<=(_cc.tBodies[0].rows.length-1)){
return _cc.tBodies[0].rows[_cd];
}else{
return null;
}
}
}
}
};
RadGridNamespace.RadGrid.prototype.GetPreviousRow=function(_ce,_cf){
if(_ce!=null){
if(_ce.tBodies[0].rows[_cf]!=null){
while(_ce.tBodies[0].rows[_cf]!=null){
_cf--;
if(_cf>=0){
return _ce.tBodies[0].rows[_cf];
}else{
return null;
}
}
}
}
};
RadGridNamespace.RadGrid.prototype.GetNextHierarchicalRow=function(_d0,_d1){
if(_d0!=null){
if(_d0.tBodies[0].rows[_d1]!=null){
_d1++;
var row=_d0.tBodies[0].rows[_d1];
if(_d0.tBodies[0].rows[_d1]!=null){
if((row.cells[1]!=null)&&(row.cells[2]!=null)){
if((row.cells[1].getElementsByTagName("table").length>0)||(row.cells[2].getElementsByTagName("table").length>0)){
var _d3=this.GetNextRow(row.cells[2].firstChild,0);
return _d3;
}else{
return null;
}
}
}
}
}
};
RadGridNamespace.RadGrid.prototype.GetPreviousHierarchicalRow=function(_d4,_d5){
if(_d4!=null){
if(_d4.parentNode!=null){
if(_d4.parentNode.tagName.toLowerCase()=="td"){
var _d6=_d4.parentNode.parentNode.parentNode.parentNode;
var _d7=_d4.parentNode.parentNode.rowIndex;
return this.GetPreviousRow(_d6,_d7);
}else{
return null;
}
}else{
return this.GetPreviousRow(_d4,_d5);
}
}
};
RadGridNamespace.RadGrid.prototype.HandleCellEdit=function(e){
var _d9=RadGridNamespace.GetCurrentElement(e);
var _da=RadGridNamespace.GetFirstParentByTagName(_d9,"td");
if(_da!=null){
_d9=_da;
var _db=_d9.parentNode.parentNode.parentNode;
var _dc=this.GetTableObjectByID(_db.id);
if((_dc!=null)&&(_dc.Columns.length>0)&&(_dc.Columns[_d9.cellIndex]!=null)){
if(_dc.Columns[_d9.cellIndex].ColumnType!="GridBoundColumn"){
return;
}
this.EditedCell=_dc.Control.rows[_d9.parentNode.rowIndex].cells[_d9.cellIndex];
this.CellEditor=new RadGridNamespace.RadGridCellEditor(this.EditedCell,_dc.Columns[_d9.cellIndex],this);
}
}
};
RadGridNamespace.RadGridCellEditor=function(_dd,_de,_df){
if(_df.CellEditor){
return;
}
this.Control=document.createElement("input");
this.Control.style.border="1px groove";
this.Control.style.width="100%";
this.Control.value=_dd.innerHTML;
this.OldValue=this.Control.value;
_dd.innerHTML="";
var _e0=this;
this.Control.onblur=function(e){
if(!e){
var e=window.event;
}
_dd.removeChild(this);
_dd.innerHTML=this.value;
if(this.value!=_e0.OldValue){
alert(1);
}
_df.CellEditor=null;
};
_dd.appendChild(this.Control);
if(this.Control.focus){
this.Control.focus();
}
};
if(!("console" in window)||!("firebug" in console)){
var names=["log","debug","info","warn","error","assert","dir","dirxml","group","groupEnd","time","timeEnd","count","trace","profile","profileEnd"];
window.console={};
for(var i=0;i<names.length;++i){
window.console[names[i]]=function(){
};
}
}
RadGridNamespace.Error=function(_e2,_e3,_e4){
if((!_e2)||(!_e3)||(!_e4)){
return false;
}
this.Message=_e2.message;
if(_e4!=null){
if("string"==typeof (_e4)){
try{
eval(_e4);
}
catch(e){
var _e5="";
_e5="";
_e5+="Telerik RadGrid Error:\r\n";
_e5+="-----------------\r\n";
_e5+="Message: \""+e.message+"\"\r\n";
_e5+="Raised by: "+_e3.Type+"\r\n";
alert(_e5);
}
}else{
if("function"==typeof (_e4)){
try{
_e4(this);
}
catch(e){
var _e5="";
_e5="";
_e5+="Telerik RadGrid Error:\r\n";
_e5+="-----------------\r\n";
_e5+="Message: \""+e.message+"\"\r\n";
_e5+="Raised by: "+_e3.Type+"\r\n";
alert(_e5);
}
}
}
}else{
this.Owner=_e3;
for(var _e6 in _e2){
this[_e6]=_e2[_e6];
}
this.Message="";
this.Message+="Telerik RadGrid Error:\r\n";
this.Message+="-----------------\r\n";
this.Message+="Message: \""+_e2.message+"\"\r\n";
this.Message+="Raised by: "+_e3.Type+"\r\n";
alert(this.Message);
}
};
RadGridNamespace.RadGrid.prototype.GetTableObjectByID=function(id){
if(this.MasterTableView.ClientID==id||this.MasterTableView.UID==id){
return this.MasterTableView;
}else{
for(var i=0;i<this.DetailTablesCollection.length;i++){
if(this.DetailTablesCollection[i].ClientID==id||this.DetailTablesCollection[i].UID==id){
return this.DetailTablesCollection[i];
}
}
}
if(this.MasterTableViewHeader!=null){
if(this.MasterTableViewHeader.ClientID==id||this.MasterTableViewHeader.UID==id){
return table=this.MasterTableViewHeader;
}
}
};
RadGridNamespace.RadGrid.prototype.GetRowObjectByRealRow=function(_e9,row){
if(_e9.Rows!=null){
for(var i=0;i<_e9.Rows.length;i++){
if(_e9.Rows[i].Control==row){
return _e9.Rows[i];
}
}
}
};
RadGridNamespace.RadGrid.prototype.SavePostData=function(){
try{
var _ec=new String();
for(var i=0;i<arguments.length;i++){
_ec+=arguments[i]+",";
}
_ec=_ec.substring(0,_ec.length-1);
if(this.PostDataValue!=null){
switch(arguments[0]){
case "ReorderedColumns":
this.PostDataValue.value+=_ec+";";
break;
case "HidedColumns":
var _ee=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
_ee="ShowedColumns"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
break;
case "ShowedColumns":
var _ee=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
_ee="HidedColumns"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
break;
case "HidedRows":
var _ee=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
_ee="ShowedRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
break;
case "ShowedRows":
var _ee=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
_ee="HidedRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
break;
case "ResizedColumns":
var _ee=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
break;
case "ResizedRows":
var _ee=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
break;
case "ResizedControl":
var _ee=arguments[0]+","+arguments[1];
this.UpdatePostData(_ec,_ee);
break;
case "ClientCreated":
var _ee=arguments[0]+","+arguments[1];
this.UpdatePostData(_ec,_ee);
break;
case "ScrolledControl":
var _ee=arguments[0]+","+arguments[1];
this.UpdatePostData(_ec,_ee);
break;
case "AJAXScrolledControl":
var _ee=arguments[0]+","+arguments[1];
this.UpdatePostData(_ec,_ee);
break;
case "SelectedRows":
var _ee=arguments[0]+","+arguments[1]+",";
this.UpdatePostData(_ec,_ee);
break;
case "EditRow":
var _ee=arguments[0]+","+arguments[1];
this.UpdatePostData(_ec,_ee);
break;
case "ActiveRow":
var _ee=arguments[0]+","+arguments[1];
this.UpdatePostData(_ec,_ee);
break;
case "CollapsedRows":
var _ee=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
_ee="ExpandedRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
break;
case "ExpandedRows":
var _ee=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
_ee="CollapsedRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
break;
case "CollapsedGroupRows":
var _ee=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
_ee="ExpandedGroupRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
break;
case "ExpandedGroupRows":
var _ee=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
_ee="CollapsedGroupRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_ec,_ee);
break;
default:
this.UpdatePostData(_ec,_ec);
break;
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.UpdatePostData=function(_ef,_f0){
var _f1,_f2=new Array();
_f1=this.PostDataValue.value.split(";");
for(var i=0;i<_f1.length;i++){
if(_f1[i].indexOf(_f0)==-1){
_f2[_f2.length]=_f1[i];
}
}
this.PostDataValue.value=_f2.join(";");
this.PostDataValue.value+=_ef+";";
};
RadGridNamespace.RadGrid.prototype.DeletePostData=function(_f4,_f5){
var _f6,_f7=new Array();
_f6=this.PostDataValue.value.split(";");
for(var i=0;i<_f6.length;i++){
if(_f6[i].indexOf(_f5)==-1){
_f7[_f7.length]=_f6[i];
}
}
this.PostDataValue.value=_f7.join(";");
};
RadGridNamespace.RadGrid.prototype.HandleDragAndDrop=function(e,_fa){
try{
var _fb=this;
if((_fa!=null)&&(_fa.tagName.toLowerCase()=="th")){
var _fc=_fa.parentNode.parentNode.parentNode;
var _fd=this.GetTableObjectByID(_fc.id);
if((_fd!=null)&&(_fd.Columns.length>0)&&(_fd.Columns[_fa.cellIndex]!=null)&&((_fd.Columns[_fa.cellIndex].Reorderable)||(_fd.Owner.ClientSettings.AllowDragToGroup&&_fd.Columns[_fa.cellIndex].Groupable))){
var _fe=RadGridNamespace.GetEventPosX(e);
var _ff=RadGridNamespace.FindPosX(_fa);
var endX=_ff+_fa.offsetWidth;
this.ResizeTolerance=5;
var _101=_fa.title;
var _102=_fa.style.cursor;
if(!((_fe>=endX-this.ResizeTolerance)&&(_fe<=endX+this.ResizeTolerance))){
if(this.MoveHeaderDiv){
if(this.MoveHeaderDiv.innerHTML!=_fa.innerHTML){
_fa.title=this.ClientSettings.ClientMessages.DropHereToReorder;
_fa.style.cursor="default";
if(_fa.parentNode.parentNode.parentNode==this.MoveHeaderDivRefCell.parentNode.parentNode.parentNode){
this.MoveReorderIndicators(e,_fa);
}else{
if(this.ReorderIndicator1!=null){
this.ReorderIndicator1.style.visibility="hidden";
this.ReorderIndicator1.style.display="none";
this.ReorderIndicator1.style.position="absolute";
}
if(this.ReorderIndicator2!=null){
this.ReorderIndicator2.style.visibility=this.ReorderIndicator1.style.visibility;
this.ReorderIndicator2.style.display=this.ReorderIndicator1.style.display;
this.ReorderIndicator2.style.position=this.ReorderIndicator1.style.position;
}
}
}
}else{
_fa.title=this.ClientSettings.ClientMessages.DragToGroupOrReorder;
_fa.style.cursor="move";
}
this.AttachDomEvent(_fa,"mousedown","OnDragDropMouseDown");
}else{
_fa.style.cursor=_102;
_fa.title="";
}
}
}
if(this.MoveHeaderDiv!=null){
this.MoveHeaderDiv.style.visibility="";
this.MoveHeaderDiv.style.display="";
RadGridNamespace.RadGrid.PositionDragElement(this.MoveHeaderDiv,e);
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.PositionDragElement=function(_103,_104){
_103.style.top=_104.clientY+document.documentElement.scrollTop+document.body.scrollTop+1+"px";
_103.style.left=_104.clientX+document.documentElement.scrollLeft+document.body.scrollLeft+1+"px";
};
RadGridNamespace.RadGrid.prototype.OnDragDropMouseDown=function(e){
var _106=RadGridNamespace.GetCurrentElement(e);
var _107=false;
var form=document.getElementById(this.FormID);
if(form!=null&&form["__EVENTTARGET"]!=null&&form["__EVENTTARGET"].value!=""){
_107=true;
}
if((_106.tagName.toLowerCase()=="input"&&_106.type.toLowerCase()=="text")||(_106.tagName.toLowerCase()=="textarea")){
return;
}
_106=RadGridNamespace.GetFirstParentByTagName(_106,"th");
if(_106.tagName.toLowerCase()=="th"&&!this.IsResize){
if(((window.netscape||window.opera)&&(e.button==0))||(e.button==1)){
this.CreateDragAndDrop(e,_106);
}
RadGridNamespace.ClearDocumentEvents();
this.DetachDomEvent(_106,"mousedown","OnDragDropMouseDown");
this.AttachDomEvent(document,"mouseup","OnDragDropMouseUp");
if(this.GroupPanelControl!=null){
this.AttachDomEvent(this.GroupPanelControl,"mouseup","OnDragDropMouseUp");
}
}
};
RadGridNamespace.RadGrid.prototype.OnDragDropMouseUp=function(e){
this.DetachDomEvent(document,"mouseup","OnDragDropMouseUp");
if(this.GroupPanelControl!=null){
this.DetachDomEvent(this.GroupPanelControl,"mouseup","OnDragDropMouseUp");
}
this.FireDropAction(e);
this.DestroyDragAndDrop(e);
RadGridNamespace.RestoreDocumentEvents();
};
RadGridNamespace.CopyAttributes=function(_10a,_10b){
for(var i=0;i<_10b.attributes.length;i++){
try{
if(_10b.attributes[i].name.toLowerCase()=="id"){
continue;
}
if(_10b.attributes[i].value!=null&&_10b.attributes[i].value!="null"&&_10b.attributes[i].value!=""){
_10a.setAttribute(_10b.attributes[i].name,_10b.attributes[i].value);
}
}
catch(e){
continue;
}
}
};
RadGridNamespace.RadGrid.prototype.CreateDragAndDrop=function(e,_10e){
this.MoveHeaderDivRefCell=_10e;
this.MoveHeaderDiv=document.createElement("div");
var _10f=document.createElement("table");
if(this.MoveHeaderDiv.mergeAttributes){
this.MoveHeaderDiv.mergeAttributes(this.Control);
}else{
RadGridNamespace.CopyAttributes(this.MoveHeaderDiv,this.Control);
}
if(_10f.mergeAttributes){
_10f.mergeAttributes(this.MasterTableView.Control);
}else{
RadGridNamespace.CopyAttributes(_10f,this.MasterTableView.Control);
}
_10f.style.margin="0px";
_10f.style.height=_10e.offsetHeight+"px";
_10f.style.width=_10e.offsetWidth+"px";
var _110=document.createElement("thead");
var tr=document.createElement("tr");
_10f.appendChild(_110);
_110.appendChild(tr);
tr.appendChild(_10e.cloneNode(true));
this.MoveHeaderDiv.appendChild(_10f);
document.body.appendChild(this.MoveHeaderDiv);
this.MoveHeaderDiv.style.height=_10e.offsetHeight+"px";
this.MoveHeaderDiv.style.width=_10e.offsetWidth+"px";
this.MoveHeaderDiv.style.position="absolute";
RadGridNamespace.RadGrid.PositionDragElement(this.MoveHeaderDiv,e);
if(window.netscape){
this.MoveHeaderDiv.style.MozOpacity=3/4;
}else{
this.MoveHeaderDiv.style.filter="alpha(opacity=75);";
}
this.MoveHeaderDiv.style.cursor="move";
this.MoveHeaderDiv.style.visibility="hidden";
this.MoveHeaderDiv.style.display="none";
this.MoveHeaderDiv.style.fontWeight="bold";
this.MoveHeaderDiv.onmousedown=null;
RadGridNamespace.ClearDocumentEvents();
if(this.ClientSettings.AllowColumnsReorder){
this.CreateReorderIndicators(_10e);
}
};
RadGridNamespace.RadGrid.prototype.DestroyDragAndDrop=function(){
if(this.MoveHeaderDiv!=null){
var _112=this.MoveHeaderDiv.parentNode;
_112.removeChild(this.MoveHeaderDiv);
this.MoveHeaderDiv.onmouseup=null;
this.MoveHeaderDiv.onmousemove=null;
this.MoveHeaderDiv=null;
this.MoveHeaderDivRefCell=null;
this.DragCellIndex=null;
RadGridNamespace.RestoreDocumentEvents();
this.DestroyReorderIndicators();
}
};
RadGridNamespace.RadGrid.prototype.FireDropAction=function(e){
if((this.MoveHeaderDiv!=null)&&(this.MoveHeaderDiv.style.display!="none")){
var _114=RadGridNamespace.GetCurrentElement(e);
if((_114!=null)&&(this.MoveHeaderDiv!=null)){
if(_114!=this.MoveHeaderDivRefCell){
var _115=this.GetTableObjectByID(this.MoveHeaderDivRefCell.parentNode.parentNode.parentNode.id);
var _116=_115.HeaderRow;
if(RadGridNamespace.IsChildOf(_114,_116)){
if(_114.tagName.toLowerCase()!="th"){
_114=RadGridNamespace.GetFirstParentByTagName(_114,"th");
}
var _117=_114.parentNode.parentNode.parentNode;
var _118=this.MoveHeaderDivRefCell.parentNode.parentNode.parentNode;
if(_117.id==_118.id){
var _119=this.GetTableObjectByID(_117.id);
var _11a=_114.cellIndex;
if(window.attachEvent&&!window.opera&&!window.netscape){
_11a=RadGridNamespace.GetRealCellIndex(_119,_114);
}
var _11b=this.MoveHeaderDivRefCell.cellIndex;
if(window.attachEvent&&!window.opera&&!window.netscape){
_11b=RadGridNamespace.GetRealCellIndex(_119,this.MoveHeaderDivRefCell);
}
if(!_119||!_119.Columns[_11a]){
return;
}
if(!_119.Columns[_11a].Reorderable){
return;
}
_119.SwapColumns(_11a,_11b,(this.ClientSettings.ColumnsReorderMethod!="Reorder"));
if(this.ClientSettings.ColumnsReorderMethod=="Reorder"){
if((!this.ClientSettings.ReorderColumnsOnClient)&&(this.ClientSettings.PostBackReferences.PostBackColumnsReorder!="")){
eval(this.ClientSettings.PostBackReferences.PostBackColumnsReorder);
}
}
}
}else{
if(RadGridNamespace.CheckParentNodesFor(_114,this.GroupPanelControl)){
if((this.ClientSettings.PostBackReferences.PostBackGroupByColumn!="")&&(this.ClientSettings.AllowDragToGroup)){
var _119=this.GetTableObjectByID(this.MoveHeaderDivRefCell.parentNode.parentNode.parentNode.id);
var _11c=this.MoveHeaderDivRefCell.cellIndex;
if(window.attachEvent&&!window.opera&&!window.netscape){
_11c=RadGridNamespace.GetRealCellIndex(_119,this.MoveHeaderDivRefCell);
}
var _11d=_119.Columns[_11c].RealIndex;
if(_119.Columns[_11c].Groupable){
if(_119==this.MasterTableViewHeader){
this.SavePostData("GroupByColumn",this.MasterTableView.ClientID,_11d);
}else{
this.SavePostData("GroupByColumn",_119.ClientID,_11d);
}
eval(this.ClientSettings.PostBackReferences.PostBackGroupByColumn);
}
}
}
}
}
}
}
};
RadGridNamespace.GetRealCellIndex=function(_11e,cell){
for(var i=0;i<_11e.Columns.length;i++){
if(_11e.Columns[i].Control==cell){
return i;
}
}
};
RadGridNamespace.RadGrid.prototype.CreateReorderIndicators=function(_121){
if((this.ReorderIndicator1==null)&&(this.ReorderIndicator2==null)){
var _122=this.MoveHeaderDivRefCell.parentNode.parentNode.parentNode;
var _123=this.GetTableObjectByID(_122.id);
var _124=_123.HeaderRow;
if(!RadGridNamespace.IsChildOf(_121,_124)){
return;
}
this.ReorderIndicator1=document.createElement("span");
this.ReorderIndicator2=document.createElement("span");
this.ReorderIndicator1.innerHTML="&darr;";
this.ReorderIndicator2.innerHTML="&uarr;";
this.ReorderIndicator1.style.backgroundColor="transparent";
this.ReorderIndicator1.style.color="darkblue";
this.ReorderIndicator1.style.font="bold 18px Arial";
this.ReorderIndicator2.style.backgroundColor=this.ReorderIndicator1.style.backgroundColor;
this.ReorderIndicator2.style.color=this.ReorderIndicator1.style.color;
this.ReorderIndicator2.style.font=this.ReorderIndicator1.style.font;
this.ReorderIndicator1.style.top=RadGridNamespace.FindPosY(_121)-this.ReorderIndicator1.offsetHeight+"px";
this.ReorderIndicator1.style.left=RadGridNamespace.FindPosX(_121)+"px";
this.ReorderIndicator2.style.top=RadGridNamespace.FindPosY(_121)+_121.offsetHeight+"px";
this.ReorderIndicator2.style.left=this.ReorderIndicator1.style.left;
this.ReorderIndicator1.style.visibility="hidden";
this.ReorderIndicator1.style.display="none";
this.ReorderIndicator1.style.position="absolute";
this.ReorderIndicator2.style.visibility=this.ReorderIndicator1.style.visibility;
this.ReorderIndicator2.style.display=this.ReorderIndicator1.style.display;
this.ReorderIndicator2.style.position=this.ReorderIndicator1.style.position;
document.body.appendChild(this.ReorderIndicator1);
document.body.appendChild(this.ReorderIndicator2);
}
};
RadGridNamespace.RadGrid.prototype.DestroyReorderIndicators=function(){
if((this.ReorderIndicator1!=null)&&(this.ReorderIndicator2!=null)){
document.body.removeChild(this.ReorderIndicator1);
document.body.removeChild(this.ReorderIndicator2);
this.ReorderIndicator1=null;
this.ReorderIndicator2=null;
}
};
RadGridNamespace.RadGrid.prototype.MoveReorderIndicators=function(e,_126){
if((this.ReorderIndicator1!=null)&&(this.ReorderIndicator2!=null)){
this.ReorderIndicator1.style.visibility="visible";
this.ReorderIndicator1.style.display="";
this.ReorderIndicator2.style.visibility="visible";
this.ReorderIndicator2.style.display="";
this.ReorderIndicator1.style.top=RadGridNamespace.FindPosY(_126)-RadGridNamespace.FindScrollPosY(_126)+document.documentElement.scrollTop+document.body.scrollTop-_126.offsetHeight+"px";
this.ReorderIndicator1.style.left=RadGridNamespace.FindPosX(_126)-RadGridNamespace.FindScrollPosX(_126)+document.documentElement.scrollLeft+document.body.scrollLeft+"px";
if(parseInt(this.ReorderIndicator1.style.left)<RadGridNamespace.FindPosX(this.Control)){
this.ReorderIndicator1.style.left=RadGridNamespace.FindPosX(this.Control)+5;
}
this.ReorderIndicator2.style.top=parseInt(this.ReorderIndicator1.style.top)+_126.offsetHeight*2+"px";
this.ReorderIndicator2.style.left=this.ReorderIndicator1.style.left;
}
};
RadGridNamespace.RadGrid.prototype.AttachDomEvents=function(){
try{
this.AttachDomEvent(this.Control,"mousemove","OnMouseMove");
this.AttachDomEvent(document,"keydown","OnKeyDown");
this.AttachDomEvent(document,"keyup","OnKeyUp");
this.AttachDomEvent(this.Control,"click","OnClick");
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.OnMouseMove=function(e){
try{
var _128=RadGridNamespace.GetCurrentElement(e);
if(this.ClientSettings.Resizing.AllowRowResize){
this.DetectResizeCursorsOnRows(e,_128);
this.MoveRowResizer(e);
}
if((this.ClientSettings.AllowDragToGroup)||(this.ClientSettings.AllowColumnsReorder)){
this.HandleDragAndDrop(e,_128);
}
}
catch(error){
return false;
}
};
RadGridNamespace.RadGrid.prototype.OnKeyDown=function(e){
var _12a={KeyCode:e.keyCode,IsShiftPressed:e.shiftKey,IsCtrlPressed:e.ctrlKey,IsAltPressed:e.altKey,Event:e};
if(!RadGridNamespace.FireEvent(this,"OnKeyPress",[_12a])){
return;
}
if(e.keyCode==16){
this.IsShiftPressed=true;
}
if(e.keyCode==17){
this.IsCtrlPressed=true;
}
if(this.ClientSettings.AllowKeyboardNavigation){
this.ActiveRow.HandleActiveRow(e);
}
};
RadGridNamespace.RadGrid.prototype.OnClick=function(e){
};
RadGridNamespace.RadGrid.prototype.OnKeyUp=function(e){
if(e.keyCode==16){
this.IsShiftPressed=false;
}
if(e.keyCode==17){
this.IsCtrlPressed=false;
}
};
RadGridNamespace.RadGrid.prototype.DetectResizeCursorsOnRows=function(e,_12e){
try{
var _12f=this;
if((_12e!=null)&&(_12e.tagName.toLowerCase()=="td")){
var _130=_12e.parentNode.parentNode.parentNode;
var _131=this.GetTableObjectByID(_130.id);
if(_131!=null){
if(_131.Columns!=null){
if(_131.Columns[_12e.cellIndex].ColumnType!="GridRowIndicatorColumn"){
return;
}
}
if(!_131.Control.tBodies[0]){
return;
}
var _132=this.GetRowObjectByRealRow(_131,_12e.parentNode);
if(_132!=null){
var _133=RadGridNamespace.GetEventPosY(e);
var _134=RadGridNamespace.FindPosY(_12e);
var endY=_134+_12e.offsetHeight;
this.ResizeTolerance=5;
var _136=_12e.title;
if((_133>endY-this.ResizeTolerance)&&(_133<endY+this.ResizeTolerance)){
_12e.style.cursor="n-resize";
_12e.title=this.ClientSettings.ClientMessages.DragToResize;
this.AttachDomEvent(_12e,"mousedown","OnResizeMouseDown");
}else{
_12e.style.cursor="default";
_12e.title="";
this.DetachDomEvent(_12e,"mousedown","OnResizeMouseDown");
}
}
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.OnResizeMouseDown=function(e){
this.CreateRowResizer(e);
RadGridNamespace.ClearDocumentEvents();
this.AttachDomEvent(document,"mouseup","OnResizeMouseUp");
};
RadGridNamespace.RadGrid.prototype.OnResizeMouseUp=function(e){
this.DetachDomEvent(document,"mouseup","OnResizeMouseUp");
this.DestroyRowResizerAndResizeRow(e,true);
RadGridNamespace.RestoreDocumentEvents();
};
RadGridNamespace.RadGrid.prototype.CreateRowResizer=function(e){
try{
this.DestroyRowResizer();
var _13a=RadGridNamespace.GetCurrentElement(e);
if((_13a!=null)&&(_13a.tagName.toLowerCase()=="td")){
if(_13a.cellIndex>0){
var _13b=_13a.parentNode.rowIndex;
_13a=_13a.parentNode.parentNode.parentNode.rows[_13b].cells[0];
}
this.RowResizer=null;
this.CellToResize=_13a;
var _13c=_13a.parentNode.parentNode.parentNode;
var _13d=this.GetTableObjectByID(_13c.id);
this.RowResizer=document.createElement("div");
this.RowResizer.style.backgroundColor="navy";
this.RowResizer.style.height="1px";
this.RowResizer.style.fontSize="1";
this.RowResizer.style.position="absolute";
this.RowResizer.style.cursor="n-resize";
if(_13d!=null){
this.RowResizerRefTable=_13d;
if(this.GridDataDiv){
this.RowResizer.style.left=RadGridNamespace.FindPosX(this.GridDataDiv)+"px";
var _13e=(RadGridNamespace.FindPosX(this.GridDataDiv)+this.GridDataDiv.offsetWidth)-parseInt(this.RowResizer.style.left);
if(_13e>_13d.Control.offsetWidth){
this.RowResizer.style.width=_13d.Control.offsetWidth+"px";
}else{
this.RowResizer.style.width=_13e+"px";
}
if(parseInt(this.RowResizer.style.width)>this.GridDataDiv.offsetWidth){
this.RowResizer.style.width=this.GridDataDiv.offsetWidth+"px";
}
}else{
this.RowResizer.style.width=_13d.Control.offsetWidth+"px";
this.RowResizer.style.left=RadGridNamespace.FindPosX(_13a)+"px";
}
}
this.RowResizer.style.top=RadGridNamespace.GetEventPosY(e)-(RadGridNamespace.GetEventPosY(e)-e.clientY)+document.body.scrollTop+document.documentElement.scrollTop+"px";
var _13f=document.body;
_13f.appendChild(this.RowResizer);
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.DestroyRowResizerAndResizeRow=function(e,_141){
try{
if((this.CellToResize!="undefined")&&(this.CellToResize!=null)&&(this.CellToResize.tagName.toLowerCase()=="td")&&(this.RowResizer!="undefined")&&(this.RowResizer!=null)){
var _142;
if(this.GridDataDiv){
_142=parseInt(this.RowResizer.style.top)+this.GridDataDiv.scrollTop-(RadGridNamespace.FindPosY(this.CellToResize));
}else{
_142=parseInt(this.RowResizer.style.top)-(RadGridNamespace.FindPosY(this.CellToResize));
}
if(_142>0){
var _143=this.CellToResize.parentNode.parentNode.parentNode;
var _144=this.GetTableObjectByID(_143.id);
if(_144!=null){
_144.ResizeRow(this.CellToResize.parentNode.rowIndex,_142);
}
}
}
if(_141){
this.DestroyRowResizer();
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.DestroyRowResizer=function(){
try{
if((this.RowResizer!="undefined")&&(this.RowResizer!=null)&&(this.RowResizer.parentNode!=null)){
var _145=this.RowResizer.parentNode;
_145.removeChild(this.RowResizer);
this.RowResizer=null;
this.RowResizerRefTable=null;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.MoveRowResizer=function(e){
try{
if((this.RowResizer!="undefined")&&(this.RowResizer!=null)&&(this.RowResizer.parentNode!=null)){
this.RowResizer.style.top=RadGridNamespace.GetEventPosY(e)-(RadGridNamespace.GetEventPosY(e)-e.clientY)+document.body.scrollTop+document.documentElement.scrollTop+"px";
if(this.ClientSettings.Resizing.EnableRealTimeResize){
this.DestroyRowResizerAndResizeRow(e,false);
this.UpdateRowResizerWidth(e);
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.UpdateRowResizerWidth=function(e){
var _148=RadGridNamespace.GetCurrentElement(e);
if((_148!=null)&&(_148.tagName.toLowerCase()=="td")){
var _149=this.RowResizerRefTable;
if(_149!=null){
if(this.GridDataDiv){
var _14a=(RadGridNamespace.FindPosX(this.GridDataDiv)+this.GridDataDiv.offsetWidth)-parseInt(this.RowResizer.style.left);
if(_14a>_149.Control.offsetWidth){
this.RowResizer.style.width=_149.Control.offsetWidth+"px";
}else{
this.RowResizer.style.width=_14a+"px";
}
if(parseInt(this.RowResizer.style.width)>this.GridDataDiv.offsetWidth){
this.RowResizer.style.width=this.GridDataDiv.offsetWidth+"px";
}
}else{
this.RowResizer.style.width=_149.Control.offsetWidth+"px";
}
}
}
};
RadGridNamespace.RadGrid.prototype.SetHeaderAndFooterDivsWidth=function(){
if((document.compatMode=="BackCompat"&&navigator.userAgent.toLowerCase().indexOf("msie")!=-1)||(navigator.userAgent.toLowerCase().indexOf("msie")!=-1&&navigator.userAgent.toLowerCase().indexOf("6.0")!=-1)){
if(this.ClientSettings.Scrolling.UseStaticHeaders){
if(this.GridHeaderDiv!=null&&this.GridDataDiv!=null&&this.GridHeaderDiv!=null){
this.GridHeaderDiv.style.width="100%";
if(this.GridHeaderDiv&&this.GridDataDiv){
if(this.GridDataDiv.offsetWidth>0){
this.GridHeaderDiv.style.width=this.GridDataDiv.offsetWidth-RadGridNamespace.GetScrollBarWidth()+"px";
}
}
if(this.GridHeaderDiv&&this.GridFooterDiv){
this.GridFooterDiv.style.width=this.GridHeaderDiv.style.width;
}
}
}
}
if(this.ClientSettings.Scrolling.AllowScroll&&this.ClientSettings.Scrolling.UseStaticHeaders){
var _14b=RadGridNamespace.IsParentRightToLeft(this.GridHeaderDiv);
if((!_14b&&this.GridHeaderDiv&&parseInt(this.GridHeaderDiv.style.marginRight)!=RadGridNamespace.GetScrollBarWidth())||(_14b&&this.GridHeaderDiv&&parseInt(this.GridHeaderDiv.style.marginLeft)!=RadGridNamespace.GetScrollBarWidth())){
if(!_14b){
this.GridHeaderDiv.style.marginRight=RadGridNamespace.GetScrollBarWidth()+"px";
this.GridHeaderDiv.style.marginLeft="";
}else{
this.GridHeaderDiv.style.marginLeft=RadGridNamespace.GetScrollBarWidth()+"px";
this.GridHeaderDiv.style.marginRight="";
}
}
if(this.GridHeaderDiv&&this.GridDataDiv){
if(navigator.userAgent.toLowerCase().indexOf("msie")!=-1&&navigator.userAgent.toLowerCase().indexOf("6.0")!=-1){
this.GridHeaderDiv.style.width="100%";
if(!_14b){
this.GridHeaderDiv.style.marginRight="";
}else{
this.GridHeaderDiv.style.marginLeft="";
}
}else{
if((this.GridDataDiv.clientWidth==this.GridDataDiv.offsetWidth)){
this.GridHeaderDiv.style.width="100%";
if(!_14b){
this.GridHeaderDiv.style.marginRight="";
}else{
this.GridHeaderDiv.style.marginLeft="";
}
}
}
}
if(this.GroupPanelObject&&this.GroupPanelObject.Items.length>0&&navigator.userAgent.toLowerCase().indexOf("msie")!=-1){
if(this.MasterTableView&&this.MasterTableViewHeader){
this.MasterTableView.Control.style.width=this.MasterTableViewHeader.Control.offsetWidth+"px";
}
}
if(this.GridFooterDiv){
this.GridFooterDiv.style.marginRight=this.GridHeaderDiv.style.marginRight;
this.GridFooterDiv.style.marginLeft=this.GridHeaderDiv.style.marginLeft;
this.GridFooterDiv.style.width=this.GridHeaderDiv.style.width;
}
}
};
RadGridNamespace.RadGrid.prototype.SetDataDivHeight=function(){
if(this.GridDataDiv&&this.Control.style.height!=""){
this.GridDataDiv.style.height="10px";
var _14c=0;
if(this.GroupPanelObject){
_14c+=this.GroupPanelObject.Control.offsetHeight;
}
if(this.GridHeaderDiv){
_14c+=this.GridHeaderDiv.offsetHeight;
}
if(this.GridFooterDiv){
_14c+=this.GridFooterDiv.offsetHeight;
}
if(this.PagerControl){
_14c+=this.PagerControl.offsetHeight;
}
if(this.TopPagerControl){
_14c+=this.TopPagerControl.offsetHeight;
}
var _14d=this.Control.clientHeight-_14c;
if(_14d>0){
var _14e=this.Control.style.position;
if(window.netscape){
this.Control.style.position="absolute";
}
this.GridDataDiv.style.height=_14d+"px";
if(window.netscape){
this.Control.style.position=_14e;
}
}
}
};
RadGridNamespace.RadGrid.prototype.InitializeDimensions=function(){
try{
var _14f=this;
this.InitializeAutoLayout();
if(!this.EnableAJAX){
this.OnWindowResize();
}else{
var _150=function(){
_14f.OnWindowResize();
};
if(window.netscape&&!window.opera){
_150();
}else{
setTimeout(_150,0);
}
}
this.Control.RadResize=function(){
_14f.OnWindowResize();
};
if(navigator.userAgent.toLowerCase().indexOf("msie")!=-1){
setTimeout(function(){
_14f.AttachDomEvent(window,"resize","OnWindowResize");
},0);
}else{
this.AttachDomEvent(window,"resize","OnWindowResize");
}
this.Control.RadShow=function(){
_14f.OnWindowResize();
};
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.OnWindowResize=function(e){
this.SetHeaderAndFooterDivsWidth();
this.SetDataDivHeight();
};
RadGridNamespace.RadGrid.prototype.InitializeAutoLayout=function(){
if(this.ClientSettings.Scrolling.AllowScroll&&this.ClientSettings.Scrolling.UseStaticHeaders){
if(this.MasterTableView&&this.MasterTableViewHeader){
if(this.MasterTableView.TableLayout!="Auto"||window.netscape||window.opera){
return;
}
this.MasterTableView.Control.style.tableLayout=this.MasterTableViewHeader.Control.style.tableLayout="";
var _152=this.MasterTableView.Control.tBodies[0].rows[this.ClientSettings.FirstDataRowClientRowIndex];
for(var i=0;i<this.MasterTableViewHeader.HeaderRow.cells.length;i++){
var col=this.MasterTableViewHeader.ColGroup.Cols[i];
if(!col){
continue;
}
if(col.width!=""){
continue;
}
var _155=this.MasterTableViewHeader.HeaderRow.cells[i].offsetWidth;
var _156=_152.cells[i].offsetWidth;
var _157=(_155>_156)?_155:_156;
if(this.MasterTableViewFooter&&this.MasterTableViewFooter.Control){
if(this.MasterTableViewFooter.Control.tBodies[0].rows[0]&&this.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[i]){
if(this.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[i].offsetWidth>_157){
_157=this.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[i].offsetWidth;
}
}
}
if(_157<=0){
continue;
}
this.MasterTableViewHeader.HeaderRow.cells[i].style.width=_152.cells[i].style.width=this.MasterTableView.ColGroup.Cols[i].width=col.width=_157;
if(this.MasterTableViewFooter&&this.MasterTableViewFooter.Control){
if(this.MasterTableViewFooter.Control.tBodies[0].rows[0]&&this.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[i]){
this.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[i].style.width=_157;
}
}
}
this.MasterTableView.Control.style.tableLayout=this.MasterTableViewHeader.Control.style.tableLayout="fixed";
if(this.MasterTableViewFooter&&this.MasterTableViewFooter.Control){
this.MasterTableViewFooter.Control.style.tableLayout="fixed";
}
if(window.netscape){
this.OnWindowResize();
}
}
}
};
RadGridNamespace.RadGrid.prototype.InitializeSaveScrollPosition=function(){
if(!this.ClientSettings.Scrolling.SaveScrollPosition||this.ClientSettings.Scrolling.EnableAJAXScrollPaging){
return;
}
if(this.ClientSettings.Scrolling.ScrollTop!=""){
this.GridDataDiv.scrollTop=this.ClientSettings.Scrolling.ScrollTop;
}
if(this.ClientSettings.Scrolling.ScrollLeft!=""){
if(this.GridHeaderDiv){
this.GridHeaderDiv.scrollLeft=this.ClientSettings.Scrolling.ScrollLeft;
}
if(this.GridFooterDiv){
this.GridFooterDiv.scrollLeft=this.ClientSettings.Scrolling.ScrollLeft;
}
this.GridDataDiv.scrollLeft=this.ClientSettings.Scrolling.ScrollLeft;
}
};
RadGridNamespace.RadGrid.prototype.InitializeAjaxScrollPaging=function(){
if(!this.ClientSettings.Scrolling.EnableAJAXScrollPaging){
return;
}
this.ScrollCounter=0;
this.CurrentAJAXScrollTop=0;
if(this.ClientSettings.Scrolling.AJAXScrollTop!=""){
this.CurrentAJAXScrollTop=this.ClientSettings.Scrolling.AJAXScrollTop;
}
var _158=this.CurrentPageIndex*this.MasterTableView.PageSize*20;
var _159=this.MasterTableView.PageCount*this.MasterTableView.PageSize*20;
var _15a=this.MasterTableView.Control;
var _15b=_15a.offsetHeight;
if(!window.opera){
_15a.style.marginTop=_158+"px";
_15a.style.marginBottom=_159-_158-_15b+"px";
}else{
_15a.style.position="relative";
_15a.style.top=_158+"px";
_15a.style.marginBottom=_159-_15b+"px";
}
this.CurrentAJAXScrollTop=_158;
this.GridDataDiv.scrollTop=_158;
this.CreateScrollerToolTip();
this.AttachDomEvent(this.GridDataDiv,"scroll","OnAJAXScroll");
};
RadGridNamespace.RadGrid.prototype.CreateScrollerToolTip=function(){
var _15c=document.getElementById(this.ClientID+"ScrollerToolTip");
if(!_15c){
this.ScrollerToolTip=document.createElement("span");
this.ScrollerToolTip.id=this.ClientID+"ScrollerToolTip";
this.ScrollerToolTip.style.backgroundColor="#F5F5DC";
this.ScrollerToolTip.style.border="1px solid";
this.ScrollerToolTip.style.position="absolute";
this.ScrollerToolTip.style.display="none";
this.ScrollerToolTip.style.font="icon";
this.ScrollerToolTip.style.padding="2";
document.body.appendChild(this.ScrollerToolTip);
}
};
RadGridNamespace.RadGrid.prototype.HideScrollerToolTip=function(){
var _15d=this;
setTimeout(function(){
var _15e=document.getElementById(_15d.ClientID+"ScrollerToolTip");
if(_15e&&_15e.parentNode){
_15e.style.display="none";
}
},200);
};
RadGridNamespace.RadGrid.prototype.ShowScrollerTooltip=function(_15f,_160){
var _161=document.getElementById(this.ClientID+"ScrollerToolTip");
if(_161){
_161.style.display="";
_161.style.top=parseInt(RadGridNamespace.FindPosY(this.GridDataDiv))+Math.round(this.GridDataDiv.offsetHeight*_15f)+"px";
_161.style.left=parseInt(RadGridNamespace.FindPosX(this.GridDataDiv))+this.GridDataDiv.offsetWidth-(this.GridDataDiv.offsetWidth-this.GridDataDiv.clientWidth)-_161.offsetWidth+"px";
_161.innerHTML="Page: <b>"+((_160==0)?1:_160+1)+"</b> out of <b>"+this.MasterTableView.PageCount+"</b> pages";
}
};
RadGridNamespace.RadGrid.prototype.InitializeScroll=function(){
var _162=this;
var grid=this;
var _164=function(){
grid.InitializeSaveScrollPosition();
};
if(window.netscape&&!window.opera){
window.setTimeout(_164,0);
}else{
_164();
}
this.InitializeAjaxScrollPaging();
this.AttachDomEvent(this.GridDataDiv,"scroll","OnGridScroll");
};
RadGridNamespace.RadGrid.prototype.OnGridScroll=function(e){
if(this.ClientSettings.Scrolling.UseStaticHeaders){
if(this.GridHeaderDiv){
this.GridHeaderDiv.scrollLeft=this.GridDataDiv.scrollLeft;
}
if(this.GridFooterDiv){
this.GridFooterDiv.scrollLeft=this.GridDataDiv.scrollLeft;
}
}
this.SavePostData("ScrolledControl",this.ClientID,this.GridDataDiv.scrollTop,this.GridDataDiv.scrollLeft);
var evt={};
evt.ScrollTop=this.GridDataDiv.scrollTop;
evt.ScrollLeft=this.GridDataDiv.scrollLeft;
evt.ScrollControl=this.GridDataDiv;
evt.IsOnTop=(this.GridDataDiv.scrollTop==0)?true:false;
evt.IsOnBottom=((this.GridDataDiv.scrollHeight-this.GridDataDiv.offsetHeight+16)==this.GridDataDiv.scrollTop)?true:false;
RadGridNamespace.FireEvent(this,"OnScroll",[evt]);
};
RadGridNamespace.RadGrid.prototype.OnAJAXScroll=function(e){
if(this.GridDataDiv){
this.CurrentScrollTop=this.GridDataDiv.scrollTop;
}
this.ScrollCounter++;
var _168=this;
RadGridNamespace.AJAXScrollHanlder=function(_169){
if(_168.ScrollCounter!=_169){
return;
}
if(_168.CurrentAJAXScrollTop!=_168.GridDataDiv.scrollTop){
if(_168.CurrentPageIndex==_16a){
return;
}
var _16b=_168.ClientID;
var _16c=_168.MasterTableView.ClientID;
_168.SavePostData("AJAXScrolledControl",_168.GridDataDiv.scrollLeft,_168.LastScrollTop,_168.GridDataDiv.scrollTop,_16a);
var _16d=_168.ClientSettings.PostBackFunction;
_16d=_16d.replace("{0}",_168.UniqueID);
eval(_16d);
}
_168.ScrollCounter=0;
_168.HideScrollerToolTip();
};
var evt={};
evt.ScrollTop=this.GridDataDiv.scrollTop;
evt.ScrollLeft=this.GridDataDiv.scrollLeft;
evt.ScrollControl=this.GridDataDiv;
evt.IsOnTop=(this.GridDataDiv.scrollTop==0)?true:false;
evt.IsOnBottom=((this.GridDataDiv.scrollHeight-this.GridDataDiv.offsetHeight+16)==this.GridDataDiv.scrollTop)?true:false;
RadGridNamespace.FireEvent(this,"OnScroll",[evt]);
var _16f=this.GridDataDiv.scrollTop/(this.GridDataDiv.scrollHeight-this.GridDataDiv.offsetHeight+16);
var _16a=Math.round((this.MasterTableView.PageCount-1)*_16f);
setTimeout("RadGridNamespace.AJAXScrollHanlder("+this.ScrollCounter+")",500);
this.ShowScrollerTooltip(_16f,_16a);
};
RadGridNamespace.RadGridTable=function(_170){
if((!_170)||typeof (_170)!="object"){
return;
}
for(var _171 in _170){
this[_171]=_170[_171];
}
this.Type="RadGridTable";
this.ServerID=this.ID;
this.SelectedRows=new Array();
this.SelectedCells=new Array();
this.SelectedColumns=new Array();
this.ExpandCollapseColumns=new Array();
this.GroupSplitterColumns=new Array();
this.HeaderRow=null;
};
RadGridNamespace.RadGridTable.prototype._constructor=function(_172){
if((!_172)||typeof (_172)!="object"){
return;
}
this.Control=document.getElementById(this.ClientID);
if(!this.Control){
return;
}
this.ColGroup=RadGridNamespace.GetTableColGroup(this.Control);
if(!this.ColGroup){
return;
}
this.ColGroup.Cols=RadGridNamespace.GetTableColGroupCols(this.ColGroup);
this.Owner=_172;
this.InitializeEvents(this.Owner.ClientSettings.ClientEvents);
this.Control.style.overflow=((this.Owner.ClientSettings.Resizing.ClipCellContentOnResize&&((this.Owner.ClientSettings.Resizing.AllowColumnResize)||(this.Owner.ClientSettings.Resizing.AllowRowResize)))||(this.Owner.ClientSettings.Scrolling.AllowScroll&&this.Owner.ClientSettings.Scrolling.UseStaticHeaders))?"hidden":"";
if(navigator.userAgent.toLowerCase().indexOf("msie")!=-1&&this.Control.style.tableLayout=="fixed"&&this.Control.style.width.indexOf("%")!=-1){
this.Control.style.width="";
}
this.CreateStyles();
if(this.Owner.ClientSettings.Scrolling.AllowScroll&&this.Owner.ClientSettings.Scrolling.UseStaticHeaders){
if(this.ClientID.indexOf("_Header")!=-1||this.ClientID.indexOf("_Detail")!=-1){
this.Columns=this.GetTableColumns(this.Control,this.RenderColumns);
}else{
this.Columns=this.Owner.MasterTableViewHeader.Columns;
this.ExpandCollapseColumns=this.Owner.MasterTableViewHeader.ExpandCollapseColumns;
this.GroupSplitterColumns=this.Owner.MasterTableViewHeader.GroupSplitterColumns;
}
}else{
this.Columns=this.GetTableColumns(this.Control,this.RenderColumns);
}
if(this.Owner.ClientSettings.ShouldCreateRows){
this.InitializeRows(this.Controls[0].Rows);
}
};
RadGridNamespace.RadGridTable.prototype.Dispose=function(){
if(this.ColGroup&&this.ColGroup.Cols){
this.ColGroup.Cols=null;
this.ColGroup=null;
}
this.Owner=null;
this.DisposeEvents();
this.ExpandCollapseColumns=null;
this.GroupSplitterColumns=null;
this.DisposeRows();
this.DisposeColumns();
this.RenderColumns=null;
this.SelectedRows=null;
this.ExpandCollapseColumns=null;
this.DetailTables=null;
this.DetailTablesCollection=null;
this.Control=null;
this.HeaderRow=null;
};
RadGridNamespace.RadGridTable.prototype.CreateStyles=function(){
if(!this.SelectedItemStyleClass||this.SelectedItemStyleClass==""){
if(this.SelectedItemStyle&&this.SelectedItemStyle!=""){
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".SelectedItemStyle"+this.ClientID+"1 td",this.SelectedItemStyle);
}else{
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".SelectedItemStyle"+this.ClientID+"2 td","background-color:Navy;color:White;");
}
}
var _173=((this.Owner.ClientSettings.Resizing.ClipCellContentOnResize&&((this.Owner.ClientSettings.Resizing.AllowColumnResize)||(this.Owner.ClientSettings.Resizing.AllowRowResize)))||(this.Owner.ClientSettings.Scrolling.AllowScroll&&this.Owner.ClientSettings.Scrolling.UseStaticHeaders))?"hidden":"";
_173="hidden";
if(_173=="hidden"){
RadGridNamespace.addClassName(this.Control,"grid"+this.ClientID);
if(window.netscape){
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".grid"+this.ClientID+" td","overflow: hidden;-moz-user-select:-moz-none;");
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".grid"+this.ClientID+" th","overflow: hidden;-moz-user-select:-moz-none;");
}else{
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".grid"+this.ClientID+" td","overflow: hidden; text-overflow: ellipsis;");
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".grid"+this.ClientID+" th","overflow: hidden; text-overflow: ellipsis;");
}
}
};
RadGridNamespace.RadGridTable.prototype.InitializeEvents=function(_174){
for(clientEvent in _174){
if(typeof (_174[clientEvent])!="string"){
continue;
}
if(!this.Owner.IsClientEventName(clientEvent)){
if(_174[clientEvent]!=""){
var _175=_174[clientEvent];
if(_175.indexOf("(")!=-1){
this[clientEvent]=_175;
}else{
this[clientEvent]=eval(_175);
}
}else{
this[clientEvent]=null;
}
}
}
};
RadGridNamespace.RadGridTable.prototype.DisposeEvents=function(){
for(var _176 in RadGridNamespace.RadGridTable.ClientEventNames){
this[_176]=null;
}
};
RadGridNamespace.RadGridTable.prototype.InitializeRows=function(rows){
if(this.ClientID.indexOf("_Header")!=-1||this.ClientID.indexOf("_Footer")!=-1){
return;
}
try{
var _178=[];
for(var i=0;i<rows.length;i++){
if(!rows[i].Visible||rows[i].ClientRowIndex<0){
continue;
}
if(rows[i].ItemType=="THead"||rows[i].ItemType=="TFoot"){
continue;
}
RadGridNamespace.FireEvent(this,"OnRowCreating");
rows[i]._constructor(this);
_178[_178.length]=rows[i];
RadGridNamespace.FireEvent(this,"OnRowCreated",[rows[i]]);
}
this.Rows=_178;
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.DisposeRows=function(){
if(this.Rows!=null){
for(var i=0;i<this.Rows.length;i++){
var row=this.Rows[i];
row.Dispose();
}
this.Rows=null;
}
};
RadGridNamespace.RadGridTable.prototype.DisposeColumns=function(){
if(this.Columns!=null){
for(var i=0;i<this.Columns.length;i++){
var _17d=this.Columns[i];
_17d.Dispose();
}
this.Columns=null;
}
};
RadGridNamespace.RadGridTable.prototype.GetTableRows=function(_17e,_17f){
if(this.ClientID.indexOf("_Header")!=-1||this.ClientID.indexOf("_Footer")!=-1){
return;
}
try{
var _180=new Array();
var j=0;
for(var i=0;i<_17f.length;i++){
if((_17f[i].ItemType=="THead")||(_17f[i].ItemType=="TFoot")){
continue;
}
if((_17f[i])&&(_17f[i].Visible)){
RadGridNamespace.FireEvent(this,"OnRowCreating");
_180[_180.length]=_17f[i]._constructor(this);
RadGridNamespace.FireEvent(this,"OnRowCreated",[_180[j]]);
j++;
}
}
return _180;
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.GetTableHeaderRow=function(){
try{
if(this.Control.tHead){
for(var i=0;i<this.Control.tHead.rows.length;i++){
if(this.Control.tHead.rows[i]!=null){
if(this.Control.tHead.rows[i].cells[0]!=null){
if(this.Control.tHead.rows[i].cells[0].tagName!=null){
if(this.Control.tHead.rows[i].cells[0].tagName.toLowerCase()=="th"){
this.HeaderRow=this.Control.tHead.rows[i];
break;
}
}
}
}
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.GetTableColumns=function(_184,_185){
try{
this.GetTableHeaderRow();
var _186=new Array();
if(!this.HeaderRow){
return;
}
if(!this.HeaderRow.cells[0]){
return;
}
var j=0;
for(var i=0;i<_185.length;i++){
if(_185[i].Visible){
RadGridNamespace.FireEvent(this,"OnColumnCreating");
_186[_186.length]=new RadGridNamespace.RadGridTableColumn(_185[i]);
_186[j]._constructor(this.HeaderRow.cells[j],this);
_186[j].RealIndex=i;
if(_185[i].ColumnType=="GridExpandColumn"){
this.ExpandCollapseColumns[this.ExpandCollapseColumns.length]=_186[j];
}
if(_185[i].ColumnType=="GridGroupSplitterColumn"){
this.GroupSplitterColumns[this.GroupSplitterColumns.length]=_186[j];
}
RadGridNamespace.FireEvent(this,"OnColumnCreated",[_186[j]]);
j++;
}
}
return _186;
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.RemoveTableLayOut=function(){
this.masterTableLayOut=this.Owner.MasterTableView.Control.style.tableLayout;
this.detailTablesTableLayOut=new Array();
for(var i=0;i<this.Owner.DetailTablesCollection.length;i++){
this.detailTablesTableLayOut[this.detailTablesTableLayOut.length]=this.Owner.DetailTablesCollection[i].Control.style.tableLayout;
this.Owner.DetailTablesCollection[i].Control.style.tableLayout="";
}
};
RadGridNamespace.RadGridTable.prototype.RestoreTableLayOut=function(){
this.Owner.MasterTableView.Control.style.tableLayout=this.masterTableLayOut;
for(var i=0;i<this.Owner.DetailTablesCollection.length;i++){
this.Owner.DetailTablesCollection[i].Control.style.tableLayout=this.detailTablesTableLayOut[i];
}
};
RadGridNamespace.RadGridTable.prototype.SelectRow=function(row,_18c){
try{
if(!this.Owner.ClientSettings.Selecting.AllowRowSelect){
return;
}
var _18d=this.Owner.GetRowObjectByRealRow(this,row);
if(_18d!=null){
if(_18d.ItemType=="Item"||_18d.ItemType=="AlternatingItem"){
_18d.SetSelected(_18c);
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.DeselectRow=function(row){
try{
if(!this.Owner.ClientSettings.Selecting.AllowRowSelect){
return;
}
var _18f=this.Owner.GetRowObjectByRealRow(this,row);
if(_18f!=null){
if(_18f.ItemType=="Item"||_18f.ItemType=="AlternatingItem"){
this.RemoveFromSelectedRows(_18f);
_18f.RemoveSelectedRowStyle();
_18f.Selected=false;
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ResizeRow=function(_190,_191,_192){
try{
if(!this.Owner.ClientSettings.Resizing.AllowRowResize){
return;
}
if(!RadGridNamespace.FireEvent(this,"OnRowResizing",[_190,_191])){
return;
}
this.RemoveTableLayOut();
var _193=this.Control.style.tableLayout;
this.Control.style.tableLayout="";
var _194=this.Control.parentNode.parentNode.parentNode.parentNode;
var _195=this.Owner.GetTableObjectByID(_194.id);
var _196;
if(_195!=null){
_196=_195.Control.style.tableLayout;
_195.Control.style.tableLayout="";
}
if(!_192){
if(this.Control){
if(this.Control.rows[_190]){
if(this.Control.rows[_190].cells[0]){
this.Control.rows[_190].cells[0].style.height=_191+"px";
this.Control.rows[_190].style.height=_191+"px";
}
}
}
}else{
if(this.Control){
if(this.Control.tBodies[0]){
if(this.Control.tBodies[0].rows[_190]){
if(this.Control.tBodies[0].rows[_190].cells[0]){
this.Control.tBodies[0].rows[_190].cells[0].style.height=_191+"px";
this.Control.tBodies[0].rows[_190].style.height=_191+"px";
}
}
}
}
}
this.Control.style.tableLayout=_193;
if(_195!=null){
_195.Control.style.tableLayout=_196;
}
this.RestoreTableLayOut();
var _197=this.Owner.GetRowObjectByRealRow(this,this.Control.rows[_190]);
this.Owner.SavePostData("ResizedRows",this.Control.id,_197.RealIndex,_191+"px");
RadGridNamespace.FireEvent(this,"OnRowResized",[_190,_191]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ResizeColumn=function(_198,_199){
if(isNaN(parseInt(_198))){
var _19a="Column index must be of type \"Number\"!";
alert(_19a);
return;
}
if(isNaN(parseInt(_199))){
var _19a="Column width must be of type \"Number\"!";
alert(_19a);
return;
}
if(_198<0){
var _19a="Column index must be non-negative!";
alert(_19a);
return;
}
if(_199<0){
var _19a="Column width must be non-negative!";
alert(_19a);
return;
}
if(_198>(this.Columns.length-1)){
var _19a="Column index must be less than columns count!";
alert(_19a);
return;
}
if(!this.Owner.ClientSettings.Resizing.AllowColumnResize){
return;
}
if(!this.Columns){
return;
}
if(!this.Columns[_198].Resizable){
return;
}
if(!RadGridNamespace.FireEvent(this,"OnColumnResizing",[_198,_199])){
return;
}
try{
if(this==this.Owner.MasterTableView&&this.Owner.MasterTableViewHeader){
this.Owner.MasterTableViewHeader.ResizeColumn(_198,_199);
}
var _19b=this.Control.clientWidth;
var _19c=this.Owner.Control.clientWidth;
if(this.HeaderRow){
var _19d=this.HeaderRow.cells[_198].scrollWidth-_199;
}
if(window.netscape||window.opera){
if(this.HeaderRow){
if(this.HeaderRow.cells[_198]){
this.HeaderRow.cells[_198].style.width=_199+"px";
}
}
if(this==this.Owner.MasterTableViewHeader){
var _19e=this.Owner.MasterTableView.Control.tBodies[0].rows[this.Owner.ClientSettings.FirstDataRowClientRowIndex];
if(_19e){
if(_19e.cells[_198]){
_19e.cells[_198].style.width=_199+"px";
}
}
if(this.Owner.MasterTableViewFooter&&this.Owner.MasterTableViewFooter.Control){
if(this.Owner.MasterTableViewFooter.Control.tBodies[0].rows[0]&&this.Owner.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[_198]){
if(_199>0){
this.Owner.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[_198].style.width=_199+"px";
}
}
}
}
}
if(this.ColGroup){
if(this.ColGroup.Cols[_198]){
if(_199>0){
this.ColGroup.Cols[_198].width=_199+"px";
}
}
}
if(this==this.Owner.MasterTableViewHeader){
if(this.Owner.MasterTableView.ColGroup){
if(this.Owner.MasterTableView.ColGroup.Cols[_198]){
if(_199>0){
this.Owner.MasterTableView.ColGroup.Cols[_198].width=_199+"px";
}
}
}
if(this.Owner.MasterTableViewFooter&&this.Owner.MasterTableViewFooter.ColGroup){
if(this.Owner.MasterTableViewFooter.ColGroup.Cols[_198]){
if(_199>0){
this.Owner.MasterTableViewFooter.ColGroup.Cols[_198].width=_199+"px";
}
}
}
}
if(this==this.Owner.MasterTableView||this==this.Owner.MasterTableViewHeader){
this.Owner.SavePostData("ResizedColumns",this.Owner.MasterTableView.ClientID,this.Columns[_198].RealIndex,_199+"px");
}else{
this.Owner.SavePostData("ResizedColumns",this.ClientID,this.Columns[_198].RealIndex,_199+"px");
}
if(this.Owner.ClientSettings.Resizing.ResizeGridOnColumnResize){
if(this==this.Owner.MasterTableViewHeader){
for(var i=0;i<this.ColGroup.Cols.length;i++){
if(i!=_198&&this.ColGroup.Cols[i].width==""){
this.ColGroup.Cols[i].width=this.HeaderRow.cells[i].scrollWidth+"px";
this.Owner.MasterTableView.ColGroup.Cols[i].width=this.ColGroup.Cols[i].width;
if(this.Owner.MasterTableViewFooter&&this.Owner.MasterTableViewFooter.ColGroup){
this.Owner.MasterTableViewFooter.ColGroup.Cols[i].width=this.ColGroup.Cols[i].width;
}
}
}
this.Control.style.width=(this.Control.offsetWidth-_19d)+"px";
this.Owner.MasterTableView.Control.style.width=this.Control.style.width;
if(this.Owner.MasterTableViewFooter&&this.Owner.MasterTableViewFooter.Control){
this.Owner.MasterTableViewFooter.Control.style.width=this.Control.style.width;
}
var _1a0=(this.Control.scrollWidth>this.Control.offsetWidth)?this.Control.scrollWidth:this.Control.offsetWidth;
var _1a1=this.Owner.GridDataDiv.offsetWidth;
this.Owner.SavePostData("ResizedControl",this.ClientID,_1a0+"px",_1a1+"px",this.Owner.Control.offsetHeight+"px");
}else{
this.Control.style.width=(this.Control.offsetWidth-_19d)+"px";
this.Owner.Control.style.width=this.Control.style.width;
var _1a0=(this.Control.scrollWidth>this.Control.offsetWidth)?this.Control.scrollWidth:this.Control.offsetWidth;
this.Owner.SavePostData("ResizedControl",this.ClientID,_1a0+"px",this.Owner.Control.offsetWidth+"px",this.Owner.Control.offsetHeight+"px");
}
}
if(this.Owner.GroupPanelObject&&this.Owner.GroupPanelObject.Items.length>0&&navigator.userAgent.toLowerCase().indexOf("msie")!=-1){
if(this.Owner.MasterTableView&&this.Owner.MasterTableViewHeader){
this.Owner.MasterTableView.Control.style.width=this.Owner.MasterTableViewHeader.Control.offsetWidth+"px";
}
}
RadGridNamespace.FireEvent(this,"OnColumnResized",[_198,_199]);
if(window.netscape){
this.Control.style.cssText=this.Control.style.cssText;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ReorderColumns=function(_1a2,_1a3){
if(isNaN(parseInt(_1a2))){
var _1a4="First column index must be of type \"Number\"!";
alert(_1a4);
return;
}
if(isNaN(parseInt(_1a3))){
var _1a4="Second column index must be of type \"Number\"!";
alert(_1a4);
return;
}
if(_1a2<0){
var _1a4="First column index must be non-negative!";
alert(_1a4);
return;
}
if(_1a3<0){
var _1a4="Second column index must be non-negative!";
alert(_1a4);
return;
}
if(_1a2>(this.Columns.length-1)){
var _1a4="First column index must be less than columns count!";
alert(_1a4);
return;
}
if(_1a3>(this.Columns.length-1)){
var _1a4="Second column index must be less than columns count!";
alert(_1a4);
return;
}
if(!this.Owner.ClientSettings.AllowColumnsReorder){
return;
}
if(!this.Columns){
return;
}
if(!this.Columns[_1a2].Reorderable){
return;
}
if(!this.Columns[_1a3].Reorderable){
return;
}
this.SwapColumns(_1a2,_1a3);
if((!this.Owner.ClientSettings.ReorderColumnsOnClient)&&(this.Owner.ClientSettings.PostBackReferences.PostBackColumnsReorder!="")){
if(this==this.Owner.MasterTableView){
eval(this.Owner.ClientSettings.PostBackReferences.PostBackColumnsReorder);
}
}
};
RadGridNamespace.RadGridTable.prototype.SwapColumns=function(_1a5,_1a6,_1a7){
if(isNaN(parseInt(_1a5))){
var _1a8="First column index must be of type \"Number\"!";
alert(_1a8);
return;
}
if(isNaN(parseInt(_1a6))){
var _1a8="Second column index must be of type \"Number\"!";
alert(_1a8);
return;
}
if(_1a5<0){
var _1a8="First column index must be non-negative!";
alert(_1a8);
return;
}
if(_1a6<0){
var _1a8="Second column index must be non-negative!";
alert(_1a8);
return;
}
if(_1a5>(this.Columns.length-1)){
var _1a8="First column index must be less than columns count!";
alert(_1a8);
return;
}
if(_1a6>(this.Columns.length-1)){
var _1a8="Second column index must be less than columns count!";
alert(_1a8);
return;
}
if(!this.Owner.ClientSettings.AllowColumnsReorder){
return;
}
if(!this.Columns){
return;
}
if(!this.Columns[_1a5].Reorderable){
return;
}
if(!this.Columns[_1a6].Reorderable){
return;
}
try{
if(this==this.Owner.MasterTableView&&this.Owner.MasterTableViewHeader){
this.Owner.MasterTableViewHeader.SwapColumns(_1a5,_1a6,!this.Owner.ClientSettings.ReorderColumnsOnClient);
return;
}
if(typeof (_1a7)=="undefined"){
_1a7=true;
}
if(this.Owner.ClientSettings.ColumnsReorderMethod=="Reorder"){
if(_1a6>_1a5){
while(_1a5+1<_1a6){
this.SwapColumns(_1a6-1,_1a6,false);
_1a6--;
}
}else{
while(_1a6<_1a5-1){
this.SwapColumns(_1a6+1,_1a6,false);
_1a6++;
}
}
}
if(!RadGridNamespace.FireEvent(this,"OnColumnSwapping",[_1a5,_1a6])){
return;
}
var _1a9=this.Control;
var _1aa=this.Columns[_1a5];
var _1ab=this.Columns[_1a6];
this.Columns[_1a5]=_1ab;
this.Columns[_1a6]=_1aa;
var _1ac=this.ColGroup.Cols[_1a5].width;
if(_1ac==""&&this.HeaderRow){
_1ac=this.HeaderRow.cells[_1a5].offsetWidth;
}
var _1ad=this.ColGroup.Cols[_1a6].width;
if(_1ad==""&&this.HeaderRow){
_1ad=this.HeaderRow.cells[_1a6].offsetWidth;
}
var _1ae=this.Owner.ClientSettings.Resizing.AllowColumnResize;
var _1af=(typeof (this.Columns[_1a5].Resizable)=="boolean")?this.Columns[_1a5].Resizable:false;
var _1b0=(typeof (this.Columns[_1a6].Resizable)=="boolean")?this.Columns[_1a6].Resizable:false;
this.Owner.ClientSettings.Resizing.AllowColumnResize=true;
this.Columns[_1a5].Resizable=true;
this.Columns[_1a6].Resizable=true;
this.ResizeColumn(_1a5,_1ad);
this.ResizeColumn(_1a6,_1ac);
this.Owner.ClientSettings.Resizing.AllowColumnResize=_1ae;
this.Columns[_1a5].Resizable=_1af;
this.Columns[_1a6].Resizable=_1b0;
var _1b1=(this==this.Owner.MasterTableViewHeader)?this.Owner.MasterTableView.ClientID:this.ClientID;
this.Owner.SavePostData("ReorderedColumns",_1b1,this.Columns[_1a5].UniqueName,this.Columns[_1a6].UniqueName);
for(var i=0;i<_1a9.rows.length;i++){
if(_1a9.rows[i]!=null){
if((_1a9.rows[i].cells[_1a5]!=null)&&(_1a9.rows[i].cells[_1a6]!=null)){
if(!_1a9.rows[i].cells[_1a6].swapNode){
if(_1a9.rows[i].cells[_1a5].innerHTML!=null){
var _1b3=_1a9.rows[i].cells[_1a5].innerHTML;
var _1b4=_1a9.rows[i].cells[_1a6].innerHTML;
_1a9.rows[i].cells[_1a5].innerHTML=_1b4;
_1a9.rows[i].cells[_1a6].innerHTML=_1b3;
}
}else{
_1a9.rows[i].cells[_1a6].swapNode(_1a9.rows[i].cells[_1a5]);
}
}
}
}
if(this.Owner.MasterTableViewHeader==this){
var _1a9=this.Owner.MasterTableView.Control;
for(var i=0;i<_1a9.rows.length;i++){
if(_1a9.rows[i]!=null){
if((_1a9.rows[i].cells[_1a5]!=null)&&(_1a9.rows[i].cells[_1a6]!=null)){
if(window.netscape||window.opera){
if(_1a9.rows[i].cells[_1a5].innerHTML!=null){
var _1b3=_1a9.rows[i].cells[_1a5].innerHTML;
var _1b4=_1a9.rows[i].cells[_1a6].innerHTML;
_1a9.rows[i].cells[_1a5].innerHTML=_1b4;
_1a9.rows[i].cells[_1a6].innerHTML=_1b3;
}
}else{
_1a9.rows[i].cells[_1a6].swapNode(_1a9.rows[i].cells[_1a5]);
}
}
}
}
}
if(_1a7&&(!this.Owner.ClientSettings.ReorderColumnsOnClient)&&(this.Owner.ClientSettings.PostBackReferences.PostBackColumnsReorder!="")){
eval(this.Owner.ClientSettings.PostBackReferences.PostBackColumnsReorder);
}
RadGridNamespace.FireEvent(this,"OnColumnSwapped",[_1a5,_1a6]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.MoveColumnToLeft=function(_1b5){
if(isNaN(parseInt(_1b5))){
var _1b6="Column index must be of type \"Number\"!";
alert(_1b6);
return;
}
if(_1b5<0){
var _1b6="Column index must be non-negative!";
alert(_1b6);
return;
}
if(_1b5>(this.Columns.length-1)){
var _1b6="Column index must be less than columns count!";
alert(_1b6);
return;
}
if(!this.Owner.ClientSettings.AllowColumnsReorder){
return;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnColumnMovingToLeft",[_1b5])){
return;
}
var _1b7=_1b5--;
this.SwapColumns(_1b5,_1b7);
RadGridNamespace.FireEvent(this,"OnColumnMovedToLeft",[_1b5]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.MoveColumnToRight=function(_1b8){
if(isNaN(parseInt(_1b8))){
var _1b9="Column index must be of type \"Number\"!";
alert(_1b9);
return;
}
if(_1b8<0){
var _1b9="Column index must be non-negative!";
alert(_1b9);
return;
}
if(_1b8>(this.Columns.length-1)){
var _1b9="Column index must be less than columns count!";
alert(_1b9);
return;
}
if(!this.Owner.ClientSettings.AllowColumnsReorder){
return;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnColumnMovingToRight",[_1b8])){
return;
}
var _1ba=_1b8++;
this.SwapColumns(_1b8,_1ba);
RadGridNamespace.FireEvent(this,"OnColumnMovedToRight",[_1b8]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.HideColumn=function(_1bb){
if(!this.Owner.ClientSettings.AllowColumnHide){
return;
}
if(isNaN(parseInt(_1bb))){
var _1bc="Column index must be of type \"Number\"!";
alert(_1bc);
return;
}
if(_1bb<0){
var _1bc="Column index must be non-negative!";
alert(_1bc);
return;
}
if(_1bb>(this.Columns.length-1)){
var _1bc="Column index must be less than columns count!";
alert(_1bc);
return;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnColumnHiding",[_1bb])){
return;
}
for(var i=0;i<this.Control.rows.length;i++){
if(this.Control.rows[i].cells[_1bb]!=null){
if(this.Control.rows[i].cells[_1bb].colSpan==1){
this.Control.rows[i].cells[_1bb].style.display="none";
}
}
}
this.Columns[_1bb].Display=false;
if(this.Owner.FooterControl){
for(var i=0;i<this.Owner.FooterControl.rows.length;i++){
if(this.Owner.FooterControl.rows[i].cells[_1bb]!=null){
if(this.Owner.FooterControl.rows[i].cells[_1bb].colSpan==1){
this.Owner.FooterControl.rows[i].cells[_1bb].style.display="none";
}
}
}
}
if(this.Owner.HeaderControl){
for(var i=0;i<this.Owner.MasterTableViewHeader.Control.rows.length;i++){
if(this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1bb]!=null){
if(this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1bb].colSpan==1){
this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1bb].style.display="none";
}
}
}
}
if(this==this.Owner.MasterTableViewHeader){
for(var i=0;i<this.Owner.MasterTableView.Control.rows.length;i++){
if(this.Owner.MasterTableView.Control.rows[i].cells[_1bb]!=null){
if(this.Owner.MasterTableView.Control.rows[i].cells[_1bb].colSpan==1){
this.Owner.MasterTableView.Control.rows[i].cells[_1bb].style.display="none";
}
}
}
}
if(this.Owner.ClientSettings.Scrolling.AllowScroll&&this.Owner.ClientSettings.Scrolling.UseStaticHeaders&&this==this.Owner.MasterTableView){
for(var i=0;i<this.Owner.MasterTableViewHeader.Control.rows.length;i++){
if(this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1bb]!=null){
if(this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1bb].colSpan==1){
this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1bb].style.display="none";
}
}
}
}
if(this!=this.Owner.MasterTableViewHeader){
this.Owner.SavePostData("HidedColumns",this.ClientID,this.Columns[_1bb].RealIndex);
}
RadGridNamespace.FireEvent(this,"OnColumnHidden",[_1bb]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ShowColumn=function(_1be){
if(!this.Owner.ClientSettings.AllowColumnHide){
return;
}
if(isNaN(parseInt(_1be))){
var _1bf="Column index must be of type \"Number\"!";
alert(_1bf);
return;
}
if(_1be<0){
var _1bf="Column index must be non-negative!";
alert(_1bf);
return;
}
if(_1be>(this.Columns.length-1)){
var _1bf="Column index must be less than columns count!";
alert(_1bf);
return;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnColumnShowing",[_1be])){
return;
}
if(this.Control.tHead){
for(var i=0;i<this.Control.tHead.rows.length;i++){
if(this.Control.tHead.rows[i].cells[_1be]!=null){
if(window.netscape){
this.Control.tHead.rows[i].cells[_1be].style.display="table-cell";
}else{
this.Control.tHead.rows[i].cells[_1be].style.display="";
}
}
}
}
if(this.Control.tBodies[0]){
for(var i=0;i<this.Control.tBodies[0].rows.length;i++){
if(this.Control.tBodies[0].rows[i].cells[_1be]!=null){
if(window.netscape){
this.Control.tBodies[0].rows[i].cells[_1be].style.display="table-cell";
}else{
this.Control.tBodies[0].rows[i].cells[_1be].style.display="";
}
}
}
}
if(this.Owner.FooterControl){
for(var i=0;i<this.Owner.FooterControl.rows.length;i++){
if(this.Owner.FooterControl.rows[i].cells[_1be]!=null){
if(window.netscape){
this.Owner.FooterControl.rows[i].cells[_1be].style.display="table-cell";
}else{
this.Owner.FooterControl.rows[i].cells[_1be].style.display="";
}
}
}
}
if(this==this.Owner.MasterTableViewHeader){
for(var i=0;i<this.Owner.MasterTableView.Control.rows.length;i++){
if(this.Owner.MasterTableView.Control.rows[i].cells[_1be]!=null){
if(window.netscape){
this.Owner.MasterTableView.Control.rows[i].cells[_1be].style.display="table-cell";
}else{
this.Owner.MasterTableView.Control.rows[i].cells[_1be].style.display="";
}
}
}
}
if(this.Owner.ClientSettings.Scrolling.AllowScroll&&this.Owner.ClientSettings.Scrolling.UseStaticHeaders&&this==this.Owner.MasterTableView){
for(var i=0;i<this.Owner.MasterTableViewHeader.Control.rows.length;i++){
if(this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1be]!=null){
if(window.netscape){
this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1be].style.display="table-cell";
}else{
this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1be].style.display="";
}
}
}
}
if(this!=this.Owner.MasterTableViewHeader){
this.Owner.SavePostData("ShowedColumns",this.ClientID,this.Columns[_1be].RealIndex);
}
this.Columns[_1be].Display=true;
RadGridNamespace.FireEvent(this,"OnColumnShowed",[_1be]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.HideRow=function(_1c1){
if(!this.Owner.ClientSettings.AllowRowHide){
return;
}
if(isNaN(parseInt(_1c1))){
var _1c2="Row index must be of type \"Number\"!";
alert(_1c2);
return;
}
if(_1c1<0){
var _1c2="Row index must be non-negative!";
alert(_1c2);
return;
}
if(_1c1>(this.Rows.length-1)){
var _1c2="Row index must be less than rows count!";
alert(_1c2);
return;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnRowHiding",[_1c1])){
return;
}
if(this.Rows){
if(this.Rows[_1c1]){
if(this.Rows[_1c1].Control){
this.Rows[_1c1].Control.style.display="none";
this.Rows[_1c1].Display=false;
}
}
}
if(this!=this.Owner.MasterTableViewHeader){
this.Owner.SavePostData("HidedRows",this.ClientID,this.Rows[_1c1].RealIndex);
}
RadGridNamespace.FireEvent(this,"OnRowHidden",[_1c1]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ShowRow=function(_1c3){
if(!this.Owner.ClientSettings.AllowRowHide){
return;
}
if(isNaN(parseInt(_1c3))){
var _1c4="Row index must be of type \"Number\"!";
alert(_1c4);
return;
}
if(_1c3<0){
var _1c4="Row index must be non-negative!";
alert(_1c4);
return;
}
if(_1c3>this.Rows.length){
var _1c4="Row index must be less than rows count!";
alert(_1c4);
return;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnRowShowing",[_1c3])){
return;
}
if(this.Rows){
if(this.Rows[_1c3]){
if(this.Rows[_1c3].Control){
if(this.Rows[_1c3].ItemType!="NestedView"){
if(window.netscape){
this.Rows[_1c3].Control.style.display="table-row";
}else{
this.Rows[_1c3].Control.style.display="";
}
this.Rows[_1c3].Display=true;
}
}
}
}
if(this!=this.Owner.MasterTableViewHeader){
this.Owner.SavePostData("ShowedRows",this.ClientID,this.Rows[_1c3].RealIndex);
}
RadGridNamespace.FireEvent(this,"OnRowShowed",[_1c3]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ExportToExcel=function(_1c5){
try{
if(this.Owner.ClientSettings.PostBackReferences.PostBackExportToExcel!=""){
this.Owner.SavePostData("ExportToExcel",this.ClientID,_1c5);
eval(this.Owner.ClientSettings.PostBackReferences.PostBackExportToExcel);
}
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.ExportToWord=function(_1c6){
try{
if(this.Owner.ClientSettings.PostBackReferences.PostBackExportToWord!=""){
this.Owner.SavePostData("ExportToWord",this.ClientID,_1c6);
eval(this.Owner.ClientSettings.PostBackReferences.PostBackExportToWord);
}
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.AddToSelectedRows=function(_1c7){
try{
this.SelectedRows[this.SelectedRows.length]=_1c7;
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.IsInSelectedRows=function(_1c8){
try{
for(var i=0;i<this.SelectedRows.length;i++){
if(this.SelectedRows[i]!=_1c8){
return true;
}
}
return false;
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.ClearSelectedRows=function(){
var _1ca=this.SelectedRows;
for(var i=0;i<this.SelectedRows.length;i++){
if(!RadGridNamespace.FireEvent(this,"OnRowDeselecting",[this.SelectedRows[i]])){
continue;
}
this.SelectedRows[i].Selected=false;
this.SelectedRows[i].CheckClientSelectColumns();
this.SelectedRows[i].RemoveSelectedRowStyle();
var last=this.SelectedRows[i];
try{
this.SelectedRows.splice(i,1);
i--;
}
catch(ex){
}
RadGridNamespace.FireEvent(this,"OnRowDeselected",[last]);
}
this.SelectedRows=new Array();
};
RadGridNamespace.RadGridTable.prototype.RemoveFromSelectedRows=function(_1cd){
try{
var _1ce=new Array();
for(var i=0;i<this.SelectedRows.length;i++){
var last=this.SelectedRows[i];
if(this.SelectedRows[i]!=_1cd){
_1ce[_1ce.length]=this.SelectedRows[i];
}else{
if(!this.Owner.AllowMultiRowSelection){
if(!RadGridNamespace.FireEvent(this,"OnRowDeselecting",[this.SelectedRows[i]])){
continue;
}
}
try{
this.SelectedRows.splice(i,1);
i--;
}
catch(ex){
}
_1cd.CheckClientSelectColumns();
setTimeout(function(){
RadGridNamespace.FireEvent(_1cd.Owner,"OnRowDeselected",[_1cd]);
},100);
}
}
this.SelectedRows=_1ce;
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.GetSelectedRowsIndexes=function(){
try{
var _1d1=new Array();
for(var i=0;i<this.SelectedRows.length;i++){
_1d1[_1d1.length]=this.SelectedRows[i].RealIndex;
}
return _1d1.join(",");
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.GetCellByColumnUniqueName=function(_1d3,_1d4){
if(this.ClientID.indexOf("_Header")!=-1){
return;
}
if((!_1d3)||(!_1d4)){
return;
}
if(!this.Columns){
return;
}
for(var i=0;i<this.Columns.length;i++){
if(this.Columns[i].UniqueName.toUpperCase()==_1d4.toUpperCase()){
return _1d3.Control.cells[i];
}
}
return null;
};
RadGridNamespace.RadGridTableColumn=function(_1d6){
if((!_1d6)||typeof (_1d6)!="object"){
return;
}
RadControlsNamespace.DomEventMixin.Initialize(this);
for(var _1d7 in _1d6){
this[_1d7]=_1d6[_1d7];
}
this.Type="RadGridTableColumn";
this.ResizeTolerance=5;
this.CanResize=false;
};
RadGridNamespace.RadGridTableColumn.prototype._constructor=function(_1d8,_1d9){
this.Control=_1d8;
this.Owner=_1d9;
this.Index=_1d8.cellIndex;
if(window.opera&&typeof (_1d8.cellIndex)=="undefined"){
this.Index=0;
}
this.AttachDomEvent(this.Control,"click","OnClick");
this.AttachDomEvent(this.Control,"dblclick","OnDblClick");
this.AttachDomEvent(this.Control,"mousemove","OnMouseMove");
this.AttachDomEvent(this.Control,"mousedown","OnMouseDown");
this.AttachDomEvent(this.Control,"mouseup","OnMouseUp");
this.AttachDomEvent(this.Control,"mouseover","OnMouseOver");
this.AttachDomEvent(this.Control,"mouseout","OnMouseOut");
this.AttachDomEvent(this.Control,"contextmenu","OnContextMenu");
};
RadGridNamespace.RadGridTableColumn.prototype.Dispose=function(){
this.DisposeDomEventHandlers();
if(this.ColumnResizer){
this.ColumnResizer.Dispose();
}
this.Control=null;
this.Owner=null;
this.Index=null;
};
RadGridNamespace.RadGridTableColumn.prototype.OnContextMenu=function(e){
try{
if(!RadGridNamespace.FireEvent(this.Owner,"OnColumnContextMenu",[this.Index,e])){
return;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnClick=function(e){
try{
if(!RadGridNamespace.FireEvent(this.Owner,"OnColumnClick",[this.Index])){
return;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnDblClick=function(e){
try{
if(!RadGridNamespace.FireEvent(this.Owner,"OnColumnDblClick",[this.Index])){
return;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnMouseMove=function(e){
if(this.Owner.Owner.ClientSettings.Resizing.AllowColumnResize&&this.Resizable&&this.Control.tagName.toLowerCase()=="th"){
var _1de=RadGridNamespace.GetEventPosX(e);
var _1df=RadGridNamespace.FindPosX(this.Control);
var endX=_1df+this.Control.offsetWidth;
var _1e1=RadGridNamespace.GetCurrentElement(e);
if((_1de>=endX-this.ResizeTolerance)&&(_1de<=endX+this.ResizeTolerance)){
this.Control.style.cursor="e-resize";
this.Control.title=this.Owner.Owner.ClientSettings.ClientMessages.DragToResize;
this.CanResize=true;
_1e1.style.cursor="e-resize";
this.Owner.Owner.IsResize=true;
}else{
this.Control.style.cursor="";
this.Control.title="";
this.CanResize=false;
_1e1.style.cursor="";
this.Owner.Owner.IsResize=false;
}
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnMouseDown=function(e){
if(this.CanResize){
if(((window.netscape||window.opera)&&(e.button==0))||(e.button==1)){
var _1e3=RadGridNamespace.GetEventPosX(e);
var _1e4=RadGridNamespace.FindPosX(this.Control);
var endX=_1e4+this.Control.offsetWidth;
if((_1e3>=endX-this.ResizeTolerance)&&(_1e3<=endX+this.ResizeTolerance)){
this.ColumnResizer=new RadGridNamespace.RadGridColumnResizer(this,this.Owner.Owner.ClientSettings.Resizing.EnableRealTimeResize);
this.ColumnResizer.Position(e);
}
}
RadGridNamespace.ClearDocumentEvents();
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnMouseUp=function(e){
RadGridNamespace.RestoreDocumentEvents();
};
RadGridNamespace.RadGridTableColumn.prototype.OnMouseOver=function(e){
if(!RadGridNamespace.FireEvent(this.Owner,"OnColumnMouseOver",[this.Index])){
return;
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnMouseOut=function(e){
if(!RadGridNamespace.FireEvent(this.Owner,"OnColumnMouseOut",[this.Index])){
return;
}
};
RadGridNamespace.RadGridColumnResizer=function(_1e9,_1ea){
if(!_1e9){
return;
}
RadControlsNamespace.DomEventMixin.Initialize(this);
this.Column=_1e9;
this.IsRealTimeResize=_1ea;
this.CurrentWidth=null;
this.LeftResizer=document.createElement("span");
this.LeftResizer.style.backgroundColor="navy";
this.LeftResizer.style.width="1"+"px";
this.LeftResizer.style.position="absolute";
this.LeftResizer.style.cursor="e-resize";
this.RightResizer=document.createElement("span");
this.RightResizer.style.backgroundColor="navy";
this.RightResizer.style.width="1"+"px";
this.RightResizer.style.position="absolute";
this.RightResizer.style.cursor="e-resize";
this.ResizerToolTip=document.createElement("span");
this.ResizerToolTip.style.backgroundColor="#F5F5DC";
this.ResizerToolTip.style.border="1px solid";
this.ResizerToolTip.style.position="absolute";
this.ResizerToolTip.style.font="icon";
this.ResizerToolTip.style.padding="2";
this.ResizerToolTip.innerHTML="Width: <b>"+this.Column.Control.offsetWidth+"</b> <em>pixels</em>";
document.body.appendChild(this.LeftResizer);
document.body.appendChild(this.RightResizer);
document.body.appendChild(this.ResizerToolTip);
this.CanDestroy=true;
this.AttachDomEvent(document,"mouseup","OnMouseUp");
this.AttachDomEvent(this.Column.Owner.Owner.Control,"mousemove","OnMouseMove");
};
RadGridNamespace.RadGridColumnResizer.prototype.OnMouseUp=function(e){
this.Destroy(e);
};
RadGridNamespace.RadGridColumnResizer.prototype.OnMouseMove=function(e){
this.Move(e);
};
RadGridNamespace.RadGridColumnResizer.prototype.Position=function(e){
this.LeftResizer.style.top=RadGridNamespace.FindPosY(this.Column.Control)-RadGridNamespace.FindScrollPosY(this.Column.Control)+document.documentElement.scrollTop+document.body.scrollTop+"px";
this.LeftResizer.style.left=RadGridNamespace.FindPosX(this.Column.Control)-RadGridNamespace.FindScrollPosX(this.Column.Control)+document.documentElement.scrollLeft+document.body.scrollLeft+"px";
this.RightResizer.style.top=this.LeftResizer.style.top;
this.RightResizer.style.left=parseInt(this.LeftResizer.style.left)+this.Column.Control.offsetWidth+"px";
this.ResizerToolTip.style.top=parseInt(this.RightResizer.style.top)-20+"px";
this.ResizerToolTip.style.left=parseInt(this.RightResizer.style.left)-5+"px";
if(parseInt(this.LeftResizer.style.left)<RadGridNamespace.FindPosX(this.Column.Owner.Control)){
this.LeftResizer.style.display="none";
}
this.LeftResizer.style.height=this.Column.Control.offsetHeight+"px";
this.RightResizer.style.height=this.Column.Control.offsetHeight+"px";
};
RadGridNamespace.RadGridColumnResizer.prototype.Destroy=function(e){
if(this.CanDestroy){
this.DetachDomEvent(document,"mouseup","OnMouseUp");
this.DetachDomEvent(this.Column.Owner.Owner.Control,"mousemove","OnMouseMove");
if(this.CurrentWidth!=null){
if(this.CurrentWidth>0){
this.Column.Owner.ResizeColumn(RadGridNamespace.GetRealCellIndex(this.Column.Owner,this.Column.Control),this.CurrentWidth);
this.CurrentWidth=null;
}
}
document.body.removeChild(this.LeftResizer);
document.body.removeChild(this.RightResizer);
document.body.removeChild(this.ResizerToolTip);
this.CanDestroy=false;
}
};
RadGridNamespace.RadGridColumnResizer.prototype.Dispose=function(){
try{
this.Destroy();
}
catch(error){
}
this.DisposeDomEventHandlers();
this.MouseUpHandler=null;
this.MouseMoveHandler=null;
this.LeftResizer=null;
this.RightResizer=null;
this.ResizerToolTip=null;
};
RadGridNamespace.RadGridColumnResizer.prototype.Move=function(e){
this.LeftResizer.style.left=RadGridNamespace.FindPosX(this.Column.Control)-RadGridNamespace.FindScrollPosX(this.Column.Control)+document.documentElement.scrollLeft+document.body.scrollLeft+"px";
this.RightResizer.style.left=parseInt(this.LeftResizer.style.left)+(RadGridNamespace.GetEventPosX(e)-RadGridNamespace.FindPosX(this.Column.Control))+"px";
this.ResizerToolTip.style.left=parseInt(this.RightResizer.style.left)-5+"px";
var _1f0=parseInt(this.RightResizer.style.left)-parseInt(this.LeftResizer.style.left);
var _1f1=this.Column.Control.scrollWidth-_1f0;
this.ResizerToolTip.innerHTML="Width: <b>"+_1f0+"</b> <em>pixels</em>";
if(!RadGridNamespace.FireEvent(this.Column.Owner,"OnColumnResizing",[this.Column.Index,_1f0])){
return;
}
if(_1f0<=0){
this.RightResizer.style.left=this.RightResizer.style.left;
this.Destroy(e);
return;
}
this.CurrentWidth=_1f0;
if(this.IsRealTimeResize){
this.Column.Owner.ResizeColumn(RadGridNamespace.GetRealCellIndex(this.Column.Owner,this.Column.Control),_1f0);
}else{
this.CurrentWidth=_1f0;
return;
}
if(RadGridNamespace.FindPosX(this.LeftResizer)!=RadGridNamespace.FindPosX(this.Column.Control)){
this.LeftResizer.style.left=RadGridNamespace.FindPosX(this.Column.Control)+"px";
}
if(RadGridNamespace.FindPosX(this.RightResizer)!=(RadGridNamespace.FindPosX(this.Column.Control)+this.Column.Control.offsetWidth)){
this.RightResizer.style.left=RadGridNamespace.FindPosX(this.Column.Control)+this.Column.Control.offsetWidth+"px";
}
if(RadGridNamespace.FindPosY(this.LeftResizer)!=RadGridNamespace.FindPosY(this.Column.Control)){
this.LeftResizer.style.top=RadGridNamespace.FindPosY(this.Column.Control)+"px";
this.RightResizer.style.top=RadGridNamespace.FindPosY(this.Column.Control)+"px";
}
if(this.LeftResizer.offsetHeight!=this.Column.Control.offsetHeight){
this.LeftResizer.style.height=this.Column.Control.offsetHeight+"px";
this.RightResizer.style.height=this.Column.Control.offsetHeight+"px";
}
if(this.Column.Owner.Owner.GridDataDiv){
this.LeftResizer.style.left=parseInt(this.LeftResizer.style.left.replace("px",""))-this.Column.Owner.Owner.GridDataDiv.scrollLeft+"px";
this.RightResizer.style.left=parseInt(this.LeftResizer.style.left.replace("px",""))+this.Column.Control.offsetWidth+"px";
this.ResizerToolTip.style.left=parseInt(this.RightResizer.style.left)-5+"px";
}
};
RadGridNamespace.RadGridTableRow=function(_1f2){
if((!_1f2)||typeof (_1f2)!="object"){
return;
}
RadControlsNamespace.DomEventMixin.Initialize(this);
for(var _1f3 in _1f2){
this[_1f3]=_1f2[_1f3];
}
this.Type="RadGridTableRow";
var _1f4=document.getElementById(this.OwnerID);
this.Control=_1f4.tBodies[0].rows[this.ClientRowIndex];
if(!this.Control){
return;
}
this.Index=this.Control.sectionRowIndex;
this.RealIndex=this.RowIndex;
};
RadGridNamespace.RadGridTableRow.prototype._constructor=function(_1f5){
this.Owner=_1f5;
this.CreateStyles();
if(this.Selected){
this.LoadSelected();
}
if(this.Owner.HierarchyLoadMode=="Client"){
if(this.Owner.Owner.ClientSettings.AllowExpandCollapse){
for(var i=0;i<this.Owner.ExpandCollapseColumns.length;i++){
var _1f7=this.Owner.ExpandCollapseColumns[i].Control.cellIndex;
var _1f8=this.Control.cells[_1f7];
var html=this.Control.innerHTML;
if(!_1f8){
continue;
}
var _1fa;
for(var j=0;j<_1f8.childNodes.length;j++){
if(!_1f8.childNodes[j].tagName){
continue;
}
var _1fc;
if(this.Owner.ExpandCollapseColumns[i].ButtonType=="ImageButton"){
_1fc="img";
}else{
if(this.Owner.ExpandCollapseColumns[i].ButtonType=="LinkButton"){
_1fc="a";
}else{
if(this.Owner.ExpandCollapseColumns[i].ButtonType=="PushButton"){
_1fc="button";
}
}
}
if(_1f8.childNodes[j].tagName.toLowerCase()==_1fc){
_1fa=_1f8.childNodes[j];
break;
}
}
if(_1fa){
var _1fd=this;
var _1fe=function(){
_1fd.OnHierarchyExpandButtonClick(this);
};
_1fa.onclick=_1fe;
_1fa.ondblclick=null;
_1fe=null;
}
_1fa=null;
}
}
}
if(this.Owner.GroupLoadMode=="Client"){
if(this.Owner.Owner.ClientSettings.AllowGroupExpandCollapse){
for(var i=0;i<this.Owner.GroupSplitterColumns.length;i++){
var _1f7=this.Owner.GroupSplitterColumns[i].Control.cellIndex;
var html=this.Control.innerHTML;
var _1f8=this.Control.cells[_1f7];
if(!_1f8){
continue;
}
var _1fa;
for(var j=0;j<_1f8.childNodes.length;j++){
if(!_1f8.childNodes[j].tagName){
continue;
}
if(_1f8.childNodes[j].tagName.toLowerCase()=="img"){
_1fa=_1f8.childNodes[j];
break;
}
}
if(_1fa){
var _1fd=this;
var _1fe=function(){
_1fd.OnGroupExpandButtonClick(this);
};
_1fa.onclick=_1fe;
_1fa.ondblclick=null;
_1fe=null;
}
_1fa=null;
}
}
}
this.AttachDomEvent(this.Control,"click","OnClick");
this.AttachDomEvent(this.Control,"dblclick","OnDblClick");
this.AttachDomEvent(document,"mousedown","OnMouseDown");
this.AttachDomEvent(document,"mouseup","OnMouseUp");
this.AttachDomEvent(document,"mousemove","OnMouseMove");
this.AttachDomEvent(this.Control,"mouseover","OnMouseOver");
this.AttachDomEvent(this.Control,"mouseout","OnMouseOut");
this.AttachDomEvent(this.Control,"contextmenu","OnContextMenu");
if(this.Owner.Owner.ClientSettings.ActiveRowData&&this.Owner.Owner.ClientSettings.ActiveRowData!=""){
var data=this.Owner.Owner.ClientSettings.ActiveRowData.split(";")[0].split(",");
if(data[0]==this.Owner.ClientID&&data[1]==this.RealIndex){
this.Owner.Owner.ActiveRow=this;
}
}
};
RadGridNamespace.GroupRowExpander=function(_200){
this.startRow=_200;
};
RadGridNamespace.GroupRowExpander.prototype.NotFinished=function(_201){
var _202=(this.currentGridRow!=null);
if(!_202){
return false;
}
var _203=(this.currentGridRow.GroupIndex=="");
var _204=(this.currentGridRow.GroupIndex==_201.GroupIndex);
var _205=(this.currentGridRow.GroupIndex.indexOf(_201.GroupIndex+"_")==0);
return (_203||_204||_205);
};
RadGridNamespace.GroupRowExpander.prototype.ToggleExpandCollapse=function(_206){
var _207=this.startRow;
var _208=_207.Owner;
var _209=_206.parentNode.parentNode.sectionRowIndex;
var _20a=_208.Rows[_209];
if(_20a.Expanded){
if(!RadGridNamespace.FireEvent(_20a.Owner,"OnGroupCollapsing",[_20a])){
return;
}
}else{
if(!RadGridNamespace.FireEvent(_20a.Owner,"OnGroupExpanding",[_20a])){
return;
}
}
var _20b=_208.Control.rows[_209+1];
if(!_20b){
return;
}
this.currentRowIndex=_20b.rowIndex;
this.lastGroupIndex=null;
while(true){
this.currentGridRow=_208.Rows[this.currentRowIndex];
var _20c=this.NotFinished(_20a);
if(!_20c){
break;
}
var _20d=(this.lastGroupIndex!=null)&&(this.currentGridRow.GroupIndex.indexOf(this.lastGroupIndex)!=-1);
var _20e=(this.currentGridRow.ItemType!="GroupHeader")&&(!this.currentGridRow.IsVisible());
var _20f=_20d&&_20e;
if(this.currentGridRow.ItemType=="GroupHeader"&&!this.currentGridRow.Expanded){
if(this.currentGridRow.IsVisible()){
this.currentGridRow.Hide();
_206.src=_208.GroupSplitterColumns[0].ExpandImageUrl;
if(_208.Rows[this.currentRowIndex+1]==null||_208.Rows[this.currentRowIndex+1].ItemType=="GroupHeader"){
this.currentGridRow.Expanded=false;
}
}else{
_206.src=_208.GroupSplitterColumns[0].CollapseImageUrl;
this.currentGridRow.Show();
if(_208.Rows[this.currentRowIndex+1]==null||_208.Rows[this.currentRowIndex+1].ItemType=="GroupHeader"){
this.currentGridRow.Expanded=true;
}
}
this.lastGroupIndex=this.currentGridRow.GroupIndex;
}else{
if(!_20f){
if(this.currentGridRow.ItemType=="NestedView"){
if(this.currentGridRow.Expanded){
if(this.currentGridRow.IsVisible()){
this.currentGridRow.Hide();
}else{
this.currentGridRow.Show();
}
}
}else{
if(this.currentGridRow.IsVisible()){
this.currentGridRow.Hide();
_206.src=_208.GroupSplitterColumns[0].ExpandImageUrl;
_20a.Expanded=false;
}else{
_206.src=_208.GroupSplitterColumns[0].CollapseImageUrl;
this.currentGridRow.Show();
_20a.Expanded=true;
}
}
}
}
this.currentRowIndex++;
}
if(_20a.Expanded!=null){
if(_20a.Expanded){
_208.Owner.SavePostData("ExpandedGroupRows",_208.ClientID,_20a.RealIndex);
_207.title=_208.Owner.GroupingSettings.CollapseTooltip;
}else{
_208.Owner.SavePostData("CollapsedGroupRows",_208.ClientID,_20a.RealIndex);
_207.title=_208.Owner.GroupingSettings.ExpandTooltip;
}
}
if(_20a.Expanded){
if(!RadGridNamespace.FireEvent(_20a.Owner,"OnGroupExpanded",[_20a])){
return;
}
}else{
if(!RadGridNamespace.FireEvent(_20a.Owner,"OnGroupCollapsed",[_20a])){
return;
}
}
};
RadGridNamespace.RadGridTableRow.prototype.OnGroupExpandButtonClick=function(_210){
var _211=new RadGridNamespace.GroupRowExpander(this);
_211.ToggleExpandCollapse(_210);
};
RadGridNamespace.RadGridTableRow.prototype.OnHierarchyExpandButtonClick=function(_212){
var _213=this.Owner.Control.rows[_212.parentNode.parentNode.rowIndex+1];
var _214=this.Owner.Rows[_212.parentNode.parentNode.sectionRowIndex];
if(!_213){
return;
}
if(this.TableRowIsVisible(_213)){
if(!RadGridNamespace.FireEvent(this.Owner,"OnHierarchyCollapsing",[this])){
return;
}
this.HideTableRow(_213);
_214.Expanded=false;
if(this.Owner.ExpandCollapseColumns[0].ButtonType=="ImageButton"){
_212.src=this.Owner.ExpandCollapseColumns[0].ExpandImageUrl;
}else{
_212.innerHTML="+";
}
_212.title=this.Owner.Owner.HierarchySettings.ExpandTooltip;
this.Owner.Owner.SavePostData("CollapsedRows",this.Owner.ClientID,this.RealIndex);
if(!RadGridNamespace.FireEvent(this.Owner,"OnHierarchyCollapsed",[this])){
return;
}
}else{
if(!RadGridNamespace.FireEvent(this.Owner,"OnHierarchyExpanding",[this])){
return;
}
if(this.Owner.ExpandCollapseColumns[0].ButtonType=="ImageButton"){
_212.src=this.Owner.ExpandCollapseColumns[0].CollapseImageUrl;
}else{
_212.innerHTML="-";
}
_212.title=this.Owner.Owner.HierarchySettings.CollapseTooltip;
this.ShowTableRow(_213);
_214.Expanded=true;
this.Owner.Owner.SavePostData("ExpandedRows",this.Owner.ClientID,this.RealIndex);
if(!RadGridNamespace.FireEvent(this.Owner,"OnHierarchyExpanded",[this])){
return;
}
}
};
RadGridNamespace.RadGridTableRow.prototype.TableRowIsVisible=function(_215){
return _215.style.display!="none";
};
RadGridNamespace.RadGridTableRow.prototype.IsVisible=function(){
return this.TableRowIsVisible(this.Control);
};
RadGridNamespace.RadGridTableRow.prototype.HideTableRow=function(_216){
if(this.TableRowIsVisible(_216)){
_216.style.display="none";
}
};
RadGridNamespace.RadGridTableRow.prototype.Hide=function(){
this.HideTableRow(this.Control);
};
RadGridNamespace.RadGridTableRow.prototype.ShowTableRow=function(_217){
if(window.netscape||window.opera){
_217.style.display="table-row";
}else{
_217.style.display="block";
}
};
RadGridNamespace.RadGridTableRow.prototype.Show=function(){
this.ShowTableRow(this.Control);
};
RadGridNamespace.RadGridTableRow.prototype.Dispose=function(){
this.DisposeDomEventHandlers();
this.Control=null;
this.Owner=null;
};
RadGridNamespace.RadGridTableRow.prototype.CreateStyles=function(){
if(!this.Owner.Owner.ClientSettings.ApplyStylesOnClient){
return;
}
switch(this.ItemType){
case "GroupHeader":
break;
case "EditFormItem":
this.Control.className+=" "+this.Owner.RenderEditItemStyleClass;
this.Control.style.cssText+=" "+this.Owner.RenderEditItemStyle;
break;
default:
var _218=eval("this.Owner.Render"+this.ItemType+"StyleClass");
if(typeof (_218)!="undefined"){
this.Control.className+=" "+_218;
}
var _219=eval("this.Owner.Render"+this.ItemType+"Style");
if(typeof (_219)!="undefined"){
this.Control.style.cssText+=" "+_219;
}
break;
}
if(!this.Display){
if(this.Control.style.cssText!=""){
if(this.Control.style.cssText.lastIndexOf(";")==this.Control.style.cssText.length-1){
this.Control.style.cssText+="display:none;";
}else{
this.Control.style.cssText+=";display:none;";
}
}else{
this.Control.style.cssText+="display:none;";
}
}
};
RadGridNamespace.RadGridTableRow.prototype.OnContextMenu=function(e){
try{
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowContextMenu",[this.Index,e])){
return;
}
if(this.Owner.Owner.ClientSettings.ClientEvents.OnRowContextMenu!=""){
if(e.preventDefault){
e.preventDefault();
}else{
e.returnValue=false;
return false;
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableRow.prototype.OnClick=function(e){
try{
if(this.Owner.Owner.RowResizer){
return;
}
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowClick",[this.Control.sectionRowIndex,e])){
return;
}
if(e.shiftKey&&this.Owner.SelectedRows[0]){
if(this.Owner.SelectedRows[0].Control.rowIndex>this.Control.rowIndex){
for(var i=this.Control.rowIndex;i<this.Owner.SelectedRows[0].Control.rowIndex+1;i++){
var _21d=this.Owner.Owner.GetRowObjectByRealRow(this.Owner,this.Owner.Control.rows[i]);
if(_21d){
if(!_21d.Selected){
this.Owner.SelectRow(this.Owner.Control.rows[i],false);
}
}
}
}
if(this.Owner.SelectedRows[0].Control.rowIndex<this.Control.rowIndex){
for(var i=this.Owner.SelectedRows[0].Control.rowIndex;i<this.Control.rowIndex+1;i++){
var _21d=this.Owner.Owner.GetRowObjectByRealRow(this.Owner,this.Owner.Control.rows[i]);
if(_21d){
if(!_21d.Selected){
this.Owner.SelectRow(this.Owner.Control.rows[i],false);
}
}
}
}
}
if(!e.shiftKey){
this.HandleRowSelection(e);
}
var _21e=RadGridNamespace.GetCurrentElement(e);
if(!_21e){
return;
}
if(!_21e.tagName){
return;
}
if(_21e.tagName.toLowerCase()=="input"&&_21e.tagName.toLowerCase()=="select"&&_21e.tagName.toLowerCase()=="option"&&_21e.tagName.toLowerCase()=="button"&&_21e.tagName.toLowerCase()=="a"&&_21e.tagName.toLowerCase()=="textarea"){
return;
}
if(this.ItemType=="Item"||this.ItemType=="AlternatingItem"){
if(this.Owner.Owner.ClientSettings.EnablePostBackOnRowClick){
var _21f=this.Owner.Owner.ClientSettings.PostBackFunction;
_21f=_21f.replace("{0}",this.Owner.Owner.UniqueID).replace("{1}","RowClick;"+this.ItemIndexHierarchical);
var form=document.getElementById(this.Owner.Owner.FormID);
if(form!=null&&form["__EVENTTARGET"]!=null&&form["__EVENTTARGET"].value==""){
eval(_21f);
}
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableRow.prototype.HandleActiveRow=function(e){
var _222=RadGridNamespace.GetCurrentElement(e);
if(_222!=null&&_222.tagName&&(_222.tagName.toLowerCase()=="input"||_222.tagName.toLowerCase()=="textarea")){
return;
}
if(this.Owner.Owner.ActiveRow!=null){
if(!RadGridNamespace.FireEvent(this.Owner,"OnActiveRowChanging",[this.Owner.Owner.ActiveRow])){
return;
}
if(e.keyCode==13){
this.Owner.Owner.SavePostData("EditRow",this.Owner.ClientID,this.Owner.Owner.ActiveRow.RealIndex);
eval(this.Owner.Owner.ClientSettings.PostBackReferences.PostBackEditRow);
}
if(e.keyCode==40){
var _223=this.Owner.Rows[this.Owner.Owner.ActiveRow.Control.sectionRowIndex+1];
if(_223!=null){
this.Owner.Owner.SetActiveRow(_223);
this.ScrollIntoView(_223);
}
}
if(e.keyCode==39){
return;
var _223=this.Owner.Owner.GetNextHierarchicalRow(_224,this.Owner.Owner.ActiveRow.Control.sectionRowIndex);
if(_223!=null){
_224=_223.parentNode.parentNode;
this.Owner.Owner.SetActiveRow(_224,_223.sectionRowIndex);
this.ScrollIntoView(_223);
}
}
if(e.keyCode==38){
var _225=this.Owner.Rows[this.Owner.Owner.ActiveRow.Control.sectionRowIndex-1];
if(_225!=null){
this.Owner.Owner.SetActiveRow(_225);
this.ScrollIntoView(_225);
}
}
if(e.keyCode==37){
return;
var _225=this.Owner.Owner.GetPreviousHierarchicalRow(_224,this.Owner.Owner.ActiveRow.Control.sectionRowIndex);
if(_225!=null){
var _224=_225.parentNode.parentNode;
this.Owner.Owner.SetActiveRow(_224,_225.sectionRowIndex);
this.ScrollIntoView(_225);
}
}
if(e.keyCode==32){
if(this.Owner.Owner.ClientSettings.Selecting.AllowRowSelect){
this.Owner.Owner.ActiveRow.Owner.SelectRow(this.Owner.Owner.ActiveRow.Control,!this.Owner.Owner.AllowMultiRowSelection);
}
}
}
if(window.netscape){
e.preventDefault();
return false;
}else{
e.returnValue=false;
}
RadGridNamespace.FireEvent(this.Owner,"OnActiveRowChanged",[this.Owner.Owner.ActiveRow]);
};
RadGridNamespace.RadGridTableRow.prototype.ScrollIntoView=function(row){
if(row.Control&&row.Control.focus){
row.Control.scrollIntoView(false);
try{
row.Control.focus();
}
catch(e){
}
}
};
RadGridNamespace.RadGridTableRow.prototype.HandleExpandCollapse=function(){
};
RadGridNamespace.RadGridTableRow.prototype.HandleGroupExpandCollapse=function(){
};
RadGridNamespace.RadGridTableRow.prototype.HandleRowSelection=function(e){
var _228=RadGridNamespace.GetCurrentElement(e);
if(_228.onclick){
return;
}
if(_228.tagName.toLowerCase()=="a"&&_228.tagName.toLowerCase()=="img"||_228.tagName.toLowerCase()=="input"){
return;
}
this.SetSelected(!e.ctrlKey);
};
RadGridNamespace.RadGridTableRow.prototype.CheckClientSelectColumns=function(){
if(!this.Owner.Columns){
return;
}
for(var i=0;i<this.Owner.Columns.length;i++){
if(this.Owner.Columns[i].ColumnType=="GridClientSelectColumn"){
var cell=this.Owner.GetCellByColumnUniqueName(this,this.Owner.Columns[i].UniqueName);
if(cell!=null){
var _22b=cell.getElementsByTagName("input")[0];
if(_22b!=null){
_22b.checked=this.Selected;
}
}
}
}
};
RadGridNamespace.RadGridTableRow.prototype.SetSelected=function(_22c){
if(!this.Selected){
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowSelecting",[this])){
return;
}
}
if((this.ItemType=="Item")||(this.ItemType=="AlternatingItem")){
if(_22c){
this.SingleSelect();
}else{
this.MultiSelect();
}
}
this.CheckClientSelectColumns();
if(this.Selected){
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowSelected",[this])){
return;
}
}
};
RadGridNamespace.RadGridTableRow.prototype.SingleSelect=function(){
if(!this.Owner.Owner.ClientSettings.Selecting.AllowRowSelect){
return;
}
this.Owner.ClearSelectedRows();
this.Owner.Owner.ClearSelectedRows();
this.Selected=true;
this.ApplySelectedRowStyle();
this.Owner.AddToSelectedRows(this);
var _22d=this.Owner.GetSelectedRowsIndexes();
this.Owner.Owner.SavePostData("SelectedRows",this.Owner.ClientID,_22d);
};
RadGridNamespace.RadGridTableRow.prototype.SingleDeselect=function(){
if(!this.Owner.Owner.ClientSettings.Selecting.AllowRowSelect){
return;
}
this.Owner.ClearSelectedRows();
this.Owner.Owner.ClearSelectedRows();
this.Selected=false;
this.RemoveSelectedRowStyle();
this.Owner.RemoveFromSelectedRows(this);
var _22e=this.Owner.GetSelectedRowsIndexes();
this.Owner.Owner.SavePostData("SelectedRows",this.Owner.ClientID,_22e);
};
RadGridNamespace.RadGridTableRow.prototype.MultiSelect=function(){
if((!this.Owner.Owner.ClientSettings.Selecting.AllowRowSelect)||(!this.Owner.Owner.AllowMultiRowSelection)){
return;
}
if(this.Selected){
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowDeselecting",[this])){
return;
}
this.Selected=false;
this.RemoveSelectedRowStyle();
this.Owner.RemoveFromSelectedRows(this);
var _22f=this.Owner.GetSelectedRowsIndexes();
this.Owner.Owner.SavePostData("SelectedRows",this.Owner.ClientID,_22f);
}else{
this.Selected=true;
this.ApplySelectedRowStyle();
this.Owner.AddToSelectedRows(this);
var _22f=this.Owner.GetSelectedRowsIndexes();
this.Owner.Owner.SavePostData("SelectedRows",this.Owner.ClientID,_22f);
}
};
RadGridNamespace.RadGridTableRow.prototype.LoadSelected=function(){
this.ApplySelectedRowStyle();
this.Owner.AddToSelectedRows(this);
};
RadGridNamespace.RadGridTableRow.prototype.ApplySelectedRowStyle=function(){
if(!this.Owner.SelectedItemStyleClass||this.Owner.SelectedItemStyleClass==""){
if(this.Owner.SelectedItemStyle&&this.Owner.SelectedItemStyle!=""){
RadGridNamespace.addClassName(this.Control,"SelectedItemStyle"+this.Owner.ClientID+"1");
}else{
RadGridNamespace.addClassName(this.Control,"SelectedItemStyle"+this.Owner.ClientID+"2");
}
}else{
RadGridNamespace.addClassName(this.Control,this.Owner.SelectedItemStyleClass);
}
};
RadGridNamespace.RadGridTableRow.prototype.RemoveSelectedRowStyle=function(){
if(this.Owner.SelectedItemStyle){
RadGridNamespace.removeClassName(this.Control,"SelectedItemStyle"+this.Owner.ClientID+"1");
}else{
RadGridNamespace.removeClassName(this.Control,"SelectedItemStyle"+this.Owner.ClientID+"2");
}
RadGridNamespace.removeClassName(this.Control,this.Owner.SelectedItemStyleClass);
if(this.Control.style.cssText==this.Owner.SelectedItemStyle){
this.Control.style.cssText="";
}
};
RadGridNamespace.RadGridTableRow.prototype.OnDblClick=function(e){
try{
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowDblClick",[this.Control.sectionRowIndex,e])){
return;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableRow.prototype.CreateRowSelectorArea=function(e){
if((this.Owner.Owner.RowResizer)||(e.ctrlKey)){
return;
}
var _232=null;
if(e.srcElement){
_232=e.srcElement;
}else{
if(e.target){
_232=e.target;
}
}
if(!_232.tagName){
return;
}
if(_232.tagName.toLowerCase()=="input"){
return;
}
if((!this.Owner.Owner.ClientSettings.Selecting.AllowRowSelect)||(!this.Owner.Owner.AllowMultiRowSelection)){
return;
}
var _233=RadGridNamespace.GetCurrentElement(e);
if((!_233)||(!RadGridNamespace.IsChildOf(_233,this.Control))){
return;
}
if(!this.RowSelectorArea){
this.RowSelectorArea=document.createElement("span");
this.RowSelectorArea.style.backgroundColor="navy";
this.RowSelectorArea.style.border="indigo 1px solid";
this.RowSelectorArea.style.position="absolute";
this.RowSelectorArea.style.font="icon";
if(window.netscape&&!window.opera){
this.RowSelectorArea.style.MozOpacity=1/10;
}else{
if(window.opera||navigator.userAgent.indexOf("Safari")>-1){
this.RowSelectorArea.style.opacity=0.1;
}else{
this.RowSelectorArea.style.filter="alpha(opacity=10);";
}
}
if(this.Owner.Owner.GridDataDiv){
this.RowSelectorArea.style.top=RadGridNamespace.FindPosY(this.Control)-this.Owner.Owner.GridDataDiv.scrollTop+"px";
this.RowSelectorArea.style.left=RadGridNamespace.FindPosX(this.Control)-this.Owner.Owner.GridDataDiv.scrollLeft+"px";
if(parseInt(this.RowSelectorArea.style.left)<RadGridNamespace.FindPosX(this.Owner.Owner.Control)){
this.RowSelectorArea.style.left=RadGridNamespace.FindPosX(this.Owner.Owner.Control)+"px";
}
}else{
this.RowSelectorArea.style.top=RadGridNamespace.FindPosY(this.Control)+"px";
this.RowSelectorArea.style.left=RadGridNamespace.FindPosX(this.Control)+"px";
}
document.body.appendChild(this.RowSelectorArea);
this.FirstRow=this.Control;
RadGridNamespace.ClearDocumentEvents();
}
};
RadGridNamespace.RadGridTableRow.prototype.DestroyRowSelectorArea=function(e){
if(this.RowSelectorArea){
var _235=this.RowSelectorArea.style.height;
document.body.removeChild(this.RowSelectorArea);
this.RowSelectorArea=null;
RadGridNamespace.RestoreDocumentEvents();
var _236=RadGridNamespace.GetCurrentElement(e);
var _237;
if((!_236)||(!RadGridNamespace.IsChildOf(_236,this.Owner.Control))){
return;
}
if((_236.tagName.toLowerCase()=="td")||(_236.tagName.toLowerCase()=="tr")){
if(_236.tagName.toLowerCase()=="td"){
_237=_236.parentNode;
}else{
if(_236.tagName.toLowerCase()=="tr"){
_237=_236;
}
}
for(var i=this.FirstRow.rowIndex;i<_237.rowIndex+1;i++){
var _239=this.Owner.Owner.GetRowObjectByRealRow(this.Owner,this.Owner.Control.rows[i]);
if(_239){
if(_235!=""){
if(!_239.Selected){
this.Owner.SelectRow(this.Owner.Control.rows[i],false);
}
}
}
}
}
}
};
RadGridNamespace.RadGridTableRow.prototype.ResizeRowSelectorArea=function(e){
if((this.RowSelectorArea)&&(this.RowSelectorArea.parentNode)){
var _23b=RadGridNamespace.GetCurrentElement(e);
if((!_23b)||(!RadGridNamespace.IsChildOf(_23b,this.Owner.Control))){
return;
}
var _23c=parseInt(this.RowSelectorArea.style.left);
if(this.Owner.Owner.GridDataDiv){
var _23d=RadGridNamespace.GetEventPosX(e)-this.Owner.Owner.GridDataDiv.scrollLeft;
}else{
var _23d=RadGridNamespace.GetEventPosX(e);
}
var _23e=parseInt(this.RowSelectorArea.style.top);
if(this.Owner.Owner.GridDataDiv){
var _23f=RadGridNamespace.GetEventPosY(e)-this.Owner.Owner.GridDataDiv.scrollTop;
}else{
var _23f=RadGridNamespace.GetEventPosY(e);
}
if((_23d-_23c-5)>0){
this.RowSelectorArea.style.width=_23d-_23c-5+"px";
}
if((_23f-_23e-5)>0){
this.RowSelectorArea.style.height=_23f-_23e-5+"px";
}
if(this.RowSelectorArea.offsetWidth>this.Owner.Control.offsetWidth){
this.RowSelectorArea.style.width=this.Owner.Control.offsetWidth+"px";
}
var _240=(RadGridNamespace.FindPosX(this.Owner.Control)+this.Owner.Control.offsetHeight)-parseInt(this.RowSelectorArea.style.top);
if(this.RowSelectorArea.offsetHeight>_240){
if(_240>0){
this.RowSelectorArea.style.height=_240+"px";
}
}
}
};
RadGridNamespace.RadGridTableRow.prototype.OnMouseDown=function(e){
if(this.Owner.Owner.ClientSettings.Selecting.EnableDragToSelectRows&&this.Owner.Owner.AllowMultiRowSelection){
if(!this.Owner.Owner.RowResizer){
this.CreateRowSelectorArea(e);
}
}
};
RadGridNamespace.RadGridTableRow.prototype.OnMouseUp=function(e){
this.DestroyRowSelectorArea(e);
};
RadGridNamespace.RadGridTableRow.prototype.OnMouseMove=function(e){
this.ResizeRowSelectorArea(e);
};
RadGridNamespace.RadGridTableRow.prototype.OnMouseOver=function(e){
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowMouseOver",[this.Control.sectionRowIndex,e])){
return;
}
};
RadGridNamespace.RadGridTableRow.prototype.OnMouseOut=function(e){
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowMouseOut",[this.Control.sectionRowIndex,e])){
return;
}
};
RadGridNamespace.RadGridGroupPanel=function(_246,_247){
this.Control=_246;
this.Owner=_247;
this.Items=new Array();
this.groupPanelItemCounter=0;
this.getGroupPanelItems(this.Control,0);
var _248=this;
};
RadGridNamespace.RadGridGroupPanel.prototype.Dispose=function(){
this.UnLoadHandler=null;
this.Control=null;
this.Owner=null;
this.DisposeItems();
for(var _249 in this){
this[_249]=null;
}
};
RadGridNamespace.RadGridGroupPanel.prototype.DisposeItems=function(){
if(this.Items!=null){
for(var i=0;i<this.Items.length;i++){
var item=this.Items[i];
item.Dispose();
}
}
};
RadGridNamespace.RadGridGroupPanel.prototype.groupPanelItemCounter=0;
RadGridNamespace.RadGridGroupPanel.prototype.getGroupPanelItems=function(_24c){
for(var i=0;i<_24c.rows.length;i++){
var _24e=false;
var row=_24c.rows[i];
for(var j=0;j<row.cells.length;j++){
var cell=row.cells[j];
if(cell.tagName.toLowerCase()=="th"){
var _252;
if(this.Owner.GroupPanel.GroupPanelItems[this.groupPanelItemCounter]){
_252=this.Owner.GroupPanel.GroupPanelItems[this.groupPanelItemCounter].HierarchicalIndex;
}
if(_252){
this.Items[this.Items.length]=new RadGridNamespace.RadGridGroupPanelItem(cell,this,_252);
_24e=true;
this.groupPanelItemCounter++;
}
}
if((cell.firstChild)&&(cell.firstChild.tagName)){
if(cell.firstChild.tagName.toLowerCase()=="table"){
this.getGroupPanelItems(cell.firstChild);
}
}
}
}
};
RadGridNamespace.RadGridGroupPanel.prototype.IsItem=function(_253){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Control==_253){
return this.Items[i];
}
}
return null;
};
RadGridNamespace.RadGridGroupPanelItem=function(_255,_256,_257){
RadControlsNamespace.DomEventMixin.Initialize(this);
this.Control=_255;
this.Owner=_256;
this.HierarchicalIndex=_257;
this.Control.style.cursor="move";
this.AttachDomEvent(this.Control,"mousedown","OnMouseDown");
};
RadGridNamespace.RadGridGroupPanelItem.prototype.Dispose=function(){
this.DisposeDomEventHandlers();
for(var _258 in this){
this[_258]=null;
}
this.Control=null;
this.Owner=null;
};
RadGridNamespace.RadGridGroupPanelItem.prototype.OnMouseDown=function(e){
if(((window.netscape||window.opera)&&(e.button==0))||(e.button==1)){
this.CreateDragDrop(e);
this.CreateReorderIndicators(this.Control);
this.AttachDomEvent(document,"mouseup","OnMouseUp");
this.AttachDomEvent(document,"mousemove","OnMouseMove");
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.OnMouseUp=function(e){
this.FireDropAction(e);
this.DestroyDragDrop(e);
this.DestroyReorderIndicators();
this.DetachDomEvent(document,"mouseup","OnMouseUp");
this.DetachDomEvent(document,"mousemove","OnMouseMove");
};
RadGridNamespace.RadGridGroupPanelItem.prototype.OnMouseMove=function(e){
this.MoveDragDrop(e);
};
RadGridNamespace.RadGridGroupPanelItem.prototype.FireDropAction=function(e){
var _25d=RadGridNamespace.GetCurrentElement(e);
if(_25d!=null){
if(!RadGridNamespace.IsChildOf(_25d,this.Owner.Control)){
this.Owner.Owner.SavePostData("UnGroupByExpression",this.HierarchicalIndex);
eval(this.Owner.Owner.ClientSettings.PostBackReferences.PostBackUnGroupByExpression);
}else{
var item=this.Owner.IsItem(_25d);
if((_25d!=this.Control)&&(item!=null)&&(_25d.parentNode==this.Control.parentNode)){
this.Owner.Owner.SavePostData("ReorderGroupByExpression",this.HierarchicalIndex,item.HierarchicalIndex);
eval(this.Owner.Owner.ClientSettings.PostBackReferences.PostBackReorderGroupByExpression);
}
if(window.netscape){
this.Control.style.MozOpacity=4/4;
}else{
this.Control.style.filter="alpha(opacity=100);";
}
}
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.CreateDragDrop=function(e){
this.MoveHeaderDiv=document.createElement("div");
var _260=document.createElement("table");
if(this.MoveHeaderDiv.mergeAttributes){
this.MoveHeaderDiv.mergeAttributes(this.Owner.Owner.Control);
}else{
RadGridNamespace.CopyAttributes(this.MoveHeaderDiv,this.Control);
}
if(_260.mergeAttributes){
_260.mergeAttributes(this.Owner.Control);
}else{
RadGridNamespace.CopyAttributes(_260,this.Owner.Control);
}
_260.style.margin="0px";
_260.style.height=this.Control.offsetHeight+"px";
_260.style.width=this.Control.offsetWidth+"px";
_260.style.border="0px";
_260.style.borderCollapse="collapse";
_260.style.padding="0px";
var _261=document.createElement("thead");
var tr=document.createElement("tr");
_260.appendChild(_261);
_261.appendChild(tr);
tr.appendChild(this.Control.cloneNode(true));
this.MoveHeaderDiv.appendChild(_260);
document.body.appendChild(this.MoveHeaderDiv);
this.MoveHeaderDiv.style.height=_260.style.height;
this.MoveHeaderDiv.style.width=_260.style.width;
this.MoveHeaderDiv.style.position="absolute";
RadGridNamespace.RadGrid.PositionDragElement(this.MoveHeaderDiv,e);
if(window.netscape){
this.MoveHeaderDiv.style.MozOpacity=3/4;
}else{
this.MoveHeaderDiv.style.filter="alpha(opacity=75);";
}
this.MoveHeaderDiv.style.cursor="move";
this.MoveHeaderDiv.style.display="none";
this.MoveHeaderDiv.onmousedown=null;
RadGridNamespace.ClearDocumentEvents();
};
RadGridNamespace.RadGridGroupPanelItem.prototype.DestroyDragDrop=function(e){
if(this.MoveHeaderDiv!=null){
var _264=this.MoveHeaderDiv.parentNode;
_264.removeChild(this.MoveHeaderDiv);
this.MoveHeaderDiv.onmouseup=null;
this.MoveHeaderDiv.onmousemove=null;
this.MoveHeaderDiv=null;
RadGridNamespace.RestoreDocumentEvents();
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.MoveDragDrop=function(e){
if(this.MoveHeaderDiv!=null){
if(window.netscape){
this.Control.style.MozOpacity=1/4;
}else{
this.Control.style.filter="alpha(opacity=25);";
}
this.MoveHeaderDiv.style.visibility="";
this.MoveHeaderDiv.style.display="";
RadGridNamespace.RadGrid.PositionDragElement(this.MoveHeaderDiv,e);
var _266=RadGridNamespace.GetCurrentElement(e);
if(_266!=null){
if(RadGridNamespace.IsChildOf(_266,this.Owner.Control)){
var item=this.Owner.IsItem(_266);
if((_266!=this.Control)&&(item!=null)&&(_266.parentNode==this.Control.parentNode)){
this.MoveReorderIndicators(e,_266);
}else{
this.ReorderIndicator1.style.visibility="hidden";
this.ReorderIndicator1.style.display="none";
this.ReorderIndicator1.style.position="absolute";
this.ReorderIndicator2.style.visibility=this.ReorderIndicator1.style.visibility;
this.ReorderIndicator2.style.display=this.ReorderIndicator1.style.display;
this.ReorderIndicator2.style.position=this.ReorderIndicator1.style.position;
}
}
}
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.CreateReorderIndicators=function(_268){
if((this.ReorderIndicator1==null)&&(this.ReorderIndicator2==null)){
this.ReorderIndicator1=document.createElement("span");
this.ReorderIndicator2=document.createElement("span");
this.ReorderIndicator1.innerHTML="&darr;";
this.ReorderIndicator2.innerHTML="&uarr;";
this.ReorderIndicator1.style.backgroundColor="transparent";
this.ReorderIndicator1.style.color="darkblue";
this.ReorderIndicator1.style.font="bold 18px Arial";
this.ReorderIndicator2.style.backgroundColor=this.ReorderIndicator1.style.backgroundColor;
this.ReorderIndicator2.style.color=this.ReorderIndicator1.style.color;
this.ReorderIndicator2.style.font=this.ReorderIndicator1.style.font;
this.ReorderIndicator1.style.top=RadGridNamespace.FindPosY(_268)-this.ReorderIndicator1.offsetHeight+"px";
this.ReorderIndicator1.style.left=RadGridNamespace.FindPosX(_268)+"px";
this.ReorderIndicator2.style.top=RadGridNamespace.FindPosY(_268)+_268.offsetHeight+"px";
this.ReorderIndicator2.style.left=this.ReorderIndicator1.style.left;
this.ReorderIndicator1.style.visibility="hidden";
this.ReorderIndicator1.style.display="none";
this.ReorderIndicator1.style.position="absolute";
this.ReorderIndicator2.style.visibility=this.ReorderIndicator1.style.visibility;
this.ReorderIndicator2.style.display=this.ReorderIndicator1.style.display;
this.ReorderIndicator2.style.position=this.ReorderIndicator1.style.position;
document.body.appendChild(this.ReorderIndicator1);
document.body.appendChild(this.ReorderIndicator2);
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.DestroyReorderIndicators=function(){
if((this.ReorderIndicator1!=null)&&(this.ReorderIndicator2!=null)){
document.body.removeChild(this.ReorderIndicator1);
document.body.removeChild(this.ReorderIndicator2);
this.ReorderIndicator1=null;
this.ReorderIndicator2=null;
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.MoveReorderIndicators=function(e,_26a){
if((this.ReorderIndicator1!=null)&&(this.ReorderIndicator2!=null)){
this.ReorderIndicator1.style.visibility="visible";
this.ReorderIndicator1.style.display="";
this.ReorderIndicator2.style.visibility="visible";
this.ReorderIndicator2.style.display="";
this.ReorderIndicator1.style.top=RadGridNamespace.FindPosY(_26a)-this.ReorderIndicator1.offsetHeight+"px";
this.ReorderIndicator1.style.left=RadGridNamespace.FindPosX(_26a)+"px";
this.ReorderIndicator2.style.top=RadGridNamespace.FindPosY(_26a)+_26a.offsetHeight+"px";
this.ReorderIndicator2.style.left=this.ReorderIndicator1.style.left;
}
};
RadGridNamespace.RadGridMenu=function(_26b,_26c,_26d){
if(!_26b||!_26c){
return;
}
RadControlsNamespace.DomEventMixin.Initialize(this);
for(var _26e in _26b){
this[_26e]=_26b[_26e];
}
this.Owner=_26c;
this.ItemData=_26b.Items;
this.Items=[];
};
RadGridNamespace.RadGridMenu.prototype.Initialize=function(){
if(this.Control!=null){
return;
}
this.Control=document.createElement("table");
this.Control.style.backgroundColor=this.SelectColumnBackColor;
this.Control.style.border="outset 1px";
this.Control.style.fontSize="small";
this.Control.style.textAlign="left";
this.Control.cellPadding="0";
this.Control.style.borderCollapse="collapse";
this.Control.style.zIndex=998;
this.Items=this.CreateItems(this.ItemData);
this.Control.style.position="absolute";
this.Control.style.display="none";
document.body.appendChild(this.Control);
var _26f=document.createElement("img");
_26f.src=this.SelectedImageUrl;
_26f.src=this.NotSelectedImageUrl;
this.Control.className=this.CssClass;
};
RadGridNamespace.RadGridMenu.prototype.Dispose=function(){
this.DisposeDomEventHandlers();
this.DisposeItems();
this.ItemData=null;
this.Owner=null;
this.Control=null;
};
RadGridNamespace.RadGridMenu.prototype.CreateItems=function(_270){
var _271=[];
for(var i=0;i<_270.length;i++){
_271[_271.length]=new RadGridNamespace.RadGridMenuItem(_270[i],this);
}
return _271;
};
RadGridNamespace.RadGridMenu.prototype.DisposeItems=function(){
for(var i=0;i<this.Items.length;i++){
var item=this.Items[i];
item.Dispose();
}
this.Items=null;
};
RadGridNamespace.RadGridMenu.prototype.HideItem=function(_275){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Value==_275){
this.Items[i].Control.style.display="none";
}
}
};
RadGridNamespace.RadGridMenu.prototype.ShowItem=function(_277){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Value==_277){
this.Items[i].Control.style.display="";
}
}
};
RadGridNamespace.RadGridMenu.prototype.SelectItem=function(_279){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Value==_279){
this.Items[i].Selected=true;
this.Items[i].SelectImage.src=this.SelectedImageUrl;
}else{
this.Items[i].Selected=false;
this.Items[i].SelectImage.src=this.NotSelectedImageUrl;
}
}
};
RadGridNamespace.RadGridMenu.prototype.Show=function(_27b,_27c,e){
this.Initialize();
this.Control.style.display="";
this.Control.style.top=e.clientY+document.documentElement.scrollTop+document.body.scrollTop+5+"px";
this.Control.style.left=e.clientX+document.documentElement.scrollLeft+document.body.scrollLeft+5+"px";
this.AttachHideEvents();
};
RadGridNamespace.RadGridMenu.prototype.OnKeyPress=function(e){
if(e.keyCode==27){
this.DetachHideEvents();
this.Hide();
}
};
RadGridNamespace.RadGridMenu.prototype.OnClick=function(e){
if(!e.cancelBubble){
this.DetachHideEvents();
this.Hide();
}
};
RadGridNamespace.RadGridMenu.prototype.AttachHideEvents=function(){
this.AttachDomEvent(document,"keypress","OnKeyPress");
this.AttachDomEvent(document,"click","OnClick");
};
RadGridNamespace.RadGridMenu.prototype.DetachHideEvents=function(){
this.DetachDomEvent(document,"keypress","OnKeyPress");
this.DetachDomEvent(document,"click","OnClick");
};
RadGridNamespace.RadGridMenu.prototype.Hide=function(){
if(this.Control.style.display==""){
this.Control.style.display="none";
}
};
RadGridNamespace.RadGridMenuItem=function(_280,_281){
for(var _282 in _280){
this[_282]=_280[_282];
}
this.Owner=_281;
this.Control=this.Owner.Control.insertRow(-1);
this.Control.insertCell(-1);
var _283=document.createElement("table");
_283.style.width="100%";
_283.cellPadding="0";
_283.cellSpacing="0";
_283.insertRow(-1);
var td1=_283.rows[0].insertCell(-1);
var td2=_283.rows[0].insertCell(-1);
td1.style.borderTop="solid 1px "+this.Owner.SelectColumnBackColor;
td1.style.borderLeft="solid 1px "+this.Owner.SelectColumnBackColor;
td1.style.borderRight="none 0px";
td1.style.borderBottom="solid 1px "+this.Owner.SelectColumnBackColor;
td1.style.padding="2px";
td1.style.textAlign="center";
td1.style.width="16px";
td1.appendChild(document.createElement("img"));
td1.childNodes[0].src=this.Owner.NotSelectedImageUrl;
this.SelectImage=td1.childNodes[0];
td2.style.borderTop="solid 1px "+this.Owner.TextColumnBackColor;
td2.style.borderLeft="none 0px";
td2.style.borderRight="solid 1px "+this.Owner.TextColumnBackColor;
td2.style.borderBottom="solid 1px "+this.Owner.TextColumnBackColor;
td2.style.padding="2px";
td2.innerHTML=this.Text;
td2.style.backgroundColor=this.Owner.TextColumnBackColor;
td2.style.cursor="hand";
this.Control.cells[0].appendChild(_283);
var _286=this;
this.Control.onclick=function(){
if(_286.Owner.Owner.Owner.EnableAJAX){
if(_286.Owner.Owner==_286.Owner.Owner.Owner.MasterTableViewHeader){
RadGridNamespace.AsyncRequest(_286.UID,_286.Owner.Owner.Owner.MasterTableView.UID+"!"+_286.Owner.Column.UniqueName,_286.Owner.Owner.Owner.ClientID);
}else{
RadGridNamespace.AsyncRequest(_286.UID,_286.Owner.Owner.UID+"!"+_286.Owner.Column.UniqueName,_286.Owner.Owner.Owner.ClientID);
}
}else{
var _287=_286.Owner.Owner.Owner.ClientSettings.PostBackFunction;
if(_286.Owner.Owner==_286.Owner.Owner.Owner.MasterTableViewHeader){
_287=_287.replace("{0}",_286.UID).replace("{1}",_286.Owner.Owner.Owner.MasterTableView.UID+"!"+_286.Owner.Column.UniqueName);
}else{
_287=_287.replace("{0}",_286.UID).replace("{1}",_286.Owner.Owner.UID+"!"+_286.Owner.Column.UniqueName);
}
eval(_287);
}
};
this.Control.onmouseover=function(e){
this.cells[0].childNodes[0].rows[0].cells[0].style.backgroundColor=_286.Owner.HoverBackColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.borderTop="solid 1px "+_286.Owner.HoverBorderColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.borderLeft="solid 1px "+_286.Owner.HoverBorderColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.borderBottom="solid 1px "+_286.Owner.HoverBorderColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.backgroundColor=_286.Owner.HoverBackColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.borderTop="solid 1px "+_286.Owner.HoverBorderColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.borderRight="solid 1px "+_286.Owner.HoverBorderColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.borderBottom="solid 1px "+_286.Owner.HoverBorderColor;
};
this.Control.onmouseout=function(e){
this.cells[0].childNodes[0].rows[0].cells[0].style.borderTop="solid 1px "+_286.Owner.SelectColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.borderLeft="solid 1px "+_286.Owner.SelectColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.borderBottom="solid 1px "+_286.Owner.SelectColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.backgroundColor="";
this.cells[0].childNodes[0].rows[0].cells[1].style.borderTop="solid 1px "+_286.Owner.TextColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.borderRight="solid 1px "+_286.Owner.TextColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.borderBottom="solid 1px "+_286.Owner.TextColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.backgroundColor=_286.Owner.TextColumnBackColor;
};
};
RadGridNamespace.RadGridMenuItem.prototype.Dispose=function(){
this.Control.onclick=null;
this.Control.onmouseover=null;
this.Control.onmouseout=null;
var _28a=this.Control.getElementsByTagName("table");
while(_28a.length>0){
var _28b=_28a[0];
if(_28b.parentNode!=null){
_28b.parentNode.removeChild(_28b);
}
}
this.Control=null;
this.Owner=null;
};
RadGridNamespace.RadGridFilterMenu=function(_28c,_28d){
RadGridNamespace.RadGridMenu.call(this,_28c,_28d);
};
RadGridNamespace.RadGridFilterMenu.prototype=new RadGridNamespace.RadGridMenu;
RadGridNamespace.RadGridFilterMenu.prototype.Show=function(_28e,e){
this.Initialize();
if(!_28e){
return;
}
this.Owner=_28e.Owner;
this.Column=_28e;
for(var i=0;i<this.Items.length;i++){
if(_28e.DataTypeName!="System.String"){
if((this.Items[i].Value=="StartsWith")||(this.Items[i].Value=="EndsWith")||(this.Items[i].Value=="Contains")||(this.Items[i].Value=="DoesNotContain")||(this.Items[i].Value=="IsEmpty")||(this.Items[i].Value=="NotIsEmpty")){
this.Items[i].Control.style.display="none";
continue;
}
}
if(_28e.FilterListOptions=="VaryByDataType"){
if(this.Items[i].Value=="Custom"){
this.Items[i].Control.style.display="none";
continue;
}
}
this.Items[i].Control.style.display="";
}
this.SelectItem(_28e.CurrentFilterFunction);
this.Control.style.display="";
this.Control.style.top=e.clientY+document.documentElement.scrollTop+document.body.scrollTop+5+"px";
this.Control.style.left=e.clientX+document.documentElement.scrollLeft+document.body.scrollLeft+5+"px";
this.AttachHideEvents();
};
RadGridNamespace.RadGrid.prototype.InitializeFilterMenu=function(_291){
if(this.AllowFilteringByColumn||_291.AllowFilteringByColumn){
if(!_291||!_291.Control){
return;
}
if(!_291.Control.tHead){
return;
}
if(!_291.IsItemInserted){
var _292=_291.Control.tHead.rows[_291.Control.tHead.rows.length-1];
}else{
var _292=_291.Control.tHead.rows[_291.Control.tHead.rows.length-2];
}
if(!_292){
return;
}
var _293=_292.getElementsByTagName("img");
var _294=this;
if(!_291.Columns){
return;
}
if(!_291.Columns[0]){
return;
}
var _295=_291.Columns[0].FilterImageUrl;
for(var i=0;i<_293.length;i++){
if(_293[i].getAttribute("src").indexOf(_295)==-1){
continue;
}
_293[i].onclick=function(e){
if(!e){
var e=window.event;
}
e.cancelBubble=true;
var _298=this.parentNode.cellIndex;
if(window.attachEvent&&!window.opera&&!window.netscape){
_298=RadGridNamespace.GetRealCellIndexFormCells(this.parentNode.parentNode.cells,this.parentNode);
}
_294.FilteringMenu.Show(_291.Columns[_298],e);
if(e.preventDefault){
e.preventDefault();
}else{
e.returnValue=false;
return false;
}
};
}
this.FilteringMenu=new RadGridNamespace.RadGridFilterMenu(this.FilterMenu,_291);
}
};
RadGridNamespace.RadGrid.prototype.DisposeFilterMenu=function(_299){
if(this.FilteringMenu!=null){
this.FilteringMenu.Dispose();
this.FilteringMenu=null;
}
};
RadGridNamespace.GetRealCellIndexFormCells=function(_29a,cell){
for(var i=0;i<_29a.length;i++){
if(_29a[i]==cell){
return i;
}
}
};
if(typeof (window.RadGridNamespace)=="undefined"){
window.RadGridNamespace=new Object();
}
RadGridNamespace.Slider=function(_29d){
RadControlsNamespace.DomEventMixin.Initialize(this);
if(!document.readyState||document.readyState=="complete"||window.opera){
this._constructor(_29d);
}else{
this.objectData=_29d;
this.AttachDomEvent(window,"load","OnWindowLoad");
}
};
RadGridNamespace.Slider.prototype.OnWindowLoad=function(e){
this.DetachDomEvent(window,"load","OnWindowLoad");
this._constructor(this.objectData);
this.objectData=null;
};
RadGridNamespace.Slider.prototype._constructor=function(_29f){
var _2a0=this;
for(var _2a1 in _29f){
this[_2a1]=_29f[_2a1];
}
this.Owner=window[this.OwnerID];
this.OwnerGrid=window[this.OwnerGridID];
this.Control=document.getElementById(this.ClientID);
this.Control.unselectable="on";
this.Control.parentNode.style.padding="10px";
this.ToolTip=document.createElement("div");
this.ToolTip.unselectable="on";
this.ToolTip.style.backgroundColor="#F5F5DC";
this.ToolTip.style.border="1px outset";
this.ToolTip.style.font="icon";
this.ToolTip.style.padding="2px";
this.ToolTip.style.marginTop="5px";
this.ToolTip.style.marginBottom="15px";
this.Control.appendChild(this.ToolTip);
this.Line=document.createElement("hr");
this.Line.unselectable="on";
this.Line.style.width="100%";
this.Line.style.height="2px";
this.Line.style.backgroundColor="buttonface";
this.Line.style.border="1px outset threedshadow";
this.Control.appendChild(this.Line);
this.Thumb=document.createElement("div");
this.Thumb.unselectable="on";
this.Thumb.style.position="relative";
this.Thumb.style.width="8px";
this.Thumb.style.marginTop="-15px";
this.Thumb.style.height="16px";
this.Thumb.style.backgroundColor="buttonface";
this.Thumb.style.border="1px outset threedshadow";
this.Control.appendChild(this.Thumb);
this.Link=document.createElement("a");
this.Link.unselectable="on";
this.Link.style.width="100%";
this.Link.style.height="100%";
this.Link.style.display="block";
this.Link.href="javascript:void(0);";
this.Thumb.appendChild(this.Link);
this.LineX=RadGridNamespace.FindPosX(this.Line);
this.AttachDomEvent(this.Control,"mousedown","OnMouseDown");
this.AttachDomEvent(this.Link,"keydown","OnKeyDown");
var _2a2=this.OwnerGrid.CurrentPageIndex/this.OwnerGrid.MasterTableView.PageCount;
this.SetPosition(_2a2*this.Line.offsetWidth);
var _2a3=parseInt(this.Thumb.style.left)/this.Line.offsetWidth;
var _2a4=Math.round((this.OwnerGrid.MasterTableView.PageCount-1)*_2a3);
this.ToolTip.innerHTML="Page: <b>"+(this.OwnerGrid.CurrentPageIndex+1)+"</b> out of <b>"+this.OwnerGrid.MasterTableView.PageCount+"</b> pages";
};
RadGridNamespace.Slider.prototype.Dispose=function(){
this.DisposeDomEventHandlers();
for(var _2a5 in this){
this[_2a5]=null;
}
this.Control=null;
this.Line=null;
this.Thumb=null;
this.ToolTip=null;
};
RadGridNamespace.Slider.prototype.OnKeyDown=function(e){
this.AttachDomEvent(this.Link,"keyup","OnKeyUp");
if(e.keyCode==39){
this.SetPosition(parseInt(this.Thumb.style.left)+this.Thumb.offsetWidth);
}
if(e.keyCode==37){
this.SetPosition(parseInt(this.Thumb.style.left)-this.Thumb.offsetWidth);
}
if(e.keyCode==39||e.keyCode==37){
var _2a7=parseInt(this.Thumb.style.left)/this.Line.offsetWidth;
var _2a8=Math.round((this.OwnerGrid.MasterTableView.PageCount-1)*_2a7);
this.ToolTip.innerHTML="Page: <b>"+((_2a8==0)?1:_2a8+1)+"</b> out of <b>"+this.OwnerGrid.MasterTableView.PageCount+"</b> pages";
}
};
RadGridNamespace.Slider.prototype.OnKeyUp=function(e){
this.DetachDomEvent(this.Link,"keyup","OnKeyUp");
if(e.keyCode==39||e.keyCode==37){
var _2aa=this;
setTimeout(function(){
_2aa.ChangePage();
},100);
}
};
RadGridNamespace.Slider.prototype.OnMouseDown=function(e){
this.DetachDomEvent(this.Control,"mousedown","OnMouseDown");
if(((window.netscape||window.opera)&&(e.button==0))||(e.button==1)){
this.SetPosition(RadGridNamespace.GetEventPosX(e)-this.LineX);
this.AttachDomEvent(document,"mousemove","OnMouseMove");
this.AttachDomEvent(document,"mouseup","OnMouseUp");
}
};
RadGridNamespace.Slider.prototype.OnMouseUp=function(e){
this.DetachDomEvent(document,"mousemove","OnMouseMove");
this.DetachDomEvent(document,"mouseup","OnMouseUp");
var _2ad=parseInt(this.Thumb.style.left)/this.Line.offsetWidth;
var _2ae=Math.round((this.OwnerGrid.MasterTableView.PageCount-1)*_2ad);
this.ToolTip.innerHTML="Page: <b>"+((_2ae==0)?1:_2ae+1)+"</b> out of <b>"+this.OwnerGrid.MasterTableView.PageCount+"</b> pages";
var _2af=this;
setTimeout(function(){
_2af.ChangePage();
},100);
};
RadGridNamespace.Slider.prototype.OnMouseMove=function(e){
this.SetPosition(RadGridNamespace.GetEventPosX(e)-this.LineX);
var _2b1=parseInt(this.Thumb.style.left)/this.Line.offsetWidth;
var _2b2=Math.round((this.OwnerGrid.MasterTableView.PageCount-1)*_2b1);
this.ToolTip.innerHTML="Page: <b>"+((_2b2==0)?1:_2b2+1)+"</b> out of <b>"+this.OwnerGrid.MasterTableView.PageCount+"</b> pages";
};
RadGridNamespace.Slider.prototype.GetPosition=function(e){
this.SetPosition(RadGridNamespace.GetEventPosX(e)-this.LineX);
};
RadGridNamespace.Slider.prototype.SetPosition=function(_2b4){
if(_2b4>=0&&_2b4<=this.Line.offsetWidth){
this.Thumb.style.left=_2b4+"px";
}
};
RadGridNamespace.Slider.prototype.ChangePage=function(){
var _2b5=parseInt(this.Thumb.style.left)/this.Line.offsetWidth;
var _2b6=Math.round((this.OwnerGrid.MasterTableView.PageCount-1)*_2b5);
if(this.OwnerGrid.CurrentPageIndex==_2b6){
this.AttachDomEvent(this.Control,"mousedown","OnMouseDown");
return;
}
this.OwnerGrid.SavePostData("AJAXScrolledControl",(this.OwnerGrid.GridDataDiv)?this.OwnerGrid.GridDataDiv.scrollLeft:"",(this.OwnerGrid.GridDataDiv)?this.OwnerGrid.LastScrollTop:"",(this.OwnerGrid.GridDataDiv)?this.OwnerGrid.GridDataDiv.scrollTop:"",_2b6);
var _2b7=this.OwnerGrid.ClientSettings.PostBackFunction;
_2b7=_2b7.replace("{0}",this.OwnerGrid.UniqueID);
eval(_2b7);
};

//BEGIN_ATLAS_NOTIFY
if (typeof(Sys) != "undefined"){if (Sys.Application != null && Sys.Application.notifyScriptLoaded != null){Sys.Application.notifyScriptLoaded();}}
//END_ATLAS_NOTIFY
