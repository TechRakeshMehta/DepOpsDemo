


var IsDocViewed = false;
//Function to redirect to parent 
function returnToParent() {
    if ($jQuery("[id$=hdnIsDocViewed]").val() == 'True')
    {
        IsDocViewed = true;
    }
    var fileTemporaryPath = $jQuery("[id$=hfTemporaryApplicantDocPath]").val();
    var fileName = $jQuery("[id$=hdnFileName]").val();
    var oArg = {};
    oArg.Action = "Submit";
    oArg.IsDocViewed = IsDocViewed;
    oArg.fileTemporaryPath = fileTemporaryPath;
    oArg.fileName = fileName;
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



