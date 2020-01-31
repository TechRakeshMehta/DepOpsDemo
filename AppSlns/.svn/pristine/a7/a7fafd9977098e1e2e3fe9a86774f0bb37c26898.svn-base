Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };


$jQuery(document).ready(function () {

    showHideButton(false);
});
var selectedFileIndex;
function onClientFileSelected(sender, args) {
    selectedFileIndex = args.get_rowIndex();
}

function onFileUploaded(sender, args) {
    if (sender.getUploadedFiles() != "") {
        showHideButton(true);
    }
}

function onFileRemoved(sender, args) {
    if (sender.getUploadedFiles() == "") {
        showHideButton(false)
    }
}

function showHideButton(visible) {
    if (visible) {
        $jQuery("[id$=btnUpload]").show();
       // $jQuery("[id$=btnCancelUpload]").show();
    }
    else {
        $jQuery("[id$=btnUpload]").hide();
      //  $jQuery("[id$=btnCancelUpload]").hide();
    }
}

function onFileUploadedZeroSize(sender, args) {
    var fileSize = args.get_fileInfo().ContentLength;
    //Added minimum file size check regarding UAT-862: WB: As a student or an admin, I should not be allowed to upload documents with a size of 0
    if (fileSize > 0) {
        if (sender.getUploadedFiles() != "") {
            showHideButton(true);
        }
    }
    else {
        sender.deleteFileInputAt(selectedFileIndex);
        sender.updateClientState();
        alert("File size should be more than 0 byte.");
        return;
    }
}

