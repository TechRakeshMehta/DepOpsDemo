var winopen = false;
function openpopup() {
    var screenname = "Get Started Video";
    var composeScreenWindowName = "Get Started Video";
    var url = $page.url.create("~/ProfileSharing/Pages/GetStartedVideo.aspx?ScreenName=" + screenname);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName });
    winopen = true;
}

window.onload = function () {
    //UAT-2548
    if (Telerik.Web.UI.Calendar != undefined) {
        Telerik.Web.UI.Calendar.RenderDay.prototype.ApplyHoverBehavior = function () { return false; };
    }
}
function OnDateSelected(sender, args) {
    if (args.get_renderDay().IsSelected == false) {
        args.get_renderDay().Select();
    }
}

function VerifyFromDateWithToFilterDate(picker) {

    //Clinical Date Range
    var clinicalDate1 = $jQuery("[id$=dpFromDate]")[0].control.get_selectedDate();
    var clinicalDate2 = $jQuery("[id$=dpToDate]")[0].control.get_selectedDate();
    if (clinicalDate1 != null && clinicalDate2 != null) {
        if (clinicalDate1 > clinicalDate2)
            $jQuery("[id$=dpToDate]")[0].control.set_selectedDate(null);
    }
}

function SetMinFromDateForPieFilter(picker) {
    //Clinical Date 
    var date = $jQuery("[id$=dpFromDate]")[0].control.get_selectedDate();
    if (date != null) {
        picker.set_minDate(date);
    }
    else {
        picker.set_minDate(minDate);
    }
}

function grd_rwDbClick(s, e) {
    var _id = "btnViewDetail";
    var b = e.get_gridDataItem().findControl(_id);
    if (b && typeof (b.click) != "undefined") { b.click(); }
}

//------------------UAT-3628 changes START------------------------------------
function ExportRotation(sender, args) {

    jQuery("[id$=btnExportRotation]")[0].click();
}

function ExportUserGuide(event, sender) {
    Page.showProgress('Processing...');

    var screenname = "Download User Guide";
    var composeScreenWindowName = "Download User Guid";

    var fileName = sender.getAttribute("FileName");
    var documentPath = sender.getAttribute("DocumentPath");
    var dopcumentType = sender.getAttribute("DocumentType");

    if (fileName != "" && documentPath != "") {

        var consolidatedURL = "~/ComplianceOperations/UserControl/DocumentViewer.aspx?FileName=" + fileName + "&DocumentPath=" + documentPath + "&DocumentType=" + dopcumentType;
        var url = $page.url.create(consolidatedURL);
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName });
        win.Close();
    }
    event.stopPropagation();
    Page.hideProgress();

    return false;
}
//------------------UAT-3628 changes END------------------------------------