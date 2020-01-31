var IsItemSwappedOkClicked = false;

function onSwapClick(s, e) {

    s.set_autoPostBack(false);
    var _cnt = 0;
    var _catIds = [];
    var _trs = [];

    $jQuery("#tblForm").find("[id*=chkSwap_]").each(function () {
        if (this.checked) {
            var _crntTR = $jQuery(this).closest("tr");
            _cnt += 1;
            _catIds.push(_crntTR.find("[id$=hdfCatId]").val());
            _trs.push(_crntTR);
        }
    });

    if (_cnt != 2) {
        $alert('Data can be changed between 2 items.');
    }
    else if (!isValidSelection(_catIds)) {
        $alert('Data can be changed between two items of the same category only.');
    }
    else {

        var _srcNewStsValue = getRowControlValue("hdfNewStatusCode", _trs[0]);
        var _tempStsValue = getRowControlValue("hdfNewStatusCode", _trs[1]);

        setRowControlValue("hdfNewStatusCode", _trs[1], _srcNewStsValue);
        setRowControlValue("hdfNewStatusCode", _trs[0], _tempStsValue);

        var _srcItemId = getRowControlValue("hdfItemId", _trs[0]);;
        var _srcSwapId = getRowControlValue("hdfSwappedItemId", _trs[0]);;
        if (_srcSwapId == "") {
            _srcSwapId = getRowControlValue("hdfItemId", _trs[0]);
        }

        var _targetItemId = getRowControlValue("hdfItemId", _trs[1]);;
        var _targetSwapId = getRowControlValue("hdfSwappedItemId", _trs[1]);;
        if (_targetSwapId == "") {
            _targetSwapId = getRowControlValue("hdfItemId", _trs[1]);
        }

        var _swpIds = _srcItemId + "_" + _targetItemId;

        var _srcNewSts = getRowControlValue("hdfSwappedItemId", _trs[0]);
        var _tempSts = getRowControlValue("hdfSwappedItemId", _trs[1]);

        setRowControlValue("hdfSwappedItemId", _trs[1], _srcSwapId);
        setRowControlValue("hdfSwappedItemId", _trs[0], _targetSwapId);

        var _srcNewStsValue = getRowControlValue("hdfSwappedItemId", _trs[0]);
        var _tempStsValue = getRowControlValue("hdfSwappedItemId", _trs[1]);

        swapElements(_trs);
    }
}

function getRowControlValue(ctrlType, tr) {
    return $jQuery(tr).find("[id$=" + ctrlType + "]").val();
}

function setRowControlValue(ctrlType, tr, value) {
    $jQuery(tr).find("[id$=" + ctrlType + "]").val(value);
}

var _mappedTargetCtrls = [];
function swapElements(trArr) {
    var _srcCtrls = [];
    var _targetCtrls = [];

    var _ctrlTypes = ["txt_", "numericTxt_", "datePicker_", "comboBox_"]
    for (var i = 0; i < trArr.length; i++) {

        $jQuery(trArr[i]).find("[id$=pnlAttrContainer]").each(function () {

            var _catId = trArr[i].find("[id$=hdfCatId]").val();
            var _itemId = trArr[i].find("[id$=hdfItemId]").val();
            var _attrId = $jQuery(this).find("[id$=hdfAttrId]").val();
            var _attrGrpId = $jQuery(this).find("[id$=hdfAttrGrpId]").val();
            var _attrDataType = $jQuery(this).find("[id$=hdfAttrDataType]").val();

            var _idSuffix = _catId + "_" + _itemId + "_" + _attrId + "_" + _attrGrpId + "_" + _attrDataType;

            for (var j = 0; j < _ctrlTypes.length; j++) {
                if ($jQuery(this).find("[id$=" + _ctrlTypes[j] + _idSuffix + "]").length > 0) {
                    if (i == 0)
                        _srcCtrls.push(_ctrlTypes[j] + _idSuffix);
                    else
                        _targetCtrls.push(_ctrlTypes[j] + _idSuffix);
                    break;
                }
            }
        })
    }

    _mappedTargetCtrls = [];
    for (var i = 0; i < _srcCtrls.length; i++) {

        var _srcAttrId = getCtrlAttrId(_srcCtrls[i]);
        var _srcAttrGrpId = getCtrlAttrGrpId(_srcCtrls[i]);
        var _srcAttrType = getCtrlAttrType(_srcCtrls[i]);
        var _tempValue = getCtrlValue(_srcCtrls[i], _srcAttrType);
        var _targetAttrGrpId = 0;
        var _targetAttrType = "";

        var _isSrcMatchFound = false;

        var _keyProperty = 0;

        if (_srcAttrGrpId > 0) {
            for (var j = 0; j < _targetCtrls.length; j++) {
                _targetAttrGrpId = getCtrlAttrGrpId(_targetCtrls[j]);
                _targetAttrType = getCtrlAttrType(_targetCtrls[j]);

                if (_targetAttrGrpId > 0 && _srcAttrGrpId == _targetAttrGrpId && _srcAttrType == _targetAttrType) {
                    _isSrcMatchFound = swapData(_srcCtrls[i], _srcAttrType, _tempValue, _targetCtrls[j], _targetAttrType);
                    break;
                }
            }
            clearUnMatchedSrcCtrls(_isSrcMatchFound, _srcCtrls[i], _srcAttrType);
        }
        else {
            for (var j = 0; j < _targetCtrls.length; j++) {
                var _targetAttrId = getCtrlAttrId(_targetCtrls[j]);
                _targetAttrType = getCtrlAttrType(_targetCtrls[j]);

                if (_targetAttrGrpId == 0 && _srcAttrId == _targetAttrId && _srcAttrType == _targetAttrType) {
                    _isSrcMatchFound = swapData(_srcCtrls[i], _srcAttrType, _tempValue, _targetCtrls[j], _targetAttrType);
                    break;
                }
            }
            clearUnMatchedSrcCtrls(_isSrcMatchFound, _srcCtrls[i], _srcAttrType);
        }
    }
    clearUnMatchedTargetCtrls(_targetCtrls);
    //Method to unchecked the check boxes and clear the _catIds[] and _trs[]
    ClearCheckBoxes();
    IsItemSwappedOkClicked = true;
    var successMsg = 'Items swapped successfully.';
    ShowMessageInAlert(successMsg, $page.msgTypes.SUCCESS);
}

function clearUnMatchedSrcCtrls(isMatchFound, srcCtrl, srcAttrType) {
    if (!isMatchFound) {
        setCtrlValue(srcCtrl, "", srcAttrType);
    }
}

function clearUnMatchedTargetCtrls(targetCtrls) {
    for (var i = 0; i < targetCtrls.length; i++) {
        if ($jQuery.inArray(targetCtrls[i], _mappedTargetCtrls) == -1) {
            var _targetAttrType = getCtrlAttrType(targetCtrls[i]);
            setCtrlValue(targetCtrls[i], "", _targetAttrType);
        }
    }
}

function swapData(_srcCtrl, _srcAttrType, _tempValue, _targetCtrl, _targetAttrType) {
    var _targetCtrlValue = getCtrlValue(_targetCtrl, _targetAttrType);
    setCtrlValue(_srcCtrl, _targetCtrlValue, _srcAttrType);
    setCtrlValue(_targetCtrl, _tempValue, _targetAttrType);
    _mappedTargetCtrls.push(_targetCtrl);
    return true;
}

function getCtrlAttrGrpId(ctrl) {
    var _ctrlIdArr = ctrl.split('_');
    return _ctrlIdArr[4];
}


function getCtrlAttrId(ctrl) {
    var _ctrlIdArr = ctrl.split('_');
    return _ctrlIdArr[3];
}

function getCtrlAttrType(ctrl) {
    var _ctrlIdArr = ctrl.split('_');
    return _ctrlIdArr[5];
}

function getCtrlValue(ctrlId, ctrlType) {
    var _ctrlValue = "";
    if (ctrlType == "ADTOPT") {
        _ctrlValue = $find($jQuery("[id$=" + ctrlId + "]")[0].id).get_selectedItem().get_value();
    }
    else if (ctrlType == "ADTNUM" || ctrlType == "ADTTEX")
        _ctrlValue = $find($jQuery("[id$=" + ctrlId + "]")[0].id).get_value();
    else if (ctrlType == "ADTDAT")
        _ctrlValue = $find($jQuery("[id$=" + ctrlId + "]")[0].id).get_selectedDate();
    return _ctrlValue;
}

function setCtrlValue(ctrlId, ctrlValue, ctrlType) {

    if (ctrlType == "ADTNUM" || ctrlType == "ADTTEX")
        $find($jQuery("[id$=" + ctrlId + "]")[0].id).set_value(ctrlValue);
    else if (ctrlType == "ADTOPT") {
        if (ctrlValue == "") {
            ctrlValue = "0";
        }
        var _crtnItem = $find($jQuery("[id$=" + ctrlId + "]")[0].id).findItemByValue(ctrlValue);
        if (_crtnItem != null && _crtnItem != undefined)
            _crtnItem.select();
    }
    else if (ctrlType == "ADTDAT")
        $find($jQuery("[id$=" + ctrlId + "]")[0].id).set_selectedDate(ctrlValue);
}

function isValidSelection(catIds) {

    if (catIds.length != 2) {
        return false;
    }
    if (catIds[0] != catIds[1])
        return false;

    return true;
}

function OpenMutlipleSubscriptionsPopup() {
    var popupWindowName = "Manage Multiple Subscriptions";
    var fromScreenName = "Admin Data Entry";
    var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    var ApplicantUserID = $jQuery("[id$=hdnApplicantUserID]").val();
    var isSameDocument = $jQuery("[id$=hdnIsSameDocument]").val();
    var currentSubId = $jQuery("[id$=hdnPackageSubscriptionID]").val();

    var nextTenantId = $jQuery("[id$=hdnNextTenantId]").val();
    var nextApplicantId = $jQuery("[id$=hdnNextApplicantId]").val();

    var documentId = $jQuery("[id$=hdnDocumentId]").val();
    var crntUserId = $jQuery("[id$=hdnCurrentUserId]").val();

    var fdeqId = $jQuery("[id$=hdnCrntFdeqId]").val();
    var IsDiscardDocument = $jQuery("[id$=hdnIsDiscardDocument ]").val();//UAT-2742

    if (nextApplicantId == '' || nextApplicantId == undefined) {
        nextApplicantId = "0";
    }
    if (nextTenantId == '' || nextTenantId == undefined) {
        nextTenantId = "0";
    }

    var urlNew = $page.url.get_currentFolder() + "Pages/PackageSelectionForDataEntry.aspx?TenantID=" + tenantID + "&ApplicantID=" + ApplicantUserID
                        + "&IsSameDoc=" + isSameDocument + "&CurrentSubId=" + currentSubId + "&NextTenantId=" + nextTenantId + "&NextAppId=" + nextApplicantId
                        + "&DocId=" + documentId + "&CrntUserId=" + crntUserId + "&FdeqId=" + fdeqId + "&IsDiscardDocument=" + IsDiscardDocument
    //var url = $page.url.create("~/ComplianceOperations/Pages/PackageSelectionForDataEntry.aspx?TenantID=" + tenantID + "&ApplicantID=" + ApplicantUserID
    //                    + "&IsSameDoc=" + isSameDocument + "&CurrentSubId=" + currentSubId + "&NextTenantId=" + nextTenantId + "&NextAppId=" + nextApplicantId
    //                    + "&DocId=" + documentId + "&CrntUserId=" + crntUserId + "&FdeqId=" + fdeqId);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(urlNew, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnClose }
       );
    return false;
}

//This event fired when multiple subscription popup closed.
function OnClose(oWnd, args) {
    oWnd.remove_close(OnClose);
    var arg = args.get_argument();
    if (arg) {
        if (arg.Action == "Submit") {
            $jQuery("[id$=hdnPackageSubscriptionID]").val(arg.PackageSubscriptionID);
            $jQuery("[id$=hdnApplicantUserID]").val(arg.ApplicantId);
            $jQuery("[id$=hdnSelectedTenantID]").val(arg.TenantId);

            $jQuery("[id$=hdnDocChanged]").val(arg.IsDocChange);
            // __doPostBack("<%= btnRedirect.ClientID %>", "");
            var btnId = $jQuery("[id$=btnRedirect]");
            btnId.click();
        }
    }
    else {
        var btnId = $jQuery("[id$=btnBackToQueue]");
        btnId.click();
    }

}

function onDocReady() {
    setTimeout(function () {

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(requestHandler);
        if (parent.pdfDocViewerChildWnd == null) {
            load_pdf_Iwnd();
        }
        else {
            //change_cwnd_loc();
            CheckChildWIndowClose();
            //ChangePdfDocVwrScroll($jQuery("[id$=hdnSelectedCatUnifiedStartPageID]").val());
        }

        if (parent.pdfDocViewerChildWnd != null) {
            var hdnDocIDDataEntry = $jQuery("[id$=hdnDocIDDataEntry]").val();
            var hdnDocIDDocVViewer = parent.pdfDocViewerChildWnd.$jQuery('[id$=hdnDocIDDocViewer]').val();
            var current_tenant = $jQuery("[id$=hdnSelectedTenantID]").val();
            var existing_tenant = parent.pdfDocViewerChildWnd.$jQuery('[id$=hdnTenantIdCurrent]').val();
            if (hdnDocIDDataEntry != hdnDocIDDocVViewer || current_tenant != existing_tenant) {
                parent.pdfDocViewerChildWnd.location = $jQuery("[id$=hdnADEDocVwr]").val();
            }
        }

        //---UAT-1163-----------------------------------------------------------------------------
        $jQuery("[id$=paneTop]").on('scroll', function () {
            $jQuery("[id$=paneHeader]").scrollLeft($jQuery(this).scrollLeft());
        });

        var firstRow = $jQuery("[id$=tblForm]").find('tr')[0].innerHTML;
        var tblHtml = "<table style=\"height:34px\" id=\"tblCopyHeader\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tbody><tr>" + firstRow + "</tr></tbody></table>";
        $jQuery("[id$=Panel1]")[1].innerHTML = tblHtml;
        setHeaderHeightWidth();
        //---------------------------------------------------------------------------------
    }, 100);
}


$jQuery(document).ready(function () {

});


function load_pdf_Iwnd(isFromChild) {
    var adeDocVwr = $jQuery("[id$=hdnADEDocVwr]").val();
    //following code added to handle window close event
    iframeInstance = $jQuery("[id$=iframePdfDocViewer]");

    $jQuery(iframeInstance).contents().find('body').html("<h1>Loading...</h1>");
    $jQuery(iframeInstance).attr('src', adeDocVwr);

    if (isFromChild) {
        parent.pdfDocViewerChildWnd = null;
        $jQuery("[id$=btnUndockPdfVwr]").show();
    }
}

function btnUndockClick() {
    var iframe = $jQuery("[id$=iframePdfDocViewer]").attr('src', "");
    var name = "docViewChild";
    var height = "height=" + 800 + ", ";
    var width = "width=" + 800 + ", ";
    var adeDocVwr = $jQuery("[id$=hdnADEDocVwr]").val();
    var options = height + width + "menubar=no,status=no, titlebar=yes,top=0 ";
    wnd = window.open(adeDocVwr, "pdfwnd", 'width=900,height=600,status=yes,resizable=yes,titlebar=yes', false);
    parent.pdfDocViewerChildWnd = wnd;

    if (iframeInstance) {
        iframeInstance = null;
    }

    if (parent.wnd_load_in != undefined) {
        //1 for Cwnd and 0 for Iwnd
        parent.wnd_load_in = 1;
    }

    CheckChildWIndowClose(wnd);
}

function CheckChildWIndowClose(wnd) {
    var timer = setInterval(function () {
        if (wnd != undefined && wnd.closed) {
            OnChildWindowClose(wnd);
            clearInterval(timer);
            parent.pdfDocViewerChildWnd = null;
        }
        else if (parent.pdfDocViewerChildWnd.closed) {
            OnChildWindowClose();
            clearInterval(timer);
            parent.pdfDocViewerChildWnd = null;
        }
    }, 1000);
}

function OnChildWindowClose(wnd) {
    if (wnd != undefined) {
        load_pdf_Iwnd(true);
    }
    else if (parent.pdfDocViewerChildWnd != undefined && parent.pdfDocViewerChildWnd != null) {
        load_pdf_Iwnd(true);
    }
}

function dock_wndvwr() {
    //window.parent.opener.load_pdf_Iwnd(true);    
    window.close();
    return true;
}

function requestHandler(sender, args) {
    if (args.get_error() == undefined) {
        if (parent.pdfDocViewerChildWnd == null) {
            load_pdf_Iwnd();
        }
        else {
            $jQuery("[id$=btnUndockPdfVwr]").hide();
        }
    }
}

function ClearCheckBoxes() {
    _catIds = [];
    _trs = [];
    _mappedTargetCtrls = [];

    $jQuery("#tblForm").find("[id*=chkSwap_]").each(function () {
        if (this.checked) {
            this.checked = false;
        }
    })
}

function ShowMessageInAlert(msg, msgtype) {
    $jQuery("#pageMsgBox").children("span").text(msg).attr("class", msgtype);
    $window.showDialog($jQuery("#pageMsgBox").clone().show(), { closeBtn: { autoclose: true, text: "Ok" } }, 400, 'Complio');
}

window.onbeforeunload = function () {
    //return "Hello";
    var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    var urltoPost = "/ComplianceOperations/Default.aspx/DataEntryTracking";
    var dataString = "tenantID : '" + tenantID + "'";

    if (navigator.userAgent.search("MSIE") >= 0 && IsItemSwappedOkClicked == true) {
        IsItemSwappedOkClicked = false;
    }
    else {
        $jQuery.ajax
         (
          {
              type: "POST",
              async: false,
              cache: false,
              url: urltoPost,
              data: "{ " + dataString + " }",
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (result) {
                  //var data = JSON.parse(result.d);
                  //return "hello";
              }
          });
    }
};


function handleRadComboUI() {
    $jQuery('div[id$=paneTop]').on("scroll", function () {
        $jQuery.each($jQuery('.RadComboBox').toArray(), function (index, obj) {
            $find(obj.id).hideDropDown();
        });
    });
}

function handleRadPickerUI() {
    $jQuery('div[id$=paneTop]').on("scroll", function () {
        $jQuery.each($jQuery('.RadCalendarPopup').toArray(), function (index, obj) {
            obj.style.display = "none";
        });
    });
}




function RadPaneResized(sender, args) {
    setHeaderHeightWidth();
}

function setHeaderHeightWidth() {
    //debugger;
    var mainHeader = $jQuery("[id$=tblForm]").find('tr:first') //.clone();
    $jQuery($jQuery("[id$=tblCopyHeader]").find("tr")).css('width', mainHeader.width());
    $jQuery($jQuery("[id$=tblCopyHeader]")).css('width', mainHeader.width());
    mainHeader.find('td').each(function (index) {
        $jQuery($jQuery("[id$=tblCopyHeader]").find("td")[index]).css('width', $jQuery(this)[0].offsetWidth);
    });


    var copyHeaderOffsetHeight = $jQuery("[id$=tblCopyHeader]")[0].offsetHeight;
    if (navigator.userAgent.search("MSIE") >= 0) {
        $jQuery($jQuery("[id$=paneHeader]")).css('height', copyHeaderOffsetHeight);
        $jQuery($jQuery("[id$=tblForm]")).css('margin-top', Math.abs(copyHeaderOffsetHeight) * -1);
    }
    else {

        var updatedHieght = Math.abs(copyHeaderOffsetHeight) - 7;
        //Set Height of Header Pane.
        $jQuery($jQuery("[id$=paneHeader]")).css('height', copyHeaderOffsetHeight);
        $jQuery($jQuery("[id$=tblForm]")).css('margin-top', Math.abs(updatedHieght) * -1);
    }


    //Insert scrollbar in header pane if lowerpane has scroll bar.
    if ($jQuery("[id$=paneTop]")[2].scrollHeight > $jQuery("[id$=paneTop]")[2].offsetHeight) {
        $jQuery($jQuery("[id$=paneHeader]")).css('overflow-y', 'scroll');
        $jQuery($jQuery("[id$=paneHeader]")).css('overflow-x', 'hidden');
    }
    else {
        $jQuery($jQuery("[id$=paneHeader]")).css('overflow-y', 'auto');
    }

    $jQuery(mainHeader).css('visibility', 'hidden');
}

function ShowConfimDialog() {
    $confirm("No active subscription found for this document. " + " <br/><span style='font-weight:bolder'>Press Yes/No to go back to queue.</span><br/>&nbsp;", function (cr) {
        var btnId = $jQuery("[id$=btnBackToQueue]");
        btnId.click();
    }, 'Complio', true);
}

function ConfirmDiscard(sender, eventArgs) {
    if (!confirm('Are you sure you want to discard this document?')) {
        sender.set_autoPostBack(false);
    }
    else {
        //Production issue [26/12/2016]
        //ShowDiscardReasonPopup(sender, eventArgs);
        OpenDocumentDiscardReasonPopup();
        //Commented below code for Production issue [26/12/2016]
        //sender.set_autoPostBack(true);
    }
}


$page.showAlertMessage = function (msg, msgtype, overriderErrorPanel) {
    if (typeof (msg) == "undefined") return;
    var c = typeof (msgtype) != "undefined" ? msgtype : "";
    if (overriderErrorPanel) {
        $jQuery("#pageMsgBoxDataEntry").children("span")[0].innerHTML = msg;
        $jQuery("#pageMsgBoxDataEntry").children("span").attr("class", msgtype);
        if (c == 'sucs') {
            c = "Success";
        }
        else (c = "Validation Message(s)");

        $jQuery("[id$=pnlErrorDataEntry]").hide();

        $window.showDialog($jQuery("#pageMsgBoxDataEntry").clone().show(), { closeBtn: { autoclose: true, text: "Ok" } }, 500, c);
    }
    else {
        $jQuery("#pageMsgBoxDataEntry").fadeIn().children("span")[0].innerHTML = msg;
        $jQuery("#pageMsgBoxDataEntry").fadeIn().children("span").attr("class", msgtype);

    }
}


//Production Issue [26/12/2016]

function OpenDocumentDiscardReasonPopup() {
    var popupWindowName = "Discard Reason";
    var fromScreenName = "Admin Data Entry";
    var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    var documentId = $jQuery("[id$=hdnDocumentId]").val();
    var crntUserId = $jQuery("[id$=hdnCurrentUserId]").val();

    var fdeqId = $jQuery("[id$=hdnCrntFdeqId]").val();
    var hdnDiscardDocumentCount = $jQuery("[id$=hdnDiscardDocumentCount]").val();
    var url = $page.url.create("~/ComplianceOperations/Pages/DataEntryDocumentDiscardReason.aspx?TenantID=" + tenantID
                        + "&DocId=" + documentId + "&FdeqId=" + fdeqId + "&DiscardDocumentCount=" + hdnDiscardDocumentCount);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (45 / 100);

    var win = $window.createPopup(url, { size: "700," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnCloseDocumentDiscardReasonPopup },
        function () {
            this.set_title(popupWindowName);
        });
    return false;
}

//This event fired when multiple subscription popup closed.
function OnCloseDocumentDiscardReasonPopup(oWnd, args) {
    oWnd.remove_close(OnCloseDocumentDiscardReasonPopup);
    var arg = args.get_argument();
    if (arg) {
        if (arg.Action == "Submit") {
            $jQuery("[id$=hdnDocumentDiscardReasonId]").val(arg.DiscardReasonId);
            var btnId = $jQuery("[id$=btnDiscardReasonRedirect]");
            btnId.click();
        }
    }

}

function ShowDiscardReasonPopup(sender, args) {
    var popupWindowName = "Discard Reason";
    $window.showDialog($jQuery(".discardStatusPopup").clone().show(), {
        continuebtn: {
            autoclose: true, text: "Continue", click: function () {
                var btnId = $jQuery("[id$=btnDiscardReasonRedirect]");
                btnId.click();
            }
        }, closeBtn: {
            autoclose: true, text: "Cancel"
        }
    }, 600, popupWindowName);
}

$page.showAlertMessageWithTitle = function (msg, msgtype, overRideErrorPanel) {
    if (typeof (msg) == "undefined") return;
    var c = typeof (msgtype) != "undefined" ? msgtype : "";

    $jQuery("#pageMsgBoxDataEntry").children("span")[0].innerHTML = msg;
    $jQuery("#pageMsgBoxDataEntry").children("span").attr("class", msgtype);


    $window.showDialog($jQuery("#pageMsgBoxDataEntry").clone().show(), {
        approvebtn: {
            autoclose: true, text: "Proceed with violations", click: function () {
                $jQuery("[id$=hdnOverRideUiRule]").val("1");
                var btnName = $jQuery("[id$=hdnButtonClicked]").val();
                var btnId = $jQuery("[id$=" + btnName + "]");
                btnId.click();
            }
        }, closeBtn: {
            autoclose: true, text: "Return to Data Entry", click: function () {

            }
        }
    }, 475, '&nbsp;');
    $jQuery("#pageMsgBoxDataEntry").css("display", "none");
    $jQuery("[id$=pnlErrorDataEntry]").hide();
}