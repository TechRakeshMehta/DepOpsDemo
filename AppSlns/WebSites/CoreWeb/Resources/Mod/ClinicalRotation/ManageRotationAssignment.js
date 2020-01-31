$page.add_pageLoaded(function () {
    if (Telerik.Web.UI.RadAsyncUpload != null && Telerik.Web.UI.RadAsyncUpload != undefined) {
        Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
        Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };
    }
});

var selectedFileIndex;
function onFileUploaded(sender, args) {
    var fileSize = args.get_fileInfo().ContentLength;
    //Added minimum file size check regarding UAT-862: WB: As a student or an admin, I should not be allowed to upload documents with a size of 0
    if (fileSize > 0) {
        if (sender.getUploadedFiles() != "") {
            $jQuery("[id$=btnUpload]").click();
        }
    }
    else {
        sender.deleteFileInputAt(selectedFileIndex);
        sender.updateClientState();
        alert("File size should be more than 0 byte.");
        return;
    }
}
function onClientFileSelected(sender, args) {
    selectedFileIndex = args.get_rowIndex();
}

var upl_OnClientValidationFailed = function (s, a) {
    var error = false;
    var errorMsg = "";

    var extn = a.get_fileName().substring(a.get_fileName().lastIndexOf('.') + 1, a.get_fileName().length);

    if (a.get_fileName().lastIndexOf('.') != -1) {
        if (s.get_allowedFileExtensions().indexOf(extn) == -1) {
            error = true;
            errorMsg = "! Error: Unsupported File Format";
        }
        else {
            error = true;
            errorMsg = "! Error: File size exceeds 5MB";
        }
    }
    else {
        error = true;
        errorMsg = "! Error: Unrecognized File Format";
    }

    if (error) {
        var row = a.get_row();
        smsg = document.createElement("span");

        smsg.innerHTML = errorMsg;
        smsg.setAttribute("class", "ruFileWrap");
        smsg.setAttribute("style", "color:red;padding-left:10px;");

        row.appendChild(smsg);
    }
}

function ValidateCheckbox(sender, args) {
    var checkedItems = $jQuery("[id$= ddlInstructor]")[0].control.get_checkedItems();
    if (checkedItems.length > 0) {
        args.IsValid = true;
        return false;
    }
    args.IsValid = false;
}

function ValidateUploadControl(sender, args) {
    var inputFiles = $find($jQuery("[id$= uploadControl]")[0].id).getUploadedFiles();
    if (inputFiles.length > 0) {
        args.IsValid = true;
        return false;
    }
    args.IsValid = false;
}
