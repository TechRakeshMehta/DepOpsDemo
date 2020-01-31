$jQuery = $telerik.$;
var isReloadClicked = false;
var myApi = null;

$jQuery(document).ready(function () {

});

function onDocReady_DocViewer() {
    SetPdfControls();
}

function dock_wndvwr() {
    //window.parent.opener.load_pdf_Iwnd(true);    
    window.close();
    return true;
}

function SetPdfControls() {
    if (window.parent.opener != undefined) {
        var loadIn = window.parent.opener.parent.wnd_load_in;
        if (loadIn == 1) {
            $jQuery("[id$=btnDock]").show();
            window.parent.opener.$jQuery("[id$=btnUndockPdfVwr]").hide();
        }
        else {
            $jQuery("[id$=btnDock]").hide();
        }
    }
    else {
        $jQuery("[id$=btnDock]").hide();
    }
}

function e_wcload() {
    // Get id   
    var pdfWebControlID = $jQuery("[id$=PdfWebControl1]").attr("id");
    // Get api instance
    myApi = new PdfWebControlApi(pdfWebControlID);
    myApi.addEventListener(
        "viewChanged",
        function (evt) {
            fitPDFDocuments();
        });

    myApi.addEventListener(
    "rendered",
    function () {
        fitPDFDocuments(true);
        myApi.setMode(myApi.Mode.SelectText);
    });
}

function fitPDFDocuments(isFitReq) {
    if ((myApi.getView().zoom != 50 && myApi.getView().zoom != 100 && myApi.getView().zoom != 200) || isReloadClicked == true || isFitReq) {
        myApi.setView({ "zoom": "fit" });
        isReloadClicked = false;
    }
}

function rotate_page() {
    if (myApi) {
        myApi.getPageViewed().rotatePage(90);
        fitPDFDocuments();
    }
    return false;
}

function reload_page() {
    if (myApi) {
        isReloadClicked = true;
        fitPDFDocuments();
    }
}