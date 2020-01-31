if (Telerik.Web.UI.RadAsyncUpload != undefined) {
    Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
    Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };
}

function showHideBrowseButton() {

    if ($jQuery(".dvApplicantDocumentDropzone").length > 0) {

        $jQuery(".RadUpload .ruBrowse[value='Hidden']").remove();
        $jQuery(".ruButton .ruBrowse[value='Hidden']").remove();
        $jQuery(".ruDropZone").remove();
        /*var dropZone1 = $jQuery(".ruDropZone");
        $jQuery(document).bind({
            "dragstart": function (e) {
                $jQuery(".ruDropZone").remove();
                e = e || event;
                e.preventDefault(); e.stopPropagation();
                //e.stopImmediatePropagation();
                //e.cancelBubble = true;
                return false;
            }
        });
        dropZone1.bind({
            "drag": function (e) {
                $jQuery(".ruDropZone").remove();
                e = e || event;
                e.preventDefault(); e.stopPropagation();
                //e.stopImmediatePropagation();
                //e.cancelBubble = true;
                return false;
            }
        });
        dropZone1.bind({
            "dragenter": function (e) {
                $jQuery(".ruDropZone").remove();
                e = e || event;
                e.preventDefault();
                e.stopPropagation();
                //e.stopImmediatePropagation();
                //e.cancelBubble = true;
                return false;
            }
        })
               .bind({
                   "dragleave": function (e) {
                       $jQuery(".ruDropZone").remove();
                       e = e || event;
                       e.preventDefault(); e.stopPropagation();
                       //e.stopImmediatePropagation();
                       //e.cancelBubble = true;
                       $jQuery(".ruDropZone").remove(); return false;
                   }
               })
               .bind({
                   "drop": function (e) {
                       $jQuery(".ruDropZone").remove();
                       e = e || event;
                       e.preventDefault(); e.stopPropagation();
                       //e.stopImmediatePropagation();
                       //e.cancelBubble = true;
                       $jQuery(".ruDropZone").remove(); return false;
                   }
               });
        $jQuery(".ruDropZone").remove();
        */
    }
}

function SetDropZone() {
    window.addEventListener("dragover", function (e) {
        e = e || event;
        e.preventDefault();
    }, false);
    window.addEventListener("drop", function (e) {
        e = e || event;
        e.preventDefault();
    }, false);
    var dropZone1 = $jQuery(".dvApplicantDocumentDropzone");
    if (dropZone1.length == 0) return;
    showHideBrowseButton();
    if (!Telerik.Web.UI.RadAsyncUpload.Modules.FileApi.isAvailable()) {
        var dropZone1 = $jQuery(".dvApplicantDocumentDropzone").innerHtml("<strong>Your browser does not support Drag and Drop. Please take a look at the info box for additional information.</strong>");
    }
    else {
        $jQuery(document).bind({ "drop": function (e) { e.preventDefault(); } });
        dropZone1.bind({ "dragenter": function (e) { dragEnterHandler(e, dropZone1); } })
            .bind({ "dragleave": function (e) { dragLeaveHandler(e, dropZone1); } })
            .bind({ "drop": function (e) { dropHandler(e, dropZone1); } });
    }
};

function dropHandler(e, dropZone1) {
    dropZone1[0].style.backgroundColor = "#ffffff";
}

function dragEnterHandler(e, dropZone1) {
    dropZone1[0].style.backgroundColor = "#f0f0f6";
}

function dragLeaveHandler(e, dropZone1) {
    dropZone1[0].style.backgroundColor = "#ffffff";
}


$jQuery('div#cmbItems').hide();
$jQuery(document).ready(function () {
    HideDropDown();
    showHideButton(false);
    //$jQuery('[id$=btnChkFileName]').attr('style', 'display:none;');
});

$page.add_pageLoad(function () {
    SetDropZone();
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
    showHideBrowseButton();
    uploader = sender;
    selectedFileIndex = args.get_rowIndex();

    var fileInputs = sender._selectedFilesCount;
    var completeFileName = args.get_fileName();
    var selectedFileName = "";

    if (completeFileName.indexOf("\\") != -1)
        selectedFileName = completeFileName.substring(completeFileName.lastIndexOf("\\") + 1);
    else
        selectedFileName = completeFileName;

    // REGION UAT - 4270 START
    if (selectedFileName != "" || selectedFileName != undefined) {

        var hdnPCN = $jQuery("[id$=hdnPCN]");
        if (hdnPCN != undefined && hdnPCN.length > 0) {
            if (hdnPCN.val() != null && hdnPCN.val() != "" && selectedFileName.split('.')[0] != hdnPCN.val()) {
                $jQuery("[id$=hdnIsValidPCNFile]").val(1);
               // upl_OnClientValidationFailed(sender, args);
                //var row = args.get_row();
                //smsg = document.createElement("span");
                //smsg.innerHTML = "Please provide the file with correct PCN Name.";
                //smsg.setAttribute("class", "ruFileWrap");
                //smsg.setAttribute("style", "color:red;padding-left:10px;");
                //row.appendChild(smsg);
                return false;
            }
        }
        
        //$jQuery(".chkFileName").trigger("click");
       

    }
    // REGION UAT - 4270 END

}

function onClientFileSelectedDandADocument(sender, args) {
    selectedFileIndex = args.get_rowIndex();

}

function onClientFileUploaded(radAsyncUpload, args) {
    showHideBrowseButton();
    var documentAssociationEnabled = $jQuery("[id$=hdnDocumentAssociationSettingEnabled]");
    var row = args.get_row(),
        inputName = radAsyncUpload.getAdditionalFieldID("TextBox"),
        inputType = "text",
        inputId = inputName,
        input = createInputControl(inputType, inputId, inputName),
        label = createInputLabel(inputId),
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


    row.appendChild(br);
    if (label != undefined)
        row.appendChild(label);
    if (input != undefined)
        row.appendChild(input);


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

    // region UAT - 4270 Start
    if ($jQuery("[id$=hdnIsValidPCNFile]").val() == "1") {
        radAsyncUpload.deleteFileInputAt(selectedFileIndex);
        radAsyncUpload.updateClientState();
        showHideBrowseButton();
        alert("Please select the file with same PCN.");
        $jQuery("[id$=hdnIsValidPCNFile]").val("0");
        showHideBrowseButton();
        return; 
    }
    // region UAT - 4270 END
    
    //Added minimum file size check regarding UAT-862: WB: As a student or an admin, I should not be allowed to upload documents with a size of 0
    if (fileSize > 0) {
        //To check if duplicate file is uploading
        var isDuplicateFile = false;
        var uploadedFilesCount = $jQuery(radAsyncUpload._uploadedFiles).toArray().length;
        if (uploadedFilesCount > 1) {
            for (var fileindex = 0; fileindex < uploadedFilesCount - 1; fileindex++) {
                if (radAsyncUpload._uploadedFiles[fileindex].fileInfo.FileName == selectedFileName && radAsyncUpload._uploadedFiles[fileindex].fileInfo.ContentLength == fileSize) {
                    isDuplicateFile = true;
                }
            }
        }
        if (isDuplicateFile) {
            radAsyncUpload.deleteFileInputAt(selectedFileIndex);
            isDuplicateFile = false;
            radAsyncUpload.updateClientState();
            showHideBrowseButton();
            alert("You have already updated this document.");
            showHideBrowseButton();
            return;
        }
        //Check if document is already uploaded.
        if (IgnoreAlreadyUploadedDoc == false) {
            var isPersonalDocumentScreen = parseInt($jQuery("[id$=hdnSelectedTab]").val()) == 1 ? false : true;
            PageMethods.IsDocumentAlreadyUploaded(selectedFileName, fileSize, organizationUserId, isPersonalDocumentScreen, checkCallBack);
        }
    }
    else {
        radAsyncUpload.deleteFileInputAt(selectedFileIndex);
        isDuplicateFile = false;
        radAsyncUpload.updateClientState();
        alert("File size should be more than 0 byte.");
    }
    showHideBrowseButton();
    return;
}

//callBack after the check has been done
function checkCallBack(result) {
    if (result) {
        showHideBrowseButton();
        alert('You have already updated this document.');
        uploader.deleteFileInputAt(selectedFileIndex);
        uploader.updateClientState();
        showHideBrowseButton();
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
    showHideBrowseButton();
    enabledisableitems();
}

function createInputLabel(attr) {
    var inputLabel = document.createElement("label");
    inputLabel.setAttribute("for", attr);
    inputLabel.innerHTML = "Description: ";
    return inputLabel;
}

function onFileRemoved(sender, args) {
    
    showHideBrowseButton();
    if (sender.getUploadedFiles() == "") {
        showHideButton(false)
    }
    window.setTimeout(function () { showHideBrowseButton(); }, 0);
    window.setTimeout(function () { showHideBrowseButton(); }, 100);
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

var upl_OnClientValidationFailed = function (s, a) {
    var error = false;
    var errorMsg = "";
    var extn = a.get_fileName().substring(a.get_fileName().lastIndexOf('.') + 1, a.get_fileName().length);
    $jQuery("[id$=hdnIsValidPCNFile]").val("0");
    if (a.get_fileName().lastIndexOf('.') != -1) {
        if (s.get_allowedFileExtensions().indexOf(extn) == -1) {
            error = true;
            errorMsg = "Error: Unsupported File Format";
        }
        else {
            error = true;
            errorMsg = "Error: File size exceeds 20 MB";
        }
    }
    else {
        error = true;
        errorMsg = "Error: Unrecognized File Format";
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
    
    
    if (!isValid(args.get_fileName())||args.get_fileName().length > 100  ) {
        args.set_cancel(true);

        //UAT-3181
        var row = args.get_row();
        smsg = document.createElement("span");
        smsg.innerHTML = "Document name is not valid. Please make sure document name should not exceed 100 characters and should not contain special character(s).";
        smsg.setAttribute("class", "ruFileWrap");
        smsg.setAttribute("style", "color:red;padding-left:10px;");
        row.appendChild(smsg);
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

function OnAttestationFormUploaded(radAsyncUpload, args) {
    var fileSize = args.get_fileInfo().ContentLength;
    if (fileSize > 0) {
        //To check if duplicate file is uploading 
    }
    else {
        radAsyncUpload.deleteFileInputAt(selectedFileIndex);
        isDuplicateFile = false;
        radAsyncUpload.updateClientState();
        alert("File size should be more than 0 byte.");
    }
    return;
}