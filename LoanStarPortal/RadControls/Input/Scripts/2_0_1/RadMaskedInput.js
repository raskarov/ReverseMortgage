function RadDigitMaskPart(){
}
RadDigitMaskPart.prototype=new RadMaskPart();
RadDigitMaskPart.prototype.GetValue=function(){
return this.value.toString();
};
RadDigitMaskPart.prototype.IsCaseSensitive=function(){
return true;
};
RadDigitMaskPart.prototype.GetVisValue=function(){
if(this.value.toString()==""){
if(this.PromptChar==""){
return " ";
}else{
return this.PromptChar;
}
}
return this.value.toString();
};
RadDigitMaskPart.prototype.CanHandle=function(_1,_2){
if(isNaN(parseInt(_1))){
this.controller.OnChunkError(this,this.GetValue(),_1);
return false;
}
return true;
};
RadDigitMaskPart.prototype.SetValue=function(_3,_4){
if(_3==""||_3==this.PromptChar||_3==" "){
this.value="";
return true;
}
if(this.CanHandle(_3,_4)){
this.value=parseInt(_3);
}
return true;
};;function RadEnumerationMaskPart(_1){
this.SetOptions(_1);
this.lastOffsetPunched=-1;
this.selectedForCompletion=0;
this.FlipDirection=0;
this.RebuildKeyBuff();
}
RadEnumerationMaskPart.prototype=new RadMaskPart();
RadEnumerationMaskPart.prototype.SetOptions=function(_2){
this.length=0;
this.Options=_2;
this.optionsIndex=[];
for(var i=0;i<this.Options.length;i++){
this.length=Math.max(this.length,this.Options[i].length);
this.optionsIndex[this.Options[i]]=i;
}
};
RadEnumerationMaskPart.prototype.CanHandle=function(){
return true;
};
RadEnumerationMaskPart.prototype.SetController=function(_4){
this.controller=_4;
this.InitializeSelection(_4.AllowEmptyEnumerations);
};
RadEnumerationMaskPart.prototype.InitializeSelection=function(_5){
if(_5){
this.value="";
this.selectedIndex=-1;
}else{
this.value=this.Options[0];
this.selectedIndex=0;
}
};
RadEnumerationMaskPart.prototype.RebuildKeyBuff=function(){
this.keyBuff=[];
for(i=0;i<this.length;i++){
this.keyBuff[i]="";
}
this.keyBuffRebuilt=true;
};
RadEnumerationMaskPart.prototype.IsCaseSensitive=function(){
return true;
};
RadEnumerationMaskPart.prototype.ShowHint=function(_6){
var _7=this;
for(var i=0;i<this.Options.length;i++){
var _9=document.createElement("a");
_9.index=i;
_9.onclick=function(){
_7.SetOption(this.index);
_7.controller.Visualise();
return false;
};
_9.innerHTML=this.Options[i];
_9.href="javascript:void(0)";
_6.appendChild(_9);
}
return true;
};
RadEnumerationMaskPart.prototype.ResetCompletion=function(){
this.selectedForCompetion=0;
};
RadEnumerationMaskPart.prototype.SelectNextCompletion=function(){
this.selectedForCompletion++;
};
RadEnumerationMaskPart.prototype.Store=function(_a,_b){
if(this.lastOffsetPunched==_b){
if(this.keyBuff[_b]==_a){
this.SelectNextCompletion();
}else{
this.RebuildKeyBuff();
}
}else{
this.ResetCompletion();
}
this.lastOffsetPunched=_b;
this.keyBuff[_b]=_a;
};
RadEnumerationMaskPart.prototype.SetNoCompletionValue=function(){
if(this.controller.AllowEmptyEnumerations){
this.SetOption(-1);
}
};
RadEnumerationMaskPart.prototype.SetValue=function(_c,_d){
_d-=this.offset;
this.Store(_c,_d);
var _e=new CompletionList(this.Options,this.PromptChar);
var _f=_e.GetCompletions(this.keyBuff,_d);
if(_f.length>0){
var _10=this.optionsIndex[_f[this.selectedForCompletion%_f.length]];
this.SetOption(_10);
}else{
this.SetNoCompletionValue();
return false;
}
return true;
};
RadEnumerationMaskPart.prototype.GetVisValue=function(){
var v=this.value;
while(v.length<this.length){
v+=this.PromptChar;
}
return v;
};
RadEnumerationMaskPart.prototype.GetLength=function(){
return this.length;
};
RadEnumerationMaskPart.prototype.GetSelectedIndex=function(){
return this.selectedIndex;
};
RadEnumerationMaskPart.prototype.SetOption=function(_12,up){
var _14=this.value;
if(this.controller.AllowEmptyEnumerations){
if(_12<-1){
_12=this.Options.length+_12+1;
this.FlipDirection=-1;
}else{
if(_12>=this.Options.length){
_12=_12-this.Options.length-1;
this.FlipDirection=1;
}
}
}else{
if(_12<0){
_12=this.Options.length+_12;
this.FlipDirection=-1;
}else{
if(_12>=this.Options.length){
_12=_12-this.Options.length;
this.FlipDirection=1;
}
}
}
this.selectedIndex=_12;
this.value=_12==-1?"":this.Options[_12];
if(typeof (up)!="undefined"){
if(up){
this.controller.OnMoveUp(this,_14,this.value);
}else{
this.controller.OnMoveDown(this,_14,this.value);
}
}
this.controller.OnEnumChanged(this,_14,this.value);
this.FlipDirection=0;
};
RadEnumerationMaskPart.prototype.HandleKey=function(e){
this.controller.CalculateSelection();
var _16=new MaskedEventWrap(e,this.controller.field);
if(_16.IsDownArrow()){
this.SetOption(this.selectedIndex+1,false);
this.controller.Visualise();
this.controller.FixSelection(_16);
return true;
}else{
if(_16.IsUpArrow()){
this.SetOption(this.selectedIndex-1,true);
this.controller.Visualise();
this.controller.FixSelection(_16);
return true;
}
}
};
RadEnumerationMaskPart.prototype.HandleWheel=function(e){
this.controller.CalculateSelection();
var _18=new MaskedEventWrap(e,this.controller.field);
this.SetOption(this.selectedIndex-e.wheelDelta/120);
this.controller.Visualise();
this.controller.FixSelection(_18);
return false;
};
function CompletionList(_19,_1a){
this.options=_19;
this.blankChar=_1a;
}
CompletionList.prototype.GetCompletions=function(_1b,_1c){
var _1d=this.options;
for(var _1e=0;_1e<=_1c;_1e++){
var _1f=_1b[_1e].toLowerCase();
_1d=this.FilterCompletions(_1d,_1e,_1f);
}
return _1d;
};
CompletionList.prototype.FilterCompletions=function(_20,_21,key){
var _23=[];
for(var _24=0;_24<_20.length;_24++){
var _25=_20[_24];
var _26=_25.charAt(_21).toLowerCase();
if(this.CharacterMatchesCompletion(key,_26)){
_23[_23.length]=_25;
}
}
return _23;
};
CompletionList.prototype.CharacterMatchesCompletion=function(_27,_28){
return _27==this.blankChar||_27==" "||_27==_28;
};;function RadFreeMaskPart(){
}
RadFreeMaskPart.prototype=new RadMaskPart();
RadFreeMaskPart.prototype.IsCaseSensitive=function(){
return true;
};
RadFreeMaskPart.prototype.GetVisValue=function(){
if(this.value.toString()==""){
return this.PromptChar;
}
return this.value;
};
RadFreeMaskPart.prototype.SetValue=function(_1,_2){
this.value=_1;
return true;
};;function RadInputHint(_1,_2){
this.textBox=_1;
this.skin=_2;
}
RadInputHint.prototype.Show=function(_3,_4){
if(_3){
var _5=this.GetRect(this.textBox.field);
this.Container=document.createElement("div");
if(_3.ShowHint(this.Container)){
this.Container.className="radHint_"+this.skin;
document.body.appendChild(this.Container);
this.Container.style.position="absolute";
if(_4){
this.Container.style.left=_4.left+this.BodyScrollWidth()+"px";
this.Container.style.top=_5.Y+_5.Height+"px";
}else{
this.Container.style.left=_5.X+"px";
this.Container.style.top=_5.Y+_5.Height+"px";
}
this.CreateOverlay();
this.textBox.OnShowHint(this);
}else{
this.Container=null;
}
}
};
RadInputHint.prototype.HideOverlay=function(){
if(this.shim){
this.shim.style.visibility="hidden";
}
};
RadInputHint.prototype.CreateOverlay=function(){
if(window.opera){
return;
}
if(!this.shim){
this.shim=document.createElement("IFRAME");
this.shim.src="javascript:false;";
this.shim.frameBorder=0;
this.shim.id=this.Container.parentNode.id+"Overlay";
this.shim.style.position="absolute";
this.shim.style.visibility="hidden";
this.shim.style.border="1px solid red";
this.shim.style.filter="progid:DXImageTransform.Microsoft.Alpha(style=0,opacity=0)";
this.shim.allowTransparency=false;
this.Container.parentNode.insertBefore(this.shim,this.Container);
}
var _6=this.GetRect(this.Container);
this.shim.style.cssText=this.Container.style.cssText;
this.shim.style.left=_6.X+"px";
this.shim.style.top=_6.Y+"px";
this.shim.style.width=_6.Width+"px";
this.shim.style.height=_6.Height+"px";
this.shim.style.visibility="visible";
};
RadInputHint.prototype.FindScrollPosX=function(_7){
var x=0;
var _9=_7;
while(_9.parentNode&&_9.parentNode.tagName!="BODY"){
if(typeof (_9.parentNode.scrollLeft)=="number"){
x+=_9.parentNode.scrollLeft;
}
_9=_9.parentNode;
}
return x;
};
RadInputHint.prototype.FindScrollPosY=function(_a){
var y=0;
var _c=_a;
while(_c.parentNode&&_c.parentNode.tagName!="BODY"){
if(typeof (_c.parentNode.scrollTop)=="number"){
y+=_c.parentNode.scrollTop;
}
_c=_c.parentNode;
}
return y;
};
RadInputHint.prototype.BodyScrollWidth=function(){
var _d=0;
if(typeof (document.body.scrollLeft)=="number"){
_d+=document.body.scrollLeft;
}
if(typeof (document.documentElement.scrollLeft)=="number"){
_d+=document.documentElement.scrollLeft;
}
return _d;
};
RadInputHint.prototype.BodyScrollHeight=function(){
var _e=0;
if(typeof (document.body.scrollTop)=="number"){
_e+=document.body.scrollTop;
}
if(typeof (document.documentElement.scrollTop)=="number"){
_e+=document.documentElement.scrollTop;
}
return _e;
};
RadInputHint.prototype.FindScrollPosXOpera=function(_f){
var x=0;
var _11=_f;
while(_11.offsetParent&&_11.offsetParent.tagName!="BODY"){
if(typeof (_11.offsetParent.scrollLeft)=="number"){
x+=_11.offsetParent.scrollLeft;
}
_11=_11.offsetParent;
}
return x;
};
RadInputHint.prototype.FindScrollPosYOpera=function(_12){
var y=0;
var _14=_12;
while(_14.offsetParent&&_14.offsetParent.tagName!="BODY"){
if(typeof (_14.offsetParent.scrollTop)=="number"){
y+=_14.offsetParent.scrollTop;
}
_14=_14.offsetParent;
}
return y;
};
RadInputHint.prototype.Hide=function(){
if(this.Container){
this.HideOverlay();
this.Container.parentNode.removeChild(this.Container);
this.Container=null;
}
};
RadInputHint.prototype.GetRect=function(_15){
var _16=_15.offsetWidth;
var _17=_15.offsetHeight;
var x=0;
var y=0;
var _1a=_15;
while(_1a.offsetParent){
x+=_1a.offsetLeft;
y+=_1a.offsetTop;
_1a=_1a.offsetParent;
}
var _1b=0;
var _1c=0;
if(window.opera){
x-=this.FindScrollPosXOpera(_15);
y-=this.FindScrollPosYOpera(_15);
}else{
x-=this.FindScrollPosX(_15);
y-=this.FindScrollPosY(_15);
}
return {X:x,Y:y,Width:_16,Height:_17};
};;function RadLiteralMaskPart(ch){
this.ch=ch;
}
RadLiteralMaskPart.prototype=new RadMaskPart();
RadLiteralMaskPart.prototype.GetVisValue=function(){
return this.ch;
};
RadLiteralMaskPart.prototype.GetLength=function(){
if(this.controller.mozilla){
return this.ch.length-(this.ch.split("\r\n").length-1);
}
return this.ch.length;
};
RadLiteralMaskPart.prototype.GetValue=function(){
return "";
};
RadLiteralMaskPart.prototype.IsCaseSensitive=function(){
if(this.NextChunk!=null){
return this.NextChunk.IsCaseSensitive();
}
};
RadLiteralMaskPart.prototype.SetValue=function(_2,_3){
_3-=this.offset;
return _2==this.ch.charAt(_3)||!_2;
};
RadLiteralMaskPart.prototype.CanHandle=function(_4,_5){
_5-=this.offset;
if(_4==this.ch.charAt(_5)){
return true;
}
if(!_4){
return true;
}
if(this.NextChunk!=null){
return this.NextChunk.CanHandle(_4,_5+this.GetLength());
}
};;function RadLowerMaskPart(){
}
RadLowerMaskPart.prototype=new RadMaskPart();
RadLowerMaskPart.prototype.CanHandle=function(_1,_2){
if(!RadMaskPart.IsAlpha(_1)){
this.controller.OnChunkError(this,this.GetValue(),_1);
return false;
}
return true;
};
RadLowerMaskPart.prototype.GetVisValue=function(){
if(this.value.toString()==""){
return this.PromptChar;
}
return this.value.toString();
};
RadLowerMaskPart.prototype.SetValue=function(_3,_4){
if(_3==""){
this.value="";
return true;
}
if(RadMaskPart.IsAlpha(_3)){
this.value=_3.toLowerCase();
}else{
this.controller.OnChunkError(this,this.GetValue(),_3);
}
return true;
};;if(typeof (window.RadInputNamespace)=="undefined"){
window.RadInputNamespace=new Object();
}
function RadMaskedTextBox(id,_2,_3){
RadTextBox.Extend(this);
this.CallBase("DisposeOldInstance",arguments);
this.Constructor(id);
this.Initialize(_2,_3);
}
RadMaskedTextBox.prototype={Constructor:function(id){
this.ID=id;
this.WrapperElementID=id+"_wrapper";
this.TextBoxElement=document.getElementById(id+"_TextBox");
this.field=this.TextBoxElement;
this.hiddenField=document.getElementById(id+"_Value");
this.validationField=document.getElementById(id);
this.HiddenElement=this.validationField;
this.Enabled=!this.TextBoxElement.disabled;
RadControlsNamespace.EventMixin.Initialize(this);
RadControlsNamespace.DomEventMixin.Initialize(this);
},Initialize:function(_5,_6){
this.LoadCongfiguration(_5);
this.LoadClientEvents(_5);
this.parts=[];
this.partIndex=[];
this.displayPartIndex=[];
this.value="";
this.blurred=true;
this.TextBoxElement.oldValue=this.TextBoxElement.value;
this.lastState=null;
this.length=0;
this.displayLength=0;
this.internalValueUpdate=false;
this.projectedValue="";
this.Hint=null;
this.isTextarea=this.TextBoxElement.tagName.toLowerCase()=="textarea";
this.safari=navigator.userAgent.indexOf("Safari")>-1;
this.mozilla=navigator.userAgent.indexOf("Gecko")>-1;
this.form=this.TextBoxElement.form;
this.Hint=new RadInputHint(this,_5.Skin);
this.AttachEventHandlers();
if(_5.Skin){
this.TextBoxElement.overClass="radHoverCss_"+_5.Skin;
this.TextBoxElement.focusClass="radFocusedCss_"+_5.Skin;
this.TextBoxElement.defaultClass=this.TextBoxElement.className?this.TextBoxElement.className:"radEnabledCss_"+_5.Skin;
this.TextBoxElement.errorClass="radInvalidCss_"+_5.Skin;
this.TextBoxElement.className=this.TextBoxElement.defaultClass;
}else{
var _7=this.TextBoxElement.className;
this.TextBoxElement.overClass=_7;
this.TextBoxElement.focusClass=_7;
this.TextBoxElement.defaultClass=_7;
this.TextBoxElement.errorClass=_7;
}
this.TextBoxElement.focused=false;
this.FixAbsolutePositioning();
this.SetValue(this.TextBoxElement.value);
if(this.FocusOnStartup){
this.Focus();
}
this.RecordInitialState();
},LoadClientEvents:function(_8){
var _9=new Array("OnClientEnumerationChanged","OnClientError","OnClientMoveDown","OnClientMoveUp","OnClientShowHint","OnClientValueChanged");
var _a=null;
for(var _b in _9){
var _a=eval(_8[_b]);
if(typeof (_a)=="function"){
this.AttachEvent(_b,_a);
}
}
},AttachToTextBoxEvent:function(_c,_d){
this.AttachDomEvent(this.TextBoxElement,_c,_d);
},AttachEventHandlers:function(){
this.AttachDomEvent(this.TextBoxElement.form,"reset","OnReset");
this.AttachToTextBoxEvent("keydown","OnKeyDown");
this.AttachToTextBoxEvent("keypress","OnKeyPress");
this.AttachToTextBoxEvent("keyup","onKeyUp");
this.AttachToTextBoxEvent("focus","onFocus");
this.AttachToTextBoxEvent("mousedown","onMouseDown");
this.AttachToTextBoxEvent("mouseover","onMouseOver");
this.AttachToTextBoxEvent("mouseout","onMouseOut");
this.AttachToTextBoxEvent("mouseup","onMouseUp");
this.AttachToTextBoxEvent("blur","onBlur");
if(this.ShouldUseAttachEvent(this.TextBoxElement)){
this.AttachToTextBoxEvent("paste","onPaste");
this.AttachToTextBoxEvent("propertychange","onPropertyChange");
this.AttachToTextBoxEvent("mousewheel","onMouseWheel");
}else{
this.AttachToTextBoxEvent("input","onInput");
}
if(window.opera){
var _e=this;
var _f=function(){
return _e.ValueHandler({});
};
setInterval(_f,10);
}
},SetValue:function(_10){
this.internalValueUpdate=true;
this.UpdatePartsInRange(_10,0,this.length);
this.internalValueUpdate=false;
this.Visualise();
},Enable:function(){
this.TextBoxElement.disabled="";
this.Enabled=true;
},Disable:function(){
this.TextBoxElement.disabled="disabled";
this.Enabled=false;
},Focus:function(){
this.TextBoxElement.focus();
this.TextBoxElement.selectionStart=this.TextBoxElement.selectionEnd=0;
},GetValue:function(){
var _11=[];
for(var i=0;i<this.parts.length;i++){
_11[i]=this.parts[i].GetValue();
}
return _11.join("");
},onInput:function(e){
this.ValueHandler(e);
},onMouseWheel:function(e){
return this.OnMouseWheel(event);
},onPropertyChange:function(e){
this.OnPropertyChange();
},onPaste:function(e){
if(this.ReadOnly){
return false;
}
if(this.selectionStart==this.value.length){
return false;
}
setTimeout(function(){
this.FakeOnPropertyChange();
},1);
},onBlur:function(e){
this.TextBoxElement.focused=false;
this.TextBoxElement.className=this.TextBoxElement.defaultClass;
this.blurValue();
var _18=this;
window.setTimeout(function(){
if(_18.Hint){
_18.Hint.Hide();
}
},200);
if(this.AutoPostBack&&this.ValueHasChanged()){
eval(this.AutoPostBackCode);
}
this.TextBoxElement.oldValue=this.TextBoxElement.value;
},onMouseUp:function(e){
this.FakeOnPropertyChange();
this.ValueHandler(e);
this.ActivityHandler(e);
this.DisplayHint();
},onMouseOut:function(e){
if(!this.TextBoxElement.focused){
this.TextBoxElement.className=this.TextBoxElement.defaultClass;
}
},onMouseOver:function(e){
this.FakeOnPropertyChange();
if(!this.TextBoxElement.focused){
this.TextBoxElement.className=this.TextBoxElement.overClass;
}
},onMouseDown:function(e){
this.FakeOnPropertyChange();
this.ActivityHandler(e);
},onFocus:function(e){
this.TextBoxElement.focused=true;
this.TextBoxElement.className=this.TextBoxElement.focusClass;
this.focusValue();
this.FakeOnPropertyChange();
this.ActivityHandler(e);
},onKeyUp:function(e){
this.FakeOnPropertyChange();
this.DisplayHint();
},OnActivity:function(e){
this.CalculateSelection();
this.lastState=new MaskedEventWrap(e,this.TextBoxElement);
},OnPropertyChange:function(){
if(this.internalValueUpdate){
return;
}
if(event.propertyName=="value"){
var e=event;
var _21=this;
var _22=function(){
_21.ValueHandler(e);
};
this.CalculateSelection();
if(this.TextBoxElement.selectionStart>0||this.TextBoxElement.selectionEnd>0){
_22();
}else{
setTimeout(_22,1);
}
}
},OnMouseWheel:function(e){
if(this.ReadOnly){
return false;
}
this.CalculateSelection();
var _24=this.partIndex[this.TextBoxElement.selectionStart];
if(_24==null){
return true;
}
return _24.HandleWheel(e);
},OnKeyDown:function(e){
this.FakeOnPropertyChange();
if(this.InSelection(e)){
return true;
}
var _26=this.partIndex[this.TextBoxElement.selectionStart];
var _27=e.which?e.which:e.keyCode;
if(this.ReadOnly&&(_27==46||_27==8||_27==38||_27==40)){
RadControlsNamespace.DomEvent.PreventDefault(e);
return false;
}
if(_27==13){
return true;
}
if(_26==null&&_27!=8){
return true;
}
if(_26!=null){
if(_26.HandleKey(e)){
RadControlsNamespace.DomEvent.PreventDefault(e);
return false;
}
}
var _28=this.TextBoxElement.selectionEnd;
var _29=false;
if((_27==46)&&_28<this.TextBoxElement.value.length&&!window.opera){
_26.SetValue("",this.TextBoxElement.selectionStart);
_28++;
_29=true;
}else{
if(_27==8&&_28&&!window.opera){
this.partIndex[this.TextBoxElement.selectionStart-1].SetValue("",this.TextBoxElement.selectionStart-1);
_28--;
_29=true;
}
}
if(_29){
return this.UpdateAfterKeyHandled(e,_28);
}
this.OnActivity(e);
return true;
},OnKeyPress:function(e){
if(this.ReadOnly){
RadControlsNamespace.DomEvent.PreventDefault(e);
RadControlsNamespace.DomEvent.StopPropagation(e);
return false;
}
if(this.InSelection(e)){
return true;
}
var _2b=this.partIndex[this.TextBoxElement.selectionStart];
if(_2b==null){
return true;
}
if(this.mozilla||window.opera){
if(e.which==8){
RadControlsNamespace.DomEvent.PreventDefault(e);
RadControlsNamespace.DomEvent.StopPropagation(e);
return false;
}
if(!e.which){
this.OnActivity(e);
RadControlsNamespace.DomEvent.StopPropagation(e);
return true;
}
}
var _2c=this.TextBoxElement.selectionEnd;
var _2d=e.which?e.which:e.keyCode;
if(_2d==13){
return true;
}
var ch=String.fromCharCode(_2d);
if(_2b.CanHandle(ch)){
while(_2c<this.TextBoxElement.value.length){
if(this.partIndex[_2c].SetValue(ch,_2c)){
_2c++;
break;
}
_2c++;
}
}
var _2f=this.UpdateAfterKeyHandled(e,_2c);
if(!_2f){
RadControlsNamespace.DomEvent.PreventDefault(e);
}
RadControlsNamespace.DomEvent.StopPropagation(e);
return _2f;
},OnEnumChanged:function(_30,_31,_32){
this.RaiseEvent("OnClientEnumerationChanged",{"CurrentPart":_30,"OldValue":_31,"NewValue":_32});
},OnMoveUp:function(_33,_34,_35){
this.RaiseEvent("OnClientMoveUp",{"CurrentPart":_33,"OldValue":_34,"NewValue":_35});
},OnMoveDown:function(_36,_37,_38){
this.RaiseEvent("OnClientMoveDown",{"CurrentPart":_36,"OldValue":_37,"NewValue":_38});
},OnValueChange:function(_39,_3a,_3b){
this.RaiseEvent("OnClientValueChanged",{"CurrentPart":_39,"OldValue":_3a,"NewValue":_3b});
},OnShowHint:function(_3c){
this.RaiseEvent("OnClientShowHint",{"CurrentPart":_3c,"OldValue":this.TextBoxElement.value,"NewValue":this.TextBoxElement.value});
},OnChunkError:function(_3d,_3e,_3f){
this.RaiseEvent("OnClientError",{"CurrentPart":_3d,"OldValue":_3e,"NewValue":_3f});
var _40=this.TextBoxElement.focusClass;
this.TextBoxElement.className=this.TextBoxElement.errorClass;
var _41=this;
var _42=function(){
if(_41.TextBoxElement.className==_41.TextBoxElement.errorClass){
_41.TextBoxElement.className=_40;
}
};
setTimeout(_42,100);
},ShouldUseAttachEvent:function(_43){
return (_43.attachEvent&&!window.opera&&!window.netscape);
},FixAbsolutePositioning:function(){
var f=this.TextBoxElement;
if(f.previousSibling&&f.previousSibling.tagName.toLowerCase()=="label"&&f.style.position=="absolute"){
f.style.position="static";
var _45=f.parentNode;
_45.style.position="absolute";
_45.style.top=f.style.top;
_45.style.left=f.style.left;
}
},RecordInitialState:function(){
this.initialFieldValue=this.TextBoxElement.value;
},PartAt:function(_46){
return this.partIndex[_46];
},SetDisabledClass:function(_47){
this.TextBoxElement.disabledClass=_47;
if(this.TextBoxElement.disabled){
this.TextBoxElement.className=this.TextBoxElement.disabledClass;
}
},SetFocusClass:function(_48){
this.TextBoxElement.focusClass=_48;
},SetErrorClass:function(_49){
this.TextBoxElement.errorClass=_49;
},SetOverClass:function(_4a){
this.TextBoxElement.overClass=_4a;
},CreatePartCollection:function(_4b,_4c){
var _4d;
var _4e=[];
var _4f=0;
for(var j=0;j<_4b.length;j++){
_4d=_4b[j];
_4d.PromptChar=_4c;
_4d.SetController(this);
_4d.index=this.parts.length;
_4e[_4e.length]=_4d;
if(_4e.length>1){
_4e[_4e.length-2].NextChunk=_4d;
}
_4d.NextChunk=null;
var _51=_4d.GetLength();
_4d.offset=_4f;
_4f+=_51;
}
return _4e;
},SetMask:function(){
this.parts=this.CreatePartCollection(arguments,this.PromptChar);
for(var i=0;i<this.parts.length;i++){
var _53=this.parts[i].GetLength();
for(var j=this.length;j<this.length+_53;j++){
this.partIndex[j]=this.parts[i];
}
this.length+=_53;
}
},SetDisplayMask:function(){
this.displayParts=this.CreatePartCollection(arguments,this.DisplayPromptChar);
for(var i=0;i<this.displayParts.length;i++){
var _56=this.displayParts[i];
var _57=_56.GetLength();
if(_56.ch){
continue;
}
for(var j=this.displayLength;j<this.displayLength+_57;j++){
this.displayPartIndex[j]=this.displayParts[i];
}
this.displayLength+=_57;
}
},SafariSelectionFix:function(e){
var _5a=this.StrCompare(this.lastState.fieldValue,e.fieldValue);
e.selectionStart=_5a[0];
e.selectionEnd=_5a[0];
this.lastState.selectionStart=_5a[1];
this.lastState.selectionEnd=_5a[2];
},HandleValueChange:function(e){
if(this.ReadOnly){
this.Visualise();
return false;
}
if(this.lastState==null){
return;
}
var i,j;
if(this.safari){
this.SafariSelectionFix(e);
}
if(this.lastState.fieldValue.length>e.fieldValue.length){
if(e.selectionStart==this.TextBoxElement.value.length){
this.partIndex[this.partIndex.length-1].SetValue("",this.partIndex.length-1);
}
if(this.lastState.selectionEnd>e.selectionStart){
i=this.lastState.selectionEnd;
while(i-->e.selectionStart){
this.partIndex[i].SetValue("",i);
}
}else{
i=this.lastState.selectionEnd+1;
while(i-->e.selectionStart){
this.partIndex[i].SetValue("",i);
e.selectionEnd++;
}
}
}
var _5e=this.lastState.selectionStart;
var _5f=Math.min(e.selectionStart,this.length);
var _60=e.fieldValue.substr(_5e,_5f-_5e);
var _61=this.UpdatePartsInRange(_60,_5e,_5f);
e.selectionEnd+=_61;
this.FixSelection(e);
},SetPartValues:function(_62,_63,_64,_65,to){
var _67;
var i=0;
var j=_65;
var _6a=0;
_64=_64.toString();
while(i<to-_65&&j<_63){
_67=_64.charAt(i);
if(_67==this.PromptChar){
_67="";
}
if(_62[j].SetValue(_67,j)){
i++;
}else{
_6a++;
}
j++;
}
return _6a;
},UpdateDisplayPartsInRange:function(_6b,_6c,to){
this.SetPartValues(this.displayPartIndex,this.displayLength,_6b,_6c,to);
},UpdatePartsInRange:function(_6e,_6f,to){
var _71=this.SetPartValues(this.partIndex,this.length,_6e,_6f,to);
this.Visualise();
return _71;
},SetCursorPosition:function(_72){
if(!this.TextBoxElement.focused){
return;
}
this.CalculateSelection();
if(document.all&&!window.opera){
this.TextBoxElement.select();
sel=document.selection.createRange();
var _73=this.TextBoxElement.value.substr(0,_72).split("\r\n").length-1;
sel.move("character",_72-_73);
sel.select();
}else{
this.TextBoxElement.selectionStart=_72;
this.TextBoxElement.selectionEnd=_72;
}
},FixSelection:function(_74){
this.SetCursorPosition(_74.selectionEnd);
},GetValueWithLiterals:function(){
var _75=[];
for(var i=0;i<this.parts.length;i++){
_75[i]=this.parts[i].ch||this.parts[i].GetValue();
}
return _75.join("");
},GetVisibleValues:function(_77){
var _78=[];
for(var i=0;i<_77.length;i++){
_78[i]=_77[i].GetVisValue();
}
return _78.join("");
},GetValueForField:function(){
return this.GetVisibleValues(this.parts);
},GetPrompt:function(){
var _7a=new RegExp(".","g");
var _7b=[];
for(var i=0;i<this.parts.length;i++){
_7b[i]=this.parts[i].ch||this.parts[i].GetVisValue().replace(_7a,this.PromptChar);
}
return _7b.join("");
},Visualise:function(){
var _7d=this.GetValueForField();
var _7e=this.GetValue();
this.internalValueUpdate=true;
var _7f=this.projectedValue;
this.Render(_7d);
this.value=_7e;
this.UpdateHiddenFieldValue();
this.UpdateValidationFieldValue();
this.internalValueUpdate=false;
this.projectedValue=this.TextBoxElement.value;
if(_7f!=this.TextBoxElement.value){
this.OnValueChange(null,_7f,this.TextBoxElement.value);
}
},Render:function(_80){
if(this.blurred){
if(this.HideOnBlur&&this.isEmpty()){
this.TextBoxElement.value=this.GetBlurredValue();
}else{
if(this.displayParts&&this.displayParts.length){
this.TextBoxElement.value=this.GetDisplayValue();
}else{
this.TextBoxElement.value=_80;
}
}
}else{
this.TextBoxElement.value=_80;
}
},UpdateHiddenFieldValue:function(){
this.hiddenField.value=this.GetValueForField();
},UpdateValidationFieldValue:function(){
if(this.isEmpty()){
this.validationField.value="";
}else{
this.validationField.value=this.GetValueWithLiterals();
}
},OnReset:function(){
this.SetValue(this.initialFieldValue);
this.Visualise();
},ValueHasChanged:function(){
return this.TextBoxElement.value!=this.TextBoxElement.oldValue;
},FakeOnPropertyChange:function(){
if(document.createEventObject){
if(event){
var ev=document.createEventObject(event);
}else{
var ev=document.createEventObject();
}
ev.propertyName="value";
this.TextBoxElement.fireEvent("onpropertychange",ev);
}
},GetBlurredValue:function(){
return this.EmptyMessage;
},GetDisplayValue:function(){
var _82=this.value;
while(_82.length<this.displayLength){
if(this.DisplayFormatPosition){
_82=this.PromptChar+_82;
}else{
_82+=this.PromptChar;
}
}
this.UpdateDisplayPartsInRange(_82,0,this.displayLength);
return this.GetVisibleValues(this.displayParts);
},isEmpty:function(){
return this.value=="";
},blurValue:function(){
this.blurred=true;
this.Visualise();
},focusValue:function(){
this.blurred=false;
if((this.HideOnBlur&&this.isEmpty())||this.displayParts){
this.Visualise();
this.TextBoxElement.select();
}
if(this.ResetCaretOnFocus){
this.ResetCursor();
}
},ResetCursor:function(){
this.SetCursorPosition(0);
},UpdateAfterKeyHandled:function(e,_84){
this.Visualise();
var _85=new MaskedEventWrap(this.TextBoxElement,e);
_85.selectionEnd=_84;
this.FixSelection(_85);
return false;
},InSelection:function(e){
this.CalculateSelection();
if(this.TextBoxElement.selectionStart!=this.TextBoxElement.selectionEnd){
this.OnActivity(e);
return true;
}
if(e.ctrlKey||e.altKey||this.safari){
this.OnActivity(e);
return true;
}
return false;
},ValueHandler:function(e){
if(this.internalValueUpdate){
return true;
}
if(!e){
e=window.event;
}
this.CalculateSelection();
var _88=new MaskedEventWrap(e,this.TextBoxElement);
if(_88.fieldValue!=this.projectedValue){
this.HandleValueChange(_88);
}
return true;
},ActivityHandler:function(e){
if(this.internalValueUpdate){
return true;
}
if(!e){
e=window.event;
}
this.OnActivity(e);
return true;
},DisplayHint:function(){
if(!this.ShowHint){
return;
}
this.CalculateSelection();
var _8a=this.partIndex[this.TextBoxElement.selectionStart];
this.Hint.Hide();
var _8b=null;
if(document.selection){
var _8c=document.selection.createRange();
if(_8c.getBoundingClientRect){
_8b=_8c.getBoundingClientRect();
}
}
this.Hint.Show(_8a,_8b);
},CalculateSelection:function(){
if(document.selection&&!window.opera){
var s1;
try{
s1=document.selection.createRange();
}
catch(error){
return;
}
if(s1.parentElement()!=this.TextBoxElement){
return;
}
var s=s1.duplicate();
if(this.isTextarea){
s.moveToElementText(this.TextBoxElement);
}else{
s.move("character",-this.TextBoxElement.value.length);
}
s.setEndPoint("EndToStart",s1);
this.TextBoxElement.selectionStart=s.text.length;
this.TextBoxElement.selectionEnd=this.TextBoxElement.selectionStart+s1.text.length;
if(this.isTextarea){
}
}
},StrCompare:function(_8f,_90){
var i;
var _92,_93,_94;
i=0;
while(_8f.charAt(i)==_90.charAt(i)&&i<_8f.length){
i++;
}
_93=i;
_8f=_8f.substr(_93).split("").reverse().join("");
_90=_90.substr(_93).split("").reverse().join("");
i=0;
while(_8f.charAt(i)==_90.charAt(i)&&i<_8f.length){
i++;
}
_92=_93+_90.length-i;
_94=_8f.length-i+_93;
return [_92,_93,_94];
}};
function RadInputEventArgs(){
}
function MaskedEventWrap(e,_96){
this.event=e;
this.selectionStart=_96.selectionStart;
this.selectionEnd=_96.selectionEnd;
this.fieldValue=_96.value;
}
MaskedEventWrap.prototype.IsUpArrow=function(){
return this.event.keyCode==38;
};
MaskedEventWrap.prototype.IsDownArrow=function(){
return this.event.keyCode==40;
};
function rdmskd(){
return new RadDigitMaskPart();
}
function rdmskl(_97){
return new RadLiteralMaskPart(_97);
}
function rdmske(_98){
return new RadEnumerationMaskPart(_98);
}
function rdmskr(_99,_9a,_9b,_9c){
return new RadNumericRangeMaskPart(_99,_9a,_9b,_9c);
}
function rdmsku(){
return new RadUpperMaskPart();
}
function rdmsklw(){
return new RadLowerMaskPart();
}
function rdmskp(){
return new RadPasswordMaskPart();
}
function rdmskf(){
return new RadFreeMaskPart();
};function RadMaskPart(){
this.value="";
this.index=-1;
this.type=-1;
this.PromptChar="_";
}
RadMaskPart.prototype.HandleKey=function(ev){
return false;
};
RadMaskPart.prototype.HandleWheel=function(_2){
return true;
};
RadMaskPart.prototype.SetController=function(_3){
this.controller=_3;
};
RadMaskPart.prototype.GetValue=function(){
return this.value.toString();
};
RadMaskPart.prototype.GetVisValue=function(){
return "";
};
RadMaskPart.prototype.SetValue=function(_4,_5){
return true;
};
RadMaskPart.prototype.CanHandle=function(_6,_7){
return true;
};
RadMaskPart.prototype.IsCaseSensitive=function(){
return false;
};
RadMaskPart.prototype.ShowHint=function(_8){
return false;
};
RadMaskPart.prototype.GetLength=function(){
return 1;
};
RadMaskPart.IsAlpha=function(_9){
return _9.match(/[^\u005D\u005B\t\n\r\f\s\v\\!-@|^_`{-┬┐]{1}/)!=null;
};;function RadNumericRangeMaskPart(_1,_2,_3,_4){
this.upperLimit=_2;
this.lowerLimit=_1;
this.length=Math.max(this.lowerLimit.toString().length,this.upperLimit.toString().length);
this.leftAlign=_3;
this.zeroFill=_4;
this.minusIncluded=this.lowerLimit<0||this.upperLimit<0;
this.value=_1;
this.FlipDirection=0;
}
RadNumericRangeMaskPart.prototype=new RadMaskPart();
RadNumericRangeMaskPart.prototype.SetController=function(_5){
this.controller=_5;
this.GetVisValue();
};
RadNumericRangeMaskPart.prototype.IsCaseSensitive=function(){
return true;
};
RadNumericRangeMaskPart.prototype.CanHandle=function(_6,_7){
if((_6=="-"||_6=="+")&&this.lowerLimit<0){
return true;
}
if(isNaN(parseInt(_6))){
this.controller.OnChunkError(this,this.GetValue(),_6);
return false;
}
return true;
};
RadNumericRangeMaskPart.prototype.InsertAt=function(_8,_9){
return this.visValue.substr(0,_9)+_8.toString()+this.visValue.substr(_9+1,this.visValue.length);
};
RadNumericRangeMaskPart.prototype.ReplacePromptChar=function(_a){
var _b=this.leftAlign?"":"0";
while(_a.indexOf(this.PromptChar)>-1){
_a=_a.replace(this.PromptChar,_b);
}
return _a;
};
RadNumericRangeMaskPart.prototype.SetValue=function(_c,_d){
if(_c==""){
_c=0;
}
if(isNaN(parseInt(_c))&&_c!="+"&&_c!="-"){
return true;
}
_d-=this.offset;
var _e=this.InsertAt(_c,_d);
_e=this.ReplacePromptChar(_e);
if(_e.indexOf("-")!=-1&&_e.indexOf("-")>0){
_e=_e.replace("-","0");
}
if(isNaN(parseInt(_e))){
_e=0;
}
if(this.controller.RoundNumericRanges){
_e=Math.min(this.upperLimit,_e);
_e=Math.max(this.lowerLimit,_e);
this.setInternalValue(_e);
}else{
if(_e<=this.upperLimit&&_e>=this.lowerLimit){
this.setInternalValue(_e);
this.GetVisValue();
}else{
return false;
}
}
this.GetVisValue();
return true;
};
RadNumericRangeMaskPart.prototype.setInternalValue=function(_f){
var _10=this.value;
this.value=_f;
this.controller.OnEnumChanged(this,_10,_f);
if(_10>_f){
this.controller.OnMoveDown(this,_10,_f);
}else{
this.controller.OnMoveUp(this,_10,_f);
}
this.FlipDirection=0;
};
RadNumericRangeMaskPart.prototype.GetVisValue=function(){
var out="";
var _12=Math.abs(this.value).toString();
if(this.leftAlign){
if(this.value<0){
out+=this.PromptChar;
}
out+=_12;
while(out.length<this.length){
out+=this.controller.PromptChar;
}
}else{
var _13=this.zeroFill?"0":this.controller.PromptChar;
if(this.value<0){
_12="-"+_12;
}
while(out.length<this.length-_12.length){
out+=_13;
}
out+=_12;
}
this.visValue=out;
return out;
};
RadNumericRangeMaskPart.prototype.GetLength=function(){
return this.length;
};
RadNumericRangeMaskPart.prototype.HandleKey=function(e){
this.controller.CalculateSelection();
var _15=new MaskedEventWrap(e,this.controller.field);
if(_15.IsDownArrow()){
this.MoveDown();
this.controller.FixSelection(_15);
return true;
}else{
if(_15.IsUpArrow()){
this.MoveUp();
this.controller.FixSelection(_15);
return true;
}
}
};
RadNumericRangeMaskPart.prototype.MoveUp=function(){
var _16=this.value;
_16++;
if(_16>this.upperLimit){
_16=this.lowerLimit;
this.FlipDirection=1;
}
this.setInternalValue(_16);
this.controller.Visualise();
};
RadNumericRangeMaskPart.prototype.MoveDown=function(){
var _17=this.value;
_17--;
if(_17<this.lowerLimit){
_17=this.upperLimit;
this.FlipDirection=-1;
}
this.setInternalValue(_17);
this.controller.Visualise();
};
RadNumericRangeMaskPart.prototype.HandleWheel=function(e){
var _19=this.value;
_19=parseInt(_19)+parseInt(e.wheelDelta/120);
var _1a=new MaskedEventWrap(e,this.controller.field);
if(_19<this.lowerLimit){
_19=this.upperLimit-(this.lowerLimit-_19-1);
this.FlipDirection=-1;
}
if(_19>this.upperLimit){
_19=this.lowerLimit+(_19-this.upperLimit-1);
this.FlipDirection=1;
}
this.setInternalValue(_19);
this.controller.Visualise();
this.controller.FixSelection(_1a);
return false;
};;function RadPasswordMaskPart(){
}
RadPasswordMaskPart.prototype=new RadMaskPart();
RadPasswordMaskPart.prototype.IsCaseSensitive=function(){
return true;
};
RadPasswordMaskPart.prototype.GetVisValue=function(){
if(this.value.toString()==""){
return this.PromptChar;
}
return "*";
};
RadPasswordMaskPart.prototype.SetValue=function(_1,_2){
this.value=_1;
return true;
};;function RadUpperMaskPart(){
}
RadUpperMaskPart.prototype=new RadMaskPart();
RadUpperMaskPart.prototype.CanHandle=function(_1,_2){
if(!RadMaskPart.IsAlpha(_1)){
this.controller.OnChunkError(this,this.GetValue(),_1);
return false;
}
return true;
};
RadUpperMaskPart.prototype.GetVisValue=function(){
if(this.value.toString()==""){
return this.PromptChar;
}
return this.value.toString();
};
RadUpperMaskPart.prototype.SetValue=function(_3,_4){
if(_3==""){
this.value="";
return true;
}
if(RadMaskPart.IsAlpha(_3)){
this.value=_3.toUpperCase();
}else{
this.controller.OnChunkError(this,this.GetValue(),_3);
}
return true;
};;//BEGIN_ATLAS_NOTIFY
if (typeof(Sys) != "undefined"){if (Sys.Application != null && Sys.Application.notifyScriptLoaded != null){Sys.Application.notifyScriptLoaded();}}
//END_ATLAS_NOTIFY
