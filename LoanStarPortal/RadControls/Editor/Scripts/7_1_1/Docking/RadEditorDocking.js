RadEditorNamespace.O97= {l97:null,i97: [],I97: [],o98:function (i6,O98,l98,i98,I98,o99){if (!i6)return; this.O99(i6,(null==O98? true :O98),(null==l98? true :l98),i98,I98,o99); this.l99(i6); } ,l99:function (i99){if (i99.I99)return; var o9a; var O9a=i99.getAttribute("d\x6fck\x69\x6egzon\x65"); if (O9a){o9a=document.getElementById(O9a); }else if (i99.parentNode.getAttribute("\x64\x6fcking")){o9a=i99.parentNode; }if (o9a && typeof(o9a.l9a)=="\x66unctio\x6e"){var i9a=parseInt(i99.getAttribute("\x64oc\x6b\x69ngorde\x72")); if (isNaN(i9a)){i9a=null; }o9a.l9a(i99,i9a); }else if (i99.parentNode!=document.body){if ("\x63omplet\x65"==document.readyState){i99.parentNode.removeChild(i99); document.body.appendChild(i99); }if (i99.I9a){i99.I9a(); }}} ,o9b: false ,O9b:function (l9b){if (l9b){var i9b=this.I9b(); if (i9b){i9b.src=l9b; }}if (this.o9b){return; }RadEditorNamespace.Utils.l1b(document,"onmous\x65\x6dove",RadEditorNamespace.O97.o9c); RadEditorNamespace.Utils.l1b(document,"\x6fnkeydown",RadEditorNamespace.O97.O9c); this.o9b= true; }} ; RadEditorNamespace.O97.o9c= function (e){if (!RadEditorNamespace.O97 || !RadEditorNamespace.O97.l9c)return; if (RadEditorNamespace.O97.l9c.i9c)return; if (!e){e=window.event; }RadEditorNamespace.O97.l9c.I9c(e); if (RadEditorNamespace.O97.l9c.o9d()){if (null==RadEditorNamespace.O97.l9c.I99 || RadEditorNamespace.O97.l9c.O9d){RadEditorNamespace.O97.l97=null; var o9a; var l9d=RadEditorNamespace.O97.l18; for (var i=0; i<l9d.length; i++){o9a=l9d[i]; if (o9a.HitTest(RadEditorNamespace.O97.l9c,null==RadEditorNamespace.O97.l97,e)){if (!RadEditorNamespace.O97.l97){RadEditorNamespace.O97.l97=o9a; }}}}else if (!RadEditorNamespace.O97.l9c.O9d){RadEditorNamespace.O97.l9c.i9d(e); }}return RadEditorNamespace.Utils.o6w(e); } ; RadEditorNamespace.O97.O9c= function (e){if (!RadEditorNamespace.O97.l9c)return; if (!e){e=window.event; }if (27==e.keyCode){if (l97){RadEditorNamespace.O97.l97.HitTest(RadEditorNamespace.O97.l9c, false ,e); RadEditorNamespace.O97.l97=null; }RadEditorNamespace.O97.l9c.I9d(e); }} ;;RadEditorNamespace.O97.o9e= function (o8,O9e,l9e,i9e,I9e,o9f,O9f){var doc=document; var i1l=doc.createElement("\x74\x61\x62le"); i1l.border=0; i1l.cellSpacing=0; i1l.cellPadding=0; i1l.setAttribute("unsel\x65\x63tabl\x65","on"); i1l.setAttribute("\144\x6f\x63kable","\x61ll"); var O2m=i1l.insertRow(-1); var I1l=O2m.insertCell(-1); var l9f=doc.createElement("\x73\x70an"); l9f.className="\x52\141dAu\x74\x6fDock\x42\x75tt\x6fn"; l9f.innerHTML="&nbsp;&nb\x73\x70;&nb\x73\x70;"; l9f.setAttribute("autoDo\x63\x6b","true"); I1l.appendChild(l9f); I1l.innerHTML+=(O9f?"\x26nbsp\x3b"+O9f: ""); I1l.colSpan=2; I1l.setAttribute("noWrap","true"); I1l.setAttribute("titleGri\x70","autohid\x65"); I1l.className="\x52adETitleGr\x69\x70"; I1l.parentNode.style.display="\x6eone"; var O2m=i1l.insertRow(-1); I1l=O2m.insertCell(-1); I1l.innerHTML="&nbs\x70\x3b"; I1l.colSpan=2; I1l.setAttribute("\x74\157pSi\x64\x65Grip","autohi\x64\x65"); I1l.className="Rad\x45\x53ideGri\x70\x56erti\x63\141l"; O2m=i1l.insertRow(-1); I1l=O2m.insertCell(-1); I1l.innerHTML="&nbs\x70\x3b&nbsp\x3b\x26nbsp\x3b"; I1l.setAttribute("leftSideGri\x70","autohide"); I1l.className="\x52adESideGrip\x48\x6frizo\x6e\x74al"; I1l=O2m.insertCell(-1); I1l.appendChild(o8); i1l.i9f=l9e; i1l.I9f=i9e; i1l.o9g=I9e; i1l.O9g=o9f; var l9g=document.all && !window.opera?"\x69nline": ""; i1l.setAttribute("display",l9g); if (document.all && !window.opera)i1l.style.display="\x69nline"; else i1l.setAttribute("styl\x65","flo\x61\x74:left"); return i1l; } ; RadEditorNamespace.O97.i9g= function (){try {var I9g=RadEditorNamespace.O97.o9h.length; for (var I8i=0; I8i<I9g; I8i++){var Z=RadEditorNamespace.O97.o9h[I8i]; Z.I99=null; Z.O9h=null; var l9h=RadEditorNamespace.Utils.i9h(Z,"\x61utodock", true); for (var i=0; i<l9h.length; i++){l9h[i].I9h=null; l9h[i].onclick=null; }var o9i=[Z.rows[0].cells[0],Z.rows[1].cells[0],Z.rows[2].cells[0]]; for (var i=0; i<o9i.length; i++){O9i=o9i[i]; if (O9i){O9i.style.display=""; O9i.parentNode.deleteCell(O9i); }}Z.onmousemove=null; Z.onmouseout=null; Z.onmousedown=null; Z.l9i=null; Z.i9i=null; Z.Title=null; Z.i9f=null; Z.I9f=null; Z.o9g=null; Z.O9g=null; Z.I9i=null; Z.I9d=null; Z.O16=null; Z.o9j=null; Z.O9j=null; Z.I1a=null; Z.l9j=null; Z.i9j=null; Z.I9j=null; Z.Initialize=null; Z.o9k=null; Z.o9d=null; Z.O9k=null; Z.l9k=null; Z.IsVisible=null; Z.i9k=null; Z.I9k=null; Z.o9l=null; Z.O9l=null; Z.l9l=null; Z.i9l=null; Z.I9l=null; Z.SetPosition=null; Z.SetSize=null; Z.o9m=null; Z.O9m=null; Z.I9a=null; Z.l9m=null; Z.i9d=null; Z.i9m=null; }RadEditorNamespace.O97.o9h=null; }catch (e){}} ; RadEditorNamespace.O97.o9h=[]; RadEditorNamespace.O97.O99= function (Z,O98,l98,i98,I98,o99){if (!Z || Z.i9d)return; this.I9m(Z,O98,l98,i98,( true ==o99),o99); RadEditorNamespace.Utils.w(Z,RadEditorNamespace.O97.o9n); Z.O9d=O98; if (!Z.I9f){var o7=Z.getAttribute("rend\x65rVerti\x63\x61l"); Z.I9f=(o7?o7:RadEditorNamespace.O97.I9f); }if (!Z.i9f){var o7=Z.getAttribute("\x72enderHo\x72\x69zont\x61\x6c"); Z.i9f=(o7?o7:RadEditorNamespace.O97.i9f); }Z.I99=null; if (Z.Initialize){Z.Initialize(); }var O9n=RadEditorNamespace.O97.o9h; O9n[O9n.length]=Z; } ; RadEditorNamespace.O97.o9n= {l9n:null,i9n:null,O9d: true ,O9l:function (e){if (this.O9d){ this.i9d(e); }if (RadEditorNamespace.O97.l97){RadEditorNamespace.O97.l97.l9a(this ); RadEditorNamespace.O97.l97=null; }} ,I9i:function (o9a){var I9n=this.getAttribute("docka\x62le"); if ("\x73tring"==typeof(I9n)){I9n=I9n.toLowerCase(); }if ("\x61\x6cl"==I9n){return true; }else {return (I9n==o9a.o9o.toLowerCase()); }} ,i9l:function (){if (this.I9a && !this.o9k()){ this.I9a(); }} ,O9o:function (){ this.O9h=this.I99; this.l9o= false; if (document.all && "none"!=this.style.display){ this.style.display="in\x6c\x69ne"; }if (this.I9j){ this.I9j(); } this.O9m(this.Title, false); this.O9m(this.l9i,!this.i9o); this.O9m(this.i9i,this.i9o); this.O9j(); if (this.l9n){ this.l9n(); }} ,i9d:function (e){if (!this.I99)return; this.I99=null; this.l9o= true; this.parentNode.removeChild(this ); this.style.position="a\x62\x73olute"; this.O9m(this.Title, true); this.O9m(this.l9i, false); this.O9m(this.i9i, false); document.body.appendChild(this ); this.I9l(); if (this.I9a){ this.I9a(); }if (this.i9n){ this.i9n(); }} ,O16:function (){if (!this.O9h)return; this.O9h.l9a(this ); } ,o9k:function (){return (null!=this.I99); } ,O9j:function (){if (null!=this.I99 && RadEditorNamespace.Utils.I6v(this.I99.o9o,"vert") && null!=this.I9f){if (this.i9o)return; try {if (this.i9i)this.i9i.style.display=""; if (this.l9i)this.l9i.style.display="\x6eone"; this.className=this.O9g; if (typeof(this.I9f)=="\x66\165\x6e\x63tion"){ this.I9f(); }else if (typeof(this.I9f)=="\x73\x74ring"){eval(this.I9f); }}catch (ex){} this.i9o= true; }else if (this.i9o && null!=this.i9f){try {if (this.i9i)this.i9i.style.display="\x6eone"; if (this.l9i)this.l9i.style.display=""; this.className=this.o9g; if (typeof(this.i9f)=="\x66unction"){ this.i9f(); }else if (typeof(this.i9f)=="s\x74\x72ing"){eval(this.i9f); }}catch (ex){} this.i9o= false; }} ,l9j:function (e){var source=RadEditorNamespace.Utils.o6x(e); return (null!=source && (null!=source.getAttribute("grip") || null!=source.getAttribute("titlegrip") || null!=source.getAttribute("to\x70\163i\x64\x65grip") || null!=source.getAttribute("\x6c\x65ftside\x67\x72ip"))); } ,Initialize:function (){var l9h=RadEditorNamespace.Utils.i9h(this,"leftSide\x47\x72ip", true); if (l9h.length>0){ this.l9i=l9h[0]; this.l9i.I9o=(l9h[0].getAttribute("\154e\x66\x74SideG\x72\x69p").toLowerCase()=="visible"); }l9h=RadEditorNamespace.Utils.i9h(this,"\x74opSideGr\x69\x70", true); if (l9h.length>0){ this.i9i=l9h[0]; this.i9i.I9o=(l9h[0].getAttribute("\x74\x6fpSideG\x72\x69p").toLowerCase()=="\x76isible"); }l9h=RadEditorNamespace.Utils.i9h(this,"\x74\x69tleGri\x70", true); if (l9h.length>0){ this.Title=l9h[0]; var o9p=(l9h[0].getAttribute("\x74\x69tleGri\x70").toLowerCase()=="\x76isible"); if (this.Title.tagName=="\x54D" || this.Title.tagName=="\x54H"){ this.Title=this.Title.parentNode; } this.Title.I9o=o9p; } this.O9m(this.Title, true); this.O9m(this.l9i, false); this.O9m(this.i9i, false); l9h=RadEditorNamespace.Utils.i9h(this,"autodock", true); for (var i=0; i<l9h.length; i++){l9h[i].I9h=this ; l9h[i].onclick= function (){ this.I9h.O16(); } ; }} ,O9m:function (O9p,O4f){if (O9p && !O9p.I9o){O9p.style.display=O4f?"": "\x6eone"; }}} ;;RadEditorNamespace.O97.O6v= function (left,top,width,height){ this.left=(null!=left?left: 0); this.top=(null!=top?top: 0); this.width=(null!=width?width: 0); this.height=(null!=height?height: 0); this.right=left+width; this.bottom=top+height; };RadEditorNamespace.O97.O6v.prototype.v= function (){return new RadEditorNamespace.O97.O6v(this.left,this.top,this.width,this.height); } ; RadEditorNamespace.O97.O6v.prototype.l9p= function (x,y){return (this.left<=x && x<=(this.left+this.width) && this.top<=y && y<=(this.top+this.height)); } ; RadEditorNamespace.O97.O6v.prototype.i9p= function (I9p){if (null==I9p)return false; if (this ==I9p)return true; return (I9p.left<this.right && I9p.top<this.bottom && I9p.right>this.left && I9p.bottom>this.top); } ; RadEditorNamespace.O97.O6v.prototype.ToString= function (){return "le\x66t:"+this.left+" "+"right:"+this.right+"\x20"+"\x74op:"+this.top+" "+"\x62ottom\x3a"+this.bottom+"\x20"+"\x28"+this.width+"\x20x "+this.height+"\x29"; } ; RadEditorNamespace.O97.O6v.prototype.o9q= function (I9p){if (null==I9p)return false; if (this ==I9p)return this.v(); if (!this.i9p(I9p))return new RadEditorNamespace.O97.O6v(); var left=Math.max(this.left,I9p.left); var top=Math.max(this.top,I9p.top); var right=Math.min(this.right,I9p.right); var bottom=Math.min(this.bottom,I9p.bottom); return new RadEditorNamespace.O97.O6v(left,right,right-left,bottom-top); } ; RadEditorNamespace.O97.O9q= function (i6){if (!i6){i6=this ; }var left=0; var top=0; var width=i6.offsetWidth; var height=i6.offsetHeight; while (i6.offsetParent){left+=i6.offsetLeft; top+=i6.offsetTop; i6=i6.offsetParent; }if (i6.x)left=i6.x; if (i6.y)top=i6.y; left=RadEditorNamespace.Utils.o6v(left,0); top=RadEditorNamespace.Utils.o6v(top,0); width=RadEditorNamespace.Utils.o6v(width,0); height=RadEditorNamespace.Utils.o6v(height,0); return new RadEditorNamespace.O97.O6v(left,top,width,height); };RadEditorNamespace.O97.l9q= function (){if (document.documentElement && document.documentElement.scrollTop){return document.documentElement.scrollTop; }else {return document.body.scrollTop; }} ; RadEditorNamespace.O97.i9q= function (){if (document.documentElement && document.documentElement.scrollLeft){return document.documentElement.scrollLeft; }else {return document.body.scrollLeft; }} ;;RadEditorNamespace.O97.l18=[]; RadEditorNamespace.O97.I9q= function (Z,o9r){if (!Z)return; RadEditorNamespace.Utils.w(Z,RadEditorNamespace.O97.I99); if (!o9r){o9r=Z.getAttribute("\144ocking"); }Z.o9o=(o9r?o9r: "horiz"); RadEditorNamespace.O97.l18.push(Z); };RadEditorNamespace.O97.I99= {l9a:function (O9r,i9a){if (this ==O9r.I99)return; if (null==O9r.getAttribute("d\x6fckable")){alert("\x45rror: You \x61\x72e t\x72\x79i\x6e\x67 to\x20\x64ock\x20\156\x6fn-d\x6f\143k\x61\142l\x65\040\x6fbjec\x74"); return; }if (!O9r.I9i(this )){alert("\x45rror: Yo\x75\x20are\x20\x6eot \x61\154l\x6f\x77ed\x20\x74o \x64\157c\x6b \047"+O9r.id+"\x27 to \047"+this.id+"\047\x20\x64ockin\x67\040z\x6f\x6ee"); return; }O9r.I99=this ; O9r.parentNode.removeChild(O9r); O9r.style.position=""; var insertBeforeObject; if (null!=i9a){insertBeforeObject=this.l9r(i9a); }else {insertBeforeObject=(this.i9r!=this ?this.i9r:null); }if (insertBeforeObject){ this.insertBefore(O9r,insertBeforeObject); }else { this.appendChild(O9r); } this.I9r(this.i9r, false); this.i9r=null; O9r.O9o(); } ,HitTest:function (O9r,o9s,O6w){if (!O9r.I9i(this ))return false; if (null==o9s)o9s= true; var O9s=O9r.I1a(); var l9s=this.I1a(); var i9s=RadEditorNamespace.O97.i9q(); var I9s=RadEditorNamespace.O97.l9q(); var o9t=O6w.clientX+i9s; var O9t=O6w.clientY+I9s; var l9t=this.I1a().l9p(o9t,O9t); this.i9r=null; var node; for (var i=0; i<this.childNodes.length; i++){node=this.childNodes[i]; if (1!=node.nodeType)continue; if (!node.I99)continue; if (node==O9r)continue; if (!this.i9r && l9t && node.I1a().l9p(o9t,O9t)){ this.i9r=node; } this.I9r(node,o9s && node==this.i9r); }if (!this.i9r){ this.i9r=(l9t?this :null); } this.I9r(this,o9s && this ==this.i9r); return l9t; } ,I9r:function (i6,o9s){if (!i6)return; if (o9s && null==i6.i9t){i6.i9t=i6.style.cssText; i6.style.border="1px \x64ashed\x20#666666"; }else if (!o9s && null!=i6.i9t){i6.style.cssText=i6.i9t; i6.i9t=null; }} ,l9r:function (i9a){if (0<=i9a && i9a<this.childNodes.length){return this.childNodes[i9a]; }return null; } ,I1a:function (){return RadEditorNamespace.O97.O9q(this ); }} ;;RadEditor.prototype.I9t= function (I){var module; for (var j=0; j<this.i16.length; j++){module=this.i16[j]; if (module.Title==I){return module; }}return null; } ; RadEditor.prototype.o9u= function (){if (!this.O9u)return; var l9u=this.l18; for (var item in l9u){var i9u=l9u[item]; if (i9u && i9u.tagName!=null){RadEditorNamespace.O97.I9q(i9u); }}RadEditorNamespace.O97.O9b(this.I1u+"Buttons/t\x72ansp\x2e\147\x69\x66"); } ; RadEditor.prototype.I9u="\x52adEditorG\x6c\x6fbalS\x65\x72ia\x6c\x69zeC\x6f\x6fkie"; RadEditor.prototype.o9v= function (O9v,l9v){O9v="\x5b"+this.Id+O9v+"]"; var i9v=this.I9v(this.I9u); var o9w=""; var O9w=""; if (i9v){var array=i9v.split(O9v); if (array && array.length>1){o9w=array[0]; O9w=array[1].substr(array[1].indexOf("\x23")+1); }else O9w=i9v; }var l9w=new Date(); l9w.setFullYear(l9w.getFullYear()+10); document.cookie=this.I9u+"="+(o9w+O9v+"\055"+l9v+"#"+O9w)+";path\x3d\x2f;exp\x69\x72es="+l9w.toUTCString()+"\x3b"; } ; RadEditor.prototype.i9w= function (O9v){O9v="\x5b"+this.Id+O9v+"]"; var I9w=this.I9v(this.I9u); if (!I9w)return null; var l9v=null; var index=I9w.indexOf(O9v); if (index>=0){var o9x=index+O9v.length+1; l9v=I9w.substring(o9x,I9w.indexOf("#",o9x)); }return l9v; } ; RadEditor.prototype.I9v= function (O9v){var O9x=document.cookie.split("; "); for (var i=0; i<O9x.length; i++){var l9x=O9x[i].split("="); if (O9v==l9x[0])return l9x[1]; }return null; } ; RadEditor.prototype.i9x= function (I9x){if (!this.o9y || !this.O9u)return; if (I9x){if (this.l14(RadEditorNamespace.i14.O9y)){var o47="\133"; var l9y= false; var toolbar; var i9y=this.I15(); for (var i=0; i<i9y.length; i++){toolbar=i9y[i]; var O9f=escape(toolbar.getAttribute("title")); var I9y=this.o9z(toolbar,O9f, true); if (I9y){if (l9y){o47+=","; }o47+=I9y; l9y= true; }}o47+="\x5d"; this.o9v("\x54oolbar\x73",o47); }o47="["; l9y= false; var module; for (var i=0; i<this.i16.length; i++){module=this.i16[i]; var I9y=this.o9z(module.I16(),module.Title,module.IsEnabled); if (I9y){if (l9y){o47+=","; }o47+=I9y; l9y= true; }}o47+="]"; this.o9v("\x4dodules",o47); }else {if (this.l14(RadEditorNamespace.i14.O9y)){var o47=this.i9w("\x54\x6folbars"); if (null!=o47){var O9z,toolbar; var i15=this.I15(); var l9z=eval(o47); for (var i=0; i<l9z.length; i++){O9z=l9z[i]; var O9f=unescape(O9z[0]); toolbar=this.i9z(i15,O9f); if (!toolbar)continue; this.I9z(toolbar,O9z); }}}var o47=this.i9w("Modu\x6c\x65s"); if (null!=o47){var O9z,module; var l9z=eval(o47); for (var i=0; i<l9z.length; i++){O9z=l9z[i]; module=this.I9t(O9z[0]); if (!module)continue; var i3o=this.I9z(module.I16(),O9z); module.I13(i3o); }}}} ; RadEditor.prototype.i9z= function (i15,I){for (var j=0; j<i15.length; j++){var toolbar=i15[j]; if (toolbar.getAttribute("title")==I){return toolbar; }}return null; } ; RadEditor.prototype.o9z= function (i99,title,i3o){if (!i99 || !i99.i9d)return null; var o9a=i99.I99; var o47="["; o47+="\047"+title+"\047"; var oa0=( false !=i3o)? true : false; o47+="\x2c"+oa0; var Oa0=o9a?o9a.id: ""; if (!o9a && i99.la0)Oa0=i99.la0; o47+=","+"\x27"+(Oa0)+"\x27"; if (null!=o9a){for (var j=0; j<o9a.childNodes.length; j++){if (i99==o9a.childNodes[j]){o47+=("\x2c"+j); break; }}}else {o47+="\x2c"; var ia0=i99.I1a(); o47+=RadEditorNamespace.Utils.Ia0("[{0},{1}]",ia0.left,ia0.top); }o47+="\x5d"; return o47; } ; RadEditor.prototype.I9z= function (i99,O9z){if (!i99 || !i99.i9d)return null; var title=O9z[0]; var i3o=O9z[1]; var O9a=O9z[2]; var i9a=null; var left=null; var top=null; if (O9a){i9a=O9z[3]; }else {left=O9z[3][0]; top=O9z[3][1]; }if ( false ==i3o)i99.i9j(); else i99.o9m(); if (""==O9a){i99.i9d(); i99.oa1(left,top); }else if (null!=(o9a=document.getElementById(O9a)) && null!=o9a.l9a){o9a.l9a(i99,i9a); }return i3o; } ;;RadEditorNamespace.O97.l9c=null; RadEditorNamespace.O97.I9m= function (Z,O98,l98,i98,I98,o99){if (!Z || Z.l6i)return; RadEditorNamespace.Utils.w(Z,RadEditorNamespace.O97.Oa1); if (i98!= false){RadEditorNamespace.Utils.w(Z,RadEditorNamespace.O97.la1); Z.ia1(); }Z.onmouseout= function (e){if (""!=this.style.cursor){ this.style.cursor=""; }} ; Z.onmousedown= function (e){if (!e){e=window.event; }if (document.all && !window.opera && e.button!=1){return; }if (this.I9l)this.I9l(); this.DragMode=""; if (this.Ia1 && this.oa2){ this.DragMode="\x72e\x73\x69ze"; }else if (this.Oa2 && this.l9j(e)){ this.DragMode="move"; }if (""!=this.DragMode){ this.l9m(e); }RadEditorNamespace.Utils.o6w(e); return false; } ; Z.onmousemove= function (e){if (!e){e=window.event; }if (!this.l9k() && null!=this.la2){ this.oa2=this.la2(e); this.style.cursor=this.oa2; }if (!this.oa2 && this.l9j(e)){ this.style.cursor="m\x6f\x76e"; }} ; var ia2=navigator.userAgent.toLowerCase(); if (l98!= false && null!=document.all && ia2.indexOf("msie 7.0")==-1){ this.Ia2(Z); }Z.oa3=(O98!= false); Z.i9c=( false !=I98); } ; RadEditorNamespace.O97.Oa1= {OnDragStart:null,O9l:null,Oa2: true ,Ia1: true ,oa3: true ,i9c: true ,l9m:function (O6w){ this.Oa3=O6w.clientX; this.la3=O6w.clientY; RadEditorNamespace.Utils.l1b(document,"\x6fnmouseu\x70",RadEditorNamespace.O97.ia3); if (this.i9c){RadEditorNamespace.Utils.l1b(document,"onmo\x75\x73emove",RadEditorNamespace.O97.Ia3); RadEditorNamespace.Utils.l1b(document,"\x6fnkeydo\x77\x6e",RadEditorNamespace.O97.oa4); }RadEditorNamespace.O97.l9c=this ; if (this.oa3){ this.Oa4=RadEditorNamespace.O97.la4(); this.Oa4.o9m(this.I1a()); }if (this.OnDragStart){ this.OnDragStart(O6w); }RadEditorNamespace.O97.ia4(this ); window.status="Hit Esc to\x20\x63anc\x65\x6c"; } ,o9j:function (O6w){if (this.Oa4){var ia0=this.Oa4.I1a(); this.oa1(ia0.left,ia0.top); if ("resize"==this.DragMode){ this.SetSize(ia0.width,ia0.height); }} this.I9d(O6w); if (this.O9l){ this.O9l(O6w); }} ,I9d:function (O6w){RadEditorNamespace.O97.l9c=null; RadEditorNamespace.O97.Ia4(); RadEditorNamespace.Utils.o18(document,"\x6f\x6emouseup",RadEditorNamespace.O97.ia3); if (this.i9c){RadEditorNamespace.Utils.o18(document,"\x6fnmousemove",RadEditorNamespace.O97.Ia3); RadEditorNamespace.Utils.o18(document,"\x6f\156\x6b\x65ydown",RadEditorNamespace.O97.oa4); }if (this.Oa4){ this.Oa4.i9j(); this.Oa4=null; } this.DragMode=""; window.status=""; if (this.oa5){ this.oa5.i9j(); }} ,I9c:function (O6w){switch (this.DragMode){case "\x6d\x6fve": this.l6i(O6w); break; case "\x72esize": this.i6j(O6w); break; } this.Oa3=O6w.clientX; this.la3=O6w.clientY; } ,l9j:function (O6w){var source=RadEditorNamespace.Utils.o6x(O6w); return (null!=source && null!=source.getAttribute("\147\x72ip")); } ,l6i:function (O6w){var Oa5=O6w.clientX-this.Oa3; var la5=O6w.clientY-this.la3; if (this.Oa4){ this.Oa4.ia5(Oa5,la5); }else { this.ia5(Oa5,la5); }} ,ia5:function (Oa5,la5){if (!this.Ia5){ this.Ia5=parseInt(this.style.left); }if (!this.Top){ this.Top=parseInt(this.style.top); } this.oa1(this.Ia5+Oa5,this.Top+la5); } ,oa1:function (x,y){ this.Ia5=x; this.Top=y; this.style.position="\x61bsolut\x65"; this.style.left=this.Ia5+"\x70x"; this.style.top=this.Top+"px"; if (this.oa6){ this.Oa6(); this.oa6= false; }if (this.i9m){if (this.i9m.style.display=="none"){} this.i9m.style.top=this.style.top; this.i9m.style.left=this.style.left; }} ,SetSize:function (width,height){width=parseInt(width); if (!isNaN(width) && width>=0){ this.style.width=width+"\x70x"; if (this.i9m){ this.i9m.style.width=width+"\x70x"; }}height=parseInt(height); if (!isNaN(height) && height>=0){ this.style.height=height+"px"; if (this.i9m){ this.i9m.style.height=height+"px"; }}if (this.la6 && "f\x75\x6ection"==typeof(this.la6))this.la6(); } ,I1a:function (){if (this ==RadEditorNamespace.O97.l9c && this.Oa4 && this.Oa4.IsVisible()){return RadEditorNamespace.O97.O9q(this.Oa4); }else {return RadEditorNamespace.O97.O9q(this ); }} ,SetPosition:function (I9p){if (I9p){ this.oa1(I9p.left,I9p.top); this.SetSize(I9p.width,I9p.height); }} ,I9l:function (){var ia6=0; var zIndex=0; var Ia6=this.parentNode.childNodes; var node; for (var i=0; i<Ia6.length; i++){node=Ia6[i]; if (1!=node.nodeType)continue; zIndex=parseInt(node.style.zIndex); if (zIndex>ia6){ia6=zIndex; }} this.style.zIndex=ia6+1; } ,o9m:function (I9p){if (this.IsVisible())return; this.style.display=this.oa7?this.oa7: ""; if (null!=I9p){ this.SetPosition(I9p); } this.I9l(); if (this.i9l){ this.i9l(); }} ,i9j:function (){if (!this.IsVisible())return; this.oa7=this.style.display; this.style.display="n\x6fne"; if (this.l9l){ this.l9l(); }} ,i9l:function (){if (this.I9a){ this.I9a(); }} ,l9l:function (){if (this.I9j){ this.I9j(); }} ,IsVisible:function (){return (this.style.display!="\x6e\x6fne"); } ,l9k:function (){return ("\x72esize"==this.DragMode); } ,o9d:function (){return ("\x6d\x6fve"==this.DragMode); }} ; RadEditorNamespace.O97.ia3= function (O6w){if (!RadEditorNamespace.O97.l9c)return; if (!O6w){O6w=window.event; }RadEditorNamespace.O97.l9c.o9j(O6w); } ; RadEditorNamespace.O97.Ia3= function (O6w){if (!RadEditorNamespace.O97.l9c)return; if (!O6w){O6w=window.event; }RadEditorNamespace.O97.l9c.I9c(O6w); RadEditorNamespace.Utils.o6w(O6w); } ; RadEditorNamespace.O97.oa4= function (O6w){if (!RadEditorNamespace.O97.l9c)return; if (!O6w){O6w=window.event; }if (27==O6w.keyCode){RadEditorNamespace.O97.l9c.I9d(O6w); }} ; RadEditorNamespace.O97.Oa7=null; RadEditorNamespace.O97.la4= function (){if (RadEditorNamespace.O97.Oa7){return RadEditorNamespace.O97.Oa7; }var la7=document.createElement("\x44IV"); document.body.appendChild(la7); la7.setAttribute("\163\x74\x79le","-moz-opacit\x79\x3a0.3"); la7.style.border="1px dashed \x67\x72ay"; la7.style.backgroundColor="\x23cccccc"; la7.style.filter="\x70rogid:DXIma\x67\x65Tra\x6e\x73fo\x72m.Micr\x6f\163o\x66t.Alp\x68a(opa\x63\151t\x79=50)"; la7.style.margin="0\x70\x78 0px 0\x70\x78 0px"; la7.style.padding="0px"; la7.style.position="absolute"; la7.style.top=10; la7.style.left=10; la7.style.width=100; la7.style.height=100; la7.style.zIndex=50000; la7.style.overflow="\x68idden"; la7.style.display="non\x65"; RadEditorNamespace.O97.I9m(la7, false , false , true); RadEditorNamespace.O97.Oa7=la7; return la7; } ; RadEditorNamespace.O97.Ia2= function (Z){Z.Oa6= function (){var frm=document.createElement("\x49FRAM\x45"); frm.src="javascript:\x66\x61lse"; frm.frameBorder=0; frm.scrolling="\x6eo"; frm.style.overflow="hi\x64\x64en"; frm.style.display="\x69nline"; frm.style.position="absolute"; try {var I9p=this.I1a(); frm.style.width=I9p.width; frm.style.height=I9p.height; frm.style.left=I9p.left; frm.style.top=I9p.top; }catch (ex){} this.parentNode.insertBefore(frm,this ); this.i9m=frm; } ; Z.I9a= function (){if (this.i9m){ this.parentNode.insertBefore(this.i9m,this ); this.i9m.style.display="\x69nline"; this.i9m.style.position="absolute"; var I9p=this.I1a(); this.i9m.style.width=I9p.width; this.i9m.style.height=I9p.height; this.i9m.style.left=I9p.left; this.i9m.style.top=I9p.top; }} ; Z.I9j= function (){if (null!=this.i9m && null!=this.i9m.parentNode){ this.i9m.parentNode.removeChild(this.i9m); this.i9m.style.display="no\x6e\x65"; }} ; Z.O9k= function (){return (this.i9m && this.i9m.style.display!="\x6e\157n\x65"); } ; Z.oa6= true; } ; RadEditorNamespace.O97.ia7=null; RadEditorNamespace.O97.I9b= function (){if (!RadEditorNamespace.O97.ia7){var img=document.createElement("IMG"); img.style.display="none"; img.setAttribute("\x75nselect\x61\x62le","\x6f\x6e"); var Ia7= function (){return false; } ; img.onselectstart=Ia7; img.ondragstart=Ia7; img.onmouseover=Ia7; img.onmousemove=Ia7; RadEditorNamespace.O97.ia7=img; }return RadEditorNamespace.O97.ia7; } ; RadEditorNamespace.O97.ia4= function (insertBefore){var i9b=this.I9b(); if (i9b){document.body.appendChild(i9b); i9b.style.position="\141\x62\x73olute"; i9b.style.display=""; i9b.style.left=i9b.style.top="\x30\x70x"; i9b.style.width=parseInt(window.screen.width)-1; i9b.style.height=parseInt(window.screen.height)-1; }} ; RadEditorNamespace.O97.Ia4= function (){var i9b=this.I9b(); if (i9b){i9b.parentNode.removeChild(i9b); i9b.style.display="\x6eone"; }} ;;RadEditorNamespace.O97.oa8=5; RadEditorNamespace.O97.Oa8=5; RadEditorNamespace.O97.la1= {l9o: true ,la2:function (O6w,la8,ia8){if (!this.l9o)return ""; var Ia8=O6w.srcElement?O6w.srcElement:O6w.target; if (Ia8!=this )return ""; var ia0=this.I1a(); var oa9=""; if (null==la8)la8=RadEditorNamespace.O97.oa8; if (null==ia8)ia8=RadEditorNamespace.O97.Oa8; var offsetX,offsetY; if (null!=O6w.offsetY){offsetX=O6w.offsetX; offsetY=O6w.offsetY; }else if (null!=O6w.layerY){offsetX=O6w.layerX; offsetY=O6w.layerY; }if (offsetY<=ia8 && this.Oa9){oa9+="\156"; }else if ((ia0.height-offsetY)<=ia8 && this.la9){oa9+="s"; }if (offsetX<=la8 && this.ia9){oa9+="\x77"; }else if ((ia0.width-offsetX)<=la8 && this.Ia9){oa9+="\x65"; }return (""!=oa9?(oa9+"\x2dresize"): ""); } ,i6j:function (O6w){var Oa5=O6w.clientX-this.Oa3; var la5=O6w.clientY-this.la3; this.style.cursor=this.oa2; switch (this.oa2){case "\x6e-resize": this.oaa(0,la5,null,null); break; case "\x73-resi\x7a\x65": this.oaa(0,0,0,la5); break; case "w\x2d\x72esize": this.oaa(Oa5,0,null,null); break; case "\x65\x2dresize": this.oaa(0,0,Oa5,0); break; case "ne\x2d\x72esize": this.oaa(0,la5,Oa5,null); break; case "nw-resiz\x65": this.oaa(Oa5,la5,null,null); break; case "\x73e-resiz\x65": this.oaa(0,0,Oa5,la5); break; case "\x73w-resiz\x65": this.oaa(Oa5,0,null,la5); break; default:break; }} ,oaa:function (offsetLeft,offsetTop,offsetWidth,offsetHeight){var ia0=this.I1a(); var top=ia0.top+offsetTop; var left=ia0.left+offsetLeft; if (top<0){offsetTop=-ia0.top; }if (left<0){offsetLeft=-ia0.left; }top=ia0.top+offsetTop; left=ia0.left+offsetLeft; if (null==offsetWidth)offsetWidth=-offsetLeft; if (null==offsetHeight)offsetHeight=-offsetTop; var width=ia0.width+offsetWidth; var height=ia0.height+offsetHeight; width=Math.max(this.Oaa,width); width=Math.min(this.laa,width); height=Math.max(this.iaa,height); height=Math.min(this.Iaa,height); var oab=(this.Oa4?this.Oa4: this ); if (ia0.width!=width){oab.ia5(offsetLeft,0); oab.SetSize(width,null); }if (ia0.height!=height){oab.ia5(0,offsetTop); oab.SetSize(null,height); }} ,Oab:function (lab){ this.Oa9=(-1!=lab.indexOf("n")); this.la9=(-1!=lab.indexOf("\x73")); this.Ia9=(-1!=lab.indexOf("e")); this.ia9=(-1!=lab.indexOf("w")); } ,ia1:function (){var lab=this.getAttribute("\162\x65\163ize"); if ("\x73tring"==typeof(lab)){lab=lab.toLowerCase(); }else {lab="nsew"; } this.Oab(lab); this.Oaa=RadEditorNamespace.Utils.o6v(this.getAttribute("minWid\x74\x68")); this.laa=RadEditorNamespace.Utils.o6v(this.getAttribute("\x6daxWid\x74\x68"),100000); this.iaa=RadEditorNamespace.Utils.o6v(this.getAttribute("m\x69\x6eHeight")); this.Iaa=RadEditorNamespace.Utils.o6v(this.getAttribute("maxH\x65\x69ght"),100000); }} ;;//BEGIN_ATLAS_NOTIFY
if (typeof(Sys) != "undefined"){if (Sys.Application != null && Sys.Application.notifyScriptLoaded != null){Sys.Application.notifyScriptLoaded();}}
//END_ATLAS_NOTIFY