//----------------------------------------------------------
// Copyright (C) Copyright Intersoft Data Labs Inc. All rights reserved.
//----------------------------------------------------------
//ScriptName: LocationContrl
//UsedBy: LocationContrl

if (typeof (IWEBLIB) != "undefined" && IWEBLIB) {
    $page.add_pageLoaded(function () {
        $jQuery(".RadComboBox").each(function () {

            var ComboBox = $find(this.id);
            ComboBox.add_onClientBlur(ClearDropDownIfNoValueSelected);
        });
    });
}

function ClearDropDownIfNoValueSelected(sender, e) {

    if (sender._value == '' && !sender._checkBoxes) {
        sender.trackChanges();
        sender.clearSelection();
        sender.set_text('');
        sender.commitChanges();
    }
}

/// Code for location info.

var radcombo;




function MsgSuccess(response, txtState, cmbCity, hdnState, hdnCity, txtZip, txtCountyID, hdnCounty) {
    radcombo = $find(cmbCity);
    var flag = true;
    var cities = response.d;
    var Statename;
    var countyName;
    var countyID;
    if (radcombo == null)
        return;
    var comboItems = radcombo.get_items();
    radcombo.trackChanges();
    for (var i = 0; i < comboItems.get_count() ; i++) {
        var item = comboItems.getItem(i);
        if (item) {
            radcombo.get_items().remove(item);
        }
    }
    radcombo.clearItems();
    radcombo.clearCache();
    radcombo.commitChanges();
    if (response.d.length > 0) {
        if (response.d[0].ZipID != 0) {
            $jQuery.each(cities, function (index, city) {

                //this code would be for rad combo.
                var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                comboItem.set_text(city.CityName);
                comboItem.set_value(city.CityID)
                radcombo.trackChanges();
                radcombo.get_items().add(comboItem);
                if (flag) {
                    comboItem.select();
                }
                radcombo.commitChanges();
                flag = false;
                //end rad combo
                Statename = city.StateName;
                countyName = city.CountyName;
                $get(hdnState).value = city.StateID;
                $get(hdnCity).value = city.ZipID;
            });
        }
    }
    if (flag) {
        var comboItem = new Telerik.Web.UI.RadComboBoxItem();
        if ($find(txtZip).get_textBoxValue().trim() == '') {
            comboItem.set_text("");
        }
        else {
            comboItem.set_text("NONE");
        }
        comboItem.set_value("")
        radcombo.trackChanges();
        radcombo.get_items().add(comboItem);
        comboItem.select();
        radcombo.commitChanges();
        comboItem.set_text("NONE");
        radcombo.get_items().add(comboItem);
        comboItem.select();
        $find(txtState).set_textBoxValue('');

        if ($get(hdnCounty).value != '') {
            $find(txtCountyID).set_textBoxValue(countyName);
        }

        Statename = '';
    }


    if ($get(hdnCounty).value != '') {
        $find(txtCountyID).set_textBoxValue(countyName);
    }
    if (radcombo.get_items().get_count() > 0) {
        cmbCity_ClientSelectedIndexChanging(radcombo, null);
    }
    //    $jQuery('#' + txtState)[0].control.set_textBoxValue(Statename);
}


function getCities(txtZip, txtState, cmbCity, hdnState, hdnCity, path, hdnZipCode, txtCountyID, hdnCounty) {
    var editZipCode = $get(hdnZipCode).value;
    var userZipCode = $get(txtZip).value;

    if (userZipCode == '') {
        var radcomboCity = $find(cmbCity);
        var comboItems = radcomboCity.get_items();
        for (var i = 0; i < comboItems.get_count() ; i++) {
            var item = comboItems.getItem(i);
            if (item) {
                radcomboCity.get_items().remove(item);
            }
        }
        radcomboCity.clearItems();
        radcomboCity.clearCache();
        radcomboCity.commitChanges();
        $get(hdnCity).value = '';
        $get(hdnZipCode).value = '';
        radcomboCity.set_emptyMessage("--SELECT--");
        radcomboCity.trackChanges();
        $find(txtState).set_textBoxValue('');

        if ($get(hdnCounty).value != '') {
            $find(txtCountyID).set_textBoxValue('');
        }
    }
    if (editZipCode != userZipCode && userZipCode != '') {
        $get(hdnZipCode).value = userZipCode;
        $jQuery.ajax({
            type: "POST",
            url: path + '/CommonControls/Default.aspx/GetCities',
            data: "{'zipcode': '" + $get(txtZip).value + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                MsgSuccess(response, txtState, cmbCity, hdnState, hdnCity, txtZip, txtCountyID, hdnCounty);
            },
            error: function (msg) {
                if (msg.status == 200) {
                    MsgSuccess(JSON.parse(msg.responseText), txtState, cmbCity, hdnState, hdnCity, txtZip, txtCountyID, hdnCounty);
                }
                else {
                    alert("Failed to load names");
                }
            }
        });
    }
}

function numbersonly(myfield, e, dec) {
    var key;
    var returnval = false;
    var keychar;
    if (window.event)
        key = window.event.keyCode;
    else if (e)
        key = e.which;
    else
        returnval = true;
    keychar = String.fromCharCode(key);
    if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27))
        returnval = true;
    else if ((('0123456789').indexOf(keychar) > -1))
        returnval = true;
    else if (dec && (keychar == '.')) {
        myfield.form.elements[dec].focus();
        returnval = false;
    }
    else
        returnval = false;

    return returnval;
}


function cmbCity_ClientSelectedIndexChanging(s, e) {
    var txtZipCode = s.get_attributes()._data.txtZip;
    var hdnZipID1 = s.get_attributes()._data.hdnZipID;
    var Path1 = s.get_attributes()._data.Path;
    var txtStateID = s.get_attributes()._data.txtState;
    var hdnCounty = s.get_attributes()._data.county;
    var countytxtBoxID = s.get_attributes()._data.countyTxt;
    var cityId = s._value;
    var zipCode = $find(txtZipCode).get_value();

    if (cityId == '0') {
        $find(txtStateID).set_textBoxValue('');
    }
    else {
        var zipCode = $get(txtZipCode).value;
        if (cityId != '' && zipCode != '') {
            var cityZipcode = cityId + ',' + zipCode;
            $jQuery.ajax({
                type: "POST",
                url: Path1 + '/CommonControls/Default.aspx/GetZipCodeID',
                data: "{'zipcode': '" + cityZipcode + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    MsgSetzipCodeID(response, hdnZipID1, txtStateID, hdnCounty, countytxtBoxID);
                },
                error: function (msg) {
                    if (msg.status == 200) {
                        MsgSuccess(JSON.parse(msg.responseText), txtState, cmbCity, hdnState, hdnCity, txtZip);
                    }
                    else {
                        alert("Failed to load names");
                    }
                }
            }
            );
        }
    }
}


function MsgSetzipCodeID(response, hdnZipID1, txtStateID, hdnCounty, countytxtBoxID) {
    var result = response.d;
    if (result.indexOf(",") != -1) {
        $find(txtStateID).set_textBoxValue(result.split(',')[0]);
        $get(hdnZipID1).value = result.split(',')[1];

        if ($get(hdnCounty).value != '') {
            $find(countytxtBoxID).set_textBoxValue(result.split(',')[2]);
        }
    }
}

function resetControl(txtZip, txtState, cmbCity, hdnState, hdnCity, path, hdnZipCode, txtCountyID, hdnCounty) {
    var radcomboCity = $jQuery.find('#' + cmbCity);
    if ($jQuery('#' + txtZip).length == 0)
        return;
    var zipcode = $jQuery('#' + txtZip)[0].control.get_value();
    if (zipcode != '')
        return;
    $jQuery('#' + txtZip)[0].control.set_textBoxValue('');
    $jQuery('#' + txtZip).val('');
    radcomboCity[0].control.trackChanges();
    var comboItems = radcomboCity[0].control.get_items();
    for (var i = 0; i < comboItems.get_count() ; i++) {
        var item = comboItems.getItem(i);
        if (item) {
            radcomboCity[0].control.get_items().remove(item);
        }
    }
    radcomboCity[0].control.clearItems();
    radcomboCity[0].control.clearCache();
    radcomboCity[0].control.commitChanges();
    radcomboCity[0].control.set_text('');
    radcomboCity[0].control.set_emptyMessage("--SELECT--");
    radcomboCity[0].control.commitChanges();
    $jQuery('#' + txtState)[0].control.set_textBoxValue('');
}

function test() {
    $alert("Test");
}

////Methods to bind combo box for Reverse State Lookup Address. 

var controlIdExtension = '';
function getFreshData(value, type, controlId, cityId, stateId, useNameOnly) {
    if (this.Page != undefined && this.Page.showProgress != undefined)
        Page.showProgress('Please wait...');
    $jQuery.ajax({
        type: "POST",
        url: '/CommonControls/Default.aspx/GetDataForAddressDropdowns',
        data: "{'searchId': '" + value + "', type: '" + type + "', cityId: '" + cityId + "', stateId: '" + stateId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (Result) {
            if (Result != undefined && Result.d.length > 0) {
                BindListToCombo(controlId, Result.d, useNameOnly);
                SetReverseLookupAddressControls(type, true);
            }
            else {
                SetReverseLookupAddressControls(type, false);
            }
            if (type == 'State') {
                if (value == 233)
                    SetLabelsForCountry(false);
                else
                    SetLabelsForCountry(true);
            }
        },
        error: function (Result) {
            alert("Error");
        }
    });
    if (this.Page != undefined && this.Page.hideProgress != undefined)
        Page.hideProgress();
}

function PopulateBindStateDropdown(e) {
    var lstIds = e._element.id.split('_');
    if (lstIds[lstIds.length - 1] != "cmbCountry") {
        controlIdExtension = "_" + lstIds[lstIds.length - 1];
    }
    getFreshData(e._value, 'State', 'cmbRSL_State', null, null, false);
}

function PopulateBindCityDropdown(e) {
    var lstIds = e._element.id.split('_');
    if (lstIds[lstIds.length - 1] != "State") {
        controlIdExtension = "_" + lstIds[lstIds.length - 1];
    }
    GetSetHiddenFieldValue('hdnRSLStateID', e._value);
    isStateSelectionChanged = true;
    getFreshData(e._value, 'City', 'cmbRSL_City', null, null, false);
}

function PopulateBindZipcodeCountyDropdown(e) {
    var lstIds = e._element.id.split('_');
    if (lstIds[lstIds.length - 1] != "City") {
        controlIdExtension = "_" + lstIds[lstIds.length - 1];
    }
    GetSetHiddenFieldValue('hdnRSLCityID', e._value);
    var stateId = GetSetHiddenFieldValue('hdnRSLStateID', null);
    isCitySelectionChanged = true
    getFreshData(e._value, 'ZipCode', 'cmbRSL_ZipId', null, stateId, true);
    getFreshData(e._value, 'County', 'cmbRSL_County', null, stateId, false);
}

function PopulateBindCountyDropdown(e) {
    if (e.get_items().get_count() > 1) {
        var lstIds = e._element.id.split('_');
        if (lstIds[lstIds.length - 1] != "ZipId") {
            controlIdExtension = "_" + lstIds[lstIds.length - 1];
        }
        isZipSelectionChanged = true;
        var cityId = GetSetHiddenFieldValue('hdnRSLCityID', null);
        getFreshData(e._value, 'County', 'cmbRSL_County', cityId, null, false);
    }
}

function PopulateBindZipCodeDropdown(e) {
    if (e.get_items().get_count() > 1) {
        var lstIds = e._element.id.split('_');
        if (lstIds[lstIds.length - 1] != "County") {
            controlIdExtension = "_" + lstIds[lstIds.length - 1];
        }
        var cityId = GetSetHiddenFieldValue('hdnRSLCityID', null);
        isCountySelectionChanged = true;
        getFreshData(e._value, 'ZipCode', 'cmbRSL_ZipId', cityId, null, true);
    }
}

function BindListToCombo(controlId, result, useNameOnly) {
    var control = $jQuery("[id$=" + controlId + controlIdExtension + "]")[0];
    if (control != undefined || control != null) {
        var combo = $find(control.id);
        if (combo != undefined || combo != null) {
            combo.trackChanges();
            var clearSelection = true;
            var selectedItem = combo.get_selectedItem();
            var items = combo.get_items();
            items.clear();
            for (var i = 0; i < result.length; i++) {
                var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                comboItem.set_text(result[i].Name);
                if (useNameOnly) {
                    comboItem.set_value(result[i].Name);
                }
                else {
                    comboItem.set_value(result[i].ID);
                }
                items.add(comboItem);

                if (selectedItem != null && comboItem.get_text() == selectedItem.get_text() && comboItem.get_value() == selectedItem.get_value()) {
                    //comboItem.select();
                    clearSelection = false;
                }
            }
            if (result.length == 1) {
                comboItem.select();
                clearSelection = false;
            }
            if (clearSelection == true)
                combo.clearSelection();
            combo.commitChanges();
        }
    }
}


function ClearListOfCombo(controlId) {
    var control = $jQuery("[id$=" + controlId + controlIdExtension + "]")[0];
    if (control != undefined || control != null) {
        var combo = $find(control.id);
        if (combo != undefined || combo != null) {
            combo.trackChanges();
            var items = combo.get_items();
            items.clear();
            combo.clearSelection();
            combo.commitChanges();
        }
    }
}

function SetReverseLookupAddressControls(type, showDropdown) {
    switch (type) {
        case 'State':
            if (!showDropdown)
                ClearListOfCombo('cmbRSL_State');
            ClearListOfCombo('cmbRSL_City');
            ClearListOfCombo('cmbRSL_ZipId');
            ClearListOfCombo('cmbRSL_County');
            SetObjectsAndValidations('cmbRSL_State', 'rfvCmbRSL_State', 'txtRSL_State', 'dvTxtState', 'rfvTxtRSL_State', showDropdown);
            SetObjectsAndValidations('cmbRSL_City', 'rfvCmbRSL_City', 'txtRSL_City', 'dvTxtCity', 'rfvTxtRSL_City', showDropdown);
            SetObjectsAndValidations('cmbRSL_ZipId', 'rfvCmbRSL_ZipId', 'txtRSL_ZipId', 'dvTxtZipId', 'rfvTxtRSL_ZipId', showDropdown);
            SetCountyDropdownValidations(showDropdown);
            break;
        case 'City':
            if (!showDropdown)
                ClearListOfCombo('cmbRSL_City');
            ClearListOfCombo('cmbRSL_ZipId');
            ClearListOfCombo('cmbRSL_County');
            SetObjectsAndValidations('cmbRSL_City', 'rfvCmbRSL_City', 'txtRSL_City', 'dvTxtCity', 'rfvTxtRSL_City', showDropdown);
            SetObjectsAndValidations('cmbRSL_ZipId', 'rfvCmbRSL_ZipId', 'txtRSL_ZipId', 'dvTxtZipId', 'rfvTxtRSL_ZipId', showDropdown);
            SetCountyDropdownValidations(showDropdown);
            break;
        case 'ZipCode':
            if (!showDropdown)
                ClearListOfCombo('cmbRSL_ZipId');
            SetObjectsAndValidations('cmbRSL_ZipId', 'rfvCmbRSL_ZipId', 'txtRSL_ZipId', 'dvTxtZipId', 'rfvTxtRSL_ZipId', showDropdown);
            break;
        case 'County':
            if (!showDropdown)
                ClearListOfCombo('cmbRSL_County');
            SetCountyDropdownValidations(showDropdown);
            break;
    }
}


function SetCountyDropdownValidations(ShowDropDown) {
    var comboCounty = $jQuery("[id$=cmbRSL_County" + controlIdExtension + "]")[0];
    var reqFieldCounty = $jQuery("[id$=rfvCmbRSL_County" + controlIdExtension + "]");
    //var divCounty = $jQuery("[id$=dvRSLCounty" + controlIdExtension)[0];
    var divLblCounty = $jQuery("[id$=dvLblRSLCounty" + controlIdExtension + "]")[0];
    var dvCntRSLCounty = $jQuery("[id$=dvCntRSLCounty" + controlIdExtension + "]")[0];

    if (comboCounty != undefined && reqFieldCounty != undefined && divLblCounty != undefined && dvCntRSLCounty != undefined) {
        if (!ShowDropDown || ShowDropDown == "False") {
            comboCounty.style.display = "none";
            divLblCounty.style.display = "none";
            dvCntRSLCounty.style.display = "none";
            ValidatorEnable(reqFieldCounty[0], false);
        }
        else if (ShowDropDown || ShowDropDown == "True") {
            comboCounty.style.display = "block";
            divLblCounty.style.display = "block";
            dvCntRSLCounty.style.display = "block";
            ValidatorEnable(reqFieldCounty[0], true);
        }
        reqFieldCounty.hide();
    }
}

function SetLabelsForCountry(isNonUSAddress) {
    var labelState = $jQuery("[id$=lblRSL_State" + controlIdExtension + "]")[0];
    var labelZipcode = $jQuery("[id$=lblRSL_Zipcode" + controlIdExtension + "]")[0];
    if (labelState != undefined && labelZipcode != undefined) {
        if (isNonUSAddress) {
            labelState.textContent = "State/Province";
            labelZipcode.textContent = "Postal Code";

        }
        else {
            labelState.textContent = "State";
            labelZipcode.textContent = "Zip Code";
        }
    }
}


function SetObjectsAndValidations(comboName, comboReqValidation, textName, divTextBox, textValidation, showDropdown) {
    var control = $jQuery("[id$=" + comboName + controlIdExtension + "]")[0];
    if (control != undefined || control != null) {
        var textbox = $jQuery("[id$=" + textName + controlIdExtension + "]")[0];
        var comboValidator = $jQuery("[id$=" + comboReqValidation + controlIdExtension + "]");
        var textboxValidator = $jQuery("[id$=" + textValidation + controlIdExtension + "]");
        var divText = $jQuery("[id$=" + divTextBox + controlIdExtension + "]")[0];
        if (textbox != undefined && comboValidator != undefined && textboxValidator != undefined && divText != undefined) {
            if (showDropdown == true) {
                control.style.display = "block";
                divText.style.display = "none";
                textbox.value = '';
                ValidatorEnable(comboValidator[0], true);
                ValidatorEnable(textboxValidator[0], false);
            }
            else {
                control.style.display = "none";
                divText.style.display = "block";
                ValidatorEnable(comboValidator[0], false);
                ValidatorEnable(textboxValidator[0], true);
            }
            textboxValidator.hide();
            comboValidator.hide();
        }
    }
}

function GetSetHiddenFieldValue(controlName, controlValue) {
    var hdnRSLObjectID = $jQuery("[id$=" + controlName + controlIdExtension + "]");
    if (hdnRSLObjectID != undefined) {
        if (controlValue == null) {
            return hdnRSLObjectID[0].value;
        }
        else {
            hdnRSLObjectID[0].value = controlValue;
        }
    }
}


function onLocationBlur(sender, args) {
    //if (sender.get_highlightedItem() != null && (sender.get_originalText() != null && sender.get_originalText() != sender.get_highlightedItem().get_text()))
    //    sender.get_highlightedItem().select();
    //else
    //    sender.set_text("");
    if (sender._value == '' && !sender._checkBoxes) {
        sender.trackChanges();
        sender.clearSelection();
        sender.set_text('');
        sender.commitChanges();
    }
}

//UAT-1980
function openLocationCmbBoxOnTab(sender, e) {
    if (!sender.get_dropDownVisible())
        sender.showDropDown();
}
