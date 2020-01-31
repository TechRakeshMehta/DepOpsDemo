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
//UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
function AliasMiddleNameEnableDisableForPortfolioRepeater(ID) {
    var IsLocationTenant = $jQuery("[id$=hdnIsPersonAliasLocationTenant]")[0].value;
    var hdnIFYOUDONTHAVEMIDDLENAME = $jQuery("[id$=hdnIFYOUDONTHAVEMIDDLENAME]")[0].value;
    //var txtMiddleName = $jQuery("[id$=" + ID.id + "]").closest('.col-md-12').next().find(".mddName").find(".form-control");
    var txtMiddleName = $jQuery("[id$=" + ID.id + "]").parentsUntil("[id$=divMiddleNameCheckBoxRepeater]").prev('.aliasDiv').find(".mddName").find(".form-control");
    //var rfvValidator = $jQuery("[id$=" + ID.id + "]").closest('.col-md-12').next().find(".vlMiddelName").find(".errmsg");
    var rfvValidator = $jQuery("[id$=" + ID.id + "]").parentsUntil("[id$=divMiddleNameCheckBoxRepeater]").prev('.aliasDiv').find(".vlMiddelName").find(".errmsg");

    if (ID.checked) {
        //UAT_2169:Send Middle Name and Email address to clearstar in Complio
        var noMiddleNameText = $jQuery("[id$=hdnNoMiddleNameText]")[0].value;
        var IsLocationTenant = $jQuery("[id$=hdnIsLocationTenant]")[0].value;
        if (IsLocationTenant.toLowerCase() == "true") {
            $find(txtMiddleName[0].id).set_value();

            $find(txtMiddleName[0].id)._element.setAttribute("Placeholder", "");
            $find(txtMiddleName[0].id)._element.setAttribute("title", "");

        }
        else {
            $find(txtMiddleName[0].id).set_value(noMiddleNameText);
        }
        $find(txtMiddleName[0].id).disable();
        ValidatorEnable(rfvValidator[0], false);
        //$jQuery('[id$=spnMiddleName]').hide();
    }
    else {
        $find(txtMiddleName[0].id).set_value('');

        $find(txtMiddleName[0].id)._element.setAttribute("Placeholder", hdnIFYOUDONTHAVEMIDDLENAME);
        $find(txtMiddleName[0].id)._element.setAttribute("title", hdnIFYOUDONTHAVEMIDDLENAME);

        $find(txtMiddleName[0].id).enable();
        ValidatorEnable(rfvValidator[0], true);
        rfvValidator.hide();
        //$jQuery('[id$=spnMiddleName]').show();
    }
}


// this function create by Joginder singh
//function NewPersonAliasInfoValidateVerifyAlias(ID) {

//   alert('ha');
//    var IsLocationTenant = $jQuery("[id$=hdnIsPersonAliasLocationTenant]")[0].value;

//    if (IsLocationTenant.toLowerCase() == "true") {
//        if ($jQuery("[id$=chkMiddleNameRequired]")[1].checked) {
//            ValidatorEnable($jQuery("[id$=rfvNewMiddleName]")[0], false);
//        }
//    }

//}

function AliasMiddleNameEnableDisableForPortfolio(ID) {

  var IsLocationTenant = $jQuery("[id$=hdnIsPersonAliasLocationTenant]")[0].value;
  var hdnIFYOUDONTHAVEMIDDLENAME = $jQuery("[id$=hdnIFYOUDONTHAVEMIDDLENAME]")[0].value;
  var middlename=  $find($jQuery("[id$=txtNewMiddleName]")[0].id);
    if (ID.checked) {
        //UAT_2169:Send Middle Name and Email address to clearstar in Complio
        var noMiddleNameText = $jQuery("[id$=hdnNoMiddleNameText]")[0].value;
        if (IsLocationTenant.toLowerCase() == "true") {
            $find($jQuery("[id$=txtNewMiddleName]")[0].id).set_value();

            middlename._element.setAttribute("Placeholder", "");
            middlename._element.setAttribute("title", "")

            $find($jQuery("[id$=txtNewMiddleName]")[0].id).disable();
            ValidatorEnable($jQuery("[id$=rfvNewMiddleName]")[0], false);
            ValidatorEnable($jQuery("[id$=revNewMiddleName]")[0], false);
        }
        else {
            $find($jQuery("[id$=txtNewMiddleName]")[0].id).set_value(noMiddleNameText);
            $find($jQuery("[id$=txtNewMiddleName]")[0].id).disable();
            ValidatorEnable($jQuery("[id$=rfvNewMiddleName]")[0], false);
        }
        //$jQuery('[id$=spnMiddleName]').hide();
    }
    else {

        if (IsLocationTenant.toLowerCase() == "true") {
            ValidatorEnable($jQuery("[id$=revNewMiddleName]")[0], true);
            $jQuery("[id$=revNewMiddleName]").hide();

        }
        $find($jQuery("[id$=txtNewMiddleName]")[0].id).set_value('');

        middlename._element.setAttribute("Placeholder", hdnIFYOUDONTHAVEMIDDLENAME);
        middlename._element.setAttribute("title", hdnIFYOUDONTHAVEMIDDLENAME);

        $find($jQuery("[id$=txtNewMiddleName]")[0].id).enable();
        ValidatorEnable($jQuery("[id$=rfvNewMiddleName]")[0], true);
        $jQuery("[id$=rfvNewMiddleName]").hide();
       
        //$jQuery('[id$=spnMiddleName]').show();
    }
}

$page.add_pageLoad(function () { 
    $jQuery("[id$=lblfirstName_0]").attr('tabindex', '0');
    $jQuery("[id$=lblMiddleName_0]").attr('tabindex', '0');
    $jQuery("[id$=lblLastName_0]").attr('tabindex', '0');
    $jQuery("[id$=chkShowHideAlias]").attr('tabindex', '0');
});
