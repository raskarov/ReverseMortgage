var processing = 0;
var amountTotal = 0;
var isIe = navigator.userAgent.toLowerCase().indexOf('msie') > 0;
var isFF = navigator.userAgent.toLowerCase().indexOf('firefox/') > 0;
var showTime = 30;
function GetAmountValue(id) {
    var r = window[id];
    if (r == null) return 0;
    return r.GetValue();
}

function CalculateNTierUnit(o, ids, m, k) {
    var i = o.id.lastIndexOf('_text');
    var n = 1;
    if ((i > 0) && ((i + 5) == o.id.length)) {
        n = 2;
    }
    var id = GetClearId(o.id, n);
    if (id != '') {
        var a = ids.split('&');
        var a1 = GetControlsIds(id, a[0]);
        var a2 = GetControlsIds(id, a[1]);
        var a3 = GetControlsIds(id, a[2]);
        var av1 = GetSelectValues(a1);
        var av2 = GetRCTAmounts(a2);
        var t = CalculateNTiers(av1, av2, m, k);
        SetAmountLabel(a3[0], t);
        var i1 = av2.length - 3;
        if (av2[i1] != '') t = t - av2[i1];
        if (av2[i1 + 1] != '') t = t - av2[i1 + 1];
        if (av2[i + 2] != '') t = t - av2[i1 + 2];
        SetAmountTotal(a3[1], t);
    }
}
function CalculateNTiers(av1, av2, m, n) {
    var t = m;
    if (av1[0] == 2) {
        t *= 1.5;
    }
    var r = 0;
    var ut1 = 0;
    for (i = 0; i < n; i++) {
        var uf = av2[i * 3];;
        var pr = av2[i * 3 + 1];;
        var ut = t;
        if (i < (n - 1)) {
            ut = av2[i * 3 + 2];
        }
        if (t <= ut) {
            ut = t;
        }
        r += GetTierUnitFee(ut - ut1, pr, uf);
        ut1 = ut;
    }
    return r;
}
function GetHiddenValue(id, param) {
    var wm = GetRadWindowManager();
    var w = wm.GetWindowByName('GetHiddenValue');
    var url = 'GetHiddenValue.aspx?id=' + id + '&' + param;
    radopen(url, 'GetHiddenValue');
}
function HideValue(id) {
    var o = document.getElementById(id);
    var r = GetRadMTB(o.id);
    r.SetValue('');
    o.readOnly = true;
    r.ReadOnly = true;
    o.setAttribute('hidden', 1);
}
function ShowHiddenValue(id, val, msg) {
    if (msg != '') {
        alert(msg);
    } else {
        var o = document.getElementById(id);
        if (o != null) {
            o.setAttribute('hidden', 0);
            var e = o.getAttribute('editable');
            var r = GetRadMTB(o.id);
            if (e == '1') {
                o.readOnly = false;
                r.ReadOnly = false;
            }
            r.SetValue(val);
            var func = "HideValue('" + id + "')";
            window.setTimeout(func, 1000 * showTime);
            o.focus();
        }
    }
}
function SetAmountTotal(id, val) {
    amountTotal = val;
    SetFinancedAmountLabel(id, val);
}
function FormulaAmountFocusLost(o, ids, k) {
    var id = GetClearId(o.id, 2);
    if (id != '') {
        var a = GetControlsIds(id, ids);
        if (a != null) {
            var v = GetRCTAmounts(a);
            var t = CalculateGrandTotal(v, k);
            SetAmountTotal(id + '_FinancedAmount', t);
        }
    }
}
function FormulaAmountFocusLostOneRow(o, ids, idt, k) {
    var id = GetClearId(o.id, 2);
    if (id != '') {
        var a = GetControlsIds(id, ids);
        var v = GetRCTAmounts(a);
        var t = CalculateRowTotal(v, k);
        SetAmountLabel(id + '_' + idt, t);
    }
}
function GetFirstAddPage(v1, v2, v3) {
    var r = v1;
    if ((v2 > 0) && (v3 > 0)) r += v2 * (v3 - 1);
    return r;
}
function CalculateRowTotal(v, i) {
    var r = 0;
    switch (i) {
        case 2:
            r = v[0];
            if ((v[1] > 0) && (v[2] > 0)) r += v[1] * (v[2] - 1);
            break;
        case 3:
            r = v[0] * v[1];
            break;
    }
    return r;
}
function ValidatePOCAmount(src, arg) {
    arg.IsValid = true;
    var o = document.getElementById(GetClearId(src.id, 1) + '_FinancedAmount');
    var a = o.getAttribute('amount');
    if (a != '' && a < 0) {
        arg.IsValid = false;
    }
}
function SetAmountLabel(id, v) {
    var o = document.getElementById(id);
    if (o != null) {
        o.innerText = GetMoneyValue(v);
    }
}
function SetFinancedAmountLabel(id, v) {
    SetAmountLabel(id, v);
    var o = document.getElementById(id);
    if (o != null) {
        o.setAttribute('amount', v);
    }
}
function CalculateGrandTotal(v, i) {
    var r = 0;
    switch (i) {
        case 1:
            r = v[0] + v[1] + v[2] - v[3] - v[4] - v[5];
            break;
        case 2:
            r = GetFirstAddPage(v[0], v[1], v[2]) + GetFirstAddPage(v[3], v[4], v[5]) + GetFirstAddPage(v[6], v[7], v[8]) - v[9] - v[10] - v[11];
            break;
        case 3:
            r = v[0] * v[1] + v[2] * v[3] + v[4] * v[5] - v[6] - v[7] - v[8];
            break;
        case 4:
            r = v[0] + v[1] - v[2] - v[3] - v[4];
            break;
        case 5:
            r = v[0] - v[1] - v[2] - v[3];
            break;
        case 6:
            r = v[0] - v[1] - v[2] - v[3];
            break;
    }
    return r;
}
function GetClearId(id, cnt) {
    var i = 0;
    for (var k = 0; k < cnt; k++) {
        i = id.lastIndexOf('_');
        if (i > 0) {
            id = id.substring(0, i);
        } else {
            return '';
        }
    }
    return id;
}
function GetControlsIds(id, ids) {
    var a = ids.split(',');
    var res = new Array(a.length);
    for (var i = 0; i < a.length; i++) {
        res[i] = id + '_' + a[i];
    }
    return res;
}
function GetRCTAmounts(a) {
    var r = new Array(a.length);
    for (var i = 0; i < a.length; i++) {
        var v = GetAmountValue(GetRMTBId(a[i]));
        if (v == '') {
            v = 0;
        }
        r[i] = v;
    }
    return r;
}
function GetMoneyValue(v) {
    var r = v;
    if (r == '') {
        r = 0;
        r = r.toFixed(2);
    } else {
        var n = r < 0;
        r = Math.abs(r);
        var s = r.toFixed(2);
        var l = s.length - 6;
        if (l > 0) {
            var s1 = ',' + s.substr(l);
            s = s.substr(0, l);
            while (1) {
                l = s.length - 3;
                if (l <= 0) {
                    s1 = s + s1;
                    break;
                } else {
                    s1 = ',' + s.substr(l) + s1;
                    s = s.substr(0, l);
                }
            }
            r = s1
        } else {
            r = s;
        }
        if (n) r = '-' + r;
    }
    return '$' + r;
}
function ClickCLRB(o, v, b) {
    var i = o.parentNode.getAttribute('itemid');
    var r = 'checklist=' + o.parentNode.getAttribute('itemid') + '&val=' + v;
    SendNoResponseRequest(r);
    if (b == '1') {
        SetProceedButtonState(i + ';');
    }
}
function SendNoResponseRequest(req) {
    var xmlhttp = GetXmlHttp();
    if (xmlhttp == null) {
        alert('ajax not supported');
        return;
    }
    var url = document.URL;
    var i = url.indexOf('?');
    if (i > 0) {
        url = url.substring(0, i);
    }
    url = url.toLowerCase().replace('default.aspx', 'ProcessRequest.aspx');
    url += '?' + req;
    xmlhttp.open("GET", url, true);
    xmlhttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
    xmlhttp.setRequestHeader('Accept-Language', 'en/us');
    xmlhttp.setRequestHeader("Content-Length", "0");
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4) {
            var resp = xmlhttp.responseText;
            var statusText = xmlhttp.statusText;
            var status = xmlhttp.status;
            if (status == 200) {
                xmlhhtp = null;
            } else {
                alert(statusText);
            }
        }
    }
    xmlhttp.send(null);
}
function ClearReq(id) {
    var e = document.getElementById(id + '_l');
    if (e) {
        var ind = e.getAttribute('reqindex');
        if (ind) {
            var c = '';
            var i = document.getElementById(id);
            if (i) {
                if (i.tagName.toLowerCase() == 'input') {
                    if (i.value == '') c = 'reqfield';
                } else if (i.tagName.toLowerCase() == 'select') {
                    if (i.value == '0') c = 'reqfield';
                }
            }
            var cn = isIe ? 'className' : 'class';
            e.setAttribute(cn, c);
            var isCompleted = c == '';
            var r = ChangeRequiredFields('aReq2Fields', isCompleted, ind);
            if (r) {
                var tid = document.getElementById('tab2index').value;
                SetTabRedFormatting(isCompleted, 'tab2Id', tid);
                ind = document.getElementById('tab1index').value - 1;
                r = ChangeRequiredFields('aReq1Fields', isCompleted, tid);
                if (r) {
                    SetTabRedFormatting(isCompleted, 'tabId', ind);
                    r = ChangeRequiredFields('mortgreq', isCompleted, ind);
                    if (r) {
                        UpdateLoanInfo(isCompleted);
                        var isFollowUpRed = CheckFollowup();
                        UpdateAppList(isCompleted && (!isFollowUpRed));
                    }
                }
            }
        }
    }
}
function CheckFollowup() {
    var t = window['Tabs_RadTabStrip1'];
    var t2 = t.Tabs[3];
    var d = document.getElementById(t2.ID);
    var isRed = false;
    if (d) isRed = d.style.color == 'red';
    return isRed;
}
function UpdateLoanInfo(isCompleted) {
    var t = window['Tabs_RadTabStrip1'];
    var t2 = t.Tabs[0];
    var d = document.getElementById(t2.ID);
    if (d) {
        var cn = isIe ? 'className' : 'class';
        d.style.color = isCompleted ? '' : 'red';
        var s = d.getAttribute(cn);
        var a = s.split(' ');
        for (var i = 0; i < a.length; i++) {
            if (a[i] == 'red') {
                a[i] = '';
                break;
            }
        }
        s = a.join(' ');
        if (!isCompleted) s += ' red';
        d.setAttribute(cn, s);
    }
}
function UpdateFollowup(isCompleted) {
    var t = window['Tabs_RadTabStrip1'];
    var t2 = t.Tabs[3];
    var d = document.getElementById(t2.ID);
    if (d) {
        var cn = isIe ? 'className' : 'class';
        d.style.color = isCompleted ? '' : 'red';
        var s = d.getAttribute(cn);
        var a = s.split(' ');
        for (var i = 0; i < a.length; i++) {
            if (a[i] == 'red') {
                a[i] = '';
                break;
            }
        }
        s = a.join(' ');
        if (!isCompleted) s += ' red';
        d.setAttribute(cn, s);
    }
}
function IsApplicantListRed() {
    var o = window['ApplicantList1_RadPBMortgages'];
    if (o) {
        var cn = isIe ? 'className' : 'class';
        var s = o.SelectedItem.LinkElement.getAttribute(cn);
        var i = s.lastIndexOf('textUpdateNeededSelected');
        return i >= 0;
    }
}
function UpdateAppList(isCompleted) {
    var o = window['ApplicantList1_RadPBMortgages'];
    if (o) {
        var cn = isIe ? 'className' : 'class';
        var s = o.SelectedItem.LinkElement.getAttribute(cn);
        if (isCompleted) {
            s = 'link selected';
            o.SelectedItem.SelectedCssClass = 'selected';
        } else {
            s = 'link textUpdateNeeded textUpdateNeededSelected';
            o.SelectedItem.SelectedCssClass = 'textUpdateNeededSelected';
        }
        o.SelectedItem.LinkElement.className = s;
    }
}
function SetTabRedFormatting(isCompleted, tabId, tid) {
    var t = document.getElementById(tabId);
    if (t) {
        t = window[t.value];
        if (t) {
            SetTabColor(t, tid, isCompleted);
        }
    }
}
function SetTabColor(t, tid, isCompleted) {
    var t2 = t.Tabs[tid];
    var d = document.getElementById(t2.ID);
    if (d) d.style.color = isCompleted ? '' : 'red';
}
function ChangeRequiredFields(id, isCompleted, index) {
    var r = false;
    var h = document.getElementById(id);
    if (h) {
        var state = GetTabState(h.value);
        SetTabState(h, index, isCompleted ? '1' : '0');
        var newstate = GetTabState(h.value);
        r = state != newstate;
    }
    return r;
}
function SetTabState(h, i, val) {
    var index = i * 1;
    var s = h.value;
    if (index < s.length) {
        if (index < 1) {
            s = val + s.substring(1);
        } else if (index == (s.length - 1)) {
            s = s.substring(0, s.length - 1) + val;
        } else {
            s = s.substring(0, index) + val + s.substring(index + 1);
        }
        h.value = s;
    }
}
function GetTabState(s) {
    for (var i = 0; i < s.length; i++) {
        if (s.charAt(i) != '1') {
            return false;
        }
    }
    return true;
}
function OnTabLoad(obj) {
    RestoreActiveElement();
}
function RestoreActiveElement() {
    if (focusedElement != null && focusedElement != 'undefined') {
        if (focusedElement.id != null && focusedElement.id != '') {
            var o = document.getElementById(focusedElement.id);
            if (o != null) o.focus();
            focusedElement = null;
        }
    }
}
var ErrList = new Array();
var focusedElement = null;
function ErrorItem(id, errtext) {
    this.Id = id;
    this.ErrText = errtext;
}
var ajaxFlag = -1;
var errid = null;
function GetRadMTB(id) {
    var i = GetRMTBId(id);
    if (i != null) {
        return window[i];
    }
    return null;
}
function GetRMTBId(id) {
    var i = id.lastIndexOf('_');
    if (i > 0) {
        var id = id.substring(0, i);
        return id;
    }
    return null
}
function RadMaskedTextBoxHiddenFocusGot(o) {
    var hidden = o.getAttribute("hidden");
    if (hidden != null && hidden == '1') {
        var id = GetRMTBId(o.id);
        var param = GetXml(id, '?');
        GetHiddenValue(o.id, param);
    } else {
        RadMaskedTextBoxFocusGot(o);
    }
}
function RadMaskedTextBoxFocusGot(o) {
    var r = GetRadMTB(o.id);
    if (r != null) {
        o.setAttribute("val", r.GetDisplayValue());
    }
}
function RadMaskedTextBoxFocusLost(o, needPostBack) {
    var hidden = o.getAttribute("hidden");
    if (hidden != null && hidden == '1') {
    } else {
        var oldv = o.getAttribute("val");
        if (needPostBack == 1) {
            var o1 = document.getElementById(o.id.substring(0, o.id.length - 5));
            o1.setAttribute('needPostBack', 1);
        }
        var r = GetRadMTB(o.id);
        if (r != null) {
            var newv = r.GetDisplayValue();
            if (newv != oldv) {
                var id = GetRMTBId(o.id);
                SendRequest(id, GetXml(id, r.GetValue()));
            }
        }
    }
}
function ClickRBList(o) {
    var oldvalue = o.getAttribute("selected");
    var newvalue = null;
    var rbl = o.getElementsByTagName("input");
    for (var i = 0; i < rbl.length; i++) {
        if (rbl[i].checked) {
            var newvalue = rbl[i].value;
            break;
        }
    }
    if (newvalue) {
        if (oldvalue != newvalue) {
            o.setAttribute("selected", newvalue);
            SendRequest(o.id, GetXml(o.id, newvalue));
        }
    }
}
function GetXmlHttp() {
    if (window.ActiveXObject) {
        try {
            return new ActiveXObject("Microsoft.XMLHTTP");
        } catch (e) {
            return null;
        }
    } else if (window.XMLHttpRequest) {
        return new XMLHttpRequest();
    }
    return null;
}
function GetAttribute(arr, name) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i].name == name) {
            return arr[i].value;
        }
    }
    return '';
}
function UpdateCampaignResult(r) {
    var isRed = IsApplicantListRed();
    if (r == '0') {
        UpdateFollowup(true);
        if (isRed) {
            UpdateAppList(true);
        }
    } else {
        UpdateFollowup(false);
        if (!isRed) {
            UpdateAppList(false);
        }
    }
}
function ProcessResponse(id, data) {
    processing = 0;
    var xmlDoc = null;
    var err = '';
    var result = '';
    var arr = null;
    if (navigator.userAgent.toLowerCase().indexOf('msie') > 0) {
        xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc.loadXML(data);
        arr = xmlDoc.documentElement.attributes;
    } else {
        var parser = new DOMParser();
        xmlDoc = parser.parseFromString(data, "text/xml");
        arr = xmlDoc.documentElement.attributes;
        parser = null;
    }
    xmlDoc = null;
    result = GetAttribute(arr, 'result');
    if (result != '0') {
        err = GetAttribute(arr, 'errtext');
        SetError(id, err);
        ajaxFlag = 1;
        errid = id;
    } else {
        SetError(id, '');
        ClearReq(id);
        var tabupdate = parseInt(GetAttribute(arr, 'tabupdate'));
        if (tabupdate & 1) {
            var tabvalue = GetAttribute(arr, 'tabvalue');
            UpdateTab(GetAttribute(arr, 'tabvalue'));
        }
        if (tabupdate & 2) {
            UpdateApplicantListClient(GetAttribute(arr, 'applistvalue'));
        }
        var campaignresult = GetAttribute(arr, 'campaignresult');
        if (campaignresult != '') {
            UpdateCampaignResult(campaignresult);
        }
        ajaxFlag = -1;
        var o = document.getElementById(id);
        if (o != null) {
            var needPostBack = o.getAttribute('needPostBack');
            if (needPostBack == 1)
                InitiateAjaxRequest(GetPropertyName(id));
        }
    }
}
function UpdateApplicantListClient(listvalue) {
    var o = window['ApplicantList1_RadPBMortgages'];
    o.SelectedItem.SetText(listvalue);
}
function UpdateTab(tabval) {
    var o = window['Tabs_CtrlMortgageProfiles1_tabsBorrower'];
    o.SelectedTab.SetText(tabval);
}
function SetError(id, text) {
    var e = document.getElementById(id + '_e');
    e.innerHTML = text;
    AddError(id, text);
}
function AddError(id, text) {
    for (var i = 0; i < ErrList.length; i++) {
        var item = ErrList[i];
        if (item.Id == id) {
            if (text == '') {
                ErrList.splice(i, 1);
            }
            return;
        }
    }
    if (text != '') {
        ErrList = ErrList.concat(new ErrorItem(id, text));
    }
}
function SendRequest(id, req) {
    focusedElement = document.activeElement;
    if (req.length == 0) return;
    var xmlhttp = GetXmlHttp();
    if (xmlhttp == null) {
        alert('ajax not supported');
        return;
    }
    ajaxFlag = 0;
    var url = document.URL;
    var i = url.indexOf('?');
    if (i > 0) {
        url = url.substring(0, i);
    }

    if (url.indexOf('default.aspx') == -1) {
        url += '/default.aspx'
    }

    url = url.toLowerCase().replace('default.aspx', 'ProcessRequest.aspx');
    url += '?' + req;
    xmlhttp.open("GET", url, true);
    xmlhttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
    xmlhttp.setRequestHeader('Accept-Language', 'en/us');
    xmlhttp.setRequestHeader("Content-Length", "0");
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4) {
            var resp = xmlhttp.responseText;
            var statusText = xmlhttp.statusText;
            var status = xmlhttp.status;
            if (status == 200) {
                ProcessResponse(id, resp);
                xmlhhtp = null;
            } else {
                alert(statusText);
            }
        }
    }
    processing = 1;
    xmlhttp.send(null);
}
function GetPropertyName(id) {
    var i = id.lastIndexOf('_');
    if (i > 0) {
        var s = id.substring(0, i);

        i = s.lastIndexOf('_');
        if (i > 0) {
            return s.substring(i + 1);
        } else {
            return '';
        }
        return '';
    } else {
        return '';
    }
    return '';
}
function GetXml(id, val) {
    var o = document.getElementById('currentmortgageid');
    if (o == null) return '';
    var p = document.getElementById(id + '_l');
    var r = '0';
    if (p != null) {
        r = p.getAttribute('reqindex');
        if (!r || (r == '')) {
            r = '0';
        } else {
            r = '1';
        }
    }
    var data = 'mid=' + o.value + '&req=' + r;
    var i = id.lastIndexOf('_');
    if (i > 0) {
        data += '&oid=' + id.substring(i + 1);
        var s = id.substring(0, i);
        i = s.lastIndexOf('.');
        if (i > 0) {
            var s1 = id.substring(0, i);
            i = s1.lastIndexOf('_');
            if (i > 0) {
                data += '&property=' + s.substring(i + 1) + '&value=' + encodeURIComponent(val);
            } else {
                return '';
            }
        } else {
            return '';
        }
        /*        
                i=s.lastIndexOf('_');
                if (i>0){
                    data +='&property='+s.substring(i+1)+'&value='+encodeURIComponent(val);
                    return data;
                }else{
                    return '';
                }
        */
        return data;
    } else {
        return '';
    }
    return '';
}
function FocusGot(o) {
    o.setAttribute("val", o.getAttribute("value"));
}
function FocusLost(o) {
    var val1 = o.getAttribute("val");
    var val2 = o.value;
    if (val1 != val2) {
        o.setAttribute("val", val2);
        o.setAttribute("value", val2);
        SendRequest(o.id, GetXml(o.id, o.value));
    }
}
function ClickCheckbox(o, needPostBack) {
    o.setAttribute('needPostBack', needPostBack);
    SendRequest(o.id, GetXml(o.id, o.checked ? 'true' : 'false'));
}
var currentComboIndex = '';
/*
function RadComboFocusGot(o){
    currentComboIndex=o.GetValue();
}
function RadComboIndexChanged(o,needPostBack){
    var s = o.GetValue();
    if (s!=currentComboIndex){        
        var oo=document.getElementById(o.ClientID);
        if(oo!=null){
            oo.setAttribute('needPostBack',needPostBack);
        }
        currentComboIndex=s;
        if (s!='0') {
            SendRequest(o.ClientID,GetXml(o.ClientID,s));
            SetError(o.ClientID,'');
        } else
            SetError(o.ClientID,'*');
    } 
}
*/
function DDLFocusGot(o) {
    currentComboIndex = o.value;
}
function DDLIndexChanged(o, needPostBack) {
    var s = o.value;
    if (s != currentComboIndex) {
        if (o != null) {
            o.setAttribute('needPostBack', needPostBack);
        }
        currentComboIndex = s;
        //        if (s!='0') {
        SendRequest(o.id, GetXml(o.id, s));
        SetError(o.id, '');
        //        } else
        //            SetError(o.id,'*');
    }
}
function RadInputDateChanged(o, args) {
    var ob = document.getElementById(o.ClientID + '_text');
    var needPostBack = null;
    if (ob != null) {
        needPostBack = ob.getAttribute('needPostBack');
        ob = document.getElementById(o.ClientID);
        ob.setAttribute('needPostBack', needPostBack);
    }
    var d = o.GetDate();
    var s = '';
    if (d != null) {
        //        SetError(o.ClientID,'*');
        s = (d.getMonth() + 1) + '/' + d.getDate() + '/' + d.getFullYear();
    }
    SendRequest(o.ClientID, GetXml(o.ClientID, s));
    //    else{        
    //        var s = (d.getMonth()+1)+'/'+d.getDate()+'/'+d.getFullYear();
    //        SendRequest(o.ClientID,GetXml(o.ClientID,s));
    //    }
}
function Sleep(ms) {
    var date = new Date();
    var curDate = null;
    do { curDate = new Date(); }
    while (curDate - date < ms);
}
function CheckFlag() {
    for (var i = 0; i < 99; i++) {
        if (ajaxFlag == -1) return true;
        if (ajaxFlag > 0) return false;
        Sleep(10);
    }
    return true;
}
function CheckCalculatorValues() {
    var c = document.getElementById('calcneedcheck');
    if (c != null && c.value == 1) {
        c = document.getElementById('calcmodified');
        if (c.value == 1) {
            alert('The payment plan settings have changed. Please click the calculate button before navigating away from this page.');
            return false;
        }
    }
    return true;
}
function CheckError() {
    if (!CheckCalculatorValues()) return;
    if (CheckFlag()) {
        return true;
    } else {
        var res = confirm('You have changed a field value without moving away from the field. Save the changed data and continue?');
        if (!res) {
            if (errid != null) {
                var o = document.getElementById(errid);
                if (o != null) o.focus();
            }
        }
        else {
            ajaxFlag = -1;
        }
        return res;
    }
}
var cnt = 0;
function SetDiv(o, status) {
    var vis = status ? 'block' : 'none';
    var id = o.id.substr(0, o.id.lastIndexOf('_') + 1) + 'divb';
    SetVisibile(vis, document.getElementById(id));
    SetVisibile('none', o);
    if (status) {
        id = o.id.replace('iexp', 'icol');
    } else {
        id = o.id.replace('icol', 'iexp');
    }
    var o1 = document.getElementById(id);
    if (o1 != null) {
        o1.style.display = 'block';
    }
}
function HideShowDiv(str) {
    var fobj = document.getElementById(str);
    if (fobj && fobj != 'undefined') {
        if (fobj.style.display == 'none') fobj.style.display = 'block';
        else fobj.style.display = 'none';
    }
}
function HideShowNoteDiv(id, show) {
    var fobj = document.getElementById(id);
    if (fobj && fobj != 'undefined') {
        if (show == 1) fobj.style.display = 'block';
        else fobj.style.display = 'none';
    }
}
function SetFocus(str) {
    var fobj = document.getElementById(str);
    if (fobj && fobj != 'undefined') {
        fobj.focus();
    }
}
function SetFields(o, objects) {
    var vis = o.checked ? 'block' : 'none';
    var pref = o.id.substr(0, o.id.lastIndexOf('_') + 1);
    for (var i = 0; i < objects.length; i++) {
        var c = pref + objects[i];
        var l = pref + 'l_' + objects[i];
        k = l.indexOf('_TextBox');
        if (k > 0) {
            l = l.substr(0, k);
        }
        SetVisibile(vis, document.getElementById(c));
        SetVisibile(vis, document.getElementById(l));
    }
}
function SetVisibile(vis, o) {
    if (o) {
        o.style.display = vis;
        if (o.tagName.toLowerCase() == 'img') return;
        if ((vis == 'none') || (o.tagName.toLowerCase() == 'span')) return;
        var p = o.offsetParent;
        if (p.tagName.toLowerCase() != 'span') {
            if (p.childNodes.length > 0) {
                p = p.childNodes[0];
                if (p.tagName.toLowerCase() != 'span') return;
            }
        }
        if (p != null) {
            if (p.style.display == 'none') {
                p.style.display = vis;
            }
        }
    }
}
var showProcMsg = true;
function TurnOffProcessingMessage() {
    showProcMsg = false;
    return true;
}
var needAjaxRequest = true;
function TurnOffAjaxRequest() {
    needAjaxRequest = false;
    return true;
}
var dr;
var de;
var ds;
function AjaxRequestStart(sender, args) {
    ds = new Date();
    if (!CheckError()) {
        return false;
    }
    if (needAjaxRequest && showProcMsg) {
        if (args.EventTarget != 'MailTimer') {
            StopTimer();
            document.getElementById("LoadingDiv").style.visibility = 'visible';
        }
    }
    else {
        if (!needAjaxRequest) {
            args.EnableAjax = false;
            needAjaxRequest = true;
        }
        if (!showProcMsg)
            showProcMsg = true;
    }
}
function AjaxResponseReceived() {
    dr = new Date();
}

function GetActiveElement() {
    alert(document.activeElement.outerHTML);
}
function SetActiveElement() {
    RestoreActiveElement();
    //    if(focusedElement!=null && focusedElement!='undefined'){    
    //        var o=document.getElementById(focusedElement.id);
    //        o.focus();  
    //    }
}
function RestoreErrors() {
    if (ErrList != null) {
        for (var i = 0; i < ErrList.length; i++) {
            var item = ErrList[i];
            var o = document.getElementById(item.Id + '_e');
            if (o != null) {
                o.innerHTML = item.ErrText;
            }
        }
    }
}
function OnClientItemClickedHandler(sender, eventArgs) {
    if (eventArgs.Item) {
        if (eventArgs.Item.Parent != null) {
            eventArgs.Item.Parent.Close();
        }
    }
}
function WinOpen(pagename) {
    var popup = window.open(pagename);
    if (!popup) alert("A popup blocker may be preventing opening the page! Please, disable it to open the window.");
}
function ActivateNextBtn(btnSaveObj) {
    var mainRes = false;
    var f = document.forms['form1'];
    var cn; // choice number
    var nl, nli; // node list, and index, of radio input objects
    var re; // radio input element object
    var res = new Array(); // boolean results per radio group

    nl = 1;
    for (cn = 0; nl; ++cn) {
        str = 'Tabs$ctl00$rpChecklist$ctl0' + cn + '$rg0';
        nl = f.elements[str];
        if (nl) {
            res[cn - 1] = false;
            for (nli = 0; nli < nl.length; ++nli) {
                re = nl[nli];
                if (re.checked) {
                    res[cn - 1] = true;
                    mainRes = true;
                }
            }
        }
    }
    for (var i = 0; i < res.length; ++i) {
        if (!res[i]) {
            mainRes = false;
        }
    }
    if (btnSaveObj && btnSaveObj != null) btnSaveObj.disabled = !mainRes;
}
function OnClicking(sender, eventArgs) {
    if (eventArgs.Item.Value == "Loan" || eventArgs.Item.Value == "Tools") {
        return false;
    }
    if (eventArgs.Item.Value == "RetailSite" || eventArgs.Item.Value == "Tools") {
        window.open("/RetailSite/RetailPage.aspx?control=init");
        return false;
    }
    if (eventArgs.Item.Value == "NewMortgage") {
        radopen(null, "CreateMortgage1");
        return false;
    }
    if (eventArgs.Item.Value == "NewBorrower") {
        AddNewBorrower();
        return false;
    }
    if (eventArgs.Item.Value == "Help") {
        ShowHelp();
        return false;
    }

    if (eventArgs.Item.Value == "Logout") {
        $.ajax({
            type: "POST",
            url: "/Handlers/LogOut.ashx",
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                window.top.location.reload(true);
            },
            error: function () {
                alert("Error");
            }
        });
    }

    
    var itemName = eventArgs.Item.Value;
    var popups = ["Emails", "Vendors", "Reports", "Links", "MyProfile", "GFE"];

    $.each(popups, function(i, item) {
        if (item === itemName) {
            showContainer(itemName);
        }
    });
}

function showContainer(classToAdd) {
    var dialogPanel = $("#DialogPanel");
    var container = dialogPanel.parents('.pnlDialog').removeClass(this.classToAdd).addClass(classToAdd);
    container.show();
    this.classToAdd = classToAdd;
};

function AddNewMortgage(mortgageId) {
    //window.opener['RadAjaxManager1'].AjaxRequest();
    //TurnOffProcessingMessage();
    CallApplicantList(mortgageId);
}
function AddNewBorrower() {
    radopen(null, "AddBorrower");
    return false;
}
function InitiateAjaxRequest(arguments) {
    RadAjaxManager1.AjaxRequest(arguments);
}
function TabSelected(sender, eventArgs) {
    var tab = eventArgs.Tab
    var o = document.getElementById('currenttab');
    if (o != null) {
        o.value = tab.Index;
    }
}
function AdvCalculator() {
    radopen(null, "AdvCalculator");
    return false;
}
function CreateMailFolder() {
    radopen(null, "CreateMailFolder");
    return false;
}
function RefreshPage() {
    location.href = 'Default.aspx';
}
function StartTimer() {
    //MailTimer.Start();
}

function StopTimer() {
    //MailTimer.Stop();
}
function ShowProcessImage(show) {
    var img = document.getElementById('imgMailChecking');
    var lnk = document.getElementById('Emails_lnkCheckNewEmail');
    if (img && img != null) {
        if (show) {
            img.setAttribute('style', 'display:inline;');
        }
        if (lnk) lnk.innerHTML = (show) ? "checking for new email" : "check email";
        img.style.display = (show) ? 'inline' : 'none';
    }
}

function BindContextMenu() {


    $('#Tabs_CtrlTasks1_FollowupConditions1_gridConditions_ctl01').contextMenu({
        selector: 'tr',
        callback: function (key, options) {
            var id = $(this).find(".row_id").val();

            $("#conditionActiveID").val(id);

            if (key == "Advance") {
                $.ajax({
                    type: "POST",
                    url: "/Handlers/Advance.ashx",
                    data: JSON.stringify({ id: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        AjaxNS.AR('Tabs$CtrlTasks1$FollowupConditions1$btnRefresh', '', 'RadAjaxManager1', event);
                        return false;
                    },
                    error: function () {
                        alert("Error");
                    }
                });

                return false;
            }
            else if (key == "Frequency-Everyday") {
                $.ajax({
                    type: "POST",
                    url: "/Handlers/FrequencyEveryday.ashx",
                    data: JSON.stringify({ id: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        AjaxNS.AR('Tabs$CtrlTasks1$FollowupConditions1$btnRefresh', '', 'RadAjaxManager1', event);
                        return false;
                    },
                    error: function () {
                        alert("Error");
                    }
                });

                return false;
            }
            else if (key == "Frequency-EveryOtherDay") {
                $.ajax({
                    type: "POST",
                    url: "/Handlers/FrequencyEveryOtherDay.ashx",
                    data: JSON.stringify({ id: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        AjaxNS.AR('Tabs$CtrlTasks1$FollowupConditions1$btnRefresh', '', 'RadAjaxManager1', event);
                        return false;
                    },
                    error: function () {
                        alert("Error");
                    }
                });

                return false;
            }
            else if (key == "Frequency-OnceWeek") {
                $.ajax({
                    type: "POST",
                    url: "/Handlers/FrequencyOnceWeek.ashx",
                    data: JSON.stringify({ id: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        AjaxNS.AR('Tabs$CtrlTasks1$FollowupConditions1$btnRefresh', '', 'RadAjaxManager1', event);
                        return false;
                    },
                    error: function () {
                        alert("Error");
                    }
                });

                return false;
            }
            else if (key == "Frequency-EveryOtherWeek") {
                $.ajax({
                    type: "POST",
                    url: "/Handlers/FrequencyEveryOtherWeek.ashx",
                    data: JSON.stringify({ id: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        AjaxNS.AR('Tabs$CtrlTasks1$FollowupConditions1$btnRefresh', '', 'RadAjaxManager1', event);
                        return false;
                    },
                    error: function () {
                        alert("Error");
                    }
                });

                return false;
            }
            else if (key == "Frequency-OnceMonth") {
                $.ajax({
                    type: "POST",
                    url: "/Handlers/FrequencyOnceMonth.ashx",
                    data: JSON.stringify({ id: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        AjaxNS.AR('Tabs$CtrlTasks1$FollowupConditions1$btnRefresh', '', 'RadAjaxManager1', event);
                        return false;
                    },
                    error: function () {
                        alert("Error");
                    }
                });

                return false;
            }
            else if (key == "Completed") {
                $.ajax({
                    type: "POST",
                    url: "/Handlers/Completed.ashx",
                    data: JSON.stringify({ id: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        AjaxNS.AR('Tabs$CtrlTasks1$FollowupConditions1$btnRefresh', '', 'RadAjaxManager1', event);
                        return false;
                    },
                    error: function () {
                        alert("Error");
                    }
                });

                return false;
            }
        },
        items: {
            "Advance": { name: "Advance to the next follow-up date" },
            "Frequency": {
                name: "Change the follow-up frequency",
                items: {
                    "Frequency-Everyday": { name: "Everyday" },
                    "Frequency-EveryOtherDay": { name: "Every other day" },
                    "Frequency-OnceWeek": { name: "Once a week" },
                    "Frequency-EveryOtherWeek": { name: "Every other week" },
                    "Frequency-OnceMonth": { name: "Once a month" },
                    //"Frequency-Never": { name: "Never" }
                }
            },
            "Completed": { name: "Satisfy" }
        }
    });
}

function AjaxResponseEnd() {
    de = new Date();
    var ds1 = (de.getTime() - ds.getTime()) / 1000;
    document.getElementById("LoadingDiv").style.visibility = 'hidden';
    RestoreErrors();
    window.setTimeout('SetActiveElement()', 500);
}

function hideDialog(e) {
    if (!e) return false;
    var btn = e.target;
    var dialog = $(btn).parents('.pnlDialog').hide();
    var dialogPanel = $("#DialogPanel");
    dialogPanel.empty();
};

function toggleReccurence(item) {
    var rows = $("#TableFollowUp").find("> tbody > tr");
    if (item.value === "6") {
        rows.eq(1).hide();
        rows.eq(2).hide();
    } else {
        rows.eq(1).show();
        rows.eq(2).show();
    }
};

function OnAjaxPanelRequestEnd() {
    console.log(arguments);
};