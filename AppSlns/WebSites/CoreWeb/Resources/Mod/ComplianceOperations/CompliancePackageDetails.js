Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };

function setPostBackSourceDE(event, sender) {
    $jQuery('.postbacksource').val('DE');
    window.DashboardChildClick = 1;
}
function HideInstructionMessageDiv(CIA_ID, isInstructionBox) {
    var tempCIA_ID = null;
    if (isInstructionBox != undefined) {
        var datepickerId = "[id$=" + CIA_ID._element.id + "]";
        tempCIA_ID = $jQuery(datepickerId).parent().parent().find("#hdnCIA_ID")[0].value;
    }
    else {
        tempCIA_ID = CIA_ID;
    }
    var idToFind = "[id$=divInsTemp" + tempCIA_ID + "]";
    var hdnInstBoxCIA_ID = "[id$=hdnInstBoxCIA_ID_" + tempCIA_ID + "]";
    var hdnInstBoxCIA_IDValue = $jQuery(hdnInstBoxCIA_ID)[0].value;
    var idmainDiv = "[id$=" + hdnInstBoxCIA_IDValue + "]";
    var divToHide = $jQuery(idToFind);
    if (divToHide.length > 0) {
        divToHide.hide();
    }
    if ($jQuery(idmainDiv).length > 0) {
        var divIsVisible = false;
        $jQuery(idmainDiv).find(".instBox").each(function () {
            if ($jQuery(this).is(':visible'))
                divIsVisible = true;
        });
        if (!divIsVisible)
            $jQuery(idmainDiv).hide();
    }

}

function efn_viewItemsClick() {
    var url = $page.url.create("~/ComplianceOperations/Pages/ComplianceExplanation.aspx?TenantId=" + tenantId + "&PackageId=" + clientCompliancePackageID);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    $window.createPopup(url, { size: "800," + popupHeight });
}

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
function efn_ManageUploadDocuments() {
    var url = $page.url.create("~/ComplianceOperations/Pages/ManageUploadDocuments.aspx?TenantId=" + tenantId);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    $window.createPopup(url, { size: "800," + popupHeight });
}
function applyExceptionClick() {
    var url = $page.url.create("~/ComplianceOperations/Pages/CategoryExceptionEntry.aspx?TenantId=" + tenantId);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    $window.createPopup(url, { size: "800," + popupHeight });
}
$page.add_pageReady(function () {

    $jQuery(document).on("click", "#alnkSummary", function () {
        //            if (typeof (tenantId) != "undefined" && typeof (_packageSubscriptionId) != "undefined" && typeof (_rptUrl) != "undefined" && _rptUrl) {
        if (typeof (tenantId) != "undefined" && typeof (_packageSubscriptionId) != "undefined") {
            // _rptUrl = _rptUrl.lastIndexOf("/") + 1 == _rptUrl.length ? _rptUrl : _rptUrl + "/";
            var url = $page.url.create("Reports/ImmunizationSummaryReport.aspx?tid=" + tenantId + "&psid=" + _packageSubscriptionId);
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var doc_win = $window.createPopup(url, { size: "760," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move }, function () { this.set_title("Immunization Summary Report"); this.set_destroyOnClose(true); });
        }
        return false;
    });
});

function ShowHideDetails(e, f) {
    // var documentHeader = $jQuery('[id$=documentHeader' + e +'-'+ f+ ']');       
}

$page.add_pageLoad(function () {

    //Attribute details viewer
    $jQuery(".details_viewer").click(function () {
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
                $jQuery(content).show().css({ "width": "100%", "background-color": "#fff" }).find("td.td-two, td.td-three, td.td-four").css({ padding: "5px", border: "1px solid black" });
                $jQuery(content).find("td.td-four a").click(function () { documentPreviewPopup(this); });
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
    //Code changes for UAT 531- As a student, I should not be able to upload a duplicate document.
    //PageMethods.IsDocumentAlreadyUploaded(selectedFileName, checkCallBack);
}

//callBack after the check has been done
function checkCallBack(result) {
    if (result) {
        //Code changes for UAT 531- As a student, I should not be able to upload a duplicate document.
        //if (!confirm('You have already uploaded this document. Are you sure you want to upload this file again?')) {
        //    uploader.deleteFileInputAt(selectedFileIndex);
        //}
        showHideBrowseButton();
        alert('You have already updated this document. Please select this document from the Document dropdown.');
        uploader.deleteFileInputAt(selectedFileIndex);
        uploader.updateClientState();
        showHideBrowseButton();
    }
}

function ShowCallBackMessage(docMessage) {
    if (docMessage != '') {
        alert(docMessage);
    }
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
    showHideBrowseButton();
}

function OnClientFileUploading(sender, args) {
    if (!isValid(args.get_fileName())|| args.get_fileName().length > 100) {
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
function OnClientFileUploaded(sender, args) {
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
            showHideBrowseButton();
            alert("You have already updated this document.");
            showHideBrowseButton();
            return;
        }
        showHideBrowseButton();
        //Check if document is already uploaded.
        PageMethods.IsDocumentAlreadyUploaded(selectedFileName, fileSize, organizationUserId, checkCallBack);
    }
    else {
        sender.deleteFileInputAt(selectedFileIndex);
        sender.updateClientState();
        alert("File size should be more than 0 byte.");
        return;
    }
    showHideBrowseButton();
}

function ShowToolTip(e) {
    var id = "#" + e.id;
    var ToolTipCustom = $jQuery(id).parent().siblings("#ToolTipCustom");
    var spnText = $jQuery(id).parent().siblings("#litCatDesc")[0].innerHTML;
    var resultHTML = spnText.substring(1, spnText.length - 1);
    ToolTipCustom[0].innerHTML = resultHTML;
    var position = $jQuery()
    if (resultHTML != "") {
        ToolTipCustom.show();
        IsToolTipHover = false;
    }
}
var IsToolTipHover;
function HideToolTip(e) {
    var id = "#" + e.id;
    var ToolTipCustom = $jQuery(id).parent().siblings("#ToolTipCustom");
    setTimeout(function () {
        if (IsToolTipHover != undefined && IsToolTipHover != null && !IsToolTipHover) {
            ToolTipCustom.hide();
            IsToolTipHover = false;
        }
    }, 100);
}
function ToolTipMouseEnter(sender) {
    IsToolTipHover = true;
    sender.style["display"] = "block";
}
function ToolTipMouseOut(sender) {
    sender.style["display"] = "none";
    IsToolTipHover = false;
}

function ShowItemToolTip(e) {
    var id = "#" + e.id;
    var ToolTipCustom = $jQuery(id).parent().find("[id$='_ItemToolTipCustom']");
    if (ToolTipCustom != null) {
        ToolTipCustom.show();
    }
}
function HideItemToolTip(e) {
    var id = "#" + e.id;
    var ToolTipCustom = $jQuery(id).parent().find("[id$='_ItemToolTipCustom']");
    if (ToolTipCustom != null) {
        ToolTipCustom.hide();
    }
}

//function CmbCheckedChange(sender, event) {
//    //debugger;
//    $jQuery('[id$="hdnCmbName"]').val(sender._text)
//}

//function test() {
//    debugger;
//    var cmbtext = $jQuery('[id$="hdnCmbName"]').val()
//    $find(($jQuery("[id$=cmbExceptionItems]"))[0].id).set_text(cmbtext);
//}

//function SetCmbText(sender, event) {
//    //debugger;
//    var cmbtext = $jQuery('[id$="hdnCmbName"]').val()
//    $find(($jQuery("[id$=cmbExceptionItems]"))[0].id).set_text(cmbtext);
//}


function OnClientDropDownClosing(sender, eventArgs) {
    var code = eventArgs.keyCode || eventArgs.which;
    if (code == 13) {
        eventArgs.preventDefault();
        return false;
    }
}

function setFormMode(mode) {
    var control = $jQuery('[id$="hidEditForm"]');
    control.val(mode);

    if (mode == '2' || mode == '3') {
        return confirm('Are you sure you want to delete the selected record ?');

        return true;
    }
}

function confirmDeletion() {
    return (confirm('Are you sure you want to delete'));

}

function ShowDataEntryTooltip() {
    window.setTimeout(function () {
        $jQuery('.lnkEnterRequirement').first().each(function () {
            if ($jQuery('.lnkEnterRequirement').is(":visible")) {
                $findByKey("step2tooltip", function () {
                    this.show();
                });
            }
        });
    }, 10);
}


$page.add_pageLoaded(function () {
    if (!$jQuery(".rtlEditForm").is(':visible')) {
        $jQuery(".rtlEditForm").slideDown(1000);
    }
});

function openPopUp() {
    var composeScreenWindowName = "StartHere";
    var PackageId = $jQuery("[id$=hdfPackageId]").val();
    var PackageName = $jQuery("[id$=lblPackageName]").text();
    var tenantId = $jQuery("[id$=hdfTenantId]").val();
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var url = $page.url.create("~/ComplianceOperations/Pages/DataEntryHelp.aspx?TenantId=" + tenantId + "&PackageId=" + PackageId + "&PackageName=" + PackageName);
    var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName });
}

function OnClientClose(oWnd, args) {
    oWnd.set_title('');
    oWnd.remove_close(OnClientClose);
}

var winopen = false;

function openPdfPopUp(hdnOrgUserID, hdnfSystemDocumentId, hdnAppDocID) {
    var documentType = "ClientSystemDocument";
    var composeScreenWindowName = "Service Form Details";
    if (hdnfSystemDocumentId == "0" && hdnAppDocID == "0") {
        alert("No document exists for current attribute.");
    }
    else {
        var url = $page.url.create("~/ComplianceAdministration/Pages/ViewDocumentPopup.aspx?ClientSysDocID=" + hdnfSystemDocumentId + "&OrganizationUserID=" + hdnOrgUserID + "&ApplicantDocId=" + hdnAppDocID);
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var win = $window.createPopup(url, { size: "1100," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, name: composeScreenWindowName, onclose: RebindDocumentDropDown });
        winopen = true;
    }
    return false;
}



function RebindDocumentDropDown(oWnd, args) {
 
    oWnd.remove_close(RebindDocumentDropDown);
    if (args != undefined) {
        var arg = args.get_argument();
        if (arg.applicantDocID) {
            if ($jQuery(".cmbFileUpload").length > 0) {
                var combo = $find($jQuery(".cmbFileUpload")[0].id);
                var items = combo.get_items();
                //combo.trackChanges();

                for (var i = 0; i <= items.get_count() - 1; i++) {
                    var cntrlItem = items.getItem(i);
                    if (!cntrlItem.get_enabled()) {
                        cntrlItem.enable();
                        cntrlItem.set_checked(false);
                    }
                }

                var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                comboItem.set_text(arg.fileName);
                comboItem.set_value(arg.applicantDocID);
                if (arg.applicantDocID != "") {
                    items.add(comboItem);
                }
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

                var item = combo.findItemByValue(arg.applicantDocID);
                if (item != null) {
                    item.set_checked(true);
                    item.disable();
                }
                combo.commitChanges();
                BindDocumentForPreview(combo);
            }
            //if (arg.IsDocViewed == true) {
            //    //hdfIsDocViewed.val("1");
            //    //hdfViewedDocPath.val(arg.fileTemporaryPath);
            //    //hdfDocFileName.val(arg.fileName);
            //    $jQuery("[id$=hdfDocViewed]").val("1");
            //    $jQuery("[id$=hdfViewedDocPath]").val(arg.fileTemporaryPath);
            //    $jQuery("[id$=hdfDocFileName]").val(arg.fileName);
            //    $jQuery("[id$=hdfapplicantDocID]").val(arg.applicantDocID);
            //}
            //else {
            //    // hdfIsDocViewed.val("0");
            //    $jQuery("[id$=hdfDocViewed]").val("0");
            //}

        }
    }

    if (arg.Action == "Submit") {
        if (arg.applicantDocID && arg.IsDocViewed == true) {
            $jQuery("[id$=hdfDocViewed]").val("1");
            $jQuery("[id$=hdfViewedDocPath]").val(arg.fileTemporaryPath);
            $jQuery("[id$=hdfDocFileName]").val(arg.fileName);
            $jQuery("[id$=hdfapplicantDocID]").val(arg.applicantDocID);
        }
        else {
            //hdfIsDocViewed.val("0");
            $jQuery("[id$=hdfDocViewed]").val("0");
        }

        var hdnfIsAutoSubmitTriggerForItem = $jQuery("[id$=hdnfIsAutoSubmitTriggerForItem]").val();

        if (hdnfIsAutoSubmitTriggerForItem != null && hdnfIsAutoSubmitTriggerForItem != undefined && hdnfIsAutoSubmitTriggerForItem == "true") {
            Page.showProgress('Processing...');
            __doPostBack("<%= btnAutoSubmit.ClientID %>", "");
        }
    }
}

//Method used to show screening document in popup.
//UAT-1738:
function ViewScreeningDoc(applicantDocId) {
    var composeScreenWindowName = "Screening Document";
    var tenantId = $jQuery("[id$=hdfTenantId]").val();
    if (tenantId == "0" && applicantDocId == "0") {
        alert("No document exists for current attribute.");
    }
    else {
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/ComplianceOperations/Pages/FormViewer.aspx?&tenantId=" + tenantId + "&documentId=" + applicantDocId + "&IsApplicantDocument=" + "true");
        var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close },
            function () {
                this.set_title(composeScreenWindowName);
                this.set_destroyOnClose(true);
                this.set_status("");
            });
        winopen = true;
    }
    return false;
}

//UAT-1864 : As an applicant, I should be able to preview documents in the document selection dropdown on the submit item screen.
function BindDocumentForPreview(sender, args) {
    var documentpreviewDiv = $jQuery("[id$=dvDocumentPreview]")[0];
    var combo = sender;
    var items = combo.get_items();
    var checkedIndices = items._parent._checkedIndices;
    var checkedIndicesCount = checkedIndices.length;
    var pnlDocumentPreview = $jQuery("[id$=pnlDocumentPreview]")[0];
    pnlDocumentPreview.innerHTML = "";
    if (checkedIndicesCount == 0) {
        documentpreviewDiv.style.display = "none";
    }
    else {
        for (var itemIndex = 0; itemIndex < checkedIndicesCount; itemIndex++) {
            documentpreviewDiv.style.display = "block";
            var item = items.getItem(checkedIndices[itemIndex]);
            var html = "<a id='" + item._properties._data.value + "' onClick='OpenAddAnotherItemPopup(" + item._properties._data.value + ")' href='#'>" + item._text + "</a></br>";
            pnlDocumentPreview.innerHTML = pnlDocumentPreview.innerHTML + html;
        }
    }
}

function OpenAddAnotherItemPopup(DocumentID) {
    var DocumentName = $jQuery("[id$=" + DocumentID + "]")[0].innerHTML;
    openDocumentInPopUp(DocumentName, DocumentID);
}



//UAT-2028: Expired items should also show in the Enter Requirements item selection dropdown on the student screen
function CheckExpiredItemOnSelection(sender, args) {
    var itemID = 0;
    if (args.get_item()._attributes._data["IsItemSeries"] == null || args.get_item()._attributes._data["IsItemSeries"] == "False")
        itemID = args.get_item().get_value();
    var itemIds = $jQuery("[id$=hdnItemID]");
    //Start UAT-5220
    var categoryId = $jQuery("[id$=hdfComplianceCategoryId]");
    //End UAT-5220
    if (itemID > 0) {
        for (i = 0; i < itemIds.length; i++) {
            if (itemIds[i].value != null && itemIds[i].value.split(',')[1] == categoryId[0].value && itemIds[i].value.split(',')[0] == itemID) //UAT-5220
            {
                $jQuery("[id$=" + itemIds[i].parentNode.id + "]")[0].click();
                args._cancel = true;
                return false;
                break;
            }
        }
    }
}

function documentPreviewPopup(sender) {
    var documentId;
    var documentName;
    var anchor = $jQuery("[id$=" + sender.id + "]");
    if (anchor.length > 0) {
        documentName = anchor[0].attributes['FileName'].value;
        documentId = anchor[0].attributes['DocumentID'].value;
        openDocumentInPopUp(documentName, documentId);
    }
}

function openDocumentInPopUp(documentName, documentId) {
    var extention = documentName.substring(documentName.lastIndexOf('.') + 1);
    var tenantId = $jQuery("[id$=hdfTenantId]").val();
    var url = "";
    if (extention == "pdf" || extention == "bmp" || extention == "gif" || extention == "jpg") {
        url = $page.url.create("~/ComplianceOperations/Pages/FormViewer.aspx?tenantId=" + tenantId + "&documentId=" + documentId + "&IsApplicantDocument=true");
        winopen = true;
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var win = $window.createPopup(url,
            {
                size: "900," + popupHeight,
                behaviors: Telerik.Web.UI.WindowBehaviors.Close
                    | Telerik.Web.UI.WindowBehaviors.Move
                , onClose: OnClientClose
            }
            , function () { this.set_title("Document Preview"); this.set_destroyOnClose(true); }
        );
    }
    else {
        url = $page.url.create("~/ComplianceOperations/UserControl/DoccumentDownload.aspx?tenantId=" + tenantId + "&documentId=" + documentId + "&IsApplicantDocument=true");
        location.href = url;
    }
    return false;
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

function onFileRemoved(sender, args) {
    showHideBrowseButton();
    window.setTimeout(function () { showHideBrowseButton(); }, 0);
    window.setTimeout(function () { showHideBrowseButton(); }, 100);
}
