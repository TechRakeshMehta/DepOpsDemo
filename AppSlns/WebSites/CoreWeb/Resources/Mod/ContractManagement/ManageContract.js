var winopen = false;
var composeScreenWindowName = "Institution Hierarchy";
var screenName = "CommonScreen";

function openPopUp() {
    var tenantId = $jQuery("[id$=hdnTenantId]").val();
    if (tenantId != "0" && tenantId != "") {
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();
        var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName=" + screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);
        var win = $window.createPopup(url, {
            size: "600,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move,
            name: composeScreenWindowName, onclose: OnClientClose
        });
        winopen = true;
    }
    else {
        $alert("Please select Institution.");
    }
    return false;
}

function OnClientClose(oWnd, args) {
    oWnd.remove_close(OnClientClose);
    if (winopen) {
        var arg = args.get_argument();
        if (arg) {
            $jQuery("[id$=hdnDepartmntPrgrmMppng]").val(arg.DepPrgMappingId);
            $jQuery("[id$=hdnHierarchyLabel]").val(arg.HierarchyLabel);
            $jQuery("[id$=hdnInstitutionNodeId]").val(arg.InstitutionNodeId);
            $jQuery("[id$=lblInstituteHierarchyName]")[0].innerHTML = arg.HierarchyLabel;
        }
        winopen = false;
    }
}


//for Adding new Bundle------------------------------------
function OpenInstitutionHierarchyGridMode() {
    var composeScreenWindowName = "Institution Hierarchy";
    var screenName = "CommonScreen";

    var tenantId = $find($jQuery("[id$=cmbTenantGridMode]").attr("id")).get_value();
    if (tenantId != "0" && tenantId != "") {
        var DepartmentProgramId = $jQuery("[id$=hdnDPMGridMode]").val();
        var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName="
                                    + screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var win = $window.createPopup(url, {
            size: "600,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move,
            name: composeScreenWindowName, onclose: OnHierarhyClientCloseGridMode
        });
        winopen = true;
    }
    else {
        $alert("Please select Institution.");
    }
    return false;
}

function OnHierarhyClientCloseGridMode(oWnd, args) {
    oWnd.remove_close(OnHierarhyClientCloseGridMode);
    if (winopen) {
        var arg = args.get_argument();
        if (arg) {
            $jQuery("[id$=hdnDPMGridMode]").val(arg.DepPrgMappingId);
            $jQuery("[id$=hdnInstitutionHierarchyGridMode]").val(arg.HierarchyLabel);
            $jQuery("[id$=hdnInstNodeIdGridMode]").val(arg.InstitutionNodeId);
            $jQuery("[id$=lblInstitutionHierarchyGridMode]")[0].innerHTML = arg.HierarchyLabel;
        }
        winopen = false;
    }
}

//---------Manage Documents-------------//

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

function CreatePackageVersionClicked(sender, args) {
    if (sender._checked) {
        $jQuery("[id$=divUploadDoc]").hide();
    }
    else {
        $jQuery("[id$=divUploadDoc]").show();
    }
}

//-------------//


var minDate = new Date("01/01/1980");
function SetMinDate(picker) {
    picker.set_minDate(minDate);
}

function SetMinEndDate(picker) {
    var date = $jQuery("[id$=txtSdateSrch]")[0].control.get_selectedDate();
    if (date != null) {
        picker.set_minDate(date);
    }
    else {
        picker.set_minDate(minDate);
    }
}

function CorrectStartToEndDate(picker) {
    var date1 = $jQuery("[id$=txtSdateSrch]")[0].control.get_selectedDate();
    var date2 = $jQuery("[id$=txtEdateSrch]")[0].control.get_selectedDate();
    if (date1 != null && date2 != null) {
        if (date1 > date2)
            $jQuery("[id$=txtEdateSrch]")[0].control.set_selectedDate(null);
    }
}

function SetMinEndDateOnAdd(picker) {
    var date = $jQuery("[id$=dpStartDate]")[0].control.get_selectedDate();
    if (date != null) {
        picker.set_minDate(date);
    }
    else {
        picker.set_minDate(minDate);
    }
}

function CorrectStartToEndDateOnAdd(picker) {
    var date1 = $jQuery("[id$=dpStartDate]")[0].control.get_selectedDate();
    var date2 = $jQuery("[id$=dpEdate]")[0].control.get_selectedDate();
    if (date1 != null && date2 != null) {
        if (date1 > date2)
            $jQuery("[id$=dpEdate]")[0].control.set_selectedDate(null);
    }
}
//UAT-1475 Make it easier to view a contract entry on Manage Contract.
function OpenPopUpViewDocument(isReadOnly) {
    if (isReadOnly == true) {
        var saveButton = $jQuery(".rgEditForm").find("[id$=btnGrd]");
        if (saveButton) {
            saveButton.hide();
        }
    }

    var content = $jQuery("[id$=hdnDocHtml]").val();
    if (content.length > 0) {
        $window.showDialog($jQuery(content), { btnClose: { autoclose: true, "text": "Close" } }, 500, "Contract Document(s)");
    }

}

function OpenPopUpSiteViewDocument(isReadOnly) {
    if (isReadOnly == true) {
        var saveButton = $jQuery(".rgEditForm").find("[id$=btnGrd]");
        if (saveButton) {
            saveButton.hide();
        }
    }
    var content = $jQuery("[id$=hdnDocHtml]").val();
    if (content.length > 0) {
        $window.showDialog($jQuery(content), { btnClose: { autoclose: true, "text": "Close" } }, 500, "Site Document(s)");
    }
}

function HideControlsBasedOnGranularPermissions() {
    $(".MyImageButton").hide();
    $(".tplcohdr").hide();
}

