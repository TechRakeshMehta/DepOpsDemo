$page.add_pageLoad(function () {
    $jQuery("[id$=lblfirstName_0]").attr('tabindex', '0');
    $jQuery("[id$=lblMiddleName_0]").attr('tabindex', '0');
    $jQuery("[id$=lblLastName_0]").attr('tabindex', '0');
    $jQuery("[id$=chkShowHideAlias]").attr('tabindex', '0');
});

function OnAliasDelete_ClientClicked() {
    var _aliasDelConfirmMsg = "<%=Resources.Language.CONFMALIASDEL %>";
    return confirm(_aliasDelConfirmMsg);
}

function ValidateVerifyAlias(ID) {
    if ($jQuery("[id$=chkMiddleNameRequired]")[1].checked) {
        ValidatorEnable($jQuery("[id$=rfvNewMiddleName]")[0], false);
    }
}

function ValidateVerifyAlias() {
    var hdnIFYOUDONTHAVEMIDDLENAME = $jQuery("[id$=hdnIFYOUDONTHAVEMIDDLENAME]")[0].value;
    var middlename = $find($jQuery("[id$=txtNewMiddleName]")[0].id);
    middlename._element.setAttribute("Placeholder", hdnIFYOUDONTHAVEMIDDLENAME);
    if ($jQuery("[id$=chkMiddleNameRequired]")[1].checked) {
        ValidatorEnable($jQuery("[id$=rfvNewMiddleName]")[0], false);
    }
}

function AliasMiddleNameEnableDisable(ID) {
    var middlename = $find($jQuery("[id$=txtNewMiddleName]")[0].id);
    var hdnIFYOUDONTHAVEMIDDLENAME = $jQuery("[id$=hdnIFYOUDONTHAVEMIDDLENAME]")[0].value;
    if (ID.checked) {
        //UAT_2169:Send Middle Name and Email address to clearstar in Complio
        //  var rfvMiddle = $jQuery("[id$=revNewMiddleName]")[0].css('display', 'block');        
        var noMiddleNameText = $jQuery("[id$=hdnNoMiddleNameText]")[0].value;
        $find($jQuery("[id$=txtNewMiddleName]")[0].id).set_value();

        middlename._element.setAttribute("Placeholder", "");
        middlename._element.setAttribute("title", "");

        //ValidatorEnable($jQuery("[id$=revNewMiddleName]")[0], false);  // commented this line
        $find($jQuery("[id$=txtNewMiddleName]")[0].id).set_value(noMiddleNameText);
        $find($jQuery("[id$=txtNewMiddleName]")[0].id).disable();
        ValidatorEnable($jQuery("[id$=rfvNewMiddleName]")[0], false);
    }
    else {
        $find($jQuery("[id$=txtNewMiddleName]")[0].id).set_value('');

        middlename._element.setAttribute("Placeholder", hdnIFYOUDONTHAVEMIDDLENAME);
        middlename._element.setAttribute("title", hdnIFYOUDONTHAVEMIDDLENAME);

        $find($jQuery("[id$=txtNewMiddleName]")[0].id).enable();
        ValidatorEnable($jQuery("[id$=rfvNewMiddleName]")[0], true);
        $jQuery("[id$=rfvNewMiddleName]").hide();
        //ValidatorEnable($jQuery("[id$=revNewMiddleName]")[0], true);    ///commented this line
    }
}

function HideShow(checkBox) {
    var aliasBox = $jQuery("[id$=divPersonalAlias]");
    if (aliasBox[0] != undefined) {
        if (checkBox.checked == true) {
            aliasBox[0].style.display = "block";
            $jQuery("[id$=rfvNewFirstName]")[0].style.display = "none";
            $jQuery("[id$=rfvNewLastName]")[0].style.display = "none"
            $jQuery("[id$=rfvNewMiddleName]")[0].style.display = "none"
        }
        else {
            aliasBox[0].style.display = "none";
            if ($jQuery("[id$=divErrorMessage]")[0] != undefined) {
                $jQuery("[id$=divErrorMessage]")[0].style.display = "none"
            }
            $find($jQuery("[id$=txtNewFirstName]")[0].id).clear();
            $find($jQuery("[id$=txtNewLastName]")[0].id).clear();
            $find($jQuery("[id$=txtNewMiddleName]")[0].id).clear();
        }
    }
}

function CheckUncheck() {
    var checkBox = $jQuery(".chkShowHideAlias");
    checkBox.click();
}

function AliasMiddleNameEnableDisableForRepeater(ID) {
    //var txtMiddleName = $jQuery("[id$=" + ID.id + "]").closest('.sxro').next().find(".mddName").find(".helloAni");
    var hdnIFYOUDONTHAVEMIDDLENAME = $jQuery("[id$=hdnIFYOUDONTHAVEMIDDLENAME]")[0].value;
    var txtMiddleName = $jQuery("[id$=" + ID.id + "]").parentsUntil("[id$=divMiddleNameCheckBoxRepeater]").prev('.aliasDiv').find(".mddName").find(".helloAni");
    //var rfvValidator = $jQuery("[id$=" + ID.id + "]").closest('.sxro').next().find(".vlMiddelName").find(".errmsg");
    var rfvValidator = $jQuery("[id$=" + ID.id + "]").parentsUntil("[id$=divMiddleNameCheckBoxRepeater]").prev('.aliasDiv').find(".vlMiddelName").find(".errmsg");
    if (ID.checked) {
        //UAT_2169:Send Middle Name and Email address to clearstar in Complio
        var noMiddleNameText = $jQuery("[id$=hdnNoMiddleNameText]")[0].value;
        $find(txtMiddleName[0].id).set_value();
        $find(txtMiddleName[0].id)._element.setAttribute("Placeholder", "");
        $find(txtMiddleName[0].id)._element.setAttribute("title", "");
        $find(txtMiddleName[0].id).disable();
        ValidatorEnable(rfvValidator[0], false);
    }
    else {
        if (txtMiddleName[0] != undefined) {
            $find(txtMiddleName[0].id).set_value('');
            $find(txtMiddleName[0].id)._element.setAttribute("Placeholder", hdnIFYOUDONTHAVEMIDDLENAME);
            $find(txtMiddleName[0].id)._element.setAttribute("title", hdnIFYOUDONTHAVEMIDDLENAME);
            $find(txtMiddleName[0].id).enable();
        }
        ValidatorEnable(rfvValidator[0], true);
        rfvValidator.hide();
    }
}

//check if required here
function EnableValidator(id) {
    if ($jQuery('#' + id)[0] != undefined) {
        ValidatorEnable($jQuery('#' + id)[0], true);
        $jQuery('#' + id).hide();
    }
}
