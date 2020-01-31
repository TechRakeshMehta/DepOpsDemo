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

$jQuery(document).ready(function () {
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

function ShowMessageInAlert(msg, msgtype) {
    $jQuery("#pageMsgBox").children("span").text(msg).attr("class", msgtype);
    $window.showDialog($jQuery("#pageMsgBox").clone().show(), { closeBtn: { autoclose: true, text: "Ok" } }, 400, 'Complio');
}


function GetSingleDocument_CallBack(result) {
    if (result != null && result != undefined) {
        result = $jQuery.parseJSON(result)
        //var iFrame = $jQuery("[id$=iframeDocViewer]");
        var iFrame = $jQuery("[id$=iframePdfDocViewer]")
        var adeDocVwr = $jQuery("[id$=hdnADEDocVwr]").val(result.redirectUrl);

        if (iFrame.length != 0) {
            $jQuery("[id$=hdnDocVwr]").val(result.redirectUrl);
            $jQuery("[id$=hdnSingleDocVwr]").val(result.redirectUrl);
            if (parent.pdfDocViewerChildWnd != null) {
                parent.pdfDocViewerChildWnd.location = result.redirectUrl;
            }
            else { iFrame[0].src = result.redirectUrl; }
        }
    }
}