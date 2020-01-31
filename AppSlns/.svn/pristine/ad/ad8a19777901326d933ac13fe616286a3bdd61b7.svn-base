var minDate = new Date("01/01/1980");
function SetMinDate(picker) {
    picker.set_minDate(minDate);
}

function SetMinEndDate(picker) {
    var date = $jQuery("[id$=dpStartDate]")[0].control.get_selectedDate();
    if (date != null) {
        picker.set_minDate(date);
    }
    else {
        picker.set_minDate(minDate);
    }
}

function CorrectStartToEndDate(picker) {
    var date1 = $jQuery("[id$=dpStartDate]")[0].control.get_selectedDate();
    var date2 = $jQuery("[id$=dpEndDate]")[0].control.get_selectedDate();
    if (date1 != null && date2 != null) {
        if (date1 > date2)
            $jQuery("[id$=dpEndDate]")[0].control.set_selectedDate(null);
    }
}

function SetMinEndDateOnAdd(picker) {
    var date = $jQuery("[id$=dpStartDate]")[1].control.get_selectedDate();
    if (date != null) {
        picker.set_minDate(date);
    }
    else {
        picker.set_minDate(minDate);
    }
}

function CorrectStartToEndDateOnAdd(picker) {
    var date1 = $jQuery("[id$=dpStartDate]")[1].control.get_selectedDate();
    var date2 = $jQuery("[id$=dpEndDate]")[1].control.get_selectedDate();
    if (date1 != null && date2 != null) {
        if (date1 > date2)
            $jQuery("[id$=dpEndDate]")[1].control.set_selectedDate(null);
    }
}
var selectedFileIndex;
function onFileUploaded(sender, args) {
    var WhichEventFire = sender._clientStateFieldID;
    if (WhichEventFire.indexOf('Additional') != -1) {
        $jQuery("[id$=lblAdditionalDocumentsRequired]").hide();
    }
    if (WhichEventFire.indexOf('uploadControl') != -1) {
        $jQuery("[id$=lblSyllabusDocumentError]").hide();
    }


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


function grdRotation_rwDbClick(s, e) {
    var _id = "btnEdit";
    var b = e.get_gridDataItem().findControl(_id);
    if (b && typeof (b.click) != "undefined") { b.click(); }
}


//---------------------------------------------------------------------------------------------------------------------------
function CheckAllRotation(id) {
    var gridRotationID = $jQuery("[id$=grdRotations]").attr("id");
    var masterTable = $find(gridRotationID).get_masterTableView();
    var row = masterTable.get_dataItems();
    var isChecked = false;
    if (id.checked == true) {
        var isChecked = true;
    }
    for (var i = 0; i < row.length; i++) {
        if (masterTable.get_dataItems()[i].findElement("chkSelectItem") != null) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled)) {
                masterTable.get_dataItems()[i].findElement("chkSelectItem").checked = isChecked; // for checking the checkboxes
            }
        }
    }
}

function UnCheckRotationHeader(id) {
    var checkHeader = true;
    //var masterTable = $find("<%= grdInvitations.ClientID %>").get_masterTableView();
    var gridRotationID = $jQuery("[id$=grdRotations]").attr("id");
    var masterTable = $find(gridRotationID).get_masterTableView();
    var row = masterTable.get_dataItems();
    for (var i = 0; i < row.length; i++) {
        if (masterTable.get_dataItems()[i].findElement("chkSelectItem") != null) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
    }
    $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
}
//--------------------------------------------------------------------------------------------------

var winopen = false;

function OpenInstitutionHierarchyPopupInsideGrid(IsMappingScreen, IsRequestFromAddRotationScreen) {
    
    //UAT-1843: Bug Fix: Institution Hierarchy popup not shown on top if this popup maximized
    if (IsMappingScreen) {
        var win = $page.get_window();
        if (win) {
            win.restore();
        }
    }

    var composeScreenWindowName = "Institution Hierarchy";
    var screenName = "CommonScreen";
    var ScreenNameForPermission = "ScreenNameForPermissionReadOnly";

    var tenantId = $find($jQuery("[id$=ddlTenant]").attr("id")).get_value();
    if (tenantId != "0" && tenantId != "") {
        var DelemittedDeptPrgMapIds = $jQuery("[id$=hdnDepartmentProgmapNew]").val();
        var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&DelemittedDeptPrgMapIds=" + DelemittedDeptPrgMapIds + "&ScreenName=" + screenName + "&ScreenNameForPermission=" + ScreenNameForPermission + "&IsRequestFromAddRotationScreen=" + IsRequestFromAddRotationScreen);
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);
        var win = $window.createPopup(url, {
            size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move,
            name: composeScreenWindowName, onclose: OnHierarhyClientClose
        });
        if (IsParentMaximizedWindow && parseInt(IsParentzIndex) > 0) {
            if (parseInt(win._popupElement.style.zIndex) < parseInt(IsParentzIndex)) {
                win._popupElement.style.zIndex = parseInt(IsParentzIndex) + 3;
            }
        }
        winopen = true;
    }
    else {
        $alert("Please select Institution.");
    }
    return false;
}

function OnHierarhyClientClose(oWnd, args) {
    //debugger;
    oWnd.remove_close(OnHierarhyClientClose);
    if (winopen) {
        var arg = args.get_argument();
        if (arg) {

           // debugger;
            $jQuery("[id$=hdnDepartmentProgmapNew]").val(arg.DepPrgMappingId);
            $jQuery("[id$=hdnInstitutionHierarchyPBLbl]").val(arg.HierarchyLabel);
            $jQuery("[id$=hdnInstNodeIdNew]").val(arg.InstitutionNodeId);
            $jQuery("[id$=lblInstitutionHierarchyPB]")[0].innerHTML = arg.HierarchyLabel;

            if ($jQuery("[id$=hdnInstNodeLabel]").length > 0) {
                $jQuery("[id$=hdnInstNodeLabel]").val(arg.HierarchyLabel);
            }

            //Settings for Add/Edit Rotation (Services --> Manage Rotation)
            if ($jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnInstitutionNodeIds]").length > 0) {
                $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnInstitutionNodeIds]").val(arg.DepPrgMappingId);
            }

            if ($jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnAgencyNodeId]").length > 0) {
                $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnAgencyNodeId]").val('');
            }

            if ($jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_lblAgencyHierarchy]").length > 0) {
                $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_lblAgencyHierarchy]").text('');
            }

            if ($jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnselectedRootNodeId]").length > 0) {
                $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnselectedRootNodeId]").val('');
            }

            if ($jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnAgencyName]").length > 0) {
                $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnAgencyName]").val('');
            }

            if ($jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnHierarchyLabel]").length > 0) {
                $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnHierarchyLabel]").val('');
            }

            //Settings for Add Rotation (Bulk Student Assignment)
            if ($jQuery("[id$=hdnIsFromStudentBulkAssignmentScreen]").length > 0) {

                if ($jQuery("[id$=hdnInstitutionNodeIds]").length > 0) {
                    $jQuery("[id$=hdnInstitutionNodeIds]").val(arg.DepPrgMappingId);
                }

                if ($jQuery("[id$=hdnAgencyNodeId]").length > 0) {
                    $jQuery("[id$=hdnAgencyNodeId]").val('0');
                }

                if ($jQuery("[id$=lblAgencyHierarchy]").length > 0) {
                    $jQuery("[id$=lblAgencyHierarchy]").text('');
                }

                if ($jQuery("[id$=hdnselectedRootNodeId]").length > 0) {
                    $jQuery("[id$=hdnselectedRootNodeId]").val('');
                }

                if ($jQuery("[id$=hdnAgencyName]").length > 0) {
                    $jQuery("[id$=hdnAgencyName]").val('');
                }

                if ($jQuery("[id$=hdnHierarchyLabel]").length > 0) {
                    $jQuery("[id$=hdnHierarchyLabel]").val('');
                }
            }


            var dvInstAvailability = $jQuery("[id$=dvInstAvailability]");
            var rdbInstAvailabileYes = $jQuery("[id$=rdbInstAvailabileYes]");
            var rdbInstAvailabileNo = $jQuery("[id$=rdbInstAvailabileNo]");

            if (dvInstAvailability != undefined && dvInstAvailability != null)
                dvInstAvailability.attr("style", "display:none !important;");
            if (rdbInstAvailabileYes != undefined && rdbInstAvailabileYes != null)
                rdbInstAvailabileYes[0].checked = false;
            if (rdbInstAvailabileNo != undefined && rdbInstAvailabileNo != null)
                rdbInstAvailabileNo[0].checked = true;

            $jQuery("[id$=spnInsPre]").attr('class', 'reqd controlHidden');
        }
        winopen = false;
    }
}




/*UAT-1641:*/

function BindAgencyDropDown() {
    //debugger;
    var comboBoxTenant = $find($jQuery("[id$=cmbTenant]")[0].id);
    if (IsAnyChangesInTenantSelection(comboBoxTenant)) {
        var selectedTenantIds = "";
        for (var i = 0; i < comboBoxTenant.get_checkedItems().length ; i++) {
            selectedTenantIds += comboBoxTenant.get_checkedItems()[i].get_value();
            if (i != comboBoxTenant.get_checkedItems().length - 1) {
                selectedTenantIds += ",";

            }
        }
        var userID = $jQuery("[id$=hdnUserID]")[0].value;
        var dropDownControlID = "ddlAgency";
        $jQuery.ajax({
            type: "POST",
            url: '/ProfileSharing/Default.aspx/GetUserAgencyList',
            data: "{'selectedTenantIDs': '" + selectedTenantIds + "', userId: '" + userID + "', orgUserID: '" + 0 + "', isTabTypeInvitation: '" + false + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {
                if (Result != undefined) {
                    BindListToCombo(dropDownControlID, Result.d);
                }
            },
            error: function (Result) {
            }
        });
    }
    else { return false; }
}

function BindListToCombo(controlId, result) {
    var control = $jQuery("[id$=" + controlId + "]")[0];
    if (control != undefined || control != null) {
        var combo = $find(control.id);
        if (combo != undefined || combo != null) {
            combo.trackChanges();
            var selectedItem = combo.get_selectedItem();
            var items = combo.get_items();
            //var checkedItems = combo.get_checkedItems();
            items.clear();
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                    comboItem.set_text(result[i].Name);
                    comboItem.set_value(result[i].ID);
                    //for (var j = 0; j < checkedItems.length; j++) {
                    //    if (checkedItems[j].get_value() == result[i].ID) {
                    //        //var currentItem = combo.findItemByValue(result[i].ID);
                    //        //if (currentItem != null)
                    //        comboItem.set_checked(true);
                    //    }
                    //}
                    items.add(comboItem);
                    comboItem._element.style.fontWeight = 'bold';
                    //if (comboItem._element.style.marginLeft != combo._checkAllItemsElement[0].currentStyle.paddingLeft) {
                    //    comboItem._element.style.marginLeft = combo._checkAllItemsElement[0].currentStyle.paddingLeft;
                    //}
                }
            }
            combo.clearSelection();
            combo.commitChanges();
            $jQuery("[id$=btnDoPostBack]").click();
        }
    }
}

function ComboBoxSelectedIdList(sender) {
    var selectedTenantIdList = [];
    var combo = sender;
    var checkeditems = combo.get_checkedItems();
    for (i = 0; i < checkeditems.length; i++) {
        selectedTenantIdList.push(checkeditems[i].get_value());
    }
    return selectedTenantIdList;
}


function IsAnyChangesInTenantSelection(sender) {
    var oldTenantIdList = [];
    var hdnPreviousTenantIds = $jQuery("[id$=hdnPreviousTenantIds]");
    if (hdnPreviousTenantIds.val() != "" && hdnPreviousTenantIds.val() != null && hdnPreviousTenantIds.val() != undefined) {
        oldTenantIdList = hdnPreviousTenantIds.val().split(',');
    }
    var selectedIdList = ComboBoxSelectedIdList(sender);
    hdnPreviousTenantIds.val(selectedIdList.join(","));
    var isTheCountOfEachSelectionEqual = (selectedIdList.length == oldTenantIdList.length);
    if (isTheCountOfEachSelectionEqual == false)
        return true;

    var oldIdListMINUSNewIdList = $(oldTenantIdList).not(selectedIdList).get();
    var newIdListMINUSOldIdList = $(selectedIdList).not(oldTenantIdList).get();

    if (oldIdListMINUSNewIdList.length != 0 || newIdListMINUSOldIdList.length != 0)
        return true;

    return false;
}

$page.showAlertMessageWithTitle = function (msg, msgtype, overriderErrorPanel) {
    msg = $jQuery("[id$=hdnErrorMessage]")[0].value;
    if (typeof (msg) == "undefined") return;
    var c = typeof (msgtype) != "undefined" ? msgtype : "";
    if (overriderErrorPanel) {
        $jQuery("#pageMsgBoxSchuduleInv").children("span")[0].innerHTML = msg;
        $jQuery("#pageMsgBoxSchuduleInv").children("span").attr("class", msgtype);
        if (c == 'sucs') {
            c = "Success";
        }
        else (c = "Validation Message for Rotation Package:");

        $jQuery("[id$=pnlErrorSchuduleInv]").hide();

        $window.showDialog($jQuery("#pageMsgBoxSchuduleInv").clone().show(), { closeBtn: { autoclose: true, text: "Ok" } }, 500, c);
    }
    else {
        $jQuery("#pageMsgBoxSchuduleInv").fadeIn().children("span")[0].innerHTML = msg;
        $jQuery("#pageMsgBoxSchuduleInv").fadeIn().children("span").attr("class", msgtype);

    }
}

//function CheckPreceptorRequiredForAgency(tenantID, agencyID) {

//    if ($jQuery("[id$=ddlTenant]").length <= 0) {
//        return;
//    }

//    if (parseInt(tenantID) > 0 && parseInt(agencyID) > 0) {
//        $jQuery.ajax({
//            type: "POST",
//            url: '/ClinicalRotation/Default.aspx/IsInstructorPreceptorRequiredForAgency',
//            data: "{'strTenantId': '" + tenantID + "', strAgencyID: '" + agencyID + "'}",
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            success: function (result) {
//                if (result != undefined) {
//                    var res = JSON.parse(result.d);
//                    var isRequired = res.IsRequired;
//                    var validator = $jQuery("[id$=cstValidator]")[0];

//                    if (isRequired) {
//                        $jQuery("[id$=spnInsPre]").attr('class', 'reqd');
//                        //ValidatorEnable(validator, true);
//                    }
//                    else {
//                        $jQuery("[id$=spnInsPre]").attr('class', 'reqd controlHidden');
//                        //ValidatorEnable(validator, false);
//                    }
//                }
//            },
//            error: function (error) {
//            }
//        });
//    }
//}




//UAT-2712: Ajax call for getting the rotation rew. field validator
function GetRotationRequiredFieldOptions(tenantID, hierarchyID, agencyNames) {
    if ($jQuery("[id$=ddlTenant]").length <= 0) {
        return;
    }

    // debugger;
    if (parseInt(tenantID) > 0 && parseInt(hierarchyID) > 0) {

        //alert("Tenant ID is: " + tenantID + " and hierarchyId is: " + hierarchyID);
        $jQuery.ajax({
            type: "POST",
            url: '/ClinicalRotation/Default.aspx/GetAgencyHierarchyRotationFieldOptionSetting',
            data: "{'strTenantId': '" + tenantID + "', strhierarchyId: '" + hierarchyID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result != undefined) {

                    var res = JSON.parse(result.d);
                    //alert("IsDepartment_Required: " + res.RequiredFieldSettings.AHRFO_IsDepartment_Required
                    //    + "\nIsRotationName_Required: " + res.RequiredFieldSettings.AHRFO_IsRotationName_Required
                    //    + "\nIsTypeSpecialty_Required: " + res.RequiredFieldSettings.AHRFO_IsTypeSpecialty_Required
                    //    + "\nIsProgram_Required: " + res.RequiredFieldSettings.AHRFO_IsProgram_Required
                    //    + "\nIsCourse_Required: " + res.RequiredFieldSettings.AHRFO_IsCourse_Required
                    //    + "\nIsTerm_Required: " + res.RequiredFieldSettings.AHRFO_IsTerm_Required
                    //    + "\nIsUnitFloorLoc_Required: " + res.RequiredFieldSettings.AHRFO_IsUnitFloorLoc_Required
                    //    + "\nIsNoOfHours_Required: " + res.RequiredFieldSettings.AHRFO_IsNoOfHours_Required
                    //    + "\nIsRotDays_Required: " + res.RequiredFieldSettings.AHRFO_IsRotDays_Required
                    //    + "\nIsIP_Required: " + res.RequiredFieldSettings.AHRFO_IsIP_Required
                    //    + "\nIsStartTime_Required: " + res.RequiredFieldSettings.AHRFO_IsStartTime_Required
                    //    + "\nIsEndTime_Required: " + res.RequiredFieldSettings.AHRFO_IsEndTime_Required
                    //    + "\nIsSyllabusDocument_Required: " + res.RequiredFieldSettings.AHRFO_IsSyllabusDocument_Required
                    //    + "\nIsDeadlineDate_Required: " + res.RequiredFieldSettings.AHRFO_IsDeadlineDate_Required
                    //    );
                    if (res.RequiredFieldSettings.AHRFO_IsDepartment_Required != null
                        //|| res.RequiredFieldSettings.AHRFO_IsRotationName_Required != null
                        //|| res.RequiredFieldSettings.AHRFO_IsTypeSpecialty_Required != null
                        //|| res.RequiredFieldSettings.AHRFO_IsProgram_Required != null
                        //|| res.RequiredFieldSettings.AHRFO_IsCourse_Required != null
                        //|| res.RequiredFieldSettings.AHRFO_IsTerm_Required != null
                        //|| res.RequiredFieldSettings.AHRFO_IsUnitFloorLoc_Required != null
                        //|| res.RequiredFieldSettings.AHRFO_IsNoOfHours_Required != null
                        //|| res.RequiredFieldSettings.AHRFO_IsRotDays_Required != null
                        //|| res.RequiredFieldSettings.AHRFO_IsIP_Required != null
                        //|| res.RequiredFieldSettings.AHRFO_IsStartTime_Required != null
                        //|| res.RequiredFieldSettings.AHRFO_IsEndTime_Required != null
                        //|| res.RequiredFieldSettings.AHRFO_IsSyllabusDocument_Required != null
                        //|| res.RequiredFieldSettings.AHRFO_IsDeadlineDate_Required != null
                        ) {
                        if ($jQuery("[id$=hdnchkRotationFieldOptionValidator]").length > 0) {
                            $jQuery("[id$=hdnchkRotationFieldOptionValidator]").val("true");
                        }

                    }

                    if (res.RequiredFieldSettings.AHRFO_IsRotationName_Required) {
                        $jQuery("[id$=spnRotationName]").removeClass().addClass("reqd");
                        ValidatorEnable($jQuery("[id$=rfvRotationName]")[0], true);
                        $jQuery("[id$=rfvRotationName]").hide();
                    }
                    else if (!res.RequiredFieldSettings.AHRFO_IsRotationName_Required) {
                        $jQuery("[id$=spnRotationName]").removeClass().addClass("reqd controlHidden");
                        ValidatorEnable($jQuery("[id$=rfvRotationName]")[0], false);
                    }

                    //Type/Speciality
                    if (res.RequiredFieldSettings.AHRFO_IsTypeSpecialty_Required) {
                        $jQuery("[id$=spnTypeSpecialty]").removeClass().addClass("reqd");
                        ValidatorEnable($jQuery("[id$=rfvTypeSpecialtyAddEdit]")[0], true);
                        $jQuery("[id$=rfvTypeSpecialtyAddEdit]").hide();
                    }
                    else if (!res.RequiredFieldSettings.AHRFO_IsTypeSpecialty_Required) {
                        $jQuery("[id$=spnTypeSpecialty]").removeClass().addClass("reqd controlHidden");
                        ValidatorEnable($jQuery("[id$=rfvTypeSpecialtyAddEdit]")[0], false);
                    }

                    //Department
                    if (res.RequiredFieldSettings.AHRFO_IsDepartment_Required) {
                        $jQuery("[id$=spnDepartment]").removeClass().addClass("reqd");
                        ValidatorEnable($jQuery("[id$=rfvDepartment]")[0], true);
                        $jQuery("[id$=rfvDepartment]").hide();
                    }
                    else if (!res.RequiredFieldSettings.AHRFO_IsDepartment_Required) {
                        $jQuery("[id$=spnDepartment]").removeClass().addClass("reqd controlHidden");
                        ValidatorEnable($jQuery("[id$=rfvDepartment]")[0], false);
                    }

                    //Program
                    if (res.RequiredFieldSettings.AHRFO_IsProgram_Required) {
                        $jQuery("[id$=spnProgram]").removeClass().addClass("reqd");
                        ValidatorEnable($jQuery("[id$=rfvProgram]")[0], true);
                        $jQuery("[id$=rfvProgram]").hide();
                    }
                    else if (!res.RequiredFieldSettings.AHRFO_IsProgram_Required) {
                        $jQuery("[id$=spnProgram]").removeClass().addClass("reqd controlHidden");
                        ValidatorEnable($jQuery("[id$=rfvProgram]")[0], false);
                    }
                    //Course
                    if (res.RequiredFieldSettings.AHRFO_IsCourse_Required) {
                        $jQuery("[id$=spnCourse]").removeClass().addClass("reqd");
                        ValidatorEnable($jQuery("[id$=rfvCourse]")[0], true);
                        $jQuery("[id$=rfvCourse]").hide();
                    }
                    else if (!res.RequiredFieldSettings.AHRFO_IsCourse_Required) {
                        $jQuery("[id$=spnCourse]").removeClass().addClass("reqd controlHidden");
                        ValidatorEnable($jQuery("[id$=rfvCourse]")[0], false);
                    }
                    //Term
                    if (res.RequiredFieldSettings.AHRFO_IsTerm_Required) {
                        $jQuery("[id$=spnTerm]").removeClass().addClass("reqd");
                        ValidatorEnable($jQuery("[id$=rfvTerm]")[0], true);
                        $jQuery("[id$=rfvTerm]").hide();
                    }
                    else if (!res.RequiredFieldSettings.AHRFO_IsTerm_Required) {
                        $jQuery("[id$=spnTerm]").removeClass().addClass("reqd controlHidden");
                        ValidatorEnable($jQuery("[id$=rfvTerm]")[0], false);
                    }

                    // Unit/Floor or Location
                    if (res.RequiredFieldSettings.AHRFO_IsUnitFloorLoc_Required) {
                        $jQuery("[id$=spnUnitFloorLocation]").removeClass().addClass("reqd");
                        ValidatorEnable($jQuery("[id$=rfvUnitFloorLocation]")[0], true);
                        $jQuery("[id$=rfvUnitFloorLocation]").hide();
                    }
                    else if (!res.RequiredFieldSettings.AHRFO_IsUnitFloorLoc_Required) {
                        $jQuery("[id$=spnUnitFloorLocation]").removeClass().addClass("reqd controlHidden");
                        ValidatorEnable($jQuery("[id$=rfvUnitFloorLocation]")[0], false);
                    }

                    //Student
                    if (res.RequiredFieldSettings.AHRFO_IsNoOfStudents_Required) {
                        $jQuery("[id$=spnStudent]").removeClass().addClass("reqd");
                        ValidatorEnable($jQuery("[id$=rfvStudent]")[0], true);
                        $jQuery("[id$=rfvStudent]").hide();
                    }
                    else if (!res.RequiredFieldSettings.AHRFO_IsNoOfStudents_Required) {
                        $jQuery("[id$=spnStudent]").removeClass().addClass("reqd controlHidden");
                        ValidatorEnable($jQuery("[id$=rfvStudent]")[0], false);
                    }

                    //Recommended Hours
                    if (res.RequiredFieldSettings.AHRFO_IsNoOfHours_Required) {
                        $jQuery("[id$=spnRecommendedHrs]").removeClass().addClass("reqd");
                        ValidatorEnable($jQuery("[id$=rfvRecommendedHrs]")[0], true);
                        $jQuery("[id$=rfvRecommendedHrs]").hide();
                    }
                    else if (!res.RequiredFieldSettings.AHRFO_IsNoOfHours_Required) {
                        $jQuery("[id$=spnRecommendedHrs]").removeClass().addClass("reqd controlHidden");
                        ValidatorEnable($jQuery("[id$=rfvRecommendedHrs]")[0], false);
                    }
                    //Days
                    if (res.RequiredFieldSettings.AHRFO_IsRotDays_Required) {
                        $jQuery("[id$=spnDays]").removeClass().addClass("reqd");
                        ValidatorEnable($jQuery("[id$=rfvDays]")[0], true);
                        $jQuery("[id$=rfvDays]").hide();
                    }
                    else if (!res.RequiredFieldSettings.AHRFO_IsRotDays_Required) {
                        $jQuery("[id$=spnDays]").removeClass().addClass("reqd controlHidden");
                        ValidatorEnable($jQuery("[id$=rfvDays]")[0], false);
                    }

                    //Shift
                    if (res.RequiredFieldSettings.AHRFO_IsRotationShift_Required) {
                        $jQuery("[id$=spnShift]").removeClass().addClass("reqd");
                        ValidatorEnable($jQuery("[id$=rfvShift]")[0], true);
                        $jQuery("[id$=rfvShift]").hide();
                    }
                    else if (!res.RequiredFieldSettings.AHRFO_IsRotationShift_Required) {
                        $jQuery("[id$=spnShift]").removeClass().addClass("reqd controlHidden");
                        ValidatorEnable($jQuery("[id$=rfvShift]")[0], false);
                    }
                    //I/P
                    // debugger;
                    var IsInstAvailabilityDefined = "false";
                    hdnIsInstAvailabilityDefined = $jQuery("[id$=hdnIsInstAvailabilityDefined]");
                    if (hdnIsInstAvailabilityDefined != undefined && hdnIsInstAvailabilityDefined != null)
                        IsInstAvailabilityDefined = hdnIsInstAvailabilityDefined.val();

                    if (IsInstAvailabilityDefined != "True" && IsInstAvailabilityDefined != "true") {

                        if (res.RequiredFieldSettings.AHRFO_IsIP_Required) {
                            $jQuery("[id$=spnInsPre]").removeClass().addClass("reqd");
                            ValidatorEnable($jQuery("[id$=rfvInsPre]")[0], true);
                            $jQuery("[id$=rfvInsPre]").hide();
                        }
                        else if (!res.RequiredFieldSettings.AHRFO_IsIP_Required) {
                            $jQuery("[id$=spnInsPre]").removeClass().addClass("reqd controlHidden");
                            ValidatorEnable($jQuery("[id$=rfvInsPre]")[0], false);
                        }
                    }
                    //Start Time
                    if (res.RequiredFieldSettings.AHRFO_IsStartTime_Required) {
                        $jQuery("[id$=spnTime]").removeClass().addClass("reqd");
                        ValidatorEnable($jQuery("[id$=rfvStartTime]")[0], true);
                        $jQuery("[id$=rfvStartTime]").hide();
                    }
                    else if (!res.RequiredFieldSettings.AHRFO_IsStartTime_Required) {
                        $jQuery("[id$=spnTime]").removeClass().addClass("reqd controlHidden");
                        ValidatorEnable($jQuery("[id$=rfvStartTime]")[0], false);
                    }
                    //End Time
                    if (res.RequiredFieldSettings.AHRFO_IsEndTime_Required) {
                        $jQuery("[id$=spnTime]").removeClass().addClass("reqd");
                        ValidatorEnable($jQuery("[id$=rfvEndTime]")[0], true);
                        $jQuery("[id$=rfvEndTime]").hide();
                    }
                    else if (!res.RequiredFieldSettings.AHRFO_IsEndTime_Required) {
                        $jQuery("[id$=spnTime]").removeClass().addClass("reqd controlHidden");
                        ValidatorEnable($jQuery("[id$=rfvEndTime]")[0], false);
                    }

                    //Syllabus
                    if (res.RequiredFieldSettings.AHRFO_IsSyllabusDocument_Required) {
                        $jQuery("[id$=spnSyllabus]").removeClass().addClass("reqd");
                        // ValidatorEnable($jQuery("[id$=cstValidator]")[0], true);
                        // $jQuery("[id$=cstValidator]").hide();
                        $jQuery("[id$=hdnValidateFileUploadControl]").val("true");

                    }
                    else if (!res.RequiredFieldSettings.AHRFO_IsSyllabusDocument_Required) {
                        $jQuery("[id$=spnSyllabus]").removeClass().addClass("reqd controlHidden");
                        $jQuery("[id$=hdnValidateFileUploadControl]").val("false");
                    }


                    //Additional Documnets 4062
                    if (res.RequiredFieldSettings.AHRFO_IsAdditionalDocuments_Required) {
                        $jQuery("[id$=spnAdditional]").removeClass().addClass("reqd");
                        // ValidatorEnable($jQuery("[id$=cstValidator]")[0], true);
                        // $jQuery("[id$=cstValidator]").hide();
                        $jQuery("[id$=hdnValidateAdditionalControl]").val("true");

                    }
                    else if (!res.RequiredFieldSettings.AHRFO_IsAdditionalDocuments_Required) {
                        $jQuery("[id$=spnAdditional]").removeClass().addClass("reqd controlHidden");
                        $jQuery("[id$=hdnValidateAdditionalControl]").val("false");
                    }

                    //Deadline Date
                    if (res.RequiredFieldSettings.AHRFO_IsDeadlineDate_Required) {
                        $jQuery("[id$=spnDeadlineDate]").removeClass().addClass("reqd");
                        ValidatorEnable($jQuery("[id$=rfvDeadlineDate]")[0], true);
                        $jQuery("[id$=rfvDeadlineDate]").hide();
                    }
                    else if (!res.RequiredFieldSettings.AHRFO_IsDeadlineDate_Required) {
                        $jQuery("[id$=spnDeadlineDate]").removeClass().addClass("reqd controlHidden");
                        ValidatorEnable($jQuery("[id$=rfvDeadlineDate]")[0], false);
                    }
                    ////Days Before
                    //if (res.RequiredFieldSettings.AHRFO_IsDaysBefore_Required) {
                    //    $jQuery("[id$=spnDaysBefore]").removeClass().addClass("reqd");
                    //    ValidatorEnable($jQuery("[id$=rfvDaysBefore]")[0], true);
                    //    $jQuery("[id$=rfvDaysBefore]").hide();
                    //}
                    //else if (!res.RequiredFieldSettings.AHRFO_IsDaysBefore_Required) {
                    //    $jQuery("[id$=spnDaysBefore]").removeClass().addClass("reqd controlHidden");
                    //    ValidatorEnable($jQuery("[id$=rfvDaysBefore]")[0], false);
                    //}
                    //// Frequency
                    //if (res.RequiredFieldSettings.AHRFO_IsFrequency_Required) {
                    //    $jQuery("[id$=spnFrequency]").removeClass().addClass("reqd");
                    //    ValidatorEnable($jQuery("[id$=rfvFrequency]")[0], true);
                    //    $jQuery("[id$=rfvFrequency]").hide();
                    //}
                    //else if (!res.RequiredFieldSettings.AHRFO_IsFrequency_Required) {
                    //    $jQuery("[id$=spnFrequency]").removeClass().addClass("reqd controlHidden");
                    //    ValidatorEnable($jQuery("[id$=rfvFrequency]")[0], false);
                    //}
                    //EnableDisableValidator(res);
                }
            },
            error: function (error) {
                alert(error);
            }
        });
    }

    //UAT-3241

    if ((parseInt(hierarchyID)) > 0) {
        if (hierarchyID != undefined)
            var agencyIds = hierarchyID.split(',');
        if (agencyNames != undefined)
            var agencyNames = agencyNames.split(',');
        var count = agencyIds.length;
        var selAgencyIds = [];
        var selAgencyName = [];
        var isRepeated = 0;
        if (agencyNames != undefined && hierarchyID != undefined) {
            for (var i = 0 ; i < count; i++) {

                if ((jQuery.inArray(agencyIds[i], selAgencyIds)) == -1) {
                    selAgencyIds.push(agencyIds[i]);
                    selAgencyName.push(agencyNames[i]);
                }
            }
            var totalAgencyIds = selAgencyIds.length;
            if (totalAgencyIds > 1) {
                var SelectedAgencyNames = selAgencyName.join();
                $jQuery("[id$=lblAttention]")[0].innerHTML = "ATTENTION: You have associated this rotation with multiple agencies: <br/> <br/>" + SelectedAgencyNames +
                                   "<br/> <br/>" + "Please note that if a rotation is associated with multiple agencies, you may not be able to assign a requirements package to the rotation. <br/> <br/>" +
                                   "If the students in this rotation will be rotating at multiple facilities, please create a new rotation for each facility.";

                multipleHierarchyPopUp();
            }
        }
    }

}


//UAT-3241
function multipleHierarchyPopUp() {
    $jQuery("[id$=multipleHierarchyPopUpDiv]").attr("style", "display:block");
    $window.showDialog($jQuery("[id$=multipleHierarchyPopUpDiv]").clone().show(), {
        closeBtn: {
            autoclose: true, text: "OK", click: function () {
                $jQuery("[id$=lblAttention]")[0].innerHTML = "";
                $jQuery("[id$=multipleHierarchyPopUpDiv]").attr("style", "display:none !important;");
            }
        }
    }, 475, 'ATTENTION!');
    $jQuery("[id$=multipleHierarchyPopUpDiv]").attr("style", "display:none !important;");
    $('.msgbox').css('display', 'none');
    $('.info').css('display', 'none');
}



