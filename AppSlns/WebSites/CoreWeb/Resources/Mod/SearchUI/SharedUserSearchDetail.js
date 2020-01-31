
///-------------------------------------------------------------------------------------------------
///  THIS JAVASCRIPT FILE CONTAINS FUNCTIONS REQUIRED ON SHAREDUSERSEARCHDETAILS SCREEN (UAT-1237).
///-------------------------------------------------------------------------------------------------

//Function to download Attestation Report document
function DownloadAttestationDocument(sender) {
    //debugger;
    var btnID = sender.id;
    var containerID = btnID.substr(0, btnID.indexOf("btnAttestationDocument"));
    var btnAttestationDocument = $jQuery(sender).closest("#dvAttestationDoc").find("[id*=" + containerID + "btnAttestationDocument]")[0];
    var invitationid = btnAttestationDocument.attributes["invitationid"].value;
    if (invitationid == "" || invitationid == null || invitationid <= 0) {
        DisplayError();
    }
    else {
        var documentType = "ViewAttestationForAdmin"; //THIS CAN NOT BE CHANGED AS IT REQUIRED IN DOCUEMNTVIEWER
        var url = "../../../ComplianceOperations/UserControl/DocumentViewer.aspx?ProfileSharingInvitationID=" + invitationid + "&DocumentType=" + documentType;
        if ($jQuery("[id$=ifrExportDocument]") != undefined && $jQuery("[id$=ifrExportDocument]").length > 0) {
            $jQuery("[id$=ifrExportDocument]")[0].src = url;
            $jQuery("[id$=pageMsgBox]")[0].style.display = "none";
        }
    }
    return false;
}

//Function to display error message.
function DisplayError() {
    //debugger;
    $jQuery("[id$=pageMsgBox]")[0].style.display = "block";
    $jQuery("[id$=lblError]")[0].innerHTML = "Attestation Document is not found for this invitation.";
    return false;
}

var winopen = false;

//Function to open Result report for service group
function ShowReportResultForServiceGroup(urlString) {
    var composeScreenWindowName = "Filterd Report Detail";
    var url = $page.url.create(urlString);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url, { size: "800,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientCloseReportResult }
         , function () {
             this.set_title("Report Document");
         });
    winopen = true;
    return false;
}

//Function called when result report popup closed.
function OnClientCloseReportResult(oWnd, args) {
    oWnd.get_contentFrame().src = ''; //This is added for fixing pop-up close issue in Safari browser.
    oWnd.remove_close(OnClientCloseReportResult);
    if (winopen) {
        winopen = false;
    }
}

//Function to open Passport report popup
function OpenPassportReportPopup(urlString) {
    var composeScreenWindowName = "Passport Report";
    var url = $page.url.create(urlString);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var doc_win = $window.createPopup(url, { size: "760,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move }, function () { this.set_title(composeScreenWindowName); this.set_destroyOnClose(true); });
    return false;
}