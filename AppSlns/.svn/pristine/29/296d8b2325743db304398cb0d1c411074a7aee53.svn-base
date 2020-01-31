function CheckAll(id) {
    var gridInvitationID = $jQuery("[id$=grdInvitations]").attr("id");
    var masterTable = $find(gridInvitationID).get_masterTableView();
    var row = masterTable.get_dataItems();
    var isChecked = false;
    if (id.checked == true) {
        var isChecked = true;
    }
    for (var i = 0; i < row.length; i++) {
        if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled)) {
            masterTable.get_dataItems()[i].findElement("chkSelectItem").checked = isChecked; // for checking the checkboxes
        }
    }
}

function UnCheckHeader(id) {
    var checkHeader = true;
    //var masterTable = $find("<%= grdInvitations.ClientID %>").get_masterTableView();
    var gridInvitationID = $jQuery("[id$=grdInvitations]").attr("id");
    var masterTable = $find(gridInvitationID).get_masterTableView();
    var row = masterTable.get_dataItems();
    for (var i = 0; i < row.length; i++) {
        if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled)) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").checked)) {
                checkHeader = false;
                break;
            }
        }
    }
    $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
}

//click on link button while double click on any row of grid.
function grd_rwDbClick(s, e) {
    var _id = "btnEditNew";
    var b = e.get_gridDataItem().findControl(_id);
    if (b && typeof (b.click) != "undefined") { b.click(); }
}

function grd_rwDbClick_Rotation(s, e) {
    var _idViewDetail = "btnViewDetail";
    var _idEnterData = "btnEnterData";
    var btnViewDetail = e.get_gridDataItem().findControl(_idViewDetail);
    var btnEnterData = e.get_gridDataItem().findControl(_idEnterData);
    if (btnViewDetail && typeof (btnViewDetail.click) != "undefined") {
        btnViewDetail.click();
    }
    else if (btnEnterData && typeof (btnEnterData.click) != "undefined") {
        btnEnterData.click();
    }
}

function ValidateCheckBoxSelection(source, args) {
    var cntrlToValidate = $find($jQuery("[id$=cmbTenant]").attr("id"));
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

function CorrectFrmToExpirationDate(picker) {
    var date1 = $jQuery("[id$=dpExpirationFrm]")[0].control.get_selectedDate();
    var date2 = $jQuery("[id$=dpExpirationTo]")[0].control.get_selectedDate();
    if (date1 != null && date2 != null) {
        if (date1 > date2)
            $jQuery("[id$=dpExpirationTo]")[0].control.set_selectedDate(null);
    }
}

function SetMinDateExpiration(picker) {
    var date = $jQuery("[id$=dpExpirationFrm]")[0].control.get_selectedDate();
    if (date != null) {
        picker.set_minDate(date);
    }
    else {
        picker.set_minDate(minDate);
    }
}

function CorrectFrmToInvitationDate(picker) {
    var date1 = $jQuery("[id$=dpInvitationFrm]")[0].control.get_selectedDate();
    var date2 = $jQuery("[id$=dpInvitationTo]")[0].control.get_selectedDate();
    if (date1 != null && date2 != null) {
        if (date1 > date2)
            $jQuery("[id$=dpInvitationTo]")[0].control.set_selectedDate(null);
    }
}

function SetMinDateInvitation(picker) {
    var date = $jQuery("[id$=dpInvitationFrm]")[0].control.get_selectedDate();
    if (date != null) {
        picker.set_minDate(date);
    }
    else {
        picker.set_minDate(minDate);
    }
}

function CorrectFrmToLastViewedDate(picker) {
    var date1 = $jQuery("[id$=dpLastViewedFrm]")[0].control.get_selectedDate();
    var date2 = $jQuery("[id$=dpLastViewedTo]")[0].control.get_selectedDate();
    if (date1 != null && date2 != null) {
        if (date1 > date2)
            $jQuery("[id$=dpLastViewedTo]")[0].control.set_selectedDate(null);
    }
}

function SetMinDateLastViewed(picker) {
    var date = $jQuery("[id$=dpLastViewedFrm]")[0].control.get_selectedDate();
    if (date != null) {
        picker.set_minDate(date);
    }
    else {
        picker.set_minDate(minDate);
    }
}

/*UAT-1641:*/
 
function BindAgencyDropDownForInvitation() {
    //debugger;
    var comboBoxTenant = $find($jQuery("[id$=cmbTenant]")[0].id);
    if (IsAnyChangesInTenantSelectionForInvitation(comboBoxTenant)) {
        var selectedTenantIds = "";
        var screenName=$jQuery("[id$=hdnScreenNameID]")[0].value;
        if (screenName == "REQShares" && comboBoxTenant.get_checkedItems().length <= 0) {
                var defaulTenantIDs = $jQuery("[id$=hdnDefaultTenantIDs]")[0].value;
                selectedTenantIds = defaulTenantIDs;
            }
        else {
            for (var i = 0; i < comboBoxTenant.get_checkedItems().length ; i++) {
                selectedTenantIds += comboBoxTenant.get_checkedItems()[i].get_value();
                if (i != comboBoxTenant.get_checkedItems().length - 1) {
                    selectedTenantIds += ",";

                }
            }
        }
        var userID = $jQuery("[id$=hdnUserID]")[0].value;
        var hdnOrgUserID = $jQuery("[id$=hdnOrgUserID]")[0].value;
        var dropDownControlID = "ddlAgency";
        if (this.Page != undefined)
            Page.showProgress('Please wait...');
        $jQuery.ajax({
            type: "POST",
            url: '/ProfileSharing/Default.aspx/GetUserAgencyList',
            data: "{'selectedTenantIDs': '" + selectedTenantIds + "', userId: '" + userID + "', orgUserID: '" + hdnOrgUserID + "', isTabTypeInvitation: '" + true + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {
                if (Result != undefined) {
                    BindListToComboForInvitation(dropDownControlID, Result.d);
                } return false;
            },
            error: function (Result) {
            }
        });
        if (this.Page != undefined)
            Page.hideProgress();
    }
    else { return false; }
}

function BindListToComboForInvitation(controlId, result) {
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
                    if (comboItem._element.innerHTML != null && comboItem._element.innerHTML != undefined)
                    {
                        var str = '<label>' + comboItem._element.innerHTML + '</label>';
                        comboItem._element.innerHTML = str;
                    } 
                }
            }
            //combo.clearSelection();
            combo.commitChanges();
           // $jQuery("[id$=btnDoPostBack]").click();
        }
    }
}

function ComboBoxSelectedIdListForInvitation(sender) {
    var selectedTenantIdList = [];
    var combo = sender;
    var checkeditems = combo.get_checkedItems();
    for (i = 0; i < checkeditems.length; i++) {
        selectedTenantIdList.push(checkeditems[i].get_value());
    }
    return selectedTenantIdList;
}


function IsAnyChangesInTenantSelectionForInvitation(sender) {
    //debugger;
        var oldTenantIdList = [];
        var hdnPreviousTenantIds = $jQuery("[id$=hdnPreviousTenantIds]");
        if (hdnPreviousTenantIds.val() != "" && hdnPreviousTenantIds.val() != null && hdnPreviousTenantIds.val() != undefined) {
            oldTenantIdList = hdnPreviousTenantIds.val().split(',');
        }
        var selectedIdList = ComboBoxSelectedIdListForInvitation(sender);
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

/*UAT-2362:*/
function BindAgencyDropDownForRotationSearch() {
    var comboBoxTenant = $find($jQuery("[id$=cmbTenant]")[0].id);

    var btnPostBack = $jQuery("[id$=btnPostBack]");

    if (this.Page != undefined)
        Page.showProgress('Please wait...');

    if (btnPostBack != undefined && btnPostBack != null)
    {
        btnPostBack.click();
    }
    if (this.Page != undefined)
        Page.hideProgress();

    if (IsAnyChangesInTenantSelection(comboBoxTenant)) {
        var selectedTenantIds = "";
        for (var i = 0; i < comboBoxTenant.get_checkedItems().length ; i++) {
            selectedTenantIds += comboBoxTenant.get_checkedItems()[i].get_value();
            if (i != comboBoxTenant.get_checkedItems().length - 1) {
                selectedTenantIds += ",";

            }
        }
        var userID = $jQuery("[id$=hdnUserID]")[0].value;
        //var dropDownControlID = "ddlAgency";
        //$jQuery.ajax({
        //    type: "POST",
        //    url: '/ProfileSharing/Default.aspx/GetUserAgencyList',
        //    data: "{'selectedTenantIDs': '" + selectedTenantIds + "', userId: '" + userID + "', orgUserID: '" + 0 + "', isTabTypeInvitation: '" + false + "'}",
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    success: function (Result) {
        //        if (Result != undefined) {
        //            BindListToCombo(dropDownControlID, Result.d);
        //        }
        //    },
        //    error: function (Result) {
        //    }
        //});
    }
    else { return false; }
}
/*UAT-2362:*/
function BindListToCombo(controlId, result) {
    var control = $jQuery("[id$=" + controlId + "]")[0];
    if (control != undefined || control != null) {
        var combo = $find(control.id);
        if (combo != undefined || combo != null) {
            combo.trackChanges();
            var selectedItem = combo.get_selectedItem();
            var items = combo.get_items();
      
            items.clear();
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                    comboItem.set_text(result[i].Name);
                    comboItem.set_value(result[i].ID); 
                    items.add(comboItem);
                    comboItem._element.style.fontWeight = 'bold';
                    if (comboItem._element.innerHTML != null && comboItem._element.innerHTML != undefined) {
                        var str = '<label>' + comboItem._element.innerHTML + '</label>';
                        comboItem._element.innerHTML = str;
                    }
                }
            }
   
            combo.commitChanges();
            //$jQuery("[id$=btnDoPostBack]").click();
        }
    }
}

