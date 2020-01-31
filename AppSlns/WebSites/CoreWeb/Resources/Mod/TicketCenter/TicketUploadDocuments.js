//Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
//Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };

//var hdnIsUploadValidationSuccess = true;

$jQuery('div#cmbItems').hide();
$jQuery(document).ready(function () {
    HideDropDown();
    showHideButton(false);
});

function HideDropDown() {
    $jQuery('div#cmbItems').hide();
}

//function onFileUploaded(sender, args) {
//    if (sender.getUploadedFiles() != "") {
//        showHideButton(true);
//    }
//}

var uploader;
var selectedFileIndex;

function clientFileSelected(sender, args) {
    uploader = sender;
    selectedFileIndex = args.get_rowIndex();

    var fileInputs = sender._selectedFilesCount;
    var completeFileName = args.get_fileName();
    var selectedFileName = "";

    if (completeFileName.indexOf("\\") != -1)
        selectedFileName = completeFileName.substring(completeFileName.lastIndexOf("\\") + 1);
    else
        selectedFileName = completeFileName;

}

function onClientFileSelectedDandADocument(sender, args) {
    selectedFileIndex = args.get_rowIndex();
}

function onClientFileUploaded(radAsyncUpload, args) {
    var documentAssociationEnabled = $jQuery("[id$=hdnDocumentAssociationSettingEnabled]");
    var row = args.get_row(),
 inputName = radAsyncUpload.getAdditionalFieldID("TextBox"),
 inputType = "text",
 inputId = inputName,
 input = createInputControl(inputType, inputId, inputName),
    //label = createInputLabel(inputId),
     br = document.createElement("br");
    //Code changes for UAT 2128- Added functionality to map the document with items in the category
    //Drop down will be shown when Document Association setting is enable.
    if (documentAssociationEnabled.val() == "1") {
        var inputHiddenType = "hidden",
            inputHidden = radAsyncUpload.getAdditionalFieldID("HiddedFeild"),
             inputName2 = radAsyncUpload.getAdditionalFieldID("DropDown");
        var HiddenFeild = createInputHiddenFeild(inputHiddenType, inputHidden, inputHidden),
            dropdown = CreateSelect(inputName2, inputName2, inputHidden);
    }

    var IgnoreAlreadyUploadedDoc = false;
    var hdfIgnoreAlreadyUploadedDoc = $jQuery('[id$="hdfIgnoreAlreadyUploadedDoc"]');
    if (hdfIgnoreAlreadyUploadedDoc != undefined && hdfIgnoreAlreadyUploadedDoc.length > 0) {
        IgnoreAlreadyUploadedDoc = hdfIgnoreAlreadyUploadedDoc.val();
    }

    var organizationUserId = "";
    var hdnOrganizationUserId = $jQuery('[id$="hdfOrganizationUserId"]');
    if (hdnOrganizationUserId != undefined && hdnOrganizationUserId.length > 0) {
        organizationUserId = hdnOrganizationUserId.val();
    }

    //row.appendChild(br);
    //row.appendChild(label);
    //row.appendChild(input);

    //Code changes for UAT 2128- Added functionality to map the document with items in the category
    //Drop down will be shown when Document Association setting is enable.
    if (documentAssociationEnabled.val() == "1") {
        row.appendChild(HiddenFeild);
        row.appendChild(dropdown);
    }

    if (radAsyncUpload.getUploadedFiles() != "") {
        showHideButton(true);
    }
    //Code changes for UAT 531- As a student, I should not be able to upload a duplicate document.
    var fileSize = args.get_fileInfo().ContentLength;
    var completeFileName = args.get_fileName();
    var selectedFileName = "";

    if (completeFileName.indexOf("\\") != -1)
        selectedFileName = completeFileName.substring(completeFileName.lastIndexOf("\\") + 1);
    else
        selectedFileName = completeFileName;

    //Added minimum file size check regarding UAT-862: WB: As a student or an admin, I should not be allowed to upload documents with a size of 0

    //Changes made by Sagar Arora on 24 May 2018
    //System will automatically remove the uploaded file if the file has already been uploaded. 
    //System will delete the duplicate file uploaded.
    //Bug 19595 - CUSN-OMS (.19):Sprint-9 HF9.1: >>Business Setup>>Manage Service Setup – User is able to upload duplicate documents. Also, ''Browse'' button is getting disappeared once duplicate documents uploaded on ''Manage Service Setup'' screen. 

    if (fileSize > 0) {
        //To check if duplicate file is uploading
        var isDuplicateFile = false;
        var uploadedFilesCount = $jQuery(radAsyncUpload._uploadedFiles).toArray().length;
        if (uploadedFilesCount > 1) {
            for (var fileindex = 0; fileindex < uploadedFilesCount - 1; fileindex++) {
                if (radAsyncUpload._uploadedFiles[fileindex].fileInfo.FileName == selectedFileName && radAsyncUpload._uploadedFiles[fileindex].fileInfo.ContentLength == fileSize) {
                    isDuplicateFile = true;
                    break;
                }
            }
            //Set Dirty flag 
            //parent.parent.globalDirty.screenDirtyByFields = true;
        }
        if (isDuplicateFile) {
            //radAsyncUpload.deleteFileInputAt(currentFileIndex);
            $telerik.$(".ruRemove", args.get_row()).click();
            isDuplicateFile = false;
            radAsyncUpload.updateClientState();
            alert("You have already updated this document.");
            return;
        }
        //Check if document is already uploaded.
       // debugger;
        if (IgnoreAlreadyUploadedDoc == false) {
            var isPersonalDocumentScreen = parseInt($jQuery("[id$=hdnSelectedTab]").val()) == 1 ? false : true;
            //PageMethods.IsDocumentAlreadyUploaded(selectedFileName, fileSize, organizationUserId, isPersonalDocumentScreen, checkCallBack);
        }
    }
    else {
        radAsyncUpload.deleteFileInputAt(selectedFileIndex);
        isDuplicateFile = false;
        radAsyncUpload.updateClientState();
        alert("File size should be more than 0 byte.");
    }
    return;
}

//callBack after the check has been done
function checkCallBack(result) {
    if (result) {
        alert('You have already updated this document.');
        uploader.deleteFileInputAt(selectedFileIndex);
        uploader.updateClientState();
    }
}

function createInputControl(inputType, inputId, inputName) {

    var inputFileUplpoad = document.createElement("input");
    inputFileUplpoad.setAttribute("type", inputType);
    inputFileUplpoad.setAttribute("id", inputId);
    inputFileUplpoad.setAttribute("name", inputName);
    inputFileUplpoad.maxLength = 500;
    return inputFileUplpoad;
}

//Code changes for UAT 2128- create a label with css as a combobox.
function CreateSelect(inputId, inputname, inputHiddenid) {
    var inputbuttonControl = document.createElement("label");
    inputbuttonControl.innerHTML = 'Select Requirement(s)';
    inputbuttonControl.setAttribute("id", inputId);
    inputbuttonControl.setAttribute("name", inputname);
    inputbuttonControl.style.border = "1px solid #ccc";
    inputbuttonControl.style.height = "28px";
    inputbuttonControl.style.marginBottom = "5px";
    inputbuttonControl.style.lineHeight = "28px";
    inputbuttonControl.style.verticalAlign = "middle";
    inputbuttonControl.style.background = "linear-gradient(to bottom, #e6e6e6 0%,#ffffff 100%)";
    inputbuttonControl.style.marginLeft = "20px";
    inputbuttonControl.style.width = "40%";
    inputbuttonControl.style.textAlign = "left";
    inputbuttonControl.style.fontSize = "13px";
    inputbuttonControl.style.borderRadius = "4px";
    inputbuttonControl.style.fontFamily = "Titillium Web , sans-serif";
    inputbuttonControl.setAttribute("class", "rcbArrowCell");
    inputbuttonControl.setAttribute("onclick", "callComboBox('" + inputHiddenid + "','" + inputId + "')");
    return inputbuttonControl;
}

//Code changes for UAT 2128- create a hideen field to save the selected dropdown items id.
function createInputHiddenFeild(inputType, inputId, inputName) {
    var inputhidden = document.createElement("input");
    inputhidden.setAttribute("type", inputType);
    inputhidden.setAttribute("id", inputId);
    inputhidden.setAttribute("name", inputName);
    return inputhidden;
}

//Code changes for UAT 2128- call the combo box and show its drop down list under the label.
function callComboBox(sender, args) {
    var cbmitems = $find($jQuery("[id$=cmbItems]")[0].id);
    var dropdownlabel = $jQuery("[id$=" + args + "]")[0];
    cbmitems.set_offsetY(dropdownlabel.getClientRects()['0'].top + 28);
    cbmitems.set_offsetX(dropdownlabel.offsetLeft + 80);
    cbmitems._dropDownWidth = dropdownlabel.offsetWidth;


    cbmitems.trackChanges();
    if (window.hidden_id != sender) {
        window.hidden_id = sender;
        window.hidden1_id = args;


        var items = $jQuery('#' + window.hidden_id).val();
        cbmitems.get_checkedItems().forEach(function (item) { item.set_checked(false); });
        items.split(',').forEach(function (val) {
            var item = cbmitems.findItemByValue(val);
            if (item != null) { if (!item.get_enabled()) { item.enable(); } item.set_checked(true); }
        });
        enabledisableitems();
    }
    cbmitems.showDropDown();
    cbmitems.commitChanges();
}
function OnDropdownClosed(sender, eventArgs) {
    var idlist = '';
    //var idtext = '';
    var cbmitems = $find($jQuery("[id$=cmbItems]")[0].id);
    cbmitems.get_checkedItems().forEach(function (item) {

        idlist = idlist.concat(item.get_value(), ',');
        // idtext += idtext.concat(item.get_text(), ',');
    });
    $jQuery('#' + window.hidden_id).val(idlist.substr(0, idlist.length - 1));
    var dropdownlabel = $jQuery("[id$=" + window.hidden1_id + "]")[0];
    if (cbmitems.get_checkedItems().length > 0) {
        dropdownlabel.innerHTML = cbmitems.get_checkedItems().length + ' item selected';
    }
    else {
        dropdownlabel.innerHTML = 'Select Requirement(s)';
    }
}
function enabledisableitems() {
    // return;
    var exceptions = [];
    var cbmitems = $find($jQuery("[id$=cmbItems]")[0].id);
    cbmitems.get_items().forEach(function (item) {
        if (item.get_value().indexOf('_0') >= 0) {
            exceptions[item.get_value()] = item.get_checked();
        }
        if (item.get_value().indexOf('_-1') >= 0 || item.get_value().indexOf('_0') >= 0) return;
        var excvalue = item.get_value().substring(0, item.get_value().indexOf('_')).concat('_0');

        if (exceptions[excvalue] == undefined) {
            var items = cbmitems.findItemByValue(excvalue);
            if (items != undefined) {
                exceptions[excvalue] = cbmitems.findItemByValue(excvalue).get_checked();
            }
            else {
                exceptions[excvalue] = false;

            }
        }

        if (exceptions[excvalue] == true) {
            if (item.get_enabled()) {
                item.set_checked(false);
                item.disable();
            }
        }
        else {
            item.enable();
        }

    });

}
function OnClientItemChecked(sender, eventArgs) {
    enabledisableitems();
}

function createInputLabel(attr) {
    var inputLabel = document.createElement("label");
    inputLabel.setAttribute("for", attr);
    inputLabel.innerHTML = "Description: ";
    return inputLabel;
}

function OnFileRemoving(sender, args) {
}

function onFileRemoved(sender, args) {
    if ($jQuery('.ruError').length == 0) {
       ValidatorEnable($jQuery("[id$=cvUploadFile]")[0], false);
        $jQuery("[id$=cvUploadFile]").text("");
    }
    if (sender.getUploadedFiles() == "") {
        showHideButton(false)
    }
}

function showHideButton(visible) {
    if (visible) {
        $jQuery("[id$=btnUploadAll]").show();
        $jQuery("[id$=btnUploadCancel]").show();
    }
    else {
        $jQuery("[id$=btnUploadAll]").hide();
        $jQuery("[id$=btnUploadCancel]").hide();
    }
}

//var upl_OnClientValidationFailed = function (s, a) {
//    // var error = false;
//    debugger;
//    var errorMsg = "";
//    var extn = a.get_fileName().substring(a.get_fileName().lastIndexOf('.') + 1, a.get_fileName().length);
//    if (a.get_fileName().lastIndexOf('.') != -1) {
//        if (s.get_allowedFileExtensions().indexOf(extn) == -1) {
//            ValidatorEnable($jQuery("[id$=cvUploadFile]")[0], true);
//            errorMsg = errorMsg + " File Format is not correct."
//            $jQuery("[id$=cvUploadFile]").text(errorMsg);
//        }
//        else {
//            ValidatorEnable($jQuery("[id$=cvUploadFile]")[0], true);
//            errorMsg = errorMsg + " File Size should be less than 25 MB."
//            $jQuery("[id$=cvUploadFile]").text(errorMsg);
//        }
//    }
//    else {
//        ValidatorEnable($jQuery("[id$=cvUploadFile]")[0], true);
//        errorMsg = errorMsg + " File Format is not correct."
//        $jQuery("[id$=cvUploadFile]").text(errorMsg);
//    }
//}


var upl_OnClientValidationFailed = function (sender, args) {
    ValidatorEnable($jQuery("[id$=cvUploadFile]")[0], true);
    var $row = $jQuery(args.get_row());
    var erorMessage = getErrorMessage(sender, args);
    var span = createError(erorMessage);
    $row.addClass("ruError");
    $row.append(span);
}

function getErrorMessage(sender, args) {
    var fileExtention = args.get_fileName().substring(args.get_fileName().lastIndexOf('.') + 1, args.get_fileName().length);
    if (args.get_fileName().lastIndexOf('.') != -1) {//this checks if the extension is correct
        if (sender.get_allowedFileExtensions().indexOf(fileExtention) == -1) {
            return ("File Format is not correct.");
        }
        else {
            return ("File Size should be less than 25 MB.");
        }
    }
    else {
        return ("File Format is not correct.");
    }
}

function createError(erorMessage) {
    var input = '<span class="ruErrorMessage">' + erorMessage + ' </span>';
    return input;
}


function OnClientFileUploading(sender, args) {
    if (!isValid(args.get_fileName())|| args.get_fileName().length > 100) {
        args.set_cancel(true);
    }
    else {
        args.set_cancel(false);
    }
}
function isValid(fname) {
    var rg1 = /^[^\\/:\*\?"<>\|]+$/; // forbidden characters \ / : * ? " < > |
    var rg2 = /^\./; // cannot start with dot (.)
    var rg3 = /^(nul|prn|con|lpt[0-9]|com[0-9])(\.|$)/i; // forbidden file names
    return rg1.test(fname) && !rg2.test(fname) && !rg3.test(fname);
}
function ShowCallBackMessage(docMessage) {
    if (docMessage != '') {
        alert(docMessage);
    }
}

function validateUpload(sender, args) {
    if ($jQuery('.ruError').length > 0) {
        args.IsValid = false;
    }
    else {
        args.IsValid = true;
    }
}