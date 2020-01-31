//$jQuery.page_load(){}
var CascadingCount = 0;
$jQuery(document).ready(function () {
    HideTextBox(0);
});

$page.add_pageLoaded(function () {
    HideTextBox(0);
    checkTheDisplay();
    SetTheDataForIsDecisionFields();
    SetDecisionForEmploymentEndDateField(true);
    //UAT-2866
    SetGrayOutProperties();
    // DisableEnter();
});

//Function that get the data that to be loaded in the next drop down.
function GetDataForDropDown(e) {    
    if (e._value != "" && e._value != 0) {
        var lstIds = e._element.id.split('_');
        var type = lstIds[lstIds.length - 4];
        var instanceId = lstIds[lstIds.length - 3];
        var attributeGroupId = lstIds[lstIds.length - 2];
        var perviousSearchId = GetPreviousDripDownValue(type, instanceId);
        if (perviousSearchId == "" && type == "State") {
            perviousSearchId = "UNITED STATES";
        }
        var IsLocationTenant = $jQuery('[id$=hdnIsLocationServiceTenant]').val();
        var SelectedCountry = '';
        if (IsLocationTenant == 'True' && type == 'Country') {
            SelectedCountry = e._value;
        }

        var dataurl = JSON.stringify({ 'searchId': e._value, 'previousSearchId': perviousSearchId, 'type': type });
        //UAT-2842
        var url = "Default.aspx/GetDataForDropDown";
        var IsAdminCreateOrder = $jQuery("[id$=hdnIsAdminCreateOrder]").val();
        if (IsAdminCreateOrder != undefined && IsAdminCreateOrder == "True") {
            url = "CustomFormPage.aspx/GetDataForDropDown";
        }

        $jQuery.ajax({
            type: "POST",
            url: url,
            data: dataurl,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {
                var newDropDownType = GetDropDownType(type);
                //var optItems = CreateHtml(Result.d);
                var control = $jQuery('.J' + newDropDownType + '');
                var newDropDown = CheckForTheCorrectControl(control, instanceId);
                if (newDropDown != null) {
                    var combo = $find(newDropDown.id);

                    if (IsLocationTenant == 'True') {
                        combo.enable();
                        var req = jQuery("[id*=lblState_" + attributeGroupId + "_" + instanceId + "]").siblings();
                        if (req[0] != undefined) {
                            req[0].style.display = "inline";
                        }
                    }
                    if (IsLocationTenant == 'True' && type == 'Country' && (SelectedCountry != 'UNITED STATES')) {
                        CustomizeTheFields(type, false, instanceId, attributeGroupId, true);
                    }
                    else {
                        if (Result.d.length > 0) {
                            bindCombo(combo, Result.d, newDropDownType, instanceId);
                            CustomizeTheFields(type, true, instanceId, attributeGroupId);
                        }
                        else {
                            CustomizeTheFields(type, false, instanceId, attributeGroupId);
                        }
                    }
                }
                else {
                    CustomizeTheFields(type, false, instanceId, attributeGroupId, false);
                }

            },
            error: function (Result) {
                alert("Error");
            }
        });
    }
}

//Bind the drop down with the data.
function bindCombo(combo, result, newDropDownType, instanceId) {
    combo.trackChanges();
    var items = combo.get_items();
    items.clear();
    combo.clearSelection();
    for (var i = 0; i < result.length; i++) {
        var comboItem = new Telerik.Web.UI.RadComboBoxItem();
        comboItem.set_text(result[i].Name);
        comboItem.set_value(result[i].Name);
        items.add(comboItem);
        if (result[i].ID == 0 || result.length == 1) {
            comboItem.select();
        }
    }
    combo.commitChanges();
    clearAllSelection(newDropDownType, instanceId);
}

function clearAllSelection(type, instanceId) {
    var lst = ["Country", "State", "City", "ZipCode", "County"];
    var Hide = 0;
    for (var i = 0; i < lst.length; i++) {
        var typeOfDropDown = lst[i];
        if (typeOfDropDown == type || Hide == 1) {
            Hide = Hide + 1;
        }
        if (Hide == 2) {
            var control = $jQuery('.J' + typeOfDropDown + '');
            var newDropDown = CheckForTheCorrectControl(control, instanceId);
            if (newDropDown != undefined && newDropDown != null) {
                var combo = $find(newDropDown.id);
                combo.clearSelection();
            }
        }
    }

}

//Gets the next drop down to be loaded.
function GetDropDownType(type) {
    switch (type) {
        case "Country": return "State";
        case "State": return "City";
        case "City": return "ZipCode";
        case "Zip Code": return "County";
    }
}

function GetPreviousDropDownType(type) {
    switch (type) {
        case "Country": return "";
        case "State": return "Country";
        case "City": return "State";
        case "Zip Code": return "City";
    }
}

function GetPreviousDripDownValue(type, instanceId) {
    var previousType = GetPreviousDropDownType(type);
    if (previousType != "") {
        var control = $jQuery('.J' + previousType + '');
        var dropDown = CheckForTheCorrectControl(control, instanceId);
        if (dropDown != null) {
            return dropDown.control.get_selectedItem().get_value();
        }
    }
    return "";
}



//It show hides the drop down n text boxes based on special condition of country
function CustomizeTheFields(type, IsShowHide, InstanceId, attributeGroupId, IsStateDisabled) {
    var lst = ["Country", "State", "City", "ZipCode", "County"];
    var Hide = 0;
    var index = GetTheIndexForgroupId(attributeGroupId);
    if (IsShowHide == false) {
        var IsLocationTenant = $jQuery('[id$=hdnIsLocationServiceTenant]').val();
        for (var i = 0; i < lst.length; i++) {
            var typeOfDropDown = lst[i];
            if (typeOfDropDown == type || Hide == 1) {
                Hide = Hide + 1;
            }
            if (Hide == 2 && (IsLocationTenant == 'False' || typeOfDropDown != 'State')) {
                var dropDownControl = $jQuery('.J' + typeOfDropDown + '');
                var textBoxControl = $jQuery('.txt' + typeOfDropDown + '');
                var riteDropDownControl = CheckForTheCorrectControl(dropDownControl, InstanceId);
                if (riteDropDownControl != null) {
                    riteDropDownControl.style.display = "none";
                    var combo = $find(riteDropDownControl.id);
                    combo.trackChanges();
                    var items = combo.get_items();
                    items.clear();
                    combo.clearSelection();
                    combo.commitChanges();
                    StoreIdOfHiddenControl(riteDropDownControl, "drop", InstanceId, index);
                    disableValidator(typeOfDropDown, InstanceId, GetAttributeMappingId(riteDropDownControl.id));

                }
                var riteTextBoxControl = CheckForTheCorrectControl(textBoxControl, InstanceId);
                if (riteTextBoxControl != null) {
                    RemoveIdOfHiddenControl(riteTextBoxControl, "txt", InstanceId, index);
                    (riteTextBoxControl).style.display = "block";
                }

            }

            if (IsLocationTenant == 'True' && typeOfDropDown == 'State' && IsStateDisabled) {
                var dropDownControl = $jQuery('.J' + typeOfDropDown + '');
                var riteDropDownControl = CheckForTheCorrectControl(dropDownControl, InstanceId);
                if (riteDropDownControl != null) {
                    var combo = $find(riteDropDownControl.id);
                    combo.trackChanges();
                    var items = combo.get_items();
                    items.clear();
                    combo.clearSelection();
                    combo.commitChanges();
                    combo.disable();
                    var requiredFieldDropDown = $jQuery("[id$=rfv_drop" + typeOfDropDown + "_" + InstanceId + "_" + GetAttributeMappingId(riteDropDownControl.id) + "]");
                    if (requiredFieldDropDown[0] != undefined) {
                        ValidatorEnable(requiredFieldDropDown[0], false);
                        var req = jQuery("[id*=lblState_" + attributeGroupId + "_" + InstanceId + "]").siblings();
                        if (req[0] != undefined) {
                            req[0].style.display = "none";
                        }
                    }
                }
            }
        }
    }
    else {
        for (var i = 0; i < lst.length; i++) {
            var typeOfDropDown = lst[i];
            var dropDownControl = $jQuery('.J' + typeOfDropDown + '');
            var textBoxControl = $jQuery('.txt' + typeOfDropDown + '');
            var riteDropDownControl = CheckForTheCorrectControl(dropDownControl, InstanceId);
            if (riteDropDownControl != null) {
                riteDropDownControl.style.display = "block";
                RemoveIdOfHiddenControl(riteDropDownControl, "drop", InstanceId, index);
                EnableValidator(typeOfDropDown, InstanceId, GetAttributeMappingId(riteDropDownControl.id));
            }
            var riteTextBoxControl = CheckForTheCorrectControl(textBoxControl, InstanceId);
            if (riteTextBoxControl != null) {
                riteTextBoxControl.style.display = "none";
                StoreIdOfHiddenControl(riteTextBoxControl, "txt", InstanceId, index);
                riteTextBoxControl.value = "";
            }
        }
    }
}

function GetTheIndexForgroupId(attributeGroupId) {
    var lstGroupId = $jQuery('[id$=hdnGroupId]');
    if (lstGroupId.length > 0) {
        for (var i = 0; i < lstGroupId.length; i++) {
            if (lstGroupId[i].value == attributeGroupId)
                return i;
        }
    }
    return 0;
}
//Initially hide all the text boxes corresponding to the datatype country, state,county,city zip code
function HideTextBox(start) {
    var textBoxes = $jQuery('.classTxt');
    var instanceHiddenField = $jQuery('[id$=hdnInstanceId]');
    var instanceHdnDropDown = $jQuery('[id$=hdnHiddenDropDownIds]');
    var instanceHdnTextBox = $jQuery('[id$=hdnHiddenTextBoxIds]');
    var lstGroupId = $jQuery('[id$=hdnGroupId]');
    if (textBoxes.length > 0 && lstGroupId[0] != undefined) {
        for (var i = 0; i < instanceHdnDropDown.length; i++) {
            var hiddenFieldCustomLoad = $jQuery("[id$=hdnGroupidandIntanceNumber]");
            var groupId = lstGroupId[i].value;
            if (instanceHiddenField[i] != undefined && instanceHiddenField[i].value == "")
                var numberofInstance = GetInstanceIdOfParticularGroup(hiddenFieldCustomLoad, groupId);
            else
                var numberofInstance = instanceHiddenField[i].value;

            var actualDropDownHiddenList = instanceHdnDropDown[i];
            var actualTextBoxHiddenList = instanceHdnTextBox[i];
            if (actualDropDownHiddenList != null && actualTextBoxHiddenList != null) {
                for (var k = 0; k < numberofInstance; k++) {
                    if (CheckForTheExistenceIfInstance(k + 1, actualDropDownHiddenList.value, actualTextBoxHiddenList.value)) {
                        for (var j = 0; j < textBoxes.length; j++) {
                            if (GetInstanceId(textBoxes[j].id) == (k + 1) && GetAttributeId(textBoxes[j].id) == groupId) {
                                textBoxes[j].style.display = "none";
                                StoreIdOfHiddenControl(textBoxes[j], "txt", k + 1, i);
                            }
                        }
                    }
                }
            }
        }
    }
}


function CheckForTheExistenceIfInstance(instanceID, dropDownValueLst, textBoxValueList) {
    if (dropDownValueLst == "" && textBoxValueList == "") {
        return true;
    }
    else {
        var lstDrop = dropDownValueLst.split(':');
        var lstTxt = textBoxValueList.split(':');
        for (var i = 0; i < lstDrop.length; i++) {
            var getInstance = lstDrop[i].split('_');
            if (getInstance[0] != undefined && getInstance[0] == instanceID) {
                return false;
            }
        }
        for (var i = 0; i < lstTxt.length; i++) {
            var getInstance = lstTxt[i].split('_');
            if (getInstance[0] != undefined && getInstance[0] == instanceID) {
                return false;
            }
        }
        return true;
    }
}


//This is to prevent the show hide state of drop down and text box after page load
function checkTheDisplay() {
    var lst = ["State", "City", "ZipCode", "County"];
    var instanceHiddenField = $jQuery('[id$=hdnInstanceId]');
    var instanceHdnDropDown = $jQuery('[id$=hdnHiddenDropDownIds]');
    var instanceHdnTextBox = $jQuery('[id$=hdnHiddenTextBoxIds]');
    var lstGroupId = $jQuery('[id$=hdnGroupId]');
    var hiddenFieldCustomLoad = $jQuery("[id$=hdnGroupidandIntanceNumber]");
    var IsLocationTenant = $jQuery('[id$=hdnIsLocationServiceTenant]').val();
    if (instanceHiddenField[0] != undefined) {
        for (var i = 0; i < lst.length; i++) {
            var typeOfDropDown = lst[i];
            var lstDropDown = $jQuery('.J' + typeOfDropDown + '');
            var lstTextBox = $jQuery('.txt' + typeOfDropDown + '');

            for (var k = 0; k < lstGroupId.length; k++) {
                var groupId = lstGroupId[k].value;
                if (instanceHiddenField[k] != undefined && instanceHiddenField[k].value == "")
                    var numberofInstance = GetInstanceIdOfParticularGroup(hiddenFieldCustomLoad, groupId);
                else
                    var numberofInstance = instanceHiddenField[k].value;
                for (var inst = 0; inst < numberofInstance; inst++) {
                    var hdnValueDropDown = $jQuery('[id$=hdnHiddenDropDownIds]')[k].value;
                    var hdnValueTextBox = $jQuery('[id$=hdnHiddenTextBoxIds]')[k].value;
                    if (lstDropDown[0] != undefined) {
                        for (var j = 0; j < lstDropDown.length; j++) {
                            if (GetInstanceId(lstDropDown[j].id) == (inst + 1)) {
                                var idDrop = GetIdForTheHiddenField(lstDropDown[j]);
                                if (GetAttributeId(lstDropDown[j].id) == groupId) {
                                    if (IsValueToBeAdded(idDrop, hdnValueDropDown) && lstTextBox[j]) {
                                        lstDropDown[j].style.display = "block";
                                        //UAT 3573
                                        var CountrySelectedControl = $jQuery('[id*=dropDown_Country_' + (inst + 1) + ']')[0];
                                        var CountrySelected = "";
                                        if (CountrySelectedControl != undefined || CountrySelectedControl != null) {
                                            CountrySelected = CountrySelectedControl.value;
                                        }
                                        if (IsLocationTenant && CountrySelected != 'UNITED STATES' && typeOfDropDown == 'State') {
                                            var req = jQuery("[id*=lblState_" + groupId + "_" + (inst + 1) + "]").siblings();
                                            if (req[0] != undefined) {
                                                req[0].style.display = "none";
                                            }
                                            var requiredFieldDropDown = $jQuery("[id$=rfv_drop" + typeOfDropDown + "_" + (inst + 1) + "_" + GetAttributeMappingId(lstDropDown[j].id) + "]");
                                            if (requiredFieldDropDown[0] != undefined) {
                                                ValidatorEnable(requiredFieldDropDown[0], false);
                                            }

                                        }
                                        else {
                                            EnableValidator(typeOfDropDown, inst + 1, GetAttributeMappingId(lstDropDown[j].id));
                                        }
                                    }
                                    else {
                                        lstDropDown[j].style.display = "none";
                                        disableValidator(typeOfDropDown, inst + 1, GetAttributeMappingId(lstDropDown[j].id));
                                    }
                                }
                            }
                            if (GetInstanceId(lstTextBox[j].id) == (inst + 1)) {
                                var idText = GetIdForTheHiddenField(lstTextBox[j]);
                                if (GetAttributeId(lstTextBox[j].id) == groupId) {
                                    if (IsValueToBeAdded(idText, hdnValueTextBox)) {
                                        lstTextBox[j].style.display = "block";
                                    }
                                    else {
                                        lstTextBox[j].style.display = "none";
                                    }
                                }
                            }
                        }
                    }
                }//instance
            }//group
        }//other


    }
}

//This methods checks whether the control is of the same instance.
function CheckForTheCorrectControl(control, instanceId) {
    if (control[0] != undefined) {
        for (var i = 0; i < control.length; i++) {
            var lst = control[i].id.split('_');
            if (lst[lst.length - 3] == instanceId) { return control[i]; }
        }
    }
    return null;
}

//Stores the value in the hidden field to avoid the drop down 
//for whom the value is to be considered
function StoreIdOfHiddenControl(Control, type, instanceId, index) {
    if (type == "drop") {
        var hiddenField = $jQuery('[id$=hdnHiddenDropDownIds]');
    }
    else {
        var hiddenField = $jQuery('[id$=hdnHiddenTextBoxIds]');
    }
    var id = "";
    if (hiddenField[index] != undefined) {
        if (hiddenField[index].value == "") {
            id = GetIdForTheHiddenField(Control);
            hiddenField[index].value = id;
        }
        else {
            var hdnVal = hiddenField[index].value;
            id = GetIdForTheHiddenField(Control);
            if (IsValueToBeAdded(id, hdnVal)) {
                hdnVal = hdnVal + ":" + id;
                hiddenField[index].value = hdnVal;
            }
        }
    }
}

//All the records that are now display need to have their ids
//removed from the hidden field.
function RemoveIdOfHiddenControl(Control, type, instanceId, index) {
    if (type == "drop") {
        var hiddenField = $jQuery('[id$=hdnHiddenDropDownIds]');
    }
    else {
        var hiddenField = $jQuery('[id$=hdnHiddenTextBoxIds]');
    }
    var id = "";
    if (hiddenField[index] != undefined) {
        if (hiddenField[index].value != "")
            var hdnVal = hiddenField[index].value;
        id = GetIdForTheHiddenField(Control);
        if (hdnVal != undefined) {
            var newValue = GetRemovedValue(hdnVal.split(':'), id);
            hiddenField[index].value = newValue;
        }
    }
}


//This gets the id of the control as combination of :
//1.instanceID
//2.groupId
//3.AttributeId
function GetIdForTheHiddenField(control) {
    var ids = control.id.split('_');
    var desiredId = "";
    for (i = (ids.length - 3) ; i < ids.length; i++) {
        if (desiredId == "") {
            desiredId = ids[i];
        }
        else {
            desiredId = desiredId + "_" + ids[i];
        }
    }
    return desiredId;
}

function IsValueToBeAdded(id, hdnVal) {
    var arrayOfIds = hdnVal.split(':');
    if (($jQuery.inArray(id, arrayOfIds) == -1)) {
        return true;
    }
    return false;
}

function GetRemovedValue(y, removeItem) {
    var newValues = "";
    y = $jQuery.grep(y, function (value) {
        return value != removeItem;
    });

    for (var i = 0; i < y.length; i++) {
        if (newValues == "") {
            newValues = y[i];
        }
        else {
            newValues = newValues + ":" + y[i];
        }
    }

    return newValues;
}

function GetInstanceId(controlId) {
    var idArray = controlId.split('_');
    var length = idArray.length;
    return idArray[length - 3];
}

function GetAttributeMappingId(controlId) {
    var idArray = controlId.split('_');
    var length = idArray.length;
    return idArray[length - 1];
}

function GetAttributeId(controlId) {
    var idArray = controlId.split('_');
    var length = idArray.length;
    return idArray[length - 2];
}

function disableValidator(typeOfDropDown, instanceID, attributeMappingId) {
    var requiredFieldDropDown = $jQuery("[id$=rfv_drop" + typeOfDropDown + "_" + instanceID + "_" + attributeMappingId + "]");
    if (requiredFieldDropDown[0] != undefined)
    {
        ValidatorEnable(requiredFieldDropDown[0], false);
    }
    var requiredFieldTextBox = $jQuery("[id$=rfv_txt" + typeOfDropDown + "_" + instanceID + "_" + attributeMappingId + "]");
    if (requiredFieldTextBox[0] != undefined) {
        ValidatorEnable(requiredFieldTextBox[0], true);
        requiredFieldTextBox.hide();
    }
}

function EnableValidator(typeOfDropDown, instanceID, attributeMappingId) {
    var requiredFieldDropDown = $jQuery("[id$=rfv_drop" + typeOfDropDown + "_" + instanceID + "_" + attributeMappingId + "]");
    if (requiredFieldDropDown[0] != undefined) {
        ValidatorEnable(requiredFieldDropDown[0], true);
        requiredFieldDropDown.hide();
    }
    var requiredFieldTextBox = $jQuery("[id$=rfv_txt" + typeOfDropDown + "_" + instanceID + "_" + attributeMappingId + "]");
    if (requiredFieldTextBox[0] != undefined) {
        ValidatorEnable(requiredFieldTextBox[0], false);
    }
}


function SetInstanceId(e) {
    var groupId = e.get_cssClass();
    var hiddenField = null;
    //UAT-2855
    var IsAdminCreateOrder = $jQuery("[id$=hdnIsAdminCreateOrder]").val();
    if (IsAdminCreateOrder != undefined && IsAdminCreateOrder == "True") {
        hiddenField = $jQuery("[id$=pnlLoader]").find("[id$=hdnInstanceId]");
    } else {
        hiddenField = $jQuery("[id$=hdnInstanceId]");
    }

    var hiddenFieldCustomLoad = $jQuery("[id$=hdnGroupidandIntanceNumber]");

    var hdnGroupidandIntanceNumberMain = $jQuery("[id$=hdnGroupidandIntanceNumberMain]");
    if (hiddenField.length > 0 && ValidatePage() == true) {
        var hiddeFieldParents = hiddenField.parent();
        if (hiddeFieldParents.length > 0) {
            for (var i = 0; i < hiddeFieldParents.length; i++) {
                if (hiddeFieldParents[i].className == groupId) {
                    if (hiddenField[i].value == "") {
                        hiddenField[i].value = 2;
                        SetInstanceValue(hiddenFieldCustomLoad, groupId, 2);
                        break;
                    }
                    else {
                        var x = hiddenField[i].value;
                        hiddenField[i].value = parseInt(x) + 1;
                        SetInstanceValue(hiddenFieldCustomLoad, groupId, parseInt(x) + 1);
                        break;
                    }
                }
            }
        }
        if (hdnGroupidandIntanceNumberMain != undefined && hdnGroupidandIntanceNumberMain.length > 0) {
            hdnGroupidandIntanceNumberMain.val(hiddenFieldCustomLoad.val());
        }
    }
    //UAT-2063:Combine the screens to add new Alias and add new locations
    var groupIdToSetAutoFocus = $jQuery("[id$=hdnAutofocusGroupID]");
    if (groupIdToSetAutoFocus != undefined && groupIdToSetAutoFocus != null && groupIdToSetAutoFocus.length > 0) {
        groupIdToSetAutoFocus.val(groupId);
    }
}


function SetInstanceValue(hiddenFieldCustomLoad, groupId, instanceId) {
    if (hiddenFieldCustomLoad.val() == "") {
        hiddenFieldCustomLoad.val(groupId + "_" + instanceId + ":");
    }
    else {
        var valueForHiddenField = CheckValue(hiddenFieldCustomLoad.val(), groupId, instanceId);
        hiddenFieldCustomLoad.val(valueForHiddenField);
    }
}

function CheckValue(value, groupId, instanceId) {
    var lstValues = value.split(':');
    var actualValue = "";
    var previousValue = "";
    var finalValue = "";
    if (lstValues.length > 0) {
        for (var i = 0; i < lstValues.length; i++) {
            var lstOfInstance = lstValues[i].split('_');
            if (lstOfInstance[0] == groupId) {
                previousValue = lstValues[i];
                actualValue = groupId + "_" + instanceId;
                finalValue += actualValue + ":";
            }
            else {
                if (lstValues[i] != "")
                    finalValue += lstValues[i] + ":";
            }
        }
    }
    if (actualValue == "") {
        finalValue += groupId + "_" + instanceId + ":";
    }
    return finalValue;
}

function GetInstanceIdOfParticularGroup(hiddenField, groupId) {
    if (hiddenField.val() == "")
        return 1;
    else {
        var lstValues = value.split(':');
        for (var i = 0; i < lstValues.length; i++) {
            var lstOfInstance = lstValues[i].split('_');
            if (lstOfInstance[0] == groupId) {
                return lstOfInstance[1];
            }
        }
        return 1;
    }
}

//Hide panel functionality
function HideTheCurrentInstance(e) {
    //var panelUniqueId = e.className;
    var panelUniqueId = e.id;

    var panels = $jQuery("[id$=mainDiv_" + panelUniqueId + "]");
    if (panels.length > 0) {
        panels.slideUp();
        //if ($jQuery("[id$=CommandBar]")[0].style.display = "none") {
        //    $jQuery("[id$=CommandBar]")[0].style.display = "block";
        //}
        ShowCorrectCommandBar(panelUniqueId.split('_')[0]);
        var hdnHiddenControl = $jQuery("[id$=hdnHiddenPanels]");
        //UAT-2842
        var hdnHiddenPanelsMain = $jQuery("[id$=hdnHiddenPanelsMain]");
        if (hdnHiddenControl.length > 0) {
            var hiddenValue = hdnHiddenControl.val();
            var lstHidenPanels = hiddenValue.split(':');
            if (lstHidenPanels.length > 0) {
                if ($jQuery.inArray(panelUniqueId, lstHidenPanels) == -1) {
                    if (hiddenValue == "") {
                        hiddenValue = panelUniqueId + ":";
                    }
                    else {
                        hiddenValue = hiddenValue + panelUniqueId + ":";
                    }
                    hdnHiddenControl.val(hiddenValue);
                }
            }

            ResetLable(panelUniqueId.split('_')[0]);
            disableAvailableRequiredFields(panelUniqueId.split('_')[1]);

            //UAT-2842
            if (hdnHiddenPanelsMain != undefined && hdnHiddenPanelsMain.length > 0) {
                hdnHiddenPanelsMain.val(hdnHiddenControl.val());
            }
        }
    }
}

function ResetLable(groupId) {
    var sectionLabel = $jQuery("[id*=lblHeader_]");
    if (sectionLabel.length > 0) {
        var presentInstance = parseInt(1);
        var visibleSectionIndexes = [];
        for (var i = 0; i < sectionLabel.length; i++) {
            var sectionList = sectionLabel[i].id.split('_');
            if (sectionList[sectionList.length - 2] == groupId) {
                if (CheckExistanceOdHidden(groupId, parseInt(sectionLabel[i].className))) {
                    var previousText = sectionLabel[i].innerHTML;
                    var array = previousText.split('-');
                    visibleSectionIndexes.push(i);
                    if (array.length > 0) {
                        var newsectionTitle = "";
                        array[array.length - 1] = presentInstance;
                        presentInstance = presentInstance + 1;
                        for (var j = 0; j < array.length; j++) {
                            if (newsectionTitle == "")
                                newsectionTitle = array[j];
                            else
                                newsectionTitle = newsectionTitle + "- " + array[j];
                        }
                        sectionLabel[i].innerHTML = newsectionTitle;
                    }
                }
            }
        }

        if (visibleSectionIndexes.length === 1) {
            var previousText = sectionLabel[visibleSectionIndexes[0]].innerHTML;
            var array = previousText.split('-');
            sectionLabel[visibleSectionIndexes[0]].innerHTML = array[0];
        }
    }
}

function CheckExistanceOdHidden(groupId, instanceId) {
    var hidden = $jQuery("[id$=hdnHiddenPanels]");
    if (hidden.length > 0) {
        var hiddenValues = hidden.val();
        var lstOfgroupInstanceHidden = hiddenValues.split(':');
        if (lstOfgroupInstanceHidden.length > 0) {
            for (var i = 0; i < lstOfgroupInstanceHidden.length ; i++) {
                var seperateDataOfGroupInstance = lstOfgroupInstanceHidden[i].split('_');
                if (seperateDataOfGroupInstance[0] == groupId && seperateDataOfGroupInstance[1] == instanceId)
                { return false; }
            }
        }
    }
    return true;
}

function disableAvailableRequiredFields(instanceId) {
    var requiredFields = $jQuery(".errmsg");
    if (requiredFields.length > 0) {
        for (var i = 0; i < requiredFields.length; i++) {
            var id = requiredFields[i].id.split('_');
            if (id[id.length - 2] == instanceId) {
                ValidatorEnable(requiredFields[i], false);
            }
        }
    }
}

function ShowCorrectCommandBar(groupId) {
    var commandBar = $jQuery("[id$=CommandBar]");
    if (commandBar.length > 0) {
        for (var i = 0; i < commandBar.length; i++) {
            if (commandBar[i].style.display = "none" && commandBar[i].className == groupId) {
                commandBar[i].style.display = "block";
            }
        }
    }
}

//function DisableEnter() {
//    var code = e.keyCode || e.which;
//    if (code == 13) {
//        e.preventDefault();
//        return false;
//    }
//}

$jQuery(document).keypress(function (e) {
    if (e.which == 13) return false;
});

//UAT:2065
//Hide panel functionality for Supplemental Order
function HideCurrentInstance(e) {
    var panelUniqueId = e.id;
    var panels = $jQuery("[id$=pnlInner_" + panelUniqueId + "]");

    if (panels.length > 0) {
        panels.slideUp();
        ShowCorrectCommandBar(panelUniqueId.split('_')[0]);
        var hdnHiddenControl = $jQuery("[id$=hdnHiddenPanels]");

        if (hdnHiddenControl.length > 0) {
            var hiddenValue = hdnHiddenControl.val();
            var lstHidenPanels = hiddenValue.split(':');
            if (lstHidenPanels.length > 0) {
                if ($jQuery.inArray(panelUniqueId, lstHidenPanels) == -1) {
                    if (hiddenValue == "") {
                        hiddenValue = panelUniqueId + ":";
                    }
                    else {
                        hiddenValue = hiddenValue + panelUniqueId + ":";
                    }
                    hdnHiddenControl.val(hiddenValue);
                }
            }
            disableAvailableRequiredFields(panelUniqueId.split('_')[1]);
        }
    }
}

//Hide panel functionality for Read Only Supplemental Order
function HideReadOnlyCurrentInstance(e) {
    var panelUniqueId = e.id;
    var panels = $jQuery("[id$=pnlInner_" + panelUniqueId + "]");

    if (panels.length > 0) {
        panels.slideUp();

        var hdnHiddenControl = $jQuery("[id$=hdnHiddenReadOnlyPanels]");
        if (hdnHiddenControl.length > 0) {
            var hiddenValue = hdnHiddenControl.val();
            var lstHidenPanels = hiddenValue.split(':');
            if (lstHidenPanels.length > 0) {
                if ($jQuery.inArray(panelUniqueId, lstHidenPanels) == -1) {
                    if (hiddenValue == "") {
                        hiddenValue = panelUniqueId + ":";
                    }
                    else {
                        hiddenValue = hiddenValue + panelUniqueId + ":";
                    }
                    hdnHiddenControl.val(hiddenValue);
                }
            }
            disableAvailableRequiredFields(panelUniqueId.split('_')[1]);
        }
    }
}

//UAT-2216:Remove "End Date" from current employer (not previous employers) on Employment Verification.
//Method used to make the 'EmploymentEndDate' required or not.
//----------------------Start UAT-2216----------------------------------------------
function GetDecisionForEmploymentEndDate(e, args) {
    var instanceID = GetInstanceId(e._element.id);
    var isArray = e._element.id.split("_");
    var attributeGroupId = isArray[isArray.length - 2];
    var empEndDateIds = $jQuery("[id$=hdn_" + instanceID + "_" + attributeGroupId + "]").val().split("_");
    var empEndDateAttrMappingID = empEndDateIds[1];
    var empEndDateAttrID = empEndDateIds[0];
    var empEndDateField = $jQuery("[id$=dp_Date_" + instanceID + "_" + attributeGroupId + "_" + empEndDateAttrMappingID + "]");
    var empEndDateReqFieldValidator = $jQuery("[id$=rfv_Date_" + instanceID + "_" + empEndDateAttrMappingID + "]");

    var empEndDateFieldLabel = $jQuery("[id$=lblDate_" + attributeGroupId + "_" + instanceID + "_" + empEndDateAttrID + "]");
    var hdnCurrentEmployerDecisionField = $jQuery("[id$=hdnCurrentEmployerDecisionField]");
    if (e._value == 'True' && empEndDateReqFieldValidator.length > 0) {
        ValidatorEnable(empEndDateReqFieldValidator[0], false);
        empEndDateFieldLabel[0].parentElement.getElementsByClassName("reqd ")[0].style["display"] = "none";
        var hdnDecisionArray = hdnCurrentEmployerDecisionField.val().split(",");
        var mappedID = instanceID + "_" + attributeGroupId + "_" + empEndDateAttrMappingID + "_" + empEndDateAttrID;

        if (IsFieldAlreadyMapped(hdnDecisionArray, mappedID) == false) {
            hdnCurrentEmployerDecisionField.val(hdnCurrentEmployerDecisionField.val() + "," + instanceID + "_" + attributeGroupId + "_" + empEndDateAttrMappingID + "_" + empEndDateAttrID);
        }
        empEndDateField[0].control.set_selectedDate(new Date()); //UAT-3085
    }
    else if (empEndDateReqFieldValidator.length > 0) {
        ValidatorEnable(empEndDateReqFieldValidator[0], true);
        empEndDateReqFieldValidator.hide();
        empEndDateFieldLabel[0].parentElement.getElementsByClassName("reqd ")[0].style["display"] = "inline";
        var hdnDecisionArray = hdnCurrentEmployerDecisionField.val().split(",");
        var mappedID = instanceID + "_" + attributeGroupId + "_" + empEndDateAttrMappingID + "_" + empEndDateAttrID;
        SetCurrentEmployerDecisionField(hdnDecisionArray, mappedID, hdnCurrentEmployerDecisionField);
        empEndDateField[0].control.set_selectedDate(null); //UAT-3085
    }

}

//Method to take decision for 'Employment End Date' validators.
function SetDecisionForEmploymentEndDateField(needToCheckWithOtherDecisionFld) {
    var hdnCurrentEmployerDecisionField = $jQuery("[id$= hdnCurrentEmployerDecisionField]");
    if (hdnCurrentEmployerDecisionField[0] != undefined) {
        var mappedIDs = hdnCurrentEmployerDecisionField.val().split(",");
        if (mappedIDs != null && mappedIDs != "") {
            for (i = 0; i < mappedIDs.length; i++) {
                if (mappedIDs[i] != null && mappedIDs[i] != "") {
                    var mappedId = mappedIDs[i].split("_");
                    if (mappedId != null && mappedId != "") {
                        var instanceId = mappedId[0];
                        var attributeGroupId = mappedId[1];
                        var empEndDateAttrMappingID = mappedId[2];
                        var empEndDateAttrID = mappedId[3];
                        var empEndDateFieldLabel = $jQuery("[id$=lblDate_" + attributeGroupId + "_" + instanceId + "_" + empEndDateAttrID + "]");
                        var empEndDateReqFieldValidator = $jQuery("[id$=rfv_Date_" + instanceId + "_" + empEndDateAttrMappingID + "]");
                        if (IsDecisionFieldSelected(instanceId, needToCheckWithOtherDecisionFld) == false) {
                            ValidatorEnable(empEndDateReqFieldValidator[0], false);
                            empEndDateFieldLabel[0].parentElement.getElementsByClassName("reqd ")[0].style["display"] = "none";
                        }

                    }
                }
            }
        }
    }
}

//Method to check that any decision field selected to override changes of that.
function IsDecisionFieldSelected(instanceID, needToCheckWithOtherDecisionFld) {
    var decisionField = $jQuery("[id$=hdnIsedcisionField]");
    var isSelected = false;
    if (needToCheckWithOtherDecisionFld == true) {
        if (decisionField[0] != undefined) {
            var groupInstanceArray = decisionField[0].value.split(',');
            for (var k = 0; k < groupInstanceArray.length; k++) {
                if (groupInstanceArray[k] != "") {
                    var values = groupInstanceArray[k].split('_');
                    if (values[1] == instanceID && isSelected == false) {
                        isSelected = true;
                    }
                }
            }
        }
    }
    return isSelected;
}

//To Check id that current id already inserted in 'CurrentEmployerDecision' hidden Field data.
function IsFieldAlreadyMapped(hdnDecisionArray, mappedID) {
    var isAlreadyMapped = false;
    for (i = 0; i < hdnDecisionArray.length; i++) {
        if (hdnDecisionArray[i] == mappedID) {
            isAlreadyMapped = true;
            break;
        }
    }
    return isAlreadyMapped;
}

//Set new data in current Employer Decision hidden field.
function SetCurrentEmployerDecisionField(hdnDecisionArray, mappedID, hdnCurrentEmployerDecisionField) {
    for (i = 0; i < hdnDecisionArray.length; i++) {
        if (hdnDecisionArray[i] == mappedID) {
            hdnDecisionArray.splice(i, 1);
            break;
        }
    }
    var newMappedIds = "";
    for (i = 0; i < hdnDecisionArray.length; i++) {
        if (hdnDecisionArray[i] != null && hdnDecisionArray[i] != "") {
            newMappedIds = newMappedIds + "," + hdnDecisionArray[i];
        }
    }
    hdnCurrentEmployerDecisionField.val(newMappedIds);
}
//----------------------------END UAT-2216-----------------------------------


//---------------------------UAT 3521---------------------

function GetDataForCascadingDropDown(sender, args) {
      if (sender._value != "" && sender._value != 0) {

        CascadingCount = CascadingCount + 1;

        Page.showProgress('Processing...');
        var lstIds = sender._element.id.split('_');

        var attributeGroupMappingId = lstIds[lstIds.length - 1];
        var attributeGroupId = lstIds[lstIds.length - 2];
        var instanceId = lstIds[lstIds.length - 3];
        var attributeID = lstIds[lstIds.length - 4];
        var tenantID = lstIds[lstIds.length - 5];

        var dataurl = JSON.stringify({ 'tenantID': tenantID, 'searchId': sender._value, 'AtrributeGroupId': attributeGroupId, 'AttributeID': attributeID });
        var url = "Default.aspx/GetDataForCascadingDropDown";
        var IsAdminCreateOrder = $jQuery("[id$=hdnIsAdminCreateOrder]").val();
        if (IsAdminCreateOrder != undefined && IsAdminCreateOrder == "True") {
            url = "CustomFormPage.aspx/GetDataForCascadingDropDown";
        }

        $jQuery.ajax({
            type: "POST",
            url: url,
            data: dataurl,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {
                $jQuery.each(Result.d, function (key, val) {
                    var control = $jQuery('[id$=' + instanceId + '_' + attributeGroupId + '_' + key + ']');
                    var newDropDown = CheckForTheCorrectControl(control, instanceId);
                    if (newDropDown != null) {
                        var combo = $find(newDropDown.id);
                        bindCascadingDropDown(combo, val); 
                    }
                });
                CascadingCount = CascadingCount - 1;
                if (CascadingCount == 0) {
                    Page.hideProgress();
                }
            },
            error: function (Result) {

                alert("Error" + dataurl);
            }
        }); 
    } 
}

//Bind the drop down with the data.
function bindCascadingDropDown(combo, result) {
    combo.trackChanges();
    var items = combo.get_items();
    items.clear();
    combo.clearSelection();
    combo.enable();
    for (var i = 0; i < result.length; i++) {
        var comboItem = new Telerik.Web.UI.RadComboBoxItem();
        var rValue = result[i];
        if (rValue == null) {
            rValue = '';
        }
        comboItem.set_text(rValue);
        comboItem.set_value(rValue);
        items.add(comboItem);
        if (result[i] == null || result[i].ID == 0 || result.length == 1) {
            comboItem.select();
        }
    }

    if (result.length == 1) {
        combo.disable();
    }
    combo.commitChanges();
}

function CascadingDropdownClosing(sender, args) { 

    if (sender._text != "" && sender._value == "")
    {
        sender.set_text("");
    }
        sender.trackChanges();
        sender.commitChanges();
}

function hideExtraValidation()
{
    var lstDiv = $jQuery('div.ExtraValidation');
    for (var i = 0; i < lstDiv.length; i++) {
        lstDiv[i].style.display = "none";
    }
}

function showExtraValidation(validationMesssage)
{
    
    var lstofControls = validationMesssage.split(';');
    for (var j = 0; j < lstofControls.length; j++) {
        var controlMsg = lstofControls[j].split(':');
        var control = $jQuery('[id*=RequiredValidator_' + controlMsg[0] + '_]')
        if (control.length > 0)
        {
            control[0].innerHTML = controlMsg[1];
            control[0].parentElement.style.display = "Block";
        } 
    }
}
 