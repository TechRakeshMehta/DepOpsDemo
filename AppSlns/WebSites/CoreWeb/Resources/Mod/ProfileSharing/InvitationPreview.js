
function closePreview() {
    var arg = {};
    arg.statusId = $jQuery('[id$=hdfSaveType]').val();
    var window = GetPreviewWindow();
    window.Close(arg);
}

function GetPreviewWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}