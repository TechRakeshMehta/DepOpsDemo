var myApi = null;
var isReloadClicked = false;
var currentPage = null;



function ReloadRotationDocument(packageSubscriptionId) {
    var tenantId = $jQuery("[id$=hdfTenantId]").val();
    var documentId;
    var hdnRotationDocumentId = "[id$=hdnRotationDocumentId]";
    if ($jQuery(hdnRotationDocumentId) != undefined && $jQuery(hdnRotationDocumentId) != null && $jQuery(hdnRotationDocumentId).val() != "") {
        documentId = $jQuery(hdnRotationDocumentId).val();
        var docUrl = "Pages/InvitationDetailDocViewer.aspx?TenantId=" + tenantId + "&DocumentId=" + documentId
        var hdnUrlID = "[id$=hdnRtnDocVwr_" + packageSubscriptionId + "]";
        $jQuery(hdnUrlID).val(docUrl);
        load_Rotationpdf_Iwnd(false, packageSubscriptionId);
    }
}

function ShowRotationDocument(documentId, packageSubscriptionId) {
    var tenantId = $jQuery("[id$=hdfTenantId]").val();
    var hdnRotationDocumentId = "[id$=hdnRotationDocumentId]";
    var docUrl = "Pages/InvitationDetailDocViewer.aspx?TenantId=" + tenantId + "&DocumentId=" + documentId
    var hdnUrlID = "[id$=hdnRtnDocVwr]";

    $jQuery(hdnRotationDocumentId).val(documentId);
    $jQuery(hdnUrlID).val(docUrl);
    load_Rotationpdf_Iwnd(false, packageSubscriptionId);
}


function load_Rotationpdf_Iwnd(isFromChild, packageSubscriptionId) {
    var hdnUrlID = "[id$=hdnRtnDocVwr]";
    var iframePdfDocViewerRotationID = "[id$=iframePdfDocViewerRotation]";
    var adeDocVwr = $jQuery(hdnUrlID).val();
    //following code added to handle window close event
    iframeInstance = $jQuery(iframePdfDocViewerRotationID);
    $jQuery(iframeInstance).contents().find('body').html("<h1>Loading...</h1>");
    $jQuery(iframeInstance).attr('src', adeDocVwr);

    if (isFromChild) {
        parent.pdfDocViewerChildWnd = null;
        $jQuery("[id$=btnUndockPdfVwr]").show();
    }
}

function btnUndockClick() {
    var iframe = $jQuery("[id$=iframePdfDocViewerRotation]").attr('src', "");
    var name = "docViewChild";
    var height = "height=" + 800 + ", ";
    var width = "width=" + 800 + ", ";
    var adeDocVwr = $jQuery("[id$=hdnRtnDocVwr]").val();
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

    CheckRotationChildWIndowClose(wnd);
}

function CheckRotationChildWIndowClose(wnd) {
    var timer = setInterval(function () {
        if (wnd != undefined && wnd.closed) {
            OnRotationChildWindowClose(wnd);
            clearInterval(timer);
            parent.pdfDocViewerChildWnd = null;
        }
        else if (parent.pdfDocViewerChildWnd.closed) {
            OnRotationChildWindowClose();
            clearInterval(timer);
            parent.pdfDocViewerChildWnd = null;
        }
    }, 1000);
}

function OnRotationChildWindowClose(wnd) {
    if (wnd != undefined) {
        load_Rotationpdf_Iwnd(true);
    }
    else if (parent.pdfDocViewerChildWnd != undefined && parent.pdfDocViewerChildWnd != null) {
        load_Rotationpdf_Iwnd(true);
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

function e_wcload() {
    // Get id   
    var pdfWebControlID = $jQuery("[id$=PdfWebControl1]").attr("id");
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
function OpenMutlipleRequirementCategoriesPopup(tenantID, PackageSubscriptionId, SharedCategoryIds,snapshotId,invitationSourceTypeCode) {
    var popupWindowName = "Manage Categories";
    //var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    //var ApplicantUserID = $jQuery("[id$=hdnApplicantUserID]").val();
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var url = $page.url.create("~/ProfileSharing/Pages/SharedInvitationCategorySelection.aspx?TenantID=" + tenantID + "&PackageSubscriptionId=" + PackageSubscriptionId + "&SharedCategoryIds=" + SharedCategoryIds + "&SnapshotId=" + snapshotId + "&invitationSourceTypeCode=" + invitationSourceTypeCode + "&InvitationGroupTypeCode="+ 'AAAB');
    var win = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnPopupClose }
                                    , function () {
                                        this.set_title(popupWindowName);
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
