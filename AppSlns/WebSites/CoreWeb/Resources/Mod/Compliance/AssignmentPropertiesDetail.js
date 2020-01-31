var RuleSetTreeNodeType = {
    Package: "PAK",
    Category: "CAT",
    Item: "ITM",
    Attribute: "ATR"
};

$jQuery(document).ready(function () {
    //debugger;
    if ($jQuery("[id$=hdnRuleSetTreeTypeCode]").val() == RuleSetTreeNodeType.Attribute) {
        //Hide Validator for Attribute
        HideShowEditableByValidator();
    }
    else {
        OnChkActiveCheckedChanged();
        showHideUserDropDown();
    }
    parent.ResetTimer();
});

function HideShowEditableByValidator() {
    //debugger;
    //var isActive = $jQuery("[id$=chkIsActiveAttr]")[0].control._checked;
    var IsActiveYesClientId = $jQuery("[id$=hdnIsActiveYesClientIDAttr]").val();
    var isActive = $jQuery("[id$=" + IsActiveYesClientId + "]")[0].control._checked;
    if (isActive) {
        $jQuery("[id$=rfvEditableBy]")[0].enabled = true;
        $jQuery("[id$=dvEditableBy]").show();
        $jQuery("[id$=spanEditableBy]").show();
    }
    else {
        $jQuery("[id$=rfvEditableBy]")[0].enabled = false;
        $jQuery("[id$=dvEditableBy]").hide();
        $jQuery("[id$=spanEditableBy]").hide();
    }
}

function OnChkActiveCheckedChanged() {
    var IsActiveYesClientId = $jQuery("[id$=hdnIsActiveYesPAKCAT]").val();
    var isActive = $jQuery("[id$=" + IsActiveYesClientId + "]")[0].control._checked;
    if (isActive) {
        //UAT-906: If assignment properties are active at category and item level then no asterisk sign appears for mandatory fields. Also, admin is able to save the assignment properties with Null values. 
        $jQuery("[id$=rfvEffectiveDate]")[0].enabled = false;
        $jQuery("[id$=dvEffectiveDate]").hide();
        $jQuery("[id$=spanEffectiveDate]").hide();

        if ($jQuery("[id$=hdnRuleSetTreeTypeCode]").val() != RuleSetTreeNodeType.Package) {
            $jQuery("[id$=rfvEditableBy]")[0].enabled = true;
            $jQuery("[id$=dvEditableBy]").show();
            $jQuery("[id$=spanEditableBy]").show();
            if ($jQuery("[id$=hdnRuleSetTreeTypeCode]").val() == RuleSetTreeNodeType.Category) {
                if ($jQuery("[id$=rfvAllowException]").length > 0 && $jQuery("[id$=rfvAllowException]") != undefined) {
                    $jQuery("[id$=rfvAllowException]")[0].enabled = true;
                }
                if ($jQuery("[id$=divAllowException]").length > 0 && $jQuery("[id$=divAllowException]") != undefined) {
                    $jQuery("[id$=divAllowException]").show();
                }
                if ($jQuery("[id$=spnAllowException]").length > 0 && $jQuery("[id$=spnAllowException]") != undefined) {
                    $jQuery("[id$=spnAllowException]").show();
                }
            }
            //UAT-1137:Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
            if ($jQuery("[id$=hdnRuleSetTreeTypeCode]").val() == RuleSetTreeNodeType.Item) {
                $jQuery("[id$=rfvItemDataEntry]")[0].enabled = true;
                $jQuery("[id$=divItemDataEntryVldxMsg]").show();
                $jQuery("[id$=spnItemDataEntry]").show();
            }
        }
        $jQuery("[id$=rfvApprovalReqd]")[0].enabled = true;
        $jQuery("[id$=dvApprovalReqd]").show();
        $jQuery("[id$=spanApprovalReqd]").show();
    }
    else {
        $jQuery("[id$=rfvEffectiveDate]")[0].enabled = false;
        $jQuery("[id$=dvEffectiveDate]").hide();
        $jQuery("[id$=spanEffectiveDate]").hide();

        if ($jQuery("[id$=hdnRuleSetTreeTypeCode]").val() != RuleSetTreeNodeType.Package) {
            $jQuery("[id$=rfvEditableBy]")[0].enabled = false;
            $jQuery("[id$=dvEditableBy]").hide();
            $jQuery("[id$=spanEditableBy]").hide();
            //UAT-906: If assignment properties are active at category and item level then no asterisk sign appears for mandatory fields. Also, admin is able to save the assignment properties with Null values. 
            if ($jQuery("[id$=hdnRuleSetTreeTypeCode]").val() == RuleSetTreeNodeType.Category) {
                if ($jQuery("[id$=rfvAllowException]").length > 0 && $jQuery("[id$=rfvAllowException]") != undefined) {

                    $jQuery("[id$=rfvAllowException]")[0].enabled = false;
                }
                if ($jQuery("[id$=divAllowException]").length > 0 && $jQuery("[id$=divAllowException]") != undefined) {
                    $jQuery("[id$=divAllowException]").hide();
                }
                if ($jQuery("[id$=spnAllowException]") > 0 && $jQuery("[id$=spnAllowException]") != undefined) {
                    $jQuery("[id$=spnAllowException]").hide();
                }
            }
            //UAT-1137:Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
            if ($jQuery("[id$=hdnRuleSetTreeTypeCode]").val() == RuleSetTreeNodeType.Item) {
                $jQuery("[id$=rfvItemDataEntry]")[0].enabled = false;
                $jQuery("[id$=divItemDataEntryVldxMsg]").hide();
                $jQuery("[id$=spnItemDataEntry]").hide();
            }
        }

        $jQuery("[id$=rfvApprovalReqd]")[0].enabled = false;
        $jQuery("[id$=dvApprovalReqd]").hide();
        $jQuery("[id$=spanApprovalReqd]").hide();
    }
    OnRdoApprovalReqdClick();
}

function OnRdoApprovalReqdClick() {
    var approvalRequired = $jQuery("[id$=rdoApprovalReqd] input:checked").val();
    var IsActiveYesClientId = $jQuery("[id$=hdnIsActiveYesPAKCAT]").val();
    var isActive = $jQuery("[id$=" + IsActiveYesClientId + "]")[0].control._checked;

    if (approvalRequired == "True" && isActive == true) {
        $jQuery("[id$=rfvReviewedBy]")[0].enabled = true;
        $jQuery("[id$=dvReviewedBy]").show();
        $jQuery("[id$=spanReviewedBy]").show();
    }
    else {
        $jQuery("[id$=rfvReviewedBy]")[0].enabled = false;
        $jQuery("[id$=dvReviewedBy]").hide();
        $jQuery("[id$=spanReviewedBy]").hide();
    }
}

function OnClientItemChecked(sender, eventArgs) {
    showHideUserDropDown();
}

function OnClientSelectedIndexChanged(sender, eventArgs) {
    showHideUserDropDown();
}

function showHideUserDropDown() {
    var cmbReviewertype = $find($jQuery("[id$=cmbReviewedBy]")[0].id);
    if ($jQuery("[id$=cmbThirdPartyReviewer]").length > 0) {
        var dropDownReviewer = $find($jQuery("[id$=cmbThirdPartyReviewer]")[0].id);

        var checked = 0;
        var items = cmbReviewertype.get_items();
        for (var i = 0; i < cmbReviewertype.get_items().get_count() ; i++) {
            if (items._array[i].get_checked()) {
                checked++;
            }
        }
        if (checked == 1) {
            if (cmbReviewertype.get_checkedItems()[0].get_text() == "Admin" && dropDownReviewer._value != "") {
                $jQuery("[id$=dvThirdPartyUser]").show();
                $jQuery("[id$=rfvThirdPartyUser]")[0].enabled = true;
                $jQuery("[id$=dvThirdPartyUserVldx]").show();
            }
            else {
                $jQuery("[id$=dvThirdPartyUser]").hide();
                $jQuery("[id$=rfvThirdPartyUser]")[0].enabled = false;
                $jQuery("[id$=dvThirdPartyUserVldx]").hide();
            }
        }
        else {
            $jQuery("[id$=dvThirdPartyUser]").hide();
            $jQuery("[id$=rfvThirdPartyUser]")[0].enabled = false;
            $jQuery("[id$=dvThirdPartyUserVldx]").hide();
        }
    }
    else {
        $jQuery("[id$=dvThirdPartyUser]").hide();
        $jQuery("[id$=rfvThirdPartyUser]")[0].enabled = false;
        $jQuery("[id$=dvThirdPartyUserVldx]").hide();
    }
}

//--------------UAT-906 : If assignment properties are active at category and item level then no asterisk sign appears for mandatory fields.
//Also, admin is able to save the assignment properties with Null values.------------------------------------

// Function to validate ReviewedBy
function ValidateReviewedBy(sender, args) {
    ValidateComboBox("cmbReviewedBy", args);
}
// Function to validate EditableBy
function ValidateEditableBy(sender, args) {
    ValidateComboBox("cmbEditableBy", args);

}
// Function to validate ThirdPartyUser
function ValidateThirdPartyUser(sender, args) {
    ValidateComboBox("cmbThirdPartyUser", args);

}

//Function to validate ComboBox
function ValidateComboBox(comboBoxId, args) {
    var checkedItems = $jQuery("[id$=" + comboBoxId + "]")[0].control.get_checkedItems();
    if (checkedItems.length > 0) {
        args.IsValid = true;
        return false;
    }
    args.IsValid = false;
}

//function ResetTimer() {
//    debugger;
//    var hdntimeout = $jQuery('[id$=hdntimeout]');  //, $jQuery(parent.theForm));
//    if (hdntimeout != null) {
//        var timeout = hdntimeout.val();
//        parent.StartCountDown(timeout);
//    }
//}


