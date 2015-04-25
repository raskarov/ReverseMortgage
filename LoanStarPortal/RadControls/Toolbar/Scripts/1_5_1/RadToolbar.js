if(typeof (window.RadToolbarNamespace)=="undefined"){
window.RadToolbarNamespace=new Object();
}
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
function RadToolbar(_5){
this.ClientID=_5;
if(typeof (window[_5])!="undefined"&&window[_5].Dispose){
window[_5].Dispose();
}
this.ControlElement=document.getElementById(this.ClientID+"toolbar");
this.ALT_Code=18;
this.CTRL_Code=17;
this.ShortcutKeyPressed=false;
var _6=this;
this.OnKeyDownHandler=function(e){
_6.OnKeyDown(e);
};
this.OnKeyUpHandler=function(e){
_6.OnKeyUp(e);
};
this.OnUnloadHandler=function(e){
_6.Dispose(e);
};
this._AttachEvent(document,"keydown",this.OnKeyDownHandler);
this._AttachEvent(document,"keyup",this.OnKeyUpHandler);
this._AttachEvent(window,"unload",this.OnUnloadHandler);
}
RadToolbar.prototype.Dispose=function(){
this._DetachEvent(window,"unload",this.OnUnloadHandler);
this._DetachEvent(document,"keydown",this.OnKeyDownHandler);
this._DetachEvent(document,"keyup",this.OnKeyUpHandler);
this.ControlElement=null;
if(this.ButtonInstanceArray){
for(var i=0;i<this.ButtonInstanceArray.length;i++){
this.ButtonInstanceArray[i].Dispose();
}
}
};
RadToolbar.prototype.Start=function(){
var _b=this;
this.OnLoadHandler=function(e){
_b.InitializeDock(e);
};
this._AttachEvent(window,"load",this.OnLoadHandler);
this.InitializeButtons();
this.UpdateState();
};
RadToolbar.prototype.InitializeDock=function(e){
if(!this.DockableObjectID){
return;
}
var _e=RadDock_GetDockableObject(this.DockableObjectID);
var _f=this;
_e.radToolbar=_f;
_e.AddEventHandler("DockStateChanged",_f.DockableObjectHandler);
};
RadToolbar.prototype.DockableObjectHandler=function(_10,e){
var _12=_10.radToolbar;
if(!_12){
return;
}
var _13=true;
if(e.docked){
_13=((_10.ParentDockingZone.Type&RadDockingZoneTypeFlags.Horizontal)>0);
}
if(_13){
if(this.dockableObject_height&&this.dockableObject_width){
_10.style.width=this.dockableObject_width+"px";
_10.style.height=this.dockableObject_height+"px";
}
_12.MakeHorizontal();
}else{
this.dockableObject_height=_10.offsetHeight;
this.dockableObject_width=_10.offsetWidth;
_10.style.width="1px";
_10.style.height="1px";
_12.MakeVertical();
}
};
RadToolbarNamespace.CancelEvent=function(_14){
_14.returnValue=false;
_14.cancelBubble=true;
if(_14.stopPropagation){
_14.stopPropagation();
}
return false;
};
RadToolbar.prototype.attachEvent=function(_15,_16){
this.AttachEvent(_15,_16);
};
RadToolbar.prototype.AttachEvent=function(_17,_18){
var _19=this.FunctionFromVariable(_18);
var _1a=function(_1b,e){
return _19(_1b,e);
};
this[_17+"Handler"]=_1a;
};
RadToolbar.prototype._AttachEvent=function(_1d,_1e,_1f){
try{
if(_1d.addEventListener){
_1d.addEventListener(_1e,_1f,true);
}else{
if(_1d.attachEvent){
_1d.attachEvent("on"+_1e,_1f);
}
}
}
catch(error){
}
};
RadToolbar.prototype._DetachEvent=function(_20,_21,_22){
try{
if(_20.removeEventListener){
_20.removeEventListener(_21,_22,true);
}else{
if(_20.detachEvent){
_20.detachEvent("on"+_21,_22);
}
}
}
catch(error){
}
};
RadToolbar.prototype.FunctionFromVariable=function(_23){
var _24=function(_25,e){
};
if(typeof (_23).toString().toLowerCase()=="function"){
_24=_23;
}else{
if(typeof (_23).toString().toLowerCase()=="string"){
try{
_24=eval(_23);
}
catch(error){
}
}
}
return _24;
};
RadToolbar.prototype.FireOnClientOrientationChanged=function(_27,e){
if(this.OnClientOrientationChangedHandler!=null){
var _29=this.OnClientOrientationChangedHandler;
return _29(_27,e);
}
};
RadToolbar.prototype.FireOnClientMouseOver=function(_2a,e){
if(this.OnClientMouseOverHandler!=null){
var _2c=this.OnClientMouseOverHandler;
return _2c(_2a,e);
}
};
RadToolbar.prototype.FireOnClientMouseOut=function(_2d,e){
if(this.OnClientMouseOutHandler!=null){
var _2f=this.OnClientMouseOutHandler;
return _2f(_2d,e);
}
};
RadToolbar.prototype.FireOnClientClick=function(_30,e){
if(this.CausesValidation&&_30.CausesValidation&&!this.PostBackEventOptions){
if(!(typeof (Page_ClientValidate)!="function"||Page_ClientValidate())){
return;
}
}
if(this.OnClientClickHandler!=null){
var _32=this.OnClientClickHandler;
var _33=_32(_30,e);
if(_33==false){
return;
}
}
this.UpdateState();
if(this.AutoPostBack){
if(this.PostBackEventOptions){
if(!_30.CausesValidation){
var _34=this.PostBackFunction;
var _35=_34.replace(/@@arguments@@/g,"onclick#"+_30.CommandName);
}else{
var _35=this.PostBackEventOptions.replace(/@@argument@@/g,"onclick#"+_30.CommandName);
}
eval(_35);
}else{
var _36=this.PostBackFunction.replace(/@@arguments@@/g,"onclick#"+_30.CommandName);
eval(_36);
}
}
};
RadToolbar.prototype.FireOnClientMouseDown=function(_37,e){
if(this.OnClientMouseDownHandler!=null){
var _39=this.OnClientMouseDownHandler;
return _39(_37,e);
}
};
RadToolbar.prototype.FireOnClientButtonToggled=function(_3a,e){
if(this.OnClientButtonToggledHandler!=null){
var _3c=this.OnClientButtonToggledHandler;
return _3c(_3a,e);
}
};
RadToolbar.prototype.OnKeyDown=function(e){
if(e.keyCode==this.ALT_Code&&this.ShortcutKey.toLowerCase()=="alt"){
this.ShortcutKeyPressed=true;
}
if(e.keyCode==this.CTRL_Code&&this.ShortcutKey.toLowerCase()=="control"){
this.ShortcutKeyPressed=true;
}
if(this.ShortcutKeyPressed&&(e.keyCode!=this.ALT_Code&&e.keyCode!=this.CTRL_Code)){
for(var i=0;i<this.NumberOfButtons;i++){
var _3f=this.ButtonInstanceArray[i].AccessKey;
if(_3f&&_3f.toLowerCase()==String.fromCharCode(e.keyCode).toLowerCase()&&this.ButtonInstanceArray[i].Enabled){
this.ButtonInstanceArray[i].EventClick();
}
}
}
};
RadToolbar.prototype.OnKeyUp=function(e){
if(e.keyCode==this.ALT_Code&&this.ShortcutKey.toLowerCase()=="alt"){
this.ShortcutKeyPressed=false;
}
if(e.keyCode==this.CTRL_Code&&this.ShortcutKey.toLowerCase()=="control"){
this.ShortcutKeyPressed=false;
}
};
RadToolbar.prototype.MakeVertical=function(){
if(this.Orientation.toLowerCase()!="vertical"){
this.RenderVertical();
}
};
RadToolbar.prototype.MakeHorizontal=function(){
if(this.Orientation.toLowerCase()!="horizontal"){
this.RenderHorizontal();
}
};
RadToolbar.prototype.ToggleONButton=function(_41){
this.ToggleButton(true,_41);
};
RadToolbar.prototype.ToggleOFFButton=function(_42){
this.ToggleButton(false,_42);
};
RadToolbar.prototype.DisableButton=function(_43){
var _44=this.GetButtonByCommand(_43);
if(_44==-1){
alert("Cannot find a button with such command name!");
return;
}
_44.DisableButton();
this.UpdateState();
};
RadToolbar.prototype.EnableButton=function(_45){
var _46=this.GetButtonByCommand(_45);
if(_46==-1){
alert("Cannot find a button with such command name!");
return;
}
_46.EnableButton();
this.UpdateState();
};
RadToolbar.prototype.ShowButton=function(_47){
var _48=this.GetButtonByCommand(_47);
if(_48==-1){
alert("Cannot find a button with such command name!");
return;
}
_48.ShowButton();
this.UpdateState();
};
RadToolbar.prototype.HideButton=function(_49){
var _4a=this.GetButtonByCommand(_49);
if(_4a==-1){
alert("Cannot find a button with such command name!");
return;
}
_4a.HideButton();
this.UpdateState();
};
RadToolbar.prototype.Serialize=function(){
var _4b="";
_4b+=this.Orientation+"#";
if(this.NumberOfButtons<1){
return _4b;
}
for(var i=0;i<(this.NumberOfButtons-1);i++){
_4b+=this.ButtonInstanceArray[i].Serialize()+"#";
}
_4b+=this.ButtonInstanceArray[i].Serialize();
return _4b;
};
RadToolbar.prototype.GetButtonByCommand=function(_4d){
for(var i=0;i<this.NumberOfButtons;i++){
if(this.ButtonCommandArray[i]==_4d){
return this.ButtonInstanceArray[i];
}
}
return -1;
};
RadToolbar.prototype.CheckOrientation=function(){
if(this.ControlElement.rows.length>1){
return "vertical";
}else{
return "horizontal";
}
};
RadToolbar.prototype.RenderVertical=function(){
if(this.CheckOrientation()=="vertical"){
return;
}
var _4f=this.ControlElement.rows[0];
var _50=_4f.cells.length;
if(!this.DisplayEnds){
_4f.cells[0].className=this.Skin+"_radtoolbar_vertical";
}
for(var i=1;i<(_50);i++){
var _52=_4f.cells[1];
_52.className=this.Skin+"_radtoolbar_vertical";
_52.parentNode.removeChild(_52);
var _53=this.ControlElement.insertRow(this.ControlElement.rows.length);
_53.appendChild(_52);
}
this.Orientation="vertical";
this.PerformPostRenderActions();
};
RadToolbar.prototype.RenderHorizontal=function(){
if(this.CheckOrientation()=="horizontal"){
return;
}
var _54=this.ControlElement.rows[0];
var _55=this.ControlElement.rows.length;
if(!this.DisplayEnds){
this.ControlElement.rows[0].cells[0].className=this.Skin+"_radtoolbar_horizontal";
}
for(var i=1;i<_55;i++){
var _57=this.ControlElement.rows[1];
var _58=_57.cells[0];
_58.className=this.Skin+"_radtoolbar_horizontal";
_54.appendChild(_58);
_57.parentNode.removeChild(_57);
}
this.Orientation="horizontal";
this.PerformPostRenderActions();
};
RadToolbar.prototype.PerformPostRenderActions=function(){
this.ToggleSeparators();
this.ToggleEnds();
this.FireOnClientOrientationChanged(this,this.Orientation);
this.UpdateState();
};
RadToolbar.prototype.ToggleEnds=function(){
if(!this.DisplayEnds){
return;
}
if(this.Orientation.toLowerCase()=="horizontal"){
var _59=document.getElementById(this.ClientID+"_topleft");
_59.className=this.Skin+"_radtoolbar_left";
var _5a=document.getElementById(this.ClientID+"_botright");
_5a.className=this.Skin+"_radtoolbar_right";
}else{
var _59=document.getElementById(this.ClientID+"_topleft");
_59.className=this.Skin+"_radtoolbar_top";
var _5a=document.getElementById(this.ClientID+"_botright");
_5a.className=this.Skin+"_radtoolbar_bot";
}
};
RadToolbar.prototype.ToggleSeparators=function(){
for(var i in this.SeparatorIDArray){
var _5c=document.getElementById(this.SeparatorIDArray[i]);
if(this.Orientation.toLowerCase()=="horizontal"){
_5c.className=this.Skin+"_radseparator_h";
}else{
_5c.className=this.Skin+"_radseparator_v";
}
}
};
RadToolbar.prototype.ToggleButtonGroups=function(_5d){
for(var i=0;i<this.NumberOfButtons;i++){
if(this.ButtonInstanceArray[i].IsToggle&&this.ButtonInstanceArray[i].ButtonGroup==_5d.ButtonGroup){
if(this.ButtonInstanceArray[i].ClientID!=_5d.ClientID){
if(this.ButtonInstanceArray[i].Toggled){
this.ButtonInstanceArray[i].ToggleButton(false);
}
}else{
this.ButtonInstanceArray[i].ToggleButton(!_5d.Toggled);
}
}
}
};
RadToolbar.prototype.UpdateState=function(){
var _5f=document.getElementById(this.HiddenInputID);
_5f.value=this.Serialize();
};
RadToolbar.prototype.InitializeButtons=function(){
this.ButtonInstanceArray=new Array();
for(var i=0;i<this.NumberOfButtons;i++){
var _61=window[this.ButtonIDArray[i]];
_61.ToolbarInstance=this;
this.ButtonInstanceArray[i]=_61;
}
this.ButtonElementArray=new Array();
for(var i=0;i<this.NumberOfButtons;i++){
this.ButtonElementArray[i]=document.getElementById(this.ButtonIDArray[i]);
}
};
RadToolbar.prototype.ToggleButton=function(_62,_63){
var _64=this.GetButtonByCommand(_63);
if(_64==-1){
alert("Cannot find a button with such command name!");
}
if(_64.Toggled==_62){
return;
}else{
_64.ToggleLogic();
}
};

//BEGIN_ATLAS_NOTIFY
if (typeof(Sys) != "undefined"){if (Sys.Application != null && Sys.Application.notifyScriptLoaded != null){Sys.Application.notifyScriptLoaded();}}
//END_ATLAS_NOTIFY
