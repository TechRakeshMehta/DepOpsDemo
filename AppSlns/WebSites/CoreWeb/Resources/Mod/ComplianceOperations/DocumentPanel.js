$jQuery(document).ready(function () {
    //$jQuery(".rrButtonLeft")[0].title = "Click to view previous page of documents";
    //$jQuery(".rrButtonRight")[0].title = "Click to view next page of documents";

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(requestHandler);
    if (parent.pdfDocViewerChildWnd == null) {
        load_pdf_Iwnd();
    }
    else {
        change_cwnd_loc();
        CheckChildWIndowClose();
        ChangePdfDocVwrScroll($jQuery("[id$=hdnSelectedCatUnifiedStartPageID]").val());
    }
    if ($jQuery("[id$=hdnIsApplicantChanged]").val() == "true" && parent.pdfDocViewerChildWnd != null) {
        parent.pdfDocViewerChildWnd.location = $jQuery("[id$=hdnDocVwr]").val();
    }
});

function change_cwnd_loc() {
    var current_app = $jQuery("[id$=hdnApplicantIdDocumentPanel]").val();
    var current_tenant = $jQuery("[id$=hdnTenantId]").val();
    var existing_app = parent.pdfDocViewerChildWnd.$jQuery('[id$=hdnApplicantIdCurrent]').val();
    var existing_tenant = parent.pdfDocViewerChildWnd.$jQuery('[id$=hdnTenantIdCurrent]').val();
    if (existing_app && existing_tenant) {
        if (existing_app != current_app || existing_tenant != current_tenant) {
            parent.pdfDocViewerChildWnd.location = $jQuery("[id$=hdnDocVwr]").val();
            parent.pdfDocViewerChildWnd.$jQuery('[id$=hdnPdfVwrLoadIn]').val("1");
        }
    }
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

function load_pdf_Iwnd(isFromChild) {
    var hdnDocVwr = $jQuery("[id$=hdnDocVwr]").val();
    $jQuery("[id$=btnDockPdfWnd]").hide();

    //following code added to handle window close event
    iframeInstance = $jQuery("[id$=iframePdfDocViewer]");
    $jQuery(iframeInstance).contents().find('body').html("<h1>Loading...</h1>");
    $jQuery(iframeInstance).attr('src', hdnDocVwr);

    if (isFromChild) {
        parent.pdfDocViewerChildWnd = null;
        $jQuery("[id$=btnUndockPdfVwr]").show();
    }
}

function highlight_category(currentPageID) {
    var catAnchorLinks = $jQuery("a.cat_lslnk");

    //if (catAnchorLinks && catAnchorLinks.length == 0) {
    //    catAnchorLinks = $jQuery("[id$=btnCategorylnk]");
    //}

    //UAT-1538
    var selectedDocumentViewType = $jQuery("[id$=rdbLstViewType]").find('input:radio:checked').val();
    //AAAC: is for Unified Document View Type and AAAD: is for Single Document view type
    if (selectedDocumentViewType == "AAAC") {
        $jQuery(catAnchorLinks).each(function () {
            var anchorLink = $jQuery(this);
            var unifiedDocPageMapping = anchorLink.attr("UnifiedDocPageMapping");
            if (CheckCurrentPageExistCatItemLink(unifiedDocPageMapping, currentPageID)) {
                anchorLink.addClass("cat_highlight");
            }
            else {
                anchorLink.removeClass("cat_highlight");
            }
        });
    }
    else { Highlight_Cat_For_SingleDocView(catAnchorLinks); }
}

//UAT-722
function highlight_item(currentPageID) {
    var itemAnchorLinks = $jQuery("a.item_lslnk");
    //UAT-1538
    var selectedDocumentViewType = $jQuery("[id$=rdbLstViewType]").find('input:radio:checked').val();
    //AAAC: is for Unified Document View Type and AAAD: is for Single Document view type
    if (selectedDocumentViewType == "AAAC") {
        $jQuery(itemAnchorLinks).each(function () {
            var anchorLink = $jQuery(this);
            var unifiedDocPageMapping = anchorLink.attr("UnifiedDocPageMapping");
            if (CheckCurrentPageExistCatItemLink(unifiedDocPageMapping, currentPageID)) {
                anchorLink.addClass("item_highlight");
            }
            else {
                anchorLink.removeClass("item_highlight");
            }
        });
    }
    else {
        Highlight_Item_For_SingleDocView(itemAnchorLinks);
    }
}

function CheckCurrentPageExistCatItemLink(unifiedDocPageMapping, currentPageID) {
    var isExists = false;
    var splitValueItemLevel = unifiedDocPageMapping.split(",");
    $jQuery(splitValueItemLevel).each(function () {
        if (!isExists) {
            var splitValue = this.split("-");
            if (currentPageID >= splitValue[0] && currentPageID <= splitValue[1]) {
                isExists = true;
            }
        }
    });
    return isExists;
}

function btnUndockClick() {
    ResetUtilityFeatForDockUnDock();
    var iframe = $jQuery("[id$=iframePdfDocViewer]").attr('src', "");
    var name = "docViewChild";
    var height = "height=" + 800 + ", ";
    var width = "width=" + 800 + ", ";
    var hdnDocVwr = $jQuery("[id$=hdnDocVwr]").val();
    var options = height + width + "menubar=no,status=no, titlebar=yes,top=0 ";
    wnd = window.open(hdnDocVwr, "pdfwnd", 'width=900,height=600,status=yes,resizable=yes,titlebar=yes', false);
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

var initialButtonClass = "";

function resizing(sender, Args) {

    var rotator = $jQuery(".rrRelativeWrapper");
    //var oldRotatorWidth = rotator[0].style.width;
    //var width = parseInt(oldRotatorWidth.substr(0, (oldRotatorWidth.indexOf('px') + 1)));
    //var newRotatorwidth = (width + (sender._width - width)) - 50;
    //rotator[0].style.width = newRotatorwidth.toString() + "px";
    //$jQuery("[id$=hdnRoratorWidth]").attr('value', newRotatorwidth.toString());
    //$jQuery(".rotatorcss").attr('overflow-y', "hidden");

}

function OnClientButtonClick(sender, eventArgs) {

    if (sender._canSlideMore() == false) {
        if (initialButtonClass == "" || initialButtonClass == eventArgs._button.className) {
            eventArgs._cancel = true;
        }
        else {
            eventArgs._cancel = false;
        }
        initialButtonClass = eventArgs._button.className;
    }
}

function itemclicked(sender, args) {
    //var getHiddenField = $jQuery(args.get_item().get_element()).find("input[type=hidden]");
    var getHiddenField = $jQuery(args.get_item().get_element()).find("[id$=hdnDoc]");
    var rotatorItem = args.get_item();
    var documentId = rotatorItem._element.lastElementChild.firstElementChild.className;
    var tenantId = $jQuery("input[id$=hdnTenantId]").val();
    var mergingCompletedDocStatusID = $jQuery("input[id$=hdnMergingCompletedDocStatusID]").val();
    var hdnApplicantDocumentMergingStatusID = $jQuery(args.get_item().get_element()).find("[id$=hdnApplicantDocumentMergingStatusID]");
    if (getHiddenField.length > 0) {
        //if documentStatusID is not 3 i.e. not Merging Completed
        if (hdnApplicantDocumentMergingStatusID.length > 0 && hdnApplicantDocumentMergingStatusID.val() != "" &&
            hdnApplicantDocumentMergingStatusID.val() != mergingCompletedDocStatusID) {
            var IsOKClicked = confirm('There was an error in merging this document. Do you want to view this document individually?');
            if (IsOKClicked)
                PageMethods.GetDataForFailedUnifiedDocument(documentId, tenantId, GetFailedUnifiedDocument_CallBack);
        }
        else {
            var hiddenFieldValue = $jQuery(getHiddenField).val();
            if (hiddenFieldValue == "") {
                PageMethods.GetDataForUnifiedDocument(documentId, tenantId, get_document_callback);
            }
            else {
                ChangePdfDocVwrScroll(hiddenFieldValue);
                return false;
            }
        }
    }
}

$page.add_pageReady(function () {
    $findByKey("step2tooltip", function () {
        this.show();
    });
});

function OnChildWindowClose(wnd) {
    if (wnd != undefined) {
        load_pdf_Iwnd(true);
    }
    else if (parent.pdfDocViewerChildWnd != undefined && parent.pdfDocViewerChildWnd != null) {
        load_pdf_Iwnd(true);
    }
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

function UpdateSettingForDocumentViewType() {
    //UAT-1538
    if (this.Page != undefined)
        Page.showProgress('Processing...');
    var selectedValue = $jQuery("[id$=rdbLstViewType]").find('input:radio:checked').val();
    SaveUpdateDocumentViewSetting(selectedValue);
    LoadDocumentInPDFViewer(selectedValue);
}

//UAT-1538----------------------------------------------------------------------------------
function LoadDocumentInPDFViewer(selectedValue) {
    var hdnDocVwr;
    //AAAC: is for Unified Document View Type and AAAD: is for Single Document view type
    if (selectedValue == "AAAC") {
        hdnDocVwr = $jQuery("[id$=hdnUnifiedDocVwr]").val();
    }
    else {
        hdnDocVwr = $jQuery("[id$=hdnSingleDocVwr]").val();
    }
    $jQuery("[id$=hdnDocVwr]").val(hdnDocVwr);
    var iFrame = $jQuery("[id$=iframePdfDocViewer]");
    //if (iFrame.length != 0) {
    //    iFrame.attr('src', hdnDocVwr);
    //}
    if (parent.pdfDocViewerChildWnd != null) {
        parent.pdfDocViewerChildWnd.location = hdnDocVwr;
    }
    else { iFrame[0].src = hdnDocVwr; }
}

function Highlight_Cat_For_SingleDocView(catAnchorLinks) {
    $jQuery(catAnchorLinks).each(function () {
        var anchorLink = $jQuery(this);
        var categoryDocumentLst = anchorLink.attr("appDocumentIds");
        var currentDocumentID = $jQuery("[id$=hdnCurrentDocID]").val();
        if (CheckCurrentDocumentExistCatItemLink(categoryDocumentLst, currentDocumentID)) {
            anchorLink.addClass("cat_highlight");
        }
        else {
            anchorLink.removeClass("cat_highlight");
        }
    });
}
function Highlight_Item_For_SingleDocView(itemAnchorLinks) {
    $jQuery(itemAnchorLinks).each(function () {
        var anchorLink = $jQuery(this);
        var itemDocumentLst = anchorLink.attr("appDocumentIds");
        var currentDocumentID = $jQuery("[id$=hdnCurrentDocID]").val();
        if (CheckCurrentDocumentExistCatItemLink(itemDocumentLst, currentDocumentID)) {
            anchorLink.addClass("item_highlight");
        }
        else {
            anchorLink.removeClass("item_highlight");
        }
    });
}

function CheckCurrentDocumentExistCatItemLink(documentList, currentDocumentID) {
    var isExists = false;
    var splitValueItemLevel = documentList.split(",");
    $jQuery(splitValueItemLevel).each(function () {
        if (!isExists) {
            if (currentDocumentID == this) {
                isExists = true;
            }
        }
    });
    return isExists;
}
//------------------------------------------------------------------------------------

