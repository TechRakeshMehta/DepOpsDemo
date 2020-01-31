//Global variable for this js 
var _ReqItemFieldId = null;


$page.add_pageLoaded(function () {
    if (Telerik.Web.UI.RadAsyncUpload != null && Telerik.Web.UI.RadAsyncUpload != undefined) {
        Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
        Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };
    }
});

$page.add_pageLoad(function () {

    //Attribute details viewer
    $jQuery(".dtl_viewer").click(function () {
        var e = $jQuery(this).attr("_p");
        var f = $jQuery(this).attr("_n");
        var a = $jQuery(this).attr("_a");
        var i = $jQuery(this).attr("_i");
        var documentDescription = $jQuery('[id$=documentDescription' + e + '-' + f + ']');
        if (documentDescription[0] != undefined && documentDescription[0].style.display != "none") {
            // documentHeader.hide();
            documentDescription.hide();
        }
        else {
            // documentHeader.show();
            //documentDescription.show();
            $jQuery(this).parents("tr").first().addClass("selected");
            var content = documentDescription.clone();
            if (content.length > 0) {
                $jQuery(content).show().css({ "width": "100%", "background-color": "#fff" }).find("td.td-two, td.td-three").css({ padding: "5px", border: "1px solid black" });
                //                    var a = $jQuery(s).find("td.td-two:first").text();
                var a = "<div style='margin-bottom:10px'><div style='font-size:11px;font-weight:bold'>Attached to</div>" +
                    "<div style='padding-left:20px;'>" + i + "</div>" +
                    "<div style='padding-left:30px;color:#8C1921'>" + a + "</div></div>";
                $jQuery(content).wrap("<div class='doc-wrapper' style='padding:10px;background-color:#f0f0f0;overflow:auto;max-height:400px;'></div>");
                $window.showDialog($jQuery(content).parent('.doc-wrapper').prepend(a), { btnClose: { autoclose: true, "text": "Close" } }, 500, "Showing documents");
            }
        }
    });
});


//Event handler for on load event of File Upload Combo
function efn_atrFileUpdOnLoad(a, b) {

    if (a) {
        var lst = a.get_items();
        var mxLen = 50;
        if (lst) {
            for (var i = 0; i < lst.get_count(); i++) {
                var itm = lst.getItem(i);
                if (itm) {
                    var desc = itm.get_attributes().getAttribute("desc");
                    var m_desc = $jQuery("<p></p>");

                    $jQuery(m_desc).css({ "color": "#777", "margin": "0px auto 7px 22px", "font-style": "italic" });
                    $jQuery(m_desc).click(function () {
                        $jQuery(this).prev(".rcbCheckBox").click();
                    });

                    if (desc != undefined && desc.length > mxLen) {
                        $jQuery(m_desc).text(desc.substring(0, mxLen) + "...");
                        $jQuery(itm.get_element()).attr("title", desc).find("label:first").append(m_desc);
                    }
                    else {
                        $jQuery(m_desc).text(desc);
                        $jQuery(itm.get_element()).find("label:first").append(m_desc);
                    }

                }
            }
        }
    }
}

function setPostBackSourceDE(event, sender) {
    $jQuery('.postbacksource').val('DE');
    window.DashboardChildClick = 1;
}

var uploader;
var selectedFileIndex;

function ReqClientFileSelected(sender, args) {
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
    //TODO: Commneted for Cilinical rotation
    //Code changes for UAT 531- As a student, I should not be able to upload a duplicate document.
    //PageMethods.IsDocumentAlreadyUploaded(selectedFileName, checkCallBack);
}

function OnClientFileUploadingReq(sender, args) {
    if (args.get_fileName().length > 100) {
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

function OnClientFileUploadedReq(sender, args) {
    showHideBrowseButton();
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

        showHideBrowseButton();
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
        //TODO: Commneted for Cilinical rotation
        //Check if document is already uploaded.
        //PageMethods.IsDocumentAlreadyUploaded(selectedFileName, fileSize, organizationUserId, checkCallBack);
    }
    else {
        sender.deleteFileInputAt(selectedFileIndex);
        sender.updateClientState();
        alert("File size should be more than 0 byte.");
        return;
    }
}

//callBack after the check has been done
function checkCallBack(result) {
    if (result) {
        showHideBrowseButton();
        alert('You have already updated this document. Please select this document from the Document dropdown.');
        uploader.deleteFileInputAt(selectedFileIndex);
        uploader.updateClientState();
        showHideBrowseButton();
    }
}

var upl_OnClientValidationFailedReq = function (s, a) {
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

//function ShowToolTip(e) {
//    var id = "#" + e.id;
//    var ToolTipCustom = $jQuery(id).parent().siblings("#ToolTipCustom");
//    var spnText = $jQuery(id).parent().siblings("#litCatDesc")[0].innerHTML;
//    var resultHTML = spnText.substring(1, spnText.length - 1);
//    ToolTipCustom[0].innerHTML = resultHTML;
//    if (resultHTML != "")
//        ToolTipCustom.show();
//}
//function HideToolTip(e) {
//    var id = "#" + e.id;
//    var ToolTipCustom = $jQuery(id).parent().siblings("#ToolTipCustom");
//    ToolTipCustom.hide();
//}

//function ShowItemToolTip(e) {
//    var id = "#" + e.id;
//    var ToolTipCustom = $jQuery(id).parent().find("[id$='_ItemToolTipCustom']");
//    if (ToolTipCustom != null) {
//        ToolTipCustom.show();
//    }
//}
//function HideItemToolTip(e) {
//    var id = "#" + e.id;
//    var ToolTipCustom = $jQuery(id).parent().find("[id$='_ItemToolTipCustom']");
//    if (ToolTipCustom != null) {
//        ToolTipCustom.hide();
//    }
//}


function ShowCallBackMessage(docMessage) {
    if (docMessage != '') {
        alert(docMessage);
    }
}

function setFormMode(mode) {
    var control = $jQuery('[id$="hidEditForm"]');
    control.val(mode);

    if (mode == '2') {
        return confirm('Are you sure you want to delete the selected record ?');

        return true;
    }
}



function ValidateUploadDocument(sender, args) {
    var uploadedFilesCount = $jQuery("[id$=fupItemData]")[0].control._uploadedFiles.length;

    var checkedItems = $jQuery("[id$=" + sender.controltovalidate + "]")[0].control.get_checkedItems();
    if (checkedItems.length > 0 || uploadedFilesCount > 0) {
        args.IsValid = true;
        return false;
    }
    args.IsValid = false;
}

function ValidateViewVideo(sender, args) {
    var reqItemFieldId = sender.getAttribute("ReqItemFieldId");
    var hdfIsViewVideoRequired = $jQuery('[id$="' + "hdfIsViewVideoRequired_" + reqItemFieldId + '"]')[0];
    var hdfIsVideoViewed = $jQuery('[id$= "' + "hdfIsVideoViewed_" + reqItemFieldId + '"]')[0];
    if (hdfIsViewVideoRequired.value == "0" || (hdfIsViewVideoRequired.value == "1" && hdfIsVideoViewed.value == "1")) {
        args.IsValid = true;
        return false;
    }
    args.IsValid = false;
}

function ValidateViewDocument(sender, args) {
    var reqItemFieldId = sender.getAttribute("ReqItemFieldId");
    var hdfIsViewDocumentRequired = $jQuery('[id$="' + "hdfIsViewDocumentRequired_" + reqItemFieldId + '"]')[0];
    var hdfIsDocumentViewed = $jQuery('[id$= "' + "hdfIsDocumentViewed_" + reqItemFieldId + '"]')[0];
    if (hdfIsViewDocumentRequired.value == "0" || (hdfIsViewDocumentRequired.value == "1" && hdfIsDocumentViewed.value == "1")) {
        args.IsValid = true;
        return false;
    }
    args.IsValid = false;
}

//View documnet and view video
function ViewDocument(clientSysDocId, reqFieldId, reqObjectTreeId, reqItemFieldId, applicantDocId) {
    _ReqItemFieldId = reqItemFieldId;
    var popupWindowName = "View Document Window";
    var TenantID = $jQuery("[id$=hdfTenantId]").val();
    var organizationUserId = $jQuery("[id$=hdfOrganizationUserId]").val();
    var IsEdited = 'True';
    winopen = true;
    ReqObjectTreeId = reqObjectTreeId;
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var url = $page.url.create("~/ApplicantRotationRequirement/Pages/SharedUserViewDocumentPopup.aspx?ClientSysDocID=" + clientSysDocId + "&ReqFieldID=" + reqFieldId + "&TenantID=" + TenantID + "&ReqObjectTreeID=" + ReqObjectTreeId + "&ApplicantDocID=" + applicantDocId + "&OrganizationUserId=" + organizationUserId);
    var win = $window.createPopup(url, { size: "1100," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName, onclose: SetViewDocPropertiesOnClose });
    return false;
}

function ViewVideo(reqFieldVideoId, reqObjectTreeId, reqItemFieldId) {
    _ReqItemFieldId = reqItemFieldId;
    var popupWindowName = "View Video Window";
    var tenantID = $jQuery("[id$=hdfTenantId]").val();
    var mode = $jQuery('[id$="hidEditForm"]')[0];
    var _reqObjectTreeId = reqObjectTreeId;
    var isEditMode = false;
    //Check if in edit mode or not.Mode=="1" (Edit mode)
    if (mode.value == "1") {
        isEditMode = true;
    }
    winopen = true;
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var url = $page.url.create("~/ApplicantRotationRequirement/Pages/SharedUserViewVideoPopup.aspx?RequrmntFieldVideoID=" + reqFieldVideoId + "&TenantID=" + tenantID + "&RequrmntObjTreeID=" + _reqObjectTreeId + "&IsEditMode=" + isEditMode);
    var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName, onclose: SetViewVideoPropertiesOnClose });
    return false;
}


function SetViewVideoPropertiesOnClose(oWnd, args) {

    oWnd.remove_close(SetViewVideoPropertiesOnClose);
    var arg = args.get_argument();
    var hdfIsVideoViewed = $jQuery('[id$="' + "hdfIsVideoViewed_" + _ReqItemFieldId + '"]');
    var hdfVideoViewedTime = $jQuery('[id$="' + "hdfVideoViewedTime_" + _ReqItemFieldId + '"]');
    var mode = $jQuery('[id$="hidEditForm"]')[0];
    var IsVideoViewed;
    var VideoCurrentPlaybackTime;
    if (arg) {
        if (arg.Action == "Submit") {
            if (arg.IsVideoViewed == true) {
                hdfIsVideoViewed.val("1");
            }
            else {
                hdfIsVideoViewed.val("0");
            }
            //Check if in edit mode or not.Mode=="1" (Edit mode)
            if (mode.value != "1") {
                $jQuery('[id$="' + "hdfVideoViewedTime_" + _ReqItemFieldId + '"]').val(arg.VideoCurrentPlaybackTime);
            }
            //UAT-1470 :As a student, there should be a way to close out of the video once you open it.
            else if (arg.VideoCurrentPlaybackTime > hdfVideoViewedTime.val()) {
                $jQuery('[id$="' + "hdfVideoViewedTime_" + _ReqItemFieldId + '"]').val(arg.VideoCurrentPlaybackTime);
            }
        }
    }
    else {
        var hdnVideoRequiredOpenTime = oWnd._iframe.contentDocument.getElementById("hdnVideoRequiredOpenTime");
        var hdnIsEditMode = oWnd._iframe.contentDocument.getElementById("hdnIsEditMode");
        var hdnIsAdminRequested = oWnd._iframe.contentDocument.getElementById("hdnIsAdminRequested");
        var hdnVideoCurrentPlaybackTimed = oWnd._iframe.contentDocument.getElementById("hdnVideoCurrentPlaybackTimed");
        var hdnIsReqToOpen = oWnd._iframe.contentDocument.getElementById("hdnIsReqToOpen");
        if (hdnIsAdminRequested.value != "True") {
            if (parseInt(hdnVideoCurrentPlaybackTimed.value) >= parseInt(hdnVideoRequiredOpenTime.value) || hdnIsReqToOpen.value == "False" || hdnIsEditMode.value == "true") {
                IsVideoViewed = true;
            }
            else { IsVideoViewed = false; }
            VideoCurrentPlaybackTime = hdnVideoCurrentPlaybackTimed.value;
            if (IsVideoViewed == true) {
                hdfIsVideoViewed.val("1");
            }
            else {
                hdfIsVideoViewed.val("0");
            }
            //Check if in edit mode or not.Mode=="1" (Edit mode)
            if (mode.value != "1") {
                $jQuery('[id$="' + "hdfVideoViewedTime_" + _ReqItemFieldId + '"]').val(VideoCurrentPlaybackTime);
            }
            //UAT-1470 :As a student, there should be a way to close out of the video once you open it.
            else if (parseInt(VideoCurrentPlaybackTime) > parseInt(hdfVideoViewedTime.val())) {
                $jQuery('[id$="' + "hdfVideoViewedTime_" + _ReqItemFieldId + '"]').val(VideoCurrentPlaybackTime);
            }
        }
    }

    ClearValidationMessage('cst_' + _ReqItemFieldId);
}

function SetViewDocPropertiesOnClose(oWnd, args) {
    oWnd.remove_close(SetViewDocPropertiesOnClose);
    var arg = args.get_argument();
    var hdfIsDocViewed = $jQuery('[id$="' + "hdfIsDocumentViewed_" + _ReqItemFieldId + '"]');
    var hdfViewedDocPath = $jQuery('[id$="' + "hdfViewedDocPath_" + _ReqItemFieldId + '"]');
    var hdfDocFileName = $jQuery('[id$="' + "hdfDocFileName_" + _ReqItemFieldId + '"]');
    var hdfIsViewDocumentRequired = $jQuery('[id$="' + "hdfIsViewDocumentRequired_" + _ReqItemFieldId + '"]');
    if (arg) {
        if (arg.Action == "Submit") {
            if (arg.IsDocViewed == true) {
                //Start UAT-5062
                if (arg.fileTemporaryPath != "") {
                if ($jQuery(".cmbFileUpload").length > 0) {
                    var combo = $find($jQuery(".cmbFileUpload")[0].id);
                    combo.trackChanges(); //Bug Id-24881
                    var items = combo.get_items();
                    var dummyApplicantDocId = 0;
                    var comboDummyItem = combo.findItemByValue(dummyApplicantDocId);
                    if (comboDummyItem != null) {
                        items.remove(comboDummyItem);
                    }

                    for (var i = 0; i <= items.get_count() - 1; i++) {
                        var cntrlItem = items.getItem(i);
                        if (!cntrlItem.get_enabled()) {
                            cntrlItem.enable();
                            cntrlItem.set_checked(false);
                        }
                    }

                    var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                    
                    comboItem.set_text(arg.fileName);
                    comboItem.set_value(dummyApplicantDocId);
                    items.add(comboItem);

                    var checkItemsArray = new Array();
                    combo.get_checkedItems().forEach(function (x) {
                        checkItemsArray.push(x.get_value());
                    });

                    var tempArray = new Array();
                    for (var i = 0; i < items.get_count(); i++) {
                        tempArray[i] = new Array();
                        tempArray[i][0] = items.getItem(i).get_text();
                        tempArray[i][1] = items.getItem(i).get_value();
                    }
                    tempArray.sort(function (a, b) { return b[1] - a[1] });

                    items.clear();

                    for (var i = 0; i < tempArray.length; i++) {
                        var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                        comboItem.set_text(tempArray[i][0]);
                        comboItem.set_value(tempArray[i][1]);
                        items.insert(i, comboItem);
                    }

                    checkItemsArray.forEach(function (val) {
                        var item = combo.findItemByValue(val);
                        if (item != null) {
                            item.set_checked(true);
                        }
                    });

                    var item = combo.findItemByValue(dummyApplicantDocId);
                    if (item != null) {
                        item.set_checked(true);
                        item.disable();
                    }
                    combo.commitChanges();
                }
            }
                //End UAT-5062
                hdfIsDocViewed.val("1");
                hdfViewedDocPath.val(arg.fileTemporaryPath);
                hdfDocFileName.val(arg.fileName);
                $jQuery("[id$=hdfIsReqDocViewed]").val("1");
                $jQuery("[id$=hdfReqViewedDocPath]").val(arg.fileTemporaryPath);
                $jQuery("[id$=hdfReqDocFileName]").val(arg.fileName);
            }
            else {
                hdfIsDocViewed.val("0");
                $jQuery("[id$=hdfIsReqDocViewed]").val("0");
            }
            var hdnfIsAutoSubmitTriggerForItem = $jQuery("[id$=hdnfReqIsAutoSubmitTriggerForItem]").val();

            if (hdnfIsAutoSubmitTriggerForItem != null && hdnfIsAutoSubmitTriggerForItem != undefined && hdnfIsAutoSubmitTriggerForItem == "true") {

                if (hdfIsViewDocumentRequired != null && hdfIsViewDocumentRequired != undefined && hdfIsViewDocumentRequired.val() == "1" && hdfIsDocViewed.val() == "0") {
                    return false;
                }
                Page.showProgress('Processing...');
                __doPostBack("<%= btnAutoSubmit.ClientID %>", "");
            }
        }
    }
    ClearValidationMessage('cst_' + _ReqItemFieldId);
}

function ClearValidationMessage(controlId) {
    var validatorControl = $jQuery('[id$="' + controlId + '"]');
    if (validatorControl != null && validatorControl != undefined) {
        validatorControl.hide();
    }

}

//UAT-1615: If I am a student using the view document for a requirement in a rotation package, I should be able to access a copy of the completed form.
function ShowSignedDocument(appDocumentId) {
    var tenantID = $jQuery("[id$=hdfTenantId]").val();
    var isApplicantDocument = "True";
    var applicantDocumentType = "RequirementViewDocument";
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var url = $page.url.create("~/ComplianceOperations/Pages/FormViewer.aspx?TenantId=" + tenantID + "&documentId=" + appDocumentId + "&DocumentType=" + applicantDocumentType + "&IsApplicantDocument=" + isApplicantDocument);
    var win = $window.createPopup(url,
        {
            size: "800," + popupHeight,
            behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move, onclose: OnClientCloseSignedDocument
        },
        function () {
            this.set_title("");
            this.set_destroyOnClose(true);
            this.set_status("");
        });

    winopen = true;
    return false;
}

function OnClientCloseSignedDocument(oWnd, args) {
    oWnd.remove_close(OnClientCloseSignedDocument);
}

//UAT-3639

$page.add_pageLoad(function () {
    SetDropZone();
});


function showHideBrowseButton() {
    if ($jQuery(".dvApplicantDocumentDropzone").length > 0) {
        $jQuery(".RadUpload .ruBrowse[value='Hidden']").remove();
        $jQuery(".ruButton .ruBrowse[value='Hidden']").remove();
        //$jQuery(".ruButton .ruBrowse").remove();
        $jQuery(".ruDropZone").remove();
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
    //showHideBrowseButton();
    dropZone1[0].style.backgroundColor = "#ffffff";
}

function dragEnterHandler(e, dropZone1) {
    dropZone1[0].style.backgroundColor = "#f0f0f6";
}

function dragLeaveHandler(e, dropZone1) {
    dropZone1[0].style.backgroundColor = "#ffffff";
}

function OpenDialog() {
    $telerik.$(".complioFileUpload .ruFileInput").click();
}

function onFileRemoved(sender, args) {
    showHideBrowseButton();
    window.setTimeout(function () { showHideBrowseButton(); }, 0);
    window.setTimeout(function () { showHideBrowseButton(); }, 100);
}
