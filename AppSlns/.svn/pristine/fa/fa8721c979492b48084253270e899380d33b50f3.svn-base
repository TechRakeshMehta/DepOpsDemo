$jQuery = $telerik.$;
var timeoutId = 0;

$jQuery(document).ready(function () {
    if ($jQuery("[id$=hdnIsAdminRequested]").val() != "True") {
        if ($jQuery("[id$=hdnIsReqToOpen]").val() == "True") {
            CheckVideoClose();
        }
        StartCountDown();
    }
});

function CheckVideoClose() {
    //Commented below code for UAT-1470 :As a student, there should be a way to close out of the video once you open it.
    //if ($jQuery("[id$=hdnBoxStayOpenTime]").val() == "0") {
    //    $jQuery("[id$=btnCloseViewVideo]").show();
    //}
    //else {
    //    $jQuery("[id$=btnCloseViewVideo]").hide();
    //    var videoOpenTimeDuration = parseFloat(parseFloat($jQuery("[id$=hdnBoxStayOpenTime]").val()) * 1000);
    //    window.setTimeout(function () {
    //        $jQuery("[id$=btnCloseViewVideo]").show();
    //    }, videoOpenTimeDuration);
    //}
    $jQuery("[id$=btnCloseViewVideo]").show();
}

function StartCountDown() {
    setInterval('Tick()', 1000);
}

function Tick() {
    timeoutId++;
    $jQuery("[id$=hdnVideoCurrentPlaybackTimed]").val(timeoutId);
}

//Check id user has viewd the video for necessary time duration
function OnViewVideoPopupWndClosed() {
    ReturnToParent();
}

//Function to redirect to parent 
function ReturnToParent() {
    var hdnVideoRequiredOpenTime = $jQuery("[id$=hdnVideoRequiredOpenTime]");
    var hdnIsEditMode = $jQuery("[id$=hdnIsEditMode]");
    var oArg = {};
    if ($jQuery("[id$=hdnIsAdminRequested]").val() != "True") {
        oArg.Action = "Submit";
        if (timeoutId >= parseInt(hdnVideoRequiredOpenTime.val()) || $jQuery("[id$=hdnIsReqToOpen]").val() == "False" || hdnIsEditMode.val() == "true") {
            oArg.IsVideoViewed = true;
        }
        else { oArg.IsVideoViewed = false; }
        oArg.VideoCurrentPlaybackTime = timeoutId;
    }
    var oWnd = GetRadWindow();
    oWnd.Close(oArg);
}

//function to get current popup window
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}