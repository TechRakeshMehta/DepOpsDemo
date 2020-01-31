

$jQuery(".rtlCollapse").hide();

function ManageChild(obj) {
    $jQuery("." + $jQuery("#" + obj.id).parent("span").attr("fieldIndex") + " input:checkbox").each(function () {
        if (this.id.length > 0) {
            $jQuery("input[id$=" + this.id + "]")[0].checked = obj.checked;
            checkPermissions(this.id, obj.checked)
            ManageChild(this);
        }
    });

}

// This function is to check uncheck the access check box
function checkPermissions(obj, isChecked) {
    var i = 1;
    $jQuery("input[id$=" + this.id + "]").attr("checked", obj.checked);

    var actionFeatureID = obj.substr(0, obj.indexOf("chkFeature")) + "btnFeatureActionList";
    var uIControlID = obj.substr(0, obj.indexOf("chkFeature")) + "hdnUIControlID";
    var actionFeature = $jQuery("[id$=" + actionFeatureID + "]");
    var uIControlValue = $jQuery("input[id$=" + uIControlID + "]").val();
    if (actionFeature.length > 0) {
        if (uIControlValue != '') {
            if (isChecked) {
                actionFeature[0].style.display = "block";
            }
            else {
                actionFeature[0].style.display = "none";
            }
        }
    }
    while (i != 5) {
        var id = obj.substr(0, obj.indexOf("chkFeature")) + "chkPermission" + i;
        if ($jQuery("input[id$=" + id + "]").length > 0) {
            $jQuery("input[id$=" + id + "]")[0].checked = isChecked;
        }
        i++;
    }
}


function ManageParent(obj) {
    var checked = false;
    if (obj == undefined)
        return;
    if ($jQuery("#" + obj.id).parent("span").attr("fieldIndex").length == 0)
        return;
    var isSibiling = false;
    checkPermissions(obj.id, obj.checked);
    $jQuery('[fieldIndex="' + $jQuery("input[id$=" + obj.id + "]").parent("span").attr("parent") + '"]').children("input").each(function () {
        if (this.id.length > 0) {
            //here check all sibling is checked or unchecked
            var parent = $jQuery("#" + obj.id).parent("span").attr("parent");

            if (parent != undefined) {
                if (parent.length > 0) {
                    $jQuery('[parent="' + parent + '"]').children("input").each(function () {
                        if (this.checked) {
                            if (this.id != obj.id) {
                                isSibiling = true;
                                return;
                            }
                        }
                        if (isSibiling == 0 && !obj.checked) {
                            //$jQuery("input[id$=" + this.id + "]").attr("checked", obj.checked);
                            $jQuery("input[id$=" + this.id + "]")[0].checked = obj.checked;
                        }
                    }
            );
                }
            }
            // end sibling check
            if (!isSibiling && obj.checked) {
                //$jQuery("input[id$=" + this.id + "]").attr("checked", obj.checked);
                $jQuery("input[id$=" + this.id + "]")[0].checked = obj.checked;
            }
            if (!isSibiling && !obj.checked) {
                // $jQuery("input[id$=" + this.id + "]").attr("checked", obj.checked);
                $jQuery("input[id$=" + this.id + "]")[0].checked = obj.checked;

            }
            ManageParent(this);
        }
    }
    );
}


/// Code for location info.
var radcombo;
function MsgSuccess(response, txtState, cmbCity, hdnState, hdnCity, txtZip) {
    radcombo = $jQuery.find('#' + cmbCity);
    //combo[0].control
    var flag = true;
    var cities = response.d;
    var statename;
    var comboItems = radcombo[0].control.get_items();
    for (var count = 0; count < comboItems.get_count() ; count++) {
        var item = comboItems.getItem(count);
        if (item) {
            radcombo[0].control.get_items().remove(item);
        }
    }
    radcombo[0].control.clearItems();
    radcombo[0].control.clearCache();
    radcombo[0].control.commitChanges();

    $jQuery.each(cities, function (index, city) {

        flag = false;

        //this code would be for rad combo.
        var comboItem = new Telerik.Web.UI.RadComboBoxItem();
        comboItem.set_text(city.CityName);
        comboItem.set_value(city.CityID);
        radcombo[0].control.trackChanges();
        radcombo[0].control.get_items().add(comboItem);
        comboItem.select();
        radcombo[0].control.commitChanges();

        //end rad combo

        statename = city.StateName;

        $jQuery('#' + hdnState).val(city.StateID);
        $jQuery('#' + hdnCity).val(city.ZipID);
    });
    if (flag) {
        var comboItem = new Telerik.Web.UI.RadComboBoxItem();
        if ($jQuery('#' + txtZip)[0].control.get_textBoxValue().trim() == '') {
            comboItem.set_text("");
        }
        else {
            comboItem.set_text("NONE");
        }
        comboItem.set_value("");
        radcombo[0].control.trackChanges();
        radcombo[0].control.get_items().add(comboItem);
        comboItem.select();
        radcombo[0].control.commitChanges();
        statename = '';
    }

    $jQuery('#' + txtState)[0].control.set_textBoxValue(statename);
    $jQuery('#' + txtState)[0].control.set_enabled(false);
}

function getCities(txtZip, txtState, cmbCity, hdnState, hdnCity) {

    $jQuery.ajax({
        type: "POST",
        url: '../CommonControls/Default.aspx/GetCities',
        data: "{'Zipcode': '" + $jQuery('#' + txtZip).val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            MsgSuccess(response, txtState, cmbCity, hdnState, hdnCity, txtZip);
        },
        error: function (msg) {
            alert("Failed to load names");
        }
    });
}

var winopen = false;
function openPopUp(sender) {

  
    var btnID = sender.get_id();
    var containerID = btnID.substr(0, btnID.indexOf("btnFeatureActionList"));
    var hdnProductFeatureID = $jQuery("[id$=" + containerID + "hdnProductFeatureID]").val();
    var SysyXBlockFeatureID = $jQuery("[id$=" + containerID + "hdnSysyXBlockFeatureID]").val();
    var defaultAccess = $jQuery("#" + sender.get_id()).parent().parent().find("input[type=radio]:checked").val();

    var composeScreenWindowName = "Manage Feature Action";
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var url = $page.url.create("~/IntsofSecurityModel/UserControl/FeatureActionList.aspx?ProductFeatureID=" + hdnProductFeatureID + "&SysXBlockFeatureID=" + SysyXBlockFeatureID + "&defaultAttributeAccess=" + defaultAccess);
    var win = $window.createPopup(url, { size: "520,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
    winopen = true;
    return false;
}

function OnClientClose(oWnd, args) {
    oWnd.remove_close(OnClientClose);
    if (winopen) {
        winopen = false;
    }
}