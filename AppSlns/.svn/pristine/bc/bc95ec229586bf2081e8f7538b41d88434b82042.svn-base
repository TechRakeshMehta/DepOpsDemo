function GetDataForDropDownForSupplemental(e) {
    if (e._value != "" && e._value != 0) {
        var lstIds = e._element.id.split('_');
        var type = lstIds[lstIds.length - 4];
        var instanceId = lstIds[lstIds.length - 3];
        var attributeGroupId = lstIds[lstIds.length - 2];
        var perviousSearchId = GetPreviousValueForSupplemntal(type, instanceId)
        if (perviousSearchId == "" && type == "State") {
            perviousSearchId = "UNITED STATES";
        }
        var nextType = GetNextTypeToBePopulated(type, instanceId);
        if (nextType != "") {
            var dataurl = "searchId: '" + e._value + "', previousSearchId: '" + perviousSearchId + "', type: '" + type + nextType + "' ";
            $jQuery.ajax({
                type: "POST",
                url: "Default.aspx/GetDataForDropDownForSupplement",
                data: "{ " + dataurl + " }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (Result) {
                    var control = $jQuery('.J' + nextType + '');
                    var newDropDown = CheckForTheCorrectControl(control, instanceId);
                    if (newDropDown != null) {
                        var combo = $find(newDropDown.id);
                        if (Result.d.length > 0) {
                            bindCombo(combo, Result.d, nextType, instanceId);
                        }
                    }
                },
                error: function (Result) {
                    alert("Error");
                }
            });
        }
    }
}

function GetPreviousValueForSupplemntal(type, instanceId) {
    var lst = ["Country", "State", "City", "ZipCode", "County"];
    var index = lst.indexOf(type);
    for (var i = index - 1; i = 0; i--) {
        var control = $jQuery('.J' + lst[i] + '');
        if (control.length > 0) {
            var dropDown = CheckForTheCorrectControl(control, instanceId);
            if (dropDown != null) {
                return dropDown.control._value;
            }
        }
    }
    return "";
}

function GetNextTypeToBePopulated(type, instanceId) {
    var lst = ["Country", "State", "City", "ZipCode", "County"];
    var index = lst.indexOf(type);
    for (var i = index + 1; i < lst.length; i++) {
        var control = $jQuery('.J' + lst[i] + '');
        if (control.length > 0) {
            return lst[i];
        }
    }
    return "";
}


function GetDecisionForTheFields(e) {
    var selectedValue = e._value;
    var instanceId = GetInstanceId(e._element.id);
    var groupId = GetAttributeId(e._element.id);
    var isEnabled = true;

    if (selectedValue.toLowerCase().trim() == "false" || selectedValue.toLowerCase().trim() == "no") {
        isEnabled = false;
    }
    DisableEnableAllRequiredFieldValidators(instanceId, isEnabled, groupId);
    SetDecisionForEmploymentEndDateField(false);
    //UAT-2866
    GrayOutAllField(e._element.id, groupId, instanceId, isEnabled);
}
//UAT-2866
function SetGrayOutProperties() {
    var decisionFieldValues = $jQuery("[id$=hdnDecisionFieldId]").val();
    if (decisionFieldValues != undefined && decisionFieldValues != null) {
        var values = decisionFieldValues.split(',');
        var TotalInstanceIds = $jQuery("[id$=hdnInstanceId]").val();

        for (var i = 1; i <= TotalInstanceIds; i++) {
            var InstanceId = i;
            var AttributeType = values[0];
            var AttributeGroupId = values[1];
            var AtrributeGroupMappingId = values[2];
            var GroupId = $jQuery("[id$=hdnGroupId]").val();
            var elementValue = $jQuery("[id$=dropDown_" + AttributeType + "_" + InstanceId + "_" + AttributeGroupId + "_" + AtrributeGroupMappingId + "]").val();

            if (elementValue != undefined || elementValue != null) {
                var elementId = $jQuery("[id$=dropDown_" + AttributeType + "_" + InstanceId + "_" + AttributeGroupId + "_" + AtrributeGroupMappingId + "]")[0].id;
                elementValue = elementValue.toLowerCase().trim();

                if (elementValue == "" || elementValue == "false" || elementValue == "no") {
                    GrayOutAllField(elementId, GroupId, InstanceId, false);
                    DisableEnableAllRequiredFieldValidators(InstanceId, false, GroupId);
                }
                else {
                    GrayOutAllField(elementId, GroupId, InstanceId, true);
                    DisableEnableAllRequiredFieldValidators(InstanceId, true, GroupId);
                }
            }
        }
    }
}

function GrayOutAllField(elementId, groupId, instanceId, isEnabled) {
    var divId = 'mainDiv_' + groupId + '_' + instanceId;
    $jQuery("[id$=" + divId + "]").find(".grayoutClass").each(function (i) {

        var controlId = $jQuery("[id$=" + divId + "]").find(".grayoutClass")[i].id;
        if (controlId == "") {
            controlId = $jQuery($jQuery("[id$=" + divId + "]").find(".grayoutClass")[i]).find("input")[0].id;
            if (isEnabled) {
                $jQuery("[id$=" + controlId + "]").attr('disabled', false);
            }
            else {
                $jQuery("[id$=" + controlId + "]").attr('disabled', 'disabled');
            }
        }
        else {
            var controlDisplayVal = $jQuery("[id$=" + divId + "]").find(".grayoutClass")[i].style["display"];
            var isDatePickerControl = false;

            //Check Is control is datepicker
            if (controlId.toLowerCase().indexOf("date") >= 0) {
                isDatePickerControl = true;
                controlId = controlId.replace("_wrapper", "");
            }

            if (controlDisplayVal != "none") {
                if (controlId != elementId) {
                    var Control = $find(controlId);
                    if (Control != undefined && Control != null) {
                        if (isEnabled) {
                            if (!isDatePickerControl) {
                                //NoDatePicker Controls
                                Control.enable();
                            }
                            else {
                                //DatePicker Controls
                                Control.set_enabled(true);
                            }
                        }
                        else {
                            if (!isDatePickerControl) {
                                //NoDatePicker Controls
                                Control.disable();
                            }
                            else {
                                //DatePicker Controls
                                Control.set_enabled(false);
                            }
                        }
                    }
                }
            }
        }
    });
}

function DisableEnableAllRequiredFieldValidators(instanceId, IsEnabled, groupId) {
    var requiredFieldValidators = $jQuery("[id*=rfv]");
    if (requiredFieldValidators[0] != undefined) {
        SetgroupIdInstanceId(instanceId, IsEnabled, groupId);//, 
        for (var i = 0; i < requiredFieldValidators.length; i++) {
            //CheckIfSpecialTypeValidated(requiredFieldValidators[i].id);
            if ((GetInstanceIdForRequiredField(requiredFieldValidators[i].id) == instanceId) && (GetGroupIDValidators(requiredFieldValidators[i].validationGroup) == groupId) && CheckIfSpecialTypeValidated(requiredFieldValidators[i].id))
                ValidatorEnable(requiredFieldValidators[i], IsEnabled);
        }
        ShowHideRequired(groupId, instanceId, IsEnabled);
    }
    //UAT-2447
    ShowHidePhoneOnLoad(instanceId, IsEnabled);
}

function GetInstanceIdForRequiredField(controlId) {
    var idArray = controlId.split('_');
    var length = idArray.length;
    return idArray[length - 2];
}

function SetgroupIdInstanceId(instanceID, IsEnabled, groupId) {
    var decisionField = $jQuery("[id$=hdnIsedcisionField]");
    if (decisionField.length > 0) {
        var value = groupId + '_' + instanceID;
        var values = decisionField.val().split(',');
        if (($jQuery.inArray(value, values) == -1)) {
            //return true;
            if (IsEnabled == false)
                decisionField[0].value += value + ',';
        }
        else if (IsEnabled == true) {
            decisionField[0].value = "";
            for (var j = 0; j < values.length; j++) {
                if (values[j] != value && values[j] != "") {

                    decisionField[0].value += values[j] + ',';
                }
            }
        }
    }
}

function GetGroupIDValidators(validationgroup) {
    var groupId = validationgroup.split('submitForm');
    return groupId[1];
}

function SetTheDataForIsDecisionFields() {
    var decisionField = $jQuery("[id$=hdnIsedcisionField]");
    if (decisionField[0] != undefined) {
        var groupInstanceArray = decisionField[0].value.split(',');
        for (var k = 0; k < groupInstanceArray.length; k++) {
            if (groupInstanceArray[k] != "") {
                var values = groupInstanceArray[k].split('_');
                DisableEnableAllRequiredFieldValidators(values[1], false, values[0]);
            }
        }
    }
}

function ShowHideRequired(groupId, instanceID, IsEnabled) {
    var requiredSpan = $jQuery("." + groupId + "_" + instanceID + "");
    if (requiredSpan[0] != undefined) {
        for (var i = 0; i < requiredSpan.length; i++) {
            requiredSpan[i].innerHTML = "";
            if (IsEnabled == true) {
                requiredSpan[i].innerHTML = "*";
            }
        }
    }
}

function CheckIfSpecialTypeValidated(validationId) {
    var ids = validationId.split('_');
    var attributeType = ids[ids.length - 3];
    var mappingId = ids[ids.length - 1];
    if (attributeType.indexOf("txt") == 0) {
        var instanceHdnTextBox = $jQuery('[id$=hdnHiddenTextBoxIds]');
        var hiddenTextBoxes = instanceHdnTextBox.val().split(':');
        for (var i = 0; i < hiddenTextBoxes.length; i++) {
            if (hiddenTextBoxes[i].split('_')[2] == mappingId) {
                return false;
            }
        }

    }
    if (attributeType.indexOf("drop") == 0) {
        var instanceHdnDropDown = $jQuery('[id$=hdnHiddenDropDownIds]');
        var hiddendropDown = instanceHdnDropDown.val().split(':');
        for (var i = 0; i < hiddendropDown.length; i++) {
            if (hiddendropDown[i].split('_')[2] == mappingId) {
                return false;
            }
        }
    }
    return true;

}
