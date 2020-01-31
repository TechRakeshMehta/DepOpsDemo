/*
    pdfviewer.js for Verifications
*/
$jQuery = $telerik.$;

var myApi = null;
var isDownload = false;
var currentPage = null;
var dock = null;

var isReloadClicked = false;


$jQuery(document).ready(function () {
    SetPdfControls();
    //UAT-1538
    //Hide progress bar 
    if (parent.Page != undefined)
        parent.Page.hideProgress();
});

//function highlight_cat_lnk(currentPageID) {
//    if (parent.iframeInstance) {
//        window.parent.hightlight_cat_link(currentPageID);
//    }
//    else {
//        window.parent.opener.hightlight_cat_link(currentPageID);
//    }
//}

////UAT-722
//function highlight_item_lnk(currentPageID) {
//    if (parent.iframeInstance) {
//        window.parent.highlight_item(currentPageID);
//    }
//    else {
//        window.parent.opener.highlight_item(currentPageID);
//    }

//}

//UAT-722 - Also the Category Highlight code combined with item Highlight code
function highlight_cat_item_lnk(currentPageID) {
    if (parent.iframeInstance) {
        window.parent.highlight_category(currentPageID);
        window.parent.highlight_item(currentPageID);
    }
    else {
        window.parent.opener.highlight_category(currentPageID);
        window.parent.opener.highlight_item(currentPageID);
    }
}

function SetPdfControls(isChildWnd) {
    //if (window.parent.opener != undefined) {
    //}
    if (window.parent.opener != undefined) {
        var loadIn = window.parent.opener.parent.wnd_load_in;
        //$jQuery("[id$=hdnPdfVwrLoadIn]").val(loadIn);
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
    currentPage = myApi.getPageViewed().getPageNumber();
    //myApi.setView({ "zoom": "fit" });
    myApi.addEventListener(
        "viewChanged",
        function (evt) {
            fitPDFDocuments();
            if (evt.view.page) {
                currentPage = evt.view.page;
                highlight_cat_item_lnk(evt.view.page);
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

//Function to fit the size of PDF documents in RADPDF document viewer.
function fitPDFDocuments(isFitReq) {
    if ((myApi.getView().zoom != 50 && myApi.getView().zoom != 100 && myApi.getView().zoom != 200) || isReloadClicked == true || isFitReq) {
        myApi.setView({ "zoom": "fit" });
        isReloadClicked = false;
    }
}

function change_pdf_scrollpos(pageID, uni_doc_val) {
    var current_unidoc_val = $jQuery("[id$=hdnUnifiedDoc_value]").val();
    if (uni_doc_val != undefined && uni_doc_val != current_unidoc_val) {
        ShowPdfMessage("Latest merged PDF document is not loaded yet in the document viewer. Please reload latest merged PDF document.");
        return false;
    }
    if (myApi) {
        myApi.setView({ "page": pageID });
        currentPage = pageID;
        HidePdfMessage();
    }
}

function rotate_page() {
    if (myApi) {
        myApi.getPageViewed().rotatePage(90);
        fitPDFDocuments();
    }
    return false;
}

//UAT-518

function reload_page() {
    if (myApi) {
        isReloadClicked = true;
        fitPDFDocuments();
    }
}

function OnDragResetUtilityFeature(sender, args) {
    var currentLoggedInUserId = $("[id$=hdnCurrentLoggedInUserId]")[0].value;
    var dataString = "organizationUserId : '" + currentLoggedInUserId + "',ignoreAlert : '" + "true" + "'";
    var urltoPost = "/ComplianceOperations/Default.aspx/ResetUtilityFeatureForDockUnDock";
    HideDockUndock();
    GetDockPosition(sender, args);
    $.ajax
     (
      {
          type: "POST",
          url: urltoPost,
          data: "{ " + dataString + " }",
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          success: function (data) {
              var fileIdentifier = data.d;
          }
      });
}

function dock_wndvwr() {
    //window.parent.opener.load_pdf_Iwnd(true);    
    window.close();
    return true;
}


function ShowPdfMessage(message) {
    $jQuery("#lblPdfMessage")[0].innerText = message;
    $jQuery("#lblPdfMessage").addClass("info");
    $jQuery("#dvMsgBox").show();
    $jQuery("#dvMsgBox").css("display", "block");
}

function HidePdfMessage() {
    $jQuery("#lblPdfMessage")[0].innerText = "";
    $jQuery("#dvMsgBox").hide();
    $jQuery("#dvMsgBox").css("display", "none");
}