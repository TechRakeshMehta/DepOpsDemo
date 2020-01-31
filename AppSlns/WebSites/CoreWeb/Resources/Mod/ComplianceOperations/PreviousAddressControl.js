var winopen = false;
var minDate = new Date("01/01/1980");

function CorrectFrmToPrevResDate(picker) {
    var date1 = $jQuery("[id$=dpResFrm]")[0].control.get_selectedDate();
    var date2 = $jQuery("[id$=dpResTill]")[0].control.get_selectedDate();
    if (date1 != null && date2 != null) {
        if (date1 > date2)
            $jQuery("[id$=dpResTill]")[0].control.set_selectedDate(null);
    }
}

function SetMinDatePrevRes(picker) {
    var date = $jQuery("[id$=dpResFrm]")[0].control.get_selectedDate();
    if (date != null) {
        picker.set_minDate(date);
    }
    else {
        picker.set_minDate(minDate);
    }
}

function HideShowResidence(checkBox) {
    var aliasBox = $jQuery("[id$=divResidentialShowHide]");
    if (aliasBox[0] != undefined) {
        if (checkBox.checked == true) {
            aliasBox[0].style.display = "block";
        }
        else {
            $jQuery("[id$=grdResidentialHistory]").find("[id$=fsucCmdBarPrevRes_btnCancel]").trigger("click");
            aliasBox[0].style.display = "none";
        }
    }
}


function CheckUncheckResidentialHistory() {
    var checkBox = $jQuery(".chkShowHideResidence");
    checkBox.click();

}

$page.add_pageLoad(function () {
    if ($jQuery('[id$=hdnControlToSetFocus]').val() != "") {
        $jQuery('[id$=' + $jQuery('[id$=hdnControlToSetFocus]').val() + ']').attr("tabindex", 0).focus();
        $jQuery('[id$=hdnControlToSetFocus]').val("");
    }

    $jQuery("[id$=grdResidentialHistory]").find("th").each(function (element) {
        if ($jQuery(this).text() != "" && $jQuery(this).text() != undefined && $jQuery(this).text().length > 1) {
            $jQuery(this).attr("tabindex", "0");
            //this.tabIndex = 0;
        }
    });
});

//UAT-2448
function openPopUp() {

    var popupWindowName = "Country Identification Details";
    winopen = true;
    var popupHeight = $jQuery(window).height() * (100 / 100);
    var url = $page.url.create("~/BkgOperations/Pages/CountryIdentificationDetails.aspx");
    var win = $window.createPopup(url, { size: "500," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: popupWindowName, onclose: OnClientClose });
    return false;
}

function OnClientClose(oWnd, args) {

    oWnd.remove_close(OnClientClose);
    if (winopen) {
        var arg = args.get_argument();
        if (arg) {
        }
        winopen = false;
    }
}


