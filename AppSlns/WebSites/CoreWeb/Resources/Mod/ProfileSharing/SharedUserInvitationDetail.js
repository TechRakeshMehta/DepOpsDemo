$jQuery = $telerik.$;

var myApi = null;
var isReloadClicked = false;
var currentPage = null;

function ShowToolTip(e) {
    var id = "#" + e.id;
    var ToolTipCustom = $jQuery(id).parent().siblings("#ToolTipCustom");
    var spnText = $jQuery(id).parent().siblings("#litCatDesc")[0].innerHTML;
    var resultHTML = spnText.substring(1, spnText.length - 1);
    ToolTipCustom[0].innerHTML = resultHTML;
    if (resultHTML != "")
        ToolTipCustom.show();
}
function HideToolTip(e) {
    var id = "#" + e.id;
    var ToolTipCustom = $jQuery(id).parent().siblings("#ToolTipCustom");
    ToolTipCustom.hide();
}


//Rotation
function ReloadRotationDocument(packageSubscriptionId) {
    var tenantId = $jQuery("[id$=hdfTenantId]").val();
    var documentId;
    var hdnRotationDocumentId = "[id$=hdnRotationDocumentId]";
    if ($jQuery(hdnRotationDocumentId) != undefined && $jQuery(hdnRotationDocumentId) != null && $jQuery(hdnRotationDocumentId).val() != "") {
        documentId = $jQuery(hdnRotationDocumentId).val();
        var docUrl = "Pages/InvitationDetailDocViewer.aspx?TenantId=" + tenantId + "&DocumentId=" + documentId
        var hdnUrlID = "[id$=hdnRtnDocVwr_" + packageSubscriptionId + "]";
        $jQuery(hdnUrlID).val(docUrl);
        load_pdf_Iwnd(false, packageSubscriptionId);
    }
}

function ShowRotationDocument(documentId, packageSubscriptionId, IsUnifiedDocument, startingIndex) {
    if (IsRotationDocumentUnifiedView && startingIndex == null) {
        if (RotationUnifiedSubsctionArray != "undefined" && RotationUnifiedSubsctionArray != "" && RotationUnifiedSubsctionArray != null) {
            $.each(RotationUnifiedSubsctionArray, function (i, j) {
                if (j.PkgSubscriptionID == packageSubscriptionId) {
                    //if (j.PkgSubscriptionStartIndex == j.PkgSubscriptionEndIndex) {
                    //    startingIndex = j.PkgSubscriptionStartIndex;
                    //}
                    //else {
                    //var pageNumber = 0;
                    //$.each(j.ApplicantDocumentDetailContarctList, function (k, l) {
                    //    if (l.ApplicantDocumentID == documentId) {
                    //        if (pageNumber == 0) {
                    //            startingIndex = j.PkgSubscriptionStartIndex;
                    //        }
                    //        else {
                    //            startingIndex = pageNumber + j.PkgSubscriptionStartIndex;
                    //        }
                    //    }
                    //    pageNumber = pageNumber + l.TotalPages;
                    //});
                    // }

                    //  ShowDocument(j.UnifiedDocumentPath, PackageSubscriptionId, true, j.PkgSubscriptionStartIndex); //j.PkgSubscriptionEndIndex

                    $.each(j.ApplicantDocumentDetailContarctList, function (k, l) {
                        if (l.ApplicantDocumentID == documentId) {
                            startingIndex = l.StartIndex;
                        }
                    });

                    IsUnifiedDocument = true;
                    documentId = j.UnifiedDocumentPath;
                    //debugger;
                    ChangePdfDocVwrScroll(startingIndex);
                }
            });
        }
        else
            return false;
    }
    else {
        if (startingIndex != "undefined" && startingIndex != null) {
            currentPage = startingIndex;
        }
        else
            currentPage = null;

        var tenantId = $jQuery("[id$=hdfTenantId]").val();
        var hdnRotationDocumentId = "[id$=hdnRotationDocumentId]";
        var docUrl = "Pages/InvitationDetailDocViewer.aspx?TenantId=" + tenantId;
        if (IsUnifiedDocument != "undefined" && IsUnifiedDocument != null && IsUnifiedDocument)
            docUrl = docUrl + "&UnifiedDocumentPath=" + documentId + "&StartPageIndexNumber=" + startingIndex;
        else
            docUrl = docUrl + "&DocumentId=" + documentId
        var hdnUrlID = "[id$=hdnADEDocVwr]";
        if (startingIndex != "undefined" && startingIndex != null) {
            currentPage = startingIndex;
        }
        else
            currentPage = null;
        $jQuery(hdnRotationDocumentId).val(documentId);
        $jQuery(hdnUrlID).val(docUrl);
        load_Rotationpdf_Iwnd(false, packageSubscriptionId);
    }
}


//Tracking
function ReloadDocument(packageSubscriptionId) {
    var tenantId = $jQuery("[id$=hdfTenantId]").val();
    var documentId;
    var hdnSelectedDocumentId = "[id$=hdnSelectedDocumentId_" + packageSubscriptionId + "]";
    if ($jQuery(hdnSelectedDocumentId) != undefined && $jQuery(hdnSelectedDocumentId) != null && $jQuery(hdnSelectedDocumentId).val() != "") {
        documentId = $jQuery(hdnSelectedDocumentId).val();
        var docUrl = "Pages/InvitationDetailDocViewer.aspx?TenantId=" + tenantId + "&DocumentId=" + documentId
        var hdnUrlID = "[id$=hdnADEDocVwr_" + packageSubscriptionId + "]";
        $jQuery(hdnUrlID).val(docUrl);
        load_pdf_Iwnd(false, packageSubscriptionId);
    }
}

function ShowDocument(documentId, packageSubscriptionId, IsUnifiedDocument, startingIndex) {
    if (IsRotationDocumentUnifiedView && (startingIndex == null || startingIndex == "undefined")) {
        if (RotationUnifiedSubsctionArray != "undefined" && RotationUnifiedSubsctionArray != "" && RotationUnifiedSubsctionArray != null) {
            $.each(RotationUnifiedSubsctionArray, function (i, j) {
                if (j.PkgSubscriptionID == packageSubscriptionId) {
                    $.each(j.ApplicantDocumentDetailContarctList, function (k, l) {
                        if (l.ApplicantDocumentID == documentId) {
                            startingIndex = l.StartIndex;
                        }
                    });
                    //  ShowDocument(j.UnifiedDocumentPath, PackageSubscriptionId, true, j.PkgSubscriptionStartIndex); //j.PkgSubscriptionEndIndex
                    IsUnifiedDocument = true;
                    documentId = j.UnifiedDocumentPath;
                    ChangePdfDocVwrScroll(startingIndex);
                }
            });
        }
        else
            return false;
    }
    else {
        if (startingIndex != "undefined" && startingIndex != null) {
            currentPage = startingIndex;
        }
        else
            currentPage = null;
        var tenantId = $jQuery("[id$=hdfTenantId]").val();
        var hdnSelectedDocumentId = "[id$=hdnSelectedDocumentId_" + packageSubscriptionId + "]";
        var docUrl = "Pages/InvitationDetailDocViewer.aspx?TenantId=" + tenantId;
        if (IsUnifiedDocument != "undefined" && IsUnifiedDocument != null && IsUnifiedDocument)
            docUrl = docUrl + "&UnifiedDocumentPath=" + documentId + "&StartPageIndexNumber=" + startingIndex;
        else
            docUrl = docUrl + "&DocumentId=" + documentId
        // + "&DocumentId=" + documentId
        var hdnUrlID = "[id$=hdnADEDocVwr_" + packageSubscriptionId + "]";
        // var hdnUrlID = "[id$=iframePdfDocViewer_New]";
        $jQuery(hdnSelectedDocumentId).val(documentId);
        $jQuery(hdnUrlID).val(docUrl);
        load_pdf_Iwnd(false, packageSubscriptionId);
    }
}


function ChangePdfDocVwrScroll(pageID) {
    if (parent.pdfDocViewerChildWnd != undefined && parent.pdfDocViewerChildWnd != null) {
        parent.pdfDocViewerChildWnd.change_pdf_scrollpos(pageID);
    }
    else if (iframeInstance) {
        iframeInstance[0].contentWindow.change_pdf_scrollpos(pageID);
    }
}

function change_pdf_scrollpos(pageID) {
    if (myApi) {
        myApi.setView({ "page": pageID });
        currentPage = pageID;
    }
}
// PDF Viewer

//$jQuery(document).ready(function () {
//    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(requestHandler);
//    if (parent.pdfDocViewerChildWnd == null) {
//        load_pdf_Iwnd();
//    }
//    else {
//        change_cwnd_loc();
//        CheckChildWIndowClose();
//        ChangePdfDocVwrScroll($jQuery("[id$=hdnSelectedCatUnifiedStartPageID]").val());
//    }
//    if ($jQuery("[id$=hdnIsApplicantChanged]").val() == "true" && parent.pdfDocViewerChildWnd != null) {
//        parent.pdfDocViewerChildWnd.location = $jQuery("[id$=hdnADEDocVwr]").val();
//    }
//});


function load_Rotationpdf_Iwnd(isFromChild, packageSubscriptionId) {
    var hdnUrlID = "[id$=hdnADEDocVwr]";
    var iframePdfDocViewerRotationID = "[id$=iframePdfDocViewer_New]";

    var adeDocVwr = $jQuery(hdnUrlID).val();
    //following code added to handle window close event
    iframeInstance = $jQuery(iframePdfDocViewerRotationID);

    if (IsIEBrowserAndLinkExist()) {
        $jQuery("[id$=lnkLoadDoc").attr('href', 'about:blank');
        $jQuery("[id$=lnkLoadDoc")[0].click();
    }
    else {
        $jQuery(iframeInstance).attr('src', '');
    }

    try {
        $jQuery(iframeInstance).contents().find('body').html("<h1>Loading...</h1>");
    } catch (e) {
    }

    if (IsIEBrowserAndLinkExist()) {
        $jQuery("[id$=lnkLoadDoc").attr('href', adeDocVwr);
        $jQuery("[id$=lnkLoadDoc")[0].click();
    }
    else {
        $jQuery(iframeInstance).attr('src', adeDocVwr);
    }

    if (isFromChild) {
        parent.pdfDocViewerChildWnd = null;
        $jQuery("[id$=btnUndockPdfVwr]").show();
    }
}
function load_pdf_Iwnd(isFromChild, packageSubscriptionId) {
    var hdnUrlID = "[id$=hdnADEDocVwr_" + packageSubscriptionId + "]";
    //var iframePdfDocViewerID = "[id$=iframePdfDocViewer_" + packageSubscriptionId + "]";
    var iframePdfDocViewerID = "[id$=iframePdfDocViewer_New]";
    //iframePdfDocViewer_New
    var adeDocVwr = $jQuery(hdnUrlID).val();
    //following code added to handle window close event
    iframeInstance = $jQuery(iframePdfDocViewerID);


    if (IsIEBrowserAndLinkExist()) {
        $jQuery("[id$=lnkLoadDoc").attr('href', 'about:blank');
        $jQuery("[id$=lnkLoadDoc")[0].click();
    }
    else {
        $jQuery(iframeInstance).attr('src', '');
    }

    try {
        $jQuery(iframeInstance).contents().find('body').html("<h1>Loading...</h1>");
    } catch (e) {

    }

    if (IsIEBrowserAndLinkExist()) {
        $jQuery("[id$=lnkLoadDoc").attr('href', adeDocVwr);
        $jQuery("[id$=lnkLoadDoc")[0].click();
    }
    else {
        $jQuery(iframeInstance).attr('src', adeDocVwr);
    }

    if (isFromChild) {
        parent.pdfDocViewerChildWnd = null;
        $jQuery("[id$=btnUndockPdfVwr]").show();
    }
}

function IsIEBrowserAndLinkExist() {

    var ua = navigator.userAgent;
    /* MSIE used to detect old browsers and Trident used to newer ones*/
    var is_ie = ua.indexOf("MSIE ") > -1 || ua.indexOf("Trident/") > -1;

    if (is_ie && $jQuery("[id$=lnkLoadDoc").length > 0) {
        return true;
    }
    else {
        return false;
    }

}

function btnUndockClick() {
    var iframe = $jQuery("[id$=iframePdfDocViewer_New]").attr('src', "");
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

//Function to fit the size of PDF documents in RADPDF document viewer.
function fitPDFDocuments(isFitReq) {
    if ((myApi.getView().zoom != 50 && myApi.getView().zoom != 100 && myApi.getView().zoom != 200) || isReloadClicked == true || isFitReq) {
        myApi.setView({ "zoom": "fit" });
        isReloadClicked = false;
    }
}

function e_wcload1() {
    // Get id
    var pdfWebControlID = $jQuery("[id$=PdfWebControl]").attr("id");
    // Get api instance
    myApi = new PdfWebControlApi(pdfWebControlID);
    currentPage = myApi.getPageViewed().getPageNumber();
    //myApi.setView({ "zoom": "fit" });
    myApi.addEventListener(
        "viewChanged",
        function (evt) {
            fitPDFDocuments();
            if (evt.view.page) {
                currentPage = evt.view.page;
                //highlight_cat_item_lnk(evt.view.page);
                //highlight_item_lnk(evt.view.page);//UAT-722
            }
        });

    myApi.addEventListener(
    "rendered",
    function () {
        fitPDFDocuments(true);
        myApi.setMode(myApi.Mode.SelectText);
    });
}

//Method to open categorys popup to select categories for document export

var IsPopManageCategoriesOpen = false;
function OpenMutlipleCategoriesPopup(tenantID, PackageSubscriptionId, SharedCategoryIds, snapshotId, invitationSourceTypeCode) {
    var popupWindowName = "Manage Categories";
    //var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    //var ApplicantUserID = $jQuery("[id$=hdnApplicantUserID]").val();
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);
    IsPopManageCategoriesOpen = true;

    var url = $page.url.create("~/ProfileSharing/Pages/SharedInvitationCategorySelection.aspx?TenantID=" + tenantID + "&PackageSubscriptionId=" + PackageSubscriptionId + "&SharedCategoryIds=" + SharedCategoryIds + "&SnapshotId=" + snapshotId + "&invitationSourceTypeCode=" + invitationSourceTypeCode);
    var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnClose }
                                    , function () {
                                        this.set_title(popupWindowName);
                                    });

    parent.$jQuery('.rwMaximizeButton').on('click', function () {
        setTimeout(function () {
            parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
        }, 40);
    });

    parent.$jQuery('.rwReloadButton').on('click', function () {
        setTimeout(function () {
            parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
        }, 40);
    });

    setTimeout(function () {
        parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
    }, 150);

    parent.$jQuery('.rwTitlebarControls').on('click', function () {
        setTimeout(function () {
            parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
        }, 40);
    });

    return false;
}

////This event fired when multiple Category popup closed.
function OnClose(oWnd, args) {
    debugger
    IsPopManageCategoriesOpen = false;
    oWnd.remove_close(OnClose);
    var arg = args.get_argument();
    if (arg) {
        if (arg.Action == "Submit") {
            $jQuery("[id$=hdnCategoryIdsForExport]").val(arg.SelectedCatgoryIds);
            $jQuery("[id$=hdnPackageSubIdForExport]").val(arg.PackageSubscriptionId);
            $jQuery("[id$=hdnSnapshotIdForExport]").val(arg.SnapshotID);
            $jQuery("[id$=hdnInvitationSourceTypeForExport]").val(arg.InvitationSourceType);
            var btnId = $jQuery("[id$=btnAfterExport]");
            //Call show progress method to show progress bar.
            Page.showProgress("Processing...");
            btnId.click();
        }
    }
}


//Open popup from Status and Data Reports
function OpenPassportReportPopup(subscriptionId, sharedCategoryIds, snapshotId, userId) {
    
    var tenantId = $jQuery("[id$=hdfTenantId]").val();
    var documentType = "ReportDocument";
    var reportType = "Data Report";
    if (snapshotId != "0") {
        reportType = "Status Report";
    }
   
    var composeScreenWindowName = "Passport Report";
    var url;
    if (snapshotId == "0")
        // url = $page.url.create("~/ComplianceOperations/Reports/ReportViewer.aspx?tid=" + tenantId + "&psid=" + subscriptionId + "&rptType=" + reportType + "&ShrdCatIds=" + sharedCategoryIds);
        var url = $page.url.create("~/ComplianceOperations/Pages/ComplianceReportViewer.aspx?Psid=" + subscriptionId + "&ShrdCatIds=" + sharedCategoryIds + "&DocumentType=" + documentType + "&ReportType=" + reportType + "&tenantId=" + tenantId + "&UserID=" + userId);
    else
        //url = $page.url.create("~/ComplianceOperations/Reports/ReportViewer.aspx?tid=" + tenantId + "&psid=" + subscriptionId + "&rptType=" + reportType + "&snpShtId=" + snapshotId);
        var url = $page.url.create("~/ComplianceOperations/Pages/ComplianceReportViewer.aspx?Psid=" + subscriptionId + "&SnpShtId=" + snapshotId + "&DocumentType=" + documentType + "&ReportType=" + reportType + "&tenantId=" + tenantId + "&UserID=" + userId);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var doc_win = $window.createPopup(url, { size: "760," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move }, function () { this.set_title(composeScreenWindowName); this.set_destroyOnClose(true); });

    parent.$jQuery('.rwMaximizeButton').on('click', function () {
        setTimeout(function () {
            parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
        }, 40);
    });

    setTimeout(function () {
        parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
    }, 150);

    parent.$jQuery('.rwTitlebarControls').on('click', function () {
        setTimeout(function () {
            parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
        }, 40);
    });

    return false;
}

function SaveCategoryOnCheck(sender) {
    var CheckBox = $jQuery("[id$=" + sender.id + "]");
    var cid = CheckBox[0].parentNode.getAttribute("cid");
    var psid = CheckBox[0].parentNode.getAttribute('psid');
    var isCmpSub = CheckBox[0].parentNode.getAttribute('isCmpSub');
    var tid = CheckBox[0].parentNode.getAttribute('tid');
    var psInvId = CheckBox[0].parentNode.getAttribute('psInvId');
    var isCatView;
    if (CheckBox.context.activeElement.checked) {
        isCatView = true;
    }
    else {
        isCatView = false;
    }
    PageMethods.SaveCategoryView(tid, psInvId, psid, cid, isCmpSub, isCatView);
}

function ViewScreeningDocument(documentId) {
    var popupWindowName = "View Document Window";
    var tenantId = $jQuery("[id$=hdfTenantId]").val();
    var url = $page.url.create("~/ComplianceOperations/Pages/FormViewer.aspx?tenantId=" + tenantId + "&documentId=" + documentId + "&IsApplicantDocument=" + "true");
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close },
                                        function () {
                                            this.set_title(popupWindowName);
                                            this.set_destroyOnClose(true);
                                        });
    return false;
}


function OpenEmployerDisclosureDocument(documentType) {
    var popupWindowName = "Employment Disclosure";
    var tenantId = $jQuery("[id$=hdfTenantId]").val();
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var url = $page.url.create("~/ComplianceOperations/Pages/ReportEmploymentDisclosure.aspx?DocumentTypeCode=" + documentType + "&tenantId=" + tenantId);
    var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close },
                                        function () {
                                            this.set_title(popupWindowName);
                                            this.set_destroyOnClose(true);
                                        });
    return false;
}




//Method to open categorys popup to select categories for document export
function OpenMutlipleRequirementCategoriesPopup(tenantID, PackageSubscriptionId, SharedCategoryIds, snapshotId, invitationSourceTypeCode, IsInstructorPreceptorData) {
    var popupWindowName = "Manage Categories";
    //var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    //var ApplicantUserID = $jQuery("[id$=hdnApplicantUserID]").val();
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var url = $page.url.create("~/ProfileSharing/Pages/SharedInvitationCategorySelection.aspx?TenantID=" + tenantID + "&PackageSubscriptionId=" + PackageSubscriptionId + "&SharedCategoryIds=" + SharedCategoryIds + "&SnapshotId=" + snapshotId + "&invitationSourceTypeCode=" + invitationSourceTypeCode + "&InvitationGroupTypeCode=" + 'AAAB' + "&IsInstructorPreceptorData=" + IsInstructorPreceptorData);
    var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnPopupClose }
                                    , function () {
                                        this.set_title(popupWindowName);
                                    });

    parent.$jQuery('.rwMaximizeButton').on('click', function () {
        setTimeout(function () {
            parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
        }, 40);
    });

    parent.$jQuery('.rwReloadButton').on('click', function () {
        setTimeout(function () {
            parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
        }, 40);
    });

    setTimeout(function () {
        parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
    }, 150);

    parent.$jQuery('.rwTitlebarControls').on('click', function () {
        setTimeout(function () {
            parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
        }, 40);
    });

    return false;
}

////This event fired when multiple Category popup closed.
function OnPopupClose(oWnd, args) {

    //   oWnd.remove_close(onClose);
    var arg = args.get_argument();
    if (arg) {
        if (arg.Action == "Submit") {
            $jQuery("[id$=hdnCategoryIdsForExport]").val(arg.SelectedCatgoryIds);
            $jQuery("[id$=hdnPackageSubIdForExport]").val(arg.PackageSubscriptionId);
            $jQuery("[id$=hdnSnapshotIdForExport]").val(arg.SnapshotID);
            $jQuery("[id$=hdnInvitationSourceTypeForExport]").val(arg.InvitationSourceType);
            var btnId = $jQuery("[id$=btnRtnDocExport]");
            //Call show progress method to show progress bar.
            Page.showProgress("Processing...");
            btnId.click();
        }
    }
}


function SaveRotationCategoryOnCheck(sender) {
    var CheckBox = $jQuery("[id$=" + sender.id + "]");
    var cid = CheckBox[0].parentNode.getAttribute("cid");
    var psid = CheckBox[0].parentNode.getAttribute('psid');
    var isCmpSub = CheckBox[0].parentNode.getAttribute('isCmpSub');
    var tid = CheckBox[0].parentNode.getAttribute('tid');
    var psInvId = CheckBox[0].parentNode.getAttribute('psInvId');
    var isCatView;
    if (CheckBox.context.activeElement.checked) {
        isCatView = true;
    }
    else {
        isCatView = false;
    }
    PageMethods.SaveCategoryView(tid, psInvId, psid, cid, isCmpSub, isCatView);
}



