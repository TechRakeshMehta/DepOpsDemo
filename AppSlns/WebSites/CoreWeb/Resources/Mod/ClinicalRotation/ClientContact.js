/*
MANAGE CLIENT CONTACT and CLIENT CONTACT PROFILE screens.
*/
function SetMinTime(picker) {
    var date = $jQuery("[id$=tpStartTime]")[0].control.get_selectedDate();
    if (date != null) {
        picker.set_minDate(date);
    }
}

function clientFileSelected(sender, args) {
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
            errorMsg = "! Error: File size exceeds 20 MB";
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

function OnClientFileUploading(sender, args) {
    if (args.get_fileName().length > 100) {
        args.set_cancel(true);
    }
    else {
        args.set_cancel(false);
    }
}

function OnClientFileUploaded(sender, args) {
    var fileSize = args.get_fileInfo().ContentLength;
    var completeFileName = args.get_fileName();
    var selectedFileName = "";
    var organizationUserId = "";
    if (completeFileName.indexOf("\\") != -1)
        selectedFileName = completeFileName.substring(completeFileName.lastIndexOf("\\") + 1);
    else
        selectedFileName = completeFileName;

    var hdnOrganizationUserId = $jQuery('[id$="hdfOrganizationUserId"]');
    if (hdnOrganizationUserId != undefined && hdnOrganizationUserId.length > 0) {
        organizationUserId = hdnOrganizationUserId.val();
    }

    //Added minimum file size check regarding UAT-862: WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 byte
    if (fileSize > 0) {
        //To check if duplicate file is uploading
        var isDuplicateFile = false;
        var uploadedFilesCount = $jQuery(sender._uploadedFiles).toArray().length;
        if (uploadedFilesCount > 1) {
            for (var fileindex = 0; fileindex < uploadedFilesCount - 1; fileindex++) {
                if (sender._uploadedFiles[fileindex].fileInfo.FileName == selectedFileName && sender._uploadedFiles[fileindex].fileInfo.ContentLength == fileSize) {
                    isDuplicateFile = true;
                }
            }
        }
        if (isDuplicateFile) {
            sender.deleteFileInputAt(selectedFileIndex);
            isDuplicateFile = false;
            sender.updateClientState();
            alert("You have already updated this document.");
            return;
        }
    }
    else {
        sender.deleteFileInputAt(selectedFileIndex);
        sender.updateClientState();
        alert("File size should be more than 0 byte.");
        return;
    }
}



function openDocumentWithDocumentID(sender) {
    var btnID = sender.id;
    var containerID = btnID.substr(0, btnID.indexOf("lbRotationDocument"));
    var TenantId = $jQuery("[id$=hfTenantId]").val();
    var hdnfOrderID = $jQuery("[id$=" + containerID + "hdnfOrderID]").val();
    var documentType = "RotationDocument";
    var reportType = "OrderCompletion";
    var composeScreenWindowName = "Report Detail";
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?OrderID=" + hdnfOrderID + "&DocumentType=" + documentType + "&ReportType=" + reportType + "&tenantId=" + TenantId);
    var win = $window.createPopup(url, { size: "800,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
    winopen = true;
    return false;
}

function DownloadForm(url) {
    //debugger;
    location.href = url;
}

function ValidateAvailableDays(source, args) {
    var cntrlToValidate = $find($jQuery("[id$=cmbAvailableDays]").attr("id"));
    var check = 0;
    if (cntrlToValidate) {
        var cntrlItems = cntrlToValidate.get_items();
        for (var i = 0; i <= cntrlItems.get_count() - 1; i++) {
            var cntrlItem = cntrlItems.getItem(i);
            if (cntrlItem.get_checked()) {
                check = 1;
            }
        }
    }
    if (check)
        args.IsValid = true;
    else
        args.IsValid = false;
}