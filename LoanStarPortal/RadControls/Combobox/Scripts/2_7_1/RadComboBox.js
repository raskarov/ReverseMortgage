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
function RadComboItem(){
this.ComboBox=null;
this.ClientID=null;
this.Highlighted=false;
this.Index=0;
this.Enabled=1;
this.Selected=0;
this.Text="";
this.Value=null;
this.Attributes=new Array();
}
RadComboItem.prototype.Initialize=function(_5){
for(var _6 in _5){
this[_6]=_5[_6];
}
};
RadComboItem.prototype.AdjustDownScroll=function(){
var _7=0;
var _8=document.getElementById(this.ComboBox.ClientID+"_DropDown");
if(_8.offsetWidth!=_8.scrollWidth+16){
_7=16;
}
if(this.ComboBox.Items.length>0){
var _9=0;
for(var i=0;i<=this.Index;i++){
var _b=document.getElementById(this.ComboBox.Items[i].ClientID);
if(_b){
_9+=_b.offsetHeight;
}
}
_8.scrollTop=_9-_8.offsetHeight+_7;
}
};
RadComboItem.prototype.AdjustUpScroll=function(){
if(this.ComboBox.Items.length>0){
var _c=0;
for(var i=0;i<this.Index;i++){
var _e=document.getElementById(this.ComboBox.Items[i].ClientID);
if(_e){
_c+=_e.offsetHeight;
}
}
var _f=document.getElementById(this.ComboBox.ClientID+"_DropDown").scrollTop;
if(_f>_c){
document.getElementById(this.ComboBox.ClientID+"_DropDown").scrollTop=_c;
}
}
};
RadComboItem.prototype.Highlight=function(){
if(!this.Highlighted&&this.Enabled){
this.ComboBox.UnHighlightAll();
if(!this.ComboBox.IsTemplated||this.ComboBox.HighlightTemplatedItems){
var _10=document.getElementById(this.ClientID);
if(_10){
if(!this.ComboBox.HighlightedItem){
if(_10.className!=this.ComboBox.ItemCssClassHover){
this.CssClass=_10.className;
}else{
this.CssClass=this.ComboBox.ItemCssClass;
}
}
_10.className=this.ComboBox.ItemCssClassHover;
}
}
this.Highlighted=true;
this.ComboBox.HighlightedItem=this;
}
};
RadComboItem.prototype.UnHighlight=function(){
if(this.Highlighted&&this.Enabled&&document.getElementById(this.ClientID)){
document.getElementById(this.ClientID).className=this.CssClass;
this.Highlighted=false;
this.ComboBox.HighlightedItem=null;
}
};
RadComboItem.prototype.Select=function(){
this.ComboBox.SelectedItem=this;
this.ComboBox.SetState(this);
this.ComboBox.HideDropDown();
this.ComboBox.PostBackActive=true;
this.ComboBox.PostBack();
};
function RadComboBox(_11,_12,_13){
var _14=window[_12];
if(_14!=null&&!_14.tagName){
_14.Dispose();
}
if(window.tlrkComboBoxes==null){
window.tlrkComboBoxes=new Array();
}
tlrkComboBoxes[tlrkComboBoxes.length]=this;
this.Items=new Array();
this.Created=false;
this.ID=_11;
this.ClientID=_12;
this.TagID=_12;
this.DropDownID=_12+"_DropDown";
this.InputID=_12+"_Input";
this.ImageID=_12+"_Image";
this.DropDownPlaceholderID=_12+"_DropDownPlaceholder";
this.MoreResultsBoxID=_12+"_MoreResultsBox";
this.MoreResultsBoxImageID=_12+"_MoreResultsBoxImage";
this.MoreResultsBoxMessageID=_12+"_MoreResultsBoxMessage";
this.Header=_12+"_Header";
this.InputDomElement=document.getElementById(this.InputID);
this.ImageDomElement=document.getElementById(this.ImageID);
this.DropDownPlaceholderDomElement=document.getElementById(this.DropDownPlaceholderID);
this.TextHidden=document.getElementById(this.ClientID+"_text");
this.ValueHidden=document.getElementById(this.ClientID+"_value");
this.IndexHidden=document.getElementById(this.ClientID+"_index");
this.ClientWidthHidden=document.getElementById(this.ClientID+"_clientWidth");
this.ClientHeightHidden=document.getElementById(this.ClientID+"_clientHeight");
this.Enabled=true;
this.DropDownVisible=false;
this.LoadOnDemandUrl=null;
this.PollTimeOut=0;
this.HighlightedItem=null;
this.SelectedItem=null;
this.ItemRequestTimeout=300;
this.EnableLoadOnDemand=false;
this.AutoPostBack=false;
this.ShowMoreResultsBox=false;
this.OpenDropDownOnLoad=false;
this.CurrentlyPolling=false;
this.MarkFirstMatch=false;
this.IsCaseSensitive=false;
this.SelectOnTab=true;
this.PostBackReference=null;
this.LoadingMessage="Loading...";
this.ScrollDownImage=null;
this.ScrollDownImageDisabled=null;
this.IFrameShim=null;
this.RadComboBoxImagePosition="Right";
this.ItemCssClass=null;
this.ItemCssClassHover=null;
this.ItemCssClassDisabled=null;
this.ImageCssClass=null;
this.ImageCssClassHover=null;
this.InputCssClass=null;
this.InputCssClassHover=null;
this.LoadingMessageCssClass="ComboBoxLoadingMessage";
this.AutoCompleteSeparator=null;
this.ExternalCallBackPage=null;
this.OnClientSelectedIndexChanging=null;
this.OnClientDropDownOpening=null;
this.OnClientDropDownClosing=null;
this.OnClientItemsRequesting=null;
this.OnClientSelectedIndexChanged=null;
this.OnClientItemsRequested=null;
this.OnClientKeyPressing=null;
this.Skin="Classic";
this.HideTimeoutID=0;
this.RequestTimeoutID=0;
this.IsDetached=false;
this.TextPriorToCallBack=null;
this.AllowCustomText=false;
this.ExpandEffectString=null;
this.HighlightTemplatedItems=false;
this.CausesValidation=false;
this.ClientDataString=null;
this.ShowDropDownOnTextboxClick=true;
this.ShowWhileLoading=_13;
this.MoreResultsImageHovered=false;
this.PostBackActive=false;
this.SelectedIndex=-1;
this.IsTemplated=false;
this.CurrentText=null;
this.OffsetX=0;
this.OffsetY=0;
this.Disposed=false;
this.DetermineDirection();
var _15=this;
this.HideOnClickHandler=function(){
_15.HideOnClick();
};
this.AttachEvent(document,"click",this.HideOnClickHandler);
this.OnBlurHandler=function(e){
_15.HandleBlur(e||event);
};
this.AttachEvent(this.InputDomElement,"blur",this.OnBlurHandler);
this.OnFocusHandler=function(){
_15.HandleFocus();
};
this.AttachEvent(this.InputDomElement,"focus",this.OnFocusHandler);
this.InputDomElement.setAttribute("autocomplete","off");
this.DropDownPlaceholderDomElement.onselectstart=function(){
return false;
};
this.OnWindowLoadHandler=function(){
_15.FixUp(_15.InputDomElement,true);
};
if(typeof (RadCallbackNamespace)!="undefined"){
window.setTimeout(function(){
_15.FixUp(document.getElementById(_15.InputID),true);
},100);
}else{
if(window.addEventListener){
if(window.opera){
this.AttachEvent(window,"load",this.OnWindowLoadHandler);
}else{
this.OnWindowLoadHandler();
}
}else{
if(document.getElementById(this.ClientID).offsetWidth==0){
this.AttachEvent(window,"load",this.OnWindowLoadHandler);
}else{
this.OnWindowLoadHandler();
}
}
}
this.OnUnloadHandler=function(){
_15.Dispose();
};
this.AttachEvent(window,"unload",this.OnUnloadHandler);
}
RadComboBox.prototype.AttachEvent=function(_17,_18,_19){
if(_17.attachEvent){
_17.attachEvent("on"+_18,_19);
}else{
if(_17.addEventListener){
_17.addEventListener(_18,_19,false);
}
}
};
RadComboBox.prototype.DetachEvent=function(_1a,_1b,_1c){
if(_1a.detachEvent){
_1a.detachEvent("on"+_1b,_1c);
}else{
if(_1a.removeEventListener){
_1a.removeEventListener(_1b,_1c,false);
}
}
};
RadComboBox.prototype.ClearItems=function(){
this.Items=[];
document.getElementById(this.DropDownID).innerHTML="";
};
RadComboBox.prototype.GetViewPortSize=function(){
var _1d=0;
var _1e=0;
var _1f=document.body;
if(window.innerWidth){
_1d=window.innerWidth;
_1e=window.innerHeight;
}else{
if(document.compatMode&&document.compatMode=="CSS1Compat"){
_1f=document.documentElement;
}
_1d=_1f.clientWidth;
_1e=_1f.clientHeight;
}
_1d+=_1f.scrollLeft;
_1e+=_1f.scrollTop;
return {width:_1d-6,height:_1e-6};
};
RadComboBox.prototype.GetElementPosition=function(el){
var _21=null;
var pos={x:0,y:0};
var box;
if(el.getBoundingClientRect){
box=el.getBoundingClientRect();
var _24=document.documentElement.scrollTop||document.body.scrollTop;
var _25=document.documentElement.scrollLeft||document.body.scrollLeft;
pos.x=box.left+_25-2;
pos.y=box.top+_24-2;
return pos;
}else{
if(document.getBoxObjectFor){
box=document.getBoxObjectFor(el);
pos.x=box.x-2;
pos.y=box.y-2;
}else{
pos.x=el.offsetLeft;
pos.y=el.offsetTop;
_21=el.offsetParent;
if(_21!=el){
while(_21){
pos.x+=_21.offsetLeft;
pos.y+=_21.offsetTop;
_21=_21.offsetParent;
}
}
}
}
if(window.opera){
_21=el.offsetParent;
while(_21&&_21.tagName!="BODY"&&_21.tagName!="HTML"){
pos.x-=_21.scrollLeft;
pos.y-=_21.scrollTop;
_21=_21.offsetParent;
}
}else{
_21=el.parentNode;
while(_21&&_21.tagName!="BODY"&&_21.tagName!="HTML"){
pos.x-=_21.scrollLeft;
pos.y-=_21.scrollTop;
_21=_21.parentNode;
}
}
return pos;
};
RadComboBox.prototype.Dispose=function(){
this.DropDownPlaceholderDomElement.onselectstart=null;
this.HideDropDown();
if(this.DropDownPlaceholderDomElement!=null&&this.DropDownPlaceholderDomElement.parentNode!=null){
this.DropDownPlaceholderDomElement.parentNode.removeChild(this.DropDownPlaceholderDomElement);
}
this.DetachEvent(document,"click",this.HideOnClickHandler);
this.DetachEvent(this.InputDomElement,"blur",this.OnBlurHandler);
this.DetachEvent(this.InputDomElement,"focus",this.OnFocusHandler);
this.DetachEvent(window,"load",this.OnWindowLoadHandler);
this.DetachEvent(window,"unload",this.OnUnloadHandler);
this.InputDomElement=null;
this.DropDownPlaceholderDomElement=null;
this.Items=null;
this.ImageDomElement=null;
this.TextHidden=null;
this.ValueHidden=null;
this.IndexHidden=null;
this.IFrameShim=null;
tlrkComboBoxes[this.ID]=null;
this.Disposed=true;
};
RadComboBox.prototype.Initialize=function(_26,_27){
this.LoadConfiguration(_26);
this.CreateItems(_27);
this.InitCssNames();
this.HighlightSelectedItem();
};
RadComboBox.prototype.LoadConfiguration=function(_28){
for(var _29 in _28){
this[_29]=_28[_29];
}
};
RadComboBox.prototype.InitCssNames=function(){
this.ItemCssClass="ComboBoxItem_"+this.Skin;
this.ItemCssClassHover="ComboBoxItemHover_"+this.Skin;
this.ItemCssClassDisabled="ComboBoxItemDisabled_"+this.Skin;
this.ImageCssClass="ComboBoxImage_"+this.Skin;
this.ImageCssClassHover="ComboBoxImageHover_"+this.Skin;
this.InputCssClass="ComboBoxInput_"+this.Skin;
this.InputCssClassHover="ComboBoxInputHover_"+this.Skin;
this.LoadingMessageCssClass="ComboBoxLoadingMessage_"+this.Skin;
};
RadComboBox.prototype.SetState=function(_2a){
if(_2a!=null){
this.SetTextAfterLastDelimiter(_2a.Text);
this.SetValue(_2a.Value);
this.SetIndex(_2a.Index);
}else{
this.SetText("");
this.SetValue("");
this.SetIndex("-1");
}
};
RadComboBox.prototype.PostBack=function(){
if(this.AutoPostBack){
if(this.CausesValidation){
if(typeof (WebForm_DoPostBackWithOptions)!="function"&&!(typeof (Page_ClientValidate)!="function"||Page_ClientValidate())){
return;
}
}
eval(this.PostBackReference);
}
};
RadComboBox.prototype.HandleBlur=function(e){
var _2c=this.SelectedItem;
var _2d=this.HighlightedItem;
if(_2c!=null&&_2d!=null&&_2c!=_2d){
if(this.FireEvent(this.OnClientSelectedIndexChanging,_2d,e)==false){
return;
}
this.SetState(_2d);
this.ExecuteAction();
if(this.HighlightedItem==null){
_2d.Select();
this.FireEvent(this.OnClientSelectedIndexChanged,_2d,e);
}
}
if(this.MoreResultsImageHovered){
return;
}
var _2e=this.CurrentText;
var _2f=this.GetText();
if(_2e!=_2f&&this.AllowCustomText){
this.SetText(this.GetText());
if(!this.PostBackActive){
if(_2d!=null||_2e!=_2f){
if(this.FireEvent(this.OnClientSelectedIndexChanging,_2d,e)==false){
return;
}
if(_2d!=null){
this.SetText(_2d.Text);
this.SetValue(_2d.Value);
}
this.PostBack();
}
}else{
this.PostBackActive=false;
}
}
};
RadComboBox.prototype.HandleFocus=function(e){
this.CurrentText=this.GetText();
this.RaiseOnClientFocus();
};
RadComboBox.prototype.FindParentForm=function(){
var _31=document.getElementById(this.TagID);
while(_31.tagName!="FORM"){
_31=_31.parentNode;
}
return _31;
};
RadComboBox.prototype.DropDownRequiresForm=function(){
var _32=this.DropDownPlaceholderDomElement.getElementsByTagName("input");
return _32.length>0;
};
RadComboBox.prototype.DetachDropDown=function(){
if((!document.readyState||document.readyState=="complete")&&(!this.IsDetached)){
var _33=document.body;
if(this.DropDownRequiresForm()){
_33=this.FindParentForm();
}
this.DropDownPlaceholderDomElement.parentNode.removeChild(this.DropDownPlaceholderDomElement);
this.DropDownPlaceholderDomElement.style.marginLeft="0";
_33.insertBefore(this.DropDownPlaceholderDomElement,_33.firstChild);
this.IsDetached=true;
}
};
RadComboBox.prototype.HideOnClick=function(){
var _34=this;
this.HideTimeoutID=window.setTimeout(function(){
_34.DoHideOnClick();
},5);
};
RadComboBox.prototype.DoHideOnClick=function(){
if(this.HideTimeoutID){
this.HideDropDown();
}
};
RadComboBox.prototype.ClearHideTimeout=function(){
this.HideTimeoutID=0;
};
RadComboBox.prototype.GetLastSeparatorIndex=function(_35){
var _36=-1;
for(var i=0;i<this.AutoCompleteSeparator.length;i++){
var _38=this.AutoCompleteSeparator.charAt(i);
var _39=_35.lastIndexOf(_38);
if(_39>_36){
_36=_39;
}
}
return _36;
};
RadComboBox.prototype.SetTextAfterLastDelimiter=function(_3a){
var _3b=-1;
var _3c=this.GetText();
if(this.AutoCompleteSeparator!=null){
_3b=this.GetLastSeparatorIndex(_3c);
}
var _3d=_3c.substring(0,_3b+1)+_3a;
this.SetText(_3d);
};
RadComboBox.prototype.ClearSelection=function(){
this.SetState(null);
this.SelectedItem=null;
this.HighLightedItem=null;
};
RadComboBox.prototype.CreateItems=function(_3e){
for(var i=0;i<_3e.length;i++){
var _40=new RadComboItem();
_40.ComboBox=this;
_40.Index=this.Items.length;
_40.Initialize(_3e[i]);
this.Items[this.Items.length]=_40;
if(_40.Selected&&!this.AllowCustomText){
this.SetText(_40.Text);
this.SetValue(_40.Value);
}
}
};
RadComboBox.prototype.HighlightSelectedItem=function(){
if(this.SelectedItem!=null){
this.SelectedItem.Highlight();
}else{
var _41;
var _42=this.GetValue();
_41=this.FindItemByValue(_42);
if(_41==null){
var _43=this.GetText();
_41=this.FindItemByText(_43);
}
if(_41!=null){
this.SelectedItem=_41;
this.SelectedItem.Highlight();
}
}
this.Created=true;
if(this.SelectedItem==null&&this.SelectedIndex==-1&&this.Items.length>0){
this.SelectedItem=this.Items[0];
this.Items[0].Selected=true;
this.SelectedItem.Highlight();
}
var _44=this;
if(this.OpenDropDownOnLoad){
if(window.attachEvent){
window.attachEvent("onload",function(){
_44.ShowDropDown();
});
}else{
window.addEventListener("load",function(){
_44.ShowDropDown();
},false);
}
}
};
RadComboBox.prototype.InitializeAfterCallBack=function(_45,_46){
if(!_46){
this.Items.length=0;
}
this.HighlightedItem=null;
this.SelectedItem=null;
this.Created=false;
if(this.Items.length>0){
if(this.Items[0].Text==document.getElementById(this.InputID).value){
this.SetValue(this.Items[0].Value);
}else{
this.SetValue("");
}
this.TextPriorToCallBack=this.GetText();
}
this.CreateItems(_45);
};
RadComboBox.prototype.SetText=function(_47){
document.getElementById(this.InputID).value=_47;
this.TextHidden.value=_47;
};
RadComboBox.prototype.GetText=function(){
return document.getElementById(this.InputID).value;
};
RadComboBox.prototype.SetValue=function(_48){
if(_48||_48==""){
this.ValueHidden.value=_48;
}
};
RadComboBox.prototype.GetValue=function(){
return this.ValueHidden.value;
};
RadComboBox.prototype.SetIndex=function(_49){
this.IndexHidden.value=_49;
};
RadComboBox.prototype.getXY=function(el){
var _4b=null;
var pos=[];
var box;
if(el.getBoundingClientRect){
box=el.getBoundingClientRect();
var _4e=document.documentElement.scrollTop||document.body.scrollTop;
var _4f=document.documentElement.scrollLeft||document.body.scrollLeft;
var x=box.left+_4f-2;
var y=box.top+_4e-2;
return [x,y];
}else{
if(document.getBoxObjectFor){
box=document.getBoxObjectFor(el);
pos=[box.x-1,box.y-1];
}else{
pos=[el.offsetLeft,el.offsetTop];
_4b=el.offsetParent;
if(_4b!=el){
while(_4b){
pos[0]+=_4b.offsetLeft;
pos[1]+=_4b.offsetTop;
_4b=_4b.offsetParent;
}
}
}
}
if(window.opera){
_4b=el.offsetParent;
while(_4b&&_4b.tagName.toUpperCase()!="BODY"&&_4b.tagName.toUpperCase()!="HTML"){
pos[0]-=_4b.scrollLeft;
pos[1]-=_4b.scrollTop;
_4b=_4b.offsetParent;
}
}else{
_4b=el.parentNode;
while(_4b&&_4b.tagName.toUpperCase()!="BODY"&&_4b.tagName.toUpperCase()!="HTML"){
pos[0]-=_4b.scrollLeft;
pos[1]-=_4b.scrollTop;
_4b=_4b.parentNode;
}
}
return pos;
};
RadComboBox.prototype.ShowOverlay=function(x,y){
if(document.readyState&&document.readyState!="complete"){
return;
}
var _54=(navigator.userAgent.toLowerCase().indexOf("safari")!=-1);
var _55=window.opera;
if(_54||_55||(!document.all)){
return;
}
if(this.IFrameShim==null){
this.IFrameShim=document.createElement("IFRAME");
this.IFrameShim.src="javascript:''";
this.IFrameShim.id=this.ClientID+"_Overlay";
this.IFrameShim.frameBorder=0;
this.IFrameShim.style.position="absolute";
this.IFrameShim.style.display="none";
this.DetachDropDown();
this.DropDownPlaceholderDomElement.parentNode.insertBefore(this.IFrameShim,this.DropDownPlaceholderDomElement);
this.IFrameShim.style.zIndex=this.DropDownPlaceholderDomElement.style.zIndex-1;
}
this.IFrameShim.style.left=x;
this.IFrameShim.style.top=y;
var _56=this.DropDownPlaceholderDomElement.offsetWidth;
var _57=this.DropDownPlaceholderDomElement.offsetHeight;
this.IFrameShim.style.width=_56+"px";
this.IFrameShim.style.height=_57+"px";
this.IFrameShim.style.display="";
};
RadComboBox.prototype.HideOverlay=function(){
var _58=(navigator.userAgent.toLowerCase().indexOf("safari")!=-1);
var _59=window.opera;
if(_58||_59||(!document.all)){
return;
}
if(this.IFrameShim!=null){
this.IFrameShim.style.display="none";
}
};
RadComboBox.prototype.HideAllDropDowns=function(){
for(var i=0;i<tlrkComboBoxes.length;i++){
if(tlrkComboBoxes[i].ClientID!=this.ClientID){
tlrkComboBoxes[i].HideDropDown();
}
}
};
RadComboBox.prototype.DetermineDirection=function(){
var el=document.getElementById(this.ClientID+"_wrapper");
while(el.tagName.toLowerCase()!="html"){
if(el.dir){
this.RightToLeft=(el.dir.toLowerCase()=="rtl");
return;
}
el=el.parentNode;
}
this.RightToLeft=false;
};
RadComboBox.prototype.ShowDropDown=function(){
if(this.FireEvent(this.OnClientDropDownOpening,this)==false){
return;
}
this.HideAllDropDowns();
this.DetachDropDown();
var _5c;
(this.RadComboBoxImagePosition=="Right"&&!this.RightToLeft)?_5c=this.InputDomElement:_5c=document.getElementById(this.ImageID);
var _5d=this.getXY(_5c);
var x=_5d[0]+this.OffsetX;
var y=_5d[1]+_5c.offsetHeight+this.OffsetY;
var _60=document.getElementById(this.TagID);
dropDownWidth=_60.offsetWidth;
if(this.ExpandEffectString!=null&&document.all){
try{
this.DropDownPlaceholderDomElement.style.filter=this.ExpandEffectString;
this.DropDownPlaceholderDomElement.filters[0].Apply();
this.DropDownPlaceholderDomElement.filters[0].Play();
}
catch(e){
}
}
if(this.RightToLeft){
this.DropDownPlaceholderDomElement.dir="rtl";
}
var _61=this.GetViewPortSize();
this.DropDownPlaceholderDomElement.style.position="absolute";
if(window.netscape||window.opera){
dropDownWidth-=2;
}
this.DropDownPlaceholderDomElement.style.width=dropDownWidth+"px";
this.DropDownPlaceholderDomElement.style.display="block";
if(this.ElementOverflowsBottom(_61,this.DropDownPlaceholderDomElement,_5c)){
var _62=y-this.DropDownPlaceholderDomElement.offsetHeight-_5c.offsetHeight;
if(_62>0){
y=_62;
}
}
this.DropDownPlaceholderDomElement.style.left=x+"px";
this.DropDownPlaceholderDomElement.style.top=y+"px";
this.ShowOverlay(x+"px",y+"px");
if(this.HighlightedItem!=null){
this.HighlightedItem.AdjustDownScroll();
}
if(this.SelectedItem!=null){
this.SelectedItem.AdjustDownScroll();
}
this.ClearHideTimeout();
this.DropDownVisible=true;
try{
document.getElementById(this.InputID).focus();
}
catch(e){
}
if((this.EnableLoadOnDemand)&&(this.Items.length==0)){
this.PollServerInterMediate(true,null);
}
if(this.SelectedItem!=null){
this.SelectedItem.Highlighted=false;
this.SelectedItem.Highlight();
this.SelectedItem.AdjustUpScroll();
}
};
RadComboBox.prototype.ElementOverflowsBottom=function(_63,_64,_65){
var _66=this.GetElementPosition(_65).y+_64.offsetHeight;
return _66>_63.height;
};
RadComboBox.prototype.FindItemByText=function(_67){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Text==_67){
return this.Items[i];
}
}
return null;
};
RadComboBox.prototype.FindItemByValue=function(_69){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Value==_69){
return this.Items[i];
}
}
return null;
};
RadComboBox.prototype.HideDropDown=function(){
if(this.DropDownVisible){
if(this.FireEvent(this.OnClientDropDownClosing,this)==false){
return;
}
this.DropDownPlaceholderDomElement.style.display="none";
this.HideOverlay();
this.DropDownVisible=false;
this.RaiseOnClientBlur();
}
};
RadComboBox.prototype.RaiseOnClientBlur=function(){
this.FireEvent(this.OnClientBlur,this);
};
RadComboBox.prototype.RaiseOnClientFocus=function(){
this.FireEvent(this.OnClientFocus,this);
};
RadComboBox.prototype.ToggleDropDown=function(){
(this.DropDownVisible)?this.HideDropDown():this.ShowDropDown();
};
RadComboBox.prototype.HtmlElementToItem=function(obj){
if(obj){
while(obj!=null){
if(obj.id&&this.IsElementAnItem(obj.id)){
return obj;
}
obj=obj.parentNode;
}
}
return null;
};
RadComboBox.prototype.IsElementAnItem=function(_6c){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].ClientID==_6c){
return true;
}
}
return false;
};
RadComboBox.prototype.ItemToInstance=function(_6e){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].ClientID==_6e.id){
return this.Items[i];
}
}
return null;
};
RadComboBox.prototype.HandleMouseOver=function(_70){
_70.Highlight();
};
RadComboBox.prototype.HandleMouseOut=function(_71){
_71.UnHighlight();
};
RadComboBox.prototype.ExecuteAction=function(_72){
var _73=this.HighlightedItem;
if(_73!=null){
if(this.FireEvent(this.OnClientSelectedIndexChanging,_73,_72)==false){
return;
}
_73.Select();
this.FireEvent(this.OnClientSelectedIndexChanged,_73,_72);
}
this.HideDropDown();
};
RadComboBox.prototype.HandleClick=function(_74){
this.ExecuteAction(_74);
};
RadComboBox.prototype.FindNextAvailableItem=function(_75){
var i=_75;
var _77=false;
while(i<this.Items.length-1){
i=i+1;
if(this.Items[i].Enabled){
_77=true;
break;
}
}
if(_77){
return i;
}
return _75;
};
RadComboBox.prototype.FindPrevAvailableItem=function(_78){
var i=_78;
var _7a=false;
while(i>0){
i=i-1;
if(this.Items[i].Enabled){
_7a=true;
break;
}
}
if(_7a){
return i;
}
return _78;
};
RadComboBox.prototype.HandleKeyPress=function(_7b,_7c){
this.FireEvent(this.OnClientKeyPressing,this,_7c);
var _7d=_7c.keyCode;
if(_7d==40){
if(_7c.altKey&&(!this.DropDownVisible)){
this.ShowDropDown();
return;
}
var _7e=0;
if(this.HighlightedItem!=null){
_7e=this.FindNextAvailableItem(this.HighlightedItem.Index);
}
if(_7e>=0&&this.Items.length>0){
if(this.FireEvent(this.OnClientSelectedIndexChanging,this.Items[_7e],_7c)==false){
return;
}
this.Items[_7e].Highlight();
this.Items[_7e].AdjustDownScroll();
this.SetState(this.Items[_7e]);
this.PreventDefault(_7c);
}
return;
}
if(_7d==27&&this.DropDownVisible){
this.HideDropDown();
return;
}
if(_7d==38){
if(_7c.altKey&&this.DropDownVisible){
this.HideDropDown();
return;
}
var _7e=-1;
if(this.HighlightedItem!=null){
_7e=this.FindPrevAvailableItem(this.HighlightedItem.Index);
}
if(_7e>=0){
if(this.FireEvent(this.OnClientSelectedIndexChanging,this.Items[_7e],_7c)==false){
return;
}
this.Items[_7e].AdjustUpScroll();
this.Items[_7e].Highlight();
this.SetState(this.Items[_7e]);
this.PreventDefault(_7c);
}
return;
}
if((_7d==13||_7d==9)&&this.DropDownVisible){
if(_7d==13){
this.PreventDefault(_7c);
}
if(!this.SelectOnTab&&_7d==9){
if(this.AutoPostBack){
this.PostBack();
}
this.HideDropDown();
return;
}
this.ExecuteAction();
return;
}
if(_7d==9&&!this.DropDownVisible){
this.RaiseOnClientBlur();
return;
}
if(_7d==35||_7d==36||_7d==37||_7d==39){
return;
}
if(this.EnableLoadOnDemand&&(!_7c.altKey)&&(!_7c.ctrlKey)&&(!(_7d==16))){
if(!this.DropDownVisible){
this.ShowDropDown();
}
this.PollServer(false,_7d);
return;
}
if((_7d<32)||(_7d>=33&&_7d<=46)||(_7d>=112&&_7d<=123)||(_7c.altKey==true)){
return;
}
var _7f=this;
window.setTimeout(function(){
_7f.HighlightMatches();
},20);
};
RadComboBox.prototype.HandleKeyDown=function(_80){
if(_80.preventDefault){
if(_80.keyCode==13||(_80.keyCode==32&&(!this.EnableLoadOnDemand))){
_80.preventDefault();
}
}
};
RadComboBox.prototype.EncodeURI=function(s){
if(typeof (encodeURIComponent)!="undefined"){
return encodeURIComponent(this.EscapeQuotes(s));
}
if(escape){
return escape(this.EscapeQuotes(s));
}
};
RadComboBox.prototype.EscapeQuotes=function(_82){
if(typeof (_82)!="number"){
return _82.replace(/'/g,"&squote");
}
};
RadComboBox.prototype.GetXmlHttpRequest=function(){
if(typeof (XMLHttpRequest)!="undefined"){
return new XMLHttpRequest();
}
if(typeof (ActiveXObject)!="undefined"){
return new ActiveXObject("Microsoft.XMLHTTP");
}
};
RadComboBox.prototype.GetAjaxUrl=function(_83,_84,_85,_86){
_83=_83.replace(/'/g,"&squote");
var url=window.unescape(this.LoadOnDemandUrl)+"&text="+this.EncodeURI(_83);
url=url+"&comboText="+this.EncodeURI(_84);
url=url+"&comboValue="+this.EncodeURI(_85);
url=url+"&skin="+this.EncodeURI(this.Skin);
if(_86){
url=url+"&itemCount="+this.Items.length;
}
if(this.ExternalCallBackPage!=null){
url=url+"&external=true";
}
if(this.ClientDataString!=null){
url+="&clientDataString="+this.EncodeURI(this.ClientDataString);
}
url=url+"&timeStamp="+encodeURIComponent((new Date()).getTime());
return url;
};
RadComboBox.prototype.FetchCallBackData=function(_88,_89,_8a){
if(!this.CurrentlyPolling){
if(this.Disposed){
return;
}
this.CurrentlyPolling=true;
var _8b=this.GetText();
var _8c=this.GetValue();
var _8d=(_89)?_89:_8b;
var _8e=this.GetAjaxUrl(_8d,_8b,_8c,_88);
var _8f=this;
var _90=this.GetXmlHttpRequest();
_90.onreadystatechange=function(){
if(_90.readyState!=4){
return;
}
_8f.OnCallBackResponse(_90.responseText,_88,_8d,_8a,_90.status,_8e);
};
_90.open("GET",_8e,true);
_90.setRequestHeader("Content-Type","application/json; charset=utf-8");
_90.send("");
}
};
RadComboBox.prototype.OnCallBackResponse=function(_91,_92,_93,_94,_95,url){
if(_95==500){
alert("RadComboBox: Server error in the ItemsRequested event handler, press ok to view the result.");
document.body.innerHTML=_91;
return;
}
if(_95==404){
alert("RadComboBox: Load On Demand Page not found: "+url);
var _97="RadComboBox: Load On Demand Page not found: "+url+"<br/>";
_97+="Please, try using ExternalCallBackPage to map to the exact location of the callbackpage you are using.";
document.body.innerHTML=_97;
return;
}
try{
eval("var callBackData = "+_91+";");
}
catch(e){
alert("RadComboBox: load on demand callback error. Press Enter for more information");
var _97="If RadComboBox is not initially visible on your ASPX page, you may need to use streamers (the ExternallCallBackPage property)";
_97+="<br/>Please, read our online documentation on this problem for details";
_97+="<br/><a href='http://www.telerik.com/help/radcombobox/v2%5FNET2/?combo_externalcallbackpage.html'>http://www.telerik.com/help/radcombobox/v2%5FNET2/combo_externalcallbackpage.html</a>";
document.body.innerHTML=_97;
return;
}
if(this.GetText()!=callBackData.Text){
this.CurrentlyPolling=false;
this.PollServer(false,null);
return;
}
if(this.ShowMoreResultsBox){
document.getElementById(this.MoreResultsBoxMessageID).innerHTML=callBackData.Message;
}
var _98=this.Items.length;
this.InitializeAfterCallBack(callBackData.Items,_92);
if(_92){
document.getElementById(this.DropDownID).removeChild(document.getElementById(this.ClientID+"_LoadingDiv"));
document.getElementById(this.DropDownID).innerHTML+=callBackData.DropDownHtml;
if(this.Items[_98+1]!=null){
this.Items[_98+1].AdjustDownScroll();
}
}else{
document.getElementById(this.DropDownID).innerHTML=callBackData.DropDownHtml;
}
this.ShowOverlay(this.DropDownPlaceholderDomElement.style.left,this.DropDownPlaceholderDomElement.style.top);
this.FireEvent(this.OnClientItemsRequested,this,_93,_92);
this.CurrentlyPolling=false;
var _99=this.FindItemByText(this.GetText());
if(_99!=null){
_99.Highlight();
_99.AdjustDownScroll();
}
if(!_94){
return;
}
if(_94<32||(_94>=33&&_94<=46)||(_94>=112&&_94<=123)&&_94!=8){
return;
}
this.HighlightMatches();
};
RadComboBox.prototype.GetLastWord=function(_9a){
var _9b=-1;
if(this.AutoCompleteSeparator!=null){
_9b=this.GetLastSeparatorIndex(_9a);
}
var _9c=_9a.substring(_9b+1,_9a.length);
return _9c;
};
RadComboBox.prototype.CompareWords=function(_9d,_9e){
if(!this.IsCaseSensitive){
return (_9d.toLowerCase()==_9e.toLowerCase());
}else{
return (_9d==_9e);
}
};
RadComboBox.prototype.HighlightMatches=function(){
if(!this.MarkFirstMatch){
return;
}
var _9f=this.GetText();
var _a0=this.GetLastWord(_9f);
if(_a0.length==0){
return;
}
for(var i=0;i<this.Items.length;i++){
var _a2=this.Items[i].Text;
if(_a2.length>=_a0.length){
var _a3=_a2.substring(0,_a0.length);
if(this.CompareWords(_a3,_a0)){
var _a4=-1;
if(this.AutoCompleteSeparator!=null){
_a4=this.GetLastSeparatorIndex(_9f);
}
var _a5=_9f.substring(0,_a4+1)+_a2;
this.SetText(_a5);
this.SetValue(this.Items[i].Value);
this.SetIndex(this.Items[i].Index);
if(this.FireEvent(this.OnClientSelectedIndexChanging,this.Items[i],null)==false){
return;
}
this.Items[i].Highlight();
this.Items[i].AdjustDownScroll();
var _a6=_a4+_a0.length+1;
var _a7=_a5.length-_a6;
if(document.all){
var _a8=document.getElementById(this.InputID).createTextRange();
_a8.moveStart("character",_a6);
_a8.moveEnd("character",_a7);
_a8.select();
}else{
document.getElementById(this.InputID).setSelectionRange(_a6,_a6+_a7);
}
return;
}else{
this.SetValue("");
this.SetIndex(-1);
if(this.HighlightedItem!=null){
this.HighlightedItem.UnHighlight();
}
}
}
}
this.SetValue("");
this.SetIndex("-1");
if(!this.AllowCustomText){
var _a9=_9f.substring(0,_9f.length-1);
if(this.TextPriorToCallBack!=null){
this.SetText(this.TextPriorToCallBack);
return;
}
this.SetText(_a9);
this.HighlightMatches();
}
};
RadComboBox.prototype.PollServer=function(_aa,_ab){
if(!this.CurrentlyPolling){
var _ac=this;
if(this.RequestTimeoutID){
window.clearTimeout(this.RequestTimeoutID);
this.RequestTimeoutID=0;
}
this.RequestTimeoutID=window.setTimeout(function(){
_ac.PollServerInterMediate(_aa,_ab);
},this.ItemRequestTimeout);
}
};
RadComboBox.prototype.PollServerInterMediate=function(_ad,_ae){
var _af=document.getElementById(this.InputID).value;
if(_af==""){
_af=false;
}
if(this.FireEvent(this.OnClientItemsRequesting,this,_af,_ad)==false){
return;
}
if(!this.CurrentlyPolling){
if(!document.getElementById(this.ClientID+"_LoadingDiv")){
document.getElementById(this.DropDownID).innerHTML="<div id='"+this.ClientID+"_LoadingDiv'"+" class='"+this.LoadingMessageCssClass+" '>"+this.LoadingMessage+"</div>"+document.getElementById(this.DropDownID).innerHTML;
}
}
var _b0=this;
window.setTimeout(function(){
_b0.FetchCallBackData(_ad,_af,_ae);
},20);
};
RadComboBox.prototype.RequestItems=function(_b1,_b2){
this.FetchCallBackData(_b2,_b1,null);
};
RadComboBox.prototype.UnHighlightAll=function(){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Highlighted){
this.Items[i].UnHighlight();
}
}
};
RadComboBox.prototype.HandleInputImageOut=function(){
document.getElementById(this.InputID).className=this.InputCssClass;
var _b4=document.getElementById(this.ImageID);
if(_b4){
_b4.className=this.ImageCssClass;
}
};
RadComboBox.prototype.HandleInputImageHover=function(){
document.getElementById(this.InputID).className=this.InputCssClassHover;
var _b5=document.getElementById(this.ImageID);
if(_b5){
_b5.className=this.ImageCssClassHover;
}
};
RadComboBox.prototype.HandleMoreResultsImageOut=function(){
document.getElementById(this.MoreResultsBoxImageID).style.cursor="default";
document.getElementById(this.MoreResultsBoxImageID).src=this.ScrollDownImageDisabled;
this.MoreResultsImageHovered=false;
};
RadComboBox.prototype.HandleMoreResultsImageHover=function(){
document.getElementById(this.MoreResultsBoxImageID).style.cursor="hand";
document.getElementById(this.MoreResultsBoxImageID).src=this.ScrollDownImage;
this.MoreResultsImageHovered=true;
};
RadComboBox.prototype.HandleMoreResultsImageClick=function(){
this.UnHighlightAll();
this.PollServer(true,null);
document.getElementById(this.InputID).focus();
};
RadComboBox.prototype.CancelPropagation=function(_b6){
if(_b6.stopPropagation){
_b6.stopPropagation();
}else{
_b6.cancelBubble=true;
}
};
RadComboBox.prototype.PreventDefault=function(_b7){
if(_b7.preventDefault){
_b7.preventDefault();
}else{
_b7.returnValue=false;
}
};
RadComboBox.prototype.FireEvent=function(_b8,a,b,c){
if(!_b8){
return true;
}
RadComboBoxGlobalFirstParam=a;
RadComboBoxGlobalSecondParam=b;
RadComboBoxGlobalThirdParam=c;
var s=_b8;
s=s+"(RadComboBoxGlobalFirstParam";
s=s+",RadComboBoxGlobalSecondParam";
s=s+",RadComboBoxGlobalThirdParam";
s=s+");";
return eval(s);
};
RadComboBox.prototype.HandleEvent=function(_bd,_be){
var _bf;
var _c0=(document.all)?_be.srcElement:_be.target;
var _c1=this.HtmlElementToItem(_c0);
if(_c1!=null){
_bf=this.ItemToInstance(_c1);
}
if(!this.Enabled){
return;
}
switch(_bd){
case "showdropdown":
this.CancelPropagation(_be);
this.ShowDropDown();
break;
case "hidedropdown":
this.CancelPropagation(_be);
this.HideDropDown();
break;
case "toggledropdown":
this.CancelPropagation(_be);
this.ToggleDropDown();
break;
case "mouseover":
if(_bf!=null){
this.HandleMouseOver(_bf);
}
break;
case "mouseout":
if(_bf!=null){
this.HandleMouseOut(_bf);
}
break;
case "keypress":
this.HandleKeyPress(this,_be);
break;
case "keydown":
this.HandleKeyDown(_be);
break;
case "click":
this.HandleClick(_be);
break;
case "inputclick":
this.CancelPropagation(_be);
document.getElementById(this.InputID).select();
if(this.ShowDropDownOnTextboxClick){
this.ShowDropDown();
}
break;
case "inputimageout":
this.HandleInputImageOut();
break;
case "inputimagehover":
this.HandleInputImageHover();
break;
case "moreresultsimageclick":
this.CancelPropagation(_be);
this.HandleMoreResultsImageClick();
break;
case "moreresultsimagehover":
this.HandleMoreResultsImageHover();
break;
case "moreresultsimageout":
this.HandleMoreResultsImageOut();
break;
}
};
RadComboBox.prototype.Enable=function(){
document.getElementById(this.InputID).disabled=false;
this.Enabled=true;
};
RadComboBox.prototype.Disable=function(){
document.getElementById(this.InputID).disabled="disabled";
this.Enabled=false;
this.TextHidden.value=this.GetText();
};
RadComboBox.prototype.FixUp=function(_c2,_c3){
if((this.ClientWidthHidden.value!="")&&(this.ClientHeightHidden.value!="")){
if(_c2.style.width!=this.ClientWidthHidden.value){
_c2.style.width=this.ClientWidthHidden.value;
}
if(_c2.style.height!=this.ClientHeightHidden.value){
_c2.style.height=this.ClientHeightHidden.value;
}
this.ShowWrapperElement();
return;
}
var _c4=_c2.parentNode.getElementsByTagName("img")[0];
if(_c3&&_c4&&(_c4.offsetWidth==0)){
var _c5=this;
if(document.attachEvent){
if(document.readyState=="complete"){
window.setTimeout(function(){
_c5.FixUp(_c2,false);
},100);
}else{
window.attachEvent("onload",function(){
_c5.FixUp(_c2,false);
});
}
}else{
window.addEventListener("load",function(){
_c5.FixUp(_c2,false);
},false);
}
return;
}
var _c6=null;
if(_c2.currentStyle){
_c6=_c2.currentStyle;
}else{
if(document.defaultView&&document.defaultView.getComputedStyle){
_c6=document.defaultView.getComputedStyle(_c2,null);
}
}
if(_c6==null){
this.ShowWrapperElement();
return;
}
var _c7=parseInt(_c6.height);
var _c8=parseInt(_c2.offsetWidth);
var _c9=parseInt(_c6.paddingTop);
var _ca=parseInt(_c6.paddingBottom);
var _cb=parseInt(_c6.paddingLeft);
var _cc=parseInt(_c6.paddingRight);
var _cd=parseInt(_c6.borderTopWidth);
if(isNaN(_cd)){
_cd=0;
}
var _ce=parseInt(_c6.borderBottomWidth);
if(isNaN(_ce)){
_ce=0;
}
var _cf=parseInt(_c6.borderLeftWidth);
if(isNaN(_cf)){
_cf=0;
}
var _d0=parseInt(_c6.borderRightWidth);
if(isNaN(_d0)){
_d0=0;
}
if(document.compatMode&&document.compatMode=="CSS1Compat"){
if(!isNaN(_c7)&&(this.ClientHeightHidden.value=="")){
_c2.style.height=_c7-_c9-_ca-_cd-_ce+"px";
this.ClientHeightHidden.value=_c2.style.height;
}
}
if(!isNaN(_c8)&&_c8&&(this.ClientWidthHidden.value=="")){
var _d1=0;
if(_c4){
_d1=_c4.offsetWidth;
}
if(document.compatMode&&document.compatMode=="CSS1Compat"){
var _d2=_c8-_d1-_cb-_cc-_cf-_d0;
if(_d2>=0){
_c2.style.width=_d2+"px";
}
this.ClientWidthHidden.value=_c2.style.width;
}else{
_c2.style.width=_c8-_d1;
}
}
this.ShowWrapperElement();
};
RadComboBox.prototype.ShowWrapperElement=function(){
if(!this.ShowWhileLoading){
document.getElementById(this.ClientID+"_wrapper").style.visibility="visible";
}
};
function rcbDispatcher(_d3,_d4,_d5){
var _d6=null;
try{
_d6=window[_d3];
}
catch(e){
}
if(typeof (_d6)=="undefined"||_d6==null){
return;
}
if(typeof (_d6.HandleEvent)!="undefined"){
_d6.HandleEvent(_d4,_d5);
}
}
function rcbAppendStyleSheet(_d7,_d8){
var _d9=(navigator.appName=="Microsoft Internet Explorer")&&((navigator.userAgent.toLowerCase().indexOf("mac")!=-1)||(navigator.appVersion.toLowerCase().indexOf("mac")!=-1));
var _da=(navigator.userAgent.toLowerCase().indexOf("safari")!=-1);
if(_d9||_da){
document.write("<"+"link"+" rel='stylesheet' type='text/css' href='"+_d8+"'>");
}else{
var _db=document.createElement("LINK");
_db.rel="stylesheet";
_db.type="text/css";
_db.href=_d8;
document.getElementById(_d7+"StyleSheetHolder").appendChild(_db);
}
}

//BEGIN_ATLAS_NOTIFY
if (typeof(Sys) != "undefined"){if (Sys.Application != null && Sys.Application.notifyScriptLoaded != null){Sys.Application.notifyScriptLoaded();}}
//END_ATLAS_NOTIFY
